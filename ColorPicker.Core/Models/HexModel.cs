using System.Globalization;
using System.Text.RegularExpressions;

namespace ColorPicker.Core.Models
{
    /// <summary>
    /// Object representing operations with hexadecimal format.
    /// </summary>
    public class HexModel : ModuleBase
    {
        private string _hex = "#FFFFFFFF";
        /// <summary>
        /// Gets or sets the color in hexadecimal (#RRGGBB or #AARRGGBB).
        /// </summary>
        public string Hex
        {
            get => _hex;
            set
            {
                if (value == null) return;
                if (_hex == value) return;
                // Accept only #RRGGBB or #AARRGGBB
                if (!Regex.IsMatch(value, "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$")) return;
                _hex = value;
                NotifyAndRaiseChanged();
            }
        }

        /// <summary>
        /// Hub sets hex converted from RGBA. This writes and raises PropertyChanged but suppresses Changed.
        /// </summary>
        internal void SetFromHub(string hex)
        {
            SetSuppressChanged(true);

            if (_hex != hex && Regex.IsMatch(hex, "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$")) 
            {
                _hex = hex;
                NotifyPropertyChanged(nameof(Hex));
            }

            SetSuppressChanged(false);
        }

        /// <summary>
        /// Convert hex to rgba bytes (alpha in 0..1).
        /// Accepts #RRGGBB or #AARRGGBB
        /// </summary>
        internal void ToRgba(out byte r, out byte g, out byte b, out double a)
        {
            string h = _hex;
            if (h.Length == 7)
                h = h.Insert(1, "FF");

            a = int.Parse(h.Substring(1, 2), NumberStyles.HexNumber) / 255.0;
            r = byte.Parse(h.Substring(3, 2), NumberStyles.HexNumber);
            g = byte.Parse(h.Substring(5, 2), NumberStyles.HexNumber);
            b = byte.Parse(h.Substring(7, 2), NumberStyles.HexNumber);
        }
    }
}