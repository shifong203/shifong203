using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace ErosSocket.ErosConLink
{
    public class RS232Con : SocketClint
    {
        public System.IO.Ports.SerialPort SerialPort = new System.IO.Ports.SerialPort();


        public System.IO.Ports.Parity Parity { get; set; } = Parity.Even;

        public int BaudRate { get; set; } = 9600;

        public int DataBits { get; set; } = 1;
        public StopBits StopBits { get; set; } = StopBits.None;

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
                }
            }
            catch (Exception)
            {
            }
            return base.AsynLink(isCycle);
        }

        public override void Receive()
        {

            Thread threadReceive = new Thread(() =>
            {
                Thread.Sleep(2000);
                while (!CloseBool)
                {
                    try
                    {
                        if (Recivebuffer == null)
                        {
                            Recivebuffer = new byte[1024 * 1024 * 5];
                        }
                        if (Insocket != null)
                        {
                            ReciveBufferLenth = SerialPort.Read(Recivebuffer, 0, 1000);
                            if (ReciveBufferLenth == 1) continue;
                            if (ReciveBufferLenth > 1)
                            {
                                OnEventArge(Recivebuffer.Skip(0).Take(ReciveBufferLenth).ToArray(), null);
                                watchSen.Restart();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            })
            {
                IsBackground = true,
                Priority = ThreadPriority.Highest,
            };//接受信息事件
            threadReceive.Start();

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
            {
            }

        }


    }
}
