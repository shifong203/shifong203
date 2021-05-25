//using AVT.VmbAPINET;
//using ErosProjcetDLL.Project;
//using System;
//using System.Collections.Generic;

//namespace NokidaE.vision.Cams
//{
//    public class AVTCam : CamParam
//    {
//        private static Dictionary<string, AVTCam> AVTCamS = new Dictionary<string, AVTCam>();
//        private static Vimba vimba;

//        public override void UpProperty(PropertyForm property, object data)
//        {
//            base.UpProperty(property, data);
//            System.Windows.Forms.TabPage tabControl = new System.Windows.Forms.TabPage();
//            tabControl.Name = "集合";
//            tabControl.Text = "集合";
//            property.tabControl1.TabPages.Add(tabControl);
//        }

//        public static CameraCollection Cameras { get; set; }

//        public static void AvtCaStart()
//        {
//            if (vimba == null)
//            {
//                vimba = new Vimba();
//                vimba.Startup();
//            }
//            Cameras = vimba.Cameras;
//            foreach (Camera item in Cameras)
//            {
//                AVTCam aVTCam = new AVTCam();
//                AddAVTCam(item);
//            }
//        }

//        public void StartCamera()
//        {
//            Frame[] frames = new Frame[3];
//        }

//        private static void AddAVTCam(Camera camera)
//        {
//            //if (AVTCamS.ContainsKey(camera.Name))
//            // {
//            //     AVTCamS[camera.Name] = camera;
//            // }
//            // else
//            // {
//            //     AVTCamS.Add(camera.Name, camera);
//            // }
//        }

//        public void Up()
//        {
//        }

//        private Camera m_Camera = null;
//        private Feature feature = null;

//        public void SStartCamera()
//        {
//            Vimba sys = new Vimba();
//            CameraCollection cameras = null;
//            FeatureCollection features = null;

//            long payloadSize;
//            Frame[] frameArray = new Frame[3];
//            sys.Startup();
//            cameras = sys.Cameras;
//            if (cameras.Count == 0)
//            {
//                return;
//            }

//            m_Camera = cameras[0];
//            m_Camera.Close();
//            m_Camera.Open(VmbAccessModeType.VmbAccessModeFull);
//            m_Camera.OnFrameReceived +=
//            new Camera.OnFrameReceivedHandler(OnFrameReceived);
//            features = m_Camera.Features;
//            feature = features["PayloadSize"];
//            payloadSize = feature.IntValue;
//            for (int index = 0; index < frameArray.Length; ++index)
//            {
//                frameArray[index] = new Frame(payloadSize);
//                m_Camera.AnnounceFrame(frameArray[index]);
//            }
//            m_Camera.StartCapture();
//            for (int index = 0; index < frameArray.Length; ++index)
//            {
//                m_Camera.QueueFrame(frameArray[index]);
//            }
//            feature = features["AcquisitionMode"];
//            feature.EnumValue = "Continuous";
//            feature = features["AcquisitionStart"];
//            feature.RunCommand();
//        }

//        public void StartCont()
//        {
//            try
//            {
//                if (m_Camera==null)
//                {
//                    return;
//                }
//                long payloadSize;
//                FeatureCollection features = null;
//                features = m_Camera.Features;
//                feature = features["PayloadSize"];
//                payloadSize = feature.IntValue;
//                Frame frameAr = new Frame(feature.IntValue);
//                m_Camera.AcquireSingleImage(ref frameAr, 500);
//                m_Camera.StartContinuousImageAcquisition(0);
//            }
//            catch (Exception)
//            {
//            }
//        }

//        public void StopCamera()
//        {
          
//            if (m_Camera != null)
//            {
//                m_Camera.EndCapture();
//                m_Camera.FlushQueue();
//                m_Camera.RevokeAllFrames();
//                m_Camera.Close();
//                FeatureCollection features = m_Camera.Features;
//                Feature feature = features["AcquisitionStop"];
//                if (feature != null)
//                {
//                    feature.RunCommand();
//                }
//            }
         
//        }

//        private void OnFrameReceived(Frame frame)
//        {
//            //if (InvokeRequired) // if not from this thread invoke it in our context
//            //{
//            //    // In case of a separate thread (e.g. GUI ) use BeginInvoke to avoid a deadlock
//            //    Invoke(new Camera.OnFrameReceivedHandler(OnFrameReceived), frame);
//            //}

//            if (VmbFrameStatusType.VmbFrameStatusComplete == frame.ReceiveStatus)
//            {
//                Console.WriteLine("Frame status complete");
//            }
//            m_Camera.QueueFrame(frame);
//        }
//    }
//}