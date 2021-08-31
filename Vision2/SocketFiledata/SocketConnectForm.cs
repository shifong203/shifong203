using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace ErosSocket
{
    public partial class SocketConnectForm : Form
    {
        public SocketConnectForm()
        {
            InitializeComponent();
            foreach (var item in StaticCon.SocketClint.Keys)
            {
                comBoxRunID.Items.Add(item);
            }
            NetType.Items.Clear();
            List<string> list = SocketClint.GetListClassName();
            foreach (var item in list)
            {
                NetType.Items.Add(item);
            }

            Control.CheckForIllegalCrossThreadCalls = false;
            cmbEncoding.SelectedIndex = 0;
            loadDate();
            Thread thread = new Thread(() => { runData(); });
            thread.IsBackground = true;
            thread.Start();
        }

        public List<SocketClint> FindChild(List<SocketClint> list, int parent = 0)
        {
            List<SocketClint> pageValues = new List<SocketClint>();
            if (list.Count() > 0)
            {
                foreach (var item in list.ToList())
                {
                    pageValues.Add(item);
                    pageValues.AddRange(FindChild(list, 0));
                }
            }
            return pageValues;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows != null)
                {
                    if (cmbEncoding.SelectedItem.ToString() == "UTF8")
                    {
                        StaticCon.SocketClint[dataGridView1.SelectedCells[1].Value.ToString()].Send(Encoding.UTF8.GetBytes(txtSend.Text));
                    }
                    else if (cmbEncoding.SelectedItem.ToString() == "UTF7")
                    {
                        StaticCon.SocketClint[dataGridView1.SelectedCells[1].Value.ToString()].Send(Encoding.UTF7.GetBytes(txtSend.Text));
                    }
                    else if (cmbEncoding.SelectedItem.ToString() == "ASCII")
                    {
                        StaticCon.SocketClint[dataGridView1.SelectedCells[1].Value.ToString()].Send(Encoding.ASCII.GetBytes(txtSend.Text));
                    }
                    else if (cmbEncoding.SelectedItem.ToString() == "Hex")
                    {
                        //int head = Convert.ToInt32(txtSend.Text.Substring(0, 2), 16);
                        //for (int i = 1; i < txtSend.Text.Length / 2; i++)
                        //{
                        //    head = head + Convert.ToInt32(txtSend.Text.Substring(i * 2, 2), 16);
                        //}
                        // Convert.ToString(head, 16).ToString().PadLeft(4, '0').Substring(4, 4);
                        //StaticCon.SocketClint[dataGridView1.SelectedCells[1].Value.ToString()].Send(Encoding.ASCII.GetBytes(txtSend.Text));
                    }
                }
            }
            catch (Exception et)
            {
                MessageBox.Show(et.ToString());
            }
        }

        private void SockeServer_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 周期刷新
        /// </summary>
        private void runData()
        {
            while (!this.IsDisposed)
            {
                try
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1["LinkSta", i] == null || dataGridView1["NameID", i].Value == null)
                        {
                            continue;
                        }
                        if (StaticCon.SocketClint.ContainsKey(dataGridView1["NameID", i].Value.ToString()))
                        {
                            dataGridView1["LinkSta", i].Value = StaticCon.SocketClint[dataGridView1["NameID", i].Value.ToString()].LinkState;
                            if (dataGridView1["NameID", i].Value.ToString() == StaticCon.DebugID)
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        private string loadDate()
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("nameID", Type.GetType("System.String"));
                dataTable.Columns.Add("ValueName", Type.GetType("System.String"));
                dataTable.Columns.Add("outIP", Type.GetType("System.String"));
                dataTable.Columns.Add("outPort", Type.GetType("System.String"));
                dataTable.Columns.Add("Event", Type.GetType("System.String"));
                dataTable.Columns.Add("NetType", Type.GetType("System.String"));
                dataTable.Columns.Add("Default", Type.GetType("System.String"));
                foreach (var item in ErosSocket.ErosConLink.StaticCon.SocketClint)
                {
                    DataRow newRow = dataTable.NewRow();
                    TabPage newtabPage = new TabPage();
                    newRow["nameID"] = newtabPage.Text = item.Value.Name;
                    newRow["ValueName"] = item.Value.ValusName;
                    newRow["outIP"] = item.Value.IP;
                    newRow["outPort"] = item.Value.Port;
                    newRow["Event"] = item.Value.Event;
                    newRow["NetType"] = item.Value.NetType;
                    newRow["Default"] = item.Value.FacillttState;

                    //tpgListReadW.TabPages.Add(newtabPage);
                    dataTable.Rows.Add(newRow);
                }
                dataGridView1.DataSource = dataTable;

                Directory.CreateDirectory(Application.StartupPath + "\\ValueS");
                var path = Directory.GetFiles(Application.StartupPath + "\\ValueS")/*/*.Where(t=>t.EndsWith(".xml"))*/;//获取文件下的全部路径，附加多选筛选Where
                string[] names = new string[path.Length];
                for (int i = 0; i < path.Length; i++)
                {
                    names[i] = Path.GetFileNameWithoutExtension(path[i].ToString());
                }
                ValueName.DataSource = names;
                //获得本地地址
                this.Text = "TCPServer》》计算机名称：" + Dns.GetHostName();
                IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var IPadd in ipEntry.AddressList)
                {
                    //判断当前字符串是否为正确IP地址
                    //得到本地IP地址
                    if (SocketClint.IsValidateIPAddress(IPadd.ToString()))
                    {
                        //得到本地IP地址
                        this.Text += ";本地可链接IP:" + IPadd.ToString();
                        TextBoxPingIP.Text = IPadd.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("链接" + ex.Message);
            }
            return "192.168.0.1";
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    StaticCon.ReadSocketXML("XMLSocket.xml");
        //}

        private void btnNewTCP_Click(object sender, EventArgs e)
        {
            NewSocketForm newSocketForm = new NewSocketForm();

            newSocketForm.Show();
        }

        private void toolStripMenuItemSeleDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void SocketConnectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Hide();
            //switch (e.CloseReason)
            //{
            //    //应用程序要求关闭窗口
            //    case CloseReason.ApplicationExitCall:
            //        e.Cancel = false; //不拦截，响应操作
            //        break;
            //    //自身窗口上的关闭按钮
            //    case CloseReason.FormOwnerClosing:
            //        e.Cancel = true;//拦截，不响应操作
            //        break;
            //    //MDI窗体关闭事件
            //    case CloseReason.MdiFormClosing:
            //        e.Cancel = true;//拦截，不响应操作
            //        break;
            //    //不明原因的关闭
            //    case CloseReason.None:
            //        break;
            //    //任务管理器关闭进程
            //    case CloseReason.TaskManagerClosing:
            //        e.Cancel = false;//不拦截，响应操作
            //        break;
            //    //用户通过UI关闭窗口或者通过Alt+F4关闭窗口
            //    case CloseReason.UserClosing:
            //        e.Cancel = true;//拦截，不响应操作
            //        break;
            //    //操作系统准备关机
            //    case CloseReason.WindowsShutDown:
            //        e.Cancel = false;//不拦截，响应操作
            //        break;

            //    default:
            //        break;
            //}

            //if(e.Cancel == false)
            // base.OnFormClosing(e);
        }

        private void tsmiNewModbusTCP_Click(object sender, EventArgs e)
        {
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModbusTcpForm form1 = new ModbusTcpForm();
            form1.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void tsbtnSaveValues_Click(object sender, EventArgs e)
        {
            if (!StaticCon.WriteSocketXML(dataGridView1, ProjectINI.ProjectPathRun + "\\" + DicSocket.Instance.FileName + "\\XMLSocket.xml"))
            {
                MessageBox.Show("主链接:保存失败");
            }
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 链接列表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (dataGridView1.SelectedRows != null)
                    {
                        if (dataGridView1.SelectedCells[1].Value == null)
                        {
                            return;
                        }
                        string data = "";
                        if (!StaticCon.SocketClint.ContainsKey(dataGridView1.SelectedCells[1].Value.ToString()) || StaticCon.SocketClint[dataGridView1.SelectedCells[1].Value.ToString()].ReciveStr == null)
                        {
                            return;
                        }
                        for (int i = 0; i < StaticCon.SocketClint[dataGridView1.SelectedCells[1].Value.ToString()].ReciveStr.Count; i++)
                        {
                            data += StaticCon.SocketClint[dataGridView1.SelectedCells[1].Value.ToString()].ReciveStr[i] + "\r\n";
                        }
                        txtRead.Text = data;
                        //this.txtRead.Focus();//获取焦点
                        this.txtRead.Select(this.txtRead.TextLength, 0);//光标定位到文本最后
                        this.txtRead.ScrollToCaret();//滚动到光标处
                    }
                }
            }
            catch (Exception et)
            {
            }
        }

        private void 打开变量表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows[0].Cells["NameID"].Value != null || dataGridView1.SelectedRows[0].Cells["NameID"].Value.ToString() != "")
                {
                    Values values = new Values(StaticCon.SocketClint[dataGridView1.SelectedRows[0].Cells["NameID"].Value.ToString()]);
                    values.Show();
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnHMI_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void 新建ModbusRTUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Modbus modbus = new Modbus();
            modbus.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        private void 子链接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private SocketServerForm socketServerForm;

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (socketServerForm == null || socketServerForm.IsDisposed)
                {
                    socketServerForm = new SocketServerForm();
                }
                socketServerForm.Show();
            }
            catch (Exception)
            {
            }
        }

        private ErosUI.ErosNewFrom erosNewFrom;

        private void 控制设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (erosNewFrom == null || erosNewFrom.IsDisposed)
            {
                erosNewFrom = new ErosUI.ErosNewFrom();
            }
            erosNewFrom.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                string processName = Process.GetCurrentProcess().ProcessName;
                System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName(processName);
                Process[] prc = Process.GetProcesses();
                foreach (System.Diagnostics.Process p in ps)
                {
                    p.Kill();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            loadDate();
        }

        private void 打开HMIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 打开事件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                toolStripLabel1.ForeColor = Color.Black;

                if (SocketClint.IsPingIP(TextBoxPingIP.Text, out string textR))
                {
                    toolStripLabel1.ForeColor = Color.Green;
                }
                else
                {
                    toolStripLabel1.ForeColor = Color.Red;
                }
                txtRead.AppendText(textR);
            }
            catch
            {
                //Ping失败
                toolStripLabel1.ForeColor = Color.Red;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void 本地网络ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalIPForm localIPForm1 = new LocalIPForm();
            localIPForm1.Show();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
        }

        private void hMIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ErosUI.retxtForm form = new ErosUI.retxtForm();
            form.Show();
        }

        private void 开机管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void dSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Vision2.ErosProjcetDLL.ErosDesignSurface.ErosDesign erosDesign = new Vision2.ErosProjcetDLL.ErosDesignSurface.ErosDesign();
            //erosDesign.Show();
        }

        private void comBoxRunID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                StaticCon.DebugID = comBoxRunID.SelectedItem.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void 曲线模拟ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ErosUI.曲线窗口 Form = new ErosUI.曲线窗口();
            Form.Show();
        }

        private void comBoxRunID_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
        }
    }
}