namespace ErosSocket
{
    partial class Modbus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Modbus));
            this.label14 = new System.Windows.Forms.Label();
            this.cbValues = new System.Windows.Forms.ComboBox();
            this.numAddress = new System.Windows.Forms.NumericUpDown();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numWirteAddress = new System.Windows.Forms.NumericUpDown();
            this.numWriteValue = new System.Windows.Forms.NumericUpDown();
            this.numReadAddress = new System.Windows.Forms.NumericUpDown();
            this.numReadAddressLength = new System.Windows.Forms.NumericUpDown();
            this.cobWrite = new System.Windows.Forms.ComboBox();
            this.cobFunctionCode = new System.Windows.Forms.ComboBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.btnReadBool = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOutIP = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.BtnDeleteRead = new System.Windows.Forms.Button();
            this.BtnSend = new System.Windows.Forms.Button();
            this.txtRead = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.btnSaveModbusLink = new System.Windows.Forms.Button();
            this.btnP = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnSaveValues = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.numAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWirteAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWriteValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReadAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReadAddressLength)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(501, 41);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 12);
            this.label14.TabIndex = 76;
            this.label14.Text = "变量表";
            // 
            // cbValues
            // 
            this.cbValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValues.FormattingEnabled = true;
            this.cbValues.Location = new System.Drawing.Point(542, 38);
            this.cbValues.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbValues.Name = "cbValues";
            this.cbValues.Size = new System.Drawing.Size(78, 20);
            this.cbValues.TabIndex = 75;
            // 
            // numAddress
            // 
            this.numAddress.Location = new System.Drawing.Point(357, 35);
            this.numAddress.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numAddress.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numAddress.Name = "numAddress";
            this.numAddress.Size = new System.Drawing.Size(59, 21);
            this.numAddress.TabIndex = 73;
            this.numAddress.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(249, 34);
            this.nudPort.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.nudPort.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(70, 21);
            this.nudPort.TabIndex = 74;
            this.nudPort.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.nudPort.Value = new decimal(new int[] {
            502,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(321, 40);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 12);
            this.label12.TabIndex = 67;
            this.label12.Text = "站号:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numWirteAddress);
            this.groupBox3.Controls.Add(this.numWriteValue);
            this.groupBox3.Controls.Add(this.numReadAddress);
            this.groupBox3.Controls.Add(this.numReadAddressLength);
            this.groupBox3.Controls.Add(this.cobWrite);
            this.groupBox3.Controls.Add(this.cobFunctionCode);
            this.groupBox3.Controls.Add(this.btnSet);
            this.groupBox3.Controls.Add(this.button8);
            this.groupBox3.Controls.Add(this.btnReadBool);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(705, 31);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(343, 124);
            this.groupBox3.TabIndex = 72;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "特殊测试";
            // 
            // numWirteAddress
            // 
            this.numWirteAddress.Location = new System.Drawing.Point(204, 38);
            this.numWirteAddress.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numWirteAddress.Name = "numWirteAddress";
            this.numWirteAddress.Size = new System.Drawing.Size(68, 21);
            this.numWirteAddress.TabIndex = 66;
            // 
            // numWriteValue
            // 
            this.numWriteValue.Location = new System.Drawing.Point(204, 63);
            this.numWriteValue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numWriteValue.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numWriteValue.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numWriteValue.Name = "numWriteValue";
            this.numWriteValue.Size = new System.Drawing.Size(68, 21);
            this.numWriteValue.TabIndex = 66;
            // 
            // numReadAddress
            // 
            this.numReadAddress.Location = new System.Drawing.Point(46, 37);
            this.numReadAddress.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numReadAddress.Name = "numReadAddress";
            this.numReadAddress.Size = new System.Drawing.Size(68, 21);
            this.numReadAddress.TabIndex = 66;
            // 
            // numReadAddressLength
            // 
            this.numReadAddressLength.Location = new System.Drawing.Point(46, 63);
            this.numReadAddressLength.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numReadAddressLength.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numReadAddressLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numReadAddressLength.Name = "numReadAddressLength";
            this.numReadAddressLength.Size = new System.Drawing.Size(68, 21);
            this.numReadAddressLength.TabIndex = 66;
            this.numReadAddressLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cobWrite
            // 
            this.cobWrite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobWrite.FormattingEnabled = true;
            this.cobWrite.Items.AddRange(new object[] {
            "写线圈",
            "写寄存器"});
            this.cobWrite.Location = new System.Drawing.Point(238, 12);
            this.cobWrite.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cobWrite.Name = "cobWrite";
            this.cobWrite.Size = new System.Drawing.Size(88, 20);
            this.cobWrite.TabIndex = 65;
            // 
            // cobFunctionCode
            // 
            this.cobFunctionCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobFunctionCode.FormattingEnabled = true;
            this.cobFunctionCode.Items.AddRange(new object[] {
            "读寄存器3",
            "读离散量2",
            "读线圈1"});
            this.cobFunctionCode.Location = new System.Drawing.Point(11, 14);
            this.cobFunctionCode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cobFunctionCode.Name = "cobFunctionCode";
            this.cobFunctionCode.Size = new System.Drawing.Size(88, 20);
            this.cobFunctionCode.TabIndex = 65;
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(204, 91);
            this.btnSet.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(72, 22);
            this.btnSet.TabIndex = 36;
            this.btnSet.Text = "写入数据";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(11, 91);
            this.button8.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(81, 22);
            this.button8.TabIndex = 34;
            this.button8.Text = "切换长链接";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // btnReadBool
            // 
            this.btnReadBool.Location = new System.Drawing.Point(102, 12);
            this.btnReadBool.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnReadBool.Name = "btnReadBool";
            this.btnReadBool.Size = new System.Drawing.Size(59, 22);
            this.btnReadBool.TabIndex = 18;
            this.btnReadBool.Text = "读取";
            this.btnReadBool.UseVisualStyleBackColor = true;
            this.btnReadBool.Click += new System.EventHandler(this.btnReadBool_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(166, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 27;
            this.label8.Text = "值：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(166, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 25;
            this.label9.Text = "地址：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(166, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 24;
            this.label7.Text = "写单个地址：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "长度：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 68;
            this.label2.Text = "端口号：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 66;
            this.label1.Text = "Ip地址：";
            // 
            // txtOutIP
            // 
            this.txtOutIP.Location = new System.Drawing.Point(80, 34);
            this.txtOutIP.Name = "txtOutIP";
            this.txtOutIP.Size = new System.Drawing.Size(116, 21);
            this.txtOutIP.TabIndex = 65;
            this.txtOutIP.Text = "192.168.1.195";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BtnDeleteRead);
            this.groupBox2.Controls.Add(this.BtnSend);
            this.groupBox2.Controls.Add(this.txtRead);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtSend);
            this.groupBox2.Location = new System.Drawing.Point(10, 62);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(688, 267);
            this.groupBox2.TabIndex = 71;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "基础数据收发，需要输入完整的数据信息";
            // 
            // BtnDeleteRead
            // 
            this.BtnDeleteRead.Location = new System.Drawing.Point(604, 54);
            this.BtnDeleteRead.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BtnDeleteRead.Name = "BtnDeleteRead";
            this.BtnDeleteRead.Size = new System.Drawing.Size(78, 22);
            this.BtnDeleteRead.TabIndex = 10;
            this.BtnDeleteRead.Text = "清空";
            this.BtnDeleteRead.UseVisualStyleBackColor = true;
            // 
            // BtnSend
            // 
            this.BtnSend.Location = new System.Drawing.Point(604, 21);
            this.BtnSend.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(78, 22);
            this.BtnSend.TabIndex = 11;
            this.BtnSend.Text = "发送";
            this.BtnSend.UseVisualStyleBackColor = true;
            // 
            // txtRead
            // 
            this.txtRead.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRead.Location = new System.Drawing.Point(7, 93);
            this.txtRead.Multiline = true;
            this.txtRead.Name = "txtRead";
            this.txtRead.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRead.Size = new System.Drawing.Size(677, 169);
            this.txtRead.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "响应信息：";
            // 
            // txtSend
            // 
            this.txtSend.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSend.Location = new System.Drawing.Point(6, 20);
            this.txtSend.Multiline = true;
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(594, 57);
            this.txtSend.TabIndex = 0;
            // 
            // btnSaveModbusLink
            // 
            this.btnSaveModbusLink.Location = new System.Drawing.Point(629, 31);
            this.btnSaveModbusLink.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSaveModbusLink.Name = "btnSaveModbusLink";
            this.btnSaveModbusLink.Size = new System.Drawing.Size(70, 29);
            this.btnSaveModbusLink.TabIndex = 69;
            this.btnSaveModbusLink.Text = "保存链接";
            this.btnSaveModbusLink.UseVisualStyleBackColor = true;
            // 
            // btnP
            // 
            this.btnP.Location = new System.Drawing.Point(423, 34);
            this.btnP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnP.Name = "btnP";
            this.btnP.Size = new System.Drawing.Size(71, 22);
            this.btnP.TabIndex = 70;
            this.btnP.Text = "链接";
            this.btnP.UseVisualStyleBackColor = true;
            this.btnP.Click += new System.EventHandler(this.btnP_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnSaveValues});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1086, 25);
            this.toolStrip1.TabIndex = 77;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnSaveValues
            // 
            this.tsbtnSaveValues.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnSaveValues.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSaveValues.Image")));
            this.tsbtnSaveValues.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSaveValues.Name = "tsbtnSaveValues";
            this.tsbtnSaveValues.Size = new System.Drawing.Size(36, 22);
            this.tsbtnSaveValues.Text = "保存";
            // 
            // Modbus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 398);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cbValues);
            this.Controls.Add(this.numAddress);
            this.Controls.Add(this.nudPort);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOutIP);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSaveModbusLink);
            this.Controls.Add(this.btnP);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Modbus";
            this.Text = "Modbus";
            ((System.ComponentModel.ISupportInitialize)(this.numAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWirteAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWriteValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReadAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReadAddressLength)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cbValues;
        private System.Windows.Forms.NumericUpDown numAddress;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown numWirteAddress;
        private System.Windows.Forms.NumericUpDown numWriteValue;
        private System.Windows.Forms.NumericUpDown numReadAddress;
        private System.Windows.Forms.NumericUpDown numReadAddressLength;
        private System.Windows.Forms.ComboBox cobWrite;
        private System.Windows.Forms.ComboBox cobFunctionCode;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button btnReadBool;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOutIP;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button BtnDeleteRead;
        private System.Windows.Forms.Button BtnSend;
        private System.Windows.Forms.TextBox txtRead;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Button btnSaveModbusLink;
        private System.Windows.Forms.Button btnP;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnSaveValues;
    }
}