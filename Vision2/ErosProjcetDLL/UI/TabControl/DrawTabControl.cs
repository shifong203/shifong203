using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.UI
{
    #region 重绘tablecontrol的类(添加关闭页面功能),ablepage的创建，以及窗体的附加

    public class DrawTabControl
    {
        private TabControl tabControl1 = null;
        private Font font1 = null;

        //打开和关闭的页面索引
        private int tabindex_show = 0;

        private int tabindex_close = 0;

        public DrawTabControl()
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tabcontrol">TacControl控件</param>
        /// <param name="fo">主程序this.Font</param>
        public DrawTabControl(TabControl tabcontrol, Font fo)
        {
            tabControl1 = tabcontrol;
            font1 = fo;
        }

        #region 关于tabcontrol的重绘

        private const int CLOSE_SIZE = 15;//大小
        //tabPage标签图片
        //Bitmap image = new Bitmap();

        public void ClearPage()
        {
            //清空控件
            //this.MainTabControl.TabPages.Clear();
            //绘制的方式OwnerDrawFixed表示由窗体绘制大小也一样
            this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Padding = new System.Drawing.Point(CLOSE_SIZE, CLOSE_SIZE - 8);
            this.tabControl1.DrawItem += new DrawItemEventHandler(this.tabControl1_DrawItem);
            this.tabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseDown);
        }

        /// <summary>
        /// 产生新的窗体时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                Rectangle myTabRect = this.tabControl1.GetTabRect(e.Index);

                //先添加TabPage属性
                e.Graphics.DrawString(this.tabControl1.TabPages[e.Index].Text, this.font1, SystemBrushes.ControlText, myTabRect.X + 2, myTabRect.Y + 2);

                //再画一个矩形框
                using (Pen p = new Pen(Color.White))
                {
                    myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                    myTabRect.Width = CLOSE_SIZE;
                    myTabRect.Height = CLOSE_SIZE;
                    e.Graphics.DrawRectangle(p, myTabRect);
                }

                ////填充矩形框
                Color recColor = e.State == DrawItemState.Selected ? Color.White : Color.White;
                using (Brush b = new SolidBrush(recColor))
                {
                    e.Graphics.FillRectangle(b, myTabRect);
                }

                //画关闭符号
                using (Pen objpen = new Pen(Color.Black))
                {
                    ////=============================================
                    //自己画X
                    ////"\"线
                    Point p1 = new Point(myTabRect.X + 3, myTabRect.Y + 3);
                    Point p2 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + myTabRect.Height - 3);
                    e.Graphics.DrawLine(objpen, p1, p2);
                    ////"/"线
                    Point p3 = new Point(myTabRect.X + 3, myTabRect.Y + myTabRect.Height - 3);
                    Point p4 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + 3);
                    e.Graphics.DrawLine(objpen, p3, p4);

                    ////=============================================
                    //使用图片
                    //Bitmap bt = new Bitmap(image);
                    //Point p5 = new Point(myTabRect.X, 4);
                    //e.Graphics.DrawImage(bt, p5);
                    //e.Graphics.DrawString(this.MainTabControl.TabPages[e.Index].Text, this.font1, objpen.Brush, p5);
                }
                e.Graphics.Dispose();
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// 鼠标点击选项卡时的触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    int x = e.X, y = e.Y;
                    //计算关闭区域
                    Rectangle myTabRect = this.tabControl1.GetTabRect(this.tabControl1.SelectedIndex);

                    myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                    myTabRect.Width = CLOSE_SIZE;
                    myTabRect.Height = CLOSE_SIZE;

                    //如果鼠标在区域内就关闭选项卡,如果不在就显示选项卡
                    //如果关闭的选项卡正好打开着，则关闭当前先项卡，显示下一个选项卡
                    bool isClose = x > myTabRect.X && x < myTabRect.Right && y > myTabRect.Y && y < myTabRect.Bottom;
                    if (isClose == true)
                    {
                        //判断关闭的页面索引是否比正在显示的页面的索引
                        //如果关闭的页面的索引大于或等于显示正在显示的页面索引，刚tabindex_close的值为关闭的页面的索引;
                        //相反 的，刚tabindex_close则为tabindex_show-1;

                        tabindex_close = this.tabControl1.SelectedIndex >= tabindex_show ? this.tabControl1.SelectedIndex : tabindex_close - 1;
                        this.tabControl1.TabPages.Remove(this.tabControl1.SelectedTab);
                        if (tabControl1.TabPages.Count == 0)
                        {
                            tabControl1.Visible = false;
                        }
                    }
                    else
                    {
                        tabindex_close = this.tabControl1.SelectedIndex;
                        tabindex_show = this.tabControl1.SelectedIndex;
                    }
                    if (tabindex_close < tabindex_show)
                    {
                        tabindex_show = tabindex_show - 1;
                    }
                    //显示页面
                    try { this.tabControl1.SelectedTab = this.tabControl1.TabPages[tabindex_show]; }
                    catch (Exception ex) { }

                    Console.WriteLine("显示页面" + tabindex_show + "关闭页面" + tabindex_close);
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion 关于tabcontrol的重绘

        #region 关于tablepage的创建，以及窗体的附加

        /// <summary>
        /// 创建新的选项卡
        /// </summary>
        /// <param name="ParentForm">父窗体</param>
        /// <param name="ChildForm">子窗体</param>
        /// <param name="Pagename">页面名称</param>
        public void CreatePage(Form ParentForm, Form ChildForm, string Pagename)
        {
            ChildForm.MdiParent = ParentForm;
            ChildForm.Text = "第二个窗体";
            ChildForm.Dock = DockStyle.Fill;
            ChildForm.TopMost = true;
            ChildForm.FormBorderStyle = FormBorderStyle.None;
            TabPage tb1 = AddPage(Pagename);
            tb1.Controls.Add(ChildForm);
            this.tabControl1.Controls.Add(tb1);
            ChildForm.Show();
        }

        /// <summary>
        /// 添加选项卡
        /// </summary>
        /// <param name="PageName">页面名称</param>
        /// <returns>一个页面</returns>
        public TabPage AddPage(string PageName)
        {
            TabPage tabPage = new TabPage();
            tabPage.Location = new System.Drawing.Point(4, 21);
            tabPage.Name = PageName;
            tabPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage.Size = new System.Drawing.Size(658, 410);
            tabPage.Text = PageName;
            tabPage.UseVisualStyleBackColor = true;
            return tabPage;
        }

        #endregion 关于tablepage的创建，以及窗体的附加
    }

    #endregion 重绘tablecontrol的类(添加关闭页面功能),ablepage的创建，以及窗体的附加
}