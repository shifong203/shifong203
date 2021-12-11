using ErosSocket.DebugPLC.Robot;
using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ConClass;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project.formula;

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
                SocketClint.PassiveStringBuilderEvent += SocketClint_PassiveStringBuilderEvent;
            }
        }

        private string sisfStr = "";

        private string SocketClint_PassiveStringBuilderEvent(System.Text.StringBuilder key, SocketClint socket, System.Net.Sockets.Socket socketR)
        {
            try
            {
                sisfStr = key.ToString();
                RestWait = true;
                //SocketClint. RecivesDone = true;
            }
            catch (Exception)
            {
            }
            return "";
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

                this.GetSocketClint().AlwaysReceiveReset();
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
            AlarmText.AddTextNewLine("SISF:" + text);
            text = DateTime.Now + " " + text;
            ErosProjcetDLL.Excel.Npoi.AddTextLine(RecipeCompiler.Instance.DataPaht + "\\SFIS记录" + "\\" +
                      DateTime.Now.ToString("yyyy-MM-dd") + ".txt", text);
        }
        [DescriptionAttribute("CODE_NAME,"), Category("SISF"), DisplayName("SISF版本")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "芯片扫码改造V1", "Presasm1_SPEC")]
        public string SISFVersions { get; set; } = "";

        [DescriptionAttribute("设备名称。"), Category("设备标识"), DisplayName("Fixture_ID设备名")]
        public override string Fixture_ID { get; set; } = "EQDIW00011-01";

        [DescriptionAttribute("位置标示。"), Category("设备标识"), DisplayName("Line位置标示")]
        public override string Line_Name { get; set; } = "JORDAN";

        [DescriptionAttribute("状态。"), Category("设备标识"), DisplayName("Status状态")]
        public string Status { get; set; } = "OK";

        [DescriptionAttribute("CODE_NAME,"), Category("设备标识"), DisplayName("CODE_NAME")]
        public string CODE_NAME { get; set; } = "";

        [DescriptionAttribute("OutTime超时等待时间ms,"), Category("SISF"), DisplayName("等待超时")]
        public int OutTime { get; set; } = 10000;

        [DescriptionAttribute("判断SN长度,"), Category("功能"), DisplayName("码长度判断")]
        public bool QRLength { get; set; }

        public bool RestWait;

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

     

        public override void WrietMes(TrayData trayData, string Product_Name)
        {
            try
            {
                UserFormulaContrsl.SetOK(0);
                RestWait = false;
                if (!this.GetSocketClint().IsConn)
                {
                    this.GetSocketClint().AsynLink(false);
                }
                Thread.Sleep(200);
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
                Task task = new Task(new Action(() =>
                {
                    WrietDATA(trayData);
                }));
                task.Start();
            }
            catch (Exception)
            {
            }
        }

        public void WrietDATA(TrayData tray)
        {
            try
            {
                string FileName = RecipeCompiler.Instance.DataPaht + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
                if (!System.IO.File.Exists(FileName))
                {
                    Npoi.AddWriteColumnToExcel(FileName, "数据", "位号", "SN", "状态", "SISF信息");
                }
                Npoi.AddRosWriteToExcel(FileName, "数据", DateTime.Now.ToString(),
                    tray.GetDataVales().Count.ToString(), tray.OK.ToString(), tray.MesRestStr);

                for (int i = 0; i < tray.GetDataVales().Count; i++)
                {
                    Npoi.AddRosWriteToExcel(FileName, "数据", tray.GetDataVales()[i].TrayLocation.ToString(),
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
                trayData.SetNumberValue(false);
                this.SendStep7(trayData.TrayIDQR);
                if (!this.GetSocketClint().AlwaysReceive(out string datas, OutTime))
                {
                    AddTextSisf("R:" + datas);
                    this.GetSocketClint().Close();
                    Thread.Sleep(200);
                    if (!this.GetSocketClint().IsConn)
                    {
                        this.GetSocketClint().AsynLink(false);
                    }
                    Thread.Sleep(500);
                    this.SendStep7(trayData.TrayIDQR);
                    this.GetSocketClint().AlwaysReceive(out datas, OutTime);
                }
                AddTextSisf("R:" + datas);
                trayData.MesRestStr += datas;
                int stratInt = 0;
                int endInt = trayData.Count - 1;
                if (datas.ToLower().StartsWith("ok7"))
                {
                    this.GetSocketClint().Close();
                    Thread.Sleep(200);
                    List<string> datatass = new List<string>();

                    if (datas.Contains(";"))
                    {
                       string[]  datatas= datas.Remove(0, 4).Split(';');
                        datatass.AddRange(datatas);
                    }
                    if (datatass.Count!=3)
                    {
                        datatass.Clear();
                        datatass.Add("PANEL_SERIAL_NUMBER");
                        datatass.Add("1_SUFFIX_SERIAL_NUMBER");
                        datatass.Add("2_SUFFIX_SERIAL_NUMBER");
                    }
                    //AlarmText.AddTextNewLine("SISF1OK");
                    exst:
                    string statSN = "";
                    string endSN = "";
                    for (int i = stratInt; i < trayData.Count; i++)
                    {
                        if (trayData.GetOneDataVale(i).PanelID!="")
                        {
                            stratInt = i;
                            statSN = trayData.GetOneDataVale(i).PanelID;
                            break;
                        }
                    }
                    for (int i = endInt; i>0; i--)
                    {
                        if (trayData.GetOneDataVale(i).PanelID != "")
                        {
                            endInt = i;
                            endSN = trayData.GetOneDataVale(i).PanelID;
                            break;
                        }
                    }
                    if (!this.GetSocketClint().IsConn)
                    {
                        this.GetSocketClint().AsynLink(false);
                    }
                    Thread.Sleep(200);
                    if (statSN==""|| endSN=="")
                    {
                        AlarmListBoxt.AddAlarmText("SISF", "SISF重复发送超出数量错误,请确认");
                        trayData.SetNumberValue(false);
                        UserFormulaContrsl.SetOK(2);
                    }
                    else
                    {
                  
                        this.SendStep2(trayData.TrayIDQR, datatass.ToArray(), statSN, endSN);
                        /* datas = this.GetSocketClint().AlwaysReceive(OutTime);*/
                        if (this.GetSocketClint().AlwaysReceive(out datas, OutTime))
                        {
                            trayData.MesRestStr += datas;
                            if (datas.StartsWith("OK"))
                            {
                                trayData.SetNumberValue(true);
                                UserFormulaContrsl.SetOK(3);
                            }
                            else
                            {
                                if (datas.Contains("0x0002"))
                                {
                                    bool IS1 = false;
                                    bool IS2 = false;
                                    string[] DATAST = datas.Remove(0, 6).Split(';');
                                    for (int i = 0; i < DATAST.Length; i++)
                                    {
                                        if (DATAST[i].StartsWith("1_SUFFIX_SERIAL_NUMBER"))
                                        {
                                            stratInt++;
                                            IS1 = true;
                                        }
                                        if (DATAST[i].StartsWith("2_SUFFIX_SERIAL_NUMBER"))
                                        {
                                            endInt--;
                                            IS2 = true;
                                        }
                                    }
                                    this.GetSocketClint().Close();
                                    goto exst;
                                }
                                trayData.SetNumberValue(false);
                                if (datas != "")
                                {
                                    AlarmListBoxt.AddAlarmText("SISF", "SISF2错误:" + datas);
                                    AlarmText.AddTextNewLine("SISF2" + datas);
                                }
                                UserFormulaContrsl.SetOK(2);
                            }
                        }
                        else
                        {
                            AlarmListBoxt.AddAlarmText("SISF", "SISF2等待超时错误");
                            AlarmText.AddTextNewLine("SISF2等待超时错误");
                            trayData.SetNumberValue(false);
                            UserFormulaContrsl.SetOK(2);
                        }
                        AddTextSisf(" R:" + datas);
                    }
                }
                else
                {
                    trayData.SetNumberValue(false);
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
                this.GetSocketClint().Close();
            }
            catch (Exception ex)
            {
                AlarmText.AddTextNewLine("SISF1报错：" + ex.Message);
                trayData.SetNumberValue(false);
                UserFormulaContrsl.SetOK(2);
            }

        }

        public string SendStep7(string Cid)
        {
            return Send(Fixture_ID, Cid, "7", ProjectINI.In.UserID, Line_Name, "", Status, CODE_NAME, "", "", "", "", "", "");
        }

        public string SendStep2(string Cid, string[] pranames, params string[] traySn)
        {
            if (pranames.Length==3)
            {
                return Send(Fixture_ID, Cid, "2", ProjectINI.In.UserID, Line_Name, "", Status, pranames[0] + "=" + Cid + ";" + pranames[1] + "=" + traySn[0] + ";" + pranames[2] + "=" + traySn[1]);
            }
            return "";
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
            }
            datSSD = Send(Fixture_ID, trayData.GetOneDataVale(0).PanelID, "2", ProjectINI.In.UserID, Line_Name, "", Status,
                "", "", "", "", Product.Work_Order, datSSD);

            string datas = this.GetSocketClint().AlwaysReceive(OutTime);
            trayData.MesRestStr = datas;
            if (datas == "")
            {
                AddTextSisf(" R:等待超时ms" + OutTime);
                UserFormulaContrsl.SetString("超时", Color.Red);
                trayData.SetNumberValue(false);
            }
            else if (datas.ToLower().StartsWith("ok"))
            {
            
                AlarmText.AddTextNewLine("SISF:" + datas);
                AddTextSisf(" R:" + datas);
                UserFormulaContrsl.SetOK(3);
                if (datas.Contains(","))
                {
                    try
                    {
                        string[] dtaStr = datas.Split(',');
                        if (datas.Contains(";"))
                        {
                            string[] datsdd = dtaStr[1].Split(';');
                            for (int i = 0; i < datsdd.Length; i++)
                            {
                                string[] datsItmes = datsdd[i].Split('=');
                                if (datsItmes[0].StartsWith("SN"))
                                {
                                    string[] dataTrs = datsItmes[01].Split(' ');
                                    int dint = ProjectINI.GetStrReturnInt(datsItmes[0]);
                                    if (trayData.GetOneDataVale(dint - 1).PanelID == dataTrs[0])
                                    {
                                        trayData.GetOneDataVale(dint - 1).MesStr = datsdd[i];
                                    }
                                    else
                                    {
                                        trayData.GetOneDataVale(dint - 1).MesStr = datsdd[i];
                                    }
                                }
                            }
                            ListSNForm.ShowMesabe(datas, datsdd);
                        }
                   
                    }
                    catch (Exception)
                    {
                    }
              
                }
                trayData.SetNumberValue(true);
            }
            else if (datas.ToLower().Contains("fail"))
            {
                trayData.SetNumberValue(true);
                RestData(datas, trayData);
                AddTextSisf(" R:" + datas);
                UserFormulaContrsl.SetOK(2);
            }
            else
            {
                AddTextSisf(" R:" + datas);
                RestData(datas, trayData);
                UserFormulaContrsl.SetOK(2);
            }

            RecipeCompiler.AddOKNumber(trayData.OKNumber);
            RecipeCompiler.GetSPC();
            this.GetSocketClint().Close();
            return datas;
        }

        public void RestData(string datastr, TrayData trayData)
        {
            try
            {
                //FAIL2,SN1=FYJ1167300MUEC053D1Y00303K   MO not Match
                //FAIL2,SN1=FYJ1167300MUEC053D1Y00303K ROUTE NG;SN3=FYJ1167300MUEC053D1Y00303K ROUTE NG
                //FAIL2, NO EMP 2200，Underfil01 no set
                string[] datas = datastr.Split(',');
                string[] dats = datas[1].Split(';');
                if (datastr.Contains("="))
                {
                    for (int i = 0; i < dats.Length; i++)
                    {
                        string[] datsItmes = dats[i].Split('=');
                        if (datsItmes[0].StartsWith("SN"))
                        {
                            string[] dataTrs = datsItmes[01].Split(' ');
                            int dint = ProjectINI.GetStrReturnInt(datsItmes[0]);
                            if (trayData.GetOneDataVale(dint - 1).PanelID == dataTrs[0])
                            {
                                trayData.GetOneDataVale(dint - 1).MesStr = dats[i];
                            }
                            else
                            {
                                trayData.GetOneDataVale(dint - 1).MesStr = dats[i];
                            }
                            RecipeCompiler.AddNGNumber(1);
                            trayData.SetNumberValue(dint, false);
                        }
                        else if (datsItmes[0]=="")     
                        {
                            AlarmListBoxt.AddAlarmText("SISF", "SISF格式错误:"+ datastr);
                            trayData.SetNumberValue(false);
                        }
                    }
                    trayData.GetITrayRobot().UpData();
                }
                else
                {
                    trayData.SetNumberValue(false);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {

            }


        }

        #endregion 扫码Presasm1_SPEC

        public override void WrietMes(OneDataVale trayData, string Product_Name)
        {
            throw new NotImplementedException();
        }

        public override void WrietMesAll<T>(T data, string Product_Name)
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