using System;

namespace ColorPicker.Core.Models
{
    /// <summary>
    /// Object representing operations with alpha component.
    /// </summary>
    public class AlphaModel: ModuleBase
    {
        private float _alpha = 1;
        /// <summary>
        /// Gets or sets the alpha component of the color (0-1).
        /// </summary>
        public float Alpha
        {
            get => _alpha;
            set
            {
                float clamped = value < 0 ? 0 : (value > 1 ? 1 : value);
                if (Math.Abs(clamped - _alpha) < 0.0000001) return;
                _alpha = clamped;
                NotifyAndRaiseChanged();
            }
        }

        /// <summary>
        /// Hub sets alpha from HEX. This writes and raises PropertyChanged but suppresses Changed.
        /// </summary>
        internal void SetFromHub(float alpha)
        {
            SetSuppressChanged(true);

            if (_alpha != alpha)
            {
                float clamped = alpha < 0 ? 0 : (alpha > 1 ? 1 : alpha);
                if (Math.Abs(clamped - _alpha) > 0.0000001)
                {
                    _alpha = clamped;
                    NotifyPropertyChanged(nameof(Alpha));
                }
            }

            SetSuppressChanged(false);
        }
    }
}