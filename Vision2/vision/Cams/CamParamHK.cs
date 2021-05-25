//using ErosProjcetDLL.Project;
//using ErosSocket.ErosConLink;
//using HalconDotNet;
////using MvCamCtrl.NET;
//using NokidaE.vision.HalconRunFile.RunProgramFile;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace NokidaE.vision.Cams
//{
//    public class CamParamHK : CamParam
//    {
//        #region 属性
//        /// <summary>
//        /// 设备集合
//        /// </summary>
//        public static MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList;
//        public static List<MyCamera.MV_CC_DEVICE_INFO> ListDevice;
//        public static Dictionary<string, MyCamera.MV_CC_DEVICE_INFO> KeyCams;

//        /// <summary>
//        /// 相机对象
//        /// </summary>
//        private MyCamera m_pMyCamera;
//        MyCamera.MV_CC_DEVICE_INFO device;



//        /// <summary>
//        /// 图像缓存
//        /// </summary>
//        byte[] m_pBufForSaveImage = new byte[50 * 1024 * 1024];
//        byte[] m_pDataForRed;
//        byte[] m_pDataForGreen;
//        byte[] m_pDataForBlue;

//        MyCamera.cbOutputExdelegate ImageCallback;

//        HObject Hobj = new HObject();
//        #endregion
//        public CamParamHK()
//        {
//            m_pMyCamera = new MyCamera();

//            Hobj.GenEmptyObj();
//        }
//        int nRet = -1;

//        /// <summary>
//        /// 连接相机
//        /// </summary>
//        /// <returns></returns>
//        protected override bool LiakCam()
//        {
//            int temp;
//            DeviceListAcq();
//            int index = 0;
//            //MyCamera.MV_CC_DEVICE_INFO device =
//            //    (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[index],
//            //                                                  typeof(MyCamera.MV_CC_DEVICE_INFO));
//            //更改IP后需要重新刷新设备列表，否则打开相机时会报错
//            //temp = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE, ref m_pDeviceList);//更新设备列表
//            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
//            {
//                device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));//获取设备
//                //nRet = m_pMyCamera.MV_CC_CreateDevice_NET(ref device);
//                //if (MyCamera.MV_OK != nRet)
//                //{
//                //    return false;
//                //}
//                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
//                {
//                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
//                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
//                    //m_SerialNumber = gigeInfo.chSerialNumber;//获取序列号
//                    if (this.m_IDStr.Equals(gigeInfo.chSerialNumber))
//                    {
//                        bool dst = MyCamera.MV_CC_IsDeviceAccessible_NET(ref device, MyCamera.MV_ACCESS_Exclusive);
//                        if (!dst)
//                        {
//                            ShowErrorMsg("不可访问:", 0);
//                            return false;
//                        }
//                        temp = m_pMyCamera.MV_CC_CreateDevice_NET(ref device);
//                        OffCam();
//                        temp = m_pMyCamera.MV_CC_CreateDevice_NET(ref device);
//                        if (MyCamera.MV_OK != temp)
//                        {
//                            //创建相机失败
//                            return false;
//                        }
//                        temp = m_pMyCamera.MV_CC_OpenDevice_NET();//
//                        if (MyCamera.MV_OK != temp)
//                        {
//                            ShowErrorMsg("打开设备失败:", temp);
//                            //打开相机失败
//                            return false;
//                        } 
//                        // ch:获取包大小 || en: Get Payload Size
//                        MyCamera.MVCC_INTVALUE stParam = new MyCamera.MVCC_INTVALUE();
//                        nRet = m_pMyCamera.MV_CC_GetIntValue_NET("PayloadSize", ref stParam);
//                        if (MyCamera.MV_OK != nRet)
//                        {
//                            MessageBox.Show("Get PayloadSize Fail");
//                            return false;
//                        }
//                        g_nPayloadSize = stParam.nCurValue;
//                        // ch:获取高 || en: Get Height
//                        nRet = m_pMyCamera.MV_CC_GetIntValue_NET("Height", ref stParam);
//                        if (MyCamera.MV_OK != nRet)
//                        {
//                            MessageBox.Show("Get Height Fail");
//                            return false;
//                        }
//                        uint nHeight = stParam.nCurValue;

//                        // ch:获取宽 || en: Get Width
//                        nRet = m_pMyCamera.MV_CC_GetIntValue_NET("Width", ref stParam);
//                        if (MyCamera.MV_OK != nRet)
//                        {
//                            MessageBox.Show("Get Width Fail");
//                            return false; 
//                        }
//                        uint nWidth = stParam.nCurValue;

//                        m_pDataForRed = new byte[nWidth * nHeight];
//                        m_pDataForGreen = new byte[nWidth * nHeight];
//                        m_pDataForBlue = new byte[nWidth * nHeight];


//                        SetFramegrabberParam();
//                        ImageCallback = new MyCamera.cbOutputExdelegate(GrabImage);
//                        nRet = m_pMyCamera.MV_CC_RegisterImageCallBackForRGB_NET(ImageCallback, IntPtr.Zero);
//                        if (MyCamera.MV_OK != nRet)
//                        {
//                            ShowErrorMsg("注册镜像回调失败:", temp);
//                            return false;
//                        }

//                        //GaedImage(out HObject hObject);
//                        //hObject.Dispose();
//                        return true;
//                    }
//                }

//                continue;
//            }
//            return false;
//        }

//        public override void SetGain()
//        {
//            nRet = m_pMyCamera.MV_CC_SetGain_NET(this.M_GainInt);
//            m_pMyCamera.MV_CC_SetIntValue_NET("GainRaw", this.M_GainInt);


//        }

//        public override void SetExposureTime(int exp = 0)
//        {
//            if (exp != 0)
//            {
//                nRet = m_pMyCamera.MV_CC_SetExposureTime_NET(exp);
//                nRet = m_pMyCamera.MV_CC_SetFloatValue_NET("ExposureTimeAbs", exp);
//            }
//            else
//            {
//                nRet = m_pMyCamera.MV_CC_SetExposureTime_NET(this.ExposureTimeAbs);
//                nRet = m_pMyCamera.MV_CC_SetFloatValue_NET("ExposureTimeAbs", this.ExposureTimeAbs);

