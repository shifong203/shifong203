using ErosSocket.DebugPLC.Robot;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Vision2.Project.DebugF.IO
{
	partial class MP_C154Form1
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.runCodeUserControl1 = new Vision2.Project.DebugF.IO.RunCodeUserControl();
            this.tabPage15 = new System.Windows.Forms.TabPage();
            this.runCodeUserControl2 = new Vision2.Project.DebugF.IO.RunCodeUserControl();
            this.tabPage16 = new System.Windows.Forms.TabPage();
            this.runCodeUserControl3 = new Vision2.Project.DebugF.IO.RunCodeUserControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.trayControl1 = new ErosSocket.DebugPLC.Robot.TrayControl();
            this.propertyGrid3 = new System.Windows.Forms.PropertyGrid();
            this.listBox5 = new System.Windows.Forms.ListBox();
            this.contextMenuSt托盘 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出到产品位置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出到全局位置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox6 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip5 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除矩阵ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label16 = new System.Windows.Forms.Label();
            this.propertyGrid4 = new System.Windows.Forms.PropertyGrid();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.tabPage17 = new System.Windows.Forms.TabPage();
            this.runCodeUserControl4 = new Vision2.Project.DebugF.IO.RunCodeUserControl();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewComboBoxColumn5 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn6 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn7 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加脚本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage15.SuspendLayout();
            this.tabPage16.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.contextMenuSt托盘.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            this.tabPage17.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage15);
            this.tabControl1.Controls.Add(this.tabPage16);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage17);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1341, 471);
            this.tabControl1.TabIndex = 41;
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Controls.Add(this.flowLayoutPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1333, 445);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "调试";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1327, 439);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.runCodeUserControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1333, 445);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "流程调试";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // runCodeUserControl1
            // 
            this.runCodeUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runCodeUserControl1.Location = new System.Drawing.Point(3, 3);
            this.runCodeUserControl1.Name = "runCodeUserControl1";
            this.runCodeUserControl1.Size = new System.Drawing.Size(1327, 439);
            this.runCodeUserControl1.TabIndex = 0;
            // 
            // tabPage15
            // 
            this.tabPage15.Controls.Add(this.runCodeUserControl2);
            this.tabPage15.Location = new System.Drawing.Point(4, 22);
            this.tabPage15.Name = "tabPage15";
            this.tabPage15.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage15.Size = new System.Drawing.Size(1333, 445);
            this.tabPage15.TabIndex = 9;
            this.tabPage15.Text = "初始化流程";
            this.tabPage15.UseVisualStyleBackColor = true;
            // 
            // runCodeUserControl2
            // 
            this.runCodeUserControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runCodeUserControl2.Location = new System.Drawing.Point(3, 3);
            this.runCodeUserControl2.Name = "runCodeUserControl2";
            this.runCodeUserControl2.Size = new System.Drawing.Size(1327, 439);
            this.runCodeUserControl2.TabIndex = 1;
            // 
            // tabPage16
            // 
            this.tabPage16.Controls.Add(this.runCodeUserControl3);
            this.tabPage16.Location = new System.Drawing.Point(4, 22);
            this.tabPage16.Name = "tabPage16";
            this.tabPage16.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage16.Size = new System.Drawing.Size(1333, 445);
            this.tabPage16.TabIndex = 10;
            this.tabPage16.Text = "停止流程";
            this.tabPage16.UseVisualStyleBackColor = true;
            // 
            // runCodeUserControl3
            // 
            this.runCodeUserControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runCodeUserControl3.Location = new System.Drawing.Point(3, 3);
            this.runCodeUserControl3.Name = "runCodeUserControl3";
            this.runCodeUserControl3.Size = new System.Drawing.Size(1327, 439);
            this.runCodeUserControl3.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.trayControl1);
            this.tabPage3.Controls.Add(this.propertyGrid3);
            this.tabPage3.Controls.Add(this.listBox5);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1333, 445);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "托盘调试";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // trayControl1
            // 
            this.trayControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trayControl1.Location = new System.Drawing.Point(123, 3);
            this.trayControl1.Name = "trayControl1";
            this.trayControl1.Size = new System.Drawing.Size(1207, 221);
            this.trayControl1.TabIndex = 48;
            // 
            // propertyGrid3
            // 
            this.propertyGrid3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.propertyGrid3.Location = new System.Drawing.Point(123, 224);
            this.propertyGrid3.Name = "propertyGrid3";
            this.propertyGrid3.Size = new System.Drawing.Size(1207, 218);
            this.propertyGrid3.TabIndex = 50;
            // 
            // listBox5
            // 
            this.listBox5.ContextMenuStrip = this.contextMenuSt托盘;
            this.listBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBox5.FormattingEnabled = true;
            this.listBox5.ItemHeight = 12;
            this.listBox5.Location = new System.Drawing.Point(3, 3);
            this.listBox5.Name = "listBox5";
            this.listBox5.Size = new System.Drawing.Size(120, 439);
            this.listBox5.TabIndex = 49;
            this.listBox5.SelectedIndexChanged += new System.EventHandler(this.listBox5_SelectedIndexChanged);
            // 
            // contextMenuSt托盘
            // 
            this.contextMenuSt托盘.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出到产品位置ToolStripMenuItem,
            this.导出到全局位置ToolStripMenuItem});
            this.contextMenuSt托盘.Name = "contextMenuSt托盘";
            this.contextMenuSt托盘.Size = new System.Drawing.Size(161, 48);
            // 
            // 导出到产品位置ToolStripMenuItem
            // 
            this.导出到产品位置ToolStripMenuItem.Name = "导出到产品位置ToolStripMenuItem";
            this.导出到产品位置ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.导出到产品位置ToolStripMenuItem.Text = "导出到产品位置";
            this.导出到产品位置ToolStripMenuItem.Click += new System.EventHandler(this.导出到产品位置ToolStripMenuItem_Click);
            // 
            // 导出到全局位置ToolStripMenuItem
            // 
            this.导出到全局位置ToolStripMenuItem.Name = "导出到全局位置ToolStripMenuItem";
            this.导出到全局位置ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.导出到全局位置ToolStripMenuItem.Text = "导出到全局位置";
            this.导出到全局位置ToolStripMenuItem.Click += new System.EventHandler(this.导出到全局位置ToolStripMenuItem_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.hWindowControl1);
            this.tabPage5.Controls.Add(this.panel1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1333, 445);
            this.tabPage5.TabIndex = 8;
            this.tabPage5.Text = "矩阵管理";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(195, 3);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(1135, 439);
            this.hWindowControl1.TabIndex = 54;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(1135, 439);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button8);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.listBox6);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.propertyGrid4);
            this.panel1.Controls.Add(this.numericUpDown4);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.numericUpDown3);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(192, 439);
            this.panel1.TabIndex = 58;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(86, 260);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 39);
            this.button8.TabIndex = 64;
            this.button8.Text = "导出到位置信息";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(86, 231);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 63;
            this.button7.Text = "图像平铺";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(86, 202);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 62;
            this.button6.Text = "开始";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(86, 173);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 61;
            this.button5.Text = "标定";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(86, 144);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 60;
            this.button4.Text = "定位Mark2";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(86, 115);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 59;
            this.button3.Text = "定位Mark1";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(86, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 58;
            this.button1.Text = "定位Mark";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox6
            // 
            this.listBox6.ContextMenuStrip = this.contextMenuStrip5;
            this.listBox6.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBox6.FormattingEnabled = true;
            this.listBox6.ItemHeight = 12;
            this.listBox6.Location = new System.Drawing.Point(0, 0);
            this.listBox6.Name = "listBox6";
            this.listBox6.Size = new System.Drawing.Size(76, 152);
            this.listBox6.TabIndex = 56;
            this.listBox6.SelectedIndexChanged += new System.EventHandler(this.listBox6_SelectedIndexChanged);
            // 
            // contextMenuStrip5
            // 
            this.contextMenuStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加点ToolStripMenuItem,
            this.删除矩阵ToolStripMenuItem});
            this.contextMenuStrip5.Name = "contextMenuStrip5";
            this.contextMenuStrip5.Size = new System.Drawing.Size(125, 48);
            // 
            // 添加点ToolStripMenuItem
            // 
            this.添加点ToolStripMenuItem.Name = "添加点ToolStripMenuItem";
            this.添加点ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.添加点ToolStripMenuItem.Text = "添加矩阵";
            this.添加点ToolStripMenuItem.Click += new System.EventHandler(this.添加点ToolStripMenuItem_Click);
            // 
            // 删除矩阵ToolStripMenuItem
            // 
            this.删除矩阵ToolStripMenuItem.Name = "删除矩阵ToolStripMenuItem";
            this.删除矩阵ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除矩阵ToolStripMenuItem.Text = "删除矩阵";
            this.删除矩阵ToolStripMenuItem.Click += new System.EventHandler(this.删除矩阵ToolStripMenuItem_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(83, 5);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(48, 16);
            this.label16.TabIndex = 52;
            this.label16.Text = "宽度X";
            // 
            // propertyGrid4
            // 
            this.propertyGrid4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.propertyGrid4.Location = new System.Drawing.Point(0, 152);
            this.propertyGrid4.Name = "propertyGrid4";
            this.propertyGrid4.Size = new System.Drawing.Size(192, 287);
            this.propertyGrid4.TabIndex = 57;
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(137, 3);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(46, 21);
            this.numericUpDown4.TabIndex = 50;
            this.numericUpDown4.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(86, 57);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 55;
            this.button2.Text = "计算";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(137, 30);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(46, 21);
            this.numericUpDown3.TabIndex = 51;
            this.numericUpDown3.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(83, 32);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(48, 16);
            this.label17.TabIndex = 53;
            this.label17.Text = "高度Y";
            // 
            // tabPage17
            // 
            this.tabPage17.Controls.Add(this.runCodeUserControl4);
            this.tabPage17.Location = new System.Drawing.Point(4, 22);
            this.tabPage17.Name = "tabPage17";
            this.tabPage17.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage17.Size = new System.Drawing.Size(1333, 445);
            this.tabPage17.TabIndex = 11;
            this.tabPage17.Text = "CPK脚本";
            this.tabPage17.UseVisualStyleBackColor = true;
            // 
            // runCodeUserControl4
            // 
            this.runCodeUserControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runCodeUserControl4.Location = new System.Drawing.Point(3, 3);
            this.runCodeUserControl4.Name = "runCodeUserControl4";
            this.runCodeUserControl4.Size = new System.Drawing.Size(1327, 439);
            this.runCodeUserControl4.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "点名称";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "X位置";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Y位置";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Z位置";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "执行ID";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "点名称";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn6.ToolTipText = "-1移动不拍照，首个0优先执行，-2不执行，";
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.HeaderText = "轴组名";
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            this.dataGridViewComboBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewComboBoxColumn2
            // 
            this.dataGridViewComboBoxColumn2.HeaderText = "轴组名";
            this.dataGridViewComboBoxColumn2.Name = "dataGridViewComboBoxColumn2";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "X位置";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Y位置";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn8.ToolTipText = "-1移动不拍照，首个0优先执行，-2不执行，";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Z位置";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "执行ID";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewComboBoxColumn5
            // 
            this.dataGridViewComboBoxColumn5.HeaderText = "起点集合";
            this.dataGridViewComboBoxColumn5.Name = "dataGridViewComboBoxColumn5";
            this.dataGridViewComboBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewComboBoxColumn6
            // 
            this.dataGridViewComboBoxColumn6.HeaderText = "点功能";
            this.dataGridViewComboBoxColumn6.Name = "dataGridViewComboBoxColumn6";
            this.dataGridViewComboBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewComboBoxColumn7
            // 
            this.dataGridViewComboBoxColumn7.HeaderText = "轨迹名称";
            this.dataGridViewComboBoxColumn7.Name = "dataGridViewComboBoxColumn7";
            this.dataGridViewComboBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.panel2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1333, 445);
            this.tabPage4.TabIndex = 12;
            this.tabPage4.Text = "线程脚本";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(221, 172);
            this.listBox1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.propertyGrid1);
            this.panel2.Controls.Add(this.listBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(221, 439);
            this.panel2.TabIndex = 1;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 172);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(221, 267);
            this.propertyGrid1.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加脚本ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 48);
            // 
            // 添加脚本ToolStripMenuItem
            // 
            this.添加脚本ToolStripMenuItem.Name = "添加脚本ToolStripMenuItem";
            this.添加脚本ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.添加脚本ToolStripMenuItem.Text = "添加线程";
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            // 
            // MP_C154Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1341, 471);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Name = "MP_C154Form1";
            this.Text = "MP_C154Form1";
            this.Load += new System.EventHandler(this.MP_C154Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MP_C154Form1_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage15.ResumeLayout(false);
            this.tabPage16.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.contextMenuSt托盘.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            this.tabPage17.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		// Token: 0x040008DC RID: 2268
		private TabControl tabControl1;

		// Token: 0x040008DD RID: 2269
		private TabPage tabPage1;

		// Token: 0x040008DE RID: 2270
		private TabPage tabPage2;

		// Token: 0x040008E4 RID: 2276
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x040008E5 RID: 2277
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x040008E6 RID: 2278
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x040008E7 RID: 2279
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x040008E8 RID: 2280
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        #endregion

        private TabPage tabPage3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private RunCodeUserControl runCodeUserControl1;
        private RunCodeUserControl runCodeUserControl2;
        private RunCodeUserControl runCodeUserControl3;
        private ErosSocket.DebugPLC.Robot.TrayControl trayControl1;
        private ListBox listBox5;
        private TabPage tabPage5;
        private PropertyGrid propertyGrid3;
        private PropertyGrid propertyGrid4;
        private ListBox listBox6;
        private Button button2;
        private HalconDotNet.HWindowControl hWindowControl1;
        private Label label17;
        private Label label16;
        private NumericUpDown numericUpDown3;
        private NumericUpDown numericUpDown4;
        private ContextMenuStrip contextMenuStrip5;
        private ToolStripMenuItem 添加点ToolStripMenuItem;
        private ToolStripMenuItem 删除矩阵ToolStripMenuItem;
        private Panel panel1;
        private Button button1;
        private Button button4;
        private Button button3;
        private Button button5;
        private Button button6;
        private Button button7;
        private RunCodeUserControl runCodeUserControl4;
        private Button button8;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn5;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn6;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn7;
        private ContextMenuStrip contextMenuSt托盘;
        private ToolStripMenuItem 导出到产品位置ToolStripMenuItem;
        private ToolStripMenuItem 导出到全局位置ToolStripMenuItem;
        private TabPage tabPage15;
        private TabPage tabPage16;
        private TabPage tabPage17;
        private FlowLayoutPanel flowLayoutPanel1;
        private TabPage tabPage4;
        private Panel panel2;
        private PropertyGrid propertyGrid1;
        private ListBox listBox1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem 添加脚本ToolStripMenuItem;
        private ToolStripMenuItem 删除ToolStripMenuItem;
    }
}
