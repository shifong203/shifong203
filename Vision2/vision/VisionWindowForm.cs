using NokidaE.vision;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using NokidaE.ErosUI;
using System.Windows.Forms;
using ErosProjcetDll.Project;
namespace NokidaE
{

    public partial class VisionWindow : Form
    {


        public VisionWindow(HalconRun halc)
        {
            InitializeComponent();
            panel1.Visible = HalconRunObj.Checked = false;
            Up(halc);
        }

        public HalconRun halcon;
        public string TraversalExecutionPath = @"Image\";
        private ListObjHalcon halconObjList;

        public void Up(HalconRun halc)
        {
            try
            {
                halcon = halc;
                halconObjList = new ListObjHalcon(halcon);
                panel1.Controls.Add(halconObjList);
                halconObjList.Dock = DockStyle.Fill;
                halconObjList.Show();
                toolStRowCol.SelectedIndex = 0;
                halcon.hWindowHalconID = this.hWindowControl1.HalconID;
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
        public HObject UpObj(HalconRun hRun, string objName)
        {
            try
            {
                halconObjList.Halcon_EventShowObj(hRun, objName);
                this.RunTime.Text = "运行时间：" + hRun.RunTime + "ms";
                if (!TSCBDicObj.Items.Contains("结果区域"))
                {
                    TSCBDicObj.Items.Add("结果区域");
                }
                if (!TSCBDicObj.Items.Contains("叠加区域"))
                {
                    TSCBDicObj.Items.Add("叠加区域");
                }
                foreach (var item in hRun.TKHobject.DirectoryHObject)
                {
                    if (!TSCBDicObj.Items.Contains("TKHobject." + item.Key))
                    {
                        TSCBDicObj.Items.Add("TKHobject." + item.Key);
                    }
                }

                foreach (var item in halcon.KeyHObject.DirectoryHObject)
                {
                    if (!TSCBDicObj.Items.Contains("KHobject." + item.Key))
                    {
                        TSCBDicObj.Items.Add("KHobject." + item.Key);
                    }
                }
                TSCBDicObj.SelectedItem = objName;
            }
            catch (Exception exx)
            {
            }
            return halcon.GetShowObj();
        }



        /// <summary>
        /// 执行完成
        /// </summary>
        public bool threadOK;

        public string Pname = "";

        private void VisionWindow_Load(object sender, EventArgs e)
        {
            try
            {
                if (halcon["区域文本"] == "显示")
                {
                    toolStripButton3.Text = "隐藏区域文本";
                }
                else
                {
                    toolStripButton3.Text = "显示区域文本";
                }
                if (halcon.WhidowAdd)
                {
                    toolStripButton5.Text = "固定";
                }
                else
                {
                    toolStripButton5.Text = "缩放";
                }
                if (!halcon.ISMesage)
                {
                    toolStripButton4.Text = "隐藏文本结果";
                }
                else
                {
                    toolStripButton4.Text = "显示文本结果";
                }
                halcon.EventShowObj += UpObj;
                Directory.CreateDirectory(Application.StartupPath + @"\Image\");
                var path = Directory.GetFiles(Application.StartupPath + @"\Image\");
                ListImagePath.AddRange(path);

                Control.CheckForIllegalCrossThreadCalls = false;
                HOperatorSet.SetDraw(hWindowControl1.HalconID, "margin");
                HOperatorSet.SetColored(hWindowControl1.HalconID, 12);
                HOperatorSet.SetSystem("tsp_width", 50000000);
                HOperatorSet.SetSystem("tsp_height", 50000000);
                HOperatorSet.SetSystem("clip_region", "false");
                HOperatorSet.SetSystem("do_low_error", "false");
                m_ImageRow0 = m_ImageCol0 = 0;
                //ModelVision modelV = new ModelVision();
                //VisionDataS visionDataS = new VisionDataS();
                //HalconProgram halconProgram = new HalconProgram(visionDataS);
                //halconProgram.Runet += modelV.Run;
                //foreach (var item in ErosSocket.ErosConLink.StaticCon.SocketClint)
                //{
                //    System.Windows.Forms.ToolStripItem LinKbtn = new ToolStripLabel();
                //    LinKbtn.Name = item.Key;
                //    LinKbtn.Text = item.Key + ":" + item.Value.LinkState;
                //    toolStrip2.Items.Add(LinKbtn);
                //    if (item.Value.LinkState == "连接成功")
                //    {
                //        toolStrip2.Items[item.Key].ForeColor = Color.Green;
                //    }
                //    else
                //    {
                //        toolStrip2.Items[item.Key].ForeColor = Color.Red;
                //    }
                //    item.Value.LinkO += Value_LinkO;
                //    string Value_LinkO(bool key)
                //    {
                //        if (key)
                //        {
                //            toolStrip2.Items[item.Key].ForeColor = Color.Green;
                //        }
                //        else
                //        {
                //            toolStrip2.Items[item.Key].ForeColor = Color.Red;
                //        }
                //        toolStrip2.Items[item.Key].Text = item.Key + ":" + item.Value.LinkState;
                //        return "";
                //    }
                //}
                foreach (var item in Vision.Instance.RunCamParam)
                {
                    System.Windows.Forms.ToolStripItem LinKbtn = new ToolStripLabel();
                    LinKbtn.Name = item.Key;
                    if (item.Value.m_bCamIsOK)
                    {
                        LinKbtn.ForeColor = Color.Green;
                        LinKbtn.Text = item.Value.Name + item.Value["链接状态"];
                    }
                    else
                    {
                        LinKbtn.Text = item.Value.Name + item.Value["链接状态"];
                        LinKbtn.ForeColor = Color.Red;
                    }

                    toolStrip2.Items.Add(LinKbtn);
                    item.Value.LinkSt += Cam_LinkSt;
                    void Cam_LinkSt(bool key)
                    {
                        if (key)
                        {
                            toolStrip2.Items[LinKbtn.Name].ForeColor = Color.Green;
                            toolStrip2.Items[LinKbtn.Name].Text = item.Value.Name + item.Value["链接状态"];
                        }
                        else
                        {
                            toolStrip2.Items[LinKbtn.Name].Text = item.Value.Name + item.Value["链接状态"];
                            toolStrip2.Items[LinKbtn.Name].ForeColor = Color.Red;
                        }
                    }
                }
                //System.Windows.Forms.ToolStripItem LinKbtn1 = new ToolStripLabel();
                //LinKbtn1.Name = "Cam1";
                //LinKbtn1.Text = "Cam1:" + halcon.ListRun["Cam1"]["链接状态"];

                //toolStrip2.Items.Add(LinKbtn1);

                //if (halcon.ListRun["Cam1"]["链接状态"] == "链接成功")
                //{
                //    toolStrip2.Items["Cam1"].ForeColor = Color.Green;
                //    toolStrip2.Items["Cam1"].Text = "Cam1:" + halcon.ListRun["Cam1"]["链接状态"];
                //}
                //else
                //{
                //    toolStrip2.Items["Cam1"].Text = "Cam1:" + halcon.ListRun["Cam1"]["链接状态"];
                //    toolStrip2.Items["Cam1"].ForeColor = Color.Red;
                //}

                //Cam = (CamParam)halcon.ListRun["Cam1"];

                //Cam.LinkSt += Cam_LinkSt;


            }
            catch (Exception ex)
            {
                MessageBox.Show("窗口初始化错误" + ex.Message);

            }
        }

        #region 11.13图形窗口事件

        public List<string> ListImagePath = new List<string>();

        public HTuple m_ImageRow1, m_ImageCol1;

        private HTuple H_Scale = 0.2; //缩放步长
        private HTuple MaxScale = 10000;//最大放大系数
        private HTuple ptX, ptY;
        private HTuple m_ImageRow0, m_ImageCol0;
        private HTuple hv_Button;

        private HTuple Row0_1, Col0_1, Row1_1, Col1_1;

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            halcon.HTempobjectClear();
            halcon.Message = "";
            halcon.ShowImage();
        }

        private void 设置图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "请选择图片文件可多选";
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
            openFileDialog.InitialDirectory = halcon.OpenfileDialogImage;
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.Length == 0) return;
            halcon.OpenfileDialogImage = Path.GetDirectoryName(openFileDialog.FileName);
            ListImagePath.AddRange(openFileDialog.FileNames);
            imageFielNumber.DropDownItems.Clear();
            for (int i = 0; i < openFileDialog.FileNames.Length; i++)
            {
                ToolStripItem toolStrip = imageFielNumber.DropDownItems.Add(openFileDialog.FileNames[i]);
                toolStrip.Click += ToolStrip_Click;
                toolStrip.PerformClick();
            }
        }

  
        private void TSCBDicObj_Click(object sender, EventArgs e)
        {
            if (TSCBDicObj.SelectedItem != null)
            {
                halcon.SetShow(TSCBDicObj.SelectedItem.ToString());
                halcon.GetShowObj();
            }
        }

        private void TSCBDicObj_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSCBDicObj.PerformClick();
        }

