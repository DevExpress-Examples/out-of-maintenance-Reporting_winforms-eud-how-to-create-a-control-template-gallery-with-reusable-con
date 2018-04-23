Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Linq
Imports System.Windows.Forms
Imports DevExpress.XtraEditors

Namespace ControlTemplateGallerySample
	Partial Public Class TemplateEditorForm
		Inherits DevExpress.XtraEditors.XtraForm

		Public Sub New()
			InitializeComponent()
		End Sub

		Public Property CategoryName() As String
			Get
				Return teCategoryName.Text
			End Get
			Set(ByVal value As String)
				teCategoryName.Text = value
			End Set
		End Property

		Public Property TemplateName() As String
			Get
				Return teTemplateName.Text
			End Get
			Set(ByVal value As String)
				teTemplateName.Text = value
			End Set
		End Property
	End Class
End Namespace