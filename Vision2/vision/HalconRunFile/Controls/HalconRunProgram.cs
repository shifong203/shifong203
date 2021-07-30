using HalconDotNet;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ThridLibray;
using Vision2.ErosProjcetDLL.UI.DataGridViewF;
using Vision2.Project.formula;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.Vision;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class HalconRunProgram : UserControl
    {
        private HalconRun halconRun;
        public HalconRunProgram()
        {
            Movable = true;
            InitializeComponent();
            checkBox2.Checked = ErosProjcetDLL.Project.ProjectINI.DebugMode;
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView2);
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView3);
            dataGridView3.CellValueChanged += DataGridView3_CellValueChanged;
            dataGridView3.CurrentCellDirtyStateChanged += DataGridView3_CurrentCellDirtyStateChanged; ;
        }
        int y;
        int x;
        private void Tool_Click(object sender, EventArgs e)
        {
            string[] text;
            try
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                text = dataGridView3.Rows[dataGridView3.CurrentCellAddress.Y].Cells[dataGridView3.CurrentCellAddress.X].Value.ToString().Split('.');
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
                if (dataGridView3.Rows[dataGridView3.CurrentCellAddress.Y].Cells[dataGridView3.CurrentCellAddress.X].Value.ToString().Contains("."))
                {
                    dataGridView3.Rows[dataGridView3.CurrentCellAddress.Y].Cells[dataGridView3.CurrentCellAddress.X].Value += item.Text;
                }
                else
                {
                    dataGridView3.Rows[dataGridView3.CurrentCellAddress.Y].Cells[dataGridView3.CurrentCellAddress.X].Value = texts + ".";
                }

                int X = dataGridView3.CurrentCellAddress.X;
                int Y = dataGridView3.CurrentCellAddress.Y;
                if ((Y != -1 && X != -1) && dataGridView3.Rows[Y].Cells[X] != null && dataGridView3.Rows[Y].Cells[X].Value != null)
                {
                    ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                    string[] keys = dataGridView3.Rows[Y].Cells[X].Value.ToString().Split(',');
                    foreach (var item2 in RecipeCompiler.Instance.ProductEX[Project.formula.Product.ProductionName].Key_Navigation_Picture[text[0]].KeyRoi.Keys)
                    {
                        ToolStripMenuItem tool = new ToolStripMenuItem();
                        tool.Text = item2;
                        tool.Click += Tool_Click;
                        contextMenuStrip.Items.Add(tool);
                    }
                    Rectangle rectangle = dataGridView3.GetCellDisplayRectangle(x, y, false);
                    Rectangle rectangle2 = dataGridView3.RectangleToScreen(rectangle);
                    contextMenuStrip.Show(rectangle2.X, rectangle2.Y + this.dataGridView3.Rows[x].Height);
                }
                dataGridView3.EndEdit();
                dataGridView3.BeginEdit(false);

                //dataGridView1.RefreshEdit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
             try
                {
                    ErosProjcetDLL.UI.UICon.GetCursorPos(out ErosProjcetDLL.UI.UICon.POINT pOINT);
                    int X = dataGridView3.CurrentCellAddress.X;
                    int Y = dataGridView3.CurrentCellAddress.Y;
                    if ((Y != -1 && X != -1) && dataGridView3.Rows[Y].Cells[X] != null && dataGridView3.Rows[Y].Cells[X].Value != null)
                    {
                        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                        if (dataGridView3.Rows[Y].Cells[X].Value.ToString().Contains("."))
                        {
                            string[] keys = dataGridView3.Rows[Y].Cells[X].Value.ToString().Split('.');
                            if (keys.Length == 1)
                            {

                                if (RecipeCompiler.Instance.ProductEX[Project.formula.Product.ProductionName].Key_Navigation_Picture.ContainsKey(keys[0]))
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
                        }
                        Rectangle rectangle = dataGridView3.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                        x = e.ColumnIndex;
                        y = e.RowIndex;
                        Rectangle rectangle2 = dataGridView3.RectangleToScreen(rectangle);
                        contextMenuStrip.Show(rectangle2.X, rectangle2.Y + this.dataGridView3.Rows[e.RowIndex].Height);
                        //contextMenuStrip.Show(pOINT.X, pOINT.Y);
                    }
                }
                catch (Exception es)
                {
                }
        }

        private void DataGridView3_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView3.IsCurrentCellDirty)
            {
                dataGridView3.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        public HalconRunProgram(HalconRun halcon) : this()
        {
            Movable = true;
            halconRun = halcon;
            //camControl1.
            dataGridViewHalcon.ContextMenuStrip = halconRun.GetNewPrajetContextMenuStrip("");
            halconRun.UpHalconRunProgram += ContextMenuStrip_Click;
            Updata();
            halconRun.EventShowObj += HalconRun_EventShowObj;
            this.Disposed += MeasureControl_Disposed;
            if (Vision.GetSaveImageInfo(halcon.Name).ReadCamName == "")
            {
                button1.Visible = checkBox2.Visible = false;
            }
        }

        private void ContextMenuStrip_Click(HalconRun halcon,RunProgram run)
        {
            Movable = true;
            //halconRun.UpData(dataGridViewHalcon);
            Updata();
            Movable = false;
        }

        public void UpHalcon(HalconRun halcon)
        {
            halconRun = halcon;
            Updata();
            halconRun.EventShowObj += HalconRun_EventShowObj;
            this.Disposed += MeasureControl_Disposed;

        }
        private HalconDotNet.HObject HalconRun_EventShowObj(HalconRun halcon, string objName)
        {
            Movable = true;
            halconRun.UpData(dataGridViewHalcon);
            Movable = false;
            return new HalconDotNet.HObject();
        }

        private void MeasureControl_Disposed(object sender, EventArgs e)
        {
            halconRun.UpHalconRunProgram -= ContextMenuStrip_Click;
            halconRun.EventShowObj -= HalconRun_EventShowObj;
        }

        public void Updata()
        {
            try
            {
                Movable = true;
                isChanged = true;
                Column22.Items.Clear();
                Column22.Items.AddRange(Vision.Instance.DicLightSource.Keys.ToArray());
                if (halconRun.GetSaveImageInfo().ListCamData.Count!=0)
                {
                    dataGridView5.Rows.Add(halconRun.GetSaveImageInfo().ListCamData.Count);
                }
                for (int i = 0; i < halconRun.GetSaveImageInfo().ListCamData.Count; i++)
                {
                   dataGridView5.Rows[i].Cells[0].Value = i + 1;
                    dataGridView5.Rows[i].Cells[1].Value =
                        halconRun.GetSaveImageInfo().ListCamData[i].ExposureTime;
                    dataGridView5.Rows[i].Cells[2].Value =
                       halconRun.GetSaveImageInfo().ListCamData[i].Gain;
                    dataGridView5.Rows[i].Cells[3].Value =
                     halconRun.GetSaveImageInfo().ListCamData[i].Gamma;
                    dataGridView5.Rows[i].Cells[4].Value =
                   halconRun.GetSaveImageInfo().ListCamData[i].Light_Source;
                }
                if (Vision.Instance.DicLightSource.Count != 0)
                {
                    dataGridView6.Rows.Add(Vision.Instance.DicLightSource.Count);
                }
                int nIndex = 0;
                foreach (var item in Vision.Instance.DicLightSource)
                {
                    dataGridView6.Rows[nIndex].Cells[0].Value = item.Key;
                    dataGridView6.Rows[nIndex].Cells[1].Value = item.Value.H1;
                    dataGridView6.Rows[nIndex].Cells[2].Value = item.Value.H2;
                    dataGridView6.Rows[nIndex].Cells[3].Value = item.Value.H3;
                    dataGridView6.Rows[nIndex].Cells[4].Value = item.Value.H4;
                    nIndex++;
                }   
                listBox1.Items.Clear();
                foreach (var item in Vision.Instance.DicDrawbackNameS)
                {
                    listBox1.Items.Add(item.Key);
                }
                if (Vision.GetSaveImageInfo(halconRun.Name) == null)
                {
                    Vision.Instance.DicSaveType.Add(halconRun.Name, new SaveImageInfo());
                }
                if (!Vision.Instance.DicSaveType.ContainsKey(halconRun.Name))
                {
                    Vision.Instance.DicSaveType.Add(halconRun.Name, new SaveImageInfo());
                }
                if (halconRun.GetCam() != null)
                {
                    label4.Text = "FOV:" + halconRun.GetCam().FOV;
                }
                //listBox1.Items.Clear();
                propertyGrid2.SelectedObject = halconRun.TiffeOffsetImageEX;
                checkBox3.Checked = halconRun.EnbExposureTime;
                numericUpDown4.Value = (decimal)halconRun.CamData. ExposureTime;
                numericUpDown5.Value =  (decimal)halconRun.CamData.Gain;
                numericUpDown6.Value = (decimal)halconRun.CamData.Gamma;
                hSBExposure.Value = (int)(halconRun.CamData.ExposureTime*100);
                hSBGain.Value =  (int)halconRun.CamData.Gain*10;
                hScrollBar1.Value =  (int)(halconRun.CamData.Gamma*100);

                halconRun.UpData(dataGridViewHalcon);
                numericUpDown1.Value = halconRun.MaxRunID;
                if (halconRun.RunIDStr == null)
                {
                    halconRun.RunIDStr = new List<string>();
                }
                camControl1.UpDataRe(halconRun.GetCam(), halconRun);
                dataGridView1.Rows.Clear();
                if (!Vision.Instance.ListPrX.ContainsKey(halconRun.Name))
                {
                    Vision.Instance.ListPrX.Add(halconRun.Name, new System.Collections.Generic.List<string>());
                }
                if (!Vision.Instance.ListPrY.ContainsKey(halconRun.Name))
                {
                    Vision.Instance.ListPrY.Add(halconRun.Name, new System.Collections.Generic.List<string>());
                }


                while (halconRun.ReNmae.Count < halconRun.MaxRunID)
                {
                    halconRun.ReNmae.Add("");
                }
                while (halconRun.ReNmae.Count > halconRun.MaxRunID)
                {
                    halconRun.ReNmae.RemoveAt(halconRun.ReNmae.Count - 1);
                }
                dataGridView3.Rows.Clear();
                for (int i = 0; i < halconRun.ReNmae.Count; i++)
                {
                    int det = dataGridView3.Rows.Add();
                    dataGridView3.Rows[det].Cells[0].Value = halconRun.ReNmae[i];
                }
                for (int i = 0; i < halconRun.RunIDStr.Count; i++)
                {
                    int dt = dataGridView1.Rows.Add();
                    dataGridView1.Rows[dt].Cells[0].Value = halconRun.RunIDStr[i];
                    dataGridView1.Rows[dt].Cells[3].Value = "";
                    string[] dataStr = halconRun.RunIDStr[i].Split(';');
                    string names = "";
                    for (int i2 = 0; i2 < dataStr.Length; i2++)
                    {
                        string[] teimS = dataStr[i2].Split(',');
                        for (int i3 = 0; i3 < teimS.Length; i3++)
                        {
                            var detee = from objDic in halconRun.GetRunProgram()
                                        where objDic.Value.CDID == Single.Parse(teimS[i3])
                                        orderby objDic.Value.CDID ascending
                                        select objDic;
                            foreach (var item in detee)
                            {
                                names += item.Key + ",";
                            }
                        }
                        names = names.TrimEnd(',');
                        names += ";";
                    }
                    names = names.TrimEnd(';');
                    dataGridView1.Rows[dt].Cells[2].Value = names;
                    if (halconRun.RunIDStr.Count > Vision.Instance.ListPrX[halconRun.Name].Count)
                    {
                        Vision.Instance.ListPrX[halconRun.Name].Add("");
                    }
                    if (halconRun.RunIDStr.Count > Vision.Instance.ListPrY[halconRun.Name].Count)
                    {
                        Vision.Instance.ListPrY[halconRun.Name].Add("");
                    }
                }
                for (int i = 0; i < halconRun.RunName.Count; i++)
                {
                    if (dataGridView1.Rows.Count <= i)
                    {
                        int dt = dataGridView1.Rows.Add();
                    }
                    dataGridView1.Rows[i].Cells[4].Value = halconRun.RunName[i];
                    if (halconRun.RunName.Count > Vision.Instance.ListPrX[halconRun.Name].Count)
                    {
                        Vision.Instance.ListPrX[halconRun.Name].Add("");
                    }
                    if (halconRun.RunName.Count > Vision.Instance.ListPrY[halconRun.Name].Count)
                    {
                        Vision.Instance.ListPrY[halconRun.Name].Add("");
                    }
                }
                if (!Vision2.ErosProjcetDLL.Project.ProjectINI.GetUserJurisdiction("工程师"))
                {
                    if (dataGridView1.Columns.Count > 8)
                    {
                        dataGridView1.Columns.RemoveAt(8);
                    }
                    if (dataGridView1.Columns.Count > 7)
                    {
                        dataGridView1.Columns.RemoveAt(7);
                    }


                }
                for (int i = 0; i < Vision.Instance.ListPrX[halconRun.Name].Count; i++)
                {
                    if (dataGridView1.Rows.Count <= i)
                    {
                        break;
                    }
                    if (Vision2.ErosProjcetDLL.Project.ProjectINI.GetUserJurisdiction("工程师"))
                    {
                        dataGridView1.Rows[i].Cells[7].Value = Vision.Instance.ListPrX[halconRun.Name][i];
                        dataGridView1.Rows[i].Cells[8].Value = Vision.Instance.ListPrY[halconRun.Name][i];
                    }

                    if (Product.GetProd().ContainsKey(Vision.Instance.ListPrY[halconRun.Name][i]))
                    {
                        dataGridView1.Rows[i].Cells[6].Value = Project.formula.Product.GetProd()[Vision.Instance.ListPrY[halconRun.Name][i]];
                    }
                    if (Product.GetProd().ContainsKey(Vision.Instance.ListPrX[halconRun.Name][i]))
                    {
                        dataGridView1.Rows[i].Cells[5].Value = Project.formula.Product.GetProd()[Vision.Instance.ListPrX[halconRun.Name][i]];
                    }
                }
         
                this.numericUpDown2.Value = halconRun.TiffeOffsetImageEX.ImageNumberCol;
                this.numericUpDown3.Value = halconRun.TiffeOffsetImageEX.ImageNumberROW;

                dataGridView2.Rows.Clear();
                if (halconRun.TiffeOffsetImageEX.Rows1.Length == 0)
                {
                    halconRun.TiffeOffsetImageEX.Rows1 = HTuple.TupleGenConst(halconRun.TiffeOffsetImageEX.Rows.Length, -1);
                    halconRun.TiffeOffsetImageEX.Cols1 = HTuple.TupleGenConst(halconRun.TiffeOffsetImageEX.Rows.Length, -1);
                    halconRun.TiffeOffsetImageEX.Rows2 = HTuple.TupleGenConst(halconRun.TiffeOffsetImageEX.Rows.Length, -1);
                    halconRun.TiffeOffsetImageEX.Cols2 = HTuple.TupleGenConst(halconRun.TiffeOffsetImageEX.Rows.Length, -1);
                }
                for (int i = 0; i < halconRun.TiffeOffsetImageEX.Rows.Length; i++)
                {
                    int det = dataGridView2.Rows.Add();
                    dataGridView2.Rows[det].Cells[0].Value = halconRun.TiffeOffsetImageEX.Rows[i].D;
                    dataGridView2.Rows[det].Cells[1].Value = halconRun.TiffeOffsetImageEX.Cols[i].D;
                    dataGridView2.Rows[det].Cells[2].Value = halconRun.TiffeOffsetImageEX.Rows1[i].D;
                    dataGridView2.Rows[det].Cells[3].Value = halconRun.TiffeOffsetImageEX.Cols1[i].D;
                    dataGridView2.Rows[det].Cells[4].Value = halconRun.TiffeOffsetImageEX.Rows2[i].D;
                    dataGridView2.Rows[det].Cells[5].Value = halconRun.TiffeOffsetImageEX.Cols2[i].D;
                }
                propertyGrid1.SelectedObject = Vision.GetSaveImageInfo(halconRun.Name);
                Movable = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isChanged = false;
        }

        private bool _movable = true;

        private Pen pen = new Pen(Color.Black);

        /// <summary>
        /// UISizeDot的边框颜色
        /// </summary>
        public Color BorderColor
        {
            get { return pen.Color; }
            set
            {
                this.pen = new Pen(value);
                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: 在此处添加自定义绘制代码
            //this.BackColor = Color.White;
            pe.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            // 调用基类 OnPaint
            base.OnPaint(pe);
        }


        /// <summary>
        ///
        /// </summary>
        public bool Movable
        {
            get { return this._movable; }
            set { this._movable = value; }
        }

        private void MeasureControl_Load(object sender, EventArgs e)
        {
            Movable = false;
         

        }

        private void MeasureControl_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void MeasureControl_Leave(object sender, EventArgs e)
        {
        }

        private void dataGridViewHalcon_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void dataGridViewHalcon_MouseDown(object sender, MouseEventArgs e)
        {
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //点击button按钮事件
                if (dataGridViewHalcon.Columns[e.ColumnIndex].Name == "dataGridViewButtonColumn1" && e.RowIndex >= 0)
                {
                    dataGridViewHalcon[4, e.RowIndex].Style.BackColor = Color.White;
                    RunProgram runProgram = dataGridViewHalcon.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag as RunProgram;
                    if (runProgram != null)
                    {
                        halconRun.UPStart();
                        halconRun.ShowVision(runProgram.Name ,halconRun.GetOneImageR() );
                        halconRun.EndChanged(halconRun.GetOneImageR());
                        dataGridViewHalcon.Rows[e.RowIndex].Cells[4].Value = runProgram.RunTime;
                        if (runProgram.ErrBool)
                        {
                            dataGridViewHalcon[4, e.RowIndex].Style.BackColor = Color.Red;
                        }
                    }
                }
                if (e.ColumnIndex == 5)
                {
                    if (halconRun.GetRunProgram().ContainsKey(dataGridViewHalcon.Rows[e.RowIndex].Cells[1].Value.ToString()))
                    {
                    
                        RunProgram runProgram = halconRun.GetRunProgram()[dataGridViewHalcon.Rows[e.RowIndex].Cells[1].Value.ToString()];
                        //if (true)
                        //{
                        //    DrawVisionForm drawVisionForm = new DrawVisionForm(runProgram);
                        //    drawVisionForm.Show();
                        //    return;
                        //}
                        Vision2.ErosProjcetDLL.Project.PropertyForm propertyForm = new Vision2.ErosProjcetDLL.Project.PropertyForm();
                        propertyForm.TopLevel = false;
                        propertyForm.Location = new Point(1000, 20);
                        this.FindForm().ParentForm.Controls.Add(propertyForm);
                        propertyForm.Show();
                        propertyForm.UProperty(runProgram);
                        propertyForm.BringToFront();
                        propertyForm.TopMost = true;
                        propertyForm.Name = runProgram.Name;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    Vision.Instance.SocketClint_PassiveEvent(Encoding.Default.GetBytes(textBox1.Text), halconRun.GetSocket(), halconRun.GetSocket().Socket());
                }
            }
            catch (Exception)
            {
            }

        }

        private void dataGridViewHalcon_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Movable)
                {
                    return;
                }
                if (e.ColumnIndex == 0)
                {
                    RunProgram runProgram = dataGridViewHalcon.Rows[e.RowIndex].Cells[3].Tag as RunProgram;
                    runProgram.CDID = Single.Parse(dataGridViewHalcon.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                else if (e.ColumnIndex == 1)
                {
                    RunProgram runProgram = dataGridViewHalcon.Rows[e.RowIndex].Cells[3].Tag as RunProgram;
                    if (!halconRun.GetRunProgram().ContainsKey(dataGridViewHalcon.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                    {
                        halconRun.GetRunProgram().Remove(runProgram.Name);
                        runProgram.Name = dataGridViewHalcon.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        halconRun.GetRunProgram().Add(runProgram.Name, runProgram);
                    }
                    else
                    {
                        MessageBox.Show("名称已存在");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1)
                {
                    if (checkBox1.Checked)

                    {
                        halconRun.HobjClear();

                        halconRun.ReadCamImage((e.RowIndex + 1).ToString(), e.RowIndex +1);
                        //halconRun.CamImageEvent(e.RowIndex.ToString(), null, e.RowIndex + 1, false);
                        dataGridView1.Rows[e.RowIndex].Cells[3].Value = halconRun.RunTimeI;
                    }
                    else
                    {
                        OneResultOBj oneResultOBj = new OneResultOBj();
                        oneResultOBj.Image = halconRun.GetOneImageR().Image;

                        halconRun.CamImageEvent((e.RowIndex+1).ToString(), oneResultOBj, e.RowIndex +1, false);
                        dataGridView1.Rows[e.RowIndex].Cells[3].Value = halconRun.RunTimeI;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Movable)
                {
                    return;
                }

                if (e.ColumnIndex == 4)
                {

                    while (halconRun.RunName.Count <= e.RowIndex)
                    {
                        halconRun.RunName.Add(dataGridView1.Rows[halconRun.RunName.Count].Cells[e.ColumnIndex].Value.ToString());
                    }
                    halconRun.RunName[e.RowIndex] = (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                else if (e.ColumnIndex == 0)
                {


                    string[] datas = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Split(';');
                    string names = "";
                    for (int i = 0; i < datas.Length; i++)
                    {
                        string[] timeStr = datas[i].Split(',');
                        for (int i2 = 0; i2 < timeStr.Length; i2++)
                        {
                            if (Single.TryParse(timeStr[i2], out Single resultInt))
                            {
                                var detee = from objDic in halconRun.GetRunProgram()
                                            where objDic.Value.CDID == resultInt
                                            orderby objDic.Value.CDID ascending
                                            select objDic;

                                foreach (var item in detee)
                                {
                                    names += item.Key + ",";
                                }
                            }
                            else
                            {
                                MessageBox.Show("请输入整数");
                            }
                        }
                        names = names.TrimEnd(',');
                        names += ';';
                    }
                    names = names.TrimEnd(';');
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value = names;
                    for (int i = 0; i < e.RowIndex - halconRun.RunIDStr.Count; i++)
                    {
                        halconRun.RunIDStr.Add(dataGridView1.Rows[e.RowIndex - (e.RowIndex - i)].Cells[0].Value.ToString());
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (halconRun.RunIDStr.Count <= e.RowIndex)
                        {
                            halconRun.RunIDStr.Add("");
                        }
                        if (dataGridView1.Rows[i].Cells[0].Value != null)
                        {
                            halconRun.RunIDStr[i] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        }
                    }
                }
                else if (e.ColumnIndex == 7)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                    {
                        Vision.Instance.ListPrX[halconRun.Name][e.RowIndex] = "";
                    }
                    else
                    {
                        Vision.Instance.ListPrX[halconRun.Name][e.RowIndex] = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    }

                }
                else if (e.ColumnIndex == 8)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                    {
                        Vision.Instance.ListPrY[halconRun.Name][e.RowIndex] = "";
                    }
                    else
                    {
                        Vision.Instance.ListPrY[halconRun.Name][e.RowIndex] = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    }

                }
                else if (e.ColumnIndex == 6)
                {
                    if (Project.formula.Product.GetProd().ContainsKey(Vision.Instance.ListPrY[halconRun.Name][e.RowIndex]))
                    {
                        Project.formula.Product.GetProd()[Vision.Instance.ListPrY[halconRun.Name][e.RowIndex]] = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    }
                }
                else if (e.ColumnIndex == 5)
                {

                    if (Project.formula.Product.GetProd().ContainsKey(Vision.Instance.ListPrX[halconRun.Name][e.RowIndex]))
                    {
                        Project.formula.Product.GetProd()[Vision.Instance.ListPrX[halconRun.Name][e.RowIndex]] = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }
        private void dataGridViewHalcon_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    dataGridViewHalcon.ContextMenuStrip = halconRun.GetNewPrajetContextMenuStrip(dataGridViewHalcon.Rows[e.RowIndex].Cells[1].Value.ToString());
                    List<string> names = new List<string>();
                    for (int i = 0; i < dataGridViewHalcon.SelectedRows.Count; i++)
                    {
                        names.Add(dataGridViewHalcon.SelectedRows[i].Cells[1].Value.ToString());
                    }
                    dataGridViewHalcon.ContextMenuStrip.Items["同步到库"].Tag = names;
                    dataGridViewHalcon.ContextMenuStrip.Items["删除"].Tag = names;
                }
            }
            catch (Exception)
            {
            }
        }
        private void 打开程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int d = dataGridView1.Rows.Add();
                halconRun.RunIDStr = new List<string>();
                halconRun.RunName.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value == null)
                    {
                        return;
                    }
                    halconRun.RunIDStr.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    if (dataGridView1.Rows[i].Cells[4].Value == null)
                    {
                        halconRun.RunName.Add("");
                    }
                    else
                    {
                        halconRun.RunName.Add(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 删除程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int d = dataGridView1.SelectedRows.Count;
                for (int i = 0; i < d; i++)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                }
                halconRun.RunIDStr = new List<string>();
                halconRun.RunName.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    halconRun.RunIDStr.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    if (dataGridView1.Rows[i].Cells[4].Value == null)
                    {
                        halconRun.RunName.Add("");
                    }
                    else
                    {
                        halconRun.RunName.Add(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Movable)
                {
                    return;
                }
                halconRun.MaxRunID = (sbyte)numericUpDown1.Value;
            }
            catch (Exception)
            {
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ErosProjcetDLL.Project.ProjectINI.SelpMode = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ErosProjcetDLL.Project.ProjectINI.DebugMode = checkBox2.Checked;
        }
        int DET;
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                halconRun.CamImageEvent(dataGridView1.Rows[0].Cells[0].Value.ToString(), null, DET, false);
                dataGridView1.Rows[0].Cells[3].Value = halconRun.RunTimeI;
                DET++;
                button2.Text = "模拟ID" + DET;
            }
            catch (Exception)
            {


            }

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.Movable)
                {
                    return;
                }
                halconRun.TiffeOffsetImageEX.ImageNumberROW = (int)numericUpDown3.Value;
                halconRun.TiffeOffsetImageEX.ImageNumberCol = (int)numericUpDown2.Value;

            }
            catch (Exception)
            {
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                halconRun.TiffeOffsetImageEX.Rows = new HalconDotNet.HTuple();
                halconRun.TiffeOffsetImageEX.Cols = new HalconDotNet.HTuple();
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Cells[0].Value == null)
                    {
                        return;
                    }
                    halconRun.TiffeOffsetImageEX.Rows.Append(int.Parse(dataGridView2.Rows[i].Cells[0].Value.ToString()));
                    halconRun.TiffeOffsetImageEX.Cols.Append(int.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                HObject hObject = new HObject();
                hObject.GenEmptyObj();
                hObject = RunProgram.DrawModOBJ(halconRun, HalconRun.EnumDrawType.Rectangle1, hObject);
                HOperatorSet.SmallestRectangle1(hObject, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells[2].Value = 0;
                dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells[3].Value = 0;
                dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells[4].Value = row2 - Double.Parse(dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
                dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells[5].Value = col2 - Double.Parse(dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells[1].Value.ToString());

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

                halconRun.Image(halconRun.TiffeOffsetImageEX.TiffeOffsetImage());
                halconRun.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            SetCamPraegrm();
        }
        public void SetCamPraegrm()
        {
            try
            {

                if (_movable)
                {
                    return;
                }
                _movable = true;
                halconRun.EnbExposureTime = checkBox3.Checked;
                hSBExposure.Value  =(int) numericUpDown4.Value*100;
                hScrollBar1.Value = (int)numericUpDown6.Value * 100;
                hSBGain.Value = (int)numericUpDown5.Value * 10;

                halconRun.CamData.ExposureTime = (double)numericUpDown4.Value;
                halconRun.CamData.Gain = (double)numericUpDown5.Value;
                halconRun.CamData.Gamma = (double)numericUpDown6.Value;

                halconRun.SetCamPraegrm();
            }
            catch (Exception)
            {
            }
            _movable = false;
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_movable)
                {
                    return;
                }
                if (e.ColumnIndex == 0)
                {
                    halconRun.TiffeOffsetImageEX.Rows[e.RowIndex] = double.Parse(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                else if (e.ColumnIndex == 1)
                {
                    halconRun.TiffeOffsetImageEX.Cols[e.RowIndex] = double.Parse(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                else if (e.ColumnIndex == 2)
                {
                    halconRun.TiffeOffsetImageEX.Rows1[e.RowIndex] = double.Parse(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }

                else if (e.ColumnIndex == 3)
                {
                    halconRun.TiffeOffsetImageEX.Cols1[e.RowIndex] = double.Parse(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                else if (e.ColumnIndex == 4)
                {
                    halconRun.TiffeOffsetImageEX.Rows2[e.RowIndex] = double.Parse(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                else if (e.ColumnIndex == 5)
                {
                    halconRun.TiffeOffsetImageEX.Cols2[e.RowIndex] = double.Parse(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                halconRun.Image(halconRun.TiffeOffsetImageEX.TiffeOffsetImage());
                halconRun.ShowImage();
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
                if (halconRun == null)
                {
                    MessageBox.Show("未关联执行程序");
                    return;
                }
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择图片文件可多选";
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;


                for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                {
                    HOperatorSet.ReadImage(out HObject hObject, openFileDialog.FileNames[i]);
                    halconRun.TiffeOffsetImageEX.SetTiffeOff(hObject, dataGridView2.SelectedCells[0].RowIndex + 1 + i);
                }

                halconRun.Image(halconRun.TiffeOffsetImageEX.TiffeOffsetImage());
                halconRun.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {

                halconRun.TiffeOffsetImageEX.Rows = new HalconDotNet.HTuple();
                halconRun.TiffeOffsetImageEX.Cols = new HalconDotNet.HTuple();
                if (true)
                {

                }
                halconRun.TiffeOffsetImageEX.ImageHeightI = halconRun.GetCam(). Height;
                halconRun.TiffeOffsetImageEX.ImageWidthI = halconRun.GetCam().Width;

                for (int i = 0; i < numericUpDown2.Value * numericUpDown3.Value; i++)
                {
                    int sd = i / halconRun.TiffeOffsetImageEX.ImageNumberROW;
                    int dt = i % halconRun.TiffeOffsetImageEX.ImageNumberROW;
                    halconRun.TiffeOffsetImageEX.Rows.Append((int)(halconRun.TiffeOffsetImageEX.ImageHeightI / halconRun.TiffeOffsetImageEX.ZoomImageSize * dt));
                    halconRun.TiffeOffsetImageEX.Cols.Append((int)(halconRun.TiffeOffsetImageEX.ImageWidthI / halconRun.TiffeOffsetImageEX.ZoomImageSize * sd));
                }
                halconRun.TiffeOffsetImageEX.Rows1 = HTuple.TupleGenConst((int)(numericUpDown2.Value * numericUpDown3.Value), -1);
                halconRun.TiffeOffsetImageEX.Cols1 = HTuple.TupleGenConst((int)(numericUpDown2.Value * numericUpDown3.Value), -1);
                halconRun.TiffeOffsetImageEX.Rows2 = HTuple.TupleGenConst((int)(numericUpDown2.Value * numericUpDown3.Value), -1);
                halconRun.TiffeOffsetImageEX.Cols2 = HTuple.TupleGenConst((int)(numericUpDown2.Value * numericUpDown3.Value), -1);
                //halconRun.TiffeOffsetImageEX.Width =  halconRun.TiffeOffsetImageEX.ImageWidth/ halconRun.TiffeOffsetImageEX.ZoomImageSize *    (halconRun.TiffeOffsetImageEX.ImageNumberCol-1);
                //halconRun.TiffeOffsetImageEX.Height = halconRun.TiffeOffsetImageEX.ImageHeight/ halconRun.TiffeOffsetImageEX.ZoomImageSize *    (halconRun.TiffeOffsetImageEX.ImageNumberROW-1);

                halconRun.TiffeOffsetImageEX.SetTiffeOff();
                dataGridView2.Rows.Clear();
                int det = 0;
                Movable = true;
                for (int i = 0; i < halconRun.TiffeOffsetImageEX.Rows.Length; i++)
                {
                    det = dataGridView2.Rows.Add();
                    dataGridView2.Rows[det].Cells[0].Value = halconRun.TiffeOffsetImageEX.Rows[i].D;
                    dataGridView2.Rows[det].Cells[1].Value = halconRun.TiffeOffsetImageEX.Cols[i].D;
                    dataGridView2.Rows[det].Cells[2].Value = halconRun.TiffeOffsetImageEX.Rows1[i].D;
                    dataGridView2.Rows[det].Cells[3].Value = halconRun.TiffeOffsetImageEX.Cols1[i].D;
                    dataGridView2.Rows[det].Cells[4].Value = halconRun.TiffeOffsetImageEX.Rows2[i].D;
                    dataGridView2.Rows[det].Cells[5].Value = halconRun.TiffeOffsetImageEX.Cols2[i].D;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Movable = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CPKForm1 cPKForm1 = new CPKForm1();
            cPKForm1.HalconRun = halconRun;
            cPKForm1.Show();
        }

        private void camControl1_Load(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                Vision.ShowVisionResetForm(this.halconRun);
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
                halconRun.Image(halconRun.TiffeOffsetImageEX.TiffeOffSetImageFill());
                halconRun.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                if (halconRun == null)
                {
                    MessageBox.Show("未关联执行程序");
                    return;
                }
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择图片文件可多选";
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                {
                    HOperatorSet.ReadImage(out HObject hObject, openFileDialog.FileNames[i]);
                    halconRun.TiffeOffsetImageEX.SetTiffeOff(hObject, dataGridView2.SelectedCells[0].RowIndex + 1 + i);
                }
                halconRun.Image(halconRun.TiffeOffsetImageEX.TiffeOffsetImage());
                halconRun.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                halconRun.TiffeOffsetImageEX.CatTimetPoint();
                halconRun.Image(halconRun.TiffeOffsetImageEX.TiffeOffsetImage());
                halconRun.ShowImage();
            }
            catch (Exception)
            {
            }
        }
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (Movable)
                {
                    return;
                }
                halconRun.ReNmae[e.RowIndex] = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (halconRun == null)
                {
                    MessageBox.Show("未关联执行程序");
                    return;
                }

                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择图像文件夹";
                fbd.SelectedPath = halconRun.ImagePaths;
                if (fbd.SelectedPath=="")
                {
                    fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                }
                DialogResult dialog = ErosProjcetDLL.UI.PropertyGrid.FolderBrowserLauncher.ShowFolderBrowser(fbd);
                if (dialog == DialogResult.OK)
                {
                    halconRun.ImagePaths = fbd.SelectedPath;
                    paths = ErosProjcetDLL.FileCon.FileConStatic.GetFilesListPath(halconRun.ImagePaths, ".jpg,.tif,.tiff,.gif,.bmp,.jpg,.jpeg,.jp2,.png,.pcx,.pgm,.ppm,.pbm,.xwd,.ima,.hobj");
                    ImageIndx = 0;
                    List<string> list = new List<string>();
                    List<int> RunIDt= new List<int>();
                
                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (paths[i].Contains(halconRun.Name))
                        {

                            if (!paths[i].Contains("拼图"))
                            {
                                list.Add(paths[i]);
                                string ata = Path.GetFileNameWithoutExtension(paths[i]);
                                 ata = ata.Remove(ata.LastIndexOf('-'), ata.Length - ata.LastIndexOf('-'));
                                int dtw= ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(ata);
                                RunIDt.Add(dtw);
                            }

                        }
                    }
                    string[] pahts = new string[RunIDt.Count];

                    //for (int i = 0; i < RunIDt.Count; i++)
                    //{
                    //    pahts[RunIDt[i] - 1] = list[i];
                    //}
                    paths = list.ToList();

                    toolStripDropDownButton1.DropDownItems.Clear();
                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (i>30)
                        {
                            break;
                        }
                        ToolStripItem toolStripItem= toolStripDropDownButton1.DropDownItems.Add(paths[i]);
                        toolStripItem.Click += ToolStripItem_Click;
                        void ToolStripItem_Click(object sender, EventArgs e)
                        {
                            try
                            {
                               int index= toolStripDropDownButton1.DropDownItems.IndexOf(toolStripItem);
                                ImageIndx = index;
                                if (ImageIndx >= 0)
                                {


                                    OneResultOBj oneResultOBj = new OneResultOBj();
                                    oneResultOBj.ReadImage(paths[ImageIndx]);
                                    string name = Path.GetFileNameWithoutExtension(paths[ImageIndx]);
                                    toolStripDropDownButton1.Text = name + "{" + paths.Count + "/" + (ImageIndx + 1) + "}";
                                    if (!name.Contains("拼图"))
                                    {
                                        int liID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name);
                                        int data = liID;
                                        if (name.Contains('-'))
                                        {
                                            string dat = name.Split('-')[0];
                                            data = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(dat);
                                        }
                                        halconRun.CamImageEvent(liID.ToString(), oneResultOBj, data, toolStripCheckbox2.GetBase().Checked);
                                    }
                                }
                                halconRun.ShowObj();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                        }
                    }
     
                    toolStripDropDownButton1.Text =  "{" + paths.Count + "/" + (ImageIndx + 1) + "}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       

        int ImageIndx;
        private void tsButton2_Click(object sender, EventArgs e)
        {
            try
            {
 
                if (ImageIndx >1)
                {
                    ImageIndx--;
                    //halconRun.ReadImage(paths[ImageIndx-1]);
                    OneResultOBj oneResultOBj = new OneResultOBj();
                    oneResultOBj.ReadImage(paths[ImageIndx - 1]);
                    string name = Path.GetFileNameWithoutExtension(paths[ImageIndx-1]);
                    toolStripDropDownButton1.Text = name + "{" + paths.Count + "/" + (ImageIndx ) + "}";
                    if (!name.Contains("拼图"))
                    {
                        int liID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name);
                        int data = liID;
                        if (name.Contains('-'))
                        {
                            string dat = name.Split('-')[0];
                            data = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(dat);
                        }
                        halconRun.CamImageEvent(liID.ToString(), oneResultOBj, data, toolStripCheckbox2.GetBase().Checked);
                    }
                }
                halconRun.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
       
        }

        List<string> paths;
        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (ImageIndx < paths.Count)
                {
                    ImageIndx++;
                    //halconRun.ReadImage(paths[ImageIndx-1]);
                    OneResultOBj oneResultOBj = new OneResultOBj();
                    oneResultOBj.ReadImage(paths[ImageIndx - 1]);
                    string name = Path.GetFileNameWithoutExtension(paths[ImageIndx-1]);
                    toolStripDropDownButton1.Text = name + "{" + paths.Count + "/" + (ImageIndx ) + "}";
                    if (!name.Contains("拼图"))
                    {
                        int liID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name);
                        int data = liID;
                        if (name.Contains('-'))
                        {
                            string dat = name.Split('-')[0];
                            data = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(dat);
                        }
                        halconRun.CamImageEvent(liID.ToString(), oneResultOBj, data, toolStripCheckbox2.GetBase().Checked);
                    }
         
                }
                halconRun.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        bool RunBool;

        private void tsButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (RunBool)
                {
                    RunBool = false;
                    return;
                }
                Thread thread = new Thread(() => {
                    try
                    {
                        RunBool = true;
                        int det = paths.Count - ImageIndx;
                        for (int i = 0; i < det; i++)
                        {
                            if (ImageIndx < paths.Count)
                            {
                                ImageIndx++;
                                //halconRun.ReadImage(paths[ImageIndx-1]);
                                OneResultOBj oneResultOBj = new OneResultOBj();
                                oneResultOBj.ReadImage(paths[ImageIndx - 1]);
                                string name = Path.GetFileNameWithoutExtension(paths[ImageIndx-1]);
                                toolStripDropDownButton1.Text = name + "{" + paths.Count + "/" + (ImageIndx ) + "}";
                                if (!name.Contains("拼图"))
                                {
                                    int liID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name);
                                    int data = liID;
                                    if (name.Contains('-'))
                                    {
                                        string dat = name.Split('-')[0];
                                        data = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(dat);
                                    }
                                    halconRun.CamImageEvent(liID.ToString(), oneResultOBj, data, toolStripCheckbox2.GetBase().Checked);
                                }

                            }
                            halconRun.ShowObj();
                            if (toolStripCheckbox1.GetBase().Checked)
                            {
                                if (!halconRun.ResultBool)
                                {
                                    break;
                                }
                            }
                            if (!RunBool)
                            {
                                break;
                            }
                            Thread.Sleep(100);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    RunBool = false;
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        int iDNX = 0;
        private void button12_Click_1(object sender, EventArgs e)
        {
            try
            {
                iDNX++;
                //DebugCompiler.GetThis().DDAxis.GetTrayInxt(0).SetNumberValue(iDNX, halconRun.GetdataVale());
            }
            catch (Exception)
            {
            }
        }

        private void 注册图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
                try
                {
                    string path = Vision.VisionPath+"Image\\"+ halconRun.Name+(dataGridView1.SelectedRows[0].Index+1);
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    HOperatorSet.WriteImage(halconRun.Image(), "bmp", 0, path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void 读取注册图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //halconRun.HobjClear();

                string path = Vision.VisionPath + "Image\\" +halconRun.Name+ (dataGridView1.SelectedRows[0].Index + 1)+ ".bmp";

                if (File.Exists(path))
                {
                    HOperatorSet.ReadImage(out HObject hObject, path);
                    halconRun.SetResultOBj(new OneResultOBj());
                    HOperatorSet.GetImageSize(hObject, out HTuple width, out HTuple height);
                    halconRun.GetHWindow().Setprat(0, 0, height, width);
                    halconRun.Image(hObject);
                }
                else
                {
                    MessageBox.Show(Path.GetFileName(path) +"图片未注册");
                }
                halconRun.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 打开注册文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Vision.VisionPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         
        }

        bool isChanged ; 
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                isChanged = true;
                drawBackSt = Vision.Instance.DicDrawbackNameS[listBox1.SelectedItem.ToString()];
                dataGridView4.Rows.Clear();
                if (drawBackSt.DicDrawbackName.Count!=0)
                {
                    dataGridView4.Rows.Add(drawBackSt.DicDrawbackName.Count);
                }
           
                for (int i = 0; i < drawBackSt.DicDrawbackName.Count; i++)
                {
                    dataGridView4.Rows[i].Cells[0].Value = drawBackSt.DicDrawbackIndex[i];
                    dataGridView4.Rows[i].Cells[1].Value = drawBackSt.DicDrawbackName[i];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isChanged = false;
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string name = Interaction.InputBox("请输入新名称", "缺陷类型", "缺陷1", 100, 100);
                if (name!="")
                {
                    if (!Vision.Instance.DicDrawbackNameS.ContainsKey(name))
                    {
                        Vision.Instance.DicDrawbackNameS.Add(name, new DrawBackSt());
                        listBox1.Items.Add(name);
                    }
                    else
                    {
                        MessageBox.Show("已存在"+name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        DrawBackSt drawBackSt;
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Vision.Instance.DicDrawbackNameS.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    Vision.Instance.DicDrawbackNameS.Remove(listBox1.SelectedItem.ToString());
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView4_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isChanged)
                {
                    return;
                }
                if (drawBackSt!=null)
                {
                    for (int i = 0; i < dataGridView4.Rows.Count; i++)
                    {
                        if (dataGridView4.Rows[i].Cells[0].Value!=null)
                        {
                            if (drawBackSt.DicDrawbackIndex.Count <= i)
                            {
                                drawBackSt.DicDrawbackIndex.Add(int.Parse(dataGridView4.Rows[i].Cells[0].Value.ToString()));
                            }
                            else
                            {
                                drawBackSt.DicDrawbackIndex[i] = int.Parse(dataGridView4.Rows[i].Cells[0].Value.ToString());
                            }
                        }
                        if (dataGridView4.Rows[i].Cells[1].Value != null)
                        {
                            if (drawBackSt.DicDrawbackName.Count <= i)
                            {
                                drawBackSt.DicDrawbackName.Add(dataGridView4.Rows[i].Cells[1].Value.ToString());
                            }
                            else
                            {
                                drawBackSt.DicDrawbackName[i] = dataGridView4.Rows[i].Cells[1].Value.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void 删除缺陷类型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                drawBackSt.DicDrawbackIndex.RemoveAt(dataGridView4.SelectedCells[0].RowIndex);
                drawBackSt.DicDrawbackName.RemoveAt(dataGridView4.SelectedCells[0].RowIndex);
                dataGridView4.Rows.RemoveAt(dataGridView4.SelectedCells[0].RowIndex);
                
            }
            catch (Exception)
            {
            }
        }

        private void hSBExposure_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                if (_movable)
                {
                    return;
                }
                _movable = true;
                numericUpDown4.Value = (decimal)(hSBExposure.Value * 0.01);
                halconRun.CamData.ExposureTime = (double)numericUpDown4.Value;

                halconRun.SetCamPraegrm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            _movable = false;
        }

        private void hSBGain_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                if (_movable)
                {
                    return;
                }
                _movable = true;
                numericUpDown5.Value = (decimal)(hSBGain.Value*0.1);
                halconRun.CamData.Gain = (double)numericUpDown5.Value;

                halconRun.SetCamPraegrm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            _movable = false;
        }




        private void hScrollBar1_Scroll_1(object sender, ScrollEventArgs e)
        {
            try
            {
                if (_movable)
                {
                    return;
                }
                _movable = true;
                numericUpDown6.Value = (decimal)(hScrollBar1.Value * 0.01);
                halconRun.CamData.Gamma = (double)numericUpDown6.Value;

                halconRun.SetCamPraegrm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            _movable = false;
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                halconRun.GetSaveImageInfo().ListCamData.
                    RemoveAt(dataGridView5.SelectedCells[0].RowIndex);
                dataGridView5.Rows.RemoveAt(dataGridView5.SelectedCells[0].RowIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 添加采图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isChanged = true;
                halconRun.GetSaveImageInfo().ListCamData.Add(new Cams.CamData());
                int indext=  dataGridView5.Rows.Add();
                dataGridView5.Rows[indext].Cells[0].Value = indext;
                dataGridView5.Rows[indext].Cells[1].Value = halconRun.GetSaveImageInfo().ListCamData[
                    indext].ExposureTime;
                dataGridView5.Rows[indext].Cells[2].Value = halconRun.GetSaveImageInfo().ListCamData[
                    indext].Gain;
                dataGridView5.Rows[indext].Cells[3].Value = halconRun.GetSaveImageInfo().ListCamData[
               indext].Gamma;
                dataGridView5.Rows[indext].Cells[4].Value = halconRun.GetSaveImageInfo().ListCamData[
                 indext].Light_Source;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isChanged = false;
        }



        private void 删除ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                string key = dataGridView6.Rows[dataGridView6.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                dataGridView6.Rows.RemoveAt(dataGridView6.SelectedCells[0].RowIndex);
                Vision.Instance.DicLightSource.Remove(key);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void 添加ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                isChanged = true;
                int indext = dataGridView6.Rows.Add();
                for (int i = 0; i < 10; i++)
                {
                    if (!Vision.Instance.DicLightSource.ContainsKey("光源" + indext))
                    {
                        Vision.Instance.DicLightSource.Add("光源" + indext, new Cams.LightSource.LightSourceData());
                        dataGridView6.Rows[indext].Cells[0].Value = "光源" + indext;
                        dataGridView6.Rows[indext].Cells[1].Value = Vision.Instance.DicLightSource["光源" + indext].H1;
                        dataGridView6.Rows[indext].Cells[2].Value = Vision.Instance.DicLightSource["光源" + indext].H2;
                        dataGridView6.Rows[indext].Cells[3].Value = Vision.Instance.DicLightSource["光源" + indext].H3;
                        dataGridView6.Rows[indext].Cells[4].Value = Vision.Instance.DicLightSource["光源" + indext].H4;
                        break;
                    }
                    indext++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isChanged = false;

        }

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex==5)
                {
                   string key= dataGridView6.Rows[e.RowIndex].Cells[0].Value.ToString();
                   Vision.SetLight(key);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex==5)
                {
                    halconRun.SetCamPraegrm(e.RowIndex+1);
                    Thread.Sleep(400);
                    HObject imaget = halconRun.GetCam().GetImage();
                    OneResultOBj oneResultOBj = new OneResultOBj();
                    oneResultOBj.Image = imaget;
                    oneResultOBj.LiyID = 0;
                    oneResultOBj.RunID = e.RowIndex + 1;
                    halconRun.Image(imaget);
                    halconRun.ShowImage();
                    //VisionWindow.UPOneImage(oneResultOBj);
                    halconRun.GetHWindow().UPOneImage(oneResultOBj);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView5_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isChanged)
                {
                    return;
                }
                for (int i = 0; i < dataGridView5.RowCount; i++)
                {
                    halconRun.GetSaveImageInfo().ListCamData[i].ExposureTime = double.Parse(dataGridView5.Rows[i].Cells[1].Value.ToString());
                    halconRun.GetSaveImageInfo().ListCamData[i].Gain = double.Parse(dataGridView5.Rows[i].Cells[2].Value.ToString());
                    halconRun.GetSaveImageInfo().ListCamData[i].Gamma = double.Parse(dataGridView5.Rows[i].Cells[3].Value.ToString());
                    halconRun.GetSaveImageInfo().ListCamData[i].Light_Source = dataGridView5.Rows[i].Cells[4].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView6_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isChanged)
                {
                    return;
                }
                for (int i = 0; i < dataGridView6.RowCount; i++)
                {
                    if (dataGridView6.Rows[i].Cells[0].Value==null)
                    {
                        continue;
                    }
                    string key = dataGridView6.Rows[i].Cells[0].Value.ToString();
                    if (Vision.Instance.DicLightSource.ContainsKey(key))
                    {
                        Vision.Instance.DicLightSource[key].H1 = Int16.Parse(dataGridView6.Rows[i].Cells[1].Value.ToString());
                        Vision.Instance.DicLightSource[key].H2 = Int16.Parse(dataGridView6.Rows[i].Cells[2].Value.ToString());
                        Vision.Instance.DicLightSource[key].H3 = Int16.Parse(dataGridView6.Rows[i].Cells[3].Value.ToString());
                        Vision.Instance.DicLightSource[key].H4 = Int16.Parse(dataGridView6.Rows[i].Cells[4].Value.ToString());
                        //Vision.Instance.DicLightSource[key].H1 = byte.Parse(dataGridView6.Rows[i].Cells[2].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                Thread thread=new Thread(()=> {
                    halconRun.HobjClear();
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Restart(); 
                    for (int i = 0; i < dataGridView5.RowCount; i++)
                    {
                        OneResultOBj oneResultOBj = new OneResultOBj();
                        oneResultOBj.LiyID = 0;
                        oneResultOBj.RunID = i+1;
                        halconRun.SetResultOBj(oneResultOBj);
                        halconRun.GetStopwatch().Restart();
                        halconRun.SetCamPraegrm(i + 1);
                        halconRun.AddMeassge((i + 1) + "setCamP:" + halconRun.GetStopwatch().ElapsedMilliseconds + "ms");
                        Thread.Sleep((int)numericUpDown7.Value);
                        if (!halconRun.GetCam().GetImage(out IGrabbedRawData dse))
                        {
                            continue;
                        }
                        halconRun.SetImages(oneResultOBj, dse);
                        halconRun.AddMeassge((i+1)+":"+halconRun.GetStopwatch().ElapsedMilliseconds + "ms");
                    }
                    stopwatch.Stop();
                    halconRun.AddMeassge( "总时间:" + stopwatch.ElapsedMilliseconds + "ms");
                    halconRun.ShowObj();
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                Thread thread = new Thread(() => {
                    halconRun.HobjClear();
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Restart();
                    OneResultOBj oneResultOBj = new OneResultOBj();
                    oneResultOBj.LiyID = 0;
                    oneResultOBj.RunID = 1;
                    for (int i = 0; i < dataGridView5.RowCount; i++)
                    {
                        halconRun.SetResultOBj(oneResultOBj);
                        halconRun.GetStopwatch().Restart();
                        halconRun.SetCamPraegrm(i + 1);
                        halconRun.AddMeassge((i + 1) + "setCamP:" + halconRun.GetStopwatch().ElapsedMilliseconds + "ms");
                        Thread.Sleep((int)numericUpDown7.Value);
                        if (!halconRun.GetCam().GetImage(out IGrabbedRawData dse))
                        {
                            continue;
                        }
                       HObject hObject=  halconRun.GetCam().IGrabbedRawDataTOImage(dse);
                        oneResultOBj.Image= oneResultOBj.Image.ConcatObj(hObject);
                        //halconRun.SetImages(oneResultOBj, dse);
                        halconRun.AddMeassge((i + 1) + ":" + halconRun.GetStopwatch().ElapsedMilliseconds + "ms");
                    }
                    stopwatch.Stop();
                    halconRun.AddMeassge("总时间:" + stopwatch.ElapsedMilliseconds + "ms");
                    halconRun.ShowObj();
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                halconRun.SetCamPraegrm();
                halconRun.ReadCamImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}