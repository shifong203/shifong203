using ErosSocket.DebugPLC.Robot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project.DebugF;
using Vision2.Project.formula;

namespace Vision2.Project.Mes
{
    public class MesJib : MesInfon
    {
        public MesData MesData { get; set; } = new MesData();
        private cnshah0tis01.MES_TIS webservice;

        public cnshah0tis01.MES_TIS GetMES_TIS()
        {
            return webservice;
        }

        public MesJib()
        {
            try
            {
                webservice = new cnshah0tis01.MES_TIS();
            }
            catch (Exception ex)
            {
            }
        }

        public override event IMesData.ResTMesd ResDoneEvent;

        public override bool ReadMes(out string resetMesString, TrayData trayData)
        {
            bool OK = false;
            resetMesString = "";

            foreach (var item in trayData.GetDataVales())
            {
                try
                {
                    if (item.NotNull)
                    {
                        resetMesString = webservice.OKToTest(MesData.Customer, MesData.DiviSion, item.PanelID, item.Product_Name, MesData.Testre_Name,
                      MesData.Test_Process);
                        if (!resetMesString.ToLower().Contains("pass"))
                        {
                            ErosProjcetDLL.Project.AlarmText.AddTextNewLine(item.PanelID + ":" + resetMesString, Color.Red);
                        }
                        else
                        {
                            ErosProjcetDLL.Project.AlarmText.AddTextNewLine(item.PanelID + ":" + resetMesString);
                            OK = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    resetMesString = "SN:" + item.PanelID + "读取失败:" + ex.Message;
                }
            }

            try
            {
                ResDoneEvent?.Invoke(OK, resetMesString);
            }
            catch (Exception)
            {
            }
            return OK;
        }

        public override bool ReadMes(string sn, out string resetMesString)
        {
            bool OK = false;
            resetMesString = "";
            try
            {
                resetMesString = webservice.OKToTest(MesData.Customer,
                    MesData.DiviSion,
                sn, Product.ProductionName,
                MesData.Testre_Name,
                MesData.Test_Process);
                if (!resetMesString.ToLower().Contains("pass"))
                {
                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine(sn + ":" + resetMesString, Color.Red);
                }
                else
                {
                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine(sn + ":" + resetMesString);
                    OK = true;
                }
            }
            catch (Exception ex)
            {
                resetMesString = "SN:" + sn + "读取失败:" + ex.Message;
            }
            try
            {
                ResDoneEvent?.Invoke(OK, resetMesString);
            }
            catch (Exception)
            {
            }
            return OK;

            //return ReadMes(out resetMesString, DebugCompiler.Instance.DDAxis.GetTrayInxt(0).GetTrayData().GetDataVales()[0].PanelID);
        }

        public override void WrietMes(UserFormulaContrsl userFormulaContrsl, string QRCODE, string Product_Name)
        {
            List<string> ListText = new List<string>();

            ListText.Add("S");
            ListText.Add("C" + MesData.Customer);
            ListText.Add("I" + MesData.DiviSion);
            ListText.Add("N" + MesData.Testre_Name);
            ListText.Add("P" + MesData.Test_Process);
            ListText.Add("TF");
            //ListText.Add(RecipeCompiler.Instance.MesDatat.CRD);
            ListText.Add("O" + ErosProjcetDLL.Project.ProjectINI.In.UserName);
            ListText.Add("[" + DebugF.DebugCompiler.Instance.DDAxis.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
            ListText.Add("]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            string sn = "";
            string strTimed = DateTime.Now.ToString("yyyy-MM-dd");
            string pathEx = MesData.DataPaht + "//" + strTimed + ".xls";
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
            dat.Add(DebugCompiler.Instance.DDAxis.StartTime.ToString("HH:mm:ss"));
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
                                string paht = ProcessControl.ProcessUser.Instancen.ExcelPath + "//" + sn + DateTime.Now.ToString(" yyyy-MM-dd-HH-mm-ss");
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

        public override void WrietMes(OneDataVale data, string Product_Name)
        {
            if (data.PanelID == null || data.PanelID == "")
            {
                ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new ErosProjcetDLL.Project.AlarmText.alarmStruct() { Text = "Mes信息缺少SN", Name = "Mes" });
                return;
            }
            List<string> ListText = new List<string>();
            //MesData.UserIDName = ErosProjcetDLL.Project.ProjectINI.In.UserName;
            //MesData.UserID = ErosProjcetDLL.Project.ProjectINI.In.UserID;
            ListText.Add("S" + data.PanelID);
            ListText.Add("C" + MesData.Customer);
            ListText.Add("B" + MesData.Board_Style);
            if (MesData.DiviSion != "")
            {
                ListText.Add("I" + MesData.DiviSion);
            }

            ListText.Add("N" + MesData.Testre_Name);
            ListText.Add("P" + MesData.Test_Process);
            ListText.Add('s' + data.TrayLocation.ToString());
            ListText.Add('D' + MesData.Software_Document);
            ListText.Add('R' + MesData.Software_Revision);
            ListText.Add('n' + data.Product_Name);
            ListText.Add('r' + MesData.Assembly_Revision);
            ListText.Add('W' + MesData.Firmware_Revision);

            if (data.OK) ListText.Add("TP");
            else ListText.Add("TF");
            ListText.Add("O" + ErosProjcetDLL.Project.ProjectINI.In.UserID);
            ListText.Add("L" + MesData.Line);
            ListText.Add("p" + MesData.Site);
            ListText.Add("[" + DebugCompiler.Instance.DDAxis.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
            ListText.Add("]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            string strTimed = data.EndTime.ToString(MesData.FileTimeName);
            string pathEx = MesData.DataPaht + "//" + data.EndTime.ToString("yyyyMMdd") + ".csv";
            if (!File.Exists(pathEx))
            {
                List<string> columnText = new List<string>() { "进站时间", "结束时间", "状态", "SN", "托盘位" };
                for (int i = 0; i < RecipeCompiler.Instance.Data.ListDatV.Count; i++)
                {
                    columnText.Add(RecipeCompiler.Instance.Data.ListDatV[i].ComponentName);
                }
                ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, columnText.ToArray());
            }
            List<string> dat = new List<string>();
            dat.Add(DebugCompiler.Instance.DDAxis.StartTime.ToString("HH:mm:ss"));
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
                                dat.Add(iteminde2.ComponentID + "=" + iteminde2.dataMinMax.ValueStrs[i]);
                            }
                        }
                    }
                }
            }
            ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, dat.ToArray());
            string paht = data.PanelID + strTimed;
            if (ErosProjcetDLL.Project.ProjectINI.DebugMode)
            {
                paht = "DEBUG-" + paht;
            }
            ErosProjcetDLL.Excel.Npoi.WriteF(MesData.DataPaht + "\\" + DateTime.Now.ToString("yyyyMMdd") + "//" + paht, ListText, RecipeCompiler.Instance.Filet);
            ErosProjcetDLL.Excel.Npoi.WriteF(ProcessControl.ProcessUser.Instancen.ExcelPath + "//" + paht, ListText, RecipeCompiler.Instance.Filet);
        }

