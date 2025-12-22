using SkiaSharp;

namespace ColorPicker.View.Maui.Components;

/// <summary>
/// Main component for displaying the selected color.
/// </summary>
public class SelectedColorView : Grid, IDisposable
{
    /// <summary>
    /// Rendering the background.
    /// </summary>
    private readonly SquareBackground _squareBackground;

    /// <summary>
    /// Rendering the foreground.
    /// </summary>
    private readonly SelectedColorForeground _selectedColorForeground;

    /// <summary>
    /// Initializes a new instance of the SelectedColorView class.
    /// </summary>
    public SelectedColorView()
    {
        _squareBackground = new SquareBackground();
        _selectedColorForeground = new SelectedColorForeground();
        
        Children.Add(_squareBackground);
        Children.Add(_selectedColorForeground);
    }

    /// <summary>
    /// Sets the fill color.
    /// </summary>
    /// <param name="color">Color to set.</param>
    internal void SetFillColor(SKColor color)
    {
        _selectedColorForeground.SetFillColor(color);
    }

    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        _squareBackground.Dispose();
        _selectedColorForeground.Dispose();
    }
}