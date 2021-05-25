using Vision2.ErosProjcetDLL.Project;

namespace Vision2.vision
{
    /// <summary>
    /// 视觉控件接口
    /// </summary>
    interface Interface_Vision_Control : ProjectNodet.IClickNodeProject
    {

        void Set_Item<T>(T run_Projet);

        void UP_Item<T>(T run_Projet);

    }
}
