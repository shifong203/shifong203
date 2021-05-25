using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.UI.Iamage
{
    public partial class ImageList : UserControl
    {
        public ImageList()
        {
            InitializeComponent();
            Size size = this.Size;

        }
        public List<Image> Images;
        public List<string> Paths;

        [DescriptionAttribute("fales为水平显示，true为垂直"), Category("显示"), DisplayName("方向")]
        /// <summary>
        /// 水平还是垂直fales水平，ture垂直
        /// </summary>
        public bool Orientation { get; set; }
        /// <summary>
        /// 显示图片组合
        /// </summary>
        /// <param name="images"></param>
        public void RefreshIamge(List<Image> images)
        {
            foreach (Control item in this.Controls)
            {
                item.Dispose();
            }
            this.Controls.Clear();
            this.AutoScroll = true;
            for (int i = 0; i < images.Count; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                ///垂直或水平
                if (Orientation)
                {
                    pictureBox.Size = new Size(this.Width - 20, this.Width - 22);
                    pictureBox.Location = new Point(0, (this.Width - 20) * (i));
                }
                else
                {
                    pictureBox.Size = new Size(this.Height - 22, this.Height - 20);
                    pictureBox.Location = new Point((this.Height - 20) * i, 0);
                }
                pictureBox.Image = images[i];
                this.Controls.Add(pictureBox);
            }
        }

        /// <summary>
        /// 更新地址显示的图片
        /// </summary>
        /// <param name="paths"></param>
        public void RefreshIamge(List<string> paths)
        {
            foreach (Control item in this.Controls)
            {
                item.Dispose();
            }
            this.Controls.Clear();
            this.AutoScroll = true;
            for (int i = 0; i < paths.Count; i++)
            {

                PictureBox pictureBox = new PictureBox();
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                ///垂直或水平
                if (Orientation)
                {
                    pictureBox.Size = new Size(this.Width - 20, this.Width - 22);
                    pictureBox.Location = new Point(0, (this.Width - 20) * (i));
                }
                else
                {
                    pictureBox.Size = new Size(this.Height - 22, this.Height - 20);
                    pictureBox.Location = new Point((this.Height - 20) * i, 0);
                }
                if (File.Exists(paths[i]))
                {
                    pictureBox.LoadAsync(paths[i]);
                }
                else
                {
                    pictureBox.LoadAsync(@"D:\WindowsFormsApp5\NokidaE\Iamge\favicon-20180531010245196.ico");
                }
                this.Controls.Add(pictureBox);

            }
        }
        /// <summary>
        /// 更新指定地址Paths显示的图片
        /// </summary>
        public void RefreshIamge()
        {
            if (Paths != null)
            {
                RefreshIamge(Paths);
            }

        }
    }
}
