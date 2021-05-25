using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ErosSocket.ErosConLink
{
    /// <summary>
    /// Json动态协议
    /// </summary>
    public static class JsonCon
    {
        /// <summary>
        /// 服务端接受Json协议事件
        /// </summary>
        /// <param name="ReciveJson"></param>
        /// <returns></returns>
        public static string ServerJson(string ReciveJson)
        {
            Newtonsoft.Json.Linq.JObject json = new Newtonsoft.Json.Linq.JObject();
            try
            {
                json = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(ReciveJson);
                if (json["Command"] == null || json["ObjectID"] == null)
                {
                    if (json["Status"] == null)
                    {
                        json.Add("Status", "Err:Json格式错误;");
                    }
                    else
                    {
                        json["Status"] = "Err:Json格式错误;";
                    }
                    return json.ToString();
                }
                if (json["Status"] == null) json.Add("Status", "");
                else { json["Status"] = ""; }
                switch (json["Command"].ToString().ToLower())
                {
                    case "get":
                        ServerCommandGet(json, StaticCon.SocketClint);
                        break;

                    case "set":
                        ServerCommandSet(json, StaticCon.SocketClint);
                        break;

                    case "getdic":
                        ServerCommandGet(json, StaticCon.SocketDic);
                        break;

                    case "setdic":
                        ServerCommandSet(json, StaticCon.SocketDic);
                        break;

                    case "settimetask":
                        ServerCommandSetTimeTask(json, StaticCon.SocketClint);
                        break;

                    case "gettimetask":
                        ServerCommandGetTimeTask(json, StaticCon.SocketClint);
                        break;

                    case "settaskstate":
                        ServerCommandSetTimeStask(json, StaticCon.SocketClint);
                        break;

                    case "gettaskstate":
                        ServerCommandGetTimeStask(json, StaticCon.SocketClint);

                        break;

                    default:
                        if (json["Command"].ToString().ToLower().StartsWith("get("))
                        {
                        }
                        else if (json["Command"].ToString().ToLower().StartsWith("set("))
                        {
                        }
                        else
                        {
                            json["Status"] = "Err:Comand命令" + json["Command"].ToString() + "不存在;";
                            return json.ToString();
                        }
                        break;
                }
                if (json["Status"].ToString().Length < 2)
                {
                    json["Status"] = "Done";
                }
            }
            catch (Exception ex)
            {
                json["Status"] += "Err:代码错误：" + ex.ToString();
            }
            return json.ToString();
        }

        /// <summary>
        /// 服务器端Set返回命令
        /// </summary>
        /// <param name="json"></param>
        public static void ServerCommandSet(Newtonsoft.Json.Linq.JObject json, Dictionary<string, SocketClint> socketDic)
        {
            if (json["ObjectID"].ToString() == "All")
            {
                json["Status"] += "Err:对象错误：" + json["ObjectID"].ToString() + ";";
                return;
            }
            string[] objectIDs = json["ObjectID"].ToString().Split(',');
            Newtonsoft.Json.Linq.JObject jo = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json["ObjectDatas"].ToString());    //字段对象
            for (int i = 0; i < objectIDs.Length; i++)
            {
                if (socketDic.ContainsKey(objectIDs[i]))
                {
                    if (socketDic[objectIDs[i]].LinkState == "连接成功")
                    {
                        socketDic[objectIDs[i]].SendDataSetValues(jo, out string ErrCode);
                        json["Status"] += ErrCode;
                    }
                    else
                    {
                        json["Status"] += "Err:" + objectIDs[i] + "连接断开;";
                    }
                }
                else
                {
                    json["Status"] += "Err:对象:" + objectIDs[i] + "不存在;";
                }
            }//遍历列表
            json["ObjectDatas"] = jo;
        }

        /// <summary>
        /// 服务端Get返回命令
        /// </summary>
        /// <param name="json"></param>
        public static void ServerCommandGet(Newtonsoft.Json.Linq.JObject json, Dictionary<string, SocketClint> socketDic)
        {
            Newtonsoft.Json.Linq.JObject jo = null;
            if (json["ObjectDatas"] != null)
                jo = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json["ObjectDatas"].ToString());    //字段对象
            string ErrCode = "";
            //执行Command
            string[] objectIDs = json["ObjectID"].ToString().Split(',');
            if (json["ObjectID"].ToString().ToLower() == "all")
            {
                jo = new Newtonsoft.Json.Linq.JObject();
                foreach (var item in socketDic)
                {
                    try
                    {
                        Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();
                        if (item.Value.LinkState == "连接成功")
                        {
                            item.Value.SendDataGetValues(ref jObject, out ErrCode);
                        }
                        else
                        {
                            ErrCode = "Err:" + item.Key + "连接断开;";
                            jObject.Add(item.Key, "Err:连接断开;");
                        }
                        if (ErrCode.Length > 1)
                        {
                            json["Status"] += ErrCode;
                        }
                        jo.Add(item.Key, jObject);
                    }
                    catch (Exception)
                    {
                    }
                }
            } //读全部
            else
            {
                for (int i = 0; i < objectIDs.Length; i++)
                {
                    if (StaticCon.SocketClint.ContainsKey(objectIDs[i]))
                    {
                        if (StaticCon.SocketClint[objectIDs[i]].LinkState == "连接成功")
                        {
                            StaticCon.SocketClint[objectIDs[i]].SendDataGetValues(ref jo, out ErrCode);
                        }
                        else
                        {
                            ErrCode += "Err:" + objectIDs[i] + "连接断开;";
                        }
                        json["Status"] += ErrCode;
                    }
                    else
                    {
                        json["Status"] += "Err:对象:" + objectIDs[i] + "不存在;";
                    }
                }//遍历列表
            }//读非全部对象
            json["ObjectDatas"] = jo;
        }

        /// <summary>
        /// 设置时间任务参数
        /// </summary>
        /// <param name="json"></param>
        /// <param name="socketDic"></param>
        public static void ServerCommandSetTimeTask(Newtonsoft.Json.Linq.JObject json, Dictionary<string, SocketClint> socketDic)
        {
            if (json["ObjectID"].ToString() == "All")
            {
                json["Status"] += "Err:对象错误：" + json["ObjectID"].ToString() + ";";
                return;
            }
            string[] objectIDs = json["ObjectID"].ToString().Split(',');
            Newtonsoft.Json.Linq.JObject jo = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json["ObjectDatas"].ToString());    //字段对象
            for (int i = 0; i < objectIDs.Length; i++)
            {
                if (socketDic.ContainsKey(objectIDs[i]))
                {
                    if (socketDic[objectIDs[i]].LinkState == "连接成功")
                    {
                        socketDic[objectIDs[i]].SendDataSetTimeValues(jo, out string ErrCode);
                        json["Status"] += ErrCode;
                    }
                    else
                    {
                        json["Status"] += "Err:" + objectIDs[i] + "连接断开;";
                    }
                }
                else
                {
                    json["Status"] += "Err:对象:" + objectIDs[i] + "不存在;";
                }
            }//遍历列表
            json["ObjectDatas"] = jo;
        }

        /// <summary>
        /// 读取时间任务参数
        /// </summary>
        /// <param name="json"></param>
        /// <param name="socketDic"></param>
        public static void ServerCommandGetTimeTask(Newtonsoft.Json.Linq.JObject json, Dictionary<string, SocketClint> socketDic)
        {
            if (json["ObjectID"].ToString() == "All")
            {
                json["Status"] += "Err:对象错误：" + json["ObjectID"].ToString() + ";";
                return;
            }
            string[] objectIDs = json["ObjectID"].ToString().Split(',');

            for (int i = 0; i < objectIDs.Length; i++)
            {
                if (socketDic.ContainsKey(objectIDs[i]))
                {
                    if (socketDic[objectIDs[i]].LinkState == "连接成功")
                    {
                        socketDic[objectIDs[i]].SendDataGetTimeValues(json, out string ErrCode);
                        json["Status"] += ErrCode;
                    }
                    else
                    {
                        json["Status"] += "Err:" + objectIDs[i] + "连接断开;";
                    }
                }
                else
                {
                    json["Status"] += "Err:对象:" + objectIDs[i] + "不存在;";
                }
            }//遍历列表
            //json["ObjectDatas"] = jo;
        }

        /// <summary>
        /// 设置时间任务状态
        /// </summary>
        /// <param name="json"></param>
        /// <param name="socketDic"></param>
        public static void ServerCommandSetTimeStask(Newtonsoft.Json.Linq.JObject json, Dictionary<string, SocketClint> socketDic)
        {
            if (json["ObjectID"].ToString() == "All")
            {
                json["Status"] += "Err:对象错误：" + json["ObjectID"].ToString() + ";";
                return;
            }
            string[] objectIDs = json["ObjectID"].ToString().Split(',');
            for (int i = 0; i < objectIDs.Length; i++)
            {
                if (socketDic.ContainsKey(objectIDs[i]))
                {
                    if (socketDic[objectIDs[i]].LinkState == "连接成功")
                    {
                        socketDic[objectIDs[i]].SetTimeTask(json["ObjectDatas"].ToString(), out string ErrCode);
                        json["Status"] += ErrCode;
                    }
                    else
                    {
                        json["Status"] += "Err:" + objectIDs[i] + "连接断开;";
                    }
                }
                else
                {
                    json["Status"] += "Err:对象:" + objectIDs[i] + "不存在;";
                }
            }//遍历列表
            //json["ObjectDatas"] = jo;
        }

        /// <summary>
        /// 读取任务状态
        /// </summary>
        /// <param name="json"></param>
        /// <param name="socketDic"></param>
        public static void ServerCommandGetTimeStask(Newtonsoft.Json.Linq.JObject json, Dictionary<string, SocketClint> socketDic)
        {
            if (json["ObjectID"].ToString() == "All")
            {
                json["Status"] += "Err:对象错误：" + json["ObjectID"].ToString() + ";";
                return;
            }
            string[] objectIDs = json["ObjectID"].ToString().Split(',');

            for (int i = 0; i < objectIDs.Length; i++)
            {
                if (socketDic.ContainsKey(objectIDs[i]))
                {
                    if (socketDic[objectIDs[i]].LinkState == "连接成功")
                    {
                        if (json.ContainsKey("ObjectDatas"))
                        {
                            json["ObjectDatas"] = socketDic[objectIDs[i]].GetTimeTask();
                        }
                        else
                        {
                            json.Add("ObjectDatas", socketDic[objectIDs[i]].GetTimeTask());
                        }
                    }
                    else
                    {
                        json["Status"] += "Err:" + objectIDs[i] + "连接断开;";
                    }
                }
                else
                {
                    json["Status"] += "Err:对象:" + objectIDs[i] + "不存在;";
                }
            }//遍历列表
        }

        ///// <summary>
        ///// 服务端Off指令
        ///// </summary>
        ///// <param name="json"></param>
        //public static void ServerCommandGets(Newtonsoft.Json.Linq.JObject json)
        //{
        //    string ErrCode = "";
        //    //执行Command
        //    string[] objectIDs = json["ObjectID"].ToString().Split(',');
        //    if (json["ObjectID"].ToString().ToLower() == "all")
        //    {
        //        foreach (var item in StaticCon.SocketClint)
        //        {
        //            try
        //            {
        //                item.Value.RetOFF(1 , out ErrCode);

        //                if (ErrCode.Length > 1)
        //                {
        //                    json["Status"] += ErrCode;
        //                }
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }
        //    } //全部
        //    else
        //    {
        //        for (int i = 0; i < objectIDs.Length; i++)
        //        {
        //            if (StaticCon.SocketClint.ContainsKey(objectIDs[i]))
        //            {
        //                StaticCon.SocketClint[objectIDs[i]].RetOFF(1, out ErrCode);
        //                json["Status"] += ErrCode;
        //            }
        //            else
        //            {
        //                json["Status"] += "Err:对象:" + objectIDs[i] + "不存在;";
        //            }
        //        }//遍历列表
        //    }//非全部对象
        //}
        ///// <summary>
        ///// 服务端On指令
        ///// </summary>
        ///// <param name="json"></param>
        //public static void ServerCommandOn(Newtonsoft.Json.Linq.JObject json)
        //{
        //    string ErrCode = "";
        //    //执行Command
        //    string[] objectIDs = json["ObjectID"].ToString().Split(',');
        //    if (json["ObjectID"].ToString().ToLower() == "all")
        //    {
        //        foreach (var item in StaticCon.SocketClint)
        //        {
        //            try
        //            {
        //                item.Value.SetOn(1, out ErrCode);

        //                if (ErrCode.Length > 1)
        //                {
        //                    json["Status"] += ErrCode;
        //                }
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }
        //    } //全部
        //    else
        //    {
        //        for (int i = 0; i < objectIDs.Length; i++)
        //        {
        //            if (StaticCon.SocketClint.ContainsKey(objectIDs[i]))
        //            {
        //                StaticCon.SocketClint[objectIDs[i]].SetOn(1, out ErrCode);
        //                json["Status"] += ErrCode;
        //            }
        //            else
        //            {
        //                json["Status"] += "Err:对象:" + objectIDs[i] + "不存在;";
        //            }
        //        }//遍历列表
        //    }//非全部对象
        //}
        /// <summary>
        /// 组合读取对象数据返回Json对象
        /// </summary>
        /// <typeparam name="T">数据类</typeparam>
        /// <param name="ObjectID">对象ID</param>
        /// <param name="ObjectDatas">对象数据</param>
        /// <param name="json">Json对象</param>
        public static void GetServer<T>(string ObjectID, T ObjectDatas, out Newtonsoft.Json.Linq.JObject json)
        {
            json = new Newtonsoft.Json.Linq.JObject();
            try
            {
                string jsonStr = JsonConvert.SerializeObject(ObjectDatas);
                //Newtonsoft.Json.Linq.JObject jo = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(jsonStr);
                json.Add("Command", "Get");//添加发送命令符
                json.Add("ObjectID", ObjectID);//添加对象ID
                //json.Add("ObjectDatas", jsonStr);//添加对象字段
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 组合读取对象数据返回Json对象
        /// </summary>
        /// <typeparam name="T">数据类</typeparam>
        /// <param name="ObjectDatas">对象键值</param>
        /// <param name="json">Json对象</param>
        public static void GetServer<T>(Dictionary<string, T> ObjectDatas, Newtonsoft.Json.Linq.JObject json)
        {
            foreach (var item in ObjectDatas)
            {
            }
        }

        /// <summary>
        /// 组合设置对象数据返回Json对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ObjectID"></param>
        /// <param name="ObjectDatas"></param>
        /// <param name="json"></param>
        public static void SetServer(string ObjectID, object ObjectDatas, out Newtonsoft.Json.Linq.JObject json)
        {
            json = new Newtonsoft.Json.Linq.JObject();
            try
            {
                string jsonStr = JsonConvert.SerializeObject(ObjectDatas);
                //Newtonsoft.Json.Linq.JObject jo = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(jsonStr);
                json.Add("Command", "Set");//添加发送命令符
                json.Add("ObjectID", ObjectID);//添加对象ID
                json.Add("ObjectDatas", jsonStr);//添加对象字段
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}