Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.UserDesigner
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraBars.Docking
Imports System.ComponentModel.Design

Namespace ControlTemplateGallerySample
	Partial Public Class Form1
		Inherits XtraForm

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			Dim control As Control = CreateTemplateControl(reportDesigner1)
			templateDockPanel1_Container.Controls.Add(control)

			AddHandler reportDesigner1.DesignPanelLoaded, AddressOf reportDesigner1_DesignPanelLoaded
			reportDesigner1.OpenReport(New XtraReport1())
		End Sub

		Private Sub reportDesigner1_DesignPanelLoaded(ByVal sender As Object, ByVal e As DesignerLoadedEventArgs)
			RemoveHandler reportDesigner1.DesignPanelLoaded, AddressOf reportDesigner1_DesignPanelLoaded

			'Set initial selection in designer for demonstration purposes 
			BeginInvoke(New MethodInvoker(Sub()
				Dim panel As XRDesignPanel = DirectCast(sender, XRDesignPanel)
				Dim rep As XtraReport1 = DirectCast(panel.Report, XtraReport1)
				Dim serv As ISelectionService = TryCast(panel.GetService(GetType(ISelectionService)), ISelectionService)
				serv.SetSelectedComponents(New Object() { rep.xrRichText1, rep.xrRichText2 }, SelectionTypes.Auto)
			End Sub))
		End Sub

		Private Function CreateTemplateControl(ByVal controller As XRDesignMdiController) As Control
			Dim control As New TemplateUserControl(controller) With {.Dock = System.Windows.Forms.DockStyle.Fill}

			control.Storage = New PersistentControlTemplateStorage("SampleStorage.xml") ' Persistent storage
			'userControl.Storage = new ControlTemplateStorage(); // In-memory storage

			controller.DesignPanelListeners.Add(control)
			Return control
		End Function
	End Class
End Namespace
