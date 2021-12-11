using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class OvergildControl1 : UserControl
    {
        public OvergildControl1()
        {
            InitializeComponent();
        }

        public OvergildControl1(Overgild pingZheng) : this()
        {
            overgild = pingZheng;
            halcon = overgild.GetPThis();
            propertyGrid1.SelectedObject = overgild;
            try
            {
                this.checkBox3.Checked = overgild.EnableScratch;
                this.checkBox4.Checked = overgild.EnableSymmetry;
                //this.checkBox5.Checked = overgild.EnableSymmetry;
                for (int i = 0; i < overgild.RunListOvergil.Count; i++)
                {
                    listBox1.Items.Add(i + 1);
                }
                thresholdControls1.SetData(overgild.threshold_Min_Max);
                select_obj_type1.SetData(overgild.Select_Shape_Min_Max);
                select_obj_type2.SetData(overgild.Select_Shape_Min_Outobj);
                numericUpDown7.Value =(decimal) overgild.ColsingSocek;
                //comboBox1.Items.Clear();

                //comboBox1.Items.AddRange(Enum.GetNames(typeof(ImageTypeObj)));
                //comboBox1.SelectedItem = overgild.ImageTypeOb.ToString();
                textBox1.Text = overgild.Defect_Type;
                numericUpDown1.Value = (decimal)overgild.DilationCircle;
                numericUpDown2.Value = (decimal)overgild.ErosinCircle;
                numericUpDown3.Value = (decimal)overgild.DnyValue;
                numericUpDown4.Value = (decimal)overgild.MeanHeith;
                numericUpDown5.Value = (decimal)overgild.MeanWidth;
                domainUpDown1.Text = overgild.DnyTypeValue;
                checkBox2.Checked = overgild.IsDisObj;
                //numericUpDown4.Value = (decimal)overgild.ThresSelectMin;
                //numericUpDown5.Value = (decimal)overgild.ThresSelectMax;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isChaved = false;
        }

        private bool isChaved = true;
        private Overgild overgild;
        private HalconRun halcon;

        private void button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                overgild.Run(halcon.GetOneImageR(), new AoiObj());
                halcon.HobjClear();
                halcon.AddObj(overgild.SelecRoi);
                halcon.ShowObj();
            }
            catch (Exception)
            {
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                listBox1.Items.Add(listBox1.Items.Count + 1);
                overgild.RunListOvergil.Add(new Overgild.OvergilEX());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                overgild.RunListOvergil.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    return;
                }
                overgild.GetPThis().HobjClear();
                groupBox3.Text = listBox1.SelectedItem.ToString();
                propertyGrid2.SelectedObject = overgild.RunListOvergil[listBox1.SelectedIndex];
                overgild.RunSeleRoi(halcon.GetOneImageR(), 1, out HalconDotNet.HObject err);
                overgild.RunListOvergil[listBox1.SelectedIndex].RunPa(halcon.GetOneImageR(), overgild, out HalconDotNet.HObject hObject);
                halcon.AddObj(hObject);
                overgild.GetPThis().ShowObj();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetDatd(int id = 0)
        {
            try
            {
                if (isChaved)
                {
                    return;
                }
                overgild.ColsingSocek =(double) numericUpDown7.Value;
                overgild.EnableScratch = checkBox3.Checked;
                overgild.EnableSymmetry = checkBox4.Checked;
                overgild.EnableRB = checkBox5.Checked;
                //overgild.ImageTypeOb = (ImageTypeObj)Enum.Parse(typeof(ImageTypeObj),
                //comboBox1.SelectedItem.ToString());
                //overgild.ThresSelectMin = (byte)numericUpDown4.Value;
                overgild.IsDisObj = checkBox2.Checked;
                overgild.Defect_Type = textBox1.Text;
                overgild.ErosinCircle = (double)numericUpDown2.Value;
                overgild.DilationCircle = (double)numericUpDown1.Value;
                overgild.ISAoiMode = checkBox1.Checked;
                overgild.DnyValue = (double)numericUpDown3.Value;
                overgild.MeanHeith = (double)numericUpDown4.Value;
                overgild.MeanWidth = (double)numericUpDown5.Value;

                overgild.DnyTypeValue = domainUpDown1.Text;
                //overgild.ThresSelectMax = (byte)numericUpDown5.Value;
                halcon.HobjClear();
                overgild.RunSeleRoi(halcon.GetOneImageR(), id, out HalconDotNet.HObject err);
                halcon.AddObj(err);
                overgild.GetPThis().ShowObj();
                halcon.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void select1(object sender, System.EventArgs e)
        {
            SetDatd(2);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                overgild.Run(halcon.GetOneImageR());
                overgild.ModeOBj = overgild.SelecRoi;
                halcon.AddObj(overgild.ModeOBj);
                halcon.ShowObj();
            }
            catch (Exception)
            {
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                overgild.AOIObj = RunProgram.DrawHObj(halcon, overgild.AOIObj);
            }
            catch (Exception)
            {
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                overgild.AOIObj = RunProgram.DrawRmoveObj(halcon, overgild.AOIObj);
            }
            catch (Exception)
            {
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetDatd(1);
        }

        private void Sele3(object sender, EventArgs e)
        {
            SetDatd(3);
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            SetDatd(2);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetDatd(listBox2.SelectedIndex + 1);
            }
            catch (Exception)
            {
            }
        }

        private void thresholdControls1_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SetDatd(1);
            }
            catch (Exception)
            {
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetDatd(1);
            }
            catch (Exception)
            {
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            SetDatd(1);
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            SetDatd(4);
        }
    }
}