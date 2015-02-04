using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WpfMagic.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = Visibility.Collapsed;

            if (value == null)
                return result;

            if (value is bool)
                result = ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
            else if (value is string)
                result = !string.IsNullOrWhiteSpace(value as string) ? Visibility.Visible : Visibility.Collapsed;
            else if (value is IEnumerable)
                result = (value as IEnumerable).OfType<object>().Count() > 0 ? Visibility.Visible : Visibility.Collapsed;

            if (parameter != null && parameter is string)
            {
                if (parameter.ToString().ToLower() == Constants.ConverterParameters.INVERT)
                    result = result == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
