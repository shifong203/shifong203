using System;
using System.Drawing;
using System.Windows.Forms;
using static Vision2.vision.HalconRunFile.RunProgramFile.Select_shape_Min_Max;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public partial class Select_obj_type : UserControl
    {
        public Select_obj_type()
        {
            isChaer = true;
            InitializeComponent();

            Column1.Items.AddRange(Enum.GetNames(typeof(Enum_Select_Type)));
            Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
            isChaer = false;
        }

        private Select_shape_Min_Max _Min_Max;
        private bool isChaer;

        public Select_obj_type(Select_shape_Min_Max select_Obj_Type) : this()
        {
            SetData(select_Obj_Type);
        }

        public void SetData(Select_shape_Min_Max select_Shape_Min_Max)
        {
            isChaer = true;
            try
            {
                _Min_Max = select_Shape_Min_Max;
                dataGridView1.Rows.Clear();
                dataGridView1.Rows.Add(_Min_Max.Select_Type.Length);
                for (int i = 0; i < _Min_Max.Select_Type.Length; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = _Min_Max.Select_Type.TupleSelect(i).S;
                    if (_Min_Max.Min.Length <= i)
                    {
                        _Min_Max.Min.Append(10);
                    }
                    if (_Min_Max.Max.Length <= i)
                    {
                        _Min_Max.Max.Append(999999);
                    }

                    dataGridView1.Rows[i].Cells[1].Value = _Min_Max.Min.TupleSelect(i);
                    dataGridView1.Rows[i].Cells[2].Value = _Min_Max.Max.TupleSelect(i);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isChaer = false;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isChaer)
            {
                return;
            }
            try
            {
                _Min_Max.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    if (dataGridView1.Rows[i].Cells[0].Value == null)
                    {
                        continue;
                    }
                    if (dataGridView1.Rows[i].Cells[1].Value != null && dataGridView1.Rows[i].Cells[2].Value != null)
                    {
                        if (!_Min_Max.AddSelectType(dataGridView1.Rows[i].Cells[0].Value.ToString(), dataGridView1.Rows[i].Cells[1].Value.ToString(), dataGridView1.Rows[i].Cells[2].Value.ToString()))
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
                _Min_Max.Clear();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    if (dataGridView1.Rows[i].Cells[0].Value == null)
                    {
                        continue;
                    }
                    if (dataGridView1.Rows[i].Cells[1].Value != null && dataGridView1.Rows[i].Cells[2].Value != null)
                    {
                        if (!_Min_Max.AddSelectType(dataGridView1.Rows[i].Cells[0].Value.ToString(), dataGridView1.Rows[i].Cells[1].Value.ToString(), dataGridView1.Rows[i].Cells[2].Value.ToString()))
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3)
                {
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}