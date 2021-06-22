using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.捷普
{
    public partial class MForm : Form
    {
        public MForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Project.formula.UserFormulaContrsl. StaticAddQRCode(textBox1.Text);
            Project.DebugF.DebugCompiler.Start();
            tabControl1.SelectedIndex = 1;
        }

        private void MForm_Load(object sender, EventArgs e)
        {
            try
            {
                vision.Vision.GetRunNameVision().EventDoen += MForm_EventDoen;
                timer1.Interval = 100;
                timer1.Start();


            }
            catch (Exception ex)
            {
            }
        }

        private void RunCodeT_RunDone(Project.DebugF.IO.RunCodeStr.RunErr key)
        {

            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[8].Value = key.RunTime;
                }
            }
            catch (Exception)
            {
            }
        }

        private void MForm_EventDoen(vision.OneResultOBj oneResultO)
        {
            try
            {
                dataGridView1.Rows.Clear();
                foreach (var item in oneResultO.GetNgOBJS().DicOnes)
                {
                    foreach (var itemdt in item.Value.oneRObjs)
                    {
                        int index = dataGridView1.Rows.Add();
                        if (itemdt.dataMinMax.GetRsetOK())
                        {
                            dataGridView1.Rows[index].Cells[2].Value = "Pass";
                        }
                        else
                        {
                            dataGridView1.Rows[index].Cells[2].Value = "Fill";
                        }
                        dataGridView1.Rows[index].Cells[1].Value = item.Value.ComponentID;
                        dataGridView1.Rows[index].Cells[1].Value = itemdt.ComponentID;
                        dataGridView1.Rows[index].Cells[5].Value =vision.Vision.Instance.TransformName;
                        dataGridView1.Rows[index].Cells[3].Value = itemdt.dataMinMax.Reference_Name[0];
                        dataGridView1.Rows[index].Cells[4].Value = itemdt.dataMinMax.ValueStrs[0];
                        dataGridView1.Rows[index].Cells[6].Value = itemdt.dataMinMax.Reference_ValueMin[0];
                        dataGridView1.Rows[index].Cells[7].Value = itemdt.dataMinMax.Reference_ValueMax[0];
                    }
                }

            }
            catch (Exception)
            {
            }
        }

        private void MForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
      Project.DebugF.DebugCompiler.GetThis().DDAxis.RunCodeT.RunDone += RunCodeT_RunDone;
        }
    }
}
