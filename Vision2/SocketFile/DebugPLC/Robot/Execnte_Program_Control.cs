using System;
using System.Drawing;
using System.Windows.Forms;


namespace ErosSocket.DebugPLC.Robot
{
    public partial class Execnte_Program_Control : UserControl
    {
        public Execnte_Program_Control()
        {
            InitializeComponent();
            bit = new Bitmap((int)(MonitorDPI / 25.4 * 1 * scaling) - offSetX, (int)(MonitorDPI / 25.4 * 1 * scaling) - offSetX);
            Graphics gh = Graphics.FromImage(bit);
            gh.Clear(this.BackColor);
            gh.DrawRectangle(Pens.Green, new Rectangle(0, 0, (int)(MonitorDPI / 25.4 * 1 * scaling) - offSetX, (int)(MonitorDPI / 25.4 * 1 * scaling) - offSetX));
            gh.Dispose();
            textureBrush = new TextureBrush(bit);//使用TextureBrush可以有效减少窗体拉伸时的闪烁   
        }
        private int originLocation = 10; //坐标原地起始位置
        private int maxScaleX = 1000; //X轴最大刻度
        private int maxScaleY = 1000; //Y轴最大刻度
        private float scaling = 1.0F; //缩放比例
        private int offSetX = 10; //X轴偏移位置
        private int offSetY = 10; //Y轴偏移位置
        private Font font = new Font("Arial", 9); //刻度值显示字体
        private Bitmap bit;
        private TextureBrush textureBrush;
        int x = 0;
        int y = 0;
        bool showRec = false;
        float MonitorDPI = 1000.0f;

