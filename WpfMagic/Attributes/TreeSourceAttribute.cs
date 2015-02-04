using System;
using WpfMagic.Contracts;

namespace WpfMagic.Attributes
{
    /// <summary>
    /// This property will tell any ItemContainer's in the view to generate a TreeView and use the proper template.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public class TreeSourceAttribute : Attribute, IControlBinding
    {
        public string ChildProperty { get; private set; }

        public string ParentTemplateOverride { get; private set; }
        public string ChildTemplateOverride { get; private set; }

        /// <summary>
        /// Use the given property as the source of the children. Use the default template for the parent template.
        /// </summary>
        /// <param name="childProperty">The name of the child property for the underlying type of the decorated generic collection</param>
        public TreeSourceAttribute(string childProperty)
        {
            if (string.IsNullOrWhiteSpace(childProperty))
                throw new ArgumentException("You must supply the name of the property that holds the children");

            this.ChildProperty = childProperty;
        }

        /// <summary>
        /// Use the given property as the source of the children. Use the named template for the parent items
        /// </summary>
        /// <param name="childProperty">The name of the child property for the underlying type of the decorated generic collection</param>
        /// <param name="parentTemplateOverride">The name of the template to use as the parent item template</param>
        public TreeSourceAttribute(string childProperty, string parentTemplateOverride)
            : this(childProperty)
        {
            this.ParentTemplateOverride = parentTemplateOverride;
        }

        /// <summary>
        /// Use the given property as the source of the children. Use the named templates for the parent and child items.
        /// <remarks>This does not work yet.</remarks>
        /// </summary>
        /// <param name="childProperty">The name of the child property for the underlying type of the decorated generic collection</param>
        /// <param name="parentTemplateOverride">The name of the template to use as the parent item template</param>
        /// <param name="childTemplateOverride">The name of the template to use as the child item template</param>
        public TreeSourceAttribute(string childProperty, string parentTemplateOverride, string childTemplateOverride)
            : this(childProperty, parentTemplateOverride)
        {
            this.ChildTemplateOverride = childTemplateOverride;
        }
    }
}
