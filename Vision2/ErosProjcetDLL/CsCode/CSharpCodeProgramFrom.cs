using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.CsCode
{
    public partial class CSharpCodeProgramFrom : Form
    {
        public CSharpCodeProgramFrom()
        {
            InitializeComponent();
        }

        private CSharpCode cSharpCode;

        private void toolStripDropDownButton2_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        private void 插入测试代码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (cSharpCode == null)
                {
                    cSharpCode = new CSharpCode();
                }

                dynamic sd = cSharpCode.DebugCode("DynamicCodeGenerate.HelloWorld", "OutPut", out string errs);

                richTextBox2.AppendText(sd.ToString() + Environment.NewLine);
                //cSharpCode.DebugCode(richTextBox3.Text, richTextBox3.Text, out string errs);
                richTextBox2.AppendText(errs + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (cSharpCode == null)
                {
                    cSharpCode = new CSharpCode();
                }
                dynamic dsa = cSharpCode.CompileAssmblyFromSource(richTextBox1.Text, out string errstring);
                if (dsa != null)
                {
                    richTextBox2.AppendText(dsa.ToString() + Environment.NewLine);
                    //for (int i = 0; i < cSharpCode.GetAssembly().GetManifestResourceNames().Length; i++)
                    //{
                    //    richTextBox2.AppendText(cSharpCode.GetAssembly().GetManifestResourceNames()[i]+ Environment.NewLine);
                    //}
                    //richTextBox2.AppendText(cSharpCode.GetAssembly().GetName().FullName + Environment.NewLine);
                    //foreach (var item in cSharpCode.GetAssembly().Modules)
                    //{
                    //    richTextBox2.AppendText(item.ScopeName + Environment.NewLine);
                    //    richTextBox2.AppendText(item.FullyQualifiedName + Environment.NewLine);
                    //}
                    richTextBox2.AppendText(cSharpCode.GetAssembly().GetName().CultureName + Environment.NewLine);
                    for (int i = 0; i < cSharpCode.GetAssembly().GetModules().Length; i++)
                    {
                        richTextBox2.AppendText(cSharpCode.GetAssembly().GetModules()[i].ScopeName + Environment.NewLine);
                        richTextBox2.AppendText(cSharpCode.GetAssembly().GetModules()[i].GetFields().Length + Environment.NewLine);
                        richTextBox2.AppendText(cSharpCode.GetAssembly().GetModules()[i].GetMethods().Length + Environment.NewLine);

                        foreach (var item in cSharpCode.GetAssembly().GetModules()[i].GetMethods())
                        {
                            richTextBox2.AppendText(item.Name + Environment.NewLine);
                            richTextBox2.AppendText(item.DeclaringType.Name + Environment.NewLine);
                        }
                        //richTextBox2.AppendText(cSharpCode.GetAssembly().GetModules()[i].Name + Environment.NewLine);
                        //richTextBox2.AppendText(cSharpCode.GetAssembly().GetModules()[i].ScopeName + Environment.NewLine);
                    }
                }
                else
                {
                    richTextBox2.AppendText("结果为Null" + Environment.NewLine);
                }
                treeView1.Nodes.Clear();
                foreach (var item in cSharpCode.ListDll)
                {
                    treeView1.Nodes.Add(item);
                }
                richTextBox2.AppendText(errstring + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 插入实例ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.AppendText(CSharpCode.GenerateCode());
                treeView1.Nodes.Clear();
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                //string dats = Class1.UpMaix(out string errs);
                //richTextBox1.AppendText(dats+Environment.NewLine);
                //richTextBox2.AppendText(errs);
            }
            catch (Exception)
            {
            }
        }

        private void 导入DllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "";
                openFileDialog.Filter = "dll文件(.dll)|*.dll|执行文件(.exe)|*.exe";
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName != "")
                {
                    cSharpCode.AddDll(openFileDialog.FileName);
                }
                treeView1.Nodes.Clear();
                foreach (var item in cSharpCode.ListDll)
                {
                    treeView1.Nodes.Add(item);
                }
            }
            catch (Exception)
            {
            }
        }

        private string errs;
        private List<object> listObj = new List<object>();

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //dynamic sd = cSharpCode.DebugCode("DynamicCodeGenerate.HelloWorld", "OutPut", out string errs);
                dynamic sd;
                listObj.Clear();
                //listObj.Add(hWindowControl1.HalconWindow);
                object[] datas = textBox1.Text.Split(',');
                listObj.AddRange(datas);
                if (textBox1.Text == "")
                {
                    sd = cSharpCode.DebugCode(richTextBox3.Text, comboBox1.Text, out errs);
                }
                else
                {
                    sd = cSharpCode.DebugCode(richTextBox3.Text, comboBox1.Text, out errs, listObj.ToArray());
                }
                AddTextNewLine(sd.ToString());
                AddTextNewLine(errs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddTextNewLine(string data)
        {
            richTextBox2.AppendText(data + Environment.NewLine);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "";
                openFileDialog.Filter = "所有文件|";
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName != "")
                {
                    listObj.Clear();
                    //listObj.Add(hWindowControl1.HalconWindow);
                    listObj.Add(openFileDialog.FileName);
                    HalconDotNet.HTuple hTuple = new HalconDotNet.HTuple(hWindowControl1.HalconID);
                    listObj.Add(hTuple);
                    cSharpCode.DebugCode(richTextBox3.Text, "ReadImage", out errs, listObj.ToArray());
                    richTextBox2.AppendText(errs + Environment.NewLine);
                }
            }
            catch (Exception)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listObj.Clear();
            //listObj.Add(hWindowControl1.HalconWindow);
            cSharpCode.DebugCode(richTextBox3.Text, "DrawReng", out errs, listObj.ToArray());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                listObj.Clear();
                //listObj.Add(hWindowControl1.HalconWindow);
                AddTextNewLine(cSharpCode.DebugCode(richTextBox3.Text, "Find", out errs, listObj.ToArray()).ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                cSharpCode.DebugCode(richTextBox3.Text, "New", out errs);
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void toolStripSplitButton1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripSplitButton1.ShowDropDown();
        }

        private void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
        }
    }
}