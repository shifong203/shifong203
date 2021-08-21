using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ErosSocket.ErosConLink
{
    public partial class LinkName_ValuesNameUserControl : UserControl
    {
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
                    value = "";
                }
                if (service != null)
                {
                    LinkName_ValuesNameUserControl linkNamesControl = new LinkName_ValuesNameUserControl(value.ToString());
                    service.DropDownControl(linkNamesControl);
                    if (linkNamesControl.Tag != null)
                    {
                        value = linkNamesControl.Tag;
                    }
                }
                return value;
            }
        }

        public LinkName_ValuesNameUserControl()
        {
            InitializeComponent();
        }

        public LinkName_ValuesNameUserControl(string value = null)
        {
            InitializeComponent();
            if (value == null)
            {
                value = "";
            }
            textBox1.AppendText(value);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            this.Tag = textBox1.Text;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                string[] keys = textBox1.Text.Split('.');
                if (textBox1.Text.Contains('.'))
                {
                    if (StaticCon.GetLingkNames().Contains(keys[0]))
                    {
                        if (StaticCon.GetLingkNmaeValues(keys[0]).Contains(keys[1]))
                        {
                            return;
                        }
                        foreach (var item in StaticCon.GetLingkNmaeValues(keys[0]))
                        {
                            contextMenuStrip.Items.Add(item).Click += LinkName_ValuesNameUserControl_Click;
                        }
                    }
                }
                else
                {
                    foreach (var item in DicSocket.Instance.SocketClint.Keys)
                    {
                        contextMenuStrip.Items.Add(item).Click += LinkName_ValuesNameUserControl_Click;
                    }
                }
                contextMenuStrip.Show(textBox1.PointToScreen(textBox1.Location));
                textBox1.Focus();
                textBox1.SelectionStart = textBox1.Text.Length;
            }
            catch (Exception)
            {
            }
        }

        private void LinkName_ValuesNameUserControl_Click(object sender, EventArgs e)
        {
            ToolStripItem stripItem = (ToolStripItem)sender;
            if (textBox1.Text.Contains("."))
            {
                string[] item = textBox1.Text.Split('.');

                textBox1.Text = item[0] + "." + stripItem.Text;
                this.Tag = textBox1.Text;
                this.FindForm().Focus();
                //isCont = true;
                return;
            }
            else
            {
                textBox1.Text = stripItem.Text + ".";
            }
            textBox1.Focus();
            textBox1.SelectionStart = textBox1.Text.Length;
        }
    }
}