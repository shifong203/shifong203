using ErosSocket.ErosConLink;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ErosSocket
{
    public partial class Values : Form
    {
        //public delegate void LardData(UClass.ErosValues eros);

        private Thread thread;

        private UClass.ErosValues _ErosValue;
        private string LinkID;

        public Values()
        {
            InitializeComponent();
            toolStripComboBox1.Items.AddRange(ErosConLink.StaticCon.GetLingkNames().ToArray());
        }

        private IErosValueD Socket;

        public Values(SocketClint dic) : this()
        {
            try
            {
                interfacePlcUserControl21.UpDat(dic.PLCRun, dic.Name);
                _ErosValue = dic.KeysValues;
                this.Text = dic.Name;
                LinkID = dic.Name;
                Socket = dic;
                DoubleBufferListView.DoubleBufferedDataGirdView(this.dataGridView1, true);
                gbValueSName.Text = dic.ValusName;
                thread = new Thread(() => { CycleEvent(Socket); });
                thread.IsBackground = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string CycleEvent(IErosValueD dic)
        {
            Thread.Sleep(500);

            while (!this.IsDisposed)
            {
                try
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        try
                        {
                            if (dataGridView1.Rows[i].Cells[0].Value == null)
                            {
                                continue;
                            }
                            if (dic.KeysValues == null)
                            {
                                return "";
                            }
                            if (dic.KeysValues.DictionaryValueD != null)
                            {
                                if (dic.KeysValues.DictionaryValueD.ContainsKey(dataGridView1["Column1", i].Value.ToString()))
                                {
                                    dataGridView1["Column6", i].Value = dic.KeysValues.DictionaryValueD[dataGridView1["Column1", i].Value.ToString()].Value;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    this.Text = dic.Name + ":更新时间:" + dic._DataTime + ",更新周期:" + dic.CDataTime + "ms,错误信息:" + dic.ErrMesage;
                    if (dic is SocketClint)
                    {
                        this.Text += ",连接状态: " + ((SocketClint)dic).LinkState;
                    }
                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    Thread.Sleep(5000);
                }
            }
            return "";
        }

        private void 导出变量表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                openFileDialog.Filter = "Exlce文件|*.xls;*.xlsx";
                if (gbValueSName.Text == "")
                {
                    gbValueSName.Text = LinkID;
                }
                openFileDialog.FileName = gbValueSName.Text + ".xls";
                DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    if (gbValueSName.Text == "")
                    {
                        gbValueSName.Text = "变量表";
                    }
                    Vision2.ErosProjcetDLL.Excel.Npoi.DataGridViewExportExcel(openFileDialog.FileName, gbValueSName.Text, dataGridView1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 创建变量表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sd = Interaction.InputBox("请输入表名！", "创建变量表", "", 100, 100);
        strat:
            if (sd.Length == 0)
            {
                return;
            }
            if (StaticCon.DicErosValuess.ContainsKey(sd))
            {
                sd = Interaction.InputBox("变量表已存在，请重新输入！", "创建变量表", "", 100, 100);
                goto strat;
            }
            StaticCon.DicErosValuess.Add(sd, new UClass.ErosValues(this.Name, this.LinkID) { });
            //dataGridView1.DataSource =
            TreeNode[] treeNodes = tvListValue.Nodes.Find("Values", true);
            if (treeNodes.Count() == 1)
            {
                treeNodes[0].Nodes.Add(sd);
            }
        }

        private void Values_Load(object sender, EventArgs e)
        {
            try
            {
                this.dataGridView1.AutoGenerateColumns = false;
                System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
                ldData();
                if (Socket.KeysValues == null)
                {
                    Socket.KeysValues = new UClass.ErosValues();
                }
                if (Socket.KeysValues.DictionaryValueD != null)
                {
                    dataGridView1.Rows.Clear();

                    foreach (var item in Socket.KeysValues.DictionaryValueD)
                    {
                        int d = dataGridView1.Rows.Add();
                        dataGridView1.Rows[d].Cells[0].Value = item.Value.Name;
                        dataGridView1.Rows[d].Cells[1].Value = item.Value.District;
                        dataGridView1.Rows[d].Cells[2].Value = item.Value._Type;
                        dataGridView1.Rows[d].Cells[3].Value = item.Value.LinkID;
                        dataGridView1.Rows[d].Cells[4].Value = item.Value.AddressID;
                        dataGridView1.Rows[d].Cells[5].Value = item.Value.DecimalShift;
                        if (item.Value.WR == "")
                        {
                            item.Value.WR = "R/W";
                        }
                        dataGridView1.Rows[d].Cells[6].Value = item.Value.WR;
                        dataGridView1.Rows[d].Cells[7].Value = item.Value.Value;
                        dataGridView1.Rows[d].Cells[8].Value = item.Value.SetValueStr;
                        dataGridView1.Rows[d].Cells[9].Value = item.Value.Default;
                        dataGridView1.Rows[d].Cells[10].Value = item.Value.SnapshootValueStr;
                        dataGridView1.Rows[d].Cells[11].Value = item.Value.Annotation;
                        dataGridView1.Rows[d].Cells[12].Value = item.Value.Alarmd.AlarmType;
                        if (item.Value.Alarmd.Enabled)
                        {
                            dataGridView1.Rows[d].Cells[14].Value = item.Value.Alarmd.Text;
                        }
                        if (item.Value.Alarmd.Triggers.Count >= 1)
                        {
                            dataGridView1.Rows[d].Cells[13].Value = item.Value.Alarmd.Triggers[0].TriggerValue;
                            if (item.Value.Alarmd.Triggers[0].TriggerText != "")
                            {
                                dataGridView1.Rows[d].Cells[14].Value = item.Value.Alarmd.Triggers[0].TriggerText;
                            }
                            else
                            {
                                dataGridView1.Rows[d].Cells[14].Value = item.Value.Alarmd.Text;
                            }
                        }
                    }
                }
                else
                {
                    StaticCon.ReadDataGridViewToXML(UClass.ErosValues.constPathXML + gbValueSName.Text, dataGridView1);
                }

                if (thread != null) thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void ldData()
        {
            try
            {
                tvListValue.Nodes.Clear();
                TreeNode treeNode = new TreeNode();
                treeNode.Name = "Values";
                treeNode.Text = "《变量表》";
                TreeNode treeNode1 = new TreeNode();
                treeNode1.Name = "Types";
                treeNode1.Text = "《类型表》";
                tvListValue.Nodes.Add(treeNode);
                tvListValue.Nodes.Add(treeNode1);
                ValesType.Items.Clear();
                ValesType.Items.AddRange(UClass.GetUesrListType().ToArray());
                Column7.Items.Clear();
                Column7.Items.AddRange(StaticCon.SocketClint.Keys.ToArray());

                var TypePath = Directory.GetFiles(Application.StartupPath + "\\" + UClass.ErosType.constPathXML);
                foreach (var item in TypePath)
                {
                    treeNode1.Nodes.Add(Path.GetFileNameWithoutExtension(item));
                }
                var path = Directory.GetFiles(UClass.ErosValues.constPathXML);
                foreach (string item in path)
                {
                    if (!StaticCon.DicErosValuess.ContainsKey(Path.GetFileNameWithoutExtension(item)))
                    {
                        StaticCon.DicErosValuess.Add(Path.GetFileNameWithoutExtension(item), new UClass.ErosValues("", Path.GetFileNameWithoutExtension(item)) { });
                    }
                    else
                    {
                        StaticCon.DicErosValuess[Path.GetFileNameWithoutExtension(item)] = new UClass.ErosValues("", Path.GetFileNameWithoutExtension(item)) { };
                    }
                    treeNode.Nodes.Add(Path.GetFileNameWithoutExtension(item));
                }//从XML获取每个表里表里的变量
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsbtnSaveValues_Click(object sender, EventArgs e)
        {
            try
            {
                if (StaticCon.WrithDataGridViewToXML(UClass.ErosValues.constPathXML + gbValueSName.Text, UClass.ErosValues.constXmlElement, dataGridView1))
                {
                    MessageBox.Show("保存变量表：" + gbValueSName.Text + "成功");
                }
                if (txtTypeName.Text == "")
                {
                    return;
                }
                if (StaticCon.WrithDataGridViewToXML(UClass.ErosType.constPathXML + txtTypeName.Text, UClass.ErosType.constXmlElement, dataGridView1))
                {
                    MessageBox.Show("保存类型：" + txtTypeName.Text + "成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 打开变量表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void tvListValue_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)//判断你点的是不是右键
            {
                tvListValue.ContextMenuStrip = contextMenuStrip2;
            }
            else if (e.Button == MouseButtons.Left)
            {
            }
        }

        private void tvListValue_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                TreeView treeView = (TreeView)sender;
                if (treeView.SelectedNode != null && treeView.SelectedNode.Parent.Text.EndsWith("《变量表》"))
                {
                    tabControl1.SelectTab(0);
                    //var col0 = dataGridView1.Columns[4];
                    ////用代码升序排序
                    //dataGridView1.Sort(col0,
                    ////获取或设置列头的三角形None/Asc/Desc
                    //col0.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    gbValueSName.Text = treeView.SelectedNode.Text;
                    StaticCon.ReadDataGridViewToXML(UClass.ErosValues.constPathXML + treeView.SelectedNode.Text, dataGridView1);
                    return;
                }
                else if (treeView.SelectedNode.Parent.Text.EndsWith("《类型表》"))
                {
                    tabControl1.SelectTab(1);
                    StaticCon.ReadDataGridViewToXML(UClass.ErosType.constPathXML + treeView.SelectedNode.Text, dataGridView1);
                    txtTypeName.Text = treeView.SelectedNode.Text;
                }
            }
            catch (Exception)
            {
            }
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
        }

        private void 删除变量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> list = new List<int>();
                for (int i = 0; i < dataGridView1.SelectedCells.Count; i++)
                {
                    list.Add(dataGridView1.SelectedCells[i].RowIndex);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    dataGridView1.Rows.RemoveAt(list[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 导入变量表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Exlce文件|*xls;*.xlsx|Xml文件|*.Xml";
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName != "")
                {
                    System.Data.DataTable dataGridTable = Vision2.ErosProjcetDLL.Excel.Npoi.ReadExcelFile(openFileDialog.FileName);

                    foreach (System.Data.DataRow item in dataGridTable.Rows)
                    {
                        int ds = dataGridView1.Rows.Add();
                        if (ds > 40)
                        {
                        }
                        for (int i = 0; i < item.ItemArray.Length; i++)
                        {
                            if (i >= dataGridView1.Columns.Count)
                            {
                                DataGridViewTextBoxColumn dataGridViewColumn = new DataGridViewTextBoxColumn();
                                dataGridViewColumn.Name = dataGridViewColumn.HeaderText = "报警文本";
                                dataGridView1.Columns.Add(dataGridViewColumn);
                            }
                            dataGridView1.Rows[ds].Cells[i].Value = item.ItemArray[i];
                        }
                    }
                    dataGridView1.EndEdit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 删除变量表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        private void 创建类型表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void tvListValue_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void 地址排序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 写入值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string err = "";

                if (dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value != null
                    & dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value.ToString() != "")
                {
                    if (dataGridView1[8, dataGridView1.SelectedCells[0].RowIndex].Value == null ||
                        dataGridView1[8, dataGridView1.SelectedCells[0].RowIndex].Value.ToString() == "")
                    {
                        if (dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ValesType"].Value.ToString() == "String")
                        {
                            Socket.SetValue(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["Column1"].Value.ToString(), "", out err);
                        }
                        else
                        {
                            MessageBox.Show("请输入值");
                        }
                        return;
                    }
                    if (dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ValesType"].Value.ToString() == "Boolean")
                    {
                        if (!bool.TryParse(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["SetDataCol"].Value.ToString(), out bool result))
                        {
                            decimal.TryParse(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["SetDataCol"].Value.ToString(), out decimal dec);
                            result = Convert.ToBoolean(dec);
                        }
                        dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["SetDataCol"].Value = result.ToString();
                    }
                    Socket.SetValue(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].
                        Cells["Column1"].Value.ToString(),
                        dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["SetDataCol"].Value.ToString(), out err);

                    if (err.Contains("Err:"))
                    {
                        MessageBox.Show(err);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                if (this.dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == System.Windows.Forms.SortOrder.Ascending)
                {
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //    DicSocket[LinkID].KeysValues.DictionaryValueD[dataGridView1.SelectedRows[0].Cells["Column1"].Value.ToString()]._Type = dataGridView1.SelectedRows[0].Cells["ValesType"].Value.ToString();
        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (LinkID == null)
                {
                    return;
                }
                if (e.ColumnIndex == 2)
                {
                    if (Socket.KeysValues != null)
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value != null &&
                            Socket.KeysValues.DictionaryValueD.ContainsKey(dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString()))
                        {
                            Socket.KeysValues.DictionaryValueD[dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString()]._Type =
                                dataGridView1.Rows[e.RowIndex].Cells["ValesType"].Value.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (LinkID == null || Socket.KeysValues == null || Socket.KeysValues.DictionaryValueD == null || dataGridView1.Rows[e.RowIndex].Cells[0].Value == null)
                {
                    return;
                }

                if (Socket.KeysValues.DictionaryValueD.ContainsKey(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()))
                {
                    propertyGrid1.SelectedObject = Socket.KeysValues.DictionaryValueD[dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()].Alarmd;
                }
            }
            catch (Exception ex)
            {
                //      MessageBox.Show(ex.Message);
            }
        }

        private void 导入链接变量表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Socket.SetKeysValues(dataGridView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView1.SelectedCells.Count == 1)
                {
                    if (e.RowIndex >= 0)
                    {
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                        dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                        if (dataGridView1[2, e.RowIndex].Value != null &&
                            dataGridView1[2, e.RowIndex].Value.ToString() == "Boolean")
                        {
                            if (contextMenuStrip1.Items.ContainsKey("写入值"))
                            {
                                contextMenuStrip1.Items.RemoveAt(0);
                            }
                            if (!contextMenuStrip1.Items.ContainsKey("写入True"))
                            {
                                ToolStripItem toolStripItem = new ToolStripButton();
                                toolStripItem.Name = toolStripItem.Text = "写入true";
                                toolStripItem.Click += ToolStripItem_Click;
                                contextMenuStrip1.Items.Insert(0, toolStripItem);
                            }
                            if (!contextMenuStrip1.Items.ContainsKey("写入Fales"))
                            {
                                ToolStripItem toolStripItem = new ToolStripButton();
                                toolStripItem.Name = toolStripItem.Text = "写入Fales";
                                toolStripItem.Click += ToolStripItem_Click1;
                                contextMenuStrip1.Items.Insert(1, toolStripItem);
                            }
                        }
                        else
                        {
                            if (contextMenuStrip1.Items.ContainsKey("写入True"))
                            {
                                contextMenuStrip1.Items.RemoveByKey("写入True");
                            }
                            if (contextMenuStrip1.Items.ContainsKey("写入Fales"))
                            {
                                contextMenuStrip1.Items.RemoveByKey("写入Fales");
                            }
                            if (!contextMenuStrip1.Items.ContainsKey("写入值"))
                            {
                                ToolStripItem toolStripItem = new ToolStripButton();
                                toolStripItem.Name = toolStripItem.Text = "写入值";
                                toolStripItem.Click += 写入值ToolStripMenuItem_Click;
                                contextMenuStrip1.Items.Insert(0, toolStripItem);
                            }
                        }
                        contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                    }
                }
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
            else if (Control && e.Button == MouseButtons.Left && dataGridView1.SelectedCells.Count > 1
                 && dataGridView1.SelectedCells[0].Value != null && (dataGridView1.SelectedCells[0].ColumnIndex == 13 || dataGridView1.SelectedCells[0].ColumnIndex == 12))
            {
                Control = false;
                for (int i = 0; i < dataGridView1.SelectedCells.Count - 1; i++)
                {
                    dataGridView1.SelectedCells[i].Value = dataGridView1.SelectedCells[dataGridView1.SelectedCells.Count - 1].Value;
                }
            }
        }

        private void ToolStripItem_Click1(object sender, EventArgs e)
        {
            try
            {
                SetValue(dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value.ToString(),
                     dataGridView1[2, dataGridView1.SelectedCells[0].RowIndex].Value.ToString(), "False");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ToolStripItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetValue(dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value.ToString(),
                dataGridView1[2, dataGridView1.SelectedCells[0].RowIndex].Value.ToString(), "True");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="typeStr"></param>
        /// <param name="value"></param>
        private void SetValue(string name, string typeStr, string value)
        {
            try
            {
                string err = "";
                if (UClass.GetTypeValue(typeStr, value, out dynamic dynamic))
                {
                    Socket.SetValue(name, value, out err);

                    if (err != "")
                    {
                        MessageBox.Show(err);
                    }
                }
                else
                {
                    MessageBox.Show("输入的值类型不正确，请重新输入");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                StaticCon.DebugID = toolStripComboBox1.SelectedItem.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                if (dataGridView1["Column8", e.RowIndex].Value == null)
                {
                    dataGridView1["Column8", e.RowIndex].Value = "W/R";
                }
            }
            catch (Exception)
            {
            }
        }

        private void 导入变量表ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //  if (e.Button == MouseButtons.Left && dataGridView1.SelectedCells.Count > 1
                //&& dataGridView1.SelectedCells[0].ColumnIndex == 13 && dataGridView1.SelectedCells[0].Value != null)
                //{
                //    for (int i = 1; i < dataGridView1.SelectedCells.Count; i++)
                //    {
                //        dataGridView1.SelectedCells[i].Value = dataGridView1.SelectedCells[0].Value;
                //    }
                //}
            }
            catch (Exception)
            {
            }
        }

        private bool Control;

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Control = e.Control;
            }
            catch (Exception)
            {
            }
        }

        private void 导出变量表ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }
    }
}