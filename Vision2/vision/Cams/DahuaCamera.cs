using HalconDotNet;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ThridLibray;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.Cams
{

    public class DahuaCamera : Camera, ProjectNodet.IClickNodeProject
    {

        private IDevice dahuaCam;     /*  相机设备    */


        //private string CameraImageFormat = "BayerRG8";

        //相机连接丢失事件

        #region  ********************相机连接关闭丢失事件********************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dahuaCam_CameraClosed(object sender, EventArgs e)
        {
            AlarmText.AddTextNewLine(this.name + "相机关闭");
            OnLinkEnvet(false);

            OnCameraStatusChanged(this.ID + ":" + e.ToString());
        }
        /// <summary>
        /// 相机掉线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dahuaCam_ConnectionLost(object sender, EventArgs e)
        {
            ErosProjcetDLL.Project.AlarmText.AddTextNewLine(this.name + "相机掉线");

            OnLinkEnvet(false);
            this.OpenCam();
            OnCameraStatusChanged(this.ID + ":" + e.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dahuaCam_CameraOpened(object sender, EventArgs e)
        {
            AlarmText.AddTextNewLine(this.name + "相机打开");
            OnLinkEnvet(true);
            OnCameraStatusChanged(this.ID + ":" + e.ToString());
        }
        #endregion

        /// <summary>
        /// dahua相机构造函数
        /// </summary>
        /// <param name="name">相机key</param>
        public DahuaCamera()
        {
            cameratype = EnumCameraType.Dahua;
        }

        [Description("Off关闭,Once 一次,Continuous 自动"), Category("采图属性"), DisplayName("白平衡"),
        TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", false, "Off", "Once", "Continuous")]
        public string BalanceWhiteAuto
        {
            get
            {
                if (IsCamConnected)
                {
                    using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.BalanceRatioSelector])
                    {
                        p.SetValue("Blue");
                    }
                    using (IFloatParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.BalanceRatio])
                    {
                        Blue = p.GetValue();
                    }
                    using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.BalanceRatioSelector])
                    {
                        p.SetValue("Red");
                    }
                    using (IFloatParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.BalanceRatio])
                    {
                        Red = p.GetValue();
                    }
                    using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.BalanceRatioSelector])
                    {
                        p.SetValue("Green");
                    }
                    using (IFloatParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.BalanceRatio])
                    {
                        Green = p.GetValue();
                    }
                    using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.BalanceWhiteAuto])
                    {
                        balanceWhiteAuto = p.GetValue().ToString();
                    }
                }
                return balanceWhiteAuto;
            }
            set
            {
                try
                {
                    if (balanceWhiteAuto==value)
                    {
                        return;
                    }
                    balanceWhiteAuto = value;
                    if (IsCamConnected)
                    {
                        using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.BalanceWhiteAuto])
                        {
                            p.SetValue(value);
                        }
                    }
                }
                catch (Exception)
                {


                }
            }
        }
        string balanceWhiteAuto = "Off";
        [Description("彩色通道"), Category("采图属性"), DisplayName("红色")]
        public double Red { get; set; }
        [Description("彩色通道"), Category("采图属性"), DisplayName("蓝色")]
        public double Blue { get; set; }
        [Description("彩色通道"), Category("采图属性"), DisplayName("绿色")]
        public double Green { get; set; }
        public override string IP
        {
            get;
            set;
        }
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
                if ( expoursetime== value)
                {
                    return;
                }
                if (IsCamConnected)
                {
                    using (IFloatParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.ExposureTime])
                    {
                        p.SetValue(value);
                    }
                }
                expoursetime = value;
            }
        }


        public override double Gamma 
        { get => base.Gamma;
            set
            {
                if (gamma == value)
                {
                    return;
                }
                if (IsCamConnected)
                {
                    using (IFloatParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.Gamma])
                    {
                        p.SetValue(value);
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
                if (IsCamConnected)
                {
                    using (IFloatParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.GainRaw])
                    {
                        p.SetValue(value);
                    }
                }
                gain = value;
            }
        }
        /// <summary>
        /// 设置相机连接状态
        /// </summary>
        public override bool IsCamConnected
        {
            get
            {
                if (dahuaCam != null)
                {
                    return dahuaCam.IsOpen;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 连接相机
        /// </summary>
        public override void OpenCam()
        {
            if (IsCamConnected)
            {
                return;
            }
            try
            {
                Enumerator.EnumerateDevices();
                dahuaCam = Enumerator.GetDeviceByKey(ID);
                if (dahuaCam != null)
                {
                    Enumerator.GigeCameraNetInfo(this.Index, out string MaxAdd, out string IP, out string subnetMaxk, out string defaultg);
                    this.IP = IP;
                    IDeviceInfo device = Enumerator.getDeviceInfoByKey(this.ID);
                    IGigeInterfaceInfo device2 = Enumerator.GigeInterfaceInfo(this.Index);
                    this.IntIP = device2.IPAddress;
                    dahuaCam.CameraOpened += dahuaCam_CameraOpened;
                    dahuaCam.ConnectionLost += dahuaCam_ConnectionLost;
                    dahuaCam.CameraClosed += dahuaCam_CameraClosed;

                    if (dahuaCam.Open())
                    {
                        dahuaCam.TriggerSet.Close();
                        using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.ImagePixelFormat])
                        {
                            //根据实际使用的相机，设置实际需要的图像格式
                            p.SetValue(CameraImageFormat);
                        }
                        dahuaCam.StreamGrabber.SetBufferCount(2);
                        if (false == dahuaCam.StreamGrabber.Start(GrabStrategyEnum.grabStrartegyLatestImage, GrabLoop.ProvidedByUser))
                        {
                            AlarmText.AddTextNewLine("开启码流失败");
                            throw new Exception("开启码流失败！");
                        }
                        dahuaCam.StreamArgEvent += DahuaCam_StreamArgEvent;
                        this.OnLinkEnvet(true);
                    }
                    BalanceWhiteAuto = balanceWhiteAuto;
                    Gain = gain;
                    ExposureTime = expoursetime;
                    using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.TriggerSource])
                    {
                        p.SetValue(TriggerSourceEnum.Software);
                    }
                    using (ICommandParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.TriggerSoftware])
                    {
                        //p.IsExecuteing
                    }
                    using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.TriggerMode])
                    {
                        p.SetValue(TriggerModeEnum.Off);
                    }
                    using (IIntegraParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.ImageHeight])
                    {
                        Height = (int)p.GetValue();
                    }
                    using (IIntegraParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.ImageWidth])
                    {
                        this.Width = (int)p.GetValue();
                    }
                }
                else
                {
                    this.OnLinkEnvet(false);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void DahuaCam_StreamArgEvent(object sender, StreamEventArgs e)
        {

            ulong uTime = e.Timestamp;
            ulong nId = e.BlockID;
            uint uChannel = e.Channel;
            StreamEventStatus status = e.StreamEventStatus;
            ulong ulTime = e.Timestamp;
            string das = "";
            switch (e.StreamEventStatus)
            {
                case StreamEventStatus.EN_EVENT_STATUS_NORMAL:
                    break;
                case StreamEventStatus.EN_EVENT_STATUS_LOST_FRAME:
                    das = "缓存被覆盖导致的丢帧";
                    break;
                case StreamEventStatus.EN_EVENT_STATUS_LOST_PACKET:
                    das = "丢包导致的丢帧";
                    break;
                case StreamEventStatus.EN_EVENT_STATUS_IMAGE_ERROR:
                    das = "图像错误";
                    break;
                case StreamEventStatus.EN_EVENT_STATUS_CHANNEL_ERROR:
                    das = "通道错误";
                    break;
                default:
                    break;
            }
            if (das != "")
            {
                AlarmText.AddTextNewLine("流事件:" + das);
            }
        }

        public System.Diagnostics.Stopwatch Watch = new System.Diagnostics.Stopwatch();
        /// <summary>
        /// 关闭相机
        /// </summary>
        public override void CloseCam()
        {
            if (dahuaCam != null && dahuaCam.IsOpen)
            {
                try
                {
                    dahuaCam.ShutdownGrab();
                    dahuaCam.Close();
                }
                catch (Exception)
                {
                }
            }
        }
        /// <summary>
        /// 采集图片
        /// </summary>
        /// <returns></returns>
        public override HObject GetImage()
        {
            Watch.Restart();
            if (!this.Grabbing)
            {

                Vision.TriggerSetup(this.FlashLampName, true.ToString());
                if (FlashLampName!=null&&  FlashLampName != "")
                {
                    Thread.Sleep(FlashLampTime);
                }
            }
            lock (this)
            {
                int err = 0;
                HObject ho_image2 = new HObject();
                ho_image2.GenEmptyObj();
                ST:
                if (dahuaCam != null)
                {
                    if (!dahuaCam.IsOpen)
                    {
                        AlarmText.AddTextNewLine(this.name + "短线重连");
                        OpenCam();
                    }
                    try
                    {
                        IFrameRawData frame;//IGrabbedRawData frame;
                        if (ID.Contains("SONY"))
                        {
                            dahuaCam.TriggerSet.ExecuteSoftwareTrigger();
                            dahuaCam.TriggerSet.Start();
                        }
                        if (dahuaCam.WaitForFrameTriggerReady(out frame, 1000))
                        {
                            //黑白图像
                            if (CameraImageFormat == "Mono8")
                            {
                                int nRGB = RGBFactory.EncodeLen(frame.Width, frame.Height, false);
                                IntPtr pData = Marshal.AllocHGlobal(nRGB);
                                Marshal.Copy(frame.GrabResult.Image, 0, pData, frame.GrabResult.ImageSize);
                                HOperatorSet.GenImage1Extern(out ho_image2, "byte", frame.Width, frame.Height, (HTuple)pData, 0);
                                Marshal.FreeHGlobal(pData);
                            }
                            //彩色图像
                            else
                            {
                                int nRGB = RGBFactory.EncodeLen(frame.Width, frame.Height, true);
                                IntPtr pData = Marshal.AllocHGlobal(nRGB);
                                RGBFactory.ToRGB(frame.GrabResult.Image, frame.Width, frame.Height, true, frame.GrabResult.PixelFmt, pData, nRGB);
                                HOperatorSet.GenImageInterleaved(out ho_image2, (HTuple)pData, "bgr", frame.Width, frame.Height, 0, "byte", frame.Width, frame.Height, 0, 0, 8, 0);
                                Marshal.FreeHGlobal(pData);
                            }
                            if (RotateTypeStr != "None")
                            {
                                HOperatorSet.MirrorImage(ho_image2, out ho_image2, RotateTypeStr);
                            }
                            Watch.Stop();
                            RunTime = Watch.ElapsedMilliseconds;
                            if (!this.Grabbing)
                            {
                                Vision.TriggerSetup(this.FlashLampName, false.ToString());
                            }
                            frame.Dispose();
                            return ho_image2;
                        }
                        else
                        {
                            CloseCam();
                            OpenCam();
                            err++;
                             AlarmText.AddTextNewLine("采图失败");
                            if (err<3)
                            {
                                goto ST;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AlarmText.AddTextNewLine("采图错误" + ex.Message);
                        throw ex;
                    }
                }
                else
                {
                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine("程序未关联相机"+this.name);
                }
                return ho_image2;
            }
        }

        public override bool GetImage(out IGrabbedRawData image)
        {
            image = null;
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
                int err = 0;
                HObject ho_image2 = new HObject();
                ho_image2.GenEmptyObj();
            ST:
                if (dahuaCam != null)
                {
                    if (!dahuaCam.IsOpen)
                    {
                        AlarmText.AddTextNewLine(this.name + "短线重连");
                        OpenCam();
                    }
                    try
                    {
                        IFrameRawData frame;//IGrabbedRawData frame;
                        if (ID.Contains("SONY"))
                        {
                            dahuaCam.TriggerSet.ExecuteSoftwareTrigger();
                            dahuaCam.TriggerSet.Start();
                        }
                        if (dahuaCam.WaitForFrameTriggerReady(out frame, 1000))
                        {
                            image = frame.GrabResult.Clone();
                            frame.Dispose();
                            Watch.Stop();
                            RunTime = Watch.ElapsedMilliseconds;
                            return true;
                        }
                        else
                        {
                            CloseCam();
                            OpenCam();
                            err++;
                            AlarmText.AddTextNewLine("采图失败");
                            if (err < 3)
                            {
                                goto ST;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AlarmText.AddTextNewLine("采图错误" + ex.Message);
                        throw ex;
                    }
                }
                else
                {
                    AlarmText.AddTextNewLine("程序未关联相机" + this.name);
                }
                return false;
            }
        }
        public override void Straing(HalconRun halconRun)
        {

            if (Grabbing)
            {
                return;
            }

            base.Straing(halconRun);
            Thread thread = new Thread(() =>
            {

                while (Grabbing)
                {
                    try
                    {
                        halconRun.HobjClear();
                        halconRun.Image(this.GetImage());
                        halconRun.AddMeassge("采图时间:" + RunTime);
                        halconRun.ShowObj();
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
            return new CamProUI(this);
        }

        public override string GetFramegrabberParam(string pName)
        {
            try
            {
                if (IsCamConnected)
                {
                    using (IStringParameter p = dahuaCam.ParameterCollection[new StringName(pName)])
                    {
                        //Trace.WriteLine(string.Format("DeviceModelName value: {0}", p.GetValue()));
                        //textBox_Model.Text = p.GetValue();
                        return p.GetValue();
                    }
                }
            }
            catch (Exception)
            {

            }


            return "";
        }
        public override bool SetProgramValue(string pName, string value)
        {
            return false;
        }
        public override bool SetProgramValue(string pName, double value)
        {
            return false;
        }

        public override void SetExposureTime(double VALUE)
        {
            if (IsCamConnected)
            {
                using (IFloatParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.ExposureTime])
                {
                    p.SetValue(VALUE);
                }
            }
            base.SetExposureTime(VALUE);
        }

        public override object GetIDevice()
        {
            return dahuaCam;
        }
    }

}
