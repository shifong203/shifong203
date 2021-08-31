using ErosSocket.DebugPLC.Robot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.ToolStrip;
using Vision2.Project.Mes;

namespace Vision2.Project.DebugF.IO
{
    public partial class TrayDatas : UserControl, ITrayRobot
    {
        /// <summary>
        ///
        /// </summary>
        public TrayDatas()
        {
            InitializeComponent();
        }

        //public void SetTray(TrayData tray)
        //{
        //    this.tray = tray;
        //}
        private TrayData tray;

        /// <summary>
        /// 复位矩阵
        /// </summary>
        public void RestValue(TrayData trayData)
        {
            if (tray != null)
            {
                tray = trayData;
                label1.Text = tray.GetTrayString();
                //tray.GetDataVales().AddRange(new Mes.DataVale [tray.Count]);
                for (int i = 0; i < panel1.Controls.Count; i++)
                {
                    Label control = panel1.Controls[i] as Label;
                    if (control != null)
                    {
                        control.Text = control.Name;
                        control.BackColor = Color.White;
                        control.Tag = null;
                    }
                }
            }
        }

        public void UpData()
        {
            label1.Text = tray.GetTrayString();
            for (int i = 0; i < tray.Count; i++)
            {
                if (tray.GetDataVales()[i] != null)
                {
                    if (tray.GetDataVales()[i].NotNull)
                    {
                        SetValue(i + 1, tray.GetDataVales()[i].OK);
                    }
                }
                else
                {
                    SetValue(i + 1, 0);
                }
            }
        }

