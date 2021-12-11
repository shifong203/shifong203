using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Vision2.vision;

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
                            Thread.Sleep(1);
                            labelIDCode.Text = DebugCompiler.Instance.DDAxis.runID.ToString();
                            label1.Text = "CT:" + DebugCompiler.Instance.DDAxis.WatchT.Elapsed.TotalSeconds.ToString("0.00");

                            this.Invoke(new Action(() =>
                            {
                                if (DebugCompiler.Instance.DDAxis.AlwaysIODot.Value)
                                {
                                 this.pictureBoxINPace.Visible = true;
                                    pictureBoxINPace.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                 this.pictureBoxINPace.Visible = false;
                                    pictureBoxINPace.BackColor = Color.Gray;
                                }
                                if (DebugCompiler.Instance.DDAxis.AlwaysIOInt.Value)
                                {
                                  this.pictureBoxIntoPlate.Visible = true;
                                    label2.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    this.pictureBoxIntoPlate.Visible = false;
                                    label2.BackColor = Color.Gray;
                                }
                                if (DebugCompiler.Instance.DDAxis.AlwaysIOOut.Value)
                                {
                                    if (!this.pictureBoxOutPlate.Visible)
                                    {
                                        this.pictureBoxOutPlate.Visible = true;
                                    }
                                    if (!DebugCompiler.Instance.DDAxis.Int[DebugCompiler.Instance.To_Board_DI])
                                    {
                                        if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.运行中)
                                        {
                                            //if (DebugCompiler.GetThis().DDAxis.runID>6&& DebugCompiler.GetThis().DDAxis.runID>=1)
                                            //{
                                            DebugCompiler.Instance.DDAxis.MoveAxisStop();
                                        }
                                    }
                                    label4OutSen.BackColor = Color.GreenYellow;
                                }
                                else
                                {
                                    this.pictureBoxOutPlate.Visible = false;
                                    label4OutSen.BackColor = Color.Gray;
                                }
                                if (DODIAxis.RresWait)
                                {
                                    label5Await.Visible = true;
                                    label5Await.BackColor = Color.Green;
                                }
                                else
                                {
                                    label5Await.Visible = false;
                                }
                                if (DebugCompiler.Instance.OutDischarging>=0)
                                {
                                    double det = DebugCompiler.Instance.DDAxis.Out.EventDs[DebugCompiler.Instance.OutDischarging].TimeVlue;
                             
                                    label6PT.Text = "W" + det.ToString("0.00");
                                    if (DebugCompiler.Instance.DDAxis.Out[DebugCompiler.Instance.OutDischarging])
                                    {
                                        label6PT.BackColor = Color.Green;
                                        if (det>=10)
                                        {
                                            DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.OutDischarging, false);
                                        }
                                    }
                                    else
                                    {
                                        label6PT.BackColor = Color.Transparent;
                                    }
                          
                                }
                           
                             
                           
                                label4OutSen.Text = "出" + DebugCompiler.Instance.DDAxis.AlwaysIOOut.RunTime.ToString("00.0");
                                //label3.Text = "" + DebugCompiler.GetThis().DDAxis.AlwaysIODot.RunTime.ToString("00.0");
                                label2.Text = "进" + DebugCompiler.Instance.DDAxis.AlwaysIOInt.RunTime.ToString("00.0");
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
                                if (DebugCompiler.Instance.DDAxis.Out[DebugCompiler.Instance.RunButton.yellow])
                                {
                                    pictureYel.BackColor = Color.Yellow;
                                }
                                else
                                {
                                    pictureYel.BackColor = Color.Olive;
                                }
                                if (DebugCompiler.Instance.DDAxis.Out[DebugCompiler.Instance.RunButton.green])
                                {
                                    pictureBoxGreen.BackColor = Color.Lime;
                                }
                                else
                                {
                                    pictureBoxGreen.BackColor = Color.DarkGreen;
                                }

                                if (DebugCompiler.Instance.DDAxis.Out[DebugCompiler.Instance.RunButton.red])
                                {
                                    pictureBoxRed.BackColor = Color.Red;
                                }
                                else
                                {
                                    pictureBoxRed.BackColor = Color.Maroon;
                                }
                                if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.运行中)
                                {
                                    if (!DebugCompiler.Instance.DDAxis.RunCodeT.Runing)
                                    {
                                        ndtr++;
                                        if (ndtr >= 3000)
                                        {
                                            if (number / 10 % 2 > 0)
                                            {
                                                DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.RunButton.yellow, true);
                                            }
                                            else
                                            {
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

                                if (DebugCompiler.Instance.DDAxis.GetCylinderName(DebugCompiler.Instance.LoctionCylinder) != null)
                                {
                                    if (DebugCompiler.Instance.DDAxis.GetCylinderName(DebugCompiler.Instance.LoctionCylinder).PrValue)
                                    {
                                        pictureBox6.BackColor = Color.GreenYellow;

                                        pictureBox6.Location = new Point(pictureBox6.Location.X, 80);
                                    }
                                    else
                                    {
                                        pictureBox6.BackColor = Color.Gray;
                                        pictureBox6.Location = new Point(pictureBox6.Location.X, this.Height - pictureBox6.Height);
                                    }
                                }
                                if (DebugCompiler.Instance.DDAxis.GetCylinderName(DebugCompiler.Instance.RCylinder) != null)
                                {
                                    if (DebugCompiler.Instance.DDAxis.GetCylinderName(DebugCompiler.Instance.RCylinder).PrValue)
                                    {
                                        pictureBox4.BackColor = Color.GreenYellow;

                                        pictureBox4.Location = new Point(pictureBox4.Location.X, 80);
                                    }
                                    else
                                    {
                                        pictureBox4.BackColor = Color.Gray;
                                        pictureBox4.Location = new Point(pictureBox4.Location.X, this.Height - pictureBox4.Height);
                                    }
                                }
                                else
                                {
                                    pictureBox4.Visible = false;
                                }
                            }));
            
                        }
                        catch (Exception ex)
                        {
                        }
                        Thread.Sleep(10);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
                thread = new Thread(() =>
                {
                    Thread.Sleep(100);
                    int dnumber = 0;
                    while (!this.IsDisposed)
                    {
                        try
                        {
                            if (DebugCompiler.Instance.OutDischarging >= 0)
                            {
                                double det = DebugCompiler.Instance.DDAxis.Out.EventDs[DebugCompiler.Instance.OutDischarging].TimeVlue;
                                if (DebugCompiler.Instance.DDAxis.AlwaysIOOut.TimeValue&& RestObjImage.TrayImage == null)
                                {
                                    if (DebugCompiler.Instance.DDAxis.AlwaysIOOut.Value && DebugCompiler.Instance.DDAxis.AlwaysIOOut.RunTime >= 15)
                                        {
                                            if (DebugCompiler.Instance.DDAxis.Out.EventDs[DebugCompiler.Instance.OutDischarging].TimeOutBool || !DebugCompiler.Instance.DDAxis.Out[DebugCompiler.Instance.OutDischarging] && det > 10)
                                            {
                                                DebugCompiler.GetDoDi().WritDO(DebugCompiler.Instance.OutDischarging, true);
                                            }
                                        }
                                }
                            }
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

        private void label5Await_Click(object sender, EventArgs e)
        {
            label5Await.Enabled = true;
        }

        private void label6PT_Click(object sender, EventArgs e)
        {
            label6PT.Enabled = true;
        }

        private void pictureBox4_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (!DebugCompiler.Instance.DDAxis.GetCyp(DebugCompiler.Instance.RCylinder))
                {
                    DebugCompiler.Instance.DDAxis.Cyp(DebugCompiler.Instance.RCylinder, true);
                }
                else
                {
                    DebugCompiler.Instance.DDAxis.Cyp(DebugCompiler.Instance.RCylinder, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox6_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (!DebugCompiler.Instance.DDAxis.GetCyp(DebugCompiler.Instance.LoctionCylinder))
                {
                    DebugCompiler.Instance.DDAxis.Cyp(DebugCompiler.Instance.LoctionCylinder, true);
                }
                else
                {
                    DebugCompiler.Instance.DDAxis.Cyp(DebugCompiler.Instance.LoctionCylinder, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox8_DoubleClick(object sender, EventArgs e)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}