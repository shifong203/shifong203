﻿using ErosSocket.ErosConLink;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.Project.DebugF;
using Vision2.Project.formula;
using Vision2.Project.ProcessControl;
using Vision2.vision;

namespace Vision2.Project
{
    public partial class LodingProject : Form
    {
        public LodingProject()
        {
            InitializeComponent();
            this.Cursor = Cursors.WaitCursor;
        }

        //主窗体
        Vision2.Project.MainForm1 ds;
        string[] ImageS;
        private void LodingProject_Load(object sender, EventArgs e)
        {
            ds = new Vision2.Project.MainForm1();
            ds.Show();
            try
            {
                if (System.IO.File.Exists(Application.StartupPath + "\\LOGO.jpg"))
                {
                    pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\LOGO.jpg");
                }
                if (System.IO.Directory.Exists(Application.StartupPath + "\\ImageD"))
                {
                    ImageS = System.IO.Directory.GetFiles(Application.StartupPath + "\\ImageD");
                }
            }
            catch (Exception)
            {
            }
            Thread thread = new Thread(() =>
            {
                MethodInvoker methodInvoker = new MethodInvoker(TimeHide);
                this.Invoke(methodInvoker);
            });
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
            Thread thread2 = new Thread(() =>
            {
                try
                {
                    if (ImageS.Length > 0)
                    {
                        pictureBox1.Image = Image.FromFile(ImageS[0]);
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(100);
                        dss1.progressBar2.Value = i * 10;
                    }
                    dss1.progressBar1.Value = 20;
                    if (ImageS.Length > 1)
                    {
                        pictureBox1.Image = Image.FromFile(ImageS[1]);
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(100);
                        dss1.progressBar2.Value = i * 10;
                    }
                    dss1.progressBar1.Value = 50;
                    if (ImageS.Length > 2)
                    {
                        pictureBox1.Image = Image.FromFile(ImageS[2]);
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(100);
                        dss1.progressBar2.Value = i * 10;
                    }
                    dss1.progressBar1.Value = 90;
                    if (ImageS.Length > 3)
                    {
                        pictureBox1.Image = Image.FromFile(ImageS[3]);
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(100);
                        dss1.progressBar2.Value = i * 10;
                    }
                    dss1.label1.Text = "载入完成;";
                    dss1.progressBar1.Value = 100;
                    Thread.Sleep(500);
                    foreach (var item in vision.Vision.GetHimageList())
                    {
                        try
                        {
                            if (System.IO.File.Exists(Application.StartupPath + "\\ImageD\\8.jpeg"))
                            {
                                //string[] ImageS = System.IO.Directory.GetFiles(Application.StartupPath + "\\ImageD\\.5.jpg");
                                //Read_Image(out HObject hObject, Application.StartupPath + "\\ImageD\\.5.jpg");
                                item.Value.ReadImage(Application.StartupPath + "\\ImageD\\8.jpeg");
                            }
                        }
                        catch (Exception)
                        {
                        }
                        item.Value.ShowImage();
                    }

                    this.Hide();
                }
                catch (Exception)
                {
                }
            });
            thread2.IsBackground = true;
            thread2.Start();
        }
        private void TimeHide()
        {
            try
            {
                dss1.label1.Text = "载入程序中......";
                dss1.progressBar1.Value = 10;
                dss1.label1.Text = "载入链接信息......";
                dss1.progressBar1.Value = 10;
                Thread.Sleep(100);

                RecipeCompiler recipeCompiler = new RecipeCompiler();
                DebugCompiler debugCalss = new DebugCompiler();
                ErosSocket.ErosConLink.DicSocket dicSocket = new DicSocket();
                ErosSocket.DebugPLC.DebugComp debugComp = new ErosSocket.DebugPLC.DebugComp();
                ProcessUser process = new ProcessUser();

                if (!System.IO.Directory.Exists(ProjectINI.In.ProjectPathRun))
                {
                    ProjectINI.In.AddProject(debugComp);
                    ProjectINI.In.AddProject(dicSocket);
                    ProjectINI.In.AddProject(process);
                    ProjectINI.In.AddProject(Vision.Instance);
                    ProjectINI.In.AddProject(debugCalss);
                }
                else
                {
                    ProjectINI.In.UserName = "";
                    ProjectINI.In.UserID = "";
                    //加载程序参数
                    //ErosSocket.ErosConLink.StaticCon.StataRunSocket(Application.StartupPath + "\\Project\\" + ProjectINI.In.ProjectName + "\\" + ProjectINI.In.RunName + "\\Socket");
                    ProjectINI.ReadPathJsonToCalss<ErosSocket.ErosConLink.DicSocket>(ProjectINI.In.ProjectPathRun + "\\Socket\\SocketLink.socket", out dicSocket);
                    dss1.label1.Text = "载入链接信息完成.....";

                    if (dicSocket == null)
                    {
                        dicSocket = new ErosSocket.ErosConLink.DicSocket();
                    }
                    dicSocket.initialization();

                    dss1.label1.Text = "载入配方信息......";

                    recipeCompiler = recipeCompiler.ReadThis<RecipeCompiler>(ProjectINI.In.ProjectPathRun);
                    if (recipeCompiler != null)
                    {
                        ProjectINI.In.AddProject(recipeCompiler);
                    }
                    recipeCompiler.initialization();

                    dss1.label1.Text = "载入视觉信息......";
                    Vision.Instance.UpReadThis(ProjectINI.In.ProjectPathRun, Product.ProductionName);

                    if (Vision.Instance.RsetPort >= 0)
                    {
                        MainForm1.MainFormF.Hide();
                        goto endt;
                    }
                    RecipeCompiler.GetUserFormulaContrsl().AddData(RecipeCompiler.Instance.OKNumber);

                    debugComp = debugComp.ReadThis<ErosSocket.DebugPLC.DebugComp>(ProjectINI.In.ProjectPathRun);
                    if (debugComp == null)
                    {
                        debugComp = new ErosSocket.DebugPLC.DebugComp();
                    }
                    debugComp.initialization();


                    debugCalss = debugCalss.ReadThis<DebugCompiler>(ProjectINI.In.ProjectPathRun);
                    if (debugCalss == null)
                    {
                        debugCalss = new DebugCompiler();
                    }
       
                    RecipeCompiler.GetUserFormulaContrsl().UPSetGetPargam();
       
                    endt:
                    process = process.ReadThis<ProcessUser>(ProjectINI.In.ProjectPathRun);
                    //添加程序服务
                    ProjectINI.In.AddProject(process);
                    ProjectINI.In.AddProject(Vision.Instance);
                    ProjectINI.In.AddProject(debugComp);
                    ProjectINI.In.AddProject(dicSocket);
                    ProjectINI.In.AddProject(debugCalss);
                    //SocketUI.Project.HMIDIC hMIDIC = new SocketUI.Project.HMIDIC();
                    //hMIDIC = hMIDIC.ReadThis<SocketUI.Project.HMIDIC>(ProjectINI.In.ProjectPathRun);
                    //ProjectINI.In.AddProject(hMIDIC);
                    //添加工具栏
                    //ToolForm.ThisForm.AddKeyConvert("视觉", new VisionWindow(false));
                    ToolForm.ThisForm.AddKeyConvert("PLC", new ErosSocket.ErosUI.PLCBtn() { Name = "按钮" });
                    //ToolForm.ThisForm.AddKeyConvert("PLC", new UserInterfaceControl());
                    //Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton toolStripButton = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();

                    //toolStripButton.Text = "历史信息";
                    //toolStripButton.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    //toolStripButton.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;

                    //toolStripButton.Click += Btn_Massge_Click;
                    //ds.FileToolStripAdd(toolStripButton);


                    //加载链接状态
                    foreach (var item in StaticCon.SocketClint)
                    {
                        System.Windows.Forms.ToolStripItem LinKbtn = new ToolStripLabel();
                        LinKbtn.Name = item.Key;
                        LinKbtn.Text = item.Key + ":" + item.Value.LinkState;
                        ds.BoolToolStripAdd(LinKbtn);
                        if (item.Value.IsConn)
                        {
                            LinKbtn.ForeColor = Color.Green;
                        }
                        else
                        {
                            LinKbtn.ForeColor = Color.Red;
                        }
                        item.Value.LinkO += Value_LinkO;
                        Value_LinkO(item.Value.IsConn);
                        string Value_LinkO(bool key)
                        {
                            this.Invoke(new Action(() =>
                            {
                                if (key)
                                {
                                    LinKbtn.ForeColor = Color.Green;
                                }
                                else
                                {
                                    LinKbtn.ForeColor = Color.Red;
                                }
                                if (item.Value.LinkState.Contains("连接成功"))
                                {
                                    LinKbtn.ForeColor = Color.Green;
                                }
                                else
                                {
                                    LinKbtn.ForeColor = Color.Red;
                                }
                                LinKbtn.Text = item.Key + ":" + item.Value.LinkState;
                            }));


                            return "";
                        }
                    }
                    foreach (var item in Vision.Instance.RunCams)
                    {
                        System.Windows.Forms.ToolStripItem LinKbtn = new ToolStripLabel();
                        LinKbtn.Name = item.Key;
                        Cam_LinkSt(item.Value.IsCamConnected);
                        ds.BoolToolStripAdd(LinKbtn);
                        item.Value.LinkEnvet += Cam_LinkSt;
                        void Cam_LinkSt(bool key)
                        {
                            if (key)
                            {
                                LinKbtn.ForeColor = Color.Green;
                                LinKbtn.Text = item.Value.Name + ":连接成功";
                            }
                            else
                            {
                                LinKbtn.Text = item.Value.Name + ":连接失败";
                                LinKbtn.ForeColor = Color.Red;
                            }
                        }
                    }

                    if (DebugCompiler.GetThis().Run_Mode == ProjectINI.RunMode.Debug)
                    {
                        Vision2.ErosProjcetDLL.Project.User.Loge("Eros", "ErosEE1988");
                    }
                    //添加运行框
                    //UserInterfaceControl formText = new UserInterfaceControl();
                    //formText.Dock = DockStyle.Top;
                    //MainForm1.MainFormF.u2.Controls.Add(formText);
                    //MainForm1.MainFormF.Up();
                    debugCalss.SetUesrContrsl(MainForm1.MainFormF.userInterfaceControl1);
                }

                System.GC.Collect();
                //form.LoadEnd();
                //UPForm1_EventShowObj(Vision.Instance.GetRunNameVision());
                Thread.Sleep(100);
                //this.Hide();
                this.Cursor = Cursors.Default;
                //MainForm1.MainFormF.UserControl2.tabControl1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void LodingProject_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;//拦截，不响应操作
            return;
        }
    }
}
