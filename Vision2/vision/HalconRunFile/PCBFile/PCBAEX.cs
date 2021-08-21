using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    /// <summary>
    /// 库
    /// </summary>
    public class PCBAEX : RunProgram
    {
        public override Control GetControl(IDrawHalcon halcon)
        {
            return new PCBBControl(this) { };
            //return new PCBAControl(this);
        }

        public override RunProgram UpSatrt<T>(string path)
        {
            RunProgram pCBA = base.ReadThis<PCBAEX>(path);
            PCBAEX pCBA1 = pCBA as PCBAEX;
            try
            {
                if (pCBA1 == null)
                {
                    pCBA1 = new PCBAEX();
                }
            }
            catch (Exception ex)
            {
            }
            return pCBA1;
        }

        public override void SaveThis(string path)
        {
            base.SaveThis(path);
        }

        public Dictionary<string, Library.LibraryVisionBase> DictRoi { get; set; } = new Dictionary<string, Library.LibraryVisionBase>();

        public override bool RunHProgram(OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj)
        {
            oneRObjs = new List<OneRObj>();
            Dictionary<string, bool> keyValue = new Dictionary<string, bool>();
            bool OK = true;
            foreach (var item in DictRoi)
            {
                item.Value.Name = item.Key;
                AoiObj rok = item.Value.Run(oneResultOBj);
                if (!rok.RestBool)
                {
                    oneResultOBj.AddImageMassage(rok.AoiRow + 60, rok.AoiCol, rok.CiName);
                    //    //oneResultOBj.AddNGOBJ(new OneRObj() { NGText = this.Name + "." + item.Key,ROI=ErrROI,NGROI= ErrROI });
                    OK = false;
                }
                keyValue.Add(item.Key, rok.RestBool);
            }
            return OK;
        }
    }
}