using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Vision2.ErosProjcetDLL.StaticConFile
{
    /// <summary>
    /// 公共类
    /// </summary>
    public class StaticCon
    {
        /// <summary>
        /// 枚举类型转换为键值对
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <returns>键值</returns>
        public static Dictionary<int, string> EnumToDictionary<T>()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            if (!typeof(T).IsEnum)
            {
                return dic;
            }
            string desc = string.Empty;
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                desc = item.ToString();
                var attrs = item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    DescriptionAttribute descAttr = attrs[0] as DescriptionAttribute;
                    desc = descAttr.Description;
                }
                dic.Add(Convert.ToInt32(item), desc);
            }
            return dic;
        }

    }
}