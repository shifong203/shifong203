using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision2.vision.Cams
{
    public delegate void dosome();
    public class SerialPortHelper :SerialPort
    {

        string comReceive;
        /// <summary>
        /// 
        /// </summary>
        public dosome DOrecevie;
        /// <summary>
        /// 接受到的字符
        /// </summary>
        public string ComReceive
        {
            get { return comReceive; }
            set
            {
                if (value != comReceive)
                {
                    if (DOrecevie != null)
                    {
                        DOrecevie();
                    }
                }
                comReceive = "";
            }
        }
        public SerialPortHelper(int i, int baudRate, out string ErrInfo)
        {
            try
            {
                this.PortName = "COM" + i.ToString();//端口号
                this.BaudRate = baudRate;//波特率
                this.Parity = Parity.None;//奇偶校验位
                this.DataBits = 8;//数据长度
                this.StopBits = StopBits.One;
                this.Open();
                this.DataReceived += Sp_DataReceived;
                ErrInfo = null;
            }
            catch (Exception EX)
            {

                ErrInfo = EX.Message;
            }
        }
        /// <summary>
        /// 数据接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int count = this.BytesToRead;                 //接收串口的数据数
            if (this.BytesToRead == 40)
            {
                string str = "";
                byte[] buff = new byte[count];
                this.Read(buff, 0, count);
                foreach (byte item in buff)           //读取Buff中存的数据，转换成显示的十六进制数
                {
                    str += item.ToString("X2") + " ";
                }
                ComReceive = str;
            }
        }
        /// <summary>
        /// 发送数据,
        /// </summary>
        /// <param name="sendbuff">发送的数据内容</param>
        public void SerialSend(string sendbuff)
        {
            try
            {
                byte[] sendData = Encoding.ASCII.GetBytes(sendbuff);
                this.Write(sendData, 0, sendData.Length);//发送数据  
                                                       //s.WriteLine(sendbuff);
            }
            catch (Exception)
            {
                //throw;
            }
        }
    }
}
