using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision
{
    /// <summary>
    /// 单次拍照
    /// </summary>
    public class OneResultOBj
    {
        public bool OK
        {
            get
            {
                for (int i = 0; i < NGObj.Count; i++)
                {
                    if (!NGObj[i].OK)
                    {
                        return false;
                    }
                }
                return true;
            }
            set
            {
                if (value)
                {
                    foreach (var item in NGObj)
                    {
                        item.OK = true;
                    }
                }
                autoOk = value;
            }
        }
        bool autoOk;

        public bool Done
        {
            get
            {
                for (int i = 0; i < NGObj.Count; i++)
                {
                    if (!NGObj[i].Done)
                    {
                        return false;
                    }
                }
                return true;
            }
            set
            {
                for (int i = 0; i < NGObj.Count; i++)
                {
                    NGObj[i].Done = value;
                }
            }
        }

        public HalconResult HalconResult;

        public List<OneRObj> NGObj = new List<OneRObj>();
        public void AddNGOBJ(OneRObj rObj)
        {
            this.NGObj.Add(rObj);
            this.HalconResult.AddNGObj(rObj);
        }

        public void AddNGOBJ(string component, string nGText, HObject roi,HObject err)
        {
            OneRObj rObj = new OneRObj() { NGText = nGText, ComponentID = component, ROI = roi, NGROI = err };
            this.NGObj.Add(rObj);
            this.HalconResult.AddNGObj(rObj);
        }
        public void ADDRed(string name,  string ngText, HObject hObject, HObject roi = null)
        {
            if (roi == null)
            {
                roi = hObject;
            }
            AddNGOBJ( name,  ngText,   Vision.XLD_To_Region(hObject), roi );
        }
        ///// <summary>
        ///// 元件名称
        ///// </summary>
        //public string ComponentID { get; set; }
        /// <summary>
        /// 执行ID
        /// </summary>
        public int RunID { get; set; }
        /// <summary>
        /// 库ID
        /// </summary>
        public int LiyID { get; set; }
        /// <summary>
        /// 程序名
        /// </summary>
        public string RunName { get; set; }
    }

}
