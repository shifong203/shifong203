using System;
using System.Windows.Forms;

namespace Vision2.EPSON_Robot_Remote_TCPIP
{
    public partial class EPSONRobot : Form
    {
        public EPSONRobot()
        {
            InitializeComponent();
        }

        //private RCAPINet.Spel m_spel = new RCAPINet.Spel();

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ////m_spel = new RCAPINet.Spel();

                //m_spel.Initialize();
                //OpenFileDialog openFileDialog = new OpenFileDialog();
                //if (Directory.Exists("d:\\EpsonRC70\\projects\\API_Demos\\Demo1"))
                //{
                //    openFileDialog.InitialDirectory = "d:\\EpsonRC70\\projects\\API_Demos\\Demo1";
                //}
                //else
                //{
                //}
                //openFileDialog.ShowDialog();
                //if (openFileDialog.FileName != "")
                //{
                //    m_spel.Project = openFileDialog.FileName;
                //}
                //else
                //{
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}