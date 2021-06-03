using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.PCBFile
{

    public class ICPint : BPCBoJB, InterfacePCBA
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

            public int ErrNumber = 0;
            public bool PintRun(HObject iamge,HTuple homMat2D,HalconRun halcon ,ICPint iCPint, OneResultOBj oneResultOBj, out HObject ErrDobj,out HObject ROI,int debug=0)
            {
                ErrDobj = new HObject();
                ROI = new HObject();
                ROI.GenEmptyObj();
                ErrDobj.GenEmptyObj();
                 ErrNumber = 0;
                if (!Enbler)
                {
                    return true;
                }
                try
                {
                    HOperatorSet.AffineTransRegion(ICOBJ1, out HObject hObject1, homMat2D, "nearest_neighbor");
                    HOperatorSet.AffineTransRegion(PintZi, out HObject hObject12, homMat2D, "nearest_neighbor");
                    ROI = ROI.ConcatObj(hObject12);
                    ROI = ROI.ConcatObj(hObject1);
                    HOperatorSet.ReduceDomain(iamge, hObject1, out HObject hObject2);
                    HOperatorSet.SmallestRectangle2(hObject12, out HTuple rwo, out HTuple col, out HTuple phi, out HTuple leng1, out HTuple leng2);        
                    HOperatorSet.ReduceDomain(iamge, hObject12, out HObject hObject23);
                    //halcon.AddOBJ(hObject12, RunProgram.ColorResult.blue);
                    //halcon.AddOBJ(hObject1, RunProgram.ColorResult.green);
                    HOperatorSet.Threshold(hObject23, out hObject12, iCPint.HanPMin, iCPint.HanPMax);
                    HOperatorSet.Connection(hObject12, out hObject12);
                    if (debug != 0)
                    {
                        halcon.AddOBJ(hObject12, ColorResult.blue);
                    }
                        HOperatorSet.SelectShape(hObject12, out hObject12, "area", "and", iCPint.HPAreaMin, 99999999);
                    if (hObject12.CountObj() > 0)
                    {
                        halcon.AddOBJ(hObject12, ColorResult.red);
                        ErrNumber++;
                    }
                    HOperatorSet.Threshold(hObject2, out hObject1, iCPint.PintMin, iCPint.PintMax);
                    HOperatorSet.OpeningCircle(hObject1, out hObject1, 3.5);
                    HOperatorSet.Connection(hObject1, out hObject1);
                    if (debug != 0)
                    {
                        halcon.AddMessage("角度" + phi.TupleDeg());
                        halcon.AddOBJ(hObject1, ColorResult.red);
                        halcon.AddOBJ(hObject12, ColorResult.red);
                    }
                    HOperatorSet.SelectShape(hObject1, out HObject hObject3, "area", "and", iCPint.PintAreaMin, 99999999);
                    HOperatorSet.AreaCenter(hObject3, out HTuple area, out HTuple row3, out HTuple column3);
                    HOperatorSet.GenCrossContourXld(out HObject cross, row3, column3, 10, 0);
                    halcon.AddOBJ(cross);
                    HOperatorSet.Union1(hObject3, out hObject3);
                    HOperatorSet.GenRectangle2(out HObject hObject4, iCPint. Row1, iCPint.Col1, phi , 1, 40);
                    HOperatorSet.Closing(hObject3, hObject4, out hObject3);
                    HOperatorSet.Connection(hObject3, out hObject3);
                    //HTuple  hTuple2 = phi.TupleDeg().TupleInt()% 90;
                    if (phi.TupleDeg() + 5 >90) HOperatorSet.SortRegion(hObject3, out hObject3, "first_point", "true", "row");
                    else HOperatorSet.SortRegion(hObject3, out hObject3, "first_point", "true", "column");
                    HOperatorSet.AreaCenter(hObject3, out area, out row3, out column3);
                    HOperatorSet.SmallestRectangle2(hObject3, out HTuple row4, out HTuple column4, out HTuple phi4, out HTuple length11, out HTuple length22);
                    HOperatorSet.GenCrossContourXld(out cross, row3, column3, 10, 0);
                    halcon.AddOBJ(cross);
                    HTuple DistanceS = new HTuple();
                    HTuple Indetx = new HTuple();

                    if (true)
                    {
                        length11 = halcon.GetCaliConstMM(length11);
                        length22 = halcon.GetCaliConstMM(length22);
                    }
                    if (row4.Length > 1)
                    {
                        for (int i2 = 0; i2 < row4.Length; i2++)
                        {
                            Indetx.Append(i2+1);
                            bool ErrBool = false;
                            if (i2 < row4.Length - 1)
                            {
                                HOperatorSet.DistancePp(row4[i2], column4[i2], row4[i2 + 1], column4[i2 + 1], out HTuple distance);
                                if (true)
                                {
                                    distance = halcon.GetCaliConstMM(distance);
                                }
                                DistanceS.Append(distance);
                                if (PintInterval.SetValeu(distance)!=0)
                                {
                                    HOperatorSet.GenRegionLine(out HObject hObject5, row4[i2], column4[i2], row4[i2 + 1], column4[i2 + 1]);
                                    oneResultOBj.AddNGOBJ(iCPint.Name, "针脚间距", hObject1, hObject5);
                                    ErrDobj = ErrDobj.ConcatObj(hObject5);
                                    ErrNumber++;
                                }
                            }
                            if (PintLength.SetValeu(length11[i2])!=0)
                            {
                                oneResultOBj.AddNGOBJ(iCPint.Name, "长度", hObject1, hObject3.SelectObj(i2 + 1));
                                ErrBool = true;
                                ErrDobj = ErrDobj.ConcatObj(hObject3.SelectObj(i2 + 1));
                            }
                            if (PintWatih.SetValeu(length22[i2]) != 0)
                            {
                                oneResultOBj.AddNGOBJ(iCPint.Name, "宽度", hObject1, hObject3.SelectObj(i2 + 1));
                                ErrBool = true;
                                if (!ErrBool)
                                {
                                    ErrDobj = ErrDobj.ConcatObj(hObject3.SelectObj(i2 + 1));
                                }
                            }
                        }
                        if (Number!= row4.Length)
                        {
                            oneResultOBj.AddNGOBJ(iCPint.Name, "引脚数量", hObject1, hObject3);
                            ErrDobj = ErrDobj.ConcatObj(hObject3);
                            ErrNumber++;
                        }
                        if (debug != 0)
                        {
                            halcon.AddOBJ(hObject3, ColorResult.yellow);
                            halcon.AddMessage("数量"+ length11.Length+ "长度" + length11.TupleMean() + "宽度" + length22.TupleMean() + "距离" + DistanceS.TupleMean());
                            DistanceS.Append(0);
                            halcon.AddMessageIamge(row4, column4, Indetx+ "长度" + length11 + "宽度" + length22 + "距离" + DistanceS);
                        }
                    }
                    if (ErrNumber==0)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                { }
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
            public int SetValeu(double value )
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
        public Control GetControl(HalconRun run)
        {
            return new ICPintControl(this, run);
        }
        public override BPCBoJB UpSatrt<T>(string path)
        {
            BPCBoJB bPCBoJB = base.UpSatrt<ICPint>(path);
            if (bPCBoJB == null)
            {
                bPCBoJB = this;
            }
            return bPCBoJB;
        }
        public void SaveThis(string path)
        {
            HalconRun.ClassToJsonSavePath(this, path);
        }
        public Threshold_Min_Max Threshold_Min_Max = new Threshold_Min_Max();

        public Select_shape_Min_Max select_Shape_Min_ = new Select_shape_Min_Max();

        public Threshold_Min_Max PintThreshold_Min_Max = new Threshold_Min_Max();

        public ImageTypeObj ImageTypeObj { get; set; }

        [Category(""), DisplayName("高度"), Description("")]
        public double Heiath{ get; set;}= 100;
        [Category(""), DisplayName("宽度"), Description("")]
        public double Watih { get; set; } = 100;
        [Category("检测区域"), DisplayName("芯片开运算"), Description("")]
        public double OpeningClosin { get; set; } = 10;


        [Category("焊脚检测区域"), DisplayName("焊脚检测长度"), Description("")]
        public double PintLengthD { get; set; } = 100;

        [Category("焊脚检测区域"), DisplayName("焊脚灰度Min"), Description("")]
        public byte PintMin { get; set; } = 105;
        [Category("焊脚检测区域"), DisplayName("焊脚灰度Max"), Description("")]
        public byte PintMax { get; set; } = 255;
        [Category("焊脚检测区域"), DisplayName("焊脚检测宽度"), Description("")]
        public double PintDPT { get; set; } = 30;
        [Category("焊脚检测区域"), DisplayName("Z字焊脚"), Description("")]
        public bool IsZpint { get; set; }

  
        [Category("焊脚检测区域"), DisplayName("针脚面积min"), Description("")]
        public double PintAreaMin { get; set; } = 400;



        [Category("焊盘检测区域"), DisplayName("焊盘灰度Min"), Description("")]
        public byte HanPMin { get; set; } = 105;
        [Category("焊盘检测区域"), DisplayName("焊盘面积Min"), Description("")]
        public double HPAreaMin { get; set; } = 800;

        [Category("焊盘检测区域"), DisplayName("焊盘灰度Max"), Description("")]
        public byte HanPMax { get; set; } = 255;
        [Category("焊盘检测区域"), DisplayName("焊盘检测宽度"), Description("")]
        public double PintDT { get; set; } = 80;

        [Category("焊盘检测区域"), DisplayName("焊盘检测长度"), Description("")]
        public double HPLengthD { get; set; } = 100;

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
        public override bool Run(HalconRun halcon, RunProgram run, OneResultOBj oneResultOBj, out HObject ErrRoi)
        {
            HObject ErrDobj = new HObject();
            ErrDobj.GenEmptyObj();
            Threshold_Min_Max.ImageTypeObj = ImageTypeObj;
            ErrNumber = 0;
            RestBool = true;
            ErrRoi = new HObject();
            ErrRoi.GenEmptyObj();
            Rows = new List<HTuple>();
            Cols = new List<HTuple>();
            Phis = new List<HTuple>();
            Lengt1S = new List<HTuple>();
            Lengt2S = new List<HTuple>();
            HOperatorSet.ReduceDomain(halcon.GetImageOBJ(Threshold_Min_Max.ImageTypeObj), TestingRoi, out HObject V);
            HObject  hObject= Threshold_Min_Max.Threshold(V);
            HOperatorSet.OpeningCircle(hObject, out hObject, OpeningClosin);
            HOperatorSet.Connection(hObject, out hObject);
            hObject= select_Shape_Min_.select_shape(hObject);
            HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple rowm, out HTuple colukn);
            HOperatorSet.SelectShape(hObject, out hObject, "area", "and", area.TupleMax() - 1, area.TupleMax() + 1);
            HOperatorSet.SmallestRectangle2(hObject, out  Row1, out  Col1, out  ahi1, out HTuple length1,out  HTuple length2);
            HOperatorSet.FillUp(hObject, out hObject);
            if (isROI)
            {
                halcon.AddOBJ(hObject);
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
                HOperatorSet.SmallestRectangle2(ziPoints[i].ICOBJ1, out HTuple row, out HTuple col, out HTuple phi, out HTuple leng1, out HTuple leng2);
                HOperatorSet.GenRectangle2(out HObject hObject12, row, col, phi , PintLengthD, PintDPT);
                ziPoints[i].ICOBJ1 = hObject12;
                HOperatorSet.SmallestRectangle2(ziPoints[i].PintZi, out  row, out  col, out  phi, out  leng1, out  leng2);
                HOperatorSet.GenRectangle2(out  hObject12, row, col, phi, HPLengthD, PintDT);
                ziPoints[i].PintZi = hObject12;

                if (!ziPoints[i].PintRun(V, homMat2D, halcon, this, oneResultOBj, out HObject errObj,out HObject roi))
                {
                  
                    //oneResultOBj.AddNGOBJ(new OneRObj() { NGText = (i + 1) + "针脚缺陷", ComponentID = this.Name, NGROI = errObj, ROI = roi });
                    //ErrDobj = ErrDobj.ConcatObj(errObj);
                    ErrNumber++;
                } 
            }

            if (ErrNumber!=0)
            {
                //halcon.AddOBJ(ErrDobj, RunProgram.ColorResult.red);
                RestBool = false;
            }

            return RestBool;
        }

        public void DebugP(HalconRun halcon, RunProgram run,out HObject hObjectErr)
        {
            Threshold_Min_Max.ImageTypeObj = ImageTypeObj;
            hObjectErr = new HObject();
            hObjectErr.GenEmptyObj();
            HOperatorSet.ReduceDomain(halcon.GetImageOBJ(Threshold_Min_Max.ImageTypeObj), TestingRoi, out HObject V);
            HObject hObject = Threshold_Min_Max.Threshold(V);
            HOperatorSet.OpeningCircle(hObject, out hObject, OpeningClosin);
            HOperatorSet.Connection(hObject, out hObject);
            hObject = select_Shape_Min_.select_shape(hObject);
            HOperatorSet.AreaCenter(hObject, out HTuple areat, out HTuple rowm, out HTuple colukn);
            HOperatorSet.SelectShape(hObject, out hObject, "area", "and", areat.TupleMax() - 1, areat.TupleMax() + 1);
            halcon.AddOBJ(hObject);

            HOperatorSet.SmallestRectangle2(hObject, out HTuple row, out HTuple column, out HTuple phi, out HTuple length1, out HTuple length2);
            if (row.Length!=1)
            {
                MessageBox.Show("未找到芯片");
                return;
            }
            HOperatorSet.VectorAngleToRigid(0, 0, 0, row, column, phi, out HTuple homMat2D);
            Row = row;
            Col = column;
            ahi = phi;
            HPLengthD = length1-10;
            PintLengthD = length1-20;
            HOperatorSet.AffineTransPixel(homMat2D, new HTuple(0, (double)-length2, 0, (double)length2),
                new HTuple((double)length1, 0, -(double)length1, 0), out HTuple rowTrans, out HTuple colTrans);
            HOperatorSet.AffineTransPixel(homMat2D, new HTuple(0,(double) -length2-40, 0, (double)length2 + 40),
             new HTuple((double)length1 + 40, 0, (double)-length1 - 40, 0), out HTuple rowTrans2, out HTuple colTrans2);
            DistanceS1 = new List<HTuple>();
            //ziPoints.Clear();
            for (int i = 0; i < rowTrans.Length; i++)
            {
                DistanceS1.Add(new HTuple());
                HOperatorSet.GenRectangle2(out HObject hObject1, rowTrans[i], colTrans[i], phi + new HTuple(90).TupleRad() * (i + 1), PintLengthD, PintDT);
                HOperatorSet.GenRectangle2(out HObject hObject12, rowTrans2[i], colTrans2[i], phi + new HTuple(90).TupleRad() * (i + 1), HPLengthD, PintDPT);
                if (ziPoints.Count<i)
                {
                    ziPoints.Add(new ZiPoint());
                }

                ziPoints[i].ICOBJ1 = hObject1;
                ziPoints[i].PintZi = hObject12;
                HOperatorSet.ReduceDomain(V, hObject1, out HObject hObject2);
                HOperatorSet.ReduceDomain(V, hObject12, out HObject hObject23);
                halcon.AddOBJ(hObject12, ColorResult.blue);
                halcon.AddOBJ(hObject1, ColorResult.blue);
                HOperatorSet.Threshold(hObject23, out hObject12, HanPMin, HanPMax);
                HOperatorSet.Connection(hObject12, out hObject12);
                HOperatorSet.SelectShape(hObject12, out hObject12, "area", "and", 200, 999999);

                halcon.AddOBJ(hObject12, ColorResult.red);
                HOperatorSet.Threshold(hObject2, out hObject1, PintMin, PintMax);
                HOperatorSet.Connection(hObject1, out hObject1);
                HOperatorSet.OpeningCircle(hObject1, out hObject1, 3.5);
                halcon.AddOBJ(hObject1, ColorResult.red);
                HOperatorSet.SelectShape(hObject1, out HObject hObject3, "area", "and", 450, 999999);
                HOperatorSet.AreaCenter(hObject3, out HTuple area, out HTuple row3, out HTuple column3);
                HOperatorSet.GenCrossContourXld(out HObject cross, row3, column3, 10, 0);
                halcon.AddOBJ(cross);
                HOperatorSet.Union1(hObject3, out hObject3);
                HOperatorSet.GenRectangle2(out HObject hObject4, row, column, phi + new HTuple(90).TupleRad() * (i + 1), 1, 40);
                HOperatorSet.Closing(hObject3, hObject4, out hObject3);
                HOperatorSet.Connection(hObject3, out hObject3);
                if (i % 2 <= 0) HOperatorSet.SortRegion(hObject3, out hObject3, "first_point", "true", "column");
                else HOperatorSet.SortRegion(hObject3, out hObject3, "first_point", "true", "row");
                HOperatorSet.AreaCenter(hObject3, out area, out row3, out column3);
                HOperatorSet.SmallestRectangle2(hObject3, out HTuple row4, out HTuple column4, out HTuple phi4, out HTuple length11, out HTuple length22);
                halcon.AddOBJ(hObject3, ColorResult.yellow);
                Rows.Add(row4);
                Cols.Add(column4);
                Phis.Add(phi4);
                Lengt1S.Add(length11);
                Lengt2S.Add(length22);
                HOperatorSet.GenCrossContourXld(out cross, row3, column3, 10, 0);
                halcon.AddOBJ(cross);
                if (row4.Length>3)
                {
                    for (int i2 = 0; i2 < row4.Length - 1; i2++)
                    {
                        HOperatorSet.DistancePp(row4[i], column4[i], row4[i + 1], column4[i + 1], out HTuple distance);
                        DistanceS1[i].Append(distance);
                    }
                }
             
            }

        }
    }
}
