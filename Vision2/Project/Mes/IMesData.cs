using ErosSocket.DebugPLC.Robot;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project.DebugF;
using Vision2.Project.formula;
using Vision2.vision;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.Project.Mes
{
    public interface IMesData
    {
        delegate void ResTMesd(bool ok, string mesRestStr);
        event ResTMesd ResDoneEvent;
       
        void WrietMes(TrayData trayData, string Product_Name);
        void WrietMes(OneDataVale trayData, string Product_Name);
        void WrietMesAll<T>(T data,  string Product_Name);
        /// <summary>
        /// 根据sn查询mes信息
        /// </summary>
        /// <param name="SerialNumber">sn</param>
        /// <param name="resetMesString">错误信息</param>
        /// <returns>成功返回true</returns>
        bool ReadMes(out string resetMesString,TrayData trayData);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetMesString"></param>
        /// <returns></returns>
        bool ReadMes(string sn, out string resetMesString);

    }
    public abstract class MesInfon : IMesData
    {

        public abstract Form GetForm();

        public abstract event IMesData.ResTMesd ResDoneEvent;

        [Description("记录Mes长度"), Category("Mes"), DisplayName("Mes历史长度")]
        public int MesLength { get; set; } = 1000;
        public List<string> MesSN { get; set; } = new List<string>();
        [Description("使用SN重复检测"), Category("Mes"), DisplayName("使用SN重复检测")]
        public bool MesReduplication { get; set; } = true;


        //    return datas;
        //}
        /// <summary>
        /// 写入产品CRD数据
        /// </summary>
        /// <param name="oneDataVale"></param>
        public virtual void WrietDATA(OneDataVale oneDataVale)
        {
            try
            {
                string path = RecipeCompiler.Instance.DataPaht + "\\CRD数据\\" + DateTime.Now.ToString("yyyyMMdd") + ".CSV";
                if (!File.Exists(path))
                {
                    List<string> columnText = new List<string>() { "NO",/*"Line", "Customer",*/"Mode",
                        "Defect Type","Location" ,"Serial Number","Result","Date" ,
                        "Start Time" ,"End Time","User", "Placement Route Step","位置","机检"};
                    ErosProjcetDLL.Excel.Npoi.AddWriteCSV(path, columnText.ToArray());
                }
                int no = 0;
                foreach (var item in oneDataVale.GetAllCompOBJs().DicOnes)
                {
                    no++;
                    List<string> data = new List<string>();
                    data.Add(no.ToString());
                    //data.AddRange(RecipeCompiler.Instance.GetMes().GetCRDMesData());
                    //data.Add(RecipeCompiler.Instance.GetMes().Customer);
                    data.Add(oneDataVale.Product_Name);
                    data.Add(item.Value.RestText);
                    data.Add(item.Value.ComponentID);
                    data.Add(oneDataVale.PanelID);
                    if (item.Value.aOK)
                    {
                        data.Add("Pass");
                    }
                    else
                    {
                        data.Add("Fail");
                    }
                    data.Add(DateTime.Now.ToString("d"));
                    data.Add(oneDataVale.StrTime.ToString("T"));
                    data.Add(oneDataVale.EndTime.ToString("T"));
                    data.Add(ProjectINI.In.UserName);
                    data.Add(DebugCompiler.Instance.DeviceNameText);
                    data.Add(oneDataVale.TrayLocation.ToString());
                    data.Add(item.Value.NGText);
                    ErosProjcetDLL.Excel.Npoi.AddWriteCSV(path,
                      data.ToArray());
                }
            }
            catch (Exception) { }
        }
        public virtual void WrietOneData(OneDataVale oneDataVale,string path)
        {
            try
            {
                List<string>dat = new List<string>();
                dat.Add(oneDataVale.StrTime.ToString("HH:mm:ss"));
                dat.Add(oneDataVale.TrayLocation.ToString());
                if (oneDataVale.OK)
                {
                    dat.Add("OK");
                }
                else
                {
                    dat.Add("NG");
                }
                dat.Add(oneDataVale.PanelID);
                dat.Add(oneDataVale.AutoOK.ToString());
                dat.Add(oneDataVale.MesStr);
                dat.Add(oneDataVale.FVTStr);
                if (oneDataVale.UesrRest)
                {
                    dat.Add("人工提交");
                }
                else
                {
                    dat.Add("");
                }
                 dat.Add(oneDataVale.Done.ToString());
              
               
                ErosProjcetDLL.Excel.Npoi.AddWriteCSV(path, dat.ToArray());

            }
            catch (Exception)
            {
            }
        } 


        [Description("线号"), Category("Mes信息"), DisplayName("线号")]
        public virtual string Line_Name { get; set; } = "Bay32";


        [Description("Fail结果Mes处理"), Category("Mes处理"), DisplayName("Fail结果上传")]

        public virtual bool FaliMesRest { get; set; } = true;

        [DescriptionAttribute("线体标示。"), Category("设备标识"), DisplayName("站号")]
        public virtual string Fixture_ID { get; set; } = "EQDIW00011-01";

        [Description("站号选择"), Category("Mes信息"), DisplayName("站号选择")]
        public List<string> FixtureList { get; set; } = new List<string>();

        [Description("线号选择"), Category("Mes信息"), DisplayName("线号选择")]
        public List<string> ListStr { get; set; } = new List<string>();



        [Description("默认写入缺陷"), Category("缺陷管理"), DisplayName("默认缺陷")]
        public string DefaultFlaw { get; set; } = "MISSING";


        [Description(""), Category("Mes查询"), DisplayName("异步查询")]
        /// <summary>
        ///模拟FVTPass
        /// </summary>
        public bool MesArye { get; set; }
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

    


        public abstract void WrietMes(TrayData trayData, string Product_Name);

        public abstract void WrietMes(OneDataVale trayData, string Product_Name);

        public virtual void WrietMes(OneDataVale trayData)
        {
            WrietMes(trayData, Product.ProductionName);
        }
        public virtual void WrietMes(TrayData trayData)
        {
            WrietMes(trayData, Product.ProductionName);
        }
        public virtual void WrietMesAll<T>(T data, string Product_Name)
        {
            try
            {
                TrayData tray = data as TrayData;
                if (tray != null)
                {
                    WrietMes(tray, Product_Name);
                }
                else
                {
                    OneDataVale one = data as OneDataVale;
                    WrietMes(one, Product_Name);
                }
            }
            catch (Exception ex)
            {
                AlarmListBoxt.AddAlarmText(new AlarmText.alarmStruct() { Name = "WrietMesAll写入失败", Text = ex.Message });
            }
        }

        public abstract bool ReadMes(out string resetMesString, TrayData trayData);


        public abstract bool ReadMes(string sn,out string resetMesString);

    }

    /// <summary>
    /// 单个产品信息
    /// </summary>
    public class OneDataVale :IDisposable
    {
        public void Dispose()
        {
            KeyPamgr.Clear();
            foreach (var item in ListCamsData)
            {
                //item.Value.GetImagePlus().Dispose();
                for (int i = 0; i < item.Value.ResuOBj().Count; i++)
                {
                    item.Value.ResuOBj()[i].Dispose();
                }
                foreach (var itemDT in item.Value.AllCompObjs.DicOnes)
                {
                    //itemDT.Value.NGROI.Dispose();
                    //itemDT.Value.ROI.Dispose();
                    foreach (var itemDTE in itemDT.Value.oneRObjs)
                    {
                        //itemDTE.ROI.Dispose();
                        //itemDTE.NGROI.Dispose();
                    }
                }
                //foreach (var item in oneContOBJs.DicOnes)
                //{
                //    foreach (var itemtd in item.Value.oneRObjs)
                //    {
                foreach (var itemOBJ in item.Value.NGObj.DicOnes)
                {
                    //itemOBJ.Value.NGROI.Dispose();
                    //itemOBJ.Value.ROI.Dispose();
                    foreach (var itemDTE in itemOBJ.Value.oneRObjs)
                    {
                        //itemDTE.ROI.Dispose();
                        //itemDTE.NGROI.Dispose();
                    }
                }
                //item.Value.NGObj.DicOnes.Clear();
            }

        }
        ~OneDataVale()
        {
            try
            {
                foreach (var item in ListCamsData)
                {
                    item.Value.GetImagePlus().Dispose();
                }
                //Dispose();
            }
            catch (Exception)
            {

          
            }
        
        }
        /// <summary>
        /// 图像文件夹
        /// </summary>
        public string ImagePaht = "";
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StrTime = DateTime.Now;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime;
        public Dictionary<string, string> KeyPamgr { get; set; } = new Dictionary<string, string>();
        public string DeviceName { get; set; } = "AVI";
        public bool UesrRest { get; set; }
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
        public void AddNG(string name, bool OKt = false)
        {
            if (NGName == null)
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
        public string PanelID
        {
            get;
            set;
        } = "";
        /// <summary>
        /// 产品型号
        /// </summary>
        public string Product_Name {
            get
            { return Product.ProductionName; }

            //set { product_Name = value; } 
        
        }
      
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
        public bool Done
        {
            get
            {
                foreach (var item in ListCamsData)
                {
                    if (!item.Value.Done) return false;
                }
                return true;
            }
            set
            {
                foreach (var item in ListCamsData)
                {
                    item.Value.Done = value;
                }
                //if (value)
                //{
                //    ok = true;
                //}
            }
        }
        public void SetOKBit(bool okB=true)
        {
            ok = okB;
        }
        public string MesStr = "";
        public string FVTStr = "";
       /// <summary>
       /// True不是空
       /// </summary>
        public bool NotNull { get; set; }

        bool ok = true;
        /// <summary>
        /// 结果
        /// </summary>
        public bool OK
        {
            get
            {
                if (!NotNull)
                  {
                    return false;
                }
                foreach (var item in ListCamsData)
                {
                    if (!item.Value.aOK) 
                        return false;
                }
                foreach (var item in NGName)
                {
                    if (!item.Value)
                        return false;
                }
                if (!ok)
                {
                    return false;
                }
                 return true;
            }
            set
            {
                Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>();
                foreach (var item in NGName)
                {
                    keyValuePairs.Add(item.Key, value);
                }
                NGName = keyValuePairs;
                foreach (var item in ListCamsData)
                {
                    item.Value.aOK = value;
                }
                ok = value;
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
            string data = "";
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
        /// <summary>
        /// 获取全部结果
        /// </summary>
        /// <returns></returns>
        public OneCompOBJs GetAllCompOBJs()
        {
            OneCompOBJs OneCompO= new OneCompOBJs();
            foreach (var item in ListCamsData)
            {
                foreach (var itemdet in item.Value.AllCompObjs.DicOnes)
                {
                    OneCompO.Add( itemdet.Value);
                }
                foreach (var itemdet in item.Value.NGObj.DicOnes)
                {
                    OneCompO.Add(itemdet.Value);
                }
            }
            return OneCompO;
        }

        public HObject GetNGImage()
        {
            foreach (var item in ListCamsData)
            {
                if (!item.Value.Done)
                {
                    return item.Value.GetImagePlus();
                }
            }
            foreach (var item in ListCamsData)
            {
                return item.Value.GetImagePlus();
            }
            return null;
        }

        public HObject[] GetImages(string camName)
        {
            List<HObject> hObjects = new List<HObject>();
            List<int> runIDs = new List<int>();
            try
            {
                if (ListCamsData.ContainsKey(camName))
                {
                    for (int i = 0; i < ListCamsData[camName].ResuOBj().Count; i++)
                    {
                   
                        if (!runIDs.Contains(ListCamsData[camName].ResuOBj()[i].LiyID))
                        {
                            runIDs.Add(ListCamsData[camName].ResuOBj()[i].LiyID);
                            hObjects.Add(ListCamsData[camName].ResuOBj()[i].Image);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return hObjects.ToArray();

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
        public void AddCamsData(string runName, int runID, OneCamData oneCam)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="runName"></param>
        /// <param name="imagePalus"></param>
        public void AddCamsData(string runName, HObject imagePalus)
        {
            if (!ListCamsData.ContainsKey(runName))
            {
           //     ListCamsData.Add(runName, oneCam);
            }
            else
            {
                ListCamsData[runName].GetImagePlus(imagePalus);
            }
        }

        public void AddCamOBj(string camName, OneRObj oneRObj)
        {
            if (ListCamsData.ContainsKey(camName))
            {
                if (oneRObj.aOK)
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
        public OneCamData()
        {
            ImagePlus.GenEmptyObj();
        }
        /// <summary>
        /// 视觉程序名称
        /// </summary>
        public string RunVisionName;
        /// <summary>
        /// 单面集合是否OK
        /// </summary>
        public bool aOK
        {
            get
            {

                foreach (var item in DicNGObj.DicOnes)
                {
                    if (!item.Value.aOK)
                    {
                        return false;
                    }
                }
                return true;
            }
            set
            {

                foreach (var item in DicNGObj.DicOnes)
                {
                    item.Value.aOK = value;
                }
            }
        }
        /// <summary>
        /// 单面集合是否完成
        /// </summary>
        public bool Done
        {
            get
            {
                foreach (var item in NGObj.DicOnes)
                {
                    if (!item.Value.Done)
                    {
                        return false;
                    }
                }
                return true;
            }
            set
            {
                foreach (var item in NGObj.DicOnes)
                {
                    item.Value.Done = value;
                }

            }
        }
        /// <summary>
        /// 软件判断是否OK
        /// </summary>
        public bool AutoOK
        {
            get
            {
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
        /// 误判数量
        /// </summary>
        public int ErrJudNumber
        {
            get
            {
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

        public HObject GetImagePlus(HObject hObject=null)
        {
            if (hObject!=null)
            {
                ImagePlus = hObject;
            }
            return ImagePlus ;
        }
        public string ImagePaht = "";
        /// <summary>
        /// 整合图片
        /// </summary>
        HObject ImagePlus = new HObject();

        /// <summary>
        /// 拍照数据集合
        /// </summary>
          List<OneResultOBj> resuOBj = new List<OneResultOBj>();
        public List<OneResultOBj> ResuOBj()
        {
            return resuOBj;
        }

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
        public OneCompOBJs AllCompObjs = new OneCompOBJs();

        public OneCompOBJs GetAllCompOBJs()
        {
            try
            {
                foreach (var item in ResuOBj())
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
