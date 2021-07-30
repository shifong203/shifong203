using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision2.vision.RestVisionForm
{
    public class XldOjb
    {
        public XldOjb(HObject hObject)
        {
            XLd = hObject;
        }
        public bool OK = false;
        public bool Done = false;
        public HObject XLd = new HObject();
    }   
}
