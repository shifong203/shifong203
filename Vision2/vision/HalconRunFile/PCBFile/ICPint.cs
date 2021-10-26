using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    public class ICPint : RunProgram, InterfacePCBA
    {
        /// <summary>
        ///  IC芯片
        /// </summary>
        public class ZiPoint
        {
            public ZiPoint()
            {
                ICOBJ1.GenEmptyObj();
                PintZi.GenEmptyObj();
            }

            /// <summary>
            /// 针脚
            /// </summary>
            public HObject ICOBJ1 = new HObject();

            /// <summary>
            /// 焊盘
            /// </summary>
            public HObject PintZi = new HObject();

            public bool Enbler { get; set; } = true;

            [Category("检测项"), DisplayName("针脚数量"), Description("")]
            public int Number { get; set; } = 10;

            [Category("检测项"), DisplayName("针脚长度"), Description("")]
            [Editor(typeof(ValueMaxMinContrl.Editor), typeof(UITypeEditor))]
            /// <summary>
            /// 针脚长度
            /// </summary>
            public ValueMaxMin PintLength { get; set; } = new ValueMaxMin();

            [Category("检测项"), DisplayName("针脚宽度"), Description("")]
            [Editor(typeof(ValueMaxMinContrl.Editor), typeof(UITypeEditor))]
            /// <summary>
            /// 针脚宽度
            /// </summary>
            public ValueMaxMin PintWatih { get; set; } = new ValueMaxMin();

            [Category("检测项"), DisplayName("针脚间距"), Description("")]
            [Editor(typeof(ValueMaxMinContrl.Editor), typeof(UITypeEditor))]
            /// <summary>
            /// 针脚间距
            /// </summary>
            public ValueMaxMin PintInterval { get; set; } = new ValueMaxMin();

            [Category("焊脚异物"), DisplayName("异物灰度Min"), Description("")]
            public byte Pint_Foreign_MatterMin { get; set; } = 60;

            [Category("焊脚异物"), DisplayName("异物灰度Max"), Description("")]
            public byte Pint_Foreign_MatterMax { get; set; } = 255;

            [Category("焊脚异物"), DisplayName("异物面积"), Description("")]
            public int Pint_Foreign_Matter_Area { get; set; } = 100;

    

            //[Category("焊盘破损"), DisplayName("焊盘最小面积"), Description("")]
            //public int HP_Foreign_Matter_Area { get; set; } = 150;

            /// <summary>
            ///
            /// </summary>
            public int ErrNumber = 0;

            public bool PintRun(HObject iamge, HTuple homMat2D, ICPint iCPint, OneResultOBj oneResultOBj, out HObject ErrDobj, out HObject ROI, int debug = 0)
            {
                ErrDobj = new HObject();
                ROI = new HObject();
                ROI.GenEmptyObj();
                ErrDobj.GenEmptyObj();
                ErrNumber = 0;
                string NGText = "";
                HObject HJROI = new HObject();
                HJROI.GenEmptyObj();
                if (!Vision.IsObjectValided(ICOBJ1) || !Vision.IsObjectValided(PintZi))
                {
                    oneResultOBj.AddMeassge("为绘制区域");
                    return false;
                }
                if (!Enbler)
                {
                    return true;
                }
                try
                {
                    if (homMat2D == null)
                    {
                        HOperatorSet.HomMat2dIdentity(out homMat2D);
                    }
                    HOperatorSet.AffineTransRegion(ICOBJ1, out HObject HjAOI, homMat2D, "nearest_neighbor");
                    HOperatorSet.AffineTransRegion(PintZi, out HObject hObject1H, homMat2D, "nearest_neighbor");
                    ROI = ROI.ConcatObj(hObject1H);
                    ROI = ROI.ConcatObj(HjAOI);
                    HOperatorSet.Union1(ROI, out ROI);
                    HOperatorSet.ReduceDomain(iamge, HjAOI, out HObject hObject2);
                    HOperatorSet.SmallestRectangle2(hObject1H, out HTuple rwo, out HTuple col, out HTuple phi, out HTuple leng1, out HTuple leng2);
                    HOperatorSet.ReduceDomain(iamge, hObject1H, out HObject HJImage);
                    HOperatorSet.Threshold(HJImage, out HObject hObjectH2, iCPint.HanPMin, iCPint.HanPMax);
                    HOperatorSet.OpeningCircle(hObjectH2, out hObjectH2, 2);
                    HOperatorSet.Connection(hObjectH2, out HObject hObjectH1);
                    HOperatorSet.SelectShape(hObjectH1, out hObjectH2, "area", "and", iCPint.HPAreaMin,9999999);
                    if (hObjectH2.CountObj() > 0)
                    {
                        //NGText += "焊盘错误:";
                        NGText += "  Bonding pad error:";
                        oneResultOBj.AddObj(hObjectH2, ColorResult.red);
                        ErrNumber++;
                        if (debug == 3)
                        {
                            HOperatorSet.AreaCenter(hObjectH2, out HTuple aread, out HTuple row3d, out HTuple column3d);
                            oneResultOBj.AddImageMassage(row3d, column3d, aread);
                        }
                    }
                    HOperatorSet.Threshold(hObject2, out HObject hObject11, iCPint.PintMin, iCPint.PintMax);
                    HOperatorSet.OpeningCircle(hObject11, out hObject11, 2.5);
                    HOperatorSet.Connection(hObject11, out hObject11);
         
                    HOperatorSet.SelectShape(hObject11, out HObject hObject3, "area", "and", iCPint.PintAreaMin, iCPint.PintAreaMax);
                    HOperatorSet.AreaCenter(hObject3, out HTuple area, out HTuple row3, out HTuple column3);
                    HOperatorSet.GenCrossContourXld(out HObject cross, row3, column3, 10, 0);
                    HOperatorSet.Union1(hObject3, out hObject3);
                    HOperatorSet.GenRectangle2(out HObject hObject4, rwo, col, phi, 1, 40);
                    HOperatorSet.Closing(hObject3, hObject4, out hObject3);
                    HOperatorSet.Connection(hObject3, out hObject3);

                    if (phi.TupleDeg().TupleAbs() + 5 > 90) HOperatorSet.SortRegion(hObject3, out hObject3, "first_point", "true", "row");
                    else HOperatorSet.SortRegion(hObject3, out hObject3, "first_point", "true", "column");
                    HOperatorSet.AreaCenter(hObject3, out area, out row3, out column3);
                    HOperatorSet.SmallestRectangle2(hObject3, out HTuple row4, out HTuple column4, out HTuple phi4, out HTuple length11, out HTuple length22);
                    //No pin found 未找到针点
                    if (length11.Length == 0)
                    {
                        if (!NGText.Contains("No pin found"))
                        {
                            ErrNumber++;
                            NGText += "No pin found:";
                        }
                    }
                    else
                    {
                        HOperatorSet.GenRectangle2(out hObject4, rwo, col, phi, 1, 80);
                        HOperatorSet.Union2(hObjectH1, hObject3, out HObject hObject5);
                        HOperatorSet.Closing(hObject5, hObject4, out hObject4);
                        HOperatorSet.FillUp(hObject4, out hObject4);
                        HOperatorSet.Union1(hObject4, out hObject4);
                        if (debug != 0)
                        {
                            oneResultOBj.AddObj(hObject4, ColorResult.blue);
                        }
                            HOperatorSet.DilationCircle(hObject4, out hObject4, iCPint.PintDif);
                      

                        if (phi.TupleDeg().TupleAbs() + 5 > 90) HOperatorSet.SortRegion(hObject4, out hObject4, "first_point", "true", "row");
                        else HOperatorSet.SortRegion(hObject4, out hObject4, "first_point", "true", "column");

                        HOperatorSet.Connection(hObject4, out hObject4);
                        HOperatorSet.Intersection(hObject4, hObjectH1, out HObject hObject);

                        HOperatorSet.Intersection(hObject4, hObject1H, out HObject hObject6);
                   
                        HOperatorSet.AreaCenter(hObject, out HTuple areah1, out HTuple rowh1, out HTuple coluh1);

                        for (int i = 0; i < areah1.Length; i++)
                        {  //Leg broken 焊脚破损
                            if (areah1[i] < iCPint.HPAreaminT)
                            {
                                if (!NGText.Contains("Leg broken"))
                                {
                                    ErrNumber++;
                                    NGText += "Leg broken:";
                                }
                                ErrDobj = ErrDobj.ConcatObj(hObject6.SelectObj(i + 1));
                                oneResultOBj.AddObj(hObject6.SelectObj(i + 1), ColorResult.red);
                            }
                        }
                        if (debug == 1)
                        {
                            oneResultOBj.AddObj(ROI, ColorResult.blue);
                            oneResultOBj.AddObj(hObject, ColorResult.blue);
                            oneResultOBj.AddImageMassage(rowh1, coluh1, areah1);
                            oneResultOBj.AddObj(hObject4, ColorResult.yellow);
                        }
                        HOperatorSet.DilationCircle(hObject4, out HObject hObjectDil, 3);
                        HOperatorSet.ClosingRectangle1(ROI, out HJROI, 1000, 1000);
                        HOperatorSet.Difference(HJROI, hObject1H, out HJROI);
                        HOperatorSet.Intersection(HJROI, hObject4, out HObject hjtOBj);
                        HOperatorSet.ReduceDomain(iamge, HJROI, out HJImage);
                        HOperatorSet.Threshold(HJImage, out HObject hObjectErr, Pint_Foreign_MatterMin, Pint_Foreign_MatterMax);
                        HOperatorSet.Difference(hObjectErr, hObject4, out HObject err);
                        HOperatorSet.Connection(err, out err);
                        HOperatorSet.SelectShape(err, out err, "area", "and", iCPint. Pint_Skew_Area, 999999999);
                        if (debug != 0)
                        {
                            oneResultOBj.AddObj(hObjectErr, ColorResult.coral);
                            oneResultOBj.AddObj(err, ColorResult.red);
                            oneResultOBj.AddObj(hjtOBj, ColorResult.yellow);
                            HOperatorSet.AreaCenter(err, out HTuple areaErr, out HTuple rowErr, out HTuple columnErr);
                            oneResultOBj.AddImageMassage(rowErr, columnErr, areaErr);
                        }
                 
                        if (err.CountObj()!=0)
                        {
                            HOperatorSet.DilationCircle(err, out err, 3);
                            ErrDobj = ErrDobj.ConcatObj(err);
                            if (!NGText.Contains("Pin skew"))
                            {
                                ErrNumber++;
                                NGText += "Pin skew:";
                            }
                            oneResultOBj.AddObj(err, ColorResult.red);
                        }
                        HOperatorSet.Difference(HJROI, hObjectDil, out HObject hObject7);

                        //oneResultOBj.AddObj(hObject3, ColorResult.red);
                        HOperatorSet.ReduceDomain(iamge, hObject7, out HJImage);
                        HOperatorSet.Threshold(HJImage, out HObject hObject8, Pint_Foreign_MatterMin, Pint_Foreign_MatterMax);
                        HOperatorSet.Connection(hObject8, out hObject8);
                        HOperatorSet.SelectShape(hObject8, out hObject8, "area", "and", Pint_Foreign_Matter_Area, 999999999);
                        //Foreign body in welding foot 焊脚内异物
                        if (hObject8.CountObj() != 0)
                        {
                            HOperatorSet.DilationCircle(hObject8, out HObject errObj, 5);
                            oneResultOBj.AddObj(errObj, ColorResult.red);
                            ErrDobj = ErrDobj.ConcatObj(errObj);
                            if (!NGText.Contains("Foreign body in welding foot "))
                            {
                                ErrNumber++;
                                NGText += "Foreign body in welding foot :";
                            }
                        }
                    }
                    HOperatorSet.GenCrossContourXld(out cross, row3, column3, 10, 0);
                    oneResultOBj.AddObj(cross, ColorResult.green);
                    HTuple DistanceS = new HTuple();
                    HTuple Indetx = new HTuple();
                    length11 = oneResultOBj.GetCaliConstMM(length11);
                    length22 = oneResultOBj.GetCaliConstMM(length22);
                    if (row4.Length > 1)
                    {
                        for (int i2 = 0; i2 < row4.Length; i2++)
                        {
                            Indetx.Append(i2 + 1);
                            bool ErrBool = false;
                            if (i2 < row4.Length - 1)
                            {
                                HOperatorSet.DistancePp(row4[i2], column4[i2], row4[i2 + 1], column4[i2 + 1], out HTuple distance);
                                if (true)
                                {
                                    distance = oneResultOBj.GetCaliConstMM(distance);
                                }
                                DistanceS.Append(distance);
                                if (PintInterval.SetValeu(distance) != 0)
                                {
                                    ErrNumber++;
                                    HOperatorSet.GenRegionLine(out HObject hObject5, row4[i2], column4[i2], row4[i2 + 1], column4[i2 + 1]);
                                    //Stitch spacing针脚间距
                                    oneResultOBj.AddNGOBJ(iCPint.Name, "Stitch Spacing", HjAOI, hObject5, iCPint.GetBackNames());
                                    if (!NGText.Contains("Stitch Spacing"))
                                    {
                                        NGText += "Stitch Spacing:";
                                    }
                                    ErrDobj = ErrDobj.ConcatObj(hObject5);
                                }
                            }
                            //长度length
                            if (PintLength.SetValeu(length11[i2]) != 0)
                            {
                                ErrNumber++;
                                if (!NGText.Contains("Length"))
                                {
                                    NGText += "Length:";
                                }
                                oneResultOBj.AddNGOBJ(iCPint.Name, "Length", ROI, hObject3.SelectObj(i2 + 1), iCPint.GetBackNames());
                                ErrBool = true;
                                ErrDobj = ErrDobj.ConcatObj(hObject3.SelectObj(i2 + 1));
                            }
                            //width宽度
                            if (PintWatih.SetValeu(length22[i2]) != 0)
                            {
                                ErrNumber++;
                                if (!NGText.Contains("Width"))
                                {
                                    NGText += "Width:";
                                }
                                oneResultOBj.AddNGOBJ(iCPint.Name, "Width", ROI, hObject3.SelectObj(i2 + 1), iCPint.GetBackNames());
                                ErrBool = true;
                                if (!ErrBool)
                                {
                                    ErrDobj = ErrDobj.ConcatObj(hObject3.SelectObj(i2 + 1));
                                }
                            }
                        }
                        //Pin number引脚数量
                        if (Number != row4.Length)
                        {
                            if (!NGText.Contains("Pin number"))
                            {
                                NGText += "Pin number:";
                            }
                            oneResultOBj.AddNGOBJ(iCPint.Name, "Pin number", ROI, hObject3, iCPint.GetBackNames());
                            ErrDobj = ErrDobj.ConcatObj(hObject3);
                            ErrNumber++;
                        }
                        if (debug == 2)
                        {
                            oneResultOBj.AddObj(HJROI, ColorResult.blue);
                            oneResultOBj.AddObj(hObject3, ColorResult.yellow);
                            oneResultOBj.AddMeassge("数量" + length11.Length + "长度" + length11.TupleMean() + "宽度" + length22.TupleMean() + "距离" + DistanceS.TupleMean());
                            DistanceS.Append(0);
                            oneResultOBj.AddImageMassage(row4, column4, Indetx + "长度" + length11 + "宽度" + length22 + "距离" + DistanceS);
                        }
                        if (debug == 3)
                        {
                            oneResultOBj.AddObj(hObjectH2, ColorResult.blue);
                            oneResultOBj.AddObj(hObjectH1, ColorResult.yellow);
                        //    oneResultOBj.AddMeassge("数量" + length11.Length + "长度" + length11.TupleMean() + "宽度" + length22.TupleMean() + "距离" + DistanceS.TupleMean());
                        //    DistanceS.Append(0);
                        //    oneResultOBj.AddImageMassage(row4, column4, Indetx + "长度" + length11 + "宽度" + length22 + "距离" + DistanceS);
                        }
                    }
                    if (ErrNumber == 0)
                    {
                        return true;
                    }
                    else
                    {
                        HOperatorSet.AreaCenter(ROI, out area, out row3, out column3);
                        oneResultOBj.AddImageMassage(row3, column3, NGText, ColorResult.red);
                    }
                }
                catch (Exception ex)
                {
                    iCPint.LogErr(ex);
                }
                return false;
            }
        }

        public class ValueMaxMin
        {
            //[Editor(typeof(pgu))]
            [DisplayName("±值"), Description("")]
            public double PM { get; set; } = 10;

            //[Editor(typeof(NumericUpDown))]
            [DisplayName("标准值"), Description("")]
            public double Value { get; set; } = 10;

            [DisplayName("Min"), Description("")]
            public double Min { get; set; } = 10;

            [DisplayName("Max"), Description("")]
            public double Max { get; set; } = 100;

            [DisplayName("使用标准值"), Description("使用标准值=true,fales使用最大最小值")]
            public bool PMBool { get; set; } = true;

            [DisplayName("当前值"), Description("")]
            public double LValue { get; set; }

            /// <summary>
            /// 比较参数
            /// </summary>
            /// <param name="value">值</param>
            /// <returns>0=OK,1=标准值NG，2大于正负值NG</returns>
            public int SetValeu(double value)
            {
                LValue = value;
                if (PMBool)
                {
                    if (value > Value + PM)
                    {
                        return 4;
                    }
                    else if (value < Value - PM)
                    {
                        return 3;
                    }
                }
                else
                {
                    if (value > Max)
                    {
                        return 2;
                    }
                    else if (value < Min)
                    {
                        return 1;
                    }
                }
                return 0;
            }
        }

        public ICPint()
        {
            //OBJPint.GenEmptyObj();
            //OBJPintD.GenEmptyObj();
        }

        public override Control GetControl(IDrawHalcon run)
        {
            return new ICPintControl(this, run);
        }

        public override RunProgram UpSatrt<T>(string path)
        {
            RunProgram bPCBoJB = base.ReadThis<ICPint>(path);
            if (bPCBoJB == null)
            {
                bPCBoJB = this;
            }
            return bPCBoJB;
        }

        //public void SaveThis(string path)
        //{
        //    HalconRun.ClassToJsonSavePath(this, path);
        //}
        public Threshold_Min_Max Threshold_Min_Max = new Threshold_Min_Max();

        public Select_shape_Min_Max select_Shape_Min_ = new Select_shape_Min_Max();

        public Threshold_Min_Max PintThreshold_Min_Max = new Threshold_Min_Max();

        [Category("焊脚检测"), DisplayName("焊脚图像通道"), Description("")]
        /// <summary>
        /// 针脚
        /// </summary>
        public ImageTypeObj ImageTypeObj { get; set; }

        [Category("检测区域"), DisplayName("芯片开运算"), Description("")]
        public double OpeningClosin { get; set; } = 10;

        [Category("焊脚检测区域"), DisplayName("焊脚灰度Min"), Description("")]
        public byte PintMin { get; set; } = 105;

        [Category("焊脚检测区域"), DisplayName("焊脚灰度Max"), Description("")]
        public byte PintMax { get; set; } = 255;

        [Category("焊脚检测区域"), DisplayName("焊脚膨胀"), Description("")]
        public byte PintDif { get; set; } = 3;

        [Category("焊脚检测区域"), DisplayName("Z字焊脚"), Description("")]
        public bool IsZpint { get; set; }

        [Category("焊脚检测区域"), DisplayName("针脚面积min"), Description("")]
        public double PintAreaMin { get; set; } = 400;

        [Category("焊脚检测区域"), DisplayName("针脚面积max"), Description("")]
        public double PintAreaMax { get; set; } = 999999;

        [Category("焊盘检测区域"), DisplayName("焊盘灰度Min"), Description("")]
        public byte HanPMin { get; set; } = 105;

        [Category("焊盘检测区域"), DisplayName("焊盘最大面积"), Description("")]
        public double HPAreaMin { get; set; } = 800;

        [Category("焊盘检测区域"), DisplayName("焊盘最小面积"), Description("")]
        public double HPAreaminT { get; set; } = 300;

        [Category("焊盘检测区域"), DisplayName("焊盘灰度Max"), Description("")]
        public byte HanPMax { get; set; } = 255;
        [Category("焊脚偏移"), DisplayName("焊脚偏移面积"), Description("缺陷大于这个面积将会被标记")]
        public int Pint_Skew_Area { get; set; } = 50;

        public bool Homat2dT { get; set; }

        public bool isROI { get; set; }

        public double Phi { get; set; } = 100;

        public HTuple Row = new HTuple();
        public HTuple Col = new HTuple();
        public HTuple ahi = new HTuple();
        public HTuple Row1 = new HTuple();
        public HTuple Col1 = new HTuple();
        public HTuple ahi1 = new HTuple();

        public List<HTuple> DistanceS1 = new List<HTuple>();
        public List<HTuple> Rows = new List<HTuple>();
        public List<HTuple> Cols = new List<HTuple>();
        public List<HTuple> Lengt1S = new List<HTuple>();
        public List<HTuple> Lengt2S = new List<HTuple>();
        public List<HTuple> Phis = new List<HTuple>();
        public List<ZiPoint> ziPoints = new List<ZiPoint>();

        public HTuple homMat2D;

        public override bool RunHProgram(OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj)
        {
            oneRObjs = new List<OneRObj>();
            HObject ErrDobj = new HObject();
            ErrDobj.GenEmptyObj();
            Threshold_Min_Max.ImageTypeObj = ImageTypeObj;

            ResltBool = true;
            //ErrRoi = new HObject();
            //ErrRoi.GenEmptyObj();
            Rows = new List<HTuple>();
            Cols = new List<HTuple>();
            Phis = new List<HTuple>();
            Lengt1S = new List<HTuple>();
            Lengt2S = new List<HTuple>();
            HOperatorSet.ReduceDomain(oneResultOBj.GetHalcon().GetImageOBJ(Threshold_Min_Max.ImageTypeObj), AOIObj, out HObject V);
            HObject hObject = Threshold_Min_Max.Threshold(V);
            HOperatorSet.OpeningCircle(hObject, out hObject, OpeningClosin);
            HOperatorSet.Connection(hObject, out hObject);
            hObject = select_Shape_Min_.select_shape(hObject);
            HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple rowm, out HTuple colukn);
            HOperatorSet.SelectShape(hObject, out hObject, "area", "and", area.TupleMax() - 1, area.TupleMax() + 1);
            HOperatorSet.SmallestRectangle2(hObject, out Row1, out Col1, out ahi1, out HTuple length1, out HTuple length2);
            HOperatorSet.FillUp(hObject, out hObject);
            if (isROI)
            {
                oneResultOBj.AddObj(hObject);
            }
            if (Homat2dT)
            {
                HOperatorSet.VectorAngleToRigid(Row, Col, 0, Row1, Col1, 0, out homMat2D);
            }
            else
            {
                HOperatorSet.HomMat2dIdentity(out homMat2D);
            }
            DistanceS1 = new List<HTuple>();
            for (int i = 0; i < ziPoints.Count; i++)
            {
                if (!ziPoints[i].PintRun(V, homMat2D, this, oneResultOBj, out HObject errObj, out HObject roi))
                {
                    //oneResultOBj.AddObj(errObj);
                    oneResultOBj.AddNGOBJ(this.Name, (i + 1) + "Pin Err", roi, errObj, this.GetBackNames());
                    //ErrDobj = ErrDobj.ConcatObj(errObj);
                    NGNumber++;
                }
            }

            if (NGNumber != 0)
            {
                //halcon.AddObj(ErrDobj, RunProgram.ColorResult.red);
                ResltBool = false;
            }
            return ResltBool;
        }
    }
}