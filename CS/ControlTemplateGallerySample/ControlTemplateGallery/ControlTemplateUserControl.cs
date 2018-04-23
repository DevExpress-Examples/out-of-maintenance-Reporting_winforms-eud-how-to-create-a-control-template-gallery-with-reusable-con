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

namespace ControlTemplateGallerySample
{
    public partial class ControlTemplateUserControl : DevExpress.XtraEditors.XtraUserControl, IDesignPanelListener
    {
        private XRDesignMdiController controller;
        private IControlTemplateStorage templateStorage;
        private XRDesignPanel panel;

        protected ControlTemplateUserControl()
        {
            InitializeComponent();
            tlTemplates.BeforeDragNode += OnBeforeDragNode;
            tlTemplates.DragOver += tlTemplates_OnDragOver;
            tlTemplates.GetStateImage += OnGetStateImage;
            tlTemplates.FocusedNodeChanged += OnFocusedNodeChanged;
            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
        }        

        void btnDelete_Click(object sender, EventArgs e)
        {
            TreeListNode node = tlTemplates.FocusedNode;

            if (node.Level == 0)
            {
                string categoryName = Convert.ToString(node.GetValue(0));
                templateStorage.DeleteCategory(categoryName);
            }
            else
            {
                string templateName = Convert.ToString(node.GetValue(0));
                string categoryName = Convert.ToString(node.ParentNode.GetValue(0));

                templateStorage.DeleteTemplate(categoryName, templateName);
            }

            InitializeStorage();
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            List<XRControl> selectedControls = GetSelectedControls();
            if (selectedControls.Count > 0)
            {
                EditTemplateForm templateForm = new EditTemplateForm();
                if (templateForm.ShowDialog() == DialogResult.OK)
                {
                    string catName = templateForm.CategoryName;
                    string templateName = templateForm.TemplateName;
                    byte[] layout = GenerateLayoutBytes(selectedControls.ToArray());

                    templateStorage.SetData(catName, templateName, layout);

                    InitializeStorage();
                }
            }
        }

        public ControlTemplateUserControl(XRDesignMdiController mdiController)
            : this()
        {
            this.Controller = mdiController;
        }        

        private List<XRControl> GetSelectedControls()
        {
            object[] selectedComponents = panel.GetSelectedComponents();

            List<XRControl> selectedControls = new List<XRControl>();

            for (int i = 0; i < selectedComponents.Length; i++)
                if (selectedComponents[i] is XRControl && !(selectedComponents[i] is Band))
                    selectedControls.Add(selectedComponents[i] as XRControl);

            return selectedControls;
        }

        private void InitializeStorage()
        {
            tlTemplates.Nodes.Clear();
            if (templateStorage != null)
            {
                string[] categories = templateStorage.GetCategoryNames();
                for (int i = 0; i < categories.Length; i++)
                {
                    TreeListNode node = tlTemplates.Nodes.Add(new object[] { categories[i] });

                    string[] templates = templateStorage.GetTemplateNamesForCategory(categories[i]);
                    for (int j = 0; j < templates.Length; j++)
                        node.Nodes.Add(new object[] { templates[j] });
                }
            }

            tlTemplates.ExpandAll();
        }

        void OnBeforeDragNode(object sender, DevExpress.XtraTreeList.BeforeDragNodeEventArgs e)
        {
            e.CanDrag = e.Node.Level > 0;
        }

        private void tlTemplates_OnDragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
        }

        void OnDesignPanelLoaded(object sender, DesignerLoadedEventArgs e)
        {
            e.DesignerHost.RemoveService(typeof(IDragDropService));
            e.DesignerHost.AddService(typeof(IDragDropService), new ControlTemplateDragDropService(e.DesignerHost, templateStorage));
        }

        private void OnGetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            e.NodeImageIndex = e.Node.Level;
        }

        private void OnFocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            btnDelete.Enabled = e.Node != null;
        }        

        private byte[] GenerateLayoutBytes(object[] controls)
        {
            IBandViewInfoService serv = panel.GetService(typeof(IBandViewInfoService)) as IBandViewInfoService;
            List<PointF> locations = new List<PointF>();

            //In pixels
            RectangleF rect = serv.GetControlScreenBounds((XRControl)controls[0]);
            locations.Add(new PointF(rect.Left, rect.Top));
            int topMost = (int)rect.Top;
            int leftMost = (int)rect.Left;

            for (int i = 1; i < controls.Length; i++)
            {
                XRControl control = controls[i] as XRControl;
                if (!(control is Band))
                {
                    rect = serv.GetControlScreenBounds(control);
                    locations.Add(new PointF(rect.Left, rect.Top));
                    if (rect.Top < topMost)
                        topMost = (int)rect.Top;
                    if (rect.Left < leftMost)
                        leftMost = (int)rect.Left;
                }
            }

            XtraReport tempReport = new XtraReport();

            using (MemoryStream stream = new MemoryStream())
            {
                panel.Report.SaveLayoutToXml(stream);
                stream.Position = 0;

                tempReport.LoadLayoutFromXml(stream);
            }

            ArrayList clones = new ArrayList();

            for (int i = 0; i < controls.Length; i++)
            {
                XRControl control = controls[i] as XRControl;
                XRControl clone = tempReport.FindControl(control.Name, true);
                clones.Add(clone);
            }

            tempReport.Bands.Clear();

            DetailBand tempBand = new DetailBand();
            tempReport.Bands.Add(tempBand);

            for (int i = 0; i < clones.Count; i++)
            {
                XRControl newControl = clones[i] as XRControl;
                tempBand.Controls.Add(newControl);

                PointF originalLocation = locations[i];
                PointF newLocation = new PointF(originalLocation.X - leftMost, originalLocation.Y - topMost);

                newLocation = GraphicsUnitConverter.Convert(newLocation, GraphicsDpi.Pixel, panel.Report.Dpi);

                newControl.LocationF = newLocation;
            }

            byte[] layoutBytes = null;

            using (MemoryStream stream = new MemoryStream())
            {
                tempReport.SaveLayoutToXml(stream);
                layoutBytes = stream.ToArray();
            }

            tempReport.Dispose();

            return layoutBytes;
        }

        public XRDesignMdiController Controller
        {
            get { return controller; }
            set
            {
                if (controller != value)
                {
                    if (controller != null)
                        controller.DesignPanelLoaded -= OnDesignPanelLoaded;

                    controller = value;

                    if (controller != null)
                        controller.DesignPanelLoaded += OnDesignPanelLoaded;
                }
            }
        }

        public IControlTemplateStorage Storage
        {
            get { return templateStorage; }
            set
            {
                if (templateStorage != value)
                {
                    templateStorage = value;
                    InitializeStorage();
                }
            }
        }

        public XRDesignPanel XRDesignPanel
        {
            get
            {
                return panel;
            }
            set
            {
                if (panel != value)
                {
                    if (panel != null)
                        panel.SelectionChanged -= OnSelectionChanged;

                    panel = value;

                    if (panel != null)
                        panel.SelectionChanged += OnSelectionChanged;

                    UpdateButtonState();
                }
            }
        }

        void OnSelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            btnAdd.Enabled = panel != null && GetSelectedControls().Count > 0;
        }        
    }
}
