using ErosSocket.DebugPLC;
using ErosSocket.DebugPLC.DIDO;
using ErosSocket.ErosConLink;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project.DebugF.IO;
using Vision2.Project.formula;
using static Vision2.ErosProjcetDLL.Project.ProjectINI;

namespace Vision2.Project.DebugF
{
    public class DebugCompiler : ProjectObj, ProjectNodet.IClickNodeProject
    {
        /// <summary>
        /// 硬件
        /// </summary>
        public DebugCompiler()
        {
            this.Information = "调试功能";
            //this.Name = "调试";
            StaticThis = this;
        }

        /// <summary>
        /// 返回实例
        /// </summary>
        /// <returns></returns>
        //public static DebugCompiler Instance()
        //{
        //    if (StaticThis == null)
        //    {
        //        StaticThis = new DebugCompiler();
        //   }
        //    return StaticThis;
        //}
        public static DebugCompiler Instance
        {
            get
            {
                if (StaticThis == null)
                {
                    StaticThis = new DebugCompiler();
                }
                return StaticThis;
            }
        }

        public static ErosSocket.DebugPLC.Robot.TrayRobot GetTray(int index)
        {
            if (index < 0)
            {
                return null;
            }
            if (index > 16)
            {
                return null;
            }
            return DebugCompiler.Instance.DDAxis.ListTray[index];
        }

        private static DebugCompiler StaticThis;

        #region 继承重写方法

        private TreeNode treeNodeAlram = new TreeNode() { Name = "信息框", Text = "信息框" };
        private TreeNode treeNodeDebug = new TreeNode() { Name = "调试窗口", Text = "调试窗口" };

        [DescriptionAttribute("。"), Category("设备硬件"), DisplayName("板卡名称")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "FY6400", "C154", "PCI-1245L")]
        public string ListKat { get; set; } = "";

        [DescriptionAttribute("是否附加IO模块。"), Category("设备硬件"), DisplayName("附加IO模块")]
        public bool ListIO { get; set; }

        public override string FileName => "Debug";
        public override string Text { get; set; } = "调试";
        public override string SuffixName => ".Debug";
        public override string ProjectTypeName => "调试功能";
        public override string Name => "调试";

