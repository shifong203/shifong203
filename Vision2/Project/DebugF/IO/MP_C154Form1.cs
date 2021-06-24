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
            isCot = true;
            InitializeComponent();
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView2);
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView3);
            //ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView4);
        }
        HWindID HWindID2 = new HWindID();

        HWindID HWindNt = new HWindID();

        ProductEX productEX;


        List<XYZPoint> xYZPoints;

        List<XYZPoint> RelativelyPoint;
        List<ProductEX.Relatively.PointType> RelNamePoints;

        bool isCot = false;

        工艺库.MatrixC MatrixC;
        private void DDAxis_UpCycle(DODIAxis key)
        {
            try
            {
                if (this.IsDisposed)
                {
                    DebugCompiler.GetThis().DDAxis.UpCycle -= DDAxis_UpCycle;
                }
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<DODIAxis>(DDAxis_UpCycle), key);
                    return;
                }

            }
            catch (Exception)
            {
            }
        }
        private void MP_C154Form1_Load(object sender, EventArgs e)
        {
            try
            {
                isCot = true;

                listBox1.Items.Clear();

                listBox1.Items.Add("全局点位");

                HWindNt.Initialize(hWindowControl2);
                listBox1.Items.AddRange(Vision2.Project.formula.RecipeCompiler.Instance.ProductEX.Keys.ToArray());

                this.didoUserControl1.setDODI(DebugCompiler.GetDoDi());

                Column8.Items.AddRange(Enum.GetNames(typeof(EnumXYZUMoveType)));
                Column11.Items.AddRange(Enum.GetNames(typeof(ProductEX.Relatively.EnumPointType)));
                dataGridViewComboBoxColumn3.Items.AddRange(Enum.GetNames(typeof(EnumXYZUMoveType)));
                DebugCompiler.GetThis().DDAxis.UpCycle += DDAxis_UpCycle;
                //checkBox1.Checked = DODIAxis.Single_step;
                Column7.Items.AddRange(DebugCompiler.GetThis().DDAxis.AxisGrot.Keys.ToArray());
                dataGridViewComboBoxColumn4.Items.AddRange(DebugCompiler.GetThis().DDAxis.AxisGrot.Keys.ToArray());

                Point point = new Point();
                int sdt = 0;
                listBox5.Items.Clear();
                for (int it = 0; it < DebugCompiler.GetThis().DDAxis.ListTray.Count; it++)
                {
                    listBox5.Items.Add(it);
                }
                HWindID2.Initialize(hWindowControl1);
                HWindID2.HeigthImage = (int)numericUpDown3.Value;
                HWindID2.WidthImage = (int)numericUpDown4.Value;
                HWindID2.OneResIamge = new OneResultOBj();

                listBox6.Items.Clear();
                for (int it = 0; it < DebugCompiler.GetThis().ListMatrix.Count; it++)
                {
                    listBox6.Items.Add(it);
                }

                runCodeUserControl1.SetData(DebugCompiler.GetThis().DDAxis.RunCodeT);
                runCodeUserControl2.SetData(DebugCompiler.GetThis().DDAxis.HomeCodeT);
                runCodeUserControl3.SetData(DebugCompiler.GetThis().DDAxis.StopCodeT);
                runCodeUserControl4.SetData(DebugCompiler.GetThis().DDAxis.CPKCodeT);


                int i = 0;
                foreach (var item in DebugCompiler.GetThis().DDAxis.AxisS)
                {
                    ErosSocket.DebugPLC.PLC.AxisControl axis = new ErosSocket.DebugPLC.PLC.AxisControl(item);
                    tabPage2.Controls.Add(axis);
                    int sd = i / 3;
                    int dt = i % 3;
                    axis.Location = new Point(new Size(axis.Width * dt, axis.Height * sd));
                    point = axis.Location;
                    i++;
                    sdt = axis.Height + axis.Location.Y;
                }
                i = 0;
                foreach (var item in DebugCompiler.GetThis().DDAxis.Cylinders)
                {
                    CylinderControl CylinderControlT = new CylinderControl(item);
                    tabPage2.Controls.Add(CylinderControlT);
                    int sd = i / 3;
                    int dt = i % 3;
                    CylinderControlT.Location = new Point(new Size(CylinderControlT.Width * dt, sdt + CylinderControlT.Height * sd));
                    i++;
                }
                if (DebugCompiler.GetThis().DDAxis.AxisS.Count == 0 && DebugCompiler.GetThis().DDAxis.Cylinders.Count == 0)
                {
                    tabControl1.TabPages.Remove(tabPage2);
                }
                //if (DebugCompiler.GetThis().TrayCont < 0)
                //{
                //    tabControl1.TabPages.Remove(tabPage3);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show( "位置加载:"+ex.Message);
            }

            isCot = false;
        }

        private void 添加新点ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                isCot = true;
                int de = dataGridView1.Rows.Add();
                XYZPoint xYZPoint = new XYZPoint();
                xYZPoints.Add(xYZPoint);

                xYZPoint.Name = "P" + de;
                dataGridView1.Rows[de].Cells[0].Value = xYZPoint.Name;
                dataGridView1.Rows[de].Cells[1].Value = xYZPoint.X;
                dataGridView1.Rows[de].Cells[2].Value = xYZPoint.Y;
                dataGridView1.Rows[de].Cells[3].Value = xYZPoint.Z;
                dataGridView1.Rows[de].Cells[4].Value = xYZPoint.U;
                dataGridView1.Rows[de].Cells[5].Value = xYZPoint.ID;
                dataGridView1.Rows[de].Cells[6].Value = xYZPoint.isMove;
                xYZPoint.AxisGrabName = Column7.Items[0].ToString();
                dataGridView1.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
            }
            catch (Exception)
            {

            }

            isCot = false;


        }

        private void 移动到点位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int de = dataGridView1.SelectedCells[0].RowIndex;
                Thread thread = new Thread(() =>
                {
                    Enum.TryParse<EnumXYZUMoveType>(this.dataGridView1.Rows[de].Cells[6].Value.ToString(), out EnumXYZUMoveType enumXYZUMoveType);
                    bool flag2 = DebugCompiler.GetThis().DDAxis.SetXYZ1Points(this.dataGridView1.Rows[de].Cells[7].Value.ToString(), 15,
                      double.Parse(this.dataGridView1.Rows[de].Cells[1].Value.ToString()), double.Parse(this.dataGridView1.Rows[de].Cells[2].Value.ToString()),
                  Convert.ToDouble(this.dataGridView1.Rows[de].Cells[3].Value), Convert.ToDouble(this.dataGridView1.Rows[de].Cells[4].Value), enumXYZUMoveType);
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void 删除点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                int det = dataGridView1.SelectedRows.Count;
                if (MessageBox.Show("删除点", "是否删除点位" + det, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int i = 0; i < det; i++)
                    {
                        xYZPoints.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                bool flag = e.ColumnIndex == 8;
                this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                if (flag)
                {
                    if (listBox1.SelectedIndex == 0 || listBox1.SelectedItem.ToString() == formula.Product.ProductionName)
                    {
                        int de = e.RowIndex;
                        this.dataGridView1.Rows[de].DefaultCellStyle.BackColor = Color.Green;
                        Thread thread = new Thread(() =>
                        {

                            try
                            {
                                Enum.TryParse<EnumXYZUMoveType>(this.dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString(), out EnumXYZUMoveType enumXYZUMoveType);
                                bool flag2 = DebugCompiler.GetThis().DDAxis.SetXYZ1Points(this.dataGridView1.Rows[de].Cells[7].Value.ToString(), 15,
                           double.Parse(this.dataGridView1.Rows[de].Cells[1].Value.ToString()), double.Parse(this.dataGridView1.Rows[de].Cells[2].Value.ToString()),
                         Convert.ToDouble(this.dataGridView1.Rows[de].Cells[3].Value), Convert.ToDouble(this.dataGridView1.Rows[de].Cells[4].Value),
                           enumXYZUMoveType);

                                if (!flag2)
                                {
                                    this.dataGridView1.Rows[de].DefaultCellStyle.BackColor = Color.Red;
                                }
                                Thread.Sleep(1000);
                                foreach (var item in Vision.GetHimageList().Keys)
                                {
                                    if (this.dataGridView1.Rows[de].Cells[7].Value.ToString() == Vision.GetSaveImageInfo(item).AxisGrot)
                                    {
                                        int det = int.Parse(this.dataGridView1.Rows[de].Cells[5].Value.ToString());
                                        if (det <= 0)
                                        {
                                            Vision.GetRunNameVision(item).HobjClear();
                                            Vision.GetRunNameVision(item).Image(Vision.GetRunNameVision(item).GetCam().GetImage());
                                        }
                                        else
                                        {
                                            Vision.GetRunNameVision(item).ReadCamImage(this.dataGridView1.Rows[de].Cells[5].Value.ToString(), int.Parse(this.dataGridView1.Rows[de].Cells[5].Value.ToString()));
                                        }
                                        Vision.GetRunNameVision(item).ShowObj();
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                        });
                        thread.IsBackground = true;
                        thread.Start();
                    }
                    else
                    {
                        MessageBox.Show("非当前生产的产品无法移动!");
                    }
                    this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.isCot)
            {
                try
                {
                    xYZPoints[e.RowIndex].Name = this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    double eX;
                    if (double.TryParse(this.dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), out eX))
                    {
                        xYZPoints[e.RowIndex].X = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "X值错误");
                    }
                    if (double.TryParse(this.dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(), out eX))
                    {
                        xYZPoints[e.RowIndex].Y = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "Y值错误");
                    }

        

                    if (this.dataGridView1.Rows[e.RowIndex].Cells[3].Value!=null)
                    {

                        xYZPoints[e.RowIndex].Z = Convert.ToDouble(this.dataGridView1.Rows[e.RowIndex].Cells[3].Value);
         
                    }
                    else
                    {
                        //MessageBox.Show(e.RowIndex.ToString() + "Z值错误");
                    }
                    if (this.dataGridView1.Rows[e.RowIndex].Cells[4].Value!=null)
                    {
                        if (double.TryParse(this.dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString(), out eX))
                        {
                            xYZPoints[e.RowIndex].U = eX;
                        }
                        else
                        {
                            MessageBox.Show(e.RowIndex.ToString() + "U值错误");
                        }
                    }
             
                    int iX;

                    if (int.TryParse(this.dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString(), out iX))
                    {
                        xYZPoints[e.RowIndex].ID = iX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "ID值错误");
                    }

                    Enum.TryParse<EnumXYZUMoveType>(this.dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString(), out EnumXYZUMoveType enumXYZUMoveType);

                    xYZPoints[e.RowIndex].isMove = enumXYZUMoveType;
                    if (this.dataGridView1.Rows[e.RowIndex].Cells[7].Value != null)
                    {
                        xYZPoints[e.RowIndex].AxisGrabName = this.dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    }
                    this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;



                }
                catch (Exception)
                {
                }
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void 读取当前位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                int de = dataGridView1.SelectedCells[0].RowIndex;
                DebugCompiler.GetThis().DDAxis.GetAxisGroupPoints(this.dataGridView1.Rows[de].Cells[7].Value.ToString(), out double? XpT, out double? Ypt, out double? zpt, out double? u);
                xYZPoints[de].Name = this.dataGridView1.Rows[de].Cells[0].Value.ToString();
                xYZPoints[de].X = XpT.Value;
                xYZPoints[de].Y = Ypt.Value;
                if (zpt != null)
                {
                    xYZPoints[de].Z = zpt.Value;
                }
                if (u != null)
                {
                    xYZPoints[de].U = u.Value;
                }
                this.dataGridView1.Rows[de].Cells[1].Value = XpT;
                this.dataGridView1.Rows[de].Cells[2].Value = Ypt;
                this.dataGridView1.Rows[de].Cells[3].Value = zpt;
                this.dataGridView1.Rows[de].Cells[4].Value = u;
            }
            catch (Exception ex)
            {


            }
            isCot = false;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                propertyGrid3.SelectedObject = DebugCompiler.GetThis().DDAxis.ListTray[listBox5.SelectedIndex];
                trayControl1.SetTray(DebugCompiler.GetThis().DDAxis.ListTray[listBox5.SelectedIndex]);
            }
            catch (Exception)
            {
            }
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MatrixC = DebugCompiler.GetThis().ListMatrix[listBox6.SelectedIndex];
                propertyGrid4.SelectedObject = MatrixC;
            }
            catch (Exception)
            {
            }
        }
        private void 添加点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.GetThis().ListMatrix.Add(new 工艺库.MatrixC());
                listBox6.Items.Add(DebugCompiler.GetThis().ListMatrix.Count - 1);
            }
            catch (Exception)
            {
            }
        }

        private void 删除矩阵ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.GetThis().ListMatrix.RemoveAt(listBox6.SelectedIndex);
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
                        XYZPoint point = DebugCompiler.GetThis().DDAxis.GetToPoint(MatrixC.PointName);
                        if (point != null)
                        {
                            HalconDotNet.HOperatorSet.GenCircle(out HalconDotNet.HObject hObject1, point.Y, point.X, 10);
                            HalconDotNet.HOperatorSet.GenCrossContourXld(out HalconDotNet.HObject hObjectxs, point.Y, point.X, 10, 0);

                            HWindID2.OneResIamge.AddObj(hObject1.ConcatObj(hObjectxs), ColorResult.yellow);
                            HWindID2.OneResIamge.AddImageMassage(point.Y, point.X, "Mk1",ColorResult.green);
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
                        XYZPoint point = DebugCompiler.GetThis().DDAxis.GetToPoint(MatrixC.PointNameEnd);
                        if (point != null)
                        {
                            HalconDotNet.HOperatorSet.GenCircle(out HalconDotNet.HObject hObject1, point.Y, point.X, 10);
                            HalconDotNet.HOperatorSet.GenCrossContourXld(out HalconDotNet.HObject hObjectxs, point.Y, point.X, 10, 0);
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
        bool IThaing;
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    return;
                }
                this.isCot = true;
                dataGridView1.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView2.Rows.Clear();
                listBox3.Items.Clear();
                toolStripComboBox1.Items.Clear();
                listBox2.Items.Clear();
                Column10.Items.Clear();
                listBox4.Items.Clear();

                if (listBox1.SelectedIndex == 0)
                {
                    xYZPoints = DebugCompiler.GetThis().DDAxis.XyzPoints;
                    RelativelyPoint = null;
                    移动到点位ToolStripMenuItem.Enabled = true;
                }
                else
                {
                    productEX = RecipeCompiler.Instance.ProductEX[listBox1.SelectedItem.ToString()];
                    listBox4.Items.AddRange(productEX.Relativel.DicRelativelyPoint.Keys.ToArray());
                    toolStripLabel1.Text = listBox1.SelectedItem.ToString();
                    xYZPoints = RecipeCompiler.Instance.ProductEX[listBox1.SelectedItem.ToString()].DPoint;

                    HWindNt.ClearObj();
                    listBox3.Items.AddRange(productEX.Key_Navigation_Picture.Keys.ToArray());
                    if (listBox3.Items.Count != 0)
                    {
                        listBox3.SelectedIndex = 0;
                    }
                    if (listBox1.SelectedItem.ToString() == formula.Product.ProductionName)
                    {
                        移动到点位ToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        移动到点位ToolStripMenuItem.Enabled = false;
                    }
                }

                for (int i = 0; i < xYZPoints.Count; i++)
                {
                    int de = dataGridView1.Rows.Add();
                    XYZPoint xYZPoint = xYZPoints[i];
                    Column10.Items.AddRange(xYZPoint.Name);
                    toolStripComboBox1.Items.Add(xYZPoint.Name);
                    dataGridView1.Rows[de].Cells[0].Value = xYZPoint.Name;
                    dataGridView1.Rows[de].Cells[1].Value = xYZPoint.X;
                    dataGridView1.Rows[de].Cells[2].Value = xYZPoint.Y;
                    dataGridView1.Rows[de].Cells[3].Value = xYZPoint.Z;
                    dataGridView1.Rows[de].Cells[4].Value = xYZPoint.U;
                    dataGridView1.Rows[de].Cells[5].Value = xYZPoint.ID;
                    dataGridView1.Rows[de].Cells[6].Value = xYZPoint.isMove.ToString();
                    dataGridView1.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
                }
                listBox7.Items.Clear();
                if (productEX != null)
                {
                    for (int i = 0; i < productEX.Relativel.ListListPointName.Count; i++)
                    {
                        listBox7.Items.Add(i + 1);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.isCot = false;
        }

        private void 插入新点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                int de = 0;
                if (dataGridView1.SelectedCells.Count != 0)
                {
                    de = dataGridView1.SelectedCells[0].RowIndex + 1;
                }
                xYZPoints.Insert(de, new XYZPoint());
                dataGridView1.Rows.Insert(de, new DataGridViewRow());
                XYZPoint xYZPoint = xYZPoints[de];

                if (de != 0)
                {
                    ProjectINI.GetStrReturnInt(xYZPoints[de - 1].Name, out string names);

                    xYZPoint.Name = names + (ProjectINI.GetStrReturnInt(xYZPoints[de - 1].Name) + 1);
                }
                dataGridView1.Rows[de].Cells[0].Value = xYZPoint.Name;
                dataGridView1.Rows[de].Cells[1].Value = xYZPoint.X;
                dataGridView1.Rows[de].Cells[2].Value = xYZPoint.Y;
                dataGridView1.Rows[de].Cells[3].Value = xYZPoint.Z;
                dataGridView1.Rows[de].Cells[4].Value = xYZPoint.U;
                dataGridView1.Rows[de].Cells[5].Value = xYZPoint.ID;
                dataGridView1.Rows[de].Cells[6].Value = xYZPoint.isMove;
                xYZPoint.AxisGrabName = Column7.Items[0].ToString();
                dataGridView1.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            isCot = false;
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
                    toolStripButton2.PerformClick();
                }
                else if (e.KeyCode == Keys.F7)
                {
                    toolStripButton4.PerformClick();
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
                    List<XYZPoint> xYZP =RecipeCompiler.GetProductEX().DPoint;
                    XYZPoint point2 = DebugCompiler.GetThis().DDAxis.GetToPointFileProt(MatrixC.PointName);
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

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HWindNt.ClearObj();
                if (productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.ContainsKey(listBox2.SelectedItem.ToString()))
                {
                    HObject hObject = productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi[listBox2.SelectedItem.ToString()];

                    if (hObject.CountObj() != 0)
                    {
                        HWindNt.OneResIamge.AddObj(hObject);
                        HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple rows, out HTuple clos);
                        HWindNt.OneResIamge.AddImageMassage(rows, clos, listBox2.SelectedItem.ToString());
                    }

                }
                HWindNt.ShowImage();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 添加区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                string meassge = "创建区域";
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, "区域1", 100, 100);

                if (productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.ContainsKey(sd))
                {
                    meassge = "名称已存在";
                    goto st;
                }
                productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.Add(sd, new HalconDotNet.HObject());
                productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi[sd].GenEmptyObj();
                listBox2.Items.Add(sd);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 修改区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                HWindNt.ClearObj();

                if (productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.ContainsKey(listBox2.SelectedItem.ToString()))
                {
                    HTuple rows, cols, row2, cols2;
                    HObject hObject1 = productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi[listBox2.SelectedItem.ToString()];

                    if (hObject1.IsInitialized() && hObject1.CountObj() == 1)
                    {
                        HOperatorSet.SmallestRectangle1(hObject1, out rows, out cols, out row2, out cols2);
                        HOperatorSet.DrawRectangle1Mod(hWindowControl2.HalconWindow, rows, cols, row2, cols2,
                            out rows, out cols, out row2, out cols2);
                    }
                    else
                    {
                        HOperatorSet.DrawRectangle1(hWindowControl2.HalconWindow, out rows,
                           out cols, out row2, out cols2);
                    }

                    HalconDotNet.HOperatorSet.GenRectangle1(out HalconDotNet.HObject hObject, rows, cols, row2, cols2);
                    productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi[listBox2.SelectedItem.ToString()] = hObject;
                    HWindNt.OneResIamge.AddObj(hObject);
                    HalconDotNet.HOperatorSet.AreaCenter(hObject, out HalconDotNet.HTuple area, out rows, out HalconDotNet.HTuple clos);
                    HWindNt.OneResIamge.AddImageMassage(rows, clos, listBox2.SelectedItem.ToString());
                    HWindNt.ShowImage();
                }
                else
                {
                    MessageBox.Show(listBox2.SelectedItem.ToString() + "不存在");
                }
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

                if (productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.ContainsKey(listBox2.SelectedItem.ToString()))
                {
                    productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.Remove(listBox2.SelectedItem.ToString());
                }
                listBox2.Items.Remove(listBox2.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void 导入整图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择图片文件可多选";
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].ImagePath = openFileDialog.FileName;
                productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].Cler();
                HWindNt.SetImaage(productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].GetHObject());
                HWindNt.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void 添加导航图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string meassge = "创建导航图";
                string names = "导航图1";
                if (listBox4.Items.Count != 0)
                {
                    names = ProjectINI.GetStrReturnStr(listBox4.Items[listBox4.Items.Count - 1].ToString());
                }
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, names, 100, 100);
                if (productEX.Key_Navigation_Picture.ContainsKey(sd))
                {
                    meassge = "名称已存在";
                    goto st;
                }
                productEX.Key_Navigation_Picture.Add(sd, new ProductEX.Navigation_Picture());
                listBox3.Items.Add(sd);
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
                string meassge = "重命名";
                string names = listBox3.SelectedItem.ToString();
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, names, 100, 100);
                if (productEX.Key_Navigation_Picture.ContainsKey(sd))
                {
                    meassge = "名称已存在";
                    goto st;
                }
                productEX.Key_Navigation_Picture.Add(sd, productEX.Key_Navigation_Picture[names]);
                listBox3.Items.Add(sd);
                productEX.Key_Navigation_Picture.Remove(names);
                listBox3.Items.Remove(names);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 删除导航图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (productEX.Key_Navigation_Picture.ContainsKey(listBox3.SelectedItem.ToString()))
                {
                    productEX.Key_Navigation_Picture.Remove(listBox3.SelectedItem.ToString());
                }
                listBox3.Items.Remove(listBox3.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.Clear();
                HWindNt.ClearObj();
                listBox2.Items.AddRange(productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.Keys.ToArray());
                HWindNt.SetImaage(productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].GetHObject());
                foreach (var item in productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi)
                {
                    if (item.Value.IsInitialized())
                    {
                        HOperatorSet.AreaCenter(item.Value, out HalconDotNet.HTuple area, out HalconDotNet.HTuple rows, out HalconDotNet.HTuple clos);
                        HWindNt.OneResIamge.AddImageMassage(rows, clos, item.Key);
                        HWindNt.OneResIamge.AddObj(item.Value);
                    }
                }
                HWindNt.ShowImage();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.isCot)
            {
                try
                {
                    RelativelyPoint[e.RowIndex].Name = this.dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                    double eX;
                    if (double.TryParse(this.dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString(), out eX))
                    {
                        RelativelyPoint[e.RowIndex].X = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "X值错误");
                    }
                    if (double.TryParse(this.dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString(), out eX))
                    {
                        RelativelyPoint[e.RowIndex].Y = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "Y值错误");
                    }

                    if (double.TryParse(this.dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString(), out eX))
                    {
                        RelativelyPoint[e.RowIndex].Z = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "Z值错误");
                    }
                    if (double.TryParse(this.dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString(), out eX))
                    {
                        RelativelyPoint[e.RowIndex].U = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "U值错误");
                    }
                    int iX;

                    if (int.TryParse(this.dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString(), out iX))
                    {
                        RelativelyPoint[e.RowIndex].ID = iX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "ID值错误");
                    }

                    Enum.TryParse<EnumXYZUMoveType>(this.dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString(), out EnumXYZUMoveType enumXYZUMoveType);
                    RelativelyPoint[e.RowIndex].isMove = enumXYZUMoveType;
                    if (this.dataGridView2.Rows[e.RowIndex].Cells[7].Value != null)
                    {
                        RelativelyPoint[e.RowIndex].AxisGrabName = this.dataGridView2.Rows[e.RowIndex].Cells[7].Value.ToString();
                    }
                    this.dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                int det = dataGridView2.SelectedRows.Count;
                if (MessageBox.Show("删除点", "是否删除点位" + det, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int i = 0; i < det; i++)
                    {
                        RelativelyPoint.RemoveAt(dataGridView2.SelectedCells[0].RowIndex);
                        dataGridView2.Rows.RemoveAt(dataGridView2.SelectedCells[0].RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 插入轨迹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                int de = 0;
                if (dataGridView2.SelectedCells.Count != 0)
                {
                    de = dataGridView2.SelectedCells[0].RowIndex + 1;
                }
                RelativelyPoint.Insert(de, new XYZPoint());
                dataGridView2.Rows.Insert(de, new DataGridViewRow());
                XYZPoint xYZPoint = RelativelyPoint[de];
                if (de != 0)
                {
                    ProjectINI.GetStrReturnInt(RelativelyPoint[de - 1].Name, out string names);

                    xYZPoint.Name = names + (ProjectINI.GetStrReturnInt(RelativelyPoint[de - 1].Name) + 1);
                }
                dataGridView2.Rows[de].Cells[0].Value = xYZPoint.Name;
                dataGridView2.Rows[de].Cells[1].Value = xYZPoint.X;
                dataGridView2.Rows[de].Cells[2].Value = xYZPoint.Y;
                dataGridView2.Rows[de].Cells[3].Value = xYZPoint.Z;
                dataGridView2.Rows[de].Cells[4].Value = xYZPoint.U;
                dataGridView2.Rows[de].Cells[5].Value = xYZPoint.ID;
                dataGridView2.Rows[de].Cells[6].Value = xYZPoint.isMove;
                xYZPoint.AxisGrabName = dataGridViewComboBoxColumn4.Items[0].ToString();
                dataGridView2.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
            }
            catch (Exception ex)
            {

            }
            isCot = false;
        }

        private void 添加轨迹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                int de = dataGridView2.Rows.Add();
                XYZPoint xYZPoint = new XYZPoint();
                RelativelyPoint.Add(xYZPoint);
                xYZPoint.Name = "P" + de;
                dataGridView2.Rows[de].Cells[0].Value = xYZPoint.Name;
                dataGridView2.Rows[de].Cells[1].Value = xYZPoint.X;
                dataGridView2.Rows[de].Cells[2].Value = xYZPoint.Y;
                dataGridView2.Rows[de].Cells[3].Value = xYZPoint.Z;
                dataGridView2.Rows[de].Cells[4].Value = xYZPoint.U;
                dataGridView2.Rows[de].Cells[5].Value = xYZPoint.ID;
                dataGridView2.Rows[de].Cells[6].Value = xYZPoint.isMove;
                xYZPoint.AxisGrabName = dataGridViewComboBoxColumn4.Items[0].ToString();
                dataGridView2.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
            }
            catch (Exception)
            {
            }
            isCot = false;

        }

        private void 读取轨迹位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
            }
        }

        private void 移动轨迹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int det = dataGridView3.SelectedCells.Count;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.isCot)
            {
                try
                {
                    if (RelNamePoints.Count <= e.RowIndex)
                    {
                        RelNamePoints.Add(new ProductEX.Relatively.PointType());
                    }
                    RelNamePoints[e.RowIndex].PointNmae = this.dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();
                    if (this.dataGridView3.Rows[e.RowIndex].Cells[2].Value != null)
                    {
                        RelNamePoints[e.RowIndex].RelativeLyPiintName = this.dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();
                    }
                    if (this.dataGridView3.Rows[e.RowIndex].Cells[1].Value != null)
                    {
                        Enum.TryParse<ProductEX.Relatively.EnumPointType>(this.dataGridView3.Rows[e.RowIndex].Cells[1].Value.ToString(), out ProductEX.Relatively.EnumPointType enumvet);
                        RelNamePoints[e.RowIndex].EnumPointTyp = enumvet;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void 删除ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (!this.isCot)
            {

                try
                {
                    int det = dataGridView3.SelectedCells.Count;
                    if (MessageBox.Show("删除点", "是否删除点位" + det, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        for (int i = 0; i < det; i++)
                        {
                            RelNamePoints.RemoveAt(dataGridView3.SelectedCells[0].RowIndex);
                            dataGridView3.Rows.RemoveAt(dataGridView3.SelectedCells[0].RowIndex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                int de = dataGridView1.SelectedCells[0].RowIndex;
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        productEX.Relativel.RelativelyId = 0;
                        toolStripLabel3.Text = "流程步:" + productEX.Relativel.RelativelyId;
                        XYZPoint xYZPoint = xYZPoints[toolStripComboBox1.SelectedIndex];
                        zoer = xYZPoint;
                        bool flag2 = DebugCompiler.GetThis().DDAxis.SetXYZ1Points(xYZPoint.AxisGrabName, 15, xYZPoint.X, xYZPoint.Y, xYZPoint.Z, xYZPoint.U, xYZPoint.isMove);
                        if (flag2)
                        {
                            for (int i = 0; i < dataGridView2.Rows.Count; i++)
                            {

                                this.dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.White;
                            }
                        }
                        else
                        {
                            MessageBox.Show("移动失败");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
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
                int de = dataGridView1.SelectedCells[0].RowIndex;
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        if (productEX.Relativel.RelativelyId == 0)
                        {
                            return;
                        }
                        if (productEX.Relativel.RelativelyId > 0)
                        {
                            productEX.Relativel.RelativelyId--;
                        }

                        this.dataGridView2.Rows[productEX.Relativel.RelativelyId].DefaultCellStyle.BackColor = Color.Yellow;
                        toolStripLabel3.Text = "流程步:" + productEX.Relativel.RelativelyId;
                        XYZPoint xYZPoi = RelativelyPoint[productEX.Relativel.RelativelyId];
                        XYZPoint xYz = new XYZPoint()
                        {
                            isMove = xYZPoi.isMove,
                            AxisGrabName = xYZPoi.AxisGrabName,
                            X = zoer.X - xYZPoi.X,
                            Y = zoer.Y - xYZPoi.Y,
                            U = zoer.U - xYZPoi.U,
                            ID = xYZPoi.ID,
                            Name = xYZPoi.Name,
                            Z = zoer.Z - xYZPoi.Z,
                        };
                        zoer = xYz;
                        bool flag2;
                        if (xYz.isMove == EnumXYZUMoveType.先移动再旋转)
                        {
                            flag2 = DebugCompiler.GetThis().DDAxis.SetXYZ1Points(xYz.AxisGrabName, 15, xYz.X, xYz.Y, xYz.Z, xYz.U, EnumXYZUMoveType.先旋转再移动);
                        }
                        if (xYz.isMove == EnumXYZUMoveType.先旋转再移动)
                        {
                            flag2 = DebugCompiler.GetThis().DDAxis.SetXYZ1Points(xYz.AxisGrabName, 15, xYz.X, xYz.Y, xYz.Z, xYz.U, EnumXYZUMoveType.先移动再旋转);
                        }
                        else
                        {
                            flag2 = DebugCompiler.GetThis().DDAxis.SetXYZ1Points(xYz.AxisGrabName, 15, xYz.X, xYz.Y, xYz.Z, xYz.U, xYz.isMove);
                        }

                        if (flag2)
                        {

                            Thread.Sleep(500);
                            foreach (var item in Vision.GetHimageList().Keys)
                            {
                                if (xYz.AxisGrabName == Vision.GetSaveImageInfo(item).AxisGrot)
                                {
                                    if (productEX.Relativel.RelativelyId <= 0)
                                    {
                                        Vision.GetRunNameVision(item).Image(Vision.GetRunNameVision(item).GetCam().GetImage());
                                    }
                                    else
                                    {
                                        Vision.GetRunNameVision(item).ReadCamImage(productEX.Relativel.RelativelyId.ToString(), productEX.Relativel.RelativelyId);
                                    }
                                    break;
                                }
                            }
                            this.dataGridView2.Rows[productEX.Relativel.RelativelyId].DefaultCellStyle.BackColor = Color.Blue;
                        }
                        else
                        {
                            this.dataGridView2.Rows[productEX.Relativel.RelativelyId].DefaultCellStyle.BackColor = Color.Red;
                            MessageBox.Show("移动失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("移动失败" + ex.Message);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        XYZPoint zoer;
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                int de = dataGridView1.SelectedCells[0].RowIndex;
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        XYZPoint xYZPoi = RelativelyPoint[productEX.Relativel.RelativelyId];
                        XYZPoint xYz = new XYZPoint()
                        {
                            isMove = xYZPoi.isMove,
                            AxisGrabName = xYZPoi.AxisGrabName,
                            X = zoer.X + xYZPoi.X,
                            Y = xYZPoi.Y + zoer.Y,
                            U = xYZPoi.U + zoer.U,
                            ID = xYZPoi.ID,
                            Name = xYZPoi.Name,
                            Z = xYZPoi.Z + zoer.Z,
                        };
                        zoer = xYz;
                        productEX.Relativel.RelativelyId++;
                        this.dataGridView2.Rows[productEX.Relativel.RelativelyId - 1].DefaultCellStyle.BackColor = Color.Yellow;
                        bool flag2 = DebugCompiler.GetThis().DDAxis.SetXYZ1Points(xYz.AxisGrabName, 15, xYz.X, xYz.Y, xYz.Z, xYz.U, xYz.isMove);
                        if (flag2)
                        {
                            Thread.Sleep(500);
                            foreach (var item in Vision.GetHimageList().Keys)
                            {
                                if (xYz.AxisGrabName == Vision.GetSaveImageInfo(item).AxisGrot)
                                {

                                    if (productEX.Relativel.RelativelyId <= 0)
                                    {
                                        Vision.GetRunNameVision(item).Image(Vision.GetRunNameVision(item).GetCam().GetImage());
                                    }
                                    else
                                    {
                                        Vision.GetRunNameVision(item).ReadCamImage(productEX.Relativel.RelativelyId.ToString(), productEX.Relativel.RelativelyId);
                                    }
                                    break;
                                }
                            }
                            this.dataGridView2.Rows[productEX.Relativel.RelativelyId - 1].DefaultCellStyle.BackColor = Color.GreenYellow;
                        }
                        else
                        {
                            if (productEX.Relativel.RelativelyId != 1)
                            {
                                this.dataGridView2.Rows[productEX.Relativel.RelativelyId - 1].DefaultCellStyle.BackColor = Color.Red;
                            }
                            MessageBox.Show("移动失败");
                        }
                        toolStripLabel3.Text = "流程步:" + productEX.Relativel.RelativelyId;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("移动失败" + ex.Message);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView3_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox4.SelectedItem == null)
                {
                    return;
                }
                isCot = true;

                RelativelyPoint = productEX.Relativel.DicRelativelyPoint[listBox4.SelectedItem.ToString()];
                dataGridView2.Rows.Clear();
                if (RelativelyPoint != null)
                {
                    for (int i = 0; i < RelativelyPoint.Count; i++)
                    {
                        int de = dataGridView2.Rows.Add();
                        XYZPoint xYZPoint = RelativelyPoint[i];
                        dataGridView2.Rows[de].Cells[0].Value = xYZPoint.Name;
                        dataGridView2.Rows[de].Cells[1].Value = xYZPoint.X;
                        dataGridView2.Rows[de].Cells[2].Value = xYZPoint.Y;
                        dataGridView2.Rows[de].Cells[3].Value = xYZPoint.Z;
                        dataGridView2.Rows[de].Cells[4].Value = xYZPoint.U;
                        dataGridView2.Rows[de].Cells[5].Value = xYZPoint.ID;
                        dataGridView2.Rows[de].Cells[6].Value = xYZPoint.isMove.ToString();
                        dataGridView2.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isCot = false;
        }



        private void 添加轨迹ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string meassge = "创建轨迹";
                string names = "轨迹1";
                if (listBox4.Items.Count != 0)
                {
                    names = ProjectINI.GetStrReturnStr(listBox4.Items[listBox4.Items.Count - 1].ToString());
                }
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, names, 100, 100);
                if (sd == "")
                {
                    return;
                }
                if (productEX.Relativel.DicRelativelyPoint.ContainsKey(sd))
                {
                    meassge = "名称已存在";
                    goto st;
                }
                productEX.Relativel.DicRelativelyPoint.Add(sd, new List<XYZPoint>());
                listBox4.Items.Add(sd);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 删除轨迹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                productEX.Relativel.DicRelativelyPoint.Remove(listBox4.SelectedItem.ToString());
                listBox4.Items.RemoveAt(listBox4.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 重命名ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string meassge = "重命名";
                string names = listBox4.SelectedItem.ToString();
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, names, 100, 100);
                if (productEX.Relativel.DicRelativelyPoint.ContainsKey(sd))
                {
                    meassge = "名称已存在";
                    goto st;
                }
                productEX.Relativel.DicRelativelyPoint.Add(sd, productEX.Relativel.DicRelativelyPoint[names]);
                listBox4.Items.Add(sd);
                productEX.Relativel.DicRelativelyPoint.Remove(names);
                listBox4.Items.Remove(names);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.isCot = true;
                Column12.Items.Clear();
                Column12.Items.AddRange(productEX.Relativel.DicRelativelyPoint.Keys.ToArray());
                dataGridView3.Rows.Clear();
                RelNamePoints = productEX.Relativel.ListListPointName[listBox7.SelectedIndex];
                for (int i = 0; i < RelNamePoints.Count; i++)
                {
                    int de = dataGridView3.Rows.Add();
                    dataGridView3.Rows[de].Cells[0].Value = RelNamePoints[i].PointNmae;
                    dataGridView3.Rows[de].Cells[1].Value = RelNamePoints[i].EnumPointTyp.ToString();
                    dataGridView3.Rows[de].Cells[2].Value = RelNamePoints[i].RelativeLyPiintName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.isCot = false;
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void 添加轨迹ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                productEX.Relativel.ListListPointName.Add(new List<ProductEX.Relatively.PointType>());
                listBox7.Items.Add(productEX.Relativel.ListListPointName.Count());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void 删除轨迹ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                productEX.Relativel.ListListPointName.RemoveAt(listBox7.SelectedIndex);
                listBox7.Items.RemoveAt(listBox7.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                if (toolStripComboBox1.SelectedItem != null)
                {
                    List<XYZPoint> xes = new List<XYZPoint>();
                    XYZPoint xYZPoint0 = productEX.GetPoint(toolStripComboBox1.SelectedItem.ToString());
                    int index = productEX.GetPointIntdx(toolStripComboBox1.SelectedItem.ToString());
                    if (xYZPoint0 != null)
                    {
                        List<XYZPoint> xYZPoints = new List<XYZPoint>();
                        for (int i = index; i < productEX.DPoint.Count; i++)
                        {
                            if (productEX.DPoint[i].AxisGrabName == xYZPoint0.AxisGrabName)
                            {
                                xYZPoints.Add(productEX.DPoint[i]);
                            }
                        }
                        XYZPoint xYZPoint1 = new XYZPoint();
                        xYZPoint1.X = 0;
                        xYZPoint1.Y = 0;
                        xYZPoint1.Z = 0;
                        xYZPoint1.U = 0;
                        xYZPoint1.AxisGrabName = xYZPoint0.AxisGrabName;
                        xYZPoint1.ID = xYZPoint0.ID;
                        xYZPoint1.isMove = xYZPoint0.isMove;
                        xYZPoint1.Name = "PX1";
                        xes.Add(xYZPoint1);

                        for (int i = 0; i < xYZPoints.Count - 1; i++)
                        {
                            xYZPoint0 = xYZPoints[i];
                            XYZPoint xYZPoint2 = xYZPoints[i + 1];
                            xYZPoint1 = new XYZPoint();
                            xYZPoint1.X = Math.Round(xYZPoint2.X - xYZPoint0.X, 2);
                            xYZPoint1.Y = Math.Round(xYZPoint2.Y - xYZPoint0.Y, 2);
                            xYZPoint1.Z = Math.Round(xYZPoint2.Z - xYZPoint0.Z, 2);
                            xYZPoint1.U = Math.Round(xYZPoint2.U - xYZPoint0.U, 2);
                            xYZPoint1.AxisGrabName = xYZPoint0.AxisGrabName;
                            xYZPoint1.ID = xYZPoint2.ID;
                            xYZPoint1.isMove = xYZPoint2.isMove;
                            xYZPoint1.Name = "PX" + (i + 2);
                            xes.Add(xYZPoint1);
                        }
                        productEX.Relativel.DicRelativelyPoint[listBox4.SelectedItem.ToString()].AddRange(xes);
                        dataGridView2.Rows.Clear();
                        if (RelativelyPoint != null)
                        {
                            for (int i = 0; i < RelativelyPoint.Count; i++)
                            {
                                int de = dataGridView2.Rows.Add();
                                XYZPoint xYZPoint = RelativelyPoint[i];
                                dataGridView2.Rows[de].Cells[0].Value = xYZPoint.Name;
                                dataGridView2.Rows[de].Cells[1].Value = xYZPoint.X;
                                dataGridView2.Rows[de].Cells[2].Value = xYZPoint.Y;
                                dataGridView2.Rows[de].Cells[3].Value = xYZPoint.Z;
                                dataGridView2.Rows[de].Cells[4].Value = xYZPoint.U;
                                dataGridView2.Rows[de].Cells[5].Value = xYZPoint.ID;
                                dataGridView2.Rows[de].Cells[6].Value = xYZPoint.isMove.ToString();
                                dataGridView2.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
                            }
                        }
                    }

                }
                else
                {
                    MessageBox.Show("未选择起点");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isCot = false;
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void 导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 导入ExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void 导出到全局位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否导出到位置信息?", "导出托盘位置", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    List<PointFile> pointFiles = DebugCompiler.GetThis().DDAxis.ListTray[listBox5.SelectedIndex].GetPoints();
                    List<XYZPoint> xYZP = DebugCompiler.GetThis().DDAxis.XyzPoints; 
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
                      List< PointFile>   pointFiles=  DebugCompiler.GetThis().DDAxis.ListTray[listBox5.SelectedIndex].GetPoints();
                        List<XYZPoint> xYZP = RecipeCompiler.GetProductEX().DPoint;
                        //XYZPoint point2 = DebugCompiler.GetThis().DDAxis.GetToPointFileProt(MatrixC.PointName);
                        for (int i = 0; i < pointFiles.Count; i++)
                        {
                            xYZP.Add(new XYZPoint() { Name = "TP" + (i + 1), 
                                X = pointFiles[i].X.Value, 
                                Y = pointFiles[i].Y.Value,
                                Z = pointFiles[i].Z.Value,
                                ID = i + 1,});
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
