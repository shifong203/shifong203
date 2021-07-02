using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project.DebugF.IO;
using Vision2.vision;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.Vision;

namespace Vision2.Project.DebugF.工艺库
{
    public class MatrixC
    {


        /// <summary>
        /// 起点位置
        /// </summary>
        [Description("位置点名称"), Category("起点位置"), DisplayName("起点点名称"),
       TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("ListName")]
        public string PointName { get; set; }
        public static List<string> ListName
        {
            get
            {
                return new List<string>(DebugCompiler.GetThis().DDAxis.GetToPointName());
            }
        }


        [Description("结束点名称，并计算出角度"), Category("起点位置"), DisplayName("结束点名称"),
         TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("ListName")]
        public string PointNameEnd { get; set; }
        [Description("起点位置矩阵方向角度"), Category("起点位置"), DisplayName("角度")]
        public double Angle { get; set; }

        [Description("起点位置矩阵X方向步进间距"), Category("位置"), DisplayName("X间距")]
        public double XInterval { get; set; }
        [Description("起点位置矩阵Y方向步进间距"), Category("位置"), DisplayName("Y间距")]
        public double YInterval { get; set; }
        [Description("矩阵宽度"), Category("位置"), DisplayName("矩阵宽度")]
        public double Width { get; set; }
        [Description("矩阵高度"), Category("位置"), DisplayName("矩阵高度")]
        public double Heith { get; set; }
        [Description("使用矩阵计算或点位计算"), Category("计算方式"), DisplayName("使用点位计算")]
        public bool IsPointD { get; set; } = true;


        public List<double> XS { get; set; }
        public List<double> YS { get; set; }

        public int ImageWidth { get; set; }

        public int ImageHeith { get; set; }

        public int Xindxe { get; private set; }
        public int Yindxe { get; private set; }

        [Description("X正反移动方向"), Category("位置"), DisplayName("X方向")]
        public bool IsXHet { get; set; }

        [Description("Y正反移动方向"), Category("位置"), DisplayName("Y方向")]
        public bool IsYHet { get; set; }

        [Description("是否平铺图像"), Category("图像"), DisplayName("平铺图像")]
        public bool IsFillImage { get; set; }


        [Description("轴组名称"), Category("轴组"), DisplayName("轴组名")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("AxisGrotList", false, true)]
        public string AxisName { get; set; }
        public static string[] AxisGrotList
        {
            get
            {
                List<string> vs = new List<string>();
                vs = DebugCompiler.GetThis().DDAxis.AxisGrot.Keys.ToList();
                for (int i = 0; i < ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP.Count; i++)
                {
                    vs.Add(ErosSocket.DebugPLC.DebugComp.GetThis().ListAxisP[i].Name);
                }

                return vs.ToArray();
            }
        }
        public static string[] AxisVisionList
        {
            get
            {
                List<string> vs = new List<string>();
                vs = Vision.GetHimageList().Keys.ToList();

                return vs.ToArray();
            }
        }
        [Description("视觉程序名"), Category("轴组"), DisplayName("视觉程序")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("AxisVisionList", false, true)]
        public string VisionName { get; set; }

        public HTuple ImageSRows;
        public HTuple ImageSCols;

