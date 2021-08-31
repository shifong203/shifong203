using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public class PinT : RunProgram
    {
        public override Control GetControl(IDrawHalcon halcon)
        {
            return new Controls.PinUserControl1(this);
        }

        public override RunProgram UpSatrt<T>(string Path)
        {
            return base.ReadThis<PinT>(Path);
        }

        public PinT()
        {
            this.PintOBj.GenEmptyObj();
        }

        public PinT(string name)
        {
            Name = name;
        }

        public bool isCTX { get; set; } = false;

        public List<double> ListCTY { get; set; } = new List<double>();
        public int EnveType { get; set; } = 0;
        public HTuple HalconWindow { get; set; }

        public byte RunIDMAX { get; set; } = 7;

        public byte ThrMin { get; set; } = 10;

        public byte ThrMax { get; set; } = 255;

        public string XAxisName { get; set; } = "";

        public string YAxisName { get; set; } = "";

        public string X2AxisName { get; set; } = "";

        public string Y2AxisName { get; set; } = "";
        public byte ThrMin2 { get; set; } = 200;
        public byte ThrMax2 { get; set; } = 255;

        public Byte ThrMinA { get; set; } = 0;
        public byte ThrMaxA { get; set; } = 50;
        public byte ThrMin3 { get; set; } = 50;
        public byte ThrMinExternal { get; set; } = 0;

        /// <summary>
        /// 针尖闭合运算
        /// </summary>
        public double CloseCirt { get; set; } = 2;

        public byte ThrMaxExternal { get; set; } = 50;
        public double PinHeightExternal { get; set; } = 40;

        public double PinHeightExternalPercent { get; set; } = 60;
        public byte ThrMax3 { get; set; } = 255;
        public HTuple Rows { get; set; } = new HTuple();
        public HTuple Columns { get; set; } = new HTuple();

        public HTuple Min { get; set; } = new HTuple(10.0, 2.0);
        public HTuple Max { get; set; } = new HTuple(10.0, 1000.0);

        /// <summary>
        /// 针区RY参考点
        /// </summary>
        public HTuple MRows { get; set; } = new HTuple();

        /// <summary>
        /// 针区CX参考点
        /// </summary>
        public HTuple MColumns { get; set; } = new HTuple();

        /// <summary>
        /// 针区R
        /// </summary>
        public HTuple ModeRow { get; set; } = new HTuple();

        /// <summary>
        /// 针区C
        /// </summary>
        public HTuple ModeColumn { get; set; } = new HTuple();

        public HTuple rowRenge1 { get; set; } = new HTuple();

        public HTuple ColumnRenge2 { get; set; } = new HTuple();

        public HTuple Lengt1Renge2 { get; set; } = new HTuple();
        public HTuple PhiRenge2 { get; set; } = new HTuple();

        public HTuple Lengt2Renge2 { get; set; } = new HTuple();

        public int RowNumber { get; set; } = 2;

        public int ColumnNumber { get; set; } = 12;

        /// <summary>
        /// 行间距
        /// </summary>
        public double RowMM { get; set; }

        /// <summary>
        /// 列间距
        /// </summary>
        public double ColumnMM { get; set; }

        public double ToleranceMM { get; set; } = 0.1;

        public double DifferenceRow { get; set; } = 30;

        public double DifferenceCol { get; set; } = 100;
        public bool SelsMax { get; set; }

        /// <summary>
        /// 黑到白检测
        /// </summary>
        public bool IsHtoB { get; set; }

        public int PintWrt { get; set; } = 30;
        public double ErosionWaightValue { get; set; }
        public double ErosionHeightValue { get; set; }
        public double OpeningValue { get; set; } = 0;

        public override string ShowHelpText()
        {
            return "2.3_创建Pin识别";
        }

        public Select_shape_Min_Max select_Shape_Min_Max { get; set; } = new Select_shape_Min_Max();
        public HTuple SelectShapeMin { get; set; } = 100;
        public HTuple SelectShapeMax { get; set; } = 9999999999;
        public HTuple SelectShapeType { get; set; } = new HTuple("area");

        public class PintC_F_M
        {
            public PintC_F_M()
            {
                HObject.GenEmptyObj();
            }

            public HObject HObject = new HObject();

            public HObject FM_c(HObject image, HObject difHobj)
            {
                HOperatorSet.Difference(HObject, difHobj, out HObject hObject1);
                HOperatorSet.ReduceDomain(image, hObject1, out HObject hObject2);
                HOperatorSet.Threshold(hObject2, out hObject2, MinThr, MaxThr);
                HOperatorSet.Connection(hObject2, out hObject2);
                return hObject2;
            }

            public byte MinThr { get; set; } = 0;
            public byte MaxThr { get; set; } = 100;
        }

        //public List<PintC_F_M> ListHobj { get; set; } = new List<PintC_F_M>();

        public void SetOre()
        {
            //ModeRow = new HTuple();
        }

        public override bool RunHProgram(OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj)
        {
            oneRObjs = new List<OneRObj>();
            bool isOk = false;
            int err = 0;
            bool istBool = false;
            try
            {
                //ISCompound = false;
                oneResultOBj.GetHalcon().GetHomdeMobel(this.HomName);
                HObject hObject, ho_PinSort;
                nGRoi = new HObject();
                nGRoi.GenEmptyObj();
                //Pin好坏
                HTuple rows, columns, area;
                HObject FreiAmpImage;
                HOperatorSet.GenRectangle2(out hObject, rowRenge1, ColumnRenge2, PhiRenge2, Lengt1Renge2, Lengt2Renge2);

                hObject = oneResultOBj.GetHalcon().GetModelHaoMatRegion(HomName, hObject);
                //halcon.AddObj(hObject);
                //xld = hObject;
                HOperatorSet.ReduceDomain(oneResultOBj.Image, hObject, out FreiAmpImage);
                //if (id != 0)
                //{
                //    HOperatorSet.DispObj(oneResultOBj.Image, oneResultOBj.h());
                //}
                if (this.Rows.Length != 0)
                {
                    HOperatorSet.GenRectangle2(out HObject hObject2, this.Rows, this.Columns, HTuple.TupleGenConst(this.Columns.Length, 0),
                     HTuple.TupleGenConst(this.Columns.Length, this.PinHeightExternal), HTuple.TupleGenConst(this.Columns.Length, this.PinHeightExternal));
                    HOperatorSet.Union1(hObject2, out hObject2);
                    HOperatorSet.ReduceDomain(FreiAmpImage, hObject2, out FreiAmpImage);
                }
                try
                {
                    CheckPin(oneResultOBj, FreiAmpImage, out rows, out columns, aoiObj.DebugID);

                    HTuple rowCent, columnCent, phi, lengt;
                    DicHtuple.SortTuple(rows, columns, out HTuple rows2, out HTuple columns2);
                    columns = new HTuple();
                    rows = new HTuple();
                    HTuple RowMt = new HTuple();
                    HTuple ColMt = new HTuple();
                    for (int i = 0; i < RowNumber; i++)
                    {
                        DicHtuple.SortTuple(columns2.TupleSelectRange(columns2.Length / RowNumber * i, columns2.Length / RowNumber * (i + 1) - 1),
                            rows2.TupleSelectRange(columns2.Length / RowNumber * i, columns2.Length / RowNumber * (i + 1) - 1), out HTuple columns3, out HTuple rows3);
                        columns.Append(columns3);
                        rows.Append(rows3);
                    }
                    if (EnveType == 0)
                    {
                        if (rows.Length != ColumnNumber * RowNumber)
                        {
                            oneResultOBj.AddMeassge("点数量错误" + rows.Length);
                            err++;
                        }
                        if (aoiObj.DebugID < 7)
                        {
                            for (int i = 0; i < rows.Length - 1; i++)
                            {
                                /// Col横向
                                if (i % ColumnNumber != ColumnNumber - 1 || RowNumber == 1)
                                {
                                    HOperatorSet.LinePosition(rows.TupleSelect(i), columns.TupleSelect(i), rows.TupleSelect(i + 1), columns.TupleSelect(i + 1), out rowCent, out columnCent, out lengt, out phi);
                                    HOperatorSet.GenRegionPolygon(out ho_PinSort, new HTuple(rows.TupleSelect(i), rows.TupleSelect(i + 1)), new HTuple(columns.TupleSelect(i), columns.TupleSelect(i + 1)));
                                    lengt = oneResultOBj.GetCaliConstMM(lengt);
                                    RowMt.Append(lengt);
                                    double TolerMM = RowMM;
                                    int dti = i % ColumnNumber;
                                    if (isCTX)
                                    {
                                        if (ListCTY.Count > dti)
                                        {
                                            TolerMM = ListCTY[dti];
                                        }
                                    }
                                    if (lengt > TolerMM + ToleranceMM || lengt < TolerMM - ToleranceMM)
                                    {
                                        oneResultOBj.AddImageMassage(rowCent, columnCent, new HTuple(lengt).TupleString(".3") + "C", ColorResult.red);
                                        err++;
                                        //oneCompo.AddNgObj("", Name,r)
                                        oneResultOBj.AddNGOBJ(aoiObj.CiName, "偏移", ho_PinSort, ho_PinSort, this.GetBackNames());
                                    }
                                    else
                                    {
                                        AddGreen(ho_PinSort);
                                        oneResultOBj.AddImageMassage(rowCent, columnCent, new HTuple(lengt).TupleString(".3") + "C");
                                    }
                                }
                                ///Row纵向
                                if (RowNumber > 1 && i < (RowNumber - 1) * ColumnNumber)
                                {
                                    try
                                    {
                                        HOperatorSet.LinePosition(rows.TupleSelect(i), columns.TupleSelect(i), rows.TupleSelect(i + ColumnNumber), columns.TupleSelect(i + ColumnNumber), out rowCent, out columnCent, out lengt, out phi);
                                        HOperatorSet.GenRegionPolygon(out ho_PinSort, new HTuple(rows.TupleSelect(i), rows.TupleSelect(i + ColumnNumber)), new HTuple(columns.TupleSelect(i), columns.TupleSelect(i + ColumnNumber)));
                                        lengt = oneResultOBj.GetCaliConstMM(lengt);
                                        ColMt.Append(lengt);
                                        if (lengt > ColumnMM + ToleranceMM || lengt < ColumnMM - ToleranceMM)
                                        {
                                            oneResultOBj.AddNGOBJ(aoiObj.CiName, "偏移", ho_PinSort, ho_PinSort, this.GetBackNames());
                                            oneResultOBj.AddImageMassage(rowCent, columnCent, new HTuple(lengt).TupleString(".3") + "R", ColorResult.red);
                                            err++;
                                        }
                                        else
                                        {
                                            AddGreen(ho_PinSort);
                                            oneResultOBj.AddImageMassage(rowCent, columnCent, new HTuple(lengt).TupleString(".3") + "R");
                                        }
                                    }
                                    catch (Exception ex) { }
                                }
                            }
                        }
                    }
                    else if (EnveType == 1)
                    {
                        HTuple dintst = oneResultOBj.GetCaliConstPx(ColumnMM) / 4;
                        HTuple dintstR = oneResultOBj.GetCaliConstPx(RowMM) / 4;
                        for (int i = 0; i < MRows.Length; i++)
                        {
                            //HOperatorSet.GenRectangle2(out HObject hObjectT, MRows[i], MColumns[i],  0, dintst, dintst);
                            HOperatorSet.DistancePp(HTuple.TupleGenConst(rows.Length, MRows[i]), HTuple.TupleGenConst(rows.Length, MColumns[i]), rows, columns,
                                out HTuple DINGSE);
                            HTuple DET = oneResultOBj.GetCaliConstMM(DINGSE.TupleMin());
                            HOperatorSet.GenRectangle2(out HObject hObjec, MRows.TupleSelect(i), MColumns.TupleSelect(i), new HTuple(0).TupleRad(), dintstR, dintst);
                            if (DET.TupleMax() > ToleranceMM)
                            {
                                RowMt.Append(-99999.9);
                                HOperatorSet.GenCrossContourXld(out HObject hObjectxt, MRows[i], MColumns[i], 60, 0);
                                oneResultOBj.AddObj(hObjec.ConcatObj(hObjectxt), ColorResult.red);
                                err++;
                            }
                            else
                            {
                                if (aoiObj.DebugID != 0 && aoiObj.DebugID < 6)
                                {
                                    oneResultOBj.AddObj(hObjec);
                                }
                                RowMt.Append(DET);
                                oneResultOBj.AddObj(hObjec);
                            }
                            if (aoiObj.DebugID != 0 && aoiObj.DebugID < 6)
                            {
                                oneResultOBj.AddImageMassage(MRows[i], MColumns[i], DET.TupleString("0.02f"));
                            }
                        }
                    }
                    else
                    {
                        HTuple XaxisRow = new HTuple();
                        HTuple XaxisCol = new HTuple();
                        HTuple YaxisRow = new HTuple();
                        HTuple YaxisCol = new HTuple();
                        int OKtE = 0;
                        if (Dic_Measure.Keys_Measure.ContainsKey(XAxisName))
                        {
                            Dic_Measure.Keys_Measure[XAxisName].color = ColorResult.blue.ToString();
                            if (!Dic_Measure.Keys_Measure[XAxisName].Run(oneResultOBj)) OKtE++;
                            XaxisRow = Dic_Measure.Keys_Measure[XAxisName].OutRows;
                            XaxisCol = Dic_Measure.Keys_Measure[XAxisName].OutCols;
                            if (Dic_Measure.Keys_Measure.ContainsKey(X2AxisName))
                            {
                                Dic_Measure.Keys_Measure[X2AxisName].color = ColorResult.blue.ToString();
                                if (!Dic_Measure.Keys_Measure[X2AxisName].Run(oneResultOBj))
                                {
                                    OKtE++;
                                }
                                else
                                {
                                    HOperatorSet.LinePosition(YaxisRow, YaxisCol,
                                   Dic_Measure.Keys_Measure[X2AxisName].OutRows,
                                   Dic_Measure.Keys_Measure[X2AxisName].OutCols, out HTuple rowc, out HTuple colC,
                                   out HTuple length, out HTuple phic);
                                    XaxisRow = rowc;
                                    XaxisRow = colC;
                                }
                            }
                        }
                        if (Dic_Measure.Keys_Measure.ContainsKey(YAxisName))
                        {
                            Dic_Measure.Keys_Measure[YAxisName].color = ColorResult.yellow.ToString();
                            if (!Dic_Measure.Keys_Measure[YAxisName].Run(oneResultOBj)) OKtE++;
                            YaxisRow = Dic_Measure.Keys_Measure[YAxisName].OutRows;
                            YaxisCol = Dic_Measure.Keys_Measure[YAxisName].OutCols;
                            if (Dic_Measure.Keys_Measure.ContainsKey(Y2AxisName))
                            {
                                Dic_Measure.Keys_Measure[Y2AxisName].color = ColorResult.yellow.ToString();
                                if (!Dic_Measure.Keys_Measure[Y2AxisName].Run(oneResultOBj))
                                {
                                    OKtE++;
                                }
                                else
                                {
                                    HOperatorSet.LinePosition(YaxisRow, YaxisCol,
                                Dic_Measure.Keys_Measure[Y2AxisName].OutRows,
                                Dic_Measure.Keys_Measure[Y2AxisName].OutCols, out HTuple rowc, out HTuple colC,
                                out HTuple length, out HTuple phic);
                                    YaxisRow = rowc;
                                    YaxisCol = colC;
                                }
                            }
                        }
                        if (OKtE == 0)
                        {
                            Vision.Gen_arrow_contour_xld(out HObject yho_Arrow, YaxisRow[0], YaxisCol[0], YaxisRow[1], YaxisCol[1]);
                            Vision.Gen_arrow_contour_xld(out HObject xho_Arrow, XaxisRow[0], XaxisCol[0], XaxisRow[1], XaxisCol[1]);
                            oneResultOBj.AddObj(xho_Arrow, ColorResult.blue);
                            oneResultOBj.AddObj(yho_Arrow, ColorResult.yellow);
                            ModeRow = new HTuple();
                            ModeColumn = new HTuple();
                            oneResultOBj.AddImageMassage(XaxisRow[0], XaxisCol[0], "x");
                            oneResultOBj.AddImageMassage(YaxisRow[0], YaxisCol[0], "y");
                            if (MRows.Length == 0)
                            {
                                MRows = HTuple.TupleGenConst(rows.Length, 0);
                                MColumns = HTuple.TupleGenConst(rows.Length, 0);
                            }
                            for (int i = 0; i < rows.Length; i++)
                            {
                                MeasureMlet.PointToLineExtension(new HTuple(rows.TupleSelect(i), columns.TupleSelect(i)), XaxisRow, XaxisCol, out HTuple minlengt,
                                    out HObject hObject2, out HTuple outRow, out HTuple outCol);
                                minlengt = oneResultOBj.GetCaliConstMM(minlengt);
                                ModeRow.Append(minlengt);
                                HTuple iletM = minlengt - MRows.TupleSelect(i);
                                if (iletM.TupleAbs() < RowMM)
                                {
                                    oneResultOBj.AddObj(hObject2, ColorResult.yellow);
                                    oneResultOBj.AddImageMassage(rows.TupleSelect(i), columns.TupleSelect(i), "Y" + minlengt);
                                }
                                else
                                {
                                    oneResultOBj.AddImageMassage(rows.TupleSelect(i), columns.TupleSelect(i), iletM, ColorResult.red);
                                    err++;
                                    oneResultOBj.AddObj(hObject2, ColorResult.red);
                                }
                                MeasureMlet.PointToLineExtension(new HTuple(rows.TupleSelect(i), columns.TupleSelect(i)), YaxisRow, YaxisCol, out HTuple minlengt1,
                                              out HObject hObject3, out HTuple outRow1, out HTuple outCol1);
                                minlengt1 = oneResultOBj.GetCaliConstMM(minlengt1);
                                ModeColumn.Append(minlengt1);
                                iletM = minlengt1 - MColumns.TupleSelect(i);
                                if (iletM.TupleAbs() < ColumnMM)
                                {
                                    oneResultOBj.AddObj(hObject3, ColorResult.blue);
                                    oneResultOBj.AddImageMassage(rows.TupleSelect(i) + 80, columns.TupleSelect(i), "X" + minlengt1);
                                }
                                else
                                {
                                    oneResultOBj.AddImageMassage(rows.TupleSelect(i) + 80, columns.TupleSelect(i), iletM, ColorResult.red);
                                    err++;
                                    oneResultOBj.AddObj(hObject3, ColorResult.red);
                                }
                            }
                        }
                    }
                    HOperatorSet.GenCrossContourXld(out HObject hObjectx, rows, columns, 50, 0);
                    oneResultOBj.AddObj(hObjectx);
                    if (EnveType != 2)
                    {
                        ModeRow = rows;
                        ModeColumn = columns;
                    }
                    if (aoiObj.DebugID == 5)
                    {
                        return false;
                    }
                    if (RowMt.Length == 0)
                    {
                        RowMt.Append(0);
                    }
                    if (ColMt.Length == 0)
                    {
                        ColMt.Append(0);
                    }
                    oneResultOBj.RowsData.Append(RowMt);
                    oneResultOBj.ColumnsData.Append(ColMt);
                    if (ModeRow.Length > 0)
                    {
                        HOperatorSet.GenRectangle2(out hObjectTE, ModeRow, ModeColumn,
                            HTuple.TupleGenConst(ModeRow.Length, new HTuple(0).TupleRad()), HTuple.TupleGenConst(ModeRow.Length, DifferenceCol
                         ), HTuple.TupleGenConst(ModeRow.Length, DifferenceRow));
                    }
                }
                catch (Exception ex)
                {
                    err++;
                }
                if (aoiObj.DebugID == 7)
                {
                    oneResultOBj.AddObj(hObjectTE);
                    return false;
                }
                if (err == 0)
                {
                    isOk = true;
                }
                else
                {
                    oneResultOBj.AddMeassge("间距");
                }
                if (aoiObj.DebugID != 0 && aoiObj.DebugID < 8)
                {
                    return false;
                }
                HObject imagetd = GetEmset(oneResultOBj.Image);
                HOperatorSet.Difference(this.AOIObj, this.hObjectTE, out HObject hObject1);
                if (aoiObj.DebugID != 0 && aoiObj.DebugID > 7)
                {
                    oneResultOBj.AddObj(hObject1, ColorResult.blue);
                }
                HOperatorSet.ReduceDomain(imagetd, hObject1, out imagetd);
                DeFilt(oneResultOBj, imagetd, aoiObj.DebugID);
                HOperatorSet.FreiAmp(imagetd, out hObject1);
                //HOperatorSet.ReduceDomain(hObject1, DrawObj, out ho_PinSort);
                istBool = DeFilt2(oneResultOBj, oneResultOBj, hObject1, aoiObj.DebugID);
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine(ex.Message);
            }

            if (!istBool)
            {
                oneResultOBj.AddMeassge("异物");
            }

            if (istBool && isOk)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private HObject hObjecttD;

        /// <summary>
        /// 检测灰度二值化
        /// </summary>
        /// <param name="halcon"></param>
        /// <param name="imageT"></param>
        /// <param name="runid"></param>
        /// <returns></returns>
        public bool DeFilt(OneResultOBj halcon, HObject imageT, int runid = 0)
        {
            try
            {
                HObject hObject = new HObject();

                HOperatorSet.Threshold(imageT, out hObject, ThrMin2, ThrMax2);
                HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple row, out HTuple column);
                if (runid == 8)
                {
                    halcon.AddImageMassage(row, column, area);
                    halcon.AddObj(hObject);
                    return false;
                }
                HOperatorSet.Threshold(imageT, out HObject hObject3, ThrMinA, ThrMaxA);
                if (runid == 9)
                {
                    HOperatorSet.AreaCenter(hObject3, out area, out row, out column);
                    halcon.AddImageMassage(row, column, area);
                    halcon.AddObj(hObject3);
                    return false;
                }
                HOperatorSet.ConcatObj(hObject, hObject3, out hObject);
                HOperatorSet.Connection(hObject, out hObject);
                HOperatorSet.SelectShape(hObject, out hObject, new HTuple("width", "height"), "and", new HTuple(1, 1), new HTuple(999999999, 999999999));
                HOperatorSet.AreaCenter(hObject, out area, out row, out column);
                hObjecttD = hObject;
                if (hObject.CountObj() != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 检测边缘幅度
        /// </summary>
        /// <param name="halcon"></param>
        /// <param name="imageT"></param>
        /// <param name="runid"></param>
        /// <returns></returns>
        public bool DeFilt2(OneResultOBj halcon, OneResultOBj oneResultOBj, HObject imageT, int runid = 0)
        {
            try
            {
                if (runid != 0 && runid <= 9)
                {
                    return false;
                }
                HOperatorSet.Threshold(imageT, out HObject hObject, ThrMin3, ThrMax3);
                HOperatorSet.ClosingCircle(hObject, out hObject, 8);
                if (runid == 10)
                {
                    halcon.AddObj(hObject);
                    return false;
                }
                HOperatorSet.Union2(hObjecttD, hObject, out hObject);
                HOperatorSet.Union1(hObject, out hObject);
                HOperatorSet.Connection(hObject, out hObject);
                HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple row, out HTuple column);
                for (int i = 0; i < SelectShapeType.Length; i++)
                {
                    HOperatorSet.SelectShape(hObject, out hObject, SelectShapeType.TupleSelect(i), "and", SelectShapeMin.TupleSelect(i), SelectShapeMax.TupleSelect(i));
                    if (runid == 11 + i)
                    {
                        halcon.AddObj(hObject);
                        return false;
                    }
                }
                HOperatorSet.Union1(hObject, out hObject);
                HOperatorSet.ClosingCircle(hObject, out hObject, 100);
                HOperatorSet.Connection(hObject, out hObject);
                HOperatorSet.AreaCenter(hObject, out area, out row, out column);
                HOperatorSet.SmallestCircle(hObject, out row, out column, out HTuple radius);
                if (hObject.CountObj() != 0)
                {
                    HOperatorSet.GenCircle(out HObject circle, row, column, radius * 2);
                    oneResultOBj.AddNGOBJ(this.CRDName, "异物", circle, hObject, this.GetBackNames());
                    //ADDRed(hObject);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public HObject PintOBj { get; set; } = new HObject();

        private HObject hObjectTE;

        public void DarwRenge2(HObject ho_Image, HTuple hv_WindowHandle)
        {
            HObject ho_Regions;
            HTuple hv_RowA = null, hv_ColumnA = null, hv_Phi = null;
            HTuple hv_Length1, length2;
            try
            {
                if (rowRenge1 == null)
                {
                    rowRenge1 = new HTuple();
                }
                HOperatorSet.SetDraw(hv_WindowHandle, "margin");
                HOperatorSet.SetLineWidth(hv_WindowHandle, 2);
                HOperatorSet.SetColor(hv_WindowHandle, "green");
                HOperatorSet.DispObj(ho_Image, hv_WindowHandle);
                if (rowRenge1.Length == 1)
                {
                    HOperatorSet.DrawRectangle2Mod(hv_WindowHandle, rowRenge1[0], ColumnRenge2[0], PhiRenge2[0], Lengt1Renge2[0], Lengt2Renge2[0],
                        out hv_RowA, out hv_ColumnA, out hv_Phi,
                   out hv_Length1, out length2);
                }
                else
                {
                    HOperatorSet.DrawRectangle2(hv_WindowHandle, out hv_RowA, out hv_ColumnA, out hv_Phi,
                   out hv_Length1, out length2);
                }
                rowRenge1 = hv_RowA;
                ColumnRenge2 = hv_ColumnA;
                PhiRenge2 = hv_Phi;
                Lengt1Renge2 = hv_Length1;
                Lengt2Renge2 = length2;
                HOperatorSet.GenRectangle2(out ho_Regions, hv_RowA, hv_ColumnA, hv_Phi, hv_Length1, length2);
                HOperatorSet.DispObj(ho_Regions, hv_WindowHandle);
                return;
            }
            catch (Exception exception)
            {
            }
        }

        public void ShowReing(HTuple HalconWiid)
        {
            HObject hObject;
            HOperatorSet.GenRectangle2(out hObject, rowRenge1, ColumnRenge2, PhiRenge2, Lengt1Renge2, Lengt2Renge2);

            HOperatorSet.DispObj(hObject, HalconWiid);
        }

        public bool CheckPin(OneResultOBj halcon, HObject imageT, out HTuple rows, out HTuple columns, int runID = 0)
        {
            bool OKt = false;
            rows = new HTuple();
            columns = new HTuple();
            HObject ho_PinSort = null, ho_PinHave, ho_PinNo, ho_Line;
            HTuple hv_PinNum = new HTuple(), hv_ConstCol = new HTuple();
            HTuple hv_ConstRow = new HTuple(), hv_ConstRC = new HTuple();
            HTuple hv_CaliConst = new HTuple(), hv_i = new HTuple();
            HTuple hv_Area4 = new HTuple(), hv_Row4 = new HTuple();
            HTuple hv_Column4 = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_PinHave);
            HOperatorSet.GenEmptyObj(out ho_PinNo);
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_PinSort);
            //Pin好坏
            HTuple area;
            try
            {
                HOperatorSet.Threshold(imageT, out ho_PinHave, ThrMin, ThrMax);
                HOperatorSet.ClosingRectangle1(ho_PinHave, out ho_PinHave, 1, 10);
                HOperatorSet.Connection(ho_PinHave, out ho_PinHave);
                HOperatorSet.ClosingCircle(ho_PinHave, out ho_PinHave, 15);
                if (runID == 1)
                {
                    halcon.AddObj(ho_PinHave);
                    return false;
                }

                HOperatorSet.Connection(ho_PinHave, out ho_PinHave);
                HOperatorSet.AreaCenter(ho_PinHave, out area, out rows, out columns);
                HOperatorSet.SelectShape(ho_PinHave, out ho_PinNo, "area", "and", Min[0], Max[0]);
                if (runID == 2)
                {
                    HOperatorSet.SortRegion(ho_PinNo, out ho_PinNo, "character", "true", "row");
                    halcon.AddObj(ho_PinNo);
                    HOperatorSet.AreaCenter(ho_PinNo, out area, out rows, out columns);
                    for (int i = 0; i < rows.Length; i++)
                    {
                        halcon.AddImageMassage(rows[i], columns[i], (i + 1));
                    }
                    return false;
                }
                if (!IsHtoB)
                {
                    HOperatorSet.SelectShape(ho_PinNo, out ho_PinNo, "radius", "and", Min[1], Max[1]);
                    if (runID == 3)
                    {
                        HOperatorSet.SortRegion(ho_PinNo, out ho_PinNo, "character", "true", "row");
                        halcon.AddObj(ho_PinNo);
                        for (int i = 1; i < ho_PinNo.CountObj() + 1; i++)
                        {
                            HOperatorSet.AreaCenter(ho_PinNo.SelectObj(i), out area, out rows, out columns);
                            halcon.AddImageMassage(rows, columns, i);
                        }
                        return false;
                    }
                }
                HOperatorSet.AreaCenter(ho_PinNo, out area, out rows, out columns);
                if (rows.Length < ColumnNumber * RowNumber)
                {
                    halcon.AddObj(ho_PinNo, ColorResult.blue);
                    halcon.AddMeassge("未找到足够的区域3:数量" + rows.Length);
                }
                if (!IsHtoB)
                {
                    HOperatorSet.GenRectangle2(out ho_PinHave, rows, columns, HTuple.TupleGenConst(rows.Length, 0), HTuple.TupleGenConst(rows.Length, PinHeightExternal), HTuple.TupleGenConst(rows.Length, PinHeightExternal));
                    HOperatorSet.Difference(ho_PinHave, ho_PinNo, out ho_PinHave);
                    HOperatorSet.Union1(ho_PinHave, out ho_PinHave);
                    if (runID >= 4)
                    {
                        halcon.AddObj(ho_PinHave);
                        HOperatorSet.AreaCenter(ho_PinNo, out area, out rows, out columns);
                        halcon.AddImageMassage(rows + 10, columns, "面积" + area);
                        HOperatorSet.InnerCircle(ho_PinNo, out rows, out columns, out area);
                        HOperatorSet.GenCircle(out ho_PinNo, rows, columns, area);
                        halcon.AddObj(ho_PinNo, ColorResult.red);
                        halcon.AddImageMassage(rows + 40, columns, "内半径" + area);
                        if (runID == 4)
                        {
                            return false;
                        }
                    }
                    HOperatorSet.ReduceDomain(imageT, ho_PinHave, out ho_PinSort);
                    HOperatorSet.Threshold(ho_PinSort, out ho_PinHave, ThrMinExternal, ThrMaxExternal);
                    HOperatorSet.Connection(ho_PinHave, out ho_PinHave);
                    if (OpeningValue > 0)
                    {
                        HOperatorSet.OpeningCircle(ho_PinHave, out ho_PinHave, OpeningValue);
                    }
                    if (runID == 5)
                    {
                        halcon.AddObj(ho_PinHave, ColorResult.blue);
                        return false;
                    }
                    HOperatorSet.SelectShape(ho_PinHave, out ho_PinHave, "area", "and", PinHeightExternal * 2 * PinHeightExternal * 2 * PinHeightExternalPercent / 100, 9999999);
                    HOperatorSet.AreaCenter(ho_PinHave, out area, out rows, out columns);
                    HOperatorSet.ClosingCircle(ho_PinHave, out ho_PinHave, 5);
                    HOperatorSet.FillUp(ho_PinHave, out ho_PinHave);
                    HOperatorSet.Union1(ho_PinHave, out ho_PinHave);
                    HOperatorSet.Connection(ho_PinHave, out ho_PinHave);
                    if (runID == 6)
                    {
                        halcon.AddObj(ho_PinHave);
                        return false;
                    }
                }
                else
                {
                    HOperatorSet.FillUpShape(ho_PinNo, out ho_PinHave, "area", 1, 209);
                    HOperatorSet.OpeningCircle(ho_PinHave, out ho_PinHave, 10);
                    //HOperatorSet.ClosingCircle(ho_PinHave, out ho_PinHave, 50);
                    HOperatorSet.Union1(ho_PinHave, out ho_PinHave);
                    HOperatorSet.Connection(ho_PinHave, out ho_PinHave);
                    HOperatorSet.SelectShape(ho_PinHave, out ho_PinHave, "area", "and", Min[0], Max[0]);
                    HOperatorSet.AreaCenter(ho_PinHave, out area, out rows, out columns);

                    if (Rows == null)
                    {
                        Rows = new HTuple();
                        Columns = new HTuple();
                    }
                    if (Rows.Length != 0)
                    {
                        HOperatorSet.GenRectangle2(out HObject hObject2, Rows, Columns, HTuple.TupleGenConst(Rows.Length, 0), HTuple.TupleGenConst(Rows.Length, PinHeightExternal), HTuple.TupleGenConst(Rows.Length, PinHeightExternal));
                        HOperatorSet.Intersection(ho_PinHave, hObject2, out ho_PinHave);
                        if (runID == 4)
                        {
                            halcon.AddObj(hObject2, ColorResult.orange);
                        }
                    }
                    HOperatorSet.Connection(ho_PinHave, out ho_PinHave);
                    HOperatorSet.SelectShape(ho_PinHave, out ho_PinHave, "area", "and", Min[0], Max[0]);
                    HOperatorSet.Union1(ho_PinHave, out ho_PinHave);
                    HOperatorSet.ClosingCircle(ho_PinHave, out ho_PinHave, 5);
                    if (runID == 4)
                    {
                        halcon.AddObj(ho_PinHave, ColorResult.red);
                    }
                    HOperatorSet.Connection(ho_PinHave, out ho_PinHave);
                    HOperatorSet.AreaCenter(ho_PinHave, out area, out rows, out columns);
                    HOperatorSet.InnerRectangle1(ho_PinHave, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                    HOperatorSet.GenRectangle1(out ho_PinHave, row1, col1, row2, col2);
                    HOperatorSet.AreaCenter(ho_PinHave, out area, out row1, out col1);

                    HOperatorSet.GenRectangle2(out ho_PinHave, row1, col1, HTuple.TupleGenConst(row1.Length, 0), HTuple.TupleGenConst(row1.Length, PintWrt), HTuple.TupleGenConst(row1.Length, 60));
                    if (runID >= 4)
                    {
                        halcon.AddObj(ho_PinHave, ColorResult.orange);
                    }

                    HOperatorSet.Union1(ho_PinHave, out ho_PinHave);
                    HOperatorSet.ReduceDomain(imageT, ho_PinHave, out ho_PinSort);
                    HOperatorSet.Threshold(ho_PinSort, out ho_PinHave, ThrMin, ThrMax);

                    HOperatorSet.FillUp(ho_PinHave, out ho_PinHave);
                    HOperatorSet.ClosingCircle(ho_PinHave, out ho_PinHave, 10);
                    HOperatorSet.Connection(ho_PinHave, out ho_PinHave);
                    HOperatorSet.SelectShape(ho_PinHave, out ho_PinHave, "area", "and", Min[0], Max[0]);

                    if (ErosionWaightValue < 0)
                    {
                        HOperatorSet.ErosionRectangle1(ho_PinHave, out ho_PinHave, Math.Abs(ErosionHeightValue), Math.Abs(ErosionWaightValue));
                    }
                    else
                    {
                        if (ErosionWaightValue > 0 || ErosionHeightValue > 0)
                        {
                            HOperatorSet.DilationRectangle1(ho_PinHave, out ho_PinHave, ErosionWaightValue, ErosionHeightValue);
                        }
                    }
                    HOperatorSet.ClosingCircle(ho_PinHave, out ho_PinHave, 10);

                    HOperatorSet.FillUp(ho_PinHave, out ho_PinHave);
                    if (runID == 4)
                    {
                        halcon.AddObj(ho_PinHave, ColorResult.pink);
                        return false;
                    }
                }
                if (!this.PintOBj.IsInitialized())
                {
                    this.PintOBj.GenEmptyObj();
                }
                HOperatorSet.Difference(ho_PinHave, PintOBj, out ho_PinHave);
                HObject hObject1 = new HObject();
                hObject1.GenEmptyObj();
                int d = ho_PinHave.CountObj();

                if (d < ColumnNumber * RowNumber)
                {
                    halcon.AddObj(ho_PinNo, ColorResult.blue);
                    halcon.AddMeassge("未找到足够的区域6：数量" + d);
                }
                HObject hObject = new HObject();
                for (int i = 0; i < ho_PinHave.CountObj(); i++)
                {
                    HOperatorSet.ReduceDomain(imageT, ho_PinHave.SelectObj(i + 1), out ho_PinSort);

                    if (!IsHtoB)
                    {
                        HOperatorSet.Threshold(ho_PinSort, out hObject, ThrMin, ThrMax);
                        if (CloseCirt != 0)
                        {
                            HOperatorSet.ClosingCircle(hObject, out hObject, CloseCirt);
                        }
                        HOperatorSet.Connection(hObject, out hObject);
                        HOperatorSet.SelectShape(hObject, out hObject, "area", "and", Min[0], Max[0]);
                        HOperatorSet.SelectShape(hObject, out hObject, "radius", "and", Min[1], Max[1]);
                        HOperatorSet.AreaCenter(hObject, out area, out rows, out columns);
                        if (area.Length > 1)
                        {
                            HOperatorSet.SelectShape(hObject, out hObject, "area", "and", area.TupleMax() - 1, area.TupleMax() + 1);
                        }
                        hObject1 = hObject1.ConcatObj(hObject);
                    }
                    else
                    {
                        HOperatorSet.Threshold(ho_PinSort, out hObject, ThrMinExternal, ThrMaxExternal);
                        if (CloseCirt != 0)
                        {
                            HOperatorSet.ClosingCircle(hObject, out hObject, CloseCirt);
                        }
                        HOperatorSet.Intensity(hObject, ho_PinSort, out HTuple mean, out HTuple deviation);
                        if (!SelsMax)
                        {
                            if (mean[0] == 0.0)
                            {
                            }
                            HOperatorSet.Threshold(ho_PinSort, out hObject, mean.TupleMax() - 1, ThrMaxExternal);
                            if (runID != 0)
                            {
                                HOperatorSet.AreaCenter(ho_PinHave.SelectObj(i + 1), out area, out rows, out columns);
                                halcon.AddImageMassage(rows + 100, columns, (i + 1), ColorResult.red);
                                halcon.AddObj(hObject, ColorResult.blue);
                            }
                        }
                        HOperatorSet.Connection(hObject, out hObject);
                        if (hObject.CountObj() > 1)
                        {
                            HOperatorSet.SelectShape(hObject, out hObject, "area", "and", 20, 99999999);
                            HOperatorSet.OpeningCircle(hObject, out hObject, 3);
                        }
                        if (!SelsMax)
                        {
                            HOperatorSet.Intensity(hObject, ho_PinSort, out HTuple mean2, out deviation);
                            if (mean2.Length > 1)
                            {
                                if (runID != 0)
                                {
                                    HOperatorSet.AreaCenter(hObject, out area, out rows, out columns);
                                    halcon.AddImageMassage(rows, columns, mean2, ColorResult.red);
                                    halcon.AddObj(hObject, ColorResult.red);
                                }
                                if (mean2[0] != 0.0)
                                {
                                    hObject = hObject.SelectObj(mean2.TupleFind(mean2.TupleMax()) + 1);
                                }
                            }
                        }
                        HOperatorSet.Connection(hObject, out hObject);
                        HOperatorSet.AreaCenter(hObject, out area, out rows, out columns);
                        HOperatorSet.AreaCenter(ho_PinHave.SelectObj(i + 1), out area, out HTuple rows2, out HTuple columns2);
                        if (runID != 0)
                        {
                            halcon.AddObj(ho_PinHave.SelectObj(i + 1), ColorResult.blue);
                            HOperatorSet.GenCrossContourXld(out HObject hObject2, rows, columns, 50, 0);
                            halcon.AddObj(hObject2, ColorResult.red);
                            halcon.AddObj(hObject, ColorResult.red);
                        }
                        if (rows.Length != 0)
                        {
                            HOperatorSet.DistancePp(HTuple.TupleGenConst(rows.Length, rows2), HTuple.TupleGenConst(rows.Length, columns2), rows, columns, out HTuple dingt);
                            if (dingt.Length > 1)
                            {
                                hObject = hObject.SelectObj(dingt.TupleFind(dingt.TupleMin()) + 1);
                            }
                            HOperatorSet.AreaCenter(hObject, out area, out rows, out columns);
                            if (area.Length > 1)
                            {
                                HOperatorSet.SelectShape(hObject, out hObject, "area", "and", area.TupleMax() - 1, area.TupleMax() + 1);
                            }
                            hObject1 = hObject1.ConcatObj(hObject);
                        }
                        else
                        {
                        }
                    }

                    halcon.AddObj(hObject1, ColorResult.yellow);
                }
                if (runID == 5)
                {
                    HOperatorSet.AreaCenter(ho_PinHave, out area, out rows, out columns);
                    HOperatorSet.GenCrossContourXld(out HObject hObject2, rows, columns, 50, 0);
                    halcon.AddObj(hObject2, ColorResult.yellow);
                    halcon.AddObj(hObject1, ColorResult.yellow);
                    return false;
                }

                ho_PinHave = hObject1;
                HOperatorSet.AreaCenter(ho_PinHave, out area, out rows, out columns);
                columns = columns.TupleRemove(rows.TupleFind(0));
                rows = rows.TupleRemove(rows.TupleFind(0));
                nGRoi = nGRoi.ConcatObj(ho_PinHave);
            }
            catch (HalconException HDevExpDefaultException)
            {
                Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(HDevExpDefaultException.Message);
            }
            return OKt;
        }
    }
}