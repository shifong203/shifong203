using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Vision2.Project.formula;
using Vision2.Project.Mes;
using Vision2.vision.HalconRunFile.RunProgramFile;
using HalconDotNet;
using System.IO;
using Vision2.ErosProjcetDLL.UI;
using System.Runtime.InteropServices;
using  ErosSocket.DebugPLC.Robot;
using Vision2.Project.DebugF;
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
        static Queue<DataVale> OneProductVS = new Queue<DataVale>();
        /// <summary>
        /// 整盘集合
        /// </summary>
        static Queue<TrayData> TrayImageTs = new Queue<TrayData>();
        HWindID HWindd;
        /// <summary>
        /// 单个产品
        /// </summary>
        static DataVale OneProductV;
            
        /// <summary>
        /// 单个元件
        /// </summary>
        OneComponent OneRObjT;
        /// <summary>
        /// 整盘
        /// </summary>
        static TrayData TrayImage;

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

        static RestObjImage objImageFrom;

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

                    //this.Text = "";
                    for (int i = 0; i < TrayImage.Count; i++)
                    {
                        if (TrayImage.GetDataVales()[i].Done)
                        {
                            int dt = dataGridView1.Rows.Add();
                            //objImageFrom.dataGridView1.Rows[dt].Cells[0].Value = TrayImage.GetDataVales()[i].ResuOBj[0].RunName;
                            //objImageFrom.dataGridView1.Rows[dt].Cells[1].Value = TrayImage.GetDataVales()[i].ResuOBj[0].RunID;
                            break;
                        }
                    }
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
                            if (TrayImageTs.Count == 0)
                            {
                                //TrayImage = null;
                            }
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
                    else
                    {
                        if (trayImage.Done)
                        {
                            //RecipeCompiler.AddOKNumber(true);
                            UserFormulaContrsl.WeirtAll(trayImage);
                        }
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
        double dee;
        void UpData()
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
                HWindd.OneResIamge.GetNgOBJS( OneProductV.GetNGCompData());
                HWindd.SetImaage(OneProductV.GetNGImage());
            }
            PatText = "托盘号:" + OneProductV.TrayLocation + "." +OneProductV.GetNGCamName()+ Environment.NewLine;
            if (Vision.Instance.RestT)
            {
                if (!OneProductV.Done)
                {
                    int intd = 0;
                    foreach (var item in OneProductV.GetNGCompData().DicOnes)
                    {

                        if (!item.Value.Done)
                        {
                            OneRObjT = item.Value;
                   
                            Rectangle rectangle = dataGridView1.GetCellDisplayRectangle(1, intd, false);
                            Rectangle rectangle2 = dataGridView1.RectangleToScreen(rectangle);
                            dataGridView1.Controls.Add(restOneComUserControl1);
                            restOneComUserControl1.Location = new Point(rectangle.X, rectangle.Y);
                            restOneComUserControl1.Visible = true;
                            restOneComUserControl1.BringToFront();
                            restOneComUserControl1.UpData(OneRObjT);
                            break;
                        }
                        intd++;
                    }
                }
                else
                {
                    restOneComUserControl1.Visible = false;
                }

                dataGridView1.Rows.Clear();
                int idt = 0;
                foreach (var item in OneProductV.GetNGCompData().DicOnes)
                {
                    int det=   dataGridView1.Rows.Add();
                    dataGridView1.Rows[det].Cells[0].Value = item.Value.ComponentID;
                    dataGridView1.Rows[det].Cells[1].Value = item.Value.NGText;
                    if (item.Value.Done)
                    {
                        if (item.Value.OK)
                        {
                            dataGridView1.Rows[det].DefaultCellStyle.BackColor = Color.Green;
                        }
                        else
                        {
                            dataGridView1.Rows[det].DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        if (!item.Value.OK)
                        {
                            dataGridView1.Rows[det].DefaultCellStyle.BackColor = Color.Yellow;
                        }
                    }
                
                    idt++;
                }
            }
            if (OneRObjT!=null)
            {
                PatText += OneRObjT.ComponentID + "NG信息:" + OneRObjT.NGText + "\\" + OneProductV.NGNumber;
                if (OneRObjT.ComponentID != "")
                {
                    PatText += ";位号:" + OneRObjT.ComponentID + Environment.NewLine;
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
                            catch (Exception)  { }
                            HObject imaget = new HObject();
                            imaget.GenEmptyObj();
                            //HOperatorSet.ReadImage(out HObject imaget, Vision.VisionPath + "Image\\" + OneRImage.LiyID + ".bmp");
                            if (File.Exists(Vision.VisionPath + "Image\\" + OneProductV.GetNGCamName() + "拼图.jpg"))
                            {
                                HOperatorSet.ReadImage(out imaget, Vision.VisionPath + "Image\\" + OneProductV.GetNGCamName() + "拼图.jpg");
                            }
                            else
                            {
                                hWindowControl4.HalconWindow.DispText( "为找到参考图片"+ OneProductV.GetNGCamName() , "window", 0, 0, "red", new HTuple(), new HTuple());
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
                                 hWindowControl3.HalconWindow.SetPart(row1 - (200*1),col1 -(200 * d), row2 + (200 * 1), clo2 + (200 * d));
                                 hWindowControl4.HalconWindow.SetPart(row1 - (200*1),col1 -(200 * d), row2 + (200 * 1), clo2 + (200 * d));
                            }
                            else
                            {
                                hObject1 = Vision.XLD_To_Region(item.Value.ROI);
                                HOperatorSet.SmallestRectangle1(hObject1, out  row1, out  col1, out  row2, out  clo2);
                                if (row1.Length!=0)
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
                            hWindowControl3.HalconWindow.DispObj(item.Value.NGROI);
                            hWindowControl3.HalconWindow.DispText(item.Value.NGText+ "{" + item.Value.NGText + "}", "window", 0, 0, "red", new HTuple(), new HTuple());
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
                if (OneProductV.PanelID=="")
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
                dataGridView1.Rows.Clear();
                //RecipeCompiler.AlterNumber(OneProductV.OK);
                //TrayImage.Done = true;

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
                        catch (Exception)         { }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                TrayImage = null;
            }
            catch (Exception ex) {}
        }

        private void RestObjImage_Load(object sender, EventArgs e)
        {
            try
            {
                ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
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

                                        //DebugCompiler.GetTrayDataUserControl().GetTrayEx().AddTary(trayDatas1);
                                        trayDatas1.SetTray(TrayImage);
                                        trayDatas1.UpDa();
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
                                                dataGridView1.Rows.Clear();
                                                foreach (var item in TrayImage.GetDataVales())
                                                {
                                                    if (item.Done && item.OK)
                                                    {
                                                        continue;
                                                    }

                                                        OneProductV = item;
                 
                                                        foreach (var itemd in OneProductV.GetNGCompData().DicOnes)
                                                        {
                                                            int dt = dataGridView1.Rows.Add();
                                                            if (!itemd.Value.OK)
                                                            {
                                                                dataGridView1.Rows[dt].Cells[0].Value = itemd.Value.ComponentID;
                                                                dataGridView1.Rows[dt].Cells[1].Value = itemd.Value.NGText;
                                                                dataGridView1.Rows[dt].Cells[0].Tag = itemd.Value;
                                                            }
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
                ;
                thread.IsBackground = true;
                thread.Start();
                //foreach (var itemd in OneProductV.GetNGCompData().DicOnes)
                //{
                //    if (!itemd.Value.)
                //    {
                //        OneRObj oneRObj = new OneRObj()
                //        {
                //            NGText = itemd.Value.RunNameOBJ,
                //            ComponentID = itemd.Value.ComponentName,
                //            RestText = itemd.Value.ComponentName,
                //        };
                //        if (itemd.Value.RunNameOBJ != null && itemd.Value.RunNameOBJ.Contains('.'))
                //        {
                //            string[] vs = itemd.Value.RunNameOBJ.Split('.');
                //            if (vs.Length == 2)
                //            {
                //                oneRObj.NGROI = RecipeCompiler.GetProductEX().Key_Navigation_Picture[vs[0]].KeyRoi[vs[1]].Clone();
                //                oneRObj.ROI = RecipeCompiler.GetProductEX().Key_Navigation_Picture[vs[0]].KeyRoi[vs[1]].Clone();
                //            }
                //        }
                //        OneProductV.ResuOBj[0].AddNGOBJ(oneRObj);
                //    }
                //}
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
                            dataGridView1.Rows.Clear();
                            for (int i = 0; i < TrayImage.Count; i++)
                            {
                                if (TrayImage.GetDataVales()[i]!=null)
                                {
                                    if (TrayImage.GetDataVales()[i].OK)
                                    {
                                        continue;
                                    }
                                    if (!TrayImage.GetDataVales()[i].Done)
                                    {
                                        OneProductV = TrayImage.GetDataVales()[i];
                                        foreach (var item in OneProductV.GetNGCompData().DicOnes)
                                        {
                                            int dt = dataGridView1.Rows.Add();
                                            if (!item.Value.OK)
                                            {
                                                dataGridView1.Rows[dt].Cells[0].Value = item.Value.ComponentID;
                                                dataGridView1.Rows[dt].Cells[1].Value = item.Value.NGText;
                                                dataGridView1.Rows[dt].Cells[0].Tag = item.Value;
                                            }
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
                        {
                            TrayImage.SetNumberValue(OneProductV.TrayLocation, OneProductV.OK);
                            label1.Text = "OK";
                            label1.BackColor = Color.Green;
                        }
                        else
                        {
                            label1.Text = "NG";
                            label1.BackColor = Color.Red;
                        }
                        UpData();
                    }else  if (e.KeyCode==Keys.D1)
                    {
                        restOneComUserControl1.SetRest(1);
                    }
                    else  if(e.KeyCode == Keys.D2)
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
                foreach (var item in OneProductV.GetNGCompData().DicOnes)
                {
                    if (!item.Value.Done)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    }
                    i++;
                }
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

                OneProductV.PanelID = textBox1.Text;
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("取消提交将不处理当前产品！", "是否取消提交？", MessageBoxButtons.YesNo)==DialogResult.Yes)
                {
                    timer1.Start();
                    hWindowControl1.Focus();
                    dataGridView1.Rows.Clear();
                    //dataGridView2.Rows.Clear();
                    TrayImage.Clear();
                    //TrayImage.Done = true;

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

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
    
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //for (int i = 0; i < OneProductV.ResuOBj.Count; i++)
                //{
                //    if (!OneProductV.ResuOBj[i].Done)
                //    {
                //        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                //    }
                //}
                //UpData();
                //HWindd.ShowImage();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
