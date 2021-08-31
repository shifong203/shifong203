using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Vision2.ErosProjcetDLL.Excel;
using Vision2.Project;
using Vision2.Project.DebugF.IO;
using Vision2.Project.formula;
using Vision2.Project.Mes;
using Vision2.Project.ProcessControl;
using static ErosSocket.DebugPLC.Robot.TrayRobot;

namespace ErosSocket.DebugPLC.Robot
{
    /// <summary>
    ///
    /// </summary>

    public interface ITrayRobot
    {
        void SetValue(int number, bool value, double? valueDouble = null);

        /// <summary>
        /// 更新指定位置状态
        /// </summary>
        /// <param name="number">编号</param>
        /// <param name="errNuber">错误状态，-1错误,0 清空，1OK，2NG红色，3NG黄色，4NG蓝色，5NG粉色 </param>
        void SetValue(int number, int errNuber);

        void SetValue(int number, OneDataVale dataVale);

        void SetValue(int number, TrayData dataVale);

        void SetValue(double value);

        void SetValue(List<bool> listValue);

        void SetValue(bool listValue);

        void SetPanleSN(List<string> listSN, List<int> tryaid);

        void RestValue(TrayData trayData);

        ///// <summary>
        ///// 关联数据
        ///// </summary>
        ///// <param name="tray"></param>
        //void SetTray(TrayData tray);
        void Initialize(TrayData trayData = null);

        void UpData();
    }

    /// <summary>
    ///
    /// </summary>
    public class TrayRobot
    {
        public enum TrayDirectionEnum
        {
            左上 = 0,
            右上 = 1,
            左下 = 2,
            右下 = 3,
        }

        public TrayRobot()
        {
        }

        public TrayRobot(sbyte x, sbyte y)
        {
            XNumber = x;
            YNumber = y;
        }

        [DescriptionAttribute("起点位置四个角落，并根据横向或竖向共8种排列方式。"), Category("排列"), DisplayName("起点位置")]
        public TrayDirectionEnum TrayDirection { get; set; }

        [DescriptionAttribute("横向Fales或竖向Ture。"), Category("排列"), DisplayName("横向或竖向")]
        public bool HorizontallyORvertically { get; set; }

        /// <summary>
        /// 托盘名称
        /// </summary>
        public string Name { get; set; }

        [DescriptionAttribute("X2方向数量。"), Category("排列"), DisplayName("X2方向数量")]
        public sbyte X2Number { get; set; }

        [DescriptionAttribute("Y2方向数量。"), Category("排列"), DisplayName("Y2方向数量")]
        public sbyte Y2Number { get; set; }

        [DescriptionAttribute("X方向数量。"), Category("排列"), DisplayName("X方向数量")]
        public sbyte XNumber { get; set; }

        [DescriptionAttribute("Y方向数量。"), Category("排列"), DisplayName("Y方向数量")]
        public sbyte YNumber { get; set; }

        [DescriptionAttribute("结果状态。"), Category("数据"), DisplayName("托盘产品数量")]
        public List<sbyte> bitW { get; set; } = new List<sbyte>();

        [DescriptionAttribute("使用8点位。"), Category("位置"), DisplayName("使用8点位")]
        public bool Is8Point { get; set; }

        [DescriptionAttribute("托盘位置。"), Category("数据"), DisplayName("托盘位置")]
        public int Number
        {
            get
            {
                if (TrayDataS == null)
                {
                    return 0;
                }
                return TrayDataS.Number;
            }

            set
            {
                if (TrayDataS == null)
                {
                    return;
                }
                TrayDataS.Number = value;
            }
        }

        [DescriptionAttribute("判断穴位SN是否空 。"), Category("判断"), DisplayName("判断穴位SN")]
        public bool IsSN { get; set; }

        [DescriptionAttribute("判断托盘SN是否空。"), Category("判断"), DisplayName("判断托盘SN")]
        public bool IsTraySN { get; set; }

        private ITrayRobot trayRobots;

