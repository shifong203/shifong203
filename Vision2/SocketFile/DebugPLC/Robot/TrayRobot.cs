using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Vision2.Project.Mes;

namespace ErosSocket.DebugPLC.Robot
{
    public interface ITrayRobot
    {
        void SetValue(int number, bool value, double? valueDouble = null);
        void SetValue(int number, DataVale dataVale);

        void RestValue();
    }
    public class TrayRobot
    {
        public enum TrayDirectionEnum
        {
            左上 = 0,
            右上 = 1,
            左下 = 2,
            右下 = 3,
        }

        [DescriptionAttribute("起点位置四个角落，并根据横向或竖向共8种排列方式。"), Category("排列"), DisplayName("起点位置")]
        public TrayDirectionEnum TrayDirection { get; set; }

        [DescriptionAttribute("托盘ID。"), Category("结果"), DisplayName("托盘ID")]
        public string TrayIDQR { get; set; } = "";
        [DescriptionAttribute("横向Fales或竖向Ture。"), Category("排列"), DisplayName("横向或竖向")]
        public bool HorizontallyORvertically { get; set; }
        public string Name { get; set; }
        [DescriptionAttribute("X方向数量。"), Category("排列"), DisplayName("X方向数量")]
        public sbyte XNumber { get; set; }
        [DescriptionAttribute("Y方向数量。"), Category("排列"), DisplayName("Y方向数量")]
        public sbyte YNumber { get; set; }

        [DescriptionAttribute("X2方向数量。"), Category("排列"), DisplayName("X2方向数量")]

        public sbyte X2Number { get; set; }
        [DescriptionAttribute("Y2方向数量。"), Category("排列"), DisplayName("Y2方向数量")]
        public sbyte Y2Number { get; set; }

        [DescriptionAttribute("穴位数量。"), Category("排列"), DisplayName("总数量")]
        public int Count
        {
            get
            {
                if (Is8Point)
                {
                    return XNumber * YNumber + X2Number * Y2Number; ;
                }
                return XNumber * YNumber;
            }
        }


        public class DataObj
        {
            public bool RAoOK;
            public bool OK
            {
                get
                {
                    if (DebugComp.GetThis().PalenID)
                    {
                        if (ID == null || ID == "")
                        {
                            return false;
                        }
                    }
                    return ok;
                }

                set { ok = value; }
            }
            bool ok;

            public object Inage(object data=null)
            {
            
                if (data!=null)
                {
                    iamge = data;
                }
                return iamge;
            }
             object iamge;
            public List<double> Data { get; set; }
            public string ID { get; set; }
        }

