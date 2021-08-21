using System;
using System.Collections.Generic;
using System.ComponentModel;

/// <summary>
/// 转换类
/// </summary>
namespace Vision2.ErosProjcetDLL.UI.PropertyGrid
{
    /// <summary>
    /// 从标记类ThisDropDownAttribute指定下拉列表选项值的动态转换；
    /// </summary>
    public class ErosConverter : StringConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> lstValues = new List<string>();
            ThisDropDownAttribute db = context.PropertyDescriptor.Attributes[typeof(ThisDropDownAttribute)] as ThisDropDownAttribute;  //<---
            try
            {
                dynamic dynamic = context.Instance;
                dynamic = Vision2.ErosProjcetDLL.Dynamic.ErosDynamic.GetPropertyExist(dynamic, db.TypeName);

                if (dynamic != null)
                {
                    if (dynamic is List<string>)
                    {
                        lstValues = dynamic;
                    }
                    else if (Vision2.ErosProjcetDLL.Dynamic.ErosDynamic.GetPropertyExist(dynamic, "Keys") != null)
                    {
                        foreach (var item in dynamic.Keys)
                        {
                            lstValues.Add(item);
                        }
                    }
                    else if (dynamic is List<Type>)
                    {
                        foreach (var item in dynamic)
                        {
                            lstValues.Add(item.ToString());
                        }
                    }
                    else if (dynamic is string[])
                    {
                        foreach (var item in dynamic)
                        {
                            lstValues.Add(item.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (db.listValu != null)
            {
                foreach (var item in db.listValu)
                {
                    if (!lstValues.Contains(item))
                    {
                        lstValues.Add(item);
                    }
                }
            }
            //List<string> lstValues = CommonDropDownValue.GetData(db.TypeName);   //<---
            return new StandardValuesCollection(lstValues.ToArray());
        }

        /// <summary>
        /// 是否使用标准值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// 是否禁用编译选择项
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            ThisDropDownAttribute db = context.PropertyDescriptor.Attributes[typeof(ThisDropDownAttribute)] as ThisDropDownAttribute;  //<---
            return db.IsEditor;
        }

        /// <summary>
        /// Attribute标记类
        /// 从标记的This实例指定属性获取动态下拉列表;指定是否可编译，输入的编译项是否添加到List;可添加其他选择项;
        /// </summary>
        public class ThisDropDownAttribute : Attribute
        {
            /// <summary>
            /// 从指定属性获取动态下拉列表;指定是否可编译，输入的编译项是否添加到List;可添加其他选择项;
            /// </summary>
            /// <param name="typeName">This实例List属性名</param>
            /// <param name="isEditor">False可编译,True禁用编辑</param>
            /// <param name="isAdd">新输入的编译项是否添加到List</param>
            /// <param name="addValue">添加其他下拉选项</param>
            public ThisDropDownAttribute(string typeName, bool isEditor, bool isAdd, params string[] addValue)
            {
                TypeName = typeName;
                IsEditor = isEditor;
                IsAdd = isAdd;
                if (addValue != null)
                {
                    listValu = new List<string>();
                    listValu.AddRange(addValue);
                }
            }

            /// <summary>
            /// 从指定属性获取动态下拉列表;指定False可编译，输入的编译项不会添加到List;可添加其他选择项;
            /// </summary>
            /// <param name="typeName">This实例List属性名</param>
            /// <param name="isEditor">False可编译,True禁用编辑</param>
            /// <param name="addValue">添加其他下拉选项</param>
            public ThisDropDownAttribute(string typeName, bool isEditor, params string[] addValue) : this(typeName, isEditor, false, addValue)
            {
            }

            /// <summary>
            /// 从指定属性获取动态下拉列表，指定不可编译，可添加其他选择项
            /// </summary>
            /// <param name="typeName">This实例List属性名</param>
            /// <param name="addValue">添加其他下拉选项</param>
            public ThisDropDownAttribute(string typeName, params string[] addValue) : this(typeName, true, false, addValue)
            {
            }

            public string TypeName { get; set; }
            public List<string> listValu;

            public bool IsEditor;

            /// <summary>
            /// 新输入的编译项是否添加到List
            /// </summary>
            public bool IsAdd;
        }
    }
}