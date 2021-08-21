using System.Collections.Generic;
using Vision2.ErosProjcetDLL.Project;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision
{
    /// <summary>
    /// 视觉控件接口
    /// </summary>
    internal interface Interface_Vision_Control : ProjectNodet.IClickNodeProject
    {
        void SaveThis(string path);

        RunProgram UpSatrt<T>(string path);

        RunProgram ReadThis<T>(string Path);

        bool Run(OneResultOBj oneResultOBj, AoiObj aoiObj = null);

        bool RunHProgram(OneResultOBj oneResultOBj, out List<OneRObj> oneRObj, AoiObj aoiObj);

        void Set_Item<T>(T run_Projet);

        void UP_Item<T>(T run_Projet);
    }
}