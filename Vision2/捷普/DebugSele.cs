using System;
using System.Windows.Forms;

namespace Vision2.捷普
{
    public partial class DebugSele : Form
    {
        public DebugSele()
        {
            InitializeComponent();
            listBox1.SelectedIndex = 0;
        }

        private void DebugSele_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (listBox1.SelectedIndex == 1)
                    {
                        ErosProjcetDLL.Project.ProjectINI.DebugMode = true;
                    }
                    else
                    {
                        ErosProjcetDLL.Project.ProjectINI.DebugMode = false;
                    }
                    this.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex == 1)
                {
                    ErosProjcetDLL.Project.ProjectINI.DebugMode = true;
                }
                else
                {
                    ErosProjcetDLL.Project.ProjectINI.DebugMode = false;
                }
                this.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}