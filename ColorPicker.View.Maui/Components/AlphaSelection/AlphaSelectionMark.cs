using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ColorPicker.View.Maui.Components;
public class AlphaSelectionMark : SKCanvasView, IDisposable
{
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
    /// Selection mark path.
    /// </summary>
    private SKPath _selectionMarkPath;

    /// <summary>
    /// Pain for selection mark background.
    /// </summary>
    private readonly SKPaint _selectionMarkPaint;

    /// <summary>
    /// Relative position of the mark selection on the x-axis.
    /// </summary>
    private float _relPositionX = 0;

    /// <summary>
    /// Relative position of the mark selection on the y-axis.
    /// </summary>
    private float _relPositionY = 0;

    /// <summary>
    /// Selected alpha
    /// </summary>
    private float _alpha;

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
    /// Initializes a new instance of the AlphaSelectionMark class.
    /// </summary>
    public AlphaSelectionMark()
    {
        _selectionMarkPaint = new SKPaint 
        { 
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        SizeChanged += AlphaSelectionMark_SizeChanged;
        Touch += AlphaSelectionMark_Touch;

        EnableTouchEvents = true;
    }

    /// <summary>
    /// Sets the alpha value.
    /// </summary>
    /// <param name="alpha">Alpha value to set.</param>
    internal void SetAlpha(float alpha)
    {
        if (_alpha == alpha)
            return;

        _alpha = alpha;

        InvalidateSurface();
    }

    /// <summary>
    /// Sets the new alpha value.
    /// </summary>
    /// <param name="alpha">Alpha value to set.</param>
    private void SetNewAlpha(float alpha)
    {
        _viewModel.Alpha.Alpha = _alpha = alpha;
    }

    /// <summary>
    /// Occurs when the size of an element changed.
    /// </summary>
    private void AlphaSelectionMark_SizeChanged(object? sender, EventArgs e)
    {
        InvalidateSurface();
    }

    /// <summary>
    /// Occurs when a touch interaction is detected on the control.
    /// </summary>
    private void AlphaSelectionMark_Touch(object? sender, SKTouchEventArgs e)
    {
        if (e.ActionType == SKTouchAction.Pressed)
        {
            _isTouchPressed = true;

            if (_height > _width)
                SetLocationY(e.Location.Y);
            else
                SetLocationX(e.Location.X);

            InvalidateSurface();
        }
        else if (e.ActionType == SKTouchAction.Moved)
        {
            if (_isTouchPressed)
            {
                if (_height > _width)
                    SetLocationY(e.Location.Y);
                else
                    SetLocationX(e.Location.X);

                InvalidateSurface();
            }
        }
        else if (e.ActionType == SKTouchAction.Released)
        {
            _isTouchPressed = false;
        }

        e.Handled = true;
    }

    /// <summary>
    /// Sets the location X from touch.
    /// </summary>
    /// <param name="locationX">Location on the x-axis.</param>
    private void SetLocationX(float locationX)
    {
        float x = Math.Min(Math.Max(0, locationX), _width);
        float relPositionX = x / _width;

        if (Math.Abs(_relPositionX - relPositionX) > 0.001)
        {
            _relPositionX = relPositionX;

            SetNewAlpha((float)(1 - _relPositionX));
        }
    }

    /// <summary>
    /// Sets the location Y from touch.
    /// </summary>
    /// <param name="locationY">Location on the y-axis.</param>
    private void SetLocationY(float locationY)
    {
        float y = Math.Min(Math.Max(0, locationY), _height);
        float relPositionY = y / _height;

        if (Math.Abs(_relPositionY - relPositionY) > 0.001)
        {
            _relPositionY = relPositionY;

            SetNewAlpha((float)(1 - _relPositionY));
        }
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

        if (_width != e.Info.Width || _height != e.Info.Height)
        {
            // For touch event
            _width = e.Info.Width;
            _height = e.Info.Height;

            // For the first draw
            if (_relPositionX == 0 && _relPositionY == 0)
            {
                if (e.Info.Width > e.Info.Height)
                {
                    _relPositionX = _alpha;
                }
                else
                {
                    _relPositionY = _alpha;
                }
            }
        }

        if (_selectionMarkPath == null)
        {
            CreateSelectionMark(e.Info.Width, e.Info.Height);
        }

        canvas.Translate(_relPositionX * e.Info.Width, _relPositionY * e.Info.Height);
        canvas.DrawPath(_selectionMarkPath, _selectionMarkPaint);
    }

    /// <summary>
    /// Creates the selection mark.
    /// </summary>
    /// <param name="width">Width to draw.</param>
    /// <param name="height">Height to draw.</param>
    private void CreateSelectionMark(int width, int height)
    {
        if (width > height)
        {
            float size = _height;

            var path1 = new SKPath();
            path1.MoveTo(0 - size / 3, 0);
            path1.LineTo(0, size / 3);
            path1.LineTo(size / 3, 0);
            path1.Close();

            var path2 = new SKPath();
            path1.MoveTo(0 - size / 3, size);
            path1.LineTo(0, 2 * size / 3);
            path1.LineTo(size / 3, size);
            path2.Close();

            var combined = new SKPath();
            combined.AddPath(path1);
            combined.AddPath(path2);

            _selectionMarkPath = combined;
        }
        else
        {
            float size = _width;

            var path1 = new SKPath();
            path1.MoveTo(0, 0 - size / 3);
            path1.LineTo(size / 3, 0);
            path1.LineTo(0, size / 3);
            path1.Close();

            var path2 = new SKPath();
            path1.MoveTo(size, 0 - size / 3);
            path1.LineTo(2 * size / 3, 0);
            path1.LineTo(size, size / 3);
            path2.Close();

            var combined = new SKPath();
            combined.AddPath(path1);
            combined.AddPath(path2);

            _selectionMarkPath = combined;
        }
    }

    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        _selectionMarkPaint?.Dispose();

        SizeChanged -= AlphaSelectionMark_SizeChanged;
        Touch -= AlphaSelectionMark_Touch;
    }
}