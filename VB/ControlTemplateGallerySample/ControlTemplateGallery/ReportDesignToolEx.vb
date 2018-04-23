Imports DevExpress.XtraBars.Docking
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.UserDesigner
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ControlTemplateGallerySample
    Public Class ReportDesignToolEx
        Inherits ReportDesignTool


        Private storage_Renamed As IControlTemplateStorage
        Private storageChangedListeners As List(Of ControlTemplateUserControl)

        Public Sub New(ByVal report As XtraReport)
            MyBase.New(report)
            storageChangedListeners = New List(Of ControlTemplateUserControl)()
        End Sub

        Public Sub New(ByVal report As XtraReport, ByVal templateStorage As IControlTemplateStorage)
            Me.New(report)
            Me.storage_Renamed = templateStorage
        End Sub

        Protected Overrides Function CreateDesignForm() As IDesignForm
            Dim form As New XRDesignForm()
            InitializeForm(form)
            Return form
        End Function

        Protected Overrides Function CreateDesignRibbonForm() As IDesignForm
            Dim form As New XRDesignRibbonForm()
            InitializeForm(form)
            Return form
        End Function

        Public Function GetTemplateControls(ByVal categoryName As String, ByVal templateName As String) As XRControl()
            Return ControlTemplateStorage.GetTemplateControls(Storage, categoryName, templateName)
        End Function

        Private Sub InitializeForm(ByVal designForm As IDesignForm)
            Dim dockManager As XRDesignDockManager = designForm.DesignDockManager
            If dockManager IsNot Nothing Then
                Dim dockPanel As New DockPanel()
                dockPanel.Text = "Control Templates"
                dockManager.AddPanel(DockingStyle.Right, dockPanel)
                InitializePanelContent(designForm.DesignMdiController, dockPanel)

                Dim reportExplorer As DockPanel = dockManager(DesignDockPanelType.ReportExplorer)

                dockPanel.DockAsTab(reportExplorer)
                dockManager.ActivePanel = reportExplorer
            End If
        End Sub

        Protected Overridable Sub InitializePanelContent(ByVal controller As XRDesignMdiController, ByVal panel As DockPanel)
            Dim userControl As New ControlTemplateUserControl(controller)
            storageChangedListeners.Add(userControl)
            panel.Controls.Add(userControl)
            userControl.Storage = Storage
            userControl.Dock = System.Windows.Forms.DockStyle.Fill

            controller.DesignPanelListeners.Add(userControl)
        End Sub

        Public Overrides Sub Dispose()
            storage_Renamed = Nothing

            If storageChangedListeners IsNot Nothing Then
                Dim i As Integer = 0
                Do While i < storageChangedListeners.Count
                    storageChangedListeners(i).Dispose()
                    i += 1
                Loop

                storageChangedListeners.Clear()
                storageChangedListeners = Nothing
            End If

            MyBase.Dispose()
        End Sub

        Public Property Storage() As IControlTemplateStorage
            Get
                If storage_Renamed Is Nothing Then
                    storage_Renamed = New ControlTemplateStorage()
                End If
                Return storage_Renamed
            End Get

            Set(ByVal value As IControlTemplateStorage)
                If storage_Renamed IsNot value Then
                    storage_Renamed = value

                    For i As Integer = 0 To storageChangedListeners.Count - 1
                        storageChangedListeners(i).Storage = storage_Renamed
                    Next i
                End If
            End Set
        End Property
    End Class
End Namespace
