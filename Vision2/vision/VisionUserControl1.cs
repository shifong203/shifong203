using System;
using System.Windows.Forms;

namespace Vision2.vision
{
    public partial class VisionUserControl1 : UserControl
    {
        public VisionUserControl1()
        {
            InitializeComponent();

            //propertyGrid1.SelectedObject = Vision.Instance.GetSerPort();
        }

        public VisionUserControl1(vision.Vision vision) : this()
        {
            Vision = vision;
        }

        private Vision Vision;

        private bool iscont = false;

        private void SetP()
        {
            try
            {
                if (iscont)
                {
                    return;
                }
                iscont = true;
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
                checkedListBox1.Items.Clear();
                ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);

                foreach (var item in Vision.GetHimageList())
                {
                    if (!Vision.Instance.VisionList.ContainsKey(item.Key))
                    {
                        Vision.Instance.VisionList.Add(item.Key, true);
                    }
                }
                foreach (var item in Vision.Instance.VisionList)
                {
                  int dset=  dataGridView1.Rows.Add();
                    dataGridView1.Rows[dset].Cells[0].Value = item.Key;
                    dataGridView1.Rows[dset].Cells[1].Value = item.Value;
                }
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