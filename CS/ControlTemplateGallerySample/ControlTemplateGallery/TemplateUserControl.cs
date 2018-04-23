using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraReports.Design;
using System.ComponentModel.Design;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraReports.UI;
using System.ComponentModel.Design.Serialization;
using DevExpress.XtraReports.UserDesigner.Native;
using System.Collections;
using DevExpress.XtraPrinting;
using System.IO;

namespace ControlTemplateGallerySample {
    public partial class TemplateUserControl : DevExpress.XtraEditors.XtraUserControl, IDesignPanelListener {
        private XRDesignMdiController controller;
        private IControlTemplateStorage templateStorage;
        private XRDesignPanel panel;

        protected TemplateUserControl() {
            InitializeComponent();
            tlTemplates.BeforeDragNode += OnBeforeDragNode;
            tlTemplates.DragOver += tlTemplates_OnDragOver;
            tlTemplates.GetStateImage += OnGetStateImage;
            tlTemplates.FocusedNodeChanged += OnFocusedNodeChanged;
            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
        }

        void btnDelete_Click(object sender, EventArgs e) {
            TreeListNode node = tlTemplates.FocusedNode;

            if(node.Level == 0 && ShowMessage("Are you sure you want to delete this category?", "Confirm Deleting") == DialogResult.Yes) {
                string categoryName = Convert.ToString(node.GetValue(0));
                templateStorage.DeleteCategory(categoryName);
            } else if(ShowMessage("Are you sure you want to delete this template?", "Confirm Deleting") == DialogResult.Yes) {
                string templateName = Convert.ToString(node.GetValue(0));
                string categoryName = Convert.ToString(node.ParentNode.GetValue(0));
                templateStorage.DeleteTemplate(categoryName, templateName);
            }
            RebuildNodes();
        }

        DialogResult ShowMessage(string text, string caption) {
            return XtraMessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
        }

        void btnAdd_Click(object sender, EventArgs e) {
            List<XRControl> selectedControls = GetSelectedControls();
            if(selectedControls.Count == 0) return;

            TemplateEditorForm templateForm = new TemplateEditorForm();
            if(templateForm.ShowDialog(FindForm()) == DialogResult.OK) {

                string catName = templateForm.CategoryName;
                string templateName = templateForm.TemplateName;

                templateStorage.Store(catName, templateName, selectedControls.ToArray());
                RebuildNodes();
            }
        }

        public TemplateUserControl(XRDesignMdiController mdiController)
            : this() {
            this.Controller = mdiController;
        }

        List<XRControl> GetSelectedControls() {
            ISelectionService serv = panel.GetService(typeof(ISelectionService)) as ISelectionService;

            IEnumerable<XRControl> selectedComponents = panel.GetSelectedComponents()
                .Where<object>(item => item is XRControl && !(item is Band) && !(item is XRCrossBandControl))
                .Cast<XRControl>();

            List<XRControl> selectedControls = new List<XRControl>();
            if(selectedComponents.Count<XRControl>() == 0) return selectedControls;

            XRControl primaryControl = serv.PrimarySelection as XRControl ?? selectedComponents.ElementAt<XRControl>(0);

            foreach(XRControl item in selectedComponents) {
                if(ReferenceEquals(item.Band, primaryControl.Band))
                    selectedControls.Add(item as XRControl);
            }
            return selectedControls;
        }

        private void RebuildNodes() {
            tlTemplates.Nodes.Clear();
            if(templateStorage != null) {
                string[] categories = templateStorage.GetCategoryNames();
                foreach(string category in categories) {
                    TreeListNode node = tlTemplates.Nodes.Add(new object[] { category });

                    string[] templates = templateStorage.GetTemplateNamesForCategory(category);
                    foreach(string template in templates)
                        node.Nodes.Add(new object[] { template });
                }
            }
            tlTemplates.ExpandAll();
        }

        void OnBeforeDragNode(object sender, DevExpress.XtraTreeList.BeforeDragNodeEventArgs e) {
            e.CanDrag = e.Node.Level > 0;
        }

        private void tlTemplates_OnDragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.None;
        }

        void OnDesignPanelLoaded(object sender, DesignerLoadedEventArgs e) {
            e.DesignerHost.RemoveService(typeof(IDragDropService));
            e.DesignerHost.AddService(typeof(IDragDropService), new MyDragDropService(e.DesignerHost, templateStorage));
        }

        private void OnGetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e) {
            e.NodeImageIndex = e.Node.Level;
        }

        private void OnFocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e) {
            btnDelete.Enabled = e.Node != null;
        }

        public XRDesignMdiController Controller {
            get { return controller; }
            set {
                if(controller != value) {
                    if(controller != null)
                        controller.DesignPanelLoaded -= OnDesignPanelLoaded;

                    controller = value;

                    if(controller != null)
                        controller.DesignPanelLoaded += OnDesignPanelLoaded;
                }
            }
        }

        public IControlTemplateStorage Storage {
            get { return templateStorage; }
            set {
                if(templateStorage != value) {
                    templateStorage = value;
                    RebuildNodes();
                }
            }
        }

        public XRDesignPanel XRDesignPanel {
            get {
                return panel;
            }
            set {
                if(panel != value) {
                    if(panel != null)
                        panel.SelectionChanged -= OnSelectionChanged;

                    panel = value;

                    if(panel != null)
                        panel.SelectionChanged += OnSelectionChanged;

                    UpdateButtonState();
                }
            }
        }

        void OnSelectionChanged(object sender, EventArgs e) {
            UpdateButtonState();
        }

        private void UpdateButtonState() {
            btnAdd.Enabled = panel != null && GetSelectedControls().Count > 0;
        }
    }
}
