using System;
using System.Windows.Forms;

namespace Vision2.vision
{
    public partial class VisonForm1 : Form
    {
        public VisonForm1()
        {
            InitializeComponent();
        }

        private void VisonForm1_Load(object sender, EventArgs e)
        {
            try
            {
                checkedListBox1.Items.Clear();
                foreach (var item in Vision.Instance.VisionPr)
                {
                    checkedListBox1.Items.Add(item.Key, item.Value);

                }
            }
            catch (Exception)
            {
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void VisonForm1_FormClosing(object sender, FormClosingEventArgs e)
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
