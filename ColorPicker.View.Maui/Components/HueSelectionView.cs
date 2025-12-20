using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ColorPicker.View.Maui.Components;

public class HueSelectionView : SKCanvasView, IDisposable
{
    /// <summary>
    /// Orientation of the component.
    /// </summary>
    private SelectionOrientation _orientation;

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
    private bool _needViewResize;

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

    private readonly SKPaint _markerPaint = new()
    {
        Color = SKColors.Black,
        IsAntialias = true,
        Style = SKPaintStyle.Fill
    };

    private readonly SKPath _trianglePath = new();

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
            _selectionMarkNeedsUpdate = true;
            _relativeMarkY = _relativeMarkX = -1;
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
    /// Initializes a new instance of the HueSelectionView class.
    /// </summary>
    public HueSelectionView()
    {
        SizeChanged += HueSelectionView_SizeChanged;
        Touch += HueSelectionView_Touch;

        EnableTouchEvents = true;
    }

    private void HueSelectionView_SizeChanged(object? sender, EventArgs e)
    {
        this.InvalidateSurface();
    }

    private void HueSelectionView_Touch(object? sender, SKTouchEventArgs e)
    {
        if (e.ActionType == SKTouchAction.Pressed)
        {
            if (_orientation == SelectionOrientation.Vertical)
            {
                double y = Math.Min(Math.Max(0, e.Location.Y), _height);
                double newY = y / _height;

                if (Math.Abs(_relativeMarkY - newY) > 0.01)
                {
                    _relativeMarkY = newY;

                    _isTouchPressed = true;

                    _selectionMarkNeedsUpdate = true;

                    UpdateSelectionMark();
                    UpdateColorPreview();
                    InvalidateSurface();
                }
            }
            else
            {
                double x = Math.Min(Math.Max(0, e.Location.X), _width);
                double newX = x / _width;

                if (Math.Abs(_relativeMarkX - newX) > 0.01)
                {
                    _relativeMarkX = newX;

                    _isTouchPressed = true;

                    _selectionMarkNeedsUpdate = true;

                    UpdateSelectionMark();
                    UpdateColorPreview();
                    InvalidateSurface();
                }
            }
        }
        else if (e.ActionType == SKTouchAction.Moved)
        {
            if (_isTouchPressed)
            {
                if (_orientation == SelectionOrientation.Vertical)
                {
                    double y = Math.Min(Math.Max(0, e.Location.Y), _height);
                    double newY = y / _height;

                    if (Math.Abs(_relativeMarkY - newY) > 0.002)
                    {
                        _relativeMarkY = newY;

                        _selectionMarkNeedsUpdate = true;

                        UpdateSelectionMark();
                        UpdateColorPreview();
                        InvalidateSurface();
                    }
                }
                else
                {
                    double x = Math.Min(Math.Max(0, e.Location.X), _width);
                    double newX = x / _width;

                    if (Math.Abs(_relativeMarkX - newX) > 0.002)
                    {
                        _relativeMarkX = newX;

                        _selectionMarkNeedsUpdate = true;

                        UpdateSelectionMark();
                        UpdateColorPreview();
                        InvalidateSurface();
                    }
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

        if (_orientation == SelectionOrientation.Horizontal)
        {
            float x = (float)(_relativeMarkX * _width);
            float h = _height;

            DrawTriangle(canvas,
                new SKPoint(x - h / 3, 0),
                new SKPoint(x + h / 3, 0),
                new SKPoint(x, h / 3));

            DrawTriangle(canvas,
                new SKPoint(x - h / 3, h),
                new SKPoint(x + h / 3, h),
                new SKPoint(x, 2 * h / 3));
        }
        else
        {
            float y = (float)(_relativeMarkY * _height);
            float w = _width;

            DrawTriangle(canvas,
                new SKPoint(0, y - w / 3),
                new SKPoint(0, y + w / 3),
                new SKPoint(w / 3, y));

            DrawTriangle(canvas,
                new SKPoint(w, y - w / 3),
                new SKPoint(w, y + w / 3),
                new SKPoint(2 * w / 3, y));
        }

        _selectionMarkNeedsUpdate = false;
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        var canvas = e.Surface.Canvas;
        canvas.Clear();

        // For the case, when size has been changed
        if (_width != e.Info.Width || _height != e.Info.Height)
        {
            _width = e.Info.Width;
            _height = e.Info.Height;

            _orientation = _width > _height ? SelectionOrientation.Horizontal : SelectionOrientation.Vertical;

            _needViewResize = true;
        }

        // Calculates the new position of the mark
        if (_relativeMarkX == -1 || _relativeMarkY == -1)
        {
            _relativeMarkX = _hue / 360d;
            _relativeMarkY = _hue / 360d;
        }

        if (_needViewResize)
        {
            _backgroundBitmap?.Dispose();
            _backgroundBitmap = null;

            _selectionMarkSurface?.Dispose();
            _selectionMarkSurface = null;

            _selectionMarkNeedsUpdate = true;
            _needViewResize = false;
        }

        CreateSelectionMarkSurface();
        UpdateSelectionMark();

        if (_backgroundBitmap == null || _backgroundBitmap.Width != _width || _backgroundBitmap.Height != _height)
        {
            _backgroundBitmap?.Dispose();
            _backgroundBitmap = CreateHueBitmap();
        }

        canvas.DrawBitmap(_backgroundBitmap, 0, 0);
        canvas.DrawSurface(_selectionMarkSurface, 0, 0);
    }

    /// <summary>
    /// Returns the hue bitmap of the current hue.
    /// </summary>
    /// <returns>The hue bitmap of the current hue.</returns>
    private SKBitmap CreateHueBitmap()
    {
        var bitmap = new SKBitmap(
            _width, _height, 
            SKColorType.Bgra8888, SKAlphaType.Premul);

        Span<byte> pixels = bitmap.GetPixelSpan();

        int index = 0;

        for (int y = 0; y < _height; y++)
        {
            double h = _orientation == SelectionOrientation.Vertical
                ? (double) y / (_height - 1) * 360d : 0;

            for (int x = 0; x < _width; x++)
            {
                if (_orientation == SelectionOrientation.Horizontal)
                    h = (double)x / (_width - 1) * 360d;

                var c = HsvToSkColor(h, 1, 1);

                pixels[index++] = c.Blue;
                pixels[index++] = c.Green;
                pixels[index++] = c.Red;
                pixels[index++] = c.Alpha;
            }
        }

        return bitmap;
    }

    /// <summary>
    /// Creates the selection mark surface.
    /// </summary>
    private void CreateSelectionMarkSurface()
    {
        // It is not necessary to create the surface again
        if (_selectionMarkSurface != null &&
            _selectionMarkImage.Width == _width &&
            _selectionMarkImage.Height == _height)
            return;

        _selectionMarkSurface?.Dispose();

        _selectionMarkImage = new SKImageInfo(
            _width,
            _height,
            SKColorType.Bgra8888,
            SKAlphaType.Premul);

        _selectionMarkSurface = SKSurface.Create(_selectionMarkImage);

        // For first draw
        _selectionMarkNeedsUpdate = true;
    }

    private void DrawTriangle(SKCanvas canvas, SKPoint p1, SKPoint p2, SKPoint p3)
    {
        _trianglePath.Reset();
        _trianglePath.MoveTo(p1);
        _trianglePath.LineTo(p2);
        _trianglePath.LineTo(p3);
        _trianglePath.Close();

        canvas.DrawPath(_trianglePath, _markerPaint);
    }

    /// <summary>
    /// Returns the color for specific HSV.
    /// </summary>
    /// <param name="hue">Hue.</param>
    /// <param name="saturation">Saturation.</param>
    /// <param name="value">Value (brightness).</param>
    /// <returns>The color for specific HSV.</returns>
    private static SKColor HsvToSkColor(double hue, double saturation, double value)
    {
        hue %= 360d;
        double c = value * saturation;
        double x = c * (1 - Math.Abs((hue / 60) % 2 - 1));
        double m = value - c;

        double r = 0, g = 0, b = 0;
        int hi = (int)(hue / 60);

        switch (hi)
        {
            case 0: r = c; g = x; break;
            case 1: r = x; g = c; break;
            case 2: g = c; b = x; break;
            case 3: g = x; b = c; break;
            case 4: r = x; b = c; break;
            default: r = c; b = x; break;
        }

        return new SKColor(
            (byte)((r + m) * 255),
            (byte)((g + m) * 255),
            (byte)((b + m) * 255));
    }

    /// <summary>
    /// Updates the selected color from clicked position.
    /// </summary>
    private void UpdateColorPreview()
    {
        double hue;

        if (_orientation == SelectionOrientation.Vertical)
        {
            hue = _relativeMarkY * 360d;
        }
        else
        {
            hue = _relativeMarkX * 360d;
        }

        hue = Math.Max(0, hue);
        hue = Math.Min(360d, hue);

        _viewModel.Hsv.Hue = _hue = hue;
    }

    public void Dispose()
    {
        if (_backgroundBitmap != null)
            _backgroundBitmap.Dispose();

        if (_selectionMarkSurface != null)
            _selectionMarkSurface.Dispose();

        SizeChanged -= HueSelectionView_SizeChanged;
        Touch -= HueSelectionView_Touch;
    }
}