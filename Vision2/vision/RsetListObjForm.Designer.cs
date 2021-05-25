namespace Vision2.vision
{
    partial class RsetListObjForm
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
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.hWindowControl2 = new HalconDotNet.HWindowControl();
            this.hWindowControl3 = new HalconDotNet.HWindowControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.hWindowControl4 = new HalconDotNet.HWindowControl();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 421);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(347, 377);
            this.hWindowControl1.TabIndex = 10;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(347, 377);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.hWindowControl4);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(993, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(383, 798);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "复判信息";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridView1.Location = new System.Drawing.Point(3, 174);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(377, 330);
            this.dataGridView1.TabIndex = 7;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "程序";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "位号";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "复判";
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 116);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(377, 58);
            this.panel1.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.Font = new System.Drawing.Font("宋体", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.Color.Lime;
            this.button1.Location = new System.Drawing.Point(3, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 57);
            this.button1.TabIndex = 1;
            this.button1.Text = "提交";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Red;
            this.label1.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(218, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 54);
            this.label1.TabIndex = 8;
            this.label1.Text = "OK";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(3, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 24);
            this.label4.TabIndex = 4;
            this.label4.Text = "SN:1234567890";
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(3, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(377, 66);
            this.label3.TabIndex = 3;
            this.label3.Text = "计数:0    良率%:0\r\n OK:0     NG:0\r\n";
            // 
            // hWindowControl2
            // 
            this.hWindowControl2.BackColor = System.Drawing.Color.Black;
            this.hWindowControl2.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl2.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl2.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl2.Name = "hWindowControl2";
            this.hWindowControl2.Size = new System.Drawing.Size(646, 798);
            this.hWindowControl2.TabIndex = 10;
            this.hWindowControl2.WindowSize = new System.Drawing.Size(646, 798);
            // 
            // hWindowControl3
            // 
            this.hWindowControl3.BackColor = System.Drawing.Color.Black;
            this.hWindowControl3.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.hWindowControl3.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl3.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl3.Name = "hWindowControl3";
            this.hWindowControl3.Size = new System.Drawing.Size(347, 421);
            this.hWindowControl3.TabIndex = 11;
            this.hWindowControl3.WindowSize = new System.Drawing.Size(347, 421);
            this.hWindowControl3.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hWindowControl3_HMouseMove);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.hWindowControl1);
            this.panel2.Controls.Add(this.hWindowControl3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(646, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(347, 798);
            this.panel2.TabIndex = 12;
            // 
            // hWindowControl4
            // 
            this.hWindowControl4.BackColor = System.Drawing.Color.Black;
            this.hWindowControl4.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl4.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl4.Location = new System.Drawing.Point(3, 504);
            this.hWindowControl4.Name = "hWindowControl4";
            this.hWindowControl4.Size = new System.Drawing.Size(377, 291);
            this.hWindowControl4.TabIndex = 11;
            this.hWindowControl4.WindowSize = new System.Drawing.Size(377, 291);
            // 
            // RsetListObjForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1376, 798);
            this.Controls.Add(this.hWindowControl2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Name = "RsetListObjForm";
            this.Text = "RsetListObjForm";
            this.Load += new System.EventHandler(this.RsetListObjForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private HalconDotNet.HWindowControl hWindowControl2;
        private HalconDotNet.HWindowControl hWindowControl3;
        private System.Windows.Forms.Panel panel2;
        private HalconDotNet.HWindowControl hWindowControl4;
    }
}