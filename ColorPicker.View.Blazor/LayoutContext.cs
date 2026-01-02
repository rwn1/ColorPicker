using Microsoft.AspNetCore.Components;

public sealed class LayoutContext
{
    /// <summary>
    /// Color selection.
    /// </summary>
    public RenderFragment PART_ColorSelectionView { get; init; } = default!;

    /// <summary>
    /// Hue selection.
    /// </summary>
    public RenderFragment PART_HueSelectionView { get; init; } = default!;

    /// <summary>
    /// Alpha selection.
    /// </summary>
    public RenderFragment PART_AlphaSelectionView { get; init; } = default!;

    /// <summary>
    /// Selected color.
    /// </summary>
    public RenderFragment PART_SelectedColorView { get; init; } = default!;

    #region HEX

    /// <summary>
    /// Hex input.
    /// </summary>
    public RenderFragment PART_HexTextBox { get; init; } = default!;

    #endregion

    #region RGB

    /// <summary>
    /// Red input.
    /// </summary>
    public RenderFragment PART_RedTextBox { get; init; } = default!;
    public RenderFragment PART_RedSlider { get; init; } = default!;

    /// <summary>
    /// Green input.
    /// </summary>
    public RenderFragment PART_GreenTextBox { get; init; } = default!;
    public RenderFragment PART_GreenSlider { get; init; } = default!;

    /// <summary>
    /// Blue input.
    /// </summary>
    public RenderFragment PART_BlueTextBox { get; init; } = default!;
    public RenderFragment PART_BlueSlider { get; init; } = default!;

    #endregion

    #region Alpha

    /// <summary>
    /// Alpha input.
    /// </summary>
    public RenderFragment PART_AlphaTextBox { get; init; } = default!;
    public RenderFragment PART_AlphaSlider { get; init; } = default!;

    #endregion

    #region HSV

    /// <summary>
    /// Hue input.
    /// </summary>
    public RenderFragment PART_HueTextBox { get; init; } = default!;
    public RenderFragment PART_HueSlider { get; init; } = default!;

    /// <summary>
    /// Saturation input.
    /// </summary>
    public RenderFragment PART_SaturationTextBox { get; init; } = default!;
    public RenderFragment PART_SaturationSlider { get; init; } = default!;

    /// <summary>
    /// Value input.
    /// </summary>
    public RenderFragment PART_ValueTextBox { get; init; } = default!;
    public RenderFragment PART_ValueSlider { get; init; } = default!;

    #endregion

    #region HSL

    /// <summary>
    /// Hue input.
    /// </summary>
    public RenderFragment PART_HslHueTextBox { get; init; } = default!;
    public RenderFragment PART_HslHueSlider { get; init; } = default!;

    /// <summary>
    /// Saturation input.
    /// </summary>
    public RenderFragment PART_HslSaturationTextBox { get; init; } = default!;
    public RenderFragment PART_HslSaturationSlider { get; init; } = default!;

    /// <summary>
    /// Lightness input.
    /// </summary>
    public RenderFragment PART_HslLightnessTextBox { get; init; } = default!;
    public RenderFragment PART_HslLightnessSlider { get; init; } = default!;

    #endregion HSL

    #region CMYK

    /// <summary>
    /// Cyan input.
    /// </summary>
    public RenderFragment PART_CyanTextBox { get; init; } = default!;
    public RenderFragment PART_CyanSlider { get; init; } = default!;

    /// <summary>
    /// Magenta input.
    /// </summary>
    public RenderFragment PART_MagentaTextBox { get; init; } = default!;
    public RenderFragment PART_MagentaSlider { get; init; } = default!;

    /// <summary>
    /// Yellow input.
    /// </summary>
    public RenderFragment PART_YellowTextBox { get; init; } = default!;
    public RenderFragment PART_YellowSlider { get; init; } = default!;

    /// <summary>
    /// Key input.
    /// </summary>
    public RenderFragment PART_KeyTextBox { get; init; } = default!;
    public RenderFragment PART_KeySlider { get; init; } = default!;

    #endregion
}