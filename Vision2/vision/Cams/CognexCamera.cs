//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Cognex.VisionPro;
//using Cognex.VisionPro.FGGigE;
//using Cognex.VisionPro.ToolBlock;

//namespace NokidaE.vision.Cams
//{
//    public class CognexCamera
//    {
//        public string _path;

//        public string _name;

//        public CogAcqFifoTool _camera;

//        private int numAcqs ;

//        CogToolBlock _tool;

//        CogRecordDisplay cogRecordDisplay1;

//        ICogAcqFifo macqfifo;//定义相机对象类型

//        CogImage8Grey myImage;//定义照片类型（这里是黑白的）


//        public CognexCamera()
//        {

//        }

//        public CognexCamera(string path, string name)
//        {
//            _path = path;
//            _name = name;
//        }

//        public void LoadCamera()
//        {
//            _camera = CogSerializer.LoadObjectFromFile(_path) as CogAcqFifoTool;
 

//        }
//        public  static ICogFrameGrabber Seek()
//        {
//            CogFrameGrabberGigEs mf2 = new CogFrameGrabberGigEs();//获取已连接相机列表
//            if (mf2.Name!=null)
//            {
//                ICogFrameGrabber mber = mf2[0];//取相机列表中的第一个相机
//                return mber;
//            }

//            return null;

        
//        }
//       /// <summary>
//       /// 触发拍照
//       /// </summary>
//       /// <returns></returns>
//        public bool Trigger()
//        {
//            _camera.Run();
//            if (_camera.RunStatus.Result == CogToolResultConstants.Accept)
//            {
//                cogRecordDisplay1.Image = null;
//                cogRecordDisplay1.StaticGraphics.Clear();
//                cogRecordDisplay1.InteractiveGraphics.Clear();
//                cogRecordDisplay1.Fit(true);
//                cogRecordDisplay1.AutoFit = true;
//                _tool.Inputs["InputImage"].Value = _camera.OutputImage;
//                return true;
//            }
//            return false;
//        }
//        /// <summary>
//        /// Bitmap转康耐视图像
//        /// </summary>
//        /// <param name="curBitmap"></param>
//        /// <returns></returns>
//        public CogImage8Grey BitmapToCogImage8Grey(Bitmap curBitmap)
//        {
//            // 定义处理区域
//            Rectangle rect = new Rectangle(0, 0, curBitmap.Width, curBitmap.Height);
//            // 获取像素数据
//            System.Drawing.Imaging.BitmapData bmpData = curBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, curBitmap.PixelFormat);
//            // 获取像素数据指针
//            IntPtr IntPtrPixelData = bmpData.Scan0;
//            // 定义Buffer
//            byte[] PixelDataBuffer = new byte[curBitmap.Width * curBitmap.Height];

//            // 拷贝Bitmap的像素数据的到Buffer

//            System.Runtime.InteropServices.Marshal.Copy(IntPtrPixelData, PixelDataBuffer, 0, PixelDataBuffer.Length);

//            // 创建CogImage8Grey

//            Cognex.VisionPro.CogImage8Grey cogImage8Grey = new Cognex.VisionPro.CogImage8Grey(curBitmap.Width, curBitmap.Height);

//            // 获取CogImage8Grey的像素数据指针

//            IntPtr Image8GreyIntPtr = cogImage8Grey.Get8GreyPixelMemory(Cognex.VisionPro.CogImageDataModeConstants.Read, 0, 0, cogImage8Grey.Width, cogImage8Grey.Height).Scan0;
//            // 拷贝Buffer数据到CogImage8Grey的像素数据区
//            System.Runtime.InteropServices.Marshal.Copy(PixelDataBuffer, 0, Image8GreyIntPtr, PixelDataBuffer.Length);

//            // 显示图像
//            return cogImage8Grey;
    


//        }
//    }
//}
