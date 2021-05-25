namespace ErosSocket.DebugPLC.Robot
{
    partial class Axis4PUserControl1
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
            this.butZs = new System.Windows.Forms.Button();
            this.butZA = new System.Windows.Forms.Button();
            this.butXS = new System.Windows.Forms.Button();
            this.butXA = new System.Windows.Forms.Button();
            this.butYSd = new System.Windows.Forms.Button();
            this.butYAd = new System.Windows.Forms.Button();
            this.butUs = new System.Windows.Forms.Button();
            this.butUA = new System.Windows.Forms.Button();
            this.JogMode = new System.Windows.Forms.CheckBox();
            this.UPoint = new System.Windows.Forms.Label();
            this.YPoint = new System.Windows.Forms.Label();
            this.Zpoint = new System.Windows.Forms.Label();
            this.XPoint = new System.Windows.Forms.Label();
            this.Znumber = new System.Windows.Forms.NumericUpDown();
            this.Ynumb = new System.Windows.Forms.NumericUpDown();
            this.Unumb = new System.Windows.Forms.NumericUpDown();
            this.Xnum = new System.Windows.Forms.NumericUpDown();
            this.butStop = new System.Windows.Forms.Button();
            this.butReset = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AxisEnabled = new System.Windows.Forms.Button();
            this.XsetPBut = new System.Windows.Forms.Button();
            this.XSetPointNu = new System.Windows.Forms.NumericUpDown();
            this.YSetPointNumb = new System.Windows.Forms.NumericUpDown();
            this.YsetPBut = new System.Windows.Forms.Button();
            this.ZsetPointNumber = new System.Windows.Forms.NumericUpDown();
            this.ZsetPBut = new System.Windows.Forms.Button();
            this.UsetPointNumbe = new System.Windows.Forms.NumericUpDown();
            this.UsetPBut = new System.Windows.Forms.Button();
            this.MoveBut = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.JogZnubme = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Znumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ynumb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Unumb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Xnum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XSetPointNu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YSetPointNumb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZsetPointNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsetPointNumbe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JogZnubme)).BeginInit();
            this.SuspendLayout();
            // 
            // butZs
            // 
            this.butZs.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butZs.Location = new System.Drawing.Point(172, 170);
            this.butZs.Name = "butZs";
            this.butZs.Size = new System.Drawing.Size(64, 36);
            this.butZs.TabIndex = 87;
            this.butZs.Text = "Z-";
            this.butZs.UseVisualStyleBackColor = true;
            this.butZs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.butZs_MouseDown);
            this.butZs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.butZs_MouseUp);
            // 
            // butZA
            // 
            this.butZA.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butZA.Location = new System.Drawing.Point(172, 123);
            this.butZA.Name = "butZA";
            this.butZA.Size = new System.Drawing.Size(64, 36);
            this.butZA.TabIndex = 88;
            this.butZA.Text = "Z+";
            this.butZA.UseVisualStyleBackColor = true;
            this.butZA.MouseDown += new System.Windows.Forms.MouseEventHandler(this.butZA_MouseDown);
            this.butZA.MouseUp += new System.Windows.Forms.MouseEventHandler(this.butZA_MouseUp);
            // 
            // butXS
            // 
            this.butXS.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butXS.Location = new System.Drawing.Point(91, 160);
            this.butXS.Name = "butXS";
            this.butXS.Size = new System.Drawing.Size(66, 46);
            this.butXS.TabIndex = 84;
            this.butXS.Text = "X-";
            this.butXS.UseVisualStyleBackColor = true;
            this.butXS.MouseDown += new System.Windows.Forms.MouseEventHandler(this.butXS_MouseDown);
            this.butXS.MouseUp += new System.Windows.Forms.MouseEventHandler(this.butXS_MouseUp);
            // 
            // butXA
            // 
            this.butXA.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butXA.Location = new System.Drawing.Point(4, 160);
            this.butXA.Name = "butXA";
            this.butXA.Size = new System.Drawing.Size(66, 46);
            this.butXA.TabIndex = 83;
            this.butXA.Text = "X+";
            this.butXA.UseVisualStyleBackColor = true;
            this.butXA.MouseDown += new System.Windows.Forms.MouseEventHandler(this.butXA_MouseDown);
            this.butXA.MouseUp += new System.Windows.Forms.MouseEventHandler(this.butXA_MouseUp);
            // 
            // butYSd
            // 
            this.butYSd.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butYSd.Location = new System.Drawing.Point(48, 207);
            this.butYSd.Name = "butYSd";
            this.butYSd.Size = new System.Drawing.Size(66, 46);
            this.butYSd.TabIndex = 85;
            this.butYSd.Text = "Y-";
            this.butYSd.UseVisualStyleBackColor = true;
            this.butYSd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.butYSd_MouseDown);
            this.butYSd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.butYSd_MouseUp);
            // 
            // butYAd
            // 
            this.butYAd.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butYAd.Location = new System.Drawing.Point(48, 113);
            this.butYAd.Name = "butYAd";
            this.butYAd.Size = new System.Drawing.Size(66, 46);
            this.butYAd.TabIndex = 86;
            this.butYAd.Text = "Y+";
            this.butYAd.UseVisualStyleBackColor = true;
            this.butYAd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.butYAd_MouseDown);
            this.butYAd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.butYAd_MouseUp);
            // 
            // butUs
            // 
            this.butUs.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butUs.Location = new System.Drawing.Point(217, 215);
            this.butUs.Name = "butUs";
            this.butUs.Size = new System.Drawing.Size(64, 36);
            this.butUs.TabIndex = 89;
            this.butUs.Text = "U-";
            this.butUs.UseVisualStyleBackColor = true;
            this.butUs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.butUs_MouseDown);
            this.butUs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.butUs_MouseUp);
            // 
            // butUA
            // 
            this.butUA.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butUA.Location = new System.Drawing.Point(139, 215);
            this.butUA.Name = "butUA";
            this.butUA.Size = new System.Drawing.Size(64, 36);
            this.butUA.TabIndex = 90;
            this.butUA.Text = "U+";
            this.butUA.UseVisualStyleBackColor = true;
            this.butUA.MouseDown += new System.Windows.Forms.MouseEventHandler(this.butUA_MouseDown);
            this.butUA.MouseUp += new System.Windows.Forms.MouseEventHandler(this.butUA_MouseUp);
            // 
            // JogMode
            // 
            this.JogMode.AutoSize = true;
            this.JogMode.Location = new System.Drawing.Point(131, 101);
            this.JogMode.Name = "JogMode";
            this.JogMode.Size = new System.Drawing.Size(72, 16);
            this.JogMode.TabIndex = 101;
            this.JogMode.Text = "寸动模式";
            this.JogMode.UseVisualStyleBackColor = true;
            this.JogMode.CheckedChanged += new System.EventHandler(this.JogMode_CheckedChanged);
            // 
            // UPoint
            // 
            this.UPoint.AutoSize = true;
            this.UPoint.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UPoint.Location = new System.Drawing.Point(131, 50);
            this.UPoint.Name = "UPoint";
            this.UPoint.Size = new System.Drawing.Size(109, 21);
            this.UPoint.TabIndex = 100;
            this.UPoint.Text = "U:9999.99";
            // 
            // YPoint
            // 
            this.YPoint.AutoSize = true;
            this.YPoint.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.YPoint.Location = new System.Drawing.Point(131, 1);
            this.YPoint.Name = "YPoint";
            this.YPoint.Size = new System.Drawing.Size(109, 21);
            this.YPoint.TabIndex = 99;
            this.YPoint.Text = "Y:9999.99";
            // 
            // Zpoint
            // 
            this.Zpoint.AutoSize = true;
            this.Zpoint.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Zpoint.Location = new System.Drawing.Point(3, 51);
            this.Zpoint.Name = "Zpoint";
            this.Zpoint.Size = new System.Drawing.Size(109, 21);
            this.Zpoint.TabIndex = 98;
            this.Zpoint.Text = "Z:9999.99";
            // 
            // XPoint
            // 
            this.XPoint.AutoSize = true;
            this.XPoint.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.XPoint.Location = new System.Drawing.Point(3, 2);
            this.XPoint.Name = "XPoint";
            this.XPoint.Size = new System.Drawing.Size(109, 21);
            this.XPoint.TabIndex = 97;
            this.XPoint.Text = "X:9999.99";
            // 
            // Znumber
            // 
            this.Znumber.DecimalPlaces = 3;
            this.Znumber.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.Znumber.Location = new System.Drawing.Point(3, 76);
            this.Znumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.Znumber.Name = "Znumber";
            this.Znumber.Size = new System.Drawing.Size(70, 21);
            this.Znumber.TabIndex = 96;
            this.Znumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Znumber.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Znumber.ValueChanged += new System.EventHandler(this.Znumber_ValueChanged);
            // 
            // Ynumb
            // 
            this.Ynumb.DecimalPlaces = 3;
            this.Ynumb.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.Ynumb.Location = new System.Drawing.Point(131, 26);
            this.Ynumb.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.Ynumb.Name = "Ynumb";
            this.Ynumb.Size = new System.Drawing.Size(70, 21);
            this.Ynumb.TabIndex = 95;
            this.Ynumb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Ynumb.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Ynumb.ValueChanged += new System.EventHandler(this.Ynumb_ValueChanged);
            // 
            // Unumb
            // 
            this.Unumb.DecimalPlaces = 3;
            this.Unumb.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.Unumb.Location = new System.Drawing.Point(131, 74);
            this.Unumb.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.Unumb.Name = "Unumb";
            this.Unumb.Size = new System.Drawing.Size(70, 21);
            this.Unumb.TabIndex = 94;
            this.Unumb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Unumb.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Unumb.ValueChanged += new System.EventHandler(this.Unumb_ValueChanged);
            // 
            // Xnum
            // 
            this.Xnum.DecimalPlaces = 3;
            this.Xnum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.Xnum.Location = new System.Drawing.Point(3, 27);
            this.Xnum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.Xnum.Name = "Xnum";
            this.Xnum.Size = new System.Drawing.Size(70, 21);
            this.Xnum.TabIndex = 93;
            this.Xnum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Xnum.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Xnum.ValueChanged += new System.EventHandler(this.Xnum_ValueChanged);
            // 
            // butStop
            // 
            this.butStop.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butStop.Location = new System.Drawing.Point(251, 181);
            this.butStop.Name = "butStop";
            this.butStop.Size = new System.Drawing.Size(65, 32);
            this.butStop.TabIndex = 104;
            this.butStop.Text = "停止";
            this.butStop.UseVisualStyleBackColor = true;
            this.butStop.Click += new System.EventHandler(this.butStop_Click);
            // 
            // butReset
            // 
            this.butReset.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butReset.Location = new System.Drawing.Point(251, 103);
            this.butReset.Name = "butReset";
            this.butReset.Size = new System.Drawing.Size(65, 32);
            this.butReset.TabIndex = 103;
            this.butReset.Text = "复位";
            this.butReset.UseVisualStyleBackColor = true;
            this.butReset.Click += new System.EventHandler(this.butReset_Click);
            // 
            // btnHome
            // 
            this.btnHome.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnHome.Location = new System.Drawing.Point(251, 142);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(65, 32);
            this.btnHome.TabIndex = 102;
            this.btnHome.Text = "Home";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.label1.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(254, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 24);
            this.label1.TabIndex = 105;
            this.label1.Text = "故障";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.label2.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(254, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 24);
            this.label2.TabIndex = 106;
            this.label2.Text = "原点";
            // 
            // AxisEnabled
            // 
            this.AxisEnabled.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AxisEnabled.Location = new System.Drawing.Point(251, 66);
            this.AxisEnabled.Name = "AxisEnabled";
            this.AxisEnabled.Size = new System.Drawing.Size(65, 32);
            this.AxisEnabled.TabIndex = 107;
            this.AxisEnabled.Text = "使能";
            this.AxisEnabled.UseVisualStyleBackColor = true;
            this.AxisEnabled.Click += new System.EventHandler(this.button1_Click);
            // 
            // XsetPBut
            // 
            this.XsetPBut.Location = new System.Drawing.Point(5, 280);
            this.XsetPBut.Name = "XsetPBut";
            this.XsetPBut.Size = new System.Drawing.Size(84, 35);
            this.XsetPBut.TabIndex = 108;
            this.XsetPBut.Text = "X去目标位置";
            this.XsetPBut.UseVisualStyleBackColor = true;
            this.XsetPBut.Click += new System.EventHandler(this.XsetPBut_Click);
            // 
            // XSetPointNu
            // 
            this.XSetPointNu.DecimalPlaces = 3;
            this.XSetPointNu.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.XSetPointNu.Location = new System.Drawing.Point(5, 257);
            this.XSetPointNu.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.XSetPointNu.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.XSetPointNu.Name = "XSetPointNu";
            this.XSetPointNu.Size = new System.Drawing.Size(84, 21);
            this.XSetPointNu.TabIndex = 109;
            this.XSetPointNu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // YSetPointNumb
            // 
            this.YSetPointNumb.DecimalPlaces = 3;
            this.YSetPointNumb.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.YSetPointNumb.Location = new System.Drawing.Point(91, 257);
            this.YSetPointNumb.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.YSetPointNumb.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.YSetPointNumb.Name = "YSetPointNumb";
            this.YSetPointNumb.Size = new System.Drawing.Size(84, 21);
            this.YSetPointNumb.TabIndex = 111;
            this.YSetPointNumb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // YsetPBut
            // 
            this.YsetPBut.Location = new System.Drawing.Point(91, 280);
            this.YsetPBut.Name = "YsetPBut";
            this.YsetPBut.Size = new System.Drawing.Size(84, 35);
            this.YsetPBut.TabIndex = 110;
            this.YsetPBut.Text = "Y去目标位置";
            this.YsetPBut.UseVisualStyleBackColor = true;
            this.YsetPBut.Click += new System.EventHandler(this.YsetPBut_Click);
            // 
            // ZsetPointNumber
            // 
            this.ZsetPointNumber.DecimalPlaces = 3;
            this.ZsetPointNumber.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ZsetPointNumber.Location = new System.Drawing.Point(177, 257);
            this.ZsetPointNumber.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ZsetPointNumber.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.ZsetPointNumber.Name = "ZsetPointNumber";
            this.ZsetPointNumber.Size = new System.Drawing.Size(84, 21);
            this.ZsetPointNumber.TabIndex = 113;
            this.ZsetPointNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ZsetPBut
            // 
            this.ZsetPBut.Location = new System.Drawing.Point(177, 280);
            this.ZsetPBut.Name = "ZsetPBut";
            this.ZsetPBut.Size = new System.Drawing.Size(84, 35);
            this.ZsetPBut.TabIndex = 112;
            this.ZsetPBut.Text = "Z去目标位置";
            this.ZsetPBut.UseVisualStyleBackColor = true;
            this.ZsetPBut.Click += new System.EventHandler(this.ZsetPBut_Click);
            // 
            // UsetPointNumbe
            // 
            this.UsetPointNumbe.DecimalPlaces = 3;
            this.UsetPointNumbe.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.UsetPointNumbe.Location = new System.Drawing.Point(263, 257);
            this.UsetPointNumbe.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.UsetPointNumbe.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.UsetPointNumbe.Name = "UsetPointNumbe";
            this.UsetPointNumbe.Size = new System.Drawing.Size(84, 21);
            this.UsetPointNumbe.TabIndex = 115;
            this.UsetPointNumbe.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // UsetPBut
            // 
            this.UsetPBut.Location = new System.Drawing.Point(263, 280);
            this.UsetPBut.Name = "UsetPBut";
            this.UsetPBut.Size = new System.Drawing.Size(84, 35);
            this.UsetPBut.TabIndex = 114;
            this.UsetPBut.Text = "U去目标位置";
            this.UsetPBut.UseVisualStyleBackColor = true;
            this.UsetPBut.Click += new System.EventHandler(this.UsetPBut_Click);
            // 
            // MoveBut
            // 
            this.MoveBut.Location = new System.Drawing.Point(353, 255);
            this.MoveBut.Name = "MoveBut";
            this.MoveBut.Size = new System.Drawing.Size(54, 60);
            this.MoveBut.TabIndex = 117;
            this.MoveBut.Text = "全部 移动";
            this.MoveBut.UseVisualStyleBackColor = true;
            this.MoveBut.Click += new System.EventHandler(this.MoveBut_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(322, 225);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(85, 28);
            this.comboBox2.TabIndex = 118;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(286, 232);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 119;
            this.label7.Text = "轨迹:";
            // 
            // JogZnubme
            // 
            this.JogZnubme.DecimalPlaces = 3;
            this.JogZnubme.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.JogZnubme.Location = new System.Drawing.Point(360, 199);
            this.JogZnubme.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.JogZnubme.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.JogZnubme.Name = "JogZnubme";
            this.JogZnubme.Size = new System.Drawing.Size(70, 21);
            this.JogZnubme.TabIndex = 121;
            this.JogZnubme.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.JogZnubme.ValueChanged += new System.EventHandler(this.JogZnubme_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(320, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 120;
            this.label5.Text = "安全Z:";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(7, 319);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(422, 23);
            this.panel1.TabIndex = 122;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(353, 159);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 34);
            this.button1.TabIndex = 123;
            this.button1.Text = "执行";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(333, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(96, 153);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // Axis4PUserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.JogZnubme);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.MoveBut);
            this.Controls.Add(this.UsetPointNumbe);
            this.Controls.Add(this.UsetPBut);
            this.Controls.Add(this.ZsetPointNumber);
            this.Controls.Add(this.ZsetPBut);
            this.Controls.Add(this.YSetPointNumb);
            this.Controls.Add(this.YsetPBut);
            this.Controls.Add(this.XSetPointNu);
            this.Controls.Add(this.XsetPBut);
            this.Controls.Add(this.AxisEnabled);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butStop);
            this.Controls.Add(this.butReset);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.JogMode);
            this.Controls.Add(this.UPoint);
            this.Controls.Add(this.YPoint);
            this.Controls.Add(this.Zpoint);
            this.Controls.Add(this.XPoint);
            this.Controls.Add(this.Znumber);
            this.Controls.Add(this.Ynumb);
            this.Controls.Add(this.Unumb);
            this.Controls.Add(this.Xnum);
            this.Controls.Add(this.butUs);
            this.Controls.Add(this.butUA);
            this.Controls.Add(this.butZs);
            this.Controls.Add(this.butZA);
            this.Controls.Add(this.butXS);
            this.Controls.Add(this.butXA);
            this.Controls.Add(this.butYSd);
            this.Controls.Add(this.butYAd);
            this.Name = "Axis4PUserControl1";
            this.Size = new System.Drawing.Size(432, 346);
            ((System.ComponentModel.ISupportInitialize)(this.Znumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ynumb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Unumb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Xnum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XSetPointNu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YSetPointNumb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZsetPointNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsetPointNumbe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JogZnubme)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butZs;
        private System.Windows.Forms.Button butZA;
        private System.Windows.Forms.Button butXS;
        private System.Windows.Forms.Button butXA;
        private System.Windows.Forms.Button butYSd;
        private System.Windows.Forms.Button butYAd;
        private System.Windows.Forms.Button butUs;
        private System.Windows.Forms.Button butUA;
        private System.Windows.Forms.CheckBox JogMode;
        private System.Windows.Forms.Label UPoint;
        private System.Windows.Forms.Label YPoint;
        private System.Windows.Forms.Label Zpoint;
        private System.Windows.Forms.Label XPoint;
        private System.Windows.Forms.NumericUpDown Znumber;
        private System.Windows.Forms.NumericUpDown Ynumb;
        private System.Windows.Forms.NumericUpDown Unumb;
        private System.Windows.Forms.NumericUpDown Xnum;
        private System.Windows.Forms.Button butStop;
        private System.Windows.Forms.Button butReset;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button AxisEnabled;
        private System.Windows.Forms.Button XsetPBut;
        private System.Windows.Forms.NumericUpDown XSetPointNu;
        private System.Windows.Forms.NumericUpDown YSetPointNumb;
        private System.Windows.Forms.Button YsetPBut;
        private System.Windows.Forms.NumericUpDown ZsetPointNumber;
        private System.Windows.Forms.Button ZsetPBut;
        private System.Windows.Forms.NumericUpDown UsetPointNumbe;
        private System.Windows.Forms.Button UsetPBut;
        private System.Windows.Forms.Button MoveBut;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown JogZnubme;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}
