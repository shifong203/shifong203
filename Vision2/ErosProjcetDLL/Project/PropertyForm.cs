using System;
using System.Collections;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Project
{
    /// <summary>
    /// 属性界面窗体
    /// </summary>
    public partial class PropertyForm : Form
    {
        public PropertyForm(Control contro) : this()
        {
            //Control = contro;
            this.TopLevel = false;
            this.Name = "PT";
            contro.Controls.Add(this);
        }

        private void PropertyForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShouHelp();
            e.Cancel = true;
        }

        public PropertyForm()
        {
            InitializeComponent();
            this.HelpButtonClicked += PropertyForm_HelpButtonClicked;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | //不擦除背景 ,减少闪烁
          ControlStyles.OptimizedDoubleBuffer | //双缓冲
             ControlStyles.UserPaint, //使用自定义的重绘事件,减少闪烁
            true);
            this.TopLevel = false;
        }

        public new static void Close()
        {
            if (ThisForm != null)
            {
                ThisForm.Dispose();
                ThisForm = null;
            }
        }

        private ProjectNodet.IClickNodeProject attributeUI;

        public static PropertyForm ThisForm
        {
            get
            {
                if (form == null || form.IsDisposed)
                {
                    form = new PropertyForm();
                    form.TopLevel = false;
                    ProjectINI.Form().Controls.Add(form);
                    form.Location = new System.Drawing.Point(800, 0);
                }
                return form;
            }

            set { form = value; }
        }

        private static PropertyForm form;

        /// <summary>
        /// 对象集合
        /// </summary>
        public Hashtable hashtable;

        private void propertyGrid1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 更新属性到属性窗体
        /// </summary>
        /// <param name="data">对象实体</param>
        /// <param name="text">对象名称</param>
        public static void UPProperty(object data, object superclass = null)
        {
            try
            {
                ThisForm.Data = data;
                ProjectINI.Form().Invoke(new Action(() =>
                {
                    ThisForm.BringToFront();
                    ThisForm.TopMost = true;
                    ThisForm.Show();
                    ThisForm.attributeUI = data as ProjectNodet.IClickNodeProject;
                    if (ThisForm.attributeUI != null)
                    {
                        ThisForm.tabPage1.Text = ThisForm.attributeUI.Name + "属性";
                    }
                    ThisForm.Controls.Clear();
                    ThisForm.Controls.Add(ThisForm.tabControl1);
                    int ds = ThisForm.tabControl1.TabPages.Count - 1;
                    for (int i = 0; i < ds; i++)
                    {
                        ThisForm.tabControl1.TabPages.RemoveAt(1);
                    }
                    if (ThisForm.attributeUI != null)
                    {
                        ThisForm.propertyGrid1.Visible = true;
                        TabPage tabPage = new TabPage();
                        tabPage.Text = tabPage.Name = "调试窗口";
                        ThisForm.tabControl1.TabPages.Add(tabPage);

                        tabPage.Controls.Add(ThisForm.attributeUI.GetThisControl());
                        ThisForm.tabControl1.SelectedTab = tabPage;
                    }
                    else
                    {
                        ThisForm.tabPage1.Controls.Clear();
                    }
                    if (ProjectINI.Enbt)
                    {
                        ThisForm.tabPage1.Controls.Add(ThisForm.propertyGrid1);
                        //ThisForm.tabControl1.TabPages.Add(ThisForm.tabPage1);
                        ThisForm.propertyGrid1.SelectedObject = data;
                    }
                    else
                    {
                        ThisForm.tabControl1.TabPages.Remove(ThisForm.tabPage1);
                        ThisForm.tabPage1.Controls.Remove(ThisForm.propertyGrid1);
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="text"></param>
        /// <param name="control"></param>
        /// <param name="superclass"></param>
        public void UProperty(object data, object superclass = null)
        {
            try
            {
                Data = data;
                ProjectINI.Form().Invoke(new Action(() =>
                {
                    try
                    {
                        this.attributeUI = data as ProjectNodet.IClickNodeProject;
                        if (this.attributeUI != null)
                        {
                            this.tabPage1.Text = this.attributeUI.Name + "属性";
                            this.Text = "属性" + this.attributeUI.Name;
                        }
                        if (this.attributeUI != null)
                        {
                            TabPage tabPage = new TabPage();
                            tabPage.Text = tabPage.Name = "调试窗口";
                            this.tabControl1.TabPages.Add(tabPage);
                            tabPage.Controls.Add(this.attributeUI.GetThisControl());
                            this.tabControl1.SelectedTab = tabPage;
                        }
                        else
                        {
                            this.tabPage1.Controls.Clear();
                            this.tabPage1.Controls.Add(this.propertyGrid1);
                        }
                        //this.tabControl1.TabPages.Add(this.tabPage1);
                        this.propertyGrid1.SelectedObject = data;
                        tabControl1.Dock = DockStyle.Fill;
                    }
                    catch (Exception ex)
                    {
                        ErrForm.Show(ex);

                    }
           
                }));
            }
            catch (Exception ex)
            {
                ErrForm.Show(ex);
               
            }
        }

        private object Data;

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="text"></param>
        /// <param name="tab"></param>
        public void UPProperty(object data, string text, params TabPage[] tab)
        {
            Data = data;
            UPProperty(data, text);
            for (int i = 0; i < tab.Length; i++)
            {
                if (tabControl1.TabPages.Contains(tab[i]))
                {
                    tabControl1.TabPages[tabControl1.TabPages.IndexOf(tab[i])] = tab[i];
                }
                else
                {
                    tabControl1.TabPages.Add(tab[i]);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertyForm_Load(object sender, EventArgs e)
        {
        }

        private void PropertyForm_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void PropertyForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                ShouHelp();
            }
        }

        private void ShouHelp()
        {
            IHelp help = Data as Project.IHelp;
            if (help != null)
            {
                help.ShowHelp();
            }
            else
            {
                CHMHelp.ShowHelp();
            }
        }
    }
}