using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Vision2.ErosProjcetDLL.Project
{
    public enum enumLanguage
    {
        Chinese,
        English,
    }

    public class MultiLanguage
    {
        //当前默认语言
        public static string DefaultLanguage = "ChineseSimplified";

        private static List<string> ListMenu = new List<string>();
        private static Dictionary<string, ToolStripMenuItem> DicMenu = new Dictionary<string, ToolStripMenuItem>();

        /// <summary>
        /// 读取当前默认语言
        /// </summary>
        /// <returns>当前默认语言</returns>
        public static string GetDefaultLanguage()
        {
            string defaultLanguage = "ChineseSimplified";

            XDocument document = new XDocument();
            string strRead = "Languages/" + "readme" + ".xml";
            string strFile = System.Windows.Forms.Application.StartupPath + "/" + strRead;
            if (!System.IO.File.Exists(strFile))
            {
                defaultLanguage = string.Empty;
                return defaultLanguage;
            }

            document = XDocument.Load(strRead);

            XElement root1 = document.Root;
            defaultLanguage = root1.FirstAttribute.Value;

            return defaultLanguage;
        }

        /// <summary>
        /// 修改默认语言
        /// </summary>
        /// <param name="lang">待设置默认语言</param>
        public static void SetDefaultLanguage(string lang)
        {
            try
            {
                DataSet ds = new DataSet();
                XDocument document = new XDocument();
                document = XDocument.Load("Languages/" + "readme" + ".xml");
                XElement root = document.Root;
                root.FirstAttribute.Value = lang;
                document.Save("Languages/" + "readme" + ".xml");
            }
            catch (Exception)
            {
            }
        }

        private static void EnumerateMenu(ToolStripMenuItem item)
        {
            foreach (ToolStripMenuItem subItem in item.DropDownItems)
            {
                ListMenu.Add(subItem.Name);
                DicMenu.Add(subItem.Name, subItem);
                EnumerateMenu(subItem);
            }
        }

        /// <summary>
        /// 加载语言
        /// </summary>
        /// <param name="form">加载语言的窗口</param>
        public static bool LoadLanguage(Form form, string language)
        {
            if (form == null || form.IsDisposed)
            {
                return false;
            }
            if (string.IsNullOrEmpty(language))
            {
                return false;
            }
            //根据用户选择的语言获得表的显示文字
            Hashtable hashText = ReadXMLText(form.Name, language);
            Hashtable hashHeaderText = ReadXMLHeaderText(form.Name, language);
            if (hashText == null)
            {
                return false;
            }
            //获取当前窗口的所有控件
            Control.ControlCollection sonControls = form.Controls;

            try
            {
                DicMenu.Clear();
                ListMenu.Clear();
                MenuStrip menu = form.MainMenuStrip;
                if (menu != null)
                {
                    foreach (ToolStripMenuItem item in menu.Items)
                    {
                        ListMenu.Add(item.Name);
                        DicMenu.Add(item.Name, item);
                        EnumerateMenu(item);
                    }
                }

                var result = from pair in DicMenu orderby pair.Key select pair;
                foreach (KeyValuePair<string, ToolStripMenuItem> pair in result)
                {
                    if (hashText.Contains(pair.Key))
                    {
                        pair.Value.Text = (string)hashText[pair.Key];
                    }
                }
                Control[] controls;
                foreach (var item in hashHeaderText.Keys)
                {
                    controls = form.Controls.Find(item.ToString(), true);
                    if (controls.Length > 0)
                    {
                    }
                }
                controls = form.Controls.Find("lanquageToolStripMenultem", true);

                if (controls.Length > 0)
                {
                }
                //遍历所有控件
                foreach (Control control in sonControls)
                {
                    if (control.GetType() == typeof(Panel))     //Panel
                    {
                        GetSetSubControls(control.Controls, hashText, hashHeaderText);
                    }
                    else if (control.GetType() == typeof(GroupBox))     //GroupBox
                    {
                        GetSetSubControls(control.Controls, hashText, hashHeaderText);
                    }
                    else if (control.GetType() == typeof(TabControl))       //TabControl
                    {
                        GetSetSubControls(control.Controls, hashText, hashHeaderText);
                    }
                    else if (control.GetType() == typeof(TabPage))      //TabPage
                    {
                        GetSetSubControls(control.Controls, hashText, hashHeaderText);
                    }
                    else if (control.GetType() == typeof(TableLayoutPanel))     //TableLayoutPanel
                    {
                        GetSetSubControls(control.Controls, hashText, hashHeaderText);
                    }
                    else if (control.GetType() == typeof(DataGridView))     //DataGridView
                    {
                        GetSetHeaderCell((DataGridView)control, hashHeaderText);
                    }
                    else if (control.GetType() == typeof(Button))     //Button
                    {
                        GetSetSubControls(control.Controls, hashText, hashHeaderText);
                    }
                    else if (control.GetType() == typeof(ToolStripMenuItem))     //menu
                    {
                        GetSetSubControls(control.Controls, hashText, hashHeaderText);
                    }
                    GetContNameText(control, hashText);
                }
                ////如果找到了控件，就将对应的名字赋值过去
                if (hashText.Contains(form.Text))
                {
                    form.Text = (string)hashText[form.Text];
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                return false;
            }
            return true;
        }

        private static void GetContNameText(Control control, Hashtable hashText)
        {
            try
            {
                if (hashText.Contains(control.Text))
                {
                    control.Text = (string)hashText[control.Text];
                }
                else
                {
                    GetSetSubControls(control.Controls, hashText, hashText);
                    for (int i = 0; i < control.Controls.Count; i++)
                    {
                        GetContNameText(control.Controls[i], hashText);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 获取并设置控件中的子控件
        /// </summary>
        /// <param name="controls">父控件</param>
        /// <param name="hashResult">哈希表</param>
        private static void GetSetSubControls(Control.ControlCollection controls, Hashtable hashText, Hashtable hashHeaderText)
        {
            try
            {
                foreach (Control control in controls)
                {
                    if (control.GetType() == typeof(Panel))     //Panel
                    {
                        GetSetSubControls(control.Controls, hashText, hashHeaderText);
                    }
                    else if (control.GetType() == typeof(GroupBox))     //GroupBox
                    {
                        GetSetSubControls(control.Controls, hashText, hashHeaderText);
                    }
                    else if (control.GetType() == typeof(TabControl))       //TabControl
                    {
                        GetSetSubControls(control.Controls, hashText, hashHeaderText);
                    }
                    else if (control.GetType() == typeof(TabPage))      //TabPage
                    {
                        GetSetSubControls(control.Controls, hashText, hashHeaderText);
                    }
                    else if (control.GetType() == typeof(TableLayoutPanel))     //TableLayoutPanel
                    {
                        GetSetSubControls(control.Controls, hashText, hashHeaderText);
                    }
                    else if (control.GetType() == typeof(DataGridView))
                    {
                        GetSetHeaderCell((DataGridView)control, hashHeaderText);
                    }
                    else if (control.GetType() == typeof(Button))     //Button
                    {
                        GetSetSubControls(control.Controls, hashText, hashHeaderText);
                    }
                    else
                    {
                    }

                    if (hashText.Contains(control.Text))
                    {
                        control.Text = (string)hashText[control.Text];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 从XML文件中读取需要修改Text的內容
        /// </summary>
        /// <param name="frmName">窗口名，用于获取对应窗口的那部分内容</param>
        /// <param name="xmlName">目标语言</param>
        /// <returns></returns>
        private static Hashtable ReadXMLText(string frmName, string xmlName)
        {
            try
            {
                Hashtable hashResult = new Hashtable();
                XmlReader reader = null;
                //判断是否存在该语言的配置文件
                if (!(new System.IO.FileInfo("Languages/" + xmlName + ".xml")).Exists)
                {
                    return null;
                }
                else
                {
                    reader = new XmlTextReader("Languages/" + xmlName + ".xml");
                }

                XDocument document = new XDocument();
                document = XDocument.Load("Languages/" + xmlName + ".xml");

                var classData = (from n in document.Root.Elements("Form")
                                 where n.Attribute("Name").Value == frmName
                                 select n).ToList();
                foreach (var item in classData.Elements("Controls"))
                {
                    XElement xe = (XElement)item;
                    XAttribute xName = xe.Attribute("name");
                    XAttribute xText = xe.Attribute("text");
                    string name = xName.Value;
                    string text = xText.Value;
                    if (name != null && text != null)
                    {
                        hashResult.Add(name, text);
                    }
                }

                reader.Close();
                // reader.Dispose();
                return hashResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 从XML文件中读取需要修改HeaderText的內容
        /// </summary>
        /// <param name="frmName">窗口名，用于获取对应窗口的那部分内容</param>
        /// <param name="xmlName">目标语言</param>
        /// <returns></returns>
        private static Hashtable ReadXMLHeaderText(string frmName, string xmlName)
        {
            try
            {
                Hashtable hashResult = new Hashtable();
                XmlReader reader = null;
                //判断是否存在该语言的配置文件
                if (!(new System.IO.FileInfo("Languages/" + xmlName + ".xml")).Exists)
                {
                    return null;
                }
                else
                {
                    reader = new XmlTextReader("Languages/" + xmlName + ".xml");
                }

                XDocument document = new XDocument();
                document = XDocument.Load("Languages/" + xmlName + ".xml");

                var classData = (from n in document.Root.Elements("Form")
                                 where n.Attribute("Name").Value == frmName
                                 select n).ToList();
                foreach (var item in classData.Elements("Controls"))
                {
                    XElement xe = (XElement)item;
                    XAttribute xName = xe.Attribute("name");
                    XAttribute xText = xe.Attribute("text");
                    string name = xName.Value;
                    string text = xText.Value;
                    if (name != null && text != null)
                    {
                        hashResult.Add(name, text);
                    }
                }
                reader.Close();
                //reader.Dispose();
                return hashResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取并设置DataGridView的列头
        /// </summary>
        /// <param name="dataGridView">DataGridView名</param>
        /// <param name="hashResult">哈希表</param>
        private static void GetSetHeaderCell(DataGridView dataGridView, Hashtable hashHeaderText)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (hashHeaderText.Contains(column.Name.ToLower()))
                {
                    column.HeaderText = (string)hashHeaderText[column.Name.ToLower()];
                }
            }
        }
    }
}