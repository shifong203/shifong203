using System;
using System.Windows.Forms;

namespace ErosSocket.ErosUI
{
    public partial class 曲线窗口 : Form
    {
        public 曲线窗口()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        private float[] GetRandomValueByCount(int count, float min, float max)
        {
            Random random = new Random();
            float[] data = new float[count];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (float)random.NextDouble() * (max - min) + min;
            }
            return data;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
        }
    }
}