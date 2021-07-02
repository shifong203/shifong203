using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using HalconDotNet;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using static Vision2.vision.Vision;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    /// <summary>
    /// 连接器
    /// </summary>
    public class Overgild : RunProgram
    {
        public override Control GetControl(HalconRun halcon )
        {
            return new Controls.OvergildControl1(this);
        }
        public override RunProgram UpSatrt<T>(string Path)
        {
            return base.ReadThis<Overgild>(Path);
        }
        [Description(""), Category("搜索区域"), DisplayName("区域灰度值Min"),]
        public byte ThresSelectMin { get; set; } = 100;
        [Description(""), Category("搜索区域"), DisplayName("区域灰度值Max"),]
        public byte ThresSelectMax { get; set; } = 255;
        [Description(""), Category("搜索区域"), DisplayName("区域筛选Min"),]
        public double SelectMin { get; set; } = 1500;
        [Description(""), Category("搜索区域"), DisplayName("区域筛选Max"),]
        public double SelectMax { get; set; } = 99999999;

        [Description("区域膨胀或缩小，正值放大，负值缩小"), Category("搜索区域"), DisplayName("区域膨胀或缩小"),]
        public double ErosinCircle { get; set; } = 5;

        [Description(""), Category("局部分割"), DisplayName("分割阈值"),]
        public double DnyValue { get; set; } = 5;

        [Description(""), Category("局部分割"), DisplayName("分割区域"),]
        public string DnyTypeValue { get; set; } = "dark";
        [Description(""), Category("划痕处理"), DisplayName("划痕第一次赛选最小长度"),]
        public double SocekSeleMin1 { get; set; } = 5;
        [Description(""), Category("划痕处理"), DisplayName("划痕第二次赛选最小长度"),]
        public double SocekSeleMin2 { get; set; } = 20;
        [Description(""), Category("划痕处理"), DisplayName("划痕赛选最大宽度"),]
        public double SocekSeleMax { get; set; } = 20;
        [Description(""), Category("划痕处理"), DisplayName("划痕第一次赛选后圆闭运算大小"),]
        public double ColsingSocek { get; set; } = 10;

        [Description(""), Category("轮廓检测"), DisplayName("轮廓"),]
        public bool isRoiComparison { get; set; }

        [Description(""), Category("轮廓检测"), DisplayName("最小内圆半径"),]
        public double ComparisonSelecMinR { get; set; } = 2;

        [Description(""), Category("均值幅度"), DisplayName("均值幅度极限"),]
        public double Dicet { get; set; } = 5;
        [Description(""), Category("搜索区域"), DisplayName("倒角尺寸"),]
        public double ChamferPhi { get; set; } = 5;
        public HObject ModeOBj;
        public HObject SelecRoi;
        public HObject Mhimage;
        public List<OvergilEX> RunListOvergil { get; set; } = new List<OvergilEX>();
        HObject R;
        HObject G;
        HObject B;
        HObject H;
        HObject S;
        HObject V;
        HObject Gray;

        void ImageHdt(HObject hObject)
        {
            try
            {
                HOperatorSet.CountChannels(hObject, out HTuple htcon);
                if (htcon == 3)
                {
                    HOperatorSet.Decompose3(hObject, out R, out G, out B);
                    HOperatorSet.TransFromRgb(R, G, B, out H, out S, out V, "hsv");
                    HOperatorSet.Rgb1ToGray(hObject, out Gray);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public HObject GetImageOBJ(ImageTypeObj imageType)
        {

            switch (imageType)
            {
                case ImageTypeObj.Gray:
                    return Gray;
                case ImageTypeObj.R:
                    return R;
                case ImageTypeObj.G:
                    return G;
                case ImageTypeObj.B:
                    return B;
                case ImageTypeObj.H:
                    return H;
                case ImageTypeObj.S:
                    return S;
                case ImageTypeObj.V:
                    return V;
            }
            return Gray;
        }
        /// <summary>
        /// 查找区域并进行均值划痕检测
        /// </summary>
        /// <param name="halcon"></param>
        /// <param name="id"></param>
        /// <param name="errRoi"></param>
        public void RunSeleRoi(OneResultOBj halcon,int id,out HObject errRoi)
        {
            errRoi = new HObject();
            errRoi.GenEmptyObj();
            try
            {
                ImageHdt(halcon.Image);
                HOperatorSet.Threshold(this.GetEmset(halcon.GetHalcon().GetImageOBJ(ImageTypeOb)), out HObject hobj1, ThresSelectMin, ThresSelectMax);
                HOperatorSet.Connection(hobj1, out hobj1);
                HOperatorSet.SelectShape(hobj1, out HObject hObject, "area", "and", SelectMin, SelectMax);
                HOperatorSet.FillUp(hObject, out hObject); 
                if (id != 0)
                {
                    halcon.AddObj(hObject);
                }
                HOperatorSet.ClosingCircle(hObject, out hObject,800);
                HOperatorSet.SmallestRectangle2(hObject, out HTuple rowR, out HTuple colR, out HTuple phiR, out HTuple length1R, out HTuple length2R);
                HOperatorSet.HomMat2dIdentity(out HTuple homMat);
                HObject image = halcon.Image;
                HOperatorSet.GenRectangle2(out HObject rectangle2, rowR, colR, phiR, length1R - ChamferPhi, length2R);
                HOperatorSet.GenRectangle2(out HObject rectangle3, rowR, colR, phiR, length1R, length2R - ChamferPhi);
                HOperatorSet.Union1(rectangle3.ConcatObj(rectangle2), out rectangle3);
                HOperatorSet.ErosionCircle(rectangle3, out rectangle3, ErosinCircle);
                HOperatorSet.Union1(rectangle3.ConcatObj(hObject), out rectangle3);
                HOperatorSet.ReduceDomain(halcon.GetHalcon().GetImageOBJ(ImageTypeOb), rectangle3, out HObject hImage);
                HOperatorSet.Threshold(hImage, out HObject errt, 0, 160);
                HOperatorSet.ClosingCircle(errt, out errt, 5);
                HOperatorSet.OpeningCircle(errt, out errt, 2);
                HOperatorSet.Connection(errt, out errt);
                HOperatorSet.SelectShape(errt, out errt, "area", "and", 100, 999999);
                //HOperatorSet.DilationCircle(errt, out errt, 10);
                if (id!=0)
                {
                    halcon.AddObj(rectangle3, ColorResult.yellow);
                    HOperatorSet.AreaCenter(errt, out HTuple ate, out rowR, out colR );
                    if (rowR.Length!=0)
                    {
                        halcon.AddImageMassage(rowR, colR, ate);
                    }
                }
                halcon.AddObj(errt, ColorResult.red);
                HOperatorSet.ErosionCircle(hObject, out hObject, ErosinCircle);
                SelecRoi = hObject;
                HOperatorSet.ReduceDomain(image, hObject, out  hImage);
                Mhimage = hImage;
                HOperatorSet.MeanImage(hImage, out HObject hObject1, 7, 7);
                HOperatorSet.DynThreshold(hImage, hObject1, out HObject hObject2, DnyValue, DnyTypeValue);
                HOperatorSet.Connection(hObject2, out hObject2);
                HOperatorSet.SelectShape(hObject2, out hObject2, "ra", "and", SocekSeleMin1, 9999999999);
                HOperatorSet.Union1(hObject2,out hObject2);
                HOperatorSet.ClosingCircle(hObject2, out hObject2, ColsingSocek);
                HOperatorSet.Connection(hObject2, out hObject2);
                HOperatorSet.SelectShape(hObject2, out hObject2, "ra", "and", SocekSeleMin2, 9999999999);
                HOperatorSet.SelectShape(hObject2, out hObject2, "inner_radius", "and", 0, SocekSeleMax);
                HOperatorSet.DilationCircle(hObject2, out errRoi, 20);
                HOperatorSet.Intersection(SelecRoi, errRoi, out errRoi);
                HOperatorSet.ReduceDomain(image, errRoi, out  hImage);
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

                    if (area2.Length!=0)
                    {
                        HOperatorSet.Intensity(hObject5, hImage, out HTuple mean2, out deviation);
                        //halcon.AddObj(hObject3, ColorResult.yellow);
                        if (deviation.TupleMax()> Dicet)
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

                HOperatorSet.DilationCircle(hObject4.ConcatObj(errt), out hObject4, 20);
                errRoi = hObject4;
   
                HOperatorSet.Intensity(hObject7, hImage, out mean, out deviation);
                //halcon.AddObj(hObject3,ColorResult.red);
                HOperatorSet.AreaCenter(hObject7, out HTuple area, out HTuple row, out HTuple column);
                if (id!=0)
                {
                    halcon.AddImageMassage(row, column, mean + ":" + deviation);
                }
                //halcon.AddObj(hObject);
              
   
            }
            catch (Exception ex)
            {
            }
        }
        public void RunDebug( OneResultOBj oneResultOBj, int id)
        {
            try
            {
                RunSeleRoi(oneResultOBj, id,out HObject errRoi);
                if (errRoi.CountObj() != 0)
                {
                    oneResultOBj.AddObj(errRoi, ColorResult.red);
                }
                RunListOvergil[id].RunPa(oneResultOBj, this, out HObject hObject1);
                    if (hObject1.CountObj() != 0)
                    {
                        oneResultOBj.AddNGOBJ(this.Name,RunListOvergil[id].ErrText,hObject1, hObject1);
                    }
                
            }
            catch (Exception ex)
            {
            }

        }

        public override bool RunHProgram( OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj)
        {
            oneRObjs = new List<OneRObj>();
            try
            {
                
                RunSeleRoi(oneResultOBj, aoiObj.DebugID, out HObject errRoi);
                //halcon.AddObj(SelecRoi);
                if (errRoi.CountObj() != 0)
                {
                    this.NGNumber++;
                    oneResultOBj.AddObj(errRoi, ColorResult.blue);
                }
                if (isRoiComparison)
                {
                    HOperatorSet.SmallestRectangle2(SelecRoi, out HTuple row, out HTuple col, out HTuple phi, out HTuple length1, out HTuple length2);
                    HOperatorSet.HomMat2dIdentity(out HTuple home2d);
                    HOperatorSet.SmallestRectangle2(ModeOBj, out HTuple row2, out HTuple col2, out HTuple phi2, out HTuple length12, out HTuple length22);
                    HOperatorSet.VectorAngleToRigid(row2, col2,phi2, row, col, phi, out home2d);
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
                    RunListOvergil[i].RunPa(oneResultOBj, this,out HObject hObject1);
    
                    if (hObject1.CountObj() != 0)
                    {
                        HOperatorSet.DilationCircle(hObject1, out HObject hObject, 50);
                        //halcon.AddObj(hObject, ColorResult.firebrick);
                        this.NGNumber++;
                        oneResultOBj.AddNGOBJ(this.Name, RunListOvergil[i].ErrText ,  hObject.Clone() , hObject1 );
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
            public void RunPa(OneResultOBj halcon,Overgild overgild,out HObject errObj)
            {
                errObj = new HObject();
                //halcon.AddObj(overgild.SelecRoi);
                HOperatorSet.ReduceDomain(overgild.GetImageOBJ(ImageType), overgild.SelecRoi, out HObject Himage);
                HOperatorSet.Intensity(overgild.SelecRoi, overgild.GetImageOBJ(ImageType), out HTuple mean, out HTuple deviation);
                HOperatorSet.AreaCenter(overgild.SelecRoi, out HTuple area, out HTuple row, out HTuple column);
      
                //halcon.AddMessageIamge(row, column, mean + ":" + deviation);
        
                HOperatorSet.Threshold(Himage, out HObject hobj1, 0, mean- deviation* DevSecl);
                HOperatorSet.Connection(hobj1, out hobj1);
    
                if (ClosingCir>0)
                {
                    HOperatorSet.ClosingCircle(hobj1, out hobj1, ClosingCir);
                }
                else
                {
                    HOperatorSet.OpeningCircle(hobj1, out hobj1, Math.Abs( ClosingCir));
                }
                HOperatorSet.Connection(hobj1, out hobj1);
                HOperatorSet.SelectShape(hobj1, out HObject hObject, "area", "and", SelectMin, SelectMax);
                HOperatorSet.Union1(hObject, out errObj);
                //Himage= overgild.GetEmset(Himage);
                //halcon.Image(Himage);
                HOperatorSet.Intensity(errObj, overgild.GetImageOBJ(ImageType), out mean, out deviation);
                HOperatorSet.AreaCenter(errObj, out area, out row, out column);
                //HOperatorSet.AutoThreshold(Himage, out  errObj, 0);
                //halcon.AddObj(errObj, ColorResult.blue);
                if (mean.Length!=0)
                {
                    //halcon.AddObj(errObj, ColorResult.blue);
                    //halcon.AddMessageIamge(row, column, mean + ":" + deviation);
                }
            }
        }

    }

}

