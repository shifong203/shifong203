using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using ThoughtWorks.QRCode.Codec;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace NokidaE.vision.HalconRunFile.RunProgramFile
{
    /// <summary>
    /// 描述：条形码和二维码帮助类
    /// 时间：2018-02-18
    /// BarcodeWriter 用于生成图片格式的条码类，通过Write函数进行输出。继承关系如上图所示。
    ///    BarcodeFormat 枚举类型，条码格式
    ///    QrCodeEncodingOptions 二维码设置选项，继承于EncodingOptions，主要设置宽，高，编码方式等信息。
    ///MultiFormatWriter 复合格式条码写码器，通过encode方法得到BitMatrix。
    ///BitMatrix 表示按位表示的二维矩阵数组，元素的值用true和false表示二进制中的1和0。
    /// </summary>
    public class BarcodeHelper
    {
        /// <summary>
        /// 生成二维码UTF8
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public static Bitmap Generate1(string text, int height, int width)
        {
            if (text.Length > 1800)
            {
                return null;
            }
            if (width < 50)
            {
                width = 400;
            }
            if (height < 50)
            {
                height = 400;
            }
            //width = 400;
            //height = 400;
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions options = new QrCodeEncodingOptions()
            {
                DisableECI = true,//设置内容编码
                CharacterSet = "UTF-8",
                QrVersion = 40,
                //设置二维码的宽度和高度
                Width = width,
                Height = height,
                Margin = 0,//设置二维码的边距,单位不是固定像素
                ErrorCorrection = ErrorCorrectionLevel.H,
            };
            writer.Options = options;
            //writer.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.L);
            return writer.Write(text);
        }




        /// <summary>
        /// 生成一维条形码
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public static Bitmap Generate2(string text, int width, int height)
        {
            BarcodeWriter writer = new BarcodeWriter();
            //使用ITF 格式，不能被现在常用的支付宝、微信扫出来
            //如果想生成可识别的可以使用 CODE_128 格式
            //writer.Format = BarcodeFormat.ITF;
            writer.Format = BarcodeFormat.CODE_39;
            EncodingOptions options = new EncodingOptions()
            {
                Width = width,
                Height = height,
                Margin = 2
            };
            writer.Options = options;
            Bitmap map = writer.Write(text);
            return map;
        }

        /// <summary>
        /// 生成带Logo的二维码,UTF8格式
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public static Bitmap Generate3(string text, int width, int height)
        {
            //Logo 图片
            string logoPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\img\logo.png";
            Bitmap logo = new Bitmap(logoPath);
            //构造二维码写码器
            MultiFormatWriter writer = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>();
            hint.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            hint.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            //hint.Add(EncodeHintType.MARGIN, 2);//旧版本不起作用，需要手动去除白边

            //生成二维码 
            BitMatrix bm = writer.encode(text, BarcodeFormat.QR_CODE, width + 30, height + 30, hint);
            bm = deleteWhite(bm);
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            Bitmap map = barcodeWriter.Write(bm);

            //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
            int[] rectangle = bm.getEnclosingRectangle();

            //计算插入图片的大小和位置
            int middleW = Math.Min((int)(rectangle[2] / 3), logo.Width);
            int middleH = Math.Min((int)(rectangle[3] / 3), logo.Height);
            int middleL = (map.Width - middleW) / 2;
            int middleT = (map.Height - middleH) / 2;

            Bitmap bmpimg = new Bitmap(map.Width, map.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmpimg))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(map, 0, 0, width, height);
                //白底将二维码插入图片
                g.FillRectangle(Brushes.White, middleL, middleT, middleW, middleH);
                g.DrawImage(logo, middleL, middleT, middleW, middleH);
            }
            return bmpimg;
        }

        /// <summary>
        /// 删除默认对应的空白
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private static BitMatrix deleteWhite(BitMatrix matrix)
        {
            int[] rec = matrix.getEnclosingRectangle();
            int resWidth = rec[2] + 1;
            int resHeight = rec[3] + 1;

            BitMatrix resMatrix = new BitMatrix(resWidth, resHeight);
            resMatrix.clear();
            for (int i = 0; i < resWidth; i++)
            {
                for (int j = 0; j < resHeight; j++)
                {
                    if (matrix[i + rec[0], j + rec[1]])
                        resMatrix[i, j] = true;
                }
            }
            return resMatrix;
        }
    }

    public class QRcode
    {
        /// <summary>
        /// 生成二维码（中间不带LOGO）
        /// </summary>
        /// <param name="absoluteSave">保存的绝对路径</param>
        /// <param name="qrdata">要附加的内容</param>
        /// <returns></returns>
        public static Image CreateQRCode(string qrdata, int heigth, int widgth)
        {
            try
            {
                if (qrdata.Length > 1800)
                {
                    return null;
                }
                QRCodeEncoder encoder = new QRCodeEncoder();
                encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//编码方式(注意：BYTE能支持中文，ALPHA_NUMERIC扫描出来的都是数字)
                encoder.QRCodeScale = 40;//大小(值越大生成的二维码图片像素越高)
                encoder.QRCodeVersion = 0;//版本(注意：设置为0主要是防止编码的字符串太长时发生错误)
                encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;//错误效验、错误更正(有4个等级)

                encoder.QRCodeBackgroundColor = System.Drawing.Color.White;

                encoder.QRCodeForegroundColor = System.Drawing.Color.Black;
                System.Drawing.Image image = encoder.Encode(qrdata, Encoding.UTF8);
                //image.Height = heigth;
                //image.Width = widgth;

                return image;
            }
            catch (Exception e)
            {
                //一些操作
            }
            return null;
        }

        //写二维码
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="size"></param>
        /// <param name="qulity"></param>
        /// <returns></returns>
        public Image ErQ(string txt, int size, int qulity)
        {
            if (txt != "")
            {
                //字符类型
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();

                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                //尺寸
                qrCodeEncoder.QRCodeScale = size;//默认2
                                                 //打印容量
                qrCodeEncoder.QRCodeVersion = qulity; //默认12
                                                      //条形码质量
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                //加入内容
                return qrCodeEncoder.Encode(txt);

            }
            else
            {
                return null;
            }
        }
        //合成图片在二维码里加图标         
        public Image CompeImg(PictureBox p1, PictureBox p2, int L, int T, int w, int h)
        {
            Image imgBack = p1.Image;
            Image img = p2.Image;
            Graphics g = Graphics.FromImage(imgBack);
            g.DrawImage(img, L, T, w, h);
            GC.Collect();
            return imgBack;
        }
    }

}
