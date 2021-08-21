using HalconDotNet;
using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    public partial class Cap : UserControl
    {
        private Capacitance cap;
        private HalconRun run;

        public Cap()
        {
            InitializeComponent();
        }

        public Cap(Capacitance capacitance, IDrawHalcon halconRun) : this()
        {
            cap = capacitance;
            run = halconRun as HalconRun;
        }

        private bool isChev;

        private void Cap_Load(object sender, EventArgs e)
        {
            try
            {
                isChev = true;
                numericUpDown1.Value = (decimal)cap.Periphery_Circle;
                numericUpDown2.Value = (decimal)cap.Inside_Circle;
                numericUpDown3.Value = cap.Inside_Thread_Min;
                numericUpDown4.Value = cap.Inside_Thread_Max;
                numericUpDown5.Value = cap.Erosion_Circle;
                numericUpDown6.Value = cap.Periphery_ThreadMin;
                numericUpDown7.Value = cap.Periphery_ThreadMax;
                numericUpDown8.Value = (decimal)cap.AngleMin;
                numericUpDown9.Value = (decimal)cap.AngleMax;
                checkBox1.Checked = cap.IsText;
                checkBox2.Checked = cap.IsRoi;

                select_obj_type1.SetData(cap.Select_Shape_Min_Max);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isChev = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.AreaCenter(cap.AOIObj, out HTuple area, out HTuple row, out HTuple colu);
                if (row.Length == 0)
                {
                    row = 500;
                    colu = 500;
                }
                HOperatorSet.GenCircle(out HObject circle, row, colu, cap.Inside_Circle);
                HOperatorSet.GenCircle(out HObject circle2, row, colu, cap.Periphery_Circle);
                HOperatorSet.Union2(circle, circle2, out circle2);
                cap.AOIObj = circle2;
                cap.AOIObj = RunProgram.DragMoveOBJ(run, cap.AOIObj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Set()
        {
            try
            {
                if (isChev)
                {
                    return;
                }
                cap.Periphery_Circle = (double)numericUpDown1.Value;
                cap.Inside_Circle = (double)numericUpDown2.Value;
                cap.Inside_Thread_Min = (byte)numericUpDown3.Value;
                cap.Inside_Thread_Max = (byte)numericUpDown4.Value;
                cap.Erosion_Circle = (byte)numericUpDown5.Value;
                cap.Periphery_ThreadMin = (byte)numericUpDown6.Value;
                cap.Periphery_ThreadMax = (byte)numericUpDown7.Value;
                cap.AngleMin = (double)numericUpDown8.Value;
                cap.AngleMax = (double)numericUpDown9.Value;
                cap.IsText = checkBox1.Checked;
                cap.IsRoi = checkBox2.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Set();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Set();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Set();
        }
    }
}