namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    partial class CalculateUserControl
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown_select_rb_max = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_select_rb_min = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown_select_ra_max = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_select_ra_min = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.thresholdControls1 = new Vision2.vision.HalconRunFile.Controls.ThresholdControls();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_select_rb_max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_select_rb_min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_select_ra_max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_select_ra_min)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.thresholdControls1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(0, 138);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(536, 121);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "2、灰度赛选";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(536, 138);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1、绘制区域";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(114, 28);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 36);
            this.button2.TabIndex = 1;
            this.button2.Text = "擦除";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(33, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "绘制";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown3.Location = new System.Drawing.Point(140, 27);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(92, 23);
            this.numericUpDown3.TabIndex = 90;
            this.numericUpDown3.Value = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            this.numericUpDown3.Validated += new System.EventHandler(this.numericUpDown3_Validated);
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown4.Location = new System.Drawing.Point(75, 26);
            this.numericUpDown4.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(61, 23);
            this.numericUpDown4.TabIndex = 91;
            this.numericUpDown4.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(11, 32);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 14);
            this.label8.TabIndex = 89;
            this.label8.Text = "面积筛选";
            // 
            // numericUpDown_select_rb_max
            // 
            this.numericUpDown_select_rb_max.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown_select_rb_max.Location = new System.Drawing.Point(140, 73);
            this.numericUpDown_select_rb_max.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numericUpDown_select_rb_max.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDown_select_rb_max.Name = "numericUpDown_select_rb_max";
            this.numericUpDown_select_rb_max.Size = new System.Drawing.Size(92, 23);
            this.numericUpDown_select_rb_max.TabIndex = 87;
            this.numericUpDown_select_rb_max.Value = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            // 
            // numericUpDown_select_rb_min
            // 
            this.numericUpDown_select_rb_min.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown_select_rb_min.Location = new System.Drawing.Point(75, 72);
            this.numericUpDown_select_rb_min.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown_select_rb_min.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDown_select_rb_min.Name = "numericUpDown_select_rb_min";
            this.numericUpDown_select_rb_min.Size = new System.Drawing.Size(61, 23);
            this.numericUpDown_select_rb_min.TabIndex = 88;
            this.numericUpDown_select_rb_min.Value = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(11, 77);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 86;
            this.label5.Text = "rb宽筛选";
            // 
            // numericUpDown_select_ra_max
            // 
            this.numericUpDown_select_ra_max.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown_select_ra_max.Location = new System.Drawing.Point(140, 50);
            this.numericUpDown_select_ra_max.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numericUpDown_select_ra_max.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDown_select_ra_max.Name = "numericUpDown_select_ra_max";
            this.numericUpDown_select_ra_max.Size = new System.Drawing.Size(92, 23);
            this.numericUpDown_select_ra_max.TabIndex = 84;
            this.numericUpDown_select_ra_max.Value = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            // 
            // numericUpDown_select_ra_min
            // 
            this.numericUpDown_select_ra_min.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown_select_ra_min.Location = new System.Drawing.Point(75, 49);
            this.numericUpDown_select_ra_min.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown_select_ra_min.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDown_select_ra_min.Name = "numericUpDown_select_ra_min";
            this.numericUpDown_select_ra_min.Size = new System.Drawing.Size(61, 23);
            this.numericUpDown_select_ra_min.TabIndex = 85;
            this.numericUpDown_select_ra_min.Value = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(11, 55);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 83;
            this.label4.Text = "ra长筛选";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDown3);
            this.groupBox2.Controls.Add(this.numericUpDown4);
            this.groupBox2.Controls.Add(this.numericUpDown_select_ra_max);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.numericUpDown_select_ra_min);
            this.groupBox2.Controls.Add(this.numericUpDown_select_rb_min);
            this.groupBox2.Controls.Add(this.numericUpDown_select_rb_max);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, 259);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(536, 106);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2、灰度赛选";
            // 
            // thresholdControls1
            // 
            this.thresholdControls1.Location = new System.Drawing.Point(8, 30);
            this.thresholdControls1.Margin = new System.Windows.Forms.Padding(5);
            this.thresholdControls1.Name = "thresholdControls1";
            this.thresholdControls1.Size = new System.Drawing.Size(207, 26);
            this.thresholdControls1.TabIndex = 5;
            // 
            // CalculateUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "CalculateUserControl";
            this.Size = new System.Drawing.Size(536, 638);
            this.Load += new System.EventHandler(this.CalculateUserControl_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_select_rb_max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_select_rb_min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_select_ra_max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_select_ra_min)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private Controls.ThresholdControls thresholdControls1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown_select_rb_max;
        private System.Windows.Forms.NumericUpDown numericUpDown_select_rb_min;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown_select_ra_max;
        private System.Windows.Forms.NumericUpDown numericUpDown_select_ra_min;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
