using ErosSocket.ErosConLink;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ErosSocket
{
    public partial class SocketServerForm : Form
    {
        public SocketServerForm(SocketServer socket = null)
        {
            InitializeComponent();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(SocketClint.GetEncodingNamese.ToArray());
            comboBox1.Items.AddRange(SocketClint.GetEncodingNamese.ToArray());

            if (socket == null)
            {
                StaticCon.Server = new SocketServer();
                socket = StaticCon.Server;
            }
            comboBox1.SelectedItem = socket.GetEncoding().HeaderName;
            comboBox2.SelectedItem = socket.GetEncoding().HeaderName;

            Server = socket;
            textIP.Text = Server.EndIP;
            NumInputProt.Value = Server.EndPort;
            label6.Text = "状态:" + Server.LinkState;
            nuSocketPort.Value = Server.LinkMaxNunber;
        }
        SocketServer Server;
        private void buteMonitor_Click(object sender, EventArgs e)
        {
            try
            {
                label6.Text = "状态:尝试监听";
                Server.EndIP = textIP.Text;
                Server.LinkMaxNunber = (byte)nuSocketPort.Value;
                Server.EndPort = Convert.ToUInt16(NumInputProt.Value);
                Server.AsynLink(false);
                label6.Text = "状态:" + Server.LinkState;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 服务器发送到客户端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null && Server.GetDictiPoints().ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    string datas = textBox2.Text + Server.GetCRLF();

                    byte[] bytes = Encoding.GetEncoding(comboBox1.SelectedItem.ToString()).GetBytes(datas);
                    Server.GetDictiPoints()[listBox1.SelectedItem.ToString()].Send(bytes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormCon_Load(object sender, EventArgs e)
        {
            try
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                if (Server == null)
                {
                    Server = new SocketServer();
                }
                Server.PassiveEvent += larddata;
                Server.NewLink += newLink;
                lbNumberLink.Text = "链接数：" + Server.GetDictiPoints().Count;
                listBox1.Items.AddRange(Server.GetDictiPoints().Keys.ToArray());
                for (int i = 0; i < Server.linkInformation.Count; i++)
                {
                    txtRead.Text += Server.linkInformation[i] + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string larddata(byte[] key, SocketClint socketClint, Socket socket)
        {
            try
            {
                if (comboBox1.SelectedItem != null)
                {
                    string da = Encoding.GetEncoding(comboBox1.SelectedItem.ToString()).GetString(key) + Environment.NewLine;
                    if (txtRead != null && !txtRead.IsDisposed)
                    {
                        txtRead.AppendText(DateTime.Now.ToString("HH时mm分ss秒:") + da);
                        txtRead.ScrollToCaret();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return "";
        }

        private string newLink(byte[] data)
        {
            try
            {
                if (txtRead != null && !txtRead.IsDisposed)
                {
                    txtRead.AppendText(Encoding.UTF8.GetString(data) + DateTime.Now.TimeOfDay.ToString() + Environment.NewLine);
                    txtRead.ScrollToCaret();
                    listBox1.Items.Clear();
                    foreach (var item in Server.GetDictiPoints().Keys)
                    {
                        listBox1.Items.Add(item);
                    }
                    lbNumberLink.Text = "链接数：" + Server.GetDictiPoints().Count;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    MessageBox.Show("请选择目标端口");
                    return;
                }
                Server.GetNameSocket(listBox1.SelectedItem.ToString()).Send(Encoding.GetEncoding(comboBox2.Text).GetBytes(textBox2.Text));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null && Server.GetDictiPoints().ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    richTextBox1.AppendText(Server.GetSendStr(listBox1.SelectedItem.ToString()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Server.Close();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            txtRead.Clear();
        }

        private void SocketServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Server.PassiveEvent -= larddata;
            Server.NewLink -= newLink;
        }
    }
}