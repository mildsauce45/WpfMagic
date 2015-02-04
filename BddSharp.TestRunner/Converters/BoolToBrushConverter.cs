using System;
using System.Windows.Data;
using System.Windows.Media;

namespace BddSharp.TestRunner.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public Brush TrueBrush { get; set; }
        public Brush FalseBrush { get; set; }
        public Brush NullBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool) || !(value is bool?))
                return NullBrush;

            var b = (bool?)value;

            return !b.HasValue ? NullBrush : (b.Value ? TrueBrush : FalseBrush);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
