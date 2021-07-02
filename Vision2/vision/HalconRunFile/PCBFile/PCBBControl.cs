using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.vision.HalconRunFile.RunProgramFile;
using HalconDotNet;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    public partial class PCBBControl : UserControl
    {
        public PCBBControl()
        {
            InitializeComponent();
        }
        public PCBBControl( PCBAEX pCBAEX):this()
        {
            pCBAEX1 = pCBAEX;
            halcon = pCBAEX1.GetPThis();
            UPdata();
        }
        HalconRun halcon;
        PCBAEX pCBAEX1;
        TreeNode CurrentNode;
        public void UPdata()
        {
            try
            {
                treeView1.Nodes.Clear();
                foreach (var item in pCBAEX1.DictRoi)
                {
                    TreeNode treeNode = treeView1.Nodes.Add(item.Key);
                    treeNode.Tag = item.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PCBBControl_Load(object sender, EventArgs e)
        {
      
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            PCBLibraryForm pCBLibraryForm = new PCBLibraryForm(pCBAEX1);

            pCBLibraryForm.ShowDialog();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                CurrentNode = e.Node;
                tabPage1.Text = CurrentNode.Text;
                halcon.HobjClear();
                Library.LibraryVisionBase Capacit = CurrentNode.Tag as Library.LibraryVisionBase;
                propertyGrid1.SelectedObject = Capacit;
                halcon.AddObj(Capacit.GetAOI().Aoi);
                halcon.AddImageMassage(Capacit.Row, Capacit.Col,Capacit.Name);
                halcon.ShowObj();
            }
            catch (Exception)
            {
            }

        }

        private void 添加ICToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string name = Interaction.InputBox("请输入新名称", "创建电容", "IC芯片", 100, 100);
                string dsts = name;
                if (name == "") return;
                Library.LibraryVisionBase bPCBoJB = null;
                if (pCBAEX1.DictRoi.ContainsKey(dsts))
                {
                    int ds = ProjectINI.GetStrReturnInt(name, out dsts);
                    stret:
                    if (pCBAEX1.DictRoi.ContainsKey(dsts + (++ds)))
                    {
                        goto stret;
                    }
                    dsts = dsts + (ds);
                    DialogResult dr = MessageBox.Show(name + ":已存在!是否新建《" + dsts + "》？", "新建程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        string jsonStr = JsonConvert.SerializeObject(pCBAEX1.DictRoi[name]);
                        object DA = JsonConvert.DeserializeObject(jsonStr, pCBAEX1.DictRoi[name].GetType());
                        bPCBoJB = DA as Library.LibraryVisionBase;
                    }
                    else
                    {
                        return;
                    }
                }
                else bPCBoJB = new Library.LibraryVisionBase();
                pCBAEX1.DictRoi.Add(dsts, bPCBoJB);
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
                if (CurrentNode == null) return;
                string dsts = "重命名";
                 stret:
                string name = Interaction.InputBox("请输入新名称", dsts, CurrentNode.Text, 100, 100);
                if (name == "") return;
                Library.LibraryVisionBase bPCBoJB = CurrentNode.Tag as Library.LibraryVisionBase;
                if (pCBAEX1.DictRoi.ContainsKey(name))
                {
                    DialogResult dr = MessageBox.Show(name + ":已存在!请重新输入", "请重新输入", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK) goto stret;
                    else return;
                }
                bPCBoJB.Name = name;
                pCBAEX1.DictRoi.Add(name, bPCBoJB);
                treeView1.Nodes.Add(name).Tag = bPCBoJB;
                pCBAEX1.DictRoi.Remove(CurrentNode.Text);
                treeView1.Nodes.Remove(CurrentNode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode treeNode = treeView1.SelectedNode;
                if (treeNode != null)
                {
                    if (pCBAEX1.DictRoi.ContainsKey(treeNode.Text))
                    {
                        pCBAEX1.DictRoi.Remove(treeNode.Text);
                        treeView1.Nodes.Remove(treeNode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
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
                                Library.LibraryVisionBase Capacit = CurrentNode.Tag as Library.LibraryVisionBase;
                                propertyGrid1.SelectedObject = Capacit;
                                if (Capacit != null)
                                {
                                    panel1.Controls.Clear();
                                    Control control = Capacit.GetControl(pCBAEX1.GetPThis());
                                    panel1.Controls.Add(control);
                                    control.Dock = DockStyle.Fill;
                                    halcon.HobjClear();
                                    Capacit.Run(halcon.GetOneImageR());
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                Library.LibraryVisionBase Capacit = CurrentNode.Tag as Library.LibraryVisionBase;

                HObject hObject = RunProgram.DragMoveOBJ(halcon, Capacit.GetRun().AOIObj);
                HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple row, out HTuple column);
                Capacit.Row = row;
                Capacit.Col = column;
            }
            catch (Exception)
            {
            }
           
        }
    }
}
