using ColorPicker.Core.Models;
using ColorPicker.View.Maui.Components;

namespace ColorPicker.View.Maui;

public class ColorPicker : TemplatedView
{
    public static readonly BindableProperty SelectedColorProperty =
    BindableProperty.Create(
        nameof(SelectedColor),
        typeof(Color),
        typeof(ColorPicker),
        default(Color),
        propertyChanged: OnSelectedColorChanged);

    /// <summary>
    /// Gets or sets the selected color of color picker.
    /// </summary>
    public Color SelectedColor
    {
        get => (Color)GetValue(SelectedColorProperty);
        set => SetValue(SelectedColorProperty, value);
    }

    private static void OnSelectedColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ColorPicker)bindable;
        var newColor = (Color)newValue;

        control.OnSelectedColorChanged(newColor);
    }

    protected virtual void OnSelectedColorChanged(Color newColor)
    {
        _viewModel.Hex.Hex = newColor.ToArgbHex(true);
    }

    /// <summary>
    /// Main view model.
    /// </summary>
    private readonly ColorPickerViewModel _viewModel;

    /// <summary>
    /// Hex controls.
    /// </summary>
    private Label _hexLabel = null;

    /// <summary>
    /// Views for hue selection.
    /// </summary>
    private HueSelectionView _hueGraphicsView = null;

    /// <summary>
    /// Views for alpha selection.
    /// </summary>
    private AlphaSelectionView _alphaSelectionView = null;

    /// <summary>
    /// Views for selected color.
    /// </summary>
    private SelectedColorView _selectedColorView = null;

    /// <summary>
    /// Graphics view for color selection.
    /// </summary>
    private ColorSelectionView _colorSelectionView = null;

    /// <summary>
    /// Initializes a new instance of the ColorPicker class.
    /// </summary>
    public ColorPicker()
    {
        _viewModel = new ColorPickerViewModel();
        _viewModel.Hsv.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(HsvModel.Hue))
            {
                if (_colorSelectionView != null)
                {
                    if (_colorSelectionView.Hue != _viewModel.Hsv.Hue)
                    {
                        _colorSelectionView.Hue = _viewModel.Hsv.Hue;

                        if (_hueGraphicsView != null && _hueGraphicsView.Hue != _viewModel.Hsv.Hue)
                        {
                            if (_hueGraphicsView != null)
                                _hueGraphicsView.Hue = _viewModel.Hsv.Hue;
                        }
                    }

                }
            }
            else if (e.PropertyName == nameof(HsvModel.Saturation) || e.PropertyName == nameof(HsvModel.Value))
            {
                if (_colorSelectionView != null)
                {
                    _colorSelectionView.UpdateHsv(_viewModel.Hsv.Saturation, _viewModel.Hsv.Value);
                }
            }
        };
        _viewModel.Hex.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(HexModel.Hex))
            {
                Color color = Color.FromArgb(_viewModel.Hex.Hex);

                if (_alphaSelectionView != null)
                {
                    _alphaSelectionView.SelectedColor = color;
                }

                if (_selectedColorView != null)
                {
                    _selectedColorView.SelectedColor = color;
                }

                SelectedColor = color;
            }
        };
        _viewModel.Alpha.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(AlphaModel.Alpha))
            {
                if (_alphaSelectionView != null)
                {
                    if (_alphaSelectionView.Alpha != _viewModel.Alpha.Alpha)
                    {
                        _alphaSelectionView.Alpha = _viewModel.Alpha.Alpha;
                    }
                }
            }
        };
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // Hex
        _hexLabel = GetPart<Label>("PART_HexLabel");
        BindLabel(_hexLabel, _viewModel.Hex, nameof(ColorPickerViewModel.Hex.Hex));

        // Hue
        if (_hueGraphicsView != null)
        {
            _hueGraphicsView.Dispose();
        }

        _hueGraphicsView = GetPart<HueSelectionView>("PART_HueSelectionView");
        if (_hueGraphicsView != null)
        {
            _hueGraphicsView.ViewModel = _viewModel;
        }

        // Alpha
        if (_alphaSelectionView != null)
        {
            _alphaSelectionView.Dispose();
        }
        _alphaSelectionView = GetPart<AlphaSelectionView>("PART_AlphaSelectionView");
        if (_alphaSelectionView != null)
        {
            _alphaSelectionView.ViewModel = _viewModel;
        }

        // Selected color
        if (_selectedColorView != null)
        {
            _selectedColorView.Dispose();
        }
        _selectedColorView = GetPart<SelectedColorView>("PART_SelectedColorView");

        // Color selection
        if (_colorSelectionView != null)
        {
            _colorSelectionView.Dispose();
        }

        _colorSelectionView = GetPart<ColorSelectionView>("PART_ColorSelectionView");
        if (_colorSelectionView != null)
        {
            _colorSelectionView.ViewModel = _viewModel;
        }
    }

    /// <summary>
    /// Retrieves a template child by name and casts it to the specified type.
    /// </summary>
    private T GetPart<T>(string name) where T : class
    {
        return GetTemplateChild(name) as T;
    }

    /// <summary>
    /// Binds a Entry to a property.
    /// </summary>
    private void BindLabel(Label textBox, object dataContext, string path)
    {
        if (textBox == null) return;
        textBox.SetBinding(Label.TextProperty,
            new Binding(path) { Source = dataContext, Mode = BindingMode.OneWay });
    }
}

/// <summary>
/// Selection orientation types.
/// </summary>
public enum SelectionOrientation { Vertical, Horizontal }