        /// <summary>
        ///
        /// </summary>
        /// <param name="trayRobot"></param>
        public void AddTary(ITrayRobot trayRobot)
        {
            trayRobots = trayRobot;
            if (TrayDataS != null)
            {
                TrayDataS.SetITrayRobot(trayRobots);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ITrayRobot GetITrayRobot()
        {
            return trayRobots;
        }

        /// <summary>
        /// 读取托盘文件
        /// </summary>
        public void UpS()
        {
            try
            {
                if (System.IO.File.Exists(Vision2.ErosProjcetDLL.Project.ProjectINI.TempPath + Name + "Tray.txt"))
                {
                    string data = System.IO.File.ReadAllText(Vision2.ErosProjcetDLL.Project.ProjectINI.TempPath + Name + "Tray.txt");
                    string[] dataStrS = data.Split(',');
                    //number = int.Parse(dataStrS[0]);
                    for (int i = 1; i < dataStrS.Length; i++)
                    {
                        bitW[i] = sbyte.Parse(dataStrS[i]);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public virtual void Clear()
        {
            TrayDataS = new TrayData(this);
        }

        [DescriptionAttribute("穴位数量。"), Category("排列"), DisplayName("总数量")]
        public int Count
        {
            get
            {
                return XNumber * YNumber;
            }
        }

        public PointFile P1 { get; set; }

        public PointFile P2 { get; set; }

        public PointFile P3 { get; set; }

        public PointFile P4 { get; set; }

        public PointFile P5 { get; set; }

        public PointFile P6 { get; set; }

        public PointFile P7 { get; set; }

        public PointFile P8 { get; set; }

        public HTuple ListX { get; set; }
        public HTuple ListY { get; set; }

        public TrayData GetTrayData(TrayData data = null)
        {
            if (data != null)
            {
                TrayDataS = data;
                TrayDataS.TrayDirection = this.TrayDirection;
                TrayDataS.XNumber = XNumber;
                TrayDataS.YNumber = YNumber;
                TrayDataS.HorizontallyORvertically = HorizontallyORvertically;
                TrayDataS.SetITrayRobot(trayRobots);
            }
            if (TrayDataS == null)
            {
                TrayDataS = new TrayData(this);
            }
            return TrayDataS;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public TrayData QuntData()
        {
            TrayData tray = TrayDataS;

            TrayDataS = new TrayData(this);
            this.GetITrayRobot().Initialize(TrayDataS);
            //TrayDataS.SetITrayRobot(this.GetITrayRobot());
            //TrayDataS.RestValue();
            return tray;
        }

        /// <summary>
        /// 参数
        /// </summary>
        private TrayData TrayDataS;

        private PointFile[] pointFiles;

        public PointFile GetPoint(int number)
        {
            try
            {
                if (pointFiles == null)
                {
                    Calculate(out HTuple listx, out HTuple listY);
                    ListX = listx;
                    ListY = listY;
                    bitW = new List<sbyte>();
                    bitW.AddRange(new sbyte[pointFiles.Length]);
                }
                if (pointFiles.Length > number - 1)
                {
                    pointFiles[number - 1].Z = P1.Z;
                    return pointFiles[number - 1];
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        public List<PointFile> GetPoints()
        {
            List<PointFile> pointFi = new List<PointFile>();
            for (int i = 0; i < Count; i++)
            {
                pointFi.Add(new PointFile() { X = ListX[i], Y = ListY[i], Z = P1.Z });
            }

            return pointFi;
        }

        /// <summary>
        /// 计算矩阵，根据4个点计算矩阵
        /// </summary>
        /// <param name="xNumber"></param>
        /// <param name="yNumber"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="pointFile"></param>
        public static void Calculate(int xNumber, int yNumber, PointFile p1, PointFile p2, PointFile p3, PointFile p4, out PointFile[] pointFile)
        {
            pointFile = new PointFile[xNumber * yNumber];
            try
            {
                HTuple listx = new HTuple();
                HTuple listy = new HTuple();

                LineC(p1.X, p1.Y, p4.X, p4.Y, yNumber, out double[] xs, out double[] ys);
                LineC(p2.X, p2.Y, p3.X, p3.Y, yNumber, out double[] xs1, out double[] ys1);
                for (int i = 0; i < xs.Length; i++)
                {
                    LineC(xs[i], ys[i], xs1[i], ys1[i], xNumber, out double[] xs2, out double[] ys2);
                    listx.Append(xs2);
                    listy.Append(ys2);
                }

                for (int i = 0; i < listx.Length; i++)
                {
                    listx[i] = Math.Round(listx.TupleSelect(i).D, 1);
                    listy[i] = Math.Round(listy.TupleSelect(i).D, 1);
                    pointFile[i] = new PointFile();
                    pointFile[i].X = double.Parse(listx.TupleSelect(i).ToString());
                    pointFile[i].Y = double.Parse(listy.TupleSelect(i).ToString());
                }
                HOperatorSet.GenCrossContourXld(out HObject hObject, listx, listy, 10, 0);
                //ListX = listx;
                //ListY = listy;
            }
            catch (Exception ex)
            {
            }
        }

        public void Calculate(sbyte xNumber, sbyte yNumber, PointFile p1, PointFile p2, PointFile p3, PointFile p4, sbyte xNumber2, sbyte yNumber2,
            PointFile p5, PointFile p6, PointFile p7, PointFile p8, out PointFile[] pointFile)
        {
            pointFile = new PointFile[(xNumber * yNumber) + (xNumber2 * yNumber2)];
            try
            {
                //Is8Point = true;
                HTuple listx = new HTuple();
                HTuple listy = new HTuple();
                LineC(p1.X, p1.Y, p4.X, p4.Y, yNumber, out double[] xs, out double[] ys);
                LineC(p2.X, p2.Y, p3.X, p3.Y, yNumber, out double[] xs1, out double[] ys1);
                double[] ys2 = null;
                double[] xs2 = null;
                double[] xs12 = null;
                double[] ys12 = null;
                if (Is8Point)
                {
                    LineC(p5.X, p5.Y, p8.X, p8.Y, yNumber2, out xs2, out ys2);
                    LineC(p6.X, p6.Y, p7.X, p7.Y, yNumber2, out xs12, out ys12);
                }

                for (int i = 0; i < xs.Length; i++)
                {
                    LineC(xs[i], ys[i], xs1[i], ys1[i], xNumber, out double[] x1, out double[] y1);
                    listx.Append(x1);
                    listy.Append(y1);
                    if (ys12 != null)
                    {
                        if (i < ys12.Length)
                        {
                            LineC(xs2[i], ys2[i], xs12[i], ys12[i], xNumber2, out double[] x2, out double[] y2);
                            listx.Append(x2);
                            listy.Append(y2);
                        }
                    }
                }
                for (int i = 0; i < listx.Length; i++)
                {
                    listx[i] = Math.Round(listx.TupleSelect(i).D, 1);
                    listy[i] = Math.Round(listy.TupleSelect(i).D, 1);
                    pointFile[i] = new PointFile();
                    pointFile[i].X = double.Parse(listx.TupleSelect(i).ToString());
                    pointFile[i].Y = double.Parse(listy.TupleSelect(i).ToString());
                }
                pointFiles = pointFile;
                ListX = listx;
                ListY = listy;
                bitW = new List<sbyte>();
                bitW.AddRange(new sbyte[pointFiles.Length]);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 计数矩阵起点,间距计算矩阵，
        /// </summary>
        /// <param name="xNumber">X点数量</param>
        /// <param name="yNumber">Y点数量</param>
        /// <param name="xInterval">X间距</param>
        /// <param name="yInterval">Y间距</param>
        /// <param name="xLocation">X起点</param>
        /// <param name="yLocation">Y起点</param>
        /// <param name="xLocationS"></param>
        /// <param name="yLocationS"></param>
        public static void Calculate(int xNumber, int yNumber, int xInterval, int yInterval, int xLocation, int yLocation, out double[] xLocationS, out double[] yLocationS)
        {
            xLocationS = new double[xNumber * yNumber];
            yLocationS = new double[xNumber * yNumber];
            try
            {
                for (int i = 0; i < xNumber * yNumber; i++)
                {
                    int sd = i / yNumber;
                    int dt = i % yNumber;
                    xLocationS[i] = dt * xInterval + xLocation;
                    yLocationS[i] = sd * yInterval + yLocation;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///
        /// </summary>
        private TrayControl TrayControl;

        public void SetControl(TrayControl trayControl)
        {
            TrayControl = trayControl;
        }

        public void Calculate(out HTuple listx, out HTuple listy, HWindow hawindid = null)
        {
            listx = new HTuple();
            listy = new HTuple();
            this.Clear();
            try
            {
                Is8Point = false;
                //bitW = new List<sbyte> { };
                //bitW.AddRange(new sbyte[XNumber * YNumber]);
                HOperatorSet.GenRegionLine(out HObject hObject1, P1.X, P1.Y, P2.X, P2.Y);
                HOperatorSet.GenRegionLine(out HObject hObject2, P2.X, P2.Y, P3.X, P3.Y);
                HOperatorSet.GenRegionLine(out HObject hObject3, P3.X, P3.Y, P4.X, P4.Y);
                HOperatorSet.GenRegionLine(out HObject hObject4, P4.X, P4.Y, P1.X, P1.Y);
                hObject1 = hObject1.ConcatObj(hObject2);
                hObject1 = hObject1.ConcatObj(hObject3);
                hObject1 = hObject1.ConcatObj(hObject4);
                LineC(P1.X, P1.Y, P4.X, P4.Y, YNumber, out double[] xs, out double[] ys);
                LineC(P2.X, P2.Y, P3.X, P3.Y, YNumber, out double[] xs1, out double[] ys1);
                for (int i = 0; i < xs.Length; i++)
                {
                    LineC(xs[i], ys[i], xs1[i], ys1[i], XNumber, out double[] xs2, out double[] ys2);
                    listx.Append(xs2);
                    listy.Append(ys2);
                }
                pointFiles = new PointFile[listx.Length];
                bitW.Clear();
                sbyte[] vs = new sbyte[listx.Length];
                bitW.AddRange(vs);
                TrayDataS.XNumber = XNumber;
                TrayDataS.YNumber = YNumber;
                TrayDataS.Clear();
                for (int i = 0; i < listx.Length; i++)
                {
                    listx[i] = Math.Round(listx.TupleSelect(i).D);
                    listy[i] = Math.Round(listy.TupleSelect(i).D);
                    pointFiles[i] = new PointFile();
                    pointFiles[i].X = double.Parse(listx.TupleSelect(i).ToString());
                    pointFiles[i].Y = double.Parse(listy.TupleSelect(i).ToString());
                    pointFiles[i].Z = P1.Z;
                    //pointFiles[i].z = P1.Z;
                }
                HOperatorSet.GenCrossContourXld(out HObject hObject, listx, listy, 10, 0);
                if (hawindid != null)
                {
                    HOperatorSet.DispObj(hObject, hawindid);
                }

                ListX = listx;
                ListY = listy;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            this.Number = 1;
        }

        private static void LineC(HTuple x, HTuple y, HTuple xEnd, HTuple yEnd, int numbert, out double[] xs, out double[] ys)
        {
            xs = new double[numbert];
            ys = new double[numbert];
            HOperatorSet.LinePosition(x, y, xEnd, yEnd, out HTuple s, out HTuple c, out HTuple leng, out HTuple phi);
            HOperatorSet.AngleLx(x, y, xEnd, yEnd, out phi);
            HOperatorSet.HomMat2dIdentity(out HTuple homMat3d);
            HOperatorSet.HomMat2dTranslate(homMat3d, x, y, out homMat3d);
            HOperatorSet.HomMat2dRotate(homMat3d, phi, x, y, out homMat3d);
            for (int i = 0; i < numbert; i++)
            {
                HOperatorSet.AffineTransPoint2d(homMat3d, 0, leng / (numbert - 1) * i, out HTuple rowt, out HTuple colt);
                xs[i] = Math.Round(rowt.D, 2);
                ys[i] = Math.Round(colt.D, 2);
            }
        }

        public TrayControl GetControl()
        {
            return new TrayControl(this);
        }

        private IAxisGrub AxisSPD;

        public IAxisGrub GetAxis(IAxisGrub axisS = null)
        {
            if (axisS == null)
            {
                AxisSPD = axisS;
            }
            return AxisSPD;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="d"></param>
        public void UPsetTrayNumbar(int d)
        {
            try
            {
                d = d - 1;
                int rowt = d / XNumber;
                int colt = d % XNumber;
                bitW[d] = 1;
                Number = (sbyte)d + 1;
                if (TrayControl != null)
                {
                    TrayControl.UPsetTrayNumbar(d + 1);
                }
            }
            catch (Exception)
            {
            }
        }
    }

    public class TrayData
    {
        public bool IsSn { get { return tray1.IsSN; } }

        public bool IsTryaSN { get { return tray1.IsTraySN; } }

        public TrayData(TrayRobot trayRobot)
        {
            tray1 = trayRobot;
            SetITrayRobot(tray1.GetITrayRobot());
            Clear();
        }

        public string GetTrayString()
        {
            return string.Format("TraySN:{0};  TrayID:{1},X{2}*Y{3}/数量:{4},NG数:{5}当前:{6}",
                      this.TrayIDQR, tray1.Name, XNumber, YNumber, Count, NGLocation.Count, Number);
        }

        private TrayRobot tray1;
        private ITrayRobot trayRobots;

        public ITrayRobot GetITrayRobot()
        {
            return trayRobots;
        }

        public List<string> GetTraySN()
        {
            List<string> listSn = new List<string>();
            for (int i = 0; i < dataVales1.Count; i++)
            {
                listSn.Add(dataVales1[i].PanelID);
            }
            return listSn;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="trayRobot"></param>
        public void SetITrayRobot(ITrayRobot trayRobot)
        {
            trayRobots = trayRobot;
        }

        [DescriptionAttribute("起点位置四个角落，并根据横向或竖向共8种排列方式。"), Category("排列"), DisplayName("起点位置")]
        public TrayDirectionEnum TrayDirection { get; set; }

        [DescriptionAttribute("横向Fales或竖向Ture。"), Category("排列"), DisplayName("横向或竖向")]
        public bool HorizontallyORvertically { get; set; }

        [DescriptionAttribute("X方向数量。"), Category("排列"), DisplayName("X方向数量")]
        public sbyte XNumber { get; set; }

        [DescriptionAttribute("Y方向数量。"), Category("排列"), DisplayName("Y方向数量")]
        public sbyte YNumber { get; set; }

        [DescriptionAttribute("结果状态。"), Category("数据"), DisplayName("托盘产品数量")]
        public List<sbyte> bitW { get; set; } = new List<sbyte>();

        [DescriptionAttribute("判断整盘结果信息，托盘ID，穴位ID，穴位数据等。"), Category("结果"), DisplayName("整盘是否OK")]
        public bool OK
        {
            get
            {
                try
                {
                    if (tray1.IsTraySN)
                    {
                        if (TrayIDQR == null || TrayIDQR == "")
                        {
                            return false;
                        }
                    }

                    if (GetDataVales() != null)
                    {
                        for (int i = 0; i < GetDataVales().Count; i++)
                        {
                            if (GetDataVales()[i].NotNull)
                            {
                                if (!GetDataVales()[i].OK)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                }
                catch (Exception)
                {
                }
                return false;
            }
        }

        public string MesRestStr = "";

        public bool Done
        {
            get
            {
                if (dataVales1 != null)
                {
                    for (int i = 0; i < dataVales1.Count; i++)
                    {
                        if (dataVales1[i] != null)
                        {
                            if (!dataVales1[i].Done)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// NG位置
        /// </summary>
        public List<int> NGLocation = new List<int>();

        public int NGNubmer
        {
            get
            {
                int numbert = 0;
                for (int i = 0; i < dataVales1.Count; i++)
                {
                    if (!dataVales1[i].OK)
                    {
                        numbert++;
                    }
                }
                return numbert;
            }
        }

        public int OKNumber
        {
            get
            {
                int numbert = 0;
                for (int i = 0; i < dataVales1.Count; i++)
                {
                    if (dataVales1[i].OK)
                    {
                        numbert++;
                    }
                }
                return numbert;
            }
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string Product_Name { get; set; }

        [DescriptionAttribute("托盘ID。"), Category("结果"), DisplayName("托盘ID")]
        public string TrayIDQR { get; set; } = "";

        [DescriptionAttribute("托盘位置。"), Category("数据"), DisplayName("托盘位置")]
        public int Number
        {
            get;
            set;
        }

        [DescriptionAttribute("穴位数量。"), Category("排列"), DisplayName("总数量")]
        public int Count
        {
            get
            {
                return XNumber * YNumber;
            }
        }

        private List<OneDataVale> dataVales1;

        public List<OneDataVale> GetDataVales(List<OneDataVale> dataVales = null)
        {
            if (dataVales != null)
            {
                dataVales1 = dataVales;
            }
            return dataVales1;
        }

        public OneDataVale GetOneDataVale(int nubemr)
        {
            if (dataVales1 != null)
            {
                if (this.Count > nubemr)
                {
                    return GetDataVales()[nubemr];
                }
            }
            return null;
        }

        public HObject ImagePlus;

        public void RestValue()
        {
            Clear();
            if (trayRobots != null)
            {
                trayRobots.RestValue(this);
            }
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            if (tray1 != null)
            {
                XNumber = tray1.XNumber;
                YNumber = tray1.YNumber;
                TrayDirection = tray1.TrayDirection;
                TrayIDQR = "";
                HorizontallyORvertically = tray1.HorizontallyORvertically;
            }
            Number = 1;
            dataVales1 = new List<OneDataVale>(new OneDataVale[XNumber * YNumber]);
            for (int i = 0; i < dataVales1.Count; i++)
            {
                dataVales1[i] = new OneDataVale();
                dataVales1[i].TrayLocation = (i + 1);
            }
        }

        public void SetNumberValue(int number, bool value, double? valueDouble)
        {
            trayRobots.SetValue(number, value, valueDouble);
        }

        public void SetPanleSN(List<string> listSN, List<int> tryaid)
        {
            if (trayRobots == null)
            {
                Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(this.tray1.Name + "托盘未加载显示", System.Drawing.Color.Red);
            }
            else
            {
                trayRobots.SetPanleSN(listSN, tryaid);
            }
        }

        public void SetNumberValue(int number, OneDataVale dataVale)
        {
            trayRobots.SetValue(number, dataVale);
        }

        public void SetNumberValue(int number, TrayData TrayResetD)
        {
            trayRobots.SetValue(number, TrayResetD);
        }

        public void SetNumberValue(int number, int errCode)
        {
            trayRobots.SetValue(number, errCode);
        }

        public void SetNumberValue(int number, bool Vaules)
        {
            trayRobots.SetValue(number, Vaules);
        }

        public void SetNumberValue(List<bool> Vaules)
        {
            trayRobots.SetValue(Vaules);
        }

        public void SetNumberValue(bool Vaules)
        {
            for (int i = 0; i < dataVales1.Count; i++)
            {
                if (Vaules)
                {
                    if (dataVales1[i].PanelID != "")
                    {
                        dataVales1[i].OK = Vaules;
                    }
                }
                else
                {
                    dataVales1[i].OK = Vaules;
                }

                if (Vaules)
                {
                    dataVales1[i].NotNull = Vaules;
                }
            }
            trayRobots.SetValue(Vaules);
        }

        public void SetNumberValue(List<DataMinMax> dataMins, int number = 0)
        {
            //if (number != 0)
            //{
            //    Number = number;
            //}
            //if (Number <=0)
            //{
            //    Number = 1;
            //}
            for (int i = 0; i < dataMins.Count; i++)
            {
                this.GetDataVales()[number - 1].AddCamOBj(RecipeCompiler.Instance.DataMCamName, dataMins[i].GetOneRObj());
            }
            SetNumberValue(number, this.GetDataVales()[number - 1]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="d"></param>
        public void UPsetTrayNumbar(int d)
        {
            try
            {
                d = d - 1;
                int rowt = d / XNumber;
                int colt = d % XNumber;
                bitW[d] = 1;
                Number = (sbyte)d + 1;
            }
            catch (Exception)
            {
            }
        }

        public void UpITrayRobt()
        {
            trayRobots.UpData();
        }

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
                                    if (textT[i2].Contains(Sipe.ToString()) && textT[i2].Split(Sipe)[StrtCol] == rset)
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
                                    SimulateQRForm.ShowMesabe(err);
                                }
                                else
                                {
                                    UserFormulaContrsl.SetOK(3);
                                }
                                this.GetITrayRobot().SetValue(ListR);
                                System.IO.Directory.CreateDirectory(ProcessUser.Instancen.ExcelPath + "\\历史记录\\");
                                System.IO.File.Move(Pahts[i],
                                    ProcessUser.Instancen.ExcelPath + "\\历史记录\\" + System.IO.Path.GetFileName(Pahts[i]));
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
            List<int> NumberLo = new List<int>();
            List<string> SN = new List<string>();
            List<string> Datas = new List<string>();
            List<string> OKs = new List<string>();

            try
            {
                //文件名规则
                //  日期+字符+托盘ID  =[关键字(托盘ID，和日期时间)]字符[关键字托盘(trayid)]

                string FileName = DateTime.Now.ToString("yyyyMMdd");
                int row = 1;
                int Column = 1;
                string fileDta = ".txt";
                int trype = 0;//1=包含数据，0=不包含数据
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
                                if (dtast[0] == "文件名")
                                {
                                    while (dtast[1].Contains("["))
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
                                            dtattd = DateTime.Now.ToString("yyyyMMdd");
                                            dtast[1] = dtast[1].Remove(dtast[1].IndexOf('['), dtast[1].IndexOf(']') - dtast[1].IndexOf('[') + 1);
                                            dtast[1] = dtast[1].Insert(stR, dtattd);
                                        }
                                    }
                                    FileName = dtast[1].Trim();
                                }
                                else if (dtast[0] == "后缀")
                                {
                                    fileDta = dtast[1].Trim();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dtat.Contains("="))
                        {
                            string[] dtast = dtat.Split('=');
                            if (dtast[0] == "文件名")
                            {
                                while (dtast[1].Contains("["))
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
                Datas.Add(ProcessUser.Instancen.CarrierQRIDName + ProcessUser.Instancen.Split_Symbol +
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
                if (fileDta == ".xls" || fileDta == ".csv")
                {
                    if (System.IO.File.Exists(ProcessUser.Instancen.ExcelPath + "\\" + FileName + fileDta))
                    {
                        Npoi.AddWriteColumnToExcel(ProcessUser.Instancen.ExcelPath + "\\" + FileName + fileDta, "托盘", "位号", "SN", "OK", "数据");
                    }
                    Npoi.AddRosWriteToExcel(ProcessUser.Instancen.ExcelPath + "\\" + FileName + fileDta, "托盘", DateTime.Now.ToString(), tray.GetDataVales().Count, tray.OK);
                }
                for (int i = 0; i < tray.GetDataVales().Count; i++)
                {
                    if (tray.GetDataVales()[i] == null)
                    {
                        continue;
                    }
                    if (fileDta == ".xls" || fileDta == ".csv")
                    {
                        Npoi.AddRosWriteToExcel(ProcessUser.Instancen.ExcelPath + "\\" + FileName + fileDta, "托盘", tray.GetDataVales()[i].TrayLocation,
                            tray.GetDataVales()[i].PanelID, tray.GetDataVales()[i].OK, tray.GetDataVales()[i].MesStr);
                    }
                    NumberLo.Add(tray.GetDataVales()[i].TrayLocation);
                    SN.Add(tray.GetDataVales()[i].PanelID);
                    List<double> objt = new List<double>();
                    objt.Add((i + 1));
                    string objtStr = "";
                    if (tray.GetDataVales()[i].PanelID != null)
                    {
                        deNumber++;
                        objtStr = tray.GetDataVales()[i].PanelID;
                        list1[i] = "";

                        if (objtStr != "" && list1.Contains(objtStr))
                        {
                            ErrString += (i + 1) + ":" + objtStr + ";";
                            Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("重复信息:" + objtStr);
                        }
                        else
                        {
                            Datas.Add(ProcessUser.Instancen.SN_Name + (deNumber) + ProcessUser.Instancen.Split_Symbol + objtStr);
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
                            Npoi.AddWriteColumnToExcel(ProcessUser.Instancen.ExcelPath + "\\" + FileName, "托盘", vs2.ToArray());
                        }
                        Npoi.AddRosWriteToExcel(ProcessUser.Instancen.ExcelPath + "\\" + FileName, "托盘", objt.ToArray());
                    }
                    else if (tray.GetDataVales()[i].Data != null && tray.GetDataVales()[i].Data.Count == 1)
                    {
                        double objtDouble = tray.GetDataVales()[i].Data[0];
                        objtStr += objtDouble;
                        Datas.Add(ProcessUser.Instancen.SN_Name + (deNumber) + ProcessUser.Instancen.Split_Symbol + objtStr + Environment.NewLine);
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
                    Npoi.AddRosWriteToExcel(ProcessUser.Instancen.ExcelPath + "\\" + FileName, "托盘", max.ToSArr());
                    Npoi.AddRosWriteToExcel(ProcessUser.Instancen.ExcelPath + "\\" + FileName, "托盘", min.ToSArr());
                    Npoi.AddRosWriteToExcel(ProcessUser.Instancen.ExcelPath + "\\" + FileName, "托盘", metw.ToSArr());
                }
                else
                {
                    if (Err)
                    {
                        UserFormulaContrsl.SetOK(2);
                        SimulateQRForm.ShowMesabe("托盘码重复:" + ErrString);
                    }
                    else
                    {
                        if (fileDta == ".txt")
                        {
                            Npoi.AddText(ProcessUser.Instancen.ExcelPath + "\\" + FileName + fileDta, Datas.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }
    }
}