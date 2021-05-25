//using System.Drawing;

//namespace ErosSocket.ErosUI
//{/// <summary>
///// 移动控件
///// </summary>
//    public class UIMoveKnob
//    {
//        private System.Windows.Forms.Control _Owner;
//        private int _MouseClickAtX;
//        private int _MouseClickAtY;
//        private bool _BeginDrag;

//        public UIMoveKnob(System.Windows.Forms.Control Owner)
//        {
//            this._Owner = Owner;

//            this._Owner.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Owner_MouseDown);
//            this._Owner.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Owner_MouseMove);
//            this._Owner.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Owner_MouseUp);
//        }

//        private void Owner_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            this._Owner.Cursor = System.Windows.Forms.Cursors.Default;
//            this._MouseClickAtX = e.X;
//            this._MouseClickAtY = e.Y;
//            this._BeginDrag = true;
//        }

//        private void Owner_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            try
//            {
//                if (this._BeginDrag)
//                {
//                    Rectangle rect;

//                    /*
//                     * 对于下列控件,是不能拖动的,所以这里也不绘制拖动边框
//                     * TabPage,
//                     */
//                    if (this._Owner is System.Windows.Forms.TabPage)
//                    {
//                        //
//                    }
//                    else
//                    {
//                        this._Owner.Location = new Point(this._Owner.Left + e.X - this._MouseClickAtX, this._Owner.Top + e.Y - this._MouseClickAtY);
//                    }
//                }
//            }
//            catch { }
//        }

//        private void Owner_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            this._BeginDrag = false;
//            this._Owner.Parent.Refresh();
//        }
//    }
//}