namespace Vision2.Project
{
    partial class NewProjectForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("解决方案", 0, 0);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("节点0", 1, 1);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("节点1", 2, 2);
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("节点2");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("节点3");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("节点4");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("节点5");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("节点6");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("节点7");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProjectForm));
            this.新建程序 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tVProject = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.OpenProjectButton = new System.Windows.Forms.ToolStripButton();
            this.AddProjectButton = new System.Windows.Forms.ToolStripButton();
            this.RedrawProjectButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveProjectButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.新建程序.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // 新建程序
            // 
            this.新建程序.Controls.Add(this.textBox1);
            this.新建程序.Controls.Add(this.label3);
            this.新建程序.Controls.Add(this.button1);
            this.新建程序.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.新建程序.Location = new System.Drawing.Point(0, 571);
            this.新建程序.Name = "新建程序";
            this.新建程序.Size = new System.Drawing.Size(1557, 72);
            this.新建程序.TabIndex = 16;
            this.新建程序.TabStop = false;
            this.新建程序.Text = "新建程序";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(73, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(526, 31);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "程序名称";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(15, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 21);
            this.label3.TabIndex = 7;
            this.label3.Text = "名称";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(619, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "确认";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1557, 546);
            this.splitContainer1.SplitterDistance = 1159;
            this.splitContainer1.TabIndex = 17;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tVProject);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(1159, 546);
            this.splitContainer2.SplitterDistance = 385;
            this.splitContainer2.TabIndex = 0;
            // 
            // tVProject
            // 
            this.tVProject.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.tVProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tVProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tVProject.ForeColor = System.Drawing.Color.White;
            this.tVProject.FullRowSelect = true;
            this.tVProject.ImageIndex = 0;
            this.tVProject.ImageList = this.imageList1;
            this.tVProject.Indent = 20;
            this.tVProject.ItemHeight = 25;
            this.tVProject.LabelEdit = true;
            this.tVProject.Location = new System.Drawing.Point(0, 0);
            this.tVProject.Margin = new System.Windows.Forms.Padding(2);
            this.tVProject.Name = "tVProject";
            treeNode1.ImageIndex = 0;
            treeNode1.Name = "解决方案";
            treeNode1.SelectedImageIndex = 0;
            treeNode1.Text = "解决方案";
            treeNode2.ImageIndex = 1;
            treeNode2.Name = "节点0";
            treeNode2.SelectedImageIndex = 1;
            treeNode2.Text = "节点0";
            treeNode3.ImageIndex = 2;
            treeNode3.Name = "节点1";
            treeNode3.SelectedImageIndex = 2;
            treeNode3.Text = "节点1";
            treeNode4.ImageIndex = 3;
            treeNode4.Name = "节点2";
            treeNode4.SelectedImageKey = "key-vector.png";
            treeNode4.Text = "节点2";
            treeNode5.ImageIndex = 4;
            treeNode5.Name = "节点3";
            treeNode5.SelectedImageKey = "money-vector.png";
            treeNode5.Text = "节点3";
            treeNode6.ImageIndex = 5;
            treeNode6.Name = "节点4";
            treeNode6.SelectedImageKey = "recycle-sign-vector.png";
            treeNode6.Text = "节点4";
            treeNode7.ImageIndex = 6;
            treeNode7.Name = "节点5";
            treeNode7.SelectedImageKey = "video-camera-vector.png";
            treeNode7.Text = "节点5";
            treeNode8.ImageIndex = 7;
            treeNode8.Name = "节点6";
            treeNode8.SelectedImageKey = "video-camera-vector.png";
            treeNode8.Text = "节点6";
            treeNode9.ImageIndex = 8;
            treeNode9.Name = "节点7";
            treeNode9.SelectedImageKey = "iphone-we.png";
            treeNode9.Text = "节点7";
            this.tVProject.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9});
            this.tVProject.SelectedImageIndex = 0;
            this.tVProject.Size = new System.Drawing.Size(385, 546);
            this.tVProject.TabIndex = 22;
            this.tVProject.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tVProject_AfterLabelEdit);
            this.tVProject.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tVProject_KeyDown_1);
            this.tVProject.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tVProject_MouseUp);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "link.png");
            this.imageList1.Images.SetKeyName(1, "network.png");
            this.imageList1.Images.SetKeyName(2, "archive-vector.png");
            this.imageList1.Images.SetKeyName(3, "box-vector.png");
            this.imageList1.Images.SetKeyName(4, "globe-vector.png");
            this.imageList1.Images.SetKeyName(5, "key-vector.png");
            this.imageList1.Images.SetKeyName(6, "money-vector.png");
            this.imageList1.Images.SetKeyName(7, "recycle-sign-vector.png");
            this.imageList1.Images.SetKeyName(8, "video-camera-vector.png");
            this.imageList1.Images.SetKeyName(9, "user-male-circle-vector.png");
            this.imageList1.Images.SetKeyName(10, "iphone-we.png");
            // 
            // groupBox2
            // 
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(770, 546);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.propertyGrid1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(394, 546);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "属性";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 17);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(388, 526);
            this.propertyGrid1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenProjectButton,
            this.AddProjectButton,
            this.RedrawProjectButton,
            this.toolStripSeparator1,
            this.SaveProjectButton,
            this.toolStripSeparator2,
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1557, 25);
            this.toolStrip1.TabIndex = 23;
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
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(76, 22);
            this.toolStripButton2.Text = "本地网络";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // NewProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1557, 643);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.新建程序);
            this.Controls.Add(this.toolStrip1);
            this.Name = "NewProjectForm";
            this.Text = "NewProjectForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewProjectForm_FormClosing);
            this.Load += new System.EventHandler(this.NewProjectForm_Load);
            this.新建程序.ResumeLayout(false);
            this.新建程序.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox 新建程序;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ImageList imageList1;
        public System.Windows.Forms.TreeView tVProject;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton OpenProjectButton;
        private System.Windows.Forms.ToolStripButton AddProjectButton;
        private System.Windows.Forms.ToolStripButton RedrawProjectButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton SaveProjectButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}