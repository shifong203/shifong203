using HalconDotNet;
using System;
using System.Collections.Generic;
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

        public override bool RunHProgram(OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj)
        {
            oneRObjs = new List<OneRObj>();
            OneRObj oneRObj = new OneRObj();
            oneRObj.ComponentID = this.Name;
            if (CRDName!="")
            {
                oneRObj.ComponentID = CRDName;
            }
            HObject hObjectRoi = new HObject();
            hObjectRoi.GenEmptyObj();
            bool OK = false; HObject image;
            //this.SetDefault("NG距离", 100, true);
            image = GetEmset(oneResultOBj.GetHalcon().GetImageOBJ(this.ImageTypeOb));
            foreach (var item in Dic_Measure.Keys_Measure)
            {
                item.Value.ImageTypeOb = this.ImageTypeOb;
                item.Value.ResltBool = false;
                if (item.Value.ISMatHat)
                {
                    if (oneResultOBj.GetHalcon().GetHomdeMobelEx(this.HomName) != null)
                    {
                        for (int i = 0; i < oneResultOBj.GetHalcon().GetHomdeMobelEx(this.HomName).HomMat.Count; i++)
                        {
                            ObjectColor objectColor = item.Value.MeasureObj(image, oneResultOBj.GetHalcon(), oneResultOBj.GetHalcon().GetHomdeMobelEx(this.HomName).HomMat[i], oneResultOBj);
                            if (objectColor._HObject.CountObj() > 0)
                            {
                                item.Value.ResltBool = true;
                                nGRoi = nGRoi.ConcatObj(objectColor._HObject);
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
                    bool ist = item.Value.Run(oneResultOBj);
                    ist = item.Value.ResltBool;
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
         
                HTuple rows = new HTuple(measure2["DrawRows"], measure1["DrawRows"]);
                HTuple cols = new HTuple(measure2["DrawCols"], measure1["DrawCols"]);
         
  
                HOperatorSet.GenRegionPolygonFilled(out hObjectRoi, rows, cols);
                HOperatorSet.SmallestRectangle1(hObjectRoi, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);

                HOperatorSet.GenRectangle1(out hObjectRoi, row1, col1, row2, col2);

                //HOperatorSet.GenRegionPoints(out hObjectRoi, rows, cols);
                //HOperatorSet.Union1(hObjectRoi, out hObjectRoi);
                switch (MeasureMode)
                {
                    case MeasureModeEnum.点与线垂足:
                        HTuple lengtMM = new HTuple(-99999);
                        HTuple outrow = new HTuple();
                        HTuple outcol = new HTuple();
                        HObject Roi = new HObject();
                        Roi.GenEmptyObj();
                        bool rest = false;
                        if (name1.Length == 2)
                        {
                            HTuple hTuple = new HTuple(oneResultOBj.GetHalcon().GetRunProgram()[name1[0]]["模板Row"], oneResultOBj.GetHalcon().GetRunProgram()[name1[0]]["模板Col"]);
                            rest = PointToLineExtension(hTuple, Dic_Measure.Keys_Measure[MeasureName2], out lengtMM, out Roi, out outrow, out outcol);
                        }
                        else if (Name.Length == 1)
                        {
                            rest = PointToLineExtension(oneResultOBj.GetHalcon()[name1[0]], Dic_Measure.Keys_Measure[MeasureName2], out lengtMM, out Roi, out outrow, out outcol);
                        }
                        else
                        {
                            if (this.Dic_Measure.Keys_Measure[MeasureName2].ResltBool && Dic_Measure.Keys_Measure[MeasureName1].ResltBool)
                            {
                                HTuple hTuple = new HTuple(this.Dic_Measure.Keys_Measure[MeasureName2].OutCentreRow, this.Dic_Measure.Keys_Measure[MeasureName2].OutCentreCol);
                                rest = PointToLineExtension(hTuple, Dic_Measure.Keys_Measure[MeasureName1], out lengtMM, out Roi, out outrow, out outcol);
                            }
                            else
                            {
                                break;
                            }
                        }
                        ValuePP = lengtMM;
                        this["垂足P"] = lengtMM;
                        lengtMM = this.ScaleMM(oneResultOBj.GetCaliConstMM(lengtMM));
                        if (oneResultOBj.GetHalcon().keyValuePairs1.ContainsKey(this.Name))
                        {
                            oneResultOBj.GetHalcon().keyValuePairs1[this.Name + ""] = lengtMM;
                        }
                        else
                        {
                            oneResultOBj.GetHalcon().keyValuePairs1.Add(this.Name + "", lengtMM);
                        }
                     
                        this["垂足mm"] = lengtMM;
                        if (!rest)
                            OK = false;
                        oneRObj.dataMinMax.AddData("垂足", lengtMM, DistanceMin, DistanceMax);
                        Roi = Roi.ConcatObj(measure2.MeasureHObj);
                        Roi = Roi.ConcatObj(measure1.MeasureHObj);
                        if (DistanceMax < lengtMM.D || DistanceMin > lengtMM.D)
                        {
                            oneResultOBj.AddObj(Roi, ColorResult.red);
                            OK = false;
                        }
                        else
                        {
                            oneResultOBj.AddObj(Roi, ColorResult.green);

                            OK = true;
                        }
                        ColorResult color = ColorResult.green;
                        if (OK)
                        {
                            color = ColorResult.red;
                        }
                        oneRObj.AddImageMessage(this.Dic_Measure.Keys_Measure[MeasureName2].OutCentreRow,
                        this.Dic_Measure.Keys_Measure[MeasureName2].OutCentreCol, 
                        lengtMM.TupleString("0." + Vision.Instance.Decimal_point + "f"));
                        if (this.ISShowText)
                        {
                            oneResultOBj.AddImageMassage(this.Dic_Measure.Keys_Measure[MeasureName2].OutCentreRow,
                             this.Dic_Measure.Keys_Measure[MeasureName2].OutCentreCol, this.Name + "=" +
                             lengtMM.TupleString("0." + Vision.Instance.Decimal_point + "f"), color);
                        }
                        break;
                    case MeasureModeEnum.同心圆:
                        OK = CilcreToCilcre(oneResultOBj, oneRObj);
                        break;

                    case MeasureModeEnum.测长:
                        this.SetDefault("Row", 0, true);
                        this.SetDefault("Col", 0, true);
                        this.SetDefault(name1[1], 0, true);
                        HOperatorSet.GenCrossContourXld(out HObject pointxld, this[name1[1]], this["Col"], 50, 0);
                        oneResultOBj.AddObj(pointxld);
                        this["LengtRow"] = this[name1[1]].TupleSub(this.ScaleMM(oneResultOBj.GetCaliConstMM(measure1[name1[1]])));
                        this["LengtCol"] = this["Col"].TupleSub(this.ScaleMM(oneResultOBj.GetCaliConstMM(measure1["col"])));
                        oneResultOBj.AddImageMassage(measure1[name1[1]], measure1["col"], "r:" + this["LengtRow"].TupleString("0.3f") + " c:" + this["LengtCol"].TupleString("0.3f"), ColorResult.green);
                        oneResultOBj.GetHalcon().SendMesage("GoLengt", this["LengtRow"].TupleString("0.3f"), this["LengtCol"].TupleString("0.3f"));
                        oneRObj.AddImageMessage(measure1[name1[1]], measure1["col"], "r:" + this["LengtRow"].TupleString("0.3f") + " c:" + this["LengtCol"].TupleString("0.3f"));
                        break;

                    case MeasureModeEnum.线平行:
                        if (measure2.ResltBool && measure1.ResltBool)
                        {
                            OK = true;
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
                            HOperatorSet.DistancePp(projectRow2, projectCol02, dinRow2, dinCol2, out HTuple dist3);
                            Vision.Gen_arrow_contour_xld(out HObject contour3, dinRow2, dinCol2, projectRow2, projectCol02);

                            HOperatorSet.ProjectionPl(dinRow3, dinCol3, dinRow21, dinCol21, dinRow22, dinCol22, out HTuple projectRow3, out HTuple projectCol03);
                            HOperatorSet.DistancePp(projectRow3, projectCol03, dinRow3, dinCol3, out HTuple dist2);

                            Vision.Gen_arrow_contour_xld(out HObject contour4, dinRow3, dinCol3, projectRow3, projectCol03);
                            HObject corss = new HObject();
                            corss.GenEmptyObj();

                            HTuple angleD = angle.TupleDeg();
                            this["夹角"] = angleD;
                            this["第1点P"] = dist;
                            this["第2点P"] = dist2;
                            this["第3点P"] = dist3;
                            ValuePP = dist2;

                            HTuple DistM = this.ScaleMM(oneResultOBj.GetCaliConstMM(dist));
                            HTuple DistM2 = this.ScaleMM(oneResultOBj.GetCaliConstMM(dist2));
                            HTuple DistM3 = this.ScaleMM(oneResultOBj.GetCaliConstMM(dist3));
                            this["第1点mm"] = DistM;
                            this["第2点mm"] = DistM2;
                            this["第3点mm"] = DistM3;
                            HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(dinRow1, dinRow2), new HTuple(dinCol1, dinCol2));
                            HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(dinRow21, dinRow22), new HTuple(dinCol21, dinCol22));
                            oneResultOBj.AddObj(hObject.ConcatObj(hObject2), ColorResult.green);
                            oneResultOBj.GetHalcon().AddTData(DistM.D, DistM3.D, DistM2.D);
                            string massage = "";
                            if (SelePointName == "第一点" || SelePointName == "全部")
                            {
                                oneRObj.dataMinMax.AddData("第一点", DistM, DistanceMin, DistanceMax);
                                massage = DistM.TupleString("0." + Vision.Instance.Decimal_point + "f") + Vision.Instance.TransformName;
                               
                                    massage += "夹角°" + angleD.TupleString("0.2f");
                                
                                if (oneResultOBj.GetHalcon().keyValuePairs1.ContainsKey(this.Name + ".第1点"))
                                {
                                    oneResultOBj.GetHalcon().keyValuePairs1[this.Name + ".第1点"] = DistM;
                                }
                                else
                                {
                                    oneResultOBj.GetHalcon().keyValuePairs1.Add(this.Name + ".第1点", DistM);
                                }
                                HOperatorSet.GenCrossContourXld(out HObject corss2, new HTuple(projectRow.D, dinRow1), new HTuple(projectCol0.D, dinCol1), 20, 0);
                                corss = corss.ConcatObj(corss2);
                                if (angleD > AngleMax || angleD < AngleMin || DistM < DistanceMin || DistM > DistanceMax)
                                {
                                    oneResultOBj.AddImageMassage(projectRow, projectCol0, massage, ColorResult.red);
                                    OK = false;
                                    oneResultOBj.AddObj(contour1, ColorResult.red);
                                }
                                else
                                {
                                    oneResultOBj.AddImageMassage(projectRow - 60, projectCol0, massage);
                                    oneResultOBj.AddObj(contour1);
                                }
                                oneRObj.AddImageMessage(projectRow - 60, projectCol0, massage);
                            }
                            if (SelePointName == "中心点" || SelePointName == "全部")
                            {
                                oneRObj.dataMinMax.AddData("中心点", DistM2, DistanceMin, DistanceMax);
                                massage = DistM2.TupleString("0." + Vision.Instance.Decimal_point + "f") + Vision.Instance.TransformName;
                                if (oneResultOBj.GetHalcon().keyValuePairs1.ContainsKey(this.Name + ".第2点"))
                                {
                                    oneResultOBj.GetHalcon().keyValuePairs1[this.Name + ".第2点"] = DistM2;
                                }
                                else
                                {
                                    oneResultOBj.GetHalcon().keyValuePairs1.Add(this.Name + ".第2点", DistM2);
                                }
                           
                                    massage += "夹角°" + angleD.TupleString("0.2f");
                                
                                HOperatorSet.GenCrossContourXld(out HObject corss2, new HTuple(projectRow3.D, dinRow3), new HTuple(projectCol03.D, dinCol3), 20, 0);
                                corss = corss.ConcatObj(corss2);
                                if (angleD > AngleMax || angleD < AngleMin || DistM2 < DistanceMin || DistM2 > DistanceMax)
                                {
                                    oneResultOBj.AddImageMassage(projectRow3, projectCol03, massage, ColorResult.red);
                                    OK = false;
                                    oneResultOBj.AddObj(contour4, ColorResult.red);
                                }
                                else
                                {
                                    oneResultOBj.AddObj(contour4);
                                    oneResultOBj.AddImageMassage(projectRow3 - 60, projectCol03, massage);
                                }
                                oneRObj.AddImageMessage(projectRow3 - 60, projectCol03, massage);
                            }
                            if (SelePointName == "结束点" || SelePointName == "全部")
                            {
                                oneRObj.dataMinMax.AddData("结束点", DistM3, DistanceMin, DistanceMax);
                                HOperatorSet.GenCrossContourXld(out HObject corss2, new HTuple(projectRow2.D, dinRow2), new HTuple(projectCol02.D, dinCol2), 20, 0);
                                corss = corss.ConcatObj(corss2);
                                massage = DistM3.TupleString("0." + Vision.Instance.Decimal_point + "f") + Vision.Instance.TransformName;
                             
                                 massage += "夹角" + angleD.TupleString("0.2f");
                                
                                if (oneResultOBj.GetHalcon().keyValuePairs1.ContainsKey(this.Name + ".第3点"))
                                {
                                    oneResultOBj.GetHalcon().keyValuePairs1[this.Name + ".第3点"] = DistM3;
                                }
                                else
                                {
                                    oneResultOBj.GetHalcon().keyValuePairs1.Add(this.Name + ".第3点", DistM3);
                                }
                                if (angleD > AngleMax || angleD < AngleMin || DistM3 < DistanceMin || DistM3 > DistanceMax)
                                {
                                    oneResultOBj.AddImageMassage(projectRow2, projectCol02, massage, ColorResult.red);
                                    OK = false;
                                    oneResultOBj.AddObj(contour3, ColorResult.red);
                                }
                                else
                                {
                                    oneResultOBj.AddImageMassage(projectRow2 - 60, projectCol02, massage);
                                    oneResultOBj.AddObj(contour3);
                                }
                                oneRObj.AddImageMessage(projectRow2 - 60, projectCol02, massage);
                            }
                            oneResultOBj.AddObj(corss, ColorResult.blue);
                        }
                        break;
                    case MeasureModeEnum.圆中心:
                        if (measure1 != null)
                        {
                            if (measure1.ResltBool)
                            {
                                HOperatorSet.GenCrossContourXld(out HObject hObject1, measure1.OutCentreRow, measure1.OutCentreCol, 20, 0);
                                if (this.CoordinateMeassage == Coordinate.Coordinate_Type.XYU2D)
                                {
                                    Coordinate.CpointXY cpointXY = oneResultOBj.GetHalcon().GetCalib().GetPointRctoYX(measure1.OutCentreRow.Value, measure1.OutCentreCol.Value);
                                    oneResultOBj.AddImageMassage(measure1.OutCentreRow, measure1.OutCentreCol, "x:" + cpointXY.X.ToString("0.00") + "y:" + cpointXY.Y.ToString("0.00"));
                                }
                                else
                                {
                                    oneResultOBj.AddImageMassage(measure1.OutCentreRow, measure1.OutCentreCol, "r:" + measure1.OutCentreRow.Value.ToString("0.00") + "c:" + measure1.OutCentreCol.Value.ToString("0.00"));
                                }
                                oneRObj.AddImageMessage(measure1.OutCentreRow, measure1.OutCentreCol, "r:" 
                                    + measure1.OutCentreRow.Value.ToString("0.00") + "c:" +
                                    measure1.OutCentreCol.Value.ToString("0.00"));
                                this["圆R"] = measure1.OutCentreRow.Value;
                                this["圆C"] = measure1.OutCentreCol.Value;
                                this["圆直径"] = measure1.OutRadius;
                                this["圆直径MM"] = measure1.OutRadiusMM;
                                ValuePP = measure1.OutRadius;
                                nGRoi = nGRoi.ConcatObj(hObject1);
                                OK = true;
                            }
                            else
                            {
                                nGRoi = nGRoi.ConcatObj(measure1.GetHamMatDraw());
                                OK = false;
                            }
                        }
                        break;
                    case MeasureModeEnum.坐标:
                        OK=      ZuobiaoDifp(measure1, measure2, oneResultOBj, oneRObj,aoiObj);
                      break;

                    default:
                        return true;
                }
            }
            HOperatorSet.AreaCenter(hObjectRoi, out HTuple area, out HTuple row, out HTuple colue);
            HOperatorSet.DilationCircle(hObjectRoi, out HObject hObjectRoi2, 100);
            if (row.Length >= 1)
            {
                oneRObj.Row = row.TupleInt();
                oneRObj.Col = colue.TupleInt();
            }
            oneRObj.RestStrings .AddRange(this.GetBackNames().ToSArr());
            oneRObj.NGROI = hObjectRoi;
            oneRObj.ROI = hObjectRoi2;
            oneRObj.NGText = this.Name;
            oneRObj.Done =oneRObj.aOK = OK;
            oneResultOBj.AddNGOBJ(oneRObj);
            return OK;
        }

        public double  dinRowX2, dinColX2 ;
        public double  dinRowY2, dinColY2 ;
        public double XYzroeRows, XYzroeColus;

        public bool Cier { get; set; }
        /// <summary>
        /// 坐标处理
        /// </summary>
        /// <param name="measure1"></param>
        /// <param name="measure2"></param>
        /// <param name="oneResultOBj"></param>
        /// <param name="oneRObj"></param>
        /// <returns></returns>
        public bool ZuobiaoDifp(Measure measure1,  Measure measure2 , OneResultOBj oneResultOBj, OneRObj oneRObj, AoiObj aoiObj)
        {
            bool ok;
            try
            {
                if (measure2.ResltBool && measure1.ResltBool)
                {
                    ok = true;
                    double dinRowX1=0, dinColX1=  0;
                    double dinRowY1=0, dinColY1=  0;
            
                    if (Cier)
                    {
                        dinRowX1 = measure1.OutCentreRow.Value;
                        dinColX1 = measure1.OutCentreCol.Value;
                        dinRowX2 = measure2.OutCentreRow.Value;
                        dinColX2 = measure2.OutCentreCol.Value;
                        HOperatorSet.LinePosition(dinRowX1, dinColX1, dinRowX2, dinColX2, out HTuple rowc, out HTuple colc, out HTuple length, out HTuple phti);
                        XYzroeRows = rowc;
                        XYzroeColus = colc;
                        HOperatorSet.GenCrossContourXld(out HObject hObjectd, dinRowX2, dinColX2, 100, 0);
                        oneResultOBj.AddObj(hObjectd);

                    }
                    else
                    {
                        dinRowX1 = measure1.OutRows[0];
                        dinColX1 = measure1.OutCols[0];
                        dinRowX2 = measure1.OutRows[1];
                        dinColX2 = measure1.OutCols[1];
                        dinRowY1 = measure2.OutRows[0];
                        dinColY1 = measure2.OutCols[0];
                        dinRowY2 = measure2.OutRows[1];
                        dinColY2 = measure2.OutCols[1];
                        HOperatorSet.AngleLl(dinRowX1, dinColX1, dinRowX2, dinColX2, dinRowY1, dinColY1, dinRowY2, dinColY2, out HTuple angle);
                        HOperatorSet.IntersectionLines(dinRowX1, dinColX1, dinRowX2, dinColX2, dinRowY1, dinColY1, dinRowY2, dinColY2,
                            out HTuple XYzroerows, out HTuple XYzroecolus, out HTuple isOvel);
                        oneResultOBj.AddImageMassage(XYzroerows - 200, XYzroecolus, angle.TupleDeg(), ColorResult.red);
                        XYzroeRows = XYzroerows;
                        XYzroeColus = XYzroecolus;
                        Measure.Pts_Ling_Extension(dinRowY1, dinColY1, XYzroeRows, XYzroeColus, 2000, out double rowY1, out double colY1);
                        Measure.Pts_Ling_Extension(dinRowY1, dinColY1, XYzroeRows, XYzroeColus, -2000, out double rowY2, out double colY2);
                        Vision.Gen_arrow_contour_xld(out HObject axsY, rowY1, colY1, rowY2, colY2);
                        oneResultOBj.AddObj(axsY, ColorResult.red);
                    } 
                  
                    Measure.Pts_Ling_Extension(dinRowX1, dinColX1, XYzroeRows, XYzroeColus, 2000, out double row, out double col);
                    Measure.Pts_Ling_Extension(dinRowX1, dinColX1, XYzroeRows, XYzroeColus, -2000, out double rowX2, out double colX2);

               
                 
                    HOperatorSet.LineOrientation(dinRowX1, dinColX1, XYzroeRows, XYzroeColus, out HTuple phi);

                    Vision.GetLinePoint(XYzroeRows, XYzroeColus, phi + new HTuple(90).TupleRad(), 2000, out HTuple yLineR, out HTuple yLineC);
                    Vision.GetLinePoint(XYzroeRows, XYzroeColus, phi + new HTuple(270).TupleRad(), 2000, out HTuple yLineR1, out HTuple yLineC1);
                    
                    //rowY1 = yLineR1[1];
                    //colY1 = yLineC1[1];
                    //rowY2 = yLineR[1];
                    //colY2 = yLineC[1];
                    dinRowY1 = yLineR1[1]; 
                    dinColY1 = yLineC1[1];
                    dinRowY2 = yLineR[1];
                    dinColY2 = yLineC[1];
                
                    dinRowX2 = row;
                    dinColX2 = col;

                    if (XDirection)
                    {
                        double xr = dinRowX1;
                        double xc = dinColX1;
                        dinRowX1 = dinRowX2;
                        dinColX1 = dinColX2;
                        dinRowX2 = xr;
                        dinColX2 = xc;
                    }
                    if (YDirection)
                    {
                        double xr = dinRowY1;
                        double xc = dinColY1;
                        dinRowY1 = dinRowY2;
                        dinColY2 = dinColY2;
                        dinRowY2 = xr;
                        dinColY2 = xc;
                    }
                    Vision.Gen_arrow_contour_xld(out HObject axsX, dinRowX1, dinColX1, dinRowX2, dinColX2);

                   

                    Vision.Gen_arrow_contour_xld(out HObject lineY, yLineR1[1], yLineC1[1], yLineR[1], yLineC[1]);
                    

                    oneResultOBj.AddObj(axsX, ColorResult.blue);
                   
                    oneResultOBj.AddObj(lineY, ColorResult.yellow);
             
                    HOperatorSet.GenCrossContourXld(out HObject hObject2, XYzroeRows, XYzroeColus, 100, 0);
                    oneResultOBj.AddObj(hObject2 );
                    HOperatorSet.GenCrossContourXld(out HObject hObject, dinRowX2, dinColX2, 100, 0);
                    oneResultOBj.AddObj(hObject);
                    HOperatorSet.GenCrossContourXld(out hObject, dinRowY2, dinColY2, 100, 0);
                    oneResultOBj.AddObj(hObject);
                    oneResultOBj.AddImageMassage(dinRowY2, dinColY2, "Y", ColorResult.red);
                    oneResultOBj.AddImageMassage(dinRowX2, dinColX2, "X", ColorResult.red);
                    if (aoiObj.DebugID==1)
                    {
                        return false;
                    }
                    foreach (var item in MestaNames)
                    {
                        item.Value. Mestan(oneResultOBj, this, XYzroeRows, XYzroeColus, dinRowX2, dinColX2, dinRowY2, dinColY2);
                        continue;
                        Measure.Pts_Ling_Extension(dinRowX1, dinColX1, XYzroeRows, XYzroeColus,
                            oneResultOBj.GetCaliConstPx(item.Value.MeasureX), out double ro2, out double co2);
                        Measure.Pts_Ling_Extension(dinRowY1, dinColY1, XYzroeRows, XYzroeColus,
                            oneResultOBj.GetCaliConstPx(item.Value.MeasureY), out double drY2, out double dcoY2);
                        HOperatorSet.GenCrossContourXld(out  hObject, ro2, co2, 100, 0);
                        oneResultOBj.AddObj(hObject, ColorResult.blue);
                        HOperatorSet.DistancePl(ro2, co2, dinRowY1, dinColY1, dinRowY2, dinColY2, out HTuple distY1);
              
                        Vision.GetLinePoint(ro2, co2, phi + new HTuple(90).TupleRad(), oneResultOBj.GetCaliConstPx(-2.06),out HTuple rowd,out HTuple cosd );
                        HOperatorSet.GenCrossContourXld(out HObject hObject1d, rowd, cosd, 100, 0);
                        oneResultOBj.AddObj(hObject1d, ColorResult.blue);
                        oneResultOBj.AddImageMassage(ro2, co2, "Y" + oneResultOBj.GetCaliConstMM(distY1), ColorResult.red);
                   
                        DispPoist(oneResultOBj, drY2, dcoY2, dinRowX1, dinColX1, dinRowX2, dinColX2,out HTuple dist);
                        HOperatorSet.GenCrossContourXld(out  hObject1d, drY2, dcoY2, 100, 0);
                        oneResultOBj.AddObj(hObject1d, ColorResult.blue);
                        oneResultOBj.AddImageMassage(drY2, dcoY2, "Y" + oneResultOBj.GetCaliConstMM(dist), ColorResult.red);

                        HTuple hTuple = this.Dic_Measure[item.Value.MeasureName].DrawCols = cosd[1];
                        HTuple hTupleR = this.Dic_Measure[item.Value.MeasureName].DrawRows = rowd[1]; 
                        this.Dic_Measure[item.Value.MeasureName].Run(oneResultOBj);
                        double Rows = this.Dic_Measure[item.Value.MeasureName].OutCentreRow.Value;
                        double Cols = this.Dic_Measure[item.Value.MeasureName].OutCentreCol.Value;
                        HOperatorSet.DistancePl(Rows, Cols, dinRowY1, dinColY1, dinRowY2, dinColY2, out HTuple distY);
                        HOperatorSet.ProjectionPl(Rows, Cols, dinRowY1, dinColY1, dinRowY2, dinColY2,  out HTuple rowProj, out HTuple colProj);
                            Vision.Gen_arrow_contour_xld(out HObject axstY, rowProj, colProj, Rows, Cols);
                            distY = oneResultOBj.GetCaliConstMM(distY);
                            HOperatorSet.DistancePl(Rows, Cols, dinRowX1, dinColX1, dinRowX2, dinColX2, out HTuple distX);
                            HOperatorSet.ProjectionPl(Rows, Cols, dinRowX1, dinColX1, dinRowX2, dinColX2,
                                out  rowProj, out  colProj);
                            Vision.Gen_arrow_contour_xld(out HObject axstX, rowProj, colProj, Rows, Cols);
                            oneResultOBj.AddObj(axstX, ColorResult.blue);
                            oneResultOBj.AddObj(axstY, ColorResult.yellow);
                        distX = oneResultOBj.GetCaliConstMM(distX);
                            oneResultOBj.AddImageMassage(Rows, Cols,"X"+ distX + "Y"+  distY, ColorResult.red);
                        item.Value.ValueX = distY;
                        item.Value.ValueY = distX;

                    }

                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        public void DispPoist(OneResultOBj oneResultOBj,double poisR, double poisC, double row1, double col1, double row2, double col2,out HTuple dist)
        {
            dist = new HTuple();

            try
            {

                HOperatorSet.GenCrossContourXld(out HObject hObject1, poisR, poisC, 100, 0);
                HOperatorSet.DistancePl(poisR, poisC, row1, col1, row2, col2, out  dist);
                //oneResultOBj.AddImageMassage(poisR, poisC, "Dp" + oneResultOBj.GetCaliConstMM(dist), ColorResult.red);
                //oneResultOBj.AddObj(hObject1, ColorResult.yellow);
            }
            catch (Exception)
            {
            }

        }
        public class MestanData
        {
            public string MeasureName { get; set; } = "";
            public string MeasureCalss { get; set; } = "";

            public double MeasureMin { get; set; }

            public double MeasureMax { get; set; } = 100;
            /// <summary>
            /// 参考值
            /// </summary>
            public double MeasureX { get; set; }
            /// <summary>
            /// 参考值
            /// </summary>
            public double MeasureY { get; set; }
            /// <summary>
            /// 实际值
            /// </summary>
            public double ValueX { get; set; }
            /// <summary>
            /// 实际值
            /// </summary>
            public double ValueY { get; set; }
            public double ValueXp { get; set; }
            public double ValueYp { get; set; }

            public void Mestan(OneResultOBj oneResultOBj  , MeasureMlet measureMlet,double XYzroerows,double XYzroecolus,  double xpointR,double xpointC, double ypointR, double ypointC )
            {
                try
                {
                  
                    //求测量基准位置，并显示黄色
                    Measure.Pts_Ling_Extension(  xpointR, xpointC, XYzroerows, XYzroecolus,
                  oneResultOBj.GetCaliConstPx(-MeasureX), out double ro2, out double co2);
                    HOperatorSet.GenCrossContourXld(out HObject hObject, ro2, co2, 100, 0);
                    oneResultOBj.AddObj(hObject, ColorResult.yellow);
                    //计算基准X距离到轴线的距离是否相等
                    HOperatorSet.DistancePl(ro2, co2, ypointR, ypointC, XYzroerows, XYzroecolus, out HTuple distY1);
                
               
                   
                    //求测量基准位置，并显示黄色
                    Measure.Pts_Ling_Extension(ypointR, ypointC, XYzroerows, XYzroecolus,
                        oneResultOBj.GetCaliConstPx(-MeasureY), out double drY2, out double dcoY2);

                   
                    //计算基准X距离到轴线的距离是否相等
                    measureMlet.DispPoist(oneResultOBj, drY2, dcoY2, xpointR, xpointC, XYzroerows, XYzroecolus, out HTuple dist);
                  
                    HOperatorSet.LineOrientation(xpointR, xpointC, XYzroerows, XYzroecolus, out HTuple phi);

                    ///再X轴点上做参考测量点
                    Vision.GetLinePoint(ro2, co2, phi+  new HTuple(90).TupleRad(), oneResultOBj.GetCaliConstPx(MeasureY), out HTuple rowd, out HTuple cosd);
                    if (oneResultOBj.DebugID == 2)
                    { ///    
                        oneResultOBj.AddImageMassage(drY2, dcoY2, "y" + oneResultOBj.GetCaliConstMM(dist), ColorResult.red);
                        distY1 = oneResultOBj.GetCaliConstMM(distY1);
                        oneResultOBj.AddImageMassage(ro2, co2, "x" + distY1, ColorResult.red);
                        HOperatorSet.GenCrossContourXld(out HObject hObject1d, drY2, dcoY2, 100, 0);
                        oneResultOBj.AddObj(hObject1d, ColorResult.yellow);
                        HOperatorSet.GenCrossContourXld(out  hObject1d, rowd, cosd, 100, 0);
                        oneResultOBj.AddObj(hObject1d, ColorResult.blue);
                    }

                    HTuple hTuple = measureMlet.Dic_Measure[MeasureName].DrawCols = cosd[1];
                    HTuple hTupleR = measureMlet.Dic_Measure[MeasureName].DrawRows = rowd[1];
                    measureMlet.Dic_Measure[MeasureName].Run(oneResultOBj);
                    double Rows = measureMlet.Dic_Measure[MeasureName].OutCentreRow.Value;
                    double Cols = measureMlet.Dic_Measure[MeasureName].OutCentreCol.Value;

                    HOperatorSet.DistancePl(Rows, Cols, XYzroerows, XYzroecolus, ypointR, ypointC, out HTuple distY);
                    HOperatorSet.ProjectionPl(Rows, Cols, XYzroerows, XYzroecolus, ypointR, ypointC, out HTuple rowProj, out HTuple colProj);
                    Vision.Gen_arrow_contour_xld(out HObject axstY, rowProj, colProj, Rows, Cols);
                    ValueYp = distY;

                    distY = oneResultOBj.GetCaliConstMM(distY);
                    HOperatorSet.DistancePl(Rows, Cols, xpointR, xpointC, XYzroerows, XYzroecolus, out HTuple distX);
                    HOperatorSet.ProjectionPl(Rows, Cols, xpointR, xpointC, XYzroerows, XYzroecolus,
                        out rowProj, out colProj);
                    Vision.Gen_arrow_contour_xld(out HObject axstX, rowProj, colProj, Rows, Cols);
                    oneResultOBj.AddObj(axstX, ColorResult.blue);
                    oneResultOBj.AddObj(axstY, ColorResult.yellow);
                    ValueXp = distX;
                    distX = oneResultOBj.GetCaliConstMM(distX);
                    oneResultOBj.AddImageMassage(Rows, Cols,"x" + distY + "y" + distX, ColorResult.red);
                    ValueX = distY;
                    ValueY = distX;

                }
                catch (Exception)
                {
                }
            }
        }

        public bool XDirection { get; set; }

        public bool YDirection { get; set; }

        public Dictionary<string,MestanData> MestaNames { get; set; } = new Dictionary< string,MestanData>();
        //public static void Disp
        public override Control GetControl(IDrawHalcon halcon)
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
        public static bool PointToLineExtension(HTuple point, Measure MeasureLien, out HTuple minLentg, out HObject RoiLengt, out HTuple outRows, out HTuple outcols)
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
                    MeasureLien.OutCols[1], out outRows, out outcols);
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

                //halcon.AddObj(hObject1, ColorResult.blue);
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
        public static bool PointToLineExtension(HTuple point, HTuple lineRow, HTuple lineCol, out HTuple minLentg, out HObject RoiLengt, out HTuple outRows, out HTuple outcols)
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
                HOperatorSet.ProjectionPl(rowt, colt, lineRow[0], lineCol[0], lineRow[1], lineCol[1], out outRows, out outcols);
                HOperatorSet.DistancePp(outRows, outcols, rowt, colt, out HTuple min);
                HOperatorSet.AngleLx(outRows, outcols, rowt, colt, out HTuple angle);
                HOperatorSet.GenCrossContourXld(out HObject hObject1, rowt, colt, min / 10 + 10, angle + new HTuple(45).TupleRad());
                minLentg = min;
                outRows.Append(rowt);
                outcols.Append(colt);
                HOperatorSet.GenContourPolygonXld(out HObject hObject2, outRows, outcols);
                HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(outRows[0].D, lineRow[0], lineRow[1]), new HTuple(outcols[0].D, lineCol[0], lineCol[1]));
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

        /// <summary>
        /// 参考值
        /// </summary>
        [DescriptionAttribute(""), Category("测量结果"), DisplayName("参考值")]
        public double ResValue { get; set; } = 1;

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

        public bool CilcreToCilcre(OneResultOBj halcon, OneRObj oneRObj)
        {
            this["同心圆mm"] = new HTuple(-99999);
            if (!this.Dic_Measure[MeasureName1].ResltBool || !this.Dic_Measure[MeasureName2].ResltBool)
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
            halcon.AddImageMassage(row, Col, minMM);
            oneRObj.AddImageMessage(row, Col, minMM);
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
            不测良 = 0,
            点与线垂足 = 1,
            同心圆 = 2,
            测长 = 3,
            线平行 = 4,
            圆中心 = 5,
            胶宽测量 = 6,
            坐标 = 7,
            绘制区域 = 8,
      
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