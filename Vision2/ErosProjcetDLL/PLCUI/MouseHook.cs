using System;
using System.Drawing;

using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.PLCUI
{
    /// <summary>
    /// 鼠标事件
    /// </summary>
    public class MouseHook
    {
        private Control _Owner;
        private int _CLickAtX;
        private int _ClickAtY;
        private int _MoveAtX;
        private int _MoveAtY;
        private bool _BeginDrag;
        private bool _BeginDrawControl;

        /// <summary>
        /// 这里Owner使用的是Control类型，是因为我们不仅仅需要在Winform上增加控件，
        /// 也需要在其它容器，比如Panel,GroupBox等上面增加容器
        /// </summary>
        /// <param name="Owner"></param>
        public MouseHook(System.Windows.Forms.Control Owner)
        {
            this._Owner = Owner;
            this._Owner.MouseDown += new MouseEventHandler(this.Control_MouseDown);
            this._Owner.MouseMove += new MouseEventHandler(this.Control_MouseMove);
            this._Owner.MouseUp += new MouseEventHandler(this.Control_MouseUp);
            this._Owner.MouseEnter += new EventHandler(this.Control_MouseEnter);
            this._BeginDrawControl = false;
        }

        #region Control上的鼠标事件

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            //如果没有选择控件，那么退出
            if (SettingService.Instance.SelectedToolBoxControl != null)
            {
                this._CLickAtX = e.X;
                this._ClickAtY = e.Y;
                this._MoveAtX = e.X;
                this._MoveAtY = e.Y;
                this._BeginDrag = true;

                if (SettingService.Instance.SelectedToolBoxControl != null)
                {
                    this._BeginDrawControl = true;
                }
                else
                {
                    this._BeginDrawControl = false;
                }
            }
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (SettingService.Instance.SelectedToolBoxControl == null)
            {
                return;
            }

            if (this._BeginDrag)
            {
                //取消上次绘制的选择框
                int iLeft, iTop, iWidth, iHeight;
                Pen pen;
                Rectangle rect;

                pen = new Pen(this._Owner.BackColor);
                if (this._BeginDrawControl == true)
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    pen.Width = 2;
                }
                else
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                }

                iLeft = this._CLickAtX < this._MoveAtX ? this._CLickAtX : this._MoveAtX;
                iTop = this._ClickAtY < this._MoveAtY ? this._ClickAtY : this._MoveAtY;
                iWidth = Math.Abs(this._MoveAtX - this._CLickAtX);
                iHeight = Math.Abs(this._MoveAtY - this._ClickAtY);

                rect = new Rectangle(iLeft, iTop, iWidth, iHeight);
                this._Owner.CreateGraphics().DrawRectangle(pen, rect);

                //重新绘制选择框
                this._MoveAtX = e.X;
                this._MoveAtY = e.Y;
                pen = new Pen(Color.Black);
                if (this._BeginDrawControl == true)
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    pen.Width = 2;
                }
                else
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                }
                iLeft = this._CLickAtX < this._MoveAtX ? this._CLickAtX : this._MoveAtX;
                iTop = this._ClickAtY < this._MoveAtY ? this._ClickAtY : this._MoveAtY;
                iWidth = Math.Abs(this._MoveAtX - this._CLickAtX);
                iHeight = Math.Abs(this._MoveAtY - this._ClickAtY);

                rect = new Rectangle(iLeft, iTop, iWidth, iHeight);
                this._Owner.CreateGraphics().DrawRectangle(pen, rect);
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            this._BeginDrag = false;
            this._Owner.SuspendLayout();

            if (SettingService.Instance.SelectedToolBoxControl == null)
            {
                return;
            }
            //取消上次绘制的选择框
            int iLeft, iTop, iWidth, iHeight;
            Pen pen;
            Rectangle rect;
            pen = new Pen(this._Owner.BackColor);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            iLeft = this._CLickAtX < this._MoveAtX ? this._CLickAtX : this._MoveAtX;
            iTop = this._ClickAtY < this._MoveAtY ? this._ClickAtY : this._MoveAtY;
            iWidth = Math.Abs(this._MoveAtX - this._CLickAtX);
            iHeight = Math.Abs(this._MoveAtY - this._ClickAtY);
            rect = new Rectangle(iLeft, iTop, iWidth, iHeight);
            this._Owner.CreateGraphics().DrawRectangle(pen, rect);

            if (SettingService.Instance.SelectedToolBoxControl != null)
            {
                AddControl(SettingService.Instance.SelectedToolBoxControl, rect);
            }
            else
            {
                //这里是拖动鼠标，选择控件，这里将会在后续的介绍中给出
            }
            this._Owner.Refresh();
            this._Owner.ResumeLayout();
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
            if (SettingService.Instance.SelectedToolBoxControl != null)
            {
                this._Owner.Cursor = Cursors.Cross;
            }
            else
            {
                this._Owner.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="rect"></param>
        private void AddControl(System.Windows.Forms.Control control, Rectangle rect)
        {
            try
            {
                control.Location = rect.Location;
                control.Size = rect.Size;
                control.Name = GetControlName(control);
                //因为对于DataTimePiker控件来说不能设置.Text为非日期型，所以忽略错误
                try
                {
                    control.Text = GetControlType(control);
                }
                catch { }

                this._Owner.Controls.Add(control);
                control.Visible = true;

                this._Owner.Cursor = Cursors.Default;
                SettingService.Instance.SelectedToolBoxControl = null;
            }
            catch (Exception e)
            {
                this._Owner.Cursor = Cursors.Default;
                SettingService.Instance.SelectedToolBoxControl = null;
            }
        }

        /// <summary>
        /// 控件类型
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private string GetControlType(System.Windows.Forms.Control ctrl)
        {
            string strType = ctrl.GetType().ToString();
            string strControlType;
            string[] strArr = strType.Split(".".ToCharArray());

            strControlType = strArr[strArr.Length - 1].Trim();

            return strControlType;
        }

        private string GetControlName(System.Windows.Forms.Control control)
        {
            //这里简单返回控件名，如果需要，可以通过修改这个函数做特殊处理
            return control.GetType().Name;
        }

        #endregion Control上的鼠标事件
    }
}