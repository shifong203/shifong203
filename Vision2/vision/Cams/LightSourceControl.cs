using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Vision2.vision.Cams
{
    public partial class LightSourceControl : UserControl
    {
        public LightSourceControl()
        {
            InitializeComponent();
        }

        public void GetData(LightSource lightSo)
        {
            try
            {
                iscont = true;
                this.groupBox1.Text = "光源" + lightSo.Rs232Name;
                lightSource = lightSo;
                trackBar1.Value = lightSource.H1;
                trackBar2.Value = lightSource.H2;
                trackBar3.Value = lightSource.H3;
                trackBar4.Value = lightSource.H4;
                numericUpDown1.Value = lightSource.H1;
                numericUpDown2.Value = lightSource.H2;
                numericUpDown3.Value = lightSource.H3;
                numericUpDown4.Value = lightSource.H4;
                checkBox1.Checked = lightSource.H1Off;
                checkBox2.Checked = lightSource.H2Off;
                checkBox3.Checked = lightSource.H3Off;
                checkBox4.Checked = lightSource.H4Off;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            iscont = false;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                SetScroll();
            }
            catch (Exception)
            {
            }
        }

        private bool iscont = false;
        private LightSource lightSource;

        private void SetScroll()
        {
            try
            {
                if (iscont)
                {
                    return;
                }
                iscont = true;
                lightSource.H1 = (byte)trackBar1.Value;
                lightSource.H2 = (byte)trackBar2.Value;
                lightSource.H3 = (byte)trackBar3.Value;
                lightSource.H4 = (byte)trackBar4.Value;
                numericUpDown1.Value = lightSource.H1;
                numericUpDown2.Value = lightSource.H2;
                numericUpDown3.Value = lightSource.H3;
                numericUpDown4.Value = lightSource.H4;
                lightSource.SetHx();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            iscont = false;
        }

        private void SetP()
        {
            try
            {
                if (iscont)
                {
                    return;
                }
                iscont = true;
                lightSource.H1 = (byte)numericUpDown1.Value;
                lightSource.H2 = (byte)numericUpDown2.Value;
                lightSource.H3 = (byte)numericUpDown3.Value;
                lightSource.H4 = (byte)numericUpDown4.Value;
                trackBar1.Value = lightSource.H1;
                trackBar2.Value = lightSource.H2;
                trackBar3.Value = lightSource.H3;
                trackBar4.Value = lightSource.H4;
                lightSource.H1Off = checkBox1.Checked;
                lightSource.H2Off = checkBox2.Checked;
                lightSource.H3Off = checkBox3.Checked;
                lightSource.H4Off = checkBox4.Checked;
                lightSource.SetHx();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            iscont = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetP();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            iscont = true;
            checkBox4.Checked = checkBox3.Checked =
                checkBox2.Checked = checkBox1.Checked = true;
            trackBar1.Value = 255;
            trackBar2.Value = 255;
            trackBar3.Value = 255;
            trackBar4.Value = 255;
            iscont = false;
            SetScroll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            iscont = true;
            checkBox4.Checked = checkBox3.Checked =
            checkBox2.Checked = checkBox1.Checked = false;
            trackBar1.Value = 0;
            trackBar2.Value = 0;
            trackBar3.Value = 0;
            trackBar4.Value = 0;
            iscont = false;
            SetScroll();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            SetScroll();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            SetScroll();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            SetScroll();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            SetP();
        }
    }

    public class LightSource
    {
        public class LightSourceData
        {
            [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("1通道")]
            public Int16 H1 { get; set; }

            [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("2通道")]
            public Int16 H2 { get; set; }

            [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("3通道")]
            public Int16 H3 { get; set; }

            [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("4通道")]
            public Int16 H4 { get; set; }
        }

        private SerialPortHelper serialPort;

        //public string OffName = "浮根";
        public string Rs232Name = "COM1";

        public LightSource(int com)
        {
            Rs232Name = "COM" + com;
            serialPort = new SerialPortHelper(com, 15600, out string errr);
        }

        public LightSource()
        {
            serialPort = new SerialPortHelper(0, 15600, out string errr);
        }

        /// <summary>
        /// 浮根
        /// </summary>
        /// <returns></returns>
        private string CheckChData()
        {
            string data = "S";
            if (H1Off)
            {
                data += H1.ToString("000") + "T";
            }
            else
            {
                data += H1.ToString("000") + "F";
            }
            if (H2Off)
            {
                data += H2.ToString("000") + "T";
            }
            else
            {
                data += H2.ToString("000") + "F";
            }

            if (H3Off)
            {
                data += H3.ToString("000") + "T";
            }
            else
            {
                data += H3.ToString("000") + "F";
            }
            if (H4Off)
            {
                data += H4.ToString("000") + "T";
            }
            else
            {
                data += H4.ToString("000") + "F";
            }

            return data + "C#";
        }

        /// <summary>
        /// 凯威
        /// </summary>
        /// <returns></returns>
        public string CheckChKWData()
        {
            string data = "S";
            if (H1Off)
            {
                data += H1.ToString("000") + "T";
                serialPort.Write("#1106411");
            }
            else
            {
                serialPort.Write("#2106411");

                data += H1.ToString("000") + "F";
            }
            Thread.Sleep(100);
            if (H2Off)
            {
                serialPort.Write("#1206412");
                data += H2.ToString("000") + "T";
            }
            else
            {
                serialPort.Write("#2106412");
                data += H2.ToString("000") + "F";
            }
            Thread.Sleep(100);
            if (H3Off)
            {
                serialPort.Write("#1306413");
                data += H3.ToString("000") + "T";
            }
            else
            {
                serialPort.Write("#2106412");
                data += H3.ToString("000") + "F";
            }
            Thread.Sleep(100);
            if (H4Off)
            {
                serialPort.Write("#1406414");
                data += H4.ToString("000") + "T";
            }
            else
            {
                serialPort.Write("#2106413");
                data += H4.ToString("000") + "F";
            }

            return data;
        }

        public void Opne()
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    if (Vision.Instance.OffName == "浮根")
                    {
                        serialPort.Parity = Parity.None;
                        serialPort.BaudRate = 19200;
                        serialPort.StopBits = StopBits.One;
                    }
                    else if (Vision.Instance.OffName == "浮根")
                    {
                        serialPort.BaudRate = 9600;
                        serialPort.DataBits = 8;
                        serialPort.Parity = Parity.None;
                    }
                    else
                    {
                        serialPort.BaudRate = 9600;
                        serialPort.DataBits = 8;
                        serialPort.Parity = Parity.None;
                    }
                    serialPort.PortName = Rs232Name;
                    serialPort.Open();
                }
            }
            catch (Exception)
            {
            }
        }

        public void SetHx()
        {
            Opne();
            if (Vision.Instance.OffName == "浮根")
            {
                serialPort.Write(CheckChData() + "C#");
            }
            else if (Vision.Instance.OffName == "嘉历")
            {
                SetLightV();
            }
            else
            {
                CheckChKWData();
            }
        }

        public void SetOFF()
        {
            Opne();
            if (Vision.Instance.OffName == "浮根")
            {
                serialPort.Write(CheckChData() + "C#");
            }
            else if (Vision.Instance.OffName == "嘉历")
            {
                SetTimeValue('2', '1', "000");
                SetTimeValue('2', '2', "000");
                SetTimeValue('2', '3', "000");
                SetTimeValue('2', '4', "000");
            }
            else
            {
                CheckChKWData();
            }
        }

        public void SetLightSource(LightSourceData lightSource)
        {
            try
            {
                H1 = lightSource.H1;
                H2 = lightSource.H2;
                H3 = lightSource.H3;
                H4 = lightSource.H4;
                this.SetHx();
            }
            catch (Exception ex)
            {
            }
        }

        [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("1通道")]
        public Int16 H1 { get; set; } = 255;

        [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("2通道")]
        public Int16 H2 { get; set; } = 255;

        [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("3通道")]
        public Int16 H3 { get; set; } = 255;

        [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("4通道")]
        public Int16 H4 { get; set; } = 255;

        private short h1 = -10;
        private short h2 = -10;
        private short h3 = -10;
        private short h4 = -10;
        public bool H1Off;

        public bool H2Off;

        public bool H3Off;

        public bool H4Off;

        private StringBuilder DataUsing = new StringBuilder();

        //设置频闪时间模块
        /// <summary>
        /// 嘉丽光源控制
        /// </summary>
        /// <param name="code">1：打开对应通道亮度；
        /// 2：关闭对应通道亮度；
        /// 3：设置对应通道亮度参数；
        /// 4：读出对应通道亮度参数</param>
        /// <param name="Chanel">2~4通道</param>
        /// <param name="Value">0~255</param>
        public void SetTimeValue(char code, char Chanel, string Value)
        {
            DataUsing.Clear();
            DataUsing.Append("$");
            DataUsing.Append(code);
            DataUsing.Append(Chanel);
            int a = Convert.ToInt32(Value);
            Value = a.ToString("X");
            if (Value.Length == 1)
            {
                DataUsing.Append("00" + Value);
            }
            else
            {
                DataUsing.Append("0" + Value);
            }
            try
            {
                string b = xorCheack(DataUsing.ToString());
                serialPort.WriteLine(b);//发送数据
                //Thread.Sleep(100);
                //richTextBox1.AppendText(b + "\r\n");
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// 设置光源参数
        /// </summary>
        public void SetLightV()
        {
            if (H1 >= 0)
            {
                if (H1 > 0)
                {
                    SetTimeValue('1', '1', H1.ToString("000"));
                }
                else
                {
                    SetTimeValue('2', '1', H1.ToString("000"));
                }
            }

            if (H2 >= 0)
            {
                if (H2 > 0)
                {
                    SetTimeValue('1', '2', H2.ToString("000"));
                }
                else
                {
                    SetTimeValue('2', '2', H2.ToString("000"));
                }
            }

            if (H3 >= 0)
            {
                if (H3 > 0)
                {
                    SetTimeValue('1', '3', H3.ToString("000"));
                }
                else
                {
                    SetTimeValue('2', '3', H3.ToString("000"));
                }
            }
            if (H4 >= 0)
            {
                if (H4 > 0)
                {
                    SetTimeValue('1', '4', H4.ToString("000"));
                }
                else
                {
                    SetTimeValue('2', '4', H4.ToString("000"));
                }
            }
        }

        //异或校验
        private string xorCheack(string str)
        {
            //获取s应字节数组
            byte[] b = Encoding.ASCII.GetBytes(str);
            // xorResult 存放校验结注意：初值首元素值
            byte xorResult = b[0];
            // 求xor校验注意：XOR运算第二元素始
            for (int i = 1; i < b.Length; i++)
            {
                xorResult ^= b[i];
            }
            // 运算xorResultXOR校验结，^=为异或符号
            // MessageBox.Show();

            return str + xorResult.ToString("X");
        }
    }
}