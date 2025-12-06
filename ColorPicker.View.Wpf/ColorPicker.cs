using ColorPicker.Core.Models;
using ColorPicker.Core.Properties;
using ColorPicker.Core.Utilities;
using ColorPicker.View.Wpf.Controls;
using ColorPicker.View.Wpf.Shared;
using ColorPicker.View.Wpf.Utilities.Converters;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ColorPicker.View.Wpf
{
    public class ColorPicker : Control
    {
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register(
                nameof(SelectedBrush),
                typeof(Brush),
                typeof(ColorPicker),
                new PropertyMetadata(
                    Brushes.Transparent,
                    OnSelectedBrushChanged
                )
            );

        /// <summary>
        /// Gets or sets the selected brush of color picker.
        /// </summary>
        public Brush SelectedBrush
        {
            get => (Brush)GetValue(SelectedBrushProperty);
            set => SetValue(SelectedBrushProperty, value);
        }

        private static void OnSelectedBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ColorPicker)d;
            var newBrush = (Brush)e.NewValue;

            control.OnSelectedBrushChanged(newBrush);
        }

        protected virtual void OnSelectedBrushChanged(Brush newBrush)
        {
            if (newBrush is SolidColorBrush solidBrush)
            {
                Color color = solidBrush.Color;
                _viewModel.Hex.Hex = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            }
        }

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ColorPicker),
                new FrameworkPropertyMetadata(typeof(ColorPicker)));

            // A solution for .NET projects that, unlike .NET Framework, cannot automatically load Generic.xaml from Themes folder.
            var dictionary = new ResourceDictionary
            {
                Source = new Uri("/ColorPicker.View.Wpf;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
            };
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        /// <summary>
        /// Main view model.
        /// </summary>
        private readonly ColorPickerViewModel _viewModel;

        /// <summary>
        /// Internal styles.
        /// </summary>
        private readonly ResourceDictionary _internalResources;

        /// <summary>
        /// Hex controls.
        /// </summary>
        private TextBox _hexTextBox;
        private Label _hexLabel;

        /// <summary>
        /// RGB controls.
        /// </summary>
        private Label _redLabel; private TextBox _redTextBox; private Slider _redSlider;
        private Label _greenLabel; private TextBox _greenTextBox; private Slider _greenSlider;
        private Label _blueLabel; private TextBox _blueTextBox; private Slider _blueSlider;

        /// <summary>
        /// Alpha controls.
        /// </summary>
        private Label _alphaLabel; private TextBox _alphaTextBox; private Slider _alphaSlider;

        /// <summary>
        /// HSV controls.
        /// </summary>
        private Label _hueLabel; private TextBox _hueTextBox; private Slider _hueSlider;
        private Label _saturationLabel; private TextBox _saturationTextBox; private Slider _saturationSlider;
        private Label _valueLabel; private TextBox _valueTextBox; private Slider _valueSlider;

        /// <summary>
        /// HSL controls.
        /// </summary>
        private Label _hslHueLabel; private TextBox _hslHueTextBox; private Slider _hslHueSlider;
        private Label _hslSaturationLabel; private TextBox _hslSaturationTextBox; private Slider _hslSaturationSlider;
        private Label _lightnessLabel; private TextBox _lightnessTextBox; private Slider _lightnessSlider;

        /// <summary>
        /// CMYK controls.
        /// </summary>
        private Label _cyanLabel; private TextBox _cyanTextBox; private Slider _cyanSlider;
        private Label _magentaLabel; private TextBox _magentaTextBox; private Slider _magentaSlider;
        private Label _yellowLabel; private TextBox _yellowTextBox; private Slider _yellowSlider;
        private Label _keyLabel; private TextBox _keyTextBox; private Slider _keySlider;

        /// <summary>
        /// Eye dropper button.
        /// </summary>
        private Button _eyedropperButton;

        /// <summary>
        /// Canvases for hue selection.
        /// </summary>
        private Canvas _horizontalHueCanvas;
        private Canvas _verticalHueCanvas;

        /// <summary>
        /// Canvas for color selection.
        /// </summary>
        private Canvas _colorSelectionCanvas;

        /// <summary>
        /// Background for selected color.
        /// </summary>
        private Rectangle _selectedColorBackground;

        /// <summary>
        /// Selected color.
        /// </summary>
        private Rectangle _selectedColor;

        /// <summary>
        /// Cached pixel buffer and writeable bitmap for saturation/value surface.
        /// </summary>
        private byte[] _renderedPixels;

        /// <summary>
        /// Image in canvas is used to render the saturation/value bitmap.
        /// </summary>
        private ImageBrush _colorSelectionImage;

        /// <summary>
        /// Size of color selection canvas.
        /// </summary>
        private int _colorSelectionWidth;
        private int _colorSelectionHeight;

        /// <summary>
        /// The last hexadecimal value before using the eye dropper.
        /// </summary>
        private string _lastHexValue = null;

        /// <summary>
        /// The last rendered data about the used hue.
        /// </summary>
        private WriteableBitmap _lastRenderedSelectedHueBitmap;
        private double _lastRenderedSelectedHueValue = -1;

        /// <summary>
        /// The latest HSV values from which the result was rendered.
        /// </summary>
        private double _lastHue = -1;
        private double _lastSaturation = -1;
        private double _lastValue = -1;

        /// <summary>
        /// Marker for color selection.
        /// </summary>
        private SelectionMark _selectionColorMarker;

        /// <summary>
        /// Markers for hue selection.
        /// </summary>
        private HueMarker _hueMarkerVertical;
        private HueMarker _hueMarkerHorizontal;

        /// <summary>
        /// Initializes a new instance of the ColorPicker class.
        /// </summary>
        public ColorPicker()
        {
            _internalResources = new ResourceDictionary
            {
                Source = new Uri("/ColorPicker.View.Wpf;component/Themes/Styles.xaml", UriKind.Relative)
            };
            _viewModel = new ColorPickerViewModel();
            _viewModel.Hsv.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(HsvModel.Hue))
                {
                    if (_colorSelectionCanvas != null && _colorSelectionImage != null)
                    {
                        // Prevention against changes from this class
                        if (_lastHue != _viewModel.Hsv.Hue)
                        {
                            RenderColorSelectionCanvas();

                            MoveHueMarker(_viewModel.Hsv.Hue);
                        }
                    }
                }
                else if (e.PropertyName == nameof(HsvModel.Saturation) || e.PropertyName == nameof(HsvModel.Value))
                {
                    // Prevention against changes from this class
                    if (_lastSaturation != _viewModel.Hsv.Saturation || _lastValue != _viewModel.Hsv.Value)
                    {
                        // Updates the last set values
                        _lastSaturation = _viewModel.Hsv.Saturation;
                        _lastValue = _viewModel.Hsv.Value;

                        double x = _viewModel.Hsv.Saturation * (_colorSelectionCanvas.ActualWidth - 1);
                        double y = (1 - _viewModel.Hsv.Value) * (_colorSelectionCanvas.ActualHeight - 1);

                        MoveColorSelectionMark(x, y);
                    }
                }
            };
            _viewModel.Hex.PropertyChanged += (sender, e) =>
            {
                SelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_viewModel.Hex.Hex));
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            #region Hex

            _hexLabel = GetPart<Label>("PART_HexLabel");
            BindLabel(_hexLabel, "HEX", Resource.Red);

            _hexTextBox = GetPart<TextBox>("PART_HexTextBox");
            BindTextBox(_hexTextBox, _viewModel.Hex, nameof(ColorPickerViewModel.Hex.Hex), Resource.Hex);

            #endregion

            #region RGB
            
            // Red
            _redLabel = GetPart<Label>("PART_RedLabel");
            BindLabel(_redLabel, "R", Resource.Red);

            _redTextBox = GetPart<TextBox>("PART_RedTextBox");
            BindTextBox(_redTextBox, _viewModel.Rgb, nameof(ColorPickerViewModel.Rgb.Red), Resource.Red);

            _redSlider = GetPart<Slider>("PART_RedSlider");
            BindSlider(_redSlider, _viewModel.Rgb, nameof(ColorPickerViewModel.Rgb.Red), 0, 255);

            // Green
            _greenLabel = GetPart<Label>("PART_GreenLabel");
            BindLabel(_greenLabel, "G", Resource.Green);

            _greenTextBox = GetPart<TextBox>("PART_GreenTextBox");
            BindTextBox(_greenTextBox, _viewModel.Rgb, nameof(ColorPickerViewModel.Rgb.Green), Resource.Green);

            _greenSlider = GetPart<Slider>("PART_GreenSlider");
            BindSlider(_greenSlider, _viewModel.Rgb, nameof(ColorPickerViewModel.Rgb.Green), 0, 255);

            // Blue
            _blueLabel = GetPart<Label>("PART_BlueLabel");
            BindLabel(_blueLabel, "B", Resource.Blue);

            _blueTextBox = GetPart<TextBox>("PART_BlueTextBox");
            BindTextBox(_blueTextBox, _viewModel.Rgb, nameof(ColorPickerViewModel.Rgb.Blue), Resource.Blue);

            _blueSlider = GetPart<Slider>("PART_BlueSlider");
            BindSlider(_blueSlider, _viewModel.Rgb, nameof(ColorPickerViewModel.Rgb.Blue), 0, 255);

            #endregion

            #region Alpha

            // Alpha
            _alphaLabel = GetPart<Label>("PART_AlphaLabel");
            BindLabel(_alphaLabel, "A", Resource.Alpha);

            _alphaTextBox = GetPart<TextBox>("PART_AlphaTextBox");
            BindTextBox(_alphaTextBox, _viewModel.Alpha, nameof(AlphaModel.Alpha), Resource.Alpha, new PercentConverter(), "{0}%");

            _alphaSlider = GetPart<Slider>("PART_AlphaSlider");
            BindSlider(_alphaSlider, _viewModel.Alpha, nameof(AlphaModel.Alpha), 0, 100, new PercentConverter());

            #endregion

            #region HSV

            // Hue
            _hueLabel = GetPart<Label>("PART_HueLabel");
            BindLabel(_hueLabel, "H", Resource.Hue);

            _hueTextBox = GetPart<TextBox>("PART_HueTextBox");
            BindTextBox(_hueTextBox, _viewModel.Hsv, nameof(ColorPickerViewModel.Hsv.Hue), Resource.Hue, new DegreesConverter(), "{0}°");

            _hueSlider = GetPart<Slider>("PART_HueSlider");
            BindSlider(_hueSlider, _viewModel.Hsv, nameof(ColorPickerViewModel.Hsv.Hue), 0, 360, new DegreesConverter());

            // Saturation
            _saturationLabel = GetPart<Label>("PART_SaturationLabel");
            BindLabel(_saturationLabel, "S", Resource.Saturation);

            _saturationTextBox = GetPart<TextBox>("PART_SaturationTextBox");
            BindTextBox(_saturationTextBox, _viewModel.Hsv, nameof(ColorPickerViewModel.Hsv.Saturation), Resource.Saturation, new PercentConverter(), "{0}%");

            _saturationSlider = GetPart<Slider>("PART_SaturationSlider");
            BindSlider(_saturationSlider, _viewModel.Hsv, nameof(ColorPickerViewModel.Hsv.Saturation), 0, 100, new PercentConverter());

            // Value
            _valueLabel = GetPart<Label>("PART_ValueLabel");
            BindLabel(_valueLabel, "V", Resource.Blue);

            _valueTextBox = GetPart<TextBox>("PART_ValueTextBox");
            BindTextBox(_valueTextBox, _viewModel.Hsv, nameof(ColorPickerViewModel.Hsv.Value), Resource.Value, new PercentConverter(), "{0}%");

            _valueSlider = GetPart<Slider>("PART_ValueSlider");
            BindSlider(_valueSlider, _viewModel.Hsv, nameof(ColorPickerViewModel.Hsv.Value), 0, 100, new PercentConverter());
                        
            #endregion

            #region HSL
            
            // Hue
            _hslHueLabel = GetPart<Label>("PART_HslHueLabel");
            BindLabel(_hslHueLabel, "H", Resource.HslHue);

            _hslHueTextBox = GetPart<TextBox>("PART_HslHueTextBox");
            BindTextBox(_hslHueTextBox, _viewModel.Hsl, nameof(ColorPickerViewModel.Hsl.Hue), Resource.HslHue, new DegreesConverter(), "{0}°");

            _hslHueSlider = GetPart<Slider>("PART_HslHueSlider");
            BindSlider(_hslHueSlider, _viewModel.Hsl, nameof(ColorPickerViewModel.Hsl.Hue), 0, 360, new DegreesConverter());

            // Saturation
            _hslSaturationLabel = GetPart<Label>("PART_HslSaturationLabel");
            BindLabel(_hslSaturationLabel, "S", Resource.HslSaturation);

            _hslSaturationTextBox = GetPart<TextBox>("PART_HslSaturationTextBox");
            BindTextBox(_hslSaturationTextBox, _viewModel.Hsl, nameof(ColorPickerViewModel.Hsl.Saturation), Resource.HslSaturation, new PercentConverter(), "{0}%");

            _hslSaturationSlider = GetPart<Slider>("PART_HslSaturationSlider");
            BindSlider(_hslSaturationSlider, _viewModel.Hsl, nameof(ColorPickerViewModel.Hsl.Saturation), 0, 100, new PercentConverter());

            // Lightness
            _lightnessLabel = GetPart<Label>("PART_LightnessLabel");
            BindLabel(_lightnessLabel, "L", Resource.Lightness);

            _lightnessTextBox = GetPart<TextBox>("PART_LightnessTextBox");
            BindTextBox(_lightnessTextBox, _viewModel.Hsl, nameof(ColorPickerViewModel.Hsl.Lightness), Resource.Lightness, new PercentConverter(), "{0}%");

            _lightnessSlider = GetPart<Slider>("PART_LightnessSlider");
            BindSlider(_lightnessSlider, _viewModel.Hsl, nameof(ColorPickerViewModel.Hsl.Lightness), 0, 100, new PercentConverter());

            #endregion

            #region CMYK

            // Cyan
            _cyanLabel = GetPart<Label>("PART_CyanLabel");
            BindLabel(_cyanLabel, "C", Resource.Cyan);

            _cyanTextBox = GetPart<TextBox>("PART_CyanTextBox");
            BindTextBox(_cyanTextBox, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Cyan), Resource.Cyan, new PercentConverter(), "{0}%");

            _cyanSlider = GetPart<Slider>("PART_CyanSlider");
            BindSlider(_cyanSlider, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Cyan), 0, 100, new PercentConverter());

            // Magenta
            _magentaLabel = GetPart<Label>("PART_MagentaLabel");
            BindLabel(_magentaLabel, "M", Resource.Magenta);

            _magentaTextBox = GetPart<TextBox>("PART_MagentaTextBox");
            BindTextBox(_magentaTextBox, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Magenta), Resource.Magenta, new PercentConverter(), "{0}%");

            _magentaSlider = GetPart<Slider>("PART_MagentaSlider");
            BindSlider(_magentaSlider, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Magenta), 0, 100, new PercentConverter());

            // Yellow
            _yellowLabel = GetPart<Label>("PART_YellowLabel");
            BindLabel(_yellowLabel, "Y", Resource.Yellow);

            _yellowTextBox = GetPart<TextBox>("PART_YellowTextBox");
            BindTextBox(_yellowTextBox, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Yellow), Resource.Yellow, new PercentConverter(), "{0}%");

            _yellowSlider = GetPart<Slider>("PART_YellowSlider");
            BindSlider(_yellowSlider, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Yellow), 0, 100, new PercentConverter());

            // Key
            _keyLabel = GetPart<Label>("PART_KeyLabel");
            BindLabel(_keyLabel, "K", Resource.Key);

            _keyTextBox = GetPart<TextBox>("PART_KeyTextBox");
            BindTextBox(_keyTextBox, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Key), Resource.Key, new PercentConverter(), "{0}%");

            _keySlider = GetPart<Slider>("PART_KeySlider");
            BindSlider(_keySlider, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Key), 0, 100, new PercentConverter());
                        
            #endregion

            #region Eyedropper
            
            if (_eyedropperButton != null)
                _eyedropperButton.Click -= EyedropperButton_Click;

            _eyedropperButton = GetPart<Button>("PART_EyedropperButton");
            if (_eyedropperButton != null)
            {
                var img = new Image()
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/ColorPicker.View.Wpf;component/Resources/eyedropper.png"))
                };
                _eyedropperButton.Content = img;
                _eyedropperButton.Click += EyedropperButton_Click;
            }

            #endregion

            #region Hue selection canvases

            // Horizontal
            if (_horizontalHueCanvas != null)
            {
                _horizontalHueCanvas.PreviewMouseLeftButtonDown -= HueCanvas_PreviewMouseLeftButtonDown;
                _horizontalHueCanvas.PreviewMouseLeftButtonUp -= HueCanvas_PreviewMouseLeftButtonUp;
                _horizontalHueCanvas.MouseMove -= HueCanvas_MouseMove;
            }

            _horizontalHueCanvas = GetPart<Canvas>("PART_HorizontalHueCanvas");

            if (_horizontalHueCanvas != null)            
            {
                var brush = _internalResources["HorizontalHueBrush"] as LinearGradientBrush;
                if (brush != null) _horizontalHueCanvas.Background = brush;
                _horizontalHueCanvas.Children.Clear();
                _horizontalHueCanvas.ClipToBounds = true;
                _horizontalHueCanvas.PreviewMouseLeftButtonDown += HueCanvas_PreviewMouseLeftButtonDown;
                _horizontalHueCanvas.PreviewMouseLeftButtonUp += HueCanvas_PreviewMouseLeftButtonUp;
                _horizontalHueCanvas.MouseMove += HueCanvas_MouseMove;

                _hueMarkerHorizontal = new HueMarker();

                _hueMarkerHorizontal.RenderTransformOrigin = new Point(0.5 * 0.65d, 0.5);

                var transformGroup = new TransformGroup();
                transformGroup.Children.Add(new RotateTransform { Angle = 90 });

                _hueMarkerHorizontal.RenderTransform = transformGroup;

                _horizontalHueCanvas.Children.Add(_hueMarkerHorizontal);

                _hueMarkerHorizontal.Loaded += (sender, e) =>
                {
                    _hueMarkerHorizontal.Width = _horizontalHueCanvas.ActualHeight;
                    _hueMarkerHorizontal.Height = _horizontalHueCanvas.ActualHeight * 0.65d;

                    MoveHueMarker(_viewModel.Hsv.Hue);
                };
            }

            // Vertical
            if (_verticalHueCanvas != null)
            {
                _verticalHueCanvas.PreviewMouseLeftButtonDown -= HueCanvas_PreviewMouseLeftButtonDown;
                _verticalHueCanvas.PreviewMouseLeftButtonUp -= HueCanvas_PreviewMouseLeftButtonUp;
                _verticalHueCanvas.MouseMove -= HueCanvas_MouseMove;
            }

            _verticalHueCanvas = GetPart<Canvas>("PART_VerticalHueCanvas");

            if (_verticalHueCanvas != null)
            {
                var brush = _internalResources["VerticalHueBrush"] as LinearGradientBrush;
                if (brush != null) _verticalHueCanvas.Background = brush;
                _verticalHueCanvas.Children.Clear();
                _verticalHueCanvas.ClipToBounds = true;
                _verticalHueCanvas.PreviewMouseLeftButtonDown += HueCanvas_PreviewMouseLeftButtonDown;
                _verticalHueCanvas.PreviewMouseLeftButtonUp += HueCanvas_PreviewMouseLeftButtonUp;
                _verticalHueCanvas.MouseMove += HueCanvas_MouseMove;

                _hueMarkerVertical = new HueMarker();

                _verticalHueCanvas.Children.Add(_hueMarkerVertical);

                _hueMarkerVertical.Loaded += (sender, e) =>
                {
                    _hueMarkerVertical.Width = _verticalHueCanvas.ActualWidth;
                    _hueMarkerVertical.Height = _verticalHueCanvas.ActualWidth * 0.65d;

                    MoveHueMarker(_viewModel.Hsv.Hue);
                };
            }

            #endregion

            #region Color selection canvas

            if (_colorSelectionCanvas != null)
            {
                _colorSelectionCanvas.SizeChanged -= ColorSelectionCanvas_SizeChanged;
                _colorSelectionCanvas.PreviewMouseMove -= ColorSelectionCanvas_PreviewMouseMove;
                _colorSelectionCanvas.PreviewMouseLeftButtonDown -= ColorSelectionCanvas_PreviewMouseLeftButtonDown;
                _colorSelectionCanvas.PreviewMouseLeftButtonUp -= ColorSelectionCanvas_PreviewMouseLeftButtonUp;
            }

            _colorSelectionCanvas = GetPart<Canvas>("PART_ColorSelectionCanvas");
            if (_colorSelectionCanvas != null)
            {
                _colorSelectionCanvas.Children.Clear();
                _colorSelectionCanvas.ClipToBounds = true;

                _colorSelectionImage = new ImageBrush();
                _colorSelectionCanvas.Background = _colorSelectionImage;

                _selectionColorMarker = new SelectionMark();

                _selectionColorMarker.Loaded += (sender, e) =>
                {
                    double x = _viewModel.Hsv.Saturation * (_colorSelectionCanvas.ActualWidth - 1);
                    double y = (1 - _viewModel.Hsv.Value) * (_colorSelectionCanvas.ActualHeight - 1);

                    MoveColorSelectionMark(x, y);
                };

                _colorSelectionCanvas.Children.Add(_selectionColorMarker);

                if (_colorSelectionCanvas.ActualWidth > 0 && _colorSelectionCanvas.ActualHeight > 0)
                {
                    RenderColorSelectionCanvas();
                }

                _colorSelectionCanvas.SizeChanged += ColorSelectionCanvas_SizeChanged;
                _colorSelectionCanvas.PreviewMouseMove += ColorSelectionCanvas_PreviewMouseMove;
                _colorSelectionCanvas.PreviewMouseLeftButtonDown += ColorSelectionCanvas_PreviewMouseLeftButtonDown;
                _colorSelectionCanvas.PreviewMouseLeftButtonUp += ColorSelectionCanvas_PreviewMouseLeftButtonUp;
            }

            _selectedColorBackground = GetPart<Rectangle>("PART_SelectedColorBackground");
            if (_selectedColorBackground != null)
            {
                _selectedColorBackground.Fill = (DrawingBrush)_internalResources["SquareBrush"];
            }

            _selectedColor = GetPart<Rectangle>("PART_SelectedColor");
            if (_selectedColor != null)
            {
                _selectedColor.SetBinding(Rectangle.FillProperty,
                    new Binding(nameof(ColorPickerViewModel.Hex.Hex)) { Source = _viewModel.Hex });
            }
            
            #endregion
        }

        /// <summary>
        /// Retrieves a template child by name and casts it to the specified type.
        /// </summary>
        private T GetPart<T>(string name) where T : class
        {
            return GetTemplateChild(name) as T;
        }

        /// <summary>
        /// Sets the label’s content and tooltip.
        /// </summary>
        private void BindLabel(Label label, string content, string tooltipResource)
        {
            if (label == null) return;
            label.Content = content;
            label.ToolTip = tooltipResource;
        }

        /// <summary>
        /// Binds a TextBox to a property.
        /// </summary>
        private void BindTextBox(TextBox textBox, object dataContext, string path, string tooltipResource, IValueConverter converter = null, string stringFormat = null)
        {
            if (textBox == null) return;
            textBox.ToolTip = tooltipResource;
            textBox.SetBinding(TextBox.TextProperty,
                new Binding(path) { Source = dataContext, Mode = BindingMode.TwoWay, Converter = converter, StringFormat = stringFormat });
        }

        /// <summary>
        /// Binds a Slider to a property.
        /// </summary>
        private void BindSlider(Slider slider, object dataContext, string path, double min, double max, IValueConverter converter = null)
        {
            if (slider == null) return;
            slider.Minimum = min;
            slider.Maximum = max;
            slider.SmallChange = 1;
            slider.LargeChange = 5;
            slider.SetBinding(Slider.ValueProperty,
                new Binding(path) { Source = dataContext, Mode = BindingMode.TwoWay, Converter = converter });
        }

        #region Color selection

        /// <summary>
        /// Color selection mouse left button down event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorSelectionCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeColorPreview(sender as Canvas, e.GetPosition(sender as Canvas));
            Mouse.Capture(sender as Canvas);
        }

        /// <summary>
        /// Color selection mouse left button up event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorSelectionCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangeColorPreview(sender as Canvas, e.GetPosition(sender as Canvas));
            Mouse.Capture(null);
        }

        /// <summary>
        /// Color selection mouse move event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorSelectionCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                ChangeColorPreview(sender as Canvas, e.GetPosition(sender as Canvas));
            }
        }

        /// <summary>
        /// Color selection size change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorSelectionCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _colorSelectionWidth = 0;
            _colorSelectionHeight = 0;

            RenderColorSelectionCanvas();
        }

        /// <summary>
        /// Called when user clicks/moves on the SV (saturation/value) canvas.
        /// Uses cached pixel buffer for fast sampling.
        /// </summary>
        private void ChangeColorPreview(Canvas canvas, Point point)
        {
            if (canvas == null) return;

            Point pos = point;

            double x = Math.Min(Math.Max(0, point.X), _colorSelectionCanvas.ActualWidth);
            double y = Math.Min(Math.Max(0, point.Y), _colorSelectionCanvas.ActualHeight);

            MoveColorSelectionMark(x, y);

            // Bytes: B,G,R,A
            byte[] bytes = GetRGBFromBitmap(pos);
            if (bytes == null || bytes.Length < 3) return;

            // Convert RGB to HSV
            ColorConversions.RgbToHsv(bytes[2], bytes[1], bytes[0], out double h, out double s, out double v);

            // Only for prevention against rending due to changes in view model.
            _lastHue = h;
            _lastSaturation = s;
            _lastValue = v;

            _viewModel.SelectColor(bytes[2], bytes[1], bytes[0], _viewModel.Alpha.Alpha);
        }

        /// <summary>
        /// Returns the stored color from position in array.
        /// </summary>
        /// <param name="point">Position in array.</param>
        /// <returns>The stored color from position in array.</returns>
        private byte[] GetRGBFromBitmap(Point point)
        {
            if (_renderedPixels == null) return new byte[4];

            int x = (int)Math.Round(point.X);
            int y = (int)Math.Round(point.Y);

            if (x < 0 || y < 0 || x >= _colorSelectionWidth || y >= _colorSelectionHeight)
                return null;

            int index = (y * _colorSelectionWidth + x) * 4;

            return new[]
            {
                _renderedPixels[index + 0], // B
                _renderedPixels[index + 1], // G
                _renderedPixels[index + 2], // R
                _renderedPixels[index + 3], // A
            };
        }

        /// <summary>
        /// Moves the color selection marker.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void MoveColorSelectionMark(double x, double y)
        {
            if (_selectionColorMarker.IsLoaded)
            {
                Canvas.SetLeft(_selectionColorMarker, x - _selectionColorMarker.ActualWidth / 2);
                Canvas.SetTop(_selectionColorMarker, y - _selectionColorMarker.ActualHeight / 2);
            }
        }

        #endregion

        #region Hue selection

        /// <summary>
        /// Hue selection mouse left button down event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HueCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeHueComponent(sender as Canvas, e.GetPosition(sender as Canvas));
            Mouse.Capture(sender as Canvas);
        }

        /// <summary>
        /// Hue selection mouse left button up event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HueCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        /// <summary>
        /// Hue selection mouse move event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HueCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                ChangeHueComponent(sender as Canvas, e.GetPosition(sender as Canvas));
        }

        /// <summary>
        /// Moves the hue marker.
        /// </summary>
        /// <param name="hue">Hue</param>
        private void MoveHueMarker(double hue)
        {
            if (_hueMarkerVertical != null && _hueMarkerVertical.IsLoaded)
            {
                double y = _verticalHueCanvas.ActualHeight * hue / 360d;

                Canvas.SetTop(_hueMarkerVertical, y - _hueMarkerVertical.Height / 2 + 1);
            }
            if (_hueMarkerHorizontal != null && _hueMarkerHorizontal.IsLoaded)
            {
                double x = _horizontalHueCanvas.ActualWidth * hue / 360d;

                Canvas.SetLeft(_hueMarkerHorizontal, x - _hueMarkerHorizontal.Width * 0.65d / 2);
            }
        }

        /// <summary>
        /// Called when user interacts with hue canvases.
        /// Recomputes hue from sampled pixel and updates ViewModel.
        /// </summary>
        private void ChangeHueComponent(Canvas canvas, Point point)
        {
            if (canvas == null) return;

            int hue;

            if (canvas == _verticalHueCanvas)
            {
                hue = (int)((point.Y / canvas.ActualHeight) * 360d);
            }
            else
            {
                hue = (int)((point.X / canvas.ActualWidth) * 360d);
            }

            hue = Math.Max(0, hue);
            hue = Math.Min(360, hue);

            if (hue != (int)_viewModel.Hsv.Hue)
            {
                _viewModel.Hsv.Hue = _lastHue = hue;

                RenderColorSelectionCanvas();

                MoveHueMarker(_viewModel.Hsv.Hue);
            }
        }

        /// <summary>
        /// Render saturation/value surface for current hue into a pixel buffer and that write to image.
        /// </summary>
        private void RenderColorSelectionCanvas()
        {
            if (_colorSelectionCanvas == null || _colorSelectionImage == null) return;

            int width = (int)_colorSelectionCanvas.ActualWidth;
            int height = (int)_colorSelectionCanvas.ActualHeight;

            if (width <= 0 || height <= 0) return;

            if (_colorSelectionWidth == width && _colorSelectionHeight == height && _lastRenderedSelectedHueValue == _viewModel.Hsv.Hue && _renderedPixels != null && _lastRenderedSelectedHueBitmap != null)
            {
                return;
            }

            if (_colorSelectionWidth != width || _colorSelectionHeight != height || _renderedPixels == null)
            {
                _colorSelectionWidth = width;
                _colorSelectionHeight = height;
                _renderedPixels = new byte[width * height * 4];
                _lastRenderedSelectedHueBitmap = null;
            }

            int index = 0;
            double hue = _viewModel.Hsv.Hue;

            for (int y = 0; y < height; y++)
            {
                double v = 1.0 - (double)y / (height - 1);
                for (int x = 0; x < width; x++)
                {
                    double s = (double)x / (width - 1);

                    ColorConversions.HsvToRgb(hue, s, v, out byte r, out byte g, out byte b);

                    _renderedPixels[index++] = b;
                    _renderedPixels[index++] = g;
                    _renderedPixels[index++] = r;
                    _renderedPixels[index++] = 255; // alpha
                }
            }

            if (_lastRenderedSelectedHueBitmap == null || _lastRenderedSelectedHueBitmap.PixelWidth != width || _lastRenderedSelectedHueBitmap.PixelHeight != height)
            {
                _lastRenderedSelectedHueBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
                _colorSelectionImage.ImageSource = _lastRenderedSelectedHueBitmap;
            }

            _lastRenderedSelectedHueBitmap.WritePixels(new Int32Rect(0, 0, width, height), _renderedPixels, width * 4, 0);

            _lastRenderedSelectedHueValue = _viewModel.Hsv.Hue;
        }

        #endregion

        /// <summary>
        /// Request for color from eye dropper.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EyedropperButton_Click(object sender, RoutedEventArgs e)
        {
            _lastHexValue = _viewModel.Hex.Hex;
            Debug.WriteLine(_lastHexValue);

            var overlay = new EyedropperOverlay();
            overlay.ColorPicked += (s, color) =>
            {
                if (color.HasValue)
                {
                    var c = color.Value;
                    _viewModel.SelectColor(c.R, c.G, c.B);
                }
                // Sets the previous value
                else
                {
                    _lastHue = -1;
                    _lastSaturation = -1;
                    _lastValue = -1;

                    _viewModel.Hex.Hex = _lastHexValue;
                }
            };
            overlay.ColorChanged += (s, color) =>
            {
                _viewModel.SelectColor(color.R, color.G, color.B);
            };
            overlay.Show();
        }
    }
}