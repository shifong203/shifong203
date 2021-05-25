using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
      public    interface InterfaceVisionControl
    {
        RunProgram AddListRun(string name, RunProgram run);
        ContextMenuStrip GetNewPrajetContextMenuStrip(string name);
         Dictionary<string, RunProgram> GetRunProgram();
         Dictionary<string, string> ListRunName { get; set; }
         bool RunHProgram(HalconRun run, OneResultOBj oneResultOBj, int id =0,string name = null);
    }


}
