using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ControlTemplateGallerySample
{
    public partial class EditTemplateForm : DevExpress.XtraEditors.XtraForm
    {
        public EditTemplateForm()
        {
            InitializeComponent();
        }

        public string CategoryName 
        {
            get { return teCategoryName.Text; }
            set { teCategoryName.Text = value; }
        }

        public string TemplateName
        {
            get { return teTemplateName.Text; }
            set { teTemplateName.Text = value; }
        }
    }
}