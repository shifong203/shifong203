using HalconDotNet;
using System;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class PretreatmentUserControl : UserControl
    {
        public PretreatmentUserControl()
        {
            InitializeComponent();
        }

        public void SetData(RunProgramFile.RunProgram run, HWindID userC)
        {
            RunProgram = run;
            visionUserC1 = userC;
            isCheave = true;
            checkBox4.Checked = RunProgram.IsImage_range;
            checkBox3.Checked = RunProgram.IsEmphasize;
            numericUpDown1.Value = (decimal)RunProgram.Emphasizefactor;
            numericUpDown2.Value = (decimal)RunProgram.EmphasizeH;
            numericUpDown3.Value = (decimal)RunProgram.EmphasizeW;
            trackBar1.Value = (int)numericUpDown1.Value;
            trackBar2.Value = (int)numericUpDown2.Value;
            trackBar3.Value = (int)numericUpDown3.Value;
            numericUpDown11.Value = (decimal)RunProgram.SeleImageRangeMin;
            numericUpDown12.Value = (decimal)RunProgram.SeleImageRangeMax;
            trackBar5.Value = (int)numericUpDown11.Value;
            trackBar4.Value = (int)numericUpDown12.Value;
            isCheave = false;
        }

        private RunProgramFile.RunProgram RunProgram;
        private bool isCheave;

        private HWindID visionUserC1;

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (isCheave)
                {
                    return;
                }
                RunProgram.IsEmphasize = checkBox3.Checked;
                RunProgram.IsImage_range = checkBox4.Checked;
                numericUpDown12.Value = trackBar4.Value;
                numericUpDown11.Value = trackBar5.Value;
                RunProgram.SeleImageRangeMax = (byte)numericUpDown12.Value;
                RunProgram.SeleImageRangeMin = (byte)numericUpDown11.Value;
                numericUpDown1.Value = trackBar1.Value;
                numericUpDown2.Value = trackBar2.Value;
                numericUpDown3.Value = trackBar3.Value;
                RunProgram.Emphasizefactor = (byte)numericUpDown1.Value;
                RunProgram.EmphasizeH = (byte)numericUpDown2.Value;
                RunProgram.EmphasizeW = (byte)numericUpDown3.Value;
                HObject image = RunProgram.GetEmset(RunProgram.GetPThis().Image());
                visionUserC1.SetImaage(image);
                visionUserC1.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                HObject image = RunProgram.GetEmset(RunProgram.GetPThis().Image());
                visionUserC1.SetImaage(image);
                visionUserC1.ShowImage();
            }
            catch (Exception)
            {
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
        }

        private void label11_Click(object sender, EventArgs e)
        {
        }
    }
}