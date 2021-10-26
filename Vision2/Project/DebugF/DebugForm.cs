using ErosSocket.DebugPLC.Robot;
using HalconDotNet;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Vision2.ConClass;
using Vision2.ErosProjcetDLL.Project;
using Vision2.Project.DebugF.IO;
using Vision2.Project.formula;
using Vision2.vision;
using static Vision2.vision.Vision;

namespace Vision2.Project.DebugF
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();

            if (!ProjectINI.DebugMode)
            {
            }
            toolStripDropDownButton1.Enabled = true;
        }

        private HWindID HWindNt = new HWindID();

        private ProductEX productEX;

        /// <summary>
        /// 新建产品过度点位
        /// </summary>
        private List<XYZPoint> xYZPoints1;

        private ProductEX PEX;

        /// <summary>
        /// 产品点位集合
        /// </summary>
        private List<XYZPoint> xYZPoints;

        /// <summary>
        /// 轨迹点
        /// </summary>
        private List<XYZPoint> RelativelyPoint;

        private List<ProductEX.Relatively.PointType> RelNamePoints;
        private bool isCot = true;

        private void dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new Form();
            SteppingControl axisDPUserControl1 = new SteppingControl();
            axisDPUserControl1.Dock = DockStyle.Fill;
            form.Width = axisDPUserControl1.Width;
            form.Height = axisDPUserControl1.Height;
            form.AutoSize = true;
            form.Controls.Add(axisDPUserControl1);
            form.Show();
        }

        private void DebugForm_Load(object sender, EventArgs e)
        {
            string ErrText = "";
            try
            {
                //ErrText = ErrText[-1].ToString();
                isCot = true;
                ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView7);
                foreach (var item in Vision.Instance.DefectTypeDicEx)
                {
                    int det = dataGridView7.Rows.Add();
                    dataGridView7.Rows[det].Cells[0].Tag = item.Value;
                    dataGridView7.Rows[det].Cells[0].Value = item.Value.DrawbackIndex;
                    dataGridView7.Rows[det].Cells[1].Value = item.Value.DrawbackName;
                    dataGridView7.Rows[det].Cells[2].Value = item.Value.DrawbackEnglish;
                }

                HWindNt.Initialize(hWindowControl2);
                Column8.Items.AddRange(Enum.GetNames(typeof(EnumXYZUMoveType)));
                Column11.Items.AddRange(Enum.GetNames(typeof(ProductEX.Relatively.EnumPointType)));
                dataGridViewComboBoxColumn3.Items.AddRange(Enum.GetNames(typeof(EnumXYZUMoveType)));
                dataGridViewComboBoxColumn1.Items.AddRange(Enum.GetNames(typeof(EnumXYZUMoveType)));
                Column7.Items.AddRange(DebugCompiler.Instance.DDAxis.AxisGrot.Keys.ToArray());
                dataGridViewComboBoxColumn2.Items.AddRange(DebugCompiler.Instance.DDAxis.AxisGrot.Keys.ToArray());
                dataGridViewComboBoxColumn4.Items.AddRange(DebugCompiler.Instance.DDAxis.AxisGrot.Keys.ToArray());
                ErrText = "通信机器人";
                //通信组合
                foreach (var item in ErosSocket.ErosConLink.StaticCon.SocketClint)
                {
                    if (item.Value is ErosSocket.DebugPLC.IAxisGrub)
                    {
                        ErosSocket.DebugPLC.IAxisGrub axisGrub = item.Value as ErosSocket.DebugPLC.IAxisGrub;
                        ToolStripButton toolStripItem = new ToolStripButton();
                        toolStripItem.Name = item.Key;
                        toolStripItem.Text = item.Key + "调试界面";
                        toolStripDropDownButton1.DropDownItems.Add(toolStripItem);
                        toolStripItem.Click += ToolStripItem_Click;
                        void ToolStripItem_Click(object senderw, EventArgs ee)
                        {
                            item.Value.ShowForm(ee.ToString());
                        }
                    }
                }
                int i = 0;
                ErrText = "轴组";
                for (i = 0; i < ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP.Count; i++)
                {
                    ToolStripButton toolStripItem = new ToolStripButton();
                    toolStripItem.Name = ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP[i].Name;
                    toolStripItem.Text = ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP[i].Name + "调试界面";
                    toolStripItem.Tag = ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP[i];
                    ErosSocket.DebugPLC.PLC.PLCAxis x = null, y = null, z = null, u = null;
                    foreach (var item in ErosSocket.DebugPLC.DebugComp.GetThis().DicAxes)
                    {
                        if (item.Value.Name == ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP[i].AsixXName)
                        {
                            x = item.Value;
                        }
                        if (item.Value.Name == ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP[i].AsixYName)
                        {
                            y = item.Value;
                        }
                        if (item.Value.Name == ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP[i].AsixZName)
                        {
                            z = item.Value;
                        }
                        if (item.Value.Name == ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP[i].AsixUName)
                        {
                            u = item.Value;
                        }
                    }
                    ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP[i].UpAxis(x, y, z, u);
                    toolStripDropDownButton1.DropDownItems.Add(toolStripItem);
                    toolStripDropDownButton1.DropDownItems.Add(toolStripItem);
                    toolStripItem.Click += ToolStripItem_Click;
                    void ToolStripItem_Click(object senderw, EventArgs ee)
                    {
                        try
                        {
                            ToolStripButton toolStripButton = (ToolStripButton)senderw;
                            AxisSPD axisSPD = toolStripButton.Tag as AxisSPD;
                            axisSPD.DebugFormShow();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                if (ErosSocket.DebugPLC.DebugComp.GetThis().ISPLCDebug)
                {
                    ErrText = "PLC";
                    TabPage tabPage = new TabPage() { Name = "PLC", Text = "PLC" };
                    PLC.PLCDebug plcDebug1 = new PLC.PLCDebug();
                    plcDebug1.Dock = DockStyle.Fill;
                    tabControl1.TabPages.Add(tabPage);
                    tabPage.Controls.Add(plcDebug1);
                }
                else
                {
                    ErrText = "板卡";
                    TabPage tabPage2 = new TabPage
                    {
                        Name = "硬件调试",
                        Text = "硬件调试"
                    };
                    MP_C154Form1 plcDebug2 = new MP_C154Form1();
                    plcDebug2.Dock = DockStyle.Fill;
                    plcDebug2.TopLevel = false;
                    plcDebug2.ShowInTaskbar = false;
                    plcDebug2.ControlBox = false;
                    plcDebug2.FormBorderStyle = FormBorderStyle.None;
                    this.tabControl1.TabPages.Add(tabPage2);
                    tabPage2.Controls.Add(plcDebug2);
                    plcDebug2.Show();
                }

                tabControl1.TabPages.RemoveByKey(tabPage5.Name);
                int dn = 0;
                ErrText = "通信机器人 ";
                foreach (var item in ErosSocket.ErosConLink.DicSocket.Instance.SocketClint)
                {
                    if (item.Value is EpsenRobot6)
                    {
                        EpsenRobot6 sd = (EpsenRobot6)item.Value;
                        dn = 0;
                        TabPage tabPage = new TabPage();
                        tabPage.Name = tabPage.Text = item.Key;
                        tabControl2.TabPages.Add(tabPage);
                        Button button2 = new Button();
                        button2.Height = 50;
                        button2.Width = 80;
                        button2.Click += Button2_Click;
                        void Button2_Click(object sender3, EventArgs e3)
                        {
                            try
                            {
                                sd.SendCommand("Quit");
                            }
                            catch (Exception)
                            {
                            }
                        }
                        button2.Text = button2.Name = "停止";
                        button2.Location = new Point(10, 10);
                        tabPage.Controls.Add(button2);
                        dn++;
                        foreach (var item2 in sd.DicSendMeseage)
                        {
                            Button button = new Button();
                            button.Height = 50;
                            button.Width = 80;
                            button.Text = button.Name = item2.Key;
                            button.Location = new Point(10, 10 + dn * button.Height);
                            button.Click += Button_Click;
                            tabPage.Controls.Add(button);
                            dn++;
                            void Button_Click(object sender1, EventArgs e2)
                            {
                                try
                                {
                                    sd.SendCommand(item2.Value);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
                // 显示标定
                dn = 0;
                TabPage tabPage1 = new TabPage();
                tabPage1.Name = tabPage1.Text = "3D标定";
                tabControl2.TabPages.Add(tabPage1);
                foreach (var item in Vision.Instance.DicCalib3D)
                {
                    if (item.Value.TRobotCall != null && item.Value.TRobotCall != "")
                    {
                        Button button = new Button();
                        button.Height = 50;
                        button.Width = 100;
                        button.Text = button.Name = item.Key + "固定标定";
                        button.Location = new Point(10, 10 + dn * button.Height);
                        button.Click += Button_Click;
                        Button button2 = new Button();
                        button2.Height = 50;
                        button2.Click += Button2_Click;
                        void Button2_Click(object sender3, EventArgs e3)
                        {
                            try
                            {
                                string[] datas = item.Value.TRobotCall.Split(',');
                                ErosSocket.ErosConLink.StaticCon.GetSocketClint(datas[0]).Send("Quit");
                            }
                            catch (Exception)
                            {
                            }
                        }
                        button2.Text = button2.Name = "停止";
                        button2.Location = new Point(10 + button.Width, 10 + dn * button.Height);
                        tabPage1.Controls.Add(button2);
                        tabPage1.Controls.Add(button);
                        dn++;
                        void Button_Click(object sender1, EventArgs e2)
                        {
                            try
                            {
                                string[] datas = item.Value.TRobotCall.Split(',');
                                if (datas.Length == 11)
                                {
                                    ErosSocket.ErosConLink.StaticCon.GetSocketClint(datas[0]).Send(item.Value.TRobotCall.Remove(0, datas[0].Length + 1));
                                }
                                else
                                {
                                    MessageBox.Show("标定参数不正确");
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    if (item.Value.MRobotCall != null && item.Value.MRobotCall != "")
                    {
                        Button button = new Button();
                        button.Height = 50;
                        button.Width = 100;
                        button.Text = button.Name = item.Key + "移动标定";
                        button.Location = new Point(10, 10 + dn * button.Height);
                        button.Click += Button_Click;
                        Button button2 = new Button();
                        button2.Height = 50;
                        button2.Click += Button2_Click;
                        void Button2_Click(object sender3, EventArgs e3)
                        {
                            try
                            {
                                string[] datas = item.Value.MRobotCall.Split(',');
                                ErosSocket.ErosConLink.StaticCon.GetSocketClint(datas[0]).Send("Quit");
                            }
                            catch (Exception)
                            {
                            }
                        }
                        button2.Text = button2.Name = "停止";
                        button2.Location = new Point(10 + button.Width, 10 + dn * button.Height);
                        tabPage1.Controls.Add(button2);
                        tabPage1.Controls.Add(button);
                        dn++;
                        void Button_Click(object sender1, EventArgs e2)
                        {
                            try
                            {
                                string[] datas = item.Value.MRobotCall.Split(',');
                                if (datas.Length == 11)
                                {
                                    ErosSocket.ErosConLink.StaticCon.GetSocketClint(datas[0]).Send(item.Value.MRobotCall.Remove(0, datas[0].Length + 1));
                                    //vision2UserControl1.UpHalcon(Vision.GetRunNameVision(datas[2]));
                                }
                                else
                                {
                                    MessageBox.Show("标定参数不正确");
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
                dataGridView4.Rows.Clear();
                ErrText = "全局点位";
                if (DebugCompiler.Instance.DDAxis.XyzPoints.Count != 0)
                {
                    dataGridView4.Rows.Add(DebugCompiler.Instance.DDAxis.XyzPoints.Count);
                    for (int idx = 0; idx < DebugCompiler.Instance.DDAxis.XyzPoints.Count; idx++)
                    {
                        XYZPoint xYZPoint = DebugCompiler.Instance.DDAxis.XyzPoints[idx];
                        dataGridView4.Rows[idx].Cells[0].Value = xYZPoint.Name;
                        dataGridView4.Rows[idx].Cells[1].Value = xYZPoint.X;
                        dataGridView4.Rows[idx].Cells[2].Value = xYZPoint.Y;
                        dataGridView4.Rows[idx].Cells[3].Value = xYZPoint.Z;
                        dataGridView4.Rows[idx].Cells[4].Value = xYZPoint.U;
                        dataGridView4.Rows[idx].Cells[5].Value = xYZPoint.ID;
                        dataGridView4.Rows[idx].Cells[6].Value = xYZPoint.isMove.ToString();
                        dataGridView4.Rows[idx].Cells[7].Value = xYZPoint.AxisGrabName;
                    }
                }

                UpData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载参数:" + ErrText + ex.Message + ex.StackTrace);
            }
            isCot = false;
        }

        private void toolStripDropDownButton1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                ProjectINI.In.SaveProjectAll();
                Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ex.Message);
            }
        }

        public void UpData()
        {
            try
            {
                listBox1.Items.Clear();

                listBox1.Items.AddRange(Product.GetThisP().Keys.ToArray());
                if (xYZPoints1 == null)
                {
                    foreach (var item in Product.GetThisP())
                    {
                        if (RecipeCompiler.Instance.ProductEX.ContainsKey(item.Key))
                        {
                            xYZPoints1 = RecipeCompiler.Instance.ProductEX[item.Key].DPoint;
                            break;
                        }
                    }
                }
                foreach (var item in Product.GetThisP())
                {
                    if (!RecipeCompiler.Instance.ProductEX.ContainsKey(item.Key))
                    {
                        RecipeCompiler.Instance.ProductEX.Add(item.Key, new ProductEX());

                        string da = ProjectINI.ClassToJsonString(PEX);
                        ProductEX Prod = new ProductEX();
                        ProjectINI.StringJsonToCalss<ProductEX>(da, out Prod);
                        RecipeCompiler.Instance.ProductEX[item.Key] = Prod;
                    }
                }
                Dictionary<string, ProductEX> produceEX = new Dictionary<string, ProductEX>();
                foreach (var item in RecipeCompiler.Instance.ProductEX)
                {
                    if (Product.GetThisP().ContainsKey(item.Key))
                    {
                        produceEX.Add(item.Key, item.Value);
                    }
                }
                RecipeCompiler.Instance.ProductEX = produceEX;
            }
            catch (Exception ex)
            {
                MessageBox.Show("刷新参数:" + ex.Message);
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
                propertyGrid1.SelectedObject = Product.GetProduct();
                this.formulaEditorControl1.AddDataGridConmlus(Product.GetListLinkNames.ToArray());
                this.formulaEditorControl1.Updatas(listBox1.SelectedItem.ToString());
                try
                {
                    if (listBox1.SelectedItem == null)
                    {
                        return;
                    }
                    this.isCot = true;
                    dataGridView1.Rows.Clear();
                    dataGridView3.Rows.Clear();
                    dataGridView2.Rows.Clear();
                    listBox3.Items.Clear();
                    toolStripComboBox1.Items.Clear();
                    listBox2.Items.Clear();
                    Column10.Items.Clear();
                    listBox4.Items.Clear();
                    productEX = RecipeCompiler.Instance.ProductEX[listBox1.SelectedItem.ToString()];
                    propertyGrid2.SelectedObject = productEX;
                    listBox4.Items.AddRange(productEX.Relativel.DicRelativelyPoint.Keys.ToArray());
                    toolStripLabel1.Text = listBox1.SelectedItem.ToString();
                    xYZPoints = RecipeCompiler.Instance.ProductEX[listBox1.SelectedItem.ToString()].DPoint;
                    HWindNt.HobjClear();
                    listBox3.Items.AddRange(productEX.Key_Navigation_Picture.Keys.ToArray());
                    if (listBox3.Items.Count != 0)
                    {
                        listBox3.SelectedIndex = 0;
                    }
                    if (listBox1.SelectedItem.ToString() == Product.ProductionName)
                    {
                        移动到点位ToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        移动到点位ToolStripMenuItem.Enabled = false;
                    }
                    for (int i = 0; i < xYZPoints.Count; i++)
                    {
                        int de = dataGridView1.Rows.Add();
                        XYZPoint xYZPoint = xYZPoints[i];
                        Column10.Items.AddRange(xYZPoint.Name);
                        toolStripComboBox1.Items.Add(xYZPoint.Name);
                        dataGridView1.Rows[de].Cells[0].Value = xYZPoint.Name;
                        dataGridView1.Rows[de].Cells[1].Value = xYZPoint.X;
                        dataGridView1.Rows[de].Cells[2].Value = xYZPoint.Y;
                        dataGridView1.Rows[de].Cells[3].Value = xYZPoint.Z;
                        dataGridView1.Rows[de].Cells[4].Value = xYZPoint.U;
                        dataGridView1.Rows[de].Cells[5].Value = xYZPoint.ID;
                        dataGridView1.Rows[de].Cells[6].Value = xYZPoint.isMove.ToString();
                        dataGridView1.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
                    }
                    listBox7.Items.Clear();
                    if (productEX != null)
                    {
                        for (int i = 0; i < productEX.Relativel.ListListPointName.Count; i++)
                        {
                            listBox7.Items.Add(i + 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.isCot = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region 配方

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string name = listBox1.SelectedItem.ToString();
                string newNmae = Interaction.InputBox("请输入名称", "另存配方", listBox1.SelectedItem.ToString(), 100, 100);
                if (newNmae != "")
                {
                    if (!RecipeCompiler.Instance.ProductEX.ContainsKey(newNmae))
                    {
                        PEX = RecipeCompiler.Instance.ProductEX[name];
                        string da = ProjectINI.ClassToJsonString(PEX);
                        ProductEX Prod = new ProductEX();
                        ProjectINI.StringJsonToCalss<ProductEX>(da, out Prod);
                        RecipeCompiler.Instance.ProductEX.Add(newNmae, Prod);
                        string path = ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\" + Vision.Instance.FileName + "\\";
                        CopyFolder1(path + name, path + newNmae);

                        Product.Add(name, newNmae);
                    }
                }
                UpData();
            }
            catch (Exception)
            {
            }
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择文件夹";
                fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + listBox1.SelectedItem.ToString();
                DialogResult dialog = ErosProjcetDLL.UI.PropertyGrid.FolderBrowserLauncher.ShowFolderBrowser(fbd);
                if (dialog == DialogResult.OK)
                {
                    ProjectINI.ClassToJsonSavePath(RecipeCompiler.Instance.Produc[listBox1.SelectedItem.ToString()], fbd.SelectedPath + "\\配方参数");
                    if (RecipeCompiler.Instance.ProductEX.ContainsKey(listBox1.SelectedItem.ToString()))
                    {
                        ProductEX xYZPoints = RecipeCompiler.Instance.ProductEX[listBox1.SelectedItem.ToString()];
                        ProjectINI.ClassToJsonSavePath(xYZPoints, fbd.SelectedPath + "\\产品参数");
                    }

                    string path = ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\" + Vision.Instance.FileName + "\\" + listBox1.SelectedItem.ToString();

                    CopyFolder1(path, fbd.SelectedPath);
                }
            }
            catch (Exception)
            {
            }
        }

        private void 导入产品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择文件夹";
                fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\";
                DialogResult dialog = ErosProjcetDLL.UI.PropertyGrid.FolderBrowserLauncher.ShowFolderBrowser(fbd);
                if (dialog == DialogResult.OK)
                {
                    string names = Path.GetFileNameWithoutExtension(fbd.SelectedPath);
                    ProductEX xYZPoints = new ProductEX();
                    if (!RecipeCompiler.Instance.ProductEX.ContainsKey(names))
                    {
                        RecipeCompiler.Instance.ProductEX.Add(names, xYZPoints);
                    }

                    if (ProjectINI.ReadPathJsonToCalss(fbd.SelectedPath + "//产品参数", out xYZPoints))
                    {
                        RecipeCompiler.Instance.ProductEX[names] = xYZPoints;
                    }
                    string path = ProjectINI.ProjectPathRun + "\\" + Vision.Instance.FileName + "\\";
                    CopyFolder1(fbd.SelectedPath, path + names);
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    if (ProjectINI.ReadPathJsonToCalss(fbd.SelectedPath + "//配方参数", out keyValuePairs))
                    {
                        if (RecipeCompiler.Instance.Produc.ContainsKey(names))
                        {
                            RecipeCompiler.Instance.Produc[names] = keyValuePairs;
                        }
                        else
                        {
                            RecipeCompiler.Instance.Produc.Add(names, keyValuePairs);
                        }
                        if (Product.GetThisP().ContainsKey(Name))
                        {
                            Product.GetThisP().Add(names, keyValuePairs);
                        }
                        Product.GetThisP()[names] = keyValuePairs;
                    }
                    UpData();
                }
            }
            catch (Exception)
            {
            }
        }

        private void 删除产品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                return;
            }
            Product.Remove(listBox1.SelectedItem.ToString());
            UpData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (ProjectINI.Enbt || ProjectINI.GetUserJurisdiction("管理权限"))
            {
                Product.SaveDicExcel(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\产品配方\\产品文件");
            }
            else
            {
                MessageBox.Show("权限不足无法保存修改");
            }
            Cursor = Cursors.Arrow;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            try
            {
                openFileDialog.InitialDirectory = ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\产品配方\\";
                openFileDialog.Filter = "文件|*";
                DialogResult dialog = openFileDialog.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    if (Product.ReadExcelDic(openFileDialog.FileName, out string err))
                    {
                        MessageBox.Show("读取成功" + err);
                    }
                    else
                    {
                        MessageBox.Show("读取失败" + err);
                    }
                    UpData();
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            UpData();
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    Product.Aotu(listBox1.SelectedItem.ToString(), ErosSocket.ErosConLink.DicSocket.Instance.SocketClint);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\产品配方");
                System.Diagnostics.Process.Start(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\产品配方");
            }
            catch (Exception)
            {
            }
        }

        private void 管理参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormulaPrForm formulaPrForm = new FormulaPrForm();
            formulaPrForm.Show();
        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string NewName = Product.AmendName(listBox1.SelectedItem.ToString());
                if (NewName != "")
                {
                    string path = ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\" + Vision.Instance.FileName + "\\" + listBox1.SelectedItem.ToString();
                    Product.SaveDicExcel(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\产品配方\\产品文件");
                    if (!Directory.Exists(path))
                    {
                        MessageBox.Show("未创建图像程序[" + listBox1.SelectedItem.ToString() + "]", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (MessageBox.Show("是否修改图像程序名？", NewName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            string Newpath = ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\" + Vision.Instance.FileName + "\\" + NewName;
                            Directory.Move(path, Newpath);
                        }
                    }
                    if (RecipeCompiler.Instance.ProductEX.ContainsKey(listBox1.SelectedItem.ToString()))
                    {
                        ProductEX xYZPoints = RecipeCompiler.Instance.ProductEX[listBox1.SelectedItem.ToString()];
                        RecipeCompiler.Instance.ProductEX.Remove(listBox1.SelectedItem.ToString());
                        RecipeCompiler.Instance.ProductEX.Add(NewName, xYZPoints);
                    }
                    UpData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Application.StartupPath + @"\help.chm"))
                {
                    string url = Application.StartupPath + @"\help.chm::调试1.htm";
                    Help.ShowHelp(this, Application.StartupPath + @"\help.chm", HelpNavigator.Topic, "3_调试位置.htm");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 备份配方文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProjectINI.ClassToJsonSavePath(Product.GetThisP(), ProjectINI.ProjectPathRun + "\\产品配方\\配方备份\\配方文件" + DateTime.Now.ToString("yy年MM月dd日HH时mm分ss秒")))
                {
                    MessageBox.Show("备份成功");
                }
            }
            catch (Exception)
            {
            }
        }

        private void 导入配方备份ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, Dictionary<string, string>> keyValuePairs = new Dictionary<string, Dictionary<string, string>>();
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (Directory.Exists(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\产品配方\\配方备份\\"))
                {
                    openFileDialog.InitialDirectory = ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\产品配方\\配方备份\\";
                }
                else
                {
                    openFileDialog.InitialDirectory = ErosProjcetDLL.Project.ProjectINI.ProjectPathRun + "\\产品配方\\";
                }
                openFileDialog.Filter = "文件|*.txt";
                DialogResult dialog = openFileDialog.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    if (ProjectINI.ReadPathJsonToCalss(openFileDialog.FileName, out keyValuePairs))
                    {
                        RecipeCompiler.Instance.Produc = keyValuePairs;
                        Product.GetThisP(keyValuePairs);
                        MessageBox.Show("导入成功，共" + keyValuePairs.Count + "个配方");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 复制文件夹及文件
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public int CopyFolder2(string sourceFolder, string destFolder)
        {
            try
            {
                string folderName = System.IO.Path.GetFileName(sourceFolder);
                string destfolderdir = System.IO.Path.Combine(destFolder, folderName);
                string[] filenames = System.IO.Directory.GetFileSystemEntries(sourceFolder);
                foreach (string file in filenames)// 遍历所有的文件和目录
                {
                    if (System.IO.Directory.Exists(file))
                    {
                        string currentdir = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(currentdir))
                        {
                            System.IO.Directory.CreateDirectory(currentdir);
                        }
                        CopyFolder2(file, destfolderdir);
                    }
                    else
                    {
                        string srcfileName = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(destfolderdir))
                        {
                            System.IO.Directory.CreateDirectory(destfolderdir);
                        }
                        System.IO.File.Copy(file, srcfileName, true);
                    }
                }

                return 1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// 复制文件夹到新地址并改名称
        /// </summary>
        /// <param name="sourceFolder">目标文件夹</param>
        /// <param name="destFolder">目标地址及新名称</param>
        /// <returns></returns>
        public int CopyFolder1(string sourceFolder, string destFolder)
        {
            try
            {
                string folderName = Path.GetFileName(sourceFolder);
                string destfolderdir = destFolder;/* Path.Combine(destFolder, folderName);*/
                string[] filenames = Directory.GetFileSystemEntries(sourceFolder);
                foreach (string file in filenames)// 遍历所有的文件和目录
                {
                    if (System.IO.Directory.Exists(file))
                    {
                        string currentdir = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(currentdir))
                        {
                            System.IO.Directory.CreateDirectory(currentdir);
                        }
                        CopyFolder2(file, destfolderdir);
                    }
                    else
                    {
                        string srcfileName = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(destfolderdir))
                        {
                            System.IO.Directory.CreateDirectory(destfolderdir);
                        }
                        File.Copy(file, srcfileName, true);
                    }
                }

                return 1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            string name = Product.ProductionName;
            Product product = new Product(true);
            xYZPoints1 = RecipeCompiler.Instance.ProductEX[name].DPoint;
            PEX = RecipeCompiler.Instance.ProductEX[name];
            //RelativelyPoint = RecipeCompiler.Instance.ProductEX[name].Relativel.RelativelyPoint;
            if (Product.ProductionName != name)
            {
                vision.Vision.Instance.SaveRunPojcet();
            }
            UpData();
        }

        #endregion 配方

        #region 产品点位

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                bool flag = e.ColumnIndex == 8;
                this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                if (flag)
                {
                    if (listBox1.SelectedItem.ToString() == formula.Product.ProductionName)
                    {
                        int de = e.RowIndex;
                        this.dataGridView1.Rows[de].DefaultCellStyle.BackColor = Color.Green;
                        Thread thread = new Thread(() =>
                        {
                            try
                            {
                                Enum.TryParse<EnumXYZUMoveType>(this.dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString(), out EnumXYZUMoveType enumXYZUMoveType);
                                bool flag2 = DebugCompiler.Instance.DDAxis.SetXYZ1Points(this.dataGridView1.Rows[de].Cells[7].Value.ToString(), 15,
                           double.Parse(this.dataGridView1.Rows[de].Cells[1].Value.ToString()), double.Parse(this.dataGridView1.Rows[de].Cells[2].Value.ToString()),
                         Convert.ToDouble(this.dataGridView1.Rows[de].Cells[3].Value), Convert.ToDouble(this.dataGridView1.Rows[de].Cells[4].Value),
                           enumXYZUMoveType);

                                if (!flag2)
                                {
                                    this.dataGridView1.Rows[de].DefaultCellStyle.BackColor = Color.Red;
                                }
                                Thread.Sleep(1000);
                                foreach (var item in Vision.GetHimageList().Keys)
                                {
                                    if (this.dataGridView1.Rows[de].Cells[7].Value.ToString() == Vision.GetSaveImageInfo(item).AxisGrot)
                                    {
                                        int det = int.Parse(this.dataGridView1.Rows[de].Cells[5].Value.ToString());
                                        if (det <= 0)
                                        {
                                            Vision.GetRunNameVision(item).HobjClear();
                                            if (Vision.GetRunNameVision(item).GetCam().GetImage(out HObject image))
                                            {
                                                Vision.GetRunNameVision(item).Image(image);
                                            }
                                            else
                                            {
                                                Vision.GetRunNameVision(item).Image().GenEmptyObj();
                                            }
                                        }
                                        else
                                        {
                                            Vision.GetRunNameVision(item).ReadCamImage(this.dataGridView1.Rows[de].Cells[5].Value.ToString(), int.Parse(this.dataGridView1.Rows[de].Cells[5].Value.ToString()));
                                        }
                                        Vision.GetRunNameVision(item).ShowObj();
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        });
                        thread.IsBackground = true;
                        thread.Start();
                    }
                    else
                    {
                        MessageBox.Show("非当前生产的产品无法移动!");
                    }
                    this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.isCot)
            {
                try
                {
                    xYZPoints[e.RowIndex].Name = this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    double eX;
                    if (double.TryParse(this.dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), out eX))
                    {
                        xYZPoints[e.RowIndex].X = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "X值错误");
                    }
                    if (double.TryParse(this.dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(), out eX))
                    {
                        xYZPoints[e.RowIndex].Y = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "Y值错误");
                    }
                    if (this.dataGridView1.Rows[e.RowIndex].Cells[3].Value != null)
                    {
                        xYZPoints[e.RowIndex].Z = Convert.ToDouble(this.dataGridView1.Rows[e.RowIndex].Cells[3].Value);
                    }
                    if (this.dataGridView1.Rows[e.RowIndex].Cells[4].Value != null)
                    {
                        if (double.TryParse(this.dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString(), out eX))
                        {
                            xYZPoints[e.RowIndex].U = eX;
                        }
                        else
                        {
                            MessageBox.Show(e.RowIndex.ToString() + "U值错误");
                        }
                    }
                    int iX;
                    if (int.TryParse(this.dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString(), out iX))
                    {
                        xYZPoints[e.RowIndex].ID = iX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "ID值错误");
                    }
                    Enum.TryParse<EnumXYZUMoveType>(this.dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString(), out EnumXYZUMoveType enumXYZUMoveType);
                    xYZPoints[e.RowIndex].isMove = enumXYZUMoveType;
                    if (this.dataGridView1.Rows[e.RowIndex].Cells[7].Value != null)
                    {
                        xYZPoints[e.RowIndex].AxisGrabName = this.dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    }
                    this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                catch (Exception)
                {
                }
            }
        }

        private void 读取当前位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                int de = dataGridView1.SelectedCells[0].RowIndex;
                DebugCompiler.Instance.DDAxis.GetAxisGroupPoints(this.dataGridView1.Rows[de].Cells[7].Value.ToString(), out double? XpT, out double? Ypt, out double? zpt, out double? u);
                xYZPoints[de].Name = this.dataGridView1.Rows[de].Cells[0].Value.ToString();
                xYZPoints[de].X = XpT.Value;
                xYZPoints[de].Y = Ypt.Value;
                if (zpt != null)
                {
                    xYZPoints[de].Z = zpt.Value;
                }
                if (u != null)
                {
                    xYZPoints[de].U = u.Value;
                }
                this.dataGridView1.Rows[de].Cells[1].Value = XpT;
                this.dataGridView1.Rows[de].Cells[2].Value = Ypt;
                this.dataGridView1.Rows[de].Cells[3].Value = zpt;
                this.dataGridView1.Rows[de].Cells[4].Value = u;
            }
            catch (Exception ex)
            {
            }
            isCot = false;
        }

        private void 添加新点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                int de = dataGridView1.Rows.Add();
                XYZPoint xYZPoint = new XYZPoint();
                xYZPoints.Add(xYZPoint);
                xYZPoint.Name = "P" + de;
                dataGridView1.Rows[de].Cells[0].Value = xYZPoint.Name;
                dataGridView1.Rows[de].Cells[1].Value = xYZPoint.X;
                dataGridView1.Rows[de].Cells[2].Value = xYZPoint.Y;
                dataGridView1.Rows[de].Cells[3].Value = xYZPoint.Z;
                dataGridView1.Rows[de].Cells[4].Value = xYZPoint.U;
                dataGridView1.Rows[de].Cells[5].Value = xYZPoint.ID;
                dataGridView1.Rows[de].Cells[6].Value = xYZPoint.isMove;
                xYZPoint.AxisGrabName = Column7.Items[0].ToString();
                dataGridView1.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
            }
            catch (Exception)
            {
            }
            isCot = false;
        }

        private void 移动到点位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int de = dataGridView1.SelectedCells[0].RowIndex;
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        Enum.TryParse<EnumXYZUMoveType>(this.dataGridView1.Rows[de].Cells[6].Value.ToString(), out EnumXYZUMoveType enumXYZUMoveType);
                        bool flag2 = DebugCompiler.Instance.DDAxis.SetXYZ1Points(this.dataGridView1.Rows[de].Cells[7].Value.ToString(), 15,
                          double.Parse(this.dataGridView1.Rows[de].Cells[1].Value.ToString()), double.Parse(this.dataGridView1.Rows[de].Cells[2].Value.ToString()),
                      Convert.ToDouble(this.dataGridView1.Rows[de].Cells[3].Value), Convert.ToDouble(this.dataGridView1.Rows[de].Cells[4].Value), enumXYZUMoveType);
                    }
                    catch (Exception)
                    {
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 删除点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                int det = dataGridView1.SelectedRows.Count;
                if (MessageBox.Show("删除点", "是否删除点位" + det, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int i = 0; i < det; i++)
                    {
                        xYZPoints.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 插入新点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                int de = 0;
                if (dataGridView1.SelectedCells.Count != 0)
                {
                    de = dataGridView1.SelectedCells[0].RowIndex + 1;
                }
                xYZPoints.Insert(de, new XYZPoint());
                dataGridView1.Rows.Insert(de, new DataGridViewRow());
                XYZPoint xYZPoint = xYZPoints[de];

                if (de != 0)
                {
                    ProjectINI.GetStrReturnInt(xYZPoints[de - 1].Name, out string names);

                    xYZPoint.Name = names + (ProjectINI.GetStrReturnInt(xYZPoints[de - 1].Name) + 1);
                }
                dataGridView1.Rows[de].Cells[0].Value = xYZPoint.Name;
                dataGridView1.Rows[de].Cells[1].Value = xYZPoint.X;
                dataGridView1.Rows[de].Cells[2].Value = xYZPoint.Y;
                dataGridView1.Rows[de].Cells[3].Value = xYZPoint.Z;
                dataGridView1.Rows[de].Cells[4].Value = xYZPoint.U;
                dataGridView1.Rows[de].Cells[5].Value = xYZPoint.ID;
                dataGridView1.Rows[de].Cells[6].Value = xYZPoint.isMove;
                xYZPoint.AxisGrabName = Column7.Items[0].ToString();
                dataGridView1.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isCot = false;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        #endregion 产品点位

        #region 轨迹

        private XYZPoint zoer;

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.isCot)
            {
                try
                {
                    RelativelyPoint[e.RowIndex].Name = this.dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                    double eX;
                    if (double.TryParse(this.dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString(), out eX))
                    {
                        RelativelyPoint[e.RowIndex].X = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "X值错误");
                    }
                    if (double.TryParse(this.dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString(), out eX))
                    {
                        RelativelyPoint[e.RowIndex].Y = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "Y值错误");
                    }

                    if (double.TryParse(this.dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString(), out eX))
                    {
                        RelativelyPoint[e.RowIndex].Z = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "Z值错误");
                    }
                    if (double.TryParse(this.dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString(), out eX))
                    {
                        RelativelyPoint[e.RowIndex].U = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "U值错误");
                    }
                    int iX;

                    if (int.TryParse(this.dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString(), out iX))
                    {
                        RelativelyPoint[e.RowIndex].ID = iX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "ID值错误");
                    }

                    Enum.TryParse<EnumXYZUMoveType>(this.dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString(), out EnumXYZUMoveType enumXYZUMoveType);
                    RelativelyPoint[e.RowIndex].isMove = enumXYZUMoveType;
                    if (this.dataGridView2.Rows[e.RowIndex].Cells[7].Value != null)
                    {
                        RelativelyPoint[e.RowIndex].AxisGrabName = this.dataGridView2.Rows[e.RowIndex].Cells[7].Value.ToString();
                    }
                    this.dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            try
            {
                int de = dataGridView1.SelectedCells[0].RowIndex;
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        productEX.Relativel.RelativelyId = 0;
                        toolStripLabel3.Text = "流程步:" + productEX.Relativel.RelativelyId;
                        if (toolStripComboBox1.SelectedIndex < 0)
                        {
                            MessageBox.Show("未选择起点");
                            return;
                        }
                        XYZPoint xYZPoint = xYZPoints[toolStripComboBox1.SelectedIndex];
                        zoer = xYZPoint;
                        bool flag2 = DebugCompiler.Instance.DDAxis.SetXYZ1Points(xYZPoint.AxisGrabName, 15, xYZPoint.X, xYZPoint.Y, xYZPoint.Z, xYZPoint.U, xYZPoint.isMove);
                        if (flag2)
                        {
                            for (int i = 0; i < dataGridView2.Rows.Count; i++)
                            {
                                this.dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.White;
                            }
                        }
                        else
                        {
                            MessageBox.Show("移动失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            try
            {
                int de = dataGridView1.SelectedCells[0].RowIndex;
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        if (productEX.Relativel.RelativelyId == 0)
                        {
                            return;
                        }
                        if (productEX.Relativel.RelativelyId > 0)
                        {
                            productEX.Relativel.RelativelyId--;
                        }
                        this.dataGridView2.Rows[productEX.Relativel.RelativelyId].DefaultCellStyle.BackColor = Color.Yellow;
                        toolStripLabel3.Text = "流程步:" + productEX.Relativel.RelativelyId;
                        XYZPoint xYZPoi = RelativelyPoint[productEX.Relativel.RelativelyId];
                        XYZPoint xYz = new XYZPoint()
                        {
                            isMove = xYZPoi.isMove,
                            AxisGrabName = xYZPoi.AxisGrabName,
                            X = zoer.X - xYZPoi.X,
                            Y = zoer.Y - xYZPoi.Y,
                            U = zoer.U - xYZPoi.U,
                            ID = xYZPoi.ID,
                            Name = xYZPoi.Name,
                            Z = zoer.Z - xYZPoi.Z,
                        };
                        zoer = xYz;
                        bool flag2;
                        if (xYz.isMove == EnumXYZUMoveType.先移动再旋转)
                        {
                            flag2 = DebugCompiler.Instance.DDAxis.SetXYZ1Points(xYz.AxisGrabName, 15, xYz.X, xYz.Y, xYz.Z, xYz.U, EnumXYZUMoveType.先旋转再移动);
                        }
                        if (xYz.isMove == EnumXYZUMoveType.先旋转再移动)
                        {
                            flag2 = DebugCompiler.Instance.DDAxis.SetXYZ1Points(xYz.AxisGrabName, 15, xYz.X, xYz.Y, xYz.Z, xYz.U, EnumXYZUMoveType.先移动再旋转);
                        }
                        else
                        {
                            flag2 = DebugCompiler.Instance.DDAxis.SetXYZ1Points(xYz.AxisGrabName, 15, xYz.X, xYz.Y, xYz.Z, xYz.U, xYz.isMove);
                        }
                        if (flag2)
                        {
                            Thread.Sleep(500);
                            foreach (var item in Vision.GetHimageList().Keys)
                            {
                                if (xYz.AxisGrabName == Vision.GetSaveImageInfo(item).AxisGrot)
                                {
                                    if (productEX.Relativel.RelativelyId <= 0)
                                    {
                                        if (Vision.GetRunNameVision(item).GetCam().GetImage(out HObject image))
                                        {
                                            Vision.GetRunNameVision(item).Image(image);
                                        }
                                        else
                                        {
                                            Vision.GetRunNameVision(item).Image().GenEmptyObj();
                                        }
                                        //Vision.GetRunNameVision(item).Image(Vision.GetRunNameVision(item).GetCam().GetImage());
                                    }
                                    else
                                    {
                                        Vision.GetRunNameVision(item).ReadCamImage(productEX.Relativel.RelativelyId.ToString(), productEX.Relativel.RelativelyId);
                                    }
                                    break;
                                }
                            }
                            this.dataGridView2.Rows[productEX.Relativel.RelativelyId].DefaultCellStyle.BackColor = Color.Blue;
                        }
                        else
                        {
                            this.dataGridView2.Rows[productEX.Relativel.RelativelyId].DefaultCellStyle.BackColor = Color.Red;
                            MessageBox.Show("移动失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("移动失败" + ex.Message);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            try
            {
                int de = dataGridView1.SelectedCells[0].RowIndex;
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        XYZPoint xYZPoi = RelativelyPoint[productEX.Relativel.RelativelyId];
                        XYZPoint xYz = new XYZPoint()
                        {
                            isMove = xYZPoi.isMove,
                            AxisGrabName = xYZPoi.AxisGrabName,
                            X = zoer.X + xYZPoi.X,
                            Y = xYZPoi.Y + zoer.Y,
                            U = xYZPoi.U + zoer.U,
                            ID = xYZPoi.ID,
                            Name = xYZPoi.Name,
                            Z = xYZPoi.Z + zoer.Z,
                        };
                        zoer = xYz;
                        productEX.Relativel.RelativelyId++;
                        this.dataGridView2.Rows[productEX.Relativel.RelativelyId - 1].DefaultCellStyle.BackColor = Color.Yellow;
                        bool flag2 = DebugCompiler.Instance.DDAxis.SetXYZ1Points(xYz.AxisGrabName, 15, xYz.X, xYz.Y, xYz.Z, xYz.U, xYz.isMove);
                        if (flag2)
                        {
                            Thread.Sleep(500);
                            foreach (var item in Vision.GetHimageList().Keys)
                            {
                                if (xYz.AxisGrabName == Vision.GetSaveImageInfo(item).AxisGrot)
                                {
                                    if (productEX.Relativel.RelativelyId <= 0)
                                    {
                                        if (Vision.GetRunNameVision(item).GetCam().GetImage(out HObject image))
                                        {
                                            Vision.GetRunNameVision(item).Image(image);
                                        }
                                        else
                                        {
                                            Vision.GetRunNameVision(item).Image().GenEmptyObj();
                                        }
                                        //Vision.GetRunNameVision(item).Image(Vision.GetRunNameVision(item).GetCam().GetImage());
                                    }
                                    else
                                    {
                                        Vision.GetRunNameVision(item).ReadCamImage(productEX.Relativel.RelativelyId.ToString(), productEX.Relativel.RelativelyId);
                                    }
                                    break;
                                }
                            }
                            this.dataGridView2.Rows[productEX.Relativel.RelativelyId - 1].DefaultCellStyle.BackColor = Color.GreenYellow;
                        }
                        else
                        {
                            if (productEX.Relativel.RelativelyId != 1)
                            {
                                this.dataGridView2.Rows[productEX.Relativel.RelativelyId - 1].DefaultCellStyle.BackColor = Color.Red;
                            }
                            MessageBox.Show("移动失败");
                        }
                        toolStripLabel3.Text = "流程步:" + productEX.Relativel.RelativelyId;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("移动失败" + ex.Message);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                if (toolStripComboBox1.SelectedItem != null)
                {
                    List<XYZPoint> xes = new List<XYZPoint>();
                    XYZPoint xYZPoint0 = productEX.GetPoint(toolStripComboBox1.SelectedItem.ToString());
                    int index = productEX.GetPointIntdx(toolStripComboBox1.SelectedItem.ToString());
                    if (xYZPoint0 != null)
                    {
                        List<XYZPoint> xYZPoints = new List<XYZPoint>();
                        for (int i = index; i < productEX.DPoint.Count; i++)
                        {
                            if (productEX.DPoint[i].AxisGrabName == xYZPoint0.AxisGrabName)
                            {
                                xYZPoints.Add(productEX.DPoint[i]);
                            }
                        }
                        XYZPoint xYZPoint1 = new XYZPoint();
                        xYZPoint1.X = 0;
                        xYZPoint1.Y = 0;
                        xYZPoint1.Z = 0;
                        xYZPoint1.U = 0;
                        xYZPoint1.AxisGrabName = xYZPoint0.AxisGrabName;
                        xYZPoint1.ID = xYZPoint0.ID;
                        xYZPoint1.isMove = xYZPoint0.isMove;
                        xYZPoint1.Name = "PX1";
                        xes.Add(xYZPoint1);
                        for (int i = 0; i < xYZPoints.Count - 1; i++)
                        {
                            xYZPoint0 = xYZPoints[i];
                            XYZPoint xYZPoint2 = xYZPoints[i + 1];
                            xYZPoint1 = new XYZPoint();
                            xYZPoint1.X = Math.Round(xYZPoint2.X - xYZPoint0.X, 2);
                            xYZPoint1.Y = Math.Round(xYZPoint2.Y - xYZPoint0.Y, 2);
                            xYZPoint1.Z = Math.Round(xYZPoint2.Z - xYZPoint0.Z, 2);
                            xYZPoint1.U = Math.Round(xYZPoint2.U - xYZPoint0.U, 2);
                            xYZPoint1.AxisGrabName = xYZPoint0.AxisGrabName;
                            xYZPoint1.ID = xYZPoint2.ID;
                            xYZPoint1.isMove = xYZPoint2.isMove;
                            xYZPoint1.Name = "PX" + (i + 2);
                            xes.Add(xYZPoint1);
                        }
                        productEX.Relativel.DicRelativelyPoint[listBox4.SelectedItem.ToString()].AddRange(xes);
                        dataGridView2.Rows.Clear();
                        if (RelativelyPoint != null)
                        {
                            for (int i = 0; i < RelativelyPoint.Count; i++)
                            {
                                int de = dataGridView2.Rows.Add();
                                XYZPoint xYZPoint = RelativelyPoint[i];
                                dataGridView2.Rows[de].Cells[0].Value = xYZPoint.Name;
                                dataGridView2.Rows[de].Cells[1].Value = xYZPoint.X;
                                dataGridView2.Rows[de].Cells[2].Value = xYZPoint.Y;
                                dataGridView2.Rows[de].Cells[3].Value = xYZPoint.Z;
                                dataGridView2.Rows[de].Cells[4].Value = xYZPoint.U;
                                dataGridView2.Rows[de].Cells[5].Value = xYZPoint.ID;
                                dataGridView2.Rows[de].Cells[6].Value = xYZPoint.isMove.ToString();
                                dataGridView2.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("未选择起点");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isCot = false;
        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.isCot = true;
                Column12.Items.Clear();
                Column12.Items.AddRange(productEX.Relativel.DicRelativelyPoint.Keys.ToArray());
                dataGridView3.Rows.Clear();
                RelNamePoints = productEX.Relativel.ListListPointName[listBox7.SelectedIndex];
                for (int i = 0; i < RelNamePoints.Count; i++)
                {
                    int de = dataGridView3.Rows.Add();
                    dataGridView3.Rows[de].Cells[0].Value = RelNamePoints[i].PointNmae;
                    dataGridView3.Rows[de].Cells[1].Value = RelNamePoints[i].EnumPointTyp.ToString();
                    dataGridView3.Rows[de].Cells[2].Value = RelNamePoints[i].RelativeLyPiintName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.isCot = false;
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox4.SelectedItem == null)
                {
                    return;
                }
                isCot = true;
                RelativelyPoint = productEX.Relativel.DicRelativelyPoint[listBox4.SelectedItem.ToString()];
                dataGridView2.Rows.Clear();
                if (RelativelyPoint != null)
                {
                    for (int i = 0; i < RelativelyPoint.Count; i++)
                    {
                        int de = dataGridView2.Rows.Add();
                        XYZPoint xYZPoint = RelativelyPoint[i];
                        dataGridView2.Rows[de].Cells[0].Value = xYZPoint.Name;
                        dataGridView2.Rows[de].Cells[1].Value = xYZPoint.X;
                        dataGridView2.Rows[de].Cells[2].Value = xYZPoint.Y;
                        dataGridView2.Rows[de].Cells[3].Value = xYZPoint.Z;
                        dataGridView2.Rows[de].Cells[4].Value = xYZPoint.U;
                        dataGridView2.Rows[de].Cells[5].Value = xYZPoint.ID;
                        dataGridView2.Rows[de].Cells[6].Value = xYZPoint.isMove.ToString();
                        dataGridView2.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isCot = false;
        }

        private void 删除ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (!this.isCot)
            {
                try
                {
                    int det = dataGridView3.SelectedCells.Count;
                    if (MessageBox.Show("删除点", "是否删除点位" + det, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        for (int i = 0; i < det; i++)
                        {
                            RelNamePoints.RemoveAt(dataGridView3.SelectedCells[0].RowIndex);
                            dataGridView3.Rows.RemoveAt(dataGridView3.SelectedCells[0].RowIndex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void 添加轨迹ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                productEX.Relativel.ListListPointName.Add(new List<ProductEX.Relatively.PointType>());
                listBox7.Items.Add(productEX.Relativel.ListListPointName.Count());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 删除轨迹ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                productEX.Relativel.ListListPointName.RemoveAt(listBox7.SelectedIndex);
                listBox7.Items.RemoveAt(listBox7.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 添加轨迹ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string meassge = "创建轨迹";
                string names = "轨迹1";
                if (listBox4.Items.Count != 0)
                {
                    names = ProjectINI.GetStrReturnStr(listBox4.Items[listBox4.Items.Count - 1].ToString());
                }
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, names, 100, 100);
                if (sd == "")
                {
                    return;
                }
                if (productEX.Relativel.DicRelativelyPoint.ContainsKey(sd))
                {
                    meassge = "名称已存在";
                    goto st;
                }
                productEX.Relativel.DicRelativelyPoint.Add(sd, new List<XYZPoint>());
                listBox4.Items.Add(sd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 删除轨迹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                productEX.Relativel.DicRelativelyPoint.Remove(listBox4.SelectedItem.ToString());
                listBox4.Items.RemoveAt(listBox4.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 重命名ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string meassge = "重命名";
                string names = listBox4.SelectedItem.ToString();
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, names, 100, 100);
                if (productEX.Relativel.DicRelativelyPoint.ContainsKey(sd))
                {
                    meassge = "名称已存在";
                    goto st;
                }
                productEX.Relativel.DicRelativelyPoint.Add(sd, productEX.Relativel.DicRelativelyPoint[names]);
                listBox4.Items.Add(sd);
                productEX.Relativel.DicRelativelyPoint.Remove(names);
                listBox4.Items.Remove(names);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 移动轨迹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int det = dataGridView3.SelectedCells.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 读取轨迹位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }

        private void 添加轨迹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                int de = dataGridView2.Rows.Add();
                XYZPoint xYZPoint = new XYZPoint();
                RelativelyPoint.Add(xYZPoint);
                xYZPoint.Name = "P" + de;
                dataGridView2.Rows[de].Cells[0].Value = xYZPoint.Name;
                dataGridView2.Rows[de].Cells[1].Value = xYZPoint.X;
                dataGridView2.Rows[de].Cells[2].Value = xYZPoint.Y;
                dataGridView2.Rows[de].Cells[3].Value = xYZPoint.Z;
                dataGridView2.Rows[de].Cells[4].Value = xYZPoint.U;
                dataGridView2.Rows[de].Cells[5].Value = xYZPoint.ID;
                dataGridView2.Rows[de].Cells[6].Value = xYZPoint.isMove;
                xYZPoint.AxisGrabName = dataGridViewComboBoxColumn4.Items[0].ToString();
                dataGridView2.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
            }
            catch (Exception)
            {
            }
            isCot = false;
        }

        private void 插入轨迹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                int de = 0;
                if (dataGridView2.SelectedCells.Count != 0)
                {
                    de = dataGridView2.SelectedCells[0].RowIndex + 1;
                }
                RelativelyPoint.Insert(de, new XYZPoint());
                dataGridView2.Rows.Insert(de, new DataGridViewRow());
                XYZPoint xYZPoint = RelativelyPoint[de];
                if (de != 0)
                {
                    ProjectINI.GetStrReturnInt(RelativelyPoint[de - 1].Name, out string names);

                    xYZPoint.Name = names + (ProjectINI.GetStrReturnInt(RelativelyPoint[de - 1].Name) + 1);
                }
                dataGridView2.Rows[de].Cells[0].Value = xYZPoint.Name;
                dataGridView2.Rows[de].Cells[1].Value = xYZPoint.X;
                dataGridView2.Rows[de].Cells[2].Value = xYZPoint.Y;
                dataGridView2.Rows[de].Cells[3].Value = xYZPoint.Z;
                dataGridView2.Rows[de].Cells[4].Value = xYZPoint.U;
                dataGridView2.Rows[de].Cells[5].Value = xYZPoint.ID;
                dataGridView2.Rows[de].Cells[6].Value = xYZPoint.isMove;
                xYZPoint.AxisGrabName = dataGridViewComboBoxColumn4.Items[0].ToString();
                dataGridView2.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
            }
            catch (Exception ex)
            {
            }
            isCot = false;
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                int det = dataGridView2.SelectedRows.Count;
                if (MessageBox.Show("删除点", "是否删除点位" + det, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int i = 0; i < det; i++)
                    {
                        RelativelyPoint.RemoveAt(dataGridView2.SelectedCells[0].RowIndex);
                        dataGridView2.Rows.RemoveAt(dataGridView2.SelectedCells[0].RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion 轨迹

        #region 导航图

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HWindNt.HobjClear();
                if (listBox2.SelectedItem != null)
                {
                    if (productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.ContainsKey(listBox2.SelectedItem.ToString()))
                    {
                        HObject hObject = productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi[listBox2.SelectedItem.ToString()];

                        if (hObject.CountObj() != 0)
                        {
                            HWindNt.OneResIamge.AddObj(hObject);
                            HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple rows, out HTuple clos);
                            HWindNt.OneResIamge.AddImageMassage(rows, clos, listBox2.SelectedItem.ToString());
                        }
                    }
                    HWindNt.ShowImage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.Clear();
                HWindNt.HobjClear();
                listBox2.Items.AddRange(productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.Keys.ToArray());
                HWindNt.SetImaage(productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].GetHObject());
                foreach (var item in productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi)
                {
                    if (item.Value.IsInitialized())
                    {
                        HOperatorSet.AreaCenter(item.Value, out HalconDotNet.HTuple area, out HalconDotNet.HTuple rows, out HalconDotNet.HTuple clos);
                        if (area.Length != 0)
                        {
                            HWindNt.OneResIamge.AddImageMassage(rows, clos, item.Key);
                            HWindNt.OneResIamge.AddObj(item.Value);
                        }
                    }
                }
                HWindNt.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 导入整图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择图片文件可多选";
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].ImagePath = openFileDialog.FileName;
                productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].Cler();
                HWindNt.SetImaage(productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].GetHObject());
                HWindNt.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 删除导航图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (productEX.Key_Navigation_Picture.ContainsKey(listBox3.SelectedItem.ToString()))
                {
                    productEX.Key_Navigation_Picture.Remove(listBox3.SelectedItem.ToString());
                }
                listBox3.Items.Remove(listBox3.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string meassge = "重命名";
                string names = listBox3.SelectedItem.ToString();
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, names, 100, 100);
                if (productEX.Key_Navigation_Picture.ContainsKey(sd))
                {
                    meassge = "名称已存在";
                    goto st;
                }
                productEX.Key_Navigation_Picture.Add(sd, productEX.Key_Navigation_Picture[names]);
                listBox3.Items.Add(sd);
                listBox3.SelectedItem = sd;

                productEX.Key_Navigation_Picture.Remove(names);
                listBox3.Items.Remove(names);

                foreach (var item in productEX.ListDicData)
                {
                    if (item.RunNameOBJ.Contains('.'))
                    {
                        string[] datSd = item.RunNameOBJ.Split('.');
                        if (datSd[0] == names)
                        {
                            item.RunNameOBJ = sd + "." + datSd[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 添加导航图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string meassge = "创建导航图";
                string names = "导航图1";
                if (listBox3.Items.Count != 0)
                {
                    names = ProjectINI.GetStrReturnStr(listBox3.Items[listBox3.Items.Count - 1].ToString());
                }
            st:
                string sd = Interaction.InputBox("请输入新名称", meassge, names, 100, 100);

                if (sd == "")
                {
                    return;
                }
                if (productEX.Key_Navigation_Picture.ContainsKey(sd))
                {
                    meassge = "名称已存在";
                    goto st;
                }
                productEX.Key_Navigation_Picture.Add(sd, new ProductEX.Navigation_Picture());
                listBox3.Items.Add(sd);
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
                if (productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.ContainsKey(listBox2.SelectedItem.ToString()))
                {
                    productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.Remove(listBox2.SelectedItem.ToString());
                }
                listBox2.Items.Remove(listBox2.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 修改区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                HWindNt.HobjClear();

                if (productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.ContainsKey(listBox2.SelectedItem.ToString()))
                {
                    HTuple rows, cols, row2, cols2;
                    HObject hObject1 = productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi[listBox2.SelectedItem.ToString()];

                    if (hObject1.IsInitialized() && hObject1.CountObj() == 1)
                    {
                        HOperatorSet.SmallestRectangle1(hObject1, out rows, out cols, out row2, out cols2);
                        HOperatorSet.DrawRectangle1Mod(hWindowControl2.HalconWindow, rows, cols, row2, cols2,
                            out rows, out cols, out row2, out cols2);
                    }
                    else
                    {
                        HOperatorSet.DrawRectangle1(hWindowControl2.HalconWindow, out rows,
                           out cols, out row2, out cols2);
                    }

                    HalconDotNet.HOperatorSet.GenRectangle1(out HalconDotNet.HObject hObject, rows, cols, row2, cols2);
                    productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi[listBox2.SelectedItem.ToString()] = hObject;
                    HWindNt.OneResIamge.AddObj(hObject);
                    HalconDotNet.HOperatorSet.AreaCenter(hObject, out HalconDotNet.HTuple area, out rows, out HalconDotNet.HTuple clos);
                    HWindNt.OneResIamge.AddImageMassage(rows, clos, listBox2.SelectedItem.ToString());
                    HWindNt.ShowImage();
                }
                else
                {
                    MessageBox.Show(listBox2.SelectedItem.ToString() + "不存在");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 添加区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int Inde = 1;
                string meassge = "创建区域";
                string name = "区域" + Inde;
                string sd = "";
            st:
                while (true)
                {
                    if (productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.ContainsKey(name))
                    {
                        meassge = "名称已存在";
                        name = "区域" + Inde++;
                        goto st;
                    }
                    break;
                }
                sd = Interaction.InputBox("请输入新名称", meassge, name, 100, 100);
                if (sd == "")
                {
                    return;
                }
                while (true)
                {
                    if (productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.ContainsKey(sd))
                    {
                        meassge = "名称已存在";
                        name = "区域" + Inde++;
                        goto st;
                    }
                    break;
                }
                productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.Add(sd, new HalconDotNet.HObject());
                productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi[sd].GenEmptyObj();
                listBox2.Items.Add(sd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 重命名ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                string meassge = "重命名区域";
                string name = listBox2.SelectedItem.ToString();
                string sd = "";
            st:
                sd = Interaction.InputBox("请输入新名称", meassge, name, 100, 100);
                if (sd == "")
                {
                    return;
                }
                if (productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.ContainsKey(sd))
                {
                    meassge = "名称已存在";
                    goto st;
                }
                productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.Add(sd, productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi[name]);
                listBox2.Items.Remove(name);
                productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.Remove(name);
                listBox2.Items.Add(sd);
            }
            catch (Exception)
            {
            }
        }

        #endregion 导航图

        #region 全局点位

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                bool flag = e.ColumnIndex == 8;
                this.dataGridView4.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                if (flag)
                {
                    int de = e.RowIndex;
                    this.dataGridView4.Rows[de].DefaultCellStyle.BackColor = Color.Green;
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            Enum.TryParse<EnumXYZUMoveType>(this.dataGridView4.Rows[e.RowIndex].Cells[6].Value.ToString(), out EnumXYZUMoveType enumXYZUMoveType);
                            bool flag2 = DebugCompiler.Instance.DDAxis.SetXYZ1Points(this.dataGridView4.Rows[de].Cells[7].Value.ToString(), 15,
                            double.Parse(this.dataGridView4.Rows[de].Cells[1].Value.ToString()), double.Parse(this.dataGridView4.Rows[de].Cells[2].Value.ToString()),
                             Convert.ToDouble(this.dataGridView4.Rows[de].Cells[3].Value), Convert.ToDouble(this.dataGridView4.Rows[de].Cells[4].Value),
                             enumXYZUMoveType);
                            if (!flag2)
                            {
                                this.dataGridView4.Rows[de].DefaultCellStyle.BackColor = Color.Red;
                            }
                            Thread.Sleep(1000);
                            foreach (var item in Vision.GetHimageList().Keys)
                            {
                                if (this.dataGridView4.Rows[de].Cells[7].Value.ToString() == Vision.GetSaveImageInfo(item).AxisGrot)
                                {
                                    int det = int.Parse(this.dataGridView4.Rows[de].Cells[5].Value.ToString());
                                    if (det <= 0)
                                    {
                                        Vision.GetRunNameVision(item).HobjClear();
                                        if (Vision.GetRunNameVision(item).GetCam().GetImage(out HObject image))
                                        {
                                            Vision.GetRunNameVision(item).Image(image);
                                        }
                                        else
                                        {
                                            Vision.GetRunNameVision(item).Image().GenEmptyObj();
                                        }
                                        //Vision.GetRunNameVision(item).Image(Vision.GetRunNameVision(item).GetCam().GetImage());
                                    }
                                    else
                                    {
                                        Vision.GetRunNameVision(item).ReadCamImage(this.dataGridView4.Rows[de].Cells[5].Value.ToString(),
                                                int.Parse(this.dataGridView4.Rows[de].Cells[5].Value.ToString()));
                                    }
                                    Vision.GetRunNameVision(item).ShowObj();
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                    this.dataGridView4.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dataGridView4_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.isCot)
            {
                try
                {
                    DebugCompiler.Instance.DDAxis.XyzPoints[e.RowIndex].Name = this.dataGridView4.Rows[e.RowIndex].Cells[0].Value.ToString();
                    double eX;
                    if (double.TryParse(this.dataGridView4.Rows[e.RowIndex].Cells[1].Value.ToString(), out eX))
                    {
                        DebugCompiler.Instance.DDAxis.XyzPoints[e.RowIndex].X = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "X值错误");
                    }
                    if (double.TryParse(this.dataGridView4.Rows[e.RowIndex].Cells[2].Value.ToString(), out eX))
                    {
                        DebugCompiler.Instance.DDAxis.XyzPoints[e.RowIndex].Y = eX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "Y值错误");
                    }
                    if (this.dataGridView4.Rows[e.RowIndex].Cells[3].Value != null)
                    {
                        DebugCompiler.Instance.DDAxis.XyzPoints[e.RowIndex].Z = Convert.ToDouble(this.dataGridView4.Rows[e.RowIndex].Cells[3].Value);
                    }
                    if (this.dataGridView4.Rows[e.RowIndex].Cells[4].Value != null)
                    {
                        if (double.TryParse(this.dataGridView4.Rows[e.RowIndex].Cells[4].Value.ToString(), out eX))
                        {
                            DebugCompiler.Instance.DDAxis.XyzPoints[e.RowIndex].U = eX;
                        }
                        else
                        {
                            MessageBox.Show(e.RowIndex.ToString() + "U值错误");
                        }
                    }
                    int iX;
                    if (int.TryParse(this.dataGridView4.Rows[e.RowIndex].Cells[5].Value.ToString(), out iX))
                    {
                        DebugCompiler.Instance.DDAxis.XyzPoints[e.RowIndex].ID = iX;
                    }
                    else
                    {
                        MessageBox.Show(e.RowIndex.ToString() + "ID值错误");
                    }
                    Enum.TryParse<EnumXYZUMoveType>(this.dataGridView4.Rows[e.RowIndex].Cells[6].Value.ToString(), out EnumXYZUMoveType enumXYZUMoveType);
                    DebugCompiler.Instance.DDAxis.XyzPoints[e.RowIndex].isMove = enumXYZUMoveType;
                    if (this.dataGridView4.Rows[e.RowIndex].Cells[7].Value != null)
                    {
                        DebugCompiler.Instance.DDAxis.XyzPoints[e.RowIndex].AxisGrabName = this.dataGridView4.Rows[e.RowIndex].Cells[7].Value.ToString();
                    }
                    this.dataGridView4.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                catch (Exception)
                {
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                int de = dataGridView4.SelectedCells[0].RowIndex;
                Thread thread = new Thread(() =>
                {
                    Enum.TryParse<EnumXYZUMoveType>(this.dataGridView4.Rows[de].Cells[6].Value.ToString(), out EnumXYZUMoveType enumXYZUMoveType);
                    bool flag2 = DebugCompiler.Instance.DDAxis.SetXYZ1Points(this.dataGridView4.Rows[de].Cells[7].Value.ToString(), 15,
                      double.Parse(this.dataGridView4.Rows[de].Cells[1].Value.ToString()), double.Parse(this.dataGridView4.Rows[de].Cells[2].Value.ToString()),
                  Convert.ToDouble(this.dataGridView4.Rows[de].Cells[3].Value), Convert.ToDouble(this.dataGridView4.Rows[de].Cells[4].Value), enumXYZUMoveType);
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                int de = dataGridView4.SelectedCells[0].RowIndex;
                DebugCompiler.Instance.DDAxis.GetAxisGroupPoints(this.dataGridView4.Rows[de].Cells[7].Value.ToString(), out double? XpT, out double? Ypt, out double? zpt, out double? u);
                DebugCompiler.Instance.DDAxis.XyzPoints[de].Name = this.dataGridView4.Rows[de].Cells[0].Value.ToString();
                DebugCompiler.Instance.DDAxis.XyzPoints[de].X = XpT.Value;
                DebugCompiler.Instance.DDAxis.XyzPoints[de].Y = Ypt.Value;
                if (zpt != null)
                {
                    DebugCompiler.Instance.DDAxis.XyzPoints[de].Z = zpt.Value;
                }
                if (u != null)
                {
                    DebugCompiler.Instance.DDAxis.XyzPoints[de].U = u.Value;
                }
                this.dataGridView4.Rows[de].Cells[1].Value = XpT;
                this.dataGridView4.Rows[de].Cells[2].Value = Ypt;
                this.dataGridView4.Rows[de].Cells[3].Value = zpt;
                this.dataGridView4.Rows[de].Cells[4].Value = u;
            }
            catch (Exception ex)
            {
            }
            isCot = false;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                int de = 0;
                if (dataGridView4.SelectedCells.Count != 0)
                {
                    de = dataGridView4.SelectedCells[0].RowIndex + 1;
                }
                DebugCompiler.Instance.DDAxis.XyzPoints.Insert(de, new XYZPoint());
                dataGridView4.Rows.Insert(de, new DataGridViewRow());
                XYZPoint xYZPoint = DebugCompiler.Instance.DDAxis.XyzPoints[de];

                if (de != 0)
                {
                    ProjectINI.GetStrReturnInt(DebugCompiler.Instance.DDAxis.XyzPoints[de - 1].Name, out string names);

                    xYZPoint.Name = names + (ProjectINI.GetStrReturnInt(DebugCompiler.Instance.DDAxis.XyzPoints[de - 1].Name) + 1);
                }
                dataGridView4.Rows[de].Cells[0].Value = xYZPoint.Name;
                dataGridView4.Rows[de].Cells[1].Value = xYZPoint.X;
                dataGridView4.Rows[de].Cells[2].Value = xYZPoint.Y;
                dataGridView4.Rows[de].Cells[3].Value = xYZPoint.Z;
                dataGridView4.Rows[de].Cells[4].Value = xYZPoint.U;
                dataGridView4.Rows[de].Cells[5].Value = xYZPoint.ID;
                dataGridView4.Rows[de].Cells[6].Value = xYZPoint.isMove;
                xYZPoint.AxisGrabName = Column7.Items[0].ToString();
                dataGridView4.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            isCot = false;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                isCot = true;
                int de = dataGridView4.Rows.Add();
                XYZPoint xYZPoint = new XYZPoint();
                DebugCompiler.Instance.DDAxis.XyzPoints.Add(xYZPoint);

                xYZPoint.Name = "P" + de;
                dataGridView4.Rows[de].Cells[0].Value = xYZPoint.Name;
                dataGridView4.Rows[de].Cells[1].Value = xYZPoint.X;
                dataGridView4.Rows[de].Cells[2].Value = xYZPoint.Y;
                dataGridView4.Rows[de].Cells[3].Value = xYZPoint.Z;
                dataGridView4.Rows[de].Cells[4].Value = xYZPoint.U;
                dataGridView4.Rows[de].Cells[5].Value = xYZPoint.ID;
                dataGridView4.Rows[de].Cells[6].Value = xYZPoint.isMove;
                xYZPoint.AxisGrabName = Column7.Items[0].ToString();
                dataGridView4.Rows[de].Cells[7].Value = xYZPoint.AxisGrabName;
            }
            catch (Exception)
            {
            }
            isCot = false;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                int det = dataGridView4.SelectedRows.Count;
                if (MessageBox.Show("删除点", "是否删除点位" + det, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int i = 0; i < det; i++)
                    {
                        DebugCompiler.Instance.DDAxis.XyzPoints.RemoveAt(dataGridView4.SelectedCells[0].RowIndex);
                        dataGridView4.Rows.RemoveAt(dataGridView4.SelectedCells[0].RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion 全局点位

        private void dataGridView4_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        #region 产品创建方式

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (isCot)
            {
                return;
            }
            try
            {
            }
            catch (Exception)
            {
            }
        }

        #endregion 产品创建方式

        private void toolStripSplitButton1_ButtonClick_1(object sender, EventArgs e)
        {
        }

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void 导入ExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = ProjectINI.ProjectPathRun ;
            try
            {//"文本文件|*txt.*|C#文件|*.cs|所有文件|*.*";
                openFileDialog.Filter = "Excel文件|*.xls;*.xlsx;*.txt;";
                DialogResult dialog = openFileDialog.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    if (System.IO.Path.GetExtension(openFileDialog.FileName) == ".txt")
                    {
                        Npoi.ReadText(openFileDialog.FileName, out List<string> text);
                        //dataGridView1.Rows.Clear();

                        foreach (var item in text)
                        {
                            string[] ItemArray = System.Text.RegularExpressions.Regex.Split(item, @"\s+");
                        }
                    }
                    else
                    {
                        DataTable dataTable2 = Npoi.ReadExcelFile(openFileDialog.FileName, 0);
                        if (dataTable2 == null)
                        {
                            MessageBox.Show("参数信息不存在;" + Environment.NewLine);
                        }
                        else
                        {
                            foreach (DataRow item1 in dataTable2.Rows)
                            {
                                HOperatorSet.GenRectangle1(out HObject hObject, double.Parse(item1.ItemArray[1].ToString()) - 50, double.Parse(item1.ItemArray[2].ToString()) - 50,
                               double.Parse(item1.ItemArray[1].ToString()) + 50, double.Parse(item1.ItemArray[2].ToString()) + 50);
                                if (!productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.ContainsKey(item1.ItemArray[0].ToString()))
                                {
                                    productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi.Add(item1.ItemArray[0].ToString(), hObject);
                                    listBox2.Items.Add(item1.ItemArray[0]);
                                }
                                else
                                {
                                    productEX.Key_Navigation_Picture[listBox3.SelectedItem.ToString()].KeyRoi[item1.ItemArray[0].ToString()] = hObject;
                                }
                            }
                            MessageBox.Show("导入成功");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败:" + ex.Message);
            }
        }

        private void toolStripDropDownButton3_Click(object sender, EventArgs e)
        {
        }

        private DrawBackSt drawBackSt;

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    return;
                }
                isCot = true;
                drawBackSt = Vision.Instance.DicDrawbackNameS[listBox1.SelectedItem.ToString()];
                dataGridView6.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isCot = false;
        }

        private void dataGridView6_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isCot)
                {
                    return;
                }
                if (drawBackSt != null)
                {
                    //for (int i = 0; i < dataGridView6.Rows.Count; i++)
                    //{
                    //    if (dataGridView6.Rows[i].Cells[0].Value != null)
                    //    {
                    //        if (drawBackSt.DicDrawbackIndex.Count <= i)
                    //        {
                    //            drawBackSt.DicDrawbackIndex.Add(int.Parse(dataGridView6.Rows[i].Cells[0].Value.ToString()));
                    //        }
                    //        else
                    //        {
                    //            drawBackSt.DicDrawbackIndex[i] = int.Parse(dataGridView6.Rows[i].Cells[0].Value.ToString());
                    //        }
                    //    }
                    //    if (dataGridView6.Rows[i].Cells[1].Value != null)
                    //    {
                    //        if (drawBackSt.DicDrawbackName.Count <= i)
                    //        {
                    //            drawBackSt.DicDrawbackName.Add(dataGridView6.Rows[i].Cells[1].Value.ToString());
                    //        }
                    //        else
                    //        {
                    //            drawBackSt.DicDrawbackName[i] = dataGridView6.Rows[i].Cells[1].Value.ToString();
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string name = Interaction.InputBox("请输入新名称", "缺陷类型", "缺陷1", 100, 100);
                if (name != "")
                {
                    if (!Vision.Instance.DicDrawbackNameS.ContainsKey(name))
                    {
                        Vision.Instance.DicDrawbackNameS.Add(name, new DrawBackSt());
                        listBox5.Items.Add(name);
                    }
                    else
                    {
                        MessageBox.Show("已存在" + name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            try
            {
                if (Vision.Instance.DicDrawbackNameS.ContainsKey(listBox5.SelectedItem.ToString()))
                {
                    Vision.Instance.DicDrawbackNameS.Remove(listBox5.SelectedItem.ToString());
                    listBox5.Items.RemoveAt(listBox5.SelectedIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 导出缺陷类型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ErosProjcetDLL.Project.ProjectINI.Weait(Vision.Instance.DicDrawbackNameS);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 导入缺陷类型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, DrawBackSt> keyValuePairs = new Dictionary<string, DrawBackSt>();

                ErosProjcetDLL.Project.ProjectINI.ReadWeait(out keyValuePairs);
                if (keyValuePairs != null)
                {
                    foreach (var item in keyValuePairs)
                    {
                        if (!Vision.Instance.DicDrawbackNameS.ContainsKey(item.Key))
                        {
                            Vision.Instance.DicDrawbackNameS.Add(item.Key, item.Value);
                        }
                        else
                        {
                            Vision.Instance.DicDrawbackNameS[item.Key] = item.Value;
                        }
                    }
                    listBox5.Items.Clear();
                    foreach (var item in Vision.Instance.DicDrawbackNameS)
                    {
                        listBox5.Items.Add(item.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView7_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isCot)
                {
                    return;
                }
                if (dataGridView7.Rows[e.RowIndex].Cells[1].Value == null)
                {
                    return;
                }
                DrawBack drawBack = dataGridView7.Rows[e.RowIndex].Cells[0].Tag as DrawBack;

                int dw = 0;
                string key = dataGridView7.Rows[e.RowIndex].Cells[1].Value.ToString();
                if (drawBack != null)
                {
                    if (drawBack.DrawbackName != key)
                    {
                        Vision.Instance.DefectTypeDicEx.Remove(drawBack.DrawbackName);
                    }
                }
                string values = "";
                if (dataGridView7.Rows[e.RowIndex].Cells[1].Value != null)
                {
                    values = dataGridView7.Rows[e.RowIndex].Cells[2].Value.ToString();
                }
                if (dataGridView7.Rows[e.RowIndex].Cells[0].Value != null)
                {
                    int.TryParse(dataGridView7.Rows[e.RowIndex].Cells[0].Value.ToString(), out dw);
                }
                if (Vision.Instance.DefectTypeDicEx.ContainsKey(key))
                {
                    Vision.Instance.DefectTypeDicEx[key].DrawbackEnglish = values;
                    Vision.Instance.DefectTypeDicEx[key].DrawbackIndex = dw;
                    Vision.Instance.DefectTypeDicEx[key].DrawbackName = key;
                }
                else
                {
                    Vision.Instance.DefectTypeDicEx.Add(key, new DrawBack()
                    {
                        DrawbackEnglish = values,
                        DrawbackName = key,
                        DrawbackIndex = dw
                    });
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 导入ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = ProjectINI.ProjectPathRun ;
            try
            {//"文本文件|*txt.*|C#文件|*.cs|所有文件|*.*";
                openFileDialog.Filter = "Excel文件|*.xls;*.xlsx;*.txt;";
                DialogResult dialog = openFileDialog.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    if (System.IO.Path.GetExtension(openFileDialog.FileName) == ".txt")
                    {
                        Npoi.ReadText(openFileDialog.FileName, out List<string> text);
                        //dataGridView1.Rows.Clear();

                        foreach (var item in text)
                        {
                            string[] ItemArray = System.Text.RegularExpressions.Regex.Split(item, @"\s+");
                        }
                    }
                    else
                    {
                        DataTable dataTable2 = Npoi.ReadExcelFile(openFileDialog.FileName, 0);
                        if (dataTable2 == null)
                        {
                            MessageBox.Show("参数信息不存在;" + Environment.NewLine);
                        }
                        else
                        {
                            dataGridView7.Rows.Clear();
                            Vision.Instance.DefectTypeDicEx.Clear();
                            foreach (DataRow item1 in dataTable2.Rows)
                            {
                                if (!Vision.Instance.DefectTypeDicEx.ContainsKey(item1.ItemArray[1].ToString()))
                                {
                                    Vision.Instance.DefectTypeDicEx.Add(item1.ItemArray[1].ToString(),
                                        new DrawBack()
                                        {
                                            DrawbackName = item1.ItemArray[1].ToString(),
                                            DrawbackEnglish = item1.ItemArray[2].ToString(),
                                        });

                                    int det = dataGridView7.Rows.Add();
                                    int.TryParse(item1.ItemArray[0].ToString(),
                                        out Vision.Instance.DefectTypeDicEx[item1.ItemArray[1].ToString()].
                                    DrawbackIndex);
                                    dataGridView7.Rows[det].Cells[0].Value = item1.ItemArray[0];
                                    dataGridView7.Rows[det].Cells[1].Value = item1.ItemArray[1];
                                    dataGridView7.Rows[det].Cells[2].Value = item1.ItemArray[2];
                                }
                                else
                                {
                                    //Vision.Instance.CRDName.Add(item1.ItemArray[0].ToString(), item1.ItemArray[0].ToString());
                                }
                            }
                            MessageBox.Show("导入成功");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败:" + ex.Message);
            }
        }

        private void 导出ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void 删除ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                Vision.Instance.DefectTypeDicEx.Remove(dataGridView7.Rows[dataGridView7.SelectedCells[0].RowIndex].Cells[1].Value.ToString());
                dataGridView7.Rows.RemoveAt(dataGridView7.SelectedCells[0].RowIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}