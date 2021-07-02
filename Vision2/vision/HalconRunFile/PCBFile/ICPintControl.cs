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
using static Vision2.vision.HalconRunFile.PCBFile.ICPint;
using static Vision2.vision.Vision;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    public partial class ICPintControl : UserControl
    {
        public ICPintControl()
        {
            InitializeComponent();
        }
        public ICPintControl(ICPint iCPint , HalconRun halconRun) : this()
        {
            ICPintT = iCPint;
            halcon = halconRun;
            select_obj_type1.SetData(ICPintT.select_Shape_Min_);
            GetParm();
        }
        HalconRun halcon;
        ICPint ICPintT;

        public void GetParm()
        {
            try
            {
                isChev = true;
                for (int i = 0; i < ICPintT.ziPoints.Count; i++)
                {
                    listBox1.Items.Add(i + 1);
                }
                //numericUpDown4.Value =(decimal) ICPintT.Heiath;
                //numericUpDown6.Value = (decimal)ICPintT.Watih;
                //numericUpDown8.Value = (decimal)ICPintT.Phi;
                numericUpDown3.Value = ICPintT.Threshold_Min_Max.Min;
                numericUpDown7.Value = ICPintT.Threshold_Min_Max.Max ;
                checkBox1.Checked = ICPintT.IsZpint;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isChev = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.SmallestRectangle2(ICPintT.AOIObj, out HTuple row, out HTuple colu,
                    out HTuple phi, out HTuple lengt1, out HTuple lengt2);
                if (row.Length == 0)
                {
                HOperatorSet.DrawRectangle2(halcon.hWindowHalcon(),  out row, out colu,
                    out phi, out lengt1, out lengt2);
                }
                else
                {
                    HOperatorSet.DrawRectangle2Mod(halcon.hWindowHalcon(), row, colu, ICPintT.Phi, lengt1, lengt2, out row, out colu,
                    out phi, out lengt1, out lengt2);
                }

                ICPintT.Phi = phi;
                HOperatorSet.GenRectangle2(out HObject circle, row, colu, ICPintT.Phi, lengt1, lengt2);
                ICPintT.AOIObj = circle;
      
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {

        }
        bool isChev = true;
        void Set()
        {
            try
            {
                if (isChev)
                {
                    return;
                }
                //ICPintT.Heiath = (double)numericUpDown4.Value;
                //ICPintT.Watih = (double)numericUpDown6.Value;
                //ICPintT.Phi = (byte)numericUpDown8.Value;
                ICPintT.IsZpint = checkBox1.Checked;
                ICPintT. Threshold_Min_Max.Min = (byte)numericUpDown3.Value;
                ICPintT.Threshold_Min_Max.Max =  (byte)numericUpDown7.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                ziPoint.ICOBJ1 = RunProgram.DragMoveOBJ(halcon, ziPoint.ICOBJ1);
            }
            catch (Exception)
            {
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ziPoint.PintZi = RunProgram.DragMoveOBJ(halcon, ziPoint.PintZi);
            }
            catch (Exception)
            {
            }
        }
        ICPint.ZiPoint ziPoint;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                ziPoint = ICPintT.ziPoints[listBox1.SelectedIndex];
                ziPoint.PintRun(halcon.GetImageOBJ(ICPintT.Threshold_Min_Max.ImageTypeObj),
                    ICPintT.homMat2D,  ICPintT, halcon.GetOneImageR(), out HObject errDobj, out HObject obj,1);;
                halcon.ShowObj();
                propertyGrid1.SelectedObject = ziPoint;
            }
            catch (Exception)
            {
            }
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            Set();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                ziPoint = ICPintT.ziPoints[listBox1.SelectedIndex];
                ziPoint.PintRun(halcon.GetImageOBJ(ICPintT.Threshold_Min_Max.ImageTypeObj),
                    ICPintT.homMat2D, ICPintT, halcon.GetOneImageR(), out HObject errDobj, out HObject obj, 2);
                //halcon.AddObj(errDobj, ColorResult.yellow);
                //halcon.AddObj(obj);
                halcon.ShowObj();
                propertyGrid1.SelectedObject = ziPoint;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ICPintT. ziPoints.Add(new ZiPoint());
                listBox1.Items.Add(listBox1.Items.Count);


            }
            catch (Exception)
            {
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ICPintT.ziPoints.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
            catch (Exception)
            {
            }
        }

        private void 绘制区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ziPoint.PintZi = RunProgram.DrawModOBJ(halcon, HalconRun.EnumDrawType.Rectangle2, ziPoint.PintZi);
            }
            catch (Exception)
            {
            }
        }

        private void 绘制焊脚区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ziPoint.ICOBJ1 = RunProgram.DrawModOBJ(halcon, HalconRun.EnumDrawType.Rectangle2, ziPoint.ICOBJ1);
       
            }
            catch (Exception)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                ziPoint = ICPintT.ziPoints[listBox1.SelectedIndex];
                ziPoint.PintRun(halcon.GetImageOBJ(ICPintT.Threshold_Min_Max.ImageTypeObj),
                    ICPintT.homMat2D, ICPintT, halcon.GetOneImageR(), out HObject errDobj, out HObject obj, 3);
                halcon.ShowObj();
                propertyGrid1.SelectedObject = ziPoint;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
     
        }
    }
}
