using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ColorPicker.View.Maui.Components;

public class SelectedColorView : SKCanvasView, IDisposable
{
    /// <summary>
    /// Orientation of the component.
    /// </summary>
    private SelectionOrientation _orientation;

    /// <summary>
    /// Canvas width.
    /// </summary>
    private int _width;

    /// <summary>
    /// Canvas width.
    /// </summary>
    private int _height;

    /// <summary>
    /// Information about whether the canvas needs to be redrawn.
    /// </summary>
    private bool _needViewResize;

    /// <summary>
    /// Cached backround bitmap.
    /// </summary>
    private SKBitmap _backgroundBitmap = null;

    private Color _selectedColor = null;
    /// <summary>
    /// Gets or sets the hue to visualization.
    /// </summary>
    internal Color SelectedColor
    {
        get => _selectedColor;
        set
        {
            if (_selectedColor == value)
                return;

            _selectedColor = value;

            InvalidateSurface();
        }
    }

    /// <summary>
    /// Initializes a new instance of the SelectedColorView class.
    /// </summary>
    public SelectedColorView()
    {
        SizeChanged += SelectedColorView_SizeChanged;

        EnableTouchEvents = true;
    }

    private void SelectedColorView_SizeChanged(object? sender, EventArgs e)
    {
        this.InvalidateSurface();
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

        if (_needViewResize)
        {
            _backgroundBitmap?.Dispose();
            _backgroundBitmap = null;

            _needViewResize = false;
        }

        if (_backgroundBitmap == null || _backgroundBitmap.Width != _width || _backgroundBitmap.Height != _height)
        {
            _backgroundBitmap?.Dispose();
            _backgroundBitmap = CreateBackgroundBitmap();
        }

        using var fill = new SKPaint
        {
            Color = _selectedColor.ToSKColor(),
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        canvas.DrawBitmap(_backgroundBitmap, 0, 0);
        canvas.DrawRect(0, 0, _width, _height, fill);
    }

    /// <summary>
    /// Returns the foreground bitmap of the selected color.
    /// </summary>
    /// <returns>The hue foreground of the selected color.</returns>
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
                canvas.DrawRect(
                    x, y, tileSize, tileSize,
                    lightPaint);

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

        return bitmap;
    }

    public void Dispose()
    {
        if (_backgroundBitmap != null)
            _backgroundBitmap.Dispose();

        SizeChanged -= SelectedColorView_SizeChanged;
    }
}