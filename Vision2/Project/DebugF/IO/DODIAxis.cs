using Advantech.Motion;
using ErosSocket.DebugPLC;
using ErosSocket.DebugPLC.DIDO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project.formula;

namespace Vision2.Project.DebugF.IO
{
    public class DODIAxis : Run_project, IDIDO
    {
        public DODIAxis()
        {
        }
        public void Close()
        {
            SaveValue();
        }

        public List<XYZPoint> XyzPoints { get; set; } = new List<XYZPoint>();

        /// <summary>
        /// 轴集合
        /// </summary>
        public List<Axis> AxisS { get; set; } = new List<Axis>();

        public List<ErosSocket.DebugPLC.Robot.TrayRobot> ListTray { get; set; } = new List<ErosSocket.DebugPLC.Robot.TrayRobot>();

        /// <summary>
        /// 
        /// </summary>
        public List<C154Cylinder> Cylinders { get; set; } = new List<C154Cylinder>();

        public ErosSocket.ErosConLink.UClass.ErosValues KeyVales
        {
            get { return keyValuePairs; }
            set
            {
                if (value != keyValuePairs)
                {
                    keyValuePairs = value;
                    SaveValue();
                }
            }
        }

        public static void SaveValue()
        {
            try
            {
                ErosProjcetDLL.Project.ProjectINI.ClassToJsonSavePath(keyValuePairs, ErosProjcetDLL.Project.ProjectINI.TempPath + "本地变量");
            }
            catch (Exception)
            {
            }
        }

        static ErosSocket.ErosConLink.UClass.ErosValues keyValuePairs
               = new ErosSocket.ErosConLink.UClass.ErosValues();
        public Dictionary<string, List<string>> AxisGrot { get; set; } = new Dictionary<string, List<string>>();
        /// <summary>
        /// 轴组
        /// </summary>
        Dictionary<string, List<Axis>> axisGrot;

        /// <summary>
        /// 获得指定名称的轴组合，不存在为Null;
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Axis> GetAxisGrotName(string name)
        {
            try
            {
                if (axisGrot == null)
                {
                    return null;
                }
                List<Axis> list = new List<Axis>();
                if (AxisGrot.ContainsKey(name))
                {
                    foreach (var item in AxisGrot[name])
                    {
                        for (int i = 0; i < AxisS.Count; i++)
                        {
                            if (item == AxisS[i].Name)
                            {
                                list.Add(AxisS[i]);
                                break;
                            }
                        }
                    }
                    return list;
                    //return axisGrot[name];
                }
            }
            catch (Exception)
            {


            }

            return null;
        }


