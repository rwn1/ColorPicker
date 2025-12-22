using ColorPicker.Core.Utilities;
using ColorPicker.View.Maui.Models;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ColorPicker.View.Maui.Components;
internal class ColorSelectionBackground : SKGLView, IDisposable
{
    /// <summary>
    /// Model for sharing data between the background and the mark.
    /// </summary>
    private readonly ShareDataModel _shareModel;

    /// <summary>
    /// Cached background bitmap.
    /// </summary>
    private SKBitmap _backgroundBitmap;

    /// <summary>
    /// Initializes a new instance of the ColorSelectionBackground class.
    /// </summary>
    /// <param name="shareModel">Model for sharing data between the background and the mark.</param>
    public ColorSelectionBackground(ShareDataModel shareModel)
    {
        _shareModel = shareModel;

        SizeChanged += ColorSelectionBackground_SizeChanged;
    }

    /// <summary>
    /// Occurs when the size of an element changed.
    /// </summary>
    private void ColorSelectionBackground_SizeChanged(object? sender, EventArgs e)
    {
        InvalidateSurface();
    }

    /// <summary>
    /// Called whenever the SkiaSharp surface needs to be redrawn.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        var canvas = e.Surface.Canvas;

        _backgroundBitmap = CreateBitmap(e.Info.Width, e.Info.Height);

        canvas.DrawBitmap(_backgroundBitmap, 0, 0);
    }

    /// <summary>
    /// Returns the bitmap of the color palette.
    /// </summary>
    /// <param name="width">Width to draw.</param>
    /// <param name="height">Height to draw.</param>
    /// <returns>The bitmap of the color palette.</returns>
    private SKBitmap CreateBitmap(int width, int height)
    {
        var bitmap = new SKBitmap(
              width, height,
              SKColorType.Bgra8888,
              SKAlphaType.Premul);

        Span<byte> pixels = bitmap.GetPixelSpan();
        int index = 0;

        int half = height / 2;

        for (int y = 0; y < height; y++)
        {
            bool top = y < half;

            for (int x = 0; x < width; x++)
            {
                float h = (float)x / (width - 1) * 360.0f;

                float s, v;

                if (top)
                {
                    s = (float)y / (half - 1);
                    v = 1.0f;
                }
                else
                {
                    s = 1.0f;
                    v = 1.0f - (float)(y - half) / (half - 1);
                }

                ColorConversions.HsvToRgb(h, s, v, out byte r, out byte g, out byte b);

                pixels[index++] = b;
                pixels[index++] = g;
                pixels[index++] = r;
                pixels[index++] = 255;
            }
        }

        _shareModel.Pixels = pixels.ToArray();
        return bitmap;
    }

    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        _backgroundBitmap?.Dispose();

        SizeChanged -= ColorSelectionBackground_SizeChanged;
    }
}