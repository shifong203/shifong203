namespace NokidaE.vision.Cams
{
    partial class CameraOperatorControl1
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
            this.cbDeviceList = new System.Windows.Forms.ComboBox();
            this.bnClose = new System.Windows.Forms.Button();
            this.bnOpen = new System.Windows.Forms.Button();
            this.bnEnum = new System.Windows.Forms.Button();
            this.bnTriggerExec = new System.Windows.Forms.Button();
            this.cbSoftTrigger = new System.Windows.Forms.CheckBox();
            this.bnStartGrab = new System.Windows.Forms.Button();
            this.bnSaveJpg = new System.Windows.Forms.Button();
            this.bnSaveBmp = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbLineMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bnSetParam = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.bnGetParam = new System.Windows.Forms.Button();
            this.cbLineSel = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbFrameRate = new System.Windows.Forms.TextBox();
            this.tbGain = new System.Windows.Forms.TextBox();
            this.tbExposure = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbDefaultWay = new System.Windows.Forms.TextBox();
            this.tbMask = new System.Windows.Forms.TextBox();
            this.bnSetIp = new System.Windows.Forms.Button();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.lbTip = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cbDeviceList
            // 
            this.cbDeviceList.FormattingEnabled = true;
            this.cbDeviceList.Location = new System.Drawing.Point(1, 3);
            this.cbDeviceList.Name = "cbDeviceList";
            this.cbDeviceList.Size = new System.Drawing.Size(361, 20);
            this.cbDeviceList.TabIndex = 1;
            this.cbDeviceList.SelectedIndexChanged += new System.EventHandler(this.cbDeviceList_SelectedIndexChanged);
            // 
            // bnClose
            // 
            this.bnClose.Location = new System.Drawing.Point(82, 15);
            this.bnClose.Name = "bnClose";
            this.bnClose.Size = new System.Drawing.Size(64, 23);
            this.bnClose.TabIndex = 2;
            this.bnClose.Text = "关闭设备";
            this.bnClose.UseVisualStyleBackColor = true;
            this.bnClose.Click += new System.EventHandler(this.bnClose_Click);
            // 
            // bnOpen
            // 
            this.bnOpen.Location = new System.Drawing.Point(11, 15);
            this.bnOpen.Name = "bnOpen";
            this.bnOpen.Size = new System.Drawing.Size(66, 23);
            this.bnOpen.TabIndex = 1;
            this.bnOpen.Text = "打开设备";
            this.bnOpen.UseVisualStyleBackColor = true;
            this.bnOpen.Click += new System.EventHandler(this.bnOpen_Click);
            // 
            // bnEnum
            // 
            this.bnEnum.Location = new System.Drawing.Point(365, 2);
            this.bnEnum.Name = "bnEnum";
            this.bnEnum.Size = new System.Drawing.Size(49, 23);
            this.bnEnum.TabIndex = 0;
            this.bnEnum.Text = "搜索";
            this.bnEnum.UseVisualStyleBackColor = true;
            this.bnEnum.Click += new System.EventHandler(this.bnEnum_Click);
            // 
            // bnTriggerExec
            // 
            this.bnTriggerExec.Enabled = false;
            this.bnTriggerExec.Location = new System.Drawing.Point(82, 40);
            this.bnTriggerExec.Name = "bnTriggerExec";
            this.bnTriggerExec.Size = new System.Drawing.Size(62, 23);
            this.bnTriggerExec.TabIndex = 5;
            this.bnTriggerExec.Text = "采图";
            this.bnTriggerExec.UseVisualStyleBackColor = true;
            this.bnTriggerExec.Click += new System.EventHandler(this.bnTriggerExec_Click);
            // 
            // cbSoftTrigger
            // 
            this.cbSoftTrigger.AutoSize = true;
            this.cbSoftTrigger.Enabled = false;
            this.cbSoftTrigger.Location = new System.Drawing.Point(82, 67);
            this.cbSoftTrigger.Name = "cbSoftTrigger";
            this.cbSoftTrigger.Size = new System.Drawing.Size(60, 16);
            this.cbSoftTrigger.TabIndex = 4;
            this.cbSoftTrigger.Text = "软触发";
            this.cbSoftTrigger.UseVisualStyleBackColor = true;
            this.cbSoftTrigger.CheckedChanged += new System.EventHandler(this.cbSoftTrigger_CheckedChanged);
            // 
            // bnStartGrab
            // 
            this.bnStartGrab.Enabled = false;
            this.bnStartGrab.Location = new System.Drawing.Point(11, 40);
            this.bnStartGrab.Name = "bnStartGrab";
            this.bnStartGrab.Size = new System.Drawing.Size(66, 23);
            this.bnStartGrab.TabIndex = 2;
            this.bnStartGrab.Text = "实时采图";
            this.bnStartGrab.UseVisualStyleBackColor = true;
            this.bnStartGrab.Click += new System.EventHandler(this.bnStartGrab_Click);
            // 
            // bnSaveJpg
            // 
            this.bnSaveJpg.Enabled = false;
            this.bnSaveJpg.Location = new System.Drawing.Point(79, 257);
            this.bnSaveJpg.Name = "bnSaveJpg";
            this.bnSaveJpg.Size = new System.Drawing.Size(67, 23);
            this.bnSaveJpg.TabIndex = 0;
            this.bnSaveJpg.Text = "保存JPG";
            this.bnSaveJpg.UseVisualStyleBackColor = true;
            this.bnSaveJpg.Click += new System.EventHandler(this.bnSaveJpg_Click);
            // 
            // bnSaveBmp
            // 
            this.bnSaveBmp.Enabled = false;
            this.bnSaveBmp.Location = new System.Drawing.Point(9, 257);
            this.bnSaveBmp.Name = "bnSaveBmp";
            this.bnSaveBmp.Size = new System.Drawing.Size(67, 23);
            this.bnSaveBmp.TabIndex = 0;
            this.bnSaveBmp.Text = "保存BMP";
            this.bnSaveBmp.UseVisualStyleBackColor = true;
            this.bnSaveBmp.Click += new System.EventHandler(this.bnSaveBmp_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.cbSoftTrigger);
            this.groupBox4.Controls.Add(this.cbLineMode);
            this.groupBox4.Controls.Add(this.bnClose);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.bnOpen);
            this.groupBox4.Controls.Add(this.bnSaveJpg);
            this.groupBox4.Controls.Add(this.bnTriggerExec);
            this.groupBox4.Controls.Add(this.bnSetParam);
            this.groupBox4.Controls.Add(this.bnSaveBmp);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.bnGetParam);
            this.groupBox4.Controls.Add(this.cbLineSel);
            this.groupBox4.Controls.Add(this.bnStartGrab);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.tbFrameRate);
            this.groupBox4.Controls.Add(this.tbGain);
            this.groupBox4.Controls.Add(this.tbExposure);
            this.groupBox4.Location = new System.Drawing.Point(261, 25);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(154, 288);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "参数";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 12);
            this.label9.TabIndex = 10;
            this.label9.Text = "100ms";
            // 
            // cbLineMode
            // 
            this.cbLineMode.FormattingEnabled = true;
            this.cbLineMode.Location = new System.Drawing.Point(11, 208);
            this.cbLineMode.Name = "cbLineMode";
            this.cbLineMode.Size = new System.Drawing.Size(129, 20);
            this.cbLineMode.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "LineMode";
            // 
            // bnSetParam
            // 
            this.bnSetParam.Enabled = false;
            this.bnSetParam.Location = new System.Drawing.Point(79, 232);
            this.bnSetParam.Name = "bnSetParam";
            this.bnSetParam.Size = new System.Drawing.Size(67, 23);
            this.bnSetParam.TabIndex = 7;
            this.bnSetParam.Text = "设置参数";
            this.bnSetParam.UseVisualStyleBackColor = true;
            this.bnSetParam.Click += new System.EventHandler(this.bnSetParam_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "LineSelector";
            // 
            // bnGetParam
            // 
            this.bnGetParam.Enabled = false;
            this.bnGetParam.Location = new System.Drawing.Point(9, 232);
            this.bnGetParam.Name = "bnGetParam";
            this.bnGetParam.Size = new System.Drawing.Size(67, 23);
            this.bnGetParam.TabIndex = 6;
            this.bnGetParam.Text = "获取参数";
            this.bnGetParam.UseVisualStyleBackColor = true;
            this.bnGetParam.Click += new System.EventHandler(this.bnGetParam_Click);
            // 
            // cbLineSel
            // 
            this.cbLineSel.FormattingEnabled = true;
            this.cbLineSel.Location = new System.Drawing.Point(11, 171);
            this.cbLineSel.Name = "cbLineSel";
            this.cbLineSel.Size = new System.Drawing.Size(129, 20);
            this.cbLineSel.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "帧率";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "增益";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "曝光";
            // 
            // tbFrameRate
            // 
            this.tbFrameRate.Enabled = false;
            this.tbFrameRate.Location = new System.Drawing.Point(44, 130);
            this.tbFrameRate.Name = "tbFrameRate";
            this.tbFrameRate.Size = new System.Drawing.Size(100, 21);
            this.tbFrameRate.TabIndex = 2;
            // 
            // tbGain
            // 
            this.tbGain.Enabled = false;
            this.tbGain.Location = new System.Drawing.Point(44, 108);
            this.tbGain.Name = "tbGain";
            this.tbGain.Size = new System.Drawing.Size(100, 21);
            this.tbGain.TabIndex = 1;
            // 
            // tbExposure
            // 
            this.tbExposure.Enabled = false;
            this.tbExposure.Location = new System.Drawing.Point(44, 85);
            this.tbExposure.Name = "tbExposure";
            this.tbExposure.Size = new System.Drawing.Size(100, 21);
            this.tbExposure.TabIndex = 0;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(4, 25);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(246, 136);
            this.listBox1.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.tbDefaultWay);
            this.groupBox2.Controls.Add(this.tbMask);
            this.groupBox2.Controls.Add(this.bnSetIp);
            this.groupBox2.Controls.Add(this.tbIP);
            this.groupBox2.Controls.Add(this.lbTip);
            this.groupBox2.Location = new System.Drawing.Point(4, 163);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(246, 111);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置IP";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "默认网关";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "子网掩码";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "IP地址";
            // 
            // tbDefaultWay
            // 
            this.tbDefaultWay.Location = new System.Drawing.Point(66, 61);
            this.tbDefaultWay.Name = "tbDefaultWay";
            this.tbDefaultWay.Size = new System.Drawing.Size(168, 21);
            this.tbDefaultWay.TabIndex = 4;
            // 
            // tbMask
            // 
            this.tbMask.Location = new System.Drawing.Point(66, 38);
            this.tbMask.Name = "tbMask";
            this.tbMask.Size = new System.Drawing.Size(168, 21);
            this.tbMask.TabIndex = 3;
            // 
            // bnSetIp
            // 
            this.bnSetIp.Location = new System.Drawing.Point(164, 85);
            this.bnSetIp.Name = "bnSetIp";
            this.bnSetIp.Size = new System.Drawing.Size(70, 23);
            this.bnSetIp.TabIndex = 2;
            this.bnSetIp.Text = "设置";
            this.bnSetIp.UseVisualStyleBackColor = true;
            this.bnSetIp.Click += new System.EventHandler(this.bnSetIp_Click);
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(66, 15);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(168, 21);
            this.tbIP.TabIndex = 1;
            // 
            // lbTip
            // 
            this.lbTip.AutoSize = true;
            this.lbTip.Location = new System.Drawing.Point(10, 85);
            this.lbTip.Name = "lbTip";
            this.lbTip.Size = new System.Drawing.Size(65, 12);
            this.lbTip.TabIndex = 0;
            this.lbTip.Text = "提示信息：";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox1.Location = new System.Drawing.Point(13, 282);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(225, 201);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // CameraOperatorControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bnEnum);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.cbDeviceList);
            this.Name = "CameraOperatorControl1";
            this.Size = new System.Drawing.Size(422, 487);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbDeviceList;
        private System.Windows.Forms.Button bnClose;
        private System.Windows.Forms.Button bnOpen;
        private System.Windows.Forms.Button bnEnum;
        private System.Windows.Forms.Button bnTriggerExec;
        private System.Windows.Forms.CheckBox cbSoftTrigger;
        private System.Windows.Forms.Button bnStartGrab;
        private System.Windows.Forms.Button bnSaveJpg;
        private System.Windows.Forms.Button bnSaveBmp;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button bnSetParam;
        private System.Windows.Forms.Button bnGetParam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbFrameRate;
        private System.Windows.Forms.TextBox tbGain;
        private System.Windows.Forms.TextBox tbExposure;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ComboBox cbLineMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbLineSel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbDefaultWay;
        private System.Windows.Forms.TextBox tbMask;
        private System.Windows.Forms.Button bnSetIp;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Label lbTip;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label9;
    }
}
