using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.PLCUI;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI;
using Vision2.Project.DebugF;

namespace Vision2.Project
{
    public partial class MainForm1 : Form, IMessageFilter
    {
        public enum ToolStripAddStyle
        {
            Right = 0,
            Left = 1,
            File = 2,
            Boon = 3,
            toon = 4,
        }

        //需添加using System.Runtime.InteropServices;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        public bool PreFilterMessage(ref Message msg)
        {
            const int WM_MOUSEMOVE = 0x200;
            if (msg.Msg == WM_MOUSEMOVE)
            {
            }
            return false;
        }

        public MainForm1()
        {
            InitializeComponent();
            MainFormF = this;
            ProjectINI.Form(this);
            this.LeftToolStripAdd("工具箱", new ToolForm());
            MainFormF.Controls.Add(PropertyForm.ThisForm);
            MainFormF.toolStripLeft.Visible = false;
            Application.AddMessageFilter(MainFormF);
            MainFormF.Up();
        }

        private void User_EventLog(bool isLog)
        {
            MainFormF.Up();
        }

        public static MainForm1 MainFormF
        {
            get
            {
                if (FormF == null)
                {
                    FormF = new MainForm1();
                }
                return FormF;
            }
            private set { FormF = value; }
        }

        private static MainForm1 FormF;

