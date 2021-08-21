namespace ErosSocket.DebugPLC.Robot
{
    partial class DebugRobot
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
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.steppingControl1 = new ErosSocket.DebugPLC.Robot.SteppingControl();
            this.SuspendLayout();
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.Location = new System.Drawing.Point(0, 756);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1074, 25);
            this.toolStrip2.TabIndex = 5;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // steppingControl1
            // 
            this.steppingControl1.AutoScroll = true;
            this.steppingControl1.AutoSize = true;
            this.steppingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.steppingControl1.Location = new System.Drawing.Point(0, 0);
            this.steppingControl1.Name = "steppingControl1";
            this.steppingControl1.Size = new System.Drawing.Size(1074, 756);
            this.steppingControl1.TabIndex = 6;
            this.steppingControl1.Load += new System.EventHandler(this.steppingControl1_Load);
            // 
            // DebugRobot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1074, 781);
            this.Controls.Add(this.steppingControl1);
            this.Controls.Add(this.toolStrip2);
            this.KeyPreview = true;
            this.Name = "DebugRobot";
            this.Text = "DebugRobot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugRobot_FormClosing);
            this.Load += new System.EventHandler(this.DebugRobot_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DebugRobot_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip2;
        private ErosSocket.DebugPLC.Robot.SteppingControl steppingControl1;
    }
}