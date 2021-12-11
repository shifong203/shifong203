using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision2.Project.formula;
using static Vision2.ErosProjcetDLL.Project.AlarmText;

namespace Vision2.Project.DebugF
{
    /// <summary>
    /// 设备状态
    /// </summary>
    public   class Device_State
    {

        public void GetDeviceState()
        {
            UserName = ErosProjcetDLL.Project.ProjectINI.In.UserName;
            DeviceName = DebugCompiler.Instance.DeviceNameText;
            Product_Name = Product.ProductionName;
            mEquipmentStatus = DebugCompiler.EquipmentStatus;
            DicAlarm = ErosProjcetDLL.Project.AlarmText.DicAlarm;
            ProductNames = Product.GetThisP().Keys.ToList();
        }
        public EnumEquipmentStatus mEquipmentStatus { get; set; } = EnumEquipmentStatus.未初始化;

        public string Product_Name { get; set; } = "";

        public string UserName { get; set; } = "";

        public  Dictionary<string, alarmStruct> DicAlarm { get;  set; } = new Dictionary<string, alarmStruct>();

        public string DeviceName { get; set; } = "";

        public List<string> ProductNames { get; set; } = new List<string>();

        public OKNumberClass OKNumber = new OKNumberClass();


    }
}
