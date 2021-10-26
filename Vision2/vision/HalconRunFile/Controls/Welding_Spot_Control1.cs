using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class Welding_Spot_Control1 : UserControl
    {
        public Welding_Spot_Control1()
        {
            InitializeComponent();
        }

        public Welding_Spot_Control1(Welding_Spot welding_Spot) : this()
        {
            UpProgram(welding_Spot);
        }

        public void UpProgram(Welding_Spot welding_Spot)
        {
            isMove = true;
            hWind.Initialize(hWindowControl1);

            welding_ = welding_Spot;
            if (welding_.listWelding.Count > 1)
            {
                WeldingCCT = welding_.listWelding[0];
            }
            if (welding_.listWelding.Count == 0)
            {
                WeldingCCT = new Welding_Spot.WeldingCC();
            }
            GetProgram();
            Halcon = (HalconRun)welding_.GetPThis();
            isMove = false;
        }

        private bool isMove;
        private HWindID hWind = new HWindID();

        private HalconRun Halcon;
        private Welding_Spot welding_;

        private void Welding_Spot_Control1_Load(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                for (int i = 0; i < welding_.listWelding.Count; i++)
                {
                    listBox1.Items.Add(i + 1);
                }
                WeldingCCT = welding_.listWelding[0];

                GetProgram();
            }
            catch (Exception)
            {
            }
        }

        private HObject hObjectImage;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Halcon.HobjClear();
                if (Halcon.Drawing)
                {
                    return;
                }
                if (welding_.DrawObj != null && welding_.DrawObj.CountObj() == 1)
                {
                    HObject hObject = RunProgram.DragMoveOBJ(Halcon, welding_.DrawObj);
                    Halcon.AddObj(hObject);
                    HOperatorSet.ReduceDomain(Halcon.Image(), hObject, out hObjectImage);
                    welding_.listWelding.Add(new Welding_Spot.WeldingCC() { HObject = hObject, });
                    listBox1.Items.Clear();
                    for (int i = 0; i < welding_.listWelding.Count; i++)
                    {
                        listBox1.Items.Add(i + 1);
                    }
                    HOperatorSet.ReduceDomain(Halcon.Image(), WeldingCCT.HObject, out hObjectImage);
                    if (WeldingCCT.Solder_joint_inspection(welding_, Halcon.GetOneImageR(), out bool iscon, out bool isless, out HTuple areas, this.hWind))
                    {
                        Halcon.AddObj(WeldingCCT.HObject);
                    }
                    else
                    {
                        Halcon.AddObj(WeldingCCT.HObject, ColorResult.red);
                    }

                    Halcon.ShowImage();
                }
                else
                {
                    Halcon.AddMeassge("未绘制添加区域");
                }
                Halcon.ShowObj();
            }
            catch (Exception ex)
            {
            }
            Cursor = Cursors.Default;
            Halcon.Drawing = false;
        }

        private ImageTypeObj RGBHSVEnum = ImageTypeObj.R;

        private void SetProgram(sbyte id)
        {
            try
            {
                if (isMove)
                {
                    return;
                }
                Halcon.HobjClear();

                Halcon.ShowImage();
                welding_.R_threa_min = (byte)numericUpDown2.Value;
                welding_.R_threa_max = (byte)numericUpDown1.Value;
                welding_.R_area_compute_thr_max = (byte)numericUpDown15.Value;
                welding_.R_area_compute_thr_min = (byte)numericUpDown16.Value;
                WeldingCCT.CiericROut = (double)numericUpDown11.Value;
                WeldingCCT.CiericRInt = (double)numericUpDown12.Value;
                WeldingCCT.IsEllIpse = checkBox1.Checked;
                WeldingCCT.Radius2 = (double)numericUpDown13.Value;
                WeldingCCT.Phi = (double)numericUpDown14.Value;

                welding_.H_enabled = true;
                welding_.Overflow_Welding_Area = (double)numericUpDown3.Value;
                welding_.H_threa_min = (byte)numericUpDownHThrMin.Value;
                welding_.H_threa_max = (byte)numericUpDownHThrMax.Value;
                welding_.H_sele_area_min = (double)numericUpDownAreaMin.Value;
                welding_.H_sele_area_max = new HTuple((double)numericUpDownAreaMax.Value);
                welding_.Welding_ra = (double)numericUpDown17.Value;
                welding_.S_enabled = true;
                welding_.S_threa_min = (byte)numericUpDownSThrMin.Value;
                welding_.S_threa_max = (byte)numericUpDownSThrMax.Value;

                welding_.V_enabled = true;
                welding_.V_threa_min = (byte)numericUpDownVThrMin.Value;
                welding_.V_threa_max = (byte)numericUpDownVThrMax.Value;
                welding_.H_area_compute_thr_min = (byte)numericUpDownHAraeComputeMin.Value;
                welding_.H_area_compute_thr_max = (byte)numericUpDownHAraeComputeMax.Value;
                welding_.S_area_compute_thr_min = (byte)numericUpDownSAreaComputeThrMin.Value;
                welding_.S_area_compute_thr_max = (byte)numericUpDownSAreaComputeThrMax.Value;
                welding_.V_area_compute_thr_min = (byte)numericUpDownVAreaComputeThrMin.Value;
                welding_.V_area_compute_thr_max = (byte)numericUpDownVAreaComputeThrMax.Value;
                //welding_.R_area_compute_thr_min = (byte)numericUpDown2.Value;
                //welding_.R_area_compute_thr_max = (byte)numericUpDown1.Value;
                welding_.Compute_sele_area_min = (double)numericUpDownComputeSeleAreaMin.Value;

                welding_.Compute_sele_area_max = (double)numericUpDownComputeSeleAreaMax.Value;
                WeldingCCT.Number = (byte)numericUpDownNumber.Value;
                welding_.NoNeedle_H_thr_min = (byte)numericUpDown9.Value;
                welding_.NoNeedle_H_thr_max = (byte)numericUpDown6.Value;
                welding_.NoNeedle_S_thr_min = (byte)numericUpDown8.Value;
                welding_.NoNeedle_S_thr_max = (byte)numericUpDown7.Value;
                welding_.NoNeedle_V_thr_min = (byte)numericUpDown5.Value;
                welding_.NoNeedle_V_thr_max = (byte)numericUpDown4.Value;
                welding_.NoNeedle_area_min = (double)numericUpDown10.Value;

                while (WeldingCCT.Number < WeldingCCT.ListHObj.Count)
                {
                    WeldingCCT.ListHObj.RemoveAt(WeldingCCT.ListHObj.Count - 1);
                }
                while (WeldingCCT.Number > WeldingCCT.ListHObj.Count)
                {
                    WeldingCCT.ListHObj.Add(new HObject());
                    WeldingCCT.ListHObj[WeldingCCT.ListHObj.Count - 1].GenEmptyObj();
                }
                if (listBox2.Items.Count > WeldingCCT.ListHObj.Count)
                {
                    listBox2.Items.Clear();
                }

                while (listBox2.Items.Count < WeldingCCT.ListHObj.Count)
                {
                    listBox2.Items.Add(listBox2.Items.Count + 1);
                }

                List<HTuple> hTuples = welding_.GetHomMatList(Halcon.GetOneImageR());
                HObject  imageObj = WeldingCCT.HObject;

                if (hTuples.Count!=0)
                {

                    imageObj= welding_.GetHomMatObj(imageObj, hTuples[0]);
                }
                HOperatorSet.ReduceDomain(Halcon.Image(), imageObj, out hObjectImage);
                HOperatorSet.SmallestRectangle1(imageObj, out HTuple row1, out HTuple column1, out HTuple row2, out HTuple column2);
                hWind.SetPerpetualPart(row1, column1, row2, column2);
                hWind.SetImaage(hObjectImage);
                hWind.ShowImage();
                if (WeldingCCT.Solder_joint_inspection(welding_, Halcon.GetOneImageR(), out bool iscon, out bool isless, out HTuple areas, hWind, RGBHSVEnum, id))
                {
                    Halcon.AddObj(imageObj);
                }
                else
                {
                    Halcon.AddObj(imageObj, ColorResult.red);
                }
                hWind.SetPerpetualPart(row1, column1, row2, column2);
                hWind.ShowImage();
                Halcon.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetProgram()
        {
            isMove = true;
            try
            {
                numericUpDownHThrMin.Value = welding_.H_threa_min;
                numericUpDownHThrMax.Value = welding_.H_threa_max;
                numericUpDownAreaMin.Value = (decimal)welding_.H_sele_area_min.D;
                numericUpDownAreaMax.Value = (decimal)welding_.H_sele_area_max.D;
                numericUpDown3.Value = (decimal)welding_.Overflow_Welding_Area;
                numericUpDown11.Value = (decimal)WeldingCCT.CiericROut;
                numericUpDown12.Value = (decimal)WeldingCCT.CiericRInt;
                numericUpDown17.Value = (decimal)welding_.Welding_ra;
                checkBox1.Checked = WeldingCCT.IsEllIpse;
                numericUpDown13.Value = (decimal)WeldingCCT.Radius2;
                numericUpDown14.Value = (decimal)WeldingCCT.Phi;

                numericUpDownSThrMin.Value = welding_.S_threa_min;
                numericUpDownSThrMax.Value = welding_.S_threa_max;
                //numericUpDownAreaMin.Value = (decimal)welding_.S_sele_area_min.D;
                //numericUpDownAreaMax.Value = (decimal)welding_.S_sele_area_max.D;

                numericUpDownVThrMin.Value = welding_.V_threa_min;
                numericUpDownVThrMax.Value = welding_.V_threa_max;

                numericUpDown2.Value = (decimal)welding_.R_threa_min;
                numericUpDown1.Value = (decimal)welding_.R_threa_max;
                numericUpDownHAraeComputeMin.Value = welding_.H_area_compute_thr_min;
                numericUpDownHAraeComputeMax.Value = welding_.H_area_compute_thr_max;
                numericUpDownSAreaComputeThrMin.Value = welding_.S_area_compute_thr_min;
                numericUpDownSAreaComputeThrMax.Value = welding_.S_area_compute_thr_max;
                numericUpDownVAreaComputeThrMin.Value = welding_.V_area_compute_thr_min;
                numericUpDownVAreaComputeThrMax.Value = welding_.V_area_compute_thr_max;
                //numericUpDown2.Value = welding_.R_area_compute_thr_min;
                //numericUpDown1.Value = welding_.R_area_compute_thr_max;
                numericUpDownComputeSeleAreaMin.Value = (decimal)welding_.Compute_sele_area_min;
                numericUpDownComputeSeleAreaMax.Value = (decimal)welding_.Compute_sele_area_max;
                numericUpDownNumber.Value = WeldingCCT.Number;

                numericUpDown9.Value = welding_.NoNeedle_H_thr_min;
                numericUpDown6.Value = welding_.NoNeedle_H_thr_max;
                numericUpDown8.Value = welding_.NoNeedle_S_thr_min;
                numericUpDown7.Value = welding_.NoNeedle_S_thr_max;
                numericUpDown5.Value = welding_.NoNeedle_V_thr_min;
                numericUpDown4.Value = welding_.NoNeedle_V_thr_max;
                numericUpDown15.Value = welding_.R_area_compute_thr_max;
                numericUpDown16.Value = welding_.R_area_compute_thr_min;
                numericUpDown10.Value = (decimal)welding_.NoNeedle_area_min;

                while (WeldingCCT.Number < WeldingCCT.ListHObj.Count)
                {
                    WeldingCCT.ListHObj.RemoveAt(WeldingCCT.ListHObj.Count - 1);
                }
                while (WeldingCCT.Number > WeldingCCT.ListHObj.Count)
                {
                    WeldingCCT.ListHObj.Add(new HObject());
                    WeldingCCT.ListHObj[WeldingCCT.ListHObj.Count - 1].GenEmptyObj();
                }

                if (listBox2.Items.Count > WeldingCCT.ListHObj.Count)
                {
                    listBox2.Items.Clear();
                }

                while (listBox2.Items.Count < WeldingCCT.ListHObj.Count)
                {
                    listBox2.Items.Add(listBox2.Items.Count + 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            isMove = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                HObject hObject = new HObject();
                Halcon.HobjClear();
                List<HObject> hObject2 = new List<HObject>();
                for (int i = 0; i < WeldingCCT.ListHObj.Count; i++)
                {
                    hObject2.Add(new HObject());
                }
                if (WeldingCCT.HObject != null)
                {
                    if (Halcon.Drawing)
                    {
                        return;
                    }
                    Halcon.Drawing = true;
                    try
                    {
                        HTuple homMat2d;
                        HTuple hv_Button = null;
                        HTuple hv_Row = null, hv_Column = null;
                        HOperatorSet.AreaCenter(WeldingCCT.HObject, out HTuple area, out HTuple rows, out HTuple column);
                        this.hWindowControl1.HalconWindow.ClearWindow();
                        HOperatorSet.SetColor(Halcon.hWindowHalcon(), Color.Red.Name.ToLower());
                        Halcon.GetHWindow().Focus();
                        hv_Button = 0;
                        // 4为鼠标右键
                        while (hv_Button != 4)
                        {
                            Halcon.HobjClear();
                            Halcon.AddMeassge("绘制中，单击鼠标右键结束绘制");
                            //一直在循环,需要让halcon控件也响应事件,不然到时候跳出循环,之前的事件会一起爆发触发,
                            Application.DoEvents();
                            try
                            {
                                HTuple Brow = new HTuple();
                                HTuple Bcol = new HTuple();
                                HOperatorSet.GetMposition(Halcon.hWindowHalcon(), out hv_Row, out hv_Column, out hv_Button);
                                hv_Button = 0;
                                HOperatorSet.GetMposition(Halcon.hWindowHalcon(), out hv_Row, out hv_Column, out hv_Button);
                                HOperatorSet.VectorAngleToRigid(rows, column, 0, hv_Row, hv_Column, 0, out homMat2d);
                                HOperatorSet.AffineTransRegion(WeldingCCT.HObject, out hObject, homMat2d, "nearest_neighbor");
                                for (int i = 0; i < WeldingCCT.ListHObj.Count(); i++)
                                {
                                    HOperatorSet.AffineTransRegion(WeldingCCT.ListHObj[i], out HObject hObject2T, homMat2d, "nearest_neighbor");
                                    Halcon.AddObj(hObject2T, ColorResult.blue);
                                    hObject2[i] = hObject2T;
                                }
                                Halcon.AddObj(hObject);
                                HOperatorSet.ReduceDomain(Halcon.Image(), hObject, out HObject hObject1);
                                Halcon.ShowImage();
                                Halcon.ShowObj();
                                HOperatorSet.ReduceDomain(Halcon.Image(), WeldingCCT.HObject, out hObjectImage);
                                if (WeldingCCT.Solder_joint_inspection(welding_, Halcon.GetOneImageR(), out bool iscon, out bool isless, out HTuple areas, hWind))
                                {
                                    Halcon.AddObj(WeldingCCT.HObject);
                                }
                                else
                                {
                                    Halcon.AddObj(WeldingCCT.HObject, ColorResult.red);
                                }
                                for (int i = 0; i < WeldingCCT.ListHObj.Count(); i++)
                                {
                                    Halcon.AddObj(WeldingCCT.ListHObj[i], ColorResult.blue);
                                }
                                if (hv_Button == 4)
                                {
                                    WeldingCCT.ListHObj = hObject2;
                                    WeldingCCT.HObject = hObject;
                                    Halcon.AddObj(hObject);
                                }
                            }
                            catch (HalconException ex)
                            {
                                hv_Button = 0;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    Halcon.HobjClear();
                }
                else
                {
                    Halcon.AddMeassge("未绘制区域，无法调试");
                }
                Halcon.ShowImage();
                Halcon.ShowObj();
            }
            catch (Exception)
            {
            }
            Halcon.Drawing = false;
        }

        private Welding_Spot.WeldingCC WeldingCCT;

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            SetProgram(3);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                WeldingCCT = welding_.listWelding[listBox1.SelectedIndex];
                GetProgram();
                if (tabControl2.SelectedIndex == 2)
                {
                    SetProgram(6);
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SetProgram(3);
                }
                else if (tabControl2.SelectedIndex == 3)
                {
                    SetProgram(9);
                }
                else
                {
                    SetProgram(0);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex > -1)
                {
                    welding_.listWelding.RemoveAt(listBox1.SelectedIndex);
                    listBox1.Items.Clear();
                    for (int i = 0; i < welding_.listWelding.Count; i++)
                    {
                        listBox1.Items.Add(i + 1);
                    }
                    GetProgram();
                }
            }
            catch (Exception)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (WeldingCCT.ListHObj.Count > listBox2.SelectedIndex)
                {
                    if (!WeldingCCT.ListHObj[listBox2.SelectedIndex].IsInitialized())
                    {
                        WeldingCCT.ListHObj[listBox2.SelectedIndex] = new HObject();
                        WeldingCCT.ListHObj[listBox2.SelectedIndex].GenEmptyObj();
                    }
                    HObject hObject = new HObject();
                    if (WeldingCCT.IsEllIpse)
                    {
                        HOperatorSet.GenEllipse(out hObject, 10, 10, new HTuple(WeldingCCT.Phi).TupleRad(), new HTuple(WeldingCCT.CiericROut), WeldingCCT.Radius2);
                    }
                    else
                    {
                        HOperatorSet.GenCircle(out hObject, 10, 10, WeldingCCT.CiericROut);
                    }
                    HOperatorSet.GenCircle(out HObject hObject1, 10, 10, WeldingCCT.CiericRInt);
                    HOperatorSet.Difference(hObject, hObject1, out hObject);
                    Halcon.HobjClear();
                    WeldingCCT.ListHObj[listBox2.SelectedIndex] = RunProgram.DragMoveOBJ(Halcon, hObject);
                }
            }
            catch (Exception)
            {
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Halcon.HobjClear();
                Halcon.AddObj(WeldingCCT.ListHObj[listBox2.SelectedIndex], ColorResult.red);
                Halcon.ShowImage();
                Halcon.ShowObj();
            }
            catch (Exception)
            {
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Halcon.HobjClear();
                if (Halcon.Drawing)
                {
                    return;
                }
                WeldingCCT.HObject = RunProgram.DrawRmoveObj(Halcon, WeldingCCT.HObject);
            }
            catch (Exception)
            {
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Halcon.HobjClear();
                if (Halcon.Drawing)
                {
                    return;
                }

                WeldingCCT.HObject = RunProgram.DrawHObj(Halcon, WeldingCCT.HObject);
            }
            catch (Exception)
            {
            }
        }

        private void numericUpDownSAreaComputeThrMin_ValueChanged(object sender, EventArgs e)
        {
            SetProgram(6);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            SetProgram(6);
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            SetProgram(9);
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox3.SelectedItem == null)
                {
                    return;
                }
                RGBHSVEnum = (ImageTypeObj)Enum.Parse(typeof(ImageTypeObj), listBox3.SelectedItem.ToString());
                if (tabControl2.SelectedIndex == 2)
                {
                    SetProgram(6);
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SetProgram(3);
                }
                else if (tabControl2.SelectedIndex == 3)
                {
                    SetProgram(9);
                }
                else
                {
                    SetProgram(9);
                }
            }
            catch (Exception)
            {
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }
    }
}