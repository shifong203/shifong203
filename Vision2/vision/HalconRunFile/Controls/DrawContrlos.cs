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

        private HWindID HWindI = new HWindID();

        public DrawContrlos(RunProgram run) : this()
        {
            runPa = run;
            halcon = run.GetPThis() as HalconRun;
            toolStripComboBox1.Items.AddRange(Enum.GetNames(typeof(ImageTypeObj)));
            TrackBar trackBar = (TrackBar)toolStripTrackBar1.Control;
            trackBar.Maximum = 200;
            trackBar.Value = (int)RunProgram.Circl_Rire;
            toolStripTrackBar1.GetBase().TickStyle = TickStyle.None;
            //toolStripLabel1.Text = trackBar.Value.ToString();
            RunProgram.Circl_Rire = trackBar.Value;
            trackBar.Scroll += TrackBar_Scroll;
        }

        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            try
            {
                TrackBar trackBar = (TrackBar)sender;
                //toolStripLabel1.Text = trackBar.Value.ToString();
                RunProgram.Circl_Rire = trackBar.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private HalconRun halcon;
        private RunProgram runPa;

        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                runPa.DrawObj = RunProgram.DrawHObj(halcon, runPa.DrawObj);
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
                runPa.DrawObj = RunProgram.DrawRmoveObj(halcon, runPa.DrawObj);
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
                if (halcon.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                halcon.Drawing = true;
                HOperatorSet.SetColor(halcon.hWindowHalcon(), "red");
                halcon.GetHWindow().Focus();
                HOperatorSet.DrawEllipse(halcon.hWindowHalcon(), out hv_Row, out hv_Column, out HTuple phi, out HTuple length1, out HTuple length2);
                HOperatorSet.GenEllipse(out final_region, hv_Row, hv_Column, phi, length1, length2);
                runPa.DrawObj = final_region;
                HOperatorSet.DispObj(runPa.DrawObj, halcon.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            halcon.Drawing = false;
        }

        private void 绘制圆弧ToolStripMenuItem_Click(object sender, EventArgs e)
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
                HOperatorSet.DrawCircle(halcon.hWindowHalcon(), out hv_Row, out hv_Column, out HTuple phi);
                HOperatorSet.GenCircle(out final_region, hv_Row, hv_Column, phi);
                runPa.DrawObj = final_region;
                HOperatorSet.DispObj(runPa.DrawObj, halcon.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            halcon.Drawing = false;
        }

        private void 绘制方矩形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 绘制角度矩形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (halcon.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                HTuple hv_Row = null, hv_Column = null;

                HObject final_region = new HObject();
                halcon.Drawing = true;
                HOperatorSet.SetColor(halcon.hWindowHalcon(), "red");
                halcon.GetHWindow().Focus();
                HOperatorSet.DrawRectangle2(halcon.hWindowHalcon(), out hv_Row, out hv_Column, out HTuple phi, out HTuple length1, out HTuple length2);
                HOperatorSet.GenRectangle2(out final_region, hv_Row, hv_Column, phi, length1, length2);
                runPa.DrawObj = final_region;
                HOperatorSet.DispObj(runPa.DrawObj, halcon.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            halcon.Drawing = false;
        }

        private void 绘制区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 绘制NBSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (halcon.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                halcon.Drawing = true;
                HTuple hv_Row = null, hv_Column = null;
                HObject final_region = new HObject();
                halcon.Drawing = true;
                //radius = int.Parse(toolStripComboBox1.SelectedItem.ToString());
                HOperatorSet.SetColor(halcon.hWindowHalcon(), "red");
                halcon.GetHWindow().Focus();
                HOperatorSet.DrawNurbs(out final_region, halcon.hWindowHalcon(), "true", "true", "true",
                  "true", 3, out HTuple hv_Rows, out HTuple hv_Cols, out HTuple hv_Weights);
                runPa.DrawObj = final_region;
                HOperatorSet.DispObj(runPa.DrawObj, halcon.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            halcon.Drawing = false;
        }

        private void 绘制点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (halcon.Drawing)
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
                if (halcon.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                halcon.Drawing = true;
                HOperatorSet.SetColor(halcon.hWindowHalcon(), "red");
                halcon.GetHWindow().Focus();
                HOperatorSet.DrawLine(halcon.hWindowHalcon(), out hv_Row, out hv_Column, out HTuple length1, out HTuple length2);
                HOperatorSet.GenRegionLine(out final_region, hv_Row, hv_Column, length1, length2);
                runPa.DrawObj = final_region;
                HOperatorSet.DispObj(runPa.DrawObj, halcon.hWindowHalcon());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            halcon.Drawing = false;
        }

        private void tsButton6_Click_1(object sender, EventArgs e)
        {
            runPa.ShowHelp();
        }

        private void toolStripTrackBar1_Click(object sender, EventArgs e)
        {
        }

        private void DrawContrlos_Load(object sender, EventArgs e)
        {
            try
            {
                HWindI.Initialize(hWindowControl1);
                HWindI.SetImaage(halcon.Image());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                InterfaceVisionControl control = runPa.GetPInt() as InterfaceVisionControl;
                halcon.RunHProgram(halcon.GetOneImageR(), runPa);
                halcon.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 移动区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 清除掩模ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 绘制ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void 绘制掩模方型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripDropDownButton1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripDropDownButton1.ShowDropDown();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                runPa.AOIObj = RunProgram.DrawHObj(halcon, runPa.AOIObj);
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
                runPa.AOIObj = RunProgram.DrawRmoveObj(halcon, runPa.AOIObj);
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

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            OBJSeleForm oBJSeleForm = new OBJSeleForm();

            oBJSeleForm.Show();

            oBJSeleForm.ShowImage(halcon.GetOneImageR());
            //oBJSeleForm.AddErrObj(runPa.nGRoi);
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
        }

        private void 显示缺陷细节ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}