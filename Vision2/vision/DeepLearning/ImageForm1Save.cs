using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Vision2.vision.VisionSimulateForm1;

namespace Vision2.vision.DeepLearning
{
    public partial class ImageForm1Save : Form
    {
        public ImageForm1Save()
        {
            InitializeComponent();
        }
        HWindID HWiID = new HWindID();

        private void ImageForm1Save_Load(object sender, EventArgs e)
        {
            try
            {
                ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
                ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView2);
                HWiID.Initialize(hWindowControl1);
                toolStripComboBox1.Items.Clear();
                toolStripComboBox1.Items.AddRange(Vision.GetHimageList().Keys.ToArray());
                toolStripComboBox1.SelectedIndex = 0;

            }
            catch (Exception)
            {
            }
        }
        private List<string> imagePath = new List<string>();
        private DebugI debug = new DebugI();
        int runid;
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (debug.ImageDeepLearning == "")
                {
                    if (File.Exists(ErosProjcetDLL.Project.ProjectINI.TempPath + "Debug.txt"))
                    {
                        ErosProjcetDLL.Project.ProjectINI.ReadPathJsonToCalss(ErosProjcetDLL.Project.ProjectINI.TempPath + "Debug.txt", out debug);
                    }
                    if (debug == null)
                    {
                        debug = new DebugI();
                    }
                }
                if (System.IO.Directory.Exists(debug.ImageDeepLearning))
                {
                    dialog.SelectedPath = debug.ImageDeepLearning;
                }
                dialog.Description = "请选择Txt所在文件夹";
                DialogResult dialoge = ErosProjcetDLL.UI.PropertyGrid.FolderBrowserLauncher.ShowFolderBrowser(dialog);
                if (dialoge != DialogResult.OK) return;
                debug.ImageDeepLearning = dialog.SelectedPath;
                //ErosProjcetDLL.Project.ProjectINI.SetTempPrjectDataINI("视觉", "模拟文件夹地址", TraversalExecutionPath);

                ErosProjcetDLL.Project.ProjectINI.ClassToJsonSavePath(debug, ErosProjcetDLL.Project.ProjectINI.TempPath + "Debug.txt");

                this.Text = debug.ImageDeepLearning;
                Dictionary<string, List<string>> dicFile = ErosProjcetDLL.FileCon.FileConStatic.GetFilesDicListPath(debug.ImageDeepLearning, ".bmp,.jpg");
                if (dicFile.Count == 0)
                {
                    MessageBox.Show("本地Image未找到图片");
                    return;
                }
                int numbers = 0;
                dataGridView1.Rows.Clear();
                if (dicFile.Count == 0)
                {
                    MessageBox.Show("本地Image未找到图片");
                    return;
                }
                //string[] Dires = Directory.GetDirectories(debug.ImageDeepLearning);
                //string[] seles = new string[] { ".bmp", ".jpg" };
                foreach (var item in dicFile)
                {
                    HalconDotNet.HTuple hTuple = new HalconDotNet.HTuple();
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        int de = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(Path.GetFileNameWithoutExtension(item.Value[i]));
                        hTuple.Append(de);
                        if (item.Value[i].Contains("拼图"))
                        {
                            continue;
                        }
                        if (!item.Value[i].Contains(toolStripComboBox1.SelectedItem.ToString()))
                        {
                            continue;
                        }
                        String ITME = Path.GetFileNameWithoutExtension(item.Value[i]);
                        if (!ITME.EndsWith(toolStripNumericUpDown1.GetBase().Value.ToString()))
                        {
                            continue;
                        }
                        numbers++;
                        int ds = dataGridView1.Rows.Add();
                        dataGridView2.Rows.Add();
                        dataGridView1.Rows[ds].Cells[0].Value = item.Value[i] ;
                        imagePath.Add(item.Value[i]);
                    }
                }

                this.Text = "总数:" + numbers + ";" + "当前数量：" + 0 + "";
                //Demo(dicFile);
                runid = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }
        bool Stating=false;
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
           
                if (Stating)
                {
                    Stating = false;
             
                    return;
                }
                runid = 0;
                toolStripButton2.Text = "停止";
                Task task = new Task(()=> {
                    try
                    {
                        Stating = true;
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (runid < 0)
                            {
                                runid = 0;
                            }
                            HWiID.RaedIamge(dataGridView1.Rows[runid].Cells[0].Value.ToString());
                            dataGridView2.Rows[runid].Cells[0].Value = Path.GetFileName(dataGridView1.Rows[runid].Cells[0].Value.ToString());
                            runid++;
                            Vision.GetRunNameVision(toolStripComboBox1.SelectedItem.ToString()).GetOneImageR().LiyID = (int)toolStripNumericUpDown1.GetBase().Value;
                            Vision.GetRunNameVision(toolStripComboBox1.SelectedItem.ToString()).
                                CamImageEvent(Vision.GetRunNameVision(toolStripComboBox1.SelectedItem.ToString()).GetOneImageR());
                            HWiID.OneResIamge = Vision.GetRunNameVision(toolStripComboBox1.SelectedItem.ToString()).GetOneImageR();
                            HWiID.ShowObj();
                            this.Text = "图片总数:" + imagePath.Count + ";" + "当前数量：" + runid + "";
                            Thread.Sleep(500);
                        }
                        Stating = false;
                        toolStripButton2.Text = "启动";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
        
                });

                task.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ImageForm1Save_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode==Keys.Right)
                {
                    if (runid<0)
                    {
                        runid = 0;
                    }
                    HWiID.RaedIamge(dataGridView1.Rows[runid].Cells[0].Value.ToString());
                    dataGridView2.Rows[runid].Cells[0].Value = Path.GetFileName( dataGridView1.Rows[runid].Cells[0].Value.ToString());
                    runid++;
                }
                else if (e.KeyCode==Keys.Left)
                {
                    runid--;
                    HWiID.RaedIamge(dataGridView1.Rows[runid].Cells[0].Value.ToString());
              
                }
                if (e.KeyCode == Keys.Right||e.KeyCode==Keys.Left)
                {
                    Vision.GetRunNameVision(toolStripComboBox1.SelectedItem.ToString()).GetOneImageR().LiyID =(int) toolStripNumericUpDown1.GetBase().Value;
                    Vision.GetRunNameVision(toolStripComboBox1.SelectedItem.ToString()).
                        CamImageEvent(Vision.GetRunNameVision(toolStripComboBox1.SelectedItem.ToString()).GetOneImageR());
                    HWiID.OneResIamge = Vision.GetRunNameVision(toolStripComboBox1.SelectedItem.ToString()).GetOneImageR();
                    HWiID.ShowObj();
                }
                this.Text = "图片总数:" + imagePath.Count+ ";" + "当前数量：" + runid + "";
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                HWiID.  RaedIamge(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception ex)
            {

            }
        }
    }
}
