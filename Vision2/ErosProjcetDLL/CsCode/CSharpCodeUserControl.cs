using System;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.CsCode
{
    public partial class CSharpCodeUserControl : UserControl
    {
        public CSharpCodeUserControl()
        {
            InitializeComponent();
        }

        private Project.ProjectC obj;

        public void UpCSharpCode(Project.ProjectC projectObj)
        {
            try
            {
                obj = projectObj;
                richTextBox1.Lines = obj.GetCode();
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                obj.SaveCode(richTextBox1.Lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Project.ProjectINI.ProjectPathRun;
            openFileDialog.Filter = "脚本文件|*Cs;*.Cs|Txt文件|*.Txt";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                richTextBox1.Lines = System.IO.File.ReadAllLines(openFileDialog.FileName);
            }
        }
    }
}