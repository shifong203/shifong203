using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using System.IO;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
  public  class Variation_Model
    {
        public Variation_Model()
        {
            Xld.GenEmptyObj();
        }
        public Select_shape_Min_Max select_Shape_Min_Max = new Select_shape_Min_Max();
        /// <summary>
        /// 模型区域
        /// </summary>
        public HObject Xld { get; set; } = new HObject();
        [Description("。"), Category("执行"), DisplayName("使能")]
        public bool Enbler { get; set; }
        [Description("。"), Category("执行"), DisplayName("使用放射变换")]
        public bool IsHomed { get; set; }


        public string Name { get; set; } = "脏污";
        HTuple ID = -10; 
        public HObject  Create_Variation_Model(HObject image)
        {
            HObject hObject1 = image;

            try
            {
                HOperatorSet.ReduceDomain(image, Xld, out HObject hObject);
                switch (PaImageMode)
                {
                    case "sobel_amp":
                        HOperatorSet.SobelAmp(hObject, out hObject1, "sum_abs", PaImageValue);
                        HOperatorSet.BinomialFilter(hObject1, out hObject1, 1, 1);
                        break;
                    default:
                        HOperatorSet.BinomialFilter(hObject, out hObject1, PaImageValue, PaImageValue);
                        break;
                }
             
                HOperatorSet.GetImageSize(image, out HTuple width, out HTuple height);
                try
                {
                    if (ID.H.IsInitialized())
                    {
                        HOperatorSet.ClearVariationModel(ID);
                    }
                }
                catch (Exception)
                {
                }
                HOperatorSet.CreateVariationModel(width, height, CreateModeType, CreateModeMode, out ID);
                if (CreateModeMode=="direct")
                {
                    Prepare_Direct_Variation_Model(hObject, hObject1);
                }

            }
            catch (Exception ex)
            {
                throw ex; 
            }
            return hObject1;
        }


        public void ReadModel(string path )
        {
            try
            {
                if (File.Exists(path+ ".vam"))
                {
                    HOperatorSet.ReadVariationModel(path+ ".vam", out ID);
                }
            }
            catch (Exception)
            {
            }
        }
        public void Write_Variation_Model(string path)
        {
            try
            {
                if (ID.H.IsInitialized())
                {

                }
                HOperatorSet.WriteVariationModel(ID, path+ ".vam");
            }
            catch (Exception ex) 
            {
            }
      
        }

        [Description("PaImageMode 。"), Category("创建模型方式"), DisplayName("创建形变图片模式")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", true, "sobel_amp", "binomial_filter")]
        public string PaImageMode { get; set; } = "sobel_amp";

        [Description("PaImageModeValue。"), Category("创建模型方式"), DisplayName("创建形变值")]
        public double PaImageValue { get; set; } = 3;
        /// <summary>
        /// Suggested values: 'byte', 'int2', 'uint2'
        /// </summary>
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", true, "byte", "int2", "uint2")]
        public string CreateModeType { get; set; } = "byte";

        [Description("CreateModeMode 。"), Category("创建模型"), DisplayName("创建形变模式")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", true, "standard", "robust", "direct")]
        public string CreateModeMode { get; set; } = "direct";


        /// <summary>
        ///  'light', 'dark', 'light_dark'
        /// </summary>
       [Description("CompareMode 。"), Category("创建模型"), DisplayName("创建模板模式")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", true, "absolute", "light", "dark", "light_dark")]
        public string CompareMode { get; set; } = "absolute";

        [Description("图像和变化模型之间的差异的绝对最小阈值。"), Category("创建模型"), DisplayName("绝对最小阈值")]

        /// <summary>
        /// 数字(-array)(真实/整数)图像和变化模型之间的差异的绝对最小阈值。
        /// </summary>
        public int AbsThreshold { get; set; } = 10;
        [Description("阈值的变化基于变化模型的差异。"), Category("创建模型"), DisplayName("差异阈值")]
        /// <summary>
        /// 阈值的变化基于变化模型的差异。
        /// </summary>
        public int VarThreshold { get; set; } = 1;
        [Description("对差异区域闭运算。0不使用"), Category("结果区域"), DisplayName("闭运算圆")]
        /// <summary>
        /// 
        /// </summary>
        public int ClosingCircle { get; set; } = 5;
        [Description("对差异区域开运算。0不使用"), Category("结果区域"), DisplayName("开运算圆")]
        /// <summary>
        /// 
        /// </summary>
        public int OpenCircle { get; set; } = 2;

        [Description(""), Category("结果处理"), DisplayName("首次赛选面积")]
        /// <summary>
        /// 
        /// </summary>
        public int SeleMin { get; set; } = 10;

        [Description("对差异区域开运算。0不使用"), Category("结果处理"), DisplayName("首次开运算")]
        /// <summary>
        /// 
        /// </summary>
        public double OpenInC { get; set; } = 1;
        //[Description("结果区域膨胀。"), Category("结果区域"), DisplayName("膨胀区域")]
        //public int DilationRectangle1 { get; set; } = 100;
        public void Prepare_Direct_Variation_Model(HObject RefImage, HObject VarImage)
        {
            try
            {
                HOperatorSet.PrepareDirectVariationModel(RefImage, VarImage, ID, AbsThreshold, VarThreshold);
            }
            catch (Exception ex)
            {
            }
        }

        public void train_variation_model(HObject image)
        {
            HOperatorSet.TrainVariationModel(image, ID);
        }
        public void get_variation_model( out HObject iamge,out HObject varImage)
        {
            HOperatorSet.GetVariationModel(out  iamge, out  varImage, ID);
        }
        public bool RunPa(HalconRun halcon  , OneResultOBj oneResultOBj,  RunProgram run,List<HTuple> hTuples,HWindID hWindID=null)
        {
            int NGNumber = 0;
            try
            {
                if (CreateModeMode != "direct")
                {
                    HOperatorSet.PrepareVariationModel(ID, AbsThreshold, VarThreshold);
                }
                if (!Enbler)
                {
                    return true;
                }
                HObject hObject4 = Xld;
                HObject hObject = new HObject();
                hObject.GenEmptyObj();
                HObject hObjectRed = new HObject();
                hObjectRed.GenEmptyObj();
                HObject ImageTD = run.GetEmset(halcon.Image());
                for (int i = 0; i < hTuples.Count; i++)
                {
                    HTuple home2d = hTuples[i];
                    HObject hObject3 = new HObject();
                    HObject hObject1 = ImageTD;
                    if (IsHomed)
                    {
                        HOperatorSet.HomMat2dInvert(home2d, out home2d);
                        HOperatorSet.AffineTransImage(hObject1, out hObject1, home2d, "constant", "false");
                        if (hWindID != null)
                        {
                            hWindID.SetImaage(hObject1);
                        }
                    }
                    HOperatorSet.ReduceDomain(hObject1, Xld, out hObject1);
                    HOperatorSet.CompareExtVariationModel(hObject1, out hObject3, ID, CompareMode);
                    if (hWindID != null)
                    {
                        HOperatorSet.SmallestRectangle1(Xld, out HTuple ROW1, out HTuple column1, out HTuple row2, out HTuple column2);
                        hWindID.SetPerpetualPart(ROW1, column1, row2, column2);
                        hWindID.HalconResult.AddObj(hObject3, ColorResult.blue);
                
                
                    }
                    if (hObject3.CountObj()==0)
                    {
                        return true;
                    }
                    if (OpenInC!=0)
                    {
                        HOperatorSet.OpeningCircle(hObject3, out hObject3, OpenInC);
                    }
                    HOperatorSet.Connection(hObject3, out hObject3);
                    HOperatorSet.SelectShape(hObject3, out hObject3, "area", "and", SeleMin, 999999999999999);
                    HOperatorSet.Union1(hObject3, out hObject3);
                    if (ClosingCircle>0)
                    {
                        HOperatorSet.ClosingCircle(hObject3, out hObject3, ClosingCircle);
                    }
                    if (OpenCircle > 0)
                    {
                        HOperatorSet.OpeningCircle(hObject3, out hObject3, Math.Abs(OpenCircle));
                    }
                    if (IsHomed)
                    {
                        HOperatorSet.HomMat2dInvert(home2d, out home2d);
                        HOperatorSet.AffineTransRegion(hObject3, out hObject3, home2d, "nearest_neighbor");
                    }
                     HOperatorSet.Connection(hObject3, out hObject3);
                     hObject1 = select_Shape_Min_Max.select_shape(hObject3);
                    int det = hObject1.CountObj();
                    if (det > 0)
                    {
                        if (hWindID != null)
                        {
                      
                            HOperatorSet.EllipticAxis(hObject1, out HTuple ra, out HTuple rb, out HTuple phi);
                            HOperatorSet.HeightWidthRatio(hObject1, out HTuple height, out HTuple width, out HTuple cir);
                            HOperatorSet.SmallestCircle(hObject1, out HTuple row, out HTuple column, out HTuple radius);
                            HOperatorSet.AreaCenter(hObject1, out HTuple area, out  row, out  column);
                            hWindID.HalconResult.AddImageMassage(row, column, "面积" + area.TupleString("0.3f") + "长" + ra.TupleString("0.3f") + "rb" + rb.TupleString("0.3f") + "高" +
                            height.TupleString("0.3f") + "宽" + width.TupleString("0.3f") + "半径" + radius.TupleString("0.3f"));
                            hWindID.HalconResult.AddImageMassage(row + 40, column, "MM:面积" + Math.Sqrt(halcon.GetCaliConstMM(area)).ToString("0.000") + "长" + halcon.GetCaliConstMM(ra).TupleString("0.3f")
                                + "rb" + halcon.GetCaliConstMM(rb).TupleString("0.3f") + "高" + halcon.GetCaliConstMM(height).TupleString("0.3f") + "宽" + halcon.GetCaliConstMM(width).TupleString("0.3f") +
                                "半径" + halcon.GetCaliConstMM(radius).TupleString("0.3f"));
                            hWindID.HalconResult.AddObj(hObject1,ColorResult.red);
                            hWindID.ShowObj();
                        }
                        HOperatorSet.Union1(hObject1, out hObject1);
                        HOperatorSet.FillUp(hObject1, out hObject1);
                        HOperatorSet.DilationRectangle1(hObject1, out hObject3, Vision.Instance.DilationRectangle1, Vision.Instance.DilationRectangle1);
                        HOperatorSet.Connection(hObject3, out hObject3);
                        if (hObject3.IsInitialized())
                        {
                            //HOperatorSet.SmallestRectangle1(hObject3, out HTuple row1, out HTuple colu1, out HTuple row2, out HTuple colu2);
                            //HOperatorSet.GenRectangle1(out HObject hObject2, row1, colu1, row2, colu2);
                            hObject = hObject.ConcatObj(hObject3);
                            //HOperatorSet.SmallestCircle(hObject3, out row1,out colu1, out HTuple radius);
                            //HOperatorSet.GenCircle(out hObject3, row1, colu1, radius);
                            NGNumber++;
                            hObjectRed = hObjectRed.ConcatObj(hObject1);
                        }
                    }
                }
                if (NGNumber>0)
                {
                    oneResultOBj.AddNGOBJ(run.Name, Name, hObject, hObjectRed);
                    //oneResultOBj.AddNGOBJ(new OneRObj() { NGText = run.Name + ":" + Name, ROI = hObject, NGROI = hObjectRed });
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

    }
}
