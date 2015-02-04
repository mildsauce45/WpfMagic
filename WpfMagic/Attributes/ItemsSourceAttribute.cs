using System;
using WpfMagic.Contracts;

namespace WpfMagic.Attributes
{
    /// <summary>
    /// This property will tell any ItemContainer's in the view to generate an ItemsControl and use the proper template.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public class ItemsSourceAttribute : Attribute, IControlBinding
    {
        public string Template { get; private set; }

        /// <summary>
        /// Use the default template of the underlying type for the ItemTemplate of the ItemsControl
        /// </summary>
        public ItemsSourceAttribute()
        {
        }

        /// <summary>
        /// Use the named template of the underlying type for the ItemTemplate of the ItemsControl
        /// </summary>        
        public ItemsSourceAttribute(string template)
        {
            this.Template = template;
        }
    }
}
