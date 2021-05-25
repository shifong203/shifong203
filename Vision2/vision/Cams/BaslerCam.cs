
using Basler.Pylon;
using HalconDotNet;
//using CommonServiceLocator;
//using MovementVision.Infrastructure.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace NokidaE.vision.Cams
{
    public class BaslerCam
    {
        private Camera camera = null;
        private PixelDataConverter converter = new PixelDataConverter();
        private String strUserID = null;
        int[] FailCount = new int[9];
        IntPtr latestFrameAddress = IntPtr.Zero;
        public long imageWidth = 0;                        // 图像宽
        public long imageHeight = 0;                       // 图像高
        public string TirggerMode = "";
        public double LineRateAbs = 0;                      //外触发帧率
        public double minLineRateAbs = 0;               //最小外触发帧率
        public double maxLineRateAbs = 0;               //最大外触发帧率
        public int numWindowIndex = 0;
        public ConcurrentQueue<HObject> ImageList = new ConcurrentQueue<HObject>();
        private long grabTime = 0;                    // 采集图像时间

        private HObject hPylonImage ;/////////////////////////////////////////////////////////////删除halcon的
    /*    private ICogImage CogImg; */                //VisionPRO图片
        //private IntPtr latestFrameAddress = IntPtr.Zero;
        private Stopwatch stopWatch = new Stopwatch();

        /// 计算采集图像时间自定义委托
        public delegate void delegateComputeGrabTime(long time);
        /// 计算采集图像时间委托事件
        public event delegateComputeGrabTime eventComputeGrabTime;

        /// 图像处理自定义委托
        public delegate void delegateProcessHImage(HObject hImage);/////////////////////////////////////////////////////////////删除halcon的
        //public delegate void delegateProcessHImage(ICogImage CogImg, long n);
        /// 图像处理委托事件
        public event delegateProcessHImage eventProcessImage;/////////////////////////////////////////////////////////////删除halcon的
        //public event delegateProcessHImage eventProcessImage;

        /// if >= Sfnc2_0_0,说明是ＵＳＢ３的相机
        static Version Sfnc2_0_0 = new Version(2, 0, 0);
        /**************************************************************    实例化相机    ******************/
        /// 实例化第一个找到的相机
        public BaslerCam()
        {
            try
            {
                camera = new Camera();
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
        /// 根据相机序列号实例化相机
        //public BaslerCam(string SN)
        //{
        //    camera = new Camera(SN);
        //}
        /// 根据相机UserID实例化相机
        public BaslerCam(string UserID)
        {
            try
            {
                strUserID = UserID;     //掉线重连用
                // 枚举相机列表
                List<ICameraInfo> allCameraInfos = CameraFinder.Enumerate();
                foreach (ICameraInfo cameraInfo in allCameraInfos)
                {
                    if (strUserID == cameraInfo[CameraInfoKey.UserDefinedName])
                    {
                        camera = new Camera(cameraInfo);
                    }
                }
                if (camera == null)
                {
                    MessageBox.Show("未识别到UserID为“" + strUserID + "”的相机！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
        /*****************************************************/
        public ICameraInfo GetCameraInfo()
        {
            return camera.CameraInfo;
        }
        /**************************************************************    相机操作     *******************/
        /// 打开相机+
        public void OpenCam()
        {
            try
            {
                if (camera==null)
                {
                    try
                    {
                        camera = new Camera();
                    }
                    catch (Exception)
                    {
                    }
                    if (camera==null)
                    {
                        MessageBox.Show("未找到相机");
                        return;
                    }
                    OpenCam();
                }
                if (!camera.IsOpen)
                {
                    camera.Close();
                    camera.Open();
                    SetGetCameraParameter();//初始化时的参数
                }


            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
        /// 关闭相机,释放相关资源
        public void CloseCam()
        {
            try
            {
                if (camera != null)
                {
                    camera.Close();
                    camera.Dispose();
                    camera = null;
                }

                if (hPylonImage != null)/////////////////////////////////////////////////////////////删除halcon的
                {
                    hPylonImage.Dispose();
                 }
                 //CogImg
                    //if (CogImg != null)//VisionPRO用
                    //{
                    //    CogImg = null;
                    //}
                    //if (latestFrameAddress != null)
                    //{
                    //    Marshal.FreeHGlobal(latestFrameAddress);
                    //    latestFrameAddress = IntPtr.Zero;
                    //}
                
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
        /// 单张采集
        public bool GrabOne()
        {
            try
            {
                if (camera.StreamGrabber.IsGrabbing)
                {
                    MessageBox.Show("相机当前正处于采集状态！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    camera.Parameters[PLCamera.AcquisitionMode].SetValue("SingleFrame");
                    camera.StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                    stopWatch.Restart();    // ****  重启采集时间计时器   ****
                    return true;
                }
            }
            catch (Exception e)
            {
                ShowException(e);
                return false;
            }
        }
        /// 开始连续采集
        public bool StartGrabbing()
        {
            try
            {
                if (camera.StreamGrabber.IsGrabbing)
                {
                    MessageBox.Show("相机当前正处于采集状态！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                    camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                    stopWatch.Restart();    // ****  重启采集时间计时器   ****
                    return true;
                }
            }
            catch (Exception e)
            {
                ShowException(e);
                return false;
            }
        }
        /// 停止连续采集
        public void StopGrabbing()
        {
            try
            {
                if (camera.StreamGrabber.IsGrabbing)
                {
                    camera.StreamGrabber.Stop();
                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
        /// <summary>
        /// 存取相机用户参数
        /// </summary>
        /// <param name="UserName">Default：默认，UserSet1：用户1.。。。。。</param>
        /// <param name="SaveOrLoad">Save：保存，Load：加载.</param>
        /// <param name="AfterstartIsAutoLoad">是否自动加载用户中的参数</param>
        public void SaveLoadUserParameters(string UserName, string SaveOrLoad, bool AfterstartIsAutoLoad)
        {
            //★★★ ★★★ ★★★
            //相机处于非采集状态时，执行以下操作。
            try
            {
                if (UserName == "Default")
                {
                    if (SaveOrLoad == "Load")
                    {
                        //★★★ 恢复出厂设置
                        camera.Parameters[PLCamera.UserSetSelector].SetValue("Default");
                        camera.Parameters[PLCamera.UserSetLoad].TryExecute();
                    }
                    else
                        MessageBox.Show("此用户只能读取！恢复出厂设置！");

                }
                else if (UserName == "UserSet1")
                {
                    if (SaveOrLoad == "Load")
                    {
                        //★★★ 从UserSet1中加载参数。
                        camera.Parameters[PLCamera.UserSetSelector].SetValue("UserSet1");
                        camera.Parameters[PLCamera.UserSetLoad].TryExecute();
                    }
                    else if (SaveOrLoad == "Save")
                    {
                        //★★★ 保存参数到 UserSet1，相机会有多个UserSet可以用来存储多套参数使用。
                        camera.Parameters[PLCamera.UserSetSelector].SetValue("UserSet1");
                        camera.Parameters[PLCamera.UserSetSave].TryExecute();
                        if (AfterstartIsAutoLoad)
                        {
                            //★★★ 设置相机启动后，自动加载UserSet1中的参数。
                            camera.Parameters[PLCamera.UserSetDefaultSelector].SetValue("UserSet1");
                        }
                        else
                            MessageBox.Show("参数错误！");
                    }
                    else
                        MessageBox.Show("参数错误！");
                }
                else
                {
                    MessageBox.Show("未设置该用户参数");
                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }

        /*********************************************************/

        /**************************************************************    相机参数设置   ********************************************/
        ///相机是否开始采集
        public bool IsGrabing
        {
            get { return camera.StreamGrabber.IsGrabbing; }
        }
        ///相机是否打开
        public bool IsOpen
        {
            get
            {
                if (camera==null)
                {
                    return false;
                }
                return camera.IsOpen; }
        }
        ///心跳时间
        public long HeartBeatTime
        {
            get { return camera.Parameters[PLGigECamera.GevHeartbeatTimeout].GetValue(); }
            set { camera.Parameters[PLGigECamera.GevHeartbeatTimeout].SetValue(value); }
        }
        ///曝光时间
        public long ExposureTime
        {
            get
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    return (long)camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();
                }
                else// For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    return (long)camera.Parameters[PLCamera.ExposureTime].GetValue();
                }
            }
            set
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    camera.Parameters[PLCamera.ExposureAuto].TrySetValue(PLCamera.ExposureAuto.Off); // Set ExposureAuto to Off if it is writable.
                    camera.Parameters[PLCamera.ExposureMode].TrySetValue(PLCamera.ExposureMode.Timed); // Set ExposureMode to Timed if it is writable.
                    long min = (long)camera.Parameters[PLCamera.ExposureTimeRaw].GetMinimum();
                    long max = (long)camera.Parameters[PLCamera.ExposureTimeRaw].GetMaximum();
                    if (value < min)
                    {
                        value = min;
                    }
                    else if (value > max)
                    {
                        value = max;
                    }
                    camera.Parameters[PLCamera.ExposureTimeRaw].SetValue(value);
                }
                else// For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    camera.Parameters[PLCamera.ExposureAuto].TrySetValue(PLCamera.ExposureAuto.Off); // Set ExposureAuto to Off if it is writable.
                    camera.Parameters[PLCamera.ExposureMode].TrySetValue(PLCamera.ExposureMode.Timed); // Set ExposureMode to Timed if it is writable.
                    long min = (long)camera.Parameters[PLCamera.ExposureTime].GetMinimum();
                    long max = (long)camera.Parameters[PLCamera.ExposureTime].GetMaximum();
                    if (value < min)
                    {
                        value = min;
                    }
                    else if (value > max)
                    {
                        value = max;
                    }
                    camera.Parameters[PLCamera.ExposureTime].SetValue(value);
                }
            }
        }
        //亮度
        public double Brightness
        {
            set { camera.Parameters[PLCamera.BslBrightness].SetValue(value); }
            get { return camera.Parameters[PLCamera.BslBrightness].GetValue(); }
        }
        ///增益
        public long Gain
        {
            get
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    return (long)camera.Parameters[PLCamera.GainRaw].GetValue();
                }
                else// For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    return (long)camera.Parameters[PLCamera.Gain].GetValue();
                }
            }
            set
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off); // Set GainAuto to Off if it is writable.
                    long min = (long)camera.Parameters[PLCamera.GainRaw].GetMinimum();
                    long max = (long)camera.Parameters[PLCamera.GainRaw].GetMaximum();
                    if (value < min)
                    {
                        value = min;
                    }
                    else if (value > max)
                    {
                        value = max;
                    }
                    try
                    {
                        camera.Parameters[PLCamera.GainRaw].SetValue(value);
                    }
                    catch (Exception ex) { }
                }
                else// For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    //某些相机型号可能启用了自动功能。 要将增益值设置为特定值，增益自动功能必须先禁用（如果增益自动可用）。
                    camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off); // Set GainAuto to Off if it is writable.
                    long min = (long)camera.Parameters[PLCamera.Gain].GetMinimum();
                    long max = (long)camera.Parameters[PLCamera.Gain].GetMaximum();
                    if (value < min)
                    {
                        value = min;
                    }
                    else if (value > max)
                    {
                        value = max;
                    }
                    camera.Parameters[PLCamera.Gain].SetValue(value);
                }
            }
        }
        ///采集帧率
        public double FrameRate
        {
            get
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    return camera.Parameters[PLCamera.AcquisitionFrameRateAbs].GetValue();
                }
                else// For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    return camera.Parameters[PLCamera.AcquisitionFrameRate].GetValue();
                }
            }
            set
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    double min = camera.Parameters[PLCamera.AcquisitionFrameRateAbs].GetMinimum();
                    double max = camera.Parameters[PLCamera.AcquisitionFrameRateAbs].GetMaximum();
                    if (value < min)
                    {
                        value = min;
                    }
                    else if (value > max)
                    {
                        value = max;
                    }
                    camera.Parameters[PLCamera.AcquisitionFrameRateAbs].SetValue(value);
                }
                else// For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    double min = camera.Parameters[PLCamera.AcquisitionFrameRate].GetMinimum();
                    double max = camera.Parameters[PLCamera.AcquisitionFrameRate].GetMaximum();
                    if (value < min)
                    {
                        value = min;
                    }
                    else if (value > max)
                    {
                        value = max;
                    }
                    camera.Parameters[PLCamera.AcquisitionFrameRate].SetValue(value);
                }
            }
        }
        ///帧率设置是否有效
        public bool FrameRateEnable
        {
            get { return camera.Parameters[PLCamera.AcquisitionFrameRateEnable].GetValue(); }
            set { camera.Parameters[PLCamera.AcquisitionFrameRateEnable].SetValue(value); }
        }
        ///触发延迟时间
        public double TriggerDelayAbs
        {
            get
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    return camera.Parameters[PLCamera.TriggerDelayAbs].GetValue();
                }
                else// For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    return camera.Parameters[PLCamera.TriggerDelay].GetValue();
                }
            }
            set
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    double min = camera.Parameters[PLCamera.TriggerDelayAbs].GetMinimum();
                    double max = camera.Parameters[PLCamera.TriggerDelayAbs].GetMaximum();
                    if (value < min)
                    {
                        value = min;
                    }
                    else if (value > max)
                    {
                        value = max;
                    }
                    camera.Parameters[PLCamera.TriggerDelayAbs].SetValue(value);
                }
                else// For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    double min = camera.Parameters[PLCamera.TriggerDelay].GetMinimum();
                    double max = camera.Parameters[PLCamera.TriggerDelay].GetMaximum();
                    if (value < min)
                    {
                        value = min;
                    }
                    else if (value > max)
                    {
                        value = max;
                    }
                    camera.Parameters[PLCamera.TriggerDelay].SetValue(value);
                }
            }
        }
        ///滤波时间
        public double DebouncerTimeAbs
        {
            get
            { // Set an enum parameter.
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    camera.Parameters[PLCamera.LineSelector].SetValue("Line1");
                    if (camera.Parameters[PLCamera.LineDebouncerTimeAbs].IsReadable)
                    {
                        double temp = camera.Parameters[PLCamera.LineDebouncerTimeAbs].GetValue();
                        return temp;
                    }
                    else
                        return 0;
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    camera.Parameters[PLCamera.LineSelector].SetValue("Line1");
                    if (camera.Parameters[PLCamera.LineDebouncerTime].IsReadable)
                    {
                        return camera.Parameters[PLCamera.LineDebouncerTime].GetValue();
                    }
                    else
                        return 0;
                }
            }
            set
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    camera.Parameters[PLCamera.LineSelector].SetValue("Line1");
                    double min = camera.Parameters[PLCamera.LineDebouncerTimeAbs].GetMinimum();
                    double max = camera.Parameters[PLCamera.LineDebouncerTimeAbs].GetMaximum();
                    if (value < min)
                    {
                        value = min;
                    }
                    else if (value > max)
                    {
                        value = max;
                    }
                    camera.Parameters[PLCamera.LineDebouncerTimeAbs].SetValue(value);
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    camera.Parameters[PLCamera.LineSelector].SetValue("Line1");
                    double min = camera.Parameters[PLCamera.LineDebouncerTime].GetMinimum();
                    double max = camera.Parameters[PLCamera.LineDebouncerTime].GetMaximum();
                    if (value < min)
                    {
                        value = min;
                    }
                    else if (value > max)
                    {
                        value = max;
                    }
                    camera.Parameters[PLCamera.LineDebouncerTime].SetValue(value);
                }
            }
        }
        ///最大缓存数
        public long MaxNumBuffer
        {
            get
            {
                if (camera.Parameters[PLStream.MaxNumBuffer].IsReadable)
                {
                    return camera.Parameters[PLStream.MaxNumBuffer].GetValue();
                }
                else
                    return 0;
            }
            set
            {
                long min = camera.Parameters[PLStream.MaxNumBuffer].GetMinimum();
                long max = camera.Parameters[PLStream.MaxNumBuffer].GetMaximum();
                if (value < min)
                {
                    value = min;
                }
                else if (value > max)
                {
                    value = max;
                }
                camera.Parameters[PLStream.MaxNumBuffer].SetValue(value);
            }
        }
        /// 设置相机Freerun模式
        public void SetFreerun()
        {
            try
            {
                // Set an enum parameter.
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
                            TirggerMode = "Free";
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
                        }
                    }
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
                        }
                    }
                }
                stopWatch.Restart();    // ****  重启采集时间计时器   ****
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
        /// 设置相机软触发模式
        public void SetSoftwareTrigger()
        {
            try
            {
                // Set an enum parameter.
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                            TirggerMode = "Software";
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                        }
                    }
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                        }
                    }
                }
                stopWatch.Reset();    // ****  重置采集时间计时器   ****
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
        /// 发送软触发命令
        public void SendSoftwareExecute()
        {
            try
            {
                if (camera.WaitForFrameTriggerReady(1000, TimeoutHandling.ThrowException))
                {
                    camera.ExecuteSoftwareTrigger();
                    stopWatch.Restart();    // ****  重启采集时间计时器   ****
                }
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }
        /// 设置相机外触发模式
        public void SetExternTrigger()
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
                            TirggerMode = "Line1";
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
                        }
                    }

                    ////Sets the trigger delay time in microseconds.
                    //camera.Parameters[PLCamera.TriggerDelayAbs].SetValue(5);        // 设置触发延时

                    ////Sets the absolute value of the selected line debouncer time in microseconds
                    //camera.Parameters[PLCamera.LineSelector].TrySetValue(PLCamera.LineSelector.Line1);
                    //camera.Parameters[PLCamera.LineMode].TrySetValue(PLCamera.LineMode.Input);
                    //camera.Parameters[PLCamera.LineDebouncerTimeAbs].SetValue(5);       // 设置去抖延时，过滤触发信号干扰

                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
                        }
                    }

                    ////Sets the trigger delay time in microseconds.//float
                    //camera.Parameters[PLCamera.TriggerDelay].SetValue(5);       // 设置触发延时

                    ////Sets the absolute value of the selected line debouncer time in microseconds
                    //camera.Parameters[PLCamera.LineSelector].TrySetValue(PLCamera.LineSelector.Line1);
                    //camera.Parameters[PLCamera.LineMode].TrySetValue(PLCamera.LineMode.Input);
                    //camera.Parameters[PLCamera.LineDebouncerTime].SetValue(5);       // 设置去抖延时，过滤触发信号干扰

                }
                stopWatch.Reset();    // ****  重置采集时间计时器   ****
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
        /// 设置外触发帧率
        public void SetLineRateAbs(double value)
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    // Some parameters have restrictions. You can use GetIncrement/GetMinimum/GetMaximum to make sure you set a valid value.                              
                    // In previous SFNC versions, GainRaw is an integer parameter.
                    // integer parameter的数据，设置之前，需要进行有效值整合，否则可能会报错
                    double min = camera.Parameters[PLCamera.AcquisitionLineRateAbs].GetMinimum();
                    double max = camera.Parameters[PLCamera.AcquisitionLineRateAbs].GetMaximum();
                    //double incr = camera.Parameters[PLCamera.LineDebouncerTimeAbs].GetIncrement().Value;
                    if (value < min)
                    {
                        value = min;
                    }
                    else if (value > max)
                    {
                        value = max;
                    }
                    else
                    {
                        //value = min + (((value - min) / incr) * incr);
                        value = value + 0;
                    }
                    camera.Parameters[PLCamera.AcquisitionLineRateAbs].SetValue(value);

                    //// Or,here, we let pylon correct the value if needed.
                    //camera.Parameters[PLCamera.GainRaw].SetValue(value, IntegerValueCorrection.Nearest);
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    // USB相机没有此参数

                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
        /// 获取外触发帧率
        public void GetLineRateAbs()
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    if (camera.Parameters[PLCamera.AcquisitionLineRateAbs].IsReadable)
                    {
                        LineRateAbs = camera.Parameters[PLCamera.AcquisitionLineRateAbs].GetValue();
                        minLineRateAbs = camera.Parameters[PLCamera.AcquisitionLineRateAbs].GetMinimum();
                        maxLineRateAbs = camera.Parameters[PLCamera.AcquisitionLineRateAbs].GetMaximum();
                    }
                }
                else
                {
                    //USB相机无此参数
                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }

        /***************************************************************************************************************************/
        HWindowControl HWindowControl1;
        public void SetHw(HWindowControl hWindowControl)
        {
            HWindowControl1 = hWindowControl;
        }
        /****************  图像响应事件函数  ****************/
        int Imagecount = 0;
        // 相机取像回调函数.
        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            //IntPtr latestFrameAddress = IntPtr.Zero;
            try
            {
                HObject hObject=GradImageHalcon(e);
                HOperatorSet.GetImageSize(hObject, out HTuple width, out HTuple height);
                HOperatorSet.SetPart(HWindowControl1.HalconWindow, 0, 0, width - 1, height - 1);
                //HWindowControl1.SetFullImagePart(.);

                HOperatorSet.DispObj(hObject, HWindowControl1.HalconWindow);

             
                // Acquire the image from the camera. Only show the latest image. The camera may acquire images faster than the images can be displayed.
                //从相机中获取图像。 只显示最新的图像。 相机可能会以比可以显示图像更快的速度获取图像。
                // Get the grab result.
                //IGrabResult grabResult = e.GrabResult;//获取采集结果
                //// 采集结束后判断是否有图可读
                //if (grabResult.GrabSucceeded)
                //{
                //    grabTime = stopWatch.ElapsedMilliseconds;
                //    stopWatch.Restart();
                //    if (eventComputeGrabTime != null)
                //        eventComputeGrabTime(grabTime);


                //    // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
                //    //如果相机非常快速地获取图像，请将显示的图像数量减少到合理的数量。
                //    // ****  降低显示帧率，减少CPU占用率  **** //

                //    if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)
                //    {

                //        //stopWatch.Restart();
                //        // 判断是否是黑白图片格式
                //        if (grabResult.PixelTypeValue == PixelType.Mono8)
                //        {

                //            ////allocate the m_stream_size amount of bytes in non-managed environment  在非托管环境中分配m_stream_size字节量
                //            //if (latestFrameAddress == IntPtr.Zero)
                //            //{
                //            //    latestFrameAddress = Marshal.AllocHGlobal((Int32)grabResult.PayloadSize);
                //            //}
                //            //converter.OutputPixelFormat = PixelType.Mono8;
                //            //converter.Convert(latestFrameAddress, grabResult.PayloadSize, grabResult);
                //            // 转换为Halcon图像显示
                //            /////////////////////////////////////////////////////////////删除halcon的
                //            //HOperatorSet.GenImage1(out hPylonImage, "byte", (HTuple)grabResult.Width, (HTuple)grabResult.Height, (HTuple)2);
                //            Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format8bppIndexed);
                //            // Lock the bits of the bitmap.
                //            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                //            // Place the pointer to the buffer of the bitmap.
                //            converter.OutputPixelFormat = PixelType.Mono8;
                //            IntPtr ptrBmp = bmpData.Scan0;
                //            converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult); //Exception handling TODO
                //            bitmap.UnlockBits(bmpData);
                //            //CogImg = new CogImage8Grey(bitmap);
                //            // 定义Buffer
                //            byte[] PixelDataBuffer = new byte[grabResult.Width * grabResult.Height];//curBitmap.Width * curBitmap.Height];
                //            // 拷贝Bitmap的像素数据的到Buffer
                //            System.Runtime.InteropServices.Marshal.Copy(ptrBmp, PixelDataBuffer, 0, PixelDataBuffer.Length);
                //            // 创建CogImage8Grey
                //            Cognex.VisionPro.CogImage8Grey cogImage8Grey = new Cognex.VisionPro.CogImage8Grey(grabResult.Width, grabResult.Height);// (curBitmap.Width, curBitmap.Height);
                //            // 获取CogImage8Grey的像素数据指针
                //            IntPtr Image8GreyIntPtr = cogImage8Grey.Get8GreyPixelMemory(Cognex.VisionPro.CogImageDataModeConstants.Read, 0, 0, grabResult.Width, grabResult.Height).Scan0;
                //            // 拷贝Buffer数据到CogImage8Grey的像素数据区
                //            System.Runtime.InteropServices.Marshal.Copy(PixelDataBuffer, 0, Image8GreyIntPtr, PixelDataBuffer.Length);
                //            CogImg = cogImage8Grey;
                //            bitmap.Dispose();
                //        }
                //        else if (grabResult.PixelTypeValue == PixelType.BayerBG8 || grabResult.PixelTypeValue == PixelType.BayerGB8
                //                    || grabResult.PixelTypeValue == PixelType.BayerRG8 || grabResult.PixelTypeValue == PixelType.BayerGR8)
                //        {
                //            //int imageWidth = grabResult.Width - 1;
                //            //int imageHeight = grabResult.Height - 1;
                //            //int payloadSize = imageWidth * imageHeight;

                //            ////allocate the m_stream_size amount of bytes in non-managed environment 
                //            //if (latestFrameAddress == IntPtr.Zero)
                //            //{
                //            //    latestFrameAddress = Marshal.AllocHGlobal((Int32)(3 * payloadSize));
                //            //}
                //            //converter.OutputPixelFormat = PixelType.BGR8packed;     // 根据bayer格式不同切换以下代码
                //            ////converter.OutputPixelFormat = PixelType.RGB8packed;
                //            //converter.Parameters[PLPixelDataConverter.InconvertibleEdgeHandling].SetValue("Clip");
                //            //converter.Convert(latestFrameAddress, 3 * payloadSize, grabResult);

                //            /////////////////////////////////////////////////////////////删除halcon的
                //            //HOperatorSet.GenImageInterleaved(out hPylonImage, latestFrameAddress, "bgr",
                //            //        (HTuple)imageWidth, (HTuple)imageHeight, -1, "byte", (HTuple)imageWidth, (HTuple)imageHeight, 0, 0, -1, 0);
                //            Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                //            // Lock the bits of the bitmap.
                //            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                //            // Place the pointer to the buffer of the bitmap.
                //            converter.OutputPixelFormat = PixelType.RGB8packed;
                //            IntPtr ptrBmp = bmpData.Scan0;
                //            converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult); //Exception handling TODO
                //            bitmap.UnlockBits(bmpData);
                //            CogImg = new CogImage24PlanarColor(bitmap);
                //            bitmap.Dispose();
                //        }
                //        // 抛出图像处理事件
                //        /////////////////////////////////////////////////////////////删除halcon的
                //        long Buffers = camera.Parameters[PLCameraInstance.NumReadyBuffers].GetValue();
                //        ImageList.Enqueue(CogImg);
                //        if (eventProcessImage != null)
                //            eventProcessImage(CogImg, Buffers);
                //    }
                //    // pylon 自带窗体显示图像
                //    //ImageWindow.DisplayImage(numWindowIndex, grabResult);
                //}
                //else
                //{
                //    CogImage8Grey cogImage8Grey = new Cognex.VisionPro.CogImage8Grey();// (curBitmap.Width, curBitmap.Height);
                //    ImageList.Enqueue(CogImg);
                //    if (eventProcessImage != null)
                //        eventProcessImage(CogImg, 0);//运行一次空图片检测

                //    //改为写到日志文件
                //    //MessageBox.Show("Grab faild!\n" + grabResult.ErrorDescription, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //    //IVppOperateService _vppOperateService = ServiceLocator.Current.GetInstance<IVppOperateService>();
                //    //switch(strUserID)
                //    //{
                //    //    case "c1":

                //    //        Task.Factory.StartNew(() =>
                //    //        {
                //    //            _vppOperateService.logNet.WriteDebug($"牌号：{_vppOperateService.CurrentProgroName} {strUserID}号相机 第{ ++FailCount[0]}次取图失败");//
                //    //        });
                //    //        break;
                //    //    case "c2":
                //    //        Task.Factory.StartNew(() =>
                //    //        {
                //    //            _vppOperateService.logNet.WriteDebug($"牌号：{_vppOperateService.CurrentProgroName} {strUserID}号相机 第{ ++FailCount[1]}次取图失败");//
                //    //        });
                //    //        break;
                //    //    case "c3":
                //    //        Task.Factory.StartNew(() =>
                //    //        {
                //    //            _vppOperateService.logNet.WriteDebug($"牌号：{_vppOperateService.CurrentProgroName} {strUserID}号相机 第{ ++FailCount[2]}次取图失败");//
                //    //        });
                //    //        break;
                //    //    case "c4":
                //    //        Task.Factory.StartNew(() =>
                //    //        {
                //    //            _vppOperateService.logNet.WriteDebug($"牌号：{_vppOperateService.CurrentProgroName} {strUserID}号相机 第{ ++FailCount[3]}次取图失败");//
                //    //        });
                //    //        break;
                //    //    case "c5":
                //    //        Task.Factory.StartNew(() =>
                //    //        {
                //    //            _vppOperateService.logNet.WriteDebug($"牌号：{_vppOperateService.CurrentProgroName} {strUserID}号相机 第{ ++FailCount[4]}次取图失败");//
                //    //        });
                //    //        break;
                //    //    case "c6":
                //    //        Task.Factory.StartNew(() =>
                //    //        {
                //    //            _vppOperateService.logNet.WriteDebug($"牌号：{_vppOperateService.CurrentProgroName} {strUserID}号相机 第{ ++FailCount[5]}次取图失败");//
                //    //        });
                //    //        break;
                //    //    case "c7":
                //    //        Task.Factory.StartNew(() =>
                //    //        {
                //    //            _vppOperateService.logNet.WriteDebug($"牌号：{_vppOperateService.CurrentProgroName} {strUserID}号相机 第{ ++FailCount[6]}次取图失败");//
                //    //        });
                //    //        break;
                //    //    case "c8":
                //    //        Task.Factory.StartNew(() =>
                //    //        {
                //    //            _vppOperateService.logNet.WriteDebug($"牌号：{_vppOperateService.CurrentProgroName} {strUserID}号相机 第{ ++FailCount[7]}次取图失败");//
                //    //        });
                //    //        break;
                //    //    case "c9":
                //    //        Task.Factory.StartNew(() =>
                //    //        {
                //    //            _vppOperateService.logNet.WriteDebug($"牌号：{_vppOperateService.CurrentProgroName} {strUserID}号相机 第{ ++FailCount[9]}次取图失败");//
                //    //        });
                //    //        break;
                //    //}

                //}
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
            }

            //Marshal.FreeHGlobal(latestFrameAddress);
            Imagecount++;
            if (Imagecount > 8)
            {
                //在这里手动释放内存会导致释放内存和图像处理同时进行，导致检测时间不稳定变长。所以取消
                //GC.Collect();
                Imagecount = 0;
            }
        }
        /// <summary>
        /// Basler图像转Halcon图像
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public HObject GradImageHalcon(ImageGrabbedEventArgs e)
        {
            IGrabResult grabResult = e.GrabResult;//获取采集结果
            HObject hPylonImage = new HObject();
            try
            {
                //allocate the m_stream_size amount of bytes in non-managed environment  在非托管环境中分配m_stream_size字节量
                if (latestFrameAddress == IntPtr.Zero)
                {
                    latestFrameAddress = Marshal.AllocHGlobal((Int32)grabResult.PayloadSize);
                }
                if (grabResult.PixelTypeValue == PixelType.Mono8)
                {
                    converter.OutputPixelFormat = PixelType.Mono8;
                    converter.Convert(latestFrameAddress, grabResult.PayloadSize, grabResult);
                    //  转换为Halcon图像显示
                    ///////////////////////////////////////////////////////////删除halcon的
                    HOperatorSet.GenImage1(out  hPylonImage, "byte", (HTuple)grabResult.Width, (HTuple)grabResult.Height, latestFrameAddress);
                } else if (grabResult.PixelTypeValue == PixelType.BayerBG8 || grabResult.PixelTypeValue == PixelType.BayerGB8
                                   || grabResult.PixelTypeValue == PixelType.BayerRG8 || grabResult.PixelTypeValue == PixelType.BayerGR8)
                {
                    int imageWidth = grabResult.Width - 1;
                    int imageHeight = grabResult.Height - 1;
                    int payloadSize = imageWidth * imageHeight;
                    converter.OutputPixelFormat = PixelType.BGR8packed;     // 根据bayer格式不同切换以下代码
                    converter.Parameters[PLPixelDataConverter.InconvertibleEdgeHandling].SetValue("Clip");
                    converter.Convert(latestFrameAddress, 3 * payloadSize, grabResult);

                    /////////////////////////////////////////////////////////////删除halcon的
                   HOperatorSet.GenImageInterleaved(out  hPylonImage, latestFrameAddress, "bgr",
                          imageWidth, imageHeight, -1, "byte", imageWidth, imageHeight, 0, 0, -1, 0);

      
                }

            }
            catch (Exception)
            {


            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
            }
            return hPylonImage;

        }
        ///// <summary>
        ///// Basler图像转VisionPro图像CogImage8Grey
        ///// </summary>
        ///// <returns></returns>
        //public Cognex.VisionPro.CogImage8Grey GradImageCognex(ImageGrabbedEventArgs e)
        //{
        //    Cognex.VisionPro.CogImage8Grey cogImage8Grey;
        //    IGrabResult grabResult = e.GrabResult;//获取采集结果
        //    if (grabResult.GrabSucceeded)
        //    {
        //        grabTime = stopWatch.ElapsedMilliseconds;
        //        stopWatch.Restart();
        //        if (eventComputeGrabTime != null)
        //            eventComputeGrabTime(grabTime);

        //        Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format8bppIndexed);
        //        // Lock the bits of the bitmap.
        //        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
        //        // Place the pointer to the buffer of the bitmap.
        //        converter.OutputPixelFormat = PixelType.Mono8;
        //        IntPtr ptrBmp = bmpData.Scan0;
        //        converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult); //Exception handling TODO
        //        bitmap.UnlockBits(bmpData);
        //        //CogImg = new CogImage8Grey(bitmap);
        //        // 定义Buffer
        //        byte[] PixelDataBuffer = new byte[grabResult.Width * grabResult.Height];//curBitmap.Width * curBitmap.Height];
        //                                                                                // 拷贝Bitmap的像素数据的到Buffer
        //        System.Runtime.InteropServices.Marshal.Copy(ptrBmp, PixelDataBuffer, 0, PixelDataBuffer.Length);
        //        // 创建CogImage8Grey
        //         cogImage8Grey = new Cognex.VisionPro.CogImage8Grey(grabResult.Width, grabResult.Height);// (curBitmap.Width, curBitmap.Height);
        //                                                                                                                               // 获取CogImage8Grey的像素数据指针
        //        IntPtr Image8GreyIntPtr = cogImage8Grey.Get8GreyPixelMemory(Cognex.VisionPro.CogImageDataModeConstants.Read, 0, 0, grabResult.Width, grabResult.Height).Scan0;
        //        // 拷贝Buffer数据到CogImage8Grey的像素数据区
        //        System.Runtime.InteropServices.Marshal.Copy(PixelDataBuffer, 0, Image8GreyIntPtr, PixelDataBuffer.Length);
        //        CogImg = cogImage8Grey;
        //        bitmap.Dispose();
        //        return cogImage8Grey;
        //    }
        //    return null;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public ICogImage CompleteAcquireEx()
        //{
        //    ICogImage image = null;
        //    ImageList.TryDequeue(out image);
        //    return image;
        //}
        /// <summary>
        /// 清理图片
        /// </summary>
        public void CleanImage()
        {
            HObject image = null;
            //StopGrabbing();
            while (ImageList.TryDequeue(out image)) { }
            //StartGrabbing();

        }
        /****************************************************/

        /// <summary>
        /// 掉线重连回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectionLost(Object sender, EventArgs e)
        {
            try
            {
                camera.Close();

                for (int i = 0; i < 100; i++)
                {
                    try
                    {
                        camera.Open();
                        if (camera.IsOpen)
                        {
                            MessageBox.Show("已重新连接上UserID为“" + strUserID + "”的相机！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        Thread.Sleep(200);
                    }
                    catch
                    {
                        MessageBox.Show("请重新连接UserID为“" + strUserID + "”的相机！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (camera == null)
                {
                    MessageBox.Show("重连超时20s:未识别到UserID为“" + strUserID + "”的相机！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //HeartBeatTime=5000;
                SetGetCameraParameter();//初始化时的参数
                camera.StreamGrabber.Start();
                Thread.Sleep(10);
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="exception"></param>
        // Shows exceptions in a message box.
        private void ShowException(Exception exception)
        {
            MessageBox.Show("出现错误:\n" + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// 初始化参数
        /// </summary>
        private void SetGetCameraParameter()//初始化相机时的参数
        {
            //camera.Parameters[PLCamera.AcquisitionFrameRateEnable].SetValue(true);  // 限制相机帧率（开始后后可更改）
            //camera.Parameters[PLCamera.AcquisitionFrameRateAbs].SetValue(90);         //采集帧率数值设置（开始后后可更改）
            //camera.Parameters[PLCameraInstance.MaxNumBuffer].SetValue(10);          // 设置内存中接收图像缓冲区大小（★★开始后不可更改★★）
            imageWidth = camera.Parameters[PLCamera.Width].GetValue();               // 获取图像宽 
            imageHeight = camera.Parameters[PLCamera.Height].GetValue();              // 获取图像高
            long dsas = camera.Parameters[PLCamera.GevPersistentIPAddress].GetValue();
            camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;                      // 注册采集回调函数
            if (camera.GetSfncVersion() < Sfnc2_0_0)
            {
                HeartBeatTime = 1200;//timeout in milliseconds.
            }
            camera.ConnectionLost += OnConnectionLost;                                              //丢失相机事件,重新连接
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clearEvent"></param>
        //移除代理所有绑定
        private void clear_event(delegateProcessHImage clearEvent)
        {
            Delegate[] dels = eventProcessImage.GetInvocationList();
            foreach (Delegate d in dels)
            {
                //得到方法名
                object delObj = d.GetType().GetProperty("Method").GetValue(d, null);
                string funcName = (string)delObj.GetType().GetProperty("Name").GetValue(delObj, null);
                Debug.Print(funcName);
                eventProcessImage -= d as delegateProcessHImage;
            }
        }
        /// <summary>
        ///  图像处理委托事件
        /// </summary>
        public void ClearDelegate()
        {
            if (eventProcessImage != null)
                clear_event(eventProcessImage);
        }

    }
}
