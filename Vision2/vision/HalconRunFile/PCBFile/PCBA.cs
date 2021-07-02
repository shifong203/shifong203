using System.Collections.Generic;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.Controls;
using HalconDotNet;
using System.Reflection;
using System.IO;
using Vision2.vision.HalconRunFile.PCBFile;
using System;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public class PCBA : RunProgram
    {
        public override Control GetControl(HalconRun halcon)
        {
            return new PCBAControl(this);
        }
        public override RunProgram UpSatrt<T>(string path)
        {
            RunProgram pCBA = base.ReadThis<PCBA>(path);
            PCBA pCBA1 = pCBA as PCBA;
            try
            {
                if (pCBA1 == null)
                {
                    pCBA1 = new PCBA();
                }
                Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
                Dictionary<string, RunProgram> DictR = new Dictionary<string, RunProgram>();
                path = Path.GetDirectoryName(path);
                foreach (var item in pCBA1.DicPCBType)
                {
                    try
                    {
                        if (item.Key!="")
                        {
                            dynamic obj = assembly.CreateInstance(item.Value); // 创建类的实例                            
                            if (obj != null)
                            {
                                DictR.Add(item.Key, obj.UpSatrt<RunProgram>(path + "\\" + item.Key+"\\"+item.Key));
                                DictR[item.Key].GetRunProgram(this);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                pCBA1.DictRoi = DictR;
            }
            catch (Exception ex)
            {
            }
            return pCBA1;
        }

        public override void SaveThis(string path)
        {
            foreach (var item in DictRoi)
            {

                if (!DicPCBType.ContainsKey(item.Key))
                {
                    DicPCBType.Add(item.Key, item.Value.GetType().ToString());
                }
                item.Value.Name = item.Key;
                InterfacePCBA interfacePCBA =item.Value as InterfacePCBA;
                if (interfacePCBA!=null)
                {
                    interfacePCBA.SaveThis(path + "\\"+this.Name);
                }

            }

            base.SaveThis(path);
        }
        public Dictionary<string, RunProgram> GetDicPCBA()
        {

            return DictRoi;
        }
        Dictionary <string, RunProgram> DictRoi { get; set; } = new Dictionary<string, RunProgram>();

        public Dictionary<string, string> DicPCBType { get; set; } = new Dictionary<string, string>();
        public override bool RunHProgram( OneResultOBj oneResultOBj, out List< OneRObj> oneRObj, AoiObj aoiObj)
        {
            oneRObj = new List<OneRObj>();
            Dictionary<string, bool> keyValue = new Dictionary<string, bool>();
            bool OK = true;
            foreach (var item in DictRoi)
            {
                item.Value.Name = item.Key;
                bool rok=item.Value.Run( oneResultOBj);
                 if (!rok)
                 {
                    //oneResultOBj.AddNGOBJ(new OneRObj() { NGText = this.Name + "." + item.Key,ROI=ErrROI,NGROI= ErrROI });
                   OK = false;
                 }
                keyValue.Add(item.Key, rok);
            }
            return OK;
        }
     

   

    }
}
