using ColorPicker.Core.Utilities;
using ColorPicker.View.Wpf.Controls;
using ColorPicker.View.Wpf.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColorPicker.View.Wpf.Elements
{
    /// <summary>
    /// Handles saturation/value (SV) selection and visualization for the ColorPicker.
    /// Responsible for rendering the SV bitmap and interpreting mouse interactions.
    /// </summary>
    internal class ColorSelection : IDisposable
    {
        private readonly Canvas _colorSelectionView;
        private readonly ShareDataModel _shareDataModel;
        private readonly ColorPickerViewModel _viewModel;

        private ImageBrush _svImageBrush;
        private ColorSelectionMark _marker;

        /// <summary>
        /// Initializes a new instance of the ColorSelection class.
        /// </summary>
        public ColorSelection(Canvas colorSelectionView, ShareDataModel shareDataModel, ColorPickerViewModel viewModel)
        {
            _colorSelectionView = colorSelectionView;
            _shareDataModel = shareDataModel;
            _viewModel = viewModel;

            InitializeCanvas();
            InitializeImageBrush();
            InitializeMarker();

            _colorSelectionView.PreviewMouseLeftButtonDown += OnMouseDown;
            _colorSelectionView.PreviewMouseLeftButtonUp += OnMouseUp;
            _colorSelectionView.PreviewMouseMove += OnMouseMove;
            _colorSelectionView.SizeChanged += OnSizeChanged;
        }

        /// <summary>
        /// Configures the canvas for SV rendering.
        /// </summary>
        private void InitializeCanvas()
        {
            _colorSelectionView.Children.Clear();
            _colorSelectionView.ClipToBounds = true;
        }

        /// <summary>
        /// Creates and assigns the ImageBrush used to display the SV bitmap.
        /// </summary>
        private void InitializeImageBrush()
        {
            _svImageBrush = new ImageBrush();
            _colorSelectionView.Background = _svImageBrush;
        }

        /// <summary>
        /// Creates the draggable color selection marker.
        /// </summary>
        private void InitializeMarker()
        {
            _marker = new ColorSelectionMark();
            _marker.Loaded += OnMarkerLoaded;
            _colorSelectionView.Children.Add(_marker);
        }

        /// <summary>
        /// Positions the marker according to the current HSV values.
        /// </summary>
        private void OnMarkerLoaded(object sender, RoutedEventArgs e)
        {
            double x = _viewModel.Hsv.Saturation * (_colorSelectionView.ActualWidth - 1);
            double y = (1 - _viewModel.Hsv.Value) * (_colorSelectionView.ActualHeight - 1);

            MoveColorSelectionMark(x, y);
        }

        /// <summary>
        /// Moves the visual selection marker to an SV coordinate.
        /// </summary>
        internal void MoveColorSelectionMark(double x, double y)
        {
            if (!_marker.IsLoaded) return;

            Canvas.SetLeft(_marker, x - _marker.ActualWidth / 2);
            Canvas.SetTop(_marker, y - _marker.ActualHeight / 2);
        }

        /// <summary>
        /// Handles left mouse button press on color selection canvas.
        /// Starts color selection and captures mouse input.
        /// </summary>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeMarkerPosition(e.GetPosition(_colorSelectionView));
            Mouse.Capture(_colorSelectionView);
        }

        /// <summary>
        /// Releases mouse capture when user releases the left mouse button.
        /// </summary>
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        /// <summary>
        /// Handles selection drag behavior when the mouse moves while pressed.
        /// </summary>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                ChangeMarkerPosition(e.GetPosition(_colorSelectionView));
        }

        /// <summary>
        /// Moves color selection marker when canvas size changes.
        /// </summary>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            RenderColorSelection(sizeChanged: true);

            double x = _viewModel.Hsv.Saturation * (_colorSelectionView.ActualWidth - 1);
            double y = (1 - _viewModel.Hsv.Value) * (_colorSelectionView.ActualHeight - 1);

            MoveColorSelectionMark(x, y);
        }

        /// <summary>
        /// Handles user interaction on the SV surface.
        /// Calculates SV from the pixel buffer at the clicked location.
        /// </summary>
        internal void ChangeMarkerPosition(Point point)
        {
            double x = Math.Min(Math.Max(0, point.X), _colorSelectionView.ActualWidth);
            double y = Math.Min(Math.Max(0, point.Y), _colorSelectionView.ActualHeight);

            MoveColorSelectionMark(x, y);

            double saturation = x / _colorSelectionView.ActualWidth;
            double value = y / _colorSelectionView.ActualHeight;

            _viewModel.SelectColor(_viewModel.Hsv.Hue, (float)saturation, (float)(1 - value), _viewModel.Alpha.Alpha);
        }

        /// <summary>
        /// Renders the saturation/value rectangle for the currently selected hue.
        /// Stores it into a pixel buffer and applies it as the ImageBrush texture.
        /// </summary>
        internal void RenderColorSelection(bool sizeChanged = false)
        {
            if (_colorSelectionView == null || _svImageBrush == null) return;

            int width = (int)_colorSelectionView.ActualWidth;
            int height = (int)_colorSelectionView.ActualHeight;

            if (width <= 0 || height <= 0) return;

            var pixelBuffer = new byte[width * height * 4];

            float hue = _viewModel.Hsv.Hue;
            int index = 0;

            for (int y = 0; y < height; y++)
            {
                float v = 1f - (float)y / (height - 1);

                for (int x = 0; x < width; x++)
                {
                    float s = (float)x / (width - 1);

                    ColorConversions.HsvToRgb(hue, s, v, out byte r, out byte g, out byte b);

                    pixelBuffer[index++] = b;
                    pixelBuffer[index++] = g;
                    pixelBuffer[index++] = r;
                    pixelBuffer[index++] = 255;
                }
            }

            var bmp = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            _svImageBrush.ImageSource = bmp;

            bmp.WritePixels(new Int32Rect(0, 0, width, height), pixelBuffer, width * 4, 0);
            bmp.Freeze();
        }

        /// <summary>
        /// Releases all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            _marker.Loaded -= OnMarkerLoaded;

            _colorSelectionView.PreviewMouseLeftButtonDown -= OnMouseDown;
            _colorSelectionView.PreviewMouseLeftButtonUp -= OnMouseUp;
            _colorSelectionView.PreviewMouseMove -= OnMouseMove;
            _colorSelectionView.SizeChanged -= OnSizeChanged;
        }
    }
}