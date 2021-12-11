using ErosSocket.ErosConLink;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Vision2.Project.formula;

namespace Vision2.Project
{
    public partial class SimulateTrayMesForm : Form
    {
        public SimulateTrayMesForm()
        {
            InitializeComponent();
        }

        ///// <summary>
        ///// 判断结束
        ///// </summary>
        //public static bool RresOK = false;

        ///// <summary>
        ///// 放行等待
        ///// </summary>
        // static bool RresWait = false;

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private ErosSocket.DebugPLC.Robot.TrayData trayRobot1;

        public static void ShowMesabe(string text, ErosSocket.DebugPLC.Robot.TrayData trayRobot, bool await = false)
        {
            try
            {
                if (trayRobot.OK)
                {
                    if (RecipeCompiler.Instance.GetMes() != null)
                    {
                        RecipeCompiler.Instance.GetMes().WrietMesAll(trayRobot,  Product.ProductionName);
                    }
                    DebugF.IO.DODIAxis.RresOK = true;
                    DebugF.IO.DODIAxis.RresWait = false;
                    return;
                }
                DebugF.IO.DODIAxis.RresOK = false;
                DebugF.IO.DODIAxis.RresWait = true;
                void Show()
                {
                    MainForm1.MainFormF.Invoke(new Action(() =>
                    {
                        SimulateTrayMesForm simulateQRForm = new SimulateTrayMesForm();
                        simulateQRForm.trayRobot1 = trayRobot;
                        simulateQRForm.label2.Text = "提示信息:" + text;
                        simulateQRForm.checkedListBox1.Tag = trayRobot;
                        simulateQRForm.checkedListBox1.Items.Clear();
                        for (int i = 0; i < trayRobot.GetDataVales().Count; i++)
                        {
                            if (trayRobot.GetDataVales()[i] == null)
                            {
                                simulateQRForm.checkedListBox1.Items.Add(i + 1);
                            }
                            else
                            {
                                if (!trayRobot.GetDataVales()[i].OK)
                                {
                                    simulateQRForm.checkedListBox1.Items.Add(i + 1 + ":" + trayRobot.GetDataVales()[i].PanelID);
                                }
                            }
                        }
                        simulateQRForm.Show();
                    }));
                }
                if (await)
                {
                    Thread thread = new Thread(() =>
                    {
                        Show();
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                else
                {
                    Show();
                }
            }
            catch (Exception)
            {
            }
        }

        private void SimulateTrayMesForm_Load(object sender, EventArgs e)
        {
            try
            {
                DebugF.DebugCompiler.Buzzer = true;
                Thread thread = new Thread(() =>
                {
                    while (this.Visible)
                    {
                        try
                        {
                            if (checkBox1.Checked)
                            {
                                checkBox1.BackColor = Color.Red;
                                Thread.Sleep(500);
                            }
                            checkBox1.BackColor = Color.Wheat;
                        }
                        catch (Exception)
                        {
                        }
                        Thread.Sleep(500);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (RecipeCompiler.Instance.RsetSever)
            {
                string jsonStr = JsonConvert.SerializeObject(trayRobot1);
                if (StaticCon.GetSocketClint(RecipeCompiler.Instance.RsetSoeverLinkName) != null)
                {
                    StaticCon.GetSocketClint(RecipeCompiler.Instance.RsetSoeverLinkName).Send("RsetTray" + jsonStr);
                }
            }
            else
            {
                if (RecipeCompiler.Instance.GetMes() != null)
                {
                    RecipeCompiler.Instance.GetMes().WrietMesAll(trayRobot1, Product.ProductionName);
                }
            }

            DebugF.IO.DODIAxis.RresOK = true;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            DebugF.IO.DODIAxis.RresOK = true;
            this.Close();
        }

        private void checkedListBox1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private void checkedListBox1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    int j = checkedListBox1.IndexFromPoint(e.Location);
                    checkedListBox1.SelectedIndex = j;
                    if (j != CheckedListBox.NoMatches)
                    {
                        groupBox1.Text = checkedListBox1.Items[j].ToString();
                        dataGridView1.Visible = true;
                        dataGridView1.Rows.Clear();
                        bool IsCa = false;
                        string datd = checkedListBox1.Items[j].ToString();
                        int id = int.Parse(datd.Split(':')[0]) - 1;
                        ErosSocket.DebugPLC.Robot.TrayData trayRobot = checkedListBox1.Tag as ErosSocket.DebugPLC.Robot.TrayData;
                        if (trayRobot != null)
                        {
                            List<double> vs = trayRobot.GetDataVales()[id].Data as List<double>;
                            if (vs != null)
                            {
                                for (int i = 0; i < vs.Count; i++)
                                {
                                    try
                                    {
                                        int d = dataGridView1.Rows.Add();
                                        dataGridView1.Rows[d].Cells[0].Style.BackColor = System.Drawing.Color.White;
                                        if (formula.RecipeCompiler.Instance.Data.ListDatV.Count > i)
                                        {
                                            dataGridView1.Rows[d].Cells[0].Value = RecipeCompiler.Instance.Data.ListDatV[i].ComponentName;
                                        }
                                        dataGridView1.Rows[d].Cells[1].Value = vs[i];
                                        dataGridView1.Rows[d].Cells[2].Value = RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMin;
                                        dataGridView1.Rows[d].Cells[3].Value = RecipeCompiler.Instance.Data.ListDatV[i].Reference_ValueMax[i];

                                        if (!RecipeCompiler.Instance.Data.ListDatV[i].GetRsetOK())
                                        {
                                            dataGridView1.Rows[d].Cells[0].Style.BackColor = Color.Red;
                                            IsCa = true;
                                        }
                                        //if (formula.RecipeCompiler.Instance.Data.Reference_ValueMin[i] > vs[i])
                                        //{
                                        //    dataGridView1.Rows[d].Cells[2].Style.BackColor = Color.Red;
                                        //}
                                        //if (formula.RecipeCompiler.Instance.Data.Reference_ValueMax[i] < vs[i])
                                        //{
                                        //    dataGridView1.Rows[d].Cells[3].Style.BackColor = Color.Red;
                                        //}
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }

                                //dset = dataGridView1.Rows.Add();
                                //dataGridView1.Rows[dset].Cells[0].Value = "TrayId:";
                                //dataGridView1.Rows[dset].Cells[1].Value = trayRobot.dataObjs[j].TrayIDQR;
                            }
                        }
                    }

                    //Control control=     checkedListBox1.GetChildAtPoint(e.Location);
                    //checkedListBox1.g
                }
            }
            catch (Exception)
            {
            }
        }

        private void simulateQRForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F10)
                {
                    if (checkBox1.Checked)
                    {
                        checkBox1.Checked = false;
                    }
                    else
                    {
                        checkBox1.Checked = true;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                ErosSocket.DebugPLC.Robot.TrayData trayRobot = checkedListBox1.Tag as ErosSocket.DebugPLC.Robot.TrayData;
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    string datd = checkedListBox1.Items[i].ToString();
                    int id = int.Parse(datd.Split(':')[0]) - 1;
                    if (trayRobot != null)
                    {
                        trayRobot.GetDataVales()[id].OK = checkedListBox1.GetItemChecked(i);
                        trayRobot.GetDataVales()[id].Done = trayRobot.GetDataVales()[id].OK;
                        //RecipeCompiler.AlterNumber(trayRobot.dataObjs[id].OK, id + 1);
                    }
                }
                if (trayRobot.OK)
                {
                    label3.Text = "OK";
                    label3.BackColor = Color.Green;
                }
                else
                {
                    label3.Text = "NG";
                    label3.BackColor = Color.Red;
                }
            }
            catch (Exception)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                ErosSocket.DebugPLC.Robot.TrayData trayRobot = checkedListBox1.Tag as ErosSocket.DebugPLC.Robot.TrayData;
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    string datd = checkedListBox1.Items[i].ToString();
                    int id = int.Parse(datd.Split(':')[0]) - 1;
                    checkedListBox1.SetItemChecked(i, true);
                    trayRobot.GetDataVales()[id].OK = true;
                    trayRobot.GetDataVales()[id].Done = true;
                }
                if (trayRobot.OK)
                {
                    label3.Text = "OK";
                    label3.BackColor = Color.Green;
                }
                else
                {
                    label3.Text = "NG";
                    label3.BackColor = Color.Red;
                }
            }
            catch (Exception)
            {
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DebugF.DebugCompiler.Instance.DDAxis.WritDO(DebugF.DebugCompiler.Instance.RunButton.ANmen, false);

                if (DebugF.DebugCompiler.Instance.DDAxis.Out[DebugF.DebugCompiler.Instance.RunButton.ANmen])
                {
                    button4.BackColor = Color.Green;
                }
                else
                {
                    button4.BackColor = Color.Red;
                }
            }
            catch (Exception)
            {
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DebugF.IO.DODIAxis.RresOK = true;
        }

        private void SimulateTrayMesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DebugF.IO.DODIAxis.RresWait = false;
            DebugF.DebugCompiler.Buzzer = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                {
                    DebugF.DebugCompiler.FmqIS = true;
                }
                else
                {
                    DebugF.DebugCompiler.FmqIS = false;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}