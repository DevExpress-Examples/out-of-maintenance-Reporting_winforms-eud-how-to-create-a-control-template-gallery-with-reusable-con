Namespace ControlTemplateGallerySample
	Partial Public Class XtraReport1
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

		#Region "Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(XtraReport1))
			Me.Detail = New DevExpress.XtraReports.UI.DetailBand()
			Me.TopMargin = New DevExpress.XtraReports.UI.TopMarginBand()
			Me.BottomMargin = New DevExpress.XtraReports.UI.BottomMarginBand()
			Me.xrRichText2 = New DevExpress.XtraReports.UI.XRRichText()
			Me.xrRichText1 = New DevExpress.XtraReports.UI.XRRichText()
			DirectCast(Me.xrRichText2, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.xrRichText1, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me, System.ComponentModel.ISupportInitialize).BeginInit()
			' 
			' Detail
			' 
			Me.Detail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() { Me.xrRichText2, Me.xrRichText1})
			Me.Detail.Dpi = 100F
			Me.Detail.HeightF = 100F
			Me.Detail.Name = "Detail"
			Me.Detail.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F)
			Me.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
			' 
			' TopMargin
			' 
			Me.TopMargin.Dpi = 100F
			Me.TopMargin.HeightF = 100F
			Me.TopMargin.Name = "TopMargin"
			Me.TopMargin.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F)
			Me.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
			' 
			' BottomMargin
			' 
			Me.BottomMargin.Dpi = 100F
			Me.BottomMargin.HeightF = 100F
			Me.BottomMargin.Name = "BottomMargin"
			Me.BottomMargin.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F)
			Me.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
			' 
			' xrRichText2
			' 
			Me.xrRichText2.Dpi = 100F
			Me.xrRichText2.Font = New System.Drawing.Font("Times New Roman", 9.75F)
			Me.xrRichText2.LocationFloat = New DevExpress.Utils.PointFloat(333.75F, 10.00001F)
			Me.xrRichText2.Name = "xrRichText2"
			Me.xrRichText2.SerializableRtfString = resources.GetString("xrRichText2.SerializableRtfString")
			Me.xrRichText2.SizeF = New System.Drawing.SizeF(306.25F, 63.54167F)
			' 
			' xrRichText1
			' 
			Me.xrRichText1.Dpi = 100F
			Me.xrRichText1.Font = New System.Drawing.Font("Times New Roman", 9.75F)
			Me.xrRichText1.LocationFloat = New DevExpress.Utils.PointFloat(10.00001F, 10.00001F)
			Me.xrRichText1.Name = "xrRichText1"
			Me.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString")
			Me.xrRichText1.SizeF = New System.Drawing.SizeF(306.25F, 63.54167F)
			' 
			' XtraReport1
			' 
			Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() { Me.Detail, Me.TopMargin, Me.BottomMargin})
			Me.Version = "16.1"
			DirectCast(Me.xrRichText2, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.xrRichText1, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me, System.ComponentModel.ISupportInitialize).EndInit()

		End Sub

		#End Region

		Private Detail As DevExpress.XtraReports.UI.DetailBand
		Private TopMargin As DevExpress.XtraReports.UI.TopMarginBand
		Private BottomMargin As DevExpress.XtraReports.UI.BottomMarginBand
		Public xrRichText2 As DevExpress.XtraReports.UI.XRRichText
		Public xrRichText1 As DevExpress.XtraReports.UI.XRRichText
	End Class
End Namespace
