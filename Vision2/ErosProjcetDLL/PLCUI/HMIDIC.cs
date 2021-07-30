using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace Vision2.ErosProjcetDLL.PLCUI
{
    public class HMIDIC : ProjectObj, ProjectNodet.IClickNodeProject
    {
        [CategoryAttribute("控制界面是否显示"), DisplayName("显示")]
        public bool Visible { get; set; }

        [CategoryAttribute("调试控制界面"), DisplayName("调试")]
        public bool Debug { get; set; }
        public Dictionary<string, PLCIntEnum> DicPLCEnum { get; set; } = new Dictionary<string, PLCIntEnum>();

        public static HMIDIC This { get; set; }
        public override string FileName { get { return "界面管理"; } }

        /// <summary>
        /// 子集合
        /// </summary>
        public new Dictionary<string, HMI> ProjectClass
        {
            //get { return HMIS; }
            //set { HMIS = value; }
            get; set;
        }

        public override string SuffixName => ".HMI";

        public override string ProjectTypeName => "HMI";
        public override string Name => "界面管理";
        public HMIDIC()
        {
            This = this;
            Text = "界面管理";
            this.Information = "控件窗体";
        }

        /// <summary>
        /// 添加程序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="project"></param>
        public override void AddProject<T>(string path, out ProjectC project)
        {
            base.AddProject<HMI>(path, out project);
        }

        public override void SaveThis(string path)
        {
            foreach (var item in ProjectClass)
            {
                item.Value.SaveControl();
            }
            base.SaveThis(path);
        }
        public override void UpProjectNode(TreeNode tree)
        {
            base.UpProjectNode(tree);
        }
        //public override void UpProperty(PropertyForm pertyForm, object data = null)
        //{
        //    base.UpProperty(pertyForm, data);
        //    TabPage tabPage1 = new TabPage();
        //    tabPage1.Name = "枚举类型";
        //    tabPage1.Text = "枚举类型";
        //    pertyForm.tabControl1.TabPages.Add(tabPage1);
        //    tabPage1.Controls.Add(new IntEnumUserControl(this.DicPLCEnum) { Dock = DockStyle.Fill });
        //}

        public Control GetThisControl()
        {
            return new IntEnumUserControl(this.DicPLCEnum);
        }
        public override void initialization()
        {
            if (ProjectClass == null)
            {
                ProjectClass = new Dictionary<string, HMI>();
                ProjectClass.Add("主屏幕", new HMI());
                return;
            }
            foreach (var item in ProjectClass)
            {
                item.Value.initialization();
            }
            //base.initialization();
        }

        /// <summary>
        ///
        /// </summary>
        public override void Close()
        {
            base.Close();
        }


    }
}