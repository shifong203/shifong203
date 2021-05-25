using Vision2.ErosProjcetDLL.UI.ToolStrip;
using ErosSocket.DebugPLC.PLC;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Vision2.Project.DebugF.IO
{
	partial class Form1C154DIDO
	{
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


		// Token: 0x06000B36 RID: 2870 RVA: 0x0009A158 File Offset: 0x00098358
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1C154DIDO));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.axisControl1 = new ErosSocket.DebugPLC.PLC.AxisControl();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加轴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.cylinderControl1 = new Vision2.Project.DebugF.IO.CylinderControl();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip4 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加气缸ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.didoUserControl1 = new ErosSocket.DebugPLC.DIDO.DIDOUserControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.trayControl1 = new ErosSocket.DebugPLC.Robot.TrayControl();
            this.propertyGrid3 = new System.Windows.Forms.PropertyGrid();
            this.listBox5 = new System.Windows.Forms.ListBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.propertyGrid4 = new System.Windows.Forms.PropertyGrid();
            this.contextMenuStrip5 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除矩阵ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsButton1 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tsButton2 = new Vision2.ErosProjcetDLL.UI.ToolStrip.TSButton();
            this.dataGridViewComboBoxColumn2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.listBox6 = new System.Windows.Forms.ListBox();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.contextMenuStrip4.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.contextMenuStrip5.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1058, 595);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.listBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1050, 569);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "轴管理";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl2);
            this.groupBox1.Controls.Add(this.propertyGrid1);
            this.groupBox1.Controls.Add(this.axisControl1);
            this.groupBox1.Location = new System.Drawing.Point(127, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(920, 558);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "轴参数";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.HotTrack = true;
            this.tabControl2.Location = new System.Drawing.Point(416, 204);
            this.tabControl2.Multiline = true;
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(504, 339);
            this.tabControl2.TabIndex = 9;
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(496, 313);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "回原点方式";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(222, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 21);
            this.label6.TabIndex = 20;
            this.label6.Text = "3";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(18, 95);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(21, 21);
            this.label11.TabIndex = 19;
            this.label11.Text = "1";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(146, 95);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 21);
            this.label12.TabIndex = 18;
            this.label12.Text = "2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(222, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 21);
            this.label3.TabIndex = 17;
            this.label3.Text = "软件正限位";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(18, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 21);
            this.label4.TabIndex = 16;
            this.label4.Text = "软件负限位";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(146, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 21);
            this.label5.TabIndex = 15;
            this.label5.Text = "12";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(222, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 21);
            this.label9.TabIndex = 14;
            this.label9.Text = "正限位";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(18, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 21);
            this.label8.TabIndex = 13;
            this.label8.Text = "负限位";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(146, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 21);
            this.label7.TabIndex = 12;
            this.label7.Text = "零点";
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(496, 313);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "原点信号";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(6, 20);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(404, 523);
            this.propertyGrid1.TabIndex = 10;
            // 
            // axisControl1
            // 
            this.axisControl1.AutoSize = true;
            this.axisControl1.Location = new System.Drawing.Point(416, 20);
            this.axisControl1.Name = "axisControl1";
            this.axisControl1.Size = new System.Drawing.Size(231, 178);
            this.axisControl1.TabIndex = 2;
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.contextMenuStrip2;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(2, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(122, 244);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加轴ToolStripMenuItem,
            this.删除ToolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(113, 48);
            // 
            // 添加轴ToolStripMenuItem
            // 
            this.添加轴ToolStripMenuItem.Name = "添加轴ToolStripMenuItem";
            this.添加轴ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.添加轴ToolStripMenuItem.Text = "添加轴";
            this.添加轴ToolStripMenuItem.Click += new System.EventHandler(this.添加轴ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem1
            // 
            this.删除ToolStripMenuItem1.Name = "删除ToolStripMenuItem1";
            this.删除ToolStripMenuItem1.Size = new System.Drawing.Size(112, 22);
            this.删除ToolStripMenuItem1.Text = "删除";
            this.删除ToolStripMenuItem1.Click += new System.EventHandler(this.删除ToolStripMenuItem1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listBox3);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.comboBox1);
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.listBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1050, 569);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "轴组";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listBox3
            // 
            this.listBox3.ContextMenuStrip = this.contextMenuStrip3;
            this.listBox3.FormattingEnabled = true;
            this.listBox3.ItemHeight = 12;
            this.listBox3.Location = new System.Drawing.Point(473, 6);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(122, 244);
            this.listBox3.TabIndex = 11;
            this.listBox3.SelectedIndexChanged += new System.EventHandler(this.listBox3_SelectedIndexChanged);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem2});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(101, 26);
            // 
            // 删除ToolStripMenuItem2
            // 
            this.删除ToolStripMenuItem2.Name = "删除ToolStripMenuItem2";
            this.删除ToolStripMenuItem2.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem2.Text = "删除";
            this.删除ToolStripMenuItem2.Click += new System.EventHandler(this.删除ToolStripMenuItem2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(335, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "添加轴组";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(197, 70);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(109, 20);
            this.comboBox1.TabIndex = 9;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column2});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridView1.Location = new System.Drawing.Point(130, 93);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(307, 155);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "名称";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "轴定义";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "轴编号";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(197, 42);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "轴组名";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "轴类型";
            // 
            // listBox2
            // 
            this.listBox2.ContextMenuStrip = this.contextMenuStrip3;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Location = new System.Drawing.Point(2, 4);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(122, 244);
            this.listBox2.TabIndex = 2;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.cylinderControl1);
            this.tabPage5.Controls.Add(this.propertyGrid2);
            this.tabPage5.Controls.Add(this.listBox4);
            this.tabPage5.Controls.Add(this.didoUserControl1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1050, 569);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "气缸IO";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // cylinderControl1
            // 
            this.cylinderControl1.AutoSize = true;
            this.cylinderControl1.BackColor = System.Drawing.Color.Transparent;
            this.cylinderControl1.BorderColor = System.Drawing.Color.Orange;
            this.cylinderControl1.Location = new System.Drawing.Point(129, 6);
            this.cylinderControl1.Name = "cylinderControl1";
            this.cylinderControl1.Size = new System.Drawing.Size(176, 105);
            this.cylinderControl1.TabIndex = 12;
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Location = new System.Drawing.Point(129, 120);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(405, 364);
            this.propertyGrid2.TabIndex = 11;
            // 
            // listBox4
            // 
            this.listBox4.ContextMenuStrip = this.contextMenuStrip4;
            this.listBox4.FormattingEnabled = true;
            this.listBox4.ItemHeight = 12;
            this.listBox4.Location = new System.Drawing.Point(1, 2);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(122, 340);
            this.listBox4.TabIndex = 3;
            this.listBox4.SelectedIndexChanged += new System.EventHandler(this.listBox4_SelectedIndexChanged);
            // 
            // contextMenuStrip4
            // 
            this.contextMenuStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加气缸ToolStripMenuItem,
            this.删除ToolStripMenuItem3});
            this.contextMenuStrip4.Name = "contextMenuStrip4";
            this.contextMenuStrip4.Size = new System.Drawing.Size(125, 48);
            // 
            // 添加气缸ToolStripMenuItem
            // 
            this.添加气缸ToolStripMenuItem.Name = "添加气缸ToolStripMenuItem";
            this.添加气缸ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.添加气缸ToolStripMenuItem.Text = "添加气缸";
            this.添加气缸ToolStripMenuItem.Click += new System.EventHandler(this.添加气缸ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem3
            // 
            this.删除ToolStripMenuItem3.Name = "删除ToolStripMenuItem3";
            this.删除ToolStripMenuItem3.Size = new System.Drawing.Size(124, 22);
            this.删除ToolStripMenuItem3.Text = "删除";
            this.删除ToolStripMenuItem3.Click += new System.EventHandler(this.删除ToolStripMenuItem3_Click);
            // 
            // didoUserControl1
            // 
            this.didoUserControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.didoUserControl1.Location = new System.Drawing.Point(565, 3);
            this.didoUserControl1.Name = "didoUserControl1";
            this.didoUserControl1.Size = new System.Drawing.Size(482, 563);
            this.didoUserControl1.TabIndex = 4;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.trayControl1);
            this.tabPage6.Controls.Add(this.propertyGrid3);
            this.tabPage6.Controls.Add(this.listBox5);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(1050, 569);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "托盘";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // trayControl1
            // 
            this.trayControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trayControl1.Location = new System.Drawing.Point(123, 3);
            this.trayControl1.Name = "trayControl1";
            this.trayControl1.Size = new System.Drawing.Size(552, 563);
            this.trayControl1.TabIndex = 46;
            this.trayControl1.Load += new System.EventHandler(this.trayControl1_Load);
            // 
            // propertyGrid3
            // 
            this.propertyGrid3.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid3.Location = new System.Drawing.Point(675, 3);
            this.propertyGrid3.Name = "propertyGrid3";
            this.propertyGrid3.Size = new System.Drawing.Size(372, 563);
            this.propertyGrid3.TabIndex = 48;
            // 
            // listBox5
            // 
            this.listBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBox5.FormattingEnabled = true;
            this.listBox5.ItemHeight = 12;
            this.listBox5.Location = new System.Drawing.Point(3, 3);
            this.listBox5.Name = "listBox5";
            this.listBox5.Size = new System.Drawing.Size(120, 563);
            this.listBox5.TabIndex = 47;
            this.listBox5.SelectedIndexChanged += new System.EventHandler(this.listBox5_SelectedIndexChanged);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.propertyGrid4);
            this.tabPage7.Controls.Add(this.listBox6);
            this.tabPage7.Controls.Add(this.hWindowControl1);
            this.tabPage7.Controls.Add(this.label17);
            this.tabPage7.Controls.Add(this.label16);
            this.tabPage7.Controls.Add(this.numericUpDown3);
            this.tabPage7.Controls.Add(this.numericUpDown4);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(1050, 569);
            this.tabPage7.TabIndex = 4;
            this.tabPage7.Text = "矩阵";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // propertyGrid4
            // 
            this.propertyGrid4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.propertyGrid4.Location = new System.Drawing.Point(3, 228);
            this.propertyGrid4.Name = "propertyGrid4";
            this.propertyGrid4.Size = new System.Drawing.Size(307, 338);
            this.propertyGrid4.TabIndex = 49;
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
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.添加ToolStripMenuItem.Text = "添加";
            this.添加ToolStripMenuItem.Click += new System.EventHandler(this.添加ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButton1,
            this.toolStripButton1,
            this.tsButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1058, 25);
            this.toolStrip1.TabIndex = 11;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsButton1
            // 
            this.tsButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton1.IsCher = false;
            this.tsButton1.Name = "tsButton1";
            this.tsButton1.Size = new System.Drawing.Size(72, 22);
            this.tsButton1.Text = "初始化板卡";
            this.tsButton1.Click += new System.EventHandler(this.tsButton1_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::Vision2.Properties.Resources.update_vector;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton1.Text = "刷新";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // tsButton2
            // 
            this.tsButton2.Image = ((System.Drawing.Image)(resources.GetObject("tsButton2.Image")));
            this.tsButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButton2.IsCher = false;
            this.tsButton2.Name = "tsButton2";
            this.tsButton2.Size = new System.Drawing.Size(52, 22);
            this.tsButton2.Text = "停止";
            this.tsButton2.Click += new System.EventHandler(this.tsButton2_Click);
            // 
            // dataGridViewComboBoxColumn2
            // 
            this.dataGridViewComboBoxColumn2.HeaderText = "名称";
            this.dataGridViewComboBoxColumn2.Name = "dataGridViewComboBoxColumn2";
            this.dataGridViewComboBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "轴定义";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "轴编号";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // listBox6
            // 
            this.listBox6.ContextMenuStrip = this.contextMenuStrip5;
            this.listBox6.FormattingEnabled = true;
            this.listBox6.ItemHeight = 12;
            this.listBox6.Location = new System.Drawing.Point(0, 2);
            this.listBox6.Name = "listBox6";
            this.listBox6.Size = new System.Drawing.Size(101, 220);
            this.listBox6.TabIndex = 17;
            this.listBox6.SelectedIndexChanged += new System.EventHandler(this.listBox6_SelectedIndexChanged);
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(165, 7);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(99, 21);
            this.numericUpDown4.TabIndex = 6;
            this.numericUpDown4.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(165, 34);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(99, 21);
            this.numericUpDown3.TabIndex = 7;
            this.numericUpDown3.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(111, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(48, 16);
            this.label16.TabIndex = 10;
            this.label16.Text = "宽度X";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(111, 36);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(48, 16);
            this.label17.TabIndex = 11;
            this.label17.Text = "高度Y";
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(310, 3);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(737, 563);
            this.hWindowControl1.TabIndex = 12;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(737, 563);
            // 
            // Form1C154DIDO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1058, 620);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1C154DIDO";
            this.Text = "Form1C154DIDO";
            this.Load += new System.EventHandler(this.Form1C154DIDO_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.contextMenuStrip3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.contextMenuStrip4.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.contextMenuStrip5.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}


		// Token: 0x040008AC RID: 2220
		private TabControl tabControl1;

		// Token: 0x040008AD RID: 2221
		private TabPage tabPage1;

		// Token: 0x040008AE RID: 2222
		private TabPage tabPage2;

		// Token: 0x040008AF RID: 2223
		private ListBox listBox1;

		// Token: 0x040008B0 RID: 2224
		private DataGridView dataGridView1;

		// Token: 0x040008B1 RID: 2225
		private TextBox textBox1;

		// Token: 0x040008B2 RID: 2226
		private Label label2;

		// Token: 0x040008B3 RID: 2227
		private Label label1;

		// Token: 0x040008B4 RID: 2228
		private ListBox listBox2;

		// Token: 0x040008B5 RID: 2229
		private GroupBox groupBox1;

		// Token: 0x040008B6 RID: 2230
		private AxisControl axisControl1;

		// Token: 0x040008B7 RID: 2231
		private ComboBox comboBox1;

		// Token: 0x040008B8 RID: 2232
		private ContextMenuStrip contextMenuStrip1;

		// Token: 0x040008BD RID: 2237
		private ContextMenuStrip contextMenuStrip2;

		// Token: 0x040008BE RID: 2238
		private PropertyGrid propertyGrid1;

		// Token: 0x040008BF RID: 2239
		private ToolStripMenuItem 添加ToolStripMenuItem;

		// Token: 0x040008C0 RID: 2240
		private ToolStripMenuItem 删除ToolStripMenuItem;

		// Token: 0x040008C1 RID: 2241
		private ToolStrip toolStrip1;

		// Token: 0x040008C2 RID: 2242
		private TSButton tsButton1;

		// Token: 0x040008C5 RID: 2245
		private TabControl tabControl2;

		// Token: 0x040008C6 RID: 2246
		private TabPage tabPage3;

		// Token: 0x040008C7 RID: 2247
		private Label label9;

		// Token: 0x040008C8 RID: 2248
		private Label label8;

		// Token: 0x040008C9 RID: 2249
		private Label label7;

		// Token: 0x040008CA RID: 2250
		private TabPage tabPage4;

		// Token: 0x040008CB RID: 2251
		private Button button1;

		// Token: 0x040008CC RID: 2252
		private ToolStripMenuItem 添加轴ToolStripMenuItem;

		// Token: 0x040008CD RID: 2253
		private ToolStripMenuItem 删除ToolStripMenuItem1;

		// Token: 0x040008CE RID: 2254
		private ContextMenuStrip contextMenuStrip3;

		// Token: 0x040008CF RID: 2255
		private ToolStripMenuItem 删除ToolStripMenuItem2;

		// Token: 0x040008D0 RID: 2256
		private DataGridViewComboBoxColumn Column1;

		// Token: 0x040008D1 RID: 2257
		private DataGridViewTextBoxColumn Column3;

		// Token: 0x040008D2 RID: 2258
		private DataGridViewTextBoxColumn Column2;
		private DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private ToolStripButton toolStripButton1;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private ListBox listBox3;
        private TabPage tabPage5;
        private ListBox listBox4;
        private ErosSocket.DebugPLC.DIDO.DIDOUserControl didoUserControl1;
        private PropertyGrid propertyGrid2;
        private ContextMenuStrip contextMenuStrip4;
        private ToolStripMenuItem 添加气缸ToolStripMenuItem;
        private ToolStripMenuItem 删除ToolStripMenuItem3;
        private CylinderControl cylinderControl1;
        private TabPage tabPage6;
        private ErosSocket.DebugPLC.Robot.TrayControl trayControl1;
        private PropertyGrid propertyGrid3;
        private ListBox listBox5;
        private TSButton tsButton2;
        private Label label6;
        private Label label11;
        private Label label12;
        private Label label3;
        private Label label4;
        private Label label5;
        private TabPage tabPage7;
        private PropertyGrid propertyGrid4;
        private ContextMenuStrip contextMenuStrip5;
        private ToolStripMenuItem 添加点ToolStripMenuItem;
        private ToolStripMenuItem 删除矩阵ToolStripMenuItem;
        private ListBox listBox6;
        private HalconDotNet.HWindowControl hWindowControl1;
        private Label label17;
        private Label label16;
        private NumericUpDown numericUpDown3;
        private NumericUpDown numericUpDown4;
    }
}