namespace Vision2.vision.HalconRunFile.Controls
{
    partial class DrawContrlos
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawContrlos));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsButton4 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.toolStripTrackBar1 = new Vision2.ErosProjcetDLL.UI.ToolStrip.ToolStripTrackBar();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.绘制区域ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制椭圆ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制方矩形ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制角度矩形ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制圆弧ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制NBSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsButton1 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.tsButton3 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.tsButton5 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.tsButton2 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.tsButton6 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripCheckbox1 = new Vision2.ErosProjcetDLL.UI.ToolStrip.ToolStripCheckbox();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButton4,
            this.toolStripTrackBar1,
            this.toolStripLabel1,
            this.toolStripSplitButton1,
            this.tsButton1,
            this.tsButton3,
            this.tsButton5,
            this.tsButton2,
            this.tsButton6,
            this.toolStripComboBox1,
            this.toolStripCheckbox1,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(771, 48);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "绘制区域";
            // 
            // tsButton4
            // 
            this.tsButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsButton4.Image = ((System.Drawing.Image)(resources.GetObject("tsButton4.Image")));
            this.tsButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton4.IsCher = false;
            this.tsButton4.Name = "tsButton4";
            this.tsButton4.Size = new System.Drawing.Size(45, 45);
            this.tsButton4.Text = "测试";
            this.tsButton4.Click += new System.EventHandler(this.tsButton4_Click);
            // 
            // toolStripTrackBar1
            // 
            this.toolStripTrackBar1.Name = "toolStripTrackBar1";
            this.toolStripTrackBar1.Size = new System.Drawing.Size(104, 45);
            this.toolStripTrackBar1.Text = "toolStripTrackBar1";
            this.toolStripTrackBar1.Click += new System.EventHandler(this.toolStripTrackBar1_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(27, 45);
            this.toolStripLabel1.Text = "10";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.绘制区域ToolStripMenuItem,
            this.绘制椭圆ToolStripMenuItem,
            this.绘制方矩形ToolStripMenuItem,
            this.绘制角度矩形ToolStripMenuItem,
            this.绘制圆弧ToolStripMenuItem,
            this.绘制点ToolStripMenuItem,
            this.绘制ToolStripMenuItem,
            this.绘制NBSToolStripMenuItem});
            this.toolStripSplitButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(38, 45);
            this.toolStripSplitButton1.Text = "圆";
            this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
            // 
            // 绘制区域ToolStripMenuItem
            // 
            this.绘制区域ToolStripMenuItem.Name = "绘制区域ToolStripMenuItem";
            this.绘制区域ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制区域ToolStripMenuItem.Text = "绘制区域";
            this.绘制区域ToolStripMenuItem.Click += new System.EventHandler(this.绘制区域ToolStripMenuItem_Click);
            // 
            // 绘制椭圆ToolStripMenuItem
            // 
            this.绘制椭圆ToolStripMenuItem.Name = "绘制椭圆ToolStripMenuItem";
            this.绘制椭圆ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制椭圆ToolStripMenuItem.Text = "绘制椭圆";
            this.绘制椭圆ToolStripMenuItem.Click += new System.EventHandler(this.绘制椭圆ToolStripMenuItem_Click);
            // 
            // 绘制方矩形ToolStripMenuItem
            // 
            this.绘制方矩形ToolStripMenuItem.Name = "绘制方矩形ToolStripMenuItem";
            this.绘制方矩形ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制方矩形ToolStripMenuItem.Text = "绘制方矩形";
            this.绘制方矩形ToolStripMenuItem.Click += new System.EventHandler(this.绘制方矩形ToolStripMenuItem_Click);
            // 
            // 绘制角度矩形ToolStripMenuItem
            // 
            this.绘制角度矩形ToolStripMenuItem.Name = "绘制角度矩形ToolStripMenuItem";
            this.绘制角度矩形ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制角度矩形ToolStripMenuItem.Text = "绘制角度矩形";
            this.绘制角度矩形ToolStripMenuItem.Click += new System.EventHandler(this.绘制角度矩形ToolStripMenuItem_Click);
            // 
            // 绘制圆弧ToolStripMenuItem
            // 
            this.绘制圆弧ToolStripMenuItem.Name = "绘制圆弧ToolStripMenuItem";
            this.绘制圆弧ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制圆弧ToolStripMenuItem.Text = "绘制圆弧";
            this.绘制圆弧ToolStripMenuItem.Click += new System.EventHandler(this.绘制圆弧ToolStripMenuItem_Click);
            // 
            // 绘制点ToolStripMenuItem
            // 
            this.绘制点ToolStripMenuItem.Name = "绘制点ToolStripMenuItem";
            this.绘制点ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制点ToolStripMenuItem.Text = "绘制点";
            this.绘制点ToolStripMenuItem.Click += new System.EventHandler(this.绘制点ToolStripMenuItem_Click);
            // 
            // 绘制ToolStripMenuItem
            // 
            this.绘制ToolStripMenuItem.Name = "绘制ToolStripMenuItem";
            this.绘制ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制ToolStripMenuItem.Text = "绘制直线";
            this.绘制ToolStripMenuItem.Click += new System.EventHandler(this.绘制ToolStripMenuItem_Click);
            // 
            // 绘制NBSToolStripMenuItem
            // 
            this.绘制NBSToolStripMenuItem.Name = "绘制NBSToolStripMenuItem";
            this.绘制NBSToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制NBSToolStripMenuItem.Text = "绘制NBS";
            this.绘制NBSToolStripMenuItem.Click += new System.EventHandler(this.绘制NBSToolStripMenuItem_Click);
            // 
            // tsButton1
            // 
            this.tsButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsButton1.Image = global::Vision2.Properties.Resources.brush;
            this.tsButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton1.IsCher = false;
            this.tsButton1.Name = "tsButton1";
            this.tsButton1.Size = new System.Drawing.Size(68, 45);
            this.tsButton1.Text = "绘制掩模";
            this.tsButton1.Click += new System.EventHandler(this.tsButton1_Click);
            // 
            // tsButton3
            // 
            this.tsButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsButton3.Image = global::Vision2.Properties.Resources.cut;
            this.tsButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton3.IsCher = false;
            this.tsButton3.Name = "tsButton3";
            this.tsButton3.Size = new System.Drawing.Size(68, 45);
            this.tsButton3.Text = "擦除掩模";
            this.tsButton3.Click += new System.EventHandler(this.tsButton3_Click);
            // 
            // tsButton5
            // 
            this.tsButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsButton5.Image = ((System.Drawing.Image)(resources.GetObject("tsButton5.Image")));
            this.tsButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton5.IsCher = false;
            this.tsButton5.Name = "tsButton5";
            this.tsButton5.Size = new System.Drawing.Size(45, 45);
            this.tsButton5.Text = "移动";
            this.tsButton5.Click += new System.EventHandler(this.tsButton5_Click);
            // 
            // tsButton2
            // 
            this.tsButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsButton2.Image = ((System.Drawing.Image)(resources.GetObject("tsButton2.Image")));
            this.tsButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton2.IsCher = false;
            this.tsButton2.Name = "tsButton2";
            this.tsButton2.Size = new System.Drawing.Size(45, 45);
            this.tsButton2.Text = "清除";
            this.tsButton2.Click += new System.EventHandler(this.tsButton2_Click_1);
            // 
            // tsButton6
            // 
            this.tsButton6.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsButton6.Image = ((System.Drawing.Image)(resources.GetObject("tsButton6.Image")));
            this.tsButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton6.IsCher = false;
            this.tsButton6.Name = "tsButton6";
            this.tsButton6.Size = new System.Drawing.Size(61, 45);
            this.tsButton6.Text = "帮助";
            this.tsButton6.Click += new System.EventHandler(this.tsButton6_Click_1);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(80, 48);
            this.toolStripComboBox1.DropDownClosed += new System.EventHandler(this.toolStripComboBox1_DropDownClosed);
            // 
            // toolStripCheckbox1
            // 
            this.toolStripCheckbox1.Name = "toolStripCheckbox1";
            this.toolStripCheckbox1.Size = new System.Drawing.Size(60, 45);
            this.toolStripCheckbox1.Text = "隐藏";
            this.toolStripCheckbox1.Click += new System.EventHandler(this.toolStripCheckbox1_Click);
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 48);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(771, 1);
            this.hWindowControl1.TabIndex = 3;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(771, 1);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(77, 45);
            this.toolStripButton1.Text = "动态参数";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // DrawContrlos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hWindowControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "DrawContrlos";
            this.Size = new System.Drawing.Size(771, 49);
            this.Load += new System.EventHandler(this.DrawContrlos_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton tsButton1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem 绘制点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制椭圆ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制圆弧ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制方矩形ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制角度矩形ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制区域ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制NBSToolStripMenuItem;
        private Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton tsButton3;
        private Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton tsButton2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton tsButton4;
        private Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton tsButton6;
        private ErosProjcetDLL.UI.ToolStrip.ToolStripTrackBar toolStripTrackBar1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        public HalconDotNet.HWindowControl hWindowControl1;
        private ErosProjcetDLL.UI.ToolStrip.ToolStripCheckbox toolStripCheckbox1;
        private ErosProjcetDLL.UI.ToolStrip.TSButton tsButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}
