using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.Project.DebugF;
using Vision2.Project.formula;

namespace Vision2.Project.ProcessControl
{
    /// <summary>
    /// 过程监视
    /// </summary>
    public class ProcessUser : ProjectObj, ProjectNodet.IClickNodeProject
    {
        public override string FileName => "Process";

        public override string Name => "过程监视";

        public override string SuffixName => ".Txt";

        public override string ProjectTypeName => "Process";
        //[DescriptionAttribute("最大存储数量。"), Category("显示"), DisplayName("最大存储数量"),]
        //public int MaxLine { get; set; } = 100;

        public ProcessUser()
        {
            This = this;
        }

        [DescriptionAttribute(""), Category("EAP"), DisplayName("启动EAPStub"),]
        public bool EndbEap { get; set; }

        [DescriptionAttribute(""), Category("EAP"), DisplayName("离线Eap"),]
        public bool LinkFEap { get; set; }

        public static string QRCode
        {
            get
            {
                return qrCode;
            }
            set
            {
                if (qrCode != "")
                {
                    UserFormulaContrsl.This.label6.Text = "历史码:" + qrCode;
                }
                else if (value != "")
                {
                    UserFormulaContrsl.This.label6.Text = "历史码:";
                }
                //vision.Vision.OneProductVale.PanelID = value;
                qrCode = value;
            }
        }

        private static String qrCode = "";

        /// <summary>
        ///
        /// </summary>
        public string ExcelPath { get; set; } = Application.StartupPath;

        /// <summary>
        /// 托盘ID名称
        /// </summary>
        public string CarrierQRIDName { get; set; } = "Sputter Carrier";

        /// <summary>
        /// 分割符号
        /// </summary>
        public string Split_Symbol { get; set; } = "=";

        /// <summary>
        /// 托盘前缀名称
        /// </summary>
        public string SN_Name { get; set; } = "SN";

        private static ProcessUser This;

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

        private ProcessControl userVisionManagement;

        [DescriptionAttribute("读取产品ID的地址指针。（可自定多个地址读取，以,分割）"), Category("触发器"), DisplayName("产品ID地址名称"),]
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string ReadName { get; set; } = string.Empty;

        [DescriptionAttribute("载具ID的地址指针。"), Category("触发器"), DisplayName("载具地址名称"),]
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string ReadPairsName { get; set; } = string.Empty;

        [DescriptionAttribute("读取二维码地址。"), Category("触发器"), DisplayName("触发读取二维码"),]
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string GetQRCodeName { get; set; } = string.Empty;

        [DescriptionAttribute("读取完成。"), Category("触发器"), DisplayName("读取完成"),]
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string SetQRCodeDoneName { get; set; } = string.Empty;

        [DescriptionAttribute("是否启用Eap功能地址。"), Category("Eap"), DisplayName("启用Eap地址"),]
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string EapEnName { get; set; } = string.Empty;

        [DescriptionAttribute("Eap加工请求开始标志。"), Category("Eap"), DisplayName("请求开始标识地址"),]
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string EapGetQRSName { get; set; } = string.Empty;

        [DescriptionAttribute("Eap加工请求码地址标识。"), Category("Eap"), DisplayName("请求码地址"),]
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string EapGetQRCodeName { get; set; } = string.Empty;

        [DescriptionAttribute("EAP返回可加工地址。"), Category("Eap"), DisplayName("返回允许执行地址"),]
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string EapGetOKName { get; set; } = string.Empty;

        [DescriptionAttribute("EAP返回不可加工地址。"), Category("Eap"), DisplayName("返回不允许执行地址"),]
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string EapGetNGName { get; set; } = string.Empty;

        /// <summary>
        /// 载具码
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> TrayID =
            new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 获得托盘码集合
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> GetTrayID()
        {
            return TrayID;
        }

        /// <summary>
        /// 返回程序的托盘码动态参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetTyayIDP(string id)
        {
            if (TrayID.ContainsKey(id))
            {
                return TrayID[id];
            }
            return null;
        }

        /// <summary>
        /// 历史数据采集
        /// </summary>
        public Dictionary<string, MyProcessU> DIcSewS = new Dictionary<string, MyProcessU>();

