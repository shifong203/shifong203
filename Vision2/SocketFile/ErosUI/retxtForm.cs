using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ErosSocket.ErosUI
{
    public partial class retxtForm : Form
    {
        public retxtForm()
        {
            InitializeComponent();
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = @"C:\Users\Eros\Desktop";
                openFileDialog.Filter = "文本文件|*.txt|所有文件|*.*";
                openFileDialog.ShowDialog();                   //展示对话框
                string name = openFileDialog.FileName;          //获得打开的文件的路径
                if (name == "") { return; }       //判断是否选择路径

                richTextBox1.Text = "";

                string[] strs = File.ReadAllLines(name, Encoding.UTF8);  //以每一行读取数据

                string[] strRets = new string[strs.Length];
                for (int i = 0; i < strs.Length; i++)
                {
                    string[] item = strs[i].Split('.');
                    if (item.Length > 1)
                    {
                        if (item[0].Length > Convert.ToInt16(txtLeng.Text))
                        {
                            strRets[i] = strs[i];
                        }
                        else
                        {
                            for (int i1 = 1; i1 < item.Length; i1++)
                            {
                                strRets[i] += item[i1] + ".";
                            }
                            strRets[i] = strRets[i].TrimEnd('.');
                        }
                    }
                    else
                    {
                        strRets[i] = strs[i];
                    }
                    richTextBox1.AppendText(strRets[i]);
                }
                string patht = Path.GetDirectoryName(name) + "\\New" + Path.GetFileName(name);
                File.WriteAllLines(patht, strRets, Encoding.UTF8);  //以每一行读取数据
            }
            catch (Exception)
            {
            }
        }
    }
}