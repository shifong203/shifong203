using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace Vision2.vision
{
    public partial class VisionUserControl1 : UserControl
    {
        public VisionUserControl1()
        {
            InitializeComponent();

            propertyGrid1.SelectedObject = Vision.Instance.GetSerPort();

        }
        public VisionUserControl1(vision.Vision vision) : this()
        {
            Vision = vision;
            iscont = true;
            trackBar1.Value = Vision.H1;
            trackBar2.Value = Vision.H2;
            trackBar3.Value = Vision.H3;
            trackBar4.Value = Vision.H4;
            numericUpDown1.Value = Vision.H1;
            numericUpDown2.Value = Vision.H2;
            numericUpDown3.Value = Vision.H3;
            numericUpDown4.Value = Vision.H4;
            checkBox1.Checked = Vision.H1Off;
            checkBox2.Checked = Vision.H2Off;
            checkBox3.Checked = Vision.H3Off;
            checkBox4.Checked = Vision.H4Off;



            iscont = false;
        }
        Vision Vision;
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SetP();
        }
        bool iscont = false;
        void SetP()
        {
            try
            {
                if (iscont)
                {
                    return;
                }
                iscont = true;
                Vision.H1 = (byte)trackBar1.Value;
                Vision.H2 = (byte)trackBar2.Value;
                Vision.H3 = (byte)trackBar3.Value;
                Vision.H4 = (byte)trackBar4.Value;
                numericUpDown1.Value = Vision.H1;
                numericUpDown2.Value = Vision.H2;
                numericUpDown3.Value = Vision.H3;
                numericUpDown4.Value = Vision.H4;
                Vision.H1Off = checkBox1.Checked;
                Vision.H2Off = checkBox2.Checked;
                Vision.H3Off = checkBox3.Checked;
                Vision.H4Off = checkBox4.Checked;
                Vision.SetHx();

            }
            catch (Exception)
            {


            }
            iscont = false;


        }

        private void VisionUserControl1_Load(object sender, EventArgs e)
        {
            try
            {
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(SerialPort.GetPortNames());
                comboBox2.SelectedItem = Vision.Rs232Name;
                checkedListBox1.Items.Clear();
                foreach (var item in Vision.Instance.VisionPr)
                {
                    checkedListBox1.Items.Add(item.Key, item.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Vision.H1Off = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Vision.H2Off = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Vision.H3Off = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Vision.H4Off = checkBox4.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                SetP();
            }
            catch (Exception)
            {
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Vision.Rs232Name != comboBox2.SelectedItem.ToString())
                {
                    Vision.GetSerPort().Close();
                }
                Vision.Rs232Name = comboBox2.SelectedItem.ToString();
                Vision.H4Off = Vision.H3Off = Vision.H2Off = Vision.H1Off = true;
                iscont = true;
                checkBox1.Checked = Vision.H1Off;
                checkBox2.Checked = Vision.H2Off;
                checkBox3.Checked = Vision.H3Off;
                checkBox4.Checked = Vision.H4Off;
                Vision.SetHx();
            }
            catch (Exception)
            {
            }
            iscont = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Vision.H4Off = Vision.H3Off = Vision.H2Off = Vision.H1Off = false;
                iscont = true;
                checkBox1.Checked = Vision.H1Off;
                checkBox2.Checked = Vision.H2Off;
                checkBox3.Checked = Vision.H3Off;
                checkBox4.Checked = Vision.H4Off;
                Vision.SetHx();
            }
            catch (Exception)
            {


            }
            iscont = false;
        }



        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    Vision.Instance.VisionPr[checkedListBox1.Items[i].ToString()] = checkedListBox1.GetItemChecked(i);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