//            }

//        }

//        public override double GetExposureTimeAbs()
//        {
//            MyCamera.MVCC_FLOATVALUE pstValue = new MyCamera.MVCC_FLOATVALUE();
//            nRet = m_pMyCamera.MV_CC_GetFloatValue_NET("ExposureTimeAbs", ref pstValue);
//            if (nRet == 0)
//            {
//                return pstValue.fCurValue;
//            }
//            return pstValue.fCurValue;
//        }

//        public override double GetGain()
//        {
//            MyCamera.MVCC_INTVALUE pstValue = new MyCamera.MVCC_INTVALUE();
//            nRet = m_pMyCamera.MV_CC_GetIntValue_NET("GainRaw", ref pstValue);
//            return (double)pstValue.nCurValue;
//        }


//        public override void GetFramegradderParam()
//        {
//            MyCamera.MV_NETTRANS_INFO pstValue = new MyCamera.MV_NETTRANS_INFO();
//            m_pMyCamera.MV_GIGE_GetNetTransInfo_NET(ref pstValue);

//        }

//        public override HTuple GetFramegrabberParam(string parmName)
//        {
//            MyCamera.MVCC_STRINGVALUE set;
//            set = new MyCamera.MVCC_STRINGVALUE();
//            nRet = m_pMyCamera.MV_CC_GetStringValue_NET(parmName, ref set);
//            MyCamera.MVCC_ENUMVALUE mVCC_ENUMVALUE = new MyCamera.MVCC_ENUMVALUE();

//            nRet = m_pMyCamera.MV_CC_GetEnumValue_NET(parmName, ref mVCC_ENUMVALUE);
//            MyCamera.MVCC_FLOATVALUE mVCC_FLOATVALUE = new MyCamera.MVCC_FLOATVALUE();


//            //nRet = m_pMyCamera.MV_CC_GetBalanceRatioRed_NET(parmName, ref mVCC_ENUMVALUE);
//            if (nRet != 0)
//            {
//                nRet = m_pMyCamera.MV_CC_GetFloatValue_NET(parmName, ref mVCC_FLOATVALUE);
//                if (nRet==0)
//                {
//                    return mVCC_FLOATVALUE.fCurValue;
//                }
//                return null;
//            }
//            return mVCC_ENUMVALUE.nCurValue;
//        }


//        public override string GetIP()
//        {
//            try
//            {
//                IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
//                MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
//                // 显示IP
//                UInt32 nIp1 = (gigeInfo.nCurrentIp & 0xFF000000) >> 24;
//                UInt32 nIp2 = (gigeInfo.nCurrentIp & 0x00FF0000) >> 16;
//                UInt32 nIp3 = (gigeInfo.nCurrentIp & 0x0000FF00) >> 8;
//                UInt32 nIp4 = (gigeInfo.nCurrentIp & 0x000000FF);
//                this.camS_Information.IP = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();
//            }
//            catch (Exception)
//            {
//            }
//            return this.camS_Information.IP;

//        }

//        public override void SetFrameGradderIP(string ip)
//        {
//            try
//            {
//                nRet = m_pMyCamera.MV_CC_SetBoolValue_NET("GevCurrentIPConfigurationPersistentIP", true);
//                nRet = m_pMyCamera.MV_CC_SetBoolValue_NET("GevCurrentIPConfigurationDHCP", false);


//                //nRet = m_pMyCamera.MV_CC_SetIntValueEx_NET("GevCurrentIPConfiguration", 1);
//                // ch:IP转换 | en:IP conversion
//                IPAddress clsIpAddr;
//                if (false == IPAddress.TryParse(ip, out clsIpAddr))
//                {
//                    ShowErrorMsg("Please enter correct IP", 0);
//                    return;
//                }
//                long nIp = IPAddress.NetworkToHostOrder(clsIpAddr.Address);
//                // ch:掩码转换 | en:Mask conversion
//                IPAddress clsSubMask;
//                if (false == IPAddress.TryParse("255.255.255.0", out clsSubMask))
//                {
//                    ShowErrorMsg("Please enter correct IP", 0);
//                    return;
//                }
//                long nSubMask = IPAddress.NetworkToHostOrder(clsSubMask.Address);

//                // ch:网关转换 | en:Gateway conversion
//                IPAddress clsDefaultWay;
//                if (false == IPAddress.TryParse("192.168.0.1", out clsDefaultWay))
//                {
//                    ShowErrorMsg("Please enter correct IP", 0);
//                    return;
//                }
//                long nDefaultWay = IPAddress.NetworkToHostOrder(clsDefaultWay.Address);
//                // ch:打开设备 | en:Open device
//                if (null == m_pMyCamera)
//                {
//                    m_pMyCamera = new MyCamera();
//                }
//                nRet = m_pMyCamera.MV_CC_CreateDevice_NET(ref device);

