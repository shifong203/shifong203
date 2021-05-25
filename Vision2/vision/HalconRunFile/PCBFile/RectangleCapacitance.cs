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
    public class RectangleCapacitance : BPCBoJB, InterfacePCBA
    {
        public Control GetControl(HalconRun run)
        {
            return new RectangleCapacitanceControl1(this, run);
        }
        public void SaveThis(string path)
        {
            HalconRun.ClassToJsonSavePath(this, path);
        }
        public override BPCBoJB UpSatrt<T>(string path)
        {
            BPCBoJB bPCBoJB = base.UpSatrt<RectangleCapacitance>(path);
            if (bPCBoJB == null)
            {
                bPCBoJB = this;
            }
            return bPCBoJB;
        }

        public Threshold_Min_Max Threshold_Min_M { get; set; } = new Threshold_Min_Max();
        public Threshold_Min_Max Threshold_Min_DP { get; set; } = new Threshold_Min_Max();

        public Select_shape_Min_Max select_Shape_Min_Max { get; set; } = new Select_shape_Min_Max();

        [Category("检测项"), DisplayName("检测标准"), Description("")]
        [Editor(typeof(ValueMaxMinContrl.Editor), typeof(UITypeEditor))]

        public CapacitanceMinMaxV IntCapcitanceMinx { get; set; } = new CapacitanceMinMaxV();




        public override bool Run(HalconRun halcon, RunProgram run, OneResultOBj oneResultOBj, out HObject ErrRoi)
        {
         

           return  RunDebug(halcon, run, oneResultOBj,out  ErrRoi);
        }

        public bool RunDebug(HalconRun halcon, RunProgram run, OneResultOBj oneResultOBj, out HObject ErrRoi, int debugId = 0)
        {
        
            ErrNumber = 0;
            ErrRoi = new HObject();
            ErrRoi.GenEmptyObj();
            HObject hObject2 = new HObject();
            bool RsetBool = false;
            try
            {
                if (debugId != 0)
                {
                    halcon.HobjClear();
                }
                HOperatorSet.ReduceDomain(halcon.GetImageOBJ(Threshold_Min_M.ImageTypeObj), TestingRoi, out HObject imaget);
                HObject hObject = Threshold_Min_M.Threshold(imaget);
                HOperatorSet.Connection(hObject, out hObject);
                HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple row, out HTuple column);
                hObject= select_Shape_Min_Max.select_shape(hObject);
                HOperatorSet.SmallestRectangle2(hObject, out HTuple rows, out HTuple columns, out HTuple phi, out HTuple length1, out HTuple length2);
                HOperatorSet.AreaCenter(hObject, out area, out rows, out columns);
                if (rows.Length==2)
                {
                    HOperatorSet.DistancePp(rows[0], columns[0], rows[1], columns[1], out HTuple dist);

                    HOperatorSet.Union1(hObject, out hObject);
                    HOperatorSet.FillUp(hObject, out hObject);
                    HOperatorSet.ClosingCircle(hObject, out HObject hObject1, dist);
                    HOperatorSet.AreaCenter(hObject1, out HTuple area2, out row, out column);
                    HOperatorSet.SmallestRectangle2(hObject1, out row, out column, out HTuple phi2, out HTuple length12, out HTuple length22);
                    HOperatorSet.GenRectangle2(out  hObject2, row, column, phi2, length12, length22);
                    halcon.AddOBJ(hObject);
                    length12 = halcon.GetCaliConstMM(length12);
                    length22 = halcon.GetCaliConstMM(length22);
                    length2 = halcon.GetCaliConstMM(length2);
                    length1 = halcon.GetCaliConstMM(length1);
                    area2 = Math.Sqrt( halcon.GetCaliConstMM(area2));
                    for (int i = 0; i < area.Length; i++)
                    {
                        area[i] = Math.Sqrt(halcon.GetCaliConstMM(area.TupleSelect(i)));
                    }
                    //HOperatorSet.Union1()
                    HOperatorSet.Difference(hObject2, hObject, out HObject hObject3);
                    HObject hObject4  = Threshold_Min_DP.Threshold(imaget);
                    HOperatorSet.Intersection(hObject4, hObject3, out  hObject3);
                    HOperatorSet.AreaCenter(hObject3, out HTuple areaDt, out HTuple row2,out HTuple column2);
                    //HOperatorSet.ErosionCircle(hObject3, out hObject3, 1);
                    HOperatorSet.OpeningCircle(hObject3, out hObject3, 5);
                    halcon.AddOBJ(hObject3,RunProgram.ColorResult.yellow);
                    if (this.IntCapcitanceMinx.RaSetValeu(length12) != 0)
                    {
                        ErrNumber++;
                    }
                    if (this.IntCapcitanceMinx.RbSetValeu(length22) != 0)
                    {
                        ErrNumber++;
                    }
                    if (this.IntCapcitanceMinx.AngleSetValeu(phi2) != 0)
                    {
                        ErrNumber++;
                    }
                    if (this.IntCapcitanceMinx.AreaSetValeu(areaDt) != 0)
                    {
                        ErrNumber++;
                    }
                    if (debugId != 0)
                    {
                        halcon.AddMessageIamge(row, column, "长度" + length12 + "宽度" + length22+ "角度" + phi2.TupleDeg() + "面积" + area2);
                        halcon.AddMessageIamge(rows, columns, "长度" + length1 + "宽度" + length2+ "角度" + phi.TupleDeg() + "面积" + area);
                        halcon.AddOBJ(hObject);
                        halcon.ShowObj();
                    }
                }
                else
                {
                    ErrNumber++;
                    halcon.AddOBJ(TestingRoi, RunProgram.ColorResult.blue);
                    halcon.AddOBJ(hObject, RunProgram.ColorResult.red);
                    if (debugId != 0)
                    {
           
                        halcon.AddMessageIamge(rows, columns, "长度" + length1 + "宽度" + length2 + "角度" + phi.TupleDeg() + "面积" + area);
                        halcon.AddOBJ(hObject);
                        halcon.ShowObj();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (ErrNumber == 0)
            {
                halcon.AddOBJ(hObject2);
                RsetBool = true;
            }
            else
            {
                ErrRoi = ErrRoi.ConcatObj(hObject2);
                oneResultOBj.AddNGOBJ(this.Name, "长度", TestingRoi.Clone(), ErrRoi);
            }
            return RsetBool;
        }
    }
}
