using System;
using System.Threading;
using System.Windows.Forms;

namespace ErosSocket.ErosConLink
{
    /// <summary>
    /// ModbusTCPClient,客户端
    /// </summary>
    public class ModbusTCPClint : SocketClint
    {
        /// <summary>
        /// Modbus站地址
        /// </summary>
        public byte AddressID { get; set; }

        /// <summary>
        /// 实例
        /// </summary>
        public HslCommunication.ModBus.ModbusTcpNet modBusTcpClient;

        public ModbusTCPClint()
        {
            Split_Mode = SplitMode.KeyValue;
            Recivebuffer = null;
        }

        /// <summary>
        /// 带变量表更新的链接
        /// </summary>
        /// <param name="xmlName">表名称</param>
        public ModbusTCPClint(string xmlName) : this()
        {
            base.ValusName = xmlName;
        }

        public override void Receive()
        {
        }

        public override bool AsynLink(bool isCycle)
        {
            try
            {
                if (this.Name == StaticCon.DebugID && this.NetType != "modbusTCP")
                {
                }
                modBusTcpClient = new HslCommunication.ModBus.ModbusTcpNet(this.IP.ToString(), this.Port, this.AddressID);
                if (modBusTcpClient.ConnectServer().IsSuccess)
                {
                    this.LinkState = "连接成功";
                    return true;
                }
                else
                {
                    Thread.Sleep(3000);
                    this.LinkState = "连接失败";
                }
            }
            catch
            {
                this.LinkState = "连接失败";
                Thread.Sleep(3000);
            }
            return false;
        }

        public override bool GetValue(UClass.ErosValues.ErosValueD item)
        {
            bool retsbool = false;
            dynamic dynamics;
            try
            {
                dynamics = item.Value;
                if (item._Type == "Boolean")
                {
                    retsbool = this.SendData(1, UInt16.Parse(item.AddressID), 1, ref dynamics, out string errStr);
                }
                else
                {
                    retsbool = this.SendData(3, UInt16.Parse(item.AddressID), 1, ref dynamics, out string errStr);
                }
                if (retsbool)
                {
                    item.Value = dynamics;
                }
            }
            catch (Exception ex)
            {
                this.ErrerLog(ex);
            }
            return retsbool;
        }

        public override bool SetValue(UClass.ErosValues.ErosValueD item, dynamic value, out string errStr)
        {
            errStr = "";
            if (item._Type == "Boolean")
            {
                if (item.District != null && item.District == "M")
                {
                    this.SendData(5, UInt16.Parse(item.AddressID), 1, ref value, out errStr);
                }
                else
                {
                    this.SendData(6, UInt16.Parse(item.AddressID), 1, ref value, out errStr);
                }

                if (errStr == "")
                {
                    return true;
                }
            }
            else
            {
                this.SendData(10, UInt16.Parse(item.AddressID), 1, ref value, out errStr);
            }
            return false;
        }

        /// <summary>
        /// 链接到指定的ModbusServer
        /// </summary>
        /// <param name="inIPStr">IP</param>
        /// <param name="port">端口号</param>
        /// <param name="address">站号</param>
        /// <returns>成功返回ture</returns>
        public bool LinkModbusTCP(string inIPStr, UInt16 port, byte address)
        {
            try
            {
                modBusTcpClient = new HslCommunication.ModBus.ModbusTcpNet(inIPStr, port, address);

                if (modBusTcpClient.ConnectServer().IsSuccess)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.LinkState = "连接失败" + ex.Message.ToString();
            }
            return false;
        }

