using ErosSocket.ErosConLink;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.Cams
{
    [Serializable]
    public class CamParam : ProjectC, ProjectNodet.IClickNodeProject
    {
        public CamParam()
        {
            TriggerMode = "Off";
            m_AcqHandle = -1;
            m_IDStr = "default";
            ExposureTimeAbs = 100;
            m_CamInformation = "";
            M_GainInt = 1;
            Name = "Cam";
        }

        ~CamParam()
        {
        }

        public int Height { get; set; }
        public int Width { get; set; }

        public Control GetThisControl()
        {
            return null;
        }

        #region 属性

        /// <summary>
        /// 相机句柄
        /// </summary>
        private HTuple m_AcqHandle;

        [Description("曝光"), Category("拍照属性"), DefaultValue(""), DisplayName("曝光")]
        /// <summary>
        /// 相机快门
        /// </summary>
        public Int32 ExposureTimeAbs { get; set; }

        [Description("增益参数"), Category("拍照属性"), DefaultValue(""), DisplayName("增益")]
        /// <summary>
        /// 相机增益
        /// </summary>
        public uint M_GainInt { get; set; }

        [ReadOnly(true)]
        [DisplayName("链接状态"), Category("拍照属性"), DefaultValue("")]
        /// <summary>
        /// 相机是否链接
        /// </summary>
        public bool m_bCamIsCon { get; set; } = false;

        [Description("链接硬件标识"), Category("硬件属性"), DefaultValue("default"), DisplayName("链接硬件标识")]
        public string m_IDStr { get; set; }

        [Description("采图尝试链接次数"), Category("拍照属性"), DefaultValue(1), DisplayName("失败链接")]
        public int LinkNumber { get; set; } = 1;

        [Description("相机的基本信息"), Category("硬件属性"), DisplayName("相机的基本信息")]
        /// <summary>
        /// 相机详细信息
        /// </summary>
        public string m_CamInformation { get; set; }

        [Description("软硬触发"), Category("拍照属性"), DisplayName("触发模式"),
            TypeConverter(typeof(Vision2.ErosProjcetDLL.UI.PropertyGrid.ErosConverter)),
            Vision2.ErosProjcetDLL.UI.PropertyGrid.ErosConverter.ThisDropDown("", true, "Off", "On")]
        /// <summary>
        /// 使能硬触发 On为外触发，Off为软触发
        /// </summary>
        public string TriggerMode { get; set; } = "Off";

        /// <summary>
        /// 实时采集模式
        /// </summary>
        [Description("实时模式状态"), Category("拍照属性"), DisplayName("实时状态")]
        public bool RealTimeMode { get; protected set; }

        [Description("触发源,FixedRate:固定频率，Software：软触发，Freerun：自由转换，Line1输入1"), Category("拍照属性"), DisplayName("触发源"),
            TypeConverter(typeof(Vision2.ErosProjcetDLL.UI.PropertyGrid.ErosConverter)),
            Vision2.ErosProjcetDLL.UI.PropertyGrid.ErosConverter.ThisDropDown("", true, "Freerun", "Line1", "FixedRate", "Software")]
        public string TriggerSource { get; set; } = "Software";

        [Description("镜像角度,row上翻转，diagonal对角斜线翻转，column左右翻转，None无翻转"), Category("拍照属性"),
            DisplayName("镜像角度"), TypeConverter(typeof(Vision2.ErosProjcetDLL.UI.PropertyGrid.ErosConverter)),
            Vision2.ErosProjcetDLL.UI.PropertyGrid.ErosConverter.ThisDropDown("", true, "row", "diagonal", "column", "None")]
        public string RotateTypeStr { get; set; } = "None";

        [Description("转换比值像素/MM"), Category("图像属性"), DisplayName("转换比值")]
        public double CaliConst { get; set; } = 1;

        /// <summary>
        /// 相机信息
        /// </summary>
        [Browsable(true)]
        [Description("相机的信息集合"), Category("硬件属性"), DisplayName("硬件信息"), DefaultValue("")]
        public Cam_information camS_Information { get; set; } = new Cam_information();

        [Description("异步采图有效时间"), DisplayName("有效时间"), Category("异步采集"), DefaultValue(-1)]
        public double Max { get; set; } = -1;

        [ReadOnly(true)]
        [Description("异步采集帧数量"), Category("异步采集"), DisplayName("帧数")]
        public int Frame { get; set; } = 0;

        [ReadOnly(true)]
        [Description("异步采集帧率，秒/帧"), Category("异步采集"), DisplayName("FPS")]
        public double Fps
        {
            get { return fps; }
        }

        protected double fps;

        [Description("true程序使用异步采集,Fales同步模式"), Category("异步采集"), DisplayName("使用异步采集")]
        public bool Modet { get; set; }

        [Description("采集超时设置"), Category("采集"), DisplayName("采集超时")]
        public int Grab_OutTime
        {
            get { return grad_OutTime; }
            set
            {
                grad_OutTime = value;
                if (m_bCamIsCon)
                {
                    try
                    {
                        HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "grab_timeout", value);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private int grad_OutTime = 5000;

        [DescriptionAttribute("灯光输出。"), Category("触发器"), DisplayName("灯光输出名称")]
        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string FlashLampName { get; set; } = string.Empty;

        public bool Grabbing { get; set; }
        public int MaxNumbe { get; set; }
        public int RunID { get; set; }

        [DescriptionAttribute("灯光打开后延时触发拍照。"), Category("触发器"), DisplayName("拍照触发延时")]
        public int CamTime
        {
            get { return camTime; }
            set
            {
                if (value > 10000)
                {
                    return;
                }
                camTime = value;
            }
        }

        private int camTime = 0;

        [DescriptionAttribute("NG结果输出的变量名称。"), Category("触发器"), DisplayName("结果NG名称")]
        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string NGName { get; set; } = string.Empty;

        [DescriptionAttribute("NG结果输出的变量名称。"), Category("触发器"), DisplayName("结果NG名称")]
        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string OKName { get; set; } = string.Empty;

        [DescriptionAttribute("运行时间MS。"), Category("结果显示"), DisplayName("运行时间MS")]
        public long RunTime { get { return Watch.ElapsedMilliseconds; } }

        #endregion 属性

        /// <summary>
        /// 运行计时
        /// </summary>
        public System.Diagnostics.Stopwatch Watch = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// 链接类委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public delegate void LinkStart<T>(T key);

        public delegate void Sw(string key, HObject image, int runID, bool isSave = true);

        /// <summary>
        /// 执行链接事件
        /// </summary>
        public event LinkStart<bool> LinkSt;

        /// <summary>
        /// 执行链接事件
        /// </summary>
        public event Sw Swtr;

        public string Key { get; set; } = "";

        /// <summary>
        /// 调用图像事件
        /// </summary>
        /// <param name="key">程序ID</param>
        /// <param name="iamge">采集的图像</param>
        /// <param name="runID">运行ID</param>
        public void OnSwtr(string key, HObject iamge, int runID)
        {
            this.Swtr?.Invoke(key, iamge, runID);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="isThread"></param>
        public void OnLinkSt(bool isThread)
        {
            this.m_bCamIsCon = false;
            if (isThread)
            {
                Thread thread = new Thread(() =>
                {
                    this.m_bCamIsCon = LiakCam();
                    LinkSt?.Invoke(this.m_bCamIsCon);
                    if (!this.m_bCamIsCon)
                    {
                        this.m_bCamIsCon = LiakCam();
                        LinkSt?.Invoke(this.m_bCamIsCon);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                this.m_bCamIsCon = LiakCam();
                LinkSt?.Invoke(this.m_bCamIsCon);
            }
        }

        /// <summary>
        /// 链接相机
        /// </summary>
        protected virtual bool LiakCam()
        {
            try
            {
                this.m_bCamIsCon = false;
                try
                {
                    HOperatorSet.CloseFramegrabber(this.m_AcqHandle);
                }
                catch (Exception)
                {
                }
                string Camid = this.m_IDStr.ToString().Trim(' ', '"');
                HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "progressive", -1, "default", -1, "false", "default",
                   Camid, 0, -1, out HTuple AcqHandle);
                this.m_AcqHandle = AcqHandle;
                m_bCamIsCon = true;
                HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "grab_timeout", 2000);
                //HOperatorSet.GrabImageStart(AcqHandle, Max);
                //HOperatorSet.GetFramegrabberParam(this.m_AcqHandle, "AcquisitionMode", out HTuple sdv);
                SetFramegrabberParam();
            }
            catch (Exception ex)
            {
                this.LogErr(ex);
                //OnLinkSt(false);
            }

            return this.m_bCamIsCon;
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        public virtual void OffCam()
        {
            try
            {
                HOperatorSet.CloseFramegrabber(this.m_AcqHandle);
            }
            catch (Exception)
            {
            }

            LinkSt?.Invoke(false);
            Thread.Sleep(500);
            this.m_bCamIsCon = false;
        }

        /// <summary>
        /// 设置曝光增益，
        /// </summary>
        public virtual void SetFramegrabberParam(string name = null, HTuple value = null)
        {
            if (this.m_bCamIsCon)
            {
                try
                {
                    if (name == null)
                    {
                        try
                        {
                            HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "ExposureTimeAbs", this.ExposureTimeAbs);
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            try
                            {
                                HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "GainRaw", this.M_GainInt);
                            }
                            catch (Exception)
                            {
                                HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "Gain", this.M_GainInt);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "TriggerSource", this.TriggerSource);
                        //HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "TriggerMode", this.TriggerMode);
                    }
                    else
                    {
                        if (value != null)
                        {
                            try
                            {
                                HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, name, value);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("相机:" + this.Name + "写参数错误：" + ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        public virtual void SetProgramValue(string pragrmName, HTuple value)
        {
        }

        //public virtual void ge(string pragrmName, uint value)
        //{
        //}

        public virtual void SetProgramValue(string pragrmName, string value)
        {
        }

        public virtual void SetFrameGradderIP(string ip)
        {
            try
            {
                string forceIP = this.camS_Information.information.ToString();
                string[] dats = forceIP.Split('|');
                for (int i = 0; i < dats.Length; i++)
                {
                    if (dats[i].StartsWith(" suggestion:force_ip="))
                    {
                        ErosSocket.DebugPLC.Pop_UpWindow pop_Up = new ErosSocket.DebugPLC.Pop_UpWindow("建议参数", dats[i].TrimEnd('"'));
                        DialogResult dialogResult = pop_Up.ShowDialog();
                        if (dialogResult == DialogResult.Yes)
                        {
                            forceIP = pop_Up.Tag.ToString();
                            HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "progressive", -1, "default", forceIP, "false", "default", this.m_IDStr, 0, -1, out HTuple hTuple);
                            this.m_AcqHandle = hTuple;
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public virtual void SetGain()
        {
            try
            {
                HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "Gain", this.M_GainInt);
            }
            catch (Exception)
            {
            }
        }

        public virtual void SetTriggerMode()
        {
            try
            {
                HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "TriggerMode", this.TriggerMode);
            }
            catch (Exception)
            {
            }
        }

        public virtual void SetTriggerSource()
        {
            try
            {
                HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "TriggerSource", this.TriggerSource);
            }
            catch (Exception)
            {
            }
        }

        public virtual void SetExposureTime(int exp = 0)
        {
            try
            {
                if (exp != 0)
                {
                    HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "ExposureTime", exp);
                }
                else
                {
                    HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "ExposureTime", this.ExposureTimeAbs);
                }
            }
            catch (Exception)
            {
            }
            try
            {
                HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "ExposureTimeAbs", this.ExposureTimeAbs);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 读取基础数据
        /// </summary>
        public virtual void GetFramegradderParam()
        {
            try
            {
                if (this.m_bCamIsCon)
                {
                    this.M_GainInt = (uint)GetFramegrabberParam("GainRaw").I;
                    this.ExposureTimeAbs = GetFramegrabberParam("ExposureTimeAbs").TupleInt();
                    this.TriggerMode = GetFramegrabberParam("TriggerMode");
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 读取增益
        /// </summary>
        /// <returns></returns>
        public virtual double GetGain()
        {
            try
            {
                if (this.m_bCamIsCon)
                {
                    this.M_GainInt = (uint)GetFramegrabberParam("GainRaw");
                }
            }
            catch (Exception)
            {
            }
            return this.M_GainInt;
        }

        /// <summary>
        /// 读取曝光
        /// </summary>
        /// <returns></returns>
        public virtual double GetExposureTimeAbs()
        {
            try
            {
                if (this.m_bCamIsCon)
                {
                    this.ExposureTimeAbs = (int)GetFramegrabberParam("ExposureTimeAbs").D;
                }
            }
            catch (Exception)
            {
            }
            return this.ExposureTimeAbs;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual HTuple GetFramegrabberParam(string parmName)
        {
            HTuple hTuple = new HTuple();

            try
            {
                if (this.m_bCamIsCon)
                {
                    HOperatorSet.GetFramegrabberParam(this.m_AcqHandle, parmName, out hTuple);
                }
            }
            catch (Exception ex)
            {
                //this.LogErr(ex);
            }
            return hTuple;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual string GetIP()
        {
            HTuple hTuple = new HTuple();
            try
            {
                if (this.m_bCamIsCon)
                {
                    hTuple = GetFramegrabberParam("GevCurrentIPAddress");
                    if (hTuple.TupleType() != 15)
                    {
                        this.camS_Information.IP = IntToIp(hTuple);
                    }
                }
            }
            catch (Exception ex)
            {
                //this.LogErr(ex);
            }
            return this.camS_Information.IP;
        }

        /// <summary>
        /// int转换IP
        /// </summary>
        /// <param name="ipInt"></param>
        /// <returns></returns>
        public static string IntToIp(long ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((ipInt >> 24) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append(ipInt & 0xFF);
            return sb.ToString();
        }

        /// <summary>
        /// iP转换int
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long IpToInt(string ip)
        {
            try
            {
                if (ip == "")
                {
                    return 0;
                }
                char[] separator = new char[] { '.' };
                string[] items = ip.Split(separator);
                return long.Parse(items[0]) << 24
                        | long.Parse(items[1]) << 16
                        | long.Parse(items[2]) << 8
                        | long.Parse(items[3]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }

        /// <summary>
        /// 读取触发模式
        /// </summary>
        /// <returns></returns>
        public virtual bool GetTriggerMode()
        {
            try
            {
                if (this.m_bCamIsCon)
                {
                    HOperatorSet.GetFramegrabberParam(this.m_AcqHandle, "TriggerMode", out HTuple hTuple);
                    this.TriggerMode = hTuple;
                }
            }
            catch (Exception ex)
            {
                this.LogErr(ex);
            }
            return Convert.ToBoolean(this.TriggerMode);
        }

        /// <summary>
        /// 相机采图成功返回true
        /// </summary>
        /// <param name="Image">返回图片</param>
        /// <returns>成功返回true</returns>
        public virtual void GaedImageAsync(out HObject Image)
        {
            try
            {
                HOperatorSet.GrabImageAsync(out Image, this.m_AcqHandle, Max);
            }
            catch (Exception EX)
            {
                Image = new HObject();
                Image.GenEmptyObj();

                this.LogErr(EX);
                return;
            }
        }

        /// <summary>
        /// 拍照接口
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool Run(HalconRun halcon)
        {
            Watch.Restart();

            Thread.Sleep(camTime);
            bool iscong = GaedImage(out HObject hObject);
            if (hObject.IsInitialized())
            {
                halcon.Image(hObject);
            }
            Vision.TriggerSetup(this.FlashLampName, false.ToString());

            //halcon.Image(hObject);
            ImageDefinition(halcon);
            if (iscong)
            {
                Vision.TriggerSetup(this.OKName, true.ToString());
            }
            else
            {
                if (this.CoordinateMeassage != Coordinate.Coordinate_Type.Hide)
                {
                    halcon.GetOneImageR().AddMeassge(this.Name + ":执行失败");
                }
                Vision.TriggerSetup(this.NGName, true.ToString());
            }
            Watch.Stop();
            return iscong;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool GaedImage(out HObject image)
        {
            image = new HObject();
            image.GenEmptyObj();
            bool isdwr = false;
        stren:
            try
            {
                Vision.TriggerSetup(this.FlashLampName, true.ToString());
                //HOperatorSet.GetFramegrabberParam()
                //HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "TriggerMode",   "Off");
                //SetTriggerMode();
                //SetTriggerSource();
                //HOperatorSet.GetFramegrabberParam(this.m_AcqHandle, "TriggerMode", out HTuple hTuple);
                if (Modet)
                {
                    GaedImageAsync(out image);
                }
                else
                {
                    //GaedImageAsync(out image);
                    HOperatorSet.GrabImage(out image, this.m_AcqHandle);
                    HOperatorSet.GetImageSize(image, out HTuple hTuple, out HTuple hTuple2);
                    Width = hTuple;
                    Height = hTuple2;
                }
                Vision.TriggerSetup(this.FlashLampName, false.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Vision.TriggerSetup(this.FlashLampName, false.ToString());
                if (!isdwr)
                {
                    image.GenEmptyObj();
                    for (int i = 0; i < LinkNumber; i++)
                    {
                        this.OnLinkSt(false);

                        if (this.m_bCamIsCon)
                        {
                            isdwr = true;
                            goto stren;
                        }
                    }
                }
                this.LogErr("釆图失败,连接不成功");
                return false;
            }
        }

        public Coordinate.Coordinate_Type CoordinateMeassage { get; set; } = new Coordinate.Coordinate_Type();

        private HTuple MaxTuple = new HTuple();

        /// <summary>
        /// 图像清晰度评估
        /// </summary>
        /// <param name="halcon"></param>
        public void ImageDefinition(HalconRun halcon)
        {
            HTuple hTuple;

            if (this.CoordinateMeassage == Coordinate.Coordinate_Type.XYU2D)
            {
                int widgth = 1000;
                int Rab = widgth / 2;
                HTuple Tool;
                HTuple Lift;
                HTuple Heit;
                HTuple loot;
                hTuple = Vision.Evaluate_definition(halcon.Image());

                HOperatorSet.GenRectangle1(out HObject ToPhObject, 1, halcon.Width / 2 - Rab, widgth, halcon.Width / 2 + Rab);
                HOperatorSet.GenRectangle1(out HObject lefthObject, halcon.Height / 2 - Rab, 1, halcon.Height / 2 + Rab, widgth);
                HOperatorSet.GenRectangle1(out HObject BottomhObject, halcon.Height - widgth, halcon.Width / 2 - Rab, halcon.Height - 1, halcon.Width / 2 + Rab);
                HOperatorSet.GenRectangle1(out HObject RigthObject, halcon.Height / 2 - Rab, halcon.Width - widgth, halcon.Height / 2 + Rab, halcon.Width - 1);
                HOperatorSet.ReduceDomain(halcon.Image(), ToPhObject, out HObject ImageToPhObject);
                HOperatorSet.ReduceDomain(halcon.Image(), lefthObject, out HObject ImagelefthObject);
                HOperatorSet.ReduceDomain(halcon.Image(), BottomhObject, out HObject ImageBottomhObject);
                HOperatorSet.ReduceDomain(halcon.Image(), RigthObject, out HObject ImageRigthObject);
                Tool = Vision.Evaluate_definition(ImageToPhObject);
                Lift = Vision.Evaluate_definition(ImagelefthObject);
                Heit = Vision.Evaluate_definition(ImageBottomhObject);
                loot = Vision.Evaluate_definition(ImageRigthObject);
                halcon.AddShowObj(BottomhObject);
                halcon.AddShowObj(lefthObject);
                halcon.AddShowObj(ToPhObject);
                halcon.AddShowObj(RigthObject);

                if (MaxTuple.Length == 0)
                {
                    MaxTuple = hTuple;
                }
                else
                {
                    if (hTuple.TupleGreaterEqualElem(MaxTuple))
                    {
                        MaxTuple = hTuple;
                    }
                }

                halcon.GetOneImageR().AddMeassge("整体清晰度:" + hTuple.ToString());
                halcon.GetOneImageR().AddMeassge("最大清晰度:" + MaxTuple.ToString());
                halcon.ShowMessage("上" + Tool.ToString(), Rab, halcon.Width / 2);
                halcon.ShowMessage("下" + loot.ToString(), halcon.Height - Rab, halcon.Width / 2);
                halcon.ShowMessage("左" + Lift.ToString(), halcon.Height / 2, Rab);
                halcon.ShowMessage("右" + Heit.ToString(), halcon.Height / 2, halcon.Width - Rab);
            }
            if (this.CoordinateMeassage == Coordinate.Coordinate_Type.PixelRC)
            {
                hTuple = Vision.Evaluate_definition(halcon.Image());
                halcon.GetOneImageR().AddMeassge("整体清晰度:" + hTuple.ToString());
                halcon.GetOneImageR().AddMeassge("最大清晰度:" + MaxTuple.ToString());
                if (MaxTuple.Length == 0)
                {
                    MaxTuple = hTuple;
                }
                else
                {
                    if (hTuple.TupleGreaterEqualElem(MaxTuple))
                    {
                        MaxTuple = hTuple;
                    }
                }
            }
            if (RotateTypeStr != "None")
            {
                HOperatorSet.MirrorImage(halcon.Image(), out HObject hObject, RotateTypeStr);
                halcon.Image(hObject);
            }
        }

        private Thread ThreadSatrReadCam;

        /// <summary>
        /// 线程执行
        /// </summary>
        public virtual void ThreadSatring(HalconRun halcon)
        {
            if (Grabbing)
            {
                return;
            }
            Grabbing = true;
            Frame = 0;
            //HOperatorSet.SetFramegrabberParam(this.m_AcqHandle, "AcquisitionMode", "Continuous");
            HOperatorSet.CountSeconds(out HTuple SecondsBegin);
            ThreadSatrReadCam = new Thread(() =>
            {
                while (Grabbing)
                {
                    try
                    {
                        Thread.Sleep(1);
                        halcon.UPStart();
                        this.GaedImageAsync(out HObject hObject);
                        halcon.Image(hObject);
                        ImageDefinition(halcon);
                        Frame++;
                        HOperatorSet.CountSeconds(out HTuple SecondsCurrent);
                        fps = Frame / (SecondsCurrent - SecondsBegin);
                        halcon.GetOneImageR().AddMeassge(fps.ToString("0.##") + "fps");
                        //halcon.ShowObj();
                        halcon.EndChanged(halcon.GetOneImageR());
                    }
                    catch (Exception)
                    {
                    }
                }
                //HOperatorSet.GrabImageStart (this.m_AcqHandle, Max);
            });
            ThreadSatrReadCam.Priority = ThreadPriority.Highest;
            ThreadSatrReadCam.Start();
        }

        /// <summary>
        /// 停止实时采集
        /// </summary>
        public void Stop()
        {
            Grabbing = false;
            Key = "One";
        }

        [Serializable]
        /// <summary>
        /// 相机信息
        /// </summary>
        public class Cam_information : ProjectNodet.IClickNodeProject
        {
            public string Name { get; set; }

            public static Cam_information In
            {
                get
                {
                    if (cam == null)
                    {
                        cam = new Cam_information();
                    }
                    return cam;
                }
            }

            private static Cam_information cam;

            /// <summary>
            /// 本地相机信息
            /// </summary>
            public static List<Cam_information> Cam_Information
            {
                get
                {
                    if (cam_Information == null)
                    {
                        cam_Information = SeekCam();
                    }

                    return cam_Information;
                }
            }

            private static List<Cam_information> cam_Information;

            /// <summary>
            /// 搜索本地相机参数
            /// </summary>
            /// <param name="intname">接口</param>
            /// <param name="cam_Information">相机详细信息</param>
            public static List<Cam_information> SeekCam(string intname = null)
            {
                HTuple info;
                try
                {
                    if (cam_Information == null)
                    {
                        cam_Information = new List<Cam_information>();
                    }
                    cam_Information.Clear();
                    if (intname == null)
                    {
                        intname = "GigEVision";
                    }
                    HTuple valueList = new HTuple();
                    try
                    {
                        HOperatorSet.InfoFramegrabber(intname, "info_boards", out info, out valueList);
                    }
                    catch (Exception ex)
                    {
                        if (intname == "GigEVision")
                        {
                            intname = "GigEVision2";
                            HOperatorSet.InfoFramegrabber(intname, "info_boards", out info, out valueList);
                        }
                    }
                    for (int i = 0; i < valueList.Length; i++)
                    {
                        Cam_information cam = new Cam_information();
                        string list = valueList[i];
                        for (int i1 = 0; i1 < list.Split('|').Length; i1++)
                        {
                            string dte = list.Split('|')[i1].Trim(' ');

                            if (dte.StartsWith("ip_address:") || dte.StartsWith("device_ip:"))
                            {
                                cam.IP = list.Split('|')[i1].Split(':')[1];
                            }
                            else if (dte.StartsWith("mac_address:"))
                            {
                                cam.Mac = list.Split('|')[i1].Split(':')[1];
                            }
                            else if (dte.StartsWith("interface_ip"))
                            {
                                cam.PC_IP = list.Split('|')[i1].Split(':')[1];
                            }
                            else if (dte.StartsWith("device:"))
                            {
                                cam.ID = list.Split('|')[i1].Split(':')[1];
                            }
                        }
                        cam.information = list;
                        cam_Information.Add(cam);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                return cam_Information;
            }

            //public void UpProperty(PropertyForm pertyForm, object data = null)
            //{
            //    camPragramV.Dock = DockStyle.Top;
            //    pertyForm.tabPage1.Controls.Add(camPragramV);
            //}

            public Control GetThisControl()
            {
                DahuaCams camPragra = new DahuaCams();
                return camPragra;
            }

            public Cam_information()
            {
                ID = Name = PC_IP = Mac = information = IP = "0";
            }

            /// <summary>
            /// 相机IP
            /// </summary>
            [Browsable(true)]
            [Description("相机的IP地址"), Category("硬件属性"), DefaultValue("")]
            public HTuple IP { get; set; }

            [Browsable(true)]
            [Description("相机的链接ID"), Category("硬件属性"), DefaultValue("")]
            /// <summary>
            /// 相机链接标识
            /// </summary>
            public HTuple ID { get; set; }

            [Browsable(true)]
            [Description("链接目标的IP"), Category("硬件属性"), DefaultValue("")]
            /// <summary>
            /// 相机链接目标IP
            /// </summary>
            public HTuple PC_IP { get; set; }

            [Browsable(true)]
            [Description("相机硬件Mac"), Category("硬件属性"), DefaultValue("")]
            /// <summary>
            /// 相机硬件标识
            /// </summary>
            public HTuple Mac { get; set; }

            [Browsable(true)]
            [Description("相机基本信息"), Category("基本信息"), DefaultValue("")]
            /// <summary>
            /// 基本信息集合
            /// </summary>
            public HTuple information { get; set; }
        }
    }
}