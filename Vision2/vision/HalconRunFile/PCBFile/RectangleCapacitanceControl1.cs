using HalconDotNet;
using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    public partial class RectangleCapacitanceControl1 : UserControl
    {
        public RectangleCapacitanceControl1()
        {
            InitializeComponent();
        }

        public RectangleCapacitanceControl1(RectangleCapacitance rectangleCapacitance, IDrawHalcon halconRun) : this()
        {
            halcon = halconRun as HalconRun;
            Run = rectangleCapacitance;
            propertyGrid2.SelectedObject = Run;
            propertyGrid1.SelectedObject = Run.IntCapcitanceMinx;
            select_obj_type1.SetData(Run.select_Shape_Min_Max);
            thresholdControls1.SetData(Run.Threshold_Min_M);
            thresholdControls2.SetData(Run.Threshold_Min_DP);
            numericUpDown1.Value = (decimal)Run.IntCapcitanceMinx.AreaCValue;
        }

        private HalconRun halcon;
        private RectangleCapacitance Run;

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.SmallestRectangle2(Run.Point1, out HTuple row, out HTuple colu, out HTuple phi, out HTuple lengt1, out HTuple lnegt2);
                if (row.Length == 0)
                {
                    HOperatorSet.DrawRectangle2(halcon.hWindowHalcon(), out row, out colu, out phi, out lengt1, out lnegt2);
                }
                else
                {
                    HOperatorSet.DrawRectangle2Mod(halcon.hWindowHalcon(), row, colu, phi, lengt1, lnegt2, out row, out colu, out phi, out lengt1, out lnegt2);
                }
                HOperatorSet.GenRectangle2(out HObject circle, row, colu, phi, lengt1, lnegt2);
                Run.Point1 = circle;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.AreaCenter(Run.AOIObj, out HTuple area, out HTuple row, out HTuple column);

                Run.AOIObj = RunProgram.DragMoveOBJ(halcon, Run.AOIObj);

                HOperatorSet.AreaCenter(Run.AOIObj, out HTuple area2, out HTuple row2, out HTuple column2);

                HOperatorSet.VectorAngleToRigid(row, column, 0, row2, column2, 0, out HTuple hTuple);
                HOperatorSet.AffineTransRegion(Run.Point1, out HObject hObject, hTuple, "nearest_neighbor");
                HOperatorSet.AffineTransRegion(Run.Point2, out HObject hObject2, hTuple, "nearest_neighbor");
                Run.Point1 = hObject;
                Run.Point2 = hObject2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                AoiObj aoiObj = Run.GetAoi();
                aoiObj.DebugID = 1;
                Run.RunDebug(halcon.GetOneImageR(), aoiObj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RectangleCapacitanceControl1_Load(object sender, EventArgs e)
        {
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Run.IntCapcitanceMinx.AreaCValue = (double)numericUpDown1.Value;
            }
            catch (Exception)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.SmallestRectangle2(Run.Point2, out HTuple row, out HTuple colu, out HTuple phi, out HTuple lengt1, out HTuple lnegt2);
                if (row.Length == 0)
                {
                    HOperatorSet.DrawRectangle2(halcon.hWindowHalcon(), out row, out colu, out phi, out lengt1, out lnegt2);
                }
                else
                {
                    HOperatorSet.DrawRectangle2Mod(halcon.hWindowHalcon(), row, colu, phi, lengt1, lnegt2, out row, out colu, out phi, out lengt1, out lnegt2);
                }
                HOperatorSet.GenRectangle2(out HObject circle, row, colu, phi, lengt1, lnegt2);
                Run.Point2 = circle;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}