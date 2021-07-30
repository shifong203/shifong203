using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project.DebugF.IO;
using Vision2.Project.Mes;
using ErosSocket.DebugPLC.Robot;
using static Vision2.Project.Mes.安费诺Mes;
using System.Reflection;

namespace Vision2.Project.formula
{
    /// <summary>
    /// 配方管理
    /// </summary>
    public class RecipeCompiler : ProjectObj
    {
        public RecipeCompiler()
        {
            Instance = this;
        }

        public static RecipeCompiler Instance;
        /// <summary>
        /// 目标与当前值
        /// </summary>
        public struct LinkVaset
        {
            public LinkVaset(string texe)
            {
                Text = texe;
                SetName = "";
                GetName = "";
            }

            public LinkVaset(string tex, string getName, string setName)
            {
                Text = tex;
                SetName = setName;
                GetName = getName;
            }
            /// <summary>
            /// 显示参数名
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// 目标值变量名称
            /// </summary>
            public string SetName { get; set; }
            /// <summary>
            /// 当前值变量名称
            /// </summary>
            public string GetName { get; set; }

        }

    
        //public MesData MesDatat { get; set; } = new MesData();

        public 安费诺MesData MesDa { get; set; } = new 安费诺MesData();
        /// <summary>
        /// 全局点集合
        /// </summary>
        public Dictionary<string, List<XYZPoint>> DPoint { get; set; } = new Dictionary<string, List<XYZPoint>>();

        /// <summary>
        /// 产品
        /// </summary>
        public Dictionary<string, ProductEX> ProductEX { get; set; } = new Dictionary<string, ProductEX>();

        public static ProductEX GetProductEX()
        {
            if (RecipeCompiler.Instance.ProductEX.ContainsKey(Product.ProductionName))
            {
                return RecipeCompiler.Instance.ProductEX[Product.ProductionName];
            }
            return null;
        }

        #region 继承重写方法
        public override string FileName => "配方管理";
        public override string Text { get; set; } = "配方管理";
        public override string SuffixName => ".recipeCompiler";
        public override string ProjectTypeName => "配方管理";
        public override string Name => "配方管理";

        [Description("程序与产品数量关系，1以下为单个产品多个程序，1以上为1个程序1个产品"), Category("产品扫码"), DisplayName("计数规则")]
        public TrayEnumType TrayQRType { get; set; }

        public enum TrayEnumType
        {
            一个流程一个产品 = 0,
            多个流程一个产品 = 1,
            托盘扫码计数 = 2,
        }

      
        public string GetSPC()
        {
            try
            {
                string data = Instance.OKNumber.OKNumber.ToString() + "," + Instance.OKNumber.NGNumber.ToString() + "," + Instance.OKNumber.IsOK + "," + Instance.OKNumber.Number + "," + Instance.OKNumber.OKNG+ "," + Instance.OKNumber.AutoNGNumber;
                File.WriteAllText(ProjectINI.TempPath + "计数.txt", data);
            }
            catch (Exception)
            {
            }
            return "计数:" + OKNumber.Number + "  良率%:" + OKNumber.OKNG.ToString("0.00") + "\n\rOK:" + OKNumber.OKNumber + "  NG:" + OKNumber.NGNumber + "  NOK:" + OKNumber.AutoNGNumber;
        }
  
        public enum EnumUpDataType
        {
            表格 = 0,
            托盘 = 1,
            复判按钮 = 2,
            不显示 = 3,
            弹出复判按钮 = 4,
        }
      

        public MesInfon GetMes()
        {
            return data1;
        }
     
        MesInfon data1;
        [Description("客户端使用"), Category("复判通信"), DisplayName("复判客户端名")]
        [TypeConverter(typeof(ErosSocket.ErosConLink.DicSocket.LinkNameConverter))]
        public string RsetLinkName { get; set; } = "";

        [Description("服务端使用"), Category("复判通信"), DisplayName("复判服务器名")]
        [TypeConverter(typeof(ErosSocket.ErosConLink.DicSocket.LinkNameConverter))]
        public string RsetSoeverLinkName { get; set; } = "";

        [Description("通信连接名"), Category("通信数据"), DisplayName("通信测试数据连接名")]
        [TypeConverter(typeof(ErosSocket.ErosConLink.DicSocket.LinkNameConverter))]
        public string DataLinkName { get; set; } = "";

