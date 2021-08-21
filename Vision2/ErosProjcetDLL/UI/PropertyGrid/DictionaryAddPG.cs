using System;
using System.Collections;
using System.ComponentModel;

namespace Vision2.ErosProjcetDLL.UI.PropertyGrid
{
    /// <summary>
    ///
    /// </summary>
    public class DictionaryPropertyGridAdapter : ICustomTypeDescriptor
    {
        private IDictionary _dictionary;

        public DictionaryPropertyGridAdapter(IDictionary d)
        {
            _dictionary = d;
        }

        //Three of the ICustomTypeDescriptor methods are never called by the property grid, but we'll stub them out properly anyway:

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        //Then there's a whole slew of methods that are called by PropertyGrid, but we don't need to do anything interesting in them:

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return _dictionary;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        PropertyDescriptorCollection
            System.ComponentModel.ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
        }

        //Then the interesting bit. We simply iterate over the IDictionary, creating a property descriptor for each entry:

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            ArrayList properties = new ArrayList();
            foreach (DictionaryEntry e in _dictionary)
            {
                properties.Add(new DictionaryPropertyDescriptor(_dictionary, e.Key));
            }

            PropertyDescriptor[] props =
                (PropertyDescriptor[])properties.ToArray(typeof(PropertyDescriptor));

            return new PropertyDescriptorCollection(props);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class DictionaryPropertyDescriptor : PropertyDescriptor
    {
        //PropertyDescriptor provides 3 constructors. We want the one that takes a string and an array of attributes:

        private IDictionary _dictionary;
        private object _key;

        internal DictionaryPropertyDescriptor(IDictionary d, object key)
            : base(key.ToString(), null)
        {
            _dictionary = d;
            _key = key;
        }

        //The attributes are used by PropertyGrid to organise the properties into categories, to display help text and so on. We don't bother with any of that at the moment, so we simply pass null.

        //The first interesting member is the PropertyType property. We just get the object out of the dictionary and ask it:

        public override Type PropertyType
        {
            get { return _dictionary[_key].GetType(); }
        }

        //If you knew that all of your values were strings, for example, you could just return typeof(string).

        //Then we implement SetValue and GetValue:

        public override void SetValue(object component, object value)
        {
            _dictionary[_key] = value;
        }

        public override object GetValue(object component)
        {
            return _dictionary[_key];
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type ComponentType
        {
            get { return null; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}