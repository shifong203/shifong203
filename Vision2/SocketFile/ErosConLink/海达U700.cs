using System;
using System.Linq;
using System.Threading;

namespace ErosSocket.ErosConLink
{
    public class 海达U700 : SocketClint
    {
        public 海达U700()
        {
            Address = 1;
        }

        public byte Address;
        private int maxLentgh;
        private int linte;
        private int moe;
        private short[] dataBures;
        private short[] data;

        /// <summary>
        /// 首次链接
        /// </summary>
        public override void LinkSucceed()
        {
            base.LinkSucceed();

            Int16 max = 50;
            foreach (var item in KeysValues.DictionaryValueD.Values)
            {
                if (max < UInt16.Parse(item.AddressID)) max = Convert.ToInt16(item.AddressID);
            }
            maxLentgh = 100;
            linte = max / maxLentgh;
            moe = max % maxLentgh;
            dataBures = new short[max + 1];
        }

        public override bool GetValues()
        {
            try
            {
                string errStr = "";

                for (int i = 0; i < linte; i++)
                {
                    data = this.SendData(3, (UInt16)(i * maxLentgh), maxLentgh, null, out errStr);
                    if (data == null) return false;
                    data.CopyTo(dataBures, i * maxLentgh);
                }
                data = this.SendData(3, (UInt16)(linte * maxLentgh), moe, null, out errStr);
                if (data == null) return false;
                data.CopyTo(dataBures, linte * maxLentgh);

                foreach (var item in base.KeysValues.DictionaryValueD.Values)
                {
                    if (item._Type == "Boolean") ;
                    else
                    {
                        try
                        {
                            item.Value = Convert.ToInt16(dataBures[int.Parse(item.AddressID)]);
                        }
                        catch (Exception exe)
                        {
                            StaticCon.ErrerLog(exe);
                        }
                    }
                }
                _DataTime = DateTime.Now;

                return true;
            }
            catch (Exception EX)
            {
                StaticCon.ErrerLog(EX);
                return false;
            }
        }

        public override bool SetValue(string key, string value, out string err)
        {
            err = "";
            try
            {
                sbyte det = Convert.ToSByte(-(this.KeysValues.DictionaryValueD[key].DecimalShift));
                Single ds = UClass.DecimalShift(Convert.ToSingle(value), det);
                ReadDRegister(1, 6, int.Parse(KeysValues.DictionaryValueD[key].AddressID), Convert.ToInt16(value), out err);
                return true;
            }
            catch (Exception ex)
            {
                err += "键:" + key + ex.Message.ToString();
            }

            return false;
        }

        public override void Receive()
        {
            if (Insocket != null)
            {
                Insocket.ReceiveTimeout = 1500;
            }
        }

