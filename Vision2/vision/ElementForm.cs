using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.vision
{
    public partial class ElementForm : Form
    {
        public ElementForm()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.panel1.Controls.Clear();
                foreach (var item in Vision.GetHimageList())
                {
                    foreach (var itemtime in item.Value.GetRunProgram())
                    {
                        elementControl elementControl = new elementControl();
                        elementControl.Dock = DockStyle.Top;
                        this.panel1.Controls.Add(elementControl);
                        elementControl.Updata(itemtime.Value);
                    } 
                }
            }
            catch (Exception)
            {
            }
        }

        private void ElementForm_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            try
            {
      //          panel1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
      //| System.Reflection.BindingFlags.NonPublic).SetValue(panel1, true, null);
            }
            catch (Exception)
            {

            }

            panel1.AutoScroll = true;


        }
        bool foldT;
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                foldT = foldT == true ? false:true;   
                foreach (var item in this.panel1.Controls)
                {
                    elementControl elementControl =item as elementControl;
                    elementControl.Fold(foldT);
                }

            }
            catch (Exception)
            {
            }
        }
    }
}
