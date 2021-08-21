using System;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC.Robot
{
    public partial class DebugRobot : Form
    {
        public DebugRobot()
        {
            InitializeComponent();
        }

        public DebugRobot(IAxisGrub axisGru) : this()
        {
            steppingControl1.SetAxiss(axisGru);
        }

        public DebugRobot(EpsenRobot6 epsenRobot6) : this()
        {
            EpsenRobo = epsenRobot6;
            steppingControl1.SetAxiss(EpsenRobo);
        }

        private EpsenRobot6 EpsenRobo;

        public void SetAxis(IAxisGrub iAxisGrub)
        {
            steppingControl1.SetAxiss(iAxisGrub);
            axisGrub = iAxisGrub;
            Text = iAxisGrub.Name;
        }

        private IAxisGrub axisGrub;

        private void DebugRobot_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Modifiers == Keys.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.C:
                            this.Focus();
                            EpsenRobo.Stop();
                            break;

                        case Keys.Up:
                            if (EpsenRobo != null)
                            {
                                EpsenRobo.SendCommand("StepMove,Y,Plus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogY.ToString());
                            }
                            if (axisGrub != null)
                            {
                                axisGrub.GetAxis(1).JogAdd(false, axisGrub.JogMode, axisGrub.GetAxis(1).Jog_Distance);
                            }
                            this.Focus();
                            break;

                        case Keys.Down:
                            if (EpsenRobo != null)
                            {
                                EpsenRobo.SendCommand("StepMove,Y,Minus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogY.ToString());
                            }
                            if (axisGrub != null)
                            {
                                axisGrub.GetAxis(1).JogAdd(true, axisGrub.JogMode, axisGrub.GetAxis(1).Jog_Distance);
                            }
                            this.Focus();
                            break;

                        case Keys.Right:
                            if (EpsenRobo != null)
                            {
                                EpsenRobo.SendCommand("StepMove,X,Minus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogX.ToString());
                            }
                            if (axisGrub != null)
                            {
                                axisGrub.GetAxis(0).JogAdd(true, axisGrub.JogMode, axisGrub.GetAxis(0).Jog_Distance);
                            }
                            this.Focus();
                            break;

                        case Keys.Left:
                            if (EpsenRobo != null)
                            {
                                EpsenRobo.SendCommand("StepMove,X,Plus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogX.ToString());
                            }
                            if (axisGrub != null)
                            {
                                axisGrub.GetAxis(0).JogAdd(false, axisGrub.JogMode, axisGrub.GetAxis(0).Jog_Distance);
                            }
                            this.Focus();
                            break;

                        default:
                            break;
                    }
                }
                else if (e.Modifiers == Keys.Alt)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.C:
                            if (EpsenRobo != null)
                            {
                                EpsenRobo.Stop();
                            }
                            this.Focus();
                            break;

                        case Keys.Up:
                            if (EpsenRobo != null)
                            {
                                EpsenRobo.SendCommand("StepMove,Z,Plus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogZ.ToString());
                            }
                            if (axisGrub != null && axisGrub.GetAxis(2) != null)
                            {
                                axisGrub.GetAxis(2).JogAdd(false, axisGrub.JogMode, axisGrub.GetAxis(2).Jog_Distance);
                            }
                            this.Focus();
                            break;

                        case Keys.Down:
                            this.Focus();
                            if (EpsenRobo != null)
                            {
                                EpsenRobo.SendCommand("StepMove,Z,Minus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogZ.ToString());
                            }
                            if (axisGrub != null && axisGrub.GetAxis(2) != null)
                            {
                                axisGrub.GetAxis(2).JogAdd(true, axisGrub.JogMode, axisGrub.GetAxis(2).Jog_Distance);
                            }
                            break;

                        case Keys.Right:
                            this.Focus();
                            if (EpsenRobo != null)
                            {
                                EpsenRobo.SendCommand("StepMove,U,Minus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogU.ToString());
                            }
                            if (axisGrub != null && axisGrub.GetAxis(3) != null)
                            {
                                axisGrub.GetAxis(3).JogAdd(true, axisGrub.JogMode, axisGrub.GetAxis(3).Jog_Distance);
                            }

                            break;

                        case Keys.Left:
                            this.Focus();
                            if (EpsenRobo != null)
                            {
                                EpsenRobo.SendCommand("StepMove,U,Plus", EpsenRobo.Seelp.ToString(), EpsenRobo.jogU.ToString());
                            }
                            if (axisGrub != null && axisGrub.GetAxis(3) != null)
                            {
                                axisGrub.GetAxis(3).JogAdd(false, axisGrub.JogMode, axisGrub.GetAxis(3).Jog_Distance);
                            }

                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void steppingControl1_Load(object sender, EventArgs e)
        {
        }

        private void DebugRobot_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void DebugRobot_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }
    }
}