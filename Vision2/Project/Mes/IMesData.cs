using System.Collections.Generic;
using Vision2.Project.formula;
using Vision2.vision;
using HalconDotNet;
using Vision2.vision.HalconRunFile.RunProgramFile;
using  ErosSocket.DebugPLC.Robot;
using System;
using Vision2.ErosProjcetDLL.Project;
using System.Windows.Forms;

namespace Vision2.Project.Mes
{
    public interface IMesData
    {
        delegate void ResTMesd(bool ok, string mesRestStr);
        event ResTMesd ResDoneEvent;
        void WrietMes(UserFormulaContrsl userFormulaContrsl, string QRCODE, string Product_Name);
        void WrietMes(TrayData trayData, string Product_Name);
        void WrietMes(OneDataVale trayData, string Product_Name);
        void WrietMesAll<T>(T data, string QRCODE, string Product_Name);
        /// <summary>
        /// 根据sn查询mes信息
        /// </summary>
        /// <param name="SerialNumber">sn</param>
        /// <param name="resetMesString">错误信息</param>
        /// <returns>成功返回true</returns>
        bool ReadMes(string SerialNumber, out string resetMesString);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetMesString"></param>
        /// <returns></returns>
        bool ReadMes(out string resetMesString);

    }
    public abstract  class MesInfon :IMesData
    {

        public abstract Form GetForm();

        public abstract  event IMesData.ResTMesd ResDoneEvent;

        /// <summary>
        /// 保存到项目地址,必须调用父类
        /// </summary>
        /// <param name="name"></param>
        public virtual void SaveThis(string path = null)
        {
            if (path == null)
            {
                path = ProjectINI.ProjietPath + "Project\\" + ProjectINI.In.ProjectName + "\\" + ProjectINI.In.RunName;
            }
            ProjectINI.ClassToJsonSavePath(this, path + "\\Mes.txt");
        }

        /// <summary>
        /// 读取Josn到实例
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="obj">对象</param>
        public virtual T ReadThis<T>(string path, T obj = default(T)) where T : new()
        {
            if (System.IO.File.Exists(path + "\\Mes.txt"))
            {
                ProjectINI.ReadPathJsonToCalss(path + "\\Mes.txt", out obj);
            }
            if (obj == null)
            {
                obj = new T();
            }
            return obj;
        }

        public abstract void WrietMes(UserFormulaContrsl userFormulaContrsl,
            string QRCODE, string Product_Name);


        public abstract void WrietMes(TrayData trayData, string Product_Name);

        public abstract void WrietMes(OneDataVale trayData, string Product_Name);


        public abstract void WrietMesAll<T>(T data, string QRCODE, string Product_Name);


        public abstract bool ReadMes(string SerialNumber, out string resetMesString);


        public abstract bool ReadMes(out string resetMesString);
  
    }

    /// <summary>
        /// 单个产品信息
        /// </summary>
    public class OneDataVale
    {
            public DateTime StrTime = DateTime.Now;
            public DateTime EndTime ;
            public void ResetOK()
           {
            Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>();
                foreach (var item in NGName)
                {
                    keyValuePairs.Add(item.Key, true);
                }
            NGName = keyValuePairs;
            OK = true;
                Done = true;
            }
            public void ResetNG()
            {
        
               OK = false;
                Done = true;
            }
            public void AddNG(string name,bool OKt=false)
            {
            if (NGName==null)
            {
                NGName = new Dictionary<string, bool>();

            }
                if (!NGName.ContainsKey(name))
                {
                   NGName.Add(name, OKt);
                 }
                 else
                {
                  NGName[name] = OKt;
                }
            }
            public Dictionary<string, bool> NGName { get; set; } = new Dictionary<string, bool>();

           /// <summary>
        /// 产品SN
        /// </summary>
            public string PanelID { get; 
            set; } = "";
            /// <summary>
            /// 产品型号
            /// </summary>
            public string Product_Name { get; set; } = "";

            /// <summary>
            /// 托盘位号
            /// </summary>
            public int TrayLocation { get; set; }

            public List<double> Data
            {
                get;
                set;
            }
             /// <summary>
            /// 自动OK
            /// </summary>
            public bool AutoOK { get; set; }
            /// <summary>
            /// 完成
            /// </summary>
            public bool Done {
                get      {
                     foreach (var item in ListCamsData)
                {
                    if (!item.Value.Done) return false;
                }
                     return true;     }
                set        {
                   foreach (var item in ListCamsData)
                   {
                    item.Value.Done = value;
                    }     } }

            public bool NotNull;

            /// <summary>
            /// 结果
            /// </summary>
            public bool OK
            {
                get
                {
                    if (!NotNull)
                    {
                        return NotNull;
                    }
                    foreach (var item in ListCamsData)
                    {
                        if (!item.Value.OK) return false;
                    }
                    foreach (var item in NGName)
                    {
                        if (!item.Value) return false;
                    }
                    return true;
                }
                set
                {
                        Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>();
                        foreach (var item in NGName)
                        {
                            keyValuePairs.Add(item.Key, false);
                        }
                        NGName = keyValuePairs;
                        foreach (var item in ListCamsData)
                        {
                            item.Value.OK = value;
                        }     
                    }
            }
      
            /// <summary>
            /// NG数量
            /// </summary>
            public int NGNumber { get; set; }

