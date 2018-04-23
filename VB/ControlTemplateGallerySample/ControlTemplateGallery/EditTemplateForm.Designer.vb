Namespace ControlTemplateGallerySample
    Partial Public Class EditTemplateForm
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

        #Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.labelControl1 = New DevExpress.XtraEditors.LabelControl()
            Me.teCategoryName = New DevExpress.XtraEditors.TextEdit()
            Me.teTemplateName = New DevExpress.XtraEditors.TextEdit()
            Me.labelControl2 = New DevExpress.XtraEditors.LabelControl()
            Me.separatorControl1 = New DevExpress.XtraEditors.SeparatorControl()
            Me.btnCancel = New DevExpress.XtraEditors.SimpleButton()
            Me.btnOk = New DevExpress.XtraEditors.SimpleButton()
            DirectCast(Me.teCategoryName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.teTemplateName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.separatorControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' labelControl1
            ' 
            Me.labelControl1.Location = New System.Drawing.Point(13, 13)
            Me.labelControl1.Name = "labelControl1"
            Me.labelControl1.Size = New System.Drawing.Size(49, 13)
            Me.labelControl1.TabIndex = 0
            Me.labelControl1.Text = "Category:"
            ' 
            ' teCategoryName
            ' 
            Me.teCategoryName.Anchor = (CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
            Me.teCategoryName.EditValue = "New Category"
            Me.teCategoryName.Location = New System.Drawing.Point(78, 10)
            Me.teCategoryName.Name = "teCategoryName"
            Me.teCategoryName.Size = New System.Drawing.Size(334, 20)
            Me.teCategoryName.TabIndex = 1
            ' 
            ' teTemplateName
            ' 
            Me.teTemplateName.Anchor = (CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
            Me.teTemplateName.EditValue = "New Template"
            Me.teTemplateName.Location = New System.Drawing.Point(78, 38)
            Me.teTemplateName.Name = "teTemplateName"
            Me.teTemplateName.Size = New System.Drawing.Size(334, 20)
            Me.teTemplateName.TabIndex = 3
            ' 
            ' labelControl2
            ' 
            Me.labelControl2.Location = New System.Drawing.Point(13, 41)
            Me.labelControl2.Name = "labelControl2"
            Me.labelControl2.Size = New System.Drawing.Size(48, 13)
            Me.labelControl2.TabIndex = 2
            Me.labelControl2.Text = "Template:"
            ' 
            ' separatorControl1
            ' 
            Me.separatorControl1.Anchor = (CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
            Me.separatorControl1.Location = New System.Drawing.Point(12, 60)
            Me.separatorControl1.Name = "separatorControl1"
            Me.separatorControl1.Size = New System.Drawing.Size(400, 21)
            Me.separatorControl1.TabIndex = 4
            ' 
            ' btnCancel
            ' 
            Me.btnCancel.Anchor = (CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(337, 87)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 5
            Me.btnCancel.Text = "Cancel"
            ' 
            ' btnOk
            ' 
            Me.btnOk.Anchor = (CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
            Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOk.Location = New System.Drawing.Point(256, 87)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(75, 23)
            Me.btnOk.TabIndex = 6
            Me.btnOk.Text = "OK"
            ' 
            ' EditTemplateForm
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(423, 118)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.separatorControl1)
            Me.Controls.Add(Me.teTemplateName)
            Me.Controls.Add(Me.labelControl2)
            Me.Controls.Add(Me.teCategoryName)
            Me.Controls.Add(Me.labelControl1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
            Me.Name = "EditTemplateForm"
            Me.Text = "EditTemplateForm"
            DirectCast(Me.teCategoryName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.teTemplateName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.separatorControl1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        #End Region

        Private labelControl1 As DevExpress.XtraEditors.LabelControl
        Private teCategoryName As DevExpress.XtraEditors.TextEdit
        Private teTemplateName As DevExpress.XtraEditors.TextEdit
        Private labelControl2 As DevExpress.XtraEditors.LabelControl
        Private separatorControl1 As DevExpress.XtraEditors.SeparatorControl
        Private btnCancel As DevExpress.XtraEditors.SimpleButton
        Private btnOk As DevExpress.XtraEditors.SimpleButton

    End Class
End Namespace