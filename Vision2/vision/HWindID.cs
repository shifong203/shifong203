using HalconDotNet;
using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;
namespace Vision2.vision
{
    public class HWindID :IDrawHalcon
    {
        public HWindID()
        {
            hObject = new HObject();
            hObject.GenEmptyObj();
        }
        public static void DispImage(HTuple hv_WindowHandle, HObject iamge)
        {
            try
            {
                if (iamge.IsInitialized() || iamge.CountObj() >= 1)
                {
                    HOperatorSet.GetImageSize(iamge, out HTuple width, out HTuple height);
                    HOperatorSet.SetPart(hv_WindowHandle, 0, 0, height, width);
                    HOperatorSet.DispImage(iamge, hv_WindowHandle);
                }
            }
            catch (Exception)
            {
            }
        }

        private HTuple Row0_1, Col0_1, Row1_1, Col1_1;
        public int TrayNumberID;
        public int TrayNumberCol;
        public int TrayNumberRow;
        private HTuple m_ImageRow1, m_ImageCol1;
        private double stratX;
        private double stratY;
        private HTuple H_Scale = 0.2; //缩放步长
        private HTuple MaxScale = 10000;//最大放大系数
        private HTuple ptX, ptY;
        private HTuple m_ImageRow0, m_ImageCol0;
        private HTuple hv_Button;
        bool meuseBool;
        bool WhidowAdd;
        public double WidthImage = 2000;
        public double HeigthImage = 2000;
        //public HObject image;
        public string Mesgage;

        public HWindowControl GetHWindowControl()
        {
            return hWindowControl1;
        }

        public void Initialize(HWindowControl hWindowControl)
        {
            hWindowControl1 = hWindowControl;
            try
            {
                HOperatorSet.SetDraw(hWindowControl.HalconWindow, "margin");
                HOperatorSet.SetLineWidth(hWindowControl1.HalconWindow, Vision.Instance.LineWidth);
                //HOperatorSet.QueryFont(hWindowControl1.HalconWindow, out HTuple font);
                Vision.SetFont(hWindowControl1.HalconWindow);

            }
            catch (Exception)
            {
            }
            hWindowControl.HMouseUp += hWindowControl1_HMouseUp;
            hWindowControl1.HMouseDown += hWindowControl1_MouseDown;
            hWindowControl1.HMouseWheel += hWindowControl2_HMouseWheel;
            hWindowControl1.HMouseMove += hWindowControl4_HMouseMove;
            hWindowControl1.KeyDown += HWindowControl1_KeyDown; ;
            hWindowControl1.KeyUp += HWindowControl1_KeyUp;
        }

