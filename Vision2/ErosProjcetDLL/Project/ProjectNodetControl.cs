using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ErosProjcetDLL.Project.ProjectNodet;
using ErosProjcetDLL.UI;

namespace ErosProjcetDLL.Project
{
    public partial class ProjectNodetControl : UserControl
    {
        public ProjectNodetControl()
        {
            InitializeComponent();
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun;
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
            }
        }

        private void AddProjectButton_Click(object sender, EventArgs e)
        {

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

            ProjectINI.In.SaveProjectAll(Application.StartupPath + "\\Project\\" + ProjectINI.In.ProjectName + "\\" + ProjectINI.In.RunName + "备份");
            foreach (var item in ProjectINI.In.GetListRun())
            {
                item.Value.SaveThis(Application.StartupPath + "\\Project\\" + ProjectINI.In.ProjectName + "\\" + ProjectINI.In.RunName + "备份");
            }
            Cursor = Cursors.Arrow;
        }

        private void tVProject_KeyDown(object sender, KeyEventArgs e)
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
                Task.Run(() => {
                    this.Invoke(new Action(() => {
                        Point ClickPoint = new Point(e.X, e.Y);
                        TreeNode CurrentNode = tVProject.GetNodeAt(ClickPoint);

                        if (CurrentNode != null)
                        {
                            TreeNode CurrentNodeP = CurrentNode.Parent;
                            if (CurrentNodeP != null)
                            {
                                PropertyForm.UPProperty(CurrentNode.Tag,  CurrentNode);
                            }
                            else
                            {
                                PropertyForm.UPProperty(CurrentNode.Tag);
                            }

                        }
                    }));

                });

            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Form父容器
        /// </summary>
        private Control ControlForm;

        /// <summary>
        /// 双击节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tVProject_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (tVProject.SelectedNode != null)
                {
                    IUpProjetNode upProjetNode = tVProject.SelectedNode.Tag as IUpProjetNode;
                    if (upProjetNode != null)
                    {
                        TabControlEx tabControlEx = ControlForm as TabControlEx;
                        if (tabControlEx != null)
                        {
                            TabPage tabPage = new TabPage();
                            int dint = tabControlEx.TabPages.IndexOfKey(upProjetNode.Name);
                            if (dint > -1)
                            {
                                tabControlEx.SelectedIndex = dint;
                                tabControlEx.Visible = true;
                                tabPage = tabControlEx.TabPages[dint];
                            }
                            else
                            {
                                tabPage.AutoScroll = true;
                                tabPage.Name = tabPage.Text = upProjetNode.Name;
                                upProjetNode.DoubleClickUpForm(tabPage);
                                if (tabPage.Controls.Count != 0)
                                {
                                    tabControlEx.TabPages.Add(tabPage);
                                    tabControlEx.SelectedTab = tabPage;
                                    tabControlEx.Visible = true;
                                }
                                else
                                {
                                    tabPage.Dispose();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
