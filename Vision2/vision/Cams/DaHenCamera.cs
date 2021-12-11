using GxIAPINET;
using HalconDotNet;
using htmlMaker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThridLibray;
using Vision2.ErosProjcetDLL.Project;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.RunProgramFile.HalconRun;

namespace Vision2.vision.Cams
{
    public  class DaHenCamera :Cams.Camera, ProjectNodet.IClickNodeProject
    {
    
        bool m_bIsColor = false;

        int a = 0;
        string strValue = null;

         static  IGXFactory m_objIGXFactory = null;

        public DaHenCamera()
        {
            if (m_objIGXFactory==null)
            {
                m_objIGXFactory = IGXFactory.GetInstance();
                m_objIGXFactory.Init();
            }
        }
        IGXFeatureControl m_objIGXFeatureControl = null;
        IGXStream m_objIGXStream = null;
        IGXDevice m_objIGXDevice = null;

        /// <summary>
        /// 相机曝光时间
        /// </summary>
        public override double ExposureTime
        {
            get
            {
                return expoursetime;
            }
            set
            {
                if (expoursetime == value)
                {
                    return;
                }
                if (m_objIGXFeatureControl != null)
                {
                    m_objIGXFeatureControl.GetFloatFeature("ExposureTime").SetValue(expoursetime);//单帧
              
                    //using (IFloatParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.ExposureTime])
                    //{
                    //    p.SetValue(value);
                    //}
                }
                expoursetime = value;
            }
        }
        public override double Gamma
        {
            get => base.Gamma;
            set
            {
                if (gamma == value)
                {
                    return;
                }
                if (m_objIGXFeatureControl!=null)
                {
                    try
                    {
                        m_objIGXFeatureControl.GetFloatFeature("Gamma").SetValue(gamma);
                    }
                    catch (Exception)
                    {
                    }
               
                }
                gamma = value;
            }
        }

        public override double Gain
        {
            get
            {
                return gain;
            }
            set
            {
                if (gain == value)
                {
                    return;
                }
                if (m_objIGXFeatureControl != null)
                {
                    try
                    {
                        m_objIGXFeatureControl.GetFloatFeature("Gain").SetValue(gain);
                    }
                    catch (Exception)
                    {
                    }
                    //using (IFloatParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.GainRaw])
                    //{
                    //    p.SetValue(value);
                    //}
                }
                gain = value;
            }
        }
        HObject ho_Image;
        public override void CloseCam()
        {
            base.CloseCam();
      
            try
            {
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.Close();
                    m_objIGXStream = null;
                }
            }
            catch (Exception)
            {

            }
            this.OnLinkEnvet(false);
            IsCamConnected = false;
            __closeDevice();
        }

        private void __closeDevice()
        {
            try
            {
                if (null != m_objIGXDevice)
                {
                    m_objIGXDevice.Close();
                    m_objIGXDevice = null;
                }
            }
            catch (Exception)
            {
            }
        }
        //void __InitDevice()
        //{
         