//                if (MyCamera.MV_OK != nRet)
//                {
//                    return;
//                }
//                m_pMyCamera.MV_CC_OpenDevice_NET();
//                uint nType = MyCamera.MV_IP_CFG_STATIC; //固定IP地址模式
//                nRet = m_pMyCamera.MV_GIGE_SetIpConfig_NET(nType);
//                //if (0 != nRet)
//                //    ShowErrorMsg("Please enter correct IP", nRet);
//                nRet = m_pMyCamera.MV_GIGE_ForceIpEx_NET((uint)(nIp >> 32), (uint)(nSubMask >> 32), (uint)(nDefaultWay >> 32));
//                GC.Collect();
//                if (nRet != MyCamera.MV_OK)
//                {
//                    ShowErrorMsg("设置失败", nRet);
//                }
//                nRet = m_pMyCamera.MV_CC_SetCommandValue_NET("DeviceReset");
//            }
//            catch (Exception)
//            {
//            }
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="ip"></param>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public static bool SetCamIP(string ip, string id)
//        {
//            MyCamera m_pMyCamera = new MyCamera();
//            int nRet = m_pMyCamera.MV_CC_SetIntValueEx_NET("GevCurrentIPConfiguration", 1);
//            // ch:IP转换 | en:IP conversion
//            IPAddress clsIpAddr;
//            if (false == IPAddress.TryParse(ip, out clsIpAddr))
//            {
//                MessageBox.Show("IP地址错误");
//                return false;
//            }
//            long nIp = IPAddress.NetworkToHostOrder(clsIpAddr.Address);
//            // ch:掩码转换 | en:Mask conversion
//            IPAddress clsSubMask;
//            if (false == IPAddress.TryParse("255.255.255.0", out clsSubMask))
//            {
//                MessageBox.Show("网关地址错误");
//                return false;
//            }
//            long nSubMask = IPAddress.NetworkToHostOrder(clsSubMask.Address);
//            // ch:网关转换 | en:Gateway conversion
//            IPAddress clsDefaultWay;
//            if (false == IPAddress.TryParse("192.168.0.1", out clsDefaultWay))
//            {
//                MessageBox.Show("网关地址错误");
//                return false;
//            }
//            long nDefaultWay = IPAddress.NetworkToHostOrder(clsDefaultWay.Address);
//            MyCamera.MV_CC_DEVICE_INFO device;
//            // ch:打开设备 | en:Open device
//            for (int i = 0; i < ListDevice.Count; i++)
//            {
//                device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));//获取设备
//                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
//                {
//                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
//                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
//                    //m_SerialNumber = gigeInfo.chSerialNumber;//获取序列号
//                    if (id.Equals(gigeInfo.chSerialNumber))
//                    {
//                        nRet = m_pMyCamera.MV_CC_CreateDevice_NET(ref device);
//                        if (MyCamera.MV_OK != nRet)
//                        {
//                            return false;
//                        }
//                        nRet = m_pMyCamera.MV_GIGE_ForceIpEx_NET((uint)(nIp >> 32), (uint)(nSubMask >> 32), (uint)(nDefaultWay >> 32));
//                        GC.Collect();
//                        if (nRet != MyCamera.MV_OK)
//                        {
//                            ShowErrorMsg("设置失败", nRet);
//                        }
//                        m_pMyCamera.MV_CC_CloseDevice_NET();
//                        m_pMyCamera.MV_CC_DestroyDevice_NET();
//                        return true;
//                    }
//                }
//            }
//            return false;
//        }

//        /// <summary>
//        /// 登录文本信息
//        /// </summary>
//        /// <param name="csMessage">错误信息</param>
//        /// <param name="nErrorNum">错误编号</param>
//        private static void ShowErrorMsg(string csMessage, int nErrorNum)
//        {
//            string errorMsg;
//            if (nErrorNum == 0)
//            {
//                if (csMessage != "")
//                {
//                    ErosProjcetDLL.Project.AlarmText.LogWarning("相机信息", csMessage);
//                }
//                return;
//                //errorMsg = this.Name +csMessage;
//            }
//            else
//            {
//                errorMsg = csMessage + ": Error =" + String.Format("{0:X}", nErrorNum);
//            }

//            switch (nErrorNum)
//            {
//                case MyCamera.MV_E_HANDLE: errorMsg += "错误或无效句柄/Error or invalid handle "; break;
//                case MyCamera.MV_E_SUPPORT: errorMsg += " 不支持功能/Not supported function "; break;
//                case MyCamera.MV_E_BUFOVER: errorMsg += " 缓存已满/Cache is full "; break;
//                case MyCamera.MV_E_CALLORDER: errorMsg += "函数调用顺序错误/ Function calling order error "; break;
//                case MyCamera.MV_E_PARAMETER: errorMsg += " 不正确的参数/Incorrect parameter "; break;
//                case MyCamera.MV_E_RESOURCE: errorMsg += " 应用资源失败/Applying resource failed "; break;
//                case MyCamera.MV_E_NODATA: errorMsg += " 无数据/No data "; break;
//                case MyCamera.MV_E_PRECONDITION: errorMsg += "前置条件错误，或运行环境改变/ Precondition error, or running environment changed "; break;
//                case MyCamera.MV_E_VERSION: errorMsg += " 版本不匹配/Version mismatches "; break;
//                case MyCamera.MV_E_NOENOUGH_BUF: errorMsg += "内存空间不足/ Insufficient memory "; break;
//                case MyCamera.MV_E_UNKNOW: errorMsg += "未知的错误/ Unknown error "; break;
//                case MyCamera.MV_E_GC_GENERIC: errorMsg += "一般的错误/ General error "; break;
//                case MyCamera.MV_E_GC_ACCESS: errorMsg += "节点访问条件错误/ Node accessing condition error "; break;
//                case MyCamera.MV_E_ACCESS_DENIED: errorMsg += "设备无没有权限"; break;
//                case MyCamera.MV_E_BUSY: errorMsg += "设备忙，或网络断开/ Device is busy, or network disconnected "; break;
//                case MyCamera.MV_E_NETER: errorMsg += "网络错误/ Network error "; break;
//            }
//            ErosProjcetDLL.Project.AlarmText.LogWarning("相机错误", errorMsg);
//            //MessageBox.Show(errorMsg, "PROMPT");
//        }
//        public static void CloseCam(MyCamera.MV_CC_DEVICE_INFO mV_CC)
//        {
//            try
//            {
//                MyCamera m_pMyCa = new MyCamera();
//                int temp = m_pMyCa.MV_CC_CreateDevice_NET(ref mV_CC);
//                temp = m_pMyCa.MV_CC_StopGrabbing_NET();
//                temp = m_pMyCa.MV_CC_CloseDevice_NET();
//                Thread.Sleep(50);
//                temp = m_pMyCa.MV_CC_DestroyDevice_NET();
//                //m_pMyCa.d
//            }
//            catch (Exception)
//            {
//            }

