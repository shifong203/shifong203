using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Vision2.Project.formula
{
    public partial class SynchronizationForm : Form
    {
        public string formulaName = "";

        /// <summary>
        /// 通信接口
        /// </summary>
        private List<ErosSocket.ErosConLink.IErosLinkNet> erosLinkNet;

        private bool off;

        public SynchronizationForm()
        {
            InitializeComponent();
            progressBar1.BackColor = Color.Red;
            progressBar1.ForeColor = Color.Red;
        }

        public void Updatas(string name)
        {
            try
            {
                formulaName = name;
                int i = 0;
                dataGridView1.Rows.Clear();
                foreach (var item in Product.GetProd(formulaName))
                {
                    i = dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = item.Key;

                    dataGridView1.Rows[i].Cells[1].Value = item.Value;
                    if (Product.GetParameters().ContainsKey(item.Key))
                    {
                        dataGridView1.Rows[i].Cells[2].Value = Product.GetParameters()[item.Key].TypeStr;
                    }

                    for (int it = 3; it < dataGridView1.Columns.Count; it++)
                    {
                        if (Product.GetProduct().Parameter_Dic.ParameterMap.ContainsKey(dataGridView1.Columns[it].HeaderText))
                        {
                            dataGridView1.Rows[i].Cells[it].Value = Product.GetProduct().Parameter_Dic.ParameterMap[dataGridView1.Columns[it].HeaderText][item.Key];
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void Setd()
        {
            List<string> listErr = new List<string>();

            progressBar2.Maximum = dataGridView1.Rows.Count;
            if (erosLinkNet.Count == 0)
            {
                threadStop = false;
                return;
            }
            List<string> keys = new List<string>();
            List<string> values = new List<string>();
            progressBar1.Maximum = erosLinkNet.Count;
            int ErrNumber = 0;
            for (int it = 0; it < erosLinkNet.Count; it++)
            {
                progressBar1.Value = it;
                label2.Text = "进度:" + (erosLinkNet.Count * 1) + @"\" + it;
                textBox1.AppendText(erosLinkNet[it].Name + "开始下载参数" + Environment.NewLine);
                keys.Clear();
                values.Clear();
                string ds = erosLinkNet[it].Name;
                if (erosLinkNet[it].Split_Mode != ErosSocket.ErosConLink.SocketClint.SplitMode.Array)
                {
                    if (!erosLinkNet[it].IsConn)
                    {
                        listErr.Add(erosLinkNet[it].Name + ":连接断开!加载异常");
                    }
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (this.off)
                    {
                        Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("取消加载产品" + formulaName);
                        return;
                    }
                    if (erosLinkNet[it].Split_Mode != ErosSocket.ErosConLink.SocketClint.SplitMode.Array)
                    {
                        if (dataGridView1[ds, i].Value != null && dataGridView1[ds, i].Value.ToString() != "")
                        {
                            try
                            {
                                keys.Add(dataGridView1[ds, i].Value.ToString());
                                values.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                                if (ErosSocket.ErosConLink.UClass.GetTypeList().Contains(dataGridView1[2, i].Value.ToString()))
                                {
                                    //if (dataGridView1[ds, i].Value.ToString().Contains("."))
                                    //{
                                    if (erosLinkNet[it].SetIDValue(dataGridView1[ds, i].Value.ToString(),
                                        dataGridView1[2, i].Value.ToString(), dataGridView1.Rows[i].Cells[1].Value.ToString(), out string errs))
                                    {
                                        //textBox1.AppendText(erosLinkNet[it].Name);
                                    }
                                    //}
                                    //else
                                    //{
                                    //  erosLinkNet[it].SetValue(dataGridView1[ds, i].Value.ToString(),
                                    //    dataGridView1.Rows[i].Cells[1].Value.ToString(), out string errs);
                                    //}
                                }
                            }
                            catch (Exception ec)
                            {
                            }
                        }
                    }
                    else
                    {
                        values.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    }
                    if (!threadStop)
                    {
                        threadStop = true;
                        if (DialogResult.OK == MessageBox.Show(
                            "确定取消？", "取消加载参数", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                        {
                            button1.Enabled = false;
                            button2.Enabled = true;
                            threadStop = false;
                            off = true;
                            return;
                        }
                    }
                    button1.Enabled = true;
                    progressBar2.Value = i + 1;
                    label1.Text = "进度:" + dataGridView1.Rows.Count + @"\" + i;
                }
                label1.Text = "进度:" + dataGridView1.Rows.Count + @"\" + dataGridView1.Rows.Count;

                progressBar2.Value = dataGridView1.Rows.Count;
                textBox1.AppendText(erosLinkNet[it].Name + "加载完成" + Environment.NewLine);
                textBox1.AppendText(erosLinkNet[it].Name + "开始回传" + Environment.NewLine);
                if (erosLinkNet[it].Split_Mode != ErosSocket.ErosConLink.SocketClint.SplitMode.Array)
                {
                    if (!erosLinkNet[it].IsConn)
                    {
                        listErr.Add(erosLinkNet[it].Name + ":连接断开!回传失败");
                    }
                }
                //回传
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (this.off)
                    {
                        Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("取消回传产品" + formulaName);
                        return;
                    }
                    if (erosLinkNet[it].Split_Mode != ErosSocket.ErosConLink.SocketClint.SplitMode.Array)
                    {
                        if (dataGridView1[ds, i].Value == null || dataGridView1[ds, i].Value.ToString() == "")
                        {
                            continue;
                        }
                        if (ErosSocket.ErosConLink.UClass.GetTypeList().Contains(dataGridView1[2, i].Value.ToString()))
                        {
                            //if (dataGridView1[ds, i].Value.ToString().Contains("."))
                            //{
                            if (erosLinkNet[it].GetIDValue(dataGridView1[ds, i].Value.ToString(), dataGridView1[2, i].Value.ToString(), out dynamic dynamic))
                            {
                                if (dynamic.ToString().StartsWith(dataGridView1.Rows[i].Cells[1].Value.ToString()))
                                {
                                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                                }
                                else
                                {
                                    this.dataGridView1[ds, i].Style.BackColor = Color.Red;
                                    ErrNumber++;
                                }
                                dataGridView1[ds, i].Value += "(" + dynamic.ToString() + ")";
                            }
                            else
                            {
                                dataGridView1[ds, i].Value += "(null)";
                            }
                            //}
                            //else
                            //{
                            //    ErosSocket.ErosConLink.UClass.ErosValues.ErosValueD erosValueD =
                            //        new ErosSocket.ErosConLink.UClass.ErosValues.ErosValueD();
                            //    if (erosLinkNet[it].KeysValues.DictionaryValueD.ContainsKey(dataGridView1.Rows[i].Cells[ds].Value.ToString()))
                            //    {
                            //        erosValueD = erosLinkNet[it].KeysValues.DictionaryValueD[dataGridView1.Rows[i].Cells[ds].Value.ToString()];
                            //        if (erosLinkNet[it].GetValue(erosValueD))
                            //        {
                            //            if (erosValueD.Value.ToString().StartsWith(dataGridView1.Rows[i].Cells[1].Value.ToString()))
                            //            {
                            //                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                            //            }
                            //            else
                            //            {
                            //                this.dataGridView1[ds, i].Style.BackColor = Color.Red;
                            //                ErrNumber++;
                            //            }
                            //            dataGridView1[ds, i].Value += "(" + erosValueD.Value.ToString() + ")";
                            //        }
                            //        else
                            //        {
                            //            dataGridView1[ds, i].Value += "(null)";
                            //        }
                            //    }

                            //}
                        }
                    }
                }
                if (erosLinkNet[it] is ErosSocket.ErosConLink.SocketClint)
                {
                    ErosSocket.ErosConLink.SocketClint itnem = erosLinkNet[it] as ErosSocket.ErosConLink.SocketClint;
                    if (itnem.PLCRun != null)
                    {
                        if (itnem.PLCRun.LinkHCIF != null && itnem.PLCRun.LinkCOn != null && itnem.PLCRun.LinkIDname != null &&
                            itnem.PLCRun.LinkHCIF != "" && itnem.PLCRun.LinkCOn != "" && itnem.PLCRun.LinkIDname != "")
                        {
                            if (itnem.KeysValues[itnem.PLCRun.LinkHCIF])
                            {
                                itnem.SetValue(itnem.PLCRun.LinkIDname, Product.GetProd(formulaName)["产品ID"], out string err);
                                if (err != "")
                                {
                                    ErrNumber++;
                                }
                                itnem.SetValue(itnem.PLCRun.LinkCOn, true.ToString(), out err);
                                if (err != "")
                                {
                                    ErrNumber++;
                                }
                            }
                            else
                            {
                                ErrNumber++;
                                Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("加载失败:" + itnem.Name + "换型条件不满足！");
                                textBox1.AppendText("加载失败:" + itnem.Name + "换型条件不满足！" + Environment.NewLine);
                            }
                        }
                    }
                }
            }

            threadStop = false;
            progressBar1.Value = erosLinkNet.Count;
            button1.Enabled = false;
            if (vision.Vision.Instance.ISPName)
            {
                vision.Vision.UpReadThis(formulaName);
            }
            Thread.Sleep(100);
            if (ErrNumber == 0 && listErr.Count == 0)
            {
                textBox1.AppendText(formulaName + "产品下载完成" + Environment.NewLine);

                MessageBox.Show("加载完成");
            }
            else
            {
                for (int i = 0; i < listErr.Count; i++)
                {
                    textBox1.AppendText(listErr[i] + Environment.NewLine);
                }
                progressBar2.BackColor = Color.Red;
                progressBar1.ForeColor = Color.Red;
                if (ErrNumber != 0)
                {
                    MessageBox.Show(string.Format("加载失败，{0}个参数未加载成功", ErrNumber));
                }
                else
                {
                    MessageBox.Show(string.Format("加载失败,{0}个错误", listErr.Count));
                }
            }
            Product.ProductionName = formulaName;
        }

        /// <summary>
        /// 加载参数
        /// </summary>
        public void Setdata()
        {
            threadStop = true;
            Thread thread = new Thread(() =>
            {
                Thread.Sleep(500);
                try
                {
                    Setd();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }

        private bool threadStop = false;

        public void ShowDialogSet(string[] linkNames, string formula_Name)
        {
            formulaName = formula_Name;
            this.Text = "加载产品:" + formulaName + "参数";
            erosLinkNet = new List<ErosSocket.ErosConLink.IErosLinkNet>();
            foreach (var item in linkNames)
            {
                DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
                //     DataGridViewComboEditBoxColumn column1 = new DataGridViewComboEditBoxColumn();
                column1.Name = item;
                column1.HeaderText = item;
                column1.Width = 150;
                column1.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridView1.Columns.Add(column1);
                if (ErosSocket.ErosConLink.DicSocket.Instance.SocketClint.ContainsKey(item))
                {
                    erosLinkNet.Add(ErosSocket.ErosConLink.DicSocket.Instance.SocketClint[item]);
                }
                else
                {
                    threadStop = false;
                    MessageBox.Show("链接名[" + item + "}不存在!");
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int it = 3; it < dataGridView1.Columns.Count; it++)
                {
                    if (Product.GetProduct().Parameter_Dic.ParameterMap.ContainsKey(dataGridView1.Columns[it].HeaderText))
                    {
                        if (Product.GetProduct().Parameter_Dic.ParameterMap[dataGridView1.Columns[it].HeaderText]
                            .ContainsKey(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                        {
                            dataGridView1.Rows[i].Cells[it].Value =
                                Product.GetProduct().Parameter_Dic.ParameterMap[dataGridView1.Columns[it].HeaderText][dataGridView1.Rows[i].Cells[0].Value.ToString()];
                        }
                    }
                }
            }
            this.Setdata();
            this.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            threadStop = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Setdata();
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void SynchronizationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!threadStop)
            {
                return;
         
            }
            off = true;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }
    }
}