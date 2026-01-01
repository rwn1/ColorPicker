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
}