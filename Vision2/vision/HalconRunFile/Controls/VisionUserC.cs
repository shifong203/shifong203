using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.RunProgramFile.RunProgram;
using static Vision2.vision.Vision;

namespace Vision2.vision.HalconRunFile.Controls
{
    public interface DrawD
    {
        bool Drawing { get; set; }
        HWindow hWindowHalconID { get; set; }
        bool WhidowAdd { get; set; }
    }
    public class VisionUserC : HWindowControl
    {
        public class HWindwC : IDrawHalcon
        {
            List<HObject> hObjects = new List<HObject>();
            public Dictionary<string, HalconRun.ObjectColor> keyValuePairs = new Dictionary<string, HalconRun.ObjectColor>();
            public HObject Image(HObject image = null)
            {
                if (image != null)
                {
                    iamget = image;
                }
                return iamget;
            }
            HObject iamget;
            public HTuple magessage = new HTuple();
            public HTuple hWindowHalconID { get; set; }
            public HTuple Width;
            public HTuple Height;

            public string ShowName = "";

            public bool Drawing { get; set; }
            public bool WhidowAdd { get; set; }
            public bool AddMode { get; set; }
            public bool FillMode { get; set; }
            public void Focus()
            {

            }


            public bool Keys;
            public void ShowFill()
            {

            }
            public void ShowOBJ(int rowi = 0, int coli = 0)
            {
                try
                {
                    HObject = new HObject();
                    HObject.GenEmptyObj();
                    HSystem.SetSystem("flush_graphic", "false");
                    HOperatorSet.ClearWindow(this.hWindowHalconID);
                    if (Vision.ObjectValided(this.Image()))
                    {
                        if (Width == null || Width < 0 && WhidowAdd)
                        {
                            HOperatorSet.GetImageSize(this.Image(), out HTuple width, out HTuple heigth);
                            this.Width = width;
                            this.Height = heigth;
                        }
                        HOperatorSet.DispObj(this.Image(), this.hWindowHalconID);
                    }

                    if (FillMode)
                    {
                        HOperatorSet.SetDraw(hWindowHalconID, "fill");
                    }
                    else
                    {
                        HOperatorSet.SetDraw(hWindowHalconID, "margin");
                    }
                    if (ShowName == "")
                    {
                        foreach (var item in keyValuePairs)
                        {
                            HOperatorSet.SetColor(hWindowHalconID, item.Value.HobjectColot);

                            if (item.Value._HObject != null)
                            {
                                if (item.Value.IsShow)
                                {
                                    SelectOBJ(item.Value._HObject, hWindowHalconID, rowi, coli, Keys);
                                }
                            }
                        }
                        for (int i = 0; i < hObjects.Count; i++)
                        {
                            SelectOBJ(hObjects[i], hWindowHalconID, rowi, coli, Keys);
                        }
                        if (Keys)
                        {
                            SelesShoOBJ(hWindowHalconID);
                        }
                    }
                    else
                    {
                        if (keyValuePairs.ContainsKey(ShowName))
                        {
                            HOperatorSet.SetColor(hWindowHalconID, keyValuePairs[ShowName].HobjectColot);
                            SelectOBJ(keyValuePairs[ShowName]._HObject, hWindowHalconID, rowi, coli, Keys);
                        }
                    }
                    HSystem.SetSystem("flush_graphic", "true");
                    HOperatorSet.DispObj(HObject, this.hWindowHalconID);
                }
                catch (Exception)
                {
                }



            }
            void SelectOBJ(HObject hObject, HTuple hWindowHalconID, int rowi, int coli, bool ismove)
            {
                HTuple intd = new HTuple();
                if (ismove)
                {
                    try
                    {
                        HOperatorSet.GetObjClass(hObject, out HTuple classv);
                        if (classv.Length == 0)
                        {
                            return;
                        }
                        if (classv[0] == "region")
                        {
                            HOperatorSet.GetRegionIndex(hObject, rowi, coli, out intd);
                            //HOperatorSet.TestRegionPoint(hObject, rowi, coli, out intd);//test_region_point
                            HOperatorSet.SelectRegionPoint(hObject, out HObject hObject1, rowi, coli);//test_region_point
                            if (hObject1.CountObj() >= 1)
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    if (intd >= 0)
                    {
                        if (HObject == null)
                        {
                            HObject = new HObject();
                            HObject.GenEmptyObj();
                        }
                        HObject = HObject.ConcatObj(hObject.SelectObj(intd));
                        if (hObject.CountObj() != 1)
                        {
                            HOperatorSet.DispObj(hObject.RemoveObj(intd), hWindowHalconID);
                        }
                        return;
                    }
                }
                HOperatorSet.DispObj(hObject, hWindowHalconID);
            }

            HObject HObject;
            public void SelesShoOBJ(HTuple hWindowHalconID)
            {
                HOperatorSet.AreaCenter(HObject, out HTuple ar, out HTuple row, out HTuple colum);
                HOperatorSet.HeightWidthRatio(HObject, out HTuple height, out HTuple width, out HTuple ratio);
                HOperatorSet.Circularity(HObject, out HTuple circularity);
                HOperatorSet.Compactness(HObject, out HTuple compactness);
                HOperatorSet.Convexity(HObject, out HTuple convexity);
                HOperatorSet.Rectangularity(HObject, out HTuple Rectangularity);
                HTuple hTuple = "X" + row + " Y" + colum + " 面积:" + ar + "高" + height + "宽" + width + "比例" + ratio + "圆度" + circularity
               + Environment.NewLine + "紧密度" + compactness + "凸面" + convexity + "长方形" + Rectangularity;


                HOperatorSet.SetColor(hWindowHalconID, "yellow");
                HOperatorSet.SetDraw(hWindowHalconID, "fill");
                HOperatorSet.DispObj(HObject, hWindowHalconID);
                HOperatorSet.SetColor(hWindowHalconID, "red");
                HOperatorSet.GenCrossContourXld(out HObject cross, row, colum, 20, 0);
                HOperatorSet.DispObj(cross, hWindowHalconID);
                HOperatorSet.SetDraw(hWindowHalconID, "margin");
                Vision.Disp_message(hWindowHalconID, hTuple, 100, 20, true, "red");
            }
            public void AddListObj(HObject hObject)
            {
                hObjects.Add(hObject);
            }
            public void AddKeyObj(string name, HalconRun.ObjectColor hObject)
            {
                if (!keyValuePairs.ContainsKey(name))
                {
                    keyValuePairs.Add(name, hObject);
                }
                keyValuePairs[name] = hObject;

            }
            public void AddKeyObj(string name, HObject hObject, ColorResult colorResult = ColorResult.red)
            {
                AddKeyObj(name, new HalconRun.ObjectColor() { _HObject = hObject, HobjectColot = colorResult.ToString() });
            }

            public void ClearObj()
            {
                hObjects.Clear();
                keyValuePairs.Clear();
            }
        }

        private HTuple m_ImageRow1, m_ImageCol1;
        private double stratX;
        private double stratY;
        private HTuple H_Scale = 0.2; //缩放步长
        private HTuple MaxScale = 10000;//最大放大系数
        private HTuple ptX, ptY;
        private HTuple m_ImageRow0, m_ImageCol0;
        private HTuple hv_Button;
        bool meuseBool;

        private HTuple Row0_1, Col0_1, Row1_1, Col1_1;

        public HWindwC hWindwC = new HWindwC();
        private void VisionUserControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            try
            {
                if (hWindwC.Drawing || hWindwC.WhidowAdd)
                {
                    return;
                }
                if (e.Button == MouseButtons.Left)
                {
                    this.OnClick(e);
                }
                if (e.Button == MouseButtons.Left)
                {
                    stratX = e.X;
                    stratY = e.Y;
                    meuseBool = true;
                    Cursor = Cursors.SizeAll;
                    System.Drawing.Rectangle rect = this.ImagePart;
                }
                if (e.Clicks == 2)
                {
                    HOperatorSet.SetPart(this.HalconWindow, 0, 0, hWindwC.Height - 1, hWindwC.Width - 1);
                }

            }
            catch (Exception)
            {

            }

        }

