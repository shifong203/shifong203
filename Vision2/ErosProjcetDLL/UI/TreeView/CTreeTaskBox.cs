using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace XoExpress.SeControl
{
    /// <summary>
    /// 基于树视图的任务控件
    /// </summary>
    public class CTreeTaskBox : TreeView
    {
        #region //construckor

        /// <summary>
        ///
        /// </summary>
        public CTreeTaskBox()
        {
            FullRowSelect = true;
            ShowLines = false;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer
            , true);
            this.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            this.ItemHeight = 20;
            this.ShowLines = true;
        }

        #endregion //construckor

        #region //field & property

        /// <summary>
        /// 展开按扭的大小
        /// </summary>
        private Size ExpandButtonSize = new Size(16, 16);

        private Color _ExpandButtonColor = Color.FromArgb(255, 255, 255);

        /// <summary>
        /// 展开按扭颜色
        /// </summary>
        [Description("展开按扭颜色"), Category("XOProperty")]
        public Color ExpandButtonColor
        {
            get
            {
                return _ExpandButtonColor;
            }
            set
            {
                if (_ExpandButtonColor != value)
                {
                    _ExpandButtonColor = value;
                    this.Invalidate();
                }
            }
        }

        private Color _GroupBgColor = Color.FromArgb(95, 162, 211);

        /// <summary>
        /// 分组栏背景色
        /// </summary>
        [Description("分组栏背景色"), Category("XOProperty")]
        public Color GroupBgColor
        {
            get
            {
                return _GroupBgColor;
            }
            set
            {
                if (_GroupBgColor != value)
                {
                    _GroupBgColor = value;
                    this.Invalidate();
                }
            }
        }

        private Color _GroupTitleColor = Color.FromArgb(0, 0, 0);

        /// <summary>
        /// 分组栏标题色
        /// </summary>
        [Description("分组栏标题色"), Category("XOProperty")]
        public Color GroupTitleColor
        {
            get
            {
                return _GroupTitleColor;
            }
            set
            {
                if (_GroupTitleColor != value)
                {
                    _GroupTitleColor = value;
                    this.Invalidate();
                }
            }
        }

        private Color _OverForeColor = Color.LightBlue;

        /// <summary>
        /// 鼠标悬停色
        /// </summary>
        [Description("鼠标悬停色"), Category("XOProperty")]
        public Color OverForeColor
        {
            get
            {
                return _OverForeColor;
            }
            set
            {
                if (_OverForeColor != value)
                {
                    _OverForeColor = value;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// 节点高度
        /// </summary>
        public new int ItemHeight
        {
            get
            {
                return base.ItemHeight;
            }
            set
            {
                if (base.ItemHeight != value && value >= 20)
                {
                    base.ItemHeight = value;
                    this.Invalidate();
                }
            }
        }

        #endregion //field & property

        #region //override

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            //base.OnDrawNode(e);
            DrawNodeItem(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0201)//单击
            {
                int wparam = m.LParam.ToInt32();
                Point point = new Point(
                    LOWORD(wparam),
                    HIWORD(wparam));
                //point = PointToClient(point);
                TreeNode tn = this.GetNodeAt(point);
                if (tn == null)
                {
                    base.WndProc(ref m);
                    return;
                }
                if (tn.Level == 0)
                {
                    if (tn.IsExpanded)
                    {
                        tn.Collapse();
                    }
                    else
                    {
                        tn.Expand();
                    }
                    m.Result = IntPtr.Zero;
                    return;
                }
                else
                {
                    base.WndProc(ref m);
                    //tn.IsSelected = true;
                    //this.SelectedNode = tn;
                }
            }
            else if (m.Msg == 0x0203)//双击
            {
                int wparam = m.LParam.ToInt32();
                Point point = new Point(
                    LOWORD(wparam),
                    HIWORD(wparam));
                //point = PointToClient(point);
                TreeNode tn = this.GetNodeAt(point);
                if (tn == null)
                {
                    base.WndProc(ref m);
                    return;
                }
                if (tn.Level == 0)
                {
                    m.Result = IntPtr.Zero;
                    return;
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            else if (m.Msg == 0x0200)//鼠标移动
            {
                try
                {
                    int wparam = m.LParam.ToInt32();
                    Point point = new Point(
                        LOWORD(wparam),
                        HIWORD(wparam));
                    //point = PointToClient(point);
                    TreeNode tn = this.GetNodeAt(point);
                    if (tn == null)
                    {
                        this.SelectedNode = null;
                        base.WndProc(ref m);
                        return;
                    }
                    this.SelectedNode = tn;
                }
                catch { }
            }
            else if (m.Msg == 0x02A3)//鼠标移出 WM_MOUSELEAVE = $02A3;
            {
                this.SelectedNode = null;
                base.WndProc(ref m);
                return;
            }
            else
            {
                base.WndProc(ref m);
            }
            //WM_LBUTTONDOWN = $0201
            //WM_LBUTTONDBLCLK = $0203;
        }

        #endregion //override

        #region //private method

        /// <summary>
        /// 自定义绘制节点
        /// </summary>
        /// <param name="e"></param>
        private void DrawNodeItem(DrawTreeNodeEventArgs e)
        {
            TreeNode tn = e.Node;
            if (tn.Level == 0)
            {
                using (Graphics g = e.Graphics)
                {
                    //绘制分组的背景
                    RenderBackgroundInternalRate(g,
                        e.Bounds,
                        GroupBgColor,
                        GroupBgColor,
                        Color.FromArgb(200, 255, 255, 255),
                        0.45f,
                        true,
                        300);
                    //绘制展开按扭
                    g.FillEllipse(new SolidBrush(ExpandButtonColor), ExpandButtonBounds(e.Bounds));
                    g.DrawEllipse(new Pen(Color.LightGray), ExpandButtonBounds(e.Bounds));
                    Point p1;
                    Point p2;
                    Point p3;
                    if (tn.IsExpanded)
                    {
                        p1 = new Point(ExpandButtonBounds(e.Bounds).X + 3, ExpandButtonBounds(e.Bounds).Bottom - 4);
                        p2 = new Point(ExpandButtonBounds(e.Bounds).X + (ExpandButtonSize.Width) / 2, ExpandButtonBounds(e.Bounds).Top + 5);
                        p3 = new Point(ExpandButtonBounds(e.Bounds).Right - 3, ExpandButtonBounds(e.Bounds).Bottom - 4);
                    }
                    else
                    {
                        p1 = new Point(ExpandButtonBounds(e.Bounds).X + 3, ExpandButtonBounds(e.Bounds).Y + 4);
                        p2 = new Point(ExpandButtonBounds(e.Bounds).X + (ExpandButtonSize.Width) / 2, ExpandButtonBounds(e.Bounds).Bottom - 5);
                        p3 = new Point(ExpandButtonBounds(e.Bounds).Right - 3, ExpandButtonBounds(e.Bounds).Y + 4);
                    }
                    GraphicsPath gp = new GraphicsPath();
                    gp.AddLine(p1, p2);
                    gp.AddLine(p2, p3);
                    g.DrawPath(new Pen(Color.FromArgb(255, 150, 0, 0), 2f), gp);

                    //绘制分组的文本
                    TextRenderer.DrawText(g, e.Node.Text, this.Font, GroupTitleBounds(e.Bounds), this.GroupTitleColor,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                }
            }
            else if (tn.Level == 1)
            {
                //e.DrawDefault = true;

                using (Graphics g = e.Graphics)
                {
                    if (tn.IsSelected)
                    {
                        TextRenderer.DrawText(g, e.Node.Text, new Font(this.Font.FontFamily.Name, this.Font.Size, FontStyle.Underline), e.Bounds, OverForeColor,
                                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                    }
                    else
                    {
                        TextRenderer.DrawText(g, e.Node.Text, this.Font, e.Bounds, this.ForeColor,
                                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                    }
                }
            }
        }

        /// <summary>
        /// 展开按扭区域
        /// </summary>
        /// <param name="childRect"></param>
        /// <returns></returns>
        private Rectangle ExpandButtonBounds(Rectangle childRect)
        {
            Rectangle lrect = new Rectangle(new Point(childRect.Right - ExpandButtonSize.Width * 3 / 2, (childRect.Height - ExpandButtonSize.Height) / 2 + childRect.Top), ExpandButtonSize);
            return lrect;
        }

        /// <summary>
        /// 取得分组标题绘制空间
        /// </summary>
        /// <param name="childRect"></param>
        /// <returns></returns>
        private Rectangle GroupTitleBounds(Rectangle childRect)
        {
            Rectangle lrect = childRect;
            lrect.Width -= ExpandButtonSize.Width * 3 / 2 + 20;
            lrect.Offset(20, 0);
            return lrect;
        }

        #endregion //private method

        #region //draw

        internal void RenderBackgroundInternal(
   Graphics g,
   Rectangle rect,
   Color baseColor,
   Color borderColor,
   Color innerBorderColor,
   float basePosition,
   bool drawBorder,
   LinearGradientMode mode)
        {
            if (drawBorder)
            {
                rect.Width--;
                rect.Height--;
            }
            using (LinearGradientBrush brush = new LinearGradientBrush(
               rect, Color.Transparent, Color.Transparent, mode))
            {
                Color[] colors = new Color[4];
                colors[0] = GetColor(baseColor, 0, 35, 24, 9);
                colors[1] = GetColor(baseColor, 0, 13, 8, 3);
                colors[2] = baseColor;
                colors[3] = GetColor(baseColor, 0, 68, 69, 54);
                ColorBlend blend = new ColorBlend();
                blend.Positions = new float[] { 0.0f, basePosition, basePosition + 0.05f, 1.0f };
                blend.Colors = colors;
                brush.InterpolationColors = blend;
                g.FillRectangle(brush, rect);
            }
            if (baseColor.A > 80)
            {
                Rectangle rectTop = rect;
                if (mode == LinearGradientMode.Vertical)
                {
                    rectTop.Height = (int)(rectTop.Height * basePosition);
                }
                else
                {
                    rectTop.Width = (int)(rect.Width * basePosition);
                }
                using (SolidBrush brushAlpha =
                    new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
                {
                    g.FillRectangle(brushAlpha, rectTop);
                }
            }
            if (drawBorder)
            {
                using (Pen pen = new Pen(borderColor))
                {
                    g.DrawRectangle(pen, rect);
                }
                rect.Inflate(-1, -1);
                using (Pen pen = new Pen(innerBorderColor))
                {
                    g.DrawRectangle(pen, rect);
                }
            }
        }

        internal void RenderBackgroundInternalRate(
   Graphics g,
   Rectangle rect,
   Color baseColor,
   Color borderColor,
   Color innerBorderColor,
   float basePosition,
   bool drawBorder, float rate)
        {
            if (drawBorder)
            {
                rect.Width--;
                rect.Height--;
            }
            using (LinearGradientBrush brush = new LinearGradientBrush(
               rect, Color.Transparent, Color.Transparent, rate))
            {
                Color[] colors = new Color[4];
                colors[0] = GetColor(baseColor, 0, 35, 24, 9);
                colors[1] = GetColor(baseColor, 0, 13, 8, 3);
                colors[2] = baseColor;
                colors[3] = GetColor(baseColor, 0, 68, 69, 54);
                ColorBlend blend = new ColorBlend();
                blend.Positions = new float[] { 0.0f, basePosition, basePosition + 0.05f, 1.0f };
                blend.Colors = colors;
                brush.InterpolationColors = blend;
                g.FillRectangle(brush, rect);
            }
            if (baseColor.A > 80)
            {
                Rectangle rectTop = rect;
                if (true/*mode == LinearGradientMode.Vertical*/)
                {
                    rectTop.Height = (int)(rectTop.Height * basePosition);
                }
                //else
                //{
                //    rectTop.Width = (int)(rect.Width * basePosition);
                //}
                using (SolidBrush brushAlpha =
                    new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
                {
                    g.FillRectangle(brushAlpha, rectTop);
                }
            }
            if (drawBorder)
            {
                using (Pen pen = new Pen(borderColor))
                {
                    g.DrawRectangle(pen, rect);
                }
                rect.Inflate(-1, -1);
                using (Pen pen = new Pen(innerBorderColor))
                {
                    g.DrawRectangle(pen, rect);
                }
            }
        }

        private Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int a0 = colorBase.A;
            int r0 = colorBase.R;
            int g0 = colorBase.G;
            int b0 = colorBase.B;
            if (a + a0 > 255) { a = 255; } else { a = a + a0; }
            if (r + r0 > 255) { r = 255; } else { r = r + r0; }
            if (g + g0 > 255) { g = 255; } else { g = g + g0; }
            if (b + b0 > 255) { b = 255; } else { b = b + b0; }
            return Color.FromArgb(a, r, g, b);
        }

        private Color GetColor2(Color colorBase, int a, int r, int g, int b)
        {
            int a0 = colorBase.A;
            int r0 = colorBase.R;
            int g0 = colorBase.G;
            int b0 = colorBase.B;
            if (a + a0 > 255) { a = 255; } else { a = a + a0; }
            if (r0 - r < 0) { r = 0; } else { r = r0 - r; }
            if (g0 - g < 0) { g = 0; } else { g = g0 - g; }
            if (b0 - b < 0) { b = 0; } else { b = b0 - b; }
            return Color.FromArgb(a, r, g, b);
        }

        #endregion //draw

        public static int LOWORD(int value)
        {
            return value & 0xFFFF;
        }

        public static int HIWORD(int value)
        {
            return value >> 16;
        }
    }
}