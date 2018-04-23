using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlTemplateGallerySample
{
    public class ReportDesignToolEx : ReportDesignTool
    {
        IControlTemplateStorage storage;
        List<ControlTemplateUserControl> storageChangedListeners;        

        public ReportDesignToolEx(XtraReport report) : base(report) 
        {
            storageChangedListeners = new List<ControlTemplateUserControl>();
        }

        public ReportDesignToolEx(XtraReport report, IControlTemplateStorage templateStorage) : this(report) 
        {
            this.storage = templateStorage;
        }

        protected override IDesignForm CreateDesignForm()
        {
            XRDesignForm form = new XRDesignForm();
            InitializeForm(form);
            return form;
        }

        protected override IDesignForm CreateDesignRibbonForm()
        {
            XRDesignRibbonForm form = new XRDesignRibbonForm();
            InitializeForm(form);
            return form;            
        }

        public XRControl[] GetTemplateControls(string categoryName, string templateName)
        {
            return ControlTemplateStorage.GetTemplateControls(Storage, categoryName, templateName);
        }

        private void InitializeForm(IDesignForm designForm)
        {
            XRDesignDockManager dockManager = designForm.DesignDockManager;
            if (dockManager != null)
            {
                DockPanel dockPanel = new DockPanel();
                dockPanel.Text = "Control Templates";
                dockManager.AddPanel(DockingStyle.Right, dockPanel);
                InitializePanelContent(designForm.DesignMdiController, dockPanel);

                DockPanel reportExplorer = dockManager[DesignDockPanelType.ReportExplorer];

                dockPanel.DockAsTab(reportExplorer);
                dockManager.ActivePanel = reportExplorer;                
            }
        }

        protected virtual void InitializePanelContent(XRDesignMdiController controller, DockPanel panel)
        {                        
            ControlTemplateUserControl userControl = new ControlTemplateUserControl(controller);
            storageChangedListeners.Add(userControl);
            panel.Controls.Add(userControl);
            userControl.Storage = Storage;
            userControl.Dock = System.Windows.Forms.DockStyle.Fill;

            controller.DesignPanelListeners.Add(userControl);
        }

        public override void Dispose()
        {
            storage = null;

            if (storageChangedListeners != null)
            {
                for (int i = 0; i < storageChangedListeners.Count; i++)
                    storageChangedListeners[i].Dispose();

                storageChangedListeners.Clear();
                storageChangedListeners = null;
            }

            base.Dispose();
        }

        public IControlTemplateStorage Storage
        {
            get
            {
                if (storage == null)
                    storage = new ControlTemplateStorage();
                return storage;
            }

            set
            {
                if (storage != value)
                {
                    storage = value;

                    for (int i = 0; i < storageChangedListeners.Count; i++)
                        storageChangedListeners[i].Storage = storage;
                }
            }
        }
    }
}
