using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Excel;
using Vision2.ErosProjcetDLL.Project;
using static Vision2.Project.formula.Product;

namespace Vision2.Project.formula
{
    public partial class FormulaPrForm : Form
    {
        public FormulaPrForm()
        {
            InitializeComponent();
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
            Column3.Items.Clear();
            Column3.Items.AddRange(ErosSocket.ErosConLink.UClass.GetTypeList().ToArray());
            Column3.Items.AddRange("array[]");
        }

        private int y;
        private int x;

        /// <summary>
        /// 更改中发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {///自动叠加数组
            try
            {
                if (e.ColumnIndex == 2)
                {
                    try
                    {
                        Vision2.ErosProjcetDLL.UI.UICon.GetCursorPos(out Vision2.ErosProjcetDLL.UI.UICon.POINT pOINT);
                        int X = dataGridView1.CurrentCellAddress.X;
                        int Y = dataGridView1.CurrentCellAddress.Y;
                        string lingkn = "";
                        if ((Y != -1 && X != -1) && dataGridView1.Rows[Y].Cells[X] != null && dataGridView1.Rows[Y].Cells[X].Value != null)
                        {
                            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                            if (dataGridView1.Rows[Y].Cells[X].Value.ToString().StartsWith("array"))
                            {
                                string dsd = dataGridView1.Rows[Y].Cells[X].Value.ToString().Remove(0, 5);

                                for (int i = 5; i < dataGridView1.Columns.Count; i++)
                                {
                                    if (dataGridView1[dataGridView1.Columns[i].HeaderText, Y].Value != null)
                                    {
                                        lingkn = dataGridView1[dataGridView1.Columns[i].HeaderText, Y].Value.ToString();
                                    }
                                }
                                if (lingkn.Contains('.'))
                                {
                                    int lingkIDav = int.Parse(lingkn.Split('.')[1]);
                                    lingkn = lingkn.Split('.')[0];
                                    dsd = dsd.Trim('[', ']');
                                    if (int.TryParse(dsd, out int ids))
                                    {
                                        int itd = Y + 1;
                                        for (int i = 0; i < ids; i++)
                                        {
                                            if (itd + 1 >= dataGridView1.Rows.Count)
                                            {
                                                itd = dataGridView1.Rows.Add();
                                            }
                                            dataGridView1.Rows[itd].Cells[0].Value = dataGridView1.Rows[Y].Cells[0].Value.ToString() + "[" + i + "]";
                                            ErosSocket.ErosConLink.IErosLinkNet erosLinkNet = ErosSocket.ErosConLink.StaticCon.GetSocketClint(links[0]);
                                            dataGridView1.Rows[itd].Cells[2].Value = dataGridView1.Rows[Y].Cells[1].Value.ToString();
                                            dataGridView1.Rows[itd].Cells[3].Value = lingkn + "." + (lingkIDav + i * 4);
                                            if (dataGridView1.Rows[Y].Cells[1].Value != null)
                                            {
                                                Type type = ErosSocket.ErosConLink.UClass.GetTypeByString(dataGridView1.Rows[Y].Cells[1].Value.ToString());
                                                dataGridView1.Rows[itd].Cells[1].Value = Activator.CreateInstance(type).ToString();
                                            }
                                            else
                                            {
                                                dataGridView1.Rows[itd].Cells[1].Value = 0;
                                            }
                                            itd++;
                                        }
                                    }
                                }
                                else
                                {
                                    int lingkIDav = int.Parse(lingkn.Remove(0, 1));

                                    dsd = dsd.Trim('[', ']');
                                    if (int.TryParse(dsd, out int ids))
                                    {
                                        int itd = Y + 1;
                                        for (int i = 0; i < ids; i++)
                                        {
                                            if (itd + 1 >= dataGridView1.Rows.Count)
                                            {
                                                itd = dataGridView1.Rows.Add();
                                            }
                                            dataGridView1.Rows[itd].Cells[0].Value = dataGridView1.Rows[Y].Cells[0].Value.ToString() + "[" + i + "]";
                                            ErosSocket.ErosConLink.IErosLinkNet erosLinkNet = ErosSocket.ErosConLink.StaticCon.GetSocketClint(links[0]);
                                            dataGridView1.Rows[itd].Cells[2].Value = dataGridView1.Rows[Y].Cells[1].Value.ToString();
                                            dataGridView1.Rows[itd].Cells[5].Value = lingkn.Remove(1, lingkn.Length - 1) + (lingkIDav + i);
                                            if (dataGridView1.Rows[Y].Cells[1].Value != null)
                                            {
                                                Type type = ErosSocket.ErosConLink.UClass.GetTypeByString(dataGridView1.Rows[Y].Cells[1].Value.ToString());
                                                dataGridView1.Rows[itd].Cells[1].Value = Activator.CreateInstance(type).ToString();
                                            }
                                            else
                                            {
                                                dataGridView1.Rows[itd].Cells[1].Value = 0;
                                            }
                                            itd++;
                                        }
                                    }
                                }
                            }
                            //contextMenuStrip.Show(pOINT.X, pOINT.Y);
                        }
                    }
                    catch (Exception es)
                    {
                    }
                }
                if (e.ColumnIndex >= 2)
                {
                    try
                    {
                        Vision2.ErosProjcetDLL.UI.UICon.GetCursorPos(out Vision2.ErosProjcetDLL.UI.UICon.POINT pOINT);
                        int X = dataGridView1.CurrentCellAddress.X;
                        int Y = dataGridView1.CurrentCellAddress.Y;
                        if ((Y != -1 && X != -1) && dataGridView1.Rows[Y].Cells[X] != null && dataGridView1.Rows[Y].Cells[X].Value != null)
                        {
                            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

                            string keys = dataGridView1.Columns[e.ColumnIndex].HeaderText;
                            if (dataGridView1.Rows[Y].Cells[X].Value.ToString().Length <= 3)
                            {
                                if (StaticCon.GetLingkNames().Contains(keys))
                                {
                                    foreach (var item2 in StaticCon.GetLingkNmaeValues(keys))
                                    {
                                        ToolStripMenuItem tool = new ToolStripMenuItem();
                                        tool.Text = item2;
                                        tool.Click += Tool_Click;
                                        contextMenuStrip.Items.Add(tool);
                                    }
                                    Rectangle rectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                                    x = e.ColumnIndex;
                                    y = e.RowIndex;
                                    Rectangle rectangle2 = dataGridView1.RectangleToScreen(rectangle);
                                    contextMenuStrip.Show(rectangle2.X, rectangle2.Y + this.dataGridView1.Rows[e.RowIndex].Height);
                                }
                            }
                        }
                    }
                    catch (Exception es)
                    {
                    }
                }
                if (e.ColumnIndex < 4)
                {
                    if (e.ColumnIndex == 3)
                    {
                        string[] data = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString().Split(',');
                        dynamic min = 1;
                        if (data[0] != "")
                        {
                            UClass.GetTypeValue(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(),
                         data[0], out min);
                        }
                        dynamic max = 4;
                        if (data[0] != "")
                        {
                            UClass.GetTypeValue(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(),
                         data[1], out max);
                        }
                        dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
                        if (max == null || min == null || max < min)
                        {
                            dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 点击下拉写入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[dataGridView1.CurrentCellAddress.X].Value = texts;
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

                //dataGridView1.RefreshEdit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedCells[0].ColumnIndex > 5)
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

        public List<string> links;

        public void UPData()
        {
            try
            {
                foreach (var item in Product.GetThisDic())
                {
                    listBox1.Items.Add(item.Value.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormulaPrForm_Load(object sender, EventArgs e)
        {
            UPData();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Text = listBox1.SelectedItem.ToString();
                try
                {
                    if (listBox1.SelectedItem == null)
                    {
                        return;
                    }
                    this.AddDataGridConmlus(Product.GetListLinkNames.ToArray());
                    this.Updatas(listBox1.SelectedItem.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = ProjectINI.ProjectPathRun + "\\产品配方\\";
            openFileDialog.Filter = "Excel文件|*.xls";
            DialogResult dialog = openFileDialog.ShowDialog();
            try
            {
                if (dialog == DialogResult.OK)
                {
                    System.Data.DataTable dataTable2 = Npoi.ReadExcelFile(openFileDialog.FileName, "参数信息");
                    if (dataTable2 == null)
                    {
                        MessageBox.Show("参数信息不存在;" + Environment.NewLine);
                    }
                    else
                    {
                        Product.ParameterDic product = new Product.ParameterDic();

                        foreach (System.Data.DataRow item1 in dataTable2.Rows)
                        {
                            string[] st = item1[3].ToString().Split(',');

                            product.SetKet(item1[0].ToString(), item1[1].ToString(), item1[2].ToString(), st[0], st[1]);
                            for (int i = 3; i < item1.ItemArray.Length; i++)
                            {
                                if (!product.ParameterMap.ContainsKey(item1.Table.Columns[i].ColumnName))
                                {
                                    product.ParameterMap.Add(item1.Table.Columns[i].ColumnName, new Dictionary<string, string>());
                                }
                                if (product.ParameterMap[item1.Table.Columns[i].ColumnName].ContainsKey(item1[0].ToString()))
                                {
                                    product.ParameterMap[item1.Table.Columns[i].ColumnName][item1[0].ToString()] = item1[i].ToString();
                                }
                                else
                                {
                                    product.ParameterMap[item1.Table.Columns[i].ColumnName].Add(item1[0].ToString(), item1[i].ToString());
                                }
                            }
                        }
                        //this.Parameters = product;
                        Product.GetProduct().Parameter_Dic.GetParameters().Clear();
                        Product.GetProduct().Parameter_Dic.ParameterMap.Clear();

                        foreach (var item in product.GetParameters())
                        {
                            Product.GetProduct().Parameter_Dic.GetParameters().Add(item.Key, item.Value);
                        }
                        foreach (var item in product.ParameterMap)
                        {
                            Product.GetProduct().Parameter_Dic.ParameterMap.Add(item.Key, item.Value);
                        }

                        Updatas();
                        MessageBox.Show("导入成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败:" + ex.Message);
            }
        }

        public void Updatas()
        {
            Updatas(groupBox1.Text);
        }

        public void Updatas(string name)
        {
            try
            {
                if (name != null)
                {
                    groupBox1.Text = name;
                }

                int i = 0;

                dataGridView1.Rows.Clear();
                foreach (var item in Product.GetParameters())
                {
                    i = dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = item.Key;
                    dataGridView1.Rows[i].Cells[1].Value = item.Value.ValueStr;
                    if (Product.GetParameters().ContainsKey(item.Key))
                    {
                        dataGridView1.Rows[i].Cells[2].Value = Product.GetParameters()[item.Key].TypeStr;
                    }
                    dataGridView1.Rows[i].Cells[3].Value = item.Value.minValue + "," + item.Value.MaxValue;
                    dataGridView1.Rows[i].Cells[4].Value = item.Value.Pst;
                    for (int it = 4; it < dataGridView1.Columns.Count; it++)
                    {
                        if (Product.GetProduct().Parameter_Dic.ParameterMap.ContainsKey(dataGridView1.Columns[it].HeaderText))
                        {
                            if (Product.GetProduct().Parameter_Dic.ParameterMap[dataGridView1.Columns[it].HeaderText].ContainsKey(item.Key))
                            {
                                dataGridView1.Rows[i].Cells[it].Value = Product.GetProduct().Parameter_Dic.ParameterMap[dataGridView1.Columns[it].HeaderText][item.Key];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 添加链接列到数据表
        /// </summary>
        /// <param name="linkNames"></param>
        public void AddDataGridConmlus(string[] linkNames)
        {
            links = linkNames.ToList();
            while (this.dataGridView1.Columns.Count > 5)
            {
                this.dataGridView1.Columns.RemoveAt(5);
            }
            tool1.DropDownItems.Clear();
            foreach (var item in linkNames)
            {
                int cIndex = this.dataGridView1.Columns.Add(item, item);
                ToolStripButton toolStripButton = new ToolStripButton();
                toolStripButton.Name = toolStripButton.Text = item;
                tool1.DropDownItems.Add(toolStripButton);
                toolStripButton.Click += ToolStripButton_Click;
                void ToolStripButton_Click(object sender, EventArgs e)
                {
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        this.Invoke(new Action(() =>
                        {
                            try
                            {
                                if (StaticCon.SocketClint.ContainsKey(item))
                                {
                                    ErosSocket.ErosConLink.IErosLinkNet erosLinkNet = StaticCon.SocketClint[item];
                                    if (!erosLinkNet.IsConn)
                                    {
                                        MessageBox.Show(erosLinkNet.Name + ":未连接成功");
                                        return;
                                    }
                                    if (erosLinkNet.Split_Mode == ErosSocket.ErosConLink.SocketClint.SplitMode.Array)
                                    {
                                        if (erosLinkNet.Send(erosLinkNet.Identifying, out string data))
                                        {
                                        }
                                    }
                                    else
                                    {
                                        if (erosLinkNet.KeysValues.DictionaryValueD == null)
                                        {
                                            MessageBox.Show(item + "变量表未初始化");
                                            return;
                                        }
                                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                                        {
                                            if (dataGridView1.Rows[i].Cells[cIndex].Value != null &&
                                            dataGridView1.Rows[i].Cells[cIndex].Value.ToString() != ""
                                            && dataGridView1.Rows[i].Cells[2].Value != null)
                                            {
                                                string vaName = dataGridView1.Rows[i].Cells[cIndex].Value.ToString();
                                                if (UClass.GetTypeList().Contains(dataGridView1.Rows[i].Cells[2].Value.ToString()))
                                                {
                                                    erosLinkNet.GetIDValue(vaName, dataGridView1.Rows[i].Cells[2].Value.ToString(), out dynamic value);
                                                    if (value != null)
                                                    {
                                                        dataGridView1.Rows[i].Cells[1].Value = value.ToString();
                                                        dataGridView1[1, i].Style.BackColor = Color.Turquoise;
                                                    }
                                                    else
                                                    {
                                                        dataGridView1[1, i].Style.BackColor = Color.Red;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(string.Format("未创建{0}", item));
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }));
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, StructTypeValue> item = new Dictionary<string, StructTypeValue>();
                Product.GetProduct().Parameter_Dic.ParameterMap.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value != null && dataGridView1.Rows[i].Cells[1].Value != null)
                        {
                            dataGridView1[0, i].Style.BackColor = Color.White;
                            dataGridView1[1, i].Style.BackColor = Color.White;
                            if (item.ContainsKey(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                            {
                                string key = dataGridView1.Rows[i].Cells[0].Value.ToString();
                                for (int idx = 0; idx < dataGridView1.Rows.Count; idx++)
                                {
                                    if (dataGridView1.Rows[idx].Cells[0].Value != null && key == dataGridView1.Rows[idx].Cells[0].Value.ToString())
                                    {
                                        dataGridView1[0, idx].Style.BackColor = Color.Red;
                                    }
                                }
                                MessageBox.Show("错误[" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "]名称已存在！");
                            }
                            else
                            {
                                item.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(), new StructTypeValue()
                                {
                                    ValueStr = dataGridView1.Rows[i].Cells[1].Value.ToString(),
                                });
                                if (dataGridView1.Rows[i].Cells[2].Value != null)
                                {
                                    item[dataGridView1.Rows[i].Cells[0].Value.ToString()].TypeStr = dataGridView1.Rows[i].Cells[2].Value.ToString();
                                }
                                if (dataGridView1.Rows[i].Cells[4].Value != null)
                                {
                                    item[dataGridView1.Rows[i].Cells[0].Value.ToString()].Pst = dataGridView1.Rows[i].Cells[4].Value.ToString();
                                }
                            }
                            if (UClass.GetUesrListType().Contains(item[dataGridView1.Rows[i].Cells[0].Value.ToString()].TypeStr))
                            {
                                if (!UClass.GetTypeValue(item[dataGridView1.Rows[i].Cells[0].Value.ToString()].TypeStr,
                                item[dataGridView1.Rows[i].Cells[0].Value.ToString()].ValueStr, out dynamic value))
                                {
                                    dataGridView1[0, i].Style.BackColor = Color.Red;
                                }
                                else
                                {
                                    if (item[dataGridView1.Rows[i].Cells[0].Value.ToString()].minValue != null &&
                                        item[dataGridView1.Rows[i].Cells[0].Value.ToString()].minValue != "")
                                    {
                                        UClass.GetTypeValue(item[dataGridView1.Rows[i].Cells[0].Value.ToString()].TypeStr,
                                        item[dataGridView1.Rows[i].Cells[0].Value.ToString()].minValue, out dynamic min);
                                        if (value < min)
                                        {
                                            dataGridView1[1, i].Style.BackColor = Color.Red;
                                        }
                                    }
                                    if (item[dataGridView1.Rows[i].Cells[0].Value.ToString()].MaxValue != null &&
                                        item[dataGridView1.Rows[i].Cells[0].Value.ToString()].MaxValue != "")
                                    {
                                        UClass.GetTypeValue(item[dataGridView1.Rows[i].Cells[0].Value.ToString()].TypeStr,
                                                item[dataGridView1.Rows[i].Cells[0].Value.ToString()].minValue, out dynamic max);
                                        if (value > max)
                                        {
                                            dataGridView1[1, i].Style.BackColor = Color.Red;
                                        }
                                    }
                                }
                            }
                            int itt = 5;
                            if (dataGridView1.Rows[i].Cells[3].Value != null && dataGridView1.Rows[i].Cells[3].Value.ToString() != ",")
                            {
                                string[] data = dataGridView1.Rows[i].Cells[3].Value.ToString().Split(',');
                                item[dataGridView1.Rows[i].Cells[0].Value.ToString()].minValue = data[0];
                                item[dataGridView1.Rows[i].Cells[0].Value.ToString()].MaxValue = data[1];
                            }
                            for (int it = itt; it < dataGridView1.Columns.Count; it++)
                            {
                                if (!Product.GetProduct().Parameter_Dic.ParameterMap.ContainsKey(dataGridView1.Columns[it].HeaderText))
                                {
                                    Product.GetProduct().Parameter_Dic.ParameterMap.Add(dataGridView1.Columns[it].HeaderText, new Dictionary<string, string>());
                                }
                                if (dataGridView1.Rows[i].Cells[it].Value == null)
                                {
                                    continue;
                                }
                                string valuet = dataGridView1.Rows[i].Cells[it].Value.ToString();
                                string key = dataGridView1.Rows[i].Cells[0].Value.ToString();
                                if (Product.GetProduct().Parameter_Dic.ParameterMap[dataGridView1.Columns[it].HeaderText].ContainsKey(key))
                                {
                                    Product.GetProduct().Parameter_Dic.ParameterMap[dataGridView1.Columns[it].HeaderText][key] = valuet;
                                }
                                else
                                {
                                    Product.GetProduct().Parameter_Dic.ParameterMap[dataGridView1.Columns[it].HeaderText].Add(key, valuet);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
                Product.GetParameters().Clear();
                foreach (var item2 in item)
                {
                    Product.GetParameters().Add(item2.Key, item2.Value);
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
        }

        private void tool1_MouseMove(object sender, MouseEventArgs e)
        {
            tool1.ShowDropDown();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                Product.SaveDicPrExcel(ProjectINI.ProjectPathRun + "\\产品配方\\产品配方管理\\产品文件.xls");
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
                int d = dataGridView1.SelectedCells.Count;
                for (int i = 0; i < d; i++)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
        }

        private void 插入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Add(dataGridView1.SelectedRows[0].Index);
            }
            catch (Exception)
            {
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
        }
    }
}