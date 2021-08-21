using HslCommunication;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ErosSocket.ErosConLink
{
    public class S71200 : ModbusTCPClint
    {
        private HslCommunication.Profinet.Siemens.SiemensS7Net siemensTcpNet;

        public S71200()
        {
            modBusTcpClient = null;
            Recivebuffer = null;
            Insocket = null;
        }

        //private HslCommunication.Profinet.Siemens.SiemensS7Net S7 {
        //    get {
        //        //if (S7c == null)
        //        //{
        //        //    S7c = new  HslCommunication.Profinet.Siemens.SiemensS7Net(HslCommunication.Profinet.Siemens.SiemensPLCS.S1200,this.IP);
        //        // }
        //        //OperateResult  operateResult = S7c.ConnectServer();
        //        //if (!operateResult.IsSuccess)
        //        //{
        //        //    S7c.ConnectClose();
        //        //    S7c = null;
        //        //    S7c = new HslCommunication.Profinet.Siemens.SiemensS7Net(HslCommunication.Profinet.Siemens.SiemensPLCS.S1200, this.IP);
        //        //}
        //        return siemensTcpNet;
        //    }
        //    set {
        //        value = siemensTcpNet;
        //    }
        //}
        public UClass.PLCDiave lCDiave = new UClass.PLCDiave();

        public override bool AsynLink(bool isCycle = true)
        {
            try
            {
                ErrBool = false;
                if (IsStataText)
                {
                    AddTextBox("链接:" + this.IP + "," + this.Port.ToString());
                }
                if (siemensTcpNet == null)
                {
                    siemensTcpNet = new HslCommunication.Profinet.Siemens.SiemensS7Net(HslCommunication.Profinet.Siemens.SiemensPLCS.S1200, this.IP.ToString()) { ConnectTimeOut = 1000 };
                }

                //SocketClint.EnumComputers();
                siemensTcpNet.Port = 102;
                OperateResult sdf = siemensTcpNet.ConnectServer();

                if (sdf.Message.Contains("Connect Failed : 无法访问已释放的对象。\r\n对象名:“System.Net.Sockets.Socket”。"))
                {
                    siemensTcpNet.ConnectClose();
                    siemensTcpNet = new HslCommunication.Profinet.Siemens.
                        SiemensS7Net(HslCommunication.Profinet.Siemens.SiemensPLCS.S1200,
                      this.IP.ToString())
                    { ConnectTimeOut = 1000 };
                }
                //ErrMesage = sdf.Message;
                Linking = false;
                this.IsConn = sdf.IsSuccess;
                if (sdf.IsSuccess)
                {
                    if (isCycle)
                    {
                        SendTimeMesag();
                    }
                    net = siemensTcpNet;
                    LinkState = "连接成功";
                    if (IsStataText)
                    {
                        AddTextBox(LinkState);
                    }
                    OnMastLink(sdf.IsSuccess);
                    return true;
                }
                else if (isCycle)
                {
                    LinkState = "连接失败";
                    OnMastLink(sdf.IsSuccess);
                    Thread.Sleep(this.LinkTime);
                    if (IsStataText)
                    {
                        AddTextBox(LinkState);
                    }
                    AsynLink(isCycle);
                }
                LinkState = "连接失败";
                //LinkState = sdf.Message;
                OnMastLink(sdf.IsSuccess);
                //Thread.Sleep(this.LinkTime);
                this.Linking = false;
            }
            catch (Exception)
            {
            }

            return false;
        }

        /// <summary>
        /// 根据类型返回地址长度
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public override byte GetTypeLength(string typeStr)
        {
            byte length = 1;
            switch (typeStr)
            {
                case "Decimal":
                case "UInt64":
                case "Int64":
                    length = 8;
                    break;

                case "Double":
                    length = 4;
                    break;

                case "Int16":
                case "UInt16":
                    length = 2;
                    break;

                case "Int32":
                case "UInt32":
                case "Single":
                    length = 4;
                    break;

                default:
                    length = 1;
                    break;
            }
            return length;
        }

        /// <summary>
        /// 根据类型返回地址长度
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public override string GetTypeLengthAddress(string typeStr, string address)
        {
            byte length = 1;
            switch (typeStr)
            {
                case "Decimal":
                case "UInt64":
                case "Int64":
                    length = 8;
                    break;

                case "Double":
                    length = 4;
                    break;

                case "Int16":
                case "UInt16":
                    length = 2;
                    break;

                case "Int32":
                case "UInt32":
                case "Single":
                    length = 4;
                    break;

                default:
                    length = 1;
                    break;
            }
            return address;
        }

        private UClass.ErosValues.ErosValueD[] erosValueDs;
        private UClass.ErosValues.ErosValueD[] erosValueDsT;
        private List<string> addStat;

        public override void OnMastLink(bool isConn)
        {
            addStat = new List<string>();
            foreach (var item in KeysValues.DictionaryValueD)
            {
                if (!addStat.Contains(item.Value.AddressID))
                {
                    if (!item.Value.AddressID.StartsWith("M"))
                    {
                        addStat.Add(item.Value.AddressID.Split('.')[0]);
                    }
                }
            }
            erosValueDs =
           SocketClint.GetAddres(KeysValues,
           "M", out int lengt);

            erosValueDsT = SocketClint.GetAddres(KeysValues,
         "DB", out lengt);
            base.OnMastLink(isConn);
        }

        public override bool GetValue(UClass.ErosValues.ErosValueD item)
        {
            object dynamics;
            try
            {
                if (item.WR == "W")
                {
                    return true;
                }
                dynamics = item.Value;
                while (this.SendBusy)
                {
                }
                switch (item._Type)
                {
                    case "Boolean":
                        if (item.AddressID.StartsWith("M"))
                        {
                            return true;
                        }

                        OperateResult<bool> OBOOL = siemensTcpNet.ReadBool(item.AddressID);
                        if (OBOOL.IsSuccess)
                        {
                            dynamics = OBOOL.Content;
                        }
                        else
                        {
                            ErrMesage = OBOOL.Message;
                            if (OBOOL.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "Byte":
                        OperateResult<byte> opacityConverter = siemensTcpNet.ReadByte(item.AddressID);
                        if (opacityConverter.IsSuccess)
                        {
                            dynamics = opacityConverter.Content;
                        }
                        else
                        {
                            ErrMesage = opacityConverter.Message;
                            if (opacityConverter.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "Int16":
                        OperateResult<short> sotrs = siemensTcpNet.ReadInt16(item.AddressID);
                        if (sotrs.IsSuccess)
                        {
                            dynamics = sotrs.Content;
                        }
                        else
                        {
                            ErrMesage = sotrs.Message;
                            if (sotrs.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "Int32":
                        OperateResult<int> operateINT32 = siemensTcpNet.ReadInt32(item.AddressID);
                        if (operateINT32.IsSuccess)
                        {
                            dynamics = operateINT32.Content;
                        }
                        else
                        {
                            ErrMesage = operateINT32.Message;
                            if (operateINT32.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "Int64":
                        OperateResult<Int64> operateINT64 = siemensTcpNet.ReadInt64(item.AddressID);
                        if (operateINT64.IsSuccess)
                        {
                            dynamics = operateINT64.Content;
                        }
                        else
                        {
                            ErrMesage = operateINT64.Message;
                            if (operateINT64.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "UInt16":
                        OperateResult<UInt16> operateUINT16 = siemensTcpNet.ReadUInt16(item.AddressID);
                        if (operateUINT16.IsSuccess)
                        {
                            dynamics = operateUINT16.Content;
                        }
                        else
                        {
                            ErrMesage = operateUINT16.Message;
                            if (operateUINT16.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "UInt32":
                        OperateResult<UInt32> operateUINT32 = siemensTcpNet.ReadUInt32(item.AddressID);
                        if (operateUINT32.IsSuccess)
                        {
                            dynamics = operateUINT32.Content;
                        }
                        else
                        {
                            ErrMesage = operateUINT32.Message;
                            if (operateUINT32.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "UInt64":
                        OperateResult<UInt64> operateUINT64 = siemensTcpNet.ReadUInt64(item.AddressID);
                        if (operateUINT64.IsSuccess)
                        {
                            dynamics = operateUINT64.Content;
                        }
                        else
                        {
                            ErrMesage = operateUINT64.Message;
                            if (operateUINT64.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "Double":

                        OperateResult<Double> operateDouble = siemensTcpNet.ReadDouble(item.AddressID);
                        if (operateDouble.IsSuccess)
                        {
                            dynamics = operateDouble.Content;
                        }
                        else
                        {
                            ErrMesage = operateDouble.Message;
                            if (operateDouble.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "Single":
                        OperateResult<float> operateFloat = siemensTcpNet.ReadFloat(item.AddressID);
                        if (operateFloat.IsSuccess)
                        {
                            dynamics = operateFloat.Content;
                        }
                        else
                        {
                            ErrMesage = operateFloat.Message;
                            if (operateFloat.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "String":
                        ushort leng = 254;
                        if (!ushort.TryParse(item.District, out leng))
                        {
                            leng = 254;
                        }

                        OperateResult<byte[]> operateString = siemensTcpNet.Read(item.AddressID, (ushort)(leng + 2));
                        if (operateString.IsSuccess)
                        {
                            dynamics = Sharp7.S7.GetStringAt(operateString.Content, 0, this.GetEncoding());
                        }
                        else
                        {
                            if (operateString.ErrorCode == 10000)
                            {
                                return false;
                            }
                            ErrBool = true;
                            ErrMesage = operateString.Message;
                            return true;
                        }
                        break;

                    default:
                        break;
                }
                if (dynamics != null)
                {
                    item.LinkID = this.Name;
                    item.Value = dynamics;
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                this.ErrMesage = ex.Message;
                this.ErrerLog(ex);
            }
            return false;
        }

        public void GetBypes()
        {
            try
            {
                if (erosValueDs != null && erosValueDs.Length != 0)
                {
                    double max = Convert.ToDouble(erosValueDs[erosValueDs.Length - 1].AddressID.Remove(0, 1));
                    double min = Convert.ToDouble(erosValueDs[0].AddressID.Remove(0, 1));
                    double mitn = max - min;
                    int d = (int)mitn;
                    if (d == 0)
                    {
                        d = 1;
                    }
                    d++;
                    try
                    {
                        OperateResult<byte[]> OBOOL = siemensTcpNet.Read(erosValueDs[0].AddressID, (ushort)(d));
                        if (Name == "皮带线站")
                        {
                        }
                        for (int i = 0; i < erosValueDs.Length; i++)
                        {
                            double t;
                            mitn = Convert.ToDouble(erosValueDs[i].AddressID.Remove(0, 1));
                            t = (mitn - min);
                            if (Name == "皮带线站" && erosValueDs[i].AddressID == "M203.0")
                            {
                            }
                            if (erosValueDs[i]._Type == UClass.Boolean)
                            {
                                dynamic dw = this.GetByteValue(OBOOL.Content, erosValueDs[i]._Type, (int)(t), UClass.GetTypeLentg(erosValueDs[i]._Type));
                                if (dw != null)
                                {
                                    string[] data = erosValueDs[i].AddressID.Split('.');
                                    int de = int.Parse(data[data.Length - 1]);
                                    erosValueDs[i].Value = dw[de];
                                    if (erosValueDs[i].Value)
                                    {
                                    }
                                }
                            }
                            else if (erosValueDs[i]._Type == UClass.String)
                            {
                                erosValueDs[i].Value = this.GetByteValue(OBOOL.Content, erosValueDs[i]._Type, (int)(t / 2), Convert.ToInt16(erosValueDs[i].District));
                            }
                            else
                            {
                                erosValueDs[i].Value = this.GetByteValue(OBOOL.Content, erosValueDs[i]._Type, (int)(t / 2), UClass.GetTypeLentg(erosValueDs[i]._Type));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                if (erosValueDsT != null)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        public override bool GetValues()
        {
            GetBypes();
            return base.GetValues();
        }

        private bool IDGetValue(string addressID, string type, string length, out dynamic value, out string err)
        {
            value = default(dynamic);
            err = "";
            while (this.SendBusy)
            {
            }
            try
            {
                if (!this.IsConn)
                {
                    return false;
                }
                switch (type)
                {
                    case "Boolean":
                        OperateResult<bool> OBOOL = siemensTcpNet.ReadBool(addressID);
                        if (OBOOL.IsSuccess)
                        {
                            value = OBOOL.Content;
                        }
                        else
                        {
                            err = OBOOL.Message;
                            if (OBOOL.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "Byte":
                        OperateResult<byte> opacityConverter = siemensTcpNet.ReadByte(addressID);
                        if (opacityConverter.IsSuccess)
                        {
                            value = opacityConverter.Content;
                        }
                        else
                        {
                            err = opacityConverter.Message;
                            if (opacityConverter.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "Int16":
                        OperateResult<short> sotrs = siemensTcpNet.ReadInt16(addressID);
                        if (sotrs.IsSuccess)
                        {
                            value = sotrs.Content;
                        }
                        else
                        {
                            ErrMesage = sotrs.Message;
                            if (sotrs.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "Int32":
                        OperateResult<int> operateINT32 = siemensTcpNet.ReadInt32(addressID);
                        if (operateINT32.IsSuccess)
                        {
                            value = operateINT32.Content;
                        }
                        else
                        {
                            err = operateINT32.Message;
                            if (operateINT32.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "Int64":
                        OperateResult<Int64> operateINT64 = siemensTcpNet.ReadInt64(addressID);
                        if (operateINT64.IsSuccess)
                        {
                            value = operateINT64.Content;
                        }
                        else
                        {
                            err = operateINT64.Message;
                            if (operateINT64.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "UInt16":
                        OperateResult<UInt16> operateUINT16 = siemensTcpNet.ReadUInt16(addressID);
                        if (operateUINT16.IsSuccess)
                        {
                            value = operateUINT16.Content;
                        }
                        else
                        {
                            err = operateUINT16.Message;
                            if (operateUINT16.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "UInt32":
                        OperateResult<UInt32> operateUINT32 = siemensTcpNet.ReadUInt32(addressID);
                        if (operateUINT32.IsSuccess)
                        {
                            value = operateUINT32.Content;
                        }
                        else
                        {
                            err = operateUINT32.Message;
                            if (operateUINT32.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "UInt64":
                        OperateResult<UInt64> operateUINT64 = siemensTcpNet.ReadUInt64(addressID);
                        if (operateUINT64.IsSuccess)
                        {
                            value = operateUINT64.Content;
                        }
                        else
                        {
                            err = operateUINT64.Message;
                            if (operateUINT64.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "Double":

                        OperateResult<Double> operateDouble = siemensTcpNet.ReadDouble(addressID);
                        if (operateDouble.IsSuccess)
                        {
                            value = operateDouble.Content;
                        }
                        else
                        {
                            err = operateDouble.Message;
                            if (operateDouble.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "Single":
                        OperateResult<float> operateFloat = siemensTcpNet.ReadFloat(addressID);
                        if (operateFloat.IsSuccess)
                        {
                            value = operateFloat.Content;
                        }
                        else
                        {
                            err = operateFloat.Message;
                            if (operateFloat.ErrorCode == 10000)
                            {
                                return false;
                            }
                            return true;
                        }
                        break;

                    case "String":
                        ushort leng = 254;
                        if (!ushort.TryParse(length, out leng))
                        {
                            leng = 254;
                        }
                        OperateResult<byte[]> operateString = siemensTcpNet.Read(addressID, (ushort)(leng + 2));
                        if (operateString.IsSuccess)
                        {
                            value = Sharp7.S7.GetStringAt(operateString.Content, 0, this.GetEncoding());
                        }
                        else
                        {
                            if (operateString.ErrorCode == 10000)
                            {
                                return false;
                            }
                            ErrBool = true;
                            ErrMesage = operateString.Message;
                            return true;
                        }
                        break;

                    default:
                        break;
                }
                if (value != null)
                {
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                this.ErrMesage = ex.Message;
                this.ErrerLog(ex);
            }
            return false;
        }

        public override bool SetValue(UClass.ErosValues.ErosValueD item, dynamic value, out string errStr)
        {
            errStr = "";
            SendBusy = true;
            int i = 0;
            try
            {
            strat:
                i++;
                OperateResult operateResult;
                if (item._Type == "String")
                {
                    int length = 254;
                    int.TryParse(item.District, out length);
                    if (length > 220)
                    {
                        length = 200;
                    }
                    byte[] vs = new byte[length];
                    Sharp7.S7.SetStringAt(vs, 0, length, value, GetEncoding());
                    operateResult = siemensTcpNet.Write(item.AddressID, vs);
                    //operateResult.ToMessageShowString();
                }
                else
                {
                    string id = item.AddressID.Trim();
                    operateResult = siemensTcpNet.Write(item.AddressID.Trim(), value);
                    //OperateResult operat = siemensTcpNet.ReadBool(item.AddressID.Trim());
                }
                if (i < 2)
                {
                    if (!operateResult.IsSuccess)
                    {
                        goto strat;
                    }
                }
                if (!operateResult.IsSuccess)
                {
                    this.LogErr("写入失败" + item.Name + item.AddressID + operateResult.Message);
                }
                else
                {
                    if (item.WR == "W")
                    {
                        item.Value = value;
                    }
                }
                SendBusy = false;
                return true;
            }
            catch (Exception ex)
            {
                errStr = ex.Message;
            }
            SendBusy = false;
            return false;
        }

        /// <summary>
        /// 写入多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="errStr"></param>
        /// <returns></returns>
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
        }

        /// <summary>
        /// 写入地址
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public override bool SetIDValue(string id, dynamic value, out string err)
        {
            err = "";
            try
            {
                SendBusy = true;
                if (id.Split('.').Length < 2)
                {
                    if (this.KeysValues.DictionaryValueD.ContainsKey(id))
                    {
                        id = this.KeysValues.DictionaryValueD[id].AddressID;
                    }
                }
                OperateResult operateResult;
                int i = 0;
            strat:
                i++;
                // 写入操作，这里的M100可以替换成I100,Q100,DB20.100效果时一样的
                operateResult = siemensTcpNet.Write(id, value);
                if (i < 2)
                {
                    if (!operateResult.IsSuccess)
                    {
                        goto strat;
                    }
                }
                SendBusy = false;
                if (operateResult.IsSuccess)
                {
                    return true;
                }
                else
                {
                    this.LogErr("写入失败" + id + operateResult.Message);
                }
                err = operateResult.Message;
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return false;
        }

        public override bool SetIDValue(string id, string type, string value, out string err)
        {
            return SetIDValue(id, Convert.ChangeType(value, UClass.GetTypeByString(type)), out err);
        }

        public override bool GetIDValue(string ID, out string value)
        {
            value = "";
            //IDGetValue(ID,)
            return false;
        }

        public override bool GetIDValue(string ID, string type, out dynamic value)
        {
            value = "";
            string id = ID;
            int leng = 0;

            if (type == "String")
            {
                string[] dat = id.Split('.');
                id = ID.Remove(ID.LastIndexOf('.'));
                string ds = ID.Remove(0, ID.LastIndexOf('.'));
                leng = Convert.ToInt16(ID.Remove(0, ID.LastIndexOf('.') + 1));
            }
            return IDGetValue(id, type, leng.ToString(), out value, out string Err);
        }

        public override bool GetValues(string Strkey, string typeName, ushort length, out dynamic values)
        {
            values = null;

            byte dt = UClass.GetTypeLentg(typeName);
            ushort intd = 0;

            if (typeName == UClass.Boolean)
            {
                intd = (ushort)(length / 8);
                if (length % 8 > 0)
                {
                    intd++;
                }
            }
            else if (typeName == UClass.String)
            {
                intd = (ushort)(dt * length + 2);
            }
            else
            {
                intd = (ushort)(dt * length);
            }
            OperateResult<byte[]> OBOOL = siemensTcpNet.Read(Strkey, intd);

            if (OBOOL.IsSuccess)
            {
                values = GetByteValue(OBOOL.Content, typeName, 0, length);
                if (values == null)
                {
                    return false;
                }
                return true;
            }
            else
            {
                this.ErrMesage = OBOOL.Message;
                if (OBOOL.ErrorCode == 10000)
                {
                    return false;
                }
                return true;
            }
        }

        private dynamic GetByteValue(byte[] buffre, string typeName, int str, int lengt)
        {
            dynamic value = null;
            switch (typeName)
            {
                case UClass.Boolean:
                    bool[] bytre = new bool[lengt];
                    value = GetIDValue().TransBool(buffre, str, lengt);
                    if (lengt > 8)
                    {
                        Array.Copy(value, 0, bytre, 0, lengt);
                        value = bytre;
                    }
                    break;

                case UClass.Int16:
                    value = GetIDValue().TransInt16(buffre, str, lengt);
                    break;

                case UClass.Single:
                    value = GetIDValue().TransSingle(buffre, str, lengt);
                    break;

                case UClass.Int32:
                    value = GetIDValue().TransInt32(buffre, str, lengt);
                    break;

                case UClass.Int64:
                    value = GetIDValue().TransInt64(buffre, str, lengt);
                    break;

                case UClass.Byte:
                    value = GetIDValue().TransByte(buffre, str, lengt);
                    break;

                case UClass.Double:
                    value = GetIDValue().TransDouble(buffre, str, lengt);
                    break;

                case UClass.UInt16:
                    value = GetIDValue().TransUInt16(buffre, str, lengt);
                    break;

                case UClass.UInt32:
                    value = GetIDValue().TransInt32(buffre, str, lengt);
                    break;

                case UClass.String:
                    value = Sharp7.S7.GetStringAt(buffre, str, this.GetEncoding());
                    break;

                default:
                    break;
            }
            return value;
        }

        public virtual HslCommunication.Core.IByteTransform GetIDValue()
        {
            return siemensTcpNet.ByteTransform;
        }

        public override OperateResult<byte[]> GetAddressByte(string address, ushort length)
        {
            return siemensTcpNet.Read(address, length);
        }

        public override HslCommunication.Core.IByteTransform GetByteTransform(string address, ushort length, out OperateResult<byte[]> bytes)
        {
            OperateResult<byte[]> result = siemensTcpNet.Read(address, length);
            bytes = result;
            return siemensTcpNet.ByteTransform;
        }

        /// <summary>
        /// 叠加地址
        /// </summary>
        /// <param name="address">起点地址</param>
        /// <param name="subid">叠加量，0.1：8进值，1</param>
        /// <returns></returns>
        public static string GetAddreassSub(string address, string subid)
        {
            string[] da = address.Split('.');
            if (address.StartsWith("DB"))
            {
                if (da.Length == 3)
                {
                    if (subid.Contains("."))
                    {
                        da[3] = (double.Parse(da[3]) + double.Parse(subid)).ToString();
                        if (double.Parse(da[3]) + double.Parse(subid) >= 7)
                        {
                            da[2] = (int.Parse(da[2]) + 1).ToString();
                            da[3] = "0";
                        }
                    }
                    else
                    {
                        da[2] = (double.Parse(da[2]) + double.Parse(subid)).ToString();
                        da[3] = "0";
                    }
                }
                else if (da.Length == 2)
                {
                    da[2] = (double.Parse(da[2]) + double.Parse(subid)).ToString();
                }
            }
            else
            {
                if (subid.Contains("."))
                {
                    da[2] = (double.Parse(da[2]) + double.Parse(subid)).ToString();
                    if (double.Parse(da[2]) + double.Parse(subid) >= 7)
                    {
                        da[2] = (int.Parse(da[2]) + 1).ToString();
                        //    da[2] = "0";
                    }
                }
                else
                {
                    da[2] = (double.Parse(da[2]) + double.Parse(subid)).ToString();
                    //   da[2] = "0";
                }
            }
            return address;
        }

        public override void Dispose()
        {
            base.Dispose();
            siemensTcpNet.ConnectClose();
        }
    }
}