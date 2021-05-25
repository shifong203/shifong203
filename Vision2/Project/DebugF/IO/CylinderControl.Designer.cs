namespace Vision2.Project.DebugF.IO
{
    partial class CylinderControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.Silabel = new System.Windows.Forms.Label();
            this.EiLabel = new System.Windows.Forms.Label();
            this.SQlabel = new System.Windows.Forms.Label();
            this.Eqlabel = new System.Windows.Forms.Label();
            this.plcButtonV21 = new System.Windows.Forms.Button();
            this.plcButtonV22 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "XX气缸";
            // 
            // Silabel
            // 
            this.Silabel.AutoSize = true;
            this.Silabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Silabel.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Silabel.Location = new System.Drawing.Point(55, 75);
            this.Silabel.Name = "Silabel";
            this.Silabel.Size = new System.Drawing.Size(21, 21);
            this.Silabel.TabIndex = 3;
            this.Silabel.Text = "I";
            // 
            // EiLabel
            // 
            this.EiLabel.AutoSize = true;
            this.EiLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.EiLabel.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EiLabel.Location = new System.Drawing.Point(147, 75);
            this.EiLabel.Name = "EiLabel";
            this.EiLabel.Size = new System.Drawing.Size(21, 21);
            this.EiLabel.TabIndex = 4;
            this.EiLabel.Text = "I";
            // 
            // SQlabel
            // 
            this.SQlabel.AutoSize = true;
            this.SQlabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.SQlabel.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SQlabel.Location = new System.Drawing.Point(8, 75);
            this.SQlabel.Name = "SQlabel";
            this.SQlabel.Size = new System.Drawing.Size(21, 21);
            this.SQlabel.TabIndex = 5;
            this.SQlabel.Text = "Q";
            // 
            // Eqlabel
            // 
            this.Eqlabel.AutoSize = true;
            this.Eqlabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Eqlabel.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Eqlabel.Location = new System.Drawing.Point(96, 75);
            this.Eqlabel.Name = "Eqlabel";
            this.Eqlabel.Size = new System.Drawing.Size(21, 21);
            this.Eqlabel.TabIndex = 6;
            this.Eqlabel.Text = "Q";
            // 
            // plcButtonV21
            // 
    
            this.plcButtonV21.Location = new System.Drawing.Point(91, 34);
            this.plcButtonV21.Name = "plcButtonV21";
            this.plcButtonV21.Size = new System.Drawing.Size(75, 38);
            this.plcButtonV21.TabIndex = 7;
            this.plcButtonV21.Text = "缩回";
            this.plcButtonV21.UseVisualStyleBackColor = false;
            this.plcButtonV21.Click += new System.EventHandler(this.plcButtonV21_Click);
            // 
            // plcButtonV22
            // 
           
            this.plcButtonV22.Location = new System.Drawing.Point(4, 34);
            this.plcButtonV22.Name = "plcButtonV22";
            this.plcButtonV22.Size = new System.Drawing.Size(75, 38);
            this.plcButtonV22.TabIndex = 8;
            this.plcButtonV22.Text = "伸出";
            this.plcButtonV22.UseVisualStyleBackColor = false;
            this.plcButtonV22.Click += new System.EventHandler(this.plcButtonV22_Click);
            // 
            // CylinderControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.plcButtonV22);
            this.Controls.Add(this.plcButtonV21);
            this.Controls.Add(this.Eqlabel);
            this.Controls.Add(this.SQlabel);
            this.Controls.Add(this.EiLabel);
            this.Controls.Add(this.Silabel);
            this.Controls.Add(this.label1);
            this.Name = "CylinderControl";
            this.Size = new System.Drawing.Size(177, 104);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UserControl1_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Silabel;
        private System.Windows.Forms.Label EiLabel;
        private System.Windows.Forms.Label SQlabel;
        private System.Windows.Forms.Label Eqlabel;
        private System.Windows.Forms.Button plcButtonV21;
        private System.Windows.Forms.Button plcButtonV22;
    }
}
