using System;
using System.Collections.Generic;

//using System.Threading.Tasks;
using System.Windows.Forms;
using ThridLibray;

namespace Vision2.vision.Cams
{
    public partial class DahuaCams : UserControl
    {
        public DahuaCams()
        {
            InitializeComponent();
        }

        /* 设备对象 */
        private ThridLibray.IDevice m_dev;
        private List<IDeviceInfo> li;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                li = Enumerator.EnumerateDevices();

                listBox2.Items.Clear();
                if (li.Count > 0)
                {
                    for (int i = 0; i < li.Count; i++)
                    {
                        listBox2.Items.Add(li[i].Key);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DahuaCams_Load(object sender, EventArgs e)
        {
            try
            {
                /* 设备搜索 */
                li = Enumerator.EnumerateDevices();
                if (li.Count > 0)
                {
                    for (int i = 0; i < li.Count; i++)
                    {
                        listBox2.Items.Add(li[i].Key);
                    }
                }
                foreach (var item in Vision.Instance.RunCams)
                {
                    listBox1.Items.Add(item.Key);
                }
                foreach (var item in Vision.Instance.RunDahenCams)
                {
                    listBox3.Items.Add(item.Key);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox2.SelectedItem != null)
                {
               
                        IDeviceInfo device = Enumerator.getDeviceInfoByKey(listBox2.SelectedItem.ToString());
                   
                        if (device.ManufactureInfo.StartsWith("Daheng"))
                    {
                        if (!Vision.Instance.RunDahenCams.ContainsKey(listBox2.SelectedItem.ToString()))
                        {
                            DaHenCamera camera = new DaHenCamera();
                            camera.Index = listBox2.SelectedIndex;
                            camera.ID = listBox2.SelectedItem.ToString();
                            camera.SerialNum = device.SerialNumber;
                            camera.Name = listBox2.SelectedItem.ToString();
                            Vision.Instance.RunDahenCams.Add(listBox2.SelectedItem.ToString(), camera);
                            listBox3.Items.Add(listBox2.SelectedItem.ToString());
                        }
                        }
                        else
                    {
                        if (!Vision.Instance.RunCams.ContainsKey(listBox2.SelectedItem.ToString()))
                        {
                            DahuaCamera camera = new DahuaCamera();
                            camera.Index = listBox2.SelectedIndex;
                            camera.ID = listBox2.SelectedItem.ToString();
                            camera.SerialNum = device.SerialNumber;
                            camera.Name = listBox2.SelectedItem.ToString();
                            Vision.Instance.RunCams.Add(listBox2.SelectedItem.ToString(), camera);
                            listBox1.Items.Add(listBox2.SelectedItem.ToString());
                        }
          
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (li == null)
                {
                    return;
                }

                ThridLibray.Enumerator.GigeCameraNetInfo(listBox2.SelectedIndex, out string MaxAdd, out string IP, out string subnetMaxk, out string defaultg);
                TxCamIp_address.Text = IP;

                IDeviceInfo device = Enumerator.getDeviceInfoByKey(listBox2.SelectedItem.ToString());
                propertyGrid1.SelectedObject = device;
                IGigeInterfaceInfo device2 = Enumerator.GigeInterfaceInfo(listBox2.SelectedIndex);
                propertyGrid2.SelectedObject = device2;
                IGigeDeviceInfo gigeDeviceInfo = Enumerator.GigeCameraInfo(listBox2.SelectedIndex);
                propertyGrid3.SelectedObject = gigeDeviceInfo;
                m_dev = Enumerator.GetDeviceByIndex(listBox2.SelectedIndex);
                propertyGrid4.SelectedObject = m_dev.UserSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 设置IP_Click(object sender, EventArgs e)
        {
            try
            {
                Enumerator.GigeCameraNetInfo(listBox2.SelectedIndex, out string MaxAdd, out string IPt, out string subnetMaxk, out string defaultg);
                if (Enumerator.GigeForceIP(listBox2.SelectedIndex, TxCamIp_address.Text, subnetMaxk, defaultg))
                {
                    MessageBox.Show("设置成功");
                }
                else
                {
                    MessageBox.Show("设置失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    if (Vision.GetNameCam(listBox1.SelectedItem.ToString()) != null)
                    {
                        IDevice m_deve = Vision.GetNameCam(listBox1.SelectedItem.ToString()).GetIDevice() as IDevice;
                        if (m_deve != null)
                        {
                            propertyGrid1.SelectedObject = m_deve.DeviceInfo;

                            propertyGrid3.SelectedObject = m_deve.ParameterCollection;
                        }
                        else
                        {
                            propertyGrid1.SelectedObject = Vision.GetNameCam(listBox1.SelectedItem.ToString());
                        }
                 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    if (Vision.Instance.RunCams.ContainsKey(listBox1.SelectedItem.ToString()))
                    {
                        Vision.Instance.RunCams.Remove(listBox1.SelectedItem.ToString());
                        listBox1.Items.Remove(listBox1.SelectedItem.ToString());
                    }
                    else
                    {
                        MessageBox.Show(listBox1.SelectedItem.ToString() + "不存在");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox3.SelectedItem != null)
                {
                    if (Vision.Instance.RunDahenCams.ContainsKey(listBox3.SelectedItem.ToString()))
                    {
                        Vision.Instance.RunDahenCams.Remove(listBox3.SelectedItem.ToString());
                        listBox3.Items.Remove(listBox3.SelectedItem.ToString());
                    }
                    else
                    {
                        MessageBox.Show(listBox3.SelectedItem.ToString() + "不存在");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox3.SelectedItem != null)
                {
                    if (Vision.GetNameCam(listBox3.SelectedItem.ToString()) != null)
                    {
                        IDevice m_deve = Vision.GetNameCam(listBox3.SelectedItem.ToString()).GetIDevice() as IDevice;
                        if (m_deve != null)
                        {
                            propertyGrid1.SelectedObject = m_deve.DeviceInfo;

                            propertyGrid3.SelectedObject = m_deve.ParameterCollection;
                        }
                        else
                        {
                            propertyGrid1.SelectedObject = Vision.GetNameCam(listBox3.SelectedItem.ToString());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CamsForm camsForm = new CamsForm();
            camsForm.Show();
        }
    }
}