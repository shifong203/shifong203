using ErosSocket.ErosConLink;
using System;
using System.IO;
using System.Windows.Forms;

namespace ErosSocket
{
    public partial class ModbusTcpForm : Form
    {
        public ModbusTcpForm()
        {
            InitializeComponent();
        }

        public ModbusTCPClint modbusTCP = new ModbusTCPClint();
        //private MelsecMcNet melsec_net = new MelsecMcNet("192.168.0.1", 6000);

        private void btnP_Click(object sender, EventArgs e)
        {
            try
            {
                if (modbusTCP.LinkModbusTCP(txtOutIP.Text, Convert.ToUInt16(nudPort.Value), Convert.ToByte(numAddress.Value)))
                {
                    groupBox2.Enabled = true;
                    groupBox3.Enabled = true;
                    this.txtRead.Text += "链接成功";
                }
                else
                {
                    MessageBox.Show("链接失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("失败：" + ex.Message);
            }
        }

        private void btnReadBool_Click(object sender, EventArgs e)
        {
            try
            {
                string sd = "";
                dynamic ds = "";
                switch (cobFunctionCode.SelectedItem)
                {
                    case "读线圈1":
                        modbusTCP.SendData(1, Convert.ToUInt16(numReadAddress.Value), Convert.ToByte(numReadAddressLength.Value), ref ds, out sd);
                        break;

                    case "读离散量2":
                        modbusTCP.SendData(2, Convert.ToUInt16(numReadAddress.Value), Convert.ToByte(numReadAddressLength.Value), ref ds, out sd);
                        break;

                    case "读寄存器3":
                        //short short_D1000 = modbusTCP.ReadInt16("D1000").Content;
                        modbusTCP.SendData(3, Convert.ToUInt16(numReadAddress.Value), Convert.ToByte(numReadAddressLength.Value), ref ds, out sd);
                        break;

                    default:
                        return;
                }
                this.txtRead.Text += ds + sd + Environment.NewLine;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReadAI_Click(object sender, EventArgs e)
        {
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                string sd = "";
                string ds = "";

                switch (cobWrite.SelectedItem)
                {
                    case "写线圈":
                        dynamic dsbool = Convert.ToBoolean(numWriteValue.Value);
                        ds = modbusTCP.SendData(5, Convert.ToUInt16(numWirteAddress.Value), 1, ref dsbool, out sd).ToString();
                        break;

                    case "写离散量":

                        //ds = modbusTCP.SendData(2, Convert.ToUInt16(txtReadAddress.Text), Convert.ToByte(txtReadAddressLength.Text), "", out sd);
                        break;

                    case "写寄存器":
                        dsbool = numWriteValue.Value;
                        modbusTCP.SendData(6, Convert.ToUInt16(numWirteAddress.Value), 1, ref dsbool, out sd);
                        break;

                    default:
                        return;
                }

                this.txtRead.Text += ds + sd + Environment.NewLine;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnReadAQ_Click(object sender, EventArgs e)
        {
        }

        private void BtnDeleteRead_Click(object sender, EventArgs e)
        {
            txtRead.Text = "";
        }

        private void btnNewC_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cobFunctionCode.SelectedItem = "读线圈1";
            cobWrite.SelectedItem = "写线圈";
            var path = Directory.GetFiles(Application.StartupPath + "\\ValueS")/*/*.Where(t=>t.EndsWith(".xml"))*/;//获取文件下的全部路径，附加多选筛选Where
            foreach (string item in path)
            {
                if (!StaticCon.DicErosValuess.ContainsKey(Path.GetFileNameWithoutExtension(item)))
                {
                    StaticCon.DicErosValuess.Add(Path.GetFileNameWithoutExtension(item), new UClass.ErosValues("", Path.GetFileNameWithoutExtension(item)) { });
                }
                else
                {
                    StaticCon.DicErosValuess[Path.GetFileNameWithoutExtension(item)] = new UClass.ErosValues("", Path.GetFileNameWithoutExtension(item)) { };
                }
            }
        }

        #region 变量表

        private void 导出变量表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void Values_Load(object sender, EventArgs e)
        {
            var path = Directory.GetFiles(Application.StartupPath + "\\ValueS")/*/*.Where(t=>t.EndsWith(".xml"))*/;//获取文件下的全部路径，附加多选筛选Where
            foreach (string item in path)
            {
                if (!StaticCon.DicErosValuess.ContainsKey(Path.GetFileNameWithoutExtension(item)))
                {
                    StaticCon.DicErosValuess.Add(Path.GetFileNameWithoutExtension(item), new UClass.ErosValues("", Path.GetFileNameWithoutExtension(item)) { });
                }
                else
                {
                    StaticCon.DicErosValuess[Path.GetFileNameWithoutExtension(item)] = new UClass.ErosValues("", Path.GetFileNameWithoutExtension(item)) { };
                }
            }
        }

        private void tsbtnSaveValues_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                StaticCon.ErrerLog(ex);
            }
        }

        private void tvListValue_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void tvListValue_DoubleClick(object sender, EventArgs e)
        {
        }

        #endregion 变量表

        private void label10_Click(object sender, EventArgs e)
        {
        }

        private void txtRead_TextChanged(object sender, EventArgs e)
        {
        }
    }
}