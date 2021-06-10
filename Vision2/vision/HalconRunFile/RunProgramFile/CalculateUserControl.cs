using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public partial class CalculateUserControl : UserControl
    {
        public CalculateUserControl()
        {
            InitializeComponent();
        }
        public CalculateUserControl(Calculate calculate) : this()
        {
            Calculate = calculate;
            Halcon = Calculate.GetPThis();
            numericUpDown3.DataBindings.Add("Value", Calculate, "thresholdHigh");
            numericUpDown4.DataBindings.Add("Value", Calculate, "thresholdLow");
        }

        private void DataBindings_CollectionChanging(object sender, CollectionChangeEventArgs e)
        {

        }

        private void DataBindings_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {

        }



        bool isChanged = true;
        HalconRun Halcon;
        Calculate Calculate;
        void SetPragram(int runid)
        {
            if (isChanged)
            {
                return;
            }
            Halcon.HobjClear();
            Calculate.Run(Halcon, Halcon.GetOneImageR(), runid);
            //Halcon.ShowImage();
            Halcon.ShowObj();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void CalculateUserControl_Load(object sender, EventArgs e)
        {
            isChanged = false;
        }

        private void numericUpDown4_MouseUp(object sender, MouseEventArgs e)
        {
            SetPragram(0);
        }

        private void numericUpDown3_Validated(object sender, EventArgs e)
        {

        }
    }
}
