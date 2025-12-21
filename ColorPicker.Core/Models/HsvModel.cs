using ColorPicker.Core.Utilities;
using System;

namespace ColorPicker.Core.Models
{
    /// <summary>
    /// Object representing operations with HSV color components.
    /// </summary>
    public class HsvModel : ModuleBase
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

        private float _value = 1;
        /// <summary>
        /// Gets or sets the value (brightness) component of the HSV color (0-1).
        /// </summary>
        public float Value
        {
            get => _value;
            set
            {
                float clamped = value < 0 ? 0 : (value > 1 ? 1 : value);
                if (Math.Abs(clamped - _value) < 0.0000001) return;
                _value = clamped;
                NotifyAndRaiseChanged();
            }
        }

        /// <summary>
        /// Hub calls this to update all three components HSV at once.
        /// </summary>
        /// <param name="h">Hue component.</param>
        /// <param name="s">Saturation component.</param>
        /// <param name="v">Value (brightness) component.</param>
        internal void SetFromHub(float h, float s, float v)
        {
            SetSuppressChanged(true);

            bool hueChanged = _hue != h;
            bool saturationChanged = _saturation != s;
            bool valueChanged = _value != v;

            _hue = h;
            _saturation = s;
            _value = v;

            if (hueChanged)
                NotifyPropertyChanged(nameof(Hue));
            if (saturationChanged)
                NotifyPropertyChanged(nameof(Saturation));
            if (valueChanged)
                NotifyPropertyChanged(nameof(Value));

            SetSuppressChanged(false);
        }

        /// <summary>
        /// Converts current HSV values to an RGB color.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        internal void ToRgb(out byte r, out byte g, out byte b)
        {
            ColorConversions.HsvToRgb(_hue, _saturation, _value, out r, out g, out b);
        }

        /// <summary>
        /// Updates HSV values from an RGB input.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        internal void FromRgb(byte r, byte g, byte b)
        {
            ColorConversions.RgbToHsv(r, g, b, out float hh, out float ss, out float vv);
            SetFromHub(hh, ss, vv);
        }
    }
}