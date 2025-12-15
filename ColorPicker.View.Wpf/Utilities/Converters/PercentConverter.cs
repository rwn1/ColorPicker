using System;
using System.Globalization;
using System.Windows.Data;

namespace ColorPicker.View.Wpf.Utilities.Converters
{
    public class PercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                int rounded = (int)Math.Round(d * 100.0);
                return $"{rounded}%";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double percent))
                {
                    return percent / 100.0;
                }
            }
            else if (value is double d)
            {
                return d / 100.0;
            }

            return value;
        }
    }
}