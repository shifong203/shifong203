using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vision2.Project.DebugF.IO;

namespace Vision2.Project.formula
{
    public partial class FormulaContrsl : UserControl
    {
        public FormulaContrsl()
        {
            InitializeComponent();
        }

        public void UpData()
        {
            try
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(Product.GetThisP().Keys.ToArray());
                if (xYZPoints1 == null)
                {
                    foreach (var item in Product.GetThisP())
                    {
                        if (RecipeCompiler.Instance.ProductEX.ContainsKey(item.Key))
                        {
                            xYZPoints1 = RecipeCompiler.Instance.ProductEX[item.Key].DPoint;
                            break;
                        }
                    }
                }
                foreach (var item in Product.GetThisP())
                {
                    if (!RecipeCompiler.Instance.ProductEX.ContainsKey(item.Key))
                    {
                        RecipeCompiler.Instance.ProductEX.Add(item.Key, new ProductEX());

                        string da=  ErosProjcetDLL.Project.ProjectINI.ClassToJsonString(PEX);
                        ProductEX Prod = new ProductEX();
                        ErosProjcetDLL.Project.ProjectINI.StringJsonToCalss<ProductEX>(da, out Prod);
                        RecipeCompiler.Instance.ProductEX[item.Key] = Prod;
                    }
                }
                Dictionary<string, ProductEX> produceEX = new Dictionary<string, ProductEX>();
                foreach (var item in RecipeCompiler.Instance.ProductEX)
                {
                    if (Product.GetThisP().ContainsKey(item.Key))
                    {
                        produceEX.Add(item.Key, item.Value);
                    }
                }
                RecipeCompiler.Instance.ProductEX = produceEX;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        List<XYZPoint> xYZPoints1;
        ProductEX PEX;
        
        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            string name = Product.ProductionName;
            Product product = new Product(true);
            xYZPoints1 = RecipeCompiler.Instance.ProductEX[name].DPoint;
            PEX = RecipeCompiler.Instance.ProductEX[name];
            //RelativelyPoint = RecipeCompiler.Instance.ProductEX[name].Relativel.RelativelyPoint;
            if (Product.ProductionName != name)
            {
                vision.Vision.Instance.SaveRunPojcet();
            }
            UpData();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    return;
                }
                propertyGrid1.SelectedObject = Product.GetProduct();
                this.formulaEditorControl1.AddDataGridConmlus(Product.GetListLinkNames.ToArray());
                this.formulaEditorControl1.Updatas(listBox1.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void 删除产品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                return;
            }
            Product.Remove(listBox1.SelectedItem.ToString());
            UpData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (Vision2.ErosProjcetDLL.Project.ProjectINI.Enbt || Vision2.ErosProjcetDLL.Project.ProjectINI.GetUserJurisdiction("管理权限"))
            {
                Product.SaveDicExcel(Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\产品配方\\产品文件");
            }
            else
            {
                MessageBox.Show("权限不足无法保存修改");
            }
            Cursor = Cursors.Arrow;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            try
            {
                openFileDialog.InitialDirectory = Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\产品配方\\";
                openFileDialog.Filter = "文件|*";
                DialogResult dialog = openFileDialog.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    if (Product.ReadExcelDic(openFileDialog.FileName, out string err))
                    {
                        MessageBox.Show("读取成功" + err);
                    }
                    else
                    {
                        MessageBox.Show("读取失败" + err);
                    }
                    UpData();
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            UpData();
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    Product.Aotu(listBox1.SelectedItem.ToString(), ErosSocket.ErosConLink.DicSocket.Instance.SocketClint);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }



        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\产品配方");
                System.Diagnostics.Process.Start(Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\产品配方");
            }
            catch (Exception)
            {
            }
        }

        private void FormulaContrsl_Load(object sender, EventArgs e)
        {
            UpData();
        }

