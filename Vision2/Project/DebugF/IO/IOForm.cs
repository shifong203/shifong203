using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.Project.DebugF.IO
{
    public partial class IOForm : Form
    {
        public IOForm()
        {
            InitializeComponent();
        }

        private void IOForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.didoUserControl1.setDODI(DebugCompiler.GetDoDi());
                if (DODIAxis.Debug)
                {
                    toolStripButton2.Visible = true;
                    toolStripButton1.BackColor = Color.GreenYellow;
                }
                else
                {
                    toolStripButton2.Visible = false;
                    toolStripButton1.BackColor = Color.Transparent;
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (DODIAxis.Debug)
                {
                    DODIAxis.Debug = false;
                    toolStripButton2.Visible = false;
                    toolStripButton1.BackColor = Color.Transparent;
                }
                else
                {
                    toolStripButton1.BackColor = Color.GreenYellow;
                    DODIAxis.Debug = true;
                    toolStripButton2.Visible = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (DebugCompiler.Instance.DDAxis.GetAxisName(DebugCompiler.Instance.AxisNameS) != null)
                {
                    if (DebugCompiler.Instance.DDAxis.GetAxisName(DebugCompiler.Instance.AxisNameS).IsMove)
                    {
                        DebugCompiler.Instance.DDAxis.GetAxisName(DebugCompiler.Instance.AxisNameS).IsMove = false;
                    }
                    else
                    {
                        DebugCompiler.Instance.DDAxis.GetAxisName(DebugCompiler.Instance.AxisNameS).IsMove = true;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}