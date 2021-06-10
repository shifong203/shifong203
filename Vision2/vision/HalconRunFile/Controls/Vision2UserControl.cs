using HalconDotNet;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.Project.Mes;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class Vision2UserControl : System.Windows.Forms.UserControl, HalconRun.IUpDataVisionWindow
    {
        public Vision2UserControl()
        {
            InitializeComponent();
        }
        HalconRun halcon;

        public void Setprat(int row,int col,int row2,int col2)
        {
            try
            {
               HOperatorSet.SetPart(halcon.hWindowHalcon(), row, col, row2, col2);
               Rectangle rect = this.visionUserControl1.ImagePart;
                rect.X = row;
                rect.Y = col;
            }
            catch (Exception)
            {
            }
         
        }
        public void UPOneImage(OneResultOBj oneImage)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<OneResultOBj>(UPOneImage), oneImage);
                    return;
                }
                string names = oneImage.RunID.ToString() + ":";
                if (Vision.GetSaveImageInfo(halcon.Name).ISImages)
                {
                    if (panel2 == null)
                    {
                        panel2 = new Panel();
                        this.Controls.Add(panel2);
                    }
                    panel2.Update();
                    if (halcon.RunName.Count >= oneImage.RunID)
                    {
                        if (halcon.RunName[oneImage.RunID - 1] == "")
                        {
                        }
                        names += halcon.RunName[oneImage.RunID - 1].ToString();
                    }
                    double sel = (double)halcon.Width / (double)halcon.Height;
                    panel2.Width = (int)(halcon.Form2Heigth * sel);
                    panel2.Dock = DockStyle.Right;
                    panel2.AutoScroll = true;
                    panel2.AutoScrollMinSize = new Size(20, 1000);
                    HWindowControl hWindowControl2;
                    GroupBox groupBox2;
                    if (panel2.Controls.ContainsKey("Image." + oneImage.RunID))
                    {
                        groupBox2 = panel2.Controls["Image." + oneImage.RunID] as GroupBox;

                        hWindowControl2 = groupBox2.Controls[0] as HWindowControl;
                        hWindowControl2.Tag = oneImage;
                    }
                    else
                    {
                        groupBox2 = new GroupBox();
                        groupBox2.Dock = DockStyle.Top;
                        hWindowControl2 = new HWindowControl();
                        hWindowControl2.HMouseDown += HWindowControl_HMouseDownD;
                        hWindowControl2.Tag = oneImage;
                        groupBox2.Text = groupBox2.Name = "Image." + oneImage.RunID;
                        if (halcon.RunName.Count > oneImage.RunID - 1)
                        {
                            hWindowControl2.Name = oneImage.RunID + ":" + halcon.RunName[oneImage.RunID - 1];
                        }
                        else
                        {
                            hWindowControl2.Name = oneImage.RunID + ":";
                        }
                        groupBox2.Height = halcon.Form2Heigth;
                        hWindowControl2.Dock = DockStyle.Fill;
                        groupBox2.Controls.Add(hWindowControl2);
                        panel2.Controls.Add(groupBox2);
                    }
                    Vision.SetFont(hWindowControl2.HalconWindow);
                    HOperatorSet.SetDraw(hWindowControl2.HalconWindow, "margin");
                    HSystem.SetSystem("flush_graphic", "false");
                    HOperatorSet.ClearWindow(hWindowControl2.HalconWindow);
                    HSystem.SetSystem("flush_graphic", "true");
                    if (Vision.ObjectValided(oneImage.Image))
                    {
                        HOperatorSet.GetImageSize(oneImage.Image, out HTuple width, out HTuple heigth);
                        HOperatorSet.SetPart(hWindowControl2.HalconWindow, 0, 0, heigth, width);
                        HOperatorSet.DispObj(oneImage.Image, hWindowControl2.HalconWindow);
                    }
                    OneResultOBj halconResult = hWindowControl2.Tag as OneResultOBj;
                    if (halconResult != null)
                    {
                        halconResult.ShowAll(hWindowControl2.HalconWindow);
                    }
                    panel2.Refresh();
                }
                void HWindowControl_HMouseDownD(object sender, HMouseEventArgs e)
                {
                    try
                    {
                        OneResultOBj oneResultOBj = (sender as HWindowControl).Tag as OneResultOBj;
                        //halcon.HobjClear();
                        halcon.SetResultOBj(oneResultOBj);
                        halcon.GetOneImageR().ShowAll(halcon.hWindowHalcon());
                        if (e.Clicks == 2)
                        {
                            halcon.Image(halcon.GetOneImageR().Image.Clone());
                        }
                        //HalconResult halconResultT = (sender as HWindowControl).Tag as HalconResult;
                        HWindowControl hWindowControl = sender as HWindowControl;
                        if (oneResultOBj != null)
                        {
                            if (Vision.ObjectValided(oneResultOBj.Image))
                            {
                                HOperatorSet.GetImageSize(oneResultOBj.Image, out HTuple width, out HTuple heigth);
                                HOperatorSet.SetPart(hWindowControl.HalconWindow, 0, 0, heigth, width);
                                HOperatorSet.DispObj(oneResultOBj.Image, hWindowControl.HalconWindow);
                            }
                            oneResultOBj.ShowAll(hWindowControl.HalconWindow);
                        }
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
        public void PanelClear()
        {
            panel2.Controls.Clear();
        }
        public void UPImage(OneResultOBj HResult, int runid, Dictionary<string, bool> listResultBool)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<OneResultOBj, int, Dictionary<string, bool>>(UPImage), HResult, runid, listResultBool);
                    return;
                }
                //HResult.Done = false;
                if (runid == 0)
                {
                    runid = 1;
                }
                string names = runid.ToString() + ":";
                if (halcon.MaxRunID != 0)
                {
                    if (vision.Vision.GetSaveImageInfo(halcon.Name).ISImages)
                    {
                        if (panel2 == null)
                        {
                            panel2 = new Panel();
                            this.Controls.Add(panel2);
                        }
                        panel2.Update();
                        if (runid == 1)
                        {
                            panel2.Controls.Clear();
                        }
                        if (halcon.RunName.Count >= runid)
                        {
                            if (halcon.RunName[runid - 1] == "")
                            {
                            }
                            names += halcon.RunName[runid - 1].ToString();
                        }
                        double sel = (double)halcon.Width / (double)halcon.Height;
                        panel2.Width = (int)(halcon.Form2Heigth * sel);
                        panel2.Dock = DockStyle.Right;
                        panel2.AutoScroll = true;
                        panel2.AutoScrollMinSize = new Size(20, 1000);
                        HWindowControl hWindowControl2;
                        GroupBox groupBox2;
                        if (panel2.Controls.ContainsKey("Image." + runid))
                        {
                            groupBox2 = panel2.Controls["Image." + runid] as GroupBox;

                            hWindowControl2 = groupBox2.Controls[0] as HWindowControl;
                            hWindowControl2.Tag = HResult;
                        }
                        else
                        {
                            groupBox2 = new GroupBox();
                            groupBox2.Dock = DockStyle.Top;
                            hWindowControl2 = new HWindowControl();
                            hWindowControl2.HMouseDown += HWindowControl_HMouseDownD;
                            hWindowControl2.Tag = HResult;
                            groupBox2.Text = groupBox2.Name = "Image." + runid;
                            if (halcon.RunName.Count > runid - 1)
                            {
                                hWindowControl2.Name = runid + ":" + halcon.RunName[runid - 1];
                            }
                            else
                            {
                                hWindowControl2.Name = runid + ":";
                            }
                            groupBox2.Height = halcon.Form2Heigth;
                            hWindowControl2.Dock = DockStyle.Fill;
                            groupBox2.Controls.Add(hWindowControl2);
                            panel2.Controls.Add(groupBox2);
                        }
                        Vision.SetFont(hWindowControl2.HalconWindow);
                        HOperatorSet.SetDraw(hWindowControl2.HalconWindow, "margin");
                        HSystem.SetSystem("flush_graphic", "false");
                        HOperatorSet.ClearWindow(hWindowControl2.HalconWindow);
                        HSystem.SetSystem("flush_graphic", "true");
                        if (Vision.ObjectValided(HResult.Image))
                        {
                            HOperatorSet.GetImageSize(HResult.Image, out HTuple width, out HTuple heigth);
                            HOperatorSet.SetPart(hWindowControl2.HalconWindow, 0, 0, heigth, width);
                            HOperatorSet.DispObj(HResult.Image, hWindowControl2.HalconWindow);
                        }
                        OneResultOBj halconResult = hWindowControl2.Tag as OneResultOBj;
                        if (halconResult != null)
                        {
                            halconResult.ShowAll(hWindowControl2.HalconWindow);
                        }
                        panel2.Refresh();
                    }
                    else
                    {
                        if (HResult != null)
                        {
                            HResult.Image.Dispose();
                        }
                    }
               
                    void HWindowControl_HMouseDownD(object sender, HMouseEventArgs e)
                    {
                        try
                        {
                            OneResultOBj halconResultT = (sender as HWindowControl).Tag as OneResultOBj;
                            HWindowControl hWindowControl = sender as HWindowControl;
                            if (halconResultT != null)
                            {
                                halcon.HobjClear();
                                halcon.SetResultOBj(halconResultT);
                                halconResultT.ShowAll(halcon.hWindowHalcon());
                                if (e.Clicks == 2)
                                {
                                    halcon.Image(halcon.GetOneImageR().Image.Clone());
                                }
                                if (Vision.ObjectValided(HResult.Image))
                                {
                                    HOperatorSet.GetImageSize(HResult.Image, out HTuple width, out HTuple heigth);
                                    HOperatorSet.SetPart(hWindowControl.HalconWindow, 0, 0, heigth, width);
                                    HOperatorSet.DispObj(HResult.Image, hWindowControl.HalconWindow);
                                }
                                halconResultT.ShowAll(hWindowControl.HalconWindow);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    Project.formula.UserFormulaContrsl.StaticAddResult(runid, names, halcon.TrayRestData);
                }
                else
                {
                    if (panel2 != null)
                    {
                        panel2.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        public HWindowControl GetNmaeWindowControl(string name)
        {
            try
            {
                if (panel2 != null)
                {
                    for (int i = 0; i < panel2.Controls.Count; i++)
                    {
                        if (panel2.Controls[i].Controls[0].Name == name)
                        {
                            return panel2.Controls[i].Controls[0] as HWindowControl;
                        }
                    }
                    for (int i = 0; i < panel2.Controls.Count; i++)
                    {
                        if (panel2.Controls[i].Text == name)
                        {
                            return panel2.Controls[i].Controls[0] as HWindowControl;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
        public List<string> ListImagePath = new List<string>();

        string path = "";
        private void imageFielNumber_Click(object sender, EventArgs e)
        {
            try
            {
                imageFielNumber.HideDropDown();
                if (halcon == null)
                {
                    MessageBox.Show("未关联执行程序");
                    return;
                }
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择图片文件可多选";
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";

                if (!path.StartsWith(Vision.Instance.DicSaveType[halcon.Name].SavePath))
                {
                }
                else
                {
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(path);
                    openFileDialog.FileName = Path.GetFileName(path);
                }
                //DialogResult dialogResult= openFileDialog.ShowDialog();
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                path = openFileDialog.FileName;
                ListImagePath.AddRange(openFileDialog.FileNames);
                if (imageFielNumber.DropDownItems.Count >= 6)
                {
                    imageFielNumber.DropDownItems.RemoveAt(5);
                }
                for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                {
                    ToolStripItem toolStrip = imageFielNumber.DropDownItems.Add(openFileDialog.FileNames[i]);
                    imageFielNumber.DropDownItems.Insert(0, toolStrip);
                    toolStrip.Click += ToolStrip_Click;
                    toolStrip.PerformClick();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        void ToolStrip_Click(object sender, EventArgs e)
        {
            if (halcon == null)
            {
                MessageBox.Show("未关联执行程序");
                return;
            }
            ToolStripItem eet = (ToolStripItem)sender;
            halcon.ReadImage(eet.Text);
            halcon.ShowImage();
            this.Text = eet.Text + "|" + ListImagePath.Count + "=>";
        }

        public new void Focus()
        {
            Vision.Instance.ShowFocusRun(halcon.Name);
            this.visionUserControl1.Focus();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (halcon == null)
            {
                MessageBox.Show("未关联执行程序");
                return;
            }
            halcon.HobjClear();
            halcon.ShowObj();

        }

        private void 保存图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (halcon == null)
            {
                MessageBox.Show("未关联执行程序");
                return;
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "请选择保存路径";      //文件框名称
            //    openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
            saveFile.Filter = Vision.Instance.DicSaveType[halcon.Name].SaveImageType + "|*." + Vision.Instance.DicSaveType[halcon.Name].SaveImageType + "|BMP|*.bmp|tif|*.tif|tiff|*.tiff|hobj|*.hobj|所有文件|*.*";   //筛选器
            if (Directory.Exists(@"C:\Users\Eros\Desktop"))
            {
                saveFile.InitialDirectory = @"C:\Users\Eros\Desktop";  //默认路径
            }
            saveFile.ShowDialog();    //弹出对话框
            string path = saveFile.FileName;
            if (path == "") return;    //地址为空返回
            try
            {
                string da = Path.GetFileName(path).Split('.')[1].ToLower();
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                HOperatorSet.WriteImage(halcon.Image(), da, 0, path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void 导入XLDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (halcon == null)
            {
                MessageBox.Show("未关联执行程序");
                return;
            }
            try
            {
                halcon.GetFileNameDXF();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 执行完成事件
        /// </summary>
        /// <param name="hRun"></param>
        public HObject ShowObj(HalconRun hRun, string objName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Func<HalconRun, string, HObject>(ShowObj), hRun, objName);
            }
            this.Invoke(new MethodInvoker(sd));
            void sd()
            {
                try
                {

                    if (hRun.GetCam() != null && hRun.GetCam().Grabbing)
                    {
                        toolStripButton6.Text = "停止";
                    }
                    else
                    {
                        toolStripButton6.Text = "实时采图";
                    }

                }
                catch (Exception exx)
                {
                }

            }
            return null;
        }



        /// <summary>
        /// 更新绑定程序
        /// </summary>
        /// <param name="hRun"></param>
        public void UpHalcon(HalconRun hRun = null)
        {
            if (this.Created)
            {
                this.Invoke(new Action(() =>
                {
                    dse(hRun);
                }));
            }
            else
            {
                dse(hRun);
            }
        }
        void dse(HalconRun hRun = null)
        {
            try
            {
                if (hRun != null)
                {
                    hRun.EventShowObj -= ShowObj;
                    halcon = hRun;
                    halcon.EventShowObj += ShowObj;
                }
                if (halcon == null)
                {
                    halcon = Vision.GetRunNameVision();
                    halcon.EventShowObj += ShowObj;
                }
                HOperatorSet.SetLineWidth(this.visionUserControl1.HalconWindow, Vision.Instance.LineWidth);
                Vision.SetFont(this.visionUserControl1.HalconWindow);

                Vision.Instance.SetFocusRunHalconName(halcon.Name);
                HOperatorSet.GetImageSize(halcon.Image(), out HTuple width, out HTuple heigth);
                halcon.hWindowHalcon(this.visionUserControl1.HalconWindow);
                halcon.SetWindow(this);
                visionUserControl1.UpHalcon(halcon);
                if (width.Length!= 0)
                {
                    halcon.Width = width.TupleInt();
                    halcon.Height = heigth.TupleInt();
                }

                this.Focus();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        Panel panel2;
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            try
            {
                if (halcon.GetCam().Grabbing)
                {
                    if (halcon.GetCam().IsCamConnected)
                    {
                        halcon.GetCam().Stop();
                    }

                    this.toolStripButton6.Text = "实时采图";
                }
                else
                {
                    halcon.GetCam().Straing(halcon);

                    this.toolStripButton6.Text = "停止";
                }
            }
            catch (Exception)
            {
            }

        }


        private void visionUserControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {

                if (halcon == null)
                {
                    return;
                }
                if (!halcon.WhidowAdd)
                {
                    return;
                }
                if (halcon.Drawing)
                {
                    return;
                }
                if (visionUserControl1.meuseBool)
                {
                    //halcon.GetOneImageR().ShowAll(visionUserControl1.HalconWindow);
                    return;
                }
                Application.DoEvents();
                visionUserControl1.HalconWindow.GetMposition(out int rowi, out int coli, out int button1);
                if (查看区域细节ToolStripMenuItem.Checked)
                {
                    halcon.GetOneImageR().ShowAll(visionUserControl1.HalconWindow, rowi, coli, 查看区域细节ToolStripMenuItem.Checked);
                }

                if (true)
                {
                    halcon.CoordinatePXY.ShowCoordinate(halcon);
                    switch (halcon.CoordinatePXY.CoordinateTeyp)
                    {
                        case Coordinate.Coordinate_Type.XYU2D:
                            Vision2.vision.Coordinate.CpointXY coordinate2D = halcon.CoordinatePXY.GetPointRctoYX(rowi, coli);
                            ImageXY.Text = "X" + coordinate2D.X.ToString("0.00") + ",Y" + coordinate2D.Y.ToString("0.00")
                                + "(Row" + rowi.ToString("0000") + ",Col" + coli.ToString("0000") + ")";

                            //coordinate2D = null;
                            break;

                        default:

                            ImageXY.Text = "Row(Y) = " + rowi.ToString("0000") + ",Col(X) = " + coli.ToString("0000");
                            break;
                    }
                }
                if (halcon.Height > rowi && halcon.Width > coli && rowi > 0 && coli > 0)
                {
                    HOperatorSet.GetGrayval(halcon.Image(), rowi, coli, out HTuple Grey);
                    if (Grey.Length == 3)
                    {
                        ImageRGB.Text = " RGB:" + Grey.TupleSelect(0).D.ToString("000") + "," + Grey.TupleSelect(1).D.ToString("000") + "," + Grey.TupleSelect(2).D.ToString("000");
                    }
                    else if (Grey.Length == 1)
                    {
                        ImageRGB.Text = "B:" + Grey.D.ToString("000");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private bool Cambueys = false;

        private void visionUserControl1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control)
                {
                    halcon.WhidowAdd = false;
                }
                if (Vision2.ErosProjcetDLL.Project.ProjectINI.GetUserJurisdiction("管理") && e.Control)
                {


                    if (e.KeyCode == Keys.Left)
                    {
                        AxisX.JogAdd(true, toolStripButton4.Checked, 1);
                    }
                    else if (e.KeyCode == Keys.Right)
                    {
                        AxisX.JogAdd(false, toolStripButton4.Checked, 1);
                    }
                    else if (e.KeyCode == Keys.Down)
                    {
                        AxisY.JogAdd(false, toolStripButton4.Checked, 1);
                    }
                    else if (e.KeyCode == Keys.Up)
                    {
                        AxisY.JogAdd(true, toolStripButton4.Checked, 1);
                    }
                }
                else if (Vision2.ErosProjcetDLL.Project.ProjectINI.GetUserJurisdiction("管理") && e.Alt)
                {
                    if (e.KeyCode == Keys.Up)
                    {
                        AxisZ.JogAdd(false, toolStripButton4.Checked, 1);
                    }
                    else if (e.KeyCode == Keys.Down)
                    {
                        AxisZ.JogAdd(true, toolStripButton4.Checked, 1);
                    }
                }

            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 按键执行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (halcon == null)
                {
                    MessageBox.Show("未关联执行程序");
                    return;
                }
                if (e.KeyData == Keys.ControlKey)
                {
                    halcon.WhidowAdd = true;
                }
                if (Keys.F5 == e.KeyCode)
                {
                    tcbRunType.SelectedIndex = 0;
                    if (Cambueys) return;
                    string ERR = "";

                    if (Vision.GetSaveImageInfo(halcon.Name).ReadCamName != "")
                    {
                        ErosSocket.ErosConLink.StaticCon.SetLingkValue(Vision.GetSaveImageInfo(halcon.Name).ReadCamName, 1.ToString(), out ERR);
                    }
                    else
                    {
                        if (halcon.GetCam() != null)
                        {
                            halcon.GetCam().Key = "All";
                            halcon.ReadCamImage();
                        }
                    }
                }
                else if (Keys.F6 == e.KeyCode)
                {
                    tcbRunType.SelectedIndex = 1;
                    if (Cambueys) return;
 
                }
                else if (Keys.F7 == e.KeyCode)
                {
                    if (!ProjectINI.DebugMode)
                    {
                        return;
                    }
                    tcbRunType.SelectedIndex = 2;
                    if (Cambueys) return;
                    //halcon.ShowVision(1, 99);
                }
                else if (Keys.F8 == e.KeyCode && e.Alt)
                {
                    if (halcon.GetCam() != null)
                    {
                        if (halcon.GetCam().Grabbing)
                        {
                            halcon.GetCam().Stop();
                        }
                        else
                        {
                            halcon.GetCam().Straing(halcon);
                        }
                    }
                }
                else if (Keys.F8 == e.KeyCode)
                {

                    tcbRunType.SelectedIndex = 3;
                    if (Cambueys) return;
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            Cambueys = true;
                            halcon.GetCam().Key = "One";
                            halcon.ReadCamImage("One",0);
                            halcon.ShowImage();
                        }
                        catch (Exception)
                        {
                        }
                        Cambueys = false;
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                else if (Keys.F9 == e.KeyCode)
                {
                    tcbRunType.SelectedIndex = 4;
                    if (ProjectINI.DebugMode)
                    {
                        ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(Vision.Instance.NextSetp, true);
                    }
                    return;
                }
                else if (Keys.F10 == e.KeyCode)
                {
                    ProjectINI.SelpMode = true;
                }
                else if (ProjectINI.GetUserJurisdiction("管理") && (e.Control || e.Alt))
                {
                    if (e.KeyCode == Keys.Left)
                    {
                        AxisX.Stop();
                        AxisY.Stop();
                        if (AxisZ != null)
                        {
                            AxisZ.Stop();
                        }
                        if (AxisU != null)
                        {
                            AxisU.Stop();
                        }
                    }
                    else if (e.KeyCode == Keys.Right)
                    {
                        AxisX.Stop();
                        AxisY.Stop();
                        if (AxisZ!=null)
                        {
                            AxisZ.Stop();
                        }
                        if (AxisU != null)
                        {
                            AxisU.Stop();
                        }
                    }
                    else if (e.KeyCode == Keys.Down)
                    {
                        AxisX.Stop();
                        AxisY.Stop();
                        if (AxisZ != null)
                        {
                            AxisZ.Stop();
                        }
                        if (AxisU != null)
                        {
                            AxisU.Stop();
                        }
                    }
                    else if (e.KeyCode == Keys.Up)
                    {
                        AxisX.Stop();
                        AxisY.Stop();
                        if (AxisZ != null)
                        {
                            AxisZ.Stop();
                        }
                        if (AxisU != null)
                        {
                            AxisU.Stop();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        ErosSocket.DebugPLC.IAxis AxisX;
        ErosSocket.DebugPLC.IAxis AxisY;
        ErosSocket.DebugPLC.IAxis AxisZ;
        ErosSocket.DebugPLC.IAxis AxisU;
        private void Vision2UserControl_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(() =>
            {
                while (!this.IsDisposed)
                {
                    try
                    {
                        Thread.Sleep(10);
                        if (halcon != null)
                        {
                            if (Vision.GetSaveImageInfo(halcon.Name) == null)
                            {
                                continue;
                            }
                            if (AxisX == null)
                            {
                                AxisX = Project.DebugF.DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(Vision.GetSaveImageInfo(halcon.Name).AxisGrot, ErosSocket.DebugPLC.EnumAxisType.X);
                                AxisY = Project.DebugF.DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(Vision.GetSaveImageInfo(halcon.Name).AxisGrot, ErosSocket.DebugPLC.EnumAxisType.Y);
                                AxisZ = Project.DebugF.DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(Vision.GetSaveImageInfo(halcon.Name).AxisGrot, ErosSocket.DebugPLC.EnumAxisType.Z);
                                AxisU = Project.DebugF.DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(Vision.GetSaveImageInfo(halcon.Name).AxisGrot, ErosSocket.DebugPLC.EnumAxisType.U);
                            }
                            if (AxisX == null)
                            {
                                AxisX = ErosSocket.DebugPLC.DebugComp.GetAxis(Vision.GetSaveImageInfo(halcon.Name).AxisGrot, ErosSocket.DebugPLC.EnumAxisType.X);
                            }
                            if (AxisY == null)
                            {
                                AxisY = ErosSocket.DebugPLC.DebugComp.GetAxis(Vision.GetSaveImageInfo(halcon.Name).AxisGrot, ErosSocket.DebugPLC.EnumAxisType.Y);
                            }
                            if (AxisZ == null)
                            {
                                AxisZ = ErosSocket.DebugPLC.DebugComp.GetAxis(Vision.GetSaveImageInfo(halcon.Name).AxisGrot, ErosSocket.DebugPLC.EnumAxisType.Z);
                            }
                            if (AxisU == null)
                            {
                                AxisU = ErosSocket.DebugPLC.DebugComp.GetAxis(Vision.GetSaveImageInfo(halcon.Name).AxisGrot, ErosSocket.DebugPLC.EnumAxisType.U);
                            }
                            string data = "";
                            if (AxisX != null)
                            {
                                data += "X:" + AxisX.Point.ToString("000.00");
                            }
                            if (AxisY != null)
                            {
                                data += " Y:" + AxisY.Point.ToString("000.00");
                            }
                            if (AxisZ != null)
                            {
                                data += " Z:" + AxisZ.Point.ToString("000.00");
                            }
                            if (AxisU != null)
                            {
                                data += " U:" + AxisU.Point.ToString("000.00");
                            }
                            toolStripLabel1.Text = data;
                            if (AxisZ != null || AxisY != null || AxisZ != null)
                            {
                                toolStripLabel1.Visible = true;
                                toolStripButton4.Visible = true;
                            }
                            else
                            {
                                toolStripLabel1.Visible = false;
                                toolStripButton4.Visible = false;
                                Thread.Sleep(1000);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void Vision2UserControl_Resize(object sender, EventArgs e)
        {
            if (halcon != null)
            {
                VisionControl();
                halcon.ShowObj();
            }
        }
        private void toolStRowCol_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (toolStRowCol.SelectedItem == null)
                {
                    return;
                }

                switch (toolStRowCol.SelectedItem.ToString())
                {
                    case "隐藏坐标系":
                        halcon.CoordinatePXY.CoordinateTeyp = Coordinate.Coordinate_Type.Hide;
                        break;
                    case "图像坐标系RC":
                        halcon.CoordinatePXY.CoordinateTeyp = Coordinate.Coordinate_Type.PixelRC;
                        break;
                    case "笛卡尔坐标系XY":
                        halcon.CoordinatePXY.CoordinateTeyp = Coordinate.Coordinate_Type.XYU2D;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }

        }

        private void 测试图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Vision.GetFilePath() + halcon.Name + "\\历史数据");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void visionUserControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    halcon.Drawing = false;
                }

            }
            catch (Exception)
            {


            }

        }


        private void 遍历文件夹执行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                ForImageForm1 forImageForm1 = new ForImageForm1(halcon);
                forImageForm1.Show();
            }
            catch (Exception)
            {


            }

        }

        private void imageFielNumber_MouseMove(object sender, MouseEventArgs e)
        {
            imageFielNumber.ShowDropDown();
        }

        private void 删除文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("确定删除文件？", "将删除图片和历史数据", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    if (Directory.Exists(Vision.GetFilePath() + halcon.Name + "\\历史数据"))
                    {
                        Directory.Delete(Vision.GetFilePath() + halcon.Name + "\\历史数据", true);//删除
                        AlarmText.LogIncident("删除历史数据图片", "文件夹:" + Vision.GetFilePath() + halcon.Name + "\\历史数据");
                    }
                    if (Directory.Exists(Vision.Instance.DicSaveType[halcon.Name].SavePath))
                    {
                        Directory.Delete(Vision.Instance.DicSaveType[halcon.Name].SavePath, true);//删除
                        AlarmText.LogIncident("删除SaveImage图片", "文件夹:" + Vision.Instance.DicSaveType[halcon.Name].SavePath);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private 算子表 算子;

        private void 铺满ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (halcon == null)
                {
                    MessageBox.Show("未关联执行程序");
                    return;
                }
                VisionControl(1f);
                halcon.ShowObj();

            }
            catch
            {
            }
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (halcon == null)
                {
                    MessageBox.Show("未关联执行程序");
                    return;
                }
                try
                {
                    //toolStripComboBox1.SelectedItem = halcon.Name;
                    HOperatorSet.GetImageSize(halcon.Image(), out HTuple width, out HTuple heigth);
                    halcon.Width = width;
                    halcon.Height = heigth;
                }
                catch (Exception)
                {
                }
                HOperatorSet.SetPart(halcon.hWindowHalcon(), 0, 0, halcon.Height - 1, halcon.Width - 1);
                System.Drawing.Rectangle rect = this.visionUserControl1.ImagePart;
                rect.X = 0;
                rect.Y = 0;
                //this.visionUserControl1.ImagePart = rect;

                halcon.ShowObj();
            }
            catch
            {
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                if (halcon == null)
                {
                    MessageBox.Show("未关联执行程序");
                    return;
                }

                try
                {
                    VisionControl();

                }
                catch (Exception)
                {
                }
                halcon.ShowObj();
            }
            catch
            {
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                if (halcon == null)
                {
                    MessageBox.Show("未关联执行程序");
                    return;
                }
                VisionControl(0.3f);

                halcon.ShowObj();
            }
            catch
            {
            }
        }

        public void VisionControl(Single scea = 0.0f)
        {
            try
            {
                if (scea == 0.0f)
                {
                    int iActulaHeight = Screen.PrimaryScreen.Bounds.Height;
                    this.Dock = DockStyle.Fill;
                    this.visionUserControl1.Dock = DockStyle.Left;
                    this.visionUserControl1.Width = (int)((double)this.Height * 1.3);
                    //this.FindForm().Dock = DockStyle.Fill;
                    return;
                }

                if (halcon.Width <= 1000 || halcon.Height <= 1000)
                {
                    halcon.Width = 3648;
                    halcon.Height = 2736;

                }
                if (scea >= 0.6)
                {
                    this.Dock = DockStyle.Fill;
                    this.Location = new Point(0, 0);
                    this.visionUserControl1.Dock = DockStyle.None;
                    this.visionUserControl1.Location = new Point(0, 0);
                }
                else
                {
                    this.Dock = DockStyle.Fill;
                    this.visionUserControl1.Dock = DockStyle.Fill;

                }

                this.visionUserControl1.Width = (int)(halcon.Width * scea);
                this.visionUserControl1.Height = (int)((halcon.Height * scea) - 52);
                this.Width = this.visionUserControl1.Width;
                this.Height = this.visionUserControl1.Height + 52;
            }
            catch (Exception)
            {


            }
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                if (halcon == null)
                {
                    MessageBox.Show("未关联执行程序");
                    return;
                }
                VisionControl(0.5f);

                halcon.ShowObj();
            }
            catch
            {
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                if (halcon == null)
                {
                    MessageBox.Show("未关联执行程序");
                    return;
                }
                VisionControl(0.7f);
                halcon.ShowObj();
            }
            catch
            {
            }
        }

        private void toolStripDropDownButton1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripDropDownButton1.ShowDropDown();
        }


        private void 铺满ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (halcon == null)
                {
                    MessageBox.Show("未关联执行程序");
                    return;
                }
                VisionControl(0.7f);

                halcon.ShowObj();
            }
            catch
            {
            }
        }
        private void 生成二维码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string sd = Interaction.InputBox("请输入要声才生成的内容", "", "", 100, 100);
                //Bitmap bitmap = BarcodeHelper.Generate1(sd, 100, 100);
                //bitmap.Save(AppDomain.CurrentDomain.BaseDirectory + "erwm.bmp");
                //halcon.Image(Vision.GenImageInterleaved(bitmap));
                halcon.ShowImage();
            }
            catch (Exception ex)
            {

            }

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ProjectINI.Enbt)
                {
                    Vision2.ErosProjcetDLL.Project.PropertyForm.UPProperty(halcon);
                }
                else
                {
                    MessageBox.Show("请先登录");
                }
            }
            catch (Exception)
            {


            }


        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            toolStripSplitButton1.ShowDropDown();
        }

        ImageForm1 ImageForm1 = new ImageForm1();
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Vision2.ErosProjcetDLL.UI.UICon.WindosFormerShow(ref ImageForm1);
            ImageForm1.SetUPHalc(halcon.Image());
        }

        private void 查看区域细节ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (查看区域细节ToolStripMenuItem.Checked)
            {
                查看区域细节ToolStripMenuItem.Checked = false;
            }
            else
            {
                查看区域细节ToolStripMenuItem.Checked = true;
            }
            //this.visionUserControl1.meuseBool = 查看区域细节ToolStripMenuItem.Checked;
        }

        private void 截取屏幕ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (halcon == null)
            {
                MessageBox.Show("未关联执行程序");
                return;
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "请选择保存路径";      //文件框名称

            //    openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
            saveFile.Filter = "BMP|*.bmp|jpg|*.jpg|tif|*.tif|tiff|*.tiff|hobj|*.hobj|所有文件|*.*";   //筛选器
            if (Directory.Exists(@"C:\Users\Eros\Desktop"))
            {
                saveFile.InitialDirectory = @"C:\Users\Eros\Desktop";  //默认路径
            }
            saveFile.ShowDialog();    //弹出对话框
            string path = saveFile.FileName;
            if (path == "") return;    //地址为空返回
            try
            {

                //saveFile.FileName
                Vision.SaveWindow(path, halcon.hWindowHalcon(), Path.GetExtension(path));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (toolStripButton4.Checked)
            {
                toolStripButton4.Checked = false;
                toolStripButton4.Text = "点动";
            }
            else
            {
                toolStripButton4.Checked = true;
                toolStripButton4.Text = "寸动";
            }
        }

        private void 填充或区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (填充或区域ToolStripMenuItem.Text == "填充")
                {
                    HOperatorSet.SetDraw(this.visionUserControl1.HalconWindow, "fill");

                    填充或区域ToolStripMenuItem.Text = "margin";
                }
                else
                {
                    填充或区域ToolStripMenuItem.Text = "填充";
                    HOperatorSet.SetDraw(this.visionUserControl1.HalconWindow, "margin");
                }
            }
            catch (Exception)
            {
            }
        }

    

        private void 清除序列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                panel2.Controls.Clear();
            }
            catch (Exception)
            {

            }
 
        }

        private void 模拟图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VisionSimulateForm1 visionSimulateForm1 = new VisionSimulateForm1();
            visionSimulateForm1.Show();
        }

        private void 保存到产品文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (halcon == null)
            {
                MessageBox.Show("未关联执行程序");
                return;
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "请选择保存路径";      //文件框名称
            saveFile.InitialDirectory = Vision.VisionPath + "Image\\";  //默认路径
            //    openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
            saveFile.Filter = Vision.Instance.DicSaveType[halcon.Name].SaveImageType + "|*." +
                Vision.Instance.DicSaveType[halcon.Name].SaveImageType + 
                "|BMP|*.bmp|tif|*.tif|tiff|*.tiff|hobj|*.hobj|所有文件|*.*";   //筛选器
            saveFile.ShowDialog();    //弹出对话框
            string path = saveFile.FileName;
            if (path == "") return;    //地址为空返回
            try
            {
                string da = Path.GetFileName(path).Split('.')[1].ToLower();
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                HOperatorSet.WriteImage(halcon.Image(), da, 0, path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
