using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision
{
    public partial class LibraryFormAdd : Form
    {
        public LibraryFormAdd(HalconRunFile.RunProgramFile.HalconRun halconRun)
        {
            halcon = halconRun;
            InitializeComponent();
        }

        private HalconRunFile.RunProgramFile.HalconRun halcon;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string mes = "";
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        if (!halcon.GetRunProgram().ContainsKey(checkedListBox1.Items[i].ToString()))
                        {
                            RunProgram runProgram;
                            string name = checkedListBox1.Items[i].ToString();
                            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
                            StringBuilder stdt = new StringBuilder(100);
                            try
                            {
                                ErosProjcetDLL.Excel.Npoi.GetPrivateProfileString("视觉库", name, "", stdt, 500, Library.LibraryBasics.PathStr + "\\Library.ini");
                                string ntype = stdt.ToString();     /*item.Value.Split('.')[item.Value.Split('.').Length - 1];*/
                                dynamic obj = assembly.CreateInstance(/*halcon.GetType().Namespace + "." +*/ ntype); // 创建类的实例
                                if (obj != null)
                                {
                                    runProgram = obj.UpSatrt<RunProgram>(Library.LibraryBasics.PathStr + name + "\\" + name);
                                }
                                else
                                {
                                    obj = assembly.CreateInstance(ntype); // 创建类的实例
                                    runProgram = obj.UpSatrt<RunProgram>(Library.LibraryBasics.PathStr + name + "\\" + name);
                                }
                                runProgram.SetPThis(halcon);
                                //runProgram.Name = item.Key;
                                halcon.GetRunProgram().Add(runProgram.Name, runProgram);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(name + "读取错误:" + ex.Message);
                            }
                        }
                        else
                        {
                            RunProgram runProgram;
                            string name = checkedListBox1.Items[i].ToString();
                            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
                            StringBuilder stdt = new StringBuilder(100);
                            try
                            {
                                ErosProjcetDLL.Excel.Npoi.GetPrivateProfileString("视觉库", name, "", stdt, 500, Library.LibraryBasics.PathStr + "\\Library.ini");
                                string ntype = stdt.ToString();     /*item.Value.Split('.')[item.Value.Split('.').Length - 1];*/
                                dynamic obj = assembly.CreateInstance(/*halcon.GetType().Namespace + "." +*/ ntype); // 创建类的实例
                                if (obj != null)
                                {
                                    runProgram = obj.UpSatrt<RunProgram>(Library.LibraryBasics.PathStr + name + "\\" + name);
                                }
                                else
                                {
                                    obj = assembly.CreateInstance(ntype); // 创建类的实例
                                    runProgram = obj.UpSatrt<RunProgram>(Library.LibraryBasics.PathStr + name + "\\" + name);
                                }
                                runProgram.SetPThis(halcon);
                                halcon.GetRunProgram()[checkedListBox1.Items[i].ToString()] = runProgram;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(name + "读取错误:" + ex.Message);
                            }
                        }
                        mes += checkedListBox1.Items[i] + ";";
                    }
                }
                if (mes != "")
                {
                    MessageBox.Show(mes + "导入成功");
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "导入失败");
            }
        }

        public void UPData()
        {
            try
            {
                Directory.CreateDirectory(Library.LibraryBasics.PathStr);
                string[] files = Directory.GetDirectories(Library.LibraryBasics.PathStr);
                checkedListBox1.Items.Clear();
                //TreeNode treeNode = treeView1.Nodes.Add("视觉库");
                for (int i = 0; i < files.Length; i++)
                {
                    checkedListBox1.Items.Add(Path.GetFileNameWithoutExtension(files[i]));
                }
                //StringBuilder staSET = new StringBuilder(100);
                string staSET = "                                                          ";

                ErosProjcetDLL.Excel.Npoi.GetPrivateProfileString("视觉库", null, "", staSET, 500, Library.LibraryBasics.PathStr + "\\Library.ini");
                string[] vs = staSET.ToString().Split('\0');
                StringBuilder stdt = new StringBuilder(100);
                for (int i = 0; i < vs.Length; i++)
                {
                    if (vs[i].Trim() != "")
                    {
                        ErosProjcetDLL.Excel.Npoi.GetPrivateProfileString("视觉库", vs[i], "", stdt, 500, Library.LibraryBasics.PathStr + "\\Library.ini");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LibraryFormAdd_Load(object sender, EventArgs e)
        {
            try
            {
                UPData();
            }
            catch (Exception)
            {
            }
        }
    }
}