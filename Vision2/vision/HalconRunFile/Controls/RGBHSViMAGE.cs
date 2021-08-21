using HalconDotNet;
using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class RGBHSViMAGE : UserControl
    {
        public RGBHSViMAGE()
        {
            InitializeComponent();
        }

        private HObject Image;

        public void SetHalcon(HalconRun halconRun)
        {
            visionUserC1.hWindwC.Image(halconRun.Image());

            Image = halconRun.Image();

            HOperatorSet.CountChannels(visionUserC1.hWindwC.Image(), out HTuple htcon);
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            if (htcon != 3)
            {
                listBox1.Items.AddRange(new string[] { "黑白" });
            }
            else
            {
                if (htcon == 3)
                {
                    listBox1.Items.AddRange(hsv);

                    HOperatorSet.Decompose3(visionUserC1.hWindwC.Image(), out R, out G, out B);
                    HOperatorSet.TransFromRgb(R, G, B, out H, out S, out V, "hsv");
                }
            }
        }

        private HObject R, G, B, H, S, V;

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                visionUserC1.hWindwC.ShowName = listBox2.SelectedItem.ToString();
            }
            catch (Exception)
            {
            }
        }

        private string[] hsv = new string[] { "Image", "黑白", "R", "G", "B", "H", "S", "V" };

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            visionUserC1.hWindwC.FillMode = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            visionUserC1.hWindwC.AddMode = checkBox2.Checked;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HSystem.SetSystem("flush_graphic", "false");
                HOperatorSet.ClearWindow(visionUserC1.HalconWindow);
                HSystem.SetSystem("flush_graphic", "true");
                switch (listBox1.SelectedItem.ToString())
                {
                    case "RGB":
                    case "Image":
                        visionUserC1.hWindwC.Image(Image);
                        break;

                    case "R":
                        visionUserC1.hWindwC.Image(R);

                        break;

                    case "G":
                        visionUserC1.hWindwC.Image(G);
                        break;

                    case "B":
                        visionUserC1.hWindwC.Image(B);
                        break;

                    case "H":
                        visionUserC1.hWindwC.Image(H);
                        break;

                    case "S":
                        visionUserC1.hWindwC.Image(S);
                        break;

                    case "V":
                        visionUserC1.hWindwC.Image(V);
                        break;

                    default:
                        break;
                }
                visionUserC1.hWindwC.ShowOBJ();
            }
            catch (Exception)
            {
            }
        }
    }
}