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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MP_C154Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.runCodeUserControl1 = new Vision2.Project.DebugF.IO.RunCodeUserControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.runCodeUserControl2 = new Vision2.Project.DebugF.IO.RunCodeUserControl();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.runCodeUserControl3 = new Vision2.Project.DebugF.IO.RunCodeUserControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.runCodeUserControl4 = new Vision2.Project.DebugF.IO.RunCodeUserControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.didoUserControl1 = new ErosSocket.DebugPLC.DIDO.DIDOUserControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.trayControl1 = new ErosSocket.DebugPLC.Robot.TrayControl();
            this.propertyGrid3 = new System.Windows.Forms.PropertyGrid();
            this.listBox5 = new System.Windows.Forms.ListBox();
            this.contextMenuSt托盘 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出到产品位置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出到全局位置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.contextMenuStripPoint = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.移动到点位ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读取当前位置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.插入新点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加新点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabControl4 = new System.Windows.Forms.TabControl();
            this.tabPage13 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewComboBoxColumn3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn4 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.contextMenuStrip轨迹点 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.移动轨迹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读取轨迹位置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加轨迹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.插入轨迹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip轨迹 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加轨迹ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除轨迹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重命名ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tabPage14 = new System.Windows.Forms.TabPage();
            this.listBox7 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip轨迹流程 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加轨迹ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除轨迹ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.Column10 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.contextMenuStrip轨迹表格 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.hWindowControl2 = new HalconDotNet.HWindowControl();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip导航图区域 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加区域ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改区域ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripCheckbox1 = new Vision2.ErosProjcetDLL.UI.ToolStrip.ToolStripCheckbox();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.导入整图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入ExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip导航图 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加导航图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重命名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除导航图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
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
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.contextMenuSt托盘.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStripPoint.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.tabPage13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.contextMenuStrip轨迹点.SuspendLayout();
            this.contextMenuStrip轨迹.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tabPage14.SuspendLayout();
            this.contextMenuStrip轨迹流程.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.contextMenuStrip轨迹表格.SuspendLayout();
            this.tabPage12.SuspendLayout();
            this.contextMenuStrip导航图区域.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip导航图.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1484, 640);
            this.tabControl1.TabIndex = 41;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabControl2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1476, 614);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "流程调试";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage8);
            this.tabControl2.Controls.Add(this.tabPage9);
            this.tabControl2.Controls.Add(this.tabPage10);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1470, 608);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.runCodeUserControl1);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(1462, 582);
            this.tabPage8.TabIndex = 0;
            this.tabPage8.Text = "流程脚本";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // runCodeUserControl1
            // 
            this.runCodeUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runCodeUserControl1.Location = new System.Drawing.Point(3, 3);
            this.runCodeUserControl1.Name = "runCodeUserControl1";
            this.runCodeUserControl1.Size = new System.Drawing.Size(1456, 576);
            this.runCodeUserControl1.TabIndex = 0;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.runCodeUserControl2);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(1462, 582);
            this.tabPage9.TabIndex = 1;
            this.tabPage9.Text = "初始化脚本";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // runCodeUserControl2
            // 
            this.runCodeUserControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runCodeUserControl2.Location = new System.Drawing.Point(3, 3);
            this.runCodeUserControl2.Name = "runCodeUserControl2";
            this.runCodeUserControl2.Size = new System.Drawing.Size(1456, 576);
            this.runCodeUserControl2.TabIndex = 1;
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.runCodeUserControl3);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(1462, 582);
            this.tabPage10.TabIndex = 2;
            this.tabPage10.Text = "停止脚本";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // runCodeUserControl3
            // 
            this.runCodeUserControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runCodeUserControl3.Location = new System.Drawing.Point(3, 3);
            this.runCodeUserControl3.Name = "runCodeUserControl3";
            this.runCodeUserControl3.Size = new System.Drawing.Size(1456, 576);
            this.runCodeUserControl3.TabIndex = 1;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.runCodeUserControl4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1462, 582);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "CPK脚本";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // runCodeUserControl4
            // 
            this.runCodeUserControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runCodeUserControl4.Location = new System.Drawing.Point(3, 3);
            this.runCodeUserControl4.Name = "runCodeUserControl4";
            this.runCodeUserControl4.Size = new System.Drawing.Size(1456, 576);
            this.runCodeUserControl4.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.didoUserControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1476, 614);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "调试";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // didoUserControl1
            // 
            this.didoUserControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.didoUserControl1.Location = new System.Drawing.Point(1154, 3);
            this.didoUserControl1.Name = "didoUserControl1";
            this.didoUserControl1.Size = new System.Drawing.Size(319, 608);
            this.didoUserControl1.TabIndex = 4;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.trayControl1);
            this.tabPage3.Controls.Add(this.propertyGrid3);
            this.tabPage3.Controls.Add(this.listBox5);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1476, 614);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "托盘调试";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // trayControl1
            // 
            this.trayControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trayControl1.Location = new System.Drawing.Point(123, 3);
            this.trayControl1.Name = "trayControl1";
            this.trayControl1.Size = new System.Drawing.Size(1350, 390);
            this.trayControl1.TabIndex = 48;
            // 
            // propertyGrid3
            // 
            this.propertyGrid3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.propertyGrid3.Location = new System.Drawing.Point(123, 393);
            this.propertyGrid3.Name = "propertyGrid3";
            this.propertyGrid3.Size = new System.Drawing.Size(1350, 218);
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
            this.listBox5.Size = new System.Drawing.Size(120, 608);
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
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.tabControl3);
            this.tabPage7.Controls.Add(this.groupBox2);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(1476, 614);
            this.tabPage7.TabIndex = 7;
            this.tabPage7.Text = "位置信息";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage11);
            this.tabControl3.Controls.Add(this.tabPage6);
            this.tabControl3.Controls.Add(this.tabPage12);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(251, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(1222, 608);
            this.tabControl3.TabIndex = 43;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.dataGridView1);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(1214, 582);
            this.tabPage11.TabIndex = 0;
            this.tabPage11.Text = "点位";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column9,
            this.Column5,
            this.Column8,
            this.Column7,
            this.Column6});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStripPoint;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1208, 576);
            this.dataGridView1.TabIndex = 41;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "点名称";
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "X位置";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Y位置";
            this.Column3.Name = "Column3";
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Z位置";
            this.Column4.Name = "Column4";
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "U";
            this.Column9.Name = "Column9";
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "执行ID";
            this.Column5.Name = "Column5";
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.ToolTipText = "-1移动不拍照，首个0优先执行，-2不执行，";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "门型动作";
            this.Column8.Name = "Column8";
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "轴组名";
            this.Column7.Name = "Column7";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "执行";
            this.Column6.Name = "Column6";
            this.Column6.Text = "开始执行";
            this.Column6.ToolTipText = "移动到目标位置，并执行视觉程序";
            this.Column6.UseColumnTextForButtonValue = true;
            // 
            // contextMenuStripPoint
            // 
            this.contextMenuStripPoint.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.移动到点位ToolStripMenuItem,
            this.读取当前位置ToolStripMenuItem,
            this.插入新点ToolStripMenuItem,
            this.添加新点ToolStripMenuItem,
            this.删除点ToolStripMenuItem});
            this.contextMenuStripPoint.Name = "contextMenuStrip1";
            this.contextMenuStripPoint.Size = new System.Drawing.Size(149, 114);
            // 
            // 移动到点位ToolStripMenuItem
            // 
            this.移动到点位ToolStripMenuItem.Name = "移动到点位ToolStripMenuItem";
            this.移动到点位ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.移动到点位ToolStripMenuItem.Text = "移动到点位";
            this.移动到点位ToolStripMenuItem.Click += new System.EventHandler(this.移动到点位ToolStripMenuItem_Click);
            // 
            // 读取当前位置ToolStripMenuItem
            // 
            this.读取当前位置ToolStripMenuItem.Name = "读取当前位置ToolStripMenuItem";
            this.读取当前位置ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.读取当前位置ToolStripMenuItem.Text = "读取当前位置";
            this.读取当前位置ToolStripMenuItem.Click += new System.EventHandler(this.读取当前位置ToolStripMenuItem_Click);
            // 
            // 插入新点ToolStripMenuItem
            // 
            this.插入新点ToolStripMenuItem.Name = "插入新点ToolStripMenuItem";
            this.插入新点ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.插入新点ToolStripMenuItem.Text = "插入点";
            this.插入新点ToolStripMenuItem.Click += new System.EventHandler(this.插入新点ToolStripMenuItem_Click);
            // 
            // 添加新点ToolStripMenuItem
            // 
            this.添加新点ToolStripMenuItem.Name = "添加新点ToolStripMenuItem";
            this.添加新点ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.添加新点ToolStripMenuItem.Text = "添加新点";
            this.添加新点ToolStripMenuItem.Click += new System.EventHandler(this.添加新点ToolStripMenuItem_Click);
            // 
            // 删除点ToolStripMenuItem
            // 
            this.删除点ToolStripMenuItem.Name = "删除点ToolStripMenuItem";
            this.删除点ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.删除点ToolStripMenuItem.Text = "删除点";
            this.删除点ToolStripMenuItem.Click += new System.EventHandler(this.删除点ToolStripMenuItem_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.tabControl4);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(0, 42);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "轨迹点";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabControl4
            // 
            this.tabControl4.Controls.Add(this.tabPage13);
            this.tabControl4.Controls.Add(this.tabPage14);
            this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl4.Location = new System.Drawing.Point(3, 3);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new System.Drawing.Size(0, 36);
            this.tabControl4.TabIndex = 45;
            // 
            // tabPage13
            // 
            this.tabPage13.Controls.Add(this.dataGridView2);
            this.tabPage13.Controls.Add(this.listBox4);
            this.tabPage13.Controls.Add(this.toolStrip2);
            this.tabPage13.Location = new System.Drawing.Point(4, 22);
            this.tabPage13.Name = "tabPage13";
            this.tabPage13.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage13.Size = new System.Drawing.Size(0, 10);
            this.tabPage13.TabIndex = 0;
            this.tabPage13.Text = "调试轨迹";
            this.tabPage13.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewTextBoxColumn15,
            this.dataGridViewTextBoxColumn16,
            this.dataGridViewComboBoxColumn3,
            this.dataGridViewComboBoxColumn4});
            this.dataGridView2.ContextMenuStrip = this.contextMenuStrip轨迹点;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(123, 32);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(0, 0);
            this.dataGridView2.TabIndex = 42;
            this.dataGridView2.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellValueChanged);
            this.dataGridView2.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView2_DataError);
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "点名称";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "X位置";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn12.ToolTipText = "-1移动不拍照，首个0优先执行，-2不执行，";
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "Y位置";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.HeaderText = "Z位置";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.HeaderText = "U";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.HeaderText = "执行ID";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn16.ToolTipText = "-1移动不拍照，首个0优先执行，-2不执行，";
            // 
            // dataGridViewComboBoxColumn3
            // 
            this.dataGridViewComboBoxColumn3.HeaderText = "门型动作";
            this.dataGridViewComboBoxColumn3.Name = "dataGridViewComboBoxColumn3";
            this.dataGridViewComboBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewComboBoxColumn4
            // 
            this.dataGridViewComboBoxColumn4.HeaderText = "轴组名";
            this.dataGridViewComboBoxColumn4.Name = "dataGridViewComboBoxColumn4";
            // 
            // contextMenuStrip轨迹点
            // 
            this.contextMenuStrip轨迹点.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.移动轨迹ToolStripMenuItem,
            this.读取轨迹位置ToolStripMenuItem,
            this.添加轨迹ToolStripMenuItem,
            this.插入轨迹ToolStripMenuItem,
            this.删除ToolStripMenuItem1});
            this.contextMenuStrip轨迹点.Name = "contextMenuStrip4";
            this.contextMenuStrip轨迹点.Size = new System.Drawing.Size(149, 114);
            // 
            // 移动轨迹ToolStripMenuItem
            // 
            this.移动轨迹ToolStripMenuItem.Name = "移动轨迹ToolStripMenuItem";
            this.移动轨迹ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.移动轨迹ToolStripMenuItem.Text = "移动轨迹";
            this.移动轨迹ToolStripMenuItem.Click += new System.EventHandler(this.移动轨迹ToolStripMenuItem_Click);
            // 
            // 读取轨迹位置ToolStripMenuItem
            // 
            this.读取轨迹位置ToolStripMenuItem.Name = "读取轨迹位置ToolStripMenuItem";
            this.读取轨迹位置ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.读取轨迹位置ToolStripMenuItem.Text = "读取轨迹位置";
            this.读取轨迹位置ToolStripMenuItem.Click += new System.EventHandler(this.读取轨迹位置ToolStripMenuItem_Click);
            // 
            // 添加轨迹ToolStripMenuItem
            // 
            this.添加轨迹ToolStripMenuItem.Name = "添加轨迹ToolStripMenuItem";
            this.添加轨迹ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.添加轨迹ToolStripMenuItem.Text = "添加轨迹";
            this.添加轨迹ToolStripMenuItem.Click += new System.EventHandler(this.添加轨迹ToolStripMenuItem_Click);
            // 
            // 插入轨迹ToolStripMenuItem
            // 
            this.插入轨迹ToolStripMenuItem.Name = "插入轨迹ToolStripMenuItem";
            this.插入轨迹ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.插入轨迹ToolStripMenuItem.Text = "插入轨迹";
            this.插入轨迹ToolStripMenuItem.Click += new System.EventHandler(this.插入轨迹ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem1
            // 
            this.删除ToolStripMenuItem1.Name = "删除ToolStripMenuItem1";
            this.删除ToolStripMenuItem1.Size = new System.Drawing.Size(148, 22);
            this.删除ToolStripMenuItem1.Text = "删除";
            this.删除ToolStripMenuItem1.Click += new System.EventHandler(this.删除ToolStripMenuItem1_Click);
            // 
            // listBox4
            // 
            this.listBox4.ContextMenuStrip = this.contextMenuStrip轨迹;
            this.listBox4.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBox4.FormattingEnabled = true;
            this.listBox4.ItemHeight = 12;
            this.listBox4.Location = new System.Drawing.Point(3, 32);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(120, 0);
            this.listBox4.TabIndex = 44;
            this.listBox4.SelectedIndexChanged += new System.EventHandler(this.listBox4_SelectedIndexChanged);
            // 
            // contextMenuStrip轨迹
            // 
            this.contextMenuStrip轨迹.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加轨迹ToolStripMenuItem1,
            this.删除轨迹ToolStripMenuItem,
            this.重命名ToolStripMenuItem1});
            this.contextMenuStrip轨迹.Name = "contextMenuStrip7";
            this.contextMenuStrip轨迹.Size = new System.Drawing.Size(125, 70);
            // 
            // 添加轨迹ToolStripMenuItem1
            // 
            this.添加轨迹ToolStripMenuItem1.Name = "添加轨迹ToolStripMenuItem1";
            this.添加轨迹ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.添加轨迹ToolStripMenuItem1.Text = "添加轨迹";
            this.添加轨迹ToolStripMenuItem1.Click += new System.EventHandler(this.添加轨迹ToolStripMenuItem1_Click);
            // 
            // 删除轨迹ToolStripMenuItem
            // 
            this.删除轨迹ToolStripMenuItem.Name = "删除轨迹ToolStripMenuItem";
            this.删除轨迹ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除轨迹ToolStripMenuItem.Text = "删除轨迹";
            this.删除轨迹ToolStripMenuItem.Click += new System.EventHandler(this.删除轨迹ToolStripMenuItem_Click);
            // 
            // 重命名ToolStripMenuItem1
            // 
            this.重命名ToolStripMenuItem1.Name = "重命名ToolStripMenuItem1";
            this.重命名ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.重命名ToolStripMenuItem1.Text = "重命名";
            this.重命名ToolStripMenuItem1.Click += new System.EventHandler(this.重命名ToolStripMenuItem1_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.toolStripComboBox1,
            this.toolStripButton3,
            this.toolStripButton2,
            this.toolStripButton4,
            this.toolStripLabel3,
            this.toolStripButton1});
            this.toolStrip2.Location = new System.Drawing.Point(3, 3);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(0, 29);
            this.toolStrip2.TabIndex = 43;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(56, 17);
            this.toolStripLabel2.Text = "起点位置";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBox1.Click += new System.EventHandler(this.toolStripComboBox1_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(88, 21);
            this.toolStripButton3.Text = "移动到起点";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(77, 21);
            this.toolStripButton2.Text = "F6上一步";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(77, 21);
            this.toolStripButton4.Text = "F7下一步";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(47, 17);
            this.toolStripLabel3.Text = "流程步:";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(124, 21);
            this.toolStripButton1.Text = "将点位导出到轨迹";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // tabPage14
            // 
            this.tabPage14.Controls.Add(this.listBox7);
            this.tabPage14.Controls.Add(this.dataGridView3);
            this.tabPage14.Location = new System.Drawing.Point(4, 22);
            this.tabPage14.Name = "tabPage14";
            this.tabPage14.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage14.Size = new System.Drawing.Size(1200, 550);
            this.tabPage14.TabIndex = 1;
            this.tabPage14.Text = "轨迹流程";
            this.tabPage14.UseVisualStyleBackColor = true;
            // 
            // listBox7
            // 
            this.listBox7.ContextMenuStrip = this.contextMenuStrip轨迹流程;
            this.listBox7.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBox7.FormattingEnabled = true;
            this.listBox7.ItemHeight = 12;
            this.listBox7.Location = new System.Drawing.Point(3, 3);
            this.listBox7.Name = "listBox7";
            this.listBox7.Size = new System.Drawing.Size(117, 544);
            this.listBox7.TabIndex = 45;
            this.listBox7.SelectedIndexChanged += new System.EventHandler(this.listBox7_SelectedIndexChanged);
            // 
            // contextMenuStrip轨迹流程
            // 
            this.contextMenuStrip轨迹流程.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加轨迹ToolStripMenuItem2,
            this.删除轨迹ToolStripMenuItem1});
            this.contextMenuStrip轨迹流程.Name = "contextMenuStrip8";
            this.contextMenuStrip轨迹流程.Size = new System.Drawing.Size(125, 48);
            // 
            // 添加轨迹ToolStripMenuItem2
            // 
            this.添加轨迹ToolStripMenuItem2.Name = "添加轨迹ToolStripMenuItem2";
            this.添加轨迹ToolStripMenuItem2.Size = new System.Drawing.Size(124, 22);
            this.添加轨迹ToolStripMenuItem2.Text = "添加轨迹";
            this.添加轨迹ToolStripMenuItem2.Click += new System.EventHandler(this.添加轨迹ToolStripMenuItem2_Click);
            // 
            // 删除轨迹ToolStripMenuItem1
            // 
            this.删除轨迹ToolStripMenuItem1.Name = "删除轨迹ToolStripMenuItem1";
            this.删除轨迹ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.删除轨迹ToolStripMenuItem1.Text = "删除轨迹";
            this.删除轨迹ToolStripMenuItem1.Click += new System.EventHandler(this.删除轨迹ToolStripMenuItem1_Click);
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column10,
            this.Column11,
            this.Column12});
            this.dataGridView3.ContextMenuStrip = this.contextMenuStrip轨迹表格;
            this.dataGridView3.Location = new System.Drawing.Point(126, 6);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowTemplate.Height = 23;
            this.dataGridView3.Size = new System.Drawing.Size(395, 241);
            this.dataGridView3.TabIndex = 44;
            this.dataGridView3.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView3_CellContentClick);
            this.dataGridView3.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView3_CellValueChanged);
            this.dataGridView3.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView3_DataError);
            // 
            // Column10
            // 
            this.Column10.HeaderText = "起点集合";
            this.Column10.Name = "Column10";
            this.Column10.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "点功能";
            this.Column11.Name = "Column11";
            this.Column11.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "轨迹名称";
            this.Column12.Name = "Column12";
            this.Column12.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // contextMenuStrip轨迹表格
            // 
            this.contextMenuStrip轨迹表格.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem2});
            this.contextMenuStrip轨迹表格.Name = "contextMenuStrip6";
            this.contextMenuStrip轨迹表格.Size = new System.Drawing.Size(149, 26);
            // 
            // 删除ToolStripMenuItem2
            // 
            this.删除ToolStripMenuItem2.Name = "删除ToolStripMenuItem2";
            this.删除ToolStripMenuItem2.Size = new System.Drawing.Size(148, 22);
            this.删除ToolStripMenuItem2.Text = "删除轨迹流程";
            this.删除ToolStripMenuItem2.Click += new System.EventHandler(this.删除ToolStripMenuItem2_Click);
            // 
            // tabPage12
            // 
            this.tabPage12.Controls.Add(this.hWindowControl2);
            this.tabPage12.Controls.Add(this.listBox2);
            this.tabPage12.Controls.Add(this.toolStrip1);
            this.tabPage12.Controls.Add(this.groupBox1);
            this.tabPage12.Location = new System.Drawing.Point(4, 22);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(0, 42);
            this.tabPage12.TabIndex = 1;
            this.tabPage12.Text = "导航图";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // hWindowControl2
            // 
            this.hWindowControl2.BackColor = System.Drawing.Color.Black;
            this.hWindowControl2.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl2.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl2.Location = new System.Drawing.Point(251, 28);
            this.hWindowControl2.Name = "hWindowControl2";
            this.hWindowControl2.Size = new System.Drawing.Size(0, 11);
            this.hWindowControl2.TabIndex = 59;
            this.hWindowControl2.WindowSize = new System.Drawing.Size(0, 11);
            // 
            // listBox2
            // 
            this.listBox2.ContextMenuStrip = this.contextMenuStrip导航图区域;
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Location = new System.Drawing.Point(131, 28);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(120, 11);
            this.listBox2.TabIndex = 44;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // contextMenuStrip导航图区域
            // 
            this.contextMenuStrip导航图区域.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加区域ToolStripMenuItem,
            this.修改区域ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip导航图区域.Name = "contextMenuStrip2";
            this.contextMenuStrip导航图区域.Size = new System.Drawing.Size(125, 70);
            // 
            // 添加区域ToolStripMenuItem
            // 
            this.添加区域ToolStripMenuItem.Name = "添加区域ToolStripMenuItem";
            this.添加区域ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.添加区域ToolStripMenuItem.Text = "添加区域";
            this.添加区域ToolStripMenuItem.Click += new System.EventHandler(this.添加区域ToolStripMenuItem_Click);
            // 
            // 修改区域ToolStripMenuItem
            // 
            this.修改区域ToolStripMenuItem.Name = "修改区域ToolStripMenuItem";
            this.修改区域ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.修改区域ToolStripMenuItem.Text = "修改区域";
            this.修改区域ToolStripMenuItem.Click += new System.EventHandler(this.修改区域ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripCheckbox1,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(131, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(0, 25);
            this.toolStrip1.TabIndex = 60;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(44, 17);
            this.toolStripLabel1.Text = "产品名";
            // 
            // toolStripCheckbox1
            // 
            this.toolStripCheckbox1.Name = "toolStripCheckbox1";
            this.toolStripCheckbox1.Size = new System.Drawing.Size(75, 21);
            this.toolStripCheckbox1.Text = "显示位置";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入整图ToolStripMenuItem,
            this.导入ToolStripMenuItem,
            this.导入ExcelToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(61, 21);
            this.toolStripDropDownButton1.Text = "导入";
            this.toolStripDropDownButton1.Click += new System.EventHandler(this.toolStripDropDownButton1_Click);
            // 
            // 导入整图ToolStripMenuItem
            // 
            this.导入整图ToolStripMenuItem.Name = "导入整图ToolStripMenuItem";
            this.导入整图ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.导入整图ToolStripMenuItem.Text = "导入整图";
            this.导入整图ToolStripMenuItem.Click += new System.EventHandler(this.导入整图ToolStripMenuItem_Click);
            // 
            // 导入ToolStripMenuItem
            // 
            this.导入ToolStripMenuItem.Name = "导入ToolStripMenuItem";
            this.导入ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.导入ToolStripMenuItem.Text = "导入DXF";
            this.导入ToolStripMenuItem.Click += new System.EventHandler(this.导入ToolStripMenuItem_Click);
            // 
            // 导入ExcelToolStripMenuItem
            // 
            this.导入ExcelToolStripMenuItem.Name = "导入ExcelToolStripMenuItem";
            this.导入ExcelToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.导入ExcelToolStripMenuItem.Text = "导入Excel";
            this.导入ExcelToolStripMenuItem.Click += new System.EventHandler(this.导入ExcelToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(128, 36);
            this.groupBox1.TabIndex = 62;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "导航图面";
            // 
            // listBox3
            // 
            this.listBox3.ContextMenuStrip = this.contextMenuStrip导航图;
            this.listBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox3.FormattingEnabled = true;
            this.listBox3.ItemHeight = 12;
            this.listBox3.Location = new System.Drawing.Point(3, 17);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(122, 16);
            this.listBox3.TabIndex = 61;
            this.listBox3.SelectedIndexChanged += new System.EventHandler(this.listBox3_SelectedIndexChanged);
            // 
            // contextMenuStrip导航图
            // 
            this.contextMenuStrip导航图.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加导航图ToolStripMenuItem,
            this.重命名ToolStripMenuItem,
            this.删除导航图ToolStripMenuItem});
            this.contextMenuStrip导航图.Name = "contextMenuStrip3";
            this.contextMenuStrip导航图.Size = new System.Drawing.Size(137, 70);
            // 
            // 添加导航图ToolStripMenuItem
            // 
            this.添加导航图ToolStripMenuItem.Name = "添加导航图ToolStripMenuItem";
            this.添加导航图ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.添加导航图ToolStripMenuItem.Text = "添加导航图";
            this.添加导航图ToolStripMenuItem.Click += new System.EventHandler(this.添加导航图ToolStripMenuItem_Click);
            // 
            // 重命名ToolStripMenuItem
            // 
            this.重命名ToolStripMenuItem.Name = "重命名ToolStripMenuItem";
            this.重命名ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.重命名ToolStripMenuItem.Text = "重命名";
            this.重命名ToolStripMenuItem.Click += new System.EventHandler(this.重命名ToolStripMenuItem_Click);
            // 
            // 删除导航图ToolStripMenuItem
            // 
            this.删除导航图ToolStripMenuItem.Name = "删除导航图ToolStripMenuItem";
            this.删除导航图ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.删除导航图ToolStripMenuItem.Text = "删除导航图";
            this.删除导航图ToolStripMenuItem.Click += new System.EventHandler(this.删除导航图ToolStripMenuItem_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(248, 608);
            this.groupBox2.TabIndex = 46;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "产品集合";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(3, 17);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(242, 588);
            this.listBox1.TabIndex = 42;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.hWindowControl1);
            this.tabPage5.Controls.Add(this.panel1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1476, 614);
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
            this.hWindowControl1.Size = new System.Drawing.Size(1278, 608);
            this.hWindowControl1.TabIndex = 54;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(1278, 608);
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
            this.panel1.Size = new System.Drawing.Size(192, 608);
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
            this.listBox6.Size = new System.Drawing.Size(76, 321);
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
            this.propertyGrid4.Location = new System.Drawing.Point(0, 321);
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
            // MP_C154Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 640);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Name = "MP_C154Form1";
            this.Text = "MP_C154Form1";
            this.Load += new System.EventHandler(this.MP_C154Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MP_C154Form1_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.contextMenuSt托盘.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStripPoint.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabControl4.ResumeLayout(false);
            this.tabPage13.ResumeLayout(false);
            this.tabPage13.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.contextMenuStrip轨迹点.ResumeLayout(false);
            this.contextMenuStrip轨迹.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tabPage14.ResumeLayout(false);
            this.contextMenuStrip轨迹流程.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.contextMenuStrip轨迹表格.ResumeLayout(false);
            this.tabPage12.ResumeLayout(false);
            this.tabPage12.PerformLayout();
            this.contextMenuStrip导航图区域.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip导航图.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            this.ResumeLayout(false);

		}

		// Token: 0x040008DC RID: 2268
		private TabControl tabControl1;

		// Token: 0x040008DD RID: 2269
		private TabPage tabPage1;

		// Token: 0x040008DE RID: 2270
		private TabPage tabPage2;

		// Token: 0x040008DF RID: 2271
		private DataGridView dataGridView1;

		// Token: 0x040008E0 RID: 2272
		private ContextMenuStrip contextMenuStripPoint;

		// Token: 0x040008E1 RID: 2273
		private ToolStripMenuItem 移动到点位ToolStripMenuItem;

		// Token: 0x040008E2 RID: 2274
		private ToolStripMenuItem 添加新点ToolStripMenuItem;

		// Token: 0x040008E3 RID: 2275
		private ToolStripMenuItem 删除点ToolStripMenuItem;

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
        private ErosSocket.DebugPLC.DIDO.DIDOUserControl didoUserControl1;
        private ToolStripMenuItem 读取当前位置ToolStripMenuItem;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private RunCodeUserControl runCodeUserControl1;
        private RunCodeUserControl runCodeUserControl2;
        private RunCodeUserControl runCodeUserControl3;
        private TabPage tabPage7;
        private TabControl tabControl2;
        private TabPage tabPage8;
        private TabPage tabPage9;
        private TabPage tabPage10;
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
        private ListBox listBox1;
        private ToolStripMenuItem 插入新点ToolStripMenuItem;
        private TabPage tabPage4;
        private RunCodeUserControl runCodeUserControl4;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewComboBoxColumn Column8;
        private DataGridViewComboBoxColumn Column7;
        private DataGridViewButtonColumn Column6;
        private Button button8;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
        private TabControl tabControl3;
        private TabPage tabPage11;
        private TabPage tabPage12;
        private HalconDotNet.HWindowControl hWindowControl2;
        private ToolStrip toolStrip1;
        private ErosProjcetDLL.UI.ToolStrip.ToolStripCheckbox toolStripCheckbox1;
        private ToolStripLabel toolStripLabel1;
        private ListBox listBox2;
        private ContextMenuStrip contextMenuStrip导航图区域;
        private ToolStripMenuItem 添加区域ToolStripMenuItem;
        private ToolStripMenuItem 修改区域ToolStripMenuItem;
        private ToolStripMenuItem 删除ToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem 导入整图ToolStripMenuItem;
        private GroupBox groupBox1;
        private ListBox listBox3;
        private ContextMenuStrip contextMenuStrip导航图;
        private ToolStripMenuItem 重命名ToolStripMenuItem;
        private ToolStripMenuItem 添加导航图ToolStripMenuItem;
        private ToolStripMenuItem 删除导航图ToolStripMenuItem;
        private TabPage tabPage6;
        private DataGridView dataGridView2;
        private ToolStrip toolStrip2;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ContextMenuStrip contextMenuStrip轨迹点;
        private ToolStripMenuItem 移动轨迹ToolStripMenuItem;
        private ToolStripMenuItem 读取轨迹位置ToolStripMenuItem;
        private ToolStripMenuItem 添加轨迹ToolStripMenuItem;
        private ToolStripMenuItem 插入轨迹ToolStripMenuItem;
        private ToolStripMenuItem 删除ToolStripMenuItem1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripComboBox toolStripComboBox1;
        private DataGridView dataGridView3;
        private ContextMenuStrip contextMenuStrip轨迹表格;
        private ToolStripMenuItem 删除ToolStripMenuItem2;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn5;
        private ToolStripButton toolStripButton4;
        private ToolStripLabel toolStripLabel3;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn6;
        private ToolStripMenuItem 导入ToolStripMenuItem;
        private ToolStripMenuItem 导入ExcelToolStripMenuItem;
        private TabControl tabControl4;
        private TabPage tabPage13;
        private ListBox listBox4;
        private ToolStripButton toolStripButton1;
        private TabPage tabPage14;
        private DataGridViewComboBoxColumn Column10;
        private DataGridViewComboBoxColumn Column11;
        private DataGridViewComboBoxColumn Column12;
        private ListBox listBox7;
        private GroupBox groupBox2;
        private ContextMenuStrip contextMenuStrip轨迹;
        private ToolStripMenuItem 添加轨迹ToolStripMenuItem1;
        private ToolStripMenuItem 删除轨迹ToolStripMenuItem;
        private ToolStripMenuItem 重命名ToolStripMenuItem1;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn7;
        private ContextMenuStrip contextMenuStrip轨迹流程;
        private ToolStripMenuItem 添加轨迹ToolStripMenuItem2;
        private ToolStripMenuItem 删除轨迹ToolStripMenuItem1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn3;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn4;
        private ContextMenuStrip contextMenuSt托盘;
        private ToolStripMenuItem 导出到产品位置ToolStripMenuItem;
        private ToolStripMenuItem 导出到全局位置ToolStripMenuItem;
    }
}