        /// <summary>
        ///
        /// </summary>
        public void Up()
        {
            try
            {
                if (ProjectINI.In.UserName == "")
                {
                    ProjectINI.In.UserName = "未登录";
                }
                if (ProjectINI.Enbt)
                {
                    SaveAll.Visible = true;
                }
                else
                {
                    SaveAll.Visible = false;
                }
                toolStripLabel9.Text = DebugCompiler.Instance.DeviceNameText;
                tsButton5.Text = ProjectINI.In.UserName;
                toolStripLabel1.Text = ProjectINI.In.Right + ":" + ProjectINI.In.UserName;
                if (ProjectINI.AdminEnbt)
                {
                    方案管理ToolStripMenuItem.Visible = true;
                }
                else
                {
                    方案管理ToolStripMenuItem.Visible = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public virtual void SetFormMax(Form frm)
        {
            frm.Top = 0;
            frm.Left = 0;
            frm.Width = Screen.PrimaryScreen.WorkingArea.Width;
            frm.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void MainForm1_Load(object sender, EventArgs e)
        {
            try
            {
                ProjectINI.In.User.EventLog += User_EventLog;

                //splitContainer3.Panel2Collapsed = true;
                AlarmListBoxt.AlarmFormThis.Hide();
                timer100.Start();
                timer500.Start();

                string[] paths = Directory.GetFiles(Application.StartupPath);
                for (int i = 0; i < paths.Length; i++)
                {
                    string fileName = Path.GetFileNameWithoutExtension(paths[i]);
                    if (fileName.ToLower() == "logo")
                    {
                        toolStripPictureBox1.BackgroundImage = Image.FromFile(paths[i]);
                        toolStripPictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
                        break;
                    }
                }
                //if (File.Exists(Application.StartupPath + "\\LOGO.jpg"))
                //{
                //    toolStripPictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + "\\LOGO.jpg");
                //    toolStripPictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
                //}
                SetFormMax(this);

                this.LayoutMdi(MdiLayout.TileVertical);
                HMIDIC hMIDIC = new HMIDIC();
                hMIDIC = hMIDIC.ReadThis<HMIDIC>(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun);
                hMIDIC.initialization();
                ProjectINI.In.AddProject(hMIDIC);
                tsButton7.BackColor = Color.Red;

                Thread thread = new Thread(() =>
                {
                    bool isv = false;
                    bool ISa = false;
                    bool ISdT = false;
                    bool iste = false;
                    bool ISdaMen = false;
                    bool StopE = false;
                    while (!this.IsDisposed)
                    {
                        try
                        {
                            if (this.InvokeRequired)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    try
                                    {
                                        CycleEven?.Invoke();
                                        if (DebugF.DebugCompiler.GetDoDi() != null)
                                        {
                                            iste = false;
                                            if (DebugF.DebugCompiler.Instance.RunButton.ANmen >= 0)
                                            {
                                                if (!tsButton7.Visible)
                                                {
                                                    tsButton7.Visible = true;
                                                }
                                                if (isv != DebugF.DebugCompiler.GetDoDi().Out[DebugF.DebugCompiler.Instance.RunButton.ANmen])
                                                {
                                                    isv = DebugF.DebugCompiler.GetDoDi().Out[DebugF.DebugCompiler.Instance.RunButton.ANmen];
                                                    if (!isv)
                                                    {
                                                        tsButton7.Text = "门锁开";
                                                    }
                                                    else
                                                    {
                                                        tsButton7.Text = "门锁关";
                                                    }
                                                }
                                                if (ISa != DebugF.DebugCompiler.GetDoDi().Int[DebugF.DebugCompiler.Instance.RunButton.ANmenTI])
                                                {
                                                    ISa = DebugF.DebugCompiler.GetDoDi().Int[DebugF.DebugCompiler.Instance.RunButton.ANmenTI];
                                                    if (!ISa)
                                                    {
                                                        tsButton7.BackColor = Color.Red;
                                                    }
                                                    else
                                                    {
                                                        DebugF.DebugCompiler.GetDoDi().WritDO(DebugF.DebugCompiler.Instance.RunButton.ANmen, true);
                                                        tsButton7.BackColor = Color.GreenYellow;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (tsButton7.Visible)
                                                {
                                                    tsButton7.Visible = false;
                                                }
                                            }
                                            if (DebugF.DebugCompiler.Instance.RunButton.StopTButten >= 0)
                                            {
                                                if (!toolStripLabel6.Visible)
                                                {
                                                    toolStripLabel6.Visible = true;
                                                }
                                                if (StopE != DebugF.DebugCompiler.GetDoDi().Int[DebugF.DebugCompiler.Instance.RunButton.StopTButten])
                                                {
                                                    StopE = DebugF.DebugCompiler.GetDoDi().Int[DebugF.DebugCompiler.Instance.RunButton.StopTButten];
                                                    if (StopE)
                                                    {
                                                        toolStripLabel6.Image = null;
                                                        ErosProjcetDLL.Project.AlarmListBoxt.RomveAlarm("急停按钮开启");
                                                        toolStripLabel6.BackColor = Color.GreenYellow;
                                                    }
                                                    if (!StopE)
                                                    {
                                                        toolStripLabel6.Image = global::Vision2.Properties.Resources.down_right_vector;
                                                        if (DebugF.DebugCompiler.GetDoDi().IsInitialBool)
                                                        {
                                                            if (DebugF.DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.运行中)
                                                            {
                                                                if (DebugF.DebugCompiler.Instance.IsRunStrat)
                                                                {
                                                                    DebugF.DebugCompiler.ISHome = false;
                                                                }
                                                                DebugF.DebugCompiler.RunStop = true;
                                                                DebugF.DebugCompiler.Stop();
                                                                ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText("运行中急停按钮开启停止");
                                                            }
                                                            if (DebugF.DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.初始化中)
                                                            {
                                                                if (DebugF.DebugCompiler.Instance.IsRunStrat)
                                                                {
                                                                    DebugF.DebugCompiler.ISHome = false;
                                                                }
                                                                DebugF.DebugCompiler.RunStop = true;
                                                                DebugF.DebugCompiler.Stop();
                                                                ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText("初始化中急停按钮开启停止");
                                                            }
                                                        }
                                                        ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText("急停按钮开启");
                                                        toolStripLabel6.BackColor = Color.Red;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (toolStripLabel6.Visible)
                                                {
                                                    toolStripLabel6.Visible = false;
                                                }
                                            }

                                            if (DebugF.DebugCompiler.Instance.RunButton.adMent1 >= 0)
                                            {
                                                if (!toolStripLabel5.Visible)
                                                {
                                                    toolStripLabel5.Visible = true;
                                                }
                                                if (ISdaMen != DebugF.DebugCompiler.GetDoDi().Int[DebugF.DebugCompiler.Instance.RunButton.adMent1])
                                                {
                                                    ISdaMen = DebugF.DebugCompiler.GetDoDi().Int[DebugF.DebugCompiler.Instance.RunButton.adMent1];
                                                    if (ISdaMen)
                                                    {
                                                        ErosProjcetDLL.Project.AlarmListBoxt.RomveAlarm("安全门打开");
                                                        toolStripLabel5.BackColor = Color.GreenYellow;
                                                    }
                                                }
                                                if (!ISdaMen)
                                                {
                                                    if (DebugF.DebugCompiler.GetDoDi().IsInitialBool)
                                                    {
                                                        if (DebugF.DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.运行中)
                                                        {
                                                            if (DebugF.DebugCompiler.Instance.IsRunStrat)
                                                            {
                                                                DebugF.DebugCompiler.ISHome = false;
                                                            }
                                                            DebugF.DebugCompiler.RunStop = true;
                                                            DebugF.DebugCompiler.Stop();
                                                            ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText("运行中安全检测停止");
                                                        }
                                                        if (DebugF.DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.初始化中)
                                                        {
                                                            if (DebugF.DebugCompiler.Instance.IsRunStrat)
                                                            {
                                                                DebugF.DebugCompiler.ISHome = false;
                                                            }
                                                            DebugF.DebugCompiler.RunStop = true;
                                                            DebugF.DebugCompiler.Stop();
                                                            ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText("初始化中安全检测停止");
                                                        }
                                                        ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText("安全门打开");
                                                        toolStripLabel5.BackColor = Color.Red;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (toolStripLabel5.Visible)
                                                {
                                                    toolStripLabel5.Visible = false;
                                                }
                                            }
                                            if (DebugF.DebugCompiler.Instance.RunButton.ANmenI >= 0)
                                            {
                                                if (!toolStripButton2.Visible)
                                                {
                                                    toolStripButton2.Visible = true;
                                                }
                                                if (ISdT != DebugF.DebugCompiler.GetDoDi().Int[DebugF.DebugCompiler.Instance.RunButton.ANmenI])
                                                {
                                                    ISdT = DebugF.DebugCompiler.GetDoDi().Int[DebugF.DebugCompiler.Instance.RunButton.ANmenI];
                                                    if (ISdT)
                                                    {
                                                        toolStripButton2.BackColor = Color.GreenYellow;
                                                    }
                                                }
                                                if (!ISdT)
                                                {
                                                    if (DebugF.DebugCompiler.GetDoDi().IsInitialBool)
                                                    {
                                                        if (DebugF.DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.运行中)
                                                        {
                                                            if (DebugF.DebugCompiler.Instance.IsRunStrat)
                                                            {
                                                                DebugF.DebugCompiler.ISHome = false;
                                                            }
                                                            DebugF.DebugCompiler.RunStop = true;
                                                            DebugF.DebugCompiler.Stop();
                                                            ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText("运行中安全检测停止");
                                                        }
                                                        if (DebugF.DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.初始化中)
                                                        {
                                                            if (DebugF.DebugCompiler.Instance.IsRunStrat)
                                                            {
                                                                DebugF.DebugCompiler.ISHome = false;
                                                            }
                                                            DebugF.DebugCompiler.RunStop = true;
                                                            DebugF.DebugCompiler.Stop();
                                                            ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText("初始化中安全检测停止");
                                                        }
                                                    }
                                                    toolStripButton2.BackColor = Color.Red;
                                                }
                                            }
                                            else
                                            {
                                                if (toolStripButton2.Visible)
                                                {
                                                    toolStripButton2.Visible = false;
                                                }
                                            }
                                            if (DebugF.DebugCompiler.Instance.IsCtr != toolStripCheckbox2.Visible)
                                            {
                                                toolStripCheckbox2.Visible = DebugF.DebugCompiler.Instance.IsCtr;
                                            }
                                        }
                                        else if (!iste)
                                        {
                                            iste = true;

                                            toolStripCheckbox2.Visible = false;
                                            tsButton7.Visible = false;
                                            toolStripButton2.Visible = false;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    toolStripLabel4.Text = DateTime.Now.ToString();
                                }));
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
            catch (Exception ex)
            {
                MessageBox.Show("加载主窗口错误：" + ex.Message);
            }
        }

        public void RunCodeT_RunStratCode(DebugF.IO.RunCodeStr.RunErr key)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    toolStripLabel8.Text = key.runCoStr.ToString() + key.RowIndx + ":" + key.Code + key.RunTime;
                }));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 链接委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public delegate void FromCyc();

        public event FromCyc CycleEven;

        /// <summary>
        /// 左边添加按钮
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="control"></param>
        public void LeftToolStripAdd(string buttonName, Control control)
        {
            Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton toolStripButton = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            toolStripButton.Text = toolStripButton.Name = buttonName;
            toolStripButton.ForeColor = toolStripButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            toolStripLeft.Items.Add(toolStripButton);
            toolStripButton.Click += ToolStripButton_Click1;
            void ToolStripButton_Click1(object sender, EventArgs e)
            {
                try
                {
                    Control[] controls = splitContainer1.Panel1.Controls.Find("Splitter" + toolStripButton.Name, false);
                    if (controls.Length == 1)
                    {
                        if (controls[0].Visible)
                        {
                            controls[0].Visible = false;
                            splitContainer1.Panel1.Controls.Find("pane" + toolStripButton.Name, false)[0].Visible = false;
                        }
                        else
                        {
                            controls[0].Visible = true;
                            controls = splitContainer1.Panel1.Controls.Find("pane" + toolStripButton.Name, false);
                            if (controls.Length == 1)
                            {
                                controls[0].Visible = true;
                            }
                        }
                    }
                    else
                    {
                        Panel panel = new Panel();

                        Splitter splitter = new Splitter();
                        splitter.Name = "Splitter" + toolStripButton.Name;
                        panel.Name = "pane" + toolStripButton.Name;
                        panel.Left = 200;
                        Form form = control as Form;
                        if (form != null)
                        {
                            form.TopLevel = false;
                            form.FormBorderStyle = FormBorderStyle.None;
                            form.ControlBox = false;
                        }
                        control.Dock = DockStyle.Fill;
                        panel.Controls.Add(control);
                        control.Show();
                        splitter.Dock = DockStyle.Left;
                        splitContainer1.Panel1.Controls.Add(splitter);
                        panel.Dock = DockStyle.Left;
                        splitContainer1.Panel1.Controls.Add(panel);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //private List<Control> ListFileControl = new List<Control>();

        /// <summary>
        /// 添加到底部工具栏
        /// </summary>
        /// <param name="toolStripItem"></param>
        public void BoolToolStripAdd(ToolStripItem toolStripItem)
        {
            toolStrip1.Items.Add(toolStripItem);
        }

        private LandingForm landingForm = new LandingForm();

        private bool toolLeftVisible = false;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (toolLeftVisible)
            {
                toolStripLeft.Visible = false;
            }
            else
            {
                toolStripLeft.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否退出程序？", "退出程序", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                ProjectINI.In.Clros();
            }
        }

        private void ToolSripButtonHelp_Click(object sender, EventArgs e)
        {
            string[] dtas = Directory.GetFiles(Application.StartupPath);
            if (File.Exists(Application.StartupPath + @"\help.chm"))
            {
                CHMHelp.ShowHelp();
                //Help.ShowHelp(null, Application.StartupPath + @"\help.chm");
            }
            else
            {
                if (File.Exists(Application.StartupPath + "\\Hellp.PDF"))
                {
                    //axAcroPDF1.LoadFile(Application.StartupPath + @"\PDF\系统手册.PDF");
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\Hellp.PDF");
                }
                else if (File.Exists(Application.StartupPath + "\\Hellp.docx"))
                {
                    //axAcroPDF1.LoadFile(Application.StartupPath + @"\PDF\系统手册.PDF");
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\Hellp.docx");
                }
            }
        }

        private void 平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.LayoutMdi(MdiLayout.TileVertical);
            }
            catch (Exception)
            {
            }
        }

        private void toolStripLeft_MouseLeave(object sender, EventArgs e)
        {
            if (隐藏工具ToolStripMenuItem.Checked)
            {
                toolStripLeft.Visible = false;
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath);
        }

        private void 打开历史数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(Application.StartupPath + "\\历史数据");
                System.Diagnostics.Process.Start(Application.StartupPath + "\\历史数据");
            }
            catch (Exception)
            {
            }
        }

        private void 打开目录文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath);
            }
            catch (Exception)
            {
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
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

        private void 截取屏幕ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Directory.CreateDirectory(Application.StartupPath + "\\截取屏幕\\");
                Bitmap bitmap = UICon.GetScreenCapture();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                //saveFileDialog.InitialDirectory = Application.StartupPath + @"\截取屏幕\";
                saveFileDialog.Filter = "图像|*.jpg";
                string timeStr = DateTime.Now.ToString("HH时mm分ss秒截屏");
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

        private void splitContainer1_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void tsButton4_Click(object sender, EventArgs e)
        {
        }

        private void toolStripDropDownButton4_Click(object sender, EventArgs e)
        {
        }

        private void SaveAll_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ProjectINI.In.SaveProjectAll();

            Cursor = Cursors.Arrow;
        }

        private void MainForm1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (Keys.F1 == e.KeyCode)
                {
                    ToolSripButtonHelp.PerformClick();
                }
                else if (Keys.F2 == e.KeyCode)
                {
                    if (File.Exists(Application.StartupPath + @"\help.chm"))
                    {
                        Help.ShowHelp(null, Application.StartupPath + @"\help.chm", HelpNavigator.Topic, "设备介绍");
                    }
                }
                else if (Keys.F3 == e.KeyCode)
                {
                    if (File.Exists(Application.StartupPath + @"\help.chm"))
                    {
                        Help.ShowHelp(null, Application.StartupPath + @"\help.chm", HelpNavigator.TableOfContents, "设备介绍");
                    }
                }
                else if (Keys.F4 == e.KeyCode)
                {
                    if (File.Exists(Application.StartupPath + @"\help.chm"))
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void tsButton5_Click(object sender, EventArgs e)
        {
            Vision2.ErosProjcetDLL.UI.UICon.WindosFormerShow(ref landingForm);
        }

        private void tsButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (DebugF.DebugCompiler.Instance.DDAxis.Out[DebugF.DebugCompiler.Instance.RunButton.ANmen])
                {
                    DebugF.DebugCompiler.Instance.DDAxis.WritDO(DebugF.DebugCompiler.Instance.RunButton.ANmen, false);
                }
                else
                {
                    DebugF.DebugCompiler.Instance.DDAxis.WritDO(DebugF.DebugCompiler.Instance.RunButton.ANmen, true);
                }
            }
            catch (Exception)
            {
            }
        }

        private void MainForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.TaskManagerClosing)
            {
                e.Cancel = true;
            }
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }

        private void 查看电气图纸ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\电气图纸.PDF"))
                {
                    Process.Start(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\电气图纸.PDF");
                }
                else
                {
                    MessageBox.Show("未找到相关电气图纸");
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripCheckbox2_Click(object sender, EventArgs e)
        {
            try
            {
                DebugF.DebugCompiler.CPMode = (toolStripCheckbox2.Control as CheckBox).Checked;
            }
            catch (Exception)
            {
            }
        }

        private void toolStrip4_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private Project.DebugF.IO.IOForm IOForm;

        private void 查看操作手册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (IOForm == null || IOForm.IsDisposed)
                {
                    IOForm = new DebugF.IO.IOForm();
                }

                UICon.WindosFormerShow(ref IOForm);
            }
            catch (Exception)
            {
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private NewProjectForm newProjectForm;

        private void 方案管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (newProjectForm == null || newProjectForm.IsDisposed)
            {
                newProjectForm = new NewProjectForm();
            }
            UICon.WindosFormerShow(ref newProjectForm);
            //newProjectForm.Show();
        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (AlarmListBoxt.AlarmFormThis.Visible)
                {
                    AlarmListBoxt.AlarmFormThis.Hide();
                }
                else
                {
                    AlarmListBoxt.AlarmFormThis.Show();
                }
            }
            catch (Exception ex)
            {
            }
            //Vision2.ErosProjcetDLL.Project.AlarmListBoxt.AlarmFormThis.Show();
            //Vision2.ErosProjcetDLL.Project.AlarmListBoxt.AlarmFormThis.Visible = true;
        }

        private void 拆分ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (splitContainer1.IsSplitterFixed)
            {
                splitContainer1.IsSplitterFixed = false;
                foreach (Control item in splitContainer1.Panel1.Controls)
                {
                    if (item is Splitter)
                    {
                        Splitter split = item as Splitter;
                        split.Enabled = false;
                    }
                }
                拆分ToolStripMenuItem.Text = "固定窗口";
            }
            else
            {
                splitContainer1.IsSplitterFixed = true;
                foreach (Control item in splitContainer1.Panel1.Controls)
                {
                    if (item is Splitter)
                    {
                        Splitter split = item as Splitter;
                        split.Enabled = true;
                    }
                }
                拆分ToolStripMenuItem.Text = "拆分窗口";
            }
        }

        private void 隐藏工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (隐藏工具ToolStripMenuItem.Checked)
            {
                隐藏工具ToolStripMenuItem.Checked = false;
                toolStripLeft.Visible = true;
            }
            else
            {
                隐藏工具ToolStripMenuItem.Checked = true;
                toolStripLeft.Visible = false;
            }
        }

        private void 打开目录文件夹ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath);
        }

        private void ToolSripButtonHelp_MouseMove(object sender, MouseEventArgs e)
        {
            ToolSripButtonHelp.ShowDropDown();
        }

        private void timer100_Tick(object sender, EventArgs e)
        {
        }

        public static bool HZ500;
        private bool isbuz;

        private void timer500_Tick(object sender, EventArgs e)
        {
            try
            {
                toolStripLabel7.Text = ProjectINI.GetMemory();
                toolStripLabel7.Text += ProjectINI.GetMemoryDW() + "%";
                toolStripLabel7.ToolTipText = ProjectINI.GetMemoryall();
                if (HZ500)
                {
                    HZ500 = false;
                }
                else
                {
                    HZ500 = true;
                }
                if (DebugF.DebugCompiler.Buzzer)
                {
                    if (isbuz != DebugF.DebugCompiler.Buzzer)
                    {
                        isbuz = DebugF.DebugCompiler.Buzzer;
                    }
                    if (HZ500)
                    {
                        DebugF.DebugCompiler.GetDoDi().WritDO(DebugF.DebugCompiler.Instance.RunButton.ResetButtenS, true);
                        DebugF.DebugCompiler.GetDoDi().WritDO(DebugF.DebugCompiler.Instance.RunButton.red, true);
                        if (!DebugF.DebugCompiler.FmqIS)
                        {
                            DebugF.DebugCompiler.GetDoDi().WritDO(DebugF.DebugCompiler.Instance.RunButton.Fmq, true);
                        }
                    }
                    else
                    {
                        DebugF.DebugCompiler.GetDoDi().WritDO(DebugF.DebugCompiler.Instance.RunButton.red, false);
                        DebugF.DebugCompiler.GetDoDi().WritDO(DebugF.DebugCompiler.Instance.RunButton.Fmq, false);
                        DebugF.DebugCompiler.GetDoDi().WritDO(DebugF.DebugCompiler.Instance.RunButton.ResetButtenS, false);
                    }
                }
                else
                {
                    if (isbuz != DebugF.DebugCompiler.Buzzer)
                    {
                        isbuz = DebugF.DebugCompiler.Buzzer;
                        DebugF.DebugCompiler.GetDoDi().WritDO(DebugF.DebugCompiler.Instance.RunButton.red, false);
                        DebugF.DebugCompiler.GetDoDi().WritDO(DebugF.DebugCompiler.Instance.RunButton.Fmq, false);
                        DebugF.DebugCompiler.GetDoDi().WritDO(DebugF.DebugCompiler.Instance.RunButton.ResetButtenS, false);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void 窗口模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.FormBorderStyle == FormBorderStyle.None)
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
            }
        }

        private void 版本信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private vision.LibraryForm1 LibraryForm1;

        private void 库管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (LibraryForm1 == null || LibraryForm1.IsDisposed)
                {
                    LibraryForm1 = new vision.LibraryForm1();
                }
                UICon.WindosFormerShow(ref LibraryForm1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private FOVForm FOVForm;

        private void 镜头信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (FOVForm == null || FOVForm.IsDisposed)
                {
                    FOVForm = new FOVForm();
                }
                UICon.WindosFormerShow(ref FOVForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (DebugCompiler.Instance.IsRunStrat)
                {
                    DebugCompiler.ISHome = false;
                }
                DebugF.DebugCompiler.RunStop = true;
                DebugCompiler.Stop();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Pause_Click(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.Pause();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DebugForm debugForm;

        private void Btn_Debug_Click(object sender, EventArgs e)
        {
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
        }

        private void 初始化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 复位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 复位ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
        }

        private void 初始化ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.Initialize();
                labelStat1.Text = DebugCompiler.EquipmentStatus.ToString();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsButton1_Click_2(object sender, EventArgs e)
        {
            try
            {
                if (ProjectINI.Enbt || User.MatchThePermissions("管理权限"))
                {
                    if (debugForm == null)
                    {
                        debugForm = new DebugForm();
                    }
                    UICon.WindosFormerShow(ref debugForm);
                    DebugCompiler.Debuging = true;
                    if (DebugCompiler.Instance.LinkAutoMode != null && DebugCompiler.Instance.LinkAutoMode != "")
                    {
                        if (ErosSocket.ErosConLink.StaticCon.GetLingkIDValue(DebugCompiler.Instance.LinkAutoMode, ErosSocket.ErosConLink.UClass.Boolean, out dynamic valueDy))
                        {
                            if (valueDy)
                            {
                                DialogResult dialogResult = MessageBox.Show("是否切换为手动模式", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(DebugCompiler.Instance.LinkAutoMode, false);
                                    //ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(DebugCompiler.GetThis().LinkAutoMode, true);
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("用户权限不足！需要管理员权限");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 复位ToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            try
            {
                AlarmText.ResetAlarm();
                DebugCompiler.Rest();
                DebugCompiler.Buzzer = false;
                DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.RunButton.red, false);
                DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.RunButton.Fmq, false);
                DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.RunButton.ResetButtenS, false);
                //this.Btn_Reset.KeyValuePairs = this.data.LinkRestoration;
                //labelStat.Text = DebugCompiler.EquipmentStatus.ToString();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 初始化ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.Initialize();
                labelStat1.Text = DebugCompiler.EquipmentStatus.ToString();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void userFormulaContrsl1_Load(object sender, EventArgs e)
        {
        }

        private void toolStripDropDownButton1_DropDownOpened(object sender, EventArgs e)
        {
            toolStripMenuItem1.DropDownItems.Clear();
            foreach (var item in ErosSocket.ErosConLink.StaticCon.GetLingkNames())
            {
                ToolStripItem toolStripButton = toolStripMenuItem1.DropDownItems.Add(item);
                toolStripButton.Tag = ErosSocket.ErosConLink.StaticCon.GetSocketClint(item);
                if (ProjectINI.Enbt)
                {
                    toolStripButton.Enabled = true;
                }
                else
                {
                    toolStripButton.Enabled = false;
                }
                toolStripButton.Click += ToolStripButton_Click;
            }
        }

        private void ToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripItem toolStripButton = sender as ToolStripItem;
                ErosSocket.ErosConLink.SocketClint socketClint = ErosSocket.ErosConLink.StaticCon.GetSocketClint(toolStripButton.Text);
                Form form = socketClint.ShowForm();
                form.Show();
            }
            catch (Exception)
            {
            }
        }

        private void 查看操作手册ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\操作手册.PDF"))
                {
                    Process.Start(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\操作手册.PDF");
                }
                else
                {
                    MessageBox.Show("未找到相关操作手册");
                }
            }
            catch (Exception)
            {
            }
        }
    }
}