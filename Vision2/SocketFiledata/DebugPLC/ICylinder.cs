using System.ComponentModel;

namespace ErosSocket.DebugPLC
{
    public interface ICylinder
    {
        /// <summary>
        /// 伸出Q变量名
        /// </summary>
        ///
        [DescriptionAttribute("伸出气缸变量名。"), Category("控制"), DisplayName("伸出Q")]
        string ProtrudeQ { get; set; }

        string Name { get; set; }

        /// <summary>
        /// 缩回Q变量名
        /// </summary>
        [DescriptionAttribute("缩回气缸变量名。"), Category("控制"), DisplayName("缩回Q")]
        string AnastoleQ { get; set; }

        /// <summary>
        /// 伸出I变量名
        /// </summary>
        [DescriptionAttribute("伸出信号变量名。"), Category("控制"), DisplayName("伸出I")]
        string ProtrudeI { get; set; }

        /// <summary>
        /// 缩回I变量名
        /// </summary>
        [DescriptionAttribute("缩回信号变量名。"), Category("控制"), DisplayName("缩回I")]
        string AnastoleI { get; set; }

        /// <summary>
        /// 伸出M变量名
        /// </summary>
        [DescriptionAttribute("伸出信号变量名。"), Category("控制"), DisplayName("伸出M")]
        string ProtrudeM { get; set; }

        /// <summary>
        /// 缩回M变量名
        /// </summary>
        [DescriptionAttribute("缩回信号变量名。"), Category("控制"), DisplayName("缩回M")]
        string AnastoleM { get; set; }

        /// <summary>
        /// 气缸报警状态
        /// </summary>
        [DescriptionAttribute("缩回信号变量名。"), Category("控制"), DisplayName("气缸报警状态")]
        string CylinderAlram { get; set; }

        bool ISOne { get; set; }

        bool Protrude(bool isThre = true);

        bool Anastole(bool isThre = true);
    }
}