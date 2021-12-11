using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.UI
{
    public class PanelEx : Panel
    {
        public PanelEx()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);   //   禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true);
        }
    }
}
