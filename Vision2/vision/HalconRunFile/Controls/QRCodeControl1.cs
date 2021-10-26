using ErosSocket.DebugPLC;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.RunProgramFile.Color_Detection;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class QRCodeControl1 : UserControl
    {
        private HWindID HWindID = new HWindID();
        private HWindID HWindIDTS = new HWindID();

        public QRCodeControl1(QRCode qRCode, HalconRun halc)
        {
            isCheave = true;
            InitializeComponent();
            hWindID.Initialize(hWindowControl3);
            _Classify = qRCode.color_Classify;
            Get_Pragram(_Classify);
            try
            {
                HWindIDTS.Initialize(hWindowControl2);
                runProgram = qRCode;
                propertyGrid2.SelectedObject = runProgram;
                halcon = halc;
                comboBox1.SelectedIndex = 0;
                comboBox1.SelectedItem = runProgram.SymbolType;
                numericUpDown15.Value = runProgram.TrayIDNumber;
                comboBox3.SelectedIndex = runProgram.MatrixType;
                numericUpDown17.Value = runProgram.ThraQR;
                propertyGrid1.SelectedObject = runProgram.One_QR;
                checkBox2.Checked = runProgram.Is2D;
                comboBox2.SelectedIndex = runProgram.DiscernType;
                HWindID.Initialize(hWindowControl1);
                HWindID.SetImaage(halcon.Image());
                numericUpDown1.Value = runProgram.Emphasizefactor;
                numericUpDown2.Value = runProgram.EmphasizeH;
                numericUpDown3.Value = runProgram.EmphasizeW;
                checkBox4.Checked = runProgram.IsImage_range;
                checkBox3.Checked = runProgram.IsEmphasize;
                trackBar1.Value = (int)numericUpDown1.Value;
                trackBar2.Value = (int)numericUpDown2.Value;
                trackBar3.Value = (int)numericUpDown3.Value;
                numericUpDown10.Value = runProgram.ISCont;
                numericUpDown14.Value = runProgram.Height;
                numericUpDown18.Value = runProgram.Weight;
                numericUpDown16.Value = runProgram.TrayNumber;
                if (runProgram.TrayIDS.Count < runProgram.TrayNumber)
                {
                    runProgram.TrayIDS.AddRange(new int[runProgram.TrayNumber - runProgram.TrayIDS.Count]);
                }
                else if (runProgram.TrayIDS.Count > runProgram.TrayNumber)
                {
                    runProgram.TrayIDS.RemoveRange(runProgram.TrayNumber, runProgram.TrayIDS.Count - runProgram.TrayNumber);
                }
                dataGridView2.Rows.Clear();
                if (runProgram.IDValue > 0)
                {
                    if (dataGridView2.Rows.Count < runProgram.Rows.Length)
                    {
                        dataGridView2.Rows.Add(runProgram.Rows.Length - dataGridView2.Rows.Count);
                    }
                    for (int i = 0; i < runProgram.Rows.Length; i++)
                    {
                        if (runProgram.TrayIDS.Count > i)
                        {
                            dataGridView2.Rows[i].Cells[1].Value = (runProgram.TrayIDS[i]);
                        }

                        if (runProgram.IsEt.Count > i)
                        {
                            dataGridView2.Rows[i].Cells[2].Value = (runProgram.IsEt[i]);
                        }
                        if (runProgram.Rows.Length > i)
                        {
                            dataGridView2.Rows[i].Cells[3].Value = runProgram.Rows.TupleSelect(i);
                        }
                        if (runProgram.Cols.Length > i)
                        {
                            dataGridView2.Rows[i].Cells[4].Value = runProgram.Cols.TupleSelect(i);
                        }
                    }
                }
                numericUpDown13.Value = runProgram.IDValue;
                ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView2);
                trackBar4.Value = runProgram.SeleImageRangeMax;
                trackBar5.Value = runProgram.SeleImageRangeMin;
                checkBox1.Checked = runProgram.IsMedian_image;
                checkBox7.Checked = runProgram.IsOpen_image;
                numericUpDown20.Value = (decimal)runProgram.Sub_Mult;
                numericUpDown21.Value = (decimal)runProgram.Sub_Add;
                numericUpDown19.Value = (decimal)runProgram.Median_imageVa;
                numericUpDown12.Value = trackBar4.Value;
                numericUpDown11.Value = trackBar5.Value;
                GetP();
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.StackTrace);
            }
            isCheave = false;
        }

        private bool isCheave = true;

        private QRCode runProgram;
        private HalconRun halcon;
        Color_Detection.Color_classify _Classify;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            runProgram.SymbolType = comboBox1.SelectedItem.ToString();
            runProgram.CreateDataCode2dModel(halcon);
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                halcon.HobjClear();
                halcon.ShowVision(runProgram.Name, halcon.GetOneImageR());
                HWindID.HobjClear();
                HWindID.OneResIamge.HObjectRed = runProgram.XLD;
                HWindID.ShowImage();
                //dataGridView2.Rows.Clear();
                if (dataGridView2.Rows.Count < runProgram.IDValue)
                {
                    dataGridView2.Rows.Add(runProgram.IDValue - dataGridView2.Rows.Count);
                }
                if (runProgram.TrayIDS.Count == 0)
                {
                    runProgram.TrayIDS.AddRange(new int[runProgram.QRText.Count()]);
                }
                for (int i = 0; i < runProgram.QRText.Count; i++)
                {
                    dataGridView2.Rows[i].Cells[0].Value = (runProgram.QRText[i]);
                    if (runProgram.TrayIDS.Count > i)
                    {
                        dataGridView2.Rows[i].Cells[1].Value = (runProgram.TrayIDS[i]);
                    }
                    if (runProgram.IsEt.Count > i)
                    {
                        dataGridView2.Rows[i].Cells[2].Value = (runProgram.IsEt[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //halcon.Image(Vision.GenImageInterleaved(BarcodeHelper.Generate1(textBox1.Text.ToString(), 1000, 1000)));
                //halcon.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成失败:" + ex.Message);
            }
        }

        private void QRCodeControl1_Load(object sender, EventArgs e)
        {
            try
            {
                comboBox1.SelectedIndex = 0;
                foreach (var item in runProgram.KeyHObject.DirectoryHObject)
                {
                    listBox1.Items.Add(item.Key);
                }
                comboBox1.SelectedItem = runProgram.SymbolType;
                GetP();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isCheave = false;
        }

        public void GetP()
        {
            try
            {
                isCheave = true;
                checkBox5.Checked = runProgram.Enble;

                dataGridView1.Rows.Clear();
                if (runProgram.MarkName.Count != 0)
                {
                    dataGridView1.Rows.Add(runProgram.MarkName.Count);
                    for (int i = 0; i < runProgram.MarkName.Count; i++)
                    {
                        dataGridView1.Rows[i].Cells[0].Value = runProgram.MarkName[i];
                    }
                }
                comboBox4.SelectedIndex = runProgram.QRCOntEn;
                numericUpDown4.Value = runProgram.XNumber;
                numericUpDown5.Value = runProgram.YNumber;
                numericUpDown6.Value = runProgram.XInterval;
                numericUpDown7.Value = runProgram.YInterval;
                numericUpDown8.Value = runProgram.XLocation;
                numericUpDown9.Value = runProgram.YLocation;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null && listBox1.SelectedItem.ToString() != "")
                {
                    HOperatorSet.DispObj(runProgram.KeyHObject[listBox1.SelectedItem.ToString()], halcon.hWindowHalcon());
                }
            }
            catch (Exception)
            {
            }
        }

        private void 创建识别区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int name = 1;

                while (true)
                {
                    HObject hObject = new HObject();

                    if (runProgram.KeyHObject.DirectoryHObject.ContainsKey(name.ToString()))
                    {
                        name++;
                    }
                    else
                    {
                        listBox1.Items.Add(name);
                        hObject = halcon.Draw_Region();
                        runProgram.KeyHObject.DirectoryHObject.Add(name.ToString(), hObject);
                        break;
                    }
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
                if (listBox1.SelectedItem != null)
                {
                    runProgram.KeyHObject.Remove(listBox1.SelectedItem.ToString());
                    listBox1.Items.Remove(listBox1.SelectedItem);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="halcon"></param>
        /// <returns></returns>
        private HObject DrawMod(HalconRun halcon, HObject hObject)
        {
            HObject rectangle2 = null;
            HTuple row, column, phi, length1, length2;

            HOperatorSet.SmallestRectangle2(hObject, out HTuple row1, out HTuple column1, out HTuple phi1, out HTuple length11, out HTuple length22);
            if (row1.TupleType() == 15)
            {
                HOperatorSet.DrawRectangle2(halcon.hWindowHalcon(), out row, out column, out phi, out length1, out length2);
            }
            else
            {
                HOperatorSet.DrawRectangle2Mod(halcon.hWindowHalcon(), row1, column1, phi1, length11, length22,
                    out row, out column, out phi, out length1, out length2);
            }
            //if (button == 1)
            //{
            //    HOperatorSet.GenRectangle2(out rectangle2, row, column, phi, length1, length2);
            //}
            HOperatorSet.GenRectangle2(out rectangle2, row, column, phi, length1, length2);
            return rectangle2;
        }

        private void 修改区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HObject hObject = new HObject();
            try
            {
                if (runProgram.KeyHObject.DirectoryHObject.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    hObject = runProgram.KeyHObject[listBox1.SelectedItem.ToString()];
                    if (halcon.DrawMoeIng(DrawMod, ref hObject))
                    {
                        runProgram.KeyHObject[listBox1.SelectedItem.ToString()] = hObject;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                runProgram.Watch.Restart();
                HObject image = runProgram.GetEmset(halcon.Image());
                runProgram.Watch.Stop();
                HWindID.SetImaage(image);
                HWindID.OneResIamge.AddMeassge("运行时间" + runProgram.Watch.ElapsedMilliseconds);
                HWindID.ShowImage();
            }
            catch (Exception)
            {
            }
        }

        public List<HObject> images = new List<HObject>();

        public List<HObject> OBJs = new List<HObject>();

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (isCheave)
            {
                return;
            }
            runProgram.Is2D = checkBox2.Checked;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (isCheave)
                {
                    return;
                }
                runProgram.IsEmphasize = checkBox3.Checked;
                runProgram.IsImage_range = checkBox4.Checked;
                runProgram.IsMedian_image = checkBox1.Checked;
                runProgram.IsOpen_image = checkBox7.Checked;
                runProgram.Median_imageVa = (double)numericUpDown19.Value;
                runProgram.Sub_Mult = (double)numericUpDown20.Value;
                runProgram.Sub_Add = (double)numericUpDown21.Value;
                numericUpDown12.Value = trackBar4.Value;
                numericUpDown11.Value = trackBar5.Value;
                runProgram.SeleImageRangeMax = (byte)numericUpDown12.Value;
                runProgram.SeleImageRangeMin = (byte)numericUpDown11.Value;
                numericUpDown1.Value = trackBar1.Value;
                numericUpDown2.Value = trackBar2.Value;
                numericUpDown3.Value = trackBar3.Value;
                runProgram.Emphasizefactor = (byte)numericUpDown1.Value;
                runProgram.EmphasizeH = (byte)numericUpDown2.Value;
                runProgram.EmphasizeW = (byte)numericUpDown3.Value;
                HObject image = runProgram.GetEmset(halcon.Image());
                HWindID.OneResIamge.Image = image;
                HWindID.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            try
            {
                runProgram.Rows = new HTuple();
                runProgram.Cols = new HTuple();
                runProgram.XNumber = (int)numericUpDown4.Value;
                runProgram.YNumber = (int)numericUpDown5.Value;

                halcon.HobjClear();
                SetP(); PointFile[] pointFile1 = new PointFile[4];
                for (int i = 0; i < 4; i++)
                {
                    pointFile1[i] = new PointFile();
                    pointFile1[i].Y = runProgram.XCols[i];
                    pointFile1[i].X = runProgram.YRows[i];
                }
                halcon.GetOneImageR().AddImageMassage(runProgram.YRows, runProgram.XCols, new HTuple("1", "2", "3", "4"), ColorResult.blue, "true");
                HOperatorSet.GenCrossContourXld(out HObject hObject, runProgram.YRows, runProgram.XCols, 70, 0);
                halcon.AddObj(hObject);
                numericUpDown8.Value = runProgram.YRows.TupleSelect(0).TupleInt();
                numericUpDown9.Value = runProgram.XCols.TupleSelect(0).TupleInt();
                ErosSocket.DebugPLC.Robot.TrayRobot.Calculate((sbyte)runProgram.XNumber, (sbyte)runProgram.YNumber, pointFile1[0], pointFile1[1], pointFile1[2], pointFile1[3], out pointFile1);
                SetP();
                for (int i = 0; i < runProgram.XNumber * runProgram.YNumber; i++)
                {
                    //if (!listBox1.Items.Contains(i + 1))
                    //{
                    //    listBox1.Items.Add(i + 1);
                    //}
                    HOperatorSet.GenRectangle1(out hObject, pointFile1[i].X - runProgram.Height, pointFile1[i].Y - runProgram.Weight, pointFile1[i].X + runProgram.Height, pointFile1[i].Y + runProgram.Weight);
                    halcon.AddObj(hObject);
                    runProgram.Rows.Append(pointFile1[i].X);
                    runProgram.Cols.Append(pointFile1[i].Y);
                }

                halcon.ShowObj();
            }
            catch (Exception)
            {
            }
        }

        public void SetP()
        {
            try
            {
                runProgram.XNumber = (int)numericUpDown4.Value;
                runProgram.YNumber = (int)numericUpDown5.Value;
                runProgram.XInterval = (int)numericUpDown6.Value;
                runProgram.YInterval = (int)numericUpDown7.Value;
                runProgram.XLocation = (int)numericUpDown8.Value;
                runProgram.YLocation = (int)numericUpDown9.Value;
            }
            catch (Exception)
            {
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            halcon.Drawing = true;
            try
            {
                halcon.HobjClear();
                runProgram.YRows = new HTuple();
                runProgram.XCols = new HTuple();
                runProgram.IsEt.Clear();
                PointFile[] pointFile1 = new PointFile[4];
                for (int i = 0; i < 4; i++)
                {
                    HOperatorSet.DrawPoint(halcon.hWindowHalcon(), out HTuple hTuple, out HTuple colt);
                    HOperatorSet.GenCrossContourXld(out HObject hObjectT, hTuple, colt, 70, 0);
                    HOperatorSet.DispObj(hObjectT, halcon.hWindowHalcon());
                    HOperatorSet.DispText(halcon.hWindowHalcon(), (i + 1), "image", hTuple, colt, "red", "box", "true");
                    runProgram.YRows.Append(hTuple);
                    runProgram.XCols.Append(colt);
                    pointFile1[i] = new PointFile();
                    pointFile1[i].Y = colt;
                    pointFile1[i].X = hTuple;
                }
                HOperatorSet.GenCrossContourXld(out HObject hObject, runProgram.YRows, runProgram.XCols, 70, 0);
                halcon.AddObj(hObject);
                numericUpDown8.Value = runProgram.YRows.TupleSelect(0).TupleInt();
                numericUpDown9.Value = runProgram.XCols.TupleSelect(0).TupleInt();
                ErosSocket.DebugPLC.Robot.TrayRobot.Calculate((sbyte)runProgram.XNumber, (sbyte)runProgram.YNumber, pointFile1[0], pointFile1[1], pointFile1[2], pointFile1[3], out pointFile1);
                SetP();
                for (int i = 0; i < runProgram.XNumber * runProgram.YNumber; i++)
                {
                    HOperatorSet.GenRectangle1(out hObject, pointFile1[i].X - runProgram.Height, pointFile1[i].Y - runProgram.Weight, pointFile1[i].X + runProgram.Height, pointFile1[i].Y + runProgram.Weight);
                    halcon.AddObj(hObject);
                    if (runProgram.Rows == null)
                    {
                        runProgram.Rows = new HTuple();
                        runProgram.Cols = new HTuple();
                    }
                    runProgram.IsEt.Add(true);
                    runProgram.Rows.Append(pointFile1[i].X);
                    runProgram.Cols.Append(pointFile1[i].Y);
                }
                halcon.ShowObj();
            }
            catch (Exception)
            {
            }
            halcon.Drawing = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void 删除全部ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            if (isCheave)
            {
                return;
            }
            runProgram.Height = (int)numericUpDown14.Value;
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isCheave)
                {
                    return;
                }
                string dante = ((Control)sender).Text;
                string DAET = ((Control)sender).Name;
                runProgram.ISCont = (int)numericUpDown10.Value;
                runProgram.TrayNumber = (int)numericUpDown16.Value;
                runProgram.IDValue = (int)numericUpDown13.Value;
                runProgram.TrayIDNumber = (int)numericUpDown15.Value;
                if (runProgram.TrayIDS.Count < runProgram.TrayNumber)
                {
                    runProgram.TrayIDS.AddRange(new int[runProgram.TrayNumber - runProgram.TrayIDS.Count]);
                }
                else if (runProgram.TrayIDS.Count > runProgram.TrayNumber)
                {
                    runProgram.TrayIDS.RemoveRange(runProgram.TrayNumber, runProgram.TrayIDS.Count - runProgram.TrayNumber);
                }
                if (runProgram.TrayNumber > 0)
                {
                    if (dataGridView2.Rows.Count < runProgram.IDValue)
                    {
                        dataGridView2.Rows.Add(runProgram.IDValue - dataGridView2.Rows.Count);
                    }
                    for (int i = 0; i < runProgram.TrayNumber; i++)
                    {
                        dataGridView2.Rows[i].Cells[1].Value = (runProgram.TrayIDS[i]);
                        if (runProgram.IsEt.Count > i)
                        {
                            dataGridView2.Rows[i].Cells[2].Value = (runProgram.IsEt[i]);
                        }
                        if (runProgram.Rows.Length > i)
                        {
                            dataGridView2.Rows[i].Cells[3].Value = runProgram.Rows.TupleSelect(i);
                        }
                        if (runProgram.Cols.Length > i)
                        {
                            dataGridView2.Rows[i].Cells[4].Value = runProgram.Cols.TupleSelect(i);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isCheave)
                {
                    return;
                }
                if (e.ColumnIndex == 1 && e.RowIndex >= 0)
                {
                    runProgram.TrayIDS[e.RowIndex] = int.Parse(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString());
                }
                else if (e.ColumnIndex == 2 && e.RowIndex >= 0)
                {
                    runProgram.IsEt[e.RowIndex] = bool.Parse(dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString());
                }
                else if (e.ColumnIndex == 3 && e.RowIndex >= 0)
                {
                    runProgram.Rows[e.RowIndex] = double.Parse(dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString());
                }
                else if (e.ColumnIndex == 4 && e.RowIndex >= 0)
                {
                    runProgram.Cols[e.RowIndex] = double.Parse(dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString());
                }
            }
            catch (Exception)
            {
            }
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
        }

        private void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            if (isCheave)
            {
                return;
            }
            runProgram.ThraQR = (int)numericUpDown17.Value;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isCheave)
            {
                return;
            }
            halcon.HobjClear();
            runProgram.MatrixType = (int)comboBox3.SelectedIndex;
            if (runProgram.DiscernType == 1)
            {
                runProgram.SrotCode(halcon.GetOneImageR());
                halcon.ShowObj();
            }
        }

        private void dataGridView2_CurrentCellChanged(object sender, EventArgs e)
        {
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (isCheave)
            {
                return;
            }
            SetP();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (isCheave)
            {
                return;
            }
            SetP();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();

                this.Cursor = Cursors.WaitCursor;
                string name = runProgram.GenParamName;
                listBox2.Items.Clear();
                images.Clear();
                HWindIDTS.HobjClear();
                OBJs.Clear();
                runProgram.GenParamName = "train";
                runProgram.TrainQRCode(runProgram.GetEmset(halcon.Image()), halcon.GetOneImageR(), out HObject hObject1, images, OBJs);
                for (int i = 0; i < images.Count; i++)
                {
                    listBox2.Items.Add(i + 1);
                }
                listBox2.SelectedIndex = 0;
                runProgram.GenParamName = name;
                halcon.GetOneImageR().AddMeassge(runProgram.DecodedDataString.ToString());
                halcon.ShowObj();
            }
            catch (Exception)
            {
            }
            this.Cursor = Cursors.Default;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox2.SelectedIndex < 0)
                {
                    return;
                }
                HWindIDTS.SetImaage(images[listBox2.SelectedIndex]);
                HWindIDTS.OneResIamge.AddObj(OBJs[listBox2.SelectedIndex]);
                HWindIDTS.ShowImage();
            }
            catch (Exception)
            {
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.GenRectangle1(out HObject hObject3, runProgram.Rows - runProgram.Height, runProgram.Cols - runProgram.Weight, runProgram.Rows + runProgram.Height, runProgram.Cols + runProgram.Weight);
                HObject hObject = RunProgramFile.RunProgram.DragMoveOBJS(halcon, hObject3);
                HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple rows, out HTuple colus);
                runProgram.Rows = rows;
                runProgram.Cols = colus;
                HTuple id = new HTuple();
                for (int i = 0; i < rows.Length; i++)
                {
                    if (runProgram.TrayIDS.Count <= i)
                    {
                        runProgram.TrayIDS.Add(runProgram.TrayIDS[i - 1] + 1);
                    }
                    id.Append(runProgram.TrayIDS[i]);
                }
                halcon.HobjClear();
                halcon.AddImageMassage(rows + 80, colus, id);
                halcon.ShowObj();
                //Code.SrotCode(halcon.GetOneImageR());
            }
            catch (Exception)
            {
            }
        }

        private void 插入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                runProgram.Rows = runProgram.Rows.TupleInsert(dataGridView2.SelectedCells[0].RowIndex + 1, runProgram.Rows[dataGridView2.SelectedCells[0].RowIndex] + 60);
                runProgram.Cols = runProgram.Cols.TupleInsert(dataGridView2.SelectedCells[0].RowIndex + 1, runProgram.Cols[dataGridView2.SelectedCells[0].RowIndex] + 60);
                int det = dataGridView2.SelectedCells[0].RowIndex + 2;
                runProgram.IsEt.Insert(dataGridView2.SelectedCells[0].RowIndex + 1, true);
                dataGridView2.Rows.Insert(dataGridView2.SelectedCells[0].RowIndex + 1, "", dataGridView2.SelectedCells[0].RowIndex + 2, true,
                    runProgram.Rows[dataGridView2.SelectedCells[0].RowIndex] + 60, runProgram.Cols[dataGridView2.SelectedCells[0].RowIndex] + 60);
                for (int i = det; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Cells[1].Value != null)
                    {
                        if (int.TryParse(dataGridView2.Rows[i].Cells[1].Value.ToString(), out int trayNubmer))
                        {
                            dataGridView2.Rows[i].Cells[1].Value = trayNubmer + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                runProgram.Rows.TupleRemove(dataGridView2.SelectedCells[0].RowIndex);
                runProgram.Cols.TupleRemove(dataGridView2.SelectedCells[0].RowIndex);
                runProgram.IsEt.RemoveAt(dataGridView2.SelectedCells[0].RowIndex);
                dataGridView2.Rows.RemoveAt(dataGridView2.SelectedCells[0].RowIndex);
            }
            catch (Exception)
            {
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.GenRectangle1(out HObject hObject3, runProgram.Rows - runProgram.Height, runProgram.Cols - runProgram.Weight, runProgram.Rows + runProgram.Height, runProgram.Cols + runProgram.Weight);
                HOperatorSet.Union1(hObject3, out hObject3);
                HObject hObject = RunProgramFile.RunProgram.DragMoveOBJ(halcon, hObject3);
                HOperatorSet.Connection(hObject, out hObject);
                HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple rows, out HTuple colus);
                runProgram.Rows = rows;
                runProgram.Cols = colus;
                halcon.HobjClear();
                halcon.AddImageMassage(rows + 80, colus, new HTuple(runProgram.TrayIDS.ToArray()));
                halcon.ShowObj();
                //Code.SrotCode(halcon.GetOneImageR());
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (toolStripButton2.Text == "开始实时识别")
            {
                toolStripButton2.Text = "停止";
                //halcon.StratThread();
            }
            else
            {
                toolStripButton2.Text = "开始实时识别";
                halcon.Stop();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                runProgram.SrotCode(halcon.GetOneImageR());
                dataGridView2.Rows.Clear();
                if (runProgram.IDValue > 0)
                {
                    if (dataGridView2.Rows.Count < runProgram.Rows.Length)
                    {
                        dataGridView2.Rows.Add(runProgram.Rows.Length - dataGridView2.Rows.Count);
                    }
                    for (int i = 0; i < runProgram.Rows.Length; i++)
                    {
                        if (runProgram.TrayIDS.Count > i)
                        {
                            dataGridView2.Rows[i].Cells[1].Value = (runProgram.TrayIDS[i]);
                        }

                        if (runProgram.IsEt.Count > i)
                        {
                            dataGridView2.Rows[i].Cells[2].Value = (runProgram.IsEt[i]);
                        }
                        if (runProgram.Rows.Length > i)
                        {
                            dataGridView2.Rows[i].Cells[3].Value = runProgram.Rows.TupleSelect(i);
                        }
                        if (runProgram.Cols.Length > i)
                        {
                            dataGridView2.Rows[i].Cells[4].Value = runProgram.Cols.TupleSelect(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            runProgram.DiscernType = comboBox2.SelectedIndex;
        }

        private void comboBox4_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (isCheave)
            {
                return;
            }
            runProgram.QRCOntEn = (int)comboBox4.SelectedIndex;
        }

        private void 托盘编号叠加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedCells.Count > 2)
                {
                    if (int.TryParse(dataGridView2.Rows[dataGridView2.SelectedCells[dataGridView2.SelectedCells.Count - 1].RowIndex].Cells[1].Value.ToString(), out int strIndtd))
                    {
                        for (int i = dataGridView2.SelectedCells.Count - 1; i >= 0; i--)
                        {
                            dataGridView2.Rows[dataGridView2.SelectedCells[i].RowIndex].Cells[1].Value = strIndtd++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 托盘编号递减ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedCells.Count > 2)
                {
                    if (int.TryParse(dataGridView2.Rows[dataGridView2.SelectedCells[dataGridView2.SelectedCells.Count - 1].RowIndex].Cells[1].Value.ToString(), out int strIndtd))
                    {
                        for (int i = dataGridView2.SelectedCells.Count - 1; i >= 0; i--)
                        {
                            dataGridView2.Rows[dataGridView2.SelectedCells[i].RowIndex].Cells[1].Value = strIndtd--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                runProgram.MarkName = new List<string>();
                runProgram.MarkName.Add("颜色检测.颜色");
                runProgram.MarkName.Add("颜色检测.颜色1");
                runProgram.MarkName.Add("颜色检测.颜色2");
                runProgram.MarkName.Add("颜色检测.颜色3");
                runProgram.MarkName.Add("颜色检测.颜色4");
                runProgram.GetPThis().HobjClear();
                for (int id = 0; id < runProgram.MarkName.Count; id++)
                {
                    string[] prn = runProgram.MarkName[id].Split('.');
                    Color_Detection color_Detection = runProgram.GetPThis().GetRunProgram()[prn[0]] as Color_Detection;
                    if (color_Detection != null)
                    {
                        HOperatorSet.GenCrossContourXld(out HObject hObject, color_Detection.keyColor[prn[1]].OBJRow,
                            color_Detection.keyColor[prn[1]].OBJCol, 60, 0);
                        HOperatorSet.GenRegionLine(out HObject hObject1, color_Detection.keyColor[prn[1]].OBJRow[0],
                            color_Detection.keyColor[prn[1]].OBJCol[0], color_Detection.keyColor[prn[1]].OBJRow[1],
                            color_Detection.keyColor[prn[1]].OBJCol[1]);
                        runProgram.GetPThis().AddObj(hObject1);
                        runProgram.GetPThis().AddObj(hObject);
                        HOperatorSet.DistancePl(runProgram.OutRow, runProgram.OutCol, color_Detection.keyColor[prn[1]].OBJRow[0],
                        color_Detection.keyColor[prn[1]].OBJCol[0], color_Detection.keyColor[prn[1]].OBJRow[1],
                        color_Detection.keyColor[prn[1]].OBJCol[1], out HTuple ding);
                        HTuple row = new HTuple();
                        HTuple col = new HTuple();
                        HTuple intd = new HTuple();
                        HTuple dingpp = new HTuple();
                        for (int i = 0; i < ding.Length; i++)
                        {
                            if (ding[i] < 250)
                            {
                                row.Append(runProgram.OutRow[i]);
                                col.Append(runProgram.OutCol[i]);
                                HTuple hTupleR = HTuple.TupleGenConst(runProgram.Cols.Length, runProgram.OutRow[i]);
                                HTuple hTupleC = HTuple.TupleGenConst(runProgram.Cols.Length, runProgram.OutCol[i]);
                                HOperatorSet.DistancePp(runProgram.Rows, runProgram.Cols, hTupleR, hTupleC, out HTuple dipp);
                                HTuple intex = dipp.TupleFind(dipp.TupleMin());

                                intd.Append((intex + 1) + ":" + (ding[i] - 160));
                                dingpp.Append(ding[i]);
                            }
                        }
                        HOperatorSet.GenCrossContourXld(out HObject hObject2, row, col, 60, 0);
                        runProgram.GetPThis().AddImageMassage(row, col, intd);
                        runProgram.GetPThis().AddObj(hObject2);
                    }
                    runProgram.GetPThis().ShowObj();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void numericUpDown18_ValueChanged(object sender, EventArgs e)
        {
            if (isCheave)
            {
                return;
            }
            runProgram.Weight = (int)numericUpDown18.Value;
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                
                  _Classify.DrawObj = RunProgram.DrawHObj(halcon, _Classify.DrawObj);
                
                if (_Classify.DrawObj == null || _Classify.DrawObj.IsInitialized())
                {
                    halcon.AddObj(_Classify.DrawObj, ColorResult.pink);
                }
            }
            catch (Exception)
            {
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();

                _Classify.DrawObj = RunProgram.DrawRmoveObj(halcon, _Classify.DrawObj);

                if (_Classify.DrawObj == null || _Classify.DrawObj.IsInitialized())
                {
                    halcon.AddObj(_Classify.DrawObj, ColorResult.pink);
                }
            }
            catch (Exception)
            {
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {

                _Classify.DrawObj =
                 RunProgram.DragMoveOBJ(halcon, _Classify.DrawObj);
            }
            catch (Exception ex)
            {
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Set_Pragram(_Classify);
            }
            catch (Exception)
            {
            }
        }
        public void Get_Pragram(Color_classify color_Classify)
        {
            isMove = true;
            try
            {
                _Classify = color_Classify;
                if (color_Classify.H_enabled)
                {
                    color_Classify.H_enabled = false;
                    color_Classify.threshold_Min_Maxes.Add(new Threshold_Min_Max()
                    {
                        ImageTypeObj = ImageTypeObj.H,
                        Min = color_Classify.Threshold_H.Min,
                        Max = color_Classify.Threshold_H.Max,
                    });
                }
                if (color_Classify.V_enabled)
                {
                    color_Classify.V_enabled = false;
                    color_Classify.threshold_Min_Maxes.Add(new Threshold_Min_Max()
                    {
                        ImageTypeObj = ImageTypeObj.V,
                        Min = color_Classify.Threshold_V.Min,
                        Max = color_Classify.Threshold_V.Max,
                    }); ;
                }
                if (color_Classify.S_enabled)
                {
                    color_Classify.S_enabled = false;
                    color_Classify.threshold_Min_Maxes.Add(new Threshold_Min_Max()
                    {
                        ImageTypeObj = ImageTypeObj.S,
                        Min = color_Classify.Threshold_S.Min,
                        Max = color_Classify.Threshold_S.Max,
                    }); ;
                }
                checkBoxEnble.Checked = _Classify.Enble;
                checkBoxCorss.Checked = _Classify.IsColt;
                thresholdControl1.SetData(color_Classify.threshold_Min_Maxes);
                select_obj_type1.SetData(color_Classify.Max_area);
                numericUpDownNuber.Value = color_Classify.ColorNumber;
                listBox3.Items.Add("并集");
                for (int i = 0; i < _Classify.threshold_Min_Maxes.Count; i++)
                {
                    listBox3.Items.Add(_Classify.threshold_Min_Maxes[i].ImageTypeObj);
                }
                //numericUpDown1.Value = color_Classify.Color_ID;
                //comboBox1.SelectedItem = color_Classify.ImageType.ToString();
                //checkBox3.Checked = color_Classify.EnbleSelect;
                //numericUpDown4.Value = color_Classify.ThresSelectMin;
                //numericUpDown5.Value = color_Classify.ThresSelectMax;
                //numericUpDown7.Value = (decimal)color_Classify.SelectMax;
                //numericUpDown8.Value = (decimal)color_Classify.SelectMin;
                //numericUpDown6.Value = (decimal)color_Classify.ClosingCir;
                checkBoxRoing.Checked = color_Classify.ISFillUp;
                checkBoxFilu.Checked = color_Classify.ISSelecRoiFillUP;
                numericUpDownClosingCircleValue.Value = (decimal)color_Classify.ClosingCircleValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isMove = false;
        }
        private bool isMove = false;
        private List<HObject> hObjects = new List<HObject>();
        public void Set_Pragram(Color_classify color_Classify)
        {
            if (isMove)
            {
                return;
            }
            try
            {
                _Classify = color_Classify;
                halcon.HobjClear();
                //_Classify.ImageType = (ImageTypeObj)Enum.Parse(typeof(ImageTypeObj), comboBox1.SelectedItem.ToString());
                //_Classify.EnbleSelect = checkBoxEnble.Checked;
                _Classify.IsColt = checkBoxCorss.Checked;
                _Classify.ISSelecRoiFillUP = checkBoxFilu.Checked;
                _Classify.Enble = checkBoxEnble.Checked;
                //_Classify.ThresSelectMin = (byte)numericUpDown4.Value;
                //_Classify.ThresSelectMax = (byte)numericUpDown5.Value;
                //_Classify.SelectMin = (double)numericUpDown8.Value;
                //_Classify.SelectMax = (double)numericUpDown7.Value;
                //_Classify.ClosingCir = (double)numericUpDown6.Value;
                _Classify.ColorNumber = (byte)numericUpDownNuber.Value;
                //_Classify.Color_ID = (byte)numericUpDown1.Value;
                //_Classify.COlorES = button3.BackColor;
                _Classify.ISFillUp = checkBoxRoing.Checked;
                _Classify.ClosingCircleValue = (double)numericUpDownClosingCircleValue.Value;
                AoiObj aoiObj = new AoiObj();

                aoiObj.SelseAoi = _Classify.DrawObj;

                aoiObj.CiName = _Classify.Name;
                _Classify.Classify(halcon.GetOneImageR(), aoiObj, runProgram, out HObject hObject, hObjects);
                halcon.AddObj(hObject);
                halcon.ShowImage();
                halcon.ShowObj();
            }
            catch (Exception ex) { }
        }

        private void numericUpDown24_ValueChanged(object sender, EventArgs e)
        {
            Set_Pragram(_Classify);
        }

        private void numericUpDown23_ValueChanged(object sender, EventArgs e)
        {
            Set_Pragram(_Classify);
        }
        private HWindID hWindID = new HWindID();
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hWindID.HobjClear();
           
                HOperatorSet.SmallestRectangle1(_Classify.DrawObj, out HTuple row, out HTuple col1, out HTuple row2, out HTuple col2);
                if (listBox3.SelectedIndex == 0)
                {
                    hWindID.SetImaage(halcon.Image());
                }
                else
                {
                    hWindID.SetImaage(halcon.GetImageOBJ((ImageTypeObj)Enum.Parse(typeof(ImageTypeObj), listBox3.SelectedItem.ToString())));
                }
                AoiObj aoiObj = runProgram.GetAoi();
                aoiObj.SelseAoi = _Classify.DrawObj;
                _Classify.Classify(halcon.GetOneImageR(), aoiObj, runProgram, out HObject hObject,
         this.hObjects);
                if (_Classify.IsHomMat)
                {
                    List<HTuple> listHomet = runProgram.GetHomMatList(halcon.GetOneImageR());
                    HOperatorSet.AffineTransRegion(_Classify.DrawObj, out HObject hObjectROI,
                    listHomet[0], "nearest_neighbor");
                    aoiObj.SelseAoi = hObjectROI;
                }
                else
                {
                    aoiObj.SelseAoi = _Classify.DrawObj;
                }
                groupBox3.Text = listBox3.SelectedItem.ToString();
                hWindID.SetPerpetualPart(row - 100, col1 - 100, row2 + 100, col2 + 100);
                hWindID.SetDraw(checkBoxFilu.Checked);
                hWindID.OneResIamge.AddObj(_Classify.DrawObj, ColorResult.blue);
                hWindID.OneResIamge.AddObj(hObjects[listBox3.SelectedIndex]);
                hWindID.ShowObj();

                //HImage hImage = new HImage(hWindID.Image());
                //userCtrlThreshold1.Fuction(hImage);
            }
            catch (Exception ex)
            {
            }
        }
    }
}