        /// <summary>
        /// 获取指定的轴
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Axis GetAxisName(string name)
        {
            for (int i = 0; i < AxisS.Count; i++)
            {
                if (name == AxisS[i].Name)
                {
                    return AxisS[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 获得指定轴组指定的轴类型
        /// </summary>
        /// <param name="name">轴组名</param>
        /// <param name="enumAxisType">轴类型</param>
        /// <returns></returns>
        public Axis GetAxisGrotNameEx(string name, EnumAxisType enumAxisType)
        {
            try
            {
                if (name == null || AxisGrot.Count == 1)
                {

                    foreach (var item in AxisGrot)
                    {
                        for (int i = 0; i < item.Value.Count; i++)
                        {
                            for (int i2 = 0; i2 < AxisS.Count; i2++)
                            {
                                if (item.Value[i] == AxisS[i2].Name && AxisS[i2].AxisType == enumAxisType)
                                {
                                    return AxisS[i2];
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (AxisGrot.ContainsKey(name))
                    {
                        for (int i = 0; i < AxisGrot[name].Count; i++)
                        {

                            for (int i2 = 0; i2 < AxisS.Count; i2++)
                            {
                                if (AxisGrot[name][i] == AxisS[i2].Name && AxisS[i2].AxisType == enumAxisType)
                                {
                                    return AxisS[i2];
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
            return null;
        }
        /// <summary>
        /// 获取指定的轴
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public C154Cylinder GetCylinderName(string name)
        {
            for (int i = 0; i < Cylinders.Count; i++)
            {
                if (name == Cylinders[i].Name)
                {
                    return Cylinders[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number">不大于15</param>
        /// <returns></returns>
        public ErosSocket.DebugPLC.Robot.TrayRobot GetTrayInxt(int number)
        {
            if (number > 15)
            {
                return null;
            }
            if (ListTray.Count <= number)
            {
                return null;
            }
            return ListTray[number];
        }

        public override bool Pause()
        {
            Pauseing = true;
            for (int i = 0; i < AxisS.Count; i++)
            {
                AxisS[i].Stop();
            }

            WatchT.Stop();
            HomeCodeT.Paseu();
            RunCodeT.Paseu();
            return false;
        }

        public NameBool Int
        {
            get
            { return intT; }
            set
            { intT = value; }
        }

        NameBool intT = new NameBool();

        public NameBool Out
        {
            get
            { return outT; }

            set
            { outT = value; }
        }

        NameBool outT = new NameBool();

        /// <summary>
        /// 等待信号等于Value并保持多少秒
        /// </summary>
        /// <param name="diNumber">DI编号</param>
        /// <param name="timeS">时间秒</param>
        /// <param name="Value">等于的值，默认True</param>
        public static void AlwaysIO(sbyte diNumber, double timeS, bool Value = true)
        {
            if (diNumber < 0)
            {
                return;
            }
            System.Diagnostics.Stopwatch WatchT = new System.Diagnostics.Stopwatch();
            while (true)
            {
                if (Value != DebugCompiler.GetDoDi().Int[diNumber])
                {
                    WatchT.Restart();
                }
                else
                {
                    WatchT.Start();
                }
                if (timeS != 0 && timeS <= WatchT.ElapsedMilliseconds / 1000)
                {
                    break;
                }
                System.Threading.Thread.Sleep(10);
                if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                {
                    return;
                }
            }
        }
        /// <summary>
        /// 入站信号
        /// </summary>
        public AlwaysIO AlwaysIOInt = new AlwaysIO();
        /// <summary>
        /// 到站信号
        /// </summary>
        public AlwaysIO AlwaysIODot = new AlwaysIO();
        /// <summary>
        /// 出站信号
        /// </summary>
        public AlwaysIO AlwaysIOOut = new AlwaysIO();

        public System.Diagnostics.Stopwatch WatchT = new System.Diagnostics.Stopwatch();

        public int runID = 0;

        public DateTime StartTime;

        public DateTime EndTime;
        /// <summary>
        /// 皮带移动
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="sleep"></param>
        public void SetMoveAxistP(float? Point = null, float? sleep = null)
        {
            try
            {
                if (int.TryParse(DebugCompiler.GetThis().AxisNameS, out int outd))
                {
                    if (!outT[outd])
                    {
                        DebugCompiler.GetDoDi().WritDO(outd, true);
                    }
                }
                else
                {

                    if (Point != null)
                    {
                        this.GetAxisName(DebugCompiler.GetThis().AxisNameS).SetPoint(Point.Value, sleep);
                    }
                    else
                    {
                        if (!this.GetAxisName(DebugCompiler.GetThis().AxisNameS).IsMove)
                        {
                            this.GetAxisName(DebugCompiler.GetThis().AxisNameS).SetMoveTe(true);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public bool IsMove
        {
            get
            {
                if (int.TryParse(DebugCompiler.GetThis().AxisNameS, out int outd))
                {
                    return Out[outd];
                }
                else if (this.GetAxisName(DebugCompiler.GetThis().AxisNameS) != null)
                {
                    return this.GetAxisName(DebugCompiler.GetThis().AxisNameS).IsMove;
                }
                return false;
            }
        }

        public void MoveAxisStop()
        {
            try
            {
                if (int.TryParse(DebugCompiler.GetThis().AxisNameS, out int outd))
                {
                    DebugCompiler.GetDoDi().WritDO(outd, false);
                }
                else
                {
                    if (GetAxisName(DebugCompiler.GetThis().AxisNameS).IsMove)
                    {

                        GetAxisName(DebugCompiler.GetThis().AxisNameS).Stop();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public override bool Run()
        {
            StopCodeT.Stop();
            HomeCodeT.Stop();
            if (ErosProjcetDLL.Project.ProjectINI.In.UsData.IsMet)
            {
                if (ErosProjcetDLL.Project.ProjectINI.In.UserName == "")
                {
                    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText("运行失败，请登录后运行", "");
                    DebugCompiler.EquipmentStatus = ErosSocket.ErosConLink.EnumEquipmentStatus.已停止;
                    return false;
                }
            }
            bool DaowUp = false;
            while (true)
            {
                try
                {
                    if (DebugCompiler.GetThis().IsCtr)
                    {
                        this.GetAxisName(DebugCompiler.GetThis().AxisNameS).AddSeelp();
                        while (SimulateTrayMesForm.RresWait)
                        {
                            if (AlwaysIOOut.TimeValue && Int[DebugCompiler.GetThis().To_Board_DI])
                            {
                                DebugCompiler.GetDoDi().WritDO(DebugCompiler.GetThis().To_Board_DO, false);
                                SetMoveAxistP();
                                Cyp(DebugCompiler.GetThis().LoctionCylinder, false);
                                Thread.Sleep(2000);
                            }
                            else
                            {
                                MoveAxisStop();
                            }
                            Thread.Sleep(10);
                        }

                        if (AlwaysIOInt.Value && !AlwaysIODot.Value)
                        {
                            if (DebugCompiler.GetThis().Modet != 1 || !AlwaysIOOut.Value)
                            {
                                DebugCompiler.GetDoDi().WritDO(DebugCompiler.GetThis().To_Board_DO, false);
                                SetMoveAxistP();
                                Cyp(DebugCompiler.GetThis().LoctionCylinder, false);
                                Cyp(DebugCompiler.GetThis().RCylinder, true);
                                runID = 1;
                            }
                        }
                        else if (AlwaysIOInt.RunTime > DebugCompiler.GetThis().IntTime && !AlwaysIOOut.Value && !AlwaysIODot.Value)
                        {
                            runID = 2;
                            DebugCompiler.GetDoDi().WritDO(DebugCompiler.GetThis().To_Board_DO, true);
                            MoveAxisStop();
                        }
                        if (AlwaysIOOut.TimeValue && !Int[DebugCompiler.GetThis().To_Board_DI])
                        {
                            runID = 3;
                            MoveAxisStop();
                        }
                        else if (AlwaysIOOut.TimeValue && Out[DebugCompiler.GetThis().To_Board_DO])
                        {
                            runID = 4;
                            DebugCompiler.GetDoDi().WritDO(DebugCompiler.GetThis().To_Board_DO, false);
                            SetMoveAxistP();
                            Cyp(DebugCompiler.GetThis().LoctionCylinder, false);
                            Thread.Sleep(2000);
                        }
                        else if (AlwaysIOOut.TimeValue && Int[DebugCompiler.GetThis().To_Board_DI])
                        {
                            runID = 5;
                            DebugCompiler.GetDoDi().WritDO(DebugCompiler.GetThis().To_Board_DO, false);
                            SetMoveAxistP();
                            //Cyp(DebugCompiler.GetThis().LoctionCylinder, false);
                            Thread.Sleep(2000);
                        }
                        if (DaowUp != AlwaysIODot.Value)
                        {
                            DaowUp = AlwaysIODot.Value;
                            if (DaowUp)
                            {
                                //Thread.Sleep(50);
                                runID = 6;
                                Cyp(DebugCompiler.GetThis().RCylinder, false);
                                double d = this.GetAxisName(DebugCompiler.GetThis().AxisNameS).Point+ DebugCompiler.GetThis().WaitPoint;
                                SetMoveAxistP(DebugCompiler.GetThis().WaitPoint, DebugCompiler.GetThis().WaitSleep);
                                int der = 0;
                                while (this.GetAxisName(DebugCompiler.GetThis().AxisNameS).Point <d)
                                {
                                    der++;
                                    if (der>=1000)
                                    {
                                        break;
                                    }
                                    Thread.Sleep(10);
                                }
                                MoveAxisStop();
                                this.GetAxisName(DebugCompiler.GetThis().AxisNameS).AddSeelp();
                            }
                        }
                        bool Runt = true;
                        if (DebugCompiler.GetThis().Modet == 1)
                        {
                            if (AlwaysIOOut.Value)
                            {
                                Runt = false;
                            }
                            else if (AlwaysIOOut.RunTime < 5)
                            {
                                Runt = false;
                            }
                        }
                        if (Runt && AlwaysIODot.TimeValue)
                        {
                            runID = 7;
                            StartTime = DateTime.Now;
                            Cyp(DebugCompiler.GetThis().RCylinder, false);
                            DebugCompiler.GetDoDi().WritDO(DebugCompiler.GetThis().To_Board_DO, false);
                            MoveAxisStop();
                            WatchT.Restart();
                            if (!DebugCompiler.CPMode)
                            {
                                RunCodeT.Run();
                            }
                            runID = 8;
                            this.GetAxisName(DebugCompiler.GetThis().AxisNameS).AddSeelp();
                            WatchT.Stop();
                            while (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.暂停中)
                            {
                                Thread.Sleep(10);
                            }
                            if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                            {
                                RunCodeT.Runing = false;
                                return false;
                            }
                            if (RunCodeT.StopIng)
                            {
                                DebugCompiler.EquipmentStatus = ErosSocket.ErosConLink.EnumEquipmentStatus.已停止;
                                RunCodeT.Runing = false;
                                return false;
                            }
                            SetMoveAxistP();
                            Cyp(DebugCompiler.GetThis().LoctionCylinder, true);
                            while (!AlwaysIOOut.Value)
                            {
                                runID = 9;
                                while (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.暂停中)
                                {
                                    Thread.Sleep(100);
                                }
                                if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                                {
                                    RunCodeT.Runing = false;
                                    return false;
                                }
                                SetMoveAxistP();
                                Cyp(DebugCompiler.GetThis().LoctionCylinder, true);
                                Thread.Sleep(1);
                                if (!AlwaysIODot.Value && AlwaysIODot.RunTime > 10000)
                                {
                                    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new ErosProjcetDLL.Project.AlarmText.alarmStruct() { Name = "线体", Text = "丢版" });
                                    break;
                                }
                            }
                            runID = 10;
                            MoveAxisStop();
                            if (DebugCompiler.GetThis().Modet != 1)
                            {
                                Cyp(DebugCompiler.GetThis().LoctionCylinder, false);
                            }
                            while (AlwaysIOOut.TimeValue && Int[DebugCompiler.GetThis().To_Board_DI])
                            {
                                runID = 11;
                                while (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.暂停中)
                                {
                                    Thread.Sleep(100);
                                }
                                if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                                {
                                    RunCodeT.Runing = false;
                                    return false;
                                }
                                Thread.Sleep(1);
                                SetMoveAxistP();
                            }
                            EndTime = DateTime.Now;
                            MainForm1.MainFormF.toolStripLabel3.Text = "CT" + WatchT.ElapsedMilliseconds / 1000 / 60 + ":" + WatchT.ElapsedMilliseconds / 1000 % 60;
                            Thread.Sleep(1000);
                        }
                        else if (!AlwaysIOOut.TimeValue && runID == 0)
                        {
                            runID = 12;
                            Cyp(DebugCompiler.GetThis().LoctionCylinder, false);
                            Cyp(DebugCompiler.GetThis().RCylinder, true);
                            MoveAxisStop();
                            DebugCompiler.GetDoDi().WritDO(DebugCompiler.GetThis().To_Board_DO, true);
                        }
                    }
                    else
                    {
                        StartTime = DateTime.Now;
                        WatchT.Restart();
                        RunCodeT.Run();
                        EndTime = DateTime.Now;
                        MainForm1.MainFormF.toolStripLabel3.Text = "CT" + WatchT.ElapsedMilliseconds / 1000 / 60 + ":" + WatchT.ElapsedMilliseconds / 1000 % 60;
                        WatchT.Stop();
                    }
                    while (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.暂停中)
                    {
                        Thread.Sleep(100);
                    }
                    if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                    {
                        RunCodeT.Runing = false;
                        return false;
                    }
                    SaveValue();
                    Thread.Sleep(100);
                }
                catch (Exception)
                {
                }
                Thread.Sleep(1);
            }
        }

        public override bool Stop()
        {
            WatchT.Stop();
            HomeCodeT.Stop();
            RunCodeT.Stop();
            if (DebugCompiler.GetThis() != null)
            {
                if (DebugCompiler.GetDoDi() != null)
                {
                    DebugCompiler.GetDoDi().WritDO(DebugCompiler.GetThis().To_Board_DO, false);
                }
            }
            for (int i = 0; i < AxisS.Count; i++)
            {
                AxisS[i].Stop();
            }
            StopCodeT.Run();
            return false;
        }
        Thread thread;
        public override bool SetHome()
        {
            StopCodeT.Stop();
            DebugCompiler.RunStop = false;
            DebugCompiler.ISHome = false;
            if (thread==null || !thread.IsAlive)
            {
                thread = new Thread(() =>
                {
                    if (HomeCodeT.Run())
                    {
                        if (DebugCompiler.RunStop)
                        {
                            DebugCompiler.EquipmentStatus = ErosSocket.ErosConLink.EnumEquipmentStatus.错误停止中;
                            return;
                        }
                        DebugCompiler.EquipmentStatus = ErosSocket.ErosConLink.EnumEquipmentStatus.初始化完成;
                        DebugCompiler.ISHome = true;
                        Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("初始化完成，" + HomeCodeT.RunTime + "S", Color.Green);
                    }
                    else
                    {
                        DebugCompiler.EquipmentStatus = ErosSocket.ErosConLink.EnumEquipmentStatus.错误停止中;
                        Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("初始化失败，" + HomeCodeT.RunTime + "S", Color.Red);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
                return true;
            }
            return false;
        }

        public override bool Cont()

        {
            Pauseing = false;
            WatchT.Start();
            HomeCodeT.Contr();
            RunCodeT.Contr();
            return false;
        }

        public override void Reset()
        {
            try
            {
                if (this.IsInitialBool)
                {
                    foreach (var item in Cylinders)
                    {
                        item.Reset();
                    }

                    foreach (var item in AxisS)
                    {
                        item.Reset();
                    }
                }


                Vision2.ErosProjcetDLL.Project.AlarmListBoxt.RomveAlarmAll();
                //StopCodeT.Run();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public override bool Initial()
        {
            try
            {
                BitDataInt = null;
                try
                {
                    if (System.IO.File.Exists(ErosProjcetDLL.Project.ProjectINI.TempPath + "本地变量.txt"))
                    {
                        ErosProjcetDLL.Project.ProjectINI.ReadPathJsonToCalss(ErosProjcetDLL.Project.ProjectINI.TempPath + "本地变量", out keyValuePairs);
                    }
                }
                catch (Exception)
                {
                }
                if (KeyVales == null)
                {
                    KeyVales = new ErosSocket.ErosConLink.UClass.ErosValues();
                }

                UInt16 CardID_InBit = 0;
                IsInitialBool = false;
                RunCodeT.Single_step = false;
                HomeCodeT.Single_step = false;
                StopCodeT.Single_step = false;
                //TrayRobot.UpS();
                for (int i = 0; i < ListTray.Count; i++)
                {
                    ListTray[i].UpS();
                }
                if (DebugCompiler.GetThis().ListKat == "C154")
                {
                    C154_AxisGrub.ReturnCode = MP_C154.c154_close();//初始化板卡
                    C154_AxisGrub.ReturnCode = MP_C154.c154_initial(ref CardID_InBit, 1);//初始化板卡
                    if (C154_AxisGrub.ReturnCode == 0)
                    {
                        if (AxisS == null || AxisS.Count == 0)
                        {
                            AxisS = new List<Axis>();
                        }
                        for (int i = 0; i < AxisS.Count; i++)
                        {
                            if (AxisS[i] == null)
                            {
                                AxisS[i] = new Axis();
                                AxisS[i].AxisNo = (short)(i);
                            }

                        }
                        for (short AxisNo = 0; AxisNo <= AxisS.Count - 1; AxisNo++)
                        {
                            AxisS[AxisNo].Initial();
                        }
                        IsInitialBool = true;
                        Thread thread = new Thread(() =>
                        {
                            Thread.Sleep(150);
                            Thread threadt = new Thread(() =>
                            {
                                UpCycle?.Invoke(this);
                            });
                            threadt.IsBackground = true;

                            threadt.Start();

                            while (IsInitialBool)
                            {
                                try
                                {
                                    GetStatus();
                                }
                                catch (Exception)
                                {
                                }
                                Thread.Sleep(10);
                            }
                        });
                        thread.IsBackground = true;
                        thread.Priority = ThreadPriority.Highest;
                        thread.Start();
                    }
                }
                else if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
                {
                    PCI1245();
                    IsInitialBool = true;
                    DebugCompiler.GetThis().SetSeelp();
                    Thread thread = new Thread(() =>
                    {
                        while (IsInitialBool)
                        {
                            try
                            {
                                GetStatus();
                            }
                            catch (Exception ex)
                            {
                            }
                            Thread.Sleep(1);
                        }
                    });
                    thread.IsBackground = true;
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                else
                {
                    AlwaysIOInt.Dnumber = (sbyte)DebugCompiler.GetThis().IntCDI;
                    AlwaysIODot.Dnumber = (sbyte)DebugCompiler.GetThis().RunDI;
                    AlwaysIOOut.Dnumber = (sbyte)DebugCompiler.GetThis().OutDi;
                    AlwaysIODot.TimeS = DebugCompiler.GetThis().waitTime;
                    AlwaysIOOut.TimeS = 0.1;
                    AlwaysIOOut.Always();
                    AlwaysIODot.Always();
                    AlwaysIOInt.Always();
                    return true;
                }
                if (DebugCompiler.GetThis().Is_AxisEnble >= 0)
                {
                    this.WritDO(DebugCompiler.GetThis().Is_AxisEnble, true);
                }
                if (DebugCompiler.GetThis().Is_braking >= 0)
                {
                    this.WritDO(DebugCompiler.GetThis().Is_braking, true);
                }
                axisGrot = new Dictionary<string, List<Axis>>();
                foreach (var item in AxisGrot)
                {
                    List<Axis> axess = new List<Axis>();
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        int errOut = 0;
                        for (int it = 0; it < AxisS.Count; it++)
                        {
                            if (item.Value[i] == AxisS[it].Name)
                            {
                                errOut++;
                                if (errOut > 1)
                                {
                                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine("轴组合下的子轴名称存在重复！请更改轴名称");
                                    break;
                                }
                                axess.Add(AxisS[it]);
                            }
                        }
                    }
                    axisGrot.Add(item.Key, axess);
                }
                AlwaysIOInt.Dnumber = (sbyte)DebugCompiler.GetThis().IntCDI;
                AlwaysIODot.Dnumber = (sbyte)DebugCompiler.GetThis().RunDI;
                AlwaysIOOut.Dnumber = (sbyte)DebugCompiler.GetThis().OutDi;
                AlwaysIODot.TimeS = DebugCompiler.GetThis().waitTime;
                AlwaysIOOut.TimeS = DebugCompiler.GetThis().OutwaitTime;
                AlwaysIOOut.Always();
                AlwaysIODot.Always();
                AlwaysIOInt.Always();
                if (IsInitialBool)
                {
                }
                else
                {
                    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText("板卡初始化失败");
                }
                return IsInitialBool;
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText("板卡初始化失败" + ex.Message);
            }
            return false;
        }
        int Result;
        Int16[] Axis_Gear = new Int16[8];
        DEV_LIST[] CurAvailableDevs = new DEV_LIST[Motion.MAX_DEVICES];
        uint deviceCount = 0;
        IntPtr m_DeviceHandle0 = IntPtr.Zero;
        IntPtr m_DeviceHandle1 = IntPtr.Zero;
        IntPtr[] m_Axishand = new IntPtr[32];

        IntPtr[] m_DeviceHandle;

        StringBuilder ErrorMsg = new StringBuilder("", 100);

        uint UResult
        {
            get
            {
                return uResul;
            }
            set
            {
                if (value != 0)
                {
                    Boolean res = Motion.mAcm_GetErrorMessage(value, ErrorMsg, 100);
                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(ErrorMsg.ToString());
                }
                uResul = value;
            }
        }

        uint uResul;
        void PCI1245()
        {
            int i = 0;
            for (i = 0; i < 8; i++)
            {
                Motion.mAcm_AxResetError(m_Axishand[i]);
                Motion.mAcm_AxStopDec(m_Axishand[i]);
            }
            for (i = 0; i < 8; i++)
            {
                Motion.mAcm_AxClose(ref m_Axishand[i]);
            }
            Motion.mAcm_DevClose(ref m_DeviceHandle0);
            Motion.mAcm_DevClose(ref m_DeviceHandle1);
            string[] ConfigFile0 = new string[] { "D:\\PLC\\1245LIO_0.cfg", "D:\\PLC\\1245LIO_1.cfg", "", "" };
            Result = Motion.mAcm_GetAvailableDevs(CurAvailableDevs, Motion.MAX_DEVICES, ref deviceCount);
            uint[] axePerDev = new uint[deviceCount];
            m_DeviceHandle = new IntPtr[deviceCount];
            for (i = 0; i < deviceCount; i++)
            {
                UResult = Motion.mAcm_DevOpen(CurAvailableDevs[i].DeviceNum, ref m_DeviceHandle[i]);            //打开用的板卡
                UResult = Motion.mAcm_GetU32Property(m_DeviceHandle[i], (uint)PropertyID.FT_DevAxesCount, ref axePerDev[i]);            //获取使用的板卡所包含的轴数
                for (int j = 0; j < axePerDev[i]; j++)
                {
                    int det = (UInt16)(i * axePerDev[i] + j);
                    UResult = Motion.mAcm_AxOpen(m_DeviceHandle[i], (UInt16)j, ref m_Axishand[det]);
                    for (int i2 = 0; i2 < AxisS.Count; i2++)
                    {
                        if (AxisS[i2].AxisNoEx == det)
                        {
                            AxisS[i2].SetAxisHandEx(m_Axishand[det]);
                            break;
                        }
                    }
                    for (int i2 = 0; i2 < AxisS.Count; i2++)
                    {
                        if (AxisS[i2].AxisNo == det)
                        {
                            AxisS[i2].SetAxisHand(m_Axishand[det]);
                            //AxisS[i2].Initial();
                            break;
                        }
                    }
                }
                for (int i2 = 0; i2 < AxisS.Count; i2++)
                {
                        AxisS[i2].Initial();
                }
                if (CurAvailableDevs[i].DeviceName != "")
                {
                    UResult = Motion.mAcm_DevLoadConfig(m_DeviceHandle[i], ConfigFile0[i]);
                }
            }
            Thread.Sleep(500);
        }

        /// <summary>
        /// 跟新状态委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public delegate void UPCycleEvent<T>(T key);

        /// <summary>
        /// 跟新状态
        /// </summary>
        public event UPCycleEvent<DODIAxis> UpCycle;

        public void SetTray(int name, string p1, string p2, string p3, string p4, string x, string y)
        {
            try
            {
                if (ListTray == null)
                {
                    ListTray = new List<ErosSocket.DebugPLC.Robot.TrayRobot>();
                }
                if (ListTray.Count == 0)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        ListTray.Add(new ErosSocket.DebugPLC.Robot.TrayRobot() { Name = i.ToString(), });
                    }
                }
                else
                {
                    for (int i = 0; i < 15; i++)
                    {
                        ListTray[i].Name = i.ToString();
                    }
                }

                ListTray[name].P1 = toPointFile(GetToPoint(p1));
                ListTray[name].P2 = toPointFile(GetToPoint(p2));
                ListTray[name].P3 = toPointFile(GetToPoint(p3));
                ListTray[name].P4 = toPointFile(GetToPoint(p4));

                ListTray[name].XNumber = (sbyte)RunCodeStr.ToDoubleP(x);
                ListTray[name].YNumber = (sbyte)RunCodeStr.ToDoubleP(y);
                ListTray[name].Calculate(out HalconDotNet.HTuple liset, out HalconDotNet.HTuple hTuple);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        public void SetTray(int name, string p1, string p2, string p3, string p4, string x, string y, string p5, string p6, string p7, string p8, string x2, string y2)
        {
            try
            {
                if (ListTray == null)
                {
                    ListTray = new List<ErosSocket.DebugPLC.Robot.TrayRobot>();
                }
                if (ListTray.Count == 0)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        ListTray.Add(new ErosSocket.DebugPLC.Robot.TrayRobot() { Name = i.ToString(), });
                    }
                }
                else
                {
                    for (int i = 0; i < 15; i++)
                    {
                        ListTray[i].Name = i.ToString();
                    }
                }
                ListTray[name].P1 = toPointFile(GetToPoint(p1));
                ListTray[name].P2 = toPointFile(GetToPoint(p2));
                ListTray[name].P3 = toPointFile(GetToPoint(p3));
                ListTray[name].P4 = toPointFile(GetToPoint(p4));
                ListTray[name].P5 = toPointFile(GetToPoint(p5));
                ListTray[name].P6 = toPointFile(GetToPoint(p6));
                ListTray[name].P7 = toPointFile(GetToPoint(p7));
                ListTray[name].P8 = toPointFile(GetToPoint(p8));
                ListTray[name].Is8Point = true;
                ListTray[name].XNumber = (sbyte)RunCodeStr.ToDoubleP(x);
                ListTray[name].YNumber = (sbyte)RunCodeStr.ToDoubleP(y);
                ListTray[name].X2Number = (sbyte)RunCodeStr.ToDoubleP(x2);
                ListTray[name].Y2Number = (sbyte)RunCodeStr.ToDoubleP(y2);
                ListTray[name].Calculate(ListTray[name].XNumber, ListTray[name].YNumber, ListTray[name].P1, ListTray[name].P2, ListTray[name].P3, ListTray[name].P4,
                   (sbyte)RunCodeStr.ToDoubleP(x2), (sbyte)RunCodeStr.ToDoubleP(y2), ListTray[name].P5, ListTray[name].P6, ListTray[name].P7, ListTray[name].P8,
                   out PointFile[] pointFile);
                //ListTray[name].Calculate(out HalconDotNet.HTuple liset, out HalconDotNet.HTuple hTuple);
            }
            catch (Exception)
            {
            }

        }
        /// <summary>
        /// 获取全局点位坐标
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public XYZPoint GetToPoint(string text)
        {
            XYZPoint pointFile = new XYZPoint();
            if (text == null)
            {
                return null;
            }
            if (text.StartsWith("P["))
            {
                text = RunCodeStr.ToDoubleP(text).ToString();
            }
            else
            {
                return RunCodeStr.ToDoubleP(text);
            
            }

            for (int i = 0; i < XyzPoints.Count; i++)
            {
                if (XyzPoints[i].Name == text)
                {
                    pointFile.X = XyzPoints[i].X;
                    pointFile.Y = XyzPoints[i].Y;
                    pointFile.Z = XyzPoints[i].Z;
                    return pointFile;
                }
            }
            return null;
        }
        public PointFile toPointFile(XYZPoint xYZPoint)
        {
            PointFile pointFile = new PointFile();
            pointFile.X = xYZPoint.X;
            pointFile.Y = xYZPoint.Y;
            pointFile.Z = xYZPoint.Z;
            return pointFile;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public XYZPoint GetToPointFileProt(string name)
        {

            if (name == null)
            {
                return null;
            }
            if (name.StartsWith("P["))
            {
                name = RunCodeStr.ToDoubleP(name).ToString();
            }
            List<XYZPoint> points = RecipeCompiler.GetProductEX().DPoint;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Name == name)
                {
                    return points[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 获得当前产品的点位名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetToPointName()
        {
            List<string> vs = new List<string>();
            List<XYZPoint> points = RecipeCompiler.GetProductEX().DPoint;
            for (int i = 0; i < points.Count; i++)
            {
                vs.Add(points[i].Name);
            }
            return vs;
        }

        sbyte[] BitDataInt;
        sbyte[] BitDataOut;
        sbyte[] BitDataIntID;
        sbyte[] BitDataOutID;
        /// <summary>
        /// 更新状态
        /// </summary>
        void GetStatus()
        {
            short DIvalue = 0;
            short DOvalue = 0;

            if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
            {
                if (DebugCompiler.GetThis().ListIO)
                {


                }
                else
                {
                    if (BitDataInt == null)
                    {
                        BitDataInt = new sbyte[intT.Count];
                        BitDataOut = new sbyte[Out.Count];
                        BitDataIntID = new sbyte[intT.Count];
                        BitDataOutID = new sbyte[Out.Count];
                        for (short i = 0; i < intT.Count; i++)
                        {
                            string[] dse = intT.LinkIDs[i].Split('.');
                            if (dse.Length == 2)
                            {
                                BitDataInt[i] = sbyte.Parse(dse[1]);
                            }
                            else
                            {
                                BitDataInt[i] = -1;
                            }
                            BitDataIntID[i] = sbyte.Parse(dse[0]);
                            dse = Out.LinkIDs[i].Split('.');
                            if (dse.Length == 2)
                            {
                                BitDataOut[i] = sbyte.Parse(dse[1]);
                            }
                            else
                            {
                                BitDataOut[i] = -1;
                            }
                            if (sbyte.TryParse(dse[0], out sbyte resubyte))
                            {
                                BitDataOutID[i] = resubyte;
                            }

                        }

                    }
                    //输入
                    for (short i = 0; i < intT.Count; i++)
                    {
                        byte intsd = 0;
                        byte outVa = 0;
                        uint err = 0;
                        if (BitDataInt[i] >= 0)
                        {
                            err = Motion.mAcm_AxDiGetBit(m_Axishand[BitDataIntID[i]], (byte)BitDataInt[i], ref intsd);
                            if (err == 0)
                            {
                                intT[i] = intsd == 0 ? false : true;
                            }
                        }
                        if (BitDataOut[i] >= 0)
                        {
                            err = Motion.mAcm_AxDoGetBit(m_Axishand[BitDataOutID[i]], (byte)BitDataOut[i], ref outVa);
                            if (err == 0)
                            {
                                outT[i] = outVa == 0 ? false : true;
                            }
                        }
                    }
                }
            }
            else
            {
                for (short i = 0; i < 16; i++)
                {
                    C154_AxisGrub.ReturnCode = MP_C154.c154_get_gpio_input_ex_CH(0, i, ref DIvalue);
                    C154_AxisGrub.ReturnCode = MP_C154.c154_get_gpio_output_ex_CH(0, i, ref DOvalue);
                    intT[i] = DIvalue == 0 ? false : true;
                    outT[i] = DOvalue == 0 ? false : true;
                }
                if (DebugCompiler.GetThis().Cont > 1)
                {
                    for (short i = 0; i < 16; i++)
                    {
                        C154_AxisGrub.ReturnCode = MP_C154.c154_get_gpio_input_ex_CH(1, i, ref DIvalue);
                        C154_AxisGrub.ReturnCode = MP_C154.c154_get_gpio_output_ex_CH(1, i, ref DOvalue);
                        intT[16 + i] = DIvalue == 0 ? false : true;
                        outT[16 + i] = DOvalue == 0 ? false : true;
                    }
                }
            }
            for (int i = 0; i < AxisS.Count; i++)
            {
                AxisS[i].GetStatus();
            }
        }

        /// <summary>
        /// 使用轴组动作
        /// </summary>
        /// <param name="groupName">轴组名称</param>
        /// <param name="outTime">等待到达时间</param>
        /// <param name="xp">X目标值，999999999为不移动X</param>
        /// <param name="yp">Y目标值，999999999为不移动X</param>
        /// <param name="zp">Z目标值，999999999为不移动X</param>
        /// <param name="isMove">是否使用门型动作</param>
        /// <returns></returns>
        public bool SetXYZ1Points(string groupName, int outTime = 0, double? xp = null, double? yp = null, double? zp = null, double? up = null, EnumXYZUMoveType isMove = EnumXYZUMoveType.直接移动, double? jumpZ = 0)
        {
            Axis axisX = null;
            Axis axisY = null;
            Axis axisZ = null;
            Axis axisU = null;
            try
            {

                if (axisGrot == null || !axisGrot.ContainsKey(groupName))
                {
                    throw new Exception(groupName + "轴组不存在！");
                }
                axisX = GetAxisGrotNameEx(groupName, EnumAxisType.X);
                axisY = GetAxisGrotNameEx(groupName, EnumAxisType.Y);
                axisZ = GetAxisGrotNameEx(groupName, EnumAxisType.Z);
                axisU = GetAxisGrotNameEx(groupName, EnumAxisType.U);
                if (axisU == null)
                {
                    axisU = GetAxisGrotNameEx(groupName, EnumAxisType.Udd);
                }
                if ((axisU != null && !axisU.IsHome) && (axisZ != null && !axisZ.IsHome) && !axisX.IsHome && !axisY.IsHome)
                {
                    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new ErosProjcetDLL.Project.AlarmText.alarmStruct() { Name = groupName, Text = "未初始化" });
                    return false;
                }
                if ((xp == null || axisX == null || axisX.Point == xp) && (yp == null || axisY == null || axisY.Point == yp) && (zp == null || axisZ == null || axisZ.Point == zp) && (up == null || axisU == null || axisU.Point == up))
                {
                    return true;
                }
                System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                double deviatcion = 0.2;
                if (axisZ != null)
                {
                    if (isMove == EnumXYZUMoveType.跳跃门型)
                    {
                        if (!axisZ.SetWPoint((float)jumpZ))
                        {
                            return false;
                        }
                    }
                }
                if (isMove == EnumXYZUMoveType.先旋转再移动)
                {
                    watch.Start();
                    if (axisU != null)
                    {
                        axisU.SetPoint(up);
                    }
                    bool bu = false;
                    while (watch.ElapsedMilliseconds / 1000 < outTime)
                    {
                        if (axisU != null)
                        {
                            if (!axisU.IsMove)
                            {
                                axisU.SetPoint(up);
                            }
                        }
                        double vu = Math.Abs((double)(axisU.Point - up));
                        bu = (vu < deviatcion && vu > -deviatcion);
                        if (DebugCompiler.RunStop)
                        {
                            return false;
                        }
                        if (bu)
                        {
                            break;
                        }
                    }
                    if (!bu)
                    {
                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine(groupName + "轴组U移动超时,目标" + up + "当前" + axisU.Point);
                    }
                }
                if (axisX != null)
                {
                    axisX.SetPoint((float)xp);
                }
                if (axisY != null)
                {
                    axisY.SetPoint((float)yp);
                }
                if (isMove == EnumXYZUMoveType.直接移动)
                {
                    if (axisZ != null)
                    {
                        axisZ.SetPoint(zp);
                    }
                    if (axisU != null)
                    {
                        axisU.SetPoint(up);
                    }
                }
                watch.Restart();
                while (watch.ElapsedMilliseconds / 1000 < outTime)
                {
                    double px = Math.Abs((double)(axisX.Point - xp));
                    double py = Math.Abs((double)(axisY.Point - yp));
                    bool bx = (xp == null || axisX == null || (px < deviatcion && px > -deviatcion));
                    bool by = (yp == null || axisY == null || (py < deviatcion && py > -deviatcion));
                    if (DebugCompiler.RunStop)
                    {
                        return false;
                    }
                    if (bx && by)
                    {
                        if (axisZ != null)
                        {
                            axisZ.SetPoint(zp);
                        }
                        if (axisU != null)
                        {
                            axisU.SetPoint(up);
                        }
                        while (watch.ElapsedMilliseconds / 1000 < outTime)
                        {
                            if (DebugCompiler.RunStop)
                            {
                                return false;
                            }
                            bx = (xp == null || axisX == null || (px < deviatcion && px > -deviatcion));
                            by = (yp == null || axisY == null || (py < deviatcion && py > -deviatcion));
                            bool bz = true;
                            bool bu = true;
                            if (axisZ != null && zp != null)
                            {
                                double pz = Math.Abs((double)(axisZ.Point - zp));
                                bz = (zp == null || axisZ == null || (pz < deviatcion && pz > -deviatcion));
                            }
                            if (axisU != null && up != null)
                            {
                                double pu = Math.Abs((double)(axisU.Point - up));
                                bu = (up == null || axisU == null || (pu < deviatcion && pu > -deviatcion));
                            }
                            if (bx && by && bz && bu)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(groupName + ex.Message);
            }

            return false;
        }


        /// <summary>
        /// 获取轴组值
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="xp"></param>
        /// <param name="yp"></param>
        /// <param name="zp"></param>
        public void GetAxisGroupPoints(string groupName, out double? xp, out double? yp, out double? zp, out double? up)
        {
            xp = yp = zp = up = null;
            if (axisGrot == null || !axisGrot.ContainsKey(groupName))
            {
                throw new Exception(groupName + "轴组不存在！");
            }
            try
            {
                for (int i = 0; i < axisGrot[groupName].Count; i++)
                {
                    if (axisGrot[groupName][i].AxisType == EnumAxisType.X)
                    {
                        xp = axisGrot[groupName][i].Point;
                    }
                    if (axisGrot[groupName][i].AxisType == EnumAxisType.Y)
                    {
                        yp = axisGrot[groupName][i].Point;
                    }
                    if (axisGrot[groupName][i].AxisType == EnumAxisType.Z)
                    {
                        zp = axisGrot[groupName][i].Point;
                    }
                    if (axisGrot[groupName][i].AxisType == EnumAxisType.U)
                    {
                        up = axisGrot[groupName][i].Point;
                    }
                    if (axisGrot[groupName][i].AxisType == EnumAxisType.Udd)
                    {
                        up = axisGrot[groupName][i].Point;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 单轴移动
        /// </summary>
        /// <param name="name"></param>
        /// <param name="point"></param>
        public void AxisGo(string name, string point, string seelpS = null)
        {
            double det = RunCodeStr.ToDoubleP(point);
            if (det == null)
            {
                return;
            }
            if (seelpS != null)
            {
                double seelpt = RunCodeStr.ToDoubleP(seelpS);
                if (seelpt != null)
                {
                    AxisSeelp(name, (double)seelpt);
                }
            }
            AxisGo(name, det);
        }
        public void AxisSeelp(string name, double seelp, double? stv = null, double? acc = null, double? dec = null)
        {
            for (int i = 0; i < AxisS.Count; i++)
            {
                if (AxisS[i].Name == name)
                {
                    if (stv != null)
                    {
                        AxisS[i].StrVal = stv.Value;
                    }
                    if (acc != null)
                    {
                        AxisS[i].Tacc = acc.Value;
                    }
                    if (dec != null)
                    {
                        AxisS[i].Tdec = dec.Value;
                    }
                    AxisS[i].MaxVel = seelp;
                    AxisS[i].AddSeelp();
                    return;
                }
            }
            throw (new Exception(name + ":轴不存在"));
        }
        public void AxisSeelp(string name)
        {
            for (int i = 0; i < AxisS.Count; i++)
            {
                if (AxisS[i].Name == name)
                {
                    switch (DebugCompiler.GetThis().LinkSeelpTyoe)
                    {
                        case 0:
                            AxisS[i].Tdec = this.AxisS[i].LowDceValue;
                            AxisS[i].Tacc = this.AxisS[i].LowAceValue;
                            AxisS[i].StrVal = this.AxisS[i].LowStrValue;
                            AxisS[i].MaxVel = this.AxisS[i].LowMavValue;
                            break;
                        case 1:
                            AxisS[i].Tdec = this.AxisS[i].DceValue;
                            AxisS[i].Tacc = this.AxisS[i].AceValue;
                            AxisS[i].StrVal = this.AxisS[i].StrValue;
                            AxisS[i].MaxVel = this.AxisS[i].MavValue;
                            break;
                        case 2:
                            AxisS[i].Tdec = this.AxisS[i].HighDceValue;
                            AxisS[i].Tacc = this.AxisS[i].HighAceValue;
                            AxisS[i].StrVal = this.AxisS[i].HighStrValue;
                            AxisS[i].MaxVel = this.AxisS[i].HighMavValue;
                            break;
                        default:
                            break;
                    }
                    AxisS[i].AddSeelp();
                    return;
                }
            }
            throw (new Exception(name + ":轴不存在"));
        }
        /// <summary>
        /// 单轴移动
        /// </summary>
        /// <param name="name"></param>
        /// <param name="point"></param>
        public void AxisGo(string name, double point, double seelp = 0)
        {

            for (int i = 0; i < AxisS.Count; i++)
            {
                if (AxisS[i].Name == name)
                {
                    if (seelp != 0)
                    {
                        AxisS[i].StrVal = seelp;
                    }
                    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
                    AxisS[i].SetPoint((float)point);
                    if (AxisS[i].AxisType == EnumAxisType.S)
                    {
                        //return;
                    }
                    while (AxisS[i].Point != point)
                    {
                        if (watch.ElapsedMilliseconds / 1000 > AxisS[i].HomeTime)
                        {
                            throw (new Exception(name + ":轴超时未到达" + point));
                        }
                        Thread.Sleep(10);
                    }
                    return;
                }
            }
            throw (new Exception(name + ":轴不存在"));
        }


        public void AxisSycnGo(string name, double point, double seelp = 0)
        {
            for (int i = 0; i < AxisS.Count; i++)
            {
                if (AxisS[i].Name == name)
                {
                    if (seelp != 0)
                    {
                        AxisS[i].StrVal = seelp;
                    }

                    AxisS[i].SetPoint((float)point);

                    return;
                }
            }
            throw (new Exception(name + ":轴不存在"));
        }
        public double GetAxisPoint(string name)
        {
            for (int i = 0; i < AxisS.Count; i++)
            {
                if (AxisS[i].Name == name)
                {
                    return AxisS[i].Point;
                }
            }
            throw (new Exception(name + ":轴不存在"));
        }
        public bool AxisSycnGo(string[] name, string[] point1)
        {
            if (name.Length != point1.Length)
            {
                return false;
            }
            for (int i = 0; i < name.Length; i++)
            {
                string text = point1[i];

                double det = RunCodeStr.ToDoubleP(text);
                if (det == null)
                {
                    return false;
                }
                AxisSycnGo(name[i], det);
            }

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds / 1000 <= 30)
            {
                if (RunCodeT.StopIng)
                {
                    return false;
                }
                int errt = 0;

                for (int i = 0; i < name.Length; i++)
                {
                    double det = RunCodeStr.ToDoubleP(point1[i]);
                    double dinte = Math.Abs(GetAxisPoint(name[i]) - det);

                    if (dinte < 0.1)
                    {
                        errt++;
                    }
                    else
                    {
                    }
                }
                if (errt == name.Length)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 气缸移动
        /// </summary>
        /// <param name="name"></param>
        /// <param name="point"></param>
        public bool Cy(string name, string ap)
        {
            if (ap.ToLower() == "p")
            {
                return Cyp(name, true);
            }
            else
            {
                return Cyp(name, false);
            }

        }
        /// <summary>
        /// 气缸移动
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ap">true伸出p，false缩回A</param>
        public bool Cyp(string name, bool ap)
        {
            for (int i = 0; i < Cylinders.Count; i++)
            {
                if (Cylinders[i].Name == name)
                {
                    if (ap)
                    {
                        return Cylinders[i].Protrude(false);
                    }
                    else
                    {
                        return Cylinders[i].Anastole(false);
                    }
                }
            }
            return false;
        }

        public void RunPogrma(XYZPoint xYZPoint)
        {
            SetXYZ1Points(xYZPoint.AxisGrabName, 10, (float)xYZPoint.X, (float)xYZPoint.Y, (float)xYZPoint.Z, (float)xYZPoint.U, xYZPoint.isMove);
            Vision2.vision.Vision.GetRunNameVision(null).ReadCamImage(xYZPoint.ID.ToString(), xYZPoint.ID);
            Thread.Sleep(500);
        }
        public bool ReadDO(int number)
        {
            return false;
        }

        public bool ReadDI(int number)
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intex"></param>
        /// <param name="inde"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WritDO(int intex, int inde, bool value)
        {
            try
            {
                if (!this.IsInitialBool)
                {
                    return false;
                }
                if (DebugCompiler.GetThis().ListIO)
                {

                    if (DebugCompiler.GetDoDi()!=null)
                    {
                        return DebugCompiler.GetDoDi().WritDO(intex, inde, value);
                    }
                    return false;
                }

                if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
                {
                    string[] vs = this.outT.LinkIDs[inde + 16 * intex].Split('.');
                    int idx = int.Parse(vs[0]);
                    ushort idt = ushort.Parse(vs[1]);
                    if (value)
                    {
                        UResult = Motion.mAcm_AxDoSetBit(m_Axishand[idx], idt, 1);
                    }
                    else
                    {
                        UResult = Motion.mAcm_AxDoSetBit(m_Axishand[idx], idt, 0);
                    }
                    if (UResult == 0)
                    {
                        return true;
                    }
                }
                else
                {
                    if (value)
                    {
                        C154_AxisGrub.ReturnCode = MP_C154.c154_set_gpio_output_ex_CH((sbyte)intex, (short)inde, 1);
                    }
                    else
                    {
                        C154_AxisGrub.ReturnCode = MP_C154.c154_set_gpio_output_ex_CH((sbyte)intex, (short)inde, 0);
                    }
                    if (C154_AxisGrub.ReturnCode == 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public bool WritDO(int intex, bool value)
        {
            if (intex < 0)
            {
                return false;
            }
            if (!this.IsInitialBool)
            {
                Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("未初始化成功,写入输出" + intex + "失败");
                return false;
            }
            return WritDO(intex / 16, intex % 16, value);
        }


        /// <summary>
        /// 轴定义
        /// </summary>



        public delegate int Compile(String command, StringBuilder inf);
        //编译  
    }

    public enum EnumXYZUMoveType
    {

        直接移动 = 0,

        跳跃门型 = 1,

        先旋转再移动 = 2,

        先移动再旋转 = 3,

    }
    public class XYZPoint
    {
        public string Name { get; set; } = "";
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double U { get; set; }
        public int ID { get; set; }
        public EnumXYZUMoveType isMove { get; set; }
        [Description("2D坐标彷射参数"), Category("坐标系统"), DisplayName("坐标系统"),
         TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("ListCoordinateName")]
        public string AxisGrabName { get; set; }
        public static List<string> ListCoordinateName
        {
            get
            {
                string[] liast = new string[DebugCompiler.GetThis().DDAxis.AxisGrot.Keys.Count];
                DebugCompiler.GetThis().DDAxis.AxisGrot.Keys.CopyTo(liast, 0);
                return new List<string>(liast);
            }
        }
    }

    public class Axis : ErosProjcetDLL.Project.INodeNew, IAxis
    {
        [DescriptionAttribute("正常最大速度"), Category("中速度"), DisplayName("最大速度")]
        public float MavValue { get; set; } = 500;

        [DescriptionAttribute("启动速度"), Category("中速度"), DisplayName("启动速度")]
        public float StrValue { get; set; } = 50;

        [DescriptionAttribute("正常加速度"), Category("中速度"), DisplayName("加速度")]
        public float AceValue { get; set; } = 500;

        [DescriptionAttribute("正常减速速度"), Category("中速度"), DisplayName("减速速度")]
        public float DceValue { get; set; } = 500;

        [DescriptionAttribute("最大速度"), Category("高速度"), DisplayName("最大速度")]
        public float HighMavValue { get; set; } = 1000;

        [DescriptionAttribute("启动速度"), Category("高速度"), DisplayName("启动速度")]
        public float HighStrValue { get; set; } = 800;

        [DescriptionAttribute("加速度"), Category("高速度"), DisplayName("加速度")]
        public float HighAceValue { get; set; } = 800;

        [DescriptionAttribute("减速速度"), Category("高速度"), DisplayName("减速速度")]
        public float HighDceValue { get; set; } = 800;

        [DescriptionAttribute("最大速度"), Category("低速度"), DisplayName("最大速度")]
        public float LowMavValue { get; set; } = 100;

        [DescriptionAttribute("启动速度"), Category("低速度"), DisplayName("启动速度")]
        public float LowStrValue { get; set; } = 20;

        [DescriptionAttribute("加速度"), Category("低速度"), DisplayName("加速度")]
        public float LowAceValue { get; set; } = 200;

        [DescriptionAttribute("减速速度"), Category("低速度"), DisplayName("减速速度")]
        public float LowDceValue { get; set; } = 200;


        [DescriptionAttribute("。"), Category("回零参数"), DisplayName("回零低速")]
        public int PAR_AxVelLow { get; set; } = 50;
        [DescriptionAttribute("。"), Category("回零参数"), DisplayName("回零高速")]
        public int PAR_AxVelHigh { get; set; } = 200;
        [DescriptionAttribute("。"), Category("回零参数"), DisplayName("回零加速")]
        public int PAR_AxAcc { get; set; } = 1000;
        [DescriptionAttribute("。"), Category("回零参数"), DisplayName("回零减速")]
        public int PAR_AxDec { get; set; } = 1000;

        [DescriptionAttribute("10或16，具体参考轴卡手册。"), Category("回零参数"), DisplayName("回零方式")]
        public uint HomeTepy { get; set; } = 11;

        [DescriptionAttribute("回零后将零点设置为此值"), Category("回零参数"), DisplayName("零点偏移位置")]
        public float HomePoint { get; set; } = 0;
        [DescriptionAttribute("。"), Category("基本参数"), DisplayName("反馈转换")]
        public float Ratio { get; set; } = 1;
        [DescriptionAttribute("。"), Category("限位"), DisplayName("最大软限位")]
        public int PlusLimit { get; set; } = 400;
        [DescriptionAttribute("。"), Category("限位"), DisplayName("最小软限位")]
        public int MinusLimit { get; set; } = -10;
        [DescriptionAttribute("。"), Category("初始化"), DisplayName("初始化超时")]
        public int HomeTime { get; set; } = 100;
        [DescriptionAttribute("脉冲与MM转换比"), Category("基本参数"), DisplayName("脉冲比例")]
        public double Scale { get; set; } = 1;
        [DescriptionAttribute("。"), Category("运动参数"), DisplayName("轴定义")]
        public ErosSocket.DebugPLC.EnumAxisType AxisType { get; set; }
        [DescriptionAttribute("。"), Category("运动参数"), DisplayName("当前位置")]
        public double Point { get { return point; } set { point = value; } }
        double point;
        [DescriptionAttribute("运行速度s/mm"), Category("当前速度"), DisplayName("移动速度")]
        public double MaxVel { get; set; } = 100;

        [DescriptionAttribute("起始速度s/mm"), Category("当前速度"), DisplayName("启动速度")]
        public double StrVal { get; set; } = 10;

        [DescriptionAttribute("起始到运行加速时间s"), Category("当前速度"), DisplayName("加速时间")]
        public double Tacc { get; set; } = 0.1;

        [DescriptionAttribute("运行到起始速度减速时间s"), Category("当前速度"), DisplayName("减速时间")]
        public double Tdec { get; set; } = 0.1;


        [DescriptionAttribute("。"), Category("运动参数"), DisplayName("是否已同步原点")]
        public bool IsHome { get; private set; } = false;

        [DescriptionAttribute("。"), Category("运动参数"), DisplayName("是否存在错误")]
        public bool IsError { get; set; } = false;
        [DescriptionAttribute("。"), Category("运动参数"), DisplayName("使能")]
        public bool IsEnabled { get; set; } = false;
        [DescriptionAttribute("速度百分比"), Category("运动参数"), DisplayName("速度百分比")]
        public byte Override_speed { get; set; } = 100;

        [DescriptionAttribute("=负数不使用刹车。"), Category("调试参数"), DisplayName("刹车系统")]
        public sbyte IsBand_type_brakeNumber { get; set; } = -1;
        [DescriptionAttribute("。"), Category("调试参数"), DisplayName("点动距离")]
        public double Jog_Distance { get; set; } = 10;

        [DescriptionAttribute("。"), Category("调试参数"), DisplayName("轴卡号")]
        public short AxisNo { get; set; }


        [DescriptionAttribute("。"), Category("调试参数"), DisplayName("从轴卡号")]
        public short AxisNoEx { get; set; } = -1;


        [DescriptionAttribute("。"), Category("状态"), DisplayName("是否运动中")]
        public bool IsMove { get; private set; }
        [DescriptionAttribute("1=mAcm_AxGetCmdPosition。2=mAcm_AxGetMachPosition,其他=mAcm_AxGetActualPosition"),
            Category("状态"), DisplayName("读取当前位置方法")]
        public int IsGetPoint { get; set; } = 0;

        [DescriptionAttribute("。"), Category("DD马达"), DisplayName("DD马达回零输出点")]
        public sbyte DDHomeIO { get; set; } = -1;

        [DescriptionAttribute("。"), Category("误差"), DisplayName("移动误差")]
        public double Deviatcion { get; set; } = 0.05;
        /// <summary>
        /// 比对目标与当前位置是否到位
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool PTPMOVE(double p)
        {
            double pu = Math.Abs((Point - p));
            return ((pu < Deviatcion && pu > -Deviatcion));
        }
        /// <summary>
        /// 状态编号
        /// </summary>
        public ushort StaratNn;

        public void AddSeelp(double Dacc, double strVal, double MaxVal, double Tacc)
        {
            this.MaxVel = MaxVal;
            this.StrVal = strVal;
            this.Tdec = Tacc;
            this.Tacc = Dacc;
            AddSeelp();
        }
        public void AddSeelp()
        {
            if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
            {
                UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.CFG_AxMaxVel, MaxVel * Scale);
                if (MaxVel < StrVal)
                {
                    StrVal = MaxVel;
                }
                UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxVelLow, StrVal * Scale);
                UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxVelHigh, MaxVel * Scale);
                UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxAcc, Tacc * Scale);
                UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxDec, Tdec * Scale);
                UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxJerk, 0);
            }
        }

        public void SetSeelp(double acc, double dec, double strV, double MaxV)
        {
            if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
            {
                UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.CFG_AxMaxVel, MaxV * 2 * Scale);
                UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxVelLow, strV * Scale);
                UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxVelHigh, MaxV * Scale);
                UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxAcc, acc * Scale);
                UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxDec, dec * Scale);
                UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxJerk, 0); //T 型曲线
            }
        }
        public void Dand_type_brake(bool isDeal)
        {

        }
        /// <summary>
        /// 正硬限位
        /// </summary>
        public bool Positive_Limit;
        /// <summary>
        /// 负硬限位
        /// </summary>
        public bool Negative_Limit;
        /// <summary>
        /// 正软限位
        /// </summary>
        public bool Positive_LimitSwt;
        /// <summary>
        /// 负软限
        /// </summary>
        public bool Negative_LimitSwt;
        /// <summary>
        /// 原点信号
        /// </summary>
        public bool Origin_Limit;
        public bool[] IOBools;
        /// <summary>
        /// 当前运行速度
        /// </summary>
        public double SleepValue { get; private set; }

        public void Enabled()
        {
       
            if (!IsEnabled)
            {
                UResult = Motion.mAcm_AxSetSvOn(m_Axishand, 1);
            }
            if (AxisNoEx > 0)
            {
                uint IOStatus = 0;
                Motion.mAcm_AxGetMotionIO(m_AxishandEx, ref IOStatus);
                bool[] IOBo = ErosSocket.ErosConLink.StaticCon.ConvertIntToBoolArray((int)IOStatus, 32);
                if (!IOBo[14])
                {
                    UResult = Motion.mAcm_AxSetSvOn(m_AxishandEx, 1);
                }
            }
        }
        bool isAddMove;
        public bool GetStatus()
        {
            short errorID = 0;
            double pointD = 0;
            double pos = 0;
            double seelp = 0;

            bool[] bools2 = null;
            try
            {
                if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
                {
                    try
                    {
                        uint IOStatus = 0;
                        uint Result = Motion.mAcm_AxGetMotionIO(m_Axishand, ref IOStatus);

                        IOBools = ErosSocket.ErosConLink.StaticCon.ConvertIntToBoolArray((int)IOStatus, 32);
                        Positive_Limit = IOBools[2];
                        Negative_Limit = IOBools[3];
                        Origin_Limit = IOBools[4];

                        ushort IOStatust = 0;
                        Result = Motion.mAcm_AxGetState(m_Axishand, ref IOStatust);
                        StaratNn = IOStatust;
                        double vaset = 0;
                        Result = Motion.mAcm_AxGetActVelocity(m_Axishand, ref vaset);
                        Result = Motion.mAcm_AxGetCmdVelocity(m_Axishand, ref vaset);
                        SleepValue = vaset;


                        if (IOStatust == 4)
                        {
                            homeing = true;
                        }
                        else if(IOStatust == 3)
                        {
                            //Motion.mAcm_AxResetError(m_Axishand);
                            //if (AxisNoEx>0)
                            //{
                            //    Motion.mAcm_AxResetError(m_AxishandEx);
                            //}
                        }
                        Positive_LimitSwt = IOBools[16];
                        Negative_LimitSwt = IOBools[17];
                        IsEnabled= IOBools[14];
   
                        if (IOBools[1] || (IOStatust == 3 && (Positive_LimitSwt || Negative_LimitSwt || Positive_Limit || Negative_Limit)) )
                        {
                            string errStr = "";
                            if (Negative_LimitSwt)
                            {
                                errStr += "软件负限位";
                            }
                            if (Positive_LimitSwt)
                            {
                                errStr += "软件正限位";
                            }
                            if (IOBools[1])
                            {
                                errStr += "伺服错误信号";
                            }
                            if (Positive_Limit)
                            {
                                errStr += "硬件正限位";
                            }
                            if (Negative_Limit)
                            {
                                errStr += "硬件负限位";
                            }
                            Vision2.ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new Vision2.ErosProjcetDLL.Project.AlarmText.alarmStruct() { Name = this.Name + "轴故障", Text = errStr });
                            IsError = true;
                        }
                        else
                        {
                            Vision2.ErosProjcetDLL.Project.AlarmListBoxt.RomveAlarm(this.Name + "轴故障");
                            IsError = false;
                        }
                        Result = Motion.mAcm_AxGetMotionStatus(m_Axishand, ref IOStatus);
                        bools2 = ErosSocket.ErosConLink.StaticCon.ConvertIntToBoolArray((int)IOStatus, 32);
                   
                  
                        IsMove = bools2[9];
                        isAddMove = bools2[8];
                        switch (IsGetPoint)
                        {
                            case 1:
                                Result = Motion.mAcm_AxGetCmdPosition(m_Axishand, ref this.point);
                                break;
                            case 2:
                                Result = Motion.mAcm_AxGetCompensatePosition(m_Axishand, ref this.point);
                                break;
                            default:
                                Result = Motion.mAcm_AxGetActualPosition(m_Axishand, ref this.point);
                                break;
                        }
                        this.point = Math.Round(this.point / this.Scale / Ratio, 2);
                    }
                    catch (Exception)
                    {
                    }
                    return true;
                }
                short ddonve = MP_C154.c154_motion_done(AxisNo);
                bools2 = ErosSocket.ErosConLink.StaticCon.ConvertIntToBoolArray(ddonve, 32);
                IsMove = bools2[2];
                ushort det = 0;
                C154_AxisGrub.ReturnCode = MP_C154.c154_get_io_status(AxisNo, ref det);
                bool[] bools = ErosSocket.ErosConLink.StaticCon.ConvertIntToBoolArray(det, 16);

                Positive_Limit = bools[2];
                Negative_Limit = bools[3];
                Origin_Limit = bools[4];
                C154_AxisGrub.ReturnCode = MP_C154.c154_get_error_counter(AxisNo, ref errorID);

                if (bools[1])
                {
                    IsError = true;
                }
                else
                {
                    IsError = false;
                }
                int cmd = 0;
                C154_AxisGrub.ReturnCode = MP_C154.c154_get_command(AxisNo, ref cmd);

                C154_AxisGrub.ReturnCode = MP_C154.c154_get_position(AxisNo, ref pointD);//
                if (AxisType == EnumAxisType.T)
                {

                }
                double detet = (pointD / Scale);
                point = (pointD / Scale);
                C154_AxisGrub.ReturnCode = MP_C154.c154_get_latch_data(AxisNo, 0, ref pos);//

                C154_AxisGrub.ReturnCode = MP_C154.c154_get_current_speed(AxisNo, ref seelp);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        public void SetAxisHand(IntPtr intPtr)
        {
            m_Axishand = intPtr;
        }
        public void SetAxisHandEx(IntPtr intPtr)
        {
            m_AxishandEx = intPtr;
        }
        IntPtr m_Axishand;

        IntPtr m_AxishandEx;
        /// <summary>
        /// 初始化轴
        /// </summary>
        /// <returns></returns>
        public bool Initial()
        {
            IsHome = false;
            homeing = false;
            if (Ratio == 0)
            {
                Ratio = 1;
            }
            if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
            {
                Motion.mAcm_AxSetCmdPosition(m_Axishand, 0);
                Motion.mAcm_AxSetActualPosition(m_Axishand, 0);
                if (DebugCompiler.GetThis().Is_Sv)
                {
                    GetStatus();
                    Enabled();
                }
                if (IsError) Motion.mAcm_AxResetError(m_Axishand);
                if (this.AxisType == EnumAxisType.S)
                {
                    IsHome = true;
                }
                AddSeelp();
                return true;
            }
            if (this.AxisType == EnumAxisType.S)
            {
                //MP_C154.c154_set_home_config(AxisNo, 1, 0, 0, 0, 0);  //回原点方式
                //MP_C154.c154_set_limit_logic(AxisNo, 1);   //伺服限位开关 
            }
            else if (this.AxisType == EnumAxisType.T)
            {
                MP_C154.c154_set_home_config(AxisNo, 1, 0, 0, 0, 0);  //回原点方式
                MP_C154.c154_set_limit_logic(AxisNo, 1);   //伺服限位开关 
                MP_C154.c154_set_pls_iptmode(AxisNo, 0, 1);
                MP_C154.c154_set_feedback_src(AxisNo, 01);
            }
            else
            {
                //MP_C154.c154_set_inp(AxisNo, 1, 1);
                //MP_C154.c154_set_home_config(AxisNo, 4, 0, 0, 0, 0);                
                //MP_C154.c154_set_pls_outmode(AxisNo, 4); //Output type is CW/CCW设定脉冲控制命令输出信号模式  设定脉冲控制命令输出模式，有 6 种模式可供设定。 
                //                                         //1X A/ B 1 2X A/ B 2 4X A/ B 3 CW / CCW
                //MP_C154.c154_set_pls_iptmode(AxisNo, 2, 0); //Input Type is 4*AB Phase设定反馈脉冲输入信号模式  设定反馈脉冲输入信号模式，有 4 种模式可供设定，请注意，唯 有 c154_set_feedback_src() 内的 Src 选择反馈信号，设定此函 数才有意义。 
                //                                            //0 External signal feedback 1 Command pulse
                //MP_C154.c154_set_feedback_src(AxisNo, 1); //Feedback source is Internal设定反馈计数器信号源 若外部可提供编码器反馈信号，则可设定此函数参数 Src 来启动 内部计数器依据 c154_set_pls_iptmode() 所设定之反馈信号计 数，否则计数器将依据脉冲控制命令输出计数。 
                //MP_C154.c154_set_alm(AxisNo, 1, 0); //Feedback source is Internal       
                //MP_C154.c154_set_limit_logic(AxisNo, 1);   //伺服限位开关         
                //MP_C154.c154_set_home_config(AxisNo, 1, 0, 0, 0, 0);  //回原点方式
                MP_C154.c154_set_pls_outmode(AxisNo, 4); //Output type is CW/CCW
                MP_C154.c154_set_pls_iptmode(AxisNo, 2, 0); //Input Type is 4*AB Phase
                MP_C154.c154_set_feedback_src(AxisNo, 1); //Feedback source is Internal
                MP_C154.c154_set_alm(AxisNo, 1, 0); //Feedback source is Internal
                if (true)
                {
                    MP_C154.c154_set_inp(AxisNo, 1, 1);
                    MP_C154.c154_set_limit_logic(AxisNo, 1);   //伺服限位开关    
                }
                //MP_C154.c154_set_home_config(AxisNo, 4, 0, 0, 0, 0);                
                MP_C154.c154_set_home_config(AxisNo, 1, 0, 0, 0, 0);
                //MP_C154.c154_set_pls_outmode(AxisNo, 4); //Output type is CW/CCW
                //MP_C154.c154_set_pls_iptmode(AxisNo, 2, 0); //Input Type is 4*AB Phase
                //MP_C154.c154_set_limit_logic(AxisNo, 1);   //伺服限位开关   
                //MP_C154.c154_set_feedback_src(AxisNo, 1); //Feedback source is Internal
                //MP_C154.c154_set_alm(AxisNo, 1, 0); //Feedback source is Internal
                //                                    //MP_C154.c154_set_inp(AxisNo, 1, 1);
                //                                    // MP_C154.c154_set_home_config(AxisNo, 4, 0, 0, 0, 0);
                //MP_C154.c154_set_home_config(AxisNo, 1, 0, 0, 0, 0);
                MP_C154.c154_set_servo(AxisNo, 1);//伺服上电      
            }


            MP_C154.c154_set_servo(AxisNo, 1);//伺服上电      

            if (IsBand_type_brakeNumber >= 0)
            {
                MP_C154.c154_set_gpio_output_ex_CH(0, IsBand_type_brakeNumber, 1);   //伺服刹车
            }
            return true;
        }
        /// <summary>
        /// 移动轴，正反模式，是否点的，移动量
        /// 
        /// </summary>
        /// <param name="JogPsion">正反</param>
        /// <param name="jogmode">点或寸</param>
        /// <param name="seepJog">移动量</param>
        public void JogAdd(bool JogPsion, bool jogmode = true, double seepJog = 1)
        {
            double val = MaxVel * Scale;
            if (JogPsion)
            {
                seepJog = -seepJog;
                val = -MaxVel * Scale;
            }
            if (AxisType == EnumAxisType.T)
            {
                if (DebugCompiler.GetThis().DDAxis.AlwaysIODot.Value || DebugCompiler.GetThis().DDAxis.AlwaysIOInt.Value || DebugCompiler.GetThis().DDAxis.AlwaysIOOut.Value)
                {
                    MessageBox.Show("设备中有产品,无法调整轨道宽度");
                    return;
                }
            }
            if (jogmode)
            {
                //SetPoint(Point + seepJog);
                if (this.PlusLimit < Point + seepJog)
                {
                    throw (new Exception("超出最大限位"));
                }
                if (this.MinusLimit > Point + seepJog)
                {
                    throw (new Exception("超出最小限位"));
                }
                if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
                {
                    UResult = Motion.mAcm_AxMoveAbs(m_Axishand, Math.Round((Point + seepJog) * Scale));
                }
                else
                {
                    C154_AxisGrub.ReturnCode = MP_C154.c154_start_ta_move(AxisNo, Math.Round((Point + seepJog) * Scale), StrVal * Scale, MaxVel * Scale, Tacc, Tdec);   //以梯形速度曲线控制绝对坐标的点位运动 
                }
            }
            else
            {
                if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
                {
                    if (seepJog > 0)
                    {
                        UResult = Motion.mAcm_AxMoveVel(m_Axishand, 0);
                    }
                    else
                    {
                        UResult = Motion.mAcm_AxMoveVel(m_Axishand, 1);
                    }
                }
                else
                {
                    C154_AxisGrub.ReturnCode = MP_C154.c154_tv_move(AxisNo, StrVal * Scale, val, Tacc);
                }
            }
        }

        public override void Reset()
        {
            try
            {
                ErosProjcetDLL.Project.AlarmListBoxt.RomveAlarm(this.Name + "轴故障");
                if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
                {
                    Enabled();
                    UResult = 0;
                    UResult = Motion.mAcm_AxResetError(m_Axishand);
                    if (AxisNoEx > 0)
                    {
                      UResult = Motion.mAcm_AxResetError(m_AxishandEx);
                    }
          
                    return;
                }
                else
                {
                    C154_AxisGrub.ReturnCode = MP_C154.c154_reset_error_counter(AxisNo);
                }
            }
            catch (Exception)
            {
            }
        }

        bool homeing = false;
        bool HomeStop;
        StringBuilder ErrorMsg = new StringBuilder("", 100);
        uint UResult
        {
            get
            {
                return uResul;
            }
            set
            {
                if (value != 0)
                {
                    Boolean res = Motion.mAcm_GetErrorMessage(value, ErrorMsg, 100);
                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine(this.Name + ":" + ErrorMsg.ToString());
                }
                uResul = value;
            }
        }
        uint uResul;

        void SetHomeEX()
        {
            try
            {
                IsHome = false;
                if (AxisType == EnumAxisType.S)
                {
                    AddSeelp();
                    Thread.Sleep(1000);
                     Motion.mAcm_AxSetCmdPosition(m_Axishand, 0);
                    Motion.mAcm_AxSetActualPosition(m_Axishand, 0);
                    IsHome = true;
                    return;
                }
                UResult = Motion.mAcm_AxResetError(m_Axishand);
                if (AxisNoEx>=0)
                {
                    UResult = Motion.mAcm_AxResetError(m_AxishandEx);
                }
                if (AxisType == EnumAxisType.Udd)
                {
                    DebugCompiler.GetThis().DDAxis.WritDO(DDHomeIO, true);
                }
                else
                {
                    UResult = Motion.mAcm_SetU32Property(m_Axishand, (uint)PropertyID.CFG_AxSwMelEnable, 0);
                    UResult = Motion.mAcm_SetU32Property(m_Axishand, (uint)PropertyID.CFG_AxSwPelEnable, 0);
                    UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxHomeVelLow, PAR_AxVelLow);
                    UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxHomeVelHigh, PAR_AxVelHigh);
                    UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxHomeAcc, PAR_AxAcc);
                    UResult = Motion.mAcm_SetF64Property(m_Axishand, (uint)PropertyID.PAR_AxHomeDec, PAR_AxDec);
                    Thread.Sleep(200);
                    UResult = Motion.mAcm_AxHome(m_Axishand, HomeTepy, 1);
                    if (UResult != 0)
                    {
                        ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(this.Name + "轴初始化故障", "初始化失败代码:" + UResult);
                        return;
                    }
                }
                Thread.Sleep(120);
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        Thread.Sleep(1000);
                        if (AxisType == EnumAxisType.Udd)
                        {
                            DebugCompiler.GetThis().DDAxis.WritDO(DDHomeIO, false);
                        }
                        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                        watch.Start();
                        bool Home = false;

                        while (!Home || point != 0)
                        {
                            ushort det = 0;
                            UResult = Motion.mAcm_AxGetState(m_Axishand, ref det);
                            uint detInt = 0;
                            UResult = Motion.mAcm_AxGetMotionStatus(m_Axishand, ref detInt);
                            if (HomeStop)
                            {
                                return;
                            }
                            if (AxisType == EnumAxisType.Udd)
                            {
                                UResult = Motion.mAcm_AxGetMotionIO(m_Axishand, ref detInt);
                                bool[] bools2 = ErosSocket.ErosConLink.StaticCon.ConvertIntToBoolArray((int)detInt, 32);
                                if (bools2[13])
                                {
                                    UResult = Motion.mAcm_SetU32Property(m_Axishand, (uint)PropertyID.CFG_AxSwMelEnable, 1);
                                    UResult = Motion.mAcm_SetU32Property(m_Axishand, (uint)PropertyID.CFG_AxSwPelEnable, 1);
                                    UResult = Motion.mAcm_SetI32Property(m_Axishand, (uint)PropertyID.CFG_AxSwPelValue, (int)(this.PlusLimit * Scale * this.Ratio));
                                    UResult = Motion.mAcm_SetI32Property(m_Axishand, (uint)PropertyID.CFG_AxSwMelValue, (int)(this.MinusLimit * Scale * this.Ratio));
                                    UResult = Motion.mAcm_SetU32Property(m_Axishand, (uint)PropertyID.CFG_AxSwPelReact, 0);
                                    UResult = Motion.mAcm_SetU32Property(m_Axishand, (uint)PropertyID.CFG_AxSwMelReact, 0);
                                    AddSeelp();
                                    Thread.Sleep(1000);
                                     Motion.mAcm_AxSetCmdPosition(m_Axishand, 0);
                                     Motion.mAcm_AxSetActualPosition(m_Axishand, 0);
                                    IsHome = true;
                                    Home = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (det == 1)
                                {
                                    Thread.Sleep(1000);
                                    //Motion.mAcm_AxSetCmdPosition(m_Axishand, 0);
                                    //Motion.mAcm_AxSetActualPosition(m_Axishand, 0);
                                    UResult = Motion.mAcm_SetU32Property(m_Axishand, (uint)PropertyID.CFG_AxSwMelEnable, 1);
                                    UResult = Motion.mAcm_SetU32Property(m_Axishand, (uint)PropertyID.CFG_AxSwPelEnable, 1);
                                    UResult = Motion.mAcm_SetI32Property(m_Axishand, (uint)PropertyID.CFG_AxSwPelValue, (int)(this.PlusLimit * Scale * this.Ratio));
                                    UResult = Motion.mAcm_SetI32Property(m_Axishand, (uint)PropertyID.CFG_AxSwMelValue, (int)(this.MinusLimit * Scale * this.Ratio));
                                    UResult = Motion.mAcm_SetU32Property(m_Axishand, (uint)PropertyID.CFG_AxSwPelReact, 0);
                                    UResult = Motion.mAcm_SetU32Property(m_Axishand, (uint)PropertyID.CFG_AxSwMelReact, 0);
                                    AddSeelp();
                                    IsHome = true;
                                    Home = true;
                                    break;
                                }
                            }

                            if (watch.ElapsedMilliseconds > HomeTime * 1000)
                            {
                                homeing = false;
                                return;
                            }
                            Thread.Sleep(100);
                        }
                        Thread.Sleep(1500);
                        homeing = false;
                        //C154_AxisGrub.ReturnCode = MP_C154.c154_enable_soft_limit(AxisNo, 1);
                        //C154_AxisGrub.ReturnCode = MP_C154.c154_set_soft_limit(AxisNo, (int)(PlusLimit * Scale), (int)(MinusLimit * Scale));
                    }
                    catch (Exception)
                    {
                    }

                });
                thread.IsBackground = true;
                thread.Start();

            }
            catch (Exception ex)
            {
            }
            homeing = false;
        }

        public void SetHome()
        {
            Reset();
            HomeStop = false;
            if (homeing)
            {
                homeing = false;
                return;
            }
            homeing = true;
            if (AxisType == EnumAxisType.T)
            {
                if (DebugCompiler.GetThis().DDAxis.AlwaysIODot.Value ||
                                   DebugCompiler.GetThis().DDAxis.AlwaysIOInt.Value ||
                                    DebugCompiler.GetThis().DDAxis.AlwaysIOOut.Value)
                {
                    ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new Vision2.ErosProjcetDLL.Project.AlarmText.alarmStruct() { Name = this.Name + "故障", Text = "线体存在托盘，无法回零！" });
                    return;
                }
            }
            if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
            {
                SetHomeEX();
                return;
            }

            if (AxisType == EnumAxisType.S)
            {
                C154_AxisGrub.ReturnCode = MP_C154.c154_disable_soft_limit(AxisNo);
                return;
            }
            C154_AxisGrub.ReturnCode = MP_C154.c154_disable_soft_limit(AxisNo);
            MP_C154.c154_set_home_config(AxisNo, 1, 0, 0, 0, 0);
            IsHome = false;
            Thread.Sleep(120);
            C154_AxisGrub.ReturnCode = MP_C154.c154_home_search(AxisNo, 20 * Scale, -15 * Scale, 0.5, 2);
            Thread thread = new Thread(() =>
            {
                try
                {
                    Thread.Sleep(2000);
                    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
                    bool Home = false;
                    double point = -1;
                    while (!Home || point != 0)
                    {
                        ushort det = 0;
                        C154_AxisGrub.ReturnCode = MP_C154.c154_get_io_status(AxisNo, ref det);
                        if (C154_AxisGrub.ReturnCode != 0)
                        {
                            homeing = false;
                            return;
                        }
                        bool[] bools = ErosSocket.ErosConLink.StaticCon.ConvertIntToBoolArray(det, 16);
                        Home = bools[4];
                        MP_C154.c154_get_position(AxisNo, ref point);

                        if (watch.ElapsedMilliseconds > HomeTime * 1000)
                        {
                            homeing = false;
                            short d = MP_C154.c154_emg_stop(AxisNo);
                            return;
                        }
                        if (!homeing)
                        {
                            return;
                        }
                        Thread.Sleep(100);
                    }
                    homeing = false;
                    IsHome = true;
                    Thread.Sleep(1500);

                    C154_AxisGrub.ReturnCode = MP_C154.c154_enable_soft_limit(AxisNo, 1);
                    C154_AxisGrub.ReturnCode = MP_C154.c154_set_soft_limit(AxisNo, (int)(PlusLimit * Scale), (int)(MinusLimit * Scale));
                }
                catch (Exception)
                {
                }
                homeing = false;
            });
            thread.IsBackground = true;
            thread.Start();

        }

        public bool SetPoint(double? p, double? sleep = null)
        {
            short d = 0;
            if (!IsHome)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine(Name + "轴移动错误,未初始化目标" + p.Value);
                return false;
            }
            if (p == null)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine(Name + "轴移动错误,目标Null");
                return false;
            }
            p = Math.Round(p.Value, 1);
            if (AxisType != EnumAxisType.S)
            {
                if (this.PlusLimit < p)
                {
                    throw (new Exception("超出最大限位"));
                }
                if (this.MinusLimit > p)
                {
                    throw (new Exception("超出最小限位"));
                }
            }
            if (AxisType == EnumAxisType.S)
            {
                if (p == 0)
                {

                    if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
                    {
                        if (sleep != null)
                        {
                            SetSeelp(Tacc, Tdec, StrVal, sleep.Value);
                            Thread.Sleep(10);
                        }
                        if (p.Value > 0)
                        {
                            UResult = Motion.mAcm_AxMoveVel(m_Axishand, 1);
                        }
                        else
                        {
                            UResult = Motion.mAcm_AxMoveVel(m_Axishand, 0);
                        }
                    }
                    else
                    {
                        d = MP_C154.c154_tv_move(AxisNo, StrVal * Scale, MaxVel * Scale, Tacc);
                        C154_AxisGrub.ReturnCode = d;
                    }
                }
                else
                {
                    if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
                    {
                        if (sleep != null)
                        {
                            SetSeelp(Tacc, Tdec, StrVal, sleep.Value);
                            Thread.Sleep(10);
                        }
                        UResult = Motion.mAcm_AxMoveRel(m_Axishand, Math.Round(p.Value * Scale));
                    }
                    else
                    {
                        d = MP_C154.c154_start_ta_move(AxisNo, (p.Value * Scale), StrVal * Scale, MaxVel * Scale, Tacc, Tdec);   //以梯形速度曲线控制绝对坐标的点位运动 
                        C154_AxisGrub.ReturnCode = d;
                    }
                }
                if (d == 0)
                {
                    return true;
                }
            }
            else if (AxisType == EnumAxisType.T)
            {
                if (DebugCompiler.GetThis().DDAxis.AlwaysIODot.Value ||    DebugCompiler.GetThis().DDAxis.AlwaysIOInt.Value  || DebugCompiler.GetThis().DDAxis.AlwaysIOOut.Value)
                {
                    MessageBox.Show("设备中有产品,无法调整轨道宽度");
                    return false;
                }
            }
            if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
            {
                UResult = Motion.mAcm_AxMoveAbs(m_Axishand, p.Value * Scale);
            }
            else
            {
                d = MP_C154.c154_start_ta_move(AxisNo, Math.Round(p.Value * Scale), StrVal * Scale, MaxVel * Scale, Tacc, Tdec);   //以梯形速度曲线控制绝对坐标的点位运动 
                C154_AxisGrub.ReturnCode = d;
            }
            if (d == 0)
            {
                return true;
            }
            throw (new Exception(C154_AxisGrub.ErrMasage(d)));

        }

        public bool SetWPoint(double? p)
        {
            this.SetPoint(p);
            if (AxisType == EnumAxisType.S)
            {
                return true;
            }
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Restart();
            while (!PTPMOVE(p.Value))
            {
                if (DebugCompiler.RunStop)
                {
                    return false;
                }
                if (watch.ElapsedMilliseconds / 1000 > 10)
                {
                    if (StaratNn != 5)
                    {
                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine(this.Name + ":轴超时未到达" + point, Color.Red);
                        //throw (new Exception(this.Name + ":轴超时未到达" + point));
                        return false;
                    }
                }
                Thread.Sleep(10);
            }
            return true;
        }
        public void SetMoveTe(bool Addt)
        {
            short d = 0;
            try
            {
                if (AxisType == EnumAxisType.S)
                {
                    if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
                    {
                        if (Addt)
                        {
                            UResult = Motion.mAcm_AxMoveVel(m_Axishand, 0);
                        }
                        else
                        {
                            UResult = Motion.mAcm_AxMoveVel(m_Axishand, 1);
                        }
                    }
                    else
                    {
                        if (Addt)
                        {
                            d = MP_C154.c154_tv_move(AxisNo, StrVal * Scale, MaxVel * Scale, Tacc);
                            C154_AxisGrub.ReturnCode = d;
                        }
                        else
                        {
                            d = MP_C154.c154_tv_move(AxisNo, -StrVal * Scale, MaxVel * Scale, Tacc);
                        }
                    }
                    //if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
                    //{
                    //    UResult = Motion.mAcm_AxMoveRel(m_Axishand, Math.Round(p.Value * Scale));
                    //}
                    //else
                    //{
                    //    d = MP_C154.c154_tv_move(AxisNo,- StrVal * Scale, MaxVel * Scale, Tacc);
                    //    //d = MP_C154.c154_start_ta_move(AxisNo, (p.Value * Scale), StrVal * Scale, MaxVel * Scale, Tacc, Tdec);   //以梯形速度曲线控制绝对坐标的点位运动 
                    //    //C154_AxisGrub.ReturnCode = d;
                    //}
                    if (d == 0)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Pause()
        {
            Stop();
        }
        public bool Stop()
        {
            HomeStop = true;
            if (DebugCompiler.GetThis().ListKat == "PCI-1245L")
            {
                uint UResult = Motion.mAcm_AxStopDec(m_Axishand);
            }
            else
            {
                short d = MP_C154.c154_emg_stop(AxisNo);
            }
            //C154_AxisGrub.ReturnCode = d;
            return true;
        }
    }
    public class RunButton
    {
        [Description("黄指示灯IO"), Category("指示灯"), DisplayName("黄灯")]
        public sbyte yellow { get; set; } = -1;
        [Description("绿指示灯IO"), Category("指示灯"), DisplayName("绿灯")]
        public sbyte green { get; set; } = -1;
        [Description("红指示灯IO"), Category("指示灯"), DisplayName("红灯")]
        public sbyte red { get; set; } = -1;
        [Description("蜂鸣器IO"), Category("指示灯"), DisplayName("蜂鸣器")]
        public sbyte Fmq { get; set; } = -1;
        [Description("负值无效"), Category("按钮"), DisplayName("运行IO")]
        public sbyte RunButten { get; set; } = -1;
        [Description("负值无效"), Category("按钮"), DisplayName("停止IO")]
        public sbyte StopButten { get; set; } = -1;
        [Description("负值无效"), Category("按钮"), DisplayName("复位IO")]
        public sbyte ResetButten { get; set; } = -1;

        [Description("负值无效"), Category("按钮"), DisplayName("初始化IO")]
        public sbyte InButten { get; set; } = -1;
        [Description("负值无效"), Category("按钮"), DisplayName("急停按钮I")]
        public sbyte StopTButten { get; set; } = -1;

        /// <summary>
        /// 运行按钮灯
        /// </summary>
        [Description("负值无效"), Category("按钮指示灯"), DisplayName("运行指示灯IO")]
        public sbyte RunButtenS { get; set; } = -1;
        /// <summary>
        /// 停止按钮灯
        /// </summary>
        [Description("负值无效"), Category("按钮指示灯"), DisplayName("停止指示灯IO")]
        public sbyte StopButtenS { get; set; } = -1;
        [Description("负值无效"), Category("按钮指示灯"), DisplayName("复位指示灯IO")]
        /// <summary>
        /// 复位按钮灯
        /// </summary>
        public sbyte ResetButtenS { get; set; } = -1;
        /// <summary>
        /// 安全门
        /// </summary>
        [Description("安全门锁Q"), Category("门锁"), DisplayName("安全门锁")]
        public sbyte ANmen { get; set; } = -1;
        /// <summary>
        /// 安全门检测
        /// </summary>
        [Description("安全门锁I"), Category("门锁"), DisplayName("门锁检测")]
        public sbyte ANmenTI { get; set; } = -1;
        /// <summary>
        /// 安全光栅
        /// </summary>
        [Description("安全光栅I"), Category("门锁"), DisplayName("安全光栅")]
        public sbyte ANmenI { get; set; } = -1;

        [Description("安全光栅I"), Category("门锁"), DisplayName("安全门1检测")]
        public sbyte adMent1 { get; set; } = -1;
    }

    public class DllInvoke
    {
        [DllImport("kernel32.dll")]
        private extern static IntPtr LoadLibrary(String path);
        [DllImport("kernel32.dll")]
        private extern static IntPtr GetProcAddress(IntPtr lib, String funcName);
        [DllImport("kernel32.dll")]
        private extern static bool FreeLibrary(IntPtr lib);

        private IntPtr hLib;
        public DllInvoke(String DLLPath)
        {
            hLib = LoadLibrary(DLLPath);
        }

        ~DllInvoke()
        {
            FreeLibrary(hLib);
        }

        //将要执行的函数转换为委托    
        public Delegate Invoke(String APIName, Type t)
        {
            IntPtr api = GetProcAddress(hLib, APIName);
            return (Delegate)Marshal.GetDelegateForFunctionPointer(api, t);
        }
    }
}
