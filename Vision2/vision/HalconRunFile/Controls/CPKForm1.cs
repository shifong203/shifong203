using System;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class CPKForm1 : Form
    {
        public CPKForm1()
        {
            InitializeComponent();
        }


        public HalconRunFile.RunProgramFile.HalconRun HalconRun;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                HalconRun.HobjClear();
                HalconRun.ReadCamImage((numericUpDown1.Value).ToString(), (int)numericUpDown1.Value);
                //HalconRun.CamImageEvent(numericUpDown1.Value.ToString(), null, (int)numericUpDown1.Value);
            }
            catch (Exception)
            {

            }



        }

        private void CPKForm1_Load(object sender, EventArgs e)
        {
            HalconRun.EventDoen += HalconRun_EventDoen;
            Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
        }

        private void HalconRun_EventDoen(vision.OneResultOBj halcon)
        {
            try
            {

                //foreach (var item in halcon.keyValuePairs1)
                //{
                //    if (!dataGridView1.Columns.Contains(item.Key))
                //    {
                //        DataGridViewTextBoxColumn dataGridTextBoxColumn = new DataGridViewTextBoxColumn();
                //        dataGridTextBoxColumn.Name = item.Key;
                //        dataGridTextBoxColumn.HeaderText = dataGridTextBoxColumn.Name;
                //        dataGridTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                //        dataGridView1.Columns.Add(dataGridTextBoxColumn);
                //    }
                //}
                //int de = dataGridView1.Rows.Add();
                //foreach (var item in halcon.keyValuePairs1)
                //{
                //    dataGridView1[item.Key, de].Value = item.Value.ToString();
                //}
            }
            catch (Exception)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                HalconDotNet.HTuple hTupleMin = new HalconDotNet.HTuple();
                HalconDotNet.HTuple hTupleMan = new HalconDotNet.HTuple();
                HalconDotNet.HTuple hTupleCt = new HalconDotNet.HTuple();
                for (int i2 = 0; i2 < dataGridView1.ColumnCount; i2++)
                {
                    HalconDotNet.HTuple hTuple = new HalconDotNet.HTuple();
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[i2].Value == null)
                        {
                            break;
                        }
                        hTuple.Append(double.Parse(dataGridView1.Rows[i].Cells[i2].Value.ToString()));
                    }
                    hTupleMin.Append(hTuple.TupleMin());
                    hTupleMan.Append(hTuple.TupleMax());
                    hTupleCt.Append(hTuple.TupleMax() - hTuple.TupleMin());
                }
                int de = dataGridView1.Rows.Add();
                for (int i2 = 0; i2 < dataGridView1.ColumnCount; i2++)
                {
                    dataGridView1.Rows[de].Cells[i2].Value = hTupleMin.TupleSelect(i2);
                }
                de = dataGridView1.Rows.Add();
                for (int i2 = 0; i2 < dataGridView1.ColumnCount; i2++)
                {
                    dataGridView1.Rows[de].Cells[i2].Value = hTupleMan.TupleSelect(i2);
                }
                de = dataGridView1.Rows.Add();
                for (int i2 = 0; i2 < dataGridView1.ColumnCount; i2++)
                {
                    dataGridView1.Rows[de].Cells[i2].Value = hTupleCt.TupleSelect(i2);
                }

            }
            catch (Exception)
            {


            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int ddew = dataGridView1.SelectedRows.Count;

                for (int i = 0; i < ddew; i++)
                {
                    dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);

                }




            }
            catch (Exception)
            {

            }
        }

        private void 导出表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                openFileDialog.Filter = "Exlce文件|*.xls;*.xlsx";

                openFileDialog.FileName = "CPK.xls";
                DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {

                    Vision2.ErosProjcetDLL.Excel.Npoi.DataGridViewExportExcel(openFileDialog.FileName, "CPK", dataGridView1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