//        }
//        public override void OffCam()
//        {
//            try
//            {

//                Key = "One";
//                m_pMyCamera.MV_CC_StopGrabbing_NET();
//                //int temp = m_pMyCamera.MV_CC_CreateDevice_NET(ref device);
//                if (Grabbing)
//                {
//                    Grabbing = false;
//                    // ch:停止抓图 || en:Stop grab image
//                }
//                nRet = m_pMyCamera.MV_CC_StopGrabbing_NET();
//                nRet = m_pMyCamera.MV_CC_CloseDevice_NET();
//                Thread.Sleep(50);
//                nRet = m_pMyCamera.MV_CC_DestroyDevice_NET();
//                ShowErrorMsg("", nRet);
//                Thread.Sleep(50);

//            }
//            catch (Exception)
//            {
//            }
//            base.OffCam();
//        }
//        /// <summary>
//        /// 连接相机
//        /// </summary>
//        /// <param name="id">ID</param>
//        /// <returns>成功true</returns>
//        public bool ConnectCamera(string id)
//        {
//            this.m_IDStr = id;
//            return LiakCam();
//        }
//        //成功返回0失败返回-1
//        //调用函数时可以传入需要改变的目标IP，如过没有传入则将相机IP设置为其所连接的网卡地址+1或-1
//        public int ChangeIP(MyCamera.MV_CC_DEVICE_INFO deviceInfo, string IP = "")
//        {
//            try
//            {
//                //获取相机相关信息，例如相机所连接网卡的网址
//                IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(deviceInfo.SpecialInfo.stGigEInfo, 0);
//                MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
//                IPAddress cameraIPAddress;
//                string tempStr = "";
//                if (IP.Trim().Equals("") || !(IPAddress.TryParse(IP, out cameraIPAddress)))
//                {
//                    //当前网卡的IP地址
//                    UInt32 nNetIp1 = (gigeInfo.nNetExport & 0xFF000000) >> 24;
//                    UInt32 nNetIp2 = (gigeInfo.nNetExport & 0x00FF0000) >> 16;
//                    UInt32 nNetIp3 = (gigeInfo.nNetExport & 0x0000FF00) >> 8;
//                    UInt32 nNetIp4 = (gigeInfo.nNetExport & 0x000000FF);
//                    //根据网卡IP设定相机IP，如果网卡ip第四位小于252，则相机ip第四位+1，否则相机IP第四位-1
//                    UInt32 cameraIp1 = nNetIp1;
//                    UInt32 cameraIp2 = nNetIp2;
//                    UInt32 cameraIp3 = nNetIp3;
//                    UInt32 cameraIp4 = nNetIp4;
//                    if (nNetIp4 < 252)
//                    {
//                        cameraIp4++;
//                    }
//                    else
//                    {
//                        cameraIp4--;
//                    }
//                    tempStr = cameraIp1 + "." + cameraIp2 + "." + cameraIp3 + "." + cameraIp4;
//                }
//                else
//                {
//                    tempStr = IP;
//                }
//                IPAddress.TryParse(tempStr, out cameraIPAddress);
//                long cameraIP = IPAddress.NetworkToHostOrder(cameraIPAddress.Address);
//                //设置相机掩码
//                uint maskIp1 = (gigeInfo.nCurrentSubNetMask & 0xFF000000) >> 24;
//                uint maskIp2 = (gigeInfo.nCurrentSubNetMask & 0x00FF0000) >> 16;
//                uint maskIp3 = (gigeInfo.nCurrentSubNetMask & 0x0000FF00) >> 8;
//                uint maskIp4 = (gigeInfo.nCurrentSubNetMask & 0x000000FF);
//                IPAddress subMaskAddress;
//                tempStr = maskIp1 + "." + maskIp2 + "." + maskIp3 + "." + maskIp4;
//                IPAddress.TryParse(tempStr, out subMaskAddress);
//                long maskIP = IPAddress.NetworkToHostOrder(subMaskAddress.Address);
//                //设置网关
//                uint gateIp1 = (gigeInfo.nDefultGateWay & 0xFF000000) >> 24;
//                uint gateIp2 = (gigeInfo.nDefultGateWay & 0x00FF0000) >> 16;
//                uint gateIp3 = (gigeInfo.nDefultGateWay & 0x0000FF00) >> 8;
//                uint gateIp4 = (gigeInfo.nDefultGateWay & 0x000000FF);
//                IPAddress gateAddress;
//                tempStr = gateIp1 + "." + gateIp2 + "." + gateIp3 + "." + gateIp4;
//                IPAddress.TryParse(tempStr, out gateAddress);
//                long gateIP = IPAddress.NetworkToHostOrder(gateAddress.Address);
//                int temp;
//                //temp= m_pMyCamera.MV_GIGE_ForceIp_NET.);//执行更改相机IP的命令
//                temp = m_pMyCamera.MV_GIGE_ForceIpEx_NET((UInt32)(cameraIP >> 32), (UInt32)(maskIP >> 32), (UInt32)(gateIP >> 32));//执行更改相机IP的命令
//                if (temp == 0)
//                    //强制IP成功
//                    return 0;
//                //强制IP失败
//                return -1;
//            }
//            catch
//            {
//                return -1;
//            }
//        }
//        HalconRun ha;

//        HTuple SecondsBegin;
//        /// <summary>
//        /// 实时釆图
//        /// </summary>
//        /// <param name="halcon"></param>
//        public override void ThreadSatring(HalconRun halcon)
//        {
//            if (!m_bCamIsCon)
//            {
//                return;
//            }

//            numberV = 0;
//            if (Grabbing)
//            {
//                return;
//            }
//            Frame = 0;
//            fps = 0;
//            int nRet;

//                Vision.TriggerSetup(this.FlashLampName, true.ToString());

//            MyCamera.MVCC_ENUMVALUE PSTVALUE = new MyCamera.MVCC_ENUMVALUE();

