using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Code
{
    public partial class UserCode : UserControl
    {
        public UserCode()
        {
            InitializeComponent();
        }

        public UserCode(string path) : this()
        {
            try
            {
                if (File.Exists(path))
                {
                    Ecode.Lines = File.ReadAllLines(path).ToList();
                    for (int i = 0; i < Ecode.Lines.Count; i++)
                    {
                        richTextBox1.Text += Ecode.Lines[i];
                    }
                }
                Ecode.Name = Path.GetFileNameWithoutExtension(path);
            }
            catch (Exception)
            {
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        public Code Ecode = new Code();

        private ContextMenuStrip contextMenuStrip;
        private int ds;

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //ComboBox comboBox = new ComboBox();
                //comboBox.Show();
                contextMenuStrip = new ContextMenuStrip();
                contextMenuStrip.PreviewKeyDown += ContextMenuStrip_PreviewKeyDown;
                contextMenuStrip.BackColor = Color.Black;

                Tmo tmo = Ecode.GetTmo(richTextBox1.Lines[richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart)]);

                for (int i = 0; i < tmo.Ketd.Length; i++)
                {
                    contextMenuStrip.Items.Add(tmo.Ketd[i]).ForeColor = Color.Brown;
                }
                for (int i = 0; i < tmo.PModet.Length; i++)
                {
                    contextMenuStrip.Items.Add(tmo.PModet[i]).ForeColor = Color.DarkCyan;
                }
                for (int i = 0; i < tmo.PProgma.Length; i++)
                {
                    contextMenuStrip.Items.Add(tmo.PProgma[i]).ForeColor = Color.GreenYellow;
                }
                //contextMenuStrip.Items.Add("dsg").ForeColor = Color.Brown;
                //contextMenuStrip.Items.Add("dsgt").ForeColor = Color.DarkBlue;
                //contextMenuStrip.Items.Add("dsgst").ForeColor = Color.Gray;
                Point point = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart);
                if (richTextBox1.SelectionStart != 0)
                {
                    ds = richTextBox1.SelectionStart;
                }
                //contextMenuStrip.Show(richTextBox1.PointToScreen(richTextBox1.Location));
                Ecode.Lines = richTextBox1.Lines.ToList();
                richTextBox1.SelectionStart = ds;
                richTextBox1.Focus();
                //Rectangle rectangle =
                ////richTextBox1.RectangleToScreen();
                //toolTip1.Show(dat, this, point.X+50,point.Y+50);
                //toolTip1.Show("dsd",this,point.X- richTextBox1.Location.X , point.Y- richTextBox1.Location.Y );
            }
            catch (Exception)
            {
            }
        }

        private void ContextMenuStrip_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                contextMenuStrip.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void d(object sender, KeyEventArgs e)
        {
            try
            {
                contextMenuStrip.Dispose();
                richTextBox1.Text += e.KeyData.ToString().ToLower();

                richTextBox1.SelectionStart = ds + 1;
                richTextBox1.Focus();
            }
            catch (Exception)
            {
            }
        }
    }
}