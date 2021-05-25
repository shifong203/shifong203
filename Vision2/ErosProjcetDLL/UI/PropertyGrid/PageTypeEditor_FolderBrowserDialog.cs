using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Vision2.ErosProjcetDLL.UI.PropertyGrid
{
    /// <summary>
    /// 弹出文件夹选择并跳到指定文件夹
    /// </summary>
    public static class FolderBrowserLauncher
    {
        /// <summary>
        /// Using title text to look for the top level dialog window is fragile.
        /// In particular, this will fail in non-English applications.
        /// </summary>
        private const string _topLevelSearchString = "Browse For Folder";

        /// <summary>
        /// These should be more robust.  We find the correct child controls in the dialog
        /// by using the GetDlgItem method, rather than the FindWindow(Ex) method,
        /// because the dialog item IDs should be constant.
        /// </summary>
        private const int _dlgItemBrowseControl = 0;

        private const int _dlgItemTreeView = 100;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Some of the messages that the Tree View control will respond to
        /// </summary>
        private const int TV_FIRST = 0x1100;

        private const int TVM_SELECTITEM = (TV_FIRST + 11);
        private const int TVM_GETNEXTITEM = (TV_FIRST + 10);
        private const int TVM_GETITEM = (TV_FIRST + 12);
        private const int TVM_ENSUREVISIBLE = (TV_FIRST + 20);

        /// <summary>
        /// Constants used to identity specific items in the Tree View control
        /// </summary>
        private const int TVGN_ROOT = 0x0;

        private const int TVGN_NEXT = 0x1;
        private const int TVGN_CHILD = 0x4;
        private const int TVGN_FIRSTVISIBLE = 0x5;
        private const int TVGN_NEXTVISIBLE = 0x6;
        private const int TVGN_CARET = 0x9;

        /// <summary>
        /// Calling this method is identical to calling the ShowDialog method of the provided
        /// FolderBrowserDialog, except that an attempt will be made to scroll the Tree View
        /// to make the currently selected folder visible in the dialog window.
        /// </summary>
        /// <param name="dlg"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static DialogResult ShowFolderBrowser(FolderBrowserDialog dlg, IWin32Window parent = null)
        {
            DialogResult result = DialogResult.Cancel;
            int retries = 10;
            if (dlg.SelectedPath == "")
            {
                dlg.SelectedPath = Application.StartupPath;
            }
            try
            {
                Directory.CreateDirectory(dlg.SelectedPath);
            }
            catch (Exception)
            {
            }
            using (Timer t = new Timer())
            {
                t.Tick += (s, a) =>
                {
                    if (retries > 0)
                    {
                        --retries;
                        IntPtr hwndDlg = FindWindow((string)null, dlg.SelectedPath);
                        if (hwndDlg != IntPtr.Zero)
                        {
                            IntPtr hwndFolderCtrl = GetDlgItem(hwndDlg, _dlgItemBrowseControl);
                            if (hwndFolderCtrl != IntPtr.Zero)
                            {
                                IntPtr hwndTV = GetDlgItem(hwndFolderCtrl, _dlgItemTreeView);

                                if (hwndTV != IntPtr.Zero)
                                {
                                    IntPtr item = SendMessage(hwndTV, (uint)TVM_GETNEXTITEM, new IntPtr(TVGN_CARET), IntPtr.Zero);
                                    if (item != IntPtr.Zero)
                                    {
                                        SendMessage(hwndTV, TVM_ENSUREVISIBLE, IntPtr.Zero, item);
                                        retries = 0;
                                        t.Stop();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //
                        //  We failed to find the Tree View control.
                        //
                        //  As a fall back (and this is an UberUgly hack), we will send
                        //  some fake keystrokes to the application in an attempt to force
                        //  the Tree View to scroll to the selected item.
                        //
                        t.Stop();
                        SendKeys.Send("{TAB}{TAB}{DOWN}{DOWN}{UP}{UP}");
                    }
                };
                t.Interval = 10;
                t.Start();
                result = dlg.ShowDialog(parent);
            }

            return result;
        }
    }

    /// <summary>
    /// 属性页面编译器///弹出文件夹选择标记
    /// </summary>
    public class PageTypeEditor_FolderBrowserDialog : UITypeEditor
    {
        /// <summary>
        /// 弹出文件夹选择框
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            //return base.EditValue(context, provider, value);
            var edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.Description = "请选择文件夹";
                if (value == null)
                {
                    value = Application.StartupPath;
                }
                fbd.SelectedPath = value.ToString();
                System.Windows.Forms.DialogResult dialog = FolderBrowserLauncher.ShowFolderBrowser(fbd);
                if (dialog == System.Windows.Forms.DialogResult.OK)
                {
                    return fbd.SelectedPath;
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            //return base.GetEditStyle(context);
            return UITypeEditorEditStyle.DropDown;
        }
    }
}
