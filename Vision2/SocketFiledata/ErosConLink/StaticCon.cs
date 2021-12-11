using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Vision2.ErosProjcetDLL.Project;

namespace ErosSocket.ErosConLink
{
    public class DicSocket : ProjectObj
    {
        public override string Name { get; set; } = "链接";
        public override string SuffixName => ".socket";
        public override string Text { get; set; } = "设备控制";
        public override string FileName { get { return "Socket"; } }
        public override string ProjectTypeName => "链接信息";

        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DisplayName("启动显示灯"), Category("报警设置")]
        public string LinkStan { get; set; }

        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DisplayName("报警灯"), Category("报警设置")]
        public string LinkRed { get; set; }

        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DisplayName("黄色警告灯"), Category("报警设置")]
        public string LinkW { get; set; }

        /// <summary>
        /// 返回文件夹地址信息
        /// </summary>
        /// <returns></returns>
        public string GetFilePath()
        {
            return FileName + "\\" + Name + SuffixName;
        }

        public DicSocket()
        {
            _instance = this;

            //ToolStripItem toolStripButton=   Vision2.ErosProjcetDLL.MainForm1.MainFormF.ToolStripAddMenuItem("通信管理");
            //toolStripButton.Click += ToolStripButton_Click;

            this.contextMenuTT.Items.Add("新建链接").Click += DicSocket_Click;
            this.contextMenuTT.Items.Add("调试界面").Click += ShowDebugForm;
        }

        private SocketConnectForm socketConnectForm;

        private void ToolStripButton_Click(object sender, EventArgs e)
        {
            if (socketConnectForm == null || socketConnectForm.IsDisposed)
            {
                socketConnectForm = new SocketConnectForm();
            }
            socketConnectForm.Show();
        }

        private void ShowDebugForm(object sender, EventArgs e)
        {
            SocketConnectForm socketConnectForm = new SocketConnectForm();

            socketConnectForm.Show();
        }

        private void DicSocket_Click(object sender, EventArgs e)
        {
            NewSocketForm newSocketForm = new NewSocketForm(Node);
            newSocketForm.Show();
        }

        /// <summary>
        /// 单例实例
        /// </summary>
        private static DicSocket _instance;

        public static DicSocket Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DicSocket();
                }
                return _instance;
            }
            set { _instance = value; }
        }

        public override void UpProjectNode(TreeNode tvProejcetNode)
        {
            try
            {
                base.UpProjectNode(tvProejcetNode);
                ///客户端链接
                if (StaticCon.SocketClint.Count > 0)
                {
                    Dictionary<string, SocketClint> lis = new Dictionary<string, SocketClint>();
                    foreach (var item in StaticCon.SocketClint)
                    {
                        lis.Add(item.Value.Name, item.Value);
                        TreeNode treeNode = item.Value.GetNode();

                        if (treeNode.ContextMenuStrip.Items.Find("打开界面", true).Length == 0)
                        {
                            ToolStripItem toolStripItem = treeNode.ContextMenuStrip.Items.Add("打开界面");
                            toolStripItem.Click += DicSocket_Click1;
                            toolStripItem.Name = "打开界面";
                        }
                        if (treeNode.ContextMenuStrip.Items.Find("打开变量表", true).Length == 0)
                        {
                            ToolStripItem toolStripItem = treeNode.ContextMenuStrip.Items.Add("打开变量表");
                            toolStripItem.Click += DicSocket_Click2;
                            toolStripItem.Name = "打开变量表";
                        }
                        if (treeNode.ContextMenuStrip.Items.Find("删除连接", true).Length == 0)
                        {
                            ToolStripItem toolStripItem = treeNode.ContextMenuStrip.Items.Add("删除连接");
                            toolStripItem.Click += DicSRmove_Click1;
                            toolStripItem.Name = "删除连接";
                        }
                        void DicSocket_Click1(object sender, EventArgs e)
                        {
                            item.Value.ShowForm();
                        }

                        void DicSocket_Click2(object sender, EventArgs e)
                        {
                            item.Value.ShowValuesForm();
                        }

                        void DicSRmove_Click1(object sender, EventArgs e)
                        {
                            DialogResult dr = MessageBox.Show("是否关闭连接并删除？", "删除连接", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                item.Value.Dispose();
                                StaticCon.SocketClint.Remove(item.Key);
                                treeNode.Remove();
                            }
                        }
                        Node.Nodes.Add(treeNode);
                    }
                    StaticCon.SocketClint = lis;
                }
                ///子链接
                if (StaticCon.SocketDic.Count > 0)
                {
                    TreeNode[] tNodeLinksDic = tvProejcetNode.Nodes.Find("DicClints", false);
                    TreeNode tNodeLinkDic;
                    if (tNodeLinksDic.Length == 1)
                    {
                        tNodeLinkDic = tNodeLinksDic[0];
                    }
                    else
                    {
                        tNodeLinkDic = tvProejcetNode.Nodes.Add("DicClints");
                    }
                    tNodeLinkDic.Name = "DicClints";
                    tNodeLinkDic.Tag = StaticCon.SocketDic;
                    foreach (var item in StaticCon.SocketDic)
                    {
                        tNodeLinkDic.Nodes.Add(item.Key).Tag = item.Value;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public override void DoubleClickUpForm(TabPage tabPage, object data = null)
        {
            SocketConnectForm socketConnectForm = new SocketConnectForm();
            socketConnectForm.TopLevel = false;
            socketConnectForm.Dock = DockStyle.Fill;
            tabPage.Controls.Add(socketConnectForm);
            socketConnectForm.Show();
        }

        public override void initialization()
        {
            ErosConLink.SocketClint.EnumComputers();
            //base.initialization();

            Dictionary<string, SocketClint> ites = new Dictionary<string, SocketClint>();
            foreach (var item in SocketClint.Values)
            {
                if (item != null)
                {
                    item.Dispose();
                    if (!ites.ContainsKey(item.Name))
                    {
                        ites.Add(item.Name, ErosConLink.SocketClint.NewTypeLink(item.NetType));
                        //dynamic dyna = ites[item.Name];
                        ites[item.Name] = Vision2.ErosProjcetDLL.Dynamic.ErosDynamic.PopulateEntityFromCollection(ites[item.Name], item);
                        ites[item.Name].Linking = false;
                        ites[item.Name].initialization();
                    }
                }
            }
            SocketClint = null;
            SocketClint = ites;
            ites = new Dictionary<string, SocketClint>();
            foreach (var item in SocketDic.Values)
            {
                item.Dispose();
                ites.Add(item.Name, ErosConLink.SocketClint.NewTypeLink(item.NetType));
                ites[item.Name] = Vision2.ErosProjcetDLL.Dynamic.ErosDynamic.PopulateEntityFromCollection(ites[item.Name], item);
                ites[item.Name].Linking = false;
                ites[item.Name].initialization();
            }
            SocketDic = ites;
        }

        //public override void SetRunUresControl(Control control,int index=0)
        //{
        //    Control[] dsf= control.Controls.Find("tabControl1", false);
        //    if (dsf.Length==1)
        //    {
        //        TabControl tab = dsf[0] as TabControl;
        //        if (!tab.TabPages.ContainsKey("设备控制"))
        //        {
        //            TabPage tabPage = new TabPage();

        //            foreach (var item in SocketClint)
        //            {
        //                if (item.Value.PLCRun != null)
        //                {
        //                    DebugPLC.InterfacePlcUserControl2 interfacePlcUserControl2
        //                        = new DebugPLC.InterfacePlcUserControl2(item.Value.PLCRun,item.Value.Name);
        //                    tabPage.Controls.Add(interfacePlcUserControl2);
        //                    item.Value.SetRunUresControl(interfacePlcUserControl2);
        //                    interfacePlcUserControl2.Dock = DockStyle.Top;
        //                }
        //            }

        //            tab.TabPages.Add(tabPage);
        //            base.SetRunUresControl(tabPage);
        //        }

        //    }

        //}
        //public override void SetUesrContrsl()
        //{
        //    base.SetUesrContrsl();
        //    Panel tabPage = new Panel();
        //    try
        //    {
        //        foreach (var item in SocketClint)
        //        {
        //            if (item.Value.PLCRun != null&& item.Value.PLCRun.LinkStart !=null && item.Value.PLCRun.LinkStart != "")
        //            {
        //                DebugPLC.InterfacePlcUserControl2 interfacePlcUserControl2
        //                    = new DebugPLC.InterfacePlcUserControl2(item.Value.PLCRun, item.Value.Name);
        //                tabPage.Controls.Add(interfacePlcUserControl2);
        //                //item.Value.SetRunUresControl(interfacePlcUserControl2);
        //                interfacePlcUserControl2.Dock = DockStyle.Top;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    tabPage.Text = "设备控制";
        //    tabPage.Name = "设备控制";
        //    SetRunUresControl(tabPage, 0);
        //}

        /// <summary>
        /// 变量表集合
        /// </summary>
        public Dictionary<string, UClass.ErosValues> DicErosValuess
        {
            get { return StaticCon.DicErosValuess; }
            set { StaticCon.DicErosValuess = value; }
        }

        /// <summary>
        /// 储存TCP链接实例包含地址
        /// </summary>
        public Dictionary<string, SocketClint> SocketClint
        {
            get { return StaticCon.SocketClint; }
            set { StaticCon.SocketClint = value; }
        }

        /// <summary>
        /// 储存TCP链接子链接
        /// </summary>
        public Dictionary<string, SocketClint> SocketDic
        {
            get { return StaticCon.SocketDic; }
            set { StaticCon.SocketDic = value; }
        }

        ///// <summary>
        ///// 获得链接名称集合
        ///// </summary>
        public class LinkNameConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(StaticCon.SocketClint.Keys);
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }

            //}

            ///// <summary>
            ///// 获得链接变量名称集合
            ///// </summary>
            //public class ValusNameConverter : StringConverter
            //{
            //    private string LinkName = string.Empty;

            //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            //    {
            //        return true;
            //    }

            //    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            //    {
            //        try
            //        {
            //            if (true)
            //            {
            //            }
            //            dynamic obj = context.Instance;
            //            if (obj.LingkName != null)
            //            {
            //                LinkName = obj.LingkName;
            //                return new StandardValuesCollection(ErosSocket.ErosConLink.StaticCon.SocketClint[LinkName].KeysValues.DictionaryValueD.Keys);
            //            }
            //            else
            //            {
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //        return new StandardValuesCollection(new string[] { });
            //    }

            //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            //    {
            //        return false;
            //    }
            //}

            ///// <summary>
            ///// 获得地址
            ///// </summary>
            //public class ValuIDConverter : StringConverter
            //{
            //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            //    {
            //        return true;
            //    }

            //    public string LinkName;

            //    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            //    {
            //        try
            //        {
            //            dynamic dynamic = context.Instance;
            //            if (dynamic.LingkName != null)
            //            {
            //                LinkName = dynamic.LingkName;
            //                var dicSort = from objDic in StaticCon.SocketClint[LinkName].KeysValues.DictionaryValueD
            //                              orderby objDic.Value.AddressID ascending
            //                              select objDic.Value.District + objDic.Value.AddressID.ToString();
            //                List<string> listst = dicSort.ToList();
            //                return new StandardValuesCollection(listst);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //        return new StandardValuesCollection(new string[] { });
            //    }

            //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            //    {
            //        return false;
            //    }
            //}
        }
    }

    /// <summary>
    /// 静态类通信类
    /// </summary>
    public class StaticCon
    {
        #region 常量信息

        private const string constName = "nameID";
        private const string constOutIP = "outIP";
        private const string constOutProt = "outPort";
        private const string constNetType = "NetType";
        private const string constValueName = "ValueName";
        private const string constEvent = "Event";
        private const string ConstDefault = "Default";
        private const string ErrLogName = @"\NetLog.txt";

        #endregion 常量信息

        #region 静态对象

        /// <summary>
        /// 调试ID
        /// </summary>
        public static string DebugID { get; set; } = string.Empty;

        public static HslCommunication.Core.IReadWriteNet GetIReadWriteNet(string name)
        {
            if (SocketClint.ContainsKey(name))
            {
                return SocketClint[name].GetRead();
            }
            return null;
        }

        public static SocketClint GetSocketClint(string name)
        {
            if (StaticCon.SocketClint.ContainsKey(name))
            {
                return StaticCon.SocketClint[name];
            }
            return null;
        }

        /// <summary>
        /// 读取指定连接的变量值
        /// </summary>
        /// <param name="linkName">连接名.变量名</param>
        /// <returns></returns>
        public static string GetLingkNameValueString(string linkName)
        {
            try
            {
                if (linkName != null)
                {
                    if (linkName.Contains('.'))
                    {
                        string[] teim = linkName.Split('.');
                        if (teim[1] != "")
                        {
                            if (StaticCon.SocketClint.ContainsKey(teim[0]))
                            {
                                return StaticCon.SocketClint[teim[0]].KeysValues[teim[1]].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return "";
        }

        /// <summary>
        /// 读取指定连接变量名的变量值
        /// </summary>
        /// <param name="linkName">连接名.变量名</param>
        /// <returns></returns>
        public static dynamic GetLingkNameValue(string linkName)
        {
            try
            {
                if (linkName == null)
                {
                    return false;
                }
                bool isC = false;
                string linkAdd = "";
                string type = "";

                if (linkName.Contains(","))
                {
                    linkAdd = linkName.Split(',')[0];
                    type = linkName.Split(',')[1];

                    string[] lingks = linkAdd.Split('.');
                    if (SocketClint.ContainsKey(lingks[0]))
                    {
                        if (SocketClint[lingks[0]].IsConn && lingks[1] != "")
                        {
                            if (lingks.Length >= 2)
                            {
                                isC = SocketClint[lingks[0]].GetIDValue(linkAdd.Remove(0, lingks[0].Length + 1), type, out dynamic dynamic);
                                if (isC)
                                {
                                    return dynamic;
                                }
                            }
                            else if (lingks.Length == 2)
                            {
                                isC = SocketClint[lingks[0]].GetIDValue(linkAdd.Remove(0, lingks[0].Length + 1), type, out dynamic dynamic);
                                if (isC)
                                {
                                    return dynamic;
                                }
                            }
                        }
                    }
                }

                if (linkName.Contains('.'))
                {
                    string[] teim = linkName.Split('.');
                    if (StaticCon.SocketClint.ContainsKey(teim[0]) && StaticCon.SocketClint[teim[0]].KeysValues.DictionaryValueD.ContainsKey(teim[1]))
                    {
                        return StaticCon.SocketClint[teim[0]].KeysValues[teim[1]];
                    }
                }
            }
            catch (Exception)
            {
            }
            return "";
        }

        /// <summary>
        /// 获取连接名集合
        /// </summary>
        /// <returns></returns>
        public static List<string> GetLingkNames()
        {
            return StaticCon.SocketClint.Keys.ToList();
        }

        /// <summary>
        /// 获取指定连接的变量集合
        /// </summary>
        /// <param name="lingkName">连接名</param>
        /// <returns>集合</returns>
        public static List<string> GetLingkNmaeValues(string lingkName)
        {
            if (SocketClint.ContainsKey(lingkName))
            {
                return SocketClint[lingkName].KeysValues.DictionaryValueD.Keys.ToList();
            }
            return new List<string>();
        }

        public static bool GetLingkIDValue(string linkName, string type, out dynamic value)
        {
            value = default(dynamic);
            if (linkName == null)
            {
                return false;
            }
            if (linkName.Contains('.'))
            {
                string[] teim = linkName.Split('.');
                if (SocketClint.ContainsKey(teim[0]))
                {
                    if (SocketClint[teim[0]].IsConn)
                    {
                        return SocketClint[teim[0]].GetIDValue(linkName.Remove(0, teim[0].Length + 1), type, out value);
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pLcPC"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetLingkIDValue(UClass.PLCValue pLcPC, out dynamic value)
        {
            return GetLingkIDValue(pLcPC.LinkName + "." + pLcPC.Addrea, pLcPC.TypeStr, out value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nameaddres"></param>
        /// <param name="typeName"></param>
        /// <param name="lentg"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetLingkIDValueS(string nameaddres, string typeName, UInt16 lentg, out dynamic value)
        {
            value = null;
            if (nameaddres.Contains('.'))
            {
                string[] teim = nameaddres.Split('.');
                if (SocketClint.ContainsKey(teim[0]))
                {
                    if (SocketClint[teim[0]].IsConn)
                    {
                        return SocketClint[teim[0]].GetValues(nameaddres.Remove(0, teim[0].Length + 1), typeName, lentg, out value);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 设置指定连接的变量值
        /// </summary>
        /// <param name="linkName">连接名.变量名</param>
        /// <returns></returns>
        public static bool SetLingkValue(string linkName, string value, out string err)
        {
            err = "";
            try
            {
                if (linkName.Contains('.'))
                {
                    string[] teim = linkName.Split('.');
                    if (SocketClint.ContainsKey(teim[0]))
                    {
                        if (SocketClint[teim[0]].IsConn)
                        {
                            if (!SocketClint[teim[0]].SetValue(teim[1], value, out err))
                            {
                                SocketClint[teim[0]].SetValue(teim[1], value, out err);
                            }
                            SocketClint[teim[0]].GetValue(SocketClint[teim[0]].KeysValues.DictionaryValueD[teim[1]]);
                            if (SocketClint[teim[0]].KeysValues.DictionaryValueD[teim[1]].Value.ToString() == value.ToString())
                            {
                                if (err == "")
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (SocketClint[teim[0]].KeysValues.DictionaryValueD.ContainsKey(teim[1]))
                            {
                                if (UClass.GetTypeValue(SocketClint[teim[0]].KeysValues.DictionaryValueD[teim[1]]._Type, value, out dynamic dva))
                                {
                                    SocketClint[teim[0]].KeysValues[teim[1]] = dva;
                                    return true;
                                }
                            }

                            return false;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        /// <summary>
        /// 设置指定连接的变量值
        /// </summary>
        /// <param name="linkName">连接名.变量名</param>
        /// <returns></returns>
        public static bool SetLingkValue(string linkName, dynamic value, out string err)
        {
            err = "";
            if (linkName.Contains('.'))
            {
                string[] teim = linkName.Split('.');
                if (SocketClint.ContainsKey(teim[0]))
                {
                    if (SocketClint[teim[0]].IsConn)
                    {
                        SocketClint[teim[0]].SetValue(teim[1], value.ToString(), out err);
                        if (err == "")
                        {
                            return true;
                        }
                        SocketClint[teim[0]].SetIDValue(teim[1], value, out err);
                    }
                    else
                    {
                        SocketClint[teim[0]].KeysValues[teim[1]] = value;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 写入指定连接的地址值
        /// </summary>
        /// <param name="Linkaddress">连接名.地址</param>
        /// <param name="typeStr">类型</param>
        /// <param name="value">字符串值</param>
        /// <returns></returns>
        public static bool SetLinkAddressValue(string Linkaddress, string typeStr, string value)
        {
            bool isC = false;

            if (Linkaddress != null && Linkaddress.Contains("."))
            {
                string[] lingks = Linkaddress.Split('.');
                if (SocketClint.ContainsKey(lingks[0]))
                {
                    if (SocketClint[lingks[0]].IsConn)
                    {
                        if (UClass.GetTypeValue(typeStr, value, out dynamic dynamic))
                        {
                            isC = SocketClint[lingks[0]].SetIDValue(Linkaddress.Remove(0, lingks[0].Length + 1), dynamic, out string err);
                            if (isC)
                            {
                                return true;
                            }
                            else
                            {
                                isC = SocketClint[lingks[0]].SetIDValue(Linkaddress.Remove(0, lingks[0].Length + 1), dynamic, out err);
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///  写入指定连接的地址值
        /// </summary>
        /// <param name="pLCd">plc地址类型</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool SetLinkAddressValue(UClass.PLCValue pLCd, string value)
        {
            return SetLinkAddressValue(pLCd.LinkName + "." + pLCd.Addrea, pLCd.TypeStr, value);
        }

        /// <summary>
        /// 写入指定连接的地址值
        /// </summary>
        /// <param name="Linkaddress">连接名.地址</param>
        /// <param name="value">值类型值</param>
        /// <returns></returns>
        public static bool SetLinkAddressValue(string Linkaddress = null, dynamic value = null)
        {
            if (Linkaddress == null)
            {
                return false;
            }
            bool isC = false;
            string linkAdd = "";
            string type = "";
            if (Linkaddress.Contains("."))
            {
                if (Linkaddress.Contains(","))
                {
                    linkAdd = Linkaddress.Split(',')[0];
                    type = Linkaddress.Split(',')[1];

                    UClass.GetTypeValue(type, value.ToString(), out value);
                }
                else
                {
                    linkAdd = Linkaddress;
                }
                string[] lingks = linkAdd.Split('.');
                if (SocketClint.ContainsKey(lingks[0]))
                {
                    if (SocketClint[lingks[0]].IsConn && lingks[1] != "")
                    {
                        if (lingks.Length >= 2)
                        {
                            isC = SocketClint[lingks[0]].SetIDValue(linkAdd.Remove(0, lingks[0].Length + 1), value, out string err);
                            if (isC)
                            {
                                return true;
                            }
                        }
                        else if (lingks.Length == 2)
                        {
                            isC = SocketClint[lingks[0]].SetValue(linkAdd.Remove(0, lingks[0].Length + 1), value, out string err);
                            if (isC)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else if (int.TryParse(Linkaddress, out int result))
            {
            }

            return false;
        }

        /// <summary>
        /// 变量表集合
        /// </summary>
        public static Dictionary<string, UClass.ErosValues> DicErosValuess { get; set; } = new Dictionary<string, UClass.ErosValues>();

        /// <summary>
        /// 储存TCP链接实例包含地址
        /// </summary>
        public static Dictionary<string, SocketClint> SocketClint { get; set; } = new Dictionary<string, SocketClint>();

        /// <summary>
        /// 储存TCP链接子链接
        /// </summary>
        public static Dictionary<string, SocketClint> SocketDic { get; set; } = new Dictionary<string, SocketClint>();

        /// <summary>
        /// 服务器端
        /// </summary>
        public static SocketServer Server { get; set; }

        ///// <summary>
        /////
        ///// </summary>
        //public static ErosUI.GlobalVariable Global_Variable = new ErosUI.GlobalVariable();

        #endregion 静态对象

        /// <summary>
        /// 读取XML文件并连接
        /// </summary>
        public static void StataRunSocket()
        {
            ReadSocketXML(Application.StartupPath + "\\XMLSocket.xml", SocketClint);
            ReadSocketXML(Application.StartupPath + "\\XMLSocketDic.xml", SocketDic);
        }

        /// <summary>
        /// 启动执行方法
        /// </summary>
        public static void StataRunUP()
        {
            string name = Process.GetCurrentProcess().ProcessName;
            int id = Process.GetCurrentProcess().Id;
            Process[] prc = Process.GetProcesses();
            foreach (Process pr in prc)
            {
                if ((name == pr.ProcessName) && (pr.Id != id))
                {
                    pr.Kill();
                }
            }
            //服务器
            Server = new SocketServer(7040);
            //温湿度传感器 csse = new 温湿度传感器();
            StataRunSocket();

            return;
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="messb">错误触发</param>
        public static void ErrerLog(Exception messb)
        {
            //File.AppendAllText(Application.StartupPath + ErrLogName, messb.Message + messb.StackTrace);
            HslCommunication.LogNet.ILogNet LogNet = new HslCommunication.LogNet.LogNetSingle(Application.StartupPath + ErrLogName);
            LogNet.WriteException("", messb);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="messb">日志内容</param>
        public static void ErrerLog(string key, string mess)
        {
            HslCommunication.LogNet.ILogNet LogNet = new HslCommunication.LogNet.LogNetSingle(Application.StartupPath + ErrLogName);
            LogNet.WriteError(key, mess);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="messb">日志内容</param>
        public static void ErrerLog(string mess)
        {
            HslCommunication.LogNet.ILogNet LogNet = new HslCommunication.LogNet.LogNetSingle(Application.StartupPath + ErrLogName);
            LogNet.WriteError(mess);
        }

        /// <summary>
        /// 读取SocketXML集合文件并创建链接到集合
        /// </summary>
        /// <returns></returns>
        public static bool ReadSocketXML(string path, Dictionary<string, SocketClint> keys)
        {      //读取XML文档
            XmlDocument doc = new XmlDocument();
            try
            {
                if (!File.Exists(path))//"XMLSocket.xml"
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                    doc.AppendChild(declaration);
                    XmlElement xmlElement = doc.CreateElement("Sockets");
                    doc.AppendChild(xmlElement);
                    doc.Save(path);
                    return false;
                }
                doc.Load(path);
                //获得根节点
                XmlElement users = doc.DocumentElement;
                //获得根节点下子节点
                XmlNodeList xnl = users.ChildNodes;
                foreach (var item in keys.Keys)
                {
                    if (keys[item] != null)
                    {
                        keys[item].Close();
                    }
                }
                keys.Clear();
                foreach (XmlNode item in xnl)
                {   //读取集合属性
                    Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
                    string type = item.Attributes[ErosConLink.SocketClint.constNetType].Value;
                    dynamic obj = null;

                    if (type.Contains('.'))
                    {
                        obj = assembly.CreateInstance(item.Attributes[ErosConLink.SocketClint.constNetType].Value);
                    }
                    else
                    {
                        obj = assembly.CreateInstance("ErosSocket.ErosConLink." + item.Attributes[ErosConLink.SocketClint.constNetType].Value);
                    }
                    if (obj == null)
                    {
                        MessageBox.Show(item.Attributes[ErosConLink.SocketClint.constNetType].Value + ":类型不存在");
                        continue;
                    }
                    SocketClint socketSturt = obj;

                    socketSturt.Name = item.Attributes[ErosConLink.SocketClint.constName].Value;
                    socketSturt.IP = item.Attributes[ErosConLink.SocketClint.constOutIP].Value;
                    socketSturt.Port = Convert.ToUInt16(item.Attributes[ErosConLink.SocketClint.constOutProt].Value);
                    socketSturt.Event = item.Attributes[ErosConLink.SocketClint.constEvent].Value;
                    socketSturt.ValusName = item.Attributes[ErosConLink.SocketClint.constValueName].Value;
                    socketSturt.NetType = item.Attributes[ErosConLink.SocketClint.constNetType].Value;
                    if (!keys.ContainsKey(socketSturt.Name))
                    {
                        keys.Add(socketSturt.Name, socketSturt);
                        socketSturt.ThreadLink(true);
                    }
                    else
                    {
                        StaticCon.ErrerLog("已经存在相同的键");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                StaticCon.ErrerLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 将链接写入到XML文件
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <returns></returns>
        public static bool WriteSocketXML(DataGridView dataGridView, string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                if (!File.Exists(path))
                {
                    XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                    doc.AppendChild(declaration);
                    XmlElement xmlElement = doc.CreateElement("Sockets");
                    doc.AppendChild(xmlElement);
                    XmlElement Sockete = doc.CreateElement("socket");
                    doc.Save(path);
                }
                doc.Load(path);
                XmlElement SocketList = doc.DocumentElement;
                SocketList.RemoveAll();
                //添加根节点
                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    XmlElement Socket = doc.CreateElement("socket");
                    if (dataGridView.Rows[i].Cells[dataGridView.Columns[constName].Index].Value == null || dataGridView.Rows[i].Cells[dataGridView.Columns[constName].Index].Value.ToString() == "")
                    {
                        continue;
                    }
                    Socket.SetAttribute(constName, dataGridView.Rows[i].Cells[dataGridView.Columns[constName].Index].Value.ToString());
                    Socket.SetAttribute(constOutIP, dataGridView.Rows[i].Cells[dataGridView.Columns[constOutIP].Index].Value.ToString());
                    Socket.SetAttribute(constOutProt, dataGridView.Rows[i].Cells[dataGridView.Columns[constOutProt].Index].Value.ToString());
                    Socket.SetAttribute(constNetType, dataGridView.Rows[i].Cells[dataGridView.Columns[constNetType].Index].EditedFormattedValue.ToString());
                    Socket.SetAttribute(constValueName, dataGridView.Rows[i].Cells[dataGridView.Columns[constValueName].Index].EditedFormattedValue.ToString());
                    Socket.SetAttribute(constEvent, dataGridView.Rows[i].Cells[dataGridView.Columns[constEvent].Index].Value.ToString());
                    Socket.SetAttribute(ConstDefault, dataGridView.Rows[i].Cells[dataGridView.Columns[ConstDefault].Index].Value.ToString());
                    SocketList.AppendChild(Socket);
                }

                doc.Save(path);

                return true;
            }
            catch (Exception es)
            {
                //MessageBox.Show("保存失败" + es.Message.ToString());
            }
            return false;
        }

        /// <summary>
        /// 将DataGridView表格存入XML
        /// </summary>
        /// <param name="pathXml">XML地址</param>
        /// <param name="CreateElement">节点名</param>
        /// <param name="dataGridView">表格</param>
        /// <returns></returns>
        public static bool WrithDataGridViewToXML(string pathXml, string CreateElement, DataGridView dataGridView)
        {
            try
            {
                string pathXML = (pathXml + ".xml");
                XmlDocument doc = new XmlDocument();

                Directory.CreateDirectory(Path.GetDirectoryName(pathXML));//创建文件夹

                if (!File.Exists(pathXML))
                {
                    XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                    doc.AppendChild(declaration);
                    XmlElement xmlElement = doc.CreateElement(CreateElement + "s");
                    doc.AppendChild(xmlElement);
                    doc.Save(pathXML);
                }
                doc.Load(pathXML);
                ///添加根节点
                XmlElement ListXML = doc.DocumentElement;
                ListXML.RemoveAll();
                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    if (dataGridView[0, i].Value == null || dataGridView[0, i].Value.ToString() == "")
                    {
                        continue;
                    }
                    for (int i1 = 0; i1 < dataGridView.Rows[i].Cells.Count; i1++)
                    {
                        if (dataGridView.Rows[i].Cells[i1].Value == null)
                        {
                            dataGridView.Rows[i].Cells[i1].Value = "";
                        }
                    }
                    XmlElement erosValue = doc.CreateElement(CreateElement);
                    for (int i1 = 0; i1 < dataGridView.Columns.Count; i1++)
                    {
                        erosValue.SetAttribute(dataGridView.Columns[i1].DataPropertyName, dataGridView.Rows[i].Cells[i1].Value.ToString());
                    }

                    ListXML.AppendChild(erosValue);
                }
                doc.Save(pathXML);
                return true;
            }
            catch (Exception ex)
            {
                StaticCon.ErrerLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 将XML文件读取到DataGridView
        /// </summary>
        /// <param name="pathXml"></param>
        /// <param name="dataGridView"></param>
        /// <returns></returns>
        public static void ReadDataGridViewToXML(string pathXml, DataGridView dataGridView)
        {
            DataSet xmlDs = new DataSet();
            dataGridView.Rows.Clear();
            if (File.Exists(pathXml + ".xml"))
            {
                xmlDs.ReadXml(pathXml + ".xml");
                if (xmlDs.Tables.Count > 0)
                {
                    foreach (DataRowView item in xmlDs.Tables[0].DefaultView)
                    {
                        int ds = dataGridView.Rows.Add();
                        for (int i2 = 0; i2 < item.DataView.Table.Columns.Count; i2++)
                        {
                            for (int i = 0; i < dataGridView.Columns.Count; i++)
                            {
                                if (item.DataView.Table.Columns[i2].ColumnName == dataGridView.Columns[i].DataPropertyName)
                                {
                                    dataGridView.Rows[ds].Cells[i].Value = item[i2];
                                    continue;
                                }
                            }
                            //dataGridView.Rows[ds].Cells[item.DataView.Table.Columns[i2].ColumnName].Value = item[i2];
                        }
                    }

                    //       dataGridView.DataSource = xmlDs.Tables[0].DefaultView;
                }
            }
            else
            {
                //dataGridView.DataSource = null;
            }
        }

        /// <summary>
        /// Bool数组转换为反整数
        /// </summary>
        /// <param name="barray"></param>
        /// <returns></returns>
        public static int ConvertBoolArrayToInt(bool[] barray)
        {
            int result = 0;
            if (barray != null)
            {
                int len = barray.Length;

                if (len < 33)
                {
            
                    //for (int i = 0; i < barray.Length; i++)
                    //{
                    //    result = (result<<(1   )) + (barray[i] ? 1 : 0);
                    //}
                    foreach (bool b in barray)
                    {
                        result = (result << 1) + (b ? 1 : 0);
                    }
                }
                else
                {
                    Console.WriteLine("bool数组长度大于32，整数只有32位。");
                }
            }
            else
            {
                Console.WriteLine("bool数组为空。");
            }

            return result;
        }
        /// <summary>
        /// bit转为Int
        /// </summary>
        /// <param name="barray"></param>
        /// <returns></returns>
        public static int ConvertBoolArrayInt(bool[] barray)
        {
            int result = 0;
            if (barray != null)
            {
                for (int i = barray.Length -1; i >= 0; i--)
                {
                    result = barray[i] ? result + (1 << i) : result;
                }
           
            }
            else
            {
                Console.WriteLine("bool数组为空。");
            }

            return result;

        }

        /// <summary>
        /// 32位整数转换为bool数组
        /// </summary>
        /// <param name="result"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static bool[] ConvertIntToBoolArray(int result, int len)
        {
            if (len > 32 || len < 0)
            {
                Console.WriteLine("bool数组长度应该在0到32之间。");
            }

            bool[] barray2 = new bool[len];

            for (int i = 0; i < len; i++)
            {
                barray2[i] = ((result >> i) % 2) == 1;
            }
            return barray2;
        }

        /// <summary>
        /// 修改程序在注册表中的键值  开机启动程序
        /// </summary>
        /// <param name="isAuto">true:开机启动,false:不开机自启</param>
        public static void AutoStart(bool isAuto)
        {
            try
            {
                if (isAuto == true)
                {
                    RegistryKey R_local = Registry.LocalMachine;//RegistryKey R_local = Registry.CurrentUser;
                    string strFullPath = Application.ExecutablePath;
                    string strFileName = System.IO.Path.GetFileName(strFullPath);
                    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    R_run.SetValue("ErosSockete", Application.ExecutablePath);
                    R_run.Close();
                    R_local.Close();
                }
                else
                {
                    RegistryKey R_local = Registry.LocalMachine;//RegistryKey R_local = Registry.CurrentUser;
                    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    R_run.DeleteValue("ErosSocket", false);
                    R_run.Close();
                    R_local.Close();
                }
                RegistryKey Rkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                //GlobalVariant.Instance.UserConfig.AutoStart = isAuto;
            }
            catch (Exception)
            {
                //MessageBoxDlg dlg = new MessageBoxDlg();
                //dlg.InitialData("您需要管理员权限修改", "提示", MessageBoxButtons.OK, MessageBoxDlgIcon.Error);
                //dlg.ShowDialog();
                MessageBox.Show("您需要管理员权限修改", "提示");
            }
        }

        public static bool AutoStart(bool isAuto, string Value, string Path)
        {
            try
            {
                if (isAuto == true)
                {
                    RegistryKey R_local = Registry.LocalMachine;//RegistryKey R_local = Registry.CurrentUser;
                    string strFullPath = Application.ExecutablePath;
                    string strFileName = System.IO.Path.GetFileName(strFullPath);
                    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    R_run.SetValue(Value, Path);
                    R_run.Close();
                    R_local.Close();
                    MessageBox.Show("开机启动项:" + Value + "已启动");
                }
                else
                {
                    RegistryKey R_local = Registry.LocalMachine;//RegistryKey R_local = Registry.CurrentUser;
                    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    R_run.DeleteValue(Value, false);
                    R_run.Close();
                    R_local.Close();
                    MessageBox.Show("开机启动项:" + Value + "已关闭");
                }
                RegistryKey Rkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                //GlobalVariant.Instance.UserConfig.AutoStart = isAuto;
                return true;
            }
            catch (Exception)
            {
                //MessageBoxDlg dlg = new MessageBoxDlg();
                //dlg.InitialData("您需要管理员权限修改", "提示", MessageBoxButtons.OK, MessageBoxDlgIcon.Error);
                //dlg.ShowDialog();
                MessageBox.Show("您需要管理员权限修改", "提示");
            }
            return false;
        }

        /// <summary>
        /// 是否可连接
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool connectState(string path)
        {
            return connectState(path, "", "");
        }

        /// <summary>
        /// 连接远程共享文件夹
        /// </summary>
        /// <param name="path">远程共享文件夹的路径</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        public static bool connectState(string path, string userName, string passWord)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = "net use " + path + " " + passWord + " /user:" + userName;
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }

        /// <summary>
        /// 向远程文件夹保存本地内容，或者从远程文件夹下载文件到本地
        /// </summary>
        /// <param name="src">要保存的文件的路径，如果保存文件到共享文件夹，这个路径就是本地文件路径如：@"D:\1.avi"</param>
        /// <param name="dst">保存文件的路径，不含名称及扩展名</param>
        /// <param name="fileName">保存文件的名称以及扩展名</param>
        public static void Transport(string src, string dst, string fileName)
        {
            FileStream inFileStream = new FileStream(src, FileMode.Open);
            if (!Directory.Exists(dst))
            {
                Directory.CreateDirectory(dst);
            }
            dst = dst + fileName;
            FileStream outFileStream = new FileStream(dst, FileMode.OpenOrCreate);
            byte[] buf = new byte[inFileStream.Length];
            int byteCount;
            while ((byteCount = inFileStream.Read(buf, 0, buf.Length)) > 0)
            {
                outFileStream.Write(buf, 0, byteCount);
            }
            inFileStream.Flush();
            inFileStream.Close();
            outFileStream.Flush();
            outFileStream.Close();
        }
    }
}