using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using Vision2.vision;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.ConClass
{
    public class Npoi : Vision2.ErosProjcetDLL.Excel.Npoi
    {
        /// <summary>
        /// 读取Exclec表格到TabControl并以表格创建TabPage 将数据填充到 DataGridView
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void UpDataExclec(string path, TabControl tabControl1, PictureBox pictureBox, GroupBox groupBox, HalconRun halcon)
        {
            List<string> listCoumnsName = new List<string>();
            tabControl1.Controls.Clear();
            try
            {
                //this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                string strConn = "";
                if (!File.Exists(path))
                {
                    return;
                }
                if (IntPtr.Size == 4)
                {
                    // 32-bit
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + "Extended Properties=Excel 8.0;";
                }
                else if (IntPtr.Size == 8)
                {
                    strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + path + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'"; //此連接可以操作.xls與.xlsx文件
                }
                if (System.IO.Path.GetExtension(path).ToLower().Contains(".xls"))
                {
                    //打开Excel的连接，设置连接对象
                    OleDbConnection conne = new OleDbConnection(strConn);
                    conne.Open();
                    tabControl1.TabPages.Clear();
                    DataTable sheetNames = conne.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    //遍历Excel文件获取Excel工作表，并将所有工作表名称加载到comboBox控件中
                    foreach (DataRow dr in sheetNames.Rows)
                    {
                        //添加工作表名称
                        TabPage tabPage = new TabPage();
                        tabPage.Text = dr[2].ToString();
                        tabPage.Name = dr[2].ToString();
                        tabPage.AutoScroll = true;
                        tabControl1.TabPages.Add(tabPage);
                        try
                        {
                            //当前选中的工作表前几行数据，获取数据列
                            OleDbDataAdapter oada3 = new OleDbDataAdapter("select top 5 * from [" + dr[2].ToString() + "]", strConn);
                            DataTable ds = new DataTable();
                            oada3.Fill(ds);
                            //将列加载到树节点上
                            for (int i = 0; i < ds.Columns.Count; i++)
                            {
                                listCoumnsName.Add(ds.Columns[i].ColumnName.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        //存储需要查询数据的列
                        string strName = "";
                        //遍历列名，每个列名用逗号隔开
                        for (int i = 0; i < listCoumnsName.Count; i++)
                        {
                            strName = strName + "[" + listCoumnsName[i] + "],";
                        }
                        strName = strName.Substring(0, strName.Length - 1);
                        //获取用户没有删掉留下来的列，读取这些列的数据
                        //建立Excel连接
                        //读取数据
                        OleDbDataAdapter oada = new OleDbDataAdapter("select  " + strName + " from [" + tabPage.Text + "]", strConn);
                        DataTable dt = new DataTable();

                        //填入DataTable
                        oada.Fill(dt);
                        if (IntPtr.Size == 8)
                        {
                            for (int i = 0; i < listCoumnsName.Count; i++)
                            {
                                if (listCoumnsName[i] != null)
                                {
                                    dt.Columns[listCoumnsName[i]].ColumnName = dt.Rows[0][listCoumnsName[i]].ToString();
                                }
                            }
                        }
                        DataGridView dataGridView = new DataGridView();
                        dataGridView.AllowUserToAddRows = false;
                        tabPage.Controls.Add(dataGridView);
                        dataGridView.Name = tabPage.Text;
                        dataGridView.DataSource = dt;
                        dataGridView.Dock = DockStyle.Fill;
                        dataGridView.Left = 0;
                        //dataGridView.Size= new System.Drawing.Size() { Height = tabPage.Size.Height, Width = tabPage.Size.Width * 4 };
                        dataGridView.RowHeadersVisible = false;
                        dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridView.MouseUp += DataGridView_MouseUP;
                        dataGridView.ScrollBars = ScrollBars.Both;
                        dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        ContextMenuStrip contextMenu = new ContextMenuStrip();
                        dataGridView.ContextMenuStrip = contextMenu;
                        ToolStripItem toolStripItem = contextMenu.Items.Add("打开图片");
                        ToolStripItem toolStripItem2 = contextMenu.Items.Add("计算偏差");
                        toolStripItem2.Click += ToolStripItem2_Click;

                        void ToolStripItem2_Click(object sender, EventArgs e)
                        {
                            try
                            {
                                ErosUI.FormText formTxt = new ErosUI.FormText();
                                Vision2.ErosProjcetDLL.UI.UICon.WindosFormerShow(ref formTxt);
                                if (dataGridView.Rows.Count == 0)
                                {
                                    formTxt.richTextBox1.AppendText("没有可计算的数据" + Environment.NewLine);
                                    formTxt.richTextBox1.ScrollToCaret();
                                    return;
                                }
                                else
                                {
                                    double Trydouble = 0;
                                    int hTupleNG = 0;
                                    int hTupleOK = 0;
                                    int hTupleNull = 0;
                                    for (int i = 0; i < dataGridView.Columns.Count; i++)
                                    {
                                        HalconDotNet.HTuple hTupleMin = new HalconDotNet.HTuple();
                                        for (int iR = 0; iR < dataGridView.Rows.Count; iR++)
                                        {
                                            if (dataGridView.Rows[iR].Cells[i].Value != null)
                                            {
                                                if (iR == 0)
                                                {
                                                    if (IntPtr.Size == 4)
                                                    {
                                                        formTxt.richTextBox1.AppendText(string.Format("第{0}列，行数{1}，列名:{2};", i, dataGridView.Rows.Count - 1, dataGridView.Columns[i].HeaderText.ToString()));
                                                    }
                                                    else
                                                    {
                                                        formTxt.richTextBox1.AppendText(string.Format("第{0}列，行数{1}，列名:{2};", i, dataGridView.Rows.Count - 1, dataGridView.Rows[0].Cells[i].Value.ToString()));
                                                    }
                                                }
                                                else
                                                {
                                                    if (dataGridView.Rows[iR].Cells[1].Value.ToString() != "Null")
                                                    {
                                                        if (double.TryParse(dataGridView.Rows[iR].Cells[i].Value.ToString(), out Trydouble))
                                                        {
                                                            hTupleMin.Append(Trydouble);
                                                        }
                                                    }
                                                    if (i == 1)
                                                    {
                                                        if (dataGridView.Rows[iR].Cells[1].Value.ToString() == "Null")
                                                        {
                                                            hTupleNull++;
                                                        }
                                                        else if (dataGridView.Rows[iR].Cells[1].Value.ToString() == "OK")
                                                        {
                                                            hTupleOK++;
                                                        }
                                                        else if (dataGridView.Rows[iR].Cells[1].Value.ToString() == "NG")
                                                        {
                                                            hTupleNG++;
                                                        }
                                                    }
                                                }
                                            }
                                        }//执行行计算
                                        if (i == 1)
                                        {
                                            double dobue = 0;
                                            if (dataGridView.Rows.Count - 1 - (hTupleNG + hTupleNull) != 0)
                                            {
                                                dobue = (dataGridView.Rows.Count - 1) / (dataGridView.Rows.Count - 1 - (hTupleNG + hTupleNull)) * 100;
                                            }
                                            formTxt.richTextBox1.AppendText(string.Format("NG数:{0},OK数:{1}，Null数:{2}，OK百分比%:{3}", hTupleNG, hTupleOK, hTupleNull, dobue + Environment.NewLine));
                                        }
                                        else if (hTupleMin.Length == 0)
                                        {
                                            formTxt.richTextBox1.AppendText("没有可计算的数据" + Environment.NewLine);
                                        }
                                        else
                                        {
                                            formTxt.richTextBox1.AppendText(string.Format("最小数据:{0},平均值:{1}，最大数据:{2}，偏差±:{3}", hTupleMin.TupleMin(), hTupleMin.TupleMean(), hTupleMin.TupleMax(), (hTupleMin.TupleMax() - hTupleMin.TupleMin()) / 2) + Environment.NewLine);
                                        }
                                    }//执行列计算
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        toolStripItem.Click += ToolStripItem_Click;
                        void ToolStripItem_Click(object sender, EventArgs e)
                        {
                            try
                            {
                                if (dataGridView.SelectedRows.Count == 0)
                                {
                                    return;
                                }
                                if (dataGridView.SelectedRows[0] != null && dataGridView.SelectedRows[0].Cells[0] != null)
                                {
                                    string dsd = Vision.Instance.DicSaveType[halcon.Name].SavePath + @"\" + dataGridView.Name.Trim('\'').Remove(dataGridView.Name.Length - 3) + "\\" + dataGridView.SelectedRows[0].Cells[0].Value.ToString();

                                    vision.Vision.GetRunNameVision().ShowImage(dsd + ".bmp");
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        void DataGridView_MouseUP(object sender, MouseEventArgs e)
                        {
                            DataGridView data = (DataGridView)sender;
                            try
                            {
                                if (data.SelectedRows.Count == 0)
                                {
                                    return;
                                }
                                if (data.SelectedRows[0] != null && data.SelectedRows[0].Cells[0] != null)
                                {
                                    string dsd = "";
                                    if (data.SelectedRows[0].Cells[1].Value.ToString().Contains("OK"))
                                    {
                                        dsd = Vision.Instance.DicSaveType[halcon.Name].SavePath + "\\" + data.Name.Trim('\'').Remove(data.Name.Length - 3) + "\\" + data.SelectedRows[0].Cells[0].Value.ToString();
                                    }
                                    else
                                    {
                                        dsd = Vision.Instance.DicSaveType[halcon.Name].SavePath + "\\" + data.Name.Trim('\'').Remove(data.Name.Length - 3) + "\\" + data.SelectedRows[0].Cells[0].Value.ToString();
                                    }
                                    if (File.Exists(dsd + ".bmp"))
                                    {
                                        groupBox.Text = Path.GetFileName(dsd);
                                        pictureBox.Load(dsd + ".bmp");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        for (int i = 0; i < dataGridView.Columns.Count; i++)
                        {
                            dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                    }
                    conne.Close();
                }
                else
                {
                    MessageBox.Show("excel 格式不正确！");
                }
                listCoumnsName.Clear();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return;
            //设置鼠标指针状态为默认状态
        }
    }
}