using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Vision2.ErosProjcetDLL.Project;

namespace ErosSocket.ErosConLink
{
    public class UClass
    {
        /// <summary>
        /// 变量接口
        /// </summary>
        public interface UserType
        {
        }
        public const string Boolean = "Boolean";
        public const string Byte = "Byte";
        public const string SByte = "SByte";
        public const string Char = "Char";
        public const string Decimal = "Decimal";
        public const string Double = "Double";
        public const string Single = "Single";
        public const string Int16 = "Int16";
        public const string UInt16 = "UInt16";
        public const string Int32 = "Int32";
        public const string UInt32 = "UInt32";
        public const string Int64 = "Int64";
        public const string UInt64 = "UInt64";
        public const string String = "String";

        /// <summary>
        /// 根据字符创建类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetTypeByString(string type)
        {
            if (type != "")
            {
                return Type.GetType("System." + type, false, false);
            }
            return null;
        }
        /// <summary>
        /// 根据字符串和类型转换为目标值
        /// </summary>
        /// <param name="type">值类型</param>
        /// <param name="valueStr">值字符串</param>
        /// <param name="value">值类型</param>
        /// <returns>成功为true</returns>
        public static bool GetTypeValue(string type, string valueStr, out dynamic value)
        {
            value = null;
            try
            {
                if (valueStr == null)
                {
                    return false;
                }
                if (type.StartsWith("System."))
                {
                    value = Convert.ChangeType(valueStr, Type.GetType(type, false, false));
                }
                else
                {
                    value = Convert.ChangeType(valueStr, GetTypeByString(type));
                }
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    if (valueStr.ToLower() == "true" || valueStr.ToLower() == "false" && type == "Int16")
                    {
                        value = Convert.ToInt16(bool.Parse(valueStr));
                        return true;
                    }
                    else
                    {
                        if (int.TryParse(valueStr, out int dat) && type == UClass.Boolean)
                        {
                            value = Convert.ToBoolean(dat);
                            return true;
                        }

                        Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("类型转换错误:" + type + "值:" + valueStr + ex.Message, Color.Red);
                    }
                }
                catch (Exception)
                {
                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("类型转换错误:" + type + "值:" + valueStr + ex.Message, Color.Red);
                }
            }
            return false;
        }



        /// <summary>
        /// 获取类型集合
        /// </summary>
        /// <returns></returns>
        public static List<string> GetUesrListType()
        {
            List<string> list = new List<string>();
            try
            {
                list.Add("Boolean");
                list.Add("Byte");
                list.Add("SByte");
                list.Add("Char");
                list.Add("Decimal");
                list.Add("Double");
                list.Add("Single");
                list.Add("Int16");
                list.Add("UInt16");
                list.Add("Int32");
                list.Add("UInt32");
                list.Add("Int64");
                list.Add("UInt64");
                list.Add("String");
                list.Add("DateTime");
                list.Add("Guid");
                Directory.CreateDirectory(Application.StartupPath + "\\" + ErosType.constPathXML);
                var TypePath = Directory.GetFiles(Application.StartupPath + "\\" + ErosType.constPathXML);
                foreach (var item in TypePath)
                {
                    list.Add(Path.GetFileNameWithoutExtension(item));
                }
            }
            catch (Exception)
            {
            }
            return list;
        }
        /// <summary>
        /// 获取基础类型集合
        /// </summary>
        /// <returns></returns>
        public static List<string> GetTypeList()
        {
            List<string> list = new List<string>();
            list.Add("Boolean");
            list.Add("Byte");
            list.Add("SByte");
            list.Add("Char");
            list.Add("Decimal");
            list.Add("Double");
            list.Add("Single");
            list.Add("Int16");
            list.Add("UInt16");
            list.Add("Int32");
            list.Add("UInt32");
            list.Add("Int64");
            list.Add("UInt64");
            list.Add("String");
            return list;
        }
        public static byte GetTypeLentg(string typeName)
        {
            switch (typeName)
            {
                case Boolean:
                case String:
                    return 1;
                case Int16:
                case Single:
                case UInt16:
                    return 2;
                case Int32:
                case Double:
                case UInt32:
                    return 4;
                case Decimal:
                case Int64:
                case UInt64:
                    return 8;
                default:
                    break;

            }
            return 4;
        }

