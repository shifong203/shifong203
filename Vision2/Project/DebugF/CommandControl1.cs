using System;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace Vision2.Project.DebugF
{
    public partial class CommandControl1 : System.Windows.Forms.UserControl
    {
        public CommandControl1(DebugCompiler compiler)
        {
            isChaerv = true;
            InitializeComponent();
            try
            {
                DebugC = compiler;
                propertyGrid2.SelectedObject = ProjectINI.In.UsData;
                propertyGrid1.SelectedObject = compiler.RunButton;
                //propertyGrid2.SelectedObject = compiler.SeelpData;
                Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1, 0);
                dataGridView1.Rows.Add(DebugC.DDAxis.Out.Count);

                for (int i = 0; i < DebugC.DDAxis.Out.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = DebugC.DDAxis.Int.LinkIDs[i];
                    dataGridView1.Rows[i].Cells[1].Value = DebugC.DDAxis.Int.Name[i];
                    dataGridView1.Rows[i].Cells[2].Value = DebugC.DDAxis.Out.LinkIDs[i];
                    dataGridView1.Rows[i].Cells[3].Value = DebugC.DDAxis.Out.Name[i];
                    dataGridView1.Rows[i].Cells[5].Value = DebugC.DDAxis.Out[i];
                    dataGridView1.Rows[i].Cells[4].Value = DebugC.DDAxis.Int[i];
                }
            }
            catch (Exception)
            {
            }
            isChaerv = false;
        }

        private bool isChaerv;
        private DebugCompiler DebugC;

        private void UserCommandControl1_Enter(object sender, EventArgs e)
        {
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //propertyGrid1.SelectedObject = treeView1.GetNodeAt(e.Location).Tag;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void treeView1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //propertyGrid1.SelectedObject = treeView1.SelectedNode.Tag;
            }
            catch (Exception)
            {
            }
        }

        private void 添加轴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //PLCAxis pLCAxis = new PLCAxis();
                //List<string> listn = new List<string>();
                //for (int i = 0; i < DebugC.ListAxes.Count; i++)
                //{
                //    listn.Add(DebugC.ListAxes[i].Name);
                //}
                //TreeNode treeNod = pLCAxis.NewNodeProject(listn);
                //DebugC.ListAxes.Add(pLCAxis);
                //treeNodeAx.Nodes.Add(treeNod);
            }
            catch (Exception)
            {
            }
        }

        private void 添加气缸ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void 添加轴组合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void 添加机器人ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void CommandControl1_Load(object sender, EventArgs e)
        {
            try
            {
                Thread thread = new Thread(() =>
                {
                    while (!this.IsDisposed)
                    {
                        try
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                dataGridView1.Rows[i].Cells[5].Value = DebugC.DDAxis.Out[i];
                                dataGridView1.Rows[i].Cells[4].Value = DebugC.DDAxis.Int[i];
                            }
                        }
                        catch (Exception)
                        {
                        }
                        Thread.Sleep(10);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DebugF.IO.Form1C154DIDO form1C154DIDO = new IO.Form1C154DIDO();
                form1C154DIDO.ShowDialog();
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isChaerv)
                {
                    return;
                }
                if (e.ColumnIndex == 0)
                {
                    DebugC.DDAxis.Int.LinkIDs[e.RowIndex] = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                }
                else if (e.ColumnIndex == 1)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        DebugC.DDAxis.Int.Name[e.RowIndex] = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    }
                    else
                    {
                        DebugC.DDAxis.Int.Name[e.RowIndex] = null;
                    }
                }
                else if (e.ColumnIndex == 2)
                {
                    DebugC.DDAxis.Out.LinkIDs[e.RowIndex] = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                }
                else if (e.ColumnIndex == 3)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        DebugC.DDAxis.Out.Name[e.RowIndex] = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    }
                    else
                    {
                        DebugC.DDAxis.Out.Name[e.RowIndex] = null;
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
                if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
                {
                    if (e.ColumnIndex == 4)
                    {
                        if (DebugC.DDAxis.Int[e.RowIndex])
                        {
                            DebugC.DDAxis.Int[e.RowIndex] = false;
                        }
                        else
                        {
                            DebugC.DDAxis.Int[e.RowIndex] = true;
                        }
                    }
                    if (e.ColumnIndex == 5)
                    {
                        if (DebugC.DDAxis.Out[e.RowIndex])
                        {
                            DebugC.DDAxis.Out[e.RowIndex] = false;
                        }
                        else
                        {
                            DebugC.DDAxis.Out[e.RowIndex] = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}