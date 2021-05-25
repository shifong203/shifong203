using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
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
        public override Control GetControl()
        {
            return new Color_DetectionUserControl(this);
        }

        public override RunProgram UpSatrt<T>(string path)
        {
            return base.ReadThis<Color_Detection>(path);
        }
        public Dictionary<string, Color_classify> keyColor { get; set; } = new Dictionary<string, Color_classify>();

        public class Color_classify
        {
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
                get;set;
            }

            /// <summary>
            /// 使用跟随
            /// </summary>
            public bool IsHomMat { get; set; }
            /// <summary>
            /// 使用搜索
            /// </summary>
            public bool EnbleSelect { get; set; }

            public string Name { get; set; }

            [Description(""), Category("图像通道"), DisplayName("通道"),]
            public Vision.ImageTypeObj ImageType { get; set; }


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

            public HObject ModeOBj;
            public HObject DrawObj { get; set; }

            public HObject SeleRoi;

            public List<Threshold_Min_Max> threshold_Min_Maxes = new List<Threshold_Min_Max>();

            public Threshold_Min_Max Threshold_H { get; set; } = new Threshold_Min_Max() ;
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

            public void RunSeleRoi(HObject image, int id,out HObject iamget)
            {
                iamget = new HObject();
                try
                {
                    if (SeleRoi.CountObj()>0)
                    {
                        HOperatorSet.ReduceDomain(image, SeleRoi, out image);
                    }
                    HOperatorSet.Threshold(image , out HObject hobj1, ThresSelectMin, ThresSelectMax);
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
                    else if(ClosingCir !=0)
                    {
                        HOperatorSet.ErosionCircle(iamget, out iamget, Math.Abs( ClosingCir));
                    }
                }
                catch (Exception)
                {
                }
            }
            public bool Classify(HalconRun halcon, OneResultOBj oneResultOBj, HObject drawObj,RunProgram RunPa, out HObject Color_region,
                 List<HObject> Listobjs=null)
            {
                int NGNumber = 0;
                Color_region = halcon.Image();
                HOperatorSet.CountChannels(halcon.Image(), out HTuple htcon);
                if (htcon != 3)
                {
                    halcon.AddMessage("图像类型错误,需要3通道图像");
                    return true;
                }
                HOperatorSet.GetImageSize(Color_region, out HTuple width, out HTuple height);
                HTuple row1 = new HTuple(-1);
                HTuple col1 = new HTuple(-1);
                HObject H = new HObject();
                HObject S = new HObject();
                HObject V = new HObject();
                HObject hObject = drawObj;
                HObject hObjectROI = drawObj;
        
                if (EnbleSelect)
                {
                    this.RunSeleRoi(RunPa.GetEmset(halcon.GetImageOBJ(ImageType)), 0,out   hObject);
                    hObjectROI = hObject;
                    HOperatorSet.AreaCenter(hObjectROI, out HTuple area, out HTuple row, out HTuple column);
                    halcon.AddOBJ(hObjectROI,ColorResult.blue);
                }
                else
                {
                    if (drawObj != null && drawObj.IsInitialized()) { }
                    else
                    {
                        HOperatorSet.GenRectangle1(out hObject, 0, 0, halcon.Height, halcon.Width);
                        hObjectROI = hObject;
                    }
                }
                hObject = hObjectROI;
                HOperatorSet.SmallestRectangle1(hObjectROI, out row1, out col1, out height, out width);
                HObject hObjectH = new HObject();
                HObject hObjectS = new HObject();
                HObject hObjectV = new HObject();
                if (H_enabled) hObjectH = Threshold_H.Threshold(halcon.GetImageOBJ(Vision.ImageTypeObj.H));
                if (S_enabled) hObjectS = Threshold_S.Threshold(halcon.GetImageOBJ(Vision.ImageTypeObj.S));
                if (V_enabled) hObjectV = Threshold_V.Threshold(halcon.GetImageOBJ(Vision.ImageTypeObj.V));
                if (H_enabled && S_enabled)
                {
                    HOperatorSet.Intersection(hObject, hObjectH, out hObjectH);
                    HOperatorSet.Intersection(hObjectH, hObjectS, out hObject);
                }
                if (H_enabled && S_enabled && V_enabled)
                {
                    HOperatorSet.Intersection(hObject, hObjectV, out hObject);
                }
                else if (H_enabled && V_enabled)
                {
                    HOperatorSet.Intersection(hObjectH, hObjectV, out hObject);
                }
                else if (S_enabled && V_enabled)
                {
                    HOperatorSet.Intersection(hObjectS, hObjectV, out hObject);
                }
                else if (H_enabled)
                {
                    hObject = hObjectH;
                }
                else if (S_enabled)
                {
                    hObject = hObjectS;
                }
                else if (V_enabled)
                {
                    hObject = hObjectV;
                }
                if (Listobjs != null)
                {
                    Listobjs.Clear();
                }
                for (int i = 0; i < threshold_Min_Maxes.Count; i++)
                {
                    HOperatorSet.ReduceDomain(halcon.GetImageOBJ(threshold_Min_Maxes[i].ImageTypeObj), hObjectROI, out V);
                    if (threshold_Min_Maxes[i].Enabled)
                    {
                       HObject hObject1= threshold_Min_Maxes[i].Threshold(V);
                        if (Listobjs != null)
                        {
                            Listobjs .Add(hObject1);
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
                if (ClosingCircleValue>0)
                {
                    HOperatorSet.ClosingCircle(hObject, out hObject, ClosingCircleValue);
                }
                if (ClosingCircleValue <0)
                {
                    HOperatorSet.OpeningCircle(hObject, out hObject, Math.Abs( ClosingCircleValue));
                }
                HOperatorSet.Connection(hObject, out hObject);
                if (ISFillUp)
                {
                    HOperatorSet.FillUp(hObject, out hObject);
                }
                Color_region = Max_area.select_shape(hObject);
                if (Listobjs!=null)
                {
                    Listobjs.Insert(0, Color_region);
                }
                if (Color_region.CountObj() != ColorNumber)
                {
                    HTuple Colorid = new HTuple();
                    if (Color_region.CountObj() != 0)
                    {
                        HObject hObject1 = Color_region;
                        HObject hObject2 = Color_region;
                        if (ColorNumber != 0)
                        {
                            HOperatorSet.Union1(hObject1, out hObject1);
                            HOperatorSet.DilationCircle(Color_region, out hObject1, 50);
                            HOperatorSet.Difference(hObjectROI, hObject1, out  hObject2);
                        }
                        HOperatorSet.DilationRectangle1(Color_region, out hObject1, Vision.Instance.DilationRectangle1, Vision.Instance.DilationRectangle1);
                        HOperatorSet.Union1(hObject1, out hObject1);
                        HOperatorSet.SmallestRectangle1(hObject1, out  row1, out  col1, out HTuple row2, out HTuple column2);
                        HOperatorSet.AreaCenter(hObjectROI, out HTuple area, out HTuple row, out HTuple column);
                        HOperatorSet.GenRectangle1(out hObject1, row1, col1, row2, column2);
                        Colorid = HTuple.TupleGenConst(area.Length, Color_ID);
                        if (RunPa.ISShowText)
                        {
                            halcon.AddMessageIamge(row, column, Colorid + Name, ColorResult.yellow);
                        }
                        oneResultOBj.AddNGOBJ( RunPa.Name ,  Name,  hObject1, hObject2 );
                    }
                    else
                    {
                        HOperatorSet.DilationRectangle1(hObjectROI, out HObject hObject1, Vision.Instance.DilationRectangle1, Vision.Instance.DilationRectangle1);
                        oneResultOBj.AddNGOBJ(RunPa.Name, Name, hObject1, Color_region);
                        //oneResultOBj.AddNGOBJ(new OneRObj() { NGText = RunPa.Name + "." + this.Name, ComponentID = this.Name, ROI = hObject1, NGROI = Color_region });
                    }
                    halcon.AddMessage(RunPa.Name + "."+this.Name + ":" + Color_ID + Name + "数量" + Color_region.CountObj());
                    NGNumber++;
                }
                else if(IsColt)
                {
                    HOperatorSet.Connection(hObjectROI, out hObjectROI);
                    HOperatorSet.AreaCenter(hObjectROI, out HTuple area, out HTuple row, out HTuple column);
                    HOperatorSet.GenCrossContourXld(out HObject hObject1, row, column, 50, 0);
                    halcon.AddOBJ(hObject1);
                }
                if (NGNumber!=0)
                {
                    return false;
                }
                return true;
            }
        }
        //[DescriptionAttribute("是否在结果图像中显示文本。"), Category("结果显示"), DisplayName("显示文本")]
        //public bool ISShowText { get; set; }

        public override bool RunHProgram(HalconRun halcon, OneResultOBj oneResultOBj, int id, string name = null)
        {
            foreach (var item in keyColor)
            {
                try
                {
                    if (!item.Value.Enble)
                    {
                        continue;
                    }
                    item.Value.Name = item.Key;
                    if (!item.Value.Classify(halcon, oneResultOBj, item.Value.DrawObj,this, out HObject hObject))
                    {
                        HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple row, out HTuple column);
                        HTuple Colorid = new HTuple();
                        ResltBool = false;
                    }
                }
                catch (Exception)
                {}
            }
            return ResltBool;
        }

    }
}
