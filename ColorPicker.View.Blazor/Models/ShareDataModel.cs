namespace ColorPicker.View.Blazor.Models
{
    /// <summary>
    /// An object representing rendered data.
    /// </summary>
    internal class ShareDataModel
    {
        /// <summary>
        /// Gets or sets the latest hue value from which the result was rendered.
        /// </summary>
        public float LastHue { get; set; } = -1;

        /// <summary>
        /// Gets or sets the latest saturation value from which the result was rendered.
        /// </summary>
        public float LastSaturation { get; set; } = -1;

        /// <summary>
        /// Gets or sets the latest value (brightness) from which the result was rendered.
        /// </summary>
        public float LastValue { get; set; } = -1;

        /// <summary>
        /// Gets or sets the latest alpha value from which the result was rendered.
        /// </summary>
        public float LastAlpha { get; set; } = -1;
    }
}