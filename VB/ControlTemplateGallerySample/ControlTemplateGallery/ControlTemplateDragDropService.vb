Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.Design
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.UserDesigner
Imports DevExpress.XtraTreeList.Nodes
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel.Design
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Namespace ControlTemplateGallerySample
    Public Class ControlTemplateDragDropService
        Inherits DragDropService
        Implements IDragDropService

        Private templateStorage As IControlTemplateStorage

        Public Sub New(ByVal designerHost As IDesignerHost, ByVal storage As IControlTemplateStorage)
            MyBase.New(designerHost)
            templateStorage = storage
        End Sub

        Private Function IDragDropService_GetDragHandler(ByVal data As System.Windows.Forms.IDataObject) As IDragHandler Implements IDragDropService.GetDragHandler
            Dim handler As IDragHandler = MyBase.GetDragHandler(data)

            If DirectCast(handler, Object).GetType() Is GetType(DragHandlerBase) AndAlso data IsNot Nothing AndAlso (Not data.GetDataPresent("CF_DSREF")) Then
                handler = New CustomTemplateDragHandler(Me.DesignerHost, templateStorage)
            End If

            Return handler
        End Function
    End Class

    Public Class CustomTemplateDragHandler
        Inherits DragHandlerBase

        Private templateControls() As XRControl
        Private templateStorage As IControlTemplateStorage

        Public Sub New(ByVal host As IDesignerHost, ByVal storage As IControlTemplateStorage)
            MyBase.New(host)
            templateStorage = storage
        End Sub

        Public Overrides Sub HandleDragEnter(ByVal sender As Object, ByVal e As DragEventArgs)
            e.Effect = DragDropEffects.Copy
        End Sub

        Public Overrides Sub HandleDragOver(ByVal sender As Object, ByVal e As DragEventArgs)
            MyBase.HandleDragOver(sender, e)
            Dim controlByScreenPoint As XRControl = MyBase.bandViewSvc.GetControlByScreenPoint(CType(New System.Drawing.Point(e.X, e.Y), PointF))
            If controlByScreenPoint IsNot Nothing Then
                Dim control As XRControl = controlByScreenPoint

                If Not(TypeOf control Is Band) Then
                    control = control.Band
                End If

                MyBase.SelectComponent(MyBase.host, control)

                e.Effect = DragDropEffects.Copy

                Dim basePoint As PointF = Me.EvalBasePoint(e)
                Dim controls() As XRControl = GetTemplateControlsFromData(e.Data)

                Dim dragRects As New List(Of RectangleF)()

                For i As Integer = 0 To controls.Length - 1
                    Dim ef As RectangleF = ZoomService.GetInstance(MyBase.host).ToScaledPixels(controls(i).BoundsF, controls(i).Dpi)
                    dragRects.Add(ef)
                Next i
                Dim controlArray As New List(Of XRControl)(controls.OfType(Of XRControl)())

                Dim rects() As RectangleF = Me.CreateRectArray(basePoint, TryCast(control, Band), controls(0), controls(0).BoundsF, dragRects.ToArray(), controlArray.ToArray())

                Me.AdornerService.DrawScreenRects(rects)
                Me.RulerService.DrawShadows(rects, GetBandLevel(TryCast(control, Band)))
            End If
        End Sub

        Public Overrides Sub HandleDragDrop(ByVal sender As Object, ByVal e As DragEventArgs)
            Dim controls() As XRControl = GetTemplateControlsFromData(e.Data)

            If controls IsNot Nothing AndAlso controls.Length > 0 Then

                Dim screenCoords As PointF = New System.Drawing.Point(e.X, e.Y)

                    Dim serv As IBandViewInfoService = TryCast(host.GetService(GetType(IBandViewInfoService)), IBandViewInfoService)
                    Dim zoomServ As ZoomService = TryCast(host.GetService(GetType(ZoomService)), ZoomService)

                    Dim band As Band = serv.GetViewInfoByScreenPoint(screenCoords).Band

                    If band IsNot Nothing Then
                        Dim clientCoords As PointF = serv.PointToClient(screenCoords, Nothing, band)
                        clientCoords = GraphicsUnitConverter.Convert(clientCoords, GraphicsDpi.Pixel, band.RootReport.Dpi)
                        clientCoords = New PointF(clientCoords.X / zoomServ.ZoomFactor, clientCoords.Y / zoomServ.ZoomFactor)

                        If clientCoords.X < 0 Then
                            clientCoords.X = 0
                        End If

                        If clientCoords.Y < 0 Then
                            clientCoords.Y = 0
                        End If

                        Dim selection As New ArrayList()

                        For i As Integer = controls.Length - 1 To 0 Step -1
                            Dim newControl As XRControl = controls(i)
                            band.Controls.Add(newControl)
                            newControl.LocationF = PointF.Add(newControl.LocationF, New SizeF(clientCoords))

                            host.Container.Add(newControl)
                            AddControlsToContainer(newControl.Controls, host)
                            selection.Add(newControl)
                        Next i

                        Dim selectionService As ISelectionService = TryCast(host.GetService(GetType(ISelectionService)), ISelectionService)
                        selectionService.SetSelectedComponents(selection, SelectionTypes.Auto)
                    End If
            End If

            MyBase.AdornerService.ResetSnapping()
            MyBase.RulerService.HideShadows()
        End Sub

        Private Sub AddControlsToContainer(ByVal controls As XRControlCollection, ByVal host As IDesignerHost)
            For i As Integer = 0 To controls.Count - 1
                host.Container.Add(controls(i))
                AddControlsToContainer(controls(i).Controls, host)
            Next i

        End Sub

        Protected Function GetTemplateControlsFromData(ByVal data As IDataObject) As XRControl()
            If templateControls Is Nothing Then
                Dim node As TreeListNode = TryCast(data.GetData("DevExpress.XtraTreeList.Nodes.TreeListNode"), TreeListNode)
                If node IsNot Nothing Then
                    Dim templateName As String = Convert.ToString(node.GetValue(0))
                    Dim categoryName As String = Convert.ToString(node.ParentNode.GetValue(0))

                    templateControls = ControlTemplateStorage.GetTemplateControls(templateStorage, categoryName, templateName)
                End If
            End If

            Return templateControls
        End Function

        Public Function GetBandLevel(ByVal band As Band) As Integer
            Dim nestedLevel As Integer = -1

            Dim parent As XRControl = band.Parent
            Do While parent IsNot Nothing
                parent = parent.Parent
                nestedLevel += 1
            Loop
            Return nestedLevel
        End Function
    End Class
End Namespace
