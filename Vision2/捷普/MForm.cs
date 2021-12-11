using System;
using System.Drawing;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project.DebugF;
using Vision2.Project.formula;
using Vision2.Project.Mes;
using Vision2.vision;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.捷普
{
    public partial class MForm : Form
    {
        public MForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (textBox1.Text.Length>=numericUpDown1.Value)
                // {
                // }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool MesRestBool;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
            
                if (ProjectINI.DebugMode || MesRestBool)
                {
                    //RecipeCompiler.Instance.MesDatat.UserID = textBox3.Text;
                    textBox2.Text = mesJib.MesData.Testre_Name;
                    UserFormulaContrsl.StaticAddQRCode(textBox1.Text);
                    DebugCompiler.Start();
                }
                if (ProjectINI.DebugMode)
                {
                    label6.Text = "Pass";
                    label6.BackColor = Color.Yellow;
                }
                tabControl1.SelectedIndex = 1;
                Project.MainForm1.MainFormF.WindowState = FormWindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private MesJib mesJib;

        private void MForm_Load(object sender, EventArgs e)
        {
            try
            {

                mesJib = RecipeCompiler.Instance.GetMes() as MesJib;
                this.Text += Application.StartupPath;
                comboBox1.Items.Clear();
                textBox2.Text = mesJib.MesData.Testre_Name;
                vision.Vision.GetRunNameVision().EventDoen += MForm_EventDoen;
                comboBox1.Items.Add(@"D:\NGP_SOFT\NGP BOARD STACK DIMENSION TEST_REV.B.JTS");
                comboBox1.SelectedIndex = 0;
                tabControl1.SelectedIndex = 1;
                timer1.Interval = 100;
                timer1.Start();
                ProjectINI.In.User.EventLog += User_EventLog; ;
                HalconRun halconRun = Vision.GetRunNameVision();
                textBox5.Text = RecipeCompiler.Instance.OKNumber.TrayNumber.ToString();
                foreach (var item in halconRun.GetRunProgram())
                {
                    MeasureMlet measureMlet = item.Value as MeasureMlet;
                    if (measureMlet == null)
                    {
                        continue;
                    }
                    string numbrEE = "null";
                    string txdet = "读取" + item.Key;
                    Double dvalue = 0;
                    if (item.Key == "P1_dist")
                    {
                        numbrEE = "";

                        string data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "cal_data_mm", "P1_dist_pixels");
                        if (double.TryParse(data, out dvalue))
                            measureMlet.Scale = dvalue;
                        else numbrEE += "读取比例;";
                        data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "dist_mm", "P1_dist");
                        measureMlet.ResValue = double.Parse(data);
                        if (double.TryParse(data, out dvalue))
                            measureMlet.ResValue = dvalue;
                        else numbrEE += "读取参考值;";
                        data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "p1_limit_mm", "limit_lo");

                        if (double.TryParse(data, out dvalue))
                            measureMlet.DistanceMin = dvalue / 25.4;
                        else numbrEE += "读取下限;";
                        data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "p1_limit_mm", "limit_hi");

                        if (double.TryParse(data, out dvalue))
                            measureMlet.DistanceMax = dvalue / 25.4;
                        else numbrEE += "读取上限;";
                    }
                    else if (item.Key == "P2_dist")
                    {
                        numbrEE = "";
                        item.Value.Run(halconRun.GetOneImageR(), new AoiObj());

                        string data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "cal_data_mm", "P2_dist_pixels");
                        measureMlet.Scale = double.Parse(data);
                        if (double.TryParse(data, out dvalue))
                            measureMlet.Scale = dvalue;
                        else numbrEE += "读取比例;";
                        data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "dist_mm", "P2_dist");
                        measureMlet.ResValue = double.Parse(data);
                        if (double.TryParse(data, out dvalue))
                            measureMlet.ResValue = dvalue;
                        else numbrEE += "读取参考值;";
                        data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "p2_limit_mm", "limit_lo");
                        if (double.TryParse(data, out dvalue))
                            measureMlet.DistanceMin = dvalue / 25.4;
                        else numbrEE += "读取下限;";
                        data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "p2_limit_mm", "limit_hi");
                        if (double.TryParse(data, out dvalue))
                            measureMlet.DistanceMax = dvalue / 25.4;
                        else numbrEE += "读取上限;";
                    }
                    else if (item.Key == "P3_dist")
                    {
                        numbrEE = "";

                        string data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "cal_data_mm", "P3_dist_pixels");
                        measureMlet.Scale = double.Parse(data);
                        if (double.TryParse(data, out dvalue))
                            measureMlet.Scale = dvalue;
                        else numbrEE += "读取比例;";
                        data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "dist_mm", "P3_dist");
                        if (double.TryParse(data, out dvalue))
                            measureMlet.ResValue = dvalue;
                        else numbrEE += "读取参考值;";
                        data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "p3_limit_mm", "limit_lo");
                        if (double.TryParse(data, out dvalue))
                            measureMlet.DistanceMin = dvalue / 25.4;
                        else
                            numbrEE += "读取下限;";
                        data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "p3_limit_mm", "limit_hi");
                        if (double.TryParse(data, out dvalue))
                            measureMlet.DistanceMax = dvalue / 25.4;
                        else numbrEE += "读取上限;";
                    }
                    else if (item.Key == "P4_dist")
                    {
                        numbrEE = "";

                        string data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "cal_data_mm", "P4_dist_pixels");
                        if (double.TryParse(data, out dvalue))
                            measureMlet.Scale = dvalue;
                        else numbrEE += "读取比例;";

                        data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "dist_mm", "P4_dist");

                        if (double.TryParse(data, out dvalue))
                            measureMlet.ResValue = dvalue;
                        else numbrEE += "读取参考值;";

                        data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "p4_limit_mm", "limit_lo");
                        if (double.TryParse(data, out dvalue))
                            measureMlet.DistanceMin = dvalue / 25.4;
                        else
                            numbrEE += "读取下限;";

                        data = ProjectINI.GetInI(Application.StartupPath + @"\setting.ini", "p4_limit_mm", "limit_hi");

                        if (double.TryParse(data, out dvalue))
                            measureMlet.DistanceMax = dvalue / 25.4;
                        else numbrEE += "读取上限;";
                    }
                    if (numbrEE == "")
                    {
                        richTextBox1.AppendText(txdet + "成功" + Environment.NewLine);
                    }
                    else
                    {
                        richTextBox1.AppendText(txdet + "失败:" + numbrEE + Environment.NewLine);
                    }
                }
                textBox3.Text = ProjectINI.In.UserID;
                校验ToolStripMenuItem.Visible = false;
                if (ProjectINI.Enbt)
                {
                    校验ToolStripMenuItem.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void User_EventLog(bool isLog)
        {
            try
            {
                textBox3.Text = ProjectINI.In.UserID;
                if (ProjectINI.Enbt)
                {
                    校验ToolStripMenuItem.Visible = true;
                }
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// 执行完成
        /// </summary>
        /// <param name="key"></param>
        private void RunCodeT_RunDone(Project.DebugF.IO.RunCodeStr.RunErr key)
        {
            try
            {
               
                this.Invoke(new Action(() =>
                {
      
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        dataGridView1.Rows[i].Cells[8].Value = key.RunTime;
                    }
                    string name = textBox1.Text +
                    UserFormulaContrsl.GetDataVale().EndTime.ToString(
                        mesJib.MesData.FileTimeName);
                    if (ProjectINI.DebugMode)
                    {
                        name = "DEBUG-" + name;
                    }

                    string path = mesJib.MesData.TEPath + "\\" + name;
                    if (UserFormulaContrsl.GetDataVale().OK)
                    {
                        label7.BackColor = Color.Green;
                        label7.Text = "Pass";
                    }
                    else
                    {
                        button8.Visible = true;
                        label7.BackColor = Color.Red;
                        label7.Text = "Fail";
                    }
                    textBox1.Text = "";
                    string textd = "00:00:";
                    label9.Text = textd + key.RunTime.ToString("00");
            
                    if (!name.Contains("DEBUG"))
                    {
                        HtmlMaker.Html.GenerateCode(path, key.RunTime, UserFormulaContrsl.timeStrStrat,
                         DateTime.Now, UserFormulaContrsl.GetDataVale());
                    }
                    HtmlMaker.Html.GenerateCode(RecipeCompiler.Instance.DataPaht + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + name
                               , key.RunTime, UserFormulaContrsl.timeStrStrat, DateTime.Now, UserFormulaContrsl.GetDataVale());
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 图像完成
        /// </summary>
        /// <param name="oneResultO"></param>
        private void MForm_EventDoen(vision.OneResultOBj oneResultO)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    dataGridView1.Rows.Clear();
                    foreach (var item in oneResultO.GetNgOBJS().DicOnes)
                    {
                        foreach (var itemdt in item.Value.oneRObjs)
                        {
                            int index = dataGridView1.Rows.Add();
                            dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.White;
                            if (itemdt.dataMinMax.GetRsetOK())
                            {
                                dataGridView1.Rows[index].Cells[2].Value = "Pass";
                            }
                            else
                            {
                                dataGridView1.Rows[index].Cells[2].Value = "Fail";
                                dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                            }
                            dataGridView1.Rows[index].Cells[0].Value = "Dimension Analysis";
                            dataGridView1.Rows[index].Cells[1].Value = item.Value.ComponentID;
                            dataGridView1.Rows[index].Cells[1].Value = itemdt.ComponentID;
                            dataGridView1.Rows[index].Cells[5].Value = vision.Vision.Instance.TransformName;
                            if (itemdt.dataMinMax.Reference_Name.Count>=1)
                            {
                                dataGridView1.Rows[index].Cells[3].Value = itemdt.dataMinMax.Reference_Name[0];
                                dataGridView1.Rows[index].Cells[4].Value = itemdt.dataMinMax.doubleV[0].Value.ToString("0.000000");
                                dataGridView1.Rows[index].Cells[6].Value = itemdt.dataMinMax.Reference_ValueMin[0];
                                dataGridView1.Rows[index].Cells[7].Value = itemdt.dataMinMax.Reference_ValueMax[0];
                            }
                         
                        }
                    }
                }));
            }
            catch (Exception)
            {
            }
        }

        private void MForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出程序？", "退出程序", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                ProjectINI.In.Clros();
            }
            e.Cancel = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Project.MainForm1.MainFormF.WindowState = FormWindowState.Minimized;
            timer1.Stop();
            DebugCompiler.Instance.DDAxis.RunCodeT.RunDone += RunCodeT_RunDone;
            DebugCompiler.Instance.DDAxis.EventRunDone += DDAxis_EventRunDone; ;
        }

        private void DDAxis_EventRunDone(bool done)
        {
            try
            {
                textBox5.Text= RecipeCompiler.Instance.OKNumber.TrayNumber.ToString();
            }
            catch (Exception)
            {
            }
          
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            htmlMaker.Form1 form1 = new htmlMaker.Form1();
            form1.ShowDialog();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strErr = "";
                    ProjectINI.DebugMode = false;

                    if (ProjectINI.Enbt)
                    {
                        DebugSele debugSele = new DebugSele();
                        debugSele.ShowDialog();
                    }
                    bool Passr = true;
                    if (!RestMesEnb)
                    {
                        MesRestBool = true;
                    }
                    if (!ProjectINI.DebugMode)
                    {
                        if (RestMesEnb)
                        {
                            Passr = RecipeCompiler.Instance.GetMes().ReadMes(textBox1.Text, out strErr);
                            MesRestBool = Passr;
                        }
                        if (Passr)
                        {
                            label6.BackColor = Color.ForestGreen;
                            OperatorFormShow operatorFormShow = new OperatorFormShow();
                            operatorFormShow.ShowDialog();
                            bool Err = false;
                            foreach (var item in ProjectINI.In.User.UserPassWords)
                            {
                                if (item.Value.UserID == ProjectINI.In.UserID)
                                {
                                    Err = true;
                                    break;
                                }
                            }
                            if (!Err)
                            {
                                richTextBox1.AppendText(DateTime.Now.ToString() + "用户ID无权限" + Environment.NewLine);
                                MessageBox.Show("用户ID无权限!");
                            }
                            else
                            {
                                button1.PerformClick();
                            }
                        }
                        else
                        {
                            richTextBox1.AppendText(DateTime.Now.ToString() + strErr + Environment.NewLine);
                            label6.BackColor = Color.Red;
                        }
                        textBox3.Text = ProjectINI.In.UserID;
                        label6.Text = strErr;
                    }
                    else
                    {
                        button1.PerformClick();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripDropDownButton3_Click_1(object sender, EventArgs e)
        {
        }

        private void eT数据地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.Description = "请选择文件夹";
                    if (mesJib.MesData.TEPath == null)
                    {
                        mesJib.MesData.TEPath = Application.StartupPath;
                    }
                    if (System.IO.Directory.Exists(mesJib.MesData.TEPath))
                    {
                        fbd.SelectedPath = mesJib.MesData.TEPath;
                    }
                    DialogResult dialog = FolderBrowserLauncher.ShowFolderBrowser(fbd);
                    if (dialog == DialogResult.OK)
                    {
                        mesJib.MesData.TEPath = fbd.SelectedPath;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.GetDoDi().WritDO(3, false);
                button8.Visible = false;
            }
            catch (Exception)
            {
            }
        }

        private void 校验ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProjectINI.DebugMode)
                {
                    CRRForm cRRFor = new CRRForm();
                    cRRFor.ShowDialog();
                }
                else
                {
                    MessageBox.Show("请切换到Debug模式");
                }
            }
            catch (Exception)
            {
            }
        }

        private void loingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LandingForm landingForm = new LandingForm();
                landingForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripDropDownButton6_Click(object sender, EventArgs e)
        {
        }

        public static Boolean RestMesEnb = true;

        private void cAMXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CXAMForm CXAMForm = new CXAMForm();
            CXAMForm.ShowDialog();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}