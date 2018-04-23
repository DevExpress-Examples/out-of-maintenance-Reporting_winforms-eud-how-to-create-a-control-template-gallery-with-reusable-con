using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlTemplateGallerySample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XtraReport1 report = new XtraReport1();
            using (ReportDesignToolEx designTool = new ReportDesignToolEx(report, new SampleControlTemplateStorage()))
            {                                
                designTool.ShowDesignerDialog();
            }            
        }
    }

    public class SampleControlTemplateStorage : IControlTemplateStorage
    {
        DataSet ds;

        public SampleControlTemplateStorage()
        {
            ds = new DataSet();
            if (File.Exists("SampleStorage.xml"))
                ds.ReadXml("SampleStorage.xml");
            else
            {
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

        public void DeleteTemplate(string categoryName, string templateName)
        {
            if (XtraMessageBox.Show("Are you sure you want to delete this template?", "Confirm Deleting", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataRow row = ds.Tables["Categories"].AsEnumerable().Where(x => x.Field<string>("CategoryName") == categoryName).FirstOrDefault();

                int catId = row.Field<int>("CategoryID");
                DataRow templateRow = ds.Tables["Templates"].AsEnumerable().Where(x => x.Field<int>("CategoryID") == catId && x.Field<string>("TemplateName") == templateName).FirstOrDefault();
                ds.Tables["Templates"].Rows.Remove(templateRow);

                ds.WriteXml("SampleStorage.xml", XmlWriteMode.WriteSchema);
            }
        }

        public void DeleteCategory(string categoryName)
        {

            if (XtraMessageBox.Show("Are you sure you want to delete this category?", "Confirm Deleting", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataRow row = ds.Tables["Categories"].AsEnumerable().Where(x => x.Field<string>("CategoryName") == categoryName).FirstOrDefault();

                DataRow[] templateRows = row.GetChildRows("CategoryTempaltes");
                for (int i = 0; i < templateRows.Length; i++)
                    ds.Tables["Templates"].Rows.Remove(templateRows[i]);

                ds.Tables["Categories"].Rows.Remove(row);

                ds.WriteXml("SampleStorage.xml", XmlWriteMode.WriteSchema);
            }
        }

        public string[] GetCategoryNames()
        {
            return ds.Tables["Categories"].AsEnumerable().Select(r => r.Field<string>("CategoryName")).ToArray();
        }

        public string[] GetTemplateNamesForCategory(string categoryName)
        {
            DataRow row = ds.Tables["Categories"].AsEnumerable().Where(x => x.Field<string>("CategoryName") == categoryName).FirstOrDefault();
            if (row != null)
                return row.GetChildRows("CategoryTemplates").Select(r => r.Field<string>("TemplateName")).ToArray();

            return null;
        }

        public byte[] GetData(string categoryName, string templateName)
        {
            DataRow row = ds.Tables["Categories"].AsEnumerable().Where(x => x.Field<string>("CategoryName") == categoryName).FirstOrDefault();
            if (row != null)
            {
                DataRow childRow = row.GetChildRows("CategoryTemplates").Where(x => x.Field<string>("TemplateName") == templateName).FirstOrDefault();
                return childRow == null ? null : childRow.Field<byte[]>("Layout");
            }
            return null;
        }

        public void SetData(string categoryName, string templateName, byte[] templateLayout)
        {
            DataRow row = ds.Tables["Categories"].AsEnumerable().Where(x => x.Field<string>("CategoryName") == categoryName).FirstOrDefault();
            if (row == null)
            {
                row = ds.Tables["Categories"].NewRow();
                row["CategoryName"] = categoryName;
                ds.Tables["Categories"].Rows.Add(row);
            }

            int catId = row.Field<int>("CategoryID");
            DataRow templateRow = ds.Tables["Templates"].AsEnumerable().Where(x => x.Field<int>("CategoryID") == catId && x.Field<string>("TemplateName") == templateName).FirstOrDefault();
            if (templateRow == null)
            {
                templateRow = ds.Tables["Templates"].NewRow();
                templateRow["CategoryID"] = catId;
                templateRow["TemplateName"] = templateName;
                ds.Tables["Templates"].Rows.Add(templateRow);
            }
            templateRow["Layout"] = templateLayout;

            ds.WriteXml("SampleStorage.xml", XmlWriteMode.WriteSchema);
        }
    }
}
