using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Vision2.ErosProjcetDLL.UI.PropertyGrid
{
    public partial class ListStringEditorControl : UserControl
    {
        public ListStringEditorControl(List<string> listStr)
        {
            InitializeComponent();
            try
            {
                listString = listStr;
                DataGridViewF.StCon.AddCon(dataGridView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        List<string> listString;
        private void ListStringEditorControl_Load(object sender, EventArgs e)
        {
            foreach (var item in listString)
            {
                int i = dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = item;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void ListStringEditorControl_Leave(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value != null)
                    {
                        list.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    }

                }
                this.Tag = list;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class ListStringEditor : UITypeEditor
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
                if (value is List<string>)
                {
                    ListStringEditorControl listStringEditorControl = new ListStringEditorControl(value as List<string>);
                    service.DropDownControl(listStringEditorControl);
                    if (listStringEditorControl.Tag != null)
                    {
                        value = listStringEditorControl.Tag;
                    }
                    listStringEditorControl.Dispose();
                }
            }
            return value;
        }
    }
}
