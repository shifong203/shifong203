﻿using ErosSocket.ErosConLink;
using HalconDotNet;
using System;
using System.ComponentModel;
using System.Drawing.Design;
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
        protected int expoursetime = 1000;
        protected string serialnum = "";

        public EnumCameraType cameratype = EnumCameraType.Dahua;

        private bool iscamconnected;

        public CameraStatusChangedHandle CameraStatusChanged;

        public event Action<string, HObject, int, bool> Swtr;

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
        [Description("曝光时间微秒"), Category("图像属性"), DisplayName("曝光时间")]
        /// <summary>
        /// 曝光时间
        /// </summary>
        public virtual int ExposureTime { get { return expoursetime; } set { expoursetime = value; } }
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
        [Description("增益"), Category("图像属性"), DisplayName("增益")]
        public virtual int Gain
        { get { return gain; } set { gain = value; } }
        protected int gain = 1;
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

        public double CaliConst { get; set; } = 1;
        public int MaxNumbe { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        [Description("图像类型黑白或彩色"), Category("图像属性"), DisplayName("图像类型")]
        [TypeConverter(typeof(Vision2.ErosProjcetDLL.UI.PropertyGrid.ErosConverter)),
            Vision2.ErosProjcetDLL.UI.PropertyGrid.ErosConverter.ThisDropDown("", true, "Mono8", "BayerRG8")]
        public string CameraImageFormat { get; set; } = "BayerRG8";

        [DescriptionAttribute("灯光输出。"), Category("触发器"), DisplayName("灯光输出名称")]
        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string FlashLampName { get; set; }

        [DescriptionAttribute("触发灯光等待时间。"), Category("触发器"), DisplayName("灯光等待时间")]
        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public int FlashLampTime { get; set; } = 100;

        [Description("镜像角度,row上翻转，diagonal对角斜线翻转，column左右翻转，None无翻转"), Category("图像属性"),
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

        public void OnEnverIamge(string key, int runid, HObject iamge)
        {
            this.Swtr.Invoke(key, iamge, runid, true);
        }

        public virtual void SetExposureTime(int VALUE)
        {

        }
    }
}