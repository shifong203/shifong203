namespace ErosSocket.DebugPLC.Robot
{
    partial class ExecnteControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecnteControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsButton1 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.tsButton4 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.tsButton2 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.tsButton3 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.tsButton5 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.execnte_Program_Control1 = new ErosSocket.DebugPLC.Robot.Execnte_Program_Control();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButton1,
            this.tsButton4,
            this.tsButton2,
            this.tsButton3,
            this.tsButton5});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(635, 25);
            this.toolStrip1.TabIndex = 126;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsButton1
            // 
            this.tsButton1.Image = ((System.Drawing.Image)(resources.GetObject("tsButton1.Image")));
            this.tsButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton1.Name = "tsButton1";
            this.tsButton1.Size = new System.Drawing.Size(76, 22);
            this.tsButton1.Text = "单步模式";
            this.tsButton1.Click += new System.EventHandler(this.tsButton1_Click);
            // 
            // tsButton4
            // 
            this.tsButton4.Image = ((System.Drawing.Image)(resources.GetObject("tsButton4.Image")));
            this.tsButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton4.Name = "tsButton4";
            this.tsButton4.Size = new System.Drawing.Size(52, 22);
            this.tsButton4.Text = "执行";
            this.tsButton4.Click += new System.EventHandler(this.tsButton4_Click);
            // 
            // tsButton2
            // 
            this.tsButton2.Image = ((System.Drawing.Image)(resources.GetObject("tsButton2.Image")));
            this.tsButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton2.Name = "tsButton2";
            this.tsButton2.Size = new System.Drawing.Size(52, 22);
            this.tsButton2.Text = "停止";
            this.tsButton2.Click += new System.EventHandler(this.tsButton2_Click);
            // 
            // tsButton3
            // 
            this.tsButton3.Image = ((System.Drawing.Image)(resources.GetObject("tsButton3.Image")));
            this.tsButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton3.Name = "tsButton3";
            this.tsButton3.Size = new System.Drawing.Size(52, 22);
            this.tsButton3.Text = "跳出";
            this.tsButton3.Click += new System.EventHandler(this.tsButton3_Click);
            // 
            // tsButton5
            // 
            this.tsButton5.BackColor = System.Drawing.SystemColors.Control;
            this.tsButton5.Image = ((System.Drawing.Image)(resources.GetObject("tsButton5.Image")));
            this.tsButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton5.Name = "tsButton5";
            this.tsButton5.Size = new System.Drawing.Size(76, 22);
            this.tsButton5.Text = "离线模式";
            this.tsButton5.Click += new System.EventHandler(this.tsButton5_Click);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 25);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(139, 355);
            this.listBox1.TabIndex = 127;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(139, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(496, 355);
            this.tabControl1.TabIndex = 128;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.execnte_Program_Control1);
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(488, 329);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // execnte_Program_Control1
            // 
            this.execnte_Program_Control1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.execnte_Program_Control1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.execnte_Program_Control1.Location = new System.Drawing.Point(3, 3);
            this.execnte_Program_Control1.Name = "execnte_Program_Control1";
            this.execnte_Program_Control1.OffSetX = 10;
            this.execnte_Program_Control1.OffSetY = 10;
            this.execnte_Program_Control1.Scaling = 1F;
            this.execnte_Program_Control1.Size = new System.Drawing.Size(482, 232);
            this.execnte_Program_Control1.TabIndex = 126;
            this.execnte_Program_Control1.XVale = 10F;
            this.execnte_Program_Control1.YVale = 10F;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox1.Location = new System.Drawing.Point(3, 235);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(482, 91);
            this.richTextBox1.TabIndex = 125;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // ExecnteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ExecnteControl";
            this.Size = new System.Drawing.Size(635, 380);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton tsButton1;
        private Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton tsButton4;
        private Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton tsButton2;
        private Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton tsButton3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton tsButton5;
        private Execnte_Program_Control execnte_Program_Control1;
    }
}
