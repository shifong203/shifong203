using HalconDotNet;
using System.ComponentModel;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public interface IDrawHalcon
    {
        [Browsable(false)]
        bool Drawing { get; set; }

        int DrawType { get; set; }
        bool DrawErasure { get; set; }

        void Focus();

        HWindow hWindowHalcon(HWindow hawid = null);

        HObject Image(HObject hObject = null);

        //HalconRun .EnumDrawType EnumDrawType { get; set; }
        void HobjClear();

        void AddMeassge(HTuple text);

        void AddObj(HObject hObject, ColorResult colorResult = ColorResult.green);

        void ShowObj();
    }
}