//            //nRet = m_pMyCamera.MV_CC_SetStringValue_NET("TriggerSource", "Software");
//            nRet = m_pMyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
//            m_pMyCamera.MV_CC_StopGrabbing_NET();
//            RealTimeMode = true;
//            m_pMyCamera.MV_CC_GetEnumValue_NET("TriggerSource", ref PSTVALUE);
//            nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerSource", 0);
//            m_pMyCamera.MV_CC_GetTriggerMode_NET(ref PSTVALUE);
//            m_pMyCamera.MV_CC_GetEnumValue_NET("TriggerMode", ref PSTVALUE);
//            nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerMode",0);
//            if (nRet != 0)
//            {
//                ShowErrorMsg("", nRet);
//            }
//            ha = halcon;
//            //开始采集
//            nRet = m_pMyCamera.MV_CC_StartGrabbing_NET();
//            HOperatorSet.CountSeconds(out SecondsBegin);
//            //标志位置位true
//            Grabbing = true;
//        }

//        /// <summary>
//        /// 采集单张图像
//        /// </summary>
//        /// <param name="image"></param>
//        /// <returns></returns>
//        public override bool GaedImage(out HObject image)
//        {
//            if (!m_bCamIsCon)
//            {
//                this.OnLinkSt(false);
//            }
//            image = new HObject();

//            Grabbing = false;
//            image.GenEmptyObj();
//            Vision.TriggerSetup(this.OKName, false.ToString());
//            RealTimeMode = false;
//            try
//            {
//                m_pMyCamera.MV_CC_StopGrabbing_NET();
//                MyCamera.MVCC_STRINGVALUE mVCC_STRINGVALUE = new MyCamera.MVCC_STRINGVALUE();
//                nRet = m_pMyCamera.MV_CC_GetStringValue_NET("DeviceID", ref mVCC_STRINGVALUE);
//                MyCamera.MVCC_ENUMVALUE mVCC_ENUMVALUET = new MyCamera.MVCC_ENUMVALUE();
//                nRet = m_pMyCamera.MV_CC_GetStringValue_NET("DeviceModelName", ref mVCC_STRINGVALUE);
//                MyCamera.MVCC_INTVALUE mVCC_INTVALUE_EX = new MyCamera.MVCC_INTVALUE();
//                nRet = m_pMyCamera.MV_CC_GetIntValue_NET("Width", ref mVCC_INTVALUE_EX);
//                nRet = m_pMyCamera.MV_CC_GetEnumValue_NET("TriggerMode", ref mVCC_ENUMVALUET);
//                nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerMode", 1);
//                MyCamera.MVCC_ENUMVALUE mVCC_ENUMVALUE = new MyCamera.MVCC_ENUMVALUE();
//                m_pMyCamera.MV_CC_GetBalanceWhiteAuto_NET(ref mVCC_ENUMVALUE);

//                nRet = m_pMyCamera.MV_CC_GetEnumValue_NET("TriggerSource", ref mVCC_ENUMVALUE);
//                if (mVCC_ENUMVALUE.nSupportedNum == 2)
//                {
//                    nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerSource", 0);
//                }
//                else
//                {
//                    nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerSource", 7);
//                    if (nRet != 0)
//                    {
//                        nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerSource", 6);
//                        if (nRet!=0)
//                        {
//                            nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerSource", 4);
//                        }
//                    }
//                }


//                ////m_pMyCamera.MV_CC_GetOneFrameTimeout_NET("TriggerSource", 6);
//                // ch:开启抓图 | en:start grab
//                nRet = m_pMyCamera.MV_CC_StartGrabbing_NET();
//                //m_bGrabbing = true;
//                nRet = m_pMyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
//                if (MyCamera.MV_OK != nRet)
//                {
//                    this.OnLinkSt(false);
//                    return false;
//                }
//                return true;
//            }
//            catch (Exception ex)
//            {


//            }
//            return false;
//        }

//        public Int32 ConvertToRGB(object obj, IntPtr pSrc, ushort nHeight, ushort nWidth, MyCamera.MvGvspPixelType nPixelType, IntPtr pDst)
//        {
//            if (IntPtr.Zero == pSrc || IntPtr.Zero == pDst)
//            {
//                return MyCamera.MV_E_PARAMETER;
//            }

//            int nRet = MyCamera.MV_OK;
//            MyCamera device = obj as MyCamera;
//            MyCamera.MV_PIXEL_CONVERT_PARAM stPixelConvertParam = new MyCamera.MV_PIXEL_CONVERT_PARAM();

//            stPixelConvertParam.pSrcData = pSrc;//源数据
//            if (IntPtr.Zero == stPixelConvertParam.pSrcData)
//            {
//                return -1;
//            }

//            stPixelConvertParam.nWidth = nWidth;//图像宽度
//            stPixelConvertParam.nHeight = nHeight;//图像高度
//            stPixelConvertParam.enSrcPixelType = nPixelType;//源数据的格式
//            stPixelConvertParam.nSrcDataLen = (uint)(nWidth * nHeight * ((((uint)nPixelType) >> 16) & 0x00ff) >> 3);

//            stPixelConvertParam.nDstBufferSize = (uint)(nWidth * nHeight * ((((uint)MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed) >> 16) & 0x00ff) >> 3);
//            stPixelConvertParam.pDstBuffer = pDst;//转换后的数据
//            stPixelConvertParam.enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed;
//            stPixelConvertParam.nDstBufferSize = (uint)nWidth * nHeight * 3;

//            nRet = device.MV_CC_ConvertPixelType_NET(ref stPixelConvertParam);//格式转换
//            if (MyCamera.MV_OK != nRet)
//            {
//                return -1;
//            }

//            return MyCamera.MV_OK;
//        }
//        //
//        private bool IsColorPixelFormat(MyCamera.MvGvspPixelType enType)
//        {
//            switch (enType)
//            {
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGBA8_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BGRA8_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_YUYV_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12_Packed:
//                    return true;
//                default:
//                    return false;
//            }
//        }

