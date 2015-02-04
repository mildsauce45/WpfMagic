using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace WpfMagic.Extensions
{
    internal static class BindingExtensions
    {
        public static void CreateBinding(this FrameworkElement control, string propertyName, string path, BindingMode mode = BindingMode.OneWay, IValueConverter converter = null, object converterParameter = null)
        {
            if (control == null)
                return;

            var type = control.GetType();

            var dpField = type.GetField(propertyName + "Property", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            if (dpField != null)
            {
                var dp = dpField.GetValue(null) as DependencyProperty;

                CreateBinding(control, dp, path, mode, converter, converterParameter);
            }
        }

        public static void CreateBinding(this FrameworkElement control, DependencyProperty property, string path, BindingMode mode = BindingMode.OneWay, IValueConverter converter = null, object converterParameter = null)
        {
            if (control == null || property == null)
                return;

            var binding = new Binding();

            binding.Mode = mode;
            binding.Path = new PropertyPath(path);
            binding.Converter = converter;
            binding.ConverterParameter = converterParameter;

            control.SetBinding(property, binding);
        }
    }
}
