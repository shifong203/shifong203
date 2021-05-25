using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace Vision2.vision
{
    public partial class PMRunControl1 : UserControl
    {
        public PMRunControl1()
        {
            InitializeComponent();
            UpProject();
            //Vision.Instance.UpProjectNode(tVProject.Nodes.Add("图像处理"));

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

        /// <summary>
        /// 双击节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tVProject_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //if (tVProject.SelectedNode != null)
                //{
                //    IUpProjetNode upProjetNode = tVProject.SelectedNode.Tag as IUpProjetNode;
                //    if (upProjetNode != null)
                //    {
                //        TabControlEx tabControlEx = ControlForm as TabControlEx;
                //        if (tabControlEx != null)
                //        {
                //            TabPage tabPage = new TabPage();
                //            int dint = tabControlEx.TabPages.IndexOfKey(upProjetNode.Name);
                //            if (dint > -1)
                //            {
                //                tabControlEx.SelectedIndex = dint;
                //                tabControlEx.Visible = true;
                //                tabPage = tabControlEx.TabPages[dint];
                //            }
                //            else
                //            {
                //                tabPage.AutoScroll = true;
                //                tabPage.Name = tabPage.Text = upProjetNode.Name;
                //                upProjetNode.DoubleClickUpForm(tabPage);
                //                if (tabPage.Controls.Count != 0)
                //                {
                //                    tabControlEx.TabPages.Add(tabPage);
                //                    tabControlEx.SelectedTab = tabPage;
                //                    tabControlEx.Visible = true;
                //                }
                //                else
                //                {
                //                    tabPage.Dispose();
                //                }
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                Task.Run(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        Point ClickPoint = new Point(e.X, e.Y);
                        TreeNode CurrentNode = tVProject.GetNodeAt(ClickPoint);

                        if (CurrentNode != null)
                        {
                            TreeNode CurrentNodeP = CurrentNode.Parent;
                            if (CurrentNodeP != null)
                            {
                                PropertyForm.UPProperty(CurrentNode.Tag, CurrentNode);
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

        private void tVProject_Click(object sender, EventArgs e)
        {

        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                UpProject();


                //tVProject.Nodes.Clear();
                //Vision.Instance.UpProjectNode(tVProject.Nodes.Add("图像处理"));
            }
            catch (Exception)
            {


            }

        }
    }
}
