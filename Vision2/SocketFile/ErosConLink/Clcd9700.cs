using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ErosSocket.ErosConLink
{
    internal class Clcd9700 : SocketClint
    {
        public Clcd9700()
        {
            list.Add("类比", new Code() { Rcode = "01", Length = 80, Hex = 16, address = 0, Wcode = null });
            list.Add("日期", new Code() { Rcode = "03", Length = 28, Hex = 10, address = 70, Wcode = null });
            list.Add("状态", new Code() { Rcode = "51", Length = 22, Hex = 16, address = 88, Wcode = null });
            list.Add("运转设定", new Code() { Rcode = "22", Length = 31, Hex = 16, address = 100, Wcode = "12" });
            list.Add("定值设定", new Code() { Rcode = "25", Length = 37, Hex = 16, address = 121, Wcode = "15" });
            list.Add("操作设定", new Code() { Rcode = null, Length = 37, Hex = 16, address = 148, Wcode = "53", ReDatas = "001" });

            ByteDatas = ByteDatas.PadRight(1000, '0');
        }

        public string ByteDatas = string.Empty;

        public class Code
        {
            public string Rcode { get; set; }
            public string Wcode { get; set; }
            public string ReDatas { get; set; }
            public int Length;
            public int address;
            public int Hex;
        }

        private Dictionary<string, Code> list = new Dictionary<string, Code>();

        //头和代号：@01
        private const string headandCode = "403031";

        public override void EnabledRunCycleEvent()
        {
            Thread thread = new Thread(() =>
            {
                while (!this.CloseBool)
                {
                    try
                    {
                        OnCycleEvent("");
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(2000);
                    }
                    Thread.Sleep(100);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private bool[] barray = new bool[16];

        /// <summary>
        /// 更新变量表
        /// </summary>
        /// <returns></returns>
        public override bool GetValues()
        {
            try
            {
                watch.Restart();
                foreach (var item1 in list)
                {
                    if (this.LinkState != "连接成功" && this.LinkState != "数据错误")
                    {
                        this.AsynLink();
                        Thread.Sleep(2000);
                    }
                    if (this.LinkState == "连接成功" || this.LinkState == "数据错误")
                    {
                        while (SendBusy) ;
                        string det = this.RaedData(item1.Value);
                        if (det != null)
                        {
                            item1.Value.ReDatas = det;
                            ByteDatas.Remove(item1.Value.address, item1.Value.ReDatas.Length);
                            ByteDatas = ByteDatas.Insert(item1.Value.address, item1.Value.ReDatas);
                            var de =
                            from score in KeysValues.DictionaryValueD
                            where int.Parse(score.Value.AddressID) >= item1.Value.address
                            where int.Parse(score.Value.AddressID) < item1.Value.address + item1.Value.ReDatas.Length
                            select score;
                            foreach (var item in de)
                            {
                                string ds = "";
                                int dint;
                                if (item.Value._Type != "Boolean")
                                {
                                    try
                                    {
                                        ds = ByteDatas.Substring(int.Parse(item.Value.AddressID), ReturnTypeLength(item.Value._Type)).ToString();

                                        dint = Convert.ToInt16(ByteDatas.Substring(int.Parse(item.Value.AddressID), ReturnTypeLength(item.Value._Type)), item1.Value.Hex);
                                        item.Value.Value = dint;
                                        //item.Value.Value = UClass.DecimalShift(dint, item.Value.DecimalShift);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    //dint = Convert.ToInt32(ByteDatas.Substring((int)item.Value.AddressID, ReturnTypeLength(item.Value._Type)), item1.Value.Hex);
                                }
                                else
                                {
                                    int deee = Convert.ToInt32(ByteDatas.Substring(int.Parse(item.Value.AddressID), ReturnTypeLength(item.Value._Type)));
                                    int id = 0;
                                    if (item.Value.AddressID.ToString().Contains('.'))
                                    {
                                        id = Convert.ToInt16(item.Value.AddressID.ToString().Remove(0, item.Value.AddressID.ToString().IndexOf('.') + 1));
                                    }

                                    barray = StaticCon.ConvertIntToBoolArray(deee, 16);

                                    item.Value.Value = barray[id];
                                }
                            }
                        }
                    }
                    if (item1.Value.Rcode == null)
                    {
                        continue;
                    }
                }//循环遍历设备数据
                _DataTime = DateTime.Now;

                CDataTime = watch.ElapsedMilliseconds;
                watch.Reset();
                return true;
            }
            catch (Exception ex)
            {
                watch.Reset();
                return false;
            }
        }

        public string RaedData(Code Command)
        {
            if (Command.Rcode == null)
            {
                return null;
            }
            byte[] de = Encoding.ASCII.GetBytes(FCScheck("@01" + Command.Rcode) + "*\x0d\x0A");//
            if (Name == StaticCon.DebugID)
            {
            }
            this.Send(de);//读取类比资料
            Thread.Sleep(400);
            try
            {
                Insocket.ReceiveTimeout = 500;
                if (Recivebuffer == null)
                {
                    Recivebuffer = new byte[1024 * 1024 * 5];
                }
                ReciveBufferLenth = Insocket.Receive(Recivebuffer);
                if (base.ReciveBufferLenth == Command.Length)
                {
                    LinkState = "连接成功";
                    return Encoding.ASCII.GetString(Recivebuffer, 5, Command.Length - 10);
                }
            }
            catch (Exception ex)
            {
                LinkState = "数据错误";
            }
            return null;
        }

        public string RaedData(Code Command, string data)
        {
            try
            {
                string dew = "";

                for (int i = 0; i < 2; i++)
                {
                    byte[] de = Encoding.ASCII.GetBytes(FCScheck("@01" + Command.Wcode + data) + "*\x0d");
                    this.Send(de);//读取类比资料
                    Thread.Sleep(300);
                    Insocket.ReceiveTimeout = 100;
                    ReciveBufferLenth = Insocket.Receive(Recivebuffer);
                    dew = Encoding.ASCII.GetString(Recivebuffer, 5, ReciveBufferLenth);
                    if (dew.StartsWith("00"))
                    {
                        return dew;
                    }
                }
                return dew;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public override bool SetValue(string key, string value, out string err)
        {
            err = "";
            try
            {
                SendBusy = true;
                Thread.Sleep(200);
                if (this.KeysValues.DictionaryValueD.ContainsKey(key))
                {
                    if (this.KeysValues.DictionaryValueD[key].District != null)
                    {
                        float fid = int.Parse(KeysValues.DictionaryValueD[key].AddressID) - this.list[this.KeysValues.DictionaryValueD[key].District].address;
                        if (fid < 0) return false;
                        if (list[this.KeysValues.DictionaryValueD[key].District].ReDatas == null) return false;
                        string data = list[this.KeysValues.DictionaryValueD[key].District].ReDatas;
                        if (KeysValues.DictionaryValueD[key]._Type != "Boolean")
                        {
                            sbyte sbyteT = Convert.ToSByte(-Convert.ToInt16(Convert.ToSByte(this.KeysValues.DictionaryValueD[key].DecimalShift)));
                            value = Convert.ToString(UClass.DecimalShift(Convert.ToSingle(value), sbyteT));
                            Int16 d = Convert.ToInt16(value);
                            string det = d.ToString("x" + ReturnTypeLength(KeysValues.DictionaryValueD[key]._Type));

                            if (ReturnTypeLength(KeysValues.DictionaryValueD[key]._Type) >= det.Length)
                            {
                                data = data.Remove(
                                    (int)fid, ReturnTypeLength(KeysValues.DictionaryValueD[key]._Type));
                                data = data.Insert((int)fid, det);
                            }
                        }
                        else
                        {
                            int number = Convert.ToInt32(this.list[this.KeysValues.DictionaryValueD[key].District].ReDatas.Substring((UInt16)fid, 4));
                            int id = 0;
                            if (KeysValues.DictionaryValueD[key].AddressID.ToString().Contains('.'))
                            {
                                id = Convert.ToInt16(KeysValues.DictionaryValueD[key].AddressID.ToString().Remove(0, KeysValues.DictionaryValueD[key].AddressID.ToString().IndexOf('.') + 1));
                            }
                            bool[] barray = new bool[16];
                            barray = StaticCon.ConvertIntToBoolArray(number, 16);
                            barray[id] = Convert.ToBoolean(value);
                            number = StaticCon.ConvertBoolArrayToInt(barray);
                            data = data.Remove(
                                (int)fid, ReturnTypeLength(KeysValues.DictionaryValueD[key]._Type));
                            data.Insert((int)fid, number.ToString("x4"));
                        }
                        string dete = this.RaedData(list[this.KeysValues.DictionaryValueD[key].District], data);
                    }
                }
            }
            catch (Exception ex)
            {
                err += ex.Message;
            }
            return false;
        }

        public override bool SendDataSetValues(JObject obJson, out string err)
        {
            err = "";
            try
            {
                Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
                //组合对象
                SendBusy = true;

                foreach (var item in obJson)
                {
                    //string datas = this.list[this.KeysValues.DictionaryValueD[item.Key].District].ReDatas;
                    if (!this.KeysValues.DictionaryValueD.ContainsKey(item.Key))
                    {
                        err += "Err:字段<" + item.Key + ">不存在!";
                    }
                    else
                    {
                        //string dete = this.RaedData(this.list[this.KeysValues.DictionaryValueD[item.Key].District], datas);
                        //if (dete.StartsWith("02"))
                        //{
                        //    err += "写入失败，值超出范围；";
                        //}
                        if (!dictionary.ContainsKey(this.KeysValues.DictionaryValueD[item.Key].District))
                            dictionary.Add(this.KeysValues.DictionaryValueD[item.Key].District, new Dictionary<string, string>());//添加区域符
                        dictionary[this.KeysValues.DictionaryValueD[item.Key].District].Add(item.Key, item.Value.ToString());//值添加
                    }
                }
                SendBusy = true;
                foreach (var district in dictionary)
                {
                    if (this.list.ContainsKey(district.Key))
                    {
                        string datas = this.list[district.Key].ReDatas;
                        foreach (var item1 in district.Value)
                        {
                            if (KeysValues.DictionaryValueD[item1.Key]._Type != "Boolean")
                            {
                                Int16 d = Convert.ToInt16(item1.Value);
                                sbyte sbyteT = Convert.ToSByte(-Convert.ToInt16(Convert.ToSByte(this.KeysValues.DictionaryValueD[item1.Key].DecimalShift)));
                                dynamic value = Convert.ToString(UClass.DecimalShift(d, sbyteT));
                                d = Convert.ToInt16(value);

                                string det = d.ToString("x" + ReturnTypeLength(KeysValues.DictionaryValueD[item1.Key]._Type));

                                if (ReturnTypeLength(KeysValues.DictionaryValueD[item1.Key]._Type) >= det.Length)
                                {
                                    float id = int.Parse(KeysValues.DictionaryValueD[item1.Key].AddressID) - this.list[district.Key].address;
                                    datas = datas.Remove((int)id, ReturnTypeLength(KeysValues.DictionaryValueD[item1.Key]._Type));
                                    datas = datas.Insert((int)id, det);
                                }
                            }
                            else
                            {
                                float fid = int.Parse(KeysValues.DictionaryValueD[item1.Key].AddressID) - this.list[district.Key].address;
                                int deee = Convert.ToInt32(datas.Substring((UInt16)fid, 4));
                                int id = 0;
                                if (KeysValues.DictionaryValueD[item1.Key].AddressID.ToString().Contains('.'))
                                {
                                    id = Convert.ToInt16(KeysValues.DictionaryValueD[item1.Key].AddressID.ToString().Remove(0, KeysValues.DictionaryValueD[item1.Key].AddressID.ToString().IndexOf('.') + 1));
                                }
                                bool[] barray = new bool[16];
                                barray = StaticCon.ConvertIntToBoolArray(deee, 16);
                                barray[id] = Convert.ToBoolean(item1.Value);
                                deee = StaticCon.ConvertBoolArrayToInt(barray);
                                datas = datas.Remove(
                                    (int)fid, ReturnTypeLength(KeysValues.DictionaryValueD[item1.Key]._Type));
                                datas = datas.Insert((int)fid, deee.ToString("x4"));
                            }
                        }
                        string dete = this.RaedData(list[district.Key], datas);
                        if (dete.StartsWith("02"))
                        {
                            err += "写入失败，值超出范围；";
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            SendBusy = false;
            //SetValues(item.Key, item.Value.ToString(), out string err2);
            //err += err2;
            //SendDataGetValues(obJson, out string erre);
            return false;
        }

        /// <summary>
        /// 字符长度判断
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns></returns>
        public new static int ReturnTypeLength(string type)
        {
            int lenght = 0;
            try
            {
                switch (type)
                {
                    case "Byte":
                        lenght = 2;
                        break;

                    case "Double":
                        lenght = 8;
                        break;

                    case "Int32":
                        lenght = 8;
                        break;

                    case "String":
                        lenght = 8;
                        break;

                    case "Int16":
                    case "UInt16":
                        lenght = 4;
                        break;

                    case "Char":
                        lenght = 1;
                        break;

                    case "Boolean":
                        lenght = 4;
                        break;

                    default:
                        lenght = 1;
                        break;
                }
                return lenght;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}