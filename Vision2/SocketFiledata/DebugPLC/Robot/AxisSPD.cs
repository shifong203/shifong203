using ErosSocket.DebugPLC.PLC;
using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC.Robot
{
    /// <summary>
    /// 轴组合
    /// </summary>
    public class AxisSPD : Vision2.ErosProjcetDLL.Project.INodeNew, IAxisGrub
    {
        [Browsable(false)]
        public System.Diagnostics.Stopwatch Watch { get; set; } = new System.Diagnostics.Stopwatch();

        public const string PointPath = "RobotPointFile";

        [Editor(typeof(PLC.ListPLCIOUserControl1.Editor), typeof(UITypeEditor))]
        public List<string> ListPLCIO { get; set; }

        [Editor(typeof(PLC.ListCyinderControl.Editor), typeof(UITypeEditor))]
        public List<string> ListCylin { get; set; } = new List<string>();

        [DescriptionAttribute("。"), Category("执行机制"), DisplayName("回原点超时时间")]
        public int HomeTime { get; set; } = 20000;

        [DescriptionAttribute("。"), Category("执行机制"), DisplayName("运行超时时间")]
        public int SetTime { get; set; } = 10000;

        public double Seelp { get; set; }

        [DescriptionAttribute("。"), Category("轴组状态"), DisplayName("停止中")]
        public bool Stoping { get; set; }

        [DescriptionAttribute("程序文件。"), Category("执行"), DisplayName("程序文件名")]
        /// <summary>
        ///
        /// </summary>
        public string PRunName { get; set; }

        public UInt16 MemOutW
        {
            get;
            set;
        }

        public UInt16 MemInW1
        {
            get;
            set;
        }

        public UInt16 MemInW2
        {
            get;
            set;
        }

        public List<string> IoNames { get; set; }

        [DescriptionAttribute("。"), Category("轴组组态"), DisplayName("输入IO数量")]
        public int IOintCout { get; set; } = 16;

        [DescriptionAttribute("。"), Category("轴组组态"), DisplayName("输出IO数量")]
        public int IOOutCout { get; set; } = 16;

        [DescriptionAttribute("。"), Category("轴组状态"), DisplayName("执行中")]
        public bool Busy { get; set; }

        [DescriptionAttribute("。"), Category("轴组状态"), DisplayName("已同步原点")]
        /// <summary>
        ///
        /// </summary>
        public bool IsHome { get; set; }

        [DescriptionAttribute("。"), Category("轴组状态"), DisplayName("使能中")]
        /// <summary>
        ///
        /// </summary>
        public bool enabled { get; set; }

        [DescriptionAttribute("。"), Category("轴组状态"), DisplayName("暂停中")]
        /// <summary>
        /// 暂停中
        /// </summary>
        public bool Pauseing { get; set; }

        [DescriptionAttribute("。"), Category("轴组状态"), DisplayName("错误中")]
        /// <summary>
        ///
        /// </summary>
        public bool Aralming { get; set; }

        [DescriptionAttribute("。"), Category("执行机制"), DisplayName("是否使用顺序回原点")]
        public bool IsHomeSeelp { get; set; } = true;

        [DescriptionAttribute("。"), Category("轴组状态"), DisplayName("错误代码")]
        public int ErrCode { get; set; }

        [DescriptionAttribute("。"), Category("轴组状态"), DisplayName("错误信息")]
        public string ErrMeaessge { get; set; }

        [DescriptionAttribute("。"), Category("轴组状态"), DisplayName("工具编号")]
        public sbyte Tool { get; set; }

        public void Enabled()
        {
        }

        public void Pause(bool pauses = false)
        {
        }

        public void Dand_type_brake(bool isd)
        {
        }

        [DescriptionAttribute("。"), Category("轴组组态"), DisplayName("X轴名称")]
        public string AsixXName { get; set; }

        [DescriptionAttribute("。"), Category("轴组组态"), DisplayName("Y轴名称")]
        public string AsixYName { get; set; }

        [DescriptionAttribute("。"), Category("轴组组态"), DisplayName("Z轴名称")]
        public string AsixZName { get; set; }

        [DescriptionAttribute("。"), Category("轴组组态"), DisplayName("U轴名称")]
        public string AsixUName { get; set; }

        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        [DescriptionAttribute("。"), Category("状态显示"), DisplayName("手动模式地址")]
        public string ModeName { get; set; }

        [DescriptionAttribute("。"), Category("点位信息"), DisplayName("点位文件名")]
        public string PointFileName { get; set; }

        public override string Name { get; set; }

        public void SetPointX(double x)
        {
            this.AxisX.SetPiconin(x);
        }

        public void SetPointY(double y)
        {
            this.AxisY.SetPiconin(y);
        }

        public void SetPointZ(double z)
        {
            this.AxisZ.SetPiconin(z);
        }

        public void SetPointU(double? u)
        {
            if (u != null)
            {
                this.AxisU.SetPiconin(Convert.ToSingle(u));
            }
        }

        public void SetPointV(double? v)
        {
        }

        public void SetPointW(double? w)
        {
        }

        public void SetSeep(double seelp, double? addSeelp = null, double? accSeelp = null)
        {
        }

        private PLCAxis AxisX;
        private PLCAxis AxisY;
        private PLCAxis AxisZ;
        private PLCAxis AxisU;

        private bool stop = false;

        [ReadOnly(false), Category("轴组组态"), DisplayName("轴数量")]
        public SByte AxisNumber { get; set; } = 0;

        [DescriptionAttribute("。"), Category("轴组组态"), DisplayName("Z轴安全高度")]
        public double JoupZ { get; set; } = 0.0f;

        public bool JogMode { get; set; } = true;
        public string ErrString { get; set; } = "";
        public string Execnte_Code { get; set; }

        /// <summary>
        ///
        /// </summary>
        public void UpAxis()
        {
            try
            {
                if (AsixXName != "")
                {
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void UpAxis(PLCAxis x, PLCAxis y, PLCAxis z = null, PLCAxis u = null)
        {
            try
            {
                AxisX = x;
                AxisY = y;
                AxisNumber = 2;
                if (z != null)
                {
                    AxisZ = z;
                    AxisNumber++;
                }
                if (u != null)
                {
                    AxisU = u;
                    AxisNumber++;
                }
            }
            catch (Exception)
            {
            }
        }

        public AxisSPD(PLCAxis x, PLCAxis y, PLCAxis z, PLCAxis u) : this(x, y, z)
        {
            AxisU = u;
            AxisNumber = 4;
        }

        public AxisSPD(PLCAxis x, PLCAxis y, PLCAxis z) : this(x, y)
        {
            AxisZ = z;
            AxisNumber = 3;
        }

        public AxisSPD(PLCAxis x, PLCAxis y)
        {
            AxisX = x;
            AxisY = y;
            AxisNumber = 2;
        }

        public AxisSPD()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public void Stop()
        {
            try
            {
                stop = true;
                Stoping = true;
                int ERR = 0;
                if (AxisNumber == 2)
                {
                    if (AxisX != null)
                    {
                        if (AxisX.Stop())
                        {
                            ERR++;
                        }
                    }
                    if (AxisY != null)
                    {
                        if (AxisY.Stop())
                        {
                            ERR++;
                        }
                    }
                }
                if (AxisNumber == 3)
                {
                    if (AxisZ != null)
                    {
                        if (AxisZ.Stop())
                        {
                            ERR++;
                        }
                    }
                }
                if (AxisNumber == 4)
                {
                    if (AxisU != null)
                    {
                        if (AxisU.Stop())
                        {
                            ERR++;
                        }
                    }
                }
                if (ERR == AxisNumber)
                {
                    return;
                }
            }
            catch (Exception)
            {
            }
            return;
        }

        /// <summary>
        /// 复位
        /// </summary>
        /// <returns></returns>
        public new void Reset()
        {
            try
            {
                this.Busy = false;
                Task.Run(() =>
                {
                });
                if (AxisNumber >= 2)
                {
                    if (AxisX != null)
                    {
                        AxisX.SetReset();
                    }
                    if (AxisY != null)
                    {
                        AxisY.SetReset();
                    }
                }
                if (AxisNumber >= 3)
                {
                    if (AxisZ != null)
                    {
                        AxisZ.SetReset();
                    }
                }
                if (AxisNumber >= 4)
                {
                    if (AxisU != null)
                    {
                        AxisU.SetReset();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public PLCAxis GetY()
        {
            return AxisY;
        }

        public PLCAxis GetX()
        {
            return AxisX;
        }

        public PLCAxis GetZ()
        {
            return AxisZ;
        }

        public PLCAxis GetU()
        {
            return AxisU;
        }

        public IAxis GetAxis(sbyte id)
        {
            IAxis axis = null;
            switch (id)
            {
                case 0:
                    axis = AxisX;
                    break;

                case 1:
                    axis = AxisY;
                    break;

                case 2:
                    axis = AxisZ;
                    break;

                case 3:

                    if (AxisU != null && AxisU.Name == null)
                    {
                        AxisU = null;
                    }
                    if (AxisU == null)
                    {
                        if (AsixUName != null)
                        {
                            if (DebugPLC.DebugComp.GetThis().DicAxes.ContainsKey(AsixUName))
                            {
                                AxisU = DebugPLC.DebugComp.GetThis().DicAxes[AsixUName];
                            }
                        }
                    }
                    axis = AxisU;
                    break;

                default:
                    break;
            }
            return axis;
        }

        private PLCAxis GetAxisName(string name)
        {
            if (DebugComp.GetThis().DicAxes.ContainsKey(name))
            {
                return DebugComp.GetThis().DicAxes[name];
            }
            return null;
        }

        public IAxis GetAxis(EnumAxisType axisType)
        {
            IAxis axis = null;
            switch (axisType)
            {
                case EnumAxisType.X:
                    if (AxisX == null)
                    {
                        AxisX = GetAxisName(AsixXName);
                    }
                    axis = AxisX;
                    break;

                case EnumAxisType.Y:
                    if (AxisY == null)
                    {
                        AxisY = GetAxisName(AsixYName);
                    }
                    axis = AxisY;
                    break;

                case EnumAxisType.Z:
                    if (AxisZ == null)
                    {
                        if (AsixZName == null)
                        {
                            return null;
                        }
                        AxisZ = GetAxisName(AsixZName);
                    }
                    axis = AxisZ;
                    break;

                case EnumAxisType.U:
                    if (AxisU == null)
                    {
                        AxisU = GetAxisName(AsixUName);
                    }
                    if (AxisU != null && AxisU.Name == null)
                    {
                        AxisU = null;
                    }
                    if (AxisU == null)
                    {
                        if (AsixUName != null)
                        {
                            if (DebugPLC.DebugComp.GetThis().DicAxes.ContainsKey(AsixUName))
                            {
                                AxisU = DebugPLC.DebugComp.GetThis().DicAxes[AsixUName];
                            }
                        }
                    }
                    axis = AxisU;
                    break;

                default:
                    break;
            }
            return axis;
        }

        /// <summary>
        /// 返回状态
        /// </summary>
        public void GetSt(out bool ishome, out bool enabled, out bool err, out string mess)
        {
            ishome = this.IsHome;
            enabled = this.enabled;
            err = Aralming;
            mess = this.ErrMeaessge;
        }

        public void GetSt()
        {
            this.GetX().GetSt();
            this.GetY().GetSt();
            if (AxisNumber >= 3)
            {
                if (this.GetZ() != null)
                {
                    this.GetZ().GetSt();
                    if (this.GetX().IsHome || this.GetY().IsHome || this.GetZ().IsHome)
                    {
                        this.IsHome = true;
                    }
                    else
                    {
                        this.IsHome = false;
                    }
                    if (this.GetX().Alarm || this.GetY().Alarm || this.GetZ().Alarm)
                    {
                        this.Aralming = true;
                    }
                    else
                    {
                        this.Aralming = false;
                    }
                }
            }
            else if (this.AxisNumber >= 4)
            {
                this.GetU().GetSt();
                if (this.GetX().IsHome || this.GetY().IsHome || this.GetZ().IsHome || this.GetU().IsHome)
                {
                    this.IsHome = true;
                }
                else
                {
                    this.IsHome = false;
                }
                if (this.GetX().Alarm || this.GetY().Alarm || this.GetZ().Alarm || this.GetU().Alarm)
                {
                    this.Aralming = true;
                }
                else
                {
                    this.Aralming = false;
                }
            }
            else
            {
                if (this.GetX().IsHome || this.GetY().IsHome)
                {
                    this.IsHome = true;
                }
                else
                {
                    this.IsHome = false;
                }
                if (this.GetX().Alarm || this.GetY().Alarm)
                {
                    this.Aralming = true;
                }
                else
                {
                    this.Aralming = false;
                }
            }
        }

        /// <summary>
        /// 返回轴位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        public void GetPoints(out double x, out double y, out double z, out double u)
        {
            z = u = y = x = 9999.999f;
            try
            {
                if (AxisX != null)
                {
                    x = AxisX.GetPoint();
                }
                else if (AsixXName != null)
                {
                    if (DebugPLC.DebugComp.GetThis().DicAxes.ContainsKey(AsixXName))
                    {
                        x = (Single)DebugPLC.DebugComp.GetThis().DicAxes[AsixXName].Point;
                        AxisX = DebugPLC.DebugComp.GetThis().DicAxes[AsixXName];
                    }
                }
                if (AxisY != null)
                {
                    y = AxisY.GetPoint();
                }
                else if (AsixYName != null)
                {
                    if (DebugPLC.DebugComp.GetThis().DicAxes.ContainsKey(AsixYName))
                    {
                        y = (Single)DebugPLC.DebugComp.GetThis().DicAxes[AsixYName].Point;
                        AxisY = DebugPLC.DebugComp.GetThis().DicAxes[AsixYName];
                    }
                }
                if (AxisZ != null)
                {
                    z = AxisZ.GetPoint();
                }
                else if (AsixZName != null)
                {
                    if (DebugPLC.DebugComp.GetThis().DicAxes.ContainsKey(AsixZName))
                    {
                        z = (Single)DebugPLC.DebugComp.GetThis().DicAxes[AsixZName].Point;
                        AxisZ = DebugPLC.DebugComp.GetThis().DicAxes[AsixZName];
                    }
                }
                if (AxisU != null)
                {
                    u = AxisU.GetPoint();
                }
                else if (AsixUName != null)
                {
                    if (DebugPLC.DebugComp.GetThis().DicAxes.ContainsKey(AsixUName))
                    {
                        u = (Single)DebugPLC.DebugComp.GetThis().DicAxes[AsixUName].Point;
                        AxisU = DebugPLC.DebugComp.GetThis().DicAxes[AsixUName];
                    }
                }
            }
            catch (Exception)
            {
            }
            return;
        }

        public void GetPoints(out double x, out double y, out double z)
        {
            z = y = x = 9999.999f;
            try
            {
                if (AxisX != null)
                {
                    x = AxisX.GetPoint();
                }
                if (AxisY != null)
                {
                    y = AxisY.GetPoint();
                }
                if (AxisZ != null)
                {
                    z = AxisZ.GetPoint();
                }
            }
            catch (Exception)
            {
            }
            return;
        }

        public void GetPoints(out double x, out double y, out double z, out double u, out double v, out double w)
        {
            z = u = y = x = w = v = 9999.999f;
            try
            {
                GetPoints(out x, out y, out z, out u);
            }
            catch (Exception)
            {
            }
            return;
        }

        private Thread thread;

        /// <summary>
        /// 移动指定位置
        /// </summary>
        /// <param name="runCode">轨迹代码</param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <param name="U"></param>
        public void SetPoint(string runCode, double x, double y, double? z, double? u)
        {
            if (AxisU != null)
            {
                if (runCode == "PTP")
                {
                    this.SetPointZ(JoupZ);
                    if (z != null)
                    {
                        while (this.GetZ().GetPoint() == JoupZ)
                        {
                            if (Stoping)
                            {
                                return;
                            }
                        }
                    }
                    this.SetPointU(u);
                }
                else
                {
                    this.SetPointU(u);
                }
                this.SetPoint(runCode, x, y, z);
            }
            else
            {
                this.SetPoint(runCode, x, y, z);
            }
        }

        /// <summary>
        /// 移动到点位
        /// </summary>
        public void SetPoint(string runCode, string idName)
        {
            SetPoint(runCode, this.PointFileName, idName);
        }

        public void SetPoint(string runCode, string fileName, string pointName)
        {
            PointFile pointFile = PointFile.GetPointName(fileName, pointName);
            if (pointFile.Name == pointName)
            {
                SetPoint(runCode, (Single)pointFile.X, (Single)pointFile.Y, pointFile.Z, pointFile.U);
            }
            else
            {
                if (Vision2.ErosProjcetDLL.Project.ProjectINI.DebugMode)
                {
                    MessageBox.Show("点不存在" + pointName);
                }
                else
                {
                    Vision2.ErosProjcetDLL.Project.AlarmText.LogErr("移动点不存在" + pointName, Name);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="runCode"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        public void SetPoint(string runCode, double X, double Y, double? Z)
        {
            switch (runCode)
            {
                case "PTP":
                    try
                    {
                        lock (this)
                        {
                            thread = new Thread(() =>
                            {
                                if (Z != null)
                                {
                                    AxisZ.SetPiconin(JoupZ);
                                    while (AxisZ.GetPoint() != JoupZ)
                                    {
                                        if (stop)
                                        {
                                            return;
                                        }
                                    }
                                }
                                SetPoint(X, Y);
                                while (AxisX.GetPoint() != X && AxisY.GetPoint() != Y)
                                {
                                    if (stop)
                                    {
                                        return;
                                    }
                                }
                                if (Z != null)
                                {
                                    AxisZ.SetPiconin(Convert.ToSingle(Z));
                                }
                            });
                            thread.IsBackground = true;
                            thread.Start();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Vision2.ErosProjcetDLL.Project.ProjectINI.DebugMode)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    break;

                default:
                    SetPoint(X, Y);
                    if (Z != null)
                    {
                        AxisZ.SetPiconin(Convert.ToSingle(Z));
                    }
                    break;
            }
        }

        /// <summary>
        /// 根据代码移动位置
        /// </summary>
        /// <param name="runCode"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        public bool SetPoint(string runCode, double? x, double? y, double? z, double? u)
        {
            try
            {
                if (this.Busy)
                {
                    ErrCode = 1245;
                    ErrMeaessge = "任务执行中";
                    return false;
                }
                watch.Restart();
                this.Busy = true;
                if (runCode == "Move")
                {
                    double? sy, sz, su;
                    double? sx = sy = sz = su = null;
                    if (x != null)
                    {
                        sx = this.AxisX.GetPoint() + (double)x;
                        this.AxisX.SetPiconin((double)sx);
                    }
                    if (y != null)
                    {
                        sy = this.AxisY.GetPoint() + (double)y;
                        this.AxisY.SetPiconin((double)sy);
                    }
                    if (z != null)
                    {
                        sz = this.AxisZ.GetPoint() + (double)z;
                        this.AxisZ.SetPiconin((double)sz);
                    }
                    if (u != null)
                    {
                        su = this.AxisU.GetPoint() + (double)u;
                        this.AxisU.SetPiconin((double)su);
                    }
                    while (x != null && sx != this.AxisX.GetPoint()
                  || y != null && sy != this.AxisY.GetPoint()
                  || z != null && sz != this.AxisZ.GetPoint() ||
                  u != null && su != this.AxisU.GetPoint()
                  )
                    {
                        Thread.Sleep(100);
                        if (watch.ElapsedMilliseconds > this.SetTime)
                        {
                            watch.Stop();
                            ErrMeaessge = "执行超时";
                            this.Busy = false;
                            return false;
                        }
                    }
                }
                else if (runCode == "go")
                {
                    if (x != null)
                    {
                        this.AxisX.SetPiconin((Single)x);
                    }
                    if (y != null)
                    {
                        this.AxisY.SetPiconin((Single)y);
                    }
                    if (z != null)
                    {
                        this.AxisZ.SetPiconin((Single)z);
                    }
                    if (u != null)
                    {
                        this.AxisU.SetPiconin((Single)u);
                    }

                    while (x != null && x != this.AxisX.GetPoint()
                        || y != null && y != this.AxisY.GetPoint()
                        || z != null && z != this.AxisZ.GetPoint() ||
                        u != null && u != this.AxisU.GetPoint()
                        )
                    {
                        Thread.Sleep(10);
                        if (watch.ElapsedMilliseconds > this.SetTime)
                        {
                            watch.Stop();
                            ErrMeaessge = "执行超时";
                            this.Busy = false;
                            return false;
                        }
                    }
                }
                this.Busy = false;
                return true;
            }
            catch (Exception)
            {
            }
            this.Busy = false;
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public void SetPoint(double X, double Y)
        {
            try
            {
                lock (this)
                {
                    thread = new Thread(() => { SetP(X, Y); });
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetP(double X, double Y)
        {
            AxisX.SetPiconin(X);
            AxisY.SetPiconin(Y);
        }

        private void SetP(string runCode, double X, double Y, double? Z, double? U)
        {
            SetP(X, Y);
        }

        private System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// 初始化原点
        /// </summary>
        public void SetHome()
        {
            ErrString = "初始化中";
            this.Busy = true;
            Task.Run(() =>
            {
                try
                {
                    watch.Restart();
                    if (AxisZ != null)
                    {
                        AxisZ.SetHome();
                        if (IsHomeSeelp)
                        {
                            while (!AxisZ.IsHome && AxisZ.Alarm)
                            {
                                if (AxisZ.Alarm)
                                {
                                    if (AxisZ.ErrCode == -20)
                                    {
                                        ErrString = "Z轴初始化超时";
                                    }
                                    ErrString = "Z轴初始化失败";
                                    Busy = false;
                                    return;
                                }
                            }
                        }
                    }
                    if (AxisX != null)
                    {
                        AxisX.SetHome();
                    }
                    if (AxisY != null)
                    {
                        AxisY.SetHome();
                    }
                    if (AxisNumber >= 4)
                    {
                        if (AxisU != null)
                        {
                            AxisU.SetHome();
                        }
                        while (!AxisZ.IsHome || !AxisX.IsHome || !AxisY.IsHome || !AxisU.IsHome)
                        {
                            if (watch.ElapsedMilliseconds >= HomeTime)
                            {
                                Busy = false;
                                ErrString = "初始化超时";
                                return;
                            }
                        }
                    }
                    else
                    {
                        while (!AxisZ.IsHome || !AxisX.IsHome || !AxisY.IsHome)
                        {
                            if (watch.ElapsedMilliseconds >= HomeTime)
                            {
                                ErrString = "初始化超时";
                                Busy = false;
                                return;
                            }
                        }
                    }
                    if (Busy)
                    {
                    }
                    ErrString = "初始化完成";
                }
                catch (Exception)
                {
                }
                Busy = false;
            }
            );
        }

        /// <summary>
        /// 试教当前位置到点文件
        /// </summary>
        /// <param name="pFileName">点文件名</param>
        /// <param name="pName">点编号</param>
        public void SetPointPFile(string pFileName, string pName)
        {
            if (PointFile.GetPointFile().ContainsKey(pFileName))
            {
                if (!PointFile.IsPointContainsKey(pFileName, pName))
                {
                    PointFile.GetPointFile()[pFileName].Add(new PointFile()
                    {
                        P = (short)PointFile.GetPointFile()[pFileName].Count,
                        Name = pName,
                        X = this.AxisX.GetPoint(),
                        Y = this.AxisY.GetPoint(),
                        Local = Tool,
                    });

                    if (this.AxisZ != null)
                    {
                        PointFile.GetPointFile()[pFileName][PointFile.GetPointFile()[pFileName].Count - 1].Z = this.AxisZ.GetPoint();
                    }
                    if (AxisU != null)
                    {
                        PointFile.GetPointFile()[pFileName][PointFile.GetPointFile()[pFileName].Count - 1].U = this.AxisU.GetPoint();
                    }
                }
                else
                {
                    for (int i = 0; i < PointFile.GetPointFile()[pFileName].Count; i++)
                    {
                        if (PointFile.GetPointFile()[pFileName][i].Name == pName)
                        {
                            PointFile.GetPointFile()[pFileName][i].X = this.AxisX.GetPoint();
                            PointFile.GetPointFile()[pFileName][i].Y = this.AxisY.GetPoint();
                            if (this.AxisZ != null)
                            {
                                PointFile.GetPointFile()[pFileName][i].Z = this.AxisZ.GetPoint();
                            }
                            if (AxisU != null)
                            {
                                PointFile.GetPointFile()[pFileName][i].U = this.AxisU.GetPoint();
                            }
                        }
                    }
                }
            }
        }

        public void SetPoint(string runCode, double x, double y, double? z, double? u, double? v, double? w)
        {
            this.SetPoint(runCode, x, y, Convert.ToSingle(z), Convert.ToSingle(u));
        }

        /// <summary>
        /// 下载指定名的点集合
        /// </summary>
        /// <returns></returns>
        public List<PointFile> GetFilePoints(string fileName = null)
        {
            return new List<PointFile>();
        }

        /// <summary>
        /// 上穿指定的点文件并保存
        /// </summary>
        /// <param name="fileName">点文件名</param>
        /// <returns>成功返回true</returns>
        public bool LoandPointFile(string fileName)
        {
            return false;
        }

        /// <summary>
        /// 获取运行轨迹
        /// </summary>
        /// <returns></returns>
        public List<string> GetRunCode()
        {
            return new List<string>() { "GO", "PTP" };
        }

        /// <summary>
        /// 设置IO
        /// </summary>
        /// <param name="id">IOid号</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetIOOut(string id, bool value)
        {
            ErosConLink.StaticCon.SetLinkAddressValue(id, value);
            return false;
        }

        /// <summary>
        /// 设置IO
        /// </summary>
        /// <param name="id">IOid号</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetIOOut(string id, string value)
        {
            return ErosConLink.StaticCon.SetLinkAddressValue(DebugComp.GetThis().DicPLCIO[id],
                   value);
            //ErosConLink.StaticCon.SetLinkAddressValue(id, value);
            return false;
        }

        /// <summary>
        /// 获取16位IO的值，
        /// </summary>
        /// <param name="isOut">ture为输出，默认false输入</param>
        /// <returns></returns>
        public bool[] GetIOOuts(bool isOut = false)
        {
            if (isOut)
            {
                if (IOOutCout > 16)
                {
                    IOOutCout = 16;
                }
                return StaticCon.ConvertIntToBoolArray(MemOutW, IOOutCout);
            }
            else
            {
                List<bool> lisbools = new List<bool>();
                if (IOintCout / 16 == 1)
                {
                    lisbools.AddRange(StaticCon.ConvertIntToBoolArray(MemInW1, IOintCout));
                }
                if (IOintCout / 16 == 2)
                {
                    lisbools.AddRange(StaticCon.ConvertIntToBoolArray(MemInW1, 16));
                }
                else
                {
                }

                return lisbools.ToArray();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool GetMode()
        {
            dynamic SD = false;
            //ModeName = "上料站10.手动模式";

            if (this.ModeName != null && this.ModeName != "")
            {
                SD = ErosConLink.StaticCon.GetLingkNameValue(this.ModeName);
                try
                {
                    if (SD == true)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        private DebugRobot debugRobot;

        public DebugRobot DebugFormShow()
        {
            if (debugRobot == null || debugRobot.IsDisposed)
            {
                debugRobot = new DebugRobot();
                debugRobot.SetAxis(this);
            }
            Vision2.ErosProjcetDLL.UI.UICon.WindosFormerShow(ref debugRobot);

            return debugRobot;
        }
    }
}