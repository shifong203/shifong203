using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vision2.Project;
using Vision2.Project.formula;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class AlarmText : System.Windows.Forms.UserControl
    {
        public AlarmText()
        {
            InitializeComponent();
            Program = this;
            richTextBox1.Text = "";
        }

        private static AlarmText Program;

        public static AlarmText ThisF
        {
            get
            {
                if (Program == null)
                {
                    Program = new AlarmText();
                }
                return Program;
            }

            set
            {
                Program = value;
            }
        }


        //[PropertyTabAttribute(typeof(TypeCategoryTab), PropertyTabScope.Component)]
        /// <summary>
        /// 报警类
        /// </summary>
        public class Alarm : Component
        {
            [DescriptionAttribute("触发器。")]
            public class Trigger
            {
                [Description("触发报警的最小值"), Category("限制"), DisplayName("触发值")]
                public double TriggerValue { get; set; } = 0;

                [Description("运算符号"), Category("限制"), DisplayName("运算符"), TypeConverter(typeof(TriggerType))]
                public string Operati { get; set; } = "=";

                [Description("单独的触发文本"), Category("显示"), DisplayName("触发文本")]
                public string TriggerText { get; set; } = "";
            }

            public class TriggerType : StringConverter
            {
                public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
                {
                    return true;
                }

                public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
                {
                    return new StandardValuesCollection(new string[] { ">", ">=", "<", "<=", "=", "!=" });
                }
            }

            [Description("触发报警的最小值"), Category("限制"), DisplayName("触发值")]
            public List<Trigger> Triggers
            {
                get;
                set;
            } = new List<Trigger>();

            [Description("是否启用"), Category("限制"), DisplayName("启用")]
            public bool Enabled { get; set; }

            [Description("是否自复位"), Category("限制"), DisplayName("自复位")]
            public bool IsReset { get; set; }

            [Description("是否触发报警"), Category("限制"), DisplayName("报警信号"), ReadOnly(true)]
            public bool IsAlarm { get; set; }

            [Description("触发报警显示的文本，[]为指针显示报警文本"), Category("显示"), DisplayName("报警文本")]
            public string Text { get; set; } = "报警文本！";

            [Description("触发报警显示的类型，基本类型或自定义类型"), Category("显示"), DisplayName("报警类别"), TypeConverter(typeof(AlarmTypeConverter)),]
            public string AlarmType { get; set; } = "";

            private double valuet;
            private bool alarm = false;
    

            /// <summary>
            /// 检测报警触发器
            /// </summary>
            /// <param name="value">当前值</param>
            /// <param name="text">报警文本</param>
            /// <param name="name">触发对象名称</param>
            public void UPAlarm(double value, string text, string name)
            {
                if (this.Enabled)
                {
                    if (value != valuet)
                    {
                        alarm = false;
                        valuet = value;
                        if (this.IsReset)
                        {
                            ResetAlarm();
                        }
                        for (int i = 0; i < Triggers.Count; i++)
                        {
                            switch (Triggers[i].Operati)
                            {
                                case ">":

                                    if (value > Triggers[i].TriggerValue)
                                    {
                                        if (!alarm)
                                        {
                                            alarm = true;
                                            if (Triggers[i].TriggerText != "")
                                            {
                                                SetAlarm(name, Triggers[i].TriggerText);
                                            }
                                            else
                                            {
                                                SetAlarm(name, text);
                                            }
                                        }
                                    }
                                    break;

                                case ">=":
                                    if (value >= Triggers[i].TriggerValue)
                                    {
                                        if (!alarm)
                                        {
                                            alarm = true;
                                            if (Triggers[i].TriggerText != "")
                                            {
                                                SetAlarm(name, Triggers[i].TriggerText);
                                            }
                                            else
                                            {
                                                SetAlarm(name, text);
                                            }
                                        }
                                    }
                                    break;

                                case "<":
                                    if (value < Triggers[i].TriggerValue)
                                    {
                                        if (!alarm)
                                        {
                                            alarm = true;
                                            if (Triggers[i].TriggerText != "")
                                            {
                                                SetAlarm(name, Triggers[i].TriggerText);
                                            }
                                            else
                                            {
                                                SetAlarm(name, text);
                                            }
                                        }
                                    }
                                    break;

                                case "<=":
                                    if (value <= Triggers[i].TriggerValue)
                                    {
                                        if (!alarm)
                                        {
                                            alarm = true;
                                            if (Triggers[i].TriggerText != "")
                                            {
                                                SetAlarm(name, Triggers[i].TriggerText);
                                            }
                                            else
                                            {
                                                SetAlarm(name, text);
                                            }
                                        }
                                    }
                                    break;

                                case "=":
                                    if (value == Triggers[i].TriggerValue)
                                    {
                                        if (!alarm)
                                        {
                                            alarm = true;
                                            if (Triggers[i].TriggerText != "")
                                            {
                                                SetAlarm(name, Triggers[i].TriggerText);
                                            }
                                            else
                                            {
                                                SetAlarm(name, text);
                                            }
                                        }
                                    }
                                    break;

                                case "!=":
                                    if (value != Triggers[i].TriggerValue)
                                    {
                                        if (!alarm)
                                        {
                                            alarm = true;
                                            if (Triggers[i].TriggerText != "")
                                            {
                                                SetAlarm(name, Triggers[i].TriggerText);
                                            }
                                            else
                                            {
                                                SetAlarm(name, text);
                                            }
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                        if (PLCUI.HMIDIC.This.DicPLCEnum.ContainsKey(text))
                        {
                            if (PLCUI.HMIDIC.This.DicPLCEnum[text].kaayValue.ContainsKey((int)value))
                            {
                                SetAlarm(name, PLCUI.HMIDIC.This.DicPLCEnum[text].kaayValue[(int)value]);
                            }
                            else
                            {
                                if ((int)value != 0)
                                {
                                    SetAlarm(name, "未定义枚举");
                                }
                            }
                        }
                    }
                    else
                    {
                        //alarm = true;
                    }
                }
            }

            /// <summary>
            /// 触发报警
            /// </summary>
            /// <param name="name">触发名称</param>
            /// <param name="text">文本</param>
            public void SetAlarm(string name, string text)
            {
                AlarmListBoxt.AddAlarmText(new alarmStruct() {Name=name,Text=text,AlaType=AlarmType });

                this.IsAlarm = true;
                _name = name;
           
            }

            private string _name = "";

            public void ResetAlarm()
            {
                this.IsAlarm = false;
                try
                {
                    //Stub.StubManager.getEAP().ClearAlarm(_name);
                }
                catch (Exception)
                {
                }
                Reset(_name);
            }

            private class AlarmTypeConverter : StringConverter
            {
                public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
                {
                    return true;
                }

                public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
                {
                    return new StandardValuesCollection(StaticConFile.StaticCon.EnumToDictionary<AlarmType>().Values.ToArray());
                }

                public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 报警信息
        /// </summary>
        public static Dictionary<string, alarmStruct> DicAlarm { get; private set; } = new Dictionary<string, alarmStruct>();

        private static List<TextColor> lisetText = new List<TextColor>();

        public class TextColor
        {
            public string Text { get; set; }

            public Color Color { get; set; }
        }

        public class alarmStruct
        {
            public alarmStruct()
            {
                Time = DateTime.Now.ToString("MM/dd HH:mm:ss");
                Text = "";
                Name = "";
                AlaType = "一般报警";
            }

            /// <summary>
            /// 触发时间
            /// </summary>
            public string Time;

            /// <summary>
            /// 报警信息
            /// </summary>
            public string Text;

            /// <summary>
            /// 名称
            /// </summary>
            public string Name;

            /// <summary>
            /// 类型
            /// </summary>
            public string AlaType;

            public override string ToString()
            {
                //return this.Name + ":" + this.AlaType + ":" + this.Text + ":" + this.Time;
                return this.Time + "{" + this.AlaType + "{" + this.Name + "{" + this.Text;
            }
        }



        public static void AddText(string text)
        {
            AddText(text, Color.Black);
        }

        public static void AddText(string text, Color color)
        {
          
            if (ThisF.richTextBox1.InvokeRequired)
            {
                ThisF.richTextBox1.Invoke(new Action<string, Color>(AddText), text, color);
            }
            else
            {
                text = DateTime.Now.ToString("T") + ":" + text;
                Excel.Npoi.AddText(ProjectINI.TempPath + "\\文本记录\\" +
                    DateTime.Now.ToString("yyyy年M月d日") + ".CSV", text);
                lisetText.Add(new TextColor() { Text = text, Color = color });
                try
                {
                    Color col = ThisF.richTextBox1.SelectionColor;
                    ThisF.richTextBox1.AppendText(text);
                    int strati = ThisF.richTextBox1.Text.Length - text.Length;
                    if (strati < 0)
                    {
                        strati = 0;
                    }
                    ThisF.richTextBox1.Select(strati, text.Length);
                    ThisF.richTextBox1.SelectionColor = color;
                    ThisF.richTextBox1.SelectionStart = ThisF.richTextBox1.Text.Length;
                    ThisF.richTextBox1.SelectionColor = col;
                }
                catch (Exception)
                {
                }
            }
        }

        public static void AddTextNewLine(string text, Color color)
        {
            AddText(text + Environment.NewLine, color);
        }

        public static void AddTextNewLine(string text)
        {
            AddText(text + Environment.NewLine);
        }

        

        public static void LogErr(string text, string name)
        {
             AddTextNewLine(name + ":" + text, Color.Red);
        }

   

        public static void LogWarning(string name, string text)
        {
            AddTextNewLine(name + ":" + text, Color.Yellow);
        }

        /// <summary>
        /// 登记事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public static void LogIncident(string name, string message)
        {
            try
            {
                AlarmText.AddTextNewLine("登录事件:" + name + ":" + message);
                if (!File.Exists(ProjectINI.TempPath+ @"\Log\LogIncident.txt"))
                {
                    File.AppendAllText(ProjectINI.TempPath + @"\Log\LogIncident.txt", string.Format("事件信息登记!版本1.00,作者：Eros;更新时间：2018年7月19日14:11;事件分隔符“>+换行符”,结构分隔符“|”>" + Environment.NewLine));//在文本后追加
                }
                File.AppendAllText(ProjectINI.TempPath + @"\Log\LogIncident.txt", string.Format(name + "|" + DateTime.Now + "|" +
                    message + ">" + Environment.NewLine));//在文本后追加
            }
            catch (Exception ex)
            {
                LogWarning("登记事件错误：", ex.Message);
            }
        }

        /// <summary>
        /// 复位报警信息，
        /// </summary>
        /// <param name="text"></param>
        public static void Reset(string text)
        {
            if (text == "")
            {
                return;
            }
            if (text.StartsWith("."))
            {
                text = text.Substring(1);
            }

            if (DicAlarm.ContainsKey(text))
            {
                string D = DicAlarm[text].Time + "." + DicAlarm[text].AlaType + "." + DicAlarm[text].Name + ":";
                int DST = ThisF.richTextBox1.Text.IndexOf(D);
                if (DST < 0)
                {
                    return;
                }
                for (int i = 0; i < ThisF.richTextBox1.Lines.Length; i++)
                {
                    List<string> DS = ThisF.richTextBox1.Lines.ToList();
                    if (D + DicAlarm[text].Text == DS[i].TrimStart(' '))
                    {
                        DS.RemoveAt(i);
                        ThisF.richTextBox1.Lines = DS.ToArray();
                        break;
                    }
                }
                DicAlarm.Remove(text);
            }
            else
            {
                return;
            }
            AlarmListBoxt.RomveAlarmText();
        }

        /// <summary>
        /// 文本信息类别
        /// </summary>
        public enum AlarmType
        {
            /// <summary>
            /// 提示信息
            /// </summary>
            信息 = 0,

            /// <summary>
            /// 指示信息
            /// </summary>
            提示信息 = 1,

            /// <summary>
            /// 报警信息
            /// </summary>
            一般报警 = 2,

            /// <summary>
            /// 危险报警
            /// </summary>
            致命报警 = 3,
        }

        private void 打开记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(ProjectINI.TempPath + "\\报警记录");
                System.Diagnostics.Process.Start(ProjectINI.TempPath + "\\报警记录");
            }
            catch (Exception)
            {
            }
        }

        private void 添加危险报警ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlarmListBoxt.AddAlarmText("激光.危险故障", AlarmType.致命报警.ToString(), "请联系专业人员处理");
        }

        private void 添加一般故障ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlarmListBoxt.AddAlarmText("点胶", AlarmType.一般报警.ToString(), "请排除故障");
        }

        private void 添加提示信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlarmListBoxt.AddAlarmText("已暂停", AlarmType.信息.ToString(), "已暂停");
        }

        private void 添加指示信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlarmListBoxt.AddAlarmText("初始化位", AlarmType.信息.ToString(), "请初始化");
        }

        private void dsAlphaRichTextBox1_TextChanged(object sender, EventArgs e)
        {
        
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => {
                    if (ThisF.richTextBox1.Lines.Length > ProjectINI.In.MaxText)
                    {
                        ThisF.richTextBox1.Lines = ThisF.richTextBox1.Lines.Skip(20).Take(ThisF.richTextBox1.Lines.Length - 20).ToArray();
                    }
                    ThisF.richTextBox1.SelectionStart = ThisF.richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                }));
            }
            else
            {
                if (ThisF.richTextBox1.Lines.Length > ProjectINI.In.MaxText)
                {
                    ThisF.richTextBox1.Lines = ThisF.richTextBox1.Lines.Skip(20).Take(ThisF.richTextBox1.Lines.Length - 20).ToArray();
                }
                ThisF.richTextBox1.SelectionStart = ThisF.richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

        }

  


        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //HideCaret(((RichTextBox)sender).Handle);
        }

        public static void ResetAlarm()
        {
            AlarmListBoxt.RomveAlarmText();
        }

        private void toolStripDropDownButton4_MouseEnter(object sender, EventArgs e)
        {
        }

        private void 全选_Click(object sender, EventArgs e)
        {
            if (!全选.Checked)
            {
                foreach (ToolStripMenuItem item in DropDownSele.DropDownItems)
                {
                    item.Checked = true;
                }
            }
            else
            {
                全选.Checked = false;
            }
            DropDownSele.ShowDropDown();
            //ThisF.UpAlarmText();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Program.richTextBox1.ResetText();
            ThisF.richTextBox1.Text = "  ";
            ResetAlarm();
        }

        private void 查看事件信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogMessageForm logMessageForm = new LogMessageForm(ProjectINI.TempPath + @"\Log\LogIncident.txt");
            logMessageForm.Show();
         
        }

        private void toolStripDropDownButton1_MouseMove(object sender, MouseEventArgs e)
        {
            //弹出报警列表ToolStripMenuItem.Checked = ProjectINI.In.IsShowAlarmText;
        }

        private void 弹出报警列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (ProjectINI.In.IsShowAlarmText)
            //{
            //    ProjectINI.In.IsShowAlarmText = false;
            //}
            //else
            //{
            //    ProjectINI.In.IsShowAlarmText = true;
            //}
            //弹出报警列表ToolStripMenuItem.Checked = ProjectINI.In.IsShowAlarmText;
        }

     
        private void AlarmText_Load(object sender, EventArgs e)
        {
            //tooSButtonAlarmListNumber.Text = "报警列表" + DicAlarm.Count();
            try
            {
                Color col = richTextBox1.SelectionColor;
                for (int i = 0; i < lisetText.Count; i++)
                {
                    richTextBox1.AppendText(lisetText[i].Text);
                    //Excel.Npoi.AddRosWriteToExcel(ProjectINI.ProjectPathRun + "\\报警记录\\ " + timedey + ".xls", timedey, new string[] { timeStr, text });
                    int strati = ThisF.richTextBox1.Text.Length - lisetText[i].Text.Length;
                    if (strati < 0)
                    {
                        strati = 0;
                    }
                    ThisF.richTextBox1.Select(strati, lisetText[i].Text.Length);
                    ThisF.richTextBox1.SelectionColor = lisetText[i].Color;
                    ThisF.richTextBox1.SelectionStart = ThisF.richTextBox1.Text.Length;
                    ThisF.richTextBox1.SelectionColor = col;
                }
            }
            catch (Exception)
            {
            }
        }


        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AlarmForm.UpDa(toolStripComboBox1.SelectedItem.ToString());
            }
            catch (Exception)
            {
            }
        }

        private void 打开文本记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(ProjectINI.TempPath + "\\文本记录");
                System.Diagnostics.Process.Start(ProjectINI.TempPath + "\\文本记录");
            }
            catch (Exception)
            {
            }
        }
    }
}