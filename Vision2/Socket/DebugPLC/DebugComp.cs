using ErosSocket.DebugPLC.PLC;
using ErosSocket.DebugPLC.Robot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace ErosSocket.DebugPLC
{
    public class DebugComp : ProjectObj, ProjectNodet.IClickNodeProject
    {
        public override string FileName => "组合Debug";
        public override string Text { get; set; } = "组合";
        public override string SuffixName => ".组合调试";
        public override string ProjectTypeName => "组合调试";
        public override string Name => "组合";
        [DescriptionAttribute("在调试界面显示PLC调试或板卡"), Category("参数"), DisplayName("显示PLC")]
        public bool ISPLCDebug { get; set; }
        [DescriptionAttribute("是否显示PLC的标准值读取"), Category("参数"), DisplayName("是否显示读取值")]
        public bool ISPLCValues { get; set; }
        [DescriptionAttribute("是否离线模式"), Category("轴参数"), DisplayName("离线模式")]
        public bool IsOffline_model { get; set; }

        public bool Stop { get; set; }
        [DescriptionAttribute("是否单步执行"), Category("轴参数"), DisplayName("单步模式")]
        public bool Single_step_mode { get; set; }
        [DescriptionAttribute("。"), Category("托盘信息"), DisplayName("托盘ID判断")]
        public bool TrayID { get; set; }

        [DescriptionAttribute("。"), Category("托盘信息"), DisplayName("穴位ID判断")]
        public bool PalenID { get; set; }
        public Dictionary<string, PLCAxis> DicAxes { get; set; }
        public Dictionary<string, Cylinder> DicCylinder { get; set; }
        public List<AxisSPD> ListAxisP { get; set; }
        public List<EpsenRobot6> ListRobot { get; set; }
        public Dictionary<string, ErosConLink.UClass.PLCValue> DicPLCIO { get; set; } = new Dictionary<string, ErosConLink.UClass.PLCValue>();
        public Dictionary<string, AxisGrubXY.Points> DicPoints { get; set; } = new Dictionary<string, AxisGrubXY.Points>();

        //public Dictionary<string, DIDO.C154_AxisGrub> DicC154{ get; set; } = new Dictionary<string, DIDO.C154_AxisGrub>();
        public static IAxis GetAxis(string name, EnumAxisType enumAxisType)
        {
            try
            {
                if (StaticThis != null)
                {
                    if (StaticThis.ListAxisP == null)
                    {
                        return null;
                    }
                    for (int i = 0; i < StaticThis.ListAxisP.Count; i++)
                    {
                        if (StaticThis.ListAxisP[i].Name == name)
                        {
                            return StaticThis.ListAxisP[i].GetAxis(enumAxisType);
                        }
                    }
                }

            }
            catch (Exception ex)
            {


            }

            return null;
        }


        public DebugComp()
        {
            this.Information = "调试功能";
            //this.Name = "调试";
            StaticThis = this;
        }
        public override void initialization()
        {
            base.initialization();
        }
        /// <summary>
        /// 返回实例
        /// </summary>
        /// <returns></returns>
        public static DebugComp GetThis()
        {
            if (StaticThis.ListRobot == null)
            {
                StaticThis.ListRobot = new List<EpsenRobot6>();
            }
            if (StaticThis.DicCylinder == null)
            {
                StaticThis.DicCylinder = new Dictionary<string, Cylinder>();
            }

            Dictionary<string, Cylinder> itemc = new Dictionary<string, Cylinder>();
            foreach (var item in StaticThis.DicCylinder)
            {
                itemc.Add(item.Value.Name, item.Value);
            }
            StaticThis.DicCylinder = itemc;
            if (StaticThis.ListAxisP == null)
            {
                StaticThis.ListAxisP = new List<AxisSPD>();
            }
            //if (StaticThis.DicC154 == null)
            //{
            //    StaticThis.DicC154 = new Dictionary<string, DIDO.C154_AxisGrub>();
            //}
            if (StaticThis.DicAxes == null)
            {
                StaticThis.DicAxes = new Dictionary<string, PLCAxis>();
            }
            if (StaticThis == null)
            {
                StaticThis = new DebugComp();
            }
            Dictionary<string, PLCAxis> itmeA = new Dictionary<string, PLCAxis>();
            foreach (var item in StaticThis.DicAxes)
            {
                itmeA.Add(item.Value.Name, item.Value);
            }
            StaticThis.DicAxes = itmeA;
            Dictionary<string, ErosConLink.UClass.PLCValue> itmeP = new Dictionary<string, ErosConLink.UClass.PLCValue>();
            try
            {
                if (StaticThis.DicPLCIO == null)
                {
                    StaticThis.DicPLCIO = new Dictionary<string, ErosConLink.UClass.PLCValue>();
                }
                foreach (var item in StaticThis.DicPLCIO)
                {
                    itmeP.Add(item.Value.Name, item.Value);
                }
                StaticThis.DicPLCIO = itmeP;
            }
            catch (Exception)
            {
            }


            return StaticThis;
        }

        static DebugComp StaticThis;


        public Control GetThisControl()
        {
            return new CommandControl1(this);
        }

    }
}
