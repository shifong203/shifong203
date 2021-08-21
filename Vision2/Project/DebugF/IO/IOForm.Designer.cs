
namespace Vision2.Project.DebugF.IO
{
    partial class IOForm
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
            this.didoUserControl1 = new ErosSocket.DebugPLC.DIDO.DIDOUserControl();
            this.SuspendLayout();
            // 
            // didoUserControl1
            // 
            this.didoUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.didoUserControl1.Location = new System.Drawing.Point(0, 0);
            this.didoUserControl1.Name = "didoUserControl1";
            this.didoUserControl1.Size = new System.Drawing.Size(403, 624);
            this.didoUserControl1.TabIndex = 0;
            // 
            // IOForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 624);
            this.Controls.Add(this.didoUserControl1);
            this.Name = "IOForm";
            this.Text = "IOForm";
            this.Load += new System.EventHandler(this.IOForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ErosSocket.DebugPLC.DIDO.DIDOUserControl didoUserControl1;
    }
}