using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ColorPicker.View.Wpf.Utilities
{
    internal static class FocusFormatBehavior
    {
        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.RegisterAttached(
                "Format",
                typeof(string),
                typeof(FocusFormatBehavior),
                new PropertyMetadata(null, OnFormatChanged));

        public static void SetFormat(DependencyObject element, string value)
            => element.SetValue(FormatProperty, value);

        public static string GetFormat(DependencyObject element)
            => (string)element.GetValue(FormatProperty);

        private static void OnFormatChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tb)
            {
                tb.GotKeyboardFocus -= OnGotFocus;
                tb.LostKeyboardFocus -= OnLostFocus;

                if (e.NewValue != null)
                {
                    tb.GotKeyboardFocus += OnGotFocus;
                    tb.LostKeyboardFocus += OnLostFocus;
                }
            }
        }

        private static void OnGotFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            var format = GetFormat(tb);
            if (string.IsNullOrWhiteSpace(format)) return;

            var suffix = ExtractSuffix(format);

            if (!string.IsNullOrEmpty(suffix) && tb.Text.EndsWith(suffix))
                tb.Text = tb.Text.Substring(0, tb.Text.Length - suffix.Length).Trim();

            tb.CaretIndex = tb.Text.Length;
        }

        private static void OnLostFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            var format = GetFormat(tb);

            if (string.IsNullOrWhiteSpace(format))
                return;

            var be = BindingOperations.GetBindingExpression(tb, TextBox.TextProperty);
            be?.UpdateSource();

            be?.UpdateTarget();

            if (int.TryParse(tb.Text, out var value))
            {
                tb.Text = string.Format(CultureInfo.CurrentCulture, format, value);
            }
        }

        private static string ExtractSuffix(string format)
        {
            var idx = format.IndexOf("{0}");
            if (idx < 0) return string.Empty;
            return format.Substring(idx + 3);
        }
    }
}