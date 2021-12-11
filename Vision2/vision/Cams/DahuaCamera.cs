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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Vision2.vision.Cams
{
    public class DahuaCamera : Camera, ProjectNodet.IClickNodeProject
    {
        private IDevice dahuaCam;     /*  相机设备    */

   

        //private string CameraImageFormat = "BayerRG8";

        //相机连接丢失事件

        #region ********************相机连接关闭丢失事件********************

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dahuaCam_CameraClosed(object sender, EventArgs e)
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
        private void dahuaCam_ConnectionLost(object sender, EventArgs e)
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
        private void dahuaCam_CameraOpened(object sender, EventArgs e)
        {
            AlarmText.AddTextNewLine(this.name + "相机打开");
            OnLinkEnvet(true);
            OnCameraStatusChanged(this.ID + ":" + e.ToString());
        }

        #endregion ********************相机连接关闭丢失事件********************

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
                    if (balanceWhiteAuto == value)
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

        private string balanceWhiteAuto = "Off";

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
                if (expoursetime == value)
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
        {
            get => base.Gamma;
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


        //bool m_bShowLoop = true;            /* 线程控制变量 */
        Thread grabThread = null;           /* 采集线程  */
        bool m_bGrabLoop = false;           /* 标志位，标志是否循环连续采图*/

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
                    if (dahuaCam.DeviceInfo.ManufactureInfo== "Daheng Imaging")
                    {
                    }
                    else
                    {
                        //if (null == renderThread)
                        //{
                        //    renderThread = new Thread(new ThreadStart(ShowThread));
                        //    renderThread.Start();
                        //}
                     
                        if (dahuaCam.Open())
                        {
                            dahuaCam.TriggerSet.Open(this.TriggerSource.ToString());
                            dahuaCam.StreamArgEvent += DahuaCam_StreamArgEvent;
                            dahuaCam.MsgChannelArgEvent += DahuaCam_MsgChannelArgEvent;
                            /* 设置缓存个数为8（默认值为16） */
                            dahuaCam.StreamGrabber.SetBufferCount(4);
                            //using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.TriggerSource])
                            //{
                            //    p.SetValue(TriggerSource.ToString());
                            //}
                            using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.TriggerMode])
                            {
                                p.SetValue(TriggerMode.ToString());
                            }
                            using (IIntegraParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.ImageHeight])
                            {
                                Height = (int)p.GetValue();
                            }
                            using (IIntegraParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.ImageWidth])
                            {
                                this.Width = (int)p.GetValue();
                            }
                            using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.BalanceWhiteAuto])
                            {
                                p.SetValue(balanceWhiteAuto.ToString());
                            }
                            dahuaCam.ShutdownGrab();
                            if (OneCam)
                            {
                                using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.TriggerMode])
                                {
                                    if (false == p.SetValue("Off"))
                                    {
                                        MessageBox.Show(@"关闭触发失败");
                                    }
                                }
                                dahuaCam.StreamGrabber.ImageGrabbed += StreamGrabber_ImageGrabbed;  /* 注册码流回调事件 */
                                if (!dahuaCam.GrabUsingGrabLoopThread())
                                {
                                    MessageBox.Show(@"Start grabbing failed");
                                    return;
                                }
                            }
                            else
                            {
                                dahuaCam.StreamGrabber.Stop();
                                if (false == dahuaCam.StreamGrabber.Start(GrabStrategyEnum.grabStrartegySequential, GrabLoop.ProvidedByUser))
                                {
                                    AlarmText.AddTextNewLine("开启码流失败");
                                }
                            }
                            this.OnLinkEnvet(true);
                        }
                    }
                    BalanceWhiteAuto = balanceWhiteAuto;
                    Gain = gain;
                    ExposureTime = expoursetime;
             
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
        Mutex m_mutex = new Mutex();        /* 锁，保证多线程安全 */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
 
        private void DahuaCam_MsgChannelArgEvent(object sender, MsgChannelArgs e)
        {
           
        }
        /* 主动取图处理 */
        private void grabThreadProc()
        {
            while (m_bGrabLoop)
            {
            //    Watch.Restart();
                if (DateTime.Now.Second != st)
                {
                    st = DateTime.Now.Second;
                    dnumber = contd;
                    contd = 1;
                    //Vision.Disp_message(hWindow.HalconWindow, dnumber + "帧" + Watch.ElapsedMilliseconds);
                }
                else
                {
                    contd++;
                }
                OneResultOBj frame = new OneResultOBj();
                Thread.Sleep(10);
                //IFrameRawData
                IGrabbedRawData data; 
                if (null != dahuaCam && true == dahuaCam.WaitForFrameTriggerReady(out data, 1000))
                {
                    //m_mutex.WaitOne(1000);
                    Watch.Stop();
                    Task task =new Task(()=>{
                        try
                        {
                            frame = new OneResultOBj();
                            frame.Image  = this.IGrabbedRawDataTOImage(data.Clone());
                            HSystem.SetSystem("flush_graphic", "false");
                            HOperatorSet.ClearWindow(hWindow.HalconWindow);
                            HSystem.SetSystem("flush_graphic", "true");
                            HOperatorSet.DispObj(frame.Image, hWindow.HalconWindow);
                            if (Grabbing)
                            {
                                frame.Dispose();
                            }
                            else
                            {
                                halco.Image(frame.Image);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    });
                    task.Start();
                   Thread.Sleep(50);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        public override bool GetImage(out HObject image)
        {
            image = new HObject();
            image.GenEmptyObj();
            Watch.Restart();
            if (OneCam)
            {   /* 打开Software Trigger */
            
                imageTM = null;
                if (false == dahuaCam.TriggerSet.Open(TriggerSourceEnum.Software))
                {
                    MessageBox.Show(@"打开软触发失败");
                }
                dahuaCam.ExecuteSoftwareTrigger();
                while (imageTM == null)
                {
                    if (Watch.ElapsedMilliseconds > 1000)
                    {
                        return false;
                    }
                }
                image = imageTM;
                return true;
            }


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
                            image= this.IGrabbedRawDataTOImage(frame.GrabResult);
                            Watch.Stop();
                            RunTime = Watch.ElapsedMilliseconds;
                            if (!this.Grabbing)
                            {
                                Vision.TriggerSetup(this.FlashLampName, false.ToString());
                            }
                            frame.Dispose();
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

       // List<IGrabbedRawData> m_frameList = new List<IGrabbedRawData>();

        List<OneResultOBj> m_frameList = new List<OneResultOBj>();
        // 线程控制变量 | thread looping flag 
        HWindowControl hWindow;
        private Graphics _g = null;
        HObject imageTM;
        // 转码显示线程 
        // display thread routine 
        private void ShowThread(PictureBox pbImage)
        {
            while (Grabbing)
            {
                Thread.Sleep(1);
                if (m_frameList.Count == 0)
                {
                    Thread.Sleep(10);
                    continue;
                }
                try
                {
                    //Watch.Stop();
                    // 图像队列取最新帧 
                    // always get the latest frame in list 
                    m_mutex.WaitOne();
                    IGrabbedRawData frame = grabbedRawDatas.ElementAt(m_frameList.Count - 1);
                    grabbedRawDatas.Clear();
                    m_mutex.ReleaseMutex();

                    // 主动调用回收垃圾 
                    // call garbage collection 
                    GC.Collect();

                    // 控制显示最高帧率为25FPS 
                    // control frame display rate to be 25 FPS 
                    if (false == isTimeToDisplay())
                    {
                        continue;
                    }
                    if (DateTime.Now.Second != st)
                    {
                        st = DateTime.Now.Second;
                        dnumber = contd;
                        contd = 1;
                    }
                    else
                    {
                        contd++;
                    }
                    // 图像转码成bitmap图像 
                    // raw frame data converted to bitmap 
                    var bitmap = frame.ToBitmap(false);
               
                        /* 使用GDI绘图 */
                        if (_g == null)
                        {
                            _g = pbImage.CreateGraphics();
                        }
                        _g.DrawImage(bitmap, new Rectangle(0, 0, pbImage.Width, pbImage.Height),
                        new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                        bitmap.Dispose();
                }
                catch (Exception exception)
                {
                }
            }
        }
        Stopwatch m_stopWatch = new Stopwatch();
        // 判断是否应该做显示操作 
        // calculate interval to determine if it's show time now 
        private bool isTimeToDisplay()
        {
            m_stopWatch.Stop();
            long m_lDisplayInterval = m_stopWatch.ElapsedMilliseconds;
            if (m_lDisplayInterval <= 40)
            {
                m_stopWatch.Start();
                return false;
            }
            else
            {
          
                m_stopWatch.Reset();
                m_stopWatch.Start();
                return true;
            }
        }

        public override bool GetImage(out HalconRun.ImagesOneRun image)
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
                        IFrameRawData frame;
                        if (ID.Contains("SONY"))
                        {
                            dahuaCam.TriggerSet.ExecuteSoftwareTrigger();
                            dahuaCam.TriggerSet.Start();
                        }
                        if (dahuaCam.WaitForFrameTriggerReady(out frame, 1000))
                        {
                            image = new HalconRun.ImagesOneRun(frame.GrabResult.Clone());
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
        int st;
        int contd;
        List<IGrabbedRawData> grabbedRawDatas = new List<IGrabbedRawData>();
        private void StreamGrabber_ImageGrabbed(object sender, GrabbedEventArgs e)
        {
            try
            {
                Watch.Restart();
                m_mutex.WaitOne();
                grabbedRawDatas.Add(e.GrabResult.Clone());
                m_mutex.ReleaseMutex();
          
            }
            catch (Exception)
            {
            }
        }

        public override void Stop()
        {

            //dahuaCam.StreamGrabber.ImageGrabbed -= StreamGrabber_ImageGrabbed;         // 反注册回调 | unregister grab event callback 
            m_bGrabLoop = false;
            //dahuaCam.StreamGrabber.Stop();
            dahuaCam.ExecuteSoftwareTrigger();
            base.Stop();

        }

        HalconRun halco;
        public override void Straing(HalconRun halconRun)
        {
            if (Grabbing)
            {
                return;
            }
            hWindow = halconRun.GetHWindow().GetNmaeWindowControl();
            halco = halconRun;
            using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.TriggerMode])
            {
                if (false == p.SetValue("Off"))
                {
                    MessageBox.Show(@"关闭触发失败");
                }
            }
            if (OneCam)
            {
                if (m_bGrabLoop)
                {
                    m_bGrabLoop = false;
                    dahuaCam.ShutdownGrab();
                }
                if (!dahuaCam.IsGrabbing)
                {
                    dahuaCam.ShutdownGrab();
                    /* 注册码流回调事件，每收到一帧图像后自动进入注册的回调函数里取图，不需要另开线程*/
                    dahuaCam.StreamGrabber.ImageGrabbed += StreamGrabber_ImageGrabbed;
                    /* 开启码流 */
                    if (!dahuaCam.GrabUsingGrabLoopThread())
                    {
                        MessageBox.Show(@"开启码流失败");
                        dahuaCam.ShutdownGrab(); /*停止拉流*/
                        dahuaCam.Dispose(); /*释放*/
                        return;
                    }
                }
            }
            else
            {
                m_bGrabLoop = true;
                if (null == grabThread || !grabThread.IsAlive)
                {
                    grabThread = new Thread(new ThreadStart(grabThreadProc));
                    grabThread.Start();
                }
                if (!dahuaCam.IsGrabbing)
                {
                    /*主动取图方式开启码流，这里暂时按顺序取图
                    采图策略
                    GrabStrategyEnum.grabStrartegySequential : 按顺序取图
                    GrabStrategyEnum.grabStrartegyLatestImage : 取SDK图像缓存队列里最新一帧图片*/
                    if (!dahuaCam.StreamGrabber.Start(GrabStrategyEnum.grabStrartegySequential, GrabLoop.ProvidedByUser))
                    {
                        MessageBox.Show(@"开启码流失败");
                        return;
                    }
                }
            }
 

            base.Straing(halconRun);
            return;
            //dahuaCam.StreamGrabber.ImageGrabbed -= StreamGrabber_ImageGrabbed;         // 反注册回调 | unregister grab event callback 
            ////Thread.Sleep(10);
            //dahuaCam.ShutdownGrab();
            ////m_dev.ShutdownGrab();
            //using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.AcquisitionMode])
            //{
            //    p.SetValue("Continuous");
            //} // 开启码流 
            //  //using (IEnumParameter p = m_dev.ParameterCollection[ParametrizeNameSet.TriggerMode])
            //  //{
            //  //  p.SetValue("On");
            //  //} // 开启码流 
            //dahuaCam.StreamGrabber.ImageGrabbed += StreamGrabber_ImageGrabbed;
            //if (!dahuaCam.GrabUsingGrabLoopThread())
            //{
            //}
            //dahuaCam.ExecuteSoftwareTrigger();
            //dahuaCam.StreamGrabber.Start();
            //return;
            //Thread thread = new Thread(() =>
            //{
            //    while (Grabbing)
            //    {
            //        try
            //        {
            //            halconRun.HobjClear();
            //            if (this.GetImage(out HObject image))
            //            {
            //                //if (Defintion)
            //                //{
            //                //    Vision.Evaluate_definition(image);
            //                //}
            //                halconRun.Image(image);
            //                halconRun.AddMeassge("采图时间:" + RunTime);
            //                halconRun.ShowObj();
            //            }
            //            else
            //            {
            //                halconRun.Image().GenEmptyObj();
            //                halconRun.AddMeassge("采图失败:" + RunTime);
            //                halconRun.ShowObj();
            //            }
            //        }
            //        catch (Exception)
            //        {
            //        }
            //    }
            //});
            //thread.Priority = ThreadPriority.Highest;
            //thread.IsBackground = true;
            //thread.Start();
        }
        //HWindow hWindow;
        public override void Straing(HWindowControl hWind = null)
        {
      
            if (Grabbing)
            {
                return;
            }
            if (hWind == null)
            {
                hWindow = hWind;
            }
            Grabbing = true;
            hWindow = hWind;
            dahuaCam.StreamGrabber.ImageGrabbed -= StreamGrabber_ImageGrabbed;         // 反注册回调 | unregister grab event callback 
            //Thread.Sleep(10);
            dahuaCam.ShutdownGrab();
            //m_dev.ShutdownGrab();
            using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.AcquisitionMode])
            {
                p.SetValue("Continuous");
            } // 开启码流 
            using (IEnumParameter p = dahuaCam.ParameterCollection[ParametrizeNameSet.TriggerMode])
            {
                p.SetValue("Off");
            } // 开启码流 
            dahuaCam.StreamGrabber.ImageGrabbed += StreamGrabber_ImageGrabbed;
            if (!dahuaCam.GrabUsingGrabLoopThread())
            {
            }
            dahuaCam.ExecuteSoftwareTrigger();
            dahuaCam.StreamGrabber.Start();
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