        public void SetValue(int id, bool ok)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    Label label1 = panel1.Controls.Find(id.ToString(), false)[0] as Label;
                    if (label1 != null)
                    {
                        if (ok)
                        {
                            label1.BackColor = Color.Green;
                        }
                        else
                        {
                            label1.BackColor = Color.Red;
                        }
                        if (tray.GetDataVales()[id - 1] != null)
                        {
                            label1.Text = tray.GetDataVales()[id - 1].TrayLocation.ToString();
                            label1.Text += tray.GetDataVales()[id - 1].Done.ToString();
                            if (tray.GetDataVales()[id - 1].PanelID.Length >= 6)
                            {
                                label1.Text += "SN:" + tray.GetDataVales()[id - 1].PanelID.Substring(tray.GetDataVales()[id - 1].PanelID.Length - 6);
                            }
                            else
                            {
                                label1.Text += "SN:" + tray.GetDataVales()[id - 1].PanelID;
                            }
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetValue(int id, byte byteVae)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    Label label1 = panel1.Controls.Find(id.ToString(), false)[0] as Label;
                    if (label1 != null)
                    {
                        if (byteVae == 1)
                        {
                            label1.BackColor = Color.Green;
                        }
                        else if (byteVae == 0)
                        {
                            label1.BackColor = Color.White;
                        }
                        else
                        {
                            label1.BackColor = Color.Red;
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 初始化矩阵
        /// </summary>
        /// <param name="tray"></param>
        public void Initialize(TrayData tr = null)
        {
            try
            {
                if (tr != null)
                {
                    tray = tr;
                }
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    panel1.Controls.Clear();

                    if (tray == null)
                    {
                        return;
                    }

                    int rows = tray.YNumber;
                    int columnCount = tray.XNumber;
                    // 动态添加一行
                    if (rows == 0)
                    {
                        return;
                    }
                    if (columnCount == 0)
                    {
                        return;
                    }

                    if (tray.GetDataVales() == null)
                    {
                        tray.Clear();
                    }
                    int heit, Wita;
                    heit = panel1.Height / (rows);
                    Wita = panel1.Width / (columnCount);
                    for (int i = 0; i < rows * columnCount; i++)
                    {
                        LabelEx trayControl = new LabelEx();
                        trayControl.AutoSize = false;
                        trayControl.BorderStyle = BorderStyle.FixedSingle;
                        trayControl.TextAlign = ContentAlignment.MiddleCenter;
                        trayControl.Font = new Font("宋体", 15F, FontStyle.Regular,
                            GraphicsUnit.Point, ((byte)(134)));
                        int sd = 0;
                        int dt = 0;
                        if (tray.HorizontallyORvertically)
                        {
                            heit = panel1.Height / (columnCount);
                            Wita = panel1.Width / (rows);
                            sd = i / rows;
                            dt = i % rows;
                            switch (tray.TrayDirection)
                            {
                                case TrayRobot.TrayDirectionEnum.左上:
                                    trayControl.Name = trayControl.Name = trayControl.Text = (dt * columnCount + sd + 1).ToString();
                                    break;

                                case TrayRobot.TrayDirectionEnum.右上:
                                    //trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = ((rows * (sd + 1)) - dt).ToString();
                                    trayControl.Name = trayControl.Name = trayControl.Text = (rows * columnCount - (columnCount * (dt + 1)) + sd + 1).ToString();
                                    break;

                                case TrayRobot.TrayDirectionEnum.左下:
                                    trayControl.Name = trayControl.Name = trayControl.Text = ((dt + 1) * columnCount - sd).ToString();
                                    break;

                                case TrayRobot.TrayDirectionEnum.右下:
                                    trayControl.Name = trayControl.Name = trayControl.Text = (rows * columnCount - (dt) * columnCount - sd).ToString();
                                    break;
                            }
                        }
                        else
                        {
                            sd = i / columnCount;
                            dt = i % columnCount;
                            switch (tray.TrayDirection)
                            {
                                case TrayRobot.TrayDirectionEnum.左上:
                                    trayControl.Name = trayControl.Name = trayControl.Text = (i + 1).ToString();
                                    break;

                                case TrayRobot.TrayDirectionEnum.右上:
                                    trayControl.Name = trayControl.Name = trayControl.Text = ((columnCount * (sd + 1)) - dt).ToString();
                                    break;

                                case TrayRobot.TrayDirectionEnum.左下:

                                    trayControl.Name = trayControl.Name = trayControl.Text = (rows * columnCount - (columnCount * (sd + 1)) + dt + 1).ToString();
                                    break;

                                case TrayRobot.TrayDirectionEnum.右下:
                                    trayControl.Name = trayControl.Name = trayControl.Text = (rows * columnCount - i).ToString();
                                    break;
                            }
                        }
                        trayControl.Height = heit;
                        trayControl.Width = Wita;
                        trayControl.Location = new Point(new Size(trayControl.Width * dt, trayControl.Height * sd));
                        trayControl.MouseDown += TrayControl_MouseDown;
                        trayControl.MouseUp += TrayControl_MouseUp;
                        panel1.Controls.Add(trayControl);
                    }
                    label1.Text = tray.GetTrayString();
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.PadRight(30, ' '), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TrayControl_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Label label = sender as Label;

                if (label != null)
                {
                    OneDataVale dataVale = label.Tag as OneDataVale;

                    if (dataVale != null)
                    {
                        toolTip1.ToolTipTitle = "托盘" + tray.Number + tray.TrayIDQR;
                        toolTip1.Show(dataVale.PanelID, label);
                        if (dataVale.ListCamsData.Count != 0)
                        {
                            vision.Vision.GetRunNameVision().GetOneImageR(
                   dataVale.ListCamsData[vision.Vision.GetRunNameVision().Name].ResuOBj[0]);
                            vision.Vision.GetRunNameVision().ShowObj();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void TrayControl_MouseDown(object sender, MouseEventArgs e)
        {
        }

        public void SetValue(int number, bool value, double? valueDouble = null)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    if (panel1.Controls.Find(number.ToString(), false).Length != 0)
                    {
                        Label label1 = panel1.Controls.Find(number.ToString(), false)[0] as Label;
                        if (label1 != null)
                        {
                            if (value)
                            {
                                label1.BackColor = Color.Green;
                            }
                            else
                            {
                                label1.BackColor = Color.Red;
                            }
                            label1.Text = number.ToString();
                            if (tray.GetDataVales()[number - 1] != null)
                            {
                                label1.Text += "SN:" + tray.GetDataVales()[number - 1].PanelID.Substring(tray.GetDataVales()[number - 1].PanelID.Length - 6);
                            }
                            if (valueDouble != null)
                            {
                                label1.Text += "/" + valueDouble;
                            }
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetValue(int number, OneDataVale dataVale)
        {
            try
            {
                //Tray.Number = number + 1;
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    string datStr = dataVale.TrayLocation.ToString();
                    if (panel1.Controls.Find(number.ToString(), false).Length != 0)
                    {
                        Label label1 = panel1.Controls.Find(number.ToString(), false)[0] as Label;
                        if (label1 != null)
                        {
                            if (dataVale.OK)
                            {
                                label1.BackColor = Color.Green;
                            }
                            else
                            {
                                label1.BackColor = Color.Red;
                            }
                            if (tray.GetDataVales()[number - 1] == null)
                            {
                                tray.GetDataVales()[number - 1] = new OneDataVale();
                            }
                            if (tray.GetDataVales()[number - 1] != null)
                            {
                                if (tray.GetDataVales()[number - 1].PanelID != null)
                                {
                                    if (tray.GetDataVales()[number - 1].PanelID.Length > 6)
                                    {
                                        datStr += "SN:" + tray.GetDataVales()[number - 1].PanelID.Substring(tray.GetDataVales()[number - 1].PanelID.Length - 6);
                                    }
                                }
                                label1.Text = datStr;
                            }
                            else
                            {
                                tray.GetDataVales()[number - 1].OK = false;
                            }
                            label1.Tag = tray.GetDataVales()[number - 1];
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetValue(int number, TrayData dataVale)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    for (int i = 0; i < dataVale.Count; i++)
                    {
                        string datStr = "";
                        OneDataVale data = dataVale.GetDataVales()[i];
                        if (data != null)
                        {
                            Control[] controls = panel1.Controls.Find(data.TrayLocation.ToString(), false);
                            if (controls.Length != 0)
                            {
                                Label label1 = controls[0] as Label;
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
                                    datStr = (i + 1).ToString();
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
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetPanleSN(List<string> listSN, List<int> tryaid)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    for (int i = 0; i < tryaid.Count; i++)
                    {
                        if (tryaid[i] == 0)
                        {
                            tray.TrayIDQR = listSN[i];
                        }
                        if (panel1.Controls.Find((tryaid[i]).ToString(), false).Length != 0)
                        {
                            Label label1 = panel1.Controls.Find((tryaid[i]).ToString(), false)[0] as Label;
                            if (label1 != null)
                            {
                                if (listSN.Count <= i)
                                {
                                    return;
                                }
                                if (listSN[i] == null || listSN[i] == "")
                                {
                                    label1.BackColor = Color.Red;
                                }
                                else
                                {
                                    label1.BackColor = Color.Yellow;
                                    //listSN[i] = "";
                                }
                                String DAT = tryaid[i].ToString();
                                if (tray.GetDataVales()[tryaid[i] - 1] == null)
                                {
                                    tray.GetDataVales()[tryaid[i] - 1] = new OneDataVale();
                                }
                                tray.GetDataVales()[tryaid[i] - 1].PanelID = listSN[i];
                                if (listSN[i] != "")
                                {
                                    tray.GetDataVales()[tryaid[i] - 1].NotNull = true;
                                }
                                if (tray.GetDataVales()[tryaid[i] - 1] != null)
                                {
                                    if (tray.GetDataVales()[tryaid[i] - 1].PanelID != null)
                                    {
                                        DAT += "SN:" + tray.GetDataVales()[tryaid[i] - 1].PanelID + Environment.NewLine;
                                        //tray.dataObjs[idS[i] - 1].OK = false;
                                    }
                                    else
                                    {
                                        tray.GetDataVales()[tryaid[i] - 1].OK = false;
                                    }
                                    if (tray.GetDataVales()[tryaid[i] - 1].Data != null)
                                    {
                                        DAT += "数据:" + tray.GetDataVales()[tryaid[i] - 1].Data;
                                    }
                                    label1.Text = DAT;
                                }
                                else
                                {
                                    tray.GetDataVales()[tryaid[i] - 1].OK = false;
                                }
                                label1.Tag = tray.GetDataVales()[tryaid[i] - 1];
                            }
                        }
                    }
                    label1.Text = tray.GetTrayString();
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetValue(double value)
        {
            try
            {
                OneDataVale data = tray.GetDataVales()[tray.Number - 1];
                if (data != null)
                {
                    Control[] controls = panel1.Controls.Find(data.TrayLocation.ToString(), false);
                    if (controls.Length != 0)
                    {
                        Label label1 = controls[0] as Label;
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
                            string datStr = data.TrayLocation.ToString();
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
                            datStr += ":" + value;
                            data.Data = new List<double>();
                            data.Data.Add(value);
                            label1.Text = datStr;
                            label1.Tag = data;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public void SetValue(List<bool> listValue)
        {
            try
            {
                for (int i = 0; i < listValue.Count; i++)
                {
                    OneDataVale data = tray.GetDataVales()[i];
                    data.OK = listValue[i];
                    if (data != null)
                    {
                        Control[] controls = panel1.Controls.Find(data.TrayLocation.ToString(), false);
                        if (controls.Length != 0)
                        {
                            Label label1 = controls[0] as Label;
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
                                string datStr = data.TrayLocation.ToString();
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
                label1.Text = tray.GetTrayString();
            }
            catch (Exception ex)
            {
            }
        }

        public void SetValue(bool listValue)
        {
            try
            {
                for (int i = 0; i < tray.Count; i++)
                {
                    OneDataVale data = tray.GetDataVales()[i];
                    if (data != null)
                    {
                        Control[] controls = panel1.Controls.Find(data.TrayLocation.ToString(), false);
                        if (controls.Length != 0)
                        {
                            Label label1 = controls[0] as Label;
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
                                else
                                {
                                    label1.BackColor = Color.Red;
                                }
                                string datStr = data.TrayLocation.ToString();
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
                label1.Text = tray.GetTrayString();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 设置当前选择
        /// </summary>
        /// <param name="number"></param>
        public void SelesItem(int number)
        {
            try
            {
                UpData();
                Control[] controls = panel1.Controls.Find(number.ToString(), false);
                if (controls.Length != 0)
                {
                    if (label1d != null)
                    {
                        label1d.ISBorder = false;
                    }
                    label1d = controls[0] as LabelEx;
                    if (label1d != null)
                    {
                        label1d.ISBorder = true;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 更新指定位置状态
        /// </summary>
        /// <param name="number">编号</param>
        /// <param name="errNuber">错误状态，-1错误,0 清空，1OK，2NG红色，3NG黄色，4NG蓝色，5NG粉色 </param>
        public void SetValue(int number, int errNuber)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    Control[] controls = panel1.Controls.Find(number.ToString(), false);
                    OneDataVale data = tray.GetDataVales()[number - 1];
                    if (controls.Length != 0)
                    {
                        Label label1 = controls[0] as Label;
                        if (label1 != null)
                        {
                            switch (errNuber)
                            {
                                case -1:
                                    label1.BackColor = Color.Black;
                                    break;

                                case 0:
                                    data.NotNull = false;
                                    label1.BackColor = Color.Transparent;
                                    break;

                                case 1:
                                    data.OK = true;
                                    label1.BackColor = Color.Green;
                                    break;

                                case 2:
                                    data.OK = false;
                                    label1.BackColor = Color.Red;
                                    break;

                                case 3:
                                    data.OK = false;
                                    label1.BackColor = Color.Yellow;
                                    break;

                                case 4:
                                    data.OK = false;
                                    label1.BackColor = Color.Blue;
                                    break;

                                case 5:
                                    data.OK = false;
                                    label1.BackColor = Color.Pink;
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

        private LabelEx label1d;
    }
}