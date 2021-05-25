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
            /////// <summary>
            /////// 附加结果
            /////// </summary>
            //public List<MaxMinValue> Data1 { get; set; } = new List<MaxMinValue>();

            public HalconResult GetHalconResult(HalconResult halcon= null)
            {
                if (halcon != null)
                {
                    halconResult = halcon;
                }
                return halconResult;
            }
            HalconResult halconResult = new HalconResult();
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
            public bool Done { get; set; }
            /// <summary>
            /// 结果
            /// </summary>
            public bool OK
            {
                get
                {
                    foreach (var item in ResuOBj)
                    {
                        if (!item.OK) return false;
                    }
               

                
                    return true;
                }
                set
                {
                    if (value)
                    {
                        foreach (var item in ResuOBj)
                        {
                            item.OK = true;
                        }
                    }
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
                    for (int i = 0; i < ResuOBj.Count; i++)
                    {
                        for (int i2 = 0; i2 < ResuOBj[i].NGObj.Count; i2++)
                        {
                            if (!ResuOBj[i].NGObj[i2].OK) errNumber++;
                        }
                    }
                    return errNumber;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public List<string> ListImage = new List<string>();

            public List<HObject> ImageS = new List<HObject>();

            /// <summary>
            /// 拍照数据集合
            /// </summary>
            public List<OneResultOBj> ResuOBj = new List<OneResultOBj>();

            public List<OneRObj> NGObj 
            {
                get 
                {
                    List<OneRObj> oneRs = new List<OneRObj>();
                    for (int i = 0; i < ResuOBj.Count; i++)
                    {
           
                           for (int i2 = 0; i2 < ResuOBj[i].NGObj.Count; i2++)
                            { 
                                  oneRs.Add(ResuOBj[i].NGObj[i2]);
                                } } 
                   return oneRs;
                }
            }
            public HObject ImagePlus;
        /////// <summary>
        /////// 附加结果
        /////// </summary>
        public Dictionary<string, DataMinMax> DataMin_Max { get; set; } = new Dictionary<string, DataMinMax>();
            public void AddOneComponent(DataMinMax dataMinMax)
            {
                if (!DataMin_Max.ContainsKey(dataMinMax.ComponentName))
                {
                    DataMin_Max.Add(dataMinMax.ComponentName, dataMinMax);
                }
                else
                {

                }
 
            }
        }

        ///// <summary>
        ///// 点位名称
        ///// </summary>
        //public class MaxMinValue
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double Value;
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double MaxValue;
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double MinValue;
        //    /// <summary>
        //    /// 复判结果
        //    /// </summary>
        //    public bool OK;
        //    /// <summary>
        //    /// OK
        //    /// </summary>
        //    public bool AutoOK { 
        //    get {
        //        if (MinValue > Value || MaxValue < Value)
        //        {
        //            return false;
        //        }
        //        return true;
        //       }    }
        //    /// <summary>
        //    /// 名称
        //    /// </summary>
        //    public string Name;
        //    /// <summary>
        //    /// 位置号
        //    /// </summary>
        //    public string PositionName;

        //}


}
