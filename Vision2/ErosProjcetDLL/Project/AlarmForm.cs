using System;
using System.Windows.Forms;
using Vision2.Project;
using Vision2.Project.formula;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class AlarmForm : Form
    {
        public AlarmForm()
        {
            InitializeComponent();
            //this.TopLevel = false;
        }

        public static AlarmForm AlarmFormThis
        {
            get
            {
                if (alarmForm == null || alarmForm.IsDisposed)
                {
                    alarmForm = new AlarmForm();

                    // alarmForm.TopMost = true;
                    //Vision2.Project.MainForm1.MainFormF.Controls.Add(alarmForm);
                }
                return alarmForm;
            }
            set { alarmForm = value; }
        }

        private static AlarmForm alarmForm;

        private void AlarmForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                UserFormulaContrsl.This.tabControl1.TabPages.Add(UserFormulaContrsl.This.tabPage4);
                UserFormulaContrsl.This.tabPage4.Controls.Add(AlarmForm.AlarmFormThis);
                AlarmForm.AlarmFormThis.TopLevel = false;
                AlarmForm.AlarmFormThis.FormBorderStyle = FormBorderStyle.None;
                AlarmForm.AlarmFormThis.Dock = DockStyle.Fill;
                AlarmForm.AlarmFormThis.Show();
            }
            catch (System.Exception)
            { }
            e.Cancel = true;//拦截，不响应操作
            return;
        }

        private void AlarmForm_Load(object sender, System.EventArgs e)
        {
        }

        public static void UpDa(string showText)
        {
            try
            {
         
                MainForm1.MainFormF.Invoke(new Action(() => {
                    if (showText == "浮动窗口")
                    {
                        //AlarmForm.AlarmFormThis.TopLevel = true;
                        MainForm1.MainFormF.splitContainer3.Panel2Collapsed = true;
                        UserFormulaContrsl.This.tabControl1.TabPages.Remove(UserFormulaContrsl.This.tabPage4);
                        MainForm1.MainFormF.Controls.Add(AlarmForm.AlarmFormThis);
                        AlarmForm.AlarmFormThis.BringToFront();
                        AlarmForm.AlarmFormThis.Dock = DockStyle.None;
                        AlarmForm.AlarmFormThis.FormBorderStyle = FormBorderStyle.Fixed3D;
                        AlarmForm.AlarmFormThis.TopMost = true;
                        AlarmForm.AlarmFormThis.Show();
                    }
                    else if (showText == "控制栏左")
                    {
                        MainForm1.MainFormF.splitContainer3.Panel2Collapsed = true;
                        if (!UserFormulaContrsl.This.tabControl1.TabPages.Contains(UserFormulaContrsl.This.tabPage4))
                        {
                            UserFormulaContrsl.This.tabControl1.TabPages.Add(UserFormulaContrsl.This.tabPage4);
                        }
                        AlarmForm.AlarmFormThis.TopLevel = false;
                        UserFormulaContrsl.This.tabPage4.Controls.Add(AlarmForm.AlarmFormThis);

                        AlarmForm.AlarmFormThis.FormBorderStyle = FormBorderStyle.None;
                        AlarmForm.AlarmFormThis.Dock = DockStyle.Fill;
                        AlarmForm.AlarmFormThis.Show();
                    }
                    else
                    {
                        AlarmForm.AlarmFormThis.TopLevel = false;
                        UserFormulaContrsl.This.tabControl1.TabPages.Remove(UserFormulaContrsl.This.tabPage4);
                        MainForm1.MainFormF.splitContainer3.Panel2Collapsed = false;
                        MainForm1.MainFormF.splitContainer3.Panel2.Controls.Add(AlarmForm.AlarmFormThis);
                        AlarmForm.AlarmFormThis.BringToFront();
                        AlarmForm.AlarmFormThis.Dock = DockStyle.Fill;
                        AlarmForm.AlarmFormThis.FormBorderStyle = FormBorderStyle.None;
                        AlarmForm.AlarmFormThis.TopMost = false;
                        AlarmForm.AlarmFormThis.Show();
                    }


                }));

        
            }
            catch (Exception ex)
            {
            }
        }

        private void alarmText1_Load(object sender, System.EventArgs e)
        {
        }
    }
}