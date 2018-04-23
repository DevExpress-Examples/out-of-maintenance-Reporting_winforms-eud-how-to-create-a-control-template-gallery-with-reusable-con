using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using System.Drawing;

namespace ControlTemplateGallerySample
{
    public interface IControlTemplateStorage
    {
        string[] GetCategoryNames();
        string[] GetTemplateNamesForCategory(string categoryName);
        XRControl[] Restore(string categoryName, string templateName);
        void Store(string categoryName, string templateName, XRControl[] controls);
        void DeleteTemplate(string categoryName, string templateName);
        void DeleteCategory(string categoryName);
    }

    class ControlTemplateStorageBase {
        protected static XRControl[] DeserializeControls(byte[] bytes) {
            if(bytes != null && bytes.Length > 0) {
                XtraReport tempReport = new XtraReport();
                using(MemoryStream stream = new MemoryStream(bytes)) {
                    tempReport.LoadLayoutFromXml(stream);
                }
                return tempReport.Bands[BandKind.Detail].Controls.Cast<XRControl>().ToArray();
            }
            return new XRControl[0];
        }
        protected static byte[] SerializeControls(XRControl[] controls) {
            XtraReport tempReport = new XtraReport();
            DetailBand tempBand = new DetailBand();
            tempReport.Bands.Add(tempBand);

            using(XtraReport cloneReport = new XtraReport()) {
                using(MemoryStream stream = new MemoryStream()) {
                    controls[0].RootReport.SaveLayoutToXml(stream);
                    stream.Position = 0;

                    cloneReport.LoadLayoutFromXml(stream);
                }
                foreach(XRControl control in controls) {
                    XRControl clone = cloneReport.FindControl(control.Name, true);
                    if(clone != null) tempBand.Controls.Add(clone);
                }
            }

            float topMost = float.MaxValue;
            float leftMost = float.MaxValue;

            foreach(XRControl control in tempBand.Controls) {
                topMost = Math.Min(topMost, control.TopF);
                leftMost = Math.Min(leftMost, control.LeftF);
            }

            foreach(XRControl control in tempBand.Controls) {
                control.LocationF = new PointF(control.LeftF - leftMost, control.TopF - topMost);
            }

            byte[] layoutBytes = null;

            using(MemoryStream stream = new MemoryStream()) {
                tempReport.SaveLayoutToXml(stream);
                layoutBytes = stream.ToArray();
            }

            tempReport.Dispose();
            return layoutBytes;
        }
    }

    // Persistent storage
    class PersistentControlTemplateStorage : ControlTemplateStorageBase, IControlTemplateStorage {
        DataSet ds;

        public PersistentControlTemplateStorage(string path) {
            ds = new DataSet();
            if(File.Exists(path))
                ds.ReadXml(path);
            else {
                DataTable categories = new DataTable("Categories");
                ds.Tables.Add(categories);

                categories.Columns.Add("CategoryID", typeof(int)).AutoIncrement = true;
                categories.Columns.Add("CategoryName", typeof(string));

                DataTable templates = new DataTable("Templates");
                ds.Tables.Add(templates);

                templates.Columns.Add("CategoryID", typeof(int));
                templates.Columns.Add("TemplateName", typeof(string));
                templates.Columns.Add("Layout", typeof(byte[]));

                ds.Relations.Add("CategoryTemplates", categories.Columns["CategoryID"], templates.Columns["CategoryID"]);

                ds.WriteXml("SampleStorage.xml", XmlWriteMode.WriteSchema);
            }
        }

       public void DeleteTemplate(string categoryName, string templateName) {
            DataRow row = ds.Tables["Categories"].AsEnumerable().Where(x => x.Field<string>("CategoryName") == categoryName).FirstOrDefault();

            int catId = row.Field<int>("CategoryID");
            DataRow templateRow = ds.Tables["Templates"].AsEnumerable().Where(x => x.Field<int>("CategoryID") == catId && x.Field<string>("TemplateName") == templateName).FirstOrDefault();
            ds.Tables["Templates"].Rows.Remove(templateRow);

            ds.WriteXml("SampleStorage.xml", XmlWriteMode.WriteSchema);
        }

