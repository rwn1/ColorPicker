using System;

namespace ColorPicker.Core.Models
{
    /// <summary>
    /// Object representing operations with CMYK color components.
    /// </summary>
    public class CmykModel : ModuleBase
    {
        private double _cyan = 0;
        /// <summary>
        /// Gets or sets the cyan component of the CMYK color (0-1).
        /// </summary>
        public double Cyan
        {
            get => _cyan;
            set
            {
                double v = value < 0 ? 0 : (value > 1 ? 1 : value);
                if (Math.Abs(v - _cyan) < 0.0000001) return;
                _cyan = v;
                NotifyAndRaiseChanged();
            }
        }

        private double _magenta = 0;
        /// <summary>
        /// Gets or sets the magenta component of the CMYK color (0-1).
        /// </summary>
        public double Magenta
        {
            get => _magenta;
            set
            {
                double v = value < 0 ? 0 : (value > 1 ? 1 : value);
                if (Math.Abs(v - _magenta) < 0.0000001) return;
                _magenta = v;
                NotifyAndRaiseChanged();
            }
        }

        private double _yellow = 0;
        /// <summary>
        /// Gets or sets the yellow component of the CMYK color (0-1).
        /// </summary>
        public double Yellow
        {
            get => _yellow;
            set
            {
                double v = value < 0 ? 0 : (value > 1 ? 1 : value);
                if (Math.Abs(v - _yellow) < 0.0000001) return;
                _yellow = v;
                NotifyAndRaiseChanged();
            }
        }

        private double _key = 0;
        /// <summary>
        /// Gets or sets the key (black) component of the CMYK color (0-1).
        /// </summary>
        public double Key
        {
            get => _key;
            set
            {
                double v = value < 0 ? 0 : (value > 1 ? 1 : value);
                if (Math.Abs(v - _key) < 0.0000001) return;
                _key = v;
                NotifyAndRaiseChanged();
            }
        }

        /// <summary>
        /// Hub calls this to update all three components CMYK at once.
        /// </summary>
        /// <param name="c">Cyan component.</param>
        /// <param name="m">Magenta component.</param>
        /// <param name="y">Yellow component.</param>
        /// <param name="k">Key (black) component.</param>
        internal void SetFromHub(double c, double m, double y, double k)
        {
            SetSuppressChanged(true);

            bool cyanChanged = _cyan != c;
            bool magentaChanged = _magenta != m;
            bool yellowChanged = _yellow != y;
            bool keyChanged = _key != k;

            _cyan = c;
            _magenta = m;
            _yellow = y;
            _key = k;

            if (cyanChanged)
                NotifyPropertyChanged(nameof(Cyan));
            if (magentaChanged)
                NotifyPropertyChanged(nameof(Magenta));
            if (yellowChanged)
                NotifyPropertyChanged(nameof(Yellow));
            if (keyChanged)
                NotifyPropertyChanged(nameof(Key));

            SetSuppressChanged(false);
        }

        /// <summary>
        /// Updates SMYK values from an RGB input.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        internal void FromRgb(byte r, byte g, byte b)
        {
            double rd = r / 255.0;
            double gd = g / 255.0;
            double bd = b / 255.0;

            double k = 1.0 - Math.Max(rd, Math.Max(gd, bd));
            double c = 0.0, m = 0.0, y = 0.0;
            if (Math.Abs(1.0 - k) > 1e-9)
            {
                c = (1.0 - rd - k) / (1.0 - k);
                m = (1.0 - gd - k) / (1.0 - k);
                y = (1.0 - bd - k) / (1.0 - k);
            }
            else
            {
                c = 0; m = 0; y = 0;
            }

            SetFromHub(Clamp01(c), Clamp01(m), Clamp01(y), Clamp01(k));
        }

        /// <summary>
        /// Converts current CMYK values to an RGB color.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        internal void ToRgb(out byte r, out byte g, out byte b)
        {
            // Convert CMYK to RGB: R = 255 * (1 - C) * (1 - K)
            double red = (1.0 - _cyan) * (1.0 - _key);
            double green = (1.0 - _magenta) * (1.0 - _key);
            double blue = (1.0 - _yellow) * (1.0 - _key);
            r = (byte)Math.Round(Clamp01(red) * 255.0);
            g = (byte)Math.Round(Clamp01(green) * 255.0);
            b = (byte)Math.Round(Clamp01(blue) * 255.0);
        }

        /// <summary>
        /// Returns the clamp value to the 0–1 range.
        /// </summary>
        /// <param name="v">Value to clamp.</param>
        /// <returns>The clamp value to the 0–1 range.</returns>
        private static double Clamp01(double v)
        {
            if (v < 0) return 0;
            if (v > 1) return 1;
            return v;
        }
    }
}