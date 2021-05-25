namespace NokidaE.DebugF
{
    partial class FormTextProgram
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.userInterfaceControl1 = new NokidaE.DebugF.UserInterfaceControl();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.ItemSize = new System.Drawing.Size(40, 30);
            this.tabControl1.Location = new System.Drawing.Point(0, 115);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(422, 608);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 1;
            this.tabControl1.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.tabControl1_ControlAdded);
            // 
            // userInterfaceControl1
            // 
            this.userInterfaceControl1.AutoScroll = true;
            this.userInterfaceControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.userInterfaceControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userInterfaceControl1.Location = new System.Drawing.Point(0, 0);
            this.userInterfaceControl1.Name = "userInterfaceControl1";
            this.userInterfaceControl1.Size = new System.Drawing.Size(422, 115);
            this.userInterfaceControl1.TabIndex = 0;
            this.userInterfaceControl1.Load += new System.EventHandler(this.userInterfaceControl1_Load);
            // 
            // FormTextProgram
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(422, 735);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.userInterfaceControl1);
            this.Name = "FormTextProgram";
            this.Text = "FormTextProgram";
            this.Load += new System.EventHandler(this.FormTextProgram_Load);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.TabControl tabControl1;
        public UserInterfaceControl userInterfaceControl1;
    }
}