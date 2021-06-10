using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.Controls;
using static Vision2.vision.Vision;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    /// <summary>
    /// 焊线
    /// </summary>
    public class Wire_Solder : RunProgram
    {
        public List<Wire_S> listWelding = new List<Wire_S>();

        public Wire_Solder()
        {
            thresholdV_2.Max = 255;
            thresholdV_2.Min = 200;
        }
        public override string ShowHelpText()
        {
            return "2.5焊线检测";
        }
        public override Control GetControl()
        {
            return new Wire_Solder_Control(this);
        }

        public override RunProgram UpSatrt<T>(string PATH)
        {
            return base.ReadThis<Wire_Solder>(PATH);
        }

        public override bool RunHProgram(HalconRun halcon, OneResultOBj oneResultOBj, int runID = 0,string name=null)
        {
            return RunP(halcon, runID);
        }

        public Threshold_Min_Max thresholdV_2 { get; set; } = new Threshold_Min_Max();


        public bool RunP(HalconRun halcon, int runID = 0, HTuple visionUserControlH = null, HTuple visionUserControlS = null,
            HTuple visionUserControlV = null, HTuple hwindRgb = null)
        {
            int err = 0;
            try
            {
                ResltBool = true;

                for (int i = 0; i < listWelding.Count; i++)
                {
                    HOperatorSet.AreaCenter(listWelding[i].HObject, out HTuple area, out HTuple rows, out HTuple colus);
                    HTuple datStr = new HTuple();
                    for (int j = 0; j < listWelding[i].List3DName.Count; j++)
                    {
                        if (listWelding[i].List3DName[j] >= 0)
                        {
                            //halcon.GetOneImageR().AddData(Project.formula.RecipeCompiler.Instance.Data.GetMaxMinValue(listWelding[i].List3DName[j]));
                            if (!Project.formula.RecipeCompiler.Instance.Data.GetChet(listWelding[i].List3DName[j]))
                            {
                                datStr.Append(Project.formula.RecipeCompiler.Instance.Data.ListDatV[listWelding[i].List3DName[j]].ComponentName);
                                halcon.AddOBJ(listWelding[i].HObject, ColorResult.red);
                                err++;
                            }
                        }
                    }
                    if (datStr.Length != 0)
                    {
                        halcon.AddMessageIamge(rows + 60, colus, datStr, ColorResult.red);
                    }
                }
                HOperatorSet.ReduceDomain(halcon.Image(), DrawObj, out HObject hObject);
                HOperatorSet.Decompose3(hObject, out HObject image1, out HObject image2, out HObject image3);
                HOperatorSet.TransFromRgb(image1, image2, image3, out image1, out image2, out image3, "hsv");

                HOperatorSet.Threshold(image1, out HObject hObjectH, H_threshold_min, H_threshold_max);

                HOperatorSet.Threshold(image2, out HObject hObjectS, S_threshold_min, S_threshold_max);
                HOperatorSet.Threshold(image3, out HObject hObjectV, V_threshold_min, V_threshold_max);

                HObject hObjectV2 = thresholdV_2.Threshold(image3);


                HOperatorSet.Union2(hObjectV, hObjectV2, out hObjectV);

                HOperatorSet.Threshold(image2, out HObject hObjectS2, S_threshold_min2, S_threshold_max2);
                HOperatorSet.Threshold(image1, out HObject hObjectH2, H_threshold_min2, H_threshold_max2);
                if (runID != 0)
                {
                    HOperatorSet.DispObj(image1, visionUserControlH);
                    HOperatorSet.DispObj(image2, visionUserControlS);
                    HOperatorSet.DispObj(image3, visionUserControlV);
                    HOperatorSet.DispObj(hObject, hwindRgb);
                }
                HOperatorSet.Intersection(hObjectV, hObjectS, out HObject hObject1);
                HOperatorSet.Intersection(hObject1, hObjectH, out hObject1);
                HObject itmesOBJ = new HObject();
                itmesOBJ.GenEmptyObj();

                for (int i = 0; i < listWelding.Count; i++)
                {
                    //halcon.AddOBJ(listWelding[i].HObject, ColorResult.green);
                    itmesOBJ = itmesOBJ.ConcatObj(listWelding[i].HObject);
                }

                HOperatorSet.Intersection(hObjectH2, hObjectS2, out HObject hObject3);
                if (runID == 1)
                {
                    HOperatorSet.DispObj(hObjectH, visionUserControlH);
                    HOperatorSet.DispObj(hObjectS, visionUserControlS);
                    HOperatorSet.DispObj(hObjectV, visionUserControlV);
                    HOperatorSet.DispObj(hObject3, hwindRgb);
                }
                //HOperatorSet.ClosingCircle(hObject3, out hObject3,1);
                HOperatorSet.Difference(hObject3, itmesOBJ, out hObject3);
                HOperatorSet.Difference(hObject1, hObject3, out hObject1);
                HOperatorSet.FillUp(hObject1, out hObject1);
                HOperatorSet.ClosingCircle(hObject1, out hObject1, ClosingCircle);

                HOperatorSet.FillUpShape(hObject1, out hObject1, "area", 0, 9999);
                HOperatorSet.ErosionCircle(hObject1, out hObject1, ErosionCircle);
                //HOperatorSet.OpeningCircle(hObject1, out hObject1, ClosingCircle);
                HOperatorSet.Connection(hObject1, out hObject1);

                HOperatorSet.SelectShape(hObject1, out hObject1, "area", "and", Select_shap_min, Select_shap_max);
                HOperatorSet.ClosingCircle(hObject1, out hObject1, 20);
                HOperatorSet.FillUpShape(hObject1, out hObject1, "area", 0, 9999);
                //HOperatorSet.OpeningCircle(hObject1, out hObject1, 20);

                if (runID == 2)
                {
                    HOperatorSet.DispObj(hObjectH2, visionUserControlH);
                    HOperatorSet.DispObj(hObjectS2, visionUserControlS);
                    HOperatorSet.DispObj(hObjectV, visionUserControlV);
                    HOperatorSet.DispObj(hObject3, hwindRgb);
                }
                HOperatorSet.SmallestRectangle2(hObject1, out HTuple row, out HTuple column, out HTuple phi, out HTuple length1, out HTuple length2);

                HTuple lengt2MaxGre = length2.TupleGreaterElem(MaxLength2);

                HOperatorSet.GenRectangle2(out HObject hObject2, row, column, phi, length1, length2);

                for (int i = 0; i < listWelding.Count; i++)
                {
                    HOperatorSet.AreaCenter(listWelding[i].HObject, out HTuple area, out HTuple rows1, out HTuple columns1);
                    HOperatorSet.GetRegionIndex(hObject2, rows1.TupleInt(), columns1.TupleInt(), out HTuple index);
                    if (index.Length == 0)
                    {
                        break;
                    }
                    HOperatorSet.SelectObj(hObject2, out HObject obj, index);
                    HOperatorSet.SmallestRectangle2(obj, out HTuple rowst1, out HTuple columnst1, out HTuple phist1, out HTuple length1st1, out HTuple length2st1);
                    Vision.gen_rectangle2_line_point(rowst1, columnst1, phist1, length1st1.TupleMult(2).TupleDiv(1.3), out HTuple rowsT1, out HTuple columnsT1, out HTuple rowsT2, out HTuple columnsT2);
                    Vision.gen_rectangle2_line_point(rowst1, columnst1, phist1, length1st1.TupleMult(2).TupleDiv(4), out HTuple rowsT12, out HTuple columnsT12, out HTuple rowsT22, out HTuple columnsT22);
                    HOperatorSet.GenRectangle2(out HObject hObject51, rowsT1, columnsT1, phist1, length1st1.TupleDiv(4), length2st1 + 5);
                    HOperatorSet.GenRectangle2(out HObject hObject52, rowsT2, columnsT2, phist1, length1st1.TupleDiv(4), length2st1 + 5);
                    HOperatorSet.GenRectangle2(out HObject hObject53, rowsT12, columnsT12, phist1, length1st1.TupleDiv(4), length2st1 + 5);
                    HOperatorSet.GenRectangle2(out HObject hObject54, rowsT22, columnsT22, phist1, length1st1.TupleDiv(4), length2st1 + 5);
                    HOperatorSet.Intersection(hObject51, hObject1, out hObject51);
                    HOperatorSet.Connection(hObject51, out hObject51);
                    HOperatorSet.AreaCenter(hObject51, out HTuple area2, out HTuple row35, out HTuple hTuple2);
                    HOperatorSet.SelectShape(hObject51, out hObject51, "area", "and", area2.TupleMax().TupleSub(1), 9999999999);
                    HOperatorSet.SmallestRectangle2(hObject51, out HTuple row1_s, out HTuple column1_s, out HTuple phi1_s, out HTuple length1_s, out HTuple length2_s);
                    if (runID > 2)
                    {
                        halcon.AddMessageIamge(row1_s, column1_s, "1宽:" + (length2_s * 2).TupleString("0.1f"));
                    }

                    HOperatorSet.GenRectangle2(out hObject51, row1_s, column1_s, phi1_s, length1_s, length2_s);
                    //halcon.AddOBJ(hObject51, ColorResult.Red);


                    HOperatorSet.Intersection(hObject52, hObject1, out hObject52);
                    HOperatorSet.Connection(hObject52, out hObject52);
                    HOperatorSet.AreaCenter(hObject52, out area, out row35, out hTuple2);
                    HOperatorSet.SelectShape(hObject52, out hObject52, "area", "and", area.TupleMax().TupleSub(1), 9999999999);
                    HOperatorSet.SmallestRectangle2(hObject52, out row1_s, out column1_s, out phi1_s, out length1_s, out length2_s);
                    if (runID > 2)
                    {
                        halcon.AddMessageIamge(row1_s, column1_s, "2宽:" + (length2_s * 2).TupleString("0.1f"));
                    }
                    HOperatorSet.GenRectangle2(out hObject51, row1_s, column1_s, phi1_s, length1_s, length2_s);
                    //halcon.AddOBJ(hObject51, ColorResult.Red);

                    HOperatorSet.Intersection(hObject53, hObject1, out hObject53);
                    HOperatorSet.Connection(hObject53, out hObject53);
                    HOperatorSet.AreaCenter(hObject53, out area, out row35, out hTuple2);
                    HOperatorSet.SelectShape(hObject53, out hObject53, "area", "and", area.TupleMax().TupleSub(1), 9999999999);
                    HOperatorSet.SmallestRectangle2(hObject53, out row1_s, out column1_s, out phi1_s, out length1_s, out length2_s);
                    if (runID > 2)
                    {

                        halcon.AddMessageIamge(row1_s, column1_s, "3宽:" + (length2_s * 2).TupleString("0.1f"));
                    }
                    HOperatorSet.GenRectangle2(out hObject51, row1_s, column1_s, phi1_s, length1_s, length2_s);
                    //halcon.AddOBJ(hObject51, ColorResult.Red);
                    HOperatorSet.Intersection(hObject54, hObject1, out hObject54);
                    HOperatorSet.Connection(hObject54, out hObject54);
                    HOperatorSet.AreaCenter(hObject54, out area, out row35, out hTuple2);
                    HOperatorSet.SelectShape(hObject54, out hObject54, "area", "and", area.TupleMax().TupleSub(1), 9999999999);

                    HOperatorSet.SmallestRectangle2(hObject54, out row1_s, out column1_s, out phi1_s, out length1_s, out length2_s);
                    if (runID > 2)
                    {
                        halcon.AddMessageIamge(row1_s, column1_s, "4宽:" + (length2_s * 2).TupleString("0.1f"));
                    }
                    HOperatorSet.GenRectangle2(out hObject51, row1_s, column1_s, phi1_s, length1_s, length2_s);
                    //halcon.AddOBJ(hObject51, ColorResult.Red);
                }

                for (int i = 0; i < lengt2MaxGre.Length; i++)
                {
                    HObject seleOBJ = hObject1.SelectObj(i + 1);
                    HObject seleOBJ2 = hObject2.SelectObj(i + 1);
                    HOperatorSet.SmallestRectangle2(seleOBJ, out HTuple row4, out HTuple column4, out HTuple phi4, out HTuple length14, out HTuple length24);
                    HOperatorSet.GenRectangle2(out seleOBJ, row4, column4, phi4, length14, length24);
                    HTuple cont = 0;
                    HTuple indexs = new HTuple();
                    for (int i2 = 0; i2 < listWelding.Count; i2++)
                    {
                        HOperatorSet.AreaCenter(listWelding[i2].HObject, out HTuple area, out HTuple rows1, out HTuple columns1);

                        HOperatorSet.GetRegionIndex(seleOBJ, rows1.TupleInt(), columns1.TupleInt(), out HTuple index);
                        if (index.Length > 0)
                        {
                            cont++;
                            indexs.Append(i2);
                        }
                    }
                    if (indexs.Length > 1)
                    {
                        HTuple rowstCi = new HTuple();
                        HTuple ColumnstCi = new HTuple();
                        for (int i2 = 0; i2 < indexs.Length; i2++)
                        {
                            HOperatorSet.AreaCenter(listWelding[indexs[i2]].HObject, out HTuple area, out HTuple rows1, out HTuple columns1);
                            HOperatorSet.GenCircle(out HObject circle, rows1, columns1, 30);
                            halcon.AddOBJ(circle, ColorResult.red);
                            halcon.AddMessageIamge(rows1, columns1, "short circuit", ColorResult.red);
                            rowstCi.Append(rows1);
                            err++;
                            ColumnstCi.Append(columns1);
                        }
                        HOperatorSet.GenContourPolygonXld(out HObject hObject4, rowstCi, ColumnstCi);
                        halcon.AddOBJ(hObject4, ColorResult.red);
                        halcon.AddOBJ(hObject1.SelectObj(i + 1), ColorResult.yellow);
                    }
                    else if (indexs.Length == 1)
                    {
                        HOperatorSet.SmallestRectangle2(listWelding[indexs].HObject, out HTuple rowR, out HTuple columnR, out HTuple phiR, out HTuple length1R, out HTuple length2R);
                        HOperatorSet.SmallestRectangle2(seleOBJ, out HTuple rowR2, out HTuple columnR2, out HTuple phiR2, out HTuple length1R2, out HTuple length2R2);
                        HOperatorSet.AreaCenter(listWelding[indexs].HObject, out HTuple area, out HTuple rows1, out HTuple columns1);
                        double phidoub = (phiR.TupleDeg().TupleAbs() - phiR2.TupleDeg().TupleAbs()).TupleAbs();

                        if (phidoub > MaxDeg)
                        {
                            ResltBool = false;
                            halcon.AddMessageIamge(rows1, columns1, "deg:" + phidoub.ToString("0.00°"), ColorResult.red);
                        }
                        else if (runID > 2)
                        {
                            halcon.AddMessageIamge(rows1, columns1, "deg:" + phidoub.ToString("0.00°"));
                        }
                        if (length2R2 * 2 >= MaxLength2)
                        {
                            ResltBool = false;
                            halcon.AddMessageIamge(rows1 + 50, columns1 + 40, "宽:" + (length2R2 * 2), ColorResult.red);
                        }
                        else if (runID > 2)
                        {
                            halcon.AddMessageIamge(rows1 + 50, columns1 + 40, "宽:" + (length2R2 * 2));
                        }

                        if (length2R2 * 2 >= MaxLength2 || phidoub > MaxDeg)
                        {
                            ResltBool = false;
                            halcon.AddOBJ(seleOBJ2, ColorResult.red);
                        }
                        else if (runID > 2)
                        {
                            halcon.AddOBJ(seleOBJ2, ColorResult.blue);
                        }
                        halcon.AddOBJ(hObject1.SelectObj(i + 1), ColorResult.yellow);
                    }
                    else
                    {
                        //halcon.AddMessage("空错误");
                    }
                }

            }
            catch (Exception ex)
            {
                halcon.AddMessage(ex.Message);
            }
            if (err != 0)
            {
                ResltBool = false;
                return false;
            }
            return ResltBool;
        }
        public double ClosingCircle { get; set; } = 7;
        public double ErosionCircle { get; set; } = 1;


        public double MaxLength2 { get; set; }
        public double MinLength2 { get; set; }
        public double MaxLength1 { get; set; }
        public double MinLength1 { get; set; }

        public double MaxDeg { get; set; }



        public byte H_threshold_min { get; set; }

        public byte H_threshold_max { get; set; }

        public byte H_threshold_min2 { get; set; }

        public byte H_threshold_max2 { get; set; }

        public byte S_threshold_min { get; set; }
        public byte S_threshold_max { get; set; }


        public byte S_threshold_min2 { get; set; }
        public byte S_threshold_max2 { get; set; }

        public byte V_threshold_min { get; set; }
        public byte V_threshold_max { get; set; }

        public double Select_shap_min { get; set; }
        public double Select_shap_max { get; set; }

        public class Wire_S
        {

            public HObject HObject { get; set; } = new HObject();

            public List<int> List3DName { get; set; } = new List<int>();


            public string Color { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="halcon"></param>
            /// <param name="wire"></param>
            /// <param name="hObject2"></param>
            /// <param name="hObject1"></param>
            /// <param name="HwindS"></param>
            /// <param name="hwindH"></param>
            /// <param name="hwindV"></param>
            /// <returns></returns>
            public bool RunP(HalconRun halcon, Wire_Solder wire, HObject hObject2, HObject hObject1, HTuple HwindS = null, HTuple hwindH = null, HTuple hwindV = null)
            {
                try
                {

                    HOperatorSet.AreaCenter(this.HObject, out HTuple area, out HTuple rows1, out HTuple columns1);
                    HOperatorSet.GetRegionIndex(hObject2, rows1.TupleInt(), columns1.TupleInt(), out HTuple index);
                    if (index.Length == 0)
                    {
                        return false;
                    }
                    HOperatorSet.SelectObj(hObject2, out HObject obj, index);
                    HOperatorSet.SmallestRectangle2(obj, out HTuple rowst1, out HTuple columnst1, out HTuple phist1, out HTuple length1st1, out HTuple length2st1);
                    Vision.gen_rectangle2_line_point(rowst1, columnst1, phist1, length1st1.TupleMult(2).TupleDiv(1.3), out HTuple rowsT1, out HTuple columnsT1, out HTuple rowsT2, out HTuple columnsT2);
                    Vision.gen_rectangle2_line_point(rowst1, columnst1, phist1, length1st1.TupleMult(2).TupleDiv(4), out HTuple rowsT12, out HTuple columnsT12, out HTuple rowsT22, out HTuple columnsT22);
                    HOperatorSet.GenRectangle2(out HObject hObject51, rowsT1, columnsT1, phist1, length1st1.TupleDiv(4), length2st1 + 5);
                    HOperatorSet.GenRectangle2(out HObject hObject52, rowsT2, columnsT2, phist1, length1st1.TupleDiv(4), length2st1 + 5);
                    HOperatorSet.GenRectangle2(out HObject hObject53, rowsT12, columnsT12, phist1, length1st1.TupleDiv(4), length2st1 + 5);
                    HOperatorSet.GenRectangle2(out HObject hObject54, rowsT22, columnsT22, phist1, length1st1.TupleDiv(4), length2st1 + 5);
                    HOperatorSet.Intersection(hObject51, hObject1, out hObject51);
                    HOperatorSet.Connection(hObject51, out hObject51);
                    HOperatorSet.AreaCenter(hObject51, out HTuple area2, out HTuple row35, out HTuple hTuple2);
                    HOperatorSet.SelectShape(hObject51, out hObject51, "area", "and", area2.TupleMax().TupleSub(1), 9999999999);
                    HOperatorSet.SmallestRectangle2(hObject51, out HTuple row1_s, out HTuple column1_s, out HTuple phi1_s, out HTuple length1_s, out HTuple length2_s);
                    halcon.AddMessageIamge(row1_s, column1_s, "1宽:" + (length2_s * 2).TupleString("0.1f"));
                    HOperatorSet.GenRectangle2(out hObject51, row1_s, column1_s, phi1_s, length1_s, length2_s);
                    //halcon.AddOBJ(hObject51, ColorResult.Red);


                    HOperatorSet.Intersection(hObject52, hObject1, out hObject52);
                    HOperatorSet.Connection(hObject52, out hObject52);
                    HOperatorSet.AreaCenter(hObject52, out area, out row35, out hTuple2);
                    HOperatorSet.SelectShape(hObject52, out hObject52, "area", "and", area.TupleMax().TupleSub(1), 9999999999);
                    HOperatorSet.SmallestRectangle2(hObject52, out row1_s, out column1_s, out phi1_s, out length1_s, out length2_s);
                    halcon.AddMessageIamge(row1_s, column1_s, "2宽:" + (length2_s * 2).TupleString("0.1f"));
                    HOperatorSet.GenRectangle2(out hObject51, row1_s, column1_s, phi1_s, length1_s, length2_s);
                    //halcon.AddOBJ(hObject51, ColorResult.Red);

                    HOperatorSet.Intersection(hObject53, hObject1, out hObject53);
                    HOperatorSet.Connection(hObject53, out hObject53);
                    HOperatorSet.AreaCenter(hObject53, out area, out row35, out hTuple2);
                    HOperatorSet.SelectShape(hObject53, out hObject53, "area", "and", area.TupleMax().TupleSub(1), 9999999999);
                    HOperatorSet.SmallestRectangle2(hObject53, out row1_s, out column1_s, out phi1_s, out length1_s, out length2_s);
                    halcon.AddMessageIamge(row1_s, column1_s, "3宽:" + (length2_s * 2).TupleString("0.1f"));
                    HOperatorSet.GenRectangle2(out hObject51, row1_s, column1_s, phi1_s, length1_s, length2_s);
                    //halcon.AddOBJ(hObject51, ColorResult.Red);
                    HOperatorSet.Intersection(hObject54, hObject1, out hObject54);
                    HOperatorSet.Connection(hObject54, out hObject54);
                    HOperatorSet.AreaCenter(hObject54, out area, out row35, out hTuple2);
                    HOperatorSet.SelectShape(hObject54, out hObject54, "area", "and", area.TupleMax().TupleSub(1), 9999999999);
                    HOperatorSet.SmallestRectangle2(hObject54, out row1_s, out column1_s, out phi1_s, out length1_s, out length2_s);
                    halcon.AddMessageIamge(row1_s, column1_s, "4宽:" + (length2_s * 2).TupleString("0.1f"));
                    HOperatorSet.GenRectangle2(out hObject51, row1_s, column1_s, phi1_s, length1_s, length2_s);

                }
                catch (Exception)
                {
                }

                return false;
            }
        }

    }
}
