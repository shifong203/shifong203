namespace Vision2.vision.HalconRunFile.Controls
{
    partial class ThresholdControls
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
            this.numericUpDownThrMax = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownThrMin = new System.Windows.Forms.NumericUpDown();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThrMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThrMin)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownThrMax
            // 
            this.numericUpDownThrMax.Dock = System.Windows.Forms.DockStyle.Left;
            this.numericUpDownThrMax.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDownThrMax.Location = new System.Drawing.Point(156, 0);
            this.numericUpDownThrMax.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDownThrMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownThrMax.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownThrMax.Name = "numericUpDownThrMax";
            this.numericUpDownThrMax.Size = new System.Drawing.Size(45, 23);
            this.numericUpDownThrMax.TabIndex = 54;
            this.numericUpDownThrMax.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownThrMax.ValueChanged += new System.EventHandler(this.numericUpDownThrMax_ValueChanged);
            // 
            // numericUpDownThrMin
            // 
            this.numericUpDownThrMin.Dock = System.Windows.Forms.DockStyle.Left;
            this.numericUpDownThrMin.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDownThrMin.Location = new System.Drawing.Point(111, 0);
            this.numericUpDownThrMin.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDownThrMin.Name = "numericUpDownThrMin";
            this.numericUpDownThrMin.Size = new System.Drawing.Size(45, 23);
            this.numericUpDownThrMin.TabIndex = 55;
            this.numericUpDownThrMin.ValueChanged += new System.EventHandler(this.numericUpDownThrMax_ValueChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.comboBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(35, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(76, 22);
            this.comboBox1.TabIndex = 56;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.numericUpDownThrMax_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 14);
            this.label1.TabIndex = 57;
            this.label1.Text = "通道";
            // 
            // ThresholdControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numericUpDownThrMax);
            this.Controls.Add(this.numericUpDownThrMin);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Name = "ThresholdControls";
            this.Size = new System.Drawing.Size(203, 24);
            this.Load += new System.EventHandler(this.ThresholdControls_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThrMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThrMin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown numericUpDownThrMax;
        private System.Windows.Forms.NumericUpDown numericUpDownThrMin;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
    }
}
