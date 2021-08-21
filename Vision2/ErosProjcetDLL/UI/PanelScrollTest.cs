using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.UI
{
    public class PanelWithoutAutoScroll : Panel
    {
        protected override System.Drawing.Point ScrollToControl(Control activeControl)
        {
            return DisplayRectangle.Location;
        }
    }
}