        private void 选择文件夹ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = Application.StartupPath;
            dialog.Description = "请选择Txt所在文件夹";
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            var file = ErosProject.ErosCon.GetFilesArrayPath(dialog.SelectedPath, ".bmp"); ;//包含查询子目录
            this.ListImagePath.Clear();
            this.ListImagePath.AddRange(file);
            number = 0;
            this.Text = "图像窗口" + dialog.SelectedPath + "|" + file.Length + "/" + number;
            m_ImageRow1 = halcon._Widgth;
            m_ImageCol1 = halcon._Heigth;
        }

        private void 遍历文件夹执行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (遍历文件夹执行ToolStripMenuItem.Text == "遍历执行中 >")
            {
                Cambueys = false;
                遍历文件夹执行ToolStripMenuItem.Text = "遍历文件夹执行";
                return;
            }

            dialog.SelectedPath = Application.StartupPath;
            dialog.Description = "请选择Txt所在文件夹";
            System.Windows.Forms.DialogResult dialoge = PropertyGridEx.FolderBrowserLauncher.ShowFolderBrowser(dialog);
            if (dialoge != System.Windows.Forms.DialogResult.OK) return;
            TraversalExecutionPath = dialog.SelectedPath;
            if (halcon.Mode != "开发者模式")
            {
                MessageBox.Show("非开发模式中！");
                return;
            }
            tcbRunType.SelectedIndex = 6;
            if (Cambueys)
            {
                if (tSLStata.Text.StartsWith("==> 遍历执行文件夹中....", StringComparison.Ordinal))
                {
                    Cambueys = false;
                }
                return;
            }
            MethodInvoker methodInvoker = ThreadFilesVison;