            /// <summary>
            /// 误判数量
            /// </summary>
            public int ErrJudNumber
            {
                get
                {
                    int errNumber = 0;
                    foreach (var item in ListCamsData)
                    {
                        errNumber = +item.Value.ErrJudNumber;
                    }
                    return errNumber;
                }
            }
            public Dictionary<string, OneCamData> ListCamsData = new Dictionary<string, OneCamData>();


        public OneCompOBJs GetNGCompData()
        {
            string data ="";
            foreach (var item in ListCamsData)
            {
                data = item.Key;
                if (!item.Value.Done)
                {
                    return item.Value.NGObj;
                }
            }
            return ListCamsData[data].NGObj;
        }
        public OneCompOBJs GetCompOBJs()
        {
            string data = "";
            foreach (var item in ListCamsData)
            {
                data = item.Key;
                return item.Value.NGObj;
            }
            return ListCamsData[data].NGObj;
        }
        public HObject GetNGImage()
        {
            foreach (var item in ListCamsData)
            {
                if (!item.Value.Done)
                {
                    return item.Value.ImagePlus;
                }
            }
            foreach (var item in ListCamsData)
            {
                return item.Value.ImagePlus;
            }
            return null;
        }
        public string GetNGCamName()
        {
            foreach (var item in ListCamsData)
            {
                if (!item.Value.Done)
                {
                    return item.Key;
                }
            }
            return "";
        }
        public void AddCamsData(string runName,int runID,OneCamData oneCam)
        {
            if (!ListCamsData.ContainsKey(runName))
            {
                ListCamsData.Add(runName, oneCam);
            }
            else
            {
                ListCamsData[runName] = oneCam;
            }
        }
        public void AddCamsData(string runName,HObject imagePalus)
        {
            if (!ListCamsData.ContainsKey(runName))
            {
                //ListCamsData.Add(runName, oneCam);
            }
            else
            {
                ListCamsData[runName].ImagePlus = imagePalus;
            }
        }

        public void AddCamOBj(string camName, OneRObj oneRObj)
        {
            if (ListCamsData.ContainsKey(camName))
            {
                if (oneRObj.OK)
                {
                    ListCamsData[camName].AllCompObjs.AddCont(oneRObj);
                }
                else
                {
                    ListCamsData[camName].NGObj.AddCont(oneRObj);
                }
            }
        }
   }
    /// <summary>
    /// 单个拍摄面
    /// </summary>
    public class OneCamData
    {
        /// <summary>
        /// 视觉程序名称
        /// </summary>
        public string RunVisionName;
         /// <summary>
         /// 单面集合是否OK
         /// </summary>
        public bool OK 
        {
            get {

                foreach (var item in DicNGObj.DicOnes)
                {
                    if (!item.Value.OK)
                    {
                        return false;
                    }
                }
                return true; 
            
            }
            set {

                foreach (var item in DicNGObj.DicOnes)
                {
                    item.Value.OK = value;
                }
            }
        }
        /// <summary>
        /// 单面集合是否完成
        /// </summary>
        public bool Done 
        {
            get {
                foreach (var item in NGObj.DicOnes)
                {
                    if (!item.Value.Done)
                    {
                        return false;
                    }
                }
                return true;
            }
            set { 
              foreach (var item in NGObj.DicOnes)
                {
                    item.Value.Done = true; 
                }
            
            }
        }
        /// <summary>
        /// 软件判断是否OK
        /// </summary>
        public bool AutoOK
        { get {
                foreach (var item in NGObj.DicOnes)
                {
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 产品托盘穴位号
        /// </summary>
        public int TrayLocation;
        /// <summary>
        /// 产品SN
        /// </summary>
        public string PanelID;
        /// <summary>
        /// 误判数量
        /// </summary>
        public int ErrJudNumber
        {
            get {
                return DicNGObj.DicOnes.Count;
            }
        }
        /// <summary>
        /// NG数量
        /// </summary>
        public int NGNumber
        {
            get
            {
                return DicNGObj.DicOnes.Count;
            }
        }
        /// <summary>
        /// 整合图片
        /// </summary>
        public HObject ImagePlus;

        /// <summary>
        /// 拍照数据集合
        /// </summary>
        public List<OneResultOBj> ResuOBj = new List<OneResultOBj>();


        /// <summary>
        /// NG元件
        /// </summary>
        OneCompOBJs DicNGObj = new OneCompOBJs();
         /// <summary>
         /// NG元件集合
         /// </summary>
        public OneCompOBJs NGObj
        {
            get
            {
                return DicNGObj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oneContOBJs"></param>
        public void SetOneContOBJ(OneCompOBJs oneContOBJs)
        {
            DicNGObj = oneContOBJs;
        }
         /// <summary>
         /// 全部集合
         /// </summary>
        public OneCompOBJs  AllCompObjs= new OneCompOBJs();

        public OneCompOBJs GetAllCompOBJs()
        {
            try
            {
                foreach (var item in ResuOBj)
                {
                    foreach (var itemd in item.GetNgOBJS().DicOnes)
                    {
                        if (!AllCompObjs.DicOnes.ContainsKey(itemd.Key))
                        {
                            AllCompObjs.DicOnes.Add(itemd.Key, itemd.Value);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return AllCompObjs;
        }


    }



}
