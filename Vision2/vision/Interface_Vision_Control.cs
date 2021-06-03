using Vision2.ErosProjcetDLL.Project;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision
{
    /// <summary>
    /// 视觉控件接口
    /// </summary>
    interface Interface_Vision_Control : ProjectNodet.IClickNodeProject
    {
        void SaveThis(string path);
        RunProgram UpSatrt<T>(string path);

        RunProgram ReadThis<T>(string Path);

        bool Run(HalconRun halcon, OneResultOBj oneResultOBj, int runid = 0);
        bool RunHProgram(HalconRun halcon, OneResultOBj oneResultOBj, int runID = 0, string name = null);

        void Set_Item<T>(T run_Projet);

        void UP_Item<T>(T run_Projet);

    }
}
