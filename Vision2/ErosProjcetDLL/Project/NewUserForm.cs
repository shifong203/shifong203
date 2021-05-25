using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class NewUserForm : Form
    {
        public NewUserForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ProjectINI.In.User.UserPassWords.ContainsKey(textBox1.Text))
                {
                    label7.ForeColor = Color.Red;
                    label7.Text = "用户名已存在";
                }
                else if (textBox1.Text.Length < 4 && textBox1.Text.Length > 12)
                {
                    label7.ForeColor = Color.Red;
                    label7.Text = "请输入4-12个字符";
                }
                else
                {
                    label7.ForeColor = Color.Green;
                    label7.Text = "可以使用";
                }
            }
            catch (Exception)
            {


            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text.Length < 6 && textBox2.Text.Length > 12)
                {
                    label8.ForeColor = Color.Red;
                    label8.Text = "请输入4-12个字符";
                }
                else
                {
                    label8.ForeColor = Color.Green;
                    label8.Text = "可以使用";
                }
            }
            catch (Exception)
            {
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text.Length < 6 && textBox3.Text.Length > 12)
                {
                    label9.ForeColor = Color.Red;
                    label9.Text = "请输入4-12个字符";
                }
                else if (textBox2.Text != textBox3.Text)
                {
                    label9.ForeColor = Color.Green;
                    label9.Text = "2次输入的密码不相同";
                }
            }
            catch (Exception)
            {
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //User
        }
    }
}
