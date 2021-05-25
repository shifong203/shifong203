using System;
using System.Drawing;
using System.Windows.Forms;


namespace Vision2.Project.DebugF
{
    public partial class UserInterfaceControl : UserControl
    {
        public UserInterfaceControl()
        {
            InitializeComponent();

        }
        public static UserInterfaceControl This;
        Timer timer;
        bool timeing;
        //UserInterfaceData data;
        private void Btn_Stop_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DebugCompiler.Stop(true);
                D++;
                if (!timeing)
                {
                    timeing = true;
                    timer.Start();
                }
                if (D >= 3)
                {
                    D = 0;
                    timeing = false;
                    if (DebugCompiler.GetThis().IsRunStrat)
                    {
                        DebugCompiler.ISHome = false;
                    }
                    DebugF.DebugCompiler.RunStop = true;
                    DebugCompiler.Stop(true);
                }

                if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                {
                    Btn_Debug.Enabled =
                      Btn_Initialize.Enabled =
                      Btn_Start.Enabled = true;
                    Btn_Pause.Text = "暂停";
                    Btn_Start.Text = "启动";
                }
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            D = 0;
            timeing = false;
        }
        int D = 0;

        private void Btn_Massge_Click(object sender, System.EventArgs e)
        {
            labelStat.Text = DebugCompiler.EquipmentStatus.ToString();
        }
        DebugF.DebugForm debugForm;
        private void Btn_Debug_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (Vision2.ErosProjcetDLL.Project.ProjectINI.Enbt || Vision2.ErosProjcetDLL.Project.User.MatchThePermissions("管理权限"))
                {
                    if (debugForm == null)
                    {
                        debugForm = new DebugF.DebugForm();
                    }
                    Vision2.ErosProjcetDLL.UI.UICon.WindosFormerShow(ref debugForm);
                    DebugCompiler.Debuging = true;
                    if (DebugCompiler.GetThis().LinkAutoMode != null && DebugCompiler.GetThis().LinkAutoMode != "")
                    {
                        if (ErosSocket.ErosConLink.StaticCon.GetLingkIDValue(DebugCompiler.GetThis().LinkAutoMode, ErosSocket.ErosConLink.UClass.Boolean, out dynamic valueDy))
                        {
                            if (valueDy)
                            {
                                DialogResult dialogResult = MessageBox.Show("是否切换为手动模式", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(DebugCompiler.GetThis().LinkAutoMode, false);

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
            labelStat.Text = DebugCompiler.EquipmentStatus.ToString();
        }

        private void Btn_Reset_Click(object sender, System.EventArgs e)
        {
            try
            {
                Vision2.ErosProjcetDLL.Project.AlarmText.ResetAlarm();
                DebugCompiler.Rest();
                DebugF.DebugCompiler.Buzzer = false;
                DebugF.DebugCompiler.GetDoDi().WritDO(DebugF.DebugCompiler.GetThis().RunButton.red, false);
                DebugF.DebugCompiler.GetDoDi().WritDO(DebugF.DebugCompiler.GetThis().RunButton.Fmq, false);
                DebugF.DebugCompiler.GetDoDi().WritDO(DebugF.DebugCompiler.GetThis().RunButton.ResetButtenS, false);
                //this.Btn_Reset.KeyValuePairs = this.data.LinkRestoration;
                //labelStat.Text = DebugCompiler.EquipmentStatus.ToString();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        string err = "";
        private void Btn_Pause_Click(object sender, System.EventArgs e)
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

        private void Btn_Start_Click(object sender, System.EventArgs e)
        {
            try
            {
                DebugCompiler.Start();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void Btn_Stop_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (DebugCompiler.GetThis().IsRunStrat)
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

        private void Btn_Initialize_Click(object sender, System.EventArgs e)
        {
            try
            {
                DebugCompiler.Initialize();
                labelStat.Text = DebugCompiler.EquipmentStatus.ToString();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabControl1_ControlAdded(object sender, ControlEventArgs e)
        {
            try
            {
                //foreach (TabPage item in tabControl1.TabPages)
                //{
                //    item.AutoScroll = true;
                //    //    item = new System.Drawing.Font("Microsoft YaHei UI", 12F);
                //}
            }
            catch (Exception)
            {
            }

        }

        private void Btn_Reset_MouseDown(object sender, MouseEventArgs e)
        {
            Btn_Reset.BackColor = Color.Peru;
        }

        private void Btn_Reset_MouseUp(object sender, MouseEventArgs e)
        {
            Btn_Reset.BackColor = Color.DimGray;
        }

        private void Btn_Initialize_MouseUp(object sender, MouseEventArgs e)
        {
            Btn_Initialize.BackColor = Color.DimGray;
        }

        private void Btn_Initialize_MouseDown(object sender, MouseEventArgs e)
        {
            Btn_Initialize.BackColor = Color.Peru;
        }

        private void Btn_Start_MouseDown(object sender, MouseEventArgs e)
        {
            Btn_Start.BackColor = Color.Peru;
        }

        private void Btn_Start_MouseUp(object sender, MouseEventArgs e)
        {
            Btn_Start.BackColor = Color.DimGray;
        }

        private void Btn_Pause_MouseDown(object sender, MouseEventArgs e)
        {
            Btn_Pause.BackColor = Color.Peru;
        }

        private void Btn_Pause_MouseUp(object sender, MouseEventArgs e)
        {
            Btn_Pause.BackColor = Color.DimGray;
        }

        private void Btn_Stop_MouseUp(object sender, MouseEventArgs e)
        {
            Btn_Stop.BackColor = Color.DimGray;
        }

        private void Btn_Stop_MouseDown(object sender, MouseEventArgs e)
        {
            Btn_Stop.BackColor = Color.Peru;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UserInterfaceControl_Load(object sender, EventArgs e)
        {
            Name = "UserInterface";
            Btn_Stop.MouseClick += Btn_Stop_MouseDoubleClick;
            //DebugCompiler.EquipmentStatus = ErosSocket.ErosConLink.EnumEquipmentStatus.已停止;
            labelStat.Text = DebugCompiler.EquipmentStatus.ToString();
            timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 3000;
            This = this;
        }
    }
}