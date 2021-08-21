using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Vision2.Project.DebugF.IO
{
    public partial class RunUControl : UserControl
    {
        public RunUControl()
        {
            InitializeComponent();
        }

        private void RunUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                Thread thread = new Thread(() =>
                {
                    int number = 0;
                    int ndtr = 0;
                    while (!this.IsDisposed)
                    {
                        try
                        {
                            number++;
                            Thread.Sleep(2);
                            label3.Text = DebugCompiler.Instance.DDAxis.runID.ToString();
                            label1.Text = "CT:" + DebugCompiler.Instance.DDAxis.WatchT.Elapsed.TotalSeconds;
                            if (DebugCompiler.Instance.DDAxis.AlwaysIODot.Value)
                            {
                                pictureBox2.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                pictureBox2.BackColor = Color.Gray;
                            }
                            if (DebugCompiler.Instance.DDAxis.AlwaysIOInt.Value)
                            {
                                label2.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                label2.BackColor = Color.Gray;
                            }
                            if (DebugCompiler.Instance.DDAxis.AlwaysIOOut.Value)
                            {
                                if (!DebugCompiler.Instance.DDAxis.Int[DebugCompiler.Instance.To_Board_DI])
                                {
                                    if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.运行中)
                                    {
                                        //if (DebugCompiler.GetThis().DDAxis.runID>6&& DebugCompiler.GetThis().DDAxis.runID>=1)
                                        //{
                                        DebugCompiler.Instance.DDAxis.MoveAxisStop();
                                    }
                                }
                                label4.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                label4.BackColor = Color.Gray;
                            }
                            this.Invoke(new Action(() =>
                            {
                                if (DODIAxis.RresWait)
                                {
                                    label5.Visible = true;
                                    label5.BackColor = Color.Green;
                                }
                                else
                                {
                                    label5.Visible = false;
                                }
                                label4.Text = "出" + DebugCompiler.Instance.DDAxis.AlwaysIOOut.RunTime.ToString("00.0");
                                //label3.Text = "" + DebugCompiler.GetThis().DDAxis.AlwaysIODot.RunTime.ToString("00.0");
                                label2.Text = "进" + DebugCompiler.Instance.DDAxis.AlwaysIOInt.RunTime.ToString("00.0");
                            }));
                            if (DebugCompiler.Instance.DDAxis.Int[DebugCompiler.Instance.To_Board_DI])
                            {
                                pictureBox5.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                pictureBox5.BackColor = Color.Gray;
                            }
                            if (DebugCompiler.Instance.DDAxis.Out[DebugCompiler.Instance.To_Board_DO])
                            {
                                pictureBox8.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                pictureBox8.BackColor = Color.Gray;
                            }
                            if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.运行中)
                            {
                                if (!DebugCompiler.Instance.DDAxis.RunCodeT.Runing)
                                {
                                    ndtr++;
                                    if (ndtr >= 600)
                                    {
                                        if (number / 10 % 2 > 0)
                                        {
                                            pictureYel.BackColor = Color.Yellow;
                                            DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.RunButton.yellow, true);
                                        }
                                        else
                                        {
                                            pictureYel.BackColor = Color.Olive;
                                            DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.RunButton.yellow, false);
                                        }
                                    }
                                }
                                else
                                {
                                    DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.RunButton.yellow, false);
                                    ndtr = 0;
                                }
                            }
                            if (DebugCompiler.Instance.DDAxis.GetCylinderName(DebugCompiler.Instance.RCylinder) != null)
                            {
                                if (DebugCompiler.Instance.DDAxis.GetCylinderName(DebugCompiler.Instance.RCylinder).AnValue)
                                {
                                    pictureBox4.BackColor = Color.GreenYellow;

                                    pictureBox4.Location = new Point(pictureBox4.Location.X, 80);
                                }
                                else
                                {
                                    pictureBox4.Location = new Point(pictureBox4.Location.X, 80);
                                }
                            }
                            else
                            {
                                pictureBox4.Visible = false;
                            }
                            if (DebugCompiler.Instance.DDAxis.GetCylinderName(DebugCompiler.Instance.LoctionCylinder) != null)
                            {
                                if (DebugCompiler.Instance.DDAxis.GetCylinderName(DebugCompiler.Instance.LoctionCylinder).AnValue)
                                {
                                    pictureBox6.BackColor = Color.GreenYellow;

                                    pictureBox6.Location = new Point(pictureBox6.Location.X, 80);
                                }
                                else
                                {
                                    pictureBox6.BackColor = Color.Gray;
                                    pictureBox6.Location = new Point(pictureBox6.Location.X, 80);
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        Thread.Sleep(50);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
                thread = new Thread(() =>
                {
                    int dnumber = 0;
                    while (!this.IsDisposed)
                    {
                        try
                        {
                            if (DebugCompiler.Instance.DDAxis.IsMove)
                            {
                                dnumber++;
                                for (int i = 0; i < 7; i++)
                                {
                                    pictureBox1.Location = new Point(-50 + (i * 20), pictureBox1.Location.Y);
                                    pictureBox7.Location = new Point(50 + (20 * i), pictureBox1.Location.Y);
                                    pictureBox10.Location = new Point(150 + (20 * i), pictureBox1.Location.Y);
                                    pictureBox12.Location = new Point(250 + (20 * i), pictureBox1.Location.Y);
                                    pictureBox3.Location = new Point(-50 + (i * 20), pictureBox3.Location.Y);
                                    pictureBox9.Location = new Point(50 + (20 * i), pictureBox3.Location.Y);
                                    pictureBox11.Location = new Point(150 + (20 * i), pictureBox3.Location.Y);
                                    pictureBox13.Location = new Point(250 + (20 * i), pictureBox3.Location.Y);
                                    Thread.Sleep(200);
                                }
                                pictureBox1.Location = new Point(-40 + (0), pictureBox1.Location.Y);
                                pictureBox7.Location = new Point(80 + (0), pictureBox1.Location.Y);
                                pictureBox10.Location = new Point(180 + (0), pictureBox1.Location.Y);
                                pictureBox12.Location = new Point(280 + (0), pictureBox1.Location.Y);
                                pictureBox3.Location = new Point(-40 + (0), pictureBox3.Location.Y);
                                pictureBox9.Location = new Point(80 + (0), pictureBox3.Location.Y);
                                pictureBox11.Location = new Point(180 + (0), pictureBox3.Location.Y);
                                pictureBox13.Location = new Point(280 + (0), pictureBox3.Location.Y);

                                if (dnumber >= 5)
                                {
                                    if (DebugCompiler.Instance.DDAxis.AlwaysIOOut.Value && DebugCompiler.Instance.DDAxis.AlwaysIODot.RunTime >= 15)
                                    {
                                        ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText("出板线体卡板");
                                    }
                                    //if (DebugCompiler.GetThis().DDAxis.AlwaysIOInt.Value && DebugCompiler.GetThis().DDAxis.AlwaysIOInt.WatchT.Elapsed.TotalSeconds >= 10)
                                    //{
                                    //    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText("入板线体卡板");
                                    //}
                                }
                            }
                            else
                            {
                                dnumber = 0;
                            }
                        }
                        catch (Exception)
                        {
                        }
                        Thread.Sleep(400);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (DebugCompiler.Instance.DDAxis.Out[DebugCompiler.Instance.To_Board_DO])
                {
                    DebugCompiler.Instance.DDAxis.WritDO(DebugCompiler.Instance.To_Board_DO, false);
                }
                else
                {
                    DebugCompiler.Instance.DDAxis.WritDO(DebugCompiler.Instance.To_Board_DO, true);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}