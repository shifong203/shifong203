namespace Vision2.vision.Cams
{
    partial class CamControl
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
            this.hSBGain = new System.Windows.Forms.HScrollBar();
            this.Lebm_bCamIsOK = new System.Windows.Forms.Label();
            this.hSBExposure = new System.Windows.Forms.HScrollBar();
            this.txExposure = new System.Windows.Forms.TextBox();
            this.TBGain = new System.Windows.Forms.TextBox();
            this.btnThreadGrab = new System.Windows.Forms.Button();
            this.toCamLink = new System.Windows.Forms.Button();
            this.btnOneShot = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CamIntPut1 = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hSBGain
            // 
            this.hSBGain.Location = new System.Drawing.Point(157, 44);
            this.hSBGain.Maximum = 1000;
            this.hSBGain.Minimum = 1;
            this.hSBGain.Name = "hSBGain";
            this.hSBGain.Size = new System.Drawing.Size(187, 18);
            this.hSBGain.TabIndex = 34;
            this.hSBGain.Value = 1;
            this.hSBGain.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hSBGain_Scroll);
            // 
            // Lebm_bCamIsOK
            // 
            this.Lebm_bCamIsOK.AutoSize = true;
            this.Lebm_bCamIsOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lebm_bCamIsOK.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Lebm_bCamIsOK.Location = new System.Drawing.Point(92, 12);
            this.Lebm_bCamIsOK.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lebm_bCamIsOK.Name = "Lebm_bCamIsOK";
            this.Lebm_bCamIsOK.Size = new System.Drawing.Size(44, 12);
            this.Lebm_bCamIsOK.TabIndex = 31;
            this.Lebm_bCamIsOK.Text = "已断开";
            // 
            // hSBExposure
            // 
            this.hSBExposure.Location = new System.Drawing.Point(158, 4);
            this.hSBExposure.Maximum = 1000000;
            this.hSBExposure.Minimum = 1;
            this.hSBExposure.Name = "hSBExposure";
            this.hSBExposure.Size = new System.Drawing.Size(187, 18);
            this.hSBExposure.TabIndex = 35;
            this.hSBExposure.Value = 1;
            this.hSBExposure.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hSBExposure_Scroll);
            // 
            // txExposure
            // 
            this.txExposure.Location = new System.Drawing.Point(234, 22);
            this.txExposure.Margin = new System.Windows.Forms.Padding(2);
            this.txExposure.Name = "txExposure";
            this.txExposure.Size = new System.Drawing.Size(92, 21);
            this.txExposure.TabIndex = 32;
            this.txExposure.TextChanged += new System.EventHandler(this.txExposure_TextChanged);
            // 
            // TBGain
            // 
            this.TBGain.Location = new System.Drawing.Point(196, 63);
            this.TBGain.Margin = new System.Windows.Forms.Padding(2);
            this.TBGain.Name = "TBGain";
            this.TBGain.Size = new System.Drawing.Size(62, 21);
            this.TBGain.TabIndex = 33;
            this.TBGain.TextChanged += new System.EventHandler(this.TBGain_TextChanged);
            // 
            // btnThreadGrab
            // 
            this.btnThreadGrab.Location = new System.Drawing.Point(79, 30);
            this.btnThreadGrab.Name = "btnThreadGrab";
            this.btnThreadGrab.Size = new System.Drawing.Size(69, 28);
            this.btnThreadGrab.TabIndex = 36;
            this.btnThreadGrab.Text = "实时采图";
            this.btnThreadGrab.UseVisualStyleBackColor = true;
            this.btnThreadGrab.Click += new System.EventHandler(this.btnThreadGrab_Click);
            // 
            // toCamLink
            // 
            this.toCamLink.Location = new System.Drawing.Point(12, 3);
            this.toCamLink.Name = "toCamLink";
            this.toCamLink.Size = new System.Drawing.Size(68, 28);
            this.toCamLink.TabIndex = 37;
            this.toCamLink.Text = "连接";
            this.toCamLink.UseVisualStyleBackColor = true;
            this.toCamLink.Click += new System.EventHandler(this.toCamLink_Click);
            // 
            // btnOneShot
            // 
            this.btnOneShot.Location = new System.Drawing.Point(12, 30);
            this.btnOneShot.Name = "btnOneShot";
            this.btnOneShot.Size = new System.Drawing.Size(68, 28);
            this.btnOneShot.TabIndex = 38;
            this.btnOneShot.Text = "单帧采图";
            this.btnOneShot.UseVisualStyleBackColor = true;
            this.btnOneShot.Click += new System.EventHandler(this.btnOneShot_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(159, 26);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 43;
            this.label3.Text = "曝光(微秒)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(161, 69);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 12);
            this.label4.TabIndex = 44;
            this.label4.Text = "增益";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(18, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 12);
            this.label2.TabIndex = 46;
            this.label2.Text = "白平衡";
            // 
            // CamIntPut1
            // 
            this.CamIntPut1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CamIntPut1.FormattingEnabled = true;
            this.CamIntPut1.Items.AddRange(new object[] {
            "Off",
            "Once",
            "Continuons"});
            this.CamIntPut1.Location = new System.Drawing.Point(66, 59);
            this.CamIntPut1.Margin = new System.Windows.Forms.Padding(2);
            this.CamIntPut1.Name = "CamIntPut1";
            this.CamIntPut1.Size = new System.Drawing.Size(91, 20);
            this.CamIntPut1.TabIndex = 48;
            this.CamIntPut1.SelectedIndexChanged += new System.EventHandler(this.CamIntPut1_SelectedIndexChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Location = new System.Drawing.Point(55, 85);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(53, 21);
            this.numericUpDown1.TabIndex = 56;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(14, 87);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 12);
            this.label6.TabIndex = 57;
            this.label6.Text = "red";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Location = new System.Drawing.Point(14, 107);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 12);
            this.label7.TabIndex = 59;
            this.label7.Text = "green";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 2;
            this.numericUpDown2.Location = new System.Drawing.Point(55, 105);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(53, 21);
            this.numericUpDown2.TabIndex = 58;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.DecimalPlaces = 2;
            this.numericUpDown3.Location = new System.Drawing.Point(55, 125);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(53, 21);
            this.numericUpDown3.TabIndex = 60;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(14, 127);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 12);
            this.label8.TabIndex = 61;
            this.label8.Text = "blue";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(362, 465);
            this.propertyGrid1.TabIndex = 62;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toCamLink);
            this.panel1.Controls.Add(this.btnOneShot);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.btnThreadGrab);
            this.panel1.Controls.Add(this.numericUpDown3);
            this.panel1.Controls.Add(this.TBGain);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txExposure);
            this.panel1.Controls.Add(this.numericUpDown2);
            this.panel1.Controls.Add(this.hSBExposure);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.Lebm_bCamIsOK);
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Controls.Add(this.hSBGain);
            this.panel1.Controls.Add(this.CamIntPut1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(376, 154);
            this.panel1.TabIndex = 63;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 154);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(376, 497);
            this.tabControl1.TabIndex = 62;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.propertyGrid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(368, 471);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "属性";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // CamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "CamControl";
            this.Size = new System.Drawing.Size(376, 651);
            this.Load += new System.EventHandler(this.CamControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.HScrollBar hSBGain;
        private System.Windows.Forms.Label Lebm_bCamIsOK;
        private System.Windows.Forms.HScrollBar hSBExposure;
        private System.Windows.Forms.TextBox txExposure;
        private System.Windows.Forms.TextBox TBGain;
        private System.Windows.Forms.Button btnThreadGrab;
        private System.Windows.Forms.Button toCamLink;
        private System.Windows.Forms.Button btnOneShot;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CamIntPut1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
    }
}