        [DescriptionAttribute("整盘穴位信息。"), Category("结果"), DisplayName("穴位信息")]
        public List<DataObj> dataObjs { get; set; }


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
                    if (dataObjs != null)
                    {
                        for (int i = 0; i < dataObjs.Count; i++)
                        {

                            if (dataObjs[i] == null || !dataObjs[i].OK)
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
        [DescriptionAttribute("使用8点位。"), Category("位置"), DisplayName("使用8点位")]
        public bool Is8Point { get; set; }
        [DescriptionAttribute("结果状态。"), Category("数据"), DisplayName("托盘产品数量")]
        public List<sbyte> bitW { get; set; } = new List<sbyte>();

        [DescriptionAttribute("托盘位置。"), Category("数据"), DisplayName("托盘位置")]
        public int Number
        {
            get { return number; }

            set
            {
                number = value;
                string data = number.ToString();
                for (int i = 0; i < bitW.Count; i++)
                {
                    data += "," + bitW[i].ToString();
                }
                System.IO.File.WriteAllText(Vision2.ErosProjcetDLL.Project.ProjectINI.TempPath + Name + "Tray.txt", data);
            }
        }
        int number;
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
        PointFile[] pointFiles;
        public PointFile GetPoint(int number)
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
        public static void Calculate(sbyte xNumber, sbyte yNumber, PointFile p1, PointFile p2, PointFile p3, PointFile p4, out PointFile[] pointFile)
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
        TrayControl TrayControl;
        public void SetControl(TrayControl trayControl)
        {
            TrayControl = trayControl;
        }
        public void SetITary(ITrayRobot trayControl)
        {
            trayRobot = trayControl;
        }
        public void RestValue()
        {
            if (trayRobot != null)
            {
                trayRobot.RestValue();
            }
        }

        ITrayRobot trayRobot;
        public void SetNumberValue(int number, bool value, double? valueDouble)
        {
            if (trayRobot != null)
            {
                trayRobot.SetValue(number, value, valueDouble);
            }
        }
        public void SetNumberValue(int number, DataVale dataVale)
        {
            if (trayRobot != null)
            {
                trayRobot.SetValue(number, dataVale);
            }
        }
        public void SetNumberValue(int number, bool Vaules)
        {

        }



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
        public void Calculate(out HTuple listx, out HTuple listy, HWindow hawindid = null)
        {
            listx = new HTuple();
            listy = new HTuple();
            try
            {
                Is8Point = false;
                bitW = new List<sbyte> { };
                bitW.AddRange(new sbyte[XNumber * YNumber]);
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
                dataObjs = new List<DataObj>();
                dataObjs.AddRange(new DataObj[listx.Length]);

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

        public static void Disp_message(HTuple hv_WindowHandle, HTuple hv_String,
             double hv_Row = 20, double hv_Column = 20, bool hv_CoordSystem = false, string hv_Color = "red", string hv_Box = "false")
        {
            // Local control variables
            if (hv_Box == null)
            {
                hv_Box = "";
            }
            if (hv_Color == null)
            {
                hv_Color = "yellow";
            }
            if (hv_Column == null)
            {
                hv_Column = 20;
            }
            if (hv_Row == null)
            {
                hv_Row = 20;
            }
            HTuple hv_Red, hv_Green, hv_Blue, hv_Row1Part;
            HTuple hv_Column1Part, hv_Row2Part, hv_Column2Part, hv_RowWin;
            HTuple hv_ColumnWin, hv_WidthWin, hv_HeightWin, hv_MaxAscent;
            HTuple hv_MaxDescent, hv_MaxWidth, hv_MaxHeight, hv_R1 = new HTuple();
            HTuple hv_C1 = new HTuple(), hv_FactorRow = new HTuple(), hv_FactorColumn = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple(), hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple(), hv_W = new HTuple(), hv_H = new HTuple();
            HTuple hv_FrameHeight = new HTuple(), hv_FrameWidth = new HTuple();
            HTuple hv_R2 = new HTuple(), hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_CurrentColor = new HTuple();

            HTuple hv_Color_COPY_INP_TMP = hv_Color;
            HTuple hv_Column_COPY_INP_TMP = hv_Column;
            HTuple hv_Row_COPY_INP_TMP = hv_Row;
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part,
                out hv_Column2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            //
            //default settings
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            //
            hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
            try
            {
                //Estimate extentions of text depending on font size.
                HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                   out hv_MaxWidth, out hv_MaxHeight);
                if (hv_CoordSystem)
                {
                    hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                    hv_C1 = hv_Column_COPY_INP_TMP.Clone();
                }
                else
                {
                    //transform image to window coordinates
                    hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                    hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                    hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                    hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
                }
                //
                //display text box depending on text size
                if (hv_Box == "true")
                {
                    //calculate box extents
                    hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                    hv_Width = new HTuple();
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                            hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                        hv_Width = hv_Width.TupleConcat(hv_W);
                    }
                    hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                        ));
                    hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                    hv_R2 = hv_R1 + hv_FrameHeight;
                    hv_C2 = hv_C1 + hv_FrameWidth;
                    //display rectangles
                    HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                    HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                    HOperatorSet.SetColor(hv_WindowHandle, "light gray");
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 3, hv_C1 + 3, hv_R2 + 3, hv_C2 + 3);
                    HOperatorSet.SetColor(hv_WindowHandle, "white");
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                    HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
                }
                else
                {
                    //hv_Exception = "Wrong value of control parameter Box";
                }
                //Write text.
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                        )));
                    if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                        "auto")))) != 0)
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                    }
                    else
                    {
                        HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                    }
                    hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                    HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                    HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index));
                }
                //reset changed window settings
                HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                    hv_Column2Part);

                return;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 绘制箭头
        /// </summary>
        /// <param name="hv_Row1"></param>
        /// <param name="hv_Column1"></param>
        /// <param name="hv_Row2"></param>
        /// <param name="hv_Column2"></param>
        /// <param name="hv_HeadLength"></param>
        /// <param name="hv_HeadWidth"></param>
        /// <returns></returns>
        public static HObject GenArrowContourXld(HTuple hv_Row1, HTuple hv_Column1,
        HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {
            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];

            // Local iconic variables

            HObject ho_TempArrow = null;

            // Local control variables

            HTuple hv_Length = null, hv_ZeroLengthIndices = null;
            HTuple hv_DR = null, hv_DC = null, hv_HalfHeadWidth = null;
            HTuple hv_RowP1 = null, hv_ColP1 = null, hv_RowP2 = null;
            HTuple hv_ColP2 = null, hv_Index = null;
            // Initialize local and output iconic variables
            HObject ho_Arrow = new HObject();
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);

            ho_Arrow.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            //
            //Calculate the arrow length
            HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
            //
            //Mark arrows with identical start and end point
            //(set Length to -1 to avoid division-by-zero exception)
            hv_ZeroLengthIndices = hv_Length.TupleFind(0);
            if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
            {
                if (hv_Length == null)
                    hv_Length = new HTuple();
                hv_Length[hv_ZeroLengthIndices] = -1;
            }
            //
            //Calculate auxiliary variables.
            hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
            hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
            hv_HalfHeadWidth = hv_HeadWidth / 2.0;
            //
            //Calculate end points of the arrow head.
            hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
            hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
            hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
            hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
            //
            //Finally create output XLD contour for each input point pair
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                {
                    //Create_ single points for arrows with identical start and end point
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(hv_Index),
                        hv_Column1.TupleSelect(hv_Index));
                }
                else
                {
                    //Create arrow contour
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                        hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                        ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                        hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_Arrow, ho_TempArrow, out ExpTmpOutVar_0);
                    ho_Arrow.Dispose();
                    ho_Arrow = ExpTmpOutVar_0;
                }
            }
            ho_TempArrow.Dispose();
            return ho_Arrow;
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

    }

}
