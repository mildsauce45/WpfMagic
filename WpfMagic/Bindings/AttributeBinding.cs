using System;
using System.Reflection;

namespace WpfMagic.Bindings
{
    /// <summary>
    /// This binding is used internally to simplify several LINQ statements and prevent over usage of reflection
    /// </summary>    
    internal class AttributeBinding<T> where T : Attribute
    {
        public T Attr { get; private set; }
        public PropertyInfo Property { get; private set; }

        public AttributeBinding(T attr, PropertyInfo property)
        {
            this.Attr = attr;
            this.Property = property;
        }
    }
}
