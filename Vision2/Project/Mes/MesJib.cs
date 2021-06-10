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

        public bool ReadMes(string SerialNumber, out string resetMesString)
        {
            resetMesString = "";
            try
            {
                resetMesString = webservice.OKToTest(RecipeCompiler.Instance.MesDatat.Customer, RecipeCompiler.Instance.MesDatat.DiviSion,
                SerialNumber, RecipeCompiler.Instance.MesDatat.AssemblyNumber, RecipeCompiler.Instance.MesDatat.Testre_Name, RecipeCompiler.Instance.MesDatat.Test_Process);
                if (!resetMesString.ToLower().Contains("pass"))
                {
                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(resetMesString, Color.Red);
                }
                else
                {
                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(SerialNumber + ":" + resetMesString);
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
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

        public void WrietMes(DataVale data, string Product_Name)
        {

            if (data.PanelID == null || data.PanelID == "")
            {
                ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new ErosProjcetDLL.Project.AlarmText.alarmStruct() { Text = "Mes信息缺少SN", Name = "Mes" });
                return;
            }
            List<string> ListText = new List<string>();
            RecipeCompiler.Instance.MesDatat.UserIDName = ErosProjcetDLL.Project.ProjectINI.In.UserName;
            RecipeCompiler.Instance.MesDatat.UserID = ErosProjcetDLL.Project.ProjectINI.In.UserID;
            ListText.Add("S");
            ListText.Add("C" + RecipeCompiler.Instance.MesDatat.Customer);
            ListText.Add("I" + RecipeCompiler.Instance.MesDatat.DiviSion);
            ListText.Add("N" + RecipeCompiler.Instance.MesDatat.Testre_Name);
            ListText.Add("P" + RecipeCompiler.Instance.MesDatat.Test_Process);
            ListText.Add("TF");
            ListText.Add("O" + ErosProjcetDLL.Project.ProjectINI.In.UserName);
            ListText.Add("[" + DebugCompiler.GetThis().DDAxis.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
            ListText.Add("]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            ListText[0] = "S" + data.PanelID;
            if (data.OK)
            {
                ListText[5] = "TP";
            }
            else
            {
                ListText[5] = "TF";
            }
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
            if (data.OK)
            {
                dat.Add("OK");
            }
            else
            {
                dat.Add("NG");
            }
            dat.Add(data.PanelID);
            //foreach (var item in data.DataMin_Max)
            //{
            //   dat.Add(item.Value.ComponentName + "");
            //    for (int i2 = 0; i2 < item.Value.Reference_Name.Count; i2++)
            //    {
            //        dat.Add(item.Value.Reference_Name[i2] + ":" + item.Value.Reference_ValueMin[i2] + ">" + item.Value.ValueStrs + "<" + item.Value.Reference_ValueMax);
            //    }
            //}
            ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, dat.ToArray());
            string paht = ProcessControl.ProcessUser.GetThis().ExcelPath + "//" + data.PanelID + DateTime.Now.ToString(" yyyy-MM-dd-HH-mm-ss");
            ErosProjcetDLL.Excel.Npoi.WriteF(paht, ListText, ".txt");
        }

        public void WrietMes(TrayRobot trayData, string Product_Name)
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
                    ListText.Add("[" + DebugF.DebugCompiler.GetThis().DDAxis.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
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
                                    //if (tray.GetDataVales()[i].OK)
                                    //{
                                    //    RecipeCompiler.AddRlsNumber();
                                    //    dat.Add("OK");
                                    //}
                                    //else
                                    //{
                                    //    dat.Add("");
                                    //}
                                    dat.Add(sn);
                                    RecipeCompiler.AddOKNumber(tray.GetDataVales()[i].OK);
                 
                                    string paht = ProcessControl.ProcessUser.GetThis().ExcelPath + "//" + sn + DateTime.Now.ToString(" yyyy-MM-dd-HH-mm-ss");
                                    ErosProjcetDLL.Excel.Npoi.WriteF(paht, ListText, ".txt");
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
                    DataVale data = datas as DataVale;
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
