using System.Windows.Forms;

namespace ErosSocket.DebugPLC.PLC
{
    public partial class PLCIOUserControl1 : UserControl
    {
        public PLCIOUserControl1(PLCIO pIO)
        {
            InitializeComponent();
            pLCIO = pIO;
        }
        PLCIO pLCIO;

    }
}
