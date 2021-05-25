using ErosSocket.ErosConLink;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.DataGridViewF;

namespace Vision2.ErosUI
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Button button1;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.button1 = new System.Windows.Forms.Button();
            //
            // button1
            //
            this.button1.Location = new System.Drawing.Point(116, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button1_MouseDown);

            this.Controls.Add(this.button1);
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            this.button1.DoDragDrop(sender, DragDropEffects.Move);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            Form2 frm = new Form2();
            frm.Show();
        }

        private void InitializeComponent()
        {
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // button2
            //
            this.button2.Location = new System.Drawing.Point(891, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            //
            // Form1
            //
            this.ClientSize = new System.Drawing.Size(992, 595);
            this.Controls.Add(this.button2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }

        [DefaultPropertyAttribute("SaveOnClose")]
        public class AppSettings
        {
            private bool saveOnClose = true;
            private string greetingText = "欢迎使用应用程序！";
            private int maxRepeatRate = 10;
            private int itemsInMRU = 4;
            private bool settingsChanged = false;
            private string appVersion = "1.0";

            private SpellingOptions spelling = new SpellingOptions();

            private Size windowSize = new Size(100, 100);
            private Font windowFont = new Font("宋体", 9, FontStyle.Regular);
            private Color toolbarColor = SystemColors.Control;
            private string defaultFileName = "";
            private string valusFileName = "";

            // 应用到 DefaultFileName 属性的 TypeConverter 特性。
            [CategoryAttribute("变量设置"), DescriptionAttribute("链接名。")]
            public string DefaultFileName
            {
                get { return defaultFileName; }
                set { defaultFileName = value; }
            }

            [CategoryAttribute("变量设置"), DescriptionAttribute("变量名称。")]
            public string ValusFileName
            {
                get { return valusFileName; }
                set
                {
                    valusFileName = value;
                    valuID = StaticCon.SocketClint[DefaultFileName].KeysValues.DictionaryValueD[value].AddressID;
                }
            }

            [CategoryAttribute("变量设置"), DescriptionAttribute("变量地址。")]
            public string ValuID
            {
                get { return valuID; }
                set
                {
                    valuID = value;
                    var dicSort = from objDic in StaticCon.SocketClint[DefaultFileName].KeysValues.DictionaryValueD
                                  where objDic.Value.AddressID == ValuID
                                  select new { objDic.Key };
                    foreach (var item in dicSort)
                    {
                        valusFileName = item.Key;
                    }
                }
            }

            private string valuID;

            [DescriptionAttribute("展开以查看应用程序的拼写选项。")]
            public SpellingOptions Spelling
            {
                get { return spelling; }
                set { spelling = value; }
            }

            [CategoryAttribute("文档设置"),
            DefaultValueAttribute(true)]
            public bool SaveOnClose
            {
                get { return saveOnClose; }
                set { saveOnClose = value; }
            }

            [CategoryAttribute("文档设置")]
            public Size WindowSize
            {
                get { return windowSize; }
                set { windowSize = value; }
            }

            [CategoryAttribute("文档设置")]
            public Font WindowFont
            {
                get { return windowFont; }
                set { windowFont = value; }
            }

            [CategoryAttribute("全局设置")]
            public Color ToolbarColor
            {
                get { return toolbarColor; }
                set { toolbarColor = value; }
            }

            [CategoryAttribute("全局设置"),
            ReadOnlyAttribute(true),
            DefaultValueAttribute("欢迎使用应用程序！")]
            public string GreetingText
            {
                get { return greetingText; }
                set { greetingText = value; }
            }

            [CategoryAttribute("全局设置"),
            DefaultValueAttribute(4)]
            public int ItemsInMRUList
            {
                get { return itemsInMRU; }
                set { itemsInMRU = value; }
            }

            [DescriptionAttribute("以毫秒表示的文本重复率。"),
            CategoryAttribute("全局设置"),
            DefaultValueAttribute(10)]
            public int MaxRepeatRate
            {
                get { return maxRepeatRate; }
                set { maxRepeatRate = value; }
            }

            [BrowsableAttribute(false),
            DefaultValueAttribute(false)]
            public bool SettingsChanged
            {
                get { return settingsChanged; }
                set { settingsChanged = value; }
            }

            [CategoryAttribute("版本"),
            DefaultValueAttribute("1.0"),
            ReadOnlyAttribute(true)]
            public string AppVersion
            {
                get { return appVersion; }
                set { appVersion = value; }
            }
        }

        [TypeConverterAttribute(typeof(SpellingOptionsConverter)),
         DescriptionAttribute("展开以查看应用程序的拼写选项。")]
        public class SpellingOptions
        {
            private bool spellCheckWhileTyping = true;
            private bool spellCheckCAPS = false;
            private bool suggestCorrections = true;

            [DefaultValueAttribute(true)]
            public bool SpellCheckWhileTyping
            {
                get { return spellCheckWhileTyping; }
                set { spellCheckWhileTyping = value; }
            }

            [DefaultValueAttribute(false)]
            public bool SpellCheckCAPS
            {
                get { return spellCheckCAPS; }
                set { spellCheckCAPS = value; }
            }

            [DefaultValueAttribute(true)]
            public bool SuggestCorrections
            {
                get { return suggestCorrections; }
                set { suggestCorrections = value; }
            }
        }

        public class SpellingOptionsConverter : ExpandableObjectConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
            {
                if (destinationType == typeof(SpellingOptions))
                    return true;
                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
            {
                if (destinationType == typeof(System.String) &&
                     value is SpellingOptions)
                {
                    SpellingOptions so = (SpellingOptions)value;
                    return "在键入时检查:" + so.SpellCheckWhileTyping +
                           "，检查大小写: " + so.SpellCheckCAPS +
                           "，建议更正: " + so.SuggestCorrections;
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
            {
                if (sourceType == typeof(string))
                    return true;
                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                {
                    try
                    {
                        string s = (string)value;
                        int colon = s.IndexOf(':');
                        int comma = s.IndexOf(',');
                        if (colon != -1 && comma != -1)
                        {
                            string checkWhileTyping = s.Substring(colon + 1, (comma - colon - 1));
                            colon = s.IndexOf(':', comma + 1);
                            comma = s.IndexOf(',', comma + 1);
                            string checkCaps = s.Substring(colon + 1, (comma - colon - 1));
                            colon = s.IndexOf(':', comma + 1);
                            string suggCorr = s.Substring(colon + 1);
                            SpellingOptions so = new SpellingOptions();
                            so.SpellCheckWhileTyping = Boolean.Parse(checkWhileTyping);
                            so.SpellCheckCAPS = Boolean.Parse(checkCaps);
                            so.SuggestCorrections = Boolean.Parse(suggCorr);
                            return so;
                        }
                    }
                    catch
                    {
                        throw new ArgumentException(
                            "无法将“" + (string)value +
                                               "”转换为 SpellingOptions 类型");
                    }
                }
                return base.ConvertFrom(context, culture, value);
            }
        }

        //public class FileNameConverter : StringConverter
        //{
        //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        //    {
        //        return true;
        //    }

        //    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        //    {
        //        return new StandardValuesCollection(StaticCon.SocketClint.Keys);
        //    }

        //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        //    {
        //        return false;
        //    }
        //}

        //public class ValusNameConverter : StringConverter
        //{
        //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        //    {
        //        return true;
        //    }

        //    public string LinkName;

        //    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        //    {
        //        try
        //        {
        //            LinkName = ((global::Vision2.ErosUI.Form1.AppSettings)(context).Instance).DefaultFileName;
        //            return new StandardValuesCollection(ErosSocket.ErosConLink.StaticCon.SocketClint[LinkName].KeysValues.DictionaryValueD.Keys);
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //        return new StandardValuesCollection(new string[] { });
        //    }

        //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        //    {
        //        return false;
        //    }
        //}

        //public class ValuIDConverter : SingleConverter
        //{
        //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        //    {
        //        return true;
        //    }

        //    public string LinkName;

        //    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        //    {
        //        try
        //        {
        //            LinkName = ((global::Vision2.ErosUI.Form1.AppSettings)(context).Instance).DefaultFileName;
        //            var dicSort = from objDic in ErosSocket.ErosConLink.StaticCon.SocketClint[LinkName].KeysValues.DictionaryValueD
        //                              //where objDic.Value.AddressID
        //                          select objDic.Value.AddressID.ToString();
        //            return new StandardValuesCollection(dicSort.ToArray());
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //        return new StandardValuesCollection(new string[] { });
        //    }

        //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        //    {
        //        return false;
        //    }
        //}

        private Button button2;
        private System.Windows.Forms.PropertyGrid OptionsPropertyGrid;

        private void Form1_Load(object sender, EventArgs e)
        {
            //OptionsPropertyGrid = new PropertyGrid();
            //OptionsPropertyGrid.Size = new Size(500, 500);
            //OptionsPropertyGrid.ToolbarVisible = true;
            //this.Controls.Add(OptionsPropertyGrid);
            //this.Text = "选项对话框";
            //AppSettings appset = new AppSettings();
            //OptionsPropertyGrid.SelectedObject = appset;
            DataGridViewEx dataGridViewEx = new DataGridViewEx();
            dataGridViewEx.Dock = DockStyle.Fill;
            DataGridViewGroupColumn column1 = new DataGridViewGroupColumn();
            column1.HeaderText = "列1";
            DataGridViewComboBoxColumnEx column2 = new DataGridViewComboBoxColumnEx();
            column2.HeaderText = "列2";
            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column3.HeaderText = "列3";
            DataGridViewComboBoxColumn column4 = new DataGridViewComboBoxColumn();
            column4.HeaderText = "列4";
            dataGridViewEx.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
           column1, column2 ,column3,column4});
            this.Controls.Add(dataGridViewEx);

            for (int i = 0; i < 5; i++)
            {
                dataGridViewEx.Rows.Add();
            }
            DataGridViewGroupCell cell1 = dataGridViewEx.Rows[0].Cells[0] as DataGridViewGroupCell;
            DataGridViewGroupCell cell2 = dataGridViewEx.Rows[1].Cells[0] as DataGridViewGroupCell;
            DataGridViewGroupCell cell3 = dataGridViewEx.Rows[2].Cells[0] as DataGridViewGroupCell;
            //DataGridViewGroupCell cell4 = dataGridViewEx.Rows[3].Cells[0] as DataGridViewGroupCell;
            cell1.AddChildCellRange(new DataGridViewGroupCell[] { cell2, cell3 });
            //cell3.AddChildCell(cell4);
            //dataGridViewEx.Rows.Add(cell1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //OptionsPropertyGrid.SelectedObject = new ErosBtn().SeeName;
        }
    }
}