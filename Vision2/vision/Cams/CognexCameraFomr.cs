//using Basler.Pylon;
//using Cognex.VisionPro;
//using Cognex.VisionPro.FGGigE;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace NokidaE.vision.Cams
//{
//    public partial class CognexCameraFomr : Form
//    {


//        CognexCamera cognexCamera;
//        BaslerCam baslerCam;
//        public CognexCameraFomr()
//        {
//            InitializeComponent();
//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
//            if (baslerCam!=null)
//            {
//                baslerCam.GrabOne();
//            }
//        }

//        private void toolStripButton3_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                CognexCamera.Seek();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
            
//            }


//        }

//        private void toolStripButton4_Click(object sender, EventArgs e)
//        {
//            //CogJobManager mymanger; //定义vpp管理器
//            //String path = "C:\\Users\\Administrator\\Desktop\\1111.vpp";//vpp文件路径
//            //mymanger = (CogJobManager)CogSerializer.LoadObjectFromFile(path);//加载vpp
//            //CogToolGroup mytg = mymanger.Job(0).VisionTool as CogToolGroup;//获取job中的工具组
//            //CogAcqFifoTool mytll = mytg.Tools["CogAcqFifoTool1"] as CogAcqFifoTool;//获取工具组中的CogAcqFifoTool1工具
//            //mytll.Run();//运行工具
//            //cogRecordDisplay1.Image = mytll.OutputImage;//将工具图像显示在控件上

//        }

//        private void button2_Click(object sender, EventArgs e)
//        {
//            if (baslerCam!=null)
//            {
//                baslerCam.StartGrabbing();
//                button2.Enabled = false;
//                button3.Enabled = true;
//            }

//        }

//        private void button3_Click(object sender, EventArgs e)
//        {
//            if (baslerCam != null)
//            {
//                baslerCam.StopGrabbing();
//                button2.Enabled = true;
//                button3.Enabled = false;
//            }
//        }

//        private void button4_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                if (baslerCam.IsOpen)
//                {
//                    foreach (var item in baslerCam.GetCameraInfo())
//                    {
//                        richTextBox1.AppendText(item.Key + ":" + item.Value.ToString() + Environment.NewLine);
//                    }
//                }
//            }
//            catch (Exception)
//            {
//            }
        
//        }

//        private void button5_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                if (baslerCam == null)
//                {
//                    baslerCam = new BaslerCam();
//                }
//                baslerCam.OpenCam();
//                if (baslerCam.IsOpen)
//                {
//                    button2.Enabled = true;
//                    button1.Enabled = true;
//                    button3.Enabled = true;
//                    button4.Enabled = true;
//                    button6.Enabled = true;
//                    button7.Enabled = true;
//                    baslerCam.SetHw(visionUserControl1);
//                    baslerCam.CleanImage();
//                }
//                else
//                {
//                    MessageBox.Show("链接失败");
//                }    
             
             
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//        }

//        private void button7_Click(object sender, EventArgs e)
//        {
//            baslerCam.CloseCam();
//            button7.Enabled = button6.Enabled =
//                button4.Enabled = button3.Enabled = button1.Enabled = button2.Enabled = false;
           
//        }

//        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
//        {

//        }

//        private void tabPage2_Click(object sender, EventArgs e)
//        {

//        }

//        private void toolStripButton1_Click(object sender, EventArgs e)
//        {

//        }
//        /// <summary>

//        /// 公有静态方法，查找单个相机。例如“Basler”

//        /// </summary>

//        public static ICogFrameGrabber FindFrameGrabber(string CameraType)

//        {
//           List<ICameraInfo>dsd=        CameraFinder.Enumerate();

//            CogFrameGrabberGigEs frameGrabbers = new CogFrameGrabberGigEs();

//            foreach (ICogFrameGrabber fg in frameGrabbers)
//            {

//                if (fg.Name.Contains(CameraType))

//                {

//                    return (fg);

//                }

//            }

//            return null;

//        }
//        private void button8_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                FindFrameGrabber("Basler");
//                //   CogFrameGrabberGigEs mframe = new CogFrameGrabberGigEs();
//                //cognexCamera = new CognexCamera();
//                //ICogAcqFifo macqfifo;//定义相机对象类型
//                //CogImage8Grey myImage;//定义照片类型（这里是黑白的）
//                //CogFrameGrabberGigEs mf2 = new CogFrameGrabberGigEs();//获取已连接相机列表
//                //CogFrameGrabbers mFrameGrabbers = new CogFrameGrabbers();
//                //for (int i = 0; i < mFrameGrabbers.Count; i++)
//                //{
//                //         //                   mFrameGrabber = mframe[i];
//                //         //                   string name = mFrameGrabber.Name ；
//                //         //string serialnumber = mFrameGrabber.SerialNumber；
//                //         //String videoformat = mFrameGrabber.AvailableVideoFormats[0];
//                //}
//                //ICogFrameGrabber mber = mf2[0];//取相机列表中的第一个相机
//                //int trigNum;
//                //macqfifo = mber.CreateAcqFifo(mber.AvailableVideoFormats[0], CogAcqFifoPixelFormatConstants.Format8Grey, 0, true);//创建相机对象
//                //myImage = (CogImage8Grey)macqfifo.Acquire(out trigNum);//使用相机对象的acquire方法拍照
//                //                                                       //cogRecordDisplay1.Image = myImage;//使用cogRecordDisplay控件显示出来

//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
           
//            }
 
//        }

//        private void button9_Click(object sender, EventArgs e)
//        {

//        }

//        private void button10_Click(object sender, EventArgs e)
//        {

//        }

//        private void button12_Click(object sender, EventArgs e)
//        {

//        }

//        private void button13_Click(object sender, EventArgs e)
//        {

//        }

//        private void button14_Click(object sender, EventArgs e)
//        {

//        }

//        private void button11_Click(object sender, EventArgs e)
//        {

//        }

//        private void toolStripButton5_Click(object sender, EventArgs e)
//        {
//            OpenCV.Form1 form1 = new OpenCV.Form1();
//            form1.Show();
//        }
//    }
//}
