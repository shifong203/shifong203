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
        private bool isThrad;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                button4.Enabled = false;
                if (isThrad)
                {
                    return;
                }
                isThrad = true;
                if (checkBox1.Checked)
                {
                    for (int j = 0; j < dataGridView2.Rows.Count; j++)
                    {
                        if (numericUpDown2.Value >= dataGridView2.Rows.Count)
                        {
                            button4.Enabled = true;
                            MessageBox.Show("已完成");
                            return;
                        }

                        HalconRun.ReadImage(dataGridView2.Rows[(int)numericUpDown2.Value].Cells[0].Value.ToString());
                        HalconRun.GetOneImageR().LiyID = (int)numericUpDown1.Value;
                        HalconRun.GetOneImageR().RunID = (int)numericUpDown1.Value;
                        HalconRun.GetOneImageR().IsSave = false;
                        HalconRun.CamImageEvent(HalconRun.GetOneImageR());
                        if (numericUpDown2.Value == 0)
                        {
                            dataGridView1.Columns.Clear();
                            DataGridViewTextBoxColumn dataGridTextBoxColumn = new DataGridViewTextBoxColumn();
                            dataGridTextBoxColumn.Name = "序号";
                            dataGridTextBoxColumn.HeaderText = dataGridTextBoxColumn.Name;
                            dataGridTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView1.Columns.Add(dataGridTextBoxColumn);
                            foreach (var item in HalconRun.GetOneImageR().GetNgOBJS().DicOnes)
                            {
                                //DataGridViewTextBoxColumn dataGridTextBoxColumn = new DataGridViewTextBoxColumn();
                                //dataGridTextBoxColumn.Name = item.Value.ComponentID;
                                //dataGridTextBoxColumn.HeaderText = dataGridTextBoxColumn.Name;
                                //dataGridTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                                //dataGridView1.Columns.Add(dataGridTextBoxColumn);
                                foreach (var itemd in item.Value.oneRObjs)
                                {
                                    for (int i = 0; i < itemd.dataMinMax.Reference_Name.Count; i++)
                                    {
                                        dataGridTextBoxColumn = new DataGridViewTextBoxColumn();
                                        dataGridTextBoxColumn.Name = item.Key + "." + itemd.dataMinMax.Reference_Name[i];
                                        dataGridTextBoxColumn.HeaderText = dataGridTextBoxColumn.Name;
                                        dataGridTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView1.Columns.Add(dataGridTextBoxColumn);
                                    }
                                }
                            }
                            dataGridView1.Rows.Clear();
                        }
                        int det = dataGridView1.Rows.Add();
                        dataGridView1.Rows[det].Cells[0].Value = System.IO.Path.GetFileNameWithoutExtension(dataGridView2.Rows[(int)numericUpDown2.Value].Cells[0].Value.ToString());
                        int cellIndex = 1;
                        foreach (var item in HalconRun.GetOneImageR().GetNgOBJS().DicOnes)
                        {
                            foreach (var itemd in item.Value.oneRObjs)
                            {
                                for (int i = 0; i < itemd.dataMinMax.ValueStrs.Count; i++)
                                {
                                    dataGridView1.Rows[det].Cells[cellIndex].Value = itemd.dataMinMax.ValueStrs[i];
                                    cellIndex++;
                                }
                            }
                        }
                        numericUpDown2.Value++;
                        if (checkBox1.Checked)
                        {
                            if (numericUpDown2.Value >= dataGridView2.Rows.Count)
                            {
                                HalconDotNet.HTuple hTupleC = new HalconDotNet.HTuple();
                                if (dataGridView1.Rows.Count == dataGridView2.Rows.Count)
                                {
                                    dataGridView1.Rows.Add(4);
                                }
                                for (int i2 = 1; i2 < dataGridView1.Columns.Count; i2++)
                                {
                                    HalconDotNet.HTuple hTuple = new HalconDotNet.HTuple();
                                    for (int i = 0; i < dataGridView1.Rows.Count - 3; i++)
                                    {
                                        if (dataGridView1.Rows[i].Cells[i2].Value != null)
                                        {
                                            if (double.TryParse(dataGridView1.Rows[i].Cells[i2].Value.ToString()
                                         , out double dvaet))
                                            {
                                                hTuple.Append(dvaet);
                                            }
                                        }
                                    }
                                    if (hTuple.Length >= 1)
                                    {
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 4].Cells[i2].Value = hTuple.TupleMin();
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 3].Cells[i2].Value = hTuple.TupleMean();
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[i2].Value = hTuple.TupleMax();
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[i2].Value = hTuple.TupleMax() - hTuple.TupleMin();
                                    }
                                }
                                dataGridView1.Rows[dataGridView1.Rows.Count - 4].Cells[0].Value = "最小值";
                                dataGridView1.Rows[dataGridView1.Rows.Count - 3].Cells[0].Value = "平均值";
                                dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[0].Value = "最大值";
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = "差";
                                button4.Enabled = true;
                                MessageBox.Show("已完成");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    HalconRun.ReadCamImage((numericUpDown1.Value).ToString(), (int)numericUpDown1.Value);
                    if (numericUpDown2.Value == 0)
                    {
                        dataGridView1.Columns.Clear();
                        DataGridViewTextBoxColumn dataGridTextBoxColumn = new DataGridViewTextBoxColumn();
                        dataGridTextBoxColumn.Name = "元件名称";
                        dataGridTextBoxColumn.HeaderText = dataGridTextBoxColumn.Name;
                        dataGridTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView1.Columns.Add(dataGridTextBoxColumn);
                        foreach (var item in HalconRun.GetOneImageR().GetNgOBJS().DicOnes)
                        {
                            foreach (var itemd in item.Value.oneRObjs)
                            {
                                for (int i = 0; i < itemd.dataMinMax.Reference_Name.Count; i++)
                                {
                                    dataGridTextBoxColumn = new DataGridViewTextBoxColumn();
                                    dataGridTextBoxColumn.Name = itemd.dataMinMax.Reference_Name[i];
                                    dataGridTextBoxColumn.HeaderText = dataGridTextBoxColumn.Name;
                                    dataGridTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dataGridView1.Columns.Add(dataGridTextBoxColumn);
                                }
                            }
                        }
                        dataGridView1.Rows.Clear();
                    }
                    int det = dataGridView1.Rows.Add();
                    foreach (var item in HalconRun.GetOneImageR().GetNgOBJS().DicOnes)
                    {
                        dataGridView1.Rows[det].Cells[0].Value = item.Key;
                        int cellIndex = 1;
                        foreach (var itemd in item.Value.oneRObjs)
                        {
                            string dataCName = itemd.dataMinMax.ComponentName;
                            string data = "";
                            for (int i = 0; i < itemd.dataMinMax.ValueStrs.Count; i++)
                            {
                                data = itemd.dataMinMax.ValueStrs[i];
                                dataGridView1.Rows[det].Cells[cellIndex].Value = data;
                                cellIndex++;
                            }
                        }
                    }
                    numericUpDown2.Value++;
                }
                isThrad = false;
                //HalconRun.CamImageEvent(numericUpDown1.Value.ToString(), null, (int)numericUpDown1.Value);
            }
            catch (Exception ex)
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择图片文件可多选";
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                dataGridView2.Rows.Clear();
                dataGridView2.Rows.Add(openFileDialog.FileNames.Length);
                for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                {
                    dataGridView2.Rows[i].Cells[0].Value = openFileDialog.FileNames[i];
                }
            }
            catch (Exception)
            {
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                openFileDialog.Filter = "Exlce文件|*.xls;*.xlsx";

                openFileDialog.FileName = DateTime.Now.ToString("yy年MM月dd日") + "CPK.xls";
                DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    ErosProjcetDLL.Excel.Npoi.DataGridViewExportExcel(openFileDialog.FileName, "CPK", dataGridView1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool isStra;

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isStra)
                {
                    return;
                }
                HalconRun.ReadImage(dataGridView2.Rows[(int)e.RowIndex].Cells[0].Value.ToString());
                HalconRun.GetOneImageR().LiyID = (int)numericUpDown1.Value;
                HalconRun.GetOneImageR().RunID = (int)numericUpDown1.Value;
                HalconRun.GetOneImageR().IsSave = false;
                HalconRun.CamImageEvent(HalconRun.GetOneImageR());
                if (numericUpDown2.Value == 0)
                {
                    dataGridView1.Columns.Clear();
                    DataGridViewTextBoxColumn dataGridTextBoxColumn = new DataGridViewTextBoxColumn();
                    dataGridTextBoxColumn.Name = "序号";
                    dataGridTextBoxColumn.HeaderText = dataGridTextBoxColumn.Name;
                    dataGridTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView1.Columns.Add(dataGridTextBoxColumn);
                    foreach (var item in HalconRun.GetOneImageR().GetNgOBJS().DicOnes)
                    {
                        //DataGridViewTextBoxColumn dataGridTextBoxColumn = new DataGridViewTextBoxColumn();
                        //dataGridTextBoxColumn.Name = item.Value.ComponentID;
                        //dataGridTextBoxColumn.HeaderText = dataGridTextBoxColumn.Name;
                        //dataGridTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        //dataGridView1.Columns.Add(dataGridTextBoxColumn);
                        foreach (var itemd in item.Value.oneRObjs)
                        {
                            for (int i = 0; i < itemd.dataMinMax.Reference_Name.Count; i++)
                            {
                                dataGridTextBoxColumn = new DataGridViewTextBoxColumn();
                                dataGridTextBoxColumn.Name = item.Key + "." + itemd.dataMinMax.Reference_Name[i];
                                dataGridTextBoxColumn.HeaderText = dataGridTextBoxColumn.Name;
                                dataGridTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                                dataGridView1.Columns.Add(dataGridTextBoxColumn);
                            }
                        }
                    }
                    dataGridView1.Rows.Clear();
                }
                int det = dataGridView1.Rows.Add();
                dataGridView1.Rows[det].Cells[0].Value = System.IO.Path.GetFileNameWithoutExtension(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());
                int cellIndex = 1;
                foreach (var item in HalconRun.GetOneImageR().GetNgOBJS().DicOnes)
                {
                    foreach (var itemd in item.Value.oneRObjs)
                    {
                        for (int i = 0; i < itemd.dataMinMax.ValueStrs.Count; i++)
                        {
                            dataGridView1.Rows[det].Cells[cellIndex].Value = itemd.dataMinMax.ValueStrs[i];
                            cellIndex++;
                        }
                    }
                }
                if (checkBox1.Checked)
                {
                    if (e.RowIndex >= dataGridView2.Rows.Count)
                    {
                        HalconDotNet.HTuple hTupleC = new HalconDotNet.HTuple();
                        if (dataGridView1.Rows.Count == dataGridView2.Rows.Count)
                        {
                            dataGridView1.Rows.Add(4);
                        }
                        for (int i2 = 1; i2 < dataGridView1.Columns.Count; i2++)
                        {
                            HalconDotNet.HTuple hTuple = new HalconDotNet.HTuple();
                            for (int i = 0; i < dataGridView1.Rows.Count - 3; i++)
                            {
                                if (dataGridView1.Rows[i].Cells[i2].Value != null)
                                {
                                    if (double.TryParse(dataGridView1.Rows[i].Cells[i2].Value.ToString()
                                 , out double dvaet))
                                    {
                                        hTuple.Append(dvaet);
                                    }
                                }
                            }
                            if (hTuple.Length >= 1)
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 4].Cells[i2].Value = hTuple.TupleMin();
                                dataGridView1.Rows[dataGridView1.Rows.Count - 3].Cells[i2].Value = hTuple.TupleMean();
                                dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[i2].Value = hTuple.TupleMax();
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[i2].Value = hTuple.TupleMax() - hTuple.TupleMin();
                            }
                        }
                        dataGridView1.Rows[dataGridView1.Rows.Count - 4].Cells[0].Value = "最小值";
                        dataGridView1.Rows[dataGridView1.Rows.Count - 3].Cells[0].Value = "平均值";
                        dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[0].Value = "最大值";
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = "差";
                        button4.Enabled = true;
                        MessageBox.Show("已完成");
                        return;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}