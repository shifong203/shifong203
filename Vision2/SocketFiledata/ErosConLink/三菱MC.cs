using HslCommunication;
using HslCommunication.Profinet.Melsec;
using System;
using System.Threading;

namespace ErosSocket.ErosConLink
{
    public class 三菱MC : ModbusTCPClint
    {
        //private HslCommunication.Profinet.Melsec.MelsecMcNet melsec_net;

        public 三菱MC(string ip, int Prot)
        {
            //melsec_net = new HslCommunication.Profinet.Melsec.MelsecMcNet(ip, Prot);
        }

        public 三菱MC()
        {
            this.Insocket = null;
            this.Recivebuffer = null;
            this.ReciveStr = null;
            this.modBusTcpClient = null;
        }

        //IReadWriteNet readWriteNet;

        private dynamic networkDeviceBase;

        /// <summary>
        ///
        /// </summary>
        /// <param name="isCycle"></param>
        /// <returns></returns>
        public override bool AsynLink(bool isCycle = true)
        {
            try
            {
                ErrBool = false;
                if (IsStataText)
                {
                    AddTextBox("链接:" + this.IP + "," + this.Port.ToString());
                }
                if (networkDeviceBase == null)
                {
                    if (linkType == "MC")
                    {
                        networkDeviceBase = new MelsecMcNet()
                        {
                            NetworkNumber = 0x00,
                            NetworkStationNumber = 0x00,
                            IpAddress = IP,
                            Port = this.Port,
                            ConnectTimeOut = 2000,
                            ReceiveTimeOut = 200
                        };
                    }
                    else if (linkType == "1E")
                    {
                        networkDeviceBase = new MelsecA1ENet()
                        {
                            IpAddress = IP,
                            Port = this.Port,
                            ConnectTimeOut = 2000,
                            ReceiveTimeOut = 200
                        };
                    }

                    //readWriteNet.IpAddress = this.IP.ToString();
                    //readWriteNet.Port = this.Port;
                    //readWriteNet.ConnectTimeOut = 2000; //网络连接的超时时间
                    //readWriteNet.ReceiveTimeOut = 2000;

                    //readWriteNet = melsec_net;
                    this.SendTimeMesag();
                }
                //networkDeviceBase = readWriteNet ;

                //melsec_net = new HslCommunication.Profinet.Melsec.MelsecA1ENet();
                this.SendBusy = false;
                //melsec_net = new HslCommunication.Profinet.Melsec.MelsecMcNet(this.OutIP.ToString(), this.OutPort);

                networkDeviceBase.ConnectClose();
                OperateResult connect = networkDeviceBase.ConnectServer();
                if (connect.IsSuccess)
                {
                    //net = melsec_net;
                    LinkState = "连接成功";
                    if (IsStataText)
                    {
                        AddTextBox(LinkState);
                    }
                    IsConn = true;
                    return true;
                }
                else if (isCycle)
                {
                    LinkState = "连接失败";
                    Thread.Sleep(this.LinkTime);
                    AsynLink(isCycle);
                }
                LinkState = "连接失败";
            }
            catch (Exception)
            {
            }
            return false;
        }

        private OperateResult<byte[]> read;

        /// <summary>
        ///
        /// </summary>
        /// <param name="district">地址头</param>
        /// <param name="address">地址</param>
        /// <param name="length">长度</param>
        /// <param name="errStr">错误代码状态</param>
        /// <returns>返回值</returns>
        public dynamic GetData(dynamic district, dynamic address, byte length, string type, out string errStr)
        {
            errStr = "";
            dynamic datas = false;
            try
            {
                if (type == "String")
                {
                    if (byte.TryParse(district, out byte res))
                    {
                        length = res;
                    }
                    else
                    {
                        length = 254;
                    }
                    district = "";
                }
                byte dset = length;
                if (type == "Double")
                {
                    dset = 4;
                }

                OperateResult<byte[]> readR = networkDeviceBase.Read(address.ToString(), dset);
                if (readR.IsSuccess)
                {
                    if (type == "Int16")
                    {
                        datas = new short[length];
                        for (int i = 0; i < length; i++)
                        {
                            datas[i] = networkDeviceBase.ByteTransform.TransInt16(readR.Content, i);
                        }
                    }
                    else if (type == "Boolean")
                    {
                        datas = new bool[length];
                        for (int i = 0; i < length; i++)
                        {
                            datas[i] = networkDeviceBase.ByteTransform.TransBool(readR.Content, i);
                        }
                    }
                    else if (type == "UInt16")
                    {
                        datas = new UInt16[length];
                        for (int i = 0; i < length; i++)
                        {
                            datas[i] = networkDeviceBase.ByteTransform.TransUInt16(readR.Content, i);
                        }
                    }
                    else if (type == "String")
                    {
                        datas = string.Empty;
                        datas = networkDeviceBase.ByteTransform.TransString(readR.Content, 0, readR.Content.Length, this.GetEncoding());
                    }
                    else if (type == "Int32")
                    {
                        datas = new Int32[length];
                        for (int i = 0; i < length / 2; i++)
                        {
                            datas[i] = networkDeviceBase.ByteTransform.TransInt32(readR.Content, i);
                        }
                    }
                    else if (type == "Single")
                    {
                        datas = new Single[length];
                        for (int i = 0; i < length - 1; i++)
                        {
                            datas[i] = networkDeviceBase.ByteTransform.TransSingle(readR.Content, i);
                        }
                    }
                    else if (type == "Double")
                    {
                        datas = new double[1];
                        datas[0] = networkDeviceBase.ByteTransform.TransDouble(readR.Content, 0);
                    }
                }
                else
                {
                    ErrBool = false;

                    if (readR.ErrorCode != 10000)
                    {
                        errStr += readR.ToMessageShowString();
                        this.LinkState = "连接断开";
                    }
                }
                if (length == 2)
                {
                    return datas[0];
                }
                return datas;
            }
            catch (Exception ex)
            {
            }
            errStr += "读取失败";
            return "";
        }

