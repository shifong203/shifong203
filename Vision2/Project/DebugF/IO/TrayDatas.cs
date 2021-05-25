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
        public TrayDatas()
        {
            InitializeComponent();
        }
        public void SetTray(TrayRobot tray)
        {
            Tray = tray;
            AddRow(tray);
        }
        TrayRobot Tray;
        public void RestValue()
        {
            if (Tray != null)
            {
                Tray.Number = 1;
                Tray.GetDataVales().Clear();
                Tray.GetDataVales().AddRange(new Mes.DataVale [Tray.Count]);
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
   
            for (int i = 0; i < Tray.Count; i++)
            {
                if (Tray.GetDataVales()[i]!=null)
                {
                    SetValue(i + 1, Tray.GetDataVales()[i].OK);
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
                        if (Tray.GetDataVales()[id - 1] != null)
                        {
                            if (Tray.GetDataVales()[id - 1].PanelID.Length>=6)
                            {
                                label1.Text += "SN:" + Tray.GetDataVales()[id - 1].PanelID.Substring(Tray.GetDataVales()[id - 1].PanelID.Length - 6);
                            }
                            else
                            {
                                label1.Text += "SN:" + Tray.GetDataVales()[id - 1].PanelID;
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
        private void AddRow(TrayRobot tray)
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
                if (tray.Is8Point)
                {
                    rows = tray.YNumber + tray.Y2Number;
                }
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
                    tray.GetDataVales(new List<DataVale>());
                    tray.GetDataVales().AddRange(new DataVale[rows * columnCount]);
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
                            if (Tray.GetDataVales()[number - 1] != null)
                            {
                                   label1.Text += "SN:" + Tray.GetDataVales()[number - 1].PanelID.Substring(Tray.GetDataVales()[number - 1].PanelID.Length-6);
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
                            if (Tray.GetDataVales()[number - 1] == null)
                            {
                                Tray.GetDataVales()[number - 1] = new DataVale();
                            }
                            Tray.GetDataVales()[number - 1].PanelID = dataVale.PanelID;
                            Tray.GetDataVales()[number - 1] = dataVale;

                            if (Tray.GetDataVales()[number - 1] != null)
                            {
                                if (Tray.GetDataVales()[number - 1].PanelID != null)
                                {
                                    datStr += "SN:" + Tray.GetDataVales()[number - 1].PanelID.Substring(Tray.GetDataVales()[number - 1].PanelID.Length - 6);
                                }
                                label1.label1.Text = datStr;
                            }
                            else
                            {
                                Tray.GetDataVales()[number - 1].OK = false;
                            }
                            label1.Tag = Tray.GetDataVales()[number - 1];
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetValue(int number, TrayRobot dataVale)
        {
            try
            {

                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    for (int i = 0; i < dataVale.GetDataVales().Count; i++)
                    {
                        string datStr = "";
                        DataVale data = dataVale.GetDataVales()[i];
                        if (data != null)
                        {
                            Label label1 = this.Controls.Find(data.TrayLocation.ToString(), false)[0] as Label;
                            if (label1 != null)
                            {
                                if (data.OK)
                                {
                                    label1.BackColor = Color.Green;
                                }
                                else
                                {
                                    label1.BackColor = Color.Red;
                                }
                                datStr = (i + 1).ToString();
                                if (data.PanelID.Length >= 6)
                                {
                                    datStr += "SN:" + data.PanelID.Substring(data.PanelID.Length - 6);
                                }
                                else
                                {
                                    datStr += "SN:" + data.PanelID;
                                }
                                label1.Text = datStr;
                                label1.Tag = data;
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
