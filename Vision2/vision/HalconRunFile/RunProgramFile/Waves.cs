using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.vision.HalconRunFile.Controls;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    [System.Runtime.InteropServices.Guid("AC6A3902-2C4B-4CBA-81B6-FA66F18E18CA")]
    public class Waves : RunProgram
    {

        public override Control GetControl( HalconRun halconRun)
        {
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        private HTuple hv_HomMat2DRotate = new HTuple(), hv_Qx = new HTuple(), hv_Qy = new HTuple();

        /// <summary>
        /// 中心点
        /// </summary>
        public HTuple hv_RowProj1 { get; set; } = new HTuple();

        public HTuple hv_ColProj1 { get; set; } = new HTuple();

        [DescriptionAttribute("原点补偿。"), Category("原点处理")]
        public bool IsDis { get; set; }

        public static string constStrPath = Application.StartupPath + "/Vision/Waves/";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="data"></param>
        public new void UpProperty(PropertyForm control, object data)
        {
            base.UpProperty(control, data);
            HalconRun halconRun = null;
            TreeNode Current = data as TreeNode;
            if (Current != null)
            {
                TreeNode CurrentNodeP = Current.Parent;
                if (CurrentNodeP != null)
                {
                stru:
                    if (CurrentNodeP != null)
                    {
                        halconRun = CurrentNodeP.Tag as HalconRun;
                        if (halconRun == null)
                        {
                            CurrentNodeP = CurrentNodeP.Parent;
                            goto stru;
                        }
                    }
                }
            }
            TabPage tab = control.Controls.Find("tabPage1", true)[0] as TabPage;
            if (halconRun != null)
            {
                ElementMeasureControl1 measureConTrolEx =
                    new ElementMeasureControl1(this, halconRun);
                if (tab != null)
                {
                    int dint = control.Controls.IndexOfKey(measureConTrolEx.Name);
                    if (dint > 0)
                    {
                        measureConTrolEx = control.Controls[dint] as ElementMeasureControl1;
                    }
                    else
                    {
                        control.Controls.Add(measureConTrolEx);
                        measureConTrolEx.Dock = DockStyle.Top;
                    }
                }
                measureConTrolEx.Up();
            }
        }


        public Waves()
        {
            Type = GetType().ToString();
        }

        public Waves(string path) : this()
        {
            this.ReadData(path);
        }

        public void ReadData(string path)
        {
        }

        public void WriteThis()
        {
            HalconRun.ClassToJsonSavePath(this, constStrPath + "This");
        }

        public static bool WrtieObject(HObject ho_Line, string name)
        {
            Directory.CreateDirectory(constStrPath);
            HOperatorSet.WriteObject(ho_Line, constStrPath + name);
            //MessageBox.Show(name + "，保存成功");
            return false;
        }

        public static bool ReadData(string name, out HObject ho_Line)
        {
            ho_Line = null;
            try
            {
                HOperatorSet.ReadObject(out ho_Line, constStrPath + name);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        private HObject ho_Image2 = new HObject();

        /// <summary>
        /// 保存测量数据文件名
        /// </summary>
        public string SaveDataFileName = "";

        public string SaveDataPath { get { return Application.StartupPath + "\\测量数据\\" + SaveDataFileName; } }

        private HObject ho_ImageMedian = new HObject();

        /// <summary>
        /// 接口方法
        /// </summary>
        /// <param name="hImage"></param>
        /// <returns></returns>
        public override bool RunHProgram( OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj)
        {
            oneRObjs = new List<OneRObj>();
            try
            {
                hv_ColProj1 = new HTuple();
                hv_RowProj1 = new HTuple();
                HTuple hTuple = new HTuple();

                int d = 0;
                if (!IsDis)
                {
                    foreach (var item in this.Dic_Measure.Keys_Measure)
                    {
                        d = Vision2.ErosProjcetDLL.Project.ProjectINI.GetStrReturnToInt(item.Value.Name);
                        item.Value.DrawLength1 = 30;
                        item.Value.ISMatHat = true;
                        item.Value.Enabled = true;
                        if (d <= 9)
                        {
                            item.Value["测量XLD名称1"] = "模板程序1";
                            item.Value["测量XLD名称2"] = "模板名称";
                        }
                        else
                        {
                            item.Value["测量XLD名称1"] = "模板程序2";
                            item.Value["测量XLD名称2"] = "模板名称";
                        }
                        item.Value.SetSave("测量XLD名称2");
                        item.Value.SetSave("测量XLD名称1");
                        item.Value.Distance = 0;
                    }
                }


                for (int i = 1; i < 26; i++)
                {
                    hTuple = new HTuple();
                    HOperatorSet.TupleGenConst(8, 0.0, out hTuple);
                    this["距离值" + i] = hTuple;
                }
                HTuple DistanceMin1S = new HTuple();
                int NG = 0;
                string sd = string.Empty;

                int dt = 0;

                foreach (var item in this.Dic_Measure.Keys_Measure)
                {
                    HTuple DistanceMin1 = new HTuple();
                    HTuple dismints = new HTuple();
                    try
                    {
                        if (item.Value.OutRows.Length >= 1)
                        {
                            d = Vision2.ErosProjcetDLL.Project.ProjectINI.GetStrReturnToInt(item.Value.Name);
                            dt = Vision2.ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(item.Value.Name);
                            this["距离值" + d][dt - 1] = item.Value.Distance.TupleSelect(0).TupleMult(oneResultOBj.CoordinatePXY.Scale);
                            if (item.Value.Distance.TupleMult(oneResultOBj.CoordinatePXY.Scale).D >= this["0.4标准"].D)
                            {
                                NG++;
                                oneResultOBj.AddImageMassage(item.Value.OutRows[0], item.Value.OutCols[0], d + "." + dt);
                                HOperatorSet.GenCircle(out HObject circle, item.Value.OutRows[0], item.Value.OutCols[0], item.Value.Distance[0]);
                                oneResultOBj.AddObj(circle);
                            }
                        }
                        else
                        {
                            if (item.Value.Enabled)
                            {
                                oneResultOBj.AddMeassge(item.Key + ":未测量到点");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ErrBool = true;
                        this.LogErr(this.Name + item.Key, ex);
                    }
                }
                int rets = 0;
                string datastr = "";
                for (int it = 1; it < 26; it++)
                {
                    for (int i = 0; i < this["距离值" + it].Length; i++)
                    {
                        datastr += (i + 1) + ":" + this["距离值" + it].TupleSelect(i).TupleString("0.3f") + ",";
                        if (this["距离值" + it].TupleSelect(i).TupleEqual(0))
                        {
                            rets++;
                        }
                    }
                    oneResultOBj.AddMeassge(it + "." + "(" + datastr.TrimEnd(',') + ")");
                    datastr = "";
                }
                //halcon.WriteData.Append(halcon["距离值1"]);
                //halcon.WriteData.Append(halcon["距离值2"]);
                this["NG数量"] = NG;
                //halcon.SetDefault("0.5标准", 0.3);
                oneResultOBj.AddMeassge(sd);
                this.SetDefault("0.4数量", 2);
                if (!this["0.4数量"].TupleGreater(NG))
                {
                    this["0.4结果"] = "NG";
                }
                else
                {
                    this["0.4结果"] = "OK";
                }
                if (this["0.4结果"].ToString().Contains("OK") && this["0.2结果"].ToString().Contains("OK"))
                {
                   
                    //oneResultOBj.Result = "OK";
                }
                else
                {
                    NGNumber++;
                    /*    halcon.Result = "NG"*/
                    ;
                }
                if (rets == (this["距离值1"].Length + this["距离值2"].Length))
                {
                    NGNumber++;
                    //halcon.Result = "NG";
                }

                return true;

            }
            catch (Exception ex)
            {
                this.LogErr(this.Name, ex);
                return false;
            }
        }

        private System.Diagnostics.Stopwatch watchts = new System.Diagnostics.Stopwatch();



        
        public void ShowMesager(HalconRun halcon)
        {
            NGRoi.GenEmptyObj();
            foreach (var item in this.Dic_Measure.Keys_Measure)
            {
                NGRoi = NGRoi.ConcatObj(item.Value.MeasureObj(halcon,halcon.GetOneImageR())._HObject);
            }
            halcon.AddObj(NGRoi);
        }


        // Short Description: 测量边缘输出边缘数组点
        public void rake(HObject ho_Image, out HObject ho_Regions, HTuple hv_Elements,
           HTuple hv_DetectHeight, HTuple hv_DetectWidth, HTuple hv_Sigma, HTuple hv_Threshold,
           HTuple hv_Transition, HTuple hv_Select, HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2,
           HTuple hv_Column2, out HTuple hv_ResultRow, out HTuple hv_ResultColumn)
        {
            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            // Local iconic variables
            HObject ho_RegionLines, ho_Rectangle = null;
            HObject ho_Arrow1 = null, ho_Cross;
            // Local control variables
            HTuple hv_Width = null, hv_Height = null, hv_ATan = null;
            HTuple hv_tRow = null, hv_tCol = null, hv_t = null, hv_i = null;
            HTuple hv_RowC = new HTuple(), hv_ColC = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_RowL2 = new HTuple();
            HTuple hv_RowL1 = new HTuple(), hv_ColL2 = new HTuple();
            HTuple hv_ColL1 = new HTuple(), hv_MsrHandle_Measure = new HTuple();
            HTuple hv_RowEdge = new HTuple(), hv_ColEdge = new HTuple();
            HTuple hv_Amplitude = new HTuple(), hv_Number = new HTuple();
            HTuple hv_j = new HTuple();
            HTuple hv_DetectWidth_COPY_INP_TMP = hv_DetectWidth.Clone();
            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            //获取图像尺寸
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            //产生一个空显示对象，用于显示
            ho_Regions.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Regions);
            //初始化边缘坐标数组
            hv_ResultRow = new HTuple();
            hv_ResultColumn = new HTuple();
            //产生直线xld
            ho_RegionLines.Dispose();
            HOperatorSet.GenContourPolygonXld(out ho_RegionLines, hv_Row1.TupleConcat(hv_Row2),
                hv_Column1.TupleConcat(hv_Column2));
            //存储到显示对象
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_Regions, ho_RegionLines, out ExpTmpOutVar_0);
                ho_Regions.Dispose();
                ho_Regions = ExpTmpOutVar_0;
            }
            //计算直线与x轴的夹角，逆时针方向为正向。
            HOperatorSet.AngleLx(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_ATan);
            //边缘检测方向垂直于检测直线：直线方向正向旋转90°为边缘检测方向
            hv_ATan = hv_ATan + ((new HTuple(90)).TupleRad());
            //临时变量初始化
            //tRow，tCol保存找到指定边缘的坐标
            hv_tRow = 0;
            hv_tCol = 0;
            //t保存边缘的幅度绝对值
            hv_t = 0;
            //根据检测直线按顺序产生测量区域矩形，并存储到显示对象
            HTuple end_val22 = hv_Elements;
            HTuple step_val22 = 1;
            for (hv_i = 1; hv_i.Continue(end_val22, step_val22); hv_i = hv_i.TupleAdd(step_val22))
            {
                //如果只有一个测量矩形，作为卡尺工具，宽度为检测直线的长度
                if ((int)(new HTuple(hv_Elements.TupleEqual(1))) != 0)
                {
                    hv_RowC = (1.0 * (hv_Row1 + hv_Row2)) * 0.5;
                    hv_ColC = (1.0 * (hv_Column1 + hv_Column2)) * 0.5;
                    //判断是否超出图像,超出不检测边缘
                    if ((int)((new HTuple((new HTuple((new HTuple(hv_RowC.TupleGreater(hv_Height - 1))).TupleOr(
                        new HTuple(hv_RowC.TupleLess(0))))).TupleOr(new HTuple(hv_ColC.TupleGreater(
                        hv_Width - 1))))).TupleOr(new HTuple(hv_ColC.TupleLess(0)))) != 0)
                    {
                        continue;
                    }
                    HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_DetectWidth_COPY_INP_TMP);
                    //gen_rectangle2_contour_xld (Rectangle, RowC, ColC, ATan, 1.0*DetectHeight/2, 1.0*Distance/2)
                }
                else
                {
                    //如果有多个测量矩形，产生该测量矩形xld
                    hv_RowC = 1.0 * (hv_Row1 + (((hv_Row2 - hv_Row1) * (hv_i - 1)) / (hv_Elements - 1)));
                    hv_ColC = 1.0 * (hv_Column1 + (((hv_Column2 - hv_Column1) * (hv_i - 1)) / (hv_Elements - 1)));
                    //判断是否超出图像,超出不检测边缘
                    if ((int)((new HTuple((new HTuple((new HTuple(hv_RowC.TupleGreater(hv_Height - 1))).TupleOr(
                        new HTuple(hv_RowC.TupleLess(0))))).TupleOr(new HTuple(hv_ColC.TupleGreater(
                        hv_Width - 1))))).TupleOr(new HTuple(hv_ColC.TupleLess(0)))) != 0)
                    {
                        continue;
                    }
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                        hv_ATan, (1.0 * hv_DetectHeight) / 2, (1.0 * hv_DetectWidth_COPY_INP_TMP) / 2);
                }

                //把测量矩形xld存储到显示对象

                if ((int)(new HTuple(hv_i.TupleEqual(1))) != 0)
                {
                    //在第一个测量矩形绘制一个箭头xld，用于只是边缘检测方向
                    hv_RowL2 = hv_RowC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                    hv_RowL1 = hv_RowC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                    hv_ColL2 = hv_ColC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                    hv_ColL1 = hv_ColC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                    ho_Arrow1.Dispose();
                    gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2,
                        25, 25);
                    //把xld存储到显示对象
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_Regions, ho_Arrow1, out ExpTmpOutVar_0);
                        ho_Regions.Dispose();
                        ho_Regions = ExpTmpOutVar_0;
                    }
                }
                //产生测量对象句柄
                HOperatorSet.GenMeasureRectangle2(hv_RowC, hv_ColC, hv_ATan, hv_DetectHeight / 2,
                    hv_DetectWidth_COPY_INP_TMP / 2, hv_Width, hv_Height, "bilinear", out hv_MsrHandle_Measure);
                //检测边缘
                HOperatorSet.MeasurePos(ho_Image, hv_MsrHandle_Measure, hv_Sigma, hv_Threshold,
                    hv_Transition, hv_Select, out hv_RowEdge, out hv_ColEdge, out hv_Amplitude,
                    out hv_Distance);
                //清除测量对象句柄
                HOperatorSet.CloseMeasure(hv_MsrHandle_Measure);
                //找到的边缘必须至少为1个
                HOperatorSet.TupleLength(hv_RowEdge, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleLess(1))) != 0)
                {
                    continue;
                }
                hv_t = 0;
                //有多个边缘时，选择幅度绝对值最大的边缘
                HTuple end_val69 = hv_Number - 1;
                HTuple step_val69 = 1;
                for (hv_j = 0; hv_j.Continue(end_val69, step_val69); hv_j = hv_j.TupleAdd(step_val69))
                {
                    if ((int)(new HTuple(((((hv_Amplitude.TupleSelect(hv_j))).TupleAbs())).TupleGreater(
                        hv_t))) != 0)
                    {
                        hv_tRow = hv_RowEdge.TupleSelect(hv_j);
                        hv_tCol = hv_ColEdge.TupleSelect(hv_j);
                        hv_t = ((hv_Amplitude.TupleSelect(hv_j))).TupleAbs();
                        continue;
                    }
                }
                //把找到的边缘保存在输出数组
                if ((int)(new HTuple(hv_t.TupleGreater(0))) != 0)
                {
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_tRow, hv_tCol, 26, 0);
                    hv_ResultRow = hv_ResultRow.TupleConcat(hv_tRow);
                    hv_ResultColumn = hv_ResultColumn.TupleConcat(hv_tCol);
                }
            }
            ho_Cross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_ResultRow, hv_ResultColumn,
                26, 0);

            ho_RegionLines.Dispose();
            ho_Rectangle.Dispose();
            ho_Arrow1.Dispose();
            ho_Cross.Dispose();

            return;
        }

        // Chapter: XLD / Creation
        // Short Description: Creates an arrow shaped XLD contour.
        public void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
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

            return;
        }

        // Procedures
        // Local procedures
        // Short Description: 拟合波浪
        /// <summary>
        /// : 拟合波浪
        /// </summary>
        /// <param name="ho_image"></param>
        /// <param name="ho_XLD"></param>
        /// <param name="hv_Elements"></param>
        /// <param name="hv_process"></param>
        /// <param name="hv_Length2"></param>
        /// <param name="hv_StartRow"></param>
        /// <param name="hv_StartCol"></param>
        /// <param name="hv_EndRow"></param>
        /// <param name="hv_EndCol"></param>
        /// <param name="hv_EdgeWidth"></param>
        /// <param name="hv_PeaksRow"></param>
        /// <param name="hv_PeaksCol"></param>
        /// <param name="hv_pitsRow"></param>
        /// <param name="hv_pitsCol"></param>
        /// <param name="hv_pointsRow">中心点R</param>
        /// <param name="hv_pointsCol">中心点C</param>
        /// <returns></returns>
        public bool waves(HObject ho_image, out HObject ho_XLD, out HObject out_XLD, HTuple hv_Elements,
             HTuple hv_Length2, HTuple hv_StartRow, HTuple hv_StartCol, HTuple hv_EndRow,
            HTuple hv_EndCol, HTuple hv_EdgeWidth, out HTuple hv_PeaksRow, out HTuple hv_PeaksCol,
            out HTuple hv_pitsRow, out HTuple hv_pitsCol, out HTuple hv_pointsRow, out HTuple hv_pointsCol)
        {
            hv_PeaksRow = hv_PeaksCol = hv_pointsCol = hv_pointsRow = hv_pitsCol = hv_pitsRow = new HTuple();

            HObject ho_Arrow1, ho_Cross = null, ho_Cross2;
            HObject ho_Line = null, ho_Cross1 = null, ho_Rectangle2, ho_Rectangle1 = null;
            HObject ho_Line1 = new HObject();
            ho_Line1.GenEmptyObj();
            HObject ho_Circle = null, ho_Rectangle = null;

            HTuple hv_RowCenter = null, hv_ColCenter = null;
            HTuple hv_Length1 = null, hv_Phi = null, hv_length = null;
            HTuple hv_HomMat2D = null, hv_RowTrans = null, hv_ColTrans = null;
            HTuple hv_Col1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Col2 = new HTuple(), hv_ATan = null, hv_RowC = null;
            HTuple hv_ColC = null, hv_RowC3 = null, hv_ColC3 = null;
            HTuple hv_EdgeRows1 = null, hv_EdgeColumns1 = null, hv_RowL2 = null;
            HTuple hv_RowL1 = null, hv_ColL2 = null, hv_ColL1 = null;
            HTuple hv_Index = null;
            HTuple hv_EdgeRows = new HTuple(), hv_EdgeColumns = new HTuple();
            HTuple hv_ResultRow2 = new HTuple(), hv_ResultColumn2 = new HTuple();
            HTuple hv_ResultRow4 = new HTuple(), hv_ResultColumn4 = new HTuple();
            HTuple hv_strint = new HTuple(), hv_endint = new HTuple();
            HTuple hv_Row12 = new HTuple(), hv_Col11 = new HTuple();
            HTuple hv_Row22 = new HTuple(), hv_Col21 = new HTuple();
            HTuple hv_ResultRow1 = new HTuple(), hv_ResultColumn1 = new HTuple();
            HTuple hv_ResultRow3 = new HTuple(), hv_ResultColumn3 = new HTuple();
            HTuple hv_ArcType = new HTuple(), hv_RowCenter1 = new HTuple();
            HTuple hv_ColCenter1 = new HTuple(), hv_Radius = new HTuple();
            HTuple hv_ResultRow = new HTuple(), hv_ResultColumn = new HTuple();
            HTuple hv_Row11 = new HTuple();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_XLD);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Rectangle2);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);

            hv_PeaksRow = new HTuple();
            hv_PeaksCol = new HTuple();
            HTuple hTupleLinex = new HTuple();
            HTuple hTupleLiney = new HTuple();
            out_XLD = new HObject();
            out_XLD.GenEmptyObj();
            try
            {
                HOperatorSet.LinePosition(hv_StartRow, hv_StartCol, hv_EndRow, hv_EndCol, out hv_RowCenter,
                out hv_ColCenter, out hv_Length1, out hv_Phi);
                hv_length = (hv_Length1 / hv_Elements) / 4;

                //计算直线与x轴的夹角，逆时针方向为正向。
                HOperatorSet.AngleLx(hv_StartRow, hv_StartCol, hv_EndRow, hv_EndCol, out hv_ATan);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_StartRow, hv_StartCol, hv_ATan + ((new HTuple(180)).TupleRad()
                    ), out hv_HomMat2D);
                HOperatorSet.AffineTransPixel(hv_HomMat2D, 0, (-hv_length * 2.5), out hv_RowTrans,
                    out hv_ColTrans);
                //边缘检测方向垂直于检测直线：直线方向正向旋转90°为边缘检测方向
                hv_ATan = hv_ATan + ((new HTuple(90)).TupleRad());
                //*定义第一个点
                hv_RowC = 1.0 * (hv_StartRow + (((hv_EndRow - hv_StartRow) * 0) / (hv_Elements - 1)));
                hv_ColC = 1.0 * (hv_StartCol + (((hv_EndCol - hv_StartCol) * 0) / (hv_Elements - 1)));

                hv_RowC3 = 1.0 * (hv_RowTrans + (((hv_EndRow - hv_RowTrans) * -1) / (hv_Elements - 1)));
                hv_ColC3 = 1.0 * (hv_ColTrans + (((hv_EndCol - hv_ColTrans) * -1) / (hv_Elements - 1)));

                HOperatorSet.GenRectangle2(out ho_Rectangle2, hv_RowC3, hv_ColC3, hv_ATan, hv_Length2,
                    hv_length);

                ho_XLD = ho_XLD.ConcatObj(ho_Rectangle2);
                peak(ho_image, hv_RowC3, hv_ColC3, hv_ATan + ((new HTuple(180)).TupleRad()), hv_Length2,
                hv_length, 8, 4, 48, "all", "last", out hv_EdgeRows1, out hv_EdgeColumns1,
                out hv_pitsRow, out hv_pitsCol);
                if (hv_pitsRow.Length != 0)
                {
                    HOperatorSet.GenContourPolygonXld(out ho_Line, hv_EdgeRows1, hv_EdgeColumns1);
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_EdgeRows1, hv_EdgeColumns1, 6, 0);
                    HOperatorSet.GenCrossContourXld(out HObject hObject, hv_pitsRow, hv_pitsCol, 30, 0);
                    ho_XLD = ho_XLD.ConcatObj(hObject);
                    ho_XLD = ho_XLD.ConcatObj(ho_Cross);
                    ho_XLD = ho_XLD.ConcatObj(ho_Line);
                }
                else
                {
                    hv_pitsRow = hv_RowC3;
                    hv_pitsCol = hv_ColC3;
                }
                //在第一个测量矩形绘制一个箭头xld，用于只是边缘检测方向
                hv_RowL2 = hv_RowC3 + (hv_Length2 * (((-hv_ATan)).TupleSin()));
                hv_RowL1 = hv_RowC3 - (hv_Length2 * (((-hv_ATan)).TupleSin()));
                hv_ColL2 = hv_ColC3 + (hv_Length2 * (((-hv_ATan)).TupleCos()));
                hv_ColL1 = hv_ColC3 - (hv_Length2 * (((-hv_ATan)).TupleCos()));
                ho_Arrow1.Dispose();
                gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2, 25, 25);
                ho_XLD = ho_XLD.ConcatObj(ho_Arrow1);
                hv_PeaksRow = new HTuple();
                hv_PeaksCol = new HTuple();
                hv_pointsRow = hv_pitsRow;
                hv_pointsCol = hv_pitsCol;

                HTuple end_val42 = hv_Elements - 1;
                HTuple step_val42 = 1;
                for (hv_Index = 0; hv_Index.Continue(end_val42, step_val42); hv_Index = hv_Index.TupleAdd(step_val42))
                {
                    //**测量凸点
                    //如果有多个测量矩形，产生该测量矩形xld
                    hv_RowC = 1.0 * (hv_StartRow + (((hv_EndRow - hv_StartRow) * hv_Index) / (hv_Elements - 1)));
                    hv_ColC = 1.0 * (hv_StartCol + (((hv_EndCol - hv_StartCol) * hv_Index) / (hv_Elements - 1)));
                    ho_Rectangle1.Dispose();
                    HOperatorSet.GenRectangle2(out ho_Rectangle1, hv_RowC, hv_ColC, hv_ATan, hv_Length2,
                        hv_length / 1.5);
                    ho_XLD = ho_XLD.ConcatObj(ho_Rectangle1);
                    peak(ho_image, hv_RowC, hv_ColC, hv_ATan, hv_Length2, hv_length / 2, 2, this["凸点高斯因子"],
                        this["凸点幅度阈值"], "negative", "first", out hv_EdgeRows, out hv_EdgeColumns, out hv_ResultRow2,
                        out hv_ResultColumn2);

                    HOperatorSet.GenCrossContourXld(out HObject Cross4, hv_EdgeRows, hv_EdgeColumns, 6, 0);
                    ho_XLD = ho_XLD.ConcatObj(Cross4);
                    HOperatorSet.GenCrossContourXld(out Cross4, hv_ResultRow2, hv_ResultColumn2, 25, 0);
                    ho_XLD = ho_XLD.ConcatObj(Cross4);
                    HOperatorSet.GenContourPolygonXld(out ho_Line1, hv_EdgeRows, hv_EdgeColumns);
                    ho_XLD = ho_XLD.ConcatObj(ho_Line1);
                    //out_XLD = out_XLD.ConcatObj(ho_Line1);
                    hv_PeaksRow = hv_PeaksRow.TupleConcat(hv_ResultRow2);
                    hv_PeaksCol = hv_PeaksCol.TupleConcat(hv_ResultColumn2);
                    //if (hv_pitsCol.TupleSelect(hv_Index) == null)
                    //{
                    //    return false;
                    //}
                    //凹点和凸点的中心
                    if ((int)(new HTuple(hv_Index.TupleEqual(0))) == 0)
                    {
                        HOperatorSet.LinePosition(hv_pitsRow.TupleSelect(hv_Index), hv_pitsCol.TupleSelect(hv_Index), hv_PeaksRow.TupleSelect(
                        hv_Index), hv_PeaksCol.TupleSelect(hv_Index), out HTuple rowCenter, out HTuple colCenth, out HTuple Length, out HTuple phil);
                        HTuple hTupleR = hv_pitsRow.TupleSelect(hv_Index);
                        hTupleR.Append(hv_PeaksRow.TupleSelect(hv_Index));
                        HTuple hTupleC = hv_pitsCol.TupleSelect(hv_Index);
                        hTupleC.Append(hv_PeaksCol.TupleSelect(hv_Index));
                        HOperatorSet.GenContourPolygonXld(out ho_Line, hTupleR, hTupleC);
                        hTupleLinex.Append(rowCenter);
                        hTupleLiney.Append(colCenth);
                        ho_XLD = ho_XLD.ConcatObj(ho_Line);
                        out_XLD = out_XLD.ConcatObj(ho_Line);
                    }

                    //**测量凹点
                    hv_RowC3 = 1.0 * (hv_RowTrans + (((hv_EndRow - hv_RowTrans) * hv_Index) / (hv_Elements - 1.48)));
                    hv_ColC3 = 1.0 * (hv_ColTrans + (((hv_EndCol - hv_ColTrans) * hv_Index) / (hv_Elements - 1.48)));
                    HOperatorSet.GenRectangle2(out ho_Rectangle2, hv_RowC3, hv_ColC3, hv_ATan, hv_Length2, hv_length / 2);
                    ho_XLD = ho_XLD.ConcatObj(ho_Rectangle2);
                    peak(ho_image, hv_RowC3, hv_ColC3, hv_ATan + ((new HTuple(180)).TupleRad()),
                    hv_Length2, hv_length / 1, 2, this["凹点高斯因子"], this["凹点幅度阈值"], "negative", "first",
                    out hv_EdgeRows1, out hv_EdgeColumns1, out hv_ResultRow1, out hv_ResultColumn1);

                    //2,28
                    if (hv_Index.D != hv_Elements.D - 1)
                    {
                        //HOperatorSet.GenContourPolygonXld(out ho_Line, hv_EdgeRows1, hv_EdgeColumns1);
                        //ho_XLD = ho_XLD.ConcatObj(ho_Line);
                        //out_XLD = out_XLD.ConcatObj(ho_Line);
                    }
                    hv_pitsRow = hv_pitsRow.TupleConcat(hv_ResultRow1);
                    hv_pitsCol = hv_pitsCol.TupleConcat(hv_ResultColumn1);
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_EdgeRows1, hv_EdgeColumns1, 6, 0);
                    ho_XLD = ho_XLD.ConcatObj(ho_Cross);
                    HOperatorSet.GenCrossContourXld(out Cross4, hv_ResultRow1, hv_ResultColumn1, 25, 0);
                    ho_XLD = ho_XLD.ConcatObj(Cross4);
                    //凸点到凹点的中心
                    if ((int)(new HTuple(hv_Index.TupleEqual(hv_Elements - 1))) == 0)
                    {
                        HOperatorSet.LinePosition(hv_ResultRow2, hv_ResultColumn2, hv_ResultRow1, hv_ResultColumn1, out HTuple rowCenter, out HTuple colCenth, out HTuple Length, out HTuple phil);
                        hTupleLinex.Append(rowCenter);
                        hTupleLiney.Append(colCenth);
                        HTuple hTupleR = hv_ResultRow2;
                        hTupleR.Append(hv_ResultRow1);
                        HTuple hTupleC = hv_ResultColumn2;
                        hTupleC.Append(hv_ResultColumn1);
                        HOperatorSet.GenContourPolygonXld(out ho_Line, hTupleR, hTupleC);
                        ho_XLD = ho_XLD.ConcatObj(ho_Line);
                        out_XLD = out_XLD.ConcatObj(ho_Line);
                    }
                }

                //终端相连
                HOperatorSet.SortContoursXld(out_XLD, out HObject UnionContours, "upper_left", "true", "row");
                HOperatorSet.UnionAdjacentContoursXld(UnionContours, out UnionContours, 90, 10, "attr_keep");
                //HOperatorSet.UnionCotangentialContoursXld(UnionContours, out UnionContours, 10, 10, 50, 60, 2, 0.1, "attr_keep");
                out_XLD = UnionContours;
                HOperatorSet.GenCrossContourXld(out HObject ho_Cross3, hTupleLinex, hTupleLiney, 16,
                   0);
                //HOperatorSet.GetContourXld(ho_XLD, out HTuple rowc,out HTuple  colc);
                ho_XLD = ho_XLD.ConcatObj(ho_Cross3);
                hv_pointsRow = hTupleLinex;
                hv_pointsCol = hTupleLiney;

                HOperatorSet.GenCrossContourXld(out ho_Cross3, hv_PeaksRow, hv_PeaksCol, 16,
                    0);
                ho_XLD = ho_XLD.ConcatObj(ho_Cross3);
                HOperatorSet.GenCrossContourXld(out ho_Cross3, hv_pitsRow, hv_pitsCol, 45, 0);
                ho_XLD = ho_XLD.ConcatObj(ho_Cross3);

                ho_Arrow1.Dispose();
                ho_Cross.Dispose();
                ho_Cross2.Dispose();
                ho_Line.Dispose();
                ho_Cross1.Dispose();
                ho_Rectangle2.Dispose();
                ho_Rectangle1.Dispose();
                ho_Line1.Dispose();
                ho_Circle.Dispose();
                ho_Rectangle.Dispose();
            }
            catch (Exception ex)
            {
                hv_pointsRow = hTupleLinex;
                hv_pointsCol = hTupleLiney;
                HalconRun.StaticErrLog(ex);
                return false;
            }
            return true;
        }

  
        // Short Description: 测量顶点
        public void peak(HObject ho_Image, HTuple hv_Row, HTuple hv_Coloumn, HTuple hv_Phi,
            HTuple hv_Length1, HTuple hv_Length2, HTuple hv_DetectWidth, HTuple hv_Sigma,
            HTuple hv_Threshold, HTuple hv_Transition, HTuple hv_Select, out HTuple hv_EdgeRows,
            out HTuple hv_EdgeColumns, out HTuple hv_ResultRow, out HTuple hv_ResultColumn)
        {
            // Local iconic variables

            HObject ho_Rectangle, ho_Regions1;

            // Local control variables

            HTuple hv_ResultCol = null, hv_ROILineRow1 = null;
            HTuple hv_ROILineCol1 = null, hv_ROILineRow2 = null, hv_ROILineCol2 = null;
            HTuple hv_StdLineRow1 = null, hv_StdLineCol1 = null, hv_StdLineRow2 = null;
            HTuple hv_StdLineCol2 = null, hv_Cos = null, hv_Sin = null;
            HTuple hv_Col1 = null, hv_Row1 = null, hv_Col2 = null;
            HTuple hv_Row2 = null, hv_Col3 = null, hv_Row3 = null;
            HTuple hv_Col4 = null, hv_Row4 = null, hv_ResultRows = null;
            HTuple hv_ResultColumns = null, hv_Max = null, hv_i = new HTuple();
            HTuple hv_Distance1 = new HTuple();
            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Regions1);
            hv_ResultRow = hv_ResultColumn = new HTuple();

            hv_EdgeColumns = new HTuple();
            hv_EdgeRows = new HTuple();

            //仿射矩形Length2所在直线作为rake工具的ROI
            hv_ROILineRow1 = 0;
            hv_ROILineCol1 = 0;
            hv_ROILineRow2 = 0;
            hv_ROILineCol2 = 0;

            //仿射矩形方向所直线的边做基准线
            hv_StdLineRow1 = 0;
            hv_StdLineCol1 = 0;
            hv_StdLineRow2 = 0;
            hv_StdLineCol2 = 0;
            //判断仿射矩形是否有效
            if ((int)((new HTuple(hv_Length1.TupleLessEqual(0))).TupleOr(new HTuple(hv_Length2.TupleLessEqual(
                0)))) != 0)
            {
                ho_Rectangle.Dispose();
                ho_Regions1.Dispose();

                return;
            }

            //计算仿射矩形角度的正弦值、余弦值
            HOperatorSet.TupleCos(hv_Phi, out hv_Cos);
            HOperatorSet.TupleSin(hv_Phi, out hv_Sin);

            //矩形第一个端点坐标
            hv_Col1 = 1.0 * ((hv_Coloumn - (hv_Length1 * hv_Cos)) - (hv_Length2 * hv_Sin));
            hv_Row1 = 1.0 * (hv_Row - (((-hv_Length1) * hv_Sin) + (hv_Length2 * hv_Cos)));

            //矩形第二个端点坐标
            hv_Col2 = 1.0 * ((hv_Coloumn + (hv_Length1 * hv_Cos)) - (hv_Length2 * hv_Sin));
            hv_Row2 = 1.0 * (hv_Row - ((hv_Length1 * hv_Sin) + (hv_Length2 * hv_Cos)));

            //矩形第三个端点坐标
            hv_Col3 = 1.0 * ((hv_Coloumn + (hv_Length1 * hv_Cos)) + (hv_Length2 * hv_Sin));
            hv_Row3 = 1.0 * (hv_Row - ((hv_Length1 * hv_Sin) - (hv_Length2 * hv_Cos)));

            //矩形第四个端点坐标
            hv_Col4 = 1.0 * ((hv_Coloumn - (hv_Length1 * hv_Cos)) + (hv_Length2 * hv_Sin));
            hv_Row4 = 1.0 * (hv_Row - (((-hv_Length1) * hv_Sin) - (hv_Length2 * hv_Cos)));

            //仿射矩形方向所直线的边做基准线
            hv_StdLineRow1 = hv_Row2.Clone();
            hv_StdLineCol1 = hv_Col2.Clone();
            hv_StdLineRow2 = hv_Row3.Clone();
            hv_StdLineCol2 = hv_Col3.Clone();

            //仿射矩形Length2所在直线作为rake工具的ROI
            hv_ROILineRow1 = (hv_Row1 + hv_Row2) * 0.5;
            hv_ROILineCol1 = (hv_Col1 + hv_Col2) * 0.5;
            hv_ROILineRow2 = (hv_Row3 + hv_Row4) * 0.5;
            hv_ROILineCol2 = (hv_Col3 + hv_Col4) * 0.5;
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_Row, hv_Coloumn, hv_Phi,
                hv_Length1, hv_Length2);
            ho_Regions1.Dispose();
            rake(ho_Image, out ho_Regions1, hv_Length2, hv_Length1 * 2, hv_DetectWidth, hv_Sigma,
                hv_Threshold, hv_Transition, hv_Select, hv_ROILineRow1, hv_ROILineCol1, hv_ROILineRow2,
                hv_ROILineCol2, out hv_ResultRows, out hv_ResultColumns);

            //求所有边缘点到基准线的距离，保存最大距离及其对应的边缘点坐标，作为顶点
            hv_Max = 0;
            if ((int)(new HTuple((new HTuple(hv_ResultColumns.TupleLength())).TupleGreater(
                0))) != 0)
            {
                hv_EdgeRows = hv_ResultRows.Clone();
                hv_EdgeColumns = hv_ResultColumns.Clone();
                for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_ResultColumns.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                {
                    HOperatorSet.DistancePl(hv_ResultRows.TupleSelect(hv_i), hv_ResultColumns.TupleSelect(
                        hv_i), hv_StdLineRow1, hv_StdLineCol1, hv_StdLineRow2, hv_StdLineCol2,
                        out hv_Distance1);
                    if ((int)(new HTuple(hv_Max.TupleLess(hv_Distance1))) != 0)
                    {
                        hv_Max = hv_Distance1.Clone();
                        hv_ResultRow = hv_ResultRows.TupleSelect(hv_i);
                        hv_ResultColumn = hv_ResultColumns.TupleSelect(hv_i);
                    }
                }
                hv_ResultColumn = new HTuple();
                hv_ResultRow = new HTuple();
                for (int i = 0; i < hv_ResultColumns.Length; i++)
                {
                    HOperatorSet.DistancePl(hv_ResultRows.TupleSelect(i), hv_ResultColumns.TupleSelect(
                       i), hv_StdLineRow1, hv_StdLineCol1, hv_StdLineRow2, hv_StdLineCol2,
                       out hv_Distance1);
                    if (hv_Max.D <= (hv_Distance1.TupleAdd(this["顶点偏差"])))
                    {
                        hv_ResultColumn.Append(hv_ResultColumns.TupleSelect(i));
                        hv_ResultRow.Append(hv_ResultRows.TupleSelect(i));
                    }
                }
                if (hv_ResultRow.Length == 1)
                {
                    return;
                }
                Vision.Pts_to_best_line(out HObject holen, hv_ResultRow, hv_ResultColumn, hv_ResultColumn.Length, out HTuple hvrow1, out HTuple colu1, out HTuple row2, out HTuple colu2);
                HOperatorSet.LinePosition(hvrow1, colu1, row2, colu2,
                    out HTuple rowCenter, out HTuple colCenter, out HTuple leng, out HTuple phi);
                HOperatorSet.GenCrossContourXld(out HObject cross, rowCenter, colCenter, 20, 0);
                //this.KeyHObject["顶点线条"] = this.KeyHObject["顶点线条"].ConcatObj(cross);
                //this.KeyHObject["顶点线条"] = this.KeyHObject["顶点线条"].ConcatObj(holen);
                hv_ResultRow = rowCenter;
                hv_ResultColumn = colCenter;
            }

            ho_Rectangle.Dispose();
            ho_Regions1.Dispose();

            return;
        }
        public override RunProgram UpSatrt<T>(string PATH)
        {
            throw new NotImplementedException();
        }
    }
}