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

Namespace ControlTemplateGallerySample
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
            Dim report As New XtraReport1()
            Using designTool As New ReportDesignToolEx(report, New SampleControlTemplateStorage())
                designTool.ShowDesignerDialog()
            End Using
        End Sub
    End Class

    Public Class SampleControlTemplateStorage
        Implements IControlTemplateStorage

        Private ds As DataSet

        Public Sub New()
            ds = New DataSet()
            If File.Exists("SampleStorage.xml") Then
                ds.ReadXml("SampleStorage.xml")
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
            If XtraMessageBox.Show("Are you sure you want to delete this template?", "Confirm Deleting", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Dim row As DataRow = ds.Tables("Categories").AsEnumerable().Where(Function(x) x.Field(Of String)("CategoryName") = categoryName).FirstOrDefault()

                Dim catId As Integer = row.Field(Of Integer)("CategoryID")
                Dim templateRow As DataRow = ds.Tables("Templates").AsEnumerable().Where(Function(x) x.Field(Of Integer)("CategoryID") = catId AndAlso x.Field(Of String)("TemplateName") = templateName).FirstOrDefault()
                ds.Tables("Templates").Rows.Remove(templateRow)

                ds.WriteXml("SampleStorage.xml", XmlWriteMode.WriteSchema)
            End If
        End Sub

        Public Sub DeleteCategory(ByVal categoryName As String) Implements IControlTemplateStorage.DeleteCategory

            If XtraMessageBox.Show("Are you sure you want to delete this category?", "Confirm Deleting", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Dim row As DataRow = ds.Tables("Categories").AsEnumerable().Where(Function(x) x.Field(Of String)("CategoryName") = categoryName).FirstOrDefault()

                Dim templateRows() As DataRow = row.GetChildRows("CategoryTempaltes")
                For i As Integer = 0 To templateRows.Length - 1
                    ds.Tables("Templates").Rows.Remove(templateRows(i))
                Next i

                ds.Tables("Categories").Rows.Remove(row)

                ds.WriteXml("SampleStorage.xml", XmlWriteMode.WriteSchema)
            End If
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

        Public Function GetData(ByVal categoryName As String, ByVal templateName As String) As Byte() Implements IControlTemplateStorage.GetData
            Dim row As DataRow = ds.Tables("Categories").AsEnumerable().Where(Function(x) x.Field(Of String)("CategoryName") = categoryName).FirstOrDefault()
            If row IsNot Nothing Then
                Dim childRow As DataRow = row.GetChildRows("CategoryTemplates").Where(Function(x) x.Field(Of String)("TemplateName") = templateName).FirstOrDefault()
                Return If(childRow Is Nothing, Nothing, childRow.Field(Of Byte())("Layout"))
            End If
            Return Nothing
        End Function

        Public Sub SetData(ByVal categoryName As String, ByVal templateName As String, ByVal templateLayout() As Byte) Implements IControlTemplateStorage.SetData
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
End Namespace
