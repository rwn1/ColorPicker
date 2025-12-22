using ColorPicker.Core.Utilities;
using ColorPicker.View.Maui.Models;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ColorPicker.View.Maui.Components;
internal class ColorSelectionMark : SKGLView, IDisposable
{
    /// <summary>
    /// Model for sharing data between the background and the mark.
    /// </summary>
    private readonly ShareDataModel _shareModel;

    /// <summary>
    /// Mark surface size.
    /// </summary>
    private float _selectionMarkSurfaceSize;

    /// <summary>
    /// Mark radius.
    /// </summary>
    private int _markRadius = 10;

    /// <summary>
    /// Relative position of the mark selection on the x-axis.
    /// </summary>
    private float _relPositionX = 0;

    /// <summary>
    /// Relative position of the mark selection on the y-axis.
    /// </summary>
    private float _relPositionY = 0;

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
    /// Paint for white part of the mark.
    /// </summary>
    private SKPaint _whitePain = null;

    /// <summary>
    /// Paint for black part of the mark.
    /// </summary>
    private SKPaint _blackPain = null;

    /// <summary>
    /// Selection mark surface.
    /// </summary>
    private SKSurface _selectionMarkSurface;

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
    /// Last red component value.
    /// </summary>
    private double _lastRed= -1;

    /// <summary>
    /// Last green component value.
    /// </summary>
    private double _lastGreen = -1;

    /// <summary>
    /// Last blue component value.
    /// </summary>
    private double _lastBlue = -1;

    /// <summary>
    /// Initializes a new instance of the ColorSelectionMark class.
    /// </summary>
    /// <param name="shareModel">Model for sharing data between the background and the mark.</param>
    public ColorSelectionMark(ShareDataModel shareModel)
    {
        _shareModel = shareModel;

        SizeChanged += ColorSelectionMark_SizeChanged;
        Touch += ColorSelectionMark_Touch;

        EnableTouchEvents = true;

        int strokeWidth = Math.Max(6, _markRadius) / 3;

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

    /// <summary>
    /// Occurs when the size of an element changed.
    /// </summary>
    private void ColorSelectionMark_SizeChanged(object? sender, EventArgs e)
    {
        InvalidateSurface();
    }

    /// <summary>
    /// Sets the selected color.
    /// </summary>
    /// <param name="r">Red component.</param>
    /// <param name="g">Green component.</param>
    /// <param name="b">Blue component.</param>
    internal void SetSelectedColor(byte r, byte g, byte b)
    {
        if (_lastRed == r && _lastGreen == g && _lastBlue == b)
            return;

        _lastRed = r;
        _lastGreen = g;
        _lastBlue = b;

        if (_width != 0 && _height != 0)
        {
            var (x, y) = GetPixelFromColor(r, g, b);

            _relPositionX = (float)x / _width;
            _relPositionY = (float)y / _height;

            InvalidateSurface();
        }
    }

    /// <summary>
    /// Sets the mark radius.
    /// </summary>
    /// <param name="markRadius">Mark radius to set.</param>
    internal void SetMarkRadius(int markRadius)
    {
        _markRadius = markRadius;
        _selectionMarkSurface = null;
        InvalidateSurface();
    }

    /// <summary>
    /// Occurs when a touch interaction is detected on the control.
    /// </summary>
    private void ColorSelectionMark_Touch(object? sender, SKTouchEventArgs e)
    {
        if (e.ActionType == SKTouchAction.Pressed)
        {
            _isTouchPressed = true;

            SetLocation(e.Location.X, e.Location.Y);
        }
        else if (e.ActionType == SKTouchAction.Moved)
        {
            if (_isTouchPressed)
            {
                SetLocation(e.Location.X, e.Location.Y);
            }
        }
        else if (e.ActionType == SKTouchAction.Released)
        {
            _isTouchPressed = false;
        }

        e.Handled = true;
    }

    /// <summary>
    /// Sets the location from touch.
    /// </summary>
    /// <param name="locationX">Location on the x-axis.</param>
    /// <param name="locationY">Location on the y-axis.</param>
    private void SetLocation(float locationX, float locationY)
    {
        float x = Math.Min(Math.Max(0, locationX), _width);
        float y = Math.Min(Math.Max(0, locationY), _height);

        float newX = x / _width;
        float newY = y / _height;

        if (Math.Abs(_relPositionX - newX) > 0.001 ||
            Math.Abs(_relPositionY - newY) > 0.001)
        {
            _relPositionX = newX;
            _relPositionY = newY;

            SetNewColor();

            InvalidateSurface();
        }
    }

    /// <summary>
    /// Called whenever the SkiaSharp surface needs to be redrawn.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        var canvas = e.Surface.Canvas;
        canvas.Clear();

        if (_width != e.Info.Width || _height != e.Info.Height)
        {
            // For touch event
            _width = e.Info.Width;
            _height = e.Info.Height;
        }

        // For the changed color
        if (_relPositionX == 0 && _relPositionY == 0)
        {
            var (x, y) = GetPixelFromColor(_viewModel.Hsv.Hue, _viewModel.Hsv.Saturation, _viewModel.Hsv.Value);

            _relPositionX = (float)x / _width;
            _relPositionY = (float)y / _height;
        }

        if (_selectionMarkSurface == null)
        {
            CreateSelectionMark(e.Info.Width, e.Info.Height);
        }

        canvas.DrawSurface(_selectionMarkSurface,
            _relPositionX * e.Info.Width - _selectionMarkSurfaceSize,
            _relPositionY * e.Info.Height - _selectionMarkSurfaceSize);
    }

