using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision
{
    public partial class LibraryForm1 : Form
    {
        public LibraryForm1()
        {
            InitializeComponent();
            toolStripCheckbox1.GetBase().Checked = true;
        }

        //public  static string PathStr = ErosProjcetDLL.Project. ErosProjcetDLL.Project.ProjectINI.ProjectPathRun +"\\Library\\Vision\\";
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
                //PathStr = ErosProjcetDLL.Project. ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\Library\\Vision\\"+Project.formula.Product.ProductionName + "\\";
                Directory.CreateDirectory(Library.LibraryBasics.PathStr);
                string[] files = Directory.GetDirectories(Library.LibraryBasics.PathStr);
                treeView1.Nodes.Clear();
                TreeNode treeNode = treeView1.Nodes.Add("视觉库");
                foreach (var item in Vision.GetLibrary())
                {
                    TreeNode treeNodeLD = treeNode.Nodes.Add(item.Key);
                    treeNodeLD.Tag = item.Value;
                }
                treeNode.ExpandAll();
                TreeNode treeNodeTV = treeView1.Nodes.Add("全局库");
                for (int i = 0; i < files.Length; i++)
                {
                    treeNodeTV.Nodes.Add(Path.GetFileNameWithoutExtension(files[i]));
                }
                string staSET = "                                                          ";
                ErosProjcetDLL.Excel.Npoi.GetPrivateProfileString("视觉库", null, "", staSET, 500, Library.LibraryBasics.RunPath + "\\Library.ini");
                string[] vs = staSET.ToString().Split('\0');
                StringBuilder stdt = new StringBuilder(100);
                for (int i = 0; i < vs.Length; i++)
                {
                    if (vs[i].Trim() != "")
                    {
                        ErosProjcetDLL.Excel.Npoi.GetPrivateProfileString("视觉库", vs[i], "", stdt, 500, Library.LibraryBasics.RunPath + "\\Library.ini");
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                //Point ClickPoint = new Point(e.X, e.Y);
                TreeNode CurrentNode = e.Node;
                if (CurrentNode.Tag is RunProgram)
                {
                    RunProgram runProgram = CurrentNode.Tag as RunProgram;
                    tabPage1.Controls.Clear();
                    tabPage1.Controls.Add(runProgram.GetControl(Vision.GetRunNameVision()));
                }
            }
            catch (Exception)
            {
            }
        }
    }
}