using HalconDotNet;
using System;
using System.ComponentModel;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public class TiffeOffsetImageEX
    {
        [DescriptionAttribute("拼接行数。"), Category("排列"), DisplayName("图像行")]
        public int ImageNumberROW { get; set; } = 2;

        [DescriptionAttribute("拼接列数。"), Category("排列"), DisplayName("图像列")]
        public int ImageNumberCol { get; set; } = 2;

        [Browsable(false)]
        public HTuple Rows { get; set; } = new HTuple();

        [Browsable(false)]
        public HTuple Cols { get; set; } = new HTuple();

        [Browsable(false)]
        public HTuple Rows1 { get; set; } = new HTuple();

        [Browsable(false)]
        public HTuple Cols1 { get; set; } = new HTuple();

        [Browsable(false)]
        public HTuple Rows2 { get; set; } = new HTuple();

        [Browsable(false)]
        public HTuple Cols2 { get; set; } = new HTuple();

        [DescriptionAttribute("1使用相机图像，2使用执行后图像。"), Category("图像"), DisplayName("图像源")]
        public byte ISHomdeImage { get; set; } = 2;

        [DescriptionAttribute("计算拼接的起点左上点。"), Category("拼图位移"), DisplayName("起点左上Row")]
        public int OriginRow { get; set; }

        [DescriptionAttribute("计算拼接的起点左上点。"), Category("拼图位移"), DisplayName("起点左上Col")]
        public int OriginCol { get; set; }

        [DescriptionAttribute("计算拼接的左上点。"), Category("拼图位移"), DisplayName("剪切起点左上Row")]
        public int CutRow1 { get; set; }

        [DescriptionAttribute("计算拼接的左上点。"), Category("拼图位移"), DisplayName("剪切起点左上Col")]
        public int CutCol1 { get; set; }

        [DescriptionAttribute("计算拼接的右下。"), Category("拼图位移"), DisplayName("剪切终点右下Row")]
        public int CutRow2 { get; set; }

        [DescriptionAttribute("计算拼接的右下。"), Category("拼图位移"), DisplayName("剪切终点右下Col")]
        public int CutCol2 { get; set; }

        [DescriptionAttribute("单张图像缩放比例。"), Category("图像"), DisplayName("缩放比例")]
        public double ZoomImageSize { get; set; } = 1;

        [DescriptionAttribute("图像宽。"), Category("图像"), DisplayName("整图像宽")]
        public int WidthI { get; set; }

        [DescriptionAttribute("图像高。"), Category("图像"), DisplayName("整图像高")]
        public int HeightI { get; set; }

        [DescriptionAttribute("单张图像宽。"), Category("图像"), DisplayName("图像宽")]
        public int ImageWidthI { get; set; } = 6000;

        [DescriptionAttribute("单张图像高。"), Category("图像"), DisplayName("图像高")]
        public int ImageHeightI { get; set; } = 4000;

        [DescriptionAttribute("false横向排序,图像第二张往右排序，True纵向图像第二张往下方排序"), Category("排列"), DisplayName("横或纵向")]
        public bool Vertical { get; set; }

        [DescriptionAttribute("使用平铺=0，还是手动评图=1，自动评图。"), Category("排列"), DisplayName("使用平铺")]
        public int IsFill { get; set; }

        [DescriptionAttribute("true=彩色。"), Category("图像"), DisplayName("彩色或黑白")]
        public bool ImageByteT { get; set; } = true;

        public HTuple zoonRow { get; set; }

        public HTuple zoonCol { get; set; }

        private HObject[] Imgaes;

        public void TiffeClose()
        {
            if (ISHomdeImage == 1)
            {
                if (Imgaes != null)
                {
                    for (int i = 0; i < Imgaes.Length; i++)
                    {
                        if (Imgaes[i] != null)
                        {
                            Imgaes[i].Dispose();
                        }
                    }
                }
            }
            Imgaes = new HObject[ImageNumberROW * ImageNumberCol];
        }

        /// <summary>
        ///
        /// </summary>
        public void SetTiffeOff()
        {
            try
            {
                Imgaes = new HObject[ImageNumberROW * ImageNumberCol];
                if (ZoomImageSize == 0)
                {
                    ZoomImageSize = 1;
                }
                HOperatorSet.GenImageConst(out hObjectT, "byte", ImageWidthI / ZoomImageSize, ImageHeightI / ZoomImageSize);
                if (ImageByteT)
                {
                    HOperatorSet.Compose3(hObjectT, hObjectT, hObjectT, out hObjectT);
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void CatTimetPoint()
        {
            if (Vertical)
            {
                Rows = new HTuple();
                Cols = new HTuple();
                zoonCol = new HTuple();
                zoonRow = new HTuple();
                Rows1 = HTuple.TupleGenConst(ImageNumberCol * ImageNumberROW, -1);
                Cols1 = HTuple.TupleGenConst(ImageNumberCol * ImageNumberROW, -1);
                Rows2 = HTuple.TupleGenConst(ImageNumberCol * ImageNumberROW, -1);
                Cols2 = HTuple.TupleGenConst(ImageNumberCol * ImageNumberROW, -1);
                for (int i = 0; i < ImageNumberCol; i++)
                {
                    for (int j = 0; j < ImageNumberROW; j++)
                    {
                        int sd = (i * ImageNumberROW + j);
                        //int dt = (i * ImageNumberROW + j) % ImageNumberROW;
                        double rowzone = 0;
                        double colszone = 0;
                        if (j == 0)
                        {
                            rowzone = 0;
                            Rows1[sd] = 0;
                        }
                        else
                        {
                            Rows1[sd] = CutRow1;
                            rowzone = OriginRow * (j) + ((j - 1) * OriginRow); ;
                        }
                        if (i == 0)
                        {
                            colszone = 0;
                            Cols1[sd] = 0;
                        }
                        else
                        {
                            colszone = OriginCol * i + ((i - 1) * OriginCol);
                            Cols1[sd] = CutCol1;
                        }
                        if (i == ImageNumberCol - 1)
                        {
                            Cols2[sd] = (int)(ImageWidthI / ZoomImageSize);
                        }
                        else
                        {
                            Cols2[sd] = CutCol2;
                        }
                        if (j == ImageNumberROW - 1)
                        {
                            Rows2[sd] = (int)(ImageHeightI / ZoomImageSize);
                        }
                        else
                        {
                            Rows2[sd] = CutRow2;
                        }
                        zoonRow.Append(rowzone);
                        zoonCol.Append(colszone);
                        Rows.Append((int)(ImageHeightI / ZoomImageSize * j) - rowzone);
                        Cols.Append((int)(ImageWidthI / ZoomImageSize * i) - colszone);
                    }
                }
            }
            else
            {
            }
        }

        public void SetTiffeOff(HObject iamge, int number = -1)
        {
            if (Imgaes == null)
            {
                SetTiffeOff();
            }
            if (number < 0)
            {
                for (int i = 0; i < Imgaes.Length; i++)
                {
                    if (Imgaes[i] == null)
                    {
                        Imgaes[i] = iamge;
                        return;
                    }
                }
            }
            else
            {
                number = number - 1;
                if (Imgaes.Length > number)
                {
                    Imgaes[number] = iamge;
                }
            }
        }

        private HObject hObjectT = new HObject();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public HObject TiffeOffsetImage(string name = "")
        {
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            try
            {
                //if (Rows1.Length != Imgaes.Length)
                //{
                //    Rows1 = HTuple.TupleGenConst(Imgaes.Length, -1);
                //    Cols1 = HTuple.TupleGenConst(Imgaes.Length, -1);
                //    Rows2 = HTuple.TupleGenConst(Imgaes.Length, -1);
                //    Cols2 = HTuple.TupleGenConst(Imgaes.Length, -1);
                //}
                for (int i = 0; i < Imgaes.Length; i++)
                {
                    if (Imgaes[i] == null)
                    {
                        Imgaes[i] = hObjectT;
                    }
                    else
                    {
                        HOperatorSet.ZoomImageSize(Imgaes[i], out HObject hObject1, ImageWidthI / ZoomImageSize, ImageHeightI / ZoomImageSize, "constant");
                        hObject = hObject.ConcatObj(hObject1);
                    }
                }
                int d = hObject.CountObj();
                if (IsFill == 0)
                {
                    if (Vertical)
                    {
                        HOperatorSet.TileImages(hObject, out hObject, ImageNumberCol, "vertical");
                    }
                    else
                    {
                        HOperatorSet.TileImages(hObject, out hObject, ImageNumberCol, "horizontal");
                    }
                }
                else if (IsFill == 1)
                {
                    HOperatorSet.TileImagesOffset(hObject, out hObject, Rows, Cols, Rows1, Cols1, Rows2, Cols2, WidthI, HeightI);
                }
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine(name + "拼图失败，行:" + ex.StackTrace.Remove(0, ex.StackTrace.Length - 5) + ":" + ex.Message);
            }
            return hObject;
        }

        public HObject TiffeOffsetImage(HObject[] hObjects)
        {
            HObject hObjectImage = new HObject();
            hObjectImage.GenEmptyObj();
            if (hObjects.Length == 1)
            {
                return hObjects[0];
            }
            try
            {
                //if (Rows1.Length != hObjects.Length)
                //{
                //    Rows1 = HTuple.TupleGenConst(hObjects.Length, -1);
                //    Cols1 = HTuple.TupleGenConst(hObjects.Length, -1);
                //    Rows2 = HTuple.TupleGenConst(hObjects.Length, -1);
                //    Cols2 = HTuple.TupleGenConst(hObjects.Length, -1);
                //}
                for (int i = 0; i < hObjects.Length; i++)
                {
                    if (hObjects[i] == null)
                    {
                        hObjects[i] = hObjectT;
                    }
                    else
                    {
                        HOperatorSet.ZoomImageSize(hObjects[i], out HObject hObject1, ImageWidthI / ZoomImageSize, ImageHeightI / ZoomImageSize, "constant");
                        hObjectImage = hObjectImage.ConcatObj(hObject1);
                    }
                }
                int d = hObjectImage.CountObj();
                if (IsFill == 0)
                {
                    if (Vertical)
                    {
                        HOperatorSet.TileImages(hObjectImage, out hObjectImage, ImageNumberCol, "vertical");
                    }
                    else
                    {
                        HOperatorSet.TileImages(hObjectImage, out hObjectImage, ImageNumberCol, "horizontal");
                    }
                }
                else if (IsFill == 1)
                {
                    HOperatorSet.TileImagesOffset(hObjectImage, out hObjectImage, Rows, Cols, Rows1, Cols1, Rows2, Cols2, WidthI, HeightI);
                }
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("拼图失败，行:" + ex.StackTrace.Remove(0, ex.StackTrace.Length - 5) + ":" + ex.Message);
            }
            return hObjectImage;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public HObject TiffeOffSetImageFill()
        {
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            try
            {
                for (int i = 0; i < Imgaes.Length; i++)
                {
                    if (Imgaes[i] == null)
                    {
                        Imgaes[i] = hObjectT;
                    }
                    else
                    {
                        HOperatorSet.ZoomImageSize(Imgaes[i], out HObject hObject1, ImageWidthI / ZoomImageSize, ImageHeightI / ZoomImageSize, "constant");
                        hObject = hObject.ConcatObj(hObject1);
                    }
                }
                if (Vertical)
                {
                    HOperatorSet.TileImages(hObject, out hObject, ImageNumberCol, "vertical");
                }
                else
                {
                    HOperatorSet.TileImages(hObject, out hObject, ImageNumberCol, "horizontal");
                }
                //HOperatorSet.TileImages(hObject, out hObject, ImageNumberCol, "vertical");
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("拼图失败:" + ex.Message);
            }

            return hObject;
        }
    }
}