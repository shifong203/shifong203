using HalconDotNet;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class HsvUserControl : UserControl
    {
        public HsvUserControl()
        {
            InitializeComponent();
        }

        public void SetHOBJE(HObject RhObject)
        {
            HOperatorSet.Decompose3(RhObject, out HObject Image1, out HObject Image2, out HObject Image3);
            HOperatorSet.TransFromRgb(Image1, Image2, Image3, out HObject H, out HObject S, out HObject V, "hsv");

            HOperatorSet.SmallestRectangle1(RhObject, out HTuple row1, out HTuple column1, out HTuple row2, out HTuple column2);
            HOperatorSet.ClearWindow(visionUserControl1.HalconWindow);
            HOperatorSet.SetDraw(visionUserControl1.HalconWindow, "margin");
            HOperatorSet.SetColor(visionUserControl1.HalconWindow, "#ff000040");
            HOperatorSet.SetPart(visionUserControl1.HalconWindow, row1, column1, row2, column2);

            HOperatorSet.ClearWindow(visionUserControl2.HalconWindow);
            HOperatorSet.SetDraw(visionUserControl2.HalconWindow, "margin");
            HOperatorSet.SetColor(visionUserControl2.HalconWindow, "#ff000040");
            HOperatorSet.SetPart(visionUserControl2.HalconWindow, row1, column1, row2, column2);

            HOperatorSet.ClearWindow(visionUserControl3.HalconWindow);
            HOperatorSet.SetDraw(visionUserControl3.HalconWindow, "margin");
            HOperatorSet.SetColor(visionUserControl3.HalconWindow, "#ff000040");
            HOperatorSet.SetPart(visionUserControl3.HalconWindow, row1, column1, row2, column2);
            HOperatorSet.ClearWindow(visionUserControl4.HalconWindow);
            HOperatorSet.SetDraw(visionUserControl4.HalconWindow, "margin");
            HOperatorSet.SetColor(visionUserControl4.HalconWindow, "#ff000040");
            HOperatorSet.SetPart(visionUserControl4.HalconWindow, row1, column1, row2, column2);
            HOperatorSet.ClearWindow(visionUserControl5.HalconWindow);
            HOperatorSet.SetDraw(visionUserControl5.HalconWindow, "margin");
            HOperatorSet.SetColor(visionUserControl5.HalconWindow, "#ff000040");
            HOperatorSet.SetPart(visionUserControl5.HalconWindow, row1, column1, row2, column2);

            HOperatorSet.ClearWindow(visionUserControl6.HalconWindow);
            HOperatorSet.SetDraw(visionUserControl6.HalconWindow, "margin");
            HOperatorSet.SetColor(visionUserControl6.HalconWindow, "#ff000040");
            HOperatorSet.SetPart(visionUserControl6.HalconWindow, row1, column1, row2, column2);
            HOperatorSet.DispObj(Image1, visionUserControl1.HalconWindow);
            HOperatorSet.DispObj(Image2, visionUserControl2.HalconWindow);
            HOperatorSet.DispObj(Image3, visionUserControl3.HalconWindow);
            HOperatorSet.DispObj(H, visionUserControl4.HalconWindow);
            HOperatorSet.DispObj(S, visionUserControl5.HalconWindow);
            HOperatorSet.DispObj(V, visionUserControl6.HalconWindow);
        }
    }
}