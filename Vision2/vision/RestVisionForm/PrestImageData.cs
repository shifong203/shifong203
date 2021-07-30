using HalconDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Vision2.vision.RestVisionForm
{
    [Serializable]
    public class PrestImageData
    {

        public NummberSPC nummber { get; set; } = new NummberSPC();
        /// <summary>
        /// NG区域
        /// </summary>
        public Dictionary<string, XldOjb> Key1Xld { get; set; } = new Dictionary<string, XldOjb>();
        /// <summary>
        /// 线体名称
        /// </summary>
        public string LinkName { get; set; }


        /// <summary>
        ///产品名称
        /// </summary>
        public string PName { get; set; }
        /// <summary>
        /// 产品SN
        /// </summary>
        public string PaleSN { get; set; }

        /// <summary>
        /// 托盘位置号
        /// </summary>
        public int TrayID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TrayX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TrayY { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public bool OK { get; set; }
        /// <summary>
        /// 完成
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        /// 需要复判
        /// </summary>
        public bool RestNG { get; set; }

        public string ImagePath { get; set; } = "";
        ///// <summary>
        ///// 产品图
        ///// </summary>
        //public HObject Image { get; set; }

    }

    [Serializable]
    public class NummberSPC
    {/// <summary>
     /// 总数
     /// </summary>
        public int Total = 0;
        /// <summary>
        /// NG
        /// </summary>
        public int NG_num = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Rest_num = 0;
        public string GetSPC()
        {
            try
            {
                if (Total == 0)
                {
                    return "总数:" + Total.ToString() + " 不良:" + NG_num.ToString() + " 过检:" + Rest_num.ToString() +
                 " 检测直通率:00" +
                  " 产品直通率:00";
                }
                else
                {
                    return "总数:" + Total.ToString() + " 不良:" + NG_num.ToString() + " 过检:" + Rest_num.ToString() +
                     " 检测直通率:" + ((double)(Total - Rest_num - NG_num) / (double)(Total - NG_num) * 100).ToString("0.00") +
                      "% 产品直通率:" + ((double)(Total - NG_num) / (double)(Total) * 100).ToString("0.00") + "%";
                }
            }
            catch (Exception)
            {
            }
            return "总数:" + Total.ToString() + "不良:" + NG_num.ToString() + "过检:" + Rest_num.ToString() +
            "检测直通率:" + 100 +
            "产品直通率:" + 100;
        }

        public void Save()
        {
            ClassToJsonSavePath(this, System.Windows.Forms.Application.StartupPath + "\\Data\\SPC");
        }

        /// <summary>
        /// 读取json文件转换为类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="obje"></param>
        /// <returns></returns>
        public static bool ReadPathJsonToCalss<T>(string path, out T obje)/* where T : new()*/
        {
            obje = default(T);
            try
            {
                if (!path.Contains("."))
                {
                    path = path + ".txt";
                }
                if (File.Exists(path))
                {
                    string strdata = File.ReadAllText(path);

                    obje = JsonConvert.DeserializeObject<T>(strdata);
                    //登录窗口中 读取语言资源文件

                    return true;
                }
                else
                {
                    //MessageBox.Show("读取失败！文件不存在" + path);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("读取文件:" + path + " 失败;" + ex.Message);

            }
            return false;
        }

        public static bool ClassToJsonSavePath(object obje, string path)
        {
            try
            {
                if (!path.Contains("."))
                {
                    path = path + ".txt";
                }
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                string jsonStr = JsonConvert.SerializeObject(obje);
                File.WriteAllText(path, jsonStr);
                //MessageBox.Show("保存成功");
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("保存失败" + path + "=" + ex.Message);
            }
            return false;
        }

        public static string CalassToJsonString(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary> 
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>         
        /// <returns></returns> 
        public static byte[] ObjectToBytes(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter(); formatter.Serialize(ms, obj); return ms.GetBuffer();
            }
        }

        /// <summary> 
        /// 将一个序列化后的byte[]数组还原         
        /// </summary>
        /// <param name="Bytes"></param>         
        /// <returns></returns> 
        public static object BytesToObject(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                IFormatter formatter = new BinaryFormatter(); return formatter.Deserialize(ms);
            }
        }
    }
}
