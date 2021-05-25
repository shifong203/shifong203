using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;


namespace ErosProjcetDLL.Project
{
    public partial class UserControl2 : System.Windows.Forms.UserControl
    {
        public UserControl2()
        {
            InitializeComponent();
            Name = "UserInterface";

        }
        public void Up()
        {

            if (ProjectINI.Enbt)
            {
                if (!tabControl1.TabPages.Contains(tabPage1))
                {
                    tabControl1.TabPages.Add(tabPage1);
                }
                //projectNodetControl1.UpProject();
            }
            else
            {
                tabControl1.TabPages.Remove(tabPage1);
            }
    
        }

        private void tabControl1_ControlAdded(object sender, ControlEventArgs e)
        {
            try
            {
                foreach (TabPage item in tabControl1.TabPages)
                {
                    item.AutoScroll = true;
                    //    item = new System.Drawing.Font("Microsoft YaHei UI", 12F);
                }
            }
            catch (Exception)
            {

            }
           
        }
    }
}