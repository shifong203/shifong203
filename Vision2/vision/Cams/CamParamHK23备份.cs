    ����          ^Microsoft.Build.Tasks.Core, Version=15.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a   !Microsoft.Build.Tasks.SystemState   	fileStateSystem.Collections.Hashtable   	      System.Collections.Hashtable   
LoadFactorVersionComparerHashCodeProviderHashSizeKeysValues   System.Collections.IComparer$System.Collections.IHashCodeProvider�Q8?�   

�   	   	      �      zC:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\Facades\System.Runtime.Extensions.dll   iC:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\Microsoft.CSharp.dll   8G:\NOKIDA\ErosSocket\bin\Debug\Opc.Ua.ClientControls.dll	   qC:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\Facades\System.Threading.dll
   JG:\NOKIDA\ErosSocket\bin\Debug\Microsoft.Extensions.FileSystemGlobbing.dll   H..\packages\OpenCvSharp3-AnyCPU.4.0.0.20181129\lib\net40\OpenCvSharp.dll   $G:\NOKIDA\NokidaE\bin\Debug\NPOI.dll   *..\..\项目\803\Debug\Newtonsoft.Json.dll   2G:\NOKIDA\PLCEquipment\bin\Debug\TheSecsDriver.dll   G:\NOKIDA\DLL\ZenEAPCore.dll   ,G:\NOKIDA\ErosSocket\bin\Debug\System.IO.dll   7..\..\项目\803\Debug\System.Collections.Immutable.dll   nC:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\Microsoft.VisualBasic.dll   LC:\WINDOWS\assembly\GAC_MSIL\RCAPINet\1.0.0.0__0487594b04a8fcc0\RCAPINet.dll   dC:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Core.dll   C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Windows.Forms.DataVisualization.dll   fC:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Design.dll   <G:\NOKIDA\ErosSocket\bin\Debug\System.Runtime.Extensions.dll   1G:\NOKIDA\PLCEquipment\bin\Debug\PLCEquipment.dll   SG:\NOKIDA\ErosSocket\bin\Debug\Microsoft.AspNetCore.Hosting.Server.Abstractions.dll   gC:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Drawing.dll   vC:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\Facades\System.ComponentModel.dll   .G:\NOKIDA\ErosSocket\bin\Debug\TwinCAT.Ads.dll   0G:\NOKIDA\NokidaE\bin\Debug\NPOI.OpenXml4Net.dll   *G:\NOKIDA\NokidaE\bin\Debug\NPOI.OOXML.dll   dC:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Data.dll    OG:\NOKIDA\packages\OpenCvSharp3-AnyCPU.4.0.0.20181129\lib\net40\OpenCvSharp.dll!   IG:\NOKIDA\ErosSocket\bin\Debug\System.Runtime.CompilerServices.Unsafe.dll"   @C:\Program Files\IIS\Microsoft Web Deploy V3\Newtonsoft.Json.dll#   TG:\NOKIDA\ErosSocket\bin\Debug\System.Runtime.InteropServices.RuntimeInformation.dll$   ,G:\NOKIDA\PLCEquipment\bin\Debug\log4net.dll%   M..\packages\OpenCvSharp3-AnyCPU.4.0.0.20181129\lib\net40\OpenCvSharp.Blob.dll&   ]G:\NOKIDA\packages\OpenCvSharp3-AnyCPU.4.0.0.20181129\lib\net40\OpenCvSharp.UserInterface.dll'   1G:\NOKIDA\ErosSocket\bin\Debug\System.Buffers.dll(   7G:\NOKIDA\ErosSocket\bin\Debug\Opc.Ua.Configuration.dll)   #G:\NOKIDA\Stub\bin\Debug\Common.dll*   ?G:\NOKIDA\ErosSocket\bin\Debug\Microsoft.AspNetCore.Hosting.dll+   0..\packages\ZXing.Net.0.16.4\lib\net45\zxing.dll,   KG:\NOKIDA\ErosSocket\bin\Debug\Microsoft.Extensions.DependencyInjection.dll-   dC:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Xaml.dll.   pC:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\WindowsFormsIntegration.dll/   ?G:\NOKIDA\ErosSocket\bin\Debug\Microsoft.Extensions.Options.dll0   C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\Facades\System.Runtime.InteropServices.dll1   1G:\NOKIDA\StubAbstract\bin\Debug\StubAbstract.dll2   RG:\NOKIDA\ErosSocket\bin\Debug\Microsoft.Extensions.FileProviders.Abstractions.dll3   8G:\NOKIDA\ErosSocket\bin\Debug\System.IO.Compression.dll4   .G:\                  //GaedImage(out HObject hObject);
                        //hObject.Dispose();
                        return true;
                    }
                }

                continue;
            }
            return false;
        }

        public override void SetGain()
        {
          nRet=  m_pMyCamera.MV_CC_SetGain_NET( this.M_GainInt);
            m_pMyCamera.MV_CC_SetIntValue_NET("GainRaw",  this.M_GainInt);

        }

        public override void SetExposureTime(int exp=0 )
        {
            if (exp!=0)
            {
                nRet = m_pMyCamera.MV_CC_SetExposureTime_NET(exp);
                nRet = m_pMyCamera.MV_CC_SetFloatValue_NET("ExposureTimeAbs", exp);
            }
            else
            {
                nRet = m_pMyCamera.MV_CC_SetExposureTime_NET(this.ExposureTimeAbs);
                nRet = m_pMyCamera.MV_CC_SetFloatValue_NET("ExposureTimeAbs", this.ExposureTimeAbs);

            }

        }

        public override double GetExposureTimeAbs()
        {
            MyCamera.MVCC_FLOATVALUE pstValue = new MyCamera.MVCC_FLOATVALUE();
            nRet = m_pMyCamera.MV_CC_GetFloatValue_NET("ExposureTimeAbs", ref  pstValue);
            if (nRet==0)
            {
                return pstValue.fCurValue;
            }
            return pstValue.fCurValue;
        }

        public override double GetGain()
        {
            MyCamera.MVCC_INTVALUE pstValue = new MyCamera.MVCC_INTVALUE();
            nRet = m_pMyCamera.MV_CC_GetIntValue_NET("GainRaw", ref pstValue);
            return  (double)pstValue.nCurValue;
        }

  
        public override void GetFramegradderParam()
        {
            MyCamera.MV_NETTRANS_INFO pstValue = new MyCamera.MV_NETTRANS_INFO();
            m_pMyCamera.MV_GIGE_GetNetTransInfo_NET(ref pstValue);
     
        }

        public override HTuple GetFramegrabberParam(string parmName)
        {
            MyCamera.MVCC_STRINGVALUE set;
            set = new MyCamera.MVCC_STRINGVALUE();
            nRet=   m_pMyCamera. MV_CC_GetStringValue_NET(parmName, ref  set);
            if (nRet!=0)
            {
                return null;
            }
            return set.chCurValue;
        }
        public override string GetIP()
        {
            try
            {
                IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                // 显示IP
                UInt32 nIp1 = (gigeInfo.nCurrentIp & 0xFF000000) >> 24;
                UInt32 nIp2 = (gigeInfo.nCurrentIp & 0x00FF0000) >> 16;
                UInt32 nIp3 = (gigeInfo.nCurrentIp & 0x0000FF00) >> 8;
                UInt32 nIp4 = (gigeInfo.nCurrentIp & 0x000000FF);
                this.camS_Information.IP = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();
            }
            catch (Exception)
            {
            }
            return this.camS_Information.IP;

        }

        public override void SetFrameGradderIP(string ip)
        {
            try
            {
                nRet = m_pMyCamera.MV_CC_SetIntValueEx_NET("GevCurrentIPConfiguration", 1);
                // ch:IP转换 | en:IP conversion
                IPAddress clsIpAddr;
                if (false == IPAddress.TryParse(ip, out clsIpAddr))
                {
                    ShowErrorMsg("Please enter correct IP", 0);
                    return;
                }
                long nIp = IPAddress.NetworkToHostOrder(clsIpAddr.Address);
                // ch:掩码转换 | en:Mask conversion
                IPAddress clsSubMask;
                if (false == IPAddress.TryParse("255.255.255.0", out clsSubMask))
                {
                    ShowErrorMsg("Please enter correct IP", 0);
                    return;
                }
                long nSubMask = IPAddress.NetworkToHostOrder(clsSubMask.Address);

                // ch:网关转换 | en:Gateway conversion
                IPAddress clsDefaultWay;
                if (false == IPAddress.TryParse("192.168.0.1", out clsDefaultWay))
                {
                    ShowErrorMsg("Please enter correct IP", 0);
                    return;
                }
                long nDefaultWay = IPAddress.NetworkToHostOrder(clsDefaultWay.Address);
                // ch:打开设备 | en:Open device
                if (null == m_pMyCamera)
                {
                    m_pMyCamera = new MyCamera();
                }
                nRet = m_pMyCamera.MV_CC_CreateDevice_NET(ref device);
                if (MyCamera.MV_OK != nRet)
                {
                    return;
                }
                nRet = m_pMyCamera.MV_GIGE_ForceIpEx_NET((uint)(nIp >> 32), (uint)(nSubMask >> 32), (uint)(nDefaultWay >> 32));
                GC.Collect();
                if (nRet != MyCamera.MV_OK)
                {
                    ShowErrorMsg("设置失败", nRet);
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SetCamIP(string ip,string id)
        {
            MyCamera m_pMyCamera = new MyCamera();
            int  nRet = m_pMyCamera.MV_CC_SetIntValueEx_NET("GevCurrentIPConfiguration", 1);
            // ch:IP转换 | en:IP conversion
            IPAddress clsIpAddr;
            if (false == IPAddress.TryParse(ip, out clsIpAddr))
            {
                MessageBox.Show("IP地址错误");
                return false;
            }
            long nIp = IPAddress.NetworkToHostOrder(clsIpAddr.Address);
            // ch:掩码转换 | en:Mask conversion
            IPAddress clsSubMask;
            if (false == IPAddress.TryParse("255.255.255.0", out clsSubMask))
            {
                MessageBox.Show("网关地址错误");
                return false;
            }
            long nSubMask = IPAddress.NetworkToHostOrder(clsSubMask.Address);
            // ch:网关转换 | en:Gateway conversion
            IPAddress clsDefaultWay;
            if (false == IPAddress.TryParse("192.168.0.1", out clsDefaultWay))
            {
                MessageBox.Show("网关地址错误");
                return false;
            }
            long nDefaultWay = IPAddress.NetworkToHostOrder(clsDefaultWay.Address);
            MyCamera.MV_CC_DEVICE_INFO device;
            // ch:打开设备 | en:Open device
            for (int i = 0; i < ListDevice.Count; i++)
            {
                device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));//获取设备
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                    //m_SerialNumber = gigeInfo.chSerialNumber;//获取序列号
                    if (id.Equals(gigeInfo.chSerialNumber))
                    {
                        nRet = m_pMyCamera.MV_CC_CreateDevice_NET(ref device);
                        if (MyCamera.MV_OK != nRet)
                        {
                            return false;
                        }
                        nRet = m_pMyCamera.MV_GIGE_ForceIpEx_NET((uint)(nIp >> 32), (uint)(nSubMask >> 32), (uint)(nDefaultWay >> 32));
                        GC.Collect();
                        if (nRet != MyCamera.MV_OK)
                        {
                            ShowErrorMsg("设置失败", nRet);
                        }
                        m_pMyCamera.MV_CC_CloseDevice_NET();
                        m_pMyCamera.MV_CC_DestroyDevice_NET();
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 登录文本信息
        /// </summary>
        /// <param name="csMessage">错误信息</param>
        /// <param name="nErrorNum">错误编号</param>
        private static void ShowErrorMsg(string csMessage, int nErrorNum)
        {
            string errorMsg;
            if (nErrorNum == 0)
            {
                return;
                //errorMsg = this.Name +csMessage;
            }
            else
            {
                errorMsg = csMessage + ": Error =" + String.Format("{0:X}", nErrorNum);
            }

            switch (nErrorNum)
            {
                case MyCamera.MV_E_HANDLE: errorMsg += "错误或无效句柄/Error or invalid handle "; break;
                case MyCamera.MV_E_SUPPORT: errorMsg += " 不支持功能/Not supported function "; break;
                case MyCamera.MV_E_BUFOVER: errorMsg += " 缓存已满/Cache is full "; break;
                case MyCamera.MV_E_CALLORDER: errorMsg += "函数调用顺序错误/ Function calling order error "; break;
                case MyCamera.MV_E_PARAMETER: errorMsg += " 不正确的参数/Incorrect parameter "; break;
                case MyCamera.MV_E_RESOURCE: errorMsg += " 应用资源失败/Applying resource failed "; break;
                case MyCamera.MV_E_NODATA: errorMsg += " 无数据/No data "; break;
                case MyCamera.MV_E_PRECONDITION: errorMsg += "前置条件错误，或运行环境改变/ Precondition error, or running environment changed "; break;
                case MyCamera.MV_E_VERSION: errorMsg += " 版本不匹配/Version mismatches "; break;
                case MyCamera.MV_E_NOENOUGH_BUF: errorMsg += "内存空间不足/ Insufficient memory "; break;
                case MyCamera.MV_E_UNKNOW: errorMsg += "未知的错误/ Unknown error "; break;
                case MyCamera.MV_E_GC_GENERIC: errorMsg += "一般的错误/ General error "; break;
                case MyCamera.MV_E_GC_ACCESS: errorMsg += "节点访问条件错误/ Node accessing condition error "; break;
                case MyCamera.MV_E_ACCESS_DENIED: errorMsg += "没有权限"; break;
                case MyCamera.MV_E_BUSY: errorMsg += "设备忙，或网络断开/ Device is busy, or network disconnected "; break;
                case MyCamera.MV_E_NETER: errorMsg += "网络错误/ Network error "; break;
            }
          ErosProjcetDLL.Project.AlarmText.LogWarning("相机错误", errorMsg);
            //MessageBox.Show(errorMsg, "PROMPT");
        }
        public static void CloseCam(MyCamera.MV_CC_DEVICE_INFO mV_CC)
        {
            try
            {
                MyCamera m_pMyCa = new MyCamera();
                int temp = m_pMyCa.MV_CC_CreateDevice_NET(ref mV_CC);
                temp = m_pMyCa.MV_CC_StopGrabbing_NET();
                temp = m_pMyCa.MV_CC_CloseDevice_NET();
                Thread.Sleep(50);
                temp = m_pMyCa.MV_CC_DestroyDevice_NET();
                //m_pMyCa.d
            }
            catch (Exception)
            {
            }

        }
        public override void OffCam()
        {
           try
            {
               int temp = m_pMyCamera.MV_CC_CreateDevice_NET(ref device);
                if (m_bGrabbing)
                {
                    m_bGrabbing = false;
                    // ch:停止抓图 || en:Stop grab image
                }
                nRet = m_pMyCamera.MV_CC_StopGrabbing_NET();
                nRet = m_pMyCamera.MV_CC_CloseDevice_NET();
                Thread.Sleep(50);
                nRet = m_pMyCamera.MV_CC_DestroyDevice_NET();
                ShowErrorMsg("", nRet);
                Thread.Sleep(150);

            }
            catch (Exception)
            {
            }
            base.OffCam();
        }
        /// <summary>
        /// 连接相机
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>成功true</returns>
        public bool ConnectCamera(string id)
        {
            this.m_IDStr = id;
          return  LiakCam();
        }
        //成功返回0失败返回-1
        //调用函数时可以传入需要改变的目标IP，如过没有传入则将相机IP设置为其所连接的网卡地址+1或-1
        public int ChangeIP(MyCamera.MV_CC_DEVICE_INFO deviceInfo,string IP = "")
        {
            try
            {
                //获取相机相关信息，例如相机所连接网卡的网址
                IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(deviceInfo.SpecialInfo.stGigEInfo, 0);
                MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                IPAddress cameraIPAddress;
                string tempStr = "";
                if (IP.Trim().Equals("") || !(IPAddress.TryParse(IP, out cameraIPAddress)))
                {
                    //当前网卡的IP地址
                    UInt32 nNetIp1 = (gigeInfo.nNetExport & 0xFF000000) >> 24;
                    UInt32 nNetIp2 = (gigeInfo.nNetExport & 0x00FF0000) >> 16;
                    UInt32 nNetIp3 = (gigeInfo.nNetExport & 0x0000FF00) >> 8;
                    UInt32 nNetIp4 = (gigeInfo.nNetExport & 0x000000FF);
                    //根据网卡IP设定相机IP，如果网卡ip第四位小于252，则相机ip第四位+1，否则相机IP第四位-1
                    UInt32 cameraIp1 = nNetIp1;
                    UInt32 cameraIp2 = nNetIp2;
                    UInt32 cameraIp3 = nNetIp3;
                    UInt32 cameraIp4 = nNetIp4;
                    if (nNetIp4 < 252)
                    {
                        cameraIp4++;
                    }
                    else
                    {
                        cameraIp4--;
                    }
                    tempStr = cameraIp1 + "." + cameraIp2 + "." + cameraIp3 + "." + cameraIp4;
                }
                else
                {
                    tempStr = IP;
                }
                IPAddress.TryParse(tempStr, out cameraIPAddress);
                long cameraIP = IPAddress.NetworkToHostOrder(cameraIPAddress.Address);
                //设置相机掩码
                uint maskIp1 = (gigeInfo.nCurrentSubNetMask & 0xFF000000) >> 24;
                uint maskIp2 = (gigeInfo.nCurrentSubNetMask & 0x00FF0000) >> 16;
                uint maskIp3 = (gigeInfo.nCurrentSubNetMask & 0x0000FF00) >> 8;
                uint maskIp4 = (gigeInfo.nCurrentSubNetMask & 0x000000FF);
                IPAddress subMaskAddress;
                tempStr = maskIp1 + "." + maskIp2 + "." + maskIp3 + "." + maskIp4;
                IPAddress.TryParse(tempStr, out subMaskAddress);
                long maskIP = IPAddress.NetworkToHostOrder(subMaskAddress.Address);
                //设置网关
                uint gateIp1 = (gigeInfo.nDefultGateWay & 0xFF000000) >> 24;
                uint gateIp2 = (gigeInfo.nDefultGateWay & 0x00FF0000) >> 16;
                uint gateIp3 = (gigeInfo.nDefultGateWay & 0x0000FF00) >> 8;
                uint gateIp4 = (gigeInfo.nDefultGateWay & 0x000000FF);
                IPAddress gateAddress;
                tempStr = gateIp1 + "." + gateIp2 + "." + gateIp3 + "." + gateIp4;
                IPAddress.TryParse(tempStr, out gateAddress);
                long gateIP = IPAddress.NetworkToHostOrder(gateAddress.Address);

                int temp = m_pMyCamera.MV_GIGE_ForceIpEx_NET((UInt32)(cameraIP >> 32), (UInt32)(maskIP >> 32), (UInt32)(gateIP >> 32));//执行更改相机IP的命令
                if (temp == 0)
                    //强制IP成功
                    return 0;
                //强制IP失败
                return -1;
            }
            catch
            {
                return -1;
            }
        }
        HalconRunFile.RunProgramFile.HalconRun ha;

        HTuple SecondsBegin;
        /// <summary>
        /// 实时釆图
        /// </summary>
        /// <param name="halcon"></param>
        public override void ThreadSatring(HalconRunFile.RunProgramFile.HalconRun halcon)
        {
            Frame  = 0;
            fps = 0;
            int nRet;
            //nRet = m_pMyCamera.MV_CC_SetStringValue_NET("TriggerSource", "Software");
            nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerSource", 1);
            realTimeMode = true;
            nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerMode", 0);
            ha= halcon;
            m_pMyCamera.MV_CC_StopGrabbing_NET();
            //开始采集
            nRet = m_pMyCamera.MV_CC_StartGrabbing_NET();
            HOperatorSet.CountSeconds(out  SecondsBegin);
            //if (MyCamera.MV_OK != nRet)
            //{
            //    MessageBox.Show("开始取流失败！");
            //    return;
            //}
            //标志位置位true
            m_bGrabbing = true;

        }

        /// <summary>
        /// 采集单张图像
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        protected override bool GaedImage(out HObject image)
        {
            if (!m_bCamIsCon)
            {
                this.OnLinkSt(false);
            }
            image = new HObject();
            image.GenEmptyObj();
            isGradDone = false;
            realTimeMode = false;
            try
            {
                m_pMyCamera.MV_CC_StopGrabbing_NET();
                nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerMode",1);
                nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerSource", 7);
                //nRet = m_pMyCamera.MV_CC_SetStringValue_NET("TriggerSource", "Software");
                if (nRet!=0)
                {
                    nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerSource", 0);
                    //nRet = m_pMyCamera.MV_CC_SetStringValue_NET("TriggerSource", "Counter");
                }
    
                
                // ch:开启抓图 | en:start grab
                nRet = m_pMyCamera.MV_CC_StartGrabbing_NET();
                m_bGrabbing = true;
                nRet = m_pMyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
                if (MyCamera.MV_OK != nRet)
                {
                    this.OnLinkSt(false);
                    return false;
                }
             // System.Threading.Thread.Sleep(100);
                //if (!CallWithTimeout(new Action(run), 5000))
                //{
                //    //this.Message.Append("执行超时!");
                //    return false;
                //}
                // int DSS = 0;
                //     while (!isGradDone)
                //     {
                //         System.Threading.Thread.Sleep(1);
                //         DSS++;
                //         if (DSS>=1000)
                //         {
                //             return false;
                //         }
                //     }
                //image=  Hobj;
                return true;
            }
            catch (Exception)
            {

             
            }
            return false;
        }

        public override void SetFramegrabberParam(string name = null, HTuple value = null)
        {
            this.SetGain();
            this.SetExposureTime();
            //base.SetFramegrabberParam(name, value);
        }

        public override bool Run(HalconRun halcon)
        {
            Watch.Restart();
            StaticCon.SetLinkAddressValue(this.FlashLampName, true);
            Thread.Sleep(CamTime);
            bool iscong = GaedImage(out HObject hObject);
            ImageDefinition(halcon);
            return iscong;
    
        }
        bool isGradDone;
        /**********************************************************************************************************/
        private void GrabImage(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            try
            {
                //Watch.ElapsedMilliseconds.ToString();
                Hobj.Dispose();
                if (pData != null)
                {
                    uint nWidth = pFrameInfo.nWidth;

                    uint nHeight = pFrameInfo.nHeight;

                    Marshal.Copy(pData, m_pBufForSaveImage, 0, (int)nWidth * (int)nHeight * 3);
                    UInt32 nSupWidth = (pFrameInfo.nWidth + (UInt32)3) & 0xfffffffc;
                    if (m_pDataForRed == null || m_pDataForRed.Length != (int)nWidth * (int)nHeight)
                    {
                        m_pDataForRed = new byte[(int)nWidth * (int)nHeight];
                    }
                    //if (m_pDataForGreen == null || m_pDataForGreen.Length != (int)nWidth * (int)nHeight)
                    //{
                    //    m_pDataForGreen = new byte[(int)nWidth * (int)nHeight];
                    //}
                    //if (m_pDataForBlue == null || m_pDataForBlue.Length != (int)nWidth * (int)nHeight)
                    //{
                    //    m_pDataForBlue = new byte[(int)nWidth * (int)nHeight];
                    //}
                    for (int nRow = 0; nRow < pFrameInfo.nHeight; nRow++)
                    {
                        for (int col = 0; col < pFrameInfo.nWidth; col++)
                        {
                            m_pDataForRed[nRow * nSupWidth + col] = m_pBufForSaveImage[nRow * pFrameInfo.nWidth * 3 + (3 * col)];
                        //    m_pDataForGreen[nRow * nSupWidth + col] = m_pBufForSaveImage[nRow * pFrameInfo.nWidth * 3 + (3 * col + 1)];
                        //    m_pDataForBlue[nRow * nSupWidth + col] = m_pBufForSaveImage[nRow * pFrameInfo.nWidth * 3 + (3 * col + 2)];
                        }
                    }

                    IntPtr RedPtr = BytesToIntptr(m_pDataForRed);
                    //IntPtr GreenPtr = BytesToIntptr(m_pDataForGreen);
                    //IntPtr BluePtr = BytesToIntptr(m_pDataForBlue);
                    try
                    {
                        HOperatorSet.GenImage1(out Hobj, "byte", pFrameInfo.nWidth, pFrameInfo.nHeight, new HTuple(RedPtr));
                        if (!realTimeMode)
                        {
                            StaticCon.SetLinkAddressValue(this.FlashLampName, false);
                            Vision.TriggerSetup(this.OKName, true.ToString());
                            Watch.Stop();
                            this.OnSwtr(Key, Hobj);
                        }
                        //    HOperatorSet.GenImage3(out Hobj, "byte", pFrameInfo.nWidth, pFrameInfo.nHeight, (new HTuple(RedPtr)), (new HTuple(GreenPtr)), (new HTuple(BluePtr)));
                    }
                    catch (System.Exception ex)
                    {
                        this.LogErr(ex);
                    }
                    if (realTimeMode)
                    {
                        try
                        {
                            if (ha != null)
                            {
                                ha.ListObjCler();
                                ha.Image(Hobj.Clone());
                                ha.Message = new HTuple();
                                //HOperatorSet.DispObj(ha.Image, ha.hWindowHalconID);
                                this.ImageDefinition(ha);
                                Frame++;
                                HOperatorSet.CountSeconds(out HTuple SecondsCurrent);
                                fps = Frame / (SecondsCurrent - SecondsBegin);
                                ha.Message.Append(Fps.ToString("0.##") + "fps");
                                ha.GetShowObj();
                            }
                        }
                        catch (Exception)
                        { }
                    }
                    try
                    {
                        Marshal.FreeHGlobal(RedPtr);// ch 释放空间 || en: release space
                        //Marshal.FreeHGlobal(GreenPtr);// ch 释放空间 || en: release space
                        //Marshal.FreeHGlobal(BluePtr);// ch 释放空间 || en: release space
                    }
                    catch (System.Exception ex)
                    {
                    }
                }

            }
            catch (Exception)
            {
                Vision.TriggerSetup(this.NGName, true.ToString());
            }
            isGradDone = true;
            return;
        }
  
        /// <summary>
        /// 获得字节指针
        /// </summary>
        /// <param name="bytes">字节</param>
        /// <returns>指针</returns>
        public static IntPtr BytesToIntptr(byte[] bytes)
        {
            int size = bytes.Length;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, buffer, size);
            return buffer;
        }
        /// <summary>
        /// 查询在线相机，保存到静态变量KeyCams，m_pDeviceList
        /// </summary>
        /// <returns></returns>
        public static List<string> DeviceListAcq()
        {
            KeyCams = new Dictionary<string, MyCamera.MV_CC_DEVICE_INFO>();
            ListDevice = new List<MyCamera.MV_CC_DEVICE_INFO>();
            int nRet;
            // ch:创建设备列表 || en: Create device list
            System.GC.Collect();
            nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            if (0 != nRet)
            {
                MessageBox.Show("Enum Devices Fail");
                return new List<string>();
            }
            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
            {
                try
                {
                    string kay = "";
                    MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                    ListDevice.Add(device);
                    if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                    {
                        IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                        MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                        if (gigeInfo.chUserDefinedName != "")
                        {
                            kay = "GigE: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")";
                        }
                        else
                        {
                            kay = "GigE: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")";
                        }
                    }
                    else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                    {
                        IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stUsb3VInfo, 0);
                        MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                        if (usbInfo.chUserDefinedName != "")
                        {
                            kay = "USB: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")";
                        }
                        else
                        {
                            kay = "USB: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")";
                        }
                    }
                    try
                    {
                        if (!KeyCams.ContainsKey(kay))
                        {
                            KeyCams.Add(kay, device);
                        }
                    }
                    catch (Exception)
                    {


                    }
                }
                catch (Exception)
                {

                }
          
            

            }
            return KeyCams.Keys.ToList();
        }
        /// <summary>
        /// 关闭所有相机
        /// </summary>
        public static void CloseAll()
        {
            System.GC.Collect();
             int   nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            if (0 != nRet)
            {
                return;
            }
            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
            {
                string kay = "";
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
       
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                    if (gigeInfo.chUserDefinedName != "")
                    {
                        kay = "GigE: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")";
                    }
                    else
                    {
                        kay = "GigE: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")";
                    }
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stUsb3VInfo, 0);
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    if (usbInfo.chUserDefinedName != "")
                    {
                        kay = "USB: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")";
                    }
                    else
                    {
                        kay = "USB: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")";
                    }
                }
            }


        }

    }
}
