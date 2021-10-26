using System;
using System.Collections.Generic;
using System.Drawing;
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

        private HalconRunFile.RunProgramFile.HalconRun halcon;
        private string TraversalExecutionPath = "";
        private bool Cambueys = false;
        private bool ist = false;
        private int ok = 0;
        private int ng = 0;

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
                this.button1.Enabled = false;
                this.button2.Enabled = true;
                this.button3.Enabled = true;
                Thread thread = new Thread(() =>
                {
                    Cambueys = true;
                    ok = 0;
                    ng = 0;
                    try
                    {
                        if (dataGridView1.Rows.Count == 0)
                        {
                            this.button1.Enabled = true;
                            Cambueys = false;
                            this.button3.Enabled = this.button2.Enabled = false;

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
                                    this.button1.Enabled = true;
                                    label2.Text = "停止";
                                    Cambueys = false;
                                    this.button3.Enabled = this.button2.Enabled = false;
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
                                {
                                    halcon.GetOneImageR().IsSave = false;
                                    string[] names = System.IO.Path.GetFileNameWithoutExtension(dataGridView1.Rows[i].Cells[0].Value.ToString()).Split('-');
                                    halcon.GetOneImageR().RunID =
                                    ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(names[1]);
                                    halcon.GetOneImageR().LiyID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(names[0]);
                                    halcon.CamImageEvent( halcon.GetOneImageR());
                                }

                                this.Invoke(new Action(() =>
                                {
                                    progressBar1.Value = i;
                                    label4.Text = dataGridView1.Rows.Count + "/" + (i);
                                    label2.Text = files + "/" + i + ";" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "||" + dataGridView1.Rows.Count + "/" + (i + 1);
                                    //dataGridView1.Rows[cdid-1].Cells[0].Value = item.Value[i];
                                    dataGridView1.Rows[i].Cells[1].Value = halcon.Result;
                                    if (halcon.Result == "OK")
                                    {
                                        dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.Green;
                                        ok++;
                                    }
                                    else
                                    {
                                        if (checkBox3.Checked)
                                        {
                                            this.button4.Text = "继续";
                                            ist = true;
                                        }
                                        dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.Red;
                                        ng++;
                                    }
                                    string dtaT = "";
                                    foreach (var itemt in halcon.TrayRestData.DicBool)
                                    {
                                        dtaT += itemt.Key + ':';
                                        foreach (var itemt2 in itemt.Value)
                                        {
                                            dtaT += itemt2.Key + "," + itemt2.Value + ";";
                                        }
                                    }
                                    upText();
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
                    this.button1.Enabled = true;
                    this.button3.Enabled = this.button2.Enabled = false;
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
                FolderBrowserDialog dialog = new FolderBrowserDialog();
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
                //var det = Vision2.ErosProjcetDLL.FileCon.FileConStatic.GetFilesDicListPath(TraversalExecutionPath, ".bmp,.jpg,.hobj");
                List<string> images = Vision2.ErosProjcetDLL.FileCon.FileConStatic.GetFilesListPath(TraversalExecutionPath, ".bmp,.jpg,.hobj");

                if (images.Count == 0)
                {
                    Cambueys = false;
                    MessageBox.Show("本地Image未找到图片");
                    return;
                }
                int numbers = 0;
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                if (images.Count == 0)
                {
                    Cambueys = false;
                    MessageBox.Show("本地Image未找到图片");
                    return;
                }
                List<string> listImages = new List<string>();
                for (int i = 0; i < images.Count; i++)
                {
                    if (!checkBox2.Checked || images[i].Contains(halcon.Name))
                    {
                        if (checkBox1.Checked )
                        {
                          string[] names= System.IO.Path.GetFileNameWithoutExtension(images[i]).Split('-');
                      
                            if (ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(names[0]) != numericUpDown1.Value)
                            {
                                continue;
                            }
                        }
                        listImages.Add(images[i]);
                    }
                }
                this.dataGridView1.DataSource = (from paths in listImages select new { paths }).ToList();
                //dataGridView1.Columns.Add(Column1);
                dataGridView1.Columns.Add(Column2);
                dataGridView1.Columns.Add(Column3);
                progressBar1.Maximum = numbers;
                label4.Text = numbers + "/0";
                label2.Text = "总数:" + numbers + ";" + "文件数量：" + listImages.Count + "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void upText()
        {
            this.Invoke(new Action(() =>
            {
                label5.Text = "OK:" + ok + " NG:" + ng + " ==> " + ((double)(100.0 / (ok + ng) * (double)ok)).ToString("00.0") + "%";
            }));
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
                if (Cambueys && !ist)
                {
                    return;
                }

                if (e.ColumnIndex == 0)
                {
                    halcon.ReadImage(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    halcon.GetOneImageR().IsSave = false;
                    string[] names = System.IO.Path.GetFileNameWithoutExtension(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()).Split('-');
                    halcon.GetOneImageR().RunID =
                    ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(names[1]);
                    halcon.GetOneImageR().LiyID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(names[0]);
                    halcon.CamImageEvent( halcon.GetOneImageR());
                    if (ok > 0 && ng > 0)
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() != halcon.Result)
                        {
                            if (halcon.Result == "OK")
                            {
                                ok++;
                                ng--;
                            }
                            else
                            {
                                ok--;
                                ng++;
                            }
                        }
                    }
                    if (halcon.Result == "OK")
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.Yellow;
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.Red;
                    }
                    upText();
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value = halcon.Result;
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}