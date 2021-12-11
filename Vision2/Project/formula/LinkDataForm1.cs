using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Vision2.Project.formula
{
    public partial class LinkDataForm1 : Form
    {
        public LinkDataForm1()
        {
            ICc = true;
            InitializeComponent();
            checkBox1.Checked = RecipeCompiler.Instance.Data.IsChe;
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView2);
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            dataGridView1.CurrentCellDirtyStateChanged += DataGridView1_CurrentCellDirtyStateChanged;
            try
            {
                comboBox1.SelectedIndex = 0;
                for (int i = 0; i < RecipeCompiler.Instance.Data.CheCalssT.Count; i++)
                {
                    listBox1.Items.Add(i + 1);
                }
            }
            catch (Exception)
            {
            }
        }

        private int y;
        private int x;

        private void Tool_Click(object sender, EventArgs e)
        {
            string[] text;
            try
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                text = dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[dataGridView1.CurrentCellAddress.X].Value.ToString().Split('.');
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
                    string[] keys = dataGridView1.Rows[Y].Cells[X].Value.ToString().Split('.');
                    foreach (var item2 in RecipeCompiler.Instance.ProductEX[Product.ProductionName].Key_Navigation_Picture[text[0]].KeyRoi.Keys)
                    {
                        ToolStripMenuItem tool = new ToolStripMenuItem();
                        tool.Text = item2;
                        tool.Click += Tool_Click;
                        contextMenuStrip.Items.Add(tool);
                    }
                    if (keys.Length == 2 && keys[1].Length == 0)
                    {
                        Rectangle rectangle = dataGridView1.GetCellDisplayRectangle(x, y, false);
                        Rectangle rectangle2 = dataGridView1.RectangleToScreen(rectangle);
                        contextMenuStrip.Show(rectangle2.X, rectangle2.Y + this.dataGridView1.Rows[x].Height);
                    }
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

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ErosProjcetDLL.UI.UICon.GetCursorPos(out ErosProjcetDLL.UI.UICon.POINT pOINT);
                int X = dataGridView1.CurrentCellAddress.X;
                int Y = dataGridView1.CurrentCellAddress.Y;
                if (X != 1)
                {
                    return;
                }
                if ((Y != -1 && X != -1) && dataGridView1.Rows[Y].Cells[X] != null && dataGridView1.Rows[Y].Cells[X].Value != null)
                {
                    string[] keys = new string[] { };
                    ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                    if (dataGridView1.Rows[Y].Cells[X].Value.ToString().Contains("."))
                    {
                        keys = dataGridView1.Rows[Y].Cells[X].Value.ToString().Split('.');
                        if (keys.Length == 1)
                        {
                            if (RecipeCompiler.GetProductEX().Key_Navigation_Picture.ContainsKey(keys[0]))
                            {
                                foreach (var item2 in RecipeCompiler.GetProductEX().Key_Navigation_Picture[keys[0]].KeyRoi.Keys)
                                {
                                    ToolStripMenuItem tool = new ToolStripMenuItem();
                                    tool.Text = item2;
                                    tool.Click += Tool_Click;
                                    contextMenuStrip.Items.Add(tool);
                                }
                            }
                            else
                            {
                                foreach (var item in RecipeCompiler.GetProductEX().Key_Navigation_Picture.Keys)
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
                        }
                        else
                        {
                            if (RecipeCompiler.GetProductEX().Key_Navigation_Picture.ContainsKey(keys[0]) ||
                                RecipeCompiler.GetProductEX().Key_Navigation_Picture[keys[0]].KeyRoi.ContainsKey(keys[1]))
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in RecipeCompiler.GetProductEX().Key_Navigation_Picture.Keys)
                        {
                            ToolStripMenuItem tool = new ToolStripMenuItem();
                            tool.Text = item;
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
            catch (Exception es) { }
        }

        private void DataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (ICc)
                {
                    return;
                }
                if (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        ListData.ListDatV[i].ComponentName = dataGridView1.Rows[i].Cells[0].Value.ToString();

                        if (dataGridView1.Rows[i].Cells[1].Value != null)
                        {
                            ListData.ListDatV[i].RunNameOBJ = (dataGridView1.Rows[i].Cells[1].Value.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private LinkData ListData;
        private bool ICc;

        public void SetData(LinkData data)
        {
            ListData = data;
            ICc = true;
            ListData.EventAddValue -= LinkD_EventAddValue;
            ListData.EventAddValue += LinkD_EventAddValue;
            try
            {
                dataGridView1.Rows.Clear();
                while (dataGridView1.Rows.Count < ListData.ListDatV.Count)
                {
                    dataGridView1.Rows.Add();
                }
                for (int i = 0; i < ListData.ListDatV.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = ListData.ListDatV[i].ComponentName;
                    dataGridView1.Rows[i].Cells[1].Value = ListData.ListDatV[i].RunNameOBJ;
                    dataGridView1.Rows[i].Cells[0].Tag = ListData.ListDatV[i];
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ICc = false;
        }

        private void LinkD_EventAddValue(List<DataMinMax> text)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    if (text.Count == 0)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            //dataGridView1.Rows[i].Cells[3].Value = "";
                            dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Gray;
                        }
                    }
                    if (dataGridView1.Rows.Count < text.Count)
                    {
                        dataGridView1.Rows.Add(text.Count - dataGridView1.Rows.Count);
                    }

                    for (int i = 0; i < text.Count; i++)
                    {
                        if (i < ListData.ListDatV.Count)
                        {
                            string datas = "";
                            for (int i2 = 0; i2 < ListData.ListDatV[i].ValueStrs.Count; i2++)
                            {
                                datas += ListData.ListDatV[i].ValueStrs[i2] + ',';
                            }
                            dataGridView1.Rows[i].Cells[2].Value = datas;
                            int rset = ListData.ListDatV[i].GetRset();
                            if (rset == 0)
                            {
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Gray;
                            }
                            else if (rset == 1)
                            {
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Green;
                            }
                            else
                            {
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Red;
                            }
                        }
                        dataGridView1.Rows[i].Cells[0].Tag = text[i];
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LinkDataForm1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                RecipeCompiler.GetProductEX().ListDicData = ListData.ListDatV;

                ListData.EventAddValue -= LinkD_EventAddValue;
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Gray;
                    dataGridView1.Rows[i].Cells[2].Value = "";
                }
                for (int i = 0; i < ListData.ListDatV.Count; i++)
                {
                    ListData.ListDatV[i].Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void 参数覆盖ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string dat = dataGridView1.SelectedCells[dataGridView1.SelectedCells.Count - 1].Value.ToString();
                int dset = dataGridView1.SelectedCells[0].ColumnIndex;
                int des = 0;
                string NmaesTr = dat;
                if (dset == 0)
                {
                    des = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(dat, out NmaesTr);
                }
                if (dset == 4)
                {
                    des = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(dat, out NmaesTr);
                }
                for (int i = dataGridView1.SelectedCells.Count - 1; i >= 0; i--)
                {
                    string data = NmaesTr;
                    if (dset == 0 || dset == 4)
                    {
                        data = NmaesTr + (des++);
                    }
                    dataGridView1.SelectedCells[i].Value = data;
                }
            }
            catch (Exception ex) { }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RecipeCompiler.Instance.Data.IsChe = checkBox1.Checked;
                groupBox1.Enabled = RecipeCompiler.Instance.Data.IsChe;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RecipeCompiler.Instance.Data.CheCalssT.Add(new LinkData.CheCalss());
            listBox1.Items.Add(RecipeCompiler.Instance.Data.CheCalssT.Count - 1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.RemoveAt(RecipeCompiler.Instance.Data.CheCalssT.Count - 1);
            RecipeCompiler.Instance.Data.CheCalssT.RemoveAt(RecipeCompiler.Instance.Data.CheCalssT.Count - 1);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = RecipeCompiler.Instance.Data.CheCalssT[listBox1.SelectedIndex];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Random rd = new Random();
            double i = rd.NextDouble();
            RecipeCompiler.Instance.Data.AddData(i);
            DebugF.DebugCompiler.GetTray(0).GetITrayRobot().SetValue(i);

            DebugF.DebugCompiler.GetTray(0).Number++;
            if (DebugF.DebugCompiler.GetTray(0).Count < DebugF.DebugCompiler.GetTray(0).Number)
            {
                DebugF.DebugCompiler.GetTray(0).Number = 1;
            }
            numericUpDown3.Value = DebugF.DebugCompiler.GetTray(0).Number;
        }

        public double NextDouble(Random ran, double minValue, double maxValue)
        {
            return ran.NextDouble() * (maxValue - minValue) + minValue;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                List<double> vs = new List<double>();
                int nuber = (int)numericUpDown2.Value;
                Random rd = new Random();
                string dataStr = "";
                if (comboBox1.SelectedIndex == 0)
                {
                    for (int i = 0; i < nuber; i++)
                    {
                        vs.Add(rd.NextDouble());
                    }
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    for (int i = 0; i < nuber; i++)
                    {
                        if (RecipeCompiler.Instance.Data.ListDatV.Count >= nuber)
                        {
                            for (int id = 0; id < RecipeCompiler.Instance.Data.ListDatV[i].Reference_Name.Count; id++)
                            {
                                vs.Add(NextDouble(rd, RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMin[id], RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMax[id]));
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < nuber; i++)
                    {
                        if (RecipeCompiler.Instance.Data.ListDatV.Count >= nuber)
                        {
                            for (int id = 0; id < RecipeCompiler.Instance.Data.ListDatV[i].Reference_Name.Count; id++)
                            {
                                vs.Add(NextDouble(rd, RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMax[id], RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMax[id] + 5));
                            }
                        }
                    }
                }
                for (int i = 0; i < vs.Count; i++)
                {
                    dataStr += vs[i] + ",";
                }
                DebugF.DebugCompiler.DebugData(dataStr);
                //DebugF.DebugCompiler.GetTrayDataUserControl().SetValue(vs);
                DebugF.DebugCompiler.GetTray(0).Number++;
                numericUpDown3.Value = DebugF.DebugCompiler.GetTray(0).Number;
                RecipeCompiler.Instance.Data.AddData(vs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                DebugF.DebugCompiler.GetTray(0).Number = 1;
                numericUpDown3.Value = DebugF.DebugCompiler.GetTray(0).Number;
                //DebugF.DebugCompiler.GetTrayDataUserControl().GetTrayEx().RestValue();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                DebugF.DebugCompiler.GetTray(0).GetTrayData().
                    WriatTary(ProcessControl.ProcessUser.Instancen.ExcelPath + "\\",
                    "{文件名= [newtime]-[trayid];}",
                    DebugF.DebugCompiler.GetTray(0).GetTrayData(), out string err);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (ErosSocket.ErosConLink.StaticCon.GetSocketClint(RecipeCompiler.Instance.RsetLinkName) != null)
                {
                    string jsonStr = JsonConvert.SerializeObject(DebugF.DebugCompiler.GetTray(0).GetTrayData());
                    ErosSocket.ErosConLink.StaticCon.GetSocketClint(RecipeCompiler.Instance.RsetLinkName).Send("Tray" + jsonStr);
                }
                else
                {
                    MessageBox.Show("通信连接不存在!" + RecipeCompiler.Instance.RsetLinkName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DebugF.DebugCompiler.GetTray(0).Number = (int)numericUpDown3.Value;
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataMinMax datat = new DataMinMax();
                if (data != null)
                {
                    datat = new DataMinMax()
                    {
                        ComponentName = "元件" + (ListData.ListDatV.Count + 1),
                        //Reference_Name = data.Reference_Name,
                        //Reference_ValueMax = data.Reference_ValueMax,
                        //Reference_ValueMin = data.Reference_ValueMin,
                        RunNameOBJ = data.RunNameOBJ,
                    };
                    datat.Reference_Name.AddRange(data.Reference_Name);
                    datat.Reference_ValueMax.AddRange(data.Reference_ValueMax);
                    datat.Reference_ValueMin.AddRange(data.Reference_ValueMin);
                }

                datat.ComponentName = "元件" + (ListData.ListDatV.Count + 1);

                ListData.ListDatV.Add(datat);
                int d = dataGridView1.Rows.Add();
                dataGridView1.Rows[d].Cells[0].Value = datat.ComponentName;
                dataGridView1.Rows[d].Cells[1].Value = datat.RunNameOBJ;
                dataGridView1.Rows[d].Cells[0].Tag = datat;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void 删除元件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
     
                int de = dataGridView1.SelectedRows.Count;
                for (int i = 0; i < de; i++)
                {

                   ListData.ListDatV.RemoveAt(dataGridView1.SelectedRows[0].Index);
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                }
            
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void dataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                MoveFlag = true;//已经按下.
                xPos = e.X;//当前x坐标.
                yPos = e.Y;//当前y坐标.
            }
            catch (Exception)
            {
            }
        }

        private int xPos;
        private int yPos;
        private bool MoveFlag;

        private void dataGridView2_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (MoveFlag)
                {
                    dataGridView2.Left += Convert.ToInt16(e.X - xPos);//设置x坐标.
                    dataGridView2.Top += Convert.ToInt16(e.Y - yPos);//设置y坐标.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void dataGridView2_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            MoveFlag = false;
        }

        private DataMinMax data;

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
        }

        private void 添加数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                iscavet = true;
                int det = data.Reference_Name.Count;
                data.Reference_Name.Add("点" + (data.Reference_Name.Count + 1));
                data.Reference_ValueMax.Add(10);
                data.Reference_ValueMin.Add(0);
                if (det >= dataGridView2.Rows.Count)
                {
                    det = dataGridView2.Rows.Add();
                }
                dataGridView2.Rows[det].Cells[0].Value = data.Reference_Name[data.Reference_Name.Count - 1];
                dataGridView2.Rows[det].Cells[1].Value = data.Reference_ValueMin[data.Reference_Name.Count - 1];
                dataGridView2.Rows[det].Cells[3].Value = data.Reference_ValueMax[data.Reference_Name.Count - 1];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
            iscavet = false;
        }

        private void 删除数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int d = dataGridView2.SelectedCells.Count;
                for (int i = 0; i < d; i++)
                {
                    data.Reference_Name.RemoveAt(dataGridView2.SelectedCells[0].RowIndex);
                    data.Reference_ValueMax.RemoveAt(dataGridView2.SelectedCells[0].RowIndex);
                    data.Reference_ValueMin.RemoveAt(dataGridView2.SelectedCells[0].RowIndex);
                    dataGridView2.Rows.RemoveAt(dataGridView2.SelectedCells[0].RowIndex);
                }
            }
            catch (Exception)
            {
            }
        }

        private bool iscavet;

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (iscavet)
                {
                    return;
                }
                if (data == null)
                {
                    return;
                }
                for (int i = 0; i < data.Reference_Name.Count; i++)
                {
                    data.Reference_Name[i] = dataGridView2.Rows[i].Cells[0].Value.ToString();
                    if (dataGridView2.Rows[i].Cells[1].Value != null)
                    {
                        data.Reference_ValueMin[i] = double.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString());
                    }
                    if (dataGridView2.Rows[i].Cells[3].Value != null)
                    {
                        data.Reference_ValueMax[i] = double.Parse(dataGridView2.Rows[i].Cells[3].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                iscavet = true;
                dataGridView2.Visible = true;
                dataGridView2.Rows.Clear();
                if (e.RowIndex>=0)
                {
                    data = dataGridView1.Rows[e.RowIndex].Cells[0].Tag as DataMinMax;
                    if (data.Reference_Name.Count != 0)
                    {
                        int cont = data.Reference_Name.Count;
                        if (data.ValueStrs.Count > cont)
                        {
                            cont = data.ValueStrs.Count;
                        }
                        dataGridView2.Rows.Add(cont);
                        for (int i = 0; i < cont; i++)
                        {
                            if (data.Reference_Name.Count > i)
                            {
                                dataGridView2.Rows[i].Cells[0].Value = data.Reference_Name[i];
                                dataGridView2.Rows[i].Cells[1].Value = data.Reference_ValueMin[i];
                                dataGridView2.Rows[i].Cells[3].Value = data.Reference_ValueMax[i];
                                int resInt = data.GetRsetNumber(i);
                                if (resInt == 0)
                                {
                                    //dataGridView2.Rows[i].Cells[2].Style.BackColor = Color.Red;
                                }
                                else if (resInt == -1)
                                {
                                    dataGridView2.Rows[i].Cells[2].Style.BackColor = Color.Red;
                                }
                                else if (resInt == -2)
                                {
                                    dataGridView2.Rows[i].Cells[1].Style.BackColor = Color.Red;
                                }
                                else if (resInt == -3)
                                {
                                    dataGridView2.Rows[i].Cells[3].Style.BackColor = Color.Red;
                                }
                            }
                            if (data.ValueStrs.Count > i)
                            {
                                dataGridView2.Rows[i].Cells[2].Value = data.ValueStrs[i];
                            }
                        }
                    }
                }
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
            iscavet = false;
        }

        private void LinkDataForm1_Load(object sender, EventArgs e)
        {
            LinkD_EventAddValue(ListData.ListDatV);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                int nuber = (int)numericUpDown2.Value;
                Random rd = new Random();
                string dataStr = numericUpDown3.Value.ToString() + ";" + numericUpDown4.Value.ToString() + ";";

                for (int ie = 0; ie < numericUpDown1.Value; ie++)
                {
                    List<double> vs = new List<double>();
                    if (comboBox1.SelectedIndex == 0)
                    {
                        for (int i = 0; i < nuber; i++)
                        {
                            vs.Add(rd.NextDouble());
                        }
                    }
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        for (int i = 0; i < nuber; i++)
                        {
                            if (RecipeCompiler.Instance.Data.ListDatV.Count >= nuber)
                            {
                                for (int id = 0; id < RecipeCompiler.Instance.Data.ListDatV[i].Reference_Name.Count; id++)
                                {
                                    vs.Add(NextDouble(rd, RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMin[id], RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMax[id]));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < nuber; i++)
                        {
                            if (RecipeCompiler.Instance.Data.ListDatV.Count >= nuber)
                            {
                                for (int id = 0; id < RecipeCompiler.Instance.Data.ListDatV[i].Reference_Name.Count; id++)
                                {
                                    vs.Add(NextDouble(rd, RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMax[id], RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMax[id] + 5));
                                }
                            }
                        }
                    }
                    for (int i = 0; i < vs.Count; i++)
                    {
                        dataStr += vs[i] + ",";
                    }
                    dataStr += ";";
                }
                DebugF.DebugCompiler.DebugData(dataStr);
                //DebugF.DebugCompiler.GetTrayDataUserControl().SetValue(vs);
                //DebugF.DebugCompiler.GetTrayDataUserControl().GetTrayEx().Number++;
                //numericUpDown3.Value = DebugF.DebugCompiler.GetTrayDataUserControl().GetTrayEx().Number;
                //RecipeCompiler.Instance.Data.AddData(vs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 导出参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ErosProjcetDLL.Project.ProjectINI.Weait(RecipeCompiler.Instance.Data);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 导入参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LinkData linkData = new LinkData();
                ErosProjcetDLL.Project.ProjectINI.ReadWeait(out linkData);
                RecipeCompiler.Instance.Data.ListDatV = linkData.ListDatV;
                RecipeCompiler.Instance.Data.CheCalssT = linkData.CheCalssT;
                ListData.ListDatV = RecipeCompiler.Instance.Data.ListDatV;
                SetData(RecipeCompiler.Instance.Data);
                LinkD_EventAddValue(ListData.ListDatV);
                MessageBox.Show("导入成功");
 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                int nuber = (int)numericUpDown2.Value;
                Random rd = new Random();
                string dataStr = "";/* numericUpDown3.Value.ToString() + ";" + numericUpDown4.Value.ToString() + ";";*/

                for (int ie = 0; ie < 1; ie++)
                {
                    List<double> vs = new List<double>();
                    if (comboBox1.SelectedIndex == 0)
                    {
                        for (int i = 0; i < nuber; i++)
                        {
                            vs.Add(rd.NextDouble());
                        }
                    }
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        for (int i = 0; i < nuber; i++)
                        {
                            if (RecipeCompiler.Instance.Data.ListDatV.Count >= nuber)
                            {
                                for (int id = 0; id < RecipeCompiler.Instance.Data.ListDatV[i].Reference_Name.Count; id++)
                                {
                                    vs.Add(NextDouble(rd, RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMin[id], RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMax[id]));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < nuber; i++)
                        {
                            if (RecipeCompiler.Instance.Data.ListDatV.Count >= nuber)
                            {
                                for (int id = 0; id < RecipeCompiler.Instance.Data.ListDatV[i].Reference_Name.Count; id++)
                                {
                                    vs.Add(NextDouble(rd, RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMax[id], RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMax[id] + 5));
                                }
                            }
                        }
                    }
                    for (int i = 0; i < vs.Count; i++)
                    {
                        dataStr += vs[i] + ",";
                    }
              
                }

                DebugF.DebugCompiler.DebugData(dataStr.Trim(';'));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                string[] striparr = richTextBox1.Text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                striparr = striparr.Where(s => !string.IsNullOrEmpty(s)).ToArray();

               

            List<string> striparrTd = richTextBox1.Text.Split(new string[] { Environment.NewLine},
                StringSplitOptions.RemoveEmptyEntries).ToList();

                //striparr = striparr.Where(s => !string.IsNullOrEmpty(s)).ToList();
                for (int i = 0; i < striparr.Length; i++)
                {
                    DebugF.DebugCompiler.DebugData(striparr[i]);
                }
               
            }
            catch (Exception)
            {
            }
 
        }
    }
}