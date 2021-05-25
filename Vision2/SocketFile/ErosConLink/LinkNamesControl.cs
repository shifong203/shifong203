using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ErosSocket.ErosConLink
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LinkNameListControl : UserControl
    {
        public LinkNameListControl()
        {
            InitializeComponent();

            this.Column1.Items.Add("null");
            foreach (var item in DicSocket.Instance.SocketClint.Keys)
            {
                this.Column1.Items.Add(item);
            }
        }

        public LinkNameListControl(List<string> links) : this()
        {
            try
            {
                linkNames = links;
                foreach (var item in linkNames)
                {
                    int indx = dataGridView1.Rows.Add();
                    dataGridView1.Rows[indx].Cells[0].Value = item;
                }
            }
            catch (Exception)
            {
            }

        }

        List<string> linkNames;

        private void LinkNamesControl_Leave(object sender, EventArgs e)
        {

            List<string> list = new List<string>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() != "null")
                    {
                        list.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    }
                }
            }
            this.Tag = list;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
                    LinkNameListControl linkNamesControl = new LinkNameListControl(value as List<string>);
                    service.DropDownControl(linkNamesControl);
                    if (linkNamesControl.Tag != null)
                    {
                        value = linkNamesControl.Tag;
                    }
                }
                return value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
