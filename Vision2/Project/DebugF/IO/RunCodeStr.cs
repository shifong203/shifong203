﻿using HalconDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Vision2.Project.formula;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.Project.DebugF.IO.RunCodeStr.ThreadData;

namespace Vision2.Project.DebugF.IO
{
    public class RunCodeStr
    {
        public List<string> CodeStr { get; set; } = new List<string>();

        public string Name { get; set; } = "";
        /// <summary>
        /// 下一步
        /// </summary>
        public bool NextStep;
        /// <summary>
        /// 运行步
        /// </summary>
        public int StepInt;


        /// <summary>
        /// 单步模式
        /// </summary>
        public bool Single_step;
        /// <summary>
        /// 执行 0=IF或者 1=Else
        /// </summary>
        public bool IFElseBool;

        /// <summary>
        /// 是否跳过行
        /// </summary>
        public int IfRowInt;
        /// <summary>
        /// 是否是IF指令
        /// </summary>
        public bool ISif;
        /// <summary>
        /// 等待超时
        /// </summary>
        public bool AwaitOut;

        public bool Paseub;
        /// <summary>
        /// 开启模拟
        /// </summary>

        public static bool IsSimulate;

        public double RunTime;


        /// <summary>
        /// 运行中
        /// </summary>
        public bool Runing;
        /// <summary>
        /// 跟新状态委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public delegate void RunCodeOne(RunErr key);

        /// <summary>
        ///开始执行状态
        /// </summary>
        public event RunCodeOne RunStratCode;
        /// <summary>
        /// 跟新状态
        /// </summary>
        public event RunCodeOne RunCode;
        /// <summary>
        /// 跟新状态
        /// </summary>
        public event RunCodeOne RunDone;
        /// <summary>
        /// 异步等待
        /// </summary>
        public class ThreadData
        {
            public ThreadData(string codename,int outTimeMs=5000)
            {
                CodeName = codename;
                OutTime = outTimeMs;
                WatchT.Restart();
            }
           /// <summary>
           /// 异步等待时间毫秒
           /// </summary>
            public int OutTime = 5000;
            System.Diagnostics.Stopwatch WatchT = new System.Diagnostics.Stopwatch();
            /// <summary>
            /// 代码名称
            /// </summary>
            public string CodeName { get; }
          
            public  bool End 
            { get
                 {
                     
                        if (Number==0)
                        {
                              WatchT.Stop();
                            return true;
                        }
                        if (WatchT.ElapsedMilliseconds > OutTime)
                        {
                            ErosProjcetDLL.Project.AlarmText.AddTextNewLine(CodeName+ "超时" + WatchT.ElapsedMilliseconds);
                            return true;
                        }
                        return  false;
                 }
            }

            /// <summary>
            /// 剩余数量
            /// </summary>
            public int Number { get
                {
                    int number = 0;
                    for (int i = 0; i < ThreadDataS.Count; i++)
                    {
                        if (!ThreadDataS[i].Done)
                        {
                            number++;
                        }
                    }
                    return number;    } 
            }
           /// <summary>
           /// 异步数量
           /// </summary>
            public int Count { get { return ThreadDataS.Count; } }
            /// <summary>
            /// 增加异步指令代码
            /// </summary>
            public void Add(string code)
            {
                ThreadDataS.Add(new RunErr() { Code=code});
            }
            public long RunTime { get
                {
                    return WatchT.ElapsedMilliseconds; }
            }
            /// <summary>
            /// 指令参数
            /// </summary>
            public List<RunErr> ThreadDataS = new List<RunErr>();

            public string GetErr()
            {
                string err = "";
                for (int i = 0; i < Count; i++)
                {
                    if (ThreadDataS[i].ErrStr != "")
                    {
                        err += ThreadDataS[i].ErrStr;
                    }
                }
                return err;
            }

        }


