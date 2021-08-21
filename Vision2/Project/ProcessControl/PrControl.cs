using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using static Vision2.Project.ProcessControl.ProcessUser;

namespace Vision2.Project.ProcessControl
{
    public partial class PrControl : UserControl
    {
        private string nameT;

        public PrControl(string name)
        {
            try
            {
                InitializeComponent();
                Column3.Items.AddRange(ErosSocket.ErosConLink.UClass.GetTypeList().ToArray());
                nameT = name;
                toolStripTextBox1.Text = name;

                if (Instancen.DIcSewS.ContainsKey(name))
                {
                    int d = 0;
                    foreach (var item in Instancen.DIcSewS[name].keyValuePa)
                    {
                        d = dataGridView1.Rows.Add();
                        dataGridView1.Rows[d].Cells[0].Value = item.Key;
                        dataGridView1.Rows[d].Cells[1].Value = item.Value.TypeStr;
                        dataGridView1.Rows[d].Cells[2].Value = item.Value.ValueStr;
                    }
                    toolStripTextBox2.Text = Instancen.DIcSewS[name].ValueIsName;
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
                    contextMenuStrip.Show(rectangle2.X, rectangle2.Y + this.dataGridView1.Rows[y].Height);
                }
                dataGridView1.EndEdit();
                dataGridView1.BeginEdit(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int y;
        private int x;

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 2)
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(ProjectINI.ProjectPathRun + "\\历史数据");
                System.Diagnostics.Process.Start(ProjectINI.ProjectPathRun + "\\历史数据");
            }
            catch (Exception)
            {
            }
        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                int err = 0;
                Dictionary<string, formula.Product.StructTypeValue> item = new Dictionary<string, formula.Product.StructTypeValue>();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].InheritedStyle.BackColor = Color.White;
                    if (dataGridView1.Rows[i].Cells[0].Value != null && dataGridView1.Rows[i].Cells[1].Value != null)
                    {
                        if (!item.ContainsKey(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                        {
                            item.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(), new formula.Product.StructTypeValue()
                            {
                                TypeStr = dataGridView1.Rows[i].Cells[1].Value.ToString(),
                                ValueStr = dataGridView1.Rows[i].Cells[2].Value.ToString()
                            });
                        }
                        else
                        {
                            err++;
                            dataGridView1.Rows[i].InheritedStyle.BackColor = Color.Red;
                        }
                    }
                }
                if (err != 0)
                {
                    MessageBox.Show("名称重复");
                    return;
                }
                if (nameT == toolStripTextBox1.Text)
                {
                    Instancen.DIcSewS.Remove(nameT);
                    Instancen.DIcSewS.Add(toolStripTextBox1.Text, new MyProcessU()
                    {
                        FileName = toolStripTextBox1.Text,
                        ValueIsName = toolStripTextBox2.Text,
                        keyValuePa = item
                    });
                    return;
                }
                if (nameT != toolStripTextBox1.Text && Instancen.DIcSewS.ContainsKey(toolStripTextBox1.Text))
                {
                    MessageBox.Show(toolStripTextBox1.Text + "名称已存在");
                    return;
                }
                if (Instancen.DIcSewS.ContainsKey(nameT))
                {
                    if (nameT != toolStripTextBox1.Text)
                    {
                        Instancen.DIcSewS.Remove(nameT);
                        Instancen.DIcSewS.Add(toolStripTextBox1.Text, new MyProcessU()
                        {
                            FileName = toolStripTextBox1.Text,
                            ValueIsName = toolStripTextBox2.Text,
                            keyValuePa = item
                        });
                    }
                }
                else
                {
                    Instancen.DIcSewS.Add(toolStripTextBox1.Text, new MyProcessU()
                    {
                        FileName = toolStripTextBox1.Text,
                        ValueIsName = toolStripTextBox2.Text,
                        keyValuePa = item
                    });
                }
            }
            catch (Exception)
            {
            }
        }
    }
}