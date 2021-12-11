using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using static ErosSocket.ErosConLink.UClass.ErosValues;

namespace ErosSocket.ErosConLink
{
    /// <summary>
    /// 通信接口
    /// </summary>
    public interface IErosLinkNet : IErosValueD
    {
        /// <summary>
        /// 异步连接
        /// </summary>
        /// <param name="isCycle">是否重连</param>
        /// <returns></returns>
        bool AsynLink(bool isCycle = true);

        /// <summary>
        /// 传递数组参数标识
        /// </summary>
        string Identifying { get; set; }

        /// <summary>
        /// 参数模式,数组或变量表模式
        /// </summary>
        SocketClint.SplitMode Split_Mode { get; set; }

        string IP { get; set; }
        int Port { get; set; }
        bool IsConn { get; set; }

        /// <summary>
        /// 错误标识
        /// </summary>
        bool ErrBool { get; set; }

        /// <summary>
        /// 返回信息超时时间
        /// </summary>
        int GetOutTime { get; set; }

        /// <summary>
        /// 发生字符串信息
        /// </summary>
        /// <param name="dataStr">信息</param>
        /// <returns></returns>
        bool Send(string dataStr);

        /// <summary>
        /// 发生字符串信息，并等待返回信息
        /// </summary>
        /// <param name="dataStr">信息</param>
        /// <param name="data">返回信息</param>
        /// <returns>是否成功</returns>
        bool Send(string dataStr, out string data);
    }

    /// <summary>
    /// 本地变量表接口
    /// </summary>
    public interface IErosValueD
    {
        string Name { get; set; }

        /// <summary>
        /// 变量表
        /// </summary>
        UClass.ErosValues KeysValues { get; set; }

        /// <summary>
        /// 根据地址写入值
        /// </summary>
        /// <param name="id">地址</param>
        /// <param name="value">值</param>
        /// <param name="data">信息</param>
        /// <returns>是否成功</returns>
        bool SetIDValue(string id, dynamic value, out string data);

        /// <summary>
        /// 根据地址写入值
        /// </summary>
        /// <param name="id">地址</param>
        /// <param name="type">值类型字符串</param>
        /// <param name="value">值</param>
        /// <param name="data">信息</param>
        /// <returns>是否成功</returns>
        bool SetIDValue(string id, string type, string value, out string data);

        /// <summary>
        /// 写入指定动态变量值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="value"></param>
        /// <param name="errStr"></param>
        /// <returns></returns>
        bool SetValue(ErosValueD item, dynamic value, out string errStr);

        /// <summary>
        /// 写入指定表名值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="errStr"></param>
        /// <returns></returns>
        bool SetValue(string key, string value, out string errStr);

        /// <summary>
        /// 写表名数组值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="errStr"></param>
        /// <returns></returns>
        bool SetValues(string[] key, string[] value, out string errStr);

        /// <summary>
        /// 读取动态变量值
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool GetValue(ErosValueD item);

        /// <summary>
        /// 读取表名数组值
        /// </summary>
        /// <param name="Keys"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        bool GetValues(string[] Keys, out string[] values);

        /// <summary>
        /// 读取类型字节长度
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        byte GetTypeLength(string type);

        /// <summary>
        /// 叠加地址
        /// </summary>
        /// <param name="type"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        string GetTypeLengthAddress(string type, string address);

        /// <summary>
        /// 根据ID读取值
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool GetIDValue(string ID, string type, out dynamic value);

        ///<summary>
        /// 根据ID读取值
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool GetIDValue(string ID, out string value);

        /// <summary>
        /// 根据地址ID读取值
        /// </summary>
        /// <param name="linkID"></param>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool GetIDValue(byte linkID, string ID, out string value);

        /// <summary>
        /// 跟新周期
        /// </summary>
        long CDataTime { get; set; }

        string ErrMesage { get; set; }

        bool SendBusy { get; }

        DateTime _DataTime { get; set; }

        void SetKeysValues(DataGridView dataGridView);
    }

    public interface IReSendSocke
    {
        void RSend(string snedDataStr, Socket socket);
    }

    /// <summary>
    /// Sokcet通信类
    /// </summary>
    public class SocketClint : ProjectC, IErosLinkNet, IReSendSocke, ProjectNodet.IClickNodeProject
    {
        private bool keysVaWr;

        private bool Wring;

        [Browsable(false)]
        /// <summary>
        /// 调试模式
        /// </summary>
        public bool DebugMode { get; set; }

        [Browsable(false)]
        public DebugPLC.PLCRun PLCRun { get; set; }

        /// <summary>
        /// 分割模式
        /// </summary>
        public enum SplitMode
        {
            KeyValue = 0,
            Array = 1,
        }

        ///// <summary>
        ///// 动态属性
        ///// </summary>
        ///// <param name="control"></param>
        /////// <param name="data"></param>
        //public  void UpProperty(PropertyForm control, object data)
        //{
        //    //base.UpProperty(control, data);
        //    //control.动态属性.Text = "变量表";
        //    //DataGridView dataGridView = control.dataGridView3;
        //    TabPage tabPage = new TabPage();
        //    tabPage.Text = "变量表";
        //    control.tabControl1.TabPages.Add(tabPage);
        //    DataGridView dataGridView = new DataGridView();
        //    dataGridView.Dock = DockStyle.Fill;
        //    tabPage.Controls.Add(dataGridView);
        //    dataGridView.Columns.Add("名称", "名称");
        //    dataGridView.Columns.Add("值", "值");
        //    tabPage.Enter += TabPage_Click2;
        //    for (int i = 0; i < dataGridView.Columns.Count; i++)
        //    {
        //        dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
        //        dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        //    }
        //    void TabPage_Click2(object sender, EventArgs e)
        //    {
        //        dataGridView.Rows.Clear();
        //        int d = 0;
        //        if (KeysValues == null)
        //        {
        //            return;
        //        }
        //        if (KeysValues.DictionaryValueD != null)
        //        {
        //            foreach (var item in KeysValues.DictionaryValueD)
        //            {
        //                d = dataGridView.Rows.Add();
        //                dataGridView.Rows[d].Cells[0].Value = item.Key;
        //                dataGridView.Rows[d].Cells[1].Value = item.Value.Value;
        //            }
        //        }
        //    }
        //    tabPage = new TabPage();
        //    tabPage.Text = "设备控制";
        //    PropertyGrid propertyGrid = new PropertyGrid();
        //    tabPage.Controls.Add(propertyGrid);
        //    propertyGrid.Dock = DockStyle.Fill;
        //    if (PLCRun == null)
        //    {
        //        PLCRun = new DebugPLC.PLCRun();

        //    }
        //    PLCRun.Name = this.Name;
        //    propertyGrid.SelectedObject = this.PLCRun;
        //    control.tabControl1.TabPages.Add(tabPage);
        //}

        public Control GetThisControl()
        {
            Form form = ShowForm();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.Show();
            return form;
        }

        public string GetName()
        {
            return this.Name;
        }

        #region 变量

        public override string FileName => "Socket";

        public override string SuffixName => ".socketClint";
        public override string ProjectTypeName => "链接类";

        /// <summary>
        /// 超时
        /// </summary>
        private ManualResetEvent timeOutObject;

        /// <summary>
        /// 超时
        /// </summary>
        private ManualResetEvent timeOutRest;

        /// <summary>
        /// 链接委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public delegate string DelegateSocket<T>(T key);

        /// <summary>
        /// 链接委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public delegate string DelegateBytesSocket<T>(T key, SocketClint socket, Socket socketR);

        ///// <summary>
        ///// 链接委托
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public delegate string DelegateSocketCycle<(T key);
        /// <summary>
        /// 链接接受委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public delegate TextBoxBase DelegateSocketRe<T>(T key, byte[] buffrs);

        [DescriptionAttribute("设备连接类型"), Category("连接机制"), DisplayName("链接类型"),
            TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("ListClass", false)]
        /// <summary>
        /// 链接类型
        /// </summary>
        public string NetType { get; set; }

        /// <summary>
        /// 链接实例
        /// </summary>
        protected System.Net.Sockets.Socket Insocket;

        public System.Net.Sockets.Socket Socket(Socket socket = null)
        {
            if (socket == null)
            {
                return Insocket;
            }
            Insocket = socket;
            return Insocket;
        }

        [DescriptionAttribute("是否在状态栏显示连接信息"), Category("显示"), DisplayName("显示交互信息")]
        public bool IsAlramText { get; set; } = false;

        [DescriptionAttribute("是否在状态栏显示连接状态信息"), Category("显示"), DisplayName("显示状态信息")]
        public bool IsStataText { get; set; } = false;

        [DescriptionAttribute("目标IP"), Category("连接机制"), DisplayName("目标IP")]
        /// <summary>
        /// 目标地址
        /// </summary>
        public string IP { get; set; } = "127.0.0.1";

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual Control GetDebugControl(Form form = null)
        {
            return null;
        }

        [DescriptionAttribute("目标端口"), Category("连接机制"), DisplayName("目标端口")]
        /// <summary>
        /// 目标端口
        /// </summary>
        public int Port { get; set; } = 502;

        [DescriptionAttribute("本地IP"), Category("连接机制"), DisplayName("本地IP")]
        /// <summary>
        /// 目标地址
        /// </summary>
        public string EndIP { get; set; } = "0.0.0.0";

        [DescriptionAttribute("指定本地端口"), Category("连接机制"), DisplayName("本地端口")]
        /// <summary>
        /// 目标端口
        /// </summary>
        public int EndPort { get; set; } = 0;

        [DescriptionAttribute("链接失败目标间隔"), Category("连接机制"), DisplayName("链接间隔MS")]
        /// <summary>
        /// 链接时间
        /// </summary>
        public int LinkTime { get; set; } = 5000;

        [DescriptionAttribute("检测链接是否断开"), Category("连接机制"), DisplayName("心跳间隔S")]
        /// <summary>
        /// 心跳计时
        /// </summary>
        public int SendTime { get; set; } = 10;

        /// <summary>
        /// 心跳标识
        /// </summary>
        [DescriptionAttribute("定时对连接写入字符;对于变量机制的连接,请填写指定的Bool地址，定时写True"), Category("连接机制"), DisplayName("心跳符号")]
        public string SendString { get; set; } = "$";

