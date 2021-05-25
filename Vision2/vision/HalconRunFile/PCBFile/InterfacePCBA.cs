using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    interface InterfacePCBA
    {
        public Control GetControl(HalconRunFile.RunProgramFile.HalconRun run);

        public void SaveThis(string path);


    }
}
