using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    /// <summary>
    /// 计算测量
    /// </summary>
    public class Calculate : RunProgram
    {
        [DescriptionAttribute("边缘算法。 'canny', 'deriche1', 'deriche1_int4', 'deriche2', 'deriche2_int4'," +
            " 'lanser1', 'lanser2', 'mshen', 'shen', 'sobel_fast'"),
            Category("视觉参数"), DisplayName("边缘算法")]
        public string Filter { get; set; } = "canny";
        [DescriptionAttribute("过滤器参数:小的值导致强大的平滑，因此更少的细节(相反的'精明')。"),
            Category("视觉参数"), DisplayName("过滤器")]
        public double Alpha { get; set; } = 4.0;
        [DescriptionAttribute("非最大抑制(“none”，如果不需要)。 'hvnms', 'inms', 'nms', 'none'"),
         Category("视觉参数"), DisplayName("最大抑制")]
        public string NMS { get; set; } = "nms";
        [DescriptionAttribute("低阈值的迟滞阈值操作(如果不需要阈值操作为负)。1 ≤ Low ≤ 255"),
        Category("视觉参数"), DisplayName("低阀值")]
        public double Low { get; set; } = 20;

        [DescriptionAttribute("滞回阈值操作的上阈值(如果不需要阈值操作为负)。 1 ≤ High ≤ 255"),
        Category("视觉参数"), DisplayName("高阀值")]
        public double High { get; set; } = 40;
        [DescriptionAttribute("二值化 1 ≤ Low ≤ 255"),
        Category("视觉参数"), DisplayName("二值化下限")]
        public byte thresholdLow { get; set; } = 1;
        [DescriptionAttribute("二值化上限 1 ≤ High ≤ 255"),
        Category("视觉参数"), DisplayName("二值化上限")]
        public byte thresholdHigh { get; set; } = 255;

        public override bool RunHProgram( OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, int runID = 0)
        {
            oneRObjs = new List<OneRObj>();
            try
            {
                //HOperatorSet.EdgesImage(halcon.Image(), out HObject hObject1, out HObject hObject2, Filter,Alpha, NMS, Low, High);
                HOperatorSet.Threshold(oneResultOBj.Image, out HObject hObject1, thresholdLow, thresholdHigh);
                HOperatorSet.Connection(hObject1, out hObject1);
                HOperatorSet.SelectShape(hObject1, out NGRoi, "height", "and", 30, 999999);
                return true;
            }
            catch (Exception ex)
            {
                //halcon.ErrLog
                this.LogErr(this.Name, ex);
            }
            return false;
        }
        public override Control GetControl(HalconRun halcon)
        {
            return new CalculateUserControl(this);
        }

        public override RunProgram UpSatrt<T>(string PATH)
        {
            return base.ReadThis<Calculate>(PATH);
        }
    }
}