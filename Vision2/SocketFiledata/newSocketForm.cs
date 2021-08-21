using ErosSocket.ErosConLink;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ErosSocket
{
    public partial class NewSocketForm : Form
    {
        public NewSocketForm()
        {
            InitializeComponent();
        }

        public NewSocketForm(TreeNode treeNode) : this()
        {
            Node = treeNode;
        }

        public NewSocketForm(SocketClint socket) : this()
        {
            Link = socket;

            Link.PassiveEvent += Socket_PassiveEvent;
            //Link.PassiveEvent += Link_PassiveTextBoxEvent;
        }

        private string Socket_PassiveEvent(byte[] key, SocketClint clint, Socket socket)
        {
            try
            {
                if (this.IsDisposed)
                {
                    Link.PassiveEvent -= Socket_PassiveEvent;
                    return "";
                }
                if (comboBox1.SelectedItem == null)
                {
                    comboBox1.SelectedItem = Link.GetEncoding().HeaderName;
                }
                txtR.AppendText(Encoding.GetEncoding(comboBox1.SelectedItem.ToString()).GetString(key) + Environment.NewLine);
            }
            catch (Exception)
            {
            }
            txtR.ScrollToCaret();
            return Encoding.UTF8.GetString(key);
        }

        private TreeNode Node;

        private SocketClint Link;

        private void newSocketForm_Load(object sender, EventArgs e)
        {
            try
            {
                Control.CheckForIllegalCrossThreadCalls = false;

                UpDATA();
                comboBoxLinkType.SelectedIndex = 0;
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                comboBox1.Items.AddRange(SocketClint.GetEncodingNamese.ToArray());
                comboBox2.Items.AddRange(SocketClint.GetEncodingNamese.ToArray());
                if (Link != null)
                {
                    comboBox1.SelectedItem = Link.GetEncoding().HeaderName;
                    txtSocketName.Text = Link.Name;
                    comboBox2.SelectedItem = Link.GetEncoding().HeaderName;
                    comboBoxLinkType.SelectedItem = Link.NetType;
                    txtOutIP.Text = Link.IP;
                    protOut.Value = Link.Port;
                }
            }
            catch (Exception)
            {
            }
        }

        private void cbbEvnet_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxLinkType.Text == null)
                {
                    return;
                }
                if (Link != null)
                {
                    Link.Dispose();
                }
                if (Link == null)
                {
                }
                Link = SocketClint.NewTypeLink(comboBoxLinkType.Text);

                Link.PassiveEvent += Socket_PassiveEvent;
                Link.Name = txtSocketName.Text;
                Link.IP = txtOutIP.Text;
                Link.Port = (ushort)protOut.Value;
                Link.EndIP = txtInIP.Text;
                Link.EndPort = (int)protInt.Value;

                Link.AsynLink(false);
                //Link.Link();
                //if (Link.LinkBool)
                //{
                //    Link.Receive();
                //}

                txtR.AppendText(Link.LinkState + Environment.NewLine);
                UpDATA();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                Link = new SocketClint();
                Link = SocketClint.NewTypeLink(comboBoxLinkType.Text);
                Link.PassiveEvent += Socket_PassiveEvent;
                Link.Name = txtSocketName.Text;
                Link.IP = txtOutIP.Text;
                Link.Port = (ushort)protOut.Value;
                Link.EndIP = txtInIP.Text;
                Link.EndPort = (int)protInt.Value;

                if (!StaticCon.SocketClint.ContainsKey(Link.Name))
                {
                    StaticCon.SocketClint.Add(Link.Name, Link);
                    Node.Nodes.Add(Link.Name).Tag = Link;
                }
                else
                {
                    MessageBox.Show(txtSocketName.Text + "名称已存在!请重新输入");
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.SelectedItem == null)
                {
                    comboBox2.SelectedItem = Link.GetEncoding().HeaderName;
                }
                string datas = TXTS.Text;
                Link.Send(datas);
                //Link.Send(Encoding.GetEncoding(comboBox2.SelectedItem.ToString()).GetBytes(datas));
                byte[] vs = new byte[50];
                //Link.Socket().ReceiveTimeout = 1000;
                //Link.Socket().Receive(vs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void newSocketForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (MessageBox.Show("是否关闭连接？","关闭连接",MessageBoxButtons.OKCancel,MessageBoxIcon.Asterisk)==DialogResult.OK)
            //{
            //    //Link.Close();
            //}
            try
            {
                if (Link != null)
                {
                    Link.PassiveEvent -= Socket_PassiveEvent;
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            UpDATA();
        }

        /// <summary>
        ///
        /// </summary>
        private void UpDATA()
        {
            comboBoxLinkType.Items.Clear();
            foreach (var item in SocketClint.GetListClassName())
            {
                comboBoxLinkType.Items.Add(item);
            }
            propertyGrid1.SelectedObject = Link;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtR.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TXTS.Text = "";
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Link.Close();
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
        }

        private void comboBox2_Click(object sender, EventArgs e)
        {
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Link.EncodingStr = comboBox2.SelectedItem.ToString();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Link.initialization();
        }
    }
}