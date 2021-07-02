using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.vision.Cams
{
    public partial class LightSourceControl : UserControl
    {
        public LightSourceControl()
        {
            InitializeComponent();
        }
        public void GetData()
        {
            try
            {
                iscont = true;
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
            catch (Exception)
            {
            }
            iscont = false;
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                SetP();
            }
            catch (Exception)
            {
            }
        }
        bool iscont = false;
        LightSource lightSource;
        void SetP()
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

    
    }
    public class LightSource
    {
        SerialPortHelper serialPort;
        public string OffName = "浮根";
        public string Rs232Name = "COM0";
        public LightSource(int com)
        {
            Rs232Name = "COM" + com;
            serialPort = new SerialPortHelper(com, 15600, out string errr);
        }
        public LightSource()
        {
            serialPort = new SerialPortHelper(0, 15600, out string errr);
        }
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
        public void SetHx()
        {
            if (OffName == "浮根")
            {
                serialPort.Parity = Parity.None;
                serialPort.BaudRate = 19200;
                serialPort.StopBits = StopBits.One;
            }
            else
            {
                serialPort.Parity = Parity.None;
                serialPort.BaudRate = 9600;
                serialPort.StopBits = StopBits.One;
            }
            if (!serialPort.IsOpen)
            {
                serialPort.PortName = Rs232Name;
                serialPort.Open();
            }
            if (OffName == "浮根")
            {
                serialPort.Write(CheckChData() + "C#");
            }
            else
            {
                CheckChKWData();
            }
        }
        public void SetOFF()
        {
            if (!serialPort.IsOpen)
            {
                serialPort.PortName = Rs232Name;
                serialPort.Open();
            }
            serialPort.Write(CheckChData() + "C#");
        }

        [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("1通道")]
        public byte H1 { get; set; } = 255;

        [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("2通道")]
        public byte H2 { get; set; } = 255;

        [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("3通道")]
        public byte H3 { get; set; } = 255;

        [DescriptionAttribute("光源控制器1通道。"), Category("光源控制"), DisplayName("4通道")]
        public byte H4 { get; set; } = 255;

        public bool H1Off;

        public bool H2Off;

        public bool H3Off;

        public bool H4Off;

    }
}
