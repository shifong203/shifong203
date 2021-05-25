using ErosSocket.ErosConLink;
using HalconDotNet;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project;
using Vision2.Project.DebugF;
using Vision2.Project.formula;
using Vision2.Project.Mes;
using Vision2.Project.ProcessControl;
using Vision2.vision.Cams;
using Vision2.vision.HalconRunFile.Controls;
using Vision2.vision.HalconRunFile.RunProgramFile;
using MVReader;
using System.Text;
using System.Runtime.Serialization;
using Vision2.Project.DebugF.IO;
using ErosSocket.DebugPLC.Robot;

namespace Vision2.vision
{
    [Serializable]
    /// <summary>
    /// 视觉常用方法
    /// </summary>
    public class Vision : ProjectObj, ProjectNodet.IClickNodeProject
    {
        public enum ImageTypeObj
        {
             Image3=0,
             Gray=1,
             R=2,
             G=3,
             B=4,
             H=5,
             S=6,
             V=7,
        }

        public override string FileName => "Vision";
        public override string Text { get; set; } = "视觉";
        public override string SuffixName => ".vision";
        public override string ProjectTypeName => "视觉程序";
        public override string Name => "视觉";
        public Vision()
        {
            try
            {
                TabPage.Text = TabPage.Name = this.Name;
            }
            catch (Exception)
            {

            }
            this.Information = "视觉程序结构";
        }
        public static string GetFilePath()
        {
            return ProjectINI.In.ProjectPathRun + "\\" + _instance.FileName + "\\";
        }
        public static string VisionPath { get { return GetFilePath() + Product.ProductionName + "\\"; } }

        /// <summary>
        /// 启用的视觉程序
        /// </summary>
        [Browsable(false)]
        public Dictionary<string, bool> VisionPr = new Dictionary<string, bool>();
        private TreeNode treeNodeRun = new TreeNode() { Name = "视觉程序", Text = "视觉程序" };
        private TreeNode treeNodeDCams = new TreeNode() { Name = "dCams", Text = "本地相机" };
        private TreeNode treeNodeCams = new TreeNode() { Name = "Cams", Text = "相机" };
        private TreeNode treeNodeD = new TreeNode() { Name = "硬件库", Text = "硬件库" };
        private TreeNode treeNodeCoonrdinates = new TreeNode() { Name = "坐标系", Text = "坐标系" };
        private TreeNode treeNodeCoonrdinate3Ds = new TreeNode() { Name = "3D坐标系", Text = "3D坐标系" };
        private TreeNode treeNodeVisionWindow = new TreeNode() { Name = "图像显示", Text = "图像显示" };
        /// <summary>
        /// 是否支持切换产品
        /// </summary>
        [DescriptionAttribute("是否支持切换产品。"), Category("产品"), DisplayName("是否支持产品")]
        public bool ISPName { get; set; }


        [Description("复判端口"), Category("远程复判"), DisplayName("复判端口")]
  
        public int RsetPort { get; set; } = -1;

        [Description("图像属性，"), Category("显示"), DisplayName("显示区域宽度")]

        public int LineWidth { get; set; } = 1;

        [Description("图像文本大小，"), Category("显示"), DisplayName("显示文本大小")]

        public int FontSize { get; set; } = 20;


        [Description("图像文本大小，"), Category("显示"), DisplayName("显示文本大小")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "微软雅黑", "宋体")]
        public string Font { get; set; } = "微软雅黑";


