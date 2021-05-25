using ErosSocket.DebugPLC;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.Project.DebugF.IO
{
    public partial class CylinderControl : System.Windows.Forms.UserControl
    {
        public CylinderControl()
        {
            InitializeComponent();
        }
        public CylinderControl(ICylinder cylind) : this()
        {
            Up(cylind);
        }
        ICylinder cylinder;
        /// <summary>
        /// 
        /// </summary>
        public void UpC()
        {
            try
            {
                if (cylinder == null)
                {
                    return;
                }
                if (int.TryParse(cylinder.ProtrudeQ, out int redt))
                {
                    if (DebugCompiler.GetDoDi() != null)
                    {
                        if (DebugCompiler.GetDoDi().Out[redt])
                        {
                            SQlabel.BackColor = Color.Green;
                        }
                        else
                        {
                            SQlabel.BackColor = Color.Red;
                        }
                    }

                }
                if (int.TryParse(cylinder.ProtrudeI, out redt))
                {
                    if (DebugCompiler.GetDoDi() != null)
                    {
                        if (DebugCompiler.GetDoDi().Int[redt])
                        {
                            Silabel.BackColor = Color.Green;
                        }
                        else
                        {
                            Silabel.BackColor = Color.Red;
                        }
                    }
                }
                if (cylinder.CylinderAlram != null)
                {
                    if (cylinder.CylinderAlram.Length != 0)
                    {

                        label1.BackColor = Color.Red;
                        label1.Text = cylinder.Name + "故障";
                    }
                    else
                    {
                        label1.BackColor = Color.Green;
                        label1.Text = cylinder.Name;
                    }
                }
                else
                {
                    label1.BackColor = Color.White;
                    label1.Text = cylinder.Name;
                }

                if (int.TryParse(cylinder.AnastoleI, out redt))
                {
                    if (DebugCompiler.GetDoDi() != null)
                    {
                        if (DebugCompiler.GetDoDi().Int[redt])
                        {
                            EiLabel.BackColor = Color.Green;
                        }
                        else
                        {
                            EiLabel.BackColor = Color.Red;
                        }
                    }
                }
                if (int.TryParse(cylinder.AnastoleQ, out redt))
                {
                    if (DebugCompiler.GetDoDi().Out[redt])
                    {
                        Eqlabel.BackColor = Color.Green;
                    }
                    else
                    {
                        Eqlabel.BackColor = Color.Red;
                    }
                }
            }
            catch (Exception)
            {
                label1.BackColor = Color.Red;
                label1.Text = cylinder.Name + "故障";
            }
        }

        public void Up(ICylinder cylind)
        {
            cylinder = cylind;
            label1.Text = cylinder.Name;

        }

        [DescriptionAttribute("边框颜色。"), Category("外观"), DisplayName("边框颜色")]
        public Color BorderColor { get; set; } = Color.Orange;


        private void UserControl1_Load(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.GetThis().DDAxis.UpCycle += DDAxis_UpCycle;
            }
            catch (Exception)
            {
            }
        }

        private void DDAxis_UpCycle(DODIAxis key)
        {
            try
            {

                if (this.InvokeRequired)
                {
                    //Invoke(new Action<string, MonitoredItem, MonitoredItemNotificationEventArgs>(SubCallback), key, monitoredItem, args);
                    this.Invoke(new Action<DODIAxis>(DDAxis_UpCycle), key);
                    return;
                }
                if (this.IsDisposed)
                {
                    DebugCompiler.GetThis().DDAxis.UpCycle -= DDAxis_UpCycle;
                    return;
                }
                UpC();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// //绘制边框
        /// </summary>
        /// <param name="g">绘制图形</param>
        /// <param name="bordercolor">边框颜色</param>
        /// <param name="x">label宽度</param>
        /// <param name="y">label高度</param>
        private void DrawBorder(System.Drawing.Graphics g, Color bordercolor, int x, int y)
        {
            Rectangle myRectangle = new Rectangle(0, 0, x, y);
            ControlPaint.DrawBorder(g, myRectangle, bordercolor, ButtonBorderStyle.Solid);//画个边框
        }

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            DrawBorder(e.Graphics, BorderColor, this.Width, this.Height);
        }

        private void plcButtonV21_Click(object sender, EventArgs e)
        {
            try
            {
                cylinder.Anastole();
            }
            catch (Exception)
            {

            }
        }

        private void plcButtonV22_Click(object sender, EventArgs e)
        {
            try
            {
                cylinder.Protrude();
            }
            catch (Exception)
            {

            }

        }
    }


}