        public void SetDraw(bool isMargin)
        {
            try
            {
                if (!isMargin)
                {
                    HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
                }
                else
                {
                    HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "fill");
                }
            }
            catch (Exception)
            {
            }
        }
        public void SetPart(HTuple rowStrat,HTuple colStrat,HTuple rowEnd,HTuple colEnd)
        {
            try
            {
                HOperatorSet.SetPart(hWindowControl1.HalconWindow, rowStrat, colStrat, rowEnd, colEnd);
            }
            catch (Exception)
            {
            }
        }
        public void SetPerpetualPart(HTuple rowStrat, HTuple colStrat, HTuple rowEnd, HTuple colEnd)
        {
            try
            {
                ImageRowStrat = rowStrat;
                ImageColStrat = colStrat;
                HeigthImage = rowEnd;
                WidthImage = colEnd;
                HOperatorSet.SetPart(hWindowControl1.HalconWindow, ImageRowStrat, ImageColStrat, HeigthImage, WidthImage);
            }
            catch (Exception)
            {
            }
        }
        private void HWindowControl1_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.ControlKey)
            {
                WhidowAdd = false;
                this.OneResIamge.IsXLDOrImage = false;
            }
            else if (e.KeyCode == Keys.ShiftKey)
            {
                this.OneResIamge.IsXLDOrImage = false;
            }
        }

        private void HWindowControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                WhidowAdd = true;
            }
            else if (e.Shift)
            {
                this.OneResIamge.IsXLDOrImage = true;
            }
            if (e.Control&&e.KeyCode==Keys.Q)
            {
                try
                {
                    foreach (var item in OneResIamge.GetKeyHobj())
                    {
                        HOperatorSet.AreaCenter(item.Value.Object, out HTuple area, out HTuple row, out HTuple col);
                        HOperatorSet.DispText(this.hWindowControl1.HalconWindow, 
                           item.Key,"image", row, col , "black", new HTuple(), new HTuple());
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        HWindowControl hWindowControl1;
        private void hWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {
            meuseBool = false;
        }
        private void hWindowControl1_MouseDown(object sender, HMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    HOperatorSet.SetPart(hWindowControl1.HalconWindow, ImageRowStrat, ImageColStrat, HeigthImage, WidthImage);
                    ShowImage();
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (!this.Drawing)
                    {
                        foreach (var item in OneResIamge.GetKeyHobj())
                        {
                            HOperatorSet.GetRegionIndex(item.Value.Object, (int)e.Y, (int)e.X, out HTuple index);
                            if (index > 0)
                            {
                                RunProgram.DragMoveOBJS(this, OneResIamge.GetKeyHobj());
                                this.ShowObj();
                                break;
                            }
                        }
                    }
                }
                //if (!WhidowAdd)
                //{
                //    return;
                //}
                if (!this.Drawing)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        stratX = e.X;
                        stratY = e.Y;
                        meuseBool = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }


        private void hWindowControl4_HMouseMove(object sender, HMouseEventArgs e)
        {

            try
            {
                if (!WhidowAdd)
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
                            m_ImageRow1 = WidthImage;
                            m_ImageCol1 = HeigthImage;
                        }
                        System.Drawing.Rectangle rect2 = hWindowControl1.ImagePart;
                        HTuple row = rect2.Y + -motionY;
                        HTuple colum = rect2.X + -motionX;
                        rect2.X = (int)Math.Round(colum.D);
                        rect2.Y = (int)Math.Round(row.D);
                        hWindowControl1.ImagePart = rect2;
                        stratX = e.X - motionX;
                        stratY = e.Y - motionY;
                    }

                }
                ShowImage();
                string data = "C:" + e.X.ToString("0.0") + "R:" + e.Y.ToString("0.0");
                if (Vision.GetRunNameVision()!=null)
                {
                    Vision.GetRunNameVision().GetCalib().GetPointRctoXY(e.Y, e.X, out HTuple ys, out HTuple xs);
                    Vision.Disp_message(hWindowControl1.HalconWindow, "X:" + xs.TupleString("0.02f") + "Y:" + ys.TupleString("0.02f"), 5, hWindowControl1.Width / 4, true, "red", "false");
                }
                try
                {
                    HOperatorSet.GetGrayval(OneResIamge.Image, e.Y, e.X, out HTuple Grey);
                    if (Grey.Length == 3)
                    {
                        data += " RGB:" + Grey.TupleSelect(0).D.ToString("000") + "," + Grey.TupleSelect(1).D.ToString("000") + "," + Grey.TupleSelect(2).D.ToString("000");
                    }
                    else if (Grey.Length == 1)
                    {
                        data+= "B:" + Grey.D.ToString("000");
                    }
                }
                catch (Exception)
                {
                }
             
                Vision.Disp_message(hWindowControl1.HalconWindow, data, hWindowControl1.Height - 25, hWindowControl1.Width / 4, true, "red", "false");
            }
            catch (Exception)
            {
            }
        }
        private void hWindowControl2_HMouseWheel(object sender, HalconDotNet.HMouseEventArgs e)
        {
            try
            {
                if (!WhidowAdd)
                {
                    return;
                }
                System.Drawing.Rectangle rect2 = hWindowControl1.ImagePart;
                m_ImageCol0 = rect2.X;
                m_ImageRow0 = rect2.Y;
                m_ImageCol1 = rect2.X + rect2.Width;
                m_ImageRow1 = rect2.Y + rect2.Height;
                HOperatorSet.GetMposition(hWindowControl1.HalconWindow, out ptY, out ptX, out hv_Button);
                if (m_ImageRow1 == null)
                {
                    m_ImageRow1 = WidthImage;
                    m_ImageCol1 = HeigthImage;
                }
                //向上滑动滚轮，图像缩小。以当前鼠标的坐标为支点进行缩小或放大
                if (e.Delta > 0)
                {

                    //重新计算缩小后的图像区域
                    Row0_1 = ptY - 1 / (1 - H_Scale) * (ptY - m_ImageRow0);
                    Row1_1 = ptY - 1 / (1 - H_Scale) * (ptY - m_ImageRow1);
                    Col0_1 = ptX - 1 / (1 - H_Scale) * (ptX - m_ImageCol0);
                    Col1_1 = ptX - 1 / (1 - H_Scale) * (ptX - m_ImageCol1);

                    //限定缩小范围
                    if ((Col1_1 - Col0_1).TupleAbs() / WidthImage <= 100)
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
                    //重新计算放大后的图像区域
                    Row0_1 = ptY - 1 / (1 + H_Scale) * (ptY - m_ImageRow0);
                    Row1_1 = ptY - 1 / (1 + H_Scale) * (ptY - m_ImageRow1);
                    Col0_1 = ptX - 1 / (1 + H_Scale) * (ptX - m_ImageCol0);
                    Col1_1 = ptX - 1 / (1 + H_Scale) * (ptX - m_ImageCol1);
                    //限定放大范围
                    HTuple dw = (WidthImage / (Col1_1 - Col0_1).TupleAbs());
                    if ((WidthImage / (Col1_1 - Col0_1).TupleAbs()) <= MaxScale)
                    {
                        //设置在图形窗口中显示局部图像
                        m_ImageRow0 = Row0_1;
                        m_ImageCol0 = Col0_1;
                        m_ImageRow1 = Row1_1;
                        m_ImageCol1 = Col1_1;
                    }
                }
                HOperatorSet.SetPart(hWindowControl1.HalconWindow, m_ImageRow0, m_ImageCol0, m_ImageRow1, m_ImageCol1);
                ShowImage();
            }
            catch (Exception es)
            {
            }
        }
        public int ImageRowStrat = 0;
        public int ImageColStrat = 0;

        public void SetImaage(HObject imaget)
        {
            try
            {
                if (OneResIamge == null)
                {
                    OneResIamge = new OneResultOBj();
                }
                OneResIamge.Image = imaget;
                HOperatorSet.GetImageSize(OneResIamge.Image, out HTuple wi, out HTuple heit);
                if (wi.Length != 0)
                {
                    WidthImage = wi;
                    HeigthImage = heit;
                }
                hWindowControl1.HalconWindow.SetPart(ImageRowStrat, ImageColStrat, HeigthImage, WidthImage);
                hWindowControl1.HalconWindow.DispObj(OneResIamge.Image);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public OneResultOBj OneResIamge = new OneResultOBj();

        HObject hObject;

        public bool Drawing { get ; set ; }
        public int DrawType { get ; set ; }
        public bool DrawErasure { get ; set ; }

        public void ShowImage()
        {
            try
            {
                OneResIamge.ShowAll(hWindowControl1.HalconWindow);
            }
            catch (Exception)
            {
            }
        }
        public void ShowObj()
        {
            try
            {
                OneResIamge.ShowAll(hWindowControl1.HalconWindow);

            }
            catch (Exception)
            {
            }
        }
        public void HobjClear()
        {
            OneResIamge.ClearAllObj();
        }

        public void Focus()
        {
            try
            {
                hWindowControl1.Focus();
            }
            catch (Exception)
            {
            }
        }

        public HTuple hWindowHalcon(HTuple hawid = null)
        {
           return this.hWindowControl1.HalconWindow;
        }

        public HObject Image(HObject hObject = null)
        {
            if (hObject!=null)
            {
                OneResIamge.Image = hObject;
            }
            if (OneResIamge==null)
            {
                OneResIamge = new OneResultOBj();
            }
          return     OneResIamge.Image;
        }

    

        public void AddMeassge(HTuple text)
        {
            OneResIamge.AddMeassge(text);
        }

        public void AddObj(HObject hObject,ColorResult colorResult = ColorResult.green)
        {
            OneResIamge.AddObj(hObject, colorResult);
        }
    }
}
