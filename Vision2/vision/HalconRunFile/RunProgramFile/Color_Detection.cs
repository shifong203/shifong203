using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.vision.HalconRunFile.Controls;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    /// <summary>
    ///
    /// </summary>
    public class Color_Detection : RunProgram 
    {
        public override string ShowHelpText()
        {
            return "2.6_颜色识别";
        }
        public string AxisXName { get; set; } = "";
        public double CMint { get; set; } = 10;

        public bool IsAxisName { get; set; }
        public HTuple Rows = new HTuple();
        public HTuple Cols = new HTuple();
        public HTuple MRows = new HTuple();
        public HTuple MCols = new HTuple();

        public override Control GetControl(IDrawHalcon halcon)
        {
            return new Color_DetectionUserControl(this);
        }

        public override RunProgram UpSatrt<T>(string path)
        {
            return base.ReadThis<Color_Detection>(path);
        }

        public Dictionary<string, Color_classify> keyColor { get; set; } = new Dictionary<string, Color_classify>();

        public class Color_classify :PCBFile.DataMaxMinDic
        {
            [Category("坐标位置"), DisplayName("搜索坐标Row"), Description("")]
            /// <summary>
            /// 搜索点位
            /// </summary>
            public double AoiRow { get; set; }

            [Category("坐标位置"), DisplayName("搜索坐标Col"), Description("")]
            /// <summary>
            /// 搜索点位
            /// </summary>
            public double AoiCol { get; set; }


            public Color_classify()
            {
                Threshold_H.Max = Threshold_S.Max = Threshold_V.Max = 255;
                this.Max_area.AddSelectType(Select_shape_Min_Max.Enum_Select_Type.ra, 10, 999999999);
                this.Max_area.AddSelectType(Select_shape_Min_Max.Enum_Select_Type.rb, 10, 999999999);
                SeleRoi = new HObject();
                DrawObj = new HObject();
                SeleRoi.GenEmptyObj();
                DrawObj.GenEmptyObj();
            }

            /// <summary>
            /// 使用
            /// </summary>
            public bool Enble
            {
                get; set;
            }
            /// <summary>
            /// 差阈检测
            /// </summary>
            public bool EnbleDifference { get; set; }
            /// <summary>
            /// 差阈角度仿射
            /// </summary>
            public bool EnbleDifferenceAngl { get; set; }
            public Select_shape_Min_Max Diffe_Max_area { get; set; } = new Select_shape_Min_Max();

            public HObject ModeRoing { get; set; }

            public double DifferenceInt { get; set; } = -3;


            [Category("搜索"), DisplayName("使用跟随"), Description("")]
            /// <summary>
            /// 使用跟随
            /// </summary>
            public bool IsHomMat { get; set; }



            [Category("搜索"), DisplayName("使用搜索"), Description("")]
            /// <summary>
            /// 使用搜索
            /// </summary>
            public bool EnbleSelect { get; set; }


            [Category("异常检测"), DisplayName("自动参数名"), Description("mask_size默认")]
            /// <summary>
            /// 自动查找
            /// </summary>
            public bool AotuThreshold { get; set; }
            [Category("异常检测"), DisplayName("自动参数名"), Description("mask_size默认结果,range灰度参考范围,scale,")]
            [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "mask_size", "range", "scale")]
            public string AotuThresholdPagr { get; set; } = "mask_size";
            [Category("异常检测"), DisplayName("自动参数值"), Description("")]
            public double AotuThresholdValue { get; set; } = 20;

            [Category("异常检测"), DisplayName("通道"), Description("")]
            public ImageTypeObj ImageTypeObj { get; set; }

            [Category("异常检测"), DisplayName("自动暗或亮"), Description("light亮区域，dark暗区域")]
            [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", "light", "dark")]
            public string LightD { get; set; } = "light";



            public string Name { get; set; } = "颜色";

            [Description(""), Category("图像通道"), DisplayName("通道"),]
            public ImageTypeObj ImageType { get; set; }

            /// <summary>
            /// 显示区域
            /// </summary>
            [Description(""), Category("显示"), DisplayName("显示区域"),]

            public bool IsObj { get; set; } = true;

            /// <summary>
            /// 显示中心
            /// </summary>
            [Description(""), Category("显示"), DisplayName("显示中心"),]
            
            public bool IsColt { get; set; } = true;


            [Description(""), Category("通道灰度"), DisplayName("最小灰度"),]
            public byte ThresSelectMin { get; set; } = 100;

            [Description(""), Category("通道灰度"), DisplayName("最大灰度"),]
            public byte ThresSelectMax { get; set; } = 255;

            [Description("负数开运算(缩小)，正数闭运算（(放大）"), Category("通道灰度"), DisplayName("区域开/闭运输"),]
            public double ClosingCir { get; set; } = 5;

            [Description(""), Category("筛选面积"), DisplayName("最小面积"),]
            public double SelectMin { get; set; } = 50;

            [Description(""), Category("筛选面积"), DisplayName("最大面积")]
            public double SelectMax { get; set; } = 99999999;

            public Color COlorES
            {
                get { return colorEs; }
                set { colorEs = value; color = "#" + value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2") + value.A.ToString("X2"); }
            }

            public HTuple color = "blue";
            private Color colorEs = Color.Lime;
            ///// <summary>
            ///// 模板区域
            ///// </summary>
            public HObject OutRiong;
            /// <summary>
            /// 检测区域
            /// </summary>
            public HObject DrawObj { get; set; }
            /// <summary>
            /// 搜索区域
            /// </summary>
            public HObject SeleRoi;

            public HTuple OBJRow = new HTuple();

            public HTuple OBJCol = new HTuple();

            public List<Threshold_Min_Max> threshold_Min_Maxes = new List<Threshold_Min_Max>();

            public Threshold_Min_Max Threshold_H { get; set; } = new Threshold_Min_Max();
            public Threshold_Min_Max Threshold_S { get; set; } = new Threshold_Min_Max();
            public Threshold_Min_Max Threshold_V { get; set; } = new Threshold_Min_Max();

            public Select_shape_Min_Max Max_area { get; set; } = new Select_shape_Min_Max();
            public bool H_enabled { get; set; }
            public bool S_enabled { get; set; }
            public bool V_enabled { get; set; }
            public byte Color_ID { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public int ColorNumber { get; set; } = 1;

            /// <summary>
            /// 闭运算大小
            /// </summary>
            public double ClosingCircleValue { get; set; } = 10;

            /// <summary>
            /// 填充
            /// </summary>
            public bool ISFillUp { get; set; }

            /// <summary>
            /// 填充
            /// </summary>
            public bool ISSelecRoiFillUP { get; set; }

            ///// <summary>
            ///// 定位
            ///// </summary>
            //public bool ModeHom { get; set; }

            /// <summary>
            /// 设置当前为模板位置
            /// </summary>
            public bool ISModeC { get; set; } 

            public void RunSeleRoi(HObject image, int id, out HObject iamget)
            {
                iamget = new HObject();
                try
                {
                    if (SeleRoi.CountObj() > 0)
                    {
                        HOperatorSet.ReduceDomain(image, SeleRoi, out image);
                    }
                    HOperatorSet.Threshold(image, out HObject hobj1, ThresSelectMin, ThresSelectMax);
                    HOperatorSet.Connection(hobj1, out hobj1);
                    HOperatorSet.SelectShape(hobj1, out iamget, "area", "and", SelectMin, SelectMax);

                    if (ISSelecRoiFillUP)
                    {
                        HOperatorSet.FillUp(iamget, out iamget);
                    }
                    HOperatorSet.Union1(iamget, out iamget);
                    if (ClosingCir > 0)
                    {
                        HOperatorSet.DilationCircle(iamget, out iamget, ClosingCir);
                    }
                    else if (ClosingCir != 0)
                    {
                        HOperatorSet.ErosionCircle(iamget, out iamget, Math.Abs(ClosingCir));
                    }
                }
                catch (Exception)
                {
                }
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="oneResultOBj"></param>
            /// <param name="drawObj"></param>
            /// <param name="RunPa"></param>
            /// <param name="Color_region"></param>
            /// <param name="Listobjs"></param>
            /// <returns></returns>
            public bool Classify(OneResultOBj oneResultOBj, AoiObj drawObj, RunProgram RunPa, out HObject Color_region,
                 List<HObject> Listobjs = null)
            {
                int NGNumber = 0;
      
                HObject iamge = oneResultOBj.Image;
                Color_region = null;
                HOperatorSet.GetImageSize(iamge, out HTuple width, out HTuple height);

                HObject V = new HObject();
                HObject hObject = new HObject();
                ///检测区
                HObject hObjectROI = oneResultOBj.GetHalcon().GetModelHaoMatRegion(RunPa.HomName, drawObj.SelseAoi);
                //HObject hObject = oneResultOBj.GetHalcon().GetModelHaoMatRegion(RunPa.HomName, drawObj.SelseAoi);
                if (EnbleSelect)
                {
                    this.RunSeleRoi(oneResultOBj.GetHalcon().GetImageOBJ(ImageType), 0, out hObject);
                    hObjectROI = hObject;
                    HOperatorSet.AreaCenter(hObjectROI, out HTuple area, out HTuple row, out HTuple column);
                    oneResultOBj.AddObj(hObjectROI, ColorResult.blue);
                }
                else
                {
                    if (hObjectROI != null && hObjectROI.IsInitialized()) { }
                    else
                    {
                        HOperatorSet.GenRectangle1(out hObject, 0, 0, oneResultOBj.GetHalcon().Height, oneResultOBj.GetHalcon().Width);
                        hObjectROI = hObject;
                    }
                }
                if (drawObj.DebugID!=0)
                {
                    oneResultOBj.AddObj(hObjectROI, ColorResult.coral);
                }
                hObject = hObjectROI;
                //HOperatorSet.SmallestRectangle1(hObjectROI, out row1, out col1, out height, out width);
                //HObject hObjectH = new HObject();
                //HObject hObjectS = new HObject();
                //HObject hObjectV = new HObject();
                //if (H_enabled) hObjectH = Threshold_H.Threshold(oneResultOBj.GetHalcon().GetImageOBJ(ImageTypeObj.H));
                //if (S_enabled) hObjectS = Threshold_S.Threshold(oneResultOBj.GetHalcon().GetImageOBJ(ImageTypeObj.S));
                //if (V_enabled) hObjectV = Threshold_V.Threshold(oneResultOBj.GetHalcon().GetImageOBJ(ImageTypeObj.V));
                //if (H_enabled && S_enabled)
                //{
                //    HOperatorSet.Intersection(hObject, hObjectH, out hObjectH);
                //    HOperatorSet.Intersection(hObjectH, hObjectS, out hObject);
                //}
                //if (H_enabled && S_enabled && V_enabled)
                //{
                //    HOperatorSet.Intersection(hObject, hObjectV, out hObject);
                //}
                //else if (H_enabled && V_enabled)
                //{
                //    HOperatorSet.Intersection(hObjectH, hObjectV, out hObject);
                //}
                //else if (S_enabled && V_enabled)
                //{
                //    HOperatorSet.Intersection(hObjectS, hObjectV, out hObject);
                //}
                //else if (H_enabled)
                //{
                //    hObject = hObjectH;
                //}
                //else if (S_enabled)
                //{
                //    hObject = hObjectS;
                //}
                //else if (V_enabled)
                //{
                //    hObject = hObjectV;
                //}
                if (Listobjs != null)
                {
                    Listobjs.Clear();
                }
                for (int i = 0; i < threshold_Min_Maxes.Count; i++)
                {
                    HOperatorSet.ReduceDomain(oneResultOBj.GetHalcon().GetImageOBJ(threshold_Min_Maxes[i].ImageTypeObj), hObjectROI, out V);
                    if (threshold_Min_Maxes[i].Enabled)
                    {
                        HObject hObject1 = threshold_Min_Maxes[i].Threshold(V);
                        V.Dispose();
                        if (Listobjs != null)
                        {
                            Listobjs.Add(hObject1);
                        }
                        HOperatorSet.AreaCenter(hObject1, out HTuple area, out HTuple rwo, out HTuple column);
                        HOperatorSet.Intersection(hObject1, hObject, out hObject);
                    }
                    else
                    {
                        if (Listobjs != null)
                        {
                            Listobjs.Add(new HObject());
                        }
                    }
                }
                if (ClosingCircleValue > 0)
                {
                    HOperatorSet.ClosingCircle(hObject, out hObject, ClosingCircleValue);
                }
                if (ClosingCircleValue < 0)
                {
                    HOperatorSet.OpeningCircle(hObject, out hObject, Math.Abs(ClosingCircleValue));
                }
                HOperatorSet.Connection(hObject, out hObject);
                if (ISFillUp)
                {
                    HOperatorSet.FillUp(hObject, out hObject);
                }
                Color_region = Max_area.select_shape(hObject);
                if (Listobjs != null)
                {
                    Listobjs.Insert(0, Color_region);
                }
                if (Color_region.CountObj() != ColorNumber)
                {
                    if (Color_region.CountObj() != 0)
                    {
                        oneResultOBj.AddObj(Color_region);
                        HObject hObject1 = Color_region;
                        HObject hObject2 = Color_region;
                        if (ColorNumber != 0)
                        {
                            HOperatorSet.Union1(hObject1, out hObject1);
                            HOperatorSet.DilationCircle(Color_region, out hObject1, 50);
                            HOperatorSet.Difference(hObjectROI, hObject1, out hObject2);
                        }
                        HOperatorSet.DilationRectangle1(Color_region, out hObject1, Vision.Instance.DilationRectangle1, Vision.Instance.DilationRectangle1);
                        HOperatorSet.Union1(hObject1, out hObject1);
                        HOperatorSet.SmallestRectangle1(hObject1, out HTuple  row1, out HTuple col1, out HTuple row2, out HTuple column2);
                        //HOperatorSet.AreaCenter(hObjectROI, out HTuple area, out HTuple row, out HTuple column);
                        //HOperatorSet.GenRectangle1(out hObject1, row1, col1, row2, column2);
                    }
                    oneResultOBj.AddMeassge(drawObj.CiName + "." + this.Name + ":" + Color_ID + Name + "数量" + Color_region.CountObj());
                    NGNumber++;
                }
                else if (IsColt)
                {
                    HOperatorSet.Connection(hObjectROI, out hObjectROI);
                    HOperatorSet.AreaCenter(Color_region, out HTuple area, out HTuple row, out HTuple column);
                    HOperatorSet.GenCrossContourXld(out HObject hObject1, row, column, 50, 0);
                    oneResultOBj.AddObj(hObject1);
                }
                if (IsObj)
                {
                    oneResultOBj.AddObj(Color_region);
                }
                HOperatorSet.AreaCenter(Color_region, out HTuple areaTd, out HTuple rowTd, out HTuple coluTd);
                //RunPa.nGRoi = RunPa.nGRoi.ConcatObj(Color_region);
           
                drawObj.SelseAoi = hObjectROI.Clone();
                if (rowTd.Length!=0)
                {
                    OBJRow = rowTd;
                    OBJCol = coluTd;
                    OutRiong = Color_region;
                }
          
                if (OutRiong != null&& OutRiong.CountObj()==1)
                {
                    if (EnbleDifference)
                    {
                        HOperatorSet.Union1(OutRiong, out OutRiong);
                        HOperatorSet.AreaCenter(OutRiong, out HTuple area, out HTuple row, out HTuple colu);
                        HOperatorSet.AreaCenter(ModeRoing, out HTuple Modearea, out HTuple Moderow, out HTuple Modecolu);
                        HOperatorSet.HomMat2dIdentity(out HTuple homMat2d);
                        //HOperatorSet.HomMat2dTranslate(homMat2d, row, colu, out homMat2d);
                        if (EnbleDifferenceAngl)
                        {
                            HOperatorSet.SmallestRectangle2(OutRiong, out HTuple rows, out HTuple colums, out HTuple phi, out HTuple length1, out HTuple length2);
                            HOperatorSet.SmallestRectangle2(ModeRoing, out  Moderow, out  Modecolu, out HTuple phi2, out HTuple length12, out HTuple length22);
                            HOperatorSet.VectorAngleToRigid(Moderow, Modecolu, phi2, row, colu, phi, out homMat2d);
                        }
                        else
                        {
                            HOperatorSet.VectorAngleToRigid(Moderow, Modecolu, 0, row, colu, 0, out homMat2d);
                        }
                        HOperatorSet.AffineTransRegion(ModeRoing, out HObject modeHomMatRoing, homMat2d, "nearest_neighbor");
                        HOperatorSet.Difference(modeHomMatRoing, OutRiong, out HObject hObject1);
                        HOperatorSet.Difference(OutRiong, modeHomMatRoing, out HObject hObject2);
                        hObject2 = hObject2.ConcatObj(hObject1);
                        HOperatorSet.Union1(hObject2, out hObject2);
                        hObject2 = RunProgram.OpneOrCosingCircle(hObject2, DifferenceInt);
                        HOperatorSet.Connection(hObject2, out hObject2);
                        hObject2= Diffe_Max_area.select_shape(hObject2);
                        oneResultOBj.AddObj(hObject2, ColorResult.red);
                    }
                }
                OutRow = rowTd;
                OutCol = coluTd;
                if (!this.CompareData(oneResultOBj, out drawObj.NGErr))
                {
                    NGNumber++;
                }
                  if (AotuThreshold)
                    {
                      HOperatorSet.ReduceDomain(oneResultOBj.GetHalcon().GetImageOBJ(ImageTypeObj), hObjectROI, out HObject imaget );
                      HOperatorSet.LocalThreshold(imaget, out hObject, "adapted_std_deviation", LightD, AotuThresholdPagr, AotuThresholdValue);
                        HOperatorSet.FillUp(hObject, out hObject);
                        oneResultOBj.AddObj(hObject,ColorResult.red);
                         NGNumber++;
                        drawObj.NGErr = drawObj.NGErr.ConcatObj(hObject);
                    }
                if (NGNumber != 0)
                {
                    HOperatorSet.Union1(Color_region, out Color_region);
                    HOperatorSet.DilationRectangle1(Color_region, out HObject hObject1, Vision.Instance.DilationRectangle1, Vision.Instance.DilationRectangle1);
                    HOperatorSet.Connection(hObject1, out hObject1);
                    HOperatorSet.FillUp(hObject1, out hObject1);
                    drawObj.NGErr = drawObj.NGErr.ConcatObj(Color_region);
                    if (hObject1.CountObj()==0)
                    {
                        hObject1 = drawObj.SelseAoi;
                    }
                    if (RunPa.ISShowText)
                    {
                        HOperatorSet.AreaCenter(hObjectROI, out HTuple area, out HTuple row, out HTuple column);
                        HTuple Colorid = HTuple.TupleGenConst(area.Length, Color_ID);
                        oneResultOBj.AddImageMassage(row, column, drawObj.CiName + "." + Colorid + Name, ColorResult.yellow);
                    }
             
                    Color_region = drawObj.NGErr;
                    if (ColorNumber==-1)
                    {
                        return false;
                    }
                    //HOperatorSet.Connection(drawObj.SelseAoi, out drawObj.SelseAoi);
                    //int ined = drawObj.SelseAoi.CountObj();
                    oneResultOBj.AddNGOBJ(drawObj.CiName, RunPa.Name + "." + Name, hObject1, drawObj.NGErr, RunPa.GetBackNames());
                    return false;
                    //oneResultOBj.AddNGOBJ(drawObj.CiName, RunPa.Name + "." + Name, hObject1.Clone(), Color_region.Clone(), RunPa.GetBackNames());
                }
                return true;
            }
        }

       

        public override bool RunHProgram(OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj)
        {
            oneRObjs = new List<OneRObj>();

            this.AOIObj.GenEmptyObj();
            HTuple area;
            Rows = new HTuple();
            Cols = new HTuple();
            HTuple row;
            //HOperatorSet.AreaCenter(aoiObj.Aoi, out HTuple area, out HTuple row, out HTuple col);
            foreach (var item in keyColor)
            {
                try
                {
                    if (!item.Value.Enble)
                    {
                        continue;
                    }
                    if (item.Key==AxisXName)
                    {
                        continue;
                    }
                    item.Value.Name = item.Key;
                    aoiObj.SelseAoi = item.Value.DrawObj.Clone();
                    if (aoiObj.IsLibrary)
                    {
                        HOperatorSet.AreaCenter(item.Value.DrawObj, out area, out row, out HTuple col);
                        HOperatorSet.VectorAngleToRigid(row, col, 0, aoiObj.AoiRow, aoiObj.AoiCol, new HTuple(aoiObj.Angle).TupleRad(), out HTuple hom2dt);
                        HOperatorSet.AffineTransRegion(item.Value.DrawObj, out aoiObj.SelseAoi, hom2dt, "nearest_neighbor");
                    }
                    else
                    {
                        if (CRDName == "")
                        {
                            aoiObj.CiName = Name;
                        }
                        else
                        {
                            aoiObj.CiName = CRDName;
                        }
                    }
                    if (!item.Value.Classify(oneResultOBj, aoiObj, this, out HObject hObject))
                    {
                        ResltBool = false;
                    }
                    HOperatorSet.AreaCenter(hObject, out  area, out  row, out HTuple column);
                    if (item.Key!=this.AxisXName)
                    {
                        Rows.Append(row);
                        Cols.Append(column);
                    }
                    this.AOIObj = this.AOIObj.ConcatObj(aoiObj.SelseAoi);
                }
                catch (Exception ex)
                {
                }
            }
        
            if (IsAxisName)
            {
                HTuple row1p = new HTuple();
                HTuple row2p = new HTuple();
                HTuple row3p = new HTuple();
                HTuple Col1p = new HTuple();
                HTuple Col2p = new HTuple();
                HTuple Col3p = new HTuple();
                if (keyColor.ContainsKey(this.AxisXName))
                {

                    if (keyColor[this.AxisXName].Enble)
                    {
                        aoiObj.SelseAoi = keyColor[this.AxisXName].DrawObj;
                        keyColor[this.AxisXName].Classify(oneResultOBj, aoiObj, this, out HObject color);
                        HOperatorSet.Connection(color, out color);
                        HOperatorSet.AreaCenter(color, out area, out row, out HTuple column);
                        Vision.Pts_to_best_line(out HObject line, row, column, 2, out row1p, out Col1p, out row2p, out Col2p);
                        Vision.Gen_arrow_contour_xld(out line, row1p, Col1p, row2p, Col2p);
                        HOperatorSet.HomMat2dIdentity(out HTuple hTuple);
                        HOperatorSet.HomMat2dRotate(hTuple, new HTuple(-90).TupleRad(), row1p, Col1p, out hTuple);
                        //HOperatorSet.HomMat2dTranslateLocal(hTuple, heith, 0, out hTuple1);
                        HOperatorSet.AffineTransPoint2d(hTuple, row2p, Col2p, out row3p, out Col3p);
                        //HOperatorSet.DistancePp()
                        HOperatorSet.GenCrossContourXld(out HObject cor, row3p, Col3p, 150, 10);
                     
                        oneResultOBj.AddImageMassage(row3p, Col3p, "C");
                        oneResultOBj.AddImageMassage(row2p, Col2p, "R");
                        oneResultOBj.AddObj(cor);
                        oneResultOBj.AddObj(line);
                        Vision.Gen_arrow_contour_xld(out line, row1p, Col1p, row3p, Col3p);
                        oneResultOBj.AddObj(line);
                        HOperatorSet.DistancePl(Rows, Cols, row1p, Col1p, row2p, Col2p, out HTuple dist);
                        dist = oneResultOBj.GetCaliConstMM(dist);
                        HOperatorSet.ProjectionPl(Rows, Cols, HTuple.TupleGenConst(Rows.Length, row1p)
                        , HTuple.TupleGenConst(Rows.Length, Col1p), HTuple.TupleGenConst(Rows.Length, row2p),
                        HTuple.TupleGenConst(Rows.Length, Col2p), out HTuple outRows, out HTuple outOutCol);
                        HOperatorSet.DistancePl(Rows, Cols, row1p, Col1p, row3p, Col3p, out HTuple dist2);
                        dist2 = oneResultOBj.GetCaliConstMM(dist2);
                        if (this.ISShowText)
                        {
                            oneResultOBj.AddImageMassage(Rows - (100), Cols, "R" + dist);
                            oneResultOBj.AddImageMassage(Rows, Cols, "C" + dist2);
                        }
                        HOperatorSet.GenCrossContourXld(out cor, outRows, outOutCol, 100, 0);
                        OutRow = dist;
                        OutCol = dist2;
                        if (aoiObj.DebugID==1)
                        {
                            HOperatorSet.GenCrossContourXld(out HObject hObjecte, MRows, MCols, 100, 10);
                            oneResultOBj.AddObj(hObjecte,ColorResult.blue);
                        }
                        if (ModeRow.Length == 0)
                        {
                            ModeRow = OutRow;
                        }
                        if (ModeCol.Length == 0)
                        {
                            ModeCol = OutCol;
                        }
                        if (ModeRow.Length != OutRow.Length)
                        {
                            NGNumber++;
                        }
                        else
                        {
                            HTuple minR = ModeRow.TupleSub(OutRow);
                            HTuple det = minR.TupleAbs().TupleGreaterEqualElem(CMint);
                            det = det.TupleFind(1);
                            if (det >= 0)
                            {
                                if (det.Length != 0)
                                {
                                    NGNumber++;
                                    HOperatorSet.GenRectangle2(out HObject err, MRows.TupleSelect(det),
                                        MCols.TupleSelect(det), HTuple.TupleGenConst(det.Length, 0),
                                        HTuple.TupleGenConst(det.Length, 20), HTuple.TupleGenConst(det.Length, 20));
                                    aoiObj.NGErr = aoiObj.NGErr.ConcatObj(err);
                                    oneResultOBj.AddImageMassage(MRows.TupleSelect(det) + (10), MCols.TupleSelect(det), "c" + minR.TupleSelect(det), ColorResult.red);
                                }
                            }
                        }
                        if (ModeCol.Length != OutCol.Length)
                        {
                            NGNumber++;
                        }
                        else
                        {
                            HTuple minC = ModeCol.TupleSub(OutCol);
                            //minC[12] = 10;
                            HTuple det = minC.TupleAbs().TupleGreaterEqualElem(CMint);
                            det = det.TupleFind(1);
                            if (det >= 0)
                            {
                                if (det.Length != 0)
                                {
                                    NGNumber++;
                                    HOperatorSet.GenRectangle2(out HObject err, MRows.TupleSelect(det),
                                     MCols.TupleSelect(det), HTuple.TupleGenConst(det.Length, 0),
                                     HTuple.TupleGenConst(det.Length, oneResultOBj.GetCaliConstPx( CMint)),
                                     HTuple.TupleGenConst(det.Length, oneResultOBj.GetCaliConstPx(CMint)));
                                    aoiObj.NGErr = aoiObj.NGErr.ConcatObj(err);
                                    oneResultOBj.AddImageMassage(MRows.TupleSelect(det) + (60), MCols.TupleSelect(det), "r" + minC.TupleSelect(det), ColorResult.red);
                                }
                            }
                        }
                        oneResultOBj.AddObj(aoiObj.NGErr,ColorResult.red);
                    }
                }
            }
            if (NGNumber!=0)
            {
                ResltBool = false;
            }
            HOperatorSet.Union1(AOIObj, out AOIObj);
            HOperatorSet.AreaCenter(AOIObj, out HTuple area2, out HTuple rows, out HTuple colu);

            return ResltBool;
        }
    }
}