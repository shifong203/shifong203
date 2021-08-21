using ErosSocket;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace Vision2.Project
{
    public partial class NewProjectForm : Form
    {
        public NewProjectForm()
        {
            InitializeComponent();
        }

        public string SeleName = "";
        public string NewName = "";
        public string ProjectName = "";

        private void button1_Click(object sender, System.EventArgs e)
        {
            NewName = textBox1.Text;

            this.Close();
        }

        private void NewProjectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            NewName = "";
            ProjectName = "";
        }

        /// <summary>
        /// 更新程序
        /// </summary>
        public void UpProject()
        {
            try
            {
                tVProject.Nodes.Clear();
                TreeNode ProjectNodeP = tVProject.Nodes.Add(ProjectINI.In.ProjectName);
                ProjectNodeP.Tag = ProjectINI.In;
                ProjectNodeP.ImageKey = ProjectNodeP.SelectedImageKey = "globe-vector.png";
                TreeNode ProjectNode = new TreeNode();
                foreach (var item in ProjectINI.In.Dic_Project_Path)
                {
                    ProjectNode = ProjectNodeP.Nodes.Add(item.Key);
                }
                ProjectNodeP.Toggle();
                tVProject.ExpandAll();
                foreach (var item in ProjectINI.In.GetListRun())
                {
                    item.Value.UpProjectNode(ProjectNode);
                }
                if (!ProjectNode.IsExpanded)
                {
                    ProjectNode.Toggle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenProjectButton_Click(object sender, EventArgs e)
        {
            ProjectForm projectForm = new ProjectForm();
            projectForm.Show();
        }

        private void RedrawProjectButton_Click(object sender, EventArgs e)
        {
            try
            {
                UpProject();
                MainForm1.MainFormF.Up();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveProjectButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ProjectINI.In.SaveProjectAll();

            Cursor = Cursors.Arrow;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            ProjectINI.In.SaveProjectAll(ProjectINI.ProjietPath + "\\Project\\" + ProjectINI.In.ProjectName + "备份");
            foreach (var item in ProjectINI.In.GetListRun())
            {
                item.Value.SaveThis(ProjectINI.ProjietPath + "\\Project\\" + ProjectINI.In.ProjectName + "备份");
            }
            Cursor = Cursors.Arrow;
        }

        public static TreeNode AddNode(TreeNode tree, string name, bool sea = false)
        {
            TreeNode trees;
            TreeNode[] treeNodes = tree.Nodes.Find(name, sea);
            if (treeNodes.Length <= 0)
            {
                trees = tree.Nodes.Add(name);
                trees.Name = name;
                trees.Text = name;
            }
            else
            {
                trees = treeNodes[0];
            }
            return trees;
        }

        private void tVProject_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            try
            {
                if (e.Node == null)
                {
                    return;
                }
                e.Node.EndEdit(true);
                dynamic dst = e.Node.Tag;

                if (dst.Name != null && e.Label != null)
                {
                    string[] namset = e.Label.Split('.');
                    dst.Name = namset[namset.Length - 1];
                }
                else
                {
                    if (dst.Name != null)
                    {
                        dst.Name = e.Node.Text;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 单击按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tVProject_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    return;
                }
                Task.Run(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        Point ClickPoint = new Point(e.X, e.Y);
                        TreeNode CurrentNode = tVProject.GetNodeAt(ClickPoint);

                        if (CurrentNode != null)
                        {
                            groupBox2.Controls.Clear();
                            TreeNode CurrentNodeP = CurrentNode.Parent;
                            ProjectNodet.IClickNodeProject clickNode = CurrentNode.Tag as ProjectNodet.IClickNodeProject;
                            propertyGrid1.SelectedObject = CurrentNode.Tag;
                            groupBox1.Text = CurrentNode.Text;
                            if (clickNode != null)
                            {
                                Control control = clickNode.GetThisControl();
                                if (control != null)
                                {
                                    groupBox2.Text = clickNode.Name;
                                    groupBox2.Controls.Add(control);
                                }
                            }
                        }
                    }));
                });
            }
            catch (Exception)
            {
            }
        }

        private void AddProjectButton_Click(object sender, EventArgs e)
        {
        }

        private void tVProject_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (tVProject.SelectedNode == null)
                {
                    return;
                }
                if (e.KeyCode == Keys.F2)
                {
                    tVProject.SelectedNode.BeginEdit();
                }
            }
            catch (Exception)
            {
            }
        }

        private void NewProjectForm_Load(object sender, EventArgs e)
        {
            UpProject();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            LocalIPForm localIPForm1 = new LocalIPForm();
            localIPForm1.Show();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
        }
    }
}