using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.Project.DebugF;

namespace Vision2.vision
{
    public partial class VisionSimulateForm1 : Form
    {
        public VisionSimulateForm1()
        {
            InitializeComponent();
            toolStripComboBox1.SelectedIndex = 0;
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView2);
        }

        public class DebugI
        {
            public string ImagePath { get; set; } = "";
        }

        private bool Cambueys;

        private List<string> imagePath = new List<string>();
        private DebugI debug = new DebugI();

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (debug.ImagePath == "")
                {
                    if (File.Exists(ErosProjcetDLL.Project.ProjectINI.TempPath + "Debug.txt"))
                    {
                        ErosProjcetDLL.Project.ProjectINI.ReadPathJsonToCalss(ErosProjcetDLL.Project.ProjectINI.TempPath + "Debug.txt", out debug);
                    }
                    if (debug == null)
                    {
                        debug = new DebugI();
                    }
                    //TempPath + "PrjectData.ini"
                    //ErosProjcetDLL.Project.ProjectINI.GetTempPrjectDataINI("视觉", "模拟文件夹地址", out TraversalExecutionPath);
                    //TraversalExecutionPath = Vision.Instance.DicSaveType[Vision.GetRunNameVision().Name].SavePath;
                }
                if (System.IO.Directory.Exists(debug.ImagePath))
                {
                    dialog.SelectedPath = debug.ImagePath;
                }
                dialog.Description = "请选择Txt所在文件夹";
                DialogResult dialoge = ErosProjcetDLL.UI.PropertyGrid.FolderBrowserLauncher.ShowFolderBrowser(dialog);
                if (dialoge != DialogResult.OK) return;
                debug.ImagePath = dialog.SelectedPath;
                //ErosProjcetDLL.Project.ProjectINI.SetTempPrjectDataINI("视觉", "模拟文件夹地址", TraversalExecutionPath);

                ErosProjcetDLL.Project.ProjectINI.ClassToJsonSavePath(debug, ErosProjcetDLL.Project.ProjectINI.TempPath + "Debug.txt");

                this.Text = debug.ImagePath;
                var det = ErosProjcetDLL.FileCon.FileConStatic.GetFilesDicListPath(debug.ImagePath, ".bmp,.jpg");
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
                imagePath.Clear();
                Dictionary<string, List<string>> dicFile = new Dictionary<string, List<string>>();
                string[] Dires = Directory.GetDirectories(debug.ImagePath);
                string[] seles = new string[] { ".bmp", ".jpg" };
                keyValuePath = new Dictionary<string, List<string>>();
                for (int i = 0; i < Dires.Length; i++)
                {
                    List<string> fileList = new List<string>();

                    string[] files = Directory.GetFiles(Dires[i]);
                    for (int i1 = 0; i1 < files.Length; i1++)
                    {
                        if (files[i1].Contains("拼图"))
                        {
                            continue;
                        }
                        for (int i2 = 0; i2 < seles.Length; i2++)
                        {
                            if (files[i1].ToLower().EndsWith(seles[i2].ToLower(), StringComparison.Ordinal))
                            {
                                fileList.Add(files[i1]);
                                break;
                            }
                        }
                    }
                    dicFile.Add(Path.GetFileName(Dires[i]), fileList);
                }

                foreach (var item in dicFile)
                {
                    int ds = dataGridView1.Rows.Add();
                    dataGridView1.Rows[ds].Cells[0].Value = Path.GetFileName(item.Key);
                    HalconDotNet.HTuple hTuple = new HalconDotNet.HTuple();
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        int de = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(Path.GetFileNameWithoutExtension(item.Value[i]));
                        hTuple.Append(de);
                        if (toolStripComboBox1.SelectedIndex == 0)
                        {
                            imagePath.Add(item.Value[i]);
                        }
                        else
                        {
                            if (!item.Value[i].Contains(toolStripComboBox1.SelectedItem.ToString()))
                            {
                                continue;
                            }
                        }
                        numbers++;
                        imagePath.Add(item.Value[i]);
                    }
                    HalconDotNet.HTuple detd = hTuple.TupleSortIndex();
                    List<string> listImage = new List<string>();

