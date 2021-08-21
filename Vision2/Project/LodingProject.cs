using ErosSocket.ErosConLink;
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
        private Vision2.Project.MainForm1 ds;

        private string[] ImageS;

        private void LodingProject_Load(object sender, EventArgs e)
        {
            ProjectINI.In.UserName = "";
            ProjectINI.In.UserID = "";
            if (ProjectINI.In.UsData.Boot_The_Login)
            {
                LandingForm logMessageForm = new LandingForm(true);
                logMessageForm.ShowDialog();
                if (ProjectINI.In.UserName == "未登陆" || ProjectINI.In.UserName == "")
                {
                    MessageBox.Show("未登录！", "退出程序");
                    ProjectINI.In.Clros();
                }
            }
            ds = new MainForm1();
            ds.Show();
            try
            {
                string[] paths = System.IO.Directory.GetFiles(Application.StartupPath);
                for (int i = 0; i < paths.Length; i++)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(paths[i]);
                    if (fileName.ToLower() == "logo")
                    {
                        pictureBox2.Image = Image.FromFile(paths[i]);
                        break;
                    }
                }
                //if (System.IO.File.Exists(Application.StartupPath + "\\LOGO.jpg"))
                //{
                //    pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\LOGO.jpg");
                //}
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
                    for (int i = 0; i < 5; i++)
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
                    //MainForm1.MainFormF.Invoke(new MethodInvoker(() => {
                    //    RecipeCompiler.GetUserFormulaContrsl().UPSetGetPargam();

                    //}));
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(100);
                        dss1.progressBar2.Value = i * 10;
                    }
                    dss1.label1.Text = "载入完成;";
                    dss1.progressBar1.Value = 100;

                    Thread.Sleep(500);
                    //MainForm1.MainFormF.Invoke(new MethodInvoker(() => {
                    //    RecipeCompiler.GetUserFormulaContrsl().UPSetGetPargam();

                    //}));
                    foreach (var item in Vision.GetHimageList())
                    {
                        item.Value.ShowImage(true);
                    }
                    this.Hide();
                }
                catch (Exception ex)
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

                if (!System.IO.Directory.Exists(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun))
                {
                    ProjectINI.In.AddProject(debugComp);
                    ProjectINI.In.AddProject(dicSocket);
                    ProjectINI.In.AddProject(process);
                    ProjectINI.In.AddProject(Vision.Instance);
                    ProjectINI.In.AddProject(debugCalss);
                }
                else
                {
                    //加载程序参数
                    //ErosSocket.ErosConLink.StaticCon.StataRunSocket(Application.StartupPath + "\\Project\\" + ProjectINI.In.ProjectName + "\\" + ProjectINI.In.RunName + "\\Socket");
                    ProjectINI.ReadPathJsonToCalss<DicSocket>(ProjectINI.ProjectPathRun + "\\Socket\\SocketLink.socket", out dicSocket);
                    dss1.label1.Text = "载入链接信息完成.....";

                    if (dicSocket == null)
                    {
                        dicSocket = new DicSocket();
                    }
                    dicSocket.initialization();

                    dss1.label1.Text = "载入配方信息......";

                    recipeCompiler = recipeCompiler.ReadThis<RecipeCompiler>(ProjectINI.ProjectPathRun);
                    if (recipeCompiler != null)
                    {
                        ProjectINI.In.AddProject(recipeCompiler);
                    }
                    recipeCompiler.initialization();

                    dss1.label1.Text = "载入视觉信息......";
                    Vision.Instance.UpReadThis(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun, Product.ProductionName);
                    if (Vision.Instance.RsetPort >= 0)
                    {
                        MainForm1.MainFormF.Hide();
                        goto endt;
                    }
                    RecipeCompiler.GetUserFormulaContrsl().AddData(RecipeCompiler.Instance.OKNumber);

                    debugComp = debugComp.ReadThis<ErosSocket.DebugPLC.DebugComp>(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun);
                    if (debugComp == null)
                    {
                        debugComp = new ErosSocket.DebugPLC.DebugComp();
                    }
                    debugComp.initialization();

                    debugCalss = debugCalss.ReadThis<DebugCompiler>(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun);
                    if (debugCalss == null)
                    {
                        debugCalss = new DebugCompiler();
                    }

                endt:
                    process = process.ReadThis<ProcessUser>(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun);
                    //添加程序服务
                    ProjectINI.In.AddProject(process);
                    ProjectINI.In.AddProject(Vision.Instance);
                    ProjectINI.In.AddProject(debugComp);
                    ProjectINI.In.AddProject(dicSocket);
                    ProjectINI.In.AddProject(debugCalss);
                    //SocketUI.Project.HMIDIC hMIDIC = new SocketUI.Project.HMIDIC();
                    //hMIDIC = hMIDIC.ReadThis<SocketUI.Project.HMIDIC>( ErosProjcetDLL.Project.ProjectINI.ProjectPathRun);
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
                    if (DebugCompiler.Instance.Run_Mode == ProjectINI.RunMode.Debug)
                    {
                        User.Loge("Eros", "ErosEE1988");
                    }

                    //添加运行框
                    //UserInterfaceControl formText = new UserInterfaceControl();
                    //formText.Dock = DockStyle.Top;
                    //MainForm1.MainFormF.u2.Controls.Add(formText);
                    //MainForm1.MainFormF.Up();
                    debugCalss.initialization();
                }
                MainForm1.MainFormF.Invoke(new MethodInvoker(() =>
                {
                    RecipeCompiler.GetUserFormulaContrsl().UPSetGetPargam();
                }));
                //UserFormulaContrsl.This.tabControl1.TabPages.Remove(UserFormulaContrsl.This.tabPage4);
                //MainForm1.MainFormF.splitContainer3.Panel2Collapsed = false;
                AlarmForm.UpDa(Vision2.Project.DebugF.DebugCompiler.Instance.ErrTextS);

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