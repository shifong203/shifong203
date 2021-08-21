using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class VisionContainerControl : UserControl
    {
        public VisionContainerControl()
        {
            InitializeComponent();
        }

        private VisionContainer vision1;

        public VisionContainerControl(VisionContainer vision) : this()
        {
            try
            {
                this.Disposed += VisionContainerControl_Disposed;
                listBox1.ContextMenuStrip = vision.GetNewPrajetContextMenuStrip("");
                vision.UpHalconRunProgram += Vision_UpHalconRunProgram;
                vision1 = vision;
                listBox1.Items.Clear();
                foreach (var item in vision1.ListRunName)
                {
                    listBox1.Items.Add(item.Key);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void VisionContainerControl_Disposed(object sender, EventArgs e)
        {
            try
            {
                vision1.UpHalconRunProgram -= Vision_UpHalconRunProgram;
            }
            catch (Exception) { }
        }

        private void Vision_UpHalconRunProgram(HalconRun halcon, RunProgram run)
        {
            try
            {
                listBox1.Items.Clear();
                foreach (var item in vision1.ListRunName)
                {
                    listBox1.Items.Add(item.Key);
                }
            }
            catch (Exception) { }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    return;
                }
                if (vision1.GetRunProgram().ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    RunProgram runProgram = vision1.GetRunProgram()[listBox1.SelectedItem.ToString()];
                    Control control = runProgram.GetThisControl();
                    groupBox2.Controls.Clear();
                    groupBox2.Controls.Add(control);
                    control.Dock = DockStyle.Fill;
                }
            }
            catch (Exception)
            {
            }
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    return;
                }
                List<string> names = new List<string>();
                names.Add(listBox1.SelectedItem.ToString());
                listBox1.ContextMenuStrip.Items["同步到库"].Tag = names;
                listBox1.ContextMenuStrip.Items["删除"].Tag = names;
            }
            catch (Exception ex)
            {
            }
        }
    }
}