        /// <summary>
        /// 小数点位移
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="shift">位移数负数反向移动</param>
        /// <returns>移动后值</returns>
        public static float DecimalShift(float value, SByte shift)
        {
            if (shift == 0)
            {
                return Convert.ToSingle(value.ToString("f" + shift));
            }
            if (shift < 0)
            {
                for (int i = 0; i < Math.Abs(shift); i++)
                {
                    value = value * 10;
                }
                return Convert.ToSingle(value.ToString("f" + Math.Abs(shift)));
            }
            for (int i = 0; i < shift; i++)
            {
                value = value / 10;
            }
            return Convert.ToSingle(value.ToString("f" + shift));
        }

        /// <summary>
        /// 变量表类，包含2个变量类
        /// </summary>
        public class ErosValues
        {   /// <summary>
            /// 带地址的变量表
            /// </summary>
            public Dictionary<string, ErosValueD> DictionaryValueD;

            /// <summary>
            /// 值
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key"></param>
            /// <returns></returns>
            public delegate dynamic ValueAlter<T>(T key);
            public ErosValues()
            {
                DictionaryValueD = new Dictionary<string, ErosValueD>();

            }
            /// <summary>
            /// 带XML名的构造函数
            /// </summary>
            /// <param name="lingkID">连接名</param>
            /// <param name="xmlName">XML名称</param>
            public ErosValues(string lingkID, string xmlName)
            {
                if (xmlName == "" || xmlName == null)
                {
                    return;
                }

                DictionaryValueD = new Dictionary<string, ErosValueD>();
                this.ReadValueXML(xmlName);
            }

            public dynamic this[string idxne]
            {
                get
                {
                    if (DictionaryValueD.ContainsKey(idxne))
                    {
                        return DictionaryValueD[idxne].Value;
                    }
                    else
                    {
                        return null;
                    }
                }
                set
                {
                    if (DictionaryValueD.ContainsKey(idxne))
                    {
                        DictionaryValueD[idxne].Value = value;
                        DictionaryValueD[idxne].Name = idxne;
                    }
                    else
                    {
                        DictionaryValueD.Add(idxne, new ErosValueD() { Name = idxne, Value = value, _Type = value.GetType().Name });
                    }
                }
            }
            public bool ContainsKey(string key)
            {
                return DictionaryValueD.ContainsKey(key);
            }
            public void Add(string key, ErosValueD valueD)
            {
                if (!DictionaryValueD.ContainsKey(key))
                {
                    DictionaryValueD.Add(key, valueD);
                }
                else
                {
                    DictionaryValueD[key] = valueD;
                }
            }