        [Description("分割,数据起点索引"), Category("通信数据"), DisplayName("数据起点索引")]
        public int DataLinkStrat { get; set; } = 0;
        [Description("数据起点"), Category("通信数据"), DisplayName("数据起点")]
        public int DataStrat { get; set; } = 0;

        [Description("单个产品数据接受次数"), Category("通信数据"), DisplayName("数据次数")]
        public int DataNumber { get; set; } = 2;
        [Description(""), Category("通信数据"), DisplayName("数据最小长度")]
        public int DataMinCont{ get; set; } = 100;
        [Description(""), Category("通信数据"), DisplayName("相机名称")]
        public string DataMCamName { get; set; } ="上相机";

        public LinkData Data { get; set; } = new LinkData();


        [Description("数据显示的方式"), Category("显示数据"), DisplayName("数据显示方式")]
        public EnumUpDataType UpDataType { get; set; }
        /// <summary>
        /// 托盘ID
        /// </summary>
        [DescriptionAttribute("。"), Category("显示数据"), DisplayName("是否使用托盘")]
        public sbyte TrayCont { get; set; } = -1;

        [Description("是否显示二维码"), Category("显示数据"), DisplayName("显示托盘码")]
        public bool IsQRCdoe { get; set; } = true;
        [DescriptionAttribute("。"), Category("显示数据"), DisplayName("托盘ID判断")]
        public bool TrayID { get; set; }

        [DescriptionAttribute("。"), Category("显示数据"), DisplayName("穴位ID判断")]
        public bool PalenID { get; set; }
        [DescriptionAttribute("。"), Category("显示数据"), DisplayName("显示手动输入ID")]
        public bool PalenIDVsible { get; set; }



        [Description("是否复盘"), Category("复判"), DisplayName("是否复盘")]
        public bool IsRestOk { get; set; }



        [Description("文件后缀"), Category("Mes数据"), DisplayName("文件后缀")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", false,".txt", ".Csv", "")]
        public string Filet { get; set; } = ".txt";

        [Description("文件命名规则"), Category("Mes数据"), DisplayName("写文件命名规则")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "QR", "Time", "")]
        public string WritDataFileName { get; set; } = "QR";

        [Description("Mes厂家规则,"), Category("Mes数据"), DisplayName("写Mes厂家规则")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "","伟世通", "丸旭", "捷普", "安费诺","SISF")]
        public string MesType { get; set; }
        [Description("Mes厂家规则,"), Category("Mes数据"), DisplayName("Mes类")]
        public string MesTypeName { get; set; }
        [Description("复判数据回传去程序PC,"), Category("Mes数据"), DisplayName("复判数据回传")]

        public bool RsetSever { get; set; }

