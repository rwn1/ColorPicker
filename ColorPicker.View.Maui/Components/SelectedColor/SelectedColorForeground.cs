using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ColorPicker.View.Maui.Components;
public class SelectedColorForeground : SKCanvasView, IDisposable
{
    /// <summary>
    /// Paint for selected color.
    /// </summary>
    private SKPaint _paint;

    /// <summary>
    /// Initializes a new instance of the SelectedColorForeground class.
    /// </summary>
    public SelectedColorForeground()
    {
        SizeChanged += SelectedColorForeground_SizeChanged;

        _paint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };
    }

    /// <summary>
    /// Occurs when the size of an element changed.
    /// </summary>
    private void SelectedColorForeground_SizeChanged(object? sender, EventArgs e)
    {
        InvalidateSurface();
    }

    /// <summary>
    /// Sets the fill color.
    /// </summary>
    /// <param name="color">Color to set.</param>
    internal void SetFillColor(SKColor color)
    {
        _paint.Color = color;
        InvalidateSurface();
    }

    /// <summary>
    /// Called whenever the SkiaSharp surface needs to be redrawn.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        var canvas = e.Surface.Canvas;
        canvas.Clear();

        canvas.DrawRect(0, 0, e.Info.Width, e.Info.Height, _paint);
    }

    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        _paint?.Dispose();

        SizeChanged -= SelectedColorForeground_SizeChanged;
    }
}