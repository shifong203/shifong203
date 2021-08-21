using ErosSocket.ErosConLink;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC
{
    public partial class InterfacePlcUserControl2 : UserControl
    {
        public InterfacePlcUserControl2()
        {
            InitializeComponent();
        }

        public void UpDat(IPLCInterface pLCInterface, string nameStr)
        {
            DebugCompiler = pLCInterface;
            name = nameStr;
        }

        private string name;

        public InterfacePlcUserControl2(IPLCInterface pLCInterface, string nameStr) : this()
        {
            DebugCompiler = pLCInterface;
            name = nameStr;
        }

        private string aram = "";

        private void UpDa()
        {
            try
            {
                try
                {
                    if (DebugCompiler.ListNameRunID != null)
                    {
                        for (int i = 0; i < DebugCompiler.ListNameRunID.ListName.Count; i++)
                        {
                            /*   Vision2.ErosProjcetDLL.MainForm1.MainFormF*/
                            this.FindForm().Invoke(new Action(() =>
                            {
                                EnumTextUserControl1 enumTextUserControl1 = new EnumTextUserControl1();
                                string dsa = StaticCon.GetLingkNameValueString(DebugCompiler.Name + "." + DebugCompiler.ListNameRunID.ListName[i]);
                                if (int.TryParse(dsa, out int inumbe))
                                {
                                    enumTextUserControl1.UpEnumText(inumbe);
                                }
                                this.panel1.Controls.Add(enumTextUserControl1);
                                enumTextUserControl1.Dock = DockStyle.Top;
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                while (!this.IsDisposed)
                {
                    if (DebugCompiler == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    //this.Invoke(new Action(() => {
                    try
                    {
                        if (DebugCompiler.LinkStop == "")
                        {
                            button1.Enabled = false;
                        }
                        if (!this.Visible)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        if (DebugCompiler.SetStatModeName == null || DebugCompiler.SetStatModeName == "")
                        {
                            button3.Enabled = false;
                        }

                        Control[] controls = this.Controls.Find("EnumTextUserControl1", true);
                        for (int i = 0; i < controls.Length; i++)
                        {
                            EnumTextUserControl1 enumTextUserControl1 = controls[i] as EnumTextUserControl1;
                            if (enumTextUserControl1 != null)
                            {
                                string time = StaticCon.GetLingkNameValueString(DebugCompiler.Name + "." + DebugCompiler.ListNameRunID.ListName[i]);
                                if (int.TryParse(time, out int inumbe))
                                {
                                    enumTextUserControl1.UpEnumText(inumbe, DebugCompiler.ListNameRunID.ListEnumName[i]);
                                }
                            }
                        }

                        string dsa = StaticCon.GetLingkNameValueString(DebugCompiler.Name + "." + DebugCompiler.LinkPauseName);
                        if (dsa == true.ToString())
                        {
                            DebugCompiler.EquipmentStatus = EnumEquipmentStatus.暂停中;
                            btnStrat.Text = "继续";
                            btnStrat.Enabled = true;
                        }
                        else
                        {
                            dsa = StaticCon.GetLingkNameValueString(DebugCompiler.Name + "." + DebugCompiler.LinkRunName);
                            if (dsa == true.ToString())
                            {
                                if (true.ToString() == StaticCon.GetLingkNameValueString(DebugCompiler.Name + "." + DebugCompiler.LinkREName))
                                {
                                    DebugCompiler.EquipmentStatus = EnumEquipmentStatus.初始化中;
                                }
                                else
                                {
                                    DebugCompiler.EquipmentStatus = EnumEquipmentStatus.运行中;
                                }
                            }
                            else
                            {
                                if (DebugCompiler.EquipmentStatus != EnumEquipmentStatus.初始化中)
                                {
                                    btnStrat.Text = "启动";
                                    DebugCompiler.EquipmentStatus = EnumEquipmentStatus.已停止;
                                    DebugCompiler.Stoping = false;
                                }
                                else if (DebugCompiler.EquipmentStatus == EnumEquipmentStatus.初始化中)
                                {
                                    btnStrat.Text = "启动";
                                    DebugCompiler.Stoping = false;
                                    btnStrat.Enabled = false;
                                }
                            }
                        }
                        if (DebugCompiler.EquipmentStatus == EnumEquipmentStatus.运行中)
                        {
                            btnStrat.Text = "启动";
                            btnStrat.Enabled = false;
                        }
                        else if (DebugCompiler.EquipmentStatus == EnumEquipmentStatus.已停止)
                        {
                            btnStrat.Enabled = true;
                        }
                        if (DebugCompiler.LinkPause == "")
                        {
                            Btn_Pause.Enabled = false;
                        }
                        else
                        {
                            Btn_Pause.Enabled = true;
                        }

                        string data = "设备名：" + name + ";设备状态:" + DebugCompiler.EquipmentStatus.ToString();
                        dsa = StaticCon.GetLingkNameValueString(DebugCompiler.Name + "." + DebugCompiler.GetStatLinkName);
                        if (dsa != "")
                        {
                            data += "；设备状态ID:" + dsa;
                            if (int.TryParse(dsa, out int dt) && DebugCompiler.KeyIDStr.ContainsKey(dt))
                            {
                                data += "," + DebugCompiler.KeyIDStr[dt];
                            }
                            else
                            {
                                data += ":未定义";
                            }
                        }
                        dsa = StaticCon.GetLingkNameValueString(DebugCompiler.Name + "." + DebugCompiler.LinkAlarmName);
                        if (aram != dsa)
                        {
                            aram = dsa;
                            if (dsa == true.ToString())
                            {
                                data += "; 故障中";
                                DebugCompiler.IsAlarm = true;
                                button2.BackColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                DebugCompiler.IsAlarm = false;
                                button2.BackColor = System.Drawing.Color.Green;
                            }
                        }
                        dsa = StaticCon.GetLingkNameValueString(DebugCompiler.Name + "." + DebugCompiler.GetStatModeName);
                        if (dsa == true.ToString())
                        {
                            button3.Text = "运行模式";
                            button3.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            button3.Text = "调试/手动";
                            button3.ForeColor = System.Drawing.Color.Red;
                        }
                        if (groupBox1.Text != data)
                        {
                            groupBox1.Text = data;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    Thread.Sleep(1000);
                    //}));
                }
            }
            catch (Exception)
            {
            }
        }

        private IPLCInterface DebugCompiler;

        private void button1_Click(object sender, EventArgs e)
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.Stop();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.Rest();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //DebugCompiler.();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InterfacePlcUserControl2_Load(object sender, EventArgs e)
        {
            try
            {
                Task.Run(() =>
                {
                    UpDa();
                });
            }
            catch (Exception)
            {
            }
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                dtime.Stop();
                dtime.Reset();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 跟新周期
        /// </summary>
        public System.Diagnostics.Stopwatch dtime = new System.Diagnostics.Stopwatch();

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                dtime.Restart();
                Task.Run(() =>
                {
                    while (dtime.ElapsedMilliseconds != 0)
                    {
                        if (dtime.ElapsedMilliseconds >= 2000)
                        {
                            if (DebugCompiler.LinkInitialize != null && DebugCompiler.LinkInitialize != "")
                            {
                                DebugCompiler.Initialize();
                            }
                            return;
                        }
                    }
                });
            }
            catch (Exception)
            {
            }
        }
    }
}