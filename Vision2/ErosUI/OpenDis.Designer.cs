namespace Vision2.ErosUI
{
    partial class OpenDis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenDis));
            this.directoryTree = new System.Windows.Forms.TreeView();
            this.directoryIcons = new System.Windows.Forms.ImageList(this.components);
            this.filesList = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filesIcons = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // directoryTree
            // 
            this.directoryTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.directoryTree.ImageIndex = 0;
            this.directoryTree.ImageList = this.directoryIcons;
            this.directoryTree.Location = new System.Drawing.Point(0, 0);
            this.directoryTree.Name = "directoryTree";
            this.directoryTree.SelectedImageIndex = 0;
            this.directoryTree.Size = new System.Drawing.Size(222, 396);
            this.directoryTree.TabIndex = 12;
            this.directoryTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.directoryTree_BeforeExpand);
            this.directoryTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.directoryTree_AfterExpand);
            this.directoryTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.directoryTree_AfterSelect);
            // 
            // directoryIcons
            // 
            this.directoryIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("directoryIcons.ImageStream")));
            this.directoryIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.directoryIcons.Images.SetKeyName(0, "Computer.ico");
            this.directoryIcons.Images.SetKeyName(1, "Closed Folder.ico");
            this.directoryIcons.Images.SetKeyName(2, "Open Folder.ico");
            this.directoryIcons.Images.SetKeyName(3, "fixed drive.ico");
            this.directoryIcons.Images.SetKeyName(4, "My Documents.ico");
            // 
            // filesList
            // 
            this.filesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filesList.Location = new System.Drawing.Point(222, 0);
            this.filesList.Name = "filesList";
            this.filesList.Size = new System.Drawing.Size(680, 396);
            this.filesList.TabIndex = 13;
            this.filesList.UseCompatibleStateImageBehavior = false;
            this.filesList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.filesList_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.deleteFileToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 48);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.openFileToolStripMenuItem.Text = "打开文件";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // deleteFileToolStripMenuItem
            // 
            this.deleteFileToolStripMenuItem.Name = "deleteFileToolStripMenuItem";
            this.deleteFileToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.deleteFileToolStripMenuItem.Text = "删除文件";
            this.deleteFileToolStripMenuItem.Click += new System.EventHandler(this.deleteFileToolStripMenuItem_Click);
            // 
            // filesIcons
            // 
            this.filesIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("filesIcons.ImageStream")));
            this.filesIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.filesIcons.Images.SetKeyName(0, "Closed Folder.ico");
            // 
            // OpenDis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.filesList);
            this.Controls.Add(this.directoryTree);
            this.Name = "OpenDis";
            this.Size = new System.Drawing.Size(902, 396);
            this.Load += new System.EventHandler(this.OpenFile_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TreeView directoryTree;
        private System.Windows.Forms.ListView filesList;
        private System.Windows.Forms.ImageList directoryIcons;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFileToolStripMenuItem;
        private System.Windows.Forms.ImageList filesIcons;
    }
}