       public void DeleteCategory(string categoryName) {
            DataRow row = ds.Tables["Categories"].AsEnumerable().Where(x => x.Field<string>("CategoryName") == categoryName).FirstOrDefault();

            DataRow[] templateRows = row.GetChildRows("CategoryTempaltes");
            for(int i = 0; i < templateRows.Length; i++)
                ds.Tables["Templates"].Rows.Remove(templateRows[i]);

            ds.Tables["Categories"].Rows.Remove(row);

            ds.WriteXml("SampleStorage.xml", XmlWriteMode.WriteSchema);
        }

       public string[] GetCategoryNames() {
            return ds.Tables["Categories"].AsEnumerable().Select(r => r.Field<string>("CategoryName")).ToArray();
        }

       public string[] GetTemplateNamesForCategory(string categoryName) {
            DataRow row = ds.Tables["Categories"].AsEnumerable().Where(x => x.Field<string>("CategoryName") == categoryName).FirstOrDefault();
            if(row != null)
                return row.GetChildRows("CategoryTemplates").Select(r => r.Field<string>("TemplateName")).ToArray();

            return null;
        }

       public XRControl[] Restore(string categoryName, string templateName) {
            DataRow row = ds.Tables["Categories"].AsEnumerable().Where(x => x.Field<string>("CategoryName") == categoryName).FirstOrDefault();
            if(row != null) {
                DataRow childRow = row.GetChildRows("CategoryTemplates").Where(x => x.Field<string>("TemplateName") == templateName).FirstOrDefault();
                byte[] layout = childRow != null ? childRow.Field<byte[]>("Layout") : new byte[0];
                return DeserializeControls(layout);
            }
            return null;
        }

       public void Store(string categoryName, string templateName, XRControl[] controls) {
           byte[] templateLayout = SerializeControls(controls);

            DataRow row = ds.Tables["Categories"].AsEnumerable().Where(x => x.Field<string>("CategoryName") == categoryName).FirstOrDefault();
            if(row == null) {
                row = ds.Tables["Categories"].NewRow();
                row["CategoryName"] = categoryName;
                ds.Tables["Categories"].Rows.Add(row);
            }

            int catId = row.Field<int>("CategoryID");
            DataRow templateRow = ds.Tables["Templates"].AsEnumerable().Where(x => x.Field<int>("CategoryID") == catId && x.Field<string>("TemplateName") == templateName).FirstOrDefault();
            if(templateRow == null) {
                templateRow = ds.Tables["Templates"].NewRow();
                templateRow["CategoryID"] = catId;
                templateRow["TemplateName"] = templateName;
                ds.Tables["Templates"].Rows.Add(templateRow);
            }
            templateRow["Layout"] = templateLayout;

            ds.WriteXml("SampleStorage.xml", XmlWriteMode.WriteSchema);
        }
    }

    // In-memory storage
    class ControlTemplateStorage : ControlTemplateStorageBase, IControlTemplateStorage {
        class TemplateLayoutItem {
            public string Category { get; set; }
            public string Name { get; set; }
            public byte[] LayoutBytes { get; set; }
        }

        List<TemplateLayoutItem> templates;

        public ControlTemplateStorage() {
            templates = new List<TemplateLayoutItem>();
        }

        public void DeleteTemplate(string categoryName, string templateName) {
            TemplateLayoutItem item = templates.Where(x => x.Category == categoryName && x.Name == templateName).FirstOrDefault();
            templates.Remove(item);
        }

        public void DeleteCategory(string categoryName) {
            IEnumerable<TemplateLayoutItem> items = templates.Where(x => x.Category == categoryName);

            foreach(TemplateLayoutItem item in items)
                templates.Remove(item);
        }

        public string[] GetCategoryNames() {
            return templates.Select(x => x.Category).Distinct().ToArray();
        }

        public string[] GetTemplateNamesForCategory(string categoryName) {
            return templates.Where(x => x.Category == categoryName).Select(y => y.Name).ToArray();
        }

        public XRControl[] Restore(string categoryName, string templateName) {
            TemplateLayoutItem item = templates.Where(x => x.Category == categoryName && x.Name == templateName).FirstOrDefault();
            byte[] layout = item != null ? item.LayoutBytes : new byte[0];
            return DeserializeControls(layout);
        }

        public void Store(string categoryName, string templateName, XRControl[] controls) {
            byte[] templateLayout = SerializeControls(controls);
            TemplateLayoutItem item = templates.Where(x => x.Category == categoryName && x.Name == templateName).FirstOrDefault();
            if(item == null)
                templates.Add(new TemplateLayoutItem() { Name = templateName, Category = categoryName, LayoutBytes = templateLayout });
        }
    }
}
