using HalconDotNet;
using NokidaE.vision.HalconRunFile.RunProgramFile;
using System;
using System.Threading;
using System.Windows.Forms;

namespace NokidaE.vision.Cams
{
    public partial class CamPragramV : UserControl
    {
        public CamPragramV()
        {
            InitializeComponent();
            Halcon = Vision.GetRunNameVision();
        }

        private HalconRun Halcon;
        private CamParam Cam;

        private void CamPragram_Load(object sender, EventArgs e)
        {
            刷新();
        }

        private void 刷新()
        {
            try
            {
                BindingSource bs = new BindingSource();
                //将泛型集合对象的值赋给BindingSourc对象的数据源
                bs.DataSource = Vision.Instance.RunCams.Keys;
                this.listCamR.DataSource = bs;
                CamR.DataSource = bs;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOneShot_Click(object sender, EventArgs e)
        {

            try
            {
                //camParam.Run(Vision.GetFocusRunHalcon());

                //ReadImaeTime.Text = "采图时间：" + camParam.RunTime + "ms";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }//单次采图

        private bool ThreadStop = false;

        private void btnThreadGrab_Click(object sender, EventArgs e)
        {
            if (btnThreadGrab.Text == "实时采图")
            {
                //实时线程
                Thread thread = new Thread(() =>
               {
                   try
                   {
                       while (ThreadStop)
                       {
                           btnOneShot_Click(sender, e);
                       }
                   }
                   catch (Exception)
                   {
                       btnThreadGrab.Text = "实时采图";
                   }
               });
                thread.IsBackground = false;
                ThreadStop = true;
                thread.Start();
                btnThreadGrab.Text = "停止采图";
            }
            else
            {
                btnThreadGrab.Text = "实时采图";
                ThreadStop = false;
            }
        }

        private void listCamR_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CamR.SelectedItem == null)
                {
                    return;
                }
                //Halcon.CamNameStr = CamR.SelectedItem.ToString();
                if (!Vision.Instance.RunCams.ContainsKey(listCamR.SelectedItem.ToString())) return;

                if (listCamR.SelectedIndex >= 0)
                {
                    TxDeviceID.Text = Vision.Instance.RunCams[CamR.SelectedItem.ToString()].ID;
                    TxCamName.Text = Vision.Instance.RunCams[CamR.SelectedItem.ToString()].Name;
                    TxCamIp_address.Text = Vision.Instance.RunCams[CamR.SelectedItem.ToString()].IP;
                    //TxMac_address.Text = Vision.Instance.RunCams[CamR.SelectedItem.ToString()].Mac;
                    //TxFramegrabber.Text = CamParam.camS_Information[listCamR.SelectedIndex].information;
                    TxInterface_ip_address.Text = Vision.Instance.RunCams[CamR.SelectedItem.ToString()].IntIP;
                }
            }
            catch (Exception)
            {
            }
        }

        private void CamR_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Halcon.CamNameStr = CamR.SelectedItem.ToString();
        }
        DahuaCamera camParam;
        private void toCamLink_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (TxDeviceID.Text != "")
                    {
                        if (toCamLink.Text != "连接成功")
                        {
                            toCamLink.Text = "链接中";
                            //camParam.OnLinkSt(false);

                            if (camParam.IsCamConnected)
                            {
                                toCamLink.Text = "连接成功";
                            }
                        }
                        else
                        {

                            toCamLink.Text = "链接";
                        }
                    }

                }
                catch (Exception ex)
                {
                    Lebm_bCamIsOK.Text = "连接失败";
                    Vision.Log(ex.Message);
                    toCamLink.Text = "链接";
                }
            }
            catch (Exception ex)
            {
                Vision.Log(ex.Message);
            }
        }

        private void hSBExposure_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                //hSBExposure.Focus();
                txExposure.Text = (hSBExposure.Value * 0.001).ToString();
                Cam.ExposureTimeAbs = hSBExposure.Value;
                Cam.SetFramegrabberParam();
            }
            catch (Exception ex)
            {
            }
        }

        private void hSBGain_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                //hSBGain.Focus();
                TBGain.Text = (hSBGain.Value).ToString();
                Vision.Instance.RunCams[Halcon.CamNameStr].Gain = (int)hSBGain.Value;
                if (Vision.Instance.RunCams[Halcon.CamNameStr].IsCamConnected)
                {
                    Vision.Instance.RunCams[Halcon.CamNameStr].SetProgramValue("Gain", Vision.Instance.RunCams[Halcon.CamNameStr].Gain);
                }
            }
            catch (Exception)
            {
            }
        }



        //周期触发
        private void RunTime_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnRunStartOpen_Click(object sender, EventArgs e)
        {
        }

        private void txExposure_TextChanged(object sender, EventArgs e)
        {
        }

        //本地搜索
        private void ButSearchCam_Click(object sender, EventArgs e)
        {
            try
            {
                if (Vision.Instance.HalconCam)
                {
                    string[] list = new string[] { };

                    //string[] list = CamParamHK.DeviceListAcq().ToArray();

                    TxCamName.Text = TxDeviceID.Text = TxInterface_ip_address.Text
                     = TxMac_address.Text = TxCamIp_address.Text = "";
                    listCanSeek.Items.Clear();
                    for (int i = 0; i < list.Length; i++)
                    {
                        listCanSeek.Items.Add(list[i]);
                    }
                }
                else
                {
                    if (CbCamQuery.SelectedItem == null || CbCamQuery.SelectedItem.ToString() == "")
                    {
                        CbCamQuery.SelectedItem = "info_boards";
                    }
                    ////查询当前接口
                    CamParam.Cam_information.SeekCam();
                    if (CamParam.Cam_information.Cam_Information.Count > 0)
                    {
                        TxInterface_ip_address.Text = CamParam.Cam_information.Cam_Information[0].PC_IP;
                        TxCamName.Text = TxDeviceID.Text = CamParam.Cam_information.Cam_Information[0].ID;
                        TxCamIp_address.Text = CamParam.Cam_information.Cam_Information[0].IP;
                        TxMac_address.Text = CamParam.Cam_information.Cam_Information[0].Mac;
                        textBox1.AppendText("相机数:" + CamParam.Cam_information.Cam_Information.Count + Environment.NewLine);
                    }
                    else
                    {
                        textBox1.AppendText("未找到相机" + Environment.NewLine);
                    }
                    for (int i = 0; i < CamParam.Cam_information.Cam_Information.Count; i++)
                    {
                        listCanSeek.Items.Add(CamParam.Cam_information.Cam_Information[i].ID);
                        textBox1.AppendText(CamParam.Cam_information.Cam_Information[i].ID + Environment.NewLine);
                    }

                }



            }
            catch (Exception ex)
            {
                Vision.Log(ex.Message);
            }
        }
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="DeviceName"></param>
        private void GetDeviceParam(int id)
        {
            return;
            //if (CamParamHK.m_pDeviceList.nDeviceNum == 0)
            //{
            //    MessageBox.Show("无设备");
            //    return;
            //}
            //MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(CamParamHK.m_pDeviceList.pDeviceInfo[id], typeof(MyCamera.MV_CC_DEVICE_INFO));
            //IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
            //MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
            //UInt32 nNetIp1 = (gigeInfo.nNetExport & 0xFF000000) >> 24;
            //UInt32 nNetIp2 = (gigeInfo.nNetExport & 0x00FF0000) >> 16;
            //UInt32 nNetIp3 = (gigeInfo.nNetExport & 0x0000FF00) >> 8;
            //UInt32 nNetIp4 = (gigeInfo.nNetExport & 0x000000FF);
            //// 显示IP
            //UInt32 nIp1 = (gigeInfo.nCurrentIp & 0xFF000000) >> 24;
            //UInt32 nIp2 = (gigeInfo.nCurrentIp & 0x00FF0000) >> 16;
            //UInt32 nIp3 = (gigeInfo.nCurrentIp & 0x0000FF00) >> 8;
            //UInt32 nIp4 = (gigeInfo.nCurrentIp & 0x000000FF);
            //TxInterface_ip_address.Text = nNetIp1.ToString() + "." + nNetIp2.ToString() + "." + nNetIp3.ToString() + "." + nNetIp4.ToString();
            ////m = "提示信息：建议IP设置范围(" + nNetIp1.ToString() + "." + nNetIp2.ToString() + "." + nNetIp3.ToString() + "." + "0" + "~" + nNetIp1.ToString() + "." + nNetIp2.ToString() + "." + nIp3.ToString() + "." + "255)";
            //TxCamIp_address.Text = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();
            //// 显示掩码
            //nIp1 = (gigeInfo.nCurrentSubNetMask & 0xFF000000) >> 24;
            //nIp2 = (gigeInfo.nCurrentSubNetMask & 0x00FF0000) >> 16;
            //nIp3 = (gigeInfo.nCurrentSubNetMask & 0x0000FF00) >> 8;
            //nIp4 = (gigeInfo.nCurrentSubNetMask & 0x000000FF);
            ////tbMask.Text = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();
            //// 显示网关
            //nIp1 = (gigeInfo.nDefultGateWay & 0xFF000000) >> 24;
            //nIp2 = (gigeInfo.nDefultGateWay & 0x00FF0000) >> 16;
            //nIp3 = (gigeInfo.nDefultGateWay & 0x0000FF00) >> 8;
            //nIp4 = (gigeInfo.nDefultGateWay & 0x000000FF);
            //TxDeviceID.Text = gigeInfo.chSerialNumber;
            //TxCamName.Text = gigeInfo.chSerialNumber;
        }

        private void listCanSeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (!Vision.Instance.HalconCam)
                    {

                        TxInterface_ip_address.Text = CamParam.Cam_information.Cam_Information[listCanSeek.SelectedIndex].PC_IP;
                        TxCamIp_address.Text = CamParam.Cam_information.Cam_Information[listCanSeek.SelectedIndex].IP;
                        TxCamName.Text = CamParam.Cam_information.Cam_Information[listCanSeek.SelectedIndex].ID;
                        TxDeviceID.Text = CamParam.Cam_information.Cam_Information[listCanSeek.SelectedIndex].ID;
                        textBox2.Text = CamParam.Cam_information.Cam_Information[listCanSeek.SelectedIndex].information;
                    }
                    else
                    {
                        GetDeviceParam(listCanSeek.SelectedIndex);
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

        private void BtnSeveCam_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxCamName.Text == "")
                {
                    MessageBox.Show("名称不能为空");
                    return;
                }
                if (!Vision.Instance.HalconCam)
                {

                    camParam = new DahuaCamera();

                }
                else
                {
                    //camParam = new CamParamHK();
                }

                camParam.ID = TxDeviceID.Text;
                camParam.Name = TxCamName.Text;
                camParam.IP = TxCamIp_address.Text;
                camParam.IntIP = TxInterface_ip_address.Text;
                //camParam.camS_Information.information = textBox2.Text;
                if (!Vision.Instance.RunCams.ContainsKey(TxCamName.Text))//是否存在修改
                {
                    Vision.Instance.RunCams.Add(TxCamName.Text, camParam);

                }
                Vision.Instance.RunCams[TxCamName.Text] = camParam;
                刷新();
            }
            catch (Exception ex)
            {
                Vision.Log(ex.Message);
            }
        }

        public HTuple listfiles = new HTuple();

        private void listBFileImages_MouseDown(object sender, MouseEventArgs e)
        {
            //listBFileImages.ContextMenuStrip = null;
            //if (listBFileImages.SelectedItem == null)
            //{
            //    return;
            //}
            //if (e.Button == MouseButtons.Left)
            //{
            //    HOperatorSet.ReadImage(out HObject hObject, listBFileImages.SelectedItem.ToString());
            //    Halcon.Image  = hObject;
            //    //MainForm.visionWindowDictionary[Halcon.hWindowName].threadOK = true;

            //} //左键单击
            //if (e.Button == MouseButtons.Right)
            //{
            //    Point ClickPoint = new Point(e.X, e.Y);
            //    int intf = listBFileImages.IndexFromPoint(ClickPoint);
            //    if (intf >= 0 && intf < listBFileImages.Items.Count)
            //    {
            //        listBFileImages.SelectedIndex = intf;
            //        contextMenuStrip1.Items.Clear();
            //        ToolStripDropDownItem Delete = (ToolStripDropDownItem)contextMenuStrip1.Items.Add("删除");
            //        ToolStripDropDownItem Open = (ToolStripDropDownItem)contextMenuStrip1.Items.Add("添加");
            //        Delete.MouseDown += (object senderD, MouseEventArgs eD) =>
            //        {
            //            for (int i = 0; i < Halcon.fileImage.Length; i++)
            //            {
            //                if (Halcon.fileImage[i]==listBFileImages.SelectedItem.ToString())
            //                {
            //                    Halcon.fileImage=Halcon.fileImage.TupleRemove(i);
            //                    break;
            //                }
            //            }
            //            listBFileImages.DataSource = Halcon.fileImage.ToSArr();
            //        };
            //        Open.MouseDown += (object senderO, MouseEventArgs eO) =>
            //        {
            //            openFiles_Click(senderO, eO);
            //        };
            //        listBFileImages.ContextMenuStrip = contextMenuStrip1;
            //    }

            //}//右键单击
        }

        /// <summary>
        /// 文件夹采图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddIamge_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Title = "请选择图片文件可多选";
            //openFileDialog.Multiselect = true;
            //openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
            //openFileDialog.InitialDirectory = Application.StartupPath;
            //openFileDialog.ShowDialog();
            //if (openFileDialog.FileName.Length == 0) return;
            ////listBFileImages.Items.AddRange(openFileDialog.FileNames);
            //Halcon.fileImage.Append(openFileDialog.FileNames);
            //listBFileImages.DataSource = Halcon.fileImage.ToSArr();
            //ImageNumber.Text = "数量：" + listBFileImages.Items.Count;
        }//添加照片

        private void btnSave_Click(object sender, EventArgs e)
        {
            Cam.Name = CamR.SelectedItem.ToString();
            Cam.SaveThis(Halcon.ProgramPathD);
        }

        private void btnReadCam_Click(object sender, EventArgs e)
        {
            if (HalconRun.ReadPathJsonToCalss(Application.StartupPath + "\\VisionPragram\\Cam" + Cam.Name, out Cam))
            {
                MessageBox.Show("读取成功");
            }
            else
            {
                MessageBox.Show("读取失败！");
            }
        }

        private void 设置IP_Click(object sender, EventArgs e)
        {
            try
            {


                //ThridLibray.Enumerator.GigeCameraNetInfo(this.Index, out string MaxAdd, out string IPt, out string subnetMaxk, out string defaultg);

                //return ThridLibray.Enumerator.GigeForceIP(this.Index, IP, subnetMaxk, defaultg);
                //if (CamParamHK.SetCamIP(TxCamIp_address.Text, TxDeviceID.Text))
                //{
                //    MessageBox.Show("设置成功!");
                //}
                //else
                //{
                //    MessageBox.Show("设置失败!");
                //}
            }
            catch (Exception)
            {
            }

        }
    }
}