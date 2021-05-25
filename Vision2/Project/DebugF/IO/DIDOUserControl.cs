using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using ErosSocket.DebugPLC.DIDO;

namespace NokidaE.Project.DebugF.IO
{
    public partial class DIDOUserControl : UserControl
    {
        public DIDOUserControl()
        {
            InitializeComponent();
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
       
        }
        public void setDODI(IDIDO dIDO )
        {
            dID = dIDO;
            Run(dIDO.DO.Length);
        }
        bool isMoe =true;
        IDIDO dID;
        public void Run(int index)
        {
            dataGridView1.Rows.Add(index);
            int a;
            List<bool> listBool_Output = new List<bool>();
            List<bool> listBool_Input = new List<bool>();
            string str;
            isMoe = true;
            for (int i = 0; i < index; i++)
            {
                //str= (i / 8).ToString() + "_" + (i % 8).ToString();
                dataGridView1.Rows[i].Cells[0].Value = "DO_" + i;
                dataGridView1.Rows[i].Cells[3].Value = "DI_" + i;
            }

            for (int i = 0; i <= dID.Di.Length - 1; i++)
            {
                dataGridView1.Rows[i].Cells[1].Value = dID.DO_Name[i].ToString();
                dataGridView1.Rows[i].Cells[4].Value = dID.DI_Name[i].ToString();
            }
            isMoe = false;
            Task.Run(new Action(() => {
                while (!this.IsDisposed)
                {
                    try
                    {
                        for (int i = 0; i <= dID.Di.Length - 1; i++)
                        {
                            dataGridView1.Rows[i].Cells[2].Value = dID.DO[i].ToString();
                            dataGridView1.Rows[i].Cells[5].Value = dID.Di[i].ToString();
                            dataGridView1.Rows[i].Cells[5].Style.ForeColor = Color.Black;
                            if (dataGridView1.Rows[i].Cells[5].Value.ToString() == "False")
                            {
                                dataGridView1.Rows[i].Cells[5].Style.BackColor = Color.Gray;
                            }
                            else
                            {
                                dataGridView1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                            }
                        }
                        Thread.Sleep(100);
                    }
                    catch (Exception)
                    {
                    }
                }
            } ));
        }

               
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex==2)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() == "False")
                    {
                        dID.WritDO( e.RowIndex, true);
                    }
                    else
                    {
                        dID.WritDO(e.RowIndex, false);
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

                if (e.ColumnIndex==1)
                {
                    dID.DO_Name[e.RowIndex] = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                }
                else if ( e.ColumnIndex == 4)
                {
                    dID.DI_Name[e.RowIndex] = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                }

            }
            catch (Exception)
            {

            }
        }
    }
}
