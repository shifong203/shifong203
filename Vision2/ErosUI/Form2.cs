using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.ErosUI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AllowDrop = true;
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form2_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.Form2_DragOver);
        }

        private void Form2_DragOver(object sender, DragEventArgs e)
        {
            if (e.AllowedEffect == DragDropEffects.Move)
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void Form2_DragDrop(object sender, DragEventArgs e)
        {
            Button btn = e.Data.GetData("System.Windows.Forms.Button") as Button;
            if (btn != null)
            {
                if (btn.Parent != null)
                {
                    btn.Parent.Controls.Remove(btn);
                    btn.Location = this.PointToClient(new Point(e.X, e.Y));
                    this.Controls.Add(btn);
                }
            }
        }
    }
}