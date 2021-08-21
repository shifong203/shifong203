using System;
using System.Drawing;
using System.IO;
using System.Text;
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

                string ds = mesInfon1.GetSocketClint().AlwaysRece(10000);
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
                string ds = mesInfon1.GetSocketClint().AlwaysRece(10000);
                richTextBox1.AppendText(ds + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SisfForm1_Load(object sender, EventArgs e)
        {
            try
            {
                mesInfon1.GetSocketClint().PassiveStringBuilderEvent += SisfForm1_PassiveStringBuilderEvent;
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    listBox1.Items[i].ToString().Contains(mesInfon1.SISFVersions);
                    listBox1.SelectedItem = listBox1.Items[i];
                    break;
                }
            }
            catch (Exception)
            {
            }
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                mesInfon1.WrietMes(DebugF.DebugCompiler.GetTray(0).GetTrayData());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                mesInfon1.WrietMes(DebugF.DebugCompiler.GetTray(0).GetTrayData());
                //richTextBox2.AppendText(mesInfon1.Sisf2(DebugF.DebugCompiler.GetTray(0).GetTrayData()) + Environment.NewLine);
                //string ds = mesInfon1.GetSocketClint().AlwaysRece(10000);
                //richTextBox1.AppendText(ds + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}