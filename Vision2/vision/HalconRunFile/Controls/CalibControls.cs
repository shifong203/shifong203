using HalconDotNet;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.vision.Calib;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class CalibControls : UserControl
    {
        public CalibControls()
        {
            InitializeComponent();
        }

        private AutoCalibPoint AutoCalibPoint = new AutoCalibPoint();
        private string path = "";
        private HTuple ToolInBasePose;

        private void CalibControls_Load(object sender, EventArgs e)
        {
            PuData();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Tag != null)
                {
                    string path = e.Node.Tag.ToString();
                    if (path.EndsWith(".bmp"))
                    {
                        Vision.GetFocusRunHalcon().ReadImage(path);
                    }
                    else if (path.EndsWith(".dat"))
                    {
                        HOperatorSet.ReadPose(path, out ToolInBasePose);
                        textBox1.Text = (e.Node.Text + ":" + ToolInBasePose.ToString() + Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        private bool DRW;

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ToolInBasePose == null)
                {
                    MessageBox.Show("未选择机器人坐标");
                }

                if (Single.TryParse(textBox2.Text, out Single rows) && Single.TryParse(textBox3.Text, out Single cols))
                {
                    AutoCalibPoint.Run(AutoCalibPoint.CalibMode.移动抓取, rows, cols, ToolInBasePose, out HTuple x, out HTuple y);
                    if (x == null)
                    {
                        MessageBox.Show("转换失败");
                    }
                    textBox4.Text = x.ToString();
                    textBox5.Text = y.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 保存标定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (DRW)
                {
                    return;
                }
                DRW = true;

                Vision.GetFocusRunHalcon().GetHWindow().Focus();
                HOperatorSet.DrawPointMod(Vision.GetFocusRunHalcon().hWindowHalcon(), Vision.GetFocusRunHalcon().Height / 2,
                    Vision.GetFocusRunHalcon().Width / 2, out HTuple ROW, out HTuple COL);

                textBox2.Text = ROW.ToString();
                textBox3.Text = COL.ToString();
                button1.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DRW = false;
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (treeView2.SelectedNode.Tag is Calib.AutoCalibPoint)
                {
                    AutoCalibPoint = treeView2.SelectedNode.Tag as AutoCalibPoint;
                }
            }
            catch (Exception)
            {
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (MessageBox.Show("确定要删除标定:" + treeView2.SelectedNode.Text, "删除标定", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (Vision.Instance.DicCalib3D.ContainsKey(treeView2.SelectedNode.Text))
                        {
                            Vision.Instance.DicCalib3D.Remove(treeView2.SelectedNode.Text);
                            treeView2.SelectedNode.Remove();
                        }
                        else
                        {
                            MessageBox.Show("不存在标定:" + treeView2.SelectedNode.Text);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private void 新建标定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode sdf = Vision2.ErosProjcetDLL.Project.INodeNew.NewTreeNodeProject(Vision.Instance.DicCalib3D.Keys.ToList(), "新建标定");
                if (sdf != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("是否创建文件夹", "新建文件夹", MessageBoxButtons.YesNo))
                    {
                        Directory.CreateDirectory(path + "\\" + sdf.Text + "\\");
                    }
                    Vision.Instance.DicCalib3D.Add(sdf.Text, new AutoCalibPoint());
                    sdf.Tag = Vision.Instance.DicCalib3D[sdf.Text];
                    treeView2.Nodes.Add(sdf);
                }
            }
            catch (Exception)
            {
            }
        }

        private void 从文件读取ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AutoCalibPoint autoCalibPoint = new AutoCalibPoint();
                if (treeView2.SelectedNode != null)
                {
                    if (treeView2.SelectedNode.Tag is Calib.AutoCalibPoint)
                    {
                        autoCalibPoint = treeView2.SelectedNode.Tag as AutoCalibPoint;
                    }
                }
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.Description = "请选择文件夹";
                fbd.SelectedPath = path;
                System.Windows.Forms.DialogResult dialog = FolderBrowserLauncher.ShowFolderBrowser(fbd);
                if (dialog == System.Windows.Forms.DialogResult.OK)
                {
                    if (autoCalibPoint.ReadCalib(fbd.SelectedPath))
                    {
                        TreeNode treeNode = new TreeNode();
                        treeNode.Text = treeNode.Name = Path.GetFileNameWithoutExtension(fbd.SelectedPath);
                        treeNode.Tag = autoCalibPoint;
                        treeView2.Nodes.Add(treeNode);
                        if (!Vision.Instance.DicCalib3D.ContainsKey(treeNode.Text))
                        {
                            Vision.Instance.DicCalib3D.Add(treeNode.Text, autoCalibPoint);
                        }
                        else
                        {
                            Vision.Instance.DicCalib3D[treeNode.Text] = autoCalibPoint;
                        }
                        MessageBox.Show("读取成功");
                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }
                }
                return;
            }
            catch (Exception)
            {
            }
        }

        private void treeView2_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeView2.SelectedNode.Tag is Calib.AutoCalibPoint)
                {
                    AutoCalibPoint = treeView2.SelectedNode.Tag as AutoCalibPoint;
                }
                //this.propertyGrid1.SelectedObject = treeView2.SelectedNode.Tag;
            }
            catch (Exception)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                ToolInBasePose = Vision.GetFocusRunHalcon().GetRobotBaesPose();
                if (ToolInBasePose == null)
                {
                    return;
                }

                textBox1.Text = (Vision.GetSaveImageInfo("").LingkRobotName + "当前位置" + ToolInBasePose.ToString() + Environment.NewLine);
            }
            catch (Exception)
            {
            }
        }

        private void 新建标定文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            PuData();
        }

        private void PuData()
        {
            try
            {
                treeView1.Nodes.Clear();
                treeView2.Nodes.Clear();
                //AutoCalibPoint= autoCalib;
                path = Vision.GetFilePath() + AutoCalibPoint.FileName;
                Directory.CreateDirectory(path);
                TreeNode treeNode = new TreeNode();
                treeNode.Name = treeNode.Text = Path.GetFileName(path);
                Vision2.ErosProjcetDLL.FileCon.FileConStatic.GetFilesToTreeNode(treeNode, path);
                treeView1.Nodes.Add(treeNode);
                foreach (var item in Vision.Instance.DicCalib3D)
                {
                    TreeNode treeNode2 = new TreeNode();
                    treeNode2.Name = treeNode2.Text = item.Key;
                    treeNode2.Tag = item.Value;
                    treeView2.Nodes.Add(treeNode2);
                }
            }
            catch (Exception)
            {
            }
        }

        private void 读取标定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            VisonForm1 visionWindowP = new VisonForm1();
            visionWindowP.Show();
        }
    }
}