using ErosSocket.DebugPLC.Robot;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI;
using Vision2.Project.DebugF;
using Vision2.Project.formula;
using Vision2.Project.Mes;
using Vision2.vision.RestVisionForm;
using static Vision2.vision.HalconRunFile.RunProgramFile.OneCompOBJs;

namespace Vision2.vision
{
    public partial class RestObjImage : Form
    {
        public RestObjImage()
        {
            InitializeComponent();
            objImageFrom = this;
        }

        public int MaxNumber = 0;


        /// <summary>
        /// 整盘集合
        /// </summary>
        private static Queue<TrayData> TrayImageTs = new Queue<TrayData>();

        private HWindID HWindd;

        /// <summary>
        /// 单个产品
        /// </summary>
        private static OneDataVale OneProductV;

        /// <summary>
        /// 单个元件
        /// </summary>
        private OneComponent OneRObjT;

        /// <summary>
        /// 整盘
        /// </summary>
        public static TrayData TrayImage;

        public static RestObjImage RestObjImageFrom
        {
            get
            {
                if (objImageFrom == null || objImageFrom.IsDisposed)
                {
                    objImageFrom = new RestObjImage();
                }
                return objImageFrom;
            }
            set
            {
                objImageFrom = value;
            }
        }

        private static RestObjImage objImageFrom;

