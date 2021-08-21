using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC.Robot
{
    public partial class Axis4PUserControl1 : UserControl
    {
        public Axis4PUserControl1()
        {
            InitializeComponent();
        }

        private IAxisGrub Axiss;

        public Axis4PUserControl1(IAxisGrub axisGrub) : this()
        {
            setAxisGrud(axisGrub);
            JogMode.Checked = true;
        }

        public void setAxisGrud(IAxisGrub axisG)
        {
            Axiss = axisG;
            JogZnubme.Value = (decimal)axisG.JoupZ;
            if (Axiss.GetAxis(2) == null)
            {
                JogZnubme.Visible = ZsetPBut.Visible = ZsetPointNumber.Visible = butZA.Visible = butZs.Visible =
                Zpoint.Visible = Znumber.Visible = false;
            }
            if (Axiss.GetAxis(3) == null)
            {
                UsetPBut.Visible = UsetPointNumbe.Visible = butUA.Visible = butUs.Visible = UPoint.Visible = Unumb.Visible = false;
            }
            comboBox2.Items.AddRange(Axiss.GetRunCode().ToArray());
            comboBox2.SelectedIndex = 0;

            for (int i = 0; i < Axiss.ListCylin.Count; i++)
            {
                if (DebugComp.GetThis().DicCylinder.ContainsKey(Axiss.ListCylin[i]))
                {
                    DebugPLC.PLC.CylinderControl cylinder = new DebugPLC.PLC.CylinderControl(DebugComp.GetThis().DicCylinder[Axiss.ListCylin[i]]);
                    panel1.Controls.Add(cylinder);
                    int sd = i / 3;
                    int dt = i % 3;
                    cylinder.Location = new Point(new Size(cylinder.Width * dt, cylinder.Height * sd));
                    break;
                }
            }
            if (Axiss is AxisSPD)
            {
                axisSPD = Axiss as AxisSPD;
            }

            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                while (!this.IsDisposed)
                {
                    try
                    {
                        UPDataAx();
                        //System.Threading.Thread.Sleep(100);
                    }
                    catch (Exception ex)
                    {
                    }
                    System.Threading.Thread.Sleep(200);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public void UPDataAx()
        {
            Axiss.GetPoints(out double x, out double y, out double z, out double u);
            UPoint.Text = "U:" + u;
            XPoint.Text = "X:" + x;
            YPoint.Text = "Y:" + y;
            Zpoint.Text = "Z:" + z;
            Axiss.GetSt();
            for (int i = 0; i < panel1.Controls.Count; i++)
            {
                PLC.CylinderControl cylinder = panel1.Controls[i] as DebugPLC.PLC.CylinderControl;
                if (cylinder != null)
                {
                    cylinder.UpC();
                }
            }
            if (Axiss.Aralming)
            {
                label1.BackColor = Color.Red;
            }
            else
            {
                label1.BackColor = Color.GreenYellow;
            }
            if (Axiss.IsHome)
            {
                label2.BackColor = Color.GreenYellow;
            }
            else
            {
                label2.BackColor = Color.Red;
            }
            if (Axiss.enabled)
            {
            }
            else
            {
            }
        }

        private void butXA_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!JogMode.Checked)
                {
                    Axiss.GetAxis(0).Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butXS_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Axiss.GetAxis(0).JogAdd(true, JogMode.Checked, (Single)Xnum.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butXS_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!JogMode.Checked)
                {
                    Axiss.GetAxis(0).Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butYSd_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Axiss.GetAxis(1).JogAdd(true, JogMode.Checked, (Single)Ynumb.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butYSd_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!JogMode.Checked)
                {
                    Axiss.GetAxis(1).Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butUA_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Axiss.GetAxis(3).JogAdd(false, JogMode.Checked, (Single)Unumb.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butUA_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Axiss.GetAxis(3).Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butUs_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Axiss.GetAxis(3).JogAdd(true, JogMode.Checked, (Single)Unumb.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butUs_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Axiss.GetAxis(3).Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butZA_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Axiss.GetAxis(2).JogAdd(false, JogMode.Checked, (Single)Znumber.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butZA_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Axiss.GetAxis(2).Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butZs_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Axiss.GetAxis(2).JogAdd(true, JogMode.Checked, (Single)Znumber.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butZs_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Axiss.GetAxis(2).Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butStop_Click(object sender, EventArgs e)
        {
            Axiss.Stop();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Axiss.SetHome();
        }

        private void butReset_Click(object sender, EventArgs e)
        {
            Axiss.Reset();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Axiss.Enabled();
        }

        private void JogMode_CheckedChanged(object sender, EventArgs e)
        {
            Axiss.JogMode = JogMode.Checked;
        }

        private void butYAd_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Axiss.GetAxis(1).JogAdd(false, JogMode.Checked, (Single)Ynumb.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butYAd_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Axiss.GetAxis(1).Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butXA_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Axiss.GetAxis(0).JogAdd(false, JogMode.Checked, (Single)Xnum.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void XsetPBut_Click(object sender, EventArgs e)
        {
            try
            {
                Axiss.SetPointX(Convert.ToSingle(XSetPointNu.Value));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void YsetPBut_Click(object sender, EventArgs e)
        {
            try
            {
                Axiss.SetPointY(Convert.ToSingle(YSetPointNumb.Value));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ZsetPBut_Click(object sender, EventArgs e)
        {
            try
            {
                Axiss.SetPointZ(Convert.ToSingle(ZsetPointNumber.Value));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private AxisSPD axisSPD;

        private void UsetPBut_Click(object sender, EventArgs e)
        {
            try
            {
                Axiss.SetPointU(Convert.ToSingle(UsetPointNumbe.Value));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Ynumb_ValueChanged(object sender, EventArgs e)
        {
            Axiss.GetAxis(1).Jog_Distance = (Single)Ynumb.Value;
        }

        private void Xnum_ValueChanged(object sender, EventArgs e)
        {
            Axiss.GetAxis(0).Jog_Distance = (Single)Xnum.Value;
        }

        private void Znumber_ValueChanged(object sender, EventArgs e)
        {
            Axiss.GetAxis(2).Jog_Distance = (Single)Znumber.Value;
        }

        private void Unumb_ValueChanged(object sender, EventArgs e)
        {
            Axiss.GetAxis(3).Jog_Distance = (Single)Unumb.Value;
        }

        private void MoveBut_Click(object sender, EventArgs e)
        {
        }

        private void JogZnubme_ValueChanged(object sender, EventArgs e)
        {
            Axiss.JoupZ = (Single)JogZnubme.Value;
        }

        private bool buse;

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.SelectAll();
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionLength = 0;

                if (buse)
                {
                    richTextBox1.ForeColor = Color.Red;
                    return;
                }

                buse = true;
                Task.Run(() =>
                {
                    int lenhy = 0;
                    for (int i = 0; i < richTextBox1.Lines.Length; i++)
                    {
                        System.Threading.Thread.Sleep(1010);
                        richTextBox1.SelectionStart = lenhy;
                        richTextBox1.SelectionLength = richTextBox1.Lines[i].Length + 1;

                        richTextBox1.SelectionColor = Color.Black;
                        string datas = richTextBox1.Lines[i].Trim(' ');
                        if (datas.ToLower().StartsWith("move"))
                        {
                            string[] items = datas.Split(' ');
                            Single? x = null;
                            Single? y = null;
                            Single? z = null;
                            Single? u = null;
                            for (int ite = 0; ite < items.Length; ite++)
                            {
                                if (items[ite].ToUpper().StartsWith("X"))
                                {
                                    x = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].ToUpper().StartsWith("Y"))
                                {
                                    y = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].ToUpper().StartsWith("Z"))
                                {
                                    z = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].ToUpper().StartsWith("U"))
                                {
                                    u = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                            }

                            if (Axiss.SetPoint("Move", x, y, z, u))
                            {
                                richTextBox1.SelectionColor = Color.GreenYellow;
                            }
                            else
                            {
                                richTextBox1.SelectionStart = lenhy;
                                richTextBox1.SelectionLength = richTextBox1.Lines[i].Length + 1;
                                richTextBox1.SelectionColor = Color.Red;

                                buse = false;
                                return;
                            }
                        }
                        else if (datas.ToLower().StartsWith("go"))
                        {
                            string[] items = datas.Split(' ');
                            Single? x = null;
                            Single? y = null;
                            Single? z = null;
                            Single? u = null;
                            if (axisSPD == null)
                            {
                                axisSPD = Axiss as AxisSPD;
                            }
                            for (int ite = 0; ite < items.Length; ite++)
                            {
                                if (items[ite].ToUpper().StartsWith("X"))
                                {
                                    x = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].ToUpper().StartsWith("Y"))
                                {
                                    y = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].ToUpper().StartsWith("Z"))
                                {
                                    z = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].ToUpper().StartsWith("U"))
                                {
                                    u = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].Contains("="))
                                {
                                    string[] datase = items[ite].Split('=');
                                    if (datase.Length == 2)
                                    {
                                        this.axisSPD.SetIOOut(datase[0], datase[1]);
                                    }
                                }
                            }
                            if (Axiss.SetPoint("go", x, y, z, u))
                            {
                                richTextBox1.SelectionColor = Color.GreenYellow;
                            }
                            else
                            {
                                richTextBox1.SelectionStart = lenhy;
                                richTextBox1.SelectionLength = richTextBox1.Lines[i].Length + 1;
                                richTextBox1.SelectionColor = Color.Red;

                                buse = false;
                                return;
                            }
                            //
                        }
                        else if (datas.Contains("="))
                        {
                            string[] datase = datas.Split('=');
                            if (datase.Length == 2)
                            {
                                this.axisSPD.SetIOOut(datase[0], datase[1]);
                            }
                        }
                        lenhy += richTextBox1.Lines[i].Length + 1;
                    }

                    buse = false;
                });
            }
            catch (Exception)
            {
                buse = false;
            }
        }
    }
}