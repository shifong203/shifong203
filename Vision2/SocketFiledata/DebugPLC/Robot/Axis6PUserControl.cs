using System;
using System.Drawing;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC.Robot
{
    public partial class Axis6PUserControl : UserControl
    {
        public Axis6PUserControl()
        {
            InitializeComponent();
        }

        private EpsenRobot6 EpsenRobo;

        public void setAxisGrud(IAxisGrub axisG)
        {
            EpsenRobo = axisG as EpsenRobot6;

            comboBox2.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;

            if (EpsenRobo.High)
            {
                comboBox4.SelectedIndex = 1;
            }
            else
            {
                comboBox4.SelectedIndex = 0;
            }
            JogZnubme.Value = (decimal)axisG.JoupZ;
            Unumb.Value = (decimal)EpsenRobo.jogU;
            Vnumeric.Value = (decimal)EpsenRobo.jogV;
            WnumericUp.Value = (decimal)EpsenRobo.jogW;
            Znumber.Value = (decimal)EpsenRobo.jogZ;
            Xnum.Value = (decimal)EpsenRobo.jogX;
            Ynumb.Value = (decimal)EpsenRobo.jogY;
            comboBox3.SelectedItem = EpsenRobo.Tool.ToString();
            System.Threading.Thread thread2 = new System.Threading.Thread(() =>
            {
                while (!this.IsDisposed)
                {
                    try
                    {
                        if (IsMove)
                        {
                            if (!EpsenRobo.JogMode)
                            {
                                RobotMotionControl(AxJog, JogMinPin, EpsenRobo.Seelp, JogSetp);
                                System.Threading.Thread.Sleep(1500);
                            }
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(200);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            });
            thread2.IsBackground = true;
            thread2.Start();
            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                while (!this.IsDisposed)
                {
                    try
                    {
                        UPDataAx();
                        System.Threading.Thread.Sleep(100);
                    }
                    catch (Exception)
                    {
                    }
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private bool IsMove = false;
        private Single JogSetp = 20;

        /// <summary>
        /// 正或反true
        /// </summary>
        private bool JogMinPin = false;

        private string AxJog = "";

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="Axis"></param>
        /// <param name="PMIN">false正，true负</param>
        /// <param name="strCntSpeed">速度</param>
        /// <param name="strCntStep">距离</param>
        private void RobotMotionControl(string Axis, bool PMIN, double strCntSpeed, Single strCntStep)
        {
            if (PMIN)
            {
                EpsenRobo.SendCommand("StepMove," + Axis + ",Plus," + strCntSpeed + "," + strCntStep);
            }
            else
            {
                EpsenRobo.SendCommand("StepMove," + Axis + ",Minus," + strCntSpeed + "," + strCntStep);
            }
        }

        private bool heit;

        public void UPDataAx()
        {
            if (EpsenRobo.High != heit)
            {
                if (EpsenRobo.High)
                {
                    comboBox4.SelectedItem = 1;
                }
                else
                {
                    comboBox4.SelectedItem = 0;
                }
                heit = EpsenRobo.High;
            }
            EpsenRobo.GetPoints(out double x, out double y, out double z, out double u, out double v, out double w);
            UPoint.Text = "U:" + u.ToString("0.000");
            XPoint.Text = "X:" + x.ToString("0.000");
            YPoint.Text = "Y:" + y.ToString("0.000");
            Zpoint.Text = "Z:" + z.ToString("0.000");
            Vpoint.Text = "V:" + v.ToString("0.000");
            Wpoint.Text = "W:" + w.ToString("0.000");
            label7.Text = "Tool:" + EpsenRobo.Tool.ToString();
            if (EpsenRobo.Aralming)
            {
                label1.ForeColor = Color.Red;
            }
            else
            {
                label1.ForeColor = Color.GreenYellow;
            }
            if (EpsenRobo.enabled)
            {
                label6.ForeColor = Color.Red;
            }
            else
            {
                label6.ForeColor = Color.GreenYellow;
            }
            if (EpsenRobo.Pauseing)
            {
                label4.ForeColor = Color.Red;
            }
            else
            {
                label4.ForeColor = Color.GreenYellow;
            }
            if (EpsenRobo.Motor)
            {
                button2.Text = "MOTOR ON";
                button2.ForeColor = Color.Black;
                button2.BackColor = Color.Yellow;
            }
            else
            {
                button2.Text = "MOTOR OFF";
                button2.BackColor = Color.Cornsilk;
                button2.ForeColor = Color.Red;
            }
        }

        private void butYAd_Click(object sender, EventArgs e)
        {
        }

        private void Axis6PUserControl_Load(object sender, EventArgs e)
        {
            iscont = true;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            try
            {
                EpsenRobo.SetHome();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                EpsenRobo.Enabled();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butReset_Click(object sender, EventArgs e)
        {
            try
            {
                EpsenRobo.Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butStop_Click(object sender, EventArgs e)
        {
            try
            {
                EpsenRobo.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                EpsenRobo.JogMode = true;
                if (comboBox2.SelectedItem == null)
                {
                    return;
                }
                switch (comboBox2.SelectedItem.ToString())
                {
                    case "寸动自定义":
                        Xnum.Value = (decimal)EpsenRobo.jogX1;
                        Ynumb.Value = (decimal)EpsenRobo.jogY1;
                        Znumber.Value = (decimal)EpsenRobo.jogZ1;
                        Vnumeric.Value = (decimal)EpsenRobo.jogV1;
                        WnumericUp.Value = (decimal)EpsenRobo.jogW1;
                        Unumb.Value = (decimal)EpsenRobo.jogU1;
                        EpsenRobo.jogU = EpsenRobo.jogU1;
                        EpsenRobo.jogV = EpsenRobo.jogV1;
                        EpsenRobo.jogX = EpsenRobo.jogX1;
                        EpsenRobo.jogY = EpsenRobo.jogY1;
                        EpsenRobo.jogW = EpsenRobo.jogW1;
                        EpsenRobo.jogZ = EpsenRobo.jogZ1;

                        break;

                    case "寸动短距0.1":
                        Xnum.Value = Ynumb.Value = Znumber.Value
                       = Vnumeric.Value = WnumericUp.Value =
                       Unumb.Value = 0.1M;
                        EpsenRobo.jogU = EpsenRobo.jogV = EpsenRobo.jogX =
                            EpsenRobo.jogY = EpsenRobo.jogW = EpsenRobo.jogZ = 0.1f;

                        break;

                    case "寸动中距1":

                        Xnum.Value = Ynumb.Value = Znumber.Value
                         = Vnumeric.Value = WnumericUp.Value =
                         Unumb.Value = 1;
                        EpsenRobo.jogU = EpsenRobo.jogV = EpsenRobo.jogX =
                       EpsenRobo.jogY = EpsenRobo.jogW = EpsenRobo.jogZ = 1f;

                        break;

                    case "寸动长距10":
                        Xnum.Value = Ynumb.Value = Znumber.Value
                         = Vnumeric.Value = WnumericUp.Value =
                         Unumb.Value = 10;
                        EpsenRobo.jogU = EpsenRobo.jogV = EpsenRobo.jogX =
                      EpsenRobo.jogY = EpsenRobo.jogW = EpsenRobo.jogZ = 10f;

                        break;

                    default:
                        EpsenRobo.JogMode = false;
                        break;
                }
                butReset.Focus();
            }
            catch (Exception)
            {
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iscont)
                {
                    return;
                }
                butReset.Focus();
                if (comboBox1.SelectedItem == null)
                {
                    EpsenRobo.SetSeep(10, 10, 10);
                    return;
                }
                switch (comboBox1.SelectedItem.ToString())
                {
                    case "中低速":
                        EpsenRobo.SetSeep(60, 80, 80);
                        break;

                    case "中速":
                        EpsenRobo.SetSeep(200, 80, 80);
                        break;

                    case "高速":
                        EpsenRobo.SetSeep(400, 100, 100);
                        break;

                    default:
                        EpsenRobo.SetSeep(20, 20, 20);
                        break;
                }
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
                EpsenRobo.Pause();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                EpsenRobo.Pause(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butYAd_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (EpsenRobo.JogMode)
                {
                    EpsenRobo.SendCommand("StepMove,Y,Plus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogY.ToString());
                }
                else
                {
                    AxJog = "Y";
                    JogMinPin = true;
                    IsMove = true;
                }
            }
            catch (Exception)
            {
            }
        }

        private void butYAd_MouseUp(object sender, MouseEventArgs e)
        {
            IsMove = false;
            if (comboBox2.SelectedItem.ToString() == "点动模式")
            {
                EpsenRobo.Stop();
            }
        }

        private void butXA_MouseDown(object sender, MouseEventArgs e)
        {
            if (EpsenRobo.JogMode)
            {
                EpsenRobo.SendCommand("StepMove,X,Plus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogX.ToString());
            }
            else
            {
                AxJog = "X";
                JogMinPin = true;
                IsMove = true;
            }
        }

        private void butXA_MouseUp(object sender, MouseEventArgs e)
        {
            IsMove = false;
            if (comboBox2.SelectedItem.ToString() == "点动模式")
            {
                EpsenRobo.Stop();
            }
        }

        private void butXS_MouseDown(object sender, MouseEventArgs e)
        {
            if (!EpsenRobo.JogMode)
            {
                AxJog = "X";
                JogMinPin = false;
                IsMove = true;
                return;
            }
            EpsenRobo.SendCommand("StepMove,X,Minus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogX.ToString());
        }

        private void butXS_MouseUp(object sender, MouseEventArgs e)
        {
            IsMove = false;
            if (comboBox2.SelectedItem.ToString() == "点动模式")
            {
                EpsenRobo.Stop();
            }
        }

        private void butYSd_MouseDown(object sender, MouseEventArgs e)
        {
            if (!EpsenRobo.JogMode)
            {
                AxJog = "Y";
                IsMove = true;
                JogMinPin = false;
                return;
            }
            EpsenRobo.SendCommand("StepMove,Y,Minus,", EpsenRobo.Seelp.ToString(), EpsenRobo.jogY.ToString());
        }

        private void butYSd_MouseUp(object sender, MouseEventArgs e)
        {
            IsMove = false;
            if (comboBox2.SelectedItem.ToString() == "点动模式")
            {
                EpsenRobo.Stop();
            }
        }

        private void butZA_MouseDown(object sender, MouseEventArgs e)
        {
            if (!EpsenRobo.JogMode)
            {
                AxJog = "Z";
                JogMinPin = true;
                IsMove = true;
                return;
            }
            EpsenRobo.SendCommand("StepMove,Z,Plus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogZ.ToString());
        }

        private void butZA_MouseUp(object sender, MouseEventArgs e)
        {
            IsMove = false;
            if (comboBox2.SelectedItem.ToString() == "点动模式")
            {
                EpsenRobo.Stop();
            }
        }

        private void butZs_MouseUp(object sender, MouseEventArgs e)
        {
            IsMove = false;
            if (comboBox2.SelectedItem.ToString() == "点动模式")
            {
                EpsenRobo.Stop();
            }
        }

        private void butZs_MouseDown(object sender, MouseEventArgs e)
        {
            if (!EpsenRobo.JogMode)
            {
                AxJog = "Z";
                JogMinPin = false;
                IsMove = true;
                return;
            }
            EpsenRobo.SendCommand("StepMove,Z,Minus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogZ.ToString());
        }

        private void butUA_MouseUp(object sender, MouseEventArgs e)
        {
            IsMove = false;
            if (comboBox2.SelectedItem.ToString() == "点动模式")
            {
                EpsenRobo.Stop();
            }
        }

        private void butUA_MouseDown(object sender, MouseEventArgs e)
        {
            if (!EpsenRobo.JogMode)
            {
                AxJog = "U";
                JogMinPin = true;
                IsMove = true;
                return;
            }
            EpsenRobo.SendCommand("StepMove,U,Plus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogU.ToString());
        }

        private void butUs_MouseDown(object sender, MouseEventArgs e)
        {
            if (!EpsenRobo.JogMode)
            {
                AxJog = "U";
                IsMove = true;
                JogMinPin = false;
                return;
            }
            EpsenRobo.SendCommand("StepMove,U,Minus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogU.ToString());
        }

        private void butUs_MouseUp(object sender, MouseEventArgs e)
        {
            IsMove = false;
            if (comboBox2.SelectedItem.ToString() == "点动模式")
            {
                EpsenRobo.Stop();
            }
        }

        private void butVa_MouseDown(object sender, MouseEventArgs e)
        {
            if (!EpsenRobo.JogMode)
            {
                AxJog = "V";
                IsMove = true;
                JogMinPin = true;
                return;
            }
            EpsenRobo.SendCommand("StepMove,V,Plus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogV.ToString());
        }

        private void butVa_MouseUp(object sender, MouseEventArgs e)
        {
            IsMove = false;
            if (comboBox2.SelectedItem.ToString() == "点动模式")
            {
                EpsenRobo.Stop();
            }
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!EpsenRobo.JogMode)
            {
                AxJog = "V";
                IsMove = true;
                JogMinPin = false;
                return;
            }
            EpsenRobo.SendCommand("StepMove", "V", "Minus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogV.ToString());
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            IsMove = false;
            if (comboBox2.SelectedItem.ToString() == "点动模式")
            {
                EpsenRobo.Stop();
            }
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            if (!EpsenRobo.JogMode)
            {
                AxJog = "W";
                IsMove = true;
                JogMinPin = true;
                return;
            }
            EpsenRobo.SendCommand("StepMove,W,Plus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogW.ToString());
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            IsMove = false;
            if (comboBox2.SelectedItem.ToString() == "点动模式")
            {
                EpsenRobo.Stop();
            }
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            if (!EpsenRobo.JogMode)
            {
                AxJog = "W";
                IsMove = true;
                JogMinPin = false;
                return;
            }
            EpsenRobo.SendCommand("StepMove,W,Minus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogW.ToString());
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            IsMove = false;
            if (comboBox2.SelectedItem.ToString() == "点动模式")
            {
                EpsenRobo.Stop();
            }
        }

        private void Xnum_ValueChanged(object sender, EventArgs e)
        {
            EpsenRobo.jogX = (Single)Xnum.Value;
            if (comboBox2.SelectedIndex != 1)
            {
                return;
            }
            EpsenRobo.jogX1 = (Single)Xnum.Value;
        }

        private void Unumb_ValueChanged(object sender, EventArgs e)
        {
            EpsenRobo.jogU = (Single)Unumb.Value;
            if (comboBox2.SelectedIndex != 1)
            {
                return;
            }
            EpsenRobo.jogU1 = (Single)Unumb.Value;
        }

        private void Ynumb_ValueChanged(object sender, EventArgs e)
        {
            EpsenRobo.jogY = (Single)Ynumb.Value;
            if (comboBox2.SelectedIndex != 1)
            {
                return;
            }
            EpsenRobo.jogY1 = (Single)Ynumb.Value;
        }

        private void Vnumeric_ValueChanged(object sender, EventArgs e)
        {
            EpsenRobo.jogV = (Single)Vnumeric.Value;
            if (comboBox2.SelectedIndex != 1)
            {
                return;
            }
            EpsenRobo.jogV1 = (Single)Vnumeric.Value;
        }

        private void Znumber_ValueChanged(object sender, EventArgs e)
        {
            EpsenRobo.jogZ = (Single)Znumber.Value;
            if (comboBox2.SelectedIndex != 1)
            {
                return;
            }
            EpsenRobo.jogZ1 = (Single)Znumber.Value;
        }

        private void WnumericUp_ValueChanged(object sender, EventArgs e)
        {
            EpsenRobo.jogW = (Single)WnumericUp.Value;
            if (comboBox2.SelectedIndex != 1)
            {
                return;
            }
            EpsenRobo.jogW1 = (Single)WnumericUp.Value;
        }

        private bool iscont;

        private void butYAd_Click_1(object sender, EventArgs e)
        {
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Focus();
                if (comboBox4.SelectedItem == null || !iscont)
                {
                    return;
                }
                else if (comboBox4.SelectedItem.ToString() == "高")
                {
                    EpsenRobo.SendCommand("SetPower", "high");
                }
                else
                {
                    EpsenRobo.SendCommand("SetPower", "low");
                }
            }
            catch (Exception)
            {
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox3.SelectedItem == null || !iscont)
                {
                    return;
                }
                else
                {
                    EpsenRobo.SendCommand("SetTool", comboBox3.SelectedItem.ToString());
                }
            }
            catch (Exception)
            {
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SetToolForm1 setToolForm1 = new SetToolForm1();
            setToolForm1.SET(EpsenRobo);
            setToolForm1.Show();
        }

        private void JogZnubme_ValueChanged(object sender, EventArgs e)
        {
            EpsenRobo.JoupZ = (Single)JogZnubme.Value;
        }

        private void butXA_Click(object sender, EventArgs e)
        {
        }
    }
}