    /// <summary>
    /// Creates the selection mark.
    /// </summary>
    /// <param name="width">Width to draw.</param>
    /// <param name="height">Height to draw.</param>
    private void CreateSelectionMark(int width, int height)
    {
        var info = new SKImageInfo(width, height);

        _selectionMarkSurface = SKSurface.Create(info);

        var canvas = _selectionMarkSurface.Canvas;
        canvas.Clear(SKColors.Transparent);

        int strokeWidth = Math.Max(6, _markRadius) / 3;

        _whitePain.StrokeWidth = strokeWidth;
        _blackPain.StrokeWidth = strokeWidth;

        int markRadius = Math.Max(2, _markRadius);

        _selectionMarkSurfaceSize = markRadius + 2 * strokeWidth;

        canvas.DrawCircle(_selectionMarkSurfaceSize, _selectionMarkSurfaceSize, markRadius - strokeWidth, _whitePain);
        canvas.DrawCircle(_selectionMarkSurfaceSize, _selectionMarkSurfaceSize, markRadius, _blackPain);
        canvas.DrawCircle(_selectionMarkSurfaceSize, _selectionMarkSurfaceSize, markRadius + strokeWidth, _whitePain);
    }

    /// <summary>
    /// Returns the color location from RGB.
    /// </summary>
    /// <param name="r">Red component.</param>
    /// <param name="g">Green component.</param>
    /// <param name="b">Blue component.</param>
    /// <param name="h">Hue component.</param>
    /// <param name="s">Saturation component.</param>
    /// <param name="v">Value component.</param>
    private (int x, int y) GetPixelFromColor(byte r, byte g, byte b)
    {
        ColorConversions.RgbToHsv(r, g, b, out float h, out float s, out float v);
        //var (h, s, v) = RgbToHsv(red, green, blue);
        return GetPixelFromColor(h, s, v);
    }

    /// <summary>
    /// Returns the color location from hsv.
    /// </summary>
    /// <param name="h">Hue.</param>
    /// <param name="s">Saturation.</param>
    /// <param name="v">Value.</param>
    /// <returns>The color location from hsv.</returns>
    private (int x, int y) GetPixelFromColor(float h, float s, float v)
    {
        int half = _height / 2;

        h = h % 360.0f;
        if (h < 0) h += 360.0f;

        int x = (int)Math.Round(h / 360.0 * (_width - 1));
        int y;

        const double eps = 0.0001;

        if (v >= 1.0 - eps)
        {
            y = (int)Math.Round(s * (half - 1));
        }
        else if (s >= 1.0 - eps)
        {
            y = half + (int)Math.Round((1.0 - v) * (half - 1));
        }
        else
        {
            if (v > s)
            {
                y = (int)Math.Round(s * (half - 1));
            }
            else
            {
                y = half + (int)Math.Round((1.0 - v) * (half - 1));
            }
        }

        x = Math.Clamp(x, 0, _width - 1);
        y = Math.Clamp(y, 0, _height - 1);

        return (x, y);
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

        b = _shareModel.Pixels[index + 0]; // B
        g = _shareModel.Pixels[index + 1]; // G
        r = _shareModel.Pixels[index + 2]; // R
        a = _shareModel.Pixels[index + 3]; // A
    }

    /// <summary>
    /// Sets the new color.
    /// </summary>
    private void SetNewColor()
    {
        if (_shareModel.Pixels == null)
            return;

        // Clicked position
        int x = (int)(_relPositionX * _width);
        int y = (int)(_relPositionY * _height);

        if (x < 0 || y < 0 || x >= _width || y >= _height)
            return;

        // Bytes: B,G,R,A
        byte r, g, b, a;
        GetRGBFromBitmap(x, y, out r, out g, out b, out a);

        _lastRed = r;
        _lastGreen = g;
        _lastBlue = b;

        _viewModel.SelectColor(r, g, b, _viewModel.Alpha.Alpha);
    }

    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        _whitePain?.Dispose();        
        _blackPain?.Dispose();        
        _selectionMarkSurface?.Dispose();

        SizeChanged -= ColorSelectionMark_SizeChanged;
        Touch -= ColorSelectionMark_Touch;
    }
}