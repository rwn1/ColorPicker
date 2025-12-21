using System;

namespace ColorPicker.Core.Models
{
    /// <summary>
    /// Object representing operations with CMYK color components.
    /// </summary>
    public class CmykModel : ModuleBase
    {
        private float _cyan = 0;
        /// <summary>
        /// Gets or sets the cyan component of the CMYK color (0-1).
        /// </summary>
        public float Cyan
        {
            get => _cyan;
            set
            {
                float v = value < 0 ? 0 : (value > 1 ? 1 : value);
                if (Math.Abs(v - _cyan) < 0.0000001) return;
                _cyan = v;
                NotifyAndRaiseChanged();
            }
        }

        private float _magenta = 0;
        /// <summary>
        /// Gets or sets the magenta component of the CMYK color (0-1).
        /// </summary>
        public float Magenta
        {
            get => _magenta;
            set
            {
                float v = value < 0 ? 0 : (value > 1 ? 1 : value);
                if (Math.Abs(v - _magenta) < 0.0000001) return;
                _magenta = v;
                NotifyAndRaiseChanged();
            }
        }

        private float _yellow = 0;
        /// <summary>
        /// Gets or sets the yellow component of the CMYK color (0-1).
        /// </summary>
        public float Yellow
        {
            get => _yellow;
            set
            {
                float v = value < 0 ? 0 : (value > 1 ? 1 : value);
                if (Math.Abs(v - _yellow) < 0.0000001) return;
                _yellow = v;
                NotifyAndRaiseChanged();
            }
        }

        private float _key = 0;
        /// <summary>
        /// Gets or sets the key (black) component of the CMYK color (0-1).
        /// </summary>
        public float Key
        {
            get => _key;
            set
            {
                float v = value < 0 ? 0 : (value > 1 ? 1 : value);
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
        internal void SetFromHub(float c, float m, float y, float k)
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
            float rd = r / 255.0f;
            float gd = g / 255.0f;
            float bd = b / 255.0f;

            float k = 1.0f - Math.Max(rd, Math.Max(gd, bd));
            float c = 0.0f, m = 0.0f, y = 0.0f;
            if (Math.Abs(1.0 - k) > 1e-9)
            {
                c = (1.0f - rd - k) / (1.0f - k);
                m = (1.0f - gd - k) / (1.0f - k);
                y = (1.0f - bd - k) / (1.0f - k);
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
            float red = (1.0f - _cyan) * (1.0f - _key);
            float green = (1.0f - _magenta) * (1.0f - _key);
            float blue = (1.0f - _yellow) * (1.0f - _key);
            r = (byte)Math.Round(Clamp01(red) * 255.0);
            g = (byte)Math.Round(Clamp01(green) * 255.0);
            b = (byte)Math.Round(Clamp01(blue) * 255.0);
        }

        /// <summary>
        /// Returns the clamp value to the 0–1 range.
        /// </summary>
        /// <param name="v">Value to clamp.</param>
        /// <returns>The clamp value to the 0–1 range.</returns>
        private static float Clamp01(float v)
        {
            if (v < 0) return 0;
            if (v > 1) return 1;
            return v;
        }
    }
}