using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Data;

using System.Xml.Serialization;
using System.Windows.Forms;

using System.Collections;

namespace Vision2.ErosProjcetDLL.UI.PropertyGrid
{
    public class ControlEditorTypeDescriptionProvider : TypeDescriptionProvider
    {
        protected TypeDescriptionProvider _baseProvider;
        private PropertyDescriptorCollection _propCache;
        protected Dictionary<Type, Type> dictEdtor;
        /// <summary>
        ///  属性Editor字典
        ///  Key:Attribute Value:Editor
        /// </summary>
        public Dictionary<Type, Type> EdtorDictionary
        {
            get
            {
                return dictEdtor;
            }
        }
        public ControlEditorTypeDescriptionProvider()
            : base()
        {
            dictEdtor = new Dictionary<Type, Type>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t">要修改属性的基类</param>
        public ControlEditorTypeDescriptionProvider(Type t)
            : this()
        {
            _baseProvider = TypeDescriptor.GetProvider(t);
        }


        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            PropertiesTypeDescriptor typeDes = new PropertiesTypeDescriptor(_baseProvider.GetTypeDescriptor(objectType, instance), this, objectType);
            return typeDes;
        }


    }
    /// <summary>
    /// 属性集合的描述
    /// </summary>
    public class PropertiesTypeDescriptor : CustomTypeDescriptor
    {
        private Type objType;
        private DataTable dtConfig = new DataTable();
        private ControlEditorTypeDescriptionProvider provider;
        public PropertiesTypeDescriptor(ICustomTypeDescriptor descriptor, ControlEditorTypeDescriptionProvider provider, Type objType)
            : base(descriptor)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            if (descriptor == null)
            {
                throw new ArgumentNullException("descriptor");
            }
            if (objType == null)
            {
                throw new ArgumentNullException("objectType");
            }