            public void Remove(string key)
            {
                if (DictionaryValueD.ContainsKey(key))
                {
                    DictionaryValueD.Remove(key);
                }
            }
            /// <summary>
            /// 简单实例的变量类
            /// </summary>
            public class ErosValueD : ErosType.UType, System.Collections.IComparer
            {
                private Single dynamicT;
                bool ds;
                [Description("变量值"), Category("数据转换"), DisplayName("读取的值")]
                /// <summary>
                /// 值
                /// </summary>
                public dynamic Value
                {
                    get
                    {
                        try
                        {
                            //UpAlram(LinkID);
                            return _value;
                        }
                        catch (Exception)
                        {
                        }
                        return _value;
                    }
                    set
                    {
                        try
                        {
                            if (value == null)
                            {
                                _value = null;
                                return;
                            }
                            if (d == null)
                            {
                                if (base._Type != null && base._Type != "")
                                {
                                    if (!base._Type.StartsWith("System."))
                                    {
                                        d = Type.GetType("System." + base._Type, true, false);
                                    }
                                    else
                                    {
                                        d = Type.GetType(base._Type, true, false);
                                    }
                                }
                                else
                                {
                                    if (_value == null)
                                    {
                                        _value = value;
                                    }
                                    return;
                                }
                            }
                            if (int.TryParse(value.ToString(), out int reslt) && d == typeof(bool))
                            {
                                if (_value != Convert.ToBoolean(reslt))
                                {
                                    ValueCyEvent?.Invoke(this);
                                }
                                _value = Convert.ToBoolean(reslt);
                                ValueEquality?.Invoke(this);
                                return;
                            }
                            dynamic Time = Convert.ChangeType(value, d);
                            if (_value == null || _value.GetType() != d)
                            {
                                _value = Time;
                                ValueCyEvent?.Invoke(this);
                                ValueEquality?.Invoke(this);
                                return;
                            }
                            if (_value.GetType() == d && _value != Time)
                            {
                                _value = Time;
                                ValueCyEvent?.Invoke(this);
                            }
                            _value = Time;
                            ValueEquality?.Invoke(this);
                            return;
                        }
                        catch (Exception ex)
                        {
                            Vision2.ErosProjcetDLL.Project.AlarmText.LogErr(this.Name, ex.Message);
                        }
                    }
                }
                string sele;
                ///<summary>
                ///比较两个字符串，如果含用数字，则数字按数字的大小来比较。
                ///</summary>
                ///<param name="x"></param>
                ///<param name="y"></param>
                ///<returns></returns>
                public int Compare(Object x, Object y)
                {
                    try
                    {
                        if (x == null || y == null)
                            throw new ArgumentException("Parameters can't be null");

                        string fileA = ((ErosValueD)x).AddressID.ToString();
                        string fileB = ((ErosValueD)y).AddressID.ToString();

                        if (sele != null)
                        {
                            fileA = fileA.ToString().Remove(0, sele.Length);
                            fileB = fileB.ToString().Remove(0, sele.Length);
                        }
                        fileA = fileA.Trim('.', ' ');
                        fileB = fileB.Trim('.', ' ');
                        //char[] arr1 = fileA.ToCharArray();
                        //char[] arr2 = fileB.ToCharArray();
                        string[] dta = fileA.Split('.');
                        string[] dtb = fileB.Split('.');
                        if (dta.Length == 1)
                        {
                            if (double.Parse(fileA) > double.Parse(fileB))
                            {
                                return 1;
                            }
                            if (double.Parse(fileA) == double.Parse(fileB))
                            {
                                return 0;
                            }
                        }
                        else if (dta.Length == 2)
                        {
                            if (int.Parse(dta[0]) > int.Parse(dtb[0]))
                            {
                                return 1;
                            }
                            else if (int.Parse(dta[0]) == int.Parse(dtb[0]))
                            {
                                fileA = fileA.Remove(0, dta[0].Length + 1);
                                fileB = fileB.Remove(0, dtb[0].Length + 1);
                                if (double.Parse(fileA) > double.Parse(fileB))
                                {
                                    return 1;
                                }
                                if (double.Parse(fileA) == double.Parse(fileB))
                                {
                                    return 0;
                                }
                            }
                        }

                    }
                    catch (Exception)
                    {
                    }
                    return -1;

                }
                /// <summary>
                /// 更新变量报警触发器
                /// </summary>
                public void UpAlram(string linkID)
                {
                    if (Alarmd.Enabled)
                    {
                        string textStr = Alarmd.Text;
                    stru:
                        if (textStr.Contains("[") || textStr.Contains("]"))
                        {
                            int strat = textStr.IndexOf('[');
                            int lengt = textStr.IndexOf(']') - textStr.IndexOf('[');
                            string namev = textStr.Substring(strat + 1, lengt - 1);

                            if (namev.Contains("."))
                            {
                                namev.Split('.');
                                if (StaticCon.SocketClint.ContainsKey(namev.Split('.')[0]))
                                {
                                    textStr = textStr.Remove(strat, lengt + 1).Insert(strat, StaticCon.SocketClint[namev.Split('.')[0]].KeysValues[namev.Split('.')[1]].ToString());
                                    goto stru;
                                }
                            }
                        }

                        if (_Type != "Boolean")
                        {
                            if (double.TryParse(_value.ToString(), out double dvalue))
                            {
                                Alarmd.UPAlarm(dvalue, textStr, this.LinkID + "." + this.Name);
                            }
                            else if (int.TryParse(_value.ToString(), out int Intvalue))
                            {
                                Alarmd.UPAlarm(Intvalue, textStr, this.LinkID + "." + this.Name);
                            }
                        }
                        else
                        {
                            if ((bool)_value)
                            {
                                Alarmd.UPAlarm(1, textStr, this.LinkID + "." + this.Name);
                            }
                            else
                            {
                                Alarmd.UPAlarm(0, textStr, this.LinkID + "." + this.Name);
                            }
                        }
                    }
                }
                Type d;
                /// <summary>
                /// 初始值
                /// </summary>
                public void InitialValue()
                {
                    if (d == null)
                    {
                        if (base._Type != "")
                        {
                            d = Type.GetType("System." + base._Type, true, false);
                        }
                        else
                        {
                            d = Type.GetType(base._Type, true, false);
                        }
                    }

                    if (this.Default != null)
                    {
                        Value = Convert.ChangeType(this.Default, d);
                    }
                    else
                    {
                        Value = Activator.CreateInstance(d);
                    }
                }
                public bool InitialValue(string valueStr)
                {
                    if (d == null)
                    {
                        if (base._Type != "")
                        {
                            d = Type.GetType("System." + base._Type, true, false);
                        }
                        else
                        {
                            d = Type.GetType(base._Type, true, false);
                        }
                    }
                    if (valueStr != null)
                    {
                        try
                        {
                            Value = Convert.ChangeType(valueStr, d);
                            return true;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        Value = Activator.CreateInstance(d);
                        return true;
                    }
                    return false;
                }


                /// <summary>
                /// 设置快照值为当前值
                /// </summary>
                public void ValueSnapshot()
                {
                    if (d == null)
                    {
                        if (base._Type != "")
                        {
                            d = Type.GetType("System." + base._Type, true, false);
                        }
                        else
                        {
                            d = Type.GetType(base._Type, true, false);
                        }
                    }
                    if (this.SnapshootValueStr != null)
                    {
                        Value = Convert.ChangeType(this.SnapshootValueStr, d);
                    }
                }

                public void SetValue(dynamic value)
                {
                    _value = value;
                }
                /// <summary>
                /// 返回值类型
                /// </summary>
                /// <returns></returns>
                public Type GetValueType { get { return UClass.GetTypeByString(this._Type); } }

                dynamic _value;

                /// <summary>
                /// 值改变事件
                /// </summary>
                public event ValueAlter<ErosValueD> ValueCyEvent;

                /// <summary>
                /// 值相等
                /// </summary>
                public event ValueAlter<ErosValueD> ValueEquality;

                /// <summary>
                /// 链接ID
                /// </summary>
                public string LinkID { get; set; }
                /// <summary>
                /// 区域符号
                /// </summary>
                [Description("区域符号，指向地址区域"), Category("连接"), DisplayName("区域符号")]

                public string District { get; set; }

                [Description("整数移位小数点，正负方向移位"), Category("数据转换"), DisplayName("移位小数")]
                /// <summary>
                /// 小数点移位
                /// </summary>
                public SByte DecimalShift { get; set; }

                [Description("报警信息设置"), Category("报警信息"), DisplayName("报警信息")]
                public Vision2.ErosProjcetDLL.Project.AlarmText.Alarm Alarmd { get; set; } = new Vision2.ErosProjcetDLL.Project.AlarmText.Alarm();

                ///// <summary>
                ///// 变量类型
                ///// </summary>
                //public Dictionary<string, UserType> TypeValue
                //{ get;
                //    set; }

                public ErosValueD()
                {
                    //this._Type
                    // this.Value = -1;
                }
                public ErosValueD(string selt)
                {
                    //   this.Value = -1;
                    sele = selt;
                }

                /// <summary>
                /// 解析XML节点到实例
                /// </summary>
                /// <param name="xmlNode"></param>
                public ErosValueD(XmlNode xmlNode)
                {
                    base._Type = xmlNode.Attributes[constType].Value;
                    base.Default = xmlNode.Attributes[constDefault].Value;
                    string ds = string.Empty;
                    if (xmlNode.Attributes[constDecimalShift] != null && xmlNode.Attributes[constDecimalShift].Value != null && xmlNode.Attributes[constDecimalShift].Value != "")
                    {
                        if (sbyte.TryParse(xmlNode.Attributes[constDecimalShift].Value, out sbyte sby))
                        {
                            this.DecimalShift = sby;
                        }
                    }
                    this.AddressID = xmlNode.Attributes[constAddressID].Value;
                    //if (float.TryParse(xmlNode.Attributes[constAddressID].Value, out float df))
                    //{
                    //    this.AddressID = df;
                    //}
                    if (xmlNode.Attributes[constLinkID] != null) LinkID = xmlNode.Attributes[constLinkID].Value;
                    base.WR = xmlNode.Attributes[constWR].Value;
                    if (xmlNode.Attributes["district"] != null)
                    {
                        this.District = xmlNode.Attributes["district"].Value;
                    }
                    if (xmlNode.Attributes[constAnnotation] != null)
                    {
                        this.Annotation = xmlNode.Attributes[constAnnotation].Value;
                    }
                    if (xmlNode.Attributes[constName] != null)
                    {
                        this.Name = xmlNode.Attributes[constName].Value;
                    }
                    try
                    {
                        Type d = null;

                        if (base._Type != "")
                        {
                            d = Type.GetType("System." + base._Type, true, false);
                        }
                        else
                        {
                            d = Type.GetType("System.Int16", true, false);
                        }
                        if (d == typeof(int))
                        {
                            if (this.DecimalShift != 0)
                            {
                                ds = UClass.DecimalShift(Convert.ToSingle(xmlNode.Attributes[constValue].Value), this.DecimalShift).ToString();
                            }
                        }

                        if (d.IsValueType)
                        {
                            if (xmlNode.Attributes[constDefault].Value != "")
                            {
                                try
                                {
                                    this.SetValue(Convert.ChangeType(xmlNode.Attributes[constDefault].Value, d));
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                this.SetValue(Activator.CreateInstance(d));
                            }

                        }
                        else
                        {
                            //TypeValue = new Dictionary<string, UserType>();
                            //ErosType erosType = new ErosType(base._Type);
                            //TypeValue.Add(xmlNode.Attributes[constName].Value, erosType);
                        }

                    }
                    catch (Exception ex)
                    {
                        StaticCon.ErrerLog(ex);
                    }
                }
            }

            /// <summary>
            /// 常量
            /// </summary>
            public const string constName = "name";

            public const string constValue = "Value";
            public const string constType = "_Type";
            public const string constAddressID = "AddressID";
            public const string constSnapshot = "Snapshot";
            public const string constDefault = "Default";
            public const string constWR = "WR";
            public const string constAnnotation = "Annotation";
            public static string constPathXML = Application.StartupPath + "\\ValueS\\";
            public const string constLinkID = "LinkID";
            public const string constXmlElement = "ErosValue";
            public const string constDecimalShift = "DecimalShift";

            /// <summary>
            /// 读取XML并创建表
            /// </summary>
            /// <returns>成功返回ture</returns>
            public bool ReadValueXML(string XMLName)
            {
                string pathXML = (constPathXML + XMLName + ".xml");
                XmlDocument doc = new XmlDocument();
                try
                {
                    if (!File.Exists(pathXML))
                    {
                        //MessageBox.Show("未找到变量表文件:" + pathXML);
                        return false;
                    }
                    doc.Load(pathXML);
                    //获得根节点
                    XmlElement users = doc.DocumentElement;
                    //获得根节点下子节点
                    XmlNodeList xnl = users.ChildNodes;
                    foreach (XmlNode item in xnl)
                    {//读取集合属性
                        if (!DictionaryValueD.ContainsKey(item.Attributes[constName].Value))
                        {
                            try
                            {
                                DictionaryValueD.Add(item.Attributes[constName].Value, new ErosValueD(item));
                            }
                            catch (Exception me)
                            {
                                MessageBox.Show("读取变量表XML错误:" + item.Attributes[constName].Value + me.Message.ToString());
                            }
                        }
                    }
                    return true;
                }
                catch (Exception me)
                {
                    MessageBox.Show(me.Message.ToString());
                    return false;
                }
            }
        }

