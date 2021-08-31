using ErosSocket.DebugPLC.Robot;
using ErosSocket.ErosConLink;
using HalconDotNet;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Vision2.ConClass;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project.DebugF;
using Vision2.Project.Mes;
using Vision2.vision;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.Project.formula
{
    public partial class UserFormulaContrsl : UserControl
    {
        public UserFormulaContrsl()
        {
            InitializeComponent();
            This = this;
        }

        public static UserFormulaContrsl This;

        private bool di;

        private void ThreadUP()
        {
            try
            {
                if (DebugCompiler.GetLinkNmae(RecipeCompiler.Instance.GetTrageNmae) == "1"
              || DebugCompiler.GetLinkNmae(RecipeCompiler.Instance.GetTrageNmae) == true.ToString())
                {
                    if (checkBox1.Checked)
                    {
                        StaticCon.SetLingkValue(RecipeCompiler.Instance.SetTrageDoneNmae, 1, out string err);
                    }
                    else
                    {
                        StaticCon.GetSocketClint(RecipeCompiler.Instance.GetQRLinkNmae).Send(RecipeCompiler.Instance.QRTrageText);
                    }
                }
                StaticCon.GetLingkIDValue(DebugCompiler.Instance.LinkAutoMode, UClass.Boolean, out dynamic dynamic);

                if (dynamic == null)
                {
                    label5.BackColor = Color.LightSlateGray;
                    label5.Visible = false;
                }
                else
                {
                    if (dynamic != di)
                    {
                        label5.Visible = true;
                        di = dynamic;
                        if (di)
                        {
                            label5.BackColor = Color.Green;
                        }
                        else
                        {
                            label5.BackColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public HWindID HWind;

        private void UserFormulaContrsl_Load(object sender, EventArgs e)
        {
            try
            {
                HWind = new HWindID();
                HWind.Initialize(hWindowControl1);
                hWindowControl1.MouseClick += HWindowControl1_MouseClick;
                MainForm1.MainFormF.CycleEven += ThreadUP;
                chartType.Series.Clear();
                chartType.ChartAreas.Clear();
                Series Series1 = new Series();
                chartType.Series.Add(Series1);
                chartType.Series["Series1"].ChartType = SeriesChartType.Column;
                chartType.Legends[0].Enabled = false;
                chartType.Series["Series1"].LegendText = "";
                chartType.Series["Series1"].Label = "#VALY";
                chartType.Series["Series1"].ToolTip = "#VALX";
                chartType.Series["Series1"]["PointWidth"] = "0.5";
                ChartArea ChartArea1 = new ChartArea();
                chartType.ChartAreas.Add(ChartArea1);
                //开启三维模式的原因是为了避免标签重叠
                chartType.ChartAreas["ChartArea1"].AxisY.Interval = 50;
                chartType.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;//开启三维模式;PointDepth:厚度BorderWidth:边框宽
                chartType.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 20;//起始角度
                chartType.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 15;//倾斜度(0～90)
                chartType.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.Realistic;//表面光泽度
                chartType.ChartAreas["ChartArea1"].AxisX.Interval = 1; //决定x轴显示文本的间隔，1为强制每个柱状体都显示，3则间隔3个显示
                chartType.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("宋体", 9, FontStyle.Regular);
                chartType.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                chartType.Series[0].XValueMember = "name";
                chartType.Series[0].YValueMembers = "sumcount";
                //ChartArea1.AxisX.w
                ChartArea1.AxisX.Minimum = 0;
                ChartArea1.AxisX.Maximum = 24;
                ChartArea1.AxisY.Minimum = 0d;
                //     int x = 0;
                //RecipeCompiler.OKNumberClass[] oKNumberClass = RecipeCompiler.Instance.GetOKNumberList();
                //List<int> vs = new List<int>();
                //for (int i = 0; i < oKNumberClass.Length; i++)
                //{
                //    vs.Add(oKNumberClass[i].Number);
                //}
                //foreach (int v in vs)
                //{
                //    chartType.Series["Series1"].Points.AddXY(x, v);
                //    x++;
                //}
            }
            catch (Exception ex)
            {
            }
        }

        private void HWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            try
            {
                if (DebugCompiler.EquipmentStatus == EnumEquipmentStatus.初始化中)
                {
                    return;
                }
                if (DebugCompiler.EquipmentStatus == EnumEquipmentStatus.运行中)
                {
                    return;
                }
                if (MatrixC == null)
                {
                    return;
                }
                HTuple rRow1 = MatrixC.mark1Row;
                HTuple rRow2 = MatrixC.mark2Row;
                HTuple rCol1 = MatrixC.mark1Col;
                HTuple rCol2 = MatrixC.mark2Col;
                if (rRow2 < rRow1)
                {
                    rRow1 = MatrixC.mark2Row;
                    rRow2 = MatrixC.mark1Row;
                }
                if (rCol2 < rCol1)
                {
                    rCol2 = MatrixC.mark1Col;
                    rCol1 = MatrixC.mark2Col;
                }
                HOperatorSet.GenRectangle1(out HObject hObject, rRow1, rCol1, rRow2, rCol2);

                HOperatorSet.GetRegionIndex(MatrixC.GetHObject(), (int)e.Y, (int)e.X, out HTuple intec);
                if (intec.Length == 1)
                {
                    string axisName = Vision.GetSaveImageInfo(MatrixC.VisionName).AxisGrot;
                    Vision.GetRunNameVision(MatrixC.VisionName).GetCalib().GetPointRctoXY(e.Y, e.X, out HTuple ys, out HTuple xs);
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            if (DebugCompiler.Instance.DDAxis.SetXYZ1Points(axisName, 10, xs, ys, null))
                            {
                                Thread.Sleep(DebugCompiler.Instance.MarkWait);
                                Vision.GetRunNameVision(MatrixC.VisionName).AsysReadCamImage(1, 1, asyncRestImage =>
                                {
                                    try
                                    {
                                        HOperatorSet.GenRectangle2(out HObject hObject2, e.Y, e.X, 0, 2000, 2000);
                                        HWind.OneResIamge.AddObj(hObject2);
                                        HWind.ShowImage();
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                });
                                Thread.Sleep(DebugCompiler.Instance.CamWait);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void HWindowControl1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (DebugCompiler.EquipmentStatus == EnumEquipmentStatus.已停止)
                {
                    HOperatorSet.GetRegionIndex(MatrixC.GetHObject(), e.X, e.Y, out HTuple intec);
                    if (intec)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private 捷普.MForm mForm;
        private DebugF.IO.TrayDatas trayDataUserControl;

        public void Up()
        {
            try
            {
                isUP = true;
                if (DebugCompiler.Instance.DeviceName == "捷普测量1.0")
                {
                    if (mForm == null || mForm.IsDisposed)
                    {
                        mForm = new 捷普.MForm();
                    }
                    mForm.Show();
                    mForm.Focus();
                }
                string selesname = "";
                if (comboBox1.SelectedItem != null)
                {
                    selesname = comboBox1.SelectedItem.ToString();
                }
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(Product.GetThisP().Keys.ToArray());
                if (selesname == "")
                {
                    comboBox1.SelectedItem = Product.ProductionName;
                }
                else
                {
                    comboBox1.SelectedItem = selesname;
                }
                isUP = false;
            }
            catch (Exception ex)
            {
            }
        }

        private bool isUP;
        private DebugF.工艺库.MatrixC MatrixC;

        /// <summary>
        /// 刷新目标参数
        /// </summary>
        public void UPSetGetPargam()
        {
            try
            {
                Up();

                isUP = true;

                try
                {
                    lightSourceControl1.GetData(Vision.Instance.GetLightSources()[0]);
                    //ProductEX
                }
                catch (Exception)
                {
                }
                AlarmForm.UpDa(DebugCompiler.Instance.ErrTextS);
                if (DebugCompiler.Instance.LinkSeelpTyoe < 3)
                {
                    toolStripComboBox1.SelectedIndex = DebugCompiler.Instance.LinkSeelpTyoe;
                }
                if (!RecipeCompiler.Instance.ProductEX.ContainsKey(Product.ProductionName))
                {
                    RecipeCompiler.Instance.ProductEX.Add(Product.ProductionName, new ProductEX());
                }
                RecipeCompiler.Instance.Data.SetData(RecipeCompiler.GetProductEX().ListDicData);

                try
                {
                    runUControl1.Visible = DebugCompiler.Instance.IsCtr;
                    this.HWind.OneResIamge.ClearAllObj();
                    if (DebugCompiler.Instance.ListMatrix.Count > 0)
                    {
                        MatrixC = DebugCompiler.Instance.ListMatrix[0];
                        foreach (var item in RecipeCompiler.GetProductEX().Key_Navigation_Picture)
                        {
                            HWind.OneResIamge.Image = RecipeCompiler.GetProductEX().Key_Navigation_Picture[item.Key].GetHObject();
                            break;
                        }
                        if (MatrixC != null)
                        {
                            MatrixC.SetHwindId(HWind);
                            MatrixC.Calculate();
                        }
                        if (RecipeCompiler.GetProductEX().Key_Navigation_Picture.Count != 0)
                        {
                            HWind.ImageRowStrat = 0;
                            HWind.ImageColStrat = 0;
                            HWind.SetImaage(HWind.OneResIamge.Image);
                        }
                        else
                        {
                            if (MatrixC != null)
                            {
                                MatrixC.SetHwindId(HWind);
                                MatrixC.Calculate();
                                HWind.HeigthImage = MatrixC.ImageHeith;
                                HWind.WidthImage = MatrixC.ImageWidth;
                                try
                                {
                                    MatrixC.FillIamge();
                                }
                                catch (Exception ex)
                                {
                                    AlarmText.AddTextNewLine("导航图加载错误:未创建" + ex.Message);
                                }

                                HWind.SetImaage(HWind.OneResIamge.Image);
                            }
                        }
                    }
                    HWind.ShowImage();
                }
                catch (Exception ex)
                {
                    AlarmText.AddTextNewLine("导航图加载错误:" + ex.Message);
                }
                if (trayDataUserControl != null)
                {
                    trayDataUserControl.Visible = false;
                }
                dataGridView1.Visible = false;
                label3.Visible = false;

                label2.BackColor = Color.Yellow;
                label2.Text = "...";
                label4.Visible = label6.Visible = RecipeCompiler.Instance.IsQRCdoe;
                button3.Visible = false;
                tabPage1.Controls.Clear();
                if (!DebugCompiler.Instance.IsSet)
                {
                    tabControl1.TabPages.Remove(tabPage2);
                }
                else if (!tabControl1.TabPages.Contains(tabPage2))
                {
                    tabControl1.TabPages.Add(tabPage2);
                }
                if (!DebugCompiler.Instance.Display_Status)
                {
                    tabControl1.TabPages.Remove(tabPage3);
                }
                else if (!tabControl1.TabPages.Contains(tabPage3))
                {
                    tabControl1.TabPages.Add(tabPage3);
                }
                switch (RecipeCompiler.Instance.UpDataType)
                {
                    case RecipeCompiler.EnumUpDataType.表格:
                        dataGridView1.Visible = true;
                        label3.Visible = true;

                        button3.Visible = true;
                        DataGuiVievModeUI butte = new DataGuiVievModeUI();
                        butte.Dock = DockStyle.Fill;
                        tabPage1.Controls.Add(butte);
                        //tabControl1.Controls.Add(tabPage1);
                        break;

                    case RecipeCompiler.EnumUpDataType.托盘:
                        if (This.trayDataUserControl == null)
                        {
                            This.trayDataUserControl = new DebugF.IO.TrayDatas();
                        }
                        trayDataUserControl.Load += TrayDataUserControl_Load;
                        label3.Visible = true;
                        if (!tabPage1.Controls.Contains(trayDataUserControl))
                        {
                            tabPage1.Controls.Add(trayDataUserControl);
                        }
                        trayDataUserControl.BringToFront();
                        trayDataUserControl.Dock = DockStyle.Fill;
                        trayDataUserControl.Visible = true;
                        if (RecipeCompiler.Instance.TrayCont >= 0)
                        {
                            trayDataUserControl.Initialize(DebugCompiler.GetTray(RecipeCompiler.Instance.TrayCont).GetTrayData());
                            DebugCompiler.GetTray(RecipeCompiler.Instance.TrayCont).AddTary(trayDataUserControl);
                        }
                        break;

                    case RecipeCompiler.EnumUpDataType.复判按钮:
                        label3.Visible = true;
                        ButtenModeUI buttenModeUI;
                        if (!tabPage1.Controls.ContainsKey("buttenModeUI"))
                        {
                            buttenModeUI = new ButtenModeUI();
                            buttenModeUI.Dock = DockStyle.Fill;
                            tabPage1.Controls.Add(buttenModeUI);
                        }
                        else
                        {
                            buttenModeUI = tabPage1.Controls.Find("buttenModeUI", false)[0] as ButtenModeUI;
                        }

                        TrayData Traydata = DebugCompiler.GetTray(RecipeCompiler.Instance.TrayCont).GetTrayData();
                        DebugCompiler.GetTray(RecipeCompiler.Instance.TrayCont).AddTary(buttenModeUI);
                        buttenModeUI.Initialize(Traydata);
                        break;

                    case RecipeCompiler.EnumUpDataType.弹出复判按钮:
                        label3.Visible = true;

                        label3.Visible = true;

                        tabControl1.TabPages.Remove(tabPage1);
                        //ButtenModeUI buttenMode = new ButtenModeUI();
                        //buttenMode.Dock = DockStyle.Fill;
                        //tabPage1.Controls.Add(buttenMode);
                        break;

                    case RecipeCompiler.EnumUpDataType.不显示:
                        tabControl1.TabPages.Remove(tabPage1);
                        checkBox1.Visible = false;
                        dataGridView1.Visible = false;
                        label3.Visible = true;

                        button3.Visible = false;
                        button5.Visible = false;
                        break;

                    default:
                        break;
                }
                if (DebugCompiler.Instance.LinklamplightName == "")
                {
                    button5.Visible = false;
                }
                else
                {
                    button5.Visible = true;
                }
                groupBox4.Visible = DebugCompiler.Instance.UserIDText;
                groupBox5.Visible = DebugCompiler.Instance.Work_Order;

                toolStripLabel1.Visible = button1.Visible = comboBox1.Visible = DebugCompiler.Instance.IsPoName;
                if (DebugCompiler.Instance.PuPragrm != "")
                {
                    groupBox6.Visible = true;
                    dataGridView2.Rows.Clear();
                    string[] data = DebugCompiler.Instance.PuPragrm.Split(',');

                    for (int i = 0; i < data.Length; i++)
                    {
                        int indext = dataGridView2.Rows.Add();
                        dataGridView2.Rows[indext].Cells[0].Value = data[i];
                        if (Product.GetProd().ContainsKey(data[i]))
                        {
                            dataGridView2.Rows[indext].Cells[1].Value = Product.GetProd()[data[i]];
                        }
                    }
                    groupBox6.Height = (data.Length) * dataGridView2.Rows[0].Height + dataGridView2.ColumnHeadersHeight + 30;
                }
                else
                {
                    groupBox6.Visible = false;
                }
                if (DebugCompiler.Instance.UserIDText || DebugCompiler.Instance.Work_Order)
                {
                    panel2.Visible = true;
                }
                else
                {
                    panel2.Visible = false;
                }
                groupBox2.Visible = RecipeCompiler.Instance.PalenIDVsible;

                if (Vision.Instance.OffCont == 0)
                {
                    lightSourceControl1.Visible = false;
                }
                else if (DebugCompiler.Instance.isVisibleLightS)
                {
                    lightSourceControl1.Visible = true;
                }
                else
                {
                    lightSourceControl1.Visible = false;
                }

                bool visible = false;
                foreach (Control item in groupBox3.Controls)
                {
                    if (item.Visible)
                    {
                        groupBox3.Visible = true;
                        visible = true;
                    }
                }
                groupBox3.Visible = visible;

                if (RecipeCompiler.Instance.GetMes() != null)
                {
                    panel4.Visible = true;
                    comboBox2.Items.Clear();
                    comboBox3.Items.Clear();

                    if (!RecipeCompiler.Instance.GetMes().FixtureList.Contains(RecipeCompiler.Instance.GetMes().Fixture_ID))
                    {
                        RecipeCompiler.Instance.GetMes().FixtureList.Add(RecipeCompiler.Instance.GetMes().Fixture_ID);
                    }
                    if (RecipeCompiler.Instance.GetMes().FixtureList.Count != 0)
                    {
                        comboBox3.Items.AddRange(RecipeCompiler.Instance.GetMes().FixtureList.ToArray());
                    }
                    if (!RecipeCompiler.Instance.GetMes().ListStr.Contains(RecipeCompiler.Instance.GetMes().Line_Name))
                    {
                        RecipeCompiler.Instance.GetMes().ListStr.Add(RecipeCompiler.Instance.GetMes().Line_Name);
                    }
                    if (RecipeCompiler.Instance.GetMes().ListStr.Count != 0)
                    {
                        comboBox2.Items.AddRange(RecipeCompiler.Instance.GetMes().ListStr.ToArray());
                    }
                    comboBox2.SelectedItem = RecipeCompiler.Instance.GetMes().Line_Name;
                    comboBox3.SelectedItem = RecipeCompiler.Instance.GetMes().Fixture_ID;
                }
                else
                {
                    panel4.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace + ex.Message);
            }

            isUP = false;
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isUP)
                {
                    return;
                }
                if (Product.SetParameData(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString(), dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString()))
                {
                    dataGridView2[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Green;
                }
                else
                {
                    dataGridView2[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
                }
            }
            catch (Exception)
            {
            }
        }

        private void TrayDataUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (RecipeCompiler.Instance.TrayCont >= 0)
                {
                    trayDataUserControl.Initialize(DebugCompiler.GetTray(RecipeCompiler.Instance.TrayCont).GetTrayData());
                    DebugCompiler.GetTray(RecipeCompiler.Instance.TrayCont).AddTary(trayDataUserControl);
                    //trayDataUserControl.SetTray(DebugCompiler.GetTray(RecipeCompiler.Instance.TrayCont).GetTrayData());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载托盘错误:" + ex.Message);
            }
        }

        /// <summary>
        /// 使能加载项
        /// </summary>
        /// <param name="enable"></param>
        public void EnabledLog(bool enable)
        {
            comboBox1.Enabled = button1.Enabled = enable;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
        }

        private SynchronizationForm synchronizationForm;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MAXt = 0;
                if (DebugCompiler.EquipmentStatus == EnumEquipmentStatus.运行中)
                {
                    MessageBox.Show("生产中，无权限切换产品");
                }
                if (DebugCompiler.EquipmentStatus == EnumEquipmentStatus.初始化中)
                {
                    MessageBox.Show("生产中，无权限切换产品");
                }
                if (Product.IsSwitchover)
                {
                    MessageBox.Show("生产中，无权限切换产品");
                    return;
                }
                if (Product.GetProd(comboBox1.SelectedItem.ToString()) == null || Product.GetListLinkNames.Count == 0)
                {
                    if (Product.ProductionName != comboBox1.SelectedItem.ToString())
                    {
                        if (RecipeCompiler.Instance.ProductEX.ContainsKey(Product.ProductionName))
                        {
                            foreach (var item in RecipeCompiler.GetProductEX().Key_Navigation_Picture)
                            {
                                RecipeCompiler.GetProductEX().Key_Navigation_Picture[item.Key].Cler();
                            }
                            if (DebugCompiler.Instance.IsCtr)
                            {
                                if (DebugCompiler.Instance.DDAxis.AlwaysIODot.Value || DebugCompiler.Instance.DDAxis.AlwaysIOInt.Value
                                    || DebugCompiler.Instance.DDAxis.AlwaysIOOut.Value)
                                {
                                    MessageBox.Show("设备中存在产品,请取出产品");
                                    return;
                                }
                                MessageBox.Show("请确认设备中是否存在遗留产品？");
                            }
                        }
                        if (Vision.Instance.ISPName)
                        {
                            Vision.UpReadThis(comboBox1.SelectedItem.ToString());
                        }
                    }
                    Product.ProductionName = comboBox1.SelectedItem.ToString();
                    toolStripLabel1.Text = "当前生产:" + Product.ProductionName;
                    UPSetGetPargam();
                    return;
                }
                if (synchronizationForm == null || !synchronizationForm.IsDisposed)
                {
                    synchronizationForm = new SynchronizationForm();
                }
                synchronizationForm.Updatas(comboBox1.SelectedItem.ToString());
                synchronizationForm.ShowDialogSet(Product.GetListLinkNames.ToArray(), comboBox1.SelectedItem.ToString());
                UPSetGetPargam();
                toolStripLabel1.Text = "当前生产:" + Product.ProductionName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Up();
        }

        public static bool NG { get { return data.OK; } }

        /// <summary>
        /// 机判结果
        /// </summary>
        public List<string> ListReslutOK = new List<string>();

        /// <summary>
        /// 复判结果
        /// </summary>
        public List<string> ListReslutOKTR = new List<string>();

        /// <summary>
        /// 机器结果
        /// </summary>
        public List<string> ListReslutMestTR = new List<string>();

        private Dictionary<string, DataReseltBase> ReseltBase = new Dictionary<string, DataReseltBase>();

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (trayDataUserControl != null)
                {
                    trayDataUserControl.RestValue(DebugCompiler.GetTray(RecipeCompiler.Instance.TrayCont).GetTrayData());
                }
                if (checkBox1.Checked || dataGridView1.Rows.Count > 0 && ProcessControl.ProcessUser.QRCode != "")
                {
                    WeirtAll();
                }
                else if (ProcessControl.ProcessUser.QRCode == "")
                {
                    MessageBox.Show("未输入条码");
                }
                else
                {
                    MessageBox.Show("无复判数据可提交");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static DateTime timeStrStrat;

        public static void ClearData()
        {
            try
            {
                if (This.trayDataUserControl != null)
                {
                    //This.trayDataUserControl.RestValue();
                    //DebugCompiler.GetTrayDataUserControl().RestValue();
                }
                if (DebugCompiler.Instance.DDAxis != null)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        if (DebugCompiler.Instance.DDAxis.GetTrayInxt(i) != null)
                        {
                            DebugCompiler.Instance.DDAxis.GetTrayInxt(i).GetTrayData().RestValue();
                        }
                    }
                }
                Vision.Instance.HObjCler();
                dataVales.Clear(); ;
                UserFormulaContrsl.SetOK(0);
                This.ListReslutMestTR.Clear();
                This.ListOkNumber = new List<bool>();
                This.label4.Text = "产品码:";
                This.dataGridView1.Rows.Clear();
                int dt = This.dataGridView1.Columns.Count;
                for (int i = 2; i < dt; i++)
                {
                    This.dataGridView1.Columns.RemoveAt(2);
                }
                This.ListReslutOK.Clear();
                This.ListReslutOKTR.Clear();
                ProcessControl.ProcessUser.QRCode = "";

                RecipeCompiler.Instance.Data.Clear();
            }
            catch (Exception EX)
            {
                AlarmText.LogErr(EX.Message, "写入数据");
            }
        }

        public static void WriteMes(bool OKWe = true)
        {
            try
            {
                List<string> ListText = new List<string>();
                string Code = "";
                Code = ProcessControl.ProcessUser.QRCode;
                if (ProcessControl.ProcessUser.QRCode == "")
                {
                    Code = "无码";
                }
                if (ProcessControl.ProcessUser.QRCode == null)
                {
                    AlarmText.LogWarning("Mes", "无码");
                }
                if (RecipeCompiler.Instance.GetMes() != null)
                {
                    RecipeCompiler.Instance.GetMes().WrietMesAll(data, ProcessControl.ProcessUser.QRCode, Product.ProductionName);
                }
                if (RecipeCompiler.Instance.MesType != "")
                {
                    //This.label4.Text = "产品码:";
                    //ProcessControl.ProcessUser.QRCode = "";
                }
            }
            catch (Exception ex)
            {
                AlarmText.LogErr(ex.Message, "写入数据");
            }

            This.ListReslutOK.Clear();
            This.ListReslutOKTR.Clear();
            This.dataGridView1.Rows.Clear();
            int dt = This.dataGridView1.Columns.Count;
            for (int i = 2; i < dt; i++)
            {
                This.dataGridView1.Columns.RemoveAt(2);
            }
        }

        public static void WeirtAll(object datas = null)
        {
            try
            {
                if (datas == null)
                {
                    datas = DebugF.IO.TrayDataUserControl.GetTray();
                }
                if (RecipeCompiler.Instance.GetMes() != null)
                {
                    RecipeCompiler.Instance.GetMes().WrietMesAll(datas, ProcessControl.ProcessUser.QRCode, Product.ProductionName);
                }
                This.label4.Text = "产品码:";
                This.dataGridView1.Rows.Clear();
                This.ListReslutOKTR.Clear();
                This.ListReslutOK.Clear();
            }
            catch (Exception ex)
            {
                AlarmText.LogErr(ex.Message, "Mes写入数据");
            }
        }

        public List<bool> ListOkNumber = new List<bool>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="oKNumber"></param>
        public void AddData(RecipeCompiler.OKNumberClass oKNumber)
        {
            RecipeCompiler.GetSPC();
            ISOk = oKNumber.IsOK;
            if (oKNumber.IsOK)
            {
                label2.BackColor = Color.Green;
                label2.Text = "OK";
            }
            else
            {
                label2.BackColor = Color.Red;
                label2.Text = "NG";
            }
            //string data = RecipeCompiler.Instance.OKNumber.OKNumber.ToString() + "," + RecipeCompiler.Instance.OKNumber.NGNumber.ToString() +
            //    "," + RecipeCompiler.Instance.OKNumber.IsOK + "," + RecipeCompiler.Instance.OKNumber.Number + "," + RecipeCompiler.Instance.OKNumber.OKNG
            //    + "," + RecipeCompiler.Instance.OKNumber.AutoNGNumber;
            //File.WriteAllText(ProjectINI.TempPath + "NG概率.txt", data);
        }

        public bool ISOk;

        /// <summary>
        ///
        /// </summary>
        /// <param name="oKNumber"></param>
        public static void StaticAddData(RecipeCompiler.OKNumberClass oKNumber)
        {
            This.AddData(oKNumber);
        }

        /// <summary>
        /// 设置显示状态
        /// </summary>
        /// <param name="ISCont">1=OK.2=NG,3=OK-Done,其他=....</param>
        public static void SetOK(int ISCont)
        {
            This.Invoke(new Action(() =>
            {
                if (ISCont == 1)
                {
                    This.label2.BackColor = Color.Green;
                    This.label2.Text = "OK";
                }
                else if (ISCont == 2)
                {
                    This.label2.BackColor = Color.Red;
                    This.label2.Text = "NG";
                }
                else if (ISCont == 3)
                {
                    This.label2.BackColor = Color.Green;
                    This.label2.Text = "OK-Done";
                }
                else
                {
                    This.label2.BackColor = Color.Yellow;
                    This.label2.Text = "...";
                }
            }));
        }

        public static void SetString(string text, Color color)
        {
            This.Invoke(new Action(() =>
            {
                This.label2.BackColor = color;
                This.label2.Text = text;
            }));
        }

        /// <summary>
        /// 数据结果集合
        /// </summary>
        public class DataReseltBase
        {
            public string Name { get; set; }

            /// <summary>
            /// 结果集合，
            /// </summary>
            public Dictionary<string, Dictionary<string, bool>> DicBool { get; set; } = new Dictionary<string, Dictionary<string, bool>>();

            /// <summary>
            /// 结果
            /// </summary>
            public Dictionary<string, bool> ReseltBool { get; set; } = new Dictionary<string, bool>();

            /// <summary>
            /// 单次执行的程序结果
            /// </summary>
            public Dictionary<string, bool> CompoundReseltBool { get; set; } = new Dictionary<string, bool>();

            /// <summary>
            /// 程序的最大流程号
            /// </summary>
            public int MaxNumber { get; set; }

            /// <summary>
            /// 流程执行的结果
            /// </summary>
            public List<bool> ListReselt { get; set; } = new List<bool>();

            /// <summary>
            /// 机判结果
            /// </summary>
            public List<string> ListVerData = new List<string>();

            /// <summary>
            /// 结果
            /// </summary>
            public bool OK { get; set; }
        }

        public int MAXt;

        public static void StaticAddResult(int id, string name, OneDataVale dataVale)
        {
            try
            {
                foreach (var item in vision.Vision.GetHimageList())
                {
                    if (vision.Vision.GetSaveImageInfo(item.Key).ISCount)
                    {
                        if (This.MAXt < item.Value.MaxRunID)
                        {
                            This.MAXt = item.Value.MaxRunID;
                        }
                    }
                }
                if (id == 1)
                {
                    This.ListReslutMestTR.Clear();
                }
                int d = -10;
                string iDETX = name.Split(':')[0];

                This.isUP = true;

                if (dataVale.OK)
                {
                    if (RecipeCompiler.Instance.TrayQRType == RecipeCompiler.TrayEnumType.一个流程一个产品)
                    {
                        if (Vision.GetSaveImageInfo(name).ISCount)
                        {
                            if (RecipeCompiler.Instance.GetMes() != null)
                            {
                                RecipeCompiler.Instance.GetMes().WrietMes(dataVale, Product.ProductionName);
                            }
                            //WeirtOne(false);
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (RecipeCompiler.Instance.UpDataType == RecipeCompiler.EnumUpDataType.表格)
                    {
                        for (int i = 0; i < This.dataGridView1.Rows.Count; i++)
                        {
                            if (This.dataGridView1.Rows[i].Cells[0].Value == null)
                            {
                                d = i;
                                break;
                            }
                            string dtat = This.dataGridView1.Rows[i].Cells[0].Value.ToString().Split(':')[0];
                            if (dtat == iDETX)
                            {
                                d = i;
                                break;
                            }
                        }
                        if (d < 0 && !dataVale.OK)
                        {
                            d = This.dataGridView1.Rows.Add();
                        }
                        if (!dataVale.OK)
                        {
                            This.dataGridView1.Rows[d].Cells[0].Value = name;
                        }
                    }
                    else if (RecipeCompiler.Instance.UpDataType == RecipeCompiler.EnumUpDataType.复判按钮)
                    {
                        if (RecipeCompiler.Instance.TrayQRType == RecipeCompiler.TrayEnumType.一个流程一个产品)
                        {
                            if (Vision.GetSaveImageInfo(name).ISCount)
                            {
                                dataVales.Add(dataVale);
                            }
                            if (dataVale.OK)
                            {
                                dataVale.Done = true;
                                dataVale.OK = true;
                                //dataVale.RsetOK = true;
                                if (Vision.GetSaveImageInfo(name).ISCount)
                                {
                                    if (RecipeCompiler.Instance.GetMes() != null)
                                    {
                                        RecipeCompiler.Instance.GetMes().WrietMes(dataVale, Product.ProductionName);
                                    }

                                    //WeirtOne(false);
                                }
                            }
                        }
                    }
                }
                if (RecipeCompiler.Instance.UpDataType == RecipeCompiler.EnumUpDataType.托盘)
                {
                    This.trayDataUserControl.SetValue(id, dataVale.OK);
                    RecipeCompiler.AddOKNumber(dataVale.OK);
                    return;
                }
                if (This.ListReslutOK == null)
                {
                    This.ListReslutOK = new List<string>();
                }
                string listReslutS = name + "=";
                Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>();
                keyValuePairs.Clear();
                //if (text.ListVerData.Count > 0)
                //{
                //    This.ListReslutMestTR.Add(text.ListVerData[0]);
                //}
                //foreach (var item in dataVale.CompoundReseltBool)
                //{
                //    if (!item.Value)
                //    {
                //        keyValuePairs.Add(item.Key, true);
                //    }
                //}
                if (keyValuePairs.ContainsKey("OK"))
                {
                    keyValuePairs["OK"] = dataVale.OK;
                }
                else
                {
                    keyValuePairs.Add("OK", dataVale.OK);
                }
                bool isChat = dataVale.OK;
                if (!dataVale.OK)
                {
                    if (RecipeCompiler.Instance.UpDataType == RecipeCompiler.EnumUpDataType.表格)
                    {
                        foreach (var item in keyValuePairs)
                        {
                            for (int i = 0; i < This.dataGridView1.Columns.Count; i++)
                            {
                                if (This.dataGridView1.Columns[i].HeaderText == item.Key)
                                {
                                    if (item.Value == true)
                                    {
                                        This.dataGridView1.Rows[d].Cells[i].Value = true.ToString();
                                        if (item.Key != "OK")
                                        {
                                            if (item.Key.Contains("NG"))
                                            {
                                                listReslutS += item.Key + ";";
                                            }
                                            else
                                            {
                                                listReslutS += item.Key + "NG;";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.Key == "OK")
                                        {
                                            This.dataGridView1.Rows[d].Cells[i].Style.BackColor = Color.Red;
                                        }
                                        This.dataGridView1.Rows[d].Cells[i].Value = false.ToString();
                                    }
                                    goto endt;
                                }
                            }
                            DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                            dataGridViewCheckBoxColumn.Name = dataGridViewCheckBoxColumn.HeaderText = item.Key;
                            dataGridViewCheckBoxColumn.ReadOnly = true;
                            int dt = This.dataGridView1.Columns.Add(dataGridViewCheckBoxColumn);
                            if (item.Value == true)
                            {
                                This.dataGridView1.Rows[d].Cells[dt].Value = true.ToString();
                                if (item.Key.Contains("NG"))
                                {
                                    listReslutS += item.Key + ";";
                                }
                                else
                                {
                                    listReslutS += item.Key + "NG;";
                                }
                            }
                            else
                            {
                                This.dataGridView1.Rows[d].Cells[dt].Value = false.ToString();
                            }
                        endt:;
                        }
                        int dint = int.Parse(name.Split(':')[0]);
                        if (dint > This.ListReslutOK.Count)
                        {
                            This.ListReslutOK.Add(listReslutS);
                        }
                        else
                        {
                            This.ListReslutOK[dint - 1] = listReslutS;
                        }
                        for (int i = 0; i < This.dataGridView1.Columns.Count; i++)
                        {
                            if (This.dataGridView1.Columns[i].HeaderText == "OK")
                            {
                                This.dataGridView1.Rows[d].Cells[i].Value = keyValuePairs["OK"].ToString();
                                if (RecipeCompiler.Instance.TrayQRType == RecipeCompiler.TrayEnumType.多个流程一个产品)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                if (RecipeCompiler.Instance.TrayQRType == RecipeCompiler.TrayEnumType.一个流程一个产品)
                {
                    if (vision.Vision.GetSaveImageInfo(name).ISCount)
                    {
                        RecipeCompiler.AddOKNumber(id - 1, isChat);
                    }
                }
                else if (This.MAXt <= id && vision.Vision.GetSaveImageInfo(name).ISCount)
                {
                    data = dataVales[0];
                    for (int i = 0; i < dataVales.Count; i++)
                    {
                        if (!dataVales[i].OK)
                        {
                            dataVale.OK = false;
                            break;
                        }
                    }
                    dataVales.Clear();
                    if (RecipeCompiler.Instance.UpDataType == RecipeCompiler.EnumUpDataType.弹出复判按钮)
                    {
                        Vision.ShowVisionResetForm();
                    }
                    if (dataVale.OK)
                    {
                        if (RecipeCompiler.Instance.GetMes() != null)
                        {
                            RecipeCompiler.Instance.GetMes().WrietMes(dataVale, Product.ProductionName);
                        }
                        if (RecipeCompiler.Instance.Data.GetChet())
                        {
                        }
                    }
                    //text.ListReselt.Clear();
                }
                //if (Vision.Instance.IsShowImage)
                //{
                //    Vision.GetObj().OK = isChat;
                //}
            }
            catch (Exception ex)
            {
                AlarmText.AddTextNewLine("写入Mes错误:" + ex.Message);
            }
            This.isUP = false;
        }

        public static void StaticAddQRCode(string datas, int id = -1)
        {
            int det = -1;
            try
            {
                if (This.dataGridView1.InvokeRequired)
                {
                    This.dataGridView1.Invoke(new Action<string, int>(StaticAddQRCode), datas, id);
                }
                timeStrStrat = DateTime.Now;
                ProcessControl.ProcessUser.QRCode = datas;
                This.label4.Text = "产品码:" + ProcessControl.ProcessUser.QRCode;
                if (DebugF.IO.TrayDataUserControl.GetTray() != null)
                {
                    DebugF.IO.TrayDataUserControl.GetTray().GetTrayData().TrayIDQR = ProcessControl.ProcessUser.QRCode;
                }
                if (RecipeCompiler.Instance.SetTrageDoneNmae != "")
                {
                    StaticCon.SetLingkValue(RecipeCompiler.Instance.SetTrageDoneNmae, 1, out string err);
                }
                if (RecipeCompiler.Instance.TrayQRType == RecipeCompiler.TrayEnumType.多个流程一个产品)
                {
                    return;
                }
                if (id < 0)
                {
                    if (id > This.dataGridView1.Rows.Count)
                    {
                        This.dataGridView1.Rows.Add();
                    }
                    return;
                }
                for (int i = 0; i < This.dataGridView1.Rows.Count; i++)
                {
                    if (This.dataGridView1.Rows[i].Cells[0].Value.ToString().Split(':')[0] == id.ToString())
                    {
                        det = i;
                        break;
                    }
                }
            strt:
                if (det < 0)
                {
                    if (id > This.dataGridView1.Rows.Count)
                    {
                        det = This.dataGridView1.Rows.Add();
                    }
                }
                if (det < 0)
                {
                    det = This.dataGridView1.Rows.Add();
                }

                for (int i = 0; i < This.dataGridView1.Columns.Count; i++)
                {
                    if (This.dataGridView1.Columns[i].HeaderText == "二维码")
                    {
                        This.dataGridView1.Rows[det].Cells[i].Value = ProcessControl.ProcessUser.QRCode;
                        for (int i2 = 0; i2 < This.dataGridView1.Columns.Count; i2++)
                        {
                            if (This.dataGridView1.Columns[i2].HeaderText == "OK")
                            {
                                This.dataGridView1.Rows[det].Cells[i2].Value = true;
                                break;
                            }
                        }
                        This.dataGridView1.Rows[det].Cells[0].Value = id + ":";
                        return;
                    }
                }

                DataGridViewTextBoxColumn dataGridViewTextBoxCell = new DataGridViewTextBoxColumn();
                dataGridViewTextBoxCell.HeaderText = "二维码";
                dataGridViewTextBoxCell.Name = "二维码";
                This.dataGridView1.Columns.Add(dataGridViewTextBoxCell);
                goto strt;
            }
            catch (Exception ex)
            {
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.Directory.Exists(ProcessControl.ProcessUser.Instancen.ExcelPath))
                {
                    System.Diagnostics.Process.Start(ProcessControl.ProcessUser.Instancen.ExcelPath);
                }
                else
                {
                    System.Diagnostics.Process.Start(ProcessControl.ProcessUser.Instancen.ExcelPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void label4_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string name = "";
                name = Interaction.InputBox("输入二维码", "手动输入", name, 100, 100);
                if (true)
                {
                    StaticAddQRCode(name);
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isUP)
                {
                    return;
                }
                if (ListReslutOKTR == null)
                {
                    ListReslutOKTR = new List<string>();
                }
                if (ListReslutOKTR.Count != dataGridView1.Rows.Count)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        ListReslutOKTR.Add("");
                    }
                }
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "二维码")
                {
                    return;
                }
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "OK")
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString()))
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                        ListReslutOKTR[e.RowIndex] = "";
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Green;
                        for (int i = 1; i < dataGridView1.Columns.Count; i++)
                        {
                            if (dataGridView1.Columns[i].HeaderText != "OK" && dataGridView1.Columns[i].HeaderText != "二维码")
                            {
                                dataGridView1.Rows[e.RowIndex].Cells[i].Value = false;
                            }
                        }
                        ListReslutOKTR[e.RowIndex] = "OK";
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;
                    }
                }
                else if (e.ColumnIndex != 0)
                {
                    string[] dat = ListReslutOKTR[e.RowIndex].Split(';');
                    ListReslutOKTR[e.RowIndex] = "";
                    string Restl = "";
                    if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString()))
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                        Restl = "OK";
                    }
                    else
                    {
                        Restl = "NG";
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                    }
                    if (dat.Length == 1 && dat[0] == "")
                    {
                        dat[0] = dataGridView1.Columns[e.ColumnIndex].HeaderText + Restl;
                    }
                    else if (dat.Length == 1 && dat[0] != "")
                    {
                        if (dat[0].Contains(dataGridView1.Columns[e.ColumnIndex].HeaderText))
                        {
                            dat[0] = dataGridView1.Columns[e.ColumnIndex].HeaderText + Restl;
                        }
                        else
                        {
                            dat[0] += ";" + dataGridView1.Columns[e.ColumnIndex].HeaderText + Restl;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dat.Length; i++)
                        {
                            if (dat[i].Contains(dataGridView1.Columns[e.ColumnIndex].HeaderText))
                            {
                                dat[i] = dataGridView1.Columns[e.ColumnIndex].HeaderText + Restl;
                                goto EndInvoke;
                            }
                        }
                        dat[dat.Length - 1] = dataGridView1.Columns[e.ColumnIndex].HeaderText + Restl;
                    }
                EndInvoke:
                    for (int i = 0; i < dat.Length; i++)
                    {
                        ListReslutOKTR[e.RowIndex] += dat[i] + ";";
                    }
                    ListReslutOKTR[e.RowIndex] = ListReslutOKTR[e.RowIndex].TrimEnd(';');
                    if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString()))
                    {
                        for (int i = 1; i < dataGridView1.Columns.Count; i++)
                        {
                            dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                            dataGridView1.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.Red;
                            if (dataGridView1.Columns[i].HeaderText == "OK")
                            {
                                dataGridView1.Rows[e.RowIndex].Cells[i].Value = false;
                                if (RecipeCompiler.Instance.TrayQRType == RecipeCompiler.TrayEnumType.一个流程一个产品)
                                {
                                    RecipeCompiler.AlterNumber(false, e.RowIndex);
                                    return;
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        HalconRun halcon = Vision.GetRunNameVision();
                        halcon.HobjClear();
                        if (dataGridView1.Rows[e.RowIndex].Cells[0].Value == null)
                        {
                            return;
                        }
                        HWindowControl control = halcon.GetHWindow().GetNmaeWindowControl(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                        if (control != null)
                        {
                            OneResultOBj halconResult = control.Tag as OneResultOBj;
                            halcon.ShowImage(halconResult.Image);
                            halcon.SetResultOBj(halconResult);
                            halcon.GetOneImageR().ShowAll(halcon.hWindowHalcon());
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                if (e.ColumnIndex != 0)
                {
                    if (RecipeCompiler.Instance.TrayQRType == RecipeCompiler.TrayEnumType.多个流程一个产品)
                    {
                        for (int i = 0; i < ListReslutOKTR.Count; i++)
                        {
                            if (ListReslutOK[i].Split('=')[1] != "" && ListReslutOKTR[i] != "OK")
                            {
                                RecipeCompiler.AlterNumber(false);
                                return;
                            }
                            if (ListReslutOKTR[i] != "" && ListReslutOKTR[i] != "OK")
                            {
                                RecipeCompiler.AlterNumber(false);
                                return;
                            }
                        }
                        RecipeCompiler.AlterNumber(true);
                    }
                    else
                    {
                        if (Int16.TryParse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Split(':')[0], out Int16 DWET))
                        {
                            if (Convert.ToBoolean(dataGridView1["OK", e.RowIndex].EditedFormattedValue.ToString()))
                            {
                                RecipeCompiler.AlterNumber(true, DWET - 1);
                            }
                            else
                            {
                                RecipeCompiler.AlterNumber(false, DWET - 1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 清除数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ClearData();
                RestObjImage.Clser();
            }
            catch (Exception)
            {
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.TryParse(DebugCompiler.Instance.LinklamplightName, out int result))
                {
                    if (DebugCompiler.GetDoDi().Out[result])
                    {
                        Vision.TriggerSetup(DebugCompiler.Instance.LinklamplightName, false.ToString());
                        button5.BackColor = Color.White;
                    }
                    else
                    {
                        Vision.TriggerSetup(DebugCompiler.Instance.LinklamplightName, true.ToString());
                        button5.BackColor = Color.Green;
                    }
                }
                else
                {
                    if (Convert.ToInt32(StaticCon.GetLingkNameValue(DebugF.DebugCompiler.Instance.LinklamplightName)) > 0)
                    {
                        StaticCon.SetLinkAddressValue(DebugF.DebugCompiler.Instance.LinklamplightName, false);
                        button5.BackColor = Color.White;
                    }
                    else
                    {
                        StaticCon.SetLinkAddressValue(DebugF.DebugCompiler.Instance.LinklamplightName, true);
                        button5.BackColor = Color.Green;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private LinkDataForm1 linkDataForm1;

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
        }

        public static OneDataVale GetDataVale(OneDataVale dataD = null)
        {
            if (dataD != null)
            {
                data = dataD;
            }
            return data;
        }

        private static OneDataVale data;

        private static TrayRobot TrayReset;

        private static List<OneDataVale> dataVales = new List<OneDataVale>();

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void hWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    timer1.Interval = 500;
            //    if (!timer1.Enabled)
            //    {
            //        timer1.Start();
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (textBox1.Text.Length >= numericUpDown1.Value)
            //{
            //    StaticAddQRCode(textBox1.Text);
            //    textBox1.Text = "";
            //}
        }

        private void 附加测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linkDataForm1 == null || linkDataForm1.IsDisposed)
            {
                linkDataForm1 = new LinkDataForm1();
                linkDataForm1.SetData(RecipeCompiler.Instance.Data);
            }
            ErosProjcetDLL.UI.UICon.WindosFormerShow(ref linkDataForm1);
        }

        private Form form;

        private void mES设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (form == null || form.IsDisposed)
                {
                    form = RecipeCompiler.Instance.GetMes().GetForm();
                }
                UICon.SwitchToThisWindow(form.Handle, true);
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (isUP)
                {
                    return;
                }
                DebugCompiler.Instance.LinkSeelpTyoe = (byte)toolStripComboBox1.SelectedIndex;
                DebugCompiler.Instance.SetSeelp();
            }
            catch (Exception)
            {
            }
        }

        private void 打开数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.Directory.Exists(ProcessControl.ProcessUser.Instancen.ExcelPath))
                {
                    System.Diagnostics.Process.Start(ProcessControl.ProcessUser.Instancen.ExcelPath);
                }
                else
                {
                    System.Diagnostics.Process.Start(ProcessControl.ProcessUser.Instancen.ExcelPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 清除数据ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否清除数据?", "清除产能数据", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                RecipeCompiler.ResetDATA();
                label2.Text = "";
                label2.BackColor = Color.Wheat;
            }
        }

        private void toolStripDropDownButton1_MouseMove(object sender, EventArgs e)
        {
            toolStripDropDownButton1.ShowDropDown();
        }

        private void toolStripDropDownButton1_MouseLeave(object sender, EventArgs e)
        {
            toolStripDropDownButton1.HideDropDown();
        }

        private void 清除统计数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否清除数据?", "清除产能数据", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                RecipeCompiler.ResetDATA();
                label2.Text = "";
                label2.BackColor = Color.Wheat;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ProjectINI.In.UserID = textBox2.Text;
            }
            catch (Exception)
            { }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Product.Work_Order = textBox3.Text;
        }

        private void 打开历史ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.Directory.Exists(RecipeCompiler.Instance.GetMes().DataPaht))
                {
                    System.Diagnostics.Process.Start(RecipeCompiler.Instance.GetMes().DataPaht);
                }
                else
                {
                    System.Diagnostics.Process.Start(RecipeCompiler.Instance.GetMes().DataPaht);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 选择Mes地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.Description = "请选择文件夹";
                if (ProcessControl.ProcessUser.Instancen.ExcelPath == null)
                {
                    ProcessControl.ProcessUser.Instancen.ExcelPath = Application.StartupPath;
                }
                if (System.IO.Directory.Exists(ProcessControl.ProcessUser.Instancen.ExcelPath.ToString()))
                {
                    fbd.SelectedPath = ProcessControl.ProcessUser.Instancen.ExcelPath.ToString();
                }
                System.Windows.Forms.DialogResult dialog = FolderBrowserLauncher.ShowFolderBrowser(fbd);
                if (dialog == System.Windows.Forms.DialogResult.OK)
                {
                    ProcessControl.ProcessUser.Instancen.ExcelPath = fbd.SelectedPath;
                    ProcessControl.ProcessUser.Instancen.SaveThis();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 选择历史数据地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择文件夹";

                Directory.CreateDirectory(RecipeCompiler.Instance.GetMes().DataPaht);
                if (Directory.Exists(RecipeCompiler.Instance.GetMes().DataPaht))
                {
                    fbd.SelectedPath = RecipeCompiler.Instance.GetMes().DataPaht;
                }
                DialogResult dialog = FolderBrowserLauncher.ShowFolderBrowser(fbd);
                if (dialog == DialogResult.OK)
                {
                    RecipeCompiler.Instance.GetMes().DataPaht = fbd.SelectedPath;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                //chartType.Series.Clear();
                //           chartType.ChartAreas.Clear();

                //Series Series1 = new Series();
                //chartType.Series.Add(Series1);
                chartType.Series["Series1"].ChartType = SeriesChartType.Pie;
                chartType.Legends[0].Enabled = true;
                chartType.Series["Series1"].LegendText = "#INDEX:#VALX";//开启图例
                chartType.Series["Series1"].Label = "#INDEX:#PERCENT";
                chartType.Series["Series1"].IsXValueIndexed = false;
                chartType.Series["Series1"].IsValueShownAsLabel = false;
                chartType.Series["Series1"]["PieLineColor"] = "Black";//连线颜色
                chartType.Series["Series1"]["PieLabelStyle"] = "Outside";//标签位置
                chartType.Series["Series1"].ToolTip = "#VALX";//显示提示用语
                                                              //ChartArea ChartArea1 = new ChartArea();
                                                              //chartType.ChartAreas.Add(ChartArea1);
                                                              //开启三维模式的原因是为了避免标签重叠
                chartType.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;//开启三维模式;PointDepth:厚度BorderWidth:边框宽
                chartType.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 15;//起始角度
                chartType.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 45;//倾斜度(0～90)
                chartType.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.Realistic;//表面光泽度
                chartType.Series[0].XValueMember = "name";
                chartType.Series[0].YValueMembers = "sumcount";
                //int x = 0;
                //float[] values = { 905, 100, 20, 23, 60, 87, 42, 77, 92, 51, 29 };
                //foreach (float v in values)

                //{
                //    chartType.Series["Series1"].Points.AddXY(x, v);
                //    x++;

                //}
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            try
            {
                chartType.Series.Clear();
                chartType.ChartAreas.Clear();
                Series Series1 = new Series();
                chartType.Series.Add(Series1);
                chartType.Series["Series1"].ChartType = SeriesChartType.Column;
                chartType.Legends[0].Enabled = false;
                chartType.Series["Series1"].LegendText = "";
                chartType.Series["Series1"].Label = "#VALY";
                chartType.Series["Series1"].ToolTip = "#VALX";
                chartType.Series["Series1"]["PointWidth"] = "0.5";
                ChartArea ChartArea1 = new ChartArea();
                chartType.ChartAreas.Add(ChartArea1);
                //开启三维模式的原因是为了避免标签重叠
                chartType.ChartAreas["ChartArea1"].AxisY.Interval = 50;
                chartType.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;//开启三维模式;PointDepth:厚度BorderWidth:边框宽
                chartType.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 20;//起始角度
                chartType.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 15;//倾斜度(0～90)
                chartType.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.Realistic;//表面光泽度
                chartType.ChartAreas["ChartArea1"].AxisX.Interval = 1; //决定x轴显示文本的间隔，1为强制每个柱状体都显示，3则间隔3个显示
                chartType.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("宋体", 9, FontStyle.Regular);
                chartType.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                chartType.Series[0].XValueMember = "name";
                chartType.Series[0].YValueMembers = "sumcount";
                //ChartArea1.AxisX.w
                ChartArea1.AxisX.Minimum = 0;

                ChartArea1.AxisX.Maximum = 24;

                ChartArea1.AxisY.Minimum = 0d;
                int x = 0;

                RecipeCompiler.OKNumberClass[] oKNumberClass = RecipeCompiler.Instance.GetOKNumberList();
                List<int> vs = new List<int>();

                for (int i = 0; i < oKNumberClass.Length; i++)
                {
                    vs.Add(oKNumberClass[i].Number);
                }

                foreach (int v in vs)
                {
                    chartType.Series["Series1"].Points.AddXY(x, v);
                    x++;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                chartType.Series["Series1"].Points.Clear();
                // 在chart中显示数据
                chartType.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                chartType.ChartAreas["ChartArea1"].AxisY.Interval = 50;
                int x = 0;
                float[] values = { 105, 100, 20, 23, 60, 87, 42, 77, 92, 51, 29 };
                foreach (float v in values)

                {
                    chartType.Series["Series1"].Points.AddXY(x, v);
                    x++;
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = ProjectINI.ProjectPathRun + "\\产品配方\\";
            openFileDialog.Filter = "csv文件|*.Csv";
            DialogResult dialog = openFileDialog.ShowDialog();
            try
            {
                if (dialog == DialogResult.OK)
                {
                    Npoi.ReadText(openFileDialog.FileName, out List<string> str);

                    if (str == null)
                    {
                        MessageBox.Show("参数信息不存在;" + Environment.NewLine);
                    }
                    else
                    {
                        string dsat = Path.GetDirectoryName(openFileDialog.FileName) + "\\";
                        dsat += Path.GetFileNameWithoutExtension(openFileDialog.FileName) + "CRD";
                        for (int i = 0; i < str.Count; i++)
                        {
                            if (str[i].Contains("导航图1"))
                            {
                                str[i] = str[i].Remove(str[i].IndexOf("导航图1."));
                            }
                        }
                        //string daa=       str[4000];
                        Npoi.WriteF(dsat, str);
                        MessageBox.Show("处理完成");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败:" + ex.Message);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (textBox1.Text.Length >= numericUpDown1.Value)
                //{
                StaticAddQRCode(textBox1.Text);
                textBox1.Text = "";
            }
            catch (Exception)
            {
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RecipeCompiler.Instance.GetMes().Line_Name = comboBox2.SelectedItem.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in RecipeCompiler.Instance.GetMes().ListStr)
                {
                    if (!comboBox2.Items.Contains(item))
                    {
                        comboBox2.Items.Add(item);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RecipeCompiler.Instance.GetMes().Fixture_ID = comboBox3.SelectedItem.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void comboBox3_DropDown(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in RecipeCompiler.Instance.GetMes().FixtureList)
                {
                    if (!comboBox3.Items.Contains(item))
                    {
                        comboBox3.Items.Add(item);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
        }
    }
}