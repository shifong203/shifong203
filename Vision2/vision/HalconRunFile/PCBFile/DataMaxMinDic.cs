using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    public  class DataMaxMinDic
    {
        [Category("坐标位置"), DisplayName("参考坐标Row"), Description("")]
        /// <summary>
        /// 参考坐标
        /// </summary>
        public HTuple ModeRow { get; set; } = new HTuple();


        [Category("坐标位置"), DisplayName("参考坐标角度"), Description("")]
        /// <summary>
        /// 参考坐标
        /// </summary>
        public HTuple ModeAngle { get; set; } = new HTuple();

        [Category("坐标位置"), DisplayName("参考坐标Col"), Description("")]
        /// <summary>
        /// 参考坐标
        /// </summary>
        public HTuple ModeCol { get; set; } = new HTuple();

        [Category("坐标位置"), DisplayName("目标坐标Rows"), Description("")]
        /// <summary>
        /// 参考坐标
        /// </summary>
        public HTuple OutRow { get; set; } = new HTuple();

        [Category("坐标位置"), DisplayName("目标坐标Cols"), Description("")]
        /// <summary>
        /// 参考坐标
        /// </summary>
        public HTuple OutCol { get; set; } = new HTuple();

        [Category("坐标位置"), DisplayName("同步到参考坐标"), Description("将目标坐标同步到参考坐标")]
        public bool IsModePoint
        {
            get { return false; }
            set
            {
                if (value)
                {
                    ModeRow = OutRow;
                    ModeCol = OutCol;
                }
            }
        }
        /// <summary>
        /// 是否比较位置
        /// </summary>
        [Category("坐标位置"), DisplayName("坐标比较"), Description("坐标比较")]
        public bool IsModeCta { get; set; }

        [DisplayName("当前值"), Description(""), Category("坐标比较"), ReadOnly(true)]
        public HTuple SkewingValue { get; set; }

        /// <summary>
        /// 偏移
        /// </summary>
        [DisplayName("偏移±差"), Description(""), Category("坐标比较"), ReadOnly(true)]
        public double Skewing { get; set; } = 10;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oneResultOBj"></param>
        /// <param name="errObj"></param>
        /// <returns></returns>
        public  virtual  bool CompareData(OneResultOBj oneResultOBj,out HObject errObj)
        {
            errObj = new HObject();
            errObj.GenEmptyObj();
            try
            {
            
                if (IsModeCta)
                {
                    if (OutRow.Length!=ModeRow.Length)
                    {
                        return false;
                    }
                    else
                    {
                        HOperatorSet.DistancePp(OutRow, OutCol, ModeRow, ModeCol, out HTuple dist);
                        SkewingValue = oneResultOBj.GetCaliConstMM(dist);
                        HTuple det = SkewingValue.TupleAbs().TupleGreaterEqualElem(Skewing);
                        det = det.TupleFind(1);
                        if (det >= 0)
                        {
                            if (det.Length != 0)
                            {
                                HOperatorSet.GenRectangle2(out  errObj, ModeRow.TupleSelect(det),
                                ModeCol.TupleSelect(det), HTuple.TupleGenConst(det.Length, 0),
                                HTuple.TupleGenConst(det.Length,  200), HTuple.TupleGenConst(det.Length,200));
                                //aoiObj.NGErr = aoiObj.NGErr.ConcatObj(err);
                                //oneResultOBj.AddObj(errObj, ColorResult.red);
                                 oneResultOBj.AddImageMassage(ModeRow.TupleSelect(det), ModeCol.TupleSelect(det), 
                             "d" + SkewingValue.TupleSelect(det), ColorResult.red);

        
                                return false;
                            }
                        }
                    } 
                }
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }
    }
}
