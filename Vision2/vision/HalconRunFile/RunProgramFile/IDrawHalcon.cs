using HalconDotNet;
using System.ComponentModel;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public interface IDrawHalcon
    {
        [Browsable(false)]
        bool Drawing { get; set; }

        void Focus();
        HObject Image(HObject hObject = null);

    }
}
