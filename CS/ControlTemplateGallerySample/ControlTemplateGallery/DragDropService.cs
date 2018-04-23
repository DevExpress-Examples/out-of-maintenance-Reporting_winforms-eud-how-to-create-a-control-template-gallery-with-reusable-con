using DevExpress.XtraPrinting;
using DevExpress.XtraReports.Design;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlTemplateGallerySample {
    class MyDragDropService : DragDropService, IDragDropService {
        IControlTemplateStorage templateStorage;

        public MyDragDropService(IDesignerHost designerHost, IControlTemplateStorage storage)
            : base(designerHost) {
            templateStorage = storage;
        }

        IDragHandler IDragDropService.GetDragHandler(System.Windows.Forms.IDataObject data) {
            TreeListNode node = GetNode(data);
            if(node != null && !data.GetDataPresent(typeof(DragDataObject))) {
                XRControl[] controls = GetTemplateControlsFromData(node);
                DragDataObject obj = CreateDragData(controls, ZoomService.GetInstance(DesignerHost));
                data.SetData(typeof(DragDataObject), obj);
                return new MyControlDragHandler(DesignerHost);
            }
            return base.GetDragHandler(data);
        }

        static TreeListNode GetNode(IDataObject data) {
            return data != null ? data.GetData(typeof(TreeListNode).FullName) as TreeListNode : null;
        }

        XRControl[] GetTemplateControlsFromData(TreeListNode node) {
            string templateName = Convert.ToString(node.GetValue(0));
            string categoryName = Convert.ToString(node.ParentNode.GetValue(0));
            return templateStorage.Restore(categoryName, templateName);
        }

        DragDataObject CreateDragData(XRControl[] controls, ZoomService zoomService) {
            controls = controls.OrderBy<XRControl, float>(item => item.BoundsF.X).ToArray<XRControl>();
            XRControl baseControl = controls[0];

            RectangleF[] controlRects = new RectangleF[controls.Length];
            RectangleF baseRect = baseControl.BoundsF;

            for(int i = 0; i < controls.Length; i++) {
                RectangleF rect = controls[i].BoundsF;
                rect.Offset(-baseRect.X, -baseRect.Y);
                controlRects[i] = zoomService.ToScaledPixels(rect, controls[i].Dpi);
            }
            return new DragDataObject(controls, baseControl, controlRects, PointF.Empty);
        }
    }

    class MyControlDragHandler : ControlDragHandler {
        public MyControlDragHandler(IDesignerHost host) : base(host) { 
        }
        public override void HandleDragDrop(object sender, DragEventArgs e) {
            DragDataObject data = e.Data.GetData(typeof(DragDataObject)) as DragDataObject;
            if(data != null) AddToContainerRecursive(data.Controls);
            base.HandleDragDrop(sender, e);
        }
        void AddToContainerRecursive(IList controls) {
            foreach(XRControl item in controls) {
                host.Container.Add(item);
                AddToContainerRecursive(item.Controls);
            }
        }
        protected override void UpdateDragEffect(DragEventArgs e) {
            e.Effect = DragDropEffects.Copy;
        }
    }
}
