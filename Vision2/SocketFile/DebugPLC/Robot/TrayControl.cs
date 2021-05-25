using HalconDotNet;
using System;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC.Robot
{
    public partial class TrayControl : UserControl
    {
        Vision2.vision.HWindID hWindID;
        public TrayControl()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            InitializeComponent();

            //Type dgvType = this.dataGridView2.GetType();
            //PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //pi.SetValue(this.dataGridView2, true, null);


        }
        public TrayControl(TrayRobot trayRobot) : this()
        {
            SetTray(trayRobot);
        }
        bool ISChanged = true;
        public void SetTray(TrayRobot trayRobot)
        {
            ISChanged = true;
            if (tray != null)
            {
                tray.SetControl(null);
            }
            tray = trayRobot;
            numericUpDown2.Value = tray.XNumber;
            numericUpDown1.Value = tray.YNumber;

            tray.SetControl(this);
            dataGridView1.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView1.Rows.Add(4);

            try
            {
                if (tray.P1 != null)
                {
                    dataGridView1.Rows[0].Cells[0].Value = tray.P1.X;
                    dataGridView1.Rows[0].Cells[1].Value = tray.P1.Y;
                    dataGridView1.Rows[0].Cells[2].Value = tray.P1.Z;
                    dataGridView1.Rows[1].Cells[0].Value = tray.P2.X;
                    dataGridView1.Rows[1].Cells[1].Value = tray.P2.Y;
                    dataGridView1.Rows[1].Cells[2].Value = tray.P2.Z;

                    dataGridView1.Rows[2].Cells[0].Value = tray.P3.X;
                    dataGridView1.Rows[2].Cells[1].Value = tray.P3.Y;
                    dataGridView1.Rows[2].Cells[2].Value = tray.P3.Z;

                    dataGridView1.Rows[3].Cells[0].Value = tray.P4.X;
                    dataGridView1.Rows[3].Cells[1].Value = tray.P4.Y;
                    dataGridView1.Rows[3].Cells[2].Value = tray.P4.Z;
                    dataGridView3.Rows.Add(tray.ListX.Length);
                    for (int i = 0; i < tray.ListX.Length; i++)
                    {

                        dataGridView3.Rows[i].Cells[0].Value = tray.ListX.TupleSelect(i).TupleString("0.02f");
                        dataGridView3.Rows[i].Cells[1].Value = tray.ListY.TupleSelect(i).TupleString("0.02f");
                    }


                    HOperatorSet.GenRegionLine(out HObject hObject1, tray.P1.X, tray.P1.Y, tray.P2.X, tray.P2.Y);
                    HOperatorSet.GenRegionLine(out HObject hObject2, tray.P2.X, tray.P2.Y, tray.P3.X, tray.P3.Y);
                    HOperatorSet.GenRegionLine(out HObject hObject3, tray.P3.X, tray.P3.Y, tray.P4.X, tray.P4.Y);
                    HOperatorSet.GenRegionLine(out HObject hObject4, tray.P4.X, tray.P4.Y, tray.P1.X, tray.P1.Y);

                    hObject1 = hObject1.ConcatObj(hObject2);
                    hObject1 = hObject1.ConcatObj(hObject3);
                    hObject1 = hObject1.ConcatObj(hObject4);
                    HOperatorSet.Union1(hObject1, out hObject1);
                    HOperatorSet.SmallestRectangle1(hObject1, out HTuple rwo, out HTuple col, out HTuple lieng, out HTuple cl1);

                    HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, lieng + 200, cl1 + 50);

                    HObject hObject5 = TrayRobot.GenArrowContourXld(20, 20, 100, 20, 10, 20);
                    HObject hObject6 = TrayRobot.GenArrowContourXld(20, 20, 20, 100, 10, 20);

                    hWindowControl1.HalconWindow.ClearWindow();
                    TrayRobot.Disp_message(hWindowControl1.HalconWindow, "X", 100, 20);
                    TrayRobot.Disp_message(hWindowControl1.HalconWindow, "Y", 20, 100);
                    hWindowControl1.HalconWindow.SetColor("red");
                    HOperatorSet.DispObj(hObject5, hWindowControl1.HalconWindow);

                    HOperatorSet.DispObj(hObject1, hWindowControl1.HalconWindow);
                    HOperatorSet.SetColor(hWindowControl1.HalconWindow, "blue");
                    HOperatorSet.DispObj(hObject2, hWindowControl1.HalconWindow);
                    HOperatorSet.SetColor(hWindowControl1.HalconWindow, "green");
                    HOperatorSet.DispObj(hObject6, hWindowControl1.HalconWindow);
                    HOperatorSet.DispObj(hObject3, hWindowControl1.HalconWindow);

                    HOperatorSet.SetColor(hWindowControl1.HalconWindow, "yellow");
                    HOperatorSet.DispObj(hObject4, hWindowControl1.HalconWindow);
                    HOperatorSet.GenCrossContourXld(out HObject hObject, tray.P1.X, tray.P1.Y, 10, 0);
                    HOperatorSet.DispObj(hObject, hWindowControl1.HalconWindow);
                    HOperatorSet.GenCrossContourXld(out hObject, tray.P2.X, tray.P2.Y, 10, 0);
                    HOperatorSet.DispObj(hObject, hWindowControl1.HalconWindow);
                    HOperatorSet.GenCrossContourXld(out hObject, tray.P3.X, tray.P3.Y, 10, 0);
                    HOperatorSet.DispObj(hObject, hWindowControl1.HalconWindow);
                    HOperatorSet.GenCrossContourXld(out hObject, tray.P4.X, tray.P4.Y, 10, 0);
                    HOperatorSet.DispObj(hObject, hWindowControl1.HalconWindow);
                    TrayRobot.Disp_message(hWindowControl1.HalconWindow, "1", tray.P1.X.Value, tray.P1.Y.Value);
                    TrayRobot.Disp_message(hWindowControl1.HalconWindow, "2", tray.P2.X.Value, tray.P2.Y.Value);
                    TrayRobot.Disp_message(hWindowControl1.HalconWindow, "3", tray.P3.X.Value, tray.P3.Y.Value);
                    TrayRobot.Disp_message(hWindowControl1.HalconWindow, "4", tray.P4.X.Value, tray.P4.Y.Value);
                }

            }
            catch (Exception ex)
            {
            }
            UP();
            ISChanged = false;
        }
        TrayRobot tray;
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                tray.P1 = new PointFile();
                tray.P2 = new PointFile();
                tray.P3 = new PointFile();
                tray.P4 = new PointFile();
                tray.P1.Z = Convert.ToDouble(dataGridView1.Rows[0].Cells[2].Value);

                tray.P2.Z = Convert.ToDouble(dataGridView1.Rows[1].Cells[2].Value);
   
                tray.P3.Z = Convert.ToDouble(dataGridView1.Rows[2].Cells[2].Value);
          
                tray.P4.Z = Convert.ToDouble(dataGridView1.Rows[3].Cells[2].Value);

                tray.P1.X = Convert.ToDouble(dataGridView1.Rows[0].Cells[0].Value);
                tray.P1.Y = Convert.ToDouble(dataGridView1.Rows[0].Cells[1].Value);

                tray.P2.X = Convert.ToDouble(dataGridView1.Rows[1].Cells[0].Value);
                tray.P2.Y = Convert.ToDouble(dataGridView1.Rows[1].Cells[1].Value);

                tray.P3.X = Convert.ToDouble(dataGridView1.Rows[2].Cells[0].Value);
                tray.P3.Y = Convert.ToDouble(dataGridView1.Rows[2].Cells[1].Value);

                tray.P4.X = Convert.ToDouble(dataGridView1.Rows[3].Cells[0].Value);
                tray.P4.Y = Convert.ToDouble(dataGridView1.Rows[3].Cells[1].Value);

                HOperatorSet.GenRegionLine(out HObject hObject1, tray.P1.X, tray.P1.Y, tray.P2.X, tray.P2.Y);
                HOperatorSet.GenRegionLine(out HObject hObject2, tray.P2.X, tray.P2.Y, tray.P3.X, tray.P3.Y);
                HOperatorSet.GenRegionLine(out HObject hObject3, tray.P3.X, tray.P3.Y, tray.P4.X, tray.P4.Y);
                HOperatorSet.GenRegionLine(out HObject hObject4, tray.P4.X, tray.P4.Y, tray.P1.X, tray.P1.Y);
                hObject1 = hObject1.ConcatObj(hObject2);
                hObject1 = hObject1.ConcatObj(hObject3);
                hObject1 = hObject1.ConcatObj(hObject4);
                HOperatorSet.Union1(hObject1, out hObject1);
                HOperatorSet.SmallestRectangle1(hObject1, out HTuple rwo, out HTuple col, out HTuple lieng, out HTuple cl1);

                HOperatorSet.SetPart(hWindowControl1.HalconWindow, rwo, col, lieng * 1.5, cl1 * 1.5);

                //HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, lieng * 1, cl1 * 1);

                HObject hObject5 = TrayRobot.GenArrowContourXld(20, 20, 100, 20, 10, 20);
                HObject hObject6 = TrayRobot.GenArrowContourXld(20, 20, 20, 100, 10, 20);

                hWindowControl1.HalconWindow.ClearWindow();

                TrayRobot.Disp_message(hWindowControl1.HalconWindow, "X", 100, 20);
                TrayRobot.Disp_message(hWindowControl1.HalconWindow, "Y", 20, 100, false, "green");
                hWindowControl1.HalconWindow.SetColor("red");
                HOperatorSet.DispObj(hObject5, hWindowControl1.HalconWindow);

                HOperatorSet.DispObj(hObject1, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "blue");
                HOperatorSet.DispObj(hObject2, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "green");
                HOperatorSet.DispObj(hObject6, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(hObject3, hWindowControl1.HalconWindow);
                TrayRobot.Calculate(tray.XNumber, tray.YNumber, tray.P1, tray.P2, tray.P3, tray.P4, out PointFile[] posint);
                hWindowControl1.HalconWindow.SetColor("red");
      
               tray.Calculate(tray.XNumber, tray.YNumber, tray.P1, tray.P2, tray.P3, tray.P4, tray.X2Number, tray.Y2Number, tray.P5, tray.P6, tray.P7, tray.P8,
                        out PointFile[] pointFiles);


                //tray.Calculate(out HTuple listx, out HTuple listy, hWindowControl1.HalconWindow);
                dataGridView3.Rows.Clear();

                for (int i2 = 0; i2 < tray.ListX.Length; i2++)
                {
                    int dt = dataGridView3.Rows.Add();
                    dataGridView3.Rows[dt].Cells[0].Value = tray.ListX.TupleSelect(i2).TupleString("0.02f");
                    dataGridView3.Rows[dt].Cells[1].Value = tray.ListY.TupleSelect(i2).TupleString("0.02f");
                    TrayRobot.Disp_message(hWindowControl1.HalconWindow, i2 + 1, tray.ListX[i2], tray.ListY[i2], false, "red", "true");
                }
            }
            catch (Exception ex)
            {
            }

        }
        int height = 0;
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (height == 0)
                {
                    height = this.Height;
                }

                if (toolStripButton1.Text == "折叠")
                {
                    this.Height = toolStrip1.Height;
                    toolStripButton1.Text = "显示";
                }
                else
                {
                    toolStripButton1.Text = "折叠";
                    this.Height = height;
                }
            }
            catch (Exception)
            {
            }
        }

        public void UPsetTrayNumbar(int d)
        {
            try
            {
                d = d - 1;
                int rowt = d / tray.XNumber;
                int colt = d % tray.XNumber;
                dataGridView2.Rows[rowt].Cells[colt].Value = pictureBox2.Image;
                trayState.Text = "托盘:" + tray.Name + "  " + tray.Number + "/" + (tray.XNumber * tray.YNumber) + "(" + tray.XNumber + "*" + tray.YNumber + ")";
            }
            catch (Exception)
            {
            }
        }
        public void UP()
        {
            try
            {
                dataGridView2.RowHeadersWidth = 50;
                dataGridView2.Columns.Clear();
                trayState.Text = "托盘:" + tray.Name + "  " + tray.Number + "/" + (tray.XNumber * tray.YNumber) + "(" + tray.XNumber + "*" + tray.YNumber + ")";
                for (int i = 0; i < tray.XNumber; i++)
                {
                    DataGridViewImageColumn dataGridViewColumn = new DataGridViewImageColumn();
                    dataGridViewColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    dataGridViewColumn.HeaderText = "X" + (i + 1).ToString();
                    dataGridViewColumn.Width = 50;
                    dataGridView2.Columns.Add(dataGridViewColumn);
                }
                int cdt = 0;

                for (int i = 0; i < tray.YNumber; i++)
                {
                    int dt = dataGridView2.Rows.Add();
                    dataGridView2.Rows[dt].Height = 50;
                    for (int i2 = 0; i2 < dataGridView2.Rows[dt].Cells.Count; i2++)
                    {

                        if (tray.bitW[cdt] == 0)
                        {
                            dataGridView2.Rows[dt].Cells[i2].Value = pictureBox1.Image;
                        }
                        else
                        {
                            dataGridView2.Rows[dt].Cells[i2].Value = pictureBox2.Image;
                        }
                        cdt++;
                    }
                    //ListViewItem listViewItem = new ListViewItem();
                    //listViewItem.SubItems.Add(i.ToString());
                    //listViewItem.SubItems.Add(i.ToString());

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                tray.GetAxis().DebugFormShow();
            }
            catch (Exception)
            {
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void TrayControl_Load(object sender, EventArgs e)
        {
            try
            {
                hWindID = new Vision2.vision.HWindID();
                Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.DoubleBufferedDataGirdView(this.dataGridView2, true);
                Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
                Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView2);
                Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView3);
                dataGridView1.Rows.Add(4);
                if (tray == null)
                {
                    return;
                }

                hWindID.Initialize(hWindowControl1);

                hWindowControl1.HalconWindow.ClearWindow();
                if (tray.P3 != null)
                {
                    hWindowControl1.HalconWindow.SetPart(0, 0, tray.P3.X + 80, tray.P3.Y + 80);
                    hWindowControl1.HalconWindow.SetColor("red");
                    dataGridView3.Rows.Clear();
                    for (int i2 = 0; i2 < tray.ListX.Length; i2++)
                    {
                        int dt = dataGridView3.Rows.Add();
                        dataGridView3.Rows[dt].Cells[0].Value = tray.ListX.TupleSelect(i2).TupleString("0.02f");
                        dataGridView3.Rows[dt].Cells[1].Value = tray.ListY.TupleSelect(i2).TupleString("0.02f");
                        TrayRobot.Disp_message(hWindowControl1.HalconWindow, i2, tray.ListX[i2], tray.ListY[i2], false, "red", "true");
                    }
                }



            }
            catch (Exception)
            {
            }


        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (ISChanged)
                {
                    return;
                }
                tray.YNumber = (sbyte)numericUpDown1.Value;
                tray.XNumber = (sbyte)numericUpDown2.Value;
                UP();

            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (ISChanged)
                {
                    return;
                }
                tray.P1 = new PointFile();
                tray.P2 = new PointFile();
                tray.P3 = new PointFile();
                tray.P4 = new PointFile();
                tray.P1.X = Convert.ToDouble( dataGridView1.Rows[0].Cells[0].Value);

                tray.P1.Y = Convert.ToDouble(dataGridView1.Rows[0].Cells[1].Value);

                tray.P2.X = Convert.ToDouble(dataGridView1.Rows[1].Cells[0].Value);
                tray.P2.Y = Convert.ToDouble(dataGridView1.Rows[1].Cells[1].Value);

                tray.P3.X = Convert.ToDouble(dataGridView1.Rows[2].Cells[0].Value);
                tray.P3.Y = Convert.ToDouble(dataGridView1.Rows[2].Cells[1].Value);

                tray.P4.X = Convert.ToDouble(dataGridView1.Rows[3].Cells[0].Value);
                tray.P4.Y = Convert.ToDouble(dataGridView1.Rows[3].Cells[1].Value);

                HOperatorSet.GenRegionLine(out HObject hObject1, tray.P1.X, tray.P1.Y, tray.P2.X, tray.P2.Y);
                HOperatorSet.GenRegionLine(out HObject hObject2, tray.P2.X, tray.P2.Y, tray.P3.X, tray.P3.Y);
                HOperatorSet.GenRegionLine(out HObject hObject3, tray.P3.X, tray.P3.Y, tray.P4.X, tray.P4.Y);
                HOperatorSet.GenRegionLine(out HObject hObject4, tray.P4.X, tray.P4.Y, tray.P1.X, tray.P1.Y);
                hObject1 = hObject1.ConcatObj(hObject2);
                hObject1 = hObject1.ConcatObj(hObject3);
                hObject1 = hObject1.ConcatObj(hObject4);
                HOperatorSet.Union1(hObject1, out hObject1);
                HOperatorSet.SmallestRectangle1(hObject1, out HTuple rwo, out HTuple col, out HTuple lieng, out HTuple cl1);

                HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, lieng + 200, cl1 + 50);

                HObject hObject5 = TrayRobot.GenArrowContourXld(20, 20, 100, 20, 10, 20);
                HObject hObject6 = TrayRobot.GenArrowContourXld(20, 20, 20, 100, 10, 20);

                hWindowControl1.HalconWindow.ClearWindow();
                TrayRobot.Disp_message(hWindowControl1.HalconWindow, "X", 100, 20);
                TrayRobot.Disp_message(hWindowControl1.HalconWindow, "Y", 20, 100);
                hWindowControl1.HalconWindow.SetColor("red");
                HOperatorSet.DispObj(hObject5, hWindowControl1.HalconWindow);

                HOperatorSet.DispObj(hObject1, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "blue");
                HOperatorSet.DispObj(hObject2, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "green");
                HOperatorSet.DispObj(hObject6, hWindowControl1.HalconWindow);
                HOperatorSet.DispObj(hObject3, hWindowControl1.HalconWindow);

                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "yellow");
                HOperatorSet.DispObj(hObject4, hWindowControl1.HalconWindow);
                HOperatorSet.GenCrossContourXld(out HObject hObject, tray.P1.X, tray.P1.Y, 10, 0);
                HOperatorSet.DispObj(hObject, hWindowControl1.HalconWindow);
                HOperatorSet.GenCrossContourXld(out hObject, tray.P2.X, tray.P2.Y, 10, 0);
                HOperatorSet.DispObj(hObject, hWindowControl1.HalconWindow);
                HOperatorSet.GenCrossContourXld(out hObject, tray.P3.X, tray.P3.Y, 10, 0);
                HOperatorSet.DispObj(hObject, hWindowControl1.HalconWindow);
                HOperatorSet.GenCrossContourXld(out hObject, tray.P4.X, tray.P4.Y, 10, 0);
                HOperatorSet.DispObj(hObject, hWindowControl1.HalconWindow);
                TrayRobot.Disp_message(hWindowControl1.HalconWindow, "1", tray.P1.X.Value, tray.P1.Y.Value);
                TrayRobot.Disp_message(hWindowControl1.HalconWindow, "2", tray.P2.X.Value, tray.P2.Y.Value);
                TrayRobot.Disp_message(hWindowControl1.HalconWindow, "3", tray.P3.X.Value, tray.P3.Y.Value);
                TrayRobot.Disp_message(hWindowControl1.HalconWindow, "4", tray.P4.X.Value, tray.P4.Y.Value);
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                tray.UPsetTrayNumbar((e.RowIndex) * dataGridView2.Columns.Count + e.ColumnIndex + 1);

            }

            catch (Exception)
            {

            }
        }

        private void dataGridView2_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {


        }
    }
}
