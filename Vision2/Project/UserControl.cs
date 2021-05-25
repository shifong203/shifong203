using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using ErosProjcetDLL.Project;

namespace NokidaE.Project
{
    public partial class UserControl2 : System.Windows.Forms.UserControl
    {
        public UserControl2()
        {
            InitializeComponent();
            Name = "UserInterface";
        }
        public void Up(Control control=null)
        {

            if (ProjectINI.AdminEnbt)
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
         projectNodetControl1.   UpProject();
            if (control==null)
            {
                return;
            }
            if (tabControl1.TabPages.IndexOfKey(control.Text)==-1)
            {
                TabPage tabPage = new TabPage();
                tabPage.Name = tabPage.Text = control.Name;
                control.Dock = DockStyle.Fill;
                tabControl1.TabPages.Add(tabPage);
                    tabPage.Controls.Add(control);
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