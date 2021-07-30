using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.Project.formula;
using Vision2.Project.Mes;

namespace Vision2.捷普
{
    public partial class OperatorFormShow : Form
    {
        public OperatorFormShow()
        {
            InitializeComponent();
        }

        private void OperatorFormShow_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode==Keys.Enter)
                {

                    ErosProjcetDLL.Project.ProjectINI.In.UserID=
                    ((MesJib)RecipeCompiler.Instance.GetMes()).MesData.UserID = textBox1.Text;
                    this.Close();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
