using ErosSocket.DebugPLC;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.RunProgramFile.RunProgram;
using static Vision2.vision.Vision;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class QRCodeControl1 : UserControl
    {
        HWindID HWindID = new HWindID();
        HWindID HWindIDTS = new HWindID();
        public QRCodeControl1(QRCode qRCode, HalconRun halc)
        {
            isCheave = true;
            InitializeComponent();
            try
            {
                HWindIDTS.Initialize(hWindowControl2);
                Code = qRCode;
                propertyGrid2.SelectedObject = Code;
                halcon = halc;
                comboBox1.SelectedIndex = 0;
                comboBox1.SelectedItem = Code.SymbolType;
                numericUpDown15.Value = Code.TrayIDNumber;
                comboBox3.SelectedIndex = Code.MatrixType;
                numericUpDown17.Value = Code.ThraQR;
                propertyGrid1.SelectedObject = Code.One_QR;
                checkBox2.Checked = Code.Is2D;
                comboBox2.SelectedIndex = Code.DiscernType;
            
                HWindID.Initialize(hWindowControl1);
                HWindID.SetImaage(halcon.Image());
                numericUpDown1.Value = Code.Emphasizefactor;
                numericUpDown2.Value = Code.EmphasizeH;
                numericUpDown3.Value = Code.EmphasizeW;
                checkBox4.Checked = Code.IsImage_range;
                checkBox3.Checked = Code.IsEmphasize;
                trackBar1.Value = (int)numericUpDown1.Value;
                trackBar2.Value = (int)numericUpDown2.Value;
                trackBar3.Value = (int)numericUpDown3.Value;
                numericUpDown10.Value = Code.ISCont;
                numericUpDown14.Value = Code.Height;
                numericUpDown16.Value = Code.TrayNumber;
                if (Code.TrayIDS.Count < Code.TrayNumber)
                {
                    Code.TrayIDS.AddRange(new int[Code.TrayNumber - Code.TrayIDS.Count]);
                }
                else if (Code.TrayIDS.Count > Code.TrayNumber)
                {
                    Code.TrayIDS.RemoveRange(Code.TrayNumber, Code.TrayIDS.Count - Code.TrayNumber);
                }
                dataGridView2.Rows.Clear();
                if (Code.IDValue > 0)
                {
                    if (dataGridView2.Rows.Count < Code.Rows.Length)
                    {
                        dataGridView2.Rows.Add(Code.Rows.Length - dataGridView2.Rows.Count);
                    }
                    for (int i = 0; i < Code.Rows.Length; i++)
                    {
                        if (Code.TrayIDS.Count>i)
                        {
                            dataGridView2.Rows[i].Cells[1].Value = (Code.TrayIDS[i]);
                        }
         
                        if (Code.IsEt.Count>i)
                        {
                            dataGridView2.Rows[i].Cells[2].Value = (Code.IsEt[i]);
                        }
                        if (Code.Rows.Length > i)
                        {
                            dataGridView2.Rows[i].Cells[3].Value = Code.Rows.TupleSelect(i);
                        }
                        if (Code.Cols.Length > i)
                        {
                            dataGridView2.Rows[i].Cells[4].Value = Code.Cols.TupleSelect(i);
                        }
                    }
                }
                numericUpDown13.Value = Code.IDValue;
                ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView2);
                trackBar4.Value = Code.SeleImageRangeMax;
                trackBar5.Value = Code.SeleImageRangeMin;
                checkBox1.Checked = Code.IsMedian_image;
                checkBox7.Checked = Code.IsOpen_image;
                numericUpDown20.Value = (decimal)Code.Sub_Mult;
                numericUpDown21.Value = (decimal)Code.Sub_Add;
                numericUpDown19.Value =(decimal) Code.Median_imageVa;
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
        bool isCheave = true;

        private QRCode Code;
        private HalconRun halcon;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Code.SymbolType = comboBox1.SelectedItem.ToString();
            Code.CreateDataCode2dModel(halcon);
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                halcon.HobjClear();
                halcon.ShowVision(Code.Name, halcon.GetOneImageR());
                HWindID.HobjClear();
                HWindID.OneResIamge.HObjectRed = Code.XLD;
                HWindID.ShowImage();
                //dataGridView2.Rows.Clear();
                if (dataGridView2.Rows.Count< Code.IDValue)
                {
                    dataGridView2.Rows.Add(Code.IDValue - dataGridView2.Rows.Count);
                }
                if (Code.TrayIDS.Count == 0)
                {
                    Code.TrayIDS.AddRange(new int[Code.QRText.Count()]);
                }
                for (int i = 0; i < Code.QRText.Count; i++)
                {
                    dataGridView2.Rows[i].Cells[0].Value = (Code.QRText[i]);
                    if (Code.TrayIDS.Count > i)
                    {
                        dataGridView2.Rows[i].Cells[1].Value = (Code.TrayIDS[i]);
                    }
                    if (Code.IsEt.Count > i)
                    {
                        dataGridView2.Rows[i].Cells[2].Value = (Code.IsEt[i]);
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
                foreach (var item in Code.KeyHObject.DirectoryHObject)
                {
                    listBox1.Items.Add(item.Key);
                }
                comboBox1.SelectedItem = Code.SymbolType;
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
                checkBox5.Checked = Code.Enble;

                dataGridView1.Rows.Clear();
                if (Code.MarkName.Count!=0)
                {
                    dataGridView1.Rows.Add(Code.MarkName.Count);
                    for (int i = 0; i < Code.MarkName.Count; i++)
                    {
                        dataGridView1.Rows[i].Cells[0].Value = Code.MarkName[i];
                    }
                }
                comboBox4.SelectedIndex = Code.QRCOntEn;
                numericUpDown4.Value = Code.XNumber;
                numericUpDown5.Value = Code.YNumber;
                numericUpDown6.Value = Code.XInterval;
                numericUpDown7.Value = Code.YInterval;
                numericUpDown8.Value = Code.XLocation;
                numericUpDown9.Value = Code.YLocation;

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
                    HOperatorSet.DispObj(Code.KeyHObject[listBox1.SelectedItem.ToString()], halcon.hWindowHalcon());

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

                    if (Code.KeyHObject.DirectoryHObject.ContainsKey(name.ToString()))
                    {
                        name++;
                    }
                    else
                    {
                        listBox1.Items.Add(name);
                        hObject = halcon.Draw_Region();
                        Code.KeyHObject.DirectoryHObject.Add(name.ToString(), hObject);
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
                    Code.KeyHObject.Remove(listBox1.SelectedItem.ToString());
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
                if (Code.KeyHObject.DirectoryHObject.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    hObject = Code.KeyHObject[listBox1.SelectedItem.ToString()];
                    if (halcon.DrawMoeIng(DrawMod, ref hObject))
                    {
                        Code.KeyHObject[listBox1.SelectedItem.ToString()] = hObject;
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
                Code.Watch.Restart();
                HObject image = Code.GetEmset(halcon.Image());
                Code.Watch.Stop();
                HWindID.SetImaage(image);
                HWindID.OneResIamge.AddMeassge("运行时间" + Code.Watch.ElapsedMilliseconds);
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
            Code.Is2D = checkBox2.Checked;

        }



        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (isCheave)
                {
                    return;
                }
                Code.IsEmphasize = checkBox3.Checked;
                Code.IsImage_range = checkBox4.Checked;
                Code.IsMedian_image = checkBox1.Checked;
                Code.IsOpen_image = checkBox7.Checked;
                Code.Median_imageVa = (double)numericUpDown19.Value;
                Code.Sub_Mult = (double)numericUpDown20.Value;
                Code.Sub_Add = (double)numericUpDown21.Value;
                numericUpDown12.Value = trackBar4.Value;
                numericUpDown11.Value = trackBar5.Value;
                Code.SeleImageRangeMax = (byte)numericUpDown12.Value;
                Code.SeleImageRangeMin = (byte)numericUpDown11.Value;
                numericUpDown1.Value = trackBar1.Value;
                numericUpDown2.Value = trackBar2.Value;
                numericUpDown3.Value = trackBar3.Value;
                Code.Emphasizefactor = (byte)numericUpDown1.Value;
                Code.EmphasizeH = (byte)numericUpDown2.Value;
                Code.EmphasizeW = (byte)numericUpDown3.Value;
                HObject image = Code.GetEmset(halcon.Image());
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
                Code.Rows = new HTuple();
                Code.Cols = new HTuple();
                Code.XNumber = (int)numericUpDown4.Value;
                Code.YNumber = (int)numericUpDown5.Value;

                halcon.HobjClear();
                SetP(); PointFile[] pointFile1 = new PointFile[4];
                for (int i = 0; i < 4; i++)
                {
                    pointFile1[i] = new PointFile();
                    pointFile1[i].Y = Code.XCols[i];
                    pointFile1[i].X = Code.YRows[i];
                }
                halcon.GetOneImageR().AddImageMassage(Code.YRows, Code.XCols, new HTuple("1", "2", "3", "4"), ColorResult.blue, "true");
                HOperatorSet.GenCrossContourXld(out HObject hObject, Code.YRows, Code.XCols, 70, 0);
                halcon.AddObj(hObject);
                numericUpDown8.Value = Code.YRows.TupleSelect(0).TupleInt();
                numericUpDown9.Value = Code.XCols.TupleSelect(0).TupleInt();
                ErosSocket.DebugPLC.Robot.TrayRobot.Calculate((sbyte)Code.XNumber, (sbyte)Code.YNumber, pointFile1[0], pointFile1[1], pointFile1[2], pointFile1[3], out pointFile1);
                SetP();
                for (int i = 0; i < Code.XNumber * Code.YNumber; i++)
                {
                    //if (!listBox1.Items.Contains(i + 1))
                    //{
                    //    listBox1.Items.Add(i + 1);
                    //}
                    HOperatorSet.GenRectangle1(out hObject, pointFile1[i].X - Code.Height, pointFile1[i].Y - Code.Height, pointFile1[i].X + Code.Height, pointFile1[i].Y + Code.Height);
                    halcon.AddObj(hObject);
                    Code.Rows.Append(pointFile1[i].X);
                    Code.Cols.Append(pointFile1[i].Y);
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
                Code.XNumber = (int)numericUpDown4.Value;
                Code.YNumber = (int)numericUpDown5.Value;
                Code.XInterval = (int)numericUpDown6.Value;
                Code.YInterval = (int)numericUpDown7.Value;
                Code.XLocation = (int)numericUpDown8.Value;
                Code.YLocation = (int)numericUpDown9.Value;
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
                Code.YRows = new HTuple();
                Code.XCols = new HTuple();
                Code.IsEt.Clear();
                PointFile[] pointFile1 = new PointFile[4];
                for (int i = 0; i < 4; i++)
                {
                    HOperatorSet.DrawPoint(halcon.hWindowHalcon(), out HTuple hTuple, out HTuple colt);
                    HOperatorSet.GenCrossContourXld(out HObject hObjectT, hTuple, colt, 70, 0);
                    HOperatorSet.DispObj(hObjectT, halcon.hWindowHalcon());
                    HOperatorSet.DispText(halcon.hWindowHalcon(), (i + 1), "image", hTuple, colt, "red", "box", "true");
                    Code.YRows.Append(hTuple);
                    Code.XCols.Append(colt);
                    pointFile1[i] = new PointFile();
                    pointFile1[i].Y = colt;
                    pointFile1[i].X = hTuple;
                }
                HOperatorSet.GenCrossContourXld(out HObject hObject, Code.YRows, Code.XCols, 70, 0);
                halcon.AddObj(hObject);
                numericUpDown8.Value = Code.YRows.TupleSelect(0).TupleInt();
                numericUpDown9.Value = Code.XCols.TupleSelect(0).TupleInt();
                ErosSocket.DebugPLC.Robot.TrayRobot.Calculate((sbyte)Code.XNumber, (sbyte)Code.YNumber, pointFile1[0], pointFile1[1], pointFile1[2], pointFile1[3], out pointFile1);
                SetP();
                for (int i = 0; i < Code.XNumber * Code.YNumber; i++)
                {

                    HOperatorSet.GenRectangle1(out hObject, pointFile1[i].X - Code.Height, pointFile1[i].Y - Code.Height, pointFile1[i].X + Code.Height, pointFile1[i].Y + Code.Height);
                    halcon.AddObj(hObject);
                    if (Code.Rows == null)
                    {
                        Code.Rows = new HTuple();
                        Code.Cols = new HTuple();
                    }
                    Code.IsEt.Add(true);
                    Code.Rows.Append(pointFile1[i].X);
                    Code.Cols.Append(pointFile1[i].Y);
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
            Code.Height = (int)numericUpDown14.Value;
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isCheave)
                {
                    return;
                }
                string dante=      ((Control)sender).Text;
                string DAET = ((Control)sender).Name;
                Code.ISCont = (int)numericUpDown10.Value;
                Code.TrayNumber = (int)numericUpDown16.Value;
                Code.IDValue = (int)numericUpDown13.Value;
                Code.TrayIDNumber = (int)numericUpDown15.Value;
                if (Code.TrayIDS.Count < Code.TrayNumber)
                {
                    Code.TrayIDS.AddRange(new int[Code.TrayNumber - Code.TrayIDS.Count]);
                }
                else if (Code.TrayIDS.Count > Code.TrayNumber)
                {
                    Code.TrayIDS.RemoveRange(Code.TrayNumber, Code.TrayIDS.Count - Code.TrayNumber);
                }
                if (Code.TrayNumber > 0)
                {
                    if (dataGridView2.Rows.Count < Code.IDValue)
                    {
                        dataGridView2.Rows.Add(Code.IDValue - dataGridView2.Rows.Count);
                    }
                    for (int i = 0; i < Code.TrayNumber; i++)
                    {
                        dataGridView2.Rows[i].Cells[1].Value = (Code.TrayIDS[i]);
                        if (Code.IsEt.Count > i)
                        {
                            dataGridView2.Rows[i].Cells[2].Value = (Code.IsEt[i]);
                        }
                        if (Code.Rows.Length > i)
                        {
                            dataGridView2.Rows[i].Cells[3].Value = Code.Rows.TupleSelect(i);
                        }
                        if (Code.Cols.Length > i)
                        {
                            dataGridView2.Rows[i].Cells[4].Value = Code.Cols.TupleSelect(i);
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
                    Code.TrayIDS[e.RowIndex] = int.Parse(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString());
                }
                else if (e.ColumnIndex == 2 && e.RowIndex >= 0)
                {
                    Code.IsEt[e.RowIndex] = bool.Parse(dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString());
                }
                else if (e.ColumnIndex == 3 && e.RowIndex >= 0)
                {
                    Code.Rows[e.RowIndex] = double.Parse(dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString());
                }
                else if (e.ColumnIndex == 4 && e.RowIndex >= 0)
                {
                    Code.Cols[e.RowIndex] = double.Parse(dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString());
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
            Code.ThraQR = (int)numericUpDown17.Value;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isCheave)
            {
                return;
            }
            halcon.HobjClear();
            Code.MatrixType = (int)comboBox3.SelectedIndex;
            if (Code.DiscernType==1)
            {
                Code.SrotCode(halcon.GetOneImageR());
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
                string name = Code.GenParamName;
                listBox2.Items.Clear();
                images.Clear();
                HWindIDTS.HobjClear();
                OBJs.Clear();
                Code.GenParamName = "train";
                Code.TrainQRCode(Code.GetEmset(halcon.Image()), halcon.GetOneImageR(), out HObject hObject1, images, OBJs);
                for (int i = 0; i < images.Count; i++)
                {
                    listBox2.Items.Add(i+1);
                }
                listBox2.SelectedIndex = 0;
                Code.GenParamName = name;
                halcon.GetOneImageR().AddMeassge(Code.DecodedDataString.ToString());
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
                if (listBox2.SelectedIndex<0)
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
                HOperatorSet.GenRectangle1(out HObject hObject3, Code.Rows - Code.Height, Code.Cols - Code.Height, Code.Rows + Code.Height, Code.Cols + Code.Height);
                HObject hObject = RunProgram.DragMoveOBJS(halcon, hObject3);
                HOperatorSet.AreaCenter(hObject,  out HTuple area, out HTuple rows, out HTuple colus);
                Code.Rows = rows;
                Code.Cols = colus;
                HTuple id = new HTuple();
                for (int i = 0; i < rows.Length; i++)
                {
                    if (Code.TrayIDS.Count<=i)
                    {
                        Code.TrayIDS.Add(Code.TrayIDS[i-1]+1);
                    }
                    id.Append(Code.TrayIDS[i]);
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
                Code.Rows= Code.Rows.TupleInsert(dataGridView2.SelectedCells[0].RowIndex+1, Code.Rows[dataGridView2.SelectedCells[0].RowIndex] + 60);
                Code.Cols = Code.Cols.TupleInsert(dataGridView2.SelectedCells[0].RowIndex+1, Code.Cols[dataGridView2.SelectedCells[0].RowIndex] + 60);
                int det = dataGridView2.SelectedCells[0].RowIndex + 2;
                Code.IsEt.Insert(dataGridView2.SelectedCells[0].RowIndex + 1,true);
                dataGridView2.Rows.Insert(dataGridView2.SelectedCells[0].RowIndex+1,"", dataGridView2.SelectedCells[0].RowIndex+2,true,
                    Code.Rows[dataGridView2.SelectedCells[0].RowIndex]+60, Code.Cols[dataGridView2.SelectedCells[0].RowIndex] + 60);
                for (int i = det; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Cells[1].Value!=null)
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
                Code.Rows.TupleRemove(dataGridView2.SelectedCells[0].RowIndex );
                Code.Cols.TupleRemove(dataGridView2.SelectedCells[0].RowIndex );
                Code.IsEt.RemoveAt(dataGridView2.SelectedCells[0].RowIndex );
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
                HOperatorSet.GenRectangle1(out HObject hObject3, Code.Rows - Code.Height, Code.Cols - Code.Height, Code.Rows + Code.Height, Code.Cols + Code.Height);
                HOperatorSet.Union1(hObject3, out hObject3);
                HObject hObject = RunProgram.DragMoveOBJ(halcon, hObject3);
                HOperatorSet.Connection(hObject, out hObject);
                HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple rows, out HTuple colus);
                Code.Rows = rows;
                Code.Cols = colus;
                halcon.HobjClear();
                halcon.AddImageMassage(rows + 80, colus, new HTuple(Code.TrayIDS.ToArray()));
                halcon.ShowObj();
                //Code.SrotCode(halcon.GetOneImageR());
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (toolStripButton2.Text== "开始实时识别")
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
                Code.SrotCode(halcon.GetOneImageR());
                dataGridView2.Rows.Clear();
                if (Code.IDValue > 0)
                {
                    if (dataGridView2.Rows.Count < Code.Rows.Length)
                    {
                        dataGridView2.Rows.Add(Code.Rows.Length - dataGridView2.Rows.Count);
                    }
                    for (int i = 0; i < Code.Rows.Length; i++)
                    {
                        if (Code.TrayIDS.Count > i)
                        {
                            dataGridView2.Rows[i].Cells[1].Value = (Code.TrayIDS[i]);
                        }

                        if (Code.IsEt.Count > i)
                        {
                            dataGridView2.Rows[i].Cells[2].Value = (Code.IsEt[i]);
                        }
                        if (Code.Rows.Length > i)
                        {
                            dataGridView2.Rows[i].Cells[3].Value = Code.Rows.TupleSelect(i);
                        }
                        if (Code.Cols.Length > i)
                        {
                            dataGridView2.Rows[i].Cells[4].Value = Code.Cols.TupleSelect(i);
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
            Code.DiscernType = comboBox2.SelectedIndex;
        }
        private void comboBox4_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (isCheave)
            {
                return;
            }
            Code.QRCOntEn = (int)comboBox4.SelectedIndex;
        }

        private void 托盘编号叠加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedCells.Count>2)
                {

                    if (int.TryParse(dataGridView2.Rows[dataGridView2.SelectedCells[dataGridView2.SelectedCells.Count-1].RowIndex].Cells[1].Value.ToString(),out int strIndtd))
                    {
                        for (int i = dataGridView2.SelectedCells.Count-1; i >= 0; i--)
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
       
        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                Code.MarkName = new List<string>();
                Code.MarkName.Add("颜色检测.颜色");
                Code.MarkName.Add("颜色检测.颜色1");
                Code.MarkName.Add("颜色检测.颜色2");
                Code.MarkName.Add("颜色检测.颜色3");
                Code.MarkName.Add("颜色检测.颜色4");
                Code.GetPThis().HobjClear();
                for (int id = 0; id < Code.MarkName.Count; id++)
                {
                    string[] prn = Code.MarkName[id].Split('.');
                    Color_Detection color_Detection = Code.GetPThis().GetRunProgram()[prn[0]] as Color_Detection;
                    if (color_Detection != null)
                    {
                        HOperatorSet.GenCrossContourXld(out HObject hObject, color_Detection.keyColor[prn[1]].OBJRow,
                            color_Detection.keyColor[prn[1]].OBJCol, 60, 0);
                        HOperatorSet.GenRegionLine(out HObject hObject1, color_Detection.keyColor[prn[1]].OBJRow[0],
                            color_Detection.keyColor[prn[1]].OBJCol[0], color_Detection.keyColor[prn[1]].OBJRow[1],
                            color_Detection.keyColor[prn[1]].OBJCol[1]);
                        Code.GetPThis().AddObj(hObject1);
                        Code.GetPThis().AddObj(hObject);
                        HOperatorSet.DistancePl(Code.OutRow, Code.OutCol, color_Detection.keyColor[prn[1]].OBJRow[0],
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
                                row.Append(Code.OutRow[i]);
                                col.Append(Code.OutCol[i]);
                                HTuple hTupleR = HTuple.TupleGenConst(Code.Cols.Length, Code.OutRow[i]);
                                HTuple hTupleC = HTuple.TupleGenConst(Code.Cols.Length, Code.OutCol[i]);
                                HOperatorSet.DistancePp(Code.Rows, Code.Cols, hTupleR, hTupleC, out HTuple dipp);
                                HTuple intex = dipp.TupleFind(dipp.TupleMin());

                                intd.Append((intex + 1) + ":" +( ding[i]-160));
                                dingpp.Append(ding[i]);
                            }
                        }
                        HOperatorSet.GenCrossContourXld(out HObject hObject2, row, col, 60, 0);
                        Code.GetPThis().AddImageMassage(row, col, intd);
                        Code.GetPThis().AddObj(hObject2);
                    }
                    Code.GetPThis().ShowObj();

                }

            }
            catch (Exception ex)
            {
            }
        }
    }
}