        /// <summary>
        /// 更新参数节点
        /// </summary>
        /// <param name = "treeNodet" ></ param >
        public override void UpProjectNode(TreeNode treeNodet)
        {
            try
            {
                base.UpProjectNode(treeNodet);
                Node.Nodes.Clear();
                treeNodeAlram = new TreeNode() { Name = "信息框", Text = "信息框" };
                treeNodeDebug = new TreeNode() { Name = "调试窗口", Text = "调试窗口" };
                //treeNodeDllDebug = new TreeNode() { Name = "外部程序", Text = "外部程序" };
                Node.Nodes.Add(treeNodeAlram);
                //treeNodeAlram.Tag=
                Node.Nodes.Add(treeNodeDebug);
                treeNodeDebug.Nodes.Clear();
                foreach (var item in ListDllPath)
                {
                    TreeNode tre = treeNodeDebug.Nodes.Add(item.Name);
                    tre.Tag = item.GetObjDll();
                }

                //treeNodeDebug.Tag=
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Name + "刷新错误:" + ex.Message);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void Close()
        {
            base.Close();
            if (DODI != null && DODI.IsInitialBool)
            {
                DODI.WritDO(RunButton.yellow, false);
                DODI.WritDO(RunButton.ANmen, false);
                DODI.WritDO(RunButton.Fmq, false);
                DODI.WritDO(RunButton.ANmen, false);
                DODI.WritDO(RunButton.green, false);
                DODI.WritDO(RunButton.ResetButtenS, false);
                DODI.WritDO(RunButton.RunButtenS, false);
                DODI.WritDO(RunButton.StopButtenS, false);
            }
            DDAxis.Close();
        }

        /// <summary>
        /// 管理控件
        /// </summary>
        /// <param name="formText"></param>
        public void SetUesrContrsl()
        {
            try
            {
                //textProgram.Btn_Start.Enabled = IsConnect;
                //textProgram.Btn_Stop.Enabled = IsConnect;
                //textProgram.Btn_Initialize.Enabled = IsConnect;
                //textProgram.Btn_Pause.Enabled = IsConnect;
                //textProgram.Btn_Reset.Enabled = IsConnect;
                PointFile.ReadPoint();
                StartWhil0eRun();
                thread = new Thread(ThreadRun);
                thread.IsBackground = true;
                thread.Start();
                thread = new Thread(RunTime);
                thread.IsBackground = true;
                thread.Start();
                thread = new Thread(RunBtuun);
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
            }
        }

        public enum FeedingModeEnum
        {
            载具模式 = 0,
            托盘模式 = 1,
            流水线模式 = 2,
        }

        /// <summary>
        /// 上料模式
        /// </summary>
        public FeedingModeEnum feedingModeEnum { get; set; }

        [DescriptionAttribute("。"), Category("选项功能"), DisplayName("显示产品参数")]
        public string PuPragrm { get; set; } = "";

        [DescriptionAttribute("。"), Category("选项功能"), DisplayName("使用设备控制")]
        public bool IsConnect { get; set; } = true;

        [DescriptionAttribute("。"), Category("选项功能"), DisplayName("使用导航图")]
        public bool IsSet { get; set; } = false;

        [DescriptionAttribute("。"), Category("选项功能"), DisplayName("显示状态")]
        public bool Display_Status { get; set; } = false;

        [DescriptionAttribute("。"), Category("选项功能"), DisplayName("显示用户ID输入框")]
        public bool UserIDText { get; set; } = false;

        [DescriptionAttribute("。"), Category("选项功能"), DisplayName("显示工单号输入框")]
        public bool Work_Order { get; set; } = false;

        [DescriptionAttribute("。"), Category("选项功能"), DisplayName("信息窗口位置")]
        [TypeConverter(typeof(ErosConverter)),
          ErosConverter.ThisDropDown("", false, "", "主窗口下", "控制栏左", "浮动窗口")]
        public string ErrTextS { get; set; } = "主窗口下";

        [DescriptionAttribute("当设备发生异常时必须初始化设备。"), Category("选项功能"), DisplayName("异常初始化")]
        public bool IsRunStrat { get; set; } = false;

        private Thread thread;

        #endregion 继承重写方法

        #region 属性

        [DescriptionAttribute("外部程序Dll。"), Category("外部程序"), DisplayName("加载的DLL地址")]
        public List<DllUers> ListDllPath { get; set; } = new List<DllUers>();

        //[DescriptionAttribute("控制方式数据。"), Category("控制"), DisplayName("用户控制方式"), Browsable(false)]
        //public UserInterfaceControl.UserInterfaceData Data { get; set; } = new UserInterfaceControl.UserInterfaceData();

        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("继续变量名")]
        public string LinkConnectName { get; set; }

        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("自动模式")]
        public string LinkAutoMode { get; set; }

        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("运行变量名")]
        public string LinkStart { get; set; } = "";

        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("暂停变量名")]
        public string LinkPause { get; set; } = "";

        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("停止变量名")]
        public string LinkStop { get; set; } = "";

        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("初始化变量名")]
        public string LinkInitialize { get; set; } = "";

        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("复位变量名")]
        public string LinkRestoration { get; set; } = "";

        [Editor(typeof(LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("错误变量名")]
        public string LinkAlarmName { get; set; }

        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("暂停中变量名")]
        public string LinkPauseName { get; set; }

        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("运行中变量名")]
        public string LinkRunName { get; set; }

        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("初始化中变量名")]
        public string LinkREName { get; set; }

        [Editor(typeof(LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("初始化完成")]
        public string LinkHomeDoneName { get; set; }

        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("初始化超时S")]
        public int HomeOutTime { get; set; } = 60;

        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("灯光控制")]
        public string LinklamplightName { get; set; } = "";

        [DescriptionAttribute("负数不支持轴移动,0低速，1中速度,2正常速度,3高速。"), Category("速度"), DisplayName("速度级别")]
        public byte LinkSeelpTyoe { get; set; } = 0;

        [DescriptionAttribute("复位IO写1=0。"), Category("设备控制"), DisplayName("复位IO")]
        public byte RsetIOTyoe { get; set; } = 0;

        [DescriptionAttribute("是否需要初始化失败。"), Category("设备控制"), DisplayName("是否需要初始化")]
        public bool IsHomeIt { get; set; } = true;

        /// <summary>
        /// 设置速度
        /// </summary>
        public void SetSeelp()
        {
            try
            {
                for (int i = 0; i < this.DDAxis.AxisS.Count; i++)
                {
                    this.DDAxis.AxisS[i].AddSeelp(LinkSeelpTyoe);
                }
            }
            catch (Exception)
            {
            }
        }

        private double MavValue { get; set; } = 500;
        private double StrValue { get; set; } = 80;
        private double AceValue { get; set; } = 400;

        private double DceValue { get; set; } = 400;

        /// <summary>
        /// 运行中停止
        /// </summary>
        public static bool RunStop;

        /// <summary>
        /// 停机中
        /// </summary>
        public static bool Stoping { get; set; }

        /// <summary>
        /// 错误
        /// </summary>
        public static bool IsAlarm { get; set; }

        public static bool ISHome { get; set; }

        /// <summary>
        /// 调试中
        /// </summary>
        public static bool Debuging { get; set; }

        /// <summary>
        ///
        /// </summary>
        public static Dictionary<string, string> dicstr;

        /// <summary>
        /// 设备状态
        /// </summary>
        private static EnumEquipmentStatus mEquipmentStatus;

        [DescriptionAttribute("设备的运行状态。"), Category("设备状态"), DisplayName("设备状态"), Browsable(false)]
        public static EnumEquipmentStatus EquipmentStatus
        {
            get => mEquipmentStatus;
            set
            {
                if (mEquipmentStatus != value)
                {
                    mEquipmentStatus = value;
                }
            }
        }

        /// <summary>
        /// 停机
        /// </summary>
        /// <param name="isCoerce">True是强制停机</param>
        public static void Stop(bool isCoerce = true)
        {
            try
            {
                if (isCoerce)
                {
                    StaticCon.SetLinkAddressValue(StaticThis.LinkStop, true);
                    EquipmentStatus = EnumEquipmentStatus.已停止;
                    GetRunP().Stop();
                    Instance.DDAxis.Stop();
                }
                else
                {
                    GetRunP().Stop();
                    Instance.DDAxis.Stop();
                    DebugCompiler.Stoping = true;
                }
                if (EquipmentStatus == EnumEquipmentStatus.初始化中)
                {
                    EquipmentStatus = EnumEquipmentStatus.已停止;
                }
                foreach (var item in StaticCon.SocketClint)
                {
                    if (item.Value.IsConn)
                    {
                        if (item.Value.PLCRun != null)
                        {
                            item.Value.PLCRun.Stop(isCoerce);
                        }
                    }
                }
                RunUpStatus(StaticThis.LinkStop, false);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 设备启动
        /// </summary>
        public static void Start()
        {
            RunStop = false;
            if (StaticThis.LinkStart != null)
            {
                if (EquipmentStatus == EnumEquipmentStatus.已停止)
                {
                    if (IsAlarm)
                    {
                        if (MessageBox.Show("存在故障，是否继续启动？", "故障", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) != DialogResult.Yes)
                        {
                            return;
                        }
                    }
                    if (StaticCon.SetLinkAddressValue(StaticThis.LinkStart, true))
                    {
                        Stoping = false;
                    }
                    foreach (var item in StaticCon.SocketClint)
                    {
                        if (item.Value.IsConn)
                        {
                            if (item.Value.PLCRun != null)
                            {
                                item.Value.PLCRun.Start();
                            }
                        }
                    }
                }
                else if (EquipmentStatus == EnumEquipmentStatus.暂停中)
                {
                    if (StaticCon.SetLinkAddressValue(StaticThis.LinkConnectName, true))
                    {
                        //Thread.Sleep(1200);
                    }
                }
                if (!Instance.IsHomeIt)
                {
                    DebugCompiler.ISHome = true;
                }
                if (DebugCompiler.ISHome)
                {
                    EquipmentStatus = EnumEquipmentStatus.运行中;
                    Thread.Sleep(200);
                    Product.IsSwitchover = true;
                    RecipeCompiler.GetUserFormulaContrsl().EnabledLog(true);
                    MainForm1.MainFormF.Btn_Start.Enabled = false;
                    StaticCon.SetLinkAddressValue(StaticThis.LinkPause, false);
                    StaticCon.SetLinkAddressValue(StaticThis.LinkConnectName, false);
                    RunUpStatus(StaticThis.LinkStart, false);
                }
                else
                {
                    AlarmListBoxt.AddAlarmText("未初始化设备", "");
                }
            }
            //RunUpStatus();
        }

        public bool Resume()
        {
            return false;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public static void Pause()
        {
            if (EquipmentStatus == EnumEquipmentStatus.运行中)
            {
                if (StaticCon.SetLinkAddressValue(StaticThis.LinkPause, true))
                {
                }
                EquipmentStatus = EnumEquipmentStatus.暂停中;
            }
            foreach (var item in StaticCon.SocketClint)
            {
                if (item.Value.IsConn)
                {
                    if (item.Value.PLCRun != null)
                    {
                        item.Value.PLCRun.Pause();
                    }
                }
            }
            RunUpStatus(StaticThis.LinkPause, false);
        }

        /// <summary>
        /// 复位
        /// </summary>
        public static void Rest()
        {
            try
            {
                RunStop = false;
                vision.Vision.TriggerSetup(StaticThis.RsetIOTyoe.ToString(), "true");
                //DicSocket.Instance.SetLinkSTime(false);
                StaticCon.SetLinkAddressValue(StaticThis.LinkRestoration, true);
                StaticCon.SetLinkAddressValue(StaticThis.LinkPause, false);
                StaticCon.SetLinkAddressValue(StaticThis.LinkStart, false);
                StaticCon.SetLinkAddressValue(StaticThis.LinkStop, false);
                StaticCon.SetLinkAddressValue(StaticThis.LinkConnectName, false);
                RunUpStatus(StaticThis.LinkRestoration, false);
                foreach (var item in StaticCon.SocketClint)
                {
                    if (item.Value.IsConn)
                    {
                        if (item.Value.PLCRun != null)
                        {
                            item.Value.PLCRun.Rest();
                        }
                    }
                }
                StaticThis.DDAxis.Reset();
                vision.Vision.TriggerSetup(StaticThis.RsetIOTyoe.ToString(), "false");
            }
            catch (Exception)
            {
            }
        }

        public static void Initialize()
        {
            try
            {
                if (ProcessControl.ProcessUser.Instancen.ProductMessage.Count > 1)
                {
                    if (MessageBox.Show("存在历史数据，是否清除历史数据？", "初始化", MessageBoxButtons.YesNo,
                          MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
                    {
                        ProcessControl.ProcessUser.ClearAll();
                    }
                }
                if (EquipmentStatus == EnumEquipmentStatus.初始化中)
                {
                    AlarmText.AddTextNewLine("正在初始化中", Color.Red);
                    return;
                }
                if (EquipmentStatus == EnumEquipmentStatus.运行中)
                {
                    AlarmText.AddTextNewLine("运行中无法初始化", Color.Red);
                    return;
                }
                EquipmentStatus = EnumEquipmentStatus.初始化中;
                StaticCon.SetLinkAddressValue(StaticThis.LinkInitialize, true);
                StaticCon.SetLinkAddressValue(StaticThis.LinkPause, false);
                StaticCon.SetLinkAddressValue(StaticThis.LinkStart, false);
                StaticCon.SetLinkAddressValue(StaticThis.LinkStop, false);
                StaticCon.SetLinkAddressValue(StaticThis.LinkConnectName, false);
                StaticCon.SetLingkValue(StaticThis.LinkHomeDoneName, false.ToString(), out string err);
                RunUpStatus(StaticThis.LinkInitialize, false);
                if (run_Project != null)
                {
                    run_Project.Homeing = false;
                }
                Task.Run(() =>
                {
                    AlarmText.AddTextNewLine("初始化开始", Color.Green);
                    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
                    Thread.Sleep(5000);
                    while (true.ToString() != StaticCon.GetLingkNameValueString(StaticThis.LinkHomeDoneName))
                    {
                        Thread.Sleep(10);
                        if (EquipmentStatus == EnumEquipmentStatus.初始化完成)
                        {
                            return;
                        }
                        if (EquipmentStatus == EnumEquipmentStatus.已停止)
                        {
                            AlarmText.AddTextNewLine("已停止初始化");
                            return;
                        }
                        if (watch.ElapsedMilliseconds >= StaticThis.HomeOutTime * 1000)
                        {
                            EquipmentStatus = EnumEquipmentStatus.错误停止中;
                            AlarmText.AddTextNewLine("初始化失败，超时" + watch.ElapsedMilliseconds / 1000 + "S", Color.Red);
                            DebugCompiler.RunStop = true;
                            Stop(true);
                            return;
                        }
                    }
                    EquipmentStatus = EnumEquipmentStatus.初始化完成;
                    AlarmText.AddTextNewLine("初始化完成，" + watch.ElapsedMilliseconds / 1000 + "S", Color.Green);
                });
                foreach (var item in StaticCon.SocketClint)
                {
                    if (item.Value.IsConn)
                    {
                        if (item.Value.PLCRun != null)
                        {
                            item.Value.PLCRun.Initialize();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 延时复位
        /// </summary>
        /// <param name="linkname"></param>
        /// <param name="isvalues"></param>
        /// <param name="time"></param>
        public static void RunUpStatus(string linkname, bool isvalues, int time = 200)
        {
            try
            {
                Task.Run(() =>
                {
                    Thread.Sleep(time);
                    StaticCon.SetLinkAddressValue(linkname, isvalues);
                });
            }
            catch (Exception)
            {
            }
        }

        #endregion 属性

        [Description("在托盘显示图片"), Category("托盘显示"), DisplayName("显示图片")]
        public bool IsImage { get; set; } = true;

        #region 任务管理

        [Browsable(false)]
        public List<工艺库.MatrixC> ListMatrix { get; set; } = new List<工艺库.MatrixC>();

        [Description("到位Mark等待时间"), Category("定位"), DisplayName("到位Mark等待时间")]
        public int MarkWait { get; set; } = 500;

        [Description("采图等待时间"), Category("定位"), DisplayName("采图等待时间")]
        public int CamWait { get; set; } = 50;

        public static List<string> ListName
        {
            get
            {
                return new List<string>(DebugCompiler.Instance.DDAxis.GetToPointName());
            }
        }

        [DescriptionAttribute("设备类型。"), Category("设备标识"), DisplayName("设备类型")]
        [TypeConverter(typeof(ErosConverter)),
          ErosConverter.ThisDropDown("", false, "", "环维水洗", "焊点V1.0", "焊线V1.0",
            "PinV1.0", "PinV2.0", "3D线体", "捷普测量1.0")]
        public string DeviceName { get; set; } = "";

        [DescriptionAttribute("设备显示名称。"), Category("设备标识"), DisplayName("设备显示名称")]
        [TypeConverter(typeof(ErosConverter)),
          ErosConverter.ThisDropDown("", false, "", "AVI", "AOI", "DIP")]
        public string DeviceNameText { get; set; } = "AVI";

        /// <summary>
        /// 运行模式
        /// </summary>
        [Description("调试、运行、自动、手动"), Category("运行参数"), DisplayName("运行模式")]
        public RunMode Run_Mode { get; set; } = new RunMode();

        [DescriptionAttribute("。"), Category("设备硬件"), DisplayName("板卡数量")]
        public byte Cont { get; set; } = 1;

        [DescriptionAttribute("。"), Category("设备硬件"), DisplayName("轴使能信号")]
        public sbyte Is_AxisEnble { get; set; } = -1;

        [DescriptionAttribute("。"), Category("设备硬件"), DisplayName("轴刹车信号")]
        public sbyte Is_braking { get; set; } = -1;

        [DescriptionAttribute("。"), Category("设备硬件"), DisplayName("使用软使能")]
        public bool Is_Sv { get; set; }

        [Description("使用皮带线体控制"), Category("线体"), DisplayName("在线设备")]
        public bool IsCtr
        {
            get;
            set;
        }

        /// <summary>
        /// 进站检测
        /// </summary>
        [DescriptionAttribute("板卡IO输入。"), Category("线体"), DisplayName("进站检测")]
        public sbyte IntCDI { get; set; } = -1;

        [DescriptionAttribute("板卡IO输出。"), Category("线体"), DisplayName("进站风枪")]
        public sbyte OutFDEX { get; set; } = -1;

        [DescriptionAttribute(""), Category("离线设备"), DisplayName("结束停机")]
        public bool RunEndStop { get; set; }

        [DescriptionAttribute("工位数量。"), Category("线体"), DisplayName("工位数")]
        public int Modet { get; set; } = 2;

        /// <summary>
        ///
        /// </summary>
        [DescriptionAttribute("信号检测到后，等待时间后停板。"), Category("线体"), DisplayName("出站等待时间")]
        public double OutwaitTime
        {
            get { return DDAxis.AlwaysIOOut.TimeS; }
            set { DDAxis.AlwaysIOOut.TimeS = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [DescriptionAttribute("到位信号检测到后，等待时间后运行。"), Category("线体"), DisplayName("到位等待时间")]
        public double waitTime { get; set; } = 2;

        [DescriptionAttribute("到位信号检测到移动距离。"), Category("线体"), DisplayName("到位步进距离")]
        ///
        public float WaitPoint { get; set; } = 10;

        [DescriptionAttribute("到位信号检测到移动速度。"), Category("线体"), DisplayName("到位步进速度")]
        public float WaitSleep { get; set; } = 20;

        /// <summary>
        /// 到站检测
        /// </summary>
        [DescriptionAttribute("板卡IO输入。"), Category("线体"), DisplayName("到站检测")]
        public sbyte RunDI { get; set; } = -1;

        /// <summary>
        /// 出站检测
        /// </summary>
        [DescriptionAttribute("板卡输入。"), Category("线体"), DisplayName("出站检测")]
        public sbyte OutDi { get; set; } = -1;

        /// <summary>
        /// 要板输出
        /// </summary>
        [DescriptionAttribute("板卡输出。"), Category("线体"), DisplayName("要板输出")]
        public sbyte To_Board_DO { get; set; } = -1;

        [DescriptionAttribute("进板时间。"), Category("线体"), DisplayName("进板时间")]
        public sbyte IntTime { get; set; } = 15;

        /// <summary>
        /// 要板输入
        /// </summary>
        [DescriptionAttribute("板卡输入。"), Category("线体"), DisplayName("要板输入")]
        public sbyte To_Board_DI { get; set; } = -1;

        [DescriptionAttribute("轴名称。"), Category("线体"), DisplayName("皮带轴名称")]
        [TypeConverter(typeof(ErosConverter)),
        ErosConverter.ThisDropDown("ListAxis", false, "")]
        public string AxisNameS { get; set; } = "皮带轴";

        public List<string> ListAxis
        {
            get
            {
                List<string> axis = new List<string>();
                for (int i = 0; i < this.DDAxis.AxisS.Count; i++)
                {
                    axis.Add(DDAxis.AxisS[i].Name);
                }
                return axis;
            }
        }

        [DescriptionAttribute("皮带移动的距离等于0一直移动。"), Category("线体"), DisplayName("皮带移动距离")]
        public double AxisPoint { get; set; } = 0;

        /// <summary>
        /// 固定位气缸
        /// </summary>
        [DescriptionAttribute("气缸名称。"), Category("线体"), DisplayName("定位气缸名称")]
        [TypeConverter(typeof(ErosConverter)),
    ErosConverter.ThisDropDown("ListCylinders", false, "")]
        public string LoctionCylinder { get; set; } = "定位气缸";

        public List<string> ListCylinders
        {
            get
            {
                List<string> axis = new List<string>();
                for (int i = 0; i < this.DDAxis.Cylinders.Count; i++)
                {
                    axis.Add(DDAxis.Cylinders[i].Name);
                }
                return axis;
            }
        }

        /// <summary>
        /// 前阻挡气缸
        /// </summary>
        [DescriptionAttribute("气缸名称。"), Category("线体"), DisplayName("阻挡气缸名称")]
        [TypeConverter(typeof(ErosConverter)),
        ErosConverter.ThisDropDown("ListCylinders", false, "")]
        public string RCylinder { get; set; } = "阻挡气缸";

        public DODIAxis DDAxis { get; set; } = new DODIAxis();

        private static Run_project run_Project;

        private static IDIDO DODI;

        public static IDIDO GetDoDi(IDIDO dIDO = null)
        {
            if (dIDO != null)
            {
                DODI = dIDO;
            }
            return DODI;
        }

        public static Run_project GetRunP(Run_project run_Pr = null)
        {
            if (run_Pr != null)
            {
                run_Project = run_Pr;
            }
            return run_Project;
        }

        public RunButton RunButton { get; set; } = new RunButton();

        /// <summary>
        ///
        /// </summary>
        /// <param name="lingkName"></param>
        /// <returns></returns>
        public static string GetLinkNmae(string lingkName)
        {
            try
            {
                string[] dat = lingkName.Split('.');

                if (dat[0] == "DI")
                {
                    return DODI.Int[int.Parse(dat[1])].ToString();
                }
                else if (dat[0] == "DO")
                {
                    return DODI.Out[int.Parse(dat[1])].ToString();
                }
                else
                {
                    return StaticCon.GetLingkNameValueString(lingkName);
                }
            }
            catch (Exception)
            { }
            return "";
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void StartWhil0eRun()
        {
            //DDAxis.axisG = new C154();
            run_Project = DebugCompiler.Instance.DDAxis as Run_project;

            run_Project.RunCodeT.RunStratCode += MainForm1.MainFormF.RunCodeT_RunStratCode;
            DDAxis.HomeCodeT.RunCode += HomeCodeT_RunCode;
            DDAxis.HomeCodeT.RunDone += HomeCodeT_RunDone; ;
            if (Cont >= 1)
            {
                if (DDAxis.Out.Name.Count == 0)
                {
                    DDAxis.Out.AddCont(16 * Cont);
                    DDAxis.Int.AddCont(16 * Cont);
                }
                else
                {
                    DDAxis.Out.AddCont(16 * Cont);
                    DDAxis.Int.AddCont(16 * Cont);
                }
            }

            if (ListIO)
            {
                IO.FY6400 fY6400 = new IO.FY6400() { ID = 0, };
                fY6400.Out = DDAxis.Out;
                fY6400.Int = DDAxis.Int;
                fY6400.Initial();
                DODI = fY6400 as IDIDO;
            }
            if (ListKat == "FY6400")
            {
                //socketClint.initialization();
                IO.FY6400 fY6400 = new IO.FY6400() { ID = 0, };
                fY6400.Out = DDAxis.Out;
                fY6400.Int = DDAxis.Int;
                fY6400.Initial();
                DODI = fY6400 as IDIDO;
            }
            else if (ListKat == "C154")
            {
                DODI = DDAxis as IDIDO;
            }
            else if (ListKat == "PCI-1245L")
            {
                if (!ListIO)
                {
                    DODI = DDAxis as IDIDO;
                }
            }
            try
            {
                //if (TrayCont > 0)
                //{
                //    if (TrayData == null)
                //    {
                //        TrayData = new TrayDataUserControl();
                //        TrayData.Dock = DockStyle.Fill;
                //        TabPage tabPage = new TabPage();
                //        tabPage.Text = tabPage.Name = "托盘状态";
                //        tabPage.Controls.Add(TrayData);
                //        MainForm1.MainFormF.tabControl1.Controls.Add(tabPage);
                //    }
                //}

                //TrayData.SetTray(this.DDAxis.GetTrayInxt(int.Parse(Project.formula.Product.GetProd()["托盘编号"])));
                if (StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName) != null)
                {
                    StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).PassiveEvent += DebugCompiler_PassiveEvent;
                }
                if (StaticCon.GetSocketClint(RecipeCompiler.Instance.RsetSoeverLinkName) != null)
                {
                    StaticCon.GetSocketClint(RecipeCompiler.Instance.RsetSoeverLinkName).PassiveEvent += DebugCompiler_PassiveEvent1; ;
                }
                if (StaticCon.GetSocketClint(RecipeCompiler.Instance.RsetLinkName) != null)
                {
                    StaticCon.GetSocketClint(RecipeCompiler.Instance.RsetLinkName).PassiveEvent += DebugCompiler_PassiveEvent2;
                }
            }
            catch (Exception)
            {
            }
            Thread thread = new Thread(() =>
            {
                if (run_Project != null)
                {
                    if (!DebugComp.GetThis().ISPLCDebug)
                    {
                        run_Project.Initial();
                    }
                    else
                    {
                        if (DebugComp.GetThis().ISPLCValues)
                        {
                            MainForm1.MainFormF.Invoke(new Action(() =>
                            {
                                PLCValuesUI Pclde = new PLCValuesUI();
                                Pclde.Dock = DockStyle.Fill;
                                TabPage tabPage = new TabPage();
                                tabPage.Text = tabPage.Name = "PLC值状态";
                                tabPage.Controls.Add(Pclde);
                                MainForm1.MainFormF.tabControl1.Controls.Add(tabPage);
                            }));
                        }
                    }
                    if (this.DDAxis.ListTray.Count == 0)
                    {
                        this.DDAxis.ListTray.AddRange(new ErosSocket.DebugPLC.Robot.TrayRobot[15]);
                    }
                    for (int i = 0; i < this.DDAxis.ListTray.Count; i++)
                    {
                        if (this.DDAxis.ListTray[i] == null)
                        {
                            this.DDAxis.ListTray[i] = new ErosSocket.DebugPLC.Robot.TrayRobot();
                        }
                        this.DDAxis.ListTray[i].UpS();
                    }
                    run_Project.Runing = false;
                    DebugCompiler.Instance.DDAxis.RunCodeT.Runing = false;
                    DebugCompiler.Instance.DDAxis.HomeCodeT.Runing = false;
                    DebugCompiler.Instance.DDAxis.StopCodeT.Runing = false;
                    run_Project.HomeDone = false;
                    while (true)
                    {
                        try
                        {
                            if (EquipmentStatus == EnumEquipmentStatus.运行中)
                            {
                                if (!run_Project.Runing)
                                {
                                    run_Project.Runing = run_Project.Run();
                                    if (DebugCompiler.Instance.IsCtr)
                                    {
                                        DebugCompiler.Instance.DDAxis.MoveAxisStop();
                                    }
                                }
                            }
                            else if (EquipmentStatus == EnumEquipmentStatus.已停止)
                            {
                                if (run_Project.Runing)
                                {
                                    run_Project.Runing = run_Project.Stop();
                                }
                                run_Project.Homeing = false;
                            }
                            else
                            {
                                //run_Project.Stop();
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        Thread.Sleep(10);
                    }
                }
            })
            {
                IsBackground = true,
                Priority = ThreadPriority.Highest
            };
            thread.Start();
        }

        /// <summary>
        /// 服务器接受的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="socket"></param>
        /// <param name="socketR"></param>
        /// <returns></returns>
        private string DebugCompiler_PassiveEvent2(byte[] key, SocketClint socket, System.Net.Sockets.Socket socketR)
        {
            string d = socket.GetEncoding().GetString(key);
            if (d.Length >= 100 || d.StartsWith("RsetTray"))
            {
                ErosSocket.DebugPLC.Robot.TrayRobot tray = JsonConvert.DeserializeObject<ErosSocket.DebugPLC.Robot.TrayRobot>(d.Remove(0, 8));
                if (tray != null)
                {
                    if (RecipeCompiler.Instance.GetMes() != null)
                    {
                        RecipeCompiler.Instance.GetMes().WrietMesAll(tray, ProcessControl.ProcessUser.QRCode, Product.ProductionName);
                    }
                }
            }
            return "";
        }

        private List<ErosSocket.DebugPLC.Robot.TrayRobot> listTray = new List<ErosSocket.DebugPLC.Robot.TrayRobot>();

        /// <summary>
        /// 服务器接受程序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="socket"></param>
        /// <param name="socketR"></param>
        /// <returns></returns>
        private string DebugCompiler_PassiveEvent1(byte[] key, SocketClint socket, System.Net.Sockets.Socket socketR)
        {
            string d = socket.GetEncoding().GetString(key);
            if (d.Length >= 100)
            {
                ErosSocket.DebugPLC.Robot.TrayRobot tray = JsonConvert.DeserializeObject<ErosSocket.DebugPLC.Robot.TrayRobot>(d.Remove(0, 4));
                if (tray != null)
                {
                    //tray.GetITrayRobot()
                    //DebugF.IO.TrayDataUserControl.SetStaticTray(tray);
                    listTray.Add(tray);
                    SimulateTrayMesForm.ShowMesabe("存在NG请复判!", tray.GetTrayData());
                    listTray.Remove(tray);
                }
            }
            return "";
        }

        /// <summary>
        /// 附加测试接受数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="socket"></param>
        /// <param name="socketR"></param>
        /// <returns></returns>
        private string DebugCompiler_PassiveEvent(byte[] key, SocketClint socket, System.Net.Sockets.Socket socketR)
        {
            try
            {
                string dataStr = socket.GetEncoding().GetString(key);
                DebugData(dataStr);
            }
            catch (Exception ex) { }
            return "";
        }

        //static ErosSocket.DebugPLC.Robot.TrayData TrayData;
        public static void DebugData(string dataStr)
        {
            try
            {
                if (RecipeCompiler.Instance.DataMinCont < dataStr.Length)
                {
                    double trayID = 1;
                    double DataNumber = 0;
                    List<string> liastStr = new List<string>();
                    if (dataStr.Contains(";"))
                    {
                        string[] dataVat = dataStr.Trim(';').Split(';');
                        double.TryParse(dataVat[0].Trim('+').Trim(','), out trayID);
                        double.TryParse(dataVat[1].Trim('+').Trim(','), out DataNumber);
                        string[] dataStrTd = new string[dataVat.Length - 2];
                        Array.Copy(dataVat, 2, dataStrTd, 0, dataStrTd.Length);
                        AlarmText.AddTextNewLine("穴位" + trayID + "次数" + DataNumber + "数据长度" + dataStrTd.Length.ToString());
                        if (DataNumber == 1)
                        {
                            RecipeCompiler.Instance.Data.Clear();
                        }
                        for (int i = 0; i < dataStrTd.Length; i++)
                        {
                            if (dataStrTd[i] != "")
                            {
                                liastStr = new List<string>();
                                liastStr.AddRange(dataStrTd[i].Trim(',').Split(','));
                                RecipeCompiler.Instance.Data.AddData(i, liastStr);
                            }
                        }
                    }
                    else
                    {
                        liastStr.AddRange(dataStr.Trim(',').Split(','));
                        if (liastStr.Count == 1)
                        {
                            if (RecipeCompiler.Instance.TrayCont >= 0)
                            {
                                DebugCompiler.GetTray(RecipeCompiler.Instance.TrayCont).GetITrayRobot().SetValue(double.Parse(liastStr[0]));
                            }
                        }
                        else if (liastStr.Count >= 1)
                        {
                            if (RecipeCompiler.Instance.TrayCont >= 0)
                            {
                                //DebugCompiler.GetTray(RecipeCompiler.Instance.TrayCont).GetITrayRobot().SetValue(liastStr);
                            }
                        }
                    }
                    if (DataNumber == RecipeCompiler.Instance.DataNumber)
                    {
                        if (RecipeCompiler.Instance.TrayCont >= 0)
                        {
                            DebugCompiler.Instance.DDAxis.GetTrayInxt(RecipeCompiler.Instance.TrayCont).GetTrayData().SetNumberValue(RecipeCompiler.Instance.Data.ListDatV, (int)trayID);
                            //DebugCompiler.GetTray(RecipeCompiler.Instance.TrayCont).GetTrayData().SetNumberValue(RecipeCompiler.Instance.Data.ListDatV, (int)trayID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void HomeCodeT_RunDone(RunCodeStr.RunErr key)
        {
            if (key.Done)
            {
                //ISHome = true;
                //EquipmentStatus = EnumEquipmentStatus.初始化完成;
            }
        }

        private void HomeCodeT_RunCode(RunCodeStr.RunErr key)
        {
            if (key.Err)
            {
                EquipmentStatus = EnumEquipmentStatus.错误停止中;
            }
        }

        /// <summary>
        /// 蜂鸣器
        /// </summary>
        public static bool FmqIS;

        /// <summary>
        /// 直通模式
        /// </summary>
        public static bool CPMode;

        /// <summary>
        /// 时间
        /// </summary>
        private void RunTime()
        {
        }

        public static bool Buzzer;

        /// <summary>
        /// 指示灯线程
        /// </summary>
        private void RunBtuun()
        {
            Thread.Sleep(3500);
            if (DODI != null)
            {
                DODI.WritDO(RunButton.ResetButtenS, false);
                DODI.WritDO(RunButton.RunButtenS, false);
                DODI.WritDO(RunButton.StopButtenS, false);
                DODI.WritDO(RunButton.red, false);
                DODI.WritDO(RunButton.green, false);
                DODI.WritDO(RunButton.yellow, false);
                DODI.WritDO(RunButton.Fmq, false);
            }
            while (true)
            {
                if (DODI == null)
                {
                    return;
                }
                if (DODI.IsInitialBool)
                {
                    if (AlarmText.DicAlarm.Count > 0)
                    {
                        if (!isarm)
                        {
                            Buzzer = true;
                            isarm = true;
                        }
                    }
                    else
                    {
                        if (isarm)
                        {
                            Buzzer = false;
                            isarm = false;
                            DODI.WritDO(RunButton.ResetButtenS, false);
                            DODI.WritDO(RunButton.red, false);
                            DODI.WritDO(RunButton.Fmq, false);
                        }
                        if (EquipmentStatus == EnumEquipmentStatus.暂停中)
                        {
                            DODI.WritDO(RunButton.RunButtenS, true);
                            DODI.WritDO(RunButton.yellow, true);
                            Thread.Sleep(500);
                            DODI.WritDO(RunButton.yellow, false);
                            DODI.WritDO(RunButton.RunButtenS, false);
                            Thread.Sleep(500);
                        }
                        else if (EquipmentStatus == EnumEquipmentStatus.初始化中)
                        {
                            DODI.WritDO(RunButton.yellow, true);
                            DODI.WritDO(RunButton.ResetButtenS, true);
                            Thread.Sleep(100);
                            DODI.WritDO(RunButton.ResetButtenS, false);
                            DODI.WritDO(RunButton.yellow, false);
                            Thread.Sleep(100);
                        }
                        else if (EquipmentStatus == EnumEquipmentStatus.未初始化)
                        {
                            DODI.WritDO(RunButton.yellow, true);
                            DODI.WritDO(RunButton.ResetButtenS, true);
                            Thread.Sleep(1000);
                            DODI.WritDO(RunButton.yellow, false);
                            DODI.WritDO(RunButton.ResetButtenS, false);
                            Thread.Sleep(1000);
                        }
                        else if (EquipmentStatus == EnumEquipmentStatus.初始化完成)
                        {
                            DODI.WritDO(RunButton.yellow, true);
                            //DODI.WritDO(RunButton.RunButtenS, true);
                            //Thread.Sleep(500);
                            //DODI.WritDO(RunButton.RunButtenS, false);
                            //Thread.Sleep(500);
                        }
                        else if (EquipmentStatus == EnumEquipmentStatus.已停止)
                        {
                            DODI.WritDO(RunButton.yellow, true);
                            //DODI.WritDO(RunButton.RunButtenS, true);
                            //Thread.Sleep(500);
                            DODI.WritDO(RunButton.RunButtenS, false);
                            //Thread.Sleep(500);
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        private bool isarm;
        private bool strartButon;
        private bool stopButon;
        private bool restButon;
        private bool InButon;

        /// <summary>
        /// 运行程序
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private void ThreadRun()
        {
            string isPatime = "";
            Thread.Sleep(1000);
            if (DODI == null)
            {
                return;
            }
            if (DODI.IsInitialBool)
            {
                DODI.WritDO(RunButton.yellow, true);
                DODI.WritDO(RunButton.ANmen, true);
            }
            EnumEquipmentStatus enumEquipmentStatus = EquipmentStatus;
            while (true)
            {
                try
                {
                    if (EquipmentStatus != enumEquipmentStatus)
                    {
                        enumEquipmentStatus = EquipmentStatus;
                        if (MainForm1.MainFormF.Created)
                        {
                            MainForm1.MainFormF.Invoke(new Action(() =>
                            {
                                MainForm1.MainFormF.labelStat1.Text = EquipmentStatus.ToString();
                            }));
                        }
                        string dsa = StaticCon.GetLingkNameValueString(StaticThis.LinkPauseName);
                        if (dsa == true.ToString() && EquipmentStatus != EnumEquipmentStatus.初始化中)
                        {
                            DODI.WritDO(RunButton.yellow, true);
                            DODI.WritDO(RunButton.green, false);
                            DODI.WritDO(RunButton.RunButtenS, false);
                            isPatime = dsa;
                            EquipmentStatus = EnumEquipmentStatus.暂停中;

                            MainForm1.MainFormF.Btn_Start.Text = "继续";
                            MainForm1.MainFormF.Btn_Debug.Enabled =
                            MainForm1.MainFormF.Btn_Start.Enabled = true;
                        }
                        else
                        {
                            dsa = StaticCon.GetLingkNameValueString(StaticThis.LinkRunName);
                            if (dsa == true.ToString())
                            {
                                isPatime = false.ToString();
                                if (true.ToString() == StaticCon.GetLingkNameValueString(StaticThis.LinkREName))
                                {
                                    EquipmentStatus = EnumEquipmentStatus.初始化中;
                                }
                                else
                                {
                                    EquipmentStatus = EnumEquipmentStatus.运行中;
                                }
                            }
                            else
                            {
                                if (EquipmentStatus == EnumEquipmentStatus.初始化完成)
                                {
                                    MainForm1.MainFormF.Btn_Debug.Enabled =
                              MainForm1.MainFormF.Btn_Initialize.Enabled =
                               MainForm1.MainFormF.Btn_Start.Enabled = true;
                                    MainForm1.MainFormF.Btn_Start.Text = "启动";
                                    Product.IsSwitchover = true;
                                    RecipeCompiler.GetUserFormulaContrsl().EnabledLog(true);
                                }
                                else if (EquipmentStatus != EnumEquipmentStatus.初始化中)
                                {
                                    MainForm1.MainFormF.Btn_Start.Text = "启动";
                                    Product.IsSwitchover = false;
                                    RecipeCompiler.GetUserFormulaContrsl().EnabledLog(true);
                                    Stoping = false;
                                }
                                else if (EquipmentStatus == EnumEquipmentStatus.初始化中)
                                {
                                    MainForm1.MainFormF.Btn_Start.Text = "启动";
                                    Product.IsSwitchover = false;
                                    RecipeCompiler.GetUserFormulaContrsl().EnabledLog(false);
                                    Stoping = false;
                                    MainForm1.MainFormF.Btn_Start.Enabled = false;
                                }
                            }
                        }

                        if (EquipmentStatus == EnumEquipmentStatus.运行中)
                        {
                            if (DODI.IsInitialBool)
                            {
                                DODI.WritDO(RunButton.ANmen, true);
                                DODI.WritDO(RunButton.yellow, false);
                                DODI.WritDO(RunButton.green, true);
                                DODI.WritDO(RunButton.RunButtenS, false);
                                DODI.WritDO(RunButton.RunButtenS, true);
                                DODI.WritDO(RunButton.StopButtenS, false);
                            }
                            MainForm1.MainFormF.Btn_Start.Text = "启动";
                            MainForm1.MainFormF.Btn_Debug.Enabled =
                            MainForm1.MainFormF.Btn_Initialize.Enabled =
                             MainForm1.MainFormF.Btn_Start.Enabled = false;
                            MainForm1.MainFormF.Btn_Pause.Enabled = true;
                            if (run_Project.Pauseing)
                            {
                                run_Project.Cont();
                            }
                        }
                        else if (EquipmentStatus == EnumEquipmentStatus.已停止)
                        {
                            if (DODI.IsInitialBool)
                            {
                                DODI.WritDO(RunButton.yellow, true);
                                DODI.WritDO(RunButton.green, false);
                                DODI.WritDO(RunButton.RunButtenS, false);
                                DODI.WritDO(RunButton.StopButtenS, true);
                            }
                            MainForm1.MainFormF.Btn_Start.Enabled =
                             MainForm1.MainFormF.Btn_Debug.Enabled =
                             MainForm1.MainFormF.Btn_Initialize.Enabled = true;
                        }
                        else if (EquipmentStatus == EnumEquipmentStatus.暂停中)
                        {
                            run_Project.Pause();
                            MainForm1.MainFormF.Btn_Start.Enabled = true;
                            MainForm1.MainFormF.Btn_Start.Text = "继续";
                        }
                        dsa = StaticCon.GetLingkNameValueString(StaticThis.LinkAlarmName);
                        if (dsa == true.ToString())
                        {
                            //textProgram.labelAram.Text = "故障";
                            IsAlarm = true;
                            //textProgram.labelAram.BackColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            IsAlarm = false;
                            //textProgram.labelAram.Text = "正常";
                            //textProgram.labelAram.BackColor = System.Drawing.Color.Green;
                        }
                        //if (this.LinkPause == "")
                        //{
                        //    MainForm1.MainFormF.Btn_Pause.Enabled = false;
                        //}
                        //else
                        //{
                        //    MainForm1.MainFormF.Btn_Pause.Enabled = true;
                        //}
                        if (EquipmentStatus == EnumEquipmentStatus.暂停中)
                        {
                            run_Project.Pause();
                        }
                        if (EquipmentStatus == EnumEquipmentStatus.初始化中)
                        {
                            if (!run_Project.Homeing)
                            {
                                run_Project.Homeing = true;
                                run_Project.HomeDone = false;
                                Thread.Sleep(10);
                                run_Project.SetHome();
                            }
                            else
                            {
                                if (run_Project.HomeDone)
                                {
                                    DebugCompiler.ISHome = true;
                                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("初始化完成");
                                    EquipmentStatus = EnumEquipmentStatus.初始化完成;
                                }
                            }
                        }
                    }
                    if (DODI.IsInitialBool)
                    {
                        if (RunButton.RunButten >= 0)
                        {
                            if (DODI.Int[RunButton.RunButten] != strartButon)
                            {
                                strartButon = DODI.Int[RunButton.RunButten];
                                if (strartButon)
                                {
                                    Start();
                                }
                            }
                        }
                        if (RunButton.ResetButten >= 0)
                        {
                            if (DODI.Int[RunButton.ResetButten] != restButon)
                            {
                                restButon = DODI.Int[RunButton.ResetButten];
                                if (restButon)
                                {
                                    Rest();
                                }
                            }
                        }
                        if (RunButton.InButten >= 0)
                        {
                            if (DODI.Int[RunButton.InButten] != InButon)
                            {
                                InButon = DODI.Int[RunButton.InButten];
                                if (InButon)
                                {
                                    Initialize();
                                }
                            }
                        }

                        if (RunButton.StopButten >= 0)
                        {
                            if (DODI.Int[RunButton.StopButten] != stopButon)
                            {
                                stopButon = DODI.Int[RunButton.StopButten];
                                if (stopButon)
                                {
                                    Stop(true);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
                Thread.Sleep(10);
            }
        }

        public Control GetThisControl()
        {
            return new CommandControl1(this);
        }

        public override void initialization()
        {
            try
            {
                //textProgram.Btn_Start.Enabled = IsConnect;
                //textProgram.Btn_Stop.Enabled = IsConnect;
                //textProgram.Btn_Initialize.Enabled = IsConnect;
                //textProgram.Btn_Pause.Enabled = IsConnect;
                //textProgram.Btn_Reset.Enabled = IsConnect;
                PointFile.ReadPoint();
                StartWhil0eRun();
                thread = new Thread(ThreadRun);
                thread.IsBackground = true;
                thread.Start();
                thread = new Thread(RunTime);
                thread.IsBackground = true;
                thread.Start();
                thread = new Thread(RunBtuun);
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            { }
        }

        #endregion 任务管理
    }
}