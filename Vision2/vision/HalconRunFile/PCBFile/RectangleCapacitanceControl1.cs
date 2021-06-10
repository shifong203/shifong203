using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public RectangleCapacitanceControl1(RectangleCapacitance  rectangleCapacitance,HalconRun halconRun) : this()
        {
            halcon= halconRun;
            Run = rectangleCapacitance;
            propertyGrid1.SelectedObject = Run.IntCapcitanceMinx;
            select_obj_type1.SetData(Run.select_Shape_Min_Max);
            thresholdControls1.SetData(Run.Threshold_Min_M);
            thresholdControls2.SetData(Run.Threshold_Min_DP);
        }
        HalconRun halcon;
        RectangleCapacitance Run;

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.SmallestRectangle2(Run.TestingRoi, out HTuple row, out HTuple colu, out HTuple phi, out HTuple lengt1, out HTuple lnegt2);
                if (row.Length==0)
                {
                    HOperatorSet.DrawRectangle2(halcon.hWindowHalcon(), out row, out colu,  out phi, out lengt1, out lnegt2);
                }
                else
                {
                    HOperatorSet.DrawRectangle2Mod(halcon.hWindowHalcon(), row, colu, phi, lengt1, lnegt2, out row, out colu, out phi, out lengt1, out lnegt2);
                }
                HOperatorSet.GenRectangle2(out HObject circle, row, colu, phi, lengt1, lnegt2);
                Run.TestingRoi = circle;
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
                Run.TestingRoi = RunProgram.DragMoveOBJ(halcon, Run.TestingRoi);
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
                Run.RunDebug(halcon, Run.GetRunProgram(), halcon.GetOneImageR(),out HObject ERR,1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void RectangleCapacitanceControl1_Load(object sender, EventArgs e)
        {
  
        }
    }
}
