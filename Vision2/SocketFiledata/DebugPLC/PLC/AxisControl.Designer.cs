namespace ErosSocket.DebugPLC.PLC
{
    partial class AxisControl
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
            try
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
            catch (System.Exception)
            {

               
            }
         
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.labHome = new System.Windows.Forms.Label();
            this.labErrer = new System.Windows.Forms.Label();
            this.labEnble = new System.Windows.Forms.Label();
            this.labD = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.plcBtn5 = new ErosSocket.ErosUI.PLCBtn();
            this.plcBtn4 = new ErosSocket.ErosUI.PLCBtn();
            this.plcBtn3 = new ErosSocket.ErosUI.PLCBtn();
            this.plcBtn2 = new ErosSocket.ErosUI.PLCBtn();
            this.plcBtn1 = new ErosSocket.ErosUI.PLCBtn();
            this.floatNumTextBox2 = new ErosSocket.ErosUI.FloatNumTextBox();
            this.floatNumTextBox1 = new ErosSocket.ErosUI.FloatNumTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "当前位置";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "目标位置";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "寸动距离";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(58, 114);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(62, 20);
            this.numericUpDown1.TabIndex = 9;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // labHome
            // 
            this.labHome.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.labHome.Location = new System.Drawing.Point(2, 31);
            this.labHome.Name = "labHome";
            this.labHome.Size = new System.Drawing.Size(42, 23);
            this.labHome.TabIndex = 11;
            this.labHome.Text = "原点";
            this.labHome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labErrer
            // 
            this.labErrer.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.labErrer.Location = new System.Drawing.Point(46, 31);
            this.labErrer.Name = "labErrer";
            this.labErrer.Size = new System.Drawing.Size(42, 23);
            this.labErrer.TabIndex = 12;
            this.labErrer.Text = "故障";
            this.labErrer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labEnble
            // 
            this.labEnble.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.labEnble.Location = new System.Drawing.Point(90, 31);
            this.labEnble.Name = "labEnble";
            this.labEnble.Size = new System.Drawing.Size(42, 23);
            this.labEnble.TabIndex = 13;
            this.labEnble.Text = "使能";
            this.labEnble.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labD
            // 
            this.labD.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.labD.Location = new System.Drawing.Point(134, 31);
            this.labD.Name = "labD";
            this.labD.Size = new System.Drawing.Size(42, 23);
            this.labD.TabIndex = 14;
            this.labD.Text = "抱闸";
            this.labD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(179, 30);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(49, 25);
            this.button3.TabIndex = 15;
            this.button3.Text = "复位";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(136, 116);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 17);
            this.checkBox1.TabIndex = 18;
            this.checkBox1.Text = "寸动模式";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(142, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "速度";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(3, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 24);
            this.label9.TabIndex = 23;
            this.label9.Text = "轴名称";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDown2.Location = new System.Drawing.Point(179, 7);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(47, 16);
            this.numericUpDown2.TabIndex = 25;
            this.numericUpDown2.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // plcBtn5
            // 
            this.plcBtn5.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.plcBtn5.Btn_Model = ErosSocket.ErosUI.PLCBtn.BtnModel.交替;
            this.plcBtn5.FalseColor = System.Drawing.Color.DimGray;
            this.plcBtn5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.plcBtn5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.plcBtn5.GetName = null;
            this.plcBtn5.Location = new System.Drawing.Point(147, 139);
            this.plcBtn5.Name = "plcBtn5";
            this.plcBtn5.SetName = null;
            this.plcBtn5.Size = new System.Drawing.Size(68, 36);
            this.plcBtn5.TabIndex = 24;
            this.plcBtn5.Text = "停止";
            this.plcBtn5.TrueColor = System.Drawing.Color.Lime;
            this.plcBtn5.UseVisualStyleBackColor = true;
            this.plcBtn5.Click += new System.EventHandler(this.plcBtn5_Click);
            // 
            // plcBtn4
            // 
            this.plcBtn4.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.plcBtn4.Btn_Model = ErosSocket.ErosUI.PLCBtn.BtnModel.交替;
            this.plcBtn4.FalseColor = System.Drawing.Color.DimGray;
            this.plcBtn4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.plcBtn4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.plcBtn4.GetName = null;
            this.plcBtn4.Location = new System.Drawing.Point(143, 85);
            this.plcBtn4.Name = "plcBtn4";
            this.plcBtn4.SetName = null;
            this.plcBtn4.Size = new System.Drawing.Size(75, 25);
            this.plcBtn4.TabIndex = 22;
            this.plcBtn4.Text = "去目标位置";
            this.plcBtn4.TrueColor = System.Drawing.Color.Lime;
            this.plcBtn4.UseVisualStyleBackColor = true;
            this.plcBtn4.Click += new System.EventHandler(this.plcBtn4_Click);
            // 
            // plcBtn3
            // 
            this.plcBtn3.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.plcBtn3.Btn_Model = ErosSocket.ErosUI.PLCBtn.BtnModel.交替;
            this.plcBtn3.FalseColor = System.Drawing.Color.DimGray;
            this.plcBtn3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.plcBtn3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.plcBtn3.GetName = null;
            this.plcBtn3.Location = new System.Drawing.Point(143, 57);
            this.plcBtn3.Name = "plcBtn3";
            this.plcBtn3.SetName = null;
            this.plcBtn3.Size = new System.Drawing.Size(75, 25);
            this.plcBtn3.TabIndex = 21;
            this.plcBtn3.Text = "同步原点";
            this.plcBtn3.TrueColor = System.Drawing.Color.Lime;
            this.plcBtn3.UseVisualStyleBackColor = true;
            this.plcBtn3.Click += new System.EventHandler(this.plcBtn3_Click);
            // 
            // plcBtn2
            // 
            this.plcBtn2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.plcBtn2.Btn_Model = ErosSocket.ErosUI.PLCBtn.BtnModel.交替;
            this.plcBtn2.FalseColor = System.Drawing.Color.Empty;
            this.plcBtn2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.plcBtn2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.plcBtn2.GetName = null;
            this.plcBtn2.Location = new System.Drawing.Point(75, 139);
            this.plcBtn2.Name = "plcBtn2";
            this.plcBtn2.SetName = null;
            this.plcBtn2.Size = new System.Drawing.Size(68, 36);
            this.plcBtn2.TabIndex = 17;
            this.plcBtn2.Text = "反点动";
            this.plcBtn2.TrueColor = System.Drawing.Color.Empty;
            this.plcBtn2.UseVisualStyleBackColor = true;
            this.plcBtn2.Click += new System.EventHandler(this.plcBtn2_Click_1);
            this.plcBtn2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plcBtn2_MouseDown);
            this.plcBtn2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.plcBtn2_MouseUp);
            // 
            // plcBtn1
            // 
            this.plcBtn1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.plcBtn1.Btn_Model = ErosSocket.ErosUI.PLCBtn.BtnModel.交替;
            this.plcBtn1.FalseColor = System.Drawing.Color.DimGray;
            this.plcBtn1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.plcBtn1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.plcBtn1.GetName = null;
            this.plcBtn1.Location = new System.Drawing.Point(3, 139);
            this.plcBtn1.Name = "plcBtn1";
            this.plcBtn1.SetName = null;
            this.plcBtn1.Size = new System.Drawing.Size(68, 36);
            this.plcBtn1.TabIndex = 16;
            this.plcBtn1.Text = "正点动";
            this.plcBtn1.TrueColor = System.Drawing.Color.Lime;
            this.plcBtn1.UseVisualStyleBackColor = false;
            this.plcBtn1.Click += new System.EventHandler(this.plcBtn1_Click_1);
            this.plcBtn1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plcBtn1_MouseDown);
            this.plcBtn1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.plcBtn1_MouseUp);
            // 
            // floatNumTextBox2
            // 
            this.floatNumTextBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.floatNumTextBox2.Location = new System.Drawing.Point(55, 85);
            this.floatNumTextBox2.Name = "floatNumTextBox2";
            this.floatNumTextBox2.Size = new System.Drawing.Size(87, 26);
            this.floatNumTextBox2.TabIndex = 8;
            this.floatNumTextBox2.Text = "0.00";
            this.floatNumTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.floatNumTextBox2.TextChanged += new System.EventHandler(this.floatNumTextBox2_TextChanged);
            // 
            // floatNumTextBox1
            // 
            this.floatNumTextBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.floatNumTextBox1.Location = new System.Drawing.Point(55, 55);
            this.floatNumTextBox1.Name = "floatNumTextBox1";
            this.floatNumTextBox1.ReadOnly = true;
            this.floatNumTextBox1.Size = new System.Drawing.Size(87, 26);
            this.floatNumTextBox1.TabIndex = 0;
            this.floatNumTextBox1.Text = "0.0";
            this.floatNumTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // AxisControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.plcBtn5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.plcBtn4);
            this.Controls.Add(this.plcBtn3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.plcBtn2);
            this.Controls.Add(this.plcBtn1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.labD);
            this.Controls.Add(this.labEnble);
            this.Controls.Add(this.labErrer);
            this.Controls.Add(this.labHome);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.floatNumTextBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.floatNumTextBox1);
            this.Name = "AxisControl";
            this.Size = new System.Drawing.Size(231, 178);
            this.Load += new System.EventHandler(this.Axis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ErosSocket.ErosUI.FloatNumTextBox floatNumTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private ErosSocket.ErosUI.FloatNumTextBox floatNumTextBox2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label labHome;
        private System.Windows.Forms.Label labErrer;
        private System.Windows.Forms.Label labEnble;
        private System.Windows.Forms.Label labD;
        private System.Windows.Forms.Button button3;
        private ErosSocket.ErosUI.PLCBtn plcBtn1;
        private ErosSocket.ErosUI.PLCBtn plcBtn2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label8;
        private ErosSocket.ErosUI.PLCBtn plcBtn3;
        private ErosSocket.ErosUI.PLCBtn plcBtn4;
        private System.Windows.Forms.Label label9;
        private ErosSocket.ErosUI.PLCBtn plcBtn5;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
    }
}