        public class PLCValue : INodeNew
        {

            public string TypeStr { get; set; }

            public string Addrea { get; set; }

            public string LinkName { get; set; }
        }
        /// <summary>
        /// 设备控制类
        /// </summary>
        public class PLCDiave
        {

            public PLCDiave()
            {
                Diave = new DiaveState();
            }
            SocketClint SC;

            public void Socket(SocketClint socketClint)
            {
                SC = socketClint;
            }
            public class DiaveState
            {
                public DiaveState(bool f = false)
                {
                    RunStateEnum = 0;
                    RunIDs = new int[20];
                    Stoping = Err = false;
                    Mode = 0;
                }
                public EnumEquipmentStatus RunStateEnum { get; set; }
                public bool Stoping { get; private set; }
                public bool HomeIS { get; set; }
                public bool Err { get; private set; }
                public byte Mode { get; private set; }
                public int[] RunIDs { get; set; }
            }
            public const string linkStart = "0.0";
            public const string linkStop = "0.1";
            public const string linkStoping = "0.1";
            public const string linkPause = "0.2";
            public const string linkConnect = "0.3";
            public const string linkReset = "0.4";
            public const string linkInitialize = "0.5";

            public string DBSt { get; set; } = "DB10.";
            public bool Connect { get; set; }
            public bool DebugMode { get; set; }
            public bool SetStop { get; set; }
            public bool Loging { get; private set; }
            public string MessageStr { get; set; }
            public int InitializeTime { get; set; }
            public DiaveState Diave { get; set; }


