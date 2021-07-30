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

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class DrawVisionForm : Form
    {
        public DrawVisionForm()
        {
            InitializeComponent();
            HWindI.Initialize(hWindowControl1);
        }
        HWindID HWindI = new HWindID();
        HalconRun halcon;
        RunProgram runPa;
        public DrawVisionForm(RunProgram run) : this()
        {
            runPa = run;
            halcon = run.GetPThis();
          
            HWindI.SetImaage(halcon.Image());
            Control control = run. GetControl(halcon);
            if (control != null)
            {
                control.Dock = DockStyle.Fill;
                splitContainer1.Panel2.Controls.Add(control);
                control.Tag = run;
            }
            toolStripComboBox1.Items.AddRange(Enum.GetNames(typeof(ImageTypeObj)));
            TrackBar trackBar = (TrackBar)toolStripTrackBar1.Control;
            trackBar.Maximum = 200;
            trackBar.Value = (int)RunProgram.Circl_Rire;
            toolStripTrackBar1.GetBase().TickStyle = TickStyle.None;
            //toolStripLabel1.Text = trackBar.Value.ToString();
            RunProgram.Circl_Rire = trackBar.Value;
            trackBar.Scroll += TrackBar_Scroll;
        }
        private void DrawVisionForm_Load(object sender, EventArgs e)
        {
            //HWindI.Initialize(hSmartWindowControl1);
        }
        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            try
            {
                TrackBar trackBar = (TrackBar)sender;
                RunProgram.Circl_Rire = trackBar.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                InterfaceVisionControl control = runPa.GetPInt();
                halcon.RunHProgram(halcon.GetOneImageR(), runPa);
                halcon.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                runPa.DrawObj = RunProgram.DrawHObj(HWindI, runPa.DrawObj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsButton3_Click(object sender, EventArgs e)
        {
            try
            {
                runPa.DrawObj = RunProgram.DrawRmoveObj(HWindI, runPa.DrawObj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                runPa.AOIObj = RunProgram.DrawHObj(HWindI, runPa.AOIObj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                runPa.AOIObj = RunProgram.DrawRmoveObj(HWindI, runPa.AOIObj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsButton6_Click(object sender, EventArgs e)
        {
            runPa.ShowHelp();
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_DropDownClosed(object sender, EventArgs e)
        {
            try
            {

                HWindI.SetImaage(halcon.GetImageOBJ((ImageTypeObj)Enum.Parse(typeof(ImageTypeObj),
                    toolStripComboBox1.SelectedItem.ToString())));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 移动掩模区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                runPa.DrawObj = RunProgram.DragMoveOBJ(halcon, runPa.DrawObj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 清除掩模区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否清除掩模区", "确定清除掩模区", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    runPa.DrawObj = new HObject();
                    runPa.DrawObj.GenEmptyObj();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 绘制掩模区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (halcon.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                HObject final_region = new HObject();
                halcon.Drawing = true;
                HOperatorSet.SetColor(halcon.hWindowHalcon(), "red");
                halcon.GetHWindow().Focus();
                HOperatorSet.DrawRegion(out final_region, halcon.hWindowHalcon());
                runPa.DrawObj = final_region;
                HOperatorSet.DispObj(runPa.DrawObj, halcon.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            halcon.Drawing = false;
        }

        private void 绘制掩模方型ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple hv_Row = null, hv_Column = null;
                halcon.DrawType = 2;
                if (halcon.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                halcon.DrawErasure = true;
                HObject final_region = new HObject();
                halcon.Drawing = true;
                HOperatorSet.SetColor(halcon.hWindowHalcon(), "red");
                halcon.GetHWindow().Focus();
                HOperatorSet.DrawRectangle1(halcon.hWindowHalcon(), out hv_Row, out hv_Column, out HTuple length1, out HTuple length2);
                HOperatorSet.GenRectangle1(out final_region, hv_Row, hv_Column, length1, length2);
                runPa.DrawObj = final_region;
                HOperatorSet.DispObj(runPa.DrawObj, halcon.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            halcon.Drawing = false;
        }
    }
}
