namespace ErosProjcetDLL.Project
{
    partial class ProjectNodetControl
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("解决方案");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectNodetControl));
            this.tVProject = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.OpenProjectButton = new System.Windows.Forms.ToolStripButton();
            this.AddProjectButton = new System.Windows.Forms.ToolStripButton();
            this.RedrawProjectButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveProjectButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tVProject
            // 
            this.tVProject.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.tVProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tVProject.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tVProject.ForeColor = System.Drawing.Color.White;
            this.tVProject.LabelEdit = true;
            this.tVProject.Location = new System.Drawing.Point(0, 25);
            this.tVProject.Margin = new System.Windows.Forms.Padding(2);
            this.tVProject.Name = "tVProject";
            treeNode1.Name = "解决方案";
            treeNode1.Text = "解决方案";
            this.tVProject.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tVProject.Size = new System.Drawing.Size(271, 476);
            this.tVProject.TabIndex = 20;
            this.tVProject.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tVProject_AfterLabelEdit);
            this.tVProject.DoubleClick += new System.EventHandler(this.tVProject_DoubleClick);
            this.tVProject.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tVProject_KeyDown);
            this.tVProject.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tVProject_MouseUp);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenProjectButton,
            this.AddProjectButton,
            this.RedrawProjectButton,
            this.toolStripSeparator1,
            this.SaveProjectButton,
            this.toolStripSeparator2,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(271, 25);
            this.toolStrip1.TabIndex = 21;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // OpenProjectButton
            // 
            this.OpenProjectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.OpenProjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.OpenProjectButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.OpenProjectButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenProjectButton.Image")));
            this.OpenProjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenProjectButton.Name = "OpenProjectButton";
            this.OpenProjectButton.Size = new System.Drawing.Size(36, 22);
            this.OpenProjectButton.Text = "打开";
            this.OpenProjectButton.Click += new System.EventHandler(this.OpenProjectButton_Click);
            // 
            // AddProjectButton
            // 
            this.AddProjectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.AddProjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AddProjectButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.AddProjectButton.Image = ((System.Drawing.Image)(resources.GetObject("AddProjectButton.Image")));
            this.AddProjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddProjectButton.Name = "AddProjectButton";
            this.AddProjectButton.Size = new System.Drawing.Size(36, 22);
            this.AddProjectButton.Text = "添加";
            this.AddProjectButton.Click += new System.EventHandler(this.AddProjectButton_Click);
            // 
            // RedrawProjectButton
            // 
            this.RedrawProjectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.RedrawProjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RedrawProjectButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.RedrawProjectButton.Image = ((System.Drawing.Image)(resources.GetObject("RedrawProjectButton.Image")));
            this.RedrawProjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RedrawProjectButton.Name = "RedrawProjectButton";
            this.RedrawProjectButton.Size = new System.Drawing.Size(36, 22);
            this.RedrawProjectButton.Text = "刷新";
            this.RedrawProjectButton.Click += new System.EventHandler(this.RedrawProjectButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // SaveProjectButton
            // 
            this.SaveProjectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.SaveProjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SaveProjectButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.SaveProjectButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveProjectButton.Image")));
            this.SaveProjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveProjectButton.Name = "SaveProjectButton";
            this.SaveProjectButton.Size = new System.Drawing.Size(36, 22);
            this.SaveProjectButton.Text = "保存";
            this.SaveProjectButton.Click += new System.EventHandler(this.SaveProjectButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton1.Text = "备份";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // ProjectNodetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tVProject);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ProjectNodetControl";
            this.Size = new System.Drawing.Size(271, 501);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TreeView tVProject;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton OpenProjectButton;
        private System.Windows.Forms.ToolStripButton AddProjectButton;
        private System.Windows.Forms.ToolStripButton RedrawProjectButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton SaveProjectButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}
