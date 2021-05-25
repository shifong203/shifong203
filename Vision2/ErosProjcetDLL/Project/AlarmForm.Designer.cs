namespace Vision2.ErosProjcetDLL.Project
{
    partial class AlarmForm
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
            this.alarmText1 = new Vision2.ErosProjcetDLL.Project.AlarmText();
            this.SuspendLayout();
            // 
            // alarmText1
            // 
            this.alarmText1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.alarmText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alarmText1.Location = new System.Drawing.Point(0, 0);
            this.alarmText1.Name = "alarmText1";
            this.alarmText1.Size = new System.Drawing.Size(556, 585);
            this.alarmText1.TabIndex = 0;
            // 
            // AlarmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 585);
            this.Controls.Add(this.alarmText1);
            this.Name = "AlarmForm";
            this.Text = "信息窗口";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AlarmForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private AlarmText alarmText1;
    }
}