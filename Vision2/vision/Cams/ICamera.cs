using HalconDotNet;
using System;
using ThridLibray;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.Cams
{
    public interface ICamera
    {
        void OnCameraStatusChanged(string key);

        void OnEnverIamge(string key, int runid, OneResultOBj iamge);

        void OpenCam();

        void CloseCam();

        HObject GetImage();

        bool GetImage(out IGrabbedRawData image);

        HObject IGrabbedRawDataTOImage(IGrabbedRawData data);

        object GetIDevice();

        void SetExposureTime(double VALUE);

        void Straing(HalconRun halconRun);

        event Action<string, OneResultOBj, int> Swtr;

        event Action<bool> LinkEnvet;

        //delegate void Sw(string key, HObject image, int runID, bool isSave = true);

        void Stop();

        double CPrecision { get; set; }

        /// <summary>
        ///
        /// </summary>
        double Precision { get; set; }

        /// <summary>
        ///
        /// </summary>
        string FOV { get; set; }

        /// <summary>
        ///
        /// </summary>
        double Focal { get; set; }

        /// <summary>
        ///
        /// </summary>
        double Kappa { get; set; }

        /// <summary>
        ///
        /// </summary>
        double Cy { get; set; }

        /// <summary>
        ///
        /// </summary>
        double Cx { get; set; }

        /// <summary>
        ///
        /// </summary>
        double Sy { get; set; }

        /// <summary>
        ///
        /// </summary>
        double Sx { get; set; }

        bool IsCamConnected { get; }

        string CameraImageFormat { get; set; }
        double ExposureTime { get; set; }
        double Gamma { get; set; }
        HObject Map { get; set; }

        bool ISMap { get; set; }
        int Width { get; set; }

        int Height { get; set; }

        double Gain { get; set; }
        string Key { get; set; }
        long RunTime { get; }

        double CaliConst { get; set; }

        int Index { get; set; }

        int MaxNumbe { get; set; }
        string ID { get; set; }
        int RunID { get; set; }
        string Name { get; set; }

        string IntIP { get; set; }

        string IP { get; set; }

        bool Grabbing { get; }

        string FlashLampName { get; set; }

        string GetFramegrabberParam(string pName);

        bool SetProgramValue(string pName, string value);

        bool SetProgramValue(string pName, double value);
    }
}