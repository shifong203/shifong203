using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.UI.ToolStrip
{
    public class TSButton : ToolStripButton
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.DrawRectangle(new Pen(borderColor), 0, 0, this.Width - 1, this.Height - 1);
        }
        [DisplayName("是否显示选择项")]
        public bool IsCher
        { get; set; }
        private Color borderColor = Color.Black;

        /// <summary>
        /// 边框颜色
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; base.Invalidate(); }
        }
    }
}