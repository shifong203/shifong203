using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Vision2.vision
{
    public partial class ValueMaxMinContrl : UserControl
    {
        public ValueMaxMinContrl()
        {
            InitializeComponent();
        }
        public ValueMaxMinContrl(object valer):this()
        {
            propertyGrid1.SelectedObject = valer;
            this.Tag = propertyGrid1.SelectedObject;
        }
        private void UserControl1_Leave(object sender, EventArgs e)
        {
            this.Tag = propertyGrid1.SelectedObject;
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
                if (value == null)
                {
                    //value = "";
                }
                if (service != null)
                {
                    ValueMaxMinContrl linkNamesControl = new ValueMaxMinContrl(value);
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
