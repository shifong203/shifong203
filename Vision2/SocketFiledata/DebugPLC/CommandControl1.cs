using ErosSocket.DebugPLC.PLC;
using ErosSocket.DebugPLC.Robot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace ErosSocket.DebugPLC
{
    public partial class CommandControl1 : System.Windows.Forms.UserControl
    {
        private TreeNode treeNodeAx = new TreeNode() { Text = "轴集合", Name = "轴集合" };
        private TreeNode treeNodeCyl = new TreeNode() { Text = "气缸集合", Name = "气缸集合" };
        private TreeNode treeNodeAxisP = new TreeNode() { Text = "轴组合", Name = "轴组合" };
        private TreeNode treeNodeRobotS = new TreeNode() { Text = "Robot", Name = "Robot" };
        private TreeNode treeNodeIO = new TreeNode() { Text = "IO集合", Name = "IO集合" };
        private TreeNode treeNodePrun = new TreeNode() { Text = "轨迹文件", Name = "轨迹文件" };
        //TreeNode treeNodeEnum = new TreeNode() { Text = "枚举集合", Name = "枚举集合" };

        private TreeNode treeNodeC154 = new TreeNode() { Text = "板卡集合", Name = "板卡集合" };

        public CommandControl1(DebugComp compiler)
        {
            InitializeComponent();
            try
            {
                DebugC = compiler;
                treeView1.ContextMenuStrip = contextMenuStrip1;
                if (compiler.DicAxes == null)
                {
                    compiler.DicAxes = new Dictionary<string, PLCAxis>();
                }
                if (compiler.DicCylinder == null)
                {
                    compiler.DicCylinder = new Dictionary<string, Cylinder>();
                }
                if (compiler.ListAxisP == null)
                {
                    compiler.ListAxisP = new List<AxisSPD>();
                }
                if (DebugC.ListRobot == null)
                {
                    DebugC.ListRobot = new List<EpsenRobot6>();
                }
                if (DebugC.DicPLCIO == null)
                {
                    DebugC.DicPLCIO = new Dictionary<string, ErosConLink.UClass.PLCValue>();
                }
                treeView1.Nodes.Add(treeNodeAx);
                foreach (var item in compiler.DicAxes)
                {
                    TreeNode treeNod = new TreeNode();
                    treeNod.Name = treeNod.Text = item.Value.Name;
                    treeNod.Tag = item.Value;
                    treeNodeAx.Nodes.Add(treeNod);
                }

                treeView1.Nodes.Add(treeNodeCyl);
                foreach (var item in compiler.DicCylinder)
                {
                    TreeNode treeNod = new TreeNode();
                    treeNod.Name = treeNod.Text = item.Value.Name;
                    treeNod.Tag = item.Value;
                    treeNodeCyl.Nodes.Add(treeNod);
                }

                treeView1.Nodes.Add(treeNodeAxisP);
                for (int i = 0; i < compiler.ListAxisP.Count; i++)
                {
                    TreeNode treeNod = new TreeNode();
                    treeNod.Name = treeNod.Text = compiler.ListAxisP[i].Name;
                    treeNod.Tag = compiler.ListAxisP[i];
                    treeNodeAxisP.Nodes.Add(treeNod);
                }
                treeView1.Nodes.Add(treeNodeRobotS);
                for (int i = 0; i < compiler.ListRobot.Count; i++)
                {
                    TreeNode treeNod = new TreeNode();
                    treeNod.Name = treeNod.Text = compiler.ListRobot[i].Name;
                    treeNod.Tag = compiler.ListRobot[i];
                    treeNodeRobotS.Nodes.Add(treeNod);
                }

                treeView1.Nodes.Add(treeNodeIO);
                foreach (var item in compiler.DicPLCIO)
                {
                    TreeNode treeNod = new TreeNode();
                    treeNod.Name = treeNod.Text = item.Value.Name;
                    treeNod.Tag = item.Value;
                    treeNodeIO.Nodes.Add(treeNod);
                }

                foreach (var item in compiler.DicPoints)
                {
                    TreeNode treeNod = new TreeNode();
                    treeNod.Name = treeNod.Text = item.Key;
                    treeNod.Tag = item.Value;
                    treeNodePrun.Nodes.Add(treeNod);
                }
                treeView1.Nodes.Add(treeNodePrun);
                treeView1.Nodes.Add(treeNodeC154);
                //foreach (var item in compiler.DicC154)
                //{
                //    TreeNode treeNod = new TreeNode();
                //    treeNod.Name = treeNod.Text = item.Value.Name;
                //    treeNod.Tag = item.Value;
                //    TreeNode treeNod2 = new TreeNode();
                //    treeNod2.Name = treeNod2.Text = item.Value.c154Axis1.Name;
                //    treeNod2.Tag = item.Value.c154Axis1;
                //    treeNod.Nodes.Add(treeNod2);

                //    treeNod2 = new TreeNode();
                //    treeNod2.Name = treeNod2.Text = item.Value.c154Axis2.Name;
                //    treeNod2.Tag = item.Value.c154Axis2;
                //    treeNod.Nodes.Add(treeNod2);
                //    treeNod2 = new TreeNode();
                //    treeNod2.Name = treeNod2.Text = item.Value.c154Axis3.Name;
                //    treeNod2.Tag = item.Value.c154Axis3;
                //    treeNod.Nodes.Add(treeNod2);

                //    treeNod2 = new TreeNode();
                //    treeNod2.Name = treeNod2.Text = item.Value.c154Axis4.Name;
                //    treeNod2.Tag = item.Value.c154Axis4;
                //    treeNod.Nodes.Add(treeNod2);
                //    treeNodeC154.Nodes.Add(treeNod);
                //}
            }
            catch (Exception)
            {
            }
        }

        private DebugComp DebugC;

        private void UserCommandControl1_Enter(object sender, EventArgs e)
        {
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                TreeNode treeNode = treeView1.GetNodeAt(e.Location);
                propertyGrid1.SelectedObject = treeNode.Tag;
                if (e.Button == MouseButtons.Right)
                {
                    TreeNode treeT = new TreeNode();

                    if (treeNode.Parent != null)
                    {
                        treeT = treeNode.Parent;
                    }
                    else
                    {
                        treeT = treeNode;
                    }
                    contextMenuStrip1.Items.Clear();
                    if (treeT.Text == "轴集合")
                    {
                        contextMenuStrip1.Items.Add("添加轴").Click += 添加轴ToolStripMenuItem_Click;
                    }
                    else if (treeT.Text == "气缸集合")
                    {
                        contextMenuStrip1.Items.Add("添加气缸").Click += 添加气缸ToolStripMenuItem_Click;
                    }
                    else if (treeT.Text == "IO集合")
                    {
                        contextMenuStrip1.Items.Add("添加IO").Click += 添加IOToolStripMenuItem_Click;
                    }
                    else if (treeT.Text == "Robot")
                    {
                        contextMenuStrip1.Items.Add("添加Robot").Click += 添加机器人ToolStripMenuItem_Click;
                    }
                    else if (treeT.Text == "轴组合")
                    {
                        contextMenuStrip1.Items.Add("添加轴组合").Click += 添加轴组合ToolStripMenuItem_Click;
                    }
                    else if (treeT.Text == "板卡集合")
                    {
                        contextMenuStrip1.Items.Add("添加板卡").Click += 添加板卡ToolStripMenuItem_Click;
                    }

                    contextMenuStrip1.Items.Add("删除").Click += 删除ToolStripMenuItem_Click; ;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void 添加板卡ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //    string nameStr = Interaction.InputBox("请输入点名称", "新建板卡", "板卡", 100, 100);
                //str:
                //    if (DebugC.DicC154.ContainsKey(nameStr))
                //    {
                //        nameStr = Interaction.InputBox("请重新输入名称", "名称已存在", nameStr, 100, 100);
                //        goto str;
                //    }
                //    if (nameStr == "")
                //    {
                //        return;
                //    }
                //    DIDO.C154_AxisGrub pLCAxis = new DIDO.C154_AxisGrub() { Name = nameStr };
                //    TreeNode treeNod = new TreeNode();
                //    treeNod.Name = treeNod.Text = nameStr;
                //    treeNod.Tag = pLCAxis;
                //    treeNodeC154.Nodes.Add(treeNod);
                //    //DebugC.DicC154.Add(pLCAxis.Name, pLCAxis);
            }
            catch (Exception)
            {
            }
        }

        private void treeView1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //propertyGrid1.SelectedObject = treeView1.SelectedNode.Tag;
            }
            catch (Exception)
            {
            }
        }

        private void 添加轴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PLCAxis pLCAxis = new PLCAxis();
                List<string> listn = new List<string>();
                listn.AddRange(DebugC.DicAxes.Keys);

                TreeNode treeNod = pLCAxis.NewNodeProject(listn);
                if (treeNod != null)
                {
                    DebugC.DicAxes.Add(pLCAxis.Name, pLCAxis);
                    treeNodeAx.Nodes.Add(treeNod);
                }
            }
            catch (Exception)
            {
            }
        }

        private void 添加气缸ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string nameStr = Interaction.InputBox("请输入点名称", "新建气缸", "气缸", 100, 100);

            str:
                if (DebugC.DicCylinder.ContainsKey(nameStr))
                {
                    nameStr = Interaction.InputBox("请重新输入名称", "名称已存在", nameStr, 100, 100);
                    goto str;
                }
                if (nameStr == "")
                {
                    return;
                }
                Cylinder pLCAxis = new Cylinder() { Name = nameStr };
                TreeNode treeNod = new TreeNode();
                treeNod.Name = treeNod.Text = nameStr;
                treeNod.Tag = pLCAxis;
                treeNodeCyl.Nodes.Add(treeNod);
                DebugC.DicCylinder.Add(pLCAxis.Name, pLCAxis);
            }
            catch (Exception)
            {
            }
        }

        private void 添加轴组合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string nameStr = Interaction.InputBox("请输入点名称", "新建轴组合", "轴组合", 100, 100);
            str:
                for (int i = 0; i < DebugC.ListAxisP.Count; i++)
                {
                    if (DebugC.ListAxisP[i].Name == nameStr)
                    {
                        nameStr = Interaction.InputBox("请重新输入名称", "名称已存在", nameStr, 100, 100);
                        goto str;
                    }
                }
                if (nameStr == "")
                {
                    return;
                }
                AxisSPD pLCAxis = new AxisSPD() { Name = nameStr };
                TreeNode treeNod = new TreeNode();
                treeNod.Name = treeNod.Text = nameStr;
                treeNod.Tag = pLCAxis;
                treeNodeAxisP.Nodes.Add(treeNod);
                DebugC.ListAxisP.Add(pLCAxis);
            }
            catch (Exception)
            {
            }
        }

        private void 添加机器人ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string nameStr = Interaction.InputBox("请输入点名称", "新建机器人", "机器人", 100, 100);

            str:
                for (int i = 0; i < DebugC.ListAxisP.Count; i++)
                {
                    if (DebugC.ListAxisP[i].Name == nameStr)
                    {
                        nameStr = Interaction.InputBox("请重新输入名称", "名称已存在", nameStr, 100, 100);
                        goto str;
                    }
                }
                if (nameStr == "")
                {
                    return;
                }
                EpsenRobot6 pLCAxis = new EpsenRobot6() { Name = nameStr };
                TreeNode treeNod = new TreeNode();
                treeNod.Name = treeNod.Text = nameStr;
                treeNod.Tag = pLCAxis;
                treeNodeRobotS.Nodes.Add(treeNod);
                DebugC.ListRobot.Add(pLCAxis);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode.Tag is INodeNew)
                {
                    INodeNew nodeNew = treeView1.SelectedNode.Tag as INodeNew;

                    if (treeView1.SelectedNode.Parent.Text == "轴集合")
                    {
                        DebugC.DicAxes.Remove(treeView1.SelectedNode.Text);
                    }
                    else if (treeView1.SelectedNode.Parent.Text == "气缸集合")
                    {
                        DebugC.DicCylinder.Remove(treeView1.SelectedNode.Text);
                    }
                    else if (treeView1.SelectedNode.Parent.Text == "轴组合")
                    {
                        DebugC.ListAxisP.Remove(treeView1.SelectedNode.Tag as AxisSPD);
                    }
                    else if (treeView1.SelectedNode.Parent.Text == "Robot")
                    {
                        DebugC.ListRobot.Remove(treeView1.SelectedNode.Tag as EpsenRobot6);
                    }
                    //else if (treeView1.SelectedNode.Parent.Text == "板卡集合")
                    //{
                    //    DebugC.DicC154.Remove(treeView1.SelectedNode.Text);
                    //}
                    else if (DebugC.DicPLCIO.ContainsKey(treeView1.SelectedNode.Text))
                    {
                        DebugC.DicPLCIO.Remove(treeView1.SelectedNode.Text);
                    }
                    nodeNew.Remove(treeView1.SelectedNode);
                }
            }
            catch (Exception)
            {
            }
        }

        private void 添加IOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string nameStr = Interaction.InputBox("请输入点名称", "新建IO地址", "PLC-IO", 100, 100);

            str:
                for (int i = 0; i < DebugC.ListAxisP.Count; i++)
                {
                    if (DebugC.ListAxisP[i].Name == nameStr)
                    {
                        nameStr = Interaction.InputBox("请重新输入名称", "名称已存在", nameStr, 100, 100);
                        goto str;
                    }
                }
                if (nameStr == "")
                {
                    return;
                }
                ErosConLink.UClass.PLCValue pLCAxis = new ErosConLink.UClass.PLCValue() { Name = nameStr };
                TreeNode treeNod = new TreeNode();
                treeNod.Name = treeNod.Text = nameStr;
                treeNod.Tag = pLCAxis;
                treeView1.Nodes.Find("IO集合", false)[0].Nodes.Add(treeNod);
                if (!DebugC.DicPLCIO.ContainsKey(pLCAxis.Name))
                {
                    DebugC.DicPLCIO.Add(pLCAxis.Name, pLCAxis);
                }
                else
                {
                    MessageBox.Show("已存在同名");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}