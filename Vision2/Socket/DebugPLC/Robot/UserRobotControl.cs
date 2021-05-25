using System;
using System.Drawing;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC.Robot
{
    public partial class UserRobotControl : UserControl
    {
        public UserRobotControl()
        {
            InitializeComponent();
        }
        public UserRobotControl(EpsenRobot6 robot6) : this()
        {
            Robot6 = robot6;
            if (Vision2.ErosProjcetDLL.Project.ProjectINI.GetUserJurisdiction("工程师"))
            {
                button2.Enabled = button1.Enabled = true;
            }
        }
        EpsenRobot6 Robot6;
        private void UserRobotControl_Load(object sender, EventArgs e)
        {
            Robot6.LinkO += Robot6_LinkO;

            int dn = 1;
            Button button2 = new Button();
            button2.Height = 50;
            button2.Width = 80;
            button2.Click += Button2_Click;
            void Button2_Click(object sender3, EventArgs e3)
            {
                try
                {
                    Robot6.SendCommand("Quit");
                }
                catch (Exception)
                {
                }
            }
            button2.Text = button2.Name = "停止";
            button2.Location = new Point(10, 10);
            this.panel1.Controls.Add(button2);

            foreach (var item2 in Robot6.DicSendMeseage)
            {
                Button button = new Button();
                button.Height = 50;
                button.Width = 80;
                button.Text = button.Name = item2.Key;
                button.Location = new Point(10, 10 + dn * button.Height);
                button.Click += Button_Click;
                this.panel1.Controls.Add(button);
                dn++;
                void Button_Click(object sender1, EventArgs e2)
                {
                    try
                    {
                        Robot6.SendCommand(item2.Value);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

        }

        private string Robot6_LinkO(bool key)
        {
            if (key)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
            return "";
        }

        public void UpRobot()
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    //if (Robot6.GetMode())
                    //{
                    //    this.Enabled = true;
                    //}
                    //else
                    //{
                    //    this.Enabled = false;
                    //}
                    //this.Enabled = true;
                    if (Vision2.ErosProjcetDLL.Project.ProjectINI.GetUserJurisdiction("工程师"))
                    {
                        button2.Enabled = button1.Enabled = true;
                    }
                    Robot6.GetPoints(out double x, out double y, out double z, out double u, out double v, out double w);
                    UPoint.Text = "U:" + u.ToString("0.000");
                    XPoint.Text = "X:" + x.ToString("0.000");
                    YPoint.Text = "Y:" + y.ToString("0.000");
                    Zpoint.Text = "Z:" + z.ToString("0.000");
                    Vpoint.Text = "V:" + v.ToString("0.000");
                    Wpoint.Text = "W:" + w.ToString("0.000");
                    label7.Text = "Tool:" + Robot6.Tool.ToString();
                    if (Robot6.DebugMode)
                    {
                        button1.Text = "调试中";
                    }
                    else
                    {
                        button1.Text = "调试";
                    }

                    if (Robot6.Aralming)
                    {
                        label1.ForeColor = Color.Red;
                        label1.Visible = true;
                    }
                    else
                    {
                        label1.ForeColor = Color.GreenYellow;
                        label1.Visible = false;
                    }
                    if (Robot6.enabled)
                    {
                        label6.ForeColor = Color.Red;
                    }
                    else
                    {
                        label6.ForeColor = Color.GreenYellow;
                    }
                    if (Robot6.Pauseing)
                    {
                        label4.ForeColor = Color.Red;
                        label4.Visible = true;
                    }
                    else
                    {
                        label4.Visible = false;
                        label4.ForeColor = Color.GreenYellow;
                    }
                    if (Robot6.Motor)
                    {
                        label6.ForeColor = Color.Black;
                        label6.BackColor = Color.Yellow;
                    }
                    else
                    {
                        label6.BackColor = Color.Cornsilk;
                        label6.ForeColor = Color.Red;
                    }
                    if (Robot6.High)
                    {
                        label3.Text = "HIGH";
                    }
                    else
                    {
                        label3.Text = "LOW";
                    }
                    label2.Text = "Run:" + Robot6.RunID;

                    if (Vision2.ErosProjcetDLL.Project.ProjectINI.Enbt)
                    {
                        button1.Enabled = true;
                    }
                    else
                    {
                        button1.Enabled = false;
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Robot6.DebugMode)
                {
                    Robot6.SendCommand("DebugMode", "False");
                }
                else
                {
                    Robot6.SendCommand("DebugMode", "true");
                }
            }
            catch (Exception)
            {


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Vision2.ErosProjcetDLL.Project.ProjectINI.GetUserJurisdiction("工程师"))
                {
                    Robot6.DebugFormShow();
                }
                else
                {
                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("权限不足");
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
