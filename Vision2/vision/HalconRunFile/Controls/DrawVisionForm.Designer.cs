﻿
namespace Vision2.vision.HalconRunFile.Controls
{
    partial class DrawVisionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawVisionForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripTrackBar1 = new Vision2.ErosProjcetDLL.UI.ToolStrip.ToolStripTrackBar();
            this.tsButton1 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.tsButton3 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.tsButton6 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.移动掩模区域ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清除掩模区域ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制掩模区域ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制掩模方型ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripTrackBar1,
            this.tsButton1,
            this.tsButton3,
            this.tsButton6,
            this.toolStripButton3,
            this.toolStripButton2,
            this.toolStripSplitButton1,
            this.toolStripComboBox1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1173, 48);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "绘制区域";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(54, 45);
            this.toolStripDropDownButton1.Text = "执行";
            this.toolStripDropDownButton1.Click += new System.EventHandler(this.toolStripDropDownButton1_Click);
            // 
            // toolStripTrackBar1
            // 
            this.toolStripTrackBar1.Name = "toolStripTrackBar1";
            this.toolStripTrackBar1.Size = new System.Drawing.Size(104, 45);
            this.toolStripTrackBar1.Text = "toolStripTrackBar1";
            // 
            // tsButton1
            // 
            this.tsButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsButton1.Image = global::Vision2.Properties.Resources.brush;
            this.tsButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton1.IsCher = false;
            this.tsButton1.Name = "tsButton1";
            this.tsButton1.Size = new System.Drawing.Size(68, 45);
            this.tsButton1.Text = "涂抹掩模";
            this.tsButton1.Click += new System.EventHandler(this.tsButton1_Click);
            // 
            // tsButton3
            // 
            this.tsButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsButton3.Image = global::Vision2.Properties.Resources.cut;
            this.tsButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton3.IsCher = false;
            this.tsButton3.Name = "tsButton3";
            this.tsButton3.Size = new System.Drawing.Size(68, 45);
            this.tsButton3.Text = "擦除掩模";
            this.tsButton3.Click += new System.EventHandler(this.tsButton3_Click);
            // 
            // tsButton6
            // 
            this.tsButton6.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsButton6.Image = ((System.Drawing.Image)(resources.GetObject("tsButton6.Image")));
            this.tsButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton6.IsCher = false;
            this.tsButton6.Name = "tsButton6";
            this.tsButton6.Size = new System.Drawing.Size(61, 45);
            this.tsButton6.Text = "帮助";
            this.tsButton6.Click += new System.EventHandler(this.tsButton6_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(74, 45);
            this.toolStripButton3.Text = "涂抹ROI";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(74, 45);
            this.toolStripButton2.Text = "擦除ROI";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.移动掩模区域ToolStripMenuItem,
            this.清除掩模区域ToolStripMenuItem,
            this.绘制掩模区域ToolStripMenuItem,
            this.绘制掩模方型ToolStripMenuItem1});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(105, 45);
            this.toolStripSplitButton1.Text = "动态参数";
            this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
            // 
            // 移动掩模区域ToolStripMenuItem
            // 
            this.移动掩模区域ToolStripMenuItem.Name = "移动掩模区域ToolStripMenuItem";
            this.移动掩模区域ToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.移动掩模区域ToolStripMenuItem.Text = "移动掩模区域";
            this.移动掩模区域ToolStripMenuItem.Click += new System.EventHandler(this.移动掩模区域ToolStripMenuItem_Click);
            // 
            // 清除掩模区域ToolStripMenuItem
            // 
            this.清除掩模区域ToolStripMenuItem.Name = "清除掩模区域ToolStripMenuItem";
            this.清除掩模区域ToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.清除掩模区域ToolStripMenuItem.Text = "清除掩模区域";
            this.清除掩模区域ToolStripMenuItem.Click += new System.EventHandler(this.清除掩模区域ToolStripMenuItem_Click);
            // 
            // 绘制掩模区域ToolStripMenuItem
            // 
            this.绘制掩模区域ToolStripMenuItem.Name = "绘制掩模区域ToolStripMenuItem";
            this.绘制掩模区域ToolStripMenuItem.Size = new System.Drawing.Size(180, 24);
            this.绘制掩模区域ToolStripMenuItem.Text = "绘制掩模区域";
            this.绘制掩模区域ToolStripMenuItem.Click += new System.EventHandler(this.绘制掩模区域ToolStripMenuItem_Click);
            // 
            // 绘制掩模方型ToolStripMenuItem1
            // 
            this.绘制掩模方型ToolStripMenuItem1.Name = "绘制掩模方型ToolStripMenuItem1";
            this.绘制掩模方型ToolStripMenuItem1.Size = new System.Drawing.Size(180, 24);
            this.绘制掩模方型ToolStripMenuItem1.Text = "绘制掩模方型";
            this.绘制掩模方型ToolStripMenuItem1.Click += new System.EventHandler(this.绘制掩模方型ToolStripMenuItem1_Click);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(80, 48);
            this.toolStripComboBox1.DropDownClosed += new System.EventHandler(this.toolStripComboBox1_DropDownClosed);
            this.toolStripComboBox1.Click += new System.EventHandler(this.toolStripComboBox1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 48);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.hWindowControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1173, 587);
            this.splitContainer1.SplitterDistance = 786;
            this.splitContainer1.TabIndex = 4;
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(786, 587);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(786, 587);
            // 
            // DrawVisionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1173, 635);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "DrawVisionForm";
            this.Text = "DrawVisionForm";
            this.Load += new System.EventHandler(this.DrawVisionForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private ErosProjcetDLL.UI.ToolStrip.ToolStripTrackBar toolStripTrackBar1;
        private ErosProjcetDLL.UI.ToolStrip.TSButton tsButton1;
        private ErosProjcetDLL.UI.ToolStrip.TSButton tsButton3;
        private ErosProjcetDLL.UI.ToolStrip.TSButton tsButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem 移动掩模区域ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清除掩模区域ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制掩模区域ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制掩模方型ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private HalconDotNet.HWindowControl hWindowControl1;
    }
}