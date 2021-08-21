using HalconDotNet;
using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    public class VisionUserControl : HWindowControl
    {
        ////public List<string> ListImagePath = new List<string>();

        public HTuple m_ImageRow1, m_ImageCol1;
        private double stratX;
        private double stratY;
        private HTuple H_Scale = 0.2; //缩放步长
        private HTuple MaxScale = 10000;//最大放大系数
        private HTuple ptX, ptY;
        private HTuple m_ImageRow0, m_ImageCol0;
        private HTuple hv_Button;
        public bool meuseBool;
        private HalconRun halcon;
        private HTuple Row0_1, Col0_1, Row1_1, Col1_1;

        private void VisionUserControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            try
            {
                if (halcon == null)
                {
                    return;
                }
                if (halcon.Drawing || halcon.WhidowAdd)
                {
                    return;
                }

                if (e.Button == MouseButtons.Left)
                {
                    halcon.hWindowHalcon(this.HalconWindow);
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
            }
            catch (Exception)
            {
            }
        }

        private void VisionUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.SetDraw(this.HalconWindow, "margin");
            }
            catch (Exception)
            {
            }
        }

        private void VisionUserControl_HMouseUp(object sender, HMouseEventArgs e)
        {
            try
            {
                if (halcon == null)
                {
                    return;
                }
                if (halcon.Drawing || halcon.WhidowAdd)
                {
                    return;
                }
            }
            catch (Exception)
            {
            }
            halcon.GetOneImageR().IsMoveBool = false;
            meuseBool = false;
            halcon.ShowObj();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // VisionUserControl
            //
            this.Name = "VisionUserControl";
            this.Size = new System.Drawing.Size(439, 360);
            this.WindowSize = new System.Drawing.Size(439, 360);
            this.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseMove);
            this.HMouseDown += new HalconDotNet.HMouseEventHandler(this.VisionUserControl_HMouseDown);
            this.HMouseUp += new HalconDotNet.HMouseEventHandler(this.VisionUserControl_HMouseUp);
            this.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseWheel);
            this.Load += new System.EventHandler(this.VisionUserControl_Load);
            this.ResumeLayout(false);
        }

        public VisionUserControl()
        {
            InitializeComponent();
        }

        public void UpHalcon(HalconRun halconRun)
        {
            halcon = halconRun;

            HOperatorSet.SetDraw(this.HalconWindow, "margin");

            halcon.hWindowHalcon(this.HalconWindow);
            halcon.ShowImage();
            HOperatorSet.SetPart(this.HalconWindow, 0, 0, halcon.Height, halcon.Width);
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
                if (halcon == null)
                {
                    return;
                }
                if (halcon.Drawing || halcon.WhidowAdd)
                {
                    return;
                }
                if (meuseBool)
                {
                    halcon.GetOneImageR().IsMoveBool = true;
                    double motionX, motionY;
                    motionX = ((e.X - stratX));
                    motionY = ((e.Y - stratY));
                    if (((int)motionX != 0) || ((int)motionY != 0))
                    {
                        if (m_ImageRow1 == null)
                        {
                            m_ImageRow1 = halcon.Width;
                            m_ImageCol1 = halcon.Height;
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
                    halcon.ShowObj();
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
                if (halcon == null)
                {
                    return;
                }
                if (halcon.Drawing || halcon.WhidowAdd)
                {
                    return;
                }
                System.Drawing.Rectangle rect2 = ImagePart;
                m_ImageCol0 = rect2.X;
                m_ImageRow0 = rect2.Y;
                m_ImageCol1 = rect2.X + rect2.Width;
                m_ImageRow1 = rect2.Y + rect2.Height;
                ptY = (int)e.Y;
                ptX = (int)e.X;
                //HOperatorSet.GetMposition(this.HalconWindow, out ptY, out ptX, out hv_Button);

                if (m_ImageRow1 == null)
                {
                    m_ImageRow1 = halcon.Width;
                    m_ImageCol1 = halcon.Height;
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
                    if ((Col1_1 - Col0_1).TupleAbs() / halcon.Width <= 100)
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
                    HTuple dw = (halcon.Width / (Col1_1 - Col0_1).TupleAbs());
                    if ((halcon.Width / (Col1_1 - Col0_1).TupleAbs()) <= MaxScale)
                    {
                        //设置在图形窗口中显示局部图像
                        m_ImageRow0 = Row0_1;
                        m_ImageCol0 = Col0_1;
                        m_ImageRow1 = Row1_1;
                        m_ImageCol1 = Col1_1;
                    }
                }

                HOperatorSet.SetPart(this.HalconWindow, m_ImageRow0, m_ImageCol0, m_ImageRow1, m_ImageCol1);

                halcon.ShowObj();
            }
            catch (Exception es)
            {
            }
        }
    }
}