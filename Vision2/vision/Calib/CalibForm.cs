using System.Windows.Forms;

namespace Vision2.vision.Calib
{
    public partial class CalibForm : Form
    {
        public CalibForm()
        {
            InitializeComponent();
            ThisForm = this;
        }

        public static CalibForm ThisForm = new CalibForm();

    }
}
