using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ErosSocket.ErosConLink
{
    public partial class LinkNameAddreControl : UserControl
    {
        public LinkNameAddreControl()
        {
            InitializeComponent();
            foreach (var item in DicSocket.Instance.SocketClint.Keys)
            {
                this.comboBox1.Items.Add(item);
            }
            this.comboBox2.Items.AddRange(UClass.GetTypeList().ToArray());
        }
        public LinkNameAddreControl(string valuet) : this()
        {
            try
            {
                if (valuet != "")
                {
                    string datastr = "";
                    if (valuet.Contains(','))
                    {
                        this.comboBox2.SelectedItem = valuet.Split(',')[1];
                        datastr = valuet.Split(',')[0];
                    }
                    else
                    {
                        datastr = valuet;
                    }

                    string[] dats = datastr.Split('.');
                    this.comboBox1.SelectedItem = dats[0];
                    if (dats.Length > 1)
                    {
                        this.textBox1.Text = datastr.Remove(0, dats[0].Length + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LinkNameControl_Leave(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.SelectedItem != null)
                {
                    this.Tag = this.comboBox1.SelectedItem.ToString() + "." + this.textBox1.Text + "," + comboBox2.SelectedItem.ToString();
                }
                else
                {
                    if (comboBox1.SelectedItem == null)
                    {
                        return;
                    }
                    this.Tag = this.comboBox1.SelectedItem.ToString() + "." + this.textBox1.Text;
                }

            }
            catch (Exception)
            {
            }
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
                        value = "";
                    }
                    LinkNameAddreControl linkNamesControl = new LinkNameAddreControl(value.ToString());
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
