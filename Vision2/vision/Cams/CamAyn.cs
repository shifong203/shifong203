using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using HalconDotNet;

namespace NokidaE.vision.Cams
{
    /// <summary>
    /// 摄像头类
    /// </summary>
    public class ImageGraber
    {
        [DllImport("Kernel32.dll")]
        internal static extern void CopyMemory(int dest, int source, int size);


        private HObject ho_Image = null;            //Halcon中采集的图像对象
        private HTuple hv_AcqHandle = null;         //Halcon中摄像头操作句柄
        private PictureBox picCamera = null;        //采集图像显示区
        private Thread imageGrabeThread = null;     //图像采集异步线程       


        /// <summary>
            /// 摄像头初始化成功标志
            /// </summary>
        public bool IsInitSuccess { get; set; }


        /// <summary>
            /// 摄像头处于采集中标志
            /// </summary>
        public bool IsStart { get; set; }


        /// <summary>
            /// 摄像头增益参数
            /// </summary>
        private int gain;
        public int Gain
        {
            get
            {
                return gain;
            }


            set
            {
                try
                {
                    HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "gain", value);
                    gain = value;
                }
                catch (Exception) { }
            }
        }


        /// <summary>
            /// 摄像头快门参数
            /// </summary>
        private int shutter;
        public int Shutter
        {
            get
            {
                return shutter;
            }


            set
            {
                try
                {
                    HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "shutter", value);
                    shutter = value;
                }
                catch (Exception) { }
            }
        }


        //其他摄像头参数如法炮制...


        /// <summary>
            /// 构造函数
            /// </summary>
        public ImageGraber(PictureBox picCamera)
        {
            try
            {
                this.picCamera = picCamera;
                IsInitSuccess = false;
                IsStart = false;
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.OpenFramegrabber("GigEVision2", 1, 1, 0, 0, 0, 0, "interlaced", 8,
                               "gray", -1, "false", "HV-xx51", "1", 1, -1, out hv_AcqHandle);
                HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
                IsInitSuccess = true;
            }
            catch (Exception) { IsInitSuccess = false; }
        }


        /// <summary>
            /// 采集一帧图像
            /// </summary>
            /// <returns></returns>
        public Bitmap GrabSingleFrame()
        {
            try
            {
                Bitmap GrabBitmap = null;
                ho_Image.Dispose();
                HOperatorSet.GrabImageAsync(out ho_Image, hv_AcqHandle, -1);
                ConvertHalconGrayByteImageToBitmap(ho_Image, out GrabBitmap);
                return GrabBitmap;
            }
            catch (Exception) { return null; }
        }


        /// <summary>
            /// 关闭摄像头
            /// </summary>
            /// <returns></returns>
        public bool CloseCamera()
        {
            try
            {
                HOperatorSet.CloseFramegrabber(hv_AcqHandle);
                ho_Image.Dispose();
                return true;
            }
            catch (Exception) { return false; }
        }


        /// <summary>
            /// 图像采集线程函数
            /// </summary>
        private void ImageGrabThread()
        {
            while (true)
            {
                this.picCamera.Image = GrabSingleFrame();
            }
        }


        /// <summary>
            /// 开始采集
            /// </summary>
        public void Start()
        {
            if (!IsStart && imageGrabeThread == null)
            {
                imageGrabeThread = new Thread(new ThreadStart(ImageGrabThread));
                imageGrabeThread.Start();
                IsStart = true;
            }
        }


        /// <summary>
            /// 停止采集
            /// </summary>
        public void Stop()
        {
            if (IsStart && imageGrabeThread != null)
            {
                imageGrabeThread.Abort();
                imageGrabeThread = null;
                IsStart = false;
            }
        }


        /// <summary>
            /// 将Halcon中8位灰度图转换为Bitmap图像
            /// </summary>
            /// <param name="image">Halcon中8位灰度图</param>
            /// <param name="res">.net中Bitmap图像</param>
        private void ConvertHalconGrayByteImageToBitmap(HObject image, out Bitmap res)
        {
            HTuple hpoint, type, width, height;


            const int Alpha = 255;
            int[] ptr = new int[2];
            HOperatorSet.GetImagePointer1(image, out hpoint, out type, out width, out height);


            res = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette pal = res.Palette;
            for (int i = 0; i <= 255; i++)
            {
                pal.Entries[i] = Color.FromArgb(Alpha, i, i, i);
            }
            res.Palette = pal;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = res.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            int PixelSize = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
            ptr[0] = bitmapData.Scan0.ToInt32();
            ptr[1] = hpoint.I;
            if (width % 4 == 0)
                CopyMemory(ptr[0], ptr[1], width * height * PixelSize);
            else
            {
                for (int i = 0; i < height - 1; i++)
                {
                    ptr[1] += width;
                    CopyMemory(ptr[0], ptr[1], width * PixelSize);
                    ptr[0] += bitmapData.Stride;
                }
            }
            res.UnlockBits(bitmapData);
        }


        /// <summary>
            /// 将Halcon中RGB图像转换为Bitmap图像
            /// </summary>
            /// <param name="image">Halcon中RGB图像</param>
            /// <param name="res">.net中Bitmap图像</param>
        private void GenertateRGBBitmap(HObject image, out Bitmap res)
        {
            HTuple hred, hgreen, hblue, type, width, height;
            HOperatorSet.GetImagePointer3(image, out hred, out hgreen, out hblue, out type, out width, out height);
            res = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = res.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
            //unsafe
            //{
            //    byte* bptr = (byte*)bitmapData.Scan0;
            //    byte* r = ((byte*)hred.I);
            //    byte* g = ((byte*)hgreen.I);
            //    byte* b = ((byte*)hblue.I);
            //    for (int i = 0; i < width * height; i++)
            //    {
            //        bptr[i * 4] = (b)[i];
            //        bptr[i * 4 + 1] = (g)[i];
            //        bptr[i * 4 + 2] = (r)[i];
            //        bptr[i * 4 + 3] = 255;
            //    }
            //}
            res.UnlockBits(bitmapData);
        }

    }


}



