using System;
using System.Drawing;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI;

namespace ErosSocket.DebugPLC.PLC
{
    public partial class AxisControl : UserControl
    {
        public AxisControl()
        {
            InitializeComponent();
        }

        public AxisControl(IAxis pLCAxis) : this()
        {
            axisData = pLCAxis;
            UpAxisData(axisData);
        }

        private IAxis axisData { get; set; }
        private bool isMove = false;

        public void UpAxisData(IAxis axis)
        {
            try
            {
                isMove = true;
                axisData = axis;
                label9.Text = axisData.Name;
                try
                {
                    numericUpDown1.Value = (decimal)axisData.Jog_Distance;
                    numericUpDown2.Value = (decimal)axisData.MaxVel;
                }
                catch (Exception)
                {
                }
                System.Threading.Thread thread = new System.Threading.Thread(() =>
                {
                    try
                    {
                        System.Threading.Thread.Sleep(1000);
                        isMove = false;

                        while (!this.IsDisposed)
                        {
                            try
                            {
                                if (this.FindForm() == null || this.FindForm().IsDisposed)
                                {
                                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("刷新轴错误1");
                                    return;
                                }
                                if (isMove)
                                {
                                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("刷新轴错误2");
                                    return;
                                }
                                if (!axisData.IsHome)
                                {
                                    labHome.BackColor = Color.Red;
                                }
                                else
                                {
                                    labHome.BackColor = Color.Green;
                                }
                                if (axisData.IsError)
                                {
                                    labErrer.BackColor = Color.Red;
                                }
                                else
                                {
                                    labErrer.BackColor = Color.Green;
                                }
                                if (axisData.IsEnabled)
                                {
                                    labEnble.BackColor = Color.Green;
                                }
                                else
                                {
                                    labEnble.BackColor = Color.Red;
                                }
                                this.Invoke(new Action(() =>
                                {
                                    floatNumTextBox1.Text = axisData.Point.ToString();
                                }));

                                System.Threading.Thread.Sleep(10);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    catch (Exception EX)
                    {
                        Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("刷新轴错误" + EX.Message);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                axisData.Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //axisData. = checkBox1.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void plcBtn3_Click(object sender, EventArgs e)
        {
            try
            {
                axisData.SetHome();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Axis_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void plcBtn4_Click(object sender, EventArgs e)
        {
            try
            {
                axisData.SetPoint(Single.Parse(floatNumTextBox2.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void plcBtn1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                {
                    return;
                }
                axisData.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void plcBtn2_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                {
                    return;
                }
                axisData.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void floatNumTextBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Single.Parse(floatNumTextBox2.Text) > axisData.PlusLimit || Single.Parse(floatNumTextBox2.Text) < axisData.MinusLimit)
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isMove)
                {
                    return;
                }
                axisData.Jog_Distance = (float)numericUpDown1.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void plcBtn2_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                axisData.JogAdd(true, checkBox1.Checked, (Single)numericUpDown1.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void plcBtn1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                axisData.JogAdd(false, checkBox1.Checked, (Single)numericUpDown1.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isMove)
                {
                    return;
                }
                axisData.MaxVel = (Single)numericUpDown2.Value;
                axisData.AddSeelp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void plcBtn1_Click_1(object sender, EventArgs e)
        {
        }

        private void plcBtn5_Click(object sender, EventArgs e)
        {
            try
            {
                axisData.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void plcBtn2_Click_1(object sender, EventArgs e)
        {
        }

        private Pf pf;

        private void label9_Click(object sender, EventArgs e)
        {
            if (pf == null || pf.IsDisposed)
            {
                pf = new Pf(axisData);
            }
            UICon.WindosFormerShow(ref pf);
        }
    }
}