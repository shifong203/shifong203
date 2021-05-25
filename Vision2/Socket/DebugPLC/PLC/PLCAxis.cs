using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Threading.Tasks;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;

namespace ErosSocket.DebugPLC.PLC
{
    /// <summary>
    /// PLC轴控制类
    /// </summary>
    public class PLCAxis : Vision2.ErosProjcetDLL.Project.INodeNew, IAxis
    {

        public double Scale { get; set; }
        [DescriptionAttribute("。"), Category("轴信息"), DisplayName("轴名称")]
        public override string Name { get; set; }
        [Description("2D坐标彷射"), Category("坐标系统"), DisplayName("坐标系统"),
         TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "位置轴", "旋转轴", "速度轴")]
        public string AxisMode { get; set; } = "位置轴";
        [DescriptionAttribute("。"), Category("轴信息"), DisplayName("最大限位")]
        public Single MaxPoint { get; set; } = 500;
        [DescriptionAttribute("。"), Category("轴状态"), DisplayName("是否已同步原点")]
        public bool IsHome { get; set; }
        public double Point { get; set; }
        /// <summary>
        /// 故障
        /// </summary>
        public bool Alarm { get; set; }
        [DescriptionAttribute("。"), Category("执行机制"), DisplayName("回原点超时")]
        public int HomeTime { get; set; } = 20000;
        [DescriptionAttribute("。"), Category("执行机制"), DisplayName("最小限位")]
        public Single MinPoint { get; set; } = 0;
        [DescriptionAttribute("。"), Category("执行机制"), DisplayName("步进距离")]
        public double Jog_Distance { get; set; } = 1f;
        public bool enabled { get; set; }
        [DescriptionAttribute("-2超出最大限位，-3超出最小限位，-1执行失败。"), Category("执行机制"), DisplayName("错误代码")]
        public int ErrCode { get; set; } = 0;
        /// <summary>
        /// 正点动
        /// </summary>
        [Editor(typeof(ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("轴控制地址"), DisplayName("正点动")]
        public string XAddAddress { get; set; }
        /// <summary>
        /// 反点动
        /// </summary>
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("轴控制地址"), DisplayName("反点动")]
        public string X_Address { get; set; }
        /// <summary>
        /// 寸动模式
        /// </summary>
        [DescriptionAttribute("。"), Category("轴控制"), DisplayName("寸动模式")]
        public bool XMode { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        /// <summary>
        /// 同步原点地址
        /// </summary>
        [DescriptionAttribute("。"), Category("轴控制地址"), DisplayName("同步原点")]
        public string XHomeAddress { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("轴控制地址"), DisplayName("复位地址")]
        public string XRsetAddress { get; set; }
        /// <summary>
        /// 存在错误地址
        /// </summary>
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("轴状态地址"), DisplayName("错误")]
        public string XAralmAddress { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        /// <summary>
        /// 已使能地址
        /// </summary>
        [DescriptionAttribute("。"), Category("轴状态地址"), DisplayName("使能反馈")]
        public string XEnbelnAddress { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        /// <summary>
        /// 已同步原点
        /// </summary>
        [DescriptionAttribute("。"), Category("轴状态地址"), DisplayName("已同步原点")]
        public string XISHomeAddress { get; set; }
        /// <summary>
        /// 移动到目标位置地址
        /// </summary>
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("轴控制地址"), DisplayName("去目标位置")]
        public string XSetPAddress { get; set; }
        /// <summary>
        /// 移动到目标位置地址
        /// </summary>
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("轴控制地址"), DisplayName("轴停止地址")]
        public string XStopAddress { get; set; }
        /// <summary>
        /// 目标位置地址
        /// </summary>
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("轴控制地址"), DisplayName("目标位置")]
        public string XSPAddress { get; set; }
        /// <summary>
        /// 寸动距离
        /// </summary>
        [DescriptionAttribute("。"), Category("轴控制"), DisplayName("寸动距离")]
        public Single XAddP { get; set; } = 1.00F;
        /// <summary>
        /// 移动速度地址
        /// </summary>
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("轴控制地址"), DisplayName("移动速度")]
        public string XSpeelAddress { get; set; }
        /// <summary>
        /// 当前位置地址
        /// </summary>
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("轴状态地址"), DisplayName("当前位置")]
        public string XPAddress { get; set; } = "";
        /// <summary>
        /// 标准轴
        /// </summary>
        [DescriptionAttribute("是否是E标准结构轴"), Category("轴结构"), DisplayName("E标准轴")]
        public bool IsEMode { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        /// <summary>
        /// 起点地址
        /// </summary>
        [DescriptionAttribute("轴组数据起点地址"), Category("数据地址"), DisplayName("起点地址")]
        public string XAddressID { get; set; } = "0.0";
        public float Ratio { get; set; }
        public int PlusLimit { get; set; }
        public int MinusLimit { get; set; }
        public double MaxVel { get; set; }

        public bool IsError { get; set; }
        public bool IsEnabled { get; set; }
        public sbyte IsBand_type_brakeNumber { get; set; } = -1;

        public EnumAxisType AxisType { get; set; }

        public const string XjogSID = "0.0";
        public const string Xjog_ID = "0.1";
        public const string XModeID = "0.2";
        public const string XHomeID = "0.3";
        public const string XisHomeID = "0.4";
        public const string AramlPID = "0.5";
        public const string EnabledID = "0.6";
        public const string XSetID = "0.7";
        public const string XJogPID = "1";
        public const string XPID = "2";
        public const string XSeepID = "6";
        public const byte MaxByte = 14;


        public bool Initial()
        {
            return true;
            //ErosSocket.ErosConLink.S71200.GetIPAddress
            //ErosSocket.ErosConLink.StaticCon.GetSocketClint("").GetIDValue(XAddressID,);
        }
        /// <summary>
        /// 点动寸动模式，移动
        /// </summary>
        /// <param name="JogPsion">正方向False或反方向True</param>
        /// <param name="jogmode">默认寸动模式</param>
        /// <param name="seepJog">寸动距离1.00mm</param>
        public void JogAdd(bool JogPsion, bool jogmode = true, double seepJog = 1.00F)
        {
            if (XPAddress == "")
            {
                return;
            }
            if (jogmode)
            {
                if (JogPsion)
                {
                    seepJog = -seepJog;
                }
                else
                {
                    seepJog = Math.Abs(seepJog);
                }
                SetPiconin(GetPoint() + seepJog);
            }
            else
            {
                if (JogPsion)
                {
                    if (GetPoint() < this.MinPoint)
                    {
                        ErrCode = -3;
                        System.Windows.Forms.MessageBox.Show("超出最小限位");
                    }
                    ErosConLink.StaticCon.SetLinkAddressValue(X_Address, true);
                }
                else
                {
                    if (GetPoint() > this.MaxPoint)
                    {
                        ErrCode = -2;
                        if (Vision2.ErosProjcetDLL.Project.ProjectINI.DebugMode)
                        {
                            System.Windows.Forms.MessageBox.Show("超出最大限位");
                        }
                        else
                        {
                            Vision2.ErosProjcetDLL.Project.AlarmText.LogErr("定位超出最大限位", this.Name);
                        }
                    }
                    ErosConLink.StaticCon.SetLinkAddressValue(XAddAddress, true);
                }
            }
        }


        /// <summary>
        /// 移动绝对位置
        /// </summary>
        public void SetPiconin(double picoint)
        {
            ErrCode = 1;
            Task.Run(() =>
            {
                try
                {
                    Single dvae = ErosSocket.ErosConLink.StaticCon.GetLingkNameValue(XPAddress);
                    if (dvae < picoint && picoint > this.MaxPoint)
                    {
                        ErrCode = -2;
                        System.Windows.Forms.MessageBox.Show("超出最大限位");
                        picoint = MaxPoint;
                    }
                    else if (dvae > picoint && picoint < this.MinPoint)
                    {
                        ErrCode = -3;
                        System.Windows.Forms.MessageBox.Show("超出最小限位");
                        picoint = MinPoint;
                    }
                    if (!ErosConLink.StaticCon.SetLinkAddressValue(XSPAddress, (int)picoint * 10))
                    {
                        ErrCode--;
                    }
                    if (!ErosConLink.StaticCon.SetLinkAddressValue(XSetPAddress, true))
                    {
                        ErrCode--;
                    }
                    if (ErrCode != 1)
                    {
                        ErrCode = -1;
                    }
                    System.Threading.Thread.Sleep(200);
                    ErosConLink.StaticCon.SetLinkAddressValue(XSetPAddress, false);
                }
                catch (Exception)
                {
                }
            });
        }
        /// <summary>
        /// 设置速度
        /// </summary>
        /// <param name="sleep"></param>
        public void SetSleep(Single sleep)
        {
            try
            {
                ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(XSpeelAddress, sleep);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        ///开始同步原点
        /// </summary>
        public void SetHome()
        {
            IsHome = false;
            bool iscon = ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(XHomeAddress, true);
            if (iscon)
            {
                if (XISHomeAddress != null && XISHomeAddress != "")
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            bool home = ErosSocket.ErosConLink.StaticCon.GetLingkNameValue(XISHomeAddress);
                            while (!home)
                            {
                                home = ErosSocket.ErosConLink.StaticCon.GetLingkNameValue(XISHomeAddress);
                            }
                            IsHome = home;
                            return;
                        }
                        catch (Exception)
                        {
                        }
                        Alarm = true;
                    }
                  );
                }
                else
                {
                    Alarm = true;
                    IsHome = false;

                }
            }
        }
        /// <summary>
        /// 当前位置
        /// </summary>
        /// <returns></returns>
        public double GetPoint()
        {
            if (XPAddress.Contains('.'))
            {
                string[] teim = XPAddress.Split('.');
                if (ErosSocket.ErosConLink.StaticCon.GetSocketClint(teim[0]) == null)
                {
                    return 999999.999f;
                }
                if (!ErosSocket.ErosConLink.StaticCon.GetSocketClint(teim[0]).IsConn)
                {
                    return 99999.999f;
                }
            }
            Single d = 0f;
            try
            {
                d = Convert.ToSingle(ErosSocket.ErosConLink.StaticCon.GetLingkNameValue(XPAddress));
                Point = d / 10;
            }
            catch (Exception)
            {
            }
            return Point;
        }
        /// <summary>
        /// 复位故障
        /// </summary>
        public void SetReset()
        {
            this.Alarm = false;
            ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(XRsetAddress, true);
            System.Threading.Thread.Sleep(200);
            ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(XHomeAddress, false);
            ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(XRsetAddress, false);
            ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(XSetPAddress, false);
            if (XStopAddress != null)
            {
                ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(XStopAddress, false);
            }
        }

        public void Enabled()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 更新状态
        /// </summary>
        public void GetSt()
        {
            try
            {
                dynamic SD = false;
                this.enabled = Alarm = IsHome = false;
                if (this.XISHomeAddress != null && this.XISHomeAddress != "")
                {
                    SD = ErosConLink.StaticCon.GetLingkNameValue(this.XISHomeAddress);
                    if (SD == true)
                    {
                        IsHome = true;
                    }
                }
                if (this.XAralmAddress != null && this.XAralmAddress != "")
                {
                    SD = ErosConLink.StaticCon.GetLingkNameValue(this.XAralmAddress);
                    if (SD == true)
                    {
                        Alarm = true;
                    }
                }
                if (this.XEnbelnAddress != null && this.XEnbelnAddress != "")
                {
                    SD = ErosConLink.StaticCon.GetLingkNameValue(this.XEnbelnAddress);
                    if (SD == true)
                    {
                        this.enabled = true;
                    }
                }


            }
            catch (Exception)
            {


            }

        }
        public bool Stop()
        {
            try
            {
                if (XAddAddress != null)
                {
                    ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(XAddAddress, false);
                }
                if (X_Address != null)
                {
                    ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(X_Address, false);
                }
                return ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(XStopAddress, true);
            }
            catch (Exception)
            {
            }
            return false;
        }

        public void Dand_type_brake(bool isDeal)
        {

        }


        public bool SetPoint(double? p, double? sleep = null)
        {
            return false;
        }

        public void AddSeelp(double Dacc, double strVal, double MaxVal, double acc)
        {

        }

        public bool GetStatus(out bool enbeled, out bool is_home, out bool error, out bool band_type_brake)
        {
            is_home = error = band_type_brake = enbeled = false;



            return false;
        }

        public bool GetStatus()
        {
            return false;
        }

        public void AddSeelp()
        {
            throw new NotImplementedException();
        }
    }
}
