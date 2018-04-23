using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ControlTemplateGallerySample
{
    public interface IControlTemplateStorage
    {
        string[] GetCategoryNames();
        string[] GetTemplateNamesForCategory(string categoryName);
        byte[] GetData(string categoryName, string templateName);
        void SetData(string categoryName, string templateName, byte[] templateLayout);
        void DeleteTemplate(string categoryName, string templateName);
        void DeleteCategory(string categoryName);
    }

    //Default in-memory storage
    public class ControlTemplateStorage : IControlTemplateStorage
    {
        List<TemplateLayoutItem> templates;

        public ControlTemplateStorage()
        {
            templates = new List<TemplateLayoutItem>();            
        }

        internal static XRControl[] GetTemplateControls(IControlTemplateStorage storage, string categoryName, string templateName)
        {
            byte[] layoutBytes = storage.GetData(categoryName, templateName);

            if (layoutBytes != null && layoutBytes.Length > 0)
            {
                XtraReport tempReport = new XtraReport();
                using (MemoryStream stream = new MemoryStream(layoutBytes))
                {
                    tempReport.LoadLayoutFromXml(stream);
                }

                return tempReport.Bands[BandKind.Detail].Controls.Cast<XRControl>().ToArray();
            }

            return null;
        }

        public void DeleteTemplate(string categoryName, string templateName)
        {
            TemplateLayoutItem item = templates.Where(x => x.Category == categoryName && x.Name == templateName).FirstOrDefault();
            templates.Remove(item);
        }

        public void DeleteCategory(string categoryName)
        {
            IEnumerable<TemplateLayoutItem> items = templates.Where(x => x.Category == categoryName);
            
            foreach (TemplateLayoutItem item in items)
                templates.Remove(item);
        }

        public string[] GetCategoryNames()
        {
            return templates.Select(x => x.Category).Distinct().ToArray();
        }

        public string[] GetTemplateNamesForCategory(string categoryName)
        {
            return templates.Where(x => x.Category == categoryName).Select(y => y.Name).ToArray();
        }

        public byte[] GetData(string categoryName, string templateName)
        {
            TemplateLayoutItem item = templates.Where(x => x.Category == categoryName && x.Name == templateName).FirstOrDefault();
            return item == null ? null : item.LayoutBytes;
        }

        public void SetData(string categoryName, string templateName, byte[] templateLayout)
        {
            TemplateLayoutItem item = templates.Where(x => x.Category == categoryName && x.Name == templateName).FirstOrDefault();
            if (item == null)
                templates.Add(new TemplateLayoutItem() { Name = templateName, Category = categoryName, LayoutBytes = templateLayout });
        }        

        class TemplateLayoutItem 
        {
            public string Category { get; set; }
            public string Name { get; set; }
            public byte[] LayoutBytes { get; set; }
        }        
    }
}
