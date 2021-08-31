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
            foreach (var item in Color_detection.keyColor.Keys)
            {
                listBox1.Items.Add(item);
            }
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Enum.GetNames(typeof(ImageTypeObj)));
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
                checkBox6.Checked = _Classify.Enble;
                checkBox5.Checked = _Classify.IsColt;
                thresholdControl1.SetData(color_Classify.threshold_Min_Maxes);
                select_obj_type1.SetData(color_Classify.Max_area);
                numericUpDown2.Value = color_Classify.ColorNumber;
                button3.BackColor = color_Classify.COlorES;
                numericUpDown1.Value = color_Classify.Color_ID;
                comboBox1.SelectedItem = color_Classify.ImageType.ToString();
                checkBox3.Checked = color_Classify.EnbleSelect;
                numericUpDown4.Value = color_Classify.ThresSelectMin;
                numericUpDown5.Value = color_Classify.ThresSelectMax;
                numericUpDown7.Value = (decimal)color_Classify.SelectMax;
                numericUpDown8.Value = (decimal)color_Classify.SelectMin;
                numericUpDown6.Value = (decimal)color_Classify.ClosingCir;
                checkBox1.Checked = color_Classify.ISFillUp;
                checkBox4.Checked = color_Classify.ISSelecRoiFillUP;
                numericUpDown3.Value = (decimal)color_Classify.ClosingCircleValue;
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
                _Classify.ImageType = (ImageTypeObj)Enum.Parse(typeof(ImageTypeObj), comboBox1.SelectedItem.ToString());
                _Classify.EnbleSelect = checkBox3.Checked;
                _Classify.IsColt = checkBox5.Checked;
                _Classify.ISSelecRoiFillUP = checkBox4.Checked;
                _Classify.Enble = checkBox6.Checked;
                _Classify.ThresSelectMin = (byte)numericUpDown4.Value;
                _Classify.ThresSelectMax = (byte)numericUpDown5.Value;
                _Classify.SelectMin = (double)numericUpDown8.Value;
                _Classify.SelectMax = (double)numericUpDown7.Value;
                _Classify.ClosingCir = (double)numericUpDown6.Value;
                _Classify.ColorNumber = (byte)numericUpDown2.Value;
                _Classify.Color_ID = (byte)numericUpDown1.Value;
                _Classify.COlorES = button3.BackColor;
                _Classify.ISFillUp = checkBox1.Checked;
                _Classify.ClosingCircleValue = (double)numericUpDown3.Value;
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

        public Color_DetectionUserControl(RunProgramFile.Welding_Spot welding_Spot) : this()
        {
            UpDataOBJ(welding_Spot);
        }

        private RunProgramFile.Welding_Spot Welding;

        public void UpDataOBJ(RunProgramFile.Welding_Spot welding_Spot)
        {
            Welding = welding_Spot;
        }

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
                listBox2.SelectedIndex = 0;
                hWindID.ShowImage();
                hWindID.ShowObj();
                halcon.AddObj(hObject, _Classify.color);
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
                groupBox3.Text = listBox2.SelectedItem.ToString();
                hWindID.SetPerpetualPart(row - 100, col1 - 100, row2 + 100, col2 + 100);
                hWindID.SetDraw(checkBox2.Checked);
                hWindID.OneResIamge.AddObj(_Classify.DrawObj, ColorResult.blue);
                hWindID.OneResIamge.AddObj(hObjects[listBox2.SelectedIndex]);
                hWindID.ShowObj();
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

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}