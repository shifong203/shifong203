using ErosSocket.DebugPLC.Robot;
using ErosSocket.ErosConLink;
using HalconDotNet;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThridLibray;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project.DebugF;
using Vision2.Project.formula;
using Vision2.Project.Mes;
using Vision2.vision.HalconRunFile.Controls;
using static Vision2.Project.formula.UserFormulaContrsl;
using static Vision2.vision.HalconRunFile.PCBFile.PCBLibraryForm;
using static Vision2.vision.HalconRunFile.RunProgramFile.HalconRun;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    [Serializable]
    /// <summary>
    /// 图像处理类
    /// </summary>
    public class

        HalconRun : DicHtuple, IDrawHalcon, IHelp, ProjectNodet.IClickNodeProject, InterfaceVisionControl
    {
        public interface IUpDataVisionWindow
        {
            void UpHalcon(HalconRun halcon = null);

            /// <summary>
            /// 刷新执行图像到窗口
            /// </summary>
            /// <param name="halconResult"></param>
            void UPOneImage(OneResultOBj halconResult);

            void UPImage(OneResultOBj iamge, int runid, Dictionary<string, bool> listResultBool);

            void Focus();

            HWindowControl GetNmaeWindowControl(string name=null);

            void Setprat(int row, int col, int row2, int col2);
        }

        public override string FileName => "Halcon";
        public override string SuffixName => ".eros";

        #region UI控件接口显示方法

        public new void UpProperty(PropertyForm control, object data)
        {
            HalconRunProgram halconRunProgram = new HalconRunProgram(this);
            halconRunProgram.Dock = DockStyle.Fill;
            TabPage tabPage1 = new TabPage();
            tabPage1.Text = "调试界面";
            tabPage1.Name = "调试界面";
            control.tabControl1.TabPages.Insert(0, tabPage1);
            tabPage1.Controls.Add(halconRunProgram);
            //control.tabPage1.Controls.Add(halconRunProgram);
            TabPage tabPage = new TabPage();
            tabPage.Text = "坐标系";
            tabPage.Name = "坐标系";
            control.tabControl1.TabPages.Add(tabPage);
            PropertyGrid property = new PropertyGrid();
            property.Dock = DockStyle.Fill;

            tabPage.Controls.Add(property);
            tabPage.Enter += TabPage_Click;
            void TabPage_Click(object sender, EventArgs e)
            {
                property.SelectedObject = this.CoordinatePXY;
            }
            for (int i = 0; i < ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP.Count; i++)
            {
                if (vision.Vision.GetSaveImageInfo(this.Name).AxisGrot == ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP[i].Name)
                {
                    tabPage = new TabPage();
                    tabPage.Text = "轴机构";
                    tabPage.Name = "轴机构";
                    control.tabControl1.TabPages.Add(tabPage);
                    tabPage.Controls.Add(new ErosSocket.DebugPLC.Robot.Axis4PUserControl1(ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP[i]));
                    break;
                }
            }
            control.tabControl1.SelectedIndex = 0;
        }

        public Control GetThisControl()
        {
            TabControl tabControl = new TabControl();

            return new HalconRunProgram(this) { Dock = DockStyle.Fill };
        }

        /// <summary>
        /// 执行完成委托
        /// </summary>
        public delegate void DelegateOK(OneResultOBj halcon);

        /// <summary>
        /// 添加子程序
        /// </summary>
        public delegate void DelegateAddRun(HalconRun halcon, RunProgram runProgram);

        //public class iMAGERun
        //{
        //    public HObject iamgel;
        //    public int RunID;

        //}
        /// <summary>
        /// 异步采图完成
        /// </summary>
        public delegate void AsyncRestImage(OneResultOBj iamge);

        /// <summary>
        /// 显示集合委托
        /// </summary>
        /// <param name="hObject"></param>
        public delegate HObject UPShowObj(HalconRun halcon, string objName);

        /// <summary>
        /// 更新程序方法到DataGridView
        /// </summary>
        /// <param name="dataGridView1"></param>
        public void UpData(DataGridView dataGridView1)
        {
            try
            {
                var detee = from objDic in this.ListRun
                            orderby objDic.Value.CDID ascending
                            select objDic;
                int i = 0;
                int cont = detee.Count();
                if (detee.Count() == dataGridView1.Rows.Count)
                {
                    for (i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        foreach (var item in detee)
                        {
                            if (dataGridView1.Rows[i].Cells[1].Value.ToString() == item.Value.Name)
                            {
                                dataGridView1.Rows[i].Cells[0].Value = item.Value.CDID;
                                dataGridView1.Rows[i].Cells[1].Value = item.Value.Name;
                                dataGridView1.Rows[i].Cells[2].Value = item.Value.GetType().Name;
                                dataGridView1.Rows[i].Cells[3].Value = "单次执行";
                                dataGridView1.Rows[i].Cells[3].Tag = item.Value;
                                dataGridView1.Rows[i].Cells[4].Value = item.Value.Watch.ElapsedMilliseconds;
                                if (item.Value.ErrBool)
                                {
                                    dataGridView1[4, i].Style.BackColor = Color.Red;
                                }
                                else
                                {
                                    dataGridView1[4, i].Style.BackColor = dataGridView1[3, i].Style.BackColor;
                                }
                            }
                        }
                    }
                }
                else
                {
                    dataGridView1.Rows.Clear();
                    foreach (var item in detee)
                    {
                        i = dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = item.Value.CDID;
                        dataGridView1.Rows[i].Cells[1].Value = item.Value.Name;
                        dataGridView1.Rows[i].Cells[2].Value = item.Value.GetType().Name;
                        dataGridView1.Rows[i].Cells[3].Value = "单次执行";
                        dataGridView1.Rows[i].Cells[3].Tag = item.Value;
                        dataGridView1.Rows[i].Cells[4].Value = item.Value.Watch.ElapsedMilliseconds;
                        if (item.Value.ErrBool)
                        {
                            dataGridView1[4, i].Style.BackColor = Color.Red;
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 更新参数节点
        /// </summary>
        /// <param name = "treeNodet" ></ param >
        public override void UpProjectNode(TreeNode treeNodet)
        {
            try
            {
                base.UpProjectNode(treeNodet);
                //this.Kays_Measure.SyncName();
                Dictionary<string, RunProgram> sds = new Dictionary<string, RunProgram>();

                foreach (var item in ListRun)
                {
                    sds.Add(item.Value.Name, item.Value);
                }
                ListRun = sds;
                ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                Node.ImageKey = Node.SelectedImageKey = "network.png";

                Node.ContextMenuStrip = contextMenuTT;

                Node.ContextMenuStrip = GetNewPrajetContextMenuStrip("");

                /// 添加程序集合nodes
                foreach (var item in this.ListRun)
                {
                    item.Value.Dic_Measure.SyncName();
                    TreeNode[] treeNodeItems = Node.Nodes.Find(item.Value.Name, false);
                    item.Value.SetPThis(this);
                    TreeNode treeNodeItem;
                    if (treeNodeItems.Length != 0)
                    {
                        treeNodeItem = treeNodeItems[0];
                    }
                    else
                    {
                        treeNodeItem = Node.Nodes.Add(item.Value.Name);
                    }
                    treeNodeItem.Name = treeNodeItem.Text;
                    treeNodeItem.Tag = item.Value;
                    contextMenuStrip = new ContextMenuStrip();
                    treeNodeItem.ContextMenuStrip = contextMenuStrip;
                    ToolStripItem toolStripItemAdd = contextMenuStrip.Items.Add("添加测量");
                    toolStripItemAdd.Click += ToolStripItemAdd_Click;
                    /// <summary>
                    /// 添加测量程序
                    /// </summary>
                    /// <param name="sender"></param>
                    /// <param name="e"></param>
                    void ToolStripItemAdd_Click(object sender, EventArgs e)
                    {
                        try
                        {
                            if (item.Value != null)
                            {
                                AddMeasa(item.Value.Dic_Measure, treeNodeItem);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    ToolStripItem toolStripItem = contextMenuStrip.Items.Add("删除程序");
                    toolStripItem.Click += ToolStripItem_Click;
                    void ToolStripItem_Click(object sender, EventArgs e)
                    {
                        DialogResult dr = MessageBox.Show("确定删除" + item.Value.Name + "?", "删除测量程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dr == DialogResult.OK)
                        {
                            if (this.ListRun.ContainsKey(item.Value.Name))
                            {
                                this.ListRun.Remove(item.Value.Name);
                                TreeView.SelectedNode.Remove();
                            }
                        }
                    }
                    foreach (var iteme in item.Value.Dic_Measure.Keys_Measure)
                    {
                        TreeNode[] treeNodeItemss = treeNodeItem.Nodes.Find(iteme.Value.Name, false);
                        TreeNode treeNodeItemt;
                        if (treeNodeItemss.Length != 0)
                        {
                            treeNodeItemt = treeNodeItemss[0];
                        }
                        else
                        {
                            treeNodeItemt = treeNodeItem.Nodes.Add(iteme.Value.Name);
                        }
                        treeNodeItemt.Name = treeNodeItemt.Text;
                        treeNodeItemt.Tag = iteme.Value;
                        contextMenuStrip = new ContextMenuStrip();
                        treeNodeItemt.ContextMenuStrip = contextMenuStrip;
                        ToolStripItem toolStripItemt2 = contextMenuStrip.Items.Add("删除对象");
                        toolStripItemt2.Click += ToolStripItems_Click;
                        ToolStripItem toolStripItemA = contextMenuStrip.Items.Add("重命名");
                        toolStripItemA.Click += ToolStripItemA_Click;
                        void ToolStripItems_Click(object sender, EventArgs e)
                        {
                            DialogResult dr = MessageBox.Show("确定删除" + TreeView.SelectedNode.Text + "?", "删除测量程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dr == DialogResult.OK)
                            {
                                item.Value.Dic_Measure.Keys_Measure.Remove(TreeView.SelectedNode.Text);
                                TreeView.SelectedNode.Remove();
                            }
                        }
                    }
                }
                if (!Node.IsExpanded)
                {
                    Node.Toggle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            /// 修改名称完成
            void ToolStripItemA_Click(object sender, EventArgs e)
            {
                TreeView.SelectedNode.BeginEdit();
            }
        }

        /// <summary>
        /// 刷新程序事件
        /// </summary>
        public event DelegateAddRun UpHalconRunProgram;

        /// <summary>
        /// 刷新程序事件
        /// </summary>
        private void OnUpHalconRunPro(RunProgram run)
        {
            UpHalconRunProgram?.Invoke(this, run);
        }

        public ContextMenuStrip GetNewPrajetContextMenuStrip(string name)
        {
            AddRun("模板", typeof(ModelVision));
            AddRun("测量1", typeof(MeasureMlet));
            AddRun("扫码1", typeof(QRCode));
            AddRun("二值化检测1", typeof(Calculate));
            AddRun("OCR识别1", typeof(Text_Model));
            AddRun("连接器识别", typeof(PinT));
            AddRun("焊点检测", typeof(Welding_Spot));
            AddRun("焊线检测", typeof(Wire_Solder));
            AddRun("颜色检测", typeof(Color_Detection));
            AddRun("擦针检测", typeof(Pin_Round_brush_needlecs));
            AddRun("PCB", typeof(PCBA));
            AddRun("元件", typeof(VisionContainer));
            AddRun("镀层缺陷", typeof(Overgild));
            AddRun("库调用", typeof(PCBFile.PCBAEX));
            if (contextMenuTT.Items.Find("删除", false).Length == 0)
            {
                ToolStripItem toolStripItemw = contextMenuTT.Items.Add("删除");
                toolStripItemw.Click += ToolStripItemw_Click1;
                toolStripItemw.Name = toolStripItemw.Text;
                void ToolStripItemw_Click1(object sender, EventArgs e)
                {
                    try
                    {
                        //ToolStrip toolStrip  =toolStripItemw.GetCurrentParent();
                        Control control = (sender as ToolStripItem).GetCurrentParent();
                        if ((sender as ToolStripItem).Tag is List<string>)
                        {
                            List<string> list = (sender as ToolStripItem).Tag as List<string>;
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (this.ListRun.ContainsKey(list[i]))
                                {
                                    this.ListRun.Remove(list[i]);

                                    this.ListRunName.Remove(list[i]);
                                }
                                if (this.ListRunName.ContainsKey(list[i]))
                                {
                                    this.ListRunName.Remove(list[i]);
                                }
                            }
                            OnUpHalconRunPro(null);
                        }
                        else
                        {
                            Vision.GetHimageList().Remove(this.Name);
                            Vision.Instance.ListHalconName.Remove(this.Name);
                            Vision.Instance.UpProjectNode();
                        }
                    }
                    catch (Exception)
                    {
                    }
                    //弹出带输入
                }
            }
            if (contextMenuTT.Items.Find("同步到库", false).Length == 0)
            {
                ToolStripItem toolStripItemw = contextMenuTT.Items.Add("同步到库");
                toolStripItemw.Click += ToolStripItemw_Click1;
                toolStripItemw.Name = toolStripItemw.Text;
                void ToolStripItemw_Click1(object sender, EventArgs e)
                {
                    try
                    {
                        //ToolStrip toolStrip  =toolStripItemw.GetCurrentParent();
                        Control control = (sender as ToolStripItem).GetCurrentParent();
                        if ((sender as ToolStripItem).Tag is List<string>)
                        {
                            List<string> list = (sender as ToolStripItem).Tag as List<string>;
                            for (int i = 0; i < list.Count; i++)
                            {
                                this.ListRun[list[i]].SaveThis(Library.LibraryBasics.PathStr);
                                Vision.Instance.AddLibrary(this.ListRun[list[i]]);
                            }
                        }
                        else
                        {
                        }
                    }
                    catch (Exception)
                    {
                    }
                    //弹出带输入
                }
            }
            if (contextMenuTT.Items.Find("从库导入", false).Length == 0)
            {
                ToolStripItem toolStripItemw = contextMenuTT.Items.Add("从库导入");
                toolStripItemw.Click += ToolStripItemw_Click1;
                toolStripItemw.Name = toolStripItemw.Text;
                void ToolStripItemw_Click1(object sender, EventArgs e)
                {
                    try
                    {
                        LibraryFormAdd libraryFormAdd = new LibraryFormAdd(this);
                        libraryFormAdd.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                    }
                    //弹出带输入
                }
            }
            return contextMenuTT;
        }

        private void AddRun(string pRName, Type type)
        {
            Vision.AddRunNames(new string[]
                        {
                    "添加"+pRName,
                        });
            if (Vision.FindRunName("添加" + pRName))
            {
                if (contextMenuTT.Items.Find("添加" + pRName, false).Length == 0)
                {
                    ToolStripItem toolStripItemw = contextMenuTT.Items.Add("添加" + pRName);
                    toolStripItemw.Name = toolStripItemw.Text;
                    toolStripItemw.Click += ToolStripItemwT_Click;
                    void ToolStripItemwT_Click(object sender, EventArgs e)
                    {
                        try
                        {
                            //弹出带输入的
                            string sd = Interaction.InputBox("请输入程序名称", "创建程序", pRName, 100, 100);
                            if (sd.Length == 0)
                            {
                                return;
                            }
                            dynamic obj = type.Assembly.CreateInstance(type.FullName);
                            obj.Name = sd;
                            this.AddListRun(sd, obj);
                            OnUpHalconRunPro(obj);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            else
            {
                contextMenuTT.Items.RemoveByKey("添加" + pRName);
            }
        }

        /// <summary>
        ///添加程序
        /// </summary>
        /// <param name="name"></param>
        /// <param name="run"></param>
        public RunProgram AddListRun(string name, RunProgram run)
        {
            try
            {
                if (this.ListRun.ContainsKey(name))
                {
                    string dsts = string.Empty;
                    int ds = ProjectINI.GetStrReturnInt(name, out dsts);
                strt:
                    if (this.ListRun.ContainsKey(dsts + (++ds)))
                    {
                        goto strt;
                    }
                    DialogResult dr = MessageBox.Show(name + ":已存在!是否新建《" + dsts + ds + "》？", "新建程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        string jsonStr = JsonConvert.SerializeObject(ListRun[name]);
                        dsts = dsts + ds;
                        object DA = JsonConvert.DeserializeObject(jsonStr, run.GetType());
                        ListRun.Add(dsts, DA as RunProgram);
                        ListRun[dsts].Name = dsts;
                        float maxd = 1;
                        foreach (var item in ListRun)
                        {
                            if (item.Value.CDID > maxd)
                            {
                                maxd = item.Value.CDID + 1;
                            }
                        }
                        ListRun[dsts].SetPThis(this);
                        ListRun[dsts].CDID = maxd;
                        run.Name = dsts + ds;
                    }
                }
                else
                {
                    this.ListRun.Add(name, run);
                    ListRun[name].SetPThis(this);
                }
            }
            catch (Exception)
            {
            }
            if (Node != null)
            {
                this.UpProjectNode(Node.Parent);
            }
            return run;
        }

        /// <summary>
        ///添加测量
        /// </summary>
        /// <param name="dic_Measure"></param>
        /// <param name="treeNode"></param>
        private void AddMeasa(Dic_Measure dic_Measure, TreeNode treeNode)
        {
            try
            {
                //弹出带输入的
                string name = "测量1";
                foreach (var item in dic_Measure.Keys_Measure)
                {
                    name = item.Key;
                }
                string sd = Interaction.InputBox("请输入程序名称", "创建程序", name, 100, 100);
                if (sd.Length == 0)
                {
                    return;
                }
                treeNode.Nodes.Add(sd).Tag = dic_Measure.Add(sd);
            }
            catch (Exception)
            {
            }
        }

        private RunProgram AddRunProgram(string name)
        {
            if (ListRun.ContainsKey(name))
            {
                string namest = name;
            streatfor:
                string dsts = string.Empty;
                for (int i = name.Length; i > 0; i--)
                {
                    if (int.TryParse(name[i - 1].ToString(), out int dss))
                    {
                        dsts = dss + dsts;
                    }
                    else
                    {
                        string sd = name.Substring(0, name.Length - dsts.Length);
                        int.TryParse(dsts, out int intsd);
                        intsd++;
                        dsts = sd + intsd.ToString();
                        break;
                    }
                }
                if (ListRun.ContainsKey(dsts))
                {
                    name = dsts;
                    goto streatfor;
                }
                string mes = "";
                if (namest == name)
                {
                    mes = namest;
                }
                else
                {
                    mes = namest + "--" + name;
                }
                DialogResult dr = MessageBox.Show(mes + ":已存在!是否新建测量《" + dsts + "》？", "新建测量程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    string jsonStr = JsonConvert.SerializeObject(ListRun[namest]);

                    MeasureMlet measure = new MeasureMlet();
                    JsonConvert.PopulateObject(jsonStr, measure);
                    ListRun.Add(dsts, measure);
                    ListRun[dsts].Name = dsts;
                    name = dsts;
                    return ListRun[dsts];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                ListRun.Add(name, new MeasureMlet());
                ListRun[name].Name = name;
                return ListRun[name];
            }
        }

        #endregion UI控件接口显示方法

        public HalconRun()
        {
            Information = "视觉程序容器类,装载视觉执行程序";
            RunTimeI = 0;
            //HOperatorSet.GenEmptyObj(out HObjectImage);
            //CamNameStr = "default";
            Height = -1;
            Width = -1;
            MRModelHomMat = new ModelVision.RModelHomMat();
            TiffeOffsetImageEX = new TiffeOffsetImageEX();
        }

        public TiffeOffsetImageEX TiffeOffsetImageEX
        {
            get;
            set;
        }

        ~HalconRun()
        {
        }

        #region 机器人链接处理

        /// <summary>
        /// 循环执行次数
        /// </summary>
        [Browsable(false)]
        public int LoopIndx { get; set; } = 0;

        /// <summary>
        /// 循环最大次数
        /// </summary>
        [Browsable(false)]
        public int loopindxMax;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public SocketClint GetSocket(SocketClint socketClint = null)
        {
            if (socketClint != null)
            {
                SocketClint = socketClint;
            }
            return SocketClint;
        }

        /// <summary>
        /// 连接
        /// </summary>
        private SocketClint SocketClint;

        /// <summary>
        ///
        /// </summary>
        public void SendMesage(params string[] mesage)
        {
            try
            {
                LoopIndx = 0;

                if (StaticCon.SocketClint.ContainsKey(this.GetSaveImageInfo().LingkRobotName))
                {
                   GetSocket(StaticCon.SocketClint[this.GetSaveImageInfo().LingkRobotName]);
                }
                if (LoopIndx == 0)
                {
                    if (SocketClint != null)
                    {
                        string data = string.Join(",", mesage);
                        SocketClint.Send(data);
                    }
                    loopindxMax = 0;
                }
                else
                {
                    if (LoopIndx > loopindxMax)
                    {
                        if (SocketClint != null)
                        {
                            string data = string.Join(",", mesage);
                            SocketClint.Send(data);
                        }
                    }
                    loopindxMax++;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 获取机器人的3D位置
        /// </summary>
        /// <returns></returns>
        public HTuple GetRobotBaesPose()
        {
            if (!SocketClint.IsConn)
            {
                return null;
            }
            if (SocketClint is ErosSocket.DebugPLC.Robot.EpsenRobot6)
            {
                ErosSocket.DebugPLC.Robot.EpsenRobot6 epsenRobot6 = SocketClint as ErosSocket.DebugPLC.Robot.EpsenRobot6;

                epsenRobot6.GetPoints(out double x, out double y, out double z, out double u, out double v, out double w);
                if (u < 0)
                {
                    u += 360;
                }
                if (v < 0)
                {
                    v += 360;
                }
                if (w < 0)
                {
                    w += 360;
                }
                HOperatorSet.CreatePose(x / 1000, y / 1000, z / 1000, w, v, u, "Rp+T", "abg", "point", out HTuple pose);
                return pose;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得当前位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public bool GetRobotXYZUVW(out double? x, out double? y, out double? z, out double? u, out double? v, out double? w)
        {
            x = y = z = u = v = w = null;
            if (!SocketClint.IsConn)
            {
                return false;
            }
            if (SocketClint is ErosSocket.DebugPLC.Robot.EpsenRobot6)
            {
                ErosSocket.DebugPLC.Robot.EpsenRobot6 epsenRobot6 = SocketClint as ErosSocket.DebugPLC.Robot.EpsenRobot6;
                epsenRobot6.GetPoints(out double x1, out double y1, out double z1, out double u1, out double v1, out double w1);
                x = x1;
                y = y1;
                z = z1;
                u = u1;
                v = v1;
                w = w1;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion 机器人链接处理

        ///

        #region 区域显示

        /// <summary>
        /// 窗口句柄
        /// </summary>
        private HWindow hWindowHalconID;

        public HWindow hWindowHalcon(HWindow hawid = null)
        {
            if (hawid != null)
            {
                hWindowHalconID = hawid;
            }
        
            return hWindowHalconID;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IUpDataVisionWindow GetHWindow()
        {
            return VisionWindow;
        }

        public void SetWindow(IUpDataVisionWindow control)
        {
            VisionWindow = control;
        }

        private IUpDataVisionWindow VisionWindow;

        /// <summary>
        /// 执行显示委托
        /// </summary>
        /// <param name="hObject"></param>
        public virtual HObject POnShowObj(HalconRun halcon, string objName)
        {
            try
            {
                this.ObjName = objName;
                EventShowObj?.Invoke(halcon, objName);
                if (WhidowAdd)
                {
                    HOperatorSet.GetImageSize(halcon.GetOneImageR().Image, out HTuple width, out HTuple heigth);
                    halcon.Width = width;
                    halcon.Height = heigth;
                    HOperatorSet.SetPart(halcon.hWindowHalcon(), 0, 0, halcon.Height - 1, halcon.Width - 1);
                }
                this.ShowObj();
                return null
                    ;
            }
            catch (Exception ex)
            {
            }
            return null;
        }



        [Description(""), Category("预处理"), DisplayName("预处理模式")]

        public string PretreamtmentMode { get; set; } = "不执行";
        public PretreatmentVsiion Pretreatment { get; set; } = new PretreatmentVsiion();


        [Description("NG后再次拍照确认次数"), Category("结果处理"), DisplayName("NG确认次数")]
        public sbyte ResultNGNumber
        {
            get;
            set;
        } = 0;
        [Description("首次初始化产品"), Category("结果处理"), DisplayName("初始化产品")]
        public bool RestOneP { get; set; } 

        [Description("执行ID"), Category("结果处理"), DisplayName("最大执行ID")]
        public sbyte MaxRunID { get; set; } = 1;

        [Description("单个产品拍照数量"), Category("结果处理"), DisplayName("单个产品拍照数量")]
        public sbyte PaleID { get; set; } = 1;

        [Description("使用Pale"), Category("结果处理"), DisplayName("使用Pale")]
        public bool PaleMode { get; set; }

        [Description("使用托盘ID，0一下时不使用"), Category("结果处理"), DisplayName("使用托盘的ID")]
        public sbyte TrayID { get; set; } = -1;

        [Description("小图像显示大小,宽高比例按1.3|1 "), Category("结果显示"), DisplayName("小图标大小")]
        public int Form2Heigth { get; set; } = 350;

        public string ImagePaths { get; set; } = "";

        
        public List<List<Cams.CamData>> ListCamData { get; set; } = new List<List<Cams.CamData>>();

        /// <summary>
        /// 整盘集合
        /// </summary>
        private Queue<ImagesOneRun> ImageTs = new Queue<ImagesOneRun>();

        public Queue<ImagesOneRun> GetRuns()
        {
            return ImageTs;
        }

        public List<OneResultOBj> GetRunsOne()
        {
            List<OneResultOBj> oneResults = new List<OneResultOBj>();
            for (int i = 0; i < ImageTs.Count; i++)
            {
                oneResults.Add(ImageTs.ToArray()[i].ImageTs);
            }
            return oneResults;
        }

        public bool AysBing { get; private set; }

        public bool AysEndble { get; set; }

        public void AysnetRun()
        {
            if (Vision.Instance. ISVisinList)
            {
                Thread thread = new Thread(() => {
                    if (AysBing)
                    {
                        return;
                    }
                    AysBing = true;
                    AysEndble = true;
                    while (AysEndble)
                    {
                        try
                        {
                            if (!AysBing) AysBing = true;
                            if (ImageTs.Count != 0)
                            {
                                ImagesOneRun resultOBj = ImageTs.Dequeue();
                      
                                if (this.PretreamtmentMode == "覆盖原图")
                                {
                                    resultOBj.ImageTs.Image = this.Pretreatment.GetEmset(resultOBj.ImageTs.Image);
                                }
                                CamImageEvent(resultOBj.ImageTs);
                            }
                        }
                        catch (Exception ex)
                        { }
                        Thread.Sleep(1);
                    }
                    AysBing = false;
                });
                thread.IsBackground = true;
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
        }

        public void AysnetCam(int liyID, int runID, HObject imaged = null)
        {
            try
            {
                ImagesOneRun image = null;
                if (imaged == null)
                {
                    if (this.GetCam().GetImage(out  image))
                    {
                        image.ImageTs.LiyID = liyID;
                        image.ImageTs.RunID = runID;
                        Task task = new Task(() => {
                            image.GetImage(this.GetCam());
                            ImageTs.Enqueue(image);
                        });
                        task.Start();
                    }
                    else
                    {
                        throw (new Exception("采图失败;"));
                    }
                }
                else
                {
                    image = new ImagesOneRun(imaged, liyID, runID);
                    ImageTs.Enqueue(image);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// 图像队列
        /// </summary>
        public class ImagesOneRun
        {
            public ImagesOneRun(IGrabbedRawData grabbedRawData, Cams.ICamera camera, int liyID, int runID)
            {
                camera1 = camera;
                images = grabbedRawData;
                ImageTs.LiyID = liyID;
                ImageTs.RunID = runID;
                ImageTs.CamNewTime = DateTime.Now;
            }
            public ImagesOneRun(IGrabbedRawData grabbedRawData)
            {
                images = grabbedRawData;
                ImageTs.CamNewTime = DateTime.Now;
            }
            public IGrabbedRawData GetData()
            {
                return images;
            }
            public ImagesOneRun(HObject grabbedRawData, int liyID=0, int runID=0)
            {
                ImageTs.Image = grabbedRawData;
                ImageTs.LiyID = liyID;
                ImageTs.RunID = runID;
            }

            public OneResultOBj ImageTs = new OneResultOBj();

            private IGrabbedRawData images;
            Cams.ICamera camera1;

            public HObject GetImage(Cams.ICamera camera)
            {
                try
                {
                 
                    if (images != null)
                    {
                        ImageTs.Image = camera.IGrabbedRawDataTOImage(images);  
                    }
                    return ImageTs.Image;
                }
                catch (Exception)
                {
                }
                return ImageTs.Image;
            }
        }

        private static object lokt = new object();

        public HObject Image(HObject hObject = null)
        {
            if (hObject != null)
            {
                if (hObject != GetOneImageR().Image)
                {
                    ImageHdt(hObject);
                }
                if (GetOneImageR().Image != null)
                {
                    //imag.Dispose();
                }
                GetOneImageR().Image = hObject;
            }
            return GetOneImageR().Image;
        }

        private HObject R = new HObject();

        private HObject G = new HObject();

        private HObject B = new HObject();

        private HObject H = new HObject();

        private HObject S = new HObject();

        private HObject V = new HObject();

        private HObject Gray = new HObject();

        private HObject ImagePret = new HObject();

        private HObject Image3 = new HObject();

        private int ImageCountChannels = 1;

        /// <summary>
        /// 图像通道分解
        /// </summary>
        /// <param name="hObject"></param>
        public void ImageHdt(HObject hObject=null)
        {
            try
            {
                if (hObject==null)
                {
                    hObject = this.Image();
                }
                if (Image3!=null&&  Image3.Equals(this.Image()))
                {
                    return;
                }
                //this.Image(hObject);
                Image3 = hObject;
                HOperatorSet.CountChannels(hObject, out HTuple htcon);
                ImageCountChannels = htcon.TupleInt();
                if (ImageCountChannels == 3)
                {
                    if (R!=null)
                    {
                        R.Dispose();
                    }
                    G.Dispose();
                    B.Dispose();
                    H.Dispose();
                    S.Dispose();
                    V.Dispose();
                    Gray.Dispose();
                    HOperatorSet.Decompose3(hObject, out R, out G, out B);
                    HOperatorSet.TransFromRgb(R, G, B, out H, out S, out V, "hsv");
                    HOperatorSet.Rgb1ToGray(hObject, out Gray);
                }
                if (PretreamtmentMode == "新图像")
                {
                    ImagePret = this.Pretreatment.GetEmset(hObject);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void ImageDistop()
        {
            try
            {
                if (R != null)
                {
                    R.Dispose();
                }
                G.Dispose();
                B.Dispose();
                H.Dispose();
                S.Dispose();
                V.Dispose();
                Gray.Dispose();
            }
            catch (Exception)
            {
            }
        }

        public string[] GetHoamgeName()
        {
            List<string> Names = new List<string>();
            Names.Add("Image3");
            if (PretreamtmentMode != "不执行")
            {
                Names.Add(ImageTypeObj.ImagePretreatment.ToString());
            }
            HOperatorSet.CountChannels(Image(), out HTuple htcon);
            if (htcon == 3)
            {
                Names.Add("Gray");
                Names.Add("R");
                Names.Add("G");
                Names.Add("B");
                Names.Add("H");
                Names.Add("S");
                Names.Add("V");
            }
            return Names.ToArray();
        }
        /// <summary>
        /// 获取通道的图像
        /// </summary>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public HObject GetImageOBJ(ImageTypeObj imageType)
        {
       
            if (Image3 == null)
            {
                ImageHdt(GetOneImageR().Image);
            }
            if (ImageCountChannels!=3)
            {
                return GetOneImageR().Image;
            }
            switch (imageType)
            {
                case ImageTypeObj.Image3:
                    return GetOneImageR().Image;
                case ImageTypeObj.ImagePretreatment:
                    return ImagePret;
                case ImageTypeObj.Gray:
                    return Gray;

                case ImageTypeObj.R:
                    return R;

                case ImageTypeObj.G:
                    return G;

                case ImageTypeObj.B:
                    return B;

                case ImageTypeObj.H:
                    return H;

                case ImageTypeObj.S:
                    return S;

                case ImageTypeObj.V:
                    return V;
            }
            return GetOneImageR().Image;
        }

        [Category("图像属性")]
        public int Width { get; set; } = 3648;

        [Category("图像属性")]
        public int Height { get; set; } = 2736;

        [Description("2D坐标彷射参数"), Category("坐标系统"), DisplayName("坐标系统")]
        public Coordinate CoordinatePXY
        {
            get
            {
                if (Vision.Instance.DicCoordinate.ContainsKey(CoordinateName))
                {
                    return Vision.Instance.DicCoordinate[CoordinateName];
                }
                else
                {
                    CoordinateName = "默认坐标";
                    if (!Vision.Instance.DicCoordinate.ContainsKey(CoordinateName))
                    {
                        Vision.Instance.DicCoordinate.Add(CoordinateName, new Coordinate());
                    }
                    return Vision.Instance.DicCoordinate[CoordinateName];
                }
            }
            set
            {
                if (!Vision.Instance.DicCoordinate.ContainsKey(CoordinateName))
                {
                    Vision.Instance.DicCoordinate.Add("默认坐标", new Coordinate());
                }
                Vision.Instance.DicCoordinate[CoordinateName] = value;
            }
        }

        [Description("2D坐标彷射参数"), Category("坐标系统"), DisplayName("坐标系统"),
            TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("ListCoordinateName")]
        public string CoordinateName { get; set; } = "默认坐标";

        /// <summary>
        /// 坐标集合名称
        /// </summary>
        public static List<string> ListCoordinateName
        {
            get
            {
                return Vision.Instance.DicCoordinate.Keys.ToList();
            }
        }

        /// <summary>
        /// 总计时
        /// </summary>
        private System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        public System.Diagnostics.Stopwatch GetStopwatch()
        {
            return watch;
        }

        #endregion 区域显示

        #region 程序机制

        /// <summary>
        /// 程序文件夹地址
        /// </summary>
        [Browsable(false)]
        public string ProgramPathD { get { return Vision.GetFilePath() + this.Name; } }

        /// <summary>
        /// 返回指定程序
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public RunProgram GetRunListKey(string name)
        {
            if (ListRun.ContainsKey(name))
            {
                return ListRun[name];
            }
            else
            {
                return null;
            }
        }

        public Dictionary<string, RunProgram> GetRunProgram()
        {
            try
            {
                foreach (var item in ListRun)
                {
                    if (!ListRunName.ContainsKey(item.Key))
                    {
                        ListRunName.Add(item.Key, item.Value.Type);
                    }
                }
            Color_Detection:
                foreach (var item in ListRunName)
                {
                    if (!ListRun.ContainsKey(item.Key))
                    {
                        ListRunName.Remove(item.Key);
                        goto Color_Detection;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return ListRun;
        }

        [Browsable(false)]
        public Dictionary<string, string> ListRunName { get; set; } = new Dictionary<string, string>();

        [Browsable(false)]
        private
        /// <summary>
        /// 程序接口集合
        /// </summary>
        Dictionary<string, RunProgram> ListRun = new Dictionary<string, RunProgram>();

        //[Browsable(false)]
        //public Dic_Measure Kays_Measure = new Dic_Measure();

        /// <summary>
        /// 初始化任务
        /// </summary>
        public override void initialization()
        {
            //this.Height = this.Width = -1;
            this.HobjClear();
            this.MRModelHomMat = new ModelVision.RModelHomMat();
            this.SetDefault("OKNumber", 0, true);
            this.SetDefault("NGNumber", 0, true);
            string dss = ProjectINI.ProjectPathRun + "\\item\\" + this.Name;
            if (Directory.Exists(Vision.VisionPath + "\\Image\\"))
            {
                string[] files = Directory.GetFiles(Vision.VisionPath + "\\Image\\", "*.*", SearchOption.AllDirectories);
                //.Where(s => 
                //(s.EndsWith(".bmp") /*|| s.EndsWith(".jpg")*/) 
                ///*&& s.Contains(Name)*/);
                files=    Array.FindAll(files, (s) => (s.EndsWith(".bmp") || s.EndsWith(".jpg")) && s.Contains(Name));
                if (files.Length!=0)
                {
                    this.ReadImage(files.ToArray()[0].ToString());
                }
   
            }
            AysnetRun();
            SetCamPraegrm();
            //SetExposureTime();
            ObjName = "结果区域";
            WhidowAdd = true;
            buys = false;
            CycleRun();
        }

        public float RunID;

        /// <summary>
        /// 库位号
        /// </summary>
        public List<string> RunName { get; set; } = new List<string>();

        /// <summary>
        /// 库ID
        /// </summary>

        public List<string> RunIDStr { get; set; } = new List<string>();

        /// <summary>
        /// 导航图名称
        /// </summary>
        public List<string> ReNmae { get; set; } = new List<string>();

        private List<OneResultOBj> ResultOBjs = new List<OneResultOBj>();

        public List<OneResultOBj> GetResObj()
        {
            return ResultOBjs;
        }

        /// <summary>
        /// 关闭任务
        /// </summary>
        public override void Close()
        {
        }

        /// <summary>
        /// 执行开始方法
        /// </summary>
        public void UPStart()
        {
            try
            {
                this.HobjClear();
                watch.Restart();
                buys = true;
                //OneImage = new OneResultOBj();
                //halconResult.ClearAllObj();
                //OneImage.HalconResult = halconResult;
                //ResultBool = false;
                Vision.TriggerSetup(Vision.GetSaveImageInfo(this.Name).RunDoneName, false.ToString());
                // this.ClerItem();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 执行结束方法
        /// </summary>
        public virtual void EndChanged(OneResultOBj oneResultOBj, int runID = 0)
        {
            this.TrayRestData.aOK = false;
            ResultBool = false;
            if (this.Result.Contains("OK"))
            {
                ResultBool = this.TrayRestData.aOK = true;
                ResultOK(oneResultOBj);
            }
            else
            {
                ResultNG(oneResultOBj);
      
            }
            TrayRestData.ListReselt.Add(this.TrayRestData.aOK);
            OneImage.OK = ResultBool;
            TrayRestData.Name = this.Name;
            TrayRestData.MaxNumber = MaxRunID;
            //if (ResultO.NGMestage.Length != 0)
            //{
            //    ResultO.AddMeassge("NG信息:" + ResultO.NGMestage);
            //}
            Vision.TriggerSetup(Vision.GetSaveImageInfo(this.Name).RunDoneName, true.ToString());
            this.RunTimeI = watch.ElapsedMilliseconds;
            OnEventDoen(oneResultOBj);
            if (RunName.Count != 0 && RunName.Count >= runID && runID > 0)
            {
                oneResultOBj.AddMeassge("ID" + runID + "," + RunName[(int)runID - 1] + ";" + watch.ElapsedMilliseconds + ",结果:" + this.Result);
            }
            else
            {
                oneResultOBj.AddMeassge("ID" + runID + ";" + watch.ElapsedMilliseconds + ",结果:" + this.Result);
            }
            if (!ProjectINI.DebugMode)
            {
                this.ObjName = "结果区域";
            }
            GC.Collect();
            buys = false;
            Drawing = false;
            watch.Stop();
        }

        /// <summary>
        /// 运行程序的最大ID
        /// </summary>
        private float RunMaxID;

        ///// <summary>
        ///// 执行完成标志
        ///// </summary>
        //private bool threadOK;
        [DescriptionAttribute("执行状态。"), Category("执行状态"), DisplayName("程序执行中"), Browsable(false)]
        public bool Buys { get { return buys; } }

        /// <summary>
        /// 忙碌状态
        /// </summary>
        private bool buys;

        public SaveImageInfo GetSaveImageInfo()
        {
            return Vision.GetSaveImageInfo(this.Name);
        }

        public HTuple GetCaliConstMM(HTuple pint)
        {
            if (this.GetCam() != null)
            {
                if (this.GetCam().CaliConst == 0)
                {
                    this.GetCam().CaliConst = 1;
                }
                return pint.TupleMult(this.GetCam().CaliConst).TupleMult(Vision.Instance.Transform);
            }
            return pint.TupleMult(1).TupleMult(Vision.Instance.Transform);
        }

        public HTuple GetCaliConstPx(HTuple pint)
        {
            if (this.GetCam() != null)
            {
                if (this.GetCam().CaliConst == 0)
                {
                    this.GetCam().CaliConst = 1;
                }
                return pint.TupleDiv(this.GetCam().CaliConst);
            }
            return pint.TupleDiv(1);
        }

        public Coordinate GetCalib()
        {
            try
            {
                if (Vision.Instance.DicCoordinate.ContainsKey(this.CoordinateName))
                {
                    return Vision.Instance.DicCoordinate[this.CoordinateName];
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        #endregion 程序机制

        [Browsable(false)]
        /// <summary>
        /// 实时处理中
        /// </summary>
        public bool Strating { get { return strating; } }

        [DescriptionAttribute("执行超时。"), Category("执行状态"), DisplayName("超时时间")]
        public int OutTime { get; set; } = 7000;

        #region 触发机制

        private bool strating;

        //Thread ThreadSatrReadCam;

        #endregion 触发机制

        #region 结果处理

        /// <summary>
        /// 读取历史结果
        /// </summary>
        /// <returns></returns>
        public List<string> GetListResult()
        {
            return ListResult;
        }

        private List<string> ListResult = new List<string>();

        /// <summary>
        /// 保存参数
        /// </summary>
        public Dictionary<string, HTuple> WriteDataCName = new Dictionary<string, HTuple>();

        /// <summary>
        /// 判断结果
        /// </summary>
        [Browsable(true)]
        [Description("结果状态"), Category("结果显示"), DefaultValue("Null"), DisplayName("执行结果")]
        public string Result { get; set; } = "Null";

        public bool ResultBool { get; set; }

        [DescriptionAttribute("结果区域名称。"), Category("结果显示"), DisplayName("显示结果区域名称"), Browsable(false)]
        /// <summary>
        /// 结果区域
        /// </summary>
        public string ObjName
        {
            get;
            set;
        } = string.Empty;

        [DescriptionAttribute("图像是否固定。"), Category("结果显示"), DisplayName("图像是否固定显示"), Browsable(false)]
        /// <summary>
        /// 动态对象
        /// </summary>
        public bool WhidowAdd { get; set; } = true;

        [DescriptionAttribute("执行时间。"), Category("结果显示"), DisplayName("运行时间")]
        /// <summary>
        /// 运行时间
        /// </summary>
        public float RunTimeI { get; set; }
        

        #endregion 结果处理

        #region 绘制图像

        [Browsable(false)]
        /// <summary>
        /// 绘制中
        /// </summary>
        public bool Drawing { get; set; }

        [Browsable(false)]
        public int DrawType { get; set; }

        [Browsable(false)]
        public bool DrawErasure { get; set; }

        public void Focus()
        {
            this.GetHWindow().Focus();
        }

        /// <summary>
        /// 绘制委托
        /// </summary>
        /// <param name="delegate">true绘制成功</param>
        public bool DrawIng(Func<IDrawHalcon, HObject> @delegate, out HObject hObject)
        {
            hObject = new HObject();
            hObject.GenEmptyObj();
            try
            {
                this.GetHWindow().Focus();
                if (Drawing)
                {
                    MessageBox.Show("绘制中错误,请在绘制图像区域点击鼠标右键，结束绘制");
                    return false;
                }
                Drawing = true;
                hObject = @delegate(this);
                if (hObject != null && hObject.IsInitialized())
                {
                    this.ShowObj(hObject);
                }
                return hObject.IsInitialized();
            }
            catch (Exception ex)
            {
                MessageBox.Show("绘制中错误" + ex.Message);
            }
            //Drawing = false;
            return false;
        }

        /// <summary>
        /// 绘制委托
        /// </summary>
        /// <param name="delegate">true绘制成功</param>
        public bool DrawMoeIng(Func<HalconRun, HObject, HObject> @delegate, ref HObject hObject)
        {
            if (hObject == null)
            {
                hObject = new HObject();
            }
            if (!hObject.IsInitialized())
            {
                hObject.GenEmptyObj();
            }
            if (Drawing)
            {
                return false;
            }
            Drawing = true;
            try
            {
                hObject = @delegate(this, hObject);
                Drawing = false;
                if (hObject != null && hObject.IsInitialized())
                {
                    this.ShowObj(hObject);
                }
                return hObject.IsInitialized();
            }
            catch (Exception ex)
            {
                MessageBox.Show("绘制中错误" + ex.Message);
            }
            Drawing = false;
            return false;
        }

        public Dictionary<string, ModelVision.RModelHomMat> GDicModelR()
        {
            return dicModelR;
        }

        /// <summary>
        /// 模板集合
        /// </summary>
        private Dictionary<string, ModelVision.RModelHomMat> dicModelR = new Dictionary<string, ModelVision.RModelHomMat>();

        public HTuple GetHomdeMobel(string name)
        {
            if (dicModelR.ContainsKey(name))
            {
                if (dicModelR[name].HomMat.Count >= 1)
                {
                    return dicModelR[name].HomMat[0];
                }
            }
            return null;
        }

        public ModelVision.RModelHomMat GetHomdeMobelEx(string name)
        {
            if (dicModelR.ContainsKey(name))
            {
                if (dicModelR[name].HomMat.Count >= 1)
                {
                    return dicModelR[name];
                }
            }
            return null;
        }

        public HObject GetModelHaoMatRegion(string name, HObject hObject)
        {
            if (GetHomdeMobel(name) == null)
            {
                return hObject;
            }
            HOperatorSet.AffineTransRegion(hObject, out HObject hObject1, GetHomdeMobel(name), "nearest_neighbor");
            return hObject1;
        }

        [DescriptionAttribute("模板结果集合。"), Category("结果显示"), DisplayName("模板处理结果")]
        public ModelVision.RModelHomMat MRModelHomMat { get; set; }

        #endregion 绘制图像

        #region 错误管理

        ///// <summary>
        ///// 写入错误日志
        ///// </summary>
        ///// <param name="ex"></param>
        //public void LogErr(Exception ex)
        //{
        //    statrt:
        //    try
        //    {
        //        LogErr("未指定名的错误：", ex);
        //    }
        //    catch (Exception)
        //    {
        //        goto statrt;
        //    }
        //}

        ///// <summary>
        ///// 写入错误日志
        ///// </summary>
        ///// <param name="ex"></param>
        //public void LogErr(string name, Exception ex)
        //{
        //   statrt:
        //    try
        //    {
        //        if (ProjectINI.In.Run_Mode == ProjectINI.RunMode.Debug)
        //        {
        //            Vision2.ErosProjcetDLL.Project.AlarmText.LogErr(name + "，错误信息：" + ex.Message, this.Name);
        //        }
        //        Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(this.Name + "," + name + "，错误信息：" + ex.Message);
        //        logNet.WriteException(name, ex);
        //    }
        //    catch (Exception)
        //    {
        //        goto statrt;
        //    }
        //}

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="ex"></param>
        public void LogErr(string name, string ex)
        {
        statrt:
            try
            {
                if (ProjectINI.In.Run_Mode == ProjectINI.RunMode.Debug)
                {
                    MessageBox.Show(name + ":" + ex);
                }
                Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(name + ":" + ex);
                logNet.WriteError(name, ex);
            }
            catch (Exception)
            {
                goto statrt;
            }
        }

        /// <summary>
        /// 静态错误日志
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ex"></param>
        public static void StaticErrLog(Exception ex)
        {
        statrt:
            try
            {
                logNet.WriteException("静态登记：", ex);
            }
            catch (Exception)
            {
                goto statrt;
            }
        }

        /// <summary>
        /// 静态错误登录
        /// </summary>
        private static HslCommunication.LogNet.ILogNet logNet = new HslCommunication.LogNet.LogNetSingle(Application.StartupPath + @"\HalconErrLog.txt");

        #endregion 错误管理

        #region 显示处理

        /// <summary>
        /// 显示事件
        /// </summary>
        public event UPShowObj EventShowObj;

        /// <summary>
        /// 执行完成事件
        /// </summary>
        public event DelegateOK EventDoen;

        /// <summary>
        /// OK事件
        /// </summary>
        public event DelegateOK EventOK;

        /// <summary>
        /// NG事件
        /// </summary>
        public event DelegateOK EventNG;

        /// <summary>
        /// 执行完成事件
        /// </summary>
        public void OnEventDoen(OneResultOBj oneResultOBj)
        {
            EventDoen?.Invoke(oneResultOBj);
        }

        /// <summary>
        ///
        /// </summary>
        public void OnEventOK(OneResultOBj oneResultOBj)
        {
            EventOK?.Invoke(oneResultOBj);
        }

        /// <summary>
        ///
        /// </summary>
        public void OnEventNG(OneResultOBj oneResultOBj)
        {
            EventNG?.Invoke(oneResultOBj);
        }

        /// <summary>
        ///  清除临时区域，每次拍照清除
        /// </summary>
        public void HobjClear()
        {
            this.Result = "Null";
            //this.TrayRestData = new DataReseltBase();
            this.WriteDataCName.Clear();
            this.MRModelHomMat = new ModelVision.RModelHomMat();
            this.OneImage.ClearAllObj();
        }

        /// <summary>
        /// 清除临时图片，手动清除，或指令清除
        /// </summary>
        public void ListObjCler()
        {
            //this.OneImage = new OneResultOBj();
            try
            {
                this.TrayRestData = new DataReseltBase();
                OneImage = new OneResultOBj();
                if (this.OneCamData!=null)
                {
                    //this.OneCamData.GetImagePlus().Dispose();
                }
                if (this.OnePatrData!=null)
                {
                    this.OnePatrData = new OneDataVale();
                }
                this.ResultOBjs.Clear();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hObject"></param>
        /// <param name="colorResult"></param>
        public void AddObj(HObject hObject, ColorResult colorResult = ColorResult.green)
        {
            this.OneImage.AddObj(hObject, colorResult);
        }

        public void AddObj(HObject hObject, HTuple corolH)
        {
            this.OneImage.AddObj(hObject, corolH);
        }

        public void AddObj(ObjectColor objectColor)
        {
            AddObj(objectColor._HObject, objectColor.HobjectColot);
        }

        //public void AddObj(HObject hObject)
        //{
        //    AddObj(hObject, ColorResult.green);
        //}
        /// <summary>
        /// 传递区域并显示结果区域
        /// </summary>
        /// <param name="hObject"></param>
        public void AddShowObj(HObject hObject)
        {
            this.AddObj(hObject);
            this.ShowObj();
        }

        private object look = new object();

        /// <summary>
        /// 显示自己的图片
        /// </summary>
        public void ShowImage(bool fill = false)
        {
            lock (look)
            {
                try
                {
                    HSystem.SetSystem("flush_graphic", "false");
                    HOperatorSet.ClearWindow(this.hWindowHalcon());
                    HSystem.SetSystem("flush_graphic", "true");
                    if (Vision.IsObjectValided(this.OneImage.Image))
                    {
                        if (fill)
                        {
                            HOperatorSet.GetImageSize(OneImage.Image, out HTuple width, out HTuple heigth);
                            this.Width = width;
                            this.Height = heigth;
                            HOperatorSet.SetPart(this.hWindowHalcon(), 0, 0, Height - 1, Width - 1);
                        }
                        HOperatorSet.DispObj(OneImage.Image, this.hWindowHalcon());
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        /// <summary>
        /// 读取指定地址的图片到程序并显示
        /// </summary>
        /// <param name="path"></param>
        public void ShowImage(string path)
        {
            if (File.Exists(path))
            {
                HOperatorSet.ReadImage(out HObject hObject, path);
                this.GetOneImageR().Image = hObject;
                this.GetOneImageR().CamNewTime = DateTime.Now;
            }
            else
            {
                this.OneImage.AddMeassge("未找到文件：" + path);
            }
            ShowImage();
            ShowMessage();
        }

        /// <summary>
        /// 传递图片并显示在实例窗口
        /// </summary>
        /// <param name="image"></param>
        public void ShowImage(HObject imaget)
        {
            if (Vision.IsObjectValided(imaget))
            {
                Image(imaget);
                HOperatorSet.GetImageSize(Image(), out HTuple width, out HTuple heigth);
                this.Width = width;
                this.Height = heigth;
                HOperatorSet.SetPart(this.hWindowHalcon(), 0, 0, Height - 1, Width - 1);
                HOperatorSet.ClearWindow(this.hWindowHalcon());
                HOperatorSet.DispObj(Image(), this.hWindowHalcon());
                //this.Image = image;
            }
        }

        /// <summary>
        /// 更新坐标并显示文本
        /// </summary>
        public void ShowMessage()
        {
            this.CoordinatePXY.ShowCoordinate(this);
            //ResultOBj.ShowAll(this.hWindowHalcon());
        }

        public void AddImageMassage(HTuple rows, HTuple columns, HTuple message, ColorResult colorResult = ColorResult.green, string but = "false")
        {
            OneImage.AddImageMassage(rows, columns, message, colorResult, but);
        }

        public void AddMeassge(HTuple message)
        {
            OneImage.AddMeassge(message);
        }

        //public void AddNGOBJ(OneRObj rObj)
        //{
        //    this.OneImage.NGObj.Add(rObj);
        //    this.halconResult.AddNGObj(rObj);
        //}

        public void SetResultOBj(OneResultOBj result)
        {
            OneImage = result;
        }

        public void ShowMessage(string message, int row, int colmnu)
        {
            if (message.Length != 0)
            {
                this.OneImage.AddMeassge(message);
            }
        }

        public OneResultOBj GetOneImageR(OneResultOBj oneResultO = null)
        {
            if (oneResultO != null)
            {
                OneImage = oneResultO;
            }
            if (OneImage == null)
            {
                OneImage = new OneResultOBj();
            }
            if (OneImage.GetHalcon() == null)
            {
                OneImage.GetHalcon(this);
            }
            return OneImage;
        }

        private OneResultOBj OneImage = new OneResultOBj();

        /// <summary>
        /// 显示传递的OBJ
        /// </summary>
        /// <param name="hObject"></param>
        public void ShowObj()
        {
            this.ShowImage();
            this.OneImage.ShouOBJ(this.hWindowHalcon());
        }

        /// <summary>
        /// 显示区域并指定区域颜色
        /// </summary>
        /// <param name="hObject"></param>
        /// <param name="objectColot"></param>
        public void ShowObj(HObject hObject, string objectColot = "red")
        {
            try
            {
                HOperatorSet.SetColor(this.hWindowHalcon(), objectColot);
            }
            catch (Exception)
            {
                HOperatorSet.SetColor(this.hWindowHalcon(), "red");
            }
            //this.ShowObj(hObject);
        }

        #endregion 显示处理

        #region 釆图程序
        /// <summary>
        /// 设置相机光源参数
        /// </summary>
        /// <param name="liyID"></param>
        public void SetCamPraegrm(int liyID = 0,int camid=1)
        {
            try
            {
                if (this.GetCam() == null)
                {
                    return;
                }
               if (this.GetCam() != null)
               {
                
                    if (liyID!=0&&  liyID <= this.ListCamData.Count)
                      {
                           if (this.ListCamData[liyID - 1].Count>=camid)
                           {
                                Vision.SetLight(this.ListCamData[liyID - 1][camid-1].Light_Source);
                                this.GetCam().ExposureTime = this.ListCamData[liyID - 1][camid-1].ExposureTime;
                                this.GetCam().Gain = this.ListCamData[liyID - 1][camid-1].Gain;
                                this.GetCam().Gamma = this.ListCamData[liyID - 1][camid-1].Gamma;
                                return;
                           }
                    }
                    this.GetCam().ExposureTime = CamData.ExposureTime;
                    this.GetCam().Gain = CamData.Gain;
                    this.GetCam().Gamma = CamData.Gamma;
               }
            }
            catch (Exception ex)
            {
                LogErr("写入相机参数错误", ex);
            }
        }

        public void OffSetCamPraegrm()
        {
            try
            {
                Vision.SetLight("off");
            }
            catch (Exception ex)
            {
                LogErr("写入相机参数错误", ex);
            }
        }
      
        /// <summary>
        /// 采图
        /// </summary>
        /// <param name="oneResultOBj"></param>
        /// <param name="frameRawData"></param>
        public void SetImages(OneResultOBj oneResultOBj, IGrabbedRawData frameRawData)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    HObject imaget = this.GetCam().IGrabbedRawDataTOImage(frameRawData);
                    oneResultOBj.Image = imaget;
                    oneResultOBj.CamNewTime = DateTime.Now;
                    this.Image(imaget);
                    this.ShowImage();
                    this.GetHWindow().UPOneImage(oneResultOBj);
                }
                catch (Exception ex)
                {
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        //public int ExposureTime { get; set; } = 20000;
        public Cams.CamData CamData { get; set; } = new Cams.CamData();

        /// <summary>
        /// 注册
        /// </summary>
        private void CycleRun()
        {
            try
            {
                if (Vision.GetSaveImageInfo(this.Name) != null)
                {
                    if (Vision.Instance.RunCams.ContainsKey(Vision.GetSaveImageInfo(this.Name).CamNameStr))
                    {
                        if (this.GetCam() != null)
                        {
                            this.GetCam().Swtr -= this.CamImageEvent;
                            this.GetCam().TriggerCon -= this.TriggerCon;
                        }
                        this.GetCam().Swtr += this.CamImageEvent;
                        this.GetCam().TriggerCon += this.TriggerCon;
                    }
                    if (Vision.Instance.RunDahenCams.ContainsKey(Vision.GetSaveImageInfo(this.Name).CamNameStr))
                    {
                        if (this.GetCam() != null)
                        {
                            this.GetCam().Swtr -= this.CamImageEvent;
                            this.GetCam().TriggerCon -= this.TriggerCon;
                        }
                        this.GetCam().Swtr += this.CamImageEvent;
                        this.GetCam().TriggerCon += this.TriggerCon;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("相机触发事件错误:" + ex.Message);
            }
        }

        /// <summary>
        /// 采集照片成功返回ture
        /// </summary>
        /// <returns></returns>
        public bool ReadCamImage(string liyID = null, int runid = 0)
        {
            bool ok = false;
            try
            {
                if (this.GetCam() != null)
                {
                    OneResultOBj oneResultOBj = new OneResultOBj();
                    if (GetCam().GetImage(out HObject iamge))
                    {
                        ok = true;
                        oneResultOBj.Image = iamge;
                    }
                    else
                    {
                        ok = false;
                    }
            
                    oneResultOBj.CamNewTime = DateTime.Now;
                    Vision.TriggerSetup(Vision.GetSaveImageInfo(this.Name).ReadCamOKName, "true");
                    if (liyID != null)
                    {
                        if (int.TryParse(liyID, out int runidD))
                        {
                            oneResultOBj.LiyID = runidD;
                        }
                        oneResultOBj.RunID = runid;
                        this.GetCam().Key = liyID;
                        this.GetCam().RunID = runid;
                        this.GetCam().OnEnverIamge(liyID, runid, oneResultOBj);
                    }
                    else
                    {
                        this.OneImage = oneResultOBj;
                        OneImage.RunID = runid;
                        this.ShowImage();
                    }
                    return ok;
                }
            }
            catch (Exception ex)
            {
                this.LogErr("採图错误", "採图失败");
                //this.Image().GenEmptyObj();
            }
            return ok;
        }

        private bool aysDone;

        /// <summary>
        /// 异步采图
        /// </summary>
        /// <param name="liyID"></param>
        /// <param name="runid"></param>
        /// <param name="asyncRestImage"></param>
        /// <param name="oneResultOBj"></param>
        public void AsysReadCamImage(int liyID, int runid, AsyncRestImage asyncRestImage, OneResultOBj oneResultOBj = null)
        {
            if (aysDone)
            {
                this.LogErr("异步错误", "等待中");
            }
            int errN = 0;
            while (aysDone)
            {
                errN++;
                if (errN > 1000)
                {
                    break;
                }
                Thread.Sleep(1);
            }
            if (errN > 1000)
            {
                this.LogErr("异步结束" + runid, "等待超时");
            }
            Thread thread = new Thread(() =>
            {
                try
                {
                    aysDone = true;
                    if (this.GetCam() != null)
                    {
                        this.SetCamPraegrm(liyID);
                        if (oneResultOBj == null)
                        {
                            HObject imaget = new HObject();
                            imaget.GenEmptyObj();
                            if (GetCam().GetImage(out HObject iamge))
                            {
                                imaget = iamge;
                            }
                            oneResultOBj = new OneResultOBj();
                            oneResultOBj.CamNewTime = DateTime.Now;
                            if (imaget != null)
                            {
                                oneResultOBj.Image = imaget;
                            }
                            else
                            {
                                if (GetCam().GetImage(out  iamge))
                                {
                                    oneResultOBj.Image = iamge;
                                }
                            }
                        }
                        oneResultOBj.RunID = runid;
                        oneResultOBj.LiyID = liyID;
                        aysDone = false;
                        asyncRestImage.Invoke(oneResultOBj);
                        Vision.TriggerSetup(Vision.GetSaveImageInfo(this.Name).ReadCamOKName, "true");
                        this.GetCam().OnEnverIamge(liyID.ToString(), runid, oneResultOBj);
                    }
                    else if (oneResultOBj != null)
                    {
                        oneResultOBj.RunID = runid;
                        oneResultOBj.LiyID = liyID;
                        aysDone = false;
                        asyncRestImage.Invoke(oneResultOBj);
                        Vision.TriggerSetup(Vision.GetSaveImageInfo(this.Name).ReadCamOKName, "true");
                        this.CamImageEvent(oneResultOBj);
                        //this.GetCam().OnEnverIamge(liyID.ToString(), runid, oneResultOBj);
                    }
                }
                catch (Exception ex)
                {
                    this.LogErr("异步错误", ex.Message);
                    //this.Image().GenEmptyObj();
                }
                aysDone = false;
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public bool RunHProgram(OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj)
        {
            oneRObjs = new List<OneRObj>();
            if (oneResultOBj == null)
            {
                oneResultOBj = new OneResultOBj();
                this.SetResultOBj(oneResultOBj);
            }
            this.UPStart();
            oneResultOBj.RunName = this.Name;
            oneResultOBj.RunID = aoiObj.DebugID;
            this.ShowVision("1", oneResultOBj);
            this.EndChanged(oneResultOBj);
            this.ShowObj();
            return false;
        }

        public bool RunHProgram(OneResultOBj oneResultOBj, RunProgram runProgram)
        {
            if (oneResultOBj == null)
            {
                oneResultOBj = new OneResultOBj();
       
            }
            this.SetResultOBj(oneResultOBj);
            this.UPStart();
            oneResultOBj.RunName = this.Name;
            oneResultOBj.DebugID = 0;
            this.ShowVision(runProgram.Name, oneResultOBj);
            this.EndChanged(oneResultOBj);
            this.ShowObj();
            return false;
        }

        private dynamic Vdynamic;
        private System.Diagnostics.Stopwatch watchOut = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// 触发拍照
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private dynamic HalconRun_ValueEquality(UClass.ErosValues.ErosValueD key)
        {
            if (key.Value.ToString() != "")
            {
                if (Vdynamic == null || Vdynamic != key.Value)
                {
                    watchOut.Start();
                    Vdynamic = key.Value;
                    if (Convert.ToBoolean(key.Value))
                    {
                        if (StaticCon.GetLingkIDValue(Vision.Instance.ReadIDName, "Int16", out dynamic dynamic2))
                        {
                            Vision.Instance.TrayID = dynamic2;
                        }
                        int Did = 0;
                        string[] itme = Vision.GetSaveImageInfo(this.Name).ReadCamName.Split('.');
                        if (this.GetCam() != null)
                        {
                            this.GetCam().Key = "All";
                            StaticCon.GetLingkIDValue(Vision.GetSaveImageInfo(this.Name).ReadRunIDName, "Int16", out dynamic dynamic);
                            if (int.TryParse(dynamic.ToString(), out Did))
                            {
                                if (Did != 0 && RunIDStr.Count >= Did)
                                {
                                    this.GetCam().Key = Did.ToString();
                                }
                                else
                                {
                                    this.GetCam().Key = Did.ToString();
                                }
                            }
                            this.GetCam().RunID = Did;
                            if (ReadCamImage())
                            {
                                if (ProjectINI.DebugMode)
                                {
                                    while (!ProjectINI.SelpMode)
                                    {
                                        Thread.Sleep(200);
                                    }
                                    ProjectINI.SelpMode = false;
                                }
                            }
                        }
                    }
                    if (!Vision.Instance.sTime)
                    {
                        Thread.Sleep(400);
                        Vision.TriggerSetup(Vision.GetSaveImageInfo(this.Name).ReadCamName, 0.ToString());
                    }
                }
                if (watchOut.ElapsedMilliseconds > 10000)
                {
                    watchOut.Reset();
                    Vision.TriggerSetup(Vision.GetSaveImageInfo(this.Name).ReadCamName, 0.ToString());
                }

                if (ProjectINI.DebugMode && ProjectINI.SelpMode)
                {
                    Vdynamic = 0;
                    ProjectINI.SelpMode = false;
                }
            }
            return false;
        }

        private DateTime DateTimeImage;
        private string Teype = "";

        /// <summary>
        /// 相机面
        /// </summary>
        //DataVale dataP;
        private OneCamData OneCamData;

        /// <summary>
        /// 托盘参数
        /// </summary>
        private TrayData trayRobotData;

        /// <summary>
        /// 单个产品参数
        /// </summary>
        private OneDataVale OnePatrData;

    
        /// <summary>
        /// 托盘ID
        /// </summary>
        public int TrayLocation;

        public TrayData GetTrayData()
        {
            return trayRobotData;
        }

        //public TrayResetData GetTrayReset()
        //{
        //    return TrayResetData;
        //}
        public void CamImageEvent(OneResultOBj oneResultOBj)
        {
            CamImageEvent(oneResultOBj.LiyID.ToString(), oneResultOBj, oneResultOBj.RunID);
        }
        /// <summary>
        /// 采图完成
        /// </summary>
        /// <param name="triggec"></param>
        public void TriggerCon(bool triggec)
        {
            AlarmText.AddTextNewLine(this.Name+"采图完成");
        }       
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="liyID">执行名称</param>
        /// <param name="OneImageData">单次拍照集合</param>
        /// <param name="runID"> 程序ID </param>
        /// <param name="isSave">保存图片</param>
        public void CamImageEvent(string liyID, OneResultOBj OneImageData, int runID)
        {
            try
            {
                if (OneImageData == null)
                {
                    OneImageData = new OneResultOBj();
                    OneImageData.Image = OneImage.Image;
                }
                OneImageData.GetHalcon(this);
                if (!Vision.IsObjectValided(OneImageData.Image))
                {
                    this.LogErr("图片空");
                    return;
                }
                ImageHdt(OneImageData.Image);
                int LiyID = 0;
                OneImage = OneImageData;
                OneImageData.Height = this.Height;
                OneImageData.Width = this.Width;

                //OneImageData. AddMeassge("采图"+this.GetCam().RunTime);
                HObject hObject = OneImageData.Image.Clone();
                OneImageData.RunName = this.Name;
                OneImageData.RunID = runID;
                if (int.TryParse(liyID, out LiyID))
                {
                    OneImageData.LiyID = LiyID;
                }
                if (PaleMode)
                {
                    if (runID % PaleID == 1)
                    {
                        //Vision.OneProductVale = new DataVale();
                        //dataP = Vision.OneProductVale;
                    }
                    if (LiyID % this.PaleID == 1 || this.PaleID == 1)
                    {
                        if (this.PaleID != 1)
                        {
                            Vision.GetRunNameVision(this.Name).TiffeOffsetImageEX.TiffeClose();
                        }
                        OneCamData = new OneCamData();
                    }
                }
                else
                {
                    if (runID==1)
                    {
                        OneCamData = new OneCamData();
                    }
                 } 
                this.UPStart();
                if (TrayID >= 0)
                {
                    trayRobotData = DebugCompiler.Instance.DDAxis.GetTrayInxt(TrayID).GetTrayData();
                    TrayLocation = OneImageData.RunID / this.PaleID;
                    if (OneImageData.RunID % this.PaleID > 0)
                    {
                        TrayLocation++;
                    }
                    if (TrayLocation == 0) TrayLocation = 1;
                    if (trayRobotData != null)
                    {
                        OnePatrData = trayRobotData.GetOneDataVale(TrayLocation - 1);
                    }
                }
                else
                {
                    if (OneImageData.RunID <= 1)
                    {
                        OneImageData.RunID = 1;
                        this.ResultOBjs = new OneResultOBj[MaxRunID].ToList();
                        TrayRestData = new DataReseltBase();
                    }
                    OnePatrData = UserFormulaContrsl.GetDataVale();
                }
                if (RestOneP)
                {
                    if (OneImageData.LiyID == 1 || OneImageData.RunID == 1)
                    {
                        OnePatrData = new OneDataVale();
                    }
                }

                //this.ResultOBjs.Add(OneImageData);

                if (this.GetSaveImageInfo().PiNumber <= 1)
                {
                    OnePatrData.TrayLocation = TrayLocation;
                    OnePatrData.NotNull = true;
                }
                //OneCamData.RunVisionName = this.Name;
                if (keyValuePairs1 == null)
                {
                    keyValuePairs1 = new Dictionary<string, double>();
                }
                keyValuePairs1.Clear();

                DateTimeImage = DateTime.Now;

                if (liyID == "")
                {
                }
                else if (liyID == "One")
                {
                    OneImageData.AddMeassge("帧:" +  this.GetCam().CamFOLT());
                    HOperatorSet.DispObj(this.GetOneImageR().Image, this.hWindowHalcon());
                    if (liyID == "One")
                    {
                        this.ShowObj();
                    }
                    return;
                }
                else if (liyID != "All")
                {
                    if (int.TryParse(liyID, out LiyID))
                    {
                        OneImageData.LiyID = LiyID;
                        if (PaleMode)
                        {
                            if (runID % PaleID == 1)
                            {
                                //Vision.OneProductVale = new DataVale();
                                //dataP = Vision.OneProductVale;
                            }
                            if (LiyID == 1)
                            {
                                if (this.PaleID != 1)
                                {
                                    Vision.GetRunNameVision(this.Name).TiffeOffsetImageEX.TiffeClose();
                                }
                            }
                        }
                        if (RunIDStr.Count >= LiyID)
                        {
                            if (LiyID != 0)
                            {
                                this.ShowVision(this.RunIDStr[LiyID - 1], OneImageData);
                            }
                        }
                        else
                        {
                            this.ShowVision(liyID, OneImageData);
                        }
                    }
                    else
                    {
                        this.ShowVision(liyID, OneImageData);
                    }
                }
                this.EndChanged(OneImageData, runID);
                //ImageDistop();
                if (this.PaleID != 1)
                {
                    if (this.TiffeOffsetImageEX.ISHomdeImage == 1)
                    {
                        this.TiffeOffsetImageEX.SetTiffeOff(hObject);
                    }
                    else
                    {
                        this.TiffeOffsetImageEX.SetTiffeOff(OneImageData.Image);
                    }
                }
                if (RunName.Count >= runID)
                {
                    //OneImageData.PoxintID = this.RunName[runID - 1];
                }
                if (OneCamData == null)
                {
                    OneCamData = new OneCamData();
                }
                OneCamData.ResuOBj().Add(OneImageData);
               
                if (OnePatrData != null)
                {
                    if (OnePatrData.PanelID == "")
                    {
                        OnePatrData.PanelID = Project.ProcessControl.ProcessUser.QRCode;
                        Project.ProcessControl.ProcessUser.QRCode = "";
                    }
                    if (this.GetSaveImageInfo().PiNumber <= 1)
                    {
                        OnePatrData.AddCamsData(this.Name, OneImageData.RunID, OneCamData);
                    }
                }
                UPDa(OneImageData, OneImageData.IsSave, hObject, OnePatrData);
                HObject[] hObjects= OnePatrData.GetImages(this.Name);
                if (PaleMode && OneImageData.LiyID % this.PaleID == 0)
                {
                    if (ProjectINI.AdminEnbt)
                    {
                        AlarmText.AddTextNewLine("已完成:" + runID + ":" + LiyID);
                    }
                    UPDOneData(OneImageData, OnePatrData, trayRobotData, OneImageData.IsSave);
                }
            }
            catch (Exception ex)
            {
                this.LogErr(this.Name + "显示程序", ex);
            }
        }

        /// <summary>
        /// 执行图像保存显示等
        /// </summary>
        /// <param name="oneResultOBj"></param>
        /// <param name="isSave"></param>
        /// <param name="hObject"></param>
        public void UPDa(OneResultOBj oneResultOBj, bool isSave, HObject hObject, OneDataVale oneDataVale)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    POnShowObj(this, this.ObjName);
                    ImagesForm.AddImage(oneResultOBj);
                    VisionWindow.UPOneImage(oneResultOBj);
                    if (Vision.Instance.DicSaveType[this.Name].ISSaveData)
                    {
                        if (keyValuePairs1.Count > 0)
                        {
                            string pathsCPK = Vision.Instance.DicSaveType[this.Name].SaveDataPath + "\\CPK\\"+ DateTimeImage.ToString("yyyy年M月d日");
                            if (trayRobotData!=null)
                            {
                                pathsCPK += "\\"+trayRobotData.TrayIDQR ;
                            }
                            pathsCPK +=  ".xls";
                            HTuple hTuple = new HTuple();
                            hTuple.Append(DateTimeImage.ToString());
                            HTuple keys = new HTuple();
                            if (Vision.Instance.DicSaveType[this.Name].IsQRText)
                            {
                                hTuple.Append(oneDataVale.PanelID);
                            }
                            if (!File.Exists(pathsCPK))
                            {
                                keys.Append("时间");
                                if (Vision.Instance.DicSaveType[this.Name].IsQRText)
                                {
                                    keys.Append("SN");
                                }
                                keys.Append("位号");
                                foreach (var item in keyValuePairs1)
                                {
                                    keys.Append(item.Key);
                                }
                                ErosProjcetDLL.Excel.Npoi.AddWriteColumnToExcel(pathsCPK, "数据", keys);
                            }
                            hTuple.Append(TrayLocation);
                            foreach (var item in keyValuePairs1)
                            {
                                hTuple.Append(item.Value);
                            }
                            ErosProjcetDLL.Excel.Npoi.AddRosWriteToExcel(pathsCPK, "数据",   hTuple.ToOArr());
                            //this.SaveDataExce("CPK", DateTimeImage.ToString("yyyy年M月d日"), "数据", hTuple);
                        }
                    }
                    string sn = oneDataVale.PanelID;
                    if (sn == "")
                    {
                        sn = "SN";
                    }
                    oneDataVale.ImagePaht = Vision.GetSaveImageInfo(this.Name).GetSaveImageDic(sn, DateTimeImage);
             
                    if (Vision.Instance.DicSaveType[this.Name].ISSaveWindow)
                    {
                        //if (!oneResultOBj.OK)
                        //{
                           //if (Vision.Instance.DicSaveType[this.Name].ISSaveNGImage)
                        //    {
                        //        string path = Vision.Instance.DicSaveType[this.Name].SavePath + "\\NG\\" + DateTimeImage.ToString("yyyy年M月d日") + "\\" + Product.ProductionName + "\\" +
                        //        sn + "\\" + this.Name + DateTimeImage.ToString("HH时mm分ss秒") + oneResultOBj.LiyID + "-" + oneResultOBj.RunID;
                        //        Vision.SaveWindow(path + "截屏", this.hWindowHalcon());
                        //    }
                        //}
                        Vision.SaveWindow(oneDataVale.ImagePaht + DateTimeImage.ToString("HH时mm分ss秒")+ "截屏",
                                this.hWindowHalcon(), Vision.Instance.DicSaveType[this.Name].SaveWindowImageType);
                    }
                    try
                    {
                        HTuple datas = new HTuple();
                        if (Vision.Instance.DicSaveType[this.Name].IsData)
                        {
                            SaveDataExcel("历史数据", this.WriteDataCName.Keys.ToArray(),
                                DateTimeImage.ToString("HH时mm分ss秒"), this.WriteDataCName.Values.ToArray());
                        }
                        if (Vision.Instance.DicSaveType[this.Name].ISSaveModeR)
                        {
                            if (!File.Exists(Vision.Instance.DicSaveType[this.Name].SaveDataPath + "\\CPK\\" + DateTimeImage.ToString("yyyy年M月d日") + ".xls"))
                            {
                                HTuple dataStrName = new HTuple();
                                dataStrName.Append("时间");
                                dataStrName.Append("数量");
                                dataStrName.Append("Row");
                                dataStrName.Append("Col");
                                dataStrName.Append("Angle");
                                dataStrName.Append("row补偿");
                                dataStrName.Append("Col补偿");
                                dataStrName.Append("Angle补偿");
                                dataStrName.Append("分数");
                                dataStrName.Append("缩放");
                                ErosProjcetDLL.Excel.Npoi.AddWriteColumnToExcel(Vision.Instance.DicSaveType[this.Name].SaveDataPath + "\\模板定位数据\\"
                                    + DateTimeImage.ToString("yyyy年M月d日") + ".xls", "模板数据", dataStrName);
                            }
                            foreach (var item in this.dicModelR)
                            {
                                datas = new HTuple();
                                datas.Append(DateTime.Now.ToString("HH时mm分ss秒"));
                                datas.Append(item.Value.NumberT);
                                datas.Append(item.Value.Row);
                                datas.Append(item.Value.Col);
                                datas.Append(item.Value.Angle.TupleDeg());
                                datas.Append(item.Value.Score);
                                datas.Append(item.Value.Scale);
                                SaveDataExce("模板定位数据", DateTimeImage.ToString("yyyy年M月d日"), "模板数据", datas);
                            }
                        }
                    }
                    catch (Exception) { }
             
                    if (isSave)
                    {
                        string imageName = "";
                        if (this.RunName.Count >= oneResultOBj.RunID&& oneResultOBj.RunID>0)
                        {
                            imageName = this.RunName[oneResultOBj.RunID - 1];
                        }
                        imageName += oneResultOBj.LiyID;
                        OneCamData.ImagePaht = Vision.GetSaveImageInfo(this.Name).GetSaveImagePath(oneResultOBj.RunID, imageName, this.Name, sn, DateTimeImage);

                  
       
                        Vision.Instance.DicSaveType[this.Name].SaveImage(hObject, oneResultOBj.RunID, imageName, this.Name, sn, DateTimeImage);
                    }
                    hObject.Dispose();
                }
                catch (Exception ex)
                {
                    this.LogErr(this.Name + "显示线程", ex);
                }
            });
            thread.Priority = ThreadPriority.Highest;
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 更新图像到托盘产品
        /// </summary>
        /// <param name="dataP"></param>
        /// <param name="isSave"></param>
        public void UPDOneData(OneResultOBj dataP, OneDataVale onePatrData, TrayData trayD, bool isSave)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    OneCamData oneCamD = null;
                    if (onePatrData.ListCamsData.ContainsKey(this.Name))
                    {
                        oneCamD = onePatrData.ListCamsData[this.Name];
                    }
                    else
                    {
                        oneCamD = OneCamData;
                    }
                    if (this.PaleID > 1)
                    {
                        HOperatorSet.HomMat2dIdentity(out HTuple HomMat2D);
                        OneCompOBJs oneRObjs = new OneCompOBJs();
                        oneCamD = onePatrData.ListCamsData[this.Name];
                        for (int i = 0; i < oneCamD.ResuOBj().Count; i++)
                        {
                            HTuple rowsT = 0;
                            HTuple colsT = 0;
                            try
                            {
                                if (this.TiffeOffsetImageEX.IsFill == 1)
                                {
                                    rowsT = this.TiffeOffsetImageEX.Rows[i] - this.TiffeOffsetImageEX.Rows1[i];
                                    colsT = this.TiffeOffsetImageEX.Cols[i] - this.TiffeOffsetImageEX.Cols1[i];
                                }
                                else
                                {
                                    int sd = (i )/ this.TiffeOffsetImageEX.ImageNumberCol;
                                    int dt = (i) % this.TiffeOffsetImageEX.ImageNumberCol;
                                    if (!this.TiffeOffsetImageEX.Vertical)
                                    {
                                        rowsT = sd * this.Height;
                                        colsT = dt * this.Width;
                                    }
                                    else
                                    {
                                        rowsT = dt * this.Height;
                                        colsT = sd * this.Width;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            if (oneCamD.ResuOBj()[i].GetNgOBJS().DicOnes.Count > 0)
                            {
                                HOperatorSet.HomMat2dTranslate(HomMat2D, rowsT, colsT, out HTuple hTuple);
                                foreach (var item in oneCamD.ResuOBj()[i].GetNgOBJS().DicOnes)
                                {
                                    if (item.Value.oneRObjs.Count == 0)
                                    {
                                        oneRObjs.Add(item.Value);
                                    }
                                    foreach (var itemd in item.Value.oneRObjs)
                                    {
                                        try
                                        {
                                            OneRObj oneRObj = new OneRObj(itemd);
                                            HOperatorSet.AffineTransRegion(itemd.NGROI,
                                                  out oneRObj.NGROI, hTuple, "nearest_neighbor");
                                            HOperatorSet.AffineTransRegion(itemd.ROI,
                                          out oneRObj.ROI, hTuple, "nearest_neighbor");
                                            HOperatorSet.AffineTransPixel(hTuple, itemd.rows, itemd.cols, 
                                                out oneRObj.rows, out oneRObj.cols);

                                            oneRObjs.AddCont(oneRObj);
                                        }
                                        catch (Exception ex)
                                        { }
                                    }
                                }
                            }
                            oneCamD.SetOneContOBJ(oneRObjs);
                        }
                    }
                    else
                    {
                        OneCompOBJs oneRObjs = new OneCompOBJs();
                        //OneCamData oneCamD = onePatrData.ListCamsData[this.Name];
                        oneCamD.SetOneContOBJ(oneCamD.ResuOBj()[0].GetNgOBJS());
                    }
                    HObject hObject =null;
                    if (onePatrData != null)
                    {
                        if (this.PaleID != 1)
                        {
                            hObject = Vision.GetRunNameVision(this.Name).TiffeOffsetImageEX.TiffeOffsetImage(onePatrData.GetImages(this.Name));
                            //hObject = Vision.GetRunNameVision(this.Name).TiffeOffsetImageEX.TiffeOffsetImage(this.Name);

                        }
                        if (hObject == null)
                        {
                            hObject = Image().Clone();
                        }
                        if (this.GetSaveImageInfo().PiNumber <= 1)
                        {
                            oneCamD.GetImagePlus(hObject);
                            onePatrData.AddCamsData(this.Name, dataP.RunID, oneCamD);
                            //onePatrData.AddCamsData(this.Name, imageTd);
                            //onePatrData.AddCamsData(this.Name, hObject);
                        }
                    }
                    else
                    {
                        if (this.PaleID != 1)
                        {
                            hObject = Vision.GetRunNameVision(this.Name).TiffeOffsetImageEX.TiffeOffsetImage(this.Name);
                        }
                    }
                  
                    if (DebugCompiler.Instance.IsSet)
                    {
                        UserFormulaContrsl.This.HWind.SetImaage(hObject);
                    }
                    string sn = onePatrData.PanelID;
                    if (sn == "")
                    {
                        sn = "SN";
                    }
                    if (isSave)
                    {
                        if (this.PaleID != 1)
                        {
                            onePatrData.EndTime = DateTime.Now;
                            Vision.GetSaveImageInfo(this.Name).SaveImage(hObject, 0, "拼图", this.Name, sn, onePatrData.EndTime);
                            oneCamD.ImagePaht = Vision.GetSaveImageInfo(this.Name).GetSaveImagePath(0, "拼图", this.Name, sn, onePatrData.EndTime);
                        }
                        else
                        {
                            //oneCamD.ImagePaht = Vision.GetSaveImageInfo(this.Name).GetSaveImagePath(dataP.RunID, "拼图", this.Name, onePatrData.PanelID, DateTime.Now);
                        }
                    }
                    if (trayD != null)
                    {
                        if (trayD.GetITrayRobot() != null)
                        {
                            if (onePatrData != null)
                            {
                                if (RecipeCompiler.Instance.PalenID)
                                {
                                    //if (onePatrData.PanelID == null || onePatrData.PanelID == "")
                                    //{
                                    //    onePatrData.AddNG("SN not identify", false);
                                    //    onePatrData.OK = false;
                                    //}
                                }
                                onePatrData.AutoOK = onePatrData.OK;
                                onePatrData.EndTime = DateTime.Now;
                            }
                            if (this.GetSaveImageInfo().PiNumber > 1)
                            {
                                int detg = trayD.XNumber / this.GetSaveImageInfo().PiNumber + trayD.XNumber % this.GetSaveImageInfo().PiNumber;
                                int det = dataP.RunID / (trayD.XNumber / this.GetSaveImageInfo().PiNumber + trayD.XNumber % this.GetSaveImageInfo().PiNumber);
                                //det = det - 1;
                                if (dataP.RunID == 16)
                                { }
                                detg = dataP.RunID / (trayD.XNumber / this.GetSaveImageInfo().PiNumber + trayD.XNumber % this.GetSaveImageInfo().PiNumber);
                                if (dataP.RunID / (trayD.XNumber / this.GetSaveImageInfo().PiNumber) > 0 &&
                                dataP.RunID % (trayD.XNumber / this.GetSaveImageInfo().PiNumber) == dataP.RunID / (trayD.XNumber / this.GetSaveImageInfo().PiNumber))
                                { det = 1; }
                                else
                                { det = 0; }
                                for (int i = 0; i < this.GetSaveImageInfo().PiNumber - det; i++)
                                {
                                    int idext = (dataP.RunID - 1) * this.GetSaveImageInfo().PiNumber + i - detg + det;
                                    OneDataVale oneData = trayD.GetOneDataVale(idext);
                                    AlarmText.AddTextNewLine("托盘:"+(idext+1)+":"+det);
                                    OneCamData oneCamData = new OneCamData();
                                    OneResultOBj oneResultOBj = new OneResultOBj();
                                    foreach (var item in dataP.GetNgOBJS().DicOnes)
                                    {
                                        HOperatorSet.GetRegionIndex(this.GetSaveImageInfo().PObj[i], item.Value.Row, item.Value.Col, out HTuple intdes);
                                        if (intdes.Length >= 1)
                                        {
                                            oneData.NotNull = true;
                                            if (intdes != -1)
                                            {
                                                if (item.Key == "颜色检测")
                                                { }
                                                oneResultOBj.AddOKOBj(item.Value);
                                                oneCamData.NGObj.Add(item.Value);
                                            }
                                        }
                                    }
                                    HOperatorSet.SmallestRectangle1(this.GetSaveImageInfo().PObj[i], out HTuple row1, out HTuple colum1, out HTuple row2, out HTuple col2);

                                    oneResultOBj.AddMeassge((idext+1));
                                    //oneResultOBj.GetHobjt_s(dataP.GetHobjt_s());
                                    //HOperatorSet.SelectShape(dataP.HObjectYellow, out HObject hObject1 ,new HTuple  ("row", "column") , "and", 
                                    //    new HTuple(row1.D,row2.D), new HTuple(colum1.D, col2.D));
                              
                                    oneResultOBj.HObjectYellow = dataP.HObjectYellow;

                            //HObject hObject2=        Vision.Region_To_XLD(dataP.HObjectGreen);

                                    //HOperatorSet.SelectShape(dataP.HObjectGreen, out hObject1,
                                    //    new HTuple("row", "column"), "and", new HTuple(row1.D, row2.D), new HTuple(colum1.D, col2.D));
                                    oneResultOBj.HObjectGreen = dataP.HObjectGreen;
                                    oneResultOBj.HObjectRed = dataP.HObjectRed;
                                    oneResultOBj.HObjectBlue = dataP.HObjectBlue;
                                    for (int j = 0; j < dataP.MaGreen.Rows.Count; j++)
                                    {
                                        HOperatorSet.GetRegionIndex(this.GetSaveImageInfo().PObj[i], dataP.MaGreen.Rows[j].TupleInt(),
                                            dataP.MaGreen.Columns[j].TupleInt(), out HTuple intdes);
                                        if (intdes>=0)
                                        {
                                            oneResultOBj.MaGreen.Rows.Add( dataP.MaGreen.Rows[j]);
                                            oneResultOBj.MaGreen.Columns.Add(dataP.MaGreen.Columns[j]);
                                            oneResultOBj.MaGreen.Massage.Add(dataP.MaGreen.Massage[j]);
                                        }
                                    }
                                    oneResultOBj.MaGreen.MassageBlute = dataP.MaGreen.MassageBlute;
                                    oneResultOBj.MaGreen.color = dataP.MaGreen.color;
                                    for (int j = 0; j < dataP.MaRed.Rows.Count; j++)
                                    {
                                        HOperatorSet.GetRegionIndex(this.GetSaveImageInfo().PObj[i], dataP.MaRed.Rows[j].TupleInt(),
                                            dataP.MaRed.Columns[j].TupleInt(), out HTuple intdes);
                                        if (intdes >= 0)
                                        {
                                            oneResultOBj.MaRed.Rows.Add(dataP.MaRed.Rows[j]);
                                            oneResultOBj.MaRed.Columns.Add(dataP.MaRed.Columns[j]);
                                            oneResultOBj.MaRed.Massage.Add(dataP.MaRed.Massage[j]);
                                        }
                                    }
                                    oneResultOBj.MaRed.MassageBlute = dataP.MaRed.MassageBlute;

                                    HOperatorSet.ReduceDomain(dataP.Image, this.GetSaveImageInfo().PObj[i], out HObject imageTd);
                                    imageTd = dataP.Image;
                                    oneResultOBj.Image = imageTd;
                                    oneResultOBj.CamNewTime = dataP.CamNewTime;
                                    oneResultOBj.AddObj(this.GetSaveImageInfo().PObj[i]);
                                    oneResultOBj.RunID = dataP.RunID;
                                    oneResultOBj.LiyID = dataP.LiyID;
                                    oneCamData.ResuOBj().Add(oneResultOBj);

                                    oneData.AddCamsData(this.Name, dataP.RunID, oneCamData);
                                    oneData.AddCamsData(this.Name, imageTd);
                                    if (!oneData.NotNull)
                                    {
                                        trayD.SetNumberValue(oneData.TrayLocation, 0);
                                    }
                                    else
                                    {
                                        if (oneData.OK)
                                        {
                                            trayD.SetNumberValue(oneData.TrayLocation, 1);
                                        }
                                        else
                                        {
                                            trayD.SetNumberValue(oneData.TrayLocation, 2);
                                            foreach (var item in oneCamData.NGObj.DicOnes)
                                            {
                                                if (item.Value.RestStrings.Contains("气泡"))
                                                {
                                                    if (!item.Value.aOK)
                                                    {
                                                        trayD.SetNumberValue(oneData.TrayLocation, 3);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                trayD.SetNumberValue(onePatrData.TrayLocation, trayD);
                            }

                            if (Vision.GetSaveImageInfo(this.Name).ISCount)
                            {
                                if (RecipeCompiler.Instance.TrayQRType == RecipeCompiler.TrayEnumType.一个流程一个产品)
                                {
                                    RecipeCompiler.AddOKNumber(onePatrData.OK);
                                    if (onePatrData.AutoOK)
                                    {
                                        if (RecipeCompiler.Instance.GetMes() != null)
                                        {
                                            RecipeCompiler.Instance.GetMes().WrietMes(onePatrData, Product.ProductionName);
                                        }
                                    }
                                    UserFormulaContrsl.GetDataVale(onePatrData);
                                }
                            }
                        }
                        else
                        {
                            this.LogErr("未加载托盘" + TrayID);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.LogErr("显示线程" + ex.StackTrace.Remove(0, ex.StackTrace.Length - 20));
                }
            });
            thread.Priority = ThreadPriority.Highest;
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, double> keyValuePairs1 = new Dictionary<string, double>();

        #endregion 釆图程序

        /// <summary>
        /// 异步保存图片
        /// </summary>
        /// <param name="path"></param>
        public async void ReadCamSaveAsync(string path)
        {
            await Task.Run(() =>
            {
                ReadCamSave(path);
            });
        }

        /// <summary>
        /// 采图并保存
        /// </summary>
        /// <param name="path"></param>
        public void ReadCamSave(string path)
        {
            if (this.ReadCamImage())
            {
                this.ShowImage();
                this.SaveImage(path);
            }
        }

        /// <summary>
        /// 打开指定地址图片成功返回ture
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool ReadImage(string path)
        {
            if (path.Length > 1)
            {
                try
                {
                    if (!File.Exists(path))
                    {
                        MessageBox.Show("文件已不存在");
                        return false;
                    }
                    HOperatorSet.ReadImage(out HObject hObject, path);
                    this.GetOneImageR().Image = (hObject);
                    this.GetOneImageR().CamNewTime = DateTime.Now;
                    ImageHdt(GetOneImageR().Image);
                    HOperatorSet.GetImageSize(this.GetOneImageR().Image, out HTuple width, out HTuple heigth);
                    this.Width = width;
                    this.Height = heigth;
                    if (hWindowHalconID != null)
                    {
                        HOperatorSet.SetPart(this.hWindowHalcon(), 0, 0, Height - 1, Width - 1);
                        HOperatorSet.DispObj(this.GetOneImageR().Image, this.hWindowHalcon());
                    }
                    //ProjectINI.SetTempPrjectDataINI(this.Name, "图片地址", path);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return false;
        }

        public List<bool> timeBool;

        /// <summary>
        /// 添加NG信息
        /// </summary>
        /// <param name="key"></param>
        public void AddNGMessage(string key)
        {
            if (OneImage == null)
            {
                OneImage = new OneResultOBj();
            }
            OneImage.NGMestage += key + ";";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="RestBool"></param>
        public void AddTData(params Double[] RestBool)
        {
            string Data = "";
            for (int i = 0; i < RestBool.Length; i++)
            {
                Data += RestBool[i] + ";";
            }
            TrayRestData.ListVerData.Add(Data.TrimEnd(';'));
        }

        /// <summary>
        /// 单次数据
        /// </summary>

        public DataReseltBase TrayRestData = new DataReseltBase();

        /// <summary>
        /// 读取RXD文件为XLD
        /// </summary>
        /// <param name="path">地址</param>
        /// <param name="hObject">xld</param>
        /// <param name="dxfStatus">Rxd信息</param>
        /// <returns></returns>
        public bool ReadXldRxd(string path, out HObject hObject, out HTuple dxfStatus)
        {
            hObject = new HObject();
            dxfStatus = new HTuple();
            try
            {
                HOperatorSet.ReadContourXldDxf(out hObject, path, "", "", out dxfStatus);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取Rxd文件：《" + path + "》失败；" + ex.Message.ToString());
            }
            return false;
        }

        /// <summary>
        /// 执行指定的程序
        /// </summary>
        /// <param name="runName"></param>
        public void ShowVision(string runName, OneResultOBj oneResultOBj)
        {
            if (this.ListRun.ContainsKey(runName))
            {
                if (this.ListRun[runName].Run(oneResultOBj))
                {
                    this.Result = "OK";
                    SetCode(this.ListRun[runName].CDID, true);
                }
                else
                {
                    SetCode(this.ListRun[runName].CDID, false);
                    this.Result = "NG";
                }
            }
            else
            {
                int intDr = 0;
                try
                {
                    string[] runStr = runName.Split(';');
                    for (int i = 0; i < runStr.Length; i++)
                    {
                        string[] itemStr = runStr[i].Split(',');
                        runs.Clear();
                        for (int i2 = 0; i2 < itemStr.Length; i2++)
                        {
                            if (Single.TryParse(itemStr[i2], out float resultF))
                            {
                                var list = from n in this.ListRun
                                           where n.Value.CDID == resultF
                                           orderby n.Value.CDID ascending
                                           select n;
                                foreach (var item in list)
                                {
                                    Teype = item.Value.Type;
                                    if (!runs.Contains(item.Value.CDID))
                                    {
                                        runs.Add(item.Value.CDID);
                                    }
                                    if (item.Value.CDID > RunMaxID)
                                    {
                                        RunMaxID = item.Value.CDID;
                                    }
                                }
                                int detw = list.Count();
                                var detee = from n in this.ListRun
                                            where n.Value.CDID == runs[0]
                                            orderby n.Value.CDID ascending
                                            select n;
                                foreach (var item in detee)
                                {
                                    Teype = item.Value.Type;
                                    if (!item.Value.Run(oneResultOBj))
                                    {
                                        intDr++;
                                    }
                                }
                            }
                            else
                            {
                                if (ListRun.ContainsKey(itemStr[i2]))
                                {
                                    if (!ListRun[itemStr[i2]].Run(oneResultOBj))
                                    {
                                        oneResultOBj.AddMeassge(itemStr[i2] + ":NG");
                                        intDr++;
                                    }
                                }
                                else
                                {
                                    oneResultOBj.AddMeassge(itemStr[i2] + ":不存在");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                if (intDr == 0) Result = "OK";
                else Result = "NG";
            }
        }

        /// <summary>
        /// 执行过程信息处理
        /// </summary>
        /// <param name="runID"></param>
        private void SetCode(float runID, bool isswa)
        {
            try
            {
                ResultBool = isswa;
                this.SetDefault("MaxNumber", 10);
                this.SetDefault("MinNumber", 20);
                if (this.IsExist("number"))
                {
                    if (runID >= this["number"].D)
                    {
                        string tad = ErosSocket.ErosConLink.StaticCon.GetLingkNameValue("皮带线站.载具码");
                        List<string> list = Project.ProcessControl.ProcessUser.GetPidPrag("载具码", tad, "位移检测");
                        List<string> listDt = Project.ProcessControl.ProcessUser.GetPidPrag("载具码", tad, "位置");
                        var idt = from de in listDt
                                  orderby int.Parse(de.Split('=')[1]) ascending
                                  select de;
                        bool isdt = false;
                        if (list != null)
                        {
                            foreach (var item in idt)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    if (list[i].StartsWith(item.Split('=')[0]))
                                    {
                                        string[] data = list[i].Split('=');
                                        if (data.Length == 2)
                                        {
                                            if (data[1].StartsWith("False"))
                                            {
                                                if (ErosSocket.ErosConLink.StaticCon.GetLingkNameValue("皮带线站.产品位") == i + 1)
                                                {
                                                    StaticCon.SetLinkAddressValue("皮带线站.DB24.308.0", true);
                                                    AlarmText.AddTextNewLine("空基板" + (i + 1).ToString());
                                                    isdt = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (isdt)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                if (runID >= this["MaxNumber"].D)
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
                                if (ErosSocket.ErosConLink.StaticCon.GetLingkNameValue("皮带线站.产品位").ToString() == data[1])
                                {
                                    string dt = Project.ProcessControl.ProcessUser.GetIdPrag(data[0], "OK");
                                    List<string> lsNames = new List<string>();
                                    List<string> lsValues = new List<string>();
                                    lsNames.AddRange(new string[] { "OK", "过程", "打码堆栈" });
                                    if (isswa && dt == true.ToString())
                                    {
                                        StaticCon.SetLingkValue(this["打码堆栈"], data[0], out string err);
                                        StaticCon.SetLingkValue(this["打码堆栈"], data[0], out err);
                                        StaticCon.SetLingkValue(this["打码堆栈"], data[0], out err);
                                        string[] dataV = this["打码堆栈"].S.Split('.');
                                        StaticCon.GetLingkIDValueS(dataV[0] + "." + StaticCon.GetSocketClint(dataV[0]).KeysValues.
                                        DictionaryValueD[dataV[1]].AddressID, UClass.String,
                                        ushort.Parse(StaticCon.GetSocketClint(dataV[0]).KeysValues.DictionaryValueD[dataV[1]].District), out dynamic va);
                                        if (data[0] == va.ToString())
                                        {
                                            err = "入栈";
                                        }
                                        else
                                        {
                                            err = "异常";
                                        }
                                        err += va.ToString();
                                        lsValues.AddRange(new string[] { isswa.ToString(), this.Name, err });
                                    }
                                    else
                                    {
                                        lsValues.AddRange(new string[] { false.ToString(), this.Name, "null" });
                                    }
                                    lsNames.Add("测量最大值");
                                    lsNames.Add("测量最小值");
                                    lsNames.Add("测量平均值");
                                    lsNames.Add("测量NG数");
                                    lsNames.Add("测量断胶数");
                                    lsValues.Add(this["测量最大值"].ToString());
                                    lsValues.Add(this["测量最小值"].ToString());
                                    lsValues.Add(this["测量平均值"].ToString());
                                    lsValues.Add(this["测量NG数"].ToString());
                                    lsValues.Add(this["测量断胶数"].ToString());
                                    Project.ProcessControl.ProcessUser.SetCodeProValue(data[0], lsNames.ToArray(), lsValues.ToArray());
                                    this["测量最大值"] = new HTuple();
                                    this["测量平均值"] = new HTuple();
                                    this["测量断胶数"] = new HTuple();
                                    this["测量最小值"] = new HTuple();
                                    this["测量NG数"] = new HTuple();
                                    this["测量总数"] = new HTuple();
                                    if (this.IsExist("Nt") && this["Nt"] == "True")
                                    {
                                        dt = Project.ProcessControl.ProcessUser.GetIdPrag(data[0], "点胶机");
                                        if (dt == true.ToString() || dt == "")
                                        {
                                            StaticCon.SetLingkValue(this["打码堆栈"], data[0], out string err);
                                            StaticCon.SetLingkValue(this["打码堆栈"], data[0], out err);
                                            string[] dataV = this["打码堆栈"].S.Split('.');
                                            StaticCon.GetLingkIDValueS(dataV[0] + "." + StaticCon.GetSocketClint(dataV[0]).KeysValues.
                                            DictionaryValueD[dataV[1]].AddressID, UClass.String,
                                            ushort.Parse(StaticCon.GetSocketClint(dataV[0]).KeysValues.DictionaryValueD[dataV[1]].District), out dynamic va);
                                            if (data[0] == va.ToString())
                                            {
                                                err = "入栈";
                                            }
                                            else
                                            {
                                                err = "异常";
                                            }
                                            err += va.ToString();
                                            Project.ProcessControl.ProcessUser.SetCodeProValue(data[0],
                                        new string[] { "OK", "过程", "打码堆栈" },
                                        new string[] { true.ToString(), this.Name, err });
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    Dictionary<string, HTuple> keyValuePairs = this.SetData();
                    if (Vision.Instance.DicSaveType[this.Name].IsData)
                    {
                        SaveDataExcel("历史数据", keyValuePairs.Keys.ToArray(), DateTimeImage.ToString("HH时mm分ss秒"), keyValuePairs.Values.ToArray());
                    }
                }
                else if (runID > this["MinNumber"].D)
                {
                    this.SetDefault("测量最大值", new HTuple(), true);
                    this.SetDefault("测量最小值", new HTuple(), true);
                    this.SetDefault("测量平均值", new HTuple(), true);
                    this.SetDefault("测量断胶数", new HTuple(), true);
                    this.SetDefault("测量NG数", new HTuple(), true);
                    this.SetDefault("测量总数", new HTuple(), true);
                    string tad = ErosSocket.ErosConLink.StaticCon.GetLingkNameValue("皮带线站.载具码");
                    if (!isswa)
                    {
                        List<string> list = Project.ProcessControl.ProcessUser.GetCode(tad);
                        if (list != null)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (list[i] != null)
                                {
                                    string[] data = list[i].Split('=');
                                    //Project.ProcessControl.ProcessUser.SetPidPrag("载具码", data[0], false.ToString());
                                    if (data.Length == 2)
                                    {
                                        if (ErosSocket.ErosConLink.StaticCon.GetLingkNameValue("皮带线站.产品位").ToString() == data[1])
                                        {
                                            if (data[0].StartsWith("NoRead"))
                                            {
                                                Project.ProcessControl.ProcessUser.SetCodeProValue(data[0],
                                                new string[] { "OK", "过程" },
                                                   new string[] { false.ToString(), this.Name });
                                            }
                                            else
                                            {
                                                Project.ProcessControl.ProcessUser.SetCodeProValue(data[0],
                                                   new string[] { "OK", "过程" },
                                                      new string[] { false.ToString(), this.Name });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    this["测量最大值"] = new HTuple();
                    this["测量最小值"] = new HTuple();
                    this["测量平均值"] = new HTuple();
                    this["测量断胶数"] = new HTuple();
                    this["测量NG数"] = new HTuple();
                    this["测量总数"] = new HTuple();
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 执行指定ID段的程序
        /// </summary>
        /// <param name="runIDStart"></param>
        /// <param name="runIDEnd"></param>
        public void ShowVision(int runIDStart, int runIDEnd, OneResultOBj oneResultOBj)
        {
            runs.Clear();
            var list = from n in this.ListRun
                       orderby n.Value.CDID ascending
                       select n;
            foreach (var item in list)
            {
                if (item.Value.CDID >= runIDStart || item.Value.CDID <= runIDEnd)
                {
                    if (!runs.Contains(item.Value.CDID))
                    {
                        runs.Add(item.Value.CDID);
                    }
                }
                if (item.Value.CDID > RunMaxID)
                {
                    RunMaxID = item.Value.CDID;
                }
            }
            for (int i = 0; i <= runs.Count - 1; i++)
            {
                var detee = from n in this.ListRun
                            where n.Value.CDID == runs[i]
                            orderby n.Value.CDID ascending
                            select n;

                if (detee.Count() >= 0)
                {
                    buys = true;

                    foreach (var item in detee)
                    {
                        if (!item.Value.Run(oneResultOBj))
                        {
                            this.Result = "NG";
                        }
                        else
                        {
                            this.Result = "OK";
                        }
                    }
                }
                else
                {
                    buys = false;
                    this.LogText("程序:" + runIDStart + ">" + runIDEnd + "不存在！");
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private List<float> runs = new List<float>();

        private object lok = new object();

        /// <summary>
        /// 获取分配的相机
        /// </summary>
        /// <returns></returns>
        public Cams.ICamera GetCam()
        {
            if (Vision.Instance.RunCams.ContainsKey(Vision.GetSaveImageInfo(this.Name).CamNameStr))
            {
                return Vision.Instance.RunCams[Vision.GetSaveImageInfo(this.Name).CamNameStr];
            }
            if (Vision.Instance.RunDahenCams.ContainsKey(Vision.GetSaveImageInfo(this.Name).CamNameStr))
            {
                return Vision.Instance.RunDahenCams[Vision.GetSaveImageInfo(this.Name).CamNameStr];
            }
            return null;
        }

        public void Stop()
        {
            strating = false;
        }

        /// <summary>
        /// 保存结果到Excel
        /// </summary>
        /// <param name="diyName">文件夹名称</param>
        /// <param name="dataMax">最大值</param>
        /// <param name="dint">数量</param>
        /// <param name="data" >数据组</param>
        public void SaveDataExcel(string diyName, double dataMax, int dint, HTuple data)
        {
            try
            {
                HTuple datas = new HTuple();

                //timeStr = DateTime.Now.ToString("HH时mm分ss秒");
                //timeLong =  DateTime.Now.ToString("yyyy年M月d日");
                if (!File.Exists(Vision.GetFilePath() + this.Name + "\\" + diyName + "\\" + DateTimeImage.ToString("HH时mm分ss秒") + ".xls"))
                {
                    string[] dssa = new string[55];
                    dssa[0] = "名称";
                    dssa[1] = "结果";
                    dssa[2] = "最大值/ID";
                    dssa[3] = "最小值";
                    dssa[4] = "边界数量";
                    for (int i = 0; i < 18; i++)
                    {
                        dssa[5 + i] = "1." + (i + 1);
                    }
                    for (int i = 0; i < 32; i++)
                    {
                        dssa[23 + i] = "2." + (i + 1);
                    }
                    Vision2.ErosProjcetDLL.Excel.Npoi.AddWriteColumnToExcel(Vision.GetFilePath() + this.Name + "\\" + diyName + "\\" 
                        + DateTimeImage.ToString("yyyy年M月d日") + ".xls", DateTimeImage.ToString("yyyy年M月d日"),
                      dssa);
                }
                datas.Append(DateTimeImage.ToString("HH时mm分ss秒"));
                datas.Append(this.Result);
                datas.Append(dataMax);
                if (data.Length == 0)
                {
                    datas.Append(0);
                }
                else
                {
                    datas.Append(data.TupleMin());
                    datas.Append(dint);
                    datas.Append(data.TupleString("0.3f"));
                }
                Vision2.ErosProjcetDLL.Excel.Npoi.AddRosWriteToExcel(Vision.GetFilePath() + this.Name + "\\" + diyName + "\\" + DateTimeImage.ToString("yyyy年M月d日") + ".xls", DateTimeImage.ToString("yyyy年M月d日"),
                                datas.ToOArr());
            }
            catch (Exception ex)
            {
                LogErr("保存历史Save:", ex);
            }
        }

        public void SaveDataExcel(string diyName, string[] ColumnNaem, string name, HTuple[] data)
        {
            try
            {
                if (data.Length == 0)
                {
                    return;
                }
                List<object> listd = new List<object>();
                listd.Add(name);
                List<string> columnNam = new List<string>();
                columnNam.Add("时间");
                if (Vision.Instance.DicSaveType[this.Name].IsQRText)
                {
                    columnNam.Add("码");
                    listd.Add(Project.ProcessControl.ProcessUser.QRCode);
                }

                if (!Vision.Instance.DicSaveType[this.Name].IsConuName)
                {
                    columnNam.AddRange(ColumnNaem);
                }

                for (int i = 0; i < data.Length; i++)
                {
                    string dataC = "";
                    if (Vision.Instance.DicSaveType[this.Name].IsConuName)
                    {
                        dataC = ColumnNaem[i] + "[";
                    }
                    for (int it = 0; it < data[i].Length; it++)
                    {
                        if (data[i].TupleType() == 2)
                        {
                            dataC += data[i].TupleSelect(it).TupleString("0.3f");
                        }
                        else if (data[i].TupleType() == 15)
                        {
                            dataC += data[i].TupleSelect(it);
                        }
                        else
                        {
                            dataC += data[i].TupleSelect(it);
                        }
                        if (it != data[i].Length - 1)
                        {
                            dataC += ",";
                        }
                    }
                    if (Vision.Instance.DicSaveType[this.Name].IsConuName)
                    {
                        dataC = dataC + "]";
                        listd.Add(dataC);
                    }
                    else
                    {
                        listd.Add(double.Parse(dataC));
                    }
                }

                ErosProjcetDLL.Excel.Npoi.AddWriteColumnToExcel(Vision.GetFilePath() + this.Name + "\\" + diyName + "\\"
                + DateTimeImage.ToString("yyyy年M月d日") + ".xls", "历史数据", columnNam.ToArray());

                ErosProjcetDLL.Excel.Npoi.AddRosWriteToExcel(Vision.GetFilePath() + this.Name + "\\" + diyName + "\\"
                    + DateTimeImage.ToString("yyyy年M月d日") + ".xls", "历史数据", listd.ToArray());
            }
            catch (Exception ex)
            {
                LogErr("保存历史Save:", ex);
            }
        }

        /// <summary>
        /// 写入数据到Excel
        /// </summary>
        /// <param name="diyName">文件夹名称</param>
        /// <param name="fileName">文件名</param>
        /// <param name="tabName">Tab名</param>
        /// <param name="data">添加在结尾行的数据</param>
        public void SaveDataExce(string diyName, string fileName, string tabName, HTuple data)
        {
            try
            {
                //double[] OBJ = data.ToDArr();
                ErosProjcetDLL.Excel.Npoi.AddRosWriteToExcel(Vision.Instance.DicSaveType[this.Name].SaveDataPath + "\\" + diyName + "\\" + fileName + ".xls", tabName,
                                data.ToOArr());
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="maxd"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool SaveImage(string path, string finame = null)
        {
            try
            {
                if (finame != null)
                {
                    path += "\\" + DateTimeImage.ToString("HH时mm分ss秒") + finame;
                }
                if (Vision.IsObjectValided(this.Image()))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));

                    HOperatorSet.WriteImage(this.Image(), "bmp", 0, path);
                    return true;
                }
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 保存自己的实例
        /// </summary>
        public override void SaveThis(string path)
        {
            Dictionary<string, RunProgram> itemS = new Dictionary<string, RunProgram>();
            HObject hImage = null;
            if (this.GetOneImageR().Image is HImage)
            {
                hImage = this.Image().Clone();
                this.Image().GenEmptyObj();
            }
            this.ClerItem();
            foreach (var item in this.ListRun)
            {
                try
                {
                    if (!ListRunName.ContainsKey(item.Value.Name))
                    {
                        ListRunName.Add(item.Value.Name, item.Value.Type);
                    }
                    else
                    {
                        ListRunName[item.Value.Name] = item.Value.Type;
                    }
                    itemS.Add(item.Value.Name, item.Value);
                    item.Value.SaveThis(path + FileName + "\\" + this.Name);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(item.Key + ":保存失败！");
                }
            }
            this.ListRun = itemS;
            HalconRun.ClassToJsonSavePath(this, path + "\\" + FileName + "\\" + this.Name + ".eros");
            if (hImage != null) this.Image(hImage);
        }

        /// <summary>
        /// 读取参数到实例
        /// </summary>
        /// <param name="path">地址</param>
        public static void RardThis(string path, out HalconRun halcon)
        {
            halcon = null;
            try
            {
                HalconRun.ReadPathJsonToCalss<HalconRun>(path, out halcon);
                if (halcon == null)
                {
                    return;
                }
                if (path.Contains("."))
                {
                    path = Path.GetDirectoryName(path);
                }
                if (halcon.ListRunName == null || halcon.ListRunName.Count == 0)
                {
                    if (Directory.Exists(path + "\\" + halcon.Name))
                    {
                        string[] itmeName = Directory.GetDirectories(path + "\\" + halcon.Name);
                        for (int i = 0; i < itmeName.Length; i++)
                        {
                            string[] itmesStr = Directory.GetFiles(itmeName[i], "*.dicHtuole");
                            if (itmesStr.Length != 0)
                            {
                                itmeName[i] = itmesStr[0];
                            }
                        }
                        for (int i = 0; i < itmeName.Length; i++)
                        {
                            if (File.Exists(itmeName[i]))
                            {
                                string strdata = File.ReadAllText(itmeName[i]);
                                Newtonsoft.Json.Linq.JObject jo = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(strdata);    //字段对象
                                halcon.ListRunName.Add(jo["Name"].ToString(), jo["Type"].ToString());
                            }
                        }
                    }
                }
                path = path + "\\" + halcon.Name + "\\";
                var det = halcon.ListRun.ToArray();
                Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
                foreach (var item in halcon.ListRunName)
                {
                    try
                    {
                        if (item.Value != null)
                        {
                            string ntype = item.Value.Split('.')[item.Value.Split('.').Length - 1];
                            dynamic obj = assembly.CreateInstance(halcon.GetType().Namespace + "." + ntype); // 创建类的实例
                            if (obj != null)
                            {
                                obj.SetPThis(halcon);
                                halcon.ListRun[item.Key] = obj.UpSatrt<RunProgram>(path + item.Key + "\\" + item.Key);
                            }
                            else
                            {
                                obj = assembly.CreateInstance(item.Value); // 创建类的实例
                                halcon.ListRun[item.Key] = obj.UpSatrt<RunProgram>(path + item.Key + "\\" + item.Key);
                            }
                            if (halcon.ListRun[item.Key] == null)
                            {
                                halcon.ListRun[item.Key] = obj;
                            }
                            else
                            {
                                obj.Dispose();
                            }
                            halcon.ListRun[item.Key].SetPThis(halcon);
                            halcon.ListRun[item.Key].Name = item.Key;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(item.Key + "读取错误:" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取地址：" + path + "错误！", ex.Message);
            }
        }

        /// <summary>
        /// 执行OK触发输出
        /// </summary>
        private void ResultOK(OneResultOBj oneResultOBj)
        {
            try
            {
                try
                {
                    EventOK?.Invoke(oneResultOBj);
                }
                catch (Exception) { }
                StaticCon.SetLinkAddressValue(Vision.GetSaveImageInfo(this.Name).OKName, true);
                Vision.TriggerSetup(Vision.GetSaveImageInfo(this.Name).OKName, true.ToString());
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 执行NG方法
        /// </summary>
        private void ResultNG(OneResultOBj oneResultOBj)
        {
            try
            {
                try
                {
                    EventNG?.Invoke(oneResultOBj);
                }
                catch (Exception) { }
                Vision.TriggerSetup(Vision.GetSaveImageInfo(this.Name).NGName, true.ToString());
                StaticCon.SetLinkAddressValue(Vision.GetSaveImageInfo(this.Name).NGName, true);
                //Vision.TriggerSetup(this.NGName, true.ToString());
            }
            catch (Exception) { }
        }

        public override void Dispose()
        {
            try
            {
                AysEndble = false;
                if (this.GetCam() != null)
                {
                    this.GetCam().Swtr -= CamImageEvent;
                }
                if (dicModelR != null)
                {
                    dicModelR.Clear();
                    dicModelR = null;
                }
                if (ListRun != null)
                {
                    foreach (var item in this.ListRun)
                    {
                        item.Value.Dispose();
                    }
                    ListRun.Clear();
                    ListRun = null;
                }
                base.Dispose();
            }
            catch (Exception) { }
        }

        #region 绘制方法

        /// <summary>
        /// 绘制XLD
        /// </summary>
        /// <returns>返回XLD</returns>
        public HObject Draw_XLD()
        {
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            HOperatorSet.DrawXld(out hObject, this.hWindowHalcon(), "true", "true", "true", "true");

            return hObject;
        }

        /// <summary>
        /// 绘制区域
        /// </summary>
        /// <returns></returns>
        public HObject Draw_Region()
        {
            this.Drawing = true;
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            HOperatorSet.DrawRegion(out hObject, this.hWindowHalcon());
            this.Drawing = false;
            return hObject;
        }

        public enum EnumDrawType
        {
            Region = 0,
            Circle = 1,
            Ellipes = 2,
            Rectangle1 = 3,
            Rectangle2 = 4,
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="drawType"></param>
        /// <returns></returns>
        public HObject Draw_Type(EnumDrawType drawType)
        {
            if (Drawing)
            {
                this.GetOneImageR().AddMeassge("绘制中,请绘制结束");
                return new HObject();
            }
            this.Drawing = true;
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            switch (drawType)
            {
                case EnumDrawType.Region:
                    HOperatorSet.DrawRegion(out hObject, this.hWindowHalcon());
                    break;

                case EnumDrawType.Circle:
                    HOperatorSet.DrawCircle(this.hWindowHalcon(), out HTuple row, out HTuple column, out HTuple radius);
                    HOperatorSet.GenCircle(out hObject, row, column, radius);
                    break;

                case EnumDrawType.Ellipes:
                    HOperatorSet.DrawEllipse(this.hWindowHalcon(), out row, out column, out HTuple phi, out radius, out HTuple radius2);
                    HOperatorSet.GenEllipse(out hObject, row, column, phi, radius, radius2);
                    break;

                case EnumDrawType.Rectangle1:
                    HOperatorSet.DrawRectangle1(this.hWindowHalcon(), out row, out column, out HTuple row4, out HTuple column4);
                    HOperatorSet.GenRectangle1(out hObject, row, column, row4, column4);

                    break;

                case EnumDrawType.Rectangle2:
                    HOperatorSet.DrawRectangle2(this.hWindowHalcon(), out HTuple row1, out HTuple column1, out phi, out HTuple length1, out HTuple length2);
                    HOperatorSet.GenRectangle2(out hObject, row1, column1, phi, length1, length2);
                    break;

                default:
                    break;
            }
            this.Drawing = false;
            return hObject;
        }




        /// <summary>
        /// 读取DXF文件
        /// </summary>
        /// <returns></returns>
        public HObject GetFileNameDXF()
        {
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "请选择dxf文件";
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "dxf文件|*.dxf;";
            //openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName.Length == 0) return hObject;
            try
            {
                HOperatorSet.ReadContourXldDxf(out HObject contours, openFileDialog.FileNames[0], new HTuple(), new HTuple(), out HTuple dxfStratus);
                HOperatorSet.HomMat2dIdentity(out HTuple HomMat2DIdentity);
                HOperatorSet.HomMat2dScale(HomMat2DIdentity, 40, 40, 0, 0, out HTuple HomMat2DScale);
                HOperatorSet.AffineTransContourXld(contours, out contours, HomMat2DScale);
                this.AddObj(contours);

                this.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return hObject;
        }

        #endregion 绘制方法

        #region Static

        /// <summary>
        /// 类转换为Json对象保存
        /// </summary>
        /// <param name="obje"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ClassToJsonSavePath(object obje, string path)
        {
            try
            {
                if (!Path.GetExtension(path).Contains("."))
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
                MessageBox.Show("保存失败" + path + "=" + ex.Message);
            }
            return false;
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
                    return true;
                }
                else
                {
                    MessageBox.Show("读取失败！文件不存在" + path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取文件:" + path + " 失败;" + ex.Message);
                obje = default(T);
            }
            return false;
        }

        /// <summary>
        /// 绘制箭头
        /// </summary>
        /// <param name="hv_Row1"></param>
        /// <param name="hv_Column1"></param>
        /// <param name="hv_Row2"></param>
        /// <param name="hv_Column2"></param>
        /// <param name="hv_HeadLength"></param>
        /// <param name="hv_HeadWidth"></param>
        /// <returns></returns>
        public static HObject GenArrowContourXld(HTuple hv_Row1, HTuple hv_Column1,
        HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {
            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];

            // Local iconic variables

            HObject ho_TempArrow = null;

            // Local control variables

            HTuple hv_Length = null, hv_ZeroLengthIndices = null;
            HTuple hv_DR = null, hv_DC = null, hv_HalfHeadWidth = null;
            HTuple hv_RowP1 = null, hv_ColP1 = null, hv_RowP2 = null;
            HTuple hv_ColP2 = null, hv_Index = null;
            // Initialize local and output iconic variables
            HObject ho_Arrow = new HObject();
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);

            ho_Arrow.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            //
            //Calculate the arrow length
            HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
            //
            //Mark arrows with identical start and end point
            //(set Length to -1 to avoid division-by-zero exception)
            hv_ZeroLengthIndices = hv_Length.TupleFind(0);
            if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
            {
                if (hv_Length == null)
                    hv_Length = new HTuple();
                hv_Length[hv_ZeroLengthIndices] = -1;
            }
            //
            //Calculate auxiliary variables.
            hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
            hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
            hv_HalfHeadWidth = hv_HeadWidth / 2.0;
            //
            //Calculate end points of the arrow head.
            hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
            hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
            hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
            hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
            //
            //Finally create output XLD contour for each input point pair
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                {
                    //Create_ single points for arrows with identical start and end point
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(hv_Index),
                        hv_Column1.TupleSelect(hv_Index));
                }
                else
                {
                    //Create arrow contour
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                        hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                        ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                        hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_Arrow, ho_TempArrow, out ExpTmpOutVar_0);
                    ho_Arrow.Dispose();
                    ho_Arrow = ExpTmpOutVar_0;
                }
            }
            ho_TempArrow.Dispose();
            return ho_Arrow;
        }

        public void ShowHelp()
        {
            Help.ShowHelp(null, Application.StartupPath + @"\help.chm", HelpNavigator.Topic, "2.0_程序库.htm#调用程序库");
        }

        #endregion Static

        #region 附属类

        /// <summary>
        /// 区域颜色
        /// </summary>
        public class ObjectColor
        {
            public ObjectColor()
            {
                HobjectColot = "red";
                _HObject = new HObject();
                HOperatorSet.GenEmptyObj(out _HObject);
            }

            public ObjectColor(HObject hObject, string color)
            {
                HobjectColot = color;
                _HObject = hObject;
            }

            /// <summary>
            /// 结果区域
            /// </summary>
            public HObject _HObject;

            public bool IsShow { get; set; } = true;

            /// <summary>
            /// 区域颜色
            /// </summary>
            public HTuple HobjectColot
            {
                get
                {
                    if (Colot == null)
                    {
                        Colot = "red";
                    }
                    if (Colot == "null")
                    {
                        Colot = "red";
                    }

                    return Colot;
                }

                set { Colot = value; }
            }

            private HTuple Colot;

            public void Dispose()
            {
                HobjectColot = null;
                if (_HObject != null)
                {
                    _HObject.Dispose();
                }
            }
        }

        public DXFInFo DXFInFoc = new DXFInFo();

        public class DicHObject
        {
            public string Name { get; set; }

            public Dictionary<string, HObject> DirectoryHObject = new Dictionary<string, HObject>();

            public HObject this[string index]
            {
                get
                {
                    if (DirectoryHObject.ContainsKey(index))
                    {
                        return DirectoryHObject[index];
                    }

                    MessageBox.Show(index + "不存在");
                    return null;
                }
                set
                {
                    if (DirectoryHObject.ContainsKey(index))
                    {
                        //if (DirectoryHObject[index].IsInitialized())
                        //{
                        //    if (value.IsInitialized())
                        //    {
                        //        DirectoryHObject[index] = DirectoryHObject[index].ConcatObj(value.Clone());
                        //        return;
                        //    }
                        //}
                        DirectoryHObject[index] = value;
                    }
                    else
                    {
                        DirectoryHObject.Add(index, value);
                    }
                }
            }

            public void ShowObj(int hWindowHalconID)
            {
                foreach (var item in DirectoryHObject)
                {
                    try
                    {
                        HOperatorSet.DispObj(item.Value, hWindowHalconID);
                    }
                    catch { }
                }
            }

            public void ShowObj(string naem, int hWindowHalconID)
            {
                try
                {
                    if (DirectoryHObject.ContainsKey(naem))
                    {
                        HOperatorSet.DispObj(DirectoryHObject[naem], hWindowHalconID);
                    }
                    else
                    {
                        Vision.Disp_message(hWindowHalconID, naem + "不存在!", 20, 2120, true);
                    }
                }
                catch { }
            }

            public void Clear()
            {
                DirectoryHObject.Clear();
            }

            /// <summary>
            ///
            /// </summary>
            public void RemoeNull()
            {
                foreach (var item in DirectoryHObject)
                {
                    if (item.Value == null)
                    {
                        //item.Value = new HObject();
                        item.Value.GenEmptyObj();
                    }
                    if (!item.Value.IsInitialized())
                    {
                        item.Value.GenEmptyObj();
                    }
                }
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="tabControl"></param>
            /// <param name="data"></param>
            public void UpProperty(PropertyForm control, object data)
            {
                TabPage tabPage = new TabPage();
                tabPage.Text = "参数";
                control.tabControl1.TabPages.Add(tabPage);
                DataGridView dataGridView = new DataGridView();
                dataGridView.Dock = DockStyle.Fill;
                tabPage.Controls.Add(dataGridView);
                dataGridView.Columns.Add("名称", "名称");
                dataGridView.Columns.Add("值", "值");
                //tabPage.Enter += TabPage_Click2;
                if (dataGridView != null)
                {
                    dataGridView.Rows.Clear();
                    ContextMenuStrip strip = new ContextMenuStrip();
                    dataGridView.ContextMenuStrip = strip;
                    ToolStripItem toolStripItems = strip.Items.Add("删除");
                    toolStripItems.Click += ToolStripItems_Click;
                    int i = 0;
                    foreach (var item in this.DirectoryHObject)
                    {
                        i = dataGridView.Rows.Add();
                        dataGridView.Rows[i].Cells[0].Value = item.Key;
                    }
                    void ToolStripItems_Click(object sender, EventArgs e)
                    {
                        try
                        {
                            int d = dataGridView.SelectedCells.Count;
                            for (int it = 0; it < d; it++)
                            {
                                if (dataGridView.Rows[dataGridView.SelectedCells[0].RowIndex].Cells[0] != null)
                                {
                                    this.Remove(dataGridView.Rows[dataGridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
                                    dataGridView.Rows.RemoveAt(dataGridView.SelectedCells[0].RowIndex);
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            /// <summary>
            /// 移除指定名区域
            /// </summary>
            /// <param name="name"></param>
            public void Remove(string name)
            {
                if (DirectoryHObject.ContainsKey(name))
                {
                    if (DirectoryHObject[name] != null)
                    {
                        DirectoryHObject[name].Dispose();
                    }
                    DirectoryHObject.Remove(name);
                }
            }

            public void MouseButtonsLeft(ContextMenuStrip contextMenuStrip, TreeView treeView)
            {
            }
        }

        /// <summary>
        /// 键值的Object
        /// </summary>
        public class DicHObjectColot
        {
            public Dictionary<string, ObjectColor> DirectoryHObject = new Dictionary<string, ObjectColor>();
            public string Name { get; set; }

            public HObject this[string index]
            {
                get
                {
                    if (DirectoryHObject.ContainsKey(index))
                    {
                        return DirectoryHObject[index]._HObject;
                    }
                    else
                    {
                        return new HObject();
                    }
                }
                set
                {
                    if (DirectoryHObject.ContainsKey(index))
                    {
                        DirectoryHObject[index]._HObject = value;
                    }
                    else
                    {
                        DirectoryHObject.Add(index, new ObjectColor() { _HObject = value });
                    }
                }
            }

            public HObject ShowObj(int hWindowHalconID)
            {
                HObject hObjects = new HObject();
                hObjects.GenEmptyObj();
                foreach (var item in DirectoryHObject)
                {
                    try
                    {
                        HOperatorSet.SetColor(hWindowHalconID, item.Value.HobjectColot);
                    }
                    catch (Exception)
                    {
                        HOperatorSet.SetColor(hWindowHalconID, "red");
                    }
                    try
                    {
                        HOperatorSet.DispObj(item.Value._HObject, hWindowHalconID);
                    }
                    catch { }
                    hObjects = hObjects.ConcatObj(item.Value._HObject);
                }
                return hObjects;
            }

            public HObject ShowObj(HWindow hWindowHalconID)
            {
                HObject hObjects = new HObject();
                hObjects.GenEmptyObj();
                foreach (var item in DirectoryHObject)
                {
                    try
                    {
                        HOperatorSet.SetColor(hWindowHalconID, item.Value.HobjectColot);
                    }
                    catch (Exception)
                    {
                        HOperatorSet.SetColor(hWindowHalconID, "red");
                    }
                    try
                    {
                        HOperatorSet.DispObj(item.Value._HObject, hWindowHalconID);
                    }
                    catch { }
                    hObjects = hObjects.ConcatObj(item.Value._HObject);
                }
                return hObjects;
            }

            public HObject ShowObj(string naem, int hWindowHalconID)
            {
                try
                {
                    if (DirectoryHObject.ContainsKey(naem))
                    {
                        HOperatorSet.SetColor(hWindowHalconID, DirectoryHObject[naem].HobjectColot);
                        HOperatorSet.DispObj(DirectoryHObject[naem]._HObject, hWindowHalconID);
                        return DirectoryHObject[naem]._HObject;
                    }
                    else
                    {
                        Vision.Disp_message(hWindowHalconID, naem + "不存在!", 20, 2120, true);
                        return new HObject();
                    }
                }
                catch { }
                return new HObject();
            }

            public HObject ShowObj(string naem, HWindow hWindowHalconID)
            {
                try
                {
                    if (DirectoryHObject.ContainsKey(naem))
                    {
                        HOperatorSet.SetColor(hWindowHalconID, DirectoryHObject[naem].HobjectColot);
                        HOperatorSet.DispObj(DirectoryHObject[naem]._HObject, hWindowHalconID);
                        return DirectoryHObject[naem]._HObject;
                    }
                    else
                    {
                        Vision.Disp_message(hWindowHalconID, naem + "不存在!", 20, 2120, true);
                        return new HObject();
                    }
                }
                catch { }
                return new HObject();
            }

            public void Clear()
            {
                DirectoryHObject.Clear();
            }

            public void RemoeNull()
            {
                foreach (var item in DirectoryHObject)
                {
                    if (item.Value._HObject == null)
                    {
                        item.Value._HObject = new HObject();
                        item.Value._HObject.GenEmptyObj();
                    }
                    if (!item.Value._HObject.IsInitialized())
                    {
                        item.Value._HObject.GenEmptyObj();
                    }
                }
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="tabControl"></param>
            /// <param name="data"></param>
            public void UpProperty(PropertyForm Control, object data)
            {
                TabPage tabPage = new TabPage();
                tabPage.Text = "参数";
                Control.tabControl1.TabPages.Add(tabPage);
                DataGridView datagridview = new DataGridView();
                datagridview.Dock = DockStyle.Fill;
                tabPage.Controls.Add(datagridview);
                datagridview.Columns.Add("名称", "名称");
                datagridview.Columns.Add("值", "值");
                //DataGridView datagridview = Control.dataGridView3;
                if (datagridview != null)
                {
                    datagridview.Rows.Clear();
                    ContextMenuStrip strip = new ContextMenuStrip();
                    datagridview.ContextMenuStrip = strip;
                    ToolStripItem toolStripItems = strip.Items.Add("删除");
                    toolStripItems.Click += ToolStripItems_Click;
                    int i = 0;
                    foreach (var item in this.DirectoryHObject)
                    {
                        i = datagridview.Rows.Add();
                        datagridview.Rows[i].Cells[0].Value = item.Key;
                        datagridview.Rows[i].Cells[1].Value = item.Value._HObject.CountObj();
                    }
                    void ToolStripItems_Click(object sender, EventArgs e)
                    {
                        try
                        {
                            int d = datagridview.SelectedCells.Count;
                            for (int it = 0; it < d; it++)
                            {
                                if (datagridview.Rows[datagridview.SelectedCells[0].RowIndex].Cells[0] != null)
                                {
                                    this.Remove(datagridview.Rows[datagridview.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
                                    datagridview.Rows.RemoveAt(datagridview.SelectedCells[0].RowIndex);
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            private void Remove(string name)
            {
                if (DirectoryHObject.ContainsKey(name))
                {
                    DirectoryHObject[name]._HObject.Dispose();
                    DirectoryHObject.Remove(name);
                }
            }

            public void MouseButtonsLeft(ContextMenuStrip contextMenuStrip, TreeView treeView)
            {
            }
        }

        /// <summary>
        /// 测量类
        /// </summary>
        public class Dic_Measure
        {
            public Dictionary<string, Measure> Keys_Measure { get; set; } = new Dictionary<string, Measure>();
            public Dictionary<string, object> KeyObj { get; set; } = new Dictionary<string, object>();

            public Measure this[string idmes]
            {
                get
                {
                    if (Keys_Measure.ContainsKey(idmes))
                    {
                        Keys_Measure[idmes].Name = idmes;
                        if (!KeyObj.ContainsKey(idmes))
                        {
                            KeyObj.Add(idmes, Keys_Measure[idmes]);
                        }
                        else
                        {
                            KeyObj[idmes] = Keys_Measure[idmes];
                        }
                        return Keys_Measure[idmes];
                    }
                    else
                    {
                        return new Measure(Measure.MeasureType.Measure);
                    }
                }
                set
                {
                    if (Keys_Measure.ContainsKey(idmes))
                    {
                        Keys_Measure[idmes].Name = idmes;
                        Keys_Measure[idmes] = value;
                    }
                    else
                    {
                        Keys_Measure.Add(idmes, value);
                        Keys_Measure[idmes].Name = idmes;
                    }
                    if (!KeyObj.ContainsKey(idmes))
                    {
                        KeyObj.Add(idmes, value);
                    }
                    else
                    {
                        KeyObj[idmes] = value;
                    }
                }
            }

            /// <summary>
            /// 添加制定名新的测量程序
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Measure Add(string name)
            {
                if (Keys_Measure.ContainsKey(name))
                {
                    string namest = name;
                streatfor:
                    string dsts = string.Empty;
                    for (int i = name.Length; i > 0; i--)
                    {
                        if (int.TryParse(name[i - 1].ToString(), out int dss))
                        {
                            dsts = dss + dsts;
                        }
                        else
                        {
                            string sd = name.Substring(0, name.Length - dsts.Length);
                            int.TryParse(dsts, out int intsd);
                            intsd++;
                            dsts = sd + intsd.ToString();
                            break;
                        }
                    }
                    if (Keys_Measure.ContainsKey(dsts))
                    {
                        name = dsts;
                        goto streatfor;
                    }
                    string mes = "";
                    if (namest == name)
                    {
                        mes = namest;
                    }
                    else
                    {
                        mes = namest + "--" + name;
                    }
                    DialogResult dr = MessageBox.Show(mes + ":已存在!是否新建测量《" + dsts + "》？", "新建测量程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        string jsonStr = JsonConvert.SerializeObject(Keys_Measure[namest]);
                        Measure measure = new Measure();
                        JsonConvert.PopulateObject(jsonStr, measure);
                        measure.DrawCols = new HTuple();
                        measure.DrawRows = new HTuple();
                        measure.DrawHObject.GenEmptyObj();
                        Keys_Measure.Add(dsts, measure);
                        Keys_Measure[dsts].Name = dsts;
                        name = dsts;
                        return Keys_Measure[dsts];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Keys_Measure.Add(name, new Measure());

                    Keys_Measure[name].Name = name;
                    return Keys_Measure[name];
                }
            }

            /// <summary>
            /// 添加多个新的测量程序
            /// </summary>
            /// <param name="name"></param>
            /// <param name="number"></param>
            public void Add(string name, int number)
            {
                string dsts = string.Empty;
                int intsd = 0;
                if (Keys_Measure.ContainsKey(name))
                {
                    DialogResult dr = MessageBox.Show(name + ":已存在!是否新建测量？", "新建测量程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        for (int it = 0; it < number; it++)
                        {
                        streatfor:

                            for (int i = name.Length; i > 0; i--)
                            {
                                if (int.TryParse(name[i - 1].ToString(), out int dss))
                                {
                                    dsts = dss + dsts;
                                }
                                else
                                {
                                    string sd = name.Substring(0, name.Length - dsts.Length);
                                    int.TryParse(dsts, out intsd);
                                    intsd++;
                                    dsts = sd + intsd.ToString();
                                    break;
                                }
                            }
                            if (Keys_Measure.ContainsKey(dsts))
                            {
                                name = dsts;
                                goto streatfor;
                            }
                            string jsonStr = JsonConvert.SerializeObject(Keys_Measure[name]);
                            Measure measure = new Measure();
                            JsonConvert.PopulateObject(jsonStr, measure);
                            measure.DrawCols = new HTuple();
                            measure.DrawRows = new HTuple();
                            measure.DrawHObject = new HObject();
                            Keys_Measure.Add(dsts, measure);
                            Keys_Measure[dsts].Name = dsts;
                            name = dsts;
                        }
                    }
                }
                else
                {
                    for (int i = name.Length; i > 0; i--)
                    {
                        if (int.TryParse(name[i - 1].ToString(), out int dss))
                        {
                            dsts = dss + dsts;
                        }
                        else
                        {
                            string sd = name.Substring(0, name.Length - dsts.Length);
                            int.TryParse(dsts, out intsd);
                            dsts = sd;
                            break;
                        }
                    }

                    for (int i = 0; i < number; i++)
                    {
                        Keys_Measure.Add(dsts + intsd, new Measure());
                        Keys_Measure[name].Name = name;
                    }
                }
            }

            /// <summary>
            /// 添加名称测量
            /// </summary>
            /// <param name="name"></param>
            /// <param name="measure"></param>
            /// <returns></returns>
            public Measure Add(string name, Measure measure)
            {
                if (Keys_Measure.ContainsKey(name))
                {
                    string namest = name;
                streatfor:
                    string dsts = string.Empty;
                    for (int i = name.Length; i > 0; i--)
                    {
                        if (int.TryParse(name[i - 1].ToString(), out int dss))
                        {
                            dsts = dss + dsts;
                        }
                        else
                        {
                            string sd = name.Substring(0, name.Length - dsts.Length);
                            int.TryParse(dsts, out int intsd);
                            intsd++;
                            dsts = sd + intsd.ToString();
                            break;
                        }
                    }
                    if (Keys_Measure.ContainsKey(dsts))
                    {
                        name = dsts;
                        goto streatfor;
                    }
                    string mes = "";
                    if (namest == name)
                    {
                        mes = namest;
                    }
                    else
                    {
                        mes = namest + "--" + name;
                    }
                    DialogResult dr = MessageBox.Show(mes + ":已存在!是否重命名《" + dsts + "》？", "新建测量程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        Keys_Measure.Add(dsts, measure);
                        Keys_Measure[dsts].Name = dsts;
                        name = dsts;
                        return Keys_Measure[dsts];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Keys_Measure.Add(name, measure);

                    Keys_Measure[name].Name = name;
                    return measure;
                }
            }

            /// <summary>
            /// 同步键与值的唯一姓名，
            /// </summary>
            public void SyncName()
            {
                Measure[] measuress = Keys_Measure.Values.ToArray();
                Keys_Measure.Clear();
                for (int i = 0; i < measuress.Length; i++)
                {
                stra:
                    if (!Keys_Measure.ContainsKey(measuress[i].Name))
                    {
                        Keys_Measure.Add(measuress[i].Name, measuress[i]);
                    }
                    else
                    {
                        measuress[i].Name = measuress[i].Name + "(重复)";
                        goto stra;
                    }
                }
            }

            public static implicit operator Dic_Measure(DicHtuple v)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                if (Keys_Measure != null)
                {
                    foreach (var item in Keys_Measure)
                    {
                        item.Value.Dispose();
                    }
                    Keys_Measure.Clear();
                }
                if (KeyObj != null)
                {
                    KeyObj.Clear();
                }
            }
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        public class LinkCamNameConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                List<string> icas = new List<string>(Vision.Instance.RunCams.Keys);
                icas.AddRange(Vision.Instance.RunDahenCams.Keys);
                return new StandardValuesCollection(icas);
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }
        }

        #endregion 附属类
    }

    public class SaveImageInfo
    {
        public SaveImageInfo()
        {
            SavePath = Application.StartupPath;
        }

        /// <summary>
        /// 机器人链接名
        /// </summary>
        [DescriptionAttribute("机器人的链接名。"), Category("机器手连接"), DisplayName("通信连接名称"),
        TypeConverter(typeof(ErosSocket.ErosConLink.DicSocket.LinkNameConverter))]
        public string LingkRobotName
        {
            get { return lingkRobtName; }
            set
            {
      
                if (StaticCon.SocketClint.ContainsKey(value))
                {
                    if (Vision.GetRunNameVision(this.Name)!=null)
                    {
                        Vision.GetRunNameVision(this.Name).GetSocket(StaticCon.SocketClint[value]);
                    }
                }
                lingkRobtName = value;
            }
        }

        private string lingkRobtName = "";

        public string Name;

        [DescriptionAttribute("程序指定相机名称。"), Category("触发器"), TypeConverter(typeof(LinkCamNameConverter)), DisplayName("调用相机名称")]
        /// <summary>
        /// 相机名
        /// </summary>
        public string CamNameStr
        {
            get { return camNameStr; }
            set
            {
                if (value != camNameStr)
                {
                    camNameStr = value;
                }
            }
        }

        private string camNameStr = "";

        [DescriptionAttribute("触发程序的ID地址号。"), Category("触发器"), DisplayName("程序ID号地址")]
        [Editor(typeof(LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string ReadRunIDName { get; set; } = string.Empty;

        /// <summary>
        /// 拍照变量名称
        /// </summary>
        [DescriptionAttribute("触发拍照的变量名称。"), Category("触发器"), DisplayName("拍照变量名称")]
        [Editor(typeof(LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string ReadCamName { get; set; } = string.Empty;

        [DescriptionAttribute("触发拍照完成的变量名称。"), Category("触发器"), DisplayName("拍照完成名称"),]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string ReadCamOKName { get; set; } = string.Empty;

        [DescriptionAttribute("程序执行完成触发的变量名称。"), Category("触发器"), DisplayName("程序完成名称"),]
        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string RunDoneName { get; set; } = string.Empty;

        [DescriptionAttribute("OK结果输出的变量名称。"), Category("触发器"), DisplayName("结果OK名称"),]
        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string OKName { get; set; } = string.Empty;

        [DescriptionAttribute("NG结果输出的变量名称。"), Category("触发器"), DisplayName("结果NG名称"),]
        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string NGName { get; set; } = string.Empty;

        [DescriptionAttribute("是否保存测试数据。"), Category("CPK"), DisplayName("启动数据保存")]
        public bool ISSaveData { get; set; }

        [DescriptionAttribute("分割产品数量。"), Category("产品"), DisplayName("分割产品数量")]
        public int PiNumber { get; set; }

        public List<HObject> PObj = new List<HObject>();

        [DescriptionAttribute("选择轴组合。"), Category("轴"), DisplayName("轴组")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("AxisGrotList", false, true)]
        public string AxisGrot { get; set; } = "";

        public static string[] AxisGrotList
        {
            get
            {
                List<string> vs = new List<string>();
                vs = DebugCompiler.Instance.DDAxis.AxisGrot.Keys.ToList();
                for (int i = 0; i < ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP.Count; i++)
                {
                    vs.Add(ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP[i].Name);
                }

                return vs.ToArray();
            }
        }
        public string GetSaveImagePah()
        {
         
            string path = SavePath + "\\" +  DateTime.Now.ToString("yyyy年M月d日") + "\\" + Product.ProductionName;
            string sn = "";
            if (ImageNameType == "All")
            {
                path += "\\" + Name;
            }
            else if (ImageNameType == "OK")
            {
                path += "\\" + Name + "\\OK";
            }
            else if (ImageNameType == "NG")
            {
                path += "\\" + Name + "\\NG";
            }
            else if (ImageNameType == "QR")
            {
                path += "\\" +sn + "\\" + Name;
            }
            else if (ImageNameType == "TrayQR")
            {
                if (Vision.GetRunNameVision().TrayID >= 0)
                {
                    path += "\\" + DebugCompiler.GetTray(Vision.GetRunNameVision().TrayID).GetTrayData().TrayIDQR + "\\" + Name;
                }
            }
            else if (ImageNameType == "TrayQR+QR")
            {
                if (Vision.GetRunNameVision().TrayID >= 0)
                {
                    path += "\\" + DebugCompiler.GetTray(Vision.GetRunNameVision().TrayID).GetTrayData().TrayIDQR;
                }
                path += "\\" + sn + "\\" + Name;
            }

            if (SaveImageFileName.Contains("时间"))
            {
                path+= DateTime.Now.ToString("HH时mm分ss秒");
            }
            return path;
        }

        [Editor(typeof(PageTypeEditor_FolderBrowserDialog), typeof(UITypeEditor))]
        [DescriptionAttribute("保存图片文件夹。"), Category("图像保存"), DisplayName("目标文件夹")]
        public string SavePath { get; set; }

        [DescriptionAttribute("创建图像文件夹的方式,No不保存,/n/rAll全部保存一起，NG或OK保存，/n/rNG和OK分开保存以文件夹前缀,/n/rOK.|NG.以文件名前缀,/n/rQR以产品码文件夹"),
            Category("图像保存"), DisplayName("创建文件夹方式")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "No",
        "All", "NG", "OK", "NG|OK", "OK.|NG.", "QR","TrayQR","TrayQR+QR")]
        public string ImageNameType { get; set; }

        [DescriptionAttribute("保存图像的格式。"), Category("图像保存"), DisplayName("图像格式")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("",
                  "jpg", "tiff", "png", "bmp", "hobj", "ima", "tif", "jpeg", "jp2", "jxr")]
        public string SaveImageType { get; set; } = "jpg";

        [DescriptionAttribute("保存图像的名称。"), Category("图像保存"), DisplayName("图像名称")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "时间+ID", "时间+ID+位置名", "时间+位置名", "时间+ID+结果符")]
        public string SaveImageFileName { get; set; } = "时间+ID+位置名";

        //[DescriptionAttribute("当前盘符剩余的容量提醒删除图片。"), Category("图像保存"), DisplayName("余量提醒")]
        //public int ImageZise { get; set; } = 10;

        [DescriptionAttribute("是否保存结果截屏。"), Category("显示屏幕保存"), DisplayName("保存截屏")]
        public bool ISSaveWindow { get; set; }



        [DescriptionAttribute("保存屏幕图像的格式。"), Category("显示屏幕保存"), DisplayName("图像格式")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("",
            "jpg", "tiff", "png", "bmp", "hobj", "ima", "tif", "jpeg", "jp2", "jxr")]
        public string SaveWindowImageType { get; set; } = "jpg";

        [DescriptionAttribute("是否保存模板定位信息。"), Category("保存数据"), DisplayName("保存模板定位信息")]
        public bool ISSaveModeR { get; set; }

        [DescriptionAttribute("是否保存过程数据。"), Category("保存数据"), DisplayName("是否保存数据")]
        public bool IsData { get; set; }

        [DescriptionAttribute("是否二维码到数据。"), Category("保存数据"), DisplayName("是否二维码到数据")]
        public bool IsQRText { get; set; }

        [DescriptionAttribute("将列名写入数据；注意多个拍照流程时，请将列名写入数据。"), Category("保存数据"), DisplayName("写入列名")]
        public bool IsConuName { get; set; }

        /// <summary>
        /// 保存图片地址
        /// </summary>
        [Editor(typeof(PageTypeEditor_FolderBrowserDialog), typeof(UITypeEditor))]
        [DescriptionAttribute("保存数据文件夹地址。"), Category("保存数据"), DisplayName("保存数据地址")]
        public string SaveDataPath
        {
            get
            {
                if (saveDataPath == null)
                {
                    saveDataPath = Vision.GetFilePath();
                };
                return saveDataPath;
            }
            set { saveDataPath = value; }
        }

        private string saveDataPath;

        [DescriptionAttribute("程序是否计数。"), Category("计数"), DisplayName("是否计数")]
        public bool ISCount { get; set; } = false;

        [DescriptionAttribute("是否显示流程图片。"), Category("显示"), DisplayName("流程图片")]
        public bool ISImages { get; set; } = false;

        [DescriptionAttribute("左上显示格式。0显示所有，1只显示NG或OK，-1不显示文本"), Category("显示"), DisplayName("左上结果文本")]
        public byte DispTextType { get; set; }

        /// <summary>
        /// 获取保存图像文件夹
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="timeLog"></param>
        /// <returns></returns>
        public string GetSaveImageDic(string sn, DateTime timeLog)
        {
            string path = SavePath + "\\" + timeLog.ToString("yyyy年M月d日") + "\\" + Product.ProductionName;

            if (ImageNameType == "All")
            {
                path += "\\" + Name;
            }
            else if (ImageNameType == "OK")
            {
                path += "\\" + Name + "\\OK";
            }
            else if (ImageNameType == "NG")
            {
                path += "\\" + Name + "\\NG";
            }
            else if (ImageNameType == "QR")
            {
                path += "\\" + sn + "\\" + Name;
            }
            else if (ImageNameType == "TrayQR")
            {
                if (Vision.GetRunNameVision().TrayID >= 0)
                {
                    path += "\\" + DebugCompiler.GetTray(Vision.GetRunNameVision().TrayID).GetTrayData().TrayIDQR + "\\" + Name;
                }
            }
            else if (ImageNameType == "TrayQR+QR")
            {
                if (Vision.GetRunNameVision().TrayID >= 0)
                {
                    path += "\\" + DebugCompiler.GetTray(Vision.GetRunNameVision().TrayID).GetTrayData().TrayIDQR;
                }
                path += "\\" + sn + "\\" + Name;
            }

            return path;
        }

        //public List<Cams.CamData> ListCamData { get; set; } = new List<Cams.CamData>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="runID"></param>
        /// <param name="imageName"></param>
        /// <param name="Name"></param>
        /// <param name="sn"></param>
        /// <param name="timeLog"></param>
        /// <returns></returns>
        public string GetSaveImagePath(int runID, string imageName, string Name, string sn, DateTime timeLog)
        {
            string path = SavePath + "\\" + timeLog.ToString("yyyy年M月d日") + "\\" + Product.ProductionName;
            if (ImageNameType == "All")
            {
                path += "\\" + Name;
            }
            else if (ImageNameType == "OK")
            {
                path += "\\" + Name + "\\OK";
            }
            else if (ImageNameType == "NG")
            {
                path += "\\" + Name + "\\NG";
            }
            else if (ImageNameType == "QR")
            {
                path += "\\" + sn + "\\" + Name;
            }
            else if (ImageNameType == "TrayQR")
            {
                if (Vision.GetRunNameVision().TrayID >= 0)
                {
                    path += "\\" + DebugCompiler.GetTray(Vision.GetRunNameVision().TrayID).GetTrayData().TrayIDQR + "\\" + Name;
                }
            }
            else if (ImageNameType == "TrayQR+QR")
            {
                if (Vision.GetRunNameVision().TrayID >= 0)
                {
                    path += "\\" + DebugCompiler.GetTray(Vision.GetRunNameVision().TrayID).GetTrayData().TrayIDQR;
                }
                path += "\\" + sn + "\\" + Name;
            }
            string name = "";
            if (SaveImageFileName.Contains("时间"))
            {
                name = timeLog.ToString("HH时mm分ss秒");
            }
            name += imageName + "-";
            if (SaveImageFileName.Contains("ID"))
            {
                name += runID;
            }
            return path+name+"."+ this.SaveImageType;
        }

        /// <summary>
        /// 保存图片到地址
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="runID">执行ID</param>
        /// <param name="Name">相机名称</param>
        /// <param name="Result">结果</param>
        /// <param name="timeLog">时间</param>
        public void SaveImage(HObject image, int runID, string imageName, string Name, string sn, DateTime timeLog)
        {
            string path  = SavePath + "\\" + timeLog.ToString("yyyy年M月d日")+ "\\" + Product.ProductionName; 

            if (ImageNameType == "All")
            {
                path += "\\" + Name;
            }
            else if (ImageNameType == "OK")
            {
                path += "\\" + Name + "\\OK";
            }
            else if (ImageNameType == "NG")
            {
                path += "\\" + Name + "\\NG";
            }
            else if (ImageNameType == "QR")
            {
                path +=  "\\" + sn + "\\" + Name;
            }
            else if (ImageNameType == "TrayQR")
            {
                if (Vision.GetRunNameVision().TrayID>=0)
                {
                    path += "\\" + DebugCompiler.GetTray(Vision.GetRunNameVision().TrayID).GetTrayData().TrayIDQR + "\\" + Name;
                }
            }
            else if (ImageNameType == "TrayQR+QR")
            {
                if (Vision.GetRunNameVision().TrayID >= 0)
                {
                    path += "\\" + DebugCompiler.GetTray(Vision.GetRunNameVision().TrayID).GetTrayData().TrayIDQR ;
                }
                path += "\\" + sn + "\\" + Name;
            }
            else
            {
                return;
            }
            string name = "";
            if (SaveImageFileName.Contains("时间"))
            {
                name = timeLog.ToString("HH时mm分ss秒");
            }
            name += imageName + "-";
            if (SaveImageFileName.Contains("ID"))
            {
                name += runID;
            }
            SaveImage(image, path + name);
        }

        public void SaveImage(HObject image, string finame)
        {
            try
            {
                if (Vision.IsObjectValided(image))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(finame));
                    HOperatorSet.WriteImage(image, this.SaveImageType, 0, finame);
                }
            }
            catch (Exception)
            {
            }
        }

        public void SetDelete(string Path, int zise)
        {
            double dste = (double)OpenFileSiez.GetDirectoryLength(Path) / 1024 / 1024 / 1024;
            double dumax = OpenFileSiez.GetHardDiskFreeSpace(Path[0]);
            if (dumax < zise)
            {
                AlarmText.LogIncident("图片文件容量已满", "图片文件：" + dste.ToString("0.00") + "G,硬盘" + Path[0] + ":" + dumax + ",文件夹:" + Path);
            }
        }
    }

    /// <summary>
    /// 键值的元祖
    /// </summary>
    public class DicHtuple : ProjectC
    {
        #region 接口实现

        public override string FileName => "DicHtuole";
        public override string SuffixName => ".dicHtuole";

        /// <summary>
        /// 显示元祖集合
        /// </summary>
        /// <param name="tabControl"></param>
        /// <param name="data"></param>
        public void UpProperty(PropertyForm control, object data)
        {
            //base.UpProperty(control, data);
            if (ProjectINI.Enbt)
            {
                TabPage tabPage = new TabPage();
                tabPage.Text = "参数";
                control.tabControl1.TabPages.Add(tabPage);
                DynamicParameter objDataGr = new DynamicParameter();
                objDataGr.Dock = DockStyle.Fill;
                tabPage.Controls.Add(objDataGr);
                tabPage.Enter += TabPage_Click2;
                void TabPage_Click2(object sender, EventArgs e)
                {
                    objDataGr.SetUpData(this);
                }

                if (objDataGr.dataGridView1 != null)
                {
                    objDataGr.dataGridView1.Rows.Clear();
                    ContextMenuStrip strip = new ContextMenuStrip();
                    objDataGr.dataGridView1.ContextMenuStrip = strip;
                    ToolStripItem toolStripItemsSave = strip.Items.Add("保存");
                    toolStripItemsSave.Click += ToolStripItemsSave_Click;
                    ToolStripItem toolStripItems = strip.Items.Add("删除");
                    toolStripItems.Click += ToolStripItems_Click;
                    objDataGr.dataGridView1.Columns[1].Visible = true;
                    //删除
                    void ToolStripItems_Click(object sender, EventArgs e)
                    {
                        try
                        {
                            int d = objDataGr.dataGridView1.SelectedCells.Count;
                            for (int it = 0; it < d; it++)
                            {
                                if (objDataGr.dataGridView1.Rows[objDataGr.dataGridView1.SelectedCells[0].RowIndex].Cells[0] != null)
                                {
                                    this.Remove(objDataGr.dataGridView1.Rows[objDataGr.dataGridView1.SelectedCells[0].RowIndex].Cells[0].
                                        Value.ToString());
                                    objDataGr.dataGridView1.Rows.RemoveAt(objDataGr.dataGridView1.SelectedCells[0].RowIndex);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("删除错误：" + ex.Message);
                        }
                    }
                    //保存
                    void ToolStripItemsSave_Click(object sender, EventArgs e)
                    {
                        SaveData(objDataGr.dataGridView1);
                    }
                }
            }
        }

        /// <summary>
        /// 保存参数接口
        /// </summary>
        /// <param name="control"></param>
        public void SaveData(Control control)
        {
            DataGridView dataGridView = control as DataGridView;
            try
            {
                int i = 0;
                for (i = 0; i < dataGridView.Rows.Count; i++)
                {
                    if (dataGridView.Rows[i].Cells[0].Value != null && dataGridView.Rows[i].Cells[3].Value != null && dataGridView.Rows[i].Cells[0].Value.ToString() != "")
                    {
                        HTuple hTuple = new HTuple();
                        string key = dataGridView.Rows[i].Cells[0].Value.ToString();
                        string dss = dataGridView.Rows[i].Cells[3].Value.ToString().Trim('"').Trim(' ');
                        if (dss.Contains("["))
                        {
                            string[] datas = dss.Trim('[', ']').Split(',');
                            for (int i2 = 0; i2 < datas.Length; i2++)
                            {
                                double.TryParse(datas[i2], out double doubes);
                                hTuple.Append(doubes);
                            }
                        }
                        else
                        {
                            if (double.TryParse(dss, out double deo))
                            {
                                hTuple = deo;
                            }
                            else
                            {
                                hTuple = dss;
                            }
                        }

                        if (!this.DirectoryHTup.ContainsKey(key))
                        {
                            this.DirectoryHTup.Add(key, new HtupleEx());
                        }
                        this.DirectoryHTup[key].DirectoryHTuple = hTuple;
                        this.DirectoryHTup[key].DicIsSave =
                            Convert.ToBoolean(dataGridView.Rows[i].Cells[1].EditedFormattedValue);
                        this.DirectoryHTup[key].DicIsSaveData =
                               Convert.ToBoolean(dataGridView.Rows[i].Cells[2].EditedFormattedValue);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存动态属性错误：" + ex.Message);
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="control"></param>
        public void UpData(Control control)
        {
            DataGridView dataGridView = control as DataGridView;
            if (dataGridView != null)
            {
                dataGridView.Rows.Clear();
                int i = 0;

                foreach (var item in this.DirectoryHTup)
                {
                    i = dataGridView.Rows.Add();
                    dataGridView.Rows[i].Cells[0].Value = item.Key;
                    dataGridView.Rows[i].Cells[1].Value = item.Value.DicIsSave;
                    dataGridView.Rows[i].Cells[2].Value = item.Value.DicIsSaveData;
                    dataGridView.Rows[i].Cells[3].Value = item.Value.DirectoryHTuple;
                }
            }
        }

        #endregion 接口实现

        public class HtupleEx
        {
            public HTuple DirectoryHTuple { get; set; }
            public bool DicIsSave { get; set; }
            public bool DicIsSaveData { get; set; }
        }

        [Browsable(false)]
        public Dictionary<string, HtupleEx> DirectoryHTup = new Dictionary<string, HtupleEx>();

        public void SetSave(string key, bool isSave = true)
        {
            if (DirectoryHTup.ContainsKey(key))
            {
                DirectoryHTup[key].DicIsSave = isSave;
            }
            else
            {
                DirectoryHTup.Add(key, new HtupleEx() { DicIsSave = isSave });
            }
        }

        public override string ProjectTypeName => "视觉基类";

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public HTuple this[string index]
        {
            get
            {
                if (this.DirectoryHTup.ContainsKey(index))
                {
                    return DirectoryHTup[index].DirectoryHTuple;
                }
                else
                {
                    DirectoryHTup.Add(index, new HtupleEx() { DirectoryHTuple = "null" });
                    this.LogErr("变量" + index + "不存在！");
                }
                //MessageBox.Show(index+"不存在");
                return DirectoryHTup[index].DirectoryHTuple;
            }
            set
            {
                if (DirectoryHTup.ContainsKey(index))
                {
                    DirectoryHTup[index].DirectoryHTuple = value;
                }
                else
                {
                    DirectoryHTup.Add(index, new HtupleEx() { DirectoryHTuple = value });
                }
            }
        }

        /// <summary>
        /// 清除不保存的变量
        /// </summary>
        public void ClerItem()
        {
            try
            {
                Dictionary<string, HtupleEx> keyValuePairs = new Dictionary<string, HtupleEx>();

                foreach (var item in DirectoryHTup)
                {
                    if (item.Key == "")
                    {
                        continue;
                    }
                    if (item.Value == null)
                    {
                        continue;
                    }
                    if (item.Value.DicIsSave)
                    {
                        keyValuePairs.Add(item.Key, item.Value);
                    }
                    else if (item.Value.DicIsSaveData)
                    {
                        keyValuePairs.Add(item.Key, item.Value);
                        keyValuePairs[item.Key].DirectoryHTuple = "null";
                    }
                }
                DirectoryHTup = keyValuePairs;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 删除变量
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            //if (this.DirectoryHTuple.ContainsKey(name))
            //{
            //    this.DirectoryHTuple.Remove(name);
            //}

            if (DirectoryHTup.ContainsKey(name))
            {
                DirectoryHTup.Remove(name);
            }
        }

        /// <summary>
        /// 设置元祖初始值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <param name="isSave">是否保存</param>
        public void SetDefault(string name, HTuple value, bool isSave = false, bool isSaveData = false)
        {
            if (!DirectoryHTup.ContainsKey(name))
            {
                this[name] = value;
                DirectoryHTup[name].DicIsSave = isSave;
                DirectoryHTup[name].DicIsSaveData = isSaveData;
            }
            else
            {
                if (DirectoryHTup[name].DirectoryHTuple.TupleEqual("null"))
                {
                    this[name] = value;
                    DirectoryHTup[name].DicIsSave = isSave;
                    DirectoryHTup[name].DicIsSaveData = isSaveData;
                }
            }
        }

        /// <summary>
        /// 设置目标值写入本地历史表格
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isDataE"></param>
        public void SetDefault(string name, bool isDataE = true)
        {
            SetDefault(name, "null", false, isDataE);
        }

        /// <summary>
        /// 元祖变量是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public bool IsExist(string name)
        {
            if (this.DirectoryHTup.ContainsKey(name))
            {
                if (DirectoryHTup[name].DirectoryHTuple.TupleEqual("null"))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, HTuple> SetData()
        {
            Dictionary<string, HTuple> cname = new Dictionary<string, HTuple>();
            try
            {
                foreach (var item in this.DirectoryHTup)
                {
                    if (item.Value.DicIsSaveData)
                    {
                        cname.Add(this.Name + "." + item.Key, item.Value.DirectoryHTuple);
                    }
                }
            }
            catch (Exception)
            {
            }

            return cname;
        }

        /// <summary>
        ///读取
        /// </summary>
        /// <param name="path"></param>
        public void ReadTxt(string path)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        public override void Dispose()
        {
            if (this.DirectoryHTup != null)
            {
                DirectoryHTup.Clear();
            }

            DirectoryHTup = null;
            base.Dispose();
        }

        /// <summary>
        /// 根据T1的序列进行T1和T2排序
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="soetT1"></param>
        /// <param name="sortT2"></param>
        public static void SortTuple(HTuple t1, HTuple t2, out HTuple soetT1, out HTuple sortT2)
        {
            sortT2 = new HTuple();
            soetT1 = new HTuple();
            try
            {
                HTuple indx1 = t1.TupleSortIndex();
                soetT1 = t1.TupleSelect(indx1);
                sortT2 = t2.TupleSelect(indx1);
            }
            catch (Exception)
            {
            }
        }
    }
}