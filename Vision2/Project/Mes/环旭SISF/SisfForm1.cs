using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        SISF mesInfon1;
        public SisfForm1(SISF mesInfon):this()
        {
            mesInfon1 = mesInfon as  SISF;
            propertyGrid1.SelectedObject = mesInfon1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox2.AppendText(mesInfon1.SendText1(textBox1.Text) + Environment.NewLine);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                mesInfon1.GetSocketClint().Send(textBox5.Text);

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
                richTextBox2.AppendText(mesInfon1.SendText2(textBox1.Text, textBox2.Text, textBox3.Text) + Environment.NewLine);
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

            }
            catch (Exception)
            {
            }
        }

        private string SisfForm1_PassiveStringBuilderEvent(StringBuilder key, ErosSocket.ErosConLink.SocketClint socket, System.Net.Sockets.Socket socketR)
        {
            try
            {

                richTextBox1.AppendText(DateTime.Now.ToString()+"接收:"+ key.ToString() + Environment.NewLine);

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
    }
}
