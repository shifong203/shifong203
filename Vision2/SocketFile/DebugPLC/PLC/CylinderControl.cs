using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace ErosSocket.DebugPLC.PLC
{
    public partial class CylinderControl : System.Windows.Forms.UserControl
    {
        public CylinderControl()
        {
            InitializeComponent();
        }
        public CylinderControl(Cylinder cylind) : this()
        {
            Up(cylind);
        }
        Cylinder cylinder;
        /// <summary>
        /// 
        /// </summary>
        public void UpC()
        {

            try
            {
                dynamic vlaue;
                if (cylinder.CylinderSQ != null)
                {
                    if (ErosConLink.StaticCon.GetLingkIDValue(cylinder.CylinderSQ, Boolean.TrueString, out vlaue))
                    {
                        if (vlaue)
                        {
                            SQlabel.BackColor = Color.Red;
                        }
                        else
                        {
                            SQlabel.BackColor = Color.Green;
                        }
                    }
                }
                if (cylinder.CylinderSI != null)
                {
                    ErosConLink.StaticCon.GetLingkIDValue(cylinder.CylinderSI, Boolean.TrueString, out vlaue);
                    if (vlaue)
                    {
                        Silabel.BackColor = Color.Red;
                    }
                    else
                    {
                        Silabel.BackColor = Color.Green;
                    }
                }
                if (cylinder.CylinderAlram != null)
                {
                    ErosConLink.StaticCon.GetLingkIDValue(cylinder.CylinderAlram, Boolean.TrueString, out vlaue);
                    if (vlaue)
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
                if (cylinder.CylinderSM != null)
                {
                    //ErosConLink.StaticCon.GetLingkIDValue(cylinder.CylinderSM, Boolean.TrueString, out vlaue);
                    //if (vlaue)
                    //{
                    //    plcBtn1.BackColor = Color.Red;
                    //}
                    //else
                    //{
                    //    plcBtn1.BackColor = Color.Green;
                    //}
                }

                ErosConLink.StaticCon.GetLingkIDValue(cylinder.CylinderSYI, Boolean.TrueString, out vlaue);
                if (vlaue)
                {
                    EiLabel.BackColor = Color.Red;
                }
                else
                {
                    EiLabel.BackColor = Color.Green;
                }

                ErosConLink.StaticCon.GetLingkIDValue(cylinder.CylinderSYQ, Boolean.TrueString, out vlaue);
                if (vlaue)
                {
                    Eqlabel.BackColor = Color.Red;
                }
                else
                {
                    Eqlabel.BackColor = Color.Green;
                }

            }
            catch (Exception)
            {
                label1.BackColor = Color.Red;
                label1.Text = cylinder.Name + "故障";
            }
        }

        public void Up(Cylinder cylind)
        {
            cylinder = cylind;
            label1.Text = cylinder.Name;
            plcButtonV21.BtnModel = ErosUI.PLCBtn.BtnModel.按下写1抬起0;
            plcButtonV22.BtnModel = ErosUI.PLCBtn.BtnModel.按下写1抬起0;
            plcButtonV21.LinkNameStr = cylinder.CylinderSYM;
            plcButtonV22.LinkNameStr = cylinder.CylinderSM;
        }

        [DescriptionAttribute("边框颜色。"), Category("外观"), DisplayName("边框颜色")]
        public Color BorderColor { get; set; } = Color.Orange;


        private void UserControl1_Load(object sender, EventArgs e)
        {
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

        }

        private void plcButtonV22_Click(object sender, EventArgs e)
        {

        }
    }

    /// <summary>
    /// 气缸控制类
    /// </summary>
    public class Cylinder : INodeNew
    {

        /// <summary>
        /// 伸出Q变量名
        /// </summary>
        /// 
        [DescriptionAttribute("伸出气缸变量名。"), Category("控制"), DisplayName("伸出Q变量名")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string CylinderSQ { get; set; }
        /// <summary>
        /// 缩回Q变量名
        /// </summary>
        [DescriptionAttribute("缩回气缸变量名。"), Category("控制"), DisplayName("缩回Q变量名")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string CylinderSYQ { get; set; }
        /// <summary>
        /// 伸出I变量名
        /// </summary>
        [DescriptionAttribute("伸出信号变量名。"), Category("控制"), DisplayName("伸出I变量名")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string CylinderSI { get; set; }
        /// <summary>
        /// 缩回I变量名
        /// </summary>
        [DescriptionAttribute("缩回信号变量名。"), Category("控制"), DisplayName("缩回I变量名")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string CylinderSYI { get; set; }
        /// <summary>
        /// 伸出M变量名
        /// </summary>
        [DescriptionAttribute("伸出信号变量名。"), Category("控制"), DisplayName("伸出M变量名")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string CylinderSM { get; set; }
        /// <summary>
        /// 缩回M变量名
        /// </summary>
        [DescriptionAttribute("缩回信号变量名。"), Category("控制"), DisplayName("缩回M变量名")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string CylinderSYM { get; set; }
        /// <summary>
        /// 气缸报警状态
        /// </summary>
        [DescriptionAttribute("缩回信号变量名。"), Category("控制"), DisplayName("气缸报警状态")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string CylinderAlram { get; set; }
    }
}