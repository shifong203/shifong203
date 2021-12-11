using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.Project.DebugF;
using Vision2.Project.DebugF.IO;

namespace ErosSocket.DebugPLC.DIDO
{
    public partial class DIDOUserControl : UserControl
    {
        public DIDOUserControl()
        {
            InitializeComponent();
            Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1, 0);
            Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.DoubleBufferedDataGirdView(dataGridView1, true);
        }

        public void setDODI(IDIDO dIDO)
        {
            dID = dIDO;
            if (dID != null)
            {
                Run(dIDO.Int.Count);
            }
        }

        private bool isMoe = true;
        private IDIDO dID;

        public void Run(int index)
        {
            dataGridView1.Rows.Add(index);
            int a;
            List<bool> listBool_Output = new List<bool>();
            List<bool> listBool_Input = new List<bool>();

            isMoe = true;
            for (int i = 0; i <= dID.Out.Count - 1; i++)
            {
                if (dID.Out.Name[i] != null)
                {
                    dataGridView1.Rows[i].Cells[0].Value = dID.Out.Name[i];
                }

                dataGridView1.Rows[i].Cells[2].Value = dID.Int.Name[i];
            }
            isMoe = false;
            Task.Run(new Action(() =>
            {
                while (!this.IsDisposed)
                {
                    try
                    {
                        for (int i = 0; i <= dID.Int.Count - 1; i++)
                        {
                            dataGridView1.Rows[i].Cells[1].Value = dID.Out[i].ToString();
                            if (dataGridView1.Rows[i].Cells[1].Value.ToString() == "False")
                            {
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Gray;
                            }
                            else
                            {
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Green;
                            }
                            dataGridView1.Rows[i].Cells[3].Value = dID.Int[i].ToString();
                            dataGridView1.Rows[i].Cells[3].Style.ForeColor = Color.Black;
                            if (dataGridView1.Rows[i].Cells[3].Value.ToString() == "False")
                            {
                                dataGridView1.Rows[i].Cells[2].Style.BackColor = Color.Gray;
                            }
                            else
                            {
                                dataGridView1.Rows[i].Cells[2].Style.BackColor = Color.Green;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    Thread.Sleep(100);
                }
            }));
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() == "False")
                    {
                        dID.WritDO(e.RowIndex, true);
                    }
                    else
                    {
                        dID.WritDO(e.RowIndex, false);
                    }
                }
                else if (e.ColumnIndex == 3)
                {
                    if (DODIAxis.Debug)
                    {
                        if (DebugCompiler.Instance.DDAxis.Int[e.RowIndex])
                        {
                            DebugCompiler.Instance.DDAxis.Int[e.RowIndex] = false;
                        }
                        else
                        {
                            DebugCompiler.Instance.DDAxis.Int[e.RowIndex] = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isMoe)
            {
                return;
            }

            try
            {
                //if (e.ColumnIndex == 0)
                //{
                //    if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                //    {
                //        dID.Out.Name[e.RowIndex] = "";
                //    }
                //    else
                //    {
                //        dID.Out.Name[e.RowIndex] = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                //    }

                //}
                //else if (e.ColumnIndex == 2)
                //{
                //    if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                //    {
                //        dID.Int.Name[e.RowIndex] = "";
                //    }
                //    else
                //    {
                //        dID.Int.Name[e.RowIndex] = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                //    }
                //}
            }
            catch (Exception)
            {
            }
        }
    }
}