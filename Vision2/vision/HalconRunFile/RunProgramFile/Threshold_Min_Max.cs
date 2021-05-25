using HalconDotNet;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public class Threshold_Min_Max
    {
        public Threshold_Min_Max()
        {
            Max = 255;
        }
        public bool Enabled = true;
        public Vision.ImageTypeObj ImageTypeObj;
        public byte Min { get; set; }

        public byte Max { get; set; } = 255;
        public HObject Threshold(HObject hObject)
        {
            HOperatorSet.Threshold(hObject, out HObject hObject1, Min, Max);
            return hObject1;
        }
    }

}
