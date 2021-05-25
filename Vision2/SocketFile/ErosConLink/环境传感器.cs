//using RSNetDevice;
//using RSNetDevice.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Forms;
//using ErosSocket.ErosConLink;

//namespace ErosSocket.ErosConLink
//{
//    public class 温湿度传感器
//    {
//        private RSServer rsServer;
//        public Dictionary<string, SocketClint> keys;

//        public 温湿度传感器()
//        {
//            try
//            {
//                rsServer = RSServer.Initiate("0.0.0.0", 5001);
//                if (rsServer.Start())
//                {
//                }
//                rsServer.OnReceiveRealtimeData += 环境传感器_PassiveEvent;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message + "湿度传感器");
//            }
//        }

//        private void 环境传感器_PassiveEvent(RSServer server, RealTimeData data)
//        {
//            for (int i = 0; i < data.NodeList.Count; i++)
//            {
//                var dicSort = from objDic in StaticCon.SocketClint
//                              where objDic.Value.Event.Contains(data.DeviceID.ToString())
//                              select objDic.Value.KeysValues;
//                foreach (var item in dicSort)
//                {
//                    if (!item.DictionaryValueD.ContainsKey("环境湿度"))
//                    {
//                        item.DictionaryValueD.Add("环境湿度", new UClass.ErosValues.ErosValueD
//                        { Name = "环境湿度", });
//                    }
//                    else item.DictionaryValueD["环境湿度"].Value = data.NodeList[i].Hum;
//                    if (!item.DictionaryValueD.ContainsKey("环境温度"))
//                    {
//                        item.DictionaryValueD.Add("环境温度", new UClass.ErosValues.ErosValueD
//                        { Name = "环境温度", Value = data.NodeList[i].Tem });
//                    }
//                    else item.DictionaryValueD["环境温度"].Value = data.NodeList[i].Tem;
//                }
//            }
//        }
//    }
//}