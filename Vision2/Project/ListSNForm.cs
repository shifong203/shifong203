using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.Project
{
    public partial class ListSNForm : Form
    {
        public ListSNForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 显示文本显示，
        /// </summary>
        /// <param name="text"></param>
        /// <param name="await"></param>
        public static void ShowMesabe(string text, string[] pasts,bool await = false)
        {
            try
            {
                if (await)
                {
                    Thread thread = new Thread(() =>
                    {
                        MainForm1.MainFormF.Invoke(new Action(() =>
                        {
                            ListSNForm simulateQRForm = new ListSNForm();
                            simulateQRForm.label2.Text = "提示信息:" + text;
                            simulateQRForm.listBox1.Items.Clear();
                            simulateQRForm.listBox1.Items.AddRange(pasts);
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
                        ListSNForm simulateQRForm = new ListSNForm();
                        simulateQRForm.label2.Text = "提示信息:" + text;
                        simulateQRForm.listBox1.Items.Clear();
                        simulateQRForm.listBox1.Items.AddRange(pasts);
                        ErosProjcetDLL.UI.UICon.SwitchToThisWindow(simulateQRForm.Handle, true);
                        simulateQRForm.ShowDialog();
                    }));
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
