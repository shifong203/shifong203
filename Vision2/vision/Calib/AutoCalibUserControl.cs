using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.vision.Calib
{
    public partial class AutoCalibUserControl : UserControl
    {
        public AutoCalibUserControl()
        {
            InitializeComponent();
        }

        private void AutoCalibUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                int dn = 0;
                foreach (var item in Vision.Instance.DicCalib3D)
                {
                    if (item.Value.TRobotCall != null && item.Value.TRobotCall != "")
                    {
                        Button button = new Button();
                        button.Height = 50;
                        button.Width = 100;
                        button.Text = button.Name = item.Key + "固定标定";
                        button.Location = new Point(20, dn * button.Height);
                        button.Click += Button_Click;
                        dn++;
                        this.Controls.Add(button);
                        void Button_Click(object sender1, EventArgs e2)
                        {
                            try
                            {
                                string[] datas = item.Value.TRobotCall.Split(',');
                                if (datas.Length == 9)
                                {
                                    Vision.GetRunNameVision(datas[1]).SendMesage(item.Value.TRobotCall);
                                }
                                else
                                {
                                    MessageBox.Show("标定参数不正确");
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    if (item.Value.MRobotCall != null && item.Value.MRobotCall != "")
                    {
                        Button button = new Button();
                        button.Height = 50;
                        button.Width = 100;
                        button.Text = button.Name = item.Key + "移动标定";
                        button.Location = new Point(20, dn * button.Height);
                        button.Click += Button_Click;
                        dn++;
                        this.Controls.Add(button);
                        void Button_Click(object sender1, EventArgs e2)
                        {
                            try
                            {
                                string[] datas = item.Value.MRobotCall.Split(',');
                                if (datas.Length == 9)
                                {
                                    Vision.GetRunNameVision(datas[1]).SendMesage(item.Value.MRobotCall);
                                }
                                else
                                {
                                    MessageBox.Show("标定参数不正确");
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {


            }
        }


    }
}
