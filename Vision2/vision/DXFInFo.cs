using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.vision
{
    public class DXFInFo
    {
        public double Api { get; set; } = 0;

        public double Row { get; set; } = 0;

        public double Col { get; set; } = 0;

        public double ScaleX { get; set; } = 1;
        public double ScaleY { get; set; } = 1;
        public HObject DXF { get; set; }
        public HObject DXFMode { get; set; }
        public HObject ReadDxf()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "请选择dxf文件";
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "dxf文件|*.dxf;";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.Length == 0) return DXF;
            try
            {
                HOperatorSet.ReadContourXldDxf(out HObject hObject, openFileDialog.FileNames[0], new HTuple(), new HTuple(), out HTuple dxfStratus);
                DXFMode = hObject;
                return SetDXF();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return DXF;
        }
        public HObject SetDXF(HObject hObjectT=null)
        {
            try
            {
                HOperatorSet.HomMat2dIdentity(out HTuple HomMat2DIdentity);
                HOperatorSet.HomMat2dRotate(HomMat2DIdentity, new HTuple(Api).TupleRad(), 0, 0, out HomMat2DIdentity);
                HOperatorSet.HomMat2dScale(HomMat2DIdentity, ScaleX, ScaleY, 0, 0, out HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(HomMat2DIdentity, Row, Col, out HomMat2DIdentity);
                if (hObjectT==null)
                {
                    hObjectT = DXFMode;
                }
                HOperatorSet.AffineTransContourXld(hObjectT, out HObject hObject, HomMat2DIdentity);
                DXF = hObject;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return DXF;
        }
        public HTuple HomMat2DIdentity;
        public HObject GetObject(HObject hObjectT)
        {
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            try
            {
                HOperatorSet.HomMat2dIdentity(out  HomMat2DIdentity);
                HOperatorSet.HomMat2dRotate(HomMat2DIdentity, new HTuple(Api).TupleRad(), 0, 0, out HomMat2DIdentity);
                HOperatorSet.HomMat2dScale(HomMat2DIdentity, ScaleX, ScaleY, 0, 0, out HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(HomMat2DIdentity, Row, Col, out HomMat2DIdentity);
                HOperatorSet.AffineTransRegion(hObjectT, out hObject, HomMat2DIdentity, "nearest_neighbor");
                //HOperatorSet.AffineTransContourXld(hObjectT, out  hObject, HomMat2DIdentity);
                hObjectT = hObject;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
            }
            return hObjectT;
        }
        public bool GetHtupe(HTuple hTX, HTuple hTY,out HTuple htuX,out HTuple htuY)
        {
            htuX= htuY = new HTuple();
            try
            {
                HOperatorSet.HomMat2dIdentity(out HomMat2DIdentity);
                HOperatorSet.HomMat2dRotate(HomMat2DIdentity, new HTuple(Api).TupleRad(), 0, 0, out HomMat2DIdentity);
                HOperatorSet.HomMat2dScale(HomMat2DIdentity, ScaleX, ScaleY, 0, 0, out HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(HomMat2DIdentity, Row, Col, out HomMat2DIdentity);
                HOperatorSet.AffineTransPoint2d(HomMat2DIdentity, hTX, hTY, out htuX, out htuY);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
    }

}
