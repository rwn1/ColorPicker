using ColorPicker.Core.Utilities;
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
        /// Updates CMYK values from an RGB input.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        internal void FromRgb(byte r, byte g, byte b)
        {
            ColorConversions.RgbToCmyk(r, g, b, out float cc, out float mm, out float yy, out float kk);
            SetFromHub(cc, mm, yy, kk);
        }

        /// <summary>
        /// Updates CMYK values from an HSV input.
        /// </summary>
        /// <param name="h">Hue component.</param>
        /// <param name="s">Saturation component.</param>
        /// <param name="v">Value (brightness) component.</param>
        internal void FromHsv(float h, float s, float v)
        {
            ColorConversions.HsvToRgb(h, s, v, out byte rr, out byte gg, out byte bb);
            ColorConversions.RgbToCmyk(rr, gg, bb, out float cc, out float mm, out float yy, out float kk);
            SetFromHub(cc, mm, yy, kk);
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