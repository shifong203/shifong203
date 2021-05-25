namespace NokidaE.vision.HalconRunFile.Controls
{
    partial class ConRunProgram
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConRunProgram));
            this.dataGridViewHalcon = new System.Windows.Forms.DataGridView();
            this.RunID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.程序名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.程序类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单次执行 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.执行程序 = new System.Windows.Forms.GroupBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSaveAll = new System.Windows.Forms.ToolStripButton();
            this.btnRedraw = new System.Windows.Forms.ToolStripButton();
            this.tsButton1 = new ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.tsButton2 = new ErosProjcetDLL.UI.ToolStrip.TSButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHalcon)).BeginInit();
            this.执行程序.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewHalcon
            // 
            this.dataGridViewHalcon.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewHalcon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHalcon.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RunID,
            this.程序名,
            this.程序类型,
            this.单次执行,
            this.Column4});
            this.dataGridViewHalcon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewHalcon.Location = new System.Drawing.Point(3, 17);
            this.dataGridViewHalcon.Name = "dataGridViewHalcon";
            this.dataGridViewHalcon.RowHeadersVisible = false;
            this.dataGridViewHalcon.RowTemplate.Height = 23;
            this.dataGridViewHalcon.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewHalcon.Size = new System.Drawing.Size(371, 545);
            this.dataGridViewHalcon.TabIndex = 0;
            this.dataGridViewHalcon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseUp);
            // 
            // RunID
            // 
            this.RunID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.RunID.FillWeight = 20F;
            this.RunID.HeaderText = "ID";
            this.RunID.Name = "RunID";
            this.RunID.Width = 50;
            // 
            // 程序名
            // 
            this.程序名.HeaderText = "程序名";
            this.程序名.Name = "程序名";
            // 
            // 程序类型
            // 
            this.程序类型.HeaderText = "程序类型";
            this.程序类型.Name = "程序类型";
            this.程序类型.ReadOnly = true;
            // 
            // 单次执行
            // 
            this.单次执行.HeaderText = "单次执行";
            this.单次执行.Name = "单次执行";
            this.单次执行.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.单次执行.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.单次执行.Text = "执行";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "执行时间";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // 执行程序
            // 
            this.执行程序.Controls.Add(this.dataGridViewHalcon);
            this.执行程序.Dock = System.Windows.Forms.DockStyle.Left;
            this.执行程序.Location = new System.Drawing.Point(0, 25);
            this.执行程序.Name = "执行程序";
            this.执行程序.Size = new System.Drawing.Size(377, 565);
            this.执行程序.TabIndex = 2;
            this.执行程序.TabStop = false;
            this.执行程序.Text = "执行程序";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "参数名";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 354;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "参数值";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 5;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.FillWeight = 80F;
            this.dataGridViewTextBoxColumn3.HeaderText = "读写R/W";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 50;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.FillWeight = 20F;
            this.dataGridViewTextBoxColumn4.HeaderText = "ID";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 50;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "程序名";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 104;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "程序类型";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 104;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "执行时间";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 104;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSaveAll,
            this.btnRedraw,
            this.tsButton1,
            this.tsButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(857, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSaveAll.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAll.Image")));
            this.btnSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(60, 22);
            this.btnSaveAll.Text = "打开图片";
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
            // 
            // btnRedraw
            // 
            this.btnRedraw.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRedraw.Image = ((System.Drawing.Image)(resources.GetObject("btnRedraw.Image")));
            this.btnRedraw.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRedraw.Name = "btnRedraw";
            this.btnRedraw.Size = new System.Drawing.Size(36, 22);
            this.btnRedraw.Text = "刷新";
            this.btnRedraw.Click += new System.EventHandler(this.btnRedraw_Click);
            // 
            // tsButton1
            // 
            this.tsButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsButton1.Image = ((System.Drawing.Image)(resources.GetObject("tsButton1.Image")));
            this.tsButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton1.Name = "tsButton1";
            this.tsButton1.Size = new System.Drawing.Size(36, 22);
            this.tsButton1.Text = "采图";
            this.tsButton1.Click += new System.EventHandler(this.tsButton1_Click);
            // 
            // tsButton2
            // 
            this.tsButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsButton2.Image = ((System.Drawing.Image)(resources.GetObject("tsButton2.Image")));
            this.tsButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton2.Name = "tsButton2";
            this.tsButton2.Size = new System.Drawing.Size(60, 22);
            this.tsButton2.Text = "实时图像";
            this.tsButton2.Click += new System.EventHandler(this.tsButton2_Click);
            // 
            // ConRunProgram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.执行程序);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ConRunProgram";
            this.Size = new System.Drawing.Size(857, 590);
            this.Load += new System.EventHandler(this.ConRunProgram_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHalcon)).EndInit();
            this.执行程序.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewHalcon;
        private System.Windows.Forms.GroupBox 执行程序;
        private System.Windows.Forms.DataGridViewTextBoxColumn RunID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 程序名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 程序类型;
        private System.Windows.Forms.DataGridViewButtonColumn 单次执行;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnRedraw;
        private System.Windows.Forms.ToolStripButton btnSaveAll;
        private ErosProjcetDLL.UI.ToolStrip.TSButton tsButton1;
        private ErosProjcetDLL.UI.ToolStrip.TSButton tsButton2;
    }
}
