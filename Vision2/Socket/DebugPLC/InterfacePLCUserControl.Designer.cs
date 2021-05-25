namespace ErosSocket.DebugPLC
{
    partial class InterfacePLCUserControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.Btn_Start = new System.Windows.Forms.Button();
            this.Btn_Stop = new System.Windows.Forms.Button();
            this.labelAram = new System.Windows.Forms.Label();
            this.Btn_Initialize = new System.Windows.Forms.Button();
            this.labelStat = new System.Windows.Forms.Label();
            this.Btn_Debug = new System.Windows.Forms.Button();
            this.Btn_Reset = new System.Windows.Forms.Button();
            this.Btn_Pause = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Btn_Start);
            this.panel1.Controls.Add(this.Btn_Stop);
            this.panel1.Controls.Add(this.labelAram);
            this.panel1.Controls.Add(this.Btn_Initialize);
            this.panel1.Controls.Add(this.labelStat);
            this.panel1.Controls.Add(this.Btn_Debug);
            this.panel1.Controls.Add(this.Btn_Reset);
            this.panel1.Controls.Add(this.Btn_Pause);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(372, 98);
            this.panel1.TabIndex = 12;
            // 
            // Btn_Start
            // 
            this.Btn_Start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Btn_Start.Font = new System.Drawing.Font("华文中宋", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Start.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Btn_Start.Location = new System.Drawing.Point(3, 1);
            this.Btn_Start.Name = "Btn_Start";
            this.Btn_Start.Size = new System.Drawing.Size(90, 49);
            this.Btn_Start.TabIndex = 0;
            this.Btn_Start.Text = "启动";
            this.Btn_Start.UseVisualStyleBackColor = false;
            this.Btn_Start.Click += new System.EventHandler(this.Btn_Start_Click);
            // 
            // Btn_Stop
            // 
            this.Btn_Stop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Btn_Stop.Font = new System.Drawing.Font("华文中宋", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Stop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Btn_Stop.Location = new System.Drawing.Point(94, 1);
            this.Btn_Stop.Name = "Btn_Stop";
            this.Btn_Stop.Size = new System.Drawing.Size(90, 49);
            this.Btn_Stop.TabIndex = 1;
            this.Btn_Stop.Text = "停止";
            this.Btn_Stop.UseVisualStyleBackColor = false;
            // 
            // labelAram
            // 
            this.labelAram.AutoSize = true;
            this.labelAram.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelAram.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelAram.Location = new System.Drawing.Point(294, 54);
            this.labelAram.Name = "labelAram";
            this.labelAram.Size = new System.Drawing.Size(73, 29);
            this.labelAram.TabIndex = 9;
            this.labelAram.Text = "正常";
            // 
            // Btn_Initialize
            // 
            this.Btn_Initialize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Btn_Initialize.Font = new System.Drawing.Font("华文中宋", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Initialize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Btn_Initialize.Location = new System.Drawing.Point(185, 1);
            this.Btn_Initialize.Name = "Btn_Initialize";
            this.Btn_Initialize.Size = new System.Drawing.Size(111, 49);
            this.Btn_Initialize.TabIndex = 3;
            this.Btn_Initialize.Text = "初始化";
            this.Btn_Initialize.UseVisualStyleBackColor = false;
            // 
            // labelStat
            // 
            this.labelStat.AutoSize = true;
            this.labelStat.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelStat.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelStat.Location = new System.Drawing.Point(294, 10);
            this.labelStat.Name = "labelStat";
            this.labelStat.Size = new System.Drawing.Size(73, 29);
            this.labelStat.TabIndex = 8;
            this.labelStat.Text = "状态";
            // 
            // Btn_Debug
            // 
            this.Btn_Debug.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Btn_Debug.Font = new System.Drawing.Font("华文中宋", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Debug.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Btn_Debug.Location = new System.Drawing.Point(186, 47);
            this.Btn_Debug.Name = "Btn_Debug";
            this.Btn_Debug.Size = new System.Drawing.Size(110, 47);
            this.Btn_Debug.TabIndex = 4;
            this.Btn_Debug.Text = "调试";
            this.Btn_Debug.UseVisualStyleBackColor = false;
            // 
            // Btn_Reset
            // 
            this.Btn_Reset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Btn_Reset.Font = new System.Drawing.Font("华文中宋", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Reset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Btn_Reset.Location = new System.Drawing.Point(94, 47);
            this.Btn_Reset.Name = "Btn_Reset";
            this.Btn_Reset.Size = new System.Drawing.Size(90, 49);
            this.Btn_Reset.TabIndex = 6;
            this.Btn_Reset.Text = "复位";
            this.Btn_Reset.UseVisualStyleBackColor = false;
            // 
            // Btn_Pause
            // 
            this.Btn_Pause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Btn_Pause.Font = new System.Drawing.Font("华文中宋", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Pause.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Btn_Pause.Location = new System.Drawing.Point(2, 47);
            this.Btn_Pause.Name = "Btn_Pause";
            this.Btn_Pause.Size = new System.Drawing.Size(90, 49);
            this.Btn_Pause.TabIndex = 5;
            this.Btn_Pause.Text = "暂停";
            this.Btn_Pause.UseVisualStyleBackColor = false;
            // 
            // InterfacePLCUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Controls.Add(this.panel1);
            this.Name = "InterfacePLCUserControl";
            this.Size = new System.Drawing.Size(372, 97);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Button Btn_Start;
        public System.Windows.Forms.Button Btn_Stop;
        public System.Windows.Forms.Label labelAram;
        public System.Windows.Forms.Button Btn_Initialize;
        public System.Windows.Forms.Label labelStat;
        public System.Windows.Forms.Button Btn_Debug;
        public System.Windows.Forms.Button Btn_Reset;
        public System.Windows.Forms.Button Btn_Pause;
    }
}
