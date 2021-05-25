using System;
using System.Drawing;

namespace Vision2.ErosUI
{
    ///   
        /// 说明：实时图片生成类，在本例中横向坐标上每个像素都会有一个控制点  
        /// 实际开发中可以减少控制点，比如每5个像素用一个控制点  
        /// 这样的效果或许更加逼真  
        /// 作者：周公  
        /// 日期：2008-07-21  
        /// 首发地址：<a href="http://blog.csdn.net/zhoufoxcn/archive/2008/07/21/2682027.aspx" target="_blank">http://blog.csdn.net/zhoufoxcn/archive/2008/07/21/2682027.aspx</a>  
        ///   
    public class RealTimeImageMaker
    {
        private int width;//要生成的曲线图的宽度  
        private int height;//要生成的曲线图的高度  
        private Point[] pointList;//用来绘制曲线图的关键点，依次将这些点连接起来即得到曲线图  
        private Random random = new Random();//用于生成随机数  
        private Bitmap currentImage;//当前要绘制的图片  
        private Color backColor;//图片背景色  
        private Color foreColor;//图片前景色  

        ///   
        /// 图片的高度  
        ///   
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        ///   
                /// 图片的宽度  
                ///   
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        ///   
                /// 构造函数，指定生成的曲线图的宽度和高度  
                ///   
                /// 要生成的曲线图的宽度  
                /// 要生成的曲线图的高度  
        public RealTimeImageMaker(int width, int height) : this(width, height, Color.Gray, Color.Blue)
        {
        }

        ///   
                /// 构造函数，指定生成的曲线图的宽度、高度及背景色和前景色  
                ///   
                /// 要生成的曲线图的宽度  
                /// 要生成的曲线图的高度  
                /// 曲线图背景色  
                /// 曲线图前景色  
        public RealTimeImageMaker(int width, int height, Color backColor, Color foreColor)
        {
            this.width = width;
            this.height = height;
            this.backColor = backColor;
            this.foreColor = foreColor;
            pointList = new Point[width];
            Point tempPoint;
            //初始化曲线上的所有点坐标  
            for (int i = 0; i < width; i++)
            {
                tempPoint = new Point();
                //曲线的横坐标沿x轴依次递增，在横向位置上每个像素都有一个点  
                tempPoint.X = i;
                //曲线上每个点的纵坐标随机生成，但保证在显示区域之内  
                tempPoint.Y = random.Next() % height;
                pointList[i] = tempPoint;
            }
        }

        ///   
                /// 获取当前依次连接曲线上每个点绘制成的曲线  
                ///   
                ///   
        public Image GetCurrentCurve()
        {
            //currentImage = historyImage.Clone(new Rectangle(1, 0, width - 1, height), PixelFormat.Format24bppRgb);  
            currentImage = new Bitmap(width, height);
            Point p;
            //将当前定位曲线图的坐标点前移，并且将横坐标减1，  
            //这样做的效果相当于移除当前第一个点  
            for (int i = 0; i < width - 1; i++)
            {
                p = pointList[i + 1];
                pointList[i] = new Point(p.X - 1, p.Y);
            }
            Point tempPoint = new Point();
            //新生成曲线图定位点的最后一个点的坐标  
            tempPoint.X = width;
            //曲线上每个点的纵坐标随机生成，但保证在显示区域之内  
            tempPoint.Y = random.Next(DateTime.Now.Millisecond) % height;
            //在最后再添加一个新坐标点  
            pointList[width - 1] = tempPoint;
            Graphics g = Graphics.FromImage(currentImage);
            g.Clear(backColor);
            //绘制曲线图  
            g.DrawLines(new Pen(foreColor), pointList);
            g.Dispose();
            return currentImage;
        }
    }
}