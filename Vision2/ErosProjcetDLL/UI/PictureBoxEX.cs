using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.UI
{
    public class PictureBoxEX : PictureBox
    {


        public override string Text { get; set; }


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Graphics gfx = this.CreateGraphics();
            //float x = 15.0F;
            //float y = 15.0F;
            //float width = 20.0F;
            //float height = 50.0F;
            //RectangleF drawRect = new RectangleF(x, y, width, height);
            //StringFormat drawFormat = new StringFormat();
            //drawFormat.Alignment = StringAlignment.Center;
            //Font font = new Font(this.Font, this.Font.Style);
            SolidBrush brush = new SolidBrush(Color.Green);
            //gfx.DrawString(Text+"23fewfwe", font, brush, drawRect, drawFormat);
            gfx.SmoothingMode = SmoothingMode.HighQuality;
            gfx.DrawString("wetsfsfdsdddass", new Font("Arial ", 10, FontStyle.Bold), brush, new PointF(20, 30));
        }

    }
}
