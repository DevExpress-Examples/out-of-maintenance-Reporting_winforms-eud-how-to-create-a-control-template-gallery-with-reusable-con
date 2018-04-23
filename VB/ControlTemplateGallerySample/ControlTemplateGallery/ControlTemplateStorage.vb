Imports DevExpress.XtraReports.UI
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text

Namespace ControlTemplateGallerySample
    Public Interface IControlTemplateStorage
        Function GetCategoryNames() As String()
        Function GetTemplateNamesForCategory(ByVal categoryName As String) As String()
        Function GetData(ByVal categoryName As String, ByVal templateName As String) As Byte()
        Sub SetData(ByVal categoryName As String, ByVal templateName As String, ByVal templateLayout() As Byte)
        Sub DeleteTemplate(ByVal categoryName As String, ByVal templateName As String)
        Sub DeleteCategory(ByVal categoryName As String)
    End Interface

    'Default in-memory storage
    Public Class ControlTemplateStorage
        Implements IControlTemplateStorage

        Private templates As List(Of TemplateLayoutItem)

        Public Sub New()
            templates = New List(Of TemplateLayoutItem)()
        End Sub

        Friend Shared Function GetTemplateControls(ByVal storage As IControlTemplateStorage, ByVal categoryName As String, ByVal templateName As String) As XRControl()
            Dim layoutBytes() As Byte = storage.GetData(categoryName, templateName)

            If layoutBytes IsNot Nothing AndAlso layoutBytes.Length > 0 Then
                Dim tempReport As New XtraReport()
                Using stream As New MemoryStream(layoutBytes)
                    tempReport.LoadLayoutFromXml(stream)
                End Using

                Return tempReport.Bands(BandKind.Detail).Controls.Cast(Of XRControl)().ToArray()
            End If

            Return Nothing
        End Function

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

        Public Function GetData(ByVal categoryName As String, ByVal templateName As String) As Byte() Implements IControlTemplateStorage.GetData
            Dim item As TemplateLayoutItem = templates.Where(Function(x) x.Category = categoryName AndAlso x.Name = templateName).FirstOrDefault()
            Return If(item Is Nothing, Nothing, item.LayoutBytes)
        End Function

        Public Sub SetData(ByVal categoryName As String, ByVal templateName As String, ByVal templateLayout() As Byte) Implements IControlTemplateStorage.SetData
            Dim item As TemplateLayoutItem = templates.Where(Function(x) x.Category = categoryName AndAlso x.Name = templateName).FirstOrDefault()
            If item Is Nothing Then
                templates.Add(New TemplateLayoutItem() With {.Name = templateName, .Category = categoryName, .LayoutBytes = templateLayout})
            End If
        End Sub

        Private Class TemplateLayoutItem
            Public Property Category() As String
            Public Property Name() As String
            Public Property LayoutBytes() As Byte()
        End Class
    End Class
End Namespace
