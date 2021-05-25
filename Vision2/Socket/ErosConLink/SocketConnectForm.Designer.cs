namespace ErosSocket
{
    partial class SocketConnectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SocketConnectForm));
            this.txtSend = new System.Windows.Forms.TextBox();
            this.txtRead = new System.Windows.Forms.TextBox();
            this.butSend = new System.Windows.Forms.Button();
            this.tpgListReadW = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbEncoding = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ValueName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.NetType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemNewTCP = new System.Windows.Forms.ToolStripMenuItem();
            this.新建ModbusRTUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开变量表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开HMIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开事件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSeleDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnSaveValues = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.子链接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.控制设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hMIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.曲线模拟ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            this.本地网络ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开机管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.TextBoxPingIP = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.comBoxRunID = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewComboBoxColumn2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkSta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Event = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Default = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpgListReadW.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSend
            // 
            this.txtSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSend.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtSend.Location = new System.Drawing.Point(2, 242);
            this.txtSend.Margin = new System.Windows.Forms.Padding(2);
            this.txtSend.Multiline = true;
            this.txtSend.Name = "txtSend";
            this.txtSend.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSend.Size = new System.Drawing.Size(1341, 173);
            this.txtSend.TabIndex = 10;
            // 
            // txtRead
            // 
            this.txtRead.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRead.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtRead.Location = new System.Drawing.Point(1, 2);
            this.txtRead.Margin = new System.Windows.Forms.Padding(2);
            this.txtRead.Multiline = true;
            this.txtRead.Name = "txtRead";
            this.txtRead.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRead.Size = new System.Drawing.Size(1344, 199);
            this.txtRead.TabIndex = 9;
            // 
            // butSend
            // 
            this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butSend.Location = new System.Drawing.Point(1273, 200);
            this.butSend.Margin = new System.Windows.Forms.Padding(2);
            this.butSend.Name = "butSend";
            this.butSend.Size = new System.Drawing.Size(64, 38);
            this.butSend.TabIndex = 7;
            this.butSend.Text = "发送";
            this.butSend.UseVisualStyleBackColor = true;
            this.butSend.Click += new System.EventHandler(this.butSend_Click);
            // 
            // tpgListReadW
            // 
            this.tpgListReadW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tpgListReadW.Controls.Add(this.tabPage2);
            this.tpgListReadW.Location = new System.Drawing.Point(5, 259);
            this.tpgListReadW.Margin = new System.Windows.Forms.Padding(2);
            this.tpgListReadW.Name = "tpgListReadW";
            this.tpgListReadW.SelectedIndex = 0;
            this.tpgListReadW.Size = new System.Drawing.Size(1358, 435);
            this.tpgListReadW.TabIndex = 30;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.cmbEncoding);
            this.tabPage2.Controls.Add(this.txtRead);
            this.tabPage2.Controls.Add(this.txtSend);
            this.tabPage2.Controls.Add(this.butSend);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(1350, 409);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "接受";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1094, 213);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "编码格式";
            // 
            // cmbEncoding
            // 
            this.cmbEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEncoding.FormattingEnabled = true;
            this.cmbEncoding.Items.AddRange(new object[] {
            "UTF8",
            "UTF7",
            "ASCII",
            "Hex"});
            this.cmbEncoding.Location = new System.Drawing.Point(1156, 210);
            this.cmbEncoding.Margin = new System.Windows.Forms.Padding(2);
            this.cmbEncoding.Name = "cmbEncoding";
            this.cmbEncoding.Size = new System.Drawing.Size(92, 20);
            this.cmbEncoding.TabIndex = 11;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameID,
            this.outIP,
            this.outPort,
            this.ValueName,
            this.LinkSta,
            this.NetType,
            this.Event,
            this.Default});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(5, 6);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1358, 249);
            this.dataGridView1.TabIndex = 32;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            this.dataGridView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseUp);
            // 
            // ValueName
            // 
            this.ValueName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ValueName.DataPropertyName = "ValueName";
            this.ValueName.HeaderText = "变量表";
            this.ValueName.Name = "ValueName";
            // 
            // NetType
            // 
            this.NetType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.NetType.DataPropertyName = "NetType";
            this.NetType.HeaderText = "链接类型";
            this.NetType.Items.AddRange(new object[] {
            "TCP/IP",
            "modbusTCP",
            "S7200PPI",
            "巨孚9700",
            "海达U700",
            "三菱MC协议"});
            this.NetType.Name = "NetType";
            this.NetType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NetType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.NetType.Width = 150;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem,
            this.ToolStripMenuItemNewTCP,
            this.新建ModbusRTUToolStripMenuItem,
            this.打开变量表ToolStripMenuItem,
            this.打开HMIToolStripMenuItem,
            this.打开事件ToolStripMenuItem,
            this.toolStripMenuItemSeleDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(174, 158);
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.新建ToolStripMenuItem.Text = "新建ModbusTCP";
            this.新建ToolStripMenuItem.Click += new System.EventHandler(this.新建ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItemNewTCP
            // 
            this.ToolStripMenuItemNewTCP.Name = "ToolStripMenuItemNewTCP";
            this.ToolStripMenuItemNewTCP.Size = new System.Drawing.Size(173, 22);
            this.ToolStripMenuItemNewTCP.Text = "新建TCP";
            this.ToolStripMenuItemNewTCP.Click += new System.EventHandler(this.btnNewTCP_Click);
            // 
            // 新建ModbusRTUToolStripMenuItem
            // 
            this.新建ModbusRTUToolStripMenuItem.Name = "新建ModbusRTUToolStripMenuItem";
            this.新建ModbusRTUToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.新建ModbusRTUToolStripMenuItem.Text = "新建ModbusRTU";
            this.新建ModbusRTUToolStripMenuItem.Click += new System.EventHandler(this.新建ModbusRTUToolStripMenuItem_Click);
            // 
            // 打开变量表ToolStripMenuItem
            // 
            this.打开变量表ToolStripMenuItem.Name = "打开变量表ToolStripMenuItem";
            this.打开变量表ToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.打开变量表ToolStripMenuItem.Text = "打开变量表";
            this.打开变量表ToolStripMenuItem.Click += new System.EventHandler(this.打开变量表ToolStripMenuItem_Click);
            // 
            // 打开HMIToolStripMenuItem
            // 
            this.打开HMIToolStripMenuItem.Name = "打开HMIToolStripMenuItem";
            this.打开HMIToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.打开HMIToolStripMenuItem.Text = "打开HMI";
            this.打开HMIToolStripMenuItem.Click += new System.EventHandler(this.打开HMIToolStripMenuItem_Click);
            // 
            // 打开事件ToolStripMenuItem
            // 
            this.打开事件ToolStripMenuItem.Name = "打开事件ToolStripMenuItem";
            this.打开事件ToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.打开事件ToolStripMenuItem.Text = "打开事件管理";
            this.打开事件ToolStripMenuItem.Click += new System.EventHandler(this.打开事件ToolStripMenuItem_Click);
            // 
            // toolStripMenuItemSeleDelete
            // 
            this.toolStripMenuItemSeleDelete.Name = "toolStripMenuItemSeleDelete";
            this.toolStripMenuItemSeleDelete.Size = new System.Drawing.Size(173, 22);
            this.toolStripMenuItemSeleDelete.Text = "删除链接";
            this.toolStripMenuItemSeleDelete.Click += new System.EventHandler(this.toolStripMenuItemSeleDelete_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnSaveValues,
            this.toolStripButton1,
            this.toolStripDropDownButton1,
            this.toolStripButton2,
            this.toolStripSplitButton1,
            this.toolStripButton4,
            this.toolStripSplitButton2,
            this.toolStripLabel1,
            this.TextBoxPingIP,
            this.toolStripLabel2,
            this.comBoxRunID,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1378, 25);
            this.toolStrip1.TabIndex = 49;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnSaveValues
            // 
            this.tsbtnSaveValues.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnSaveValues.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSaveValues.Image")));
            this.tsbtnSaveValues.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSaveValues.Name = "tsbtnSaveValues";
            this.tsbtnSaveValues.Size = new System.Drawing.Size(62, 22);
            this.tsbtnSaveValues.Text = "保存XML";
            this.tsbtnSaveValues.Click += new System.EventHandler(this.tsbtnSaveValues_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton1.Text = " 数据库";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.子链接ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(57, 22);
            this.toolStripDropDownButton1.Text = "连接池";
            // 
            // 子链接ToolStripMenuItem
            // 
            this.子链接ToolStripMenuItem.Name = "子链接ToolStripMenuItem";
            this.子链接ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.子链接ToolStripMenuItem.Text = "子链接";
            this.子链接ToolStripMenuItem.Click += new System.EventHandler(this.子链接ToolStripMenuItem_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(72, 22);
            this.toolStripButton2.Text = "查看服务器";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.控制设备ToolStripMenuItem,
            this.hMIToolStripMenuItem,
            this.dSToolStripMenuItem,
            this.曲线模拟ToolStripMenuItem});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(73, 22);
            this.toolStripSplitButton1.Text = "HMI设计";
            // 
            // 控制设备ToolStripMenuItem
            // 
            this.控制设备ToolStripMenuItem.Name = "控制设备ToolStripMenuItem";
            this.控制设备ToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.控制设备ToolStripMenuItem.Text = "控制设备HMI";
            this.控制设备ToolStripMenuItem.Click += new System.EventHandler(this.控制设备ToolStripMenuItem_Click);
            // 
            // hMIToolStripMenuItem
            // 
            this.hMIToolStripMenuItem.Name = "hMIToolStripMenuItem";
            this.hMIToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.hMIToolStripMenuItem.Text = "文本编译";
            this.hMIToolStripMenuItem.Click += new System.EventHandler(this.hMIToolStripMenuItem_Click);
            // 
            // dSToolStripMenuItem
            // 
            this.dSToolStripMenuItem.Name = "dSToolStripMenuItem";
            this.dSToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.dSToolStripMenuItem.Text = "DS";
            this.dSToolStripMenuItem.Click += new System.EventHandler(this.dSToolStripMenuItem_Click);
            // 
            // 曲线模拟ToolStripMenuItem
            // 
            this.曲线模拟ToolStripMenuItem.Name = "曲线模拟ToolStripMenuItem";
            this.曲线模拟ToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.曲线模拟ToolStripMenuItem.Text = "曲线模拟";
            this.曲线模拟ToolStripMenuItem.Click += new System.EventHandler(this.曲线模拟ToolStripMenuItem_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton4.Text = "刷新";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripSplitButton2
            // 
            this.toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.本地网络ToolStripMenuItem,
            this.开机管理ToolStripMenuItem});
            this.toolStripSplitButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton2.Image")));
            this.toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton2.Name = "toolStripSplitButton2";
            this.toolStripSplitButton2.Size = new System.Drawing.Size(72, 22);
            this.toolStripSplitButton2.Text = "网络工具";
            // 
            // 本地网络ToolStripMenuItem
            // 
            this.本地网络ToolStripMenuItem.Name = "本地网络ToolStripMenuItem";
            this.本地网络ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.本地网络ToolStripMenuItem.Text = "本地网络";
            this.本地网络ToolStripMenuItem.Click += new System.EventHandler(this.本地网络ToolStripMenuItem_Click);
            // 
            // 开机管理ToolStripMenuItem
            // 
            this.开机管理ToolStripMenuItem.Name = "开机管理ToolStripMenuItem";
            this.开机管理ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.开机管理ToolStripMenuItem.Text = "开机管理";
            this.开机管理ToolStripMenuItem.Click += new System.EventHandler(this.开机管理ToolStripMenuItem_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(57, 22);
            this.toolStripLabel1.Text = "Ping网络";
            this.toolStripLabel1.Click += new System.EventHandler(this.toolStripLabel1_Click);
            // 
            // TextBoxPingIP
            // 
            this.TextBoxPingIP.Name = "TextBoxPingIP";
            this.TextBoxPingIP.Size = new System.Drawing.Size(100, 25);
            this.TextBoxPingIP.Text = "192.168.0.1";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(50, 22);
            this.toolStripLabel2.Text = "Debug:";
            // 
            // comBoxRunID
            // 
            this.comBoxRunID.Name = "comBoxRunID";
            this.comBoxRunID.Size = new System.Drawing.Size(121, 25);
            this.comBoxRunID.SelectedIndexChanged += new System.EventHandler(this.comBoxRunID_SelectedIndexChanged);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(96, 22);
            this.toolStripButton3.Text = "关闭并退出服务";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1378, 725);
            this.tabControl1.TabIndex = 51;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.tpgListReadW);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1370, 699);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "客户端连接池";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1370, 699);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "服务端";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewComboBoxColumn2});
            this.dataGridView2.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView2.Location = new System.Drawing.Point(3, 2);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowTemplate.Height = 27;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(1358, 248);
            this.dataGridView2.TabIndex = 33;
            // 
            // dataGridViewComboBoxColumn2
            // 
            this.dataGridViewComboBoxColumn2.DataPropertyName = "NetType";
            this.dataGridViewComboBoxColumn2.HeaderText = "服务类型";
            this.dataGridViewComboBoxColumn2.Items.AddRange(new object[] {
            "TCP/IP",
            "modbusTCP",
            "S7200PPI",
            "巨孚9700",
            "海达U700",
            "三菱MC协议"});
            this.dataGridViewComboBoxColumn2.Name = "dataGridViewComboBoxColumn2";
            this.dataGridViewComboBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "nameID";
            this.dataGridViewTextBoxColumn1.FillWeight = 80F;
            this.dataGridViewTextBoxColumn1.HeaderText = "链接名";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "outIP";
            this.dataGridViewTextBoxColumn2.FillWeight = 80F;
            this.dataGridViewTextBoxColumn2.HeaderText = "监听IP";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "outPort";
            this.dataGridViewTextBoxColumn3.FillWeight = 60F;
            this.dataGridViewTextBoxColumn3.HeaderText = "监听端口";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Event";
            this.dataGridViewTextBoxColumn4.FillWeight = 60F;
            this.dataGridViewTextBoxColumn4.HeaderText = "事件信息";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 80;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "LinkSta";
            this.dataGridViewTextBoxColumn5.FillWeight = 60F;
            this.dataGridViewTextBoxColumn5.HeaderText = "链接状态";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 200;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "nameID";
            this.dataGridViewTextBoxColumn6.FillWeight = 80F;
            this.dataGridViewTextBoxColumn6.HeaderText = "链接名";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn6.Width = 226;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "outIP";
            this.dataGridViewTextBoxColumn7.FillWeight = 80F;
            this.dataGridViewTextBoxColumn7.HeaderText = "监听IP";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn7.Width = 226;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "outPort";
            this.dataGridViewTextBoxColumn8.FillWeight = 60F;
            this.dataGridViewTextBoxColumn8.HeaderText = "监听端口";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 169;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Event";
            this.dataGridViewTextBoxColumn9.FillWeight = 60F;
            this.dataGridViewTextBoxColumn9.HeaderText = "事件信息";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Width = 282;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "LinkSta";
            this.dataGridViewTextBoxColumn10.FillWeight = 60F;
            this.dataGridViewTextBoxColumn10.HeaderText = "链接状态";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Width = 170;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "LinkSta";
            this.dataGridViewTextBoxColumn11.FillWeight = 60F;
            this.dataGridViewTextBoxColumn11.HeaderText = "链接状态";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Width = 170;
            // 
            // NameID
            // 
            this.NameID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.NameID.DataPropertyName = "nameID";
            this.NameID.FillWeight = 80F;
            this.NameID.HeaderText = "链接名";
            this.NameID.Name = "NameID";
            this.NameID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NameID.Width = 150;
            // 
            // outIP
            // 
            this.outIP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.outIP.DataPropertyName = "outIP";
            this.outIP.FillWeight = 80F;
            this.outIP.HeaderText = "目标IP";
            this.outIP.Name = "outIP";
            // 
            // outPort
            // 
            this.outPort.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.outPort.DataPropertyName = "outPort";
            this.outPort.FillWeight = 60F;
            this.outPort.HeaderText = "目标端口";
            this.outPort.Name = "outPort";
            this.outPort.Width = 80;
            // 
            // LinkSta
            // 
            this.LinkSta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LinkSta.DataPropertyName = "LinkSta";
            this.LinkSta.FillWeight = 60F;
            this.LinkSta.HeaderText = "链接状态";
            this.LinkSta.Name = "LinkSta";
            this.LinkSta.Width = 80;
            // 
            // Event
            // 
            this.Event.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Event.DataPropertyName = "Event";
            this.Event.HeaderText = "事件信息";
            this.Event.Name = "Event";
            this.Event.Width = 200;
            // 
            // Default
            // 
            this.Default.DataPropertyName = "Default";
            this.Default.HeaderText = "备注";
            this.Default.Name = "Default";
            // 
            // SocketConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1378, 750);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SocketConnectForm";
            this.Text = "TCPServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SocketConnectForm_FormClosing);
            this.Load += new System.EventHandler(this.SockeServer_Load);
            this.tpgListReadW.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.TextBox txtRead;
        private System.Windows.Forms.Button butSend;
        private System.Windows.Forms.TabControl tpgListReadW;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemNewTCP;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSeleDelete;
        private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnSaveValues;
        private System.Windows.Forms.ToolStripMenuItem 打开变量表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开事件ToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbEncoding;
        private System.Windows.Forms.ToolStripMenuItem 新建ModbusRTUToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem 子链接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem 控制设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripMenuItem 打开HMIToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox TextBoxPingIP;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton2;
        private System.Windows.Forms.ToolStripMenuItem 本地网络ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hMIToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.ToolStripMenuItem 开机管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dSToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox comBoxRunID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameID;
        private System.Windows.Forms.DataGridViewTextBoxColumn outIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn outPort;
        private System.Windows.Forms.DataGridViewComboBoxColumn ValueName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkSta;
        private System.Windows.Forms.DataGridViewComboBoxColumn NetType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Event;
        private System.Windows.Forms.DataGridViewTextBoxColumn Default;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripMenuItem 曲线模拟ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
    }
}