using System;
using System.Threading;
using System.Threading.Tasks;

namespace ErosSocket.DebugPLC.DIDO
{
    public class C154_AxisGrub : Vision2.ErosProjcetDLL.Project.INodeNew
    {
        public bool Aralming { get; set; }

        public int ErrCode { get; set; }

        public string ErrMeaessge { get; set; }

        private bool Initial_Mark = false;
        private UInt16 CardID_InBit = 0;

        public static string ErrMasage(short code)
        {
            string mesagge = code.ToString() + ":";
            switch (code)
            {
                case 0:
                    return "";

                case -10000:
                    mesagge += "Error Card number{错误的卡号}";
                    break;

                case -10001:
                    mesagge += "Error operation system version{错误操作系统版本}";
                    break;

                case -10301:
                    mesagge += "Error card not found{错误卡片未找到}";
                    break;

                case -10302:
                    mesagge += "Error Open driver failed{打开驱动程序失败}";
                    break;

                case -10303:
                    mesagge += "Error ID mapping failed{错误ID映射失败}";
                    break;

                case -10304:
                    mesagge += "Error trigger channel{错误触发通道}";
                    break;

                case -10305:
                    mesagge += "Error trigger type{错误触发类型}";
                    break;

                case -10306:
                    mesagge += "Error event already enabled{错误事件已启用}";
                    break;

                case -10307:
                    mesagge += "Error event not enable yet{错误事件尚未启用}";
                    break;

                case -10319:
                    mesagge += "{轴已停止}";
                    break;

                default:
                    mesagge += "{未定义的错误类型}";
                    break;
            }
            return mesagge;
        }

        public static void ErrMesage(short code, string name)
        {
            string mesagge = code.ToString() + ":";
            switch (code)
            {
                case 0:
                    return;

                case -10000:
                    mesagge += "Error Card number{错误的卡号}";
                    break;

                case -10001:
                    mesagge += "Error operation system version{错误操作系统版本}";
                    break;

                case -10301:
                    mesagge += "Error card not found{错误卡片未找到}";
                    break;

                case -10302:
                    mesagge += "Error Open driver failed{打开驱动程序失败}";
                    break;

                case -10303:
                    mesagge += "Error ID mapping failed{错误ID映射失败}";
                    break;

                case -10304:
                    mesagge += "Error trigger channel{错误触发通道}";
                    break;

                case -10305:
                    mesagge += "Error trigger type{错误触发类型}";
                    break;

                case -10306:
                    mesagge += "Error event already enabled{错误事件已启用}";
                    break;

                case -10307:
                    mesagge += "Error event not enable yet{错误事件尚未启用}";
                    break;

                case -10319:
                    mesagge += "{轴已停止}";
                    break;

                default:
                    mesagge += "{未定义的错误类型}";
                    break;
            }
            Vision2.ErosProjcetDLL.Project.AlarmText.LogErr(mesagge, name);
            //throw new Exception(name + "执行错误");
        }

        public static short ReturnCode
        {
            get { return C154_AxisGrub.returnCode; }
            set
            {
                string mesagge = value.ToString() + ":";
                C154_AxisGrub.returnCode = value;
                if (value != 0)
                {
                    ErrMesage(value, "");
                }
            }
        }

        private static short returnCode;

        public void initial()
        {
            if (Initial_Mark == false)
            {
                CardID_InBit = 0;
                ReturnCode = MP_C154.c154_initial(ref CardID_InBit, 0);//初始化板卡
                if (ReturnCode != 0)
                {
                    //MessageBox.Show("板卡Initial Fail", "", MessageBoxButtons.OK);
                }
                else
                {
                    // MP_C154.c154_config_from_file();
                    c154Axis4.Initial();
                    c154Axis2.Initial();
                    c154Axis1.Initial();
                    c154Axis3.Initial();
                    Initial_Mark = true;
                    Task.Run(() =>
                    {
                        while (true)
                        {
                            GetStatus();
                            Thread.Sleep(5);
                        }
                    });
                }
            }
        }

        public C154Axis c154Axis1 { get; set; } = new C154Axis();
        public C154Axis c154Axis2 { get; set; } = new C154Axis();
        public C154Axis c154Axis3 { get; set; } = new C154Axis();
        public C154Axis c154Axis4 { get; set; } = new C154Axis();

        public bool[] IN = new bool[16];
        public bool[] OUT = new bool[16];

        public void GetStatus()
        {
            short DIvalue = 0;
            short DOvalue = 0;
            for (short i = 0; i < 16; i++)
            {
                ReturnCode = MP_C154.c154_get_gpio_input_ex_CH(0, i, ref DIvalue);
                ReturnCode = MP_C154.c154_get_gpio_output_ex_CH(0, i, ref DOvalue);
                IN[i] = DIvalue == 0 ? false : true;
                OUT[i] = DOvalue == 0 ? false : true;
            }
            c154Axis1.GetStatus();
            c154Axis2.GetStatus();
            c154Axis3.GetStatus();
            c154Axis4.GetStatus();
        }
    }
}