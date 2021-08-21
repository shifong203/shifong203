using System;
using System.Linq;
using System.Text;

namespace ErosSocket.ErosConLink
{
    public class S7200PPI : SocketClint
    {
        public override bool GetValue(UClass.ErosValues.ErosValueD erosValueD)
        {
            string dstr = RaedData(erosValueD.AddressID);
            if (erosValueD._Type == "Boolean")
            {
                erosValueD.Value = Convert.ToBoolean(dstr);
            }
            else
            {
                erosValueD.Value = Convert.ToInt32(dstr);
            }

            return true;
        }

        /// <summary>
        /// 读取参数
        /// </summary>
        /// <param name="addressID">地址</param>
        /// <returns></returns>
        public string RaedData(string addressID)
        {
            try
            {
                this.Send(">" + addressID + "r");
                int r = this.Insocket.Receive(base.Recivebuffer);
                if (r > 2)
                {
                    //截取固定数据
                    return Encoding.UTF8.GetString(Recivebuffer.Skip(10).Take(r - 1).ToArray());
                }
            }
            catch (Exception)
            {
            }
            return "";
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key">名称</param>
        /// <param name="value">值</param>
        /// <param name="err"></param>
        /// <returns></returns>
        public override bool SetValue(string key, string value, out string err)
        {
            err = "";
            //先读数据
            string datavale = RaedData(this.KeysValues.DictionaryValueD[key].AddressID);
            //再写数据
            this.Send(">" + this.KeysValues.DictionaryValueD[key].AddressID + value + "r");
            int r = this.Insocket.Receive(base.Recivebuffer);
            ///判断是否写入成功
            if (r > 2)
            {
                string datas = Encoding.UTF8.GetString(Recivebuffer.Skip(10).Take(r - 1).ToArray());
                if (datas != "<00WD012E")
                {
                }
            }
            return false;
        }
    }
}