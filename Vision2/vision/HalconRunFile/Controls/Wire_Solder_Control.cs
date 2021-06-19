using HalconDotNet;
using System;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.DataGridViewF;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.RunProgramFile.RunProgram;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class Wire_Solder_Control : UserControl
    {
        public Wire_Solder_Control()
        {
            ismove = true;
            InitializeComponent();
        }
        public Wire_Solder_Control(Wire_Solder wire_) : this()
        {
            Wire_S = wire_;
            Halcon = (HalconRun)Wire_S.GetPThis();
        }
        Wire_Solder Wire_S;
        public HalconRun Halcon;
        bool ismove = false;
        public void Set_Pragram(int runid)
        {
            if (ismove)
            {
                return;
            }
            try
            {
                Wire_S.ClosingCircle = (double)numericUpDown3.Value;
                Wire_S.ErosionCircle = (double)numericUpDown4.Value;
                Wire_S.H_threshold_min = (byte)numericUpDownHThrMin.Value;
                Wire_S.H_threshold_max = (byte)numericUpDownHThrMax.Value;
                Wire_S.S_threshold_min = (byte)numericUpDownSThrMin.Value;
                Wire_S.S_threshold_max = (byte)numericUpDownSThrMax.Value;
                Wire_S.V_threshold_min = (byte)numericUpDownVThrMin.Value;
                Wire_S.V_threshold_max = (byte)numericUpDownVThrMax.Value;
                Wire_S.Select_shap_min = (double)numericUpDownAreaMin.Value;
                Wire_S.Select_shap_max = (double)numericUpDownAreaMax.Value;
                Wire_S.MaxLength2 = (double)numericUpDMaxLength1.Value;
                Wire_S.MaxLength1 = (double)numericUpDMaxLength2.Value;
                Wire_S.MaxDeg = (double)numericUpDDeg.Value;
                Wire_S.MinLength1 = (double)numericUpDMinLength1.Value;
                Wire_S.MinLength2 = (double)numericUpDMinLength2.Value;
                Wire_S.thresholdV_2.Min = (byte)numericUpDown6.Value;
                Wire_S.thresholdV_2.Max = (byte)numericUpDown5.Value;

                Wire_S.S_threshold_min2 = (byte)numericUpDownSAreaComputeThrMin.Value;
                Wire_S.S_threshold_max2 = (byte)numericUpDownSAreaComputeThrMax.Value;

                Wire_S.H_threshold_max2 = (byte)numericUpDown1.Value;
                Wire_S.H_threshold_min2 = (byte)numericUpDown2.Value;

                Halcon.HobjClear();
                Wire = Wire_S.listWelding[listBoxPoints.SelectedIndex];
                HOperatorSet.SmallestRectangle1(Wire_S.DrawObj, out HTuple row1, out HTuple column1, out HTuple row2, out HTuple column2);
                HOperatorSet.ClearWindow(visionUserControlV.HalconWindow);
                HOperatorSet.ClearWindow(visionUserControlS.HalconWindow);
                HOperatorSet.ClearWindow(visionUserControl1.HalconWindow);
                HOperatorSet.ClearWindow(visionUserControlRGB.HalconWindow);
                HOperatorSet.SetPart(visionUserControlV.HalconWindow, row1 - 20, column1 - 20, row2 + 20, column2 + 20);
                HOperatorSet.SetPart(visionUserControlS.HalconWindow, row1 - 20, column1 - 20, row2 + 20, column2 + 20);
                HOperatorSet.SetPart(visionUserControl1.HalconWindow, row1 - 20, column1 - 20, row2 + 20, column2 + 20);
                HOperatorSet.SetPart(visionUserControlRGB.HalconWindow, row1 - 20, column1 - 20, row2 + 20, column2 + 20);
                if (checkBox1.Checked)
                {
                    HOperatorSet.SetDraw(visionUserControlRGB.HalconWindow, "fill");
                    HOperatorSet.SetDraw(visionUserControl1.HalconWindow, "fill");
                    HOperatorSet.SetDraw(visionUserControlS.HalconWindow, "fill");
                    HOperatorSet.SetDraw(visionUserControlV.HalconWindow, "fill");
                }
                else
                {
                    HOperatorSet.SetDraw(visionUserControlRGB.HalconWindow, "margin");
                    HOperatorSet.SetDraw(visionUserControl1.HalconWindow, "margin");
                    HOperatorSet.SetDraw(visionUserControlS.HalconWindow, "margin");
                    HOperatorSet.SetDraw(visionUserControlV.HalconWindow, "margin");
                }

                HOperatorSet.SetColor(visionUserControlRGB.HalconWindow, "green");
                HOperatorSet.SetColor(visionUserControl1.HalconWindow, "green");
                HOperatorSet.SetColor(visionUserControlS.HalconWindow, "green");
                HOperatorSet.SetColor(visionUserControlV.HalconWindow, "green");

                bool DWET = Wire_S.RunP(Halcon.GetOneImageR(), runid, visionUserControl1.HalconWindow, visionUserControlS.HalconWindow, visionUserControlV.HalconWindow, visionUserControlRGB.HalconWindow);
                HOperatorSet.AreaCenter(Wire.HObject, out HTuple area, out HTuple row, out HTuple column);

                Halcon.AddImageMassage(row + 200, column, listBoxPoints.SelectedIndex + 1, ColorResult.blue, "true");
                //Wire.RunP(Halcon, Wire_S, hObject, hObject1);
                Halcon.AddObj(Wire_S.DrawObj, ColorResult.yellow);
                Halcon.AddObj(Wire.HObject);
                Halcon.ShowImage();
                Halcon.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Get_Pragram()
        {
            ismove = true;
            try
            {
                numericUpDown3.Value = (decimal)Wire_S.ClosingCircle;
                numericUpDown4.Value = (decimal)Wire_S.ErosionCircle;
                numericUpDownHThrMin.Value = Wire_S.H_threshold_min;
                numericUpDownHThrMax.Value = Wire_S.H_threshold_max;
                numericUpDown1.Value = Wire_S.H_threshold_max2;
                numericUpDown2.Value = Wire_S.H_threshold_min2;
                numericUpDownSThrMin.Value = Wire_S.S_threshold_min;
                numericUpDownSThrMax.Value = Wire_S.S_threshold_max;
                numericUpDownVThrMin.Value = Wire_S.V_threshold_min;
                numericUpDownVThrMax.Value = Wire_S.V_threshold_max;
                numericUpDownSAreaComputeThrMin.Value = Wire_S.S_threshold_min2;
                numericUpDownSAreaComputeThrMax.Value = Wire_S.S_threshold_max2;
                numericUpDMaxLength1.Value = (decimal)Wire_S.MaxLength2;
                numericUpDMaxLength2.Value = (decimal)Wire_S.MaxLength1;
                numericUpDDeg.Value = (decimal)Wire_S.MaxDeg;
                numericUpDownAreaMin.Value = (decimal)Wire_S.Select_shap_min;
                numericUpDownAreaMax.Value = (decimal)Wire_S.Select_shap_max;
                numericUpDMinLength1.Value = (decimal)Wire_S.MinLength1;
                numericUpDMinLength2.Value = (decimal)Wire_S.MinLength2;
                numericUpDown6.Value = (decimal)Wire_S.thresholdV_2.Min;
                numericUpDown5.Value = (decimal)Wire_S.thresholdV_2.Max;
            }
            catch (Exception ex)
            {
            }
            ismove = false;
        }
        Wire_Solder.Wire_S Wire;

        private void Wire_Solder_Control_Load(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.SetDraw(visionUserControl1.HalconWindow, "margin");
                HOperatorSet.SetDraw(visionUserControlS.HalconWindow, "margin");
                HOperatorSet.SetDraw(visionUserControlV.HalconWindow, "margin");
                Get_Pragram();
                listBoxPoints.Items.Clear();
                for (int i = 0; i < Project.formula.RecipeCompiler.Instance.Data.ListDatV.Count; i++)
                {
                    Column1.Items.Add(Project.formula.RecipeCompiler.Instance.Data.ListDatV[i].ComponentName);
                }
        
                for (int i = 0; i < Wire_S.listWelding.Count; i++)
                {
                    listBoxPoints.Items.Add(i + 1);
                }
                Wire = Wire_S.listWelding[0];
                dataGridView1.Rows.Clear();
                for (int i = 0; i < Wire.List3DName.Count; i++)
                {
                    int det = dataGridView1.Rows.Add();
                    if (Wire.List3DName[i] >= 0)
                    {
                        dataGridView1.Rows[det].Cells[0].Value = Project.formula.RecipeCompiler.Instance.Data.ListDatV[Wire.List3DName[i]].ComponentName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        HObject hObjectImage;
        private void buttonNewPoint_Click(object sender, EventArgs e)
        {
            try
            {
                Halcon.HobjClear();
                if (Halcon.Drawing)
                {
                    return;
                }
                if (Wire_S.AOIObj != null && Wire_S.AOIObj.CountObj() == 1)
                {
                    Halcon.Drawing = true;
                    HOperatorSet.DragRegion1(Wire_S.AOIObj, out HObject hObject, Halcon.hWindowHalcon());
                    Halcon.AddObj(hObject);
                    HOperatorSet.ReduceDomain(Halcon.Image(), hObject, out hObjectImage);
                    Wire_S.listWelding.Add(new Wire_Solder.Wire_S() { HObject = hObject, });
                    listBoxPoints.Items.Clear();
                    for (int i = 0; i < Wire_S.listWelding.Count; i++)
                    {
                        listBoxPoints.Items.Add(i + 1);
                    }
                    Set_Pragram(1);
                    Halcon.ShowImage();
                }
                else
                {
                    Halcon.AddMeassge("未绘制添加区域");
                }
                Halcon.ShowObj();
            }
            catch (Exception ex)
            {
            }
            Halcon.Drawing = false;

        }

        private void buttonMovePoint_Click(object sender, EventArgs e)
        {
            try
            {
                Halcon.HobjClear();
                Wire.HObject = RunProgram.DragMoveOBJ(Halcon, Wire.HObject);
                Halcon.AddObj(Wire.HObject);
                Halcon.ShowObj();
            }
            catch (Exception)
            {

            }
        }

        private void buttonDrawPoint_Click(object sender, EventArgs e)
        {
            try
            {

                Wire_S.AOIObj = RunProgram.DrawModOBJ(Halcon, HalconRun.EnumDrawType.Rectangle2, Wire_S.AOIObj);
            }
            catch (Exception)
            {
            }
        }

        private void buttonPointWipe_Click(object sender, EventArgs e)
        {
            try
            {

                Wire_S.listWelding.Remove(Wire);
                listBoxPoints.Items.Clear();
                for (int i = 0; i < Wire_S.listWelding.Count; i++)
                {
                    listBoxPoints.Items.Add(i + 1);
                }


            }
            catch (Exception)
            {
            }
        }

        private void Set1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ismove = true;
                Wire = Wire_S.listWelding[listBoxPoints.SelectedIndex];

                dataGridView1.Rows.Clear();
                for (int i = 0; i < Wire.List3DName.Count; i++)
                {
                    int det = dataGridView1.Rows.Add();
                    if (Wire.List3DName[i] >= 0)
                    {
                        dataGridView1.Rows[det].Cells[0].Value = Project.formula.RecipeCompiler.Instance.Data.ListDatV[Wire.List3DName[i]].ComponentName;
                    }
                }
                ismove = false;
            }
            catch (Exception)
            {
            }
            Set_Pragram(1);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Halcon.HobjClear();
                Wire_S.DrawObj = RunProgram.DrawModOBJ(Halcon, HalconRun.EnumDrawType.Rectangle2, Wire_S.DrawObj);

            }
            catch (Exception)
            {
            }
        }

        private void Set2ValueChanged(object sender, EventArgs e)
        {

            Set_Pragram(2);

        }



        private void button2_Click(object sender, EventArgs e)
        {
            Halcon.HobjClear();
            Halcon.AddObj(Wire_S.DrawObj);
            Halcon.ShowObj();
        }


        private void Set3_ValueChanged(object sender, EventArgs e)
        {
            Set_Pragram(2);
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            Set_Pragram(2);
        }




        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (ismove)
            {
                return;
            }
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                if (Wire.List3DName.Count <= e.RowIndex)
                {
                    Wire.List3DName.Add(-1);
                }
                DataGridViewCell acell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                DataGridViewComboBoxColumnEx acombbox = acell.Value as DataGridViewComboBoxColumnEx;
                DataGridViewComboBoxCell acombboxt = acell as DataGridViewComboBoxCell;
                Wire.List3DName[e.RowIndex] = this.Column1.Items.IndexOf(this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedCells[0].RowIndex <= dataGridView1.Rows.Count)
                {
                    Wire.List3DName.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
    }
}
