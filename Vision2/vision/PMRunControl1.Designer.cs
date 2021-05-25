namespace Vision2.vision
{
    partial class PMRunControl1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PMRunControl1));
            this.tVProject = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsButton1 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tVProject
            // 
            this.tVProject.Dock = System.Windows.Forms.DockStyle.Top;
            this.tVProject.Location = new System.Drawing.Point(0, 25);
            this.tVProject.Name = "tVProject";
            this.tVProject.Size = new System.Drawing.Size(375, 381);
            this.tVProject.TabIndex = 0;
            this.tVProject.Click += new System.EventHandler(this.tVProject_Click);
            this.tVProject.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tVProject_MouseUp);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(375, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsButton1
            // 
            this.tsButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsButton1.Image = ((System.Drawing.Image)(resources.GetObject("tsButton1.Image")));
            this.tsButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton1.Name = "tsButton1";
            this.tsButton1.Size = new System.Drawing.Size(36, 22);
            this.tsButton1.Text = "刷新";
            this.tsButton1.Click += new System.EventHandler(this.tsButton1_Click);
            // 
            // PMRunControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tVProject);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PMRunControl1";
            this.Size = new System.Drawing.Size(375, 562);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tVProject;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton tsButton1;
    }
}