        public override void WrietMes(TrayData tray, string Product_Name)
        {
            List<int> trayID = new List<int>();
            List<string> MesDatas = new List<string>();
            List<string> listResult = new List<string>();
            List<string> dat = new List<string>();
            List<bool> Resmes = new List<bool>();
            string sn = "";
            try
            {
                MesDatas.Add("S");
                MesDatas.Add("C" + MesData.Customer);
                MesDatas.Add("I" + MesData.DiviSion);
                MesDatas.Add("N" + MesData.Testre_Name);
                MesDatas.Add("P" + MesData.Test_Process);
                MesDatas.Add("TF");
                MesDatas.Add("O" + ErosProjcetDLL.Project.ProjectINI.In.UserName);
                MesDatas.Add("[" + DebugCompiler.Instance.DDAxis.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                MesDatas.Add("]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                string strTimed = DateTime.Now.ToString("yyyy-MM-dd");
                string pathEx = MesData.DataPaht + "\\" + strTimed + ".csv";
                if (!File.Exists(pathEx))
                {
                    List<string> columnText = new List<string>() { "StratTime", "EndTime", "status", "SN", "机检", "Mes", "FVt" };
                    ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, columnText.ToArray());
                }
                dat.Add(DebugCompiler.Instance.DDAxis.StartTime.ToString("HH:mm:ss"));
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
                List<string> listSn = new List<string>();
                int ErrBool = 0;
                string err = "";
                string mesErr = "";
                for (int i = 0; i < tray.Count; i++)
                {
                    if (tray.GetDataVales().Count <= i)
                    {
                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine("mes产品数量错误");
                        continue;
                    }
                    try
                    {
                        if (tray.GetDataVales()[i] != null)
                        {
                            if (tray.GetDataVales()[i].NotNull)
                            {
                                sn = tray.GetDataVales()[i].PanelID;
                                if (sn == null || sn == "")
                                {
                                    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(
                                        new ErosProjcetDLL.Project.AlarmText.alarmStruct() { Name = "ID未读取", Text = "ID未读取到" });
                                }
                                else
                                {
                                    string errMesString = "null";
                                    bool resMesOK = true;
                                    if (MesData.IsRardMes)
                                    {
                                        if (!ReadMes(sn, out errMesString))
                                        {
                                            mesErr += "位置:" + tray.GetDataVales()[i].TrayLocation + ";SN:" + sn + ":" + errMesString;
                                            ErosProjcetDLL.Project.AlarmText.AddTextNewLine("位置:" + tray.GetDataVales()[i].TrayLocation + ";SN:" + sn, Color.Red);
                                            resMesOK = false;
                                        }
                                        else
                                        {
                                            resMesOK = true;
                                        }
                                        tray.GetDataVales()[i].MesStr = errMesString;
                                    }
                                    trayID.Add(tray.GetDataVales()[i].TrayLocation);
                                    listSn.Add(sn);
                                    MesDatas[0] = "S" + sn;
                                    dat = new List<string>();
                                    dat.Add("");
                                    dat.Add(tray.GetDataVales()[i].TrayLocation.ToString());
                                    List<string> MesDataList = new List<string>();
                                    if (tray.GetDataVales()[i].OK)
                                    {
                                        MesDatas[5] = "TP";
                                        dat.Add("OK");
                                        listResult.Add("Pass");
                                        MesDataList.AddRange(MesDatas);
                                    }
                                    else
                                    {
                                        listResult.Add("Fail");
                                        MesDatas[5] = "TF";
                                        dat.Add("NG");
                                        MesDataList.AddRange(MesDatas);
                                        foreach (var item in tray.GetDataVales()[i].GetAllCompOBJs().DicOnes)
                                        {
                                            if (!item.Value.OK)
                                            {
                                                string dd = "F" + tray.GetDataVales()[i].TrayLocation + item.Value.ComponentID;
                                                if (!MesDataList.Contains(dd))
                                                {
                                                    MesDataList.Add(dd);
                                                }
                                            }
                                        }
                                        foreach (var item in tray.GetDataVales()[i].GetNGCompData().DicOnes)
                                        {
                                            if (!item.Value.OK)
                                            {
                                                string dd = "F" + tray.GetDataVales()[i].TrayLocation + item.Value.ComponentID;
                                                if (!MesDataList.Contains(dd))
                                                {
                                                    MesDataList.Add(dd);
                                                }
                                            }
                                        }
                                    }
                                    dat.Add(sn);
                                    dat.Add(tray.GetDataVales()[i].AutoOK.ToString());
                                    dat.Add(errMesString);
                                    dat.Add("null");
                                    //dat.Add(errMesString);
                                    //dat.Add("FVT:"+);
                                    RecipeCompiler.AddOKNumber(tray.GetDataVales()[i].OK);
                                    string paht = "//" + sn + DateTime.Now.ToString(" yyyy-MM-dd-HH-mm-ss");
                                    if (resMesOK)
                                    {
                                        ErosProjcetDLL.Excel.Npoi.WriteF(ProcessControl.ProcessUser.Instancen.ExcelPath + paht, MesDataList, RecipeCompiler.Instance.Filet);
                                        ErosProjcetDLL.Excel.Npoi.WriteF(MesData.DataPaht + "\\Mes记录\\" + DateTime.Now.ToString("yyyyMMdd") + paht, MesDataList, RecipeCompiler.Instance.Filet);
                                    }
                                    ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, dat.ToArray());
                                    //OneDataVale oneDataVale = tray.GetDataVales()[i];
                                    //Task task = new Task(new Action(() => {
                                    //    WrietDATA(oneDataVale);
                                    //}));
                                    //task.Start();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrBool++;
                        err += i + ex.Message;
                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine("mesTray" + i + ":" + ex.StackTrace.Substring(ex.StackTrace.Length / 2 - 1, ex.StackTrace.Length / 2));
                    }
                }

                Task task = new Task(new Action(() =>
                {
                    for (int i = 0; i < tray.Count; i++)
                    {
                        if (tray.GetDataVales().Count <= i)
                        {
                            ErosProjcetDLL.Project.AlarmText.AddTextNewLine("mes产品数量错误");
                            continue;
                        }
                        if (tray.GetDataVales()[i].NotNull)
                        {
                            WrietDATA(tray.GetOneDataVale(i));
                        }
                    }
                }));
                task.Start();

                if (MesData.FvtReadData)
                {
                    ReadFvt(trayID.ToArray(), listSn.ToArray(), listResult.ToArray());
                }
                if (mesErr != "")
                {
                    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new ErosProjcetDLL.Project.AlarmText.alarmStruct()
                    { Name = "Mes查询错误", Text = mesErr });
                }
                if (ErrBool > 0)
                {
                    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText
                   (new ErosProjcetDLL.Project.AlarmText.alarmStruct()
                   { Name = "Mes写入Tray失败", Text = "错误数:" + ErrBool + ";" + err });
                }
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new ErosProjcetDLL.Project.AlarmText.alarmStruct()
                { Name = "Mes写入Tray失败", Text = ex.Message });
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("mesTrya:" + ex.StackTrace.Substring(ex.StackTrace.Length / 2 - 1, ex.StackTrace.Length / 2));
            }
        }

        /// <summary>
        /// 写入产品CRD数据
        /// </summary>
        /// <param name="oneDataVale"></param>
        public void WrietDATA(OneDataVale oneDataVale)
        {
            try
            {
                string path = MesData.DataPaht + "\\CRD数据\\" + DateTime.Now.ToString("yyyyMMdd") + ".CSV";
                if (!File.Exists(path))
                {
                    List<string> columnText = new List<string>() { "NO","Line", "Customer","Mode","	Defect Type","Location" ,"Serial Number","Result"   ,"Date" ,
                        "Start Time"    ,"End Time","User", "Placement Route Step","位置","机检"};
                    ErosProjcetDLL.Excel.Npoi.AddWriteCSV(path, columnText.ToArray());
                }
                int no = 0;
                foreach (var item in oneDataVale.GetAllCompOBJs().DicOnes)
                {
                    no++;
                    List<string> data = new List<string>();
                    data.Add(no.ToString());
                    data.Add(MesData.Line);
                    data.Add(MesData.Customer);

                    data.Add(oneDataVale.Product_Name);
                    data.Add(item.Value.RestText);
                    data.Add(item.Value.ComponentID);
                    data.Add(oneDataVale.PanelID);
                    if (item.Value.OK)
                    {
                        data.Add("Pass");
                    }
                    else
                    {
                        data.Add("Fail");
                    }
                    data.Add(DateTime.Now.ToString("d"));
                    data.Add(oneDataVale.StrTime.ToString("T"));
                    data.Add(oneDataVale.EndTime.ToString("T"));
                    data.Add(ErosProjcetDLL.Project.ProjectINI.In.UserName);
                    data.Add(DebugCompiler.Instance.DeviceNameText);
                    data.Add(oneDataVale.TrayLocation.ToString());
                    data.Add(item.Value.NGText);
                    ErosProjcetDLL.Excel.Npoi.AddWriteCSV(path,
                      data.ToArray());
                }
            }
            catch (Exception)
            {
            }
        }

        public override void WrietMesAll<T>(T datas, string QRCODE, string Product_Name)
        {
            try
            {
                TrayData tray = datas as TrayData;
                if (tray != null)
                {
                    WrietMes(tray, Product_Name);
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

        public override Form GetForm()
        {
            return new JabilForm(this);
        }

        public string ReadFvt(int[] trayID, string[] sn, string[] resetStr)
        {
            List<string> TextWeit = new List<string>();

            try
            {
                string fileName = sn[0];
                string PathFvt = MesData.FVTDataPath + "\\" + fileName + ".txt";
                Directory.CreateDirectory(MesData.FVTDataPath);
                Directory.CreateDirectory(MesData.DataPaht + "\\FVT\\" + DateTime.Now.ToString("D"));
                if (MesData.FvtSimulateDATA)
                {
                    for (int i = 0; i < sn.Length; i++)
                    {
                        TextWeit.Add("No:" + trayID[i] + ";SN:" + sn[i] + ";AMVI result:" + resetStr[i]);
                    }
                    File.WriteAllLines(PathFvt, TextWeit);
                    string paths = MesData.DataPaht + "\\FVT\\" + DateTime.Now.ToString("D") + "\\" + Path.GetFileName(fileName) + ".txt";
                    File.WriteAllLines(paths, TextWeit);
                    //if (MesData.FVTSaveDataPath != "")
                    //{
                    //    File.WriteAllLines(MesData.FVTSaveDataPath + "\\" + fileName + ".txt", TextWeit);
                    //}

                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine("FVT写入完成:" + PathFvt);
                    return "";
                }
                List<bool> RsetOk = new List<bool>();
                for (int i = 0; i < trayID.Length; i++)
                {
                    RsetOk.Add(false);
                }
                bool isDa = false;
                string[] fileS = Directory.GetFiles(MesData.FVTPath);

                for (int i = 0; i < fileS.Length; i++)
                {
                    string snt = Path.GetFileNameWithoutExtension(fileS[i]);
                    string[] strDatas = File.ReadAllLines(fileS[i]);
                    for (int j = 0; j < sn.Length; j++)
                    {
                        for (int id = 0; id < strDatas.Length; id++)
                        {
                            if (strDatas[id].Contains(";"))
                            {
                                string[] dat = strDatas[id].Split(';');
                                string[] datas = dat[1].Split(':');
                                if (sn[j] == datas[1])
                                {
                                    string textStr = "";
                                    isDa = true;
                                    if (strDatas[id].Contains("Empty"))
                                    {
                                        resetStr[j] = "Empty";
                                    }
                                    if (strDatas[id].Contains("Fail"))
                                    {
                                        resetStr[j] = "Fail";
                                    }
                                    if (strDatas[id].Contains("Other"))
                                    {
                                        resetStr[j] = "Other";
                                    }
                                    if (strDatas[id].Contains("AMVI"))
                                    {
                                        strDatas[id] = "";
                                        for (int k = 0; k < dat.Length; k++)
                                        {
                                            if (dat[k].StartsWith("AMVI"))
                                            {
                                                dat[k] = "AMVI result:" + resetStr[j];
                                            }
                                            if (dat[k].Length > 1)
                                            {
                                                strDatas[id] += dat[k] + ";";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strDatas[id] += "AMVI result:" + resetStr[j] + ";";
                                    }
                                    for (int k = 0; k < 2; k++)
                                    {
                                        textStr += dat[k] + ";";
                                    }
                                    //textStr = strDatas[id];
                                    TextWeit.Add(textStr += "AMVI result:" + resetStr[j] + ";");
                                    break;
                                }
                            }
                        }
                    }
                    if (isDa)
                    {
                        Directory.CreateDirectory(MesData.FVTDataPath + "\\");
                        File.WriteAllLines(PathFvt, TextWeit);
                        Directory.CreateDirectory(MesData.DataPaht + "\\FVT\\" + DateTime.Now.ToString("D"));
                        string paths = MesData.DataPaht + "\\FVT\\" + DateTime.Now.ToString("D") + "\\" + Path.GetFileName(fileS[i]);
                        if (MesData.FVTSaveDataPath != "")
                        {
                            File.WriteAllLines(MesData.FVTSaveDataPath + "\\" + fileName + ".txt", strDatas);
                        }
                        if (File.Exists(paths))
                        {
                            File.Delete(paths);
                        }
                        //File.Move(fileS[i], paths);
                        File.WriteAllLines(paths, strDatas);
                        if (File.Exists(fileS[i]))
                        {
                            File.Delete(fileS[i]);
                        }
                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine("FVT写入完成:" + PathFvt);
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("FVT错误:" + ex.Message);
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("FVT错误:" + ex.StackTrace);
            }
            ErosProjcetDLL.Project.AlarmText.AddTextNewLine("FVT错误:FVT读取失败");
            ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new ErosProjcetDLL.Project.AlarmText.alarmStruct()
            { Name = "FVT", Text = "FVT读取失败" });
            return "";
        }
    }

    public class MesData
    {
        public MesData()
        {
            Testre_Name = Dns.GetHostName();
        }

        [Description("文件名明名时间规则"), Category("文件名称"), DisplayName("文件名时间规则")]
        public string FileTimeName { get; set; } = "yyyy-MM-dd";

        /// <summary>
        /// 客户名
        /// </summary>
        [Description("客户名Customer"), Category("Mes信息"), DisplayName("C-客户名")]
        public string Customer { get; set; } = "Customer";

        [Description("板样式"), Category("Mes信息"), DisplayName("B-Board_Style")]
        public string Board_Style { get; set; } = "Stack";

        /// <summary>
        /// 客户版本号
        /// </summary>
        [Description("客户版本号 DiviSion"), Category("Mes信息"), DisplayName("I-客户版本号")]
        public string DiviSion { get; set; } = "DiviSion";

        /// <summary>
        /// 计算机名称
        /// </summary>
        [Description("计算机名称 Testre_Name"), Category("Mes信息"), DisplayName("N-计算机名称")]
        public string Testre_Name { get; set; } = "Testre_Name";

        /// <summary>
        /// 设备名
        /// </summary>
        [Description("设备名Test_Process"), Category("Mes信息"), DisplayName("P-设备名")]
        public string Test_Process { get; set; } = "Test_Process";

        /// <summary>
        /// 操作员工号
        /// </summary>
        [Description("Operator ID操作员工号"), Category("Mes信息"), DisplayName("O-操作员工号")]
        public string UserID { get; set; } = "Mtd0";

        ///// <summary>
        ///// 操作员名称
        ///// </summary>
        //[Description("操作员名称"), Category("Mes信息"), DisplayName("操作员名称"), ReadOnly(true)]
        //public string UserIDName { get; set; } = "User1";

        ///// <summary>
        /////
        ///// </summary>
        //[Description("AssemblyNumber"), Category("Mes信息"), DisplayName("n-产品名")]
        //public string AssemblyNumber { get; set; } = "产品名";

        [Description("Defect Location (CRD)"), Category("Mes标识"), DisplayName("c-检测位置")]
        /// <summary>
        /// CRD
        /// </summary>
        public string CRD { get; set; } = "ccap";

        [Description("Site 位置"), Category("Mes信息"), DisplayName("p-位置")]
        public string Site { get; set; } = "SHA";

        [Description("Fixture Slot 固定槽"), Category("Mes信息"), DisplayName("s-固定槽")]
        public string Fixture_Slot { get; set; } = "1";

        [Description("Software Document 软件文件"), Category("Mes信息"), DisplayName("D-软件文件")]
        public string Software_Document { get; set; } = "TarsSoftwareDoc";

        [Description("Software Revision 软件版本"), Category("Mes信息"), DisplayName("R-软件版本")]
        public string Software_Revision { get; set; } = "TarsSoftwareRevision";

        [Description("Software Revision 软件版本"), Category("Mes信息"), DisplayName("r-装配版本")]
        public string Assembly_Revision { get; set; } = "A";

        [Description("Firmware Revision 固件版本"), Category("Mes信息"), DisplayName("W-固件版本")]
        public string Firmware_Revision { get; set; } = "TarsFirmwareRevision";

        [Description("Line 线号"), Category("Mes信息"), DisplayName("L-线体")]
        public string Line { get; set; } = "Bay32";

        [Editor(typeof(PageTypeEditor_FolderBrowserDialog), typeof(UITypeEditor))]
        [Description(""), Category("TE信息"), DisplayName("TE数据地址")]
        /// <summary>
        /// TE数据保存地址
        /// </summary>
        public string TEPath { get; set; } = "";

        [Description(""), Category("FVT信息"), DisplayName("读取FVT数据")]
        /// <summary>
        ///读取FVT数据
        /// </summary>
        public bool FvtReadData { get; set; }

        [Description(""), Category("FVT信息"), DisplayName("模拟FVTPass")]
        /// <summary>
        ///模拟FVTPass
        /// </summary>
        public bool FvtSimulateDATA { get; set; }

        [Editor(typeof(PageTypeEditor_FolderBrowserDialog), typeof(UITypeEditor))]
        [Description(""), Category("FVT信息"), DisplayName("FVT读取数据地址")]
        /// <summary>
        ///FVT读取数据地址
        /// </summary>
        public string FVTPath { get; set; } = "";

        [Editor(typeof(PageTypeEditor_FolderBrowserDialog), typeof(UITypeEditor))]
        [Description(""), Category("FVT信息"), DisplayName("FVT写入数据地址")]
        /// <summary>
        ///FVT写入数据地址
        /// </summary>
        public string FVTDataPath { get; set; } = "";

        [Editor(typeof(PageTypeEditor_FolderBrowserDialog), typeof(UITypeEditor))]
        [Description(""), Category("FVT信息"), DisplayName("FVT另存数据地址")]
        /// <summary>
        ///FVT另存数据地址
        /// </summary>
        public string FVTSaveDataPath { get; set; } = "";

        [Description(""), Category("Mes查询"), DisplayName("是否查询Mes")]
        /// <summary>
        /// 是否读取
        /// </summary>
        public bool IsRardMes { get; set; }

        [Editor(typeof(PageTypeEditor_FolderBrowserDialog), typeof(UITypeEditor))]
        [Description(""), Category("历史记录"), DisplayName("历史记录地址")]
        /// <summary>
        ///历史记录地址
        /// </summary>
        public string DataPaht { get; set; } = "D:\\历史记录";
    }
}