        public HTuple Rows { get; set; }
        public HTuple Cols { get; set; }
        List<HObject> iMAGES = new List<HObject>();
        HTuple Rows1 = new HTuple();
        HTuple Cols1 = new HTuple();
        HTuple Rows2 = new HTuple();
        HTuple Cols2 = new HTuple();
        double MaxRow;
        double MaxCol;
        HTuple hRows;
        HTuple hCols;
        HTuple hXs;
        HTuple hYs;
        HWindID HWind;
        HTuple disW;
        HTuple disH;
        public void SetHwindId(HWindID hWindID)
        {
            HWind = hWindID;
        }
        /// <summary>
        /// 计算结果
        /// </summary>
        public bool Calculate(double x, double y)
        {
            try
            {
                CountMatrix(x, y, Angle, XInterval, YInterval, Width, Heith, IsXHet, IsYHet, out List<double> xs, out List<double> ys, out HTuple rows, out HTuple cols);
                Rows = rows;
                Cols = cols;
                XS = xs;
                YS = ys;
                if (ys.Count >= 1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }
        public double mark2Row;
        public double mark2Col;
        public double mark1Row;
        public double mark1Col;
        public double markZPoint { get; set; }

        HObject HObjectRect1;
        public HObject GetHObject()
        {
            return HObjectRect1;
        }
        public void ShowMark(HWindID hWindID)
        {
            try
            {
                HTuple rows;
                HTuple cols;


                Vision.GetRunNameVision(VisionName).GetCalib().ShowCoordinate(hWindID);
                hWindID.HeigthImage = this.ImageHeith;
                hWindID.WidthImage = this.ImageWidth;
                Vision.GetRunNameVision(VisionName).GetCalib().GetPointXYtoRC(5, 0, out HTuple rowT, out HTuple colsT);
                Vision.GetRunNameVision(VisionName).GetCalib().GetPointXYtoRC(0, 0, out HTuple rowT2, out HTuple colsT2);
                HOperatorSet.DistancePp(rowT, colsT, rowT2, colsT2, out HTuple disW);
                XYZPoint point = DebugCompiler.GetThis().DDAxis.GetToPointFileProt(PointName);
                if (point != null)
                {
                    Vision.GetRunNameVision(VisionName).GetCalib().GetPointXYtoRC(point.Y, point.X, out rows, out cols);
                    HOperatorSet.GenCircle(out HObject hObject1, rows, cols, disW);
                    HOperatorSet.GenCrossContourXld(out HObject hObjectxs, rows, cols, disW, 0);
                    hWindID.OneResIamge.AddObj(hObject1.ConcatObj(hObjectxs), ColorResult.yellow);
                    mark1Row = rows;
                    mark1Col = cols;
                    hWindID.OneResIamge.AddImageMassage(rows, cols, "MK1");
                }
                point = DebugCompiler.GetThis().DDAxis.GetToPointFileProt(this.PointNameEnd);
                if (point != null)
                {
                    Vision.GetRunNameVision(VisionName).GetCalib().GetPointXYtoRC(point.Y, point.X, out rows, out cols);
                    HOperatorSet.GenCrossContourXld(out HObject hObjectxs, rows, cols, disW, 0);
                    mark2Row = rows;
                    mark2Col = cols;
                    HOperatorSet.GenCircle(out HObject hObject1, rows, cols, disW);
                    hWindID.OneResIamge.AddObj(hObject1.ConcatObj(hObjectxs), ColorResult.yellow);
                    hWindID.OneResIamge.AddImageMassage(rows, cols, "MK2");
                }

                //hWindID.ShowImage();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 计算结果
        /// </summary>
        public bool Calculate(HWindID HWindID2 = null)
        {
            try
            {
                HTuple rows;
                HTuple cols;
                XYZPoint point;

                point = DebugCompiler.GetThis().DDAxis.GetToPointFileProt(PointName);

                XYZPoint point2 = DebugCompiler.GetThis().DDAxis.GetToPointFileProt(PointNameEnd);
                if (point == null)
                {
                    return false;
                }
                if (IsPointD)
                {
                    if (point2 == null)
                    {
                        return false;
                    }
                    double rx = point.X - point2.X;
                    double ry = point.Y - point2.Y;
                    Width = Math.Abs(rx);
                    Heith = Math.Abs(ry);
                }
                Xindxe = (int)(Width / XInterval);
                Yindxe = (int)(Heith / YInterval);
                if ((Width % XInterval) != 0)
                {
                    Xindxe++;
                }
                if ((Heith % YInterval) != 0)
                {
                    Yindxe++;
                }
                ImageSRows = new HTuple();
                ImageSCols = new HTuple();
                Vision.GetRunNameVision(VisionName).GetCalib().GetPointXYtoRC(this.XInterval, 0, out HTuple rowT, out HTuple colsT);
                Vision.GetRunNameVision(VisionName).GetCalib().GetPointXYtoRC(0, 0, out HTuple rowT2, out HTuple colsT2);
                HOperatorSet.DistancePp(rowT, colsT, rowT2, colsT2, out disW);
                Vision.GetRunNameVision(VisionName).GetCalib().GetPointXYtoRC(0, this.YInterval, out rowT, out colsT);
                HOperatorSet.DistancePp(rowT, colsT, rowT2, colsT2, out disH);
                List<double> xs = new List<double>();
                List<double> ys = new List<double>();

                if (IsPointD)
                {
                    CountMatrix(point.X, point.Y, 0, XInterval, YInterval, Width, Heith, IsXHet, IsYHet, out xs, out ys, out rows, out cols, HWindID2);
                }
                else
                {
                    CountMatrix(point.X, point.Y, Angle, XInterval, YInterval, Width, Heith, IsXHet, IsYHet, out xs, out ys, out rows, out cols, HWindID2);
                }
                XS = xs;
                YS = ys;
                Rows = rows;
                Cols = cols;
                int dcc = Xindxe * Yindxe;
                for (int i = 0; i < Xindxe; i++)
                {
                    for (int i2 = 0; i2 < Yindxe; i2++)
                    {
                        dcc--;
                        double dsa = Rows[dcc] - Vision.GetRunNameVision(VisionName).GetCam().Height / 2;
                        if (dsa < 0 && MaxRow > dsa)
                        {
                            MaxRow = dsa;
                        }
                        dsa = Cols[dcc] - Vision.GetRunNameVision(VisionName).GetCam().Width / 2;
                        if (dsa < 0 && MaxCol > dsa)
                        {
                            MaxCol = dsa;
                        }
                        ImageSRows.Append(Rows[dcc] - Vision.GetRunNameVision(VisionName).GetCam().Height / 2);
                        ImageSCols.Append(Cols[dcc] - Vision.GetRunNameVision(VisionName).GetCam().Width / 2);
                    }
                }
                //HOperatorSet.GenRectangle2(out HObject hObject, Rows, Cols,
                //HTuple.TupleGenConst(Rows.Length, 0), HTuple.TupleGenConst(Rows.Length, disW) , HTuple.TupleGenConst(Rows.Length, disH)  );
                //HOperatorSet.GenRectangle1(out HObject hObject, ImageSRows.TupleSub(MaxRow), ImageSCols.TupleSub(MaxCol), ImageSRows.TupleAdd(Vision.GetRunNameVision(VisionName).GetCam().Height).TupleSub(MaxRow), ImageSCols.TupleAdd(Vision.GetRunNameVision(VisionName).GetCam().Width).TupleSub(MaxCol));

                if (HWindID2 != null)
                {
                    HWindID2.OneResIamge.ClearAllObj();
                    ShowMark(HWindID2);
                    //HWindID2.HalconResult.AddObj(hObject, RunProgram.ColorResult.blue);
                    for (int i = 0; i < Rows.Length; i++)
                    {
                        HWindID2.OneResIamge.AddImageMassage(Rows[i], Cols[i], i + 1, ColorResult.blue);
                    }
                }
                if (HWind != null)
                {
                    HWind.OneResIamge.ClearAllObj();
                    ShowMark(HWind);

                    HTuple rRow1 = mark1Row;
                    HTuple rRow2 = mark2Row;
                    HTuple rCol1 = mark1Col;
                    HTuple rCol2 = mark2Col;
                    if (rRow2 < rRow1)
                    {
                        rRow1 = mark2Row;
                        rRow2 = mark1Row;
                    }
                    if (rCol2 < rCol1)
                    {
                        rCol2 = mark1Col;
                        rCol1 = mark2Col;
                    }

                    HOperatorSet.GenRectangle1(out HObjectRect1, rRow1, rCol1, rRow2, rCol2);
                    HOperatorSet.SmallestRectangle2(HObjectRect1, out HTuple row, out HTuple col, out HTuple phi, out HTuple len1, out HTuple l2);
                    HWind.ImageRowStrat = rRow1.TupleInt() - (len1.TupleMult(0.10).TupleInt());
                    HWind.ImageColStrat = rCol1.TupleInt() - (len1.TupleMult(0.10).TupleInt());
                    HWind.WidthImage = rCol2.TupleInt() + (len1.TupleMult(0.10).TupleInt());
                    HWind.HeigthImage = rCol2.TupleInt() + (len1.TupleMult(0.10).TupleInt());
                    HWind.OneResIamge.AddObj(HObjectRect1, ColorResult.yellow);
                    HWind.ShowImage();
                    //HWind.HalconResult.AddObj(hObject, RunProgram.ColorResult.blue);
                    for (int i = 0; i < Rows.Length; i++)
                    {
                        HWind.OneResIamge.AddImageMassage(Rows[i], Cols[i], i + 1, ColorResult.blue);
                    }
                }

                ImageWidth = Vision.GetRunNameVision(VisionName).GetCam().Width * Xindxe;
                ImageHeith = Vision.GetRunNameVision(VisionName).GetCam().Height * Yindxe;
                if (ys.Count >= 1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public bool MarkMove(string MarkName1, string Mark2Name, string axisName = null, HWindID hWindID = null)
        {

            XYZPoint point = DebugCompiler.GetThis().DDAxis.GetToPointFileProt(MarkName1);
            if (point == null)
            {
                return false;
            }
            if (axisName == null)
            {
                axisName = AxisName;
            }
            if (DebugCompiler.GetThis().DDAxis.SetXYZ1Points(axisName, 10, point.X, point.Y, point.Z))
            {
                System.Threading.Thread.Sleep(DebugCompiler.GetThis().MarkWait);
                Vision.GetRunNameVision(VisionName).ReadCamImage("Mark1",1);
                //Vision.GetRunNameVision(VisionName).CamImageEvent("Mark1", null, 1);
                HOperatorSet.GenCircle(out HObject hObject1, point.Y, point.X, 10);
                HOperatorSet.GenCrossContourXld(out HObject hObjectxs, point.Y, point.X, 10, 0);
                if (hWindID != null)
                {
                    hWindID.OneResIamge.ClearAllObj();
                    Vision.Gen_arrow_contour_xld(out HObject hObject, 0, 0, 0, 100);
                    hWindID.OneResIamge.AddObj(hObject, ColorResult.green);
                    hWindID.OneResIamge.AddImageMassage(10, 110, "x");
                    vision.Vision.Gen_arrow_contour_xld(out HObject hObject22, 0, 0, 100, 0);
                    hWindID.OneResIamge.AddImageMassage(100, 10, "y");
                    hWindID.OneResIamge.AddObj(hObject22, ColorResult.yellow);
                    this.Calculate(hWindID);
                }
                if (Vision.GetRunNameVision(VisionName).ResultBool)
                {
                    hWindID.OneResIamge.AddObj(hObject1.ConcatObj(hObjectxs), ColorResult.green);
                    hWindID.OneResIamge.AddImageMassage(point.Y, point.X, "Mark1", ColorResult.green);
                    point = DebugCompiler.GetThis().DDAxis.GetToPointFileProt(Mark2Name);
                    if (DebugCompiler.GetThis().DDAxis.SetXYZ1Points(axisName, 10, point.X, point.Y, point.Z))
                    {
                        System.Threading.Thread.Sleep(DebugCompiler.GetThis().MarkWait);
                        Vision.GetRunNameVision(VisionName).ReadCamImage("Mark2", 2);
                        //Vision.GetRunNameVision(VisionName).CamImageEvent("Mark1", null, 2);
                        HOperatorSet.GenCrossContourXld(out hObjectxs, point.Y, point.X, 10, 0);
                        HOperatorSet.GenCircle(out hObject1, point.Y, point.X, 10);
                        if (Vision.GetRunNameVision(VisionName).ResultBool)
                        {
                            if (hWindID != null)
                            {
                                hWindID.OneResIamge.AddObj(hObject1.ConcatObj(hObjectxs), ColorResult.yellow);
                                hWindID.OneResIamge.AddImageMassage(point.Y, point.X, "Mark2", ColorResult.green);
                            }
                            return true;
                        }
                        else
                        {
                            if (hWindID != null)
                            {
                                hWindID.OneResIamge.AddObj(hObject1.ConcatObj(hObjectxs), ColorResult.red);
                                hWindID.OneResIamge.AddImageMassage(point.Y, point.X, "Mark2", ColorResult.red);
                            }
                            Vision.GetRunNameVision(VisionName).GetOneImageR().AddMeassge("Mark2定位失败！");
                        }
                    }
                }
                else
                {
                    if (hWindID != null)
                    {
                        hWindID.OneResIamge.AddObj(hObject1.ConcatObj(hObjectxs), ColorResult.red);
                        hWindID.OneResIamge.AddImageMassage(point.Y, point.X, "Mark1", ColorResult.red);
                    }
                    Vision.GetRunNameVision(VisionName).GetOneImageR().AddMeassge("Mark1定位失败！");
                }
                Vision.GetRunNameVision(VisionName).ShowObj();
            }
            else
            {
            }
            return false;
        }

        public void Mark1Move(string MarkName1, string axisName = null)
        {

            XYZPoint point = DebugCompiler.GetThis().DDAxis.GetToPointFileProt(MarkName1);
            if (axisName == null)
            {
                axisName = AxisName;
            }
            if (DebugCompiler.GetThis().DDAxis.SetXYZ1Points(axisName, 10, point.X, point.Y, point.Z))
            {
                System.Threading.Thread.Sleep(DebugCompiler.GetThis().MarkWait);
                Vision.GetRunNameVision(VisionName).ReadCamImage(MarkName1,1);
                //Vision.GetRunNameVision(VisionName).CamImageEvent(MarkName1, null, 1);
                if (Vision.GetRunNameVision(VisionName).ResultBool)
                {

                }
                else
                {
                    Vision.GetRunNameVision(VisionName).GetOneImageR().AddMeassge("Mark定位失败！");
                    Vision.GetRunNameVision(VisionName).ShowObj();
                }
            }
            else
            {
            }
        }

        public void MarkCiacelbMove(string MarkName1, string axisName = null, HWindID hWindID = null)
        {

            if (hWindID != null)
            {
                hWindID.OneResIamge.ClearAllObj();
                Vision.Gen_arrow_contour_xld(out HObject hObject, 0, 0, 0, 100);
                hWindID.OneResIamge.AddObj(hObject, ColorResult.green);
                hWindID.OneResIamge.AddImageMassage(10, 110, "x");
                vision.Vision.Gen_arrow_contour_xld(out HObject hObject22, 0, 0, 100, 0);
                hWindID.OneResIamge.AddObj(hObject22, ColorResult.red);
                hWindID.OneResIamge.AddImageMassage(100, 10, "y");
            }
            if (axisName == null)
            {
                axisName = AxisName;
            }
            hRows = new HTuple();
            hCols = new HTuple();
            hXs = new HTuple();
            hYs = new HTuple();
            try
            {
                XYZPoint point = DebugCompiler.GetThis().DDAxis.GetToPointFileProt(MarkName1);
                double Seelp = 5;
                double XD = point.X - Seelp;
                double YD = point.Y - Seelp;

                for (int i = 0; i < 9; i++)
                {
                    double xp = XD + i / 3 * Seelp;
                    double yp = YD + i % 3 * Seelp;
                    if (DebugCompiler.GetThis().DDAxis.SetXYZ1Points(axisName, 10, xp, yp, point.Z))
                    {
                        HOperatorSet.GenCrossContourXld(out HObject hObjectxs, yp, xp, 5, 0);
                        HOperatorSet.GenCircle(out HObject hObject1, yp, xp, 2);
                        if (hWindID != null)
                        {
                            hWindID.OneResIamge.AddObj(hObject1.ConcatObj(hObjectxs), ColorResult.yellow);
                            hWindID.OneResIamge.AddImageMassage(yp, xp, (i + 1), ColorResult.green);
                        }
                        System.Threading.Thread.Sleep(500);
                        Vision.GetRunNameVision(VisionName).Image(Vision.GetRunNameVision(VisionName).GetCam().GetImage());
                        Vision.GetRunNameVision(VisionName).CamImageEvent(MarkName1, null, i + 1);
                        if (Vision.GetRunNameVision(VisionName).ResultBool)
                        {

                            hRows.Append(Vision.GetRunNameVision(VisionName).GetHomdeMobelEx(MarkName1).Row);
                            hCols.Append(Vision.GetRunNameVision(VisionName).GetHomdeMobelEx(MarkName1).Col);
                            hXs.Append(xp);
                            hYs.Append(yp);

                        }
                        else
                        {
                            Vision.GetRunNameVision(VisionName).GetOneImageR().AddMeassge("Mark定位失败！");
                            Vision.GetRunNameVision(VisionName).ShowObj();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                Vision.GetRunNameVision(VisionName).GetCalib().VectorToHomMat2d(hRows, hCols, hYs, hXs);
                //HOperatorSet.VectorToHomMat2d(hRows, hCols,    hYs, hXs, out HTuple HomMat);
                HOperatorSet.GenCrossContourXld(out HObject hObject, hRows, hCols, 20, 0);
                Vision.GetRunNameVision(VisionName).GetCalib().GetPointRctoXY(hRows, hCols, out HTuple axisY, out HTuple axisX);
                Vision.GetRunNameVision(VisionName).AddObj(hObject);
                Vision.GetRunNameVision(VisionName).GetOneImageR().AddMeassge(Vision.GetRunNameVision(VisionName).GetCalib().GetMatHomString());
                Vision.GetRunNameVision(VisionName).GetOneImageR().AddMeassge(Vision.GetRunNameVision(VisionName).GetCalib().GetRCMatHomString());

                if (hXs.TupleSub(axisX).TupleAbs().TupleMax() < 0.05 && hYs.TupleSub(axisY).TupleAbs().TupleMax() < 0.05)
                {
                    Vision.GetRunNameVision(VisionName).GetOneImageR().AddMeassge("标定成功x误差:" + hXs.TupleSub(axisX).TupleAbs().TupleMax().TupleString("0.03f") + "y误差:" + hYs.TupleSub(axisY).TupleAbs().TupleMax().TupleString("0.003f"));
                }
                else
                {
                    Vision.GetRunNameVision(VisionName).GetOneImageR().AddMeassge("标定失败x误差:" + hXs.TupleSub(axisX).TupleAbs().TupleMax().TupleString("0.03f") + "y误差:" + hYs.TupleSub(axisY).TupleAbs().TupleMax().TupleString("0.003f"));
                }
                Vision.GetRunNameVision(VisionName).GetOneImageR().AddMeassge("机械差X（" + axisX + "/" + hXs + "=" + hXs.TupleSub(axisX) + "）;Y（"
                   + axisY + "/" + hYs + "=" + hYs.TupleSub(axisY) + ")");

                for (int i = 0; i < hRows.Length; i++)
                {
                    Vision.GetRunNameVision(VisionName).GetOneImageR().AddImageMassage(hRows[i], hCols[i], (i + 1));
                }

                if (hWindID != null)
                {
                    hWindID.OneResIamge.AddMeassge(Vision.GetRunNameVision(VisionName).GetCalib().GetMatHomString());
                    hWindID.OneResIamge.AddMeassge(Vision.GetRunNameVision(VisionName).GetCalib().GetRCMatHomString());

                    HOperatorSet.GenCrossContourXld(out HObject hObjectxs, this.hYs, this.hXs, 5, HTuple.TupleGenConst(this.hXs.Length, 1));
                    hWindID.OneResIamge.AddObj(hObjectxs, ColorResult.yellow);

                }

            }
            catch (Exception ex)
            {

                Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(ex.Message);
            }
        }

        System.Diagnostics.Stopwatch watchOut = new System.Diagnostics.Stopwatch();
        public bool MoveMxet(string axisName = null, HWindID hWindID = null)
        {
            try
            {
                int dcc = Xindxe * Yindxe;
                if (hWindID != null)
                {
                    hWindID.HobjClear();
                    watchOut.Restart();
                }
                if (HWind != null)
                {
                    HWind.HobjClear();
                    ShowMark(HWind);
                }
                if (axisName == null)
                {
                    axisName = AxisName;
                }

                HObject hObject2 = new HObject();
                HTuple RowsImage = new HTuple();
                HTuple ColsImage = new HTuple();
                iMAGES = new List<HObject>();
                HOperatorSet.GenImageConst(out HObject hObject1, "byte", ImageWidth / 2, ImageHeith / 2);

                for (int i = 0; i < XS.Count; i++)
                {
                    HObject hObject = new HObject();
                    if (Vision.GetRunNameVision(VisionName).GetCam().CameraImageFormat != "Mono8")
                    {
                        HOperatorSet.Compose3(hObject1, hObject1, hObject1, out hObject);
                    }
                    else
                    {
                        HOperatorSet.GenImageConst(out hObject1, "byte", ImageWidth / 2, ImageHeith / 2);
                    }
                    iMAGES.Add(hObject);
                }
                XYZPoint point = DebugCompiler.GetThis().DDAxis.GetToPointFileProt(PointName);
                if (point == null)
                {
                    return false;
                }
                Rows1 = new HTuple();
                Cols1 = new HTuple();
                Rows2 = new HTuple();
                Cols2 = new HTuple();
                Rows1 = HTuple.TupleGenConst(XS.Count, -1);
                Cols1 = HTuple.TupleGenConst(XS.Count, -1);
                Rows2 = HTuple.TupleGenConst(XS.Count, -1);
                Cols2 = HTuple.TupleGenConst(XS.Count, -1);
                for (int i = 0; i < XS.Count; i++)
                {
                    if (DebugCompiler.GetThis().DDAxis.SetXYZ1Points(axisName, 10, XS[i], YS[i], point.Z))
                    {
                        System.Threading.Thread.Sleep(DebugCompiler.GetThis().MarkWait);
                        Vision.GetRunNameVision(VisionName).AsysReadCamImage(0, (i + 1), asyncRestImage =>
                        {
                            try
                            {
                                iMAGES[asyncRestImage.RunID - 1] = asyncRestImage.Image;
                                HOperatorSet.GenRectangle2(out hObject2, Rows[XS.Count - (asyncRestImage.RunID)] - (MaxRow), Cols[XS.Count - (asyncRestImage.RunID)] - (MaxCol), 0, disH, disW);
                                if (HWind != null)
                                {
                                    HWind.OneResIamge.AddObj(hObject2, ColorResult.yellow);
                                    HWind.ShowImage();
                                }
                                if (hWindID != null)
                                {
                                    hWindID.OneResIamge.AddObj(hObject2, ColorResult.yellow);
                                    if (asyncRestImage.RunID == XS.Count)
                                    {

                                        FillIamge(hWindID);
                                        hWindID.OneResIamge.AddMeassge(watchOut.ElapsedMilliseconds + "ms");
                                    }
                                    hWindID.ShowImage();
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        });
                        System.Threading.Thread.Sleep(DebugCompiler.GetThis().CamWait);
                    }
                    else
                    {
                        return false;
                    }
                }
                //if (hWindID!=null)
                //{
                //    FillIamge(hWindID);
                //}
                return true;
            }
            catch (Exception ex)
            {
                Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(ex.Message);
            }
            return false;

        }
        public void FillIamge(HWindID hWindID = null)
        {

            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            HObject hObject2 = new HObject();
            HTuple RowsImage = new HTuple();
            HTuple ColsImage = new HTuple();
            for (int i2 = 0; i2 < iMAGES.Count; i2++)
            {
                hObject = hObject.ConcatObj(iMAGES[i2]);
            }
            int dcc = Xindxe * Yindxe;
            if (IsFillImage)
            {
                ImageSRows = new HTuple();
                ImageSCols = new HTuple();
                for (int i = 0; i < Xindxe; i++)
                {
                    for (int i2 = 0; i2 < Yindxe; i2++)
                    {
                        dcc--;
                        double dsa = Rows[dcc] - Vision.GetRunNameVision(VisionName).GetCam().Height / 2;
                        if (dsa < 0 && MaxRow > dsa)
                        {
                            MaxRow = dsa;
                        }
                        dsa = Cols[dcc] - Vision.GetRunNameVision(VisionName).GetCam().Width / 2;
                        if (dsa < 0 && MaxCol > dsa)
                        {
                            MaxCol = dsa;
                        }
                        ImageSRows.Append(Rows[dcc] - Vision.GetRunNameVision(VisionName).GetCam().Height / 2);
                        ImageSCols.Append(Cols[dcc] - Vision.GetRunNameVision(VisionName).GetCam().Width / 2);
                    }
                }
                //HOperatorSet.GenRectangle1(out hObject2, ImageSRows.TupleSub(MaxRow), ImageSCols.TupleSub(MaxCol),
                //ImageSRows.TupleAdd((Vision.GetRunNameVision(VisionName).GetCam().Height) - MaxRow), ImageSCols.TupleAdd((Vision.GetRunNameVision(VisionName).GetCam().Width) - MaxCol));
                RowsImage = ImageSRows;
                ColsImage = ImageSCols;
            }
            else
            {
                for (int i = 0; i < Xindxe; i++)
                {
                    for (int i2 = 0; i2 < Yindxe; i2++)
                    {
                        dcc--;
                        RowsImage.Append(i2 * Vision.GetRunNameVision(VisionName).GetCam().Height);
                        ColsImage.Append(i * Vision.GetRunNameVision(VisionName).GetCam().Width);
                    }
                }
            }
            if (disH!=null)
            {
                HOperatorSet.GenRectangle2(out hObject2, RowsImage, ColsImage, HTuple.TupleGenConst(Cols.Length, 0), HTuple.TupleGenConst(Cols.Length, disH), HTuple.TupleGenConst(Cols.Length, disW));
            }

            try
            {
                HOperatorSet.TileImages(hObject, out hObject, Xindxe, "vertical");
                //       HOperatorSet.TileImages(hObject, out hObject, Yindxe, "vertical");
                //     HOperatorSet.TileImages(hObject, out hObject, Yindxe, "horizontal");
                //HOperatorSet.TileImagesOffset(hObject, out hObject, RowsImage, ColsImage, Rows1, Cols1, Rows2, Cols2, ImageWidth, ImageHeith);
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine(ex.Message);
            }
            if (hWindID != null)
            {
                hWindID.HobjClear();
                hWindID.OneResIamge.AddObj(hObject2, ColorResult.yellow);
                hWindID.OneResIamge.Image = hObject;
                hWindID.SetImaage(hObject);
                hWindID.ShowImage();
            }
            if (HWind != null)
            {
                HWind.HobjClear();
                ShowMark(HWind);
                HTuple rRow1 = mark1Row;
                HTuple rRow2 = mark2Row;
                HTuple rCol1 = mark1Col;
                HTuple rCol2 = mark2Col;
                if (rRow2 < rRow1)
                {
                    rRow1 = mark2Row;
                    rRow2 = mark1Row;
                }
                if (rCol2 < rCol1)
                {
                    rCol2 = mark1Col;
                    rCol1 = mark2Col;
                }

                HOperatorSet.GenRectangle1(out HObjectRect1, rRow1, rCol1, rRow2, rCol2);
                HOperatorSet.SmallestRectangle2(HObjectRect1, out HTuple row, out HTuple col, out HTuple phi, out HTuple len1, out HTuple l2);
                HWind.ImageRowStrat = rRow1.TupleInt() - (len1.TupleMult(0.10).TupleInt());
                HWind.ImageColStrat = rCol1.TupleInt() - (len1.TupleMult(0.10).TupleInt());
                HWind.WidthImage = rCol2.TupleInt() + (len1.TupleMult(0.10).TupleInt());
                HWind.HeigthImage = rCol2.TupleInt() + (len1.TupleMult(0.10).TupleInt());
                HWind.OneResIamge.AddObj(HObjectRect1, ColorResult.yellow);
                HWind.OneResIamge.Image = hObject;
                HWind.ShowImage();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originX">起点x</param>
        /// <param name="originY">起点y</param>
        /// <param name="angle">角度默认0</param>
        /// <param name="xSeelp">X步进距离</param>
        /// <param name="ySeelp">Y步进距离</param>
        /// <param name="width">矩阵宽度</param>
        /// <param name="heith">矩阵高度</param>
        /// <param name="Xs">输出点集合X</param>
        /// <param name="Ys">输出点集合Y</param>
        public void CountMatrix(double originX, double originY, double angle, double xSeelp, double ySeelp, double width, double heith, bool isHet, bool isYHet, out List<double> Xs, out List<double> Ys, out HTuple Rows, out HTuple Cols, HWindID hwind = null)
        {
            Xs = new List<double>();
            Ys = new List<double>();
            Rows = new HTuple();
            Cols = new HTuple();
            int XindexC = (int)(width / xSeelp);
            int YindexC = (int)(heith / ySeelp);
            if ((width % xSeelp) != 0)
            {
                XindexC++;
            }
            if ((heith % ySeelp) != 0)
            {
                YindexC++;
            }

            try
            {
                HOperatorSet.HomMat2dIdentity(out HTuple hTuple);
                HOperatorSet.HomMat2dRotate(hTuple, new HTuple(angle).TupleRad(), originX, originY, out hTuple);
                vision.Vision.GetRunNameVision(VisionName).GetCalib().GetPointXYtoRC(originY, originX, out HTuple row, out HTuple col);

                HOperatorSet.GenCrossContourXld(out HObject hObject2, row, col, 40, 0);
                if (hwind != null)
                {
                    HOperatorSet.HomMat2dTranslateLocal(hTuple, 0, width, out HTuple hTuple1);
                    HOperatorSet.AffineTransPoint2d(hTuple1, originX, originY, out HTuple px, out HTuple py);
                    Vision.GetRunNameVision(VisionName).GetCalib().GetPointXYtoRC(py, px, out HTuple rowT, out HTuple colsT);
                    Vision.Gen_arrow_contour_xld(out HObject hObject, row, col, rowT, colsT);
                    HOperatorSet.HomMat2dTranslateLocal(hTuple, heith, 0, out hTuple1);
                    HOperatorSet.AffineTransPoint2d(hTuple1, originX, originY, out px, out py);
                    Vision.GetRunNameVision(VisionName).GetCalib().GetPointXYtoRC(py, px, out rowT, out colsT);
                    Vision.Gen_arrow_contour_xld(out HObject hObject22, row, col, rowT, colsT);
                    hwind.OneResIamge.AddObj(hObject, ColorResult.green);
                    hwind.OneResIamge.AddObj(hObject22, ColorResult.yellow);
                }
                if (isHet)
                {
                    xSeelp = -xSeelp;

                }
                if (isYHet)
                {
                    ySeelp = -ySeelp;
                }
                for (int i = 0; i < XindexC; i++)
                {
                    for (int i2 = 0; i2 < YindexC; i2++)
                    {
                        HTuple hTuple1 = new HTuple();
                        HOperatorSet.HomMat2dTranslateLocal(hTuple, xSeelp * i, ySeelp * i2, out hTuple1);
                        HOperatorSet.AffineTransPoint2d(hTuple1, originX, originY, out HTuple px, out HTuple py);
                        HOperatorSet.AffineTransPixel(hTuple1, originX, originY, out px, out py);
                        Vision.GetRunNameVision(VisionName).GetCalib().GetPointXYtoRC(py, px, out HTuple rowT, out HTuple colsT);
                        HOperatorSet.GenCrossContourXld(out HObject hObject, rowT, colsT, 10, new HTuple(angle).TupleRad());
                        Xs.Add(Math.Round(px.D, 2));
                        Ys.Add(Math.Round(py.D, 2));
                        Rows.Append(rowT);
                        Cols.Append(colsT);
                        if (hwind != null)
                        {
                            hwind.OneResIamge.AddObj(hObject);
                            hwind.OneResIamge.AddImageMassage(rowT, colsT, (i2 + 1) + (i * YindexC));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
