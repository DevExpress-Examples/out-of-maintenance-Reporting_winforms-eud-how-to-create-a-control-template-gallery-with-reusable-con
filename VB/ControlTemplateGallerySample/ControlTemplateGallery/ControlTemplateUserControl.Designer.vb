Namespace ControlTemplateGallerySample
    Partial Public Class ControlTemplateUserControl
        ''' <summary> 
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary> 
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        #Region "Component Designer generated code"

        ''' <summary> 
        ''' Required method for Designer support - do not modify 
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(ControlTemplateUserControl))
            Me.panelControl1 = New DevExpress.XtraEditors.PanelControl()
            Me.btnDelete = New DevExpress.XtraEditors.SimpleButton()
            Me.btnAdd = New DevExpress.XtraEditors.SimpleButton()
            Me.tlTemplates = New DevExpress.XtraTreeList.TreeList()
            Me.treeListColumn1 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
            Me.imageCollection1 = New DevExpress.Utils.ImageCollection(Me.components)
            DirectCast(Me.panelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.panelControl1.SuspendLayout()
            DirectCast(Me.tlTemplates, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.imageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' panelControl1
            ' 
            Me.panelControl1.Controls.Add(Me.btnDelete)
            Me.panelControl1.Controls.Add(Me.btnAdd)
            Me.panelControl1.Dock = System.Windows.Forms.DockStyle.Top
            Me.panelControl1.Location = New System.Drawing.Point(0, 0)
            Me.panelControl1.Name = "panelControl1"
            Me.panelControl1.Size = New System.Drawing.Size(254, 42)
            Me.panelControl1.TabIndex = 0
            ' 
            ' btnDelete
            ' 
            Me.btnDelete.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat
            Me.btnDelete.Image = (DirectCast(resources.GetObject("btnDelete.Image"), System.Drawing.Image))
            Me.btnDelete.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter
            Me.btnDelete.Location = New System.Drawing.Point(43, 5)
            Me.btnDelete.Name = "btnDelete"
            Me.btnDelete.Size = New System.Drawing.Size(32, 32)
            Me.btnDelete.TabIndex = 4
            Me.btnDelete.Text = "simpleButton3"
            ' 
            ' btnAdd
            ' 
            Me.btnAdd.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat
            Me.btnAdd.Image = (DirectCast(resources.GetObject("btnAdd.Image"), System.Drawing.Image))
            Me.btnAdd.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter
            Me.btnAdd.Location = New System.Drawing.Point(5, 5)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(32, 32)
            Me.btnAdd.TabIndex = 2
            Me.btnAdd.Text = "simpleButton1"
            ' 
            ' tlTemplates
            ' 
            Me.tlTemplates.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() { Me.treeListColumn1})
            Me.tlTemplates.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tlTemplates.Location = New System.Drawing.Point(0, 42)
            Me.tlTemplates.Name = "tlTemplates"
            Me.tlTemplates.BeginUnboundLoad()
            Me.tlTemplates.AppendNode(New Object() { "Test"}, -1)
            Me.tlTemplates.AppendNode(New Object() { "Test2"}, 0)
            Me.tlTemplates.EndUnboundLoad()
            Me.tlTemplates.OptionsBehavior.Editable = False
            Me.tlTemplates.OptionsDragAndDrop.DragNodesMode = DevExpress.XtraTreeList.DragNodesMode.Single
            Me.tlTemplates.OptionsSelection.InvertSelection = True
            Me.tlTemplates.OptionsView.ShowColumns = False
            Me.tlTemplates.OptionsView.ShowHorzLines = False
            Me.tlTemplates.OptionsView.ShowIndicator = False
            Me.tlTemplates.OptionsView.ShowVertLines = False
            Me.tlTemplates.Size = New System.Drawing.Size(254, 316)
            Me.tlTemplates.StateImageList = Me.imageCollection1
            Me.tlTemplates.TabIndex = 0
            ' 
            ' treeListColumn1
            ' 
            Me.treeListColumn1.MinWidth = 70
            Me.treeListColumn1.Name = "treeListColumn1"
            Me.treeListColumn1.Visible = True
            Me.treeListColumn1.VisibleIndex = 0
            ' 
            ' imageCollection1
            ' 
            Me.imageCollection1.ImageStream = (DirectCast(resources.GetObject("imageCollection1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer))
            Me.imageCollection1.InsertGalleryImage("open_16x16.png", "images/actions/open_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/open_16x16.png"), 0)
            Me.imageCollection1.Images.SetKeyName(0, "open_16x16.png")
            Me.imageCollection1.InsertGalleryImage("template_16x16.png", "images/support/template_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/support/template_16x16.png"), 1)
            Me.imageCollection1.Images.SetKeyName(1, "template_16x16.png")
            ' 
            ' ControlTemplateUserControl
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.tlTemplates)
            Me.Controls.Add(Me.panelControl1)
            Me.Name = "ControlTemplateUserControl"
            Me.Size = New System.Drawing.Size(254, 358)
            DirectCast(Me.panelControl1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.panelControl1.ResumeLayout(False)
            DirectCast(Me.tlTemplates, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.imageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        #End Region

        Private panelControl1 As DevExpress.XtraEditors.PanelControl
        Private btnDelete As DevExpress.XtraEditors.SimpleButton
        Private btnAdd As DevExpress.XtraEditors.SimpleButton
        Private tlTemplates As DevExpress.XtraTreeList.TreeList
        Private treeListColumn1 As DevExpress.XtraTreeList.Columns.TreeListColumn
        Private imageCollection1 As DevExpress.Utils.ImageCollection
    End Class
End Namespace
