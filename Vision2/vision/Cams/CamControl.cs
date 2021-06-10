using System;
using System.Drawing;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;
namespace Vision2.vision.Cams
{
    public partial class CamControl : UserControl
    {
        public CamControl(ICamera cam, HalconRun halconRun = null) : this()
        {
            UpDataRe(cam, halconRun);

        }
        public CamControl()
        {
            InitializeComponent();
        }

        private void Cam_LinkSt(bool key)
        {
            if (!this.IsDisposed)
            {
                this.Invoke(new Action(() =>
                {
                    if (key)
                    {
                        Lebm_bCamIsOK.Text = "已连接";
                        Lebm_bCamIsOK.ForeColor = Color.Green;
                    }
                    else
                    {
                        Lebm_bCamIsOK.Text = "断开";
                        Lebm_bCamIsOK.ForeColor = Color.Red;
                    }
                }));


            }
            else
            {
                Cam.LinkEnvet -= Cam_LinkSt;
            }
        }

        private HalconRun Halcon;
        private ICamera Cam;

        private void CamControl_Load(object sender, EventArgs e)
        {

        }
        bool isChetr;
        public void UpDataRe(ICamera cam, HalconRun halconRun = null)
        {
            try
            {
                isChetr = true;
                Cam = cam;
                Halcon = halconRun;
                if (Cam != null)
                {
                    propertyGrid1.SelectedObject = Cam;
                    cam.LinkEnvet += Cam_LinkSt;
                    if (cam.IsCamConnected)
                    {
                        Lebm_bCamIsOK.Text = "已连接";
                        Lebm_bCamIsOK.ForeColor = Color.Green;
                    }
                    else
                    {
                        Lebm_bCamIsOK.Text = "断开";
                        Lebm_bCamIsOK.ForeColor = Color.Red;
                    }
                    hSBExposure.Value = Convert.ToInt32(Cam.ExposureTime.ToString());
                    txExposure.Text = (hSBExposure.Value).ToString();
                    if (Cam.Gain != 0)
                    {
                        hSBGain.Value = Convert.ToInt32(Cam.Gain.ToString());
                    }
                    ThridLibray.Enumerator.GigeCameraNetInfo(Cam.Index, out string MaxAdd, out string IP, out string subnetMaxk, out string defaultg);
                    Cam.IP = IP;
                    ThridLibray.IDeviceInfo device = ThridLibray.Enumerator.getDeviceInfoByKey(Cam.ID);

                    ThridLibray.IGigeInterfaceInfo device2 = ThridLibray.Enumerator.GigeInterfaceInfo(Cam.Index);
                    Cam.IntIP = device2.IPAddress;
                    TBGain.Text = hSBGain.Value.ToString();
                    //textBoxCamName.Text = Cam.Name;
                    //TxDeviceID.Text = Cam.ID;
                    //TxCamIp_address.Text = Cam.IP;
                    //textBox1.Text = Cam.IntIP;
                    if (Cam.IsCamConnected)
                    {
                        //if (!Vision.Instance.HalconCam)
                        //{
                        //    HTuple values = Cam.GetFramegrabberParam("BalanceWhiteAuto");
                        //    if (values.Length == 1)
                        //    {
                        //        CamIntPut1.SelectedItem = values.S;
                        //    }

                        //}
                        //else
                        //{
                        //    CamIntPut1.SelectedItem = Cam.GetFramegrabberParam("BalanceWhiteAuto");
                        //}
                        toCamLink.Text = "断开";

                    }
                    else
                    {
                        toCamLink.Text = "连接";
                    }
                    if (Cam.Grabbing)
                    {
                        btnThreadGrab.Text = "停止采图";
                    }
                }
                if (Halcon == null)
                {
                    Halcon = Vision.GetFocusRunHalcon();
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show("刷新相机参数错误:" + ex.Message);
            }
            isChetr = false;
        }

        private void toCamLink_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (!Cam.IsCamConnected)
                    {
                        Lebm_bCamIsOK.Text = "链接中";
                        Cam.OpenCam();
                    }
                    else
                    {
                        Cam.CloseCam();
                        //Lebm_bCamIsOK.Text = "断开";
                    }
                }
                catch (Exception ex)
                {
                    Lebm_bCamIsOK.Text = "连接失败";
                    MessageBox.Show(ex.Message);
                }
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

                Vision.GetFocusRunHalcon();
                //Cam.CoordinateMeassage = Coordinate.Coordinate_Type.XYZU3D;

                Halcon.UPStart();
                Halcon.Image(Cam.GetImage());
                Halcon.EndChanged(Halcon.GetOneImageR());
                // halcon.GetOneImageR().AddMeassge(Cam.RunTime + "ms");
                Halcon.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show("相机採图错误!" + ex.Message);
            }
            //if (Cam.CoordinateMeassage ==Coordinate.Coordinate_Type.XYZU3D)
            //{
            //    Cam.CoordinateMeassage = Coordinate.Coordinate_Type.Hide;
            //}
        }

        private void btnThreadGrab_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnThreadGrab.Text == "实时采图")
                {
                    Cam.Straing(Halcon);
                    btnThreadGrab.Text = "停止采图";
                }
                else
                {
                    btnThreadGrab.Text = "实时采图";
                    Cam.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void hSBExposure_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                txExposure.Text = (hSBExposure.Value).ToString();
                Cam.ExposureTime = hSBExposure.Value;
                //Cam.SetFramegrabberParam();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void hSBGain_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                TBGain.Text = (hSBGain.Value).ToString();
                Cam.Gain = (int)hSBGain.Value;
                //Cam.SetFramegrabberParam();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txExposure_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (double.TryParse(txExposure.Text, out double intExp))
                {
                    if (intExp != 0)
                    {
                        Cam.ExposureTime = hSBExposure.Value = (int)(intExp);

                    }
                    else
                    {
                        txExposure.Text = (hSBExposure.Value).ToString();
                    }

                    //Cam.SetFramegrabberParam("ExposureTime", Cam.ExposureTimeAbs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TBGain_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (int.TryParse(TBGain.Text, out int intExp))
                {
                    hSBGain.Value = intExp;
                    Cam.Gain = (int)intExp;

                    //Cam.SetFramegrabberParam("GainRaw", Cam.M_GainInt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void CamIntPut1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cam.TriggerMode = CamIntPut1.SelectedItem.ToString();
            try
            {
                if (Cam.IsCamConnected)
                {
                    numericUpDown1.Value = decimal.Parse(Cam.GetFramegrabberParam("BalanceRatioRed"));
                    numericUpDown2.Value = decimal.Parse(Cam.GetFramegrabberParam("BalanceRatioGreen"));
                    numericUpDown3.Value = decimal.Parse(Cam.GetFramegrabberParam("BalanceRatioBlue"));
                }
            }
            catch (Exception ex)
            {
            }
            if (isChetr)
            {
                return;
            }
            if (CamIntPut1.SelectedIndex >= 0)
            {
                Cam.SetProgramValue("BalanceWhiteAuto", (uint)CamIntPut1.SelectedIndex);
            }


        }

        private void TxCamIp_address_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (isChetr)
            {
                return;
            }
            try
            {
                Cam.SetProgramValue("BalanceRatioRed", (double)numericUpDown1.Value);
                Cam.SetProgramValue("BalanceRatioGreen", (double)numericUpDown2.Value);
                Cam.SetProgramValue("BalanceRatioBlue", (double)(numericUpDown2.Value));
            }
            catch (Exception ex)
            {


            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //CamParam camParam = new CamParam();
            //camParam.m_IDStr = Cam.Name;
            //Cam = camParam;
        }
    }
}