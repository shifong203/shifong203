using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC.Robot
{
    public partial class SteppingControl : UserControl
    {
        public SteppingControl()
        {
            InitializeComponent();
            Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView3);
            dataGridView1.Rows.Add(16);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = i;
                dataGridView1.Rows[i].Cells[1].Value = i;
            }
        }

        public SteppingControl(IAxisGrub axis4PD) : this()
        {
            SetAxiss(axis4PD);
            numericUpDown3.Value = (decimal)axis4PD.JoupZ;
        }

        IAxisGrub Axiss;
        EpsenRobot6 EpsenRobot;
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 只能执行一次关联参数
        /// </summary>
        /// <param name="axis4PD"></param>
        public void SetAxiss(IAxisGrub axis4PD)
        {
            try
            {

                //dataGridView3.Rows.Count
                Control axis4PUserControl11;
                Axiss = axis4PD;
                if (Axiss is EpsenRobot6)
                {
                    EpsenRobot = Axiss as EpsenRobot6;

                }
                if (Axiss.AxisNumber == 6)
                {
                    Robot.Axis6PUserControl axis4PUserContro = new Axis6PUserControl();
                    axis4PUserContro.setAxisGrud(Axiss);
                    axis4PUserControl11 = axis4PUserContro;

                }
                else
                {
                    Axis4PUserControl1 axis4PUserContro = new Axis4PUserControl1();
                    axis4PUserContro.setAxisGrud(Axiss);
                    axis4PUserControl11 = axis4PUserContro;
                    //tabControl2.TabPages.Remove(tabPage2);
                }

                for (int i = 0; i < Axiss.GetRunCode().Count; i++)
                {
                    //contextMenuStrip2.Items.Add(Axiss.GetRunCode()[i]).Click += SteppingControl_Click;
                    ToolStripButton toolStripItem = new ToolStripButton();
                    toolStripItem.Text = toolStripItem.Name = Axiss.GetRunCode()[i];
                    toolStripItem.Click += SteppingControl_Click;
                    contextMenuStrip2.Items.Insert(0, toolStripItem);
                }


                comboBox4.SelectedItem = 0;
                axis4PUserControl11.Dock = DockStyle.Fill;
                groupBox2.Dock = DockStyle.Fill;
                groupBox2.Controls.Add(axis4PUserControl11);
                System.Threading.Thread thread = new System.Threading.Thread(() =>
                {
                    while (!this.IsDisposed)
                    {
                        try
                        {
                            this.Invoke(new Action(() => { UPDataAx(); }));
                            System.Threading.Thread.Sleep(200);
                        }
                        catch (Exception)
                        {
                        }
                    }
                });
                thread.IsBackground = true;
                thread.Start();

            }
            catch (Exception)
            {
            }
        }

        private void SteppingControl_Click(object sender, EventArgs e)
        {
            try
            {

                ToolStripItem sdfs = (ToolStripItem)sender;
                int sd = dataGridView2.SelectedCells[0].RowIndex;
                Single? x, y, z, u, v, w;
                uint heds, wrist, elbow, j1f, j4f, j6f;
                x = Convert.ToSingle(dataGridView2.Rows[sd].Cells[2].Value);
                y = Convert.ToSingle(dataGridView2.Rows[sd].Cells[3].Value);
                z = Convert.ToSingle(dataGridView2.Rows[sd].Cells[4].Value);
                u = Convert.ToSingle(dataGridView2.Rows[sd].Cells[5].Value);
                v = Convert.ToSingle(dataGridView2.Rows[sd].Cells[6].Value);
                w = Convert.ToSingle(dataGridView2.Rows[sd].Cells[7].Value);
                heds = Convert.ToUInt16(dataGridView2.Rows[sd].Cells[9].Value);
                elbow = Convert.ToUInt16(dataGridView2.Rows[sd].Cells[10].Value);
                wrist = Convert.ToUInt16(dataGridView2.Rows[sd].Cells[11].Value);
                j1f = Convert.ToUInt16(dataGridView2.Rows[sd].Cells[12].Value);
                j4f = Convert.ToUInt16(dataGridView2.Rows[sd].Cells[13].Value);
                j6f = Convert.ToUInt16(dataGridView2.Rows[sd].Cells[14].Value);
                if (Axiss is EpsenRobot6)
                {

                    ((EpsenRobot6)Axiss).SetPoint(sdfs.Text, Convert.ToUInt16(dataGridView2.Rows[sd].Cells[0].Value),
                        (Single)x, (Single)y, z, u, v, w, heds, elbow, wrist, j1f, j4f, j6f);
                }
                else
                {
                    Axiss.SetPoint(sdfs.Text, (Single)x, (Single)y, z, u, v, w);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        bool[] bitArray;
        void UPDataAx()
        {
            try
            {
                if (EpsenRobot != null)
                {
                    if (EpsenRobot.IsConn)
                    {
                        groupBox1.Enabled = true;
                    }
                    else
                    {
                        groupBox1.Enabled = true;
                    }
                    int cot = 16;
                    switch (comboBox4.SelectedIndex)
                    {
                        case 0:
                            bitArray = Axiss.GetIOOuts(true);
                            break;
                        case 1:
                            bitArray = Axiss.GetIOOuts();
                            break;
                        case 2:
                            bitArray = Axiss.GetIOOuts();
                            break;
                    }
                    if (bitArray.Length < 16)
                    {
                        cot = bitArray.Length;
                    }
                    while (bitArray.Length < dataGridView1.Rows.Count)
                    {
                        dataGridView1.Rows.RemoveAt(bitArray.Length);
                    }

                    while (cot > dataGridView1.Rows.Count)
                    {
                        dataGridView1.Rows.Add();
                    }
                    for (int i = 0; i < cot; i++)
                    {
                        dataGridView1.Rows[i].Cells[2].Value = bitArray[i];
                        if (bitArray[i])
                        {
                            dataGridView1.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[1].Style.ForeColor = Color.Black;
                        }
                    }
                }
                else
                {
                    if (Axiss.ListPLCIO != null)
                    {
                        if (Axiss.ListPLCIO.Count == 0)
                        {
                            tabPage2.Dispose();
                        }
                        else
                        {
                            while (Axiss.ListPLCIO.Count < dataGridView1.Rows.Count)
                            {
                                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                            }
                            while (Axiss.ListPLCIO.Count > dataGridView1.Rows.Count)
                            {
                                dataGridView1.Rows.Add();

                            }
                            for (int i = 0; i < Axiss.ListPLCIO.Count; i++)
                            {
                                dataGridView1.Rows[i].Cells[1].Value = Axiss.ListPLCIO[i];
                                if (DebugComp.GetThis().DicPLCIO.ContainsKey(Axiss.ListPLCIO[i]))
                                {
                                    dataGridView1.Rows[i].Cells[0].Value = DebugComp.GetThis().DicPLCIO[Axiss.ListPLCIO[i]].LinkName + "." + DebugComp.GetThis().DicPLCIO[Axiss.ListPLCIO[i]].Addrea;

                                    if (ErosSocket.ErosConLink.StaticCon.GetLingkIDValue(DebugComp.GetThis().DicPLCIO[Axiss.ListPLCIO[i]], out dynamic value))
                                    {
                                        dataGridView1.Rows[i].Cells[2].Value = value;
                                        if (value.ToString() == false.ToString())
                                        {
                                            dataGridView1.Rows[i].Cells[2].Style.ForeColor = Color.Red;
                                        }
                                        else
                                        {
                                            dataGridView1.Rows[i].Cells[2].Style.ForeColor = Color.Black;
                                        }
                                    }
                                    if (DebugComp.GetThis().DicPLCIO[Axiss.ListPLCIO[i]].TypeStr == "Boolean")
                                    {
                                        dataGridView1.Rows[i].ReadOnly = true;
                                    }
                                }
                                else
                                {
                                    dataGridView1.Rows[i].Cells[1].ReadOnly = false;
                                }
                            }
                        }



                    }
                    if (Axiss.ListCylin != null)
                    {

                    }
                }

            }
            catch (Exception EX)
            {
            }


        }
        private void butGetPoint_Click(object sender, EventArgs e)
        {
            try
            {
                string texty = "请输入点名";
                if (comboBox1.SelectedItem.ToString() == " ")
                {
                    string nameStr = "点名称";
                str:


                    nameStr = Interaction.InputBox(texty, "点名称", nameStr, 100, 100);
                    if (nameStr != "")
                    {
                        if (int.TryParse(nameStr[0].ToString(), out int det))
                        {
                            texty = "首位不可以包含数字 ";
                            goto str;
                        }
                        if (PointFile.IsPointContainsKey(comboBox3.SelectedItem.ToString(), nameStr))
                        {
                            texty = "名称已存在，请重新输入点文件名";
                            goto str;
                        }
                        else
                        {
                            name = nameStr;
                        }
                        Axiss.SetPointPFile(comboBox3.SelectedItem.ToString(), nameStr);
                        PointFile.GetPointDataG(comboBox3.SelectedItem.ToString(), dataGridView2);
                        comboBox1.Items.Clear();
                        for (int i = 0; i < PointFile.GetPointFile(Axiss.PointFileName).Count; i++)
                        {
                            if (PointFile.GetPointFile(Axiss.PointFileName)[i].Name != null && PointFile.GetPointFile(Axiss.PointFileName)[i].Name != "")
                            {
                                comboBox1.Items.Add(PointFile.GetPointFile(Axiss.PointFileName)[i].Name);
                            }
                        }
                        comboBox1.Items.Add(" ");
                        comboBox1.SelectedItem = nameStr;
                        einbe = false;
                    }
                }
                else
                {
                    DialogResult dialog = MessageBox.Show("是否试教点" + comboBox1.SelectedItem.ToString(), "试教点位", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        Axiss.SetPointPFile(comboBox3.SelectedItem.ToString(), comboBox1.SelectedItem.ToString());
                        PointFile.GetPointDataG(comboBox3.SelectedItem.ToString(), dataGridView2);
                        einbe = false;
                    }


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void butStop_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                Axiss.SetPoint(comboBox2.SelectedItem.ToString(), comboBox1.SelectedItem.ToString());
            }
            catch (Exception)
            {
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Axiss.SetHome();

        }

        private void butReset_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void SteppingControl_Load(object sender, EventArgs e)
        {
            try
            {

                PointFile.GetAxisGrubXY().GetPoint(dataGridView3);
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                for (int i = 0; i < dataGridView2.ColumnCount; i++)
                {
                    dataGridView2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                comboBox2.SelectedItem = 0;
                comboBox4.SelectedIndex = 0;
                name = Axiss.PointFileName;
                comboBox3.Items.Clear();
                listBox1.Items.AddRange(PointFile.GetPointFile().Keys.ToArray());
                comboBox3.Items.AddRange(PointFile.GetPointFile().Keys.ToArray());
                numericUpDown3.Value = (decimal)Axiss.JoupZ;

                if (Axiss.PointFileName == null)
                {
                    comboBox3.SelectedIndex = 0;
                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    comboBox3.SelectedItem = Axiss.PointFileName;
                }
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(Axiss.GetRunCode().ToArray());
                comboBox2.SelectedIndex = 0;
                PointFile.GetPointDataG(comboBox3.SelectedItem.ToString(), dataGridView2);
                einbe = false;
                UpDataS();

            }
            catch (Exception)
            {


            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox3.SelectedItem != null)
                {
                    Axiss.PointFileName = comboBox3.SelectedItem.ToString();
                    comboBox1.Items.Clear();
                    for (int i = 0; i < PointFile.GetPointFile(Axiss.PointFileName).Count; i++)
                    {
                        if (PointFile.GetPointFile(Axiss.PointFileName)[i].Name != null && PointFile.GetPointFile(Axiss.PointFileName)[i].Name != "")
                        {
                            comboBox1.Items.Add(PointFile.GetPointFile(Axiss.PointFileName)[i].Name);
                        }
                    }
                    comboBox1.Items.Add(" ");
                    PointFile.GetPointDataG(comboBox3.SelectedItem.ToString(), dataGridView2);

                    comboBox1.SelectedIndex = 0;
                    name = comboBox3.SelectedItem.ToString();
                }
                einbe = false;
            }
            catch (Exception)
            {

            }


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (PointFile.SavePoint())
                {
                    MessageBox.Show("保存成功");
                }
                else
                {
                    MessageBox.Show("保存失败");
                }

            }
            catch (Exception)
            {
            }
        }
        bool einbe = false;
        string name = "";
        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            EnbeData(name);
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            einbe = true;
            try
            {
                if (dataGridView2.Rows[e.RowIndex].Cells[0].Value == null || dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString() == "")
                {
                    dataGridView2.Rows[e.RowIndex].Cells[0].Value = e.RowIndex;
                }
            }
            catch (Exception)
            {


            }

        }
        /// <summary>
        /// ji
        /// </summary>
        /// <param name="name"></param>
        void EnbeData(string name)
        {
            try
            {
                PointFile.SetPointDatag(name, dataGridView2);

                if (EpsenRobot != null)
                {
                    Dictionary<string, string> item = new Dictionary<string, string>();

                    for (int i = 0; i < dataGridView4.Rows.Count; i++)
                    {
                        if (dataGridView4.Rows[i].Cells[0].Value != null && dataGridView4.Rows[i].Cells[0].Value != ""
                            && dataGridView4.Rows[i].Cells[1].Value != null && dataGridView4.Rows[i].Cells[1].Value != "")
                        {
                            item.Add(dataGridView4.Rows[i].Cells[0].Value.ToString(), dataGridView4.Rows[i].Cells[1].Value.ToString());
                        }
                    }
                    EpsenRobot.DicSendMeseage = item;
                }
                einbe = false;
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
                Axiss.JoupZ = (Single)numericUpDown3.Value;
            }
            catch (Exception)
            {
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    return;
                }

                if (einbe)
                {
                    if (MessageBox.Show("点位已修改未应用修改", "修改点位", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        EnbeData(name);
                    }
                }
                if (PointFile.GetPointFile().ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    //PointFile.GetPointDataG(listBox1.SelectedItem.ToString(), dataGridView2);
                    name = listBox1.SelectedItem.ToString();
                    comboBox3.SelectedItem = name;
                    einbe = false;

                }
                else
                {
                    MessageBox.Show(listBox1.SelectedItem.ToString() + "已丢失");
                }
            }
            catch (Exception)
            {
            }

        }

        private void 新建文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string nameStr = Interaction.InputBox("请输入点文件名", "新建点文件", "点文件1", 100, 100);
            str:
                if (PointFile.GetPointFile().ContainsKey(nameStr))
                {
                    nameStr = Interaction.InputBox("请重新输入点文件名", "名称已存在", nameStr, 100, 100);
                    goto str;
                }
                else
                {
                    name = nameStr;
                    listBox1.Items.Add(nameStr);
                    PointFile.GetPointFile().Add(nameStr, new List<PointFile>());
                }
            }
            catch (Exception)
            {
            }
        }

        private void 删除点文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (PointFile.GetPointFile().ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    PointFile.GetPointFile().Remove(listBox1.SelectedItem.ToString());
                }
                if (MessageBox.Show("删除点文件" + listBox1.SelectedItem.ToString(), "是否删除本地文件", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (System.IO.File.Exists(Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\点位文件\\" + listBox1.SelectedItem.ToString() + ".xls"))
                    {
                        System.IO.File.Delete(Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\点位文件\\" + listBox1.SelectedItem.ToString() + ".xls");
                    }
                }
                listBox1.Items.Remove(listBox1.SelectedItem.ToString());
            }
            catch (Exception)
            {

            }




        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void 下载点文件到控制器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Axiss.LoandPointFile(listBox1.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 上载点文件到PCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<PointFile> pointFiles = Axiss.GetFilePoints(listBox1.SelectedItem.ToString());
                if (Axiss.ErrMeaessge != "")
                {
                    MessageBox.Show(Axiss.ErrMeaessge);
                }
                PointFile.GetPointDataG(pointFiles, dataGridView2);
            }
            catch (Exception)
            {
            }

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = i;
                    dataGridView1.Rows[i].Cells[1].Value = i;
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    if (comboBox4.SelectedIndex==0 && e.ColumnIndex==2&&e.RowIndex>-1)
            //    {
            //        if (EpsenRobot != null)
            //        {
            //            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()=="True")
            //            {
            //                EpsenRobot.SendCommand("SetIO", e.RowIndex.ToString(),false.ToString());
            //            }
            //            else
            //            {
            //                EpsenRobot.SendCommand("SetIO", e.RowIndex.ToString(), true.ToString());
            //            }
            //        }
            //        else if (this.Axiss!=null)
            //        {
            //            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "True")
            //            {
            //                this.Axiss.SetIOOut(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), false);
            //            }
            //            else if(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "False")
            //            {
            //                this.Axiss.SetIOOut(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), true);
            //            }

            //        }

            //    }

            //}
            //catch (Exception)
            //{
            //}
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (EpsenRobot.DebugMode)
            {
                EpsenRobot.SendCommand("DebugMode", "False");
            }
            else
            {
                EpsenRobot.SendCommand("DebugMode", "true");
            }
        }

        private void 上载本地点文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<PointFile> pointFiles = Axiss.GetFilePoints();
                if (Axiss.ErrMeaessge != "")
                {
                    MessageBox.Show(Axiss.ErrMeaessge);
                }
                PointFile.GetPointDataG(pointFiles, dataGridView2);
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView2_MouseDown(object sender, MouseEventArgs e)
        {


        }

        private void dataGridView2_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left && e.ColumnIndex == 0 && e.Clicks == 2)
                {
                    dataGridView2.Rows[e.RowIndex].Selected = true;
                }
                if (e.Button == MouseButtons.Right)
                {
                    while (dataGridView2.SelectedCells.Count != 0)
                    {
                        dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Selected = false;
                    }



                    dataGridView2.Rows[e.RowIndex].Selected = true;
                }
                if (e.Button == MouseButtons.Right)
                {
                    for (int i = 0; i < contextMenuStrip2.Items.Count; i++)
                    {
                        contextMenuStrip2.Items[i].Enabled = true;
                    }
                    contextMenuStrip2.Items[contextMenuStrip2.Items.Count - 1].Enabled = true;
                    //if (!Axiss.GetMode())
                    //{
                    //    for (int i = 0; i < contextMenuStrip2.Items.Count; i++)
                    //    {
                    //        contextMenuStrip2.Items[i].Enabled = false;
                    //    }
                    //    contextMenuStrip2.Items[contextMenuStrip2.Items.Count - 1].Enabled = true;
                    //}

                }
            }
            catch (Exception)
            {


            }

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void tsButton5_Click(object sender, EventArgs e)
        {

        }

        private void tsButton4_Click(object sender, EventArgs e)
        {

        }

        private void tsButton3_Click(object sender, EventArgs e)
        {
            try
            {
                PointFile.GetAxisGrubXY().SetPint(dataGridView3);


            }
            catch (Exception)
            {


            }

        }

        private void tsButton2_Click(object sender, EventArgs e)
        {
            PointFile.GetAxisGrubXY().GetPoint(dataGridView3);
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            UpDataS();
        }
        void UpDataS()
        {
            try
            {
                dataGridView3.Rows.Clear();
                dataGridView4.Rows.Clear();
                int d = 0;

                if (EpsenRobot != null)
                {
                    foreach (var item in EpsenRobot.DicSendMeseage)
                    {
                        d = dataGridView4.Rows.Add();
                        dataGridView4.Rows[d].Cells[0].Value = item.Key;
                        dataGridView4.Rows[d].Cells[1].Value = item.Value;
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Axiss is Robot.AxisSPD)
                {
                    if (e.RowIndex == 1)
                    {
                        if (DebugComp.GetThis().DicPLCIO.ContainsKey(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()))
                        {

                            ErosConLink.StaticCon.SetLinkAddressValue(DebugComp.GetThis().DicPLCIO[dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()],
                                dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                        }

                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> list = new List<int>();
                for (int i = 0; i < dataGridView2.SelectedRows.Count; i++)
                {
                    list.Add(dataGridView2.SelectedRows[i].Index);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    PointFile.Remove(comboBox3.SelectedItem.ToString(), dataGridView2.Rows[list[i]].Cells[1].Value.ToString());
                    dataGridView2.Rows.RemoveAt(list[i]);
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (comboBox4.SelectedIndex == 0 && e.ColumnIndex == 2 && e.RowIndex > -1)
                {
                    if (EpsenRobot != null)
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "True")
                        {
                            EpsenRobot.SendCommand("SetIO", e.RowIndex.ToString(), false.ToString());
                        }
                        else
                        {
                            EpsenRobot.SendCommand("SetIO", e.RowIndex.ToString(), true.ToString());
                        }
                    }
                    else if (this.Axiss != null)
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "True")
                        {
                            this.Axiss.SetIOOut(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), false.ToString());
                        }
                        else if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "False")
                        {
                            this.Axiss.SetIOOut(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), true.ToString());
                        }

                    }

                }

            }
            catch (Exception)
            {
            }
        }

        private void ToolStripButton5_Click(object sender, EventArgs e)
        {
            try
            {
                EpsenRobot.SendCommand("DebugMode", true.ToString());

            }
            catch (Exception)
            {

            }
        }

        private void comboBox3_DropDown(object sender, EventArgs e)
        {
            try
            {

                if (comboBox3.SelectedItem != null)
                {
                    Axiss.PointFileName = comboBox3.SelectedItem.ToString();
                    comboBox1.Items.Clear();
                    for (int i = 0; i < PointFile.GetPointFile(Axiss.PointFileName).Count; i++)
                    {
                        if (PointFile.GetPointFile(Axiss.PointFileName)[i].Name != null && PointFile.GetPointFile(Axiss.PointFileName)[i].Name != "")
                        {
                            comboBox1.Items.Add(PointFile.GetPointFile(Axiss.PointFileName)[i].Name);
                        }
                    }
                    comboBox1.Items.Add(" ");
                    PointFile.GetPointDataG(comboBox3.SelectedItem.ToString(), dataGridView2);

                    comboBox1.SelectedIndex = 0;
                    name = comboBox3.SelectedItem.ToString();
                }
                einbe = false;

                foreach (var item in PointFile.GetPointFile().Keys.ToArray())
                {
                    if (!comboBox3.Items.Contains(item))
                    {
                        comboBox3.Items.Add(item);
                    }
                }
                List<string> vs = new List<string>();
                foreach (var item in comboBox3.Items)
                {
                    if (!PointFile.GetPointFile().Keys.ToArray().Contains(item))
                    {
                        vs.Add(item.ToString());
                    }
                }
                for (int i = 0; i < vs.Count; i++)
                {
                    comboBox3.Items.Remove(vs[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {


                string names = dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
                string texty = "请重新输入点文件名";
                string nameStr = names;
            ste:

                nameStr = Interaction.InputBox(texty, "重命名", nameStr, 100, 100);
                if (nameStr != "")
                {
                    if (int.TryParse(nameStr[0].ToString(), out int det))
                    {
                        texty = "首位不可以包含数字";
                        goto ste;
                    }
                    if (PointFile.IsPointContainsKey(comboBox3.SelectedItem.ToString(), nameStr))
                    {
                        texty = "名称已存在";
                        goto ste;
                    }
                    dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells[1].Value = nameStr;
                    PointFile pointFilet = PointFile.GetPointName(comboBox3.SelectedItem.ToString(), names);
                    pointFilet.Name = nameStr;


                }



            }
            catch (Exception)
            {
            }
        }
    }
}
