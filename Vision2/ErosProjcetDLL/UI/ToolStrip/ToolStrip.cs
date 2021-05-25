using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Vision2.ErosProjcetDLL.UI.ToolStrip
{

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripNumericUpDown : ToolStripControlHost
    {
        public ToolStripNumericUpDown() : base(new NumericUpDown())
        {

        }
        public NumericUpDown GetBase()
        {
            return this.Control as NumericUpDown;
        }
    }
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripCheckbox : ToolStripControlHost
    {
        public ToolStripCheckbox() : base(new CheckBox())
        {

        }
        public CheckBox GetBase()
        {
            return this.Control as CheckBox;
        }
    }
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripPictureBox : ToolStripControlHost
    {
        public ToolStripPictureBox() : base(new PictureBox())
        {

        }
        public PictureBox GetBase()
        {
            return this.Control as PictureBox;
        }
    }
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripTrackBar : ToolStripControlHost
    {
        public ToolStripTrackBar() : base(new TrackBar())
        {

        }
        public TrackBar GetBase()
        {
            return this.Control as TrackBar;
        }
    }
}
