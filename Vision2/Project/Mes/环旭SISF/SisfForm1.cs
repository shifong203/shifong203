using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace Vision2.Project.Mes.环旭SISF
{
    public partial class SisfForm1 : Form
    {
        public SisfForm1()
        {
            InitializeComponent();
        }

        private SISF mesInfon1;

        public SisfForm1(SISF mesInfon) : this()
        {
            mesInfon1 = mesInfon as SISF;
            propertyGrid1.SelectedObject = mesInfon1;
            listBox1.Items.Clear();
            try
            {
                string[] strd = Directory.GetFiles(ProjectINI.ProjectPathRun + "\\Mes");
                for (int i = 0; i < strd.Length; i++)
                {
                    listBox1.Items.Add(Path.GetFileName(strd[i]));
                }
            }
            catch (Exception)
            {
            }
            //listBox1.Items.Add("芯片扫码改造V1");

            //listBox1.Items.Add("Presasm1_SPEC");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox2.AppendText(mesInfon1.SendStep7(textBox1.Text) + Environment.NewLine);

                string ds = mesInfon1.GetSocketClint().AlwaysReceive(10000);
                richTextBox1.AppendText(ds + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                mesInfon1.GetSocketClint().Send(textBox4.Text);
                richTextBox2.AppendText(DateTime.Now + "S:" + textBox4.Text + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                mesInfon1.GetSocketClint().Send(textBox6.Text);
                richTextBox2.AppendText(DateTime.Now + "S:" + textBox6.Text + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox2.AppendText(mesInfon1.SendStep2(textBox1.Text, textBox2.Text, textBox3.Text) + Environment.NewLine);
                string ds = mesInfon1.GetSocketClint().AlwaysReceive(10000);
                richTextBox1.AppendText(ds + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool isCahtrv = false;

        private void SisfForm1_Load(object sender, EventArgs e)
        {
            try
            {
                isCahtrv = true;
                ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
                ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView2);
                for (int i = 0; i < mesInfon1.ListStr.Count; i++)
                {
                    int dset = dataGridView1.Rows.Add();
                    dataGridView1.Rows[dset].Cells[0].Value = mesInfon1.ListStr[i];
                }
                for (int i = 0; i < mesInfon1.FixtureList.Count; i++)
                {
                    int dset = dataGridView2.Rows.Add();
                    dataGridView2.Rows[dset].Cells[0].Value = mesInfon1.FixtureList[i];
                }
                mesInfon1.GetSocketClint().PassiveStringBuilderEvent += SisfForm1_PassiveStringBuilderEvent;
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    listBox1.Items[i].ToString().Contains(mesInfon1.SISFVersions);
                    listBox1.SelectedItem = listBox1.Items[i];
                    break;
                }
                if (mesInfon1.SISFVersions == "Presasm1_SPEC")
                {
                    button1.Enabled = false;
                    button2.Enabled = false;
                }
                else
                {
                }
            }
            catch (Exception)
            {
            }
            isCahtrv = false;
        }

        private string SisfForm1_PassiveStringBuilderEvent(StringBuilder key, ErosSocket.ErosConLink.SocketClint socket, System.Net.Sockets.Socket socketR)
        {
            try
            {
                richTextBox1.AppendText(DateTime.Now.ToString() + "接收:" + key.ToString() + socket.RecivesDone + Environment.NewLine);
            }
            catch (Exception)
            {
            }
            return "";
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ProjectINI.In.UserID = textBox7.Text;
            }
            catch (Exception)
            {
            }
        }

        private void SisfForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                mesInfon1.GetSocketClint().PassiveStringBuilderEvent -= SisfForm1_PassiveStringBuilderEvent;
            }
            catch (Exception)
            {
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.Image = Image.FromFile(ProjectINI.ProjectPathRun + "\\Mes\\" + listBox1.SelectedItem.ToString());
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception)
            {
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Thread thread = new Thread(() =>
                {
                    mesInfon1.WrietMes(DebugF.DebugCompiler.GetTray(0).GetTrayData());
                });

                thread.IsBackground = true;
                thread.Start();

                //richTextBox2.AppendText(mesInfon1.Sisf2(DebugF.DebugCompiler.GetTray(0).GetTrayData()) + Environment.NewLine);
                //string ds = mesInfon1.GetSocketClint().AlwaysRece(10000);
                //richTextBox1.AppendText(ds + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isCahtrv)
                {
                    return;
                }
                mesInfon1.ListStr.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value != null)
                    {
                        mesInfon1.ListStr.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isCahtrv)
                {
                    return;
                }
                mesInfon1.FixtureList.Clear();
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Cells[0].Value != null)
                    {
                        mesInfon1.FixtureList.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                mesInfon1.RestData(textBox8.Text, DebugF.DebugCompiler.GetTray(0).GetTrayData());
            }
            catch (Exception)
            {
            }
        }
    }
}