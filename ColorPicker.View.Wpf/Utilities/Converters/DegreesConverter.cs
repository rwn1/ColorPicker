using System;
using System.Globalization;
using System.Windows.Data;

namespace ColorPicker.View.Wpf.Utilities.Converters
{
    internal class DegreesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float val)
            {
                int rounded = (int)Math.Round(val);
                return $"{rounded}{parameter}";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                if (float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out float parsed))
                {
                    return parsed;
                }
            }

            return value;
        }
    }
}