using ColorPicker.Core.Models;

/// <summary>
/// Main view model for binding to color models.
/// </summary>
public class ColorPickerViewModel : ObservableObject
{
    /// <summary>
    /// Hub for calculations between models.
    /// </summary>
    private readonly ColorSyncHub _hub;

    /// <summary>
    /// Model for RGB components.
    /// </summary>
    public RgbModel Rgb { get; private set; }

    /// <summary>
    /// Model for HSV components.
    /// </summary>
    public HsvModel Hsv { get; private set; }

    /// <summary>
    /// Model for HSL components.
    /// </summary>
    public HslModel Hsl { get; private set; }

    /// <summary>
    /// Model for CMYK components.
    /// </summary>
    public CmykModel Cmyk { get; private set; }

    /// <summary>
    /// Model for HEX value.
    /// </summary>
    public HexModel Hex { get; private set; }

    /// <summary>
    /// Model for Alpha value.
    /// </summary>
    public AlphaModel Alpha { get; private set; }

    private bool _enableHsl = false;
    /// <summary>
    /// Gets or sets information about whether HSL recalculation should occur.
    /// </summary>
    public bool EnableHsl
    {
        get => _enableHsl;
        set
        {
            if (_enableHsl == value) return;
            _enableHsl = value;
            _hub.EnableHsl = value;
            NotifyPropertyChanged();
        }
    }

    private bool _enableCmyk = false;
    /// <summary>
    /// Gets or sets information about whether CMYK recalculation should occur.
    /// </summary>
    public bool EnableCmyk
    {
        get => _enableCmyk;
        set
        {
            if (_enableCmyk == value) return;
            _enableCmyk = value;
            _hub.EnableCmyk = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Initializes a new instance of the ColorPickerViewModel class.
    /// </summary>
    public ColorPickerViewModel()
    {
        _hub = new ColorSyncHub();

        Rgb = _hub.Rgb;
        Hsv = _hub.Hsv;
        Hsl = _hub.Hsl;
        Cmyk = _hub.Cmyk;
        Hex = _hub.Hex;
        Alpha = _hub.Alpha;
    }

    /// <summary>
    /// Select the new colot.
    /// </summary>
    /// <param name="r">Red component.</param>
    /// <param name="g">Green component.</param>
    /// <param name="b">Blue component.</param>
    /// <param name="alpha">Alpha component.</param>
    public void SelectColor(byte r, byte g, byte b, float alpha = 1.0f)
    {
        _hub.SetColor(r, g, b, alpha);
        NotifyPropertyChanged(nameof(Alpha));
    }

    /// <summary>
    /// Select the new colot.
    /// </summary>
    /// <param name="hue">Hue component.</param>
    /// <param name="saturation">Saturation component.</param>
    /// <param name="value">Value (brightness) component.</param>
    /// <param name="alpha">Alpha component.</param>
    public void SelectColor(float hue, float saturation, float value, float alpha = 1.0f)
    {
        _hub.SetColor(hue, saturation, value, alpha);
        NotifyPropertyChanged(nameof(Alpha));
    }
}