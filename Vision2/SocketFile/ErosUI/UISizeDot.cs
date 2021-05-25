using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ErosSocket.ErosUI
{
    /// <summary>
    /// 开发自定义的调整手柄控件：
    /// </summary>
    [ToolboxItem(false)]
    public partial class UISizeDot : Control
    {
        private bool _movable;
        private Pen pen = new Pen(Color.Black);

        public UISizeDot()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.TabStop = false;
            this._movable = true;
        }

        /// <summary>
        /// UISizeDot的边框颜色
        /// </summary>
        public Color BorderColor
        {
            get { return pen.Color; }
            set
            {
                this.pen = new Pen(value);
                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: 在此处添加自定义绘制代码
            //this.BackColor = Color.White;
            pe.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);

            // 调用基类 OnPaint
            base.OnPaint(pe);
        }

        public bool Movable
        {
            get { return this._movable; }
            set { this._movable = value; }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public enum ENUM_UISizeMode
    {
        FixNone = 0,       //不固定
        FixLocation = 1,   //固定左上角，这时只能改变两边
        FixHeight = 2,     //固定高
        FixWidth = 3,      //固定宽
        FixBoth = 4        //长宽都固定
    }

    /// <summary>
    ///8个选择框
    /// </summary>
    public class UISizeKnob
    {
        private const int DOT_WIDTH = 7;   //UISizeDot宽度
        private const int DOT_HEIGHT = 7;  //UISizeDot高度
        private const int DOT_SPACE = 0;   //UISizeDot与_Owner的距离
        private const int DOT_COUNT = 8;   //要显示的UISizeDot数

        private System.Windows.Forms.Control _Owner;

        private UISizeDot[] _UISizeDot;
        private int _OldTop;
        private int _OldLeft;
        private int _NewTop;
        private int _NewLeft;
        private int _OldWidth;
        private int _OldHeight;
        private int _ClickAtX;
        private int _ClickAtY;
        private ENUM_UISizeMode _UISizeMode;
        private bool _BeginDrag;
        private Rectangle _OldRect;
        private Color _DotColor = Color.White;        //UISizeDot默认颜色为白色
        private Color _DotBorderColor = Color.Black;  //UISizeDot默认边框颜色为黑色

        public event System.Windows.Forms.MouseEventHandler MouseDown = null;

        public event System.Windows.Forms.MouseEventHandler MouseMove = null;

        public event System.Windows.Forms.MouseEventHandler MouseUp = null;

        private int j = 0;
        private bool _IsShow = false;

        public UISizeKnob(System.Windows.Forms.Control owner)
        {
            this._Owner = owner;
            this._NewTop = owner.Top;
            this._NewLeft = owner.Left;
            this._OldWidth = owner.Width;
            this._OldHeight = owner.Height;

            InitUISizeDots();
        }

        public bool IsShow
        {
            get { return this._IsShow; }
        }

        public Color DotColor
        {
            get { return this._DotColor; }
            set
            {
                this._DotColor = value;
                this._DotBorderColor = Color.FromArgb(Math.Abs(Convert.ToInt32(value.R) - 255), Math.Abs(Convert.ToInt32(value.G) - 255), Math.Abs(Convert.ToInt32(value.B) - 255));
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < this._UISizeDot.Length; i++)
            {
                this._UISizeDot[i].Dispose();
            }
        }

        /// <summary>
        /// this._Owner的大小改变模式
        /// </summary>
        public ENUM_UISizeMode UISizeMode
        {
            get { return this._UISizeMode; }
            set { this._UISizeMode = value; }
        }

        private void InitUISizeDots()
        {
            this._UISizeDot = new UISizeDot[DOT_COUNT];
            for (int i = 0; i < DOT_COUNT; i++)
            {
                this._UISizeDot[i] = new UISizeDot();
                this._UISizeDot[i].Width = DOT_WIDTH;
                this._UISizeDot[i].Height = DOT_HEIGHT;
                this._UISizeDot[i].Visible = false;
                this._Owner.Parent.Controls.Add(this._UISizeDot[i]);
                this._UISizeDot[i].MouseDown += new System.Windows.Forms.MouseEventHandler(this.UISizeDot_MouseDown);
                this._UISizeDot[i].MouseMove += new System.Windows.Forms.MouseEventHandler(this.UISizeDot_MouseMove);
                this._UISizeDot[i].MouseUp += new System.Windows.Forms.MouseEventHandler(this.UISizeDot_MouseUp);
            }

            this._UISizeDot[0].Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this._UISizeDot[1].Cursor = System.Windows.Forms.Cursors.SizeNS;
            this._UISizeDot[2].Cursor = System.Windows.Forms.Cursors.SizeNESW;
            this._UISizeDot[3].Cursor = System.Windows.Forms.Cursors.SizeWE;
            this._UISizeDot[4].Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this._UISizeDot[5].Cursor = System.Windows.Forms.Cursors.SizeNS;
            this._UISizeDot[6].Cursor = System.Windows.Forms.Cursors.SizeNESW;
            this._UISizeDot[7].Cursor = System.Windows.Forms.Cursors.SizeWE;

            SetUISizeDotsPosition();
        }

        public void ShowUISizeDots(bool show)
        {
            try
            {
                this._IsShow = show;
                //2006-10-05:将此函数中所有的this._UISizeDot.Length全部替换成８
                if (show)
                {
                    SetUISizeDotsPositionByMove(false);
                }
                else
                {
                    this._Owner.Parent.SuspendLayout();
                    for (int i = 0; i < DOT_COUNT; i++)
                    {
                        this._UISizeDot[i].Visible = show;
                    }
                    this._Owner.Parent.ResumeLayout();
                    return;
                }

                if (this._UISizeMode == ENUM_UISizeMode.FixNone)
                {
                    for (int i = 0; i < DOT_COUNT; i++)
                    {
                        this._UISizeDot[i].BorderColor = this._DotBorderColor;
                        this._UISizeDot[i].BackColor = this._DotColor;
                        this._UISizeDot[i].Visible = show;
                    }
                }
                else if (this._UISizeMode == ENUM_UISizeMode.FixLocation)
                {
                    for (int i = 0; i < DOT_COUNT; i++)
                    {
                        this._UISizeDot[i].BorderColor = this._DotBorderColor;
                        this._UISizeDot[i].BackColor = this._DotColor;
                        this._UISizeDot[i].Visible = show;
                    }
                    this._UISizeDot[0].BackColor = System.Drawing.Color.FromArgb(9, 55, 119);
                    this._UISizeDot[0].Movable = false;
                    this._UISizeDot[1].BackColor = System.Drawing.Color.FromArgb(9, 55, 119);
                    this._UISizeDot[1].Movable = false;
                    this._UISizeDot[2].BackColor = System.Drawing.Color.FromArgb(9, 55, 119);
                    this._UISizeDot[2].Movable = false;
                    this._UISizeDot[6].BackColor = System.Drawing.Color.FromArgb(9, 55, 119);
                    this._UISizeDot[6].Movable = false;
                    this._UISizeDot[7].BackColor = System.Drawing.Color.FromArgb(9, 55, 119);
                    this._UISizeDot[7].Movable = false;
                }
                else if (this._UISizeMode == ENUM_UISizeMode.FixHeight)
                {
                    this._UISizeDot[0].Visible = false;
                    this._UISizeDot[1].Visible = false;
                    this._UISizeDot[2].Visible = false;

                    this._UISizeDot[3].BorderColor = this._DotBorderColor;
                    this._UISizeDot[3].BackColor = this._DotColor;
                    this._UISizeDot[3].Refresh();
                    this._UISizeDot[3].Visible = show;

                    this._UISizeDot[4].Visible = false;
                    this._UISizeDot[5].Visible = false;
                    this._UISizeDot[6].Visible = false;

                    this._UISizeDot[7].BorderColor = this._DotBorderColor;
                    this._UISizeDot[7].BackColor = this._DotColor;
                    this._UISizeDot[7].Refresh();
                    this._UISizeDot[7].Visible = show;
                }
                else if (this._UISizeMode == ENUM_UISizeMode.FixWidth)
                {
                    this._UISizeDot[0].Visible = false;

                    this._UISizeDot[1].BorderColor = this._DotBorderColor;
                    this._UISizeDot[1].BackColor = this._DotColor;
                    this._UISizeDot[1].Visible = show;
                    this._UISizeDot[1].Refresh();

                    this._UISizeDot[2].Visible = false;
                    this._UISizeDot[3].Visible = false;
                    this._UISizeDot[4].Visible = false;

                    this._UISizeDot[5].BorderColor = this._DotBorderColor;
                    this._UISizeDot[5].BackColor = this._DotColor;
                    this._UISizeDot[5].Visible = show;
                    this._UISizeDot[5].Refresh();

                    this._UISizeDot[6].Visible = false;
                    this._UISizeDot[7].Visible = false;
                }
                else if (this._UISizeMode == ENUM_UISizeMode.FixBoth)
                {
                    for (int i = 0; i < DOT_COUNT; i++)
                    {
                        this._UISizeDot[i].BorderColor = this._DotBorderColor;
                        this._UISizeDot[i].BackColor = System.Drawing.Color.FromArgb(9, 55, 119);
                        this._UISizeDot[i].Movable = false;
                        this._UISizeDot[i].Visible = show;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetUISizeDotsPosition()
        {
            int left, width, height, top;
            left = this._Owner.Left;
            top = this._Owner.Top;
            width = this._Owner.Width;
            height = this._Owner.Height;
            this._UISizeDot[0].Location = new Point(left - DOT_WIDTH - DOT_SPACE, top - DOT_HEIGHT - DOT_SPACE);
            this._UISizeDot[1].Location = new Point(left + width / 2 - DOT_WIDTH / 2, top - DOT_HEIGHT - DOT_SPACE);
            this._UISizeDot[2].Location = new Point(left + width + DOT_SPACE, top - DOT_HEIGHT - DOT_SPACE);
            this._UISizeDot[3].Location = new Point(left + width + DOT_SPACE, top + height / 2 - DOT_HEIGHT / 2);
            this._UISizeDot[4].Location = new Point(left + width + DOT_SPACE, top + height + DOT_SPACE);
            this._UISizeDot[5].Location = new Point(left + width / 2 - DOT_WIDTH / 2, top + height + DOT_SPACE);
            this._UISizeDot[6].Location = new Point(left - DOT_WIDTH - DOT_SPACE, top + height + DOT_SPACE);
            this._UISizeDot[7].Location = new Point(left - DOT_WIDTH - DOT_SPACE, top + height / 2 - DOT_HEIGHT / 2);
        }

        private void SetUISizeDotsPositionByMove(bool Show)
        {
            int left, width, height, top;
            left = this._Owner.Left;
            top = this._Owner.Top;
            width = this._Owner.Width;
            height = this._Owner.Height;

            this._UISizeDot[0].Visible = Show;
            this._UISizeDot[1].Visible = Show;
            this._UISizeDot[2].Visible = Show;
            this._UISizeDot[3].Visible = Show;
            this._UISizeDot[4].Visible = Show;
            this._UISizeDot[5].Visible = Show;
            this._UISizeDot[6].Visible = Show;
            this._UISizeDot[7].Visible = Show;

            this._UISizeDot[0].BringToFront();
            this._UISizeDot[1].BringToFront();
            this._UISizeDot[2].BringToFront();
            this._UISizeDot[3].BringToFront();
            this._UISizeDot[4].BringToFront();
            this._UISizeDot[5].BringToFront();
            this._UISizeDot[6].BringToFront();
            this._UISizeDot[7].BringToFront();

            this._UISizeDot[0].Location = new Point(left - DOT_WIDTH - DOT_SPACE, top - DOT_HEIGHT - DOT_SPACE);
            this._UISizeDot[1].Location = new Point(left + width / 2 - DOT_WIDTH / 2, top - DOT_HEIGHT - DOT_SPACE);
            this._UISizeDot[2].Location = new Point(left + width + DOT_SPACE, top - DOT_HEIGHT - DOT_SPACE);
            this._UISizeDot[3].Location = new Point(left + width + DOT_SPACE, top + height / 2 - DOT_HEIGHT / 2);
            this._UISizeDot[4].Location = new Point(left + width + DOT_SPACE, top + height + DOT_SPACE);
            this._UISizeDot[5].Location = new Point(left + width / 2 - DOT_WIDTH / 2, top + height + DOT_SPACE);
            this._UISizeDot[6].Location = new Point(left - DOT_WIDTH - DOT_SPACE, top + height + DOT_SPACE);
            this._UISizeDot[7].Location = new Point(left - DOT_WIDTH - DOT_SPACE, top + height / 2 - DOT_HEIGHT / 2);
        }

        private void UISizeDot_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!((UISizeDot)sender).Movable)
            {
                return;
            }
            j++;

            this.ShowUISizeDots(false);
            this._BeginDrag = true;
            this._ClickAtX = e.X;
            this._ClickAtY = e.Y;
            this._OldTop = this._Owner.Top;
            this._OldLeft = this._Owner.Left;
            this._NewTop = this._Owner.Top;
            this._NewLeft = this._Owner.Left;
            this._OldHeight = this._Owner.Height;
            this._OldWidth = this._Owner.Width;

            Rectangle rect = new Rectangle(this._NewLeft - 1, this._NewTop - 1, this._OldWidth + 2, this._OldHeight + 2);
            //this._Owner.Parent.CreateGraphics().DrawRectangle(new Pen(Color.Black,2),rect);
            this._OldRect = rect;
            if (this.MouseDown != null)
                this.MouseDown(sender, e);
        }

        private void UISizeDot_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!((UISizeDot)sender).Movable)
            {
                return;
            }

            if (this._BeginDrag)
            {
                int eX = e.X - this._ClickAtX;
                int eY = e.Y - this._ClickAtY;

                if (this._UISizeDot[0] == sender)
                {
                    this._Owner.Location = new System.Drawing.Point(this._NewLeft + eX, this._NewTop + eY);
                    this._Owner.Size = new System.Drawing.Size(this._Owner.Width - eX, this._Owner.Height - eY);
                }
                else if (this._UISizeDot[1] == sender)
                {
                    this._Owner.Location = new System.Drawing.Point(this._NewLeft, this._NewTop + eY);
                    this._Owner.Size = new System.Drawing.Size(this._Owner.Width, this._Owner.Height - eY);
                }
                else if (this._UISizeDot[2] == sender)
                {
                    this._Owner.Location = new System.Drawing.Point(this._NewLeft, this._NewTop + eY);
                    this._Owner.Size = new System.Drawing.Size(this._Owner.Width + eX, this._Owner.Height - eY);
                }
                else if (this._UISizeDot[3] == sender)
                {
                    this._Owner.Size = new System.Drawing.Size(this._Owner.Width + eX, this._Owner.Height);
                }
                else if (this._UISizeDot[4] == sender)
                {
                    this._Owner.Size = new System.Drawing.Size(this._Owner.Width + eX, this._Owner.Height + eY);
                }
                else if (this._UISizeDot[5] == sender)
                {
                    this._Owner.Size = new System.Drawing.Size(this._Owner.Width, this._Owner.Height + eY);
                }
                else if (this._UISizeDot[6] == sender)
                {
                    this._Owner.Location = new System.Drawing.Point(this._NewLeft + eX, this._NewTop);
                    this._Owner.Size = new System.Drawing.Size(this._Owner.Size.Width - eX, this._Owner.Height + eY);
                }
                else if (this._UISizeDot[7] == sender)
                {
                    this._Owner.Location = new System.Drawing.Point(this._NewLeft + eX, this._NewTop);
                    this._Owner.Size = new System.Drawing.Size(_Owner.Width - eX, this._Owner.Height);
                }

                this._NewTop = this._Owner.Top;
                this._NewLeft = this._Owner.Left;
                //this._OldHeight = this._Owner.Height;
                //this._OldWidth = this._Owner.Width;
                SetUISizeDotsPosition();
                this._Owner.Refresh();
                this._Owner.Parent.Refresh();

                //this._Owner.Parent.CreateGraphics().DrawRectangle(new Pen(this._Owner.BackColor,2), this._OldRect);
                Rectangle rect = new Rectangle(this._NewLeft - 1, this._NewTop - 1, this._Owner.Width + 2, this._Owner.Height + 2);
                //this._Owner.Parent.CreateGraphics().DrawRectangle(new Pen(Color.Black,2), rect);
                this._OldRect = rect;
            }

            if (this.MouseMove != null)
                this.MouseMove(sender, e);
        }

        private void UISizeDot_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!((UISizeDot)sender).Movable)
            {
                return;
            }

            Hashtable OldWidth;
            Hashtable OldHeight;
            Hashtable NewWidth;
            Hashtable NewHeight;
            //Test.UIResizeCommand ResizeCommand;
            //this._Owner.Parent.CreateGraphics().DrawRectangle(new Pen(this._Owner.BackColor,2),this._OldRect);
            this.ShowUISizeDots(true);

            //使用UIResizeCommand，这里主要是保存现场，这样可以方便实现Undo和Redo
            if (this._OldHeight != this._Owner.Height || this._OldWidth != this._Owner.Width)
            {
                OldWidth = new Hashtable();
                OldHeight = new Hashtable();
                NewWidth = new Hashtable();
                NewHeight = new Hashtable();
                OldWidth.Add(this._Owner, this._OldWidth);
                OldHeight.Add(this._Owner, this._OldHeight);
                NewWidth.Add(this._Owner, this._Owner.Width);
                NewHeight.Add(this._Owner, this._Owner.Height);
            }

            this._BeginDrag = false;
            if (this.MouseUp != null)
                this.MouseUp(sender, e);
        }
    }
}