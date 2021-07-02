using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Vision2.Project.formula;
using Vision2.Project.Mes;
using Vision2.vision.HalconRunFile.RunProgramFile;
using  ErosSocket.DebugPLC.Robot;
using Vision2.vision;
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
         HWindID HWi ;

        public void RaeaTary(string filePaht, string text, double outTime, out string runErr)
        {
            runErr = "";

            string ReadTextPath = "";
            int Strtrow = 0;
            int StrtCol = 0;
            string rset = "";
            char Sipe = ',';
            string StartsWith = "";
            //string filePaht = ProcessControl.ProcessUser.GetThis().ExcelPath + "\\";

            //   AwaitRead 5, Tray 2 { 行=2; 分割==;数据列=1; 结果=OK; StartsWith=SPUTTERING_Result_; }
            if (text.Contains("{"))
            {
                string dtat = text.Substring(text.IndexOf('{') + 1, text.IndexOf('}') - text.IndexOf('{') - 1);
                if (dtat.Contains(";"))
                {
                    string[] dtastTd = dtat.Split(';');
                    for (int i = 0; i < dtastTd.Length; i++)
                    {
                        if (dtastTd[i].Contains("="))
                        {
                            string[] dtast = dtastTd[i].Split('=');
                            if (dtast[0] == "行")
                            {
                                Strtrow = int.Parse(dtast[1]);
                            }
                            else if (dtast[0] == "分割")
                            {
                                if (dtast.Length == 3)
                                {
                                    Sipe = '=';
                                }
                                else
                                {
                                    Sipe = dtast[1].Trim()[0];
                                }

                            }
                            else if (dtast[0] == "数据列")
                            {
                                StrtCol = int.Parse(dtast[1]);
                            }
                            else if (dtast[0] == "结果")
                            {
                                rset = dtast[1];
                            }
                            else if (dtast[0] == "StartsWith")
                            {
                                StartsWith = dtast[1];
                            }
                        }
                    }
                }
            }
            bool Done = false;
            System.Diagnostics.Stopwatch Watch = new System.Diagnostics.Stopwatch();

            while (true)
            {

                if (StartsWith != "" && System.IO.Directory.Exists(filePaht))
                {
                    string[] Pahts = System.IO.Directory.GetFiles(filePaht);
                    for (int i = 0; i < Pahts.Length; i++)
                    {
                        if (System.IO.Path.GetFileNameWithoutExtension(Pahts[i]).StartsWith(StartsWith))
                        {
                            if (Vision2.ErosProjcetDLL.Excel.Npoi.ReadText(Pahts[i], out List<string> textT))
                            {
                                List<bool> ListR = new List<bool>();
                                string err = "";
                                if (textT.Count >= 1)
                                {
                                    if (!textT[0].Contains("OK"))
                                    {
                                        err = textT[0];
                                    }
                                }
                                for (int i2 = Strtrow; i2 < textT.Count; i2++)
                                {
                                    if (textT[i2].Contains(Sipe) && textT[i2].Split(Sipe)[StrtCol] == rset)
                                    {
                                        ListR.Add(true);
                                    }
                                    else
                                    {
                                        err += i2 + ":" + textT[i2];
                                        ListR.Add(false);
                                    }
                                }
                                if (err != "")
                                {
                                    UserFormulaContrsl.SetOK(2);
                                    simulateQRForm.ShowMesabe(err);
                                }
                                else
                                {
                                    UserFormulaContrsl.SetOK(3);
                                }
                                DebugCompiler.GetTrayDataUserControl().SetValue(ListR);
                                System.IO.Directory.CreateDirectory(ProcessControl.ProcessUser.GetThis().ExcelPath + "\\历史记录\\");
                                System.IO.File.Move(Pahts[i], ProcessControl.ProcessUser.GetThis().ExcelPath + "\\历史记录\\" + System.IO.Path.GetFileName(Pahts[i]));
                                Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("SIFS过站完成" + textT[0], Color.Green);
                                Done = true;
                            }
                        }
                    }
                }

                //else if (DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(tdat[2].Trim(' ')))
                //{
                //    if (Vision2.ErosProjcetDLL.Excel.Npoi.ReadText(ReadTextPath, out string textT))
                //    {
                //        DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[tdat[2]].Value = textT;
                //        break;
                //    }
                //}
                if (Done)
                {
                    break;
                }
                if (outTime != 0 && outTime <= Watch.ElapsedMilliseconds / 1000)
                {
                    runErr += "未找到目标文件";
                    break;
                }
                System.Threading.Thread.Sleep(10);

            }
        }


        public void WriatTary(string filePaht, string text, TrayData tray, out string runErr)
        {
            runErr = "";
            List<string> Datas = new List<string>();
            try
            {
                //文件名规则
                //  日期+字符+托盘ID  =[关键字(托盘ID，和日期时间)]字符[关键字托盘(trayid)]

                string FileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                int trype = 0;
                if (text.Contains("{"))
                {
                    string dtat = text.Substring(text.IndexOf('{') + 1, text.IndexOf('}') - text.IndexOf('{') - 1);
                    if (dtat.Contains(";"))
                    {
                        string[] dtastTd = dtat.Split(';');
                        for (int i = 0; i < dtastTd.Length; i++)
                        {
                            if (dtastTd[i].Contains('='))
                            {
                                string[] dtast = dtastTd[i].Split('=');
                                if (dtast[0] == "文件名")
                                {
                                    while (dtast[1].Contains('['))
                                    {
                                        int stR = dtast[1].IndexOf('[');
                                        string dtattd = dtast[1].Substring(dtast[1].IndexOf('[') + 1, dtast[1].IndexOf(']') - dtast[1].IndexOf('[') - 1);
                                        if (dtattd.ToLower() == "trayid")
                                        {
                                            dtattd = tray.TrayIDQR;
                                            dtast[1] = dtast[1].Remove(dtast[1].IndexOf('['), dtast[1].IndexOf(']') - dtast[1].IndexOf('[') + 1);
                                            dtast[1] = dtast[1].Insert(stR, dtattd);
                                        }
                                        else if (dtattd.ToLower() == "newtime")
                                        {
                                            dtattd = DateTime.Now.ToString("yyyyMMddHHmmss");
                                            dtast[1] = dtast[1].Remove(dtast[1].IndexOf('['), dtast[1].IndexOf(']') - dtast[1].IndexOf('[') + 1);
                                            dtast[1] = dtast[1].Insert(stR, dtattd);
                                        }
                                        else if (dtattd.ToLower().StartsWith("trayid "))
                                        {
                                            string Itmess = tray.GetDataVales()[RunCodeStr.ToDoubleP(dtattd.Split(' ')[1])].ID;

                                            if (Itmess == null)
                                            {
                                                Itmess = tray.GetDataVales()[RunCodeStr.ToDoubleP(dtattd.Split(' ')[1]) + 1].ID;
                                            }
                                            dtast[1] = dtast[1].Remove(dtast[1].IndexOf('['), dtast[1].IndexOf(']') - dtast[1].IndexOf('[') + 1);
                                            dtast[1] = dtast[1].Insert(stR, Itmess);
                                        }
                                        else if (dtattd.ToLower().StartsWith("newdata"))
                                        {
                                            string Itmess = tray.GetDataVales()[RunCodeStr.ToDoubleP(dtattd.Split(' ')[1])].ID;

                                            if (Itmess == null)
                                            {
                                                Itmess = tray.GetDataVales()[RunCodeStr.ToDoubleP(dtattd.Split(' ')[1]) + 1].ID;
                                            }
                                            dtast[1] = dtast[1].Remove(dtast[1].IndexOf('['), dtast[1].IndexOf(']') - dtast[1].IndexOf('[') + 1);
                                            dtast[1] = dtast[1].Insert(stR, Itmess);
                                        }
                                    }
                                    FileName = dtast[1].Trim();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dtat.Contains('='))
                        {
                            string[] dtast = dtat.Split('=');
                            if (dtast[0] == "文件名")
                            {
                                while (dtast[1].Contains('['))
                                {
                                    int stR = dtast[1].IndexOf('[');
                                    string dtattd = dtast[1].Substring(dtast[1].IndexOf('[') + 1, dtast[1].IndexOf(']') - dtast[1].IndexOf('[') - 1);
                                    if (dtattd.ToLower() == "trayid")
                                    {
                                        dtattd = tray.TrayIDQR;
                                        dtast[1] = dtast[1].Remove(dtast[1].IndexOf('['), dtast[1].IndexOf(']') - dtast[1].IndexOf('[') + 1);
                                        dtast[1] = dtast[1].Insert(stR, dtattd);
                                    }
                                    else if (dtattd.ToLower() == "newtime")
                                    {
                                        dtattd = DateTime.Now.ToString("HHmmss");
                                        dtast[1] = dtast[1].Remove(dtast[1].IndexOf('['), dtast[1].IndexOf(']') - dtast[1].IndexOf('[') + 1);
                                        dtast[1] = dtast[1].Insert(stR, dtattd);
                                    }
                                    else if (dtattd.ToLower().StartsWith("trayid "))
                                    {
                                        string Itmess = tray.GetDataVales()[RunCodeStr.ToDoubleP(dtattd.Split(' ')[1])].ID;
                                        if (Itmess == null)
                                        {
                                            Itmess = tray.GetDataVales()[RunCodeStr.ToDoubleP(dtattd.Split(' ')[1]) + 1].ID;
                                        }
                                        dtast[1] = dtast[1].Remove(dtast[1].IndexOf('['), dtast[1].IndexOf(']') - dtast[1].IndexOf('[') + 1);
                                        dtast[1] = dtast[1].Insert(stR, Itmess);
                                    }
                                    else if (dtattd.ToLower().StartsWith("newdata"))
                                    {
                                        string Itmess = DateTime.Now.ToString("yyyyMMdd");
                                        if (Itmess == null)
                                        {
                                            Itmess = tray.GetDataVales()[RunCodeStr.ToDoubleP(dtattd.Split(' ')[1]) + 1].ID;
                                        }
                                        dtast[1] = dtast[1].Remove(dtast[1].IndexOf('['), dtast[1].IndexOf(']') - dtast[1].IndexOf('[') + 1);
                                        dtast[1] = dtast[1].Insert(stR, Itmess);
                                    }
                                }
                                FileName = dtast[1].Trim();
                            }
                        }
                    }

                }
                Datas.Add(ProcessControl.ProcessUser.GetThis().CarrierQRIDName + ProcessControl.ProcessUser.GetThis().Split_Symbol +
                    tray.TrayIDQR + Environment.NewLine);
                int deNumber = 0;

                bool Err = false;
                string ErrString = "";

                List<string> list1 = new List<string>();
                for (int i = 0; i < tray.GetDataVales().Count; i++)
                {
                    if (tray.GetDataVales()[i] == null)
                    {
                        continue;
                    }
                    if (tray.GetDataVales()[i].PanelID == null)
                    {
                        continue;
                    }
                    list1.Add(tray.GetDataVales()[i].PanelID);
                }
                HTuple[] vs = new HTuple[tray.GetDataVales().Count];
                for (int i = 0; i < tray.GetDataVales().Count; i++)
                {

                    if (tray.GetDataVales()[i] == null)
                    {
                        continue;
                    }
                    List<double> objt = new List<double>();
                    objt.Add((i + 1));
                    string objtStr = "";

                    if (tray.GetDataVales()[i].PanelID != null)
                    {
                        deNumber++;
                        objtStr = tray.GetDataVales()[i].PanelID;
                        list1[i] = "";
                        if (list1.Contains(objtStr))
                        {
                            ErrString += (i + 1) + ":" + objtStr + ";";
                            Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("重复信息:" + objtStr);
                        }
                        else
                        {
                            Datas.Add(ProcessControl.ProcessUser.GetThis().SN_Name + (deNumber) + ProcessControl.ProcessUser.GetThis().Split_Symbol + objtStr + Environment.NewLine);
                        }
                        list1[i] = objtStr;
                    }
                    List<double> objtT = tray.GetDataVales()[i].Data as List<double>;
                    if (objtT != null)
                    {
                        objt.AddRange(objtT);
                        if (vs == null)
                        {
                            vs = new HTuple[objt.Count];
                        }
                        List<string> vs2 = new List<string>();
                        vs2.Add("托盘位号");
                        for (int i2 = 0; i2 < objt.Count; i2++)
                        {
                            if (vs[i2] == null)
                            {
                                vs[i2] = new HTuple();
                            }
                            if (i2 != objt.Count - 1)
                            {
                                vs2.Add("p" + (i2 + 1));
                            }

                            vs[i2].Append(objt[i2]);
                        }

                        if (trype != 1)
                        {
                            trype = 1;
                            Vision2.ErosProjcetDLL.Excel.Npoi.AddWriteColumnToExcel(ProcessControl.ProcessUser.GetThis().ExcelPath + "\\" + FileName, "托盘", vs2.ToArray());
                        }
                        Vision2.ErosProjcetDLL.Excel.Npoi.AddRosWriteToExcel(ProcessControl.ProcessUser.GetThis().ExcelPath + "\\" + FileName, "托盘", objt.ToArray());
                    }
                    else if (tray.GetDataVales()[i].Data.Count == 1)
                    {
                        double objtDouble = tray.GetDataVales()[i].Data[0];
                        objtStr += objtDouble;
                        Datas.Add(ProcessControl.ProcessUser.GetThis().SN_Name + (deNumber) + ProcessControl.ProcessUser.GetThis().Split_Symbol + objtStr + Environment.NewLine);
                    }
                }
                if (trype == 1)
                {
                    HTuple max = new HTuple(new HTuple("Max"));
                    HTuple min = new HTuple(new HTuple("Min"));
                    HTuple metw = new HTuple(new HTuple("差"));
                    for (int i = 1; i < vs.Length; i++)
                    {
                        if (vs[i] != null)
                        {
                            max.Append(vs[i].TupleMax());
                            min.Append(vs[i].TupleMin());
                            metw.Append(vs[i].TupleMax() - vs[i].TupleMin());
                        }
                    }
                    ErosProjcetDLL.Excel.Npoi.AddRosWriteToExcel(ProcessControl.ProcessUser.GetThis().ExcelPath + "\\" + FileName, "托盘", max.ToSArr());
                    ErosProjcetDLL.Excel.Npoi.AddRosWriteToExcel(ProcessControl.ProcessUser.GetThis().ExcelPath + "\\" + FileName, "托盘", min.ToSArr());
                    ErosProjcetDLL.Excel.Npoi.AddRosWriteToExcel(ProcessControl.ProcessUser.GetThis().ExcelPath + "\\" + FileName, "托盘", metw.ToSArr());
                }
                else
                {
                    if (Err)
                    {
                        UserFormulaContrsl.SetOK(2);
                        simulateQRForm.ShowMesabe("托盘码重复:" + ErrString);
                    }
                    else
                    {
                        Vision2.ErosProjcetDLL.Excel.Npoi.AddText(ProcessControl.ProcessUser.GetThis().ExcelPath + "\\" + FileName, Datas.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }

        public void SetTray(TrayRobot trayRobot)
        {
            if (tray != trayRobot)
            {
                tray = trayRobot;
            }
            tray.AddTary(this);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    AddRow();
                }));
            }
            else
            {
                AddRow();
            }
        }
        public void SetTray(int trayRobot)
        {
            try
            {
                tray = DebugCompiler.GetThis().DDAxis.ListTray[trayRobot];
                tray.AddTary(this);
                this.Invoke(new Action(() =>
                {
                        AddRow();
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
                return tray;
            }
            return DebugCompiler.GetThis().DDAxis.ListTray[number];
        }
        static double MinV = 00;
        static double MaxV = 2;

        static TrayRobot tray;
        public static TrayRobot GetTray(int ids = -1)
        {
            if (ids >= 0)
            {
                return DebugCompiler.GetThis().DDAxis.GetTrayInxt(ids);
            }

            return tray;
        }
        public static void SetStaticTray(TrayRobot trayRobot)
        {
            tray = trayRobot;
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    for (int i = 0; i < tray.Count; i++)
                    {
                        if (DebugCompiler.GetTrayDataUserControl().Controls.Find((i + 1).ToString(), false).Length != 0)
                        {
                            TrayControl label1 = DebugCompiler.GetTrayDataUserControl().Controls.Find((i + 1).ToString(), false)[0] as TrayControl;

                            if (label1 != null)
                            {
                                label1.label1.Text = (i + 1).ToString();

                                if (tray.GetTrayData().GetDataVales()[i] == null)
                                {
                                    tray.GetTrayData().GetDataVales()[i] = new OneDataVale();
                                }
                                label1.label1.Text += "SN:" + tray.GetTrayData().GetDataVales()[i].PanelID;
                                if (tray.GetTrayData().GetDataVales()[i].OK)
                                {
                                    RecipeCompiler.AddOKNumber(i, true);
                                    if (ErosSocket.DebugPLC.DebugComp.GetThis().PalenID)
                                    {
                                        if (tray.GetTrayData().GetDataVales()[tray.Number - 1].PanelID == null || tray.GetTrayData().GetDataVales()[tray.Number - 1].PanelID == "")
                                        {
                                            tray.GetTrayData().GetDataVales()[tray.Number - 1].OK = false;
                                            label1.label1.BackColor = Color.GreenYellow;
                                        }
                                        else
                                        {
                                            tray.GetTrayData().GetDataVales()[tray.Number - 1].OK = true;
                                            label1.label1.BackColor = Color.Green;
                                        }
                                    }
                                    else
                                    {
                                        tray.GetTrayData().GetDataVales()[tray.Number - 1].OK = true;
                                        label1.label1.BackColor = Color.Green;
                                    }
                                }
                                else
                                {
                                    RecipeCompiler.AddOKNumber(i, false);
                                    label1.label1.BackColor = Color.Red;
                                }
                                if (tray.GetTrayData().GetDataVales()[tray.Number - 1] == null)
                                {
                                    tray.GetTrayData().GetDataVales()[tray.Number - 1] = new OneDataVale();
                                }
                                label1.Tag = tray.GetTrayData().GetDataVales()[tray.Number - 1];
                            }
                        }


                    }

                }));
            }
            catch (Exception ex)
            {
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
                            label1.label1.Text = number.ToString();
                            if (tray.GetTrayData().GetDataVales()[number - 1]!=null)
                            {
                                label1.label1.Text += "SN:" + tray.GetTrayData().GetDataVales()[number - 1].PanelID;
                            }
                            if (valueDouble != null)
                            {
                                label1.label1.Text += "/" + valueDouble;
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
                    if (tray.GetTrayData().GetDataVales()[number - 1] == null)
                    {
                        tray.GetTrayData().GetDataVales()[number - 1] = new OneDataVale();
                    }
                    if (tray.GetTrayData().GetDataVales()[number - 1].Data is List<double>)
                    {
                        List<double> dset = tray.GetTrayData().GetDataVales()[number - 1].Data as List<double>;
                        dset[0] = valueDouble;
                    }
                    if (label1 != null)
                    {

                        if (valueDouble < MinV || valueDouble > MaxV)
                        {
                            label1.label1.BackColor = Color.Red;
                            tray.GetTrayData().GetDataVales()[number - 1].OK = false;
                            formula.RecipeCompiler.AddOKNumber(false);
                        }
                        else
                        {
                            if (tray.GetTrayData().GetDataVales()[number - 1].OK)
                            {
                                tray.GetTrayData().GetDataVales()[number - 1].OK = true;
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
        public void SetValue(int number,string sn)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {

                    TrayControl label1 = this.Controls.Find(number.ToString(), false)[0] as TrayControl;
                    if (label1 != null)
                    {
                        label1.label1.Text = number+"SN:" +sn;

                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }
        public void SetValue(double valueDouble)
        {
            SetValue(tray.Number, valueDouble);
        }
        public void SetValue(List<double> values)
        {
            try
            {
                RecipeCompiler.Instance.Data.AddData(values);
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    if (this.Controls.Find(tray.Number.ToString(), false).Length != 0)
                    {
                        TrayControl label1 = this.Controls.Find(tray.Number.ToString(), false)[0] as TrayControl;
                        if (tray.GetTrayData().GetDataVales()[tray.Number - 1] == null)
                        {
                            tray.GetTrayData().GetDataVales()[tray.Number - 1] = new OneDataVale();
                        }
                        if (label1 != null)
                        {
                            label1.label1.Text = tray.Number.ToString() + "SN:" + tray.GetTrayData().GetDataVales()[tray.Number - 1].PanelID +Environment.NewLine;
                            tray.GetTrayData().GetDataVales()[tray.Number - 1].Data = values;
                            label1.Tag = tray.GetTrayData().GetDataVales()[tray.Number - 1];
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
                    if (this.Controls.Find(tray.Number.ToString(), false).Length != 0)
                    {
                        TrayControl label1 = this.Controls.Find(tray.Number.ToString(), false)[0] as TrayControl;
                        if (tray.GetTrayData().GetDataVales()[tray.Number - 1] == null)
                        {
                            tray.GetTrayData().GetDataVales()[tray.Number - 1] = new OneDataVale();
                        }
                        if (label1 != null)
                        {
                            label1.label1.Text = tray.Number.ToString() + "SN:" + tray.GetTrayData().GetDataVales()[tray.Number - 1].PanelID + Environment.NewLine;
                            for (int i = 0; i < values.Count; i++)
                            {
                                //tray.GetDataVales()[tray.Number - 1].Data1.Add(new MaxMinValue()
                                //{
                                //    Value = double.Parse( values[i])
                                //});
                            }
                            bool OKt = true;
                            //if (tray.GetDataVales()[tray.Number - 1].Data1.Count >= RecipeCompiler.Instance.Data.ListDatV.Count)
                            //{
                            //    for (int i = 0; i < tray.GetDataVales()[tray.Number - 1].Data1.Count; i++)
                            //    {
                            //        if (i >= RecipeCompiler.Instance.Data.ListDatV.Count)
                            //        {
                            //            break;
                            //        }
                            //        tray.GetDataVales()[tray.Number - 1].Data1[i] = RecipeCompiler.Instance.Data.GetMaxMinValue(i);
                            //        if (!RecipeCompiler.Instance.Data.GetChet(i))
                            //        {
                            //            OKt = false;
                            //        }
                            //    }
                            //    if (OKt)
                            //    {
                            //        label1.label1.Text += "数据:OK";
                            //    }
                            //    else
                            //    {
                            //        label1.label1.Text += "数据:NG";
                            //    }
                            //    if (tray.GetDataVales()[tray.Number - 1].OK)
                            //    {
                            //        label1.label1.BackColor = Color.Green;
                            //    }
                            //    else
                            //    {
                            //        label1.label1.BackColor = Color.Red;
                            //    }
                            //}
                            //tray.GetDataVales()[tray.Number - 1].Data = values;
                            label1.Tag = tray.GetTrayData().GetDataVales()[tray.Number - 1];
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void SetComponentData(int index,List<string> datStrs)
        {
            RecipeCompiler.Instance.Data.AddData(index,datStrs);
        }

        public void SetID(List<string> values)
        {
            try
            {
                MainForm1.MainFormF.Invoke(new Action(() =>
                {
                    if (this.Controls.Find(tray.Number.ToString(), false).Length != 0)
                    {
                        TrayControl label1 = this.Controls.Find(tray.Number.ToString(), false)[0] as TrayControl;
                        if (tray.GetTrayData().GetDataVales()[tray.Number - 1] == null)
                        {
                            tray.GetTrayData().GetDataVales()[tray.Number - 1] = new OneDataVale();
                        }
                        if (label1 != null)
                        {
                            //tray.GetDataVales()[tray.Number - 1].Data1.Add(new MaxMinValue()
                            //{

                            //});
                            label1.label1.Text = tray.Number.ToString() + "SN:" + tray.GetTrayData().GetDataVales()[tray.Number - 1].PanelID;
                            label1.Tag = tray.GetTrayData().GetDataVales()[tray.Number - 1];
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
                                if (tray.GetTrayData().GetDataVales()[idS[i] - 1] == null)
                                {
                                    tray.GetTrayData().GetDataVales()[idS[i] - 1] = new OneDataVale();
                                }
                                tray.GetTrayData().GetDataVales()[idS[i] - 1].PanelID = values[i];

                                if (tray.GetTrayData().GetDataVales()[idS[i] - 1] != null)
                                {
                                    if (tray.GetTrayData().GetDataVales()[idS[i] - 1].PanelID != null)
                                    {
                                        DAT += "SN:" + tray.GetTrayData().GetDataVales()[idS[i] - 1].PanelID + Environment.NewLine;
                                        //tray.dataObjs[idS[i] - 1].OK = false;
                                    }
                                    else
                                    {
                                        tray.GetTrayData().GetDataVales()[idS[i] - 1].OK = false;
                                    }
                                    if (tray.GetTrayData().GetDataVales()[idS[i] - 1].Data != null)
                                    {
                                        DAT += "数据:" + tray.GetTrayData().GetDataVales()[idS[i] - 1].Data;
                                    }
                                    label1.label1.Text = DAT;
                                }
                                else
                                {
                                    tray.GetTrayData().GetDataVales()[idS[i] - 1].OK = false;
                                }
                                label1.Tag = tray.GetTrayData().GetDataVales()[idS[i] - 1];

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
                                if (tray.GetTrayData().GetDataVales()[tryaid[i] - 1] == null)
                                {
                                    tray.GetTrayData().GetDataVales()[tryaid[i] - 1] = new OneDataVale();
                                }
                                tray.GetTrayData().GetDataVales()[tryaid[i] - 1].PanelID = listSN[i];

                                if (tray.GetTrayData().GetDataVales()[tryaid[i] - 1] != null)
                                {
                                    if (tray.GetTrayData().GetDataVales()[tryaid[i] - 1].PanelID != null)
                                    {
                                        DAT += "SN:" + tray.GetTrayData().GetDataVales()[tryaid[i] - 1].PanelID + Environment.NewLine;
                                        //tray.dataObjs[idS[i] - 1].OK = false;
                                    }
                                    else
                                    {
                                        tray.GetTrayData().GetDataVales()[tryaid[i] - 1].OK = false;
                                        tray.GetTrayData().GetDataVales()[tryaid[i] - 1].OK = false;
                                    }
                                    if (tray.GetTrayData().GetDataVales()[tryaid[i] - 1].Data != null)
                                    {
                                        DAT += "数据:" + tray.GetTrayData().GetDataVales()[tryaid[i] - 1].Data;
                                    }
                                    label1.label1.Text = DAT;
                                }
                                else
                                {
                                    tray.GetTrayData().GetDataVales()[tryaid[i] - 1].OK = false;
                                }
                                label1.Tag = tray.GetTrayData().GetDataVales()[tryaid[i] - 1];

                            }
                        }
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
                        if (this.Controls.Find((i + 1).ToString(), false).Length != 0)
                        {
                            TrayControl label1 = this.Controls.Find((i + 1).ToString(), false)[0] as TrayControl;
                            if (label1 != null)
                            {
                                if (values[i])
                                {
                                    label1.label1.BackColor = Color.Green;
                                }
                                else
                                {

                                    label1.label1.BackColor = Color.Red;
                                }
                                //label1.Tag = values[i];
                                //tray.OBJ[i] = values[i];
                            }
                            else
                            {

                            }
                        }
                        else
                        {

                        }
                    }
                }));
            }
            catch (Exception ex)
            {
            }
        }
        public void SetNumber(int number)
        {
            tray.Number = number;
        }
        public void RestValue()
        {

            if (tray != null)
            {
                tray.Number = 1;
                tray.GetTrayData().GetDataVales().Clear();
                tray.GetTrayData().Clear();
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    TrayControl control = this.Controls[i] as TrayControl;
                    if (control != null)
                    {
                        control.checkBox1.Checked = false;
                        //control.BackColor = Color.White;
                        control.label1.Text = control.label1.Name;
                        control.label1.BackColor= Color.White;
                        control.Tag = null;
                    }
                }
            }

        }
        private void AddRow()
        {
            try
            {
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
                if (tray.GetTrayData().GetDataVales() == null)
                {
                    tray.GetTrayData().Clear();
                }
                //tray.GetTrayData().GetDataVales().Clear();
                //tray.GetTrayData().GetDataVales().AddRange(new DataVale[rows * columnCount]);

                for (int i = 0; i < rows * columnCount; i++)
                {
                    TrayControl trayControl = new TrayControl();

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
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = (dt * columnCount + sd + 1).ToString();
                                break;
                            case ErosSocket.DebugPLC.Robot.TrayRobot.TrayDirectionEnum.右上:
                                //trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = ((rows * (sd + 1)) - dt).ToString();
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = (rows * columnCount - (columnCount * (dt + 1)) + sd + 1).ToString();
                                break;
                            case ErosSocket.DebugPLC.Robot.TrayRobot.TrayDirectionEnum.左下:
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = ((dt + 1) * columnCount - sd).ToString();
                                break;
                            case ErosSocket.DebugPLC.Robot.TrayRobot.TrayDirectionEnum.右下:
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = (rows * columnCount - (dt) * columnCount - sd).ToString();
                                break;
                        }
                    }
                    else
                    {
                        sd = i / columnCount;
                        dt = i % columnCount;
                        switch (tray.TrayDirection)
                        {
                            case ErosSocket.DebugPLC.Robot.TrayRobot.TrayDirectionEnum.左上:
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = (i + 1).ToString();
                                break;
                            case ErosSocket.DebugPLC.Robot.TrayRobot.TrayDirectionEnum.右上:
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = ((columnCount * (sd + 1)) - dt).ToString();
                                break;
                            case ErosSocket.DebugPLC.Robot.TrayRobot.TrayDirectionEnum.左下:

                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = (rows * columnCount - (columnCount * (sd + 1)) + dt + 1).ToString();
                                break;
                            case ErosSocket.DebugPLC.Robot.TrayRobot.TrayDirectionEnum.右下:
                                trayControl.Name = trayControl.label1.Name = trayControl.label1.Text = (rows * columnCount - i).ToString();
                                break;
                        }
                    }
                    trayControl.Height = heit;
                    trayControl.Width = Wita;
                    trayControl.Location = new Point(new Size(trayControl.Width * dt, trayControl.Height * sd));
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
                //this.Parent.
           


            }
            catch (Exception)
            {

            }
        }

        private void Label1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button==MouseButtons.Right)
                {
                    return;
                }
                if (DebugCompiler.GetThis().IsImage)
                {
                    tabControl1.SelectedIndex = 0;
                }
                else
                {
                    tabControl1.SelectedIndex = 1;
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
                    toolStripLabel1.Text = "N:" + control.Name + ";SN:" + dataObj.PanelID;
                    if (dataObj!=null)
                    {
                        //dataGridView1.Rows.Clear();
                        treeView1.Nodes.Clear();
                        foreach (var item in dataObj.ListCamsData)
                        {
                           TreeNode treeNode=  treeView1.Nodes.Add(item.Key);
                            treeNode.Tag = item.Value;
                            treeNode.ImageIndex = 0;
                            foreach (var itemd in item.Value.NGObj.DicOnes)
                            {

                                TreeNode treeNode1= treeNode.Nodes.Add(itemd.Key);
                                if (itemd.Value.OK)
                                {
                                    treeNode1.ImageIndex = 6;
                                }
                                else
                                {
                                    treeNode1.ImageIndex = 7;
                                }
                          
                                treeNode1.Tag = itemd.Value;
                            }
                            treeNode.Expand();
                         }
                           foreach (var item in dataObj.ListCamsData)
                           {
                                HWi.SetImaage(item.Value.ImagePlus);
                                break;
                            }
                            if (dataObj.OK)
                            {
                                control.BackColor = Color.Green;
                            }
                            else
                            {
                                control.BackColor = Color.Red;
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
                    halcon.HobjClear();
                    HWindowControl controlH = halcon.GetHWindow().GetNmaeWindowControl("Image." + control.Text);
                    if (controlH != null)
                    {
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

        int xPos;
        int yPos;
        bool MoveFlag;
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
                                if (dataVale .OK)
                                {
                                    label1.label1.BackColor = Color.Green;
                                }
                                else
                                {
                                    label1.label1.BackColor = Color.Red;
                                }
                                if (tray.GetTrayData().GetDataVales()[number-1] == null)
                                {
                                    tray.GetTrayData().GetDataVales()[number - 1] = new OneDataVale();
                                }
                                tray.GetTrayData().GetDataVales()[number - 1].PanelID = dataVale.PanelID;
                                tray.GetTrayData().GetDataVales()[number - 1] =dataVale;
                                if (tray.GetTrayData().GetDataVales()[number - 1] != null)
                                {
                                    if (tray.GetTrayData().GetDataVales()[number - 1].PanelID != null)
                                    {
                                       datStr += tray.GetTrayData().GetDataVales()[number - 1].TrayLocation+  "SN:" + tray.GetTrayData().GetDataVales()[number - 1].PanelID + Environment.NewLine;
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
                                    tray.GetTrayData().GetDataVales()[number - 1].OK = false;
                                }
                                label1.Tag = tray.GetTrayData().GetDataVales()[number - 1];
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {
            }
        }
        bool fILE;
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
                panel1.Width =800;
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
                            if (data.TrayLocation==0)
                            {
                                continue;
                            }
                            if (this.Controls.Find(data.TrayLocation.ToString(), false).Length==1)
                            {
                                TrayControl label1 = this.Controls.Find(data.TrayLocation.ToString(), false)[0] as TrayControl;
                                if (label1 != null)
                                {
                                    if (data.Null)
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
                            

                                    if (data.PanelID != null)
                                    {
                                        datStr += data.TrayLocation + "SN:" + data.PanelID + Environment.NewLine;
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
                TreeNode CurrentNode =  e.Node;
                if (CurrentNode != null)
                {

                    if (CurrentNode.Tag is OneCamData)
                    {
                        OneCamData oneCamData = CurrentNode.Tag as OneCamData;
                    }
                    else if (CurrentNode.Tag is OneComponent)
                    {
                        OneComponent oneCamData = CurrentNode.Tag as OneComponent;
                        dataGridView2.Visible = true;
                        dataGridView2.Location = new Point(150, 100);
                        dataGridView2.Rows.Clear();
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
    }
}
