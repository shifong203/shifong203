using ErosSocket.DebugPLC.Robot;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vision2.Project.formula;
using Vision2.Project.Mes;
using Vision2.vision;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.RunProgramFile.OneCompOBJs;

namespace Vision2.Project.DebugF.IO
{
    public partial class TrayDataUserControl : UserControl, ITrayRobot
    {
        public TrayDataUserControl()
        {
            InitializeComponent();
            //ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView2);
        }

        private HWindID HWi;

        public void SetTray(TrayData trayRobot)
        {
            if (trayData != trayRobot)
            {
                trayData = trayRobot;
            }
            trayData.SetITrayRobot(this);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    Initialize();
                }));
            }
            else
            {
                Initialize();
            }
        }

        public void SetTray(int trayRobot)
        {
            try
            {
                trayData = DebugCompiler.Instance.DDAxis.ListTray[trayRobot].GetTrayData();
                trayData.SetITrayRobot(this);
                this.Invoke(new Action(() =>
                {
                    Initialize();
                }));
            }
            catch (Exception)
            {
            }
        }

        public void SetMinMax(double min, double max)
        {
            MinV = min;
            MaxV = max;
        }

        public TrayRobot GetTrayEx(int number = -1)
        {
            if (number < 0)
            {
                return DebugCompiler.Instance.DDAxis.ListTray[0];
            }
            return DebugCompiler.Instance.DDAxis.ListTray[number];
        }

        private static double MinV = 00;
        private static double MaxV = 2;

        private TrayData trayData;

        public static TrayRobot GetTray(int ids = -1)
        {
            if (ids >= 0)
            {
                return DebugCompiler.Instance.DDAxis.GetTrayInxt(ids);
            }
            return null;
        }

        public void SetValue(int number, bool value, double? valueDouble = null)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    if (this.Controls.Find(number.ToString(), false).Length != 0)
                    {
                        TrayControl label1 = this.Controls.Find(number.ToString(), false)[0] as TrayControl;
                        if (label1 != null)
                        {
                            if (value)
                            {
                                label1.label1.BackColor = Color.Green;
                                if (valueDouble != null || valueDouble < MinV || valueDouble > MaxV)
                                {
                                    label1.label1.BackColor = Color.Red;
                                }
                            }
                            else
                            {
                                label1.label1.BackColor = Color.Red;
                            }
                            trayData.GetDataVales()[number - 1].OK = value;
                            label1.label1.Text = number.ToString();
                            if (trayData.GetDataVales()[number - 1].PanelID != "")
                            {
                                label1.label1.Text += "SN:" + trayData.GetDataVales()[number - 1].PanelID + ";";
                            }
                            if (valueDouble != null)
                            {
                                label1.label1.Text += "/" + valueDouble;
                            }
                            if (trayData.GetDataVales()[number - 1].MesStr != "")
                            {
                                label1.label1.Text += "Mes:" + trayData.GetDataVales()[number - 1].MesStr;
                            }
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetValue(int number, double valueDouble)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    TrayControl label1 = this.Controls.Find(number.ToString(), false)[0] as TrayControl;
                    if (trayData.GetDataVales()[number - 1] == null)
                    {
                        trayData.GetDataVales()[number - 1] = new OneDataVale();
                    }
                    if (trayData.GetDataVales()[number - 1].Data is List<double>)
                    {
                        List<double> dset = trayData.GetDataVales()[number - 1].Data as List<double>;
                        dset[0] = valueDouble;
                    }
                    if (label1 != null)
                    {
                        if (valueDouble < MinV || valueDouble > MaxV)
                        {
                            label1.label1.BackColor = Color.Red;
                            trayData.GetDataVales()[number - 1].OK = false;
                            formula.RecipeCompiler.AddOKNumber(false);
                        }
                        else
                        {
                            if (trayData.GetDataVales()[number - 1].OK)
                            {
                                trayData.GetDataVales()[number - 1].OK = true;
                                formula.RecipeCompiler.AddOKNumber(true);
                                label1.label1.BackColor = Color.Green;
                            }
                            else
                            {
                                label1.label1.BackColor = Color.Red;
                            }
                        }
                        label1.label1.Text += "/" + valueDouble;
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetValue(int number, bool value)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    TrayControl label1 = this.Controls.Find(number.ToString(), false)[0] as TrayControl;
                    if (label1 != null)
                    {
                        if (value)
                        {
                            label1.label1.BackColor = Color.Green;
                        }
                        else
                        {
                            label1.label1.BackColor = Color.Red;
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetValue(int number, string sn)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    TrayControl label1 = this.Controls.Find(number.ToString(), false)[0] as TrayControl;
                    if (label1 != null)
                    {
                        label1.label1.Text = number + "SN:" + sn;
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetValue(double valueDouble)
        {
            SetValue(trayData.Number, valueDouble);
        }

        public void SetValue(List<double> values)
        {
            try
            {
                RecipeCompiler.Instance.Data.AddData(values);
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    if (this.Controls.Find(trayData.Number.ToString(), false).Length != 0)
                    {
                        TrayControl label1 = this.Controls.Find(trayData.Number.ToString(), false)[0] as TrayControl;
                        if (trayData.GetDataVales()[trayData.Number - 1] == null)
                        {
                            trayData.GetDataVales()[trayData.Number - 1] = new OneDataVale();
                        }
                        if (label1 != null)
                        {
                            label1.label1.Text = trayData.Number.ToString() + "SN:" + trayData.GetDataVales()[trayData.Number - 1].PanelID + Environment.NewLine;
                            trayData.GetDataVales()[trayData.Number - 1].Data = values;
                            label1.Tag = trayData.GetDataVales()[trayData.Number - 1];
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetValue(List<string> values)
        {
            try
            {
                RecipeCompiler.Instance.Data.AddObj(values);
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    if (this.Controls.Find(trayData.Number.ToString(), false).Length != 0)
                    {
                        TrayControl label1 = this.Controls.Find(trayData.Number.ToString(), false)[0] as TrayControl;
                        if (trayData.GetDataVales()[trayData.Number - 1] == null)
                        {
                            trayData.GetDataVales()[trayData.Number - 1] = new OneDataVale();
                        }
                        if (label1 != null)
                        {
                            label1.label1.Text = trayData.Number.ToString() + "SN:" + trayData.GetDataVales()[trayData.Number - 1].PanelID + Environment.NewLine;
                            for (int i = 0; i < values.Count; i++)
                            {
                                //tray.GetDataVales()[tray.Number - 1].Data1.Add(new MaxMinValue()
                                //{
                                //    Value = double.Parse( values[i])
                                //});
                            }
                            bool OKt = true;
                            label1.Tag = trayData.GetDataVales()[trayData.Number - 1];
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetComponentData(int index, List<string> datStrs)
        {
            RecipeCompiler.Instance.Data.AddData(index, datStrs);
        }

        public void SetID(List<string> values)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    if (this.Controls.Find(trayData.Number.ToString(), false).Length != 0)
                    {
                        TrayControl label1 = this.Controls.Find(trayData.Number.ToString(), false)[0] as TrayControl;
                        if (trayData.GetDataVales()[trayData.Number - 1] == null)
                        {
                            trayData.GetDataVales()[trayData.Number - 1] = new OneDataVale();
                        }
                        if (label1 != null)
                        {
                            //tray.GetDataVales()[tray.Number - 1].Data1.Add(new MaxMinValue()
                            //{
                            //});
                            label1.label1.Text = trayData.Number.ToString() + "SN:" + trayData.GetDataVales()[trayData.Number - 1].PanelID;
                            label1.Tag = trayData.GetDataVales()[trayData.Number - 1];
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetValue(List<string> values, List<int> idS)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    for (int i = 0; i < idS.Count; i++)
                    {
                        if (this.Controls.Find((idS[i]).ToString(), false).Length != 0)
                        {
                            TrayControl label1 = this.Controls.Find((idS[i]).ToString(), false)[0] as TrayControl;
                            if (label1 != null)
                            {
                                if (values.Count <= i)
                                {
                                    return;
                                }
                                if (values[i] != null)
                                {
                                    label1.label1.BackColor = Color.Yellow;
                                }
                                else
                                {
                                    values[i] = "";
                                    label1.label1.BackColor = Color.Red;
                                }
                                String DAT = idS[i].ToString();
                                if (trayData.GetDataVales()[idS[i] - 1] == null)
                                {
                                    trayData.GetDataVales()[idS[i] - 1] = new OneDataVale();
                                }
                                trayData.GetDataVales()[idS[i] - 1].PanelID = values[i];

                                if (trayData.GetDataVales()[idS[i] - 1] != null)
                                {
                                    if (trayData.GetDataVales()[idS[i] - 1].PanelID != null)
                                    {
                                        DAT += "SN:" + trayData.GetDataVales()[idS[i] - 1].PanelID + Environment.NewLine;
                                        //tray.dataObjs[idS[i] - 1].OK = false;
                                    }
                                    else
                                    {
                                        trayData.GetDataVales()[idS[i] - 1].OK = false;
                                    }
                                    if (trayData.GetDataVales()[idS[i] - 1].Data != null)
                                    {
                                        DAT += "数据:" + trayData.GetDataVales()[idS[i] - 1].Data;
                                    }
                                    label1.label1.Text = DAT;
                                }
                                else
                                {
                                    trayData.GetDataVales()[idS[i] - 1].OK = false;
                                }
                                label1.Tag = trayData.GetDataVales()[idS[i] - 1];
                            }
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetPanleSN(List<string> listSN, List<int> tryaid)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    try
                    {
                        for (int i = 0; i < tryaid.Count; i++)
                        {
                            if (this.Controls.Find((tryaid[i]).ToString(), false).Length != 0)
                            {
                                TrayControl label1 = this.Controls.Find((tryaid[i]).ToString(), false)[0] as TrayControl;
                                if (label1 != null)
                                {
                                    if (listSN.Count <= i)
                                    {
                                        return;
                                    }
                                    string dataSn = listSN[i];
                                    if (dataSn == null)
                                    {
                                        dataSn = "";
                                        label1.label1.BackColor = Color.Red;
                                        trayData.GetDataVales()[tryaid[i] - 1].AddNG("SN未识别");
                                    }
                                    else if (dataSn == "")
                                    {
                                        dataSn = "";
                                        label1.label1.BackColor = Color.Red;
                                        trayData.GetDataVales()[tryaid[i] - 1].AddNG("SN未识别");
                                    }
                                    else
                                    {
                                        trayData.GetDataVales()[tryaid[i] - 1].NotNull = true;
                                        label1.label1.BackColor = Color.Yellow;
                                    }
                                    String DAT = tryaid[i].ToString();
                                    if (trayData.GetDataVales()[tryaid[i] - 1] == null)
                                    {
                                        trayData.GetDataVales()[tryaid[i] - 1] = new OneDataVale();
                                    }
                                    trayData.GetDataVales()[tryaid[i] - 1].PanelID = dataSn;

                                    if (trayData.GetDataVales()[tryaid[i] - 1] != null)
                                    {
                                        if (dataSn != null)
                                        {
                                            DAT += "SN:" + dataSn + Environment.NewLine;
                                            //tray.dataObjs[idS[i] - 1].OK = false;
                                        }
                                        else
                                        {
                                            trayData.GetDataVales()[tryaid[i] - 1].OK = false;
                                            trayData.GetDataVales()[tryaid[i] - 1].OK = false;
                                        }
                                        if (trayData.GetDataVales()[tryaid[i] - 1].Data != null)
                                        {
                                            DAT += "数据:" + trayData.GetDataVales()[tryaid[i] - 1].Data;
                                        }
                                        label1.label1.Text = DAT;
                                    }
                                    else
                                    {
                                        trayData.GetDataVales()[tryaid[i] - 1].OK = false;
                                    }
                                    label1.Tag = trayData.GetDataVales()[tryaid[i] - 1];
                                }
                            }
                            if (tryaid[i] == 0)
                            {
                                trayData.TrayIDQR = listSN[i];
                                GetTexup();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetValue(List<bool> values)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    for (int i = 0; i < values.Count; i++)
                    {
                        try
                        {
                            OneDataVale data = trayData.GetDataVales()[i];
                            if (data != null)
                            {
                                data.OK = values[i];
                                Control[] controls = this.Controls.Find(data.TrayLocation.ToString(), false);
                                if (controls.Length != 0)
                                {
                                    TrayControl label1 = controls[0] as TrayControl;
                                    if (label1 != null)
                                    {
                                        if (data.NotNull)
                                        {
                                            if (data.OK)
                                            {
                                                label1.BackColor = Color.Green;
                                            }
                                            else
                                            {
                                                label1.BackColor = Color.Red;
                                            }
                                        }
                                        string datStr = (i + 1).ToString();
                                        if (data.PanelID != null && data.PanelID.Length != 0)
                                        {
                                            if (data.PanelID.Length >= 6)
                                            {
                                                datStr += "SN:" + data.PanelID.Substring(data.PanelID.Length - 6);
                                            }
                                            else
                                            {
                                                datStr += "SN:" + data.PanelID;
                                            }
                                        }
                                        label1.Text = datStr;
                                        label1.Tag = data;
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        { }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetNumber(int number)
        {
            trayData.Number = number;
        }

        public void RestValue(TrayData trayDa)
        {
            if (trayData != null)
            {
                trayData = trayDa;
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    TrayControl control = this.Controls[i] as TrayControl;
                    if (control != null)
                    {
                        control.checkBox1.Checked = false;
                        //control.BackColor = Color.White;
                        control.label1.Text = control.label1.Name;
                        control.label1.BackColor = Color.White;
                        control.Tag = null;
                    }
                }
                GetTexup();
            }
        }

        public void GetTexup()
        {
            try
            {
                label1.Text = trayData.GetTrayString();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 初始化矩阵
        /// </summary>
        public void Initialize(TrayData trayDa = null)
        {
            try
            {
                if (trayDa != null)
                {
                    trayData = trayDa;
                }
                int rows = trayData.YNumber;
                int columnCount = trayData.XNumber;
                // 动态添加一行
                this.AutoScroll = true;
                if (rows == 0)
                {
                    return;
                }
                if (columnCount == 0)
                {
                    return;
                }
                this.Controls.Clear();
                this.Controls.Add(label1);
                if (trayData.GetDataVales() == null)
                {
                    trayData.Clear();
                }
                GetTexup();
                int heit, Wita;
                heit = (this.Height - 20) / (rows);
                Wita = (this.Width - 20) / (columnCount);
                if (heit < 20)
                {
                    heit = 20;
                }
                if (Wita < 30)
                {
                    Wita = 30;
                }
                for (int i = 0; i < rows * columnCount; i++)
                {
                    TrayControl trayControl = new TrayControl();
                    int sd = 0;
                    int dt = 0;
                    if (trayData.HorizontallyORvertically)
                    {
                        sd = i / rows;
                        dt = i % rows;
                        switch (trayData.TrayDirection)
                        {
                            case TrayRobot.TrayDirectionEnum.左上:
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = (dt * columnCount + sd + 1).ToString();
                                break;

                            case TrayRobot.TrayDirectionEnum.右上:
                                //trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = ((rows * (sd + 1)) - dt).ToString();
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = (rows * columnCount - (columnCount * (dt + 1)) + sd + 1).ToString();
                                break;

                            case TrayRobot.TrayDirectionEnum.左下:
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = ((dt + 1) * columnCount - sd).ToString();
                                break;

                            case TrayRobot.TrayDirectionEnum.右下:
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = (rows * columnCount - (dt) * columnCount - sd).ToString();
                                break;
                        }
                    }
                    else
                    {
                        sd = i / columnCount;
                        dt = i % columnCount;
                        switch (trayData.TrayDirection)
                        {
                            case TrayRobot.TrayDirectionEnum.左上:
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = (i + 1).ToString();
                                break;

                            case TrayRobot.TrayDirectionEnum.右上:
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = ((columnCount * (sd + 1)) - dt).ToString();
                                break;

                            case TrayRobot.TrayDirectionEnum.左下:

                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = (rows * columnCount - (columnCount * (sd + 1)) + dt + 1).ToString();
                                break;

                            case TrayRobot.TrayDirectionEnum.右下:
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = (rows * columnCount - i).ToString();
                                break;
                        }
                    }

                    trayControl.Height = heit;
                    trayControl.Width = Wita;
                    trayControl.Location = new Point(new Size(trayControl.Width * dt, trayControl.Height * sd + label1.Height));
                    trayControl.label1.MouseDown += Label1_MouseDown;
                    trayControl.label1.MouseUp += Label1_MouseMove; ;
                    this.Controls.Add(trayControl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.PadRight(30, ' '), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Label1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void Label1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    return;
                }

                Control control = sender as Label;
                if (control == null)
                {
                    return;
                }
                dataGridView2.Visible = false;
                OneDataVale dataObj = control.Parent.Tag as OneDataVale;
                if (dataObj != null)
                {
                    if (!panel1.Visible)
                    {
                        panel1.Location = new Point(100, 100);
                    }
                    panel1.Visible = true;

                    if (!this.Controls.Contains(panel1))
                    {
                        this.Controls.Add(panel1);
                        panel1.BringToFront();
                    }
                    toolStripLabel1.Text = "N:" + control.Name + ";SN:";
                    toolStripTextBox1.Text = dataObj.PanelID;
                    if (dataObj != null)
                    {
                        //dataGridView1.Rows.Clear();
                        treeView1.Nodes.Clear();
                        if (dataObj.ListCamsData.Count == 0)
                        {
                            tabControl1.TabPages.Remove(tabPage2);
                        }
                        else
                        {
                            if (!tabControl1.TabPages.Contains(tabPage2))
                            {
                                tabControl1.TabPages.Add(tabPage2);
                            }
                        }
                        if (dataObj.GetNGImage() != null)
                        {
                            if (!dataObj.GetNGImage().IsInitialized())
                            {
                                tabControl1.TabPages.Remove(tabPage1);
                            }
                            else
                            {
                                if (!tabControl1.TabPages.Contains(tabPage1))
                                {
                                    tabControl1.TabPages.Add(tabPage1);
                                }
                            }
                        }
                        else
                        {
                            tabControl1.TabPages.Remove(tabPage1);
                        }

                        if (tabControl1.TabPages.Count == 0)
                        {
                            this.panel1.Height = 100;
                        }
                        else
                        {
                            this.panel1.Height = 600;
                        }

                        foreach (var item in dataObj.ListCamsData)
                        {
                            TreeNode treeNode = treeView1.Nodes.Add(item.Key);
                            treeNode.Tag = item.Value;
                            treeNode.ImageIndex = 0;
                            dataGridView2.Rows.Clear();
                            int index = 0;
                            foreach (var itemd in item.Value.NGObj.DicOnes)
                            {
                                DataMinMax da = itemd.Value.oneRObjs[0].dataMinMax;
                                if (da != null)
                                {
                                    if (da.Reference_Name.Count == 0)
                                    {
                                        continue;
                                    }
                                    dataGridView2.Rows.Add(da.Reference_Name.Count);
                                    for (int i = 0; i < da.Reference_Name.Count; i++)
                                    {
                                        dataGridView2.Rows[index].Cells[0].Value = itemd.Key + "." + da.Reference_Name[i];
                                        if (da.ValueStrs.Count > i)
                                        {
                                            dataGridView2.Rows[index].Cells[1].Value = da.ValueStrs[i];
                                        }
                                        dataGridView2.Rows[index].Cells[2].Value = da.Reference_ValueMin[i];
                                        dataGridView2.Rows[index].Cells[3].Value = da.Reference_ValueMax[i];
                                        index++;
                                    }
                                    if (da.ValueStrs.Count > dataGridView2.Rows.Count)
                                    {
                                        dataGridView2.Rows.Add(da.ValueStrs.Count - da.Reference_Name.Count);
                                        for (int i = da.Reference_Name.Count; i < da.ValueStrs.Count; i++)
                                        {
                                            dataGridView2.Rows[index].Cells[1].Value = da.ValueStrs[i];
                                            index++;
                                        }
                                    }
                                }
                            }

                            dataGridView2.Dock = DockStyle.Fill;
                            dataGridView2.Visible = true;
                            dataGridView2.BringToFront();
                            dataGridView2.Show();
                            foreach (var itemd in item.Value.NGObj.DicOnes)
                            {
                                TreeNode treeNode1 = treeNode.Nodes.Add(itemd.Key);
                                if (itemd.Value.aOK)
                                {
                                    treeNode1.ImageIndex = 7;
                                }
                                else
                                {
                                    treeNode1.ImageIndex = 2;
                                }
                                treeNode1.Tag = itemd.Value;
                            }

                            foreach (var itemd in item.Value.GetAllCompOBJs().DicOnes)
                            {
                                TreeNode treeNode1 = treeNode.Nodes.Add(itemd.Key);
                                if (itemd.Value.aOK)
                                {
                                    treeNode1.ImageIndex = 7;
                                }
                                else
                                {
                                    treeNode1.ImageIndex = 2;
                                }

                                treeNode1.Tag = itemd.Value;
                            }

                            treeNode.Expand();
                        }
                        foreach (var item in dataObj.ListCamsData)
                        {
                            HWi.SetImaage(item.Value.ResuOBj[0].Image);
                            HWi.OneResIamge = item.Value.ResuOBj[0];
                            HWi.ShowImage();
                            break;
                        }
                        if (DebugCompiler.Instance.IsImage)
                        {
                            tabControl1.SelectedTab = tabPage1;
                        }
                        else
                        {
                            tabControl1.SelectedTab = tabPage2;
                        }
                    }
                }
                if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.运行中)
                {
                    return;
                }
                HalconRun halcon = Vision.GetRunNameVision();
                if (halcon != null)
                {
                    HWindowControl controlH = halcon.GetHWindow().GetNmaeWindowControl("Image." + control.Text);
                    if (controlH != null)
                    {
                        halcon.HobjClear();
                        OneResultOBj halconResult = controlH.Tag as OneResultOBj;
                        halcon.ShowImage(halconResult.Image);
                        halcon.SetResultOBj(halconResult);
                        halcon.GetOneImageR().ShowAll(halcon.hWindowHalcon());
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panel1.Visible)
            {
                panel1.Visible = false;
                return;
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
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

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            MoveFlag = false;
        }

        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (MoveFlag)
                {
                    panel1.Left += Convert.ToInt16(e.X - xPos);//设置x坐标.
                    panel1.Top += Convert.ToInt16(e.Y - yPos);//设置y坐标.
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (panel1.Visible)
            {
                panel1.Visible = false;
                return;
            }
        }

        public void SetValue(int number, OneDataVale dataVale)
        {
            try
            {
                //tray.Number = number+1;
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    string datStr = "";
                    if (this.Controls.Find(number.ToString(), false).Length != 0)
                    {
                        TrayControl label1 = this.Controls.Find(number.ToString(), false)[0] as TrayControl;
                        if (label1 != null)
                        {
                            if (dataVale.OK)
                            {
                                label1.label1.BackColor = Color.Green;
                            }
                            else
                            {
                                label1.label1.BackColor = Color.Red;
                            }
                            if (trayData.GetDataVales()[number - 1] == null)
                            {
                                trayData.GetDataVales()[number - 1] = new OneDataVale();
                            }
                            trayData.GetDataVales()[number - 1].PanelID = dataVale.PanelID;
                            trayData.GetDataVales()[number - 1] = dataVale;
                            if (trayData.GetDataVales()[number - 1] != null)
                            {
                                if (trayData.GetDataVales()[number - 1].PanelID != null)
                                {
                                    datStr += trayData.GetDataVales()[number - 1].TrayLocation + "SN:" + trayData.GetDataVales()[number - 1].PanelID + Environment.NewLine;
                                }
                                bool ok = true;
                                //foreach (var item in dataVale.DataMin_Max)
                                //{
                                //    if (!item.Value.GetRsetOK())
                                //    {
                                //        datStr += item.Key+";";
                                //       ok = false;
                                //    }
                                //}
                                //if (ok)
                                //{
                                //  datStr += "数据ok;";
                                //}

                                label1.label1.Text = datStr;
                            }
                            else
                            {
                                trayData.GetDataVales()[number - 1].OK = false;
                            }
                            label1.Tag = trayData.GetDataVales()[number - 1];
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TrayDataUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                HWi = new vision.HWindID();
                HWi.Initialize(hWindowControl1);
            }
            catch (Exception)
            {
            }
        }

        private bool fILE;

        private void tsButton1_Click(object sender, EventArgs e)
        {
            if (fILE)
            {
                fILE = false;
                panel1.Location = new Point(0, 0);
                panel1.Height = this.Height;
                double dt = HWi.WidthImage / HWi.HeigthImage;

                panel1.Width = (int)(this.Height * dt);
            }
            else
            {
                fILE = true;
                panel1.Width = 800;
                panel1.Height = 500;
            }
        }

        public void SetValue(int number, TrayData dataVale)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    for (int i = 0; i < dataVale.GetDataVales().Count; i++)
                    {
                        string datStr = "";
                        OneDataVale data = dataVale.GetDataVales()[i];
                        if (data != null)
                        {
                            if (data.TrayLocation == 0)
                            {
                                continue;
                            }
                            if (this.Controls.Find(data.TrayLocation.ToString(), false).Length == 1)
                            {
                                TrayControl label1 = this.Controls.Find(data.TrayLocation.ToString(), false)[0] as TrayControl;
                                if (label1 != null)
                                {
                                    if (data.NotNull)
                                    {
                                        if (data.OK)
                                        {
                                            label1.label1.BackColor = Color.Green;
                                        }
                                        else
                                        {
                                            label1.label1.BackColor = Color.Red;
                                        }
                                    }
                                    datStr = data.TrayLocation.ToString();
                                    if (data.PanelID != "")
                                    {
                                        datStr += "SN:" + data.PanelID + Environment.NewLine;
                                    }

                                    label1.label1.Text = datStr;

                                    label1.Tag = data;
                                }
                            }
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //        try
        //        {
        //            dataGridView2.Visible = true;
        //            dataGridView2.Location = new Point(150, 100);
        //            dataGridView2.Rows.Clear();
        //            DataMinMax da = dataGridView1.Rows[e.RowIndex].Cells[0].Tag as DataMinMax;
        //            if (da!=null)
        //            {
        //            if (da.Reference_Name.Count==0)
        //            {
        //                return;
        //            }
        //                dataGridView2.Rows.Add(da.Reference_Name.Count);
        //                for (int i = 0; i < da.Reference_Name.Count; i++)
        //                {
        //                    dataGridView2.Rows[i].Cells[0].Value = da.Reference_Name[i];
        //                    if (da.ValueStrs.Count>i)
        //                    {
        //                        dataGridView2.Rows[i].Cells[1].Value = da.ValueStrs[i];
        //                    }
        //                    dataGridView2.Rows[i].Cells[2].Value = da.Reference_ValueMin[i];
        //                    dataGridView2.Rows[i].Cells[3].Value = da.Reference_ValueMax[i];
        //                }
        //                if (da.ValueStrs.Count > dataGridView2.Rows.Count)
        //                {
        //                   dataGridView2.Rows.Add(da.ValueStrs.Count -da.Reference_Name.Count);
        //                    for (int i = da.Reference_Name.Count; i < da.ValueStrs.Count; i++)
        //                    {
        //                        dataGridView2.Rows[i].Cells[1].Value = da.ValueStrs[i];
        //                    }
        //                 }
        //            }
        //            dataGridView2.BringToFront();
        //            dataGridView2.Show();
        //        }
        //        catch (Exception)
        //        {
        //        }

        //}

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //dataGridView2.Visible = false;
            //timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Visible = true;
                dataGridView2.Location = new Point(200, 100);
                dataGridView2.BringToFront();
                dataGridView2.Show();
            }
            catch (Exception)
            {
            }
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //Point ClickPoint = new Point(e.X, e.Y);
                //TreeNode CurrentNode = treeView1.GetNodeAt(ClickPoint);
                //if (CurrentNode != null)
                //{
                //    if (CurrentNode.Tag is OneCamData)
                //    {
                //        OneCamData oneCamData = CurrentNode.Tag as OneCamData;
                //    }
                //    else if (CurrentNode.Tag is OneComponent)
                //    {
                //        OneComponent oneCamData = CurrentNode.Tag as OneComponent;
                //        dataGridView2.Visible = true;
                //        dataGridView2.Location = new Point(150, 100);
                //        dataGridView2.Rows.Clear();
                //        DataMinMax da = oneCamData.oneRObjs[0].dataMinMax;
                //        if (da != null)
                //        {
                //            if (da.Reference_Name.Count == 0)
                //            {
                //                return;
                //            }
                //            dataGridView2.Rows.Add(da.Reference_Name.Count);
                //            for (int i = 0; i < da.Reference_Name.Count; i++)
                //            {
                //                dataGridView2.Rows[i].Cells[0].Value = da.Reference_Name[i];
                //                if (da.ValueStrs.Count > i)
                //                {
                //                    dataGridView2.Rows[i].Cells[1].Value = da.ValueStrs[i];
                //                }
                //                dataGridView2.Rows[i].Cells[2].Value = da.Reference_ValueMin[i];
                //                dataGridView2.Rows[i].Cells[3].Value = da.Reference_ValueMax[i];
                //            }
                //            if (da.ValueStrs.Count > dataGridView2.Rows.Count)
                //            {
                //                dataGridView2.Rows.Add(da.ValueStrs.Count - da.Reference_Name.Count);
                //                for (int i = da.Reference_Name.Count; i < da.ValueStrs.Count; i++)
                //                {
                //                    dataGridView2.Rows[i].Cells[1].Value = da.ValueStrs[i];
                //                }
                //            }
                //        }
                //        dataGridView2.BringToFront();
                //        dataGridView2.Show();

                //    }

                //}
            }
            catch (Exception)
            {
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode CurrentNode = e.Node;
                if (CurrentNode != null)
                {
                    dataGridView2.Dock = DockStyle.Fill;
                    if (CurrentNode.Tag is OneCamData)
                    {
                        dataGridView2.Rows.Clear();
                        OneCamData oneCamData = CurrentNode.Tag as OneCamData;
                        int index = 0;
                        foreach (var itemd in oneCamData.NGObj.DicOnes)
                        {
                            DataMinMax da = itemd.Value.oneRObjs[0].dataMinMax;
                            if (da != null)
                            {
                                if (da.Reference_Name.Count == 0)
                                {
                                    continue;
                                }
                                dataGridView2.Rows.Add(da.Reference_Name.Count);
                                for (int i = 0; i < da.Reference_Name.Count; i++)
                                {
                                    dataGridView2.Rows[index].Cells[0].Value = itemd.Key + "." + da.Reference_Name[i];
                                    if (da.ValueStrs.Count > i)
                                    {
                                        dataGridView2.Rows[index].Cells[1].Value = da.ValueStrs[i];
                                    }
                                    dataGridView2.Rows[index].Cells[2].Value = da.Reference_ValueMin[i];
                                    dataGridView2.Rows[index].Cells[3].Value = da.Reference_ValueMax[i];
                                    index++;
                                }
                                if (da.ValueStrs.Count > dataGridView2.Rows.Count)
                                {
                                    dataGridView2.Rows.Add(da.ValueStrs.Count - da.Reference_Name.Count);
                                    for (int i = da.Reference_Name.Count; i < da.ValueStrs.Count; i++)
                                    {
                                        dataGridView2.Rows[index].Cells[1].Value = da.ValueStrs[i];
                                        index++;
                                    }
                                }
                            }
                            dataGridView2.BringToFront();
                            dataGridView2.Show();
                        }
                    }
                    else if (CurrentNode.Tag is OneComponent)
                    {
                        OneComponent oneCamData = CurrentNode.Tag as OneComponent;
                        dataGridView2.Visible = true;
                        dataGridView2.Location = new Point(150, 100);
                        dataGridView2.Dock = DockStyle.Fill;
                        DataMinMax da = oneCamData.oneRObjs[0].dataMinMax;
                        if (da != null)
                        {
                            if (da.Reference_Name.Count == 0)
                            {
                                return;
                            }
                            dataGridView2.Rows.Add(da.Reference_Name.Count);
                            for (int i = 0; i < da.Reference_Name.Count; i++)
                            {
                                dataGridView2.Rows[i].Cells[0].Value = da.Reference_Name[i];
                                if (da.ValueStrs.Count > i)
                                {
                                    dataGridView2.Rows[i].Cells[1].Value = da.ValueStrs[i];
                                }
                                dataGridView2.Rows[i].Cells[2].Value = da.Reference_ValueMin[i];
                                dataGridView2.Rows[i].Cells[3].Value = da.Reference_ValueMax[i];
                            }
                            if (da.ValueStrs.Count > dataGridView2.Rows.Count)
                            {
                                dataGridView2.Rows.Add(da.ValueStrs.Count - da.Reference_Name.Count);
                                for (int i = da.Reference_Name.Count; i < da.ValueStrs.Count; i++)
                                {
                                    dataGridView2.Rows[i].Cells[1].Value = da.ValueStrs[i];
                                }
                            }
                        }
                        dataGridView2.BringToFront();
                        dataGridView2.Show();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        //public void SetTray(TrayData tray)
        //{
        //    //trayData.GetTrayData( tray);
        //}

        public void UpData()
        {
            for (int i = 0; i < trayData.Count; i++)
            {
                if (trayData.GetDataVales()[i] != null)
                {
                    if (trayData.GetDataVales()[i].NotNull)
                    {
                        SetValue(i + 1, trayData.GetDataVales()[i].OK);
                    }
                }
                else
                {
                    SetValue(i + 1, 0);
                }
            }
        }

        public void SetValue(bool listValue)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    for (int i = 0; i < trayData.Count; i++)
                    {
                        try
                        {
                            OneDataVale data = trayData.GetDataVales()[i];
                            if (data != null)
                            {
                                Control[] controls = this.Controls.Find(data.TrayLocation.ToString(), false);
                                if (controls.Length != 0)
                                {
                                    TrayControl label1 = controls[0] as TrayControl;
                                    if (label1 != null)
                                    {
                                        if (data.NotNull)
                                        {
                                            if (data.OK)
                                            {
                                                label1.label1.BackColor = Color.Green;
                                            }
                                            else
                                            {
                                                label1.label1.BackColor = Color.Red;
                                            }
                                        }
                                        else
                                        {
                                            label1.label1.BackColor = Color.Red;
                                        }
                                        string datStr = (i + 1).ToString();
                                        if (data.PanelID != null && data.PanelID.Length != 0)
                                        {
                                            datStr += "SN:" + data.PanelID;
                                        }
                                        label1.label1.Text = datStr+data.MesStr;
                                        label1.Tag = data;
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        { }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetValue(int number, int errNuber)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    Control[] controls = Controls.Find(number.ToString(), false);
                    OneDataVale data = trayData.GetDataVales()[number - 1];
                    if (controls.Length != 0)
                    {
                        TrayControl label1 = controls[0] as TrayControl;
                        if (label1 != null)
                        {
                            label1.Tag = data;
                            switch (errNuber)
                            {
                                case -1:
                                    label1.label1.BackColor = Color.Black;
                                    break;

                                case 0:
                                    data.NotNull = false;
                                    label1.label1.BackColor = Color.Orange;
                                    break;

                                case 1:
                                    data.OK = true;
                                    label1.label1.BackColor = Color.Green;
                                    break;

                                case 2:
                                    //data.OK = false;
                                    label1.label1.BackColor = Color.Red;
                                    break;

                                case 3:
                                    //data.OK = false;
                                    label1.label1.BackColor = Color.Yellow;
                                    break;

                                case 4:
                                    //data.OK = false;
                                    label1.label1.BackColor = Color.Blue;
                                    break;

                                case 5:
                                    data.OK = false;
                                    label1.label1.BackColor = Color.Pink;
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}