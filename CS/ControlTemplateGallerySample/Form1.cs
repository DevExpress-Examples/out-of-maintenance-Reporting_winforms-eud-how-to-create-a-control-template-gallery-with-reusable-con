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
using DevExpress.XtraBars.Docking;
using System.ComponentModel.Design;

namespace ControlTemplateGallerySample {
    public partial class Form1 : XtraForm {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            Control control = CreateTemplateControl(reportDesigner1);
            templateDockPanel1_Container.Controls.Add(control);

            reportDesigner1.DesignPanelLoaded += reportDesigner1_DesignPanelLoaded;
            reportDesigner1.OpenReport(new XtraReport1());
        }

        void reportDesigner1_DesignPanelLoaded(object sender, DesignerLoadedEventArgs e) {
            reportDesigner1.DesignPanelLoaded -= reportDesigner1_DesignPanelLoaded;

            //Set initial selection in designer for demonstration purposes 
            BeginInvoke(new MethodInvoker(() => {
                XRDesignPanel panel = (XRDesignPanel)sender;
                XtraReport1 rep = (XtraReport1)panel.Report;
                ISelectionService serv = panel.GetService(typeof(ISelectionService)) as ISelectionService;
                serv.SetSelectedComponents(new object[] { rep.xrRichText1, rep.xrRichText2 }, SelectionTypes.Auto);
            }));
        }

        Control CreateTemplateControl(XRDesignMdiController controller) {
            TemplateUserControl control = new TemplateUserControl(controller) { Dock = System.Windows.Forms.DockStyle.Fill };

            control.Storage = new PersistentControlTemplateStorage("SampleStorage.xml"); // Persistent storage
            //userControl.Storage = new ControlTemplateStorage(); // In-memory storage

            controller.DesignPanelListeners.Add(control);
            return control;
        }
    }
}