        /// <summary>
        /// 读取值
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool GetValue(UClass.ErosValues.ErosValueD item)
        {
            dynamic dynamic = this.GetData(item.District, item.AddressID, 2, item._Type, out string errStr);

            if (errStr == "")
            {
                item.Value = dynamic;
                return true;
            }
            return false;
        }

        public override bool SetValue(UClass.ErosValues.ErosValueD item, dynamic value, out string errStr)
        {
            errStr = "";
            OperateResult write = new OperateResult();
            byte length;
            if (byte.TryParse(item.District, out byte res))
            {
                length = res;
            }
            else
            {
                length = 254;
            }
            if (item._Type == "String")
            {
                write = networkDeviceBase.Write(item.AddressID.ToString(), value, length);
            }
            else
            {
                write = networkDeviceBase.Write(item.AddressID.ToString(), value);
            }
            this.SendBusy = false;
            if (!write.IsSuccess)
            {
                errStr += write.ToMessageShowString();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 对多个变量写入值，变量与值个数相等
        /// </summary>
        /// <param name="key">多个变量名</param>
        /// <param name="value">多个值</param>
        /// <param name="errStr">信息</param>
        /// <returns>是否成功</returns>
        public override bool SetValues(string[] key, string[] value, out string errStr)
        {
            errStr = "";
            if (key.Length == value.Length)
            {
                for (int i = 0; i < key.Length; i++)
                {
                    this.SetValue(key[i], value[i], out string sd);
                    errStr += sd;
                }
            }
            if (errStr == "")
            {
                return true;
            }
            else
            {
                return false;
            }

            //return base.SetValues(key, value, out errStr);
        }

        /// <summary>
        /// 对指定地址写入值
        /// </summary>
        /// <param name="id">地址</param>
        /// <param name="value">值</param>
        /// <param name="err">信息</param>
        /// <returns>是否成功</returns>
        public override bool SetIDValue(string id, dynamic value, out string err)
        {
            err = "";
            try
            {
                if (id.Contains(","))
                {
                  string[] dasts=      id.Split(',');
                    id = dasts[0];
       
                    if (UClass.GetTypeValue(dasts[1], value, out dynamic dynamic))
                    {
                        value = dynamic;
                    }
                }
                // 写入操作，这里的M100可以替换成I100,Q100,DB20.100效果时一样的
                OperateResult operateResult = networkDeviceBase.Write(id, value);                // 写位，注意M100.0等同于M100
                if (operateResult.IsSuccess)
                {
                    return true;
                }
                err = operateResult.Message;
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return false;
        }

        public override OperateResult<byte[]> GetAddressByte(string address, ushort length)
        {
            return networkDeviceBase.Read(address, length);
        }

        public override HslCommunication.Core.IByteTransform GetByteTransform(string address, ushort length, out OperateResult<byte[]> bytes)
        {
            OperateResult<byte[]> result = networkDeviceBase.Read(address, length);
            bytes = result;
            return networkDeviceBase.ByteTransform;
        }

        public override bool GetIDValue(string ID, string type, out dynamic value)
        {
            value = this.GetData("", ID, 2, type, out string errStr);
            if (errStr != "")
            {
                return false;
            }
            return true;
        }
    }
}