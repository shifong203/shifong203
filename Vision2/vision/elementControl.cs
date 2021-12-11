using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision
{
    public partial class elementControl : UserControl
    {
        public elementControl()
        {
            InitializeComponent();
            height = Height;
        }
        int height;
        public object Data;
        bool foldt;
        public void Updata(RunProgram data)
        {
            propertyGrid1.SelectedObject = data;
            RunProgram runProgram = data as RunProgram;
            toolStripButton1.Text = runProgram.Name;
        }
        public void Fold(bool fold)
        {
            foldt = fold;
            if (foldt)
            {
                this.toolStripButton1.Image = global::Vision2.Properties.Resources.back_vector;
                this.Height = toolStrip1.Height;
            }
            else
            {
                this.toolStripButton1.Image = global::Vision2.Properties.Resources.drop_down_vector;
                this.Height = height;
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (foldt)
            {
                Fold(false);
            }
            else
            {
                Fold(true);
            } 
       
        }
    }
}
