using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision2.vision
{
   public class ElementCoordinate
    {

        public ElementCoordinate()
        {
            HObject = new HalconDotNet.HObject();
            HObject.GenEmptyObj();

        }

        public HalconDotNet.HObject HObject { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double U { get; set; }
        /// <summary>
        /// 元件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 位号
        /// </summary>
        public string Part_Number { get; set; }

    }
}
