using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ColorPicker.View.Maui.Components;
public class SquareBackground: SKCanvasView, IDisposable
{
    /// <summary>
    /// Paint for light squares of background.
    /// </summary>
    private readonly SKPaint _lightPaint;

    /// <summary>
    /// Paint for dark squares of background.
    /// </summary>
    private readonly SKPaint _darkPaint;

    /// <summary>
    /// Cached backround bitmap.
    /// </summary>
    private SKBitmap _bitmap = null;

    /// <summary>
    /// Initializes a new instance of the SquareBackground class.
    /// </summary>
    public SquareBackground()
    {
        var light = new SKColor(0xF0, 0xF0, 0xF0);
        var dark = new SKColor(0xD0, 0xD0, 0xD0);

        _lightPaint = new SKPaint { Color = light };
        _darkPaint = new SKPaint { Color = dark };

        SizeChanged += SquareBackground_SizeChanged;
    }

    /// <summary>
    /// Called whenever the SkiaSharp surface needs to be redrawn.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        var canvas = e.Surface.Canvas;

        _bitmap?.Dispose();
        _bitmap = CreateBackgroundBitmap(e.Info.Width, e.Info.Height);

        canvas.DrawBitmap(_bitmap, 0, 0);
    }

    /// <summary>
    /// Returns the bitmap for background.
    /// </summary>
    /// <param name="width">Width to draw.</param>
    /// <param name="height">Height to draw.</param>
    /// <returns>The bitmap for background.</returns>
    private SKBitmap CreateBackgroundBitmap(int width, int height)
    {
        int tileSize = Math.Min(height, width);

        var bitmap = new SKBitmap(
            width, height,
            SKColorType.Bgra8888,
            SKAlphaType.Opaque);

        using var canvas = new SKCanvas(bitmap);

        for (int y = 0; y < height; y += tileSize)
        {
            for (int x = 0; x < width; x += tileSize)
            {
                canvas.DrawRect(
                    x, y, tileSize, tileSize,
                    _lightPaint);

                canvas.DrawRect(
                    x, y,
                    tileSize / 2f,
                    tileSize / 2f,
                    _darkPaint);

                canvas.DrawRect(
                    x + tileSize / 2f,
                    y + tileSize / 2f,
                    tileSize / 2f,
                    tileSize / 2f,
                    _darkPaint);
            }
        }

        return bitmap;
    }

    /// <summary>
    /// Occurs when the size of an element changed.
    /// </summary>
    private void SquareBackground_SizeChanged(object? sender, EventArgs e)
    {
        InvalidateSurface();
    }

    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        _lightPaint?.Dispose();
        _darkPaint?.Dispose();

        _bitmap?.Dispose();

        SizeChanged -= SquareBackground_SizeChanged;
    }
}