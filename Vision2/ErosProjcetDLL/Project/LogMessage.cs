using System;
using System.IO;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class LogMessageForm : Form
    {
        public LogMessageForm(string path) : this()
        {
            LogPath = path;
            try
            {
                if (File.Exists(path))
                {
                    LogMessage(File.ReadAllText(LogPath));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public LogMessageForm()
        {
            InitializeComponent();
        }

        public string LogPath = string.Empty;

        private void LogMessageForm_Load(object sender, EventArgs e)
        {
            //LogMessage(vision.HalconRun.ReadLogIncident());
        }

        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = ProjectINI.TempPath;
                openFileDialog.Title = "打开一个文本文件";
                openFileDialog.Filter = "文本文件|*.txt|所有文件|*.*";
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName != "")
                {
                    LogMessage(File.ReadAllText(openFileDialog.FileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LogMessage(string message)
        {
            try
            {
                richTextBox1.AppendText(message + Environment.NewLine);
                string[] datast = message.Split('>');
                dataGridView1.Rows.Clear();
                int d = 0;

                if (datast.Length > 2 || datast[0].StartsWith("事件信息", StringComparison.Ordinal))
                {
                    toolStripLabel1.Text = datast[0] + ">" + datast[1];
                }

                for (int i = 2; i < datast.Length; i++)
                {
                    d = dataGridView1.Rows.Add();
                    string[] datas = datast[i].Split('|');

                    for (int it = 0; it < datas.Length; it++)
                    {
                        dataGridView1.Rows[d].Cells[it].Value = datas[it];
                    }
                }

                richTextBox1.ScrollToCaret();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}