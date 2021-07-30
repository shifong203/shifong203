using HalconDotNet;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.RunProgramFile.Color_Detection;
using static Vision2.vision.HalconRunFile.RunProgramFile.RunProgram;
using static Vision2.vision.Vision;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class ModelControl : UserControl
    {

  
        public ModelControl()
        {
            InitializeComponent();
        }
        public DrawContrlos DrawCont;

        public ModelControl(ModelVision model) : this()
        {
            _Model = model;
            halcon = model.GetPThis() as HalconRun;
            propertyGrid2 .SelectedObject= model;
            Updata(model);
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(Enum.GetNames(typeof(ImageTypeObj)));
            hWindID.Initialize(hWindowControl2);
        }

        public ModelVision _Model;
        public HalconRun halcon;
        public Color_classify _Classify;
        public HWindID hWindID = new HWindID();
        HWindID HWi1 = new HWindID();
        List<HObject> hObjects = new List<HObject>();

        private void btnFindShapeModel_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.UPStart();
                halcon.ShowVision(_Model.Name, halcon.GetOneImageR());
                halcon.EndChanged(halcon.GetOneImageR());
                numOrX.Text = _Model.OriginX.TupleString(".3f");
                numOrY.Text = _Model.OriginY.TupleString(".3f");
                numOrU.Text = _Model.OriginU.TupleDeg().TupleString("0.03f");
            }
            catch (Exception ex)
            {
                MessageBox.Show("定位失败！");
            }
        }

        private void btnCreateShapeModel_Click(object sender, EventArgs e)
        {
            try
            {
             
                this.Cursor =Cursors.WaitCursor;
                modify();
                if (comboBox3.SelectedItem == null)
                {
                    MessageBox.Show("请选择创建模式！");
                    return;
                }
                if (comboBox3.SelectedItem.ToString() == "图片区域")
                {
                    _Model.Create_Shape_Model(halcon);
                }
                else if (comboBox3.SelectedItem.ToString() == "XLD区域")
                {
                }
                else
                {
                    HObject hObject = _Model.Create_ModelRegr;
                    if (_Model.DrawObj.IsInitialized())
                    {
                        HOperatorSet.Difference(hObject, _Model.DrawObj, out hObject);
                    }
                    HOperatorSet.ReduceDomain(halcon.Image(), hObject, out HObject hObject2);

                    HOperatorSet.ThresholdSubPix(hObject2, out hObject2, trackBar1.Value);
                    HOperatorSet.SelectContoursXld(hObject2, out hObject2, "contour_length", trackBar2.Value, 999999, -0.5, 0.5);
                    halcon.AddObj(hObject2);
                    _Model.Create_Scaled_Shap_Model_Xld(halcon, hObject2);
                }
                numOrX.Text = _Model.OriginX.TupleString(".2f");
                numOrY.Text = _Model.OriginY.TupleString(".2f");
                numOrU.Text = _Model.OriginU.TupleDeg().TupleString(".2f");
                //modelID.Text = _Model.ID.ToString();
                HOperatorSet.ReduceDomain(_Model.GetEmset(halcon.Image()), _Model.Create_ModelRegr, out HObject hObject1);
                HWi1.SetImaage(hObject1);

            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            halcon.Drawing = false;
        }
        bool isChe = true;
        void modify()
        {
            try
            {
                if (isChe)
                {
                    return;
                }
                _Model.ContNumber = (int)numericUpDown1.Value;
                if (cbbMode.SelectedItem == null)
                {
                    MessageBox.Show("请选择仿射模式");
                    return;
                }
                _Model.Mode = cbbMode.SelectedItem.ToString();
                _Model.NumberI = Convert.ToInt16(modelNumber.Value);
                _Model.ScoreD = Convert.ToDouble(ModelSeat.Value);
                _Model.MinScaelD = Convert.ToDouble(minSael.Value);
                _Model.MaxScaelD = Convert.ToDouble(maxSael.Value);
                _Model.AngleStartDeg = Convert.ToDouble(AngleStart.Value);
                _Model.AngleExtentDeg = Convert.ToDouble(AnlgeExtent.Value);
                _Model.GreedinessD = Convert.ToDouble(Greediness.Value);
                _Model.MaxOverlapD = Convert.ToDouble(MaxOverlap.Value);
                _Model.OriginX = Convert.ToDouble(numOrX.Text);
                _Model.OriginY = Convert.ToDouble(numOrY.Text);
                _Model.OriginU = new HTuple(Convert.ToDouble(numOrU.Text)).TupleRad();
                _Model.COlorES = button2.BackColor;
                _Model.SubPixel = comboBox4.SelectedItem.ToString();
            }
            catch (Exception)
            {
            }
        }
        private void modifyModel_Click(object sender, EventArgs e)
        {
            modify();
        }

        private void btnSaveModel_Click(object sender, EventArgs e)
        {
            //保存模板
            try
            {
                halcon.SaveImage(Application.StartupPath + "\\Project\\" + ProjectINI.In.ProjectName + "\\" + ProjectINI.In.RunName + "\\Vision\\" + Project.formula.Product.ProductionName + "\\Halcon\\" + halcon.Name + "\\" + _Model.Name + "\\模板图片");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void raedModel_Click(object sender, EventArgs e)
        {
            try
            {
                string ds = System.IO.Path.GetDirectoryName(ProjectINI.In.ProjectPathRun);
                _Model.UpSatrt<ModelVision>(ds + "\\" + _Model.Name);
                halcon.ShowImage();
                halcon.AddObj(_Model.OrignXLD);
                Updata(_Model);
                halcon.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //}
        }

        //设置模板背景色
        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //如果单击“确认键"，showDialog方法返回DialogResult.OK
                button2.BackColor = colorDialog1.Color;//设置label1的背景颜色
                modify();
            }
        }

        //添加中心
        private void button44_Click(object sender, EventArgs e)
        {
            //halcon.ShowVision(_Model.Name);
            //HOperatorSet.DrawPointMod(halcon.hWindowHalcon(), halcon.MRModelHomMat.Row, halcon.MRModelHomMat.Col, out HTuple hTupler, out HTuple hTupleC);
            //HOperatorSet.GenCrossContourXld(out HObject hObject, hTupler, hTupleC, 20, 0);
            //halcon.AddObj(hObject);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.AddObj(_Model.OrignXLD.Clone());
                halcon.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.AddObj(_Model.ROIModeXLD.Clone());
                halcon.ShowObj();
            }
            catch (Exception)
            {
            }
        }

        public void Updata(ModelVision model)
        {
            try
            {
                isChe = true;
                _Model = model;
                checkBox2.Checked = _Model.Variation_Model.Enbler;
                checkBox5.Checked = _Model.Variation_Model.IsHomed;
                listBox2.Items.Clear();
                foreach (var item in _Model.ColorDic)
                {
                    listBox2.Items.Add(item.Key);
                }

                comboBox3.SelectedIndex = 1;
                textBox1.Text=   _Model.Variation_Model.Name;
                switch (_Model.Variation_Model.CreateModeMode)
                {
                    case "direct":
                        comboBox1.SelectedIndex = 0;
                        break;
                    case "standard":
                        comboBox1.SelectedIndex = 1;
                        break;
                    case "robust":
                        comboBox1.SelectedIndex = 2;
                        break;
                    default:
                        break;
                }
                comboBox3.SelectedItem = "图片区域";
                if (comboBox3.SelectedItem.ToString() != "二值化区域")
                {
                    trackBar1.Enabled = false;
                    trackBar2.Enabled = false;
                }
                numericUpDown1.Value = _Model.ContNumber;
                checkBox1.Checked = _Model.IsDip;
                dataGridView1.Enabled = _Model.IsDip;
                for (int i = 0; i < _Model.DIPs.Count; i++)
                {
                    int d = dataGridView1.Rows.Add();
                    dataGridView1.Rows[d].Cells[0].Value = _Model.DIPs[i].Name;
                    dataGridView1.Rows[d].Cells[1].Value = _Model.DIPs[i].Row;
                    dataGridView1.Rows[d].Cells[2].Value = _Model.DIPs[i].Col;
                    dataGridView1.Rows[d].Cells[3].Value = _Model.DIPs[i].Angle;
                    dataGridView1.Rows[d].Cells[4].Value = _Model.DIPs[i].DistanceMax;
                    dataGridView1.Rows[d].Cells[5].Value = _Model.DIPs[i].AngleMax;
                }
                modelNumber.Value = _Model.NumberI;
                numOrX.ReadOnly = numOrU.ReadOnly = numOrY.ReadOnly = false;
                if (decimal.TryParse(_Model.ScoreD.ToString(), out decimal score))
                {
                    ModelSeat.Value = score;
                }
                label4.Text = "阈值" + trackBar1.Value.ToString();
                label5.Text = "最小轮廓长度" + trackBar2.Value.ToString();
                minSael.Value = Convert.ToDecimal(_Model.MinScaelD);
                maxSael.Value = Convert.ToDecimal(_Model.MaxScaelD);
                comboBox4.Text = _Model.SubPixel;
                AngleStart.Value = Convert.ToDecimal(_Model.AngleStartDeg);
                AnlgeExtent.Value = Convert.ToDecimal(_Model.AngleExtentDeg);
                Greediness.Value = Convert.ToDecimal(_Model.GreedinessD);
                MaxOverlap.Value = Convert.ToDecimal(_Model.MaxOverlapD);
                numOrX.Text = _Model.OriginX.TupleString(".3f");
                numOrY.Text = _Model.OriginY.TupleString(".3f");
                numOrU.Text = _Model.OriginU.TupleDeg().TupleString(".3f");
                cbbMode.Text = _Model.Mode;
                button2.BackColor = _Model.COlorES;
                if (listBox2.Items.Count != 0)
                {
                    listBox2.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Vision.Log(ex.Message);
            }
            isChe = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("确定修改模板原点？", "修改模板", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    _Model.Xs = halcon.MRModelHomMat.Col[0];
                    _Model.Ys = halcon.MRModelHomMat.Row[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("确定修改模板区域？", "修改模板", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                //_Model.ROIModeXLD = halcon.GetObj().CopyObj(1, -1).Clone();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.AddObj(_Model.AOIObj.Clone());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.Focus();
                _Model.DrawROI(halcon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                HObject draw(HalconRun halcon)
                {
                    HOperatorSet.DrawPointMod(halcon.hWindowHalcon(), _Model.OriginY.TupleAdd(_Model.OriginYAdd), _Model.OriginX.TupleAdd(_Model.OriginXAdd), out HTuple hTupler, out HTuple hTupleC);
                    HOperatorSet.GenCrossContourXld(out HObject hObject, hTupler, hTupleC, 20, 0);
                    _Model.OriginXAdd = hTupleC - _Model.OriginX;
                    _Model.OriginYAdd = hTupler - _Model.OriginY;
                    //_Model.ROIModeXLD = _Model.ROIModeXLD.ConcatObj(hObject);
                    halcon.AddObj(_Model.ROIModeXLD.Clone());
                    return hObject;
                }
            }
            catch (Exception)
            {
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void ModelControl_Load(object sender, EventArgs e)
        {

            try
            {
                HWi1.Initialize(hWindowControl1);
                //HWi2.Initialize(hWindowControl2);
                //HWi2.SetImaage(halcon.Image());
                HWi1.SetImaage(halcon.Image());

                select_obj_type1.SetData(_Model.Variation_Model.select_Shape_Min_Max);
                DrawCont = this.Tag as DrawContrlos;
                propertyGrid1.SelectedObject=_Model.Variation_Model;
            }
            catch (Exception ex)
            {
            }
        }



        private void button8_Click(object sender, EventArgs e)
        {
            try
            {

                halcon.HobjClear();
                _Model.Create_ModelRegr = RunProgram.DrawModOBJ(halcon, HalconRun.EnumDrawType.Rectangle2, _Model.Create_ModelRegr);
                halcon.AddObj(_Model.DrawObj, ColorResult.blue);
                halcon.ShowObj();
                halcon.AddObj(_Model.Create_ModelRegr, ColorResult.yellow);
                if (comboBox3.SelectedItem.ToString() == "二值化区域")
                {
                    HObject hObject = _Model.Create_ModelRegr;

                    if (_Model.DrawObj.IsInitialized())
                    {
                        HOperatorSet.Difference(_Model.Create_ModelRegr, _Model.DrawObj, out hObject);
                    }
                    HOperatorSet.ReduceDomain(halcon.Image(), hObject, out HObject hObject2);
                    HOperatorSet.ThresholdSubPix(hObject2, out hObject2, 30);
                    HOperatorSet.SelectContoursXld(hObject2, out hObject2, "contour_length", 50, 999999, -0.5, 0.5);
                    halcon.AddObj(hObject2);
                }

                halcon.ShowObj();

            }
            catch (Exception)
            {
            }
            halcon.Drawing = false;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (comboBox3.SelectedItem.ToString() != "二值化区域")
                {
                    halcon.HobjClear();
                    //label4.Text = "阈值" + trackBar1.Value.ToString();
                    label5.Text = "最小轮廓长度" + trackBar2.Value.ToString();
                    HObject hObject = _Model.Create_ModelRegr;
                    if (_Model.DrawObj.IsInitialized())
                    {
                        HOperatorSet.Difference(hObject, _Model.DrawObj, out hObject);
                        halcon.AddObj(_Model.DrawObj, ColorResult.blue);
                    }
                    halcon.AddObj(_Model.Create_ModelRegr, ColorResult.yellow);
                    HOperatorSet.ReduceDomain(halcon.Image(), hObject, out HObject hObject2);

                    HOperatorSet.InspectShapeModel(hObject2, out HObject modeImages, out HObject modeRegions, (int)numericUpDown2.Value, trackBar1.Value);

                    //HOperatorSet.ThresholdSubPix(hObject2, out hObject2, trackBar1.Value);
                    //HOperatorSet.SelectContoursXld(hObject2, out hObject2, "contour_length", trackBar2.Value, 999999, -0.5, 0.5);
                    halcon.AddObj(modeRegions);
                    halcon.ShowObj();
                }
            }
            catch (Exception)
            {

            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isChe)
            {
                trackBar1.Enabled = true;
                trackBar2.Enabled = true;
                //if (comboBox3.SelectedItem.ToString() == "二值化区域")
                //{
                //    trackBar1.Enabled = true;
                //    trackBar2.Enabled = true;
                //}
                //else
                //{
                //    //trackBar1.Enabled = false;
                //    //trackBar2.Enabled = false;
                //}
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!isChe)
            {
                _Model.IsDip = checkBox1.Checked;
                dataGridView1.Enabled = _Model.IsDip;
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                while (dataGridView1.SelectedCells.Count!=0)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void 读取当前到错漏检测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();
                for (int i = 0; i < _Model.MRModelHomMat.NumberT; i++)
                {
                    if (_Model.DIPs.Count>i)
                    {
                        _Model.DIPs[i].Row = _Model.MRModelHomMat.Row[i];
                        _Model.DIPs[i].Col = _Model.MRModelHomMat.Col[i];
                        _Model.DIPs[i].Angle = _Model.MRModelHomMat.Phi[i];
                    }
                    else
                    {
                        _Model.DIPs.Add(new ModelVision.DIP() { Name = i + "Pt", Row = _Model.MRModelHomMat.Row[i], Col = _Model.MRModelHomMat.Col[i], Angle = _Model.MRModelHomMat.Phi[i] });
                    }
                }

                while (_Model.MRModelHomMat.NumberT< _Model.DIPs.Count)
                {
                    _Model.DIPs.RemoveAt(_Model.DIPs.Count-1);
                }
                for (int i = 0; i < _Model.DIPs.Count; i++)
                {
                    int d = dataGridView1.Rows.Add();
                    dataGridView1.Rows[d].Cells[0].Value = _Model.DIPs[i].Name;
                    dataGridView1.Rows[d].Cells[1].Value = _Model.DIPs[i].Row;
                    dataGridView1.Rows[d].Cells[2].Value = _Model.DIPs[i].Col;
                    dataGridView1.Rows[d].Cells[3].Value = _Model.DIPs[i].Angle;
                    dataGridView1.Rows[d].Cells[4].Value = _Model.DIPs[i].DistanceMax;
                    dataGridView1.Rows[d].Cells[5].Value = _Model.DIPs[i].AngleMax;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_Model == null)
                {
                    return;
                }
                if (_Model.DIPs.Count >= e.RowIndex)
                {
                    if (e.ColumnIndex == 0)
                    {
                        _Model.DIPs[e.RowIndex].Name = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    }
                    else if (e.ColumnIndex == 1)
                    {
                        _Model.DIPs[e.RowIndex].Row = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        _Model.DIPs[e.RowIndex].Col = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    }
                    else if (e.ColumnIndex == 3)
                    {
                        _Model.DIPs[e.RowIndex].Angle = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    }
                    else if (e.ColumnIndex == 4)
                    {
                        _Model.DIPs[e.RowIndex].DistanceMax = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    }
                    else if (e.ColumnIndex == 5)
                    {
                        _Model.DIPs[e.RowIndex].AngleMax = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            try
            {
                HWi1.SetImaage(_Model.Variation_Model.Create_Variation_Model(halcon.Image()));
                 string path=    ProjectINI.In.ProjectPathRun + "\\Vision\\" + Project.formula.Product.ProductionName 
                    + "\\Halcon\\" + halcon.Name + "\\" + _Model.Name + "\\形变图片";
                //if (Vision.ObjectValided(HWi1.HalconResult.Image))
                //{
                //     System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                //    HOperatorSet.WriteImage(HWi1.HalconResult.Image, "bmp", 0, path);
                //}
                MessageBox.Show("创建成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
 
        }


        private void button9_Click_1(object sender, EventArgs e)
        {
            try
            {
                _Model.Variation_Model.Xld = RunProgram.DrawModOBJ(halcon,HalconRun.EnumDrawType.Rectangle1 ,_Model.Variation_Model.Xld);
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
                _Model.Variation_Model.RunPa( halcon.GetOneImageR(), _Model, _Model.MRModelHomMat.HomMat,HWi1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            _Model.Variation_Model.Enbler = checkBox2.Checked;
        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                _Model.Variation_Model.Xld =RunProgram.DrawRmoveObj(halcon, _Model.Variation_Model.Xld);
                halcon.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
 
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                _Model.Variation_Model.Xld = RunProgram.DrawHObj(halcon, _Model.Variation_Model.Xld);
                halcon.ShowObj();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (isChe)
            {
                return;
            }
            _Model.Variation_Model.Name = textBox1.Text;
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        _Model.Variation_Model.CreateModeMode = "direct";

                        break;
                    case 1:
                        _Model.Variation_Model.CreateModeMode = "standard";

                        break;
                    case 2:
                        _Model.Variation_Model.CreateModeMode = "robust";
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                if (_Model.Variation_Model.CreateModeMode!= "direct")
                {
                    _Model.Variation_Model.get_variation_model(out HObject image, out HObject varImage);
                    HWi1.SetImaage(varImage);
                }else
                {
                    MessageBox.Show("多图训练模式下才可以读取模板图像");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        int d;
        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                HWi1.HobjClear();
                HWi1.SetImaage(halcon.Image());
                if (_Model.MRModelHomMat.HomMat.Count<=d)
                {
                    d = 0;
                }
                List<HTuple> hTuples = new List<HTuple>();
                hTuples.Add(_Model.MRModelHomMat.HomMat[d]);
                _Model.Variation_Model.RunPa( halcon.GetOneImageR(), _Model, hTuples, HWi1);
                d++;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                _Model.Variation_Model.Xld = RunProgram.DragMoveOBJ(halcon, _Model.Variation_Model.Xld);
                halcon.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            _Model.Variation_Model.IsHomed = checkBox5.Checked;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isChe)
                {
                    return;
                }
                halcon.HobjClear();
                if (_Model.ColorDic.ContainsKey(listBox2.SelectedItem.ToString()))
                {
                    _Classify = _Model.ColorDic[listBox2.SelectedItem.ToString()];
                }
                else
                {
                    _Classify = null;
                }
                if (_Classify == null)
                {
                    return;
                }
                Get_Pragram(_Classify);
                halcon.AddObj(_Classify.DrawObj, ColorResult.coral);
                halcon.ShowObj();
            }
            catch (Exception)
            {
            }
            isChe = false;
        }

        public void Set_Pragram()
        {
            if (isChe)
            {
                return;
            }
            try
            {
                halcon.HobjClear();
                if (_Classify == null)
                {
                    return;
                }
                _Classify.ImageType = (ImageTypeObj)Enum.Parse(typeof(ImageTypeObj),
                   comboBox2.SelectedItem.ToString());
                _Classify.EnbleSelect = checkBox8.Checked;
                _Classify.Enble = checkBox3.Checked;
                _Classify.IsColt = checkBox6.Checked;
                _Classify.IsHomMat = checkBox4.Checked;
                _Classify.ThresSelectMin = (byte)numericUpDown4.Value;
                _Classify.ThresSelectMax = (byte)numericUpDown5.Value;
                _Classify.SelectMin = (double)numericUpDown8.Value;
                _Classify.SelectMax = (double)numericUpDown7.Value;
                _Classify.ClosingCir = (double)numericUpDown6.Value;
                _Classify.ColorNumber = (byte)numericUpDown10.Value;
                _Classify.Color_ID = (byte)numericUpDown9.Value;
                _Classify.ISFillUp = checkBox10.Checked;
                _Classify.ClosingCircleValue = (double)numericUpDown11.Value;
                AoiObj aoiObj = _Model.GetAoi();
                aoiObj.SelseAoi = _Classify.DrawObj;
                _Classify.Classify( halcon.GetOneImageR(), aoiObj, _Model, out HObject hObject,  this.hObjects);
                halcon.AddObj(hObject);
                halcon.ShowImage();
                halcon.ShowObj();
            }
            catch (Exception ex)
            {
            }
        }

        public void Get_Pragram(Color_classify color_Classify)
        {
            isChe = true;
            try
            {
                checkBox3.Checked = _Classify.Enble;
                checkBox4.Checked = _Classify.IsHomMat;
                checkBox7.Checked = _Classify.ISSelecRoiFillUP;
                thresholdControl1.SetData(color_Classify.threshold_Min_Maxes);
                select_obj_type2.SetData(color_Classify.Max_area);
                numericUpDown10.Value = color_Classify.ColorNumber;
                numericUpDown9.Value = color_Classify.Color_ID;
                comboBox2.SelectedItem = color_Classify.ImageType.ToString();
                checkBox8.Checked = color_Classify.EnbleSelect;
                checkBox6.Checked = color_Classify.IsColt;
                numericUpDown4.Value = color_Classify.ThresSelectMin;
                numericUpDown5.Value = color_Classify.ThresSelectMax;
                numericUpDown7.Value = (decimal)color_Classify.SelectMax;
                numericUpDown8.Value = (decimal)color_Classify.SelectMin;
                numericUpDown6.Value = (decimal)color_Classify.ClosingCir;
                checkBox10.Checked = color_Classify.ISFillUp;
                numericUpDown11.Value = (decimal)color_Classify.ClosingCircleValue;
                listBox3.Items.Clear();
                listBox3.Items.Add("并集");
                for (int i = 0; i < _Classify.threshold_Min_Maxes.Count; i++)
                {
                    listBox3.Items.Add(_Classify.threshold_Min_Maxes[i].ImageTypeObj);
                }

                for (int i = 0; i < _Model.MRModelHomMat.NumberT; i++)
                {
                    HOperatorSet.AffineTransRegion(_Classify.DrawObj, out HObject hObjectROI, 
                        _Model.MRModelHomMat.HomMat[i], "nearest_neighbor");
                     AoiObj aoiObj=    _Model.GetAoi();
                    aoiObj.SelseAoi = hObjectROI;
                    _Classify.Classify( halcon.GetOneImageR(), aoiObj, _Model, out HObject hObject, hObjects);
                    halcon.AddObj(hObject);
                    HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple row, out HTuple column);
                    HOperatorSet.EllipticAxis(hObject, out HTuple ra, out HTuple rb, out HTuple phi);
                    //HOperatorSet.SmallestRectangle2(hObject, out row, out column, out HTuple  phi2,out HTuple length1,out  HTuple length2);
                    HOperatorSet.HeightWidthRatio(hObject, out HTuple height, out HTuple width, out HTuple cir);
                    HOperatorSet.SmallestCircle(hObject, out row, out column, out HTuple radius);
                    HTuple id = new HTuple();
                    if (area.Length != 0)
                    {
                        id = HTuple.TupleGenConst(area.Length, _Classify.Color_ID);
                        //halcon.AddMessage("ID:" + id + "面积:" + area + "长度:" + ra + "宽度:" + rb + "角度:" + phi.TupleDeg());
                        halcon.GetOneImageR().AddImageMassage(row, column, "面积" + area.TupleString("0.3f") + "ra" + ra.TupleString("0.3f") + "rb" + rb.TupleString("0.3f") + "高" +
                            height.TupleString("0.3f") + "宽" + width.TupleString("0.3f") + "半径" + radius.TupleString("0.3f"));
                        halcon.GetOneImageR().AddImageMassage(row + 40, column, "MM:面积" + Math.Sqrt(halcon.GetCaliConstMM(area)).ToString("0.000") + "ra" + halcon.GetCaliConstMM(ra).TupleString("0.3f")
                            + "rb" + halcon.GetCaliConstMM(rb).TupleString("0.3f") + "高" + halcon.GetCaliConstMM(height).TupleString("0.3f") + "宽" + halcon.GetCaliConstMM(width).TupleString("0.3f") +
                            "半径" + halcon.GetCaliConstMM(radius).TupleString("0.3f"));
                    }

                }
                listBox3.SelectedIndex = 0;
                halcon.ShowImage();
                halcon.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void button18_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (_Classify == null)
                {
                    return;
                }
                halcon.HobjClear();

                _Classify.DrawObj = RunProgram.DrawHObj(halcon, _Classify.DrawObj);
                    halcon.AddObj(_Classify.DrawObj, ColorResult.pink);
              
            }
            catch (Exception)
            {
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                if (_Classify == null)
                {
                    return;
                }
                halcon.HobjClear();

                _Classify.DrawObj = RunProgram.DrawRmoveObj(halcon, _Classify.DrawObj);
        
              halcon.AddObj(_Classify.DrawObj, ColorResult.pink);
                
            }
            catch (Exception)
            {
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (_Classify == null)
                {
                    return;
                }
                _Classify.DrawObj = RunProgram.DragMoveOBJ(halcon, _Classify.DrawObj);
            }
            catch (Exception ex)
            {
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            halcon.HobjClear();
            AoiObj aoiObj = _Model.GetAoi();
            aoiObj.SelseAoi = _Classify.DrawObj;
            _Classify.Classify( halcon.GetOneImageR(), aoiObj, _Model, out HObject hObject, this.hObjects);
            //_Classify.Classify(halcon, _Model, out HObject color);
            halcon.AddObj(hObject);
            halcon.ShowObj();
        }

    

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Set_Pragram();
        }

        private void 添加颜色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string meassge = "创建颜色";
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, "颜色", 100, 100);
                if (sd=="")
                {
                    return;
                }
                if (!_Model.ColorDic.ContainsKey(sd))
                {
                    if (_Model.ColorDic.ContainsKey(sd))
                    {
                        meassge = "名称已存在";
                        goto st;
                    }
                    _Model.ColorDic.Add(sd, new Color_Detection.Color_classify()
                    {
                        Name = sd,
                        Color_ID = (byte)(_Model.ColorDic.Count + 1),
                        threshold_Min_Maxes = new List<Threshold_Min_Max> { new Threshold_Min_Max() { ImageTypeObj=ImageTypeObj.H,
                              },new Threshold_Min_Max(){ ImageTypeObj=ImageTypeObj.S,},
                        new Threshold_Min_Max(){ ImageTypeObj=ImageTypeObj.V} }
                    }); 
                    listBox2.Items.Add(sd);
                }
                else
                {
                    MessageBox.Show(listBox2.SelectedItem.ToString() + "已存在");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string meassge = "重命名";
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, listBox2.SelectedItem.ToString(), 100, 100);
                if (sd == "")
                {
                    return;
                }
                if (_Model.ColorDic.ContainsKey(listBox2.SelectedItem.ToString()))
                {
                    if (_Model.ColorDic.ContainsKey(sd))
                    {
                        meassge = "名称已存在";
                        goto st;
                    }
                    _Model.ColorDic[listBox2.SelectedItem.ToString()].Name = sd;
                    _Model.ColorDic.Add(sd, _Model.ColorDic[listBox2.SelectedItem.ToString()]);
                    _Model.ColorDic.Remove(listBox2.SelectedItem.ToString());
                    int d = listBox2.SelectedIndex;
                    listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                    listBox2.Items.Insert(d, sd);
                }
                else
                {
                    MessageBox.Show(listBox1.SelectedItem.ToString() + "不存在");
                }
            }
            catch (Exception)
            {
            }
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_Model.ColorDic.ContainsKey(listBox2.SelectedItem.ToString()))
                {
                    _Model.ColorDic.Remove(listBox2.SelectedItem.ToString());
                    listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                }
                else
                {
                    listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                }
            }
            catch (Exception)
            {
            }
        }
  
        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            Set_Pragram();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Set_Pragram();
        }

        private void button22_Click_1(object sender, EventArgs e)
        {
            try
            {
                _Classify.RunSeleRoi(_Model.
                    GetEmset(halcon.GetImageOBJ(_Classify.ImageType)), 0, out HObject hObject);
                _Classify.DrawObj = hObject;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                if (_Classify != null)
                {
                    _Classify.SeleRoi = RunProgram.DrawHObj(halcon, _Classify.SeleRoi);
                }
                if (_Classify.SeleRoi == null || _Classify.SeleRoi.IsInitialized())
                {
                    halcon.AddObj(_Classify.SeleRoi, ColorResult.pink);
                }
            }
            catch (Exception)
            {
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isChe)
                {
                    return;
                }
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
                hWindID.SetPerpetualPart(row - 100, col1 - 100, row2 + 100, col2 + 100);
                hWindID.SetDraw(checkBox9.Checked);
                hWindID.OneResIamge.AddObj(_Classify.DrawObj, ColorResult.blue);
                hWindID.OneResIamge.AddObj(hObjects[listBox3.SelectedIndex]);
                hWindID.ShowObj();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string meassge = "复制{" + listBox2.SelectedItem.ToString() + "}";
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, listBox2.SelectedItem.ToString(), 100, 100);
                if (sd == "")
                {
                    return;
                }
                if (_Model.ColorDic.ContainsKey(listBox2.SelectedItem.ToString()))
                {
                    if (_Model.ColorDic.ContainsKey(sd))
                    {
                        meassge = "名称已存在";
                        goto st;
                    }
                    _Classify.Name = sd;
                    ProjectINI.StringJsonToCalss(ProjectINI.ClassToJsonString(_Classify), out Color_classify _Cla);
                    if (_Cla != null)
                    {
                        _Model.ColorDic.Add(sd, _Cla);
                        listBox2.Items.Add(sd);
                    }
                    else
                    {
                        MessageBox.Show("失败");
                    }
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
    }
}