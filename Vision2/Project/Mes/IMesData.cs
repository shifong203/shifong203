using System.Collections.Generic;
using Vision2.Project.formula;
using Vision2.vision;
using HalconDotNet;
using Vision2.vision.HalconRunFile.RunProgramFile;
using  ErosSocket.DebugPLC.Robot;

namespace Vision2.Project.Mes
{
    public interface IMesData
    {
        void WrietMes(UserFormulaContrsl userFormulaContrsl, string QRCODE, string Product_Name);
        void WrietMes(TrayRobot trayData, string Product_Name);
        void WrietMes(DataVale trayData, string Product_Name);
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
    ///// <summary>
    ///// 托盘类结果
    ///// </summary>
    //public class TrayResetData
    //{
    //public    TrayResetData(int x,int y)
    //  {
    //      Product_Name = Product.ProductionName;
    //      OK = false;
    //      XNumber = x;
    //      YNumber = y;

    //      PaneNumber = x * y;
    //      NGLocation = new List<int>();
    //      GetDataVales() = new List<DataVale>( new DataVale[XNumber* YNumber]);
    //  }
    //  /// <summary>
    //  /// 产品名称
    //  /// </summary>
    //  public string Product_Name { get; set; }
    //  /// <summary>
    //  /// 托盘ID
    //  /// </summary>
    //  public string Tray_ID { get; set; }

    //  /// <summary>
    //  /// 整盘结果
    //  /// </summary>
    //  public bool OK { 
    //      get {
    //          for (int i = 0; i < GetDataVales().Count; i++)
    //          {
    //              if (GetDataVales()[i]!=null)
    //              {
    //                  if (!GetDataVales()[i].OK)
    //                  {
    //                      return false;
    //                  }
    //              }
    //          }
    //          return true; 
    //      }

    //      set {   }

    //  }
    //  /// <summary>
    //  /// 产品数量
    //  /// </summary>
    //  public int PaneNumber { get; set; }

    //  /// <summary>
    //  /// NG数量
    //  /// </summary>
    //  public int NGNumber { get {
    //          int ngNu = 0;
    //          for (int i = 0; i < GetDataVales().Count; i++)
    //          {
    //              if (GetDataVales()[i] != null)
    //              {
    //                  if (!GetDataVales()[i].OK)
    //                  {
    //                      ngNu++;
    //                  }
    //              }
    //          }
    //          return ngNu;
    //      } 
    //  }
    //  /// <summary>
    //  /// X方向数量
    //  /// </summary>
    //  public int XNumber { get; set; }

    //  /// <summary>
    //  /// Y方向数量
    //  /// </summary>
    //  public int YNumber { get; set; }

    //  /// <summary>
    //  /// NG位置
    //  /// </summary>
    //  public List<int> NGLocation = new List<int>();

    //  //public void AddPaneName(string name, bool isOK, double GetDataVales(), HalconDotNet.HObject XLD = null, HalconDotNet.HObject hObject = null)
    //  //{
    //  //    if (NewData != null)
    //  //    {
    //  //        NewData.Data1.Add(new MaxMinValue() { Name = name, AutoOK = isOK, ImageObj = hObject, XLD = XLD, PositionName = this.Product_Name, Value = GetDataVales() });
    //  //    }
    //  //}
    //  /// <summary>
    //  /// 结果集合
    //  /// </summary>
    //  /// <returns></returns>
    //  public List<DataVale> GetDatas()
    //  {
    //      return GetDataVales();
    //  }
    //  /// <summary>
    //  /// 多个产品信息
    //  /// </summary>
    //  public  List<DataVale> GetDataVales() = new List<DataVale>();
    //  IMesData mesData;
    //  public IMesData GetMes(IMesData mesDataT = null)
    //  {
    //      if (mesDataT != null)
    //      {
    //          mesData = mesDataT;
    //      }
    //      return mesData;
    //  }
    //  public virtual void ShowDolg()
    //  {
    //      vision.Vision.ShowVisionResetForm();
    //  }
    //  public void WritMes(UserFormulaContrsl userFormulaContrsl)
    //  {
    //      if (mesData != null)
    //      {
    //          mesData.WrietMes(userFormulaContrsl, Tray_ID, Product_Name);
    //      }
    //  }
    //  public virtual void Clear()
    //  {

    //      NGLocation = new List<int>();
    //      Tray_ID = "";
    //      if (GetDataVales() != null)
    //      {
    //          GetDataVales().Clear();
    //      }
    //  }
//}
        /// <summary>
        /// 单个产品信息
        /// </summary>
        public class DataVale
        {
            /// <summary>
            /// 产品SN
            /// </summary>
            public string PanelID { get; set; } = "";
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

            public bool Null;

            /// <summary>
            /// 结果
            /// </summary>
            public bool OK
            {
                get
                {
                    foreach (var item in ListCamsData)
                    {
                        if (!item.Value.OK) return false;
                    }
                    return true;
                }
                set
                {   if (value)
                    {         
                        foreach (var item in ListCamsData)
                        {
                            item.Value.OK = true;
                        }        }
                    autoOk = value;     
                    }
            }
            bool autoOk;
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

        //List<OneCamData> ListCamsData = new List<OneCamData>();
        ///// <summary>
        ///// 拍照数据集合
        ///// </summary>
        //public List<OneResultOBj> ResuOBj = new List<OneResultOBj>();

        //     OneContOBJs DicNGObj = new OneContOBJs();
        //    //public Dictionary<string, OneRObj> DicNGObj = new Dictionary<string, OneRObj>();

        //    public OneContOBJs NGObj 
        //    {
        //        get 
        //        {
        //             return DicNGObj;
        //        }
        //    }
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

            set { }
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


    }



}