//        private bool IsMonoPixelFormat(MyCamera.MvGvspPixelType enType)
//        {
//            switch (enType)
//            {
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12:
//                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
//                    return true;
//                default:
//                    return false;
//            }
//        }


//        public Int32 ConvertToMono8(object obj, IntPtr pInData, IntPtr pOutData, ushort nHeight, ushort nWidth, MyCamera.MvGvspPixelType nPixelType)
//        {
//            if (IntPtr.Zero == pInData || IntPtr.Zero == pOutData)
//            {
//                return MyCamera.MV_E_PARAMETER;
//            }

//            int nRet = MyCamera.MV_OK;
//            MyCamera device = obj as MyCamera;
//            MyCamera.MV_PIXEL_CONVERT_PARAM stPixelConvertParam = new MyCamera.MV_PIXEL_CONVERT_PARAM();

//            stPixelConvertParam.pSrcData = pInData;//源数据
//            if (IntPtr.Zero == stPixelConvertParam.pSrcData)
//            {
//                return -1;
//            }

//            stPixelConvertParam.nWidth = nWidth;//图像宽度
//            stPixelConvertParam.nHeight = nHeight;//图像高度
//            stPixelConvertParam.enSrcPixelType = nPixelType;//源数据的格式
//            stPixelConvertParam.nSrcDataLen = (uint)(nWidth * nHeight * ((((uint)nPixelType) >> 16) & 0x00ff) >> 3);

//            stPixelConvertParam.nDstBufferSize = (uint)(nWidth * nHeight * ((((uint)MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed) >> 16) & 0x00ff) >> 3);
//            stPixelConvertParam.pDstBuffer = pOutData;//转换后的数据
//            stPixelConvertParam.enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8;
//            stPixelConvertParam.nDstBufferSize = (uint)(nWidth * nHeight * 3);

//            nRet = device.MV_CC_ConvertPixelType_NET(ref stPixelConvertParam);//格式转换
//            if (MyCamera.MV_OK != nRet)
//            {
//                return -1;
//            }

//            return nRet;
//        }
//        //MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList;
//        //private MyCamera m_pMyCamera;
//        HWindow m_Window;
//        bool m_bGrabbing;
//        //byte[] m_pDataForRed = new byte[20 * 1024 * 1024];
//        //byte[] m_pDataForGreen = new byte[20 * 1024 * 1024];
//        //byte[] m_pDataForBlue = new byte[20 * 1024 * 1024];
//        uint g_nPayloadSize = 0;

//        //接收图像线程
//        private void ReceiveImageWorkThread(object obj)
//        {

//            int nRet = MyCamera.MV_OK;
//            MyCamera device = obj as MyCamera;
//            MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();
//            IntPtr pData = Marshal.AllocHGlobal((int)g_nPayloadSize * 3);
//            if (pData == IntPtr.Zero)
//            {
//                return;
//            }
//            IntPtr pImageBuffer = Marshal.AllocHGlobal((int)g_nPayloadSize * 3);
//            if (pImageBuffer == IntPtr.Zero)
//            {
//                return;
//            }

//            uint nDataSize = g_nPayloadSize * 3;
//            HObject Hobj = new HObject();
//            IntPtr RedPtr = IntPtr.Zero;
//            IntPtr GreenPtr = IntPtr.Zero;
//            IntPtr BluePtr = IntPtr.Zero;
//            IntPtr pTemp = IntPtr.Zero;

//            while (m_bGrabbing)
//            {
//                nRet = device.MV_CC_GetOneFrameTimeout_NET(pData, nDataSize, ref pFrameInfo, 1000);
//                if (MyCamera.MV_OK == nRet)
//                {
//                    if (IsColorPixelFormat(pFrameInfo.enPixelType))
//                    {
//                        if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed)
//                        {
//                            pTemp = pData;
//                        }
//                        else
//                        {
//                            nRet = ConvertToRGB(obj, pData, pFrameInfo.nHeight, pFrameInfo.nWidth, pFrameInfo.enPixelType, pImageBuffer);
//                            if (MyCamera.MV_OK != nRet)
//                            {
//                                return;
//                            }
//                            pTemp = pImageBuffer;
//                        }

//                        unsafe
//                        {
//                            byte* pBufForSaveImage = (byte*)pTemp;

//                            UInt32 nSupWidth = (pFrameInfo.nWidth + (UInt32)3) & 0xfffffffc;

//                            for (int nRow = 0; nRow < pFrameInfo.nHeight; nRow++)
//                            {
//                                for (int col = 0; col < pFrameInfo.nWidth; col++)
//                                {
//                                    m_pDataForRed[nRow * nSupWidth + col] = pBufForSaveImage[nRow * pFrameInfo.nWidth * 3 + (3 * col)];
//                                    m_pDataForGreen[nRow * nSupWidth + col] = pBufForSaveImage[nRow * pFrameInfo.nWidth * 3 + (3 * col + 1)];
//                                    m_pDataForBlue[nRow * nSupWidth + col] = pBufForSaveImage[nRow * pFrameInfo.nWidth * 3 + (3 * col + 2)];
//                                }
//                            }
//                        }

//                        RedPtr = Marshal.UnsafeAddrOfPinnedArrayElement(m_pDataForRed, 0);
//                        GreenPtr = Marshal.UnsafeAddrOfPinnedArrayElement(m_pDataForGreen, 0);
//                        BluePtr = Marshal.UnsafeAddrOfPinnedArrayElement(m_pDataForBlue, 0);

