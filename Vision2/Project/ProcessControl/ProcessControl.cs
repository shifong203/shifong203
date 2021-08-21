using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace Vision2.Project.ProcessControl
{
    public partial class ProcessControl : System.Windows.Forms.UserControl
    {
        public ProcessControl(ProcessUser process)
        {
            InitializeComponent();
            Proce = process;
            StaticProcessThis = this;
            if (Proce.kValues == null)
            {
                Proce.kValues = new Dictionary<string, ProcessUser.MyStruct>();
            }
            UpRest();
            dataGridView2.Rows.Add(17);
            dataGridView2.Rows[0].Cells[0].Value = "当前码";
            dataGridView2.Rows[1].Cells[0].Value = "码堆栈1";
            dataGridView2.Rows[2].Cells[0].Value = "码堆栈2";
            dataGridView2.Rows[3].Cells[0].Value = "码堆栈3";
            dataGridView2.Rows[4].Cells[0].Value = "码堆栈4";
            dataGridView2.Rows[5].Cells[0].Value = "码堆栈5";
            dataGridView2.Rows[6].Cells[0].Value = "码堆栈6";
            dataGridView2.Rows[7].Cells[0].Value = "分割线";
            dataGridView2.Rows[8].Cells[0].Value = "分割线";
            dataGridView2.Rows[7].Cells[1].Value = "————";
            dataGridView2.Rows[8].Cells[1].Value = "————";
            dataGridView2.Rows[9].Cells[0].Value = "历史码1";
            dataGridView2.Rows[10].Cells[0].Value = "历史码2";
            dataGridView2.Rows[11].Cells[0].Value = "历史码3";
            dataGridView2.Rows[12].Cells[0].Value = "历史码4";
            dataGridView2.Rows[13].Cells[0].Value = "历史码5";
            dataGridView2.Rows[14].Cells[0].Value = "历史码6";
            dataGridView2.Rows[15].Cells[0].Value = "历史码7";
            dataGridView2.Rows[16].Cells[0].Value = "历史码8";
        }

        public static ProcessControl StaticProcessThis;
        private int it = 0;
        private List<string> ts;
        private string code = "";

        /// <summary>
        /// 初始化
        /// </summary>
        public void Up()
        {
            try
            {
                if (this.Created)
                {
                    this.Invoke(new Action(() =>
                    {
                        try
                        {
                            if (ts == null)
                            {
                                ts = new List<string>();
                            }
                            ts = new List<string>();
                            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                            {
                                if (ts.Contains(dataGridView1.Columns[i].HeaderText))
                                {
                                    this.dataGridView1.Columns.RemoveAt(i);
                                    continue;
                                }
                                ts.Add(dataGridView1.Columns[i].HeaderText);
                            }
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                if (dataGridView1.Rows[i].Cells[0].Value != null)
                                {
                                    if (!Proce.ProductMessage.ContainsKey(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                                    {
                                        dataGridView1.Rows.RemoveAt(i);
                                    }
                                }
                            }
                            foreach (var item in Proce.ProductMessage)
                            {
                                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                                {
                                    if (dataGridView1.Rows[i].Cells[0].Value != null)
                                    {
                                        if (dataGridView1.Rows[i].Cells[0].Value.ToString() == item.Key)
                                        {
                                            it = i;
                                            goto enst;
                                        }
                                    }
                                }
                                it = dataGridView1.Rows.Add();
                                dataGridView1.Rows[it].Cells[0].Value = item.Key;
                            enst:
                                foreach (var item2 in item.Value)
                                {
                                    if (ts.Contains(item2.Key))
                                    {
                                        dataGridView1[item2.Key, it].Value = item2.Value;
                                    }
                                    else
                                    {
                                        DataGridViewTextBoxColumn dataGridViewColumn = new DataGridViewTextBoxColumn();
                                        dataGridViewColumn.Name = dataGridViewColumn.HeaderText = item2.Key;
                                        dataGridView1.Columns.Add(dataGridViewColumn);
                                        dataGridView1[item2.Key, it].Value = item2.Value;
                                    }
                                }
                            }
                            string itmes = ErosSocket.ErosConLink.
                                 StaticCon.GetLingkNameValueString("打码清洗.二维码0");
                            if (itmes != code)
                            {
                                if (code != "")
                                {
                                    for (int i = 8; i > 0; i--)
                                    {
                                        dataGridView2.Rows[i + 8].Cells[1].Value = dataGridView2.Rows[8 + i - 1].Cells[1].Value;
                                    }
                                    dataGridView2.Rows[9].Cells[1].Value = code;
                                }
                                code = itmes;
                            }
                            dataGridView2.Rows[0].Cells[1].Value = code;
                            dataGridView2.Rows[1].Cells[1].Value = ErosSocket.ErosConLink.
                            StaticCon.GetLingkNameValueString("打码清洗.二维码堆栈");
                            dataGridView2.Rows[2].Cells[1].Value = ErosSocket.ErosConLink.
                            StaticCon.GetLingkNameValueString("打码清洗.二维码堆栈1");
                            dataGridView2.Rows[3].Cells[1].Value = ErosSocket.ErosConLink.
                            StaticCon.GetLingkNameValueString("打码清洗.二维码2");
                            dataGridView2.Rows[4].Cells[1].Value = ErosSocket.ErosConLink.
                            StaticCon.GetLingkNameValueString("打码清洗.二维码堆栈3");
                            dataGridView2.Rows[5].Cells[1].Value = ErosSocket.ErosConLink.
                            StaticCon.GetLingkNameValueString("打码清洗.二维码堆栈4");
                            dataGridView2.Rows[6].Cells[1].Value = ErosSocket.ErosConLink.
                            StaticCon.GetLingkNameValueString("打码清洗.二维码堆栈5");
                            for (int i = 0; i < this.dataGridView3.Columns.Count; i++)
                            {
                                if (ts.Contains(dataGridView3.Columns[i].HeaderText))
                                {
                                    this.dataGridView3.Columns.RemoveAt(i);
                                    continue;
                                }
                                ts.Add(dataGridView3.Columns[i].HeaderText);
                            }
                            for (int i = 0; i < dataGridView3.Rows.Count; i++)
                            {
                                if (dataGridView3.Rows[i].Cells[0].Value != null)
                                {
                                    if (!ProcessUser.Instancen.GetTrayID().ContainsKey(dataGridView3.Rows[i].Cells[0].Value.ToString()))
                                    {
                                        dataGridView3.Rows.RemoveAt(i);
                                    }
                                }
                            }
                            foreach (var item in ProcessUser.Instancen.GetTrayID())
                            {
                                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                                {
                                    if (dataGridView3.Rows[i].Cells[0].Value != null)
                                    {
                                        if (dataGridView3.Rows[i].Cells[0].Value.ToString() == item.Key)
                                        {
                                            it = i;
                                            goto enst;
                                        }
                                    }
                                }
                                it = dataGridView3.Rows.Add();
                                dataGridView3.Rows[it].Cells[0].Value = item.Key;
                            enst:
                                foreach (var item2 in item.Value)
                                {
                                    if (ts.Contains(item2.Key))
                                    {
                                        dataGridView3[item2.Key, it].Value = item2.Value;
                                    }
                                    else
                                    {
                                        DataGridViewTextBoxColumn dataGridViewColumn = new DataGridViewTextBoxColumn();
                                        dataGridViewColumn.Name = dataGridViewColumn.HeaderText = item2.Key;
                                        dataGridView3.Columns.Add(dataGridViewColumn);
                                        dataGridView3[item2.Key, it].Value = item2.Value;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.Message);
                        }
                    }));
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void UpRest()
        {
            try
            {
                //if (ErosSocket.ErosConLink.StaticCon.GetLingkNameValue(ProcessUser.GetThis().EapEnName))
                //{
                //    button2.BackColor = Color.GreenYellow;
                //}
                //else
                //{
                //    button2.BackColor = Color.Red;
                //}
                // ErosSocket.ErosConLink.StaticCon.GetLingkIDValueS("打码清洗.DB17.4", "String",100, out dynamic ert);
                //textBox5.Text = ert;
                ErosSocket.ErosConLink.StaticCon.GetLingkIDValueS("打码清洗.DB17.106", "String", 100, out dynamic ert);
                textBox6.Text = ert;
                ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                contextMenuStrip.Items.Add("删除行").Click += ProcessControl_Click;
                dataGridView1.ContextMenuStrip = contextMenuStrip;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                DataGridViewTextBoxColumn gridViewColumn = new DataGridViewTextBoxColumn();
                gridViewColumn.Name = "产品编号";
                gridViewColumn.HeaderText = "产品编号";
                dataGridView1.Columns.Add(gridViewColumn);
                gridViewColumn.SortMode = DataGridViewColumnSortMode.Programmatic;
                DataGridViewTextBoxColumn gridViewState = new DataGridViewTextBoxColumn();
                gridViewState.Name = "过程";
                gridViewState.HeaderText = "过程";
                gridViewState.SortMode = DataGridViewColumnSortMode.Programmatic;
                dataGridView1.Columns.Add(gridViewState);
                foreach (var item in Proce.kValues)
                {
                    DataGridViewTextBoxColumn gridView = new DataGridViewTextBoxColumn();
                    gridView.SortMode = DataGridViewColumnSortMode.Programmatic;
                    gridView.Name = item.Key;
                    gridView.HeaderText = item.Key;
                    dataGridView1.Columns.Add(gridView);
                }
            }
            catch (Exception)
            {
            }
        }

        private void ProcessControl_Click(object sender, EventArgs e)
        {
            try
            {
                string item = dataGridView1.Rows[this.dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                if (ProcessUser.Instancen.ProductMessage.ContainsKey(item))
                {
                    ProcessUser.Instancen.ProductMessage.Remove(item);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public ProcessUser Proce;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            UpRest();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否删除？", "删除全部过程数据", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                {
                    ProcessUser.ClearAll();

                    Up();
                }
            }
            catch (Exception)
            {
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (e.TabPageIndex == 2)
                {
                    dataGridView3.Rows.Clear();
                    List<string> tss = new List<string>();
                    //dataGridView3.Columns.Clear();
                    for (int i = 0; i < this.dataGridView3.Columns.Count; i++)
                    {
                        if (tss.Contains(dataGridView3.Columns[i].HeaderText))
                        {
                            this.dataGridView3.Columns.RemoveAt(i);
                            continue;
                        }
                        tss.Add(dataGridView3.Columns[i].HeaderText);
                    }
                    foreach (var item in Proce.GetData())
                    {
                        it = dataGridView3.Rows.Add();
                        dataGridView3.Rows[it].Cells[0].Value = item.Key;
                        foreach (var item2 in item.Value)
                        {
                            if (!tss.Contains(item2.Key))
                            {
                                DataGridViewTextBoxColumn dataGridViewColumn = new DataGridViewTextBoxColumn();
                                dataGridViewColumn.Name = dataGridViewColumn.HeaderText = item2.Key;
                                dataGridView3.Columns.Add(dataGridViewColumn);
                                dataGridView3[item2.Key, it].Value = item2.Value;
                            }
                            dataGridView3[item2.Key, it].Value = item2.Value;
                        }
                    }
                }
                else if (e.TabPageIndex == 1)
                {
                    //if (ErosSocket.ErosConLink.StaticCon.GetLingkNameValue(ProcessUser.GetThis().EapEnName))
                    //{
                    //    button2.BackColor = Color.GreenYellow;
                    //}
                    //else
                    //{
                    //    button2.BackColor = Color.Red;
                    //}
                    if (ProcessUser.Instancen.LinkFEap)
                    {
                        button12.BackColor = Color.Green;
                    }
                    else
                    {
                        button12.BackColor = Color.Red;
                    }
                    textBox4.Text = ErosSocket.ErosConLink.StaticCon.GetLingkNameValueString(ProcessUser.Instancen.EapGetQRCodeName);
                    //ErosSocket.ErosConLink.StaticCon.GetLingkIDValueS("打码清洗.DB17.4", "String", 100, out dynamic ert);
                    //textBox5.Text = ert;
                    ErosSocket.ErosConLink.StaticCon.GetLingkIDValueS("打码清洗.DB17.106", "String", 100, out dynamic ert);
                    textBox6.Text = ert;
                }
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessUser.SetCodeProValue(textBox9.Text,
                    new string[] { "载具码", "位置", "OK", "位移检测" }
                , new string[] {textBox8.Text,comboBox3.SelectedItem.ToString(),
                    comboBox1.SelectedItem.ToString(),
                    comboBox2.SelectedItem.ToString() });
            }
            catch (Exception)
            {
            }
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ErosSocket.ErosConLink.StaticCon.GetLingkNameValue(ProcessUser.GetThis().EapEnName))
        //        {
        //            ErosSocket.ErosConLink.StaticCon.SetLingkValue(ProcessUser.GetThis().EapEnName, false, out string err);
        //            button2.BackColor = Color.Red;
        //        }
        //        else
        //        {
        //            ErosSocket.ErosConLink.StaticCon.SetLingkValue(ProcessUser.GetThis().EapEnName, true, out string err);
        //            button2.BackColor = Color.GreenYellow;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue("打码清洗.DB17.4",ErosSocket.ErosConLink.UClass.String,textBox5.Text);
                ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue("打码清洗.DB17.106", ErosSocket.ErosConLink.UClass.String, textBox6.Text);
                //ErosSocket.ErosConLink.StaticCon.GetLingkIDValueS("打码清洗.DB17.4", "String", 100, out dynamic ert);
                //textBox5.Text = ert;
                ErosSocket.ErosConLink.StaticCon.GetLingkIDValueS("打码清洗.DB17.106", "String", 100, out dynamic ert);
                textBox6.Text = ert;
            }
            catch (Exception)
            {
            }
        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(ProjectINI.ProjectPathRun + "\\过程记录\\");
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (botf)
                {
                    ProcessUser.SetCodeProValue(dataGridView1[0, e.RowIndex].Value.ToString(),
                    dataGridView1.Columns[e.ColumnIndex].HeaderText, dataGridView1[e.ColumnIndex,
                    e.RowIndex].Value.ToString());
                }
            }
            catch (Exception)
            {
            }
            botf = false;
        }

        private bool botf = false;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                botf = true;
                if (e.RowIndex >= 0)
                {
                    textBox9.Text = dataGridView1[0, e.RowIndex].Value.ToString();
                    textBox8.Text = dataGridView1["载具码", e.RowIndex].Value.ToString();
                    comboBox3.SelectedItem = dataGridView1["位置", e.RowIndex].Value.ToString();
                    comboBox1.SelectedItem = dataGridView1["OK", e.RowIndex].Value.ToString();
                    comboBox2.SelectedItem = dataGridView1["位移检测", e.RowIndex].Value.ToString();
                }
            }
            catch (Exception)
            {
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ErosSocket.ErosConLink.StaticCon.SetLingkValue("打码清洗.二维码", textBox1.Text, out string err);
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                ErosSocket.ErosConLink.StaticCon.SetLingkValue("打码清洗.二维码", textBox9.Text, out string err);
            }
            catch (Exception)
            {
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(
            //        Stub.StubManager.getDevice().getCurrentRecipe()
            //);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Dictionary<string,string>dt=
            //       Stub.StubManager.getDevice().GetRecipe("", textBox2.Text);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //bool iscont=
            //Stub.StubManager.getDevice().Switch( textBox3.Text);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessUser.TrayId(textBox4.Text);
            }
            catch (Exception)
            {
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessUser.Instancen.SendTyID(textBox5.Text);
            }
            catch (Exception ex)
            {
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (ProcessUser.Instancen.LinkFEap)
            {
                ProcessUser.Instancen.LinkFEap = false;
                button12.BackColor = Color.Red;
            }
            else
            {
                ProcessUser.Instancen.LinkFEap = true;
                button12.BackColor = Color.Green;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void plcButton1_Click(object sender, EventArgs e)
        {
        }

        private void plcBtn1_Click(object sender, EventArgs e)
        {
        }
    }
}