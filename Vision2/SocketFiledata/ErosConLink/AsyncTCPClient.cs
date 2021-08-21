using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ErosSocket.ErosConLink
{
    /// <summary>
    ///
    /// </summary>
    public class AsyncTCPClient : ErosSocket.ErosConLink.SocketClint
    {
        /// <summary>
        /// 连接到服务器
        /// </summary>
        public bool AsynConnect(IPEndPoint ipe)
        {
            //端口及IP
            //IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6065);
            if (CloseBool)
            {
                return false;
            }
            //创建套接字
            if (Insocket == null)
            {
                Insocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            if (IsStataText)
            {
                this.AddTextBox("连接" + ipe.ToString());
            }
            //开始连接到服务器
            Insocket.BeginConnect(ipe, asyncResult =>
            {
                try
                {
                    Insocket.EndConnect(asyncResult);
                    this.LinkState = "连接成功";
                    return;
                }
                catch (Exception)
                {
                    this.LinkState = "连接失败";
                }

                AsynConnect(new IPEndPoint(IPAddress.Parse(this.IP), this.Port));
            }, null);
            return true;
        }

        public override void EnabledRunCycleEvent()
        {
            AsynLink();
        }

        public override bool Send(string data)
        {
            AsynSend(Insocket, data);
            return true;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        public new void AsynSend(Socket socket, string message)
        {
            if (socket == null || message == string.Empty) return;
            //编码
            byte[] data = Encoding.UTF8.GetBytes(message);
            try
            {
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
                {
                    //完成发送消息
                    int length = socket.EndSend(asyncResult);
                    Console.WriteLine(string.Format("客户端发送消息:{0}", message));
                }, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常信息：{0}", ex.Message);
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="socket"></param>
        public new void AsynRecive(Socket socket)
        {
            byte[] data = new byte[1024];
            try
            {
                //开始接收数据
                socket.BeginReceive(data, 0, data.Length, SocketFlags.None,
                asyncResult =>
                {
                    int length = socket.EndReceive(asyncResult);
                    if (IsAlramText)
                    {
                        AddTextBox(string.Format("收到服务器消息:{0}", Encoding.UTF8.GetString(data)));
                    }
                    AsynRecive(socket);
                }, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常信息：", ex.Message);
            }
        }

        public override bool AsynLink(bool isCycle = true)
        {
            try
            {
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(this.IP), this.Port);
                AsynConnect(ipe);
                return true;
            }
            catch (Exception)
            {
            }
            //return base.Link();
            return false;
        }
    }
}