            /// <summary>
            /// 停机
            /// </summary>
            /// <param name="isCoerce">True是强制停机</param>
            public void Stop(bool isCoerce = false)
            {
                if (Diave.RunStateEnum == EnumEquipmentStatus.初始化中)
                {
                    Diave.RunStateEnum = EnumEquipmentStatus.已停止;
                }
                if (isCoerce)
                {
                    StaticCon.SetLinkAddressValue(DBSt + linkStop, true);
                    RunUpStatus(DBSt + linkStop, false);
                }
                else
                {
                    StaticCon.SetLinkAddressValue(DBSt + linkStoping, true);
                    RunUpStatus(DBSt + linkStoping, false);
                }

            }
            /// <summary>
            /// 设备启动
            /// </summary>
            public void Start()
            {
                if (Diave.RunStateEnum == EnumEquipmentStatus.已停止)
                {
                    if (Diave.Err)
                    {
                        if (MessageBox.Show("存在故障，是否继续启动？", "故障", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) != DialogResult.Yes)
                        {
                            return;
                        }
                    }
                    if (StaticCon.SetLinkAddressValue(DBSt + linkStart, true))
                    {
                    }
                }
                Thread.Sleep(100);
                //Product.IsSwitchover = true;
                //RecipeCompiler.GetUserFormulaContrsl().EnabledLog(true);
                //StaticThis.textProgram.Btn_Start.Enabled = false;
                //StaticThis.textProgram.labelStat.Text = EquipmentStatus.ToString();
                StaticCon.SetLinkAddressValue(DBSt + linkPause, false);
                StaticCon.SetLinkAddressValue(DBSt + linkConnect, false);
                RunUpStatus(DBSt + linkStart, false);


            }
            /// <summary>
            /// 暂停
            /// </summary>
            public void Pause()
            {
                //string err = "";
                if (this.Diave.RunStateEnum == EnumEquipmentStatus.运行中)
                {
                    if (StaticCon.SetLinkAddressValue(DBSt + linkPause, true))
                    {
                    }
                    RunUpStatus(DBSt + linkPause, false);
                }

            }
            /// <summary>
            /// 复位
            /// </summary>
            public void Reset()
            {
                try
                {
                    SetValue(linkReset, true);
                    SetValue(linkPause, false);
                    SetValue(linkStart, false);
                    SetValue(linkStoping, false);
                    SetValue(linkStop, false);
                    SetValue(linkConnect, false);
                }
                catch (Exception)
                {

                }
            }

