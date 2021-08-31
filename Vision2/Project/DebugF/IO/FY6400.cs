using ErosSocket.DebugPLC.DIDO;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace Vision2.Project.DebugF.IO
{
    public class FY6400 : IDIDO
    {
        //函数体的声明
        [DllImport("FY6400.dll")]
        public static extern IntPtr FY6400_OpenDevice(Int32 Devnum);

        [DllImport("FY6400.DLL")]
        public static extern Int32 FY6400_CloseDevice(IntPtr hDevice);

        [DllImport("FY6400.DLL")]
        public static extern Int32 FY6400_DI(IntPtr hDevice);

        [DllImport("FY6400.DLL")]
        public static extern Int32 FY6400_DI_Bit(IntPtr hDevice, Int32 dich);

        [DllImport("FY6400.DLL")]
        public static extern Int32 FY6400_DO(IntPtr hDevice, Int32 dodata);

        [DllImport("FY6400.DLL")]
        public static extern Int32 FY6400_DO_Bit(IntPtr hDevice, Int32 dochdata, Int32 doch);

        [DllImport("FY6400.DLL")]
        public static extern Int32 FY6400_EEPROM_WR(IntPtr hDevice, Int32 wadr, Int32 wdata);

        [DllImport("FY6400.DLL")]
        public static extern Int32 FY6400_EEPROM_RD(IntPtr hDevice, Int32 radr);

        [DllImport("FY6400.DLL")]
        public static extern Int32 FY6400_CNT_Rest(IntPtr hDevice, Int32 cntch, Int32 cpol);

        [DllImport("FY6400.DLL")]
        public static extern Int32 FY6400_CNT_Read(IntPtr hDevice, Int32 cntch, Int32[] cdata);

        [DllImport("FY6400.DLL")]
        public static extern Int32 FY6400_RDO(IntPtr hDevice);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hDevice">doch：</param>
        /// <param name="doch">入口参数 ，DO 通道号。范围 0--15代表输出通道 0--输出通道 15。</param>
        /// <returns>出口参数，=0 代表此路 DO 为低；=1 代表此路 DO 为高；-1 代表函数错误。</returns>
        [DllImport("FY6400.DLL")]
        public static extern Int32 FY6400_RDO_Bit(IntPtr hDevice, Int32 doch);

        [DescriptionAttribute("板卡定义的槽位号"), Category("板卡信息"), DisplayName("板卡槽号")]
        public int ID { get; set; }

        public void Initial()
        {
            try
            {
                IsInitialBool = false;
                int dtet = FY6400_CloseDevice((IntPtr)ID);
                hDevice = FY6400_OpenDevice(ID);
                if (hDevice == (IntPtr)(-1))
                {
                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("FY6400板卡初始化失败", System.Drawing.Color.Red);
                }
                else
                {
                    Thread thread = new Thread(() =>
                    {
                        Thread.Sleep(150);
                        IsInitialBool = true;
                        while (IsInitialBool)
                        {
                            try
                            {
                                int de = FY6400_DI(hDevice);
                                for (short i = 0; i < 16; i++)
                                {
                                    Int[i] = ReadDI(i);
                                    Out[i] = ReadDO(i);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            Thread.Sleep(10);
                        }
                    });
                    thread.IsBackground = true;
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
            }
            catch (Exception)
            {
            }
        }

        private IntPtr hDevice;//句柄

        public bool IsInitialBool { get; private set; }
        public NameBool Int { get; set; }
        public NameBool Out { get; set; }

        public bool ReadDO(int number)
        {
            return Convert.ToBoolean(FY6400_RDO_Bit(hDevice, number));
        }

        public bool ReadDI(int number)
        {
            int d = FY6400_DI_Bit(hDevice, number);

            return Convert.ToBoolean(d);
        }

        private int errInt;

        public bool WritDO(int intex, int inde, bool value)
        {
            if (value)
            {
                errInt = FY6400_DO_Bit((IntPtr)intex, 1, inde);
            }
            else
            {
                errInt = FY6400_DO_Bit((IntPtr)intex, 0, inde);
            }

            if (errInt != 0)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("写入DO " + inde + "失败");
                return false;
            }
            return true;
        }

        public bool WritDO(int intex, bool value)
        {
            if (intex < 0)
            {
                return false;
            }
            if (DODIAxis.Debug)
            {
                DebugCompiler.Instance.DDAxis.Out[intex] = value;
                return true;
            }
            if (!this.IsInitialBool)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("未初始化成功,写入输出" + intex + "失败");
                return false;
            }

            if (value)
            {
                errInt = FY6400_DO_Bit(hDevice, 1, intex);
            }
            else
            {
                errInt = FY6400_DO_Bit(hDevice, 0, intex);
            }
            if (errInt != 0)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("写入DO " + intex + "失败");
                return false;
            }
            return true;
        }
    }
}