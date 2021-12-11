using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class ErrForm : Form
    {
        public ErrForm()
        {
            InitializeComponent();
        }
        public ErrForm(Exception exception, string mesgtxt = null):this()
        {
            ShowErr(exception, mesgtxt);
        }
        public void ShowErr(Exception exception, string mesgtxt = null)
        {
            this.richTextBox2.Text = "";
            if (mesgtxt!=null)
            {
                this.richTextBox2.Text = mesgtxt + Environment.NewLine;
            }
         
            this.richTextBox2.Text += exception.Message;
            this.richTextBox1.Text = exception.StackTrace;
            this.ShowDialog();
        }
        public static void Show(Exception exception,string mesgtxt=null)
        {
            try
            {
                ErrForm errForm = new ErrForm();

                errForm.ShowErr(exception, mesgtxt);
            }
            catch (Exception)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
