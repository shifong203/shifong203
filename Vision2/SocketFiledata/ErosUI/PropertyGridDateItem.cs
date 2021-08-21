using ErosSocket.ErosConLink;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ErosSocket.ErosUI
{
    /// <summary>

    /// 在PropertyGrid 上显示日期控件

    /// </summary>

    public class PropertyGridDateItem : UITypeEditor
    {
        private MonthCalendar dateControl = new MonthCalendar();

        public PropertyGridDateItem()
        {
            dateControl.MaxSelectionCount = 1;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            try
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    if (value is string)
                    {
                        dateControl.SelectionStart = DateTime.Parse(value as String);
                        edSvc.DropDownControl(dateControl);
                        return dateControl.SelectionStart.ToShortDateString();
                    }
                    else if (value is DateTime)
                    {
                        dateControl.SelectionStart = (DateTime)value;
                        edSvc.DropDownControl(dateControl);
                        return dateControl.SelectionStart;
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Console.WriteLine("PropertyGridDateItem Error : " + ex.Message);
                return value;
            }
            return value;
        }
    }

    public class SomeProperties
    {
        private bool _bool = true;

        [Description("布尔"), Category("属性"), TypeConverter(typeof(PropertyGridBoolItem))]
        public int 布尔
        {
            get { return _bool == true ? 0 : 1; }
            set { _bool = (value == 0 ? true : false); }
        }

        // 选择列表
        private int _comboBoxItems = 0;

        [Description("选择列表"), Category("属性"), TypeConverter(typeof(PropertyGridComboBoxItem))]
        public int 选择列表
        {
            get { return _comboBoxItems; }
            set { _comboBoxItems = value; }
        }
    }    /// IMSTypeConvert 的摘要说明。  步骤一：定义从UITypeEditor 继承的抽象类：ComboBoxItemTypeConvert。示例如下：

    public abstract class ComboBoxItemTypeConvert : TypeConverter
    {
        public Hashtable _hash;

        public ComboBoxItemTypeConvert()
        {
            _hash = new Hashtable();
            GetConvertHash();
        }

        public abstract void GetConvertHash();

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            int[] ids = new int[_hash.Values.Count];
            int i = 0;
            foreach (DictionaryEntry myDE in _hash)
            {
                ids[i++] = (int)(myDE.Key);
            }
            return new StandardValuesCollection(ids);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object v)

        {
            if (v is string)

            {
                foreach (DictionaryEntry myDE in _hash)

                {
                    if (myDE.Value.Equals((v.ToString())))

                        return myDE.Key;
                }
            }

            return base.ConvertFrom(context, culture, v);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,

                  object v, Type destinationType)

        {
            if (destinationType == typeof(string))

            {
                foreach (DictionaryEntry myDE in _hash)

                {
                    if (myDE.Key.Equals(v))

                        return myDE.Value.ToString();
                }

                return "";
            }

            return base.ConvertTo(context, culture, v, destinationType);
        }

        public override bool GetStandardValuesExclusive(
            ITypeDescriptorContext context)
        {
            return false;
        }
    }

    //步骤二：定义 ComboBoxItemTypeConvert 的派生类，派生类中实现父类的抽象方法：public abstract void GetConvertHash(); 示例如下：
    public class PropertyGridBoolItem : ComboBoxItemTypeConvert
    {
        public override void GetConvertHash()
        {
        }
    }

    public class PropertyGridComboBoxItem : ComboBoxItemTypeConvert
    {
        public PropertyGridComboBoxItem()
        {
        }

        public PropertyGridComboBoxItem(string linkName)
        {
            int i = 0;
            foreach (var item in StaticCon.SocketClint[linkName].KeysValues.DictionaryValueD)
            {
                _hash.Add(i, item.Key);
                i++;
            }
        }

        public override void GetConvertHash()
        {
            _hash.Add(0, "炒肝");
            _hash.Add(1, "豆汁");
            _hash.Add(2, "灌肠");
        }
    }
}