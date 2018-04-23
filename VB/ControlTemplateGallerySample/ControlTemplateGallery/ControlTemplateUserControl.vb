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
    Partial Public Class ControlTemplateUserControl
        Inherits DevExpress.XtraEditors.XtraUserControl
        Implements IDesignPanelListener


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

            If node.Level = 0 Then
                Dim categoryName As String = Convert.ToString(node.GetValue(0))
                templateStorage.DeleteCategory(categoryName)
            Else
                Dim templateName As String = Convert.ToString(node.GetValue(0))
                Dim categoryName As String = Convert.ToString(node.ParentNode.GetValue(0))

                templateStorage.DeleteTemplate(categoryName, templateName)
            End If

            InitializeStorage()
        End Sub

        Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim selectedControls As List(Of XRControl) = GetSelectedControls()
            If selectedControls.Count > 0 Then
                Dim templateForm As New EditTemplateForm()
                If templateForm.ShowDialog() = DialogResult.OK Then
                    Dim catName As String = templateForm.CategoryName
                    Dim templateName As String = templateForm.TemplateName
                    Dim layout() As Byte = GenerateLayoutBytes(selectedControls.ToArray())

                    templateStorage.SetData(catName, templateName, layout)

                    InitializeStorage()
                End If
            End If
        End Sub

        Public Sub New(ByVal mdiController As XRDesignMdiController)
            Me.New()
            Me.Controller = mdiController
        End Sub

        Private Function GetSelectedControls() As List(Of XRControl)
            Dim selectedComponents() As Object = panel.GetSelectedComponents()

            Dim selectedControls As New List(Of XRControl)()

            For i As Integer = 0 To selectedComponents.Length - 1
                If TypeOf selectedComponents(i) Is XRControl AndAlso Not(TypeOf selectedComponents(i) Is Band) Then
                    selectedControls.Add(TryCast(selectedComponents(i), XRControl))
                End If
            Next i

            Return selectedControls
        End Function

        Private Sub InitializeStorage()
            tlTemplates.Nodes.Clear()
            If templateStorage IsNot Nothing Then
                Dim categories() As String = templateStorage.GetCategoryNames()
                For i As Integer = 0 To categories.Length - 1
                    Dim node As TreeListNode = tlTemplates.Nodes.Add(New Object() { categories(i) })

                    Dim templates() As String = templateStorage.GetTemplateNamesForCategory(categories(i))
                    For j As Integer = 0 To templates.Length - 1
                        node.Nodes.Add(New Object() { templates(j) })
                    Next j
                Next i
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
            e.DesignerHost.AddService(GetType(IDragDropService), New ControlTemplateDragDropService(e.DesignerHost, templateStorage))
        End Sub

        Private Sub OnGetStateImage(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.GetStateImageEventArgs)
            e.NodeImageIndex = e.Node.Level
        End Sub

        Private Sub OnFocusedNodeChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs)
            btnDelete.Enabled = e.Node IsNot Nothing
        End Sub

        Private Function GenerateLayoutBytes(ByVal controls() As Object) As Byte()
            Dim serv As IBandViewInfoService = TryCast(panel.GetService(GetType(IBandViewInfoService)), IBandViewInfoService)
            Dim locations As New List(Of PointF)()

            'In pixels
            Dim rect As RectangleF = serv.GetControlScreenBounds(DirectCast(controls(0), XRControl))
            locations.Add(New PointF(rect.Left, rect.Top))
            Dim topMost As Integer = CInt((rect.Top))
            Dim leftMost As Integer = CInt((rect.Left))

            For i As Integer = 1 To controls.Length - 1
                Dim control As XRControl = TryCast(controls(i), XRControl)
                If Not(TypeOf control Is Band) Then
                    rect = serv.GetControlScreenBounds(control)
                    locations.Add(New PointF(rect.Left, rect.Top))
                    If rect.Top < topMost Then
                        topMost = CInt((rect.Top))
                    End If
                    If rect.Left < leftMost Then
                        leftMost = CInt((rect.Left))
                    End If
                End If
            Next i

            Dim tempReport As New XtraReport()

            Using stream As New MemoryStream()
                panel.Report.SaveLayoutToXml(stream)
                stream.Position = 0

                tempReport.LoadLayoutFromXml(stream)
            End Using

            Dim clones As New ArrayList()

            For i As Integer = 0 To controls.Length - 1
                Dim control As XRControl = TryCast(controls(i), XRControl)
                Dim clone As XRControl = tempReport.FindControl(control.Name, True)
                clones.Add(clone)
            Next i

            tempReport.Bands.Clear()

            Dim tempBand As New DetailBand()
            tempReport.Bands.Add(tempBand)

            For i As Integer = 0 To clones.Count - 1
                Dim newControl As XRControl = TryCast(clones(i), XRControl)
                tempBand.Controls.Add(newControl)

                Dim originalLocation As PointF = locations(i)
                Dim newLocation As New PointF(originalLocation.X - leftMost, originalLocation.Y - topMost)

                newLocation = GraphicsUnitConverter.Convert(newLocation, GraphicsDpi.Pixel, panel.Report.Dpi)

                newControl.LocationF = newLocation
            Next i

            Dim layoutBytes() As Byte = Nothing

            Using stream As New MemoryStream()
                tempReport.SaveLayoutToXml(stream)
                layoutBytes = stream.ToArray()
            End Using

            tempReport.Dispose()

            Return layoutBytes
        End Function

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
                    InitializeStorage()
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
