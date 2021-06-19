using HalconDotNet;
using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.RunProgramFile.Measure;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class MeasureConTrolEx : UserControl
    {
        private HalconRun halconRun;
        private Measure measure;

        public MeasureConTrolEx(HalconRun halcon, Measure meas)
        {
            IsChanged = true;
            InitializeComponent();
            halconRun = halcon;
            measure = meas;
            Updata(meas, halcon);
        }
        public MeasureConTrolEx()
        {
            InitializeComponent();
        }

        private void 显示测量区域_Click(object sender, EventArgs e)
        {
            try
            {
                halconRun.HobjClear();
                halconRun.AddObj(measure.GetHamMatDraw());
                halconRun.ShowImage();
                halconRun.ShowObj();
            }
            catch (Exception)
            {
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                halconRun.HobjClear();
                measure.DrawObj(halconRun);
                halconRun.ShowImage();
                halconRun.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MeasureConTrolEx_Load(object sender, EventArgs e)
        {
        }

        public void Updata(Measure meas, HalconRun halcon)
        {
            try
            {
                IsChanged = true;
                halconRun = halcon;
                measure = meas;
                groupBox1.Text = meas.Name;
                trackBar1.Value = (int)measure.Sigma * 10;
                trackBar2.Value = (int)measure.Threshold;
                numericUpDown3.Value = (decimal)measure.Measure_Heigth;
                numericUpDown2.Value = (decimal)measure.MeasurePointNumber;
                label3.Text = "幅度:" + measure.Threshold;
                label1.Text = "平滑:" + measure.Sigma;
                switch (measure.SelectStr)
                {
                    case Measure.Select.all:
                        comboBox3.SelectedIndex = 0;

                        break;
                    case Measure.Select.first:
                        comboBox3.SelectedIndex = 1;
                        break;
                    case Measure.Select.last:
                        comboBox3.SelectedIndex = 2;
                        break;
                    default:
                        break;
                }
                switch (measure.TransitionStr)
                {
                    case Transition.all:
                        comboBox2.SelectedIndex = 0;
                        break;
                    case Transition.negative:
                        comboBox2.SelectedIndex = 1;
                        break;
                    case Transition.positive:
                        comboBox2.SelectedIndex = 2;
                        break;
                    default:
                        break;
                }
                comboBox1.Items.Clear();
                comboBox1.Items.Add(MeasureType.Measure);
                comboBox1.Items.Add(MeasureType.Cilcre);
                comboBox1.Items.Add(MeasureType.Line);
                comboBox1.Items.Add(MeasureType.Pake);
                comboBox1.Items.Add(MeasureType.Point);
                checkBox2.Checked = measure.Enabled;
                comboBox1.SelectedItem = meas.Measure_Type;
                checkBox1.Checked = meas.ISMatHat;
                propertyGrid1.SelectedObject = meas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            IsChanged = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                halconRun.HobjClear();
                measure.MeasureObj(halconRun, checkBox1.Checked, halconRun.GetOneImageR());
                halconRun.AddObj(measure.MeasureHObj);
                halconRun.ShowImage();
                halconRun.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                measure.DrawModObj(halconRun);
                halconRun.ShowImage();
                halconRun.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void 显示测量点_Click(object sender, EventArgs e)
        {
            try
            {
                halconRun.HobjClear();
                measure.ShowPoint(halconRun);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                halconRun.HobjClear();
                measure.ShowPstPoint(halconRun);
                halconRun.ShowImage();
                halconRun.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        bool IsChanged = false;
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                halconRun.HobjClear();
                Measure.Pts_Ling_Extension(measure.OutRows[0], measure.OutCols[0], measure.OutRows[1], measure.OutCols[1], (double)numericUpDown1.Value,
                   out double row, out double col);
                HTuple hTupleRow = new HTuple();
                HTuple hTupleCol = new HTuple();
                hTupleRow.Append(measure.OutRows);
                hTupleRow.Append(row);
                hTupleCol.Append(measure.OutCols);
                hTupleCol.Append(col);
                HOperatorSet.GenContourPolygonXld(out HObject hObject, hTupleRow, hTupleCol);
                halconRun.AddObj(hObject);
                halconRun.ShowImage();
                halconRun.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                halconRun.HobjClear();
                measure.ShowContPoint(halconRun);
                halconRun.ShowImage();
                halconRun.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsChanged)
            {
                return;
            }

            measure.Measure_Type = (MeasureType)Enum.Parse(typeof(MeasureType), comboBox1.SelectedItem.ToString());

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (IsChanged)
            {
                return;
            }
            measure.Enabled = checkBox2.Checked;
            measure.ISMatHat = checkBox1.Checked;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (IsChanged)
            {
                return;
            }
            measure.Sigma = (double)trackBar1.Value / 10;
            label1.Text = "平滑:" + measure.Sigma;

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (IsChanged)
            {
                return;
            }
            measure.Threshold = trackBar2.Value;
            label3.Text = "幅度:" + measure.Threshold;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsChanged)
            {
                return;
            }
            switch (comboBox2.SelectedIndex)
            {
                case 1:
                    measure.TransitionStr = Transition.negative;
                    break;
                case 2:
                    measure.TransitionStr = Transition.positive;
                    break;
                default:
                    measure.TransitionStr = Transition.all;
                    break;
            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsChanged)
            {
                return;
            }
            switch (comboBox3.SelectedIndex)
            {
                case 1:
                    measure.SelectStr = Measure.Select.first;
                    break;
                case 2:
                    measure.SelectStr = Measure.Select.last;
                    break;
                default:
                    measure.SelectStr = Measure.Select.all;
                    break;
            }

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (IsChanged)
            {
                return;
            }
            measure.MeasurePointNumber = (double)numericUpDown2.Value;

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (IsChanged)
            {
                return;
            }
            measure.Measure_Heigth = (double)numericUpDown3.Value;

        }
    }
}