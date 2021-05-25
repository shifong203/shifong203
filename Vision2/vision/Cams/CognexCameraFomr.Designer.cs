//using NokidaE.vision.HalconRunFile.Controls;

//namespace NokidaE.vision.Cams
//{
//    partial class CognexCameraFomr
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CognexCameraFomr));
//            NokidaE.vision.VisionCounlrs visionCounlrs1 = new NokidaE.vision.VisionCounlrs();
//            this.button1 = new System.Windows.Forms.Button();
//            this.treeView1 = new System.Windows.Forms.TreeView();
//            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
//            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
//            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
//            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
//            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
//            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
//            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
//            this.visionUserControl1 = new NokidaE.vision.HalconRunFile.Controls.VisionUserControl();
//            this.tabControl1 = new System.Windows.Forms.TabControl();
//            this.tabPage1 = new System.Windows.Forms.TabPage();
//            this.comboBox1 = new System.Windows.Forms.ComboBox();
//            this.button5 = new System.Windows.Forms.Button();
//            this.button7 = new System.Windows.Forms.Button();
//            this.button6 = new System.Windows.Forms.Button();
//            this.button2 = new System.Windows.Forms.Button();
//            this.button3 = new System.Windows.Forms.Button();
//            this.button4 = new System.Windows.Forms.Button();
//            this.tabPage2 = new System.Windows.Forms.TabPage();
//            //this.cogRecordDisplay2 = new Cognex.VisionPro.CogRecordDisplay();
//            this.comboBox2 = new System.Windows.Forms.ComboBox();
//            this.button8 = new System.Windows.Forms.Button();
//            this.button9 = new System.Windows.Forms.Button();
//            this.button10 = new System.Windows.Forms.Button();
//            this.button11 = new System.Windows.Forms.Button();
//            this.button12 = new System.Windows.Forms.Button();
//            this.button13 = new System.Windows.Forms.Button();
//            this.button14 = new System.Windows.Forms.Button();
//            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
//            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
//            this.toolStrip1.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
//            this.splitContainer1.Panel1.SuspendLayout();
//            this.splitContainer1.Panel2.SuspendLayout();
//            this.splitContainer1.SuspendLayout();
//            this.tabControl1.SuspendLayout();
//            this.tabPage1.SuspendLayout();
//            this.tabPage2.SuspendLayout();
//            //((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay2)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // button1
//            // 
//            this.button1.Enabled = false;
//            this.button1.Location = new System.Drawing.Point(6, 91);
//            this.button1.Name = "button1";
//            this.button1.Size = new System.Drawing.Size(75, 23);
//            this.button1.TabIndex = 0;
//            this.button1.Text = "采图";
//            this.button1.UseVisualStyleBackColor = true;
//            this.button1.Click += new System.EventHandler(this.button1_Click);
//            // 
//            // treeView1
//            // 
//            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
//            this.treeView1.Location = new System.Drawing.Point(0, 0);
//            this.treeView1.Name = "treeView1";
//            this.treeView1.Size = new System.Drawing.Size(137, 305);
//            this.treeView1.TabIndex = 1;
//            // 
//            // toolStrip1
//            // 
//            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
//            this.toolStripButton3,
//            this.toolStripButton2,
//            this.toolStripButton1,
//            this.toolStripButton4,
//            this.toolStripButton5});
//            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
//            this.toolStrip1.Name = "toolStrip1";
//            this.toolStrip1.Size = new System.Drawing.Size(978, 25);
//            this.toolStrip1.TabIndex = 2;
//            this.toolStrip1.Text = "toolStrip1";
//            // 
//            // toolStripButton3
//            // 
//            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
//            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
//            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
//            this.toolStripButton3.Name = "toolStripButton3";
//            this.toolStripButton3.Size = new System.Drawing.Size(60, 22);
//            this.toolStripButton3.Text = "搜索相机";
//            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
//            // 
//            // toolStripButton2
//            // 
//            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
//            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
//            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
//            this.toolStripButton2.Name = "toolStripButton2";
//            this.toolStripButton2.Size = new System.Drawing.Size(60, 22);
//            this.toolStripButton2.Text = "保存相机";
//            // 
//            // toolStripButton1
//            // 
//            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
//            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
//            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
//            this.toolStripButton1.Name = "toolStripButton1";
//            this.toolStripButton1.Size = new System.Drawing.Size(84, 22);
//            this.toolStripButton1.Text = "读取相机参数";
//            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
//            // 
//            // toolStripButton4
//            // 
//            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
//            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
//            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
//            this.toolStripButton4.Name = "toolStripButton4";
//            this.toolStripButton4.Size = new System.Drawing.Size(60, 22);
//            this.toolStripButton4.Text = "链接相机";
//            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
//            // 
//            // richTextBox1
//            // 
//            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
//            this.richTextBox1.Name = "richTextBox1";
//            this.richTextBox1.Size = new System.Drawing.Size(534, 227);
//            this.richTextBox1.TabIndex = 3;
//            this.richTextBox1.Text = "";
//            // 
//            // splitContainer1
//            // 
//            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
//            this.splitContainer1.Name = "splitContainer1";
//            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
//            // 
//            // splitContainer1.Panel1
//            // 
//            this.splitContainer1.Panel1.Controls.Add(this.visionUserControl1);
//            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
//            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
//            // 
//            // splitContainer1.Panel2
//            // 
//            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
//            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
//            this.splitContainer1.Size = new System.Drawing.Size(978, 536);
//            this.splitContainer1.SplitterDistance = 305;
//            this.splitContainer1.TabIndex = 4;
//            // 
//            // visionUserControl1
//            // 
//            visionCounlrs1.Dock = System.Windows.Forms.DockStyle.Fill;
//            visionCounlrs1.Location = new System.Drawing.Point(417, 0);
//            visionCounlrs1.Name = "HWindowControl";
//            visionCounlrs1.Size = new System.Drawing.Size(561, 305);
//            this.visionUserControl1._VisionCounlrs = visionCounlrs1;
//            this.visionUserControl1.BackColor = System.Drawing.Color.Black;
//            this.visionUserControl1.BorderColor = System.Drawing.Color.Black;
//            this.visionUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.visionUserControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
//            this.visionUserControl1.Location = new System.Drawing.Point(417, 0);
//            this.visionUserControl1.Name = "visionUserControl1";
//            this.visionUserControl1.Size = new System.Drawing.Size(561, 305);
//            this.visionUserControl1.TabIndex = 2;
//            this.visionUserControl1.WindowSize = new System.Drawing.Size(561, 305);
//            // 
//            // tabControl1
//            // 
//            this.tabControl1.Controls.Add(this.tabPage1);
//            this.tabControl1.Controls.Add(this.tabPage2);
//            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
//            this.tabControl1.Location = new System.Drawing.Point(137, 0);
//            this.tabControl1.Name = "tabControl1";
//            this.tabControl1.SelectedIndex = 0;
//            this.tabControl1.Size = new System.Drawing.Size(280, 305);
//            this.tabControl1.TabIndex = 9;
//            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
//            // 
//            // tabPage1
//            // 
//            this.tabPage1.Controls.Add(this.comboBox1);
//            this.tabPage1.Controls.Add(this.button5);
//            this.tabPage1.Controls.Add(this.button7);
//            this.tabPage1.Controls.Add(this.button1);
//            this.tabPage1.Controls.Add(this.button6);
//            this.tabPage1.Controls.Add(this.button2);
//            this.tabPage1.Controls.Add(this.button3);
//            this.tabPage1.Controls.Add(this.button4);
//            this.tabPage1.Location = new System.Drawing.Point(4, 22);
//            this.tabPage1.Name = "tabPage1";
//            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
//            this.tabPage1.Size = new System.Drawing.Size(272, 279);
//            this.tabPage1.TabIndex = 0;
//            this.tabPage1.Text = "Basler";
//            this.tabPage1.UseVisualStyleBackColor = true;
//            // 
//            // comboBox1
//            // 
//            this.comboBox1.FormattingEnabled = true;
//            this.comboBox1.Location = new System.Drawing.Point(6, 9);
//            this.comboBox1.Name = "comboBox1";
//            this.comboBox1.Size = new System.Drawing.Size(121, 20);
//            this.comboBox1.TabIndex = 9;
//            // 
//            // button5
//            // 
//            this.button5.Location = new System.Drawing.Point(6, 35);
//            this.button5.Name = "button5";
//            this.button5.Size = new System.Drawing.Size(75, 23);
//            this.button5.TabIndex = 6;
//            this.button5.Text = "链接打开";
//            this.button5.UseVisualStyleBackColor = true;
//            this.button5.Click += new System.EventHandler(this.button5_Click);
//            // 
//            // button7
//            // 
//            this.button7.Enabled = false;
//            this.button7.Location = new System.Drawing.Point(6, 62);
//            this.button7.Name = "button7";
//            this.button7.Size = new System.Drawing.Size(75, 23);
//            this.button7.TabIndex = 8;
//            this.button7.Text = "关闭相机";
//            this.button7.UseVisualStyleBackColor = true;
//            this.button7.Click += new System.EventHandler(this.button7_Click);
//            // 
//            // button6
//            // 
//            this.button6.Enabled = false;
//            this.button6.Location = new System.Drawing.Point(6, 188);
//            this.button6.Name = "button6";
//            this.button6.Size = new System.Drawing.Size(94, 23);
//            this.button6.TabIndex = 7;
//            this.button6.Text = "读取相机参数";
//            this.button6.UseVisualStyleBackColor = true;
//            // 
//            // button2
//            // 
//            this.button2.Enabled = false;
//            this.button2.Location = new System.Drawing.Point(6, 116);
//            this.button2.Name = "button2";
//            this.button2.Size = new System.Drawing.Size(75, 23);
//            this.button2.TabIndex = 3;
//            this.button2.Text = "实时采图";
//            this.button2.UseVisualStyleBackColor = true;
//            this.button2.Click += new System.EventHandler(this.button2_Click);
//            // 
//            // button3
//            // 
//            this.button3.Enabled = false;
//            this.button3.Location = new System.Drawing.Point(6, 140);
//            this.button3.Name = "button3";
//            this.button3.Size = new System.Drawing.Size(75, 23);
//            this.button3.TabIndex = 4;
//            this.button3.Text = "停止实时";
//            this.button3.UseVisualStyleBackColor = true;
//            this.button3.Click += new System.EventHandler(this.button3_Click);
//            // 
//            // button4
//            // 
//            this.button4.Enabled = false;
//            this.button4.Location = new System.Drawing.Point(6, 164);
//            this.button4.Name = "button4";
//            this.button4.Size = new System.Drawing.Size(94, 23);
//            this.button4.TabIndex = 5;
//            this.button4.Text = "读取相机信息";
//            this.button4.UseVisualStyleBackColor = true;
//            this.button4.Click += new System.EventHandler(this.button4_Click);
//            // 
//            // tabPage2
//            // 
//            //this.tabPage2.Controls.Add(this.cogRecordDisplay2);
//            this.tabPage2.Controls.Add(this.comboBox2);
//            this.tabPage2.Controls.Add(this.button8);
//            this.tabPage2.Controls.Add(this.button9);
//            this.tabPage2.Controls.Add(this.button10);
//            this.tabPage2.Controls.Add(this.button11);
//            this.tabPage2.Controls.Add(this.button12);
//            this.tabPage2.Controls.Add(this.button13);
//            this.tabPage2.Controls.Add(this.button14);
//            this.tabPage2.Location = new System.Drawing.Point(4, 22);
//            this.tabPage2.Name = "tabPage2";
//            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
//            this.tabPage2.Size = new System.Drawing.Size(272, 279);
//            this.tabPage2.TabIndex = 1;
//            this.tabPage2.Text = "Cognex";
//            this.tabPage2.UseVisualStyleBackColor = true;
//            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
//            // 
//            // cogRecordDisplay2
//            // 
//            //this.cogRecordDisplay2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
//            //| System.Windows.Forms.AnchorStyles.Left) 
//            //| System.Windows.Forms.AnchorStyles.Right)));
//            //this.cogRecordDisplay2.ColorMapLowerClipColor = System.Drawing.Color.Black;
//            //this.cogRecordDisplay2.ColorMapLowerRoiLimit = 0D;
//            //this.cogRecordDisplay2.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
//            //this.cogRecordDisplay2.ColorMapUpperClipColor = System.Drawing.Color.Black;
//            //this.cogRecordDisplay2.ColorMapUpperRoiLimit = 1D;
//            //this.cogRecordDisplay2.Location = new System.Drawing.Point(105, 59);
//            //this.cogRecordDisplay2.Margin = new System.Windows.Forms.Padding(2);
//            //this.cogRecordDisplay2.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
//            //this.cogRecordDisplay2.MouseWheelSensitivity = 1D;
//            //this.cogRecordDisplay2.Name = "cogRecordDisplay2";
//            //this.cogRecordDisplay2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogRecordDisplay2.OcxState")));
//            //this.cogRecordDisplay2.Size = new System.Drawing.Size(159, 187);
//            //this.cogRecordDisplay2.TabIndex = 17;
//            // 
//            // comboBox2
//            // 
//            this.comboBox2.FormattingEnabled = true;
//            this.comboBox2.Location = new System.Drawing.Point(6, 6);
//            this.comboBox2.Name = "comboBox2";
//            this.comboBox2.Size = new System.Drawing.Size(121, 20);
//            this.comboBox2.TabIndex = 16;
//            // 
//            // button8
//            // 
//            this.button8.Location = new System.Drawing.Point(6, 32);
//            this.button8.Name = "button8";
//            this.button8.Size = new System.Drawing.Size(75, 23);
//            this.button8.TabIndex = 13;
//            this.button8.Text = "链接打开";
//            this.button8.UseVisualStyleBackColor = true;
//            this.button8.Click += new System.EventHandler(this.button8_Click);
//            // 
//            // button9
//            // 
//            this.button9.Enabled = false;
//            this.button9.Location = new System.Drawing.Point(6, 59);
//            this.button9.Name = "button9";
//            this.button9.Size = new System.Drawing.Size(75, 23);
//            this.button9.TabIndex = 15;
//            this.button9.Text = "关闭相机";
//            this.button9.UseVisualStyleBackColor = true;
//            this.button9.Click += new System.EventHandler(this.button9_Click);
//            // 
//            // button10
//            // 
//            this.button10.Enabled = false;
//            this.button10.Location = new System.Drawing.Point(6, 88);
//            this.button10.Name = "button10";
//            this.button10.Size = new System.Drawing.Size(75, 23);
//            this.button10.TabIndex = 9;
//            this.button10.Text = "采图";
//            this.button10.UseVisualStyleBackColor = true;
//            this.button10.Click += new System.EventHandler(this.button10_Click);
//            // 
//            // button11
//            // 
//            this.button11.Enabled = false;
//            this.button11.Location = new System.Drawing.Point(6, 185);
//            this.button11.Name = "button11";
//            this.button11.Size = new System.Drawing.Size(94, 23);
//            this.button11.TabIndex = 14;
//            this.button11.Text = "读取相机参数";
//            this.button11.UseVisualStyleBackColor = true;
//            this.button11.Click += new System.EventHandler(this.button11_Click);
//            // 
//            // button12
//            // 
//            this.button12.Enabled = false;
//            this.button12.Location = new System.Drawing.Point(6, 113);
//            this.button12.Name = "button12";
//            this.button12.Size = new System.Drawing.Size(75, 23);
//            this.button12.TabIndex = 10;
//            this.button12.Text = "实时采图";
//            this.button12.UseVisualStyleBackColor = true;
//            this.button12.Click += new System.EventHandler(this.button12_Click);
//            // 
//            // button13
//            // 
//            this.button13.Enabled = false;
//            this.button13.Location = new System.Drawing.Point(6, 137);
//            this.button13.Name = "button13";
//            this.button13.Size = new System.Drawing.Size(75, 23);
//            this.button13.TabIndex = 11;
//            this.button13.Text = "停止实时";
//            this.button13.UseVisualStyleBackColor = true;
//            this.button13.Click += new System.EventHandler(this.button13_Click);
//            // 
//            // button14
//            // 
//            this.button14.Enabled = false;
//            this.button14.Location = new System.Drawing.Point(6, 161);
//            this.button14.Name = "button14";
//            this.button14.Size = new System.Drawing.Size(94, 23);
//            this.button14.TabIndex = 12;
//            this.button14.Text = "读取相机信息";
//            this.button14.UseVisualStyleBackColor = true;
//            this.button14.Click += new System.EventHandler(this.button14_Click);
//            // 
//            // propertyGrid1
//            // 
//            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
//            this.propertyGrid1.Location = new System.Drawing.Point(534, 0);
//            this.propertyGrid1.Name = "propertyGrid1";
//            this.propertyGrid1.Size = new System.Drawing.Size(444, 227);
//            this.propertyGrid1.TabIndex = 2;
//            // 
//            // toolStripButton5
//            // 
//            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
//            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
//            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
//            this.toolStripButton5.Name = "toolStripButton5";
//            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
//            this.toolStripButton5.Text = "toolStripButton5";
//            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
//            // 
//            // CognexCameraFomr
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(978, 561);
//            this.Controls.Add(this.splitContainer1);
//            this.Controls.Add(this.toolStrip1);
//            this.Name = "CognexCameraFomr";
//            this.Text = "Cfomr";
//            this.toolStrip1.ResumeLayout(false);
//            this.toolStrip1.PerformLayout();
//            this.splitContainer1.Panel1.ResumeLayout(false);
//            this.splitContainer1.Panel2.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
//            this.splitContainer1.ResumeLayout(false);
//            this.tabControl1.ResumeLayout(false);
//            this.tabPage1.ResumeLayout(false);
//            this.tabPage2.ResumeLayout(false);
//            //((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay2)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private System.Windows.Forms.Button button1;
//        private System.Windows.Forms.TreeView treeView1;
//        private System.Windows.Forms.ToolStrip toolStrip1;
//        private System.Windows.Forms.ToolStripButton toolStripButton3;
//        private System.Windows.Forms.ToolStripButton toolStripButton2;
//        private System.Windows.Forms.ToolStripButton toolStripButton1;
//        private System.Windows.Forms.RichTextBox richTextBox1;
//        private System.Windows.Forms.SplitContainer splitContainer1;
//        private System.Windows.Forms.ToolStripButton toolStripButton4;
//        private System.Windows.Forms.PropertyGrid propertyGrid1;
//        private VisionUserControl visionUserControl1;
//        private System.Windows.Forms.Button button4;
//        private System.Windows.Forms.Button button3;
//        private System.Windows.Forms.Button button2;
//        private System.Windows.Forms.Button button5;
//        private System.Windows.Forms.Button button6;
//        private System.Windows.Forms.Button button7;
//        private System.Windows.Forms.TabControl tabControl1;
//        private System.Windows.Forms.TabPage tabPage1;
//        private System.Windows.Forms.TabPage tabPage2;
//        private System.Windows.Forms.Button button8;
//        private System.Windows.Forms.Button button9;
//        private System.Windows.Forms.Button button10;
//        private System.Windows.Forms.Button button11;
//        private System.Windows.Forms.Button button12;
//        private System.Windows.Forms.Button button13;
//        private System.Windows.Forms.Button button14;
//        private System.Windows.Forms.ComboBox comboBox1;
//        private System.Windows.Forms.ComboBox comboBox2;
//        //private Cognex.VisionPro.CogRecordDisplay cogRecordDisplay2;
//        private System.Windows.Forms.ToolStripButton toolStripButton5;
//    }
//}