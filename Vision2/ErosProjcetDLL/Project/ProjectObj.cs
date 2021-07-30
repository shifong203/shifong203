using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using static Vision2.ErosProjcetDLL.Project.ProjectNodet;

namespace Vision2.ErosProjcetDLL.Project
{
    /// <summary>
    /// 程序框架抽象类
    /// </summary>
    public abstract class ProjectObj : ProjectC
    {
        /// <summary>
        /// 文件夹名称
        /// </summary>
        public override abstract string FileName { get; }
        ///// <summary>
        ///// 名称
        ///// </summary>
        //public override abstract string Name { get; }
        /// <summary>
        /// 文件后缀名
        /// </summary>
        public override abstract string SuffixName { get; }
        /// <summary>
        /// 类名
        /// </summary>
        public override abstract string ProjectTypeName { get; }

        public ProjectObj()
        {

        }
        public override abstract void initialization();



    }

    /// <summary>
    /// 程序集访问
    /// </summary>
    public  class ProjectC : IUpProjetNode, IDisposable
    {
        [DescriptionAttribute("程序名称。唯一程序名"), Category("程序参数"), DisplayName("程序名")]
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        [DescriptionAttribute("程序分类名"), Category("程序参数"), DisplayName("程序分类名"), Browsable(false)]
        public virtual string ProjectTypeName { get; } = "程序基类";

        [DescriptionAttribute("存储的文件夹名"), Category("程序参数"), DisplayName("文件夹名"), Browsable(false)]
        /// <summary>
        /// 文件夹名称
        /// </summary>
        public virtual string FileName { get; }

        [DescriptionAttribute("显示文本"), Category("程序参数"), DisplayName("显示文本"), Browsable(false)]
        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string Text { get; set; }

        [DescriptionAttribute("后缀名"), Category("程序参数"), DisplayName("后缀名"), Browsable(false)]
        /// <summary>
        /// 后缀名
        /// </summary>
        public virtual string SuffixName { get; }
        [DescriptionAttribute("程序类显示说明"), Category("程序参数"), DisplayName("类信息"), Browsable(false)]
        public virtual string Information { get; set; } = "程序基类,程序框架的实现";

        protected string[] ListCode { get; set; }
        [Browsable(false)]
        /// <summary>
        /// 程序集合
        /// </summary>
        public Dictionary<string, Hashtable> ProjectClass { get; set; }

        public ProjectC()
        {

        }
        public virtual void NewProject_Click(object sender, EventArgs e)
        {
            ToolStripItem toolStripItem = (ToolStripItem)sender;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Application.StartupPath;
                openFileDialog.Filter = "程序文件.eros|*.eros|文本|*.txt|所有文件|*";
                openFileDialog.ShowDialog();                   //展示对话框
                string name = openFileDialog.FileName;          //获得打开的文件的路径
                if (openFileDialog.FileName.Length != 0)
                {
                    ProjectC projectC;
                    AddProject<ProjectC>(openFileDialog.FileName, out projectC);
                }
                ContextMenuClick(toolStripItem.Text);
            }
            catch (System.Exception)
            {
            }
        }

        /// <summary>
        /// 保存到项目地址,必须调用父类
        /// </summary>
        /// <param name="name"></param>
        public virtual void SaveThis(string path = null)
        {
            if (path == null)
            {
                path = ProjectINI.ProjietPath + "Project\\" + ProjectINI.In.ProjectName + "\\" + ProjectINI.In.RunName;
            }
            ProjectINI.ClassToJsonSavePath(this, path + "\\" + FileName + "\\" + Name + SuffixName);
        }

