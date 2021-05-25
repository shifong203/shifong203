using System;
using System.Windows.Forms;

namespace ErosSocket
{
    public partial class Modbus : Form
    {
        public Modbus()
        {
            InitializeComponent();
        }

        public ErosConLink.海达U700 modbus;

        private void btnP_Click(object sender, EventArgs e)
        {
            try
            {
                modbus = new ErosConLink.海达U700();
                modbus.Address = Convert.ToByte(numAddress.Value);
                if (modbus.Link(txtOutIP.Text, Convert.ToUInt16(nudPort.Value)))
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
            string sd = "";
            string ds = "";
            switch (cobFunctionCode.SelectedItem)
            {
                case "读线圈1":
                    var det = modbus.SendData(1, Convert.ToUInt16(numReadAddress.Value), Convert.ToByte(numReadAddressLength.Value), null, out sd);
                    break;

                case "读离散量2":
                    det = modbus.SendData(2, Convert.ToUInt16(numReadAddress.Value), Convert.ToByte(numReadAddressLength.Value), null, out sd);
                    break;

                case "读寄存器3":
                    det = modbus.SendData(3, Convert.ToUInt16(numReadAddress.Value), Convert.ToByte(numReadAddressLength.Value), null, out sd);
                    if (det == null)
                    {
                        return;
                    }
                    foreach (var item in det)
                    {
                        ds += item.ToString() + ",";
                    }
                    break;

                default:
                    return;
            }
            this.txtRead.Text += ds + sd + Environment.NewLine;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            string sd = "";
            string ds = "";
            switch (cobWrite.SelectedItem)
            {
                case "写线圈":
                    break;

                case "写离散量":
                    break;

                case "写寄存器":
                    string[] valuesStr = numWriteValue.Value.ToString().Split(',');
                    Int16[] valuesInt = new short[valuesStr.Length];
                    for (int i = 0; i < valuesStr.Length; i++)
                    {
                        if (!Int16.TryParse(valuesStr[i], out valuesInt[i]))
                        {
                            MessageBox.Show("数据无法转换为int16");
                            return;
                        }
                    }
                    ds = modbus.SendData(6, Convert.ToUInt16(numWirteAddress.Value), valuesInt.Length, valuesInt, out sd);
                    break;

                default:
                    return;
            }
        }
    }
}