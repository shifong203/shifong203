using System;
using System.IO.Ports;
using System.Linq;

namespace ErosSocket.ErosConLink
{
    public class RS232Con : SocketClint
    {
        protected System.IO.Ports.SerialPort SerialPort = new System.IO.Ports.SerialPort();

        public SerialPort GetSerialPort()
        {
            return SerialPort;
        }

        public System.IO.Ports.Parity Parity { get; set; } = Parity.None;

        public int BaudRate { get; set; } = 9600;

        public int DataBits { get; set; } = 8;
        public StopBits StopBits { get; set; } = StopBits.One;

        public override bool AsynLink(bool isCycle = true)
        {
            try
            {
                SerialPort.PortName = IP;
                SerialPort.Parity = Parity;
                SerialPort.DataBits = DataBits;
                SerialPort.BaudRate = BaudRate;
                SerialPort.StopBits = StopBits;
                if (!SerialPort.IsOpen)
                {
                    SerialPort.Open();
                    SerialPort.DataReceived += SerialPort_DataReceived;
                    LinkState = "连接成功";
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            LinkState = "连接失败";
            return false;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                ReciveBufferLenth = SerialPort.BytesToRead;                 //接收串口的数据数
                if (ReciveBufferLenth >= 1)
                {
                    Recivebuffer = new byte[ReciveBufferLenth];
                    SerialPort.Read(Recivebuffer, 0, ReciveBufferLenth);
                    //foreach (byte item in Recivebuffer)           //读取Buff中存的数据，转换成显示的十六进制数
                    //{
                    //    string str = "";
                    //    str += item.ToString("X2") + " ";
                    //}
                    //string   data = SerialPort.ReadExisting();
                    //if (IsAlramText)
                    //{
                    //    AddTextBox("(R):" + data, System.Drawing.Color.Green);
                    //}
                    //int b = data.Length;
                    //string[] a = new string[16];
                    // int[] num = new int[16];

                    //for (int i = 1; i < b-4; i = i + 4)
                    //{
                    //    int j = i / 4;
                    //    a[j] = data.Substring(i, 4);                       //D100,.......字符串解析
                    //    a[j] = a[j].Substring(2, 2) + a[j].Substring(0, 2);
                    //}
                    //for (int i = 0; i < 16; i++)
                    //{
                    //    num[i] = int.Parse(a[i], System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                    //}
                    OnEventArge(Recivebuffer.Skip(0).Take(ReciveBufferLenth).ToArray(), null);
                }
            }
            catch (Exception)
            {
            }
        }

        public override void Close()
        {
            base.Close();
            try
            {
                if (SerialPort.IsOpen)
                {
                    SerialPort.Close();
                }
            }
            catch (Exception)
            { }
        }

        public override bool Send(byte[] buffr)
        {
            try
            {
                this.SerialPort.Write(buffr, 0, buffr.Length);
                return true;
            }
            catch (Exception)
            { }
            return false;
        }

        public override bool Send(string strdata)
        {
            try
            {
                if (this.SerialPort.IsOpen)
                {
                    if (IsAlramText)
                    {
                        AddTextBox("(S):" + strdata, System.Drawing.Color.Green);
                    }
                    this.SerialPort.Write(strdata);
                    return true;
                }
            }
            catch (Exception)
            { }
            return false;
        }
    }
}