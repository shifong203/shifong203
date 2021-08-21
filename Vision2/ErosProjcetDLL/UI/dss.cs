using System.Threading;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.UI
{
    /// <summary>
    /// 进度条
    /// </summary>
    public partial class dss : UserControl
    {
        public dss()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //MethodInvoker methodInvoker = new MethodInvoker(UpCyc);
            //this.Invoke(methodInvoker);
            Thread thread = new Thread(UpCyc);
            thread.Start();
        }

        private void UpCyc()
        {
            while (!this.Disposing && this.Visible)
            {
                progressBar2.Value++;
                Thread.Sleep(100);
                if (progressBar2.Value > 99)
                {
                    Thread.Sleep(1000);
                    progressBar2.Value = 0;
                }
            }
        }
    }
}