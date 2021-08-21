using HalconDotNet;
using System;
using System.Collections.Generic;

namespace Vision2.vision.RestVisionForm
{
    /// <summary>
    /// 生产信息
    /// </summary>
    [Serializable]
    public class PrestC
    {
        /// <summary>
        /// 产品图
        /// </summary>
        //public HObject Image { get; set; }

        public Dictionary<string, HObject> P1keysOBj { get; set; } = new Dictionary<string, HObject>();

        public string PCNamePath { get; set; }

        public string ImageModePath { get; set; }

        public string LogeName { get; set; }

        public string LogePass { get; set; }

        /// <summary>
        /// 生产名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string LineName { get; set; }

        public int XNumber { get; set; }
        public int YNumber { get; set; }
    }
}