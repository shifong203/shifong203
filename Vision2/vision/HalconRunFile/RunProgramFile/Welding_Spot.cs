using HalconDotNet;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.Controls;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    /// <summary>
    /// 焊点检测
    /// </summary>
    public class Welding_Spot : RunProgram
    {
        public Welding_Spot()
        {
            //HObject.GenEmptyObj();
            NoNeedle_S_thr_min = 50;
            NoNeedle_S_thr_max = 255;
            NoNeedle_V_thr_min = 0;
            NoNeedle_V_thr_max = 50;
            NoNeedle_H_thr_min = 0;
            NoNeedle_H_thr_max = 40;
            NoNeedle_area_min = 400;
        }
        public override string ShowHelpText()
        {
            return "2.4_焊点识别";
        }
        public override Control GetControl()
        {
            return new Welding_Spot_Control1(this);
        }


        [Description("显示NG或详细信息"), Category("显示"), DisplayName("显示结果")]
        public bool IsScdet { get; set; }
        [Description("显示Row偏移位置"), Category("显示"), DisplayName("显示Row偏移")]
        public int DispRow { get; set; } = -600;
        [Description("显示Col偏移位置"), Category("显示"), DisplayName("显示Col偏移")]
        public int DispCow { get; set; } = +600;
        public byte R_threa_min { get; set; }
        public byte R_threa_max { get; set; } = 150;
        public bool H_enabled { get; set; } = true;
        public byte H_threa_min { get; set; }
        public byte H_threa_max { get; set; } = 150;
        /// <summary>
        /// 少焊点面积筛选
        /// </summary>
        public HTuple H_sele_area_min { get; set; } = new HTuple(10);
        /// <summary>
        /// 无针点面积筛选
        /// </summary>
        public HTuple H_sele_area_max { get; set; } = new HTuple(999999);
        public bool S_enabled { get; set; } = true;
        public byte S_threa_min { get; set; }
        public byte S_threa_max { get; set; } = 150;


        public bool V_enabled { get; set; } = true;
        public byte V_threa_min { get; set; }
        public byte V_threa_max { get; set; } = 150;


        public byte H_area_compute_thr_min { get; set; }
        public byte H_area_compute_thr_max { get; set; } = 100;

        public byte R_area_compute_thr_min { get; set; } = 10;
        public byte R_area_compute_thr_max { get; set; } = 100;

        public byte S_area_compute_thr_min { get; set; }
        public byte S_area_compute_thr_max { get; set; } = 100;

        public byte V_area_compute_thr_min { get; set; }
        public byte V_area_compute_thr_max { get; set; } = 100;
        public double Compute_sele_area_min { get; set; } = 100;
        public double Compute_sele_area_max { get; set; } = 999999;

        /// <summary>
        /// 少焊园筛选
        /// </summary>
        public double Welding_ra { get; set; } = 60;
        /// <summary>
        /// 溢焊面积
        /// </summary>
        public double Overflow_Welding_Area { get; set; } = 800;
        /// <summary>
        /// 无针检测
        /// </summary>
        public byte NoNeedle_V_thr_max { get; set; }
        public byte NoNeedle_V_thr_min { get; set; }
        public byte NoNeedle_S_thr_max { get; set; }
        public byte NoNeedle_S_thr_min { get; set; }
        public byte NoNeedle_H_thr_max { get; set; }
        public byte NoNeedle_H_thr_min { get; set; }
        public double NoNeedle_area_min { get; set; }

        public override bool RunHProgram(HalconRun halcon, OneResultOBj oneResultOBj, int id, string name = null)
        {

            List<bool> restle = new List<bool>();
            for (int i = 0; i < listWelding.Count; i++)
            {
                Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>();
                restle.Add(listWelding[i].Solder_joint_inspection(this, halcon, out bool isConglutnation, out bool islessDefend, out HTuple areas));

                string text = "";
                for (int i2 = 0; i2 < areas.Length; i2++)
                {
                    if (areas[i2].S.Length > 2)
                    {
                        text += areas[i2];

                    }
                }
                halcon.TrayRestData.ListVerData.Add(text);
                halcon.AddOBJ(halcon.GetModelHaoMatRegion(HomName, listWelding[i].HObject), ColorResult.green);
            }
            for (int i = 0; i < restle.Count; i++)
            {
                if (restle[i])
                {
                    halcon.AddNGMessage("焊点");
                    return false;
                }
            }
            return true;
        }
        HObject Image1 = new HObject();
        HObject Image2 = new HObject();
        HObject Image3 = new HObject();
        /// <summary>
        /// HSV通道转换
        /// </summary>
        /// <param name="hObject">图像</param>
        /// <param name="RGBf">RGB图像通道</param>
        /// <param name="hObjectt"></param>
        public void Welding(HObject hObject, char RGBf, out HObject hObjectt)
        {
            HOperatorSet.CountChannels(hObject, out HTuple htcon);
            hObjectt = new HObject();
            if (htcon != 3)
            {
                return;
            }
            HOperatorSet.Decompose3(hObject, out Image1, out Image2, out Image3);
            HOperatorSet.TransFromRgb(Image1, Image2, Image3, out HObject H, out HObject V, out HObject S, "hsv");
            switch (RGBf)
            {
                case 'H':
                    hObjectt = H;
                    break;
                case 'V':
                    hObjectt = V;

                    break;
                case 'S':
                    hObjectt = S;
                    break;
                default:
                    hObjectt = H;
                    break;
            }

            return;
        }

        public override RunProgram UpSatrt<T>(string Path)
        {
            return base.ReadThis<Welding_Spot>(Path);
        }
        //public HObject HObject { get; set; } = new HObject();
        public List<WeldingCC> listWelding { get; set; } = new List<WeldingCC>();
        public class WeldingCC
        {
            /// <summary>
            /// 绘制焊点的区域
            /// </summary>
            public HObject HObject { get; set; } = new HObject();
            /// <summary>
            /// 点集合
            /// </summary>
            public List<HObject> ListHObj { get; set; } = new List<HObject>();


            public bool IsEllIpse { get; set; } = false;

            public double CiericROut { get; set; } = 60;
            public double CiericRInt { get; set; } = 20;
            public double Phi { get; set; } = 0;
            public double Radius2 { get; set; } = 80;
            /// <summary>
            /// 
            /// </summary>
            public byte Number { get; set; } = 1;

            /// <summary>
            /// 检测焊点
            /// </summary>
            /// <param name="welding_Spo">焊点程序</param>
            /// <param name="Halcon"></param>
            /// <param name="is_Conglutination">是否粘连</param>
            /// <param name="is_less_defend">少捍</param>
            /// <param name="areas">面积</param>
            /// <param name="hwH"></param>
            /// <param name="hwS"></param>
            /// <param name="hwV"></param>
            /// <returns></returns>
            public bool Solder_joint_inspection(Welding_Spot welding_Spo, HalconRun Halcon, out bool is_Conglutination,
                out bool is_less_defend,out HTuple areas, HWindID hwH = null, RGBHSVEnum hSVRGB = RGBHSVEnum.R, int runID = 0)
            {
                is_less_defend = is_Conglutination = false;
                areas = new HTuple();
                HOperatorSet.ReduceDomain(Halcon.Image(), Halcon.GetModelHaoMatRegion(welding_Spo.HomName, HObject), out HObject hObjectImage);
                HOperatorSet.SmallestRectangle1(hObjectImage, out HTuple row1, out HTuple column1, out HTuple row2, out HTuple column2);
                HOperatorSet.CountChannels(hObjectImage, out HTuple htcon);
                if (htcon != 3)
                {
                    Halcon.AddMessage("图像类型错误,需要3通道图像");
                    return false;
                }
                HOperatorSet.Decompose3(hObjectImage, out HObject ImageR, out HObject ImageG, out HObject ImageB);
                HOperatorSet.TransFromRgb(ImageR, ImageG, ImageB, out HObject H, out HObject S, out HObject V, "hsv");
                HOperatorSet.Threshold(H, out HObject hObject, welding_Spo.H_threa_min, welding_Spo.H_threa_max);
                HOperatorSet.Connection(hObject, out hObject);
                HOperatorSet.AreaCenter(hObject, out HTuple areaH, out HTuple row, out HTuple column);
                HOperatorSet.SelectShape(hObject, out hObject, "area", "and", welding_Spo.H_sele_area_min, 999999);
                HOperatorSet.Threshold(S, out HObject hObjectS, welding_Spo.S_threa_min, welding_Spo.S_threa_max);
                HOperatorSet.Connection(hObjectS, out hObjectS);
                HOperatorSet.AreaCenter(hObjectS, out HTuple areaS, out row, out column);
                HOperatorSet.SelectShape(hObjectS, out hObjectS, "area", "and", welding_Spo.H_sele_area_min, 99999999);
                HOperatorSet.Threshold(V, out HObject hObjectV, welding_Spo.V_threa_min, welding_Spo.V_threa_max);
                HOperatorSet.Threshold(ImageR, out HObject hObjectR, welding_Spo.R_threa_min, welding_Spo.R_threa_max);
                HOperatorSet.Connection(hObjectV, out hObjectV);
                if (hwH != null)
                {
                    hwH.SetPerpetualPart(row1, column1, row2, column2);
                    hwH.ClearObj();
                }
                    if (runID == 3)
                {
                    if (hwH != null)
                    {
                        switch (hSVRGB)
                        {
                            case RGBHSVEnum.R:
                                hwH.SetImaage(ImageR);
                                break;
                            case RGBHSVEnum.G:
                                hwH.SetImaage(ImageG);
                                break;
                            case RGBHSVEnum.B:
                                hwH.SetImaage(ImageB);
                                break;
                            case RGBHSVEnum.H:
                                hwH.SetImaage(H); 
                                hwH.HalconResult.AddObj(hObject);
                                break;
                            case RGBHSVEnum.S:
                                hwH.SetImaage(S);
                                hwH.HalconResult.AddObj(hObjectS);
                                break;
                            case RGBHSVEnum.V:
                                hwH.SetImaage(V);
                                hwH.HalconResult.AddObj(hObjectV);
                                break;
                            default:
                                break;
                        }
                    }
                }
                HOperatorSet.Intersection(hObject, hObjectS, out HObject hObject1);
                HOperatorSet.Intersection(hObject1, hObjectR, out hObject1);
                HOperatorSet.Intersection(hObject1, hObjectV, out hObject1);
                HOperatorSet.AreaCenter(hObject1, out HTuple areaV, out row, out column);
                ///对面积进行检测
                HOperatorSet.Threshold(H, out HObject hObjectHCompute, welding_Spo.H_area_compute_thr_min, welding_Spo.H_area_compute_thr_max);
                HOperatorSet.OpeningCircle(hObjectHCompute, out hObjectHCompute, 5);
                HOperatorSet.Threshold(H, out HObject hObjectHComputeT, 0, 3);
                HOperatorSet.Union2(hObjectHCompute, hObjectHComputeT, out hObjectHCompute);
                HOperatorSet.ClosingCircle(hObjectHCompute, out hObjectHCompute, 5);
                HOperatorSet.Threshold(S, out HObject hObjectSCompute, welding_Spo.S_area_compute_thr_min, welding_Spo.S_area_compute_thr_max);
                if (areaV.Length != 0)
                {
                    if (areaV.TupleMax() < 9999)
                    {
                        HOperatorSet.Threshold(S, out HObject hObjectSCompute2, 200, 255);
                        HOperatorSet.AreaCenter(hObjectSCompute2, out areaV, out row, out column);
                        if (areaV.TupleMax() < 999)
                        {
                            HOperatorSet.Union2(hObjectSCompute2, hObjectSCompute, out hObjectSCompute);
                        }
                    }
                }
                HOperatorSet.FillUp(hObjectSCompute, out hObjectSCompute);
                HOperatorSet.Connection(hObjectSCompute, out hObjectSCompute);
                HOperatorSet.SelectShape(hObjectSCompute, out hObjectSCompute, "area", "and", 30, welding_Spo.Compute_sele_area_max);
                HOperatorSet.Union1(hObjectSCompute, out hObjectSCompute);
                HOperatorSet.ClosingCircle(hObjectSCompute, out hObjectSCompute, 10);
                HOperatorSet.Threshold(V, out HObject hObjectVCompute, welding_Spo.V_area_compute_thr_min, welding_Spo.V_area_compute_thr_max);
                HOperatorSet.FillUp(hObjectVCompute, out hObjectVCompute);
                //HOperatorSet.Threshold(Image3, out HObject hObjectRCompute, welding_Spo.R_area_compute_thr_min, welding_Spo.R_area_compute_thr_max);
                //HOperatorSet.SelectShape(hObjectVCompute, out hObjectVCompute, "circularity", "and", 0.3, 1);
                HOperatorSet.Threshold(ImageB, out HObject hObjectB, welding_Spo.R_area_compute_thr_min, welding_Spo.R_area_compute_thr_max);
                HOperatorSet.ClosingCircle(hObjectB, out hObjectB, 10);
                if (runID == 6)
                {
                    if (hwH != null)
                    {
                        switch (hSVRGB)
                        {
                            case RGBHSVEnum.R:
                                hwH.SetImaage(ImageR);
                                break;
                            case RGBHSVEnum.G:
                                hwH.SetImaage(ImageG);
                                break;
                            case RGBHSVEnum.B:
                                hwH.SetImaage(ImageB);
                                hwH.HalconResult.AddObj(hObjectB);
                                break;
                            case RGBHSVEnum.H:
                                hwH.SetImaage(H);
                                hwH.HalconResult.AddObj(hObjectHCompute);
                                break;
                            case RGBHSVEnum.S:
                                hwH.SetImaage(S);
                                hwH.HalconResult.AddObj(hObjectSCompute);
                                break;
                            case RGBHSVEnum.V:
                                hwH.SetImaage(V);
                                hwH.HalconResult.AddObj(hObjectVCompute);
                                break;
                            default:
                                break;
                        }
                    }
                }
            
                HOperatorSet.Intersection(hObjectHCompute, hObjectSCompute, out HObject hObject2);
                HOperatorSet.Intersection(hObject2, hObjectVCompute, out hObject2);
                HOperatorSet.Intersection(hObject2, hObjectB, out hObject2);
                //HOperatorSet.Intersection(hObject2, hObjectRCompute, out hObject2);
                HOperatorSet.FillUp(hObject2, out hObject2);
                HOperatorSet.Connection(hObject2, out hObject2);

                List<HTuple> listArea = new List<HTuple>();
                HOperatorSet.SelectShape(hObject2, out hObject2, "area", "and", welding_Spo.Compute_sele_area_min, welding_Spo.Compute_sele_area_max);
                HOperatorSet.FillUp(hObject2, out hObjectVCompute);
                if (runID == 6)
                {
                    Halcon.AddOBJ(hObjectVCompute, ColorResult.yellow);
                }
                HObject hObject4 = new HObject();
                hObject4.GenEmptyObj();
                HOperatorSet.Threshold(S, out HObject hObjectS1, welding_Spo.NoNeedle_S_thr_min, welding_Spo.NoNeedle_S_thr_max);
                HOperatorSet.Threshold(V, out HObject hObjectV1, welding_Spo.NoNeedle_V_thr_min, welding_Spo.NoNeedle_V_thr_max);
                HOperatorSet.Threshold(H, out HObject hObjectH1, welding_Spo.NoNeedle_H_thr_min, welding_Spo.NoNeedle_H_thr_max);
                if (runID == 9)
                {
                    if (hwH != null)
                    {
                        switch (hSVRGB)
                        {
                            case RGBHSVEnum.R:
                                hwH.SetImaage(ImageR);
                                break;
                            case RGBHSVEnum.G:
                                hwH.SetImaage(ImageG);
                                break;
                            case RGBHSVEnum.B:
                                hwH.SetImaage(ImageB);
                                hwH.HalconResult.AddObj(hObjectB);
                                break;
                            case RGBHSVEnum.H:
                                hwH.SetImaage(H);
                                hwH.HalconResult.AddObj(hObjectH1);
                                break;
                            case RGBHSVEnum.S:
                                hwH.SetImaage(S);
                                hwH.HalconResult.AddObj(hObjectS1);
                                break;
                            case RGBHSVEnum.V:
                                hwH.SetImaage(V);
                                hwH.HalconResult.AddObj(hObjectV1);
                                break;
                            default:
                                break;
                        }
                    }
                }
                HOperatorSet.Intersection(hObjectS1, hObjectV1, out HObject hObjectt1);
                HOperatorSet.Intersection(hObjectt1, hObjectH1, out hObjectt1);
                HTuple magetSText = new HTuple();
                ///处理焊点
                for (int i = 0; i < ListHObj.Count; i++)
                {
                    string magetStr = (i + 1).ToString();
                    HOperatorSet.AreaCenter(ListHObj[i], out HTuple area, out HTuple rowt, out HTuple columnt);
                    if (rowt.Length == 0)
                    {
                        rowt = 0;
                        columnt = 0;
                    }
                    HObject hObOutCircle;
                    if (this.IsEllIpse)
                    {
                        HOperatorSet.GenEllipse(out hObOutCircle, rowt, columnt, new HTuple(Phi).TupleRad(), new HTuple(this.CiericROut), Radius2);
                    }
                    else
                    {
                        HOperatorSet.GenCircle(out hObOutCircle, rowt, columnt, new HTuple(this.CiericROut));
                    }
                    HOperatorSet.GenCircle(out HObject hObIntCircle, rowt, columnt, new HTuple(this.CiericRInt));
                    HOperatorSet.Difference(hObOutCircle, hObIntCircle, out hObject);
                    HOperatorSet.GenCircle(out HObject hObIntCircleT, rowt, columnt, new HTuple(this.CiericRInt + 10));
                    HOperatorSet.Intersection(hObject1, hObIntCircleT, out hObject4);
                    if (runID == 3)
                    {
                        HOperatorSet.AreaCenter(hObject4, out area, out rowt, out columnt);
                        Halcon.AddOBJ(hObject4, ColorResult.red);
                        Halcon.AddMessageIamge(rowt + 50, columnt, "无针面积" + area, ColorResult.green);
                    }
                    HOperatorSet.SelectShape(hObject4, out hObject4, "area", "and", welding_Spo.H_sele_area_max, 999999);
                    //HOperatorSet.SelectShape(hObject4, out hObject4, "ra", "and", this.CiericRInt*2 , 999999);
                    //HOperatorSet.SelectShape(hObject4, out hObject4, "rb", "and", this.CiericRInt*2 , 999999);
                    HOperatorSet.AreaCenter(hObject4, out area, out rowt, out columnt);
                    if (hObject4.CountObj() > 0)
                    {
                        HOperatorSet.AreaCenter(hObject4, out area, out HTuple rowtt, out HTuple columntt);
                        if (magetStr.Length < 2)
                        {
                            magetStr += "少针:" + area;
                        }
                        Halcon.AddOBJ(hObject4, ColorResult.red);
                    }
                    ///处理少捍
                    //HOperatorSet.Difference(hObOutCircle, hObIntCircle, out hObIntCircle);
                    HOperatorSet.Intersection(hObject1, hObject, out hObject4);
                    if (runID == 3)
                    {
                        Halcon.AddOBJ(hObject4, ColorResult.yellow);
                        HOperatorSet.AreaCenter(hObject4, out area, out HTuple rowtt, out HTuple columntt);
                        Halcon.AddMessageIamge(rowtt + 100, columntt, "少焊面积" + area, ColorResult.red);
                    }
                    HOperatorSet.SelectShape(hObject4, out hObject4, "area", "and", welding_Spo.H_sele_area_min, 999999);
                    HOperatorSet.Connection(hObject4, out hObject4);
                    HOperatorSet.AreaCenter(hObject4, out area, out rowt, out columnt);
                    if (rowt.Length > 0)
                    {
                        HOperatorSet.SelectShape(hObject4, out hObject4, "area", "and", area.TupleMax() - 1, 999999);
                        HOperatorSet.EllipticAxis(hObject4, out HTuple ra, out HTuple rb, out HTuple phi);
                        HOperatorSet.AreaCenter(hObject4, out area, out rowt, out columnt);
                        if (runID == 3)
                        {
                            Halcon.AddMessageIamge(rowt + 140, columnt, "ra" + ra + "rb" + rb, ColorResult.green);
                            HOperatorSet.GenEllipse(out HObject hObject3, rowt, columnt, phi, ra, rb);
                            Halcon.AddOBJ(hObject3, ColorResult.red);
                        }
                        if (area.Length != 0)
                        {
                            HOperatorSet.SelectShape(hObject4, out hObject4, "area", "and", area.TupleMax() - 1, 999999);
                            HOperatorSet.SelectShape(hObject4, out hObject4, "ra", "and", welding_Spo.Welding_ra, 999999);
                            HOperatorSet.SelectShape(hObject4, out hObject4, "rb", "and", welding_Spo.Welding_ra, 999999);
                        }
                    }
                    if (hObject4.CountObj() > 0)
                    {
                        HOperatorSet.AreaCenter(hObject4, out area, out HTuple rowtt, out HTuple columntt);
                        if (magetStr.Length < 2)
                        {
                            magetStr += "少焊:" + area;
                        }
                    }
                    //ListHObj[i] = hObject;
                    HOperatorSet.Intersection(hObIntCircle, hObjectt1, out HObject hObjectt1T);
                    HOperatorSet.AreaCenter(hObIntCircle, out HTuple areat1, out rowt, out columnt);
                    HOperatorSet.FillUp(hObjectt1T, out hObjectt1T);
                    HOperatorSet.AreaCenter(hObjectt1T, out area, out rowt, out columnt);
                    if (area > areat1 * 0.8)
                    {
                        magetStr += "无针";
                        Halcon.AddOBJ(hObjectt1T, ColorResult.red);
                    }
                    //else if (area > areat1 * 0.2)
                    //{
                    //    magetStr += "空焊";
                    //    Halcon.AddOBJ(hObjectt1T, ColorResult.Red);
                    //}
                    HOperatorSet.Threshold(V, out HObject hObject6, 160, 255);
                    HOperatorSet.Intersection(hObIntCircle, hObject6, out hObject6);
                    HOperatorSet.Connection(hObject6, out hObject6);
                    HOperatorSet.SelectShape(hObject6, out hObject6, "circularity", "and", 0.4, 1);
                    HOperatorSet.SelectShape(hObject6, out hObject6, "area", "and", 100, 5000);
                    HOperatorSet.AreaCenter(hObject6, out area, out rowt, out columnt);
                    if (area.Length > 1)
                    {
                        HOperatorSet.SelectShape(hObject6, out hObject6, "area", "and", area.TupleMax() - 1, 5000);
                    }

                    if (runID != 0)
                    {
                        HOperatorSet.AreaCenter(hObject6, out area, out rowt, out columnt);
                        Halcon.AddOBJ(hObject6, ColorResult.red);
                        HOperatorSet.Circularity(hObject6, out HTuple circ);
                        HOperatorSet.GenCrossContourXld(out HObject hObject9, rowt, columnt, 30, 0);
                        Halcon.AddOBJ(hObject9, ColorResult.red);
                        Halcon.AddMessageIamge(rowt + 20, columnt, "圆度" + circ, ColorResult.green);
                    }
                    HOperatorSet.DilationCircle(hObject6, out HObject hObject7, 50);
                    HOperatorSet.Difference(hObject7, hObject6, out hObject6);
                    HOperatorSet.Threshold(V, out hObject7, 0, 40);
                    HOperatorSet.AreaCenter(hObject6, out HTuple areaTd, out row, out column);
                    HOperatorSet.Intersection(hObject6, hObject7, out hObject6);
                    HOperatorSet.AreaCenter(hObject6, out HTuple areaT2, out row, out column);
                    if (areaT2 > areaTd * 0.5)
                    {
                        is_less_defend = true;
                        Halcon.AddOBJ(hObject6, ColorResult.red);
                        magetStr += "爬焊";
                    }
                    if (rowt.Length == 1)
                    {
                        if (this.IsEllIpse)
                        {

                            HOperatorSet.GenEllipse(out hObject6, rowt, columnt, new HTuple(Phi).TupleRad(), new HTuple(this.CiericROut), Radius2);
                        }
                        else
                        {
                            HOperatorSet.GenCircle(out hObject6, rowt, columnt, new HTuple(this.CiericROut));
                        }
                    }
                    else
                    {
                        HOperatorSet.FillUp(ListHObj[i], out hObject6);
                    }
                    HOperatorSet.AreaCenter(hObject6, out areaT2, out row, out column);
                    ///溢焊处理
                    HObject hObject8 = hObjectVCompute.ConcatObj(hObIntCircle);
                    HOperatorSet.Union1(hObject8, out hObject8);
                    HOperatorSet.Connection(hObject8, out hObject8);
                    HOperatorSet.SelectRegionPoint(hObject8, out hObject7, row, column);
                    HOperatorSet.Connection(hObject7, out hObject7);
                    HOperatorSet.Difference(hObject7, hObOutCircle, out HObject hObject5);
                    HOperatorSet.AreaCenter(hObject5, out areaH, out row, out column);
                    if (runID != 0)
                    {
                        Halcon.AddOBJ(hObject6, ColorResult.red);
                        if (areaH != 0) Halcon.AddMessageIamge(row + 60, column, "溢焊面积" + areaH, ColorResult.green);
                    }
                    if (areaH > welding_Spo.Overflow_Welding_Area)
                    {
                        magetStr += "溢焊:" + areaH;  Halcon.AddOBJ(hObject5, ColorResult.red);    is_less_defend = true;
                    }
                    if (magetStr.Length > 2)
                    {
                        HOperatorSet.AreaCenter(hObIntCircle, out area, out rowt, out columnt);
                        is_less_defend = true;
                        Halcon.AddOBJ(hObject, ColorResult.red);
                        magetSText.Append(magetStr);
                    }
                    else Halcon.AddOBJ(hObject, ColorResult.blue);

                    areas.Append(magetStr);
                    Halcon.AddOBJ(hObject4, ColorResult.yellow);
                }
                HOperatorSet.AreaCenter(this.HObject, out areaV, out row, out column);
                if (magetSText.Length > 0)
                {
                    if (welding_Spo.IsScdet)
                    {
                        Halcon.AddMessageIamge(row + welding_Spo.DispRow, column + welding_Spo.DispCow, "NG", ColorResult.red);
                    }
                    else
                    {

                        Halcon.AddMessageIamge(row + welding_Spo.DispRow, column + welding_Spo.DispCow, magetSText.TupleInsert(0, "NG"), ColorResult.red);
                    }
                }
                else
                {
                }
                if (hwH != null)
                {
                    hwH.ShowImage();
                    hwH.ShowObj();
                
                }
                    if (is_Conglutination || is_less_defend)
                {
                    return true;
                }
                return false;
            }


        }

    }
}
