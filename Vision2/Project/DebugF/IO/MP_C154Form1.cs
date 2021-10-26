using ErosSocket.DebugPLC;
using HalconDotNet;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.Project.formula;
using Vision2.vision;

namespace Vision2.Project.DebugF.IO
{
    public partial class MP_C154Form1 : Form
    {
        public MP_C154Form1()
        {
            InitializeComponent();
        }

        private HWindID HWindID2 = new HWindID();

        private 工艺库.MatrixC MatrixC;

        private void DDAxis_UpCycle(DODIAxis key)
        {
            try
            {
                if (this.IsDisposed)
                {
                    DebugCompiler.Instance.DDAxis.UpCycle -= DDAxis_UpCycle;
                }
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<DODIAxis>(DDAxis_UpCycle), key);
                    return;
                }
            }
            catch (Exception)
            { }
        }

        private void MP_C154Form1_Load(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.Instance.DDAxis.UpCycle += DDAxis_UpCycle;
                Point point = new Point();
                int sdt = 0;
                listBox5.Items.Clear();
                for (int it = 0; it < DebugCompiler.Instance.DDAxis.ListTray.Count; it++)
                {
                    listBox5.Items.Add(it);
                }
                HWindID2.Initialize(hWindowControl1);
                HWindID2.HeigthImage = (int)numericUpDown3.Value;
                HWindID2.WidthImage = (int)numericUpDown4.Value;
                HWindID2.OneResIamge = new OneResultOBj();

                listBox6.Items.Clear();
                for (int it = 0; it < DebugCompiler.Instance.ListMatrix.Count; it++)
                {
                    listBox6.Items.Add(it);
                }
                runCodeUserControl1.SetData(DebugCompiler.Instance.DDAxis.RunCodeT);
                runCodeUserControl2.SetData(DebugCompiler.Instance.DDAxis.HomeCodeT);
                runCodeUserControl3.SetData(DebugCompiler.Instance.DDAxis.StopCodeT);
                runCodeUserControl4.SetData(DebugCompiler.Instance.DDAxis.CPKCodeT);

                int i = 0;
                foreach (var item in DebugCompiler.Instance.DDAxis.AxisS)
                {
                    ErosSocket.DebugPLC.PLC.AxisControl axis = new ErosSocket.DebugPLC.PLC.AxisControl(item);
                    flowLayoutPanel1.Controls.Add(axis);
                    int sd = i / 5;
                    int dt = i % 5;
                    axis.Location = new Point(new Size(axis.Width * dt, axis.Height * sd));
                    point = axis.Location;
                    i++;
                    sdt = axis.Height + axis.Location.Y;
                }
                i = 0;
                foreach (var item in DebugCompiler.Instance.DDAxis.Cylinders)
                {
                    CylinderControl CylinderControlT = new CylinderControl(item);
                    flowLayoutPanel1.Controls.Add(CylinderControlT);
                    int sd = i / 3;
                    int dt = i % 3;
                    CylinderControlT.Location = new Point(new Size(CylinderControlT.Width * dt, sdt + CylinderControlT.Height * sd));
                    i++;
                }
                if (DebugCompiler.Instance.DDAxis.AxisS.Count == 0 && DebugCompiler.Instance.DDAxis.Cylinders.Count == 0)
                {
                    tabControl1.TabPages.Remove(tabPage2);
                }
      
            }
            catch (Exception ex)
            {
                MessageBox.Show("位置加载:" + ex.Message);
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                propertyGrid3.SelectedObject = DebugCompiler.Instance.DDAxis.ListTray[listBox5.SelectedIndex];
                trayControl1.SetTray(DebugCompiler.Instance.DDAxis.ListTray[listBox5.SelectedIndex]);
            }
            catch (Exception)
            { }
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MatrixC = DebugCompiler.Instance.ListMatrix[listBox6.SelectedIndex];
                propertyGrid4.SelectedObject = MatrixC;
            }
            catch (Exception)  { }
        }

        private void 添加点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.Instance.ListMatrix.Add(new 工艺库.MatrixC());
                listBox6.Items.Add(DebugCompiler.Instance.ListMatrix.Count - 1);
            }
            catch (Exception)  { }
        }

        private void 删除矩阵ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.Instance.ListMatrix.RemoveAt(listBox6.SelectedIndex);
                listBox6.Items.RemoveAt(listBox6.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MatrixC.Calculate(HWindID2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MatrixC.MarkMove(MatrixC.PointName, MatrixC.PointNameEnd, null, HWindID2);
                HWindID2.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (IThaing)
                {
                    return;
                }
                Thread thread = new Thread(() =>
                {
                    IThaing = true;
                    try
                    {
                        MatrixC.Mark1Move(MatrixC.PointName);
                        XYZPoint point = DebugCompiler.Instance.DDAxis.GetToPoint(MatrixC.PointName);
                        if (point != null)
                        {
                            HOperatorSet.GenCircle(out HObject hObject1, point.Y, point.X, 10);
                            HOperatorSet.GenCrossContourXld(out HObject hObjectxs, point.Y, point.X, 10, 0);

                            HWindID2.OneResIamge.AddObj(hObject1.ConcatObj(hObjectxs), ColorResult.yellow);
                            HWindID2.OneResIamge.AddImageMassage(point.Y, point.X, "Mk1", ColorResult.green);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    IThaing = false;
                });
                thread.IsBackground = true;

                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (IThaing)
                {
                    return;
                }
                Thread thread = new Thread(() =>
                {
                    IThaing = true;
                    try
                    {
                        MatrixC.Mark1Move(MatrixC.PointNameEnd);
                        XYZPoint point = DebugCompiler.Instance.DDAxis.GetToPoint(MatrixC.PointNameEnd);
                        if (point != null)
                        {
                            HOperatorSet.GenCircle(out HObject hObject1, point.Y, point.X, 10);
                            HOperatorSet.GenCrossContourXld(out HObject hObjectxs, point.Y, point.X, 10, 0);
                            HWindID2.OneResIamge.AddObj(hObject1.ConcatObj(hObjectxs), ColorResult.yellow);
                            HWindID2.OneResIamge.AddImageMassage(point.Y, point.X, MatrixC.PointNameEnd, ColorResult.green);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    IThaing = false;
                });
                thread.IsBackground = true;

                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool IThaing;

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (IThaing)
                {
                    return;
                }
                Thread thread = new Thread(() =>
                {
                    IThaing = true;
                    MatrixC.MarkCiacelbMove(MatrixC.PointName, null, HWindID2);
                    IThaing = false;
                });
                thread.IsBackground = true;

                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        if (MatrixC.MoveMxet(null, HWindID2))
                        {
                            MessageBox.Show("移动结束");
                        }
                        else
                        {
                            MessageBox.Show("移动失败");
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("移动结束" + ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (MatrixC.IsFillImage)
                {
                    MatrixC.IsFillImage = false;
                }
                else
                {
                    MatrixC.IsFillImage = true;
                }
                MatrixC.FillIamge(HWindID2);
            }
            catch (Exception ex)
            {
            }
        }


        private void MP_C154Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F5)
                {
                    //  DebugCompiler.GetThis().DDAxis.RunCodeT.NextStep = true;
                }
                else if (e.KeyCode == Keys.F6)
                {
                    //toolStripButton2.PerformClick();
                }
                else if (e.KeyCode == Keys.F7)
                {
                    //toolStripButton4.PerformClick();
                }
            }
            catch (Exception)
            {
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否导出到位置信息?", "导出矩阵位置", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    List<XYZPoint> xYZP = RecipeCompiler.GetProductEX().DPoint;
                    XYZPoint point2 = DebugCompiler.Instance.DDAxis.GetToPointFileProt(MatrixC.PointName);
                    for (int i = 0; i < MatrixC.XS.Count; i++)
                    {
                        xYZP.Add(new XYZPoint() { Name = "MP" + (i + 1), X = MatrixC.XS[i], Y = MatrixC.YS[i], Z = point2.Z, ID = i + 1, AxisGrabName = point2.AxisGrabName });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void 导出到全局位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否导出到位置信息?", "导出托盘位置", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    List<PointFile> pointFiles = DebugCompiler.Instance.DDAxis.ListTray[listBox5.SelectedIndex].GetPoints();
                    List<XYZPoint> xYZP = DebugCompiler.Instance.DDAxis.XyzPoints;
                    for (int i = 0; i < pointFiles.Count; i++)
                    {
                        xYZP.Add(new XYZPoint()
                        {
                            Name = "TP" + (i + 1),
                            X = pointFiles[i].X.Value,
                            Y = pointFiles[i].Y.Value,
                            Z = pointFiles[i].Z.Value,
                            ID = i + 1,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 导出到产品位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否导出到位置信息?", "导出托盘位置", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    List<PointFile> pointFiles = DebugCompiler.Instance.DDAxis.ListTray[listBox5.SelectedIndex].GetPoints();
                    List<XYZPoint> xYZP = RecipeCompiler.GetProductEX().DPoint;
                    //XYZPoint point2 = DebugCompiler.GetThis().DDAxis.GetToPointFileProt(MatrixC.PointName);
                    for (int i = 0; i < pointFiles.Count; i++)
                    {
                        xYZP.Add(new XYZPoint()
                        {
                            Name = "TP" + (i + 1),
                            X = pointFiles[i].X.Value,
                            Y = pointFiles[i].Y.Value,
                            Z = pointFiles[i].Z.Value,
                            ID = i + 1,
                        });
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