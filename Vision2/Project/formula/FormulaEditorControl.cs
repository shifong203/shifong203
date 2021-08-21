using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Vision2.ErosProjcetDLL.Excel;
using Vision2.ErosProjcetDLL.Project;
using static Vision2.Project.formula.Product;

namespace Vision2.Project.formula
{
    public partial class FormulaEditorControl : UserControl
    {
        public FormulaEditorControl()
        {
            InitializeComponent();
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
        }

        private string Formname;
        private int y;
        private int x;
        private bool upT;

        /// <summary>
        /// 更改中发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {///自动叠加数组
            try
            {
                if (!upT)
                {
                    return;
                }
                if (e.ColumnIndex == 2)
                {
                    try
                    {
                        //Vision2.ErosProjcetDLL.UI.UICon.GetCursorPos(out Vision2.ErosProjcetDLL.UI.UICon.POINT pOINT);
                        int X = dataGridView1.CurrentCellAddress.X;
                        int Y = dataGridView1.CurrentCellAddress.Y;
                        string lingkn = "";
                        if ((Y != -1 && X != -1) && dataGridView1.Rows[Y].Cells[X] != null && dataGridView1.Rows[Y].Cells[X].Value != null)
                        {
                            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                            if (dataGridView1.Rows[Y].Cells[X].Value.ToString().StartsWith("array"))
                            {
                                string dsd = dataGridView1.Rows[Y].Cells[X].Value.ToString().Remove(0, 5);

                                for (int i = 3; i < dataGridView1.Columns.Count; i++)
                                {
                                    if (dataGridView1[dataGridView1.Columns[i].HeaderText, Y].Value != null)
                                    {
                                        lingkn = dataGridView1[dataGridView1.Columns[i].HeaderText, Y].Value.ToString();
                                    }
                                }

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
                            //contextMenuStrip.Show(pOINT.X, pOINT.Y);
                        }
                    }
                    catch (Exception es)
                    {
                    }
                }
                else if (e.ColumnIndex == 0)
                {
                    try
                    {
                        int colum = dataGridView1.SelectedCells[0].ColumnIndex;
                        int rowt = dataGridView1.SelectedCells[0].RowIndex;
                        //Vision2.ErosProjcetDLL.UI.UICon.GetCursorPos(out Vision2.ErosProjcetDLL.UI.UICon.POINT pOINT);
                        int X = dataGridView1.CurrentCellAddress.X;
                        int Y = dataGridView1.CurrentCellAddress.Y;
                        List<string> liatsName = new List<string>();
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[0].Value != null && dataGridView1.Rows[i].Cells[0].Value != "")
                            {
                                if (Product.GetProduct().Parameter_Dic.GetParameters().ContainsKey(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                                {
                                    liatsName.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                                }
                            }
                        }
                        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

                        foreach (var item2 in Product.GetProduct().Parameter_Dic.GetParameters())
                        {
                            if (!liatsName.Contains(item2.Key))
                            {
                                ToolStripMenuItem tool = new ToolStripMenuItem();
                                tool.Text = item2.Key;
                                tool.Click += Tool_Click;
                                contextMenuStrip.Items.Add(tool);
                            }
                        }
                        Rectangle rectangle = dataGridView1.GetCellDisplayRectangle(colum, rowt, false);
                        x = colum;
                        y = rowt;
                        Rectangle rectangle2 = dataGridView1.RectangleToScreen(rectangle);
                        contextMenuStrip.Show(rectangle2.X, rectangle2.Y + this.dataGridView1.Rows[rowt].Height);
                    }
                    catch (Exception es)
                    {
                    }
                }
                else if (e.ColumnIndex == 1)
                {
                    StructTypeValue structTypeValue = Product.GetProduct().Parameter_Dic.GetParameters()[dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()];
                    dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
                    dynamic min = null;
                    dynamic valut = null;
                    if (!UClass.GetTypeValue(structTypeValue.TypeStr,
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), out valut))
                    {
                        dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
                    }
                    else
                    {
                        if (structTypeValue.minValue != "" && structTypeValue.minValue != null)
                        {
                            UClass.GetTypeValue(structTypeValue.TypeStr,
                         structTypeValue.minValue, out min);
                        }
                        dynamic max = null;
                        if (structTypeValue.MaxValue != "" && structTypeValue.MaxValue != null)
                        {
                            UClass.GetTypeValue(structTypeValue.TypeStr,
                         structTypeValue.MaxValue, out max);
                        }
                        if (structTypeValue.TypeStr != "Boolean")
                        {
                            if ((max != null && valut > max) || (min != null && valut < min))
                            {
                                dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
                            }
                            else
                            {
                                Product.GetProd(Formname)[dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()] = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                                dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Green;
                            }
                        }
                        else
                        {
                            if (int.TryParse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), out int det))
                            {
                                dataGridView1.Rows[e.RowIndex].Cells[1].Value = Convert.ToBoolean(det);
                            }

                            Product.GetProd(Formname)[dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()] = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                            dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Green;
                        }
                    }
                }
                else if (e.ColumnIndex == 1)
                {
                }
            }
            catch (Exception ex)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
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
                if (dataGridView1.SelectedCells[0].ColumnIndex > 4)
                {
                    if (dataGridView1.IsCurrentCellDirty)
                    {
                        dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    }
                }
                else
                {
                    int colum = dataGridView1.SelectedCells[0].ColumnIndex;
                    int rowt = dataGridView1.SelectedCells[0].RowIndex;
                    if (colum == 0)
                    {
                        if (dataGridView1.IsCurrentCellDirty)
                        {
                            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        }
                        try
                        {
                            int X = dataGridView1.CurrentCellAddress.X;
                            int Y = dataGridView1.CurrentCellAddress.Y;
                            List<string> liatsName = new List<string>();
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                if (dataGridView1.Rows[i].Cells[0].Value == null)
                                {
                                    continue;
                                }
                                if (Product.GetProd(Formname).ContainsKey(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                                {
                                    liatsName.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                                }
                            }
                            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                            if (dataGridView1.Rows[Y].Cells[0].Value == null)
                            {
                                return;
                            }

                            if (dataGridView1.Rows[Y].Cells[0].Value.ToString().Length >= 3)
                            {
                                foreach (var item2 in Product.GetProd(Formname))
                                {
                                    if (!liatsName.Contains(item2.Key))
                                    {
                                        ToolStripMenuItem tool = new ToolStripMenuItem();
                                        tool.Text = item2.Key;
                                        tool.Click += Tool_Click;
                                        contextMenuStrip.Items.Add(tool);
                                    }
                                }
                                Rectangle rectangle = dataGridView1.GetCellDisplayRectangle(colum, rowt, false);
                                x = colum;
                                y = rowt;
                                Rectangle rectangle2 = dataGridView1.RectangleToScreen(rectangle);
                                contextMenuStrip.Show(rectangle2.X, rectangle2.Y + this.dataGridView1.Rows[rowt].Height);
                            }
                        }
                        catch (Exception es)
                        {
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
        public List<string> links;

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        public FormulaEditorControl(string name) : this()
        {
            Updatas(name);
        }

        public void Updatas(string name)
        {
            try
            {
                upT = false;
                Formname = name;
                if (name != null)
                {
                    toolStripLabel1.Text = name;
                }
                int i = 0;
                dataGridView1.Rows.Clear();
                foreach (var item in Product.GetProd(name))
                {
                    i = dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                    dataGridView1.Rows[i].Cells[0].Value = item.Key;
                    dataGridView1.Rows[i].Cells[1].Value = item.Value;
                    if (Product.GetParameters().ContainsKey(item.Key))
                    {
                        string dat = Product.GetParameters()[item.Key].TypeStr + ",参考值=" + Product.GetParameters()[item.Key].ValueStr + ",";
                        if (Product.GetParameters()[item.Key].minValue != null && Product.GetParameters()[item.Key].minValue != "")
                        {
                            dat += "最小:" + Product.GetParameters()[item.Key].minValue + ",";
                        }
                        if (Product.GetParameters()[item.Key].MaxValue != null && Product.GetParameters()[item.Key].MaxValue != "")
                        {
                            dat += "最大:" + Product.GetParameters()[item.Key].MaxValue + ",";
                        }
                        if (Product.GetParameters()[item.Key].Pst != null && Product.GetParameters()[item.Key].Pst != "")
                        {
                            dat += "描述:" + Product.GetParameters()[item.Key].Pst + ",";
                        }
                        dataGridView1.Rows[i].Cells[2].Value = dat.TrimEnd(',');
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[0].ContextMenuStrip = new ContextMenuStrip();
                        ToolStripItem toolStripItem = dataGridView1.Rows[i].Cells[0].ContextMenuStrip.Items.Add("删除");
                        toolStripItem.Tag = i;
                        toolStripItem.Click += FormulaEditorControl_Click;

                        dataGridView1.Rows[i].Cells[0].ReadOnly = false;
                        dataGridView1[0, i].Style.BackColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败：" + ex.Message);
            }
            upT = true;
        }

        private void FormulaEditorControl_Click(object sender, EventArgs e)
        {
            try
            {
                int dt = dataGridView1.SelectedCells.Count;
                for (int i = 0; i < dt; i++)
                {
                    Product.GetProd(toolStripLabel1.Text).Remove(dataGridView1.SelectedCells[0].Value.ToString());
                    Product.GetParameters().Remove(dataGridView1.SelectedCells[0].Value.ToString());
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 添加链接列到数据表
        /// </summary>
        /// <param name="linkNames"></param>
        public void AddDataGridConmlus(string[] linkNames)
        {
            links = linkNames.ToList();

            tool1.DropDownItems.Clear();

            foreach (var item in linkNames)
            {
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
                                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                                {
                                    dataGridView1[1, i].Style.BackColor = Color.Coral;
                                }

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
                                            if (dataGridView1.Rows[i].Cells[0].Value != null)
                                            {
                                                string key = dataGridView1.Rows[i].Cells[0].Value.ToString();
                                                if (Product.GetProduct().Parameter_Dic.ParameterMap[item].ContainsKey(key))
                                                {
                                                    if (Product.GetProduct().Parameter_Dic.ParameterMap[item][key] != null &&
                                                 Product.GetProduct().Parameter_Dic.ParameterMap[item][key].Trim() != ""
                                                 && Product.GetParameters()[key].TypeStr != null)
                                                    {
                                                        string add = Product.GetProduct().Parameter_Dic.ParameterMap[item][key];
                                                        if (UClass.GetTypeList().Contains(Product.GetParameters()[key].TypeStr))
                                                        {
                                                            erosLinkNet.GetIDValue(add, Product.GetParameters()[key].TypeStr, out dynamic value);
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

        public void Updatas()
        {
            Updatas(Formname);
            if (!Vision2.ErosProjcetDLL.Project.ProjectINI.Enbt)
            {
                Column1.ReadOnly = Column3.ReadOnly = true;
            }
        }

        public class Editor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                if (service != null)
                {
                    service.DropDownControl(new FormulaEditorControl(value.ToString()));
                }
                return value;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
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
                        Product.GetProd(this.Formname).Clear();
                        foreach (System.Data.DataRow item1 in dataTable2.Rows)
                        {
                            if (!Product.GetProd(this.Formname).ContainsKey(item1[0].ToString()))
                            {
                                Product.GetProd(this.Formname).Add(item1[0].ToString(), item1[1].ToString());
                            }
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> itemT = new Dictionary<string, string>();
                //Dictionary<string, string> item = Product.GetProd(toolStripLabel1.Text);
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value != null && dataGridView1.Rows[i].Cells[1].Value != null)
                        {
                            dataGridView1[0, i].Style.BackColor = Color.White;
                            dataGridView1[1, i].Style.BackColor = Color.White;
                            string key = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            string type = Product.GetProduct().Parameter_Dic.GetParameters()
                                 [dataGridView1.Rows[i].Cells[0].Value.ToString()].TypeStr;
                            string valuestr = dataGridView1.Rows[i].Cells[1].Value.ToString();
                            string minValue = Product.GetProduct().Parameter_Dic.GetParameters()
                                 [dataGridView1.Rows[i].Cells[0].Value.ToString()].minValue;
                            string manValue = Product.GetProduct().Parameter_Dic.GetParameters()
                         [dataGridView1.Rows[i].Cells[0].Value.ToString()].MaxValue;
                            dataGridView1[0, i].Style.BackColor = Color.White;
                            if (UClass.GetUesrListType().Contains(type))
                            {
                                if (!UClass.GetTypeValue(type,
                                valuestr, out dynamic value))
                                {
                                    dataGridView1[0, i].Style.BackColor = Color.Red;
                                }
                                else
                                {
                                    if (minValue != null &&
                                     minValue != "")
                                    {
                                        UClass.GetTypeValue(type,
                                        minValue, out dynamic min);
                                        if (value < min)
                                        {
                                            dataGridView1[1, i].Style.BackColor = Color.Red;
                                        }
                                    }
                                    if (manValue != null &&
                                      manValue != "")
                                    {
                                        UClass.GetTypeValue(type,
                                                manValue, out dynamic max);
                                        if (value > max)
                                        {
                                            dataGridView1[1, i].Style.BackColor = Color.Red;
                                        }
                                    }
                                    if (dataGridView1[1, i].Style.BackColor != Color.Red)
                                    {
                                        //item[dataGridView1.Rows[i].Cells[0].Value.ToString()] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                                        itemT.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(), dataGridView1.Rows[i].Cells[1].Value.ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        dataGridView1[0, i].Style.BackColor = Color.Red;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
                Dictionary<string, string> item = Product.GetProd(toolStripLabel1.Text);
                item.Clear();
                foreach (var item1 in itemT)
                {
                    item.Add(item1.Key, item1.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedCells[0].ColumnIndex == 1)
                {
                    return;
                }
                if (dataGridView1.SelectedCells.Count == 1)
                {
                    dataGridView1.EnableHeadersVisualStyles = false;
                    dataGridView1.Columns[dataGridView1.SelectedCells[0].ColumnIndex].DefaultCellStyle.SelectionBackColor = Color.Blue;
                }
                if (dataGridView1.SelectedCells[0].RowIndex >= 0)
                {
                    if (dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value == null)
                    {
                        return;
                    }
                    string key = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                    if (Product.GetParameters().ContainsKey(key))
                    {
                        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                        string dat = key + ":{";
                        if (Product.GetParameters()[key].ValueStr != null && Product.GetParameters()[key].ValueStr != "")
                        {
                            dat += "参考值=" + Product.GetParameters()[key].ValueStr + ",";
                        }
                        if (Product.GetParameters()[key].TypeStr != null && Product.GetParameters()[key].TypeStr != "")
                        {
                            dat += "类型=" + Product.GetParameters()[key].TypeStr + ",";
                        }
                        if (Product.GetParameters()[key].ValueEn != null && Product.GetParameters()[key].ValueEn != "")
                        {
                            dat += "枚举=" + Product.GetParameters()[key].ValueEn + ",";
                        }
                        if (Product.GetParameters()[key].minValue != null && Product.GetParameters()[key].minValue != "")
                        {
                            dat += "最小值=" + Product.GetParameters()[key].minValue + ",";
                        }
                        if (Product.GetParameters()[key].MaxValue != null && Product.GetParameters()[key].MaxValue != "")
                        {
                            dat += "最大值=" + Product.GetParameters()[key].MaxValue + ",";
                        }
                        if (Product.GetParameters()[key].Pst != null && Product.GetParameters()[key].Pst != "")
                        {
                            dat += "描述=" + Product.GetParameters()[key].Pst + ",";
                        }

                        dat = dat.TrimEnd(',') + "}";
                        if (dat == "")
                        {
                            return;
                        }
                        contextMenuStrip.Items.Add(dat);
                        Rectangle rectangle = dataGridView1.GetCellDisplayRectangle(dataGridView1.SelectedCells[0].ColumnIndex,
                            dataGridView1.SelectedCells[0].RowIndex, false);
                        Rectangle rectangle2 = dataGridView1.RectangleToScreen(rectangle);
                        contextMenuStrip.Show(rectangle2.X, rectangle2.Y + this.dataGridView1.Rows[0].Height);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void tool1_MouseMove(object sender, MouseEventArgs e)
        {
            tool1.ShowDropDown();
        }

        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}