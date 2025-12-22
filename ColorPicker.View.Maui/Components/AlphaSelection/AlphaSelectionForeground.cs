using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ColorPicker.View.Maui.Components;
public class AlphaSelectionForeground : SKCanvasView, IDisposable
{
    /// <summary>
    /// Selected color.
    /// </summary>
    private SKColor _color;

    /// <summary>
    /// Cached foreground bitmap.
    /// </summary>
    private SKPaint _foregroundPaint;

    /// <summary>
    /// Initializes a new instance of the AlphaSelectionForeground class.
    /// </summary>
    public AlphaSelectionForeground()
    {
        SizeChanged += AlphaSelectionForeground_SizeChanged;
    }

    /// <summary>
    /// Occurs when the size of an element changed.
    /// </summary>
    private void AlphaSelectionForeground_SizeChanged(object? sender, EventArgs e)
    {
        InvalidateSurface();
    }

    /// <summary>
    /// Sets the selected color.
    /// </summary>
    /// <param name="color">Color to set.</param>
    internal void SetSelectedColor(SKColor color)
    {
        if (_color == color.WithAlpha(255))
            return;

        _color = color.WithAlpha(255);
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

        _foregroundPaint?.Dispose();
        _foregroundPaint = CreateForegroundPaint(e.Info.Width, e.Info.Height);

        canvas.DrawRect(new SKRect(0, 0, e.Info.Width, e.Info.Height), _foregroundPaint);
    }

    /// <summary>
    /// Returns the foreground paint of the selected color.
    /// </summary>
    /// <param name="width">Width to draw.</param>
    /// <param name="height">Height to draw.</param>
    /// <returns>The foreground paint of the selected color.</returns>
    private SKPaint CreateForegroundPaint(int width, int height)
    {
        return new SKPaint
        {
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                height > width
                    ? new SKPoint(0, height)
                    : new SKPoint(width, 0),
                [
                    new SKColor(_color.Red, _color.Green, _color.Blue, 255),
                    new SKColor(_color.Red, _color.Green, _color.Blue, 0)
                ],
                [0, 1],
                SKShaderTileMode.Clamp)
        };
    }

    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        _foregroundPaint?.Dispose();

        SizeChanged -= AlphaSelectionForeground_SizeChanged;
    }
}