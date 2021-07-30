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
        public Pf(IAxis axis):this()
        {
            SetAxis(axis);
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
                            Thread.Sleep(10);
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
            if (!AxisT.IsEnabled)
            {
                AxisT.Enabled();
            }
            else
            {
                AxisT.Diset();
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            AxisT.Initial();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AxisT.Reset();
        }

        private void Pf_Load(object sender, EventArgs e)
        {
            Label[] label = new Label[21];
            Label[] labelEx = new Label[21];

            try
            {
                comboBox1.SelectedIndex = Vision2.Project.DebugF.DebugCompiler.GetThis(). LinkSeelpTyoe;
                for (int i =20; i >= 0; i--)
                {
                    label[i] = new Label();
                    label[i].Text = i.ToString();
                    label[i].Name = i.ToString();
                    label[i].Dock = DockStyle.Top;
                    groupBox1.Controls.Add(label[i]);
                }
                label[0].Text = "1RDY";
                label[1].Text = "2ALM";
                label[2].Text = "3LMT+";
                label[3].Text = "4LMT-";
                label[4].Text = "5ORG";
                label[5].Text = "6DIR";
                label[6].Text = "7EMG";
                label[7].Text = "8PCS";
                label[8].Text = "9ERC";
                label[9].Text = "10EZ";
                label[10].Text = "11CLR";
                label[11].Text = "12LTC";
                label[12].Text = "13SD";
                label[13].Text = "14INP";
                label[14].Text = "15SVON";
                label[15].Text = "16RALM";
                label[16].Text = "17SLMT+";
                label[17].Text = "18SLMT-";
                label[18].Text = "19CMP";
                label[19].Text = "20CAM-D0";
                label[20].Text = "21TORLMT";

                groupBox2.Visible = false;
                if (AxisT.AxisNoEx>=0)
                {
                    groupBox2.Visible = true;
                    groupBox2.Text += "轴号:" + AxisT.AxisNoEx;
                    for (int i = 20; i >= 0; i--)
                    {
                        labelEx[i] = new Label();
                        labelEx[i].Text = i.ToString();
                        labelEx[i].Name = i.ToString();
                        labelEx[i].Dock = DockStyle.Top;
                        groupBox2.Controls.Add(labelEx[i]);
                    }
                    labelEx[0].Text = "1RDY";
                    labelEx[1].Text = "2ALM";
                    labelEx[2].Text = "3LMT+";
                    labelEx[3].Text = "4LMT-";
                    labelEx[4].Text = "5ORG";
                    labelEx[5].Text = "6DIR";
                    labelEx[6].Text = "7EMG";
                    labelEx[7].Text = "8PCS";
                    labelEx[8].Text = "9ERC";
                    labelEx[9].Text = "10EZ";
                    labelEx[10].Text = "11CLR";
                    labelEx[11].Text = "12LTC";
                    labelEx[12].Text = "13SD";
                    labelEx[13].Text = "14INP";
                    labelEx[14].Text = "15SVON";
                    labelEx[15].Text = "16RALM";
                    labelEx[16].Text = "17SLMT+";
                    labelEx[17].Text = "18SLMT-";
                    labelEx[18].Text = "19CMP";
                    labelEx[19].Text = "20CAM-D0";
                    labelEx[20].Text = "21TORLMT";
                }
         
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            groupBox1.Text += "轴号:"+AxisT.AxisNo;


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
                                for (int i = 0; i < label.Length; i++)
                                {
                                    if (!AxisT.IOBools[i])
                                    {
                                        label[i].BackColor = Color.Transparent;
                                    }
                                    else
                                    {
                                        label[i].BackColor = Color.Green;
                                    }
                                }
                                if (groupBox2.Visible)
                                {
                                    for (int i = 0; i < labelEx.Length; i++)
                                    {
                                        if (!AxisT.IOBoolsEx[i])
                                        {
                                            labelEx[i].BackColor = Color.Transparent;
                                        }
                                        else
                                        {
                                            labelEx[i].BackColor = Color.Green;
                                        }
                                    }
                                }
                           
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
                                catch (Exception ex)
                                {
                                    //toolStripStatusLabel1.Text = "信息:" + ex.Message;
                                }
                            }
                            toolStripStatusLabel2.Text = "周期:" ;
                        }));
                        Thread.Sleep(10);
                    }
                }
                catch (System.Exception ex)
                {
                    toolStripStatusLabel1.Text = "信息:" + ex.Message;
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                AxisT.Stop();
            }
            catch (Exception)
            {
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                AxisT.SetPoint((double)numericUpDown10.Value);
            }
            catch (Exception)
            {


            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AxisT.AddSeelp(comboBox1.SelectedIndex);
            }
            catch (Exception)
            {

            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
