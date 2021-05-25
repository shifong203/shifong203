using System.Drawing;
using System.Windows.Forms;

namespace ErosSocket.ErosUI
{
    public class TextBoxProperty : PropertyBase
    {
        private TextBox _Control;

        public TextBoxProperty()
        {
        }

        public TextBoxProperty(TextBox control)
        {
            this._Control = control;
        }

        [MyControlAttibute("文本", "获取或者设置控件文本", "")]
        public string Text
        {
            get { return this._Control.Text; }
            set
            {
                this._Control.Text = value;
            }
        }

        [MyControlAttibute("宽度", "获取或者设置控件宽度", "")]
        public int Width
        {
            get { return this._Control.Width; }
            set
            {
                this._Control.Width = (int)value;
            }
        }

        [MyControlAttibute("高度", "获取或者设置控件高度", "")]
        public int Height
        {
            get { return this._Control.Height; }
            set
            {
                this._Control.Height = (int)value;
            }
        }

        [MyControlAttibute("上边距", "获取或者设置控件上边位置", "")]
        public int Top
        {
            get { return this._Control.Top; }
            set
            {
                this._Control.Top = value;
            }
        }

        [MyControlAttibute("左边距", "获取或者设置控件左边位置", "")]
        public int Left
        {
            get { return this._Control.Left; }
            set
            {
                this._Control.Left = value;
            }
        }

        [MyControlAttibute("背景色", "获取或者设置控件背景颜色", "")]
        public Color BackColor
        {
            get { return this._Control.BackColor; }
            set
            {
                this._Control.BackColor = value;
            }
        }

        [MyControlAttibute("前景色", "获取或者设置控件的前景颜色", "")]
        public Color ForeColor
        {
            get { return this._Control.ForeColor; }
            set
            {
                this._Control.ForeColor = value;
            }
        }
    }
}