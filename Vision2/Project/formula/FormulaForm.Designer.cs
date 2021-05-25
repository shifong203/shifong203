namespace Vision2.Project.formula
{
    partial class FormulaForm
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
            this.formulaContrsl1 = new Vision2.Project.formula.FormulaContrsl();
            this.SuspendLayout();
            // 
            // formulaContrsl1
            // 
            this.formulaContrsl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formulaContrsl1.Location = new System.Drawing.Point(0, 0);
            this.formulaContrsl1.Name = "formulaContrsl1";
            this.formulaContrsl1.Size = new System.Drawing.Size(1138, 1061);
            this.formulaContrsl1.TabIndex = 0;
            this.formulaContrsl1.Load += new System.EventHandler(this.formulaContrsl1_Load);
            // 
            // FormulaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 1061);
            this.Controls.Add(this.formulaContrsl1);
            this.Name = "FormulaForm";
            this.Text = "产品管理";
            this.ResumeLayout(false);

        }

        #endregion

        private FormulaContrsl formulaContrsl1;
    }
}