        public System.Diagnostics.Stopwatch Watch = new System.Diagnostics.Stopwatch();
        public static dynamic ToDoubleP(string pragrm)
        {
            try
            {
                pragrm = pragrm.Trim(' ');
                double vat = 0;
                if (int.TryParse(pragrm, out int resInt))
                {
                    return resInt;
                }
                if (double.TryParse(pragrm, out vat))
                {
                    return vat;
                }  //配方参数
                else if (pragrm.ToLower().StartsWith("p["))
                {
                    pragrm = pragrm.Substring(2, pragrm.Length - 3);
                    pragrm = Product.GetProd()[pragrm];
                    if (int.TryParse(pragrm, out resInt))
                    {
                        return resInt;
                    }
                    if (double.TryParse(pragrm, out vat))
                    {
                        return vat;
                    }
                    else
                    {
                        return pragrm;
                    }
                }//全局点位
                else if (pragrm.ToLower().StartsWith("pi["))
                {
                    string date = pragrm.Substring(3, pragrm.IndexOf(']') - pragrm.IndexOf('[') - 1);
                    if (date.Contains('.'))
                    {
                        string dateTC = pragrm.Split('.')[1];
                        for (int i = 0; i < DebugCompiler.GetThis().DDAxis.XyzPoints.Count; i++)
                        {
                            if (DebugCompiler.GetThis().DDAxis.XyzPoints[i].Name == date)
                            {
                                if (dateTC == "X")
                                {
                                    return DebugCompiler.GetThis().DDAxis.XyzPoints[i].X;
                                }
                                else if (dateTC == "Y")
                                {
                                    return DebugCompiler.GetThis().DDAxis.XyzPoints[i].Y;
                                }
                                else if (dateTC == "Z")
                                {
                                    return DebugCompiler.GetThis().DDAxis.XyzPoints[i].Z;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < DebugCompiler.GetThis().DDAxis.XyzPoints.Count; i++)
                        {
                            if (DebugCompiler.GetThis().DDAxis.XyzPoints[i].Name == date)
                            {
                                return DebugCompiler.GetThis().DDAxis.XyzPoints[i];
                            }
                        }
               
                    }
              
                }
                //产品点位
                else if (pragrm.ToLower().StartsWith("pf["))
                {
                    string date = pragrm.Substring(3, pragrm.IndexOf(']') - pragrm.IndexOf('[') - 1);
                    List<XYZPoint> points = RecipeCompiler.GetProductEX().DPoint;
                    string dateTC = pragrm;
                    if (pragrm.Contains('.'))
                    {
                        dateTC = pragrm.Split('.')[1]; 
                        for (int i = 0; i < points.Count; i++)
                        {
                            if (points[i].Name == date)
                            {
                                if (dateTC == "X")
                                {
                                    return points[i].X;
                                }
                                else if (dateTC == "Y")
                                {
                                    return points[i].Y;
                                }
                                else if (dateTC == "Z")
                                {
                                    return points[i].Z;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < points.Count; i++)
                        {
                            if (points[i].Name == date)
                            {
                                return points[i];
                            }
                        }
                    }
                }
                //托盘产品数量
                else if (pragrm.ToLower().StartsWith("tray"))
                {
                    string date = pragrm.Remove(0, 4);
                    int det = (int)ToDoubleP(date);
                    return DebugCompiler.GetThis().DDAxis.GetTrayInxt(det).Count;
                }
                //特殊参数
                else if (pragrm.ToLower().StartsWith("pt["))
                {
                    string date = pragrm.Substring(3, pragrm.IndexOf(']') - pragrm.IndexOf('[') - 1).ToLower();
                    string[] dataStrs = date.Split('.');
                    int idnex = 0;
                    if (date=="qr")
                    {
                        return ProcessControl.ProcessUser.QRCode;
                    }else if (dataStrs[0] == "trayid")
                    {
                        if (dataStrs.Length>=2)
                        {
                            idnex= ToDoubleP(dataStrs[1]);
                        }
                        return DebugCompiler.GetThis().DDAxis.GetTrayInxt(idnex).GetTrayData().TrayIDQR;
                    }
                    else if (dataStrs[0] == "trayqr")
                    {
                        int idnexD = 0;
                        if (dataStrs.Length >= 2)
                        {
                            idnex = ToDoubleP(dataStrs[1]);
                        }
                        if (dataStrs.Length>=3)
                        {
                            idnexD = ToDoubleP(dataStrs[2]);
                        }
                        if (idnexD!=0)
                        {
                            return DebugCompiler.GetThis().DDAxis.GetTrayInxt(idnex).GetTrayData().GetDataVales()[idnexD - 1].PanelID;
                        }
                        else
                        {
                            string datas = "";
                            for (int i = 0; i < DebugCompiler.GetThis().DDAxis.GetTrayInxt(idnex).Count; i++)
                            {
                                if (DebugCompiler.GetThis().DDAxis.GetTrayInxt(idnex).GetTrayData().GetDataVales()[i]!=null)
                                {
                                    datas += DebugCompiler.GetThis().DDAxis.GetTrayInxt(idnex).GetTrayData().GetDataVales()[i].PanelID + ",";
                                }
                                else
                                {
                                    datas +=  ",";
                                }
                            }
                            return datas;
                        }
                    }
                }
                //变量参数
                else
                {
                    if (DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(pragrm))
                    {
                        return DebugCompiler.GetThis().DDAxis.KeyVales[pragrm];
                    }
                    else
                    {
                        return pragrm;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return null;

        }

        public static bool ToPoint(string pragrm, out double? x, out double? y, out double? z, out double? u, out EnumXYZUMoveType enumXYZUMoveType)
        {
            x = null;
            y = null;
            z = null;
            u = null;
            enumXYZUMoveType = EnumXYZUMoveType.直接移动;
            string[] tdat = pragrm.Split(',');
            try
            {
                    for (int i = 0; i < tdat.Length; i++)
                    {
                        tdat[i] = tdat[i].Trim();
                    }
                    if (pragrm.Contains("tray"))
                    {
                        string datat = pragrm.Substring(pragrm.IndexOf("tray") + 4, 2);
                        string datatNumber = pragrm.Substring(pragrm.IndexOf("tray") + 4);
                        string[] doute = datatNumber.Split(',');
                        if (!int.TryParse(doute[1], out int point))
                        {
                            if (DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(doute[1]))
                            {
                                point = DebugCompiler.GetThis().DDAxis.KeyVales[doute[1]];
                            }
                        }
                        int tyNumbre = ToDoubleP(doute[0]);
                        ErosSocket.DebugPLC.PointFile pointFile = DebugCompiler.GetThis().DDAxis.GetTrayInxt(tyNumbre).GetPoint(point);
                        x = pointFile.X;
                        y = pointFile.Y;
                        z = pointFile.Z;
                        u = pointFile.U;
                        return true;
                    }
                    else if (tdat[1].ToLower().StartsWith("pi["))
                    {
                        string dname = tdat[1].Substring(3, tdat[1].Length - 4);
                        for (int i2 = 0; i2 < DebugCompiler.GetThis().DDAxis.XyzPoints.Count; i2++)
                        {
                            if (DebugCompiler.GetThis().DDAxis.XyzPoints[i2].Name == dname)
                            {
                                x = DebugCompiler.GetThis().DDAxis.XyzPoints[i2].X;
                                y = DebugCompiler.GetThis().DDAxis.XyzPoints[i2].Y;
                                z = DebugCompiler.GetThis().DDAxis.XyzPoints[i2].Z;
                                u = DebugCompiler.GetThis().DDAxis.XyzPoints[i2].U;
                                enumXYZUMoveType = DebugCompiler.GetThis().DDAxis.XyzPoints[i2].isMove;
                                return true;
                            }
                        }
                    }
                    else if (tdat[1].ToLower().StartsWith("pf["))
                    {
                        string dname = tdat[1].Substring(3, tdat[1].Length - 4);
                        List<XYZPoint> points = RecipeCompiler.GetProductEX().DPoint;
                        for (int i2 = 0; i2 < points.Count; i2++)
                        {
                            if (points[i2].Name == dname)
                            {
                                x = points[i2].X;
                                y = points[i2].Y;
                                z = points[i2].Z;
                                u = points[i2].U;
                                return true;
                            }
                        }
                    }
                    else
                    {
                        for (int i2 = 1; i2 < tdat.Length; i2++)
                        {
                            string[] doute = tdat[i2].Split('=');
                            if (doute[0].ToLower() == "x")
                            {
                                x = ToDoubleP(doute[1]);
                            }
                            if (doute[0].ToLower() == "y")
                            {
                                y = ToDoubleP(doute[1]);
                            }
                            if (doute[0].ToLower() == "z")
                            {
                                z = ToDoubleP(doute[1]);
                            }
                            if (doute[0].ToLower() == "u")
                            {
                                u = ToDoubleP(doute[1]);
                            }
                        }
                        return true;
                    }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #region 轴组指令
        public void GoS(RunErr runErr)
        {
            ThreadData threadDatas = new  ThreadData("GO",10000);
                try
                {
                    string[] Codesdata= runErr.Code.Remove(0,3).Trim(';').Split(';');
                    for (int i = 0; i < Codesdata.Length; i++)
                    {
                        threadDatas.Add(Codesdata[i]);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(Go), threadDatas.ThreadDataS[i]);
                    }
                     while (!threadDatas.End)
                    { }
                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine("结束");
                }
                catch (Exception ex)
                {
                    runErr.ErrStr += ex.Message;
                }
                 runErr.ErrStr += threadDatas.GetErr();
        }

        public int GoOutTime = 10;
        public void Go(object codeStr)
        {
            RunErr runErr = codeStr as RunErr;
            double? x = null;
            double? y = null;
            double? z = null;
            double? u = null;
            string[] imtey = runErr.Code.Trim().Split(',');
            if (ToPoint(runErr.Code, out x, out y, out z, out u, out EnumXYZUMoveType enumXYZUMoveType))
            {
                if (!IsSimulate)
                {
                    if (!DebugCompiler.GetThis().DDAxis.SetXYZ1Points(imtey[0], GoOutTime, x, y, z, u, enumXYZUMoveType))
                    {
                        if (DebugCompiler.EquipmentStatus != ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                        {
                            runErr.ErrStr += imtey[0]+"移动失败;";
                        }
                    }
                }
            }
            else
            {
                runErr.ErrStr += "未找到点位数据:" + runErr.Code;
            }
            runErr.Done = true;
        }
        public void LocusMove(out string errStr, params string[] visionNmae)
        {
            errStr = "";
            try
            {
                String NAMES = "";
                int number = 0;
                List<XYZPoint> points = RecipeCompiler.GetProductEX().DPoint;

                if (points[0].ID == 0)
                {
                    for (int j = 0; j < visionNmae.Length; j++)
                    {
                        if (vision.Vision.GetRunNameVision(visionNmae[j]) == null)
                        {
                            errStr = "视觉程序不存在:" + visionNmae;
                            return;
                        }
                        string names = visionNmae[j];
                        HalconRun halconRun = vision.Vision.GetRunNameVision(names);
                        string axisGroupName = vision.Vision.GetSaveImageInfo(halconRun.Name).AxisGrot;
                        if (points[0].AxisGrabName == axisGroupName)
                        {
                            if (DebugCompiler.GetThis().DDAxis.SetXYZ1Points(axisGroupName, 10, points[0].X, points[0].Y, points[0].Z, points[0].U, points[0].isMove))
                            {
                                Thread.Sleep(DebugCompiler.GetThis().MarkWait);
                                halconRun.AsysReadCamImage(1, 1, asyncRestImage => { });
                                Thread.Sleep(DebugCompiler.GetThis().CamWait);
                            }
                            else
                            {
                                if (DebugCompiler.RunStop)
                                {
                                    return ;
                                }
                                if (DebugCompiler.EquipmentStatus != ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                                {
                                    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(points[0].Name + ":LocusMove失败!");
                                }
                            }
                            NAMES = axisGroupName;
                            break;
                        }
                    }
                }
                for (int j = 0; j < visionNmae.Length; j++)
                {
                    if (vision.Vision.GetRunNameVision(visionNmae[j]) == null)
                    {
                        errStr = "视觉程序不存在:" + visionNmae;
                        return;
                    }
                    string names = visionNmae[j];
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            HalconRun halconRun = vision.Vision.GetRunNameVision(names);
                            string axisGroupName = vision.Vision.GetSaveImageInfo(halconRun.Name).AxisGrot;
                            var list = from n in points
                                       where n.AxisGrabName == axisGroupName
                                       where n.ID >= -1
                                       select n;
                            int i = 0;
                            foreach (var item in list)
                            {
                                if (i == 0)
                                {
                                    if (item.ID == 0 && NAMES == axisGroupName)
                                    {
                                        i++;
                                        continue;
                                    }
                                }
                                while (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.暂停中)
                                {
                                    Thread.Sleep(10);
                                    if (DebugCompiler.RunStop)
                                    {
                                        return;
                                    }
                                }
                                if (Single_step)
                                {
                                    NextStep = false;
                                    while (!NextStep && Single_step)
                                    {
                                        if (DebugCompiler.RunStop)
                                        {
                                            return;
                                        }
                                        if (Stoping)
                                        {
                                            Runing = false;
                                            break;
                                        }
                                        Thread.Sleep(10);
                                    }
                                }
                                if (this.Stoping)
                                {
                                    break;
                                }
                                if (DebugCompiler.GetThis().DDAxis.SetXYZ1Points(axisGroupName, 10, item.X, item.Y, item.Z, item.U, item.isMove))
                                {
                                    if (item.ID != -1)
                                    {
                                        i++;
                                        Thread.Sleep(DebugCompiler.GetThis().MarkWait);
                                        halconRun.AsysReadCamImage(i, (i), asyncRestImage => { });
                                        Thread.Sleep(DebugCompiler.GetThis().CamWait);
                                    }
                                }
                                else
                                {
                                    if (DebugCompiler.RunStop)
                                    {
                                        return;
                                    }
                                       if (DebugCompiler.EquipmentStatus != ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                                        {
                                            ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(item.Name + ":LocusMove失败!");
                                        }
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        number++;
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                while (number != visionNmae.Length)
                {
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                errStr += ex.Message + ":失败!";
            }

        }
        /// <summary>
        /// 轨迹
        /// </summary>
        /// <param name="errStr"></param>
        /// <param name="visionNmae"></param>
        public void LocusMoveImage(out string errStr, params string[] visionNmae)
        {
            errStr = "";
            try
            {
                int number = 0;
                List<XYZPoint> points = RecipeCompiler.GetProductEX().DPoint;
                for (int j = 0; j < visionNmae.Length; j++)
                {
                    if (vision.Vision.GetRunNameVision(visionNmae[j]) == null)
                    {
                        errStr = "视觉程序不存在:" + visionNmae;
                        return;
                    }
                    string names = visionNmae[j];
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            HalconRun halconRun = vision.Vision.GetRunNameVision(names);
                            string axisGroupName = vision.Vision.GetSaveImageInfo(halconRun.Name).AxisGrot;
                            var list = from n in points
                                       where n.AxisGrabName == axisGroupName
                                       where n.ID <= -10
                                       select n;
                            int i = 0;
                            foreach (var item in list)
                            {
                                while (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.暂停中)
                                {
                                    Thread.Sleep(10);
                                }
                                if (Single_step)
                                {
                                    NextStep = false;
                                    while (!NextStep && Single_step)
                                    {
                                        if (Stoping)
                                        {
                                            Runing = false;
                                            break;
                                        }
                                        Thread.Sleep(10);
                                    }
                                }
                                if (this.Stoping)
                                {
                                    break;
                                }
                                if (DebugCompiler.GetThis().DDAxis.SetXYZ1Points(axisGroupName, 10, item.X, item.Y, item.Z, item.U, item.isMove))
                                {
                                    if (item.ID != -1)
                                    {
                                        i++;
                                        Thread.Sleep(DebugCompiler.GetThis().MarkWait);
                                        halconRun.AsysReadCamImage(0, -1, asyncRestImage =>
                                        {
                                            halconRun.TiffeOffsetImageEX.SetTiffeOff(asyncRestImage.Image, i);
                                        });
                                        Thread.Sleep(DebugCompiler.GetThis().CamWait);
                                    }
                                    else
                                    {
                                        if (DebugCompiler.EquipmentStatus != ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                                        {
                                            ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(item.Name + ":移动失败!");
                                        }
                                    }
                                }
                            }
                            halconRun.Image(halconRun.TiffeOffsetImageEX.TiffeOffsetImage());
                            halconRun.ShowImage();
                            string path = vision.Vision.GetSaveImageInfo(halconRun.Name).SavePath + "\\" + DateTime.Now.ToLongDateString() + "\\" + Product.ProductionName + "\\" +
                            ProcessControl.ProcessUser.QRCode + "\\" + DateTime.Now.ToString("HH时mm分ss秒");
                            halconRun.SaveImage(path);
                        }
                        catch (Exception)
                        {
                        }
                        number++;
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                while (number != visionNmae.Length)
                {
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                errStr += ex.Message + ":失败!";
            }
        }

        /// <summary>
        /// 相对轨迹移动
        /// </summary>
        /// <param name="errStr"></param>
        /// <param name="relativeName">轨迹名称</param>
        public void RelativeLocusMove( RunErr errStr,  string relativeName)
        {
            try
            {
                int runID = 1;
                int LiyID = 1;
                List<string> visionNmae = new List<string>();
                String NAMES = "";
                //int number = 0;
                List<XYZPoint> points = RecipeCompiler.GetProductEX().Relativel.DicRelativelyPoint[relativeName];

                for (int i = 0; i < points.Count; i++)
                {
                    if (!visionNmae.Contains(points[i].AxisGrabName))
                    {
                        visionNmae.Add(points[i].AxisGrabName);
                    }
                }
                RunErr runErr = new RunErr();
                if (points[0].ID == 1)
                {
                    for (int j = 0; j < visionNmae.Count; j++)
                    {
                        string names = visionNmae[j];
                        HalconRun halconRun = null;
                        foreach (var item in vision.Vision.GetHimageList())
                        {
                            if (vision.Vision.GetSaveImageInfo(item.Value.Name).AxisGrot==names)
                            {
                                halconRun = item.Value;
                                break;
                            }
                        }
                        if (halconRun==null)
                        {
                            errStr.ErrStr = "视觉程序不存在:" + visionNmae+";";
                            return;
                        }
                        string axisGroupName = vision.Vision.GetSaveImageInfo(halconRun.Name).AxisGrot;
                        runErr.RunState = "轨迹首次："+ halconRun.Name+";" +points[0].GetPointStr();
                        RunCode?.Invoke(runErr);
                        if (points[0].AxisGrabName == axisGroupName)
                        {
                            Axis AxisX = DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(axisGroupName, ErosSocket.DebugPLC.EnumAxisType.X);
                            Axis AxisY = DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(axisGroupName, ErosSocket.DebugPLC.EnumAxisType.Y);
                            Axis AxisZ = DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(axisGroupName, ErosSocket.DebugPLC.EnumAxisType.Z);
                            Axis AxisU = DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(axisGroupName, ErosSocket.DebugPLC.EnumAxisType.U);
                            double ValeZ = points[0].Z;
                            if (AxisZ!=null)
                            {
                                ValeZ += AxisZ.Point;
                            }
                            double ValeU = points[0].U;
                            if (AxisU != null)
                            {
                                ValeU += AxisU.Point;
                            }
                            if (DebugCompiler.GetThis().DDAxis.SetXYZ1Points(axisGroupName, 10, AxisX.Point+ points[0].X,  AxisY.Point + points[0].Y,
                              ValeZ,  ValeU, points[0].isMove))
                            {
                                Thread.Sleep(DebugCompiler.GetThis().MarkWait);
                                if (halconRun.PaleMode)
                                {
                                     if (halconRun.TrayID >= 0)
                                        {
                                            int dt = DebugCompiler.GetThis().DDAxis.GetTrayInxt(halconRun.TrayID).Number;
                                            int det = ((dt-1) * halconRun.MaxRunID + runID) / LiyID;
                                             runID = ((dt - 1) * halconRun.PaleID + LiyID);
                                    }
                                }
                                halconRun.AsysReadCamImage(LiyID, runID, asyncRestImage => { });
                                Thread.Sleep(DebugCompiler.GetThis().CamWait);
                            }
                            else
                            {
                                if (DebugCompiler.EquipmentStatus != ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                                {
                                    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(points[0].Name + ":移动失败!");
                                }
                            }
                            NAMES = axisGroupName;
                            break;
                        }
                    }
                }

                ThreadData threadDatas = new ThreadData("RelativeLocusMove",50000000);
                    runErr.Code = relativeName;
                    for (int j = 0; j < visionNmae.Count; j++)
                    {
                        threadDatas.Add(relativeName);
                        threadDatas.ThreadDataS[j].ParName = visionNmae[j];
                        ThreadPool.QueueUserWorkItem(new WaitCallback(RelativeLocusTs), threadDatas.ThreadDataS[j]);
                    }
                    while (!threadDatas.End)
                    { }
                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine("结束");

                //}
                //catch (Exception ex)
                //{
                //    runErr.ErrStr += ex.Message;
                //}

                errStr.ErrStr += threadDatas.GetErr();
                for (int j = 0; j < visionNmae.Count; j++)
                {
                    //string names = visionNmae[j];
                    //HalconRun halconRun = null;
                    //foreach (var item in vision.Vision.GetHimageList())
                    //{
                    //    if (vision.Vision.GetSaveImageInfo(item.Value.Name).AxisGrot == names)
                    //    {
                    //        halconRun = item.Value;
                    //        break;
                    //    }
                    //}
                    //if (halconRun == null)
                    //{
                    //    errStr = "视觉程序不存在:" + visionNmae;
                    //    return;
                    //}
                    //Thread thread = new Thread(() =>
                    //{
                    //    try
                    //    {
                         
                    //        string axisGroupName = vision.Vision.GetSaveImageInfo(halconRun.Name).AxisGrot;
                    //        var list = from n in points
                    //                   where n.AxisGrabName == axisGroupName
                    //                   where n.ID >= -1
                    //                   select n;
                    //        int i = 0;
                    //        foreach (var item in list)
                    //        {
                    //            if (i == 0)
                    //            {
                    //                if (item.ID == 0)
                    //                {
                    //                    i++;
                    //                    continue;
                    //                }
                    //            }
                    //            while (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.暂停中)
                    //            {
                    //                Thread.Sleep(10);
                    //            }
                    //            if (Single_step)
                    //            {
                    //                NextStep = false;
                    //                while (!NextStep && Single_step)
                    //                {
                    //                    if (Stoping)
                    //                    {
                    //                        Runing = false;
                    //                        break;
                    //                    }
                    //                    Thread.Sleep(10);
                    //                }
                    //            }
                    //            if (this.Stoping)
                    //            {
                    //                break;
                    //            }
                    //            Axis AxisX = DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(axisGroupName, ErosSocket.DebugPLC.EnumAxisType.X);
                    //            Axis AxisY = DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(axisGroupName, ErosSocket.DebugPLC.EnumAxisType.Y);
                    //            Axis AxisZ = DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(axisGroupName, ErosSocket.DebugPLC.EnumAxisType.Z);
                    //            Axis AxisU = DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(axisGroupName, ErosSocket.DebugPLC.EnumAxisType.U);
                    //            double ValeZ = points[0].Z;
                    //            if (AxisZ != null)
                    //            {
                    //                ValeZ += AxisZ.Point;
                    //            }
                    //            double ValeU = points[0].U;
                    //            if (AxisU != null)
                    //            {
                    //                ValeU += AxisU.Point;
                    //            }
                    //            runErr.ErrStr = i+ "位置"+ item.Name+  "轨迹：" + halconRun.Name;
                    //            RunCode?.Invoke(runErr);
                    //            if (DebugCompiler.GetThis().DDAxis.SetXYZ1Points(axisGroupName, 10, item.X + AxisX.Point, item.Y + AxisY.Point, ValeZ, ValeU, item.isMove))
                    //            {
                    //                if (item.ID != -1)
                    //                {
                    //                    i++;
                    //                    LiyID = i;
                    //                    runID = i;
                    //                    Thread.Sleep(DebugCompiler.GetThis().MarkWait);
                    //                    if (halconRun.PaleMode)
                    //                    {
                    //                        if (halconRun.TrayID >= 0)
                    //                        {
                    //                            int dt = DebugCompiler.GetThis().DDAxis.GetTrayInxt(halconRun.TrayID).Number;
                    //                            runID = ((dt - 1) * halconRun.PaleID + LiyID) ;
                    //                            if (halconRun.PaleID == LiyID)
                    //                            {
                    //                                DebugCompiler.GetThis().DDAxis.GetTrayInxt(halconRun.TrayID).Number++;
                    //                            }
                    //                            if (ErosProjcetDLL.Project.ProjectINI.AdminEnbt)
                    //                            {
                    //                                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("托盘数:" + dt + ":" + runID + ":" + LiyID);
                    //                            }
                    //                        }
                    //                    }

                    //                    halconRun.AsysReadCamImage(LiyID.ToString(), runID, asyncRestImage => { });
                    //                    Thread.Sleep(DebugCompiler.GetThis().CamWait);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if (DebugCompiler.EquipmentStatus != ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                    //                {
                    //                    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(item.Name + ":移动失败!");
                    //                }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception)
                    //    {
                    //    }
                    //    number++;
                    //});
                    //thread.IsBackground = true;
                    //thread.Start();
                }
                //while (number != visionNmae.Count)
                //{
                //    Thread.Sleep(10);
                //}
         
            }
            catch (Exception ex)
            {
                errStr.ErrStr += ex.Message + ":失败!";
            }

        }
        /// <summary>
        /// 相对轨迹移动
        /// </summary>
        /// <param name="errStr"></param>
        /// <param name="visionNmae"></param>
        public void RelativelyMoveImage(out string errStr, params string[] visionNmae)
        {
            ThreadData threadDatas = new ThreadData("RelativelyMoveImage");
            errStr = "";
            try
            {
                String NAMES = "";
                int number = 0;
                ProductEX productEX = RecipeCompiler.GetProductEX();
                List<XYZPoint> points = productEX.Relativel.DicRelativelyPoint[""];
                if (points[0].ID == 0)
                {
                    for (int j = 0; j < visionNmae.Length; j++)
                    {
                        if (vision.Vision.GetRunNameVision(visionNmae[j]) == null)
                        {
                            errStr = "视觉程序不存在:" + visionNmae;
                            return;
                        }
                        string names = visionNmae[j];
                        HalconRun halconRun = vision.Vision.GetRunNameVision(names);
                        string axisGroupName = vision.Vision.GetSaveImageInfo(halconRun.Name).AxisGrot;
                        if (points[0].AxisGrabName == axisGroupName)
                        {
                            if (DebugCompiler.GetThis().DDAxis.SetXYZ1Points(axisGroupName, 10, points[0].X, points[0].Y, points[0].Z, points[0].U, points[0].isMove))
                            {
                                Thread.Sleep(DebugCompiler.GetThis().MarkWait);
                                halconRun.AsysReadCamImage(1, 1, asyncRestImage => { });
                                Thread.Sleep(DebugCompiler.GetThis().CamWait);
                            }
                            else
                            {
                                if (DebugCompiler.EquipmentStatus != ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                                {
                                    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(points[0].Name + ":移动失败!");
                                }
                            }
                            NAMES = axisGroupName;
                            break;
                        }
                    }
                }
                for (int j = 0; j < visionNmae.Length; j++)
                {
                    if (vision.Vision.GetRunNameVision(visionNmae[j]) == null)
                    {
                        errStr = "视觉程序不存在:" + visionNmae;
                        return;
                    }
                    string names = visionNmae[j];
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            HalconRun halconRun = vision.Vision.GetRunNameVision(names);
                            string axisGroupName = vision.Vision.GetSaveImageInfo(halconRun.Name).AxisGrot;
                            var list = from n in points
                                       where n.AxisGrabName == axisGroupName
                                       where n.ID >= -1
                                       select n;
                            int i = 0;
                            foreach (var item in list)
                            {
                                if (i == 0)
                                {
                                    if (item.ID == 0 && NAMES == axisGroupName)
                                    {
                                        i++;
                                        continue;
                                    }
                                }
                                while (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.暂停中)
                                {
                                    Thread.Sleep(10);
                                }
                                if (Single_step)
                                {
                                    NextStep = false;
                                    while (!NextStep && Single_step)
                                    {
                                        if (Stoping)
                                        {
                                            Runing = false;
                                            break;
                                        }
                                        Thread.Sleep(10);
                                    }
                                }
                                if (this.Stoping)
                                {
                                    break;
                                }
                                if (DebugCompiler.GetThis().DDAxis.SetXYZ1Points(axisGroupName, 10, item.X, item.Y, item.Z, item.U, item.isMove))
                                {
                                    if (item.ID != -1)
                                    {
                                        i++;
                                        Thread.Sleep(DebugCompiler.GetThis().MarkWait);
                                        halconRun.AsysReadCamImage(i, (i), asyncRestImage => { });
                                        Thread.Sleep(DebugCompiler.GetThis().CamWait);
                                    }
                                }
                                else
                                {
                                    if (DebugCompiler.EquipmentStatus != ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                                    {
                                        ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(item.Name + ":移动失败!");
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        number++;
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                while (number != visionNmae.Length)
                {
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                errStr += ex.Message + ":失败!";
            }

        }

        public void RelativeLocusTs(object codeStr )
        {
            int runID = 1;
            int LiyID = 1;
            RunErr threadData = codeStr as RunErr;
            try
            {
                List<XYZPoint> points = RecipeCompiler.GetProductEX().Relativel.DicRelativelyPoint[threadData.Code];

                string names = threadData.ParName;
                HalconRun halconRun = null;
                foreach (var item in vision.Vision.GetHimageList())
                {
                    if (vision.Vision.GetSaveImageInfo(item.Value.Name).AxisGrot == names)
                    {
                        halconRun = item.Value;
                        break;
                    }
                }
                if (halconRun == null)
                {
                    threadData.ErrStr  = "视觉程序不存在:" + names;
                    return;
                }
                string axisGroupName = vision.Vision.GetSaveImageInfo(halconRun.Name).AxisGrot;
                var list = from n in points
                           where n.AxisGrabName == axisGroupName
                           where n.ID >= -1
                           select n;
                int i = 0;
                foreach (var item in list)
                {
                    if (i == 0)
                    {
                        if (item.ID == 1)
                        {
                            i++;
                            continue;
                        }
                    }
                    while (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.暂停中)
                    {
                        Thread.Sleep(10);
                    }
                    if (Single_step)
                    {
                        NextStep = false;
                        while (!NextStep && Single_step)
                        {
                            if (Stoping)
                            {
                                Runing = false;
                                break;
                            }
                            Thread.Sleep(10);
                        }
                    }
                    if (this.Stoping)
                    {
                        break;
                    }
                    Axis AxisX = DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(axisGroupName, ErosSocket.DebugPLC.EnumAxisType.X);
                    Axis AxisY = DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(axisGroupName, ErosSocket.DebugPLC.EnumAxisType.Y);
                    Axis AxisZ = DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(axisGroupName, ErosSocket.DebugPLC.EnumAxisType.Z);
                    Axis AxisU = DebugCompiler.GetThis().DDAxis.GetAxisGrotNameEx(axisGroupName, ErosSocket.DebugPLC.EnumAxisType.U);
                    double ValeZ = item.Z;
                    if (AxisZ != null)
                    {
                        ValeZ += AxisZ.Point;
                    }
                    double ValeU = item.U;
                    if (AxisU != null)
                    {
                        ValeU += AxisU.Point;
                    }
                    i++;
                    threadData.RunState = i + "轨迹:" + halconRun.Name + ";"+ item.GetPointStr(); 
                    RunCode?.Invoke(threadData);
                    if (DebugCompiler.GetThis().DDAxis.SetXYZ1Points(axisGroupName, 10, item.X + AxisX.Point, item.Y + AxisY.Point, ValeZ, ValeU, item.isMove))
                    {
                          LiyID = i;
                          runID = i;
                          Thread.Sleep(DebugCompiler.GetThis().MarkWait);
                          if (halconRun.PaleMode)
                          {
                               if (halconRun.TrayID >= 0)
                                {
                                    int dt = DebugCompiler.GetThis().DDAxis.GetTrayInxt(halconRun.TrayID).Number;
                                    runID = ((dt - 1) * halconRun.PaleID + LiyID);
                                    //if (halconRun.PaleID == LiyID)
                                    //{
                                    //    DebugCompiler.GetThis().DDAxis.GetTrayInxt(halconRun.TrayID).Number++;
                                    //}
                                    if (ErosProjcetDLL.Project.ProjectINI.AdminEnbt)
                                    {
                                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine("托盘数:" + dt + ":" + runID + ":" + LiyID);
                                    }
                                }
                          }
                          halconRun.AsysReadCamImage(LiyID, runID, asyncRestImage => { });
                          Thread.Sleep(DebugCompiler.GetThis().CamWait);
                    }
                    else
                    {
                        if (DebugCompiler.EquipmentStatus != ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                        {
                            ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(item.Name + ":移动失败!");
                        }
                    }
           
                }
            }
            catch (Exception ex)
            {
                threadData.ErrStr += ex.Message;
            }
            threadData.Done = true;
        }

        #endregion

        #region 图像指令

            public void Cams( RunErr runErr )
            {
               ThreadData threadDatas =  new    ThreadData("Cam");
                try
                {
                    string[] Cmas = runErr.Code.Remove(0,4).Trim(';').Split(';');
                    for (int i = 0; i < Cmas.Length; i++)
                    {
                        threadDatas .Add(Cmas[i].Trim());
                     ThreadPool.QueueUserWorkItem(new WaitCallback(Cam), threadDatas.ThreadDataS[i]);
                    }
                    while (!threadDatas.End)
                     { }
                  ErosProjcetDLL.Project.AlarmText.AddTextNewLine("结束");
                }
                catch (Exception ex)
                {
                    runErr.ErrStr += ex.Message;
                }

                runErr.ErrStr += threadDatas.GetErr();
         }
   
            private void Cam(object state)
            {
               RunErr threadData = state as RunErr;
                try
                {
                    if (threadData != null)
                    {
                    string[] tdat = threadData.Code.Split(',');
                    HalconRun hate = vision.Vision.GetRunNameVision(tdat[0]);
                    if (hate != null)
                    {
                        HObject hObject = vision.Vision.GetNameCam(tdat[1]).GetImage();
                        if (int.TryParse(tdat[2], out int det))
                        {
                            //if (det == 1)
                            //{
                            //    hate.TiffeOffsetImageEX.SetTiffeOff();
                            //}
                            hate.TiffeOffsetImageEX.SetTiffeOff(hObject, det);
                            //hate.Image(hate.TiffeOffsetImageEX.TiffeOffsetImage());
                        }
                        //hate.ShowImage();
                    }
 
                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine(threadData.Code.ToString());
                    }
                }
                catch (Exception ex)
                {
                    threadData.ErrStr = ex.Message;
                }
            threadData.Done = true;
        }
           public void Calls(RunErr runErr)
          {
            ThreadData threadDatas = new ThreadData("Call");
            try
            {
                string[] Cmas = runErr.Code.Remove(0, 5).Trim(';').Split(';');
           
                for (int i = 0; i < Cmas.Length; i++)
                {
                    threadDatas.Add(Cmas[i].Trim().Trim(','));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(Call), threadDatas.ThreadDataS[i]);
                }
                while (!threadDatas.End)
                { }
                runErr.RunState = threadDatas.CodeName+":"+ threadDatas.RunTime.ToString();
                RunCode?.Invoke(runErr);
                //ErosProjcetDLL.Project.AlarmText.AddTextNewLine("结束");
            }
            catch (Exception ex)
            {
                runErr.ErrStr += ex.Message;
            }
            runErr.ErrStr += threadDatas.GetErr();
        }
            private void Call(object state)
            {
                RunErr threadData = state as RunErr;
                try
                {
                    if (threadData!=null)
                {
                    string[] tdat = threadData.Code.Split(',');
                    int LiayID = ToDoubleP(tdat[1]);
                    int RunID = LiayID;
                    if (tdat.Length>=3)
                    {
                        RunID = ToDoubleP(tdat[2]);
                    }

      
                    HalconRun halconRun = vision.Vision.GetRunNameVision(tdat[0]);

                    halconRun.GetCam().Key = RunID.ToString();
                    if (!IsSimulate)
                    {
                        halconRun.ReadCamImage( LiayID.ToString(), RunID);
                    }
                    else
                    {
                        string path = vision. Vision.VisionPath + "Image\\" + halconRun .Name+ LiayID + ".bmp";
                        if (System.IO. File.Exists(path))
                        {
                            HOperatorSet.ReadImage(out HObject hObject, path);
                            halconRun.Image(hObject);
                         }
                        halconRun.CamImageEvent(LiayID.ToString(), halconRun.GetOneImageR(), RunID);
                    }
                     //ErosProjcetDLL.Project.AlarmText.AddTextNewLine(threadData.Code.ToString());
                    }
                }
                catch (Exception ex)
                {
                    threadData.ErrStr = ex.Message;
                }
            threadData.Done = true;

        }
 
        #endregion

        public RunErr RunSingle(string text, int rowindex)
        {
            if (Single_step)
            {
                NextStep = false;
            }
            RunErr runErr = new RunErr();
            try
            {
                StepInt = rowindex;
                runErr.RowIndx = rowindex;
                runErr.Code = text;
                runErr.runCoStr = RunCoStr.执行中;
                if (text == "")
                {
                    goto End;
                }
                RunStratCode?.Invoke(runErr);
                while (Paseub)
                {
                    Thread.Sleep(10);
                }

                if (text == "end")
                {
                    ISif = false;
                    IfRowInt = -2;
                    goto End;
                }
                if (IfRowInt > 0)
                {
                    if (text == "else")
                    {
                        if (IFElseBool)
                        {
                            IfRowInt = 0;
                        }
                        else
                        {
                            IfRowInt = 1;
                        }
                        goto End;
                    }
                    else if (text == "end")
                    {
                        ISif = false;
                        IfRowInt = -2;
                        goto End;
                    }
                    goto End;
                }
                if (Single_step)
                {
                    NextStep = false;
                    while (!NextStep && Single_step)
                    {
                        if (Stoping)
                        {

                            Runing = false;
                            goto End;
                        }
                        Thread.Sleep(10);
                    }
                }
                Watch.Restart();
                if (Stoping)
                {
                    Runing = false;
                    goto End;
                }
                List<string> DatTup = new List<string>();
                string[] tdat = text.Split(',');
                for (int i = 0; i < tdat.Length; i++)
                {
                    tdat[i] = tdat[i].Trim();
                }
                //tdat[0] = tdat[0].ToLower();
                string[] yutData = tdat[0].Split('=');
                string[] imtey = yutData[0].Split(' ');
                imtey[0] = imtey[0].ToLower();
                List<string> list = new List<string>();
                for (int i = 0; i < imtey.Length; i++)
                {
                    if (imtey[i] != "")
                    {
                        list.Add(imtey[i]);
                    }
                }
                imtey = list.ToArray();
                if (imtey[0] == "go")
                {
                    GoS(runErr);
                }
                else if (imtey[0] == "jump")
                {
                    double? x = null;
                    double? y = null;
                    double? z = null;
                    double? Jz = null;
                    double? u = null;
                    for (int i2 = 1; i2 < tdat.Length; i2++)
                    {
                        string[] doute = tdat[i2].Split('=');
                        if (i2 == 1)
                        {
                            if (doute.Length == 1)
                            {
                                Jz = ToDoubleP(doute[0]);
                            }
                            else
                            {
                                if (doute[0].ToLower().Trim(' ') == "z")
                                {
                                    Jz = ToDoubleP(doute[1]);
                                }
                            }
                            break;
                        }
                    }
                    string datas = text;

                    if (tdat.Length == 3)
                    {
                        datas = datas.Remove(0, datas.IndexOf(',') + 1);
                    }
                    ToPoint(datas, out x, out y, out z, out u, out EnumXYZUMoveType enumXYZUMoveType);
                    if (!IsSimulate)
                    {
                        if (!DebugCompiler.GetThis().DDAxis.SetXYZ1Points(imtey[1], 10, x, y, z, u, EnumXYZUMoveType.跳跃门型, Jz))
                        {
                            if (DebugCompiler.EquipmentStatus != ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                            {
                                runErr.ErrStr = "移动失败";
                            }
                        }
                    }
                }
                else if (imtey[0] == "move")
                {

                }
                else if (imtey[0] == "locusmove")
                {
                    string[] visionNmae = new string[tdat.Length - 1];

                    Array.Copy(tdat, 1, visionNmae, 0, visionNmae.Length);
                    LocusMove(out string err, visionNmae);
                }
                else if (imtey[0] == "locusmoveimage")
                {
                    string[] visionNmae = new string[tdat.Length - 1];

                    Array.Copy(tdat, 1, visionNmae, 0, visionNmae.Length);
                    LocusMoveImage(out string err, visionNmae);
                }else if(imtey[0]== "relativelocusmove")
                {
                    RelativeLocusMove(runErr, tdat[1]);
                }
                else if (imtey[0] == "relativelymoveimage")
                {
                    string[] visionNmae = new string[tdat.Length - 1];

                    Array.Copy(tdat, 1, visionNmae, 0, visionNmae.Length);
                    RelativelyMoveImage(out string err, visionNmae);
                }
                else if (imtey[0] == "movematrix")
                {


                }
                else if (text == "else")
                {
                    if (IFElseBool)
                    {
                        IfRowInt = 0;
                    }
                    else
                    {
                        IfRowInt = 1;
                    }
                    goto End;
                }
                else if (text == "end")
                {
                    ISif = false;
                    IfRowInt = -2;
                    goto End;
                }
                else if (imtey[0] == "showmesaage")
                {
                    bool Await = false;

                    if (text.Contains('{'))
                    {
                        if (imtey.Length >= 2 && imtey[1] == "sys")
                        {

                            Await = true;
                        }
                        simulateQRForm.ShowMesabe(text.Remove(0, imtey[0].Length), Await);
                    }
                    else
                    {
                        simulateQRForm.ShowMesabe(text.Remove(0, imtey[0].Length), Await);
                    }
                }
                else if (imtey[0] == "resetshowmesaage")
                {
                    bool Await = false;
                    if (text.Contains('{'))
                    {
                        if (imtey.Length >= 2 && imtey[1] == "sys")
                        {
                            Await = true;
                        }
                        simulateQRForm.ShowMesabe(text.Remove(0, text.ToLower().IndexOf("{") + 1), Await);
                    }
                    else
                    {
                        simulateQRForm.ShowMesabe(text.Remove(0, text.ToLower().IndexOf("resetshowmesaage") + imtey[0].Length), Await);
                    }
                }
                else if (imtey[0] == "resetmes")
                {
                    int TyraID = 0;
                    if (imtey.Length == 3)
                    {
                        TyraID = ToDoubleP(imtey[2]);
                    }
                    int TrayIndex = -1;
                    bool StopIS = false;
                    if (tdat.Length >= 2 && ToDoubleP(tdat[1]) != null)
                    {
                        TrayIndex = ToDoubleP(tdat[1]);
                    }
                    if (tdat.Length == 2 && tdat[1].ToLower() == "stop")
                    {
                        StopIS = true;
                    }
                    if (tdat.Length == 3 && tdat[2].ToLower() == "stop")
                    {
                        StopIS = true;
                    }
                    bool rsetOk = false;
                    string restMest = "";
                    if (TrayIndex < 0)
                    {
                        rsetOk = RecipeCompiler.Instance.GetMes().ReadMes(DebugCompiler.GetThis().DDAxis.GetTrayInxt(TyraID).GetTrayData().TrayIDQR, out restMest);
                    }
                    else
                    {
                        rsetOk = RecipeCompiler.Instance.GetMes().ReadMes(DebugCompiler.GetThis().DDAxis.GetTrayInxt(TyraID).GetTrayData().GetDataVales()[TrayIndex - 1].PanelID, out restMest);
                    }
                    if (!rsetOk)
                    {
                        simulateQRForm.ShowMesabe(restMest);
                        if (StopIS)
                        {
                            DebugCompiler.Stop(true);
                        }
                        else
                        {
                            DebugCompiler.Pause();
                        }
                    }
                }
                else if (imtey[0] == "ngshow")
                {
                    vision.Vision.ShowVisionResetForm();
                }
                else if (imtey[0] == "sendresettray")
                {
                    if (ErosSocket.ErosConLink.StaticCon.GetSocketClint(RecipeCompiler.Instance.RsetLinkName) != null)
                    {
                        string jsonStr = JsonConvert.SerializeObject(DebugF.DebugCompiler.GetTrayDataUserControl().GetTrayEx());
                        ErosSocket.ErosConLink.StaticCon.GetSocketClint(RecipeCompiler.Instance.RsetLinkName).Send("Tray" + jsonStr);
                    }
                }
                else if (imtey[0] == "resettray")
                {
                    bool Await = false;
                    if (text.Contains('{'))
                    {
                        if (imtey.Length >= 2 && imtey[1] == "sys")
                        {
                            Await = true;
                        }
                        int TyraID = ToDoubleP(tdat[1]);
                        SimulateTrayMesForm.ShowMesabe(text.Remove(0, text.ToLower().IndexOf("{") + 1),
                            DebugCompiler.GetThis().DDAxis.GetTrayInxt(TyraID).GetTrayData(), Await);
                        while (!SimulateTrayMesForm.RresOK)
                        {
                            Thread.Sleep(1);
                        }
                    }
                }
                else if (imtey[0] == "addtext")
                {
                    if (tdat.Length >= 2)
                    {
                        Color color = Color.FromName(tdat[1]);
                        if (text.Contains("{"))
                        {
                            ErosProjcetDLL.Project.AlarmText.AddTextNewLine(text.Remove(0, text.IndexOf('{') + 1), color);
                        }

                    }
                    else
                    {
                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine(text.Remove(0, imtey[0].Length), Color.Green);
                    }
                }
                else if (imtey[0] == "restmesaagetrayid")
                {
                    string name = "";
                    if (ProcessControl.ProcessUser.QRCode == "")
                    {
                        if (simulateQRForm.ShowMesabe("请手动输入托盘ID", out name))
                        {
                            if (name.Contains("\r\n"))
                            {
                                name = name.Remove(name.Length - 2);
                            }
                            UserFormulaContrsl.StaticAddQRCode(name);
                        }
                    }
                }
                else if (imtey[0] == "syncgo")
                {
                    List<string> names = new List<string>();
                    List<string> points = new List<string>();

                    for (int i = 0; i < tdat.Length; i++)
                    {
                        string[] datte = new string[] { };
                        if (tdat[i].ToLower().StartsWith("syncgo"))
                        {
                            tdat[i] = tdat[i].Remove(0, 6);
                        }
                        if (tdat[i] == "")
                        {
                            continue;
                        }
                        if (tdat[i].Contains("="))
                        {
                            datte = tdat[i].Split('=');
                        }
                        else if (tdat[i].Contains("+"))
                        {
                            datte = tdat[i].Split('+');
                            double det = RunCodeStr.ToDoubleP(datte[1]);
                            det = DebugCompiler.GetThis().DDAxis.GetAxisPoint(datte[0].Trim()) + det;
                            datte[1] = det.ToString();
                        }
                        else if (tdat[i].Contains("-"))
                        {
                            datte = tdat[i].Split('-');
                            double det = RunCodeStr.ToDoubleP(datte[1]);
                            det = DebugCompiler.GetThis().DDAxis.GetAxisPoint(datte[0].Trim()) - det;
                            datte[1] = det.ToString();
                        }
                        if (datte.Length != 2)
                        {
                            runErr.ErrStr += "参数不正确";
                        }
                        names.Add(datte[0].Trim(' '));
                        points.Add(datte[1]);
                    }
                    if (!IsSimulate)
                    {
                        if (!DebugCompiler.GetThis().DDAxis.AxisSycnGo(names.ToArray(), points.ToArray()))
                        {
                            runErr.ErrStr = "移动失败";
                        }
                    }
                }
                else if (imtey[0] == "settray")
                {
                    int det = 0;
                    int.TryParse(imtey[1], out det);
                    det = (int)ToDoubleP(imtey[1]);
                    if (tdat.Length==3)
                    {
                        int xNumber = (int)ToDoubleP(tdat[1]);
                        int YNumber = (int)ToDoubleP(tdat[2]);
                        DebugCompiler.GetThis().DDAxis.SetTray(det,  xNumber.ToString(), YNumber.ToString());
                    }
                    else
                    {
                        int xNumber = (int)ToDoubleP(tdat[5]);
                        int YNumber = (int)ToDoubleP(tdat[6]);
                        if (tdat.Length == 13)
                        {
                            int x2Number = (int)ToDoubleP(tdat[11]);
                            int Y2Number = (int)ToDoubleP(tdat[12]);
                            DebugCompiler.GetThis().DDAxis.SetTray(det, tdat[1], tdat[2], tdat[3], tdat[4], xNumber.ToString(), YNumber.ToString(),
                                tdat[7], tdat[8], tdat[9], tdat[10], x2Number.ToString(), Y2Number.ToString());
                        }
                        else
                        {
                            DebugCompiler.GetThis().DDAxis.SetTray(det, tdat[1], tdat[2], tdat[3], tdat[4], xNumber.ToString(), YNumber.ToString());
                        }
                    }
                }
                else if (imtey[0] == "axis")
                {
                    if (!IsSimulate)
                    {
                        if (tdat.Length == 1)
                        {
                            DebugCompiler.GetThis().DDAxis.AxisGo(imtey[1], yutData[1]);
                        }
                        else if (tdat.Length == 2)
                        {
                            DebugCompiler.GetThis().DDAxis.AxisGo(imtey[1], yutData[1], tdat[1]);
                        }
                    }
                }

                else if (imtey[0] == "axisstop")
                {
                    DebugCompiler.GetThis().DDAxis.GetAxisName(imtey[1]).Stop();
                }
                else if (imtey[0] == "sleep")
                {
                    if (text.Contains(';'))
                    {
                        string[] dataStr = text.Remove(0, 6).Trim().Split(';');
                        for (int i = 0; i < dataStr.Length; i++)
                        {
                            string[] dat = dataStr[i].Split('=');
                            if (dat[0] == "")
                            {
                                break;
                            }
                            if (dat.Length >= 2)
                            {
                                string[] datS = dat[1].Split(',');
                                double?[] selp = new double?[4];

                                for (int i2 = 0; i2 < datS.Length; i2++)
                                {
                                    selp[i2] = ToDoubleP(datS[i2]);
                                }
                                DebugCompiler.GetThis().DDAxis.AxisSeelp(dat[0], selp[0].Value, selp[1], selp[2], selp[3]);
                            }
                            else
                            {
                                DebugCompiler.GetThis().DDAxis.AxisSeelp(dat[0]);
                            }
                        }
                    }
                    else
                    {
                        string[] dat = text.Split('=');
                        if (dat.Length >= 2)
                        {
                            string[] datS = dat[1].Split(',');
                            double?[] selp = new double?[4];
                            for (int i2 = 0; i2 < datS.Length; i2++)
                            {
                                selp[i2] = ToDoubleP(datS[i2]);
                            }
                            DebugCompiler.GetThis().DDAxis.AxisSeelp(imtey[1], selp[0].Value, selp[1], selp[2], selp[3]);
                        }
                        else
                        {
                            if (imtey.Length == 1)
                            {
                                DebugCompiler.GetThis().SetSeelp();
                            }
                            else
                            {
                                DebugCompiler.GetThis().DDAxis.AxisSeelp(imtey[1]);
                            }
                        }
                    }
                }
                else if (imtey[0] == "axishome")
                {
                    tdat[0] = imtey[1];
                    if (!IsSimulate)
                    {
                        for (int i = 0; i < tdat.Length; i++)
                        {

                            DebugCompiler.GetThis().DDAxis.GetAxisName(tdat[i]).SetHome();


                        }
                        Thread.Sleep(1000);
                        int homeInt = 0;
                        while (homeInt != tdat.Length)
                        {
                            homeInt = 0;
                            for (int i = 0; i < tdat.Length; i++)
                            {
                                if (DebugCompiler.GetThis().DDAxis.GetAxisName(tdat[i]).IsHome)
                                {
                                    homeInt++;
                                }
                            }
                            Thread.Sleep(10);
                            if (Stoping)
                            {
                                throw new Exception("初始化停止");
                            }
                        }
                    }
                }
                else if (imtey[0] == "cp")
                {
                    if (!IsSimulate)
                    {
                        if (!DebugCompiler.GetThis().DDAxis.Cyp(yutData[1].Trim(), true))
                        {
                            runErr.ErrStr += yutData[1] + "气缸执行失败";
                        }
                    }
                }
                else if (imtey[0] == "ca")
                {
                    if (!IsSimulate)
                    {
                        if (!DebugCompiler.GetThis().DDAxis.Cyp(yutData[1].Trim(), false))
                        {
                            runErr.ErrStr += yutData[1] + ":气缸执行失败";
                        }
                    }
                }
                else if (imtey[0] == "do")
                {
                    int det = 0;
                    int inetxt = 0;
                    if (imtey[1].Contains('.'))
                    {
                        int.TryParse(imtey[1].Split('.')[0], out inetxt);
                        int.TryParse(imtey[1].Split('.')[1], out det);
                    }
                    else
                    {
                        int.TryParse(imtey[1], out det);
                    }
                    bool vat = false;
                    if (int.TryParse(yutData[1], out int dett))
                    {
                        vat = Convert.ToBoolean(dett);
                    }
                    else
                    {
                        vat = Convert.ToBoolean(yutData[1]);
                    }
                    if (!DebugCompiler.GetDoDi().WritDO(det, vat))
                    {
                        runErr.ErrStr += "Do写入命令失败:" + text;
                    }
                    for (int i = 1; i < tdat.Length; i++)
                    {
                        yutData = tdat[i].Split('=');
                        det = int.Parse(yutData[0]);
                        if (int.TryParse(yutData[1], out dett))
                        {
                            vat = Convert.ToBoolean(dett);
                        }
                        else
                        {
                            vat = Convert.ToBoolean(yutData[1]);
                        }
                        if (!IsSimulate)
                        {
                            if (!DebugCompiler.GetDoDi().WritDO(det, vat))
                            {
                                runErr.ErrStr += "Do写入命令失败:" + text;
                            }
                        }
                    }
                }
                else if (imtey[0] == "setcam")
                {
                    int det = 0;
                    int inetxt = 0;
                    int.TryParse(tdat[2], out det);
                    int.TryParse(tdat[3], out inetxt);
                    vision.Vision.GetRunNameVision(tdat[1]).GetCam().Key = tdat[2];
                    vision.Vision.GetRunNameVision(tdat[1]).GetCam().RunID = det;
                    vision.Vision.GetRunNameVision(tdat[1]).GetCam().MaxNumbe = inetxt;
                    if (!IsSimulate)
                    {
                        vision.Vision.GetRunNameVision(tdat[1]).GetCam().Straing(vision.Vision.GetRunNameVision(tdat[1]));
                    }
                }
                //else if (imtey[0] == "call")
                //{
                //    int det = 0;
                //    string RunID = tdat[2];
                //    vision.Vision.GetRunNameVision(tdat[1]).GetCam().Key = tdat[2];
                //    if (!int.TryParse(tdat[2], out det))
                //    {
                //        if (DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(tdat[2]))
                //        {
                //            det = DebugCompiler.GetThis().DDAxis.KeyVales[tdat[2]];
                //            RunID = det.ToString();
                //            vision.Vision.GetRunNameVision(tdat[1]).GetCam().Key = RunID;
                //        }
                //    }
                //    if (tdat.Length>=4)
                //    {
                //        if (!int.TryParse(tdat[3], out det))
                //        {
                //            if (DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(tdat[3]))
                //            {
                //                det = DebugCompiler.GetThis().DDAxis.KeyVales[tdat[3]];
                //            }
                //        }
                //    }
                //    vision.Vision.GetRunNameVision(tdat[1]).GetCam().RunID = det;
                //    if (!IsSimulate)
                //    {
                //        vision.Vision.GetRunNameVision(tdat[1]).ReadCamImage(RunID, det);
                //    }
                //}
                //else if (imtey[0] == "cam")
                //{
                //    //HalconRun hate = vision.Vision.GetRunNameVision(tdat[1]);
                //    //if (hate != null)
                //    //{
                //    //    HObject hObject = vision.Vision.GetNameCam(tdat[2]).GetImage();

                //    //    if (int.TryParse(tdat[3], out int det))
                //    //    {
                //    //        if (det == 1)
                //    //        {
                //    //            vision.Vision.GetRunNameVision(tdat[1]).TiffeOffsetImageEX.SetTiffeOff();
                //    //        }
                //    //        vision.Vision.GetRunNameVision(tdat[1]).TiffeOffsetImageEX.SetTiffeOff(hObject, det);
                //    //        hate.Image(vision.Vision.GetRunNameVision(tdat[1]).TiffeOffsetImageEX.TiffeOffsetImage());
                //    //    }
                //    //    hate.ShowImage();
                //    //}
                //}
                else if (imtey[0] == "cam")
                {
                    Cams(runErr);
                }
                else if (imtey[0] == "call")
                {
                    Calls(runErr);
                }
                else if (imtey[0] == "simulationcall")
                {
                    bool IsSave = false;
                    int runid = 0;
                    runid = ToDoubleP(tdat[2]);
                    int lirdID = ToDoubleP(tdat[3]);
                    if (tdat.Length == 5)
                    {
                        int.TryParse(tdat[4], out int rdet);
                        IsSave = Convert.ToBoolean(rdet);
                    }
                    vision.Vision.GetRunNameVision(tdat[1]).CamImageEvent(runid.ToString(), null, lirdID, IsSave);
                }
                else if (imtey[0] == "ng")
                {
                    if (!UserFormulaContrsl.NG)
                    {
                        bool vat = true;
                        yutData = tdat[1].Split('=');
                        if (yutData[0].StartsWith("do"))
                        {
                            int det = int.Parse(yutData[0].Remove(0, 2));
                            if (int.TryParse(yutData[1], out int dett))
                            {
                                vat = Convert.ToBoolean(dett);
                            }
                            else
                            {
                                vat = Convert.ToBoolean(yutData[1]);
                            }
                            if (!DebugCompiler.GetDoDi().WritDO(det, vat))
                            {
                                runErr.ErrStr += "Do写入命令失败:" ;
                            }
                        }
                    }
                }
                else if (imtey[0] == "ok")
                {
                    if (UserFormulaContrsl.NG)
                    {
                        bool vat = true;
                        yutData = tdat[1].Split('=');
                        if (yutData[0].StartsWith("do"))
                        {
                            int det = int.Parse(yutData[0].Remove(0, 2));
                            if (int.TryParse(yutData[1], out int dett))
                            {
                                vat = Convert.ToBoolean(dett);
                            }
                            else
                            {
                                vat = Convert.ToBoolean(yutData[1]);
                            }
                            if (!DebugCompiler.GetDoDi().WritDO(det, vat))
                            {
                                runErr.ErrStr += "Do写入命令失败:" + text;
                            }
                        }
                    }
                }
                else if (imtey[0] == "send")
                {
                    if (ErosSocket.ErosConLink.StaticCon.GetLingkNames().Contains(imtey[1]))
                    {
                        string data = "";
                        for (int i = 1; i < tdat.Length; i++)
                        {
                            data += ToDoubleP(tdat[i]).ToString() + ',';
                        }
                        ErosSocket.ErosConLink.StaticCon.GetSocketClint(imtey[1]).Send(data.Trim(','));
                    }
                    else
                    {
                        runErr.ErrStr = "通信名不存在" + imtey[1];
                    }
                }
                else if (imtey[0] == "weirtfiletray")
                {
                    int det = 0;
                    det = ToDoubleP(imtey[1]);
                    DebugCompiler.GetTrayDataUserControl().WriatTary(ProcessControl.ProcessUser.GetThis().ExcelPath + "\\", text,
                        DebugCompiler.GetTrayDataUserControl().GetTrayEx(det).GetTrayData(), out runErr.ErrStr);
                }//等待读取文件
                else if (imtey[0] == "awaitread")
                {
                    double outTime = 0;
                    if (imtey.Length >= 2)
                    {
                        outTime = Convert.ToDouble(imtey[1]);
                    }
                    int Strtrow = 0;
                    int StrtCol = 0;
                    string rset = "";
                    char Sipe = ',';
                    string StartsWith = "";
                    string filePaht = ProcessControl.ProcessUser.GetThis().ExcelPath + "\\";
                    //   AwaitRead 5, Tray 2 { 行=2; 分割==;数据列=1; 结果=OK; StartsWith=SPUTTERING_Result_; }
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
                    while (true)
                    {
                        if (tdat[1].ToLower().StartsWith("tray"))
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
                        }
                        else if (DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(tdat[2].Trim(' ')))
                        {
                            if (Vision2.ErosProjcetDLL.Excel.Npoi.ReadText(imtey[1], out string textT))
                            {
                                DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[tdat[2]].Value = textT;
                                break;
                            }
                        }
                        if (Done)
                        {
                            break;
                        }
                        if (outTime != 0 && outTime <= Watch.ElapsedMilliseconds / 1000)
                        {
                            runErr.ErrStr += "未找到目标文件";
                            AwaitOut = true;
                            break;
                        }
                        System.Threading.Thread.Sleep(10);
                        if (Stoping)
                        {
                            break;
                        }
                        if (IsSimulate)
                        {
                            break;
                        }
                    }
                }
                //等待超时信号
                else if (imtey[0] == "await") 
                {
                    AwaitOut = false; double det = 0; int dindex = 0; bool ischet = true; double outTime = 0;
                    if (tdat.Length >= 2)
                    {
                        outTime = Convert.ToDouble(tdat[1]);
                    }

                    if (double.TryParse(imtey[1], out det))
                    {
                        Thread.Sleep((int)(det * 1000));
                    }
                    else if (imtey[1].ToLower().StartsWith("di"))
                    {
                        if (yutData.Length == 2)
                        {
                            ischet = Convert.ToBoolean(yutData[1]);
                        }
                        if (imtey[1] == "di")
                        {
                            int.TryParse(imtey[2], out dindex);
                        }
                        else
                        {
                            int.TryParse(imtey[1].Remove(0, 2), out dindex);
                        }
                        while (ischet != DebugCompiler.GetDoDi().Int[dindex])
                        {
                            if (outTime != 0 && outTime <= Watch.ElapsedMilliseconds / 1000)
                            {
                                AwaitOut = true;
                                break;
                            }
                            System.Threading.Thread.Sleep(100);

                            if (Stoping)
                            {
                                break;
                            }
                            if (IsSimulate)
                            {
                                break;
                            }
                        }

                    }
                    else if (imtey[1].ToLower().StartsWith("do"))
                    {
                        if (int.TryParse(imtey[1].Substring(2, imtey[1].Length - 2), out dindex))
                        {
                            if (yutData.Length == 2)
                            {
                                ischet = Convert.ToBoolean(yutData[1]);
                            }

                            while (ischet != DebugCompiler.GetDoDi().Out[dindex])
                            {
                                if (outTime != 0 && outTime <= Watch.ElapsedMilliseconds / 1000)
                                {
                                    AwaitOut = true;
                                    break;
                                }
                                System.Threading.Thread.Sleep(10);
                                if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                                {
                                    break;
                                }
                                if (IsSimulate)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else if (ErosSocket.ErosConLink.StaticCon.GetLingkNames().Contains(imtey[1]))
                    {
                        string dsa = ErosSocket.ErosConLink.StaticCon.GetSocketClint(imtey[1]).AlwaysReceive((int)outTime * 1000);
                        if (tdat.Length >= 3)
                        {
                            if (DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(tdat[2].Trim(' ')))
                            {
                                if (!DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[tdat[2].Trim(' ')].InitialValue(dsa))
                                {
                                    runErr.ErrStr += "接受值与变量类型不符!";
                                }
                            }
                            else
                            {
                                runErr.ErrStr += "变量不存在!";
                            }
                        }
                    }
                }
                //等待信号保持
                else if (imtey[0] == "always")
                {
                    AwaitOut = false; double det = 0; int dindex = 0; bool ischet = true; double outTime = 0;
                    if (tdat.Length == 2)
                    {
                        outTime = Convert.ToDouble(tdat[1]);
                    }
                    if (imtey[1].ToLower().StartsWith("di"))
                    {
                        if (imtey.Length == 3)
                        {

                        }

                        if (int.TryParse(imtey[2], out dindex))
                        {
                            if (yutData.Length == 2)
                            {
                                int dnumber = 0;
                                if (int.TryParse(yutData[1], out dnumber))
                                {
                                    ischet = Convert.ToBoolean(dnumber);
                                }
                                else
                                {
                                    bool.TryParse(yutData[1], out ischet);
                                }
                            }
                            System.Diagnostics.Stopwatch WatchT = new System.Diagnostics.Stopwatch();
                            while (true)
                            {
                                if (ischet != DebugCompiler.GetDoDi().Int[dindex])
                                {
                                    WatchT.Restart();
                                }
                                else
                                {
                                    WatchT.Start();
                                }
                                if (outTime != 0 && outTime <= WatchT.ElapsedMilliseconds / 1000)
                                {
                                    break;
                                }
                                if (outTime == 0 && ischet != DebugCompiler.GetDoDi().Int[dindex])
                                {
                                    break;
                                }
                                System.Threading.Thread.Sleep(10);
                                if (Stoping)
                                {
                                    break;
                                }
                                if (IsSimulate)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else if (imtey[1].ToLower().StartsWith("do"))
                    {
                        if (int.TryParse(imtey[1].Substring(2, imtey[1].Length - 2), out dindex))
                        {
                            if (yutData.Length == 2)
                            {
                                ischet = Convert.ToBoolean(yutData[1]);
                            }
                            while (ischet != DebugCompiler.GetDoDi().Out[dindex])
                            {
                                if (outTime != 0 && outTime <= Watch.ElapsedMilliseconds / 1000)
                                {
                                    AwaitOut = true;
                                    break;
                                }
                                System.Threading.Thread.Sleep(10);
                                if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                                {
                                    break;
                                }

                            }
                        }
                    }

                }
                else if (imtey[0] == "if")
                {
                    int ifType = 0;
                    if (text.Contains("=="))
                    {
                        ifType = 1;
                    }
                    else
                    {
                    }
                    ISif = true;
                    IfRowInt = 0;
                    IFElseBool = true;
                    bool resetl = true;
                    int index = 0;
                    if (yutData.Length == 2)
                    {
                        if (int.TryParse(yutData[1], out int result))
                        {
                            resetl = Convert.ToBoolean(result);
                        }
                        else
                        {
                            resetl = bool.Parse(yutData[1]);
                        }
                    }
                    if (imtey.Length == 3 && imtey[1].Length == 2)
                    {
                        int.TryParse(imtey[2], out index);
                    }
                    else if (int.TryParse(imtey[1].Substring(2, imtey[1].Length - 2), out index))
                    {
                    }
                    if (imtey[1].ToLower().StartsWith("di"))
                    {
                        if (DebugCompiler.GetDoDi().Int[index] == resetl)
                        {
                            IFElseBool = false;
                        }
                    }
                    else if (imtey[1].ToLower().StartsWith("ng"))
                    {
                        if (UserFormulaContrsl.NG)
                        {
                            IFElseBool = false;
                        }
                    }
                    else if (imtey[1].ToLower().StartsWith("ok"))
                    {
                        IFElseBool = UserFormulaContrsl.NG;
                    }
                    else if (imtey[1].ToLower().StartsWith("do"))
                    {
                        if (DebugCompiler.GetDoDi().Out[index] == resetl)
                        {
                            IFElseBool = false;
                        }
                    }
                    else if (imtey[1].ToLower().StartsWith("awaitout"))
                    {
                        if (AwaitOut == resetl)
                        {
                            IFElseBool = false;
                        }
                    }
                    else if (imtey[1].ToLower().StartsWith("tray"))
                    {
                        int idet = ToDoubleP(imtey[2]);
                        if (ifType == 1)
                        {
                            int det = 0;
                            for (int i = 0; i < DebugCompiler.GetThis().DDAxis.GetTrayInxt(idet).GetTrayData().GetDataVales().Count; i++)
                            {
                                if (DebugCompiler.GetThis().DDAxis.GetTrayInxt(idet).GetTrayData().GetDataVales()[i] != null 
                                    && DebugCompiler.GetThis().DDAxis.GetTrayInxt(idet).GetTrayData().GetDataVales()[i].PanelID != null &&
                                    DebugCompiler.GetThis().DDAxis.GetTrayInxt(idet).GetTrayData().GetDataVales()[i].PanelID.ToString() != "")
                                {
                                    det++;
                                }
                            }
                            if (det == ToDoubleP(yutData[2]))
                            {
                                IFElseBool = false;
                            }
                            else
                            {
                                IFElseBool = true;
                            }
                        }
                        else
                        {
                            if (tdat.Length == 2)
                            {

                                if (int.TryParse(ToDoubleP(tdat[1]), out int idt))
                                {
                                    if (DebugCompiler.GetThis().DDAxis.GetTrayInxt(idet).GetTrayData().GetDataVales()[idt] == null ||
                                        DebugCompiler.GetThis().DDAxis.GetTrayInxt(idet).GetTrayData().GetDataVales()[idt].OK)
                                    {
                                        IFElseBool = false;
                                    }
                                    else
                                    {
                                        IFElseBool = true;
                                    }
                                }
                                else if (tdat[1] == "id")
                                {
                                    bool isDone = true;
                                    ErosSocket.DebugPLC.Robot.TrayRobot trayRobot = DebugCompiler.GetThis().DDAxis.GetTrayInxt(idet);
                                    for (int i = 0; i < trayRobot.Count; i++)
                                    {
                                        isDone = false;

                                        if (trayRobot.GetTrayData().GetDataVales()[i] == null ||
                                          trayRobot.GetTrayData().GetDataVales()[i].PanelID == null ||
                                          trayRobot.GetTrayData().GetDataVales()[i].PanelID == "")
                                        {
                                            isDone = true;
                                            break;
                                        }
                                    }
                                    if (isDone)
                                    {
                                        IFElseBool = true;
                                    }
                                    else
                                    {
                                        IFElseBool = false;
                                    }
                                }
                            }
                            else
                            {
                                if (DebugCompiler.GetThis().DDAxis.GetTrayInxt(idet).GetTrayData().OK)
                                {
                                    IFElseBool = false;
                                }
                                else
                                {
                                    IFElseBool = true;
                                }
                            }
                        }
                    }
                    else if (DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(imtey[1]))
                    {
                        ErosSocket.ErosConLink.UClass.GetTypeValue(DebugCompiler.GetThis().DDAxis.KeyVales[imtey[1]]._Type, yutData[1], out dynamic dynamic);

                        if (DebugCompiler.GetThis().DDAxis.KeyVales[imtey[1]].Value == dynamic)
                        {
                            IFElseBool = false;
                        }
                    }
                    if (IFElseBool)
                    {
                        IfRowInt = 1;
                    }
                }
                else if (imtey[0] == "stop")
                {
                    DebugCompiler.Stop(true);
                }
                else if (imtey[0] == "sethome")
                {
                    DebugCompiler.Initialize();
                }
                else if (imtey[0] == "pause")
                {
                    DebugCompiler.Pause();
                    Thread.Sleep(200);
                }
                else if (imtey[0] == "reset")
                {
                    DebugCompiler.Rest();
                }
                else if (imtey[0] == "int")
                {
                    if (!DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(imtey[1]))
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.Add(imtey[1]
                            , new ErosSocket.ErosConLink.UClass.ErosValues.ErosValueD() { _Type = ErosSocket.ErosConLink.UClass.Int16 });
                    }
                    if (yutData.Length == 2)
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Default = yutData[1];
                    }
                    if (DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Value == null)
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].InitialValue();
                    }
                    DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Name = imtey[1];
                }
                else if (imtey[0] == "bool")
                {
                    if (!DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(imtey[1]))
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.Add(imtey[1]
                            , new ErosSocket.ErosConLink.UClass.ErosValues.ErosValueD() { _Type = ErosSocket.ErosConLink.UClass.Boolean });
                    }
                    if (yutData.Length == 2)
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Default = yutData[1];
                    }
                    if (DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Value == null)
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].InitialValue();
                    }
                    DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Name = imtey[1];
                }
                else if (imtey[0] == "double")
                {
                    if (!DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(imtey[1]))
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.Add(imtey[1]
                            , new ErosSocket.ErosConLink.UClass.ErosValues.ErosValueD() { _Type = ErosSocket.ErosConLink.UClass.Double });
                    }
                    if (yutData.Length == 2)
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Default = yutData[1];
                    }
                    if (DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Value == null)
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].InitialValue();
                    }
                    DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Name = imtey[1];
                }
                else if (imtey[0] == "single")
                {
                    if (!DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(imtey[1]))
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.Add(imtey[1]
                            , new ErosSocket.ErosConLink.UClass.ErosValues.ErosValueD() { _Type = ErosSocket.ErosConLink.UClass.Single });
                    }
                    if (yutData.Length == 2)
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Default = yutData[1];
                    }
                    if (DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Value == null)
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].InitialValue();
                    }
                    DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Name = imtey[1];
                }
                else if (imtey[0] == "byte")
                {
                    if (!DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(imtey[1]))
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.Add(imtey[1]
                            , new ErosSocket.ErosConLink.UClass.ErosValues.ErosValueD() { _Type = ErosSocket.ErosConLink.UClass.Byte });
                    }
                    if (yutData.Length == 2)
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Default = yutData[1];
                    }
                    if (DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Value == null)
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].InitialValue();
                    }
                    DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Name = imtey[1];
                }
                else if (imtey[0] == "string")
                {
                    if (!DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(imtey[1]))
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.Add(imtey[1]
                            , new ErosSocket.ErosConLink.UClass.ErosValues.ErosValueD() { _Type = ErosSocket.ErosConLink.UClass.String });
                    }
                    if (yutData.Length == 2)
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Default = yutData[1];
                    }
                    if (DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Value == null)
                    {
                        DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].InitialValue();
                    }
                    DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[1]].Name = imtey[1];
                }
                else if (imtey[0] == "trayvalue")
                {
                    double det = 0; int dindex = 0; bool ischet = true; double outTime = 0;
                    if (imtey.Length >= 2)
                    {
                        dindex = (int)ToDoubleP(imtey[1]);
                    }
                    double Max = 9999999999;
                    double vat = (double)ToDoubleP(tdat[2]);
                    double min = 0;
                    if (tdat.Length >= 5)
                    {
                        if (double.TryParse(tdat[3], out min))
                        {
                            if (ischet && vat < min)
                            {
                                ischet = false;
                            }
                        }
                    }
                    if (tdat.Length == 5)
                    {
                        if (double.TryParse(tdat[4], out Max))
                        {
                            if (ischet && vat > Max)
                            {
                                ischet = false;
                            }
                        }
                    }
                    DebugCompiler.GetThis().DDAxis.GetTrayInxt(dindex).GetTrayData().SetNumberValue((int)ToDoubleP(tdat[1]), ischet, vat);
                }
                else if (imtey[0] == "trayimage")
                {
                    int dindex = 0; 
                    if (imtey.Length >= 2)
                    {
                        dindex = (int)ToDoubleP(imtey[1]);
                    }
                    //DebugCompiler.GetThis().DDAxis.GetTrayInxt(dindex).GetTrayData().SetNumberValue((int)ToDoubleP(tdat[1]),vision.Vision.OneProductVale);
                }
                else if (imtey[0] == "lodatray")
                {
                    if (tdat.Length >= 2)
                    {
                        DebugCompiler.GetTrayDataUserControl().SetTray(DebugCompiler.GetThis().DDAxis.GetTrayInxt((int)ToDoubleP(tdat[1])));
                        if (tdat.Length == 4)
                        {
                            DebugCompiler.GetTrayDataUserControl().SetMinMax(ToDoubleP(tdat[1]), ToDoubleP(tdat[2]));
                        }
                    }
                }
                else if (imtey[0] == "cleartray")
                {
                    if (tdat.Length == 2)
                    {
                        DebugCompiler.GetThis().DDAxis.GetTrayInxt((int)ToDoubleP(tdat[1])).GetTrayData().RestValue();
                    }
                }
                else if (imtey[0] == "settraynumber")
                {
                    DebugCompiler.GetTrayDataUserControl().SetNumber((int)ToDoubleP(tdat[1]));
                }
                else if (imtey[0] == "submitresults")
                {
                    UserFormulaContrsl.WeirtMes();
                }
                else if (imtey[0] == "cleardata")
                {
                    UserFormulaContrsl.ClearData();
                }
                else if (imtey[0] == "settiffeclear")
                {
                    vision.Vision.GetRunNameVision(tdat[1]).TiffeOffsetImageEX.TiffeClose();
                }
                else if (imtey[0] == "settiffeimage")
                {
                    HObject hObject=vision.Vision.GetRunNameVision(tdat[1]).TiffeOffsetImageEX.TiffeOffsetImage();
                    //vision.Vision.OneProductVale.ImagePlus = hObject;
                    vision.Vision.GetSaveImageInfo(tdat[1]).SaveImage(hObject, 0, "拼图", tdat[1],DateTime.Now);
                    vision.Vision.GetRunNameVision(tdat[1]).Image(hObject);
                    vision.Vision.GetRunNameVision(tdat[1]).ShowImage();
                }
                else if (imtey[0] == "for")
                {
                    int strtInt = 0;
                    int endInt = 0;
                    string forName = "";
                    List<string> ForCode = new List<string>();
                    for (int i = this.StepInt; i < CodeStr.Count; i++)
                    {
                        if (CodeStr[i].Contains("{"))
                        {
                            strtInt = i;
                            continue;
                        }
                        if (CodeStr[i].Contains("}"))
                        {
                            endInt = i;
                            break;
                        }
                        if (strtInt > 0)
                        {
                            ForCode.Add(CodeStr[i]);
                        }
                    }
                    int forInt = (int)ToDoubleP(tdat[1]);
                    int Stratdt = 0;
                    if (imtey.Length == 3)
                    {
                        forName = imtey[2];

                        if (yutData.Length == 2)
                        {
                            int.TryParse(yutData[1], out Stratdt);
                        }
                        if (DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(forName))
                        {
                            DebugCompiler.GetThis().DDAxis.KeyVales[forName] = Stratdt;
                        }
                        else
                        {
                            DebugCompiler.GetThis().DDAxis.KeyVales[forName] = Stratdt;
                        }
                    }
                    Stratdt = DebugCompiler.GetThis().DDAxis.KeyVales[forName];
                    for (int i = Stratdt; i < forInt + 1; i++)
                    {
                        for (int it = 0; it < ForCode.Count; it++)
                        {
                            runErr = this.RunSingle(ForCode[it], strtInt + it + 1);
                            if (runErr.Err)
                            {
                                break;
                            }
                        }
                        if (runErr.Err)
                        {
                            break;
                        }
                        DebugCompiler.GetThis().DDAxis.KeyVales[forName]++;
                    }
                    StepInt = endInt;
                }
                else if (DebugCompiler.GetThis().DDAxis.KeyVales.ContainsKey(imtey[0]) && yutData.Length == 2)//全局赋值
                {
                        if (yutData[1].Contains('*'))
                        {
                           string []datas= yutData[1].Split('*');
                           dynamic dava=    ToDoubleP(datas[0])*ToDoubleP(datas[1]);
                            DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[0]].Value = dava;
                        }
                        else    if (yutData[1].Contains('+'))
                        {
                            string[] datas = yutData[1].Split('+');
                            dynamic dava = ToDoubleP(datas[0]) + ToDoubleP(datas[1]);
                            DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[0]].Value = dava;
                        }
                        else    if (yutData[1].Contains('-'))
                        {
                            string[] datas = yutData[1].Split('-');
                            dynamic dava = ToDoubleP(datas[0]) - ToDoubleP(datas[1]);
                            DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[0]].Value = dava;
                        }
                        else      if (yutData[1].Contains('/'))
                        {
                            string[] datas = yutData[1].Split('/');
                            dynamic dava = ToDoubleP(datas[0]) / ToDoubleP(datas[1]);
                            DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[0]].Value = dava;
                        }
                        else      if (yutData[1].Contains('%'))
                        {
                            string[] datas = yutData[1].Split('%');
                            dynamic dava = ToDoubleP(datas[0]) % ToDoubleP(datas[1]);
                            DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[0]].Value = dava;
                        }
                        else
                    {
                        if (ErosSocket.ErosConLink.UClass.GetTypeValue(DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[0]]._Type, yutData[1], out dynamic valyt))
                        {
                            DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[0]].Name = imtey[0];
                            DebugCompiler.GetThis().DDAxis.KeyVales.DictionaryValueD[imtey[0]].Value = valyt;
                        }
                        else
                        {
                            runErr.ErrStr += "转换值不是有效的类型";
                        }
                    }
                }
                else
                {
                    runErr.ErrStr += "未识别的指令：" + text;
                }
                runErr.Done = true;
            }
            catch (Exception ex)
            {
                runErr.ErrStr += ex.Message;
            }
            if (DebugCompiler.RunStop)
            {
                runErr.runCoStr = RunCoStr.执行错误;
            }
            else
            {
                if (runErr.Done)
                {
                    runErr.runCoStr = RunCoStr.执行完成;
                }
            }
            runErr.StepRunTime = Watch.ElapsedMilliseconds;
            Watch.Stop();
            if (runErr.ErrStr != "")
            {
                runErr.ErrStr += runErr.Code;
                runErr.runCoStr = RunCoStr.执行错误;
                runErr.Err = true;
            }
            RunCode?.Invoke(runErr);
            End:
            Watch.Stop();
            return runErr;
        }
        public bool StopIng { get { return Stoping; } }
        bool Stoping;
        public void Stop()

