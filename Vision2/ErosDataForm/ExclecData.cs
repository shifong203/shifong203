using System;
using System.IO;
using System.Windows.Forms;
using Vision2.ConClass;
using Vision2.vision.HalconRunFile.RunProgramFile;
namespace Vision2.ErosUI
{
    public partial class ExclecData : Form
    {
        private HalconRun HalconRun;

        public ExclecData(HalconRun halcon)
        {
            InitializeComponent();
            HalconRun = halcon;
        }

        private string ptahFile = System.Windows.Forms.Application.StartupPath + "\\历史数据\\";

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void 导入ExclecToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }

        private void OpneExcel()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择文件";
            openFileDialog.InitialDirectory = Application.StartupPath; //初始路径
            openFileDialog.Filter = "Excel文件|*.xlsx;*.xls|所有文件|*.*";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileNames.Length != 0)
            {
                try
                {
                    Npoi.UpDataExclec(openFileDialog.FileName, tabControl1, pictureBox2, groupBox1, HalconRun);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //ConClass.Npoi.UpDataExclec(Application.StartupPath + "\\遍历数据\\" + DateTime.Now.ToLongDateString() + ".xls");
            //Npoi.AddWriteToExcel(ptahFile + DateTime.Now.ToLongDateString() + ".xls", DateTime.Now.ToLongDateString(),
            //   "NG", "1", "0.1", "12");
            //Npoi.AddWriteColumnToExcel(ptahFile + DateTime.Now.ToLongDateString() + ".xls", DateTime.Now.ToLongDateString(),
            //   new string[] { "名称", "结果", "最大误差", "边界数量" });
            try
            {
                //vision.Vision.Instance.Himagelist[ Vision.Instance.RunNameVision].SaveDataExcelImage("历史数据", Vision.Instance.Himagelist[Vision.Instance.RunNameVision].WriteData.TupleMax(), Vision.Instance.Himagelist[Vision.Instance.RunNameVision]["NG数量"], Vision.Instance.Himagelist[Vision.Instance.RunNameVision].WriteData);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void ExclecUI_Load(object sender, EventArgs e)
        {
            //Npoi.UpDataExclec(ptahFile + DateTime.Now.ToLongDateString() + ".xls", tabControl1, pictureBox1, groupBox1, HalconRun);
            string[] stringArray = Vision2.ErosProjcetDLL.Project.ProjectINI.GetFilesArrayPath(ptahFile, ".xls");
            for (int i = 0; i < stringArray.Length; i++)
            {
                listBox1.Items.Add(Path.GetFileNameWithoutExtension(stringArray[i]));
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    listBox2.Items.Clear();
                    string[] dsatas = Vision2.ErosProjcetDLL.FileCon.FileConStatic.GetFilesArrayPath(System.Windows.Forms.Application.StartupPath + "\\SaveImage\\" + listBox1.SelectedItem, "bmp");
                    for (int i = 0; i < dsatas.Length; i++)
                    {
                        listBox2.Items.Add(Path.GetFileName(dsatas[i]));
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
        }



        private void 打开文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(Application.StartupPath + "\\历史数据");
            System.Diagnostics.Process.Start(Application.StartupPath + "\\历史数据");
        }

        private void 导入EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpneExcel();
        }

        private void 打开表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    if (File.Exists(ptahFile + listBox1.SelectedItem.ToString() + ".xls"))
                    {
                        System.Diagnostics.Process.Start(ptahFile + listBox1.SelectedItem.ToString() + ".xls");
                    }
                    else
                    {
                        MessageBox.Show(ptahFile + listBox1.SelectedItem.ToString() + ".xls 文件已不存在！");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox2.SelectedItem != null)
                {
                    string dsd = System.Windows.Forms.Application.StartupPath + "\\SaveImage\\" + listBox1.SelectedItem.ToString() + "\\" + listBox2.SelectedItem.ToString();
                    pictureBox2.Load(dsd);
                }
            }
            catch (Exception)
            {


            }

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }
    }
}