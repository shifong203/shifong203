using System;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class RunAdminForm : Form
    {
        public static RunAdminForm FormThis;

        public RunAdminForm()
        {
            InitializeComponent();
            FormThis = this;
            UpData();
        }

        private void UpData()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            //System.Configuration.SettingsPropertyCollection settings = Properties.Settings.Default.Properties;
            //foreach (System.Configuration.SettingsProperty item in settings)
            //{
            //    i = dataGridView1.Rows.Add();
            //    dataGridView1.Rows[i].Cells[0].Value = item.Name;
            //    dataGridView1.Rows[i].Cells[1].Value = item.PropertyType.ToString();
            //    dataGridView1.Rows[i].Cells[2].Value = Properties.Settings.Default[item.Name];
            //}
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            UpData();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                //for (int i = 0; i < dataGridView1.Rows.Count; i++)
                //{
                //forStrat:
                //    if (dataGridView1.Rows[i].Cells[0].Value != null && dataGridView1.Rows[i].Cells[0].Value.ToString() != "")
                //    {
                //        System.Configuration.SettingsProperty settingsProperty = new System.Configuration.SettingsProperty(dataGridView1.Rows[i].Cells[0].Value.ToString());
                //        settingsProperty.PropertyType = Type.GetType(dataGridView1.Rows[i].Cells[1].Value.ToString());
                //        foreach (System.Configuration.SettingsProperty item in Properties.Settings.Default.Properties)
                //        {
                //            if (dataGridView1.Rows[i].Cells[0].Value.ToString() == item.Name.ToString())
                //            {
                //                Properties.Settings.Default[item.Name] = dataGridView1.Rows[i].Cells[2].Value;
                //                i++;
                //                goto forStrat;
                //            }
                //        }
                //        Properties.Settings.Default.Properties.Add(settingsProperty);
                //    }
                //}
                //Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RunAdminForm_Load(object sender, EventArgs e)
        {
        }
    }
}