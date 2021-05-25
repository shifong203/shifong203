using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Vision2.Project.DebugF.IO;

namespace ErosSocket.DebugPLC.PLC
{
    public partial class Pf : Form
    {
        public Pf()
        {
            InitializeComponent();
        }

        Axis AxisT;
        public void SetAxis(IAxis axist)
        {
            try
            {
                AxisT = axist as Axis;
                this.Text = axist.Name;
                trackBar1.Maximum = AxisT.PlusLimit + 20;
                trackBar1.Minimum = AxisT.MinusLimit - 20;
                this.propertyGrid1.SelectedObject = axist;
                label4.Text = "软负限:" + AxisT.MinusLimit;
                label3.Text = "正负限:" + AxisT.PlusLimit;
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        while (!this.IsDisposed)
                        {
                            this.Invoke(new Action(() =>
                            {

                                if (AxisT != null)
                                {
                                    if (AxisT.Negative_Limit)
                                    {
                                        label8.BackColor = Color.Green;
                                    }
                                    else
                                    {
                                        label8.BackColor = Color.Gray;
                                    }
                                    if (AxisT.Origin_Limit)
                                    {
                                        label7.BackColor = Color.Green;
                                    }
                                    else
                                    {
                                        label7.BackColor = Color.Gray;
                                    }
                                    if (AxisT.IsEnabled)
                                    {
                                        label19.BackColor = Color.Green;
                                    }
                                    else
                                    {
                                        label19.BackColor = Color.Gray;
                                    }
                                    if (AxisT.Positive_Limit)
                                    {
                                        label9.BackColor = Color.Green;
                                    }
                                    else
                                    {
                                        label9.BackColor = Color.Gray;
                                    }
                                    if (AxisT.Positive_LimitSwt)
                                    {
                                        label3.BackColor = Color.Green;
                                    }
                                    else
                                    {
                                        label3.BackColor = Color.Gray;
                                    }
                                    if (AxisT.Negative_LimitSwt)
                                    {
                                        label4.BackColor = Color.Green;
                                    }
                                    else
                                    {
                                        label4.BackColor = Color.Gray;
                                    }

                                    switch (AxisT.StaratNn)
                                    {
                                        case 0:
                                            label1.Text = "轴状态:轴被禁用";
                                            break;
                                        case 1:
                                            label1.Text = "轴状态:已就绪";
                                            break;
                                        case 2:
                                            label1.Text = "轴状态:已停止";
                                            break;
                                        case 3:
                                            label1.Text = "轴状态:出错并停止";
                                            break;
                                        case 4:
                                            label1.Text = "轴状态:回零中";
                                            break;
                                        case 5:
                                            label1.Text = "轴状态:执行PTP运动";
                                            break;
                                        case 6:
                                            label1.Text = "轴状态:执行连续运动中";
                                            break;
                                        case 7:
                                            label1.Text = "轴状态:群组,插补运动中";
                                            break;
                                        case 8:
                                            label1.Text = "轴状态:轴由外部信号控制JOG模式";
                                            break;
                                        case 9:
                                            label1.Text = "轴状态:轴由外部信号控制MPG模式";
                                            break;

                                        default:
                                            label1.Text = "轴状态:";
                                            break;
                                    }
                                    label2.Text = "当前速度:" + AxisT.SleepValue;
                                    label5.Text = "当前位置:" + AxisT.Point;
                                    try
                                    {
                                        trackBar1.Value = (int)AxisT.Point;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }));
                            Thread.Sleep(10);
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                });
                thread.IsBackground = true;
                thread.Start();

            }
            catch (System.Exception)
            {
            }
        }
        bool WateBool;
        bool Stop ;
        private void button1_Click(object sender, EventArgs e)
        {
            AxisT.SetHome();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
      
                if (!WateBool)
                {
                    button2.Enabled = false;
                    button7.Enabled = true;
                    WateBool = true;
                    Stop = false;
                    Thread thread = new Thread(() =>
                    {
                        try
                        {

                            if (!AxisT.IsHome)
                            {
                                MessageBox.Show("未初始化");
                                return;
                            }
                            for (int i = 0; i < numericUpDown3.Value; i++)
                            {
                                if (Stop)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        label12.Text = "执行状态:停止";
                                    }));   return;
                                }
                                this.Invoke(new Action(() =>
                                {
                                    label12.Text = "执行状态:" + (i + 1) + "去" + numericUpDown1.Value;
                                }));
                                AxisT.SetWPoint((double)numericUpDown1.Value);
                                this.Invoke(new Action(() =>
                                {
                                    label12.Text = "执行状态:" + (i + 1) + "等待ms" + numericUpDown4.Value;
                                }));
                                Thread.Sleep((int)numericUpDown4.Value);
                                if (Stop)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        label12.Text = "执行状态:停止";
                                    })); return;
                                }
                                this.Invoke(new Action(() =>
                                {
                                    label12.Text = "执行状态:" + (i + 1) + "去" + numericUpDown2.Value;
                                }));
                                AxisT.SetWPoint((double)numericUpDown2.Value);
                            }
                            WateBool = false;
                            button2.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                AxisT.SetPoint(AxisT.Point - (double)numericUpDown5.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                AxisT.SetPoint(AxisT.Point + (double)numericUpDown5.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                AxisT.JogAdd(true, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            AxisT.Stop();
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                AxisT.JogAdd(false, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            AxisT.Stop();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            WateBool = false;
            Stop = true;
            AxisT.Stop();
            button2.Enabled = true;
            button7.Enabled = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AxisT.Enabled();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AxisT.Initial();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AxisT.Reset();
        }
    }
}
