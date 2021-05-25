using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ErosSocket.ErosConLink
{
    /// <summary>
    /// 服务器socket类
    /// </summary>
    public class SocketServer : SocketClint
    {
        /// <summary>
        ///新的链接信息
        /// </summary>
        public event DelegateSocket<byte[]> NewLink;


        /// <summary>
        /// 截取json字符
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetSendStr(string name)
        {
            //GetSenString = "";

            if (DictiPoint.ContainsKey(name))
            {
                GetSenKey = name;
            }
            else
            {
                return "不存在链接:" + name;
            }
            return GetSenString;
        }
        /// <summary>
        /// 截取json字符
        /// </summary>
        /// <param name="send"></param>
        private void GetSeng(string send)
        {
            if (DictiPoint.ContainsKey(GetSenKey))
            {
                GetSenKey = "";
                GetSenString = send;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string GetSenString = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        private string GetSenKey = string.Empty;
        //[DisplayNameAttribute("心跳周期MS"), Category("运行参数")]
        //private int CycleTime { get; set; } = 10;
        [DisplayNameAttribute("最大链接数"), Category("运行参数")]
        public byte LinkMaxNunber { get; set; } = 100;
        /// <summary>
        /// 链接的对象池信息
        /// </summary>
        public List<string> linkInformation = new List<string>();

        /// <summary>
        /// 储存TCP链接实例,包含链接地址
        /// </summary>
        protected Dictionary<string, Socket> DictiPoint = new Dictionary<string, Socket>();

        public Dictionary<string, Socket> GetDictiPoints()
        {
            return DictiPoint;
        }

        public Socket GetNameSocket(string name)
        {
            if (DictiPoint.ContainsKey(name))
            {
                return DictiPoint[name];
            }
            else
            {
                return null;
            }
        }

        public SocketServer()
        {
            if (Recivebuffer == null)
            {
                Recivebuffer = new byte[1024 * 1024 * 5];
            }
            NetType = this.GetType().Name;
        }

        /// <summary>
        /// 监听链接
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="inputPort"></param>
        public SocketServer(string IP, ushort inputPort) : this()
        {
            this.EndIP = IP;
            this.EndPort = inputPort;
            AsynLink();
        }

        /// <summary>
        /// 监听链接
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="inputPort"></param>
        public SocketServer(ushort inputPort) : this()
        {
            this.EndIP = IPAddress.Any.ToString();
            this.EndPort = inputPort;
            this.AsynLink();
        }
        TcpListener Listener;

        public override bool Send(byte[] buffr)
        {
            try
            {
                foreach (var item in DictiPoint)
                {
                    try
                    {
                        item.Value.Send(buffr);
                    }
                    catch { }
                }
                if (DictiPoint.Count == 0)
                {
                    if (IsAlramText)
                    {
                        AddTextBox("(S)发送失败(" + this.LinkState + "):" + GetEncoding().GetString(buffr).Remove(GetEncoding().GetString(buffr).Length - FTU.Length / 2), System.Drawing.Color.Red);
                    }
                }
                else
                {
                    if (IsAlramText)
                    {
                        AddTextBox("(S)" + this.LinkState + ":" + GetEncoding().GetString(buffr).Remove(GetEncoding().GetString(buffr).Length - FTU.Length / 2), System.Drawing.Color.GreenYellow);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public override Form ShowForm()
        {
            SocketServerForm socketServerForm = new SocketServerForm(this) { TopLevel = false };

            return socketServerForm;
        }
        /// <summary>
        /// 监听链接
        /// </summary>
        /// <returns></returns>
        public override bool AsynLink(bool isCycle = true)
        {

            Action act = new Action(beginStartService);
            act.BeginInvoke(lisnerCallback, null);
            return true;
        }

        public override void ThreadLink(bool runingLink)
        {
            AsynLink();
        }
        private void beginStartService()
        {
            IPEndPoint point;
            if (this.EndIP == "Any")
            {
                point = new IPEndPoint(IPAddress.Any, Convert.ToInt32(this.EndPort));
            }
            else
            {
                point = new IPEndPoint(IPAddress.Parse(this.EndIP), Convert.ToInt32(this.EndPort));
            }
            Listener = new TcpListener(point);
            try
            {
                Listener.Start(this.LinkMaxNunber);
                this.LinkState = "监听中";
                Insocket = Listener.Server;
            }
            catch (Exception ee)
            {
                this.LinkState = ee.Message;
                return;
            }
            while (true)
            {
                try
                {
                    if (CloseBool)
                    {
                        return;
                    }
                    TcpClient client = Listener.AcceptTcpClient();
                    if (DictiPoint.ContainsKey(client.Client.RemoteEndPoint.ToString()))
                    {
                        DictiPoint[client.Client.RemoteEndPoint.ToString()] = client.Client;
                    }
                    else
                    {
                        DictiPoint.Add(client.Client.RemoteEndPoint.ToString(), client.Client);
                    }
                    OnNewLink(client.Client.RemoteEndPoint.ToString() + "连接成功！");
                    this.LinkState = "连接成功" + DictiPoint.Count;
                    OnMastLink(true);
                    AddListen(client.Client);
                }
                catch (Exception ex)
                {
                    this.LogErr(ex);
                }
            }
        }
        /// <summary>
        /// 监听出错执行
        /// </summary>
        /// <param name="ar"></param>
        private void lisnerCallback(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {

            }
        }
        /// <summary>
        /// 链接成功并创建
        /// </summary>
        /// <param name="o"></param>
        private void Listen()
        {
            System.Net.Sockets.Socket socketSend;
            while (!this.CloseBool)
            {
                try
                {
                    socketSend = Insocket.Accept();//创建链接
                    //将建立的链接储存在dictionary里
                    DictiPoint.Add(socketSend.RemoteEndPoint.ToString(), socketSend);
                    OnNewLink(socketSend.RemoteEndPoint.ToString() + "连接成功！");
                    //将链接地址储存在列表里
                    //开启一个新线程不停的接受客户端发送过来的消息
                    Thread th = new Thread(delegate ()
                    {
                        System.Timers.Timer timer = new System.Timers.Timer();
                        SendTimeMesag(timer, socketSend);
                        Recive(timer, socketSend);
                    });
                    th.IsBackground = true;
                    th.Start();
                }
                catch (Exception)
                { }
            }
        }

        protected virtual void AddListen(Socket Client)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            SendTimeMesag(timer, Client);
            byte[] buffers = new byte[1024 * 1024 * 500];
            AsyncReveive(timer, Client, buffers);
        }

        /// <summary>
        /// 递归接收消息
        /// </summary>
        /// <param name="client"></param>
        protected virtual void AsyncReveive(System.Timers.Timer timer, Socket socket, byte[] buffers)
        {
            try
            {
                //开始接收消息
                socket.BeginReceive(buffers, 0, buffers.Length, SocketFlags.None,
                asyncResult =>
                {
                    try
                    {
                        int length = socket.EndReceive(asyncResult);
                        if (length != 0)
                        {
                            timer.Stop();
                            EventArge(buffers.Skip(0).Take(length).ToArray(), socket);
                
                            timer.Start();
                        }
                        else
                        {
                            if (socket.Connected)
                            {
                                GetDictiPoints().Remove(socket.RemoteEndPoint.ToString());
                                OnNewLink(socket.RemoteEndPoint.ToString() + "断开连接!");
                                return;
                            }
                        }
                        buffers = new byte[buffers.Length];
                    }
                    catch (Exception ex)
                    {
                    }
                    this.AsyncReveive(timer, socket, buffers);
                }, null);
            }
            catch (Exception ex)
            {
            }
        }

        public override string AlwaysReceive(int de)
        {
            try
            {
                watch.Restart();
                RecivesDone = false;
                int dd = 0;
                while (true)
                {
                    dd++;
                    Thread.Sleep(1);
                    if (RecivesDone)
                    {
                        return ReciveStr[ReciveStr.Count - 1];
                    }
                    if (watch.ElapsedMilliseconds > de)
                    {
                        break;
                    }
                }


            }
            catch (Exception re)
            {
            }
            return "";

        }
        /// <summary>
        /// 定时心跳
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="socket"></param>
        protected void SendTimeMesag(System.Timers.Timer timer, Socket socket)
        {
            timer.AutoReset = true;
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                try
                {
                    if (SendString != "")
                    {
                        byte[] buffr = Encoding.UTF8.GetBytes(this.SendString + this.GetCRLF());
                        socket.Send(buffr);
                    }
                    if (!socket.Connected)
                    {
                        socket.Blocking = false;
                        this.DictiPoint.Remove(socket.RemoteEndPoint.ToString());
                        OnNewLink(socket.RemoteEndPoint.ToString() + "断开连接!");
                        if (this.DictiPoint.Count == 0)
                        {
                            this.LinkState = "连接失败";
                            OnMastLink(false);
                        }
                        else
                        {
                            this.LinkState = "连接成功";
                            OnMastLink(true);
                        }

                        socket.Close();
                        timer.Stop();
                        timer.Dispose();
                    }

                }
                catch
                {
                    try
                    {
                        if (socket.RemoteEndPoint != null)
                        {
                            socket.Blocking = false;
                            this.DictiPoint.Remove(socket.RemoteEndPoint.ToString());
                        }
                    }
                    catch (Exception)
                    {

                        //socket.Blocking = false;
                        this.DictiPoint.Remove(socket.RemoteEndPoint.ToString());
                    }

                    OnNewLink(socket.RemoteEndPoint.ToString() + "断开链接!");
                    if (this.DictiPoint.Count == 0)
                    {
                        this.LinkState = "连接失败";
                        OnMastLink(false);
                    }
                    else
                    {
                        this.LinkState = "连接成功";
                        OnMastLink(true);
                    }
                    timer.Stop();
                    timer.Dispose();
                    socket.Close();
                }
            };
            timer.Interval = SendTime * 1000;
            timer.Start();
        }
        /// <summary>
        /// 接受数据
        /// </summary>Class1.cs
        /// <param name="o"></param>
        private void Recive(System.Timers.Timer timer, Socket socket)
        {
            try
            {
                Thread th = new Thread(delegate ()
                {
                    while (socket.Blocking)
                    {
                        try
                        {
                            if (Recivebuffer == null)
                            {
                                Recivebuffer = new byte[1024 * 1024 * 5];
                            }
                            int r = socket.Receive(Recivebuffer);
                            if (r != 0)
                            {
                                timer.Stop();
                                string str = Encoding.UTF8.GetString(Recivebuffer, 0, 30);
                                //接收Json
                                if (str.StartsWith("json"))
                                {
                                    string sd = "json".PadRight(30, ' ') + JsonCon.ServerJson(this.GetEncoding().GetString(Recivebuffer, 30, r));
                                    socket.Send(GetEncoding().GetBytes(sd));
                                    GetSeng(sd);
                                }
                                EventArge(Recivebuffer.Skip(0).Take(r).ToArray(), socket);
                                timer.Start();
                            }
                            Recivebuffer = new byte[Recivebuffer.Length];
                        }
                        catch { }
                    }
                });  //;
                th.Priority = ThreadPriority.Highest;
                th.IsBackground = true;
                th.Start();
            }
            catch (Exception e)
            {
            }
        }
        public override void Receive()
        {
            base.Receive();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffers"></param>
        /// <param name="socketR"></param>
        public void EventArge(byte[] buffers, Socket socketR)
        {
            //List<byte> listbuff = new List<byte>();
            //listbuff.AddRange(buffers);
            base.OnEventArge(buffers, socketR);
        }

        /// <summary>
        /// 链接事件
        /// </summary>
        /// <param name="LinkIPPort"></param>
        public void OnNewLink(string LinkIPPort)
        {
            linkInformation.Add(LinkIPPort + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            NewLink?.Invoke(Encoding.UTF8.GetBytes(LinkIPPort));
        }

        public override void EnabledRunCycleEvent()
        {
        }
        public override void Close()
        {
            try
            {
                base.Close();
                if (Listener != null)
                {
                    Listener.Stop();//关闭该ip地址和该端口的Socket；
                }
                foreach (var item in DictiPoint)
                {
                    SocketClint.SafeClose(item.Value);
                }
                this.DictiPoint.Clear();
            }
            catch (Exception)
            {


            }


        }
    }
}