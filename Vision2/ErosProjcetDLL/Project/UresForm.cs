using System;
using System.Linq;
using System.Windows.Forms;
using static Vision2.ErosProjcetDLL.Project.User;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class UresForm : Form
    {
        public UresForm()
        {
            InitializeComponent();
            up();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
        }

        private bool IsCharve;

        public void up()
        {
            try
            {
                IsCharve = true;
                comboBox1.Items.Clear();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                jurisdiction.Items.Clear();
                jurisdiction.ReadOnly = true;
                dataGridView1.RowHeadersWidth = 60;
                jurisdiction.Items.AddRange(ProjectINI.In.User.UserRightGroup.Keys.ToArray());
                comboBox1.Items.Add("操作员");
                if (ProjectINI.Enbt)
                {
                    comboBox1.Items.Add("管理员");
                }
                if (ProjectINI.AdminEnbt)
                {
                    comboBox1.Items.Add("工程师");
                }

                comboBox1.SelectedIndex = 0;
                department.Items.Clear();
                department.Items.AddRange(ProjectINI.In.User.ListDepartmentGroup.ToArray());
                listBoxDepartmentGroup.Items.Clear();
                listBoxDepartmentGroup.Items.AddRange(ProjectINI.In.User.ListDepartmentGroup.ToArray());
                listBox3.Items.Clear();
                listBox3.Items.AddRange(ProjectINI.In.User.ListRightGroup.ToArray());
                dataGridView2.Rows.Clear();

                int ds = 0;
                foreach (var item in ProjectINI.In.User.UserRightGroup)
                {
                    ds = dataGridView2.Rows.Add();
                    dataGridView2.Rows[ds].Cells[0].Value = item.Key;
                    string datas = "";
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        datas += item.Value[i] + ",";
                    }
                    dataGridView2.Rows[ds].Cells[1].Value = datas.TrimEnd(',');
                }
                dataGridView1.Rows.Clear();
                //propertyGrid1.Visible = false;
                //if (ProjectINI.In.UserRight.Contains("工程师"))
                //{
                //    propertyGrid1.Visible = true;
                //    propertyGrid1.SelectedObject = ProjectINI.In.UsData;
                //}
                ds = 0;
                foreach (var item in ProjectINI.In.User.UserPassWords.Values)
                {
                    ds = dataGridView1.Rows.Add();
                    dataGridView1.Rows[ds].Cells[0].Value = item.uersName;
                    dataGridView1.Rows[ds].Cells[1].Value = item.name;
                    dataGridView1.Rows[ds].Cells[2].Value = item.UserID;
                    if (ProjectINI.In.UserRight.Contains("工程师"))
                    {
                        dataGridView1.Rows[ds].Cells[5].Value = item.passWord;
                    }
                    //dataGridView1.Rows[ds].Cells[5]
                    dataGridView1.Rows[ds].Cells[3].Value = item.UserRightGroup;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            IsCharve = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ProjectINI.In.User.ListDepartmentGroup.Contains(textBox1.Text))
            {
                MessageBox.Show("部门已存在");
            }
            else
            {
                ProjectINI.In.User.ListDepartmentGroup.Add(textBox1.Text);
            }
            up();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ProjectINI.In.User.ListRightGroup.Contains(textBox2.Text))
            {
                MessageBox.Show("权限组已存在");
            }
            else
            {
                ProjectINI.In.User.ListRightGroup.Add(textBox2.Text);
            }
            up();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectINI.In.User.UserPassWords.Remove(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
                up();
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectINI.In.SaveProjectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProjectINI.In.User.UserPassWords.ContainsKey(textBox3.Text))
                {
                    MessageBox.Show("用户名称已存在");
                }
                else
                {
                    if (textBox3.Text == "")
                    {
                        MessageBox.Show("用户名不能空");
                        return;
                    }
                    foreach (var item in ProjectINI.In.User.UserPassWords)
                    {
                        if (item.Value.UserID == textBox6.Text.Trim())
                        {
                            MessageBox.Show("工号已存在");
                            return;
                        }
                    }
                    if (textBox3.Text == "")
                    {
                        MessageBox.Show("用户名不能空");
                        return;
                    }
                    if (textBox4.Text.Length >= 6)
                    {
                        ProjectINI.In.User.UserPassWords.Add(textBox3.Text, new Users()
                        {
                            name = textBox5.Text,
                            passWord = textBox4.Text,
                            uersName = textBox3.Text,
                            UserDepartment = "",
                            UserID = textBox6.Text.Trim(),
                            UserRightGroup = comboBox1.SelectedItem.ToString()
                        });
                    }
                    else
                    {
                        MessageBox.Show("密码长度过短");
                    }
                    up();
                }
            }
            catch (Exception)
            {
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            up();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (IsCharve)
                {
                    return;
                }
                if (ProjectINI.In.UserRight.Contains("工程师"))
                {
                    if (e.ColumnIndex == 2)
                    {
                        if (ProjectINI.In.User.UserPassWords.ContainsKey(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()))
                        {
                            ProjectINI.In.User.UserPassWords[dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()].UserID =
                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}