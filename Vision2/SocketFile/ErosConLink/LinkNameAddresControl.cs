using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ErosSocket.ErosConLink
{
    public partial class LinkNameAddresControl : UserControl
    {
        public LinkNameAddresControl()
        {
            InitializeComponent();
            Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
        }

        public LinkNameAddresControl(List<string> listad) : this()
        {

        }

        public class Editor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                if (service != null)
                {
                    if (value == null)
                    {
                        value = new List<string>();
                    }
                    LinkNameAddresControl linkNamesControl = new LinkNameAddresControl(value as List<string>);
                    service.DropDownControl(linkNamesControl);
                    if (linkNamesControl.Tag != null)
                    {
                        value = linkNamesControl.Tag;
                    }
                }
                return value;
            }
        }


    }
}