        /// <summary>
        /// OnPaint 事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // 使用双缓冲
            this.DoubleBuffered = true;
            // 背景重绘移动到此
            if (this.BackgroundImage != null)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                e.Graphics.DrawImage(
                    this.BackgroundImage,
                    new System.Drawing.Rectangle(0, 0, this.Width, this.Height),
                    0,
                    0,
                    this.BackgroundImage.Width,
                    this.BackgroundImage.Height,
                    System.Drawing.GraphicsUnit.Pixel);
            }
            base.OnPaint(e);
        } /// <summary>
          /// OnPaintBackground 事件
          /// </summary>
          /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // 重载基类的背景擦除函数，
            // 解决窗口刷新，放大，图像闪烁
            return;
        }

        public void SetXValue(Single x)
        {
            try
            {
                double dt = XVale * 0.1;

                int d = (int)(x * 0.1 * 100) - button1.Location.Y;
                int dD = Math.Abs(d) % (int)dt;
                if (d < 0)
                {
                    d = Math.Abs(d) / (int)dt;
                    for (int i = 0; i < d; i++)
                    {
                        button1.Location = new Point(button1.Location.X, button1.Location.Y - (int)dt);
                        toolStripLabel1.Text = "X:" + 0.1 * button1.Location.X + "Y:" + 0.1 * button1.Location.Y;
                        System.Threading.Thread.Sleep(10);
                        if (DebugComp.GetThis().Stop)
                        {
                            DebugComp.GetThis().Stop = false;
                            return;
                        }
                    }
                    button1.Location = new Point(button1.Location.X, button1.Location.Y - (int)dD);
                    toolStripLabel1.Text = "X:" + 0.1 * button1.Location.X + "Y:" + 0.1 * button1.Location.Y;
                }
                else
                {
                    d = Math.Abs(d) / (int)dt;

                    for (int i = 0; i < d; i++)
                    {
                        button1.Location = new Point(button1.Location.X, button1.Location.Y + (int)dt);
                        toolStripLabel1.Text = "X:" + 0.1 * button1.Location.X + "Y:" + 0.1 * button1.Location.Y;
                        System.Threading.Thread.Sleep(10);
                        if (DebugComp.GetThis().Stop)
                        {
                            DebugComp.GetThis().Stop = false;
                            return;
                        }
                        button1.Location = new Point(button1.Location.X, button1.Location.Y - (int)dD);
                        toolStripLabel1.Text = "X:" + 0.1 * button1.Location.X + "Y:" + 0.1 * button1.Location.Y;
                    }

                }



            }
            catch (Exception)
            {
            }
        }
        public Single XVale { get; set; } = 10;
        public Single YVale { get; set; } = 10;
        public void SetYValue(Single y)
        {
            try
            {
                double dt = YVale * 0.1;

                int d = (int)(y * 0.1 * 100) - button1.Location.X;

                if (d < 0)
                {
                    d = Math.Abs(d) / (int)dt;
                    for (int i = 0; i < d; i++)
                    {
                        button1.Location = new Point(button1.Location.X - (int)dt, button1.Location.Y);
                        toolStripLabel1.Text = "X:" + 0.1 * button1.Location.X + "Y:" + 0.1 * button1.Location.Y;
                        System.Threading.Thread.Sleep(10);
                        if (DebugComp.GetThis().Stop)
                        {
                            DebugComp.GetThis().Stop = false;
                            return;
                        }
                    }
                }
                else
                {
                    d = Math.Abs(d) / (int)dt;
                    for (int i = 0; i < d; i++)
                    {
                        button1.Location = new Point(button1.Location.X + (int)dt, button1.Location.Y);
                        toolStripLabel1.Text = "X:" + 0.1 * button1.Location.X + "Y:" + 0.1 * button1.Location.Y;
                        System.Threading.Thread.Sleep(10);
                        if (DebugComp.GetThis().Stop)
                        {
                            DebugComp.GetThis().Stop = false;
                            return;
                        }
                    }
                }
                //button1.Location = new Point((int)(y* Scaling * 100), button1.Location.Y);
                //label1.Text = "X:" + Scaling * button1.Location.X + "Y:" + Scaling * button1.Location.Y;
            }
            catch (Exception)
            {
            }
        }
        private void LinearScale_Paint(object sender, PaintEventArgs e)
        {

            //this.Refresh();
            Graphics g = e.Graphics;
            //g.DrawRectangle(Pens.Black, new Rectangle(30, 30, (int)(Settings.MonitorDPI / 25.4 * 1 * scaling) - offSetX,(int)(Settings.MonitorDPI / 25.4 * 1 * scaling) - offSetX));
            int widthInmm = maxScaleX;
            int heightInmm = maxScaleY;
            originLocation = 0;
            //绘制X轴           
            for (int i = 0; i <= widthInmm; i++)
            {
                SizeF size = g.MeasureString(Convert.ToString(i), font);
                float x = originLocation + (float)(MonitorDPI / 25.4 * i) - offSetX;
                //float x = originLocation +i - offSetX;
                if (x >= originLocation)
                {
                    PointF start = new PointF(x, originLocation);
                    PointF end = new PointF(x, originLocation * 3 / 4);
                    if (i % 5 == 0)
                    {
                        end = new PointF(x, originLocation / 2);
                    }
                    if (i % 10 == 0)
                    {
                        end = new PointF(x, 3);
                        g.DrawString(Convert.ToString(i), font, Brushes.Red, new PointF(x, 0));
                    }
                    g.DrawLine(Pens.Green, start, end);
                }
            }
            g.DrawLine(Pens.Blue, new PointF(originLocation, 5), new PointF(this.Width, 5));

            //绘制Y轴
            for (int i = 0; i <= heightInmm; i++)
            {
                SizeF size = g.MeasureString(Convert.ToString(i), font);
                float y = originLocation + (float)(MonitorDPI / 25.4 * i) - offSetY;
                //Settings.MonitorDPI为一常量，在另外一个类中赋值。                

                if (y >= originLocation)
                {
                    PointF start = new PointF(originLocation, y);
                    PointF end = new PointF(originLocation * 3 / 4, y);
                    if (i % 5 == 0)
                    {
                        end = new PointF(originLocation / 2, y);
                    }
                    if (i % 10 == 0)
                    {
                        end = new PointF(3, y);
                        g.DrawString(Convert.ToString(i), font, Brushes.Red, new PointF(0, y));
                    }
                    g.DrawLine(Pens.Green, start, end);
                }
            }
            //g.DrawString("X:" + button1.Location.X + "Y" + button1.Location.Y, font, Brushes.Red, new PointF(10, 10)); 
            g.DrawLine(Pens.Green, new PointF(originLocation, originLocation), new PointF(originLocation, this.Height));
            g.FillRectangle(textureBrush, new Rectangle(originLocation + (int)(MonitorDPI / 25.4 * 1) - offSetX, originLocation + (int)(MonitorDPI / 25.4 * 1) - offSetX, maxScaleX - originLocation, maxScaleY - originLocation));
            if (showRec)
            {
                g.DrawRectangle(Pens.White, new Rectangle(x * 10, y * 10, 10, 10));
            }
        }

        public void OffSet(int offSetX, int offSetY)
        {
            this.offSetX = offSetX;
            this.offSetY = offSetY;
            this.Refresh();
        }


        #region Scaling
        public float Scaling
        {
            get
            {
                return scaling;
            }
            set
            {
                scaling = value;
                Refresh();
            }
        }
        #endregion

        #region OffSet
        public int OffSetX
        {
            get
            {
                return offSetX;
            }
            set
            {
                offSetX = value;
                Refresh();
            }
        }

        public int OffSetY
        {
            get
            {
                return offSetY;
            }
            set
            {
                offSetY = value;
                Refresh();
            }
        }
        #endregion



        bool isMove;
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    COL = e.X;
                    ROW = e.Y;
                    isMove = true;
                }



            }
            catch (Exception)
            {

            }
        }
        int ROW;
        int COL;

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {

                if (isMove)
                {
                    Scaling = 0.10f;
                    button1.Location = new Point(button1.Location.X + e.X - COL, button1.Location.Y + e.Y - ROW);
                    //    double toH = (double)this.Height / (double)trackBar1.Maximum;
                    //    double toW = (double)this.Width / (double)trackBar2.Maximum;
                    //    double toHT = ((double)(this.Height - trackBar1.Value * toH));
                    //    double toWT = ((double)(this.Width - trackBar2.Value * toW));

                    toolStripLabel1.Text = "X:" + Scaling * button1.Location.X + "Y:" + Scaling * button1.Location.Y;
                }


            }
            catch (Exception)
            {

            }

        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;
        }

        private void Execnte_Program_Control_Load(object sender, EventArgs e)
        {



        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
