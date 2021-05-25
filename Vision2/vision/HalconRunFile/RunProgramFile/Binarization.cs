using System.Windows.Forms;
using HalconDotNet;
using NokidaE.vision.HalconRunFile;

namespace NokidaE.vision.HalconRunFile.RunProgramFile
{

    /// <summary>
    /// 二值化
    /// </summary>
    public class Binarization : RunProgram
    {
        protected override bool RunHProgram(HalconRun hImage,int runid=0)
        {
            HOperatorSet.EdgesImage(hImage.Image(), out HObject imapi, out HObject imadir, "canny", 4, "nms", 45, 60);
            HOperatorSet.Threshold(imapi, out imadir, 8, 255);
            HOperatorSet.Connection(imadir, out imadir);
            HOperatorSet.SelectShape(imadir, out imadir, "height", "and", 30, 99999);
            hImage.AddOBJ( imadir);
            return true;
        }
        public override Control GetUpControl()
        {
            throw new System.NotImplementedException();
        }

        public override RunProgram UpSatrt<T>(string PATH)
        {
           return base.ReadThis<Binarization>(PATH);
        }
    }
}