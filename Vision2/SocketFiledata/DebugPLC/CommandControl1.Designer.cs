namespace ErosSocket.DebugPLC
{
    partial class CommandControl1
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
            this.components = new System.ComponentModel.Container();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加轴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加气缸ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加轴组合ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加机器人ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加IOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 281);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(384, 363);
            this.propertyGrid1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(169, 275);
            this.treeView1.TabIndex = 1;
            this.treeView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseClick);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加轴ToolStripMenuItem,
            this.添加气缸ToolStripMenuItem,
            this.添加轴组合ToolStripMenuItem,
            this.添加机器人ToolStripMenuItem,
            this.添加IOToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 136);
            // 
            // 添加轴ToolStripMenuItem
            // 
            this.添加轴ToolStripMenuItem.Name = "添加轴ToolStripMenuItem";
            this.添加轴ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.添加轴ToolStripMenuItem.Text = "添加轴 ";
            this.添加轴ToolStripMenuItem.Click += new System.EventHandler(this.添加轴ToolStripMenuItem_Click);
            // 
            // 添加气缸ToolStripMenuItem
            // 
            this.添加气缸ToolStripMenuItem.Name = "添加气缸ToolStripMenuItem";
            this.添加气缸ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.添加气缸ToolStripMenuItem.Text = "添加气缸";
            this.添加气缸ToolStripMenuItem.Click += new System.EventHandler(this.添加气缸ToolStripMenuItem_Click);
            // 
            // 添加轴组合ToolStripMenuItem
            // 
            this.添加轴组合ToolStripMenuItem.Name = "添加轴组合ToolStripMenuItem";
            this.添加轴组合ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.添加轴组合ToolStripMenuItem.Text = "添加轴组合";
            this.添加轴组合ToolStripMenuItem.Click += new System.EventHandler(this.添加轴组合ToolStripMenuItem_Click);
            // 
            // 添加机器人ToolStripMenuItem
            // 
            this.添加机器人ToolStripMenuItem.Name = "添加机器人ToolStripMenuItem";
            this.添加机器人ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.添加机器人ToolStripMenuItem.Text = "添加机器人";
            this.添加机器人ToolStripMenuItem.Click += new System.EventHandler(this.添加机器人ToolStripMenuItem_Click);
            // 
            // 添加IOToolStripMenuItem
            // 
            this.添加IOToolStripMenuItem.Name = "添加IOToolStripMenuItem";
            this.添加IOToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.添加IOToolStripMenuItem.Text = "添加IO";
            this.添加IOToolStripMenuItem.Click += new System.EventHandler(this.添加IOToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // CommandControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.propertyGrid1);
            this.Name = "CommandControl1";
            this.Size = new System.Drawing.Size(384, 644);
            this.Enter += new System.EventHandler(this.UserCommandControl1_Enter);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 添加轴ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加气缸ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加轴组合ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加机器人ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加IOToolStripMenuItem;
    }
}
