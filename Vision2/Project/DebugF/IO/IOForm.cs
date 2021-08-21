using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.Project.DebugF.IO
{
    public partial class IOForm : Form
    {
        public IOForm()
        {
            InitializeComponent();
        }

        private void IOForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.didoUserControl1.setDODI(DebugCompiler.GetDoDi());
            }
            catch (Exception)
            {
            }
        }
    }
}