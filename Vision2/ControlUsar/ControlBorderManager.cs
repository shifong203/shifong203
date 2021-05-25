using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.ControlUsar
{
    /// <summary>
    ///
    /// </summary>
    public class ControlBorderManager
    {
        //过渡效果使用的定时器为System.Windows.Forms.Timer，所以无需考虑多线程同步。
        private const int FrameCount = 30;     //过度动画总帧数

        private const int Interval = 10;       //帧与帧之间的间隔（毫秒），实际间隔会比设置的间隔略长

        public enum State
        {
            Default,
            MouseOn,
            Focus
        };

        private struct ControlGroup
        {
            public Control[] controls;
            public State state;
            public State FormerState;
            public Color DefaultColor;
            public Color MouseOnColor;
            public Color FocusColor;
            public Func<Rectangle> GetBorderRect;
        }

        private List<ControlGroup> ControlGroups = new List<ControlGroup>();
        private Form form;

        //窗体是否正在处于拖动改变大小的状态。如果窗体正在改变大小，在OnPaint事件里就不进行边框的绘制，
        //否则边框会拖出一大片“轨迹”出来。
        private bool Resizing = false;

        public ControlBorderManager()
        {
            ;
        }

        //返回id，以后便可以查询该控件组的状态
        public int AddControlGroup(
            Form FormInstance,            //窗体的this
            Control[] Controls,
            Color DefaultColor,
            Color MouseOnColor,
            Color FocusColor,
            Func<Rectangle> GetBorderRect //这个函数需要返回一个Rectangle，表示此控件组的边框
            )
        {
            ControlGroup group;
            group.controls = Controls;
            group.state = State.Default;
            group.FormerState = State.Default;
            group.DefaultColor = DefaultColor;
            group.MouseOnColor = MouseOnColor;
            group.FocusColor = FocusColor;
            group.GetBorderRect = GetBorderRect;
            ControlGroups.Add(group);

            form = FormInstance;

            int id = ControlGroups.Count - 1;

            Action<object, EventArgs> OnMouseEnter = (sender, e) =>
            {
                if (ControlGroups[id].state == State.Focus) return;

                ControlGroup newgroup;
                newgroup.controls = ControlGroups[id].controls;
                newgroup.FormerState = ControlGroups[id].state;
                newgroup.state = State.MouseOn;
                newgroup.DefaultColor = ControlGroups[id].DefaultColor;
                newgroup.MouseOnColor = ControlGroups[id].MouseOnColor;
                newgroup.FocusColor = ControlGroups[id].FocusColor;
                newgroup.GetBorderRect = ControlGroups[id].GetBorderRect;

                ControlGroups[id] = newgroup;

                DrawBorder(id);
            };

            Action<object, EventArgs> OnMouseLeave = (sender, e) =>
            {
                if (ControlGroups[id].state == State.Focus) return;

                ControlGroup newgroup;
                newgroup.controls = ControlGroups[id].controls;
                newgroup.FormerState = ControlGroups[id].state;
                newgroup.state = State.Default;
                newgroup.DefaultColor = ControlGroups[id].DefaultColor;
                newgroup.MouseOnColor = ControlGroups[id].MouseOnColor;
                newgroup.FocusColor = ControlGroups[id].FocusColor;
                newgroup.GetBorderRect = ControlGroups[id].GetBorderRect;

                ControlGroups[id] = newgroup;

                DrawBorder(id);
            };

            Action<object, MouseEventArgs> OnMouseDown = (sender, e) =>
            {
                if (ControlGroups[id].state == State.Focus) return;

                ControlGroup newgroup;
                newgroup.controls = ControlGroups[id].controls;
                newgroup.FormerState = ControlGroups[id].state;
                newgroup.state = State.Focus;
                newgroup.DefaultColor = ControlGroups[id].DefaultColor;
                newgroup.MouseOnColor = ControlGroups[id].MouseOnColor;
                newgroup.FocusColor = ControlGroups[id].FocusColor;
                newgroup.GetBorderRect = ControlGroups[id].GetBorderRect;

                ControlGroups[id] = newgroup;

                DrawBorder(id);

                ControlGroup newgroup2;

                for (int i = 0; i < ControlGroups.Count; i++)
                {
                    if (i == id) continue;

                    newgroup2.controls = ControlGroups[i].controls;
                    newgroup2.FormerState = ControlGroups[i].state;
                    newgroup2.state = State.Default;
                    newgroup2.DefaultColor = ControlGroups[i].DefaultColor;
                    newgroup2.MouseOnColor = ControlGroups[i].MouseOnColor;
                    newgroup2.FocusColor = ControlGroups[i].FocusColor;
                    newgroup2.GetBorderRect = ControlGroups[i].GetBorderRect;

                    ControlGroups[i] = newgroup2;

                    DrawBorder(i);
                }
            };

            foreach (Control control in Controls)
            {
                control.MouseEnter += new EventHandler(OnMouseEnter);
                control.MouseLeave += new EventHandler(OnMouseLeave);
                control.MouseDown += new MouseEventHandler(OnMouseDown);
            }

            form.Paint += new PaintEventHandler(OnPaint);

            form.ResizeBegin += new EventHandler((_sender, _e) =>
            {          //防止边框拖出一片“轨迹”出来
                Resizing = true;
                form.Refresh();
            });
            form.ResizeEnd += new EventHandler((_sender, _e) =>
            {            //防止边框拖出一片“轨迹”出来
                Resizing = false;
                form.Refresh();
            });

            return id;
        }

        public State GetState(int id)
        {
            if (id >= 0 && id < ControlGroups.Count)
            {
                return ControlGroups[id].state;
            }
            else
            {
                return State.Default;
            }
        }

        private void DrawBorder(int id)                                 //绘制边框，有过渡效果。
        {
            Color FormerColor;
            Color CurrentColor;

            State CurrentState = ControlGroups[id].state;

            switch (ControlGroups[id].FormerState)
            {
                case State.Default:
                    FormerColor = ControlGroups[id].DefaultColor;
                    break;

                case State.MouseOn:
                    FormerColor = ControlGroups[id].MouseOnColor;
                    break;

                case State.Focus:
                    FormerColor = ControlGroups[id].FocusColor;
                    break;

                default:
                    FormerColor = ControlGroups[id].DefaultColor;
                    break;
            }

            switch (ControlGroups[id].state)
            {
                case State.Default:
                    CurrentColor = ControlGroups[id].DefaultColor;
                    break;

                case State.MouseOn:
                    CurrentColor = ControlGroups[id].MouseOnColor;
                    break;

                case State.Focus:
                    CurrentColor = ControlGroups[id].FocusColor;
                    break;

                default:
                    CurrentColor = ControlGroups[id].DefaultColor;
                    break;
            }

            int n = 0;

            float Rstep = ((float)CurrentColor.R - (float)FormerColor.R) / (float)FrameCount;
            float Gstep = ((float)CurrentColor.G - (float)FormerColor.G) / (float)FrameCount;
            float Bstep = ((float)CurrentColor.B - (float)FormerColor.B) / (float)FrameCount;

            float R = FormerColor.R;
            float G = FormerColor.G;
            float B = FormerColor.B;

            Rectangle BorderRect = ControlGroups[id].GetBorderRect();

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = Interval;
            timer.Tick += new EventHandler((_sender, _e) =>
            {
                R += Rstep; G += Gstep; B += Bstep;
                if (n >= FrameCount || ControlGroups[id].state != CurrentState)
                {         //如果已经画完或者状态已经改变
                    timer.Stop();
                    timer.Dispose();
                    return;
                }
                Color c = Color.FromArgb((int)R, (int)G, (int)B);
                Brush b = new SolidBrush(c);
                form.CreateGraphics().DrawRectangle(new Pen(b), BorderRect);             //绘制矩形边框

                n++;
            });
            timer.Start();
        }

        private void OnPaint(object sender, PaintEventArgs e)                //响应窗体的paint事件。相应Paint事件的时候无需过渡效果。
        {
            if (Resizing) return;

            Color CurrentColor;
            Brush b;
            foreach (ControlGroup group in ControlGroups)
            {
                switch (group.state)
                {
                    case State.Default:
                        CurrentColor = group.DefaultColor;
                        break;

                    case State.MouseOn:
                        CurrentColor = group.MouseOnColor;
                        break;

                    case State.Focus:
                        CurrentColor = group.FocusColor;
                        break;

                    default:
                        CurrentColor = group.DefaultColor;
                        break;
                }

                b = new SolidBrush(CurrentColor);
                e.Graphics.DrawRectangle(new Pen(b), group.GetBorderRect());             //绘制矩形边框
            }
        }
    }
}