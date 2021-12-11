using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HalconDotNet;
using Vision2.Project.DebugF;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class CPKForm1 : Form
    {
        public CPKForm1()
        {
            InitializeComponent();
            dataGridTextBoxColumnID.Name = "序号";
            dataGridTextBoxColumnID.HeaderText = dataGridTextBoxColumnID.Name;
            dataGridTextBoxColumnID.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        public HalconRunFile.RunProgramFile.HalconRun HalconRun;
        private bool isThrad;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                FileDat = true;
                //button4.Enabled = false;
                if (button1.Text=="停止")
                {
                    isThrad = false;
                    button1.Text = "开始";
                    return;
                }
                if (isThrad)
                {
                    button1.Text = "停止";
                    return;
                }
                //dataGridView1.Columns.Clear();
                button1.Text = "停止";
                isThrad = true;
                Thread thread = new Thread(() =>
                {

                    for (int j = 0; j < dataGridView2.Rows.Count; j++)
                    {
                        if (numericUpDown2.Value >= dataGridView2.Rows.Count)
                        {
                            //button4.Enabled = true;
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
                            //DataGridViewTextBoxColumn dataGridTextBoxColumn = new DataGridViewTextBoxColumn();
                            //dataGridTextBoxColumn.Name = "序号";
                            //dataGridTextBoxColumn.HeaderText = dataGridTextBoxColumn.Name;
                            //dataGridTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridView1.Columns.Add(dataGridTextBoxColumnID);
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
                                        DataGridViewTextBoxColumn dataGridTextBoxColumn = new DataGridViewTextBoxColumn();
                                        dataGridTextBoxColumn.Name = item.Key + "." + itemd.dataMinMax.Reference_Name[i];
                                        dataGridTextBoxColumn.HeaderText = dataGridTextBoxColumn.Name;
                                        dataGridTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                                        dataGridView1.Columns.Add(dataGridTextBoxColumn);
                                    }
                                }
                            }
                            dataGridView1.Rows.Clear();
                        }
                        //int det = dataGridView1.Rows.Add();
                        //dataGridView1.Rows[det].Cells[0].Value = System.IO.Path.GetFileNameWithoutExtension(dataGridView2.Rows[(int)numericUpDown2.Value].Cells[0].Value.ToString());
                        //int cellIndex = 1;
                        //foreach (var item in HalconRun.GetOneImageR().GetNgOBJS().DicOnes)
                        //{
                        //    foreach (var itemd in item.Value.oneRObjs)
                        //    {
                        //        for (int i = 0; i < itemd.dataMinMax.ValueStrs.Count; i++)
                        //        {
                        //            dataGridView1.Rows[det].Cells[cellIndex].Value = itemd.dataMinMax.ValueStrs[i];
                        //            cellIndex++;
                        //        }
                        //    }
                        //}
                     
                    }
                    isThrad = false;
                });
                thread.IsBackground = true;
                thread.Start();

         
           
            }
            catch (Exception ex)
            {
            }
        }

        private void CPKForm1_Load(object sender, EventArgs e)
        {
            try
            {
                HalconRun.EventDoen += HalconRun_EventDoen;
                ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
                ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView2);
                runCodeUserControl4.SetData(DebugCompiler.Instance.DDAxis.CPKCodeT);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
   
        }
        DataGridViewTextBoxColumn dataGridTextBoxColumnID = new DataGridViewTextBoxColumn();
        bool FileDat = false;
        private void HalconRun_EventDoen(vision.OneResultOBj halcon)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<OneResultOBj>(HalconRun_EventDoen), halcon);
            }
            else
            {
                try
                {


                    if (!dataGridView1.Columns.Contains(dataGridTextBoxColumnID))
                    {
                        dataGridView1.Columns.Add(dataGridTextBoxColumnID);
                    }
                    int det = dataGridView1.Rows.Add();
                    if (FileDat)
                    {
                        dataGridView1.Rows[det].Cells[0].Value = System.IO.Path.GetFileNameWithoutExtension(dataGridView2.Rows[(int)numericUpDown2.Value].Cells[0].Value.ToString());
                        numericUpDown2.Value++;
                      
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
                                //button4.Enabled = true;
                                MessageBox.Show("已完成");
                                isThrad = false;
                                return;
                            }
                        
                    }
                    else
                    {
                        dataGridView1.Rows[det].Cells[0].Value = halcon.CamNewTime;
                    }
                    foreach (var item in HalconRun.GetOneImageR().GetNgOBJS().DicOnes)
                    {
                        foreach (var itemd in item.Value.oneRObjs)
                        {
                            for (int i = 0; i < itemd.dataMinMax.Reference_Name.Count; i++)
                            {
                                DataGridViewTextBoxColumn dataGridTextBoxColumn = new DataGridViewTextBoxColumn();
                                dataGridTextBoxColumn.Name = item.Key + "." + itemd.dataMinMax.Reference_Name[i];
                                dataGridTextBoxColumn.HeaderText = dataGridTextBoxColumn.Name;
                                dataGridTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                                if (!dataGridView1.Columns.Contains(dataGridTextBoxColumn.Name))
                                {
                                    dataGridView1.Columns.Add(dataGridTextBoxColumn);
                                }
                            }
                        }
                    }
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
                }
                catch (Exception ex)
                {
                }
            }
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                HalconDotNet.HTuple hTupleMin = new HalconDotNet.HTuple();
                HalconDotNet.HTuple hTupleMan = new HalconDotNet.HTuple();
                HalconDotNet.HTuple hTupleCt = new HalconDotNet.HTuple();
                List<HalconDotNet.HTuple> hTuples = new List<HalconDotNet.HTuple>();

                for (int i2 = 1; i2 < dataGridView1.ColumnCount; i2++)
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
                    hTuples.Add(hTuple);
                }

                int de = dataGridView1.Rows.Add();
                dataGridView1.Rows[de].Cells[0].Value = "min";
                for (int i2 = 1; i2 < dataGridView1.ColumnCount; i2++)
                {
                    HTuple id=      hTuples[i2 - 1].TupleFind(hTupleMin.TupleSelect(i2 - 1));
                    dataGridView1.Rows[id.TupleInt().I].Cells[i2].Style.BackColor = Color.Yellow;
                    dataGridView1.Rows[de].Cells[i2].Value = hTupleMin.TupleSelect(i2-1);

                }
                de = dataGridView1.Rows.Add();
                dataGridView1.Rows[de].Cells[0].Value = "max";
                for (int i2 = 1; i2 < dataGridView1.ColumnCount; i2++)
                {
                    HTuple id = hTuples[i2 - 1].TupleFind(hTupleMan.TupleSelect(i2 - 1));
                    dataGridView1.Rows[id.TupleInt().I].Cells[i2].Style.BackColor = Color.Red;
                    dataGridView1.Rows[de].Cells[i2].Value = hTupleMan.TupleSelect(i2-1);
                }
                de = dataGridView1.Rows.Add();
                dataGridView1.Rows[de].Cells[0].Value = "max-min";
                for (int i2 = 1; i2 < dataGridView1.ColumnCount; i2++)
                {
                    dataGridView1.Rows[de].Cells[i2].Value = hTupleCt.TupleSelect(i2-1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 导出表格ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择图片文件可多选";
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                //dataGridView2.Rows.Clear();
                List<string> listImages = new List<string>();
                listImages.AddRange(openFileDialog.FileNames);
         
                this.dataGridView2.DataSource = (from paths in listImages select new { paths }).ToList();
        
                //dataGridView2.DataSource = openFileDialog.FileNames;
                //this.dataGridView2.DataSource = (from paths in openFileDialog.FileNames select new { paths }).ToList();
                //dataGridView2.Rows.Add(openFileDialog.FileNames.Length);

                //for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                //{
                //    dataGridView2.Rows[i].Cells[0].Value = openFileDialog.FileNames[i];
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                        //button4.Enabled = true;
                        MessageBox.Show("已完成");
                        return;
                    }
                
            }
            catch (Exception)
            {
            }
        }

        private void 全部删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            FileDat = false;
            try
            {
                if (!DebugCompiler.Instance.DDAxis.Runing)
                {
                    Thread thread = new Thread(() =>
                    {
                        button4.Enabled = false;
                        DebugCompiler.Instance.DDAxis.CPKCodeT.Run();
                        button2.PerformClick();
                        button4.Enabled = true;
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
          

            }
            catch (Exception ex)
            {

            }
       
        
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DebugCompiler.Instance.DDAxis.CPKCodeT.Stop();
        }
    }
}