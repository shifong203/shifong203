using ErosSocket.DebugPLC.Robot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.Project.Mes;
using static ErosSocket.DebugPLC.Robot.TrayRobot;

namespace Vision2.Project.DebugF.IO
{
    public partial class TrayDatas : UserControl,ITrayRobot
    {
        /// <summary>
        /// 
        /// </summary>
        public TrayDatas()
        {
            InitializeComponent();
        }
        public void SetTray(TrayData tray)
        {
            this.tray = tray;
            tray.AddTary(this);
            AddRow(tray);
        }
        TrayData tray;
        public void RestValue()
        {
            if (tray != null)
            {
                tray.Number = 1;
                tray.Clear();
                //tray.GetDataVales().AddRange(new Mes.DataVale [tray.Count]);
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    Label control = this.Controls[i] as Label;
                    if (control != null)
                    {
                        control.Text = control.Name;
                        control.BackColor = Color.White;
                        control.Tag = null;
                    }
                }
            }
        }

        public void UpDa()
        {
   
            for (int i = 0; i < tray.Count; i++)
            {
                if (tray.GetDataVales()[i]!=null)
                {
                    if (tray.GetDataVales()[i].Null)
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
           
                    Label label1 = this.Controls.Find(id.ToString(), false)[0] as Label;
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
                            if (tray.GetDataVales()[id - 1].PanelID.Length>=6)
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

        public void SetValue(int id,byte byteVae)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {

                    Label label1 = this.Controls.Find(id.ToString(), false)[0] as Label;
                    if (label1 != null)
                    {
                        if (byteVae==1)
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
        private void AddRow(TrayData tray)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    this .Controls.Clear();
                if (tray == null)
                {
                    return;
                }
                int rows = tray.YNumber;
                int columnCount = tray.XNumber;
                //if (tray.Is8Point)
                //{
                //    rows = tray.YNumber + tray.Y2Number;
                //}
                // 动态添加一行
                if (rows == 0)
                {
                    return;
                }
                if (columnCount == 0)
                {
                    return;
                }
                this.Controls.Clear();
                if (tray.GetDataVales() == null)
                {
                     tray.Clear();
                }
                for (int i = 0; i < rows * columnCount; i++)
                {
                    Label trayControl = new Label();
                    trayControl.AutoSize = false;
                    trayControl.BorderStyle = BorderStyle.FixedSingle;
                    trayControl.TextAlign = ContentAlignment.MiddleCenter;
                    trayControl.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    int sd = 0;
                    int dt = 0;
                    int heit, Wita;
                    heit = this.Height / (rows);
                    Wita = this.Width / (columnCount);
                    if (tray.HorizontallyORvertically)
                    {
                        heit = this.Height / (columnCount);
                        Wita = this.Width / (rows);
                        sd = i / rows;
                        dt = i % rows;
                        switch (tray.TrayDirection)
                        {
                            case ErosSocket.DebugPLC.Robot.TrayRobot.TrayDirectionEnum.左上:
                                trayControl.Name = trayControl.Name = trayControl.Text = (dt * columnCount + sd + 1).ToString();
                                break;
                            case ErosSocket.DebugPLC.Robot.TrayRobot.TrayDirectionEnum.右上:
                                //trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = ((rows * (sd + 1)) - dt).ToString();
                                trayControl.Name = trayControl.Name = trayControl.Text = (rows * columnCount - (columnCount * (dt + 1)) + sd + 1).ToString();
                                break;
                            case ErosSocket.DebugPLC.Robot.TrayRobot.TrayDirectionEnum.左下:
                                trayControl.Name = trayControl.Name = trayControl.Text = ((dt + 1) * columnCount - sd).ToString();
                                break;
                            case ErosSocket.DebugPLC.Robot.TrayRobot.TrayDirectionEnum.右下:
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
                    //trayControl.MouseDown += Label1_MouseDown;
                    //trayControl.MouseUp += Label1_MouseMove; ;
                    this.Controls.Add(trayControl);
                }
                                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.PadRight(30, ' '), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetValue(int number, bool value, double? valueDouble = null)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    if (this.Controls.Find(number.ToString(), false).Length != 0)
                    {
                        Label label1 = this.Controls.Find(number.ToString(), false)[0] as Label;
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
                                   label1.Text += "SN:" + tray.GetDataVales()[number - 1].PanelID.Substring(tray.GetDataVales()[number - 1].PanelID.Length-6);
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
        public void SetValue(int number, DataVale dataVale)
        {
            try
            {
                //Tray.Number = number + 1;
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
                            if (tray.GetDataVales()[number - 1] == null)
                            {
                                tray.GetDataVales()[number - 1] = new DataVale();
                            }
                            tray.GetDataVales()[number - 1].PanelID = dataVale.PanelID;
                            tray.GetDataVales()[number - 1] = dataVale;

                            if (tray.GetDataVales()[number - 1] != null)
                            {
                                if (tray.GetDataVales()[number - 1].PanelID != null)
                                {
                                    datStr += "SN:" + tray.GetDataVales()[number - 1].PanelID.Substring(tray.GetDataVales()[number - 1].PanelID.Length - 6);
                                }
                                label1.label1.Text = datStr;
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
                        DataVale data = dataVale.GetDataVales()[i];
                        if (data != null)
                        {
                            Control[] controls = this.Controls.Find(data.TrayLocation.ToString(), false);
                            if (controls.Length!=0)
                            {
                                Label label1 = controls[0] as Label;
                                if (label1 != null)
                                {
                                    if (data.Null)
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
                        if (this.Controls.Find((tryaid[i]).ToString(), false).Length != 0)
                        {
                            TrayControl label1 = this.Controls.Find((tryaid[i]).ToString(), false)[0] as TrayControl;
                            if (label1 != null)
                            {
                                if (listSN.Count <= i)
                                {
                                    return;
                                }
                                if (listSN[i] != null)
                                {
                                    label1.label1.BackColor = Color.Yellow;
                                }
                                else
                                {
                                    listSN[i] = "";
                                    label1.label1.BackColor = Color.Red;
                                }
                                String DAT = tryaid[i].ToString();
                                if (tray.GetDataVales()[tryaid[i] - 1] == null)
                                {
                                    tray.GetDataVales()[tryaid[i] - 1] = new DataVale();
                                }
                                tray.GetDataVales()[tryaid[i] - 1].PanelID = listSN[i];

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
                                    label1.label1.Text = DAT;
                                }
                                else
                                {
                                    tray.GetDataVales()[tryaid[i] - 1].OK = false;
                                }
                                label1.Tag = tray.GetDataVales()[tryaid[i] - 1];

                            }
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }
    }
}
