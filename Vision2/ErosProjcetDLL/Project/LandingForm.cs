using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class LandingForm : Form
    {
        public LandingForm()
        {
            InitializeComponent();
            comboBox1.Items.Clear();

            comboBox1.Items.AddRange(ProjectINI.In.User.UserPassWords.Keys.ToArray());

            comboBox1.Text = ProjectINI.RaedTempPathText("登录名");
            if (comboBox1.Text == "")
            {
                comboBox1.SelectedIndex = 1;
            }
        }

        public LandingForm(bool isLoinClrsi) : this()
        {
            isloinColsi = isLoinClrsi;
        }

        private bool isloinColsi;

        private UresForm uresForm;

        private void button2_Click(object sender, EventArgs e)
        {
            if (uresForm == null)
            {
                uresForm = new UresForm();
            }
            UI.UICon.WindosFormerShow(ref uresForm);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (User.Loge(comboBox1.Text.ToString(), textBox2.Text))
                {
                    label3.Text = "登录成功";
                    ProjectINI.SaveTempPathText("登录名", comboBox1.Text.ToString());
                    if (isloinColsi)
                    {
                        this.Close();
                        return;
                    }
                    if (ProjectINI.In.UserRight.Contains("管理"))
                    {
                        button2.Visible = true;
                    }
                    else
                    {
                        button2.Visible = false;
                    }
                    button5.Visible = true;
                }
                else
                {
                    label3.Text = "登录失败";
                    button5.Visible = false;
                }
            }
            catch (Exception)
            {
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.UseSystemPasswordChar)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            User.Del();
            textBox2.Text = "";
            comboBox1.Text = "";
        }

        private void LandingForm_Load(object sender, EventArgs e)
        {
            try
            {
                string[] paths = System.IO.Directory.GetFiles(ErosProjcetDLL.Project.ProjectINI.ProjectPathRun);
                for (int i = 0; i < paths.Length; i++)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(paths[i]);
                    if (fileName.ToLower() == "logo")
                    {
                        pictureBox1.Image = Image.FromFile(paths[i]);

                        break;
                    }
                }
            }
            catch (Exception)
            { }
        }
    }
}