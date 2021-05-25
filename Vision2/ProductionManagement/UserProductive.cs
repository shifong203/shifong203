using System;
using System.Windows.Forms;
using Vision2.Project.formula;

namespace Vision2.ProductionManagement
{
    public partial class UserProductive : UserControl
    {
        public UserProductive()
        {
            InitializeComponent();
        }
        FormulaForm formulaForm;
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (formulaForm == null)
            {
                formulaForm = new FormulaForm();
            }
            Vision2.ErosProjcetDLL.UI.UICon.WindosFormerShow(ref formulaForm);

        }

        public void Up()
        {

        }
    }
}
