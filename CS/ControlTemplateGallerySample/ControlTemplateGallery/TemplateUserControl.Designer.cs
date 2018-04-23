namespace ControlTemplateGallerySample
{
    partial class TemplateUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateUserControl));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.tlTemplates = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlTemplates)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnDelete);
            this.panelControl1.Controls.Add(this.btnAdd);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(254, 30);
            this.panelControl1.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDelete.Location = new System.Drawing.Point(30, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(20, 20);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "simpleButton3";
            // 
            // btnAdd
            // 
            this.btnAdd.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnAdd.Location = new System.Drawing.Point(5, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(20, 20);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "simpleButton1";
            // 
            // tlTemplates
            // 
            this.tlTemplates.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.tlTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlTemplates.Location = new System.Drawing.Point(0, 30);
            this.tlTemplates.Name = "tlTemplates";
            this.tlTemplates.BeginUnboundLoad();
            this.tlTemplates.AppendNode(new object[] {
            "Test"}, -1);
            this.tlTemplates.AppendNode(new object[] {
            "Test2"}, 0);
            this.tlTemplates.EndUnboundLoad();
            this.tlTemplates.OptionsBehavior.Editable = false;
            this.tlTemplates.OptionsDragAndDrop.DragNodesMode = DevExpress.XtraTreeList.DragNodesMode.Single;
            this.tlTemplates.OptionsSelection.InvertSelection = true;
            this.tlTemplates.OptionsView.ShowColumns = false;
            this.tlTemplates.OptionsView.ShowHorzLines = false;
            this.tlTemplates.OptionsView.ShowIndicator = false;
            this.tlTemplates.OptionsView.ShowVertLines = false;
            this.tlTemplates.Size = new System.Drawing.Size(254, 328);
            this.tlTemplates.TabIndex = 0;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.MinWidth = 70;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // TemplateUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlTemplates);
            this.Controls.Add(this.panelControl1);
            this.Name = "TemplateUserControl";
            this.Size = new System.Drawing.Size(254, 358);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tlTemplates)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraTreeList.TreeList tlTemplates;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
    }
}
