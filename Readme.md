<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128605039/17.1.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T341218)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<p></ br><b>With version 17.1, we introduced the Report Gallery out of the box. It lets you store report controls, data sources, layouts and styles, and keep them for later use. Click the following links for more details. <br>
  <a href="https://www.youtube.com/watch?v=SEaEMVFoIAw">Video: Reporting: The Report Gallery</a> <br>
  <a href="https://docs.devexpress.com/XtraReports/118624/Visual-Studio-Report-Designer/Dock-Panels-and-Designer-Options/Report-Gallery">Documentation: Report Gallery</a>
</b></p></br></br></br>


<!-- default file list -->
*Files to look at*:

* [DragDropService.cs](./CS/ControlTemplateGallerySample/ControlTemplateGallery/DragDropService.cs) (VB: [DragDropService.vb](./VB/ControlTemplateGallerySample/ControlTemplateGallery/DragDropService.vb))
* [TemplateEditorForm.cs](./CS/ControlTemplateGallerySample/ControlTemplateGallery/TemplateEditorForm.cs) (VB: [TemplateEditorForm.vb](./VB/ControlTemplateGallerySample/ControlTemplateGallery/TemplateEditorForm.vb))
* [TemplateStorage.cs](./CS/ControlTemplateGallerySample/ControlTemplateGallery/TemplateStorage.cs) (VB: [TemplateStorage.vb](./VB/ControlTemplateGallerySample/ControlTemplateGallery/TemplateStorage.vb))
* [TemplateUserControl.cs](./CS/ControlTemplateGallerySample/ControlTemplateGallery/TemplateUserControl.cs) (VB: [TemplateUserControl.vb](./VB/ControlTemplateGallerySample/ControlTemplateGallery/TemplateUserControl.vb))
* **[Form1.cs](./CS/ControlTemplateGallerySample/Form1.cs) (VB: [Form1.vb](./VB/ControlTemplateGallerySample/Form1.vb))**
<!-- default file list end -->
# WinForms EUD - How to create a Control Template Gallery with reusable control sets

This example demonstrates how to add the capability to save selected controls and reuse them in this or other reports like UserControls in standard Windows Forms applications. Starting with the version 17.1, the <a href="https://documentation.devexpress.com/#XtraReports/CustomDocument118624">Report Gallery</a> is available in WinForms End-User Report Designer out of the box, so writing any code is no longer required to implement this functionality.<br><br><strong>Implementation Details:</strong><br>In the scope of this example, we add a custom End-User Designer DockPanel and fill it from the predefined storage (if this storage is not empty), which can be passed to the constructor or Storage property of a custom <strong>ReportDesignTool</strong> descendant. The storage should implement the <strong>IControlTemplateStorage</strong> interface, which declares the following members:<br><br>- <strong>GetCategoryNames</strong>. This method should return string representations of all template categories in your storage.<br>- <strong>GetTemplateNamesForCategory</strong>. This method returns template names for the specified category.<br>- <strong>GetData</strong>. This method returns a byte array containing stored controls.<br>- <strong>SetData</strong>. This method allows you to save a byte array representing saved controls.<br>- <strong>DeleteTemplate</strong>. Use this method to delete the specified template from your storage.<br>- <strong>DeleteCategory</strong>. Use this method to delete the specified category and inner templates.<br><br>In this Code example, you can find two sample implementations of this interface. The first one (<strong>ControlTemplateStorage</strong>) demonstrates how to create an in-memory storage, and the second one uses a <strong>DataSet</strong> instance for storing templates.<br><br><strong>UI for Template Gallery:</strong><br>As stated above, we added a custom Dock Panel that has two buttons (<strong>Add</strong> and <strong>Delete</strong>) for operating with templates and a TreeList instance that displays existing categories and templates. To create a new template, select the required controls in your report and click the <strong>Add</strong> button. You will see a new dialog requesting the Category and Template name. <br><br>To create controls from a template, select the required template in the TreeList and drag-and-drop it to a report design surface at the required location.<br><br>To delete a template or category, select it in the TreeList and click the <strong>Delete </strong>button.

<br/>


