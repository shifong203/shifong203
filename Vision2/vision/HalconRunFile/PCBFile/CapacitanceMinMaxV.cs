using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision2.vision.HalconRunFile.PCBFile
{
   public class CapacitanceMinMaxV
    {
        //[Editor(typeof(pgu))]
        [DisplayName("±值"), Description(""), Category("长度")]
        public double PM { get; set; } = 10;
        //[Editor(typeof(NumericUpDown))]
        [DisplayName("标准值"), Description(""), Category("长度")]
        public double Value { get; set; } = 10;

        [DisplayName("Min"), Description(""), Category("长度")]
        public double Min { get; set; } = 10;

        [DisplayName("Max"), Description(""),Category("长度")]
        public double Max { get; set; } = 100;

        [DisplayName("使用标准值"), Description("使用标准值=true,fales使用最大最小值"),Category("长度")]
        public bool PMBool { get; set; } = true;
        [DisplayName("当前值"), Description(""), Category("长度"), ReadOnly(true)]
        public double RaValue { get; set; }
        /// <summary>
        /// 比较参数
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>0=OK,1=标准值NG，2大于正负值NG</returns>
        public int RaSetValeu(double value)
        {
            RaValue = value;
            RaErr = true;
            if (PMBool)
            {
                if (value > Value + PM )
                {
                    return 4;
                }
                else if (value < Value - PM)
                {
                    return 3;
                }
            }
            else
            {
                if (value > Max )
                {
                    return 2;
                }
                else if(value < Min)
                {
                    return 1;
                }
            }
            RaErr = false;
            return 0;
        }


        [DisplayName("±值"), Description(""), Category("宽度")]
        public double RbPM { get; set; } = 10;
        [DisplayName("标准值"), Description(""), Category("宽度")]
        public double RbCValue { get; set; } = 10;

        [DisplayName("Min"), Description(""), Category("宽度")]
        public double RbMin { get; set; } = 10;

        [DisplayName("Max"), Description(""), Category("宽度")]
        public double RbMax { get; set; } = 100;

        [DisplayName("使用标准值"), Description("使用标准值=true,fales使用最大最小值"), Category("宽度")]
        public bool RbPMBool { get; set; } = true;
        [DisplayName("宽度当前值"), Description(""), Category("宽度"),ReadOnly(true)]
        public double RbValue { get; set; }
        /// <summary>
        /// 比较参数
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>0=OK,1=标准值NG，2大于正负值NG</returns>
        public int RbSetValeu(double value)
        {
            RbValue = value;
            RbErr = true;
            if (RbPMBool)
            {
                if (value > RbCValue + RbPM )
                {
                    return 4;
                }
                else if( value < RbCValue - RbPM)
                {
                    return 3;
                }
            }
            else
            {
                if (value > RbMax)
                {
                    return 2;
                }
                else if (  value < RbMin)
                {
                    return 1;
                }
            }
            RbErr = false;
            return 0;
        }
        [DisplayName("±值"), Description(""), Category("高度")]
        public double HeigthPM { get; set; } = 10;
        [DisplayName("标准值"), Description(""), Category("高度")]
        public double HeigthCValue { get; set; } = 10;

        [DisplayName("Min"), Description(""), Category("高度")]
        public double HeigthMin { get; set; } = 10;

        [DisplayName("Max"), Description(""), Category("高度")]
        public double HeigthMax { get; set; } = 100;

        [DisplayName("使用标准值"), Description("使用标准值=true,fales使用最大最小值"), Category("高度")]
        public bool HeigthPMBool { get; set; } = true;
        [DisplayName("当前值"), Description(""), Category("高度"), ReadOnly(true)]
        public double HeigthValue { get; set; }
        /// <summary>
        /// 比较参数
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>0=OK,1=标准值NG，2大于正负值NG</returns>
        public int HeigthSetValeu(double value)
        {
            HeigthValue = value;
            HeigthErr = true;
            if (HeigthPMBool)
            {
                if (value > HeigthCValue + HeigthPM)
                {
                    return 4;
                }
                else if (value < HeigthCValue - HeigthPM)
                {
                    return 3;
                }
              }
            else
            {
                if (value > HeigthMax )
                {
                    return 2;
                }else if( value < HeigthMin)
                {
                    return 1;
                }
            }
            HeigthErr = false;
            return 0;
        }



        [DisplayName("±值"), Description(""), Category("体积")]
        public double VolumePM { get; set; } = 10;
        [DisplayName("标准值"), Description(""), Category("体积")]
        public double VolumeCValue { get; set; } = 10;

        [DisplayName("Min"), Description(""), Category("体积")]
        public double VolumeMin { get; set; } = 10;

        [DisplayName("Max"), Description(""), Category("体积")]
        public double VolumeMax { get; set; } = 100;

        [DisplayName("使用标准值"), Description("使用标准值=true,fales使用最大最小值"), Category("体积")]
        public bool VolumePMBool { get; set; } = true;

        [DisplayName("当前值"), Description(""), Category("体积"), ReadOnly(true)]
        public double VolumeValue { get; set; }
        /// <summary>
        /// 比较参数
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>0=OK,1=标准值NG，2大于正负值NG</returns>
        public int VolumeSetValeu(double value)
        {
            VolumeValue = value;
            VolumeErr = true;
            if (VolumePMBool)
            {
                if (value > VolumeCValue + VolumePM )
                {
                    return 4;
                }else  if ( value < VolumeCValue - VolumePM)
                {
                    return 3;
                }
            }
            else
            {
                if (value > VolumeMax)
                {
                    return 2;
                }
                else if( value < VolumeMin)
                {
                    return 1;
                }
            }
            VolumeErr = false;
            return 0;
        }

        [DisplayName("±值"), Description(""), Category("面积")]
        public double  AreaPM { get; set; } = 10;
        [DisplayName("标准值"), Description(""), Category("面积")]
        public double AreaCValue { get; set; } = 10;

        [DisplayName("Min"), Description(""), Category("面积")]
        public double AreaMin { get; set; } = 10;

        [DisplayName("Max"), Description(""), Category("面积")]
        public double AreaMax { get; set; } = 100;

        [DisplayName("使用标准值"), Description("使用标准值=true,fales使用最大最小值"), Category("体积")]
        public bool AreaPMBool { get; set; } = true;

        [DisplayName("当前值"), Description(""), Category("面积"), ReadOnly(true)]
        public double AreaValue { get; set; }
        /// <summary>
        /// 比较参数
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>0=OK,1=标准值NG，2大于正负值NG</returns>
        public int AreaSetValeu(double value)
        {
            VolumeValue = value;
            AreaErr = true;
            if (VolumePMBool)
            {
                if (value > VolumeCValue + VolumePM)
                {
                    return 4;
                }
                else if (value < VolumeCValue - VolumePM)
                {
                    return 3;
                }
            }
            else
            {
                if (value > VolumeMax)
                {
                    return 2;
                }
                else if (value < VolumeMin)
                {
                    return 1;
                }
            }
            AreaErr = false;
            return 0;
        }

        [DisplayName("±值"), Description(""), Category("角度")]
        public double AnglePM { get; set; } = 10;
        [DisplayName("标准值"), Description(""), Category("角度")]
        public double AngleCValue { get; set; } = 10;

        [DisplayName("Min"), Description(""), Category("角度")]
        public double AngleMin { get; set; } = 10;

        [DisplayName("Max"), Description(""), Category("角度")]
        public double AngleMax { get; set; } = 100;

        [DisplayName("使用标准值"), Description("使用标准值=true,fales使用最大最小值"), Category("体积")]
        public bool AnglePMBool { get; set; } = true;

        [DisplayName("当前值"), Description(""), Category("角度"), ReadOnly(true)]
        public double   AngleValue  { get; set; }
        /// <summary>
        /// 比较参数
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>0=OK,1=标准值NG，2大于正负值NG</returns>
        public int AngleSetValeu(double value)
        {
              AngleValue  = value;
            AngleErr = true;
            if (AnglePMBool)
            {
                if (value > AngleCValue + AnglePM)
                {
                    return 4;
                }
                else if (value < AngleCValue - AnglePM)
                {
                    return 3;
                }
            }
            else
            {
                if (value > AngleMax)
                {
                    return 2;
                }
                else if (value < AngleMin)
                {
                    return 1;
                }
            }
            AngleErr = false;
            return 0;
        }



        [Category("检测结果"), DisplayName("高度缺陷"), Description(""), ReadOnly(true)]
        public bool HeigthErr { get; set; }


        [Category("检测结果"), DisplayName("长度缺陷"), Description(""), ReadOnly(true)]
        public bool RaErr { get; set; }


        [Category("检测结果"), DisplayName("宽度缺陷"), Description(""), ReadOnly(true)]
        public bool RbErr { get; set; }


        [Category("检测结果"), DisplayName("体积缺陷"), Description(""), ReadOnly(true)]
        public bool VolumeErr { get; set; }

        [Category("检测结果"), DisplayName("面积缺陷"), Description(""), ReadOnly(true)]
        public bool AreaErr { get; set; }

        [Category("检测结果"), DisplayName("角度缺陷"), Description(""), ReadOnly(true)]
        public bool AngleErr { get; set; }


        [Category("检测结果"), DisplayName("破损缺陷"), Description(""), ReadOnly(true)]
        public bool DamagedErr { get; set; }

        [Category("检测结果"), DisplayName("少捍缺陷"), Description(""), ReadOnly(true)]

        public bool LessWeldingErr { get; set; }
        [Category("检测结果"), DisplayName("多捍缺陷"), Description(""), ReadOnly(true)]
        public bool ManyWelding { get; set; }


        public void Inset()
        {
            AngleValue =   HeigthValue = RaValue = RbValue = VolumeValue = 00;
            AngleErr= HeigthErr = RaErr = RbErr = VolumeErr = AreaErr = false;
            
        }
    }
}
