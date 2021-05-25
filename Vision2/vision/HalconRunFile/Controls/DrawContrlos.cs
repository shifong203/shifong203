using HalconDotNet;
using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;


namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class DrawContrlos : UserControl
    {
        public DrawContrlos()
        {
            InitializeComponent();
        }

        HWindID HWindI = new HWindID();

        public DrawContrlos(RunProgram run) : this()
        {
            RunProgram = run;
            HalconRun = run.GetPThis() as HalconRun;
            toolStripComboBox1.Items.AddRange(Enum.GetNames(typeof(Vision.ImageTypeObj)));
            TrackBar trackBar = (TrackBar)toolStripTrackBar1.Control;
            trackBar.Maximum = 200;
            trackBar.Value = (int)RunProgram.Circl_Rire;
            toolStripLabel1.Text = trackBar.Value.ToString();
            RunProgram.Circl_Rire = trackBar.Value;
            trackBar.Scroll += TrackBar_Scroll;    
        }

        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            try
            {
                TrackBar trackBar = (TrackBar)sender;
                toolStripLabel1.Text = trackBar.Value.ToString();
                RunProgram.Circl_Rire = trackBar.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        HalconRun HalconRun;
        RunProgram RunProgram;
        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                RunProgram.DrawObj = RunProgram.DrawHObj(HalconRun, RunProgram.DrawObj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void tsButton2_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否清除绘制区", "确定清除绘制区", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    RunProgram.DrawObj = new HObject();
                    RunProgram.DrawObj.GenEmptyObj();
                }
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
                RunProgram.DrawObj = RunProgram.DrawRmoveObj(HalconRun, RunProgram.DrawObj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void 绘制椭圆ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                HTuple hv_Row = null, hv_Column = null;

                HObject final_region = new HObject();
                if (HalconRun.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                HalconRun.Drawing = true;
                HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "red");
                HalconRun.GetHWindow().Focus();
                HOperatorSet.DrawEllipse(HalconRun.hWindowHalcon(), out hv_Row, out hv_Column, out HTuple phi, out HTuple length1, out HTuple length2);
                HOperatorSet.GenEllipse(out final_region, hv_Row, hv_Column, phi, length1, length2);
                RunProgram.DrawObj = final_region;
                HOperatorSet.DispObj(RunProgram.DrawObj, HalconRun.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            HalconRun.Drawing = false;
        }

        private void 绘制圆弧ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                HTuple hv_Row = null, hv_Column = null;
                HalconRun.DrawType = 2;
                if (HalconRun.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                HalconRun.DrawErasure = true;
                HObject final_region = new HObject();
                HalconRun.Drawing = true;
                HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "red");
                HalconRun.GetHWindow().Focus();
                HOperatorSet.DrawCircle(HalconRun.hWindowHalcon(), out hv_Row, out hv_Column, out HTuple phi);
                HOperatorSet.GenCircle(out final_region, hv_Row, hv_Column, phi);
                RunProgram.DrawObj = final_region;
                HOperatorSet.DispObj(RunProgram.DrawObj, HalconRun.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            HalconRun.Drawing = false;

        }

        private void 绘制方矩形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple hv_Row = null, hv_Column = null;

                HalconRun.DrawType = 2;
                if (HalconRun.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                HalconRun.DrawErasure = true;
                HObject final_region = new HObject();
                HalconRun.Drawing = true;
                HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "red");
                HalconRun.GetHWindow().Focus();
                HOperatorSet.DrawRectangle1(HalconRun.hWindowHalcon(), out hv_Row, out hv_Column, out HTuple length1, out HTuple length2);
                HOperatorSet.GenRectangle1(out final_region, hv_Row, hv_Column, length1, length2);
                RunProgram.DrawObj = final_region;
                HOperatorSet.DispObj(RunProgram.DrawObj, HalconRun.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            HalconRun.Drawing = false;

        }

        private void 绘制角度矩形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (HalconRun.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                HTuple hv_Row = null, hv_Column = null;

                HObject final_region = new HObject();
                HalconRun.Drawing = true;
                HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "red");
                HalconRun.GetHWindow().Focus();
                HOperatorSet.DrawRectangle2(HalconRun.hWindowHalcon(), out hv_Row, out hv_Column, out HTuple phi, out HTuple length1, out HTuple length2);
                HOperatorSet.GenRectangle2(out final_region, hv_Row, hv_Column, phi, length1, length2);
                RunProgram.DrawObj = final_region;
                HOperatorSet.DispObj(RunProgram.DrawObj, HalconRun.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            HalconRun.Drawing = false;
        }

        private void 绘制区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple hv_Row = null, hv_Column = null;
                if (HalconRun.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                HObject final_region = new HObject();
                HalconRun.Drawing = true;
                HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "red");
                HalconRun.GetHWindow().Focus();
                HOperatorSet.DrawRegion(out final_region, HalconRun.hWindowHalcon());
                RunProgram.DrawObj = final_region;
                HOperatorSet.DispObj(RunProgram.DrawObj, HalconRun.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            HalconRun.Drawing = false;
        }

        private void 绘制NBSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (HalconRun.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                HalconRun.Drawing = true;
                HTuple hv_Row = null, hv_Column = null;
                HObject final_region = new HObject();
                HalconRun.Drawing = true;
                //radius = int.Parse(toolStripComboBox1.SelectedItem.ToString());
                HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "red");
                HalconRun.GetHWindow().Focus();
                HOperatorSet.DrawNurbs(out final_region, HalconRun.hWindowHalcon(), "true", "true", "true",
                  "true", 3, out HTuple hv_Rows, out HTuple hv_Cols, out HTuple hv_Weights);
                RunProgram.DrawObj = final_region;
                HOperatorSet.DispObj(RunProgram.DrawObj, HalconRun.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            HalconRun.Drawing = false;
        }

        private void 绘制点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (HalconRun.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
            }
            catch (Exception)
            {
            }
        }

        private void 绘制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple hv_Row = null, hv_Column = null;

                HObject final_region = new HObject();
                if (HalconRun.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                HalconRun.Drawing = true;
                HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "red");
                HalconRun.GetHWindow().Focus();
                HOperatorSet.DrawLine(HalconRun.hWindowHalcon(), out hv_Row, out hv_Column, out HTuple length1, out HTuple length2);
                HOperatorSet.GenRegionLine(out final_region, hv_Row, hv_Column, length1, length2);
                RunProgram.DrawObj = final_region;
                HOperatorSet.DispObj(RunProgram.DrawObj, HalconRun.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            HalconRun.Drawing = false;
        }

   

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            toolStripSplitButton1.ShowDropDown();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                HalconRun.ShowVision(RunProgram.Name, HalconRun.GetdataVale());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsButton4_Click(object sender, EventArgs e)
        {
            try
            {
                HalconRun.HobjClear();
                InterfaceVisionControl control = RunProgram .GetPInt() as InterfaceVisionControl;
                if (control!=null)
                {
                    RunProgram.Run(HalconRun, HalconRun.GetdataVale(), 0);
                    HalconRun.ShowObj();
                    //control.RunHProgram(HalconRun, 0, RunProgram.Name);
                }
                else
                {
                  RunProgram.GetPThis().RunHProgram(HalconRun, HalconRun.GetdataVale(), 0,RunProgram.Name);
                }

                //HalconRun.UPStart();
                //HalconRun.ShowVision(RunProgram.Name);
                //HalconRun.EndChanged();
                //HalconRun.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsButton6_Click_1(object sender, EventArgs e)
        {
            RunProgram.ShowHelp();
        }



        private void toolStripTrackBar1_Click(object sender, EventArgs e)
        {

        }

        private void DrawContrlos_Load(object sender, EventArgs e)
        {
            try
            {
                HWindI.Initialize(hWindowControl1);
                HWindI.SetImaage(HalconRun.Image());
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripComboBox1_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
               
                HWindI.SetImaage(HalconRun.GetImageOBJ((Vision.ImageTypeObj)Enum.Parse(typeof(Vision.ImageTypeObj), 
                    toolStripComboBox1.SelectedItem.ToString())));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripCheckbox1_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = toolStripCheckbox1.GetBase();
                hWindowControl1.Visible = checkBox.Checked;
                if (hWindowControl1.Visible)
                {
                    this.Height = tsButton1.Height + 500;
                }
                else
                {
                    this.Height = tsButton1.Height;
                }
            }
            catch (Exception)
            {
            }
        }

        private void tsButton5_Click(object sender, EventArgs e)
        {
            try
            {
                RunProgram.DrawObj = RunProgram.DragMoveOBJ(HalconRun, RunProgram.DrawObj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
