using ErosSocket.DebugPLC;
using ErosSocket.DebugPLC.PLC;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.Project.DebugF.PLC
{
    public partial class PLCDebug : UserControl
    {
        public PLCDebug()
        {
            InitializeComponent();
        }

        private void userControl11_Load(object sender, EventArgs e)
        {
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
        }

        private void PLCDebug_Load(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                //DebugCompiler debugCompiler = DebugCompiler.GetThis();
                foreach (var item in DebugComp.GetThis().DicAxes)
                {
                    AxisControl axis = new AxisControl(item.Value);
                    tabPage2.Controls.Add(axis);
                    int sd = i / 3;
                    int dt = i % 3;
                    axis.Location = new Point(new Size(axis.Width * dt, axis.Height * sd));
                    i++;
                }

                i = 0;

                foreach (var item in DebugComp.GetThis().DicCylinder)
                {
                    CylinderControl cylinder = new CylinderControl(item.Value);
                    tabPage4.Controls.Add(cylinder);
                    int sd = i / 4;
                    int dt = i % 4;
                    cylinder.Location = new Point(new Size(cylinder.Width * dt, cylinder.Height * sd));
                    i++;
                }
                //System.Threading.Thread thread = new System.Threading.Thread(() => {
                //    while (!this.IsDisposed)
                //    {
                //        try
                //        {
                //            if (tabControl1.SelectedTab.Text == "轴调试")
                //            {
                //                //for ( i = 0; i < tabPage2.Controls.Count; i++)
                //                //{
                //                //    if (tabPage2.Controls[i] is AxisControl)
                //                //    {
                //                //        AxisControl tiem = tabPage2.Controls[i] as AxisControl;
                //                //        //tiem.InvokeUP();
                //                //    }
                //                //}
                //            }
                //        }
                //        catch (Exception)
                //        {
                //        }
                //        System.Threading.Thread.Sleep(200);
                //    }
                //});
                //thread.IsBackground = true;
                //thread.Start();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
    }
}