        private void VisionUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (hWindwC != null)
                {
                    hWindwC.hWindowHalconID = this.HalconWindow;
                }
                //HOperatorSet.SetDraw(this.HalconWindow, "margin");
                //HOperatorSet.SetColored(this.HalconWindow, 12);

            }
            catch (Exception)
            {
            }
        }

        private void VisionUserC_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void VisionUserC_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void VisionUserControl_HMouseUp(object sender, HMouseEventArgs e)
        {
            try
            {

                if (hWindwC.Drawing || hWindwC.WhidowAdd)
                {
                    return;
                }
            }
            catch (Exception)
            {

            }
            meuseBool = false;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // VisionUserC
            // 
            this.Name = "VisionUserC";
            this.Size = new System.Drawing.Size(439, 360);
            this.WindowSize = new System.Drawing.Size(439, 360);
            this.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseMove);
            this.HMouseDown += new HalconDotNet.HMouseEventHandler(this.VisionUserControl_HMouseDown);
            this.HMouseUp += new HalconDotNet.HMouseEventHandler(this.VisionUserControl_HMouseUp);
            this.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseWheel);
            this.Load += new System.EventHandler(this.VisionUserControl_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VisionUserC_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.VisionUserC_KeyUp);
            this.ResumeLayout(false);

        }

        public VisionUserC()
        {
            InitializeComponent();

            //hWindwC.hWindowHalcon() = this.HalconWindow;
        }

        public void UpHalcon(HWindwC hWindw = null)
        {
            if (hWindw != null)
            {
                this.hWindwC = hWindw;
            }

            try
            {
                HOperatorSet.GetImageSize(hWindwC.Image(), out hWindwC.Width, out hWindwC.Height);
                if (hWindwC.Width.Length == 1)
                {
                    HOperatorSet.SetPart(this.HalconWindow, 0, 0, hWindwC.Height - 1, hWindwC.Width - 1);
                }
            }
            catch (Exception)
            {
            }
            hWindwC.hWindowHalconID = this.HalconWindow;
            hWindwC.ShowOBJ();
        }

        /// <summary>
        /// 获得图像值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {

                if (hWindwC == null)
                {
                    return;
                }
                if (hWindwC.Drawing || hWindwC.WhidowAdd)
                {
                    return;
                }
                if (meuseBool)
                {
                    double motionX, motionY;
                    motionX = ((e.X - stratX));
                    motionY = ((e.Y - stratY));
                    if (((int)motionX != 0) || ((int)motionY != 0))
                    {
                        if (m_ImageRow1 == null)
                        {
                            m_ImageRow1 = hWindwC.Width;
                            m_ImageCol1 = hWindwC.Height;
                        }
                        System.Drawing.Rectangle rect2 = this.ImagePart;
                        HTuple row = rect2.Y + -motionY;
                        HTuple colum = rect2.X + -motionX;
                        rect2.X = (int)Math.Round(colum.D);
                        rect2.Y = (int)Math.Round(row.D);
                        ImagePart = rect2;

                        stratX = e.X - motionX;
                        stratY = e.Y - motionY;
                    }
                }

                this.HalconWindow.GetMposition(out int rowi, out int coli, out int button1);
                hWindwC.ShowOBJ(rowi, coli);
                HOperatorSet.GetGrayval(hWindwC.Image(), rowi, coli, out HTuple Grey);
                if (Grey.Length == 3)
                {
                    Vision.Disp_message(HalconWindow, "X" + rowi + " Y" + coli + " RGB:" + Grey[0] + "," + Grey[1] + "," + Grey[2], 20, 20, true);
                }
                else if (Grey.Length == 1)
                {
                    Vision.Disp_message(HalconWindow, "X" + rowi + " Y" + coli + " B:" + Grey[0], 20, 20, true);
                }


            }
            catch
            {
            }
        }
        /// <summary>
        /// 图像缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl1_HMouseWheel(object sender, HalconDotNet.HMouseEventArgs e)
        {
            try
            {
                if (hWindwC == null)
                {
                    return;
                }
                if (hWindwC.Drawing || hWindwC.WhidowAdd)
                {
                    return;
                }
                System.Drawing.Rectangle rect2 = ImagePart;
                m_ImageCol0 = rect2.X;
                m_ImageRow0 = rect2.Y;
                m_ImageCol1 = rect2.X + rect2.Width;
                m_ImageRow1 = rect2.Y + rect2.Height;

                HOperatorSet.GetMposition(this.HalconWindow, out ptY, out ptX, out hv_Button);

                if (m_ImageRow1 == null)
                {
                    m_ImageRow1 = hWindwC.Width;
                    m_ImageCol1 = hWindwC.Height;
                }
                //向上滑动滚轮，图像缩小。以当前鼠标的坐标为支点进行缩小或放大
                if (e.Delta > 0)
                {
                    Cursor = Cursors.PanSouth;
                    //重新计算缩小后的图像区域
                    Row0_1 = ptY - 1 / (1 - H_Scale) * (ptY - m_ImageRow0);
                    Row1_1 = ptY - 1 / (1 - H_Scale) * (ptY - m_ImageRow1);
                    Col0_1 = ptX - 1 / (1 - H_Scale) * (ptX - m_ImageCol0);
                    Col1_1 = ptX - 1 / (1 - H_Scale) * (ptX - m_ImageCol1);

                    //限定缩小范围
                    if ((Col1_1 - Col0_1).TupleAbs() / hWindwC.Width <= 100)
                    {
                        //设置在图形窗口中显示局部图像
                        m_ImageRow0 = Row0_1;
                        m_ImageCol0 = Col0_1;
                        m_ImageRow1 = Row1_1;
                        m_ImageCol1 = Col1_1;
                    }
                }
                else
                {
                    Cursor = Cursors.PanNorth;
                    //重新计算放大后的图像区域
                    Row0_1 = ptY - 1 / (1 + H_Scale) * (ptY - m_ImageRow0);
                    Row1_1 = ptY - 1 / (1 + H_Scale) * (ptY - m_ImageRow1);
                    Col0_1 = ptX - 1 / (1 + H_Scale) * (ptX - m_ImageCol0);
                    Col1_1 = ptX - 1 / (1 + H_Scale) * (ptX - m_ImageCol1);
                    //限定放大范围
                    HTuple dw = (hWindwC.Width / (Col1_1 - Col0_1).TupleAbs());
                    if ((hWindwC.Width / (Col1_1 - Col0_1).TupleAbs()) <= MaxScale)
                    {
                        //设置在图形窗口中显示局部图像
                        m_ImageRow0 = Row0_1;
                        m_ImageCol0 = Col0_1;
                        m_ImageRow1 = Row1_1;
                        m_ImageCol1 = Col1_1;
                    }
                }

                HOperatorSet.SetPart(this.HalconWindow, m_ImageRow0, m_ImageCol0, m_ImageRow1, m_ImageCol1);

                hWindwC.ShowOBJ();
            }
            catch (Exception es)
            {

            }

        }
    }
}
