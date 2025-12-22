//using ColorPicker.Core.Utilities;
//using ColorPicker.View.Wpf.Controls;
//using System;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;

//namespace ColorPicker.View.Wpf.Elements
//{
//    internal class HueSelection : IDisposable
//    {
//        private readonly Canvas _hueCanvas;

//        /// <summary>
//        /// Main view model.
//        /// </summary>
//        private readonly ColorPickerViewModel _viewModel;

//        /// <summary>
//        /// Markers for hue selection.
//        /// </summary>
//        private HueMarker _hueMarker;

//        /// <summary>
//        /// Initializes a new instance of the HueSelection class.
//        /// </summary>
//        /// <param name="canvas">Canvas to hue vizualization.</param>
//        /// <param name="internalResources">Internal styles.</param>
//        /// <param name="viewModel">Main view model.</param>
//        public HueSelection(Canvas canvas, ResourceDictionary internalResources, ColorPickerViewModel viewModel)
//        {
//            _hueCanvas = canvas;
//            _viewModel = viewModel;


//            _hueCanvas.PreviewMouseLeftButtonDown -= HueCanvas_PreviewMouseLeftButtonDown;
//            _hueCanvas.PreviewMouseLeftButtonUp -= HueCanvas_PreviewMouseLeftButtonUp;
//            _hueCanvas.MouseMove -= HueCanvas_MouseMove;
//            _hueCanvas.SizeChanged -= HueCanvas_SizeChanged;


//            _hueCanvas.Children.Clear();
//            _hueCanvas.ClipToBounds = true;
//            _hueCanvas.PreviewMouseLeftButtonDown += HueCanvas_PreviewMouseLeftButtonDown;
//            _hueCanvas.PreviewMouseLeftButtonUp += HueCanvas_PreviewMouseLeftButtonUp;
//            _hueCanvas.MouseMove += HueCanvas_MouseMove;
//            _hueCanvas.SizeChanged += HueCanvas_SizeChanged;

//            _hueMarker = new HueMarker();

//            _hueCanvas.Children.Add(_hueMarker);

//            _hueMarker.Loaded += (sender, e) =>
//            {
//                LinearGradientBrush brush;

//                if (_hueCanvas.ActualWidth > _hueCanvas.ActualHeight)
//                {
//                    brush = internalResources["HorizontalHueBrush"] as LinearGradientBrush;
//                }
//                else
//                {
//                    brush = internalResources["VerticalHueBrush"] as LinearGradientBrush;
//                }

//                if (brush != null)
//                {
//                    brush.Freeze();
//                    _hueCanvas.Background = brush;
//                }

//                if (_hueCanvas.ActualWidth > _hueCanvas.ActualHeight)
//                {
//                    _hueMarker.RenderTransformOrigin = new Point(0.5 * 0.65d, 0.5);

//                    var transformGroup = new TransformGroup();
//                    transformGroup.Children.Add(new RotateTransform { Angle = 90 });

//                    _hueMarker.RenderTransform = transformGroup;
//                }

//                if (_hueCanvas.ActualWidth > _hueCanvas.ActualHeight)
//                {
//                    _hueMarker.Width = _hueCanvas.ActualHeight;
//                    _hueMarker.Height = _hueCanvas.ActualHeight * 0.65d;
//                }
//                else
//                {
//                    _hueMarker.Width = _hueCanvas.ActualWidth;
//                    _hueMarker.Height = _hueCanvas.ActualWidth * 0.65d;
//                }

//                MoveHueMarker(_viewModel.Hsv.Hue);
//            };
//        }

//        /// <summary>
//        /// Hue selection mouse left button down event.
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void HueCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
//        {
//            ChangeHueComponent(sender as Canvas, e.GetPosition(sender as Canvas));
//            Mouse.Capture(sender as Canvas);
//        }

//        /// <summary>
//        /// Hue selection mouse left button up event.
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void HueCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
//        {
//            Mouse.Capture(null);
//        }

//        /// <summary>
//        /// Hue selection mouse move event.
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void HueCanvas_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (Mouse.LeftButton == MouseButtonState.Pressed)
//                ChangeHueComponent(sender as Canvas, e.GetPosition(sender as Canvas));
//        }

//        /// <summary>
//        /// Hue selection size change event.
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void HueCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
//        {
//            MoveHueMarker(_viewModel.Hsv.Hue);
//        }

