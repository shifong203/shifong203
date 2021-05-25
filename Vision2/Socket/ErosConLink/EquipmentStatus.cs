using System.ComponentModel;

namespace ErosSocket.ErosConLink
{
    public enum EnumEquipmentStatus
    {
        未初始化 = 0,
        已停止 = 1,
        运行中 = 2,
        暂停中 = 3,
        错误暂停中 = 4,
        错误停止中 = 5,
        未上电 = 6,
        初始化中 = 7,
        切换产品中 = 8,
        初始化完成 = 9,
        未上电状态 = 10,
    }
    public class EquipmentStatusClass
    {
        /// <summary>
        /// 设备状态
        /// </summary>
        [DescriptionAttribute("设备的运行状态。"), Category("状态"), DisplayName("设备状态"), Browsable(false)]
        public EnumEquipmentStatus EquipmentStatus { get; set; }


    }
}
