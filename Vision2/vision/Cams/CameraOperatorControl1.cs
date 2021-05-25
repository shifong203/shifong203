using MvCamCtrl.NET;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NokidaE.vision.Cams
{
    public partial class CameraOperatorControl1 : UserControl
    {
        public CameraOperatorControl1()
        {
            InitializeComponent();
            m_pDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            m_pOperator = new CameraOperator();
            m_bGrabbing = false;
            DeviceListAcq();
        }

        private MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList;
        private CameraOperator m_pOperator;
        private bool m_bGrabbing;

        private UInt32 m_nBufSizeForDriver = 3072 * 2048 * 3;
        private byte[] m_pBufForDriver = new byte[3072 * 2048 * 3];            // 用于从驱动获取图像的缓存

        private UInt32 m_nBufSizeForSaveImage = 3072 * 2048 * 3 * 3 + 2048;
        private byte[] m_pBufForSaveImage = new byte[3072 * 2048 * 3 * 3 + 2048];         // 用于保存图像的缓存

        private void bnEnum_Click(object sender, EventArgs e)
        {
            DeviceListAcq();
        }

        private void DeviceListAcq()
        {
            int nRet;
            /*创建设备列表*/
            System.GC.Collect();
            cbDeviceList.Items.Clear();
            nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            if (0 != nRet)
            {
                MessageBox.Show("枚举设备失败!");
                return;
            }

            //在窗体列表中显示设备名
            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                    if (gigeInfo.chUserDefinedName != "")
                    {
                        cbDeviceList.Items.Add("GigE: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")");
                    }
                    else
                    {
                        cbDeviceList.Items.Add("GigE: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")");
                    }
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stUsb3VInfo, 0);
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    if (usbInfo.chUserDefinedName != "")
                    {
                        cbDeviceList.Items.Add("USB: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")");
                    }
                    else
                    {
                        cbDeviceList.Items.Add("USB: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")");
                    }
                }
            }

            //选择第一项
            if (m_pDeviceList.nDeviceNum != 0)
            {
                cbDeviceList.SelectedIndex = 0;
            }
        }

        private void bnOpen_Click(object sender, EventArgs e)
        {
            if (m_pDeviceList.nDeviceNum == 0 || cbDeviceList.SelectedIndex == -1)
            {
                MessageBox.Show("无设备，请选择");
                return;
            }
            int nRet = -1;

            //获取选择的设备信息
            MyCamera.MV_CC_DEVICE_INFO device =
                (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[cbDeviceList.SelectedIndex],
                                                              typeof(MyCamera.MV_CC_DEVICE_INFO));

            //打开设备
            nRet = m_pOperator.Open(ref device);
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("设备打开失败!");
                return;
            }

            //设置采集连续模式
            m_pOperator.SetEnumValue("AcquisitionMode", 2);// 工作在连续模式
            m_pOperator.SetEnumValue("TriggerMode", 0);    // 连续模式
                                                           //开始采集
            nRet = m_pOperator.StartGrabbing();

            bnGetParam_Click(null, null);//获取参数

            //控件操作
            SetCtrlWhenOpen();
        }

        private void SetCtrlWhenOpen()
        {
            bnOpen.Enabled = false;

            bnClose.Enabled = true;
            bnStartGrab.Enabled = true;
            cbSoftTrigger.Enabled = false;
            bnTriggerExec.Enabled = true;

            tbExposure.Enabled = true;
            tbGain.Enabled = true;
            tbFrameRate.Enabled = true;
            bnGetParam.Enabled = true;
            bnSetParam.Enabled = true;
        }

        private void SetCtrlWhenClose()
        {
            bnOpen.Enabled = true;

            bnStartGrab.Enabled = false;

            cbSoftTrigger.Enabled = false;
            bnTriggerExec.Enabled = false;

            bnSaveBmp.Enabled = false;
            bnSaveJpg.Enabled = false;
            tbExposure.Enabled = false;
            tbGain.Enabled = false;
            tbFrameRate.Enabled = false;
            bnGetParam.Enabled = false;
            bnSetParam.Enabled = false;
        }

        private void bnClose_Click(object sender, EventArgs e)
        {
            //关闭设备
            m_pOperator.Close();

            //控件操作
            SetCtrlWhenClose();

            //取流标志位清零
            m_bGrabbing = false;
        }

        private void bnContinuesMode_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void bnStartGrab_Click(object sender, EventArgs e)
        {
            if (bnStartGrab.Text == "实时采图")
            {
                m_pOperator.SetEnumValue("TriggerMode", 0);
                bnTriggerExec.Enabled = false;
                bnStartGrab.Text = "停止实时";

                int nRet;
                //开始采集
                nRet = m_pOperator.StartGrabbing();
                //if (MyCamera.MV_OK != nRet)
                //{
                //    MessageBox.Show("开始取流失败！");
                //    return;
                //}
                //标志位置位true
                m_bGrabbing = true;
                nRet = m_pOperator.Display(pictureBox1.Handle);
                //显示
                //nRet = m_pOperator.Display(Vision.Instance.GetRunNameVision().GetWindowHandle());
                if (MyCamera.MV_OK != nRet)
                {
                    MessageBox.Show("显示失败！");
                }
            }
            else
            {
                bnStartGrab.Text = "实时采图";
                int nRet = -1;
                //停止采集
                nRet = m_pOperator.StopGrabbing();

                if (nRet != CameraOperator.CO_OK)
                {
                    MessageBox.Show("停止取流失败！");
                }
                //打开触发模式
                m_pOperator.SetEnumValue("TriggerMode", 1);
                //触发源选择:0 - Line0;
                //           1 - Line1;
                //           2 - Line2;
                //           3 - Line3;
                //           4 - Counter;
                //           7 - Software;
                if (cbSoftTrigger.Checked)
                {
                    m_pOperator.SetEnumValue("TriggerSource", 7);
                }
                else
                {
                    m_pOperator.SetEnumValue("TriggerSource", 0);
                }
                cbSoftTrigger.Enabled = true;
                if (m_bGrabbing)
                {
                    bnTriggerExec.Enabled = true;
                }
            }
        }

        private void SetCtrlWhenStartGrab()
        {
            bnStartGrab.Enabled = false;
            if (cbSoftTrigger.Checked)
            {
                bnTriggerExec.Enabled = true;
            }

            bnSaveBmp.Enabled = true;
            bnSaveJpg.Enabled = true;
        }

        private void bnGetParam_Click(object sender, EventArgs e)
        {
            //float fExposure = 0;
            //m_pOperator.GetFloatValue("ExposureTime", ref fExposure);
            //tbExposure.Text = fExposure.ToString("F1");

            //float fGain = 0;
            //m_pOperator.GetFloatValue("Gain", ref fGain);
            //tbGain.Text = fGain.ToString("F1");

            //float fFrameRate = 0;
            //m_pOperator.GetFloatValue("ResultingFrameRate", ref fFrameRate);
            //tbFrameRate.Text = fFrameRate.ToString("F1");
            GetDeviceParam("");
        }

        private void bnSetParam_Click(object sender, EventArgs e)
        {
            int nRet;
            m_pOperator.SetEnumValue("ExposureAuto", 0);

            try
            {
                float.Parse(tbExposure.Text);
                float.Parse(tbGain.Text);
                float.Parse(tbFrameRate.Text);
            }
            catch
            {
                MessageBox.Show("请输入正确类型!");
                return;
            }

            nRet = m_pOperator.SetFloatValue("ExposureTime", float.Parse(tbExposure.Text));
            if (nRet != CameraOperator.CO_OK)
            {
                MessageBox.Show("设置曝光时间失败！");
            }

            m_pOperator.SetEnumValue("GainAuto", 0);
            nRet = m_pOperator.SetFloatValue("Gain", float.Parse(tbGain.Text));
            if (nRet != CameraOperator.CO_OK)
            {
                MessageBox.Show("设置增益失败！");
            }

            nRet = m_pOperator.SetFloatValue("AcquisitionFrameRate", float.Parse(tbFrameRate.Text));
            if (nRet != CameraOperator.CO_OK)
            {
                MessageBox.Show("设置帧率失败！");
            }

            if (cbLineSel.SelectedIndex == -1)
            {
                MessageBox.Show("请选择输出！");
                return;
            }

            String strValue = cbLineSel.SelectedItem.ToString().Substring(12);
            UInt32 nValue = Convert.ToUInt32(strValue);
            nRet = m_pOperator.SetEnumValue("LineSelector", nValue);
            if (CameraOperator.CO_OK != nRet)
            {
                MessageBox.Show("设置失败！");
                return;
            }
            MessageBox.Show("设置成功！");

            if (cbLineMode.SelectedIndex == -1)
            {
                MessageBox.Show("请选择输出！");
                return;
            }

            strValue = cbLineMode.SelectedItem.ToString().Substring(8);
            nValue = Convert.ToUInt32(strValue);
            nRet = m_pOperator.SetEnumValue("LineMode", nValue);
            if (CameraOperator.CO_OK != nRet)
            {
                MessageBox.Show("设置失败！");
                return;
            }
            MessageBox.Show("设置成功！");
        }

        private void bnSaveBmp_Click(object sender, EventArgs e)
        {
            int nRet;
            UInt32 nPayloadSize = 0;
            nRet = m_pOperator.GetIntValue("PayloadSize", ref nPayloadSize);
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("Get PayloadSize failed");
                return;
            }
            if (nPayloadSize + 2048 > m_nBufSizeForDriver)
            {
                m_nBufSizeForDriver = nPayloadSize + 2048;
                m_pBufForDriver = new byte[m_nBufSizeForDriver];

                // 同时对保存图像的缓存做大小判断处理
                // BMP图片大小：width * height * 3 + 2048(预留BMP头大小)
                m_nBufSizeForSaveImage = m_nBufSizeForDriver * 3 + 2048;
                m_pBufForSaveImage = new byte[m_nBufSizeForSaveImage];
            }

            IntPtr pData = Marshal.UnsafeAddrOfPinnedArrayElement(m_pBufForDriver, 0);
            UInt32 nDataLen = 0;
            MyCamera.MV_FRAME_OUT_INFO_EX stFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();

            //超时获取一帧，超时时间为1秒
            nRet = m_pOperator.GetOneFrameTimeout(pData, ref nDataLen, m_nBufSizeForDriver, ref stFrameInfo, 1000);
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("无数据！");
                return;
            }

            /************************Mono8 转 Bitmap*******************************
            Bitmap bmp = new Bitmap(stFrameInfo.nWidth, stFrameInfo.nHeight, stFrameInfo.nWidth * 1, PixelFormat.Format8bppIndexed, pData);

            ColorPalette cp = bmp.Palette;
            // init palette
            for (int i = 0; i < 256; i++)
            {
                cp.Entries[i] = Color.FromArgb(i, i, i);
            }
            // set palette back
            bmp.Palette = cp;

            bmp.Save("D:\\test.bmp", ImageFormat.Bmp);

            *********************RGB8 转 Bitmap**************************
            for (int i = 0; i < stFrameInfo.nHeight; i++)
            {
                for (int j = 0; j < stFrameInfo.nWidth; j++)
                {
                    byte chRed = m_buffer[i * stFrameInfo.nWidth * 3 + j * 3];
                    m_buffer[i * stFrameInfo.nWidth * 3 + j * 3] = m_buffer[i * stFrameInfo.nWidth * 3 + j * 3 + 2];
                    m_buffer[i * stFrameInfo.nWidth * 3 + j * 3 + 2] = chRed;
                }
            }
            Bitmap bmp = new Bitmap(stFrameInfo.nWidth, stFrameInfo.nHeight, stFrameInfo.nWidth * 3, PixelFormat.Format24bppRgb, pData);
            bmp.Save("D:\\test.bmp", ImageFormat.Bmp);

            ************************************************************************/

            IntPtr pImage = Marshal.UnsafeAddrOfPinnedArrayElement(m_pBufForSaveImage, 0);
            MyCamera.MV_SAVE_IMAGE_PARAM_EX stSaveParam = new MyCamera.MV_SAVE_IMAGE_PARAM_EX();
            stSaveParam.enImageType = MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Bmp;
            stSaveParam.enPixelType = stFrameInfo.enPixelType;
            stSaveParam.pData = pData;
            stSaveParam.nDataLen = stFrameInfo.nFrameLen;
            stSaveParam.nHeight = stFrameInfo.nHeight;
            stSaveParam.nWidth = stFrameInfo.nWidth;
            stSaveParam.pImageBuffer = pImage;
            stSaveParam.nBufferSize = m_nBufSizeForSaveImage;
            stSaveParam.nJpgQuality = 80;
            nRet = m_pOperator.SaveImage(ref stSaveParam);
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("保存失败!");
                return;
            }

            FileStream file = new FileStream("image.bmp", FileMode.Create, FileAccess.Write);
            file.Write(m_pBufForSaveImage, 0, (int)stSaveParam.nImageLen);
            file.Close();

            MessageBox.Show("保存成功!");
        }

        private void bnSaveJpg_Click(object sender, EventArgs e)
        {
            int nRet;
            UInt32 nPayloadSize = 0;
            nRet = m_pOperator.GetIntValue("PayloadSize", ref nPayloadSize);
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("Get PayloadSize failed");
                return;
            }
            if (nPayloadSize + 2048 > m_nBufSizeForDriver)
            {
                m_nBufSizeForDriver = nPayloadSize + 2048;
                m_pBufForDriver = new byte[m_nBufSizeForDriver];

                // 同时对保存图像的缓存做大小判断处理
                // BMP图片大小：width * height * 3 + 2048(预留BMP头大小)
                m_nBufSizeForSaveImage = m_nBufSizeForDriver * 3 + 2048;
                m_pBufForSaveImage = new byte[m_nBufSizeForSaveImage];
            }

            IntPtr pData = Marshal.UnsafeAddrOfPinnedArrayElement(m_pBufForDriver, 0);
            UInt32 nDataLen = 0;
            MyCamera.MV_FRAME_OUT_INFO_EX stFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();

            //超时获取一帧，超时时间为1秒
            nRet = m_pOperator.GetOneFrameTimeout(pData, ref nDataLen, m_nBufSizeForDriver, ref stFrameInfo, 1000);
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("无数据！");
                return;
            }

            IntPtr pImage = Marshal.UnsafeAddrOfPinnedArrayElement(m_pBufForSaveImage, 0);
            MyCamera.MV_SAVE_IMAGE_PARAM_EX stSaveParam = new MyCamera.MV_SAVE_IMAGE_PARAM_EX();
            stSaveParam.enImageType = MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Jpeg;
            stSaveParam.enPixelType = stFrameInfo.enPixelType;
            stSaveParam.pData = pData;
            stSaveParam.nDataLen = nDataLen;
            stSaveParam.nHeight = stFrameInfo.nHeight;
            stSaveParam.nWidth = stFrameInfo.nWidth;
            stSaveParam.pImageBuffer = pImage;
            stSaveParam.nBufferSize = m_nBufSizeForSaveImage;
            stSaveParam.nJpgQuality = 80;
            nRet = m_pOperator.SaveImage(ref stSaveParam);
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("保存失败!");
                return;
            }

            FileStream file = new FileStream("image.jpg", FileMode.Create, FileAccess.Write);
            file.Write(m_pBufForSaveImage, 0, (int)stSaveParam.nImageLen);
            file.Close();

            MessageBox.Show("保存成功!");
        }

        private void bnTriggerExec_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch Watch = new System.Diagnostics.Stopwatch();
            Watch.Start();
            m_pOperator.SetEnumValue("TriggerMode", 1);
            //触发源选择:0 - Line0;
            //           1 - Line1;
            //           2 - Line2;
            //           3 - Line3;
            //           4 - Counter;
            //           7 - 软触发;
            m_pOperator.SetEnumValue("TriggerSource", 7);

            //触发命令
            int nRet = m_pOperator.CommandExecute("TriggerSoftware");
            if (CameraOperator.CO_OK != nRet)
            {
                MessageBox.Show("触发失败！");
            }
            m_pOperator.StartGrabbing();
            Watch.Stop();
            label9.Text = Watch.ElapsedMilliseconds + "ms";
            //nRet = m_pOperator.Display(pictureBox1.Handle);
        }

        private void cbSoftTrigger_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSoftTrigger.Checked)
            {
                //触发源设为软触发
                m_pOperator.SetEnumValue("TriggerSource", 7);
                if (m_bGrabbing)
                {
                    bnTriggerExec.Enabled = true;
                }
            }
            else
            {
                m_pOperator.SetEnumValue("TriggerSource", 0);
                bnTriggerExec.Enabled = false;
            }
        }

        private void GetDeviceParam(string DeviceName)
        {
            if (m_pDeviceList.nDeviceNum == 0)
            {
                MessageBox.Show("无设备");
                return;
            }
            MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[cbDeviceList.SelectedIndex], typeof(MyCamera.MV_CC_DEVICE_INFO));
            IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
            MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

            UInt32 nNetIp1 = (gigeInfo.nNetExport & 0xFF000000) >> 24;
            UInt32 nNetIp2 = (gigeInfo.nNetExport & 0x00FF0000) >> 16;
            UInt32 nNetIp3 = (gigeInfo.nNetExport & 0x0000FF00) >> 8;
            UInt32 nNetIp4 = (gigeInfo.nNetExport & 0x000000FF);
            // 显示IP
            UInt32 nIp1 = (gigeInfo.nCurrentIp & 0xFF000000) >> 24;
            UInt32 nIp2 = (gigeInfo.nCurrentIp & 0x00FF0000) >> 16;
            UInt32 nIp3 = (gigeInfo.nCurrentIp & 0x0000FF00) >> 8;
            UInt32 nIp4 = (gigeInfo.nCurrentIp & 0x000000FF);

            lbTip.Text = "提示信息：建议IP设置范围(" + nNetIp1.ToString() + "." + nNetIp2.ToString() + "." + nNetIp3.ToString() + "." + "0" + "~" + nNetIp1.ToString() + "." + nNetIp2.ToString() + "." + nIp3.ToString() + "." + "255)";

            tbIP.Text = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();

            // 显示掩码
            nIp1 = (gigeInfo.nCurrentSubNetMask & 0xFF000000) >> 24;
            nIp2 = (gigeInfo.nCurrentSubNetMask & 0x00FF0000) >> 16;
            nIp3 = (gigeInfo.nCurrentSubNetMask & 0x0000FF00) >> 8;
            nIp4 = (gigeInfo.nCurrentSubNetMask & 0x000000FF);

            tbMask.Text = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();

            // 显示网关
            nIp1 = (gigeInfo.nDefultGateWay & 0xFF000000) >> 24;
            nIp2 = (gigeInfo.nDefultGateWay & 0x00FF0000) >> 16;
            nIp3 = (gigeInfo.nDefultGateWay & 0x0000FF00) >> 8;
            nIp4 = (gigeInfo.nDefultGateWay & 0x000000FF);

            tbDefaultWay.Text = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();
            //m_pOperator.Open(ref device);

            MyCamera.MVCC_ENUMVALUE stSelValue = new MyCamera.MVCC_ENUMVALUE();

            int nRet = m_pOperator.GetEnumValue("LineSelector", ref stSelValue);
            if (CameraOperator.CO_OK != nRet)
            {
                //MessageBox.Show("获取LineSelector失败！");
            }
            else
            {
                cbLineSel.Items.Clear();
                for (int i = 0; i < stSelValue.nSupportedNum; i++)
                {
                    cbLineSel.Items.Add("LineSelector" + stSelValue.nSupportValue[i]);
                    if (stSelValue.nCurValue == stSelValue.nSupportValue[i])
                    {
                        cbLineSel.SelectedIndex = i;
                    }
                }
            }

            MyCamera.MVCC_ENUMVALUE stModeValue = new MyCamera.MVCC_ENUMVALUE();
            nRet = m_pOperator.GetEnumValue("LineMode", ref stModeValue);
            if (CameraOperator.CO_OK != nRet)
            {
                //MessageBox.Show("获取LineMode失败！");
            }
            else
            {
                cbLineMode.Items.Clear();
                for (int i = 0; i < stModeValue.nSupportedNum; i++)
                {
                    cbLineMode.Items.Add("LineMode" + stModeValue.nSupportValue[i]);
                    if (stModeValue.nCurValue == stModeValue.nSupportValue[i])
                    {
                        cbLineMode.SelectedIndex = i;
                    }
                }
            }

            float fExposure = 0;
            m_pOperator.GetFloatValue("ExposureTime", ref fExposure);
            tbExposure.Text = fExposure.ToString("F1");

            float fGain = 0;
            m_pOperator.GetFloatValue("Gain", ref fGain);
            tbGain.Text = fGain.ToString("F1");

            float fFrameRate = 0;
            m_pOperator.GetFloatValue("ResultingFrameRate", ref fFrameRate);
            tbFrameRate.Text = fFrameRate.ToString("F1");
        }

        private void cbDeviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDeviceParam("");
        }

        private void bnSetIp_Click(object sender, EventArgs e)
        {
            if (m_pDeviceList.nDeviceNum == 0)
            {
                MessageBox.Show("无设备");
                return;
            }

            // IP转换
            IPAddress clsIpAddr;
            if (false == IPAddress.TryParse(tbIP.Text, out clsIpAddr))
            {
                MessageBox.Show("请输入正确的Ip");
                return;
            }
            long nIp = IPAddress.NetworkToHostOrder(clsIpAddr.Address);

            // 掩码转换
            IPAddress clsSubMask;
            if (false == IPAddress.TryParse(tbMask.Text, out clsSubMask))
            {
                MessageBox.Show("请输入正确的Ip");
                return;
            }
            long nSubMask = IPAddress.NetworkToHostOrder(clsSubMask.Address);

            // 网关转换
            IPAddress clsDefaultWay;
            if (false == IPAddress.TryParse(tbDefaultWay.Text, out clsDefaultWay))
            {
                MessageBox.Show("请输入正确的Ip");
                return;
            }
            long nDefaultWay = IPAddress.NetworkToHostOrder(clsDefaultWay.Address);
            int nRet;
            nRet = m_pOperator.ForceIp(m_pDeviceList.pDeviceInfo[cbDeviceList.SelectedIndex], (UInt32)(nIp >> 32), (UInt32)(nSubMask >> 32), (UInt32)(nDefaultWay >> 32));
            if (CameraOperator.CO_OK != nRet)
            {
                MessageBox.Show("IP设置失败！");
                return;
            }

            MessageBox.Show("IP设置成功！");
        }
    }
}