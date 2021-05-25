using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.PLCUI;
using Vision2.ErosProjcetDLL.Project;

namespace Vision2.ErosUI
{
    public class UICon
    {

        public static void DragDropConrot(Control control)
        {
            control.AllowDrop = true;
            control.DragDrop += new System.Windows.Forms.DragEventHandler(ToolForm.UIForm.Control_DragDrop);
            control.DragOver += new System.Windows.Forms.DragEventHandler(ToolForm.UIForm.Control_DragOver);
        }
        Control ThisControl;

        /// <summary>
        /// 引用并添加事件
        /// </summary>
        /// <param name="control"></param>
        public UICon(Control control)
        {
            ThisControl = control;
            ThisControl.KeyDown += ErosNewFrom_KeyDown;
            ThisControl.MouseDown += ErosNewFrom_MouseDown;
            ThisControl.MouseMove += ErosNewFrom_MouseMove;
            ThisControl.MouseUp += ErosNewFrom_MouseUp;
            ThisControl.Paint += Form1_Paint;
            this._HashUISizeKnob = new Hashtable();
            this._MouseHook = new MouseHook(control);

            this._HashUIMoveKnob = new Hashtable();

            //为了简洁明了，我们在ControlAdded中来设置具体控件和UISizeKnob的关联
            control.ControlAdded += new ControlEventHandler(Form1_ControlAdded);
        }

        private void Form1_ControlAdded(object sender, ControlEventArgs e)
        {
            if (!(e.Control is UISizeDot))
            {
                System.Windows.Forms.UserControl userControl = e.Control as System.Windows.Forms.UserControl;

                this._HashUISizeKnob.Add(e.Control, new UISizeKnob(e.Control));
                this._HashUIMoveKnob.Add(e.Control, new UIMoveKnob(e.Control));
                //点击控件的时候，显示控件的选择
                e.Control.Click += new EventHandler(Control_Click);
            }
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            Panel panel = (Panel)sender;
            if (panel != null)
            {
            }
        }
        private void Control_Click(object sender, EventArgs e)
        {
            try
            {
                if (isShift)
                {
                }
                else
                {
                    foreach (UISizeKnob knob in this._HashUISizeKnob.Values)
                    {
                        knob.ShowUISizeDots(false);
                    }
                    ListUi.Clear();
                }
                ((UISizeKnob)this._HashUISizeKnob[sender]).ShowUISizeDots(true);
                ListUi.Add((Control)sender);
                //我这里仅仅做TextBox的属性演示，如果是其它的控件的话，那么你需要设计不同的ControlProperty（比如TextBoxProperty，ComboBoxProperty）
                if (sender is TextBox)
                {
                    //this.propertyGrid1.SelectedObject = new TextBoxProperty((TextBox)sender);
                }
                else
                {
                    //this.propertyGrid1.SelectedObject = sender;
                }
                Vision2.ErosProjcetDLL.Project.PropertyForm.UPProperty(sender);
            }
            catch { }
        }

        private Hashtable _HashUISizeKnob;
        private Control _Owner = new Control();

        private Point downPos = new Point(0, 0);
        private Graphics g = null;
        private bool isDown = false;
        private Point upPos = new Point(100, 100);

        private bool isShift;
        /// <summary>
        /// 大小事件
        /// </summary>
        private MouseHook _MouseHook;

        ///<summary>负责控件移动的类</summary>
        private Hashtable _HashUIMoveKnob;

        private List<Control> ListUi = new List<Control>();

        private void ErosNewFrom_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                isShift = false;
                if (e.KeyCode == Keys.Delete)
                {
                    foreach (var item in ListUi)
                    {
                        ListUi.Remove(item);
                        ((UISizeKnob)this._HashUISizeKnob[item]).ShowUISizeDots(false);
                        ThisControl.Controls.Remove(item);
                        this._HashUISizeKnob.Remove(item);
                        this._HashUIMoveKnob.Remove(item);
                        item.Dispose();
                    }
                }
                else if (e.KeyCode == Keys.ShiftKey)
                {
                    isShift = true;
                }
                else
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private void ErosNewFrom_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //如果没有选择控件，那么退出
                g = g ?? ThisControl.CreateGraphics();
                downPos = e.Location;
                isDown = true;
            }
            catch (Exception)
            {
            }
        }

        private void ErosNewFrom_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDown)
            {
                upPos = e.Location;
                ThisControl.Invalidate();
            }
        }

        private void ErosNewFrom_MouseUp(object sender, MouseEventArgs e)
        {
            ThisControl.Invalidate();
            upPos = e.Location;
            isDown = false;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (g == null)
                return;
            g.DrawRectangle(new Pen(Color.Blue, 1), new Rectangle(downPos, new Size(upPos.X - downPos.X, upPos.Y - downPos.Y)));
        }
    }
}
