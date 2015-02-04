using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfMagic.Converters;

namespace WpfMagic.Attributes
{
    /// <summary>
    /// <para>
    /// Let's you control the visibility of the generated control.
    /// </para>
    /// <para>
    /// If no converter type is specified, the supplied WpfMagic converter will be used.
    /// </para>
    /// <para>
    /// If no path is specified, the value of the property is passed in by default.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public class VisibilityAttribute : Attribute
    {
        public Type ConverterType { get; private set; }
        public object ConverterParameter { get; private set; }
        public string Path { get; private set; }

        public VisibilityAttribute()
        {
            ConverterType = typeof(VisibilityConverter);
        }

        public VisibilityAttribute(Type converterType)
        {
            if (converterType == null)
                throw new ArgumentException("You must supply a type for the converter");

            this.ConverterType = converterType;
        }

        public VisibilityAttribute(string path)
            : this()
        {
            Path = path;
        }

        public VisibilityAttribute(object parameter)
            : this()
        {
            this.ConverterParameter = parameter;
        }

        public VisibilityAttribute(Type converterType, string path, object parameter)
            : this(converterType)
        {
            this.Path = path;
            this.ConverterParameter = parameter;
        }
    }
}
