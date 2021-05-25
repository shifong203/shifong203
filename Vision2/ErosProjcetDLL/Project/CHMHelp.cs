using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Project
{

    public interface IHelp
    {
        void ShowHelp();

    }
    public class CHMHelp
    {
        public static void ShowHelp()
        {
            Help.ShowHelp(null, Application.StartupPath + @"\help.chm");
        }
        public static void ShowHelp(string helpFile)
        {
            if (System.Environment.OSVersion.Platform == System.PlatformID.Win32NT)
            {
                HtmlHelpW(GetDesktopWindow(), helpFile, HH_DISPLAY_TOPIC, 0);//342342
            }
            else
            {
                HtmlHelpA(GetDesktopWindow(), helpFile, HH_DISPLAY_TOPIC, 0);//342342
            }
        }

        [DllImport("hhctrl.ocx", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr HtmlHelpW(IntPtr hwnd, string HelpFile, int Command, int TopicID);

        [DllImport("hhctrl.ocx", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr HtmlHelpA(IntPtr hwnd, string HelpFile, int Command, int TopicID);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();

        public const int HH_DISPLAY_TOC = 0x0001;

        public const int HH_DISPLAY_INDEX = 0x0002;

        public const int HH_DISPLAY_SEARCH = 0x0003;

        public const int HH_DISPLAY_TOPIC = 0x000;
    }
}
