using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.PCBFile.ICPint;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    public class RectangleCapacitance : RunProgram, InterfacePCBA
    {
        public override Control GetControl(HalconRun run)
        {
            return new RectangleCapacitanceControl1(this, run);
        }
   
        public override RunProgram UpSatrt<T>(string path)
        {
            RunProgram bPCBoJB = base.ReadThis<RectangleCapacitance>(path);
            if (bPCBoJB == null)
            {
                bPCBoJB = this;
            }
            return bPCBoJB;
        }


        public double ModeRow { get; set; }

        public double ModeCol { get; set; }

        public double outRow { get; set; }

        public double outCol { get; set; }


        public Threshold_Min_Max Threshold_Min_M { get; set; } = new Threshold_Min_Max();
        public Threshold_Min_Max Threshold_Min_DP { get; set; } = new Threshold_Min_Max();

        public Select_shape_Min_Max select_Shape_Min_Max { get; set; } = new Select_shape_Min_Max();

        [Category("检测项"), DisplayName("检测标准"), Description("")]
        [Editor(typeof(ValueMaxMinContrl.Editor), typeof(UITypeEditor))]

        public CapacitanceMinMaxV IntCapcitanceMinx { get; set; } = new CapacitanceMinMaxV();


        public override bool RunHProgram( OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj)
        {
            oneRObjs = new List<OneRObj>();
            return RunDebug(oneResultOBj);
        }
        public bool RunDebug( OneResultOBj oneResultOBj, int debugId = 0)
        {
            NGRoi = new HObject();
            NGRoi.GenEmptyObj();
            HObject hObject2 = new HObject();
            hObject2.GenEmptyObj();
            bool RsetBool = false;
            HTuple Moderow = new HTuple();
            HTuple Modecolumn = new HTuple();
            HOperatorSet.AreaCenter(AOIObj, out HTuple area, out Moderow, out Modecolumn);
            try
            {
                if (debugId != 0)
                {
                    oneResultOBj.ClearAllObj();
                }
                HOperatorSet.ReduceDomain(oneResultOBj .GetHalcon().GetImageOBJ(Threshold_Min_M.ImageTypeObj), AOIObj, out HObject imaget);
           
                HObject hObject = Threshold_Min_M.Threshold(imaget);
                HOperatorSet.Connection(hObject, out hObject);
                HOperatorSet.AreaCenter(hObject, out  area, out HTuple  row, out  HTuple column);
                hObject= select_Shape_Min_Max.select_shape(hObject);
                HOperatorSet.SmallestRectangle2(hObject, out HTuple rows, out HTuple columns, out HTuple phi, out HTuple length1, out HTuple length2);
                HOperatorSet.AreaCenter(hObject, out area, out rows, out columns);
                if (rows.Length==2)
                {
                    HOperatorSet.DistancePp(rows[0], columns[0], rows[1], columns[1], out HTuple dist);

                    HOperatorSet.Union1(hObject, out hObject);
                    HOperatorSet.FillUp(hObject, out hObject);
                    HOperatorSet.ClosingRectangle1(hObject, out HObject hObject1, dist, dist);
                    HOperatorSet.AreaCenter(hObject1, out HTuple area2, out row, out column);
                    HOperatorSet.SmallestRectangle2(hObject1, out row, out column, out HTuple phi2, out HTuple length12, out HTuple length22);
                    HOperatorSet.GenRectangle2(out  hObject2, row, column, phi2, length12, length22);
                    oneResultOBj.AddObj(hObject);
                    HOperatorSet.GenCrossContourXld(out HObject corss, row, column, length12/2, 0);
                    outRow = row;
                    outCol = column;
                    HOperatorSet.DistancePp(outRow, outCol, ModeRow, ModeCol, out HTuple distfMM);
                    distfMM = oneResultOBj.GetCaliConstMM(distfMM);
                    if (this.IntCapcitanceMinx.SkewingSetValeu(distfMM) != 0)
                    {
                        NGTextS.Append("偏移");
                           NGNumber++;
                    }
                    oneResultOBj.AddObj(corss, ColorResult.yellow);
                    length12 = oneResultOBj.GetCaliConstMM(length12);
                    length22 = oneResultOBj.GetCaliConstMM(length22);
                    length2 = oneResultOBj.GetCaliConstMM(length2);
                    length1 = oneResultOBj.GetCaliConstMM(length1);
                    area2 = Math.Sqrt(oneResultOBj.GetCaliConstMM(area2));
                    for (int i = 0; i < area.Length; i++)
                    {
                        area[i] = Math.Sqrt(oneResultOBj.GetCaliConstMM(area.TupleSelect(i)));
                    }
                    //HOperatorSet.Union1()
                    HOperatorSet.Difference(hObject2, hObject, out HObject hObject3);
                    HObject hObject4  = Threshold_Min_DP.Threshold(imaget);
                    HOperatorSet.Intersection(hObject4, hObject3, out  hObject3);
                    //hObject3= Rectang_select_Shape_Min_Max.select_shape(hObject3);
                    HOperatorSet.AreaCenter(hObject3, out HTuple areaDt, out HTuple row2,out HTuple column2);
                    //HOperatorSet.ErosionCircle(hObject3, out hObject3, 1);
                    HOperatorSet.OpeningCircle(hObject3, out hObject3, 5);
          
                    oneResultOBj.AddObj(hObject3,ColorResult.yellow);
                    if (this.IntCapcitanceMinx.RaSetValeu(length12) != 0)
                    {
                        NGTextS.Append("长度");
                        NGNumber++;
                    }
                    if (this.IntCapcitanceMinx.RbSetValeu(length22) != 0)
                    {
                        NGTextS.Append("宽度");
                        NGNumber++;
                    }
                    if (this.IntCapcitanceMinx.AngleSetValeu(phi2) != 0)
                    {
                        NGTextS.Append("角度");
                        NGNumber++;
                    }
                    if (this.IntCapcitanceMinx.AreaSetValeu(areaDt) != 0)
                    {
                        NGTextS.Append("面积");
                        NGNumber++;
                    }
                    if (debugId != 0)
                    {
                        oneResultOBj.AddImageMassage(row, column, "长度" + length12 + "宽度" + length22+ "角度" + phi2.TupleDeg() + "面积" + area2);
                        oneResultOBj.AddImageMassage(rows, columns, "长度" + length1 + "宽度" + length2+ "角度" + phi.TupleDeg() + "面积" + area);
                        oneResultOBj.AddObj(hObject);
                        oneResultOBj .GetHalcon().ShowObj();
                    }
                }
                else
                {
                    NGTextS.Append("焊盘数量错误");
                    NGNumber++;
                    oneResultOBj.AddObj(AOIObj, ColorResult.blue);
                    oneResultOBj.AddObj(hObject, ColorResult.red);
                    if (debugId != 0)
                    {

                        oneResultOBj.AddImageMassage(rows, columns, "长度" + length1 + "宽度" + length2 + "角度" + phi.TupleDeg() + "面积" + area);
                        oneResultOBj.AddObj(hObject);
                        oneResultOBj.GetHalcon().ShowObj();
                    }
                }
            }
            catch (Exception ex)
            {
                NGTextS.Append( "执行错误;");
                NGNumber++;
            }
            if (NGNumber == 0)
            {
                oneResultOBj.AddObj(hObject2);
                RsetBool = true;
            }
            else
            {
                oneResultOBj.AddImageMassage(Moderow, Modecolumn, NGTextS, ColorResult.red);
;                NGRoi = NGRoi.ConcatObj(hObject2);
                string ngt = "";
                for (int i = 0; i < NGTextS.Length; i++)
                {
                    ngt += NGTextS[i] + ";";
                }
                oneResultOBj.AddNGOBJ(this.Name, ngt, AOIObj, NGRoi);
            }
            return RsetBool;
        }
    }
}
