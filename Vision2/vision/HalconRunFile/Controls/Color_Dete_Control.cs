using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.RunProgramFile.Color_Detection;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class Color_Dete_Control : UserControl
    {
        public Color_Dete_Control()
        {
            InitializeComponent();
        }
        Color_Detection.Color_classify _Classify;
        HalconRun halcon;
        RunProgram runProgram;






        private void button3_Click(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                _Classify.RunSeleRoi(runProgram.GetEmset(
                    halcon.GetImageOBJ(_Classify.ImageType)), 0, out HObject hObject);
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

        private void button8_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                button3.BackColor = colorDialog1.Color;//设置label1的背景颜色
                Set_Pragram(_Classify);
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
                _Classify.ColorNumber = (int)numericUpDown2.Value;
                _Classify.Color_ID = (byte)numericUpDown1.Value;
                _Classify.COlorES = button3.BackColor;
                _Classify.ISFillUp = checkBox1.Checked;
                _Classify.ClosingCircleValue = (double)numericUpDown3.Value;
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

        private void button9_Click(object sender, EventArgs e)
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

   

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Set_Pragram(_Classify);
            }
            catch (Exception)
            {
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            Set_Pragram(_Classify);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                _Classify.RunSeleRoi(runProgram.GetEmset(halcon.GetImageOBJ(_Classify.ImageType)), 0, out HObject hObject);
                _Classify.DrawObj = hObject;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
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

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}