            public void Initialize()
            {
                try
                {
                    if (this.Diave.RunStateEnum == EnumEquipmentStatus.初始化中)
                    {
                        AlarmText.AddTextNewLine("正在初始化中", Color.Red);
                        return;
                    }
                    if (this.Diave.RunStateEnum == EnumEquipmentStatus.运行中)
                    {
                        AlarmText.AddTextNewLine("运行中无法初始化", Color.Red);
                        return;
                    }
                    SetValue(linkInitialize, true);
                    SetValue(linkPause, false);
                    SetValue(linkStart, false);
                    SetValue(linkStoping, false);
                    SetValue(linkStop, false);
                    SetValue(linkConnect, false);

                    RunUpStatus(linkInitialize, false);
                    Task.Run(() =>
                    {
                        AlarmText.AddTextNewLine("初始化开始", Color.Green);
                        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                        watch.Start();
                        while (true != Diave.HomeIS)
                        {
                            if (Diave.RunStateEnum == EnumEquipmentStatus.已停止)
                            {
                                AlarmText.AddTextNewLine("已停止初始化");
                                return;
                            }
                            if (watch.ElapsedMilliseconds >= InitializeTime)
                            {
                                AlarmText.AddTextNewLine("初始化失败，超时" + watch.ElapsedMilliseconds / 1000 + "S", Color.Red);
                                return;
                            }
                        }
                        AlarmText.AddTextNewLine("初始化完成，" + watch.ElapsedMilliseconds / 1000 + "S", Color.Green);
                    });
                }
                catch (Exception)
                {

                }
            }
            /// <summary>
            /// 写入bool值，起点地址加结尾地址
            /// </summary>
            /// <param name="key">结尾地址</param>
            /// <param name="value"></param>
            void SetValue(string key, bool value)
            {
                StaticCon.SetLinkAddressValue(DBSt + key, value);
            }

