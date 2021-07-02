using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Vision2.Project.DebugF;
using Vision2.Project.formula;
using ErosSocket.DebugPLC.Robot;

namespace Vision2.Project.Mes
{
    public class MesJib : IMesData
    {


        public cnshah0tis01.MES_TIS webservice;

        public MesJib()
        {
            try
            {
                webservice = new cnshah0tis01.MES_TIS();
            }
            catch (Exception)
            {
            }
        }

        public event IMesData.ResTMesd ResDoneEvent;

        public bool ReadMes(string SerialNumber, out string resetMesString)
        {
            bool OK = false;
            resetMesString = "";
            try
            {
                resetMesString = webservice.OKToTest(RecipeCompiler.Instance.MesDatat.Customer, RecipeCompiler.Instance.MesDatat.DiviSion,
                SerialNumber, RecipeCompiler.Instance.MesDatat.AssemblyNumber, RecipeCompiler.Instance.MesDatat.Testre_Name, RecipeCompiler.Instance.MesDatat.Test_Process);
                if (!resetMesString.ToLower().Contains("pass"))
                {
                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine(resetMesString, Color.Red);
                }
                else
                {
                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine(SerialNumber + ":" + resetMesString);
                    OK = true;
                }
            }
            catch (Exception ex)
            {
                resetMesString =  "SN:"+SerialNumber + "读取失败:"  + ex.Message;
            }
            ResDoneEvent?.Invoke(OK ,resetMesString);
            return OK;
        }
        public bool ReadMes(out string resetMesString)
        {
            return ReadMes(DebugCompiler.GetThis().DDAxis.GetTrayInxt(0).GetTrayData().GetDataVales()[0].PanelID, out resetMesString);
        }

