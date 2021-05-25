using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class AlarmListBoxt : Form
    {
        public AlarmListBoxt()
        {
            InitializeComponent();
            //this.TopLevel = false;

            this.Show();
            alarmList = this;
        }
        static AlarmListBoxt alarmList = new AlarmListBoxt();

        public static AlarmListBoxt AlarmFormThis
        {
            get
            {
                if (alarmList == null || alarmList.IsDisposed)
                {
                    alarmList = new AlarmListBoxt();
                    alarmList.BringToFront();
                    alarmList.TopMost = true;
                }

                return alarmList;
            }
            set { alarmList = value; }
        }

        private void AlramListForm1_Load(object sender, EventArgs e)
        {
            checkBox2.Checked = Vision2.Project.DebugF.DebugCompiler.FmqIS;
        }

        private void AlramListForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;//拦截，不响应操作
            return;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Vision2.Project.DebugF.DebugCompiler.FmqIS = checkBox2.Checked;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 添加报警到列表
        /// </summary>
        /// <param name="text">报警信息</param>
        /// <param name="ite">位置数</param>
        public static void AddAlarmText(AlarmText.alarmStruct text, int ite = 0)
        {
            try
            {
                if (Vision2.Project.MainForm1.MainFormF.InvokeRequired)
                {
                    Vision2.Project.MainForm1.MainFormF.Invoke(new Action<AlarmText.alarmStruct, int>(AddAlarmText), text, ite);
                }
                if (!AlarmListBoxt.AlarmFormThis.Visible)
                {
                    AlarmListBoxt.AlarmFormThis.Show();
                }
                //AlarmListBoxt.AlarmFormThis.Show();
                if (AlarmText.DicAlarm.ContainsKey(text.Name))
                {
                    return;
                }

                if (!AlarmText.DicAlarm.ContainsKey(text.Name))
                {
                    AlarmText.DicAlarm.Add(text.Name, text);
                    Excel.Npoi.AddText(Vision2.ErosProjcetDLL.Project.ProjectINI.TempPath + "\\报警记录\\" + DateTime.Now.ToLongDateString() + ".CSV", new string[] { DateTime.Now.ToString(), text.ToString() });

                }
                if (ite == 0)
                {
                    ite = AlarmText.DicAlarm.Count();
                }
                bool dt = false;
                string date = "";
                alarmList.label1.Text = "报警数：" + AlarmText.DicAlarm.Count();
                int ste = alarmList.richTextBox1.Text.Length;
                for (int i = 0; i < alarmList.richTextBox1.Lines.Length; i++)
                {
                    string[] data = alarmList.richTextBox1.Lines[i].Split('{');
                    if (data.Length > 5 && data[4] == text.Name)
                    {
                        dt = true;
                        ste += alarmList.richTextBox1.Lines[i].Length;
                        date = alarmList.richTextBox1.Lines[i] = "【" + ite + "】" + text.Time + "{" + text.AlaType + "{" + text.Name + "{" + text.Text + Environment.NewLine;
                        break;
                    }
                }
                if (!dt)
                {
                    date = "【" + ite + "】" + text.Time + "{" + text.AlaType + "{" + text.Name + "{" + text.Text + Environment.NewLine;
                    alarmList.richTextBox1.AppendText(date);
                }
                alarmList.richTextBox1.Select(ste, date.Length);
                if (ite % 2 == 1)
                {
                    alarmList.richTextBox1.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
                }
                else
                {
                    alarmList.richTextBox1.SelectionBackColor = System.Drawing.SystemColors.ActiveBorder;
                }
                if (text.AlaType == "一般报警")
                {
                    alarmList.richTextBox1.SelectionColor = Color.Green;
                }
                else if (text.AlaType == "提示信息")
                {
                    alarmList.richTextBox1.SelectionColor = Color.GreenYellow;

                }
                else if (text.AlaType == "致命报警")
                {
                    alarmList.richTextBox1.SelectionColor = Color.Red;
                }
                else
                {
                    alarmList.richTextBox1.SelectionColor = Color.Black;
                }
                alarmList.UpAlarmText();

            }
            catch (Exception ex)
            {
            }

        }


        public static void AddAlarmText(string name, string text = "")
        {
            AddAlarmText(new AlarmText.alarmStruct() { Name = name, Text = text });
        }
        public static void RomveAlarmText()
        {
            try
            {
                if (alarmList != null)
                {
                    if (alarmList.InvokeRequired)
                    {
                        alarmList.Invoke(new Action(RomveAlarmText));
                    }
                }
                alarmList.label1.Text = "报警数:" + AlarmText.DicAlarm.Count();
                if (AlarmText.DicAlarm.Count > 0)
                {
                    alarmList.checkBox2.Checked = Vision2.Project.DebugF.DebugCompiler.FmqIS;
                    alarmList.Show();
                }
                else
                {
                    alarmList.Hide();
                }

            }
            catch (Exception)
            {
            }
        }

        public static void RomveAlarm(string name)
        {
            try
            {
                if (AlarmText.DicAlarm.ContainsKey(name))
                {
                    int ste = 0;
                    for (int i = 0; i < alarmList.richTextBox1.Lines.Length; i++)
                    {
                        string[] data = alarmList.richTextBox1.Lines[i].Split('{');
                        if (data.Length > 2 && data[2] == AlarmText.DicAlarm[name].Name)
                        {

                            alarmList.richTextBox1.Text = alarmList.richTextBox1.Text.Remove(ste, alarmList.richTextBox1.Lines[i].Length + 1);
                            break;
                        }
                        ste += alarmList.richTextBox1.Lines[i].Length;
                    }

                    AlarmText.DicAlarm.Remove(name);
                }

            }
            catch (Exception)
            {

            }
            RomveAlarmText();
        }
        public static void RomveAlarmAll()
        {
            try
            {
                Vision2.Project.DebugF.DebugCompiler.FmqIS = false;
                AlarmText.DicAlarm.Clear();
                alarmList.richTextBox1.Text = "";
            }
            catch (Exception)
            {

            }
            RomveAlarmText();
        }

        /// <summary>
        /// 更新报警信息颜色
        /// </summary>
        public void UpAlarmText()
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action(UpAlarmText));
                }
                alarmList.label1.Text = "报警数:" + AlarmText.DicAlarm.Count;

                char punctuation = '.';
                //改变全部字体颜色
                richTextBox1.ForeColor = Color.Black;
                int ste = 0;
                //     改变关键字颜色
                for (int i = 0; i < richTextBox1.Lines.Length; i++)
                {
                    //改变算子关键字颜色
                    if (richTextBox1.Lines[i].Split(punctuation).Length < 2)
                    {
                        ste = ste + richTextBox1.Lines[i].Length;
                        continue;
                    }
                    string datastr = richTextBox1.Lines[i].Split(punctuation)[1];
                    string datastrt = richTextBox1.Lines[i].Split(punctuation)[2] + ".";
                    datastrt += richTextBox1.Lines[i].Split(punctuation)[3];
                    if (datastrt.Contains(':'))
                    {
                        datastrt = datastrt.Split(':')[0];
                    }
                    if (datastr.Contains(AlarmText.AlarmType.一般报警.ToString()))
                    {
                        richTextBox1.Select(ste + i, richTextBox1.Lines[i].Length);
                        richTextBox1.SelectionColor = Color.Green;
                    }
                    else if (datastr.Contains(AlarmText.AlarmType.致命报警.ToString()))
                    {
                        richTextBox1.SelectionBackColor = Color.LightSkyBlue;
                        richTextBox1.Select(ste + i, richTextBox1.Lines[i].Length);
                        richTextBox1.SelectionColor = Color.Red;
                    }
                    else if (datastr.Contains(AlarmText.AlarmType.提示信息.ToString()))
                    {
                        richTextBox1.Select(ste + i, richTextBox1.Lines[i].Length);
                        richTextBox1.SelectionColor = Color.GreenYellow;
                    }
                    //else if (datastr.Contains(AlarmText.AlarmType.指示信息.ToString()))
                    //{
                    //    richTextBox1.Select(ste + i, richTextBox1.Lines[i].Length);
                    //    richTextBox1.SelectionColor = Color.LawnGreen;
                    //}
                    else if (AlarmText.DicAlarm.ContainsKey(datastrt))
                    {
                        richTextBox1.Select(ste + i, richTextBox1.Lines[i].Length);
                        richTextBox1.SelectionColor = Color.LawnGreen;
                    }
                    else
                    {
                        richTextBox1.SelectionColor = Color.LawnGreen;
                    }
                    ste = ste + richTextBox1.Lines[i].Length;
                }
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                RomveAlarmAll();
            }
            catch (Exception)
            {
            }
        }

        private void AlarmListBoxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10 || e.KeyCode == Keys.Space)
            {
                try
                {
                    RomveAlarmAll();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