//                        try
//                        {
//                            HOperatorSet.GenImage3Extern(out Hobj, (HTuple)"byte", pFrameInfo.nWidth, pFrameInfo.nHeight,
//                                                (new HTuple(RedPtr)), (new HTuple(GreenPtr)), (new HTuple(BluePtr)), IntPtr.Zero);
//                        }
//                        catch (System.Exception ex)
//                        {
//                            MessageBox.Show(ex.ToString());
//                        }
//                    }
//                    else if (IsMonoPixelFormat(pFrameInfo.enPixelType))
//                    {
//                        if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8)
//                        {
//                            pTemp = pData;
//                        }
//                        else
//                        {
//                            nRet = ConvertToMono8(device, pData, pImageBuffer, pFrameInfo.nHeight, pFrameInfo.nWidth, pFrameInfo.enPixelType);
//                            if (MyCamera.MV_OK != nRet)
//                            {
//                                return;
//                            }
//                            pTemp = pImageBuffer;
//                        }
//                        try
//                        {
//                            HOperatorSet.GenImage1Extern(out Hobj, "byte", pFrameInfo.nWidth, pFrameInfo.nHeight, pTemp, IntPtr.Zero);
//                        }
//                        catch (System.Exception ex)
//                        {
//                            MessageBox.Show(ex.ToString());
//                            return;
//                        }
//                    }
//                    else
//                    {
//                        continue;
//                    }
//                    if (null != Hobj)
//                    {
//                        //Global.myHobj = Hobj;
//                        //Global.iSGetImgObj = true;
//                    }
//                    else
//                    {
//                        MessageBox.Show("No picObj!");
//                    }

//                    // HalconDisplay(m_Window, Hobj, pFrameInfo.nHeight, pFrameInfo.nWidth);
//                }
//                else
//                {
//                    continue;
//                }
//            }
//            if (pData != IntPtr.Zero)
//            {
//                Marshal.FreeHGlobal(pData);
//            }
//            if (pImageBuffer != IntPtr.Zero)
//            {
//                Marshal.FreeHGlobal(pImageBuffer);
//            }
//            return;
//        }

//        public override void SetProgramValue(string pragrmName, HTuple value)
//        {
//            try
//            {
//                if (uint.TryParse(value.ToString(), out uint ret))
//                {
//                    nRet = m_pMyCamera.MV_CC_SetEnumValue_NET(pragrmName, (uint)ret);
//                    if (nRet!=0)
//                    {
//                        if (float.TryParse(value.ToString(), out float rett))
//                        {

//                            nRet = m_pMyCamera.MV_CC_SetFloatValue_NET(pragrmName, rett);
//                        }
//                    }
//                }
//                else if (float.TryParse(value.ToString(), out float rett))
//                {

//                    nRet = m_pMyCamera.MV_CC_SetFloatValue_NET(pragrmName, rett);
//                }

//            }
//            catch (Exception)
//            {


//            }
//            //nRet = m_pMyCamera.MV_CC_SetBalanceWhiteAuto_NET(1);



//        }

//        public override void SetFramegrabberParam(string name = null, HTuple value = null)
//        {
//            this.SetGain();
//            this.SetExposureTime();
//            if (name != null)
//            {
//                nRet = m_pMyCamera.MV_CC_SetEnumValueByString_NET(name, value);
//            }
//            //base.SetFramegrabberParam(name, value);
//        }


//        int numberV;
//        public override bool Run(HalconRun halcon)
//        {
//            Watch.Restart();
//            Vision.TriggerSetup(this.FlashLampName, true.ToString());
//            int nRet;

//            // ch:开启抓图 | en:start grab
//            nRet = m_pMyCamera.MV_CC_StartGrabbing_NET();
//            if (MyCamera.MV_OK != nRet)
//            {
//                MessageBox.Show("Start Grabbing Fail");
//                return false;
//            }
//            m_bGrabbing = true;

//            Thread hReceiveImageThreadHandle = new Thread(ReceiveImageWorkThread);
//            hReceiveImageThreadHandle.Start(m_pMyCamera);

//            //bool iscong = GaedImage(out HObject hObject);
//            //ImageDefinition(halcon);

//            return false;
//        }
//        public bool ISRGB { get; set; }

//        /**********************************************************************************************************/
//        private void GrabImage(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
//        {
//            try
//            {
//                if (!RealTimeMode)
//                {
//                    Vision.TriggerSetup(this.FlashLampName, false.ToString());
//                }
//                Hobj.Dispose();
//                if (pData != null)
//                {
//                    uint nWidth = pFrameInfo.nWidth;
//                    uint nHeight = pFrameInfo.nHeight;
//                    if (m_pBufForSaveImage.Length < (int)nWidth * (int)nHeight * 3)
//                    {
//                        m_pBufForSaveImage = new byte[(int)nWidth * (int)nHeight * 3];
//                    }
//                    Marshal.Copy(pData, m_pBufForSaveImage, 0, (int)nWidth * (int)nHeight * 3);
//                    UInt32 nSupWidth = (pFrameInfo.nWidth + (UInt32)3) & 0xfffffffc;
//                    if (m_pDataForRed == null || m_pDataForRed.Length != (int)nWidth * (int)nHeight)
//                    {
//                        m_pDataForRed = new byte[(int)nWidth * (int)nHeight];
//                    }
//                    if (ISRGB)
//                    {
//                        if (m_pDataForGreen == null || m_pDataForGreen.Length != (int)nWidth * (int)nHeight)
//                        {
//                            m_pDataForGreen = new byte[(int)nWidth * (int)nHeight];
//                        }
//                        if (m_pDataForBlue == null || m_pDataForBlue.Length != (int)nWidth * (int)nHeight)
//                        {
//                            m_pDataForBlue = new byte[(int)nWidth * (int)nHeight];
//                        }
//                    }
//                    for (int nRow = 0; nRow < pFrameInfo.nHeight; nRow++)
//                    {
//                        for (int col = 0; col < pFrameInfo.nWidth; col++)
//                        {
//                            m_pDataForRed[nRow * nSupWidth + col] = m_pBufForSaveImage[nRow * pFrameInfo.nWidth * 3 + (3 * col)];
//                            if (ISRGB)
//                            {
//                                m_pDataForGreen[nRow * nSupWidth + col] = m_pBufForSaveImage[nRow * pFrameInfo.nWidth * 3 + (3 * col + 1)];
//                                m_pDataForBlue[nRow * nSupWidth + col] = m_pBufForSaveImage[nRow * pFrameInfo.nWidth * 3 + (3 * col + 2)];
//                            }
//                        }
//                    }
//                    IntPtr RedPtr = BytesToIntptr(m_pDataForRed);
//                    try
//                    {
//                        if (ISRGB)
//                        {
//                            IntPtr GreenPtr = BytesToIntptr(m_pDataForGreen);
//                            IntPtr BluePtr = BytesToIntptr(m_pDataForBlue);
//                            HOperatorSet.GenImage3(out Hobj, "byte", pFrameInfo.nWidth, pFrameInfo.nHeight, (new HTuple(RedPtr)), (new HTuple(GreenPtr)), (new HTuple(BluePtr)));
//                            Marshal.FreeHGlobal(GreenPtr);// ch 释放空间 || en: release space
//                            Marshal.FreeHGlobal(BluePtr);// ch 释放空间 || en: release space
//                        }
//                        else
//                        {
//                            HOperatorSet.GenImage1(out Hobj, "byte", pFrameInfo.nWidth, pFrameInfo.nHeight, new HTuple(RedPtr));
//                        }
//                        if (RotateTypeStr != "None")
//                        {
//                            HOperatorSet.MirrorImage(Hobj, out  Hobj, RotateTypeStr);
//                        }
//                        if (!RealTimeMode)
//                        {
//                            Vision.TriggerSetup(this.OKName, true.ToString());
//                            Watch.Stop();
//                            this.OnSwtr(Key, Hobj, this.RunID);
//                        }
//                    }
//                    catch (System.Exception ex)
//                    {
//                        this.LogErr(ex);
//                    }