        //}
       static     List<IGXDeviceInfo> listIGXDeviceInfo;
        public static void FindCams()
        {
            listIGXDeviceInfo = new List<IGXDeviceInfo>();
            m_objIGXFactory.UpdateDeviceList(200, listIGXDeviceInfo);

            if (listIGXDeviceInfo.Count <= 0)
            {
                MessageBox.Show("没有找到相机！");
                return;
            }


        }
        public override void OpenCam()
        {
            try
            {
            IsOneCam= OneCam;

            listIGXDeviceInfo =   new List<IGXDeviceInfo>();
            this.CloseCam();
            __closeDevice();
            m_objIGXFactory.UpdateDeviceList(200, listIGXDeviceInfo);

            //if (listIGXDeviceInfo.Count <= 0)
            //{
            //    MessageBox.Show("没有找到相机！");
            //    return;
            //}
            m_objIGXDevice = m_objIGXFactory.OpenDeviceBySN(this.SerialNum, GX_ACCESS_MODE.GX_ACCESS_EXCLUSIVE);
            m_objIGXFeatureControl = m_objIGXDevice.GetRemoteFeatureControl();

                //m_objIGXFeatureControl.GetBoolFeature("ChunkModeActive").SetValue(true);
                //m_objIGXFeatureControl.GetBoolFeature("ChunkEnable").SetValue(true);
                if (this.TriggerSource==TriggerSoEnum.Software)
                {
                    m_objIGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("Off");
                }
                else
                {
                    m_objIGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("On");
                }
                m_objIGXFeatureControl.GetEnumFeature("TriggerSource").SetValue(this.TriggerSource.ToString());
                //m_objIGXFeatureControl.GetCommandFeature("TriggerSoftware").Execute();
              
                    m_objIGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue("SingleFrame");//单帧
                

                if (m_objIGXDevice.GetRemoteFeatureControl().IsImplemented("PixelColorFilter"))
            {
                strValue = m_objIGXDevice.GetRemoteFeatureControl().GetEnumFeature("PixelColorFilter").GetValue();

                if ("None" != strValue)
                {
                    m_bIsColor = true;
                    //ThreeMenuItem.Enabled = true;
                }
            }
            if (null != m_objIGXDevice)
            {
                m_objIGXStream = m_objIGXDevice.OpenStream(0);
                    //if (null != m_objIGXFeatureControl)
                    //{
                    //    m_objIGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue("Continuous");
                    //} 
                    IsCamConnected = true;
                    this.OnLinkEnvet(true);
                    if (null != m_objIGXStream)
                    {
                        if (OneCam)
                        {
                            m_objIGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue("Continuous");//单帧
                            m_objIGXStream.RegisterCaptureCallback(this, __CaptureCallbackPro);
                            m_objIGXStream.StartGrab();
                            m_objIGXFeatureControl.GetCommandFeature("AcquisitionStart").Execute();
                        }
                    }
                }
   
                return;
            }
            catch (Exception ex)
            {
            }
            this.OnLinkEnvet(false);
        }
        public override bool GetImage(out HObject image)
        {
            image = new HObject();
            image.GenEmptyObj();
            Watch.Restart();
            if (!this.Grabbing)
            {
                Vision.TriggerSetup(this.FlashLampName, true.ToString());
                if (FlashLampName != null && FlashLampName != "")
                {
                    Thread.Sleep(FlashLampTime);
                }
            }
            lock (this)
            {
                if (m_objIGXFeatureControl != null)
                {
                    if (!IsCamConnected)
                    {
                        AlarmText.AddTextNewLine(this.name + "短线重连");
                        OpenCam();
                    }
                    try
                    {
                        if (!OneCam)
                        {
                            //开启流通道采集
                            m_objIGXStream.StartGrab();
                            //给设备发送开采命令
                            m_objIGXFeatureControl.GetCommandFeature("AcquisitionStart").Execute();
                            //采单帧
                            IImageData objImageData = null;
                            //超时时间使用 500ms，用户可以自行设定
                            objImageData = m_objIGXStream.GetImage(500);
                            if (objImageData.GetStatus() == GX_FRAME_STATUS_LIST.GX_FRAME_STATUS_SUCCESS)
                            {
                                //采图成功而且是完整帧，可以进行图像处理...
                                if (m_bIsColor)
                                {
                                    HOperatorSet.GenImageInterleaved(out ho_Image, objImageData.ConvertToRGB24(GX_VALID_BIT_LIST.GX_BIT_0_7,
                                        GX_BAYER_CONVERT_TYPE_LIST.GX_RAW2RGB_ADAPTIVE, false), "bgr", objImageData.GetWidth(),
                                        objImageData.GetHeight(), -1, "byte", 0, 0, 0, 0, -1, 0);
                                }
                                else
                                {
                                    ///黑白相机，水星IFrameData装换为halcon的ho_Image
                                    IntPtr pBufferImage;
                                    pBufferImage = IntPtr.Zero;
                                    pBufferImage = objImageData.GetBuffer();
                                    HOperatorSet.GenImage1(out image, "byte", objImageData.GetWidth(), objImageData.GetHeight(), pBufferImage);
                                }
                            }
                            objImageData.Destroy();//销毁 objImageData 对象
                                                   //停采
                            m_objIGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                            m_objIGXStream.StopGrab();
                            Watch.Stop();
                            RunTime = Watch.ElapsedMilliseconds;
                            if (!this.Grabbing)
                            {
                                Vision.TriggerSetup(this.FlashLampName, false.ToString());
                            }
                            //关闭流通道
                            //m_objIGXStream.Close();
                            return true;
                        }
                        else
                        {
                            return true;
                            //m_objIGXFeatureControl.GetCommandFeature("TriggerSoftware").Execute();
                        }
                    }
                    catch (Exception ex)
                    {
                        AlarmText.AddTextNewLine("采图错误" + ex.Message);
                        IsCamConnected = false;
                    }
                }
                else
                {
                   AlarmText.AddTextNewLine("程序未关联相机" + this.name);
                }
                return false;
            }
        }
        public override bool GetImage(out ImagesOneRun image)
        {
            image = null;
            try
            {
                GetImage(out HObject hObject);
                image = new ImagesOneRun(hObject);
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        private void __CaptureCallbackPro(object objUserParam, IFrameData objIFrameData)
        {
            try
            {
                if (!this.Grabbing)
                {
                    this.OnTriggerCon(true);
                }
             
                Watch.Restart();
                try
                {
            
                    ///彩色相机，水星IFrameData装换为halcon的ho_Image
                    if (m_bIsColor)
                    {
                        //HOperatorSet.GenImageInterleaved(out ho_Image, objIFrameData.ConvertToRGB24(GX_VALID_BIT_LIST.GX_BIT_0_7, GX_BAYER_CONVERT_TYPE_LIST.GX_RAW2RGB_ADAPTIVE, false), "bgr", objIFrameData.GetWidth(), objIFrameData.GetHeight(), -1, "byte", 0, 0, 0, 0, -1, 0);
                        //HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, objIFrameData.GetHeight() - 1, objIFrameData.GetWidth() - 1);
                        HOperatorSet.GenImageInterleaved(out ho_Image, objIFrameData.ConvertToRGB24(GX_VALID_BIT_LIST.GX_BIT_0_7,
                            GX_BAYER_CONVERT_TYPE_LIST.GX_RAW2RGB_ADAPTIVE, false), "bgr", objIFrameData.GetWidth(),
                            objIFrameData.GetHeight(), -1, "byte", 0, 0, 0, 0, -1, 0);
                    }
                    else
                    {
                        ///黑白相机，水星IFrameData装换为halcon的ho_Image
                        IntPtr pBufferImage;
                        pBufferImage = IntPtr.Zero;
                        pBufferImage = objIFrameData.GetBuffer();
                        HOperatorSet.GenImage1(out ho_Image, "byte", objIFrameData.GetWidth(), objIFrameData.GetHeight(), pBufferImage);
                    }
                    OneResultOBj oneResultOBj = new OneResultOBj();
                    oneResultOBj.Image = ho_Image;
                    Watch.Stop();
                    RunTime = Watch.ElapsedMilliseconds;
                    oneResultOBj.RunID = 1;
                    this.OnEnverIamge("1", 1, oneResultOBj);

                }
                catch (Exception ex)
                {
                    ErosProjcetDLL.Project.ErrForm.Show(ex);
                }
           
            }
            catch (Exception)
            {
            }
        }


        public override void Stop()
        {
            base.Stop();
            m_objIGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("On");
        }
        public override void Straing(HalconRun halconRun)
        {
          
            if (Grabbing)
            {
                return;
            }
            base.Straing(halconRun);

            if (OneCam)
            {
                m_objIGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("Off");
                return;
            }
            Thread thread = new Thread(() =>
            {
                while (Grabbing)
                {
                    try
                    {
                        halconRun.HobjClear();
                        if (this.GetImage(out HObject image))
                        {
                            halconRun.Image(image);
                            halconRun.AddMeassge("采图时间:" + RunTime);
                            halconRun.ShowObj();
                        }
                        else
                        {
                            halconRun.Image().GenEmptyObj();
                            halconRun.AddMeassge("采图失败:" + RunTime);
                            halconRun.ShowObj();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            });
            thread.Priority = ThreadPriority.Highest;
            thread.IsBackground = true;
            thread.Start();
        }

        public Control GetThisControl()
        {
            throw new NotImplementedException();
        }
    }
}
