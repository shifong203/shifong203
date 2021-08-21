﻿using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    /// <summary>
    ///圆形电阻
    /// </summary>
    public class Capacitance : RunProgram
    {
        //public HObject TestingRoi = new HObject();
        public Capacitance()
        {
            //TestingRoi.GenEmptyObj();
        }

        public override RunProgram UpSatrt<T>(string path)
        {
            RunProgram bPCBoJB = base.ReadThis<Capacitance>(path);
            if (bPCBoJB == null)
            {
                bPCBoJB = this;
            }
            return bPCBoJB;
        }

        public void SaveThis(string path)
        {
            HalconRun.ClassToJsonSavePath(this, path);
        }

        public double Periphery_Circle { get; set; } = 100;
        public double Inside_Circle { get; set; } = 40;

        public byte Periphery_ThreadMin { get; set; } = 180;
        public byte Periphery_ThreadMax { get; set; } = 255;

        public byte Inside_Thread_Min { get; set; } = 40;
        public byte Inside_Thread_Max { get; set; } = 80;

        public byte Erosion_Circle { get; set; } = 0;

        public double AngleMin { get; set; } = 0;

        public double AngleMax { get; set; } = 100;

        public double Angle;

        public bool IsText { get; set; }

        public bool IsRoi { get; set; }

        public ImageTypeObj ImageTypeObj { get; set; }
        public Select_shape_Min_Max Select_Shape_Min_Max { get; set; } = new Select_shape_Min_Max();

        public override Control GetControl(IDrawHalcon run)
        {
            return new Cap(this, run);
        }

        public override bool RunHProgram(OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj)
        {
            oneRObjs = new List<OneRObj>();
            HObject ErrRoi = new HObject();
            ErrRoi.GenEmptyObj();
            bool RsetBool = false;
            try
            {
                HOperatorSet.ReduceDomain(oneResultOBj.GetHalcon().GetImageOBJ(ImageTypeObj), this.AOIObj, out HObject hObject);
                HOperatorSet.Threshold(hObject, out HObject Thresholdt, Periphery_ThreadMin, Periphery_ThreadMax);
                HOperatorSet.Connection(Thresholdt, out Thresholdt);
                HOperatorSet.SelectShape(Thresholdt, out Thresholdt, new HTuple("area", "circularity"), "and", new HTuple(150, 0.6), new HTuple(99999, 1));
                HOperatorSet.AreaCenter(Thresholdt, out HTuple area, out HTuple row, out HTuple column);
                if (row.Length == 1)
                {
                    HOperatorSet.GenCircle(out ErrRoi, row, column, this.Inside_Circle);
                    HOperatorSet.ReduceDomain(hObject, ErrRoi, out HObject hObjectt);
                    HOperatorSet.Threshold(hObjectt, out HObject Thresholdt2, Inside_Thread_Min, Inside_Thread_Max);
                    if (Erosion_Circle != 0)
                    {
                        HOperatorSet.OpeningCircle(Thresholdt2, out Thresholdt2, Erosion_Circle);
                    }
                    HOperatorSet.Connection(Thresholdt2, out Thresholdt2);
                    HObject hObject2 = Select_Shape_Min_Max.select_shape(Thresholdt2);
                    oneResultOBj.AddObj(hObject2, ColorResult.yellow);
                    HOperatorSet.Union1(hObject2, out hObject2);
                    HOperatorSet.AreaCenter(hObject2, out HTuple area2, out HTuple row2, out HTuple colu2);
                    if (row2.Length == 0) oneResultOBj.AddObj(this.AOIObj, ColorResult.red);
                    else
                    {
                        HOperatorSet.AngleLx(row, column, row2, colu2, out HTuple angle);
                        Angle = angle.TupleDeg();
                        if (Angle < 0)
                        {
                            Angle = 360 + Angle;
                        }
                        Vision.Gen_arrow_contour_xld(out HObject hoarrow, row, column, row2, colu2);
                        if (Angle < AngleMin || Angle > AngleMax)
                        {
                            oneResultOBj.AddObj(this.AOIObj, ColorResult.red); NGNumber++;
                        }
                        else
                        {
                            if (IsRoi) oneResultOBj.AddObj(this.AOIObj, ColorResult.blue);
                        }
                        oneResultOBj.AddObj(hoarrow, ColorResult.blue);
                        if (IsText) oneResultOBj.AddImageMassage(row, column, Angle);
                    }
                }
                else oneResultOBj.AddObj(this.AOIObj, ColorResult.red); NGNumber++;
                if (NGNumber != 0)
                {
                    RsetBool = false;
                }
                else
                {
                    RsetBool = true;
                }
            }
            catch (Exception ex)
            {
            }
            return RsetBool;
        }
    }
}