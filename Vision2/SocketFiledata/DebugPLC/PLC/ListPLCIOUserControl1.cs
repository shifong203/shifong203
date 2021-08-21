using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ErosSocket.DebugPLC.PLC
{
    public partial class ListPLCIOUserControl1 : UserControl
    {
        public ListPLCIOUserControl1(List<string> lina)
        {
            InitializeComponent();
            dataGridView1.DataError += DataGridView1_DataError;

            if (lina == null)
            {
                return;
            }
            for (int i = 0; i < lina.Count; i++)
            {
                int d = dataGridView1.Rows.Add();
                dataGridView1.Rows[d].Cells[0].Value = lina[i];
            }
        }

        private void DataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {
                bool isd = false;
                this.Column1.Items.Clear();
                foreach (var item in DebugComp.GetThis().DicPLCIO)
                {
                    isd = false;
                    for (int i2 = 0; i2 < dataGridView1.Rows.Count; i2++)
                    {
                        if (dataGridView1.Rows[i2].Cells[0].Value != null)
                        {
                            if (item.Key == dataGridView1.Rows[i2].Cells[0].Value.ToString())
                            {
                                isd = true;
                                break;
                            }
                        }
                    }
                    if (!isd)
                    {
                        this.Column1.Items.Add(item.Value.Name);
                    }
                }

                Vision2.ErosProjcetDLL.UI.DataGridViewF.DataGridViewComboEditBoxColumn dataGridViewComboBoxColum = dataGridView1.SelectedCells[0].OwningColumn as Vision2.ErosProjcetDLL.UI.DataGridViewF.DataGridViewComboEditBoxColumn;
                //if (dataGridViewComboBoxColum!=null)
                //{
                //    dataGridViewComboBoxColum.Items.Clear();
                //}
                if (DebugComp.GetThis().DicPLCIO.ContainsKey(dataGridView1.SelectedCells[0].Value.ToString()))
                {
                    linkIDValueControl1.Up(DebugComp.GetThis().DicPLCIO[dataGridView1.SelectedCells[0].Value.ToString()]);
                }
                else
                {
                    if (DialogResult.Yes == MessageBox.Show("不存在" + dataGridView1.SelectedCells[0].Value.ToString(), "是否添加", MessageBoxButtons.YesNo))
                    {
                        DebugComp.GetThis().DicPLCIO.Add(dataGridView1.SelectedCells[0].Value.ToString(), new ErosConLink.UClass.PLCValue() { Name = dataGridView1.SelectedCells[0].Value.ToString() });
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void ListPLCIOUserControl1_Leave(object sender, EventArgs e)
        {
            try
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
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (DebugComp.GetThis().DicPLCIO.ContainsKey(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()))
                {
                    linkIDValueControl1.Up(DebugComp.GetThis().DicPLCIO[dataGridView1.SelectedRows[0].Cells[0].Value.ToString()]);
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
                    ListPLCIOUserControl1 linkNamesControl = new ListPLCIOUserControl1(value as List<string>);
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