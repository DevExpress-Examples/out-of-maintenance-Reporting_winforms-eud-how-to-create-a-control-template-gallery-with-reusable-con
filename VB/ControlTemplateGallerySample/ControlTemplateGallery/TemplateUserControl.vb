Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Text
Imports System.Linq
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UserDesigner
Imports DevExpress.XtraReports.Design
Imports System.ComponentModel.Design
Imports DevExpress.XtraTreeList.Nodes
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel.Design.Serialization
Imports DevExpress.XtraReports.UserDesigner.Native
Imports System.Collections
Imports DevExpress.XtraPrinting
Imports System.IO

Namespace ControlTemplateGallerySample
	Partial Public Class TemplateUserControl
		Inherits DevExpress.XtraEditors.XtraUserControl
		Implements IDesignPanelListener

'INSTANT VB NOTE: The variable controller was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private controller_Renamed As XRDesignMdiController
		Private templateStorage As IControlTemplateStorage
		Private panel As XRDesignPanel

		Protected Sub New()
			InitializeComponent()
			AddHandler tlTemplates.BeforeDragNode, AddressOf OnBeforeDragNode
			AddHandler tlTemplates.DragOver, AddressOf tlTemplates_OnDragOver
			AddHandler tlTemplates.GetStateImage, AddressOf OnGetStateImage
			AddHandler tlTemplates.FocusedNodeChanged, AddressOf OnFocusedNodeChanged
			AddHandler btnAdd.Click, AddressOf btnAdd_Click
			AddHandler btnDelete.Click, AddressOf btnDelete_Click
		End Sub

		Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
			Dim node As TreeListNode = tlTemplates.FocusedNode

			If node.Level = 0 AndAlso ShowMessage("Are you sure you want to delete this category?", "Confirm Deleting") = DialogResult.Yes Then
				Dim categoryName As String = Convert.ToString(node.GetValue(0))
				templateStorage.DeleteCategory(categoryName)
			ElseIf ShowMessage("Are you sure you want to delete this template?", "Confirm Deleting") = DialogResult.Yes Then
				Dim templateName As String = Convert.ToString(node.GetValue(0))
				Dim categoryName As String = Convert.ToString(node.ParentNode.GetValue(0))
				templateStorage.DeleteTemplate(categoryName, templateName)
			End If
			RebuildNodes()
		End Sub

		Private Function ShowMessage(ByVal text As String, ByVal caption As String) As DialogResult
			Return XtraMessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
		End Function

		Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs)
			Dim selectedControls As List(Of XRControl) = GetSelectedControls()
			If selectedControls.Count = 0 Then
				Return
			End If

			Dim templateForm As New TemplateEditorForm()
			If templateForm.ShowDialog(FindForm()) = DialogResult.OK Then

				Dim catName As String = templateForm.CategoryName
				Dim templateName As String = templateForm.TemplateName

				templateStorage.Store(catName, templateName, selectedControls.ToArray())
				RebuildNodes()
			End If
		End Sub

		Public Sub New(ByVal mdiController As XRDesignMdiController)
			Me.New()
			Me.Controller = mdiController
		End Sub

		Private Function GetSelectedControls() As List(Of XRControl)
			Dim serv As ISelectionService = TryCast(panel.GetService(GetType(ISelectionService)), ISelectionService)

			Dim selectedComponents As IEnumerable(Of XRControl) = panel.GetSelectedComponents().Where(Function(item) TypeOf item Is XRControl AndAlso Not(TypeOf item Is Band) AndAlso Not(TypeOf item Is XRCrossBandControl)).Cast(Of XRControl)()

			Dim selectedControls As New List(Of XRControl)()
			If selectedComponents.Count() = 0 Then
				Return selectedControls
			End If

			Dim primaryControl As XRControl = If(TryCast(serv.PrimarySelection, XRControl), selectedComponents.ElementAt(0))

			For Each item As XRControl In selectedComponents
				If ReferenceEquals(item.Band, primaryControl.Band) Then
					selectedControls.Add(TryCast(item, XRControl))
				End If
			Next item
			Return selectedControls
		End Function

		Private Sub RebuildNodes()
			tlTemplates.Nodes.Clear()
			If templateStorage IsNot Nothing Then
				Dim categories() As String = templateStorage.GetCategoryNames()
				For Each category As String In categories
					Dim node As TreeListNode = tlTemplates.Nodes.Add(New Object() { category })

					Dim templates() As String = templateStorage.GetTemplateNamesForCategory(category)
					For Each template As String In templates
						node.Nodes.Add(New Object() { template })
					Next template
				Next category
			End If
			tlTemplates.ExpandAll()
		End Sub

		Private Sub OnBeforeDragNode(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.BeforeDragNodeEventArgs)
			e.CanDrag = e.Node.Level > 0
		End Sub

		Private Sub tlTemplates_OnDragOver(ByVal sender As Object, ByVal e As DragEventArgs)
			e.Effect = DragDropEffects.None
		End Sub

		Private Sub OnDesignPanelLoaded(ByVal sender As Object, ByVal e As DesignerLoadedEventArgs)
			e.DesignerHost.RemoveService(GetType(IDragDropService))
			e.DesignerHost.AddService(GetType(IDragDropService), New MyDragDropService(e.DesignerHost, templateStorage))
		End Sub

		Private Sub OnGetStateImage(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.GetStateImageEventArgs)
			e.NodeImageIndex = e.Node.Level
		End Sub

		Private Sub OnFocusedNodeChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs)
			btnDelete.Enabled = e.Node IsNot Nothing
		End Sub

		Public Property Controller() As XRDesignMdiController
			Get
				Return controller_Renamed
			End Get
			Set(ByVal value As XRDesignMdiController)
				If controller_Renamed IsNot value Then
					If controller_Renamed IsNot Nothing Then
						RemoveHandler controller_Renamed.DesignPanelLoaded, AddressOf OnDesignPanelLoaded
					End If

					controller_Renamed = value

					If controller_Renamed IsNot Nothing Then
						AddHandler controller_Renamed.DesignPanelLoaded, AddressOf OnDesignPanelLoaded
					End If
				End If
			End Set
		End Property

		Public Property Storage() As IControlTemplateStorage
			Get
				Return templateStorage
			End Get
			Set(ByVal value As IControlTemplateStorage)
				If templateStorage IsNot value Then
					templateStorage = value
					RebuildNodes()
				End If
			End Set
		End Property

		Public Property XRDesignPanel() As XRDesignPanel Implements IDesignPanelListener.XRDesignPanel
			Get
				Return panel
			End Get
			Set(ByVal value As XRDesignPanel)
				If panel IsNot value Then
					If panel IsNot Nothing Then
						RemoveHandler panel.SelectionChanged, AddressOf OnSelectionChanged
					End If

					panel = value

					If panel IsNot Nothing Then
						AddHandler panel.SelectionChanged, AddressOf OnSelectionChanged
					End If

					UpdateButtonState()
				End If
			End Set
		End Property

		Private Sub OnSelectionChanged(ByVal sender As Object, ByVal e As EventArgs)
			UpdateButtonState()
		End Sub

		Private Sub UpdateButtonState()
			btnAdd.Enabled = panel IsNot Nothing AndAlso GetSelectedControls().Count > 0
		End Sub
	End Class
End Namespace
