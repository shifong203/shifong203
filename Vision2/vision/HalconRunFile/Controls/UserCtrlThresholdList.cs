using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    //public delegate void ThresholdChangedhandle(Threshold_Min_Max threshold_Min_);

    partial class UserCtrlThresholdList : UserControl
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public UserCtrlThresholdList()
        {
            InitializeComponent();
            //MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            try
            {
                MinValline.Interval = 0;
                MinValline.IntervalOffset = MinVal;
                MinValline.StripWidth = 1;
                MinValline.BackColor = Color.Green;
                MinValline.BorderDashStyle = ChartDashStyle.Dash;
                chart1.ChartAreas[0].AxisX.StripLines.Add(MinValline);

                MaxValline.Interval = 0;
                MaxValline.IntervalOffset = MaxVal;
                MaxValline.StripWidth = 1;
                MaxValline.BackColor = Color.Blue;
                MaxValline.BorderDashStyle = ChartDashStyle.Dash;
                chart1.ChartAreas[0].AxisX.StripLines.Add(MaxValline);
                //X轴          
                for (int i = 0; i < 256; i++)
                {
                    listx.Add(i);
                }
                for (int i = 0; i < 256; i++)
                {
                    listy.Add(100);
                }
                IniDraw();
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            try
            {
                if (IsMinValS)
                {
                    if (e.Delta<0)
                    {
                        MinVal--;
                    }
                    else
                    {
                        MinVal++;
                    }               
                    MinValline.StripWidth = 2;
          
                }
                else
                {
                    if (e.Delta < 0)
                    {
                        MaxVal--;
                    }
                    else
                    {
                        MaxVal++;
                    }
                    MaxValline.StripWidth = 2;
                }
                FuctionVal(MinVal, MaxVal);
                ThresholdChanged?.Invoke(threshold_Min_Max);
                Graphics Grap = chart1.CreateGraphics();
                SolidBrush brush = new SolidBrush(Color.Blue);

                chart1.Refresh();
                Grap.DrawString(MinVal.ToString(), this.Font, brush, e.X, 10);
                Grap.DrawString(MaxVal.ToString(), this.Font, brush, e.X, 30);

                chart1.Update();
            }
            catch (Exception)
            {
            }
            base.OnMouseWheel(e);   
        }

        List<Threshold_Min_Max> threshold_Min_MaxList;

        Threshold_Min_Max threshold_Min_Max;

        public void SetData(List<Threshold_Min_Max> threshold_Min_,HObject images)
        {
            threshold_Min_MaxList = threshold_Min_;
            try
            {
                HImage hImage = new HImage(images);
                Fuction(hImage);
                FuctionVal(threshold_Min_Max.Min, threshold_Min_Max.Max);
            }
            catch (Exception)
            {
            }
            
        }
        /// <summary>
        /// 绝对灰度直方图
        /// </summary>
        HTuple AbsoluteHisto = new HTuple();
        /// <summary>
        /// 相对灰度直方图
        /// </summary>
        HTuple RelativeHisto = new HTuple();
        /// <summary>
        /// 最小值
        /// </summary>
        public int MinVal {
            get { return minV; }
            set { 
                minV = value;
                if (threshold_Min_Max != null)
                {
                    threshold_Min_Max.Min =(byte) value;
                }
            }
        } 
        int minV = 0;
        /// <summary>
        /// 最大值
        /// </summary>
        public int MaxVal {
            get { return maxV; }
            set {
                    if (threshold_Min_Max!=null)
                    {
                      threshold_Min_Max.Max = (byte)value;
                    }
                maxV = value;
            }
        }

        int maxV = 190;

        bool IsMinValS;
        /// <summary>
        /// 最小值的可移动线条
        /// </summary>
        StripLine MinValline = new StripLine();
        /// <summary>
        /// 最大值的可移动线条
        /// </summary>
        StripLine MaxValline = new StripLine();
        /// <summary>
        /// 鼠标被按下标志
        /// </summary>
        bool IsMouseDown = false;
        /// <summary>
        /// 最小值的被选中标志
        /// </summary>
        bool IsMinValSelected = false;
        /// <summary>
        /// 最大值的被选中标志
        /// </summary>
        bool IsMaxValSelected = false;
        /// <summary>
        /// 最大值在变化中的标志
        /// </summary>
        bool IsMaxValChanging = false;
        /// <summary>
        /// 最小值在变化中的标志
        /// </summary>
        bool IsMinValChanging = false;
        /// <summary>
        /// 二值化变化事件
        /// </summary>
        public event ThresholdChangedhandle ThresholdChanged;

        /// <summary>
        /// X轴
        /// </summary>
        List<int> listx = new List<int>();

        /// <summary>
        /// Y轴
        /// </summary>
        List<double> listy = new List<double>();

   


        /// <summary>
        /// 运行图片
        /// </summary>
        /// <param name="InputImage">输入图片</param>
        public void Fuction(HImage InputImage)
        {
            //
            if (InputImage==null)
            {
                return;
            }
            HImage GrayImage;
            if (InputImage.CountChannels().I!=1)
            {
                GrayImage = InputImage.Rgb1ToGray();
            }
            else
            {
                GrayImage = InputImage;
            }
            //生成图片的灰度直方图
            AbsoluteHisto = GrayImage.GrayHisto(GrayImage, out RelativeHisto); 
            //Y轴
            listy = new List<double>(AbsoluteHisto.ToDArr());
            //画图
            DrawSpline(listx, listy);
        }

        public void FuctionVal(int _minval,int _maxval)
        {
            if (_minval>_maxval)
            {
                throw new Exception("最小阈值比最大阈值大！");
            }
            if (MinVal!=_minval|| MaxVal != _maxval)
            {
                MinVal = _minval;
                MaxVal = _maxval;
            }
            MinValline.IntervalOffset = MinVal;
            MaxValline.IntervalOffset = MaxVal;
        }
        private void IniDraw()
        {
            //点颜色
            chart1.Series[0].MarkerColor = Color.Green;
            //图表类型  设置为样条图曲线
            chart1.Series[0].ChartType = SeriesChartType.Spline;
            //设置点的大小
            chart1.Series[0].MarkerSize = 1;
            //设置曲线的颜色
            chart1.Series[0].Color = Color.Orange;
            //设置曲线宽度
            chart1.Series[0].BorderWidth = 2;
            //chart1.Series[0].CustomProperties = "PointWidth=4";
            //设置是否显示坐标标注
            chart1.Series[0].IsValueShownAsLabel = false;

            //设置游标
            chart1.ChartAreas[0].CursorX.IsUserEnabled = false;
            chart1.ChartAreas[0].CursorX.AutoScroll = false;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = false;
            //设置X轴是否可以缩放
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = false;

            //滚动条
            //将滚动条放到图表外
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
            // 设置滚动条的大小
            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 20;
            // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.SkyBlue;
            // 设置自动放大与缩小的最小量
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
            //设置刻度间隔
            chart1.ChartAreas[0].AxisX.Interval = 0;
            //将X轴上格网取消
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            //将Y轴上格网取消
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            //Y轴上的字符不显示
            chart1.ChartAreas[0].Axes[1].LabelStyle.Enabled = false;
            //Y轴1上的刻度不显示
            chart1.ChartAreas[0].Axes[1].MajorTickMark.Enabled = false;

        }
        #region 绘制曲线函数

        /// <summary>
        /// 绘制曲线函数
        /// </summary>
        /// <param name="listX">X值集合</param>
        /// <param name="listY">Y值集合</param>
        /// <param name="chart">Chart控件</param>
         void DrawSpline(List<int> listX, List<double> listY)
        {
            try
            {
                //X、Y值成员
                chart1.Series[0].Points.DataBindXY(listX, listY);
                chart1.Series[0].Points.DataBindY(listY);
                chart1.Series[0].IsVisibleInLegend = true;
                //X轴、Y轴标题
                //chart.ChartAreas[0].AxisX.Title = "环号";
                //chart.ChartAreas[0].AxisY.Title = "直径";
                //设置Y轴范围  可以根据实际情况重新修改
                double max = listY[0];
                double min = listY[0];
           
                foreach (var yValue in listY)
                {
                    if (max < yValue)
                    {
                        max = yValue;
                    }
                    if (min > yValue)
                    {
                        min = yValue;
                    }
                }
                chart1.ChartAreas[0].AxisY.Maximum = max;
                chart1.ChartAreas[0].AxisY.Minimum = min;
                chart1.ChartAreas[0].AxisY.Interval = (max - min) / 10;
                //绑定数据源
                chart1.DataBind();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var area = chart1.ChartAreas[0];
            double xValue;
            //获取当前值
            try
            {
                xValue = area.AxisX.PixelPositionToValue(e.X);
            }
            catch
            {
                return;
            }
            try
            {
                //如果鼠标没有被按下，当前鼠标移动到最大值和最小值附近时，鼠标变化，且线变粗
                if (!IsMouseDown)
                {
                 
                    //离最小值的距离和最大值的距离
                    double dist1 = Math.Abs(xValue - MinVal);
                    double dist2 = Math.Abs(xValue - MaxVal);

                    //离最大值最近且距离小于5
                    if (dist1 >= dist2 && dist2 < 5)
                    {
                        this.Cursor = Cursors.SizeWE;
                        MaxValline.StripWidth = 2;
                        IsMaxValSelected = true;
               
                    }
                    //离最小值最近且距离小于5
                    else if (dist2 > dist1 && dist1 < 5)
                    {
                        this.Cursor = Cursors.SizeWE;
                        MinValline.StripWidth = 2;
                        IsMinValSelected = true;
                    }
                    else
                    {
               
                        this.Cursor = Cursors.Default;
                        MaxValline.StripWidth = 1;
                        MinValline.StripWidth = 1;
                        IsMinValSelected = false;
                        IsMaxValSelected = false;
                    }
                }
                //如果鼠标被按下
                else
                {
                    Graphics Grap = chart1.CreateGraphics();
                    SolidBrush brush = new SolidBrush(Color.Blue);
                 
                    //如果是最小值被选中
                    if (IsMinValSelected)
                    {
                        IsMinValS = true;
                        int curval = (int)xValue;
                        if ( curval < 0) 
                        {
                            curval = 0;                  
                        }//如果值超过255或者小于0，返回  
                        if (curval > 255 )
                        {
                            curval = 254;
                        }
                        MinVal = curval;
                        //如果最小值大于最大值了，则更新最大值
                        if (MinVal > MaxVal)
                        {
                            MinVal = MaxVal - 1;
                            MaxValline.IntervalOffset = MinVal;
                        }
                        MinValline.StripWidth = 2;
                        MinValline.IntervalOffset = MinVal;
                        ThresholdChanged?.Invoke(threshold_Min_Max);
                    }
                    //如果是最大值被选中
                    else if (IsMaxValSelected)
                    {
                        IsMinValS = false;
                        int curval = (int)xValue;
                        if (curval > 255 || curval < 0) 
                        {
                            MaxVal = 255;
          
                        }//如果值超过255或者小于0，返回  
                        MaxVal = curval;
                        //如果最大值小于最小值了，则更新最小值
                        if (MaxVal < MinVal)
                        {
                            MaxVal = MinVal+1;
                            MinValline.IntervalOffset = MinVal;
                        }
                        MaxValline.StripWidth = 2;
                        MaxValline.IntervalOffset = MaxVal;
                        ThresholdChanged?.Invoke(threshold_Min_Max);
                    }
                    //如果最大最小值都未被选中
                    else
                    {
                        //
                    }
                    chart1.Refresh();
                    Grap.DrawString(MinVal.ToString(), this.Font, brush, e.X, 10);
                    Grap.DrawString(MaxVal.ToString(), this.Font, brush, e.X, 30);

                    chart1.Update();
                }
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    IsMouseDown = true;
                    double xValue;
                    //获取当前值
                    var area = chart1.ChartAreas[0];
                    xValue = area.AxisX.PixelPositionToValue(e.X);
                    //离最小值的距离和最大值的距离
                    double dist1 = Math.Abs(xValue - MinVal);
                    double dist2 = Math.Abs(xValue - MaxVal);
                    if (dist1 >= dist2 && dist2 < 5)
                    {
                        this.Cursor = Cursors.SizeWE;
                        MaxValline.StripWidth = 2;
                        IsMaxValSelected = true;
                        IsMinValS = false;
                    }
                    //离最小值最近且距离小于5
                    else if (dist2 > dist1 && dist1 < 5)
                    {
                        this.Cursor = Cursors.SizeWE;
                        MinValline.StripWidth = 2;
                        IsMinValSelected = true;
                        IsMinValS = true;
                    }
                }

            }
            catch (Exception)
            {

            }
      
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
            IsMaxValChanging = false;
            IsMinValChanging = false;
            MinValline.StripWidth = 1;
            MaxValline.StripWidth = 1;
            //Graphics Grap = chart1.CreateGraphics();
            //SolidBrush brush = new SolidBrush(Color.Blue);
            //Grap.DrawString(minVal.ToString(), this.Font, brush, 110, 50);
            //this.numericUpDown1.Value = (decimal)minVal;
            //this.numericUpDown2.Value = (decimal)maxVal;
            ThresholdChanged?.Invoke(threshold_Min_Max);
            //if (ThresholdChanged != null)
            //{
            //    ThresholdChanged(threshold_Min_Max);
            //}
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart1_MouseLeave(object sender, EventArgs e)
        {

            IsMouseDown = false;
            this.Cursor = Cursors.Default;
            IsMaxValChanging = false;
            IsMinValChanging = false;
            MinValline.StripWidth = 1;
            MaxValline.StripWidth = 1;

            if (ThresholdChanged != null)
            {
                ThresholdChanged(threshold_Min_Max);
            }
        }


        private void chart1_MouseEnter(object sender, EventArgs e)
        {

  
        }

        private void chart1_Paint(object sender, PaintEventArgs e)
        {


        }

        private void chart1_MouseCaptureChanged(object sender, EventArgs e)
        {

        }
    }
}