        private void 管理参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormulaPrForm formulaPrForm = new FormulaPrForm();
            formulaPrForm.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string NewName = Product.AmendName(listBox1.SelectedItem.ToString());
                if (NewName != "")
                {
                    string path = Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\" + vision.Vision.Instance.FileName + "\\" + listBox1.SelectedItem.ToString();
                    Product.SaveDicExcel(Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\产品配方\\产品文件");
                    if (!Directory.Exists(path))
                    {
                        MessageBox.Show("未创建图像程序[" + listBox1.SelectedItem.ToString() + "]", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (MessageBox.Show("是否修改图像程序名？", NewName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            string Newpath = Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\" + vision.Vision.Instance.FileName + "\\" + NewName;
                            Directory.Move(path, Newpath);
                        }
                    }
                    if (RecipeCompiler.Instance.ProductEX.ContainsKey(listBox1.SelectedItem.ToString()))
                    {
                        ProductEX xYZPoints = RecipeCompiler.Instance.ProductEX[listBox1.SelectedItem.ToString()];
                        RecipeCompiler.Instance.ProductEX.Remove(listBox1.SelectedItem.ToString());
                        RecipeCompiler.Instance.ProductEX.Add(NewName, xYZPoints);
                    }
                    UpData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Application.StartupPath + @"\help.chm"))
                {
                    string url = Application.StartupPath + @"\help.chm::调试1.htm";
                    //Vision2.ErosProjcetDLL.Project. CHMHelp.ShowHelp(url);
                    Help.ShowHelp(this, Application.StartupPath + @"\help.chm", HelpNavigator.Topic, "3_调试位置.htm");

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void 备份配方文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Vision2.ErosProjcetDLL.Project.ProjectINI.ClassToJsonSavePath(Product.GetThisP(), Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\产品配方\\配方备份\\配方文件" + DateTime.Now.ToString("yy年MM月dd日HH时mm分ss秒")))
                {
                    MessageBox.Show("备份成功");
                }
            }
            catch (Exception)
            {
            }
        }

        private void 导入配方备份ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, Dictionary<string, string>> keyValuePairs = new Dictionary<string, Dictionary<string, string>>();
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (Directory.Exists(ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\产品配方\\配方备份\\"))
                {
                    openFileDialog.InitialDirectory = ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\产品配方\\配方备份\\";
                }
                else
                {
                    openFileDialog.InitialDirectory = ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\产品配方\\";
                }
                openFileDialog.Filter = "文件|*.txt";
                DialogResult dialog = openFileDialog.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    if (ErosProjcetDLL.Project.ProjectINI.ReadPathJsonToCalss(openFileDialog.FileName, out keyValuePairs))
                    {
                        RecipeCompiler.Instance.Produc = keyValuePairs;
                        Product.GetThisP(keyValuePairs);
                        MessageBox.Show("导入成功，共" + keyValuePairs.Count + "个配方");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.Description = "请选择文件夹";
                fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)+"\\"+listBox1.SelectedItem.ToString();
                System.Windows.Forms.DialogResult dialog = Vision2.ErosProjcetDLL.UI.PropertyGrid.FolderBrowserLauncher.ShowFolderBrowser(fbd);
                if (dialog == System.Windows.Forms.DialogResult.OK)
                {
                    if (true)
                    {
                        ErosProjcetDLL.Project.ProjectINI.ClassToJsonSavePath(RecipeCompiler.Instance.Produc[listBox1.SelectedItem.ToString()], fbd.SelectedPath+"\\配方参数");
                        string path = Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\" + vision.Vision.Instance.FileName + "\\" + listBox1.SelectedItem.ToString();
                        CopyFolder2(path, fbd.SelectedPath);
                        //Product.SaveDicExcel(Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\产品配方\\产品文件");
                 
                        if (RecipeCompiler.Instance.ProductEX.ContainsKey(listBox1.SelectedItem.ToString()))
                        {
                            ProductEX xYZPoints = RecipeCompiler.Instance.ProductEX[listBox1.SelectedItem.ToString()];
                            ErosProjcetDLL.Project.ProjectINI.ClassToJsonSavePath(xYZPoints, fbd.SelectedPath+"\\产品参数");
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 复制文件夹及文件
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public int CopyFolder2(string sourceFolder, string destFolder)
        {
            try
            {
                string folderName = System.IO.Path.GetFileName(sourceFolder);
                string destfolderdir = System.IO.Path.Combine(destFolder, folderName);
                string[] filenames = System.IO.Directory.GetFileSystemEntries(sourceFolder);
                foreach (string file in filenames)// 遍历所有的文件和目录
                {
                    if (System.IO.Directory.Exists(file))
                    {
                        string currentdir = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(currentdir))
                        {
                            System.IO.Directory.CreateDirectory(currentdir);
                        }
                        CopyFolder2(file, destfolderdir);
                    }
                    else
                    {
                        string srcfileName = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(destfolderdir))
                        {
                            System.IO.Directory.CreateDirectory(destfolderdir);
                        }
                        System.IO.File.Copy(file, srcfileName,true);
                    }
                }

                return 1;
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
                return 0;
            }

        }
        private void formulaEditorControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
