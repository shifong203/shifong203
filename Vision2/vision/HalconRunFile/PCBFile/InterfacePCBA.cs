using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    internal interface InterfacePCBA
    {
        public Control GetControl(HalconRunFile.RunProgramFile.IDrawHalcon run);

        public void SaveThis(string path);
    }
}