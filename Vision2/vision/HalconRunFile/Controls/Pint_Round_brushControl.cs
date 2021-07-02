using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class Pint_Round_brushControl : UserControl
    {
        public Pint_Round_brushControl()
        {
            InitializeComponent();
        }
        Pin_Round_brush_needlecs pint_Round_BrushControl;
        public Pint_Round_brushControl(Pin_Round_brush_needlecs pint_Round) : this()
        {
            isChave = true;
            pint_Round_BrushControl = pint_Round;
            Halcon = pint_Round.GetPThis();
            GetProjiet();
            isChave = false;
        }
        bool isChave;
        HalconRun Halcon;

        public void SetProjie(int id)
        {
            try
            {
                if (isChave)
                {
                    return;
                }
                Halcon.HobjClear();
                pint_Round_BrushControl.Number = (int)numericUpDown10.Value;
                pint_Round_BrushControl.Threshold_Min_Max.Min = (byte)numericUpDownThrMin.Value;
                pint_Round_BrushControl.Threshold_Min_Max.Max = (byte)numericUpDownThrMax.Value;
                pint_Round_BrushControl.Threshold_Min_Max2.Min = (byte)numericUpDown2.Value;
                pint_Round_BrushControl.Threshold_Min_Max2.Max = (byte)numericUpDown1.Value;
                pint_Round_BrushControl.Threshold_Min_Max3.Min = (byte)numericUpDown4.Value;
                pint_Round_BrushControl.Threshold_Min_Max3.Max = (byte)numericUpDown3.Value;
                pint_Round_BrushControl.ECircleT = (double)numericUpDown7.Value;
                pint_Round_BrushControl.ECircle = (double)numericUpDown8.Value;
                pint_Round_BrushControl.FillArea = (double)numericUpDown9.Value;
                pint_Round_BrushControl.Threshold_MinG.Min = (byte)numericUpDown6.Value;
                pint_Round_BrushControl.Threshold_MinG.Max = (byte)numericUpDown5.Value;
                pint_Round_BrushControl.Run(Halcon.GetOneImageR(), new AoiObj() { DebugID=id});
                Halcon.ShowObj();
      
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void GetProjiet()
        {
            try
            {
                numericUpDown10.Value = pint_Round_BrushControl.Number;
                numericUpDown9.Value = (decimal)pint_Round_BrushControl.FillArea;
                numericUpDown8.Value = (decimal)pint_Round_BrushControl.ECircle;
                numericUpDown7.Value = (decimal)pint_Round_BrushControl.ECircleT;
                numericUpDown6.Value = pint_Round_BrushControl.Threshold_MinG.Min;
                numericUpDown5.Value = pint_Round_BrushControl.Threshold_MinG.Max;
                numericUpDownThrMin.Value = pint_Round_BrushControl.Threshold_Min_Max.Min;
                numericUpDownThrMax.Value = pint_Round_BrushControl.Threshold_Min_Max.Max;
                numericUpDown2.Value = pint_Round_BrushControl.Threshold_Min_Max2.Min;
                numericUpDown1.Value = pint_Round_BrushControl.Threshold_Min_Max2.Max;
                numericUpDown4.Value = pint_Round_BrushControl.Threshold_Min_Max3.Min;
                numericUpDown3.Value = pint_Round_BrushControl.Threshold_Min_Max3.Max;
                select_obj_type1.SetData(pint_Round_BrushControl.select_Shape_);
                select_obj_type2.SetData(pint_Round_BrushControl.select_Shape_arae2);
                select_obj_type3.SetData(pint_Round_BrushControl.Select_Shape_Min);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void numericUpDownThrMin_ValueChanged(object sender, EventArgs e)
        {
            SetProjie(3);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            SetProjie(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetProjie(5);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetProjie(7);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetProjie(2);
        }

        private void numericUpDown3_ValueChanged_1(object sender, EventArgs e)
        {
            SetProjie(4);
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            SetProjie(6);
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            SetProjie(6);
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            SetProjie(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetProjie(8);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                SetProjie(9);
                numericUpDown10.Value = pint_Round_BrushControl.Number;
            }
            catch (Exception)
            {
            }
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            SetProjie(8);
        }
    }
}
