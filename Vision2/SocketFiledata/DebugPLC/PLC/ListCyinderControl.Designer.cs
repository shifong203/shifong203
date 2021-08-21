namespace ErosSocket.DebugPLC.PLC
{
    partial class ListCyinderControl
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new Vision2.ErosProjcetDLL.UI.DataGridViewF.DataGridViewComboEditBoxColumn();
            this.cylinderControl1 = new ErosSocket.DebugPLC.PLC.CylinderControl();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(199, 302);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "气缸名称";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // cylinderControl1
            // 
            this.cylinderControl1.AutoSize = true;
            this.cylinderControl1.BackColor = System.Drawing.Color.Transparent;
            this.cylinderControl1.BorderColor = System.Drawing.Color.Orange;
            this.cylinderControl1.Location = new System.Drawing.Point(205, 3);
            this.cylinderControl1.Name = "cylinderControl1";
            this.cylinderControl1.Size = new System.Drawing.Size(175, 101);
            this.cylinderControl1.TabIndex = 1;
            // 
            // ListCyinderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cylinderControl1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ListCyinderControl";
            this.Size = new System.Drawing.Size(408, 302);
            this.Leave += new System.EventHandler(this.ListCyinderControl_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private Vision2.ErosProjcetDLL.UI.DataGridViewF.DataGridViewComboEditBoxColumn Column1;
        private CylinderControl cylinderControl1;
    }
}
