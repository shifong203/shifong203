using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    public  class BPCBoJB
    {
        public BPCBoJB()
        {
            TestingRoi.GenEmptyObj();
        }
        public const string SuffixName = ".dat";

        public HObject TestingRoi = new HObject();
        public string TypeStr { get; set; }

        public bool RestBool { get; set; }
        public int ErrNumber { get; set; }
        public string Name { get; set; } = "";


        public RunProgram GetRunProgram(RunProgram run=null )
        {
            if (run!=null)
            {
                RunProgramT = run;
            }
            return RunProgramT;
        }
        RunProgram RunProgramT;
        /// <summary>
        /// 读取地址文件创建实例
        /// </summary>
        /// <typeparam name="T">T改为自己的类实例</typeparam>
        /// <param name="Path"></param>
        /// <returns></returns>
        public BPCBoJB ReadThis<T>(string Path) where T : new()
        {
            T tshi;
            if (File.Exists(Path + SuffixName))
            {
                HalconRun.ReadPathJsonToCalss(Path + SuffixName, out tshi);
            }
            else if (File.Exists(Path + ".txt"))
            {
                HalconRun.ReadPathJsonToCalss(Path + ".txt", out tshi);
            }
            else
            {
                return null;
            }
            return tshi as BPCBoJB;
        }
        public virtual bool Run(HalconRun halcon, RunProgram run,OneResultOBj oneResultOBj, out  HObject ErrRoi)
        {
            ErrRoi = new HObject();
            ErrRoi.GenEmptyObj();
            return false;
        }
  
        public virtual BPCBoJB UpSatrt<T>(string path) where T : new()
        {

            return ReadThis<T>(path);
        }
    }
}
