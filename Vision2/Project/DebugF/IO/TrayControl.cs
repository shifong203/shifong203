using System.Windows.Forms;
using static ErosSocket.DebugPLC.Robot.TrayRobot;

namespace Vision2.Project.DebugF.IO
{
    public partial class TrayControl : UserControl
    {
        public TrayControl()
        {
            InitializeComponent();
        }
        RichTextBox richTextBox;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            richTextBox = new RichTextBox();
            this.Parent.Controls.Add(richTextBox);
            richTextBox.Location = this.Location;
            richTextBox.Text = label1.Text;
            richTextBox.BringToFront();
            richTextBox.Show();

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            richTextBox.Dispose();
        }

        private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {

                //DataObj dataObj = this.Tag as DataObj;

                //if (dataObj!=null)
                //{
                //    if (checkBox1.Checked)
                //    {

                //    }
                //    dataObj.OK = checkBox1.Checked;
                //}

            }
            catch (System.Exception)
            {
            }
        }
    }
}
