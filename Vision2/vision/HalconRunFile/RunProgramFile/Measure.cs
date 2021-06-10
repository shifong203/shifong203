using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.Controls;
using static Vision2.vision.HalconRunFile.RunProgramFile.HalconRun;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public interface IMeasure
    {
        void MeasureOb(HalconRun halcon);

        void DrawMeasure(HalconRun halcon);

        void ShowMeasure(HalconRun halcon);

        void ShowDraw(HalconRun halcon);
    }
    public class Measure : RunProgram
    {

        /// <summary>
        /// 曲线测量结果判定
        /// </summary>
        /// <returns></returns>
        public bool NurbsMeasureResult(HalconRun halconRun)
        {

            HTuple sa = halconRun.GetCam().CaliConst;

            HTuple distanceMM = new HTuple(0);
            DistanceMax = 3.8;
            bool bRet = false;
            int Onftint = drawRows.TupleLength();
            this.SetDefault("NG总数", 2, true);
            this.SetDefault("断胶最大数量", 4, true);
            //曲线查找失败点索引
            HTuple _findFailcountIndex = new HTuple();
            int itet = 0;
            if (distance.TupleLength() != 0)
            {
                bRet = true;
                _findFailcountIndex = distance.TupleFind(0);

                if (_findFailcountIndex.TupleSelect(0) == -1)
                {
                    _findFailcountIndex = new HTuple();
                }
                Onftint = _findFailcountIndex.Length;
                //涂胶轨迹测量点小于理想测量点数量返回失败
                if (_findFailcountIndex.Length > this["NG总数"] || drawCols.Length - distance.Length > this["NG总数"])
                {
                    bRet = false;
                }
                HOperatorSet.GenEmptyObj(out HObject OKSegments);
                HOperatorSet.GenEmptyObj(out HObject NGSegments);
                HOperatorSet.GenEmptyObj(out HObject ResultSegments);
                //胶宽从像素单位转MM单位
                distanceMM = halconRun.GetCaliConstMM(distance);
                //distanceMM = distanceMM.TupleRemove(_findFailcountIndex);
                for (int i = 0; i < distanceMM.Length; i++)
                {
                    if (distanceMM.TupleSelect(i).D == 0)
                    {
                        if (OutRows.TupleSelect(i).D == 0)
                        {
                            HOperatorSet.GenRectangle2(out HObject hObject, drawRows.TupleSelect(i),
                                drawCols.TupleSelect(i), DrawPhi.TupleSelect(i),
                                this.Length / sa, this.DrawLength2 / 2);
                            HOperatorSet.ConcatObj(NGSegments, hObject, out NGSegments);
                        }
                        if (outCols2.TupleSelect(i).D == 0)
                        {
                            HOperatorSet.GenRectangle2(out HObject hObject, DrawRows2.TupleSelect(i),
                                DrawCols2.TupleSelect(i), DrawPhi.TupleSelect(i),
                                this.Length / sa, this.DrawLength2 / 2);
                            HOperatorSet.ConcatObj(NGSegments, hObject, out NGSegments);
                        }
                    }
                    else if (
                        distanceMM.TupleSelect(i) > DistanceMax ||
                        distanceMM.TupleSelect(i) < DistanceMin)
                    {
                        HOperatorSet.GenRectangle2(out HObject hObject, DrawRows2.TupleSelect(i),
                        DrawCols2.TupleSelect(i), DrawPhi.TupleSelect(i),
                        this.Length / halconRun.GetCam().CaliConst, this.DrawLength2 / 2);
                        HOperatorSet.ConcatObj(NGSegments, hObject, out NGSegments);
                        HOperatorSet.GenRectangle2(out hObject, drawRows.TupleSelect(i),
                         drawCols.TupleSelect(i), DrawPhi.TupleSelect(i),
                         this.Length / sa, this.DrawLength2 / 2);
                        HOperatorSet.ConcatObj(NGSegments, hObject, out NGSegments);
                        itet++;
                    }

                }
                if (this["NG总数"] < itet + Onftint)
                {
                    bRet = false;
                }
                //ObjectColor NGobjectColor = new ObjectColor(NGSegments, "rad");
                halconRun.AddOBJ(NGSegments, ColorResult.red);
                //halconRun.AddOBJ( NGobjectColor._HObject);
                if (distanceMM.Length > 0)
                {
                    halconRun.GetOneImageR().AddMeassge("平均胶宽:" + distanceMM.TupleMean().TupleString("0.03f") + "mm");
                    halconRun.GetOneImageR().AddMeassge("最大胶宽:" + distanceMM.TupleMax().TupleString("0.03f") + "mm");
                    if (distanceMM.TupleRemove(distanceMM.TupleFind(0)).Length > 0)
                    {
                        halconRun.GetOneImageR().AddMeassge("最小胶宽: " + distanceMM.TupleRemove(distanceMM.TupleFind(0)).TupleMin().TupleString("0.03f") + "mm");
                    }
                    else
                    {
                        halconRun.GetOneImageR().AddMeassge("最小胶宽: 0mm");
                    }
                }
            }

            halconRun["测量最大值"].Append(distanceMM.TupleMax().TupleString("0.03f"));
            halconRun["测量平均值"].Append(distanceMM.TupleMean().TupleString("0.03f"));
            halconRun["测量最小值"].Append(distanceMM.TupleMin().TupleString("0.03f"));
            halconRun["测量断胶数"].Append(Onftint);
            halconRun["测量NG数"].Append(itet);
            halconRun["测量总数"].Append(drawRows.TupleLength());

            if (Onftint > this["断胶最大数量"])
            {
                bRet = false;
            }

            halconRun.GetOneImageR().AddMeassge("测量数:" + (distance.TupleLength()
               - _findFailcountIndex.TupleLength()) + " 总数:" + drawRows.TupleLength() + "  NG数:" + itet.ToString() +
               " 断胶:" + Onftint.ToString());
            return bRet;
        }

        public override RunProgram UpSatrt<T>(string Path)
        {

            return base.ReadThis<Measure>(Path);
        }

        /// <summary>
        /// 获得直线的延长点位置，可负数
        /// </summary>
        /// <param name="rowStart">起点R</param>
        /// <param name="colStart">起点C</param>
        /// <param name="rowEnd">终点R</param>
        /// <param name="colEnd">终点C</param>
        /// <param name="Lengt">延长</param>
        /// <param name="extension_Row">延长位置R</param>
        /// <param name="extension_Col">延长位置C</param>
        public static void Pts_Ling_Extension(double rowStart, double colStart, double rowEnd, double colEnd, double Lengt,
            out double extension_Row, out double extension_Col)
        {

            HOperatorSet.TupleSqrt((rowEnd - rowStart) * (rowEnd - rowStart) + (colEnd - colStart) * (colEnd - colStart), out HTuple hTuple);
            extension_Row = rowEnd + Lengt / hTuple.D * (rowEnd - rowStart);
            extension_Col = colEnd + Lengt / hTuple.D * (colEnd - colStart);

            return;
        }

        /// <summary>
        /// 直线插值
        /// </summary>
        /// <param name="linePoints">点</param>
        /// <param name="x">X插值点</param>
        /// <returns>插值的Y值</returns>
        public static double? LineInterpolation(List<PointF> linePoints, double x)
        {
            int? idx1 = null;
            for (int index = 0; index < linePoints.Count; index++)
            {
                PointF linePoint = linePoints[index];
                if (linePoint.X >= x)
                {
                    idx1 = index;
                    break;
                }
            }

            int? idx2 = null;
            for (int index = linePoints.Count - 1; index >= 0; index--)
            {
                PointF linePoint = linePoints[index];
                if (linePoint.X <= x)
                {
                    idx2 = index;
                    break;
                }
            }

            if (!idx1.HasValue || !idx2.HasValue)
            {
                return null;
            }

            PointF p1 = linePoints[idx1.Value];
            PointF p2 = linePoints[idx2.Value];

            if (idx1.Value == idx2.Value || idx1 == idx2)
            {
                return p1.Y;
            }

            double xDiff = p1.X - p2.X;
            double yDiff = p1.Y - p2.Y;

            double y = p1.Y + (x - p1.X) * yDiff / xDiff;

            return y;
        }
        public void MeasureOb(HalconRun halcon)
        {
        }

        public void DrawMeasure(HalconRun halcon)
        {
        }

        public void ShowMeasure(HalconRun halcon)
        {
            halcon.AddOBJ(this.MeasureObj(halcon , halcon.GetOneImageR())._HObject);
            if (this.DrawRows != null && this.DrawRows.Length > 0)
            {
                Vision.Disp_message(halcon.hWindowHalcon(), this.Name, this.DrawRows[0], this.DrawCols[0], false, "black", "true");
            }
            else
            {
                Vision.Disp_message(halcon.hWindowHalcon(), this.Name + "未绘制", 20, 20, true, "red", "true");
            }
        }
        /// <summary>
        /// 测量到的点
        /// </summary>
        /// <param name="halcon"></param>
        public void ShowPoint(HalconRun halcon)
        {
            try
            {
                HOperatorSet.GenCrossContourXld(out HObject hObject, this.OutRows, this.OutCols, 10, 0);
                HOperatorSet.GenCrossContourXld(out HObject hObject2, this.OutCentreRow, this.OutCentreCol, 10, 0);
                halcon.AddOBJ(hObject);
                halcon.AddOBJ(hObject2);
                halcon.ShowImage();
                halcon.ShowObj();
            }
            catch (Exception)
            {
            }
        }
        public void ShowContPoint(HalconRun halcon)
        {
            try
            {
                HOperatorSet.GenCrossContourXld(out HObject hObject, this.OutCentreRow, this.OutCentreCol, 20, 0);
                halcon.AddOBJ(hObject);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 显示拟合点
        /// </summary>
        /// <param name="halcon"></param>
        public void ShowPstPoint(HalconRun halcon)
        {
            try
            {
                HOperatorSet.GenCrossContourXld(out HObject hObject, this["拟合点Rows"], this["拟合点Columns"], 30, 0);
                halcon.AddOBJ(hObject);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool ISSt;


        public void ShowDraw(HalconRun halcon)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="halcon"></param>
        /// <returns></returns>
        public override bool RunHProgram(HalconRun halcon, OneResultOBj oneResultOBj, int id, string name = null)
        {

            try
            {
                this.SetPThis(halcon);
                ObjectColor objectColor = this.MeasureObj(halcon, oneResultOBj);
                if (objectColor._HObject.CountObj() > 0)
                {
                    return true;
                }

            }
            catch (Exception)

            {
            }
            return false;
        }
        public override Control GetControl()
        {
            HalconRun halconRun = this.GetPThis() as HalconRun;
            MeasureConTrolEx measureConTrolEx = new MeasureConTrolEx(halconRun, this);

            return measureConTrolEx;
        }



        #region 属性字段

        //[DescriptionAttribute("测量名称"), Category("测量参数"), DisplayName("测量区域集合")]
        //[Bindable(false)]
        //public DicHObject _DicHObject { get { return KeyHObject; } set { KeyHObject = value; } }

        [DescriptionAttribute("是否启用测量"), Category("执行参数"), DisplayName("使能")]
        public bool Enabled { get; set; }

        [DescriptionAttribute("是否启用彷射"), Category("执行参数"), DisplayName("启用彷射")]
        public bool ISMatHat { get; set; }

        [DisplayName("绘制区域位置Rows"), Category("测量位置")]
        public HTuple DrawRows { get { return drawRows; } set { drawRows = value; } }
        private HTuple drawRows = new HTuple();
        [DisplayName("绘制区域位置Cols"), Category("测量位置")]
        public HTuple DrawCols { get { return drawCols; } set { drawCols = value; } }
        private HTuple drawCols = new HTuple();
        [DisplayName("绘制区域位置角度"), Category("测量位置")]
        public HTuple DrawPhi { get { return drawPhi; } set { drawPhi = value; } }
        HTuple drawPhi;

        public HTuple DrawRows2 { get; set; }
        public HTuple DrawCols2 { get; set; }
        public HTuple RightDrawPhi { get; set; }
        public HTuple rightDrawPhi;

        public HTuple DrawDirect;
        public HTuple DrawLength1 = 30;
        public HTuple DrawLength2 = 30;

        /// <summary>
        /// 彷射角度
        /// </summary>
        private HTuple HomMatPhi = new HTuple(0);

        [DescriptionAttribute("测量角度"), Category("测量位置"), DisplayName("区域位置角度")]
        public double Draw_Phi { get; set; }

        [DescriptionAttribute("测量中心R"), Category("测量位置"), DisplayName("测量的中心点Row")]
        public double DrawCentY { get; set; }

        [DescriptionAttribute("测量中心C"), Category("测量位置"), DisplayName("测量的中心点Col")]
        public double DrawCentX { get; set; }

        [DescriptionAttribute("测量宽度"), Category("测量位置"), DisplayName("测量的宽度")]
        public double Measure_Waigth
        {
            get;

            set;
        } = 10;

        [DescriptionAttribute("测量高度"), Category("测量位置"), DisplayName("测量的高度")]
        public double Measure_Heigth
        {
            get;
            set;
        } = 200;


        /// <summary>
        /// 测量点RowS组
        /// </summary>
        [DescriptionAttribute("测量结果Rows"), Category("测量结果"), DisplayName("测量的结果位置Rows")]
        public HTuple OutRows { get { return outRows; } set { outRows = value; } }

        private HTuple outRows = new HTuple();

        [DescriptionAttribute("测量结果Cols"), Category("测量结果"), DisplayName("测量的结果位置Cols")]
        /// <summary>
        /// 测量点Cols组
        /// </summary>
        public HTuple OutCols { get { return outCols; } set { outCols = value; } }

        private HTuple outCols = new HTuple();

        /// <summary>
        /// 测量点RowS组
        /// </summary>
        [DescriptionAttribute("测量结果Rows"), Category("测量结果2"), DisplayName("测量的结果位置Rows")]
        public HTuple OutRows2 { get { return outRows2; } set { outRows2 = value; } }
        private HTuple outRows2 = new HTuple();



        [Description("测量点连续的距离"), Category("测量结果"), DisplayName("点距离")]
        /// <summary>
        ///连续边缘之间的距离。
        /// </summary>
        public HTuple Distance { get { return distance; } set { distance = value; } }

        private HTuple distance;




        [DescriptionAttribute("测量结果Cols"), Category("测量结果2"), DisplayName("测量的结果位置Cols")]
        /// <summary>
        /// 测量点Cols组
        /// </summary>
        public HTuple OutCols2 { get { return outCols2; } set { outCols2 = value; } }

        private HTuple outCols2 = new HTuple();

        [DescriptionAttribute("测量结果中心Col"), Category("测量结果"), DisplayName("测量结果中心Col")]
        public double? OutCentreCol
        {
            get { return outCentreCol; }
            set
            {
                outCentreCol = value;
            }
        }

        private double? outCentreCol;

        [DescriptionAttribute("测量结果中心Row"), Category("测量结果"), DisplayName("测量结果中心Row")]
        public double? OutCentreRow { get { return outCentreRow; } set { outCentreRow = value; } }

        private double? outCentreRow;

        [DescriptionAttribute("测量结果弧度"), Category("测量结果"), DisplayName("测量结果的角度")]
        public double OutPhi { get { return outPhi; } set { outPhi = value; } }

        private double outPhi;

        [DescriptionAttribute("园半径"), Category("测量结果"), DisplayName("半径")]
        public double? OutRadius
        {
            get { return outRadius; }
            set
            {
                outRadius = value;
            }
        }
        double? outRadius;

        [DescriptionAttribute("园半径"), Category("测量结果"), DisplayName("半径MM")]
        public double? OutRadiusMM
        {
            get { return outRadiusMM; }
            set
            {
                outRadiusMM = value;
            }
        }
        double? outRadiusMM;


        [DescriptionAttribute("显示园半径"), Category("测量结果"), DisplayName("显示半径MM")]
        public bool IsRadius { get; set; }
        [DescriptionAttribute("最小拟合点数"), Category("拟合参数"), DisplayName("拟合最小点数量")]
        public int Min_Measure_Point_Number { get; set; } = 5;
        /// <summary>
        /// 测量数量或间距
        /// </summary>
        [DescriptionAttribute("测量点数量或表示测量间距，比拟合点大"), Category("测量参数"), DisplayName("测量的数量")]
        public double MeasurePointNumber { get; set; } = 50;

        [DescriptionAttribute("拟合点超出距离将被抛弃"), Category("测量参数"), DisplayName("拟合直线超差")]
        public double MeasurePointDistance { get; set; } = 10;
        [DescriptionAttribute("高斯平滑，测量点平滑参数"), Category("测量参数"), DisplayName("高斯平滑")]
        /// <summary>
        /// 高斯平滑
        /// </summary>
        public double Sigma { get; set; } = 1;
        [DescriptionAttribute("测量点之间的距离长度"), Category("测量参数"), DisplayName("测量长度")]
        /// <summary>
        /// 长度
        /// </summary>
        public Single Length { get; set; } = 10;
        [Description("黑与白边界的幅度"), Category("测量参数"), DisplayName("边缘幅度")]
        /// <summary>
        /// 最低边缘幅度。
        /// </summary>
        public double Threshold { get; set; } = 30;

        [Description("将直线处理为曲线"), Category("直线测量"), DisplayName("测量边界")]
        /// <summary>
        /// 最低边缘幅度。
        /// </summary>
        public bool ISMLine { get; set; }

        [Description("测量超差距离"), Category("直线测量"), DisplayName("测量超差")]
        /// <summary>
        /// 最低边缘幅度。
        /// </summary>
        public double MLineM { get; set; } = 10;

        [Description("测量方向all为最大点、negative为白到黑，Positive,为黑到白"), Category("测量参数"), DisplayName("测量方向")]
        /// <summary>
        /// 测量方向
        /// </summary>
        public Transition TransitionStr { get; set; }

        [Description("筛选方式all为全部，first为第一点，last为最后点"), Category("测量参数"), DisplayName("筛选测量点")]
        /// <summary>
        /// 筛选方式
        /// </summary>
        public Select SelectStr { get; set; }


        [DescriptionAttribute("高斯平滑，测量点平滑参数"), Category("测量参数2"), DisplayName("高斯平滑")]
        /// <summary>
        /// 高斯平滑
        /// </summary>
        public double Sigma2 { get; set; } = 1;
        [Description("黑与白边界的幅度"), Category("测量参数2"), DisplayName("边缘幅度")]
        /// <summary>
        /// 最低边缘幅度。
        /// </summary>
        public double Threshold2 { get; set; } = 30;

        [Description("测量方向all为最大点、negative为白到黑，Positive,为黑到白"), Category("测量参数2"), DisplayName("测量方向")]
        /// <summary>
        /// 测量方向
        /// </summary>
        public Transition TransitionStr2 { get; set; }

        [Description("筛选方式all为全部，first为第一点，last为最后点"), Category("测量参数2"), DisplayName("筛选测量点")]
        /// <summary>
        /// 筛选方式
        /// </summary>
        public Select SelectStr2 { get; set; }
        [Description("插值方式nearest_neighbor最邻近，bicubic二次立方，bilinear双线性"), Category("测量参数"), DisplayName("插值算法")]
        /// <summary>
        /// 插值方式
        /// </summary>
        public Interpolation InterpolationStr { get; set; }
        [Description("自由曲线测量参数，成对内，成对外"), Category("测量参数"), DisplayName("类型")]
        public string Tepy { get; set; } = "成对内";
        [Description("测量点的边缘振幅，正数为白，负数为黑"), Category("测量参数"), DisplayName("测量出的边缘振幅")]
        /// <summary>
        /// 边缘振幅(带符号)。
        /// </summary>
        public HTuple Amplitude { get { return amplitude; } set { amplitude = value; } }

        private HTuple amplitude;


        [Description("measure不测量，cilcre为拟合圆或弧，line为拟合直线，pake为顶点，" +
            "point为点测量，point2D为转换坐标测量点，pointIXLD在绘制线上测量点与XLD的焦点距离，" +
            "XLDToXLD为测量XLD与XLD的轮廓距离"), Category("测量参数"), DisplayName("测量类型")]
        /// <summary>
        /// 测量类型
        /// </summary>
        public MeasureType Measure_Type { get { return measure_Type; } set { measure_Type = value; } }

        private MeasureType measure_Type;

        /// <summary>
        /// 绘制区域
        /// </summary>
        public HObject DrawHObject;

        /// <summary>
        /// 彷射绘制区
        /// </summary>
        HObject HamMatDrawObj;
        public HObject GetHamMatDraw()
        {
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            if (OutCentreRow != null)
            {
                HOperatorSet.GenCrossContourXld(out hObject, this.OutCentreRow, this.OutCentreCol, 20, 0);
            }
            //HOperatorSet.GenCrossContourXld(out HObject hObject2, this.OutRows, this.OutCols, 50, 0);
            //hObject = hObject.ConcatObj(hObject2);
            return HamMatDrawObj.ConcatObj(hObject);
        }
        /// <summary>
        /// 测量区域
        /// </summary>
        public HObject MeasureHObj
        {
            get
            {
                if (measuHobj == null)
                {
                    measuHobj = new HObject();
                    measuHobj.GenEmptyObj();
                }
                if (!measuHobj.IsInitialized())
                {
                    measuHobj.GenEmptyObj();
                }
                return measuHobj;
            }
            set { measuHobj = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ho_xld"></param>
        /// <param name="ho_XLD1"></param>
        /// <param name="hv_hWindID"></param>
        /// <param name="hv_Length"></param>
        /// <param name="hv_Height"></param>
        /// <param name="hv_Weight"></param>
        /// <param name="hv_rows"></param>
        /// <param name="hv_cols"></param>
        /// <param name="hv_phis"></param>
        public void Darw_Measuring_Arc(HObject ho_xld, out HObject ho_XLD1, HTuple hv_hWindID,
            HTuple hv_Length, HTuple hv_Height, HTuple hv_Weight, out HTuple hv_rows, out HTuple hv_cols,
            out HTuple hv_phis)
        {
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];
            // Local iconic variables 
            HObject ho_Rectangle;
            // Local control variables 
            HTuple hv_ResultRow = null, hv_ResultColumn = null;
            HTuple hv_angles1 = null, hv_Row5 = null, hv_Cols5 = null;
            HTuple hv_i = null, hv_row1 = null, hv_col1 = null, hv_Index1 = null;
            HTuple hv_Distance = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_XLD1);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            hv_ResultRow = new HTuple();
            hv_ResultColumn = new HTuple();
            hv_angles1 = new HTuple();
            HOperatorSet.GetContourAngleXld(ho_xld, "abs", "range", 3, out hv_angles1);
            hv_phis = (hv_angles1.TupleSelect(0)) + ((new HTuple(90)).TupleRad());
            HOperatorSet.GetContourXld(ho_xld, out hv_Row5, out hv_Cols5);
            ho_XLD1.Dispose();
            HOperatorSet.GenEmptyObj(out ho_XLD1);
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_XLD1, ho_xld, out ExpTmpOutVar_0);
                ho_XLD1.Dispose();
                ho_XLD1 = ExpTmpOutVar_0;
            }
            hv_i = 1;
            hv_row1 = hv_Row5.TupleSelect(0);
            hv_col1 = hv_Cols5.TupleSelect(0);
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2(out ho_Rectangle, hv_row1, hv_col1, (hv_angles1.TupleSelect(
                0)) + ((new HTuple(90)).TupleRad()), hv_Weight, hv_Height);
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_XLD1, ho_Rectangle, out ExpTmpOutVar_0);
                ho_XLD1.Dispose();
                ho_XLD1 = ExpTmpOutVar_0;
            }
            hv_rows = new HTuple();
            hv_cols = new HTuple();

            HTuple end_val18 = (new HTuple(hv_Row5.TupleLength()
                )) - 1;
            HTuple step_val18 = hv_i;
            for (hv_Index1 = 1; hv_Index1.Continue(end_val18, step_val18); hv_Index1 = hv_Index1.TupleAdd(step_val18))
            {
                hv_i = hv_i + 1;
                HOperatorSet.DistancePp(hv_row1, hv_col1, hv_Row5.TupleSelect(hv_Index1), hv_Cols5.TupleSelect(
                    hv_Index1), out hv_Distance);
                if ((int)(new HTuple(hv_Distance.TupleGreaterEqual(hv_Length))) != 0)
                {
                    hv_i = hv_i.Clone();
                    hv_rows = hv_rows.TupleConcat(hv_row1);
                    hv_cols = hv_cols.TupleConcat(hv_col1);
                    hv_row1 = hv_Row5.TupleSelect(hv_Index1);
                    hv_col1 = hv_Cols5.TupleSelect(hv_Index1);
                    hv_phis = hv_phis.TupleConcat((hv_angles1.TupleSelect(hv_Index1)) + ((new HTuple(90)).TupleRad()
                        ));
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle2(out ho_Rectangle, hv_row1, hv_col1, (hv_angles1.TupleSelect(
                        hv_Index1)) + ((new HTuple(90)).TupleRad()), hv_Weight, hv_Height);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_XLD1, ho_Rectangle, out ExpTmpOutVar_0);
                        ho_XLD1.Dispose();
                        ho_XLD1 = ExpTmpOutVar_0;
                    }
                }
            }
            HOperatorSet.TupleRemove(hv_phis, new HTuple(hv_phis.TupleLength()), out hv_phis);

            ho_Rectangle.Dispose();
            return;
        }

        public void Darw_Measuring_Arc(HObject ho_xld, out HObject ho_XLD1, HTuple hv_hWindID,
        HTuple hv_Length, HTuple hv_stLength, HTuple hv_Height, HTuple hv_Weight, HTuple hv_tepy,
        out HTuple hv_rows, out HTuple hv_cols, out HTuple hv_phis, out HTuple hv_rows2,
        out HTuple hv_cols2)
        {

            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Arrow = null, ho_ContEllipse = null;
            HObject ho_Cross = null, ho_Rectangle = null, ho_Rectangle2 = null;

            // Local control variables 

            HTuple hv_ResultRow = null, hv_ResultColumn = null;
            HTuple hv_angles1 = null, hv_Row5 = null, hv_Cols5 = null;
            HTuple hv_i = null, hv_row1 = null, hv_col1 = null, hv_hv_RowC = null;
            HTuple hv_hv_ColC = null, hv_phi = null, hv_hv_RowL2 = new HTuple();
            HTuple hv_hv_RowL1 = new HTuple(), hv_hv_ColL2 = new HTuple();
            HTuple hv_hv_ColL1 = new HTuple(), hv_hv_RowL4 = new HTuple();
            HTuple hv_hv_RowL3 = new HTuple(), hv_hv_ColL4 = new HTuple();
            HTuple hv_hv_ColL3 = new HTuple(), hv_Index1 = null, hv_Distance = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_IsOverlapping = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_XLD1);
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_ContEllipse);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Rectangle2);

            hv_ResultRow = new HTuple();
            hv_ResultColumn = new HTuple();
            hv_angles1 = new HTuple();
            hv_rows = new HTuple();
            hv_cols = new HTuple();
            hv_phis = new HTuple();
            hv_rows2 = new HTuple();
            hv_cols2 = new HTuple();

            HOperatorSet.GetContourAngleXld(ho_xld, "abs", "range", 3, out hv_angles1);

            HOperatorSet.GetContourXld(ho_xld, out hv_Row5, out hv_Cols5);
            ho_XLD1.Dispose();
            HOperatorSet.GenEmptyObj(out ho_XLD1);
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_XLD1, ho_xld, out ExpTmpOutVar_0);
                ho_XLD1.Dispose();
                ho_XLD1 = ExpTmpOutVar_0;
            }
            hv_i = 1;
            hv_row1 = hv_Row5.TupleSelect(0);
            hv_col1 = hv_Cols5.TupleSelect(0);
            hv_hv_RowC = hv_row1.Clone();
            hv_hv_ColC = hv_col1.Clone();
            hv_rows = new HTuple();
            hv_cols = new HTuple();
            hv_rows2 = new HTuple();
            hv_cols2 = new HTuple();

            hv_phi = (new HTuple(90)).TupleRad();
            hv_phis = (hv_angles1.TupleSelect(0)) + hv_phi;
            if ((int)(new HTuple(hv_tepy.TupleEqual("单个正"))) != 0)
            {

                hv_hv_RowL2 = hv_hv_RowC + ((hv_stLength * 2) * (((-hv_phis)).TupleSin()));
                hv_hv_RowL1 = hv_hv_RowC - ((hv_stLength * 2) * (((-hv_phis)).TupleSin()));
                hv_hv_ColL2 = hv_hv_ColC + ((hv_stLength * 2) * (((-hv_phis)).TupleCos()));
                hv_hv_ColL1 = hv_hv_ColC - ((hv_stLength * 2) * (((-hv_phis)).TupleCos()));
                ho_Arrow.Dispose();
                Vision.Gen_arrow_contour_xld(out ho_Arrow, hv_hv_RowL1, hv_hv_ColL1, hv_hv_RowL2,
                    hv_hv_ColL2, 50, 10);

            }
            else if ((int)(new HTuple(hv_tepy.TupleEqual("单个反"))) != 0)
            {
                hv_hv_RowL2 = hv_hv_RowC + ((hv_stLength * 2) * (((-hv_phis)).TupleSin()));
                hv_hv_RowL1 = hv_hv_RowC - ((hv_stLength * 2) * (((-hv_phis)).TupleSin()));
                hv_hv_ColL2 = hv_hv_ColC + ((hv_stLength * 2) * (((-hv_phis)).TupleCos()));
                hv_hv_ColL1 = hv_hv_ColC - ((hv_stLength * 2) * (((-hv_phis)).TupleCos()));
                ho_Arrow.Dispose();
                Vision.Gen_arrow_contour_xld(out ho_Arrow, hv_hv_RowL1, hv_hv_ColL1, hv_hv_RowL2,
                    hv_hv_ColL2, 50, 10);
            }
            else if ((int)(new HTuple(hv_tepy.TupleEqual("成对正"))) != 0)
            {
                hv_hv_RowL2 = hv_hv_RowC + ((hv_stLength * 2) * (((-hv_phis)).TupleSin()));
                hv_hv_RowL1 = hv_hv_RowC - ((hv_stLength * 2) * (((-hv_phis)).TupleSin()));
                hv_hv_ColL2 = hv_hv_ColC + ((hv_stLength * 2) * (((-hv_phis)).TupleCos()));
                hv_hv_ColL1 = hv_hv_ColC - ((hv_stLength * 2) * (((-hv_phis)).TupleCos()));
                ho_Arrow.Dispose();
                Vision.Gen_arrow_contour_xld(out ho_Arrow, hv_hv_RowL1, hv_hv_ColL1, hv_hv_RowL2,
                    hv_hv_ColL2, 50, 10);
            }
            else if ((int)(new HTuple(hv_tepy.TupleEqual("成对反"))) != 0)
            {
                hv_phi = (new HTuple(90)).TupleRad();
                hv_phis = (hv_angles1.TupleSelect(0)) + hv_phi;
                hv_hv_RowL2 = hv_hv_RowC + ((hv_stLength * 2) * (((-hv_phis)).TupleSin()));
                hv_hv_RowL1 = hv_hv_RowC - ((hv_stLength * 2) * (((-hv_phis)).TupleSin()));
                hv_hv_ColL2 = hv_hv_ColC + ((hv_stLength * 2) * (((-hv_phis)).TupleCos()));
                hv_hv_ColL1 = hv_hv_ColC - ((hv_stLength * 2) * (((-hv_phis)).TupleCos()));
                ho_Arrow.Dispose();
                Vision.Gen_arrow_contour_xld(out ho_Arrow, hv_hv_RowL1, hv_hv_ColL1, hv_hv_RowL2,
                    hv_hv_ColL2, 50, 10);
            }
            else if ((int)(new HTuple(hv_tepy.TupleEqual("成对内"))) != 0)
            {
                hv_phis = (hv_angles1.TupleSelect(0)) + hv_phi;
                hv_hv_RowL2 = hv_hv_RowC + ((hv_stLength / 2) * (((-hv_phis)).TupleSin()));
                hv_hv_RowL1 = hv_hv_RowC - ((hv_stLength / 2) * (((-hv_phis)).TupleSin()));
                hv_hv_ColL2 = hv_hv_ColC + ((hv_stLength / 2) * (((-hv_phis)).TupleCos()));
                hv_hv_ColL1 = hv_hv_ColC - ((hv_stLength / 2) * (((-hv_phis)).TupleCos()));
                hv_hv_RowL4 = hv_hv_RowC + (hv_stLength * (((-hv_phis)).TupleSin()));
                hv_hv_RowL3 = hv_hv_RowC - (hv_stLength * (((-hv_phis)).TupleSin()));
                hv_hv_ColL4 = hv_hv_ColC + (hv_stLength * (((-hv_phis)).TupleCos()));
                hv_hv_ColL3 = hv_hv_ColC - (hv_stLength * (((-hv_phis)).TupleCos()));
                hv_phi = (new HTuple(-90)).TupleRad();
                ho_Arrow.Dispose();
                Vision.Gen_arrow_contour_xld(out ho_Arrow, hv_hv_RowL3, hv_hv_ColL3, hv_hv_RowC, hv_hv_ColC,
                    50, 10);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_XLD1, ho_Arrow, out ExpTmpOutVar_0);
                    ho_XLD1.Dispose();
                    ho_XLD1 = ExpTmpOutVar_0;
                }
                ho_Arrow.Dispose();
                Vision.Gen_arrow_contour_xld(out ho_Arrow, hv_hv_RowL4, hv_hv_ColL4, hv_hv_RowC, hv_hv_ColC,
                    50, 10);
            }
            else if ((int)(new HTuple(hv_tepy.TupleEqual("成对外"))) != 0)
            {
                hv_hv_RowL2 = hv_hv_RowC + ((hv_stLength / 2) * (((-hv_phis)).TupleSin()));
                hv_hv_RowL1 = hv_hv_RowC - ((hv_stLength / 2) * (((-hv_phis)).TupleSin()));
                hv_hv_ColL2 = hv_hv_ColC + ((hv_stLength / 2) * (((-hv_phis)).TupleCos()));
                hv_hv_ColL1 = hv_hv_ColC - ((hv_stLength / 2) * (((-hv_phis)).TupleCos()));
                hv_hv_RowL4 = hv_hv_RowC + (hv_stLength * (((-hv_phis)).TupleSin()));
                hv_hv_RowL3 = hv_hv_RowC - (hv_stLength * (((-hv_phis)).TupleSin()));
                hv_hv_ColL4 = hv_hv_ColC + (hv_stLength * (((-hv_phis)).TupleCos()));
                hv_hv_ColL3 = hv_hv_ColC - (hv_stLength * (((-hv_phis)).TupleCos()));
                hv_phi = (new HTuple(90)).TupleRad();
                ho_Arrow.Dispose();
                Vision.Gen_arrow_contour_xld(out ho_Arrow, hv_hv_RowC, hv_hv_ColC, hv_hv_RowL3, hv_hv_ColL3,
                    50, 10);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_XLD1, ho_Arrow, out ExpTmpOutVar_0);
                    ho_XLD1.Dispose();
                    ho_XLD1 = ExpTmpOutVar_0;
                }
                ho_Arrow.Dispose();
                Vision.Gen_arrow_contour_xld(out ho_Arrow, hv_hv_RowC, hv_hv_ColC, hv_hv_RowL4, hv_hv_ColL4,
                    50, 10);
            }
            hv_rows = hv_hv_RowL1.Clone();
            hv_cols = hv_hv_ColL1.Clone();
            hv_rows2 = hv_hv_RowL2.Clone();
            hv_cols2 = hv_hv_ColL2.Clone();
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_XLD1, ho_Arrow, out ExpTmpOutVar_0);
                ho_XLD1.Dispose();
                ho_XLD1 = ExpTmpOutVar_0;
            }

            HTuple end_val90 = (new HTuple(hv_Row5.TupleLength()
                )) - 1;
            HTuple step_val90 = hv_i;
            for (hv_Index1 = 0; hv_Index1.Continue(end_val90, step_val90); hv_Index1 = hv_Index1.TupleAdd(step_val90))
            {
                hv_i = hv_i + 1;
                HOperatorSet.DistancePp(hv_row1, hv_col1, hv_Row5.TupleSelect(hv_Index1), hv_Cols5.TupleSelect(
                    hv_Index1), out hv_Distance);
                if ((int)((new HTuple((new HTuple(hv_Distance.TupleGreaterEqual(hv_Length))).TupleOr(
                    new HTuple(hv_Index1.TupleEqual(0))))).TupleOr(new HTuple(hv_Index1.TupleEqual(
                    (new HTuple(hv_Row5.TupleLength())) - 1)))) != 0)
                {
                    hv_i = hv_i.Clone();
                    hv_hv_RowL2 = hv_row1 + (hv_Length * (((-(hv_angles1.TupleSelect(hv_Index1)))).TupleSin()
                        ));
                    hv_hv_ColL2 = hv_col1 + (hv_Length * (((-(hv_angles1.TupleSelect(hv_Index1)))).TupleCos()
                        ));
                    ho_ContEllipse.Dispose();
                    HOperatorSet.GenEllipseContourXld(out ho_ContEllipse, hv_row1, hv_col1, (hv_angles1.TupleSelect(
                        hv_Index1)) - ((new HTuple(90)).TupleRad()), hv_Length, hv_Length, (new HTuple(0)).TupleRad(), (new HTuple(180)).TupleRad()
                        , "positive", 1.5);
                    HOperatorSet.IntersectionContoursXld(ho_ContEllipse, ho_xld, "all", out hv_Row,
                        out hv_Column, out hv_IsOverlapping);
                    if ((int)(new HTuple(hv_Index1.TupleEqual((new HTuple(hv_Row5.TupleLength()
                        )) - 1))) != 0)
                    {
                        hv_Row = hv_hv_RowL2.Clone();
                        hv_Column = hv_hv_ColL2.Clone();
                    }
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 60, hv_angles1.TupleSelect(
                        hv_Index1));
                    if ((int)(new HTuple(hv_tepy.TupleEqual("成对外"))) != 0)
                    {
                        hv_hv_RowL2 = hv_Row + ((hv_stLength / 2) * ((((-(hv_angles1.TupleSelect(hv_Index1))) + hv_phi)).TupleSin()
                            ));
                        hv_hv_ColL2 = hv_Column + ((hv_stLength / 2) * ((((-(hv_angles1.TupleSelect(hv_Index1))) + hv_phi)).TupleCos()
                            ));
                        hv_hv_RowL3 = hv_Row - ((hv_stLength / 2) * ((((-(hv_angles1.TupleSelect(hv_Index1))) + hv_phi)).TupleSin()
                            ));
                        hv_hv_ColL3 = hv_Column - ((hv_stLength / 2) * ((((-(hv_angles1.TupleSelect(hv_Index1))) + hv_phi)).TupleCos()
                            ));

                    }
                    else if ((int)(new HTuple(hv_tepy.TupleEqual("成对内"))) != 0)
                    {
                        hv_hv_RowL2 = hv_Row + ((hv_stLength / 2) * ((((-(hv_angles1.TupleSelect(hv_Index1))) + hv_phi)).TupleSin()
                            ));
                        hv_hv_ColL2 = hv_Column + ((hv_stLength / 2) * ((((-(hv_angles1.TupleSelect(hv_Index1))) + hv_phi)).TupleCos()
                            ));
                        hv_hv_RowL3 = hv_Row - ((hv_stLength / 2) * ((((-(hv_angles1.TupleSelect(hv_Index1))) + hv_phi)).TupleSin()
                            ));
                        hv_hv_ColL3 = hv_Column - ((hv_stLength / 2) * ((((-(hv_angles1.TupleSelect(hv_Index1))) + hv_phi)).TupleCos()
                            ));
                    }
                    else
                    {
                        ho_Cross.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 60, hv_angles1.TupleSelect(
                            hv_Index1));
                    }

                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_hv_RowL3, hv_hv_ColL3, 60,
                        hv_angles1.TupleSelect(hv_Index1));
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_hv_RowL2, hv_hv_ColL2, 60,
                        hv_angles1.TupleSelect(hv_Index1));
                    hv_rows = hv_rows.TupleConcat(hv_hv_RowL2);
                    hv_cols = hv_cols.TupleConcat(hv_hv_ColL2);
                    hv_rows2 = hv_rows2.TupleConcat(hv_hv_RowL3);
                    hv_cols2 = hv_cols2.TupleConcat(hv_hv_ColL3);
                    hv_row1 = hv_Row.Clone();
                    hv_col1 = hv_Column.Clone();
                    hv_phis = hv_phis.TupleConcat((hv_angles1.TupleSelect(hv_Index1)) + hv_phi);
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle2(out ho_Rectangle, hv_hv_RowL2, hv_hv_ColL2, (hv_angles1.TupleSelect(
                        hv_Index1)) + hv_phi, hv_Weight, hv_Height);
                    ho_Rectangle2.Dispose();
                    HOperatorSet.GenRectangle2(out ho_Rectangle2, hv_hv_RowL3, hv_hv_ColL3, (hv_angles1.TupleSelect(
                        hv_Index1)) + hv_phi, hv_Weight, hv_Height);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_XLD1, ho_Rectangle, out ExpTmpOutVar_0);
                        ho_XLD1.Dispose();
                        ho_XLD1 = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_XLD1, ho_Rectangle2, out ExpTmpOutVar_0);
                        ho_XLD1.Dispose();
                        ho_XLD1 = ExpTmpOutVar_0;
                    }
                }
            }

            hv_phis = hv_phis.TupleConcat((hv_angles1.TupleSelect((new HTuple(hv_angles1.TupleLength()
                )) - 1)) + hv_phi);
            ho_Arrow.Dispose();
            ho_ContEllipse.Dispose();
            ho_Cross.Dispose();
            ho_Rectangle.Dispose();
            ho_Rectangle2.Dispose();

            return;
        }

        /// <summary>
        /// 测量自由曲线
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="ho_Regions2">测量结果XLD</param>
        /// <param name="hv_DetectWidrh">测量宽度</param>
        /// <param name="hv_DetectHeight">测量高度</param>
        /// <param name="interpolation">测量差值类型</param>
        /// <param name="hv_Sigma">平滑</param>
        /// <param name="hv_Threshold">阈值</param>
        /// <param name="hv_Select">筛选</param>
        /// <param name="hv_Transition">方向</param>
        /// <param name="hv_ROIRows">测量中点Rows</param>
        /// <param name="hv_ROICols">测量中点cols</param>
        /// <param name="hv_phis">测量角度方向</param>
        /// <param name="hv_ResultRow1">起点rows</param>
        /// <param name="hv_ResultColumn1">起点cols</param>
        /// <param name="hv_ResultRow2">终点rows</param>
        /// <param name="hv_ResultColumn2">终点cols</param>
        /// <param name="hv_distances">起点与终点距离</param>
        //public void Measuring_Arc(HObject ho_Image, out HObject ho_Regions2, HTuple hv_DetectWidrh,
        //    HTuple hv_DetectHeight,HTuple interpolation, HTuple hv_Sigma, HTuple hv_Threshold, HTuple hv_Select,
        //    HTuple hv_Transition, HTuple hv_ROIRows, HTuple hv_ROICols, HTuple hv_phis,
        //    out HTuple hv_ResultRow1, out HTuple hv_ResultColumn1, out HTuple hv_ResultRow2,
        //    out HTuple hv_ResultColumn2, out HTuple hv_distances)
        //{
        //    HTuple sigma, threshold, select, transition;
        //    sigma = hv_Sigma;
        //    if (sigma.Length==1)
        //    {
        //        sigma.Append(hv_Sigma);
        //    }
        //    threshold = hv_Threshold;
        //    if (threshold.Length == 1)
        //    {
        //        threshold.Append(threshold);
        //    }
        //    select = hv_Select;
        //    if (select.Length == 1)
        //    {
        //        if (select == "first")
        //        {
        //            select.Append(Select.last.ToString());
        //        }
        //        else if (select == "last")
        //        {
        //            select.Append(Select.first.ToString());
        //        }
        //        else
        //        {
        //            select.Append(select);
        //        }

        //    }
        //    transition = hv_Transition;
        //    if (transition.Length == 1)
        //    {
        //        if (hv_Transition== "negative")
        //        {
        //            transition.Append(Transition.positive.ToString());
        //        }
        //        else if(hv_Transition == "positive")
        //        {
        //            transition.Append(Transition.negative.ToString());
        //        }
        //        else
        //        {
        //            transition.Append(transition);
        //        }
        //    }
        //    // Local iconic variables 

        //    HObject  ho_Contour = null;
        //    // Local control variables 
        //    HTuple hv_Width = null, hv_Height = null, hv_Index1 = null;
        //    HTuple hv_MeasureHandle = new HTuple(), hv_RowEdge1 = new HTuple();
        //    HTuple hv_ColumnEdge1 = new HTuple(), hv_Amplitude = new HTuple();
        //    HTuple hv_Distance1 = new HTuple(), hv_RowEdge = new HTuple();
        //    HTuple hv_ColumnEdge = new HTuple(), hv_max1 = new HTuple();
        //    HTuple hv_Indices = new HTuple();
        //    // Initialize local and output iconic variables 
        //    HOperatorSet.GenEmptyObj(out ho_Regions2);
        //    HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
        //    hv_ResultRow1 = new HTuple();
        //    hv_ResultColumn1 = new HTuple();
        //    hv_ResultRow2 = new HTuple();
        //    hv_ResultColumn2 = new HTuple();
        //    hv_distances = new HTuple();
        //    for (hv_Index1 = 0; (int)hv_Index1 <= (int)((new HTuple(hv_ROIRows.TupleLength())) - 1); hv_Index1 = (int)hv_Index1 + 1)
        //    {
        //        HOperatorSet.GenMeasureRectangle2(hv_ROIRows.TupleSelect(hv_Index1), hv_ROICols.TupleSelect(
        //            hv_Index1), hv_phis.TupleSelect(hv_Index1), hv_DetectHeight, hv_DetectWidrh,
        //            hv_Width, hv_Height, interpolation, out hv_MeasureHandle);
        //        HOperatorSet.MeasurePos(ho_Image, hv_MeasureHandle, sigma[0], threshold[0], transition[0],
        //            select[0], out hv_RowEdge1, out hv_ColumnEdge1, out hv_Amplitude, out hv_Distance1);
        //        if ((int)(new HTuple((new HTuple(hv_Amplitude.TupleLength())).TupleEqual(0))) != 0)
        //        {
        //            continue;
        //        }
        //        int d= hv_Amplitude.TupleFind(((hv_Amplitude)).TupleMax());
        //        hv_RowEdge1 = hv_RowEdge1[d];
        //        hv_ColumnEdge1 = hv_ColumnEdge1[d];
        //        HOperatorSet.MeasurePos(ho_Image, hv_MeasureHandle, sigma[1], threshold[1], transition[1],
        //            select[1], out hv_RowEdge, out hv_ColumnEdge, out hv_Amplitude, out hv_Distance1);
        //        if ((int)(new HTuple((new HTuple(hv_Amplitude.TupleLength())).TupleGreater(
        //            0))) != 0)
        //        {
        //            hv_max1 = ((hv_Amplitude)).TupleMax();
        //            HOperatorSet.TupleFind(hv_Amplitude, hv_max1, out hv_Indices);
        //            hv_ResultRow1 = hv_ResultRow1.TupleConcat(hv_RowEdge1);
        //            hv_ResultColumn1 = hv_ResultColumn1.TupleConcat(hv_ColumnEdge1);
        //            hv_ResultRow2 = hv_ResultRow2.TupleConcat(hv_RowEdge.TupleSelect(hv_Indices));
        //            hv_ResultColumn2 = hv_ResultColumn2.TupleConcat(hv_ColumnEdge.TupleSelect(
        //                hv_Indices));
        //            HOperatorSet.DistancePp(hv_RowEdge1, hv_ColumnEdge1, hv_RowEdge.TupleSelect(
        //                hv_Indices), hv_ColumnEdge.TupleSelect(hv_Indices), out hv_Distance1);
        //            hv_distances = hv_distances.TupleConcat(hv_Distance1);
        //            HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_RowEdge1.TupleConcat(hv_RowEdge.TupleSelect(
        //            hv_Indices)), hv_ColumnEdge1.TupleConcat(hv_ColumnEdge.TupleSelect(hv_Indices)));
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_Regions2, ho_Contour, out ExpTmpOutVar_0);
        //                ho_Regions2.Dispose();
        //                ho_Regions2 = ExpTmpOutVar_0;
        //            }
        //        }
        //        HOperatorSet.CloseMeasure(hv_MeasureHandle);
        //    }

        //    ho_Contour.Dispose();
        //    return;
        //}
        /// <summary>
        /// 测量自由曲线
        /// </summary>
        /// <param name="ho_Image">图像</param>
        /// <param name="ho_Regions2">输出区域</param>
        /// <param name="hv_DetectWidrh">测量宽度</param>
        /// <param name="hv_DetectHeight">测量高度</param>
        /// <param name="hv_tepy">测量类型</param>
        /// <param name="hv_Interpolation">差值</param>
        /// <param name="hv_Sigma">平滑</param>
        /// <param name="hv_Threshold">阈值</param>
        /// <param name="hv_Select">筛选</param>
        /// <param name="hv_Transition">方向</param>
        /// <param name="hv_ROIRows">测量点1rows</param>
        /// <param name="hv_ROICols">测量点1cols</param>
        /// <param name="hv_phis">测量点1角度</param>
        /// <param name="hv_roiRows2">测量点2rows</param>
        /// <param name="hv_roiCols2">测量点2c</param>
        /// <param name="hv_ResultRow1">输出测量点1</param>
        /// <param name="hv_ResultColumn1">输出测量点1</param>
        /// <param name="hv_ResultRow2">输出测量点2</param>
        /// <param name="hv_ResultColumn2">输出测量点2</param>
        /// <param name="hv_distances">输出2点距离</param>
        public void Measuring_Arc(HObject ho_Image, out HObject ho_Regions2, HTuple hv_DetectWidrh,
            HTuple hv_DetectHeight, HTuple hv_tepy, HTuple hv_Interpolation, HTuple hv_Sigma,
            HTuple hv_Threshold, HTuple hv_Select, HTuple hv_Transition, HTuple hv_ROIRows,
            HTuple hv_ROICols, HTuple hv_phis, HTuple hv_roiRows2, HTuple hv_roiCols2, out HTuple hv_ResultRow1,
            out HTuple hv_ResultColumn1, out HTuple hv_ResultRow2, out HTuple hv_ResultColumn2,
            out HTuple hv_distances)
        {
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];
            // Local iconic variables 
            HObject ho_Contour = null;
            // Local control variables 
            HTuple hv_Width = null, hv_Height = null, hv_Index1 = null;
            HTuple hv_MeasureHandle = new HTuple(), hv_RowEdge1 = new HTuple();
            HTuple hv_ColumnEdge1 = new HTuple(), hv_Amplitude = new HTuple();
            HTuple hv_Distance1 = new HTuple(), hv_max1 = new HTuple();
            HTuple hv_Indices = new HTuple(), hv_MeasureHandle2 = new HTuple();
            HTuple hv_RowEdge = new HTuple(), hv_ColumnEdge = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Regions2);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            ho_Regions2.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Regions2);

            hv_ResultRow1 = HTuple.TupleGenConst(drawCols.Length, 0);
            hv_ResultColumn1 = HTuple.TupleGenConst(drawCols.Length, 0);
            hv_ResultRow2 = HTuple.TupleGenConst(drawCols.Length, 0);
            hv_ResultColumn2 = HTuple.TupleGenConst(drawCols.Length, 0);
            hv_distances = HTuple.TupleGenConst(drawCols.Length, 0);
            try
            {
                for (hv_Index1 = 0; (int)hv_Index1 <= (int)((new HTuple(hv_ROIRows.TupleLength())) - 1); hv_Index1 = (int)hv_Index1 + 1)
                {
                    HOperatorSet.GenMeasureRectangle2(hv_ROIRows.TupleSelect(hv_Index1), hv_ROICols.TupleSelect(
                        hv_Index1), hv_phis.TupleSelect(hv_Index1), hv_DetectHeight, hv_DetectWidrh,
                        hv_Width, hv_Height, hv_Interpolation, out hv_MeasureHandle);
                    //gen_rectangle2 (Rectangle2, ROIRows[Index1], ROICols[Index1], phis[Index1], DetectHeight, DetectWidrh)
                    HOperatorSet.MeasurePos(ho_Image, hv_MeasureHandle, hv_Sigma.TupleSelect(0),
                        hv_Threshold.TupleSelect(0), hv_Transition.TupleSelect(0), hv_Select.TupleSelect(
                        0), out hv_RowEdge1, out hv_ColumnEdge1, out hv_Amplitude, out hv_Distance1);
                    HOperatorSet.CloseMeasure(hv_MeasureHandle);
                    if ((int)(new HTuple((new HTuple(hv_Amplitude.TupleLength())).TupleEqual(0))) == 0)
                    {
                        hv_max1 = hv_Amplitude.TupleMax();
                        HOperatorSet.TupleFind(hv_Amplitude, hv_max1, out hv_Indices);
                        hv_RowEdge1 = hv_RowEdge1.TupleSelect(hv_Indices);
                        hv_ColumnEdge1 = hv_ColumnEdge1.TupleSelect(hv_Indices);
                        hv_ResultRow1[hv_Index1] = hv_RowEdge1;
                        hv_ResultColumn1[hv_Index1] = hv_ColumnEdge1;
                    }
                    HOperatorSet.GenMeasureRectangle2(hv_roiRows2.TupleSelect(hv_Index1), hv_roiCols2.TupleSelect(
                        hv_Index1), hv_phis.TupleSelect(hv_Index1) + new HTuple(180).TupleRad(), hv_DetectHeight, hv_DetectWidrh,
                        hv_Width, hv_Height, hv_Interpolation, out hv_MeasureHandle2);
                    //gen_rectangle2 (Rectangle, roiRows2[Index1], roiCols2[Index1], phis[Index1], DetectHeight, DetectWidrh)
                    HOperatorSet.MeasurePos(ho_Image, hv_MeasureHandle2, hv_Sigma.TupleSelect(1),
                        hv_Threshold.TupleSelect(1), hv_Transition.TupleSelect(1), hv_Select.TupleSelect(
                        1), out hv_RowEdge, out hv_ColumnEdge, out hv_Amplitude, out hv_Distance1);
                    HOperatorSet.CloseMeasure(hv_MeasureHandle2);
                    if ((int)(new HTuple((new HTuple(hv_Amplitude.TupleLength())).TupleEqual(0))) != 0)
                    {
                        continue;
                    }
                    hv_max1 = hv_Amplitude.TupleMax();
                    HOperatorSet.TupleFind(hv_Amplitude, hv_max1, out hv_Indices);
                    hv_ResultRow2[hv_Index1] = hv_RowEdge.TupleSelect(hv_Indices);
                    hv_ResultColumn2[hv_Index1] = hv_ColumnEdge.TupleSelect(hv_Indices);
                    if (hv_ResultRow1[hv_Index1].D != 0)
                    {
                        HOperatorSet.DistancePp(hv_RowEdge1, hv_ColumnEdge1, hv_RowEdge.TupleSelect(
                    hv_Indices), hv_ColumnEdge.TupleSelect(hv_Indices), out hv_Distance1);
                        hv_distances[hv_Index1] = hv_Distance1;

                        ho_Contour.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_RowEdge1.TupleConcat(hv_RowEdge.TupleSelect(
                            hv_Indices)), hv_ColumnEdge1.TupleConcat(hv_ColumnEdge.TupleSelect(hv_Indices)));

                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_Regions2, ho_Contour, out ExpTmpOutVar_0);
                        ho_Regions2.Dispose();
                        ho_Regions2 = ExpTmpOutVar_0;
                    }
                }

            }
            catch (Exception ex)
            {

                //       this.LogErr(ex);
            }

            ho_Contour.Dispose();

            return;
        }
        HObject measuHobj;

        #region New Measuring_Arc and Measuring_Arc
        /// <summary>
        /// 绘制测量矩形
        /// </summary>
        /// <param name="ho_SegmentationContour">测量轨迹Contour</param>
        /// <param name="ho_SegmentationCircles">分割圆Contour</param>
        /// <param name="ho_SegmentationCross">分割点Cross</param>
        /// <param name="ho_LeftCross">左侧测量矩形中点Cross</param>
        /// <param name="ho_RightCross">右侧测量矩形中点Cross</param>
        /// <param name="ho_LeftRectangles">左侧测量矩形</param>
        /// <param name="ho_RightRectangles">右侧测量矩形</param>
        /// <param name="hv_SegmentationDistance">分割距离</param>
        /// <param name="hv_ControlPointDistance">测量矩形中点到测量轨迹垂直间距</param>
        /// <param name="hv_MeasureRectangleWidth">测量矩形宽</param>
        /// <param name="hv_MeasureRectangleHeight">测量矩形高</param>
        /// <param name="hv_SegmentatioRows">分割点行坐标</param>
        /// <param name="hv_SegmentatioColumns">分割点列坐标</param>
        /// <param name="hv_LeftControlPointRs">左侧测量矩形中点行坐标</param>
        /// <param name="hv_LeftControlPointCs">左侧测量矩形中点列坐标</param>
        /// <param name="hv_RightControlPointRs">右侧测量矩形中点行坐标</param>
        /// <param name="hv_RightControlPointCs">右侧测量矩形中点列坐标</param>
        /// <param name="hv_LeftMrasureAngles">左侧测量矩形测量角度</param>
        /// <param name="hv_RightMrasureAngles">右侧测量矩形测量角度</param>
        public void Darw_Measuring_Arc(HObject ho_SegmentationContour, out HObject ho_SegmentationCircles,
          out HObject ho_SegmentationCross, out HObject ho_LeftCross, out HObject ho_RightCross,
          out HObject ho_LeftRectangles, out HObject ho_RightRectangles, HTuple hv_SegmentationDistance,
          HTuple hv_ControlPointDistance, HTuple hv_MeasureRectangleWidth, HTuple hv_MeasureRectangleHeight,
          out HTuple hv_SegmentatioRows, out HTuple hv_SegmentatioColumns, out HTuple hv_LeftControlPointRs,
          out HTuple hv_LeftControlPointCs, out HTuple hv_RightControlPointRs, out HTuple hv_RightControlPointCs,
          out HTuple hv_LeftMrasureAngles, out HTuple hv_RightMrasureAngles)
        {
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_circleXLD = null, ho__LeftCross = null;
            HObject ho__RightCros = null, ho__LeftRectangle = null, ho__RightRectangle = null;

            // Local control variables 

            HTuple hv_Row = null, hv_Col = null, hv_Row1 = null;
            HTuple hv_Column1 = null, hv_oldR = null, hv_oldC = null;
            HTuple hv_circleR = null, hv_circleC = null, hv_ControlPointsR = null;
            HTuple hv_ControlPointsC = null, hv_FailCount = null, hv__oldR = new HTuple();
            HTuple hv__oldC = new HTuple(), hv_Index1 = new HTuple();
            HTuple hv_PointDistance = new HTuple(), hv_BeginRow = null;
            HTuple hv_BeginColumn = null, hv_EndRow = null, hv_EndColumn = null;
            HTuple hv_RowA1 = null, hv_ColumnA1 = null, hv_RowA2 = null;
            HTuple hv_ColumnA2 = null, hv_Angles = null, hv_LeftControlInMeasureR = null;
            HTuple hv_LeftControlInMeasureC = null, hv_RightControlInMeasureR = null;
            HTuple hv_RightControlInMeasureC = null, hv_Index = null;
            HTuple hv_HomMat2DIdentity = new HTuple(), hv_HomMat2DRotate = new HTuple();
            HTuple hv_Pixel_H_Measure = new HTuple(), hv_LeftControlPointR = new HTuple();
            HTuple hv_LeftControlPointC = new HTuple(), hv_RightControlPointR = new HTuple();
            HTuple hv_RightControlPointC = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_SegmentationCircles);
            HOperatorSet.GenEmptyObj(out ho_SegmentationCross);
            HOperatorSet.GenEmptyObj(out ho_LeftCross);
            HOperatorSet.GenEmptyObj(out ho_RightCross);
            HOperatorSet.GenEmptyObj(out ho_LeftRectangles);
            HOperatorSet.GenEmptyObj(out ho_RightRectangles);
            HOperatorSet.GenEmptyObj(out ho_circleXLD);
            HOperatorSet.GenEmptyObj(out ho__LeftCross);
            HOperatorSet.GenEmptyObj(out ho__RightCros);
            HOperatorSet.GenEmptyObj(out ho__LeftRectangle);
            HOperatorSet.GenEmptyObj(out ho__RightRectangle);
            HOperatorSet.GetContourXld(ho_SegmentationContour, out hv_Row, out hv_Col);
            HOperatorSet.IntersectionCircleContourXld(ho_SegmentationContour, hv_Row.TupleSelect(
                0), hv_Col.TupleSelect(0), hv_SegmentationDistance, 0, 6.28318, "positive",
                out hv_Row1, out hv_Column1);
            hv_oldR = hv_Row.TupleSelect(0);
            hv_oldC = hv_Col.TupleSelect(0);
            hv_circleR = hv_Row1.TupleSelect(0);
            hv_circleC = hv_Column1.TupleSelect(0);
            hv_ControlPointsR = new HTuple();
            hv_ControlPointsR = hv_ControlPointsR.TupleConcat(hv_oldR);
            hv_ControlPointsR = hv_ControlPointsR.TupleConcat(hv_circleR);
            hv_ControlPointsC = new HTuple();
            hv_ControlPointsC = hv_ControlPointsC.TupleConcat(hv_oldC);
            hv_ControlPointsC = hv_ControlPointsC.TupleConcat(hv_circleC);
            hv_FailCount = 0;
            ho_SegmentationCircles.Dispose();
            HOperatorSet.GenEmptyObj(out ho_SegmentationCircles);
            while ((int)(new HTuple(hv_FailCount.TupleLess(2))) != 0)
            {
                ho_circleXLD.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_circleXLD, hv_circleR, hv_circleC,
                    hv_SegmentationDistance, 0, 6.28318, "positive", 1);
                HOperatorSet.IntersectionCircleContourXld(ho_SegmentationContour, hv_circleR,
                    hv_circleC, hv_SegmentationDistance, 0, 6.28318, "positive", out hv_Row1,
                    out hv_Column1);
                hv__oldR = hv_circleR.Clone();
                hv__oldC = hv_circleC.Clone();
                //截取距离符合条件的控制点
                if ((int)(new HTuple((new HTuple(hv_Row1.TupleLength())).TupleEqual(1))) != 0)
                {
                    hv_FailCount = hv_FailCount + 1;
                }
                for (hv_Index1 = 0; (int)hv_Index1 <= (int)((new HTuple(hv_Row1.TupleLength())) - 1); hv_Index1 = (int)hv_Index1 + 1)
                {
                    //交点与上一个控制点的距离
                    HOperatorSet.DistancePp(hv_Row1.TupleSelect(hv_Index1), hv_Column1.TupleSelect(
                        hv_Index1), hv_oldR, hv_oldC, out hv_PointDistance);
                    if ((int)(new HTuple(hv_PointDistance.TupleGreater(hv_SegmentationDistance))) != 0)
                    {
                        hv_circleR = hv_Row1.TupleSelect(hv_Index1);
                        hv_circleC = hv_Column1.TupleSelect(hv_Index1);
                        hv_ControlPointsR = hv_ControlPointsR.TupleConcat(hv_Row1.TupleSelect(hv_Index1));
                        hv_ControlPointsC = hv_ControlPointsC.TupleConcat(hv_Column1.TupleSelect(
                            hv_Index1));
                        hv_FailCount = 0;
                    }
                    if ((int)(new HTuple(hv_PointDistance.TupleLess(hv_SegmentationDistance))) != 0)
                    {
                        hv_FailCount = hv_FailCount + 1;
                    }
                }
                hv_oldR = hv__oldR.Clone();
                hv_oldC = hv__oldC.Clone();
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_SegmentationCircles, ho_circleXLD, out ExpTmpOutVar_0
                        );
                    ho_SegmentationCircles.Dispose();
                    ho_SegmentationCircles = ExpTmpOutVar_0;
                }
            }
            ho_SegmentationCross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_SegmentationCross, hv_ControlPointsR,
                hv_ControlPointsC, 10, 0.785398);
            ///输出结果
            hv_SegmentatioRows = hv_ControlPointsR.Clone();
            hv_SegmentatioColumns = hv_ControlPointsC.Clone();
            //轮廓起点
            HOperatorSet.TupleRemove(hv_SegmentatioRows, (new HTuple(hv_SegmentatioRows.TupleLength()
                )) - 1, out hv_BeginRow);
            HOperatorSet.TupleRemove(hv_SegmentatioColumns, (new HTuple(hv_SegmentatioColumns.TupleLength()
                )) - 1, out hv_BeginColumn);
            //轮廓终点
            HOperatorSet.TupleRemove(hv_SegmentatioRows, 0, out hv_EndRow);
            HOperatorSet.TupleRemove(hv_SegmentatioColumns, 0, out hv_EndColumn);
            //计算每段线段角度
            HOperatorSet.TupleGenConst(new HTuple(hv_BeginRow.TupleLength()), 0, out hv_RowA1);
            HOperatorSet.TupleGenConst(new HTuple(hv_BeginRow.TupleLength()), 0, out hv_ColumnA1);
            HOperatorSet.TupleGenConst(new HTuple(hv_BeginRow.TupleLength()), 0, out hv_RowA2);
            HOperatorSet.TupleGenConst(new HTuple(hv_BeginRow.TupleLength()), 100, out hv_ColumnA2);
            HOperatorSet.AngleLl(hv_RowA1, hv_ColumnA1, hv_RowA2, hv_ColumnA2, hv_BeginRow,
                hv_BeginColumn, hv_EndRow, hv_EndColumn, out hv_Angles);
            //左侧控制点
            hv_LeftControlPointRs = new HTuple();
            hv_LeftControlPointCs = new HTuple();
            //右侧控制点
            hv_RightControlPointRs = new HTuple();
            hv_RightControlPointCs = new HTuple();
            //左侧测量矩形角度
            hv_LeftMrasureAngles = new HTuple();
            //右侧测量矩形角度
            hv_RightMrasureAngles = new HTuple();

            //控制点在测量坐标系下的位置
            hv_LeftControlInMeasureR = -hv_ControlPointDistance;
            hv_LeftControlInMeasureC = 0;
            hv_RightControlInMeasureR = hv_ControlPointDistance.Clone();
            hv_RightControlInMeasureC = 0;

            //左侧控制点
            ho_LeftCross.Dispose();
            HOperatorSet.GenEmptyObj(out ho_LeftCross);
            //左侧矩形
            ho_LeftRectangles.Dispose();
            HOperatorSet.GenEmptyObj(out ho_LeftRectangles);
            //右侧控制点
            ho_RightCross.Dispose();
            HOperatorSet.GenEmptyObj(out ho_RightCross);
            //左侧矩形
            ho_RightRectangles.Dispose();
            HOperatorSet.GenEmptyObj(out ho_RightRectangles);
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Angles.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, hv_Angles.TupleSelect(hv_Index),
                    0, 0, out hv_HomMat2DRotate);
                HOperatorSet.HomMat2dTranslate(hv_HomMat2DRotate, hv_BeginRow.TupleSelect(hv_Index),
                    hv_BeginColumn.TupleSelect(hv_Index), out hv_Pixel_H_Measure);
                HOperatorSet.AffineTransPoint2d(hv_Pixel_H_Measure, hv_LeftControlInMeasureR,
                    hv_LeftControlInMeasureC, out hv_LeftControlPointR, out hv_LeftControlPointC);
                HOperatorSet.AffineTransPoint2d(hv_Pixel_H_Measure, hv_RightControlInMeasureR,
                    hv_RightControlInMeasureC, out hv_RightControlPointR, out hv_RightControlPointC);
                ho__LeftCross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho__LeftCross, hv_LeftControlPointR, hv_LeftControlPointC,
                    40, 0.7);
                ho__RightCros.Dispose();
                HOperatorSet.GenCrossContourXld(out ho__RightCros, hv_RightControlPointR, hv_RightControlPointC,
                    40, 0.7);

                ho__LeftRectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho__LeftRectangle, hv_LeftControlPointR, hv_LeftControlPointC,
                    (hv_Angles.TupleSelect(hv_Index)) + ((new HTuple(90)).TupleRad()), hv_MeasureRectangleWidth,
                    hv_MeasureRectangleHeight);
                ho__RightRectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho__RightRectangle, hv_RightControlPointR, hv_RightControlPointC,
                    (hv_Angles.TupleSelect(hv_Index)) - ((new HTuple(90)).TupleRad()), hv_MeasureRectangleWidth,
                    hv_MeasureRectangleHeight);

                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_LeftCross, ho__LeftCross, out ExpTmpOutVar_0);
                    ho_LeftCross.Dispose();
                    ho_LeftCross = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_RightCross, ho__RightCros, out ExpTmpOutVar_0);
                    ho_RightCross.Dispose();
                    ho_RightCross = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_LeftRectangles, ho__LeftRectangle, out ExpTmpOutVar_0
                        );
                    ho_LeftRectangles.Dispose();
                    ho_LeftRectangles = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_RightRectangles, ho__RightRectangle, out ExpTmpOutVar_0
                        );
                    ho_RightRectangles.Dispose();
                    ho_RightRectangles = ExpTmpOutVar_0;
                }

                hv_LeftControlPointRs = hv_LeftControlPointRs.TupleConcat(hv_LeftControlPointR);
                hv_LeftControlPointCs = hv_LeftControlPointCs.TupleConcat(hv_LeftControlPointC);
                hv_RightControlPointRs = hv_RightControlPointRs.TupleConcat(hv_RightControlPointR);
                hv_RightControlPointCs = hv_RightControlPointCs.TupleConcat(hv_RightControlPointC);
                hv_LeftMrasureAngles = hv_LeftMrasureAngles.TupleConcat((hv_Angles.TupleSelect(
                    hv_Index)) + ((new HTuple(90)).TupleRad()));
                hv_RightMrasureAngles = hv_RightMrasureAngles.TupleConcat((hv_Angles.TupleSelect(
                    hv_Index)) - ((new HTuple(90)).TupleRad()));

                if ((int)(new HTuple(hv_Index.TupleEqual((new HTuple(hv_Angles.TupleLength()
                    )) - 1))) != 0)
                {
                    HOperatorSet.AffineTransPoint2d(hv_Pixel_H_Measure, hv_LeftControlInMeasureR,
                        hv_LeftControlInMeasureC + hv_SegmentationDistance, out hv_LeftControlPointR,
                        out hv_LeftControlPointC);
                    HOperatorSet.AffineTransPoint2d(hv_Pixel_H_Measure, hv_RightControlInMeasureR,
                        hv_RightControlInMeasureC + hv_SegmentationDistance, out hv_RightControlPointR,
                        out hv_RightControlPointC);
                    ho__LeftCross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho__LeftCross, hv_LeftControlPointR,
                        hv_LeftControlPointC, 40, 0.7);
                    ho__RightCros.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho__RightCros, hv_RightControlPointR,
                        hv_RightControlPointC, 40, 0.7);
                    ho__LeftRectangle.Dispose();
                    HOperatorSet.GenRectangle2(out ho__LeftRectangle, hv_LeftControlPointR, hv_LeftControlPointC,
                        (hv_Angles.TupleSelect(hv_Index)) + ((new HTuple(90)).TupleRad()), hv_MeasureRectangleWidth,
                        hv_MeasureRectangleHeight);
                    ho__RightRectangle.Dispose();
                    HOperatorSet.GenRectangle2(out ho__RightRectangle, hv_RightControlPointR,
                        hv_RightControlPointC, (hv_Angles.TupleSelect(hv_Index)) - ((new HTuple(90)).TupleRad()
                        ), hv_MeasureRectangleWidth, hv_MeasureRectangleHeight);

                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_LeftCross, ho__LeftCross, out ExpTmpOutVar_0);
                        ho_LeftCross.Dispose();
                        ho_LeftCross = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_RightCross, ho__RightCros, out ExpTmpOutVar_0);
                        ho_RightCross.Dispose();
                        ho_RightCross = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_LeftRectangles, ho__LeftRectangle, out ExpTmpOutVar_0
                            );
                        ho_LeftRectangles.Dispose();
                        ho_LeftRectangles = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_RightRectangles, ho__RightRectangle, out ExpTmpOutVar_0
                            );
                        ho_RightRectangles.Dispose();
                        ho_RightRectangles = ExpTmpOutVar_0;
                    }

                    hv_LeftControlPointRs = hv_LeftControlPointRs.TupleConcat(hv_LeftControlPointR);
                    hv_LeftControlPointCs = hv_LeftControlPointCs.TupleConcat(hv_LeftControlPointC);
                    hv_RightControlPointRs = hv_RightControlPointRs.TupleConcat(hv_RightControlPointR);
                    hv_RightControlPointCs = hv_RightControlPointCs.TupleConcat(hv_RightControlPointC);
                    hv_LeftMrasureAngles = hv_LeftMrasureAngles.TupleConcat((hv_Angles.TupleSelect(
                        hv_Index)) + ((new HTuple(90)).TupleRad()));
                    hv_RightMrasureAngles = hv_RightMrasureAngles.TupleConcat((hv_Angles.TupleSelect(
                        hv_Index)) - ((new HTuple(90)).TupleRad()));
                }
            }
            ho_circleXLD.Dispose();
            ho__LeftCross.Dispose();
            ho__RightCros.Dispose();
            ho__LeftRectangle.Dispose();
            ho__RightRectangle.Dispose();

            return;
        }

        /// <summary>
        /// 计算胶宽
        /// </summary>
        /// <param name="ho_MeasureImage">测量图像</param>
        /// <param name="ho_LeftMeasurePointCross">左侧测量结果点Cross</param>
        /// <param name="ho_RightMeasurePointCross">右侧测量结果点Cross</param>
        /// <param name="ho_MeasureSegments">测量结果线段</param>
        /// <param name="hv_LeftControlPointRs">左侧测量矩形中心行坐标</param>
        /// <param name="hv_LeftControlPointCs">左侧测量矩形中心列坐标</param>
        /// <param name="hv_RightControlPointRs">右侧测量矩形中心行坐标</param>
        /// <param name="hv_RightControlPointCs">右侧测量矩形中心列坐标</param>
        /// <param name="hv_LeftMrasureAngles">左侧测量矩形角度</param>
        /// <param name="hv_RightMrasureAngles">右侧测量矩形角度</param>
        /// <param name="hv_MeasureRectangleWidth">测量矩形宽度</param>
        /// <param name="hv_MeasureRectangleHeight">测量矩形高度</param>
        /// <param name="hv_Sigmas">平滑参数</param>
        /// <param name="hv_Thresholds">阈值参数</param>
        /// <param name="hv_LeftMeasurePointRs">左侧测量结果点行坐标</param>
        /// <param name="hv_LeftMeasurePointCs">左侧测量结果点列坐标</param>
        /// <param name="hv_RightMeasurePointRs">右侧测量结果点行坐标</param>
        /// <param name="hv_RightMeasurePointCs">右侧测量结果点列坐标</param>
        /// <param name="hv_MeasureSegmentLenghts">测量线段长度</param>
        public void Measuring_Arc(HObject ho_MeasureImage, out HObject ho_LeftMeasurePointCross,
          out HObject ho_RightMeasurePointCross, out HObject ho_MeasureSegments, out HObject LeftMeasureRectangles,
          out HObject RightMeasureRectangles, HTuple hv_LeftControlPointRs,
          HTuple hv_LeftControlPointCs, HTuple hv_RightControlPointRs, HTuple hv_RightControlPointCs,
          HTuple hv_LeftMrasureAngles, HTuple hv_RightMrasureAngles, HTuple hv_MeasureRectangleWidth,
          HTuple hv_MeasureRectangleHeight, HTuple hv_Sigmas, HTuple hv_Thresholds, out HTuple hv_LeftMeasurePointRs,
          out HTuple hv_LeftMeasurePointCs, out HTuple hv_RightMeasurePointRs, out HTuple hv_RightMeasurePointCs,
          out HTuple hv_MeasureSegmentLenghts)
        {

            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Contour = null, ho_leftCross = null;
            HObject ho_rightCross = null;

            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_Index = null;
            HTuple hv_MeasureHandle_Left = new HTuple(), hv_LeftRow = new HTuple();
            HTuple hv_LeftColumn = new HTuple(), hv_LeftAmplitude = new HTuple();
            HTuple hv_LeftDistance = new HTuple(), hv_MeasureHandle_Right = new HTuple();
            HTuple hv_RightRow = new HTuple(), hv_RightColumn = new HTuple();
            HTuple hv_RightAmplitude = new HTuple(), hv_RightDistance = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_LeftMeasurePointCross);
            HOperatorSet.GenEmptyObj(out ho_RightMeasurePointCross);
            HOperatorSet.GenEmptyObj(out ho_MeasureSegments);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_leftCross);
            HOperatorSet.GenEmptyObj(out ho_rightCross);
            HOperatorSet.GenEmptyObj(out LeftMeasureRectangles);
            HOperatorSet.GenEmptyObj(out RightMeasureRectangles);
            HOperatorSet.GetImageSize(ho_MeasureImage, out hv_Width, out hv_Height);
            ho_LeftMeasurePointCross.Dispose();
            HOperatorSet.GenEmptyObj(out ho_LeftMeasurePointCross);
            ho_RightMeasurePointCross.Dispose();
            HOperatorSet.GenEmptyObj(out ho_RightMeasurePointCross);
            ho_MeasureSegments.Dispose();
            HOperatorSet.GenEmptyObj(out ho_MeasureSegments);
            hv_LeftMeasurePointRs = new HTuple();
            hv_LeftMeasurePointCs = new HTuple();
            hv_RightMeasurePointRs = new HTuple();
            hv_RightMeasurePointCs = new HTuple();
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_LeftControlPointRs.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                //左侧测量矩形
                HOperatorSet.GenMeasureRectangle2(hv_LeftControlPointRs.TupleSelect(hv_Index),
                    hv_LeftControlPointCs.TupleSelect(hv_Index), hv_LeftMrasureAngles.TupleSelect(
                    hv_Index), hv_MeasureRectangleWidth, hv_MeasureRectangleHeight, hv_Width,
                    hv_Height, this.InterpolationStr.ToString(), out hv_MeasureHandle_Left);
                HOperatorSet.MeasurePos(ho_MeasureImage, hv_MeasureHandle_Left, hv_Sigmas.TupleSelect(
                    0), hv_Thresholds.TupleSelect(0), this.TransitionStr.ToString(), this.SelectStr.ToString(), out hv_LeftRow,
                    out hv_LeftColumn, out hv_LeftAmplitude, out hv_LeftDistance);
                HOperatorSet.CloseMeasure(hv_MeasureHandle_Left);

                HOperatorSet.GenRectangle2(out HObject _LeftMeasureRectangle,
                                           hv_LeftControlPointRs.TupleSelect(hv_Index),
                                           hv_LeftControlPointCs.TupleSelect(hv_Index),
                                           hv_LeftMrasureAngles.TupleSelect(hv_Index),
                                           hv_MeasureRectangleWidth,
                                           hv_MeasureRectangleHeight);
                HOperatorSet.ConcatObj(LeftMeasureRectangles, _LeftMeasureRectangle, out LeftMeasureRectangles);
                //右侧测量矩形
                HOperatorSet.GenMeasureRectangle2(hv_RightControlPointRs.TupleSelect(hv_Index),
                    hv_RightControlPointCs.TupleSelect(hv_Index), hv_RightMrasureAngles.TupleSelect(
                    hv_Index), hv_MeasureRectangleWidth, hv_MeasureRectangleHeight, hv_Width,
                    hv_Height, this.InterpolationStr.ToString(), out hv_MeasureHandle_Right);
                HOperatorSet.MeasurePos(ho_MeasureImage, hv_MeasureHandle_Right, hv_Sigmas.TupleSelect(
                    1), hv_Thresholds.TupleSelect(1), this.TransitionStr2.ToString(), this.SelectStr2.ToString(), out hv_RightRow,
                    out hv_RightColumn, out hv_RightAmplitude, out hv_RightDistance);
                HOperatorSet.CloseMeasure(hv_MeasureHandle_Right);

                HOperatorSet.GenRectangle2(out HObject _RightMeasureRectangle,
                           hv_RightControlPointRs.TupleSelect(hv_Index),
                           hv_RightControlPointCs.TupleSelect(hv_Index),
                           hv_RightMrasureAngles.TupleSelect(hv_Index),
                           hv_MeasureRectangleWidth,
                           hv_MeasureRectangleHeight);
                HOperatorSet.ConcatObj(RightMeasureRectangles, _RightMeasureRectangle, out RightMeasureRectangles);

                if ((int)((new HTuple((new HTuple((new HTuple((new HTuple(hv_LeftRow.TupleLength()
                    )).TupleGreater(0))).TupleAnd(new HTuple((new HTuple(hv_LeftColumn.TupleLength()
                    )).TupleGreater(0))))).TupleAnd(new HTuple((new HTuple(hv_RightRow.TupleLength()
                    )).TupleGreater(0))))).TupleAnd(new HTuple((new HTuple(hv_RightColumn.TupleLength()
                    )).TupleGreater(0)))) != 0)
                {
                    int DS = 0;
                    if (hv_LeftAmplitude.Length > 1)
                    {
                        //筛选振幅带的点
                        HOperatorSet.TupleMax(hv_LeftAmplitude, out HTuple MAX);
                        HOperatorSet.TupleMin(hv_LeftAmplitude, out HTuple MIN);

                        if (MIN.TupleAbs() > MAX)
                        {
                            DS = hv_LeftAmplitude.TupleFind(MIN);
                        }
                        else
                        {
                            DS = hv_LeftAmplitude.TupleFind(MAX);
                        }
                        hv_LeftRow = hv_LeftRow.TupleSelect(DS);
                        hv_LeftColumn = hv_LeftColumn.TupleSelect(DS);
                    }
                    if (hv_RightAmplitude.Length > 1)
                    {

                        //筛选振幅带的点
                        HOperatorSet.TupleMax(hv_RightAmplitude, out HTuple MAX);
                        HOperatorSet.TupleMin(hv_RightAmplitude, out HTuple MIN);

                        if (MIN.TupleAbs() > MAX)
                        {
                            DS = hv_RightAmplitude.TupleFind(MIN);
                        }
                        else
                        {
                            DS = hv_RightAmplitude.TupleFind(MAX);
                        }
                        hv_RightRow = hv_RightRow.TupleSelect(DS);
                        hv_RightColumn = hv_RightColumn.TupleSelect(DS);
                    }
                    //tuple_find (LeftAmplitude, leftMax, Indices)
                    //tuple_select (LeftRow, Indices, LeftRow)
                    //tuple_select (LeftColumn, Indices, LeftColumn)
                    //筛选振幅带的点
                    //tuple_max (RightAmplitude, rightMax)
                    //tuple_find (RightAmplitude, rightMax, Indices)
                    //tuple_select (RightRow, Indices, RightRow)
                    //tuple_select (RightColumn, Indices, RightColumn)
                    //创建线段
                    ho_Contour.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour, ((hv_LeftRow.TupleSelect(
                        0))).TupleConcat(hv_RightRow.TupleSelect(0)), ((hv_LeftColumn.TupleSelect(
                        0))).TupleConcat(hv_RightColumn.TupleSelect(0)));
                    //创建线段起点与终点
                    ho_leftCross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_leftCross, hv_LeftRow, hv_LeftColumn,
                        40, 0.785398);
                    ho_rightCross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_rightCross, hv_RightRow, hv_RightColumn,
                        40, 0.785398);
                    //累计测量结果
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_LeftMeasurePointCross, ho_leftCross, out ExpTmpOutVar_0
                            );
                        ho_LeftMeasurePointCross.Dispose();
                        ho_LeftMeasurePointCross = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_RightMeasurePointCross, ho_rightCross, out ExpTmpOutVar_0
                            );
                        ho_RightMeasurePointCross.Dispose();
                        ho_RightMeasurePointCross = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_MeasureSegments, ho_Contour, out ExpTmpOutVar_0
                            );
                        ho_MeasureSegments.Dispose();
                        ho_MeasureSegments = ExpTmpOutVar_0;
                    }

                    hv_LeftMeasurePointRs = hv_LeftMeasurePointRs.TupleConcat(hv_LeftRow);
                    hv_LeftMeasurePointCs = hv_LeftMeasurePointCs.TupleConcat(hv_LeftColumn);
                    hv_RightMeasurePointRs = hv_RightMeasurePointRs.TupleConcat(hv_RightRow);
                    hv_RightMeasurePointCs = hv_RightMeasurePointCs.TupleConcat(hv_RightColumn);
                }
                else
                {
                    hv_LeftMeasurePointRs = hv_LeftMeasurePointRs.TupleConcat(0);
                    hv_LeftMeasurePointCs = hv_LeftMeasurePointCs.TupleConcat(0);
                    hv_RightMeasurePointRs = hv_RightMeasurePointRs.TupleConcat(0);
                    hv_RightMeasurePointCs = hv_RightMeasurePointCs.TupleConcat(0);
                }
            }
            //线段长度
            HOperatorSet.DistancePp(hv_LeftMeasurePointRs, hv_LeftMeasurePointCs, hv_RightMeasurePointRs,
                hv_RightMeasurePointCs, out hv_MeasureSegmentLenghts);
            ho_Contour.Dispose();
            ho_leftCross.Dispose();
            ho_rightCross.Dispose();
            return;
        }
        #endregion

        public void Measuring_xld(HObject image, HTuple rows1, HTuple cols1, HTuple rows2, HTuple cols2, HTuple angel, HTuple lengt1, out HTuple Mrows, out HTuple MColums,
                out HTuple Mrows2, out HTuple MColums2,
            out HTuple lengts, out HObject hObject, out HObject xdt, out HObject itmexld)
        {
            hObject = null;
            xdt = new HObject();
            xdt.GenEmptyObj();
            itmexld = new HObject();
            itmexld.GenEmptyObj();
            MColums2 = Mrows2 = lengts = Mrows = MColums = new HTuple();
            try
            {
                //HObject itmes;
                //this["拟合点Rows"] = new HTuple();
                //this["拟合点Columns"] = new HTuple();
                //xdt = new HObject();
                //this.SetDefault("SubPixMin", 31, true);
                //this.SetDefault("SubPixMax", 100, true);

                //this.SetDefault("Sigma", 9, true);
                //this.SetDefault("SelectContlengthMin", 31, true);

                //int de = image.CountObj();
                //this["Distance"] = new HTuple();
                //HOperatorSet.EdgesSubPix(image, out  hObject, "canny", this["Sigma"], this["SubPixMin"], this["SubPixMax"]);

                //HOperatorSet.SelectShapeXld(hObject, out hObject, "contlength", "and", this["SelectContlengthMin"], 200000);
                //this.SetDefault("FitClippingLength", 0, true);
                //this.SetDefault("FitLength", "auto", true);
                //this.SetDefault("MaxTangAngle",new HTuple(90).TupleDeg(), true);
                //this.SetDefault("MaxDist", 25, true);
                //this.SetDefault("MaxDistPerp", 10, true);
                //this.SetDefault("MaxOverlap", 2, true);
                ////this["MaxTangAngle"] = new HTuple(90);

                //HOperatorSet.UnionCotangentialContoursXld(hObject, out itmes, this["FitClippingLength"], this["FitLength"], 
                //    this["MaxTangAngle"].TupleRad(), this["MaxDist"], this["MaxDistPerp"], this["MaxOverlap"], "attr_forget");

                HOperatorSet.SortContoursXld(image, out hObject, "lower_left", "true", "column");

                HTuple dist = new HTuple();
                HOperatorSet.TupleGenConst(rows1.Length, 0, out Mrows2);
                HOperatorSet.TupleGenConst(rows1.Length, 0, out MColums2);
                HOperatorSet.TupleGenConst(rows1.Length, 0, out Mrows);
                HOperatorSet.TupleGenConst(rows1.Length, 0, out MColums);
                HOperatorSet.TupleGenConst(rows1.Length, 0, out lengts);
                for (int i = 0; i < rows1.Length; i++)
                {
                    try
                    {
                        HTuple rowsT1 = new HTuple();
                        HTuple rowsT2 = new HTuple();
                        HTuple colsT1 = new HTuple();
                        HTuple colsT2 = new HTuple();
                        HObject hObjectt1 = Vision.GenLine(rows1[i], cols1[i], angel[i], lengt1);
                        HObject hObjectt2 = Vision.GenLine(rows2[i], cols2[i], angel[i], lengt1);
                        HOperatorSet.GetContourXld(hObjectt1, out HTuple trows1, out HTuple tcol1);
                        HOperatorSet.GetContourXld(hObjectt2, out HTuple trows2, out HTuple tcol2);
                        itmexld = itmexld.ConcatObj(hObjectt1);
                        itmexld = itmexld.ConcatObj(hObjectt2);
                        for (int it = 1; it < hObject.CountObj() + 1; it++)
                        {
                            HOperatorSet.IntersectionSegmentContourXld(hObject.SelectObj(it), trows1[0], tcol1[0], trows1[1], tcol1[1],
                               out HTuple row, out HTuple colum, out HTuple hTuple);
                            HOperatorSet.IntersectionSegmentContourXld(hObject.SelectObj(it), trows2[0], tcol2[0], trows2[1], tcol2[1],
                            out HTuple row2, out HTuple colum2, out HTuple hTuple2);
                            if (row.Length >= 1)
                            {
                                rowsT1.Append(row);
                                colsT1.Append(colum);
                            }
                            if (row2.Length >= 1)
                            {
                                rowsT2.Append(row2);
                                colsT2.Append(colum2);
                            }
                        }
                        this["拟合点Rows"].Append(rowsT1);
                        this["拟合点Columns"].Append(colsT1);
                        this["拟合点Rows"].Append(rowsT2);
                        this["拟合点Columns"].Append(colsT2);
                        HOperatorSet.LinePosition(rows1[i], cols1[i], rows2[i], cols2[i], out HTuple rowcenter25, out HTuple colcenter25, out HTuple Length, out HTuple phi);
                        if (rowsT1.Length >= 2)
                        {
                            HOperatorSet.LinePosition(rows1[i], cols1[i], rowcenter25, colcenter25, out HTuple rowcenter, out HTuple colcenter, out Length, out phi);
                            HOperatorSet.TupleGenConst(rowsT1.Length, rows1[i], out HTuple rowcenter2);
                            HOperatorSet.TupleGenConst(rowsT1.Length, cols1[i], out HTuple colcenter2);
                            HOperatorSet.DistancePp(rowcenter2, colcenter2, rowsT1, colsT1, out HTuple dist2);
                            rowsT1 = rowsT1[dist2.TupleFind(dist2.TupleMin())];
                            colsT1 = colsT1[dist2.TupleFind(dist2.TupleMin())];
                        }
                        if (rowsT2.Length >= 2)
                        {
                            HOperatorSet.LinePosition(rows2[i], cols2[i], rowcenter25, colcenter25, out HTuple rowcenter, out HTuple colcenter, out Length, out phi);
                            HOperatorSet.TupleGenConst(rowsT2.Length, rows2[i], out HTuple rowcenter2);
                            HOperatorSet.TupleGenConst(colsT2.Length, cols2[i], out HTuple colcenter2);
                            HOperatorSet.DistancePp(rowcenter2, colcenter2, rowsT2, colsT2, out HTuple dist2);
                            rowsT2 = rowsT2[dist2.TupleFind(dist2.TupleMin())];
                            colsT2 = colsT2[dist2.TupleFind(dist2.TupleMin())];
                        }
                        if (rowsT1.Length == 1)
                        {
                            Mrows[i] = rowsT1;
                            MColums[i] = colsT1;
                        }
                        if (rowsT2.Length == 1)
                        {
                            Mrows2[i] = rowsT2;
                            MColums2[i] = colsT2;
                        }
                        if (rowsT1.Length == 1 && rowsT2.Length == 1)
                        {
                            HOperatorSet.DistancePp(Mrows2[i], MColums2[i], Mrows[i], MColums[i], out dist);
                            HOperatorSet.GenContourPolygonXld(out HObject hObjecttt, new HTuple(Mrows2.TupleSelect(i), Mrows.TupleSelect(i)),
                                new HTuple(MColums2.TupleSelect(i), MColums.TupleSelect(i)));
                            xdt = xdt.ConcatObj(hObjecttt);
                            lengts[i] = dist;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.LogErr(ex);
                    }
                }



            }
            catch (Exception ex)
            {
                // this.LogErr(ex);
            }
        }
        /// <summary>
        /// 构造测量
        /// </summary>
        /// <param name="typeName"></param>
        public Measure(MeasureType typeName)
        {
            Sigma = 1;
            Threshold = 30;
            TransitionStr = Transition.all;
            SelectStr = Select.all;
            Min_Measure_Point_Number = 2;

            amplitude = new HTuple();
            distance = new HTuple();
            DrawHObject = new HObject();
            MeasureHObj = new HObject();
            MeasureHObj.GenEmptyObj();
            DrawHObject.GenEmptyObj();
            HamMatDrawObj = new HObject();
            HamMatDrawObj.GenEmptyObj();
            measure_Type = typeName;
            if (typeName != MeasureType.Measure)
            {
                Enabled = true;
            }
        }

        public Measure() : this(MeasureType.Measure)
        {
        }

        #endregion 属性字段

        /// <summary>
        /// 执行测量并显示区域和名称
        /// </summary>
        /// <param name="halcon"></param>
        public void GetObj(HalconRun halcon , OneResultOBj oneResultOBj)
        {
            try
            {
                if (this.HamMatDrawObj == null)
                {
                    this.HamMatDrawObj = new HObject();
                    this.HamMatDrawObj.GenEmptyObj();
                }
                if (this.DrawHObject == null)
                {
                    this.DrawHObject = new HObject();
                    this.DrawHObject.GenEmptyObj();
                }
                this.MeasureObj(halcon, oneResultOBj);
                HObject hObject = this.HamMatDrawObj.ConcatObj(this.MeasureHObj);
                if (this.outRows.Length == 2)
                {
                    HOperatorSet.LinePosition(outRows.TupleSelect(0), outCols.TupleSelect(0), outRows.TupleSelect(1), outCols.TupleSelect(1), out HTuple rosc, out HTuple colCen, out HTuple lengt, out HTuple phi);
                    HOperatorSet.GenCrossContourXld(out HObject hObjectd, rosc, colCen, 10, phi);
                    hObject = hObject.ConcatObj(hObjectd);
                }
                halcon.AddOBJ(hObject);
                if (this.DrawRows != null && this.DrawRows.Length > 0)
                {
                    Vision.Disp_message(halcon.hWindowHalcon(), this.Name, this.DrawRows[0], this.DrawCols[0]);
                }
                else
                {
                    Vision.Disp_message(halcon.hWindowHalcon(), this.Name + "未绘制", 20, 20, true);
                }
            }
            catch (Exception ex)
            {
                halcon.ErrLog("显示测量错误:" + this.Name, ex);
            }
        }

        /// <summary>
        /// /根据类型画测量区域
        /// </summary>
        /// <param name="halcon"></param>
        /// <param name="point_Number"></param>
        /// <param name="measure_Waith"></param>
        /// <param name="measure_Heigth"></param>
        public new virtual HObject DrawObj(HalconRun halcon,  double point_Number, double measure_Waith, double measure_Heigth)
        {
            HObject hObject = new HObject();
            MeasurePointNumber = point_Number;
            Measure_Waigth = measure_Waith;
            Measure_Heigth = measure_Heigth;
            halcon.Drawing = true;
            switch (measure_Type)
            {
                case MeasureType.Cilcre:
                    hObject = DrawObjCilcr(halcon);
                    break;

                case MeasureType.Line:
                    hObject = DrawObjLine(halcon);
                    break;
                case MeasureType.Pake:
                    hObject = DrawObjPoint(halcon);
                    break;

                case MeasureType.Point2D:

                case MeasureType.PointIXLD:
                case MeasureType.Point:
                case MeasureType.XLDIntersectionXLD:
                    hObject = DrawObjPoint(halcon);
                    break;
                case MeasureType.NurbsMeasure:
                    HOperatorSet.DrawNurbsInterp(out DrawHObject, halcon.hWindowHalcon(), "true", "true",
                        "true", "true", 3, out HTuple controlrows, out HTuple controlsCol, out HTuple rows,
                        out HTuple cols, out HTuple weights, out HTuple tangents);
                    //this.Darw_Measuring_Arc(DrawHObject, out HamMatDrawObj, halcon.hWindowHalcon(),
                    //    this.Length,this.MeasurePointNumber,
                    //    this.Measure_Heigth, this.Measure_Waigth, Tepy, out this.drawRows, out this.drawCols, 
                    //    out drawPhi,out HTuple drowrows,out HTuple drowcols);
                    //DrawRows2 = drowrows;
                    //DrawCols2 = drowcols;

                    HOperatorSet.GenEmptyObj(out HamMatDrawObj);
                    Darw_Measuring_Arc(DrawHObject, out HObject ho_SegmentationCircles, out HObject ho_SegmentationCross,
                        out HObject ho_LeftCross, out HObject ho_RightCross, out HObject ho_LeftRectangles,
                        out HObject ho_RightRectangles, this.MeasurePointNumber, this.Length, this.Measure_Heigth,
                        this.Measure_Waigth, out HTuple hv_SegmentationRows, out HTuple hv_SegmentationColumns,
                        out HTuple hv_LeftControlPointRs, out HTuple hv_LeftControlPointCs, out HTuple hv_RightControlPointRs,
                        out HTuple hv_RightControlPointCs, out HTuple hv_LeftMeasureAngles, out HTuple hv_RightMeasureAngles);
                    //显示线段分割点
                    HOperatorSet.ConcatObj(HamMatDrawObj, ho_SegmentationCross, out HamMatDrawObj);
                    //显示左侧测量矩形中点
                    HOperatorSet.ConcatObj(HamMatDrawObj, ho_LeftCross, out HamMatDrawObj);
                    //显示右侧测量矩形中点
                    HOperatorSet.ConcatObj(HamMatDrawObj, ho_RightCross, out HamMatDrawObj);
                    //显示左侧测量矩形
                    HOperatorSet.ConcatObj(HamMatDrawObj, ho_LeftRectangles, out HamMatDrawObj);
                    //显示右侧测量矩形
                    HOperatorSet.ConcatObj(HamMatDrawObj, ho_RightRectangles, out HamMatDrawObj);
                    //显示复合对象
                    HOperatorSet.DispObj(HamMatDrawObj, halcon.hWindowHalcon());

                    //左侧测量矩形中点坐标与角度
                    drawRows = hv_LeftControlPointRs;
                    drawCols = hv_LeftControlPointCs;
                    drawPhi = hv_LeftMeasureAngles;
                    //右侧测量矩形中点坐标与角度
                    DrawRows2 = hv_RightControlPointRs;
                    DrawCols2 = hv_RightControlPointCs;
                    RightDrawPhi = hv_RightMeasureAngles;
                    this["DrawRows2"] = halcon.GetCaliConstMM(DrawRows2);
                    this["DrawCols2"] = halcon.GetCaliConstMM(DrawCols2);
                    this["DrawPhis2"] = RightDrawPhi;
                    this["DrawPhis"] = drawPhi;
                    break;
                default:
                    halcon.GetOneImageR().AddMeassge("未指定测量类型");
                    break;
            }
            this["DrawRows"] = halcon.GetCaliConstMM(DrawRows);
            this["DrawCols"] = halcon.GetCaliConstMM(DrawCols);


            halcon.Drawing = false;
            if (hObject.IsInitialized())
            {
                DrawHObject = hObject.Clone();
            }
            this.GetObj(halcon, halcon.GetOneImageR());
            return hObject;
        }

        public new virtual HObject DrawObj(HalconRun halcon)
        {
            return this.DrawObj(halcon, this.MeasurePointNumber, Measure_Waigth, Measure_Heigth);
        }

        /// <summary>
        /// 画测量圆弧区域
        /// </summary>
        /// <param name="halcon"></param>
        public virtual HObject DrawObjCilcr(HalconRun halcon)
        {
            HObject hObject = new HObject();
            try
            {
                Vision.Daw_spoke(halcon.Image(), out hObject, halcon.hWindowHalcon(), MeasurePointNumber, Measure_Heigth, Measure_Waigth, out drawRows, out drawCols, out DrawDirect);

                if (Min_Measure_Point_Number <= 3)
                {
                    Min_Measure_Point_Number = (int)(MeasurePointNumber / 2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return hObject;
        }

        /// <summary>
        /// 画测量直线区域
        /// </summary>
        /// <param name="halcon"></param>
        public virtual HObject DrawObjLine(HalconRun halcon)
        {
            HObject hObject = new HObject();
            try
            {
                Vision.Draw_rake(out hObject, halcon.hWindowHalcon(), MeasurePointNumber, Measure_Heigth, Measure_Waigth,
                out HTuple row1, out HTuple column1, out HTuple row2, out HTuple column2);
                DrawRows = new HTuple();
                DrawRows.Append(row1);
                DrawRows.Append(row2);
                DrawCols = new HTuple();
                DrawCols.Append(column1);
                DrawCols.Append(column2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return hObject;
        }

        /// <summary>
        /// 修改区域
        /// </summary>
        /// <param name="halcon"></param>
        /// <param name="point_Number"></param>
        /// <param name="measure_Waith"></param>
        /// <param name="measure_Heigth"></param>
        public virtual HObject DrawModObj(HalconRun halcon, int point_Number, double measure_Waith, double measure_Heigth)
        {
            this.MeasurePointNumber = point_Number;
            this.Measure_Waigth = measure_Waith;
            this.Measure_Heigth = measure_Heigth;
            return DrawHObject;
        }

        public virtual HObject DrawModObj(HalconRun halcon)
        {
            HObject hObject = new HObject();
            halcon.HobjClear();
            if (halcon.Drawing)
            {
                return DrawHObject;
            }
            halcon.Drawing = true;
            switch (measure_Type)
            {
                case MeasureType.Cilcre:

                    break;

                case MeasureType.Line:
                    if (DrawRows.Length == 2 && DrawCols.Length == 2)
                    {
                        HOperatorSet.DrawLineMod(halcon.hWindowHalcon(), DrawRows[0], DrawCols[0], DrawRows[1], DrawCols[1], out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                        DrawRows[0] = row1;
                        DrawRows[1] = row2;
                        drawCols[0] = col1;
                        drawCols[1] = col2;
                    }
                    break;

                case MeasureType.Pake:
                    HOperatorSet.TupleRad(Draw_Phi, out HTuple rad);
                    HOperatorSet.DrawRectangle2Mod(halcon.hWindowHalcon(), DrawRows, DrawCols, rad, DrawLength1, DrawLength2, out HTuple row, out HTuple column,
                        out HTuple phi, out HTuple length1, out HTuple length2);
                    DrawRows = row; DrawCols = column; HomMatPhi = phi; DrawLength1 = length1; DrawLength2 = length2;
                    HOperatorSet.GenRectangle2(out HObject rectangle, DrawRows, DrawCols, HomMatPhi, DrawLength1, DrawLength2);
                    break;
                case MeasureType.Point2D:
                case MeasureType.Point:
                case MeasureType.PointIXLD:
                case MeasureType.XLDIntersectionXLD:
                    hObject = this.DrawModObjPoint(halcon);
                    break;
                case MeasureType.NurbsMeasure:
                    //this.Darw_Measuring_Arc(DrawHObject, out HamMatDrawObj, halcon.hWindowHalcon(), this.Length,
                    // this.Measure_Waigth, this.Measure_Heigth, out this.drawRows, out this.drawCols, out drawPhi);
                    //this.Darw_Measuring_Arc(DrawHObject, out HamMatDrawObj, halcon.hWindowHalcon(),   this.Length,
                    //  this.MeasurePointNumber,  this.Measure_Heigth, this.Measure_Waigth, Tepy, out this.drawRows,
                    //  out this.drawCols,out drawPhi, out HTuple drowrows, out HTuple drowcols);
                    //        DrawRows2 = drowrows;
                    //        DrawCols2 = drowcols;
                    HOperatorSet.GenEmptyObj(out HamMatDrawObj);
                    Darw_Measuring_Arc(DrawHObject, out HObject ho_SegmentationCircles, out HObject ho_SegmentationCross,
                                       out HObject ho_LeftCross, out HObject ho_RightCross, out HObject ho_LeftRectangles,
                                       out HObject ho_RightRectangles, this.MeasurePointNumber, this.Length, this.Measure_Heigth,
                                       this.Measure_Waigth, out HTuple hv_SegmentationRows, out HTuple hv_SegmentationColumns,
                                       out HTuple hv_LeftControlPointRs, out HTuple hv_LeftControlPointCs, out HTuple hv_RightControlPointRs,
                                       out HTuple hv_RightControlPointCs, out HTuple hv_LeftMeasureAngles, out HTuple hv_RightMeasureAngles);
                    //显示线段分割点
                    HOperatorSet.ConcatObj(HamMatDrawObj, ho_SegmentationCross, out HamMatDrawObj);
                    //显示左侧测量矩形中点
                    HOperatorSet.ConcatObj(HamMatDrawObj, ho_LeftCross, out HamMatDrawObj);
                    //显示右侧测量矩形中点
                    HOperatorSet.ConcatObj(HamMatDrawObj, ho_RightCross, out HamMatDrawObj);
                    //显示左侧测量矩形
                    HOperatorSet.ConcatObj(HamMatDrawObj, ho_LeftRectangles, out HamMatDrawObj);
                    //显示右侧测量矩形
                    HOperatorSet.ConcatObj(HamMatDrawObj, ho_RightRectangles, out HamMatDrawObj);
                    //显示复合对象
                    HOperatorSet.DispObj(HamMatDrawObj, halcon.hWindowHalcon());

                    //左侧测量矩形中点坐标与角度
                    drawRows = hv_LeftControlPointRs;
                    drawCols = hv_LeftControlPointCs;
                    drawPhi = hv_LeftMeasureAngles;
                    //右侧测量矩形中点坐标与角度
                    DrawRows2 = hv_RightControlPointRs;
                    DrawCols2 = hv_RightControlPointCs;
                    RightDrawPhi = hv_RightMeasureAngles;
                    HObject hObjectt = new HObject();
                    hObjectt.GenEmptyObj();
                    for (int i = 0; i < drawRows.Length; i++)
                    {
                        HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(DrawRows.TupleSelect(i), DrawRows2.TupleSelect(i)),
                            new HTuple(DrawCols.TupleSelect(i), DrawCols2.TupleSelect(i)));
                        hObjectt = hObjectt.ConcatObj(hObject2);
                    }
                    HOperatorSet.DispObj(hObjectt, halcon.hWindowHalcon());
                    break;
                default:
                    halcon.GetOneImageR().AddMeassge("未指定测量类型");
                    break;
            }
            halcon.Drawing = false;
            if (hObject.IsInitialized())
            {
                DrawHObject = hObject.Clone();
            }
            this.GetObj(halcon,halcon.GetOneImageR());
            return DrawHObject;
        }

        private HObject DrawModObjPoint(HalconRun halcon)
        {
            try
            {
                HOperatorSet.TupleRad(Draw_Phi, out HTuple rad);
                HOperatorSet.DrawRectangle2Mod(halcon.hWindowHalcon(), DrawRows[0], DrawCols[0], rad, DrawLength1, DrawLength2, out drawRows, out drawCols, out HTuple hTuple, out DrawLength1, out DrawLength2);
                Draw_Phi = hTuple.TupleDeg();
                Coordinate.CpointXY cpointRC = halcon.CoordinatePXY.GetPointXYtoRC(DrawRows.D, DrawCols.D);
                this.DrawCentX = cpointRC.X;
                this.DrawCentY = cpointRC.Y;
                HOperatorSet.GenRectangle2(out HObject rectangle, DrawRows, DrawCols, hTuple, DrawLength1, DrawLength2);
                return rectangle;
            }
            catch (Exception ex)
            {
                halcon.ErrLog("修改点错误：" + this.Name, ex);
            }
            return new HObject();
        }

        /// <summary>
        /// 修改直线
        /// </summary>
        /// <param name="halcon"></param>
        /// <param name="point_Number"></param>
        /// <param name="measure_Waith"></param>
        /// <param name="measure_Heigth"></param>
        public virtual HObject DrawModObjLine(HalconRun halcon)
        {
            return DrawHObject;
        }

        /// <summary>
        /// 修改圆
        /// </summary>
        /// <param name="halcon"></param>
        /// <param name="point_Number"></param>
        /// <param name="measure_Waith"></param>
        /// <param name="measure_Heigth"></param>
        public virtual HObject DrawModObjCilcr(HalconRun halcon)
        {
            return DrawHObject;
        }

        public virtual HObject GetMeasureOBJ()
        {
            return MeasureHObj;
        }

        public virtual HObject GetMeasurePoints()
        {
            try
            {
                HOperatorSet.GenCrossContourXld(out HObject cross, this["PointRows"], this["PointColumns"], 15, 0);
                return cross;
            }
            catch (Exception)
            {
                return new HObject();
            }
        }

        /// <summary>
        /// 测量带参数彷射
        /// </summary>
        /// <param name="halcon"></param>
        public virtual ObjectColor MeasureObj(HalconRun halcon, OneResultOBj oneResultOBj)
        {
            //this.KeyHObject.DirectoryHObject.Clear();
            return MeasureObj(halcon, this.ISMatHat, oneResultOBj);
        }

        /// <summary>
        /// 不执行彷射的测量
        /// </summary>
        /// <param name="halcon"></param>
        /// <returns></returns>
        public ObjectColor TMeasureObj(HObject image, HalconRun halcon  , OneResultOBj oneResultOBj)
        {
            ObjectColor objectColor = new ObjectColor();
            OutRows = distance = outCols = new HTuple();
            this.SetSave("DrawRows");
            this.SetSave("DrawCols");
            this.SetSave("DrawRows2");
            this.SetSave("DrawCols2");
            this.SetSave("DrawPhis");
            this.SetSave("DrawPhis2");
            this.SetSave("Distance");

            OutCentreRow = null;
            outCentreCol = null;
            this.ClerItem();
            OutPhi = 9999;
            objectColor.HobjectColot = this.color;
            if (!Enabled)
            {
                halcon.GetOneImageR().AddMeassge(this.Name + "未启用测量");
                return objectColor;
            }
            switch (measure_Type)
            {
                case MeasureType.Cilcre:
                    objectColor._HObject = MeasureObjCilcre(image, halcon);
                    if (this.IsDisObj)
                    {
                        //halcon.AddOBJ(objectColor._HObject.ConcatObj(KeyHObject["拟合圆的中心点"]));
                    }
                    break;
                case MeasureType.Pake:
                    objectColor._HObject = MeasureObjPeak(image, halcon);
                    ISSt = true;
                    break;
                case MeasureType.Line:
                    objectColor._HObject = MeasureObjLine(image, halcon, oneResultOBj);
                    break;
                case MeasureType.Point:
                case MeasureType.PointIXLD:
                case MeasureType.Point2D:
                    objectColor._HObject = MeasureObjPoint(image, halcon);
                    break;
                case MeasureType.XLDToXLD:
                case MeasureType.XLDPointInXLDPoint:
                case MeasureType.XLDIntersectionXLD:
                    objectColor._HObject = MeasureXLD(halcon);
                    break;
                case MeasureType.NurbsMeasure:
                    try
                    {
                        //HOperatorSet.GrayClosingShape(halcon.Image(), out HObject hObject, 10, 10, "octagon");
                        //Measuring_Arc(hObject, out HObject ho_LeftMeasurePointCross, out HObject ho_RightMeasurePointCross,
                        //             out HObject LeftMeasureRectangles,
                        //             out HObject RightMeasureRectangles,
                        //             out HObject ho_MeasureSegments, drawRows, drawCols, DrawRows2, DrawCols2, drawPhi,
                        //             RightDrawPhi, this.Measure_Heigth / Scale, this.Measure_Waigth / Scale, new HTuple(this.Sigma, this.Sigma2),
                        //             new HTuple(this.Threshold, this.Threshold2), out outRows,
                        //             out outCols, out outRows2, out outCols2, out this.distance);
                        Darw_Measuring_Arc(DrawHObject, out HObject ho_SegmentationCircles, out HObject ho_SegmentationCross,
                   out HObject ho_LeftCross, out HObject ho_RightCross, out HObject ho_LeftRectangles,
                   out HObject ho_RightRectangles, this.MeasurePointNumber, this.Length, this.Measure_Heigth,
                   this.Measure_Waigth, out HTuple hv_SegmentationRows, out HTuple hv_SegmentationColumns,
                   out HTuple hv_LeftControlPointRs, out HTuple hv_LeftControlPointCs, out HTuple hv_RightControlPointRs,
                   out HTuple hv_RightControlPointCs, out HTuple hv_LeftMeasureAngles, out HTuple hv_RightMeasureAngles);
                        HObject hObjectt = new HObject();
                        //左侧测量矩形中点坐标与角度
                        DrawRows = hv_LeftControlPointRs;
                        DrawCols = hv_LeftControlPointCs;
                        drawPhi = hv_LeftMeasureAngles;
                        //右侧测量矩形中点坐标与角度
                        DrawRows2 = hv_RightControlPointRs;
                        DrawCols2 = hv_RightControlPointCs;
                        RightDrawPhi = hv_RightMeasureAngles;

                        hObjectt.GenEmptyObj();
                        this.SetDefault("maxT", 100, true);
                        this.SetDefault("minT", 0, true);

                        this.SetDefault("SeleWidth", 120, true);
                        this.SetDefault("SeleHeight", 120, true);
                        this.SetDefault("OpeningCircle", 20, true);
                        HOperatorSet.DistancePp(DrawRows[0], drawCols[0], DrawRows2[0], DrawCols2[0], out HTuple dins);
                        HOperatorSet.DistancePp(DrawRows[0], drawCols[0], DrawRows[1], drawCols[1], out HTuple dins2);
                        HTuple sHeigth = this.Measure_Heigth / 2;
                        for (int i = 0; i < DrawRows.Length; i++)
                        {
                            HOperatorSet.GenRectangle2(out HObject roet2, DrawRows[i], drawCols[i], drawPhi[i],
                            dins / 2 + sHeigth, dins2);
                            HOperatorSet.GenRectangle2(out HObject roet3, DrawRows2[i], DrawCols2[i], drawPhi[i],
                            dins / 2 + sHeigth, dins2);
                            hObjectt = hObjectt.ConcatObj(roet2.ConcatObj(roet3));
                        }
                        halcon.AddOBJ(hObjectt);
                        //objectColor._HObject = hObjectt;

                        HOperatorSet.Union1(hObjectt, out hObjectt);

                        HOperatorSet.ReduceDomain(halcon.Image(), hObjectt, out hObjectt);
                        this.SetDefault("EmpMaskWidth", 3, true);
                        this.SetDefault("EmpMaskHeight", 3, true);
                        this.SetDefault("EmpFactor", 4, true);
                        this.SetDefault("GrayMaskHeight", 10, true);
                        this.SetDefault("ClosingCircle", 10, true);
                        this.SetDefault("GrayMaskWidth", 10, true);
                        this.SetDefault("GrayMaskShape", "octagon", true);
                        //HOperatorSet.Emphasize(hObjectt, out hObjectt, this["EmpMaskWidth"], this["EmpMaskHeight"], this["EmpFactor"]);
                        //HOperatorSet.GrayClosingShape(hObjectt, out hObjectt, 5, 5, this["GrayMaskShape"]);
                        //halcon.SetAddObj(this.Name + "GrayCl", hObjectt);
                        HOperatorSet.Threshold(hObjectt, out HObject timeobj, this["minT"], this["maxT"]);
                        HOperatorSet.Connection(timeobj, out timeobj);

                        HOperatorSet.SelectShape(timeobj, out timeobj, "width", "and",
             5, 999999);
                        halcon.AddOBJ(timeobj);
                        HOperatorSet.ClosingCircle(timeobj, out timeobj, this["ClosingCircle"].D);
                        HOperatorSet.OpeningCircle(timeobj, out timeobj, this["OpeningCircle"].D);

                        halcon.AddOBJ(timeobj);
                        if (timeobj.CountObj() == 0)
                        {
                            halcon.GetOneImageR().AddMeassge(this.Name + ":" + "筛选值过大,未能找到区域");
                            goto end;
                        }

                        //HOperatorSet.DilationCircle(timeobj, out timeobj, 10);
                        HOperatorSet.HeightWidthRatio(timeobj, out HTuple hTupleh, out HTuple hTuplew, out HTuple hTupleR);
                        this["Height"] = hTupleh;
                        this["Width"] = hTuplew;
                        this["Ratio"] = hTupleR;
                        HOperatorSet.Union1(timeobj, out timeobj);
                        HOperatorSet.Connection(timeobj, out timeobj);
                        //HOperatorSet.ClosingCircle(timeobj, out timeobj, this["ClosingCircle"].D);
                        int dte = timeobj.CountObj();
                        if (dte > 2)
                        {

                        }
                        HOperatorSet.SelectShape(timeobj, out timeobj, new HTuple("height", "width"), "and",
                new HTuple(this["SeleHeight"].D, this["SeleWidth"].D), new HTuple(9999999, 9999999));
                        //HOperatorSet.DilationRectangle1(timeobj, out timeobj, 50, 50);


                        if (timeobj.CountObj() == 0)
                        {
                            HOperatorSet.SelectShape(timeobj, out timeobj, new HTuple("height", "width"), "and",
                          new HTuple(hTupleh.TupleMax() - 0.1, hTuplew.TupleMax() - 0.1), new HTuple(999999, 999999));
                        }
                        else
                        {
                            //timeobj = timeobj2;
                        }

                        HOperatorSet.Union1(timeobj, out timeobj);
                        //          this["fill"] = 200;
                        this.SetDefault("fill", 200, true);
                        HOperatorSet.Complement(timeobj, out timeobj);
                        HOperatorSet.PaintRegion(timeobj, halcon.Image(), out timeobj, this["fill"], "fill");

                        HOperatorSet.Emphasize(timeobj, out timeobj, this["EmpMaskWidth"], this["EmpMaskHeight"], this["EmpFactor"]);
                        HOperatorSet.GrayClosingShape(timeobj, out timeobj, this["GrayMaskHeight"], this["GrayMaskWidth"], this["GrayMaskShape"]);

                        //        HOperatorSet.Emphasize(timeobj, out timeobj, 6, 5, 4);
                        //halcon.SetDefault("Debug", 1, true);
                        halcon.AddOBJ(timeobj);
                        halcon.SetDefault("MEs", "null", true);
                        if (halcon["MEs"] == "null")
                        {

                            //HOperatorSet.GrayClosingShape(timeobj, out timeobj, 5, 5, "octagon");
                            Measuring_Arc(timeobj, out NGRoi, this.Measure_Waigth, Measure_Heigth,
                            this.Tepy, InterpolationStr.ToString(), new HTuple(this.Sigma, this.Sigma2),
                            new HTuple(this.Threshold, this.Threshold2),
                            new HTuple(this.SelectStr.ToString(), this.SelectStr2.ToString()),
                            new HTuple(this.TransitionStr.ToString(), this.TransitionStr2.ToString()),
                            DrawRows, DrawCols, drawPhi, DrawRows2, DrawCols2, out outRows, out outCols,
                            out outRows2, out outCols2, out this.distance);
                        }
                        else
                        {

                            this["拟合点Rows"] = new HTuple();
                            this["拟合点Columns"] = new HTuple();
                            HObject xdtt = new HObject();
                            HObject itmes = new HObject();
                            this.SetDefault("SubPixMin", 31, true);
                            this.SetDefault("SubPixMax", 100, true);

                            this.SetDefault("Sigma", 9, true);
                            this.SetDefault("SelectContlengthMin", 31, true);
                            HObject hObject3;
                            int de = image.CountObj();
                            this["Distance"] = new HTuple();
                            HOperatorSet.EdgesSubPix(timeobj, out hObject3, "canny", this["Sigma"], this["SubPixMin"], this["SubPixMax"]);
                            HOperatorSet.SegmentContoursXld(hObject3, out hObject3, "lines_ellipses", 10, 0.1, 2);
                            halcon.AddOBJ(hObject3);


                            this.SetDefault("FitClippingLength", 0, true);
                            this.SetDefault("FitLength", "auto", true);
                            this.SetDefault("MaxTangAngle", new HTuple(90).TupleDeg(), true);
                            this.SetDefault("MaxDist", 25, true);
                            this.SetDefault("MaxDistPerp", 10, true);
                            this.SetDefault("MaxOverlap", 2, true);
                            //this["MaxTangAngle"] = new HTuple(90);
                            int d = hObject3.CountObj();
                            for (int i = 0; i < d; i++)
                            {
                                HOperatorSet.UnionCotangentialContoursXld(hObject3, out hObject3, this["FitClippingLength"], this["FitLength"],
                                this["MaxTangAngle"].TupleRad(), this["MaxDist"], this["MaxDistPerp"], this["MaxOverlap"], "attr_forget");
                            }
                            halcon.AddOBJ(hObject3);

                            HOperatorSet.SelectShapeXld(hObject3, out hObject3, "contlength", "and", this["SelectContlengthMin"], 200000);
                            halcon.AddOBJ(hObject3);

                            Measuring_xld(hObject3, DrawRows, drawCols, DrawRows2, DrawCols2, hv_LeftMeasureAngles, this.Measure_Heigth, out outRows,
                               out outCols, out outRows2, out outCols2, out distance, out HObject hObject, out NGRoi, out HObject itmexl);
                            halcon.AddOBJ(hObject);
                            halcon.AddOBJ(itmexl);
                        }
                    end:
                        this["distance"] = this.distance;
                        HTuple rows = outRows.TupleConcat(OutRows2);
                        rows = rows.TupleRemove(rows.TupleFind(0));
                        HTuple cols = OutCols.TupleConcat(outCols2);
                        cols = cols.TupleRemove(cols.TupleFind(0));
                        HOperatorSet.GenCrossContourXld(out HObject LeftMeasureRectangles, rows, cols, 40, 0);
                        halcon.AddOBJ(LeftMeasureRectangles);

                        HOperatorSet.ConcatObj(objectColor._HObject, NGRoi.ConcatObj(LeftMeasureRectangles), out objectColor._HObject);
                        this["距离mm"] = halcon.GetCaliConstMM(this.distance);
                        //hObject.Dispose();
                        ISSt = NurbsMeasureResult(halcon);
                    }
                    catch (Exception ex)
                    {
                        halcon.GetOneImageR().AddMeassge(this.Name + ex.Message);
                    }
                    break;
                default:
                    halcon.GetOneImageR().AddMeassge(this.Name + "未指定的测量类型:" + this.Measure_Type.ToString());
                    break;
            }
            this["row"] = this.outCentreRow;
            this["col"] = this.OutCentreCol;
            if (this.IsDisObj)
            {
                halcon.AddOBJ(objectColor._HObject,this.color);

            }
            MeasureHObj = objectColor._HObject;
            return objectColor;
        }
        [DescriptionAttribute("允许胶宽最小值(mm)"), Category("测量参数"), DisplayName("允许胶宽最小值(mm)")]
        public double DistanceMin { get; set; } = 0;

        [DescriptionAttribute("允许胶宽最大值(mm)"), Category("测量参数"), DisplayName("允许胶宽最大值(mm)")]
        public double DistanceMax { get; set; } = 0;

        /// <summary>
        /// 带启用放射的测量
        /// </summary>
        /// <param name="halcon"></param>
        /// <param name="isHamMat">true仿射,fales不仿射</param>
        /// <returns></returns>
        public virtual ObjectColor MeasureObj(HalconRun halcon, bool isHamMat, OneResultOBj oneResultOBj)
        {
            if (!Enabled)
            {
                return new ObjectColor();
            }
            if (measure_Type == MeasureType.Measure)
            {
                return new ObjectColor();
            }
            if (isHamMat)
            {
                if (halcon.GetHomdeMobel(this.HomName) != null)
                {
                    for (int i = 0; i < halcon.GetHomdeMobel(this.HomName).Length; i++)
                    {
                        return MeasureObj(halcon, halcon.GetHomdeMobel(this.HomName), oneResultOBj);
                    }
                }
                halcon.GetOneImageR().AddMeassge("缺少彷射参数");

                return new ObjectColor();
            }
            else
            {
                HOperatorSet.TupleRad(Draw_Phi, out HTuple rad);
                HomMatPhi = rad;
                this["DrawRows"] = DrawRows;
                this["DrawCols"] = DrawCols;
                this["DrawPhis"] = drawPhi;
                if (DrawRows2 == null)
                {
                    DrawRows2 = new HTuple();

                }
                if (DrawCols2 == null)
                {
                    DrawCols2 = new HTuple();
                }
                if (RightDrawPhi == null)
                {
                    RightDrawPhi = new HTuple();
                }
                this["DrawRows2"] = halcon.GetCaliConstMM(DrawRows2);
                this["DrawCols2"] = halcon.GetCaliConstMM(DrawCols2);
                this["DrawPhis2"] = RightDrawPhi;
                this["Distance"] = distance;

                return TMeasureObj(halcon.Image(), halcon, oneResultOBj);
            }
        }

        /// <summary>
        /// 放射区域
        /// </summary>
        /// <param name="halcon"></param>
        /// <param name="HamMat"></param>
        public virtual ObjectColor MeasureObj(HalconRun halcon, HTuple HamMat, OneResultOBj oneResultOBj)
        {
            return MeasureObj(halcon.Image(), halcon, HamMat, oneResultOBj);
        }
        public virtual ObjectColor MeasureObj(HObject image, HalconRun halcon, HTuple HamMat, OneResultOBj oneResultOBj)
        {
            try
            {
                this.MeasureHObj.Dispose();
                this.MeasureHObj.GenEmptyObj();
                this.HamMatDrawObj.Dispose();
                this.HamMatDrawObj.GenEmptyObj();
                this.outCentreCol = null;
                this.outCentreRow = null;
                this.OutRadius = null;
                this.outCentreRow = null;
                this.outCols = null;
                this.OutRows = null;
                this.distance = new HTuple();
                if (!Enabled)
                {
                    return new ObjectColor();
                }
                if (HamMat.Length == 6)
                {
                    this["DrawRows"] = new HTuple();
                    this["DrawCols"] = new HTuple();
                    HTuple hTupleRow = new HTuple();
                    HTuple hTupleCol = new HTuple();
                    switch (measure_Type)
                    {
                        case MeasureType.Measure:
                            break;

                        case MeasureType.Cilcre:
                        case MeasureType.Line:
                        case MeasureType.Pake:
                            HOperatorSet.AffineTransPoint2d(HamMat, DrawRows, DrawCols, out hTupleRow, out hTupleCol);
                            this["DrawRows"].Append(hTupleRow);
                            this["DrawCols"].Append(hTupleCol);

                            break;

                        case MeasureType.XLDIntersectionXLD:
                        case MeasureType.Point:
                        case MeasureType.PointIXLD:
                            HOperatorSet.AffineTransPoint2d(HamMat, DrawRows, DrawCols, out hTupleRow, out hTupleCol);
                            HOperatorSet.TupleRad(Draw_Phi, out HTuple rad);
                            HomMatPhi = rad - HamMat.TupleSelect(1);
                            this["DrawRows"].Append(hTupleRow);
                            this["DrawCols"].Append(hTupleCol);
                            break;
                        case MeasureType.Point2D:
                            Coordinate.CpointXY cpointRC = halcon.CoordinatePXY.GetPointXYtoRC(DrawCentY, DrawCentX);
                            this["DrawRows"] = cpointRC.X;
                            this["DrawRows"] = cpointRC.Y;
                            HOperatorSet.TupleRad(Draw_Phi, out rad);
                            HomMatPhi = rad - HamMat.TupleSelect(1);
                            break;

                        case MeasureType.XLDToXLD:
                        case MeasureType.XLDPointInXLDPoint:

                            break;
                        case MeasureType.NurbsMeasure:
                            HOperatorSet.AffineTransPoint2d(HamMat, DrawRows, DrawCols, out hTupleRow, out hTupleCol);
                            this["DrawPhi"] = drawPhi.TupleSub(HamMat.TupleSelect(1));
                            this["DrawRows"] = hTupleRow;
                            this["DrawCols"] = hTupleCol;

                            HOperatorSet.AffineTransPoint2d(HamMat, DrawRows2, DrawCols2, out hTupleRow, out hTupleCol);
                            this["DrawRows2"] = hTupleRow;
                            this["DrawCols2"] = hTupleCol;
                            break;
                        default:

                            return new ObjectColor();
                    }
                }
                else
                {
                    this["DrawRows"] = DrawRows;
                    this["DrawCols"] = DrawCols;
                    this["DrawPhi"] = DrawPhi;
                    this["DrawRows2"] = DrawRows2;
                    this["DrawCols2"] = DrawCols2;
                }
            }
            catch (Exception ex)
            {
                halcon.ErrLog("测量" + this.Name + "错误:", ex);
            }
            return TMeasureObj(image, halcon , oneResultOBj);
        }
        /// <summary>
        /// 测量圆弧
        /// </summary>
        /// <param name="halcon"></param>
        public virtual HObject MeasureObjCilcre(HObject image, HalconRun halcon)
        {
            try
            {
                Vision.Spoke(image, out HamMatDrawObj, MeasurePointNumber, Measure_Heigth, Measure_Waigth,
                 this.Sigma, this.Threshold, this.TransitionStr.ToString(), this.SelectStr.ToString(), this["DrawRows"], this["DrawCols"], DrawDirect,
                 out outRows, out outCols, out HTuple arc_Type);
                this["拟合点Rows"] = outRows;
                this["拟合点Columns"] = outCols;
                this["测量类型"] = arc_Type; 
                Vision.Pts_to_best_circle(out HObject _Object, outRows, outCols, this.Min_Measure_Point_Number, arc_Type, 
                    out HTuple hvRow_Center, out HTuple hvCol_Center,
                  out HTuple hvRadius, out HTuple hvStartPhi, out HTuple hvEndPhi, out HTuple hvPointOrder, out HTuple HvArcAngle);
                this.MeasureHObj = _Object;
                if (hvEndPhi.Length == 0)
                {
                    return _Object;
                }
                HOperatorSet.GenCrossContourXld(out HObject hObject1, hvRow_Center, hvCol_Center, 60, 0);
                _Object = _Object.ConcatObj(hObject1);

                OutCentreRow = hvRow_Center;
                OutCentreCol = hvCol_Center;
                OutRadius = hvRadius;
                this["半径"] = hvRadius;
                outRadiusMM = halcon.GetCaliConstMM(hvRadius);
                this["半径MM"] = outRadiusMM;
                if (IsRadius)
                {
                    this["半径MM"] = halcon.GetCalib().SetCet(hvRadius);
                    halcon.AddMessageIamge(hvRow_Center, hvCol_Center, "半径:" + this["半径MM"]);
                }
                this["开始角度"] = hvStartPhi.TupleDeg();
                this["结束角度"] = hvEndPhi.TupleDeg();
                this.outPhi = hvEndPhi.TupleDeg();
                this["区域明暗"] = hvPointOrder;
                this["角弧度"] = HvArcAngle;
                this["测量到数量"] = outCols.Length;


     
                //HOperatorSet.GenCircleContourXld(out HObject ho_Circle, hvRow_Center, hvCol_Center,
                //hvRadius, 0, 6.28318, "positive", 1);


                //    string str = "圆的中心：" + hvRowCenter + "," + hvColCenter + ",半径:" + hvRadius + "开始角度：" + hvStartPhi.TupleDeg() + "结束角度" + hvEndPhi.TupleDeg()+"弧度："+HvArcAngle.TupleDeg();
                return _Object;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return new HObject();
        }

        /// <summary>
        /// 测量直线
        /// </summary>
        /// <param name="halcon"></param>
        public virtual HObject MeasureObjLine(HObject image, HalconRun halcon, OneResultOBj oneResultOBj)
        {
            HObject line = new HObject();
            line.GenEmptyObj();
            try
            {
                Vision.Rake(image, out this.HamMatDrawObj, this.MeasurePointNumber, this.Measure_Heigth, this.Measure_Waigth,
                this.Sigma, this.Threshold, this.TransitionStr.ToString(), this.SelectStr.ToString(), this["DrawRows"].TupleSelect(0), this["DrawCols"].TupleSelect(0),
                this["DrawRows"].TupleSelect(1), this["DrawCols"].TupleSelect(1), out HTuple outRowst, out HTuple outColst);
                this["拟合点Rows"] = outRowst;
                this["拟合点Columns"] = outColst;

                if (outRowst.Length < this.Min_Measure_Point_Number)
                {
                    this["直线中心Row"] = outRowst;
                    this["直线中心Column"] = outColst;
                }
                else
                {
                    Vision.Pts_to_best_line(out line, outRowst, outColst, this.Min_Measure_Point_Number, out HTuple hv_row1, out HTuple hv_col1, out HTuple hv_row2, out HTuple hv_col2);
                    HTuple outrowsT = new HTuple();
                    HTuple outcolsT = new HTuple();
                    HOperatorSet.DistancePl(outRowst, outColst, hv_row1, hv_col1, hv_row2, hv_col2, out HTuple dslength);
                    for (int i = 0; i < dslength.Length; i++)
                    {
                        if (dslength[i] < MeasurePointDistance)
                        {
                            outrowsT.Append(outRowst[i]);
                            outcolsT.Append(outColst[i]);
                        }
                    }
                    outRowst = outrowsT;
                    outColst = outcolsT;
                    Vision.Pts_to_best_line(out line, outRowst, outColst, this.Min_Measure_Point_Number, out hv_row1, out hv_col1, out hv_row2, out hv_col2);
                    HOperatorSet.LinePosition(hv_row1, hv_col1, hv_row2, hv_col2, out HTuple rowcenter, out HTuple colcenter, out HTuple Length, out HTuple phi);
                    outRows = new HTuple();
                    outRows.Append(hv_row1);
                    outRows.Append(hv_row2);
                    outCols = new HTuple();
                    outCols.Append(hv_col1);
                    outCols.Append(hv_col2);
                    OutCentreRow = rowcenter.D;
                    outCentreCol = colcenter.D;
                    distance = Length;
                
                    if (hv_col2 != new HTuple())
                    {
                        ISSt = true;
                    }
                    HOperatorSet.AffineTransPoint2d(halcon.CoordinatePXY.CoordHanMat2DXY, outRows, outCols, out HTuple xd, out HTuple yd);
                    HOperatorSet.LinePosition(xd[0], yd[0], xd[1], yd[1], out rowcenter, out colcenter, out Length, out phi);
                    OutPhi = phi.TupleDeg();
                    HOperatorSet.AngleLl(0, 0, 0.1, 0, xd[0], yd[0], xd[1], yd[1], out phi);
                    this["直线长度mm"] = Length;
                    this["平行线夹角"] = phi.TupleDeg();
                    this["直线角弧度"] = phi;
                    this["直线中心Row"] = rowcenter;
                    this["直线中心Column"] = colcenter;
                    HOperatorSet.DistancePl(outRowst, outColst, hv_row1, hv_col1, hv_row2, hv_col2, out  dslength);
                  
                    Vision.Gen_arrow_contour_xld(out line, hv_row1, hv_col1, hv_row2, hv_col2);
                    if (ISMLine)
                    {   HTuple rowt = new HTuple();
                        HTuple colt = new HTuple();
                        HTuple distMM= halcon.GetCaliConstMM(dslength);
                        HTuple det = distMM.TupleGreaterEqualElem(MLineM);
                        for (int i = 0; i < det.Length; i++)
                        {
                            if (det[i]==1)
                            {
                                rowt.Append(outRowst[i]);
                                colt.Append(outColst[i]);
                            }
                        }
                        if (rowt.Length>0)
                        {
                            HOperatorSet.GenCrossContourXld(out HObject cross, rowt, colt, 60, 0);
                            HOperatorSet.GenRectangle2(out HObject hObject1, OutCentreRow, outCentreCol, phi,  distance/2, Vision.Instance.DilationRectangle1);
                            oneResultOBj.AddNGOBJ( this.Name,  "超差" + MLineM, hObject1, hObject1 );
                            if (IsDisObj)
                            {
                                halcon.AddMessageIamge(OutCentreRow, outCentreCol, this.Name + ".超差" + distMM.TupleMax());
                                halcon.AddOBJ(cross,ColorResult.red);
                            }
                        }
                   
                        HOperatorSet.GenContourPolygonXld(out line, outRowst, outColst);
                    }
                }
                //halcon.SetAddObj(this.Name, line,this.colorEs.ToString());
            }
            catch (Exception ex)
            {
                halcon.ErrLog(ex);
            }
            return line;
        }

        public virtual HObject DrawObjPoint(HalconRun halcon)
        {
            HOperatorSet.DrawRectangle2(halcon.hWindowHalcon(), out drawRows, out drawCols, out HTuple hTuple, out DrawLength1, out DrawLength2);
            HOperatorSet.GenRectangle2(out HObject rectangle, DrawRows, DrawCols, hTuple, DrawLength1, DrawLength2);
            Draw_Phi = hTuple.TupleDeg();
            vision.Coordinate.CpointXY cpointXY = halcon.CoordinatePXY.GetPointXYtoRC(DrawRows.D, DrawCols.D);
            DrawCentY = cpointXY.Y;
            DrawCentX = cpointXY.X;
            this.InterpolationStr = Interpolation.nearest_neighbor;
            return rectangle;
        }

        /// <summary>
        /// 测量点类型
        /// </summary>
        /// <param name="halcon"></param>
        /// <returns></returns>
        public virtual HObject MeasureObjPoint(HObject image, HalconRun halcon)
        {
            HObject line = new HObject();
            line.GenEmptyObj();
            try
            {
                //产生测量对象句柄
                HOperatorSet.GenMeasureRectangle2(this["DrawRows"], this["DrawCols"], HomMatPhi.D, DrawLength1.D,
                        DrawLength2.D, halcon.Width, halcon.Height, InterpolationStr.ToString(),
                         out HTuple hv_MsrHandle_Measure);
                HOperatorSet.GenRectangle2(out HamMatDrawObj, this["DrawRows"], this["DrawCols"], HomMatPhi, DrawLength1, DrawLength2);
                //设置极性
                //设置边缘位置。最强点是从所有边缘中选择幅度绝对值最大点，需要设置为'all'
                //检测边缘
                HOperatorSet.MeasurePos(image, hv_MsrHandle_Measure, Sigma, Threshold,
                     TransitionStr.ToString(), SelectStr.ToString(), out outRows, out outCols,
                    out amplitude, out distance);
                //清除测量对象句柄
                HOperatorSet.CloseMeasure(hv_MsrHandle_Measure);

                for (int i = 0; this.MeasurePointNumber < outRows.Length; i++)
                {
                    for (int t = 0; t < Amplitude.Length; t++)
                    {
                        if (Amplitude.TupleAbs().TupleMin().D == Amplitude.TupleSelect(t).TupleAbs().D)
                        {
                            Amplitude = Amplitude.TupleRemove(t);
                            outRows = outRows.TupleRemove(t);
                            outCols = outCols.TupleRemove(t);
                        }
                    }
                }
                distance = new HTuple();
                for (int i = 0; i < outCols.Length - 1; i++)
                {
                    HOperatorSet.DistancePp(outRows[i], outCols[i], outRows[i + 1], outCols[i + 1], out HTuple dis);
                    distance.Append(dis);
                }
                outCentreRow = outRows;
                outCentreCol = outCols;
                this["拟合点Rows"] = outRows;
                this["拟合点Columns"] = outCols;
                if (IsRadius)
                {
                    halcon.AddMessage(halcon.GetCaliConstMM(distance));
                }
                this["测量距离mm"] = halcon.GetCaliConstMM(distance);
                HOperatorSet.GenCrossContourXld(out line, OutRows, OutCols, 10, 0);
                switch (this.measure_Type)
                {
                    case MeasureType.PointIXLD:
                        HTuple row = new HTuple();
                        HTuple col = new HTuple();
                        HTuple isovet = new HTuple();
                        HObject hObjecte = Vision.GenLine(this["DrawRows"], this["DrawCols"], HomMatPhi, DrawLength1);
                        HamMatDrawObj = HamMatDrawObj.ConcatObj(hObjecte);
                        for (int i = 0; i < halcon.MRModelHomMat.GetModeXld().CountObj(); i++)
                        {
                            HOperatorSet.SelectObj(halcon.MRModelHomMat.GetModeXld(), out HObject hObject1, i + 1);
                            HOperatorSet.IntersectionContoursXld(hObject1, hObjecte, "mutual", out row, out col, out isovet);
                            if (row.Length > 0)
                            {
                                break;
                            }
                        }
                        HOperatorSet.GenCrossContourXld(out HObject hObject, row, col, 10, 0);
                        line = line.ConcatObj(hObject);

                        distance = new HTuple();
                        for (int i = 0; i < row.Length; i++)
                        {
                            HOperatorSet.DistancePp(row[i], col[i], OutRows, OutCols, out HTuple hTuple);
                            distance.Append(hTuple);
                        }
                        break;

                    case MeasureType.Point2D:
                        this["测量坐标X"] = new HTuple();
                        this["测量坐标Y"] = new HTuple();

                        for (int i = 0; i < outRows.Length; i++)
                        {
                            Coordinate.CpointXY cpointXY = halcon.CoordinatePXY.GetPointRctoYX(outRows[i], outCols[i]);
                            this["测量坐标X"] = this["测量坐标X"].TupleConcat(cpointXY.X);
                            this["测量坐标Y"] = this["测量坐标Y"].TupleConcat(cpointXY.Y);
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                halcon.GetOneImageR().AddMeassge("测量错误:" + ex.Message);
            }
            return line;
        }
        /// <summary>
        /// 测量顶点
        /// </summary>
        /// <param name="halcon"></param>
        /// <returns></returns>
        public virtual HObject MeasureObjPeak(HObject image, HalconRun halcon)
        {

            XLD.GenEmptyObj();
            vision.Vision.Peak(image, this["DrawRows"], this["DrawCols"], HomMatPhi.D, DrawLength1.D,
                        DrawLength2.D, this.Measure_Waigth, this.Sigma, this.Threshold, this.TransitionStr.ToString(), this.SelectStr.ToString(),
                        out this.outRows, out this.outCols, out this.outCentreRow, out this.outCentreCol);
            HOperatorSet.GenCrossContourXld(out NGRoi, this.OutCentreRow, this.outCentreCol, 60, 0);

            this["拟合点Rows"] = outRows;
            this["拟合点Columns"] = outCols;
            return NGRoi;
        }
        /// <summary>
        /// 测量XLD
        /// </summary>
        /// <param name="halcon"></param>
        /// <returns></returns>
        public virtual HObject MeasureXLD(HalconRun halcon)
        {
            HObject line = new HObject();
            line.GenEmptyObj();
            HObject hObjectdts = new HObject();
            hObjectdts.GenEmptyObj();
            try
            {
                HTuple dismin = new HTuple();
                HTuple dismax = new HTuple();
                HObject hObjectdt = new HObject();
                HTuple rows1 = new HTuple();
                HTuple cols1 = new HTuple();
                HTuple lengths = new HTuple();
                HTuple dints = new HTuple();
                HTuple rows = new HTuple();
                HTuple cols = new HTuple();
                HTuple Listrows = new HTuple();
                HTuple Listcols = new HTuple();
                HObject xldMdt = new HObject();
                xldMdt.GenEmptyObj();
                switch (this.measure_Type)
                {
                    case MeasureType.XLDToXLD:
                        //int d = halcon.TKHobject[this["XLD测量目标名称"]].CountObj();

                        //for (int i2 = 0; i2 < d; i2++)
                        //{
                        //    HOperatorSet.SelectObj(halcon.TKHobject[this["XLD测量目标名称"]], out xldMdt, i2 + 1);
                        //    HOperatorSet.GetContourXld(xldMdt, out rows, out cols);
                        //    if (rows.Length > 2)
                        //    {
                        //        break;
                        //    }
                        //}

                        //for (int i = 0; i < halcon.TKHobject[this["XLD测量模板名称"]].CountObj(); i++)
                        //{
                        //    HOperatorSet.SelectObj(halcon.TKHobject[this["XLD测量模板名称"]], out hObjectdt, i + 1);
                        //    HOperatorSet.DistanceCcMin(hObjectdt, xldMdt, "point_to_segment", out dismin);
                        //    dismax.Append(dismin);
                        //}
                        //int dsds = dismax.TupleFindLast(dismax.TupleMin());
                        //HOperatorSet.SelectObj(halcon.TKHobject[this["XLD测量模板名称"]], out hObjectdt, dsds + 1);
                        HOperatorSet.GetContourXld(hObjectdt, out HTuple rows2, out HTuple cols2);
                        for (int i = 0; i < rows2.Length; i++)
                        {
                            HOperatorSet.LinePosition(rows2[i], cols2[i], rows[i], cols[i], out HTuple rowcenter, out HTuple colcenter, out HTuple length, out HTuple phi);
                            lengths.Append(length);
                        }
                        this["测量坐标X"] = rows;
                        this["测量坐标Y"] = cols;

                        this["目标坐标X"] = rows2;
                        this["目标坐标Y"] = cols2;
                        this["方向偏差row"] = halcon.GetCaliConstMM(rows2.TupleSub(rows));
                        this["方向偏差col"] = halcon.GetCaliConstMM(cols2.TupleSub(cols));
                        this["测量坐标数量"] = lengths.Length;
                        this.outRows.Append(this["方向偏差row"]);
                        this.outCols.Append(this["方向偏差col"]);
                        this.distance = lengths;
                        this["测量轮廓坐标最大距离"] = halcon.GetCaliConstMM(lengths.TupleMax());
                        lengths = halcon.GetCaliConstMM(lengths);
                        for (int i = 0; i < lengths.Length; i++)
                        {
                            if (lengths[i].D > 0.2)
                            {
                                HOperatorSet.GenEllipse(out hObjectdt, rows[i], cols[i], 0, 20, 10);
                                hObjectdts = hObjectdts.ConcatObj(hObjectdt);
                            }
                        }
                        break;

                    case MeasureType.XLDPointInXLDPoint:

                        break;

                    case MeasureType.XLDIntersectionXLD:
                        HObject hObjecte = Vision.GenLine(this["DrawRows"], this["DrawCols"], HomMatPhi, DrawLength1);
                        HamMatDrawObj = hObjecte;
                        //if (!this.IsExist("测量XLD名称1"))
                        //{
                        //    if (halcon.TKHobject.DirectoryHObject.ContainsKey(this.Name))
                        //    {
                        //        halcon.TKHobject.DirectoryHObject.Remove(this.Name);
                        //    }
                        //    MeasureXLDIntersectionXLD(hObjecte, halcon.GetObj(), out rows1, out cols1);
                        //    for (int i = 0; i < rows1.Length - 1; i++)
                        //    {
                        //        HOperatorSet.DistancePp(rows1[i], cols1[i], rows1[i + 1], cols1[i + 1], out dints);
                        //        this.distance.Append(dints);
                        //    }
                        //}
                        //else
                        //{
                        //    MeasureXLDIntersectionXLD(hObjecte, halcon.TKHobject[this["测量XLD名称2"]], out rows1, out cols1);

                        //    //MeasureXLDIntersectionXLD(hObjecte, halcon.TKHobject[this["测量XLD名称2"]], out rows, out cols);

                        //    if (rows1.Length == 1)
                        //    {
                        //        for (int i = 0; i < halcon.TKHobject[this["测量XLD名称1"]].CountObj(); i++)
                        //        {
                        //            HOperatorSet.SelectObj(halcon.TKHobject[this["测量XLD名称1"]], out HObject hObject, i + 1);

                        //            HOperatorSet.DistancePc(hObject, rows1, cols1, out HTuple minst, out HTuple dimax);
                        //            dints.Append(minst);
                        //        }

                        //        //HOperatorSet.DistancePp(rows1, cols1, rows, cols, out dints);
                        //        //cols1.Append(cols);
                        //        //rows1.Append(rows);
                        //        this.distance = dints.TupleMin();
                        //    }
                        //    else
                        //    {
                        //         halcon.GetOneImageR().AddMeassge(this.Name + "测量错误XLD点数量不同");
                        //    }
                        //}

                        outCols = cols1;
                        OutRows = rows1;
                        HOperatorSet.GenCrossContourXld(out line, rows1, cols1, 15, 0);

                        break;
                }
                hObjectdt.Dispose();
                halcon.AddOBJ(hObjectdts);
            }
            catch (Exception ex)
            {
                halcon.GetOneImageR().AddMeassge(this.Name + "测量错误:" + ex.Message);
            }
            return line;
        }

        [Bindable(false), Browsable(false)]
        /// <summary>
        ///
        /// </summary>
        public MeasureParameter MeasurePa { get { return measure; } set { measure = value; } }

        private MeasureParameter measure = new MeasureParameter(new HTuple());

        /// <summary>
        /// 查找相同xld并测量点位坐标未完成
        /// </summary>
        /// <param name="xld1">xld1</param>
        /// <param name="xld2">xld2</param>
        /// <param name="rows1"></param>
        /// <param name="cols1"></param>
        /// <param name="rows2"></param>
        /// <param name="cols2"></param>
        public static void MeasureXLDtoXLD(HObject xld1, HObject xld2, out HTuple rows1, out HTuple cols1, out HTuple rows2, out HTuple cols2)
        {
            HTuple dismin = new HTuple();
            HTuple dismax = new HTuple();
            HObject hObjectdt = new HObject();
            rows1 = new HTuple();
            cols1 = new HTuple();
            rows2 = new HTuple();
            cols2 = new HTuple();
            HTuple lengths = new HTuple();
            try
            {
                int d = xld1.CountObj();
                for (int i = 0; i < d; i++)
                {
                    HOperatorSet.GetContourXld(xld1, out rows1, out cols1);
                }
                for (int i = 0; i < xld2.CountObj(); i++)
                {
                    HOperatorSet.SelectObj(xld2, out hObjectdt, i + 1);
                    HOperatorSet.DistanceCcMin(hObjectdt, xld1, "point_to_segment", out dismin);
                    dismax.Append(dismin);
                }
                int dsds = dismax.TupleFindLast(dismax.TupleMin());
                HOperatorSet.SelectObj(xld2, out hObjectdt, dsds + 1);
                HOperatorSet.GetContourXld(hObjectdt, out rows2, out cols2);
            }
            catch (Exception)
            {
            }
            hObjectdt.Dispose();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xld1"></param>
        /// <param name="xld2"></param>
        /// <param name="rows1"></param>
        /// <param name="cols1"></param>
        /// <param name="rows2"></param>
        /// <param name="cols2"></param>
        public void MeasureXLDPointToXLDPoint(HObject xld1, HObject xld2, out HTuple rows1, out HTuple cols1, out HTuple rows2, out HTuple cols2)
        {
            HTuple dismin = new HTuple();
            HTuple dismax = new HTuple();
            HObject hObjectdt = new HObject();
            rows1 = new HTuple();
            cols1 = new HTuple();
            rows2 = new HTuple();
            cols2 = new HTuple();
            HTuple lengths = new HTuple();
            try
            {
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xld1"></param>
        /// <param name="xld2"></param>
        /// <param name="rows1"></param>
        /// <param name="cols1"></param>
        public void MeasureXLDIntersectionXLD(HObject xld1, HObject xld2, out HTuple rows1, out HTuple cols1)
        {
            HTuple disRows = new HTuple();
            HTuple discols = new HTuple();

            HObject hObjectdt = new HObject();
            rows1 = new HTuple();
            cols1 = new HTuple();
            HTuple dismin = new HTuple();
            HTuple dismins = new HTuple();
            try
            {
                int sd = xld1.CountObj();
                if (sd == 1)
                {
                    sd = xld2.CountObj();
                    for (int i = 0; i < xld2.CountObj(); i++)
                    {
                        HOperatorSet.SelectObj(xld2, out hObjectdt, i + 1);
                        HOperatorSet.IntersectionContoursXld(xld1, hObjectdt, "mutual", out disRows, out discols, out dismin);
                        if (rows1.TupleEqual(disRows) == 0)
                        {
                            rows1.Append(disRows);
                            cols1.Append(discols);
                            dismins.Append(dismin);
                        }
                        //HTup
                    }
                }
                else
                {
                    throw new Exception("XLD1数量错误");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public override void Dispose()
        {
            if (DrawHObject != null)
            {
                DrawHObject.Dispose();
            }

            base.Dispose();
        }
        /// <summary>
        /// 检测方向
        /// </summary>
        public enum Transition
        {   /// <summary>
            /// 全部
            /// </summary>
            all = 0,

            /// <summary>
            /// 黑到白，阴
            /// </summary>
            negative = 1,

            /// <summary>
            /// 白到黑，阳
            /// </summary>
            positive = 2,
        }

        /// <summary>
        /// 筛选全部，第一点，最后一点
        /// </summary>
        public enum Select
        {/// <summary>
         /// 最强点
         /// </summary>
            all = 0,

            /// <summary>
            /// 第一点
            /// </summary>
            first = 1,

            /// <summary>
            /// 最后点
            /// </summary>
            last = 2,
        }

        /// <summary>
        /// 插值类型
        /// </summary>
        public enum Interpolation
        {   /// <summary>
            /// 最近邻
            /// </summary>
            nearest_neighbor = 0,

            /// <summary>
            /// 两次立方
            /// </summary>
            bicubic = 2,

            /// <summary>
            /// 双线性的
            /// </summary>
            bilinear = 1,
        }

        /// <summary>
        /// 测量类型
        /// </summary>
        public enum MeasureType
        {
            /// <summary>
            /// 为指定的测量类型
            /// </summary>
            Measure = 0,

            /// <summary>
            /// 测量圆并拟合
            /// </summary>
            Cilcre = 1,

            /// <summary>
            /// 测量并拟合直线
            /// </summary>
            Line = 2,

            /// <summary>
            /// 测量顶点
            /// </summary>
            Pake = 3,

            /// <summary>
            /// 测量点位置
            /// </summary>
            Point = 4,

            /// <summary>
            /// 2D坐标上的测量，在指定坐标上测量偏差
            /// </summary>
            Point2D = 5,

            /// <summary>
            /// 测量点到XLD的距离
            /// </summary>
            PointIXLD = 6,

            /// <summary>
            /// 测量XLD到XLD的距离
            /// </summary>
            XLDToXLD = 7,

            /// <summary>
            /// 测量XLD定点与XLD定点的距离
            /// </summary>
            XLDPointInXLDPoint = 8,

            /// <summary>
            /// 测量XLD定点与XLD定点的距离
            /// </summary>
            XLDIntersectionXLD = 9,
            /// <summary>
            /// 自由曲线
            /// </summary>
            NurbsMeasure = 10,
        }

        /// <summary>
        /// 测量参数
        /// </summary>
        public class MeasureParameter
        {
            public MeasureParameter(params HTuple[] hTuple)
            {
            }
        }
    }

}