        [Description("显示视觉窗口的方式，"), Category("显示"), DisplayName("图像显示模式")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "平铺", "选项卡", "主副窗口")]
        public string ControlsMode { get; set; } = "选项卡";

        [DescriptionAttribute("单步模式下一步。"), Category("触发器"), DisplayName("下一步变量名")]
        public string NextSetp { get; set; } = string.Empty;



        [DescriptionAttribute("延时模式或完成模式。"), Category("触发器"), DisplayName("拍照完成或延时模式")]
        public bool sTime { get; set; }

        [DescriptionAttribute("读取托盘ID编号的地址。"), Category("触发器"), DisplayName("托盘ID地址")]
        [Editor(typeof(LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string ReadIDName { get; set; } = string.Empty;

        [DescriptionAttribute("单个复判检测位置。"), Category("复判模式"), DisplayName("单个复判")]
        /// <summary>
        /// 复判单个位置
        /// </summary>
        public bool RestT { get; set; }

        [Description("结果区域膨胀。"), Category("结果区域"), DisplayName("膨胀区域")]
        public int DilationRectangle1 { get; set; } = 300;

        [Description("ROI颜色。"), Category("结果区域"), DisplayName("ROI颜色")]
        public RunProgram.ColorResult ROIColr { get; set; } =RunProgram.ColorResult.blue;

        [DescriptionAttribute("单位转换比例。"), Category("数据"), DisplayName("单位比例")]
        public int Transform { get; set; } = 1;

        [DescriptionAttribute("小数位数。"), Category("数据"), DisplayName("小数位数")]
        public int Decimal_point { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public int TrayID { get; set; }

        [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("1通道")]
        public byte H1 { get; set; } = 255;

        [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("2通道")]
        public byte H2 { get; set; } = 255;

        [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("3通道")]
        public byte H3 { get; set; } = 255;

        [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("4通道")]
        public byte H4 { get; set; } = 255;

        public bool H1Off;

        public bool H2Off;

        public bool H3Off;

        public bool H4Off;

        [Description("供应商光源控制方式，"), Category("光源控制"), DisplayName("控制器供应商")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "浮根", "凯威", "")]
        public string OffName { get; set; }

        private string CheckChData()
        {
            string data = "S";
            if (H1Off)
            {
                data += H1.ToString("000") + "T";
            }
            else
            {
                data += H1.ToString("000") + "F";
            }
            if (H2Off)
            {
                data += H2.ToString("000") + "T";
            }
            else
            {
                data += H2.ToString("000") + "F";
            }

            if (H3Off)
            {
                data += H3.ToString("000") + "T";
            }
            else
            {
                data += H3.ToString("000") + "F";
            }
            if (H4Off)
            {
                data += H4.ToString("000") + "T";
            }
            else
            {
                data += H4.ToString("000") + "F";
            }

            return data + "C#";
        }
        public string CheckChKWData()
        {
            string data = "S";
            if (H1Off)
            {
                data += H1.ToString("000") + "T";
                SerialPort.Write("#1106411");

            }
            else
            {
                SerialPort.Write("#2106411");

                data += H1.ToString("000") + "F";
            }
            Thread.Sleep(100);
            if (H2Off)
            {

                SerialPort.Write("#1206412");
                data += H2.ToString("000") + "T";
            }
            else
            {
                SerialPort.Write("#2106412");
                data += H2.ToString("000") + "F";
            }
            Thread.Sleep(100);
            if (H3Off)
            {
                SerialPort.Write("#1306413");
                data += H3.ToString("000") + "T";
            }
            else
            {
                SerialPort.Write("#2106412");
                data += H3.ToString("000") + "F";
            }
            Thread.Sleep(100);
            if (H4Off)
            {

                SerialPort.Write("#1406414");
                data += H4.ToString("000") + "T";
            }
            else
            {
                SerialPort.Write("#2106413");
                data += H4.ToString("000") + "F";
            }

            return data;
        }
        public SerialPort GetSerPort()
        {
            return SerialPort;
        }

        public void SetHx()
        {
            if (OffName == "浮根")
            {
                SerialPort.Parity = Parity.None;
                SerialPort.BaudRate = 19200;
                SerialPort.StopBits = StopBits.One;
            }
            else
            {
                SerialPort.Parity = Parity.None;
                SerialPort.BaudRate = 9600;
                SerialPort.StopBits = StopBits.One;
            }
            if (!SerialPort.IsOpen)
            {
                SerialPort.PortName = Rs232Name;
                SerialPort.Open();
            }
            if (OffName == "浮根")
            {
                SerialPort.Write(CheckChData() + "C#");
            }
            else
            {
                CheckChKWData();
            }
        }
        public void SetOFF()
        {
            if (!SerialPort.IsOpen)
            {
                SerialPort.PortName = Rs232Name;
                SerialPort.Open();
            }
            SerialPort.Write(CheckChData() + "C#");
        }

        SerialPort SerialPort = new SerialPort();

        public string Rs232Name { get; set; } = "COM1";

        System.IO.Ports.Parity Parity { get; set; } = Parity.Even;


        SocketServer SoServer;

        int BaudRate { get; set; } = 19200;

        int DataBits { get; set; } = 1;
        StopBits StopBits { get; set; } = StopBits.None;

        /// <summary>
        ///读取程序到节点
        /// </summary>
        /// <param name="toolStrip"></param>
        /// <param name="dss"></param>
        /// <returns></returns>
        public string RardPathHaolcnP(object toolStrip, object dss)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "视觉程序.eros|*.eros|文本文件.txt|*.txt|所有文件|*";

            openFileDialog.ShowDialog();                   //展示对话框
            string name = openFileDialog.FileName;          //获得打开的文件的路径

            if (openFileDialog.FileName.Length != 0)
            {
                switch (Path.GetExtension(openFileDialog.FileName))
                {
                    case ".eros":
                        HalconRun halconRun;
                        HalconRun.RardThis(openFileDialog.FileName, out halconRun);
                    stru:
                        if (this.Himagelist.ContainsKey(halconRun.Name))
                        {
                            //弹出带输入的
                            string sd = Interaction.InputBox("程序已存在！请重新输入名称", "重命名程序", halconRun.Name, 100, 100);
                            if (sd.Length == 0)
                            {
                                return name;
                            }
                            else
                            {
                                halconRun.Name = sd;
                                goto stru;
                            }
                        }
                        else
                        {
                            this.Himagelist.Add(halconRun.Name, halconRun);
                        }
                        break;

                    case ".txt":
                        break;

                    case ".vision":
                        break;

                    case ".cam":
                        DahuaCamera Cam;
                        if (ProjectINI.ReadPathJsonToCalss<DahuaCamera>(name, out Cam))
                        {
                            Cam = new DahuaCamera();
                        }
                        Cam.IsCamConnected = false;

                    struCam:
                        if (this.RunCams.ContainsKey(Cam.Name))
                        {
                            //弹出带输入的
                            string sd = Interaction.InputBox(Cam.Name + "已存在！请重新输入名称", "重命名程序", Cam.Name, 100, 100);
                            if (sd.Length == 0)
                            {
                                return "";
                            }
                            else
                            {
                                Cam.Name = sd;
                                goto struCam;
                            }
                        }
                        else
                        {
                            this.RunCams.Add(Cam.Name, Cam);
                        }
                        TreeNode treeNode = new TreeNode();
                        treeNode.Name = treeNode.Text = Cam.Name;
                        treeNode.Tag = Cam;
                        TreeView.SelectedNode.Nodes.Add(treeNode);
                        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                        treeNode.ContextMenuStrip = contextMenuStrip;
                        contextMenuStrip.Items.Add("删除").Click += VisionRemoveCam_Click;
                        break;

                    case ".coonrdinate":
                        break;

                    default:
                        break;
                }

                //UpProjectNode(Node.Parent);
            }
            return "";
        }


        /// <summary>
        /// 更新参数节点
        /// </summary>
        /// <param name = "tVProject" ></ param >
        public override void UpProjectNode(TreeNode treeNodet = null)
        {
            try
            {
                if (treeNodet != null)
                {
                    base.UpProjectNode(treeNodet);
                }

                if (Node == null || Node.TreeView.IsDisposed)
                {
                    return;
                }
                Node.Nodes.Clear();
                Node.ImageKey = Node.SelectedImageKey = "video-camera-vector.png";


                treeNodeRun = new TreeNode() { Name = "视觉程序", Text = "视觉程序" };
                treeNodeDCams = new TreeNode() { Name = "dCams", Text = "本地相机" };
                treeNodeCams = new TreeNode() { Name = "Cams", Text = "相机" };
                treeNodeD = new TreeNode() { Name = "硬件库", Text = "硬件库" };
                treeNodeCoonrdinates = new TreeNode() { Name = "坐标系", Text = "坐标系" };
                treeNodeCoonrdinate3Ds = new TreeNode() { Name = "3D坐标系", Text = "3D坐标系" };
                treeNodeVisionWindow = new TreeNode() { Name = "图像显示", Text = "图像显示" };
                //添加子节点
                if (!Node.Nodes.Contains(treeNodeRun))
                {
                    Node.Nodes.Add(treeNodeRun);
                }
                treeNodeRun.Tag = this.Himagelist;
                treeNodeRun.ImageKey = treeNodeRun.SelectedImageKey = "box-vector.png";

                if (!treeNodeRun.IsExpanded)
                {
                    treeNodeRun.Toggle();
                }
                if (!Node.Nodes.Contains(treeNodeCams))
                {
                    Node.Nodes.Add(treeNodeCams);
                }
                treeNodeCams.SelectedImageKey = treeNodeCams.ImageKey = "iphone-we.png";

                //坐标系
                if (!Node.Nodes.Contains(treeNodeCoonrdinates))
                {
                    Node.Nodes.Add(treeNodeCoonrdinates);
                }

                treeNodeCoonrdinates.Tag = this.DicCoordinate;
                treeNodeCoonrdinates.Nodes.Clear();
                treeNodeCoonrdinates.ContextMenuStrip = new ContextMenuStrip();
                if (treeNodeCoonrdinates.ContextMenuStrip.Items.Find("添加坐标系", false).Length == 0)
                {
                    treeNodeCoonrdinates.ContextMenuStrip.Items.Add("添加坐标系").Click += Vision_Click2;
                    void Vision_Click2(object sender, EventArgs e)
                    {
                        //弹出带输入的
                        string sd = Interaction.InputBox("请输入名称", "创建程序", "标定坐标", 100, 100);
                        if (sd != "")
                        {
                        strat:
                            if (!DicCoordinate.ContainsKey(sd))
                            {
                                DicCoordinate.Add(sd, new Coordinate());
                                TreeNode ds = treeNodeCoonrdinates.Nodes.Add(sd);
                                ds.ContextMenuStrip = new ContextMenuStrip();
                                ds.ContextMenuStrip.Items.Add("删除").Click += RemoveCoo_Click2;
                                void RemoveCoo_Click2(object senderd, EventArgs et)
                                {
                                    if (this.DicCoordinate.ContainsKey(ds.Text))
                                    {
                                        this.DicCoordinate.Remove(ds.Text);
                                        ds.Remove();
                                    }
                                }
                                ds.Tag = DicCoordinate[sd];
                                ds.Text = sd;
                            }
                            else
                            {
                                sd = Interaction.InputBox("程序已存在！请重新输入名称", "重命名程序", sd, 100, 100);
                                goto strat;
                            }
                        }
                    }
                }
                foreach (var item in this.DicCoordinate)
                {
                    TreeNode ds = treeNodeCoonrdinates.Nodes.Add(item.Key);
                    ds.ContextMenuStrip = new ContextMenuStrip();
                    ds.ContextMenuStrip.Items.Add("删除").Click += RemoveCoo_Click2;
                    void RemoveCoo_Click2(object sender, EventArgs e)
                    {
                        if (this.DicCoordinate.ContainsKey(ds.Text))
                        {
                            this.DicCoordinate.Remove(ds.Text);
                            ds.Remove();
                        }
                    }
                    ds.Tag = item.Value;
                    ds.Text = item.Key;
                }
                //视觉程序
                ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                treeNodeRun.ContextMenuStrip = contextMenuStrip;

                ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
                toolStripMenuItem.Text = "添加";
                contextMenuStrip.Items.Add(toolStripMenuItem);
                toolStripMenuItem.DropDownItems.Add("新建").Click += NewVision_Click;
                toolStripMenuItem.DropDownItems.Add("现有").Click += AddVision_Click1;
                toolStripMenuItem = new ToolStripMenuItem();
                toolStripMenuItem.Text = "打开程序";
                contextMenuStrip.Items.Add(toolStripMenuItem);
                toolStripMenuItem.Click += ToolStripMenuItem_Click1;
                toolStripMenuItem = new ToolStripMenuItem();
                toolStripMenuItem.Text = "删除所有程序";
                contextMenuStrip.Items.Add(toolStripMenuItem);
                toolStripMenuItem.Click += ToolStripMenuItem_Click2;
                Dictionary<string, HalconRun> tiem = new Dictionary<string, HalconRun>();
                foreach (TreeNode item in treeNodeRun.Nodes)
                {
                    item.Tag = null;
                }
                treeNodeRun.Nodes.Clear();

                foreach (var item in this.Himagelist)
                {
                    item.Value.UpProjectNode(treeNodeRun);
                    //item.Value.SetUesrContrsl(MainForm1.MainFormF.UserControl2.tabControl1);
                    toolStripMenuItem = new ToolStripMenuItem();
                    toolStripMenuItem.Name = toolStripMenuItem.Text = "删除";

                    toolStripMenuItem.Tag = item.Key;
                    if (item.Value.GetNode().ContextMenuStrip.Items.Find(toolStripMenuItem.Name, false).Length == 0)
                    {
                        item.Value.GetNode().ContextMenuStrip.Items.Add(toolStripMenuItem);
                        toolStripMenuItem.Click += ToolStripMenuItem_Click;
                    }
                    if (treeNodet != null)
                    {
                        if (!treeNodet.IsExpanded)
                        {
                            treeNodet.Toggle();
                        }
                    }

                    tiem.Add(item.Value.Name, item.Value);
                }

                this.Himagelist = tiem;
                //运行相机
                treeNodeCams.Tag = CamParam.Cam_information.In;
                if (!treeNodeCams.IsExpanded)
                {
                    treeNodeCams.Toggle();
                }
                Dictionary<string, DahuaCamera> keyValuePairs = new Dictionary<string, DahuaCamera>();
                foreach (TreeNode item in treeNodeCams.Nodes)
                {
                    item.Tag = null;
                }
                treeNodeCams.Nodes.Clear();
                foreach (var item in this.RunCams)
                {
                    keyValuePairs.Add(item.Value.Name, item.Value);
                    TreeNode treeNodeCam = new TreeNode();
                    if (treeNodeCams.Nodes.Find(item.Value.Name, false).Length == 1)
                    {
                        treeNodeCam = treeNodeCams.Nodes.Find(item.Value.Name, false)[0];
                    }
                    else
                    {
                        treeNodeCams.Nodes.Add(treeNodeCam);
                    }
                    treeNodeCam.Text = treeNodeCam.Name = item.Value.Name;
                    treeNodeCam.Tag = item.Value;
                    contextMenuStrip = new ContextMenuStrip();
                    treeNodeCam.ContextMenuStrip = contextMenuStrip;
                    contextMenuStrip.Items.Add("删除").Click += VisionRemoveCam_Click;
                    treeNodeCam.Toggle();
                }
                this.RunCams = keyValuePairs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Name + "刷新错误:" + ex.Message);
            }
        }

        private void ToolStripMenuItem_Click2(object sender, EventArgs e)
        {
            try
            {
                DisposeHiamgeListRemove();
                GC.Collect();
                this.UpProjectNode(Node.Parent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        /// <summary>
        /// 打开程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Click1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "所有文件|*";

            openFileDialog.ShowDialog();                   //展示对话框
            string name = openFileDialog.FileName;          //获得打开的文件的路径

            if (openFileDialog.FileName != "")
            {
                try
                {
                    Vision.Instance.ReadFormula(openFileDialog.FileNames);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisionRemoveCam_Click(object sender, EventArgs e)
        {
            try
            {
                DahuaCamera camParam = TreeView.SelectedNode.Tag as DahuaCamera;
                if (camParam != null)
                {
                    camParam.CloseCam();
                    if (RunCams.ContainsKey(camParam.Name))
                    {
                        RunCams.Remove(camParam.Name);
                    }
                    camParam = null;
                    TreeView.SelectedNode.Remove();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void AddCam_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "文本文件|*.txt";
            openFileDialog.ShowDialog();                   //展示对话框
            string name = openFileDialog.FileName;          //获得打开的文件的路径
            if (openFileDialog.FileName.Length != 0)
            {
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            if (Himagelist.ContainsKey(item.Tag.ToString()))
            {
                Himagelist[item.Tag.ToString()].Dispose();
            }
            Himagelist.Remove(item.Tag.ToString());

            TreeView.SelectedNode.Remove();
        }

        /// <summary>
        /// 添加现有程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddVision_Click1(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\Vision\\";
                openFileDialog.Filter = "程序文件.eros|*.eros|文本文件|*.txt";
                openFileDialog.ShowDialog();                   //展示对话框
                string name = openFileDialog.FileName;          //获得打开的文件的路径
                if (openFileDialog.FileName.Length != 0)
                {
                    HalconRun halconRun;
                    HalconRun.RardThis(openFileDialog.FileName, out halconRun);
                    if (halconRun == null)
                    {
                        return;
                    }
                stru:
                    if (this.Himagelist.ContainsKey(halconRun.Name))
                    {
                        //弹出带输入的
                        string sd = Interaction.InputBox("程序已存在！请重新输入名称", "重命名程序", halconRun.Name, 100, 100);
                        if (sd.Length == 0)
                        {
                            return;
                        }
                        else
                        {
                            halconRun.Name = sd;
                            goto stru;
                        }
                    }
                    else
                    {
                        this.Himagelist.Add(halconRun.Name, halconRun);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 添加新视觉程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewVision_Click(object sender, EventArgs e)
        {
            //弹出带输入的
            string mesage = "请输入名称";
            string mesaText = "新建程序";
            string sd = "";
        sdt:
            sd = Interaction.InputBox(mesage, mesaText, sd, 100, 100);
            if (sd.Length != 0)
            {
                if (Himagelist.ContainsKey(sd))
                {
                    mesage = "已存在！请重新输入名称";
                    mesaText = "重命名程序";
                    goto sdt;
                }
                if (!this.Himagelist.ContainsKey(sd))
                {
                    this.Himagelist.Add(sd, new HalconRun() { Name = sd });
                }
                TreeNode treeNode = treeNodeRun.Nodes.Add(sd);
                this.Himagelist[sd].initialization();
                treeNode.Tag = this.Himagelist[sd];
                treeNode.Name = sd;

                treeNode.Text = sd;
                return;
            }
        }

    
        public Control GetThisControl()
        {
            return new VisionUserControl1(this) { Dock = DockStyle.Fill };
        }
        [Category("识别参数"), DisplayName("识别解码"), Description("对二维码解码encoding，utf8，"),
            TypeConverter(typeof(ErosConverter)),
      ErosConverter.ThisDropDown("", false, "utf8", "locale")]
        public string Filename_encoding
        {
            get { return filename_encoding; }
            set
            {
                filename_encoding = value;

            }
        }
        private string filename_encoding = "";

        /// <summary>
        /// 关闭方法
        /// </summary>
        public override void Close()
        {
            foreach (var item in RunCams)
            {
                item.Value.CloseCam();
            }
            foreach (var item in this.Himagelist)
            {
                item.Value.Close();
            }
        }

        /// <summary>
        /// 保存实例
        /// </summary>
        public override void SaveThis(string path)
        {
            try
            {
                string prodName = Project.formula.Product.ProductionName;
                if (!this.ISPName)
                {
                    prodName = "M1";
                }
                ListHalconName = new List<string>();
                foreach (var item in Himagelist)
                {
                    ListHalconName.Add(item.Value.Name);
                    //item.Value.Name = item.Key;
                    item.Value.SaveThis(path + "\\" + FileName + "\\" + prodName + "\\");
                }
                ProjectINI.ClassToJsonSavePath(DicCoordinate, path + "\\" + FileName + "\\" + prodName + "\\CoordinateS");
                base.SaveThis(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveRunPojcet()
        {
            string prodName = Project.formula.Product.ProductionName;
            foreach (var item in Himagelist)
            {
                item.Value.SaveThis(Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\" + FileName + "\\" + prodName + "\\");
            }
        }
        /// <summary>
        /// 读取图像程序
        /// </summary>
        /// <returns></returns>
        public static bool RardHimageList(string productionName)
        {
            bool isdw = false;
            if (Vision.Instance.ISPName)
            {
                try
                {
                    string path = Application.StartupPath + "\\Project\\" + ProjectINI.In.ProjectName + "\\" + ProjectINI.In.RunName;
                    Dictionary<string, HalconRun> Hitem = new Dictionary<string, HalconRun>();
                    Dictionary<string, Coordinate> DicCitem = new Dictionary<string, Coordinate>();
                    foreach (var item in Instance.Himagelist)
                    {
                        HalconRun halconRun = null;
                        halconRun = item.Value.ReadThis<HalconRun>(path + "\\" + productionName + "\\" + Instance.FileName, halconRun);
                        Hitem.Add(halconRun.Name, halconRun);
                        halconRun.initialization();
                    }
                    isdw = ProjectINI.ReadPathJsonToCalss(path + "\\" + productionName + "\\" + Instance.FileName + "\\CoordinateS", out DicCitem);
                    if (isdw)
                    {
                        Instance.Himagelist = Hitem;
                        Instance.DicCoordinate = DicCitem;
                    }

                }
                catch (Exception ex)
                {


                }
            }
            else
            {
                return true;
            }
            return isdw;
        }

        /// <summary>
        /// 读取实例
        /// </summary>
        /// <param name="path"></param>
        /// <param name="control">显示控件</param>
        public void UpReadThis(string path, string productionName)
        {
            string datpath = path + "\\" + FileName + "\\" + this.Name + ".vision";
            if (path.EndsWith(".vision"))
            {
                datpath = path;
                path = Path.GetDirectoryName(Path.GetDirectoryName(path));
            }
            Vision visionf;
            ProjectINI.ReadPathJsonToCalss<Vision>(datpath, out visionf);
            if (visionf == null)
            {
                datpath = path + "备份\\" + FileName + "\\" + this.Name + ".vision";
                ProjectINI.ReadPathJsonToCalss(datpath, out visionf);
            }
            Vision.Instance = visionf;

            if (Vision.Instance.VisionPr.Count == 0)
            {
                Vision.AddRunNames(new string[]
                {
                    "添加模板",
                    "添加测量",
                    "添加图像扫码",
                    "添加二值化检测",
                    "添加OCR识别",
                    "添加连接器识别",
                    "添加焊点检测",
                    "添加焊线检测",
                    "添加颜色识别"
                });
            }
            string listPath = path + "\\" + FileName + "\\" + productionName + "\\Halcon\\";
            List<string> list = new List<string>();
            UpReadThis(productionName);
            MainForm1.MainFormF.
            tabControl1.SelectedIndex = 1;
            DicCalib3D.Clear();
            string pathT = Vision.GetFilePath() + Calib.AutoCalibPoint.FileName;
            MainForm1.MainFormF.tabControl1.SelectedIndex = 0;
            HOperatorSet.CloseAllFramegrabbers();
            if (Vision.Instance . RsetPort >=0)
            {
                SoServer = new SocketServer();
                SoServer.EndIP = "127.0.0.1";
                SoServer.EndIP = "Any";
                SoServer.EndPort = Vision.Instance.RsetPort;
                SoServer.AsynLink();
                SoServer.PassiveEvent += SocketClint_PassiveEvent1;
                ForImageFor = new FormRestfDataIamge();
                ForImageFor.Show();
            }
            foreach (var item in StaticCon.SocketClint)
            {
                item.Value.PassiveEvent += SocketClint_PassiveEvent;
            }

            foreach (var item in Vision.Instance.DicSaveType)
            {
                if (StaticCon.GetLingkNameValue(item.Value.ReadCamName).ToString() != "")
                {
                    Thread thread = new Thread(() =>
                    {
                        while (true)
                        {
                            try
                            {
                                if (StaticCon.GetLingkNameValue(item.Value.ReadCamName).GetType() == typeof(bool) && ErosSocket.ErosConLink.StaticCon.GetLingkNameValue(item.Value.ReadCamName))
                                {
                                    Instance.Himagelist[item.Key].Image(Instance.Himagelist[item.Key].GetCam().GetImage());
                                    Instance.Himagelist[item.Key].CamImageEvent("1", null, 1);
                                }
                                else if (int.TryParse(StaticCon.GetLingkNameValue(item.Value.ReadCamName).ToString(), out int resultRun))
                                {
                                    if (resultRun > 0)
                                    {
                                        if (StaticCon.GetLingkIDValue(item.Value.ReadRunIDName, UClass.Int16, out dynamic value))
                                        {
                                            int.TryParse(value.ToString(), out int resultD);
                                            Instance.Himagelist[item.Key].Image(Instance.Himagelist[item.Key].GetCam().GetImage());

                                             StaticCon.SetLingkValue(item.Value.ReadCamName, 0, out string err);
                                            Instance.Himagelist[item.Key].CamImageEvent(resultD.ToString(), null, resultD);
                                        }
                                        else
                                        {
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            Thread.Sleep(10);
                        }
                    });
                    thread.Priority = ThreadPriority.Highest;
                    thread.IsBackground = true;
                    thread.Start();
                }
            }

            foreach (var item in Instance.RunCams)
            {
                item.Value.OpenCam();
            }
            GC.Collect();
        }

        FormRestfDataIamge ForImageFor;
        private string SocketClint_PassiveEvent1(byte[] key, SocketClint socket, Socket socketR)
        {
            try
            {
                    if (ErosProjcetDLL.Bytes.ByteHelper.BytesToObject(key) is PrestC)
                    {
                         PrestC prestC = ErosProjcetDLL.Bytes.ByteHelper.BytesToObject(key) as PrestC;
                        if (!ForImageFor.keyValuePairs.ContainsKey(prestC.LineName))
                        {
                           
                            ForImageFor.Invoke(new Action(() => {

                             ForImageFor.listBox2.Items.Add(prestC.LineName);
                             TabPage tabPage = new TabPage();
                            tabPage.Text = prestC.LineName;
                            TrayDataUserControl trayDataUserControl = new TrayDataUserControl();
                            trayDataUserControl.Dock = DockStyle.Fill;
                            ForImageFor.tabControl1.TabPages.Add(tabPage);
                            tabPage.Controls.Add(trayDataUserControl);
                            ErosSocket.DebugPLC.Robot.TrayRobot     trayRobot = new ErosSocket.DebugPLC.Robot.TrayRobot();
                            trayRobot.XNumber = (sbyte)prestC.XNumber;
                            trayRobot.YNumber = (sbyte)prestC.YNumber;
                            trayDataUserControl.SetTray(trayRobot);
                            ForImageFor.keyValuePairs.Add(prestC.LineName,new FormRestfDataIamge.OBJData() { socket = socketR,prest1= prestC,trayDataUser= trayDataUserControl });
                            }));
                        }
                        else
                       {
                            ForImageFor.keyValuePairs[prestC.LineName].trayDataUser.GetTrayEx().XNumber= (sbyte)prestC.XNumber;
                            ForImageFor.keyValuePairs[prestC.LineName].trayDataUser.GetTrayEx().YNumber = (sbyte)prestC.YNumber;
                            ForImageFor.keyValuePairs[prestC.LineName].trayDataUser.GetTrayEx().RestValue();
                            ForImageFor.keyValuePairs[prestC.LineName].socket= socketR;
                            ForImageFor.keyValuePairs[prestC.LineName].prest1 = prestC;
                        }
                           ForImageFor.SetData(ForImageFor.keyValuePairs[prestC.LineName]);     
                    }
                    else if (ErosProjcetDLL.Bytes.ByteHelper.BytesToObject(key) is PrestImageData)
                    {
                        PrestImageData data = ErosProjcetDLL.Bytes.ByteHelper.BytesToObject(key) as PrestImageData;
                        ForImageFor.keyValuePairs[data.LinkName].socket = socketR;
              
                    if (data != null)
                        {

                             ForImageFor.SetImageData(data);
                        }
                    }
            }
            catch (Exception ex)
            {
            }
            return "";
        }
        /// <summary>
        /// 加载程序
        /// </summary>
        /// <param name="nameP">产品名称</param>
        /// <returns>成功返回true</returns>
        public static bool UpReadThis(string nameP)
        {
            try
            {
                bool icong = false;
                MainForm1.MainFormF.Invoke(new Action(() =>
                {

                    string listPath = ProjectINI.In.ProjectPathRun + "\\" + Instance.FileName + "\\" + nameP;
                    if (Directory.Exists(listPath))
                    {
                        foreach (var item in Vision.Instance.Himagelist)
                        {
                            item.Value.Dispose();
                        }
                        Vision.Instance.Himagelist.Clear();
                    }
                    if (!Directory.Exists(listPath))
                    {
                        Instance.LogErr("加载视觉程序出错,未创建" + nameP);
                    }
                    else
                    {
                        if (Instance.ListHalconName == null)
                        {
                            string[] itmeName = Directory.GetFiles(listPath + "\\Halcon", "*.eros");

                            for (int i = 0; i < itmeName.Length; i++)
                            {
                                Instance.ListHalconName.Add(Path.GetFileNameWithoutExtension(itmeName[i]));
                            }
                        }
                        for (int i = 0; i < Instance.ListHalconName.Count; i++)
                        {
                            if (Instance.ListHalconName[i] == null)
                            {
                                Instance.ListHalconName.Clear();
                                string[] itmeName = Directory.GetFiles(listPath + "\\Halcon", "*.eros");
                                for (int i2 = 0; i2 < itmeName.Length; i2++)
                                {
                                    Instance.ListHalconName.Add(Path.GetFileNameWithoutExtension(itmeName[i2]));
                                }
                                break;
                            }
                        }
                        int det = 0;
                        foreach (var item in Instance.ListHalconName)
                        {
                            HalconRun halconRun;
                            HalconRun.RardThis(listPath + "\\Halcon\\" + item + ".eros", out halconRun);
                            if (halconRun == null)
                            {
                                continue;
                            }
                            Vision.Instance.AddHalconUI(halconRun, det);
                            det++;
                            halconRun.initialization();
                            Vision.Instance.Himagelist[item] = halconRun;
                        }
                        Vision.Instance.UpProjectNode();
                        icong = true;
                    }
                }));
                return icong;
            }
            catch (Exception ex)
            {
                Instance.LogErr("加载视觉程序出错", ex);
            }
            return false;
        }
        TabPage TabPage = new TabPage();
        public void AddHalconUI(HalconRun halcon, int idt)
        {
            try
            {
                if (Vision.Instance.ControlsMode == "平铺")
                {
                    try
                    {
                        if (MainForm1.MainFormF.tabControl1.TabPages.IndexOfKey(this.Name) < 0)
                        {
                            MainForm1.MainFormF.tabControl1.TabPages.Add(TabPage);
                        }
                        if (MainForm1.MainFormF.tabControl1.TabPages.IndexOfKey(halcon.Name) >= 0)
                        {
                            TabPage tabPage = MainForm1.MainFormF.tabControl1.TabPages[MainForm1.MainFormF.tabControl1.TabPages.IndexOfKey(halcon.Name)];

                            MainForm1.MainFormF.tabControl1.TabPages.RemoveAt(MainForm1.MainFormF.tabControl1.TabPages.IndexOfKey(halcon.Name));
                            tabPage.Dispose();
                        }

                        Control[] control = TabPage.Controls.Find(halcon.Name, false);
                        if (control.Length <= 0)
                        {
                            Vision2UserControl vision;
                            GroupBox groupBox = new GroupBox();
                            groupBox.Name = groupBox.Text = halcon.Name;
                            groupBox.Width = 400;
                            groupBox.Height = 600;
                            vision = new Vision2UserControl();
                            int sd = idt / 2;
                            int dt = idt % 2;
                            groupBox.Location = new Point(new Size(groupBox.Width * dt, groupBox.Height * sd));
                            //point = axis.Location;
                            //i++;
                            //sdt = axis.Height + axis.Location.Y;
                            vision.Dock = DockStyle.Fill;
                            groupBox.Controls.Add(vision);
                            TabPage.Controls.Add(groupBox);
                            vision.Name = halcon.Name;
                            vision.UpHalcon(halcon);
                        }
                        else
                        {
                            control = control[0].Controls.Find(halcon.Name, false);
                            Vision2UserControl vision = control[0] as Vision2UserControl;
                            if (vision != null)
                            {
                                vision.UpHalcon(halcon);
                            }
                        }

                        HOperatorSet.GetImageSize(halcon.Image(), out HTuple width, out HTuple heigth);
                        if (width.Length == 1)
                        {
                            halcon.Width = width;
                            halcon.Height = heigth;
                            HOperatorSet.SetPart(halcon.hWindowHalcon(), 0, 0, halcon.Height - 1, halcon.Width - 1);
                        }
                        //vision.VisionControl();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    if (MainForm1.MainFormF.tabControl1.TabPages.IndexOfKey(this.Name) >= 0)
                    {
                        MainForm1.MainFormF.tabControl1.TabPages.Remove(TabPage);
                        TabPage.Dispose();
                    }
                    try
                    {
                        Vision2UserControl vision;
                        if (MainForm1.MainFormF.tabControl1.TabPages.IndexOfKey(halcon.Name) < 0)
                        {
                            vision = new Vision2UserControl();
                            TabPage tabPage = new TabPage();
                            tabPage.Name = tabPage.Text = halcon.Name;
                            tabPage.Controls.Add(vision);
                            MainForm1.MainFormF.tabControl1.TabPages.Add(tabPage);
                            tabPage.Enter += TabPage_GotFocus;
                        }
                        else
                        {
                            vision = MainForm1.MainFormF.tabControl1.TabPages[MainForm1.MainFormF.tabControl1.TabPages.IndexOfKey(halcon.Name)].Controls[0] as Vision2UserControl;
                        }
                        vision.Name = halcon.Name;
                        vision.Dock = DockStyle.Fill;
                        vision.UpHalcon(halcon);
                        HOperatorSet.GetImageSize(halcon.Image(), out HTuple width, out HTuple heigth);
                        if (width.Length == 1)
                        {
                            halcon.Width = width;
                            halcon.Height = heigth;
                            HOperatorSet.SetPart(halcon.hWindowHalcon(), 0, 0, halcon.Height - 1, halcon.Width - 1);
                        }
                        vision.VisionControl();
                        void TabPage_GotFocus(object sender, EventArgs e)
                        {
                            if (MainForm1.MainFormF.tabControl1.SelectedTab != (TabPage)sender)
                            {

                                try
                                {
                                    vision.UpHalcon();
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public void AddLibrary(RunProgram runProgram )
        {
            try
            {


            }
            catch (Exception)
            {
            }
        }

        public static  void SetFont(HTuple window)
        {
            try
            {
                HOperatorSet.SetFont(window, Vision.Instance.Font + "-" + Vision.Instance.FontSize);
            }
            catch (Exception ex)
            {
                Vision.ErrLog("设置字体失败" , ex);
            }
            //HOperatorSet.SetFont(window, "-Consolas-16-*-0-*-*-1-");
        }
        /// <summary>
        /// 链接方法
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string SocketClint_PassiveEvent(byte[] key, SocketClint socket, Socket socketR)
        {
            try
            {
                string message = "";
                message = socket.GetEncoding().GetString(key);
                string[] dataStrs = message.Split(',');
                if (message.ToLower().StartsWith(socket.Identifying, StringComparison.Ordinal))
                {
                    socket.RSend(Project.formula.Product.GetFormulaData(socket.Name, socket.Identifying, socket.Split, socket.Split_Mode), socketR);
                }//返回产品参数
                else if (dataStrs[0].ToLower() == "getproductname")
                {
                    socket.RSend("getProductName=" + Project.formula.Product.ProductionName, socketR);
                }
                else if (dataStrs[0].ToLower() == "call")
                {
                    if (dataStrs.Length >= 2)
                    {
                        HalconRun run = GetRunNameVision(dataStrs[1]);
                        run.LoopIndx = 0;
                        if (dataStrs.Length == 3)
                        {
                            run.GetCam().Key = dataStrs[2];
                            run.ReadCamImage();
                        }
                        else if (dataStrs.Length == 4)
                        {
                            run.GetCam().Key = dataStrs[1];
                            run.ReadCamImage();
                            string[] files = dataStrs[3].Split('\\');
                            if (dataStrs[2] == "SaveCalib3d")
                            {
                                run.UPStart();
                                run.ReadCamSave(Calib.AutoCalibPoint.GetFileName() + files[0] + "\\" + files[2] + "\\" + files[4] + ".bmp");
                                HTuple pos = run.GetRobotBaesPose();
                                run.AddMessage(pos);
                                HOperatorSet.WritePose(pos, Calib.AutoCalibPoint.GetFileName() + files[0] + "\\" + files[2] + "\\" + files[4] + ".dat");
                                if (Instance.DicCalib3D.ContainsKey(files[0]))
                                {
                                    Instance.DicCalib3D[files[0]].Errs = files[4] + "\\" + files[5];
                                    if (files[2] == "M")
                                    {
                                        Instance.DicCalib3D[files[0]].RunAutoMCalib(Calib.AutoCalibPoint.GetFileName() + files[0],
                                      int.Parse(files[4]), int.Parse(files[5]), run.hWindowHalcon(), pos, run.Image());
                                    }
                                    else
                                    {
                                        Instance.DicCalib3D[files[0]].RunAutoTCalib(Calib.AutoCalibPoint.GetFileName() + files[0],
                                       int.Parse(files[4]), int.Parse(files[5]), run.hWindowHalcon(), pos, run.Image());
                                    }
                                    socket.Send("SaveCalib3dOK");
                                    run.AddOBJ(Instance.DicCalib3D[files[0]].Get3DX());
                                    run.AddOBJ(Instance.DicCalib3D[files[0]].Get3DY());
                                    run.AddOBJ(Instance.DicCalib3D[files[0]].Get3DZ());
                                    run.AddShowObj(Instance.DicCalib3D[files[0]].GeT3d());
                                    run.AddMessage(Instance.DicCalib3D[files[0]].Errs);
                                    run.EndChanged();
                                }
                            }
                        }
               
                    }
                }
                else if (dataStrs[0].ToLower() == "setkey")
                {
                    HalconRun run = GetRunNameVision(dataStrs[1]);
                    //if (!run.GetCam().RealTimeMode)
                    //{
                    //    run.GetCam().ThreadSatring(run);
                    //}
                    if (dataStrs.Length >= 3)
                    {
                        run.LoopIndx = 0;
                        run.GetCam().Key = dataStrs[2];
                    }
                    if (dataStrs.Length == 4)
                    {
                        int.TryParse(dataStrs[3], out int dts);
                        run.LoopIndx = dts;
                        run.loopindxMax = 0;
                    }
                }
                else if (message.ToLower() == "pause")
                {
                    DebugCompiler.Pause();
                }
                else if (message.ToLower() == "stop")
                {
                    DebugCompiler.Stop();
                }
                else if (dataStrs[0].ToLower() == "setqrcodeed")
                {
                    Project.ProcessControl.ProcessUser.GetThis().SendTyID(dataStrs[1]);
                }
                else if (dataStrs[0].ToLower() == "setqrcodee")
                {
                    if (dataStrs.Length == 3)
                    {
                        Project.ProcessControl.ProcessUser.SetCodeProValue(dataStrs[1], "壳体", dataStrs[2]);
                    }
                }
                else if (dataStrs[0].ToLower() == "setqrcode")
                {
                    if (dataStrs.Length == 3)
                    {
                        Project.ProcessControl.ProcessUser.SetCodeProValue(dataStrs[1], "状态", dataStrs[2]);
                    }
                }
                else if (dataStrs[0].ToLower() == "autocalib")
                {
                    hruName = dataStrs[1];
                }
                else if (dataStrs[0].ToLower() == "autotool")
                {
                    Calib.CalidControl.ThisForm.AddPoint(message.Split(',')[2],
                        GetRunNameVision(message.Split(',')[1]).MRModelHomMat.Row.ToString(),
                        GetRunNameVision(message.Split(',')[1]).MRModelHomMat.Col.ToString());
                }
                else if (dataStrs[0].ToLower() == "getpoints" && dataStrs.Length == 2)
                {
                    List<ErosSocket.DebugPLC.PointFile> listP = ErosSocket.DebugPLC.PointFile.GetPointFile(dataStrs[1]);
                    if (listP != null)
                    {
                        char sipt = ';';
                        char sipt2 = '|';
                        string itmes = "getpoints," + listP.Count;
                        socket.RSend(itmes, socketR);
                        Thread.Sleep(500);
                        for (int i = 0; i < listP.Count; i++)
                        {
                            itmes = listP[i].Pstring + sipt2;
                            i++;
                            itmes += listP[i].Pstring;
                            socket.RSend(itmes, socketR);
                            Thread.Sleep(500);
                        }
                    }
                }
                else if (dataStrs[0].ToLower() == "getpointname")
                {
                    char sipt = ',';
                    ErosSocket.DebugPLC.PointFile point = ErosSocket.DebugPLC.PointFile.GetPointName(dataStrs[1], dataStrs[2]);
                    socket.Send(point.Pstring);
                }
                else if (dataStrs[0].ToLower() == "getpointp")
                {
                    ErosSocket.DebugPLC.PointFile point = ErosSocket.DebugPLC.PointFile.GetPointP(dataStrs[1], uint.Parse(dataStrs[2]));
                    if (point != null)
                    {
                        socket.RSend(point.Pstring, socketR);
                    }
                    else
                    {
                        socket.RSend("Err:点位错误", socketR);
                    }
                }
                else if (dataStrs[0].StartsWith(socket.CodeStartWith))
                {
                    Project.formula.UserFormulaContrsl.StaticAddQRCode(message.Remove(0, socket.CodeStartWith.Length));
                }
                else
                {
                    CalibPosition(message, socket, socketR, GetRunNameVision(hruName));
                }

            }
            catch (Exception ex)
            {
                Vision2.ErosProjcetDLL.Project.AlarmText.LogErr(ex.Message, "视觉通信错误" + socket.Name);
            }
            return "";
        }
        string hruName;
        /// <summary>
        /// 九点标定
        /// </summary>
        /// <param name="message"></param>
        static void CalibPosition(string message, SocketClint socket, Socket socketR, HalconRun halcon)
        {
            if (message.StartsWith("autocalib", StringComparison.Ordinal))
            {

            }   ///九点标定开始符号
            else if (message.StartsWith("Position", StringComparison.Ordinal))
            {
                string[] datas = message.Split(',');
                if (datas.Length == 3)
                {
                }
                string nds = message.Split(',')[0];
                int intds = Convert.ToInt16(nds[nds.Length - 1].ToString()) - 1;
                if (intds == 0)
                {
                    halcon.SetDefault("autocalibRow", new double[9], true);
                    halcon.SetDefault("autocalibCol", new double[9], true);
                    halcon.SetDefault("autocalibX", new double[9], true);
                    halcon.SetDefault("autocalibY", new double[9], true);
                    halcon["autocalibRow"] = new double[9];
                    halcon["autocalibCol"] = new double[9];
                    halcon["autocalibX"] = new double[9];
                    halcon["autocalibY"] = new double[9];
                }
                halcon.GetCam().Key = "One";
                halcon.ReadCamImage();
                Thread.Sleep(1000);
                //halcon.ShowVision("autocalib",);
                //if (halcon. GetHWindow() != null)
                //{
                //    halcon.GetHWindow().Up(halcon);
                //}
                Calib.CalidControl.ThisForm.AddPoint(datas[0], datas[1], datas[2]);
                Calib.CalidControl.ThisForm.AddPoint(message);
                Calib.CalidControl.ThisForm.AddPointRowsCols(halcon.MRModelHomMat.Col, halcon.MRModelHomMat.Row);
                socket.RSend("ok", socketR);
                halcon["autocalibRow"][intds] = halcon.MRModelHomMat.Row;
                halcon["autocalibCol"][intds] = halcon.MRModelHomMat.Col;
                halcon["autocalibX"][intds] = Convert.ToDouble(datas[1]);
                halcon["autocalibY"][intds] = Convert.ToDouble(datas[2]);
                halcon.GetResultOBj().AddMeassge("机械坐标X" + halcon["autocalibX"] + ";Y" + halcon["autocalibY"]
                   + "像素Col:" + halcon["autocalibCol"] + "Row:" + halcon["autocalibRow"]);
                if (intds == 8)
                {
                    HOperatorSet.VectorToHomMat2d(halcon["autocalibRow"], halcon["autocalibCol"],
                        halcon["autocalibY"], halcon["autocalibX"], out HTuple HomMat);
                    HOperatorSet.HomMat2dToAffinePar(HomMat, out HTuple sx, out HTuple sy, out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);
                    HOperatorSet.GenCrossContourXld(out HObject hObject, halcon["autocalibRow"], halcon["autocalibCol"], 20, 0);
                    HOperatorSet.AffineTransPoint2d(HomMat, halcon["autocalibRow"], halcon["autocalibCol"], out HTuple axisY, out HTuple axisX);
                    //HOperatorSet.HomMat2dSlant(HomMat, new HTuple(180).TupleDeg(), "y", 0, 0, out HomMat);
                    //HOperatorSet.HomMat2dSlant(HomMat, new HTuple(180).TupleDeg(), "x", 0, 0, out HomMat);
                    halcon.GetResultOBj().AddMeassge("转换矩阵" + HomMat);
                    halcon.GetResultOBj().AddMeassge("机械差X（" + axisX + "-" + halcon["autocalibX"] + "=" + halcon["autocalibX"].TupleSub(axisX) + "）;Y（"
                       + axisY + "-" + halcon["autocalibY"] + "=" + halcon["autocalibY"].TupleSub(axisY) + ")");
                    //this.CoordinatePXY.CoordHanMat2DXY = HomMat;
                    HOperatorSet.VectorToHomMat2d(halcon["autocalibX"], halcon["autocalibY"], halcon["autocalibCol"],
                        halcon["autocalibRow"], out HTuple HomMat2);

                    //HOperatorSet.HomMat2dSlant(HomMat2, new HTuple(180).TupleDeg(), "y", 0, 0,out HomMat2);
                    //HOperatorSet.HomMat2dSlant(HomMat2, new HTuple(180).TupleDeg(), "x", 0, 0, out HomMat2);
                    HOperatorSet.AffineTransPoint2d(HomMat2, 0, 0, out axisX, out axisY);
                    HOperatorSet.GenCrossContourXld(out HObject cros, axisY, axisX, 50, 0);
                    halcon.GetResultOBj().AddMeassge("机械原点row:" + axisY + ",col" + axisX);
                    hObject = hObject.ConcatObj(cros);
                    //if (halcon.KeyHObject.DirectoryHObject.ContainsKey("九点标定"))
                    //{
                    //    halcon.KeyHObject.DirectoryHObject["九点标定"] = new ObjectColor() { _HObject = hObject, };
                    //}
                    //else
                    //{
                    //    halcon.KeyHObject.DirectoryHObject.Add("九点标定", new ObjectColor() { _HObject = hObject, });
                    //}

                    halcon.AddOBJ(hObject.Clone());
                    Coordinate coordinate = new Coordinate();
                    coordinate.Rows = halcon["autocalibRow"];
                    coordinate.Columns = halcon["autocalibCol"];
                    coordinate.Xs = halcon["autocalibX"];
                    coordinate.Ys = halcon["autocalibY"];
                    coordinate.CoordHanMat2d = HomMat2;
                    coordinate.CoordHanMat2DXY = HomMat;
                    Calib.CalidControl.ThisForm.AddCoordinate(coordinate);
                }
                else
                {
                    halcon.ShowObj();
                }
            }   ///执行九点标定
        }

        /// <summary>
        /// 释放图像程序资源并清楚
        /// </summary>
        public void DisposeHiamgeListRemove()
        {
            try
            {



                foreach (var item in Vision.Instance.Himagelist)
                {
                    try
                    {
                        item.Value.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }

                Vision.Instance.Himagelist.Clear();
            }
            catch (Exception)
            {


            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pathNames"></param>
        public void ReadFormula(string[] pathNames)
        {
            DisposeHiamgeListRemove();
            TabControl tabControl1 = new TabControl();

            //this.RunUresControl.Controls.Add(tabControl1);
            tabControl1.Dock = DockStyle.Fill;
            foreach (var item in pathNames)
            {
                HalconRun halconRun;
                HalconRun.RardThis(item, out halconRun);
                Vision.Instance.Himagelist.Add(halconRun.Name, halconRun);
                halconRun.initialization();
                if (Vision.Instance.ControlsMode == "选项卡")
                {
                    try
                    {
                        Vision2UserControl vision = new Vision2UserControl();
                        vision.Name = halconRun.Name;
                        vision.Dock = DockStyle.Fill;
                        vision.UpHalcon(halconRun);
                        TabPage tabPage = new TabPage();
                        tabPage.Name = tabPage.Text = halconRun.Name;
                        tabPage.Controls.Add(vision);
                        tabControl1.TabPages.Add(tabPage);
                        tabPage.Enter += TabPage_GotFocus;
                        HOperatorSet.GetImageSize(halconRun.Image(), out HTuple width, out HTuple heigth);
                        if (width.TupleType() != 15)
                        {
                            halconRun.Width = width;
                            halconRun.Height = heigth;
                            HOperatorSet.SetPart(halconRun.hWindowHalcon(), 0, 0, halconRun.Height - 1, halconRun.Width - 1);
                        }
                        void TabPage_GotFocus(object sender, EventArgs e)
                        {
                            try
                            {
                                halconRun.GetHWindow().UpHalcon();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            this.UpProjectNode(Node.Parent);
            GC.SuppressFinalize(this);
        }


        #region //单例实例

        /// <summary>
        /// 单例实例
        /// </summary>
        private static Vision _instance = new Vision();

        public static Vision Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Vision();
                }
                return _instance;
            }
            set { _instance = value; }
        }
        /// <summary>
        /// 保持屏幕
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hwindowHalconID"></param>
        /// <param name="device"></param>

        public static void SaveWindow(string path, HTuple hwindowHalconID, string device = "jpg")
        {
            try
            {
                if (device.Split('.').Length == 2)
                {
                    device = device.Split('.')[1];
                }

                Directory.CreateDirectory(Path.GetDirectoryName(path));
                HOperatorSet.DumpWindow(hwindowHalconID, device, path);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
            }
        }
        #endregion //单例实例

        #region //定义变量

        /// <summary>
        /// Px位置参数名称
        /// </summary>
        [Browsable(false)]

        public Dictionary<string, List<string>> ListPrX { get; set; } = new Dictionary<string, List<string>>();
        /// <summary>
        /// Py位置参数名称
        /// </summary>
        [Browsable(false)]
        public Dictionary<string, List<string>> ListPrY { get; set; } = new Dictionary<string, List<string>>();

        [Browsable(false)]
        public Dictionary<string, SaveImageInfo> DicSaveType { get; set; } = new Dictionary<string, SaveImageInfo>();

        [Category("运行参数"), DisplayName("执行程序名称"),
            TypeConverter(typeof(RunNameConverter))]
        public string RunNameVision
        {
            get { return runNameVision; }

            set
            {
                runNameVision = value;
            }
        }

        private string runNameVision = "";


        string HostName;
        /// <summary>
        /// 设置图像焦点名
        /// </summary>
        /// <param name="name"></param>
        public void SetFocusRunHalconName(string name)
        {
            HostName = name;
        }

        public void ShowFocusRun(string name)
        {
            try
            {
                MainForm1.MainFormF.tabControl1.SelectTab(name);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 获得当前焦点的图像窗口
        /// </summary>
        /// <returns></returns>
        public static HalconRun GetFocusRunHalcon()
        {
            if (_instance.HostName == null)
            {
                return null;
            }
            if (_instance.Himagelist.ContainsKey(_instance.HostName))
            {
                return _instance.Himagelist[_instance.HostName];
            }
            return GetRunNameVision();
        }


        [Browsable(false)]
        ///<summary>
        /// 运行相机参数
        /// </summary>
        public Dictionary<string, DahuaCamera> RunCams { get; set; } = new Dictionary<string, DahuaCamera>();


        [Browsable(false)]
        ///<summary>
        /// 3D标定
        /// </summary>
        public Dictionary<string, Calib.AutoCalibPoint> DicCalib3D { get; set; } = new Dictionary<string, Calib.AutoCalibPoint>();
        [Browsable(false)]
        public List<string> ListHalconName { get; set; } = new List<string>();
        [Browsable(false)]
        /// <summary>
        /// 空间键值
        /// </summary>
        public Dictionary<string, Coordinate> DicCoordinate { get; set; } = new Dictionary<string, Coordinate>();



        ///// <summary>
        ///// 模板
        ///// </summary>
        //public Dictionary<string, ModelVision> Model { get; set; } = new Dictionary<string, ModelVision>();

        /// <summary>
        /// 图像处理集合
        /// </summary>
        Dictionary<string, HalconRun> Himagelist = new Dictionary<string, HalconRun>();
        /// <summary>
        /// 获取指定名称的程序集合
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HalconRun GetRunNameVision(string name = null)
        {
            if (name == null)
            {
                return GetRunNameVision(_instance.RunNameVision);
            }

            if (_instance.Himagelist.ContainsKey(name))
            {
                return _instance.Himagelist[name];
            }
            foreach (var item in _instance.Himagelist)
            {
                return item.Value;
            }
            return null;
        }

        /// <summary>
        /// 获取指定名称的程序集合
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ICamera GetNameCam(string name)
        {

            if (_instance.RunCams.ContainsKey(name))
            {
                return _instance.RunCams[name];
            }
            return null;
        }


        public void AddROBj(string name, HalconResult result, int runid = -0)
        {
            try
            {

                if (runid==0)
                {
                    OneProductVale.PanelID = ProcessUser.QRCode;
                }
                if (runid < 0)
                {
                    OneProductVale.ResuOBj.Add(new OneResultOBj());
                }
                else
                {
                    //if (Detfet.keyValuePairs[name].Count > runid)
                    //{
                    //    Detfet.keyValuePairs[name][runid] = result;
                    //}
                    //else
                    //{
                    //    Detfet.keyValuePairs[name].Add(result);
                    //}
                }

            }
            catch (Exception ex)
            {
                ErrLog(ex);
            }
        }

        public static void ShowVisionResetForm(HalconRun  halcon=null)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    if (halcon!=null)
                    {
                        RestObjImage.RestObjImageFrom.ShowImage(DebugCompiler.GetThis().DDAxis.GetTrayInxt(halcon.TrayID));
                    }
                    else
                    {
                        RestObjImage.RestObjImageFrom.ShowImage(Vision.TraydataVale);
                    }
                    
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public static DataVale GetObj()
        {
            return  OneProductVale;
        }
        /// <summary>
        /// 清楚单个产品视觉数据
        /// </summary>
        public void OneProductValeClert()
        {
            OneProductVale = new DataVale();
        }
        /// <summary>
        /// 清楚视觉所有数据
        /// </summary>
        public void HObjCler()
        {
            foreach (var item in this.Himagelist)
            {
                item.Value.ListObjCler();
            }
            OneProductValeClert();
            TraydataVale.Clear(); 
        }

        public static DataVale OneProductVale = new DataVale();

        public static TrayRobot TraydataVale = new TrayRobot();
        ///// <summary>
        ///// 产品数据
        ///// </summary>
        //DataVale Detfet;
        public Dictionary<string, string> ListLibrary{ get; set; } = new Dictionary<string, string>();


        /// <summary>
        /// 独立显示复判
        /// </summary>
        [DescriptionAttribute("独立显示复判窗口。"), Category("复判模式"), DisplayName("启用复判")]
        public bool IsShowImage { get; set; }
        /// <summary>
        /// 活动图像保存信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SaveImageInfo GetSaveImageInfo(string name)
        {
            if (Instance.DicSaveType.ContainsKey(name))
            {
                return Instance.DicSaveType[name];
            }
            return null;
        }
        public static Dictionary<string, HalconRun> GetHimageList()
        {
            return Instance.Himagelist;
        }
        #endregion //定义变量
        #region Halcon视觉常用算子
        public static void AddRunNames(params string[] names)
        {
            Vision.Instance.VisionPr.Clear();
            for (int i = 0; i < names.Length; i++)
            {
                bool flag = !Vision.Instance.VisionPr.ContainsKey(names[i]);
                if (flag)
                {
                    Vision.Instance.VisionPr.Add(names[i], true);
                }
            }
        }

        public static bool FindRunName(string name)
        {
            try
            {
                bool flag = Vision.Instance.VisionPr.ContainsKey(name);
                if (flag)
                {
                    return Vision.Instance.VisionPr[name];
                }
            }
            catch (Exception)
            {
            }
            return false;
        }


        /// <summary>
        /// 计算图像清晰度
        /// </summary>
        /// <param name="hObjectImage">图像</param>
        /// <param name="method">计算方法</param>
        /// <returns>清晰度</returns>
        public static double Evaluate_definition(HObject Image, string method)
        {
            HTuple value = new HTuple();
            HTuple Deviation = new HTuple();
            HObject hObjectImage;
            try
            {
                HOperatorSet.ScaleImageMax(Image, out hObjectImage);
                HOperatorSet.GetImageSize(hObjectImage, out HTuple width, out HTuple height);
                if (method == "Deviation")
                {
                    HOperatorSet.RegionToMean(hObjectImage, hObjectImage, out HObject ImageMean);
                    HOperatorSet.ConvertImageType(ImageMean, out ImageMean, "real");
                    HOperatorSet.ConvertImageType(hObjectImage, out hObjectImage, "real");
                    HOperatorSet.SubImage(hObjectImage, ImageMean, out HObject imageSub, 1, 0);
                    HOperatorSet.MultImage(imageSub, imageSub, out HObject imageResult, 1, 0);
                    HOperatorSet.Intensity(imageResult, imageResult, out value, out Deviation);
                    imageResult.Dispose();
                    ImageMean.Dispose();
                    imageSub.Dispose();
                }//方差法
                else if (method == "laplace")
                {
                    HOperatorSet.Laplace(hObjectImage, out HObject imageLaplace4, "signed", 3, "n_4");
                    HOperatorSet.Laplace(hObjectImage, out HObject imageLaplace8, "signed", 3, "n_8");
                    HOperatorSet.AddImage(imageLaplace4, imageLaplace4, out HObject imageResulit1, 1, 0);
                    HOperatorSet.AddImage(imageLaplace8, imageResulit1, out imageResulit1, 1, 0);
                    HOperatorSet.MultImage(imageResulit1, imageResulit1, out HObject imageResulit, 1, 0);
                    HOperatorSet.Intensity(imageResulit, imageResulit, out value, out Deviation);
                    imageResulit.Dispose();
                    imageResulit1.Dispose();
                    imageLaplace8.Dispose();
                    imageLaplace4.Dispose();
                }  //*拉普拉斯能量函数
                else if (method == "energy")
                {
                    HOperatorSet.CropPart(hObjectImage, out HObject imagePart0, 0, 0, width - 1, height - 1);
                    HOperatorSet.CropPart(hObjectImage, out HObject imagePart1, 0, 1, width - 1, height - 1);
                    HOperatorSet.CropPart(hObjectImage, out HObject imagePart10, 1, 0, width - 1, height - 1);
                    HOperatorSet.ConvertImageType(imagePart0, out imagePart0, "real");
                    HOperatorSet.ConvertImageType(imagePart1, out imagePart1, "real");
                    HOperatorSet.ConvertImageType(imagePart10, out imagePart10, "real");
                    HOperatorSet.SubImage(imagePart10, imagePart0, out HObject imagesub1, 1, 0);
                    HOperatorSet.MultImage(imagesub1, imagesub1, out HObject imageResult1, 1, 0);
                    HOperatorSet.SubImage(imagePart1, imagePart0, out HObject imageSub2, 1, 0);
                    HOperatorSet.MultImage(imageSub2, imageSub2, out HObject imageResult2, 1, 0);
                    HOperatorSet.AddImage(imageResult1, imageResult2, out HObject imageResult, 1, 0);
                    HOperatorSet.Intensity(imageResult, imageResult, out value, out Deviation);
                    imagesub1.Dispose();
                    imagePart0.Dispose();
                    imagePart1.Dispose();
                    imagePart10.Dispose();
                    imageResult1.Dispose();
                    imageResult2.Dispose();
                    imageResult.Dispose();
                }       //能量梯度函数
                else if (method == "Brenner")
                {
                    HOperatorSet.CropPart(hObjectImage, out HObject ImagePart00, 0, 0, width, height - 2);
                    HOperatorSet.ConvertImageType(ImagePart00, out ImagePart00, "real");
                    HOperatorSet.CropPart(hObjectImage, out HObject ImagePart20, 2, 0, width, height - 2);
                    HOperatorSet.ConvertImageType(ImagePart20, out ImagePart20, "real");
                    HOperatorSet.SubImage(ImagePart20, ImagePart00, out HObject ImageSub, 1, 0);
                    HOperatorSet.MultImage(ImageSub, ImageSub, out HObject ImageResult, 1, 0);
                    HOperatorSet.Intensity(ImageResult, ImageResult, out value, out Deviation);
                    ImageResult.Dispose();
                    ImagePart20.Dispose();
                    ImagePart00.Dispose();
                    ImageSub.Dispose();
                }//Brenner函数法
                else if (method == "Tenegrad")
                {
                    HOperatorSet.SobelAmp(hObjectImage, out HObject EdgeAmplitude, "sum_sqrt", 3);
                    HOperatorSet.MinMaxGray(EdgeAmplitude, EdgeAmplitude, 0, out HTuple min, out HTuple max, out HTuple Range);
                    HOperatorSet.Threshold(EdgeAmplitude, out HObject Region1, 11.8, 255);
                    HOperatorSet.RegionToBin(Region1, out HObject binImage, 1, 0, width, height);
                    HOperatorSet.MultImage(EdgeAmplitude, binImage, out HObject ImageResult4, 1, 0);
                    HOperatorSet.MultImage(ImageResult4, ImageResult4, out HObject imageResult, 1, 0);
                    HOperatorSet.Intensity(imageResult, imageResult, out value, out Deviation);
                    imageResult.Dispose();
                    binImage.Dispose();
                    EdgeAmplitude.Dispose();
                    Region1.Dispose();
                }//*Tenegrad函数法
                else
                {
                    MessageBox.Show("输入参数method错误,不支持的分析方法" + method + "请输入正确的方法：Deviation，laplace，energy，Brenner，Tenegrad");
                }
                hObjectImage.Dispose();
            }
            catch (Exception ex)
            {
                ErrLog("获取清晰度", ex);
            }
            return value.D;
        }

        /// <summary>
        /// 计算清晰度
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public static double Evaluate_definition(HObject Image)
        {
            return Evaluate_definition(Image, "Brenner");
        }

        /// <summary>
        /// 对指定的链接触发器写参数
        /// </summary>
        /// <param name="linkID"></param>
        /// <param name="triggerName"></param>
        /// <param name="data"></param>
        public static bool TriggerSetup(string linkID, string triggerName, string data)
        {
            try
            {
                if (triggerName == "" || data == "")
                {
                    return false;
                }


                if (triggerName.Contains("."))
                {
                    return StaticCon.SetLingkValue(triggerName, data, out string err);
                }
                else
                {
                    if (linkID == "")
                    {
                        return false;
                    }
                    if (StaticCon.SocketClint[linkID].KeysValues.ContainsKey(triggerName))
                    {
                        return StaticCon.SocketClint[linkID].SetValue(triggerName, data, out string err);
                    }
                    else
                    {
                        return StaticCon.SocketClint[linkID].SetIDValue(triggerName, data, out string err);
                    }

                }
            }
            catch (Exception)
            {

            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="triggerName"></param>
        /// <param name="data"></param>
        public static void TriggerSetup(string triggerName, string data)
        {
            try
            {
                if (triggerName == null || triggerName == "")
                {
                    return;
                }
                if (triggerName.Contains("."))
                {
                    string[] dat = triggerName.Split('.');
                    TriggerSetup(dat[0], triggerName.Remove(0, dat[0].Length + 1), data);
                }
                else if (int.TryParse(triggerName, out int result))
                {
                    if (DebugCompiler.GetDoDi() != null)
                    {
                        DebugCompiler.GetDoDi().WritDO(result, bool.Parse(data));
                    }

                }
                else
                {
                    if (StaticCon.SocketClint.ContainsKey(triggerName))
                    {
                        StaticCon.SocketClint[triggerName].Send(data);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static void Disp_message(HTuple hv_WindowHandle, HTuple hv_String,
          int hv_Row = 20, int hv_Column = 20)
        {
            Disp_message(hv_WindowHandle, hv_String, hv_Row, hv_Column);
        }
        /// <summary>
        /// 在窗口显示文本
        /// </summary>
        /// <param name="hv_WindowHandle"></param>
        /// <param name="hv_String">文本</param>
        /// <param name="hv_Row">行</param>
        /// <param name="hv_Column">列</param>
        /// <param name="hv_CoordSystem">true窗口,或图像</param>
        /// <param name="hv_Color"></param>
        /// <param name="hv_Box"></param>
        public static void Disp_message(HTuple hv_WindowHandle, HTuple hv_String,
          HTuple hv_Row, HTuple hv_Column, bool hv_CoordSystem = false, string hv_Color = "red", string hv_Box = "false")
        {
            // Local control variables
            if (hv_Box == null)
            {
                hv_Box = "";
            }
            if (hv_Color == null)
            {
                hv_Color = "yellow";
            }
            if (hv_Column == null)
            {
                hv_Column = 20;
            }
            if (hv_Row == null)
            {
                hv_Row = 20;
            }
            HTuple hv_Red, hv_Green, hv_Blue, hv_Row1Part;
            HTuple hv_Column1Part, hv_Row2Part, hv_Column2Part, hv_RowWin;
            HTuple hv_ColumnWin, hv_WidthWin, hv_HeightWin, hv_MaxAscent;
            HTuple hv_MaxDescent, hv_MaxWidth, hv_MaxHeight, hv_R1 = new HTuple();
            HTuple hv_C1 = new HTuple(), hv_FactorRow = new HTuple(), hv_FactorColumn = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple(), hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple(), hv_W = new HTuple(), hv_H = new HTuple();
            HTuple hv_FrameHeight = new HTuple(), hv_FrameWidth = new HTuple();
            HTuple hv_R2 = new HTuple(), hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_CurrentColor = new HTuple();


            HTuple hv_Column_COPY_INP_TMP = hv_Column;
            HTuple hv_Row_COPY_INP_TMP = hv_Row;
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part,
                out hv_Column2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            //
            //default settings
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }

            //
            hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
            try
            {
                //Estimate extentions of text depending on font size.
                HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                   out hv_MaxWidth, out hv_MaxHeight);
                if (hv_CoordSystem)
                {
                    hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                    hv_C1 = hv_Column_COPY_INP_TMP.Clone();
                }
                else
                {
                    //transform image to window coordinates
                    hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                    hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                    hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                    hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
                }
                //
                //display text box depending on text size
                if (hv_Box == "true")
                {
                    //calculate box extents
                    hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                    hv_Width = new HTuple();
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                            hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                        hv_Width = hv_Width.TupleConcat(hv_W);
                    }
                    if (hv_CoordSystem)
                    {
                        hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                  ));
                    }
                    else
                    {
                        hv_FrameHeight = hv_MaxHeight;
                    }

                    hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                    hv_R2 = hv_R1 + hv_FrameHeight;
                    hv_C2 = hv_C1 + hv_FrameWidth;
                    //display rectangles
                    HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                    HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                    HOperatorSet.SetColor(hv_WindowHandle, "light gray");
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 3, hv_C1 + 3, hv_R2 + 3, hv_C2 + 3);
                    HOperatorSet.SetColor(hv_WindowHandle, "white");
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                    HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
                }
                else
                {
                }

                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
           )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    hv_CurrentColor = hv_Color;
                    if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                        "auto")))) != 0)
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                    }
                    else
                    {
                        HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                    }
                    hv_Row_COPY_INP_TMP = hv_R1[0] + (hv_MaxHeight * hv_Index);
                    HTuple hTuple = hv_C1[0];
                    if (hv_R1.Length > 1)
                    {
                        if (hv_Index < hv_R1.Length)
                        {
                            hv_Row_COPY_INP_TMP = hv_R1[hv_Index];
                            hTuple = hv_C1[hv_Index];
                        }
                    }
                    try
                    {
                        HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP.TupleInt(), hTuple.TupleInt());
                        //HOperatorSet.DispText(hv_WindowHandle, hv_String_COPY_INP_TMP[hv_Index], "image", hv_Row_COPY_INP_TMP, hTuple, hv_CurrentColor, "box", "false");
                        HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP[hv_Index]);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                //HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                    hv_Column2Part);

                return;
            }
            catch (Exception ex)
            {

            }
        }
        public static HObject Reduce_domain(HObject hObject, HObject readm)
        {
            try
            {
                HOperatorSet.ReduceDomain(hObject, readm, out HObject ImageReduced);
                return ImageReduced;
            }
            catch (Exception)
            {


            }
            return null;
        }

        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="phi"></param>
        /// <param name="lieng"></param>
        /// <returns></returns>
        public static HObject GenLine(double row, double column, double phi, double lieng)
        {
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            try
            {
                HTuple rows = new HTuple(-1);
                rows.Append(-1);
                HTuple cols = new HTuple(0);
                cols.Append(lieng * 2);
                HOperatorSet.GenContourPolygonXld(out hObject, rows, cols);
                HOperatorSet.VectorAngleToRigid(0, lieng, 0, row, column, phi, out HTuple HomMat2D);
                HOperatorSet.AffineTransContourXld(hObject, out hObject, HomMat2D);
            }
            catch (Exception)
            {
            }
            return hObject;
        }

        /// <summary>
        /// 绘制箭头
        /// </summary>
        /// <param name="ho_Arrow">轮廓组</param>
        /// <param name="hv_Row1"></param>
        /// <param name="hv_Column1"></param>
        /// <param name="hv_Row2"></param>
        /// <param name="hv_Column2"></param>
        /// <param name="hv_HeadLength">长</param>
        /// <param name="hv_HeadWidth">高</param>
        public static void Gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
            HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {
            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;
            // Local iconic variables

            HObject ho_TempArrow = null;

            // Local control variables

            HTuple hv_Length, hv_ZeroLengthIndices, hv_DR;
            HTuple hv_DC, hv_HalfHeadWidth, hv_RowP1, hv_ColP1, hv_RowP2;
            HTuple hv_ColP2, hv_Index;

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);

            try
            {

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
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(
                            hv_Index), hv_Column1.TupleSelect(hv_Index));
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
                    OTemp[SP_O] = ho_Arrow.CopyObj(1, -1);
                    SP_O++;
                    ho_Arrow.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_TempArrow, out ho_Arrow);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                }
                ho_TempArrow.Dispose();

                return;
            }
            catch (HalconException)
            {
                ho_TempArrow.Dispose();

                //throw HDevExpDefaultException;
            }
        }
        /// <summary>
        /// 绘制箭头
        /// </summary>
        /// <param name="ho_Arrow"></param>
        /// <param name="hv_Row1"></param>
        /// <param name="hv_Column1"></param>
        /// <param name="hv_Row2"></param>
        /// <param name="hv_Column2"></param>
        public static void Gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
    HTuple hv_Row2, HTuple hv_Column2)
        {
            HTuple hv_HeadLength = 20;
            HTuple hv_HeadWidth = 2;

            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;
            // Local iconic variables

            HObject ho_TempArrow = null;

            // Local control variables

            HTuple hv_Length, hv_ZeroLengthIndices, hv_DR;
            HTuple hv_DC, hv_HalfHeadWidth, hv_RowP1, hv_ColP1, hv_RowP2;
            HTuple hv_ColP2, hv_Index;

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);

            try
            {

                ho_Arrow.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Arrow);
                //
                //Calculate the arrow length
                HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);

                hv_HeadLength = hv_Length / 10 + 10;
                hv_HeadWidth = hv_Length / 30 + 2;
                //
                //Mark arrows with identical start and end point
                //(set Length to -1 to avoid division-by-zero exception)
                hv_ZeroLengthIndices = hv_Length.TupleFind(0);
                if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
                {
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
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(
                            hv_Index), hv_Column1.TupleSelect(hv_Index));
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
                    OTemp[SP_O] = ho_Arrow.CopyObj(1, -1);
                    SP_O++;
                    ho_Arrow.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_TempArrow, out ho_Arrow);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                }
                ho_TempArrow.Dispose();

                return;
            }
            catch (HalconException)
            {
                ho_TempArrow.Dispose();

                //throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 画出直线检测边缘区域
        /// </summary>
        /// <param name="ho_Regions">区域</param>
        /// <param name="hv_WindowHandle">窗口</param>
        /// <param name="hv_Elements">边缘点数</param>
        /// <param name="hv_DetectHeight">卡尺高度</param>
        /// <param name="hv_DetectWidth">卡尺宽度</param>
        /// <param name="hv_Row1">起点y</param>
        /// <param name="hv_Column1">起点x</param>
        /// <param name="hv_Row2">终点y</param>
        /// <param name="hv_Column2">终点x</param>
        public static void Draw_rake(out HObject ho_Regions, HTuple hv_WindowHandle, HTuple hv_Elements,
            HTuple hv_DetectHeight, HTuple hv_DetectWidth, out HTuple hv_Row1, out HTuple hv_Column1,
            out HTuple hv_Row2, out HTuple hv_Column2)
        {
            hv_Column2 = hv_Row1 = hv_Row2 = hv_Column1 = 0;

            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;

            // Local iconic variables

            HObject ho_RegionLines, ho_Rectangle = null;
            HObject ho_Arrow1 = null;

            // Local control variables

            HTuple hv_ATan, hv_i, hv_RowC = new HTuple();
            HTuple hv_ColC = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_RowL2 = new HTuple(), hv_RowL1 = new HTuple(), hv_ColL2 = new HTuple();
            HTuple hv_ColL1 = new HTuple();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);

            try
            {
                //提示
                Disp_message(hv_WindowHandle, "点击鼠标左键画一条直线,点击右键确认",
                    12, 12, true);
                //产生一个空显示对象，用于显示
                ho_Regions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Regions);
                //画矢量检测直线
                HOperatorSet.DrawLine(hv_WindowHandle, out hv_Row1, out hv_Column1, out hv_Row2,
                    out hv_Column2);
                //产生直线xld
                ho_RegionLines.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_RegionLines, hv_Row1.TupleConcat(hv_Row2),
                    hv_Column1.TupleConcat(hv_Column2));
                //存储到显示对象
                OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                SP_O++;
                ho_Regions.Dispose();
                HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_RegionLines, out ho_Regions);
                OTemp[SP_O - 1].Dispose();
                SP_O = 0;
                //计算直线与x轴的夹角，逆时针方向为正向。
                HOperatorSet.AngleLx(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_ATan);

                //边缘检测方向垂直于检测直线：直线方向正向旋转90°为边缘检测方向
                hv_ATan = hv_ATan + ((new HTuple(90)).TupleRad());

                //根据检测直线按顺序产生测量区域矩形，并存储到显示对象
                for (hv_i = 1; hv_i.Continue(hv_Elements, 1); hv_i = hv_i.TupleAdd(1))
                {
                    //如果只有一个测量矩形，作为卡尺工具，宽度为检测直线的长度
                    if ((int)(new HTuple(hv_Elements.TupleEqual(1))) != 0)
                    {
                        hv_RowC = (hv_Row1 + hv_Row2) * 0.5;
                        hv_ColC = (hv_Column1 + hv_Column2) * 0.5;
                        HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Distance);
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_Distance / 2);
                    }
                    else
                    {
                        //如果有多个测量矩形，产生该测量矩形xld
                        hv_RowC = hv_Row1 + (((hv_Row2 - hv_Row1) * (hv_i - 1)) / (hv_Elements - 1));
                        hv_ColC = hv_Column1 + (((hv_Column2 - hv_Column1) * (hv_i - 1)) / (hv_Elements - 1));
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_DetectWidth / 2);
                    }
                    //把测量矩形xld存储到显示对象
                    OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                    SP_O++;
                    ho_Regions.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Rectangle, out ho_Regions);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                    if ((int)(new HTuple(hv_i.TupleEqual(1))) != 0)
                    {
                        //在第一个测量矩形绘制一个箭头xld，用于只是边缘检测方向
                        hv_RowL2 = hv_RowC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_RowL1 = hv_RowC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_ColL2 = hv_ColC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        hv_ColL1 = hv_ColC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        ho_Arrow1.Dispose();
                        Gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2,
                            25, 25);
                        //把xld存储到显示对象
                        OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                        SP_O++;
                        ho_Regions.Dispose();
                        HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Arrow1, out ho_Regions);
                        OTemp[SP_O - 1].Dispose();
                        SP_O = 0;
                    }
                }

                ho_RegionLines.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow1.Dispose();

                return;
            }
            catch (HalconException)
            {
                ho_RegionLines.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow1.Dispose();
            }
        }

        /// <summary>
        /// 画出圆检测边缘区域
        /// </summary>
        /// <param name="ho_Image">输入图像</param>
        /// <param name="ho_Regions">区域</param>
        /// <param name="hv_WindowHandle">窗口句柄</param>
        /// <param name="hv_Elements">边缘点数</param>
        /// <param name="hv_DetectHeight">卡尺高度</param>
        /// <param name="hv_DetectWidth">卡尺宽度</param>
        /// <param name="hv_ROIRows">spoke工具ROI的y数组</param>
        /// <param name="hv_ROICols">spoke工具ROI的X数组</param>
        /// <param name="hv_Direct">'inner'表示检测方向由边缘点指向圆心； 'outer'表示检测方向由圆心指向边缘点</param>
        public static void Daw_spoke(HObject ho_Image, out HObject ho_Regions, HTuple hv_WindowHandle,
            HTuple hv_Elements, HTuple hv_DetectHeight, HTuple hv_DetectWidth, out HTuple hv_ROIRows,
            out HTuple hv_ROICols, out HTuple hv_Direct)
        {
            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;

            // Local iconic variables

            HObject ho_ContOut1, ho_Contour, ho_ContCircle;
            HObject ho_Cross, ho_Rectangle1 = null, ho_Arrow1 = null;

            // Local control variables

            HTuple hv_Rows, hv_Cols, hv_Weights, hv_Length1;
            HTuple hv_RowC, hv_ColumnC, hv_Radius, hv_StartPhi, hv_EndPhi;
            HTuple hv_PointOrder, hv_RowXLD, hv_ColXLD, hv_Row1, hv_Column1;
            HTuple hv_Row2, hv_Column2, hv_DistanceStart, hv_DistanceEnd;
            HTuple hv_Length2, hv_i, hv_j = new HTuple(), hv_RowE = new HTuple();
            HTuple hv_ColE = new HTuple(), hv_ATan = new HTuple(), hv_RowL2 = new HTuple();
            HTuple hv_RowL1 = new HTuple(), hv_ColL2 = new HTuple(), hv_ColL1 = new HTuple();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_ContOut1);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_ContCircle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);

            hv_ROIRows = new HTuple();
            hv_ROICols = new HTuple();
            hv_Direct = new HTuple();
            try
            {
                //提示
                //Disp_message(hv_WindowHandle, "1、画4个以上点确定一个圆弧,点击右键确认", "window",
                //    12, 12, "red", "false");
                //产生一个空显示对象，用于显示
                ho_Regions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Regions);
                //沿着圆弧或圆的边缘画点
                ho_ContOut1.Dispose();
                HOperatorSet.DrawNurbs(out ho_ContOut1, hv_WindowHandle, "true", "true", "true",
                    "true", 3, out hv_Rows, out hv_Cols, out hv_Weights);
                //至少要4个点
                HOperatorSet.TupleLength(hv_Weights, out hv_Length1);
                if ((int)(new HTuple(hv_Length1.TupleLess(4))) != 0)
                {
                    Disp_message(hv_WindowHandle, "提示：点数太少，请重画", 32, 12, true);
                    ho_ContOut1.Dispose();
                    ho_Contour.Dispose();
                    ho_ContCircle.Dispose();
                    ho_Cross.Dispose();
                    ho_Rectangle1.Dispose();
                    ho_Arrow1.Dispose();

                    return;
                }
                //获取点
                hv_ROIRows = hv_Rows.Clone();
                hv_ROICols = hv_Cols.Clone();
                //产生xld
                ho_Contour.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_ROIRows, hv_ROICols);
                //用回归线法（不抛出异常点，所有点权重一样）拟合圆
                HOperatorSet.FitCircleContourXld(ho_Contour, "algebraic", -1, 0, 0, 3, 2, out hv_RowC,
                    out hv_ColumnC, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                //根据拟合结果产生xld，并保持到显示对象
                ho_ContCircle.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_ContCircle, hv_RowC, hv_ColumnC, hv_Radius,
                    hv_StartPhi, hv_EndPhi, hv_PointOrder, 3);
                OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                SP_O++;
                ho_Regions.Dispose();
                HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_ContCircle, out ho_Regions);
                OTemp[SP_O - 1].Dispose();
                SP_O = 0;

                //获取圆或圆弧xld上的点坐标
                HOperatorSet.GetContourXld(ho_ContCircle, out hv_RowXLD, out hv_ColXLD);
                //显示图像和圆弧
                if (HDevWindowStack.IsOpen())
                {
                    //HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
                }
                if (HDevWindowStack.IsOpen())
                {
                    //HOperatorSet.DispObj(ho_ContCircle, HDevWindowStack.GetActive());
                }
                //产生并显示圆心
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_RowC, hv_ColumnC, 60, 0.785398);
                if (HDevWindowStack.IsOpen())
                {
                    //HOperatorSet.DispObj(ho_Cross, HDevWindowStack.GetActive());
                }
                //提示
                //Disp_message(hv_WindowHandle, "2、远离圆心，画箭头确定边缘检测方向，点击右键确认",
                //    "window", 32, 12, "red", "false");
                //画线，确定检测方向
                HOperatorSet.DrawLine(hv_WindowHandle, out hv_Row1, out hv_Column1, out hv_Row2,
                    out hv_Column2);
                //求圆心到检测方向直线起点的距离
                HOperatorSet.DistancePp(hv_RowC, hv_ColumnC, hv_Row1, hv_Column1, out hv_DistanceStart);
                //求圆心到检测方向直线终点的距离
                HOperatorSet.DistancePp(hv_RowC, hv_ColumnC, hv_Row2, hv_Column2, out hv_DistanceEnd);

                //求圆或圆弧xld上的点的数量
                HOperatorSet.TupleLength(hv_ColXLD, out hv_Length2);
                //判断检测的边缘数量是否过少
                if ((int)(new HTuple(hv_Elements.TupleLess(3))) != 0)
                {
                    //hv_ROIRows = new HTuple();
                    //hv_ROICols = new HTuple();
                    Disp_message(hv_WindowHandle, "检测的边缘数量太少，请重新设置!",
                        52, 12, true);
                    ho_ContOut1.Dispose();
                    ho_Contour.Dispose();
                    ho_ContCircle.Dispose();
                    ho_Cross.Dispose();
                    ho_Rectangle1.Dispose();
                    ho_Arrow1.Dispose();

                    return;
                }
                //如果xld是圆弧，有Length2个点，从起点开始，等间距（间距为Length2/(Elements-1)）取Elements个点，作为卡尺工具的中点
                //如果xld是圆，有Length2个点，以0°为起点，从起点开始，等间距（间距为Length2/(Elements)）取Elements个点，作为卡尺工具的中点
                for (hv_i = 0; hv_i.Continue(hv_Elements - 1, 1); hv_i = hv_i.TupleAdd(1))
                {
                    if ((int)(new HTuple(((hv_RowXLD.TupleSelect(0))).TupleEqual(hv_RowXLD.TupleSelect(
                        hv_Length2 - 1)))) != 0)
                    {
                        //xld的起点和终点坐标相对，为圆
                        HOperatorSet.TupleInt(((1.0 * hv_Length2) / hv_Elements) * hv_i, out hv_j);
                    }
                    else
                    {
                        //否则为圆弧
                        HOperatorSet.TupleInt(((1.0 * hv_Length2) / (hv_Elements - 1)) * hv_i, out hv_j);
                    }
                    //索引越界，强制赋值为最后一个索引
                    if ((int)(new HTuple(hv_j.TupleGreaterEqual(hv_Length2))) != 0)
                    {
                        hv_j = hv_Length2 - 1;
                        //continue
                    }
                    //获取卡尺工具中心
                    hv_RowE = hv_RowXLD.TupleSelect(hv_j);
                    hv_ColE = hv_ColXLD.TupleSelect(hv_j);

                    //如果圆心到检测方向直线的起点的距离大于圆心到检测方向直线的终点的距离，搜索方向由圆外指向圆心
                    //如果圆心到检测方向直线的起点的距离不大于圆心到检测方向直线的终点的距离，搜索方向由圆心指向圆外
                    if ((int)(new HTuple(hv_DistanceStart.TupleGreater(hv_DistanceEnd))) != 0)
                    {
                        //求卡尺工具的边缘搜索方向
                        //求圆心指向边缘的矢量的角度
                        HOperatorSet.TupleAtan2((-hv_RowE) + hv_RowC, hv_ColE - hv_ColumnC, out hv_ATan);
                        //角度反向
                        hv_ATan = ((new HTuple(180)).TupleRad()) + hv_ATan;
                        //边缘搜索方向类型：'inner'搜索方向由圆外指向圆心；'outer'搜索方向由圆心指向圆外
                        hv_Direct = "inner";
                    }
                    else
                    {
                        //求卡尺工具的边缘搜索方向
                        //求圆心指向边缘的矢量的角度
                        HOperatorSet.TupleAtan2((-hv_RowE) + hv_RowC, hv_ColE - hv_ColumnC, out hv_ATan);
                        //边缘搜索方向类型：'inner'搜索方向由圆外指向圆心；'outer'搜索方向由圆心指向圆外
                        hv_Direct = "outer";
                    }

                    //产生卡尺xld，并保持到显示对象
                    ho_Rectangle1.Dispose();
                    HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle1, hv_RowE, hv_ColE,
                        hv_ATan, hv_DetectHeight / 2, hv_DetectWidth / 2);
                    OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                    SP_O++;
                    ho_Regions.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Rectangle1, out ho_Regions);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;

                    //用箭头xld指示边缘搜索方向，并保持到显示对象
                    if ((int)(new HTuple(hv_i.TupleEqual(0))) != 0)
                    {
                        hv_RowL2 = hv_RowE + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_RowL1 = hv_RowE - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_ColL2 = hv_ColE + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        hv_ColL1 = hv_ColE - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        ho_Arrow1.Dispose();
                        Gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2,
                            25, 25);
                        OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                        SP_O++;
                        ho_Regions.Dispose();
                        HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Arrow1, out ho_Regions);
                        OTemp[SP_O - 1].Dispose();
                        SP_O = 0;
                    }
                }

                ho_ContOut1.Dispose();
                ho_Contour.Dispose();
                ho_ContCircle.Dispose();
                ho_Cross.Dispose();
                ho_Rectangle1.Dispose();
                ho_Arrow1.Dispose();

                return;
            }
            catch (HalconException ex)
            {
                ho_ContOut1.Dispose();
                ho_Contour.Dispose();
                ho_ContCircle.Dispose();
                ho_Cross.Dispose();
                ho_Rectangle1.Dispose();
                ho_Arrow1.Dispose();

                //throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 拟合圆
        /// </summary>
        /// <param name="ho_Circle">输出拟合圆的xld</param>
        /// <param name="hv_Rows">拟合圆的输入y数组</param>
        /// <param name="hv_Cols">拟合圆的输入x数组</param>
        /// <param name="hv_ActiveNum">最小有效点数</param>
        /// <param name="hv_ArcType">拟合圆弧类型：'arc'圆弧；'circle'圆</param>
        /// <param name="hv_RowCenter">拟合的圆中心y</param>
        /// <param name="hv_ColCenter">拟合的圆中心x</param>
        /// <param name="hv_Radius">拟合的圆半径</param>
        /// <param name="hv_StartPhi"></param>
        /// <param name="hv_EndPhi"></param>
        /// <param name="hv_PointOrder"></param>
        /// <param name="hv_ArcAngle"></param>
        public static void Pts_to_best_circle(out HObject ho_Circle, HTuple hv_Rows, HTuple hv_Cols,
            HTuple hv_ActiveNum, HTuple hv_ArcType, out HTuple hv_RowCenter, out HTuple hv_ColCenter,
            out HTuple hv_Radius, out HTuple hv_StartPhi, out HTuple hv_EndPhi, out HTuple hv_PointOrder,
            out HTuple hv_ArcAngle)
        {
            // Local iconic variables
            HObject ho_Contour = null;
            // Local control variables
            HTuple hv_Length, hv_Length1 = new HTuple();
            HTuple hv_CircleLength = new HTuple();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            //初始化
            hv_RowCenter = 0;
            hv_ColCenter = 0;
            hv_Radius = 0;
            hv_StartPhi = new HTuple();
            hv_EndPhi = new HTuple();
            hv_PointOrder = new HTuple();
            hv_ArcAngle = new HTuple();
            try
            {          //产生一个空的直线对象，用于保存拟合后的圆
                ho_Circle.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Circle);
                //计算边缘数量
                HOperatorSet.TupleLength(hv_Cols, out hv_Length);
                //当边缘数量不小于有效点数时进行拟合
                if ((int)((new HTuple(hv_Length.TupleGreaterEqual(hv_ActiveNum))).TupleAnd(
                    new HTuple(hv_ActiveNum.TupleGreater(2)))) != 0)
                {
                    //halcon的拟合是基于xld的，需要把边缘连接成xld
                    if ((int)(new HTuple(hv_ArcType.TupleEqual("circle"))) != 0)
                    {
                        //如果是闭合的圆，轮廓需要首尾相连
                        ho_Contour.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows.TupleConcat(hv_Rows.TupleSelect(
                            0)), hv_Cols.TupleConcat(hv_Cols.TupleSelect(0)));
                    }
                    else
                    {
                        ho_Contour.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows, hv_Cols);
                    }
                    //拟合圆。使用的算法是''geotukey''，其他算法请参考fit_circle_contour_xld的描述部分。
                    HOperatorSet.FitCircleContourXld(ho_Contour, "geotukey", -1, 0, 0, 3, 2,
                        out hv_RowCenter, out hv_ColCenter, out hv_Radius, out hv_StartPhi, out hv_EndPhi,
                        out hv_PointOrder);
                    //判断拟合结果是否有效：如果拟合成功，数组中元素的数量大于0
                    HOperatorSet.TupleLength(hv_StartPhi, out hv_Length1);
                    if ((int)(new HTuple(hv_Length1.TupleLess(1))) != 0)
                    {
                        ho_Contour.Dispose();

                        return;
                    }
                    //根据拟合结果，产生直线xld
                    if ((int)(new HTuple(hv_ArcType.TupleEqual("arc"))) != 0)
                    {
                        //判断圆弧的方向：顺时针还是逆时针
                        //halcon求圆弧会出现方向混乱的问题
                        //tuple_mean (Rows, RowsMean)
                        //tuple_mean (Cols, ColsMean)
                        //gen_cross_contour_xld (Cross, RowsMean, ColsMean, 6, 0.785398)
                        //gen_circle_contour_xld (Circle1, RowCenter, ColCenter, Radius, StartPhi, EndPhi, 'positive', 1)
                        //求轮廓1中心
                        //area_center_points_xld (Circle1, Area, Row1, Column1)
                        //gen_circle_contour_xld (Circle2, RowCenter, ColCenter, Radius, StartPhi, EndPhi, 'negative', 1)
                        //求轮廓2中心
                        //area_center_points_xld (Circle2, Area, Row2, Column2)
                        //distance_pp (RowsMean, ColsMean, Row1, Column1, Distance1)
                        //distance_pp (RowsMean, ColsMean, Row2, Column2, Distance2)
                        //ArcAngle := EndPhi-StartPhi
                        //if (Distance1<Distance2)

                        //PointOrder := 'positive'
                        //copy_obj (Circle1, Circle, 1, 1)
                        //else

                        //PointOrder := 'negative'
                        //if (abs(ArcAngle)>3.1415926)
                        //ArcAngle := ArcAngle-2.0*3.1415926
                        //endif
                        //copy_obj (Circle2, Circle, 1, 1)
                        //endif
                        ho_Circle.Dispose();
                        HOperatorSet.GenCircleContourXld(out ho_Circle, hv_RowCenter, hv_ColCenter,
                            hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder, 1);

                        HOperatorSet.LengthXld(ho_Circle, out hv_CircleLength);
                        hv_ArcAngle = hv_EndPhi - hv_StartPhi;
                        if ((int)(new HTuple(hv_CircleLength.TupleGreater(((new HTuple(180)).TupleRad()
                            ) * hv_Radius))) != 0)
                        {
                            if ((int)(new HTuple(((hv_ArcAngle.TupleAbs())).TupleLess((new HTuple(180)).TupleRad()
                                ))) != 0)
                            {
                                if ((int)(new HTuple(hv_ArcAngle.TupleGreater(0))) != 0)
                                {
                                    hv_ArcAngle = ((new HTuple(360)).TupleRad()) - hv_ArcAngle;
                                }
                                else
                                {
                                    hv_ArcAngle = ((new HTuple(360)).TupleRad()) + hv_ArcAngle;
                                }
                            }
                        }
                        else
                        {
                            if ((int)(new HTuple(hv_CircleLength.TupleLess(((new HTuple(180)).TupleRad()
                                ) * hv_Radius))) != 0)
                            {
                                if ((int)(new HTuple(((hv_ArcAngle.TupleAbs())).TupleGreater((new HTuple(180)).TupleRad()
                                    ))) != 0)
                                {
                                    if ((int)(new HTuple(hv_ArcAngle.TupleGreater(0))) != 0)
                                    {
                                        hv_ArcAngle = hv_ArcAngle - ((new HTuple(360)).TupleRad());
                                    }
                                    else
                                    {
                                        hv_ArcAngle = ((new HTuple(360)).TupleRad()) + hv_ArcAngle;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        hv_StartPhi = 0;
                        hv_EndPhi = (new HTuple(360)).TupleRad();
                        hv_ArcAngle = (new HTuple(360)).TupleRad();
                        ho_Circle.Dispose();
                        HOperatorSet.GenCircleContourXld(out ho_Circle, hv_RowCenter, hv_ColCenter,
                            hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder, 1);
                    }
                }

                ho_Contour.Dispose();

                return;
            }
            catch (HalconException)
            {
                ho_Contour.Dispose();

                //  throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 拟合直线参数
        /// </summary>
        /// <param name="ho_Line">直线XLD</param>
        /// <param name="hv_Rows">y数组</param>
        /// <param name="hv_Cols">x数组</param>
        /// <param name="hv_ActiveNum">有效点数</param>
        /// <param name="hv_Row1">直线起点y</param>
        /// <param name="hv_Column1">起点x</param>
        /// <param name="hv_Row2">终点y</param>
        /// <param name="hv_Column2">终点x</param>
        public static void Pts_to_best_line(out HObject ho_Line, HTuple hv_Rows, HTuple hv_Cols,
            HTuple hv_ActiveNum, out HTuple hv_Row1, out HTuple hv_Column1, out HTuple hv_Row2,
            out HTuple hv_Column2)
        {
            hv_Row1 = 0;
            hv_Column1 = 0;
            hv_Row2 = 0;
            hv_Column2 = 0;
            HObject ho_Contour = new HObject();
            //初始化
            HTuple hv_Length, hv_Nr = new HTuple(), hv_Nc = new HTuple();
            HTuple hv_Dist = new HTuple(), hv_Length1 = new HTuple();
            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            try
            {
                //产生一个空的直线对象，用于保存拟合后的直线
                //计算边缘数量
                HOperatorSet.TupleLength(hv_Cols, out hv_Length);
                //当边缘数量不小于有效点数时进行拟合
                if ((int)((new HTuple(hv_Length.TupleGreaterEqual(hv_ActiveNum))).TupleAnd(
                    new HTuple(hv_ActiveNum.TupleGreater(1)))) != 0)
                {
                    //halcon的拟合是基于xld的，需要把边缘连接成xld

                    HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows, hv_Cols);
                    //拟合直线。使用的算法是'tukey'，其他算法请参考fit_line_contour_xld的描述部分。
                    HOperatorSet.FitLineContourXld(ho_Contour, "tukey", -1, 0, 5, 2, out hv_Row1,
                        out hv_Column1, out hv_Row2, out hv_Column2, out hv_Nr, out hv_Nc, out hv_Dist);
                    //判断拟合结果是否有效：如果拟合成功，数组中元素的数量大于0
                    HOperatorSet.TupleLength(hv_Dist, out hv_Length1);
                    if ((int)(new HTuple(hv_Length1.TupleLess(1))) != 0)
                    {
                        ho_Contour.Dispose();
                        return;
                    }
                    //根据拟合结果，产生直线xld

                    HOperatorSet.GenContourPolygonXld(out ho_Line, hv_Row1.TupleConcat(hv_Row2),
                        hv_Column1.TupleConcat(hv_Column2));
                }

                return;
            }
            catch (HalconException)
            {
                ho_Line.Dispose();
                ho_Contour.Dispose();
            }
        }

        /// <summary>
        /// 直线拟合查找
        /// </summary>
        /// <param name="ho_Image">输入图像</param>
        /// <param name="ho_Regions">查找的区域ROL</param>
        /// <param name="hv_Elements">检测点数</param>
        /// <param name="hv_DetectHeight">卡尺高</param>
        /// <param name="hv_DetectWidth">卡尺宽</param>
        /// <param name="hv_Sigma">高斯滤波因子</param>
        /// <param name="hv_Threshold">边缘幅度阈值</param>
        /// <param name="hv_Transition">极性： positive表示由黑到白 negative表示由白到黑 all表示以上两种方向</param>
        /// <param name="hv_Select">first表示选择第一点 last表示选择最后一点 max表示选择边缘幅度最强点</param>
        /// <param name="hv_Row1">直线ROI起点的y值</param>
        /// <param name="hv_Column1">直线ROI起点的x值</param>
        /// <param name="hv_Row2"></param>
        /// <param name="hv_Column2"></param>
        /// <param name="hv_ResultRow">检测到的边缘的y坐标数组</param>
        /// <param name="hv_ResultColumn">检测到的边缘的x坐标数组</param>
        public static void Rake(HObject ho_Image, out HObject ho_Regions, HTuple hv_Elements,
            HTuple hv_DetectHeight, HTuple hv_DetectWidth, HTuple hv_Sigma, HTuple hv_Threshold,
            HTuple hv_Transition, HTuple hv_Select, HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2,
            HTuple hv_Column2, out HTuple hv_ResultRow, out HTuple hv_ResultColumn)
        {
            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;

            // Local iconic variables

            HObject ho_RegionLines, ho_Rectangle = null;
            HObject ho_Arrow1 = null;

            // Local control variables

            HTuple hv_Width, hv_Height, hv_ATan, hv_i;
            HTuple hv_RowC = new HTuple(), hv_ColC = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_RowL2 = new HTuple(), hv_RowL1 = new HTuple(), hv_ColL2 = new HTuple();
            HTuple hv_ColL1 = new HTuple(), hv_MsrHandle_Measure = new HTuple();
            HTuple hv_RowEdge = new HTuple(), hv_ColEdge = new HTuple();
            HTuple hv_Amplitude = new HTuple(), hv_tRow = new HTuple();
            HTuple hv_tCol = new HTuple(), hv_t = new HTuple(), hv_Number = new HTuple();
            HTuple hv_j = new HTuple();

            HTuple hv_DetectWidth_COPY_INP_TMP = hv_DetectWidth.Clone();
            HTuple hv_Select_COPY_INP_TMP = hv_Select.Clone();
            HTuple hv_Transition_COPY_INP_TMP = hv_Transition.Clone();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);
            //产生一个空显示对象，用于显示
            ho_Regions.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Regions);
            //初始化边缘坐标数组
            hv_ResultRow = new HTuple();
            hv_ResultColumn = new HTuple();

            try
            {
                //获取图像尺寸
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                //产生直线xld
                HOperatorSet.GenContourPolygonXld(out ho_RegionLines, hv_Row1.TupleConcat(hv_Row2),
                    hv_Column1.TupleConcat(hv_Column2));
                //存储到显示对象
                //OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                //SP_O++;

                //HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_RegionLines, out ho_Regions);
                //OTemp[SP_O - 1].Dispose();
                SP_O = 0;
                //计算直线与x轴的夹角，逆时针方向为正向。
                HOperatorSet.AngleLx(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_ATan);

                //边缘检测方向垂直于检测直线：直线方向正向旋转90°为边缘检测方向
                hv_ATan = hv_ATan + ((new HTuple(90)).TupleRad());

                //根据检测直线按顺序产生测量区域矩形，并存储到显示对象
                for (hv_i = 1; hv_i.Continue(hv_Elements, 1); hv_i = hv_i.TupleAdd(1))
                {
                    //RowC := Row1+(((Row2-Row1)*i)/(Elements+1))
                    //ColC := Column1+(Column2-Column1)*i/(Elements+1)
                    //if (RowC>Height-1 or RowC<0 or ColC>Width-1 or ColC<0)
                    //continue
                    //endif
                    //如果只有一个测量矩形，作为卡尺工具，宽度为检测直线的长度
                    if ((int)(new HTuple(hv_Elements.TupleEqual(1))) != 0)
                    {
                        hv_RowC = (hv_Row1 + hv_Row2) * 0.5;
                        hv_ColC = (hv_Column1 + hv_Column2) * 0.5;
                        //判断是否超出图像,超出不检测边缘
                        if ((int)((new HTuple((new HTuple((new HTuple(hv_RowC.TupleGreater(hv_Height - 1))).TupleOr(
                            new HTuple(hv_RowC.TupleLess(0))))).TupleOr(new HTuple(hv_ColC.TupleGreater(
                            hv_Width - 1))))).TupleOr(new HTuple(hv_ColC.TupleLess(0)))) != 0)
                        {
                            continue;
                        }
                        HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Distance);
                        hv_DetectWidth_COPY_INP_TMP = hv_Distance.Clone();
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_Distance / 2);
                    }
                    else
                    {
                        //如果有多个测量矩形，产生该测量矩形xld
                        hv_RowC = hv_Row1 + (((hv_Row2 - hv_Row1) * (hv_i - 1)) / (hv_Elements - 1));
                        hv_ColC = hv_Column1 + (((hv_Column2 - hv_Column1) * (hv_i - 1)) / (hv_Elements - 1));
                        //判断是否超出图像,超出不检测边缘
                        if ((int)((new HTuple((new HTuple((new HTuple(hv_RowC.TupleGreater(hv_Height - 1))).TupleOr(
                            new HTuple(hv_RowC.TupleLess(0))))).TupleOr(new HTuple(hv_ColC.TupleGreater(
                            hv_Width - 1))))).TupleOr(new HTuple(hv_ColC.TupleLess(0)))) != 0)
                        {
                            continue;
                        }
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_DetectWidth_COPY_INP_TMP / 2);
                    }

                    //把测量矩形xld存储到显示对象
                    OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                    SP_O++;
                    ho_Regions.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Rectangle, out ho_Regions);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                    if ((int)(new HTuple(hv_i.TupleEqual(1))) != 0)
                    {
                        //在第一个测量矩形绘制一个箭头xld，用于只是边缘检测方向
                        hv_RowL2 = hv_RowC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_RowL1 = hv_RowC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_ColL2 = hv_ColC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        hv_ColL1 = hv_ColC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        ho_Arrow1.Dispose();
                        Gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2,
                            25, 25);
                        //把xld存储到显示对象
                        OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                        SP_O++;
                        ho_Regions.Dispose();
                        HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Arrow1, out ho_Regions);
                        OTemp[SP_O - 1].Dispose();
                        SP_O = 0;
                    }
                    //产生测量对象句柄
                    HOperatorSet.GenMeasureRectangle2(hv_RowC, hv_ColC, hv_ATan, hv_DetectHeight / 2,
                        hv_DetectWidth_COPY_INP_TMP / 2, hv_Width, hv_Height, "nearest_neighbor",
                        out hv_MsrHandle_Measure);

                    //设置极性
                    if ((int)(new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("negative"))) != 0)
                    {
                        hv_Transition_COPY_INP_TMP = "negative";
                    }
                    else
                    {
                        if ((int)(new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("positive"))) != 0)
                        {
                            hv_Transition_COPY_INP_TMP = "positive";
                        }
                        else
                        {
                            hv_Transition_COPY_INP_TMP = "all";
                        }
                    }
                    //设置边缘位置。最强点是从所有边缘中选择幅度绝对值最大点，需要设置为'all'
                    if ((int)(new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("first"))) != 0)
                    {
                        hv_Select_COPY_INP_TMP = "first";
                    }
                    else
                    {
                        if ((int)(new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("last"))) != 0)
                        {
                            hv_Select_COPY_INP_TMP = "last";
                        }
                        else
                        {
                            hv_Select_COPY_INP_TMP = "all";
                        }
                    }
                    //检测边缘
                    HOperatorSet.MeasurePos(ho_Image, hv_MsrHandle_Measure, hv_Sigma, hv_Threshold,
                        hv_Transition_COPY_INP_TMP, hv_Select_COPY_INP_TMP, out hv_RowEdge, out hv_ColEdge,
                        out hv_Amplitude, out hv_Distance);
                    //清除测量对象句柄
                    HOperatorSet.CloseMeasure(hv_MsrHandle_Measure);

                    //临时变量初始化
                    //tRow，tCol保存找到指定边缘的坐标
                    hv_tRow = 0;
                    hv_tCol = 0;
                    //t保存边缘的幅度绝对值
                    hv_t = 0;
                    //找到的边缘必须至少为1个
                    HOperatorSet.TupleLength(hv_RowEdge, out hv_Number);
                    if ((int)(new HTuple(hv_Number.TupleLess(1))) != 0)
                    {
                        continue;
                    }
                    //有多个边缘时，选择幅度绝对值最大的边缘
                    for (hv_j = 0; hv_j.Continue(hv_Number - 1, 1); hv_j = hv_j.TupleAdd(1))
                    {
                        if ((int)(new HTuple(((((hv_Amplitude.TupleSelect(hv_j))).TupleAbs())).TupleGreater(
                            hv_t))) != 0)
                        {
                            hv_tRow = hv_RowEdge.TupleSelect(hv_j);
                            hv_tCol = hv_ColEdge.TupleSelect(hv_j);
                            hv_t = ((hv_Amplitude.TupleSelect(hv_j))).TupleAbs();
                        }
                    }
                    //把找到的边缘保存在输出数组
                    if ((int)(new HTuple(hv_t.TupleGreater(0))) != 0)
                    {
                        hv_ResultRow = hv_ResultRow.TupleConcat(hv_tRow);
                        hv_ResultColumn = hv_ResultColumn.TupleConcat(hv_tCol);
                    }
                }

                ho_RegionLines.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow1.Dispose();
                return;
            }
            catch (HalconException)
            {
                ho_RegionLines.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow1.Dispose();
            }
        }

        /// <summary>
        /// 圆查找拟合
        /// </summary>
        /// <param name="ho_Image">输入图像</param>
        /// <param name="ho_Regions">输出边缘点检测区域及方向</param>
        /// <param name="hv_Elements">检测边缘点数</param>
        /// <param name="hv_DetectHeight">卡尺工具的高度</param>
        /// <param name="hv_DetectWidth">卡尺工具的宽</param>
        /// <param name="hv_Sigma">高斯滤波因子</param>
        /// <param name="hv_Threshold">边缘检测阈值，又叫边缘强度</param>
        /// <param name="hv_Transition">极性： positive表示由黑到白 negative表示由白到黑 all表示以上两种方向</param>
        /// <param name="hv_Select">first表示选择第一点 last表示选择最后一点 max表示选择边缘强度最强点</param>
        /// <param name="hv_ROIRows">检测区域起点的y值</param>
        /// <param name="hv_ROICols">检测区域起点的x值</param>
        /// <param name="hv_Direct">'inner'表示检测方向由边缘点指向圆心; 'outer'表示检测方向由圆心指向边缘点</param>
        /// <param name="hv_ResultRow">检测到的边缘点的y坐标数组</param>
        /// <param name="hv_ResultColumn">检测到的边缘点的x坐标数组</param>
        /// <param name="hv_ArcType">拟合圆弧类型：'arc'圆弧；'circle'圆</param>
        public static void Spoke(HObject ho_Image, out HObject ho_Regions, HTuple hv_Elements,
            HTuple hv_DetectHeight, HTuple hv_DetectWidth, HTuple hv_Sigma, HTuple hv_Threshold,
            HTuple hv_Transition, HTuple hv_Select, HTuple hv_ROIRows, HTuple hv_ROICols,
            HTuple hv_Direct, out HTuple hv_ResultRow, out HTuple hv_ResultColumn, out HTuple hv_ArcType)
        {
            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;

            // Local iconic variables

            HObject ho_Contour, ho_ContCircle, ho_Rectangle1 = null;
            HObject ho_Arrow1 = null;

            // Local control variables

            HTuple hv_Width, hv_Height, hv_RowC, hv_ColumnC;
            HTuple hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder;
            HTuple hv_RowXLD, hv_ColXLD, hv_Length2, hv_WindowHandle = new HTuple();
            HTuple hv_i, hv_j = new HTuple(), hv_RowE = new HTuple(), hv_ColE = new HTuple();
            HTuple hv_ATan = new HTuple(), hv_RowL2 = new HTuple(), hv_RowL1 = new HTuple();
            HTuple hv_ColL2 = new HTuple(), hv_ColL1 = new HTuple(), hv_MsrHandle_Measure = new HTuple();
            HTuple hv_RowEdge = new HTuple(), hv_ColEdge = new HTuple();
            HTuple hv_Amplitude = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_tRow = new HTuple(), hv_tCol = new HTuple(), hv_t = new HTuple();
            HTuple hv_Number = new HTuple(), hv_k = new HTuple();

            HTuple hv_Select_COPY_INP_TMP = hv_Select.Clone();
            HTuple hv_Transition_COPY_INP_TMP = hv_Transition.Clone();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_ContCircle);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);
            //产生一个空显示对象，用于显示
            //初始化边缘坐标数组
            hv_ResultRow = new HTuple();
            hv_ResultColumn = new HTuple();
            hv_ArcType = new HTuple();
            try
            {
                //获取图像尺寸
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                //产生xld
                ho_Contour.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_ROIRows, hv_ROICols);
                //用回归线法（不抛出异常点，所有点权重一样）拟合圆
                HOperatorSet.FitCircleContourXld(ho_Contour, "algebraic", -1, 0, 0, 3, 2, out hv_RowC,
                    out hv_ColumnC, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                //根据拟合结果产生xld，并保持到显示对象
                ho_ContCircle.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_ContCircle, hv_RowC, hv_ColumnC, hv_Radius,
                    hv_StartPhi, hv_EndPhi, hv_PointOrder, 3);
                OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                SP_O++;
                ho_Regions.Dispose();
                HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_ContCircle, out ho_Regions);
                OTemp[SP_O - 1].Dispose();
                SP_O = 0;

                //获取圆或圆弧xld上的点坐标
                HOperatorSet.GetContourXld(ho_ContCircle, out hv_RowXLD, out hv_ColXLD);

                //求圆或圆弧xld上的点的数量
                HOperatorSet.TupleLength(hv_ColXLD, out hv_Length2);
                if ((int)(new HTuple(hv_Elements.TupleLess(3))) != 0)
                {
                    Disp_message(hv_WindowHandle, "检测的边缘数量太少，请重新设置!",
                        52, 12, true);
                    ho_Contour.Dispose();
                    ho_ContCircle.Dispose();
                    ho_Rectangle1.Dispose();
                    ho_Arrow1.Dispose();

                    return;
                }
                //如果xld是圆弧，有Length2个点，从起点开始，等间距（间距为Length2/(Elements-1)）取Elements个点，作为卡尺工具的中点
                //如果xld是圆，有Length2个点，以0°为起点，从起点开始，等间距（间距为Length2/(Elements)）取Elements个点，作为卡尺工具的中点
                for (hv_i = 0; hv_i.Continue(hv_Elements - 1, 1); hv_i = hv_i.TupleAdd(1))
                {
                    if ((int)(new HTuple(((hv_RowXLD.TupleSelect(0))).TupleEqual(hv_RowXLD.TupleSelect(
                        hv_Length2 - 1)))) != 0)
                    {
                        //xld的起点和终点坐标相对，为圆
                        HOperatorSet.TupleInt(((1.0 * hv_Length2) / hv_Elements) * hv_i, out hv_j);
                        hv_ArcType = "circle";
                    }
                    else
                    {
                        //否则为圆弧
                        HOperatorSet.TupleInt(((1.0 * hv_Length2) / (hv_Elements - 1)) * hv_i, out hv_j);
                        hv_ArcType = "arc";
                    }
                    //索引越界，强制赋值为最后一个索引
                    if ((int)(new HTuple(hv_j.TupleGreaterEqual(hv_Length2))) != 0)
                    {
                        hv_j = hv_Length2 - 1;
                        //continue
                    }
                    //获取卡尺工具中心
                    hv_RowE = hv_RowXLD.TupleSelect(hv_j);
                    hv_ColE = hv_ColXLD.TupleSelect(hv_j);

                    //超出图像区域，不检测，否则容易报异常
                    if ((int)((new HTuple((new HTuple((new HTuple(hv_RowE.TupleGreater(hv_Height - 1))).TupleOr(
                        new HTuple(hv_RowE.TupleLess(0))))).TupleOr(new HTuple(hv_ColE.TupleGreater(
                        hv_Width - 1))))).TupleOr(new HTuple(hv_ColE.TupleLess(0)))) != 0)
                    {
                        continue;
                    }
                    //边缘搜索方向类型：'inner'搜索方向由圆外指向圆心；'outer'搜索方向由圆心指向圆外
                    if ((int)(new HTuple(hv_Direct.TupleEqual("inner"))) != 0)
                    {
                        //求卡尺工具的边缘搜索方向
                        //求圆心指向边缘的矢量的角度
                        HOperatorSet.TupleAtan2((-hv_RowE) + hv_RowC, hv_ColE - hv_ColumnC, out hv_ATan);
                        //角度反向
                        hv_ATan = ((new HTuple(180)).TupleRad()) + hv_ATan;
                    }
                    else
                    {
                        //求卡尺工具的边缘搜索方向
                        //求圆心指向边缘的矢量的角度
                        HOperatorSet.TupleAtan2((-hv_RowE) + hv_RowC, hv_ColE - hv_ColumnC, out hv_ATan);
                    }

                    //产生卡尺xld，并保持到显示对象
                    ho_Rectangle1.Dispose();
                    HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle1, hv_RowE, hv_ColE,
                        hv_ATan, hv_DetectHeight.TupleInt() / 2, hv_DetectWidth.TupleInt() / 2);
                    OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                    SP_O++;
                    ho_Regions.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Rectangle1, out ho_Regions);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                    //用箭头xld指示边缘搜索方向，并保持到显示对象
                    if ((int)(new HTuple(hv_i.TupleEqual(0))) != 0)
                    {
                        hv_RowL2 = hv_RowE + ((hv_DetectHeight.TupleInt() / 2) * (((-hv_ATan)).TupleSin()));
                        hv_RowL1 = hv_RowE - ((hv_DetectHeight.TupleInt() / 2) * (((-hv_ATan)).TupleSin()));
                        hv_ColL2 = hv_ColE + ((hv_DetectHeight.TupleInt() / 2) * (((-hv_ATan)).TupleCos()));
                        hv_ColL1 = hv_ColE - ((hv_DetectHeight.TupleInt() / 2) * (((-hv_ATan)).TupleCos()));
                        ho_Arrow1.Dispose();
                        Gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2,
                            25, 25);
                        OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                        SP_O++;
                        ho_Regions.Dispose();
                        HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Arrow1, out ho_Regions);
                        OTemp[SP_O - 1].Dispose();
                        SP_O = 0;
                    }

                    //产生测量对象句柄
                    HOperatorSet.GenMeasureRectangle2(hv_RowE, hv_ColE, hv_ATan, hv_DetectHeight.TupleInt() / 2,
                        hv_DetectWidth.TupleInt() / 2, hv_Width, hv_Height, "nearest_neighbor", out hv_MsrHandle_Measure);

                    //设置极性
                    if ((int)(new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("negative"))) != 0)
                    {
                        hv_Transition_COPY_INP_TMP = "negative";
                    }
                    else
                    {
                        if ((int)(new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("positive"))) != 0)
                        {
                            hv_Transition_COPY_INP_TMP = "positive";
                        }
                        else
                        {
                            hv_Transition_COPY_INP_TMP = "all";
                        }
                    }
                    //设置边缘位置。最强点是从所有边缘中选择幅度绝对值最大点，需要设置为'all'
                    if ((int)(new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("first"))) != 0)
                    {
                        hv_Select_COPY_INP_TMP = "first";
                    }
                    else
                    {
                        if ((int)(new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("last"))) != 0)
                        {
                            hv_Select_COPY_INP_TMP = "last";
                        }
                        else
                        {
                            hv_Select_COPY_INP_TMP = "all";
                        }
                    }
                    //检测边缘
                    HOperatorSet.MeasurePos(ho_Image, hv_MsrHandle_Measure, hv_Sigma, hv_Threshold,
                        hv_Transition_COPY_INP_TMP, hv_Select_COPY_INP_TMP, out hv_RowEdge, out hv_ColEdge,
                        out hv_Amplitude, out hv_Distance);
                    //清除测量对象句柄
                    HOperatorSet.CloseMeasure(hv_MsrHandle_Measure);
                    //临时变量初始化
                    //tRow，tCol保存找到指定边缘的坐标
                    hv_tRow = 0;
                    hv_tCol = 0;
                    //t保存边缘的幅度绝对值
                    hv_t = 0;
                    HOperatorSet.TupleLength(hv_RowEdge, out hv_Number);
                    //找到的边缘必须至少为1个
                    if ((int)(new HTuple(hv_Number.TupleLess(1))) != 0)
                    {
                        continue;
                    }
                    //有多个边缘时，选择幅度绝对值最大的边缘
                    for (hv_k = 0; hv_k.Continue(hv_Number - 1, 1); hv_k = hv_k.TupleAdd(1))
                    {
                        if ((int)(new HTuple(((((hv_Amplitude.TupleSelect(hv_k))).TupleAbs())).TupleGreater(
                            hv_t))) != 0)
                        {
                            hv_tRow = hv_RowEdge.TupleSelect(hv_k);
                            hv_tCol = hv_ColEdge.TupleSelect(hv_k);
                            hv_t = ((hv_Amplitude.TupleSelect(hv_k))).TupleAbs();
                        }
                    }
                    //把找到的边缘保存在输出数组
                    if ((int)(new HTuple(hv_t.TupleGreater(0))) != 0)
                    {
                        hv_ResultRow = hv_ResultRow.TupleConcat(hv_tRow);
                        hv_ResultColumn = hv_ResultColumn.TupleConcat(hv_tCol);
                    }
                }

                ho_Contour.Dispose();
                ho_ContCircle.Dispose();
                ho_Rectangle1.Dispose();
                ho_Arrow1.Dispose();

                return;
            }
            catch (HalconException)
            {
                ho_Contour.Dispose();
                ho_ContCircle.Dispose();
                ho_Rectangle1.Dispose();
                ho_Arrow1.Dispose();

                //throw HDevExpDefaultException;
            }
        }
        /// <summary>
        /// 测量顶点 
        /// </summary>
        /// <param name="ho_Image">图像</param>
        /// <param name="hv_Row">原点位置Row</param>
        /// <param name="hv_Coloumn">原点位置Col</param>
        /// <param name="hv_Phi">角度</param>
        /// <param name="hv_Length1">第一主轴长度</param>
        /// <param name="hv_Length2">第二主轴长度</param>
        /// <param name="hv_DetectWidth">测量点长度</param>
        /// <param name="hv_Sigma">高斯滤波因子</param>
        /// <param name="hv_Threshold">边缘幅度阈值</param>
        /// <param name="hv_Transition">positive表示由黑到白 negative表示由白到黑 all表示以上两种方向</param>
        /// <param name="hv_Select">first表示选择第一点 last表示选择最后一点 max表示选择边缘幅度最强点</param>
        /// <param name="hv_EdgeRows">输出点组Rows</param>
        /// <param name="hv_EdgeColumns">输出点组Cols</param>
        /// <param name="hv_ResultRow">输出顶点row</param>
        /// <param name="hv_ResultColumn">输出顶点col</param>
        public static void Peak(HObject ho_Image, HTuple hv_Row, HTuple hv_Coloumn, HTuple hv_Phi,
            HTuple hv_Length1, HTuple hv_Length2, HTuple hv_DetectWidth, HTuple hv_Sigma,
            HTuple hv_Threshold, HTuple hv_Transition, HTuple hv_Select, out HTuple hv_EdgeRows,
            out HTuple hv_EdgeColumns, out double? hv_ResultRow, out double? hv_ResultColumn)
        {
            // Local iconic variables 
            HObject ho_Rectangle, ho_Regions1;
            // Local control variables 
            HTuple hv_ROILineRow1 = null, hv_ROILineCol1 = null;
            HTuple hv_ROILineRow2 = null, hv_ROILineCol2 = null, hv_StdLineRow1 = null;
            HTuple hv_StdLineCol1 = null, hv_StdLineRow2 = null, hv_StdLineCol2 = null;
            HTuple hv_Cos = null, hv_Sin = null, hv_Col1 = null, hv_Row1 = null;
            HTuple hv_Col2 = null, hv_Row2 = null, hv_Col3 = null;
            HTuple hv_Row3 = null, hv_Col4 = null, hv_Row4 = null;
            HTuple hv_ResultRows = null, hv_ResultColumns = null, hv_Max = null;
            HTuple hv_i = new HTuple(), hv_Distance1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Regions1);
            //初始化
            hv_ResultRow = 0;
            hv_ResultColumn = 0;
            hv_EdgeColumns = new HTuple();
            hv_EdgeRows = new HTuple();
            //仿射矩形Length2所在直线作为rake工具的ROI
            hv_ROILineRow1 = 0;
            hv_ROILineCol1 = 0;
            hv_ROILineRow2 = 0;
            hv_ROILineCol2 = 0;

            //仿射矩形方向所直线的边做基准线
            hv_StdLineRow1 = 0;
            hv_StdLineCol1 = 0;
            hv_StdLineRow2 = 0;
            hv_StdLineCol2 = 0;
            //判断仿射矩形是否有效
            if ((int)((new HTuple(hv_Length1.TupleLessEqual(0))).TupleOr(new HTuple(hv_Length2.TupleLessEqual(
                0)))) != 0)
            {
                ho_Rectangle.Dispose();
                ho_Regions1.Dispose();

                return;
            }
            //计算仿射矩形角度的正弦值、余弦值
            HOperatorSet.TupleCos(hv_Phi, out hv_Cos);
            HOperatorSet.TupleSin(hv_Phi, out hv_Sin);

            //矩形第一个端点坐标
            hv_Col1 = 1.0 * ((hv_Coloumn - (hv_Length1 * hv_Cos)) - (hv_Length2 * hv_Sin));
            hv_Row1 = 1.0 * (hv_Row - (((-hv_Length1) * hv_Sin) + (hv_Length2 * hv_Cos)));

            //矩形第二个端点坐标
            hv_Col2 = 1.0 * ((hv_Coloumn + (hv_Length1 * hv_Cos)) - (hv_Length2 * hv_Sin));
            hv_Row2 = 1.0 * (hv_Row - ((hv_Length1 * hv_Sin) + (hv_Length2 * hv_Cos)));

            //矩形第三个端点坐标
            hv_Col3 = 1.0 * ((hv_Coloumn + (hv_Length1 * hv_Cos)) + (hv_Length2 * hv_Sin));
            hv_Row3 = 1.0 * (hv_Row - ((hv_Length1 * hv_Sin) - (hv_Length2 * hv_Cos)));

            //矩形第四个端点坐标
            hv_Col4 = 1.0 * ((hv_Coloumn - (hv_Length1 * hv_Cos)) + (hv_Length2 * hv_Sin));
            hv_Row4 = 1.0 * (hv_Row - (((-hv_Length1) * hv_Sin) - (hv_Length2 * hv_Cos)));
            //仿射矩形方向所直线的边做基准线
            hv_StdLineRow1 = hv_Row2.Clone();
            hv_StdLineCol1 = hv_Col2.Clone();
            hv_StdLineRow2 = hv_Row3.Clone();
            hv_StdLineCol2 = hv_Col3.Clone();
            //仿射矩形Length2所在直线作为rake工具的ROI
            hv_ROILineRow1 = (hv_Row1 + hv_Row2) * 0.5;
            hv_ROILineCol1 = (hv_Col1 + hv_Col2) * 0.5;
            hv_ROILineRow2 = (hv_Row3 + hv_Row4) * 0.5;
            hv_ROILineCol2 = (hv_Col3 + hv_Col4) * 0.5;
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_Row, hv_Coloumn, hv_Phi,
                hv_Length1, hv_Length2);
            ho_Regions1.Dispose();
            Rake(ho_Image, out ho_Regions1, hv_Length2, hv_Length1 * 2, hv_DetectWidth, hv_Sigma,
                hv_Threshold, hv_Transition, hv_Select, hv_ROILineRow1, hv_ROILineCol1, hv_ROILineRow2,
                hv_ROILineCol2, out hv_ResultRows, out hv_ResultColumns);
            //求所有边缘点到基准线的距离，保存最大距离及其对应的边缘点坐标，作为顶点
            hv_Max = 0;
            if ((int)(new HTuple((new HTuple(hv_ResultColumns.TupleLength())).TupleGreater(
                0))) != 0)
            {
                hv_EdgeRows = hv_ResultRows.Clone();
                hv_EdgeColumns = hv_ResultColumns.Clone();
                for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_ResultColumns.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                {
                    HOperatorSet.DistancePl(hv_ResultRows.TupleSelect(hv_i), hv_ResultColumns.TupleSelect(
                        hv_i), hv_StdLineRow1, hv_StdLineCol1, hv_StdLineRow2, hv_StdLineCol2,
                        out hv_Distance1);
                    if ((int)(new HTuple(hv_Max.TupleLess(hv_Distance1))) != 0)
                    {
                        hv_Max = hv_Distance1.Clone();
                        hv_ResultRow = hv_ResultRows.TupleSelect(hv_i);
                        hv_ResultColumn = hv_ResultColumns.TupleSelect(hv_i);
                    }

                }
            }
            ho_Rectangle.Dispose();
            ho_Regions1.Dispose();

            return;

        }
        /// <summary>
        /// 图像灰度值缩放
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="ho_ImageScaled"></param>
        /// <param name="hv_Min"></param>
        /// <param name="hv_Max"></param>
        public static void Scale_image_range(HObject ho_Image, out HObject ho_ImageScaled, HTuple hv_Min,
           HTuple hv_Max)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageSelected = null, ho_SelectedChannel = null;
            HObject ho_LowerRegion = null, ho_UpperRegion = null, ho_ImageSelectedScaled = null;

            // Local copy input parameter variables 
            HObject ho_Image_COPY_INP_TMP;
            ho_Image_COPY_INP_TMP = ho_Image.CopyObj(1, -1);



            // Local control variables 

            HTuple hv_LowerLimit = new HTuple(), hv_UpperLimit = new HTuple();
            HTuple hv_Mult = null, hv_Add = null, hv_NumImages = null;
            HTuple hv_ImageIndex = null, hv_Channels = new HTuple();
            HTuple hv_ChannelIndex = new HTuple(), hv_MinGray = new HTuple();
            HTuple hv_MaxGray = new HTuple(), hv_Range = new HTuple();
            HTuple hv_Max_COPY_INP_TMP = hv_Max.Clone();
            HTuple hv_Min_COPY_INP_TMP = hv_Min.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageSelected);
            HOperatorSet.GenEmptyObj(out ho_SelectedChannel);
            HOperatorSet.GenEmptyObj(out ho_LowerRegion);
            HOperatorSet.GenEmptyObj(out ho_UpperRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageSelectedScaled);

            if ((int)(new HTuple((new HTuple(hv_Min_COPY_INP_TMP.TupleLength())).TupleEqual(
                2))) != 0)
            {
                hv_LowerLimit = hv_Min_COPY_INP_TMP.TupleSelect(1);
                hv_Min_COPY_INP_TMP = hv_Min_COPY_INP_TMP.TupleSelect(0);
            }
            else
            {
                hv_LowerLimit = 0.0;
            }
            if ((int)(new HTuple((new HTuple(hv_Max_COPY_INP_TMP.TupleLength())).TupleEqual(
                2))) != 0)
            {
                hv_UpperLimit = hv_Max_COPY_INP_TMP.TupleSelect(1);
                hv_Max_COPY_INP_TMP = hv_Max_COPY_INP_TMP.TupleSelect(0);
            }
            else
            {
                hv_UpperLimit = 255.0;
            }
            //
            //Calculate scaling parameters.
            hv_Mult = (((hv_UpperLimit - hv_LowerLimit)).TupleReal()) / (hv_Max_COPY_INP_TMP - hv_Min_COPY_INP_TMP);
            hv_Add = ((-hv_Mult) * hv_Min_COPY_INP_TMP) + hv_LowerLimit;
            //
            //Scale image.
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ScaleImage(ho_Image_COPY_INP_TMP, out ExpTmpOutVar_0, hv_Mult, hv_Add);
                ho_Image_COPY_INP_TMP.Dispose();
                ho_Image_COPY_INP_TMP = ExpTmpOutVar_0;
            }
            //
            //Clip gray values if necessary.
            //This must be done for each image and channel separately.
            ho_ImageScaled.Dispose();
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.CountObj(ho_Image_COPY_INP_TMP, out hv_NumImages);
            HTuple end_val49 = hv_NumImages;
            HTuple step_val49 = 1;
            for (hv_ImageIndex = 1; hv_ImageIndex.Continue(end_val49, step_val49); hv_ImageIndex = hv_ImageIndex.TupleAdd(step_val49))
            {
                ho_ImageSelected.Dispose();
                HOperatorSet.SelectObj(ho_Image_COPY_INP_TMP, out ho_ImageSelected, hv_ImageIndex);
                HOperatorSet.CountChannels(ho_ImageSelected, out hv_Channels);
                HTuple end_val52 = hv_Channels;
                HTuple step_val52 = 1;
                for (hv_ChannelIndex = 1; hv_ChannelIndex.Continue(end_val52, step_val52); hv_ChannelIndex = hv_ChannelIndex.TupleAdd(step_val52))
                {
                    ho_SelectedChannel.Dispose();
                    HOperatorSet.AccessChannel(ho_ImageSelected, out ho_SelectedChannel, hv_ChannelIndex);
                    HOperatorSet.MinMaxGray(ho_SelectedChannel, ho_SelectedChannel, 0, out hv_MinGray,
                        out hv_MaxGray, out hv_Range);
                    ho_LowerRegion.Dispose();
                    HOperatorSet.Threshold(ho_SelectedChannel, out ho_LowerRegion, ((hv_MinGray.TupleConcat(
                        hv_LowerLimit))).TupleMin(), hv_LowerLimit);
                    ho_UpperRegion.Dispose();
                    HOperatorSet.Threshold(ho_SelectedChannel, out ho_UpperRegion, hv_UpperLimit,
                        ((hv_UpperLimit.TupleConcat(hv_MaxGray))).TupleMax());
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.PaintRegion(ho_LowerRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                            hv_LowerLimit, "fill");
                        ho_SelectedChannel.Dispose();
                        ho_SelectedChannel = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.PaintRegion(ho_UpperRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                            hv_UpperLimit, "fill");
                        ho_SelectedChannel.Dispose();
                        ho_SelectedChannel = ExpTmpOutVar_0;
                    }
                    if ((int)(new HTuple(hv_ChannelIndex.TupleEqual(1))) != 0)
                    {
                        ho_ImageSelectedScaled.Dispose();
                        HOperatorSet.CopyObj(ho_SelectedChannel, out ho_ImageSelectedScaled, 1,
                            1);
                    }
                    else
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.AppendChannel(ho_ImageSelectedScaled, ho_SelectedChannel,
                                out ExpTmpOutVar_0);
                            ho_ImageSelectedScaled.Dispose();
                            ho_ImageSelectedScaled = ExpTmpOutVar_0;
                        }
                    }
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ImageScaled, ho_ImageSelectedScaled, out ExpTmpOutVar_0
                        );
                    ho_ImageScaled.Dispose();
                    ho_ImageScaled = ExpTmpOutVar_0;
                }
            }
            ho_Image_COPY_INP_TMP.Dispose();
            ho_ImageSelected.Dispose();
            ho_SelectedChannel.Dispose();
            ho_LowerRegion.Dispose();
            ho_UpperRegion.Dispose();
            ho_ImageSelectedScaled.Dispose();

            return;
        }

        /// <summary>
        /// 保存图像
        /// </summary>
        /// <param name="Image">图像</param>
        /// <param name="Path">地址</param>
        public static void Write_Image(HObject Image, string Path)
        {
            //图像为空，不保存
            if (Image.IsInitialized() == false)
            {
                return;
            }
            int nPos;
            string ImageType;
            Directory.CreateDirectory(Path);
            //通常保存文件的路径格式为xxxx.xxx，最后一个.后的字符为图像的扩展名，获取扩展名，作为write_image的输入参数
            //从右边开始，查询.的位置

            nPos = Path.LastIndexOf('.');
            if (nPos > -1)
            {
                //获取图像扩展名
                ImageType = Path.Substring(nPos + 1, Path.Length - nPos - 1);
            }
            else
                ImageType = "bmp";
            //保存图像
            HOperatorSet.WriteImage(Image, ImageType, 0, Path);
        }

        /// <summary>
        /// 读取图像
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="Path">地址</param>
        /// <returns></returns>
        public static bool Read_Image(out HObject Image, string Path)
        {
            Image = new HObject();
            //文件是否存在
            try
            {

                HOperatorSet.ReadImage(out Image, Path);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 保存变量Tuple
        /// </summary>
        /// <param name="Tuple"></param>
        /// <param name="Path">地址</param>
        public static void Write_Tuple(HTuple Tuple, string Path)
        {
            //数组为空，不保存
            if (Tuple.Length == 0)
            {
                return;
            }
            //ZazaniaoDll.CreateAllDirectoryEx(Path);
            Directory.CreateDirectory(Path);

            HOperatorSet.WriteTuple(Tuple, Path);
        }

        /// <summary>
        /// 读取变量Tuple
        /// </summary>
        /// <param name="Tuple"></param>
        /// <param name="Path">地址</param>
        /// <returns></returns>
        public static bool Read_Tuple(ref HTuple Tuple, string Path)
        {
            //文件是否存在
            try
            {
                HOperatorSet.ReadTuple(Path, out Tuple);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 保存区域
        /// </summary>
        /// <param name="Region">区域</param>
        /// <param name="Path">地址</param>
        public static void Write_Region(HObject Region, string Path)
        {
            //区域为空，不保存
            if (!Region.IsInitialized())
            {
                return;
            }
            Directory.CreateDirectory(Path);

            HOperatorSet.WriteRegion(Region, Path);
        }

        /// <summary>
        /// 读取区域
        /// </summary>
        /// <param name="Region"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static bool Read_Region(ref HObject Region, string Path)
        {
            try
            {
                Region.Dispose();
                HOperatorSet.ReadRegion(out Region, Path);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 链接元素
        /// </summary>
        /// <param name="Obj1">元素1</param>
        /// <param name="Obj2">元素2</param>
        /// <param name="Obj3">新的元素</param>
        public static void Concat_Obj(ref HObject Obj1, ref HObject Obj2, ref HObject Obj3)
        {
            if (!Obj1.IsInitialized())
            {
                HOperatorSet.GenEmptyObj(out Obj1);
            }
            if (!Obj2.IsInitialized())
            {
                HOperatorSet.GenEmptyObj(out Obj2);
            }
            //             if (!(Obj1.IsInitialized()) || (Obj1.CountObj() < 1))
            //             {
            //                 HOperatorSet.CopyObj(Obj1,out Obj3,1,-1);
            //             }
            //             else
            {
                HOperatorSet.ConcatObj(Obj1, Obj2, out Obj3);
            }
        }

        /// <summary>
        /// 检测Object是否有效
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public static bool ObjectValided(HObject Obj)
        {
            if (Obj == null || !Obj.IsInitialized())
            {
                return false;
            }
            if (Obj.CountObj() < 1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 剔除重复的XLD
        /// </summary>
        /// <param name="XLD"></param>
        /// <returns></returns>
        public static HObject RemoveDuplicatesXld(HObject XLD)
        {
            HObject hObjectt = new HObject();
            HObject XldS = new HObject();
            XldS.GenEmptyObj();
            int dst = XLD.CountObj();

            for (int i = 0; i < XLD.CountObj(); i++)
            {
                HOperatorSet.SelectObj(XLD, out hObjectt, i + 1);
                if (XldS.CountObj() == 0)
                {
                    XldS = XldS.ConcatObj(hObjectt);
                }
                HOperatorSet.GetContourXld(hObjectt, out HTuple rows, out HTuple cols);

                for (int i2 = 0; i2 < XldS.CountObj(); i2++)
                {
                    HOperatorSet.SelectObj(XldS, out hObjectt, i2 + 1);
                    HOperatorSet.GetContourXld(hObjectt, out HTuple rows2, out HTuple cols2);
                    if (rows.TupleEqual(rows2) || cols.TupleEqual(cols2))
                    {
                        goto st;
                    }
                }
                XldS = XldS.ConcatObj(hObjectt);
            st:
                if (true)
                {
                }
            }
            dst = XldS.CountObj();

            return XldS;
        }

        /// <summary>
        /// 拟合直线
        /// </summary>
        /// <param name="Image">输入图像</param>
        /// <param name="objDisp">图形</param>
        /// <param name="HomMat2D">仿射变换</param>
        /// <param name="Elements">边缘点数</param>
        /// <param name="Threshold">边缘阀值</param>
        /// <param name="Sigma">边缘滤波系数</param>
        /// <param name="Transition">边缘极性</param>
        /// <param name="Point_Select">边缘点的选择</param>
        /// <param name="ROI_X">rake工具x数组</param>
        /// <param name="ROI_Y">rake工具y数组</param>
        /// <param name="Caliper_Height">卡尺工具高</param>
        /// <param name="Caliper_Width">卡尺工具宽</param>
        /// <param name="Min_Points_Num">最小数量</param>
        /// <param name="Caliper_Regions">产生的卡尺工具图形</param>
        /// <param name="Edges_X">找到的边缘点x数据</param>
        /// <param name="Edges_Y">找到的边缘点y数据</param>
        /// <param name="Result_xld">拟合得到的直线</param>
        /// <param name="Result_X">拟合得到的直线的点的x数组</param>
        /// <param name="Result_Y">拟合得到的直线的点的y数组</param>
        /// <param name="Error">错误信息</param>
        public static void Fit_Line(HObject Image, ref HObject objDisp, HTuple HomMat2D, int Elements, int Threshold, double Sigma, string Transition, string Point_Select,
            HTuple ROI_X, HTuple ROI_Y, int Caliper_Height, int Caliper_Width, int Min_Points_Num, out HObject Caliper_Regions, out HTuple Edges_X,
            out HTuple Edges_Y, out HObject Result_xld, out HTuple Result_X, out HTuple Result_Y)
        {
            HObject Cross;
            HOperatorSet.GenEmptyObj(out Cross);
            HOperatorSet.GenEmptyObj(out Caliper_Regions);
            HOperatorSet.GenEmptyObj(out Result_xld);
            Edges_X = new HTuple();
            Edges_Y = new HTuple();
            Result_X = new HTuple();
            Result_Y = new HTuple();
            try
            {
                //判断图像是否为空
                if (!ObjectValided(Image))
                {
                    return;
                }
                HTuple Row0, Row1, Col0, Col1;

                //判断rake工具的ROI是否有效
                if (ROI_Y.Length < 2)
                {
                    Cross.Dispose();
                    return;
                }
                //判断ROI仿射变换矩阵是否有效，有效的时候，有6个数据
                if (HomMat2D.Length < 6)
                {
                    //矩阵无效，直接用原始ROI执行rake工具找边缘点
                    Result_xld.Dispose();
                    Rake(Image, out Caliper_Regions, Elements, Caliper_Height, Caliper_Width, Sigma, Threshold,
                    Transition, Point_Select, ROI_Y[0], ROI_X[0],
                    ROI_Y[1], ROI_X[1], out Edges_Y, out Edges_X);
                }
                else
                {
                    HTuple New_ROI_Y, New_ROI_X;
                    //矩阵有效，先产生新的ROI,用新的ROI执行rake工具找边缘点
                    HOperatorSet.AffineTransPoint2d(HomMat2D, ROI_Y, ROI_X, out New_ROI_Y, out New_ROI_X);
                    Rake(Image, out Caliper_Regions, Elements, Caliper_Height, Caliper_Width, Sigma, Threshold,
                    Transition, Point_Select, New_ROI_Y[0], New_ROI_X[0], New_ROI_Y[1], New_ROI_X[1], out Edges_Y, out Edges_X);
                }
                //把产生的卡尺工具图像添加到显示图形
                Concat_Obj(ref objDisp, ref Caliper_Regions, ref objDisp);

                //判断是否找到有边缘点，如果有，产生边缘点x图形，并添加到显示图形
                if (Edges_Y.Length > 0)
                {
                    HOperatorSet.GenCrossContourXld(out Cross, Edges_Y, Edges_X, 20, (new HTuple(45)).TupleRad());
                    Concat_Obj(ref objDisp, ref Cross, ref objDisp);
                }
                //如果边缘点数大于等于最小点数，进行直线拟合；否则返回错误信息
                if (Edges_Y.Length >= Min_Points_Num)
                {
                    //拟合直线
                    Pts_to_best_line(out Result_xld, Edges_Y, Edges_X, Min_Points_Num, out Row0, out Col0, out Row1, out Col1);
                    //把直线的点添加到结果数组
                    Result_Y = Row0.TupleConcat(Row1);
                    Result_X = Col0.TupleConcat(Col1);
                    //把拟合的直线图形添加到显示图形
                    Concat_Obj(ref objDisp, ref Result_xld, ref objDisp);
                }
                else
                {
                }
            }
            catch (HalconException HDevExpDefaultException)
            {
                Cross.Dispose();

                Log(HDevExpDefaultException.Message);
            }
            Cross.Dispose();
        }

        /// <summary>
        /// XLD转换为区域
        /// </summary>
        /// <param name="xld">XLD</param>
        /// <returns>区域</returns>
        public static HObject XLD_To_Region(HObject xld)
        {
            HTuple hTuple = xld.GetObjClass();

            if(hTuple.Length==0|| hTuple == "region")
            {
                return xld;
            }
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            try
            {
                int ds = xld.CountObj();
                for (int i = 0; i < ds; i++)
                {
                    HOperatorSet.SelectObj(xld, out HObject hObject1, i + 1);
                    HOperatorSet.GetContourXld(hObject1, out HTuple row, out HTuple col);
                    HOperatorSet.GenRegionPolygon(out HObject hObject2, row, col);
                    hObject = hObject.ConcatObj(hObject2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return hObject;
        }

        /// <summary>
        /// 区域转换为XLD
        /// </summary>
        /// <param name="Region">区域</param>
        /// <returns>返回XLD</returns>
        public static HObject Region_To_XLD(HObject Region)
        {
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            try
            {
                if (Region.GetObjClass() == "region")
                {
                    return Region;
                }
                int ds = Region.CountObj();
                for (int i = 0; i < ds; i++)
                {
                    HOperatorSet.SelectObj(Region, out HObject hObject1, i + 1);
                    HOperatorSet.GetRegionContour(hObject1, out HTuple rows, out HTuple columns);
                    if (rows.Length > 1)
                    {
                        HOperatorSet.GenContourPolygonXld(out HObject hObject2, rows, columns);
                        hObject = hObject.ConcatObj(hObject2);
                    }
                }
                int dts = hObject.CountObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return hObject;
        }

        #endregion Halcon视觉常用算子

        /// <summary>
        /// 错误记录
        /// </summary>
        /// <param name="str"></param>
        public static void Log(string str)
        {
            //MessageBox.Show(str);
            logNet.WriteError("静态错误登记：" + str);
        }

        public static void ErrLog(Exception ex)
        {
            logNet.WriteException("静态错误登记：", ex);
        }

        /// <summary>
        /// 静态错误登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ex"></param>
        public static void ErrLog(string name, Exception ex)
        {
        statrt:
            try
            {
                logNet.WriteException("静态错误登记：" + name, ex);
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

        /// <summary>
        /// 图片转换BITMAP转Himage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static HImage ToHImage(Bitmap bitmap)
        {
            HImage h_img = new HImage();
            try
            {
                Bitmap image = (Bitmap)bitmap.Clone();

                BitmapData bmData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                //unsafe
                //{
                //    // Create HALCON image from the pointer.  bgr
                //    h_img.GenImageInterleaved(bmData.Scan0, "bgr", image.Width, image.Height, -1, "byte", image.Width, image.Height, 0, 0, -1, 0);
                //    //tmp = h_img;  
                //}
                image.UnlockBits(bmData);
                image.Dispose();
                bitmap.Dispose();
            }
            catch
            {
                //h_img = tmp;  
            }
            return h_img;
        }

        /// <summary>
        /// 24位实际使用时，假如原图8位灰度图，那么BitmapToHObjectBpp8 和BitmapToHObjectBpp24的结果是一样的。而为24位彩色图时，只能用BitmapToHObjectBpp24。
        ///可先得到Bitmap 图的PixelFormat ，而后再进行转换。
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static HObject Bitmap2HObjectBpp24(Bitmap bmp)
        {
            HObject image = new HObject();
            try
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                HOperatorSet.GenImageInterleaved(out image, srcBmpData.Scan0, "bgr", bmp.Width, bmp.Height, 0, "byte", 0, 0, 0, 0, -1, 0);
                bmp.UnlockBits(srcBmpData);
            }
            catch (Exception ex)
            {
                image = null;
            }
            return image;
        }

        /// <summary>
        /// 24位实际使用时，假如原图8位灰度图，那么BitmapToHObjectBpp8 和BitmapToHObjectBpp24的结果是一样的。而为24位彩色图时，只能用BitmapToHObjectBpp24。
        ///可先得到Bitmap 图的PixelFormat ，而后再进行转换。
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static HObject Bitmap2HObjectBpp8(Bitmap bmp)
        {
            HObject image = new HObject();
            try
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

                HOperatorSet.GenImage1(out image, "byte", bmp.Width, bmp.Height, srcBmpData.Scan0);
                bmp.UnlockBits(srcBmpData);
            }
            catch (Exception ex)
            {
                image = null;
            }
            return image;
        }



        /// <summary>
        /// 图像转换某些像素图像无法转换
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static HObject GenImageInterleaved(Bitmap bitmap)
        {
            HObject image;
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);

            BitmapData bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            try
            {
                if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    HOperatorSet.GenImage1(out image, "byte", bitmap.Width, bitmap.Height, bitmapData.Scan0);
                }
                else
                {
                    HOperatorSet.GenImageInterleaved(out image, bitmapData.Scan0, "bgr", bitmap.Width, bitmap.Height, 0, "byte", 0, 0, 0, 0, -1, 0);
                }
                //HOperatorSet.AccessChannel(image, out image, 1);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
                bitmap.Dispose();
            }

            return image;
        }

        /// <summary>
        /// 改变图像大小
        /// </summary>
        /// <param name="imgToResize"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Image ResizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //获取图片宽度
            int sourceWidth = imgToResize.Width;
            //获取图片高度
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //计算宽度的缩放比例
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //计算高度的缩放比例
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //期望的宽度
            int destWidth = (int)(sourceWidth * nPercent);
            //期望的高度
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //绘制图像
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hv_WindowHandle"></param>
        /// <param name="hv_Size"></param>
        /// <param name="hv_Font"></param>
        /// <param name="hv_Bold"></param>
        /// <param name="hv_Slant"></param>
        public static void Set_Display_Font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font,
            HTuple hv_Bold, HTuple hv_Slant)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_OS = null, hv_BufferWindowHandle = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Scale = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_SubFamily = new HTuple(), hv_Fonts = new HTuple();
            HTuple hv_SystemFonts = new HTuple(), hv_Guess = new HTuple();
            HTuple hv_I = new HTuple(), hv_Index = new HTuple(), hv_AllowedFontSizes = new HTuple();
            HTuple hv_Distances = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_FontSelRegexp = new HTuple(), hv_FontsCourier = new HTuple();
            HTuple hv_Bold_COPY_INP_TMP = hv_Bold.Clone();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();
            HTuple hv_Slant_COPY_INP_TMP = hv_Slant.Clone();

            // Initialize local and output iconic variables 
            //This procedure sets the text font of the current window with
            //the specified attributes.
            //It is assumed that following fonts are installed on the system:
            //Windows: Courier New, Arial Times New Roman
            //Mac OS X: CourierNewPS, Arial, TimesNewRomanPS
            //Linux: courier, helvetica, times
            //Because fonts are displayed smaller on Linux than on Windows,
            //a scaling factor of 1.25 is used the get comparable results.
            //For Linux, only a limited number of font sizes is supported,
            //to get comparable results, it is recommended to use one of the
            //following sizes: 9, 11, 14, 16, 20, 27
            //(which will be mapped internally on Linux systems to 11, 14, 17, 20, 25, 34)
            //
            //Input parameters:
            //WindowHandle: The graphics window for which the font will be set
            //Size: The font size. If Size=-1, the default of 16 is used.
            //Bold: If set to 'true', a bold font is used
            //Slant: If set to 'true', a slanted font is used
            //
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            // dev_get_preferences(...); only in hdevelop
            // dev_set_preferences(...); only in hdevelop
            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                //Set font on Windows systems
                try
                {
                    //Check, if font scaling is switched on
                    HOperatorSet.OpenWindow(0, 0, 256, 256, 0, "buffer", "", out hv_BufferWindowHandle);
                    HOperatorSet.SetFont(hv_BufferWindowHandle, "-Consolas-16-*-0-*-*-1-");
                    HOperatorSet.GetStringExtents(hv_BufferWindowHandle, "test_string", out hv_Ascent,
                        out hv_Descent, out hv_Width, out hv_Height);
                    //Expected width is 110
                    hv_Scale = 110.0 / hv_Width;
                    hv_Size_COPY_INP_TMP = ((hv_Size_COPY_INP_TMP * hv_Scale)).TupleInt();
                    HOperatorSet.CloseWindow(hv_BufferWindowHandle);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Courier New";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Consolas";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Arial";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Times New Roman";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-" + hv_Font_COPY_INP_TMP) + "-") + hv_Size_COPY_INP_TMP) + "-*-") + hv_Slant_COPY_INP_TMP) + "-*-*-") + hv_Bold_COPY_INP_TMP) + "-");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Dar"))) != 0)
            {
                //Set font on Mac OS X systems. Since OS X does not have a strict naming
                //scheme for font attributes, we use tables to determine the correct font
                //name.
                hv_SubFamily = 0;
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(1);
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(2);
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Menlo-Regular";
                    hv_Fonts[1] = "Menlo-Italic";
                    hv_Fonts[2] = "Menlo-Bold";
                    hv_Fonts[3] = "Menlo-BoldItalic";
                }
                else if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "CourierNewPSMT";
                    hv_Fonts[1] = "CourierNewPS-ItalicMT";
                    hv_Fonts[2] = "CourierNewPS-BoldMT";
                    hv_Fonts[3] = "CourierNewPS-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "ArialMT";
                    hv_Fonts[1] = "Arial-ItalicMT";
                    hv_Fonts[2] = "Arial-BoldMT";
                    hv_Fonts[3] = "Arial-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "TimesNewRomanPSMT";
                    hv_Fonts[1] = "TimesNewRomanPS-ItalicMT";
                    hv_Fonts[2] = "TimesNewRomanPS-BoldMT";
                    hv_Fonts[3] = "TimesNewRomanPS-BoldItalicMT";
                }
                else
                {
                    //Attempt to figure out which of the fonts installed on the system
                    //the user could have meant.
                    HOperatorSet.QueryFont(hv_WindowHandle, out hv_SystemFonts);
                    hv_Fonts = new HTuple();
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Regular");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "MT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[0] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Italic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-ItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Oblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[1] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Bold");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldMT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[2] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldOblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[3] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                }
                hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_SubFamily);
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, (hv_Font_COPY_INP_TMP + "-") + hv_Size_COPY_INP_TMP);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else
            {
                //Set font for UNIX systems
                hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP * 1.25;
                hv_AllowedFontSizes = new HTuple();
                hv_AllowedFontSizes[0] = 11;
                hv_AllowedFontSizes[1] = 14;
                hv_AllowedFontSizes[2] = 17;
                hv_AllowedFontSizes[3] = 20;
                hv_AllowedFontSizes[4] = 25;
                hv_AllowedFontSizes[5] = 34;
                if ((int)(new HTuple(((hv_AllowedFontSizes.TupleFind(hv_Size_COPY_INP_TMP))).TupleEqual(
                    -1))) != 0)
                {
                    hv_Distances = ((hv_AllowedFontSizes - hv_Size_COPY_INP_TMP)).TupleAbs();
                    HOperatorSet.TupleSortIndex(hv_Distances, out hv_Indices);
                    hv_Size_COPY_INP_TMP = hv_AllowedFontSizes.TupleSelect(hv_Indices.TupleSelect(
                        0));
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(
                    "Courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "courier";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "helvetica";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "times";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "bold";
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "medium";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("times"))) != 0)
                    {
                        hv_Slant_COPY_INP_TMP = "i";
                    }
                    else
                    {
                        hv_Slant_COPY_INP_TMP = "o";
                    }
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = "r";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-adobe-" + hv_Font_COPY_INP_TMP) + "-") + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    if ((int)((new HTuple(((hv_OS.TupleSubstr(0, 4))).TupleEqual("Linux"))).TupleAnd(
                        new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                    {
                        HOperatorSet.QueryFont(hv_WindowHandle, out hv_Fonts);
                        hv_FontSelRegexp = (("^-[^-]*-[^-]*[Cc]ourier[^-]*-" + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP;
                        hv_FontsCourier = ((hv_Fonts.TupleRegexpSelect(hv_FontSelRegexp))).TupleRegexpMatch(
                            hv_FontSelRegexp);
                        if ((int)(new HTuple((new HTuple(hv_FontsCourier.TupleLength())).TupleEqual(
                            0))) != 0)
                        {
                            hv_Exception = "Wrong font name";
                            //throw (Exception)
                        }
                        else
                        {
                            try
                            {
                                HOperatorSet.SetFont(hv_WindowHandle, (((hv_FontsCourier.TupleSelect(
                                    0)) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                            }
                            // catch (Exception) 
                            catch (HalconException HDevExpDefaultException2)
                            {
                                HDevExpDefaultException2.ToHTuple(out hv_Exception);
                                //throw (Exception)
                            }
                        }
                    }
                    //throw (Exception)
                }
            }
            // dev_set_preferences(...); only in hdevelop

            return;
        }
        /// <summary>
        /// 根据矩形生成点位
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="phi"></param>
        /// <param name="length1"></param>
        /// <param name="rows1"></param>
        /// <param name="columns1"></param>
        /// <param name="rows2"></param>
        /// <param name="columns2"></param>
        public static void gen_rectangle2_line_point(HTuple row, HTuple column, HTuple phi, HTuple length1, out HTuple rows1, out HTuple columns1, out HTuple rows2, out HTuple columns2)
        {
            HTuple row2 = new HTuple();
            HTuple row3 = new HTuple();
            HTuple column2 = new HTuple();
            HTuple column3 = new HTuple();
            rows1 = new HTuple();
            columns1 = new HTuple();
            rows2 = new HTuple();
            columns2 = new HTuple();
            for (int i = 0; i < row.Length; i++)
            {
                HTuple homd = new HTuple();
                HOperatorSet.HomMat2dIdentity(out homd);
                HOperatorSet.HomMat2dRotate(homd, phi[i], 0, 0, out homd);
                HOperatorSet.HomMat2dTranslate(homd, row[i], column[i], out homd);
                HTuple rowt;
                HTuple columnt;
                HOperatorSet.AffineTransPixel(homd, new HTuple(new int[2]), new HTuple(new HTuple[]
                {
            -length1.TupleSelect(i) / 2,
            length1.TupleSelect(i) / 2
                }), out rowt, out columnt);
                rows1.Append(rowt[0]);
                columns1.Append(columnt[0]);
                rows2.Append(rowt[1]);
                columns2.Append(columnt[1]);
            }
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        public class RunNameConverter : StringConverter
        {
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(Vision.Instance.Himagelist.Keys);
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }
        }
    }

}