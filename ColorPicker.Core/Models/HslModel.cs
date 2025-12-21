using ColorPicker.Core.Utilities;
using System;

namespace ColorPicker.Core.Models
{
    /// <summary>
    /// Object representing operations with HSL color components.
    /// </summary>
    public class HslModel : ModuleBase
    {
        private float _hue = 0;
        /// <summary>
        /// Gets or sets the hue component of the HSV color (0-360 degrees).
        /// </summary>
        public float Hue
        {
            get => _hue;
            set
            {
                float clamped = value;
                if (clamped < 0) clamped = 0;
                if (clamped > 360) clamped = 360;
                if (Math.Abs(clamped - _hue) < 0.00001) return;
                _hue = clamped;
                NotifyAndRaiseChanged();
            }
        }

        private float _saturation = 0;
        /// <summary>
        /// Gets or sets the saturation component of the HSV color (0-1).
        /// </summary>
        public float Saturation
        {
            get => _saturation;
            set
            {
                float clamped = value < 0 ? 0 : (value > 1 ? 1 : value);
                if (Math.Abs(clamped - _saturation) < 0.0000001) return;
                _saturation = clamped;
                NotifyAndRaiseChanged();
            }
        }

        private float _lightness = 1;
        /// <summary>
        /// Gets or sets the lightness component of the HSV color (0-1).
        /// </summary>
        public float Lightness
        {
            get => _lightness;
            set
            {
                float clamped = value < 0 ? 0 : (value > 1 ? 1 : value);
                if (Math.Abs(clamped - _lightness) < 0.0000001) return;
                _lightness = clamped;
                NotifyAndRaiseChanged();
            }
        }

        /// <summary>
        /// Hub calls this to update all three components HSV at once.
        /// </summary>
        /// <param name="h">Hue component.</param>
        /// <param name="s">Saturation component.</param>
        /// <param name="l">Lightness component.</param>
        internal void SetFromHub(float h, float s, float l)
        {
            SetSuppressChanged(true);

            bool hueChanged = _hue != h;
            bool saturationChanged = _saturation != s;
            bool lightnessChanged = _lightness != l;

            _hue = h;
            _saturation = s;
            _lightness = l;

            if (hueChanged)
                NotifyPropertyChanged(nameof(Hue));
            if (saturationChanged)
                NotifyPropertyChanged(nameof(Saturation));
            if (lightnessChanged)
                NotifyPropertyChanged(nameof(Lightness));

            SetSuppressChanged(false);
        }

        /// <summary>
        /// Converts current HSL values to an RGB color.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        internal void ToRgb(out byte r, out byte g, out byte b)
        {
            ColorConversions.HslToRgb(_hue, _saturation, _lightness, out r, out g, out b);
        }

        /// <summary>
        /// Updates HSL values from an RGB input.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        internal void FromRgb(byte r, byte g, byte b)
        {
            ColorConversions.RgbToHsl(r, g, b, out float hh, out float ss, out float ll);
            SetFromHub(hh, ss, ll);
        }
    }
}