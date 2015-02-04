using System;

namespace WpfMagic.Attributes
{
    /// <summary>
    /// This will tell the ViewBinder what View the attributed ViewModel should use.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class ViewAttribute : Attribute
    {
        public string Name { get; private set; }

        public ViewAttribute(string name)
        {
            this.Name = name;
        }
    }
}
