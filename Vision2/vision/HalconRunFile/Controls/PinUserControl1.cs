using ErosSocket.DebugPLC;
using HalconDotNet;
using Microsoft.VisualBasic;
using System;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.DataGridViewF;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.RunProgramFile.Select_shape_Min_Max;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class PinUserControl1 : UserControl
    {
        public PinUserControl1()
        {
            InitializeComponent();
            StCon.AddCon(this.dataGridView2);
            height = this.Height;
        }
        private PinT PinTTD;
        private HalconRun halconRun;
        int height;
        public PinUserControl1(PinT pinT) : this()
        {
            this.UpDat(pinT);
        }
        public void UpDat(PinT pind)
        {
            try
            {
                this.PinTTD = pind;
                this.isup = true;
                comboBox2.Items.Add("");
                comboBox3.Items.Add("");
                comboBox4.Items.Add("");
                comboBox5.Items.Add("");
                foreach (var item in pind.Dic_Measure.Keys_Measure.Keys)
                {
                    listBox1.Items.Add(item);
                    comboBox2.Items.Add(item);
                    comboBox3.Items.Add(item);
                    comboBox4.Items.Add(item);
                    comboBox5.Items.Add(item);
                }
         
                this.Column1.Items.AddRange(Enum.GetNames(typeof(Enum_Select_Type)));
                this.halconRun = pind.GetPThis() as HalconRun;
              
                comboBox2.SelectedItem = this.PinTTD.XAxisName;
                comboBox3.SelectedItem = this.PinTTD.YAxisName;
                comboBox4.SelectedItem = this.PinTTD.X2AxisName;
                comboBox5.SelectedItem = this.PinTTD.Y2AxisName;
             
                checkBox2.Checked = this.PinTTD.SelsMax;
                checkBox3.Checked = PinTTD.IsHtoB;
                numericUpDown17.Value = (decimal)this.PinTTD.CloseCirt;
                numericUpDown18.Value = (decimal)this.PinTTD.OpeningValue;
                numericUpDown16.Value = (decimal)this.PinTTD.ErosionHeightValue;
                numericUpDown15.Value = (decimal)this.PinTTD.ErosionWaightValue;
                this.numericUpDown10.Value = this.PinTTD.PintWrt;
                if (PinTTD.IsHtoB)
                {
                    numericUpDown2.Enabled = false;
                    numUpDownHeigthMax.Enabled = false;
                    numUpDownHeigthMin.Enabled = false;
                }
                else
                {
                    numericUpDown2.Enabled = true;
                    numUpDownHeigthMax.Enabled = true;
                    numUpDownHeigthMin.Enabled = true;
                }
                this.comboBox1.SelectedIndex = this.PinTTD.EnveType;
                this.numericUpDownAreaMax.Value = (decimal)this.PinTTD.Max.TupleSelect(0).D;
                this.numUpDownHeigthMax.Value = (decimal)this.PinTTD.Max.TupleSelect(1).D;
                this.checkBox1.Checked = this.PinTTD.isCTX;
                if (this.PinTTD.EnveType == 0)
                {
                    this.checkBox1.Enabled = true;
                }
                else
                {
                    this.checkBox1.Enabled = false;
                }
                dataGridView3.Rows.Clear();
                for (int i = 0; i < this.PinTTD.MRows.Length; i++)
                {
                    int det = dataGridView3.Rows.Add();
                    dataGridView3.Rows[det].Cells[0].Value = this.PinTTD.MColumns.TupleSelect(i);
                    dataGridView3.Rows[det].Cells[1].Value = this.PinTTD.MRows.TupleSelect(i);
                }
                if (this.checkBox1.Checked)
                {
                    this.dataGridView2.Visible = true;
                    this.nudPinrow.Enabled = false;
                    while (this.PinTTD.ListCTY.Count < this.PinTTD.ColumnNumber)
                    {
                        this.PinTTD.ListCTY.Add(this.PinTTD.ColumnMM);
                    }
                    this.dataGridView2.Rows.Add(this.PinTTD.ListCTY.Count);
                    for (int j = 0; j < this.PinTTD.ListCTY.Count; j++)
                    {
                        this.dataGridView2.Rows[j].Cells[0].Value = this.PinTTD.ListCTY[j];
                    }
                    Height = height;
                }
                else
                {
                    this.Height = Height - 81;
                    this.dataGridView2.Visible = false;
                    this.nudPinrow.Enabled = true;
                }

                this.numericUpDownThrMin.Value = this.PinTTD.ThrMin;
                this.numericUpDownThrMax.Value = this.PinTTD.ThrMax;
                this.numericUpDownAreaMin.Value = (decimal)this.PinTTD.Min.TupleSelect(0).D;
                //if (this.PinTTD.ToleranceMM <1)
                //{
                //    this.PinTTD.ToleranceMM = 1;
                //}
                this.nudPinCol.Value = (decimal)this.PinTTD.ColumnMM;
                this.nudPinrow.Value = (decimal)this.PinTTD.RowMM;
                this.nudPinTol3.Value = (decimal)this.PinTTD.ToleranceMM;
                this.numericUpDown6.Value = this.PinTTD.RowNumber;
                this.numericUpDown1.Value = this.PinTTD.ColumnNumber;
                this.numericUpDown4.Value = (decimal)this.PinTTD.DifferenceRow;
                this.numericUpDown5.Value = (decimal)this.PinTTD.DifferenceCol;
                this.numericUpDown7.Value = this.PinTTD.ThrMin2;
                this.numericUpDown8.Value = this.PinTTD.ThrMax2;
                this.numericUpDown9.Value = this.PinTTD.ThrMin3;


                this.numericUpDown2.Value = (decimal)this.PinTTD.PinHeightExternalPercent;
                this.numericUpDown11.Value = (decimal)this.PinTTD.PinHeightExternal;
                this.numericUpDown13.Value = this.PinTTD.ThrMinExternal;
                this.numericUpDown14.Value = this.PinTTD.ThrMaxExternal;
                this.numUpDownHeigthMin.Value = (decimal)this.PinTTD.Min.TupleSelect(1).D;
                this.numericUpDown3.Value = (decimal)this.PinTTD.ThrMinA;
                this.numericUpDown12.Value = (decimal)this.PinTTD.ThrMaxA;
                if (this.PinTTD.SelectShapeType != null)
                {
                    for (int i = 0; i < this.PinTTD.SelectShapeType.Length; i++)
                    {
                        int num3 = this.dataGridView1.Rows.Add();
                        this.dataGridView1.Rows[num3].Cells[0].Value = this.PinTTD.SelectShapeType.TupleSelect((HTuple)i).S;
                        this.dataGridView1.Rows[num3].Cells[1].Value = this.PinTTD.SelectShapeMin.TupleSelect((HTuple)i).ToString();
                        this.dataGridView1.Rows[num3].Cells[2].Value = this.PinTTD.SelectShapeMax.TupleSelect((HTuple)i).ToString();
                    }
                }


            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            this.isup = false;
        }




        private void SetValueT(int runID = 0)
        {
            try
            {
                HTuple tuple;
                if (this.isup)
                {
                    return;
                }
                this.PinTTD.XAxisName = comboBox2.SelectedItem.ToString();
                this.PinTTD.YAxisName = comboBox3.SelectedItem.ToString();
                this.PinTTD.X2AxisName = comboBox4.SelectedItem.ToString();
                this.PinTTD.Y2AxisName = comboBox5.SelectedItem.ToString();
                this.PinTTD.CloseCirt = (double)numericUpDown17.Value;
                this.PinTTD.OpeningValue = (double)numericUpDown18.Value;
                this.PinTTD.ErosionHeightValue = (double)numericUpDown16.Value;
                this.PinTTD.ErosionWaightValue = (double)numericUpDown15.Value;
                this.PinTTD.PintWrt = (byte)this.numericUpDown10.Value;
                this.PinTTD.ThrMin = (byte)this.numericUpDownThrMin.Value;
                this.PinTTD.ThrMax = (byte)this.numericUpDownThrMax.Value;
                this.PinTTD.ThrMinA = (byte)numericUpDown3.Value;
                this.PinTTD.ThrMaxA = (byte)numericUpDown12.Value;
                this.PinTTD.PinHeightExternalPercent = (double)this.numericUpDown2.Value;
                this.PinTTD.PinHeightExternal = (double)this.numericUpDown11.Value;
                this.PinTTD.ThrMinExternal = (byte)this.numericUpDown13.Value;
                this.PinTTD.ThrMaxExternal = (byte)this.numericUpDown14.Value;
                double num = (double)this.numericUpDownAreaMin.Value;
                double num2 = (double)this.numUpDownHeigthMin.Value;
                double[] numArray1 = new double[] { num, num2 };
                this.PinTTD.Min = new HTuple(numArray1);
                double[] numArray2 = new double[] { (double)this.numericUpDownAreaMax.Value, (double)this.numUpDownHeigthMax.Value };
                this.PinTTD.Max = new HTuple(numArray2);
                this.PinTTD.ColumnMM = (double)this.nudPinCol.Value;
                this.PinTTD.RowMM = (double)this.nudPinrow.Value;
                this.PinTTD.ToleranceMM = (double)this.nudPinTol3.Value;
                if (this.PinTTD.EnveType==0)
                {
                    this.checkBox1.Enabled = true;
                }
                else
                {
                    this.checkBox1.Enabled = false;
                }
                this.PinTTD.isCTX = this.checkBox1.Checked;
                if (this.PinTTD.isCTX)
                {
                    this.Height = Height - 81;
                }
                else
                {
                    Height = height;
                }
                if (!this.PinTTD.isCTX)
                {
                    goto Label_0338;
                }
                this.dataGridView2.Visible = true;
                this.nudPinrow.Enabled = false;
                if (this.dataGridView2.Rows.Count >= this.PinTTD.ColumnNumber)
                {
                    goto Label_02A1;
                }
                while (this.dataGridView2.Rows.Count < this.PinTTD.ColumnNumber)
                {
                    int num3 = this.dataGridView2.Rows.Add();
                    this.dataGridView2.Rows[num3].Cells[0].Value = this.PinTTD.RowMM;
                }
                goto Label_02C5;
            Label_027E:
                this.dataGridView2.Rows.RemoveAt(this.dataGridView2.Rows.Count - 1);
            Label_02A1:
                if (this.dataGridView2.Rows.Count > this.PinTTD.ColumnNumber)
                {
                    goto Label_027E;
                }
            Label_02C5:
                this.PinTTD.ListCTY.Clear();
                for (int i = 0; i < this.dataGridView2.Rows.Count; i++)
                {
                    this.PinTTD.ListCTY.Add(Convert.ToDouble(this.dataGridView2.Rows[i].Cells[0].Value));
                }
                goto Label_0354;
            Label_0338:
                this.nudPinrow.Enabled = true;
                this.dataGridView2.Visible = false;
            Label_0354:
                this.label4.Text = this.progressBar1.Value.ToString() + "/" + this.progressBar1.Maximum.ToString();
                HOperatorSet.GetImageType(this.halconRun.Image(null), out tuple);
                this.halconRun.ListObjCler();
                this.progressBar1.Maximum = 30;
                this.progressBar1.Value = runID;
                if (tuple == "")
                {
                    HObject obj2;
                    HObject obj3;
                    HObject obj4;
                    HOperatorSet.Decompose3(this.halconRun.Image(null), out obj2, out obj3, out obj4);
                    this.PinTTD.Run(this.halconRun, halconRun.GetOneImageR(), runID);
                }
                else
                {
                    this.PinTTD.Run(this.halconRun, halconRun.GetOneImageR(), runID);
                }
                this.halconRun.ShowObj();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private bool isup = true;





        private void btnDrawROI3_Click(object sender, EventArgs e)
        {
            try
            {
                this.halconRun.Focus();
                if (!this.halconRun.Drawing)
                {
                    this.halconRun.Drawing = true;
                    this.PinTTD.DarwRenge2(this.halconRun.Image(null), this.halconRun.hWindowHalcon());
                }
                else
                {
                    MessageBox.Show("绘制中");
                    return;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            this.halconRun.Drawing = false;
        }




        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.progressBar2.Maximum = 7 + this.PinTTD.SelectShapeType.Length;
                if (this.progressBar2.Value == this.progressBar2.Maximum)
                {
                    this.progressBar2.Value = 7;
                }
                this.progressBar2.Value++;
                this.label8.Text = this.progressBar2.Value.ToString() + "/" + this.progressBar2.Maximum.ToString();
                halconRun.HobjClear();
                this.PinTTD.Run(this.halconRun, halconRun.GetOneImageR(), this.progressBar2.Value);
                halconRun.ShowObj();
            }
            catch (Exception)
            {
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.progressBar1.Maximum = 6;
                if (this.progressBar1.Value == this.progressBar1.Maximum)
                {
                    this.progressBar1.Value = 0;
                }
                this.progressBar1.Value++;
                this.SetValueT(this.progressBar1.Value);
            }
            catch (Exception)
            {
            }
        }




        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                halconRun.AddOBJ(PinTTD.AOIObj);
                halconRun.ShowObj();
            }
            catch (Exception)
            {
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                this.halconRun.Focus();
                this.PinTTD.ShowReing(this.halconRun.hWindowHalcon());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.SetValueT(6);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isup)
                {
                    this.PinTTD.EnveType = this.comboBox1.SelectedIndex;
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3)
                {
                    halconRun.HobjClear();
                    this.PinTTD.Run(this.halconRun, halconRun.GetOneImageR(), 11 + e.RowIndex);
                    halconRun.ShowObj();
                    halconRun.ShowObj();
                }
            }
            catch (Exception)
            {
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!this.isup)
                {
                    this.PinTTD.SelectShapeType = new HTuple();
                    this.PinTTD.SelectShapeMin = new HTuple();
                    this.PinTTD.SelectShapeMax = new HTuple();
                    for (int i = 0; i < (this.dataGridView1.Rows.Count - 1); i++)
                    {
                        double num2;
                        double num3;
                        if (this.dataGridView1.Rows[i].Cells[1].Value == null || this.dataGridView1.Rows[i].Cells[2].Value == null)
                        {
                            continue;
                        }
                        if (double.TryParse(this.dataGridView1.Rows[i].Cells[1].Value.ToString(), out num2) && double.TryParse(this.dataGridView1.Rows[i].Cells[2].Value.ToString(), out num3))
                        {
                            this.PinTTD.SelectShapeMin.Append((HTuple)num2);
                            this.PinTTD.SelectShapeMax.Append((HTuple)num3);
                            this.PinTTD.SelectShapeType.Append((HTuple)this.dataGridView1.Rows[i].Cells[0].Value.ToString());
                        }
                    }
                    this.PinTTD.Run(this.halconRun, halconRun.GetOneImageR(), 10 + e.RowIndex);
                }
            }
            catch (Exception)
            {
            }
        }

        private void nudPinCol3_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isup)
                {
                    this.PinTTD.ColumnMM = (double)this.nudPinCol.Value;
                    this.SetValueT(0);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private void nudPinRow3_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isup)
                {
                    this.PinTTD.RowMM = (double)this.nudPinrow.Value;
                    this.SetValueT(0);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private void nudPinTol3_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isup)
                {

                    this.SetValueT(0);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isup)
                {
                    this.PinTTD.RowNumber = (int)this.numericUpDown6.Value;
                    this.PinTTD.ColumnNumber = (int)this.numericUpDown1.Value;
                }
            }
            catch (Exception)
            {
            }
        }
        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isup)
                {
                    this.PinTTD.ThrMin3 = (byte)this.numericUpDown9.Value;
                    this.PinTTD.ThrMax3 = 255;
                    this.PinTTD.Run(this.halconRun, halconRun.GetOneImageR(), 9);
                }
            }
            catch (Exception)
            {
            }
        }
        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            this.SetValueT(4);
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            this.SetValueT(5);
        }
        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            this.SetValueT(5);
        }


        private void numericUpDown2_ValueChanged_2(object sender, EventArgs e)
        {
            this.SetValueT(6);
        }




        private void numericUpDown4_ValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (!this.isup)
                {
                    this.PinTTD.DifferenceRow = (double)this.numericUpDown4.Value;
                    this.PinTTD.DifferenceCol = (double)this.numericUpDown5.Value;
                    this.halconRun.HobjClear();
                    this.PinTTD.Run(this.halconRun, halconRun.GetOneImageR(), 7);
                    this.halconRun.ShowObj();
                }
            }
            catch (Exception)
            {
            }
        }

        private void numericUpDown5_ValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (!this.isup)
                {
                    this.PinTTD.DifferenceRow = (double)this.numericUpDown4.Value;
                    this.PinTTD.DifferenceCol = (double)this.numericUpDown5.Value;
                    this.halconRun.HobjClear();
                    this.PinTTD.Run(this.halconRun, halconRun.GetOneImageR(), 7);
                    this.halconRun.ShowObj();

                }
            }
            catch (Exception)
            {
            }
        }
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isup)
                {
                    this.PinTTD.RowNumber = (int)this.numericUpDown6.Value;
                    this.PinTTD.ColumnNumber = (int)this.numericUpDown1.Value;
                }
            }

            catch (Exception)
            {
            }
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isup)
                {
                    this.PinTTD.ThrMin2 = (byte)this.numericUpDown7.Value;
                    this.PinTTD.ThrMax2 = (byte)this.numericUpDown8.Value;
                    halconRun.HobjClear();
                    this.PinTTD.Run(this.halconRun, halconRun.GetOneImageR(), 8);
                    halconRun.ShowObj();
                }
            }
            catch (Exception)
            {
            }
        }
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isup)
                {
                    halconRun.HobjClear();
                    this.PinTTD.ThrMin2 = (byte)this.numericUpDown7.Value;
                    this.PinTTD.ThrMax2 = (byte)this.numericUpDown8.Value;
                    this.PinTTD.Run(this.halconRun, halconRun.GetOneImageR(), 8);
                    halconRun.ShowObj();
                }
            }
            catch (Exception)
            {
            }
        }
        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isup)
                {
                    halconRun.HobjClear();
                    this.PinTTD.ThrMin3 = (byte)this.numericUpDown9.Value;
                    this.PinTTD.ThrMax3 = (byte)255;
                    this.PinTTD.Run(this.halconRun, halconRun.GetOneImageR(), 10);
                    halconRun.ShowObj();
                }
            }
            catch (Exception)
            {
            }
        }
        private void numericUpDownAreaMax_ValueChanged(object sender, EventArgs e)
        {
            this.SetValueT(2);
        }
        private void numericUpDownAreaMin_ValueChanged(object sender, EventArgs e)
        {
            this.SetValueT(2);
        }

        private void numericUpDownThrMax_ValueChanged(object sender, EventArgs e)
        {
            this.SetValueT(1);
        }
        private void numericUpDownThrMin_ValueChanged(object sender, EventArgs e)
        {
            this.SetValueT(1);

        }
        private void numUpDownHeigthMax_ValueChanged(object sender, EventArgs e)
        {
            this.SetValueT(3);
        }

        private void numUpDownHeigthMin_ValueChanged(object sender, EventArgs e)
        {
            this.SetValueT(3);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void PinUserControl1_Load(object sender, EventArgs e)
        {
        }
        private void numericUpDown3_ValueChanged_2(object sender, EventArgs e)
        {
            this.SetValueT(9);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            this.SetValueT(5);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.PinTTD.SelsMax = checkBox2.Checked;
        }





        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            SetValueT();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.isup)
            {
                return;
            }
            PinTTD.IsHtoB = checkBox3.Checked;
            if (PinTTD.IsHtoB)
            {
                numericUpDown2.Enabled = false;
                numUpDownHeigthMax.Enabled = false;
                numUpDownHeigthMin.Enabled = false;
            }
            else
            {
                numericUpDown2.Enabled = true;
                numUpDownHeigthMax.Enabled = true;
                numUpDownHeigthMin.Enabled = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                PinTTD.Rows = new HTuple();
                PinTTD.Columns = new HTuple();
                halconRun.HobjClear();
                halconRun.Focus();
                halconRun.Drawing = true;
                PointFile[] pointFile1 = new PointFile[4];
                for (int i = 0; i < 4; i++)
                {
                    HOperatorSet.DrawPoint(halconRun.hWindowHalcon(), out HTuple hTuple, out HTuple colt);
                    HOperatorSet.GenCrossContourXld(out HObject hObjectT, hTuple, colt, 70, 0);
                    HOperatorSet.DispObj(hObjectT, halconRun.hWindowHalcon());
                    Vision2.vision.Vision.Disp_message(halconRun.hWindowHalcon(), (i + 1), hTuple, colt);
                    //HOperatorSet.DispText(halconRun.hWindowHalconID, (i + 1), "image", hTuple, colt, "re", "box", "true");
                    pointFile1[i] = new PointFile();
                    pointFile1[i].Y = colt;
                    pointFile1[i].X = hTuple;
                }
                ErosSocket.DebugPLC.Robot.TrayRobot.Calculate((sbyte)PinTTD.RowNumber, (sbyte)PinTTD.ColumnNumber, pointFile1[0], pointFile1[1], pointFile1[2], pointFile1[3], out pointFile1);
                for (int i = 0; i < pointFile1.Length; i++)
                {
                    if (PinTTD.Rows == null)
                    {
                        PinTTD.Rows = new HTuple();
                        PinTTD.Columns = new HTuple();
                    }
                    PinTTD.Rows.Append(pointFile1[i].X);
                    PinTTD.Columns.Append(pointFile1[i].Y);
                }
                HOperatorSet.GenCrossContourXld(out HObject hObject, PinTTD.Rows, PinTTD.Columns, 120, 0);
                HOperatorSet.GenRectangle2(out HObject hObject1, PinTTD.Rows, PinTTD.Columns, HTuple.TupleGenConst(PinTTD.Columns.Length, 0), HTuple.TupleGenConst(PinTTD.Columns.Length, PinTTD.PinHeightExternal), HTuple.TupleGenConst(PinTTD.Columns.Length, PinTTD.PinHeightExternal));
                halconRun.AddOBJ(hObject);
                halconRun.AddOBJ(hObject1, ColorResult.blue);
                halconRun.ShowObj();

            }
            catch (Exception EX)
            {


            }
            halconRun.Drawing = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {

                halconRun.HobjClear();
                HOperatorSet.GenCrossContourXld(out HObject hObject, PinTTD.Rows, PinTTD.Columns, 120, 0);
                HOperatorSet.GenRectangle2(out HObject hObject1, PinTTD.Rows, PinTTD.Columns, HTuple.TupleGenConst(PinTTD.Columns.Length, 0), HTuple.TupleGenConst(PinTTD.Columns.Length, PinTTD.PinHeightExternal), HTuple.TupleGenConst(PinTTD.Columns.Length, PinTTD.PinHeightExternal));
                halconRun.AddOBJ(hObject);
                halconRun.AddOBJ(hObject1, ColorResult.blue);
                halconRun.ShowObj();
            }
            catch (Exception)
            {
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

            PinTTD.Rows = new HTuple();
            PinTTD.Columns = new HTuple();
        }

        private void numericUpDown10_ValueChanged_1(object sender, EventArgs e)
        {
            SetValueT(4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (PinTTD.EnveType==1)
                {
                    PinTTD.MRows = PinTTD.ModeRow;
                    PinTTD.MColumns = PinTTD.ModeColumn;
                    if (PinTTD.MColumns.Length > 0)
                    {
                        HOperatorSet.GenRectangle2(out HObject hObjectTE, PinTTD.MRows, PinTTD.MColumns,
                            HTuple.TupleGenConst(PinTTD.MColumns.Length, new HTuple(0).TupleRad()), HTuple.TupleGenConst(PinTTD.MColumns.Length, halconRun.GetCaliConstPx(PinTTD.RowMM) / 4
                         ), HTuple.TupleGenConst(PinTTD.MColumns.Length, halconRun.GetCaliConstPx(PinTTD.ColumnMM) / 4));
                        halconRun.AddOBJ(hObjectTE);
                        halconRun.ShowObj();
                    }
                }
                else  if(PinTTD.EnveType == 2)
                {
                    PinTTD.MRows = PinTTD.ModeRow;
                    PinTTD.MColumns = PinTTD.ModeColumn;
                    dataGridView3.Rows.Clear();
                    isup = true;
                    for (int i = 0; i < this.PinTTD.MRows.Length; i++)
                    {
                        int det = dataGridView3.Rows.Add();
                        dataGridView3.Rows[det].Cells[0].Value = this.PinTTD.MColumns.TupleSelect(i);
                        dataGridView3.Rows[det].Cells[1].Value = this.PinTTD.MRows.TupleSelect(i);
                    }
                }
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isup = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (PinTTD.MColumns.Length > 0)
                {
                    HOperatorSet.GenRectangle2(out HObject hObjectTE, PinTTD.MRows, PinTTD.MColumns,
                       HTuple.TupleGenConst(PinTTD.MColumns.Length, new HTuple(0).TupleRad()), HTuple.TupleGenConst(PinTTD.MColumns.Length, halconRun.GetCaliConstPx(PinTTD.RowMM) / 4
                    ), HTuple.TupleGenConst(PinTTD.MColumns.Length, halconRun.GetCaliConstPx(PinTTD.ColumnMM) / 4));
                    halconRun.AddOBJ(hObjectTE);
                    halconRun.ShowObj();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {

                PinTTD.PintOBj = RunProgram.DrawHObj(halconRun, PinTTD.PintOBj);
            }
            catch (Exception)
            {

            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                PinTTD.PintOBj = RunProgram.DrawRmoveObj(halconRun, PinTTD.PintOBj);
            }
            catch (Exception)
            {

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            PinTTD.AOIObj = RunProgram.DrawHObj(halconRun, PinTTD.AOIObj);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                PinTTD.AOIObj = RunProgram.DrawRmoveObj(halconRun, PinTTD.AOIObj);
            }
            catch (Exception)
            {

            }
        }

        private void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            SetValueT(5);
        }

        private void 添加测量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string name = "测量";
                foreach (var item in PinTTD.Dic_Measure.Keys_Measure)
                {
                    name = item.Key;
                    break;
                }
            st:
                string nameStr = "";
                if (PinTTD.Dic_Measure.Keys_Measure.ContainsKey(name))
                {

                    int idx = Vision2.ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name, out name);
                    name = name + (idx + 1);
                    goto st;
                }
                if (listBox1.SelectedItem != null)
                {
                    Vision2.vision.HalconRunFile.RunProgramFile.Measure measureM = PinTTD.Dic_Measure.Add(listBox1.SelectedItem.ToString());
                    if (measureM != null)
                    {
                        nameStr = measureM.Name;
                    }
                }
                else
                {
                    nameStr = Interaction.InputBox("请输入产品名", "新建产品", name, 100, 100);
                    if (nameStr == "")
                    {
                        return;
                    }
                    PinTTD.Dic_Measure.Add(nameStr);
                }

                if (nameStr.Length == 0)
                {
                    return;
                }
                listBox1.Items.Add(nameStr);
                comboBox2.Items.Add(nameStr);
                comboBox3.Items.Add(nameStr);
                comboBox4.Items.Add(nameStr);
                comboBox5.Items.Add(nameStr);
            }
            catch (Exception)
            { }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (PinTTD.Dic_Measure.Keys_Measure.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    PinTTD.Dic_Measure.Keys_Measure.Remove(listBox1.SelectedItem.ToString());
                }
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                comboBox2.Items.RemoveAt(listBox1.SelectedIndex);
                comboBox3.Items.RemoveAt(listBox1.SelectedIndex);
                comboBox4.Items.RemoveAt(listBox1.SelectedIndex);
                comboBox5.Items.RemoveAt(listBox1.SelectedIndex);
            }
            catch (Exception)
            {
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                halconRun.HobjClear();
                if (listBox1.SelectedItem != null)
                {
                    //Measure.Dic_Measure[listBox1.SelectedItem.ToString()].HomName = Measure.HomName;
                    measureConTrolEx1.Updata(PinTTD.Dic_Measure[listBox1.SelectedItem.ToString()], halconRun);
                    halconRun.AddOBJ(PinTTD.Dic_Measure[listBox1.SelectedItem.ToString()].GetHamMatDraw());
                    halconRun.ShowObj();
                }

            }
            catch (Exception)
            {

            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetValueT();
        }
        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.isup)
            {
                return;
            }
            try
            {
                if (e.ColumnIndex==0)
                {
                    this.PinTTD.MColumns[e.RowIndex] = double.Parse(dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                else
                {
                    this.PinTTD.MRows[e.RowIndex] = double.Parse(dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}