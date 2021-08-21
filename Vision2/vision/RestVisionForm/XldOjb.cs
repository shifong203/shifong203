using HalconDotNet;

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