using ErosSocket.DebugPLC.DIDO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;

namespace ErosSocket.ErosConLink
{
    public class FY6400 : SocketClint, IDIDO
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

        public bool[] Di
        {
            get
            {
                return di;
            }
            private set
            {
                di = value;
            }
        }

        private bool[] di = new bool[16];

        public bool[] DO
        {
            get { return dob; }
            private set { dob = value; }
        }

        private bool[] dob = new bool[16];

        public string[] DO_Name { get; set; } = new string[16];
        public string[] DI_Name { get; set; } = new string[16];

        public bool IsInitialBool { get; private set; }

        public List<NameBool> Int { get; set; }

        public List<NameBool> Out { get; set; }
        NameBool IDIDO.Int { get; set; }
        NameBool IDIDO.Out { get; set; }

        private IntPtr hDevice;//句柄

        public FY6400()
        {
            KeysValues = new UClass.ErosValues();
            for (int i = 0; i < 16; i++)
            {
                KeysValues.DictionaryValueD.Add("DI" + i, new UClass.ErosValues.ErosValueD() { AddressID = "DI" + i.ToString(), _Type = UClass.Boolean });
            }
            for (int i = 0; i < 16; i++)
            {
                KeysValues.DictionaryValueD.Add("DO" + i, new UClass.ErosValues.ErosValueD() { AddressID = "DO" + i.ToString(), _Type = UClass.Boolean });
            }
        }

        public override void initialization()
        {
            hDevice = FY6400_OpenDevice(ID);
            if (hDevice == (IntPtr)(-1))
            {
                IsInitialBool = true;
                alarmStruct = new Vision2.ErosProjcetDLL.Project.AlarmText.alarmStruct();
                alarmStruct.Text = "板卡初始化失败";
                alarmStruct.Name = "FY6400";
                alarmStruct.AlaType = "致命报警";
                Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("FY6400板卡初始化失败", System.Drawing.Color.Red);
            }
            base.initialization();
        }

        private Vision2.ErosProjcetDLL.Project.AlarmText.alarmStruct alarmStruct = new Vision2.ErosProjcetDLL.Project.AlarmText.alarmStruct();

        public override bool Link(string ip, int port)
        {
            if (hDevice == (IntPtr)(-1))
            {
                return false;
            }
            return true;
        }

        public override void AsynConnect(bool isCycle, IPEndPoint ipe = null)
        {
            IsConn = true;
            this.LinkState = "连接成功";

            return;
        }

        public override bool GetValues()
        {
            foreach (var item in KeysValues.DictionaryValueD)
            {
                item.Value.Value = GetValue(item.Value);
            }
            return true;
        }

        public override bool GetValue(UClass.ErosValues.ErosValueD erosValueD)
        {
            string[] data = erosValueD.AddressID.Split('.');

            int inAdd = int.Parse(data[1]);
            if (hDevice == (IntPtr)(-1))
            {
                return false;
            }
            if (erosValueD.AddressID.StartsWith("DO"))
            {
                return Convert.ToBoolean(FY6400_RDO_Bit(hDevice, inAdd));
            }
            else
            {
                return Convert.ToBoolean(FY6400_DI_Bit(hDevice, inAdd));
            }
        }

        public override bool SetValue(string key, string value, out string err)
        {
            err = "";
            try
            {
                if (KeysValues.DictionaryValueD.ContainsKey(key))
                {
                    string[] data = KeysValues.DictionaryValueD[key].AddressID.Split('.');
                    int intPtr = int.Parse(data[0]);
                    int inAdd = int.Parse(data[1]);
                    bool valueBool = bool.Parse(value);
                    SetIDVa((IntPtr)intPtr, inAdd, valueBool);
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public override bool SetValue(UClass.ErosValues.ErosValueD item, dynamic value, out string errStr)
        {
            errStr = "";
            try
            {
                string[] data = item.AddressID.Split('.');
                int intPtr = int.Parse(data[0]);
                int inAdd = int.Parse(data[1]);
                bool valueBool = bool.Parse(value);
                SetIDVa((IntPtr)intPtr, inAdd, valueBool);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public override bool SetIDValue(string id, dynamic value, out string err)
        {
            err = "";
            try
            {
                string[] data = id.Split('.');
                int intPtr = int.Parse(data[0]);
                int inAdd = int.Parse(data[1]);
                bool valueBool = bool.Parse(value.ToString());
                SetIDVa((IntPtr)intPtr, inAdd, valueBool);
            }
            catch (Exception)
            {
            }
            return false;
        }

        public void SetIDVa(IntPtr id, int add, bool value)
        {
            if (value)
            {
                FY6400_DO_Bit(id, 1, add);
            }
            else
            {
                FY6400_DO_Bit(id, 1, add);
            }
        }

        public bool ReadDO(int number)
        {
            return Convert.ToBoolean(FY6400_RDO_Bit(hDevice, number));
        }

        public bool ReadDI(int number)
        {
            return Convert.ToBoolean(FY6400_DI_Bit(hDevice, number));
        }

        public bool WritDO(int intex, int inde, bool value)
        {
            if (value)
            {
                FY6400_DO_Bit((IntPtr)intex, 0, inde);
            }
            else
            {
                FY6400_DO_Bit((IntPtr)intex, 1, inde);
            }
            return false;
        }

        public bool WritDO(int intex, bool value)
        {
            if (!this.IsInitialBool)
            {
                Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("未初始化成功,写入输出" + intex + "失败");
                return false;
            }
            if (value)
            {
                FY6400_DO_Bit(hDevice, 0, intex);
            }
            else
            {
                FY6400_DO_Bit(hDevice, 1, intex);
            }
            return false;
        }
    }
}