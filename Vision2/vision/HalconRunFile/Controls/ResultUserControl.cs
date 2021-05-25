using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;
namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class ResultUserControl : UserControl
    {

        public ResultUserControl()
        {
            InitializeComponent();
        }
        HalconRun halconR;
        public void UP(HalconRun halcon)
        {
            if (this.Created)
            {
                this.Invoke(new Action(() =>
                {
                    upD(halcon);
                }));
            }
            else
            {
                upD(halcon);
            }
        }
        void upD(HalconRun halcon)
        {
            try
            {
                halconR = halcon;
                groupBox1.Text = halcon.Name;
                label1.Text = "结果:" + halcon.Result;
                label2.Text = "Time:" + halcon.RunTimeI;
                //label3.Text = "OK:" + halcon.OKNumber + ",NG:" + halcon.NGNumber;
                if (halcon.Buys)
                {
                    label4.Text = "状态:忙碌中";
                }
                else
                {
                    label4.Text = "状态:空闲";
                }
                listBox1.Items.Clear();

                foreach (var item in halcon.GetListResult())
                {
                    listBox1.Items.Add(item);
                }
                label5.Text = "历史结果:" + @"\" + halcon.GetListResult().Count;
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (halconR != null)
                {
                }
            }
            catch (Exception)
            {
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                halconR.GetListResult().Clear();
                listBox1.Items.Clear();
                foreach (var item in halconR.GetListResult())
                {
                    listBox1.Items.Add(item);
                }
                label5.Text = "历史结果:" + @"\" + halconR.GetListResult().Count;
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
            catch (Exception)
            {

            }

        }
    }
}