//                    if (RealTimeMode)
//                    {
//                        try
//                        {
//                            if (ha != null)
//                            {
//                                ha.ListObjCler();
//                                ha.HTempobjectClear();
//                                //ha.GetResultOBj().ClearAllObj();
//                                ha.GetResultOBj().Massage = new HTuple();
//                                this.ImageDefinition(ha);
//                                Frame++;
//                                HOperatorSet.CountSeconds(out HTuple SecondsCurrent);
//                                fps = Frame / (SecondsCurrent - SecondsBegin);

//                                if (MaxNumbe!=0&&  MaxNumbe <= numberV)
//                                {
//                                    Key = "One";
//                                }
//                                ha.GetResultOBj().AddMeassge(Fps.ToString("0.##") + "fps");

//                                this.OnSwtr(Key, Hobj, this.RunID);
//                                this.Watch.Restart();
//                                numberV++;
//                            }
//                        }
//                        catch (Exception)
//                        { }
//                    }
//                    try
//                    {
//                        Marshal.FreeHGlobal(RedPtr);// ch 释放空间 || en: release space

//                    }
//                    catch (System.Exception ex)
//                    {
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                Vision.TriggerSetup(this.NGName, true.ToString());
//            }

//            return;
//        }

//        /// <summary>
//        /// 获得字节指针
//        /// </summary>
//        /// <param name="bytes">字节</param>
//        /// <returns>指针</returns>
//        public static IntPtr BytesToIntptr(byte[] bytes)
//        {
//            int size = bytes.Length;
//            IntPtr buffer = Marshal.AllocHGlobal(size);
//            Marshal.Copy(bytes, 0, buffer, size);
//            return buffer;
//        }
//        /// <summary>
//        /// 查询在线相机，保存到静态变量KeyCams，m_pDeviceList
//        /// </summary>
//        /// <returns></returns>
//        public static List<string> DeviceListAcq()
//        {
//            KeyCams = new Dictionary<string, MyCamera.MV_CC_DEVICE_INFO>();
//            ListDevice = new List<MyCamera.MV_CC_DEVICE_INFO>();
//            int nRet;
//            // ch:创建设备列表 || en: Create device list
//            System.GC.Collect();
//            nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE| MyCamera.MV_1394_DEVICE , ref m_pDeviceList);
//            if (0 != nRet)
//            {
//                MessageBox.Show("Enum Devices Fail");
//                return new List<string>();
//            }
//            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
//            {
//                try
//                {
//                    string kay = "";
//                    MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
//                    ListDevice.Add(device);
//                    if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
//                    {
//                        IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
//                        MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
//                        if (gigeInfo.chUserDefinedName != "")
//                        {
//                            kay = "GigE: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")";
//                        }
//                        else
//                        {
//                            kay = "GigE: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")";
//                        }
//                    }
//                    else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
//                    {
//                        IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stUsb3VInfo, 0);
//                        MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_USB3_DEVICE_INFO));
//                        if (usbInfo.chUserDefinedName != "")
//                        {
//                            kay = "USB: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")";
//                        }
//                        else
//                        {
//                            kay = "USB: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")";
//                        }
//                    }
//                    try
//                    {
//                        if (!KeyCams.ContainsKey(kay))
//                        {
//                            KeyCams.Add(kay, device);
//                        }
//                    }
//                    catch (Exception)
//                    {


//                    }
//                }
//                catch (Exception)
//                {

//                }



//            }
//            return KeyCams.Keys.ToList();
//        }
//        /// <summary>
//        /// 关闭所有相机
//        /// </summary>
//        public static void CloseAll()
//        {
//            System.GC.Collect();
//            int nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
//            if (0 != nRet)
//            {
//                return;
//            }
//            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
//            {
//                string kay = "";
//                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));

//                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
//                {
//                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
//                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
//                    if (gigeInfo.chUserDefinedName != "")
//                    {
//                        kay = "GigE: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")";
//                    }
//                    else
//                    {
//                        kay = "GigE: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")";
//                    }
//                }
//                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
//                {
//                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stUsb3VInfo, 0);
//                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_USB3_DEVICE_INFO));
//                    if (usbInfo.chUserDefinedName != "")
//                    {
//                        kay = "USB: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")";
//                    }
//                    else
//                    {
//                        kay = "USB: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")";
//                    }
//                }
//            }


//        }
//    }
//}

