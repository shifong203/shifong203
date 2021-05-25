using System;
using System.ComponentModel;

namespace Vision2.ErosProjcetDLL.UI.PropertyGrid
{
    #region Helper Class - PropertyOrderPair

    //public class PropertyOrderPair : IComparable
    //{
    //    private int _order;
    //    private string _name;

    //    public string Name
    //    {
    //        get
    //        {
    //            return _name;
    //        }
    //    }

    //    private PropertyDescriptor _property;

    //    public PropertyDescriptor Property
    //    {
    //        get
    //        {
    //            return _property;
    //        }
    //    }

    //    public PropertyOrderPair(string name, int order, PropertyDescriptor property)
    //    {
    //        _order = order;
    //        _name = name;
    //        _property = property;
    //    }

    //    public int CompareTo(object obj)
    //    {
    //        //
    //        // Sort the pair objects by ordering by order value
    //        // Equal values get the same rank
    //        //
    //        int otherOrder = ((PropertyOrderPair)obj)._order;
    //        if (otherOrder == _order)
    //        {
    //            //
    //            // If order not specified, sort by name
    //            //
    //            string otherName = ((PropertyOrderPair)obj)._name;
    //            return string.Compare(_name, otherName);
    //        }
    //        else if (otherOrder > _order)
    //        {
    //            return -1;
    //        }
    //        return 1;
    //    }
    //}

    #endregion Helper Class - PropertyOrderPair

    #region Helper Class - PropertyOrderAttribute 未用

    //[AttributeUsage(AttributeTargets.Property)]
    //public class PropertyOrderAttribute : Attribute
    //{
    //    //
    //    // Simple attribute to allow the order of a property to be specified
    //    //
    //    private int _order;
    //    public PropertyOrderAttribute(int order)
    //    {
    //        _order = order;
    //    }

    //    public int Order
    //    {
    //        get
    //        {
    //            return _order;
    //        }
    //    }
    //}

    #endregion Helper Class - PropertyOrderAttribute 未用
}