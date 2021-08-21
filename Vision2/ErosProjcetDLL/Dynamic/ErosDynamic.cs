using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection;

namespace Vision2.ErosProjcetDLL.Dynamic
{
    /// <summary>
    ///
    /// </summary>
    public class ErosDynamic
    {
        /// <summary>
        /// 判断动态类型中是否存在属性
        /// </summary>
        /// <param name="data">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>是否存在</returns>
        public static bool IsPropertyExist(dynamic data, string propertyName)
        {
            if (data is ExpandoObject)
                return ((IDictionary<string, object>)data).ContainsKey(propertyName);
            return data.GetType().GetProperty(propertyName) != null;
        }

        /// <summary>
        /// 读取动态实例指定的属性值,不存在则返回null，可获取父类属性
        /// </summary>
        /// <param name="data">动态实例</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>值</returns>
        public static dynamic GetPropertyExist(dynamic data, string propertyName)
        {
            if (IsPropertyExist(data, propertyName))
            {
                return data.GetType().GetProperty(propertyName).GetValue(data, null);
            }
            try
            {
                return data.GetType().BaseType.GetProperty(propertyName).GetValue(null, null);
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        /// <summary>
        /// 判断动态类型中是否存在属性
        /// </summary>
        /// <param name="data">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>是否存在</returns>
        public static bool IsFieldExist(dynamic data, string propertyName)
        {
            if (data is ExpandoObject)
            {
                return ((IDictionary<string, object>)data).ContainsKey(propertyName);
            }
            return data.GetType().GetField(propertyName, BindingFlags.Instance | BindingFlags.GetField) != null;
        }

        ///// <summary>
        ///// 读取字段
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="propertyName"></param>
        ///// <returns></returns>
        //public static dynamic GetFieldExist(dynamic data, string propertyName)
        //{
        //    if (IsFieldExist(data, propertyName))
        //    {
        //        return data.GetType().GetField(propertyName).GetValue(data, null);
        //    }
        //    try
        //    {
        //        return data.GetType().BaseType.GetField(propertyName).GetValue(null, null);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return null;
        //}

        /// <summary>
        /// 动态对象的属性赋值，
        /// </summary>
        /// <typeparam name="T">对象类</typeparam>
        /// <param name="entity">目标对象</param>
        /// <param name="collection">对象源</param>
        /// <returns>返回目标对象</returns>
        public static T PopulateEntityFromCollection<T>(T entity, dynamic collection) where T : new()
        {
            //初始化 如果为null
            if (entity == null)
            {
                entity = new T();
            }
            //取得属性集合
            PropertyInfo[] pi = collection.GetType().GetProperties();
            foreach (PropertyInfo item in pi)
            {
                //if (item.Name== "linkType")
                //{
                //}
                //给属性赋值
                if (Vision2.ErosProjcetDLL.Dynamic.ErosDynamic.IsPropertyExist(collection, item.Name))
                {
                    if (item.SetMethod != null)
                    {
                        item.SetValue(entity, Convert.ChangeType
                    (Vision2.ErosProjcetDLL.Dynamic.ErosDynamic.GetPropertyExist(collection, item.Name), item.PropertyType), null);
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// 获得DisplayName名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">对象</param>
        /// <returns></returns>
        public static string[] GetDisPlayNames_Values<T>(T entity, out string[] values)
        {
            List<string> dispNames = new List<string>();
            values = null;
            List<string> Values = new List<string>();
            if (entity != null)
            {
                System.Type t = entity.GetType();
                System.Reflection.MemberInfo[] memberInfot = t.GetMembers();
                foreach (MemberInfo var in memberInfot)
                {
                    if (var.MemberType == MemberTypes.Property)
                    {
                        var dats = var.GetCustomAttribute<DisplayNameAttribute>();
                        if (dats == null)
                        {
                            continue;
                        }
                        string names = dats.DisplayName;
                        System.Reflection.PropertyInfo propertyInfo = entity.GetType().GetProperty(var.Name);
                        if (propertyInfo.GetValue(entity, null) != null)
                        {
                            if (propertyInfo.GetValue(entity, null) is List<string>)
                            {
                                List<string> list = propertyInfo.GetValue(entity, null) as List<string>;
                                string valesT = "[";
                                foreach (var item in list)
                                {
                                    valesT += item + ",";
                                }
                                valesT = valesT.TrimEnd(',');
                                valesT += "]";
                                Values.Add(valesT);
                            }
                            else if (propertyInfo.GetValue(entity, null) is Dictionary<string, string>)
                            {
                                Dictionary<string, string> dic = propertyInfo.GetValue(entity, null) as Dictionary<string, string>;
                                string valesT = "{";
                                foreach (var item in dic)
                                {
                                    valesT += "[" + item.Key + "=" + item.Value + "]";
                                }
                                valesT += "}";
                                Values.Add(valesT);
                            }
                            else
                            {
                                Values.Add(propertyInfo.GetValue(entity, null).ToString());
                            }
                        }
                        else
                        {
                            Values.Add("null");
                        }
                        dispNames.Add(names);
                    }
                }
            }
            values = Values.ToArray();
            return dispNames.ToArray();
        }

        /// <summary>
        /// 写入对应DisplayName的属性
        /// </summary>
        public static void SetDisPlayName_Values<T>(T entity, Dictionary<string, string> values)
        {
            List<string> dispNames = new List<string>();

            List<string> Values = new List<string>();
            if (entity != null)
            {
                System.Type t = entity.GetType();
                System.Reflection.MemberInfo[] memberInfot = t.GetMembers();
                foreach (var item in values)
                {
                    foreach (MemberInfo var in memberInfot)
                    {
                        if (var.MemberType == MemberTypes.Property)
                        {
                            var dats = var.GetCustomAttribute<DisplayNameAttribute>();
                            if (dats == null)
                            {
                                continue;
                            }
                            try
                            {
                                System.Reflection.PropertyInfo propertyInfo = entity.GetType().GetProperty(var.Name);

                                if (dats.DisplayName == item.Key)
                                {
                                    if (propertyInfo.SetMethod != null)
                                    {
                                        if (item.Value != "null")
                                        {
                                            object defaultVal = null;
                                            object obj = null;
                                            string val = item.Value;
                                            try
                                            {
                                                if (!propertyInfo.PropertyType.IsGenericType)

                                                    obj = string.IsNullOrEmpty(val) ? defaultVal : Convert.ChangeType(val, propertyInfo.PropertyType, null);
                                                else
                                                {
                                                    Type genericTypeDefinition = propertyInfo.PropertyType.GetGenericTypeDefinition();
                                                    if (propertyInfo.GetValue(entity, null) is List<string>)
                                                    {
                                                        List<string> list = new List<string>();
                                                        if (val.StartsWith("[") || val.EndsWith("]"))
                                                        {
                                                            string[] datas = val.Trim('[', ']').Split(',');
                                                            list.AddRange(datas);
                                                            obj = list;
                                                        }
                                                    }
                                                    else if (genericTypeDefinition == typeof(Nullable<>))
                                                    {
                                                        propertyInfo.SetValue(obj, string.IsNullOrEmpty(val) ? null : Convert.ChangeType(val, Nullable.GetUnderlyingType(propertyInfo.PropertyType)), null);
                                                    }
                                                    else if (propertyInfo.GetValue(entity, null) is Dictionary<string, string>)
                                                    {
                                                        Dictionary<string, string> dics = new Dictionary<string, string>();
                                                        if (val.StartsWith("{") || val.EndsWith("}"))
                                                        {
                                                            string[] datas = val.Trim('{', '}').Split('[');
                                                            foreach (var itemt in datas)
                                                            {
                                                                string[] data = itemt.Trim('[', ']').Split('=');
                                                                if (data.Length == 2)
                                                                {
                                                                    dics.Add(data[0], data[1]);
                                                                }
                                                            }
                                                            obj = dics;
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception)
                                            {
                                            }
                                            propertyInfo.SetValue(entity, obj, null);
                                            //propertyInfo.SetValue(entity, item.Value);
                                        }
                                        else
                                        {
                                            propertyInfo.SetValue(entity, null);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 为指定对象分配参数
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dic">字段/值</param>
        /// <returns></returns>
        public static T Assign<T>(Dictionary<string, string> dic) where T : new()
        {
            Type myType = typeof(T);
            T entity = new T();
            var fields = myType.GetProperties();
            string val = string.Empty;
            object obj = null;

            foreach (var field in fields)
            {
                if (!dic.ContainsKey(field.Name))
                    continue;
                val = dic[field.Name];

                object defaultVal;
                if (field.PropertyType.Name.Equals("String"))
                    defaultVal = "";
                else if (field.PropertyType.Name.Equals("Boolean"))
                {
                    defaultVal = false;
                    val = (val.Equals("1") || val.Equals("on")).ToString();
                }
                else if (field.PropertyType.Name.Equals("Decimal"))
                    defaultVal = 0M;
                else
                    defaultVal = 0;

                if (!field.PropertyType.IsGenericType)
                    obj = string.IsNullOrEmpty(val) ? defaultVal : Convert.ChangeType(val, field.PropertyType);
                else
                {
                    Type genericTypeDefinition = field.PropertyType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(Nullable<>))
                        obj = string.IsNullOrEmpty(val) ? defaultVal : Convert.ChangeType(val, Nullable.GetUnderlyingType(field.PropertyType));
                }

                field.SetValue(entity, obj, null);
            }

            return entity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static dynamic GetDisPlayNameValue<T>(T entity)
        {
            return null;
        }
    }
}