        public override void initialization()
        {
            //base.initialization();
            if (userVisionManagement == null || userVisionManagement.IsDisposed)
            {
                userVisionManagement = new ProcessControl(this);
            }
            this.ProductMessage.Clear();
            if (File.Exists(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\过程记录\\码.Txt"))
            {
                ProjectINI.ReadPathJsonToCalss(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\过程记录\\码.Txt", out this.ProductMessage);
            }
            if (File.Exists(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\过程记录\\载具码.Txt"))
            {
                ProjectINI.ReadPathJsonToCalss(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\过程记录\\载具码.Txt", out this.TrayID);
            }
            RunData();
            RunPData();
        }

        /// <summary>
        /// 历史数据类
        /// </summary>
        public struct MyProcessU
        {
            /// <summary>
            /// 触发历史数据名，如果为数字则为定时触发
            /// </summary>
            public string ValueIsName;

            /// <summary>
            /// 文件名
            /// </summary>
            public string FileName;

            /// <summary>
            /// 历史数据名称与地址集合
            /// </summary>
            public Dictionary<string, formula.Product.StructTypeValue> keyValuePa;
        }

        /// <summary>
        /// 过程数据采集
        /// </summary>
        public struct MyStruct
        {
            /// <summary>
            /// 采集地址
            /// </summary>
            public string ValueName;

            /// <summary>
            /// 触发地址
            /// </summary>
            public string ValueIsName;
        }

        /// <summary>
        ///站动态参数
        /// </summary>
        public Dictionary<string, MyStruct> kValues;

        /// <summary>
        /// 产品ID和参数
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> ProductMessage = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 历史
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> DataProductMessage = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> GetData()
        {
            return DataProductMessage;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        public static void AddProt(string code)
        {
            if (!This.ProductMessage.ContainsKey(code))
            {
                This.ProductMessage.Add(code, new Dictionary<string, string>());
                foreach (var item2 in This.kValues)
                {
                    This.ProductMessage[code].Add(item2.Key, "Null");
                }
            }
        }

        /// <summary>
        /// 删除产品ID过程数据
        /// </summary>
        /// <param name="name"></param>
        public static void Remove(string name)
        {
            if (This.ProductMessage.ContainsKey(name))
            {
                This.ProductMessage.Remove(name);
            }
        }

        /// <summary>
        /// 删除载具
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveTrayCode(string name)
        {
            if (This.TrayID.ContainsKey(name))
            {
                This.TrayID.Remove(name);
            }
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static ProcessUser Instancen
        {
            get
            {
                if (This == null)
                {
                    This = new ProcessUser();
                }

                return This;
            }
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public static void ClearAll()
        {
            This.DataProductMessage.Clear();
            if (File.Exists(ProjectINI.ProjectPathRun + "\\过程记录\\码.Txt"))
            {
                System.IO.File.Delete(ProjectINI.ProjectPathRun + "\\过程记录\\码.Txt");
            }
            if (File.Exists(ProjectINI.ProjectPathRun + "\\过程记录\\载具码.Txt"))
            {
                System.IO.File.Delete(ProjectINI.ProjectPathRun + "\\过程记录\\载具码.Txt");
            }

            This.ProductMessage.Clear();
            This.TrayID.Clear();
        }

        /// <summary>
        /// 查询产品ID的过程数据
        /// </summary>
        /// <param name="id">ID编号</param>
        /// <returns>返回参数状态，不存在则返回NUll</returns>
        public static Dictionary<string, string> GetId(string id)
        {
            if (This.ProductMessage.ContainsKey(id))
            {
                return This.ProductMessage[id];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询id,指定到属性
        /// </summary>
        /// <param name="id">产品id</param>
        /// <param name="keyP">属性名</param>
        /// <returns>结果，不存在或空时返回""</returns>
        public static string GetIdPrag(string id, string keyP)
        {
            if (GetId(id) != null)
            {
                if (GetId(id).ContainsKey(keyP))
                {
                    return GetId(id)[keyP];
                }
            }
            return "";
        }

        /// <summary>
        /// 返回载具码托盘的位置信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<string> GetCode(string code)
        {
            return GetPidPrag("载具码", code, "位置");
        }

        /// <summary>
        /// 根据载具码返回指定的字段属性
        /// </summary>
        /// <param name="idName"></param>
        /// <param name="id">载具码</param>
        /// <param name="keyP">字段</param>
        /// <returns>属性</returns>
        public static List<string> GetPidPrag(string idName, string id, string keyP)
        {
            try
            {
                var idt = from de in This.ProductMessage
                          where de.Value.ContainsKey(idName) && de.Value[idName] == id
                          where de.Value.ContainsKey(keyP) && de.Value.ContainsKey("位置")
                          orderby int.Parse(de.Value["位置"]) ascending
                          select de;
                List<string> lt = new List<string>();
                int dt = 0;
                foreach (var item in idt)
                {
                    dt++;
                    if (item.Value["位置"] == dt.ToString())
                    {
                        if (keyP == "OK")
                        {
                            if (This.ProductMessage.ContainsKey(item.Key))
                            {
                                if (This.ProductMessage[item.Key]["位移检测"] == "False")
                                {
                                    lt.Add(item.Key + "=null");
                                }
                                else
                                {
                                    lt.Add(item.Key + "=" + item.Value[keyP]);
                                }
                            }
                            else
                            {
                                lt.Add(item.Key + "=" + item.Value[keyP]);
                            }
                        }
                        else
                        {
                            lt.Add(item.Key + "=" + item.Value[keyP]);
                        }
                    }
                }
                return lt;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        /// <summary>
        /// 写入载具码数据记录
        /// </summary>
        /// <param name="id">载具码</param>
        /// <param name="pName">属性名</param>
        /// <param name="value">属性值</param>
        public static void SetTrayIDP(string id, string pName, string value)
        {
            try
            {
                if (This.TrayID == null)
                {
                    This.TrayID = new Dictionary<string, Dictionary<string, string>>();
                }

                if (!This.TrayID.ContainsKey(id))
                {
                    This.TrayID.Add(id, new Dictionary<string, string>());
                }
                This.TrayID[id][pName] = value;
                ProjectINI.ClassToJsonSavePath(This.TrayID, ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\过程记录\\载具码.Txt");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 写入载具码数据记录
        /// </summary>
        /// <param name="id">载具码</param>
        /// <param name="pName">属性名</param>
        /// <param name="value">属性值</param>
        public static void SetTrayIDP(string id, string[] pName, string[] value)
        {
            try
            {
                if (pName.Length == value.Length)
                {
                    if (!This.TrayID.ContainsKey(id))
                    {
                        This.TrayID.Add(id, new Dictionary<string, string>());
                    }
                    for (int i = 0; i < pName.Length; i++)
                    {
                        This.TrayID[id][pName[i]] = value[i];
                    }
                    ProjectINI.ClassToJsonSavePath(This.TrayID, ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\过程记录\\载具码.Txt");
                }
                else
                {
                    MessageBox.Show("写入参数长度不同");
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 写入产品ID字段信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="ProName"></param>
        /// <param name="value"></param>
        public static void SetCodeProValue(string code, string ProName, string value)
        {
            try
            {
                if (!This.ProductMessage.ContainsKey(code))
                {
                    This.ProductMessage.Add(code, new Dictionary<string, string>());
                }
                This.ProductMessage[code][ProName] = value;
                if (ProName == "状态" && value == "Done")
                {
                    if (!This.DataProductMessage.ContainsKey(code))
                    {
                        List<string> dataNames = new List<string>();
                        dataNames.Add(DateTime.Now.ToLongTimeString());
                        dataNames.Add("ID码{" + code + "}");
                        foreach (var item in This.ProductMessage[code])
                        {
                            dataNames.Add(item.Key + "{" + item.Value + "}");
                        }
                        if (!File.Exists(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\过程记录\\ 码信息" + DateTime.Now.ToLongDateString() + ".xls"))
                        {
                            //List<string> dataColumn = new List<string>();
                            //dataColumn.Add("时间");
                            //dataColumn.Add("ID码");
                            //Vision2.ErosProjcetDLL.Excel.Npoi.AddWriteColumnToExcel( ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\过程记录\\ 码信息" + DateTime.Now.ToLongDateString()
                            //    + ".xls", "过程码",
                            // dataColumn.ToArray());
                        }
                        Vision2.ErosProjcetDLL.Excel.Npoi.AddRosWriteToExcel(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\过程记录\\ 码信息" + DateTime.Now.ToLongDateString() + ".xls", "过程码",
                        dataNames.ToArray());
                    }
                    //Remove(code);
                }
                if (ProName == "载具码")
                {
                    This.AddTyID(value);
                }

                ProjectINI.ClassToJsonSavePath(This.ProductMessage, ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\过程记录\\码.Txt");
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 添加托盘ID
        /// </summary>
        /// <param name="id"></param>
        public void AddTyID(string id)
        {
            if (!This.TrayID.ContainsKey(id))
            {
                string times = DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString();
                string timeB = StaticCon.GetLingkNameValueString("点胶机.胶水A ID");
                string timeA = StaticCon.GetLingkNameValueString("点胶机.胶水B ID");
                string timeC = StaticCon.GetLingkNameValueString("打螺丝30.螺丝物料ID");
                string timeD = StaticCon.GetLingkNameValueString("上料站10.侧框上料物料ID");
                SetTrayIDP(id, new string[] { "入站时间", "A胶水ID", "B胶水ID", "螺丝ID", "物料ID" }, new string[] { times, timeA, timeB, timeC, timeD });
            }
        }

        private void setID(string id)
        {
            string times = DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString();
            SetTrayIDP(id, "出站时间", times);
            Newtonsoft.Json.Linq.JObject jsDataS = new Newtonsoft.Json.Linq.JObject();
            jsDataS.Add("载具码", id);
            List<string> list = GetCode(id);
            List<Dictionary<string, string>> dt = new List<Dictionary<string, string>>();
            jsDataS.Add("数量", list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                string[] itmeStr = list[i].Split('=');
                Dictionary<string, string> dst = GetId(itmeStr[0]);
                dt.Add(dst);
                if (itmeStr.Length == 2)
                {
                    SetCodeProValue(itmeStr[0], "过程", "侧框出站");
                }
                RemoveTrayCode(itmeStr[0]);
                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dst);
                jsDataS.Add("产品" + (i + 1).ToString(), jsonStr);
            }
            //Stub.StubManager.getEAP().PushResult(jsDataS.ToString());
        }

        /// <summary>
        /// 发送上报信息并清除处理信息
        /// </summary>
        /// <param name="id"></param>
        public void SendTyID(string id)
        {
            string times = DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString();
            SetTrayIDP(id, "出站时间", times);
            //Newtonsoft.Json.Linq.JObject jsDataS = new Newtonsoft.Json.Linq.JObject();
            //jsDataS.Add("载具码", id);
            List<string> list = GetCode(id);
            List<Dictionary<string, string>> dt = new List<Dictionary<string, string>>();

            string datas = "";
            string dataS = "";
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    string idt = list[i].Split('=')[0];
                    List<string> listitmes = new List<string>();
                    listitmes.Add(idt);//添加产品码
                    listitmes.Add(GetTyayIDP(id)["物料ID"]);//添加物料信息
                    listitmes.Add(GetTyayIDP(id)["螺丝ID"]);
                    listitmes.Add(GetTyayIDP(id)["A胶水ID"]);
                    listitmes.Add(GetTyayIDP(id)["B胶水ID"]);
                    listitmes.Add(GetIdPrag(idt, "OK"));
                    listitmes.Add(GetIdPrag(idt, "OK"));
                    listitmes.Add(GetIdPrag(idt, "加工时长"));
                    listitmes.Add(GetIdPrag(idt, "CMK评分"));
                    listitmes.Add(GetIdPrag(idt, "胶量 ML"));
                    listitmes.Add(GetIdPrag(idt, "点胶监测.加工时长"));
                    listitmes.Add(GetIdPrag(idt, "测量断胶数量"));
                    listitmes.Add(GetIdPrag(idt, "测量最大值"));
                    listitmes.Add(GetIdPrag(idt, "测量最小值"));
                    listitmes.Add(GetIdPrag(idt, "测量平均值"));
                    listitmes.Add(GetIdPrag(idt, "测量NG数量"));
                    datas += AddL(listitmes.ToArray());//将多个产品信息转换SML字符添加到字符串
                }
                dataS = AddL(id, list.Count.ToString(), Product.ProductionName,
               Product.GetProd()["产品ID"], GetTyayIDP(id)["入站时间"],
                GetTyayIDP(id)["出站时间"], datas);//将多个产品信息SML添加到载具码上；

                ProcessControl.StaticProcessThis.textBox7.Text = dataS;
            }
            catch (Exception)
            {
            }
            for (int i = 0; i < list.Count; i++)
            {
                string[] itmeStr = list[i].Split('=');
                Dictionary<string, string> dst = GetId(itmeStr[0]);
                dt.Add(dst);
                if (itmeStr.Length == 2)
                {
                    SetCodeProValue(itmeStr[0], "过程", "侧框出站");
                }
                Remove(itmeStr[0]);
                //string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dst);
            }
            try
            {
                //AddL(GetPidPrag("位置",id, "产品ID");
                //Stub.StubManager.getEAP().PushResult(dataS);
                RemoveTrayCode(id);
            }
            catch (Exception ex)
            {
                Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(id + ":" + ex.Message);
            }
        }

        /// <summary>
        /// 创建一个L集合
        /// </summary>
        /// <param name="value">集合参数</param>
        /// <returns></returns>
        public static string AddL(params string[] value)
        {
            string data = @"< L" + Environment.NewLine;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Trim(' ').StartsWith("< L"))
                {
                    data += value[i];
                }
                else
                {
                    data += string.Format(@"  <A'{0}'>" + Environment.NewLine, value[i]);
                }
            }
            return data + ">" + Environment.NewLine;
        }

        /// <summary>
        /// 写入多个过程数据，长度不等时不添加
        /// </summary>
        /// <param name="code">产品ID</param>
        /// <param name="proNames">参数名</param>
        /// <param name="values">值</param>
        public static void SetCodeProValue(string code, string[] proNames, string[] values)
        {
            try
            {
                if (!This.ProductMessage.ContainsKey(code))
                {
                    This.ProductMessage.Add(code, new Dictionary<string, string>());
                }

                if (proNames.Length == values.Length)
                {
                    for (int i = 0; i < proNames.Length; i++)
                    {
                        if (proNames[i] == "载具码")
                        {
                            This.AddTyID(values[i]);
                        }
                        This.ProductMessage[code][proNames[i]] = values[i];
                    }
                    ProjectINI.ClassToJsonSavePath(This.ProductMessage, ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\过程记录\\码.Txt");
                }
            }
            catch (Exception ex)
            {
                Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(ex.Message);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Dictionary<string, string> TrayId(string code)
        {
            string err = "";
            try
            {
                DebugCompiler.dicstr = null;
                AlarmText.AddTextNewLine(code + "进站查询");
                //Stub.StubManager.getEAP().SetSVIDValue("TrayID", code);
                //string trayID = Stub.StubManager.getEAP().GetSVIDValue("TrayID");
                //Stub.StubManager.getEAP().PushEvent("ProcessStarted");
            }
            catch (Exception)
            {
            }
            Thread.Sleep(2000);
            if (!ProcessUser.Instancen.LinkFEap)
            {
                if (DebugCompiler.dicstr != null)
                {
                    if (DebugCompiler.dicstr.ContainsKey("ENABLE")
                        && DebugCompiler.dicstr.ContainsKey("NEXTTRAYID")
                        && DebugCompiler.dicstr.ContainsKey("PPID"))
                    {
                        if (DebugCompiler.dicstr["ENABLE"] == "true")
                        {
                            if (DebugCompiler.dicstr["PPID"] == Product.ProductionName)
                            {
                                StaticCon.SetLingkValue(This.EapGetOKName, true, out err);
                                AlarmText.AddTextNewLine(DebugCompiler.dicstr["NEXTTRAYID"] + "查询成功:" + DebugCompiler.dicstr["PPID"]);
                                string times = DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString();

                                Project.ProcessControl.ProcessUser.SetTrayIDP(DebugCompiler.dicstr["NEXTTRAYID"], "入站时间", times);
                            }
                            else
                            {
                                AlarmText.AddTextNewLine(code + "查询失败,目标产品" + DebugCompiler.dicstr["PPID"]);
                            }
                        }
                        else
                        {
                            AlarmText.AddTextNewLine(code + "查询失败");
                        }
                    }
                }
                else
                {
                    AlarmText.AddTextNewLine(code + "查询失败");
                }
            }
            else
            {
                string times = DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString();
                string timeB = StaticCon.GetLingkNameValueString("点胶机.胶水A ID");
                string timeA = StaticCon.GetLingkNameValueString("点胶机.胶水B ID");

                SetTrayIDP(code, new string[] { "入站时间", "A胶水ID", "B胶水ID" }, new string[] { times, timeA, timeB });
                StaticCon.SetLingkValue(This.EapGetOKName, true, out err);
                AlarmText.AddTextNewLine(code + "查询成功");
            }
            StaticCon.SetLingkValue(This.EapGetQRSName, false, out err);
            return DebugCompiler.dicstr;
        }

        /// <summary>
        /// 单击显示属性
        /// </summary>
        /// <param name="pertyForm"></param>
        /// <param name="data"></param>
        public void UpProperty(PropertyForm pertyForm, object data = null)
        {
            //TabPage tabPage = new TabPage();
            //tabPage.Name = "外部Dll";
            //tabPage.Text = "外部Dll";
            // pertyForm.tabControl1.TabPages.Add(tabPage);
            pertyForm.tabPage1.Controls.Add(new ManageProcess(this) { Dock = DockStyle.Top });
            //pertyForm.tabPage1.Controls.Add();
        }

        public Control GetThisControl()
        {
            return new ManageProcess(this) { Dock = DockStyle.Top };
        }

        /// <summary>
        /// 开始运行
        /// </summary>
        private bool IsRun;

        /// <summary>
        /// 等待关闭
        /// </summary>
        private bool CorsCon;

        /// <summary>
        /// 更新状态
        /// </summary>
        public void RunData()
        {
            if (IsRun)
            {
                CorsCon = false;
                IsRun = false;
                while (!CorsCon)
                {
                }
            }
            IsRun = true;
            string[] readNames = ReadName.Split(',');
            string[] readValues = new string[readNames.Length];
            string[] readPairNames = ReadPairsName.Split(',');
            string itesmVCode = "";
            string err;
            Thread thread = new Thread(() =>
            {
                while (IsRun)
                {
                    try
                    {
                        //EapEnName = "点胶机.启动MES";
                        //EapGetQRSName = "点胶机.Ask Mes前段托盘请求允许点胶";
                        //EapGetQRCodeName = "点胶机.当前载具码";
                        //EapGetNGName = "点胶机.MES不允许点胶";
                        //EapGetOKName = "点胶机.MES允许前段托盘点胶";
                        ///进站请求加工
                        if (StaticCon.GetLingkNameValue(EapEnName))
                        {
                            if (StaticCon.GetLingkNameValue(EapGetQRSName))
                            {
                                string codt = StaticCon.GetLingkNameValueString(EapGetQRCodeName);
                                TrayId(codt);
                            }
                        }
                        ///加工结束记录数据
                        string itemS = "";
                        if (GetQRCodeName != "")
                        {
                            if (StaticCon.GetLingkNameValue(GetQRCodeName))
                            {
                                Thread.Sleep(5000);
                                for (int i = 0; i < readPairNames.Length; i++)
                                {
                                    itemS = StaticCon.GetLingkNameValueString(readPairNames[i]);
                                    if (itemS != "")
                                    {
                                        itesmVCode = itemS;
                                        if (!TrayID.ContainsKey(itesmVCode))
                                        {
                                            TrayID.Add(itesmVCode, new Dictionary<string, string>());
                                        }
                                    }
                                }
                                string cmk = StaticCon.GetLingkNameValueString("点胶机.CMK");
                                string timeCD = StaticCon.GetLingkNameValueString("点胶机.CycleTime点胶时间");
                                for (int i = 0; i < readNames.Length; i++)
                                {
                                    itemS = StaticCon.GetLingkNameValueString(readNames[i]);
                                    if (itemS != "")
                                    {
                                        readValues[i] = itemS;
                                        string time = StaticCon.GetLingkNameValueString("点胶机.产品" + (i + 1).ToString() + "注胶OK");
                                        string time2 = StaticCon.GetLingkNameValueString("点胶机.点胶量");
                                        string time3 = StaticCon.GetLingkNameValueString("点胶机.产品" + (i + 1).ToString() + "位移检测OK");
                                        string codeQR = readValues[i];
                                        if (readValues[i].ToLower() == "noread")
                                        {
                                            codeQR = readValues[i] + itesmVCode + (i + 1);
                                        }
                                        SetCodeProValue(codeQR, new string[] { "过程", "载具码", "位置",
                                            readNames[i].Split('.')[0], "OK", "点胶量", "位移检测", "CMK", "点胶时间" },
                                        new string[] { readNames[i].Split('.')[0], itesmVCode, (i + 1).ToString(), time, time, time2, time3, cmk, timeCD });
                                    }
                                    else
                                    {
                                        Vision2.ErosProjcetDLL.Project.AlarmText.LogIncident("点胶机", "空数据");
                                    }
                                }
                                StaticCon.SetLingkValue(SetQRCodeDoneName, true, out err);
                                StaticCon.SetLingkValue(GetQRCodeName, false, out err);
                                Thread.Sleep(1000);
                                StaticCon.SetLingkValue(SetQRCodeDoneName, false, out err);
                            }
                        }

                        ///点胶监测
                        if (StaticCon.GetLingkNameValue("皮带线站.位置1OK"))
                        {
                            string tad = ErosSocket.ErosConLink.StaticCon.GetLingkNameValue("皮带线站.载具码");
                            List<string> list = Project.ProcessControl.ProcessUser.GetCode(tad);
                            if (list != null)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    string[] data = list[i].Split('=');
                                    if (data.Length == 2)
                                    {
                                        if (1.ToString() == data[1])
                                        {
                                            Project.ProcessControl.ProcessUser.SetCodeProValue(data[0], "OK", true.ToString());
                                        }
                                    }
                                }
                            }
                            StaticCon.SetLingkValue("皮带线站.位置1OK", false, out err);
                        }
                        if (StaticCon.GetLingkNameValue("皮带线站.位置2OK"))
                        {
                            string tad = ErosSocket.ErosConLink.StaticCon.GetLingkNameValue("皮带线站.载具码");
                            List<string> list = Project.ProcessControl.ProcessUser.GetCode(tad);
                            if (list != null)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    string[] data = list[i].Split('=');
                                    if (data.Length == 2)
                                    {
                                        if (2.ToString() == data[1])
                                        {
                                            Project.ProcessControl.ProcessUser.SetCodeProValue(data[0], "OK", true.ToString());
                                        }
                                    }
                                }
                            }
                            StaticCon.SetLingkValue("皮带线站.位置2OK", false, out err);
                        }
                        if (StaticCon.GetLingkNameValue("皮带线站.位置3OK"))
                        {
                            string tad = ErosSocket.ErosConLink.StaticCon.GetLingkNameValue("皮带线站.载具码");
                            List<string> list = Project.ProcessControl.ProcessUser.GetCode(tad);
                            if (list != null)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    string[] data = list[i].Split('=');
                                    if (data.Length == 2)
                                    {
                                        if (3.ToString() == data[1])
                                        {
                                            Project.ProcessControl.ProcessUser.SetCodeProValue(data[0], "OK", true.ToString());
                                        }
                                    }
                                }
                            }
                            StaticCon.SetLingkValue("皮带线站.位置3OK", false, out err);
                        }
                        if (StaticCon.GetLingkNameValue("皮带线站.位置1NG"))
                        {
                            string tad = ErosSocket.ErosConLink.StaticCon.GetLingkNameValue("皮带线站.载具码");
                            List<string> list = Project.ProcessControl.ProcessUser.GetCode(tad);
                            if (list != null)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    string[] data = list[i].Split('=');
                                    if (data.Length == 2)
                                    {
                                        if (1.ToString() == data[1])
                                        {
                                            Project.ProcessControl.ProcessUser.SetCodeProValue(data[0], "OK", false.ToString());
                                        }
                                    }
                                }
                            }
                            StaticCon.SetLingkValue("皮带线站.位置1NG", false, out err);
                        }
                        if (StaticCon.GetLingkNameValue("皮带线站.位置2NG"))
                        {
                            string tad = ErosSocket.ErosConLink.StaticCon.GetLingkNameValue("皮带线站.载具码");
                            List<string> list = Project.ProcessControl.ProcessUser.GetCode(tad);
                            if (list != null)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    string[] data = list[i].Split('=');
                                    if (data.Length == 2)
                                    {
                                        if (2.ToString() == data[1])
                                        {
                                            Project.ProcessControl.ProcessUser.SetCodeProValue(data[0], "OK", false.ToString());
                                        }
                                    }
                                }
                            }
                            StaticCon.SetLingkValue("皮带线站.位置2NG", false, out err);
                        }

                        if (StaticCon.GetLingkNameValue("皮带线站.位置3NG"))
                        {
                            string tad = ErosSocket.ErosConLink.StaticCon.GetLingkNameValue("皮带线站.载具码");
                            List<string> list = Project.ProcessControl.ProcessUser.GetCode(tad);
                            if (list != null)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    string[] data = list[i].Split('=');
                                    if (data.Length == 2)
                                    {
                                        if (3.ToString() == data[1])
                                        {
                                            Project.ProcessControl.ProcessUser.SetCodeProValue(data[0], "OK", false.ToString());
                                        }
                                    }
                                }
                            }
                            StaticCon.SetLingkValue("皮带线站.位置3NG", false, out err);
                        }
                        if (StaticCon.GetLingkNameValue("皮带线站.DoneT"))
                        {
                            string tad = ErosSocket.ErosConLink.StaticCon.GetLingkNameValue("皮带线站.载具码");
                            string tadt = ErosSocket.ErosConLink.StaticCon.GetLingkNameValue("皮带线41.加工时长");
                            Project.ProcessControl.ProcessUser.SetTrayIDP(tad, "AOI加工时长", tadt);
                            StaticCon.SetLingkValue("皮带线站.DoneT", false, out err);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    if (userVisionManagement != null)
                    {
                        userVisionManagement.Up();
                    }
                    Thread.Sleep(200);
                }
                CorsCon = true;
            });
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        ///
        /// </summary>
        private bool IsRunPdat;

        /// <summary>
        ///
        /// </summary>
        private bool CorsConPdata;

        /// <summary>
        /// 执行线程历史记录
        /// </summary>
        public void RunPData()
        {
            if (IsRunPdat)
            {
                CorsConPdata = false;
                IsRunPdat = false;
                while (!CorsConPdata)
                {
                }
            }
            IsRunPdat = true;
            int d = 0;
            foreach (var item in DIcSewS)
            {
                d++;
                Thread thread = new Thread(() =>
                {
                    while (IsRunPdat)
                    {
                        try
                        {
                            string itemS = StaticCon.GetLingkNameValueString(item.Value.ValueIsName);
                            if (itemS == true.ToString())
                            {
                                List<string> datas = new List<string>();
                                List<string> dataNames = new List<string>();
                                string timeStr = DateTime.Now.ToString("MM/dd HH:mm:ss");
                                string timedey = DateTime.Now.ToString("yy年MM月dd日");
                                bool cerName = false;
                                datas.Add(timeStr);
                                if (!File.Exists(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\过程记录\\" + item.Value.FileName + "\\" + timedey + ".xls"))
                                {
                                    cerName = true;
                                    dataNames.Add("时间");
                                }
                                foreach (var item2 in item.Value.keyValuePa)
                                {
                                    string[] dat = item2.Value.ValueStr.Split('.');
                                    SocketClint itmeClint = StaticCon.GetSocketClint(dat[0]);
                                    string id = item2.Value.ValueStr.Remove(0, dat[0].Length + 1);
                                    itmeClint.GetIDValue(id, item2.Value.TypeStr, out dynamic value);
                                    datas.Add(value.ToString());
                                    if (cerName)
                                    {
                                        dataNames.Add(item2.Key);
                                    }
                                }
                                if (cerName)
                                {
                                    ErosProjcetDLL.Excel.Npoi.AddWriteColumnToExcel(ProjectINI.ProjectPathRun + "\\过程记录\\" +
                                        item.Value.FileName + "\\" + timedey + ".xls", item.Value.FileName, dataNames.ToArray());
                                }
                                ErosProjcetDLL.Excel.Npoi.AddRosWriteToExcel(ProjectINI.ProjectPathRun +
                                    "\\过程记录\\" + item.Value.FileName + "\\" + timedey + ".xls", item.Value.FileName, datas.ToArray());
                                StaticCon.SetLingkValue(item.Value.ValueIsName, false.ToString(), out string err);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        Thread.Sleep(100);
                    }
                    d--;
                    if (d <= 0)
                    {
                        CorsConPdata = true;
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
        }
    }
}