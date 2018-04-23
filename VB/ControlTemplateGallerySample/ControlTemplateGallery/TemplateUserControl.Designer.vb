Namespace ControlTemplateGallerySample
	Partial Public Class TemplateUserControl
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
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(TemplateUserControl))
			Me.panelControl1 = New DevExpress.XtraEditors.PanelControl()
			Me.btnDelete = New DevExpress.XtraEditors.SimpleButton()
			Me.btnAdd = New DevExpress.XtraEditors.SimpleButton()
			Me.tlTemplates = New DevExpress.XtraTreeList.TreeList()
			Me.treeListColumn1 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
			DirectCast(Me.panelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.panelControl1.SuspendLayout()
			DirectCast(Me.tlTemplates, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' panelControl1
			' 
			Me.panelControl1.Controls.Add(Me.btnDelete)
			Me.panelControl1.Controls.Add(Me.btnAdd)
			Me.panelControl1.Dock = System.Windows.Forms.DockStyle.Top
			Me.panelControl1.Location = New System.Drawing.Point(0, 0)
			Me.panelControl1.Name = "panelControl1"
			Me.panelControl1.Size = New System.Drawing.Size(254, 30)
			Me.panelControl1.TabIndex = 0
			' 
			' btnDelete
			' 
			Me.btnDelete.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat
			Me.btnDelete.Image = (DirectCast(resources.GetObject("btnDelete.Image"), System.Drawing.Image))
			Me.btnDelete.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter
			Me.btnDelete.Location = New System.Drawing.Point(30, 5)
			Me.btnDelete.Name = "btnDelete"
			Me.btnDelete.Size = New System.Drawing.Size(20, 20)
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
			Me.btnAdd.Size = New System.Drawing.Size(20, 20)
			Me.btnAdd.TabIndex = 2
			Me.btnAdd.Text = "simpleButton1"
			' 
			' tlTemplates
			' 
			Me.tlTemplates.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() { Me.treeListColumn1})
			Me.tlTemplates.Dock = System.Windows.Forms.DockStyle.Fill
			Me.tlTemplates.Location = New System.Drawing.Point(0, 30)
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
			Me.tlTemplates.Size = New System.Drawing.Size(254, 328)
			Me.tlTemplates.TabIndex = 0
			' 
			' treeListColumn1
			' 
			Me.treeListColumn1.MinWidth = 70
			Me.treeListColumn1.Name = "treeListColumn1"
			Me.treeListColumn1.Visible = True
			Me.treeListColumn1.VisibleIndex = 0
			' 
			' TemplateUserControl
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.tlTemplates)
			Me.Controls.Add(Me.panelControl1)
			Me.Name = "TemplateUserControl"
			Me.Size = New System.Drawing.Size(254, 358)
			DirectCast(Me.panelControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.panelControl1.ResumeLayout(False)
			DirectCast(Me.tlTemplates, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private panelControl1 As DevExpress.XtraEditors.PanelControl
		Private btnDelete As DevExpress.XtraEditors.SimpleButton
		Private btnAdd As DevExpress.XtraEditors.SimpleButton
		Private tlTemplates As DevExpress.XtraTreeList.TreeList
		Private treeListColumn1 As DevExpress.XtraTreeList.Columns.TreeListColumn
	End Class
End Namespace
