using System;
using System.Windows.Forms;

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
                if (e.KeyCode == Keys.Enter)
                {
                    ErosProjcetDLL.Project.ProjectINI.In.UserID = textBox1.Text;
                    this.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}