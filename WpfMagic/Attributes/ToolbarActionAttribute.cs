using System;
using WpfMagic.Contracts;

namespace WpfMagic.Attributes
{
    /// <summary>
    /// This attribute tells WpfMagic to bind the decorated ICommand property to the WPF Button class inside the default Toolbar
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public class ToolbarActionAttribute : Attribute, IControlBinding
    {
        public string Name { get; private set; }
        public Type ControlTypeOverride { get; private set; }

        public ToolbarActionAttribute()
        {
        }

        /// <summary>
        /// The given name will display on the generated Button as opposed to the camel case separated name of the attributed property.
        /// </summary>
        public ToolbarActionAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Use the supplied type as a replacement for the default WPF Button class
        /// </summary>
        public ToolbarActionAttribute(Type controlTypeOverride)
        {
            ControlTypeOverride = controlTypeOverride;
        }

        /// <summary>
        /// Use the given name for the button text and use the given type as a replacement to the WPF Button class
        /// </summary>
        public ToolbarActionAttribute(string name, Type controlTypeOverride)
        {
            this.Name = name;
            this.ControlTypeOverride = controlTypeOverride;
        }
    }
}
