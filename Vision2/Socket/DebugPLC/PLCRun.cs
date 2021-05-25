using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace ErosSocket.DebugPLC
{
    public class PLCRun : IPLCInterface
    {
        InterfacePLCUserControl textProgram;
        void ThreadRun()
        {
            try
            {
                while (textProgram != null && !textProgram.IsDisposed)
                {
                    string dsa = StaticCon.GetLingkNameValueString(LinkPauseName);
                    if (dsa == true.ToString())
                    {
                        EquipmentStatus = EnumEquipmentStatus.暂停中;
                        textProgram.labelStat.Text = EquipmentStatus.ToString();
                        textProgram.Btn_Start.Text = "继续";
                        textProgram.Btn_Start.Enabled = true;
                    }
                    else
                    {
                        dsa = StaticCon.GetLingkNameValueString(LinkRunName);
                        if (dsa == true.ToString())
                        {

                            if (true.ToString() == StaticCon.GetLingkNameValueString(LinkREName))
                            {
                                EquipmentStatus = EnumEquipmentStatus.初始化中;
                                textProgram.labelStat.Text = EquipmentStatus.ToString();
                            }
                            else
                            {
                                EquipmentStatus = EnumEquipmentStatus.运行中;
                                textProgram.labelStat.Text = EquipmentStatus.ToString();
                            }
                        }
                        else
                        {
                            if (EquipmentStatus != EnumEquipmentStatus.初始化中)
                            {
                                textProgram.Btn_Start.Text = "启动";
                                EquipmentStatus = EnumEquipmentStatus.已停止;
                                textProgram.labelStat.Text = EquipmentStatus.ToString();
                                //formula.Product.IsSwitchover = false;
                                //formula.RecipeCompiler.GetUserFormulaContrsl().EnabledLog(true);
                                Stoping = false;
                            }
                            else if (EquipmentStatus == EnumEquipmentStatus.初始化中)
                            {
                                textProgram.Btn_Start.Text = "启动";
                                textProgram.labelStat.Text = EquipmentStatus.ToString();
                                //formula.Product.IsSwitchover = false;
                                //formula.RecipeCompiler.GetUserFormulaContrsl().EnabledLog(false);
                                Stoping = false;
                                textProgram.Btn_Start.Enabled = false;
                            }
                        }
                    }
                    if (EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.运行中)
                    {
                        textProgram.Btn_Start.Text = "启动";
                        textProgram.Btn_Debug.Enabled =
                        textProgram.Btn_Initialize.Enabled =
                        textProgram.Btn_Start.Enabled = false;
                    }
                    else if (EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                    {
                        textProgram.Btn_Start.Enabled =
                         textProgram.Btn_Debug.Enabled =
                        textProgram.Btn_Initialize.Enabled = true;
                    }
                    dsa = StaticCon.GetLingkNameValueString(LinkAlarmName);
                    if (dsa == true.ToString())
                    {
                        textProgram.labelAram.Text = "故障";
                        IsAlarm = true;
                        textProgram.labelAram.BackColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        IsAlarm = false;
                        textProgram.labelAram.Text = "正常";
                        textProgram.labelAram.BackColor = System.Drawing.Color.Green;
                    }
                    if (this.LinkPause == "")
                    {
                        textProgram.Btn_Pause.Enabled = false;
                    }
                    else
                    {
                        textProgram.Btn_Pause.Enabled = true;
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception)
            {
            }
        }
        public PLCRun()
        {
            ProjectINI.In.ModeEvenT += SelpMode;
        }

        public bool SelpMode(string key, bool selpMode)
        {
            if (LinkSelpMode != "")
            {
                StaticCon.SetLingkValue(this.Name + "." + LinkSelpMode, selpMode, out string err);
            }

            return false;
        }

        public string Name { get; set; }
        //[DescriptionAttribute("控制方式数据。"), Category("控制"), DisplayName("用户控制方式"), Browsable(false)]
        //public UserInterfaceControl.UserInterfaceData Data { get; set; } = new UserInterfaceControl.UserInterfaceData();
        [DescriptionAttribute("。"), Category("显示"), DisplayName("true颜色")]
        public Color True_Color { get; set; }
        [DescriptionAttribute("。"), Category("显示"), DisplayName("Fales颜色")]
        public Color FalesColor { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("继续变量名")]
        public string LinkConnectName { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("运行变量名")]
        public string LinkStart { get; set; } = "";
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("暂停变量名")]
        public string LinkPause { get; set; } = "";
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("停止变量名")]
        public string LinkStop { get; set; } = "";
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("初始化变量名")]
        public string LinkInitialize { get; set; } = "";
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("复位变量名")]
        public string LinkRestoration { get; set; } = "";
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("运行模式")]
        public string SetStatModeName { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("单步模式。"), Category("设备控制"), DisplayName("单步模式")]
        public string LinkSelpMode { get; set; } = "";

        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
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
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("初始化完成")]
        public string LinkHomeDoneName { get; set; }
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("初始化超时S")]
        public int HomeOutTime { get; set; } = 60;
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("读取设备状态地址名")]
        [Editor(typeof(LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string GetStatLinkName { get; set; }
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("设备运行ID")]
        [Editor(typeof(LinkNameListValueNameUserControl1.Editor), typeof(UITypeEditor))]
        public LinkNameListValueNameUserControl1.LinkNameListValueName ListNameRunID
        {
            get
            {
                listRunID.LinkName = this.Name;
                return listRunID;
            }

            set
            {

                listRunID = value;
            }
        }
        LinkNameListValueNameUserControl1.LinkNameListValueName listRunID = new LinkNameListValueNameUserControl1.LinkNameListValueName();
        public List<string> ListRunIDEnum { get; set; }
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("运行模式")]
        public string GetStatModeName { get; set; }
        [Editor(typeof(IDEnumUserControl1.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("读取设备状态枚举")]
        public Dictionary<int, string> KeyIDStr { get; set; }


        [DescriptionAttribute("。"), Category("换型控制"), DisplayName("换型条件名")]
        public string LinkHCIF { get; set; }

        [DescriptionAttribute("。"), Category("换型控制"), DisplayName("换型ID名")]
        public string LinkIDname { get; set; }

        [DescriptionAttribute("。"), Category("换型控制"), DisplayName("换型启动")]
        public string LinkCOn { get; set; }

        /// <summary>
        /// 停机中
        /// </summary>
        public bool Stoping { get; set; }
        /// <summary>
        /// 错误
        /// </summary>
        public bool IsAlarm { get; set; }
        /// <summary>
        /// 调试中
        /// </summary>
        public bool Debuging { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        EnumEquipmentStatus mEquipmentStatus;
        [DescriptionAttribute("设备的运行状态。"), Category("设备状态"), DisplayName("设备状态"), Browsable(false)]
        public EnumEquipmentStatus EquipmentStatus
        {
            get => mEquipmentStatus;
            set
            {
                if (mEquipmentStatus != value)
                {
                    //StubManager.getDevice().onProcessStateChanged((int)mEquipmentStatus, (int)value);
                    mEquipmentStatus = value;
                }
            }
        }
        /// <summary>
        /// 停机
        /// </summary>
        /// <param name="isCoerce">True是强制停机</param>
        public void Stop(bool isCoerce = false)
        {
            if (EquipmentStatus == EnumEquipmentStatus.初始化中)
            {
                EquipmentStatus = EnumEquipmentStatus.已停止;
            }
            if (isCoerce)
            {
                StaticCon.SetLinkAddressValue(this.Name + "." + LinkStop, true);

                //StaticCon.SetLinkAddressValue(StaticThis.LinkStop, false);
                //EquipmentStatus = EnumEquipmentStatus.已停止;
            }
            else
            {
                Stoping = true;
                StaticCon.SetLinkAddressValue(this.Name + "." + LinkStop, true);
            }
            RunUpStatus(this.Name + "." + LinkStop, false);
        }
        /// <summary>
        /// 设备启动
        /// </summary>
        public void Start()
        {
            if (LinkStart != null)
            {
                if (EquipmentStatus == EnumEquipmentStatus.已停止)
                {
                    if (IsAlarm)
                    {
                        if (MessageBox.Show(this.Name + "存在故障，是否继续启动？", "故障", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) != DialogResult.Yes)
                        {
                            return;
                        }
                    }
                    if (StaticCon.SetLinkAddressValue(this.Name + "." + LinkStart, true))
                    {
                        Stoping = false;
                    }
                }
                else if (EquipmentStatus == EnumEquipmentStatus.暂停中)
                {
                    if (LinkConnectName != null)
                    {
                        if (StaticCon.SetLinkAddressValue(this.Name + "." + LinkConnectName, true))
                        {
                        }
                    }

                }
                Thread.Sleep(100);
                //Product.IsSwitchover = true;
                //RecipeCompiler.GetUserFormulaContrsl().EnabledLog(true);
                //textProgram.Btn_Start.Enabled = false;
                //textProgram.labelStat.Text = EquipmentStatus.ToString();
                if (LinkPause != null && LinkPause != "")
                {
                    StaticCon.SetLinkAddressValue(this.Name + "." + LinkPause, false);
                }
                if (LinkConnectName != null && LinkConnectName != "")
                {
                    StaticCon.SetLinkAddressValue(this.Name + "." + LinkConnectName, false);
                }
                RunUpStatus(this.Name + "." + LinkStart, false);

            }
            // RunUpStatus(this.Name + "." +);
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            //string err = "";
            if (EquipmentStatus == EnumEquipmentStatus.运行中)
            {
                if (StaticCon.SetLinkAddressValue(this.Name + "." + LinkPause, true))
                {
                }
            }
            RunUpStatus(this.Name + "." + LinkPause, false);
        }

        public void Rest()
        {
            try
            {


                StaticCon.SetLinkAddressValue(this.Name + "." + LinkRestoration, true);
                StaticCon.SetLinkAddressValue(this.Name + "." + LinkPause, false);
                StaticCon.SetLinkAddressValue(this.Name + "." + LinkStart, false);
                StaticCon.SetLinkAddressValue(this.Name + "." + LinkStop, false);
                StaticCon.SetLinkAddressValue(this.Name + "." + LinkConnectName, false);
                RunUpStatus(this.Name + "." + LinkRestoration, false);
            }
            catch (Exception ex)
            {

            }
        }

        public void Initialize()
        {
            try
            {
                if (LinkInitialize == "")
                {
                    return;
                }
                if (EquipmentStatus == EnumEquipmentStatus.初始化中)
                {
                    AlarmText.AddTextNewLine(this.Name + "正在初始化中", Color.Red);
                    return;
                }
                if (EquipmentStatus == EnumEquipmentStatus.运行中)
                {
                    AlarmText.AddTextNewLine(this.Name + "运行中无法初始化", Color.Red);
                    return;
                }
                EquipmentStatus = EnumEquipmentStatus.初始化中;
                StaticCon.SetLinkAddressValue(this.Name + "." + LinkInitialize, true);
                StaticCon.SetLinkAddressValue(this.Name + "." + LinkPause, false);
                StaticCon.SetLinkAddressValue(this.Name + "." + LinkStart, false);
                StaticCon.SetLinkAddressValue(this.Name + "." + LinkStop, false);
                StaticCon.SetLinkAddressValue(this.Name + "." + LinkConnectName, false);
                StaticCon.SetLingkValue(this.Name + "." + LinkHomeDoneName, false.ToString(), out string err);
                RunUpStatus(this.Name + "." + LinkInitialize, false);
                Task.Run(() =>
                {
                    AlarmText.AddTextNewLine(this.Name + "初始化开始", Color.Green);
                    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
                    Thread.Sleep(2000);
                    while (true.ToString() != StaticCon.GetLingkNameValueString(this.Name + "." + LinkHomeDoneName))
                    {
                        if (EquipmentStatus == EnumEquipmentStatus.已停止)
                        {
                            AlarmText.AddTextNewLine(this.Name + "已停止初始化");
                            return;
                        }
                        if (watch.ElapsedMilliseconds >= HomeOutTime * 1000)
                        {
                            EquipmentStatus = EnumEquipmentStatus.错误停止中;
                            AlarmText.AddTextNewLine(this.Name + "初始化失败，超时" + watch.ElapsedMilliseconds / 1000 + "S", Color.Red);
                            return;
                        }
                    }
                    EquipmentStatus = EnumEquipmentStatus.初始化完成;
                    AlarmText.AddTextNewLine(this.Name + "初始化完成，" + watch.ElapsedMilliseconds / 1000 + "S", Color.Green);
                });
            }
            catch (Exception)
            {

            }
        }

        public static void RunUpStatus(string linkname, bool isvalues, int time = 1000)
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

    }
}
