namespace Vision2.Project.formula
{
    partial class MaterialContrsl
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新建物料ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除物料ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(255, 189);
            this.treeView1.TabIndex = 0;
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建物料ToolStripMenuItem,
            this.删除物料ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 48);
            // 
            // 新建物料ToolStripMenuItem
            // 
            this.新建物料ToolStripMenuItem.Name = "新建物料ToolStripMenuItem";
            this.新建物料ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.新建物料ToolStripMenuItem.Text = "新建物料";
            this.新建物料ToolStripMenuItem.Click += new System.EventHandler(this.新建物料ToolStripMenuItem_Click);
            // 
            // 删除物料ToolStripMenuItem
            // 
            this.删除物料ToolStripMenuItem.Name = "删除物料ToolStripMenuItem";
            this.删除物料ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除物料ToolStripMenuItem.Text = "删除物料";
            this.删除物料ToolStripMenuItem.Click += new System.EventHandler(this.删除物料ToolStripMenuItem_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 198);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(426, 238);
            this.propertyGrid1.TabIndex = 1;
            // 
            // MaterialContrsl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.treeView1);
            this.Name = "MaterialContrsl";
            this.Size = new System.Drawing.Size(426, 436);
            this.Load += new System.EventHandler(this.MaterialContrsl_Load);
            this.Leave += new System.EventHandler(this.MaterialContrsl_Leave);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 新建物料ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除物料ToolStripMenuItem;
    }
}
