using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.Project.formula;

namespace Vision2.Project
{
    public partial class ProjectForm : Form
    {
        public ProjectForm()
        {
            InitializeComponent();
        }

        private void ProjectForm_Load(object sender, EventArgs e)
        {
            this.UpDataP();
        }

        // Token: 0x060008E3 RID: 2275 RVA: 0x00006B3F File Offset: 0x00004D3F
        private void 新建项目ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private bool isCheand = true;

        // Token: 0x060008E4 RID: 2276 RVA: 0x00077628 File Offset: 0x00075828
        private void UpDataP()
        {
            try
            {
                this.isCheand = true;
                this.toolStripComboBox1.Items.Clear();
                this.treeView1.CheckBoxes = false;
                this.treeView1.Nodes.Clear();
                this.treeNode = this.treeView1.Nodes.Add("解决方案");
                this.treeNode.ImageIndex = (this.treeNode.SelectedImageIndex = 0);

                string[] dat = Directory.GetDirectories(ProjectINI.ProjietPath + "Project\\");
                for (int i = 0; i < dat.Length; i++)
                {
                    TreeNode treeNode = this.treeNode.Nodes.Add(Path.GetFileName(dat[i]));
                    treeNode.Name = treeNode.Text;
                    bool flag = Path.GetFileName(dat[i]) == ProjectINI.In.ProjectName;
                    if (flag)
                    {
                        treeNode.ImageIndex = (treeNode.SelectedImageIndex = 1);
                    }
                    else
                    {
                        treeNode.ImageIndex = (treeNode.SelectedImageIndex = 2);
                    }
                    this.toolStripComboBox1.Items.Add(Path.GetFileName(dat[i]));
                }
                this.toolStripComboBox1.SelectedItem = ProjectINI.In.ProjectName;
                this.treeNode.ExpandAll();
                propertyGrid1.SelectedObject = ProjectINI.In;
            }
            catch (Exception)
            {
            }
            this.isCheand = false;
        }

        // Token: 0x060008E5 RID: 2277 RVA: 0x00077788 File Offset: 0x00075988
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = ProjectINI.ProjietPath + "Project\\";
                saveFileDialog.FileName = "解决方案";
                bool flag = saveFileDialog.ShowDialog() == DialogResult.OK;
                if (flag)
                {
                    ProjectINI.In.SaveProject(Path.GetFileName(saveFileDialog.FileName));

                    Product.SaveDicPrExcel(ProjectINI.ProjietPath + "Project\\" + Path.GetFileName(saveFileDialog.FileName) + "\\AppRun1\\产品配方\\产品配方管理\\产品文件");
                    this.UpDataP();
                }
            }
            catch (Exception)
            {
            }
        }

        // Token: 0x060008E6 RID: 2278 RVA: 0x00077828 File Offset: 0x00075A28
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ProjectINI.In.SaveThis();
        }

        // Token: 0x060008E7 RID: 2279 RVA: 0x00077837 File Offset: 0x00075A37
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
            this.Restart();
        }

        // Token: 0x060008E8 RID: 2280 RVA: 0x00077848 File Offset: 0x00075A48
        private void Restart()
        {
            Thread thtmp = new Thread(new ParameterizedThreadStart(this.run));
            object appName = Application.ExecutablePath;
            Thread.Sleep(2000);
            thtmp.Start(appName);
        }

        // Token: 0x060008E9 RID: 2281 RVA: 0x00077884 File Offset: 0x00075A84
        private void run(object obj)
        {
            new Process
            {
                StartInfo =
                {
                    FileName = obj.ToString()
                }
            }.Start();
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool flag = this.isCheand;
                if (!flag)
                {
                    TreeNode[] treeNode = this.treeNode.Nodes.Find(ProjectINI.In.ProjectName, false);
                    if (treeNode.Length == 1)
                    {
                        treeNode[0].ImageIndex = (treeNode[0].SelectedImageIndex = 2);
                    }
                    if (ProjectINI.In.ProjectName != this.toolStripComboBox1.SelectedItem.ToString())
                    {
                        ProjectINI.In.ProjectName = this.toolStripComboBox1.SelectedItem.ToString();

                        treeNode = this.treeNode.Nodes.Find(ProjectINI.In.ProjectName, false);
                        if (treeNode.Length == 1)
                        {
                            treeNode[0].ImageIndex = (treeNode[0].SelectedImageIndex = 1);
                        }

                        ProjectINI.In.SaveThis();
                        MessageBox.Show("已成功切换解决方案！将重新启动程序");
                        Application.ExitThread();
                        this.Restart();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}