        public void ShowImage(TrayData trayImage)
        {
            try
            {
                hWindowControl1.Focus();
                if (HWindd == null)
                {
                    HWindd = new HWindID();
                    HWindd.Initialize(hWindowControl1);
                }
                try
                {
                    foreach (var item in trayImage.GetDataVales())
                    {
                        if (item.NotNull)
                        {
                            if (item.PanelID == null || item.PanelID == "")
                            {
                                foreach (var itemdt in item.ListCamsData)
                                {
                                    HOperatorSet.GenRectangle1(out HObject hObject, 0, 0, 2555, 2555);
                                    itemdt.Value.NGObj.AddCont(new HalconRunFile.RunProgramFile.OneRObj() {
                                        ComponentID = "SN", NGText = "SN为空", NGROI = hObject, ROI = hObject });
                                }
                            }
                        }
                    }
                    if (!trayImage.OK)
                    {
                        if (trayImage.Done)
                        {
                            foreach (var item in trayImage.GetDataVales())
                            {
                                if (item.OK)
                                {
                                    continue;
                                }
                         
                                    foreach (var itemdt in item.ListCamsData)
                                   {
                                      foreach (var itemdte in itemdt.Value.NGObj.DicOnes)
                                    {
                                        if (!itemdte.Value.aOK)
                                        {
                                            if (itemdte.Value.Done)
                                            {
                                                itemdte.Value.Done = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //UpData();
                    }
           
                }
                catch (Exception)
                {}
                if (Vision.Instance.RestTDT)
                {
                    if (RecipeCompiler.Instance.GetMes().MesArye)
                    {
                        Task task = new Task(new Action(() =>
                        {
                            WeirtAll(trayImage);
                        }));
                        task.Start();
                    }
                    else
                    {
                        WeirtAll(trayImage);
                    }
                    return;
                }
                if (!trayImage.OK)
                {
                    if (!trayImage.Done)
                    {
                        if (!TrayImageTs.Contains(trayImage))
                        {
                            TrayImageTs.Enqueue(trayImage);
                        }
                        toolStripLabel1.Text = "复判剩余:" + TrayImageTs.Count;
                        UICon.SwitchToThisWindow(RestObjImage.RestObjImageFrom.Handle, true);
                        RestObjImage.RestObjImageFrom.Show();
                        //if (TrayImage == null)
                        //{
                        //    DequeueData();
                        //}
                    }
                }
                if (trayImage.Done)
                {
                    if (RecipeCompiler.Instance.GetMes()!=null)
                    {
                        if (RecipeCompiler.Instance.GetMes().MesArye)
                        {
                            Task task = new Task(new Action(() =>
                            {
                                WeirtAll(trayImage);
                            }));
                            task.Start();
                        }
                        else
                        {
                            WeirtAll(trayImage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AlarmText.AddTextNewLine("复判窗口:" + ex.Message, Color.Red);
            }
        }
        
        //public void ShowImage(OneDataVale oneDataVale)
        //{
        //    try
        //    {
        //        hWindowControl1.Focus();
        //        if (HWindd == null)
        //        {
        //            HWindd = new HWindID();
        //            HWindd.Initialize(hWindowControl1);
        //        }
        //        OneProductV = oneDataVale;

        //        trayDatas1.SelesItem(OneProductV.TrayLocation);
        //        treeView1.Nodes.Clear();
        //        treeView2.Nodes.Clear();

        //        foreach (var itemdt in OneProductV.ListCamsData)
        //        {
        //            TreeNode treeNode = treeView1.Nodes.Add(itemdt.Key);
        //            treeNode.Tag = itemdt.Value;
        //            TreeNode treeNodeOK = treeView2.Nodes.Add(itemdt.Key);
        //            treeNodeOK.Tag = itemdt.Value;
        //            foreach (var itemdte in itemdt.Value.AllCompObjs.DicOnes)
        //            {
        //                TreeNode treeNode1 = treeNodeOK.Nodes.Add(itemdte.Key);
        //                treeNode1.Tag = itemdte.Value;
        //                treeNode1.ImageIndex = 6;
        //            }
        //            foreach (var itemdte in itemdt.Value.NGObj.DicOnes)
        //            {
        //                if (!itemdte.Value.aOK)
        //                {
        //                    TreeNode treeNode1 = treeNode.Nodes.Add(itemdte.Key);
        //                    treeNode1.Tag = itemdte.Value;
        //                    treeNode1.ImageIndex = 5;
        //                }
        //            }
        //            treeNodeOK.Expand();
        //            treeNode.Expand();
        //        }
             
        //        UICon.SwitchToThisWindow(RestObjImage.RestObjImageFrom.Handle, true);
        //        RestObjImage.RestObjImageFrom.Show();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        public void WeirtTray(TrayData trayImage)
        {
            try
            {
                for (int i = 0; i < trayImage.Count; i++)
                {
                    if (trayImage.GetDataVales()[i].NotNull)
                    {
                        string sn = trayImage.GetOneDataVale(i).PanelID;
                        if (sn == "")
                        {
                            sn = "SN";
                        }
                        string path = Vision.GetSaveImageInfo(Vision.GetRunNameVision().Name).SavePath + 
                            "\\" +  DateTime.Now.ToString("yyyy年M月d日") + "\\" + Product.ProductionName + "\\" +
                              sn + "\\" +"Data" + DateTime.Now.ToString("HH时mm分ss秒");
                         ProjectINI.ClassToJsonSavePath(trayImage.GetOneDataVale(i), path);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }      
         
        }

        public void WeirtAll(TrayData trayImage)
        {
            try
            {
                UserFormulaContrsl.WeirtAll(trayImage);
                WeirtTray(trayImage);
                trayImage.Dispose();
                if (DebugCompiler.Instance.OutDischarging > 0)
                {
                    Task task = new Task(() =>
                   {
                       try
                       {
                           if (TrayImage != null)
                           {
                               while (true)
                               {
                                   Thread.Sleep(10);
                                   if (TrayImage == null)
                                   {
                                       Thread.Sleep(5000);
                                       break;
                                   }
                               }
                           }
                           DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.OutDischarging, true);
                           Thread.Sleep(10000);
                           DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.OutDischarging, false);
                       }
                       catch (Exception) { }
                   });
                    task.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void Clser()
        {
            try
            {
                TrayImageTs.Clear();
            }
            catch (Exception)
            {
            }
        }

        private double dee;

        private void UpData()
        {
     
            try
            {
                string PatText = "";
                if (Vision.Instance.RestT)
                {
                    toolStripButton3.Text = "单个点";
                }
                else
                {
                    toolStripButton3.Text = "单个产品";
                }
                dee = HWindd.HeigthImage / HWindd.WidthImage;
                hWindowControl1.Height = (int)(hWindowControl1.Width * dee);
                if (panel2.Height < hWindowControl1.Height)
                {
                    hWindowControl1.Height = panel2.Height;
                }
                hWindowControl1.Dock = DockStyle.Top;
                textBox1.Text = OneProductV.PanelID;
                if (!OneProductV.Done)
                {
                    if (OneProductV != null)
                    {
                        OneProductV.UesrRest = true;
                        HWindd.OneResIamge.GetNgOBJS(OneProductV.GetNGCompData());
                        HWindd.SetImaage(OneProductV.GetNGImage());
                    }
                }
                PatText = "托盘号:" + OneProductV.TrayLocation + "." + OneProductV.GetNGCamName() + Environment.NewLine;
                foreach (TreeNode item in treeView1.Nodes)
                {
                    if (item.Nodes.Count == 0)
                    {
                        continue;
                    }
                    foreach (TreeNode itemdt in item.Nodes)
                    {
                        if (itemdt.Tag is OneComponent)
                        {
                            OneComponent oneComponent = itemdt.Tag as OneComponent;
                            if (!oneComponent.Done)
                            {
                                OneRObjT = oneComponent;
                                restOneComUserControl1.Location = new Point(itemdt.Bounds.X, itemdt.Bounds.Y + itemdt.Bounds.Height + 2);
                                restOneComUserControl1.Visible = true;
                                restOneComUserControl1.BringToFront();
                                restOneComUserControl1.UpData(OneRObjT);
                                break;
                            }
                            else
                            {
                                if (oneComponent.aOK)
                                {
                                    if (itemdt.ImageIndex != 3)
                                    {
                                        itemdt.ImageIndex = 3;
                                    }
                                }
                                else
                                {
                                    itemdt.ImageIndex = 4;
                                }
                            }
                        }
                    }
                    if (OneRObjT != null)
                    {
                        if (!OneRObjT.Done)
                        {
                            break;
                        }
                    }
                }
                foreach (var item in OneProductV.GetNGCompData().DicOnes)
                {
                    if (!item.Value.Done)
                    {
                        if (!item.Value.Done)
                        {
                            OneRObjT = item.Value;
                            //restOneComUserControl1.Location = new Point(itemdt.Bounds.X, itemdt.Bounds.Y + itemdt.Bounds.Height + 2);
                            restOneComUserControl1.Visible = true;
                            restOneComUserControl1.BringToFront();
                            restOneComUserControl1.UpData(OneRObjT);
                            break;
                        }
                    }
                }
                if (OneProductV.Done)
                {
                    restOneComUserControl1.Visible = false;
                }
                if (OneRObjT != null)
                {
                    PatText += OneRObjT.ComponentID + "NG信息:" + OneRObjT.NGText + "\\" + OneProductV.NGNumber;
                    restOneComUserControl1.UpData(OneRObjT);
                }

                label4.Text = PatText;
                label3.Text = RecipeCompiler.GetSPC();
                if (OneProductV != null)
                    {
                        foreach (var item in OneProductV.GetNGCompData().DicOnes)
                        {
                            if (item.Value.Done)
                            {
                                continue;
                            }
                            hWindowControl3.HalconWindow.ClearWindow();
                            hWindowControl4.HalconWindow.ClearWindow();
                            try
                            {
                                HOperatorSet.SetDraw(hWindowControl3.HalconWindow, "margin");
                                HOperatorSet.SetLineWidth(hWindowControl3.HalconWindow, Vision.Instance.LineWidth);
                                Vision.SetFont(hWindowControl3.HalconWindow);
                               
                            }
                            catch (Exception) { }
                            HObject imaget = new HObject();
                            imaget.GenEmptyObj();
                            //HOperatorSet.ReadImage(out HObject imaget, Vision.VisionPath + "Image\\" + OneRImage.LiyID + ".bmp");
                            bool isbde = false;
                            if (Directory.Exists(Vision.VisionPath + "Image\\"))
                            {
                                string[] images = Directory.GetFiles(Vision.VisionPath + "Image\\");
                                List<string> imageStr = new List<string>(images);
                        
                                for (int i = 0; i < images.Length; i++)
                                {
                                    if (images[i].StartsWith(Vision.VisionPath + "Image\\" + OneProductV.GetNGCamName() + "拼图"))
                                    {
                                        HOperatorSet.ReadImage(out imaget, images[i]);
                                        isbde = true;
                                        break;
                                    }
                                }
                            }
                            if (!isbde)
                            {
                                hWindowControl4.HalconWindow.DispText("未创建参考图片" + OneProductV.GetNGCamName(),
                                    "window", 0, 0, "red", new HTuple(), new HTuple());
                            }
                        HWindd.OneResIamge.SetObjCross(item.Value.NGROI, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2) ;
                        if (row1.Length==0)
                        {
                            HWindd.OneResIamge.SetObjCross(item.Value.ROI, out  row1, out  col1, out  row2, out  col2);

                        }
                        if (row1.Length != 0)
                        {
                            hWindowControl3.HalconWindow.SetPart(row1, col1, row2, col2);
                            hWindowControl4.HalconWindow.SetPart(row1, col1, row2, col2);
                        }
                        HOperatorSet.Union1(item.Value.NGROI, out HObject hObject1);
                            hWindowControl3.HalconWindow.DispObj(OneProductV.GetNGImage());
                            hWindowControl4.HalconWindow.DispObj(imaget);
                            if (item.Value.oneRObjs.Count >= 0)
                            {
                                HWindd.OneResIamge.Massage = new HTuple();
                                List<string> vstd = item.Value.oneRObjs[0].dataMinMax.GetStrTextNG();
                                if (vstd.Count!=0)
                                {
                                    HTuple hTuple2 = new HTuple(vstd.ToArray());
                                    HWindd.AddMeassge(hTuple2);
                                }
                          
                                List<string> vs = item.Value.oneRObjs[0].dataMinMax.GetStrNG();
                            if (vs.Count != 0)
                            {
                                HTuple hTuple = new HTuple(vs.ToArray());

                                HWindd.AddMeassge(hTuple);
                            }
                        }
                        HOperatorSet.SetColor(hWindowControl3.HalconWindow, ColorResult.blue.ToString());
                        hWindowControl3.HalconWindow.DispObj(item.Value.ROI);
                        HOperatorSet.SetColor(hWindowControl3.HalconWindow, "red");
                        HOperatorSet.DilationCircle(hObject1, out HObject hObject5, 50);
                        HOperatorSet.AreaCenter(item.Value.NGROI, out HTuple areas, out HTuple rows, out HTuple colus);
                        hWindowControl3.HalconWindow.DispObj(hObject5);
                         hWindowControl3.HalconWindow.DispText(item.Value.NGText + "{" + item.Value.NGText + "}", "window", 0, 0, "red", new HTuple(), new HTuple());
                        break;
                       }
                    }
            }
            catch (Exception ex)
            {
                ErrForm.Show(ex, "复判显示");
            }
            HWindd.ShowImage();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (DebugCompiler.GetDoDi()!=null)
                {
                    DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.OutDischarging, false);
                }
        
                if (OneProductV != null)
                {
                    if (OneProductV.PanelID == "")
                    {
                        textBox1.Focus();
                        MessageBox.Show("SN为空,请输入SN");
                        return;
                    }
                }
                if (!button1.Enabled)
                {
                    return;
                }
                button1.Enabled = false;
                hWindowControl1.Focus();
                if (TrayImage.OK)
                {
                    label1.Text = "OK";
                    label1.BackColor = Color.Green;
                }
                else
                {
                    label1.Text = "NG";
                    label1.BackColor = Color.Red;
                }
                treeView1.Nodes.Clear();
                treeView2.Nodes.Clear();
                 TrayImage.UserRest = true;
                  OneProductV = null;
                    UserFormulaContrsl.WeirtAll(TrayImage);
                    WeirtTray(TrayImage);
                    for (int i = 0; i < TrayImage.Count; i++)
                    {
                        if (TrayImage.GetDataVales()[i].NotNull)
                        {
                            TrayImage.GetDataVales()[i].Dispose();
                            try
                            {
                                if (File.Exists(ProjectINI.TempPath + "\\ResetImageData\\" + TrayImage.GetDataVales()[i].PanelID + ".txt"))
                                {
                                    File.Delete(ProjectINI.TempPath + "\\ResetImageData\\" + TrayImage.GetDataVales()[i].PanelID + ".txt");
                                }
                                if (File.Exists(ProjectINI.TempPath + "\\ResetImage\\" + TrayImage.GetDataVales()[i].PanelID + ".txt"))
                                {
                                    File.Delete(ProjectINI.TempPath + "\\ResetImage\\" + TrayImage.GetDataVales()[i].PanelID + ".txt");
                                }
                            }
                            catch (Exception)
                            {
                            }
                    }
                  }
                    timer1.Start();
                   TrayImage = null;
                    Task task = new Task(() =>
                    {
                        try
                        {
                            Thread.Sleep(1000);
                            DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.OutDischarging, true);
                        }
                        catch (Exception) { }
                    });
                    task.Start();
            }
            catch (Exception ex) {
                ErrForm.Show(ex, "复判提交");
            }
        }
        /// <summary>
        /// 更新复判托盘
        /// </summary>
        public void DequeueData()
        {
            try
            {
                if (TrayImageTs.Count == 0)
                {
                    Thread.Sleep(500);
                    this.Hide();
                    return;
                }
                TrayImage = TrayImageTs.Dequeue();
                foreach (var item in TrayImage.GetDataVales())
                {
                     try
                     {
                        if (item.NotNull)
                        {
                            if (item.OK)
                            {
                                continue;
                            }
                            if (!File.Exists(ProjectINI.TempPath + "\\ResetImage\\" + item.PanelID + ".txt"))
                            {
                                    item.DeviceName = DebugCompiler.Instance.DeviceNameText;
                                    ProjectINI.ClassToJsonSave(item,
                                    ProjectINI.TempPath + "\\ResetImage\\" + item.PanelID);
                            }
                        }
                        }
                        catch (Exception)
                        {}
                }
                trayDatas1.Initialize(TrayImage);
                //trayDatas1.SetTray(TrayImage);
                TrayImage.SetITrayRobot(trayDatas1);
                trayDatas1.UpData();
                this.Invoke(new Action(() =>
                {
                    try
                    {
                        panel3.Visible = Vision.Instance.RestT;
                        toolStripLabel1.Text = "复判窗口剩余:" + TrayImageTs.Count;
                        if (TrayImage.ImagePlus != null)
                        {
                            HWindd.SetImaage(TrayImage.ImagePlus);
                        }
                        foreach (var item in TrayImage.GetDataVales())
                        {
                            if (item.Done || item.OK)
                            {
                                continue;
                            }

                            OneProductV = item;
                            button1.Enabled = true;
                            trayDatas1.SelesItem(OneProductV.TrayLocation);
                            treeView1.Nodes.Clear();
                            treeView2.Nodes.Clear();
                            foreach (var itemdt in OneProductV.ListCamsData)
                            {
                                TreeNode treeNode = treeView1.Nodes.Add(itemdt.Key);
                                treeNode.Tag = itemdt.Value;
                                TreeNode treeNodeOK = treeView2.Nodes.Add(itemdt.Key);
                                treeNodeOK.Tag = itemdt.Value;
                                foreach (var itemdte in itemdt.Value.AllCompObjs.DicOnes)
                                {
                                    TreeNode treeNode1 = treeNodeOK.Nodes.Add(itemdte.Key);
                                    treeNode1.Tag = itemdte.Value;
                                    treeNode1.ImageIndex = 6;
                                }
                                foreach (var itemdte in itemdt.Value.NGObj.DicOnes)
                                {
                                    if (!itemdte.Value.aOK)
                                    {
                                        TreeNode treeNode1 = treeNode.Nodes.Add(itemdte.Key);
                                        treeNode1.Tag = itemdte.Value;
                                        treeNode1.ImageIndex = 5;
                                    }
                                }
                                treeNodeOK.Expand();
                                treeNode.Expand();
                            }
                            break;
                        }
                        label1.Text = "NG";
                        label1.BackColor = Color.Red;
                        if (OneProductV != null)
                        {
                            if (OneProductV.GetNGImage() != null)
                            {
                                HWindd.SetImaage(OneProductV.GetNGImage());
                            }
                            UpData();
                        }
                        UICon.SwitchToThisWindow(RestObjImage.RestObjImageFrom.Handle, true);
                        RestObjImage.RestObjImageFrom.Show();
                    }
                    catch (Exception ex)
                    {
                        AlarmText.AddTextNewLine("复判窗口:" + ex.StackTrace, Color.Red);
                    }
                }));
            }
            catch (Exception)
            {
            }
        }

        private void RestObjImage_Load(object sender, EventArgs e)
        {
            try
            {
                if (Vision.Instance.RestT)
                {
                    toolStripButton3.Text = "单个点";
                }
                else
                {
                    toolStripButton3.Text = "单个原件";
                } 
                this.restOneComUserControl1.EventShowObj += RestOneComUserControl1_EventShowObj;
                Directory.CreateDirectory(ProjectINI.TempPath + "\\ResetImage\\");
                Directory.CreateDirectory(ProjectINI.TempPath + "\\ResetImageData\\");
                Thread thread = new Thread(() => {
                while (true)
                {
                        Thread.Sleep(100);
                        try
                        {

                            if (this.Visible)
                            {   if (TrayImage == null)
                                {
                                    DequeueData();
                                }
                                string paths = ProjectINI.TempPath + "\\ResetImageData\\" + OneProductV.PanelID + ".txt";
                                if (File.Exists(paths))
                                {
                                    if (ProjectINI.ReadPathJsonToCalssEX(paths, out OneDataVale oneDataVale))
                                    {
                                        OneProductV = oneDataVale;
                                        TrayImage.GetDataVales()[OneProductV.TrayLocation - 1] = OneProductV;
                                        this.Invoke(new Action(() =>
                                        {
                                            RestObjImage_KeyDown(null, new KeyEventArgs(Keys.Space));
                                        }));
                                        File.Delete(paths);
                                        if (File.Exists(ProjectINI.TempPath + "\\ResetImage\\" + OneProductV.PanelID + ".txt"))
                                        {
                                            File.Delete(ProjectINI.TempPath + "\\ResetImage\\" + OneProductV.PanelID + ".txt");
                                        }
                                        if (File.Exists(ProjectINI.TempPath + "\\ResetImage\\Time\\" + OneProductV.PanelID + ".txt"))
                                        {
                                            File.Delete(ProjectINI.TempPath + "\\ResetImage\\Time\\" + OneProductV.PanelID + ".txt");
                                        }
                                    }
                                }
                                else
                                {
                                    string PATH = ProjectINI.TempPath + "\\ResetImage\\Time\\";
                                    if (Directory.Exists(PATH))
                                    {
                                        string[] imagef = Directory.GetFiles(PATH);
                                        for (int i = 0; i < imagef.Length; i++)
                                        {
                                            if (File.Exists(imagef[i]))
                                            {
                                                File.Delete(imagef[i]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                 }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
            }
        }

        private void RestOneComUserControl1_EventShowObj(int objName)
        {
            try
            {
                UpData();
            }
            catch (Exception)
            {
            }
        }

        private void RestObjImage_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

        private void RestObjImage_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
              
                if (OneProductV != null)
                {
                    if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Back)
                    {
                        this.hWindowControl1.Focus();
                        if (TrayImage == null)
                        {
                            return;
                        }
                        if (OneProductV.Done)
                        {
                            if (OneProductV.OK)
                            {
                                if (OneProductV.AutoOK != OneProductV.OK)
                                {
                                    RecipeCompiler.AddRlsNumber();
                                }
                            }
                            treeView1.Nodes.Clear();
                            treeView2.Nodes.Clear();
                            for (int i = 0; i < TrayImage.Count; i++)
                            {
                                if (TrayImage.GetDataVales()[i] != null)
                                {
                                    if (TrayImage.GetDataVales()[i].OK)
                                    {
                                        continue;
                                    }
                                    if (!TrayImage.GetDataVales()[i].Done)
                                    {
                                     
                                        OneProductV = TrayImage.GetDataVales()[i];
                                        trayDatas1.SelesItem(OneProductV.TrayLocation);
                                        foreach (var item in OneProductV.ListCamsData)
                                        {
                                            TreeNode treeNode = treeView1.Nodes.Add(item.Key);
                                            foreach (var itemdt in item.Value.NGObj.DicOnes)
                                            {
                                                if (!itemdt.Value.aOK)
                                                {
                                                    TreeNode treeNode1 = treeNode.Nodes.Add(itemdt.Key);
                                                    treeNode1.Tag = itemdt.Value;
                                                    treeNode1.ImageIndex = 6;
                                                }
                                            }
                                            TreeNode treeNodeOK = treeView2.Nodes.Add(item.Key);

                                            foreach (var itemdt in item.Value.AllCompObjs.DicOnes)
                                            {
                                                TreeNode treeNode1 = treeNodeOK.Nodes.Add(itemdt.Key);
                                                treeNode1.Tag = itemdt.Value;
                                                treeNode1.ImageKey = "OK";
                                            }
                                            treeNodeOK.Expand();
                                            treeNode.ImageIndex = 5;
                                            treeNode.Expand();
                                            treeNode.Tag = item.Value;
                                        }
                                        label1.Text = "NG";
                                        label1.BackColor = Color.Red;
                                        if (OneProductV.GetNGImage() != null)
                                        {
                                            HWindd.SetImaage(OneProductV.GetNGImage());
                                        }
                                        UpData();
                                        break;
                                    }
                                }
                            }
                            if (TrayImage.Done)
                            {
                                button1_Click(null, null);
                            }
                            return;
                        }
                        if (e.KeyCode == Keys.Space)
                        {
                            if (TrayImage.Done)
                            {
                                button1_Click(null, null);
                                return;
                            }
                            if (Vision.Instance.RestT)
                            {
                                restOneComUserControl1.SetRest(-1);
                            }
                            else
                            {
                                OneProductV.Done = true;
                                OneProductV.OK = true;
                            }
                        }
                        else
                        {
                            if (TrayImage.Done)
                            {
                                button1_Click(null, null);
                                return;
                            }
                            if (Vision.Instance.RestT)
                            {
                                restOneComUserControl1.SetRest(0);
                            }
                            else
                            {
                                OneProductV.Done = true;
                                OneProductV.OK = false;
                            }
                        }
                        if (OneProductV.OK)
                        {/*.SetNumberValue(OneProductV.TrayLocation, OneProductV.OK);*/
                            TrayImage.GetITrayRobot().UpData();
                            label1.Text = "OK";
                            label1.BackColor = Color.Green;
                        }
                        else
                        {
                            label1.Text = "NG";
                            label1.BackColor = Color.Red;
                        }
                    }
                    else if (e.KeyCode == Keys.D1)
                    {
                        restOneComUserControl1.SetRest(1);
                    }
                    else if (e.KeyCode == Keys.D2)
                    {
                        restOneComUserControl1.SetRest(2);
                    }
                    else if (e.KeyCode == Keys.D3)
                    {
                        restOneComUserControl1.SetRest(3);
                    }
                    else if (e.KeyCode == Keys.D4)
                    {
                        restOneComUserControl1.SetRest(4);
                    }
                    else if (e.KeyCode == Keys.D5)
                    {
                        restOneComUserControl1.SetRest(5);
                    }
                    else if (e.KeyCode == Keys.D6)
                    {
                        restOneComUserControl1.SetRest(6);
                    }
                    else if (e.KeyCode == Keys.D7)
                    {
                        restOneComUserControl1.SetRest(7);
                    }
                    else
                    {
                        return;
                    }
                    UpData();
                }
            }
            catch (Exception ex)
            {
               ErrForm.Show(ex,"复判按键");

            }
        }

        private void RestObjImage_Resize(object sender, EventArgs e)
        {
            try
            {
                if (HWindd != null)
                {
                    HWindd.ShowImage();
                }
            }
            catch (Exception)
            { }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int i = 0;

                UpData();
                HWindd.ShowImage();
            }
            catch (Exception ex)
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //button1.Enabled = true;
                timer1.Stop();
                if (TrayImageTs.Count == 0)
                {
                    try
                    {
                        //this.Hide();
                        Thread.Sleep(1000);
                        DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.OutDischarging, false);
                    }
                    catch (Exception) { }
                    //TrayImage = null;
                }
                else
                {
                    //DequeueData();
                    try
                    {
                        Thread.Sleep(2000);
                        DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.OutDischarging, false);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(Application.StartupPath + "\\截取屏幕\\" +  DateTime.Now.ToString("yyyy年M月d日"));
                Bitmap bitmap = UICon.GetScreenCapture();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Application.StartupPath + @"\截取屏幕\";
                saveFileDialog.Filter = "图像|*.jpg";
                string timeStr = DateTime.Now.ToString("HH时mm分ss秒");
                saveFileDialog.FileName = timeStr;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bitmap.Save(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //需添加using System.Runtime.InteropServices;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void toolStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture(); //释放鼠标捕捉
                                  //发送左键点击的消息至该窗体(标题栏)
                SendMessage(Handle, 0xA1, 0x02, 0);
            }
            if (e.Clicks == 2)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (this.WindowState != FormWindowState.Maximized)
                    {
                        SetFormMax(this);
                    }
                    else
                    {
                        this.WindowState = FormWindowState.Minimized;
                    }
                }
            }
        }

        public virtual void SetFormMax(Form frm)
        {
            frm.Top = 0;
            frm.Left = 0;
            frm.Width = Screen.PrimaryScreen.WorkingArea.Width;
            frm.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                OneProductV.PanelID = textBox1.Text.Trim();
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("取消提交将不处理当前产品！", "是否取消提交？", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (TrayImageTs.Count == 0)
                    {
                        this.Hide();
                    }
                    else
                    {
                        //DequeueData();
                    } 
                    hWindowControl1.Focus();
                    if (TrayImage == null)
                    {
                    }
                    else
                    {
                        TrayImage.Dispose();
                        TrayImage.Clear();
                        TrayImage = null;
                    }

                }
            }
            catch (Exception)
            {
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
               propertyGrid1.SelectedObject=    e.Node.Tag;
                UpData();
                HWindd.ShowImage();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private OneNGDataMinMaxControl oneNGDataMinMaxControl;

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                OneComponent one = e.Node.Tag as OneComponent;
                if (oneNGDataMinMaxControl == null)
                {
                    oneNGDataMinMaxControl = new OneNGDataMinMaxControl();
                    panel5.Controls.Add(oneNGDataMinMaxControl);
                }
                panel5.AutoScroll = true;
                oneNGDataMinMaxControl.Dock = DockStyle.Top;
                oneNGDataMinMaxControl.UpDataMax(one.oneRObjs[0].dataMinMax);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RestObjImage_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (Vision.Instance.RestWait)
                {
                    if (this.Visible)
                    {
                        Project.DebugF.IO.DODIAxis.RresOK = false;
                        Project.DebugF.IO.DODIAxis.RresWait = true;
                    }
                    else
                    {
                        Project.DebugF.IO.DODIAxis.RresOK = false;
                        Project.DebugF.IO.DODIAxis.RresWait = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrForm.Show(ex);
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            SVisionForm sVisionForm = new SVisionForm();
            sVisionForm.Show();
            sVisionForm.Finde(textBox1.Text);

        }
    }
}