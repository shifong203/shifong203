using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Vision2.Project.formula;
using Vision2.Project.Mes;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static ErosSocket.DebugPLC.Robot.TrayRobot;

namespace ErosSocket.DebugPLC.Robot
{
    /// <summary>
    /// 
    /// </summary>
 
    public interface ITrayRobot
    {
        void SetValue(int number, bool value, double? valueDouble = null);
        void SetValue(int number, DataVale dataVale);
        void SetValue(int number, TrayData dataVale);
        void SetPanleSN(List<string> listSN, List<int> tryaid);
        void RestValue();
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
            get { return number; }

            set
            {
                number = value;
            }
        }
        int number;

        ITrayRobot trayRobots;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trayRobot"></param>
        public void AddTary(ITrayRobot trayRobot)
        {
            trayRobots = trayRobot;
            TrayDataS.AddTary(trayRobots);
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
                    number = int.Parse(dataStrS[0]);
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
            number = 1;
            TrayDataS = new TrayData(this);
            //NGLocation = new List<int>();
            //TrayIDQR = "";
            //if (dataVales1 != null)
            //{
            //    dataVales1.Clear();
            //    dataVales1 = new List<DataVale>(new DataVale[XNumber * YNumber]);
            //}
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

        public TrayData GetTrayData(TrayData data=null)
        {
            if (data!=null)
            {
                TrayDataS = data;
                TrayDataS.TrayDirection = this.TrayDirection;
                TrayDataS.XNumber = XNumber;
                TrayDataS.YNumber = YNumber;
                TrayDataS.HorizontallyORvertically = HorizontallyORvertically;
            }
            if (TrayDataS==null)
            {
                TrayDataS = new TrayData(this);
            }
            TrayDataS.AddTary(trayRobots);
            return TrayDataS;
        }

        public TrayData QuntData()
        {
            TrayData tray = TrayDataS;
            TrayDataS = new TrayData(this);
            TrayDataS.RestValue();
            return tray;
        }


        TrayData TrayDataS ;
        PointFile[] pointFiles;
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
                double[] ys2=null;
                double[] xs2 = null ;
                double[] xs12 = null; 
                double[] ys12 = null;
                if (Is8Point)
                {
                    LineC(p5.X, p5.Y, p8.X, p8.Y, yNumber2, out  xs2, out  ys2);
                    LineC(p6.X, p6.Y, p7.X, p7.Y, yNumber2, out  xs12, out  ys12);
                }

                for (int i = 0; i < xs.Length; i++)
                {
                    LineC(xs[i], ys[i], xs1[i], ys1[i], xNumber, out double[] x1, out double[] y1);
                    listx.Append(x1);
                    listy.Append(y1);
                    if (ys12!=null)
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
        TrayControl TrayControl;
        public void SetControl(TrayControl trayControl)
        {
            TrayControl = trayControl;
        }
 

        public void Calculate(out HTuple listx, out HTuple listy, HWindow hawindid = null)
        {
            listx = new HTuple();
            listy = new HTuple();
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
            }
            this.Number = 1;
        }
        static void LineC(HTuple x, HTuple y, HTuple xEnd, HTuple yEnd, int numbert, out double[] xs, out double[] ys)
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
        IAxisGrub AxisSPD;
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
        public TrayData( TrayRobot trayRobot)
        {
            XNumber = trayRobot. XNumber;
            YNumber = trayRobot.YNumber;
            TrayDirection = trayRobot.TrayDirection;
            HorizontallyORvertically = trayRobot.HorizontallyORvertically;
            AddTary(trayRobot.GetITrayRobot());
            Clear();
        }
        ITrayRobot trayRobots;
        public void AddTary(ITrayRobot trayRobot)
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
                    if (DebugComp.GetThis().TrayID)
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

                            if (GetDataVales()[i] == null || !GetDataVales()[i].OK)
                            {
                                return false;
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
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Product_Name { get; set; }
        [DescriptionAttribute("托盘ID。"), Category("结果"), DisplayName("托盘ID")]
        public string TrayIDQR { get; set; } = "";

        [DescriptionAttribute("托盘位置。"), Category("数据"), DisplayName("托盘位置")]
        public int Number
        {
            get ;
            set ;
        }
        [DescriptionAttribute("穴位数量。"), Category("排列"), DisplayName("总数量")]
        public int Count
        {
            get
            {
                   return XNumber * YNumber ;
            }
        }
        List<DataVale> dataVales1;
        public List<DataVale> GetDataVales(List<DataVale> dataVales = null)
        {
            if (dataVales != null)
            {
                dataVales1 = dataVales;
            }
            return dataVales1;
        }

        public HObject ImagePlus;

        public void RestValue()
        {
            if (trayRobots!=null)
            {
                trayRobots.RestValue();
            }
    
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            dataVales1 = new List<DataVale>(new DataVale[XNumber * YNumber]);
            for (int i = 0; i < dataVales1.Count; i++)
            {
                dataVales1[i] = new DataVale();
            }
        }
        public void SetNumberValue(int number, bool value, double? valueDouble)
        {
             trayRobots.SetValue(number, value, valueDouble);
        }
        public void SetPanleSN(List<string> listSN, List<int> tryaid)
        {
            trayRobots.SetPanleSN(listSN, tryaid);
        }
        public void SetNumberValue(int number, DataVale dataVale)
        {
           trayRobots.SetValue(number, dataVale);
        }

        public void SetNumberValue(int number, TrayData TrayResetD)
        {
            trayRobots.SetValue(number, TrayResetD);
        }
        public void SetNumberValue(int number, bool Vaules)
        {
           trayRobots.SetValue(number, Vaules);
        }
        public void SetNumberValue(List<DataMinMax> dataMins, int number = 0)
        {
            if (number != 0)
            {
                Number = number;
            }
            if (Number <=0)
            {
                Number = 1;
            }
            for (int i = 0; i < dataMins.Count; i++)
            {
                this.GetDataVales()[Number - 1].AddCamOBj("上相机" , dataMins[i].GetOneRObj());
            }
            SetNumberValue(Number, this.GetDataVales()[Number - 1]);
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


    }


}
