using System;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC.Robot
{
    public partial class SetToolForm1 : Form
    {
        public SetToolForm1()
        {
            InitializeComponent();
        }

        public void SET(EpsenRobot6 epsenRobot6)
        {
            try
            {
                epsen = epsenRobot6;
                epsen.SendCommand("GetTools");

                System.Threading.Thread.Sleep(2000);
                string ds = epsen.ReciveStr[epsen.ReciveStr.Count - 1];
                string[] data = ds.Split('|');
                for (int i = 0; i < 8; i++)
                {
                    DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                    dataGridViewTextBoxColumn.Name = i.ToString();
                    dataGridViewTextBoxColumn.HeaderText = i.ToString();
                    dataGridView1.Columns.Add(dataGridViewTextBoxColumn);
                }
                dataGridView1.Rows.Add(15);

                for (int i = 0; i < data.Length; i++)
                {
                    string[] dataT = data[i].Split(',');
                    for (int i2 = 0; i2 < dataT.Length; i2++)
                    {
                        dataGridView1.Rows[i].Cells[i2].Value = dataT[i2];
                    }
                }
                textBox3.Text = 0.ToString();
            }
            catch (Exception ex)
            {
            }
        }

        private EpsenRobot6 epsen;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (epsen != null)
                {
                    //    if (textBox5.Text=="0"||textBox6.Text=="0")
                    //    {
                    //        epsen.SendCommand("SetTool",numericUpDown1.Value.ToString(),  textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                    //    }
                    //    else
                    //    {
                    epsen.SendCommand("SetTool", numericUpDown1.Value.ToString(), textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text);
                    //}
                }
            }
            catch (Exception)
            {
            }
        }
    }
}