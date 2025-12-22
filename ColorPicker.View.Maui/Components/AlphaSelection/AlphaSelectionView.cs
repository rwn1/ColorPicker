using SkiaSharp;

namespace ColorPicker.View.Maui.Components;

public class AlphaSelectionView : Grid, IDisposable
{
    private readonly SquareBackground _squareBackground;
    private readonly AlphaSelectionForeground _alphaSelectionForeground;
    private readonly AlphaSelectionMark _alphaSelectionMark;

    private ColorPickerViewModel _viewModel = null;
    /// <summary>
    /// Gets or sets the main view model.
    /// </summary>
    internal ColorPickerViewModel ViewModel
    {
        get => _viewModel;
        set
        {
            if (_viewModel == value)
                return;

            _viewModel = value;
            _alphaSelectionMark.ViewModel = _viewModel;
        }
    }

    /// <summary>
    /// Initializes a new instance of the AlphaSelectionView class.
    /// </summary>
    public AlphaSelectionView()
    {
        _squareBackground = new SquareBackground();
        _alphaSelectionForeground = new AlphaSelectionForeground();
        _alphaSelectionMark = new AlphaSelectionMark();

        Children.Add(_squareBackground);
        Children.Add(_alphaSelectionForeground);
        Children.Add(_alphaSelectionMark);
    }

    /// <summary>
    /// Sets the fill color.
    /// </summary>
    /// <param name="color">Color to set.</param>
    internal void SetSelectedColor(SKColor color)
    {
        _alphaSelectionForeground.SetSelectedColor(color);
    }

    /// <summary>
    /// Sets the alpha value.
    /// </summary>
    /// <param name="alpha">Alpha value to set.</param>
    internal void SetAlpha(float alpha)
    {
        _alphaSelectionMark.SetAlpha(alpha);
    }

    public void Dispose()
    {
        _squareBackground.Dispose();
        _alphaSelectionForeground.Dispose();
        _alphaSelectionMark.Dispose();
    }
}