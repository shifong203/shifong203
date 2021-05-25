using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NokidaE.vision
{
    public partial class UserVisionManagement : UserControl
    {
        public UserVisionManagement()
        {
            InitializeComponent();
            This = this;
        }

        public static UserVisionManagement This { get
            {
                if (thisU==null)
                {
                    thisU = new UserVisionManagement();
                }
                return thisU;
            } set { thisU = value; } }
            static UserVisionManagement thisU;
    }
}
