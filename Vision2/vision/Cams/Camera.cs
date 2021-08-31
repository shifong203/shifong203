using ErosSocket.ErosConLink;
using HalconDotNet;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using ThridLibray;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.Cams
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="CameraIndex"></param>
    public delegate void CameraStatusChangedHandle(int CameraIndex);

    public enum EnumCameraType
    {
        Dahua = 0,
        casewe = 1
    }

    /// <summary>
    /// 相机的基类
    /// </summary>
    public class Camera : ICamera
    {
        protected string name = "";

        protected string serialnum = "";

        public EnumCameraType cameratype = EnumCameraType.Dahua;

        private bool iscamconnected;

        public CameraStatusChangedHandle CameraStatusChanged;

        public event Action<string, OneResultOBj, int> Swtr;

        public event Action<bool> LinkEnvet;

        [Description("本地相机序号"), Category("硬件属性"), DisplayName("相机序号")]
        /// <summary>
        /// 相机编号
        /// </summary>

        public int Index { get; set; }

        [Description("相机自定义名称"), Category("基本属性"), DisplayName("相机名称")]
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        ///
        /// </summary>
        public string SerialNum { get { return serialnum; } set { serialnum = value; } }

        [Description("是否连接成功"), Category("Net"), DisplayName("连接状态")]

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsCamConnected { get { return iscamconnected; } set { iscamconnected = value; } }

        [Description("曝光时间微秒"), Category("采图属性"), DisplayName("曝光时间")]
        /// <summary>
        /// 曝光时间
        /// </summary>
        public virtual double ExposureTime { get { return expoursetime; } set { expoursetime = value; } }

        protected double expoursetime = 1000;

        [Description("相机类型"), Category("硬件属性"), DisplayName("相机类型")]
        public string Cameratype
        {
            get { return cameratype.ToString(); }
            set
            {
                foreach (EnumCameraType cam in Enum.GetValues(typeof(EnumCameraType)))
                {
                    if (cam.ToString() == value)
                    {
                        cameratype = cam;
                    }
                    else
                    {
                        cameratype = EnumCameraType.Dahua;
                    }
                }
            }
        }

        [Description("增益"), Category("采图属性"), DisplayName("增益")]
        public virtual double Gain
        { get { return gain; } set { gain = value; } }

        protected double gain = 1;

        [DescriptionAttribute("是否纠正图像"), Category("采图属性"), DisplayName("纠正图像")]
        public virtual double Gamma { get; set; }

        protected double gamma = 0.4;

        [Description("图像类型黑白或彩色"), Category("采图属性"), DisplayName("图像类型")]
        [TypeConverter(typeof(Vision2.ErosProjcetDLL.UI.PropertyGrid.ErosConverter)),
            Vision2.ErosProjcetDLL.UI.PropertyGrid.ErosConverter.ThisDropDown("", true, "Mono8", "BayerRG8")]
        public string CameraImageFormat { get; set; } = "BayerRG8";

        public string Key { get; set; }

        [Description("相机与PC接口IP"), Category("Net"), DisplayName("本地IP")]
        public virtual string IntIP { get; set; }

        [Description("相机IP"), Category("Net"), DisplayName("相机IP")]
        public virtual string IP { get; set; }

        public bool Grabbing { get; private set; }

        [Description("相机连接ID"), Category("硬件属性"), DisplayName("相机连接ID")]
        public string ID { get; set; }

        public int RunID { get; set; }

        public long RunTime { get; set; }

        [Description("像素与MM转换比例"), Category("像素转换"), DisplayName("像素转换比例")]
        public double CaliConst { get; set; } = 1;

        public int MaxNumbe { get; set; }

        [Description(""), Category("采图属性"), DisplayName("宽度")]
        public int Width { get; set; }

        [Description(""), Category("采图属性"), DisplayName("高度")]
        public int Height { get; set; }

        [DescriptionAttribute("灯光输出。"), Category("触发器"), DisplayName("灯光输出名称")]
        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string FlashLampName { get; set; }

        [DescriptionAttribute("触发灯光等待时间。"), Category("触发器"), DisplayName("灯光等待时间")]
        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public int FlashLampTime { get; set; } = 100;

        [Description("镜像角度,row上翻转，diagonal对角斜线翻转，column左右翻转，None无翻转"), Category("采图属性"),
        DisplayName("镜像角度"), TypeConverter(typeof(Vision2.ErosProjcetDLL.UI.PropertyGrid.ErosConverter)),
        Vision2.ErosProjcetDLL.UI.PropertyGrid.ErosConverter.ThisDropDown("", true, "row", "diagonal", "column", "None")]
        public string RotateTypeStr { get; set; } = "None";

        [DescriptionAttribute("实测精度。"), Category("镜头"), DisplayName("实际精度")]
        /// <summary>
        /// 理论精度
        /// </summary>
        public double CPrecision { get; set; }

        [DescriptionAttribute("理论精度。"), Category("镜头"), DisplayName("理论精度")]
        /// <summary>
        /// 理论精度
        /// </summary>
        public double Precision { get; set; }

        [DescriptionAttribute("镜头FOV视场大小。"), Category("镜头"), DisplayName("FOV视场")]
        /// <summary>
        /// FOV
        /// </summary>
        public string FOV { get; set; }

        [DescriptionAttribute("焦距。"), Category("内参"), DisplayName("焦距")]
        /// <summary>
        /// 焦距
        /// </summary>
        public double Focal { get; set; }

        [DescriptionAttribute("畸变。"), Category("内参"), DisplayName("畸变")]
        /// <summary>
        ///
        /// </summary>
        public double Kappa { get; set; }

        [DescriptionAttribute("畸变。"), Category("内参"), DisplayName("中心CY")]
        public double Cy { get; set; }

        [DescriptionAttribute("畸变。"), Category("内参"), DisplayName("中心CX")]
        public double Cx { get; set; }

        [DescriptionAttribute("像元Sy。"), Category("内参"), DisplayName("像元Sy")]
        public double Sy { get; set; }

        [DescriptionAttribute("像元Sx。"), Category("内参"), DisplayName("像元Sx")]
        public double Sx { get; set; }

        public HObject Map { get; set; }

        [DescriptionAttribute("是否纠正图像"), Category("标定"), DisplayName("纠正图像")]
        public bool ISMap { get; set; }

        public virtual void OpenCam()
        {
        }

        public void OnLinkEnvet(bool isc)
        {
            LinkEnvet?.Invoke(isc);
        }

        public virtual void CloseCam()
        {
        }

        /// <summary>
        /// 采集图片
        /// </summary>
        /// <returns></returns>
        public virtual HObject GetImage()
        {
            HObject ho_iamge = null;
            return ho_iamge;
        }

        public virtual string GetFramegrabberParam(string pName)
        {
            return "";
        }

        public virtual bool SetProgramValue(string pName, string value)
        {
            return false;
        }

        public virtual bool SetProgramValue(string pName, double value)
        {
            return false;
        }

        public void OnCameraStatusChanged(string key)
        {
            if (CameraStatusChanged != null)
            {
                CameraStatusChanged(this.Index);
            }
        }

        public virtual void Straing(HalconRun halconRun)
        {
            this.Grabbing = true;
            Vision.TriggerSetup(this.FlashLampName, true.ToString());
        }

        public virtual void Stop()
        {
            this.Grabbing = false;
            Vision.TriggerSetup(this.FlashLampName, false.ToString());
        }

        public virtual object GetIDevice()
        {
            return null;
        }

        public void OnEnverIamge(string key, int runid, OneResultOBj iamge)
        {
            this.Swtr.Invoke(key, iamge, runid);
        }

        public virtual void SetExposureTime(double VALUE)
        {
        }

        public virtual bool GetImage(out IGrabbedRawData image)
        {
            image = null;
            return false;
        }

        public virtual HObject IGrabbedRawDataTOImage(IGrabbedRawData frame)
        {
            HObject ho_image2 = new HObject();
            //frame.PixelFmt
            if (CameraImageFormat == "Mono8")
            {
                int nRGB = RGBFactory.EncodeLen(frame.Width, frame.Height, false);
                IntPtr pData = Marshal.AllocHGlobal(nRGB);
                Marshal.Copy(frame.Image, 0, pData, frame.ImageSize);
                HOperatorSet.GenImage1Extern(out ho_image2, "byte", frame.Width, frame.Height, (HTuple)pData, 0);
                Marshal.FreeHGlobal(pData);
            }
            //彩色图像
            else
            {
                int nRGB = RGBFactory.EncodeLen(frame.Width, frame.Height, true);
                IntPtr pData = Marshal.AllocHGlobal(nRGB);
                RGBFactory.ToRGB(frame.Image, frame.Width, frame.Height, true, frame.PixelFmt, pData, nRGB);
                HOperatorSet.GenImageInterleaved(out ho_image2, (HTuple)pData, "bgr", frame.Width, frame.Height, 0, "byte", frame.Width, frame.Height, 0, 0, 8, 0);
                Marshal.FreeHGlobal(pData);
            }
            if (RotateTypeStr != "None")
            {
                HOperatorSet.MirrorImage(ho_image2, out ho_image2, RotateTypeStr);
            }
            //frame.Dispose();
            return ho_image2;
        }
    }

    public class CamData
    {
        [Description("增益"), Category("采图属性"), DisplayName("增益")]
        public virtual double Gain
        { get { return gain; } set { gain = value; } }

        protected double gain = 1;

        [Description("曝光时间微秒"), Category("采图属性"), DisplayName("曝光时间")]
        /// <summary>
        /// 曝光时间
        /// </summary>
        public virtual double ExposureTime
        {
            get { return expoursetime; }
            set { expoursetime = value; }
        }

        protected double expoursetime = 1000;

        [DescriptionAttribute("伽马"), Category("采图属性"), DisplayName("伽马")]
        public double Gamma { get { return gamma; } set { gamma = value; } }

        protected double gamma = 1;

        [DescriptionAttribute("光源配置"), Category("采图属性"), DisplayName("光源配置")]
        public string Light_Source { get; set; } = "";
    }
}