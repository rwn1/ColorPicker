using ColorPicker.View.Wpf.Controls;
using ColorPicker.View.Wpf.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ColorPicker.View.Wpf.Elements
{
    /// <summary>
    /// Handles alpha (transparency) selection logic and visualization inside the ColorPicker.
    /// </summary>
    internal class AlphaSelection : IDisposable
    {
        private const double MarkerAspectRatio = 0.65d;

        private readonly Canvas _alphaSelectionView;
        private readonly ColorPickerViewModel _viewModel;
        private readonly ShareDataModel _shareDataModel;
        private readonly ResourceDictionary _resources;

        private readonly Border _alphaForeground;
        private readonly TriangleSelectionMarker _marker;

        /// <summary>
        /// Initializes a new instance of the AlphaSelection class.
        /// </summary>
        public AlphaSelection(Canvas alphaSelectionView, ShareDataModel shareDataModel, ColorPickerViewModel viewModel, ResourceDictionary resources)
        {
            _alphaSelectionView = alphaSelectionView;
            _shareDataModel = shareDataModel;
            _viewModel = viewModel;
            _resources = resources;

            InitializeCanvas();

            _alphaForeground = CreateForeground();
            _marker = CreateMarker();

            _alphaSelectionView.PreviewMouseLeftButtonDown += OnMouseDown;
            _alphaSelectionView.PreviewMouseLeftButtonUp += OnMouseUp;
            _alphaSelectionView.MouseMove += OnMouseMove;
            _alphaSelectionView.SizeChanged += OnSizeChanged;
        }

        /// <summary>
        /// Cleans and initializes the canvas.
        /// </summary>
        private void InitializeCanvas()
        {
            _alphaSelectionView.Children.Clear();
            _alphaSelectionView.ClipToBounds = true;
        }

        /// <summary>
        /// Creates the foreground overlay responsible for the alpha gradient.
        /// </summary>
        private Border CreateForeground()
        {
            var border = new Border();
            _alphaSelectionView.Children.Add(border);

            BindSize(border, FrameworkElement.WidthProperty, nameof(_alphaSelectionView.ActualWidth));
            BindSize(border, FrameworkElement.HeightProperty, nameof(_alphaSelectionView.ActualHeight));

            return border;
        }

        /// <summary>
        /// Creates the interactive alpha selection marker.
        /// </summary>
        private TriangleSelectionMarker CreateMarker()
        {
            var marker = new TriangleSelectionMarker();
            marker.Loaded += OnMarkerLoaded;
            _alphaSelectionView.Children.Add(marker);
            return marker;
        }

        /// <summary>
        /// Helper method to bind element width or height to the canvas size.
        /// </summary>
        private void BindSize(FrameworkElement element, DependencyProperty property, string path)
        {
            element.SetBinding(property, new Binding(path) { Source = _alphaSelectionView });
        }

        /// <summary>
        /// Handles left mouse button press on alpha canvas.
        /// Starts alpha selection and captures mouse input.
        /// </summary>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeAlpha(e.GetPosition(_alphaSelectionView));
            Mouse.Capture(_alphaSelectionView);
        }

        /// <summary>
        /// Releases mouse capture when user releases the left mouse button.
        /// </summary>
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        /// <summary>
        /// Handles alpha drag behavior when the mouse moves while pressed.
        /// </summary>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                ChangeAlpha(e.GetPosition(_alphaSelectionView));
        }

        /// <summary>
        /// Moves alpha marker when canvas size changes.
        /// </summary>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MoveAlphaMarker(_viewModel.Alpha.Alpha);
        }

        /// <summary>
        /// Called when the marker is loaded and ready for layout.
        /// </summary>
        private void OnMarkerLoaded(object sender, RoutedEventArgs e)
        {
            SetupCheckerBackground();
            SetupMarkerOrientation();
            ResizeMarker();
            MoveAlphaMarker(_viewModel.Alpha.Alpha);
        }

        /// <summary>
        /// Sets the background checkerboard pattern.
        /// </summary>
        private void SetupCheckerBackground()
        {
            if (_resources["SquareBrush"] is DrawingBrush brush)
            {
                brush.Freeze();
                _alphaSelectionView.Background = brush;
            }
        }

        /// <summary>
        /// Sets rotation and origin of the marker depending on horizontal/vertical orientation.
        /// </summary>
        private void SetupMarkerOrientation()
        {
            bool horizontal = IsHorizontal();
            _marker.RenderTransformOrigin = horizontal ? new Point(0.5 * MarkerAspectRatio, 0.5) : new Point(0.5, 0.5);

            if (horizontal)
            {
                _marker.RenderTransform = new TransformGroup
                {
                    Children = { new RotateTransform { Angle = 90 } }
                };
            }
        }

        /// <summary>
        /// Adjusts marker dimensions to match orientation.
        /// </summary>
        private void ResizeMarker()
        {
            bool horizontal = IsHorizontal();

            if (horizontal)
            {
                _marker.Width = _alphaSelectionView.ActualHeight;
                _marker.Height = _alphaSelectionView.ActualHeight * MarkerAspectRatio;
            }
            else
            {
                _marker.Width = _alphaSelectionView.ActualWidth;
                _marker.Height = _alphaSelectionView.ActualWidth * MarkerAspectRatio;
            }
        }

        /// <summary>
        /// Calculates and applies a new alpha value based on user interaction.
        /// </summary>
        private void ChangeAlpha(Point point)
        {
            float alpha = CalculateAlpha(point);

            if (Math.Abs(alpha - _viewModel.Alpha.Alpha) < float.Epsilon)
                return;

            SetNewAlpha(alpha);
            MoveAlphaMarker(alpha);
        }

        /// <summary>
        /// Converts cursor position into alpha value between 0 and 1.
        /// </summary>
        private float CalculateAlpha(Point point)
        {
            double size = IsHorizontal() ? _alphaSelectionView.ActualWidth : _alphaSelectionView.ActualHeight;
            double pos = IsHorizontal() ? point.X : point.Y;

            float alpha = (float)(pos / size);
            alpha = Math.Min(1f, Math.Max(0f, alpha));

            return 1 - alpha;
        }

        /// <summary>
        /// Applies alpha to ViewModel and shared data storage.
        /// </summary>
        private void SetNewAlpha(float alpha)
        {
            _viewModel.Alpha.Alpha = _shareDataModel.LastAlpha = alpha;
        }

        /// <summary>
        /// Moves the alpha marker based on the current alpha value.
        /// </summary>
        internal void MoveAlphaMarker(float alpha)
        {
            if (_marker == null || !_marker.IsLoaded)
                return;

            double size = IsHorizontal() ? _alphaSelectionView.ActualWidth : _alphaSelectionView.ActualHeight;
            double pos = size * (1 - alpha);

            if (IsHorizontal())
            {
                Canvas.SetLeft(_marker, pos - _marker.Width * MarkerAspectRatio / 2);
            }
            else
            {
                Canvas.SetTop(_marker, pos - _marker.Height / 2 + 1);
            }
        }

        /// <summary>
        /// Determines whether the alpha bar is horizontal.
        /// </summary>
        private bool IsHorizontal() => _alphaSelectionView.ActualWidth > _alphaSelectionView.ActualHeight;

        /// <summary>
        /// Sets the alpha gradient background based on the selected color.
        /// </summary>
        internal void SetSelectedColor(Brush brush)
        {
            _alphaForeground.Background =
                CreateGradientBrush(brush, _alphaSelectionView.ActualWidth, _alphaSelectionView.ActualHeight);
        }

        /// <summary>
        /// Creates a linear gradient from opaque to transparent based on the given color.
        /// </summary>
        private Brush CreateGradientBrush(Brush inputBrush, double width, double height)
        {
            Color baseColor = (inputBrush as SolidColorBrush).Color;
            bool vertical = height > width;

            var gradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = vertical ? new Point(0, 1) : new Point(1, 0),
                MappingMode = BrushMappingMode.RelativeToBoundingBox
            };

            gradient.GradientStops.Add(new GradientStop(Color.FromArgb(255, baseColor.R, baseColor.G, baseColor.B), 0));
            gradient.GradientStops.Add(new GradientStop(Color.FromArgb(0, baseColor.R, baseColor.G, baseColor.B), 1));

            return gradient;
        }

        /// <summary>
        /// Releases all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            _alphaSelectionView.PreviewMouseLeftButtonDown -= OnMouseDown;
            _alphaSelectionView.PreviewMouseLeftButtonUp -= OnMouseUp;
            _alphaSelectionView.MouseMove -= OnMouseMove;
            _alphaSelectionView.SizeChanged -= OnSizeChanged;

            _marker.Loaded -= OnMarkerLoaded;
        }
    }
}