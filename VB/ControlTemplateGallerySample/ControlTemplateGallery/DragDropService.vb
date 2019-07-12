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
	Friend Class MyDragDropService
		Inherits DragDropService
		Implements IDragDropService

		Private templateStorage As IControlTemplateStorage

		Public Sub New(ByVal designerHost As IDesignerHost, ByVal storage As IControlTemplateStorage)
			MyBase.New(designerHost)
			templateStorage = storage
		End Sub

		Private Function IDragDropService_GetDragHandler(ByVal data As System.Windows.Forms.IDataObject) As IDragHandler Implements IDragDropService.GetDragHandler
			Dim node As TreeListNode = GetNode(data)
			If node IsNot Nothing AndAlso (Not data.GetDataPresent(GetType(DragDataObject))) Then
				Dim controls() As XRControl = GetTemplateControlsFromData(node)
				Dim obj As DragDataObject = CreateDragData(controls, ZoomService.GetInstance(DesignerHost))
				data.SetData(GetType(DragDataObject), obj)
				Return New MyControlDragHandler(DesignerHost)
			End If
			Return MyBase.GetDragHandler(data)
		End Function

		Private Shared Function GetNode(ByVal data As IDataObject) As TreeListNode
			Return If(data IsNot Nothing, TryCast(data.GetData(GetType(TreeListNode).FullName), TreeListNode), Nothing)
		End Function

		Private Function GetTemplateControlsFromData(ByVal node As TreeListNode) As XRControl()
			Dim templateName As String = Convert.ToString(node.GetValue(0))
			Dim categoryName As String = Convert.ToString(node.ParentNode.GetValue(0))
			Return templateStorage.Restore(categoryName, templateName)
		End Function

		Private Function CreateDragData(ByVal controls() As XRControl, ByVal zoomService As ZoomService) As DragDataObject
			controls = controls.OrderBy(Function(item) item.BoundsF.X).ToArray()
			Dim baseControl As XRControl = controls(0)

			Dim controlRects(controls.Length - 1) As RectangleF
			Dim baseRect As RectangleF = baseControl.BoundsF

			For i As Integer = 0 To controls.Length - 1
				Dim rect As RectangleF = controls(i).BoundsF
				rect.Offset(-baseRect.X, -baseRect.Y)
				controlRects(i) = zoomService.ToScaledPixels(rect, controls(i).Dpi)
			Next i
			Return New DragDataObject(controls, baseControl, controlRects, PointF.Empty)
		End Function
	End Class

	Friend Class MyControlDragHandler
		Inherits ControlDragHandler

		Public Sub New(ByVal host As IDesignerHost)
			MyBase.New(host)
		End Sub
        Public Overrides Sub HandleDragDrop(ByVal sender As Object, ByVal e As DragEventArgs)
            Dim data As DragDataObject = TryCast(e.Data.GetData(GetType(DragDataObject)), DragDataObject)
            If data IsNot Nothing Then
                AddToContainerRecursive(data.Controls)
            End If
            MyBase.HandleDragDrop(sender, e)
        End Sub
        Public Overrides Sub HandleDragOver(sender As Object, e As DragEventArgs)
            MyBase.HandleDragOver(sender, e)
            e.Effect = DragDropEffects.Copy
        End Sub
        Private Sub AddToContainerRecursive(ByVal controls As IList)
            For Each item As XRControl In controls
                host.Container.Add(item)
                AddToContainerRecursive(item.Controls)
            Next item
        End Sub
    End Class
End Namespace