        /// <summary>
        /// 发送读写请求,功能码1读线圈，2读离散，3读寄存器，6写单个寄存器，10写多个寄存器,16写线圈数组
        /// <param name="functionCode">功能码</param>
        /// <param name="address">地址</param>
        /// <param name="length">命令长度</param>
        /// <param name="data">数据组</param>
        /// <param name="errStr">执行状态</param>
        /// <returns>返回值</returns>
        /// </summary>
        public dynamic SendData(byte functionCode, ushort address, int length, Int16[] values, out string errStr)
        {
            dynamic Builder = new System.Dynamic.ExpandoObject();
            errStr = "";
            try
            {
                bool[] dfe = new bool[16];
                switch (functionCode)
                {
                    case 1:   // 读线圈，功能码0x01

                        break;

                    case 2: //读离散

                        break;

                    case 3:   // 读寄存器

                        if (!ReadDRegister(Address, 3, address, length)) return null;
                        Int16[] de = new Int16[length];
                        Thread.Sleep(500);

                        Insocket.ReceiveTimeout = 800;
                        if (base.Recivebuffer == null)
                        {
                            base.Recivebuffer = new byte[1024 * 1024 * 5];
                        }
                        int r = Insocket.Receive(base.Recivebuffer);
                        string crc = Check.CRC.ToModbusCRC16(ByteToHexStr(base.Recivebuffer.Skip(0).Take(r - 2).ToArray()));
                        string crce = ByteToHexStr(base.Recivebuffer.Skip(r - 2).Take(2).ToArray());
                        if (crce != crc)
                        {
                            return null;
                        }
                        for (int i = 0; i < length; i++)
                        {
                            string dete = ByteToHexStr(base.Recivebuffer.Skip(3 + i * 2).Take(2).ToArray());
                            de[i] = Convert.ToInt16(dete, 16);
                        }
                        base.Recivebuffer = new byte[base.Recivebuffer.Length];
                        Builder = de;
                        break;

                    case 5: // 写单个线圈
                        break;

                    case 6: // 写单个寄存器测试
                        if (!ReadDRegister(Address, 6, address, length, values))
                        {
                        }
                        break;

                    case 10://写多个寄存器

                        break;

                    case 16:
                        //写线圈数组

                        break;

                    default:
                        errStr += "Err:没有的功能键" + functionCode;
                        break;
                }
                errStr = errStr.TrimEnd(',');
                if (this.LinkState != "连接成功") LinkState = "连接成功";
                return Builder;
            }
            catch (Exception ex)
            {
                LinkState = "连接断开";

                errStr = "Err:代码错误：" + ex.Message;
            }
            if (length <= 1)
            {
                return Builder[0];
            }
            else return Builder;
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="FunctionCode">指令代号</param>
        /// <param name="StartAdr">起始地址</param>
        /// <param name="Numb">读的个数</param>
        public bool ReadDRegister(byte address, byte FunctionCode, int StartAdr, int Count)
        {
            try
            {
                string CRCmodbus = Check.CRC.ToModbusCRC16(Address.ToString("x2") + FunctionCode.ToString("x2") + StartAdr.ToString("x4") + Count.ToString("x4"), true);
                byte[] bytes = StringHexToByte(address.ToString("x2") + FunctionCode.ToString("x2") + StartAdr.ToString("x4") + Count.ToString("x4"), 16);
                byte[] bytese = StringHexToByte(CRCmodbus, 16);
                byte[] db = new byte[bytes.Length + 2];
                bytes.CopyTo(db, 0);
                bytese.CopyTo(db, db.Length - 2);
            stat:
                while (SendBusy)
                {
                    Thread.Sleep(1000);
                    goto stat;
                }
                base.Send(db);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public bool ReadDRegister(byte address, byte FunctionCode, int StartAdr, int Count, Int16[] values)
        {
            try
            {
                string valuesStr = "";
                for (int i = 0; i < values.Length; i++)
                {
                    valuesStr += values[i].ToString("x4");
                }
                string CRCmodbus = Check.CRC.ToModbusCRC16(Address.ToString("x2") + FunctionCode.ToString("x2") + StartAdr.ToString("x4") + Count.ToString("x4") + valuesStr, true);
                byte[] bytes = StringHexToByte(address.ToString("x2") + FunctionCode.ToString("x2") + StartAdr.ToString("x4") + Count.ToString("x4") + valuesStr, 16);
                byte[] bytese = StringHexToByte(CRCmodbus, 16);
                byte[] db = new byte[bytes.Length + 2];
                bytes.CopyTo(db, 0);
                bytese.CopyTo(db, db.Length - 2);
                base.Send(db);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public bool ReadDRegister(byte address, byte FunctionCode, int StartAdr, Int16 values, out string err)
        {
            err = "";
            try
            {
                base.Recivebuffer = new byte[Recivebuffer.Length];
                string CRCmodbus = Check.CRC.ToModbusCRC16(Address.ToString("x2") + FunctionCode.ToString("x2") + StartAdr.ToString("x4") + values.ToString("x4"), true);
                byte[] bytes = StringHexToByte(address.ToString("x2") + FunctionCode.ToString("x2") + StartAdr.ToString("x4") + values.ToString("x4"), 16);
                byte[] bytese = StringHexToByte(CRCmodbus, 16);
                byte[] db = new byte[bytes.Length + 2];
                bytes.CopyTo(db, 0);
                bytese.CopyTo(db, db.Length - 2);
                base.Send(db);
                Insocket.ReceiveTimeout = 800;
                Thread.Sleep(500);
                int r = Insocket.Receive(base.Recivebuffer);
                if (r > 4)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                err = ex.Message.ToString();
            }
            return false;
        }
    }
}