            Thread thread = new Thread(() =>
            {
                Cambueys = true;
                遍历文件夹执行ToolStripMenuItem.Text = "遍历执行中 >";
                try
                {
                    var det = ErosProject.ErosCon.GetFilesDicListPath(TraversalExecutionPath, "bmp");
                    if (det.Count == 0)
                    {
                        Cambueys = false;
                        MessageBox.Show("本地Image未找到图片");
                        return;
                    }
                    string files = "==> 遍历执行文件夹中....";
                    int numbers = 0;
                    foreach (var item in det)
                    {
                        numbers = numbers + item.Value.Count;
                    }
                    files += "总数:" + numbers + ";" + "文件夹数量：" + det.Count + "";
                    int asd = 0;
                    foreach (var item in det)
                    {
                        asd++;
                        for (int i = 0; i < item.Value.Count; i++)
                        {
                            if (!Cambueys)
                            {
                                tSLStata.Text = "状态";
                                Cambueys = false;
                                遍历文件夹执行ToolStripMenuItem.Text = "遍历文件夹执行";
                                MessageBox.Show("停止遍历执行");
                                return;
                            }
                            tSLStata.Text = files + "/" + asd + ";" + item.Key + "||" + item.Value.Count + "/" + (i + 1);
                            if (halcon.ReadImage(item.Value[i]))
                                halcon.ShowVision(1, 999);
                            halcon.SaveDataExcelImage("遍历数据", halcon.WriteData.TupleMax(), halcon["NG数量"], halcon.WriteData);
                            Thread.Sleep(200);
                        }
                    }
                    遍历文件夹执行ToolStripMenuItem.Text = "遍历文件夹执行";
                    this.Invoke(methodInvoker);
                }
                catch (Exception)
                {
                }

                Cambueys = false;
            });
            void ThreadFilesVison()
            {
                ConClass.Npoi.UpDataExclec(Application.StartupPath + "\\遍历数据\\" + DateTime.Now.ToLongDateString() + ".xls");
            }
            thread.IsBackground = true;
            thread.Start();
        }

        private void 数据分析ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            数据分析 clFrom = new 数据分析();
            clFrom.Show();
        }

        private 算子表 算子;

        private void 命令行ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (算子 == null || 算子.IsDisposed)
            {
                算子 = new 算子表();
            }
            算子.Show();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (toolStripButton4.Text == "显示文本结果")
            {
                toolStripButton4.Text = "隐藏文本结果";
                halcon.ISMesage = false;
            }
            else
            {
                toolStripButton4.Text = "显示文本结果";
                halcon.ISMesage = true;
            }
            halcon.GetShowObj();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (toolStripButton3.Text == "显示区域文本")
            {
                toolStripButton3.Text = "隐藏区域文本";
                halcon["区域文本"] = "显示";
            }
            else
            {
                toolStripButton3.Text = "显示区域文本";
                halcon["区域文本"] = "隐藏";
            }
            halcon.GetShowObj();
        }

        private void ToolStrip_Click(object sender, EventArgs e)
        {
            ToolStripItem eet = (ToolStripItem)sender;
            halcon.ReadImage(eet.Text);
            halcon.ShowImage();
            this.Text = eet.Text + "|" + ListImagePath.Count + "=>";
        }

        private void VisionWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void VisionWindow_Resize(object sender, EventArgs e)
        {
            halcon.GetShowObj();
        }

        private ErosUI.ErosNewFrom erosNew = new ErosUI.ErosNewFrom();

        private void hMI设计ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (erosNew == null || erosNew.IsDisposed)
            {
                erosNew = new ErosUI.ErosNewFrom();
            }
            erosNew.Show();
        }

        private void 清除数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            halcon["NGNumber"] = 0;
            halcon["OKNumber"] = 0;
            halcon["触发"] = 0;
            halcon.OnEventDoen();
        }

        private void 保存图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "请选择保存路径";      //文件框名称

            saveFile.Filter = "文本文件|*.BMP|所有文件|*.*";   //筛选器
            if (Directory.Exists(@"C:\Users\Eros\Desktop"))
            {
                saveFile.InitialDirectory = @"C:\Users\Eros\Desktop";  //默认路径
            }

            saveFile.ShowDialog();    //弹出对话框
            string path = saveFile.FileName;
            if (path == "") return;    //地址为空返回
            halcon.SaveImage(path);
        }

        private void imageFielNumber_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "请选择图片文件可多选";
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
            if (Directory.Exists(halcon.OpenfileDialogImage))
            {
                openFileDialog.InitialDirectory = halcon.OpenfileDialogImage;
            }
            else
            {
                openFileDialog.InitialDirectory = Application.StartupPath;
            }

            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.Length == 0) return;
            //listBFileImages.Items.AddRange(openFileDialog.FileNames);
            //halconImage.fileImage.Append(openFileDialog.FileNames);
            //listBFileImages.DataSource = halconImage.fileImage.ToSArr();
            //ImageNumber.Text = "数量：" + listBFileImages.Items.Count;
            halcon.OpenfileDialogImage = Path.GetDirectoryName(openFileDialog.FileName);
            ListImagePath.AddRange(openFileDialog.FileNames);
            if (imageFielNumber.DropDownItems.Count >= 6)
            {
                imageFielNumber.DropDownItems.RemoveAt(0);
            }

            for (int i = 0; i < openFileDialog.FileNames.Length; i++)
            {
                ToolStripItem toolStrip = imageFielNumber.DropDownItems.Add(openFileDialog.FileNames[i]);
                imageFielNumber.DropDownItems.Insert(0, toolStrip);
                toolStrip.Click += ToolStrip_Click;
                toolStrip.PerformClick();
            }
        }

        private void imageFielNumber_MouseMove(object sender, MouseEventArgs e)
        {
            imageFielNumber.ShowDropDown();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            toolStripSplitButton1.ShowDropDown();
        }

        private void 打开EPSONRobotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EPSON_Robot_Remote_TCPIP.EPSONRobot form1 = new EPSON_Robot_Remote_TCPIP.EPSONRobot();
            form1.Show();
        }

        private void 删除文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("确定删除文件？", "将删除图片和历史数据", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    if (Directory.Exists(halcon.SaveImageNGPath))
                    {
                        Directory.Delete(halcon.SaveImageNGPath, true);//删除
                        HalconRun.LogIncident("删除SaveImageNG图片", "文件夹:" + halcon.SaveImageNGPath);
                    }
                    if (Directory.Exists(halcon.SaveImageOKPath))
                    {
                        Directory.Delete(halcon.SaveImageOKPath, true);//删除
                        HalconRun.LogIncident("删除SaveImageOK图片", "文件夹:" + halcon.SaveImageOKPath);
                    }
                    if (Directory.Exists(Application.StartupPath + "\\历史数据"))
                    {
                        Directory.Delete(Application.StartupPath + "\\历史数据", true);//删除
                        HalconRun.LogIncident("删除历史数据图片", "文件夹:" + Application.StartupPath + "\\历史数据");
                    }
                    if (Directory.Exists(halcon.SaveImagePath))
                    {
                        Directory.Delete(halcon.SaveImagePath, true);//删除
                        HalconRun.LogIncident("删除SaveImage图片", "文件夹:" + halcon.SaveImagePath);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (!halcon.WhidowAdd)
            {
                halcon.WhidowAdd = true;
                toolStripButton5.Text = "固定";
            }
            else
            {
                toolStripButton5.Text = "缩放";

                halcon.WhidowAdd = false;
            }
        }

        private void 开机管理ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ConClass.OnManagementForm onManagementForm = new ConClass.OnManagementForm();
            onManagementForm.Show();
        }

        private void 相机设置ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //fomrCam = new MdiForm();
            //fomrCam.Text = "相机设置";
            //fomrCam.Controls.Add(new CamPragramV(halcon));
            //ErosProject.ErosCon.WindosFormer(fomrCam);
        }

        private void 图像设置ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //ShowFomrHimage();
        }

        private void 通信设置ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //ErosSocket.SocketConnectForm socketConnectF = new ErosSocket.SocketConnectForm();
            //ErosProject.ErosCon.WindosFormer(socketConnectF);
        }

        private void 图像参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PorejectForm porejectForm = new PorejectForm();
            porejectForm.Show();
            //MdiForm fomrHimage = new MdiForm();
            //fomrHimage.Text = "图像程序";
            //fomrHimage.Controls.Add(new ConRunProgram(halcon));
            //fomrHimage.Show();
        }

        private void panel2_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                halcon.GetShowObj();
            }
            catch (Exception)
            {
            }
        }

        private void 打开事件信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogMessageForm logMessageForm = new LogMessageForm();
            logMessageForm.Show();
        }

        private void toolStRowCol_Click(object sender, EventArgs e)
        {
        }

        private void toolStRowCol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStRowCol.SelectedItem.ToString() == "图像坐标系RC")
            {
                halcon.Coordinate_x = Coordinate.Coordinate_Type.PixelRC;
            }
            else if (toolStRowCol.SelectedItem.ToString() == "笛卡尔坐标系XY")
            {
                halcon.Coordinate_x = Coordinate.Coordinate_Type.XYU2D;
            }
            else
            {
                halcon.Coordinate_x = Coordinate.Coordinate_Type.Hide;
            }
        }

        //原图尺寸
        public void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.GetImageSize(halcon.Image, out HTuple width, out HTuple heigth);
                halcon._Widgth = width;
                halcon._Heigth = heigth;
                HOperatorSet.SetPart(halcon.hWindowHalconID, 0, 0, halcon._Heigth - 1, halcon._Widgth - 1);
                halcon.GetShowObj();
                m_ImageRow1 = halcon._Heigth;
                m_ImageCol1 = halcon._Widgth;
                m_ImageRow0 = 0;
                m_ImageCol0 = 0;
            }
            catch
            {
            }
        }
        /// <summary>
        /// 图像缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl1_HMouseWheel(object sender, HalconDotNet.HMouseEventArgs e)
        {
            try
            {
                if (halcon.Drawing || halcon.WhidowAdd)
                {
                    return;
                }
                //判断图像是否为空
                if (!Vision.ObjectValided(halcon.Image))
                {
                    return;
                }

                HOperatorSet.GetMposition(this.hWindowControl1.HalconID, out ptY, out ptX, out hv_Button);



                if (m_ImageRow1 == null)
                {
                    m_ImageRow1 = halcon._Widgth;
                    m_ImageCol1 = halcon._Heigth;
                }
                //向上滑动滚轮，图像缩小。以当前鼠标的坐标为支点进行缩小或放大
                if (e.Delta > 0)
                {
                    Cursor = Cursors.PanSouth;
                    //重新计算缩小后的图像区域
                    Row0_1 = ptY - 1 / (1 - H_Scale) * (ptY - m_ImageRow0);
                    Row1_1 = ptY - 1 / (1 - H_Scale) * (ptY - m_ImageRow1);
                    Col0_1 = ptX - 1 / (1 - H_Scale) * (ptX - m_ImageCol0);
                    Col1_1 = ptX - 1 / (1 - H_Scale) * (ptX - m_ImageCol1);

                    //限定缩小范围
                    if ((Col1_1 - Col0_1).TupleAbs() / halcon._Widgth <= 100)
                    {
                        //设置在图形窗口中显示局部图像
                        m_ImageRow0 = Row0_1;
                        m_ImageCol0 = Col0_1;
                        m_ImageRow1 = Row1_1;
                        m_ImageCol1 = Col1_1;
                    }
                }
                else
                {
                    Cursor = Cursors.PanNorth;
                    //重新计算放大后的图像区域
                    Row0_1 = ptY - 1 / (1 + H_Scale) * (ptY - m_ImageRow0);
                    Row1_1 = ptY - 1 / (1 + H_Scale) * (ptY - m_ImageRow1);
                    Col0_1 = ptX - 1 / (1 + H_Scale) * (ptX - m_ImageCol0);
                    Col1_1 = ptX - 1 / (1 + H_Scale) * (ptX - m_ImageCol1);

                    //限定放大范围
                    HTuple dw = (halcon._Widgth / (Col1_1 - Col0_1).TupleAbs());
                    if ((halcon._Widgth / (Col1_1 - Col0_1).TupleAbs()) <= MaxScale)
                    {
                        //设置在图形窗口中显示局部图像
                        m_ImageRow0 = Row0_1;
                        m_ImageCol0 = Col0_1;
                        m_ImageRow1 = Row1_1;
                        m_ImageCol1 = Col1_1;
                    }
                }
                HOperatorSet.SetPart(this.hWindowControl1.HalconID, m_ImageRow0, m_ImageCol0, m_ImageRow1, m_ImageCol1);

                if (TSCBDicObj.SelectedItem != null)
                {
                    halcon.SetShow(TSCBDicObj.SelectedItem.ToString());
                }
                halcon.GetShowObj();

            }
            catch (Exception es)
            {
            }
        }

        bool meuseBool;

        private void hWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            if (halcon.Drawing || halcon.WhidowAdd)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                stratX = e.X;
                stratY = e.Y;
                meuseBool = true;
                Cursor = Cursors.SizeAll;
            }
        }

        private void hWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {
            meuseBool = false;
        }

        private void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            toolStripSplitButton2.ShowDropDown();
        }

        private void 导入XLDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            halcon.GetFileNameDXF();
        }

        private void 绘制区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            halcon.Draw_Region();
        }

        double stratX;

        private void ButtonHelp_Click(object sender, EventArgs e)
        {
            ErosProjcetDll.UI.UICon.WindosFormer(ref holpForm);
        }

        private void HalconRunObj_Click(object sender, EventArgs e)
        {

        }

        private void 区域窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (HalconRunObj.Checked)
            {
                panel1.Visible = HalconRunObj.Checked = false;
            }
            else
            {
                panel1.Visible = HalconRunObj.Checked = true;
            }
        }

        double stratY;
        /// <summary>
        /// 获得图像值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {
                hWindowControl1.HalconWindow.GetMposition(out int rowi, out int coli, out int button1);
                ptY = rowi;
                ptX = coli;
                switch (halcon.Coordinate_x)
                {
                    case Coordinate.Coordinate_Type.XYU2D:
                        vision.Coordinate.CpointXY coordinate2D = halcon.GetCPointXYtoRC(ptY.D, ptX.D);

                        ImageXY.Text = "Y= " + coordinate2D.Y.ToString("0.00") + ",X = " + coordinate2D.X.ToString("0.00");

                        break;

                    default:
                        ImageXY.Text = "Row(Y) = " + ptY + ",Col(X) = " + ptX;
                        break;
                }

                if (halcon.Mode == "开发者模式")
                {
                }
                else
                {
                    panel1.Visible = false;
                }
                ///移动图像
                if (meuseBool)
                {
                    double motionX, motionY;
                    motionX = ((e.X - stratX));
                    motionY = ((e.Y - stratY));
                    if (((int)motionX != 0) || ((int)motionY != 0))
                    {
                        if (m_ImageRow1 == null)
                        {
                            m_ImageRow1 = halcon._Widgth;
                            m_ImageCol1 = halcon._Heigth;
                        }
                        m_ImageRow0 += -motionY;
                        m_ImageRow1 += -motionY;
                        m_ImageCol0 += -motionX;
                        m_ImageCol1 += -motionX;
                        System.Drawing.Rectangle rect = hWindowControl1.ImagePart;
                        rect.X = (int)Math.Round(m_ImageCol0.D);
                        rect.Y = (int)Math.Round(m_ImageRow0.D);
                        hWindowControl1.ImagePart = rect;
                        stratX = e.X - motionX;
                        stratY = e.Y - motionY;
                        halcon.GetShowObj();
                    }
                }
                HOperatorSet.GetGrayval(halcon.Image, ptY, ptX, out HTuple Grey);

                if (Grey.Length == 3)
                {
                    ImageRGB.Text = " R:" + Grey[0] + ";  G:" + Grey[1] + ";  B:" + Grey[2];
                }
                else if (Grey.Length == 1)
                {
                    ImageRGB.Text = "B:" + Grey;
                }
            }
            catch
            {
                ImageRGB.Text = "B:-";
            }
        }

        private int number = 0;
        private bool Cambueys = false;
        /// <summary>
        /// 帮助
        /// </summary>
        ErosProjcetDll.Project.HolpForm holpForm = new HolpForm();
        /// <summary>
        /// 按键执行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (Keys.F5 == e.KeyCode)
                {
                    tcbRunType.SelectedIndex = 0;
                    if (Cambueys) return;

                    halcon.ShowVision();
                }
                else if (Keys.F6 == e.KeyCode)
                {
                    tcbRunType.SelectedIndex = 1;
                    if (Cambueys) return;
                }
                else if (Keys.F7 == e.KeyCode)
                {
                    tcbRunType.SelectedIndex = 2;
                    if (Cambueys) return;
                    halcon.ShowVision(1, 99);
                }
                else if (Keys.F8 == e.KeyCode)
                {
                    tcbRunType.SelectedIndex = 3;
                    if (Cambueys) return;
                    halcon.ReadCamImage();
                    halcon.ShowImage();
                }
                else if (Keys.F9 == e.KeyCode)
                {
                    return;
                    tcbRunType.SelectedIndex = 4;

                    if (Cambueys)
                    {
                        if (!tSLStata.Text.Contains("=>实时中!"))
                        {
                            return;
                        }
                        Cambueys = false;
                        tSLStata.Text = tSLStata.Text.Remove(tSLStata.Text.IndexOf("=>实时中!"));
                    }
                    else
                    {
                        Cambueys = true;
                        tSLStata.Text = "图像窗口=>实时中!";
                    }
                    Thread thread = new Thread(() =>
                    {
                        while (Cambueys)
                        {
                            try
                            {
                                halcon.ShowVision(0);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                else if (Keys.F10 == e.KeyCode)
                {
                    tcbRunType.SelectedIndex = 5;

                }
                else if (Keys.F11 == e.KeyCode)
                {
                    ErosProjcetDll.UI.UICon.WindosFormer(ref holpForm);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }

    #endregion 11.13图形窗口事件
}