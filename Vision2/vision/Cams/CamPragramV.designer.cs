namespace NokidaE.vision.Cams
{
    partial class CamPragramV
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
            this.LCamRunName = new System.Windows.Forms.Label();
            this.CamR = new System.Windows.Forms.ComboBox();
            this.listCamR = new System.Windows.Forms.ListBox();
            this.btnThreadGrab = new System.Windows.Forms.Button();
            this.btnOneShot = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.TxCamIp_address = new System.Windows.Forms.TextBox();
            this.ReadImaeTime = new System.Windows.Forms.Label();
            this.BtnSeveCam = new System.Windows.Forms.Button();
            this.CbCamQuery = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.ButSearchCam = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.listCanSeek = new System.Windows.Forms.ListBox();
            this.label15 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TxCamName = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.Lebm_bCamIsOK = new System.Windows.Forms.Label();
            this.toCamLink = new System.Windows.Forms.Button();
            this.txExposure = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.hSBExposure = new System.Windows.Forms.HScrollBar();
            this.label4 = new System.Windows.Forms.Label();
            this.TBGain = new System.Windows.Forms.TextBox();
            this.hSBGain = new System.Windows.Forms.HScrollBar();
            this.设置IP = new System.Windows.Forms.Button();
            this.TxMac_address = new System.Windows.Forms.TextBox();
            this.TxInterface_ip_address = new System.Windows.Forms.TextBox();
            this.TxDeviceID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LCamRunName
            // 
            this.LCamRunName.AutoSize = true;
            this.LCamRunName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LCamRunName.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.LCamRunName.Location = new System.Drawing.Point(7, 33);
            this.LCamRunName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LCamRunName.Name = "LCamRunName";
            this.LCamRunName.Size = new System.Drawing.Size(31, 12);
            this.LCamRunName.TabIndex = 12;
            this.LCamRunName.Text = "相机";
            // 
            // CamR
            // 
            this.CamR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CamR.FormattingEnabled = true;
            this.CamR.Location = new System.Drawing.Point(44, 30);
            this.CamR.Margin = new System.Windows.Forms.Padding(2);
            this.CamR.Name = "CamR";
            this.CamR.Size = new System.Drawing.Size(87, 20);
            this.CamR.TabIndex = 13;
            this.CamR.SelectedIndexChanged += new System.EventHandler(this.CamR_SelectedIndexChanged);
            // 
            // listCamR
            // 
            this.listCamR.FormattingEnabled = true;
            this.listCamR.ItemHeight = 12;
            this.listCamR.Location = new System.Drawing.Point(9, 55);
            this.listCamR.Margin = new System.Windows.Forms.Padding(2);
            this.listCamR.Name = "listCamR";
            this.listCamR.Size = new System.Drawing.Size(122, 148);
            this.listCamR.TabIndex = 15;
            this.listCamR.SelectedIndexChanged += new System.EventHandler(this.listCamR_SelectedIndexChanged);
            // 
            // btnThreadGrab
            // 
            this.btnThreadGrab.Location = new System.Drawing.Point(70, 205);
            this.btnThreadGrab.Name = "btnThreadGrab";
            this.btnThreadGrab.Size = new System.Drawing.Size(65, 29);
            this.btnThreadGrab.TabIndex = 23;
            this.btnThreadGrab.Text = "实时采图";
            this.btnThreadGrab.UseVisualStyleBackColor = true;
            this.btnThreadGrab.Click += new System.EventHandler(this.btnThreadGrab_Click);
            // 
            // btnOneShot
            // 
            this.btnOneShot.Location = new System.Drawing.Point(7, 205);
            this.btnOneShot.Name = "btnOneShot";
            this.btnOneShot.Size = new System.Drawing.Size(63, 29);
            this.btnOneShot.TabIndex = 25;
            this.btnOneShot.Text = "单帧采图";
            this.btnOneShot.UseVisualStyleBackColor = true;
            this.btnOneShot.Click += new System.EventHandler(this.btnOneShot_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label5.Location = new System.Drawing.Point(135, 98);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 12);
            this.label5.TabIndex = 36;
            this.label5.Text = "相机IP";
            // 
            // TxCamIp_address
            // 
            this.TxCamIp_address.Location = new System.Drawing.Point(187, 98);
            this.TxCamIp_address.Name = "TxCamIp_address";
            this.TxCamIp_address.Size = new System.Drawing.Size(144, 21);
            this.TxCamIp_address.TabIndex = 30;
            // 
            // ReadImaeTime
            // 
            this.ReadImaeTime.AutoSize = true;
            this.ReadImaeTime.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ReadImaeTime.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ReadImaeTime.Location = new System.Drawing.Point(6, 241);
            this.ReadImaeTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ReadImaeTime.Name = "ReadImaeTime";
            this.ReadImaeTime.Size = new System.Drawing.Size(64, 12);
            this.ReadImaeTime.TabIndex = 33;
            this.ReadImaeTime.Text = "采图时间:";
            // 
            // BtnSeveCam
            // 
            this.BtnSeveCam.Location = new System.Drawing.Point(66, 305);
            this.BtnSeveCam.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSeveCam.Name = "BtnSeveCam";
            this.BtnSeveCam.Size = new System.Drawing.Size(69, 24);
            this.BtnSeveCam.TabIndex = 9;
            this.BtnSeveCam.Text = "添加运行";
            this.BtnSeveCam.UseVisualStyleBackColor = true;
            this.BtnSeveCam.Click += new System.EventHandler(this.BtnSeveCam_Click);
            // 
            // CbCamQuery
            // 
            this.CbCamQuery.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbCamQuery.FormattingEnabled = true;
            this.CbCamQuery.Items.AddRange(new object[] {
            "info_boards",
            "bits_per_channel",
            "camera_type",
            "color_space",
            "defaults",
            "device",
            "external_trigger",
            "field",
            "general",
            "generic",
            "horizontal_resolution",
            "image_height",
            "image_width",
            "parameters",
            "parameters_readonly",
            "parameters_writeonly",
            "port",
            "revision",
            "start_column",
            "start_row",
            "vertical_resolution\'"});
            this.CbCamQuery.Location = new System.Drawing.Point(8, 283);
            this.CbCamQuery.Margin = new System.Windows.Forms.Padding(2);
            this.CbCamQuery.Name = "CbCamQuery";
            this.CbCamQuery.Size = new System.Drawing.Size(105, 20);
            this.CbCamQuery.TabIndex = 14;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label26.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label26.Location = new System.Drawing.Point(141, 248);
            this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(57, 12);
            this.label26.TabIndex = 8;
            this.label26.Text = "相机名称";
            // 
            // ButSearchCam
            // 
            this.ButSearchCam.Location = new System.Drawing.Point(49, 259);
            this.ButSearchCam.Margin = new System.Windows.Forms.Padding(2);
            this.ButSearchCam.Name = "ButSearchCam";
            this.ButSearchCam.Size = new System.Drawing.Size(62, 22);
            this.ButSearchCam.TabIndex = 9;
            this.ButSearchCam.Text = "搜索本地相机";
            this.ButSearchCam.UseVisualStyleBackColor = true;
            this.ButSearchCam.Click += new System.EventHandler(this.ButSearchCam_Click);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label30.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label30.Location = new System.Drawing.Point(7, 267);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(31, 12);
            this.label30.TabIndex = 8;
            this.label30.Text = "信息";
            // 
            // listCanSeek
            // 
            this.listCanSeek.FormattingEnabled = true;
            this.listCanSeek.ItemHeight = 12;
            this.listCanSeek.Location = new System.Drawing.Point(5, 330);
            this.listCanSeek.Margin = new System.Windows.Forms.Padding(2);
            this.listCanSeek.Name = "listCanSeek";
            this.listCanSeek.Size = new System.Drawing.Size(132, 148);
            this.listCanSeek.TabIndex = 43;
            this.listCanSeek.SelectedIndexChanged += new System.EventHandler(this.listCanSeek_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label15.Location = new System.Drawing.Point(6, 310);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(57, 12);
            this.label15.TabIndex = 44;
            this.label15.Text = "本地相机";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // TxCamName
            // 
            this.TxCamName.Location = new System.Drawing.Point(143, 263);
            this.TxCamName.Name = "TxCamName";
            this.TxCamName.Size = new System.Drawing.Size(134, 21);
            this.TxCamName.TabIndex = 5;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(142, 305);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(189, 173);
            this.textBox1.TabIndex = 45;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(146, 290);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 46;
            this.label1.Text = "信息：";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(143, 225);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(188, 21);
            this.textBox2.TabIndex = 47;
            // 
            // Lebm_bCamIsOK
            // 
            this.Lebm_bCamIsOK.AutoSize = true;
            this.Lebm_bCamIsOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lebm_bCamIsOK.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.Lebm_bCamIsOK.Location = new System.Drawing.Point(2, 9);
            this.Lebm_bCamIsOK.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lebm_bCamIsOK.Name = "Lebm_bCamIsOK";
            this.Lebm_bCamIsOK.Size = new System.Drawing.Size(109, 12);
            this.Lebm_bCamIsOK.TabIndex = 17;
            this.Lebm_bCamIsOK.Text = "链接状态：已断开";
            // 
            // toCamLink
            // 
            this.toCamLink.Location = new System.Drawing.Point(113, 4);
            this.toCamLink.Name = "toCamLink";
            this.toCamLink.Size = new System.Drawing.Size(58, 23);
            this.toCamLink.TabIndex = 25;
            this.toCamLink.Text = "链接";
            this.toCamLink.UseVisualStyleBackColor = true;
            this.toCamLink.Click += new System.EventHandler(this.toCamLink_Click);
            // 
            // txExposure
            // 
            this.txExposure.Location = new System.Drawing.Point(249, 8);
            this.txExposure.Margin = new System.Windows.Forms.Padding(2);
            this.txExposure.Name = "txExposure";
            this.txExposure.Size = new System.Drawing.Size(62, 21);
            this.txExposure.TabIndex = 19;
            this.txExposure.TextChanged += new System.EventHandler(this.txExposure_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(174, 12);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "曝光(毫秒)";
            // 
            // hSBExposure
            // 
            this.hSBExposure.Location = new System.Drawing.Point(152, 32);
            this.hSBExposure.Maximum = 100000;
            this.hSBExposure.Minimum = 1;
            this.hSBExposure.Name = "hSBExposure";
            this.hSBExposure.Size = new System.Drawing.Size(178, 18);
            this.hSBExposure.TabIndex = 22;
            this.hSBExposure.Value = 1;
            this.hSBExposure.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hSBExposure_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label4.Location = new System.Drawing.Point(192, 56);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "增益";
            // 
            // TBGain
            // 
            this.TBGain.Location = new System.Drawing.Point(248, 53);
            this.TBGain.Margin = new System.Windows.Forms.Padding(2);
            this.TBGain.Name = "TBGain";
            this.TBGain.Size = new System.Drawing.Size(62, 21);
            this.TBGain.TabIndex = 20;
            // 
            // hSBGain
            // 
            this.hSBGain.Location = new System.Drawing.Point(155, 76);
            this.hSBGain.Minimum = 1;
            this.hSBGain.Name = "hSBGain";
            this.hSBGain.Size = new System.Drawing.Size(176, 18);
            this.hSBGain.TabIndex = 21;
            this.hSBGain.Value = 1;
            this.hSBGain.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hSBGain_Scroll);
            // 
            // 设置IP
            // 
            this.设置IP.Location = new System.Drawing.Point(265, 192);
            this.设置IP.Name = "设置IP";
            this.设置IP.Size = new System.Drawing.Size(65, 29);
            this.设置IP.TabIndex = 48;
            this.设置IP.Text = "设置IP";
            this.设置IP.UseVisualStyleBackColor = true;
            this.设置IP.Click += new System.EventHandler(this.设置IP_Click);
            // 
            // TxMac_address
            // 
            this.TxMac_address.Location = new System.Drawing.Point(187, 121);
            this.TxMac_address.Name = "TxMac_address";
            this.TxMac_address.ReadOnly = true;
            this.TxMac_address.Size = new System.Drawing.Size(144, 21);
            this.TxMac_address.TabIndex = 29;
            // 
            // TxInterface_ip_address
            // 
            this.TxInterface_ip_address.Location = new System.Drawing.Point(187, 143);
            this.TxInterface_ip_address.Name = "TxInterface_ip_address";
            this.TxInterface_ip_address.ReadOnly = true;
            this.TxInterface_ip_address.Size = new System.Drawing.Size(144, 21);
            this.TxInterface_ip_address.TabIndex = 28;
            // 
            // TxDeviceID
            // 
            this.TxDeviceID.Location = new System.Drawing.Point(187, 165);
            this.TxDeviceID.Name = "TxDeviceID";
            this.TxDeviceID.ReadOnly = true;
            this.TxDeviceID.Size = new System.Drawing.Size(144, 21);
            this.TxDeviceID.TabIndex = 27;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label7.Location = new System.Drawing.Point(135, 121);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 12);
            this.label7.TabIndex = 35;
            this.label7.Text = "MAC地址";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label8.Location = new System.Drawing.Point(135, 146);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 12);
            this.label8.TabIndex = 34;
            this.label8.Text = "本地IP";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label33.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label33.Location = new System.Drawing.Point(135, 170);
            this.label33.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(45, 12);
            this.label33.TabIndex = 33;
            this.label33.Text = "设备ID";
            // 
            // CamPragramV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.设置IP);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.listCanSeek);
            this.Controls.Add(this.TxCamName);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.BtnSeveCam);
            this.Controls.Add(this.CbCamQuery);
            this.Controls.Add(this.ButSearchCam);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.ReadImaeTime);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TxDeviceID);
            this.Controls.Add(this.TxInterface_ip_address);
            this.Controls.Add(this.TxMac_address);
            this.Controls.Add(this.TxCamIp_address);
            this.Controls.Add(this.hSBGain);
            this.Controls.Add(this.Lebm_bCamIsOK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.hSBExposure);
            this.Controls.Add(this.txExposure);
            this.Controls.Add(this.TBGain);
            this.Controls.Add(this.btnThreadGrab);
            this.Controls.Add(this.toCamLink);
            this.Controls.Add(this.btnOneShot);
            this.Controls.Add(this.listCamR);
            this.Controls.Add(this.CamR);
            this.Controls.Add(this.LCamRunName);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CamPragramV";
            this.Size = new System.Drawing.Size(337, 486);
            this.Load += new System.EventHandler(this.CamPragram_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LCamRunName;
        private System.Windows.Forms.ComboBox CamR;
        private System.Windows.Forms.ListBox listCamR;
        private System.Windows.Forms.Button btnThreadGrab;
        private System.Windows.Forms.Button btnOneShot;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxCamIp_address;
        private System.Windows.Forms.Label ReadImaeTime;
        private System.Windows.Forms.Button BtnSeveCam;
        private System.Windows.Forms.ComboBox CbCamQuery;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button ButSearchCam;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.ListBox listCanSeek;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox TxCamName;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label Lebm_bCamIsOK;
        private System.Windows.Forms.Button toCamLink;
        private System.Windows.Forms.TextBox txExposure;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.HScrollBar hSBExposure;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TBGain;
        private System.Windows.Forms.HScrollBar hSBGain;
        private System.Windows.Forms.Button 设置IP;
        private System.Windows.Forms.TextBox TxMac_address;
        private System.Windows.Forms.TextBox TxInterface_ip_address;
        private System.Windows.Forms.TextBox TxDeviceID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label33;
    }
}
