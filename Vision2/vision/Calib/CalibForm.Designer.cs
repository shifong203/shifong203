namespace Vision2.vision.Calib
{
    partial class CalibForm
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
            this.calidControl1 = new Vision2.vision.Calib.CalidControl();
            this.SuspendLayout();
            // 
            // calidControl1
            // 
            this.calidControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calidControl1.Location = new System.Drawing.Point(0, 0);
            this.calidControl1.Name = "calidControl1";
            this.calidControl1.Size = new System.Drawing.Size(1015, 526);
            this.calidControl1.TabIndex = 0;
            // 
            // CalibForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 526);
            this.Controls.Add(this.calidControl1);
            this.Name = "CalibForm";
            this.Text = "CalibForm";
            this.ResumeLayout(false);
        }
        #endregion
        public CalidControl calidControl1;
    }
}