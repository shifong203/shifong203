using HalconDotNet;
using System;
using System.IO;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace Vision2
{
    internal static class Program
    {
        public static System.Threading.Mutex Run;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            bool noRun = false;
            BindExceptionHandler();
            Run = new System.Threading.Mutex(true, "HumControl2", out noRun);
            if (noRun)
            {
                if (IntPtr.Size == 4)
                {
                    File.Copy(Application.StartupPath + "\\X86\\FY6400.dll", Application.StartupPath + "\\FY6400.dll", true);
                    File.Copy(Application.StartupPath + "\\X86\\halcon.dll", Application.StartupPath + "\\halcon.dll", true);
                    File.Copy(Application.StartupPath + "\\X86\\ThridLibray.dll", Application.StartupPath + "\\ThridLibray.dll", true);
                    File.Copy(Application.StartupPath + "\\X86\\CLIDelegate.dll", Application.StartupPath + "\\CLIDelegate.dll", true);
                }
                else if (IntPtr.Size == 8)
                {
                    File.Copy(Application.StartupPath + "\\X64\\FY6400.dll", Application.StartupPath + "\\FY6400.dll", true);
                    File.Copy(Application.StartupPath + "\\X64\\halcon.dll", Application.StartupPath + "\\halcon.dll", true);
                    File.Copy(Application.StartupPath + "\\X64\\ThridLibray.dll", Application.StartupPath + "\\ThridLibray.dll", true);
                    File.Copy(Application.StartupPath + "\\X64\\CLIDelegate.dll", Application.StartupPath + "\\CLIDelegate.dll", true);
                }
                File.Copy(Application.StartupPath + "\\X64\\ICSharpCode.SharpZipLib.dll", Application.StartupPath + "\\ICSharpCode.SharpZipLib.dll", true);
                Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "300000" /*ms*/);
                HOperatorSet.SetSystem("tsp_width", 900000000);
                HOperatorSet.SetSystem("tsp_height", 900000000);
                HOperatorSet.SetSystem("clip_region", "false");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                string curPath = Environment.CurrentDirectory;
                string basePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                if (!Directory.Exists(ProjectINI.ProjietPath))
                {
                    ProjectINI.ProjietPath = "D:\\Vision2\\";
                    if (!Directory.Exists(ProjectINI.ProjietPath))
                    {
                        MessageBox.Show("缺少项目文件");
                        return;
                    }
                }
                try
                {
                    Application.Run(new Project.LodingProject());
                }
                catch (Exception ex)
                {
                    ErrForm.Show(ex,"主界面");
                    Program.Run.Close();
                    ProjectINI.In.Clros();
                }
            }
            else
            {
                DialogResult dialog =
                MessageBox.Show("软件已启动，请关闭或重新启动", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
                if (dialog == DialogResult.OK)
                {
                    Program.Run.Close();
                    Application.Restart();
                }
                else
                {
                    Program.Run.Close();
                }
            }
        }
        /// <summary>
        /// 绑定程序中的异常处理
        /// </summary>
        private static void BindExceptionHandler()
        {
            //设置应用程序处理异常方式：ThreadException处理
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理未捕获的异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }
        /// <summary>
        /// 处理UI线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
        }
        /// <summary>
        /// 处理未捕获的异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

    }
}