        [Description("扫码触发信号连接变量名"), Category("产品扫码"), DisplayName("扫码触发信号名")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string GetTrageNmae { get; set; } = "";

        [Description("通信触发连接名"), Category("产品扫码"), DisplayName("扫码枪通信名")]
        [TypeConverter(typeof(ErosSocket.ErosConLink.DicSocket.LinkNameConverter))]
        public string GetQRLinkNmae { get; set; } = "";
        [Description("通信触发扫码标志"), Category("产品扫码"), DisplayName("扫码触发标志")]
        public string QRTrageText { get; set; } = "T";


        [Description("扫码完成写入True/1"), Category("产品扫码"), DisplayName("扫码完成")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string SetTrageDoneNmae { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Produc { get; set; } = new Dictionary<string, Dictionary<string, string>>();


        [Description("物料集合"), Category("产品信息"), DisplayName("物料集合")]
        [Editor(typeof(MaterialContrsl.Editor), typeof(UITypeEditor))]
        public List<MaterialManagement> ListMaterial { get; set; } = new List<MaterialManagement>();

        [DescriptionAttribute("设置与当前参数。"), Category("参数"), DisplayName("设置与监视参数"),
        Editor(typeof(Editor), typeof(UITypeEditor))]
        public List<LinkVaset> Dsate { get; set; } = new List<LinkVaset>();


        /// <summary>
        /// 更新参数节点
        /// </summary>
        /// <param name = "treeNodet" ></ param >
        public override void UpProjectNode(TreeNode treeNodet)
        {
            try
            {
                base.UpProjectNode(treeNodet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Name + "刷新错误:" + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static UserFormulaContrsl GetUserFormulaContrsl()
        {
            return userVisionManagement;
        }

        static UserFormulaContrsl userVisionManagement;
        public override void initialization()
        {
            try
            {
                IMesData mesData = null;
                if (mesData == null)
                {
                    switch (RecipeCompiler.Instance.MesType)
                    {
                        case "丸旭":
                            mesData=new 丸旭Mes();
                            break;
                        case "捷普":
                            mesData=new MesJib();
                            break;
                        case "伟世通":
                            mesData=new 伟世通Mes();
                            break;
                        case "安费诺":
                            mesData = new 安费诺Mes();
                            break;
                        case "SISF":
                            mesData = new Mes.环旭SISF.SISF();
                            break;
                        default:
                            break;
                    }
                }
              
                Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
                MesTypeName = mesData.GetType().FullName;
                dynamic obj = assembly.CreateInstance(MesTypeName);
                if (obj!=null)
                {
                    data1 = obj.ReadThis(ProjectINI.In.ProjectPathRun + "\\产品配方", obj);
                }
            }
            catch (Exception ex)
            {
                AlarmText.AddTextNewLine("读取Mes失败:"+ex.Message);
            }
            Product.ReadExcelDic(ProjectINI.In.ProjectPathRun + "\\产品配方\\产品文件", out string err);
            if (RecipeCompiler.Instance.DPoint.Count != 0)
            {
                foreach (var item in Product.GetThisP())
                {
                    if (!RecipeCompiler.Instance.DPoint.ContainsKey(item.Key))
                    {
                        RecipeCompiler.Instance.DPoint.Add(item.Key, new List<XYZPoint>());
                    }
                    if (!RecipeCompiler.Instance.ProductEX.ContainsKey(item.Key))
                    {
                        RecipeCompiler.Instance.ProductEX.Add(item.Key, new ProductEX() {  });
                        string da = ProjectINI.ClassToJsonString(RecipeCompiler.Instance.DPoint[item.Key]);
                        List<XYZPoint> xYZPoints = new List<XYZPoint>();
                        ProjectINI.StringJsonToCalss<List<XYZPoint>>(da, out xYZPoints);
                        RecipeCompiler.Instance.ProductEX[item.Key].DPoint = xYZPoints;
                    }

                    if (RecipeCompiler.Instance.ProductEX[item.Key].DPoint.Count != RecipeCompiler.Instance.DPoint.Count)
                    {
                        string da = ProjectINI.ClassToJsonString(RecipeCompiler.Instance.DPoint[item.Key]);
                        List<XYZPoint> xYZPoints = new List<XYZPoint>();
                        ProjectINI.StringJsonToCalss<List<XYZPoint>>(da, out xYZPoints);
                        RecipeCompiler.Instance.ProductEX[item.Key].DPoint = xYZPoints;
                    }
                }
                AlarmText.AddTextNewLine("同步更新位置完成");
                RecipeCompiler.Instance.DPoint = new Dictionary<string, List<XYZPoint>>();
            }
            string paths = ProjectINI.In.ProjectPathRun + "\\" + vision.Vision.Instance.FileName + "\\";
            Dictionary<string, Dictionary<string, string>> keyValuePairs = new Dictionary<string, Dictionary<string, string>>();
            foreach (var item in Product.GetThisP())
            {
                if (ProjectINI.ReadPathJsonToCalss(paths + item.Key + "\\配方参数", out Dictionary<string, string> dke))
                {
                    keyValuePairs.Add(item.Key, dke);
                }
                else
                {
                    keyValuePairs.Add(item.Key, item.Value);
                }
                if (ProjectINI.ReadPathJsonToCalss(paths + item.Key + "\\产品参数", out ProductEX productEX))
                {
                    if (!ProductEX.ContainsKey(item.Key))
                    {
                        ProductEX.Add(item.Key, productEX);
                    }
                    else
                    {
                        ProductEX[item.Key] = productEX;
                    }
                }
                else
                {
                    if (!ProductEX.ContainsKey(item.Key))
                    {
                        ProductEX.Add(item.Key, new formula.ProductEX());
                    }
                }
            }
            Produc = keyValuePairs;
            if (ErosSocket.ErosConLink.StaticCon.GetSocketClint(RecipeCompiler.Instance.GetQRLinkNmae) != null)
            {
                ErosSocket.ErosConLink.StaticCon.GetSocketClint(RecipeCompiler.Instance.GetQRLinkNmae).PassiveEvent += RecipeCompiler_PassiveEvent;
            }
            userVisionManagement = MainForm1.MainFormF.userFormulaContrsl1;
            AlarmText.LogWarning(Product.ProductionName, err);
            Data.Set();
            ErosSocket.ErosConLink.SocketClint socketClint = ErosSocket.ErosConLink.StaticCon.GetSocketClint(DataLinkName);
            if (socketClint != null)
            {
                socketClint.PassiveEvent += SocketClint_PassiveEvent;
            }
            Product.SetUserFormulaContrsl(userVisionManagement);
            try
            {
                if (File.Exists(ProjectINI.TempPath + "计数.txt"))
                {
                    string data = File.ReadAllText(ProjectINI.TempPath + "计数.txt");
                    string[] datas = data.Split(',');
                    Instance.OKNumber.OKNumber = int.Parse(datas[0]);
                    Instance.OKNumber.NGNumber = int.Parse(datas[1]);
                    Instance.OKNumber.IsOK = bool.Parse(datas[2]);
                    Instance.OKNumber.Number = int.Parse(datas[3]);
                    Instance.OKNumber.OKNG = Single.Parse(datas[4]);
                    Instance.OKNumber.AutoNGNumber = int.Parse(datas[5]);
                }
                else
                {
                    Instance.OKNumber.OKNumber = 0;
                    Instance.OKNumber.NGNumber = 0;
                    Instance.OKNumber.IsOK = false;
                    Instance.OKNumber.Number = 0;
                    Instance.OKNumber.OKNG = 0;
                }
            }
            catch (Exception)
            {
            }
        }

        private string SocketClint_PassiveEvent(byte[] key, ErosSocket.ErosConLink.SocketClint socket, System.Net.Sockets.Socket socketR)
        {
            string data = socket.GetEncoding().GetString(key);
          
                Data.AddData(data.Substring(RecipeCompiler.Instance.DataStrat));
            

            return "";
        }
        private string RecipeCompiler_PassiveEvent(byte[] key, ErosSocket.ErosConLink.SocketClint socket, System.Net.Sockets.Socket socketR)
        {
            UserFormulaContrsl.StaticAddQRCode(socket.GetEncoding().GetString(key));
            return "";
        }


        public static void ResetDATA()
        {
            Instance.OKNumber.OKNumber = 0;
            Instance.OKNumber.NGNumber = 0;
            Instance.OKNumber.Number = 0;
            Instance.OKNumber.OKNG = 0;
            Instance.OKNumber.AutoNGNumber = 0;
            Instance.OKNumber.IsOK = false;
            UserFormulaContrsl.StaticAddData(Instance.OKNumber);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsOk"></param>
        public static void AddOKNumber(bool IsOk)
        {
            if (GetUserFormulaContrsl().ListOkNumber == null)
            {
                GetUserFormulaContrsl().ListOkNumber = new List<bool>();
            }
            GetUserFormulaContrsl().ListOkNumber.Add(IsOk);
            if (IsOk)
            {
                Instance.OKNumber.OKNumber++;
            }
            else
            {
                Instance.OKNumber.NGNumber++;
            }
            Instance.OKNumber.IsOK = IsOk;
            Instance.OKNumber.Number++;
            Instance.OKNumber.OKNG = (Single)((double)Instance.OKNumber.OKNumber / (double)Instance.OKNumber.Number) * 100.0F;
            //try
            //{
            //    string data = Instance.OKNumber.OKNumber.ToString() + "," + Instance.OKNumber.NGNumber.ToString() + "," + Instance.OKNumber.IsOK + "," + Instance.OKNumber.Number + "," + Instance.OKNumber.OKNG;
            //    File.WriteAllText(ProjectINI.TempPath + "NG概率.txt", data);
            //}
            //catch (Exception)
            //{
            //}
            UserFormulaContrsl.StaticAddData(Instance.OKNumber);

        }
        public static void AddRlsNumber()
        {
            Instance.OKNumber.AutoNGNumber++;

        }
        public static void AddOKNumber(int id, bool IsOk)
        {
            try
            {
                if (GetUserFormulaContrsl().ListOkNumber == null)
                {
                    GetUserFormulaContrsl().ListOkNumber = new List<bool>();
                }
                if (GetUserFormulaContrsl().ListOkNumber.Count == 0)
                {
                    GetUserFormulaContrsl().ListOkNumber = new List<bool>(new bool[GetUserFormulaContrsl().MAXt]);
                }
                GetUserFormulaContrsl().ListOkNumber[id] = IsOk;
                if (IsOk)
                {
                    Instance.OKNumber.OKNumber++;
                }
                else
                {
                    Instance.OKNumber.NGNumber++;
                }
                Instance.OKNumber.IsOK = IsOk;
                Instance.OKNumber.Number++;
                Instance.OKNumber.OKNG = (Single)((double)Instance.OKNumber.OKNumber / (double)Instance.OKNumber.Number) * 100.0F;
                //try
                //{
                //    string data = Instance.OKNumber.OKNumber.ToString() + "," + Instance.OKNumber.NGNumber.ToString() + "," + Instance.OKNumber.IsOK + "," + Instance.OKNumber.Number + "," + Instance.OKNumber.OKNG;
                //    File.WriteAllText(ProjectINI.TempPath + "NG概率.txt", data);
                //}
                //catch (Exception)
                //{
                //}
                UserFormulaContrsl.StaticAddData(Instance.OKNumber);
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 更改复判结果
        /// </summary>
        /// <param name="isOK">结果</param>
        /// <param name="id">结果I</param>
        public static void AlterNumber(bool isOK, int id = 0)
        {
            if (RecipeCompiler.Instance.TrayQRType != RecipeCompiler.TrayEnumType.一个流程一个产品)
            {
                id = 0;
            }
            if (id == 0)
            {
                if (isOK != Instance.OKNumber.IsOK)
                {
                    if (isOK)
                    {
                        Instance.OKNumber.NGNumber--;
                        Instance.OKNumber.OKNumber++;
                        Instance.OKNumber.AutoNGNumber++;
                    }
                    else
                    {
                        Instance.OKNumber.OKNumber--;
                        Instance.OKNumber.NGNumber++;
                    }
                    Instance.OKNumber.IsOK = isOK;
                }
            }
            else if (Project.formula.RecipeCompiler.GetUserFormulaContrsl().ListOkNumber[id] != isOK)
            {
                if (isOK)
                {
                    Instance.OKNumber.NGNumber--;
                    Instance.OKNumber.OKNumber++;
                    Instance.OKNumber.AutoNGNumber++;
                }
                else
                {
                    Instance.OKNumber.OKNumber--;
                    Instance.OKNumber.NGNumber++;
                }
                RecipeCompiler.GetUserFormulaContrsl().ListOkNumber[id] = isOK;
            }
            if (Instance.OKNumber.NGNumber < 0)
            {
                Instance.OKNumber.NGNumber = 0;
            }
            Instance.OKNumber.OKNG = (Single)((double)Instance.OKNumber.OKNumber / (double)Instance.OKNumber.Number) * 100.0F;

            RecipeCompiler.GetUserFormulaContrsl().AddData(Instance.OKNumber);
        }
        public OKNumberClass OKNumber = new OKNumberClass();
        public class OKNumberClass
        {
            /// <summary>
            /// 是否OK
            /// </summary>
            public bool IsOK { get; set; }
            /// <summary>
            /// OK数量
            /// </summary>
            public int OKNumber { get; set; }
            /// <summary>
            /// NG数量
            /// </summary>
            public int NGNumber { get; set; }
            /// <summary>
            /// 总数数量
            /// </summary>
            public int Number { get; set; }
            /// <summary>
            /// 良率
            /// </summary>
            public Single OKNG { get; set; }
            /// <summary>
            /// 机器误判数量
            /// </summary>
            public int AutoNGNumber { get; set; }
        }



        public override void SaveThis(string path)
        {
            if (data1 != null)
            {
                data1.SaveThis(path+ "\\产品配方");
            }
            Product.SaveDicExcel(path + "\\产品配方\\产品文件.xls");
            string paths = ProjectINI.In.ProjectPathRun + "\\" + vision. Vision.Instance.FileName + "\\";
            foreach (var item in Produc)
            {
                ProjectINI.ClassToJsonSavePath(item.Value, paths+item.Key + "\\配方参数");
            }
            foreach (var item in Instance.ProductEX)
            {
                ProjectINI.ClassToJsonSavePath(item.Value, paths + item.Key + "\\产品参数");
            }
            //if (RecipeCompiler.Instance.ProductEX.ContainsKey(listBox1.SelectedItem.ToString()))
            //{
            //    ProductEX xYZPoints = RecipeCompiler.Instance.ProductEX[listBox1.SelectedItem.ToString()];
            //    ProjectINI.ClassToJsonSavePath(xYZPoints, fbd.SelectedPath + "\\产品参数");
            //}
            base.SaveThis(path);
        }
    }

    #endregion
}
