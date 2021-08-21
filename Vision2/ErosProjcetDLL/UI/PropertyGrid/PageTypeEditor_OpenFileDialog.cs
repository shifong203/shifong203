using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Vision2.ErosProjcetDLL.UI.PropertyGrid
{
    /// <summary>
    /// 属性页面编译器///弹出文件标记
    /// </summary>
    public class PageTypeEditor_OpenFileDialog : UITypeEditor
    {
        public static string Filter = "所有文件|*.";

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
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择文件";
                if (value != null)
                {
                    string initia = value.ToString();
                    if (initia.Contains("."))
                    {
                        initia = Path.GetDirectoryName(initia);
                        openFileDialog.FileName = Path.GetFileName(value.ToString());
                    }
                    openFileDialog.InitialDirectory = initia;
                }
                openFileDialog.Multiselect = true;
                if (Filter != "")
                {
                    openFileDialog.Filter = Filter;
                }
                DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    return openFileDialog.FileName;
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