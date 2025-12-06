namespace ColorPicker.Core.Models
{
    /// <summary>
    /// Object representing operations with RGB color components.
    /// </summary>
    public class RgbModel : ModuleBase
    {
        private byte _red = 255;
        /// <summary>
        /// Gets or sets the red component of the RGB color.
        /// </summary>
        public byte Red
        {
            get => _red;
            set
            {
                if (_red == value) return;
                _red = value;
                NotifyAndRaiseChanged();
            }
        }

        private byte _green = 255;
        /// <summary>
        /// Gets or sets the green component of the RGB color.
        /// </summary>
        public byte Green
        {
            get => _green;
            set
            {
                if (_green == value) return;
                _green = value;
                NotifyAndRaiseChanged();
            }
        }

        private byte _blue = 255;
        /// <summary>
        /// Gets or sets the blue component of the RGB color.
        /// </summary>
        public byte Blue
        {
            get => _blue;
            set
            {
                if (_blue == value) return;
                _blue = value;
                NotifyAndRaiseChanged();
            }
        }

        /// <summary>
        /// Hub calls this to update all three RGB components at once.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        internal void SetFromHub(byte r, byte g, byte b)
        {
            SetSuppressChanged(true);

            bool redChanged = _red != r;
            bool greenChanged = _green != g;
            bool blueChanged = _blue != b;

            _red = r;
            _green = g;
            _blue = b;

            if (redChanged)
                NotifyPropertyChanged(nameof(Red));
            if (greenChanged)
                NotifyPropertyChanged(nameof(Green));
            if (blueChanged)
                NotifyPropertyChanged(nameof(Blue));

            SetSuppressChanged(false);
        }
    }
}