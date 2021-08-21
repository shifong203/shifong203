using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.Project.formula
{
    public partial class MaterialUserControl1 : UserControl
    {
        public MaterialUserControl1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Product.MatchTheMaterial(textBox1.Text))
                {
                    Task.Run(() =>
                    {
                        MessageBox.Show("匹配成功", "物料匹配", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        textBox2.Text = "匹配成功";
                    }
                    );
                }
                else
                {
                    textBox2.Text = "匹配失败";
                }
            }
            catch (Exception)
            {
            }
        }
    }
}