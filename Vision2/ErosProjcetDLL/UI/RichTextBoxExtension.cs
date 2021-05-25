using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.UI
{
    public static class RichTextBoxExtension
    {
        /// <summary>
        /// 添加到RichText文本框并修改颜色，添加到新的一行或结尾
        /// </summary>
        /// <param name="rtBox">文本控件</param>
        /// <param name="text">文本</param>
        /// <param name="color">颜色</param>
        /// <param name="selectionBackColor">背景色</param>
        /// <param name="font">字体</param>
        /// <param name="addNewLine">新行或结尾</param>
        public static void AppendTextColorful(RichTextBox rtBox, string text, Color color, Color selectionBackColor, Font font = null, bool addNewLine = true)
        {
            if (addNewLine)
            {
                text += Environment.NewLine;
            }
            Font thisfont = rtBox.Font;
            if (font != null)
            {
                rtBox.SelectionFont = font;
            }
            rtBox.SelectionStart = rtBox.TextLength;
            rtBox.SelectionLength = 0;
            rtBox.SelectionColor = color;
            rtBox.SelectionBackColor = selectionBackColor;
            rtBox.AppendText(text);
            rtBox.SelectionColor = rtBox.ForeColor;
            rtBox.SelectionBackColor = rtBox.BackColor;
            rtBox.SelectionFont = thisfont;
        }


    }
}