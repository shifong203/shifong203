using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace ErosSocket
{
    public partial class LocalIPForm : Form
    {
        public LocalIPForm()
        {
            InitializeComponent();
        }

        private void LocalIPForm1_Load(object sender, EventArgs e)
        {
            RefreshForm();
            PingIPS.Text = ErosConLink.SocketClint.LocalIP.ToString();
        }

        private List<string> listd = new List<string>();

        private void RefreshForm()
        {
            try
            {
                treeView1.Nodes.Clear();
                richTextBox1.AppendText(ErosConLink.SocketClint.GetAllLocalMachines(treeView1));
                richTextBox1.AppendText(ErosConLink.SocketClint.GetGateway(treeView1, tabControl1));
                treeView1.ExpandAll();
                this.Text = "TCPServer》》计算机名称：" + Dns.GetHostName();
                IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var IPadd in ipEntry.AddressList)
                {
                    //判断当前字符串是否为正确IP地址
                    //得到本地IP地址
                    if (SocketClint.IsValidateIPAddress(IPadd.ToString()))
                    {

                        //得到本地IP地址
                        this.Text += ";本地可链接IP:" + IPadd.ToString();
                        TextBoxPingIP.Text = IPadd.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshForm();
        }

        private List<string> ipList = new List<string>();

        public void getIP()
        {
            try
            {
                //获取本地机器名
                string _myHostName = Dns.GetHostName();
                string _myHostIP = string.Empty;
                //获取本机IP
                IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var IPadd in ipEntry.AddressList)
                {
                    //判断当前字符串是否为正确IP地址
                    //得到本地IP地址
                    if (ErosConLink.SocketClint.IsValidateIPAddress(IPadd.ToString()))
                    {
                        //得到本地IP地址
                        _myHostIP = IPadd.ToString();
                        break;
                    }
                }
                //截取IP网段
                string ipDuan = _myHostIP.Remove(_myHostIP.LastIndexOf('.'));
                //枚举网段计算机
                for (int i = 1; i <= 255; i++)
                {
                    Ping myPing = new Ping();
                    myPing.PingCompleted += new PingCompletedEventHandler(_myPing_PingCompleted);
                    string pingIP = ipDuan + "." + i.ToString();
                    myPing.SendAsync(pingIP, 1000);
                }
            }
            catch (Exception ex)
            {
            }
        }

        [Obsolete]
        private void _myPing_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            try
            {
                if (e.Reply.Status == IPStatus.Success)
                {
                    ipList.Add(e.Reply.Address.ToString());
                    TreeNode treeNode = treeView1.Nodes.Add(e.Reply.Address.ToString());
                    treeNode.Nodes.Add(Dns.GetHostByAddress(e.Reply.Address.ToString()).HostName.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string IPHead = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            ErosConLink.SocketClint.EnumComputers(PingIPS.Text, listd, richTextBox1);
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                toolStripButton3.ForeColor = Color.Black;

                if (SocketClint.IsPingIP(TextBoxPingIP.Text, out string textR))
                {
                    toolStripButton3.ForeColor = Color.Green;
                }
                else
                {
                    toolStripButton3.ForeColor = Color.Red;
                }
                richTextBox1.AppendText(textR + Environment.NewLine);
            }
            catch
            {
                //Ping失败
                toolStripButton3.ForeColor = Color.Red;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
    }
}