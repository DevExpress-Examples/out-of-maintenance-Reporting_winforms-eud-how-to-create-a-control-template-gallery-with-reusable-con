Imports DevExpress.XtraReports.UI
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Data
Imports DevExpress.XtraEditors
Imports System.Windows.Forms
Imports System.Drawing

Namespace ControlTemplateGallerySample
	Public Interface IControlTemplateStorage
		Function GetCategoryNames() As String()
		Function GetTemplateNamesForCategory(ByVal categoryName As String) As String()
		Function Restore(ByVal categoryName As String, ByVal templateName As String) As XRControl()
		Sub Store(ByVal categoryName As String, ByVal templateName As String, ByVal controls() As XRControl)
		Sub DeleteTemplate(ByVal categoryName As String, ByVal templateName As String)
		Sub DeleteCategory(ByVal categoryName As String)
	End Interface

	Friend Class ControlTemplateStorageBase
		Protected Shared Function DeserializeControls(ByVal bytes() As Byte) As XRControl()
			If bytes IsNot Nothing AndAlso bytes.Length > 0 Then
				Dim tempReport As New XtraReport()
				Using stream As New MemoryStream(bytes)
					tempReport.LoadLayoutFromXml(stream)
				End Using
				Return tempReport.Bands(BandKind.Detail).Controls.Cast(Of XRControl)().ToArray()
			End If
			Return New XRControl(){}
		End Function
		Protected Shared Function SerializeControls(ByVal controls() As XRControl) As Byte()
			Dim tempReport As New XtraReport()
			Dim tempBand As New DetailBand()
			tempReport.Bands.Add(tempBand)

			Using cloneReport As New XtraReport()
				Using stream As New MemoryStream()
					controls(0).RootReport.SaveLayoutToXml(stream)
					stream.Position = 0

					cloneReport.LoadLayoutFromXml(stream)
				End Using
				For Each control As XRControl In controls
					Dim clone As XRControl = cloneReport.FindControl(control.Name, True)
					If clone IsNot Nothing Then
						tempBand.Controls.Add(clone)
					End If
				Next control
			End Using

			Dim topMost As Single = Single.MaxValue
			Dim leftMost As Single = Single.MaxValue

			For Each control As XRControl In tempBand.Controls
				topMost = Math.Min(topMost, control.TopF)
				leftMost = Math.Min(leftMost, control.LeftF)
			Next control

			For Each control As XRControl In tempBand.Controls
				control.LocationF = New PointF(control.LeftF - leftMost, control.TopF - topMost)
			Next control

			Dim layoutBytes() As Byte = Nothing

			Using stream As New MemoryStream()
				tempReport.SaveLayoutToXml(stream)
				layoutBytes = stream.ToArray()
			End Using

			tempReport.Dispose()
			Return layoutBytes
		End Function
	End Class

	' Persistent storage
	Friend Class PersistentControlTemplateStorage
		Inherits ControlTemplateStorageBase
		Implements IControlTemplateStorage

		Private ds As DataSet

		Public Sub New(ByVal path As String)
			ds = New DataSet()
			If File.Exists(path) Then
				ds.ReadXml(path)
			Else
				Dim categories As New DataTable("Categories")
				ds.Tables.Add(categories)

				categories.Columns.Add("CategoryID", GetType(Integer)).AutoIncrement = True
				categories.Columns.Add("CategoryName", GetType(String))

				Dim templates As New DataTable("Templates")
				ds.Tables.Add(templates)

				templates.Columns.Add("CategoryID", GetType(Integer))
				templates.Columns.Add("TemplateName", GetType(String))
				templates.Columns.Add("Layout", GetType(Byte()))

				ds.Relations.Add("CategoryTemplates", categories.Columns("CategoryID"), templates.Columns("CategoryID"))

				ds.WriteXml("SampleStorage.xml", XmlWriteMode.WriteSchema)
			End If
		End Sub

	   Public Sub DeleteTemplate(ByVal categoryName As String, ByVal templateName As String) Implements IControlTemplateStorage.DeleteTemplate
			Dim row As DataRow = ds.Tables("Categories").AsEnumerable().Where(Function(x) x.Field(Of String)("CategoryName") = categoryName).FirstOrDefault()

			Dim catId As Integer = row.Field(Of Integer)("CategoryID")
			Dim templateRow As DataRow = ds.Tables("Templates").AsEnumerable().Where(Function(x) x.Field(Of Integer)("CategoryID") = catId AndAlso x.Field(Of String)("TemplateName") = templateName).FirstOrDefault()
			ds.Tables("Templates").Rows.Remove(templateRow)

			ds.WriteXml("SampleStorage.xml", XmlWriteMode.WriteSchema)
	   End Sub

	   Public Sub DeleteCategory(ByVal categoryName As String) Implements IControlTemplateStorage.DeleteCategory
			Dim row As DataRow = ds.Tables("Categories").AsEnumerable().Where(Function(x) x.Field(Of String)("CategoryName") = categoryName).FirstOrDefault()

			Dim templateRows() As DataRow = row.GetChildRows("CategoryTempaltes")
			For i As Integer = 0 To templateRows.Length - 1
				ds.Tables("Templates").Rows.Remove(templateRows(i))
			Next i

			ds.Tables("Categories").Rows.Remove(row)

			ds.WriteXml("SampleStorage.xml", XmlWriteMode.WriteSchema)
	   End Sub

	   Public Function GetCategoryNames() As String() Implements IControlTemplateStorage.GetCategoryNames
			Return ds.Tables("Categories").AsEnumerable().Select(Function(r) r.Field(Of String)("CategoryName")).ToArray()
	   End Function

	   Public Function GetTemplateNamesForCategory(ByVal categoryName As String) As String() Implements IControlTemplateStorage.GetTemplateNamesForCategory
			Dim row As DataRow = ds.Tables("Categories").AsEnumerable().Where(Function(x) x.Field(Of String)("CategoryName") = categoryName).FirstOrDefault()
			If row IsNot Nothing Then
				Return row.GetChildRows("CategoryTemplates").Select(Function(r) r.Field(Of String)("TemplateName")).ToArray()
			End If

			Return Nothing
	   End Function

	   Public Function Restore(ByVal categoryName As String, ByVal templateName As String) As XRControl() Implements IControlTemplateStorage.Restore
			Dim row As DataRow = ds.Tables("Categories").AsEnumerable().Where(Function(x) x.Field(Of String)("CategoryName") = categoryName).FirstOrDefault()
			If row IsNot Nothing Then
				Dim childRow As DataRow = row.GetChildRows("CategoryTemplates").Where(Function(x) x.Field(Of String)("TemplateName") = templateName).FirstOrDefault()
				Dim layout() As Byte = If(childRow IsNot Nothing, childRow.Field(Of Byte())("Layout"), New Byte(){})
				Return DeserializeControls(layout)
			End If
			Return Nothing
	   End Function

	   Public Sub Store(ByVal categoryName As String, ByVal templateName As String, ByVal controls() As XRControl) Implements IControlTemplateStorage.Store
		   Dim templateLayout() As Byte = SerializeControls(controls)

			Dim row As DataRow = ds.Tables("Categories").AsEnumerable().Where(Function(x) x.Field(Of String)("CategoryName") = categoryName).FirstOrDefault()
			If row Is Nothing Then
				row = ds.Tables("Categories").NewRow()
				row("CategoryName") = categoryName
				ds.Tables("Categories").Rows.Add(row)
			End If

			Dim catId As Integer = row.Field(Of Integer)("CategoryID")
			Dim templateRow As DataRow = ds.Tables("Templates").AsEnumerable().Where(Function(x) x.Field(Of Integer)("CategoryID") = catId AndAlso x.Field(Of String)("TemplateName") = templateName).FirstOrDefault()
			If templateRow Is Nothing Then
				templateRow = ds.Tables("Templates").NewRow()
				templateRow("CategoryID") = catId
				templateRow("TemplateName") = templateName
				ds.Tables("Templates").Rows.Add(templateRow)
			End If
			templateRow("Layout") = templateLayout

			ds.WriteXml("SampleStorage.xml", XmlWriteMode.WriteSchema)
	   End Sub
	End Class

	' In-memory storage
	Friend Class ControlTemplateStorage
		Inherits ControlTemplateStorageBase
		Implements IControlTemplateStorage

		Private Class TemplateLayoutItem
			Public Property Category() As String
			Public Property Name() As String
			Public Property LayoutBytes() As Byte()
		End Class

		Private templates As List(Of TemplateLayoutItem)

		Public Sub New()
			templates = New List(Of TemplateLayoutItem)()
		End Sub

		Public Sub DeleteTemplate(ByVal categoryName As String, ByVal templateName As String) Implements IControlTemplateStorage.DeleteTemplate
			Dim item As TemplateLayoutItem = templates.Where(Function(x) x.Category = categoryName AndAlso x.Name = templateName).FirstOrDefault()
			templates.Remove(item)
		End Sub

		Public Sub DeleteCategory(ByVal categoryName As String) Implements IControlTemplateStorage.DeleteCategory
			Dim items As IEnumerable(Of TemplateLayoutItem) = templates.Where(Function(x) x.Category = categoryName)

			For Each item As TemplateLayoutItem In items
				templates.Remove(item)
			Next item
		End Sub

		Public Function GetCategoryNames() As String() Implements IControlTemplateStorage.GetCategoryNames
			Return templates.Select(Function(x) x.Category).Distinct().ToArray()
		End Function

		Public Function GetTemplateNamesForCategory(ByVal categoryName As String) As String() Implements IControlTemplateStorage.GetTemplateNamesForCategory
			Return templates.Where(Function(x) x.Category = categoryName).Select(Function(y) y.Name).ToArray()
		End Function

		Public Function Restore(ByVal categoryName As String, ByVal templateName As String) As XRControl() Implements IControlTemplateStorage.Restore
			Dim item As TemplateLayoutItem = templates.Where(Function(x) x.Category = categoryName AndAlso x.Name = templateName).FirstOrDefault()
			Dim layout() As Byte = If(item IsNot Nothing, item.LayoutBytes, New Byte(){})
			Return DeserializeControls(layout)
		End Function

		Public Sub Store(ByVal categoryName As String, ByVal templateName As String, ByVal controls() As XRControl) Implements IControlTemplateStorage.Store
			Dim templateLayout() As Byte = SerializeControls(controls)
			Dim item As TemplateLayoutItem = templates.Where(Function(x) x.Category = categoryName AndAlso x.Name = templateName).FirstOrDefault()
			If item Is Nothing Then
				templates.Add(New TemplateLayoutItem() With {.Name = templateName, .Category = categoryName, .LayoutBytes = templateLayout})
			End If
		End Sub
	End Class
End Namespace
