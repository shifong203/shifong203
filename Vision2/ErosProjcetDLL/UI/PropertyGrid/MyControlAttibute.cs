using System;

namespace Vision2.ErosProjcetDLL.UI.PropertyGrid
{/// <summary>
/// 自定义标签类
/// </summary>
    public class MyControlAttibute : Attribute
    {
        private string _PropertyName;
        private string _PropertyDescription;
        private object _DefaultValue;

        /// <summary>
        ///
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <param name="DefalutValue"></param>
        public MyControlAttibute(string Name, string Description, object DefalutValue)
        {
            this._PropertyName = Name;
            this._PropertyDescription = Description;
            this._DefaultValue = DefalutValue;
        }

        public MyControlAttibute(string Name, string Description)
        {
            this._PropertyName = Name;
            this._PropertyDescription = Description;
            this._DefaultValue = "";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Name"></param>
        public MyControlAttibute(string Name)
        {
            this._PropertyName = Name;
            this._PropertyDescription = "";
            this._DefaultValue = "";
        }

        public string PropertyName
        {
            get { return this._PropertyName; }
        }

        public string PropertyDescription
        {
            get { return this._PropertyDescription; }
        }

        public object DefaultValue
        {
            get { return this._DefaultValue; }
        }
    }
}