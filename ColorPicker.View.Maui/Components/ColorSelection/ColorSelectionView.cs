using ColorPicker.View.Maui.Models;

namespace ColorPicker.View.Maui.Components;

public class ColorSelectionView : Grid, IDisposable
{
    private readonly ShareDataModel _shareModel;    
    private readonly ColorSelectionMark _colorSelectionMark;
    private readonly ColorSelectionBackground _colorSelectionBackground;

    public static readonly BindableProperty MarkRadiusProperty =
        BindableProperty.Create(
            nameof(MarkRadius),
            typeof(int),
            typeof(ColorSelectionView),
            10,
            propertyChanged: OnMarkRadiusChanged);

    public int MarkRadius
    {
        get => (int)GetValue(MarkRadiusProperty);
        set => SetValue(MarkRadiusProperty, value);
    }

    private static void OnMarkRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ColorSelectionView)bindable;
        int newSize = (int)newValue;

        control.OnSelectedBrushChanged(newSize);
    }

    protected virtual void OnSelectedBrushChanged(int newSize)
    {
        _colorSelectionMark.SetMarkRadius(newSize);
    }
        
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
            _colorSelectionMark.ViewModel = _viewModel;
        }
    }

    /// <summary>
    /// Initializes a new instance of the ColorSelectionView class.
    /// </summary>
    public ColorSelectionView()
    {
        _shareModel = new ShareDataModel();

        _colorSelectionMark = new ColorSelectionMark(_shareModel);
        _colorSelectionBackground = new ColorSelectionBackground(_shareModel);

        Children.Add(_colorSelectionBackground);
        Children.Add(_colorSelectionMark);
    }

    /// <summary>
    /// Sets the selected color.
    /// </summary>
    /// <param name="r">Red component.</param>
    /// <param name="g">Green component.</param>
    /// <param name="b">Blue component.</param>
    internal void SetSelectedColor(byte r, byte g, byte b)
    {
        _colorSelectionMark.SetSelectedColor(r, g, b);
    }

    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        _colorSelectionMark.Dispose();
        _colorSelectionBackground.Dispose();
    }
}