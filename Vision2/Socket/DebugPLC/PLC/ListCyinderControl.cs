using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ErosSocket.DebugPLC.PLC
{
    public partial class ListCyinderControl : UserControl
    {
        public ListCyinderControl(List<string> listCy)
        {
            InitializeComponent();

            //for (int i = 0; i < DebugComp.GetThis().DicCylinder.Count; i++)
            //{
            //    this.Column1.Items.Add(DebugComp.GetThis().DicCylinder[i].Name);
            //}
            dataGridView1.DataError += DataGridView1_DataError;
            for (int i = 0; i < listCy.Count; i++)
            {
                int d = dataGridView1.Rows.Add();
                dataGridView1.Rows[d].Cells[0].Value = listCy[i];
            }
        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
                    ListCyinderControl linkNamesControl = new ListCyinderControl(value as List<string>);
                    service.DropDownControl(linkNamesControl);
                    if (linkNamesControl.Tag != null)
                    {
                        value = linkNamesControl.Tag;
                    }
                }
                return value;
            }
        }

        private void ListCyinderControl_Leave(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    list.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                }
            }
            this.Tag = list;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {
                bool isd = false;
                this.Column1.Items.Clear();
                foreach (var item in DebugComp.GetThis().DicCylinder)
                {
                    isd = false;
                    for (int i2 = 0; i2 < dataGridView1.Rows.Count; i2++)
                    {
                        if (dataGridView1.Rows[i2].Cells[0].Value != null)
                        {
                            if (item.Value.Name == dataGridView1.Rows[i2].Cells[0].Value.ToString())
                            {
                                isd = true;
                                continue;
                            }
                        }
                    }
                    if (!isd)
                    {
                        this.Column1.Items.Add(item.Value.Name);
                    }

                }
                for (int i = 0; i < DebugComp.GetThis().DicCylinder.Count; i++)
                {

                }

                if (DebugComp.GetThis().DicCylinder.ContainsKey(dataGridView1.SelectedCells[0].Value.ToString()))
                {
                    cylinderControl1.Up(DebugComp.GetThis().DicCylinder[dataGridView1.SelectedCells[0].Value.ToString()]);
                }

            }
            catch (Exception)
            {


            }
        }
    }


}