            public static void RunUpStatus(string linkname, bool isvalues, int time = 200)
            {
                try
                {
                    Task.Run(() =>
                    {
                        Thread.Sleep(time);
                        StaticCon.SetLinkAddressValue(linkname, isvalues);
                    });
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 自定义类型
        /// </summary>
        public class ErosType : UserType
        {
            /// <summary>
            /// 带地址的类型集合
            /// </summary>
            public Dictionary<string, UType> DicTypeValue;

            public ErosType()
            {
                DicTypeValue = new Dictionary<string, UType>();
            }

            /// <summary>
            /// 带XML名的构造函数
            /// </summary>
            /// <param name="xmlName">XML名称</param>
            public ErosType(string xmlName)
            {
                DicTypeValue = new Dictionary<string, UType>();
                this.ReadGetXML(xmlName);
            }

            /// <summary>
            /// 变量类
            /// </summary>
            public class UType : UserType
            {
                public UType()
                {
                    AddressID = "";
                }

                /// <summary>
                /// 带XML节点构造，
                /// </summary>
                /// <param name="xmlNode"></param>
                public UType(XmlNode xmlNode)
                {
                    AddressID = "";
                    Name = xmlNode.Attributes[constName].Value;
                    _Type = xmlNode.Attributes[constType].Value;
                    if (xmlNode.Attributes[constAddressID] != null)
                    {
                        AddressID = xmlNode.Attributes[constAddressID].Value;
                    }
                    Default = xmlNode.Attributes[constDefault].Value;
                    Annotation = xmlNode.Attributes[constAnnotation].Value;
                    WR = xmlNode.Attributes[constWR].Value;
                }

                public string Name { get; set; }

                /// <summary>
                /// 变量类型
                /// </summary>
                public string _Type { get; set; }

                /// <summary>
                /// 偏移地址
                /// </summary>
                public string AddressID { get; set; }

                /// <summary>
                /// 注释
                /// </summary>
                public string Annotation { get; set; }

                /// <summary>
                /// 默认值
                /// </summary>
                public string Default { get; set; }

                /// <summary>
                /// 读写权限
                /// </summary>
                public string WR { get; set; } = "W/R";

                /// <summary>
                /// 设置值
                /// </summary>
                public string SetValueStr { get; set; }
                /// <summary>
                /// 快照值
                /// </summary>
                public string SnapshootValueStr { get; set; }
            }

            /// <summary>
            /// 常量
            /// </summary>
            public const string constName = "Name";

            public const string constType = "_Type";
            public const string constAddressID = "AddressID";
            public const string constDefault = "Default";
            public const string constWR = "WR";
            public const string constAnnotation = "Annotation";
            public const string constPathXML = "Type\\";
            public const string constXmlElement = "ErosType";

            /// <summary>
            /// 读取XML
            /// </summary>
            /// <returns>成功返回ture</returns>
            public string ReadGetXML(string XMLName)
            {
                string pathXML = (constPathXML + XMLName + ".xml");
                XmlDocument doc = new XmlDocument();
                try
                {
                    if (!File.Exists(pathXML))
                    {
                        MessageBox.Show("未找到文件:" + pathXML);
                        return "未找到文件:" + pathXML;
                    }
                    doc.Load(pathXML);
                    //获得根节点
                    XmlElement users = doc.DocumentElement;
                    //获得根节点下子节点
                    XmlNodeList xnl = users.ChildNodes;
                    foreach (XmlNode item in xnl)
                    {//读取集合属性
                        if (!DicTypeValue.ContainsKey(item.Attributes[constName].Value))
                        {
                            UType erosValueD = new UType(item);
                            DicTypeValue.Add(item.Attributes[constName].Value, erosValueD);
                        }
                    }
                    return "";
                }
                catch (Exception me)
                {
                    MessageBox.Show(me.Message);
                    return me.Message.ToString();
                }
            }
        }

        /// <summary>
        /// 动态委托
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="PMs"></param>
        /// <returns></returns>
        public delegate T MyDelegate<T>(dynamic Sender, params T[] PMs);

        public class DelegateObj
        {
            private MyDelegate<dynamic> _delegate;

            public MyDelegate<dynamic> CallMethod
            {
                get { return _delegate; }
            }

            private DelegateObj(MyDelegate<dynamic> D)
            {
                _delegate = D;
            }

            /// <summary>
            /// 构造委托对象，让它看起来有点javascript定义的味道.
            /// </summary>
            /// <param name="D"></param>
            /// <returns></returns>
            public static DelegateObj Function(MyDelegate<dynamic> D)
            {
                return new DelegateObj(D);
            }
        }

        public class ty
        {
            public string name { get; set; }
            public string linkID { get; set; }
            public dynamic Value { get; set; }
            public string dett { get; set; }
            public Int16 AddID { get; set; }
        }

        public class DynObj : DynamicObject
        {
            //保存对象动态定义的属性值
            private Dictionary<string, dynamic> _values;

            public DynObj()
            {
                _values = new Dictionary<string, dynamic>();
            }

            public DynObj(string p_filePath)
            {
                _values = new Dictionary<string, dynamic>();
                p_filePath = @"..\..\温控箱.xml";
                if (!File.Exists(p_filePath))
                {
                    //throw new Exception("File path does not exist.");
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(p_filePath);
                //获得根节点
                XmlElement users = doc.DocumentElement;
                //获得根节点下子节点
                XmlNodeList xnl = users.ChildNodes;
                dynamic det = new DynObj();
                foreach (XmlNode item in xnl)
                {//读取集合属性
                    Type d = Type.GetType("System." + item.Attributes["_Type"].Value, true, false);
                    ty ty = new ty();
                    ty.name = item.Attributes["name"].Value;
                    ty.Value = Activator.CreateInstance(d, true);
                    ty.linkID = item.Attributes["LinkID"].Value;
                    ty.AddID = Convert.ToInt16(item.Attributes["AddressID"].Value);
                    _values.Add(item.Attributes["name"].Value, ty);
                    System.Reflection.PropertyInfo[] properties = ty.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    PropertyInfo et = ty.GetType().GetRuntimeProperty("name");

                    det.SetPropertyValue(item.Attributes["name"].Value, ty);
                    var DE = this.GetPropertyValue("温度运行.name");

                    ty ete = new ty();

                    //item.Attributes["AddressID"].Value;
                    // results.Add(item.Attributes["Value"].Value);
                    // results.Add(item.Attributes["LinkID"].Value);
                    // results.Add(item.Attributes["WR"].Value);
                }
            }

            /// <summary>
            /// 获取属性值
            /// </summary>
            /// <param name="propertyName"></param>
            /// <returns></returns>
            public dynamic GetPropertyValue(string propertyName)
            {
                string[] propertyNames = propertyName.Split('.');
                if (_values.ContainsKey(propertyNames[0]))
                {
                    return Damic(propertyName.Remove(0, propertyName.IndexOf('.') + 1), _values[propertyNames[0]]);
                }
                return null;

                //if (!_values.ContainsKey(propertyNames[0]))
                //{
                //    return null;
                //}
            }

            private dynamic Damic(string name, dynamic mic)
            {
                string[] propertyNames = name.Split('.');
                var dett = mic;
                Type e = dett.GetType();
                PropertyInfo det = e.GetRuntimeProperty(propertyNames[0]);

                //if (mic.GetPropertyValue(propertyNames[0]) !=null)
                //{
                //    return Damic(name.Remove(0,name.IndexOf('.')+1), mic);
                //}
                return null;
            }

            /// <summary>
            /// 设置属性值
            /// </summary>
            /// <param name="propertyName"></param>
            /// <param name="value"></param>
            public void SetPropertyValue<T>(string propertyName, T value)
            {
                if (_values.ContainsKey(propertyName))
                {
                    _values[propertyName] = value;
                }
                else
                {
                    _values.Add(propertyName, value);
                }
            }

            /// <summary>
            /// 实现动态对象属性成员访问的方法，得到返回指定属性的值
            /// </summary>
            /// <param name="binder"></param>
            /// <param name="result"></param>
            /// <returns></returns>
            public override bool TryGetMember(GetMemberBinder binder, out dynamic result)
            {
                result = GetPropertyValue(binder.Name);
                return result == null ? false : true;
            }

            /// <summary>
            /// 实现动态对象属性值设置的方法。
            /// </summary>
            /// <param name="binder"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public override bool TrySetMember(SetMemberBinder binder, dynamic value)
            {
                SetPropertyValue(binder.Name, value);
                return true;
            }

            /// <summary>
            /// 动态对象动态方法调用时执行的实际代码
            /// </summary>
            /// <param name="binder"></param>
            /// <param name="args"></param>
            /// <param name="result"></param>
            /// <returns></returns>
            public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
            {
                var theDelegateObj = GetPropertyValue(binder.Name) as DelegateObj;
                if (theDelegateObj == null || theDelegateObj.CallMethod == null)
                {
                    result = null;
                    return false;
                }
                result = theDelegateObj.CallMethod(this, args);
                return true;
            }

            public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
            {
                return base.TryInvoke(binder, args, out result);
            }
        }
    }
}