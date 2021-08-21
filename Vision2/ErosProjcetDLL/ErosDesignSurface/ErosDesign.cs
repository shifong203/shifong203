//using System;
//using System.Drawing.Design;
//using System.Windows.Forms;

//namespace Vision2.ErosProjcetDLL.ErosDesignSurface
//{
//    public partial class ErosDesign : Form
//    {
//        public ErosDesign()
//        {
//            InitializeComponent();
//            CustomInitialize();
//        }

//        private void CustomInitialize()
//        {
//            _hostSurfaceManager = new HostSurfaceManager();
//            _hostSurfaceManager.AddService(typeof(IToolboxService), this.toolbox);
//            //_hostSurfaceManager.AddService(typeof(ToolWindows.SolutionExplorer), this.solutionExplorer1);
//            //_hostSurfaceManager.AddService(typeof(ToolWindows.OutputWindow), this.OutputWindow);
//            //_hostSurfaceManager.AddService(typeof(System.Windows.Forms.PropertyGrid), this.propertyGrid1);

//            //this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
//        }

//        private HostSurfaceManager _hostSurfaceManager = null;

//        private void 窗体ToolStripMenuItem_Click(object sender, EventArgs e)
//        {
//            //引用System.Deisgn.dll
//            //DesignSurface ds = new DesignSurface();

//            // ds.BeginLoad(typeof(Form));
//            //Control designerContorl = (Control)ds.View;
//            //designerContorl.Dock = DockStyle.Fill;
//            //this.Controls.Add(designerContorl);
//            HostControl hc = _hostSurfaceManager.GetNewHost(typeof(Form), LoaderType.BasicDesignerLoader);
//            AddTabForNewHost("Form - " + Strings.Design, hc);
//        }

//        private void AddTabForNewHost(string tabText, HostControl hc)
//        {
//            this.toolbox.DesignerHost = hc.DesignerHost;
//            TabPage tabpage = new TabPage(tabText);
//            tabpage.Tag = CurrentMenuSelectionLoaderType;
//            hc.Parent = tabpage;
//            hc.Dock = DockStyle.Fill;
//            this.tabControl1.TabPages.Add(tabpage);
//            this.tabControl1.SelectedIndex = this.tabControl1.TabPages.Count - 1;
//            _hostSurfaceManager.ActiveDesignSurface = hc.HostSurface;
//            //if (CurrentActiveDocumentLoaderType == LoaderType.CodeDomDesignerLoader)
//            //    this.eMenuItem.Enabled = true;
//            //else
//            //    this.eMenuItem.Enabled = false;
//            //this.solutionExplorer1.AddFileNode(tabText);
//        }

//        private LoaderType CurrentMenuSelectionLoaderType
//        {
//            get
//            {
//                //if (this.basicDesignerLoaderMenuItem.Checked)
//                //    return LoaderType.BasicDesignerLoader;
//                //else if (/*this.codeDomDesignerLoaderMenuItem.Checked)*/
//                //    return LoaderType.CodeDomDesignerLoader;
//                //else
//                return LoaderType.NoLoader;
//            }
//        }

//        private LoaderType CurrentActiveDocumentLoaderType
//        {
//            get
//            {
//                TabPage tabPage = this.tabControl1.TabPages[this.tabControl1.SelectedIndex];

//                return (LoaderType)tabPage.Tag;
//            }
//        }

//        private string CurrentDocumentView
//        {
//            get
//            {
//                TabPage tabPage = this.tabControl1.TabPages[this.tabControl1.SelectedIndex];

//                if (tabPage.Text.Contains(Strings.Design))
//                    return Strings.Design;
//                else
//                    return Strings.Code;
//            }
//        }

//        private class Strings
//        {
//            public const string Design = "Design";
//            public const string Code = "Code";
//            public const string Xml = "Xml";
//            public const string CS = "C#";
//            public const string JS = "J#";
//            public const string VB = "VB";
//        }
//    }
//}