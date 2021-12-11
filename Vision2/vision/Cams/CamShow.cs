using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.vision.Cams
{
    public partial class CamShow : UserControl
    {
        public CamShow()
        {
            InitializeComponent();
        }
        public   Camera Camera;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Camera.Stop();
            }
            catch (Exception)
            {

            }  
        }
        public void Straing()
        {
            try
            {
                Camera.Straing(hWindowControl1);
            }
            catch (Exception)
            {

            }
        }
        public void Stop()
        {
            try
            {
                Camera.Stop();
            }
            catch (Exception)
            {

            }
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                Straing();
            }
            catch (Exception)
            {

            }
        }

        private void CamShow_Load(object sender, EventArgs e)
        {
            try
            {
              toolStripLabel1.Text=     Camera.Name;
            }
            catch (Exception)
            {

            }
        }
    }
}
