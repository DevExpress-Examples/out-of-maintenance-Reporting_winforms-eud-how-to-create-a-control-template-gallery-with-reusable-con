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

namespace ControlTemplateGallerySample
{
    public class ControlTemplateDragDropService : DragDropService, IDragDropService
    {
        IControlTemplateStorage templateStorage;

        public ControlTemplateDragDropService(IDesignerHost designerHost, IControlTemplateStorage storage)
            : base(designerHost)
        {
            templateStorage = storage;
        }

        IDragHandler IDragDropService.GetDragHandler(System.Windows.Forms.IDataObject data)
        {
            IDragHandler handler = base.GetDragHandler(data);

            if (handler.GetType() == typeof(DragHandlerBase) && data != null && !data.GetDataPresent("CF_DSREF"))
            {
                handler = new CustomTemplateDragHandler(this.DesignerHost, templateStorage);
            }

            return handler;
        }
    }

    public class CustomTemplateDragHandler : DragHandlerBase
    {
        XRControl[] templateControls;
        IControlTemplateStorage templateStorage;

        public CustomTemplateDragHandler(IDesignerHost host, IControlTemplateStorage storage)
            : base(host)
        {
            templateStorage = storage;
        }

        public override void HandleDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        public override void HandleDragOver(object sender, DragEventArgs e)
        {
            base.HandleDragOver(sender, e);
            XRControl controlByScreenPoint = base.bandViewSvc.GetControlByScreenPoint((PointF)new Point(e.X, e.Y));
            if (controlByScreenPoint != null)
            {                
                XRControl control = controlByScreenPoint;

                if (!(control is Band))
                    control = control.Band;

                base.SelectComponent(base.host, control);

                e.Effect = DragDropEffects.Copy;

                PointF basePoint = this.EvalBasePoint(e);
                XRControl[] controls = GetTemplateControlsFromData(e.Data);

                List<RectangleF> dragRects = new List<RectangleF>();

                for (int i = 0; i < controls.Length; i++) 
                {
                    RectangleF ef = ZoomService.GetInstance(base.host).ToScaledPixels(controls[i].BoundsF, controls[i].Dpi);
                    dragRects.Add(ef);
                }
                List<XRControl> controlArray = new List<XRControl>(controls.OfType<XRControl>());

                RectangleF[] rects = this.CreateRectArray(basePoint, control as Band, controls[0], controls[0].BoundsF, dragRects.ToArray(), controlArray.ToArray());

                this.AdornerService.DrawScreenRects(rects);
                this.RulerService.DrawShadows(rects, GetBandLevel(control as Band));
            }
        }

        public override void HandleDragDrop(object sender, DragEventArgs e)
        {
            XRControl[] controls = GetTemplateControlsFromData(e.Data);

            if (controls != null && controls.Length > 0) 
            {

                    PointF screenCoords = new Point(e.X, e.Y);

                    IBandViewInfoService serv = host.GetService(typeof(IBandViewInfoService)) as IBandViewInfoService;
                    ZoomService zoomServ = host.GetService(typeof(ZoomService)) as ZoomService;

                    Band band = serv.GetViewInfoByScreenPoint(screenCoords).Band;

                    if (band != null)
                    {
                        PointF clientCoords = serv.PointToClient(screenCoords, null, band);
                        clientCoords = GraphicsUnitConverter.Convert(clientCoords, GraphicsDpi.Pixel, band.RootReport.Dpi);
                        clientCoords = new PointF(clientCoords.X / zoomServ.ZoomFactor, clientCoords.Y / zoomServ.ZoomFactor);

                        if (clientCoords.X < 0)
                            clientCoords.X = 0;

                        if (clientCoords.Y < 0)
                            clientCoords.Y = 0;                        
                        
                        ArrayList selection = new ArrayList();

                        for (int i = controls.Length - 1; i >= 0; i--)
                        {
                            XRControl newControl = controls[i];
                            band.Controls.Add(newControl);
                            newControl.LocationF = PointF.Add(newControl.LocationF, new SizeF(clientCoords));

                            host.Container.Add(newControl);
                            AddControlsToContainer(newControl.Controls, host);
                            selection.Add(newControl);
                        }

                        ISelectionService selectionService = host.GetService(typeof(ISelectionService)) as ISelectionService;
                        selectionService.SetSelectedComponents(selection, SelectionTypes.Auto);
                    }                    
            }

            base.AdornerService.ResetSnapping();
            base.RulerService.HideShadows();
        }

        private void AddControlsToContainer(XRControlCollection controls, IDesignerHost host)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                host.Container.Add(controls[i]);
                AddControlsToContainer(controls[i].Controls, host);
            }

        }

        protected XRControl[] GetTemplateControlsFromData(IDataObject data)
        {
            if (templateControls == null)
            {
                TreeListNode node = data.GetData("DevExpress.XtraTreeList.Nodes.TreeListNode") as TreeListNode;
                if (node != null)
                {
                    string templateName = Convert.ToString(node.GetValue(0));
                    string categoryName = Convert.ToString(node.ParentNode.GetValue(0));

                    templateControls = ControlTemplateStorage.GetTemplateControls(templateStorage, categoryName, templateName);
                }
            }

            return templateControls;
        }

        public int GetBandLevel(Band band)
        {
            int nestedLevel = -1;
            
            XRControl parent = band.Parent;
            while (parent != null)
            {
                parent = parent.Parent;
                nestedLevel++;
            }
            return nestedLevel;
        }
    }
}
