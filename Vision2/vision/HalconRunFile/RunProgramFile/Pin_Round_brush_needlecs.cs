using HalconDotNet;
using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.Controls;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    /// <summary>
    /// 
    /// </summary>
    public class Pin_Round_brush_needlecs : RunProgram
    {

        /// <summary>
        /// 空洞灰度
        /// </summary>
        public Threshold_Min_Max Threshold_Min_Max { get; set; } = new Threshold_Min_Max();

        /// <summary>
        /// 铜圈灰度
        /// </summary>
        public Threshold_Min_Max Threshold_Min_Max2 { get; set; } = new Threshold_Min_Max();
        /// <summary>
        /// 铜针灰度
        /// </summary>
        public Threshold_Min_Max Threshold_Min_Max3 { get; set; } = new Threshold_Min_Max();

        /// <summary>
        /// 跪角铜针灰度
        /// </summary>
        public Threshold_Min_Max Threshold_MinG { get; set; } = new Threshold_Min_Max();

        /// <summary>
        /// 黑孔赛选
        /// </summary>
        public Select_shape_Min_Max Select_Shape_Min = new Select_shape_Min_Max();

        /// <summary>
        /// 首次筛选
        /// </summary>
        public Select_shape_Min_Max select_Shape_ = new Select_shape_Min_Max();
        /// <summary>
        /// 多铜赛选
        /// </summary>
        public Select_shape_Min_Max select_Shape_arae2 = new Select_shape_Min_Max();
        public Pin_Round_brush_needlecs()
        {
            Threshold_Min_Max.Max = 60;
            Threshold_Min_Max.Min = 0;
            Threshold_Min_Max2.Max = 255;
            Threshold_Min_Max2.Min = 100;
            Threshold_Min_Max3.Min = 100;
            Threshold_Min_Max3.Max = 255;
            select_Shape_arae2.Min = 150;
            select_Shape_.Min = 0.8;
        }

        public int Number { get; set; }
        public HTuple Rows { get; set; }
        public HTuple Columns { get; set; }
        public double ECircle { get; set; } = 3;
        public double ECircleT { get; set; } = 7;

        public double ClosCircle { get; set; } = 10;
        public double FillArea { get; set; } = 9999;
        public override Control GetControl()
        {
            return new Pint_Round_brushControl(this);
        }

        public override RunProgram UpSatrt<T>(string path)
        {
            return base.ReadThis<Pin_Round_brush_needlecs>(path);
        }

        public override bool RunHProgram(HalconRun halcon, OneResultOBj oneResultOBj, int id, string name = null)
        {
            try
            {
                int errNumager = 0;
                HObject Imgetd;
                HObject hObject1;
                HObject hObject3;
                HObject image2 = this.GetEmset(halcon.Image());
                HObject hObject = Threshold_Min_Max2.Threshold(image2);//铜圈灰度
                HOperatorSet.ClosingCircle(hObject, out hObject, 5);
                if (id == 1)
                {
                    halcon.AddOBJ(hObject);
                    return false;
                }
                HOperatorSet.FillUpShape(hObject, out hObject, "area", 1, FillArea);
                HOperatorSet.Connection(hObject, out hObject);
                hObject = select_Shape_.select_shape(hObject);//铜圈赛选
                HOperatorSet.AreaCenter(hObject, out HTuple area2, out HTuple row2, out HTuple column2);
                HOperatorSet.GenCircle(out hObject, row2, column2, HTuple.TupleGenConst(row2.Length, ECircle));
                if (id >= 3)
                {
                    halcon.AddOBJ(hObject, ColorResult.blue);
                }
                HOperatorSet.GenCircle(out hObject3, row2, column2, HTuple.TupleGenConst(row2.Length, ECircleT));
                if (id >= 5)
                {
                    halcon.AddOBJ(hObject3, ColorResult.yellow);
                }
                if (id == 2)
                {
                    halcon.AddOBJ(hObject);
                    return false;
                }
                HOperatorSet.Union1(hObject, out hObject);
                //HOperatorSet.ErosionCircle(hObject, out hObject, ECircle);
                HOperatorSet.ReduceDomain(image2, hObject, out image2);
                hObject = Threshold_Min_Max.Threshold(image2);//铜孔灰度
                HOperatorSet.ClosingCircle(hObject, out hObject, ClosCircle);
                HOperatorSet.Connection(hObject, out hObject);
                if (hObject.CountObj() == 0)
                {
                    halcon.AddNGMessage("未找到区域");
                    return false;
                }
                HOperatorSet.SmallestCircle(hObject, out HTuple rows, out HTuple columns, out HTuple radius);

                HOperatorSet.GenCircle(out hObject, rows, columns, radius);
                HOperatorSet.ErosionCircle(hObject, out hObject, 6);//获得铜孔区域
                if (id == 3)
                {
                    halcon.AddOBJ(hObject);
                    return false;
                }
                HOperatorSet.Union1(hObject, out hObject1);
                HOperatorSet.ReduceDomain(image2, hObject1, out Imgetd);
                hObject1 = Threshold_Min_Max3.Threshold(Imgetd);//铜针灰度
                if (id == 4)
                {
                    halcon.AddOBJ(hObject1);
                    HOperatorSet.AreaCenter(hObject1, out HTuple area, out HTuple row, out HTuple column);
                    return false;
                }
                HOperatorSet.DilationCircle(hObject1, out hObject1, 20);
                HOperatorSet.Difference(hObject, hObject1, out HObject hObject2);
                HOperatorSet.Connection(hObject2, out hObject2);
                hObject = Select_Shape_Min.select_shape(hObject2);
                if (hObject.CountObj() > 0)
                {
                    halcon.AddNGMessage("空洞:" + hObject.CountObj());
                    errNumager++;
                    HOperatorSet.DilationRectangle1(hObject, out HObject hObject4, 60,60);
                    halcon.AddOBJ(hObject4, ColorResult.red);
                }
                if (id == 5)
                {
                    halcon.AddOBJ(hObject);
                    return false;
                }
                HOperatorSet.Union1(hObject3, out hObject3);
                HOperatorSet.ReduceDomain(image2, hObject3, out image2);
                hObject1 = Threshold_MinG.Threshold(image2);//跪角灰度
                HOperatorSet.Connection(hObject1, out hObject1);

                if (id == 6)
                {
                    halcon.AddOBJ(hObject1);
                    return false;
                }
                hObject = select_Shape_arae2.select_shape(hObject1);
                if (id == 7)
                {
                    halcon.AddOBJ(hObject);
                    return false;
                }
                if (hObject.CountObj() > 0)
                {
                    errNumager++;
                }
                HOperatorSet.FillUp(hObject, out hObject);
                HOperatorSet.Union1(hObject, out hObject);
                if (id ==8)
                {
                    halcon.AddOBJ(hObject);
                    return false;
                }
                halcon.AddMessage("孔数:"+row2.Length);
                if (id == 9)
                {
                    Rows = row2;
                    Columns = column2;
                    Number = row2.Length;
                    HOperatorSet.GenCrossContourXld(out HObject hObject4, Rows, Columns, 40, 0);
                    halcon.AddOBJ(hObject4);
                    return false;
                }
                if (Number>=1)
                {
                    if (Number != row2.Length)
                    {
                        halcon.AddNGMessage("针角数量:" + Number + "/" + row2.Length);
                        errNumager++;
                    }
                }
                if (errNumager == 0)
                {
                    return true;
                }
                HOperatorSet.Connection(hObject,out  hObject);
                halcon.AddNGMessage("跪角:" + hObject.CountObj());
                HOperatorSet.DilationCircle(hObject, out hObject, 60);
                XLD = XLD.ConcatObj(hObject);
                return false;
            }
            catch (Exception ex)
            {
            }
            halcon.AddNGMessage("未找到区域");
            return false;
        }
    }
}
