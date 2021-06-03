﻿using HalconDotNet;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using static Vision2.vision.HalconRunFile.RunProgramFile.HalconRun;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    /// <summary>
    /// 测量参数类
    /// </summary>
    public class MeasureMlet : RunProgram
    {
        public override string ShowHelpText()
        {
            return "2.7_测量";
        }
        public override bool RunHProgram(HalconRun halcon, OneResultOBj oneResultOBj, int id, string name = null)
        {
            bool OK = true;
            HObject image;
            //this.SetDefault("NG距离", 100, true);
            image = GetEmset(halcon.Image());
            foreach (var item in Dic_Measure.Keys_Measure)
            {
                item.Value.ResltBool = false;
                if (item.Value.ISMatHat)
                {
                    if (halcon.GetHomdeMobelEx(this.HomName) != null)
                    {
                        for (int i = 0; i < halcon.GetHomdeMobelEx(this.HomName).HomMat.Count; i++)
                        {
                            ObjectColor objectColor = item.Value.MeasureObj(image, halcon,   halcon.GetHomdeMobelEx(this.HomName).HomMat[i], oneResultOBj);
                            if (objectColor._HObject.CountObj() > 0)
                            {
                                item.Value.ResltBool = true;
                                NGRoi = NGRoi.ConcatObj(objectColor._HObject);
                            }
                            else
                            {
                                item.Value.ResltBool = false;
                            }
                        }
                    }
                }
                else
                {
                    bool ist = item.Value.Run(halcon, oneResultOBj);
                    if (item.Value.IsExist("距离mm"))
                    {
                        this["距离mm"] = item.Value["距离mm"];
                        this.SetDefault("距离mm", true);
                    }
                    if (MeasureMode == MeasureModeEnum.胶宽测量 &&
                        item.Value.Measure_Type == Measure.MeasureType.NurbsMeasure)
                    {
                    }
                }
            }
            if (MeasureName1 != null)
            {
                string[] name1 = MeasureName1.Split('.');
                string[] name2 = MeasureName2.Split('.');
                Measure measure1 = null;
                Measure measure2 = null;
                if (Dic_Measure.Keys_Measure.ContainsKey(name1[0]))
                {
                    measure1 = Dic_Measure.Keys_Measure[name1[0]];
                }
                if (Dic_Measure.Keys_Measure.ContainsKey(name2[0]))
                {
                    measure2 = Dic_Measure.Keys_Measure[name2[0]];
                }
                switch (MeasureMode)
                {
                    case MeasureModeEnum.点与线垂足 :
                        HTuple lengtMM = new HTuple();
                        HTuple outrow = new HTuple();
                        HTuple outcol = new HTuple();
                        HObject Roi = new HObject();
                        bool rest = false;
                        if (name1.Length == 2)
                        {
                            HTuple hTuple = new HTuple(halcon.GetRunProgram()[name1[0]]["模板Row"], halcon.GetRunProgram()[name1[0]]["模板Col"]);
                            rest= PointToLineExtension( hTuple,Dic_Measure.Keys_Measure [MeasureName2],out lengtMM,out Roi, out outrow, out outcol);
                        }
                        else if (Name.Length == 1)
                        {
                  
                            rest = PointToLineExtension( halcon[name1[0]], Dic_Measure.Keys_Measure[MeasureName2], out lengtMM, out Roi,out outrow, out outcol);
                        }
                        else
                        {
                            if (this.Dic_Measure.Keys_Measure[MeasureName2].ResltBool)
                            {
                                HTuple hTuple = new HTuple(this.Dic_Measure.Keys_Measure[MeasureName2].OutCentreRow, this.Dic_Measure.Keys_Measure[MeasureName2].OutCentreCol);
                                rest = PointToLineExtension( hTuple, Dic_Measure.Keys_Measure[MeasureName1], out lengtMM, out Roi,out outrow,out outcol);
                            }
                            else return false;
                        }
                        ValuePP = lengtMM;
                        this["垂足P"] = lengtMM;
                        lengtMM = this.ScaleMM(halcon.GetCaliConstMM(lengtMM));
                        if (halcon.keyValuePairs1.ContainsKey(this.Name))
                        {
                            halcon.keyValuePairs1[this.Name + ""] = lengtMM;
                        }
                        else
                        {
                            halcon.keyValuePairs1.Add(this.Name + "", lengtMM);
                        }
                        halcon.GetResultOBj().AddImageMassage(this.Dic_Measure.Keys_Measure[MeasureName2].OutCentreRow,
                            this.Dic_Measure.Keys_Measure[MeasureName2].OutCentreCol, this.Name + "=" + lengtMM.TupleString("0.3f"), ColorResult.blue);
                        this["垂足mm"] = lengtMM;
                        if (!rest) return false;
                        if (DistanceMax < lengtMM.D || DistanceMin > lengtMM.D)
                        {
                            halcon.AddOBJ(Roi, ColorResult.red);
                            return false;
                        }
                        else halcon.AddOBJ(Roi, this.color);
                        return true;
                    case MeasureModeEnum.同心圆:
 
                        CilcreToCilcre(halcon);
                        break;
                    case MeasureModeEnum.测长:
                        this.SetDefault("Row", 0, true);
                        this.SetDefault("Col", 0, true);
                        this.SetDefault(name1[1], 0, true);
                        HOperatorSet.GenCrossContourXld(out HObject pointxld, this[name1[1]], this["Col"], 50, 0);
                        halcon.AddShowObj(pointxld);
                        this["LengtRow"] = this[name1[1]].TupleSub(this.ScaleMM( halcon.GetCaliConstMM(measure1[name1[1]])));
                        this["LengtCol"] = this["Col"].TupleSub(this.ScaleMM(halcon.GetCaliConstMM(measure1["col"])));
                        halcon.GetResultOBj().AddImageMassage(measure1[name1[1]], measure1["col"], "r:" + this["LengtRow"].TupleString("0.3f") + " c:" + this["LengtCol"].TupleString("0.3f"), ColorResult.green);
                        halcon.SendMesage("GoLengt", this["LengtRow"].TupleString("0.3f"), this["LengtCol"].TupleString("0.3f"));

                        return OK;
                    case MeasureModeEnum.线平行:
                        double dinRow1, dinCol1, dinRow2, dinCol2 = 0;
                        double dinRow21, dinCol21, dinRow22, dinCol22 = 0;
                        dinRow1 = measure2.OutRows[0];
                        dinCol1 = measure2.OutCols[0];
                        dinRow2 = measure2.OutRows[1];
                        dinCol2 = measure2.OutCols[1];
                        dinRow21 = measure1.OutRows[0];
                        dinCol21 = measure1.OutCols[0];
                        dinRow22 = measure1.OutRows[1];
                        dinCol22 = measure1.OutCols[1];
                        double dinRow3 = measure2.OutCentreRow.Value;
                        double dinCol3 = measure2.OutCentreCol.Value;
                        HOperatorSet.AngleLl(dinRow1, dinCol1, dinRow2, dinCol2, dinRow21, dinCol21, dinRow22, dinCol22, out HTuple angle);
                        HOperatorSet.ProjectionPl(dinRow1, dinCol1, dinRow21, dinCol21, dinRow22, dinCol22, out HTuple projectRow, out HTuple projectCol0);
                        HOperatorSet.DistancePp(projectRow, projectCol0, dinRow1, dinCol1, out HTuple dist);
                        Vision.Gen_arrow_contour_xld(out HObject contour1, dinRow1, dinCol1, projectRow, projectCol0);
                        HOperatorSet.ProjectionPl(dinRow2, dinCol2, dinRow21, dinCol21, dinRow22, dinCol22, out HTuple projectRow2, out HTuple projectCol02);
                        HOperatorSet.DistancePp(projectRow2, projectCol02, dinRow2, dinCol2, out HTuple dist2);
                        Vision.Gen_arrow_contour_xld(out HObject contour3, dinRow2, dinCol2, projectRow2, projectCol02);
                        HOperatorSet.ProjectionPl(dinRow3, dinCol3, dinRow21, dinCol21, dinRow22, dinCol22, out HTuple projectRow3, out HTuple projectCol03);
                        HOperatorSet.DistancePp(projectRow3, projectCol03, dinRow3, dinCol3, out HTuple dist3);
                        Vision.Gen_arrow_contour_xld(out HObject contour4, dinRow3, dinCol3, projectRow3, projectCol03);
                        HObject corss = new HObject();
                        corss.GenEmptyObj();
                        HTuple angleD = angle.TupleDeg();
                        this["夹角"] = angleD;
                        this["第1点P"] = dist;
                        this["第2点P"] = dist2;
                        this["第3点P"] = dist3;
                        ValuePP = dist2;
                        HTuple DistM = this.ScaleMM(halcon.GetCaliConstMM(dist));
                        HTuple DistM2 = this.ScaleMM(halcon.GetCaliConstMM(dist2)); 
                        HTuple DistM3 = this.ScaleMM(halcon.GetCaliConstMM(dist3));
                        this["第1点mm"] = DistM;
                        this["第2点mm"] = DistM2;
                        this["第3点mm"] = DistM3;
                        HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(dinRow1, dinRow2), new HTuple(dinCol1, dinCol2));
                        HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(dinRow21, dinRow22), new HTuple(dinCol21, dinCol22));
                        halcon.AddOBJ(hObject.ConcatObj(hObject2), ColorResult.green);
                        halcon.AddTData(DistM.D, DistM3.D, DistM2.D);
                        string massage = "";
                        if (SelePointName == "第一点" || SelePointName == "全部")
                        {
                            massage =  DistM.TupleString("0.6f");
                            if (IsRadius)
                            {
                                massage += "夹角°" + angleD.TupleString("0.2f");
                            }
                            if (halcon.keyValuePairs1.ContainsKey(this.Name + ".第1点"))
                            {
                                halcon.keyValuePairs1[this.Name + ".第1点"] = DistM;
                            }
                            else
                            {
                                halcon.keyValuePairs1.Add(this.Name + ".第1点", DistM);
                            }
                            HOperatorSet.GenCrossContourXld(out HObject corss2, new HTuple(projectRow.D, dinRow1), new HTuple(projectCol0.D, dinCol1), 20, 0);
                            corss = corss.ConcatObj(corss2);
                            if (angleD > AngleMax || angleD < AngleMin || DistM < DistanceMin || DistM > DistanceMax)
                            {
                                halcon.AddMessageIamge(projectRow, projectCol0, massage, ColorResult.red);
                                OK = false;
                                halcon.AddOBJ(contour1, ColorResult.red);
                            }
                            else
                            {
                                halcon.AddMessageIamge(projectRow - 60, projectCol0, massage);
                                halcon.AddOBJ(contour1);
                            }
                        }
                        if (SelePointName == "中心点" || SelePointName == "全部")
                        {
                            massage =  DistM3.TupleString("0.6f");
                            if (halcon.keyValuePairs1.ContainsKey(this.Name + ".第2点"))
                            {
                                halcon.keyValuePairs1[this.Name + ".第2点"] = DistM3;
                            }
                            else
                            {
                                halcon.keyValuePairs1.Add(this.Name + ".第2点", DistM3);
                            }
                            if (IsRadius)
                            {
                                massage += "夹角°" + angleD.TupleString("0.2f");
                            }
                            HOperatorSet.GenCrossContourXld(out HObject corss2, new HTuple(projectRow3.D, dinRow3), new HTuple(projectCol03.D, dinCol3), 20, 0);
                            corss = corss.ConcatObj(corss2);
                            if (angleD > AngleMax || angleD < AngleMin || DistM3 < DistanceMin || DistM3 > DistanceMax)
                            {
                                halcon.AddMessageIamge(projectRow3, projectCol03, massage, ColorResult.red);
                                OK = false;
                                halcon.AddOBJ(contour4, ColorResult.red);
                            }
                            else
                            {
                                halcon.AddOBJ(contour4);
                                halcon.AddMessageIamge(projectRow3 - 60, projectCol03, massage);
                            }
                        }
                        if (SelePointName == "结束点" || SelePointName == "全部")
                        {
                            HOperatorSet.GenCrossContourXld(out HObject corss2, new HTuple(projectRow2.D, dinRow2), new HTuple(projectCol02.D, dinCol2), 20, 0);
                            corss = corss.ConcatObj(corss2);
                            massage = DistM2.TupleString("0.6f");
                            if (IsRadius)
                            {
                                massage += "夹角" + angleD.TupleString("0.2f");
                            }
                            if (halcon.keyValuePairs1.ContainsKey(this.Name + ".第3点"))
                            {
                                halcon.keyValuePairs1[this.Name + ".第3点"] = DistM2;
                            }
                            else
                            {
                                halcon.keyValuePairs1.Add(this.Name + ".第3点", DistM2);
                            }
                            if (angleD > AngleMax || angleD < AngleMin || DistM2 < DistanceMin || DistM2 > DistanceMax)
                            {
                                halcon.AddMessageIamge(projectRow2, projectCol02, massage, ColorResult.red);
                                OK = false;
                                halcon.AddOBJ(contour3, ColorResult.red);
                            }
                            else
                            {
                                halcon.AddMessageIamge(projectRow2 - 60, projectCol02, massage);
                                halcon.AddOBJ(contour3);
                            }
                        }
                        halcon.AddOBJ(corss, ColorResult.blue);
                        break;
                    case MeasureModeEnum.圆中心:
                        if (measure1 != null)
                        {
                            if (measure1.ResltBool)
                            {
                                HOperatorSet.GenCrossContourXld(out HObject hObject1, measure1.OutCentreRow, measure1.OutCentreCol, 20, 0);
                                if (this.CoordinateMeassage == Coordinate.Coordinate_Type.XYU2D)
                                {
                                    Coordinate.CpointXY cpointXY = halcon.GetCalib().GetPointRctoYX(measure1.OutCentreRow.Value, measure1.OutCentreCol.Value);
                                    halcon.AddMessageIamge(measure1.OutCentreRow, measure1.OutCentreCol, "x:" + cpointXY.X.ToString("0.00") + "y:" + cpointXY.Y.ToString("0.00"));
                                }
                                else
                                {
                                    halcon.AddMessageIamge(measure1.OutCentreRow, measure1.OutCentreCol, "r:" + measure1.OutCentreRow.Value.ToString("0.00") + "c:" + measure1.OutCentreCol.Value.ToString("0.00"));
                                }
                                this["圆R"] = measure1.OutCentreRow.Value;
                                this["圆C"] = measure1.OutCentreCol.Value;
                                this["圆直径"] = measure1.OutRadius;
                                this["圆直径MM"] = measure1.OutRadiusMM;
                                ValuePP = measure1.OutRadius; 
                                NGRoi = NGRoi.ConcatObj(hObject1);
                                OK = true;
                            }
                            else
                            {
                                NGRoi = NGRoi.ConcatObj(measure1.GetHamMatDraw());
                                OK = false;
                            }
                        }
                        break;
                    default:
                        return true;
                }
            }
            return OK;
        }

        public override Control GetControl()
        {
            return new Controls.MeasureMletUserControlcs(this) { Dock = DockStyle.Fill };
        }
        public override RunProgram UpSatrt<T>(string Path)
        {
            return base.ReadThis<MeasureMlet>(Path);
        }

        public HTuple ScaleMM(HTuple pint)
        {
            return pint.TupleMult(Scale);
        }


        public HTuple CheckoutMM(HTuple valeus)
        {
            Scale = 0;
            return Scale;
        }
        /// <summary>
        /// 点与线之间的垂足
        /// </summary>
        /// <param name="halcon">程序</param>
        /// <param name="point">点</param>
        /// <param name="MeasureLien">直线名称</param>
        /// <returns></returns>
        public static  bool PointToLineExtension(HTuple point, Measure MeasureLien,out HTuple minLentg,out HObject RoiLengt,out HTuple outRows, out    HTuple outcols)
        {
            minLentg = new HTuple();
            RoiLengt = new HObject();
            outRows = new HTuple();
            outcols = new HTuple();
            RoiLengt.GenEmptyObj();
            try
            {
                if (MeasureLien.ISSt)
                {
                }
                HTuple hTupleRow = new HTuple();
                HTuple hTupleCol = new HTuple();
                hTupleRow.Append(MeasureLien.OutRows);
                hTupleCol.Append(MeasureLien.OutCols);
                HOperatorSet.GenContourPolygonXld(out HObject hObject, hTupleRow, hTupleCol);
                double rowt = point[0];
                double colt = point[1];
                if (MeasureLien.OutRows[0].D == 0)
                {
                    //halcon.SendMesage("Goto", "0.0");
                    return false;
                }
                HOperatorSet.ProjectionPl(rowt, colt,
                    MeasureLien.OutRows[0], MeasureLien.OutCols[0], MeasureLien.OutRows[1],
                    MeasureLien.OutCols[1], out  outRows, out  outcols);
                //HOperatorSet.AngleLx(outRows, outcols, rowt, colt, out HTuple angle);
                //HOperatorSet.AngleLl(outRows, outcols, rowt, colt, Dic_Measure[name2].OutRows[0], Dic_Measure[name2].OutCols[0], Dic_Measure[name2].OutRows[1],
                //Dic_Measure[name2].OutCols[1], out angle);
                //HTuple  angle2= angle.TupleDeg();
                HOperatorSet.DistancePp(outRows, outcols, rowt, colt, out HTuple min);
                HOperatorSet.AngleLx(outRows, outcols, rowt, colt, out HTuple angle);
                HOperatorSet.GenCrossContourXld(out HObject hObject1, rowt, colt, min / 10 + 10, angle + new HTuple(45).TupleRad());
                minLentg = min;
                //min = halcon.GetCaliConstMM(min);
                outRows.Append(rowt);
                outcols.Append(colt);
                HOperatorSet.GenContourPolygonXld(out HObject hObject2, outRows, outcols);
                HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(outRows[0].D, MeasureLien.OutRows[0], MeasureLien.OutRows[1]),
                    new HTuple(outcols[0].D, MeasureLien.OutCols[0], MeasureLien.OutCols[1]));
    
                //halcon.AddOBJ(hObject1, ColorResult.blue);
                RoiLengt = hObject2;
          
                return true;
            }
            catch (Exception)
            {
            }
  
            return false;
        }

        /// <summary>
        /// 点与线之间的垂足
        /// </summary>
        /// <param name="halcon">程序</param>
        /// <param name="point">点</param>
        /// <param name="MeasureLien">直线名称</param>
        /// <returns></returns>
        public static bool PointToLineExtension(HTuple point, HTuple lineRow,HTuple lineCol, out HTuple minLentg, out HObject RoiLengt, out HTuple outRows, out HTuple outcols)
        {
            minLentg = new HTuple();
            RoiLengt = new HObject();
            outRows = new HTuple();
            outcols = new HTuple();
            RoiLengt.GenEmptyObj();
            try
            {
                HTuple hTupleRow = new HTuple();
                HTuple hTupleCol = new HTuple();
                hTupleRow.Append(lineRow);
                hTupleCol.Append(lineCol);
                HOperatorSet.GenContourPolygonXld(out HObject hObject, hTupleRow, hTupleCol);
                double rowt = point[0];
                double colt = point[1];
                if (lineRow.Length == 0)
                {
                    return false;
                }
                HOperatorSet.ProjectionPl(rowt, colt,  lineRow[0], lineCol[0], lineRow[1],lineCol[1], out outRows, out outcols);
                HOperatorSet.DistancePp(outRows, outcols, rowt, colt, out HTuple min);
                HOperatorSet.AngleLx(outRows, outcols, rowt, colt, out HTuple angle);
                HOperatorSet.GenCrossContourXld(out HObject hObject1, rowt, colt, min / 10 + 10, angle + new HTuple(45).TupleRad());
                minLentg = min;
                outRows.Append(rowt);
                outcols.Append(colt);
                HOperatorSet.GenContourPolygonXld(out HObject hObject2, outRows, outcols);
                HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(outRows[0].D, lineRow[0], lineRow[1]),new HTuple(outcols[0].D, lineCol[0], lineCol[1]));
                RoiLengt = hObject2;
                return true;
            }
            catch (Exception)
            {
            }

            return false;
        }

        [DescriptionAttribute("显示园半径"), Category("测量结果"), DisplayName("显示半径MM")]
        public bool IsRadius { get; set; }

        [DescriptionAttribute(""), Category("测量结果"), DisplayName("转换比例")]
        public double Scale { get; set; } = 1;


        [DescriptionAttribute("选择测量点"), Category("测量点"), DisplayName("选择测量点")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", false, "第一点", "中心点", "结束点", "全部")]
        public string SelePointName { get; set; } = "全部";

        [DescriptionAttribute("是否分割图像"), Category("测量关系"), DisplayName("是否分割")]
        public bool IsReduce { get; set; }

        [DescriptionAttribute("标准最短距离"), Category("测量比对"), DisplayName("最小距离")]
        public double DistanceMin { get; set; } = 0;

        [DescriptionAttribute("标准最大距离"), Category("测量比对"), DisplayName("最大距离")]
        public double DistanceMax { get; set; } = 1000;

        [DescriptionAttribute("标准最小角度"), Category("测量比对"), DisplayName("最小角度")]
        public double AngleMin { get; set; } = -180;

        [DescriptionAttribute("标准最大角度，不超过360"), Category("测量比对"), DisplayName("最大角度")]
        public double AngleMax { get; set; } = 360;

        public bool CilcreToCilcre(HalconRun halcon)
        {
            if (!this.Dic_Measure[MeasureName1].ResltBool|| !this.Dic_Measure[MeasureName2].ResltBool)
            {
                return false;
            }
            HTuple row = this.Dic_Measure[MeasureName1].OutCentreRow;
            HTuple Col = this.Dic_Measure[MeasureName1].OutCentreCol;
            HTuple row2 = this.Dic_Measure[MeasureName2].OutCentreRow;
            HTuple Col2 = this.Dic_Measure[MeasureName2].OutCentreCol;
            HOperatorSet.DistancePp(row, Col, row2, Col2, out HTuple distance);
            ValuePP = distance;
            HTuple minMM = this.ScaleMM(halcon.GetCaliConstMM(distance)) - AddOreM;
            if (ISCompound)
            {
                halcon.AddMessageIamge(row, Col, minMM);
            }
            if (!this.IsExist("同心圆mm"))
            {
                this["同心圆mm"] = minMM;
            }
            else
            {
                this["同心圆mm"].Append(minMM);
            }
            if (!this.IsExist("同心圆PP"))
            {
                this["同心圆PP"] = distance;
            }
            else
            {
                this["同心圆PP"].Append(distance);
            }
            return true;
        }

        [DescriptionAttribute("多个测量的对象关系"), Category("测量关系"), DisplayName("测量模式")]
        public MeasureModeEnum MeasureMode { get; set; } 

        public enum MeasureModeEnum
        {
            不测良=0,
            点与线垂足=1,
            同心圆 = 2,
            测长 = 3,
            线平行 = 4,
            圆中心=5,
            胶宽测量 = 6,
            绘制区域 =7,
        }

        public HTuple ValuePP { get; set; }
  
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


    }
}