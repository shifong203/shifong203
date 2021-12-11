using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class ProjectNodet : Form
    {
        /// <summary>
        /// 单击程序字段接口
        /// </summary>
        public interface IClickNodeProject
        {
            /// <summary>
            /// 左键单击属性界面显示
            /// </summary>
            //void UpProperty(PropertyForm pertyForm, object data = null);
            Control GetThisControl();

            string Name { get; }
            /// <summary>
            /// 右键单击
            /// </summary>
            /// <param name="contextMenuStrip"></param>
            //void MouseButtonsLeft(ContextMenuStrip contextMenuStrip,TreeView treeView);
        }

        /// <summary>
        /// 界面程序结构，刷新，保存，添加等
        /// </summary>
        public interface IUpProjetNode : IDoubleClickUpForm
        {
            string Name { get; }

            /// <summary>
            /// 程序类型名
            /// </summary>
            string ProjectTypeName { get; }

            /// <summary>
            /// 后缀名
            /// </summary>
            string Text { get; set; }

            /// <summary>
            /// 文件夹名称
            /// </summary>
            string FileName { get; }

            /// <summary>
            /// 后缀名
            /// </summary>
            string SuffixName { get; }

            /// <summary>
            /// 说明信息
            /// </summary>
            string Information { get; set; }

            ///// <summary>
            ///// 刷新实例到程序Nobet
            ///// </summary>
            //void UpProjectNode(TreeNode treeNode);
            /// <summary>
            /// 保存到项目地址
            /// </summary>
            /// <param name="name"></param>
            void SaveThis(string path);

            /// <summary>
            /// 读取Josn到实例
            /// </summary>
            /// <param name="path"></param>
            T ReadThis<T>(string path, T obj = default(T)) where T : new();

            /// <summary>
            ///
            /// </summary>
            void Close();
        }

        /// <summary>
        /// 双击显示窗体接口
        /// </summary>
        public interface IDoubleClickUpForm
        {
            /// <summary>
            /// 双击窗体界面显示
            /// </summary>
            /// <param name="data">关联数据</param>
            /// <param name="tabPage">输入Tab</param>
            /// <returns></returns>
            void DoubleClickUpForm(TabPage tabPage, object data = null);
        }

        /// <summary>
        /// 保存界面参数到对象接口
        /// </summary>
        public interface ISaveUIData
        {
            /// <summary>
            /// 保存参数到控件,动态集合参数，绑定到DataGridView,
            /// </summary>
            /// <param name="Control"></param>
            void SaveData(Control Control);

            /// <summary>
            /// 显示参数,将参数显示到控件
            /// </summary>
            /// <param name="Control"></param>
            void UpData(Control Control);
        }

        public static ProjectNodet FormThis;

        public ProjectNodet(Control controlNode, Control controlForm)
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            FormThis = this;
            ControlNode = controlNode;
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            controlNode.Controls.Add(this);
            ControlForm = controlForm;
            tVProject.HideSelection = false;
            ////自已绘制
            //this.tVProject.DrawMode = TreeViewDrawMode.OwnerDrawText;
            //this.tVProject.DrawNode += new DrawTreeNodeEventHandler(treeView1_DrawNode);
            this.UpProject();
        }

        //private bool isDrawNod;

        //在绘制节点事件中，按自已想的绘制

        private List<TreeNode> ListNodes = new List<TreeNode>();

        /// <summary>
        /// Node父容器
        /// </summary>
        private Control ControlNode;

        /// <summary>
        /// Form父容器
        /// </summary>
        private Control ControlForm;

        public ProjectNodet(string path, Control contro) : this(contro, FormThis.ControlForm)
        {
            UpProject();
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

        /// <summary>
        /// 单击节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tVProject_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //Point ClickPoint = new Point(e.X, e.Y);
                //TreeNode CurrentNode = tVProject.GetNodeAt(ClickPoint);
                //isDrawNod = true;
                //if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Control)
                //{
                //    ListNodes.Add(CurrentNode);
                //}
                //else if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Shift)
                //{
                //    if (ListNodes[0] != null)
                //    {
                //        for (int i = 0; ListNodes.Count > 1; i++)
                //        {
                //            ListNodes.RemoveAt(1);
                //        }
                //        Point Point = ListNodes[0].Bounds.Location;
                //        if (ListNodes[0].Bounds.Location.Y > ClickPoint.Y)
                //        {
                //            Point = CurrentNode.Bounds.Location;
                //            ClickPoint = ListNodes[0].Bounds.Location;
                //        }

                //        for (Point.Y = Point.Y + ListNodes[0].Bounds.Height; Point.Y < ClickPoint.Y; Point.Y = Point.Y + ListNodes[0].Bounds.Height)
                //        {
                //            ListNodes.Add(tVProject.GetNodeAt(Point));
                //        }
                //    }
                //    else
                //    {
                //        ListNodes.Add(CurrentNode);
                //    }
                //}
                //else if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                //{
                //    if (e.Button == MouseButtons.Right)
                //    {
                //        tVProject.SelectedNode = CurrentNode;
                //    }
                //    ListNodes.Clear();
                //    ListNodes.Add(CurrentNode);
                //}
                //isDrawNod = false;

                //tVProject.SelectedNode = CurrentNode;//选中这个节点
            }
            catch (Exception ex)
            {
                //isDrawNod = false;
                MessageBox.Show(ex.Message);
            }
        }

        private void tVProject_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
            }
            catch (System.Exception ex)
            {
            }
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
        /// 遍历键值到节点
        /// </summary>
        /// <param name="project"></param>
        /// <param name="tree"></param>
        private void ProjectCalssNet(ProjectObj project, TreeNode tree)
        {
            if (project.ProjectClass != null)
            {
                foreach (var itemk in project.ProjectClass)
                {
                    TreeNode tree2;
                    tree2 = AddNode(tree, itemk.Key);
                    tree2.Tag = itemk.Value;
                    foreach (var item2 in itemk.Value.Keys)
                    {
                        TreeNode tree3;
                        tree3 = AddNode(tree2, item2.ToString());
                        tree3.Tag = itemk.Value[item2.ToString()];
                        if (tree3.Tag is ProjectObj)
                        {
                            ProjectObj projectObj = itemk.Value[item2.ToString()] as ProjectObj;
                            ProjectCalssNet(projectObj, tree3);
                        }
                        //tree3.Tag = itemk.Value[item2.ToString()];
                        if (!tree3.IsExpanded)
                        {
                            tree3.Toggle();
                        }
                    }
                    if (!tree2.IsExpanded)
                    {
                        tree2.Toggle();
                    }
                }
            }
        }

        /// <summary>
        /// 查询添加唯一子节点
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="nodeName"></param>
        /// <param name="sea"></param>
        /// <returns></returns>
        public static TreeNode AddNode(TreeNode tree, ProjectObj obj, bool sea = false)
        {
            TreeNode trees;

            TreeNode[] treeNodes = tree.Nodes.Find(obj.Name, sea);
            if (treeNodes.Length <= 0)
            {
                trees = tree.Nodes.Add(obj.Name);
                trees.Name = obj.Name;
                trees.Text = obj.Text;
            }
            else
            {
                trees = treeNodes[0];
            }
            return trees;
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

        private void SaveProjectButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ProjectINI.In.SaveProjectAll();

            Cursor = Cursors.WaitCursor;
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

        private void OpenProjectButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = ProjectINI.ProjectPathRun;
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
            }
        }

        private void RedrawProjectButton_Click(object sender, EventArgs e)
        {
            try
            {
                UpProject();
                //MainForm1.MainFormF.Up();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ProjectINI.In.SaveProjectAll();

            Cursor = Cursors.Arrow;
        }
    }
}