using System;
using System.Globalization;
using System.Windows.Data;

namespace ColorPicker.View.Wpf.Utilities.Converters
{
    public class PercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float d)
            {
                int rounded = (int)Math.Round(d * 100.0);
                return $"{rounded}{parameter}";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                if (float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out float percent))
                {
                    return percent / 100.0;
                }
            }
            else if (value is double d)
            {
                return d / 100.0;
            }
            else if (value is float f)
            {
                return f / 100.0;
            }

            return value;
        }
    }
}