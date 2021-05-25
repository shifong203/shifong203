using ErosSocket.ErosConLink;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Excel;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using static ErosSocket.ErosConLink.SocketClint;

namespace Vision2.Project.formula
{
    /// <summary>
    /// 产品管理类，包含产品集合Dictionary
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 新建产品带名称，添加到集合
        /// </summary>
        /// <param name="name"></param>
        public Product(string name)
        {
            Add(name, this);
            Name = name;
        }
        /// <summary>
        /// 新建产品添加到集合
        /// </summary>
        public Product()
        {


        }

        /// <summary>
        /// 不添加到集合不新建产品
        /// </summary>
        /// <param name="isAdd">是否添加新产品</param>
        public Product(bool isAdd)
        {
            if (isAdd)
            {
                string name = "产品1";
                foreach (var item in ThisDic)
                {
                    name = item.Key;
                    break;
                }
            st:
                if (ThisDic.ContainsKey(name))
                {
                    int idx = Vision2.ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name, out name);
                    name = name + (idx + 1);
                    goto st;
                }
                string nameStr = Interaction.InputBox("请输入产品名", "新建产品", name, 100, 100);
                if (nameStr.Length == 0)
                {
                    return;
                }

                Add(nameStr, this);
            }
        }

        /// <summary>
        /// True不可以加载产品
        /// </summary>
        [Description("是否可以切换"), Category("加载项"), DisplayName("是否可以加载产品")]
        public static bool IsSwitchover { get; set; }
        [Description("产品唯一名称"), Category("产品信息"), DisplayName("产品名称")]
        public string Name { get; set; }
        [Description("产品唯一编号"), Category("产品信息"), DisplayName("产品编号")]
        public string ProductID { get; set; }
        [Description("产品唯一ID"), Category("产品信息"), DisplayName("产品ID")]
        public string ID { get; set; }
        [Description("尺寸信息文本格式"), Category("产品信息"), DisplayName("尺寸信息")]
        public string Size { get; set; }
        [Description("产品显示的图片"), Category("产品信息"), DisplayName("显示产品图片"),
            EditorAttribute(typeof(Vision2.ErosProjcetDLL.UI.PropertyGrid.PageTypeEditor_OpenFileDialog), typeof(System.Drawing.Design.UITypeEditor))]
        public string ImagePath
        {
            get
            {
                Vision2.ErosProjcetDLL.UI.PropertyGrid.PageTypeEditor_OpenFileDialog.Filter
                    = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
                return imagePath;
            }
            set { imagePath = value; }
        }
        string imagePath = "";

        [Description("多个产品图片地址组"), Category("产品信息"), DisplayName("产品图片组")]
        [Editor(typeof(Vision2.ErosProjcetDLL.UI.PropertyGrid.ListStringEditor), typeof(UITypeEditor))]
        public List<string> ListImages { get; set; } = new List<string>();

        [Description("首关键字>表示从第一个，<表示从结尾开始，无关键字表示只要存在；ProductName匹配配方名，或其他固定字符"), Category("物料信息"), DisplayName("二维码匹配"),
        TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", false, ">ProductName", "ProductName", "<ProductName")]
        public string QRCODE { get; set; } = "ProductName";

        /// <summary>
        /// 匹配物料
        /// </summary>
        /// <param name="qRCode">关键二维码</param>
        /// <returns>true成功，false失败</returns>
        public static bool MatchTheMaterial(string qRCode)
        {
            if (GetProduct().QRCODE.StartsWith(">"))
            {
                if (GetProduct().QRCODE == ">ProductName")
                {
                    if (qRCode.StartsWith(Product.ProductionName))
                    {
                        return true;
                    }
                }
                else
                {
                    if (qRCode.StartsWith(GetProduct().QRCODE.Remove(0, 1)))
                    {
                        return true;
                    }
                }
            }
            else if (GetProduct().QRCODE.StartsWith("<"))
            {
                if (GetProduct().QRCODE == "<ProductName")
                {
                    if (qRCode.EndsWith(Product.ProductionName))
                    {
                        return true;
                    }
                }
                else
                {
                    if (qRCode.EndsWith(GetProduct().QRCODE))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (GetProduct().QRCODE == "ProductName")
                {
                    if (qRCode.Contains(Product.ProductionName))
                    {
                        return true;
                    }
                }
                else
                {
                    if (qRCode.Contains(GetProduct().QRCODE))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 关联显示控件
        /// </summary>
        /// <param name="userFormula"></param>
        public static void SetUserFormulaContrsl(UserFormulaContrsl userFormula)
        {
            userFormula.Name = "配方";
            if (!MainForm1.MainFormF.panel2.Controls.Contains(userFormula))
            {
                MainForm1.MainFormF.panel2.Controls.Add(userFormula);
            }
            userFormula.Dock = DockStyle.Fill;
            userVisionManagement = userFormula;
            userVisionManagement.toolStripLabel1.Text = "当前生产:" + ProductionName;
            userVisionManagement.comboBox1.Items.Clear();
            userVisionManagement.comboBox1.Items.AddRange(Product.GetThisP().Keys.ToArray());
            userVisionManagement.comboBox1.SelectedItem = ProductionName;
        }

        static UserFormulaContrsl userVisionManagement;
        /// <summary>
        /// 自动切换产品配方
        /// <param name="productName">选择的产品名称</param>
        /// <paramref name="lingks">连接集合</paramref>
        /// </summary>
        public static bool Aotu(string productName, Dictionary<string, ErosSocket.ErosConLink.SocketClint> lingks)
        {
            string err = "";
            if (ValuePairs.ContainsKey(productName))
            {
                int errN = 0;
                string errStr = "";

                userVisionManagement.UPSetGetPargam();

                if (vision.Vision.Instance.ISPName)
                {
                    vision.Vision.UpReadThis(ProductionName);
                }
                //if (userVisionManagement != null)
                //{
                //    userVisionManagement.toolStripLabel1.Text = "当前生产:" + ProductionName;
                //    userVisionManagement.comboBox1.SelectedItem = ProductionName;
                //}

                ErosSocket.ErosConLink.IErosLinkNet Linl;
                foreach (var item in GetListLinkNames)
                {
                    if (lingks.ContainsKey(item))
                    {
                        Linl = lingks[item];
                    }
                    else
                    {
                        MessageBox.Show("写入配方失败，" + item + "连接不存在");
                        continue;
                    }
                    ///地址名
                    List<string> names = new List<string>();
                    ///参数名
                    List<string> pramName = new List<string>();
                    ///值
                    List<string> Values = new List<string>();

                    if (Linl.Split_Mode != ErosSocket.ErosConLink.SocketClint.SplitMode.Array)
                    {
                        foreach (var item2 in GetProduct().Parameter_Dic.ParameterMap[item])
                        {
                            if (item2.Value.Trim(' ') == "")
                            {
                                continue;
                            }

                            if (GetProd(productName).ContainsKey(item2.Key))
                            {
                                pramName.Add(item2.Key);
                                names.Add(item2.Value);
                                Values.Add(GetProd(productName)[item2.Key]);
                                if (UClass.GetTypeList().Contains(GetParameters()[item2.Key].TypeStr))
                                {
                                    if (Linl.IsConn)
                                    {
                                        Linl.SetIDValue(item2.Value, GetParameters()[item2.Key].TypeStr,
                                                             GetProd(productName)[item2.Key], out err);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var te = from de in GetProduct().Parameter_Dic.ParameterMap[item]
                                 where de.Value.StartsWith("array[")
                                 orderby de.Value.Substring(6, de.Value.LastIndexOf(']') - 6)
                                 select de;

                        foreach (var item2 in te)
                        {
                            Values.Add(GetProd(productName)[item2.Key]);
                        }
                        if (Linl.Split_Mode == ErosSocket.ErosConLink.SocketClint.SplitMode.Array)
                        {
                            Linl.SetValues(names.ToArray(), Values.ToArray(), out err);
                        }
                    }
                    errStr += err;
                    Thread.Sleep(200);
                    if (Linl.KeysValues != null)
                    {
                        for (int i = 0; i < names.Count; i++)
                        {
                            if (ErosSocket.ErosConLink.UClass.GetTypeList().Contains(GetParameters()[pramName[i]].TypeStr))
                            {
                                if (Linl.IsConn)
                                {
                                    Linl.GetIDValue(names[i], GetParameters()[pramName[i]].TypeStr, out dynamic dynamic);
                                    string dam = dynamic.ToString();
                                    if (!dam.StartsWith(Values[i]))
                                    {
                                        errN++;
                                        errStr += names[i] + "值未更改，目标值:" + Values[i] + "当前值:" + Linl.KeysValues[names[i]].ToString() + ";";
                                    }
                                }
                            }
                        }
                        if (errN > 0)
                        {
                            errStr = Linl.Name + string.Format("包含{0}个值写入失败;" + Environment.NewLine, errN) + errStr;
                        }
                    }
                    if (Linl is ErosSocket.ErosConLink.SocketClint)
                    {
                        ErosSocket.ErosConLink.SocketClint itnem = Linl as ErosSocket.ErosConLink.SocketClint;
                        if (itnem.PLCRun != null)
                        {
                            if (itnem.PLCRun.LinkHCIF != null && itnem.PLCRun.LinkCOn != null && itnem.PLCRun.LinkIDname != null &&
                                itnem.PLCRun.LinkHCIF != "" && itnem.PLCRun.LinkCOn != "" && itnem.PLCRun.LinkIDname != "")
                            {
                                if (itnem.KeysValues[itnem.PLCRun.LinkHCIF])
                                {
                                    itnem.SetValue(itnem.PLCRun.LinkIDname, Product.GetProd(productName)["产品ID"], out err);
                                    if (err != "")
                                    {
                                        errStr += err;
                                    }
                                    itnem.SetValue(itnem.PLCRun.LinkCOn, true.ToString(), out err);
                                    if (err != "")
                                    {
                                        errStr += err;
                                    }
                                }
                                else
                                {
                                    errStr += "加载失败:" + itnem.Name + "换型条件不满足！";
                                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine("加载失败:" + itnem.Name + "换型条件不满足！");
                                }
                            }
                        }
                    }
                }
                if (errStr == "")
                {
                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(productName + ",产品加载成功!");
                    ProductionName = productName;
                    return true;
                }
                else
                {
                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(productName + ",加载失败:" + errStr, System.Drawing.Color.Red);
                }
            }
            else
            {
                Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("加载失败:不存在的产品" + productName);
            }
            return false;
        }
        /// <summary>
        /// 自动切换产品配方
        /// <param name="productName">选择的产品名称</param>
        /// </summary>
        public static bool Aotu(string productName)
        {
            return Aotu(productName, ErosSocket.ErosConLink.DicSocket.Instance.SocketClint);
        }

        public static bool SetGetPLCPrgream(IErosLinkNet erosLink, Dictionary<string, string> prgaem)
        {
            try
            {
                int errI = 0;
                foreach (var item in prgaem)
                {
                    string err = "";
                    erosLink.SetIDValue(GetProduct().Parameter_Dic.ParameterMap[erosLink.Name][item.Key], GetParameters()[item.Key].TypeStr,
                          item.Value, out err);
                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(err);
                    erosLink.GetIDValue(GetProduct().Parameter_Dic.ParameterMap[erosLink.Name][item.Key], GetParameters()[item.Key].TypeStr, out dynamic value);
                    UClass.GetTypeValue(GetParameters()[item.Key].TypeStr, item.Value, out dynamic valutS);

                    if (value != valutS)
                    {
                        errI++;
                        Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("参数值加载错误;" + item.Key + "/Set[" + item.Value + "]Get[" + value.ToString() + "]");
                    }
                }
                if (errI == 0)
                {
                    return true;
                }

            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 当前生产的产品
        /// </summary>
        [DescriptionAttribute("当前生产的产品。"), Category("生产信息"), DisplayName("当前加工产品")]
        public static string ProductionName
        {
            get { return productionName; }
            set
            {
                if (productionName != value)
                {
                    try
                    {
                        if (userVisionManagement != null)
                        {
                            userVisionManagement.toolStripLabel1.Text = "当前生产:" + value;
                            userVisionManagement.comboBox1.Items.Add(value);
                            userVisionManagement.comboBox1.SelectedItem = value;
                        }

                        File.WriteAllText(Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\产品配方\\生产产品", value);

                    }
                    catch (Exception)
                    {
                    }
                }
                productionName = value;
            }

        }

        static string productionName = "";
        /// <summary>
        /// 参数集合
        /// </summary>
        [DescriptionAttribute("参数集合。"), Category("参数"), DisplayName("参数集合"), Browsable(false)]
        public ParameterDic Parameter_Dic { get; set; } = new ParameterDic();
        /// <summary>
        ///  判断值是否在最大与最小之间
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        /// <param name="codeString">输出字符</param>
        /// <returns></returns>
        public static bool GetParameterMaxMin(string parameterName, double value, ref string codeString)
        {
            try
            {
                string[] maxmin1 = GetProduct().Parameter_Dic[parameterName + "[Min,Max]"].Split(',');

                if (double.Parse(maxmin1[0]) > value || double.Parse(maxmin1[1]) < value)
                {
                    codeString += parameterName + ",";
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        /// <summary>
        ///  判断值是否在最大与最小之间
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        /// <param name="codeString">输出字符</param>
        /// <returns></returns>
        public static bool GetParameterMaxMin(string parameterName, string value, ref string codeString)
        {
            try
            {
                return GetParameterMaxMin(parameterName, double.Parse(value), ref codeString);
            }
            catch (Exception)
            {
            }
            return false;

        }
        /// <summary>
        /// 链接名集合
        /// </summary>
        [DescriptionAttribute("多个设备连接名称，加载的参数可能存在多个设备。"), Category("参数"),
            DisplayName("链接名集合")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameListControl.Editor), typeof(UITypeEditor))]
        public List<string> ListLinkNames
        {
            get
            {
                if (listLinkNames == null)
                {
                    listLinkNames = new List<string>();
                }
                return listLinkNames;
            }
            set
            {
                if (listLinkNames == null)
                {
                    listLinkNames = new List<string>();
                }
                listLinkNames = value;
            }
        }

        static List<string> listLinkNames = new List<string>();
        /// <summary>
        /// 获得连接集合名称
        /// </summary>
        public static List<string> GetListLinkNames
        {
            get { return listLinkNames; }
        }
        /// <summary>
        /// 参数配置集合
        /// </summary>
        static Dictionary<string, Product> ThisDic = new Dictionary<string, Product>();



        /// <summary>
        /// 参数值集合
        /// </summary>
        static Dictionary<string, Dictionary<string, string>> ValuePairs = new Dictionary<string, Dictionary<string, string>>();


        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="name"></param>
        /// <param name="product"></param>
        public static void Add(string name, Product product)
        {
            Start:
            string das = name;
            if (ValuePairs.ContainsKey(name))
            {
            st:
                int idx = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name, out name);
                name = name + (idx + 1);
                if (ValuePairs.ContainsKey(name))
                {
                    goto st;
                }
                name = Interaction.InputBox(das + "名称已存在，是否重新创建名称", "创建名称", name, 100, 100);
                if (name.Length == 0)
                {
                    return;
                }
                goto Start;
            }
            else
            {
                ValuePairs.Add(name, new Dictionary<string, string>());
                foreach (var item in ThisDic["M1"].Parameter_Dic.GetParameters())
                {
                    ValuePairs[name].Add(item.Key, item.Value.ValueStr);
                }
            }
            Product.ProductionName = name;
        }

        public static void Add(string name,string NewName)
        {
           Start:
            string das = NewName;
            if (ValuePairs.ContainsKey(NewName))
            {
            st:
                int idx = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(NewName, out NewName);
                NewName = NewName + (idx + 1);
                if (ValuePairs.ContainsKey(NewName))
                {
                    goto st;
                }
                NewName = Interaction.InputBox(das + "名称已存在，是否重新创建名称", "创建名称", NewName, 100, 100);
                if (NewName.Length == 0)
                {
                    return;
                }
                goto Start;
            }
            else
            {
                ValuePairs.Add(NewName, new Dictionary<string, string>());

                foreach (var item in GetProd(name))
                {
                    ValuePairs[NewName].Add(item.Key, item.Value);
                }
            }

        }

        /// <summary>
        /// 修改程序名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newNmae"></param>
        public static string AmendName(string name)
        {
            string das = name;
            string newNmae = "";
            string mesage = "修改程序名";
            if (ValuePairs.ContainsKey(name))
            {
                newNmae = name;
            st:
                int idx = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(newNmae, out newNmae);
                newNmae = newNmae + (idx + 1);
                if (ValuePairs.ContainsKey(newNmae))
                {
                    goto st;
                }
                newNmae = Interaction.InputBox(das + "请输入新名称", mesage, newNmae, 100, 100);
                if (newNmae.Length == 0)
                {
                    return "";
                }
                if (ValuePairs.ContainsKey(newNmae))
                {
                    mesage = "程序已存在，请重新输入";
                    goto st;
                }
                ValuePairs.Add(newNmae, ValuePairs[name]);
                ValuePairs.Remove(name);

                return newNmae;
            }
            return "";
        }
        /// <summary>
        /// 获取指定名的产品参数配置,不指定名时返回默认名参数配置
        /// </summary>
        /// <param name="name">产品名</param>
        /// <returns></returns>
        public static Product GetProduct(string name = null)
        {
            if (name == null)
            {
                return GetProduct("M1");
            }
            if (ThisDic.ContainsKey(name))
            {
                if (ThisDic[name].Parameter_Dic.ParameterMap == null)
                {
                    ThisDic[name].Parameter_Dic.ParameterMap = new Dictionary<string, Dictionary<string, string>>();
                }
                if (ThisDic[name].Parameter_Dic == null)
                {
                    ThisDic[name].Parameter_Dic = new ParameterDic();
                }
                return ThisDic[name];
            }
            ThisDic.Add(name, new Product(false) { Name = name, });
            return null;
        }
        /// <summary>
        /// 获取指定名的产品参数配置,不指定名时返回默认参数配置
        /// </summary>
        /// <param name="name">产品名</param>
        /// <returns></returns>
        public static Dictionary<string, StructTypeValue> GetParameters(string name = null)
        {
            if (name == null)
            {
                return GetProduct("M1").Parameter_Dic.GetParameters();
            }
            if (ThisDic.ContainsKey(name))
            {
                if (ThisDic[name].Parameter_Dic.ParameterMap == null)
                {
                    ThisDic[name].Parameter_Dic.ParameterMap = new Dictionary<string, Dictionary<string, string>>();
                }
                if (ThisDic[name].Parameter_Dic == null)
                {
                    ThisDic[name].Parameter_Dic = new ParameterDic();
                }
                return ThisDic[name].Parameter_Dic.GetParameters();
            }
            return null;
        }
        /// <summary>
        /// 获得配方参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetProd(string name = null)
        {
            if (name == null)
            {
                return GetProd(ProductionName);
            }

            if (ValuePairs.ContainsKey(name))
            {
                return ValuePairs[name];
            }
            else
            {
                ValuePairs.Add(name, new Dictionary<string, string>());
                if (GetProduct() != null)
                {
                    foreach (var item in GetProduct().Parameter_Dic.GetParameters())
                    {
                        ValuePairs[name].Add(item.Key, item.Value.ValueStr);
                    }
                    return ValuePairs[name];
                }
                return ValuePairs[name];
            }
            return null;
        }

        /// <summary>
        /// 返回当前产品参数,或指定名参数
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetProductParm(string name = null)
        {
            if (name == null)
            {
                return GetProd(ProductionName);
            }
            else
            {
                return GetProd(name);
            }
        }
        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="name"></param>
        public static void Remove(string name)
        {
            if (ValuePairs.ContainsKey(name))
            {
                ValuePairs.Remove(name);
            }
        }
        /// <summary>
        /// 获得产品集合
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Product> GetThisDic()
        {
            Dictionary<string, Product> keyValuePairs = new Dictionary<string, Product>();
            foreach (var item in ThisDic)
            {
                keyValuePairs.Add(item.Value.Name, item.Value);
            }
            ThisDic = keyValuePairs;
            return ThisDic;
        }
        /// <summary>
        /// 获得产品集合
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> GetThisP(Dictionary<string, Dictionary<string, string>> keyValuePairs = null)
        {
            if (keyValuePairs != null)
            {
                ValuePairs = keyValuePairs;
            }
            return ValuePairs;
        }
        /// <summary>
        /// 保存产品信息到Excle
        /// </summary>
        /// <param name="path"></param>
        public static void SaveDicExcel(string path)
        {
            try
            {

                GetThisDic();
                RecipeCompiler.Instance.Produc = Product.GetThisP();
                string name = Path.GetFileNameWithoutExtension(path);
                string nameP = Path.GetDirectoryName(path);
                Directory.CreateDirectory(nameP);
                File.WriteAllText(nameP + "\\生产产品", ProductionName);
                if (File.Exists(nameP + "\\" + name))
                {
                    File.Delete(nameP + "\\" + name);
                }
                foreach (var item in ValuePairs)
                {
                    File.AppendAllText(nameP + "\\" + name, item.Key + Environment.NewLine);
                }
                return;


                foreach (var item in ValuePairs)
                {
                    File.AppendAllText(nameP + "\\" + name, item.Key + Environment.NewLine);
                    //Npoi.AddRosWriteToExcel(path, "产品信息", item.Key);
                    string itmePaht = Path.GetDirectoryName(path);
                    itmePaht += "\\" + item.Key + ".xls";
                    if (File.Exists(itmePaht))
                    {
                        File.Delete(itmePaht);
                    }
                    List<string> kayName = new List<string>();
                    kayName.Add("参数");
                    kayName.Add("值");
                    Npoi.AddWriteColumnToExcel(itmePaht, "参数信息", kayName.ToArray());

                    foreach (var item1 in item.Value)
                    {
                        List<string> values = new List<string>();
                        values.Add(item1.Key);
                        values.Add(item1.Value);
                        Npoi.AddRosWriteToExcel(itmePaht, "参数信息", values.ToArray());
                    }
                }
                //MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {

                MessageBox.Show("保存失败:" + ex.Message);
            }
        }
        /// <summary>
        /// 保存产品配置信息信息到Excle
        /// </summary>
        /// <param name="path"></param>
        public static void SaveDicPrExcel(string path)
        {
            try
            {
                GetThisDic();
                //if (File.Exists(path))
                //{
                //    File.Delete(path);
                //}

                foreach (var item in ThisDic)
                {
                    //Npoi.WriteObjDisplayNameToExcel(item.Value, "产品信息", path);
                    string itmePaht = Path.GetDirectoryName(path);
                    itmePaht += "\\" + item.Key + ".xls";
                    if (File.Exists(itmePaht))
                    {
                        File.Delete(itmePaht);
                    }
                    List<string> kayName = new List<string>();
                    kayName.Add("参数");
                    kayName.Add("值");
                    kayName.Add("类型");
                    kayName.Add("最大最小值");
                    kayName.Add("描述");
                    if (item.Value.Parameter_Dic.ParameterMap == null)
                    {
                        item.Value.Parameter_Dic.ParameterMap = new Dictionary<string, Dictionary<string, string>>();
                    }
                    kayName.AddRange(item.Value.Parameter_Dic.ParameterMap.Keys);
                    Npoi.AddWriteColumnToExcel(itmePaht, "参数信息", kayName.ToArray());
                    if (item.Value.Parameter_Dic == null)
                    {
                        item.Value.Parameter_Dic = new ParameterDic();
                    }
                    foreach (var item1 in item.Value.Parameter_Dic.GetParameters())
                    {
                        List<string> values = new List<string>();
                        values.Add(item1.Key);
                        values.Add(item1.Value.ValueStr);
                        values.Add(item1.Value.TypeStr);
                        values.Add(item1.Value.minValue + "," + item1.Value.MaxValue);
                        values.Add(item1.Value.Pst);
                        foreach (var itemt in item.Value.Parameter_Dic.ParameterMap)
                        {
                            if (itemt.Value.ContainsKey(item1.Key))
                            {
                                values.Add(itemt.Value[item1.Key]);
                            }
                            else
                            {
                                values.Add("");
                            }
                        }
                        Npoi.AddRosWriteToExcel(itmePaht, "参数信息", values.ToArray());
                    }
                }
                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ex.Message);
            }
        }
        /// <summary>
        /// 读取Excel文件到集合
        /// </summary>
        public static bool ReadExcelDic(string fileName, out string err)
        {
            err = "";
            try
            {
                if (fileName == null || fileName == "")
                {
                    err += "文件地址为空";
                    return false;
                }
                string name = Path.GetFileNameWithoutExtension(fileName);
                string nameP = Path.GetDirectoryName(fileName) + "\\" + name;
                if (Path.GetExtension(fileName) == ".txt")
                {
                    nameP = fileName;
                }

                string[] dataNames = new string[] { };

                if (File.Exists(nameP))
                {
                    dataNames = File.ReadAllLines(nameP);
                }
                else
                {
                    nameP += ".txt";
                    if (File.Exists(nameP))
                    {
                        dataNames = File.ReadAllLines(nameP);
                    }

                }


                //System.Data.DataTable dataTable = Npoi.ReadExcelFile(nameP + "\\" + name + ".txt", "产品信息");
                if (dataNames == null)
                {
                    err += "未读取到产品信息";
                    return false;
                }
                Dictionary<string, Dictionary<string, string>> keyValuePairs = new Dictionary<string, Dictionary<string, string>>();
                //string[] dataNames = GetColumnsByDataTable(dataTable);
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                for (int i = 0; i < dataNames.Length; i++)
                {
                    keyValues.Add(dataNames[i], "null");
                }
                if (RecipeCompiler.Instance.Produc.Count == 0)
                {
                    foreach (var item in dataNames)
                    {
                        Product product = new Product(false);
                        product.Name = item;

                        for (int i = 0; i < Product.GetListLinkNames.Count; i++)
                        {
                            if (Product.GetListLinkNames[i] == "" || Product.listLinkNames.Contains(Product.GetListLinkNames[i]))
                            {
                                Product.listLinkNames.RemoveAt(i);
                            }
                        }
                        if (keyValuePairs.ContainsKey(product.Name))
                        {
                            err += "读取失败！文件存在重复的对象名: " + product.Name + Environment.NewLine;
                            MessageBox.Show("读取失败！文件存在重复的对象名:" + product.Name);
                        }
                        else
                        {
                            keyValuePairs.Add(product.Name, new Dictionary<string, string>());
                        }
                        string path = Path.GetDirectoryName(fileName);
                        ///开始读取参数
                        path += "\\" + product.Name + ".xls";
                        System.Data.DataTable dataTable2 = Npoi.ReadExcelFile(path, "参数信息");
                        if (dataTable2 == null)
                        {
                            err += product.Name + "参数信息不存在;" + Environment.NewLine;
                        }
                        else
                        {
                            foreach (System.Data.DataRow item1 in dataTable2.Rows)
                            {
                                keyValuePairs[product.Name].Add(item1[0].ToString(), item1[1].ToString());
                            }
                        }
                    }
                    ValuePairs = keyValuePairs;
                }
                else
                {
                    ValuePairs = RecipeCompiler.Instance.Produc;
                }

                err += "产品信息读取完成;";
                string pathT = Path.GetDirectoryName(fileName);
                System.Data.DataTable dataTable3 = Npoi.ReadExcelFile(pathT + "\\产品配方管理\\M1.xls", "参数信息");
                Dictionary<string, Product> keyValuePairsT = new Dictionary<string, Product>();
                Product product2 = new Product(false);

                product2.Name = "M1";
                if (dataTable3 == null)
                {
                }
                else
                {
                    product2.Parameter_Dic = new ParameterDic();

                    for (int i = 5; i < dataTable3.Columns.Count; i++)
                    {
                        product2.Parameter_Dic.ParameterMap.Add(dataTable3.Columns[i].ColumnName, new Dictionary<string, string>());
                    }
                    foreach (System.Data.DataRow item1 in dataTable3.Rows)
                    {
                        try
                        {
                            string[] st;
                            if (item1[3].ToString().Contains(','))
                            {
                                st = item1[3].ToString().Split(',');
                            }
                            else
                            {
                                st = new string[] { "", "" };
                            }


                            product2.Parameter_Dic.SetKet(item1[0].ToString(), item1[1].ToString(), item1[2].ToString(), st[0], st[1], item1[4].ToString());

                            for (int i = 5; i < dataTable3.Columns.Count; i++)
                            {
                                if (product2.Parameter_Dic.ParameterMap[dataTable3.Columns[i].ColumnName].ContainsKey(item1[0].ToString()))
                                {
                                    product2.Parameter_Dic.ParameterMap[dataTable3.Columns[i].ColumnName][item1[0].ToString()] = item1[i].ToString();
                                }
                                else
                                {
                                    product2.Parameter_Dic.ParameterMap[dataTable3.Columns[i].ColumnName].Add(item1[0].ToString(), item1[i].ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    keyValuePairsT.Add(product2.Name, product2);
                }
                Product.GetListLinkNames.AddRange(product2.Parameter_Dic.ParameterMap.Keys.ToArray());
                ThisDic = keyValuePairsT;
                if (File.Exists(Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\产品配方\\生产产品"))
                {
                    Product.ProductionName = File.ReadAllText(Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\产品配方\\生产产品");
                    if (!dataNames.Contains(Product.ProductionName))
                    {
                        Product.ProductionName = dataNames[dataNames.Length-1];
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取产品信息失败!异常:" + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 根据DataTable获得列名
        /// </summary>
        /// <param name="dt">表对象</param>
        /// <returns>返回结果的数据列数组</returns>
        public static string[] GetColumnsByDataTable(System.Data.DataTable dt)
        {
            string[] strColumns = null;
            if (dt.Columns.Count > 0)
            {
                int columnNum = 0;
                columnNum = dt.Columns.Count;
                strColumns = new string[columnNum];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    strColumns[i] = dt.Columns[i].ColumnName;
                }
            }
            return strColumns;
        }
        /// <summary>
        /// 返回给通信配方参数
        /// </summary>
        public static string GetFormulaData(string name, string Identifying, char split, SplitMode Split_Mode)
        {
            List<string> keys = new List<string>();
            List<string> values = new List<string>();
            string datastr = "";

            if (Project.formula.Product.GetProduct().Parameter_Dic == null)
            {
                Project.formula.Product.GetProduct().Parameter_Dic = new Project.formula.Product.ParameterDic();
                datastr = "Err:产品[" + Project.formula.Product.ProductionName + "]未添加参数";
            }
            if (Project.formula.Product.GetProduct().Parameter_Dic.ParameterMap.ContainsKey(name))
            {

                foreach (var item in Project.formula.Product.GetProduct().Parameter_Dic.ParameterMap[name])
                {
                    if (item.Value.Contains("arr"))
                    {
                        keys.Add(item.Key);
                        values.Add(Project.formula.Product.GetProd()[item.Key]);
                    }
                }
                datastr = Identifying;
                char Fs = '=';
                switch (Split_Mode)
                {
                    case SplitMode.Array:

                        datastr += String.Join(split.ToString(), values);
                        break;
                    case SplitMode.KeyValue:
                        for (int i = 0; i < keys.Count; i++)
                        {
                            datastr += keys[i] + Fs + values[i] + split;
                        }
                        break;
                }
            }
            else
            {
                datastr = "err：链接名未配置" + Identifying;
            }
            return datastr;
        }

        /// <summary>
        /// 值与类型结构
        /// </summary>
        public class StructTypeValue
        {
            /// <summary>
            /// 值类型
            /// </summary>
            public string TypeStr;
            /// <summary>
            /// 默认值
            /// </summary>
            public string ValueStr;
            /// <summary>
            /// 参数描述
            /// </summary>
            public string Pst;
            /// <summary>
            /// 最小值
            /// </summary>
            public string minValue;
            /// <summary>
            /// 最大值
            /// </summary>
            public string MaxValue;
            /// <summary>
            /// 值枚举选项
            /// </summary>
            public string ValueEn;
        }
        /// <summary>
        /// 
        /// </summary>
        public class ParameterDic
        {
            /// <summary>
            /// 配方参数，键参数，值参数值
            /// </summary>
            Dictionary<string, StructTypeValue> Parameters = new Dictionary<string, StructTypeValue>();
            ///// <summary>
            ///// 类型
            ///// </summary>
            //public Dictionary<string, string> ParameterTypes = new Dictionary<string, string>();
            /// <summary>
            /// 装载变量映射，第一键连接名，第二键参数名，第三值连接地址
            /// </summary>
            public Dictionary<string, Dictionary<string, string>> ParameterMap = new Dictionary<string, Dictionary<string, string>>();
            /// <summary>
            /// 读取参数,不存在则在提升框显示错误并返回字符串“null”
            /// </summary>
            /// <param name="index">参数名</param>
            /// <returns>参数值</returns>
            public string this[string index]
            {
                get
                {
                    if (Parameters.ContainsKey(index))
                    {
                        return Parameters[index].ValueStr;
                    }
                    else
                    {
                        Vision2.ErosProjcetDLL.Project.AlarmText.LogErr("当前配方不存在参数:" + index, "配方参数");
                        return "null";
                    }
                }
            }
            /// <summary>
            /// 设置参数
            /// </summary>
            /// <param name="key">参数名</param>
            /// <param name="values">值</param>
            /// <param name="types">类型</param>
            /// <param name="minValue">最小值</param>
            /// <param name="maxValue">最大值</param>
            /// <param name="ps">描述</param>
            public void SetKet(string key, string values, string types, string minValue = null, string maxValue = null, string ps = null)
            {
                if (Parameters.ContainsKey(key))
                {
                    Parameters[key].ValueStr = values;
                    Parameters[key].TypeStr = types;
                }
                else
                {
                    Parameters.Add(key, new StructTypeValue() { ValueStr = values, TypeStr = types });
                }
                if (ps != null)
                {
                    Parameters[key].Pst = ps;
                }
                if (maxValue != null)
                {
                    Parameters[key].MaxValue = maxValue;
                }
                if (minValue != null)
                {
                    Parameters[key].minValue = minValue;
                }
            }

            /// <summary>
            /// 获取配方参数，键参数，值参数值
            /// </summary>
            /// <returns></returns>
            public Dictionary<string, StructTypeValue> GetParameters()
            {
                return Parameters;
            }
            /// <summary>
            /// 返回参数与值
            /// </summary>
            /// <returns></returns>
            public Dictionary<string, string> GetKeyValue()
            {
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                foreach (var item in Parameters)
                {
                    keyValuePairs.Add(item.Key, item.Value.ValueStr);
                }
                return keyValuePairs;
            }
        }

    }

}
