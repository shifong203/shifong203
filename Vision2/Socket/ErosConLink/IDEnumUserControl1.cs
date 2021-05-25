using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ErosSocket.ErosConLink
{
    public partial class IDEnumUserControl1 : UserControl
    {
        public IDEnumUserControl1()
        {
            InitializeComponent();
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
        }
        public IDEnumUserControl1(Dictionary<int, string> listNameValue) : this()
        {
            foreach (var item in listNameValue)
            {
                int indx = dataGridView1.Rows.Add();
                dataGridView1.Rows[indx].Cells[0].Value = item.Key;
                dataGridView1.Rows[indx].Cells[1].Value = item.Value;
            }

        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
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
                        value = new Dictionary<int, string>();
                    }
                    IDEnumUserControl1 linkNamesControl = new IDEnumUserControl1(value as Dictionary<int, string>);
                    service.DropDownControl(linkNamesControl);
                    if (linkNamesControl.Tag != null)
                    {
                        value = linkNamesControl.Tag;
                    }
                }
                return value;
            }
        }

        private void IDEnumUserControl1_Leave(object sender, EventArgs e)
        {
            Dictionary<int, string> link = new Dictionary<int, string>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {
                    if (dataGridView1.Rows[i].Cells[0].Value != null && dataGridView1.Rows[i].Cells[1].Value != null)
                    {
                        string values = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        if (int.TryParse(values, out int tint))
                        {
                            link.Add(tint, dataGridView1.Rows[i].Cells[1].Value.ToString());
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            this.Tag = link;
        }
    }
}
