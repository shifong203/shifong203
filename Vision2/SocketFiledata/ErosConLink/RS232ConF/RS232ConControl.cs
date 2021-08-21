using System;
using System.Windows.Forms;

namespace ErosSocket.ErosConLink.RS232ConF
{
    public partial class RS232ConControl : UserControl
    {
        public RS232ConControl()
        {
            InitializeComponent();
        }

        public RS232ConControl(RS232Con rS232) : this()
        {
        }

        private void RS232ConControl_Load(object sender, EventArgs e)
        {
        }
    }
}