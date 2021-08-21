using System;

namespace ErosSocket.DebugPLC
{
    public interface IAxis
    {
        bool Initial();

        string Name { get; set; }
        double Scale { get; set; }
        EnumAxisType AxisType { get; set; }
        int HomeTime { get; set; }

        double Point { get; set; }

        Single Ratio { get; set; }
        int PlusLimit { get; set; }
        int MinusLimit { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        double MaxVel { get; set; }

        /// <summary>
        /// 步进距离
        /// </summary>
        double Jog_Distance { get; set; }

        bool IsHome { get; }

        bool IsError { get; set; }

        bool IsEnabled { get; set; }
        sbyte IsBand_type_brakeNumber { get; set; }

        /// <summary>
        /// 使能
        /// </summary>
        void Enabled();

        /// <summary>
        /// 复位
        /// </summary>
        void Reset();

        /// <summary>
        /// 回原点
        /// </summary>
        void SetHome();

        /// <summary>
        /// 停止
        /// </summary>
        bool Stop();

        /// <summary>
        /// 抱闸
        /// </summary>
        void Dand_type_brake(bool isDeal);

        /// <summary>
        /// 移动到位置
        /// </summary>
        /// <param name="p"></param>
        bool SetPoint(double? p, double? sleep = null);

        /// <summary>
        /// 设置加速度
        /// </summary>
        /// <param name="addSeelp"></param>
        void AddSeelp(double Dacc, double strVal, double MaxVal, double Tacc);

        void AddSeelp();

        /// <summary>
        /// 点动
        /// </summary>
        /// <param name="JogPsion"></param>
        /// <param name="jogmode"></param>
        /// <param name="seepJog"></param>
        void JogAdd(bool JogPsion, bool jogmode = true, double seepJog = 1.00F);

        bool GetStatus();
    }

    public enum EnumAxisType
    {
        X = 0,
        Y = 1,
        Z = 2,

        /// <summary>
        /// u旋转
        /// </summary>
        U = 3,

        /// <summary>
        /// X旋转
        /// </summary>
        V = 4,

        /// <summary>
        /// Y旋转
        /// </summary>
        w = 5,

        /// <summary>
        /// 速度轴，循环轴
        /// </summary>
        S = 6,

        /// <summary>
        /// 调宽，循环轴
        /// </summary>
        T = 7,

        /// <summary>
        /// uDD马达旋转
        /// </summary>
        Udd = 8,
    }
}