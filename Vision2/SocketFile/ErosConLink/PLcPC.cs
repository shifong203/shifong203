using Sharp7;
using System;
using System.Text;

namespace ErosSocket.ErosConLink
{
    public class PLcPC
    {
        S7Client client = new S7Client();
        public Boolean IsConnect { get; set; }

        public Boolean ConnectPLC(string ip)
        {
            int connectStatus = client.ConnectTo(ip, 0, 0);
            if (connectStatus == 0)
            {
                IsConnect = true;
                return true;
            }
            else
            {
                IsConnect = false;
                return false;
            }
        }

        public void WDBPlcbyte(int DBAddress, int offsetAddress, byte value)
        {
            byte[] wBuffer = new byte[1];
            byte[] rBuffer = new byte[1];
            int k;
            S7.SetByteAt(wBuffer, 0, value);
            do
            {
                lock (client)
                {
                    client.DBWrite(DBAddress, offsetAddress, 1, wBuffer);
                    client.DBRead(DBAddress, offsetAddress, 1, rBuffer);
                }
                k = S7.GetByteAt(rBuffer, 0);
            }
            while (k != value);
        }

        public int RDBPlcByte(int DBAddress, int offsetAddress, int value)
        {

            byte[] rBuffer = new byte[1];
            int k = 0;
            do
            {
                lock (client)
                {
                    client.DBRead(DBAddress, offsetAddress, 1, rBuffer);
                }
                k = S7.GetByteAt(rBuffer, 0);
            }
            while (k != value);
            return k;
        }
        /// <summary>
        /// 写float
        /// </summary>
        /// <param name="DBAddress"></param>
        /// <param name="offsetAdress"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int WPlcReal(int DBAddress, int offsetAdress, Single value)//写入real类型数值
        {
            byte[] wBuffer = new byte[4];
            byte[] rBuffer = new byte[4];
            int re = -1;
            float dou;
            do
            {
                S7.SetRealAt(wBuffer, 0, value);
                lock (client)
                {
                    re = client.DBWrite(DBAddress, offsetAdress, 4, wBuffer);
                    client.DBRead(DBAddress, offsetAdress, 4, rBuffer);
                }
                dou = S7.GetRealAt(rBuffer, 0);
            }
            while (dou != value);
            return re;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DBAddress"></param>
        /// <param name="offsetAddress"></param>
        /// <param name="DBbit"></param>
        /// <param name="value"></param>
        public void WDBPlcBitNoread(int DBAddress, int offsetAddress, int DBbit, bool value)
        {
            byte[] Wbuffer = new byte[1];
            byte[] rBuffer = new byte[1];
            if (value)
            {
                S7.SetBitAt(ref Wbuffer, 0, DBbit, true);
            }
            else
                S7.SetBitAt(ref Wbuffer, 0, DBbit, false);


            lock (client)
            {
                client.DBWrite(DBAddress, offsetAddress, 1, Wbuffer);
            }
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="DBAddress"></param>
        /// <param name="offsetAddress"></param>
        /// <returns></returns>
        public float RDBPlcReal(int DBAddress, int offsetAddress)
        {
            float f;
            byte[] rBuffer = new byte[4];
            lock (client)
            {
                client.DBRead(DBAddress, offsetAddress, 4, rBuffer);
                f = S7.GetRealAt(rBuffer, 0);
            }
            return f;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbaddress"></param>
        /// <param name="offsetAddress"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string RDBString(int dbaddress, int offsetAddress, int length)
        {
            string f;
            byte[] rBuffer = new byte[length];
            lock (client)
            {
                client.DBRead(dbaddress, offsetAddress, length, rBuffer);

                f = S7.GetStringAt(rBuffer, 0);
            }
            return f;

        }
        /// <summary>
        /// 根据地址写入值
        /// </summary>
        /// <param name="addressString"></param>
        /// <param name="type"></param>
        /// <param name="dynamic"></param>
        /// <returns></returns>
        public bool WretValue(string addressString, string type, dynamic dynamic)
        {
            string[] datitme = addressString.Split('.');
            string das = dynamic.GetType().Name;
            byte[] wBuffer = new byte[4];
            int retint = -10;
            int size = 4;
            int boolInx = 0;
            if (datitme.Length == 2)
            {
                boolInx = int.Parse(datitme[2]);
            }
            int mdNumbe = int.Parse(datitme[1]);
            int dBnber = 0;
            int AreaID = 0;
            switch (type)
            {
                case UClass.Single:
                    S7.SetRealAt(wBuffer, 0, dynamic);
                    size = 4;
                    break;
                case UClass.Boolean:
                    size = 2;
                    wBuffer = new byte[size];

                    if (addressString.StartsWith("DB"))
                    {
                        dBnber = int.Parse(datitme[0].Remove(0, 2));
                        AreaID = S7Consts.S7AreaDB;
                    }
                    else
                    {
                        AreaID = S7Consts.S7AreaMK;
                        mdNumbe = int.Parse(datitme[1]);
                    }
                    client.ReadArea(AreaID, dBnber, mdNumbe, size, S7Consts.S7WLByte, wBuffer);
                    S7.SetBitAt(ref wBuffer, 0, boolInx, dynamic);
                    break;
                case UClass.Byte:
                case UClass.SByte:
                    wBuffer = new byte[1];
                    size = 1;
                    S7.SetByteAt(wBuffer, 0, dynamic);
                    break;
                case UClass.Int16:
                    wBuffer = new byte[2];
                    size = 2;
                    S7.SetIntAt(wBuffer, 0, dynamic);
                    break;
                case UClass.UInt16:
                    wBuffer = new byte[2];
                    size = 2;
                    S7.SetUIntAt(wBuffer, 0, dynamic);
                    break;
                case UClass.UInt32:
                    S7.SetUDIntAt(wBuffer, 0, dynamic);
                    break;
                case UClass.Int32:
                    S7.SetDIntAt(wBuffer, 0, dynamic);
                    break;
                case UClass.Int64:
                    wBuffer = new byte[8];
                    size = 8;
                    S7.SetLIntAt(wBuffer, 0, dynamic);
                    break;
                case UClass.UInt64:
                    wBuffer = new byte[8];
                    size = 8;
                    S7.SetULintAt(wBuffer, 0, dynamic);
                    break;
                default:
                    if (type.StartsWith(UClass.String))
                    {
                        size = int.Parse(type.Remove(0, UClass.String.Length));
                        wBuffer = new byte[size];
                        S7.SetStringAt(wBuffer, 0, size, dynamic, Encoding.UTF8);
                    }
                    break;
            }
            lock (client)
            {
                wBuffer = new byte[4];
                S7.SetRealAt(wBuffer, 0, 0.01F);// DB39.DBD2

                int re = client.DBWrite(dBnber, 2, 4, wBuffer);

                client.WriteArea(AreaID, dBnber, 2, 4, S7Consts.S7WLReal, wBuffer);
                retint = client.WriteArea(AreaID, dBnber, mdNumbe, size, S7Consts.S7WLBit, wBuffer);
                if (retint == 0)
                {
                    return true;
                }
                else
                {
                    client.WriteArea(AreaID, dBnber, mdNumbe, size, S7Consts.S7WLByte, wBuffer);
                }
            }
            return false;
        }
    }
}