        /// <summary>
        /// 发送读写请求,功能码1读线圈，2读离散，3读寄存器，6写单个寄存器，10写多个寄存器,16写线圈数组
        /// <param name="functionCode">功能码</param>
        /// <param name="address">地址数组</param>
        /// <param name="length">命令长度</param>
        /// <param name="data">读取值或写入值</param>
        /// <param name="errStr">执行状态</param>
        /// <returns>返回成功或错误</returns>
        /// </summary>
        public bool SendData(dynamic functionCode, dynamic address, byte length, ref dynamic data, out string errStr)
        {
            errStr = "";
            try
            {  //写
                HslCommunication.OperateResult write = new HslCommunication.OperateResult();
                switch (functionCode)
                {
                    case 1:   // 读线圈，功能码0x01

                        bool[] dfe = modBusTcpClient.ReadCoil(address.ToString(), length).Content;
                        if (dfe != null)
                        {
                            string dats = "";
                            for (int i = 0; i < dfe.Length; i++)
                            {
                                dats += dfe[i].ToString();
                            }
                            if (length <= 1)
                            {
                                data = Convert.ToBoolean(dats);
                            }
                        }
                        else
                        {
                            errStr += "Err:读取失败";
                        }

                        break;

                    case 2:
                        // 读离散

                        dfe = modBusTcpClient.ReadDiscrete(address.ToString(), length).Content;
                        if (dfe != null)
                        {
                            string dats = "";
                            for (int i = 0; i < dfe.Length; i++)
                            {
                                dats += dfe[i].ToString();
                            }
                            if (length <= 1)
                            {
                                data = Convert.ToBoolean(dats);
                            }
                        }
                        else
                        {
                            errStr += "Err:读取失败";
                        }
                        break;

                    case 3:   // 读寄存器
                        HslCommunication.OperateResult<byte[]> read = modBusTcpClient.Read(address.ToString(), length);
                        if (read.IsSuccess)
                        {
                            data = modBusTcpClient.ByteTransform.TransInt16(read.Content, 0);
                        }
                        else
                        {
                            errStr += "Err," + read.ToMessageShowString();
                        }
                        break;

                    case 5: // 写单个线圈
                        write = modBusTcpClient.WriteCoil(address.ToString(), data);
                        if (!write.IsSuccess)
                            errStr += "Err," + write.ToMessageShowString();
                        break;

                    case 6: // 写单个离散

                        bool[] BSDF = new bool[] { data };
                        write = modBusTcpClient.WriteCoil(address.ToString(), BSDF);
                        if (!write.IsSuccess)
                            errStr += "Err," + write.ToMessageShowString();
                        break;

                    case 10://写多个寄存器

                        int.TryParse(data.ToString(), out int tryint);
                        write = modBusTcpClient.Write(address.ToString(), tryint);
                        if (!write.IsSuccess)
                            errStr += "Err," + write.ToMessageShowString();
                        break;

                    case 16:
                        //写线圈数组
                        write = modBusTcpClient.WriteCoil(address.ToString(), data);
                        if (!write.IsSuccess)
                            errStr += "Err," + write.ToMessageShowString();
                        break;

                    default:
                        errStr += "Err:没有的功能键" + functionCode;
                        break;
                }
                errStr = errStr.TrimEnd(',');
                if (errStr == "")
                {
                    if (this.LinkState != "连接成功") LinkState = "连接成功";

                    return true;
                }
            }
            catch (Exception ex)
            {
                errStr = "Err:代码错误：" + ex.Message.ToString();
            }

            return false;
        }

        /// <summary>
        /// 定义地址和长度
        /// </summary>
        /// <param name="address"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        private HslCommunication.OperateResult<ushort, ushort> GetAddressAndLength(string address, string Length)
        {
            HslCommunication.OperateResult<ushort, ushort> result = new HslCommunication.OperateResult<ushort, ushort>();
            try
            {
                result.Content1 = ushort.Parse(address);
                result.Content2 = ushort.Parse(Length);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 自定义参数的泛型
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="addressLength">地址长度</param>
        /// <returns></returns>
        private HslCommunication.OperateResult<ushort, ushort> GetAddressAndLength(ushort address, ushort addressLength)
        {
            HslCommunication.OperateResult<ushort, ushort> result = new HslCommunication.OperateResult<ushort, ushort>();
            try
            {
                result.Content1 = address;
                result.Content2 = addressLength;
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 将字节转换为16进制格式
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string MessageResultShow(HslCommunication.OperateResult<byte[]> result)
        {
            if (result.IsSuccess)
            {
                return HslCommunication.BasicFramework.SoftBasic.ByteToHexString(result.Content, ' ');
            }
            else
            {
                MessageBox.Show(result.ToMessageShowString());
                return "";
            }
        }

        /// <summary>
        /// 写入链接XML文件
        /// </summary>
        public void WriteXMLLink()
        {
        }

        /// <summary>
        /// 读取链接XML文件
        /// </summary>
        public void RaedXMLLink()
        {
        }

        public void OnEvent()
        {
        }
    }
}