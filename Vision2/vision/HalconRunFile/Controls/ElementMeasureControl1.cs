using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class ElementMeasureControl1 : UserControl
    {
        public ElementMeasureControl1(Waves waves, HalconRun run)
        {
            InitializeComponent();
            wa = waves;
            halcon = run;
        }

        private Waves wa;
        private HalconRun halcon;

        public void Up()
        {
            listBox1.Items.Clear();
            foreach (var item in wa.Dic_Measure.Keys_Measure)
            {
                listBox1.Items.Add(item.Value.Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wa.ShowMesager(halcon);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    return;
                }
                if (this.wa.Dic_Measure.Keys_Measure.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    measureConTrolEx1.Updata(this.wa.Dic_Measure.Keys_Measure[listBox1.SelectedItem.ToString()], halcon);

                    this.wa.Dic_Measure.Keys_Measure[listBox1.SelectedItem.ToString()].ShowMeasure(halcon);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}