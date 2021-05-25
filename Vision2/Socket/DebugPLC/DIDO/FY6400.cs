using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC.DIDO
{
    public class FY6400 
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

        [DllImport("FY6400.DLL")]
        public static extern Int32 FY6400_RDO_Bit(IntPtr hDevice, Int32 doch);

        [DescriptionAttribute("板卡定义的槽位号"), Category("板卡信息"), DisplayName("板卡槽号")]
        public int ID { get; set; }

        public FY6400()
        {
        }

        public bool[] Di { get; private set; }

        public bool[] DO { get; private set; }

        public string[] DI_Name { get ; set ; }
        public string[] DO_Name { get ; set; }


    }
}