                    for (int i = 0; i < detd.Length; i++)
                    {
                        listImage.Add(item.Value[detd[i]]);
                    }
                    keyValuePath.Add(item.Key, listImage);
                }
                toolStripLabel1.Text = "总数:" + numbers + ";" + "文件夹数量：" + det.Count + "";
                //Demo(dicFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Dictionary<string, List<string>> keyValuePath;

        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="path"></param>
        private void Demo(Dictionary<string, List<string>> path)
        {
            try
            {
                int idRow = 0;
                //foreach (var item in path)
                //{
                //    for (int i = 0; i < item.Value.Count; i++)
                //    {
                //        string picFileName = item.Value[i];//图片文件名路路径
                //        PictureBox pic = new PictureBox();
                //        Label label = new Label();
                //        pic.SizeMode = PictureBoxSizeMode.Zoom;
                //        pic.Name = "PicBox" + i.ToString();
                //        pic.Size = new System.Drawing.Size(450, 300);
                //        Point picLocatoin = new Point(i * 460, idRow * 340);
                //        pic.Location = picLocatoin;
                //        label.Location = new Point(i * 460 + 40, idRow * 340 + 310);
                //        label.Text = picFileName;
                //        label.AutoSize = true;
                //        //加载图片
                //        pic.Image = new Bitmap(picFileName);
                //        //窗体类加子控件添加该picBox控件
                //        this.tabPage1.Controls.Add(pic);
                //        this.tabPage1.Controls.Add(label);
                //    }
                //    idRow++;
                //}
                keyValuePath = path;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool isThra;
        private bool stop;

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (isThra)
                {
                    stop = true;

                    return;
                }
                this.toolStripButton4.Text = "停止执行";
                stop = false;
                Thread thread = new Thread(() =>
                {
                    isThra = true;
                    foreach (var item in keyValuePath)
                    {
                        for (int i = 0; i < item.Value.Count; i++)
                        {
                            foreach (var itemd in Vision.GetHimageList().Values)
                            {
                                if (stop)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        this.toolStripButton4.Text = "开始模拟执行";
                                    }));
                                    isThra = false;
                                    return;
                                }
                                if (item.Value[i].Contains(itemd.Name))
                                {
                                    OneResultOBj oneResultOBj = new OneResultOBj();

                                    oneResultOBj.IsSave = toolStripCheckbox1.GetBase().Checked;
                                    oneResultOBj.ReadImage(item.Value[i]);
                                    string name = Path.GetFileNameWithoutExtension(item.Value[i]);
                                    int liID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name);
                                    oneResultOBj.LiyID = liID;
                                    oneResultOBj.RunID = liID;
                                    if (name.Contains('-'))
                                    {
                                        string dat = name.Split('-')[0];
                                        oneResultOBj.RunID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(dat);
                                    }
                                    Thread.Sleep(DebugCompiler.Instance.MarkWait);
                                    itemd.AsysReadCamImage(oneResultOBj.LiyID, oneResultOBj.RunID, asyncRestImage => { }, oneResultOBj);
                                    Thread.Sleep(DebugCompiler.Instance.CamWait);
                                    //itemd.CamImageEvent(oneResultOBj);
                                }
                            }
                        }
                    }
                    this.Invoke(new Action(() =>
                    {
                        this.toolStripButton4.Text = "开始模拟执行";
                    }));
                    isThra = false;
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 执行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (isThra)
                {
                    stop = true;

                    return;
                }
                string SN = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                Thread thread = new Thread(() =>
                {
                    for (int i = 0; i < keyValuePath[SN].Count; i++)
                    {
                        foreach (var itemd in Vision.GetHimageList().Values)
                        {
                            if (keyValuePath[SN][i].Contains(itemd.Name))
                            {
                                OneResultOBj oneResultOBj = new OneResultOBj();
                                oneResultOBj.ReadImage(keyValuePath[SN][i]);
                                string name = Path.GetFileNameWithoutExtension(keyValuePath[SN][i]);
                                oneResultOBj.RunID = oneResultOBj.LiyID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name);
                                if (name.Contains('-'))
                                {
                                    string dat = name.Split('-')[0];
                                    oneResultOBj.RunID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(dat);
                                }
                                oneResultOBj.IsSave = false;
                                itemd.CamImageEvent(oneResultOBj);
                                Thread.Sleep(500);
                            }
                        }
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Vision.ShowVisionResetForm();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1)
                {
                    if (isThra)
                    {
                        stop = true;

                        return;
                    }
                    string SN = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    Thread thread = new Thread(() =>
                    {
                        for (int i = 0; i < keyValuePath[SN].Count; i++)
                        {
                            foreach (var itemd in Vision.GetHimageList().Values)
                            {
                                if (keyValuePath[SN][i].Contains(itemd.Name))
                                {
                                    OneResultOBj oneResultOBj = new OneResultOBj();
                                    oneResultOBj.ReadImage(keyValuePath[SN][i]);
                                    string name = Path.GetFileNameWithoutExtension(keyValuePath[SN][i]);
                                    oneResultOBj.RunID = oneResultOBj.LiyID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name);
                                    if (name.Contains('-'))
                                    {
                                        string dat = name.Split('-')[0];
                                        oneResultOBj.RunID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(dat);
                                    }
                                    oneResultOBj.IsSave = false;
                                    itemd.CamImageEvent(oneResultOBj);
                                    Thread.Sleep(500);
                                }
                            }
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            try
            {
                if (isThra)
                {
                    stop = true;

                    return;
                }
                this.toolStripButton4.Text = "停止执行";
                stop = false;
                Thread thread = new Thread(() =>
                {
                    isThra = true;
                    foreach (var item in keyValuePath)
                    {
                        for (int i = 0; i < item.Value.Count; i++)
                        {
                            foreach (var itemd in Vision.GetHimageList().Values)
                            {
                                if (stop)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        this.toolStripButton4.Text = "开始模拟执行";
                                    }));
                                    isThra = false;
                                    return;
                                }
                                if (item.Value[i].Contains(itemd.Name))
                                {
                                    OneResultOBj oneResultOBj = new OneResultOBj();
                                    oneResultOBj.IsSave = toolStripCheckbox1.GetBase().Checked;
                                    oneResultOBj.ReadImage(item.Value[i]);
                                    string name = Path.GetFileNameWithoutExtension(item.Value[i]);
                                    int liID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name);
                                    oneResultOBj.LiyID = liID;
                                    oneResultOBj.RunID = liID;
                                    if (name.Contains('-'))
                                    {
                                        string dat = name.Split('-')[0];
                                        oneResultOBj.RunID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(dat);
                                    }

                                    itemd.AysnetCam(oneResultOBj.LiyID, oneResultOBj.RunID, oneResultOBj.Image);
                                    Thread.Sleep(DebugCompiler.Instance.CamWait);
                                    //itemd.CamImageEvent(oneResultOBj);
                                }
                            }
                        }
                    }
                    this.Invoke(new Action(() =>
                    {
                        this.toolStripButton4.Text = "开始模拟执行";
                    }));
                    isThra = false;
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void VisionSimulateForm1_Load(object sender, EventArgs e)
        {
            try
            {
                Task task = new Task(() =>
                {
                    Thread.Sleep(1000);
                    this.dataGridView2.AutoGenerateColumns = false;//不自动
                    while (!this.IsDisposed)
                    {
                        try
                        {
                            this.Invoke(new Action(() =>
                            {
                                List<OneResultOBj> oneResultOBjs = Vision.GetRunNameVision().GetRunsOne();
                                if (oneResultOBjs.Count != dataGridView2.Rows.Count)
                                {
                                    BindingList<OneResultOBj> BList = new BindingList<OneResultOBj>(oneResultOBjs);
                                    //赋值给BindingList对象,Form全局变量

                                    dataGridView2.DataSource = BList;
                                }
                            }));
                        }
                        catch (Exception)
                        { }
                        Thread.Sleep(10);
                    }
                });

                task.Start();
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            try
            {
                Vision.GetRunNameVision().AysnetRun();
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (debug.ImagePath == "")
                {
                    if (File.Exists(ErosProjcetDLL.Project.ProjectINI.TempPath + "Debug.txt"))
                    {
                        ErosProjcetDLL.Project.ProjectINI.ReadPathJsonToCalss(ErosProjcetDLL.Project.ProjectINI.TempPath + "Debug.txt", out debug);
                    }
                    if (debug == null)
                    {
                        debug = new DebugI();
                    }
                    //TempPath + "PrjectData.ini"
                    //ErosProjcetDLL.Project.ProjectINI.GetTempPrjectDataINI("视觉", "模拟文件夹地址", out TraversalExecutionPath);
                    //TraversalExecutionPath = Vision.Instance.DicSaveType[Vision.GetRunNameVision().Name].SavePath;
                }
                if (System.IO.Directory.Exists(debug.ImagePath))
                {
                    dialog.SelectedPath = debug.ImagePath;
                }
                dialog.Description = "请选择Txt所在文件夹";
                DialogResult dialoge = ErosProjcetDLL.UI.PropertyGrid.FolderBrowserLauncher.ShowFolderBrowser(dialog);
                if (dialoge != DialogResult.OK) return;
                debug.ImagePath = dialog.SelectedPath;
                //ErosProjcetDLL.Project.ProjectINI.SetTempPrjectDataINI("视觉", "模拟文件夹地址", TraversalExecutionPath);

                ErosProjcetDLL.Project.ProjectINI.ClassToJsonSavePath(debug, ErosProjcetDLL.Project.ProjectINI.TempPath + "Debug.txt");

                this.Text = debug.ImagePath;
                var det = ErosProjcetDLL.FileCon.FileConStatic.GetFilesDicListPath(debug.ImagePath, ".bmp,.jpg");
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
                imagePath.Clear();
                Dictionary<string, List<string>> dicFile = new Dictionary<string, List<string>>();
                string[] Dires = Directory.GetDirectories(debug.ImagePath);
                string[] seles = new string[] { ".bmp", ".jpg" };
                keyValuePath = new Dictionary<string, List<string>>();
                for (int i = 0; i < Dires.Length; i++)
                {
                    List<string> fileList = new List<string>();

                    string[] files = Directory.GetFiles(Dires[i]);
                    for (int i1 = 0; i1 < files.Length; i1++)
                    {
                        if (files[i1].Contains("拼图"))
                        {
                            continue;
                        }
                        for (int i2 = 0; i2 < seles.Length; i2++)
                        {
                            if (files[i1].ToLower().EndsWith(seles[i2].ToLower(), StringComparison.Ordinal))
                            {
                                fileList.Add(files[i1]);
                                break;
                            }
                        }
                    }
                    dicFile.Add(Path.GetFileName(Dires[i]), fileList);
                }

                foreach (var item in det)
                {
                    int ds = dataGridView1.Rows.Add();
                    dataGridView1.Rows[ds].Cells[0].Value = Path.GetFileName(item.Key);
                    HalconDotNet.HTuple hTuple = new HalconDotNet.HTuple();
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        OneResultOBj oneResultOBj = new OneResultOBj();
                        oneResultOBj.IsSave = toolStripCheckbox1.GetBase().Checked;
                        oneResultOBj.ReadImage(item.Value[i]);
                        string name = Path.GetFileNameWithoutExtension(item.Value[i]);
                        int liID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name);
                        oneResultOBj.LiyID = liID;
                        oneResultOBj.RunID = liID;
                        if (name.Contains('-'))
                        {
                            string dat = name.Split('-')[0];
                            oneResultOBj.RunID = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(dat);
                        }

                        Vision.GetRunNameVision().AysnetCam(oneResultOBj.LiyID, oneResultOBj.RunID, oneResultOBj.Image);
                        Thread.Sleep(DebugCompiler.Instance.CamWait);
                        int de = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(Path.GetFileNameWithoutExtension(item.Value[i]));
                        hTuple.Append(de);
                        if (toolStripComboBox1.SelectedIndex == 0)
                        {
                            imagePath.Add(item.Value[i]);
                        }
                        else
                        {
                            if (!item.Value[i].Contains(toolStripComboBox1.SelectedItem.ToString()))
                            {
                                continue;
                            }
                        }
                        numbers++;
                        imagePath.Add(item.Value[i]);
                    }
                    HalconDotNet.HTuple detd = hTuple.TupleSortIndex();
                    List<string> listImage = new List<string>();

                    for (int i = 0; i < detd.Length; i++)
                    {
                        listImage.Add(item.Value[detd[i]]);
                    }
                    keyValuePath.Add(item.Key, listImage);
                }
                toolStripLabel1.Text = "总数:" + numbers + ";" + "文件夹数量：" + det.Count + "";
                //Demo(dicFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}