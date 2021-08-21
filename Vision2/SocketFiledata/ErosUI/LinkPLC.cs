using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;

namespace ErosSocket.ErosUI
{
    public class LinkPLC
    {
        public LinkPLC()
        {
            try
            {
                StatusColor.Add(0, Color.Green);
                StatusColor.Add(1, Color.Red);
            }
            catch (Exception)
            {
            }
        }

        [CategoryAttribute("变量设置"), DisplayName("读取名称")]
        [Editor(typeof(LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string GetName { get; set; }

        [CategoryAttribute("变量设置"), DisplayName("写入名称")]
        [Editor(typeof(LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string SetName { get; set; }

        [CategoryAttribute("状态显示"), DisplayName("值与按键文本映射")]
        public Dictionary<byte, string> ValueClickTexts { get; set; } = new Dictionary<byte, string>();

        [CategoryAttribute("状态显示"), DisplayName("值与背景色映射")]
        public Dictionary<byte, Color> StatusColor { get; set; } = new Dictionary<byte, Color>();

        /// <summary>
        ///
        /// </summary>
        [CategoryAttribute("状态显示"), DisplayName("按键事件")]
        public Dictionary<byte, string> UpliftClickNames { get; set; } = new Dictionary<byte, string>();

        /// <summary>
        /// 值取反
        /// </summary>
        /// <param name="elingk"></param>
        /// <param name="valueName"></param>
        public void BoolNegate(string valueName)
        {
            try
            {
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        if (Convert.ToBoolean(StaticCon.GetLingkNameValueString(valueName)))
                        {
                            StaticCon.SetLingkValue(valueName, false.ToString(), out string err);
                        }
                        else
                        {
                            StaticCon.SetLingkValue(valueName, true.ToString(), out string err);
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
                thread.Start();
                thread.IsBackground = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(valueName + ",写入值错误：" + ex.Message);
            }
        }

        /// <summary>
        /// 对链接写入指定变量名的booL值
        /// </summary>
        /// <param name="elingk"></param>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        public void BoolSetValue(string valueName, bool value)
        {
            try
            {
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        StaticCon.SetLingkValue(valueName, value.ToString(), out string err);
                    }
                    catch (Exception)
                    {
                    }
                });
                thread.Start();
                thread.IsBackground = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(valueName + ",写入值错误：" + ex.Message);
            }
        }

        /// <summary>
        /// 值取反
        /// </summary>
        /// <param name="elingk"></param>
        /// <param name="valueName"></param>
        public static void SBoolNegate(string valueName)
        {
            try
            {
                if (valueName == null)
                {
                    return;
                }
                Thread thread = new Thread(() =>
                          {
                              try
                              {
                                  dynamic dynamic = StaticCon.GetLingkNameValue(valueName);
                                  if (dynamic)
                                  {
                                      StaticCon.SetLingkValue(valueName, false, out string err);
                                  }
                                  else
                                  {
                                      StaticCon.SetLingkValue(valueName, true, out string err);
                                  }
                              }
                              catch (Exception)
                              {
                              }
                          });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(valueName + ",写入值错误：" + ex.Message);
            }
        }

        /// <summary>
        /// 对链接写入指定变量名的booL值
        /// </summary>
        /// <param name="elingk"></param>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        public static void SBoolSetValue(string valueName, bool value)
        {
            try
            {
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        StaticCon.SetLingkValue(valueName, value.ToString(), out string err);
                    }
                    catch (Exception)
                    {
                    }
                });
                thread.Start();
                thread.IsBackground = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(valueName + ",写入值错误：" + ex.Message);
            }
        }

        ///// <summary>
        ///// 获得链接名称集合
        ///// </summary>
        //public class LinkNameConverter : StringConverter
        //{
        //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        //    {
        //        return true;
        //    }

        //    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        //    {
        //        return new StandardValuesCollection(ErosSocket.ErosConLink.StaticCon.SocketClint.Keys);
        //    }

        //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        //    {
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 获得链接变量名称集合
        ///// </summary>
        //public class ValusNameConverter : StringConverter
        //{
        //    private string LinkName = string.Empty;

        //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        //    {
        //        return true;
        //    }

        //    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        //    {
        //        try
        //        {
        //            if (true)
        //            {
        //            }
        //            dynamic obj = context.Instance;
        //            if (obj.LingkName != null)
        //            {
        //                LinkName = obj.LingkName;
        //                return new StandardValuesCollection(ErosSocket.ErosConLink.StaticCon.SocketClint[LinkName].KeysValues.DictionaryValueD.Keys);
        //            }
        //            else
        //            {
        //            }
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

        //public class ValuIDConverter : StringConverter
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
        //            LinkName = ((LinkPLC)context.Instance).LingkName;
        //            if (LinkName == null)
        //            {
        //                MessageBox.Show("请先选择链接名");
        //                return new StandardValuesCollection(new List<string>());
        //            }

        //            var dicSort = from objDic in ErosSocket.ErosConLink.StaticCon.SocketClint[LinkName].KeysValues.DictionaryValueD
        //                          orderby objDic.Value.AddressID ascending
        //                          select objDic.Value.District + objDic.Value.AddressID.ToString();
        //            List<string> listst = dicSort.ToList();

        //            return new StandardValuesCollection(listst);
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
    }

    //public class SpellingOptionsConverter : ExpandableObjectConverter
    //{
    //    public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
    //    {
    //        if (destinationType == typeof(LinkPLC))
    //            return true;
    //        return base.CanConvertTo(context, destinationType);
    //    }

    //    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
    //    {
    //        if (destinationType == typeof(System.String) &&
    //             value is LinkPLC)
    //        {
    //            LinkPLC so = (LinkPLC)value;
    //            return "请输入链接名和变量名";
    //        }
    //        return base.ConvertTo(context, culture, value, destinationType);
    //    }

    //    public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
    //    {
    //        if (sourceType == typeof(string))
    //            return true;
    //        return base.CanConvertFrom(context, sourceType);
    //    }

    //    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    //    {
    //        if (value is string)
    //        {
    //            try
    //            {
    //                string s = (string)value;
    //                int colon = s.IndexOf(':');
    //                int comma = s.IndexOf(',');
    //                if (colon != -1 && comma != -1)
    //                {
    //                    string checkWhileTyping = s.Substring(colon + 1, (comma - colon - 1));
    //                    colon = s.IndexOf(':', comma + 1);
    //                    comma = s.IndexOf(',', comma + 1);
    //                    string checkCaps = s.Substring(colon + 1, (comma - colon - 1));
    //                    colon = s.IndexOf(':', comma + 1);
    //                    string suggCorr = s.Substring(colon + 1);
    //                    LinkPLC so = new LinkPLC();

    //                    return so;
    //                }
    //            }
    //            catch
    //            {
    //                throw new ArgumentException(
    //                    "无法将“" + (string)value +
    //                                       "”转换为 SpellingOptions 类型");
    //            }
    //        }
    //        return base.ConvertFrom(context, culture, value);
    //    }
    //}
}