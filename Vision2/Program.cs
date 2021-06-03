using HalconDotNet;
using System;
using System.IO;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace Vision2
{
    static class Program
    {
        public static System.Threading.Mutex Run;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool noRun = false;
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
                Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "300000" /*ms*/);
                HOperatorSet.SetSystem("tsp_width", 900000000);
                HOperatorSet.SetSystem("tsp_height", 900000000);
                HOperatorSet.SetSystem("clip_region", "false");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                string curPath = System.Environment.CurrentDirectory;
                string basePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
                if (!System.IO.Directory.Exists(ProjectINI.ProjietPath))
                {
                    ProjectINI.ProjietPath = "C:\\Vision2\\";
                    if (!System.IO.Directory.Exists(ProjectINI.In.ProjectPathRun))
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
                    MessageBox.Show(ex.Message);
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
    }
}
