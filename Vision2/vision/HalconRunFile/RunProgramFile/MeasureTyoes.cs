using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using static Vision2.vision.HalconRunFile.RunProgramFile.RunProgram;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
   public class MeasureTyoes
    {

        public enum MeasureEnum
        {
            空=0,
            点于线垂足=1,
            测长=3,
            线平行=4,
            夹角=5,
            点距离=6,
            点与点距离=7,
            圆直径=8,
            同心圆 = 2,

        }
        public class MeasureClass
        {
            public string Name { get; set; }

            [DescriptionAttribute("多个测量的对象关系"), Category("测量关系"), DisplayName("测量模式")]
            [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("ListMeasureMode")]
            public string MeasureMode { get; set; } = "";

            public List<string> ListMeasureMode
            {
                get
                {
                    return new List<string> { "点与线垂足", "同心圆", "绘制区域", "胶宽测量", "测长", "线平行" };
                }
            }

            [DescriptionAttribute("测量点固定补偿。"), Category("测量关系"), DisplayName("测量点补偿")]
            public double AddOreM { get; set; } = 0;
            /// <summary>
            /// 测量1目标名称
            /// </summary>
            [DescriptionAttribute("测量点对象名称1。"), Category("测量关系"), DisplayName("测量点名称1")]
            public string MeasureName1 { get; set; } = "";
            /// <summary>
            /// 测量2目标名称
            /// </summary>
            [DescriptionAttribute("测量点对象名称2。"), Category("测量关系"), DisplayName("测量点名称2")]
            public string MeasureName2 { get; set; } = "";


            public string SelePointName { get; set; } = "全部";

            /// <summary>
            /// 点与线之间的垂足
            /// 
            /// </summary>
            /// <param name="halcon">程序</param>
            /// <param name="OutRows">直线Rows</param>
            /// <param name="OutCols">直线Cols</param>
            /// <param name="point">点</param>
            /// <param name="name2">直线名称</param>
            /// <returns></returns>
            public bool PointToLineExtension(HalconRun halcon, HTuple OutRows, HTuple OutCols, HTuple point)
            {
                try
                {
                    HTuple hTupleRow = new HTuple();
                    HTuple hTupleCol = new HTuple();
                    hTupleRow.Append(OutRows);
                    //hTupleRow.Append(row);
                    hTupleCol.Append(OutCols);
                    //hTupleCol.Append(col);
                    HOperatorSet.GenContourPolygonXld(out HObject hObject, hTupleRow, hTupleCol);
                    double rowt = point[0];
                    double colt = point[1];
                    if (hTupleRow[0].D == 0)
                    {
                        halcon.SendMesage("Goto", "0.0");
                        return false;
                    }
                    HOperatorSet.ProjectionPl(rowt, colt,
                        OutRows[0], OutCols[0], OutRows[1],
                        OutCols[1], out HTuple outRows, out HTuple outcols);
                    HOperatorSet.AngleLx(outRows, outcols, rowt, colt, out HTuple angle);
                    HOperatorSet.GenCrossContourXld(out HObject xldd, rowt, colt, 50, 0);
                    HOperatorSet.DistancePp(outRows, outcols, rowt, colt, out HTuple min);
                    if (angle.D < 0)
                    {
                        min = -min;
                    }
                    min = halcon.GetCaliConstMM(min);
                    //halcon.AddOBJ(  xld,  this.color );
                    halcon.GetOneImageR().AddImageMassage(outRows.TupleInt(), outcols.TupleInt(), this.Name + "=" + min.TupleString("0.3f"), ColorResult.green);
                    outRows.Append(rowt);
                    outcols.Append(colt);
                    HOperatorSet.GenContourPolygonXld(out HObject hObject2, outRows, outcols);
                    //halcon.SendMesage("Goto", min.TupleString("0.3f"));
                    halcon.AddOBJ(xldd.ConcatObj(hObject.ConcatObj(hObject2)));
                    hObject2.Dispose();
                    hObject.Dispose();
                    return true;
                }
                catch (Exception)
                {
                }
                //halcon.SendMesage("Goto", "0.0");
                return false;
            }
            /// <summary>
            /// 线平行测量
            /// </summary>
            public bool LineAngelLine(HalconRun halcon,
                HTuple dinRow1, HTuple dinCol1, HTuple dinRow2, HTuple dinCol2,
                HTuple dinRow21, HTuple dinCol21, HTuple dinRow22, HTuple dinCol22,
                out HTuple distMM, out HTuple distMM2, out HTuple distMM3, out HTuple angle, out HObject corss)
            {
                angle = new HTuple();
                distMM = new HTuple();
                distMM2 = new HTuple();
                distMM3 = new HTuple();
                corss = new HObject();
                try
                {
                    HOperatorSet.LinePosition(dinRow1, dinCol1, dinRow2, dinCol2, out HTuple dinRow3, out HTuple dinCol3, out HTuple length, out HTuple phi);
                    HOperatorSet.AngleLl(dinRow1, dinCol1, dinRow2, dinCol2, dinRow21, dinCol21, dinRow22, dinCol22, out angle);
                    HOperatorSet.ProjectionPl(dinRow1, dinCol1, dinRow21, dinCol21, dinRow22, dinCol22, out HTuple projectRow, out HTuple projectCol0);
                    HOperatorSet.DistancePp(projectRow, projectCol0, dinRow1, dinCol1, out HTuple dist);
                    Vision.Gen_arrow_contour_xld(out HObject contour1, dinRow1, dinCol1, projectRow, projectCol0, 200, 40);
                    HOperatorSet.ProjectionPl(dinRow2, dinCol2, dinRow21, dinCol21, dinRow22, dinCol22, out HTuple projectRow2, out HTuple projectCol02);
                    HOperatorSet.DistancePp(projectRow2, projectCol02, dinRow2, dinCol2, out HTuple dist2);
                    Vision.Gen_arrow_contour_xld(out HObject contour3, dinRow2, dinCol2, projectRow2, projectCol02, 200, 40);
                    HOperatorSet.ProjectionPl(dinRow3, dinCol3, dinRow21, dinCol21, dinRow22, dinCol22, out HTuple projectRow3, out HTuple projectCol03);
                    HOperatorSet.DistancePp(projectRow3, projectCol03, dinRow3, dinCol3, out HTuple dist3);
                    Vision.Gen_arrow_contour_xld(out HObject contour4, dinRow3, dinCol3, projectRow3, projectCol03, 200, 40);
                    HTuple angleD = angle.TupleDeg();
                    distMM = halcon.GetCaliConstMM(dist);
                    distMM2 = halcon.GetCaliConstMM(dist2);
                    distMM3 = halcon.GetCaliConstMM(dist3);
                    HOperatorSet.GenCrossContourXld(out corss, new HTuple(projectRow2, dinRow2), new HTuple(projectCol02, dinCol2), 50, 0);
                    halcon.AddTData(distMM.D, distMM2.D, distMM3.D);

                }
                catch (Exception)
                {
                }
                //bool OK = true;
                //if (SelePointName == "结束点" || SelePointName == "全部")
                //{
                //    

                //    if (angleD > AngleMax || angleD < AngleMin || DistM2 < DistanceMin || DistM2 > DistanceMax)
                //    {
                //        halcon.AddMessageIamge(projectRow2, projectCol02, "夹角" + angleD.TupleString("0.2f") + "°距离" + DistM2.TupleString("0.4f"), ColorResult.red);
                //        OK = false;
                //        halcon.AddOBJ(contour3, ColorResult.red);
                //    }
                //    else
                //    {
                //        halcon.AddMessageIamge(projectRow2 - 60, projectCol02, "夹角" + angleD.TupleString("0.2f") + "°距离" + DistM2.TupleString("0.4f"));
                //        halcon.AddOBJ(contour3);
                //    }
                //}
                //if (SelePointName == "第一点" || SelePointName == "全部")
                //{

                //    HOperatorSet.GenCrossContourXld(out corss, new HTuple(projectRow, dinRow1), new HTuple(projectCol0, dinCol1), 50, 0);

                //    if (angleD > AngleMax || angleD < AngleMin || DistM < DistanceMin || DistM > DistanceMax)
                //    {
                //        halcon.AddMessageIamge(projectRow, projectCol0, "夹角" + angleD.TupleString("0.2f") + "°距离" + DistM.TupleString("0.4f"), ColorResult.red);
                //        OK = false;
                //        halcon.AddOBJ(contour1, ColorResult.red);
                //    }
                //    else
                //    {
                //        halcon.AddMessageIamge(projectRow - 60, projectCol0, "夹角" + angleD.TupleString("0.2f") + "°距离" + DistM.TupleString("0.4f"));
                //        halcon.AddOBJ(contour1, ColorResult.yellow);
                //    }
                //}
                //if (SelePointName == "中心点" || SelePointName == "全部")
                //{
                //    HOperatorSet.GenCrossContourXld(out corss, new HTuple(projectRow3, dinRow3), new HTuple(projectCol03, dinCol3), 50, 0);
                //    if (angleD > AngleMax || angleD < AngleMin || DistM3 < DistanceMin || DistM3 > DistanceMax)
                //    {
                //        halcon.AddMessageIamge(projectRow3, projectCol03, "夹角" + angleD.TupleString("0.2f") + "°距离" + DistM3.TupleString("0.4f"), ColorResult.red);
                //        OK = false;
                //        halcon.AddOBJ(contour4, ColorResult.red);
                //    }
                //    else
                //    {
                //        halcon.AddOBJ(contour4, ColorResult.yellow);
                //        halcon.AddMessageIamge(projectRow3 - 60, projectCol03, "夹角" + angleD.TupleString("0.2f") + "°距离" + DistM3.TupleString("0.4f"));
                //    }
                //}
                //halcon.AddOBJ(corss, ColorResult.blue);
                //if (OK)
                //{
                //    return true;
                //}
                return false;
            }

        }

    }
}
