using HalconDotNet;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision
{
    /// <summary>
    /// 坐标转换
    /// </summary>
    public class Coordinate : ProjectNodet.IClickNodeProject
    {

        public string Name { get; set; }
        public class CpointXY
        {
            public double X { get; set; } = 0;
            public double Y { get; set; } = 0;
            public double Z { get; set; } = 0;
        }

        public class Coordinate2D
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
            public double U { get; set; }
            public double S { get; set; }
        }

        public class Coordinate3D
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
            public double U { get; set; }
            public double S { get; set; }
            public double V { get; set; }
            public double W { get; set; }
        }

        /// <summary>
        /// 坐标类型
        /// </summary>
        public enum Coordinate_Type
        {
            Hide = 0,
            PixelRC = 1,
            XYU2D = 2,
            XYZU3D = 3,
        }

        public Coordinate()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public Coordinate_Type CoordinateTeyp { get; set; } = Coordinate_Type.Hide;

        [Description("2D坐标X"), Category("2d坐标"), DisplayName("原点坐标X")]
        public double PointX { get; set; }

        [Description("2D坐标Y"), Category("2d坐标"), DisplayName("原点坐标Y")]
        public double PointY { get; set; }

        [Description("2D叠加坐标X"), Category("2d坐标"), DisplayName("原点坐标X偏移")]
        public double PointAddX { get; set; }

        [Description("2D叠加坐标Y"), Category("2d坐标"), DisplayName("原点坐标Y偏移")]
        public double PointAddY { get; set; }

        [Description("2D叠加坐标U"), Category("2d坐标"), DisplayName("原点坐标U偏移")]
        public double PointAddU { get; set; }

        [Description("坐标"), Category("2d坐标"), DisplayName("原点角度")]
        /// <summary>
        /// 角度
        /// </summary>
        public double PointU { get; set; } = 0;

        [Description("斜切角度"), Category("2d坐标"), DisplayName("X斜切角")]
        public double SX { get; set; } = 0;

        [Description("斜切角度"), Category("2d坐标"), DisplayName("Y斜切角")]
        public double SY { get; set; } = 0;

        [Description("坐标转换系数"), Category("2d坐标"), DisplayName("转换系数")]
        /// <summary>
        /// 缩放
        /// </summary>
        public double Scale { get; set; } = 1;

        [Description("坐标彷射转换"), Category("2d坐标"), DisplayName("坐标彷射转换")]
        /// <summary>
        /// 彷射
        /// </summary>
        public HTuple CoordHanMat2DXY
        {
            get { return coordHanMat2DXY; }
            set { coordHanMat2DXY = value; }
        }
        private HTuple coordHanMat2DXY = new HTuple();


        [Description("图像彷射转换"), Category("2d坐标"), DisplayName("图像彷射转换")]
        public HTuple CoordHanMat2d { get; set; }

        [Description("坐标"), Category("图像坐标"), DisplayName("原点角度")]
        /// <summary>
        /// 角度
        /// </summary>
        public double RCU { get; set; } = 0;

        [Description("斜切角度"), Category("图像坐标"), DisplayName("X斜切角")]
        public double RCSX { get; set; } = 0;

        [Description("斜切角度"), Category("图像坐标"), DisplayName("Y斜切角")]
        public double RCSY { get; set; } = 0;

        [Description("原点X"), Category("原点"), DisplayName("原点X")]
        public double aSX { get; set; } = 0;

        [Description("原点Y"), Category("原点"), DisplayName("原点Y")]
        public double aSY { get; set; } = 0;
        [Description("坐标转换系数"), Category("图像坐标"), DisplayName("转换系数")]
        /// <summary>
        /// 缩放
        /// </summary>
        public double RCScale { get; set; } = 1;

        [Description("图像坐标rol"), Category("图像坐标"), DisplayName("原点坐标Row")]
        public double Row { get; set; }

        [Description("图像坐标col"), Category("图像坐标"), DisplayName("原点坐标Col")]
        public double Col { get; set; }

        [Description("像素坐标Ros"), Category("坐标转换"), DisplayName("图像彷射点Rows")]
        public HTuple Rows { get; set; }
        [Description("像素坐标Cols"), Category("坐标转换"), DisplayName("图像彷射点Cols")]
        public HTuple Columns { get; set; }
        [Description("机械坐标Xs"), Category("坐标转换"), DisplayName("机械坐标点Xs")]
        public HTuple Xs { get; set; }
        [Description("机械坐标Ys"), Category("坐标转换"), DisplayName("机械坐标点Ys")]
        public HTuple Ys { get; set; }
        /// <summary>
        /// 绘制坐标原点
        /// </summary>
        /// <param name="halcon"></param>
        public HObject DrawPoint(HalconRun halcon)
        {
            try
            {
                //halcon.DrawIng(dsdf, out HObject hObject);
                //HObject dsdf(HalconRun halcson)
                //{
                //    HOperatorSet.DrawPoint(halcson.hWindowHalcon(), out HTuple row, out HTuple column);
                //    PointX = row;
                //    PointY = column;
                //    return null;
                //}
                HOperatorSet.DrawPoint(halcon.hWindowHalcon(), out HTuple row, out HTuple column);
                PointX = row;
                PointY = column;
                halcon.CoordinatePXY = this;

                halcon.ShowImage();
                ShowCoordinate(halcon);
                return null;
            }
            catch (Exception ex)
            {
                halcon.ErrLog(ex);
            }
            return null;
        }

        /// <summary>
        /// 更新原点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="u"></param>
        /// <param name="s"></param>
        public void SetOrigin(double x, double y, double u = 0, double s = 0)
        {
            this.PointX = x;
            this.PointY = y;
            this.PointU = u;
            if (s == 0)
            {
                s = 1;
            }
            this.Scale = s;
        }
        /// <summary>
        /// 计算转换
        /// </summary>
        public void VectorToHomMat2d(HTuple Rowst, HTuple Colst, HTuple Yst, HTuple Xst)
        {
            Rows = Rowst;
            Columns = Colst;
            Ys = Yst;
            Xs = Xst;
            HOperatorSet.VectorToHomMat2d(this.Rows, this.Columns, this.Ys, this.Xs, out HTuple HomMat);
            CoordHanMat2DXY = HomMat;


            Mat2dPar();
        }

        /// <summary>
        /// 计算空间数据
        /// </summary>
        public void Mat2dPar()
        {
            HOperatorSet.HomMat2dToAffinePar(this.CoordHanMat2DXY, out HTuple sx, out HTuple sy,
                out HTuple phi, out HTuple theat, out HTuple tx, out HTuple ty);
            this.PointY = ty;
            this.PointX = tx;
            this.SX = sx;
            this.SY = sy;
            this.PointU = phi.TupleDeg();
            this.Scale = theat;
            HOperatorSet.HomMat2dInvert(this.CoordHanMat2DXY, out HTuple coordHanMat2d);
            CoordHanMat2d = coordHanMat2d;
            HOperatorSet.HomMat2dToAffinePar(this.CoordHanMat2d, out sx, out sy,
             out phi, out theat, out tx, out ty);
            this.Row = ty;
            this.Col = tx;
            this.RCSX = SX;
            this.RCSY = SY;
            this.RCU = phi.TupleDeg();
            this.RCScale = theat;
        }
        public string GetMatHomString()
        {
            HOperatorSet.HomMat2dToAffinePar(this.CoordHanMat2DXY, out HTuple sx, out HTuple sy,
                     out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);
            //HOperatorSet.AffineTransPixel(_Coordinate.CoordHanMat2d, 0, 0, out HTuple axisX, out HTuple axisY);
            return string.Format("坐标斜切X:{0}斜切Y:{1}角度旋转:{2}斜度:{3}偏移y:{4}偏移x:{5}"
                , sx.TupleString("0.02f"), sy.TupleString("0.02f"), phi.TupleDeg().TupleString("0.02f"), theta.TupleDeg().TupleString("0.02f"), tx.TupleString("0.02f"), ty.TupleString("0.02f"));
        }
        public string GetRCMatHomString()
        {
            HOperatorSet.HomMat2dToAffinePar(this.CoordHanMat2d, out HTuple sx, out HTuple sy,
                     out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);
            //HOperatorSet.AffineTransPixel(_Coordinate.CoordHanMat2d, 0, 0, out HTuple axisX, out HTuple axisY);
            return string.Format("图像斜切X:{0}斜切Y:{1}角度旋转:{2}斜度:{3}偏移y:{4}偏移x:{5}"
                , sx.TupleString("0.02f"), sy.TupleString("0.02f"), phi.TupleDeg().TupleString("0.02f"), theta.TupleDeg().TupleString("0.02f"), tx.TupleString("0.02f"), ty.TupleString("0.02f"));
        }

        /// <summary>
        /// 修改叠加偏移的坐标点位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="u"></param>
        public void AddOrigin(double x, double y, double u)
        {
            this.PointAddX = x;
            this.PointAddY = y;
            this.PointAddU = u;
            //UpOrigin();
        }

        /// <summary>
        /// 更新原点
        /// </summary>
        public void UpOrigin()
        {
            HOperatorSet.HomMat2dIdentity(out HTuple coordHanMat2DXY);
            HTuple hTupleR = new HTuple(this.PointU + this.PointAddU).TupleRad();
            HOperatorSet.HomMat2dRotate(coordHanMat2DXY, hTupleR, this.PointX + this.PointAddX, this.PointY + this.PointAddY, out coordHanMat2DXY);
            HOperatorSet.HomMat2dTranslate(coordHanMat2DXY, this.PointY + this.PointAddY, this.PointX + this.PointAddX, out coordHanMat2DXY);
            //this.CoordHanMat2DXY = coordHanMat2DXY;
        }

        private HTuple hTuplX = new HTuple(-10);
        private HTuple hTuply = new HTuple(210);
        private HTuple hTupl2X = new HTuple(210);
        private HTuple hTupl2Y = new HTuple(5);
        private HTuple hTupleR = new HTuple(0);

        /// <summary>
        /// 显示坐标系
        /// </summary>
        /// <param name="halcon"></param>
        public void ShowCoordinate(HalconRun halcon)
        {
            HObject hObjectY = new HObject();
            HObject hObjectX = new HObject();
            try
            {
                //this.UpOrigin();
                if (this.CoordHanMat2DXY.Length != 6)
                {
                    HOperatorSet.HomMat2dIdentity(out coordHanMat2DXY);
                    return;
                }
                if (CoordHanMat2d == null)
                {
                    HOperatorSet.HomMat2dIdentity(out HTuple hTuple);
                    CoordHanMat2d = hTuple;
                    return;
                }
                switch (CoordinateTeyp)
                {
                    case Coordinate.Coordinate_Type.PixelRC:
                        hObjectX = HalconRun.GenArrowContourXld(0, -150, 0, 0 + 200, 20, 20);
                        hObjectY = HalconRun.GenArrowContourXld(-150, 0, 0 + 200, 0, 20, 20);
                        hTuplX = 5;
                        hTupl2X = 210;
                        hTupl2Y = -10;
                        hTuply = 211;
                        break;

                    case Coordinate.Coordinate_Type.XYU2D:
                        hObjectX = HalconRun.GenArrowContourXld(0, -150, 0, 0 + 200, 20, 20);
                        hObjectY = HalconRun.GenArrowContourXld(-150, 0, 0 + 200, 0, 20, 20);
                        Coordinate.CpointXY xy = GetPointXYtoRC(0, 0);
                        //HOperatorSet.HomMat2dSlant(CoordHanMat2d, new HTuple(180).TupleRad(), "x", xy.Y, xy.X, out HTuple coXY);
                        //HOperatorSet.HomMat2dSlant(coXY, new HTuple(180).TupleRad(), "y", xy.Y, xy.X, out coXY);

                        HOperatorSet.AffineTransContourXld(hObjectY, out hObjectY, CoordHanMat2d);
                        HOperatorSet.AffineTransContourXld(hObjectX, out hObjectX, CoordHanMat2d);

                        //HOperatorSet.AffineTransContourXld(hObjectY, out HObject hObject2Y, CoordHanMat2DXY);
                        //HOperatorSet.AffineTransContourXld(hObjectX, out HObject hObject2X, CoordHanMat2DXY);
                        //HOperatorSet.SetColor(halcon.hWindowHalcon(), "yellow");
                        //HOperatorSet.DispObj(hObject2Y, halcon.hWindowHalcon());
                        //HOperatorSet.SetColor(halcon.hWindowHalcon(), "blue");
                        //HOperatorSet.DispObj(hObject2X, halcon.hWindowHalcon());
                        HOperatorSet.AffineTransPixel(CoordHanMat2d, 5, 210, out hTuplX, out hTuply);
                        HOperatorSet.AffineTransPixel(CoordHanMat2d, 210, -10, out hTupl2X, out hTupl2Y);
                        break;

                    case Coordinate.Coordinate_Type.XYZU3D:
                        break;

                    default:
                        return;
                }
                HOperatorSet.SetColor(halcon.hWindowHalcon(), "red");
                HOperatorSet.DispObj(hObjectY, halcon.hWindowHalcon());
                HOperatorSet.SetColor(halcon.hWindowHalcon(), "green");
                HOperatorSet.DispObj(hObjectX, halcon.hWindowHalcon());
                Vision.Disp_message(halcon.hWindowHalcon(), "X+", hTuplX, hTuply, false, "green", "false");
                Vision.Disp_message(halcon.hWindowHalcon(), "Y+", hTupl2X, hTupl2Y);
            }
            catch (Exception ex)
            {
                halcon.ErrLog(ex);
            }
        }
        /// <summary>
        /// 显示区域
        /// </summary>
        /// <param name="hWindID"></param>
        public void ShowCoordinate(HWindID hWindID)
        {
            HObject hObjectY = new HObject();
            HObject hObjectX = new HObject();
            try
            {
                //this.UpOrigin();
                if (this.CoordHanMat2DXY.Length != 6)
                {
                    HOperatorSet.HomMat2dIdentity(out coordHanMat2DXY);
                    return;
                }
                switch (CoordinateTeyp)
                {
                    case Coordinate.Coordinate_Type.PixelRC:
                        hObjectX = HalconRun.GenArrowContourXld(0, -150, 0, 0 + 200, 20, 20);
                        hObjectY = HalconRun.GenArrowContourXld(-150, 0, 0 + 200, 0, 20, 20);
                        hTuplX = 5;
                        hTupl2X = 210;
                        hTupl2Y = -10;
                        hTuply = 211;
                        break;

                    case Coordinate.Coordinate_Type.XYU2D:
                        hObjectX = HalconRun.GenArrowContourXld(0, -150, 0, 0 + 200, 20, 20);
                        hObjectY = HalconRun.GenArrowContourXld(-150, 0, 0 + 200, 0, 20, 20);
                        Coordinate.CpointXY xy = GetPointXYtoRC(0, 0);
                        //HOperatorSet.HomMat2dSlant(CoordHanMat2d, new HTuple(180).TupleRad(), "x", xy.Y, xy.X, out HTuple coXY);
                        //HOperatorSet.HomMat2dSlant(coXY, new HTuple(180).TupleRad(), "y", xy.Y, xy.X, out coXY);

                        HOperatorSet.AffineTransContourXld(hObjectY, out hObjectY, CoordHanMat2d);
                        HOperatorSet.AffineTransContourXld(hObjectX, out hObjectX, CoordHanMat2d);

                        //HOperatorSet.AffineTransContourXld(hObjectY, out HObject hObject2Y, CoordHanMat2DXY);
                        //HOperatorSet.AffineTransContourXld(hObjectX, out HObject hObject2X, CoordHanMat2DXY);
                        //HOperatorSet.SetColor(halcon.hWindowHalcon(), "yellow");
                        //HOperatorSet.DispObj(hObject2Y, halcon.hWindowHalcon());
                        //HOperatorSet.SetColor(halcon.hWindowHalcon(), "blue");
                        //HOperatorSet.DispObj(hObject2X, halcon.hWindowHalcon());
                        HOperatorSet.AffineTransPixel(CoordHanMat2d, 5, 210, out hTuplX, out hTuply);
                        HOperatorSet.AffineTransPixel(CoordHanMat2d, 210, -10, out hTupl2X, out hTupl2Y);
                        break;

                    case Coordinate.Coordinate_Type.XYZU3D:
                        break;

                    default:
                        return;
                }
                hWindID.HalconResult.AddObj(hObjectY, RunProgram.ColorResult.red);
                hWindID.HalconResult.AddObj(hObjectX, RunProgram.ColorResult.green);
                hWindID.HalconResult.AddImageMassage(hTuplX, hTuply, "X+", RunProgram.ColorResult.green);
                hWindID.HalconResult.AddImageMassage(hTupl2X, hTupl2Y, "Y+", RunProgram.ColorResult.red);
                //HOperatorSet.SetColor(halcon.hWindowHalcon(), "red");
                //HOperatorSet.DispObj(hObjectY, halcon.hWindowHalcon());
                //HOperatorSet.SetColor(halcon.hWindowHalcon(), "green");
                //HOperatorSet.DispObj(hObjectX, halcon.hWindowHalcon());
                //Vision.Disp_message(halcon.hWindowHalcon(), "X+", hTuplX, hTuply, false, "green", "false");
                //Vision.Disp_message(halcon.hWindowHalcon(), "Y+", hTupl2X, hTupl2Y);
            }
            catch (Exception ex)
            {
                //halcon.ErrLog(ex);
            }
        }

        /// <summary>
        /// 根据坐标YX返回图像坐标RC位置
        /// </summary>
        /// <param name="Y"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        public Coordinate.CpointXY GetPointXYtoRC(double Y, double X)
        {
            Coordinate.CpointXY c2D = new Coordinate.CpointXY();
            try
            {
                HOperatorSet.AffineTransPixel(this.CoordHanMat2d, Y, X, out HTuple qx, out HTuple qy);
                //HOperatorSet.AffineTransPoint2d(this.coordHanMat2d, Y, X, out HTuple qx, out HTuple qy);
                c2D.Y = qy;
                c2D.X = qx;
            }
            catch (Exception)
            {
            }
            return c2D;
        }
        public void GetPointXYtoRC(double Y, double X, out HTuple rows, out HTuple cols)
        {
            rows = new HTuple();
            cols = new HTuple();
            try
            {
                HOperatorSet.AffineTransPixel(this.CoordHanMat2d, Y, X, out rows, out cols);
                //HOperatorSet.AffineTransPoint2d(this.coordHanMat2d, Y, X, out HTuple qx, out HTuple qy);

            }
            catch (Exception)
            {
            }
        }

        public HTuple GetContrs(HTuple value)
        {

            try
            {
                HOperatorSet.AffineTransPixel(this.CoordHanMat2d, value, HTuple.TupleGenConst(value.Length, 0), out HTuple rows, out HTuple cols);
                HOperatorSet.DistancePp(HTuple.TupleGenConst(rows.Length, 0), HTuple.TupleGenConst(rows.Length, 0), cols, rows, out HTuple distance);
                return distance;
            }
            catch (Exception)
            {

            }
            return new HTuple();
        }

        public HTuple SetCet(HTuple values)
        {
            values.TupleDiv(Scale);
            return values.TupleMult(Scale);


        }

        /// <summary>
        /// 根据图像坐标RC位置
        /// </summary>
        /// <param name="row"></param>
        /// <param name="colmun"></param>
        /// <returns></returns>
        public CpointXY GetPointRctoYX(double row, double column)
        {
            CpointXY c2D = new CpointXY();
            try
            {
                HOperatorSet.AffineTransPoint2d(CoordHanMat2DXY, row, column, out HTuple qy, out HTuple qx);
                c2D.Y = qy;
                c2D.X = qx;
            }
            catch (Exception)
            {
            }
            return c2D;
        }

        public void GetPointRctoXY(HTuple rows, HTuple columns, out HTuple XS, out HTuple YS)
        {
            XS = new HTuple();
            YS = new HTuple();
            try
            {
                HOperatorSet.AffineTransPoint2d(CoordHanMat2DXY, rows, columns, out XS, out YS);
            }
            catch (Exception)
            {
            }
        }


        public Control GetThisControl()
        {
            return new Calib.CoordinateControl(this);
        }
    }
}