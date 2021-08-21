using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.IO;

//using System.Windows.Documents;
using System.Windows.Forms;

//using ErosSocket.ErosConLink;

namespace Vision2.ConClass
{
    public partial class OnManagementForm : Form
    {
        public OnManagementForm()
        {
            InitializeComponent();

            foreach (int myCode in Enum.GetValues(typeof(Vision2.ErosProjcetDLL.Project.ProjectINI.RunMode)))
            {
                string strName = Enum.GetName(typeof(Vision2.ErosProjcetDLL.Project.ProjectINI.RunMode), myCode);//获取名称
                string strVaule = myCode.ToString();//获取值
                //ListItem myLi = new ListItem(strName, strVaule);
                toolStripComboBox1.Items.Add(strName);//添加到DropDownList控件
            }
            toolStripComboBox1.SelectedItem = Vision2.ErosProjcetDLL.Project.ProjectINI.In.Run_Mode.ToString();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "执行文件.exe | *.exe| 所有文件 | *.* ";
            openFileDialog.ShowDialog();
            openFileDialog.Multiselect = false;

            if (openFileDialog.FileName == "") { return; }       //判断是否选择路径
            int d = dataGridView1.Rows.Add();
            dataGridView1.Rows[d].Cells[0].Value = true;
            dataGridView1.Rows[d].Cells[1].Value = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            dataGridView1.Rows[d].Cells[2].Value = openFileDialog.FileName;
            dataGridView1.Rows[d].Cells[3].Value = DateTime.Now;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string path = Application.StartupPath + "\\开机启动.txt";
            string values = string.Empty;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {
                    if (dataGridView1.Rows[i].Cells[2].Value != null || dataGridView1.Rows[i].Cells[0].Value != null || dataGridView1.Rows[i].Cells[1].Value != null)
                    {
                        if (dataGridView1.Rows[i].Cells[3].Value == null)
                        {
                            dataGridView1.Rows[i].Cells[3].Value = DateTime.Now;
                        }
                        bool bss = Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].EditedFormattedValue);
                        if (StaticCon.AutoStart(bss, dataGridView1.Rows[i].Cells[1].Value.ToString(), dataGridView1.Rows[i].Cells[2].Value.ToString()))
                        {
                            values = "[" + dataGridView1.Rows[i].Cells[0].EditedFormattedValue + ";" + dataGridView1.Rows[i].Cells[1].Value.ToString() + ";" +
                            dataGridView1.Rows[i].Cells[2].Value.ToString() + ";" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "]" + Environment.NewLine;
                        }
                        else
                        {
                            MessageBox.Show("保存失败！");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            File.WriteAllText(path, values);//以文本写入并覆盖

            //ErosConLink.StaticCon.AutoStart(false);
        }

        private void OnManagementForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Application.StartupPath + "\\开机启动.txt"))
                {
                    string[] strs = File.ReadAllLines(Application.StartupPath + "\\开机启动.txt");  //以每一行读取数据
                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].StartsWith("[", StringComparison.Ordinal))
                        {
                            string[] datas = strs[i].Trim('[', ']').Split(';');
                            if (datas.Length == 4)
                            {
                                int d = dataGridView1.Rows.Add();
                                dataGridView1.Rows[d].Cells[0].Value = Convert.ToBoolean(datas[0]);
                                dataGridView1.Rows[d].Cells[1].Value = datas[1];
                                dataGridView1.Rows[d].Cells[2].Value = datas[2];
                                dataGridView1.Rows[d].Cells[3].Value = datas[3];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "请选择保存路径";      //文件框名称
            saveFile.InitialDirectory = Application.StartupPath;  //默认路径
            saveFile.Filter = "文本文件|*.txt|所有文件|*.*";   //筛选器
            saveFile.ShowDialog();    //弹出对话框
            string path = saveFile.FileName;
            if (path == "") return;    //地址为空返回
            List<string> listStr = new List<string>();
            string values = string.Empty;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                values += "[" + dataGridView1.Rows[i].Cells[0].Value.ToString() + ";" + dataGridView1.Rows[i].Cells[1].Value.ToString() + ";" +
                              dataGridView1.Rows[i].Cells[2].Value.ToString() + dataGridView1.Rows[i].Cells[3].Value.ToString() + "]" + Environment.NewLine;
            }
            File.WriteAllText(path, values);//以文本写入并覆盖
            MessageBox.Show("保存完毕");
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Application.StartupPath + "\\开机启动.txt"))
                {
                    string[] strs = File.ReadAllLines(Application.StartupPath + "\\开机启动.txt");  //以每一行读取数据
                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].StartsWith("[", StringComparison.Ordinal))
                        {
                            string[] datas = strs[i].Trim('[', ']').Split(';');
                            if (datas.Length == 4)
                            {
                                int d = dataGridView1.Rows.Add();
                                dataGridView1.Rows[d].Cells[0].Value = Convert.ToBoolean(datas[0]);
                                dataGridView1.Rows[d].Cells[1].Value = datas[1];
                                dataGridView1.Rows[d].Cells[2].Value = datas[2];
                                dataGridView1.Rows[d].Cells[3].Value = datas[3];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Vision2.ErosProjcetDLL.Project.ProjectINI.RunMode runMode;
                if (Enum.TryParse(toolStripComboBox1.SelectedItem.ToString(), out runMode))
                {
                    Vision2.ErosProjcetDLL.Project.ProjectINI.In.Run_Mode = runMode;
                    Vision2.ErosProjcetDLL.Project.ProjectINI.In.SaveProjectAll();
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }
    }
}