using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class ThresholdControls : UserControl
    {
        public delegate void EventValue(Threshold_Min_Max threshold_Min_);

        public event EventValue evValue;
        public ThresholdControls()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(Enum.GetNames(typeof(ImageTypeObj)));
        }
        public ThresholdControls(Threshold_Min_Max threshold_Min_) : this()
        {
            threshold_Min_1 = threshold_Min_;

        }
        public void SetData(Threshold_Min_Max threshold_Min_)
        {
            threshold_Min_1 = threshold_Min_;

            GetPret();
        }
        bool isCave;
        public void GetPret()
        {
            isCave = true;
            try
            {
                comboBox1.SelectedItem = threshold_Min_1.ImageTypeObj.ToString();
                numericUpDownThrMax.Value = threshold_Min_1.Max;
                numericUpDownThrMin.Value = threshold_Min_1.Min;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isCave = false;
        }
        
        public void SetPret()
        {
            if (isCave)
            {
                return;
            }
            try
            {
                threshold_Min_1.ImageTypeObj = (ImageTypeObj)Enum.Parse(typeof(ImageTypeObj), comboBox1.SelectedItem.ToString());
                threshold_Min_1.Max =(byte) numericUpDownThrMax.Value;
                threshold_Min_1.Min = (byte)numericUpDownThrMin.Value;
                evValue?.Invoke(threshold_Min_1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ThresholdControls_Load(object sender, System.EventArgs e)
        {
            comboBox1.Items.AddRange(Enum.GetNames(typeof(ImageTypeObj)));
        }
        Threshold_Min_Max threshold_Min_1;

        private void numericUpDownThrMax_ValueChanged(object sender, EventArgs e)
        {
            SetPret();
        }
    }
}
