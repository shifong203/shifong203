using System;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace Vision2.vision
{
    public partial class LibraryForm1 : Form
    {
        public LibraryForm1()
        {
            InitializeComponent();
            toolStripCheckbox1.GetBase().Checked = true;
        }

        public  static string PathStr = ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun +"\\Library\\Vision\\";
        private void LibraryForm1_Load(object sender, EventArgs e)
        {
            try
            {
                UPData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void UPData()
        {
            try
            {
                Directory.CreateDirectory(PathStr);
                string[] files = Directory.GetDirectories(PathStr);
                treeView1.Nodes.Clear();
                TreeNode treeNode= treeView1.Nodes.Add("视觉库");
                for (int i = 0; i < files.Length; i++)
                {
                    treeNode.Nodes.Add(Path.GetFileNameWithoutExtension(files[i]));
                }
                //StringBuilder staSET = new StringBuilder(100);
                string staSET = "                                                          ";

                ErosProjcetDLL.Excel.Npoi.GetPrivateProfileString("视觉库", null, "", staSET, 500, PathStr + "\\Library.ini");
                string[] vs = staSET.ToString().Split('\0');
                StringBuilder stdt = new StringBuilder(100);
                for (int i = 0; i < vs.Length; i++)
                {
                    if (vs[i].Trim()!="")
                    {
                        ErosProjcetDLL.Excel.Npoi.GetPrivateProfileString("视觉库", vs[i], "", stdt, 500, PathStr + "\\Library.ini");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            UPData();
        }

        private void toolStripCheckbox1_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Visible = toolStripCheckbox1.GetBase().Checked;
            }
            catch (Exception)
            {
            }
        }
    }
}