        {
            Stoping = true;
            StepInt = 0;
            Runing = false;
        }
        public void Paseu()
        {

            Paseub = true;
        }
        public void Contr()
        {
            Paseub = false;
        }

        public bool Run()
        {
            try
            {
                if (Runing == true)
                {
                    return false;
                }
                Thread.Sleep(100);
                Stoping = false;
                Paseub = false;
                this.IfRowInt = 0;
                Runing = true;
                this.RunTime = 0;
                RunErr runErrT = new RunErr();
                for (int i = 0; i < this.CodeStr.Count; i++)
                {
                    if (Stoping)
                    {
                        break;
                    }
                    while (Paseub)
                    {
                        Thread.Sleep(10);
                    }
                    RunErr runErr = RunSingle(CodeStr[i], i);
                    RunStratCode?.Invoke(runErr);
                    i = StepInt;
                    this.RunTime += (double)runErr.StepRunTime / 1000;
                    if (CodeStr[i] == "")
                    {
                        continue;
                    }
                    runErr.RowIndx = i;
                    if (runErr.ErrStr != "")
                    {
                        runErrT.Err = true;
                        runErr.ErrStr = (runErr.RowIndx + 1) + runErr.ErrStr;
                        if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.运行中)
                        {
                            DebugCompiler.Pause();
                        }
                        ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new Vision2.ErosProjcetDLL.Project.AlarmText.alarmStruct() { Name = this.Name + "脚本故障", Text = runErr.ErrStr });
                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine(runErr.ErrStr, System.Drawing.Color.Red);
                        Runing = false;
                        return false;
                    }
                    if (IfRowInt > 0)
                    {
                        continue;
                    }
                    if (Stoping)
                    {
                        Runing = false;
                        return false;
                    }
                }
                DODIAxis.SaveValue();
                runErrT.Done = true;
                runErrT.RunTime = this.RunTime;
                Runing = false;
                RunDone?.Invoke(runErrT);
                if (DebugCompiler.RunStop)
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }


        public class RunErr
        {
            public string ErrStr = "";
            public string RunState = "";
            /// <summary>
            /// 代码
            /// </summary>
            public string Code = "";
            public string ParName = "";
            public bool Err;
            public bool Done;
            public int RowIndx;
            public long StepRunTime;
            public double RunTime;
            public RunCoStr runCoStr;
        }
        public enum RunCoStr
        {
            执行中=0,
            执行完成=1,
            执行错误=2,
        }
    }
}
