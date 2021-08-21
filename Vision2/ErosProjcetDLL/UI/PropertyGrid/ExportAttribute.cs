using System;
using System.ComponentModel;

namespace Vision2.ErosProjcetDLL.UI.PropertyGrid
{
    public class PropertyOrderPair : IComparable
    {
        private int _order;
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        private PropertyDescriptor _property;

        public PropertyDescriptor Property
        {
            get
            {
                return _property;
            }
        }

        public PropertyOrderPair(string name, int order, PropertyDescriptor property)
        {
            _order = order;
            _name = name;
            _property = property;
        }

        public int CompareTo(object obj)
        {
            //
            // Sort the pair objects by ordering by order value
            // Equal values get the same rank
            //
            int otherOrder = ((PropertyOrderPair)obj)._order;
            if (otherOrder == _order)
            {
                //
                // If order not specified, sort by name
                //
                string otherName = ((PropertyOrderPair)obj)._name;
                return string.Compare(_name, otherName);
            }
            else if (otherOrder > _order)
            {
                return -1;
            }
            return 1;
        }
    }

    /// <summary>
    /// 属性信息的顺序分类等信息
    /// 字段或者属性等 可序列化的信息上
    /// 现在是一共9位，最后3位用来标识编辑器 最后三位000 默认不会识别编辑器
    /// 中间三位用来属性排序
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PropertyIndexAttribute : Attribute
    {
        private string indexCode;

        /// <summary>
        /// 标识
        /// </summary>
        public string IndexCode
        {
            get
            {
                return indexCode;
            }
            set
            {
                indexCode = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="indexCode"></param>
        public PropertyIndexAttribute(string indexCode)
        {
            this.indexCode = indexCode;
        }
    }
}