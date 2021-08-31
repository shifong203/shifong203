using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Vision2.Project
{
    public partial class SimulateQRForm : Form
    {
        public SimulateQRForm()
        {
            InitializeComponent();
        }

        private void simulateQRForm_Load(object sender, EventArgs e)
        {
            try
            {
                DebugF.DebugCompiler.Buzzer = true;

                Thread thread = new Thread(() =>
                {
                    while (this.Visible)
                    {
                        try
                        {
                            if (checkBox1.Checked)
                            {
                                checkBox1.BackColor = Color.Red;
                                Thread.Sleep(500);
                            }
                            checkBox1.BackColor = Color.Wheat;
                        }
                        catch (Exception)
                        {
                        }
                        Thread.Sleep(500);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 显示文本显示，
        /// </summary>
        /// <param name="text"></param>
        /// <param name="await"></param>
        public static void ShowMesabe(string text, bool await = false)
        {
            try
            {
                if (await)
                {
                    Thread thread = new Thread(() =>
                    {
                        MainForm1.MainFormF.Invoke(new Action(() =>
                        {
                            SimulateQRForm simulateQRForm = new SimulateQRForm();
                            simulateQRForm.label2.Text = "提示信息:" + text;
                            simulateQRForm.richTextBox1.Text = text;
                            simulateQRForm.groupBox1.Visible = false;
                            simulateQRForm.textBox1.Visible = false;
                            simulateQRForm.button2.Visible = false;
                            ErosProjcetDLL.UI.UICon.SwitchToThisWindow(simulateQRForm.Handle, true);
                            simulateQRForm.ShowDialog();
                        }));
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                else
                {
                    MainForm1.MainFormF.Invoke(new Action(() =>
                    {
                        SimulateQRForm simulateQRForm = new SimulateQRForm();
                        simulateQRForm.label2.Text = "提示信息:" + text;
                        simulateQRForm.richTextBox1.Text = text;
                        simulateQRForm.groupBox1.Visible = false;
                        simulateQRForm.textBox1.Visible = false;
                        simulateQRForm.button2.Visible = false;
                        ErosProjcetDLL.UI.UICon.SwitchToThisWindow(simulateQRForm.Handle, true);
                        simulateQRForm.ShowDialog();
                    }));
                }
            }
            catch (Exception)
            {
            }
        }

        public static bool ShowMesabe(string text, out string restText)
        {
            restText = "";
            try
            {
                SimulateQRForm simulateQRForm = new SimulateQRForm();
                simulateQRForm.label2.Text = "提示信息:" + text;
                simulateQRForm.richTextBox1.Text = text;
                ErosProjcetDLL.UI.UICon.SwitchToThisWindow(simulateQRForm.Handle, true);
                if (simulateQRForm.ShowDialog() == DialogResult.Yes)
                {
                    restText = simulateQRForm.textBox1.Text;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
            }
            return false;
        }

        private void simulateQRForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void simulateQRForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F10)
                {
                    if (checkBox1.Checked)
                    {
                        checkBox1.Checked = false;
                    }
                    else
                    {
                        checkBox1.Checked = true;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                DebugF.DebugCompiler.FmqIS = true;
            }
            else
            {
                DebugF.DebugCompiler.FmqIS = false;
            }
        }

        private void simulateQRForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DebugF.DebugCompiler.Buzzer = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}