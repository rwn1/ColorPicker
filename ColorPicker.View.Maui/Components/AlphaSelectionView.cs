using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ColorPicker.View.Maui.Components;

public class AlphaSelectionView : SKCanvasView, IDisposable
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
    /// Cached foreground bitmap.
    /// </summary>
    private SKBitmap _foregroundBitmap = null;

    /// <summary>
    /// Cached backround bitmap.
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

    private Color _selectedColor = null;
    /// <summary>
    /// Gets or sets the color to visualization.
    /// </summary>
    internal Color SelectedColor
    {
        get => _selectedColor;
        set
        {
            if (_selectedColor == value)
                return;

            bool isColorChanged = false;

            if (_selectedColor != null &&
                (value.Red != _selectedColor.Red ||
                value.Green != _selectedColor.Green ||
                value.Blue != _selectedColor.Blue))
            {
                isColorChanged = true;
            }

            _selectedColor = value;
            _selectedColor = _selectedColor.WithAlpha(1);

            if (isColorChanged)
            {
                _foregroundBitmap = null;
                _relativeMarkY = _relativeMarkX = -1;
                InvalidateSurface();
            }
        }
    }

    private double _alpha;
    /// <summary>
    /// Gets or sets the alpha to visualization.
    /// </summary>
    internal double Alpha
    {
        get => _alpha;
        set
        {
            if (_alpha == value)
                return;

            _alpha = value;
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
    /// Initializes a new instance of the AlphaSelectionView class.
    /// </summary>
    public AlphaSelectionView()
    {
        SizeChanged += AlphaSelectionView_SizeChanged;
        Touch += AlphaSelectionView_Touch;

        EnableTouchEvents = true;
    }

    private void AlphaSelectionView_SizeChanged(object? sender, EventArgs e)
    {
        InvalidateSurface();
    }

    private void AlphaSelectionView_Touch(object? sender, SKTouchEventArgs e)
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

                    if (Math.Abs(_relativeMarkY - newY) > 0.01)
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

                    if (Math.Abs(_relativeMarkX - newX) > 0.01)
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
        if (_orientation == SelectionOrientation.Vertical)
        {
            if (_relativeMarkY == -1)
            {
                _relativeMarkY = _alpha;
            }
        }
        else
        {
            if (_relativeMarkX == -1)
            {
                _relativeMarkX = _alpha;
            }
        }

        if (_needViewResize)
        {
            _foregroundBitmap?.Dispose();
            _foregroundBitmap = null;

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
            _backgroundBitmap = CreateBackgroundBitmap();
        }

        if (_foregroundBitmap == null || _foregroundBitmap.Width != _width || _foregroundBitmap.Height != _height)
        {
            _foregroundBitmap?.Dispose();
            _foregroundBitmap = CreateForegroundBitmap();
        }

        canvas.DrawBitmap(_backgroundBitmap, 0, 0);
        canvas.DrawBitmap(_foregroundBitmap, 0, 0);
        canvas.DrawSurface(_selectionMarkSurface, 0, 0);
    }

    /// <summary>
    /// Returns the foreground bitmap of the selected color.
    /// </summary>
    /// <returns>The foreground bitmap of the selected color.</returns>
    private SKBitmap CreateForegroundBitmap()
    {
        var bitmap = new SKBitmap(
              _width,
              _height,
              SKColorType.Bgra8888,
              SKAlphaType.Premul);

        Span<byte> pixels = bitmap.GetPixelSpan();
        int index = 0;

        for (int y = 0; y < _height; y++)
        {
            double tY = (double)y / (_height - 1);

            for (int x = 0; x < _width; x++)
            {
                double t = _orientation == SelectionOrientation.Vertical
                    ? tY
                    : (double)x / (_width - 1);

                // 1.0 = neprůhledné, 0.0 = průhledné
                byte alpha = (byte)((1.0 - t) * 255);

                ToPremultipliedBytes(
                    _selectedColor,
                    alpha,
                    out byte r,
                    out byte g,
                    out byte b);

                pixels[index++] = b;     // Blue
                pixels[index++] = g;     // Green
                pixels[index++] = r;     // Red
                pixels[index++] = alpha; // Alpha
            }
        }

        return bitmap;
    }

    private static void ToPremultipliedBytes(
    Microsoft.Maui.Graphics.Color color,
    byte alpha,
    out byte r, out byte g, out byte b)
    {
        float a = alpha / 255f;

        r = (byte)(color.Red * 255f * a);
        g = (byte)(color.Green * 255f * a);
        b = (byte)(color.Blue * 255f * a);
    }

    /// <summary>
    /// Returns the foreground bitmap of the selected color.
    /// </summary>
    /// <returns>The foreground bitmap of the selected color.</returns>
    private SKBitmap CreateBackgroundBitmap()
    {
        int tileSize;

        if (_orientation == SelectionOrientation.Vertical)
        {
            tileSize = _width;
        }
        else
        {
            tileSize = _height;
        }

        var bitmap = new SKBitmap(
            _width, _height,
            SKColorType.Bgra8888,
            SKAlphaType.Opaque);

        using var canvas = new SKCanvas(bitmap);

        var light = new SKColor(0xF0, 0xF0, 0xF0);
        var dark = new SKColor(0xD0, 0xD0, 0xD0);

        using var lightPaint = new SKPaint { Color = light };
        using var darkPaint = new SKPaint { Color = dark };

        for (int y = 0; y < _height; y += tileSize)
        {
            for (int x = 0; x < _width; x += tileSize)
            {
                //bool even = ((x / tileSize) + (y / tileSize)) % 2 == 0;

                // pozadí celé dlaždice
                canvas.DrawRect(
                    x, y, tileSize, tileSize,
                    lightPaint);

                // tmavé čtverce diagonálně
                //if (even)
                {
                    canvas.DrawRect(
                        x, y,
                        tileSize / 2f,
                        tileSize / 2f,
                        darkPaint);

                    canvas.DrawRect(
                        x + tileSize / 2f,
                        y + tileSize / 2f,
                        tileSize / 2f,
                        tileSize / 2f,
                        darkPaint);
                }
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
    /// Updates the selected color from clicked position.
    /// </summary>
    private void UpdateColorPreview()
    {
        double alpha;

        if (_orientation == SelectionOrientation.Vertical)
        {
            alpha = 1 - _relativeMarkY;
        }
        else
        {
            alpha = 1 - _relativeMarkX;
        }

        alpha = Math.Max(0, alpha);
        alpha = Math.Min(1, alpha);

        _viewModel.Alpha.Alpha = _alpha = alpha;
    }

    public void Dispose()
    {
        if (_backgroundBitmap != null)
            _backgroundBitmap.Dispose();

        if (_foregroundBitmap != null)
            _foregroundBitmap.Dispose();

        if (_selectionMarkSurface != null)
            _selectionMarkSurface.Dispose();

        SizeChanged -= AlphaSelectionView_SizeChanged;
        Touch -= AlphaSelectionView_Touch;
    }
}