        /// <summary>
        /// 读取Josn到实例
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="obj">对象</param>
        public virtual T ReadThis<T>(string path, T obj = default(T)) where T : new()
        {
            if (System.IO.File.Exists(path + "\\" + FileName + "\\" + Name + SuffixName))
            {
                ProjectINI.ReadPathJsonToCalss(path + "\\" + FileName + "\\" + Name + SuffixName, out obj);
            }
            if (obj == null)
            {
                obj = new T();
            }
            if (obj is ProjectC)
            {
                ProjectC SD = obj as ProjectC;
                SD.ReadCode(path + "\\" + SD.FileName + "\\CSarp.Cs");
            }
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public virtual void ReadCode(string path)
        {
            if (System.IO.File.Exists(path))
            {
                ListCode = System.IO.File.ReadAllLines(path);
            }
        }
        public string[] GetCode()
        {
            return ListCode;
        }
        public virtual void SaveCode(string[] codes, string path = null)
        {
            if (path == null)
            {
                path = ProjectINI.In.ProjectPathRun;
            }
            ListCode = codes;
            if (ListCode != null)
            {
                Directory.CreateDirectory(path + "\\" + FileName + "\\" + Name);
                System.IO.File.WriteAllLines(path + "\\" + FileName + "\\" + "CSarp.Cs", ListCode);
            }
        }
    



        protected TreeView TreeView;
        protected TreeNode Node;
        protected ContextMenuStrip contextMenuTT = new ContextMenuStrip();

        /// <summary>
        /// 初始方法
        /// </summary>
        public virtual void initialization()
        {

        }


        /// <summary>
        ///载入错误信息
        /// </summary>
        /// <param name="exception"></param>
        public void LogErr(Exception exception)
        {
            LogErr(exception.Message+exception.StackTrace);
        }
        /// <summary>
        ///载入错误信息
        /// </summary>
        /// <param name="exception"></param>
        public void LogErr(string message, Exception exception)
        {
            LogErr(message + ":" + exception.Message+ exception.StackTrace);
        }
        /// <summary>
        /// 载入错误信息
        /// </summary>
        /// <param name="text"></param>
        public void LogErr(string text)
        {
            AlarmText.LogErr(text, "类型" + this.GetType() + "名称:" + this.Name);
        }
        /// <summary>
        /// 登入文本信息
        /// </summary>
        /// <param name="text"></param>
        public void LogText(string text)
        {
            AlarmText.AddTextNewLine(this.Name + ":" + text);
        }
        /// <summary>
        /// 登入警告信息
        /// </summary>
        /// <param name="text"></param>
        public void LogWarning(string text)
        {
            AlarmText.LogWarning(this.Name, text);
        }
        /// <summary>
        /// 登入警告信息
        /// </summary>
        /// <param name="text"></param>
        public void LogIncident(string text)
        {
            AlarmText.LogIncident(this.Name, text);
        }

        public TreeNode GetNode()
        {

            Node = new TreeNode();
            Node.Name = Node.Text = this.Name;

            Node.Tag = this;
            if (contextMenuTT == null)
            {
                contextMenuTT = new ContextMenuStrip();
            }
            Node.ContextMenuStrip = contextMenuTT;

            return Node;
        }
        /// <summary>
        ///显示程序数
        /// </summary>
        /// <param name="treeNode"></param>
        public virtual void UpProjectNode(TreeNode tree)
        {
            if (tree != null)
            {
                TreeView = tree.TreeView;
            }

            Node = new TreeNode();

            if (contextMenuTT == null)
            {
                this.contextMenuTT = new ContextMenuStrip();
            }
            Node.ContextMenuStrip = contextMenuTT;
            if (this.Text == null)
            {
                this.Text = this.Name;
            }
            Node.Name = this.Name;
            Node.Text = this.Text;
            Node.Tag = this;
            if (tree != null)
            {
                if (!tree.Nodes.ContainsKey(Node.Name))
                {
                    tree.Nodes.Add(Node);
                }
                for (int i = 0; i < Node.Nodes.Count; i++)
                {
                    Node.Nodes[i].Tag = null;
                }
                Node.Nodes.Clear();
                if (!Node.IsExpanded)
                {
                    Node.Toggle();
                }
            }

        }


        /// <summary>
        /// 关闭释放资源
        /// </summary>
        public virtual void Close()
        {

        }

        /// <summary>
        /// 双击显示窗口
        /// </summary>
        /// <param name="tabPage"></param>
        /// <param name="data"></param>
        public virtual void DoubleClickUpForm(TabPage tabPage, object data = null)
        {
            CsCode.CSharpCodeUserControl socketConnectForm = new CsCode.CSharpCodeUserControl();
            socketConnectForm.UpCSharpCode(this);
            socketConnectForm.Dock = DockStyle.Fill;
            tabPage.Controls.Add(socketConnectForm);
        }

        /// <summary>
        /// 获取ContextMenu按下的按键
        /// </summary>
        /// <param name="txt"></param>
        public virtual void ContextMenuClick(string txt)
        {
        }

        /// <summary>
        /// 新建程序接口
        /// </summary>
        /// <param name="txt"></param>
        public virtual void NewProject(object sender, EventArgs e)
        {
            NewPragram newPragram = new NewPragram();
            newPragram.ShowDialog();

        }

        /// <summary>
        /// 添加程序接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="Project"></param>
        public virtual void AddProject<T>(string path, out ProjectC Project) where T : new()
        {
            Project = new ProjectC();
            T dynamic = default(T);
            ReadThis(path, dynamic);
        }
        /// <summary>
        /// 释放对象
        /// </summary>
        public virtual void Dispose()
        {
            if (contextMenuTT != null)
            {
                contextMenuTT.Dispose();
            }
            //if (TreeView!=null)
            //{
            //    TreeView.Dispose();
            //}
            contextMenuTT = null;

            if (ProjectClass != null)
            {
                foreach (var item in ProjectClass)
                {
                    item.Value.Clear();
                }
                ProjectClass.Clear();
            }
            ProjectClass = null;
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 超时执行
        /// </summary>
        /// <param name="action">委托</param>
        /// <param name="timeoutMilliseconds">超时时间</param>
        /// <returns>是否成功</returns>
        public static bool CallWithTimeout(Action action, int timeoutMilliseconds)
        {
            Thread threadToKill = null;
            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                action();
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                wrappedAction.EndInvoke(result);
                return true;
            }
            else
            {
                threadToKill.Abort();
                return false;
                throw new TimeoutException();
            }
        }

    }
}