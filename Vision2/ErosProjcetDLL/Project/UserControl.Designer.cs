
namespace ErosProjcetDLL.Project
{
    partial class UserControl2
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            //this.projectNodetControl1 = new ErosProjcetDLL.Project.ProjectNodetControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(40, 30);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(361, 585);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 10;
            this.tabControl1.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.tabControl1_ControlAdded);
            // 
            // tabPage1
            // 
            //this.tabPage1.Controls.Add(this.projectNodetControl1);
            //this.tabPage1.Location = new System.Drawing.Point(4, 34);
            //this.tabPage1.Name = "tabPage1";
            //this.tabPage1.Size = new System.Drawing.Size(353, 547);
            //this.tabPage1.TabIndex = 0;
            //this.tabPage1.Text = "项目管理";
            //this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // projectNodetControl1
            // 
            //this.projectNodetControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.projectNodetControl1.Location = new System.Drawing.Point(0, 0);
            //this.projectNodetControl1.Name = "projectNodetControl1";
            //this.projectNodetControl1.Size = new System.Drawing.Size(353, 547);
            //this.projectNodetControl1.TabIndex = 0;
            // 
            // UserControl2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Gray;
            this.Controls.Add(this.tabControl1);
            this.Name = "UserControl2";
            this.Size = new System.Drawing.Size(361, 585);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        //public ProjectNodetControl projectNodetControl1;
    }
}
