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
                for (int i = 0; i < overgild.RunListOvergil.Count; i++)
                {
                    listBox1.Items.Add(i + 1);
                }
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(Enum.GetNames(typeof(ImageTypeObj)));
                comboBox1.SelectedItem = overgild.ImageTypeOb.ToString();
                numericUpDown1.Value = (decimal)overgild.ErosinCircle;
                numericUpDown4.Value = (decimal)overgild.ThresSelectMin;
                numericUpDown5.Value = (decimal)overgild.ThresSelectMax;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isChaved = false;
        }
        bool isChaved = true;
        Overgild overgild;
        HalconRun halcon;
        private void button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                //RunProgram.DrawHoj()


            }
            catch (Exception)
            {
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                listBox1.Items.Add(listBox1.Items.Count+1);
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
                if (listBox1.SelectedItem==null)
                {
                    return;
                }
                overgild.GetPThis().HobjClear();
                groupBox3.Text = listBox1.SelectedItem.ToString();
                propertyGrid2.SelectedObject = overgild.RunListOvergil[listBox1.SelectedIndex];
                overgild.RunSeleRoi(halcon.GetOneImageR(), 1,out HalconDotNet.HObject err);
                overgild.RunListOvergil[listBox1.SelectedIndex].RunPa(halcon.GetOneImageR(), overgild, out HalconDotNet.HObject hObject);
                halcon.AddObj(hObject);
                overgild.GetPThis().ShowObj();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SetDatd(int id=0)
        {
            try
            {
                if (isChaved)
                {
                    return;
                }
                overgild.ImageTypeOb = (ImageTypeObj)Enum.Parse(typeof(ImageTypeObj),
                comboBox1.SelectedItem.ToString());
                overgild.ThresSelectMin =(byte) numericUpDown4.Value;
                overgild.ErosinCircle = (double)numericUpDown1.Value;
               overgild.ThresSelectMax = (byte)numericUpDown5.Value;
                halcon.HobjClear();
                overgild.RunSeleRoi(halcon.GetOneImageR(), 1, out HalconDotNet.HObject err);
                halcon.AddObj(err);
                overgild.GetPThis().ShowObj();
                halcon.ShowObj();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            SetDatd();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
           
                halcon.HobjClear();
                overgild.Run( halcon.GetOneImageR());
                overgild.ModeOBj = overgild.SelecRoi;
                halcon.AddObj(overgild.ModeOBj);
                halcon.ShowObj();

            }
            catch (Exception)
            {
            }
        }
    }
}
