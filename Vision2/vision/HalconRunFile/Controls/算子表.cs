using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using HWND = System.IntPtr;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class 算子表 : Form
    {
        public 算子表()
        {
            InitializeComponent();
            panel1.Paint += Panel1_Paint;
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            showLineNo();
        }

        public List<string> richTexs = new List<string>();

        [DllImport("user32")]
        private static extern int SendMessage(HWND hwnd, int wMsg, Int32 wParam, IntPtr lParam);

        private const int WM_SETREDRAW = 0xB;

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                showLineNo();
                BtnSeveFile.Enabled = true;
                //光标所在列
                int Col = (1 + richTextBox1.SelectionStart - (richTextBox1.GetFirstCharIndexFromLine(1 + richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart) - 1)));
                //光标所在行
                int Row = (1 + richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart));
                int length = 0;
                //添加空格
                richTexs.Clear();
                richTexs.AddRange(richTextBox1.Lines);
                for (int i = 0; i < richTexs.Count; i++)
                {
                    if (richTexs[i].Length < 100)
                    {
                        for (int i1 = 0; i1 < 100 - richTexs[i].Length; i1++)
                        {
                            richTexs[i] += "  ";
                        }
                    }
                }
                string newStr = "";
                for (int i = 0; i < richTexs.Count; i++)
                {
                    if (i == 0)
                    {
                        newStr += richTexs[0];
                    }
                    else { newStr += "\n" + richTexs[i]; }
                }
                richTextBox1.Text = "";
                richTextBox1.Text = newStr;
                //改变全部字体颜色
                richTextBox1.ForeColor = Color.Red;
                int ste = 0;
                //改变关键字颜色
                for (int i = 0; i < richTextBox1.Lines.Length; i++)
                {
                    //改变算子关键字颜色
                    for (int i1 = 0; i1 < OperatorKeyword.Count; i1++)
                    {
                        if (richTextBox1.Lines[i].Contains(OperatorKeyword[i1]))
                        {
                            int t = richTextBox1.Lines[i].IndexOf(OperatorKeyword[i1]);
                            if (richTextBox1.Lines[i].Contains(")")) //找到结束点
                            {
                                richTextBox1.Select(ste + t, richTextBox1.Lines[i].IndexOf(")") - t + 1);
                            }
                            else
                            {
                                richTextBox1.Select(ste + t, OperatorKeyword[i1].Length);
                            }
                            richTextBox1.SelectionColor = Color.Blue;
                        }
                    }
                    //改变函数关键字颜色
                    for (int i1 = 0; i1 < SymbolKeyword.Count; i1++)
                    {
                        if (richTextBox1.Lines[i].Contains(SymbolKeyword[i1]))
                        {
                            richTextBox1.Select(ste + richTextBox1.Lines[i].IndexOf(SymbolKeyword[i1]), SymbolKeyword[i1].Length);
                            richTextBox1.SelectionColor = Color.LightSkyBlue;
                        }
                    }
                    //改变变量关键字颜色
                    for (int i1 = 0; i1 < VariableKeyWord.Count; i1++)
                    {
                        if (richTextBox1.Lines[i].Contains(VariableKeyWord[i1]))
                        {
                            richTextBox1.Select(ste + richTextBox1.Lines[i].IndexOf(VariableKeyWord[i1]), VariableKeyWord[i1].Length);
                            richTextBox1.SelectionColor = Color.LightSalmon;
                        }
                    }
                    //改变注释关键字颜色
                    for (int i1 = 0; i1 < NotesKeyword.Count; i1++)
                    {
                        if (richTextBox1.Lines[i].Contains(NotesKeyword[i1]))
                        {
                            richTextBox1.Select(ste + richTextBox1.Lines[i].IndexOf(NotesKeyword[i1]), richTextBox1.Lines[i1].Length - richTextBox1.Lines[i].IndexOf(NotesKeyword[i1]) - 10);
                            richTextBox1.SelectionColor = Color.LimeGreen;
                        }
                    }

                    if (Row == i + 1) //光标等于当前行获取当前光标位置
                    {
                        length = ste + Col - Row;
                    }
                    ste += richTextBox1.Lines[i].Length + 1;
                }
                richTextBox1.Select(length + Row - 1, 0);
            }
            catch (Exception es)
            {
                MessageBox.Show(es.ToString());
            }
            //for (int i = 0; i < richTextBox1.Lines.Length; i++)
            //{
            //    if (i == 0)
            //    {
            //        richTextBox2.Text += richTextBox1.Lines[0];
            //    }
            //    else { richTextBox2.Text += "\n" + richTextBox1.Lines[i]; }
            //}
            //richTextBox2 = richTextBox1;
        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            showLineNo();
        }

        /// <summary>
        /// 显示文本行号
        /// </summary>
        private void showLineNo()
        {
            //获得当前坐标信息
            Point p = this.richTextBox1.Location;
            int crntFirstIndex = this.richTextBox1.GetCharIndexFromPosition(p);

            int crntFirstLine = this.richTextBox1.GetLineFromCharIndex(crntFirstIndex);

            Point crntFirstPos = this.richTextBox1.GetPositionFromCharIndex(crntFirstIndex);

            p.Y += this.richTextBox1.Height;

            int crntLastIndex = this.richTextBox1.GetCharIndexFromPosition(p);

            int crntLastLine = this.richTextBox1.GetLineFromCharIndex(crntLastIndex);
            Point crntLastPos = this.richTextBox1.GetPositionFromCharIndex(crntLastIndex);

            //准备画图
            Graphics g = this.panel1.CreateGraphics();

            Font font = new Font(this.richTextBox1.Font, this.richTextBox1.Font.Style);
            font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            SolidBrush brush = new SolidBrush(Color.Blue);

            //画图开始

            //刷新画布

            Rectangle rect = this.panel1.ClientRectangle;
            brush.Color = this.panel1.BackColor;
            SolidBrush brusht = new SolidBrush(Color.Blue);
            brusht.Color = Color.GreenYellow;

            g.FillRectangle(brush, 0, 0, this.panel1.ClientRectangle.Width, this.panel1.ClientRectangle.Height);
            g.FillRectangle(brusht, 20, 0, 1, this.panel1.ClientRectangle.Height);
            brush.Color = Color.Blue;//重置画笔颜色

            //绘制行号

            int lineSpace = 0;

            if (crntFirstLine != crntLastLine)
            {
                lineSpace = (crntLastPos.Y - crntFirstPos.Y) / (crntLastLine - crntFirstLine);
            }
            else
            {
                lineSpace = Convert.ToInt32(this.richTextBox1.Font.Size);
            }

            int brushX = this.panel1.ClientRectangle.Width - Convert.ToInt32(font.Size * 2);

            int brushY = crntLastPos.Y + Convert.ToInt32(font.Size * 0.21f);//惊人的算法啊！！

            for (int i = crntLastLine; i > -1; i--)
            {
                g.DrawString((i + 1).ToString(), font, brush, brushX, brushY);
                brushY -= lineSpace;
            }

            g.Dispose();

            font.Dispose();

            brush.Dispose();
        }

        /// <summary>
        /// 视觉算子程序地址
        /// </summary>
        private string ProguaPath = "";

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (ProguaPath == "") //程序地址
            {
                open.InitialDirectory = Application.StartupPath;
            }
            open.Filter = "txt文件|*.txt";
            if (open.ShowDialog() == DialogResult.OK)
            {
                //获取项目路径
                ProguaPath = open.FileName;
                //获取项目名称
                this.Text = Path.GetFileNameWithoutExtension(open.FileName);
                richTextBox1.Text = File.ReadAllText(open.FileName, Encoding.UTF8);
            }
        }

        private void BtnSeveFile_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(ProguaPath))
            {
                File.WriteAllText(ProguaPath, richTextBox1.Text);
                BtnSeveFile.Enabled = false;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            算子表 编译器 = new 算子表();
            string KeywordPath = Application.StartupPath + "\\编译器\\Keyword.txt";
            try
            {
                //创建文件夹
                if (!Directory.Exists(Application.StartupPath + "\\编译器"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\编译器");
                }
                if (!File.Exists(KeywordPath))
                {
                    FileStream NewFile = File.Create(KeywordPath);
                    NewFile.Close();
                }
                编译器.richTextBox1.Text = File.ReadAllText(KeywordPath, Encoding.UTF8);
                // OperatorKeyword.Add
                编译器.toolStripButton1.Text = "保存并退出";
                编译器.Text = "编译器设置";
                // 编译器.BtnSeveFile.Enabled = false;
                if (this.toolStripButton1.Text == "编译器")
                {
                    编译器.Show();
                }
                else
                {
                    File.WriteAllText(KeywordPath, this.richTextBox1.Text);
                    MessageBox.Show("保存成功");

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 算子表_Load(object sender, EventArgs e)
        {
            OperatorKeyword.Add("read_image");
            OperatorKeyword.Add("gen_measure_arc");
            OperatorKeyword.Add("copy_obj");
            OperatorKeyword.Add("draw_rectangle2");
            OperatorKeyword.Add("gen_rectangle2");
            OperatorKeyword.Add("get_image_size");

            SymbolKeyword.Add("draw_rake");
            NotesKeyword.Add("*");
            VariableKeyWord.Add("ArcRow");
        }

        /// <summary>
        /// 算子关键字
        /// </summary>
        public List<string> OperatorKeyword = new List<string>();

        /// <summary>
        /// 函数关键字
        /// </summary>
        public List<string> SymbolKeyword = new List<string>();

        /// <summary>
        /// 注释关键字
        /// </summary>
        public List<string> NotesKeyword = new List<string>();

        /// <summary>
        /// 变量关键字
        /// </summary>
        public List<string> VariableKeyWord = new List<string>();

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

                watch.Start();
                //1）当没有对象的时候使用这种方式来获取某个类型的Type
                Type type = typeof(HOperatorSet);
                richTextBox1.Text = "";
                //Type type = assembly.GetType();
                //2获取类中的所有的方法
                dynamic dynamic = type;
                MethodInfo[] methods = type.GetMethods();

                var dicSort = from objDic in methods
                              where objDic.Name == "DispObj"
                              select objDic;
                //methods= dicSort.ToArray();
                StringBuilder data = new StringBuilder();
                foreach (MethodInfo item in dicSort)
                {
                    data.AppendLine(item.Name + ";");
                    //item.Invoke(obj, new string[] { "test" });
                }
                //for (int i = 0; i < methods.Length; i++)
                //{
                //    data.AppendLine(methods[i].Name + ";" );
                //}
                richTextBox1.Text = data.ToString();
                richTextBox1.Text += "运行时间:" + watch.ElapsedMilliseconds + "MS";
                watch.Stop();
                //3获取某个类型的所有属性
                PropertyInfo[] properties = type.GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    Console.WriteLine(properties[i].Name);
                }
                Console.ReadKey();
                //4获取类中的所有字段,私有字段无法获取
                FieldInfo[] fields = type.GetFields();
                for (int i = 0; i < fields.Length; i++)
                {
                }
                //5获取所有成员，不包含私有成员
                MemberInfo[] members = type.GetMembers();
                for (int i = 0; i < members.Length; i++)
                {
                    Console.WriteLine(members[i].Name);
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
            }
        }

        private void richTextBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //光标所在列
            int Col = (1 + richTextBox1.SelectionStart - (richTextBox1.GetFirstCharIndexFromLine(1 + richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart) - 1)));
            //光标所在行
            int Row = (1 + richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart));
            if (richTextBox1.Lines[Row - 1].Contains(":="))
            {
                string[] dataStrs = richTextBox1.Lines[Row - 1].Split(';');
                string dataStr = dataStrs[0].Trim(' ');
            }
        }
    }
}