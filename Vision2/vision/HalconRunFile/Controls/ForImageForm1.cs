using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class ForImageForm1 : Form
    {
        public ForImageForm1(HalconRunFile.RunProgramFile.HalconRun halconRun)
        {
            InitializeComponent();
            halcon = halconRun;
            Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);

        }
        HalconRunFile.RunProgramFile.HalconRun halcon;
        string TraversalExecutionPath = "";
        bool Cambueys = false;
        bool ist = false;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ist = false;
                this.button4.Text = "暂停";
                if (TraversalExecutionPath == "")
                {
                    System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                    dialog.SelectedPath = TraversalExecutionPath;
                    dialog.Description = "请选择Txt所在文件夹";
                    System.Windows.Forms.DialogResult dialoge = Vision2.ErosProjcetDLL.UI.PropertyGrid.FolderBrowserLauncher.ShowFolderBrowser(dialog);
                    if (dialoge != System.Windows.Forms.DialogResult.OK) return;
                    TraversalExecutionPath = dialog.SelectedPath;
                }
                if (Cambueys)
                {
                    if (label2.Text.StartsWith("==> 遍历执行文件夹中....", StringComparison.Ordinal))
                    {
                        Cambueys = false;
                    }
                    return;
                }
                this.Text = TraversalExecutionPath;

                Thread thread = new Thread(() =>
                {
                    Cambueys = true;
                    try
                    {
                        if (dataGridView1.Rows.Count == 0)
                        {
                            Cambueys = false;
                            MessageBox.Show("本地Image未找到图片");
                            return;
                        }
                        string files = "==> 遍历执行文件夹中....";
                        int numbers = 0;
                        progressBar1.Maximum = dataGridView1.Rows.Count;
                        label4.Text = numbers + "/0";
                        files += "总数:" + numbers + ";" + "文件夹数量：" + dataGridView1.Rows.Count + "";
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            try
                            {
                                if (!Cambueys)
                                {
                                    label2.Text = "停止";
                                    Cambueys = false;
                                    return;
                                }
                                while (ist)
                                {
                                    if (this.IsDisposed)
                                    {
                                        return;
                                    }
                                }
                                if (dataGridView1.Rows[i].Cells[0].Value == null)
                                {
                                    continue;
                                }
                                if (halcon.ReadImage(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                                    halcon.CamImageEvent(numericUpDown1.Value.ToString(), halcon.Image().Clone(), (int)numericUpDown1.Value, false);
                                this.Invoke(new Action(() =>
                                {
                                    progressBar1.Value = i;
                                    label4.Text = dataGridView1.Rows.Count + "/" + (i);
                                    label2.Text = files + "/" + i + ";" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "||" + dataGridView1.Rows.Count + "/" + (i + 1);
                                    //dataGridView1.Rows[cdid-1].Cells[0].Value = item.Value[i];
                                    dataGridView1.Rows[i].Cells[1].Value = halcon.Result;
                                    string dtaT = "";
                                    foreach (var itemt in halcon.TrayRestData.DicBool)
                                    {
                                        dtaT += itemt.Key + ':';
                                        foreach (var itemt2 in itemt.Value)
                                        {
                                            dtaT += itemt2.Key + "," + itemt2.Value + ";";
                                        }
                                    }
                                    dataGridView1.Rows[i].Cells[2].Value = dtaT;
                                    halcon.SaveDataExcel("遍历数据", halcon.WriteDataCName.Keys.ToArray(), (i + 1).ToString(), halcon.WriteDataCName.Values.ToArray());
                                    halcon.ShowObj();
                                }));
                            }
                            catch (Exception)
                            {
                            }
                            Thread.Sleep((int)(numericUpDown2.Value * 1000));
                        }
                        //progressBar1.Value = progressBar1.Maximum;

                        label2.Text = "执行完成";
                        //this.Invoke(methodInvoker);
                    }
                    catch (Exception)
                    {
                    }
                    Cambueys = false;


                });

                thread.Priority = ThreadPriority.Highest;
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
             {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                if (TraversalExecutionPath == "")
                {
                    TraversalExecutionPath = Vision.Instance.DicSaveType[halcon.Name].SavePath;
                }
                if (System.IO.Directory.Exists(TraversalExecutionPath))
                {
                    dialog.SelectedPath = TraversalExecutionPath;
                }
                dialog.Description = "请选择Txt所在文件夹";
                System.Windows.Forms.DialogResult dialoge = Vision2.ErosProjcetDLL.UI.PropertyGrid.FolderBrowserLauncher.ShowFolderBrowser(dialog);
                if (dialoge != System.Windows.Forms.DialogResult.OK) return;
                TraversalExecutionPath = dialog.SelectedPath;
                this.Text = TraversalExecutionPath;
                var det = Vision2.ErosProjcetDLL.FileCon.FileConStatic.GetFilesDicListPath(TraversalExecutionPath, ".bmp,.jpg");
                if (det.Count == 0)
                {
                    Cambueys = false;
                    MessageBox.Show("本地Image未找到图片");
                    return;
                }
                int numbers = 0;
                dataGridView1.Rows.Clear();

                if (det.Count == 0)
                {
                    Cambueys = false;
                    MessageBox.Show("本地Image未找到图片");
                    return;
                }

                foreach (var item in det)
                {       
                    for (int i = 0; i < item.Value.Count; i++)
                    {

                        if (!checkBox2.Checked || item.Value[i].Contains(halcon.Name))
                        {
                            if (checkBox1.Checked&&!System.IO.Path.GetFileNameWithoutExtension(item.Value[i]).EndsWith(numericUpDown1.Value.ToString()))
                            {
                                continue;
                            }
                            numbers++;
                            int ds = dataGridView1.Rows.Add();
                            dataGridView1.Rows[ds].Cells[0].Value = item.Value[i];
                        }
                    }
                }
                progressBar1.Maximum = numbers;
                label4.Text = numbers + "/0";
                label2.Text = "总数:" + numbers + ";" + "文件夹数量：" + det.Count + "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cambueys = false;
            this.button4.Text = "暂停";
            ist = false;
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (Cambueys)
                {
                    return;
                }
                if (e.ColumnIndex == 0)

                {
                    halcon.ReadImage(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());

                    halcon.CamImageEvent(halcon.RunIDStr[(int)numericUpDown1.Value - 1], halcon.Image().Clone(), (int)numericUpDown1.Value, false);
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value = halcon.Result;
                    //dataGridView1.Rows[e.RowIndex].Cells[2].Value = halcon.Message;
                }
            }
            catch (Exception)
            {
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (ist)
            {
                this.button4.Text = "暂停";
                ist = false;
            }
            else
            {
                this.button4.Text = "继续";
                ist = true;
            }

        }

        private void ForImageForm1_Load(object sender, EventArgs e)
        {

        }
    }
}
