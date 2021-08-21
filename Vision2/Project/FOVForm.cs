using System;
using System.Windows.Forms;

namespace Vision2.Project
{
    public partial class FOVForm : Form
    {
        public FOVForm()
        {
            InitializeComponent();
        }

        private void FOVForm_Load(object sender, EventArgs e)
        {
            try
            {
                label1.Text = "";
                foreach (var item in Vision2.vision.Vision.Instance.RunCams)
                {
                    label1.Text += item.Key + "镜头信息:" + item.Value.FOV + Environment.NewLine + Environment.NewLine;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}