using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC.Robot
{
    public class EpsenRobot6 : ErosConLink.SocketServer, IAxisGrub
    {
        public EpsenRobot6()
        {
            contextMenuTT.Items.Add("打开调试界面").Click += EpsenRobot6_Click;
            this.AxisNumber = 6;
            DicSendMeseage.Add("拍照抓取物料", "CCD0,1,11,101,2");
            DicSendMeseage.Add("固定物料拍照", "CCD1,2,13,106,2");
            DicSendMeseage.Add("放置物料拍照", "CCD1,2,11,102,3");
            //this.RunUresControl = new Panel();
        }

        private void EpsenRobot6_Click(object sender, EventArgs e)
        {
            try
            {
                ShowForm(e.ToString());
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override Control GetDebugControl(Form form = null)
        {
            SteppingControl debugRobot = new SteppingControl(this);
            if (form != null)
            {
                form.KeyPreview = true;
                form.KeyDown += DebugRobot_KeyDown;
            }
            debugRobot.Dock = DockStyle.Fill;
            return debugRobot;
        }

        private void DebugRobot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                Form form = sender as Form;
                switch (e.KeyCode)
                {
                    case Keys.C:

                        if (form != null)
                        {
                            form.Focus();
                        }
                        this.Stop();
                        break;

                    case Keys.Up:

                        if (form != null)
                        {
                            form.Focus();
                        }
                        this.SendCommand("StepMove,Y,Plus", this.Seelp.ToString(), this.jogY.ToString());
                        break;

                    case Keys.Down:
                        if (form != null)
                        {
                            form.Focus();
                        }
                        this.SendCommand("StepMove,Y,Minus", this.Seelp.ToString(), this.jogY.ToString());

                        break;

                    case Keys.Right:
                        if (form != null)
                        {
                            form.Focus();
                        }
                        this.SendCommand("StepMove,X,Minus", this.Seelp.ToString(), this.jogX.ToString());

                        break;

                    case Keys.Left:
                        if (form != null)
                        {
                            form.Focus();
                        }
                        this.SendCommand("StepMove,X,Plus", this.Seelp.ToString(), this.jogX.ToString());
                        break;

                    default:
                        break;
                }
            }
            else if (e.Modifiers == Keys.Alt)
            {
                Form form = sender as Form;
                switch (e.KeyCode)
                {
                    case Keys.C:
                        if (form != null)
                        {
                            form.Focus();
                        }
                        this.Stop();

                        break;

                    case Keys.Up:
                        if (form != null)
                        {
                            form.Focus();
                        }
                        this.SendCommand("StepMove,Z,Plus", this.Seelp.ToString(), this.jogZ.ToString());

                        break;

                    case Keys.Down:
                        if (form != null)
                        {
                            form.Focus();
                        }
                        this.SendCommand("StepMove,Z,Minus", this.Seelp.ToString(), this.jogZ.ToString());
                        break;

                    case Keys.Right:
                        this.SendCommand("StepMove,U,Minus", this.Seelp.ToString(), this.jogU.ToString());

                        break;

                    case Keys.Left:
                        if (form != null)
                        {
                            form.Focus();
                        }
                        this.SendCommand("StepMove,U,Plus", this.Seelp.ToString(), this.jogU.ToString());

                        break;

                    default:
                        break;
                }
            }
        }

        public Stopwatch Watch { get; set; } = new Stopwatch();
        private DebugRobot debugRobot;

        public DebugRobot DebugFormShow()
        {
            if (debugRobot == null || debugRobot.IsDisposed)
            {
                debugRobot = new DebugRobot();
                debugRobot.SetAxis(this);
            }
            Vision2.ErosProjcetDLL.UI.UICon.WindosFormerShow(ref debugRobot);
            return debugRobot;
        }

        [DescriptionAttribute("程序文件。"), Category("执行"), DisplayName("程序文件名")]
        public string PRunName { get; set; }

        /// <summary>
        /// 读取连接
        /// </summary>
        protected Socket InsGet;

        /// <summary>
        /// 设置连接
        /// </summary>
        protected Socket InsSet;

        /// <summary>
        /// 命令连接
        /// </summary>
        protected Socket InsContmad;

        /// <summary>
        /// 忙碌中
        /// </summary>
        public bool Busy { get; set; }

        public UInt16 MemOutW { get; set; }
        public UInt16 MemInW1 { get; set; }
        public UInt16 MemInW2 { get; set; }
        public double Seelp { get; set; }
        public Single jogY = 1;
        public Single jogX = 1;
        public Single jogZ = 1;
        public Single jogU = 1;
        public Single jogV = 1;
        public Single jogW = 1;
        public Single jogY1 = 1;
        public Single jogX1 = 1;
        public Single jogZ1 = 1;
        public Single jogU1 = 1;
        public Single jogV1 = 1;
        public Single jogW1 = 1;
        public int RunID { get; private set; }
        public int[] RunIDs { get; private set; } = new int[10];
        //public bool DebugModei { get; private set; }

        protected override void AsyncReveive(System.Timers.Timer timer, Socket socket, byte[] buffers,string endpoint="")
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
                            string str = GetEncoding().GetString(buffers, 0, 30);
                            //接收Json
                            if (str.StartsWith("json"))
                            {
                                string sd = "json".PadRight(30, ' ') + JsonCon.ServerJson(GetEncoding().GetString(buffers, 30, length));
                                socket.Send(GetEncoding().GetBytes(sd));
                            }
                            //else if (str.StartsWith("GetMode"))
                            //{
                            //    //if (DebugMode)
                            //    //{
                            //    //    socket.Send(GetEncoding().GetBytes("Debug" + this.GetCRLF()));
                            //    //}
                            //    //else
                            //    //{
                            //    //    socket.Send(this.GetEncoding().GetBytes("Run" + this.GetCRLF()));
                            //    //}
                            //}
                            else if (str.StartsWith("GetSocket"))
                            {
                                socket.Send(this.GetEncoding().GetBytes("GetSocketDone" + this.GetCRLF()));
                                timer.Stop();
                                InsGet = socket;
                                GetPoint(InsGet);
                                return;
                            }
                            else if (str.StartsWith("SetSocket"))
                            {
                                socket.Send(this.GetEncoding().GetBytes("SetSocketDone" + this.GetCRLF()));
                                timer.Stop();
                                InsSet = socket;
                                this.AsyncReveive(timer, socket, buffers);
                                return;
                            }
                            else if (str.StartsWith("ContSocet"))
                            {
                                socket.Send(this.GetEncoding().GetBytes("ContSocetDone" + this.GetCRLF()));
                                InsContmad = socket;
                                return;
                            }

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
                    catch (Exception)
                    {
                    }
                    this.AsyncReveive(timer, socket, buffers);
                }, null);
            }
            catch (Exception ex)
            {
            }
        }

        private byte[] GetBuffers = new byte[1000];
        private string[] dsatas;
        private UserRobotControl UserRobot;

        /// <summary>
        /// 接受机器人信息
        /// </summary>
        /// <param name="socket"></param>
        private void GetPoint(Socket socket)
        {
            try
            {
                this.CDataTime = watch.ElapsedMilliseconds;
                watch.Restart();
                //开始接收消息
                socket.BeginReceive(GetBuffers, 0, GetBuffers.Length, SocketFlags.None,
                asyncResult =>
                {
                    try
                    {
                        int length = socket.EndReceive(asyncResult);
                        if (length != 0)
                        {
                            IsConn = true;
                            string str = this.GetEncoding().GetString(GetBuffers, 0, length);
                            str = str.Remove(str.Length - this.GetCRLF().Length);
                            List<string> times = new List<string>();

                            while (str.Contains(Environment.NewLine))
                            {
                                string sdd = str.Remove(str.IndexOf(Environment.NewLine));
                                times.Add(sdd);
                                str = str.Remove(0, sdd.Length + Environment.NewLine.Length);
                            }
                            times.Add(str);

                            for (int it = 0; it < times.Count; it++)
                            {
                                dsatas = times[it].Split(' ');
                                switch (dsatas[0])
                                {
                                    case "1":
                                        if (dsatas.Length >= 16)
                                        {
                                            int sd = 0;
                                            X = Convert.ToSingle(dsatas[1 + sd]);
                                            Y = Convert.ToSingle(dsatas[2 + sd]);
                                            Z = Convert.ToSingle(dsatas[3 + sd]);
                                            U = Convert.ToSingle(dsatas[4 + sd]);
                                            V = Convert.ToSingle(dsatas[5 + sd]);
                                            W = Convert.ToSingle(dsatas[6 + sd]);

                                            int d = int.Parse(dsatas[12 + sd]);

                                            //int d = Convert.ToInt32(dsatas[11]);
                                            bool[] bitArray = ErosConLink.StaticCon.ConvertIntToBoolArray(d, 32);
                                            if (bitArray.Length == 32)
                                            {
                                                Aralming = bitArray[18];
                                                this.Pauseing = bitArray[17];
                                                enabled = bitArray[23];
                                            }
                                            bitArray = StaticCon.ConvertIntToBoolArray(int.Parse(dsatas[13 + sd]), 32);

                                            if (bitArray.Length == 32)
                                            {
                                                motorOn = bitArray[4];
                                            }
                                            Tool = Convert.ToSByte(dsatas[14 + sd]);
                                            MemInW1 = Convert.ToUInt16(dsatas[15 + sd]);
                                            MemInW2 = Convert.ToUInt16(dsatas[16 + sd]);
                                            MemOutW = Convert.ToUInt16(dsatas[17 + sd]);
                                            //X = Convert.ToSingle(dsatas[0]);
                                        }

                                        if (dsatas.Length >= 31)
                                        {
                                            RunID = int.Parse(dsatas[18]);
                                            if (int.Parse(dsatas[19]) != 0)
                                            {
                                                DebugMode = true;
                                            }
                                            else
                                            {
                                                DebugMode = false;
                                            }
                                            if (RunIDs == null)
                                            {
                                                RunIDs = new int[10];
                                            }
                                            for (int i = 0; i < RunIDs.Length; i++)
                                            {
                                                RunIDs[i] = int.Parse(dsatas[20 + i]);
                                            }
                                        }
                                        break;

                                    case "2":
                                        for (int i = 0; i < dsatas.Length - 1; i++)
                                        {
                                            try
                                            {
                                                if (!this.KeysValues.DictionaryValueD.ContainsKey("Bool" + i))
                                                {
                                                    this.KeysValues.DictionaryValueD.Add("Bool" + i, new UClass.ErosValues.ErosValueD()
                                                    {
                                                        _Type = UClass.Boolean,
                                                        AddressID = "GPBoolValue" + i,
                                                    });
                                                }
                                                this.KeysValues["Bool" + i] = Convert.ToBoolean(int.Parse(dsatas[i + 1]));
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        break;

                                    case "3":
                                        for (int i = 0; i < dsatas.Length - 1; i++)
                                        {
                                            try
                                            {
                                                if (!this.KeysValues.DictionaryValueD.ContainsKey("Int" + i))
                                                {
                                                    this.KeysValues.DictionaryValueD.Add("Int" + i, new UClass.ErosValues.ErosValueD()
                                                    {
                                                        _Type = UClass.Int16,
                                                        AddressID = "GPIntValue" + i,
                                                    });
                                                }
                                                this.KeysValues["Int" + i] = int.Parse(dsatas[i + 1]);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        break;

                                    case "4":
                                        for (int i = 0; i < dsatas.Length - 1; i++)
                                        {
                                            try
                                            {
                                                if (!this.KeysValues.DictionaryValueD.ContainsKey("Int32-" + i))
                                                {
                                                    this.KeysValues.DictionaryValueD.Add("Int32-" + i, new UClass.ErosValues.ErosValueD()
                                                    {
                                                        _Type = UClass.Int32,
                                                        AddressID = "GPInt32Value" + i,
                                                    });
                                                }
                                                this.KeysValues["Int32-" + i] = Int32.Parse(dsatas[i + 1]);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        break;

                                    case "5":
                                        for (int i = 0; i < dsatas.Length - 1; i++)
                                        {
                                            try
                                            {
                                                if (!this.KeysValues.DictionaryValueD.ContainsKey("Int64-" + i))
                                                {
                                                    this.KeysValues.DictionaryValueD.Add("Int64-" + i, new UClass.ErosValues.ErosValueD()
                                                    {
                                                        _Type = UClass.Int64,
                                                        AddressID = "GPInt64Value" + i,
                                                    });
                                                }
                                                this.KeysValues["Int64-" + i] = Int64.Parse(dsatas[i + 1]);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        break;

                                    case "6":
                                        for (int i = 0; i < dsatas.Length - 1; i++)
                                        {
                                            try
                                            {
                                                if (!this.KeysValues.DictionaryValueD.ContainsKey("Real" + i))
                                                {
                                                    this.KeysValues.DictionaryValueD.Add("Real" + i, new UClass.ErosValues.ErosValueD()
                                                    {
                                                        _Type = UClass.String,
                                                        AddressID = "GPRealValue" + i,
                                                    });
                                                }
                                                this.KeysValues["Real" + i] = dsatas[i + 1];
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        break;

                                    case "7":
                                        for (int i = 0; i < dsatas.Length - 1; i++)
                                        {
                                            try
                                            {
                                                if (!this.KeysValues.DictionaryValueD.ContainsKey("Short" + i))
                                                {
                                                    this.KeysValues.DictionaryValueD.Add("Short" + i, new UClass.ErosValues.ErosValueD()
                                                    {
                                                        _Type = UClass.String,
                                                        AddressID = "GPShortValue" + i,
                                                    });
                                                }
                                                this.KeysValues["Short" + i] = dsatas[i + 1];
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        break;

                                    case "8":
                                        for (int i = 0; i < dsatas.Length - 1; i++)
                                        {
                                            try
                                            {
                                                if (!this.KeysValues.DictionaryValueD.ContainsKey("Double" + i))
                                                {
                                                    this.KeysValues.DictionaryValueD.Add("Double" + i, new UClass.ErosValues.ErosValueD()
                                                    {
                                                        _Type = UClass.String,
                                                        AddressID = "GPDoubleValue" + i,
                                                    });
                                                }
                                                this.KeysValues["Double" + i] = dsatas[i + 1];
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        break;

                                    case "9":
                                        for (int i = 0; i < dsatas.Length - 1; i++)
                                        {
                                            try
                                            {
                                                if (!this.KeysValues.DictionaryValueD.ContainsKey("Long" + i))
                                                {
                                                    this.KeysValues.DictionaryValueD.Add("Long" + i, new UClass.ErosValues.ErosValueD()
                                                    {
                                                        _Type = UClass.String,
                                                        AddressID = "GPLongValue" + i,
                                                    });
                                                }
                                                this.KeysValues["Long" + i] = dsatas[i + 1];
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        break;

                                    case "10":
                                        for (int i = 0; i < dsatas.Length - 1; i++)
                                        {
                                            try
                                            {
                                                if (!this.KeysValues.DictionaryValueD.ContainsKey("Str" + i))
                                                {
                                                    this.KeysValues.DictionaryValueD.Add("Str" + i, new UClass.ErosValues.ErosValueD()
                                                    {
                                                        _Type = UClass.String,
                                                        AddressID = "GPStrValue$" + i,
                                                    });
                                                }
                                                this.KeysValues["Str" + i] = dsatas[i + 1];
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        break;

                                    default:
                                        if (dsatas.Length == 16)
                                        {
                                            int sd = 0;
                                            X = Convert.ToSingle(dsatas[1 + sd]);
                                            Y = Convert.ToSingle(dsatas[2 + sd]);
                                            Z = Convert.ToSingle(dsatas[3 + sd]);
                                            U = Convert.ToSingle(dsatas[4 + sd]);
                                            V = Convert.ToSingle(dsatas[5 + sd]);
                                            W = Convert.ToSingle(dsatas[6 + sd]);
                                            int d = int.Parse(dsatas[12 + sd]);
                                            //int d = Convert.ToInt32(dsatas[11]);
                                            bool[] bitArray = ErosConLink.StaticCon.ConvertIntToBoolArray(d, 32);
                                            if (bitArray.Length == 32)
                                            {
                                                Aralming = bitArray[18];
                                                this.Pauseing = bitArray[17];
                                                enabled = bitArray[23];
                                            }
                                            bitArray = StaticCon.ConvertIntToBoolArray(int.Parse(dsatas[13 + sd]), 32);

                                            if (bitArray.Length == 32)
                                            {
                                                motorOn = bitArray[4];
                                            }
                                            Tool = Convert.ToSByte(dsatas[14 + sd]);
                                            MemInW1 = Convert.ToUInt16(dsatas[15 + sd]);
                                            MemInW2 = Convert.ToUInt16(dsatas[16 + sd]);
                                            //MemOutW = Convert.ToUInt16(dsatas[17 + sd]);
                                            //X = Convert.ToSingle(dsatas[0]);
                                        }
                                        break;
                                }
                            }
                            if (UserRobot != null)
                            {
                                UserRobot.UpRobot();
                            }

                            //EventArge(GetBuffers.Skip(0).Take(length).ToArray(), socket.RemoteEndPoint.ToString());
                        }
                        else
                        {
                            if (socket.Connected)
                            {
                                GetDictiPoints().Remove(socket.RemoteEndPoint.ToString());
                                OnNewLink(socket.RemoteEndPoint.ToString() + "断开连接!");
                                OnMastLink(false);
                                IsConn = false;
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    this._DataTime = DateTime.Now;

                    GetPoint(socket);
                }, null);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
        }

        public override bool SetValue(string key, string value, out string err)
        {
            if (this.KeysValues.DictionaryValueD.ContainsKey(key))
            {
                if (this.KeysValues.DictionaryValueD[key].AddressID == "")
                {
                    UClass.GetTypeValue(this.KeysValues.DictionaryValueD[key]._Type, value, out dynamic vd);

                    this.KeysValues[key] = vd;
                    base.SetValue(key, value, out err);
                }
                else
                {
                    Vision2.ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(this.KeysValues.DictionaryValueD[key].AddressID, out string names);
                    this.SendCommand(names,
                        Vision2.ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(this.KeysValues.DictionaryValueD[key].AddressID).ToString(), "1", value);
                }
            }
            return base.SetValue(key, value, out err);
        }

        public void SendCommand(params string[] measssCommand)
        {
            try
            {
                string ds = "";
                for (int i = 0; i < measssCommand.Length; i++)
                {
                    ds += measssCommand[i] + this.Split;
                }
                SetCommand(ds.TrimEnd(this.Split));
            }
            catch (Exception ex)
            {
                this.ErrerLog(ex);
            }
        }

        public const string PointPath = "RobotPointFile";

        public int HomeTime { get; set; } = 20000;

        public int SetTime { get; set; } = 10000;

        /// <summary>
        /// 根据代码移动位置
        /// </summary>
        /// <param name="runCode"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        public bool SetPoint(string runCode, double? x, double? y, double? z, double? u)
        {
            try
            {
                if (runCode == "GO")
                {
                }
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        public bool Motor { get { return motorOn; } }

        private bool motorOn;
        public bool Stoping { get; private set; }

        public bool IsHome { get; private set; }

        public bool enabled { get; private set; }

        public bool Pauseing { get; private set; }

        public bool Aralming { get; private set; }
        public bool High { get; private set; }
        public int ErrCode { get; set; }

        public string ErrMeaessge { get; set; }

        public sbyte Tool { get; set; }

        public void Pause(bool pauses = false)
        {
            try
            {
                if (pauses)
                {
                    InsSet.Send(this.GetEncoding().GetBytes("Cont" + this.GetCRLF()));
                }
                else
                {
                    InsSet.Send(this.GetEncoding().GetBytes("Pause" + this.GetCRLF()));
                }
            }
            catch (Exception)
            {
            }
        }

        private byte[] SetBureef = new byte[100];

        private void SetCommand(string command)
        {
            try
            {
                if (command.StartsWith("GP") || command.StartsWith("DebugMode"))
                {
                    InsGet.Send(this.GetEncoding().GetBytes(command + this.GetCRLF()));
                    LogText(command);
                }
                else
                {
                    if (InsSet != null)
                    {
                        InsSet.Send(this.GetEncoding().GetBytes(command + this.GetCRLF()));
                    }
                    else
                    {
                        this.Send(this.GetEncoding().GetBytes(command + this.GetCRLF()));
                        LogText("未连接机器人");
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrerLog(ex);
            }
        }

        public void Enabled()
        {
            try
            {
                if (motorOn)
                {
                    this.SendCommand("SetMotor", "Off");
                }
                else
                {
                    this.SendCommand("SetMotor", "On");
                }
            }
            catch (Exception ex)
            {
                this.ErrerLog(ex);
            }
        }

        public void Dand_type_brake(bool isd)
        {
        }

        public string PointFileName { get; set; }

        public SByte AxisNumber { get; set; } = 0;

        public double JoupZ { get; set; } = 0.0f;

        /// <summary>
        /// 点动模式
        /// </summary>
        public bool JogMode { get; set; } = true;

        public IAxis GetAxis(sbyte id)
        {
            throw new NotImplementedException();
        }

        private Single X;
        private Single Y;
        private Single Z;
        private Single U;
        private Single V;
        private Single W;

        public void GetPoints(out double x, out double y, out double z, out double u, out double v, out double w)
        {
            GetPoints(out x, out y, out z, out u);
            v = V;
            w = W;
        }

        public void GetPoints(out double x, out double y, out double z, out double u)
        {
            GetPoints(out x, out y, out z);
            u = U;
        }

        public void GetPoints(out double x, out double y, out double z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public void GetSt(out bool ishome, out bool enabled, out bool err, out string mees)
        {
            throw new NotImplementedException();
        }

        public void GetSt()
        {
        }

        public void Reset()
        {
            SetCommand("Reset");
        }

        public void SetHome()
        {
            try
            {
                SetCommand("Home");
            }
            catch (Exception)
            {
            }
        }

        public void SetPoint(string runCode, string pointName)
        {
            PointFile pointFile = PointFile.GetPointName(this.PointFileName, pointName);
            if (pointFile.Name == pointName)
            {
                if (pointFile.X == null || pointFile.Y == null)
                {
                    System.Windows.Forms.MessageBox.Show(pointFile.Name + "点未定义");
                }
                else
                {
                    //SetPoint(runCode, (Single)pointFile.X, (Single)pointFile.Y, pointFile.Z, pointFile.U, pointFile.V, pointFile.W);
                    this.SendCommand("SPoint", runCode, pointFile.X.ToString(), pointFile.Y.ToString(), pointFile.Z.ToString(), pointFile.U.ToString(),
                        pointFile.V.ToString(), pointFile.W.ToString(), pointFile.Hand.ToString(), pointFile.Elbow.ToString(), pointFile.Wrist.ToString(),
                        pointFile.J1Flag.ToString(), pointFile.J4Flag.ToString(), pointFile.J6Flag.ToString()
                        );
                }
            }
            else
            {
                if (DebugMode)
                {
                    System.Windows.Forms.MessageBox.Show("点不存在" + pointName);
                }
                else
                {
                    Vision2.ErosProjcetDLL.Project.AlarmText.LogErr("移动点不存在" + pointName, Name);
                }
            }
        }

        public void SetPoint(string runCode, string fileName, string pointName)
        {
        }

        public void SetPoint(string runCode, double x, double y, double? z, double? u, double? v, double? w)
        {
            string mdw = "";

            if (z != null)
            {
                mdw += z.ToString() + this.Split;
            }
            if (u != null)
            {
                mdw += u.ToString() + this.Split;
            }
            if (v != null)
            {
                mdw += v.ToString() + this.Split;
            }
            if (w != null)
            {
                mdw += w.ToString() + this.Split;
            }
            if (runCode == "PTP")
            {
                mdw += JoupZ + this.Split.ToString();
            }
            this.SendCommand("SPoint", runCode, x.ToString(), y.ToString(), mdw);
        }

        /// <summary>
        /// 移动目标点
        /// </summary>
        /// <param name="runCode">执行轨迹</param>
        /// <param name="pid">点编号</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <param name="hand"></param>
        /// <param name="elbow"></param>
        /// <param name="wrist"></param>
        /// <param name="j1f"></param>
        /// <param name="j4f"></param>
        /// <param name="j6f"></param>
        public void SetPoint(string runCode, uint pid, double x, double y, double? z, double? u, double? v, double? w, uint hand, uint elbow, uint wrist, uint j1f, uint j4f, uint j6f)
        {
            string mdw = "";

            if (z != null)
            {
                mdw += z.ToString() + this.Split;
            }
            if (u != null)
            {
                mdw += u.ToString() + this.Split;
            }
            if (v != null)
            {
                mdw += v.ToString() + this.Split;
            }
            if (w != null)
            {
                mdw += w.ToString() + this.Split;
            }
            if (runCode == "PTP")
            {
                mdw += JoupZ + this.Split.ToString();
            }
            mdw += hand.ToString() + Split + elbow.ToString() + Split + wrist.ToString() + Split + j1f.ToString() + Split + j4f.ToString()
                + Split + j6f.ToString() + Split;
            this.SendCommand("SPoint", runCode, pid.ToString(), x.ToString(), y.ToString(), mdw.TrimEnd(Split));
        }

        public void SetPoint(string runCode, double x, double y, double? z, double? u)
        {
        }

        public void SetPoint(string runCode, double x, double y, double? z)
        {
        }

        /// <summary>
        /// 试教当前位置到点文件
        /// </summary>
        /// <param name="pFileName">点文件名</param>
        /// <param name="pName">点编号</param>
        public void SetPointPFile(string pFileName, string pName)
        {
            if (PointFile.GetPointFile().ContainsKey(pFileName))
            {
                if (!PointFile.IsPointContainsKey(pFileName, pName))
                {
                    int dp = 0;

                    if (PointFile.GetPointFile(pFileName).Count > 0)
                    {
                        dp = PointFile.GetPointFile(pFileName)[PointFile.GetPointFile(pFileName).Count - 1].P + 1;
                    }

                    PointFile.GetPointFile()[pFileName].Add(new PointFile()
                    {
                        P = (short)dp,
                        Name = pName,
                        X = X,
                        Y = Y,
                        Z = Z,
                        U = U,
                        V = V,
                        W = W,
                        Local = Tool,
                    });
                }
                else
                {
                    for (int i = 0; i < PointFile.GetPointFile()[pFileName].Count; i++)
                    {
                        if (PointFile.GetPointFile()[pFileName][i].Name == pName)
                        {
                            this.GetPoints(out double x, out double y, out double z, out double u, out double v, out double w);
                            PointFile.GetPointFile()[pFileName][i].X = x;
                            PointFile.GetPointFile()[pFileName][i].Y = y;
                            PointFile.GetPointFile()[pFileName][i].Z = z;
                            PointFile.GetPointFile()[pFileName][i].U = u;
                            PointFile.GetPointFile()[pFileName][i].V = v;
                            PointFile.GetPointFile()[pFileName][i].W = w;
                            PointFile.GetPointFile()[pFileName][i].Local = Tool;
                        }
                    }
                }
            }
        }

        public void SetPoint(double x, double y)
        {
        }

        public void SetPointU(double? u)
        {
            if (u != null)
            {
            }
        }

        public void SetPointV(double? v)
        {
            if (v != null)
            {
            }
        }

        public void SetPointW(double? w)
        {
            if (w != null)
            {
            }
        }

        public void SetPointX(double x)
        {
            throw new NotImplementedException();
        }

        public void SetPointY(double y)
        {
        }

        public void SetPointZ(double z)
        {
           
        }

        public void SetSeep(double seelp, double? addSeelp = null, double? accSeelp = null)
        {
            Seelp = seelp;
            this.SendCommand("SpeedS", seelp.ToString(), addSeelp.ToString(), accSeelp.ToString());
        }

        public void Stop()
        {
            SetCommand("Quit");
            return;
        }

        /// <summary>
        /// 获取轨迹路径
        /// </summary>
        /// <returns></returns>
        public List<string> GetRunCode()
        {
            return new List<string>() { "Go", "PTP", "Move", "Glue" };
        }

        /// <summary>
        /// 下载指定名的点集合
        /// </summary>
        /// <returns></returns>
        public List<PointFile> GetFilePoints(string fileName = null)
        {
            ErrMeaessge = "";
            List<PointFile> listP = new List<PointFile>();
            this.InsSet.ReceiveTimeout = 1000;
            byte[] vsSD = new byte[4000];
            int D = 0;
            try
            {
                string DAS;
                if (fileName == null)
                {
                    this.SendCommand("GetPList", " ");
                }
                else
                {
                    this.SendCommand("GetPList", fileName + ".pts");
                }
                this.InsSet.ReceiveTimeout = 10000;
                D = this.InsSet.Receive(vsSD);
                DAS = this.GetEncoding().GetString(vsSD, 0, D);
                if (DAS.StartsWith(this.GetCRLF()))
                {
                    DAS = DAS.Remove(0, this.GetCRLF().Length);
                }
                if (DAS.Contains("文件不存在"))
                {
                    ErrMeaessge = DAS;
                    return listP;
                }
                string[] dasf = DAS.Split('|');
                int ds = int.Parse(dasf[0]);
                for (int i = 0; i < ds; i++)
                {
                    string[] DATA = dasf[i + 1].Split(',');
                    if (DATA.Length >= 15)
                    {
                        Int16 dts = Convert.ToSByte(DATA[0]);

                        if (dts > i)
                        {
                            for (int i2 = 0; i2 < dts - i; i2++)
                            {
                                listP.Add(new PointFile()
                                {
                                    P = (Int16)(i + i2),
                                });
                            }
                        }

                        listP.Add(new PointFile()
                        {
                            P = Convert.ToSByte(DATA[0]),
                            Name = DATA[1],
                            X = Convert.ToSingle(DATA[2]),
                            Y = Convert.ToSingle(DATA[3]),
                            Z = Convert.ToSingle(DATA[4]),
                            U = Convert.ToSingle(DATA[5]),
                            V = Convert.ToSingle(DATA[6]),
                            W = Convert.ToSingle(DATA[7]),
                            Local = Convert.ToSByte(DATA[8]),
                            Hand = Convert.ToSByte(DATA[9]),
                            Elbow = Convert.ToSByte(DATA[10]),
                            Wrist = Convert.ToSByte(DATA[11]),
                            J1Flag = Convert.ToSByte(DATA[12]),
                            J4Flag = Convert.ToSByte(DATA[13]),
                            J6Flag = Convert.ToSByte(DATA[14]),
                            PointSt = DATA[15],
                        });
                    }
                }

                return listP;
            }
            catch (Exception)
            {
            }
            return listP;
        }

        [Editor(typeof(PLC.ListCyinderControl.Editor), typeof(UITypeEditor))]
        /// <summary>
        ///
        /// </summary>
        public List<string> ListCylin { get; set; } = new List<string>();

        public List<string> ListPLCIO { get; set; }
        public string Execnte_Code { get; set; }

        /// <summary>
        /// 接受信息
        /// </summary>
        /// <param name="ReceiveTimeout">超时时间</param>
        /// <returns>接受到的字符串</returns>
        public string Receive(int ReceiveTimeout)
        {
            string ReString = "";
            try
            {
                this.InsSet.ReceiveTimeout = ReceiveTimeout;
                int dleng = 0;
                byte[] bureff = new byte[4000];
                dleng = this.InsSet.Receive(bureff);
                ReString = this.GetEncoding().GetString(bureff, 0, dleng);
                if (ReString.StartsWith(this.GetCRLF()))
                {
                    ReString = ReString.Remove(0, this.GetCRLF().Length);
                }
            }
            catch (Exception)
            {
            }
            return ReString;
        }

        /// <summary>
        /// 上穿指定的点文件并保存
        /// </summary>
        /// <param name="fileName">点文件名</param>
        /// <returns>成功返回true</returns>
        public bool LoandPointFile(string fileName)
        {
            List<PointFile> listP = PointFile.GetPointFile(fileName);
            string da = listP.Count.ToString() + "|";
            for (int i = 0; i < listP.Count; i++)
            {
                da += listP[i].P + ";" + listP[i].Name + ";" + listP[i].X + ";" + listP[i].Y + ";" + listP[i].Z + ";" + listP[i].U + ";" +
                    listP[i].V + ";" + listP[i].W + ";" + listP[i].Local + ";" + listP[i].Hand + ";" + listP[i].Elbow +
                    ";" + listP[i].Wrist + ";" + listP[i].J1Flag + ";" + listP[i].J4Flag + ";" + listP[i].J6Flag +
                    "|";
            }
            this.SendCommand("SetPList", fileName + ".pts", da);

            return false;
        }

        public override void ShowForm(string ds)
        {
            try
            {
                DebugRobot debugRobot = new DebugRobot(this);
                debugRobot.Show();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 设置IO
        /// </summary>
        /// <param name="id">IOid位号</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetIOOut(string id, string value)
        {
            return false;
        }

        /// <summary>
        /// 获取16位IO的值，
        /// </summary>
        /// <param name="isOut">ture为输出，默认false输入</param>
        /// <returns></returns>
        public bool[] GetIOOuts(bool isOut = false)
        {
            if (isOut)
            {
                return StaticCon.ConvertIntToBoolArray(MemOutW, 16);
            }
            else
            {
                List<bool> lisbools = new List<bool>();
                lisbools.AddRange(StaticCon.ConvertIntToBoolArray(MemInW1, 16));
                lisbools.AddRange(StaticCon.ConvertIntToBoolArray(MemInW2, 16));
                return lisbools.ToArray();
            }
        }

        //public bool GetMode()
        //{
        //     return DebugMode;
        // }
    }
}