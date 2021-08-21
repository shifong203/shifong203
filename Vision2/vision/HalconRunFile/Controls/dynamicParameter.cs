using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class DynamicParameter : UserControl
    {
        public DynamicParameter()
        {
            InitializeComponent();
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            //dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
        }

        private bool isEnb;

        /// <summary>
        ///
        /// </summary>
        public void SetUpData(DicHtuple dicHtuple)
        {
            isEnb = true;
            dHtuple = dicHtuple;
            dicHtuple.UpData(this.dataGridView1);
            isEnb = false;
        }

        private DicHtuple dHtuple;

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isEnb)
                {
                    return;
                }
                if (e.ColumnIndex > 0)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null && dataGridView1.Rows[e.RowIndex].Cells[3].Value != null
                    && dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() != "")
                    {
                        string key = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                        if (!dHtuple.DirectoryHTup.ContainsKey(key))
                        {
                            dHtuple.DirectoryHTup.Add(key, new DicHtuple.HtupleEx());
                        }
                        if (e.ColumnIndex == 1)
                        {
                            dHtuple.DirectoryHTup[key].DicIsSave =
                              Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[1].EditedFormattedValue);
                        }
                        else if (e.ColumnIndex == 2)
                        {
                            dHtuple.DirectoryHTup[key].DicIsSaveData =
                                    Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[2].EditedFormattedValue);
                        }
                        else if (e.ColumnIndex == 3)
                        {
                            HalconDotNet.HTuple hTuple = new HalconDotNet.HTuple();

                            string dss = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString().Trim('\"').Trim(' ');
                            if (dss.Contains("["))
                            {
                                string[] datas = dss.Trim('[', ']').Split(',');
                                for (int i2 = 0; i2 < datas.Length; i2++)
                                {
                                    if (double.TryParse(datas[i2], out double doubes))
                                    {
                                        hTuple.Append(doubes);
                                    }
                                    else
                                    {
                                        hTuple.Append(datas[i2].Replace('"', ' ').Trim());
                                    }
                                }
                            }
                            else
                            {
                                if (double.TryParse(dss, out double deo))
                                {
                                    hTuple = deo;
                                }
                                else
                                {
                                    hTuple = dss;
                                }
                            }
                            dHtuple.DirectoryHTup[key].DirectoryHTuple = hTuple;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedCells[0].ColumnIndex > 4)
                {
                    if (dataGridView1.IsCurrentCellDirty)
                    {
                        dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}