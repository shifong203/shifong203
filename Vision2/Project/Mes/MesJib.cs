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
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project.DebugF;
using Vision2.Project.formula;

namespace Vision2.Project.Mes
{
    public class MesJib : MesInfon
    {
        public MesData MesData { get; set; } = new MesData();

     

        /// <summary>
        /// 写入产品CRD数据
        /// </summary>
        /// <param name="oneDataVale"></param>
        public override void WrietDATA(OneDataVale oneDataVale)
        {
            try
            {
                string path = RecipeCompiler.Instance.DataPaht + "\\CRD数据\\" + DateTime.Now.ToString("yyyyMMdd") + ".CSV";
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
                    if (item.Value.aOK)
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
                    data.Add(ProjectINI.In.UserName);
                    data.Add(DebugCompiler.Instance.DeviceNameText);
                    data.Add(oneDataVale.TrayLocation.ToString());
                    data.Add(item.Value.NGText);
                    ErosProjcetDLL.Excel.Npoi.AddWriteCSV(path,
                      data.ToArray());
                }

                //ErosProjcetDLL.Excel.Npoi.AddWriteCSV(pathEx, dat.ToArray());
            }
            catch (Exception) { }
        }
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
            catch (Exception ex)   { }
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
                resetMesString = webservice.OKToTest(MesData.Customer, MesData.DiviSion, sn,
               Product.ProductionName, MesData.Testre_Name, MesData.Test_Process);
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

       
        public override void WrietMes(OneDataVale data, string Product_Name)
        {
            if (data.PanelID == null || data.PanelID == "")
            {
                AlarmListBoxt.AddAlarmText(new AlarmText.alarmStruct() { Text = "Mes信息缺少SN", Name = "Mes" });
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
            ListText.Add("O" + ProjectINI.In.UserID);
            ListText.Add("L" + MesData.Line);
            ListText.Add("p" + MesData.Site);
            ListText.Add("[" + DebugCompiler.Instance.DDAxis.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
            ListText.Add("]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            string strTimed = data.EndTime.ToString(MesData.FileTimeName);
            string pathEx = RecipeCompiler.Instance.DataPaht + "//" + data.EndTime.ToString("yyyyMMdd") + ".csv";
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
                foreach (var itemd in item.Value.ResuOBj())
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
            if (ProjectINI.DebugMode)
            {
                paht = "DEBUG-" + paht;
            }
            else
            {
                ErosProjcetDLL.Excel.Npoi.WriteF(ProcessControl.ProcessUser.Instancen.ExcelPath + "//" + paht,
                    ListText, RecipeCompiler.Instance.Filet);
            }
            ErosProjcetDLL.Excel.Npoi.WriteF(RecipeCompiler.Instance.DataPaht + "\\" + DateTime.Now.ToString("yyyyMMdd") + "//" + paht,
                ListText, RecipeCompiler.Instance.Filet);
        }

        public override void WrietMes(TrayData tray, string Product_Name)
        {
            List<int> trayID = new List<int>();
            List<string> MesDatas = new List<string>();
            List<string> listResult = new List<string>();
            string sn = "";
            try
            {
                MesDatas.Add("S");
                MesDatas.Add("C" + MesData.Customer);
                MesDatas.Add("I" + MesData.DiviSion);
                MesDatas.Add("N" + MesData.Testre_Name);
                MesDatas.Add("P" + MesData.Test_Process);
                MesDatas.Add("TF");
                MesDatas.Add("O" + ProjectINI.In.UserName);
                MesDatas.Add("[" + DebugCompiler.Instance.DDAxis.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                MesDatas.Add("]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                List<string> listSn = new List<string>();
                int ErrBool = 0;
                string err = "";
                string mesErr = "";
                string[] FVTSTR = new string[] { "", "", "", "", "" };
                List<string> fvtRset = new List<string>();
                for (int i = 0; i < tray.Count; i++)
                {
                    if (tray.GetDataVales()[i].NotNull)
                    {
                        sn = tray.GetDataVales()[i].PanelID;
                        if (sn == null || sn == "")
                        {
                            AlarmListBoxt.AddAlarmText(
                                new AlarmText.alarmStruct() { Name = "SN为空", Text =
                                tray.GetDataVales()[i].TrayLocation+ "ID未读取到" });
                        }
                        else
                        {
                            fvtRset.Add("");
                            trayID.Add(tray.GetDataVales()[i].TrayLocation);
                            if (listSn.Contains(sn))
                            {
                                AlarmListBoxt.AddAlarmText(new AlarmText.alarmStruct()
                                { Name = "SN重复", Text = "SN重复请确认" + sn });
                            }
                            listSn.Add(sn);
                            if (tray.GetDataVales()[i].OK)
                            {
                                listResult.Add("Pass");
                            }
                            else
                            {
                                listResult.Add("Fail");
                            }
                        }
                    }
                }
                FVTSTR = fvtRset.ToArray();
                if (MesData.FvtReadData)
                {
                    if (!ReadFvt(trayID.ToArray(), listSn.ToArray(), listResult.ToArray(), out FVTSTR))
                    {
                        if (MesData.FvtForm)
                        {
                            FvtForm1 fvtForm1 = new FvtForm1(trayID.ToArray(), listSn.ToArray(), listResult.ToArray());
                            fvtForm1.ShowDialog();
                        }
                    }
                }
                string Err = "";
                for (int i = 0; i < tray.Count; i++)
                {
                    if (tray.GetDataVales().Count <= i)
                    {
                        AlarmText.AddTextNewLine("mes产品数量错误");
                        continue;
                    }
                    try
                    {
                        if (tray.GetDataVales()[i] != null)
                        {
                            if (tray.GetDataVales()[i].NotNull)
                            {
                                sn = tray.GetDataVales()[i].PanelID;
                                OneDataVale oneDataVale = tray.GetDataVales()[i];
                                if (sn == null || sn == "")
                                {
                                   AlarmListBoxt.AddAlarmText( new AlarmText.alarmStruct() { Name = "ID未读取", Text = "ID未读取到" });
                                }
                                else
                                {
                                    string errMesString = "null";
                                    bool resMesOK = false;
                                    for (int T = 0; T < listSn.Count; T++)
                                    {
                                        if (listSn[T] == sn)
                                        {
                                            oneDataVale.FVTStr = FVTSTR[T].TrimEnd(';');
                                            break;
                                        }
                                    }
                                    if (MesData.IsRardMes)
                                    {
                                            if (!ReadMes(sn, out errMesString))
                                            {
                                                mesErr += "位置:" + oneDataVale.TrayLocation + ";SN:" + sn + ":" + errMesString;
                                                AlarmText.AddTextNewLine("位置:" + oneDataVale.TrayLocation + ";SN:" + sn, Color.Red);
                                                resMesOK = false;
                                            }
                                            else
                                            {
                                                resMesOK = true;
                                            }
                                            oneDataVale.MesStr = errMesString;
                                    }
                                    else
                                    {
                                        resMesOK = true;
                                    }
                                    MesDatas[0] = "S" + sn;
                
                                    List<string> MesDataList = new List<string>();
                                    if (oneDataVale.OK)
                                    {
                                        MesDatas[5] = "TP";
                    
                                        listResult.Add("Pass");
                                        MesDataList.AddRange(MesDatas);
                                    }
                                    else
                                    {
                                        listResult.Add("Fail");
                                        MesDatas[5] = "TF";
                
                                        MesDataList.AddRange(MesDatas);
                                        foreach (var item in tray.GetDataVales()[i].GetAllCompOBJs().DicOnes)
                                        {
                                            if (!item.Value.aOK)
                                            {
                                                string dd = "F";
                                                if (vision.Vision.Instance.CRDNameList.Contains(item.Value.ComponentID))
                                                {
                                                    dd += item.Value.ComponentID + "-";/*+ item.Value.NGText;*/
                                                }
                                                else
                                                {
                                                    dd += "PCB - ";
                                                }
                                                string[] datas = item.Value.RestText.Split(';');
                                                if (vision.Vision.Instance.DefectTypeDicEx.ContainsKey(datas[0]))
                                                {
                                                    dd += datas[0];
                                                }
                                                else
                                                {
                                                    dd += MesData.DefaultFlaw;
                                                }
                                                if (!MesDataList.Contains(dd))
                                                {
                                                    MesDataList.Add(dd);
                                                }
                                            }
                                        }
                                        foreach (var item in tray.GetDataVales()[i].GetNGCompData().DicOnes)
                                        {
                                            if (!item.Value.aOK)
                                            {
                                                string dd = "F";
                                                if (vision.Vision.Instance.CRDNameList.Contains(item.Value.ComponentID))
                                                {
                                                    dd += item.Value.ComponentID + "-";/*+ item.Value.NGText;*/
                                                }
                                                else
                                                {
                                                    dd += "PCB - ";
                                                }
                                                if (vision.Vision.Instance.DefectTypeDicEx.ContainsKey(item.Value.RestText))
                                                {
                                                    dd += item.Value.RestText;
                                                }
                                                else
                                                {
                                                    dd += MesData.DefaultFlaw;
                                                }
                                                if (!MesDataList.Contains(dd))
                                                {
                                                    MesDataList.Add(dd);
                                                }
                                            }
                                        }
                                        bool isd = false;
                                        for (int j = 0; j < MesDataList.Count; j++)
                                        {
                                            if (MesDataList[j].StartsWith("F"))
                                            {
                                                isd = true;
                                            }
                                        }
                                        if (!isd)
                                        {
                                            MesDataList.Add("FPCB - " + MesData.DefaultFlaw);
                                        }
                                    }
                                
                                    for (int T = 0; T < listSn.Count; T++)
                                    {
                                        if (listSn[T] == sn)
                                        {
        
                                            if (FVTSTR[T].TrimEnd(';').ToLower().EndsWith("pass"))
                                            {
                                                resMesOK = true;
                                            }
                                            break;
                                        }
                                    }
                                    RecipeCompiler.AddOKNumber(oneDataVale.OK);
                                    string paht = "//" + sn + DateTime.Now.ToString(" yyyy-MM-dd-HH-mm-ss");
                                    if (resMesOK)
                                    {
                                        if (this.FaliMesRest || oneDataVale.OK)
                                        {
                                            if (MesReduplication)
                                            {
                                                if (MesSN.Count > MesLength)
                                                {
                                                    MesSN.RemoveRange(0, 10);
                                                }
                                                if (!MesSN.Contains(sn))
                                                {
                                                    MesSN.Add(sn);
                                                    ErosProjcetDLL.Excel.Npoi.WriteF(ProcessControl.ProcessUser.Instancen.ExcelPath + paht, MesDataList, RecipeCompiler.Instance.Filet);
                                                    ErosProjcetDLL.Excel.Npoi.WriteF(RecipeCompiler.Instance.DataPaht + "\\Mes记录\\" + DateTime.Now.ToString("D") + paht, MesDataList, RecipeCompiler.Instance.Filet);
                                                }
                                                else
                                                {
                                                    Err += "Mes重复:" + "位置" + oneDataVale.TrayLocation + "sn" + sn + ";";
                                                    AlarmText.AddTextNewLine("Mes重复:" + "位置" + oneDataVale.TrayLocation + "sn" + sn);
                                                }
                                            }
                                            else
                                            {
                                                ErosProjcetDLL.Excel.Npoi.WriteF(ProcessControl.ProcessUser.Instancen.ExcelPath + paht, MesDataList, RecipeCompiler.Instance.Filet);
                                                ErosProjcetDLL.Excel.Npoi.WriteF(RecipeCompiler.Instance.DataPaht + "\\Mes记录\\" + DateTime.Now.ToString("D") + paht, MesDataList, RecipeCompiler.Instance.Filet);

                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrBool++;
                        err += i + ex.Message;
                        AlarmText.AddTextNewLine("mesTray" + i + ":" + ex.StackTrace.Substring(ex.StackTrace.Length / 2 - 1, ex.StackTrace.Length / 2));
                    }
                }
                if (Err!="")
                {
                    AlarmListBoxt.AddAlarmText(new AlarmText.alarmStruct()
                    { Name = "MES重复", Text = Err });
                }
                if (MesData.IsErrMes)
                {
                    if (mesErr != "")
                    {
                        AlarmListBoxt.AddAlarmText(new AlarmText.alarmStruct()
                        { Name = "Mes查询错误", Text = mesErr });
                    }
                }
                if (ErrBool > 0)
                {
                    AlarmListBoxt.AddAlarmText (new AlarmText.alarmStruct()
                   { Name = "Mes写入Tray失败", Text = "错误数:" + ErrBool + ";" + err });
                }
            }
            catch (Exception ex)
            {
               AlarmListBoxt.AddAlarmText(new AlarmText.alarmStruct() { Name = "Mes写入Tray失败", Text = ex.Message });
               AlarmText.AddTextNewLine("mesTrya:" + ex.StackTrace.Substring(ex.StackTrace.Length / 2 - 1, ex.StackTrace.Length / 2));
            }
        }

        ///// <summary>
        ///// 写入产品CRD数据
        ///// </summary>
        ///// <param name="oneDataVale"></param>
        //public void WrietDATA(OneDataVale oneDataVale)
        //{
        //    try
        //    {
        //        string path = this.DataPaht + "\\CRD数据\\" + DateTime.Now.ToString("yyyyMMdd") + ".CSV";
        //        if (!File.Exists(path))
        //        {
        //            List<string> columnText = new List<string>() { "NO","Line", "Customer","Mode","	Defect Type","Location" ,"Serial Number","Result"   ,"Date" ,
        //                "Start Time"    ,"End Time","User", "Placement Route Step","位置","机检"};
        //            ErosProjcetDLL.Excel.Npoi.AddWriteCSV(path, columnText.ToArray());
        //        }
        //        int no = 0;
        //        foreach (var item in oneDataVale.GetAllCompOBJs().DicOnes)
        //        {
        //            no++;
        //            List<string> data = new List<string>();
        //            data.Add(no.ToString());
        //            data.Add(MesData.Line);
        //            data.Add(MesData.Customer);
        //            data.Add(oneDataVale.Product_Name);
        //            data.Add(item.Value.RestText);
        //            data.Add(item.Value.ComponentID);
        //            data.Add(oneDataVale.PanelID);
        //            if (item.Value.aOK)
        //            {
        //                data.Add("Pass");
        //            }
        //            else
        //            {
        //                data.Add("Fail");
        //            }
        //            data.Add(DateTime.Now.ToString("d"));
        //            data.Add(oneDataVale.StrTime.ToString("T"));
        //            data.Add(oneDataVale.EndTime.ToString("T"));
        //            data.Add(ErosProjcetDLL.Project.ProjectINI.In.UserName);
        //            data.Add(DebugCompiler.Instance.DeviceNameText);
        //            data.Add(oneDataVale.TrayLocation.ToString());
        //            data.Add(item.Value.NGText);
        //            ErosProjcetDLL.Excel.Npoi.AddWriteCSV(path,
        //              data.ToArray());
        //        }
        //    }
        //    catch (Exception) { }
        //}

        public override void WrietMesAll<T>(T datas, string Product_Name)
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
                AlarmListBoxt.AddAlarmText(new AlarmText.alarmStruct() { Name = "Mes写入失败", Text = ex.Message });
            }
        }

        public override Form GetForm()
        {
            return new JabilForm(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="trayID"></param>
        /// <param name="sn"></param>
        /// <param name="resetStr"></param>
        /// <param name="fVTsTR"></param>
        /// <returns></returns>

        public bool ReadFvt(int[] trayID, string[] sn, string[] resetStr, out string[] fVTsTR)
        {
            fVTsTR = new string[trayID.Length];
            string[] REST = new string[trayID.Length];
            for (int i = 0; i < trayID.Length; i++)
            {
                REST[i] = "Empty";
                fVTsTR[i] = "";
            }
            List<string> TextWeit = new List<string>();
            for (int i = 0; i < trayID.Length; i++)
            {
                TextWeit.Add("No:" + trayID[i] + ";SN:" + sn[i] + ";AMVI Result:");
            }
            try
            {
                if (sn.Length == 0)
                {
                    AlarmText.AddTextNewLine("FVT:码数量为0");
                    return false;
                }
                string fileName = sn[0];
                string PathFvt = MesData.FVTDataPath + "\\" + fileName + ".txt";
            
                Directory.CreateDirectory(RecipeCompiler.Instance.DataPaht + "\\FVT\\" + DateTime.Now.ToString("D"));
                if (MesData.FvtSimulateDATA)
                {
                    for (int i = 0; i < sn.Length; i++)
                    {
                        TextWeit[i] += resetStr[i] + ";";
                    }
                    File.WriteAllLines(PathFvt, TextWeit);
                    string Path = RecipeCompiler.Instance.DataPaht + "\\FVT\\" + DateTime.Now.ToString("D") + "\\" + fileName + ".txt";
                    File.WriteAllLines(Path, TextWeit);
                    AlarmText.AddTextNewLine("FVT写入完成:" + PathFvt);
                    return true;
                }
                List<bool> RsetOk = new List<bool>();
                for (int i = 0; i < trayID.Length; i++)
                {
                    RsetOk.Add(false);
                }
                string[] fileS = Directory.GetFiles(MesData.FVTPath, "*.txt");
                List<string> snTem = new List<string>();
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
                                if (strDatas[id].Contains(sn[j]))
                                {
                                    RsetOk[j] = true;
                                    fVTsTR[j] = strDatas[id];
                                    if (strDatas[id].ToLower().Contains("empty"))
                                    {
                                        resetStr[j] = "Empty";
                                    }
                                    if (strDatas[id].ToLower().Contains("fail"))
                                    {
                                        resetStr[j] = "Fail";
                                    }
                                    if (strDatas[id].ToLower().Contains("other"))
                                    {
                                        resetStr[j] = "Other";
                                    }
                                    strDatas[id] += "AMVI result:" + resetStr[j] + ";";
                                    REST[j] = resetStr[j];
                                    break;
                                }
                            }
                        }
                    }
                    if (RsetOk.Contains(true))
                    {
                        for (int J = 0; J < TextWeit.Count; J++)
                        {
                            TextWeit[J] += REST[J] + ";";
                            snTem.Add(sn[J] + "=" + REST[J]);
                        }
                        //Directory.CreateDirectory(MesData.FVTDataPath + "\\");
                        File.WriteAllLines(PathFvt, TextWeit);
                        string path = RecipeCompiler.Instance.DataPaht + "\\FVT\\" + DateTime.Now.ToString("yyyy年M月d日") + "\\" + fileName + ".txt";
                        File.WriteAllLines(path, TextWeit);
                        if (MesData.IsDelete)
                        {
                            if (File.Exists(fileS[i]))
                            {
                                string pathds = MesData.FVTPath + "\\历史\\" + DateTime.Now.ToString("yyyy年M月d日") + "\\" + Path.GetFileName(fileS[i]);
                                if (File.Exists(pathds))
                                {
                                    File.Delete(pathds);
                                }
                                Directory.CreateDirectory(MesData.FVTPath + "\\历史\\" + DateTime.Now.ToString("yyyy年M月d日"));
                                File.Move(fileS[i], pathds);
                            }
                        }
                        string mest = "F:";
                        for (int j = 0; j < snTem.Count; j++)
                        {
                            mest += snTem[j] + ";";
                        }
                         AlarmText.AddTextNewLine(mest);
                        if (File.Exists(PathFvt))
                        {
                            AlarmText.AddTextNewLine(PathFvt);
                        }
                        else
                        {
                            AlarmText.AddTextNewLine("写入失败：" + PathFvt);
                            return false;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                AlarmText.AddTextNewLine("FVT错误信息:" + ex.Message);
               AlarmText.AddTextNewLine("FVT错误:" + ex.StackTrace);
            }
            AlarmListBoxt.AddAlarmText(new AlarmText.alarmStruct()
            { Name = "FVT", Text = "FVT读取失败" });
            return false;
        }

        /// <summary>
        /// 模拟FVTPass
        /// </summary>
        /// <param name="trayID"></param>
        /// <param name="sn"></param>
        /// <param name="resetStr"></param>
        /// <returns></returns>
        public bool WriteFatData(int[] trayID, string[] sn, string[] resetStr)
        {
            List<string> TextWeit = new List<string>();
            try
            {
                string fileName = sn[0];
                string PathFvt = MesData.FVTDataPath + "\\" + fileName + ".txt";
                Directory.CreateDirectory(MesData.FVTDataPath);
                Directory.CreateDirectory(RecipeCompiler.Instance.DataPaht + "\\FVT\\" + DateTime.Now.ToString("D"));
                List<string> snTem = new List<string>();
                for (int i = 0; i < sn.Length; i++)
                {
                    TextWeit.Add("No:" + trayID[i] + ";SN:" + sn[i] + ";AMVI result:" + resetStr[i]);
                    snTem.Add(sn[i] + "=" + resetStr[i]);
                }
                File.WriteAllLines(PathFvt, TextWeit);
                string paths = RecipeCompiler.Instance.DataPaht + "\\FVT\\" + DateTime.Now.ToString("D") + "\\" + Path.GetFileName(fileName) + ".txt";
                File.WriteAllLines(paths, TextWeit);
                string mest = "F强制写入:";
                for (int j = 0; j < snTem.Count; j++)
                {
                    mest += snTem[j] + ";";
                }
                AlarmText.AddTextNewLine(mest);
                return true;
            }
            catch (Exception ex)
            {
               AlarmText.AddTextNewLine("FVT错误:" + ex.Message);
               AlarmText.AddTextNewLine("FVT错误:" + ex.StackTrace);
            }
            return false;
        }
    }

    public class MesData
    {
        public MesData()
        {
            Testre_Name = Dns.GetHostName();
        }

        public int MesIntd { get; set; } = 1;


        [Description("文件名明名时间规则"), Category("文件名称"), DisplayName("文件名时间规则")]
        public string FileTimeName { get; set; } = "yyyy-MM-dd";//(MM-dd-yyyy HH-mm-ss)

        /// <summary>
        /// 客户名
        /// </summary>
        [Description("客户名Customer"), Category("Mes信息"), DisplayName("C-客户名")]
        public string Customer { get; set; } = "Customer";

        [Description("默认写入缺陷"), Category("缺陷管理"), DisplayName("默认缺陷")]
        public string DefaultFlaw { get; set; } = "MISSING";

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
        [Description(""), Category("FVT信息"), DisplayName("FVT2读取数据地址")]
        /// <summary>
        ///FVT读取数据地址
        /// </summary>
        public string FVTPath2 { get; set; } = "";

        [Description(""), Category("FVT信息"), DisplayName("FVT弹出对话框")]
        public bool FvtForm { get; set; }

        [Description(""), Category("FVT信息"), DisplayName("是否删除读取记录")]
        public bool IsDelete { get; set; }

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


        [Description(""), Category("Mes查询"), DisplayName("Mes错误报警")]
        /// <summary>
        /// 是否
        /// </summary>
        public bool IsErrMes { get; set; } = true;
    }
}