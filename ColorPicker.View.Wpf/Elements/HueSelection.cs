using ColorPicker.View.Wpf.Controls;
using ColorPicker.View.Wpf.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ColorPicker.View.Wpf.Elements
{
    /// <summary>
    /// Handles hue selection UI logic, including rendering, interaction, and coordination with the main view model.
    /// </summary>
    internal class HueSelection : IDisposable
    {
        private readonly Canvas _hueSelectionView;
        private readonly ColorPickerViewModel _viewModel;
        private readonly ShareDataModel _shareDataModel;
        private readonly ResourceDictionary _internalResources;
        private readonly TriangleSelectionMarker _hueSelectionMarker;
        private readonly ColorSelection _colorSelection;

        /// <summary>
        /// Initializes a new instance of the HueSelection class.
        /// </summary>
        public HueSelection(Canvas hueSelectionView, ColorSelection colorSelection, ShareDataModel shareDataModel, ColorPickerViewModel viewModel, ResourceDictionary internalResources)
        {
            _hueSelectionView = hueSelectionView;
            _colorSelection = colorSelection;
            _shareDataModel = shareDataModel;
            _viewModel = viewModel;
            _internalResources = internalResources;

            _hueSelectionView.Children.Clear();
            _hueSelectionView.ClipToBounds = true;

            _hueSelectionMarker = new TriangleSelectionMarker();
            _hueSelectionMarker.Loaded += OnMarkerLoaded;

            _hueSelectionView.Children.Add(_hueSelectionMarker);

            _hueSelectionView.PreviewMouseLeftButtonDown += OnMouseDown;
            _hueSelectionView.PreviewMouseLeftButtonUp += OnMouseUp;
            _hueSelectionView.MouseMove += OnMouseMove;
            _hueSelectionView.SizeChanged += OnSizeChanged;
        }

        /// <summary>
        /// Handles left mouse button press on hue canvas.
        /// Starts hue selection and captures mouse input.
        /// </summary>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;
            ChangeHueComponent(e.GetPosition(canvas));
            Mouse.Capture(canvas);
        }

        /// <summary>
        /// Releases mouse capture when user releases the left mouse button.
        /// </summary>
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        /// <summary>
        /// Handles hue drag behavior when the mouse moves while pressed.
        /// </summary>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                ChangeHueComponent(e.GetPosition(sender as Canvas));
            }
        }

        /// <summary>
        /// Called once the marker is visualized; configures gradient brush,
        /// orientation, size, and moves marker to correct hue.
        /// </summary>
        private void OnMarkerLoaded(object sender, RoutedEventArgs e)
        {
            bool isHorizontal = _hueSelectionView.ActualWidth > _hueSelectionView.ActualHeight;

            // Choose appropriate brush based on orientation
            var brushKey = isHorizontal ? "HorizontalHueBrush" : "VerticalHueBrush";
            var brush = _internalResources[brushKey] as LinearGradientBrush;

            if (brush != null)
            {
                brush.Freeze();
                _hueSelectionView.Background = brush;
            }

            // Rotate marker when used horizontally
            if (isHorizontal)
            {
                _hueSelectionMarker.RenderTransformOrigin = new Point(0.5 * 0.65, 0.5);

                var transform = new TransformGroup();
                transform.Children.Add(new RotateTransform { Angle = 90 });
                _hueSelectionMarker.RenderTransform = transform;

                _hueSelectionMarker.Width = _hueSelectionView.ActualHeight;
                _hueSelectionMarker.Height = _hueSelectionView.ActualHeight * 0.65;
            }
            else
            {
                _hueSelectionMarker.Width = _hueSelectionView.ActualWidth;
                _hueSelectionMarker.Height = _hueSelectionView.ActualWidth * 0.65;
            }

            MoveHueMarker(_viewModel.Hsv.Hue);
        }

        /// <summary>
        /// Moves hue marker when canvas size changes.
        /// </summary>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MoveHueMarker(_viewModel.Hsv.Hue);
        }

        /// <summary>
        /// Called when the user interacts with the hue canvas.
        /// Calculates hue based on cursor position.
        /// </summary>
        private void ChangeHueComponent(Point point)
        {
            bool isVertical = _hueSelectionView.ActualHeight > _hueSelectionView.ActualWidth;

            int hue = isVertical
                ? (int)(point.Y / _hueSelectionView.ActualHeight * 360d)
                : (int)(point.X / _hueSelectionView.ActualWidth * 360d);

            // Clamp without Math.Clamp (older C# compatibility)
            hue = Math.Min(360, Math.Max(0, hue));

            if (hue != (int)_viewModel.Hsv.Hue)
            {
                _viewModel.Hsv.Hue = _shareDataModel.LastHue = hue;

                _colorSelection.RenderColorSelection();
                MoveHueMarker(hue);
            }
        }

        /// <summary>
        /// Moves marker based on hue value.
        /// Handles both horizontal and vertical orientations.
        /// </summary>
        internal void MoveHueMarker(float hue)
        {
            if (_hueSelectionMarker == null || !_hueSelectionMarker.IsLoaded)
                return;

            bool isHorizontal = _hueSelectionView.ActualWidth > _hueSelectionView.ActualHeight;

            if (isHorizontal)
            {
                double x = _hueSelectionView.ActualWidth * hue / 360d;
                Canvas.SetLeft(_hueSelectionMarker, x - _hueSelectionMarker.Width * 0.65 / 2);
            }
            else
            {
                double y = _hueSelectionView.ActualHeight * hue / 360d;
                Canvas.SetTop(_hueSelectionMarker, y - _hueSelectionMarker.Height / 2 + 1);
            }
        }

        /// <summary>
        /// Releases all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            _hueSelectionView.PreviewMouseLeftButtonDown -= OnMouseDown;
            _hueSelectionView.PreviewMouseLeftButtonUp -= OnMouseUp;
            _hueSelectionView.MouseMove -= OnMouseMove;
            _hueSelectionView.SizeChanged -= OnSizeChanged;

            _hueSelectionMarker.Loaded -= OnMarkerLoaded;
        }
    }
}