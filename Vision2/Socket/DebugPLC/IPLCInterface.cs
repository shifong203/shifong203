using ErosSocket.ErosConLink;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace ErosSocket.DebugPLC
{
    public interface IPLCInterface
    {
        string Name { get; set; }
        [DescriptionAttribute("。"), Category("显示"), DisplayName("true颜色")]
        Color True_Color { get; set; }
        [DescriptionAttribute("。"), Category("显示"), DisplayName("Fales颜色")]
        Color FalesColor { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("继续变量名")]
        string LinkConnectName { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("运行变量名")]
        string LinkStart { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("暂停变量名")]
        string LinkPause { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("停止变量名")]
        string LinkStop { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("初始化变量名")]
        string LinkInitialize { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备控制"), DisplayName("复位变量名")]
        string LinkRestoration { get; set; }
        string SetStatModeName { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("错误变量名")]
        string LinkAlarmName { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("暂停中变量名")]
        string LinkPauseName { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("运行中变量名")]
        string LinkRunName { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("初始化中变量名")]
        string LinkREName { get; set; }
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("初始化完成")]
        string LinkHomeDoneName { get; set; }
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("初始化超时S")]
        int HomeOutTime { get; set; }
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("读取设备状态地址名")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        string GetStatLinkName { get; set; }
        [DescriptionAttribute("。"), Category("设备状态"), DisplayName("设备运行ID")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameListValueNameUserControl1.Editor), typeof(UITypeEditor))]
        LinkNameListValueNameUserControl1.LinkNameListValueName ListNameRunID { get; set; }

        Dictionary<int, string> KeyIDStr { get; set; }

        string GetStatModeName { get; set; }
        /// <summary>
        /// 停机中
        /// </summary>
        bool Stoping { get; set; }
        /// <summary>
        /// 错误
        /// </summary>
        bool IsAlarm { get; set; }
        /// <summary>
        /// 调试中
        /// </summary>
        bool Debuging { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        [DescriptionAttribute("设备的运行状态。"), Category("设备状态"), DisplayName("设备状态"), Browsable(false)]
        EnumEquipmentStatus EquipmentStatus { get; set; }

        /// <summary>
        /// 停机
        /// </summary>
        /// <param name="isCoerce">True是强制停机</param>
        void Stop(bool isCoerce = false);

        /// <summary>
        /// 设备启动
        /// </summary>
        void Start();
        /// <summary>
        /// 暂停
        /// </summary>
        void Pause();

        void Rest();

        void Initialize();

    }
}
