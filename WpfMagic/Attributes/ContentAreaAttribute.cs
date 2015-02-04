using System;

namespace WpfMagic.Attributes
{
    /// <summary>
    /// <para>
    /// This attribute allows you to alter the content area that a particular control binding targets.
    /// A null or only whitespace ContentArea property will place the control binding in the default area for it (one that has no name),
    /// otherwise it will look for the content area that has the matching name and place it there.
    /// </para>
    /// <para>
    /// On it's own, this attribute does nothing, it must be paired with an attribute that implements the IControlBinding interface or the
    /// CustomArea attribute
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple=true)]
    public class ContentAreaAttribute : Attribute
    {
        public string ContentArea { get; private set; }

        public ContentAreaAttribute()
        {
        }

        public ContentAreaAttribute(string contentArea)
        {
            ContentArea = contentArea;
        }
    }
}