//        /// <summary>
//        /// Moves the hue marker.
//        /// </summary>
//        /// <param name="hue">Hue</param>
//        internal void MoveHueMarker(double hue)
//        {
//            if (_hueMarker != null && _hueMarker.IsLoaded)
//            {
//                if (_hueCanvas.ActualWidth > _hueCanvas.ActualHeight)
//                {
//                    double x = _hueCanvas.ActualWidth * hue / 360d;

//                    Canvas.SetLeft(_hueMarker, x - _hueMarker.Width * 0.65d / 2);
//                }
//                else
//                {
//                    double y = _hueCanvas.ActualHeight * hue / 360d;

//                    Canvas.SetTop(_hueMarker, y - _hueMarker.Height / 2 + 1);
//                }
//            }
//        }

//        /// <summary>
//        /// Called when user interacts with hue canvases.
//        /// Recomputes hue from sampled pixel and updates ViewModel.
//        /// </summary>
//        private void ChangeHueComponent(Canvas canvas, Point point)
//        {
//            if (canvas == null) return;

//            int hue;

//            if (_hueCanvas.ActualHeight > _hueCanvas.ActualWidth)
//            {
//                hue = (int)((point.Y / canvas.ActualHeight) * 360d);
//            }
//            else
//            {
//                hue = (int)((point.X / canvas.ActualWidth) * 360d);
//            }

//            hue = Math.Max(0, hue);
//            hue = Math.Min(360, hue);

//            if (hue != (int)_viewModel.Hsv.Hue)
//            {
//                _viewModel.Hsv.Hue = _lastHue = hue;

//                RenderColorSelectionCanvas();

//                MoveHueMarker(_viewModel.Hsv.Hue);
//            }
//        }

//        /// <summary>
//        /// Render saturation/value surface for current hue into a pixel buffer and that write to image.
//        /// </summary>
//        private void RenderColorSelectionCanvas()
//        {
//            if (_colorSelectionCanvas == null || _colorSelectionImage == null) return;

//            int width = (int)_colorSelectionCanvas.ActualWidth;
//            int height = (int)_colorSelectionCanvas.ActualHeight;

//            if (width <= 0 || height <= 0) return;

//            if (_colorSelectionWidth == width && _colorSelectionHeight == height && _lastRenderedSelectedHueValue == _viewModel.Hsv.Hue && _renderedPixels != null && _lastRenderedSelectedHueBitmap != null)
//            {
//                return;
//            }

//            if (_colorSelectionWidth != width || _colorSelectionHeight != height || _renderedPixels == null)
//            {
//                _colorSelectionWidth = width;
//                _colorSelectionHeight = height;
//                _renderedPixels = new byte[width * height * 4];
//                _lastRenderedSelectedHueBitmap = null;
//            }

//            int index = 0;
//            float hue = _viewModel.Hsv.Hue;

//            for (int y = 0; y < height; y++)
//            {
//                float v = 1.0f - (float)y / (height - 1);
//                for (int x = 0; x < width; x++)
//                {
//                    float s = (float)x / (width - 1);

//                    ColorConversions.HsvToRgb(hue, s, v, out byte r, out byte g, out byte b);

//                    _renderedPixels[index++] = b;
//                    _renderedPixels[index++] = g;
//                    _renderedPixels[index++] = r;
//                    _renderedPixels[index++] = 255; // alpha
//                }
//            }

//            if (_lastRenderedSelectedHueBitmap == null || _lastRenderedSelectedHueBitmap.PixelWidth != width || _lastRenderedSelectedHueBitmap.PixelHeight != height)
//            {
//                _lastRenderedSelectedHueBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
//                _colorSelectionImage.ImageSource = _lastRenderedSelectedHueBitmap;
//            }

//            _lastRenderedSelectedHueBitmap.WritePixels(new Int32Rect(0, 0, width, height), _renderedPixels, width * 4, 0);

//            _lastRenderedSelectedHueValue = _viewModel.Hsv.Hue;
//        }

//        /// <summary>
//        /// Releases all resources used by this instance.
//        /// </summary>
//        public void Dispose()
//        {
//            _hueCanvas.PreviewMouseLeftButtonDown -= HueCanvas_PreviewMouseLeftButtonDown;
//            _hueCanvas.PreviewMouseLeftButtonUp -= HueCanvas_PreviewMouseLeftButtonUp;
//            _hueCanvas.MouseMove -= HueCanvas_MouseMove;
//            _hueCanvas.SizeChanged -= HueCanvas_SizeChanged;
//        }
//    }
//}