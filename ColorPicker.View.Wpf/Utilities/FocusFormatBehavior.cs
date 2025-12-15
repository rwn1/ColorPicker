using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ColorPicker.View.Wpf.Utilities
{
    internal static class FocusFormatBehavior
    {
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.RegisterAttached(
                "Unit",
                typeof(string),
                typeof(FocusFormatBehavior),
                new PropertyMetadata(null, OnUnitChanged));

        public static void SetUnit(DependencyObject element, string value)
            => element.SetValue(UnitProperty, value);

        public static string GetUnit(DependencyObject element)
            => (string)element.GetValue(UnitProperty);

        private static void OnUnitChanged(
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
            var unit = GetUnit(tb);
            if (string.IsNullOrWhiteSpace(unit)) return;

            if (!string.IsNullOrEmpty(unit) && tb.Text.EndsWith(unit))
                tb.Text = tb.Text.Substring(0, tb.Text.Length - unit.Length).Trim();

            tb.CaretIndex = tb.Text.Length;
        }

        private static void OnLostFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            var unit = GetUnit(tb);

            if (string.IsNullOrWhiteSpace(unit))
                return;

            var be = BindingOperations.GetBindingExpression(tb, TextBox.TextProperty);
            be?.UpdateSource();

            if (int.TryParse(tb.Text, out var value))
            {
                tb.Text = string.Format(CultureInfo.CurrentCulture, "{0}" + unit, value);
            }
        }
    }
}