using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.UI.ToolStrip
{
    public class LabelEx : Label
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (ISBorder)
            {
                Graphics g = e.Graphics;
                g.DrawRectangle(new Pen(borderColor, BordecWith), 0, 0, this.Width - BordecWith, this.Height - BordecWith);
            }
        }
        [DisplayName("边框宽度"), Category("边框")]
        public int BordecWith { get; set; } = 5;

        [DisplayName("是否显示选择项"), Category("边框")]
        public bool ISBorder
        { get; set; }

        private Color borderColor = Color.Blue;

        /// <summary>
        /// 边框颜色
        /// </summary>
        [DefaultValue(typeof(Color), "Black"), DisplayName("边框颜色"), Category("边框")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; base.Invalidate(); }
        }
    }
}