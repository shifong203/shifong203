using ErosSocket.DebugPLC.Robot;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI;
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
        /// 产品集合
        /// </summary>
        private static Queue<OneDataVale> OneProductVS = new Queue<OneDataVale>();

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
        private static TrayData TrayImage;

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

        public void SetData(TrayData dataVale, int max = -1)
        {
            try
            {
                if (max >= 0)
                {
                    MaxNumber = max;
                }
                if (objImageFrom.InvokeRequired)
                {
                    objImageFrom.Invoke(new Action<TrayData, int>(SetData), TrayImage, max);
                    return;
                }
                Project.MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    if (MaxNumber > 0)
                    {
                    }
                    if (HWindd == null)
                    {
                        HWindd = new HWindID();
                        HWindd.Initialize(hWindowControl1);
                    }
                    if (TrayImageTs.Count == 0)
                    {
                        TrayImage = dataVale;
                    }
                    TrayImageTs.Enqueue(dataVale);
                    label3.Text = RecipeCompiler.Instance.GetSPC();
                }));
            }
            catch (Exception ex)
            {
            }
        }

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
                if (!trayImage.OK)
                {
                    if (!trayImage.Done)
                    {
                        if (!TrayImageTs.Contains(trayImage))
                        {
                            TrayImageTs.Enqueue(trayImage);
                        }
                        label3.Text = RecipeCompiler.Instance.GetSPC();
                        toolStripLabel1.Text = "复判剩余:" + TrayImageTs.Count;
                        UICon.SwitchToThisWindow(RestObjImage.RestObjImageFrom.Handle, true);
                        RestObjImage.RestObjImageFrom.Show();
                    }
                }
                if (trayImage.Done)
                {
                    UserFormulaContrsl.WeirtAll(trayImage);
                }
                label3.Text = RecipeCompiler.Instance.GetSPC();
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("复判窗口:" + ex.Message, Color.Red);
            }
        }

        public static void Clser()
        {
            try
            {
                OneProductVS.Clear();
                TrayImageTs.Clear();
            }
            catch (Exception)
            {
            }
        }

        private double dee;

        private void UpData()
        {
            string PatText = "";
            dee = HWindd.HeigthImage / HWindd.WidthImage;
            hWindowControl1.Height = (int)(hWindowControl1.Width * dee);
            if (panel2.Height < hWindowControl1.Height)
            {
                hWindowControl1.Height = panel2.Height;
            }
            hWindowControl1.Dock = DockStyle.Top;
            textBox1.Text = OneProductV.PanelID;
            if (OneProductV != null)
            {
                HWindd.OneResIamge.GetNgOBJS(OneProductV.GetNGCompData());
                HWindd.SetImaage(OneProductV.GetNGImage());
            }
            PatText = "托盘号:" + OneProductV.TrayLocation + "." + OneProductV.GetNGCamName() + Environment.NewLine;
            if (Vision.Instance.RestT)
            {
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
                                if (oneComponent.OK)
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
                    if (!OneRObjT.Done)
                    {
                        break;
                    }
                }
                if (!OneProductV.Done)
                {
                }
                else
                {
                    restOneComUserControl1.Visible = false;
                }
            }
            if (OneRObjT != null)
            {
                PatText += OneRObjT.ComponentID + "NG信息:" + OneRObjT.NGText + "\\" + OneProductV.NGNumber;
                if (OneRObjT.ComponentID != "")
                {
                    //PatText += ";位号:" + OneRObjT.ComponentID + Environment.NewLine;
                }
                restOneComUserControl1.UpData(OneRObjT);
            }
            label4.Text = PatText;
            try
            {
                if (Vision.Instance.RestT)
                {
                    if (OneProductV != null)
                    {
                        foreach (var item in OneProductV.GetNGCompData().DicOnes)
                        {
                            if (item.Value.Done)
                            {
                                continue;
                            }
                            HOperatorSet.GetImageSize(OneProductV.GetNGImage(), out HTuple wid, out HTuple hei);
                            hWindowControl3.HalconWindow.ClearWindow();
                            hWindowControl4.HalconWindow.ClearWindow();
                            try
                            {
                                HOperatorSet.SetDraw(hWindowControl3.HalconWindow, "margin");
                                HOperatorSet.SetLineWidth(hWindowControl3.HalconWindow, Vision.Instance.LineWidth);
                                Vision.SetFont(hWindowControl3.HalconWindow);
                                HOperatorSet.SetColor(hWindowControl3.HalconWindow, "red");
                            }
                            catch (Exception) { }
                            HObject imaget = new HObject();
                            imaget.GenEmptyObj();
                            //HOperatorSet.ReadImage(out HObject imaget, Vision.VisionPath + "Image\\" + OneRImage.LiyID + ".bmp");
                            string[] images = Directory.GetFiles(Vision.VisionPath + "Image\\");
                            List<string> imageStr = new List<string>(images);
                            bool isbde = false;
                            for (int i = 0; i < images.Length; i++)
                            {
                                if (images[i].StartsWith(Vision.VisionPath + "Image\\" + OneProductV.GetNGCamName() + "拼图"))
                                {
                                    HOperatorSet.ReadImage(out imaget, images[i]);
                                    isbde = true;
                                    break;
                                }
                            }
                            if (!isbde)
                            {
                                hWindowControl4.HalconWindow.DispText("未创建参考图片" + OneProductV.GetNGCamName(), "window", 0, 0, "red", new HTuple(), new HTuple());
                            }

                            HOperatorSet.SelectObj(item.Value.NGROI, out HObject hObject1, 1);
                            hObject1 = Vision.XLD_To_Region(hObject1);
                            HOperatorSet.SmallestRectangle1(hObject1, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple clo2);
                            if (row1.Length != 0)
                            {
                                HOperatorSet.AreaCenter(hObject1, out HTuple area, out HTuple row, out HTuple col);
                                HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(clo2, wid));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
                                HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                                double d = (double)wid / (double)hei;
                                hWindowControl3.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), clo2 + (200 * d));
                                hWindowControl4.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), clo2 + (200 * d));
                            }
                            else
                            {
                                hObject1 = Vision.XLD_To_Region(item.Value.ROI);
                                HOperatorSet.SmallestRectangle1(hObject1, out row1, out col1, out row2, out clo2);
                                if (row1.Length != 0)
                                {
                                    HOperatorSet.AreaCenter(hObject1, out HTuple area, out HTuple row, out HTuple col);
                                    HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                                    HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                                    HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(clo2, wid));
                                    HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
                                    HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                                    double d = (double)wid / (double)hei;
                                    hWindowControl3.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), clo2 + (200 * d));
                                    hWindowControl4.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), clo2 + (200 * d));
                                }
                            }
                            hWindowControl3.HalconWindow.DispObj(OneProductV.GetNGImage());
                            hWindowControl4.HalconWindow.DispObj(imaget);
                            HOperatorSet.DilationCircle(hObject1, out HObject hObject5, 50);
                            HOperatorSet.AreaCenter(item.Value.NGROI, out HTuple areas, out HTuple rows, out HTuple colus);
                            hWindowControl3.HalconWindow.DispObj(hObject5);
                            hWindowControl3.HalconWindow.DispText(item.Value.NGText + "{" + item.Value.NGText + "}", "window", 0, 0, "red", new HTuple(), new HTuple());
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            HWindd.ShowImage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (OneProductV.PanelID == "")
                {
                    textBox1.Focus();
                    MessageBox.Show("SN为空,请输入SN");
                    return;
                }
                button1.Enabled = false;
                timer1.Start();
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
                UserFormulaContrsl.WeirtAll(TrayImage);
                label3.Text = RecipeCompiler.Instance.GetSPC();
                if (TrayImageTs.Count == 0)
                {
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            Thread.Sleep(1000);
                            this.Hide();
                        }
                        catch (Exception) { }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                TrayImage = null;
            }
            catch (Exception ex) { }
        }

        private void RestObjImage_Load(object sender, EventArgs e)
        {
            try
            {
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        while (!this.IsDisposed)
                        {
                            try
                            {
                                if (TrayImage == null || TrayImage.Done)
                                {
                                    if (TrayImageTs.Count != 0)
                                    {
                                        TrayImage = TrayImageTs.Dequeue();

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
                                                            if (!itemdte.Value.OK)
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
                                                        //HWindd.OneResIamge = halconResult;
                                                    }
                                                    UpData();
                                                }
                                                UICon.SwitchToThisWindow(RestObjImage.RestObjImageFrom.Handle, true);
                                                RestObjImage.RestObjImageFrom.Show();
                                            }
                                            catch (Exception ex)
                                            {
                                                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("复判窗口:" + ex.StackTrace, Color.Red);
                                            }
                                        }));
                                    }
                                }
                                Thread.Sleep(100);
                            }
                            catch (Exception ex)
                            {
                                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("复判窗口:" + ex.Message, Color.Red);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine("复判窗口:" + ex.Message, Color.Red);
                    }
                });

                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
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
                        if (TrayImage.Done)
                        {
                            button1_Click(null, null);
                            return;
                        }
                        if (OneProductV.Done)
                        {
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
                                                if (!itemdt.Value.OK)
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
            button1.Enabled = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(Application.StartupPath + "\\截取屏幕\\" + DateTime.Now.ToLongDateString());
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
                    timer1.Start();
                    hWindowControl1.Focus();
                    //dataGridView1.Rows.Clear();
                    TrayImage.Clear();
                    if (TrayImageTs.Count == 0)
                    {
                        Thread thread = new Thread(() =>
                        {
                            try
                            {
                                Thread.Sleep(1000);
                                this.Hide();
                            }
                            catch (Exception)
                            {
                            }
                        });
                        thread.IsBackground = true;
                        thread.Start();
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
                UpData();
                HWindd.ShowImage();
            }
            catch (Exception)
            {
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
                else
                {
                }
            }
            catch (Exception)
            {
            }
        }
    }
}