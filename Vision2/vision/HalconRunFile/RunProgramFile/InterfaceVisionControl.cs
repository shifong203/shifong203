using System.Collections.Generic;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public interface InterfaceVisionControl
    {
        RunProgram AddListRun(string name, RunProgram run);

        ContextMenuStrip GetNewPrajetContextMenuStrip(string name);

        Dictionary<string, RunProgram> GetRunProgram();

        Dictionary<string, string> ListRunName { get; set; }

        bool RunHProgram(OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj);
    }
}