using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using static Vision2.vision.HalconRunFile.RunProgramFile.Color_Detection;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public class Mode_Color : RunProgram
    {
        public override Control GetControl(IDrawHalcon halconRun)
        {
            throw new NotImplementedException();
        }

        public Color_classify ModeColor { get; set; } = new Color_classify();

        public override bool RunHProgram(OneResultOBj oneResultOBj, out List<OneRObj> oneRObj, AoiObj aoiObj = null)
        {
            oneRObj = new List<OneRObj>();
            try
            {
                ModeColor.Classify(oneResultOBj, aoiObj, this, out HObject hObject);

            }
            catch (Exception)
            {
            }
            return false;
        }

        public override RunProgram UpSatrt<T>(string path)
        {
           return   base.ReadThis<Mode_Color>(path);
        }
    }
}