        [Browsable(false)]
        public Dictionary<string, string> DicSendMeseage { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 发送中
        /// </summary>
        public bool SendBusy { get; protected set; }

        [DescriptionAttribute("错误信息"), Category("连接状态"), DisplayName("错误信息")]
        public string ErrMesage { get; set; }

        [DescriptionAttribute("链接状态"), Category("连接状态"), ReadOnlyAttribute(true), DisplayName("链接状态")]
        /// <summary>
        /// 链接状态
        /// </summary>
        public string LinkState { get; set; } = "";

        [DescriptionAttribute("链接状态成功为true"), Category("连接状态"), ReadOnlyAttribute(true), DisplayName("链接成功标志")]
        public bool IsConn
        {
            get;
            set;
        }

        [DescriptionAttribute("指定的动态变量表"), Category("执行机制"), DisplayName("变量表")]
        /// <summary>
        /// 变量表
        /// </summary>
        public string ValusName { get; set; }

        [DescriptionAttribute("参数分割模式，键值或数组"), Category("执行机制"), DisplayName("参数分割模式")]
        public SplitMode Split_Mode { get; set; } = SplitMode.Array;

        [DescriptionAttribute("参数分割符号，发送接收参数分割符"), Category("执行机制"), DisplayName("分割符合")]
        public char Split { get; set; } = ',';

        [DescriptionAttribute("发送参数标识，参数标识头"), Category("执行机制"), DisplayName("发送参数标识")]
        public string Identifying { get; set; } = "getdata,";

        [DescriptionAttribute("事件信息"), Category("执行机制"), DisplayName("事件信息")]
        /// <summary>
        /// 事件信息
        /// </summary>
        public string Event { get; set; }

        [DescriptionAttribute("获取返回信息超时时间"), Category("执行机制"), DisplayName("读超时时间")]
        public int GetOutTime { get; set; } = 5000;

        [Browsable(false)]
        /// <summary>
        /// 变量表更新时间.ToString("yyyy-MM-dd HH:mm:ss.fff")
        /// </summary>
        public DateTime _DataTime { get; set; } = new DateTime();

        [DescriptionAttribute("变量表更新时间"), Category("执行机制"), ReadOnlyAttribute(true), DisplayName("更新周期")]
        /// <summary>
        /// 跟新周期时间
        /// </summary>
        public long CDataTime { get; set; }

        /// <summary>
        /// 跟新周期
        /// </summary>
        public System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        public System.Diagnostics.Stopwatch watchSen = new System.Diagnostics.Stopwatch();

        #region 事件信息

        [Browsable(false)]
        [DescriptionAttribute("接受数据集合"), ReadOnlyAttribute(true), DisplayName("接受数据")]
        /// <summary>
        ///接受数据集合
        /// </summary>
        public List<string> ReciveStr { get; set; } = new List<string>();

        /// <summary>
        /// 接受事件
        /// </summary>
        public event DelegateBytesSocket<byte[]> PassiveEvent;

        /// <summary>
        /// 接受事件
        /// </summary>
        public event DelegateBytesSocket<StringBuilder> PassiveStringBuilderEvent;

        /// <summary>
        /// 接受数据添加到textbox控件上，
        /// </summary>
        public event DelegateSocketRe<Encoding> PassiveTextBoxEvent;

        /// <summary>
        /// 周期事件
        /// </summary>
        public event DelegateSocket<SocketClint> CycleEvent;

        /// <summary>
        /// 连接触发事件
        /// </summary>
        public event DelegateSocket<bool> LinkO;

        #endregion 事件信息

        [Browsable(false)]
        /// <summary>
        /// 键值变量表
        /// </summary>
        public UClass.ErosValues KeysValues
        {
            get
            {
                return keysValues;
            }
            set { keysValues = value; }
        }

        private UClass.ErosValues keysValues;

        public dynamic GetKeyValue(string key)
        {
            return KeysValues[key];
        }

        public virtual HslCommunication.OperateResult<byte[]> GetAddressByte(string address, ushort length)
        {
            return null;
        }

        protected HslCommunication.Core.IReadWriteNet net;

        public virtual HslCommunication.Core.IReadWriteNet GetRead()
        {
            return net;
        }

        public virtual HslCommunication.Core.IByteTransform GetByteTransform()
        {
            return null;
        }

        public virtual HslCommunication.Core.IByteTransform GetByteTransform(string address, ushort length, out HslCommunication.OperateResult<byte[]> bytes)
        {
            bytes = null;
            return null;
        }

        /// <summary>
        /// 定时任务参数
        /// </summary>
        public Newtonsoft.Json.Linq.JObject[] ListJobject = { };

        private Thread Taskthread;

        /// <summary>
        /// 接受事件
        /// </summary>
        private void ReadMesage(string mesage, Socket socketR)
        {
            mesage = mesage.ToLower();
            Task.Run(() =>
            {
                string[] mesages = mesage.Split(',');

                if (mesage.Contains('{'))
                {
                    ErosSocket.DebugPLC.Pop_UpWindow pop_UpWindow = new ErosSocket.DebugPLC.Pop_UpWindow();
                    //pop_UpWindow.BringToFront();
                    string[] meaget = mesage.Remove(mesage.IndexOf('{')).Split(',');
                    string text = mesage.Remove(0, mesage.IndexOf('{') + 1);
                    text = text.Trim('}');
                    string forText = this.Name + " : " + DateTime.Now + " : " + meaget[1];
                    List<string> vs = new List<string>();
                    switch (meaget[0])
                    {
                        case "Message1":
                            for (int i = 4; i < meaget.Length; i++)
                            {
                                if (meaget[i] != "")
                                {
                                    vs.Add(meaget[i]);
                                }
                            }
                            pop_UpWindow.UpWindow(text, forText, vs, meaget[2], int.Parse(meaget[3]));
                            break;

                        case "Message2":
                            pop_UpWindow.UpWindow(text, forText, MessageBoxButtons.YesNo);
                            break;

                        case "Message4":
                            for (int i = 3; i < meaget.Length; i++)
                            {
                                if (meaget[i] != "")
                                {
                                    vs.Add(meaget[i]);
                                }
                            }
                            pop_UpWindow.UpWindow(text, forText, vs, meaget[2]);
                            break;

                        case "Message3":
                            pop_UpWindow.UpWindow(text, forText);
                            break;

                        default:
                            break;
                    }
                    if (pop_UpWindow.Tag != null)
                    {
                        this.Send(meaget[0] + "," + pop_UpWindow.Tag.ToString());
                    }
                }
                if (mesages[0] == "messageerron")
                {
                    //DicSocket.Instance.SetLinkSTime(false);
                }
                else if (mesages[0] == "messageerr")
                {
                    //DicSocket.Instance.SetLinkSTime(true, int.Parse(mesage.Split(',')[1]));
                }
                else if (mesages[0] == "messageerr")
                {
                    //DicSocket.Instance.SetLinkSTime(true);
                }
                else if (mesages[0] == "getvalues")
                {
                    string dataStr = "";
                    string err = "err:";
                    for (int i = 1; i < mesages.Length; i++)
                    {
                        string[] teim = mesages[i].Split('.');
                        if (!StaticCon.SocketClint.ContainsKey(teim[0]) || !StaticCon.SocketClint[teim[0]].KeysValues.DictionaryValueD.ContainsKey(teim[1]))
                        {
                            if (!StaticCon.SocketClint.ContainsKey(teim[0]))
                            {
                                err += mesages[i] + "{链接名不正确};";
                            }
                            if (!StaticCon.SocketClint[teim[0]].KeysValues.DictionaryValueD.ContainsKey(teim[1]))
                            {
                                err += mesages[i] + "{变量名不正确};";
                            }
                        }
                        dataStr += StaticCon.GetLingkNameValue(mesages[i]).ToString() + ",";
                    }
                    if (err.Length > 4)
                    {
                        this.RSend(err, socketR);
                    }
                    else
                    {
                        this.RSend("Values," + dataStr.TrimEnd(','), socketR);
                    }
                }
                else if (mesages[0] == "setvalues")
                {
                    string[] meaget = mesage.Split(',');
                    string dataStr = "";
                    string errs = "Err:";
                    for (int i = 1; i < meaget.Length; i++)
                    {
                        string[] meagetData = meaget[i].Split('=');
                        if (meagetData.Length == 2)
                        {
                            if (StaticCon.SetLingkValue(meagetData[0], meagetData[1], out string err))
                            {
                            }
                            if (err != "")
                            {
                                err += "!";
                                errs += meaget[i] + "{" + err + "}";
                            }
                        }
                        else
                        {
                            errs += meaget[i] + "{缺少赋值符=！}";
                        }
                    }
                    if (errs.Length < 4)
                    {
                        this.RSend("SetDone", socketR);
                    }
                    else
                    {
                        this.RSend("Set" + errs, socketR);
                    }
                }
                else if (mesage == "debugmode")
                {
                    this.RSend("DebugMode," + ProjectINI.DebugMode.ToString(), socketR);
                }
            });
        }

        [DescriptionAttribute("扫码字符头标识"), CategoryAttribute("扫码"), DisplayName("头标识")]
        public string CodeStartWith { get; set; } = "QRCode";

        [DescriptionAttribute("定时任务状态"), CategoryAttribute("定时任务"), ReadOnlyAttribute(true)]
        /// <summary>
        /// 任务状态
        /// </summary>
        public string TimeTaskState { get; set; } = "未激活";

        [DescriptionAttribute("定时任务时"), CategoryAttribute("定时任务"), ReadOnlyAttribute(true)]
        public uint TimeH { get { return timeH; } set { timeH = value; } }

        private uint timeH;

        [DescriptionAttribute("定时任务分"), CategoryAttribute("定时任务"), ReadOnlyAttribute(true)]
        public uint TimeM { get { return timeM; } set { timeM = value; } }

        private uint timeM;

        [DescriptionAttribute("定时任务剩余时"), CategoryAttribute("定时任务"), ReadOnlyAttribute(true)]
        public uint TimeCDH { get; set; }

        [DescriptionAttribute("定时任务剩余分"), CategoryAttribute("定时任务"), ReadOnlyAttribute(true)]
        public uint TimeCDM { get; set; }

        [DescriptionAttribute("定时任务剩余秒"), CategoryAttribute("定时任务"), ReadOnlyAttribute(true)]
        public uint TimeCDSS { get; set; }

        [DescriptionAttribute("定时任务当前ID"), CategoryAttribute("定时任务"), ReadOnlyAttribute(true)]
        public int CDID { get; set; }

        [DescriptionAttribute("定时任务数"), CategoryAttribute("定时任务"), ReadOnlyAttribute(true)]
        public int CD { get; set; }

        [DescriptionAttribute("设备状态"), CategoryAttribute("设备"), ReadOnlyAttribute(true)]
        /// <summary>
        /// 设备状态
        /// </summary>
        public string FacillttState { get; set; } = "停止";

        /// <summary>
        /// modbusLog日志
        /// </summary>
        protected HslCommunication.LogNet.ILogNet LogNet { get; set; }

        /// <summary>
        /// 关闭标识
        /// </summary>
        protected bool CloseBool;

        [DescriptionAttribute("是否链接中"), CategoryAttribute("连接状态"), ReadOnlyAttribute(true)]
        /// <summary>
        /// 链接中
        /// </summary>
        public bool Linking { get; set; }

        [DescriptionAttribute("错误标识"), CategoryAttribute("连接状态"), ReadOnlyAttribute(true), DisplayName("错误状态")]
        /// <summary>
        /// 错误标识
        /// </summary>
        public bool ErrBool { get; set; }

        [DescriptionAttribute("长连接/True短连接"), CategoryAttribute("连接类型"), DisplayName("长连接/True短连接")]
        public bool IsContData { get; set; }

        public const string constName = "nameID";
        public const string constOutIP = "outIP";
        public const string constOutProt = "outPort";
        public const string constNetType = "NetType";
        public const string constValueName = "ValueName";
        public const string constEvent = "Event";

        public byte[] Recivebuffer;

        public int ReciveBufferLenth;

        #endregion 变量

        public static List<Type> ListClass { get; set; } = new List<Type>();

        /// <summary>
        /// 获得所有连接类型
        /// </summary>
        /// <returns></returns>
        public static List<string> GetListClassName()
        {
            List<string> listStr = new List<string>();
            if (ListClass != null)
            {
                AddType(typeof(SocketClint));
                AddType(typeof(ModbusTCPClint));
                AddType(typeof(S71200));
                AddType(typeof(三菱MC));
                AddType(typeof(SocketServer));
                AddType(typeof(SocketClint));
                AddType(typeof(海达U700));
                AddType(typeof(Clcd9700));
                AddType(typeof(SECS_GEM));
                AddType(typeof(S7200PPI));
                AddType(typeof(FY6400));
                AddType(typeof(RS232Con));
                AddType(typeof(Vision2.SocketCFile.ErosConLink.三菱FX3U));
                //AddType(typeof(UCallsU));
                AddType(typeof(DebugPLC.Robot.EpsenRobot6));
                foreach (var item in ListClass)
                {
                    listStr.Add(item.Name);
                }
            }
            return listStr;
        }

        /// <summary>
        /// 创建带变量表的链接
        /// </summary>
        /// <param name="valusName">变量表名</param>
        public SocketClint(string valusName) : this()
        {
            this.ValusName = valusName;
            KeysValues = new UClass.ErosValues(this.Name, this.ValusName);
        }

        public SocketClint()
        {
            AddType(this.GetType());
            NetType = this.GetType().Name;
            Information = "对设备的链接程序";
            PassiveTextBoxEvent += EpsenV01_PassiveTextBoxEvent;
        }

        private TextBoxBase EpsenV01_PassiveTextBoxEvent(Encoding key, byte[] buffrs)
        {
            return AlarmText.ThisF.richTextBox1;
        }

        private static void AddType(Type type)
        {
            if (type != null)
            {
                if (!ListClass.Contains(type))
                {
                    ListClass.Add(type);
                }
            }
        }

        private void AddType(string type)
        {
        }

        ~SocketClint()
        {
            if (Insocket != null)
            {
                this.Insocket.Dispose();
                this.Insocket.Close();
            }
        }

        #region 方法

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="socket"></param>
        public void AsynRecive(Socket socket)
        {
            try
            {
                if (socket == null)
                {
                    return;
                }
                if (this.Recivebuffer == null)
                {
                    this.Recivebuffer = new byte[1024 * 1024 * 5];
                }
                //开始接收数据
                socket.BeginReceive(this.Recivebuffer, 0, this.Recivebuffer.Length, SocketFlags.None,
                asyncResult =>
                {
                    try
                    {
                        ReciveBufferLenth = socket.EndReceive(asyncResult);
                        OnEventArge(Recivebuffer.Skip(0).Take(ReciveBufferLenth).ToArray(), socket);
                        watchSen.Restart();
                        if (ReciveBufferLenth != 0)
                        {
                            if (socket.Connected)
                            {
                                AsynRecive(socket);
                            }
                        }
                        else if (ReciveBufferLenth == 0)
                        {
                            this.AsynSend(socket, this.SendString);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ErrerLog(ex);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                this.ErrerLog(ex);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        public void AsynSend(Socket socket, string message)
        {
            if (socket == null || message == string.Empty) return;
            //编码
            byte[] data = encoding.GetBytes(message);
            try
            {
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        //完成发送消息
                        int length = socket.EndSend(asyncResult);
                    }
                    catch (Exception ex)
                    {
                        this.ErrerLog(ex);
                    }
         
                }, null);
            }
            catch (Exception ex)
            {
                this.ErrerLog(ex);
            }
        }

        /// <summary>
        /// 线程接受
        /// </summary>
        public virtual void Receive()
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
                            ReciveBufferLenth = Insocket.Receive(Recivebuffer);
                            if (ReciveBufferLenth == 1) continue;
                            if (ReciveBufferLenth > 1)
                            {
                                OnEventArge(Recivebuffer.Skip(0).Take(ReciveBufferLenth).ToArray(), Insocket);
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

        public Stopwatch watcht = new Stopwatch();

        public void AlwaysReceiveReset()
        {
            try
            {
                if (timeOutRest == null)
                {
                    timeOutRest = new ManualResetEvent(false);
                }
                timeOutRest.Reset();
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 等待通信反馈
        /// </summary>
        /// <param name="de">等待毫秒</param>
        /// <returns></returns>
        public virtual string AlwaysReceive(int outTime = 5000)
        {
            try
            {
                if (timeOutRest==null)
                {
                    timeOutRest = new ManualResetEvent(false);
                }
                timeOutRest.Reset();
                //timeOutRest = new ManualResetEvent(false);
                if (timeOutRest.WaitOne(outTime, false))
                {
                    return dataStr.ToString();
                }
            }
            catch (Exception re)
            {
                AlarmText.AddTextNewLine(this.Name+"通信等待异常:"+re.Message);
            }
            AlarmText.AddTextNewLine(this.Name + "通信等待异常");
            return "";
        }
        /// <summary>
        /// 等待通信反馈
        /// </summary>
        /// <param name="de">等待毫秒</param>
        /// <returns></returns>
        public virtual bool AlwaysReceive(out  string datas,  int outTime = 5000)
        {
            datas = "";
            try
            {
                if (timeOutRest == null)
                {
                    timeOutRest = new ManualResetEvent(false);
                }
                //timeOutRest = new ManualResetEvent(false);
                if (timeOutRest.WaitOne(outTime, false))
                {
                    datas = dataStr.ToString();
                    return true    ;
                }
            }
            catch (Exception re)
            {
                AlarmText.AddTextNewLine(this.Name + "通信等待异常:" + re.Message);
            }
            AlarmText.AddTextNewLine(this.Name + "通信等待异常");
            return false;
        }
        /// <summary>
        ///触发首次连接事件
        /// </summary>
        /// <param name="isConn"></param>
        public virtual void OnMastLink(bool isConn)
        {
            this.LinkO?.Invoke(isConn);
        }

        public bool RecivesDone;
        private StringBuilder dataStr;

        /// <summary>
        /// 单次触发接受事件
        /// </summary>
        /// <param name="key"></param>
        public virtual void OnEventArge(byte[] key, Socket socketR)
        {
            if (ReciveStr == null)
            {
                ReciveStr = new List<string>();
            }
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            try
            {
                PassiveEvent?.Invoke(key, this, socketR);
             
                  if (key.Length < 50000)
                {
                    if (key.Length > 2)
                    {
                        dataStr = new StringBuilder(GetEncoding().GetString(key));
                        if (timeOutRest != null)
                        {
                            timeOutRest.Set();
                        }
                        PassiveStringBuilderEvent?.Invoke(dataStr, this, socketR);
                    }
                    if (IsAlramText)
                    {
                        AddTextBox("(R):" + dataStr, System.Drawing.Color.Green);
                    }
                }
     
            }
            catch (Exception ex)
            {
                ErrerLog(ex);
            }
        }

        /// <summary>
        /// 单次周期事件
        /// </summary>
        /// <param name="key"></param>
        public virtual void OnCycleEvent(string key)
        {
            GetValues();
            CycleEvent?.Invoke(this);
        }

        /// <summary>
        ///关闭链接释放Socket
        /// </summary>
        public override void Close()
        {
            CloseBool = true;
            try
            {
                Linking = false;
                this.LinkState = "关闭";

                this.IsConn = false;

                LinkO?.Invoke(IsConn);
                TextBoxBase tex = PassiveTextBoxEvent?.Invoke(GetEncoding(), new byte[] { });
                if (tex != null)
                {
                    tex.AppendText(this.Name + ":" + this.LinkState + Environment.NewLine);
                }
                SocketClint.SafeClose(this.Insocket);
            }
            catch (Exception ex)
            {
            }
            //this.Insocket = null;
        }

        public override void Dispose()
        {
            PassiveTextBoxEvent -= EpsenV01_PassiveTextBoxEvent;
            this.Recivebuffer = null;
            this.ReciveStr = null;
            Close();
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
            }
            if (Insocket != null)
            {
                Insocket.Dispose();
                Insocket = null;
            }

            base.Dispose();
        }

        /// <summary>
        /// 激活循环事件,线程接受，与循环线程循环读取变量表等
        /// </summary>
        public virtual void EnabledRunCycleEvent()
        {
            Thread.Sleep(5000);

            Thread thread = new Thread(() =>
            {
                while (!this.CloseBool)
                {
                    try
                    {
                        watch.Restart();
                        if (this.Name == StaticCon.DebugID)
                        {
                        }
                        if (this.LinkState == "连接成功")
                        {
                            OnCycleEvent("");
                            Thread.Sleep(1);
                        }
                        else
                        {
                            Thread.Sleep(200);

                            IsConn = this.AsynLink(false);
                            LinkO?.Invoke(IsConn);
                        }
                        CDataTime = watch.ElapsedMilliseconds;
                        watch.Reset();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
             );
            thread.Priority = ThreadPriority.Highest;
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 首次链接
        /// </summary>
        public virtual void LinkSucceed()
        {
        }

        /// <summary>
        /// 将当前通信写入XML
        /// </summary>
        /// <returns>成功返回ture</returns>
        public virtual bool SocketXML(string PathXML)
        {
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(PathXML))
            {
                XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(declaration);
                XmlElement xmlElement = doc.CreateElement("Sockets");
                doc.AppendChild(xmlElement);
                XmlElement Sockete = doc.CreateElement("socket");
                doc.Save(PathXML);
            }
            doc.Load(PathXML);
            XmlElement SocketList = doc.DocumentElement;
            //添加根节点
            XmlElement Socket = doc.CreateElement("socket");
            Socket.SetAttribute(constName, this.Name);
            Socket.SetAttribute(constOutIP, this.IP.ToString());
            Socket.SetAttribute(constOutProt, this.Port.ToString());
            Socket.SetAttribute(constNetType, this.NetType.ToString());

            Socket.SetAttribute(constValueName, this.ValusName.ToString());
            Socket.SetAttribute(constEvent, this.Event);
            SocketList.AppendChild(Socket);
            doc.Save(PathXML);
            MessageBox.Show("创建成功");
            return true;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buffr"></param>
        public virtual bool Send(byte[] buffr)
        {
            try
            {
                //RecivesDone = false;
                watchSen.Restart();
                if (this.Insocket == null)
                {
                    return false;
                }
                if (!this.Insocket.Connected)
                {
                    this.Link(this.IP, this.Port);
                    AsynRecive(Insocket);
                }
                if (this.Insocket.Connected)
                {
                    this.Insocket.Send(buffr);
                    if (buffr.Length > 3)
                    {
                        if (IsAlramText)
                        {
                            AddTextBox("(S):" + GetEncoding().GetString(buffr), System.Drawing.Color.GreenYellow);
                        }
                    }
                    return true;
                }
                else
                {
                    if (IsAlramText)
                    {
                        AddTextBox("(S)发送失败(" + this.LinkState + "):" + GetEncoding().GetString(buffr), System.Drawing.Color.Red);
                    }
                }
       
                buffr = null;
            }
            catch (Exception ex)
            {
                ErrerLog(ex);
                ErrMesage = ex.Message;
            }
            return false;
        }

        public string GetCRLF()
        {
            if (FTU == "CRLF")
            {
                return "\r\n";
            }
            else if (FTU == "CR")
            {
                return "\n";
            }
            else
            {
                return "\r";
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="dataStr"></param>
        /// <returns></returns>
        public virtual bool Send(string dataStr)
        {
            if (FTU == "CRLF")
            {
                dataStr = dataStr + "\r\n";
            }
            else if (FTU == "CR")
            {
                dataStr = dataStr + "\n";
            }
            else if (FTU == "LF")
            {
                dataStr = dataStr + "\r";
            }
            return this.Send(GetEncoding().GetBytes(dataStr));

        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="dataStr"></param>
        /// <param name="socket"></param>
        public void RSend(string dataStr, Socket socket)
        {
            if (FTU == "CRLF")
            {
                dataStr = dataStr + "\r\n";
            }
            else if (FTU == "CR")
            {
                dataStr = dataStr + "\n";
            }
            else if (FTU == "LF")
            {
                dataStr = dataStr + "\r";
            }
            byte[] buffr = GetEncoding().GetBytes(dataStr);
            try
            {
                watchSen.Restart();
                if (socket == null)
                {
                    return;
                }

                if (socket.Connected)
                {
                    socket.Send(buffr);
                    if (buffr.Length > 3)
                    {
                        if (IsAlramText)
                        {
                            AddTextBox("(S):" + GetEncoding().GetString(buffr).Remove(GetEncoding().GetString(buffr).Length - FTU.Length / 2), System.Drawing.Color.GreenYellow);
                        }
                    }
                    return;
                }
                else
                {
                    if (IsAlramText)
                    {
                        AddTextBox("(S)发送失败(" + this.LinkState + "):" + GetEncoding().GetString(buffr).Remove(GetEncoding().GetString(buffr).Length - FTU.Length / 2), System.Drawing.Color.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrerLog(ex);
                ErrMesage = ex.Message;
            }
        }

        /// <summary>
        /// 发生信息并等待返回信息
        /// </summary>
        /// <param name="dataStr">发生文本</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual bool Send(string dataStr, out string data)
        {
            if (IsConn)
            {
            }
            ReciveBufferLenth = 0;
            bool isDon = Send(dataStr);
            data = "";
            string dtas = "";
            isDon = CallWithTimeout(new Action(Inovet), GetOutTime);
            void Inovet()
            {
                try
                {
                    while (true)
                    {
                        Thread.Sleep(10);
                        if (ReciveBufferLenth != 0)
                        {
                            Thread.Sleep(10);
                            dtas = encoding.GetString(Recivebuffer.Skip(0).Take(ReciveBufferLenth).ToArray());
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrMesage = ex.Message;
                }
            }
            data = dtas;
            return isDon;
        }

        /// <summary>
        /// 启动链接程序
        /// </summary>
        /// <param name="runingLink">true线程链接</param>
        public override void initialization()
        {
            this.CloseBool = false;
            SendBusy = false;
            if (this.Linking)
            {
                if (IsStataText)
                {
                    AddTextBox("启动失败,链接已启动");
                }
                return;
            }
            if (IsStataText)
            {
                AddTextBox(this.Name + "启动链接");
            }
            Linking = true;
            IsConn = false;
            ThreadLink(true);
            base.initialization();
        }

        /// <summary>
        /// 链接不触发事件
        /// </summary>
        /// <param name="isCycle">链接不成功是否重复链接</param>
        public virtual bool AsynLink(bool isCycle = true)
        {
            CloseBool = false;
            this.AsynConnect(isCycle);
            return IsConn;
        }

        public bool Link()
        {
            CloseBool = false;
            return Link(this.IP, this.Port);
        }

        /// <summary>
        /// 链接不触发事件
        /// </summary>
        public virtual bool Link(string ip, int port)
        {
            if (CloseBool)
            {
                return false;
            }
            Insocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint outPoint = new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(port));
            try
            {
                Insocket.Connect(outPoint);
                this.LinkState = "连接成功";
                IsConn = true;
                LinkO?.Invoke(true);
                return true;
            }
            catch (Exception EX)
            {
                this.LinkState = "连接失败";
                ErrerLog(EX);
            }
            LinkO?.Invoke(false);
            return false;
        }

        /// <summary>
        /// 异步连接到服务器
        /// </summary>
        public virtual void AsynConnect(bool isCycle, IPEndPoint ipe = null)
        {
            if (timeOutObject==null)
            {
                timeOutObject = new ManualResetEvent(false);
            }
            if (ipe == null)
            {
                ipe = new IPEndPoint(IPAddress.Parse(this.IP), Convert.ToInt32(this.Port));
            }
            if (CloseBool)
            {
                return;
            }
            //创建套接字
            this.Linking = true;
            if (IsStataText)
            {
                this.AddTextBox("连接" + ipe.ToString());
            }

            Insocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //开始连接到服务器
            Insocket.BeginConnect(ipe, new AsyncCallback(CallBcakMethod), Insocket);
            if (!timeOutObject.WaitOne(LinkTime, false))
            {
                if (!IsConn)
                {
                    if (Insocket != null)
                    {
                        SafeClose(Insocket);
                    }
                    Thread.Sleep(200);

                    if (isCycle)
                    {
                        AsynConnect(isCycle);
                    }
                    this.Linking = false;
                }
            }
        }

        private void CallBcakMethod(IAsyncResult asyncResult)
        {
            try
            {
                Insocket = asyncResult.AsyncState as Socket;
                if (Insocket != null)
                {
                    try
                    {
                        Insocket.EndConnect(asyncResult);
                        AsynRecive(Insocket);
                        timeOutObject.Set();
                        this.Linking = false;
                        string dats = this.Name;
                        IsConn = true;
                        this.LinkState = "连接成功";
                        LinkO?.Invoke(IsConn);
                        if (IsStataText)
                        {
                            this.AddTextBox(this.LinkState);
                        }
                    }
                    catch (Exception EX)
                    {
                        this.Linking = false;
                        IsConn = false;
                        this.LinkState = "连接失败";
                        LinkO?.Invoke(IsConn);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrerLog(ex);
            }
        }

        private bool RuningLink;
        private Thread thread;

        /// <summary>
        /// 开启异步连接，成功返回ture,成功并使能事件
        /// </summary>
        /// <param name="runingLink">true一直重连，false只连一次</param>
        /// <returns></returns>
        public virtual void ThreadLink(bool runingLink)
        {
            RuningLink = runingLink;
            if (this.IsConn)
            {
                if (IsStataText)
                {
                    AddTextBox("链接成功");
                }
                return;
            }
            if (this.Name == StaticCon.DebugID)
            {
            }
            if (IsStataText)
            {
                AddTextBox("链接开始," + this.IP + "," + this.Port);
            }
            if (this.KeysValues == null)
            {
                this.KeysValues = new UClass.ErosValues(this.Name, ValusName);
            }
            if (thread != null && thread.IsAlive)
            {
                MessageBox.Show("线程连接执行中！");
                return;
            }

            thread = new Thread(() =>
            {
                try
                {
                    LinkO?.Invoke(AsynLink());
                    AsynRecive(Insocket);
                    this.EnabledRunCycleEvent();
                    return;
                }
                catch (Exception)
                {
                }
            });
            thread.Priority = ThreadPriority.Highest;
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 解码方式
        /// </summary>
        public Encoding GetEncoding()
        {
            try
            {
                encoding = Encoding.GetEncoding(EncodingStr);
            }
            catch (Exception)
            {
            }
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding;
        }

        protected Encoding encoding;

        [DescriptionAttribute("终端信息标志"), CategoryAttribute("发送机制"), DisplayName("终端标志"),
            TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "CR", "LF", "CRLF", "")]
        public string FTU { get; set; } = "";

        [DescriptionAttribute("解码方式"), CategoryAttribute("发送机制"), DisplayName("解码方式"),
        TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("GetEncodingNames")]
        public string EncodingStr { get; set; } = Encoding.Default.HeaderName;

        [Browsable(false)]
        public List<string> GetEncodingNames
        {
            get
            {
                return new List<string> { Encoding.Default.HeaderName, Encoding.UTF8.HeaderName ,
                    Encoding.ASCII.HeaderName , Encoding.Unicode.HeaderName ,Encoding.UTF32.HeaderName,Encoding.UTF7.HeaderName};
            }
        }

        public static List<string> GetEncodingNamese
        {
            get
            {
                return new List<string> { Encoding.Default.HeaderName, Encoding.UTF8.HeaderName ,
                    Encoding.ASCII.HeaderName , Encoding.Unicode.HeaderName ,Encoding.UTF32.HeaderName,Encoding.UTF7.HeaderName};
            }
        }

        /// <summary>
        /// 传递到TextBox文本框
        /// </summary>
        /// <param name="text"></param>
        public void AddTextBox(string text)
        {
            AddTextBox(text, System.Drawing.Color.Black);
        }

        private TextBoxBase TextBase;

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public void AddTextBox(string text, System.Drawing.Color color)
        {
            TextBase = PassiveTextBoxEvent?.Invoke(Encoding.UTF8, new byte[] { });
            if (TextBase != null)
            {
                if (TextBase.IsHandleCreated)
                {
                    MethodInvoker method = new MethodInvoker(add);
                    void add()
                    {
                        if (TextBase is RichTextBox)
                        {
                            RichTextBox ds = TextBase as RichTextBox;
                            System.Drawing.Color col = ds.SelectionColor;
                            string tstr = this.Name + ":" + text + "  " + DateTime.Now.ToString("MM/dd HH:mm:ss");
                            ds.AppendText(tstr + Environment.NewLine);
                            int strati = ds.Text.Length - tstr.Length - 1;
                            if (strati < 0)
                            {
                                strati = 0;
                            }
                            ds.Select(strati, tstr.Length);
                            ds.SelectionColor = color;
                            ds.SelectionStart = ds.Text.Length;
                            ds.SelectionColor = col;
                        }
                        else
                        {
                            if (TextBase != null)
                            {
                                TextBase.AppendText(this.Name + ":" + text + "//" + DateTime.Now.ToString("MM/dd HH:mm:ss") + Environment.NewLine);
                            }
                        }
                    }
                    TextBase?.Invoke(method);
                }
            }
        }

        /// <summary>
        /// 显示界面
        /// </summary>
        public virtual Form ShowForm()
        {
            return new NewSocketForm(this);
        }

        private Values values;

        /// <summary>
        ///
        /// </summary>
        /// <param name="messf"></param>
        public virtual void ShowForm(string messf)
        {
            Form form = ShowForm();
            form.Show();
        }

        public virtual void ShowValuesForm()
        {
            if (values == null || values.IsDisposed)
            {
                values = new Values(this);
            }
            Vision2.ErosProjcetDLL.UI.UICon.WindosFormerShow(ref values);
        }

        protected System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        /// <summary>
        /// 定时心跳
        /// </summary>
        protected void SendTimeMesag()
        {
            watchSen.Restart();
            timer.Tick += Timer_Tick;
            timer.Interval = SendTime * 1000;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsConn)
                {
                    if (!this.SendString.Contains("."))
                    {
                        byte[] buffr = Encoding.UTF8.GetBytes(this.SendString);
                        if (this.Socket() != null)
                        {
                            this.Socket().Send(buffr);
                        }
                    }
                    else
                    {
                        this.SetIDValue(this.SendString, true, out string err);
                    }
                }
                timer.Start();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 重新写入变量表
        /// </summary>
        public void SetKeysValues(DataGridView dataGridView)
        {
            keysVaWr = true;
            int ds = 0;
            while (keysVaWr)
            {
                if (ds > 1000)
                {
                    MessageBox.Show("超时");
                    return;
                }
                if (!this.IsConn || Wring)
                {
                    KeysValues = new UClass.ErosValues();
                    if (KeysValues.DictionaryValueD == null)
                    {
                        KeysValues.DictionaryValueD = new Dictionary<string, ErosValueD>();
                    }
                    KeysValues.DictionaryValueD.Clear();
                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {
                        try
                        {
                            if (dataGridView.Rows[i].Cells[0].Value == null)
                            {
                                continue;
                            }
                            string name = dataGridView.Rows[i].Cells[0].Value.ToString();
                            if (KeysValues.DictionaryValueD.ContainsKey(name))
                            {
                                if (dataGridView.Rows[i].Cells[1].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name].District = dataGridView.Rows[i].Cells[1].Value.ToString();
                                }
                                if (dataGridView.Rows[i].Cells[2].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name]._Type = dataGridView.Rows[i].Cells[2].Value.ToString();
                                }
                                if (dataGridView.Rows[i].Cells[3].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name].LinkID = dataGridView.Rows[i].Cells[3].Value.ToString();
                                }
                                if (dataGridView.Rows[i].Cells[4].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name].AddressID = dataGridView.Rows[i].Cells[4].Value.ToString();
                                }
                                if (sbyte.TryParse(dataGridView.Rows[i].Cells[5].Value.ToString(), out sbyte d))
                                {
                                    KeysValues.DictionaryValueD[name].DecimalShift = d;
                                }
                                if (dataGridView.Rows[i].Cells[6].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name].WR = dataGridView.Rows[i].Cells[6].Value.ToString();
                                }
                                if (dataGridView.Rows[i].Cells[9].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name].Default = dataGridView.Rows[i].Cells[9].Value.ToString();
                                }
                                if (dataGridView.Rows[i].Cells[11].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name].Annotation = dataGridView.Rows[i].Cells[11].Value.ToString();
                                }
                            }
                            else
                            {
                                KeysValues.DictionaryValueD.Add(name, new ErosValueD());
                                KeysValues.DictionaryValueD[name].Name = name;
                                if (dataGridView.Rows[i].Cells[1].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name].District = dataGridView.Rows[i].Cells[1].Value.ToString();
                                }
                                if (dataGridView.Rows[i].Cells[2].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name]._Type = dataGridView.Rows[i].Cells[2].Value.ToString();
                                }
                                if (dataGridView.Rows[i].Cells[3].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name].LinkID = dataGridView.Rows[i].Cells[3].Value.ToString();
                                }
                                if (dataGridView.Rows[i].Cells[4].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name].AddressID = dataGridView.Rows[i].Cells[4].Value.ToString();
                                }
                                if (dataGridView.Rows[i].Cells[5].Value != null && sbyte.TryParse(dataGridView.Rows[i].Cells[5].Value.ToString(), out sbyte d))
                                {
                                    KeysValues.DictionaryValueD[name].DecimalShift = d;
                                }
                                if (dataGridView.Rows[i].Cells[6].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name].WR = dataGridView.Rows[i].Cells[6].Value.ToString();
                                }
                                if (dataGridView.Rows[i].Cells[9].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name].Default = dataGridView.Rows[i].Cells[9].Value.ToString();
                                }
                                if (dataGridView.Rows[i].Cells[11].Value != null)
                                {
                                    KeysValues.DictionaryValueD[name].Annotation = dataGridView.Rows[i].Cells[11].Value.ToString();
                                }
                            }
                            if (dataGridView.Columns.Count >= 15)
                            {
                                if (dataGridView.Rows[i].Cells[13].Value != null && dataGridView.Rows[i].Cells[13].Value.ToString() != ""
                              && dataGridView.Rows[i].Cells[14].Value != null && dataGridView.Rows[i].Cells[14].Value.ToString() != ""
                              )
                                {
                                    KeysValues.DictionaryValueD[name].Alarmd.Triggers.Add(new AlarmText.Alarm.Trigger() { TriggerText = dataGridView.Rows[i].Cells[14].Value.ToString(), TriggerValue = 1 });
                                    KeysValues.DictionaryValueD[name].Alarmd.Enabled = true;
                                    KeysValues.DictionaryValueD[name].Alarmd.IsReset = true;
                                    KeysValues.DictionaryValueD[name].Alarmd.AlarmType = dataGridView.Rows[i].Cells[12].Value.ToString();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    MessageBox.Show("写入完成！");
                    keysVaWr = false;
                }
                ds++;
            }
        }

        /// <summary>
        /// 更新变量表
        /// </summary>
        /// <returns></returns>
        public virtual bool GetValues()
        {
            try
            {
                while (keysVaWr)
                {
                    Wring = true;
                    OnMastLink(true);
                    Thread.Sleep(10);
                }

                if (KeysValues.DictionaryValueD == null)
                {
                    Thread.Sleep(500);
                    if (SendString == "")
                    {
                        return true;
                    }
                    if (watchSen.ElapsedMilliseconds > SendTime * 1000)
                    {
                        watchSen.Restart();

                        this.Send(this.SendString);
                        if (!this.Insocket.Connected)
                        {
                            this.LinkState = "连接断开";
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    if (watchSen.ElapsedMilliseconds > SendTime * 1000)
                    {
                        watchSen.Restart();
                        if (this.SendString.Length > 2)
                        {
                            this.SetIDValue(this.SendString, true, out string err);
                            this.SetIDValue(this.SendString, 1, out  err);
                        }
                    }
                }
                this.ErrMesage = "";
                foreach (var item in KeysValues.DictionaryValueD.Values)
                {
                    while (this.SendBusy)
                    {
                    };
                    if (item.LinkID != this.Name)
                    {
                        item.LinkID = this.Name;
                    }
                    if (!this.GetValue(item))
                    {
                        //item.UpAlram(this.Name);
                        this.LinkState = "连接断开";
                        return false;
                    }
                    //   item.UpAlram(this.Name);
                }
                _DataTime = DateTime.Now;
            }
            catch (Exception EX)
            {
                ErrerLog(EX);
                watch.Reset();
                return false;
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="erosValueD"></param>
        /// <returns></returns>
        public virtual bool GetValue(ErosValueD erosValueD)
        {
            return false;
        }

        public virtual bool GetValues(string[] keys, out string[] values)
        {
            values = new string[] { };

            return false;
        }

        public virtual bool GetValues<T>(string Strkey, ushort length, out dynamic values)
        {
            values = null;
            return false;
        }

        public virtual bool GetValues(string Strkey, string typeName, ushort length, out dynamic values)
        {
            values = null;
            return false;
        }

        /// <summary>
        /// 写入多个参数值
        /// </summary>
        /// <param name="key">参数</param>
        /// <param name="value">值</param>
        /// <param name="errStr">状态</param>
        /// <returns></returns>
        public virtual bool SetValues(string[] key, string[] value, out string errStr)
        {
            errStr = "";
            string data = this.Identifying;
            char Fs = '=';
            switch (Split_Mode)
            {
                case SplitMode.Array:

                    data += String.Join(Split.ToString(), value);
                    break;

                case SplitMode.KeyValue:
                    for (int i = 0; i < key.Length; i++)
                    {
                        data += key[i] + Fs + value[i] + Split;
                    }
                    break;
            }
            return this.Send(data.TrimEnd(','));
        }

        /// <summary>
        /// 写字段值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public virtual bool SetValue(string key, string value, out string err)
        {
            err = "";
            bool done = false;
            try
            {
                this.SendBusy = true;
                if (this.KeysValues.DictionaryValueD == null)
                {
                    this.SendBusy = false;
                    return false;
                }
                if (!this.KeysValues.DictionaryValueD.ContainsKey(key))
                {
                    err += "Err:键:" + key + "不存在!";
                    this.SendBusy = false;
                    return false;
                }
                if (!UClass.GetTypeValue(this.KeysValues.DictionaryValueD[key]._Type, value, out dynamic dynamic))
                {
                    err += "Err:类型转换错误:" + this.KeysValues.DictionaryValueD[key]._Type + "：" + value;
                    this.SendBusy = false;
                    return false;
                }
                if (this.KeysValues.DictionaryValueD[key].DecimalShift != 0)
                {
                    sbyte det = Convert.ToSByte(-(this.KeysValues.DictionaryValueD[key].DecimalShift));
                    Single ds = UClass.DecimalShift(Convert.ToSingle(value), det);
                    if (int.TryParse(ds.ToString(), out int newValue))
                    {
                        dynamic = newValue;
                    }
                    else
                    {
                        err += "Err:健:" + key + "值类型错误:" + value + ";类型" + this.KeysValues.DictionaryValueD[key]._Type + "!";
                    }
                }
                if (this.IsConn)
                {
                    done = SetValue(KeysValues.DictionaryValueD[key], dynamic, out err);
                }
                else
                {
                    KeysValues.DictionaryValueD[key].Value = dynamic;
                }
                this.SendBusy = false;
            }
            catch (Exception ex)
            {
                err += ex.Message;
                this.ErrerLog(ex);
            }
            this.SendBusy = false;
            return done;
        }

        /// <summary>
        /// 对指定地址写入值
        /// </summary>
        /// <param name="id">地址</param>
        /// <param name="value">值</param>
        /// <param name="err">信息</param>
        /// <returns>写入状态</returns>
        public virtual bool SetIDValue(string id, dynamic value, out string err)
        {
            err = "未实现地址写入方法";

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public virtual bool SetIDValue(string id, string type, string value, out string err)
        {
            if (UClass.GetTypeValue(type, value, out dynamic values))
            {
                return SetIDValue(id, values, out err);
            }
            err = "类型与值不服！";
            return false;
        }

        /// <summary>
        /// 根据ID读取值
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool GetIDValue(string ID, string type, out dynamic value)
        {
            value = null;
            return false;
        }

        /// <summary>
        /// 根据ID读取值
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool GetIDValue(string ID, out string value)
        {
            value = "";

            return false;
        }

        public virtual bool GetIDValue(byte linkID, string ID, out string value)
        {
            value = "";

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="typeStr"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual string GetTypeLengthAddress(string typeStr, string address)
        {
            return address;
        }

        /// <summary>
        /// 返回类型的地址长度
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public virtual byte GetTypeLength(string typeStr)
        {
            return 1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool SetValue(ErosValueD item, dynamic value, out string errStr)
        {
            errStr = "";
            this.SendBusy = false;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ex"></param>
        public void ErrerLog(Exception ex)
        {
            if (LogNet == null)
            {
                LogNet = new HslCommunication.LogNet.LogNetSingle(Application.StartupPath + "\\NetLog\\" + this.Name + ".txt");
            }

            if (ProjectINI.DebugMode)
            {
                ErrForm.Show(ex);
                //MessageBox.Show(this.Name + "异常信息:" + ex.Message);
            }
            else
            {
                if (IsStataText)
                {
                    this.AddTextBox(this.Name + "异常信息:" + ex);
                }
            }
            LogNet.WriteException("", ex);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mess"></param>
        public void ErrerLog(string mess)
        {
            if (LogNet == null)
            {
                LogNet = new HslCommunication.LogNet.LogNetSingle(Application.StartupPath + "\\NetLog\\" + Name + ".txt");
            }
            if (IsStataText)
            {
                this.AddTextBox("异常信息:" + mess);
            }
            LogNet.WriteError(mess);
        }

        /// <summary>
        /// 读取到关键字DB到Json
        /// </summary>
        /// <returns></returns>
        public virtual Newtonsoft.Json.Linq.JObject GetValuesData()
        {
            Newtonsoft.Json.Linq.JObject obJson = new Newtonsoft.Json.Linq.JObject();
            List<ErosValueD> listd = new List<ErosValueD>();
            foreach (var item in this.KeysValues.DictionaryValueD)
            {
                if (item.Value.Annotation != null && item.Value.Annotation.Contains("DB"))
                {
                    listd.Add(item.Value);
                }
            }
            foreach (ErosValueD item in listd)
            {
                if (item.Value != null)
                {
                    obJson.Add(item.Name, item.Value.ToString());
                }
                else
                {
                    obJson.Add(item.Name, "");
                }
            }
            return obJson;
        }

        /// <summary>
        /// 更新读取Json内ObjectDatas字段对象，为空时遍历字段表,
        /// </summary>
        /// <param name="this.KeysValues"></param>
        /// <param name="obJson"></param>
        /// <param name="key"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public virtual bool SendDataGetValues(ref Newtonsoft.Json.Linq.JObject obJson, out string err)
        {
            err = "";
            try
            {
                if (obJson == null)
                {
                    obJson = new Newtonsoft.Json.Linq.JObject();
                }
                if (obJson.Count == 0)
                {
                    foreach (var item in this.KeysValues.DictionaryValueD)
                    {
                        if (item.Value.Value != null)
                        {
                            obJson.Add(item.Key, item.Value.Value.ToString());
                        }
                        else
                        {
                            obJson.Add(item.Key, "");
                        }
                    }
                }//读取全部变量表
                else
                {
                    foreach (var item in obJson)
                    {
                        if (this.KeysValues.DictionaryValueD.ContainsKey(item.Key))
                        {
                            obJson[item.Key] = this.KeysValues.DictionaryValueD[item.Key].Value.ToString();
                        }
                        else
                        {
                            err += "Err:键" + item.Key + "不存在；";
                        }
                    }
                }//遍历Json对象更新Values
                if (!obJson.ContainsKey("DataTime"))
                {
                    obJson.Add("DataTime", _DataTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));//添加更新时间
                }
                return true;
            }
            catch (Exception EX)
            {
                ErrerLog(EX);
                err = "Err:代码错误" + EX.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// 设置Json对象类的值
        /// </summary>
        /// <param name="obJson"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public virtual bool SendDataSetValues(Newtonsoft.Json.Linq.JObject obJson, SByte repetition, out string err)
        {
            err = "";
            try
            {
                this.SendBusy = true;
                Thread.Sleep(500);
                foreach (var item in obJson)
                {
                    if (!this.KeysValues.DictionaryValueD.ContainsKey(item.Key))
                    {
                        err += "Err:字段<" + item.Key + ">不存在!";
                    }
                    else
                    {
                        sbyte i = 0;
                        while (!SetValue(item.Key, item.Value.ToString(), out string err2))
                        {
                            err += err2;
                            if (i >= repetition)
                            {
                                break;
                            }
                            i++;
                        }
                    }
                }
                this.SendBusy = false;
                return true;
            }
            catch (Exception ex)
            {
                ErrerLog(ex);
            }
            this.SendBusy = false;
            return false;
        }

        public virtual bool SendDataSetValues(Newtonsoft.Json.Linq.JObject obJson, out string err)
        {
            return SendDataSetValues(obJson, 0, out err);
        }

        /// <summary>
        /// 设置时间逻辑参数到连接
        /// </summary>
        /// <param name="obJson"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public virtual string SendDataSetTimeValues(Newtonsoft.Json.Linq.JObject obJson, out string err)
        {
            err = "";
            try
            {
                if (TimeTaskState != "结束" && TimeTaskState != "未激活")
                {
                    err = "任务未结束";
                    return "";
                }
                ListJobject = new Newtonsoft.Json.Linq.JObject[obJson.Count];
                foreach (var item in obJson)
                {
                    Newtonsoft.Json.Linq.JObject jo = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(item.Value.ToString());    //字段对象
                    if (uint.TryParse(item.Key.ToString(), out uint id))
                    {
                        ListJobject[id - 1] = new Newtonsoft.Json.Linq.JObject();
                        ListJobject[id - 1] = jo;
                    }
                    else
                    {
                        err += "Err:ID" + item.Key.ToString() + "无法转换为无符号整数";
                    }
                    string ds = "";

                    foreach (var items in jo)
                    {
                        ds += items.Key;
                    }

                    string[] times = jo["Time"].ToString().Split(':');
                    if (times.Length == 2)
                    {
                        if (!uint.TryParse(times[0], out timeH))
                        {
                            err += "Err:时间格式H无法转换为Uint";
                        }
                        if (!uint.TryParse(times[1], out timeM))
                        {
                            err += "Err:时间格式M无法转换为Uint";
                        }
                    }
                    else if (times.Length == 1)
                    {
                        if (!uint.TryParse(times[0], out timeH))
                        {
                            err += "Err:时间格式M无法转换为Uint";
                        }
                    }
                    Newtonsoft.Json.Linq.JObject jovalues = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(jo["objValues"].ToString());
                }
            }
            catch (Exception ex)
            {
                err += ex.Message;
                ErrerLog(ex);
            }
            return "";
        }

        /// <summary>
        /// 读取时间逻辑参数
        /// </summary>
        /// <param name="obJson"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public virtual string SendDataGetTimeValues(Newtonsoft.Json.Linq.JObject obJson, out string err)
        {
            err = "";
            try
            {
                Newtonsoft.Json.Linq.JObject jo = new Newtonsoft.Json.Linq.JObject();
                if (ListJobject != null)
                {
                    for (int i = 0; i < ListJobject.Length; i++)
                    {
                        jo.Add((i + 1).ToString(), ListJobject[i]);
                    }
                }
                if (!obJson.ContainsKey("ObjectDatas"))
                {
                    obJson.Add("ObjectDatas", jo);
                }
                else
                {
                    obJson["ObjectDatas"] = jo;
                }
            }
            catch (Exception ex)
            {
                err += ex.Message;
                ErrerLog(ex);
            }
            return "";
        }

        /// <summary>
        /// 设置时间逻辑状态
        /// </summary>
        /// <param name="cont"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public virtual string SetTimeTask(string cont, out string err)
        {
            err = "";
            try
            {
                switch (cont)
                {
                    case "执行":
                        TimeTaskState = "执行中";
                        SetTimeData();
                        break;

                    case "停止":
                        TimeTaskState = "停止";
                        break;

                    case "暂停":
                        TimeTaskState = "暂停";
                        break;

                    case "重新执行":
                        TimeTaskState = "重新执行";
                        break;

                    case "跳步":
                        TimeTaskState = "跳步";
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrerLog(ex);
            }
            return "";
        }

        /// <summary>
        /// 读取任务状态
        /// </summary>
        /// <returns></returns>
        public virtual Newtonsoft.Json.Linq.JObject GetTimeTask()
        {
            Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();
            try
            {
                jObject.Add("TimeCD", TimeCDH + ":" + TimeCDM + ":" + TimeCDSS);

                int timeM = 0; int timeH = 0;
                for (int i = 0; i < ListJobject.Length; i++)
                {
                    string[] times = ListJobject[i]["Time"].ToString().Split(':');
                    int TH = 0; int TM = 0;
                    if (times.Length == 2)
                    {
                        int.TryParse(times[0], out TH);
                        int.TryParse(times[1], out TM);
                    }
                    else if (times.Length == 1)
                    {
                        int.TryParse(times[0], out TM);
                    }
                    timeM = timeM + TM;
                    if (timeM >= 60)
                    {
                        int d = timeM / 60;
                        timeH = timeH + d;
                        timeM = timeM % 60;
                    }
                    timeH = timeH + TH;
                }
                jObject.Add("Time", timeH + ":" + timeM);
                jObject.Add("CDID", CDID);
                jObject.Add("CD", CD);
                jObject.Add("TaskState", TimeTaskState);
                jObject.Add("FacillttState", FacillttState);
            }
            catch (Exception)
            {
            }
            return jObject;
        }

        /// <summary>
        /// 执行逻辑任务
        /// </summary>
        public virtual string SetTimeData()
        {
            try
            {
                if (TimeTaskState == "结束" || Taskthread == null || !Taskthread.IsAlive)
                {
                    Taskthread = new Thread(() =>
                    {
                        TimeTaskState = "执行中";
                        try
                        {
                            CD = ListJobject.Length;
                            for (int i = 0; i < ListJobject.Length; i++)
                            {
                                CDID = i + 1;
                                TimeCDSS = 0;
                                if (ListJobject[i] == null)
                                {
                                    continue;
                                }
                                Newtonsoft.Json.Linq.JObject jo = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(ListJobject[i]["objValues"].ToString());    //字段对象
                                SendDataSetValues(jo, 2, out string errs);
                                string[] times = ListJobject[i]["Time"].ToString().Split(':');
                                if (times.Length == 2)
                                {
                                    uint.TryParse(times[0], out timeH);
                                    uint.TryParse(times[1], out timeM);
                                }
                                else if (times.Length == 1)
                                {
                                    TimeH = 0;
                                    uint.TryParse(times[0], out timeM);
                                }
                                TimeCDM = TimeM;
                                TimeCDH = TimeH;
                                while (TimeCDM != 0 || TimeCDH != 0 || TimeCDSS != 0)
                                {
                                    if (TimeCDM == 0 && TimeCDH != 0)
                                    {
                                        TimeCDH--;
                                        TimeCDM = 59;
                                        TimeCDSS = 59;
                                    }
                                    if (TimeCDSS == 0 && TimeCDM != 0)
                                    {
                                        TimeCDM--;
                                        TimeCDSS = 59;
                                    }
                                    Thread.Sleep(1000);
                                    TimeCDSS--;
                                    if (TimeTaskState == "停止")
                                    {
                                        goto end;
                                    }
                                    while (FacillttState == "故障中" || TimeTaskState == "暂停")
                                    {
                                        //TaskState = "暂停";
                                    }
                                    if (FacillttState != "故障中" && TimeTaskState != "停止")
                                    {
                                        TimeTaskState = "执行中";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrerLog(ex);
                        }

                    end:
                        TimeTaskState = "结束";
                    });
                    Taskthread.IsBackground = true;
                    Taskthread.Start();
                }
            }
            catch (Exception ex)
            {
                ErrerLog(ex);
            }
            return "";
        }

        /// <summary>
        /// 周期判断连接设备状态
        /// </summary>
        /// <returns></returns>
        public virtual string CycFacillttState()
        {
            try
            {
            }
            catch (Exception)
            {
            }
            return "";
        }

        #endregion 方法

        #region 静态方法

        /// <summary>
        /// 排序变量表
        /// </summary>
        /// <param name="erosValues"></param>
        /// <returns></returns>
        public static ErosValueD[] GetAddres(UClass.ErosValues erosValues, string startsW, out int lengtm, string typeName = null)
        {
            lengtm = 0;
            try
            {
                List<ErosValueD> itemt = new List<ErosValueD>();
                if (typeName != null)
                {
                    var dst = from n in erosValues.DictionaryValueD.Values
                              where n.AddressID.StartsWith(startsW)
                              where n._Type == typeName
                              orderby Convert.ToDouble(n.AddressID.Remove(0, startsW.Length)) ascending
                              select n;
                    lengtm = dst.Count();

                    foreach (var item in dst)
                    {
                        itemt.Add(item);
                    }
                }
                else
                {
                    var dst = from n in erosValues.DictionaryValueD.Values
                              where n.AddressID.StartsWith(startsW)
                              orderby Convert.ToDouble(n.AddressID.Remove(0, startsW.Length)) ascending
                              select n;
                    lengtm = dst.Count();
                    foreach (var item in dst)
                    {
                        itemt.Add(item);
                    }
                }

                //Array.Sort(erosValueDs, new ErosValueD(startsW));

                return itemt.ToArray(); ;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        [Category("协议"), DisplayName("子协议格式"), Description("子协议格式"),
        TypeConverter(typeof(ErosConverter)),
         ErosConverter.ThisDropDown("", false, "MC", "1E", "")]
        public string linkType { get; set; } = "MC";

        /// <summary>
        /// Close the socket safely.
        /// </summary>
        /// <param name="socket">The socket.</param>
        public static void SafeClose(Socket socket)
        {
            if (socket == null)
                return;

            if (!socket.Connected)
                return;

            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {
            }

            try
            {
                socket.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 根据字符串创建链接类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SocketClint NewTypeLink(string type)
        {
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 

            dynamic obj = null;
            GetListClassName();
            if (type.Contains('.'))
            {
                obj = assembly.CreateInstance(type);
            }
            else
            {
                Type typeS = null;

                for (int i = 0; i < ListClass.Count; i++)
                {
                    if (ListClass[i].Name == type)
                    {
                        typeS = ListClass[i];
                        break;
                    }
                }
                if (typeS != null)
                {
                    obj = assembly.CreateInstance(typeS.ToString());
                }
                else
                {
                    obj = assembly.CreateInstance("ErosSocket.ErosConLink." + type);
                }
            }
            if (obj == null)
            {
                MessageBox.Show(type + ":类型不存在");
            }
            return obj;
        }

        /// <summary>
        /// 判断IP地址
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsValidateIPAddress(string ipAddress)
        {
            Regex validipregex = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
            return (ipAddress != "" && validipregex.IsMatch(ipAddress.Trim())) ? true : false;
        }

        /// <summary>
        /// ping网络
        /// </summary>
        /// <param name="ipProtStr"></param>
        /// <param name="read"></param>
        /// <returns></returns>
        public static bool IsPingIP(string ipProtStr, out string read)
        {
            read = string.Empty;
            string[] strs = ipProtStr.Split(',');
            string IP = string.Empty;
            string Prot = string.Empty;
            if (strs.Length == 2)
            {
                Prot = strs[1];
            }
            IP = strs[0];
            if (SocketClint.IsValidateIPAddress(IP))
            {
                Ping pingSender = new Ping();
                //Ping 选项设置
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                //测试数据
                string data = "test data abcabc";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                //设置超时时间
                int timeout = 120;
                //调用同步 send 方法发送数据,将返回结果保存至PingReply实例
                PingReply reply = pingSender.Send(IP, timeout);
                if (reply.Status == IPStatus.Success)
                {
                    read += "答复的主机地址：" + reply.Address.ToString() + Environment.NewLine;
                    read += "往返时间ms：" + reply.RoundtripTime + Environment.NewLine;
                    read += "生存时间（TTL）：" + reply.Options.Ttl + Environment.NewLine;
                    read += "是否控制数据包的分段：" + reply.Options.DontFragment + Environment.NewLine;
                    read += "缓冲区大小：" + reply.Buffer.Length + Environment.NewLine;

                    try
                    {
                        //IPHostEntry iPHostEntry = Dns.GetHostByAddress(IP);

                        //read += "主机名称：" + Dns.GetHostByAddress(IP).HostName.ToString() + Environment.NewLine;
                    }
                    catch (Exception)
                    {
                    }
                    return true;
                }
                else
                {
                    read += "链接失败：" + reply.Status.ToString() + Environment.NewLine;

                    return false;
                }
            }
            read += "IP地址不正确:" + ipProtStr;
            return false;
        }

        public static IPAddress LocalIP = GetLocalIP();

        /// <summary>
        /// 获取本地驱动资料
        /// </summary>
        /// <param name="treeV">网络驱动</param>
        /// <returns>返回驱动</returns>
        public static string GetGateway(TreeView treeV, TabControl tabControl)
        {
            string strGateway = "";
            //tabControl.TabPages.Clear();
            //TreeNode treeNodeP = treeV.Nodes.Add("网络驱动");
            //获取所有网卡
            NetworkInterface[] interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            int len = interfaces.Length;
            //treeNodeP.Text = "网络驱动总数" + len;
            strGateway = "驱动总数：" + len + Environment.NewLine;
            List<ManagementObject> ListIP = GetIPAddress();
            List<string> listStr = SetIPAddress("2", "2", "1", "ds");
            string dsts = "";
            for (int i = 0; i < listStr.Count; i++)
            {
                dsts += listStr[i].ToString() + "+++++++++++++++++" + Environment.NewLine;
            }
            for (int i = 0; i < len; i++)
            {
                NetworkInterface ni = interfaces[i];
                IPInterfaceProperties property = ni.GetIPProperties();
                string dss = ni.GetPhysicalAddress().ToString();
                if (ni.GetPhysicalAddress().ToString() == "")
                {
                    continue;
                }

                ///t添加TabPage
                int d = tabControl.TabPages.IndexOfKey(ni.Id);
                TabPage tabPage = new TabPage();
                if (d < 0)
                {
                    tabPage.Name = ni.Id;
                    tabPage.Text = ni.Name;
                    tabControl.TabPages.Add(tabPage);
                }
                else
                {
                    tabPage = tabControl.TabPages[d];
                    tabPage.Controls.Clear();
                }
                //
                CheckBox checkBoxIP = new CheckBox() { Text = "自动获取IP:", Top = 3, AutoSize = true, };

                for (int i2 = 0; i2 < ListIP.Count; i2++)
                {
                    string ds = ListIP[i2]["SettingID"].ToString();
                    if (ListIP[i2]["SettingID"].ToString() == ni.Id)
                    {
                        checkBoxIP.Checked = Convert.ToBoolean(ListIP[i2]["DHCPEnabled"]);
                        break;
                    }
                }
                //IP
                Label labeip = new Label() { Text = "IP:", Top = checkBoxIP.Top + checkBoxIP.Height, Width = 18, AutoSize = true, };
                TextBox textBoxIP = new TextBox() { Name = "txtIP", Top = labeip.Top, Left = 60 };
                //掩码
                Label labeMack = new Label() { Text = "Mack:", Top = labeip.Top + labeip.Height, AutoSize = true, };
                TextBox textBoxMack = new TextBox() { Name = "txtMack", Top = labeMack.Top + 2, Left = 60 };
                foreach (UnicastIPAddressInformation ip in property.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet && ni.Name == "以太网")
                        {
                        }
                        textBoxIP.Text = ip.Address.ToString();
                        textBoxMack.Text = ip.IPv4Mask.ToString();
                        break;
                    }
                }

                ///默认网关
                Label labegateWay = new Label() { Text = "默认网关:", Top = textBoxMack.Top + textBoxMack.Height, AutoSize = true, };
                TextBox textBoxGateWay = new TextBox() { Name = "txtGateWay", Top = labegateWay.Top + 2, Left = 60 };

                if (property.GatewayAddresses.Count > 0)
                {
                    textBoxGateWay.Text = property.GatewayAddresses[0].Address.ToString();//默认网关
                }
                ///事件
                if (checkBoxIP.Checked)
                {
                    textBoxGateWay.Enabled = textBoxMack.Enabled = textBoxIP.Enabled = false;
                }
                else
                {
                    textBoxGateWay.Enabled = textBoxMack.Enabled = textBoxIP.Enabled = true;
                }
                checkBoxIP.CheckedChanged += CheckBoxIP_CheckedChanged;
                void CheckBoxIP_CheckedChanged(object sender, EventArgs e)
                {
                    if (checkBoxIP.Checked)
                    {
                        textBoxGateWay.Enabled = textBoxMack.Enabled = textBoxIP.Enabled = false;
                    }
                    else
                    {
                        textBoxGateWay.Enabled = textBoxMack.Enabled = textBoxIP.Enabled = true;
                    }
                }

                CheckBox checkBoxDNS = new CheckBox() { Text = "自动获取DNS:", Top = textBoxGateWay.Top + textBoxGateWay.Height + 3, AutoSize = true, };

                ///DNS1
                Label labedns1 = new Label() { Text = "DNS1:", Top = checkBoxDNS.Top + checkBoxDNS.Height, AutoSize = true, };
                TextBox textBoxDNS1 = new TextBox() { Name = "txtDNS1", Top = labedns1.Top + 2, Left = 60 };
                ///DNS2
                Label labedns2 = new Label() { Text = "DNS2:", Top = textBoxDNS1.Top + textBoxDNS1.Height, AutoSize = true, };
                TextBox textBoxDNS2 = new TextBox() { Name = "txtDNS2", Top = labedns2.Top + 2, Left = 60 };
                if (property.DnsAddresses.Count == 1)
                {
                    textBoxDNS1.Text = property.DnsAddresses[0].ToString(); //主DNS
                }
                else if (property.DnsAddresses.Count == 2)
                {
                    textBoxDNS1.Text = property.DnsAddresses[0].ToString(); //主DNS
                    textBoxDNS2.Text = property.DnsAddresses[1].ToString(); //主DNS
                }
                checkBoxDNS.CheckedChanged += CheckBoxDNS_CheckedChanged;
                void CheckBoxDNS_CheckedChanged(object sender, EventArgs e)
                {
                    if (checkBoxDNS.Checked)
                    {
                        textBoxDNS2.Enabled = textBoxDNS1.Enabled = false;
                    }
                    else
                    {
                        textBoxDNS2.Enabled = textBoxDNS1.Enabled = true;
                    }
                }
                checkBoxDNS.Checked = property.IsDynamicDnsEnabled;
                Label labeID = new Label() { Text = "ID:" + ni.Id, Top = textBoxDNS2.Top + textBoxDNS2.Height, AutoSize = true, };
                Label labeMAC = new Label() { Text = "MAC:" + ni.GetPhysicalAddress(), Top = labeID.Top + labeID.Height, AutoSize = true, };
                Label labeLink = new Label() { Text = "连接状态:" + ni.OperationalStatus, Top = labeMAC.Top + labeMAC.Height, AutoSize = true, };
                Label labeSpeed = new Label() { Text = "连接速度:" + ni.Speed, Top = labeLink.Top + labeLink.Height, AutoSize = true, };
                Label labeDescription = new Label() { Text = "接口说明:" + ni.Description, Top = labeSpeed.Top + labeSpeed.Height, AutoSize = true, };
                Button buttonSet = new Button() { Text = "设置", Top = labeDescription.Top + labeDescription.Height, };
                buttonSet.Click += ButtonSet_Click;
                void ButtonSet_Click(object sender, EventArgs e)
                {
                    ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
                    ManagementObjectCollection moc = wmi.GetInstances();
                    ManagementBaseObject inPar = null;
                    ManagementBaseObject outPar = null;
                    foreach (ManagementObject item in moc)
                    {
                        string ds = item["SettingID"].ToString();

                        if (item["SettingID"].ToString() == ni.Id)
                        {
                            if (checkBoxIP.Checked)
                            {
                                //重置DNS为空
                                ////开启DHCP
                                item.InvokeMethod("SetDNSServerSearchOrder", null);
                                item.InvokeMethod("EnableStatic", null);
                                item.InvokeMethod("SetGateways", null);
                                item.InvokeMethod("EnableDHCP", null);
                            }
                            else
                            {
                                //设置IP地址和掩码
                                if (IsValidateIPAddress(textBoxIP.Text) && IsValidateIPAddress(textBoxMack.Text))
                                {
                                    inPar = item.GetMethodParameters("EnableStatic");
                                    inPar["IPAddress"] = new string[] { textBoxIP.Text };
                                    inPar["SubnetMask"] = new string[] { textBoxMack.Text };
                                    outPar = item.InvokeMethod("EnableStatic", inPar, null);
                                }
                                //设置网关地址
                                if (IsValidateIPAddress(textBoxGateWay.Text))
                                {
                                    inPar = item.GetMethodParameters("SetGateways");
                                    inPar["DefaultIPGateway"] = new string[] { textBoxGateWay.Text };
                                    outPar = item.InvokeMethod("SetGateways", inPar, null);
                                }
                                //设置DNS地址
                                if (IsValidateIPAddress(textBoxDNS1.Text))
                                {
                                    inPar = item.GetMethodParameters("SetDNSServerSearchOrder");
                                    inPar["DNSServerSearchOrder"] = new string[] { textBoxDNS1.Text, textBoxDNS2.Text };
                                    outPar = item.InvokeMethod("SetDNSServerSearchOrder", inPar, null);
                                }
                            }
                        }
                    }
                }
                tabPage.Controls.Add(buttonSet);
                tabPage.Controls.Add(labeLink);
                tabPage.Controls.Add(labeSpeed);
                tabPage.Controls.Add(labeDescription);
                tabPage.Controls.Add(checkBoxIP);
                tabPage.Controls.Add(checkBoxDNS);
                tabPage.Controls.Add(labeip);
                tabPage.Controls.Add(labeID);
                tabPage.Controls.Add(textBoxIP);
                tabPage.Controls.Add(labeMAC);
                tabPage.Controls.Add(labeMack);
                tabPage.Controls.Add(textBoxMack);
                tabPage.Controls.Add(labegateWay);
                tabPage.Controls.Add(textBoxGateWay);

                tabPage.Controls.Add(labedns1);
                tabPage.Controls.Add(textBoxDNS1);

                tabPage.Controls.Add(labedns2);
                tabPage.Controls.Add(textBoxDNS2);
            }
            return strGateway;
        }

        private static bool threadOFF;

        /// <summary>
        /// 获取本地IP
        /// </summary>
        public static IPAddress GetLocalIP()
        {
            //this.Text = "TCPServer》》计算机名称：" + Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var IPadd in ipEntry.AddressList)
            {
                //判断当前字符串是否为正确IP地址
                //得到本地IP地址
                if (IsValidateIPAddress(IPadd.ToString()))
                {
                    return LocalIP = IPadd;
                    //得到本地IP地址
                }
            }
            return new IPAddress(0);
        }

        /// <summary>
        /// 获得本地局域网所有IP和Mac、PC名称
        /// </summary>
        /// <param name="treeV"></param>
        /// <returns></returns>
        [Obsolete]
        public static string GetAllLocalMachines(TreeView treeV)
        {
            string IPHead = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString().Substring(0, 9);
            //LocalIP = IPHead;
            EnumComputers(IPHead, new List<string>());
            //在cmd.exe下面ping一下几台机，然后用arp - a命令查看一下，这种方式比开多线程去循环扫描的方式来的简单而有效。
            //首先来个循环ping一下那个网段的主机。
            //其次用以下的函数去获取所有的局域网内有响应的ip地址列表
            threadOFF = false;
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine("arp -a");
            p.StandardInput.WriteLine("exit");
            StreamReader reader = p.StandardOutput;
            List<string> listS = new List<string>();
            int length = 0;
            int I = 0;
            //string IPHead = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString().Substring(0, 3);
            TreeNode treeNodeP = treeV.Nodes.Add("局域网");
            treeNodeP.Text = "局域网段" + IPHead + ".1-255,总数" + length + "/ 0";
            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                listS.Add(line);
                if (line.TrimStart(' ').StartsWith(IPHead))
                {
                    length++;
                    string IP = line.Substring(0, 15).Trim();
                    string Mac = line.Substring(line.IndexOf("-") - 2, 0x11).Trim();

                    TreeNode treeNode = treeNodeP.Nodes.Add(IP);

                    treeNode.Text = "IP:" + IP + ";MAC:" + Mac + ";PC名称：";
                    treeNode.Name = IP;
                }
            }
            reader.Close();
            treeNodeP.Text = "局域网" + IPHead + ".1-255,总数" + length + " / 0";
            treeNodeP.Name = "局域网";
            Thread thread = new Thread(() =>
            {
                Thread.Sleep(5000);
                int i = 0;
                threadOFF = true;
                while (listS.Count > i && threadOFF)
                {
                    string line = listS[i].Trim();
                    if (line.StartsWith(IPHead))
                    {
                        I++;
                        string IP = line.Substring(0, 15).Trim();
                        string Mac = line.Substring(line.IndexOf("-") - 2, 0x11).Trim();
                        string name = "无名称";
                        try
                        {
                            TreeNode[] treeNodePt = treeV.Nodes.Find("局域网", true);
                            treeNodePt[0].Text = "局域网" + IPHead + ".1-255,总数" + length + "/ " + I;
                            name = Dns.GetHostByAddress(IP).HostName.ToString();
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            MethodInvoker methodInvoker = new MethodInvoker(add);
                            treeV.Invoke(methodInvoker);
                            void add()
                            {
                                TreeNode[] treeNode = treeV.Nodes.Find(IP, true);
                                if (treeNode.Length == 1)
                                {
                                    treeNode[0].Text = "IP:" + IP + ";MAC:" + Mac + ";PC名称：" + name;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    i++;
                }
            });
            thread.IsBackground = true;
            thread.Start();
            string str = "";
            for (int i = 0; i < listS.Count; i++)
            {
                str += listS[i].ToString() + Environment.NewLine;
            }

            return str;
        }

        public static void GetAllLocalMachines()
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine("arp -a");
            p.StandardInput.WriteLine("exit");
            StreamReader reader = p.StandardOutput;
            List<string> listS = new List<string>();
            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                listS.Add(line);
            }

            reader.Close();
        }

        /// <summary>
        /// 循环异步ping网络段，不带子地址
        /// </summary>
        /// <param name="ipStr">IP地址</param>
        /// <param name="pingListIP">IP更新到TreeV</param>
        /// <param name="textBox"></param>
        public static void EnumComputers(string ipStr, List<string> pingListIP, RichTextBox textBox)
        {
            try
            {
                string[] ipstr = ipStr.Trim('.', ' ').Split('.');
                if (ipstr.Length < 3)
                {
                    MessageBox.Show("IP地址错误");
                    return;
                }
                string ipstrt = string.Empty;
                for (int i = 0; i < ipstr.Length; i++)
                {
                    if (i >= 3)
                    {
                        break;
                    }
                    if (uint.TryParse(ipstr[i], out uint intIP))
                    {
                        if (intIP < 255)
                        {
                            ipstrt += intIP.ToString() + '.';
                        }
                        else
                        {
                            MessageBox.Show("IP地址错误");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("IP地址错误");
                        return;
                    }
                }
                if (pingListIP == null)
                {
                    pingListIP = new List<string>();
                }
                else pingListIP.Clear();
                textBox.AppendText("Ping网络开始:" + ipstrt + "0-255" + Environment.NewLine);

                for (int i = 1; i <= 255; i++)
                {
                    Ping myPing;
                    myPing = new Ping();
                    myPing.PingCompleted += new PingCompletedEventHandler(_myPing_PingCompleted);
                    string pingIP = ipstrt + i.ToString();
                    myPing.SendAsync(pingIP, 2000, null);
                }
                void _myPing_PingCompleted(object sender, PingCompletedEventArgs e)
                {
                    try
                    {
                        if (e.Reply.Status == IPStatus.Success)
                        {
                            pingListIP.Add(e.Reply.Address.ToString());
                            MethodInvoker methodInvoker = new MethodInvoker(Con);
                            textBox.Invoke(methodInvoker);
                        }
                        else
                        {
                            MethodInvoker methodInvoker = new MethodInvoker(add);
                            textBox.Invoke(methodInvoker);
                        }
                        void Con()
                        {
                            textBox.AppendText(e.Reply.Address.ToString() + ";" + e.Reply.RoundtripTime + "ms;Ping成功！" + Environment.NewLine);
                            textBox.ScrollToCaret();
                        }
                        void add()
                        {
                            if (e.Reply.Address.ToString().EndsWith("255"))
                            {
                                textBox.AppendText(e.Reply.Address.ToString() + "Ping失败！" + Environment.NewLine);
                            }

                            //textBox.ScrollToCaret();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///  循环异步ping网络段，不带子地址
        /// </summary>
        public static void EnumComputers(string ipStr = null)
        {
            try
            {
                GetAllLocalMachines();

                Task.Run(() =>
                {
                    if (ipStr == null)
                    {
                        ipStr = LocalIP.ToString();
                    }
                    string[] ipstr = ipStr.Trim('.', ' ').Split('.');
                    if (ipstr.Length < 3)
                    {
                        MessageBox.Show("IP地址错误");
                        return;
                    }
                    string ipstrt = string.Empty;
                    for (int i = 0; i < ipstr.Length; i++)
                    {
                        if (i >= 3)
                        {
                            break;
                        }
                        if (uint.TryParse(ipstr[i], out uint intIP))
                        {
                            if (intIP < 255)
                            {
                                ipstrt += intIP.ToString() + '.';
                            }
                            else
                            {
                                MessageBox.Show("IP地址错误");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("IP地址错误");
                            return;
                        }
                    }
                    for (int i = 1; i <= 255; i++)
                    {
                        Ping myPing;
                        myPing = new Ping();
                        string pingIP = ipstrt + i.ToString();
                        myPing.SendAsync(pingIP, 2000, null);
                    }
                    Thread.Sleep(2000);
                    //GetAllLocalMachines();
                });
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 循环异步ping网络段，不带子地址
        /// </summary>
        /// <param name="ipStr">IP地址</param>
        /// <param name="pingListIP">IP更新到TreeV</param>
        public static void EnumComputers(string ipStr, List<string> pingListIP)
        {
            try
            {
                string[] ipstr = ipStr.Trim('.', ' ').Split('.');
                if (ipstr.Length < 3)
                {
                    //MessageBox.Show("IP地址错误");
                    return;
                }
                string ipstrt = string.Empty;

                for (int i = 0; i < ipstr.Length; i++)
                {
                    if (i >= 3)
                    {
                        break;
                    }
                    if (uint.TryParse(ipstr[i], out uint intIP))
                    {
                        if (intIP < 255)
                        {
                            ipstrt += intIP.ToString() + '.';
                        }
                        else
                        {
                            MessageBox.Show("IP地址错误");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("IP地址错误");
                        return;
                    }
                }

                if (pingListIP == null)
                {
                    pingListIP = new List<string>();
                }
                else pingListIP.Clear();
                for (int i = 1; i <= 255; i++)
                {
                    Ping myPing;
                    myPing = new Ping();
                    myPing.PingCompleted += new PingCompletedEventHandler(_myPing_PingCompleted);
                    string pingIP = ipstrt + i.ToString();
                    myPing.SendAsync(pingIP, 1000, null);
                }
                void _myPing_PingCompleted(object sender, PingCompletedEventArgs e)
                {
                    try
                    {
                        if (e.Reply.Status == IPStatus.Success)
                        {
                            pingListIP.Add(e.Reply.Address.ToString());
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 设置网关
        /// </summary>
        /// <param name="getway"></param>
        public static List<string> SetGetWay(string id, string getway)
        {
            return SetIPAddress(id, null, null, new string[] { getway }, null);
        }

        /// <summary>
        /// 设置网关
        /// </summary>
        /// <param name="getway"></param>
        public static List<string> SetGetWay(string id, string[] getway)
        {
            return SetIPAddress(id, null, null, getway, null);
        }

        /// <summary>
        /// 设置IP地址和掩码
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="submask"></param>
        public static List<string> SetIPAddress(string id, string ip, string submask)
        {
            return SetIPAddress(id, new string[] { ip }, new string[] { submask }, null, null);
        }

        /// <summary>
        /// 设置IP地址，掩码和网关
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="submask"></param>
        /// <param name="getway"></param>
        public static List<string> SetIPAddress(string id, string ip, string submask, string getway)
        {
            return SetIPAddress(id, new string[] { ip }, new string[] { submask }, new string[] { getway }, null);
        }

        /// <summary>
        /// 设置IP地址，掩码，网关和DNS
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="submask"></param>
        /// <param name="getway"></param>
        /// <param name="dns"></param>
        public static List<string> SetIPAddress(string id, string[] ip, string[] submask, string[] getway, string[] dns)
        {
            List<string> listStr = new List<string>();
            try
            {
                ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = wmi.GetInstances();
                ManagementBaseObject inPar = null;
                ManagementBaseObject outPar = null;
                foreach (ManagementObject mo in moc)
                {
                    string das = "";

                    foreach (var item in mo.Properties)
                    {
                        if (item.Value != null)
                        {
                            try
                            {
                                if (item.Value is string[])
                                {
                                    string[] strs = item.Value as string[];
                                    string strData = "";
                                    for (int i = 0; i < strs.Length; i++)
                                    {
                                        strData += strs[i] + ";";
                                    }
                                    das += item.Name + "=" + strData + Environment.NewLine;
                                }
                                else
                                {
                                    das += item.Name + "=" + item.Value.ToString() + Environment.NewLine;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    listStr.Add(das);
                    if (mo["SettingID"].ToString() == id)
                    {
                        //设置IP地址和掩码
                        if (ip != null && submask != null)
                        {
                            inPar = mo.GetMethodParameters("EnableStatic");
                            inPar["IPAddress"] = ip;
                            inPar["SubnetMask"] = submask;
                            outPar = mo.InvokeMethod("EnableStatic", inPar, null);
                        }

                        //设置网关地址
                        if (getway != null)
                        {
                            inPar = mo.GetMethodParameters("SetGateways");
                            inPar["DefaultIPGateway"] = getway;
                            outPar = mo.InvokeMethod("SetGateways", inPar, null);
                        }

                        //设置DNS地址
                        if (dns != null)
                        {
                            inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                            inPar["DNSServerSearchOrder"] = dns;
                            outPar = mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return listStr;
        }

        /// <summary>
        /// 获取本地网络信息
        /// </summary>
        /// <returns></returns>
        public static List<ManagementObject> GetIPAddress()
        {
            List<ManagementObject> listStr = new List<ManagementObject>();
            try
            {
                ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = wmi.GetInstances();
                ManagementBaseObject inPar = null;
                ManagementBaseObject outPar = null;
                foreach (ManagementObject mo in moc)
                {
                    string das = "";
                    foreach (var item in mo.Properties)
                    {
                        if (item.Value != null)
                        {
                            try
                            {
                                if (item.Value is string[])
                                {
                                    string[] strs = item.Value as string[];
                                    string strData = "";
                                    for (int i = 0; i < strs.Length; i++)
                                    {
                                        strData += strs[i] + ";";
                                    }
                                    das += item.Name + "=" + strData + Environment.NewLine;
                                }
                                else
                                {
                                    das += item.Name + "=" + item.Value.ToString() + Environment.NewLine;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    listStr.Add(mo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return listStr;
        }

        /// <summary>
        /// 启用DHCP服务器
        /// </summary>
        public static void EnableDHCP()
        {
            ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = wmi.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                //如果没有启用IP设置的网络设备则跳过
                if (!(bool)mo["IPEnabled"])
                    continue;
                //重置DNS为空
                mo.InvokeMethod("SetDNSServerSearchOrder", null);
                //开启DHCP
                mo.InvokeMethod("EnableDHCP", null);
            }
        }

        /// <summary>
        /// 读取以太网网络IP信息
        /// </summary>
        /// <param name="ipStr"></param>
        /// <param name="mask"></param>
        /// <param name="gateWay"></param>
        /// <param name="DNS1"></param>
        /// <param name="DNS2"></param>
        public static void GetIPAddress(out string ipStr, out string mask, out string gateWay, out string DNS1, out string DNS2)
        {
            mask = gateWay = DNS1 = ipStr = DNS2 = "";
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                bool Pd1 = (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet); //判断是否是以太网连接

                if (Pd1 && adapter.Name == "以太网")
                {
                    IPInterfaceProperties ip = adapter.GetIPProperties();     //IP配置信息

                    for (int i = 0; i < ip.UnicastAddresses.Count; i++)
                    {
                        if (ip.UnicastAddresses[i].Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ipStr = ip.UnicastAddresses[i].Address.ToString();//IP地址
                            mask = ip.UnicastAddresses[i].IPv4Mask.ToString();//子网掩码
                        }
                    }

                    if (ip.GatewayAddresses.Count > 0)
                    {
                        gateWay = ip.GatewayAddresses[0].Address.ToString();//默认网关
                    }
                    int DnsCount = ip.DnsAddresses.Count;
                    //Console.WriteLine("DNS服务器地址：");
                    if (DnsCount > 0)
                    {
                        try
                        {
                            for (int i = 0; i < DnsCount; i++)
                            {
                                DNS1 += ip.DnsAddresses[i].ToString() + ";"; //主DNS
                            }
                        }
                        catch (Exception er)
                        {
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 字符串转指定CodeHex进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StringHexToByte(string hexString, int codeHex)
        {
            hexString = hexString.Replace(" ", "");
            hexString = hexString.Replace("-", "0");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), codeHex);
            return returnBytes;
        }

        /// <summary>
        /// 字节长度判断
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns></returns>
        public static int ReturnTypeLength(string type)
        {
            int lenght = 0;
            try
            {
                switch (type)
                {
                    case "Byte":
                        lenght = 2;
                        break;

                    case "Double":
                        lenght = 8;
                        break;

                    case "Int32":
                        lenght = 8;
                        break;

                    case "String":
                        lenght = 8;
                        break;

                    case "Int16":
                    case "UInt16":
                        lenght = 4;
                        break;

                    case "Char":
                        lenght = 1;
                        break;

                    case "Boolean":
                        lenght = 4;
                        break;

                    default:
                        lenght = 1;
                        break;
                }
                return lenght;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// CRC效验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CRC16(byte[] data)
        {
            if (data.Length > 0)
            {
                ushort crc = 0xFFFF;
                for (int i = 0; i < data.Length; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
                byte lo = (byte)(crc & 0x00FF);         //低位置
                return new byte[] { hi, lo };
            }
            return new byte[] { 0, 0 };
        }

        /// <summary>
        /// CRC效验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CRC16(string data)
        {
            if (data.Length > 0)
            {
                ushort crc = 0xFFFF;
                for (int i = 0; i < data.Length; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte[] bytes = StringHexToByte(data, 16);
                byte[] dete = BitConverter.GetBytes(crc);
                byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
                byte lo = (byte)(crc & 0x00FF);         //低位置
                byte[] det = new byte[] { hi, lo };
                byte[] db = new byte[bytes.Length + 2];
                bytes.CopyTo(db, 0);
                det.CopyTo(db, db.Length - 2);
                return db;
            }
            return new byte[] { 0, 0 };
        }

        /// <summary>
        /// FCS:异或方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string FCScheck(string data)
        {
            byte[] dse = Encoding.ASCII.GetBytes(data);
            //string de = ByteToHexStr(dse);
            byte head = dse[0];
            for (int i = 1; i < dse.Length; i++)
            {
                head ^= dse[i];
            }
            string dss = head.ToString("X2");
            return data + dss;
        }

        // 16进制字符串转字节数组   格式为 string sendMessage = "00 01 00 00 00 06 FF 05 00 64 00 00";
        private static byte[] HexStrTobyte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }

        // 字节数组转16进制字符串
        public static string ByteToHexStr(byte[] bytes)
        {
            string str0x = BitConverter.ToString(bytes, 0, bytes.Length).Replace("-", " ");
            return str0x;
        }

        //字节数组转16进制更简单的，利用BitConverter.ToString方法
        //string str0x = BitConverter.ToString(result, 0, result.Length).Replace("-"," ");

        #endregion 静态方法
    }
}