            this.objType = objType;
            this.provider = provider;
        }
        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            try
            {
                string nowIndexCode = "";
                Type editor;
                Type editorControl;
                Attribute abEvent;
                EventPropertyDescriptor des;
                bool showPageSize = true;

                //从dll中读取配置
                //Dictionary<string, ControlEditorType> dictConfig = PubFunc.GetPropertyEditor();
                ArrayList orderedProperties = new ArrayList();
                foreach (PropertyDescriptor prop in base.GetProperties(attributes))
                {
                    #region  控件、表单部分属性屏蔽
                    //屏蔽所有控件数据Category 数据 属性
                    if (((System.ComponentModel.MemberDescriptor)(prop)).Category == "数据" || ((System.ComponentModel.MemberDescriptor)(prop)).Category == "Data")
                    {
                        continue;
                    }

                    //屏蔽所有控件数据Category 杂项 属性
                    if (((System.ComponentModel.MemberDescriptor)(prop)).Category == "杂项" || ((System.ComponentModel.MemberDescriptor)(prop)).Category == "Misc")
                    {
                        continue;
                    }

                    #endregion

                    #region 屏蔽不需要显示的属性
                    switch (((System.ComponentModel.MemberDescriptor)(prop)).Name)
                    {
                        //case "DisplayStyleEnum":
                        //    CommonHelpDisplayStyleEnum displayType = (CommonHelpDisplayStyleEnum)prop.GetValue(this);
                        //    if (displayType == CommonHelpDisplayStyleEnum.TreeStyle)
                        //    {
                        //        showPageSize = false;
                        //    }
                        //    break;
                        case "PageSize":
                            if (!showPageSize)
                            {
                                continue;
                            }
                            break;
                    }
                    #endregion

                    abEvent = prop.Attributes[typeof(PropertyIndexAttribute)];
                    if (abEvent != null && abEvent is PropertyIndexAttribute)
                    {
                        nowIndexCode = ((PropertyIndexAttribute)abEvent).IndexCode;

                        #region 事件编辑器处理

                        if (nowIndexCode.Length > 6)
                        {
                            //最后三位000标识 不带编辑器
                            if (nowIndexCode.Substring(6, 3) == "000")
                            {
                                orderedProperties.Add(new PropertyOrderPair(prop.DisplayName, int.Parse(nowIndexCode), prop));
                                continue;
                            }
                            //foreach (var cet in dictConfig)
                            //{
                            //    if (cet.Key == nowIndexCode.Substring(6, 3))
                            //    {
                            //        //根据配置文件的序列号，获取出全名HC.Test.Designer.UI.EventActionEditorControl，放入下面的函数中                          
                            //        //editorControl = ReflectionActivator.GetType("HC.Test.Designer.UI", cet.Value.EditorName);
                            //        //if (editorControl == null)
                            //        //{
                            //        //    orderedProperties.Add(new PropertyOrderPair(prop.DisplayName, int.Parse(nowIndexCode), prop));
                            //        //    break;
                            //        //}
                            //        //if (cet.Value.EditorType == "CommonTypeDropDownEditor")
                            //        //{
                            //        //    editor = ReflectionActivator.GetGenericType("HC.Test.Common.Design", "HC.Test.Common.Design.GenericDropDownControlEditor`1", new Type[] { editorControl });
                            //        //}
                            //        //else
                            //        //{
                            //        //    editor = ReflectionActivator.GetGenericType("HC.Test.Common.Design", "HC.Test.Common.Design.ModalFormEditor`1", new Type[] { editorControl });
                            //        //}
                            //        //des = new EventPropertyDescriptor(prop, editor);
                            //        orderedProperties.Add(new PropertyOrderPair(prop.DisplayName, int.Parse(nowIndexCode), des));
                            //        break;
                            //    }
                            //}
                        }
                        #endregion

                        else
                        {
                            orderedProperties.Add(new PropertyOrderPair(prop.DisplayName, 0, prop));
                            continue;
                        }
                    }
                    else
                    {
                        orderedProperties.Add(new PropertyOrderPair(prop.DisplayName, 0, prop));
                    }
                }
                //属性集合按照PropertyIndexAttribute及DisplayName排序
                orderedProperties.Sort();
                PropertyDescriptorCollection propsTemp = new PropertyDescriptorCollection(null);
                foreach (PropertyOrderPair pop in orderedProperties)
                {
                    propsTemp.Add(pop.Property);
                }
                return propsTemp;

                //ArrayList propertyNames = new ArrayList();
                //foreach (PropertyOrderPair pop in orderedProperties)
                //{
                //    propertyNames.Add(pop.Name);
                //}
                //return pdc.Sort((string[])propertyNames.ToArray(typeof(string)));
            }
            catch
            {
                throw;
            }
        }
    }
    /// <summary>
    /// 属性 描述（属性的属性）
    /// </summary>
    public class EventPropertyDescriptor : PropertyDescriptor
    {
        Type editor = null;
        PropertyDescriptor prop;
        public EventPropertyDescriptor(PropertyDescriptor descr, Type editor)
            : base(descr)
        {
            this.prop = descr;
            this.editor = editor;
        }
        ///// <summary>
        ///// 获取Editor;
        ///// </summary>
        ///// <param name="editorBaseType"></param>
        ///// <returns></returns>
        //public override object GetEditor(Type editorBaseType)
        //{
        //    object obj = base.GetEditor(editorBaseType);
        //    if (obj == null)
        //    {
        //        obj = ReflectionActivator.CreateInstace(editor);
        //    }
        //    return obj;
        //}

        public override bool CanResetValue(object component)
        {
            return prop.CanResetValue(component);
        }

        public override Type ComponentType
        {
            get { return prop.ComponentType; }
        }

        public override object GetValue(object component)
        {
            return prop.GetValue(component);
        }

        public override bool IsReadOnly
        {
            get { return prop.IsReadOnly; }
        }

        public override Type PropertyType
        {
            get { return prop.PropertyType; }
        }
        public override void ResetValue(object component)
        {
            prop.ResetValue(component);
        }
        public override void SetValue(object component, object value)
        {
            prop.SetValue(component, value);
        }
        public override bool ShouldSerializeValue(object component)
        {
            return prop.ShouldSerializeValue(component);
        }
    }

}