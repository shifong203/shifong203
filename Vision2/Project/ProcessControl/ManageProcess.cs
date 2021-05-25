using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.Project.ProcessControl
{
    public partial class ManageProcess : UserControl
    {
        public ManageProcess(ProcessUser process)
        {
            InitializeComponent();
            Proce = process;
            if (Proce.kValues == null)
            {
                Proce.kValues = new Dictionary<string, ProcessUser.MyStruct>();
            }
            Up();
        }
        /// <summary>
        /// 刷新
        /// </summary>
        void Up()
        {
            try
            {
                dataGridView1.Rows.Clear();
                while (tabControl1.TabPages.Count > 1)
                {
                    tabControl1.TabPages.RemoveAt(1);
                }
                foreach (var item in Proce.DIcSewS)
                {
                    TabPage tabPage = new TabPage();
                    tabPage.Name = tabPage.Text = item.Key;
                    tabControl1.TabPages.Add(tabPage);
                    tabPage.Controls.Add(new PrControl(item.Value.FileName));
                }
                int it = 0;
                foreach (var item in Proce.kValues)
                {
                    it = dataGridView1.Rows.Add();
                    dataGridView1.Rows[it].Cells[0].Value = item.Key;
                    dataGridView1.Rows[it].Cells[1].Value = item.Value.ValueName;
                    dataGridView1.Rows[it].Cells[2].Value = item.Value.ValueIsName;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public ProcessUser Proce;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Proce.kValues.Clear();
            try
            {

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value != null && dataGridView1.Rows[i].Cells[1].Value != null)
                    {
                        if (Proce.kValues.ContainsKey(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                        {
                            dataGridView1[0, i].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            dataGridView1[0, i].Style.BackColor = Color.White;
                            string das = "";
                            string va = "";
                            if (dataGridView1.Rows[i].Cells[2].Value != null)
                            {
                                das = dataGridView1.Rows[i].Cells[2].Value.ToString();
                            }
                            if (dataGridView1.Rows[i].Cells[1].Value != null)
                            {
                                va = dataGridView1.Rows[i].Cells[1].Value.ToString();
                            }
                            Proce.kValues.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(), new ProcessUser.MyStruct() { ValueName = va, ValueIsName = das });
                        }
                    }
                }

            }
            catch (Exception)
            {

            }

        }


        private void Tool_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;

                string[] text = dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[dataGridView1.CurrentCellAddress.X].Value.ToString().Split(',');
                text[text.Length - 1] = item.Text;
                string texts = "";
                for (int i = 0; i < text.Length; i++)
                {
                    texts += text[i];
                    if (text.Length > i + 1)
                    {
                        texts += ",";
                    }
                }
                if (dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[dataGridView1.CurrentCellAddress.X].Value.ToString().Contains("."))
                {
                    dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[dataGridView1.CurrentCellAddress.X].Value += item.Text;
                }
                else
                {
                    dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[dataGridView1.CurrentCellAddress.X].Value = texts + ".";
                }
                int X = dataGridView1.CurrentCellAddress.X;
                int Y = dataGridView1.CurrentCellAddress.Y;
                if ((Y != -1 && X != -1) && dataGridView1.Rows[Y].Cells[X] != null && dataGridView1.Rows[Y].Cells[X].Value != null)
                {
                    ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                    string[] keys = dataGridView1.Rows[Y].Cells[X].Value.ToString().Split(',');

                    foreach (var item2 in StaticCon.GetLingkNmaeValues(text[0]))
                    {
                        ToolStripMenuItem tool = new ToolStripMenuItem();
                        tool.Text = item2;
                        tool.Click += Tool_Click;
                        contextMenuStrip.Items.Add(tool);
                    }
                    Rectangle rectangle = dataGridView1.GetCellDisplayRectangle(x, y, false);
                    Rectangle rectangle2 = dataGridView1.RectangleToScreen(rectangle);
                    contextMenuStrip.Show(rectangle2.X, rectangle2.Y + this.dataGridView1.Rows[x].Height);
                }
                dataGridView1.EndEdit();
                dataGridView1.BeginEdit(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        int y;
        int x;
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1 && e.ColumnIndex != 2)
            {
                return;
            }
            try
            {
                Vision2.ErosProjcetDLL.UI.UICon.GetCursorPos(out Vision2.ErosProjcetDLL.UI.UICon.POINT pOINT);

                int X = dataGridView1.CurrentCellAddress.X;
                int Y = dataGridView1.CurrentCellAddress.Y;
                if ((Y != -1 && X != -1) && dataGridView1.Rows[Y].Cells[X] != null && dataGridView1.Rows[Y].Cells[X].Value != null)
                {
                    ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                    if (dataGridView1.Rows[Y].Cells[X].Value.ToString().Contains("."))
                    {
                        string[] keys = dataGridView1.Rows[Y].Cells[X].Value.ToString().Split('.');
                        if (keys.Length == 1)
                        {
                            if (StaticCon.GetLingkNames().Contains(keys[0]))
                            {
                                foreach (var item2 in StaticCon.GetLingkNmaeValues(keys[0]))
                                {
                                    ToolStripMenuItem tool = new ToolStripMenuItem();
                                    tool.Text = item2;
                                    tool.Click += Tool_Click;
                                    contextMenuStrip.Items.Add(tool);
                                }
                            }
                            else
                            {
                                foreach (var item in StaticCon.GetLingkNames())
                                {
                                    ToolStripMenuItem tool = new ToolStripMenuItem();
                                    tool.Text = item;
                                    tool.Click += Tool_Click;
                                    contextMenuStrip.Items.Add(tool);
                                }
                            }
                        }
                        else
                        {
                            if (StaticCon.GetLingkNames().Contains(keys[0]) || StaticCon.GetLingkNmaeValues(keys[0]).Contains(keys[1]))
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in StaticCon.GetLingkNames())
                        {
                            ToolStripMenuItem tool = new ToolStripMenuItem();
                            tool.Text = item;
                            tool.Click += Tool_Click;
                            contextMenuStrip.Items.Add(tool);
                        }
                    }
                    Rectangle rectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                    x = e.ColumnIndex;
                    y = e.RowIndex;
                    Rectangle rectangle2 = dataGridView1.RectangleToScreen(rectangle);
                    contextMenuStrip.Show(rectangle2.X, rectangle2.Y + this.dataGridView1.Rows[e.RowIndex].Height);
                    //contextMenuStrip.Show(pOINT.X, pOINT.Y);
                }
            }
            catch (Exception es)
            {
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Up();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                string sd = Microsoft.VisualBasic.Interaction.InputBox("请输入名称", "创建程序", "历史信息", 100, 100);
                if (sd != "")
                {
                    if (Proce.DIcSewS.ContainsKey(sd))
                    {
                        MessageBox.Show(sd + "已存在");
                        return;
                    }
                    Proce.DIcSewS.Add(sd, new ProcessUser.MyProcessU() { FileName = sd });
                    Up();
                }

            }
            catch (Exception)
            {
            }


        }
    }
}
