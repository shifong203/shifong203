using ErosSocket.DebugPLC.Robot;
using ErosSocket.ErosConLink;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vision2.ConClass;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project.formula;
using Vision2.Project.ProcessControl;

namespace Vision2.Project.Mes.环旭SISF
{
    public class SISF : MesInfon
    {
        [DescriptionAttribute("通信连接名称。"), Category("通信连接"), DisplayName("通信名")]
        public string LinkName { get; set; } = "";

        public void initialization()
        {
            SocketClint = StaticCon.GetSocketClint(LinkName);
            if (SocketClint != null)
            {
                SocketClint.PassiveEvent += SocketClint_PassiveEvent;
            }
        }

        public class SisfData
        {
        }

        public string Send(params string[] TEXT)
        {
            string dataStr = "";
            try
            {
                for (int i = 0; i < TEXT.Length; i++)
                {
                    dataStr += TEXT[i] + ",";
                }
                dataStr = dataStr.Trim(',');

                this.GetSocketClint().Send(dataStr);
            }
            catch (Exception ex)
            {
                AlarmText.AddTextNewLine("SISF写入错误:" + ex.Message, Color.Red);
            }
        
            AddTextSisf("S:" + dataStr);
            return dataStr;
        }

        public void AddTextSisf(string text)
        {
            text= DateTime.Now  +" :"+ text;
            ErosProjcetDLL.Excel.Npoi.AddTextLine(ProcessControl.ProcessUser.Instancen.ExcelPath + "\\" +
                      DateTime.Now.ToString("yyyy-MM-dd") + "\\SFIS记录", text);
        }

        [DescriptionAttribute("CODE_NAME,"), Category("SISF"), DisplayName("SISF版本")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "芯片扫码改造V1", "Presasm1_SPEC")]
        public string SISFVersions { get; set; } = "";

        [DescriptionAttribute("线体标示。"), Category("设备标识"), DisplayName("Fixture_ID线体名")]
        public string Fixture_ID { get; set; } = "EQDIW00011-01";

        [DescriptionAttribute("位置标示。"), Category("设备标识"), DisplayName("Line位置标示")]
        public string Line_Name { get; set; } = "JORDAN";

        [DescriptionAttribute("状态。"), Category("设备标识"), DisplayName("Status状态")]
        public string Status { get; set; } = "OK";

        [DescriptionAttribute("CODE_NAME,"), Category("设备标识"), DisplayName("CODE_NAME")]
        public string CODE_NAME { get; set; } = "";

        [DescriptionAttribute("OutTime超时等待时间ms,"), Category("SISF"), DisplayName("等待超时")]
        public int OutTime { get; set; } = 10000;

        [DescriptionAttribute("判断SN长度,"), Category("功能"), DisplayName("码长度判断")]
        public bool QRLength { get; set; } 

        private string SocketClint_PassiveEvent(byte[] key, SocketClint socket, System.Net.Sockets.Socket socketR)
        {
            return "";
        }

        public SocketClint GetSocketClint()
        {
            if (SocketClint == null)
            {
                initialization();
            }
            return SocketClint;
        }

        private SocketClint SocketClint;

        public override event IMesData.ResTMesd ResDoneEvent;

        public override Form GetForm()
        {
            return new SisfForm1(this);
        }

        public override void WrietMes(UserFormulaContrsl userFormulaContrsl, string QRCODE, string Product_Name)
        {
            throw new NotImplementedException();
        }

        public override void WrietMes(TrayData trayData, string Product_Name)
        {
            try
            {
    
                if (!this.GetSocketClint().IsConn)
                {
                    this.GetSocketClint().AsynLink();
                }

                if (!this.GetSocketClint().IsConn)
                {
                    AlarmListBoxt.AddAlarmText("SISF", "错误:SISF连接断开");
                    trayData.SetNumberValue(false);
                }
                else
                {
                    if (SISFVersions == "芯片扫码改造V1")
                    {
                        Sisf1(trayData);
                   
                    }
                    else if (SISFVersions == "Presasm1_SPEC")
                    {
                       Sisf2(trayData);
                    }
                }
                WrietDATA(trayData);
            }
            catch (Exception)
            {
            }
        }

