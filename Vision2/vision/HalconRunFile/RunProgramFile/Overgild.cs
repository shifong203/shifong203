using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    /// <summary>
    /// 连接器
    /// </summary>
    public class Overgild : RunProgram
    {
        public Overgild()
        {
            Select_Shape_Min_Outobj.AddSelectType(Select_shape_Min_Max.Enum_Select_Type.ra, 20, 99999);
        }

        public override Control GetControl(IDrawHalcon halcon)
        {
            return new Controls.OvergildControl1(this);
        }

        public override RunProgram UpSatrt<T>(string Path)
        {
            return base.ReadThis<Overgild>(Path);
        }

        /// <summary>
        /// 检测区域灰度
        /// </summary>
        public Threshold_Min_Max threshold_Min_Max = new Threshold_Min_Max();

        /// <summary>
        /// 检测区域筛选
        /// </summary>

        public Select_shape_Min_Max Select_Shape_Min_Max = new Select_shape_Min_Max();

        public Select_shape_Min_Max Select_Shape_Min_Outobj = new Select_shape_Min_Max();

        [Description(""), Category("局部分割"), DisplayName("均值高度"),]
        public double MeanHeith { get; set; } = 7;

        [Description(""), Category("局部分割"), DisplayName("均值宽度"),]
        public double MeanWidth { get; set; } = 7;

        [Description("1.0, 3.0, 5.0, 7.0, 10.0, 20.0, 30.0，-255.0 ≤ Offset ≤ 255.0 (lin) "), Category("局部分割"), DisplayName("分割阈值"),]
        public double DnyValue { get; set; } = 5;

        [Description("暗=dark、相等=equal、亮=light、不相等=not_equal"), Category("局部分割"), DisplayName("分割区域"),]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", true, "dark", "equal", "light", "not_equal")]
        public string DnyTypeValue { get; set; } = "not_equal";

        [Description("区域膨胀或缩小，正值放大，负值缩小"), Category("搜索区域"), DisplayName("区域圆滑处理"),]
        public double DilationCircle { get; set; } = 10;

        [Description("区域膨胀或缩小，正值放大，负值缩小"), Category("搜索区域"), DisplayName("区域膨胀或缩小"),]
        public double ErosinCircle { get; set; } = -5;

        [Description(""), Category("划痕处理"), DisplayName("划痕第一次赛选最小长度"),]
        public double SocekSeleMin1 { get; set; } = 5;

        [Description(""), Category("划痕处理"), DisplayName("划痕第二次赛选最小长度"),]
        public double SocekSeleMin2 { get; set; } = 20;

        [Description(""), Category("划痕处理"), DisplayName("划痕赛选最大宽度"),]
        public double SocekSeleMax { get; set; } = 20;

        [Description(""), Category("划痕处理"), DisplayName("划痕第一次赛选后圆闭运算大小"),]
        public double ColsingSocek { get; set; } = 10;
        /// <summary>
        /// 轮廓检测
        /// </summary>
        [Description(""), Category("轮廓检测"), DisplayName("轮廓"),]
        public bool isRoiComparison { get; set; }

        [Description(""), Category("轮廓检测"), DisplayName("最小内圆半径"),]
        public double ComparisonSelecMinR { get; set; } = 2;

        [Description(""), Category("均值幅度"), DisplayName("均值幅度极限"),]
        public double Dicet { get; set; } = 5;

        [Description(""), Category("搜索区域"), DisplayName("倒角尺寸"),]
        public double ChamferPhi { get; set; } = 5;

        [Description(""), Category("结果输出"), DisplayName("裂痕显示"),]
        public bool SockeObj { get; set; }

        [Description(""), Category("划痕处理"), DisplayName("使用划痕检测")]
        public bool EnableScratch { get; set; } = true;

        [Description(""), Category("对称性分析"), DisplayName("使用对称性分析")]
        public bool EnableSymmetry { get; set; }


        [Description(""), Category("对称性分析"), DisplayName("短轴对称")]
        public bool EnableRB { get; set; }


        /// <summary>
        /// 模板区域
        /// </summary>
        public HObject ModeOBj;

        /// <summary>
        /// 搜索区域
        /// </summary>
        public HObject SelecRoi;

        public List<OvergilEX> RunListOvergil { get; set; } = new List<OvergilEX>();

        /// <summary>
        /// 划痕检测
        /// </summary>
        /// <param name="hIamge">图像域</param>
        /// <param name="hObject2">划痕结果</param>
        public void Scratch(HObject hIamge,out HObject hObject2)
        {
            hObject2 = new HObject();
            hObject2.GenEmptyObj();
            try
            {
                HOperatorSet.MeanImage(hIamge, out HObject hObject1, MeanWidth, MeanHeith);
                HOperatorSet.DynThreshold(hIamge, hObject1, out hObject2, DnyValue, DnyTypeValue);
                HOperatorSet.Connection(hObject2, out hObject2);
                HOperatorSet.SelectShape(hObject2, out hObject2, "ra", "and", SocekSeleMin1, 9999999999);
                HOperatorSet.Union1(hObject2, out hObject2);
            }
            catch (Exception ex)
            {
            }
        }

        public void Symmetry(OneResultOBj oneResult , int debugID,out HObject hObject2)
        {
            hObject2 = new HObject();
            hObject2.GenEmptyObj();
            HOperatorSet.HomMat2dIdentity(out HTuple hommat2didentity);
            try
            {
                HOperatorSet.SmallestRectangle2(SelecRoi, out HTuple row, out HTuple column, out HTuple phi, out HTuple length1, out HTuple length2);
                HOperatorSet.GenRectangle2(out HObject hObject, 0, length1, 0, length1, length2);
                HOperatorSet.HomMat2dTranslate(hommat2didentity, row, column, out HTuple  hommat2didentity2);
                HOperatorSet.HomMat2dRotate(hommat2didentity2, phi, row, column, out hommat2didentity2);
                HOperatorSet.AffineTransRegion(hObject, out HObject hObject1, hommat2didentity2, "nearest_neighbor");
                HOperatorSet.Difference(SelecRoi, hObject1, out HObject hObject3);
                HOperatorSet.Intersection(hObject1, SelecRoi, out HObject hObject4);
                HOperatorSet.HomMat2dSlant(hommat2didentity, new HTuple(180).TupleRad(), "y", row, column, out hommat2didentity2);
                HOperatorSet.HomMat2dRotate(hommat2didentity2, phi * 2, row, column, out hommat2didentity2);
                HOperatorSet.AffineTransRegion(hObject4, out HObject hObject5, hommat2didentity2, "nearest_neighbor");
                HOperatorSet.Difference(hObject5, hObject3, out HObject hObject6);
                if (debugID == 3)
                {
                    oneResult.AddObj(hObject1, ColorResult.firebrick);
                    oneResult.AddObj(hObject3, ColorResult.red);
                    oneResult.AddObj(hObject5, ColorResult.orange);
                    //oneResult.AddObj(hObject6, ColorResult.blue);
                    return;
                }
                HOperatorSet.Difference(hObject3, hObject5, out HObject hObject7);
                HOperatorSet.Union2(hObject7, hObject6, out hObject6);
                hObject2 = OpneOrCosingCircle(hObject6, -2);
                if (debugID == 4)
                {
                    oneResult.AddObj(hObject2, ColorResult.firebrick);
                    HOperatorSet.AreaCenter(hObject2, out HTuple area, out HTuple rows, out HTuple colus);
                    oneResult.AddImageMassage(rows+20, colus,"缺陷面积"+ area);
                }
                //HOperatorSet.Connection(hObject6, out hObject2);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 查找区域并进行均值划痕检测
        /// </summary>
        /// <param name="halcon"></param>
        /// <param name="id"></param>
        /// <param name="errRoi"></param>
        public void RunSeleRoi(OneResultOBj halcon, int id, out HObject errRoi)
        {
            errRoi = new HObject();
            errRoi.GenEmptyObj();
            HObject hobj1 = new HObject();
            hobj1.GenEmptyObj();
            HObject hImage = new HObject();
            hImage.GenEmptyObj();
            try
            {
                if (ISAoiMode)
                {
                    hImage = halcon.GetHalcon().GetImageOBJ(ImageTypeOb);
                }
                else
                {
                    hobj1 = AOIObj;
                    HOperatorSet.ReduceDomain(GetEmset(halcon.GetHalcon().GetImageOBJ(ImageTypeOb)), hobj1, out hImage);
                }

                if (id != 0)
                {
                    halcon.AddObj(hobj1);
                }
                hobj1 = threshold_Min_Max.Threshold(hImage);
                //HOperatorSet.Threshold(, out hobj1, ThresSelectMin, ThresSelectMax);
                HOperatorSet.Connection(hobj1, out hobj1);
                hobj1 = Select_Shape_Min_Max.select_shape(hobj1);
                //ImageHdt(halcon.Image);

                if (id == 1)
                {
                    halcon.AddObj(hobj1);
                }
                //HOperatorSet.FillUp(hObject, out hObject);

                hobj1 = DilationOrErosingCircle(hobj1, DilationCircle);
                if (id == 2)
                {
                    halcon.AddObj(hobj1);
                }
                HObject hObject = DilationOrErosingCircle(hobj1, ErosinCircle);
                HOperatorSet.Union1(hObject, out HObject rectangle3);
                if (IsDisObj)
                {
                    halcon.AddObj(rectangle3, ColorResult.yellow);
                }
                if (id == 2)
                {
                    HOperatorSet.AreaCenter(rectangle3, out HTuple areat, out HTuple row1, out HTuple column1);
                    
                    halcon.AddImageMassage(row1+80,column1,"检测区域面积"+areat);
                }
                HOperatorSet.ReduceDomain(halcon.GetHalcon().GetImageOBJ(ImageTypeOb), rectangle3, out hImage);

                if (id >= 2)
                {
                    halcon.AddObj(rectangle3, ColorResult.yellow);
                }
                if (false)
                {
                    HOperatorSet.AreaCenter(ModeOBj, out HTuple areat, out HTuple rowt, out HTuple columnt);
                    HOperatorSet.SmallestRectangle2(ModeOBj, out HTuple rowtd, out HTuple columntd, out HTuple phi, out HTuple leng1, out HTuple leng2);
                    HOperatorSet.AreaCenter(rectangle3, out HTuple areas, out HTuple rowdt, out HTuple columnttd);
                    HOperatorSet.SmallestRectangle2(rectangle3, out HTuple rowtdd, out HTuple columntdd, out HTuple phdi, out HTuple lengd1, out HTuple lengd2);
                    HOperatorSet.VectorAngleToRigid(rowtd, columntd, phi, rowtdd,
                        columntdd, phdi, out HTuple hTuple);
                    HOperatorSet.AffineTransRegion(ModeOBj, out HObject modeObjT, hTuple, "nearest_neighbor");
                    HOperatorSet.Difference(modeObjT, rectangle3, out HObject ertd);
                    if (id >= 3)
                    {
                        halcon.AddObj(ertd, ColorResult.red);
                    }
                    HOperatorSet.OpeningCircle(ertd, out ertd, 5);
                }

                //if (IsDisObj)
                //{
                //    halcon.AddObj(modeObjT, ColorResult.blue);
                //}

                HObject errt = new HObject();
                errt.GenEmptyObj();
                SelecRoi = hObject;
                HObject hObject2 = new HObject();
                hObject2.GenEmptyObj();
                HObject hObject1 = new HObject();

                if (EnableScratch)
                {
                    Scratch(hImage, out hObject2);
                    HOperatorSet.ClosingCircle(hObject2, out hObject2, ColsingSocek);
                }
                if (EnableSymmetry)
                {
                    Symmetry(halcon, id,out hObject2);
                }
                HOperatorSet.Connection(hObject2, out hObject2);
                hObject2 = Select_Shape_Min_Outobj.select_shape(hObject2);
                HOperatorSet.Union1(hObject2, out hObject2);

                HOperatorSet.DilationCircle(hObject2, out errt, 3.5);
                if (errt.CountObj() != 0)
                {
                    halcon.AddObj(errt, ColorResult.blue);
                }
                if (SockeObj)
                {
                    HOperatorSet.Skeleton(errt, out errt);
                }

                if (errt.CountObj() != 0)
                {
                    errRoi = errt;
                }
                return;
                //HOperatorSet.DilationCircle(hObject2, out errRoi, 20);

                //HOperatorSet.Intersection(SelecRoi, errRoi, out errRoi);

                HOperatorSet.ReduceDomain(hImage, errRoi, out hImage);
                HOperatorSet.Connection(errRoi, out errRoi);
                HOperatorSet.Intensity(errRoi, hObject1, out HTuple mean, out HTuple deviation);
                HObject hObject4 = new HObject();
                hObject4.GenEmptyObj();
                HObject hObject7 = new HObject();
                hObject7.GenEmptyObj();
                for (int i = 0; i < mean.Length; i++)
                {
                    HOperatorSet.SelectObj(errRoi, out HObject hObject5, i + 1);
                    HOperatorSet.ReduceDomain(hImage, hObject5, out HObject hObject6);
                    HOperatorSet.AutoThreshold(hObject6, out HObject hObject3, 1);
                    HOperatorSet.AreaCenter(hObject3, out HTuple area2, out HTuple row2, out HTuple column2);
                    HOperatorSet.Connection(hObject3, out hObject3);

                    if (area2.Length != 0)
                    {
                        HOperatorSet.Intensity(hObject5, hImage, out HTuple mean2, out deviation);
                        //halcon.AddObj(hObject3, ColorResult.yellow);
                        if (deviation.TupleMax() > Dicet)
                        {
                            HOperatorSet.SelectShape(hObject3, out hObject3, "area", "and", 10, area2.TupleMax() - 1);
                            HOperatorSet.Union1(hObject3, out hObject3);
                            if (hObject3.CountObj() != 0)
                            {
                                HOperatorSet.ClosingCircle(hObject3, out hObject3, 2);
                                //HOperatorSet.Intensity(hObject3, hImage, out mean, out deviation);
                                //halcon.AddObj(hObject3, ColorResult.blue);
                                hObject7 = hObject7.ConcatObj(hObject5);
                                hObject4 = hObject4.ConcatObj(hObject3);
                            }
                        }
                    }
                }

                HOperatorSet.DilationCircle(hObject4.ConcatObj(errt), out hObject4, 1);
                errRoi = hObject4;

                HOperatorSet.Intensity(hObject7, hImage, out mean, out deviation);
                //halcon.AddObj(hObject3,ColorResult.red);
                HOperatorSet.AreaCenter(hObject7, out HTuple area, out HTuple row, out HTuple column);
                if (id != 0)
                {
                    halcon.AddImageMassage(row, column, mean + ":" + deviation);
                }
                //halcon.AddObj(hObject);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oneResultOBj"></param>
        /// <param name="id"></param>
        public void RunDebug(OneResultOBj oneResultOBj, int id)
        {
            try
            {
                RunSeleRoi(oneResultOBj, id, out HObject errRoi);
                if (errRoi.CountObj() != 0)
                {
                    oneResultOBj.AddObj(errRoi, ColorResult.red);
                }
                RunListOvergil[id].RunPa(oneResultOBj, this, out HObject hObject1);
                if (hObject1.CountObj() != 0)
                {
                    oneResultOBj.AddNGOBJ(this.Name, RunListOvergil[id].ErrText, hObject1, hObject1, this.GetBackNames());
                }
            }
            catch (Exception ex)
            {
            }
        }

        public override bool RunHProgram(OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj)
        {
            oneRObjs = new List<OneRObj>();
            try
            {
                HObject errRoi = new HObject();
                errRoi.GenEmptyObj();

                RunSeleRoi(oneResultOBj, aoiObj.DebugID, out errRoi);

                //halcon.AddObj(SelecRoi);
                if (errRoi.CountObj() != 0)
                {
                    oneResultOBj.AddNGOBJ(aoiObj.CiName, Defect_Type, aoiObj.SelseAoi, errRoi, this.GetBackNames());
                    this.NGNumber++;
                    //oneResultOBj.AddObj(errRoi, ColorResult.red);
                }
                if (isRoiComparison)
                {
                    HOperatorSet.SmallestRectangle2(SelecRoi, out HTuple row, out HTuple col, out HTuple phi, out HTuple length1, out HTuple length2);
                    HOperatorSet.HomMat2dIdentity(out HTuple home2d);
                    HOperatorSet.SmallestRectangle2(ModeOBj, out HTuple row2, out HTuple col2, out HTuple phi2, out HTuple length12, out HTuple length22);
                    HOperatorSet.VectorAngleToRigid(row2, col2, phi2, row, col, phi, out home2d);
                    HOperatorSet.AffineTransRegion(ModeOBj, out HObject hObject, home2d, "nearest_neighbor");
                    //if (id != 0)
                    //{
                    oneResultOBj.AddObj(hObject, ColorResult.gold);
                    //}
                    HOperatorSet.Difference(SelecRoi, hObject, out hObject);
                    oneResultOBj.AddObj(hObject, ColorResult.black);
                    HOperatorSet.Connection(hObject, out hObject);
                    HOperatorSet.SelectShape(hObject, out hObject, "inner_radius", "and", ComparisonSelecMinR, 99999999);

                    HOperatorSet.DilationCircle(hObject, out hObject, 50);
                    oneResultOBj.AddObj(hObject, ColorResult.firebrick);
                    //if (id!=0)
                    //{
                    //    halcon.AddObj(ModeOBj, ColorResult.orange);
                    //}
                }
                for (int i = 0; i < RunListOvergil.Count; i++)
                {
                    RunListOvergil[i].RunPa(oneResultOBj, this, out HObject hObject1);

                    if (hObject1.CountObj() != 0)
                    {
                        HOperatorSet.DilationCircle(hObject1, out HObject hObject, 50);
                        //halcon.AddObj(hObject, ColorResult.firebrick);
                        this.NGNumber++;
                        oneResultOBj.AddNGOBJ(aoiObj.CiName, RunListOvergil[i].ErrText, hObject.Clone(), hObject1, this.GetBackNames());
                    }
                }
                if (this.NGNumber == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogErr(ex);
            }
            return false;
        }

        public class OvergilEX
        {
            [Description(""), Category("结果处理"), DisplayName("缺陷名称"),]
            public string ErrText { get; set; } = "镀层缺陷";

            [Description(""), Category("图像通道"), DisplayName("通道"),]
            public ImageTypeObj ImageType { get; set; }

            [Description(""), Category("通道灰度"), DisplayName("最小灰度"),]
            public byte ThresSelectMin { get; set; } = 100;

            [Description(""), Category("通道灰度"), DisplayName("最大灰度"),]
            public byte ThresSelectMax { get; set; } = 255;

            [Description("负数开运算(缩小)，正数闭运算（(放大）"), Category("通道灰度"), DisplayName("区域开/闭运输"),]
            public double ClosingCir { get; set; } = 5;

            [Description("均值查比例"), Category("通道灰度"), DisplayName("均值查比例"),]
            public double DevSecl { get; set; } = 2;

            [Description(""), Category("筛选面积"), DisplayName("最小面积"),]
            public double SelectMin { get; set; } = 50;

            [Description(""), Category("筛选面积"), DisplayName("最大面积")]
            public double SelectMax { get; set; } = 99999999;

            /// <summary>
            /// 均值处理
            /// </summary>
            /// <param name="halcon"></param>
            /// <param name="overgild"></param>
            /// <param name="errObj"></param>
            public void RunPa(OneResultOBj halcon, Overgild overgild, out HObject errObj)
            {
                errObj = new HObject();
                //halcon.AddObj(overgild.SelecRoi);
                HOperatorSet.ReduceDomain(halcon.GetHalcon().GetImageOBJ(ImageType), overgild.SelecRoi, out HObject Himage);
                HOperatorSet.Intensity(overgild.SelecRoi, halcon.GetHalcon().GetImageOBJ(ImageType), out HTuple mean, out HTuple deviation);
                HOperatorSet.AreaCenter(overgild.SelecRoi, out HTuple area, out HTuple row, out HTuple column);

                //halcon.AddMessageIamge(row, column, mean + ":" + deviation);

                HOperatorSet.Threshold(Himage, out HObject hobj1, 0, mean - deviation * DevSecl);
                HOperatorSet.Connection(hobj1, out hobj1);

                if (ClosingCir > 0)
                {
                    HOperatorSet.ClosingCircle(hobj1, out hobj1, ClosingCir);
                }
                else
                {
                    HOperatorSet.OpeningCircle(hobj1, out hobj1, Math.Abs(ClosingCir));
                }
                HOperatorSet.Connection(hobj1, out hobj1);
                HOperatorSet.SelectShape(hobj1, out HObject hObject, "area", "and", SelectMin, SelectMax);
                HOperatorSet.Union1(hObject, out errObj);
                //Himage= overgild.GetEmset(Himage);
                //halcon.Image(Himage);
                HOperatorSet.Intensity(errObj, halcon.GetHalcon().GetImageOBJ(ImageType), out mean, out deviation);
                HOperatorSet.AreaCenter(errObj, out area, out row, out column);
                //HOperatorSet.AutoThreshold(Himage, out  errObj, 0);
                //halcon.AddObj(errObj, ColorResult.blue);
                if (mean.Length != 0)
                {
                    //halcon.AddObj(errObj, ColorResult.blue);
                    //halcon.AddMessageIamge(row, column, mean + ":" + deviation);
                }
            }
        }
    }
}