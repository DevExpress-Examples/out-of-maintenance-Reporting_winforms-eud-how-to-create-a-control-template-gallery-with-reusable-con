<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128605039/15.2.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T341218)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [ControlTemplateDragDropService.cs](./CS/ControlTemplateGallerySample/ControlTemplateGallery/ControlTemplateDragDropService.cs) (VB: [ControlTemplateDragDropService.vb](./VB/ControlTemplateGallerySample/ControlTemplateGallery/ControlTemplateDragDropService.vb))
* [ControlTemplateStorage.cs](./CS/ControlTemplateGallerySample/ControlTemplateGallery/ControlTemplateStorage.cs) (VB: [ControlTemplateStorage.vb](./VB/ControlTemplateGallerySample/ControlTemplateGallery/ControlTemplateStorage.vb))
* [ControlTemplateUserControl.cs](./CS/ControlTemplateGallerySample/ControlTemplateGallery/ControlTemplateUserControl.cs) (VB: [ControlTemplateUserControl.vb](./VB/ControlTemplateGallerySample/ControlTemplateGallery/ControlTemplateUserControl.vb))
* [EditTemplateForm.cs](./CS/ControlTemplateGallerySample/ControlTemplateGallery/EditTemplateForm.cs) (VB: [EditTemplateForm.vb](./VB/ControlTemplateGallerySample/ControlTemplateGallery/EditTemplateForm.vb))
* [ReportDesignToolEx.cs](./CS/ControlTemplateGallerySample/ControlTemplateGallery/ReportDesignToolEx.cs) (VB: [ReportDesignToolEx.vb](./VB/ControlTemplateGallerySample/ControlTemplateGallery/ReportDesignToolEx.vb))
* **[Form1.cs](./CS/ControlTemplateGallerySample/Form1.cs) (VB: [Form1.vb](./VB/ControlTemplateGallerySample/Form1.vb))**
<!-- default file list end -->
# WinForms EUD - How to create a Control Template Gallery with reusable control sets


This example demonstrates how to add the capability to save selected controls and reuse them in this or other reports like UserControls in standard Windows Forms applications. Starting with the version 17.1, the <a href="https://documentation.devexpress.com/#XtraReports/CustomDocument118624">Report Gallery</a> is available in WinForms End-User Report Designer out of the box, so writing any code is no longer required to implement this functionality.<br><br><strong>Implementation Details:</strong><br>In the scope of this example, we add a custom End-User Designer DockPanel and fill it from the predefined storage (if this storage is not empty), which can be passed to the constructor or Storage property of a custom <strong>ReportDesignTool</strong> descendant. The storage should implement the <strong>IControlTemplateStorage</strong> interface, which declares the following members:<br><br>- <strong>GetCategoryNames</strong>. This method should return string representations of all template categories in your storage.<br>- <strong>GetTemplateNamesForCategory</strong>. This method returns template names for the specified category.<br>- <strong>GetData</strong>. This method returns a byte array containing stored controls.<br>- <strong>SetData</strong>. This method allows you to save a byte array representing saved controls.<br>- <strong>DeleteTemplate</strong>. Use this method to delete the specified template from your storage.<br>- <strong>DeleteCategory</strong>. Use this method to delete the specified category and inner templates.<br><br>In this Code example, you can find two sample implementations of this interface. The first one (<strong>ControlTemplateStorage</strong>) demonstrates how to create an in-memory storage, and the second one uses a <strong>DataSet</strong> instance for storing templates.<br><br><strong>UI for Template Gallery:</strong><br>As stated above, we added a custom Dock Panel that has two buttons (<strong>Add</strong> and <strong>Delete</strong>) for operating with templates and a TreeList instance that displays existing categories and templates. To create a new template, select the required controls in your report and click the <strong>Add</strong> button. You will see a new dialog requesting the Category and Template name. <br><br>To create controls from a template, select the required template in the TreeList and drag-and-drop it to a report design surface at the required location.<br><br>To delete a template or category, select it in the TreeList and click the <strong>Delete </strong>button.

<br/>


