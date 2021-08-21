using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;

namespace Vision2.SocketCFile.ErosConLink
{
    public class 三菱FX3U : RS232Con
    {
        public 三菱FX3U()
        {
            DataBits = 7;
        }

        public override void Receive()
        {
            base.Receive();
        }

        public override bool AsynLink(bool isCycle = true)
        {
            try
            {
                if (SerialPort.IsOpen)
                {
                    LinkState = "连接成功";
                    return true;
                }
                SerialPort = new SerialPort();
                SerialPort.Parity = Parity.Even;
                SerialPort.PortName = IP;
                SerialPort.BaudRate = 9600;
                SerialPort.DataBits = 7;
                SerialPort.Open();
                SerialPort.DataReceived += s_DataReceived;      //注册接收数据事件
                LinkState = "连接成功";
                return true;
            }
            catch (Exception ex)
            {
            }
            LinkState = "连接失败";
            return false;
        }

        private void s_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                ReciveBufferLenth = SerialPort.BytesToRead;                 //接收串口的数据数
                if (ReciveBufferLenth >= 1)
                {
                    Recivebuffer = new byte[ReciveBufferLenth];
                    SerialPort.Read(Recivebuffer, 0, ReciveBufferLenth);
                    //foreach (byte item in Recivebuffer)           //读取Buff中存的数据，转换成显示的十六进制数
                    //{
                    //    string str = "";
                    //    str += item.ToString("X2") + " ";
                    //}
                    //string   data = SerialPort.ReadExisting();
                    //if (IsAlramText)
                    //{
                    //    AddTextBox("(R):" + data, System.Drawing.Color.Green);
                    //}
                    //int b = data.Length;
                    //string[] a = new string[16];
                    // int[] num = new int[16];

                    //for (int i = 1; i < b-4; i = i + 4)
                    //{
                    //    int j = i / 4;
                    //    a[j] = data.Substring(i, 4);                       //D100,.......字符串解析
                    //    a[j] = a[j].Substring(2, 2) + a[j].Substring(0, 2);
                    //}
                    //for (int i = 0; i < 16; i++)
                    //{
                    //    num[i] = int.Parse(a[i], System.Globalization.NumberStyles.HexNumber);//十六进制转十进制
                    //}
                    OnEventArge(Recivebuffer.Skip(0).Take(ReciveBufferLenth).ToArray(), null);
                }
            }
            catch (Exception)
            {
            }
        }

        public override void OnEventArge(byte[] key, Socket socketR)
        {
            string str = "";
            try
            {
                foreach (byte item in key)           //读取Buff中存的数据，转换成显示的十六进制数
                {
                    str += item.ToString("X2") + " ";
                }
                if (key.Length < 50000)
                {
                    RecivesDone = true;
                    if (IsAlramText)
                    {
                        AddTextBox("(R):" + str, System.Drawing.Color.Green);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrerLog(ex);
            }
        }

        public override bool Send(string strdata)
        {
            string[] Data = strdata.Split(',');
            int[] Value = new int[Data.Length];
            string sa = "";
            Value[0] = Convert.ToInt16(Data[0]);
            for (int i = 1; i < Data.Length; i++)
            {
                Value[i] = Convert.ToInt16(Data[i]);
                sa += Value[i].ToString("X4").Substring(2, 2) + Value[i].ToString("X4").Substring(0, 2);
            }
            string s = PLC.WriteCommand(Value[0], sa.Length / 2, sa);
            return base.Send(s);
        }

        private class DataExchange
        {
            private List<char> hexStrings = new List<char>();

            public DataExchange()
            {
                hexStrings.Add('0'); hexStrings.Add('1');

                hexStrings.Add('2'); hexStrings.Add('3');

                hexStrings.Add('4'); hexStrings.Add('5');

                hexStrings.Add('6'); hexStrings.Add('7');

                hexStrings.Add('8'); hexStrings.Add('9');

                hexStrings.Add('A'); hexStrings.Add('B');

                hexStrings.Add('C'); hexStrings.Add('D');

                hexStrings.Add('E'); hexStrings.Add('F');
            }

            public enum ScrewResult
            {
                Pass = 1, Fail = 2, AllPassed = 4
            }

            private const int baseAddress = 4096;//1000HEX

            public int DStartAddress { get; set; }

            private byte stx = 2, etx = 3;

            private byte readCmd = 48;

            private byte writeCmd = 49;

            public string Dec2Hex(int decNum)
            {
                return decNum.ToString("X");
            }

            public byte[] String2Byte(string strValue)
            {
                return System.Text.Encoding.Default.GetBytes(strValue);
            }

            /// <summary>
            /// 读FX PLC的命令组合
            /// </summary>
            /// <param name="DAddress">起始的D地址位</param>
            /// <param name="readLength">读取的字节长度</param>
            /// <returns></returns>
            public string ReadCommand(int DAddress, int readLength)
            {
                byte[] readBytes = new byte[11]; //int i7 = 7 % 4; int i8 = 8% 4; int i9 = 9% 4;

                int address = DAddress * 2 + baseAddress;

                readBytes[0] = stx; readBytes[1] = readCmd; int index = 2;
                foreach (byte by in String2Byte(Dec2Hex(address)))
                {
                    readBytes[index] = by; index++;
                }
                foreach (byte by in String2Byte(Dec2Hex(readLength).PadLeft(2, '0')))
                {
                    readBytes[index] = by; index++;
                }
                readBytes[index] = etx; index++;
                int Sum = 0;
                for (int i = 1; i <= index; i++)
                {
                    Sum += readBytes[i];
                }
                byte[] sumBytes = String2Byte(Dec2Hex(Sum));
                readBytes[index] = sumBytes[sumBytes.Length - 2]; index++;
                readBytes[index] = sumBytes[sumBytes.Length - 1];
                string strCmd = "";

                foreach (byte by in readBytes)
                {
                    strCmd += ((char)by).ToString();
                }

                return strCmd;
            }

            /// <summary>
            /// 组合FX写入命令字符串
            /// </summary>
            /// <param name="StartAddress">开始地址位</param>
            /// <param name="writeLength">写入的字节长度</param>
            /// <param name="writeData">字符串长度必须为4的倍数</param>
            /// <returns>FX的写入命令</returns>
            public string WriteCommand(int StartAddress, int writeLength, string writeData)
            {
                writeData = writeData.PadLeft(4, '0');

                int length = 11 + writeData.Length;

                byte[] writeBytes = new byte[length];

                int address = StartAddress * 2 + baseAddress;

                writeBytes[0] = stx; writeBytes[1] = writeCmd; int index = 2;

                foreach (byte by in String2Byte(Dec2Hex(address)))
                {
                    writeBytes[index] = by; index++;
                }

                foreach (byte by in String2Byte(Dec2Hex(writeLength).PadLeft(2, '0')))
                {
                    writeBytes[index] = by; index++;
                }

                foreach (byte by in String2Byte(writeData))
                {
                    writeBytes[index] = by; index++;
                }

                writeBytes[index] = etx; index++;

                int Sum = 0;

                for (int i = 1; i <= index; i++)
                {
                    Sum += writeBytes[i];
                }

                byte[] sumBytes = String2Byte(Dec2Hex(Sum));

                writeBytes[index] = sumBytes[sumBytes.Length - 2]; index++;

                writeBytes[index] = sumBytes[sumBytes.Length - 1];

                string strCmd = "";

                foreach (byte by in writeBytes)
                {
                    strCmd += ((char)by).ToString();
                }

                return strCmd;
            }

            public bool PlcResponseWrite(string wResponseData)
            {
                return true;

                #region 格式判断

                ////if (string.IsNullOrEmpty(wResponseData))

                ////    return false;

                ////int stxIdx = wResponseData.IndexOf((char)2);

                ////int etxIdx = wResponseData.IndexOf((char)3);

                ////if (etxIdx < stxIdx)

                ////    etxIdx = wResponseData.LastIndexOf((char)3);

                ////if (etxIdx < stxIdx)

                ////    return false;

                ////string returnData = wResponseData.Substring(stxIdx + 1, etxIdx - stxIdx - 1);

                ////if (returnData.Length % 4 == 0)

                ////{
                ////    foreach (char ch in returnData.ToUpper().ToCharArray())

                ////    {
                ////        if (!this.hexStrings.Contains(ch))

                ////            return false;

                ////    }

                ////}

                ////else

                ////    return false;

                ////return true;

                #endregion 格式判断

                #region SUM Check

                ////byte[] bytes = System.Text.Encoding.Default.GetBytes(wResponseData);

                ////int sum = 0;

                ////if (bytes.Length < 3)

                ////    return false;

                ////for (int i = 1; i < bytes.Length - 2; i++)

                ////{
                ////    sum += (int)bytes[i];

                ////}

                ////string hexSum = Dec2Hex(sum);

                ////if (hexSum.Length > 2)

                ////    hexSum = hexSum.Substring(hexSum.Length - 2);

                ////string rtnSum = wResponseData.Substring(wResponseData.Length - 2);

                ////if (hexSum == rtnSum)

                ////    return true;

                ////return false;

                #endregion SUM Check
            }

            public List<string> ParseReadData(string readPlcData)
            {
                List<string> lstRtn = new List<string>();

                try
                {
                    if (!string.IsNullOrEmpty(readPlcData))
                    {
                        //string[] readSet = readPlcData.Replace((char)2, ' ').Trim().Split(new char[] { (char)3 });//v1.0.0.8

                        string[] readSet = readPlcData.Split(new char[] { (char)3 });//v1.0.0.9

                        string[] set2 = readSet[0].Split(new char[] { (char)2 });

                        if (set2.Length < 2)

                            return lstRtn;

                        //string returnData = readSet[0];//v1.0.0.8

                        string returnData = set2[1];//v1.0.0.9

                        if ((returnData.Length % 4) != 0)//v1.0.0.9

                            return lstRtn;//v1.0.0.9

                        string strTemp = "";

                        foreach (char ch in returnData.ToUpper().ToCharArray())//20190723 add v1.0.1.0
                        {
                            if (!this.hexStrings.Contains(ch))

                                strTemp += "0";
                            else

                                strTemp += ch.ToString();
                        }

                        returnData = strTemp;

                        for (int i = 0; i < returnData.Length; i = i + 4)
                        {
                            try
                            {
                                if (returnData.Length - i >= 4)
                                {
                                    string data = returnData.Substring(i, 4).Substring(2, 2) + returnData.Substring(i, 4).Substring(0, 2);

                                    lstRtn.Add(data);
                                }
                            }
                            catch { }
                        }
                    }
                }
                catch { }

                return lstRtn;
            }

            public int Hex2Int(string hexValue)
            {
                if (string.IsNullOrEmpty(hexValue))

                    return 0;

                foreach (char ch in hexValue.ToUpper().ToCharArray())
                {
                    if (!this.hexStrings.Contains(ch))

                        return 0;
                }

                try
                {
                    return int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
                }
                catch { return 0; }
            }
        }

        private DataExchange PLC = new DataExchange();
    }
}