        public void WrietMes(UserFormulaContrsl userFormulaContrsl, string QRCODE, string Product_Name)
        {

            List<string> ListText = new List<string>();
            RecipeCompiler.Instance.MesDatat.UserIDName = ErosProjcetDLL.Project.ProjectINI.In.UserName;
            RecipeCompiler.Instance.MesDatat.UserID = ErosProjcetDLL.Project.ProjectINI.In.UserID;
            ListText.Add("S");
            ListText.Add("C" + RecipeCompiler.Instance.MesDatat.Customer);
            ListText.Add("I" + RecipeCompiler.Instance.MesDatat.DiviSion);
            ListText.Add("N" + RecipeCompiler.Instance.MesDatat.Testre_Name);
            ListText.Add("P" + RecipeCompiler.Instance.MesDatat.Test_Process);
            ListText.Add("TF");
            //ListText.Add(RecipeCompiler.Instance.MesDatat.CRD);
            ListText.Add("O" + ErosProjcetDLL.Project.ProjectINI.In.UserName);
            ListText.Add("[" + DebugF.DebugCompiler.GetThis().DDAxis.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
            ListText.Add("]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            string sn = "";
            string strTimed = DateTime.Now.ToString("yyyy-MM-dd");
            string pathEx = ProcessControl.ProcessUser.GetThis().ExcelPath + "//历史数据//" + strTimed + ".xls";
            if (!File.Exists(pathEx))
            {
                List<string> columnText = new List<string>() { "进站时间", "结束时间", "状态", "SN" };
                for (int i = 0; i < RecipeCompiler.Instance.Data.ListDatV.Count; i++)
                {
                    columnText.Add(RecipeCompiler.Instance.Data.ListDatV[i].ComponentName);
                }
                ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, columnText.ToArray());
            }
            List<string> dat = new List<string>();
            dat.Add(DebugCompiler.GetThis().DDAxis.StartTime.ToString("HH:mm:ss"));
            dat.Add(DateTime.Now.ToString("HH:mm:ss"));

            TrayRobot trayRobot = DebugF.IO.TrayDataUserControl.GetTray();
            if (trayRobot != null)
            {
                if (DebugF.IO.TrayDataUserControl.GetTray().GetTrayData().OK)
                {
                    dat.Add("OK");
                }
                else
                {
                    dat.Add("NG");
                }
            }
            ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, dat.ToArray());
            if (trayRobot != null)
            {
                TrayData trayData = trayRobot.GetTrayData();


                for (int i = 0; i < trayData.GetDataVales().Count; i++)
            {
                if (trayData.GetDataVales()[i] != null)
                {
                    if (trayData.GetDataVales()[i].PanelID != null && trayData.GetDataVales()[i].PanelID != "")
                    {
                        sn = trayData.GetDataVales()[i].PanelID;
                        if (sn == null || sn == "")
                        {
                            ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new ErosProjcetDLL.Project.AlarmText.alarmStruct() 
                            { Name = "ID未读取", Text = "ID未读取到" });
                        }
                        else
                        {
                            ListText[0] = "S" + sn;
                            dat = new List<string>();
                            dat.Add("");
                            dat.Add((i + 1).ToString());
                            if (trayData.GetDataVales()[i].OK)
                            {
                                ListText[5] = "TP";
                                dat.Add("OK");
                            }
                            else
                            {
                                ListText[5] = "TF";
                                dat.Add("NG");
                            }
                            dat.Add(sn);
                            //ListText[6] = RecipeCompiler.Instance.MesDatat.CRD + (i + 1);
                            string paht = ProcessControl.ProcessUser.GetThis().ExcelPath + "//" + sn + DateTime.Now.ToString(" yyyy-MM-dd-HH-mm-ss");
                            ErosProjcetDLL.Excel.Npoi.WriteF(paht, ListText, ".txt");
                            List<double> valuse = trayData.GetDataVales()[i].Data as List<double>;
                            if (valuse != null)
                            {
                                for (int j = 0; j < valuse.Count; j++)
                                {
                               
                                    dat.Add(valuse[j].ToString());
                                }
                            }
                            ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, dat.ToArray());
            
                        }

                    }
                }
            }

            }

        }

        public void WrietMes(OneDataVale data, string Product_Name)
        {

            if (data.PanelID == null || data.PanelID == "")
            {
                ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new ErosProjcetDLL.Project.AlarmText.alarmStruct() { Text = "Mes信息缺少SN", Name = "Mes" });
                return;
            }
            List<string> ListText = new List<string>();
            RecipeCompiler.Instance.MesDatat.UserIDName = ErosProjcetDLL.Project.ProjectINI.In.UserName;
            RecipeCompiler.Instance.MesDatat.UserID = ErosProjcetDLL.Project.ProjectINI.In.UserID;
            ListText.Add("S" + data.PanelID);
            ListText.Add("C" + RecipeCompiler.Instance.MesDatat.Customer);
            ListText.Add("B" + RecipeCompiler.Instance.MesDatat.Customer);
            ListText.Add("I" + RecipeCompiler.Instance.MesDatat.DiviSion);
            ListText.Add("N" + RecipeCompiler.Instance.MesDatat.Testre_Name);
            ListText.Add("P" + RecipeCompiler.Instance.MesDatat.Test_Process);
            ListText.Add('s' + data.TrayLocation.ToString());
            ListText.Add('D' + RecipeCompiler.Instance.MesDatat.Software_Document);
            ListText.Add('R' + RecipeCompiler.Instance.MesDatat.Software_Revision);
            ListText.Add('n' + RecipeCompiler.Instance.MesDatat.AssemblyNumber);
            ListText.Add('r' + RecipeCompiler.Instance.MesDatat.Assembly_Revision);
            ListText.Add('W' + RecipeCompiler.Instance.MesDatat.Firmware_Revision);
          
            if (data.OK) ListText.Add("TP");
            else ListText.Add("TF");
            ListText.Add("O" + ErosProjcetDLL.Project.ProjectINI.In.UserID);
            ListText.Add("L" + RecipeCompiler.Instance.MesDatat.Line);
            ListText.Add("p" + RecipeCompiler.Instance.MesDatat.Site);
            ListText.Add("[" + DebugCompiler.GetThis().DDAxis.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
            ListText.Add("]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        
            string strTimed = data.EndTime.ToString(RecipeCompiler.Instance.MesDatat.FileTimeName);
            string pathEx = ProcessControl.ProcessUser.GetThis().ExcelPath + "//历史数据//" + data.EndTime.ToString("yyyyMMdd") + ".csv";
            if (!File.Exists(pathEx))
            {
                List<string> columnText = new List<string>() { "进站时间", "结束时间", "状态", "SN" ,"托盘位"};
                for (int i = 0; i < RecipeCompiler.Instance.Data.ListDatV.Count; i++)
                {
                    columnText.Add(RecipeCompiler.Instance.Data.ListDatV[i].ComponentName);
                }
                ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, columnText.ToArray());
            }
            List<string> dat = new List<string>();
            dat.Add(DebugCompiler.GetThis().DDAxis.StartTime.ToString("HH:mm:ss"));
            dat.Add(DateTime.Now.ToString("HH:mm:ss"));
            if (data.OK)
            {
                dat.Add("OK");
            }
            else
            {
                dat.Add("NG");
            }
            dat.Add(data.PanelID);
            dat.Add(data.TrayLocation.ToString());
            foreach (var item in data.ListCamsData)
            {
                foreach (var itemd in item.Value.ResuOBj)
                {
                    foreach (var itemdt in itemd.GetNgOBJS().DicOnes)
                    {
                        foreach (var iteminde2 in itemdt.Value.oneRObjs)
                        {
                            for (int i = 0; i < iteminde2.dataMinMax.ValueStrs.Count; i++)
                            {
                                dat.Add(iteminde2.ComponentID+"="+ iteminde2.dataMinMax.ValueStrs[i]);
                            }
                        }
                    }
                }
            }
            ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, dat.ToArray());
           
            string paht =   data.PanelID + strTimed;
            if (ErosProjcetDLL.Project.ProjectINI.DebugMode)
            {
                paht = "DEBUG-" + paht;
            }
            ErosProjcetDLL.Excel.Npoi.WriteF(ProcessControl.ProcessUser.GetThis().ExcelPath +"\\历史数据\\" + DateTime.Now.ToString("yyyyMMdd") +"//" + paht, ListText, RecipeCompiler.Instance.Filet);
            ErosProjcetDLL.Excel.Npoi.WriteF(ProcessControl.ProcessUser.GetThis().ExcelPath+ "//" + paht, ListText,RecipeCompiler.Instance.  Filet);
        }

        public void WrietMes(TrayData trayData, string Product_Name)
        {

        }

        public void WrietMesAll<T>(T datas, string QRCODE, string Product_Name)
        {
            try
            {

               TrayData tray = datas as TrayData;
                if (tray != null)
                {
                    List<string> ListText = new List<string>();
                    RecipeCompiler.Instance.MesDatat.UserIDName = ErosProjcetDLL.Project.ProjectINI.In.UserName;
                    RecipeCompiler.Instance.MesDatat.UserID = ErosProjcetDLL.Project.ProjectINI.In.UserID;
                    ListText.Add("S");
                    ListText.Add("C" + RecipeCompiler.Instance.MesDatat.Customer);
                    ListText.Add("I" + RecipeCompiler.Instance.MesDatat.DiviSion);
                    ListText.Add("N" + RecipeCompiler.Instance.MesDatat.Testre_Name);
                    ListText.Add("P" + RecipeCompiler.Instance.MesDatat.Test_Process);
                    ListText.Add("TF");
                    //ListText.Add(RecipeCompiler.Instance.MesDatat.CRD);
                    ListText.Add("O" + ErosProjcetDLL.Project.ProjectINI.In.UserName);
                    ListText.Add("[" + DebugCompiler.GetThis().DDAxis.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    ListText.Add("]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    string sn = "";
                    string strTimed = DateTime.Now.ToString("yyyy-MM-dd");
                    string pathEx = ProcessControl.ProcessUser.GetThis().ExcelPath + "//历史数据//" + strTimed + ".csv";
                    if (!File.Exists(pathEx))
                    {
                        List<string> columnText = new List<string>() { "进站时间", "结束时间", "状态", "SN" };
                        for (int i = 0; i < RecipeCompiler.Instance.Data.ListDatV.Count; i++)
                        {
                            columnText.Add(RecipeCompiler.Instance.Data.ListDatV[i].ComponentName);
                        }
                        ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, columnText.ToArray());
                    }
                    List<string> dat = new List<string>();
                    dat.Add(DebugCompiler.GetThis().DDAxis.StartTime.ToString("HH:mm:ss"));
                    dat.Add(DateTime.Now.ToString("HH:mm:ss"));
                    if (tray.OK)
                    {
                        dat.Add("OK");
                    }
                    else
                    {
                        dat.Add("NG");
                    }
                    ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, dat.ToArray());
                    for (int i = 0; i < tray.Count; i++)
                    {
                        if (tray.GetDataVales()[i] != null)
                        {
                            if (tray.GetDataVales()[i].PanelID != null && tray.GetDataVales()[i].PanelID != "")
                            {
                                sn = tray.GetDataVales()[i].PanelID;
                                if (sn == null || sn == "")
                                {
                                    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(
                                        new ErosProjcetDLL.Project.AlarmText.alarmStruct() { Name = "ID未读取", Text = "ID未读取到" });
                                }
                                else
                                {
                                    ListText[0] = "S" + sn;
                                    dat = new List<string>();
                                    dat.Add("");
                                    dat.Add((i + 1).ToString());
                                    if (tray.GetDataVales()[i].OK)
                                    {
                                        ListText[5] = "TP";
                                        dat.Add("OK");
                                    }
                                    else
                                    {
                                        ListText[5] = "TF";
                                        dat.Add("NG");
                                    }
                                    dat.Add(sn);
                                    RecipeCompiler.AddOKNumber(tray.GetDataVales()[i].OK);
                                    string paht = ProcessControl.ProcessUser.GetThis().ExcelPath + "//" + sn + DateTime.Now.ToString(" yyyy-MM-dd-HH-mm-ss");
                                    ErosProjcetDLL.Excel.Npoi.WriteF(paht, ListText, RecipeCompiler.Instance.Filet);
                                    List<double> valuse = tray.GetDataVales()[i].Data as List<double>;
                                    if (valuse != null)
                                    {
                                        for (int j = 0; j < valuse.Count; j++)
                                        {
                                            dat.Add(valuse[j].ToString());
                                        }
                                    }
                                    ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, dat.ToArray());
                                }
                            }
                        }
                    }
                }
                else
                {
                    OneDataVale data = datas as OneDataVale;
                    WrietMes(data, Product_Name);
                }
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new ErosProjcetDLL.Project.AlarmText.alarmStruct() { Name = "Mes写入失败", Text = ex.Message });
            }
        }
    }
}
