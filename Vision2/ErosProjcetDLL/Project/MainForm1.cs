using ErosProjcetDLL.Project;
using ErosProjcetDLL.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ErosProjcetDLL
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
                //this.Text = "Mouse = " + Cursor.Position;
                if (!隐藏ToolStripMenuItem.Checked)
                {
                    if (Cursor.Position.X <= this.Left + 15)
                    {
                        toolStripLeft.Visible = true;
                    }
                }
       
            }
            return false;
        }

        public MainForm1()
        {
            InitializeComponent();
            MainFormF = this;
            this.LeftToolStripAdd("工具箱", new ToolForm());

            MainFormF.Controls.Add(PropertyForm.ThisForm);
            MainFormF.toolStripLeft.Visible = false;
            Application.AddMessageFilter(MainFormF);

            DrawTabControl dtc = null;

            dtc = new DrawTabControl(MainFormF.tabControlEx1, this.Font);
            dtc.ClearPage();
            MainFormF.tabControlEx1.Dock = DockStyle.Fill;
            MainFormF.tabControlEx1.Visible = false;
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
            } private set { FormF = value; } }

        static MainForm1 FormF;
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
                if (ProjectINI.GetEndt())
                {
                    SaveAll.Visible = true;
                }
                toolStripLabel1.Text = ProjectINI.In.Right + ":" + ProjectINI.In.UserName;
                alarmText1.Visible = ProjectINI.In.IsAlramText;
                UserControl2.Up();
            }
            catch (Exception)
            {

               
            }

        }
  

        private void MainForm1_Load(object sender, EventArgs e)
        {
            try
            {

                if (Project.ProjectINI.In.Run_Mode == ProjectINI.RunMode.Run)
                {
       
                }
                if (File.Exists(Application.StartupPath+"Logo.jgp"))
                {

                }

                tsButton2.Visible = tsButton1.Visible = ProjectINI.DebugMode;
                this.WindowState = FormWindowState.Maximized;
                this.LayoutMdi(MdiLayout.TileVertical);
                PLCUI.HMIDIC hMIDIC = new PLCUI.HMIDIC();
                hMIDIC = hMIDIC.ReadThis<PLCUI.HMIDIC>(ProjectINI.In.ProjectPathRun);
                hMIDIC.UP();
                ProjectINI.In.AddProject(hMIDIC);

                Thread thread = new Thread(() =>{
                
                        while (!this.IsDisposed)
                        {
                            try
                            {
                                this.Invoke(new Action(() => {

                                    CycleEven?.Invoke();
                                }));
                            }
                            catch (Exception)
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
            //foreach (var item in this.MdiChildren)
            //{
            //    //item.FormBorderStyle = FormBorderStyle.None;
            //}
        }

        private ProjectNodet porejectNodet;

        /// <summary>
        /// 链接委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public delegate void FromCyc();

        public event FromCyc CycleEven;


        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                //for (int i = 0; i < splitContainer1.Panel1.Controls.Count; i++)
                //{
                //    splitContainer1.Panel1.Controls[i].Visible = false;
                //}

                if (tabControlEx1.Visible)
                {
                    tabControlEx1.Visible = false;
                }
                else
                {
                    tabControlEx1.Visible = true;
                }
            }
            catch (Exception)
            {
            }
        }

        ///// <summary>
        ///// 右边按钮窗口
        ///// </summary>
        ///// <param name="buttonName">按钮名称</param>
        ///// <param name="control">控件</param>
        ///// <param name="isDispy">初始是否显示</param>
        //public UI.ToolStrip.TSButton RightToolStripAdd(string buttonName, Control control, bool isDispy = false)
        //{
        //    UI.ToolStrip.TSButton toolStripButton = new UI.ToolStrip.TSButton();
        //    toolStripButton.Text = toolStripButton.Name = buttonName;
        //    toolStripButton.ForeColor = toolStripButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
        //    toolStripRight.Items.Add(toolStripButton);
        //    toolStripButton.Click += ToolStripButton_Click1;

        //    void ToolStripButton_Click1(object sender, EventArgs e)
        //    {
        //        try
        //        {
        //            Control[] controls = splitContainer1.Panel1.Controls.Find("Splitter" + toolStripButton.Name, false);
        //            if (controls.Length == 1)
        //            {
        //                if (controls[0].Visible)
        //                {
        //                    controls[0].Visible = false;
        //                    splitContainer1.Panel1.Controls.Find("pane" + toolStripButton.Name, false)[0].Visible = false;
        //                }
        //                else
        //                {
        //                    controls[0].Visible = true;
        //                    controls = splitContainer1.Panel1.Controls.Find("pane" + toolStripButton.Name, false);
        //                    if (controls.Length == 1)
        //                    {
        //                        controls[0].Visible = true;
        //                    }
        //                    if (control.IsDisposed)
        //                    {
        //                        string[] dat = control.GetType().ToString().Split('.');
        //                        Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
        //                        dynamic obj = assembly.CreateInstance(control.GetType().ToString()); // 创建类的实例
        //                        if (obj != null)
        //                        {
        //                            control = obj;
        //                            Form form = control as Form;
        //                            if (form != null)
        //                            {
        //                                form.TopLevel = false;
        //                                form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        //                                form.Disposed += Control_Disposed;
        //                            }
        //                            control.Dock = DockStyle.Fill;
        //                            controls[0].Controls.Add(control);
        //                            control.Show();
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Panel pa = new Panel();
        //                Splitter splitter = new Splitter();
        //                splitter.Enabled = false;
        //                splitter.Name = "Splitter" + toolStripButton.Name;
        //                pa.Name = "pane" + toolStripButton.Name;
        //                pa.Size = new Size(control.Right, 1000);
                   
        //                Form form = control as Form;
        //                if (form != null)
        //                {
        //                    form.TopLevel = false;
        //                    form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        //                }
        //                control.Dock = DockStyle.Fill;
        //                pa.Controls.Add(control);
        //                control.Show();
        //                splitter.Dock = DockStyle.Right;
        //                splitContainer1.Panel1.Controls.Add(splitter);
        //                pa.Dock = DockStyle.Right;
        //                splitContainer1.Panel1.Controls.Add(pa);
        //                pa.AutoScroll = false;
        //                pa.AutoSize = false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //    if (isDispy)
        //    {
        //        toolStripButton.PerformClick();
        //        Form form = control as Form;
        //        if (form != null)
        //        {
        //            form.FormBorderStyle = FormBorderStyle.None;
        //        }
        //    }
        //    ToolStripMenuItem toolStripButtn = new ToolStripMenuItem();
        //    toolStripButtn.Text = toolStripButtn.Name = buttonName;
        //    右工具栏ToolStripMenuItem.DropDownItems.Add(toolStripButtn);
        //    toolStripButtn.Click += ToolStripButtn_Click;
        //    void ToolStripButtn_Click(object sender, EventArgs e)
        //    {
        //        toolStripButton.Visible = toolStripButtn.Checked;
        //        if (toolStripButtn.Checked)
        //        {
        //            toolStripButtn.Checked = false;
        //        }
        //        else
        //        {
        //            toolStripButtn.Checked = true;
        //        }
        //    }
        //    control.Disposed += Control_Disposed;
        //    void Control_Disposed(object sender, EventArgs e)
        //    {
        //        toolStripButton.PerformClick();
        //    }
        //    return toolStripButton;
        //}

        /// <summary>
        /// 左边添加按钮
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="control"></param>
        public void LeftToolStripAdd(string buttonName, Control control)
        {
            UI.ToolStrip.TSButton toolStripButton = new UI.ToolStrip.TSButton();
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

        /// <summary>
        /// 左边添加按钮
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="control"></param>
        public void LeftToolStripAdd(Control control,bool cut_in=false)
        {
            UI.ToolStrip.TSButton toolStripButton = new UI.ToolStrip.TSButton();
            toolStripButton.Text = toolStripButton.Name = control.Name;
            toolStripButton.ForeColor = toolStripButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            toolStripLeft.Items.Add(toolStripButton);
            toolStripButton.Click += ToolStripButton_Click1;
            void ToolStripButton_Click1(object sender, EventArgs e)
            {
                try
                {
                    if (cut_in)
                    {
                        return;
                    }
               
                    if (control.Visible)
                    {
                        control.Visible = false;
                    }
                    else
                    {
                        control.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            Form formt = control as Form;
            if (formt != null)
            {
                formt.TopLevel = false;
                formt.FormBorderStyle = FormBorderStyle.None;
                formt.ControlBox = false;
            }
            control.Dock = DockStyle.Left;
            control.Show();
            splitContainer1.Panel1 .Controls.Add(control);
        }
        /// <summary>
        /// 中心添加填充控件
        /// </summary>
        /// <param name="buttonName">按钮名称</param>
        /// <param name="control">控件</param>
        /// <param name="isDispy">是否显示</param>
        public UI.ToolStrip.TSButton FileToolStripAdd(string buttonName, Control control, bool isDispy = false)
        {
            UI.ToolStrip.TSButton toolStripButton = new UI.ToolStrip.TSButton();
            control.Text = control.Name = buttonName;
            toolStripButton.Text = toolStripButton.Name = buttonName;
            toolStripButton.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            toolStripButton.ForeColor = toolStripButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));

            toolStrip4.Items.Add(toolStripButton);
            ListFileControl.Add(control);
            Form form = control as Form;
            if (form != null)
            {
                form.TopLevel = false;
                form.ShowInTaskbar = false;
                form.ControlBox = false;
            }
            form.Dock = DockStyle.Fill;
            splitContainer3.Panel1.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            control.Show();
            if (!isDispy)
            {
                control.Visible = false;
            }
            else
            {
                MaisUi = control;
            }
            toolStripButton.Click += ToolStripButton_Click1;
            void ToolStripButton_Click1(object sender, EventArgs e)
            {
                try
                {
                    Control[] controls = splitContainer3.Panel1.Controls.Find(buttonName, false);

                    if (controls.Length == 1)
                    {
                        if (controls[0].Visible)
                        {
                            int fat = 0;
                            for (int i = 0; i < ListFileControl.Count; i++)
                            {
                                if (!ListFileControl[i].Visible)
                                {
                                    fat++;
                                }
                            }
                            if (fat == ListFileControl.Count)
                            {
                                MaisUi.Visible = true;
                            }
                            return;
                            controls[0].Visible = false;
                        }
                        else
                        {
                            controls[0].Visible = true;
                            for (int i = 0; i < ListFileControl.Count; i++)
                            {
                                if (controls[0].Name != ListFileControl[i].Name)
                                {
                                    ListFileControl[i].Visible = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        form = control as Form;
                        if (form != null)
                        {
                            form.TopLevel = false;
                            form.ShowInTaskbar = false;
                            form.ControlBox = false;
                            form.FormBorderStyle = FormBorderStyle.None;
                        }
                        splitContainer3.Panel1.Controls.Add(control);
                        control.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            control.VisibleChanged += Control_VisibleChanged;
            void Control_VisibleChanged(object sender, EventArgs e)
            {
                if (!control.Visible)
                {
                    toolStripButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                }
                else
                {
                    toolStripButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
                }
            }
            return toolStripButton;
        }

        /// <summary>
        /// 添加到上方右侧工具栏
        /// </summary>
        /// <param name="toolStripItem"></param>
        public void FileToolStripAdd(ToolStripItem toolStripItem)
        {
            toolStrip4.Items.Add(toolStripItem);
        }

        /// <summary>
        /// 添加控件到主界面，并显示在各个位置
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="control"></param>
        /// <param name="toolStripAddS"></param>
        public void ToolStripUIAdd(string buttonName, Control control, ToolStripAddStyle toolStripAddS)
        {
            switch (toolStripAddS)
            {
                case ToolStripAddStyle.Right:
                    break;

                case ToolStripAddStyle.Left:
                    break;

                case ToolStripAddStyle.File:
                    break;

                case ToolStripAddStyle.Boon:
                    break;

                case ToolStripAddStyle.toon:
                    break;

                default:
                    break;
            }
        }

        private Control MaisUi = null;
        private List<Control> ListFileControl = new List<Control>();

        /// <summary>
        /// 添加到底部工具栏
        /// </summary>
        /// <param name="toolStripItem"></param>
        public void BoolToolStripAdd(ToolStripItem toolStripItem)
        {
            toolStrip1.Items.Add(toolStripItem);
        }  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="control"></param>
        /// <param name="isDispy"></param>
        public void BoolToolStripAdd(string buttonName, Control control, bool isDispy = false)
        {
            UI.ToolStrip.TSButton toolStripButton = new UI.ToolStrip.TSButton();
            control.Text = control.Name = buttonName;
            toolStripButton.Text = toolStripButton.Name = buttonName;
            toolStripButton.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            toolStripButton.ForeColor = toolStripButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            toolStrip4.Items.Add(toolStripButton);
            ListFileControl.Add(control);
            Form form = control as Form;
            if (form != null)
            {
                form.TopLevel = false;
                form.ShowInTaskbar = false;
                form.ControlBox = false;
            }
            splitContainer1.Panel1.Controls.Add(control);
            control.Dock = DockStyle.Bottom;
            control.Show();
            if (!isDispy)
            {
                control.Visible = false;
            }
            else
            {
                MaisUi = control;
            }
            toolStripButton.Click += ToolStripButton_Click1;
            void ToolStripButton_Click1(object sender, EventArgs e)
            {
                try
                {
                    Control[] controls = splitContainer1.Panel1.Controls.Find(buttonName, false);

                    if (controls.Length == 1)
                    {
                        if (controls[0].Visible)
                        {
                            int fat = 0;
                            for (int i = 0; i < ListFileControl.Count; i++)
                            {
                                if (!ListFileControl[i].Visible)
                                {
                                    fat++;
                                }
                            }
                            if (fat == ListFileControl.Count)
                            {
                                MaisUi.Visible = true;
                            }
                            return;
                            controls[0].Visible = false;
                        }
                        else
                        {
                            controls[0].Visible = true;
                            for (int i = 0; i < ListFileControl.Count; i++)
                            {
                                if (controls[0].Name != ListFileControl[i].Name)
                                {
                                    ListFileControl[i].Visible = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        form = control as Form;
                        if (form != null)
                        {
                            form.TopLevel = false;
                            form.ShowInTaskbar = false;
                            form.ControlBox = false;
                            form.FormBorderStyle = FormBorderStyle.None;
                        }
                        splitContainer1.Panel1.Controls.Add(control);
                        control.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            control.VisibleChanged += Control_VisibleChanged;
            void Control_VisibleChanged(object sender, EventArgs e)
            {
                if (!control.Visible)
                {
                    toolStripButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                }
                else
                {
                    toolStripButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
                }
            }
        }

        /// <summary>
        /// 添加上方工具栏
        /// </summary>
        //public void TolToolStripAdd(ToolStripItem toolStripItem)
        //{
        //    try
        //    {
        //        toolStrip3.Items.Add(toolStripItem);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        private LandingForm landingForm = new LandingForm();

        private void TsButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (splitContainer1.Panel2Collapsed)
                {
                    splitContainer1.Panel2Collapsed = false;
                    if (porejectNodet == null || porejectNodet.IsDisposed)
                    {
                        porejectNodet = new ProjectNodet(splitContainer1.Panel2, tabControlEx1);
                    }
                    porejectNodet.UpProject();
                    porejectNodet.Show();
                }
                else
                {
                    splitContainer1.Panel2Collapsed = true;
                }
            }
            catch (Exception)
            {
            }
        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            tsButton2.Visible= tsButton1.Visible = false;
            ProjectINI.DebugMode = false;
            调试模式ToolStripMenuItem.Checked = false;
            ProjectINI.In.OnModeEvenT("All", false);

        }

        private void tsButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProjectINI.SelpMode)
                {
                    if (ProjectINI.In.OnModeEvenT("All", false))
                    {
                        tsButton2.BackColor = Color.GreenYellow;
                    }
                    else
                    {
                        tsButton2.BackColor = Color.Red;
                    }
                }
                else
                {
                    if (ProjectINI.In.OnModeEvenT("All", true))
                    {
                        tsButton2.BackColor = Color.GreenYellow;
                    }
                    else
                    {
                        tsButton2.BackColor = Color.Red;
                    }
                }


            }
            catch (Exception)
            {

            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
        }

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

            if (MessageBox.Show("是否退出程序？", "退出程序", MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                ProjectINI.In.Clros();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UI.UICon.WindosFormerShow(ref landingForm);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
        }

        private HolpForm HolpForm = new HolpForm();

        private void ToolSripButtonHelp_Click(object sender, EventArgs e)
        {
            UI.UICon.WindosFormerShow(ref HolpForm);
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

        private void 水平平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void 层叠ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void 移动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void toolStripLeft_MouseLeave(object sender, EventArgs e)
        {
            if (隐藏ToolStripMenuItem.Checked)
            {
                toolStripLeft.Visible = false;
            }
        }

        private void toolStripRight_MouseLeave(object sender, EventArgs e)
        {
            //if (toolRightVisible)
            //{
            //    toolStripRight.Visible = false;
            //}
        }

        private void 隐藏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (隐藏ToolStripMenuItem.Checked)
            {
                隐藏ToolStripMenuItem.Checked = false;
                toolStripLeft.Visible = true;
            }
            else
            {
                隐藏ToolStripMenuItem.Checked = true;
                toolStripLeft.Visible = false;
            }
        }

   
        private void toolStripRight_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath);
        }

        private void 打开图像文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(Application.StartupPath + "\\SaveImage");
                System.Diagnostics.Process.Start(Application.StartupPath + "\\SaveImage");
            }
            catch (Exception)
            {
            }
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

    

        /// <summary>
        /// 添加到工具栏
        /// </summary>
        /// <param name="toolStripItem"></param>
        public void ToolStripAddMenuItem(ToolStripItem toolStripItem)
        {
            toolStripDropDownButton1.DropDownItems.Add(toolStripItem);
        }
        public ToolStripItem ToolStripAddMenuItem(string toolStripItem)
        {
            return toolStripDropDownButton1.DropDownItems.Add(toolStripItem);

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
                        this.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        this.WindowState = FormWindowState.Minimized;
                    }
                }
            }
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            if (alarmText1.Visible)
            {
                alarmText1.Visible = ProjectINI.In.IsAlramText = false;
            }
            else
            {
                alarmText1.Visible = ProjectINI.In.IsAlramText = true;
            }   
          
        }

        private void 截取屏幕ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(Application.StartupPath + "\\截取屏幕\\"+ DateTime.Now.ToLongDateString() );
            
                Bitmap bitmap = UI.UICon.GetScreenCapture();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Application.StartupPath + @"\截取屏幕\";

                saveFileDialog.Filter = "图像|*.jpg";

                string timeStr = DateTime.Now.ToString("HH时mm分ss秒");
                saveFileDialog.FileName = timeStr;
                if (saveFileDialog.ShowDialog()==DialogResult.OK)
                {
                    bitmap.Save(saveFileDialog.FileName);
                }
     
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
         
        }

        private void 固定窗口ToolStripMenuItem_Click(object sender, EventArgs e)
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
                固定窗口ToolStripMenuItem.Text = "固定窗口";
            }
            else
            {
                splitContainer1.IsSplitterFixed = true;
                foreach (Control item in splitContainer1.Panel1.Controls)
                {
                    if (item is Splitter)
                    {
                        Splitter split =  item as Splitter;
                        split.Enabled = true;
                    }
                }
                固定窗口ToolStripMenuItem.Text = "拆分窗口";
            }
        }

        private void splitContainer1_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            e.Cancel = true;
            //splitContainer1.Panel2=
        }

        private void 添加快捷方式到桌面ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 脚本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Code.CodeForm codeForm = new Code.CodeForm();
                codeForm.Show();
            }
            catch (Exception)
            {


            }


        }

        private void 调试模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProjectINI.DebugMode)
            {
                ProjectINI.DebugMode = false;
                调试模式ToolStripMenuItem.Checked = false;
                tsButton1.Visible = false;
            }
            else
            {
                ProjectINI.DebugMode =true;
                调试模式ToolStripMenuItem.Checked = true;
                tsButton2.Visible = tsButton1.Visible = true;
            }
        }

        private void tsButton4_Click(object sender, EventArgs e)
        {
            if (splitContainer3.Panel2Collapsed)
            {
                splitContainer3.Panel2Collapsed = false;
            }
            else
            {
                splitContainer3.Panel2Collapsed = true;
            }
     
        }

        private void 方案管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 项目管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 通信管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton4_Click(object sender, EventArgs e)
        {

        }

        private void SaveAll_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ProjectINI.In.SaveProjectAll();

            //foreach (var item in ProjectINI.In.GetListRun())
            //{
            //    item.Value.SaveThis(Application.StartupPath + "\\Project\\" + ProjectINI.In.ProjectName + "\\" + ProjectINI.In.RunName);
            //}
            //System.GC.Collect();
            Cursor = Cursors.Arrow;
        }
    }
}