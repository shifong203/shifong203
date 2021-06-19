using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.vision.HalconRunFile.PCBFile;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class PCBAControl : UserControl
    {
        public PCBAControl()
        {
            InitializeComponent();
        }
        PCBA PCBAT;
        public PCBAControl(PCBA pCBA) : this()
        {
            PCBAT = pCBA;
            halcon = pCBA.GetPThis();
        }
        HalconRun halcon;
        private void 添加电阻ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                string name = Interaction.InputBox("请输入新名称", "创建电容", "电容c1", 100, 100);
                string dsts = name;
                RunProgram bPCBoJB = null;
                if (PCBAT.GetDicPCBA().ContainsKey(dsts))
                {
                     int ds = ProjectINI.GetStrReturnInt(name, out dsts);
                     stret:
                     if (PCBAT.GetDicPCBA().ContainsKey(dsts + (++ds)))
                        {  goto stret; }
                    dsts = dsts + (ds);
                    DialogResult dr = MessageBox.Show(name + ":已存在!是否新建《" + dsts + "》？", "新建程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        string jsonStr = JsonConvert.SerializeObject(PCBAT.GetDicPCBA()[name]);
                        object DA = JsonConvert.DeserializeObject(jsonStr, PCBAT.GetDicPCBA()[name].GetType());
                        bPCBoJB = DA as RunProgram;
                    }
                    else   return;    
                }
                else bPCBoJB = new Capacitance();
                 PCBAT.GetDicPCBA().Add(dsts, bPCBoJB);
                 TreeNode treeNode = treeView1.Nodes.Add(dsts);
                 treeNode.Tag = bPCBoJB;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                TreeNode treeNode = treeView1.SelectedNode;
                if (treeNode!=null)
                {
                    if (PCBAT.GetDicPCBA().ContainsKey(treeNode.Text))
                    {
                        PCBAT.GetDicPCBA().Remove(treeNode.Text);
                        if (PCBAT.DicPCBType.ContainsKey(treeNode.Text))
                        {
                            PCBAT.DicPCBType.Remove(treeNode.Text);
                        }
                        treeView1.Nodes.Remove(treeNode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PCBAControl_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in PCBAT.GetDicPCBA())
                {
                    if (item.Key!="")
                    {
                        TreeNode treeNode = treeView1.Nodes.Add(item.Key);
                        treeNode.Text = item.Key;
                        treeNode.Tag = item.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        TreeNode CurrentNode;
        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right) return;
                Task.Run(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        Point ClickPoint = new Point(e.X, e.Y);
                        CurrentNode = treeView1.GetNodeAt(ClickPoint);
                        if (CurrentNode != null)
                        {
                            tabPage1.Text = CurrentNode.Text;
                            InterfacePCBA Capacit = CurrentNode.Tag as InterfacePCBA;
                            if (Capacit!=null)
                            {
                                tabPage1.Controls.Clear();
                                Control control = Capacit.GetControl(PCBAT.GetPThis());
                                tabPage1.Controls.Add(control);
                                control.Dock = DockStyle.Fill;
                                halcon.HobjClear();
                                RunProgram bPCBoJB =     CurrentNode.Tag as RunProgram;
                                propertyGrid1.SelectedObject = bPCBoJB;
                                bPCBoJB.GetRunProgram(PCBAT);
                                bPCBoJB.RunHProgram( halcon.GetOneImageR(),out List<OneRObj> oneRObj);
                                //halcon.AddObj(bPCBoJB.TestingRoi);
                                halcon.ShowObj();
                            }
                        }
                    }));
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 添加ICToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string name = Interaction.InputBox("请输入新名称", "创建电容", "IC芯片", 100, 100);
                string dsts = name;
                if (name == "") return;
                RunProgram bPCBoJB = null;
                if (PCBAT.GetDicPCBA().ContainsKey(dsts))
                {
                    int ds = ProjectINI.GetStrReturnInt(name, out dsts);
                stret:
                    if (PCBAT.GetDicPCBA().ContainsKey(dsts + (++ds)))
                    {
                        goto stret;
                    }
                    dsts = dsts + (ds);
                    DialogResult dr = MessageBox.Show(name + ":已存在!是否新建《" + dsts + "》？", "新建程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        string jsonStr = JsonConvert.SerializeObject(PCBAT.GetDicPCBA()[name]);
                        object DA = JsonConvert.DeserializeObject(jsonStr, PCBAT.GetDicPCBA()[name].GetType());
                        bPCBoJB = DA as RunProgram;
                    }
                    else
                    {
                        return;
                    }
                }
                else bPCBoJB = new ICPint();
                PCBAT.GetDicPCBA().Add(dsts, bPCBoJB);
                TreeNode treeNode = treeView1.Nodes.Add(dsts);
                treeNode.Tag = bPCBoJB;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentNode==null) return;
                string dsts = "重命名";
                stret:
                string name = Interaction.InputBox("请输入新名称", dsts, CurrentNode.Text, 100, 100);
                if (name=="")   return;
                RunProgram bPCBoJB = CurrentNode.Tag as RunProgram;
                if (PCBAT.GetDicPCBA().ContainsKey(name))
                {
                    DialogResult dr = MessageBox.Show(name + ":已存在!请重新输入", "请重新输入", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK) goto stret;
                    else  return;  
                }
                bPCBoJB.Name = name;
                PCBAT.GetDicPCBA().Add(name, bPCBoJB);
                treeView1.Nodes.Add(name).Tag= bPCBoJB;
                PCBAT.GetDicPCBA().Remove(CurrentNode.Text);
                treeView1.Nodes.Remove(CurrentNode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 添加矩形电容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string name = Interaction.InputBox("请输入新名称", "创建矩形电容", "电容1", 100, 100);
                string dsts = name;
                if (name == "") return;
                RunProgram bPCBoJB = null;
                if (PCBAT.GetDicPCBA().ContainsKey(dsts))
                {
                    int ds = ProjectINI.GetStrReturnInt(name, out dsts);
                   stret:
                    if (PCBAT.GetDicPCBA().ContainsKey(dsts + (++ds)))
                    {
                        goto stret;
                    }
                    dsts = dsts + (ds);
                    DialogResult dr = MessageBox.Show(name + ":已存在!是否新建《" + dsts + "》？", "新建程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        string jsonStr = JsonConvert.SerializeObject(PCBAT.GetDicPCBA()[name]);
                        object DA = JsonConvert.DeserializeObject(jsonStr, PCBAT.GetDicPCBA()[name].GetType());
                        bPCBoJB = DA as RunProgram;
                    }
                    else
                    {
                        return;
                    }
                }
                else bPCBoJB = new RectangleCapacitance();
                PCBAT.GetDicPCBA().Add(dsts, bPCBoJB);
                TreeNode treeNode = treeView1.Nodes.Add(dsts);
                treeNode.Tag = bPCBoJB;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
