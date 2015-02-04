using System;

namespace WpfMagic.Attributes
{
    /// <summary>
    /// This attributes defines either a property or class as the content of a custom area. You must provide a template for
    /// the custom area to use at runtime
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CustomAreaAttribute : Attribute
    {
        public string ContentArea { get; private set; }
        public string Template { get; private set; }

        public CustomAreaAttribute(string template)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentException("You must supply a template name for a custom area.");

            Template = template;            
        }

        public CustomAreaAttribute(string template, string contentArea)
            : this(template)
        {
            this.ContentArea = contentArea;
        }
    }
}
