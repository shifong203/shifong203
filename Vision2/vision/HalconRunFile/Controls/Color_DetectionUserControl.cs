using HalconDotNet;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.RunProgramFile.Color_Detection;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class Color_DetectionUserControl : UserControl
    {
        public Color_DetectionUserControl()
        {
            InitializeComponent();
            thresholdControl1.evValue += ThresholdControl1_evalue;
            hWindID.Initialize(hWindowControl1);
        }

        private List<HObject> hObjects = new List<HObject>();

        private void ThresholdControl1_evalue(List<Threshold_Min_Max> threshold_Min_s)
        {
            try
            {
                halcon.HobjClear();
                AoiObj aoiObj = new AoiObj();
                aoiObj.SelseAoi = _Classify.DrawObj;

                aoiObj.CiName = _Classify.Name;

                _Classify.Classify(halcon.GetOneImageR(), aoiObj,
                    Color_detection, out HObject hObject,
                    hObjects);

                hWindID.ShowImage();
                hWindID.ShowObj();

                halcon.AddObj(hObject, _Classify.color);
                HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple row, out HTuple column);
                HOperatorSet.EllipticAxis(hObject, out HTuple ra, out HTuple rb, out HTuple phi);
                HTuple id = new HTuple();
                id = HTuple.TupleGenConst(area.Length, _Classify.Color_ID);
                halcon.GetOneImageR().AddMeassge("ID:" + id + "面积:" + area + "长度:" + ra + "宽度:" + rb + "角度:" + phi.TupleDeg());
                halcon.GetOneImageR().AddImageMassage(row, column, "ID:" + id, ColorResult.yellow);
                halcon.GetOneImageR().AddImageMassage(row + 40, column, "面积:" + area, ColorResult.yellow);
                halcon.GetOneImageR().AddImageMassage(row + 80, column, "长度:" + ra, ColorResult.yellow);
                halcon.GetOneImageR().AddImageMassage(row + 120, column, "宽度:" + rb, ColorResult.yellow);
                halcon.GetOneImageR().AddImageMassage(row + 160, column, "角度:" + phi.TupleDeg(), ColorResult.yellow);
                halcon.ShowImage();
                halcon.ShowObj();
            }
            catch (Exception ex)
            { }
        }

        private HWindID hWindID = new HWindID();

        //public List<HWindID> hWindIDs = new List<HWindID>();
        public Color_DetectionUserControl(Color_Detection color_Detection) : this()
        {
            Color_detection = color_Detection;
            propertyGrid1.SelectedObject = color_Detection;
            halcon = (HalconRun)color_Detection.GetPThis();
            RunProgram = Color_detection;
            foreach (var item in Color_detection.keyColor.Keys)
            {
                listBox1.Items.Add(item);
            }
            comboBox_ImageType.Items.Clear();
            //UpDataOBJ(welding_Spot);
            comboBox_ImageType.Items.AddRange(Enum.GetNames(typeof(ImageTypeObj)));
        }

        public HalconRun halcon;
        public Color_Detection Color_detection;
        public Color_classify _Classify;
        private bool isMove = false;

        public void Get_Pragram(Color_classify color_Classify)
        {
            isMove = true;
            try
            {
                _Classify = color_Classify;
                propertyGrid2.SelectedObject = _Classify;
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
                checkBoxEnbleDifference.Checked = _Classify.EnbleDifference;
                checkBoxEnbleDifferenceAngl.Checked = _Classify.EnbleDifferenceAngl;

                checkBox_Enble.Checked = _Classify.Enble;
                checkBox_IsColt.Checked = _Classify.IsColt;
                thresholdControl1.SetData(color_Classify.threshold_Min_Maxes);
                select_obj_type1.SetData(color_Classify.Max_area);
                select_obj_type2.SetData(color_Classify.Diffe_Max_area);
                numericUpDown_ColorNumber.Value = color_Classify.ColorNumber;
                button3.BackColor = color_Classify.COlorES;
                numericUpDown_Color_ID.Value = color_Classify.Color_ID;
                comboBox_ImageType.SelectedItem = color_Classify.ImageType.ToString();
                checkBox_EnbleSelect.Checked = color_Classify.EnbleSelect;
                numericUpDown_ThresSelectMin.Value = color_Classify.ThresSelectMin;
                numericUpDown_ThresSelectMax.Value = color_Classify.ThresSelectMax;
                numericUpDown_SelectMax.Value = (decimal)color_Classify.SelectMax;
                numericUpDown_SelectMin.Value = (decimal)color_Classify.SelectMin;
                numericUpDown_ClosingCir.Value = (decimal)color_Classify.ClosingCir;
                numericUpDownDifferenceInt.Value = (decimal)_Classify.DifferenceInt;
                checkBox_ISFillUp.Checked = color_Classify.ISFillUp;
                checkBox_ISSelecRoiFillUP.Checked = color_Classify.ISSelecRoiFillUP;
                numericUpDown_ClosingCircleValue.Value = (decimal)color_Classify.ClosingCircleValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isMove = false;
        }

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
                _Classify.EnbleDifferenceAngl = checkBoxEnbleDifferenceAngl.Checked;
                _Classify.ImageType = (ImageTypeObj)Enum.Parse(typeof(ImageTypeObj), comboBox_ImageType.SelectedItem.ToString());
                _Classify.EnbleSelect = checkBox_EnbleSelect.Checked;
                _Classify.IsColt = checkBox_IsColt.Checked;
                _Classify.ISSelecRoiFillUP = checkBox_ISSelecRoiFillUP.Checked;
                _Classify.Enble = checkBox_Enble.Checked;
                _Classify.EnbleDifference = checkBoxEnbleDifference.Checked;
                _Classify.ThresSelectMin = (byte)numericUpDown_ThresSelectMin.Value;
                _Classify.ThresSelectMax = (byte)numericUpDown_ThresSelectMax.Value;
                _Classify.SelectMin = (double)numericUpDown_SelectMin.Value;
                _Classify.SelectMax = (double)numericUpDown_SelectMax.Value;
                _Classify.ClosingCir = (double)numericUpDown_ClosingCir.Value;
                _Classify.ColorNumber = (byte)numericUpDown_ColorNumber.Value;
                _Classify.Color_ID = (byte)numericUpDown_Color_ID.Value;
                _Classify.DifferenceInt = (double)numericUpDownDifferenceInt.Value;
                _Classify.COlorES = button3.BackColor;
                _Classify.ISFillUp = checkBox_ISFillUp.Checked;
                _Classify.EnbleDifference = checkBoxEnbleDifference.Checked;
                _Classify.ClosingCircleValue = (double)numericUpDown_ClosingCircleValue.Value;
                AoiObj aoiObj = new AoiObj();

                aoiObj.SelseAoi = _Classify.DrawObj;

                aoiObj.CiName = _Classify.Name;
                _Classify.Classify(halcon.GetOneImageR(), aoiObj, Color_detection, out HObject hObject, hObjects);
                halcon.AddObj(hObject);
                halcon.ShowImage();
                halcon.ShowObj();
            }
            catch (Exception ex) { }
        }

        public Color_DetectionUserControl( RunProgram welding_Spot) : this()
        {
            //UpDataOBJ(welding_Spot);
        }

       private RunProgramFile.RunProgram RunProgram;

        //public void UpDataOBJ(RunProgramFile.Welding_Spot welding_Spot)
        //{
        //    Welding = welding_Spot;
        //}

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!Color_detection.keyColor.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    MessageBox.Show("不存在" + listBox1.SelectedItem.ToString());
                    return;
                }
                Set_Pragram(Color_detection.keyColor[listBox1.SelectedItem.ToString()]);
            }
            catch (Exception)
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
                halcon.HobjClear();
                if (!Color_detection.keyColor.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    MessageBox.Show("不存在" + listBox1.SelectedItem.ToString());
                    return;
                }

                Get_Pragram(Color_detection.keyColor[listBox1.SelectedItem.ToString()]);
                HOperatorSet.ReduceDomain(halcon.GetImageOBJ((ImageTypeObj)Enum.Parse(typeof(ImageTypeObj), _Classify.threshold_Min_Maxes[0].ImageTypeObj.ToString())),
                    _Classify.DrawObj, out HObject images);

                userCtrlThreshold2.SetData(_Classify.threshold_Min_Maxes[0], images);
                listBox2.Items.Clear();
                listBox2.Items.Add("并集");
                for (int i = 0; i < _Classify.threshold_Min_Maxes.Count; i++)
                {
                    listBox2.Items.Add(_Classify.threshold_Min_Maxes[i].ImageTypeObj);
                }
                AoiObj aoiObj = new AoiObj();
                aoiObj.SelseAoi = _Classify.DrawObj;
                aoiObj.CiName = _Classify.Name;
                List<HTuple> homitd=      Color_detection.GetHomMatList(halcon.GetOneImageR());
                //if (homitd.Count >= 1)
                //{
                //    HOperatorSet.AffineTransRegion(aoiObj.SelseAoi, out aoiObj.SelseAoi, homitd[0], "nearest_neighbor");
                //}
                //if (!_Classify.EnbleSelect)
                //{
                //    if (aoiObj.SelseAoi != null && aoiObj.SelseAoi.IsInitialized())
                //    {
                //        halcon.AddObj(aoiObj.SelseAoi, ColorResult.coral);
                //    }
                //}
                aoiObj.DebugID = 1;
                _Classify.Classify(halcon.GetOneImageR(), aoiObj, Color_detection, out HObject hObject,
                this.hObjects);
                //hWindID.SetImaage(halcon.GetImageOBJ((ImageTypeObj)Enum.Parse(typeof(ImageTypeObj), _Classify.threshold_Min_Maxes[0].ImageTypeObj.ToString())));
                //listBox2.SelectedIndex = 1;
                HOperatorSet.SmallestRectangle1(_Classify.DrawObj, out HTuple row, out HTuple col1, out HTuple row2, out HTuple col2);

                hWindID.SetPerpetualPart(row - 100, col1 - 100, row2 + 100, col2 + 100);
                thresholdControl1_DispButt(_Classify.threshold_Min_Maxes[0].ImageTypeObj.ToString(), 0);
               
                hWindID.ShowImage();
                hWindID.ShowObj();
                halcon.AddObj(hObject, _Classify.color);
                HOperatorSet.AreaCenter(hObject, out HTuple area, out  row, out HTuple column);
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
                halcon.ShowImage();
                halcon.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 添加颜色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string meassge = "创建颜色";
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, "颜色", 100, 100);
                if (sd == "")
                {
                    return;
                }
                if (!Color_detection.keyColor.ContainsKey(sd))
                {
                    if (Color_detection.keyColor.ContainsKey(sd))
                    {
                        meassge = "名称已存在";
                        goto st;
                    }
                    Color_detection.keyColor.Add(sd, new Color_Detection.Color_classify()
                    {
                        Name = sd,
                        Color_ID = (byte)(Color_detection.keyColor.Count + 1),
                        threshold_Min_Maxes = new List<Threshold_Min_Max> { new Threshold_Min_Max() { ImageTypeObj=ImageTypeObj.H,
                    },new Threshold_Min_Max(){ ImageTypeObj=ImageTypeObj.S,},
                        new Threshold_Min_Max(){ ImageTypeObj=ImageTypeObj.V} }
                    });
                    listBox1.Items.Add(sd);
                }
                else
                {
                    MessageBox.Show(listBox1.SelectedItem.ToString() + "已存在");
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
                string sd = Interaction.InputBox("请输入新名称", meassge, listBox1.SelectedItem.ToString(), 100, 100);
                if (sd == "")
                {
                    return;
                }
                if (Color_detection.keyColor.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    if (Color_detection.keyColor.ContainsKey(sd))
                    {
                        meassge = "名称已存在";
                        goto st;
                    }
                    _Classify.Name = sd;
                    Color_detection.keyColor.Add(sd, _Classify);
                    Color_detection.keyColor.Remove(listBox1.SelectedItem.ToString());
                    int d = listBox1.SelectedIndex;
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    listBox1.Items.Insert(d, sd);
                }
                else
                {
                    MessageBox.Show(listBox1.SelectedItem.ToString() + "不存在");
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
                if (Color_detection.keyColor.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    Color_detection.keyColor.Remove(listBox1.SelectedItem.ToString());
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                }
                else
                {
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                }
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                if (Color_detection.keyColor.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    _Classify.DrawObj = RunProgram.DrawHObj(halcon, _Classify.DrawObj);
                }
                if (_Classify.DrawObj == null || _Classify.DrawObj.IsInitialized())
                {
                    halcon.AddObj(_Classify.DrawObj, ColorResult.pink);
                }
            }
            catch (Exception)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                if (Color_detection.keyColor.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    _Classify.DrawObj = RunProgram.DrawRmoveObj(halcon, _Classify.DrawObj);
                }
                if (_Classify.DrawObj == null || _Classify.DrawObj.IsInitialized())
                {
                    halcon.AddObj(_Classify.DrawObj, ColorResult.pink);
                }
            }
            catch (Exception)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                button3.BackColor = colorDialog1.Color;//设置label1的背景颜色
                Set_Pragram(Color_detection.keyColor[listBox1.SelectedItem.ToString()]);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    return;
                }
                _Classify.DrawObj =
                 RunProgram.DragMoveOBJ(halcon, _Classify.DrawObj);
            }
            catch (Exception ex)
            {
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                _Classify.RunSeleRoi(Color_detection.
                    GetEmset(halcon.GetImageOBJ(_Classify.ImageType)), 0, out HObject hObject);
                _Classify.DrawObj = hObject;
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

        private void thresholdControl1_Load(object sender, EventArgs e)
        {
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hWindID.HobjClear();
                HOperatorSet.SmallestRectangle1(_Classify.DrawObj, out HTuple row, out HTuple col1, out HTuple row2, out HTuple col2);
                if (listBox2.SelectedIndex == 0)
                {
                    hWindID.SetImaage(halcon.Image());
                }
                else
                {
                    hWindID.SetImaage(halcon.GetImageOBJ((ImageTypeObj)Enum.Parse(typeof(ImageTypeObj), listBox2.SelectedItem.ToString())));
                }
                AoiObj aoiObj = RunProgram.GetAoi();
        
                if (_Classify.IsHomMat)
                {
                    List<HTuple> listHomet = RunProgram.GetHomMatList(halcon.GetOneImageR());
                    HOperatorSet.AffineTransRegion(_Classify.DrawObj, out HObject hObjectROI,
                    listHomet[0], "nearest_neighbor");
                    aoiObj.SelseAoi = hObjectROI;
                }
                else
                {
                    aoiObj.SelseAoi = _Classify.DrawObj;
                }
                groupBox3.Text = listBox2.SelectedItem.ToString();
                hWindID.SetPerpetualPart(row - 100, col1 - 100, row2 + 100, col2 + 100);
                hWindID.SetDraw(checkBox2.Checked);
                hWindID.OneResIamge.AddObj(_Classify.DrawObj, ColorResult.blue);
                hWindID.OneResIamge.AddObj(hObjects[listBox2.SelectedIndex]);
                hWindID.ShowObj();
              
                //HImage hImage = new HImage(hWindID.Image());
                //userCtrlThreshold1.Fuction(hImage);
            }
            catch (Exception)
            {
            }
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string meassge = "复制{" + listBox1.SelectedItem.ToString() + "}";
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, listBox1.SelectedItem.ToString(), 100, 100);
                if (sd == "")
                {
                    return;
                }
                if (Color_detection.keyColor.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    if (Color_detection.keyColor.ContainsKey(sd))
                    {
                        meassge = "名称已存在";
                        goto st;
                    }
                    _Classify.Name = sd;

                    ErosProjcetDLL.Project.ProjectINI.StringJsonToCalss(ErosProjcetDLL.Project.ProjectINI.ClassToJsonString(_Classify), out Color_classify _Cla);
                    if (_Cla != null)
                    {
                        Color_detection.keyColor.Add(sd, _Cla);
                        listBox1.Items.Add(sd);
                    }
                    else
                    {
                        MessageBox.Show("失败");
                    }
                }
                else
                {
                    MessageBox.Show(listBox1.SelectedItem.ToString() + "不存在");
                }
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
                halcon.HobjClear();
                if (_Classify != null)
                {
                    _Classify.SeleRoi = RunProgram.DrawRmoveObj(halcon, _Classify.SeleRoi);
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

        private void Color_DetectionUserControl_Load(object sender, EventArgs e)
        {
            try
            {
              

            }
            catch (Exception)
            {
            }
        }

        private void UserCtrlThreshold1_ThresholdChanged(Threshold_Min_Max threshold_Min_Max)
        {
            try
            {

            }
            catch (Exception)
            {
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.SmallestRectangle2(_Classify.OutRiong, out HTuple row, out HTuple colum, out HTuple phi, out HTuple length1, out HTuple length2);
                HOperatorSet.SmallestRectangle2(_Classify.ModeRoing, out HTuple Moderow, out HTuple Modecolu, out HTuple phi2, out HTuple length12, out HTuple length22);
                HTuple homMat2d;
                if (_Classify.EnbleDifferenceAngl)
                {
                    HOperatorSet.VectorAngleToRigid(Moderow, Modecolu, phi2, row, colum, phi, out homMat2d);
                }
                else
                {
                    HOperatorSet.VectorAngleToRigid(Moderow, Modecolu, 0, row, colum, 0, out homMat2d);
                }
                HOperatorSet.GenCrossContourXld(out HObject hObject, row, colum, 60, phi);
                HOperatorSet.GenCrossContourXld(out HObject hObjectx, Moderow, Modecolu, 60, phi2);

                HOperatorSet.AffineTransRegion(_Classify.ModeRoing, out HObject modeHomMatRoing, homMat2d, "nearest_neighbor");
                HOperatorSet.Difference(modeHomMatRoing, _Classify.OutRiong, out HObject hObject1);
                HOperatorSet.Difference(_Classify.OutRiong, modeHomMatRoing, out HObject hObject2);
                halcon.GetOneImageR().AddObj(hObjectx, ColorResult.green);
                halcon.GetOneImageR().AddObj(hObject, ColorResult.green);
                halcon.GetOneImageR().AddObj(modeHomMatRoing, ColorResult.yellow);
                halcon.GetOneImageR().AddObj(_Classify.ModeRoing, ColorResult.blue);
                halcon.GetOneImageR().AddObj(_Classify.OutRiong, ColorResult.green);
            }
            catch (Exception)
            {

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {

                _Classify.ModeRoing = _Classify.OutRiong;
            }
            catch (Exception)
            {
            }
        }

        private void thresholdControl1_DispButt(string dispV,int dispIdxe)
        {
            try
            {
                try
                {
                    hWindID.HobjClear();
                    HOperatorSet.SmallestRectangle1(_Classify.DrawObj, out HTuple row, out HTuple col1, out HTuple row2, out HTuple col2);
                    //if (listBox2.SelectedIndex == 0)
                    //{
                    //    hWindID.SetImaage(halcon.Image());
                    //}
                 
                   hWindID.SetImaage(halcon.GetImageOBJ((ImageTypeObj)Enum.Parse(typeof(ImageTypeObj), dispV)),false);
                    
                    AoiObj aoiObj = RunProgram.GetAoi();

                    if (_Classify.IsHomMat)
                    {
                        List<HTuple> listHomet = RunProgram.GetHomMatList(halcon.GetOneImageR());
                        HOperatorSet.AffineTransRegion(_Classify.DrawObj, out HObject hObjectROI,
                        listHomet[0], "nearest_neighbor");
                        aoiObj.SelseAoi = hObjectROI;
                    }
                    else
                    {
                        aoiObj.SelseAoi = _Classify.DrawObj;
                    }
                    groupBox3.Text = dispV;
                    //hWindID.SetPerpetualPart(row - 100, col1 - 100, row2 + 100, col2 + 100);
                    hWindID.SetDraw(checkBox2.Checked);
                    hWindID.OneResIamge.AddObj(_Classify.DrawObj, ColorResult.blue);
                    hWindID.OneResIamge.AddObj(hObjects[dispIdxe+1]);
                    hWindID.ShowObj();

                    //HImage hImage = new HImage(hWindID.Image());
                  
                }
                catch (Exception)
                {
                }

            }
            catch (Exception)
            {

            }
        }

        private void userCtrlThreshold2_ThresholdChanged(Threshold_Min_Max threshold_Min_Max)
        {
            try
            {
                hWindID.HobjClear();
                AoiObj aoiObj = RunProgram.GetAoi();
                aoiObj.SelseAoi = _Classify.DrawObj;

                aoiObj.CiName = _Classify.Name;
                List<HTuple> homitd = Color_detection.GetHomMatList(halcon.GetOneImageR());
          
                aoiObj.DebugID = 1;
                _Classify.Classify(halcon.GetOneImageR(), aoiObj, Color_detection, out HObject hObject,
                          this.hObjects);
                HOperatorSet.SmallestRectangle1(_Classify.DrawObj, out HTuple row, out HTuple col1, out HTuple row2, out HTuple col2);
                //if (listBox2.SelectedIndex == 0)
                //{
                //    hWindID.SetImaage(halcon.Image(),false);
                //}
                //else
                //{
                    hWindID.SetImaage(halcon.GetImageOBJ((ImageTypeObj)Enum.Parse(typeof(ImageTypeObj),
                       threshold_Min_Max.ImageTypeObj.ToString())),false);
                //}
           

                if (_Classify.IsHomMat)
                {
                    List<HTuple> listHomet = RunProgram.GetHomMatList(halcon.GetOneImageR());
                    HOperatorSet.AffineTransRegion(_Classify.DrawObj, out HObject hObjectROI,
                    listHomet[0], "nearest_neighbor");
                    aoiObj.SelseAoi = hObjectROI;
                }
                else
                {
                    aoiObj.SelseAoi = _Classify.DrawObj;
                }
                groupBox3.Text = listBox2.SelectedItem.ToString();
                //hWindID.SetPerpetualPart(row - 100, col1 - 100, row2 + 100, col2 + 100);
                hWindID.SetDraw(checkBox2.Checked);
                hWindID.OneResIamge.AddObj(_Classify.DrawObj, ColorResult.blue);
                hWindID.OneResIamge.AddObj(hObjects[listBox2.SelectedIndex]);
                hWindID.ShowObj();

                //HImage hImage = new HImage(hWindID.Image());
                //userCtrlThreshold1.Fuction(hImage);
            }
            catch (Exception)
            {
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }
    }
}