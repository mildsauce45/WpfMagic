using System;
using System.Windows;

namespace WpfMagic.Bindings
{
    /// <summary>
    /// This binding allows us to determine what data template to use for the given type.
    /// </summary>
    internal class DataTemplateBinding
    {
        public Type Type { get; private set; }
        public string Key { get; private set; }
        public DataTemplate Template { get; private set; }

        public bool IsDefault
        {
            get { return string.IsNullOrWhiteSpace(Key); }
        }

        public DataTemplateBinding(Type type, string key, DataTemplate template)
        {
            this.Type = type;
            this.Key = key;
            this.Template = template;
        }
    }
}
