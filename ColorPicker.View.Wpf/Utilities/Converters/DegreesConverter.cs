using System;
using System.Globalization;
using System.Windows.Data;

namespace ColorPicker.View.Wpf.Utilities.Converters
{
    internal class DegreesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                int rounded = (int)Math.Round(d);
                return $"{rounded}°";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsed))
                {
                    return parsed;
                }
            }

            return value;
        }
    }
}