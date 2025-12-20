using ColorPicker.Core.Utilities;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ColorPicker.View.Maui.Components;

public class ColorSelectionView : SKGLView, IDisposable
{
    public static readonly BindableProperty MarkRadiusProperty =
        BindableProperty.Create(
            nameof(MarkRadius),
            typeof(int),
            typeof(ColorSelectionView),
            6,
            propertyChanged: OnMarkRadiusChanged
            );

    public int MarkRadius
    {
        get => (int)GetValue(MarkRadiusProperty);
        set => SetValue(MarkRadiusProperty, value);
    }
    private static void OnMarkRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ColorSelectionView)bindable;
        int newSize = (int)newValue;

    }

    /// <summary>
    /// Relative position of the mark selection on the x-axis.
    /// </summary>
    private double _relativeMarkX = -1;

    /// <summary>
    /// Relative position of the mark selection on the y-axis.
    /// </summary>
    private double _relativeMarkY = -1;

    /// <summary>
    /// Canvas width.
    /// </summary>
    private int _width;

    /// <summary>
    /// Canvas width.
    /// </summary>
    private int _height;

    /// <summary>
    /// Information about whether the touch is pressed.
    /// </summary>
    private bool _isTouchPressed = false;

    /// <summary>
    /// Information about whether the mark needs to be updated.
    /// </summary>
    private bool _selectionMarkNeedsUpdate = true;

    /// <summary>
    /// Information about whether the canvas needs to be redrawn.
    /// </summary>
    private bool _needViewResize = false;

    /// <summary>
    /// Last saturation value.
    /// </summary>
    private double _lastSaturation = -1;

    /// <summary>
    /// Last brightness value.
    /// </summary>
    private double _lastValue = -1;

    /// <summary>
    /// Cached background bitmap.
    /// </summary>
    private SKBitmap _backgroundBitmap = null;

    /// <summary>
    /// Cached selection mark surface.
    /// </summary>
    private SKSurface _selectionMarkSurface = null;

    /// <summary>
    /// Cached selection mark image.
    /// </summary>
    private SKImageInfo _selectionMarkImage;

    private SKPaint _whitePain = null;
    private SKPaint _blackPain = null;

    /// <summary>
    /// Cached pixels in current solution
    /// </summary>
    private byte[] _pixels = null;

    private double _hue;
    /// <summary>
    /// Gets or sets the hue to visualization.
    /// </summary>
    internal double Hue
    {
        get => _hue;
        set
        {
            if (_hue == value)
                return;

            _hue = value;

            _backgroundBitmap?.Dispose();
            _backgroundBitmap = null;
            InvalidateSurface();
        }
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
        }
    }

    /// <summary>
    /// Initializes a new instance of the ColorSelectionView class.
    /// </summary>
    public ColorSelectionView()
    {
        SizeChanged += ColorSelectionView_SizeChanged;
        Touch += ColorSelectionView_Touch;

        EnableTouchEvents = true;

        int strokeWidth = Math.Max(6, MarkRadius) / 3;

        _whitePain = new SKPaint
        {
            Color = SKColors.White,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = strokeWidth,
            IsAntialias = true
        };

        _blackPain = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = strokeWidth,
            IsAntialias = true
        };
    }

    private void ColorSelectionView_SizeChanged(object? sender, EventArgs e)
    {
        InvalidateSurface();
    }

    private void ColorSelectionView_Touch(object? sender, SKTouchEventArgs e)
    {
        if (e.ActionType == SKTouchAction.Pressed)
        {
            double x = Math.Min(Math.Max(0, e.Location.X), _width);
            double y = Math.Min(Math.Max(0, e.Location.Y), _height);

            double newX = x / _width;
            double newY = y / _height;

            if (Math.Abs(_relativeMarkX - newX) > 0.002 ||
                Math.Abs(_relativeMarkY - newY) > 0.002)
            {
                _relativeMarkX = newX;
                _relativeMarkY = newY;

                _isTouchPressed = true;

                _selectionMarkNeedsUpdate = true;

                UpdateSelectionMark();
                UpdateColorPreview();
                InvalidateSurface();
            }
        }
        else if (e.ActionType == SKTouchAction.Moved)
        {
            if (_isTouchPressed)
            {
                double x = Math.Min(Math.Max(0, e.Location.X), _width);
                double y = Math.Min(Math.Max(0, e.Location.Y), _height);

                double newX = x / _width;
                double newY = y / _height;

                if (Math.Abs(_relativeMarkX - newX) > 0.002 ||
                    Math.Abs(_relativeMarkY - newY) > 0.002)
                {
                    _relativeMarkX = newX;
                    _relativeMarkY = newY;

                    _selectionMarkNeedsUpdate = true;

                    UpdateSelectionMark();
                    UpdateColorPreview();
                    InvalidateSurface();
                }
            }
        }
        else if (e.ActionType == SKTouchAction.Released)
        {
            _isTouchPressed = false;
        }

        e.Handled = true;
    }

    /// <summary>
    /// Updates the selection mark position.
    /// </summary>
    private void UpdateSelectionMark()
    {
        if (!_selectionMarkNeedsUpdate || _selectionMarkSurface == null)
            return;

        var canvas = _selectionMarkSurface.Canvas;
        canvas.Clear(SKColors.Transparent);

        int strokeWidth = Math.Max(6, MarkRadius) / 3;

        _whitePain.StrokeWidth = strokeWidth;
        _blackPain.StrokeWidth = strokeWidth;

        double x = _relativeMarkX * _width;
        double y = _relativeMarkY * _height;

        int markRadius = Math.Max(2, MarkRadius);

        canvas.DrawCircle((float)x, (float)y, markRadius - strokeWidth, _whitePain);
        canvas.DrawCircle((float)x, (float)y, markRadius, _blackPain);
        canvas.DrawCircle((float)x, (float)y, markRadius + strokeWidth, _whitePain);

        _selectionMarkNeedsUpdate = false;
    }

    protected override void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        var canvas = e.Surface.Canvas;
        //canvas.Clear();

        // For the case, when size has been changed
        if (_width != e.Info.Width || _height != e.Info.Height)
        {
            _width = e.Info.Width;
            _height = e.Info.Height;

            _needViewResize = true;
        }

        // Calculates the new position of the mark
        if (_relativeMarkX == -1 || _relativeMarkY == -1)
        {
            double x = (Math.Min(Math.Max(0, _viewModel.Hsv.Saturation * (_width - 1)), _width));
            double y = (Math.Min(Math.Max(0, (1 - _viewModel.Hsv.Value) * (_height - 1)), _height));

            _relativeMarkX = x / _width;
            _relativeMarkY = y / _height;
        }

        if (_needViewResize)
        {
            if (_selectionMarkSurface != null)
                _selectionMarkSurface.Dispose();

            _selectionMarkNeedsUpdate = true;
            _needViewResize = false;
        }

        CreateSelectionMarkSurface();
        UpdateSelectionMark();

        if (_backgroundBitmap == null || _backgroundBitmap.Width != _width || _backgroundBitmap.Height != _height)
        {
            _backgroundBitmap?.Dispose();
            _backgroundBitmap = CreateHsvBitmap();
        }

        canvas.DrawBitmap(_backgroundBitmap, 0, 0);
        canvas.DrawSurface(_selectionMarkSurface, 0, 0);
    }

    /// <summary>
    /// Returns the HSV bitmap of the current hue.
    /// </summary>
    /// <returns>The HSV bitmap of the current hue.</returns>
    private SKBitmap CreateHsvBitmap()
    {
        var bitmap = new SKBitmap(
            _width, _height,
            SKColorType.Bgra8888,
            SKAlphaType.Premul);

        Span<byte> pixels = bitmap.GetPixelSpan();

        int index = 0;

        for (int y = 0; y < _height; y++)
        {
            double v = 1.0 - (double)y / (_height - 1);

            for (int x = 0; x < _width; x++)
            {
                double s = (double)x / (_width - 1);
                ColorConversions.HsvToRgb(Hue, s, v, out byte r, out byte g, out byte b);
                var c = new SKColor(r, g, b);

                pixels[index++] = c.Blue;
                pixels[index++] = c.Green;
                pixels[index++] = c.Red;
                pixels[index++] = c.Alpha;
            }
        }

        // Cache with current pixels
        _pixels = pixels.ToArray();

        return bitmap;
    }

    /// <summary>
    /// Returns the color for specific HSV.
    /// </summary>
    /// <param name="h">Hue.</param>
    /// <param name="s">Saturation.</param>
    /// <param name="v">Value (brightness).</param>
    /// <returns>The color for specific HSV.</returns>
    private static SKColor HsvToSkColor(double h, double s, double v)
    {
        ColorConversions.HsvToRgb(h, s, v, out byte r, out byte g, out byte b);
        return new SKColor(r, g, b);
    }

    /// <summary>
    /// Creates the selection mark surface.
    /// </summary>
    private void CreateSelectionMarkSurface()
    {
        // It is not necessary to create the surface again
        if (_selectionMarkSurface != null &&
            _width == _selectionMarkImage.Width &&
            _height == _selectionMarkImage.Height)
            return;

        _selectionMarkSurface?.Dispose();

        _selectionMarkImage = new SKImageInfo(
                _width, _height,
                SKColorType.Bgra8888,
                SKAlphaType.Premul);

        _selectionMarkSurface = SKSurface.Create(_selectionMarkImage);

        // For first draw
        _selectionMarkNeedsUpdate = true;
    }

    /// <summary>
    /// Returns the stored color from position in array.
    /// </summary>
    /// <param name="x">Position in x direction.</param>
    /// <param name="y">Position in y direction.</param>
    /// <returns>The stored color from position in array.</returns>
    public void GetRGBFromBitmap(int x, int y, out byte r, out byte g, out byte b, out byte a)
    {
        int index = (y * _width + x) * 4;

        b = _pixels[index + 0]; // B
        g = _pixels[index + 1]; // G
        r = _pixels[index + 2]; // R
        a = _pixels[index + 3]; // A
    }

    /// <summary>
    /// Updates the selected color from clicked position.
    /// </summary>
    private void UpdateColorPreview()
    {
        if (_pixels == null)
            return;

        // Clicked position
        int x = (int)(_relativeMarkX * _width);
        int y = (int)(_relativeMarkY * _height);

        if (x < 0 || y < 0 || x >= _width || y >= _height)
            return;

        // Bytes: B,G,R,A
        byte r, g, b, a;
        GetRGBFromBitmap(x, y, out r, out g, out b, out a);

        // Convert RGB to HSV
        ColorConversions.RgbToHsv(r, g, b, out double h, out double s, out double v);

        // Only for prevention against rending due to changes in view model
        _hue = h;
        _lastSaturation = s;
        _lastValue = v;

        _viewModel.SelectColor(r, g, b, _viewModel.Alpha.Alpha);
    }

    /// <summary>
    /// Updates the values in Hsv.
    /// </summary>
    /// <param name="saturation">Saturation.</param>
    /// <param name="value">Value (brightness).</param>
    internal void UpdateHsv(double saturation, double value)
    {
        // Prevention against changes from this class
        if (_lastSaturation != saturation || _lastValue != value)
        {
            // Updates the last set values
            _lastSaturation = saturation;
            _lastValue = value;

            _relativeMarkX = _relativeMarkY = -1;

            InvalidateSurface();
        }
    }

    public void Dispose()
    {
        if (_whitePain != null)
            _whitePain.Dispose();

        if (_blackPain != null)
            _blackPain.Dispose();

        if (_backgroundBitmap != null)
            _backgroundBitmap.Dispose();

        if (_selectionMarkSurface != null)
            _selectionMarkSurface.Dispose();

        SizeChanged -= ColorSelectionView_SizeChanged;
        Touch -= ColorSelectionView_Touch;
    }
}