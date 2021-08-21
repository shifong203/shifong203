using System;
using System.Text;

namespace ErosSocket.ErosConLink
{
    internal class Check
    {
        //FCS:异或方法
        public static string FCScheck(string data)
        {
            int head = Convert.ToInt32(data.Substring(0, 2), 16);
            for (int i = 1; i < data.Length / 2; i++)
            {
                head = head ^ Convert.ToInt32(data.Substring(i * 2, 2), 16);
            }
            string FCS = Str_to_ASCII(Convert.ToString(head, 16).ToString());
            return FCS;
        }

        /// <summary>
        /// SUM:从机器代号到起始地址所有ASCII相加的十六进制。取下位数
        /// </summary>
        /// <param name="data">十六进制的指令串</param>
        /// <returns>十六进制的SUMcheck</returns>
        public static string SUMcheck(string data)
        {
            int head = Convert.ToInt32(data.Substring(0, 2), 16);
            for (int i = 1; i < data.Length / 2; i++)
            {
                head = head + Convert.ToInt32(data.Substring(i * 2, 2), 16);
            }
            string SUM = (Str_to_ASCII(Convert.ToString(head, 16).ToString().PadLeft(4, '0'))).Substring(4, 4);
            return SUM;
        }

        /// <summary>
        /// CRC校验
        /// </summary>
        public class CRC
        {
            #region CRC16

            public static byte[] CRC16(byte[] data)
            {
                int len = data.Length;
                if (len > 0)
                {
                    ushort crc = 0xFFFF;
                    for (int i = 0; i < len; i++)
                    {
                        crc = (ushort)(crc ^ (data[i]));
                        for (int j = 0; j < 8; j++)
                        {
                            crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                        }
                    }
                    byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
                    byte lo = (byte)(crc & 0x00FF);         //低位置

                    return new byte[] { hi, lo };
                }
                return new byte[] { 0, 0 };
            }

            #endregion CRC16

            #region ToCRC16

            public static string ToCRC16(string content)
            {
                return ToCRC16(content, Encoding.UTF8);
            }

            public static string ToCRC16(string content, bool isReverse)
            {
                return ToCRC16(content, Encoding.UTF8, isReverse);
            }

            public static string ToCRC16(string content, Encoding encoding)
            {
                return ByteToString(CRC16(encoding.GetBytes(content)), true);
            }

            public static string ToCRC16(string content, Encoding encoding, bool isReverse)
            {
                return ByteToString(CRC16(encoding.GetBytes(content)), isReverse);
            }

            public static string ToCRC16(byte[] data)
            {
                return ByteToString(CRC16(data), true);
            }

            public static string ToCRC16(byte[] data, bool isReverse)
            {
                return ByteToString(CRC16(data), isReverse);
            }

            #endregion ToCRC16

            #region ToModbusCRC16

            public static string ToModbusCRC16(string s)
            {
                return ToModbusCRC16(s, true);
            }

            public static string ToModbusCRC16(string s, bool isReverse)
            {
                return ByteToString(CRC16(StringToHexByte(s)), isReverse);
            }

            public static string ToModbusCRC16(byte[] data)
            {
                return ToModbusCRC16(data, true);
            }

            public static string ToModbusCRC16(byte[] data, bool isReverse)
            {
                return ByteToString(CRC16(data), isReverse);
            }

            #endregion ToModbusCRC16

            #region ByteToString

            public static string ByteToString(byte[] arr, bool isReverse)
            {
                try
                {
                    byte hi = arr[0], lo = arr[1];
                    return Convert.ToString(isReverse ? hi + lo * 0x100 : hi * 0x100 + lo, 16).ToUpper().PadLeft(4, '0');
                }
                catch (Exception ex) { throw (ex); }
            }

            public static string ByteToString(byte[] arr)
            {
                try
                {
                    return ByteToString(arr, true);
                }
                catch (Exception ex) { throw (ex); }
            }

            #endregion ByteToString

            #region StringToHexString

            public static string StringToHexString(string str)
            {
                StringBuilder s = new StringBuilder();
                foreach (short c in str.ToCharArray())
                {
                    s.Append(c.ToString("X4"));
                }
                return s.ToString();
            }

            #endregion StringToHexString

            #region StringToHexByte

            private static string ConvertChinese(string str)
            {
                StringBuilder s = new StringBuilder();
                foreach (short c in str.ToCharArray())
                {
                    if (c <= 0 || c >= 127)
                    {
                        s.Append(c.ToString("X4"));
                    }
                    else
                    {
                        s.Append((char)c);
                    }
                }
                return s.ToString();
            }

            private static string FilterChinese(string str)
            {
                StringBuilder s = new StringBuilder();
                foreach (short c in str.ToCharArray())
                {
                    if (c > 0 && c < 127)
                    {
                        s.Append((char)c);
                    }
                }
                return s.ToString();
            }

            /// <summary>
            /// 字符串转16进制字符数组
            /// </summary>
            /// <param name="hex"></param>
            /// <returns></returns>
            public static byte[] StringToHexByte(string str)
            {
                return StringToHexByte(str, false);
            }

            /// <summary>
            /// 字符串转16进制字符数组
            /// </summary>
            /// <param name="str"></param>
            /// <param name="isFilterChinese">是否过滤掉中文字符</param>
            /// <returns></returns>
            public static byte[] StringToHexByte(string str, bool isFilterChinese)
            {
                string hex = isFilterChinese ? FilterChinese(str) : ConvertChinese(str);

                //清除所有空格
                hex = hex.Replace(" ", "");
                //若字符个数为奇数，补一个0
                hex += hex.Length % 2 != 0 ? "0" : "";

                byte[] result = new byte[hex.Length / 2];
                for (int i = 0, c = result.Length; i < c; i++)
                {
                    result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }
                return result;
            }

            #endregion StringToHexByte
        }

        /// <summary>
        /// 字符串转ASCII
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string Str_to_ASCII(string str)
        {
            string s2 = str;
            byte[] ba = Encoding.Default.GetBytes(s2);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in ba)
            {
                sb.Append(b.ToString("x"));
            }
            return sb.ToString();
        }
    }
}