        public void WrietDATA(TrayData tray)
        {
            try
            {
                string FileName = DateTime.Now.ToString("yyyyMMdd") + ".CSV";
                if (!System.IO.File.Exists(ProcessUser.Instancen.ExcelPath + "\\" + FileName))
                {
                    Npoi.AddWriteColumnToExcel(ProcessUser.Instancen.ExcelPath + "\\" + FileName, "数据", "位号", "SN", "状态","SISF信息");
                }
                Npoi.AddRosWriteToExcel(ProcessUser.Instancen.ExcelPath + "\\" + FileName,"数据", DateTime.Now.ToString(),
                    tray.GetDataVales().Count.ToString(), tray.OK.ToString(), tray.MesRestStr);

                for (int i = 0; i < tray.GetDataVales().Count; i++)
                {
                    
                    Npoi.AddRosWriteToExcel(ProcessUser.Instancen.ExcelPath + "\\" + FileName, "数据", tray.GetDataVales()[i].TrayLocation.ToString(),
                           tray.GetDataVales()[i].PanelID, tray.GetDataVales()[i].OK.ToString(), tray.GetDataVales()[i].MesStr);
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region 4楼芯片扫码改造版SISF2、7

        /// <summary>
        /// 扫码改造版SISF2、7
        /// </summary>
        /// <param name="trayData"></param>
        public void Sisf1(TrayData trayData)
        {
            try
            {
                this.SendStep7(trayData.TrayIDQR);
                string datas = this.GetSocketClint().AlwaysRece(OutTime);
                AddTextSisf("R:" + datas);
                trayData.MesRestStr += datas;
                if (datas.StartsWith("OK"))
                {
                    AlarmText.AddTextNewLine("SISF1OK");
                    this.SendStep2(trayData.TrayIDQR, trayData.GetTraySN()[0], trayData.GetTraySN()[trayData.Count-1]);
                    datas = this.GetSocketClint().AlwaysRece(OutTime);
               
                    AddTextSisf(" R:" + datas);
                    trayData.MesRestStr += datas;
                    if (datas.StartsWith("OK"))
                    {
                        AlarmText.AddTextNewLine("SISF2OK");
                        trayData.SetNumberValue(true);
                        UserFormulaContrsl.SetOK(3);
                    }
                    else
                    {
                        if (datas == "")
                        {
                            AlarmListBoxt.AddAlarmText("SISF", "SISF2等待超时错误");
                            AlarmText.AddTextNewLine("SISF2等待超时错误");
                        }
                        else
                        {
                            AlarmListBoxt.AddAlarmText("SISF", "SISF2错误:" + datas);
                            AlarmText.AddTextNewLine("SISF2" + datas);
                        }
                        UserFormulaContrsl.SetOK(2);
                    }
                }
                else
                {
                    if (datas == "")
                    {
                        AlarmListBoxt.AddAlarmText("SISF", "SISF1等待超时错误");
                        AlarmText.AddTextNewLine("SISF1等待超时错误");
                    }
                    else
                    {
                        AlarmListBoxt.AddAlarmText("SISF", "SISF1错误:" + datas);
                        AlarmText.AddTextNewLine("SISF1" + datas);
                    }
                    UserFormulaContrsl.SetOK(2);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string SendStep7(string Cid)
        {
            return Send(Fixture_ID, Cid, "7", ProjectINI.In.UserID, Line_Name, "", Status, CODE_NAME, "", "", "", "", "", "");
        }

        public string SendStep2(string Cid, params string[] traySn)
        {
            return Send(Fixture_ID, Cid, "2", ProjectINI.In.UserID, Line_Name, "", Status, "PANEL_SERIAL_NUMBER=" + Cid + ";1_SUFFIX_SERIAL_NUMBER=" + traySn[0] + ";2_SUFFIX_SERIAL_NUMBER=" + traySn[1]);
        }

        #endregion 4楼芯片扫码改造版SISF2、7

        #region 扫码Presasm1_SPEC

        /// <summary>
        /// 扫码Presasm1_SPEC SISF2
        /// </summary>
        /// <param name="trayData"></param>
        public string Sisf2(TrayData trayData)
        {
            bool QRLength = false;
            string datSSD = "";
            int number = 0;
            for (int i = 0; i < trayData.Count; i++)
            {
                if (QRLength)
                {
                    if (trayData.GetOneDataVale(i).PanelID.Length == int.Parse(Product.GetProd()["码长度"]))
                    {
                        datSSD += "SN" + (i + 1) + "=" + trayData.GetOneDataVale(i).PanelID + ":OK;";
                    }
                    else
                    {
                        number = trayData.GetOneDataVale(i).PanelID.Length;
                        QRLength = true;
                        datSSD += "SN" + (i + 1) + "=" + trayData.GetOneDataVale(i).PanelID + ":NG;";
                    }
                }
                else
                {
                    datSSD += "SN" + (i + 1) + "=" + trayData.GetOneDataVale(i).PanelID + ":OK;";
                }
            }
            if (QRLength)
            {
                AddTextSisf("err:码长度错误:" + number + "!=" + Product.GetProd()["码长度"]);
                AlarmText.AddTextNewLine("err:码长度错误:" + number +"!="+ Product.GetProd()["码长度"]);
            }
            datSSD = Send(Fixture_ID, "SN1", "2", ProjectINI.In.UserID, Line_Name, "", Status,
                "", "", "", "", Product.Work_Order, datSSD);
            string datas = this.GetSocketClint().AlwaysRece(OutTime);
            trayData.MesRestStr = datas;
            if (datas == "")
            {
                AddTextSisf( " R:等待超时ms" + OutTime);
                UserFormulaContrsl.SetString("超时", Color.Red);
            }
            else if (datas.StartsWith("OK"))
            {
                AlarmText.AddTextNewLine("SISF:" + datas);
                AddTextSisf(" R:" + datas);
                trayData.SetNumberValue(true);
                UserFormulaContrsl.SetOK(3);
             
            }
            else if (datas.Contains("FAIL"))
            {
                AlarmText.AddTextNewLine("SISF:" + datas, Color.Red);
                AddTextSisf(" R:" + datas);
                UserFormulaContrsl.SetOK(2);
            }
            return datas;
        }

        #endregion 扫码Presasm1_SPEC

        public override void WrietMes(OneDataVale trayData, string Product_Name)
        {
            throw new NotImplementedException();
        }

        public override void WrietMesAll<T>(T data, string QRCODE, string Product_Name)
        {
            throw new NotImplementedException();
        }

        public override bool ReadMes(out string resetMesString, TrayData trayData)
        {
            resetMesString = "";
            try
            {
                //this.SendStep7(traySN);
                //this.GetSocketClint().AlwaysRece();
            }
            catch (Exception)
            {
            }
            return false;
        }

        public override bool ReadMes(string sn, out string resetMesString)
        {
            throw new NotImplementedException();
        }
    }
}