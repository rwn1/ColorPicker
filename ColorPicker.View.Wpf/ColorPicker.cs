using ColorPicker.Core.Models;
using ColorPicker.Core.Properties;
using ColorPicker.View.Wpf.Elements;
using ColorPicker.View.Wpf.Models;
using ColorPicker.View.Wpf.Shared;
using ColorPicker.View.Wpf.Utilities;
using ColorPicker.View.Wpf.Utilities.Converters;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColorPicker.View.Wpf
{
    public class ColorPicker : Control, IDisposable
    {
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(Brush),
                typeof(ColorPicker),
                new PropertyMetadata(
                    Brushes.Transparent,
                    OnSelectedColorChanged
                )
            );

        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        public Brush SelectedColor
        {
            get => (Brush)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ColorPicker)d;
            var newBrush = (Brush)e.NewValue;

            control.OnSelectedColorChanged(newBrush);
        }

        protected virtual void OnSelectedColorChanged(Brush newBrush)
        {
            if (newBrush is SolidColorBrush solidBrush)
            {
                Color color = solidBrush.Color;
                _viewModel.Hex.Hex = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            }
        }

        /// <summary>
        /// Initializes a new instance of the ColorPicker class.
        /// </summary>
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
        /// Shared object for data exchange.
        /// </summary>
        private readonly ShareDataModel _shareDataModel;

        /// <summary>
        /// Eye dropper button.
        /// </summary>
        private Button _eyedropperButton;

        /// <summary>
        /// Canvas for hue selection.
        /// </summary>
        private Canvas _hueSelectionView;
        private HueSelection _hueSelection;

        /// <summary>
        /// Canvas for alpha selection.
        /// </summary>
        private Canvas _alphaSelectionView;
        private AlphaSelection _alphaSelection;

        /// <summary>
        /// Canvas for color selection.
        /// </summary>
        private Canvas _colorSelectionView;
        internal ColorSelection _colorSelection;

        /// <summary>
        /// Selected color.
        /// </summary>
        private Border _selectedColorView;

        /// <summary>
        /// Initializes a new instance of the ColorPicker class.
        /// </summary>
        public ColorPicker()
        {
            _shareDataModel = new ShareDataModel();

            _internalResources = new ResourceDictionary
            {
                Source = new Uri("/ColorPicker.View.Wpf;component/Themes/Styles.xaml", UriKind.Relative)
            };

            _viewModel = new ColorPickerViewModel();

            _viewModel.Hsv.PropertyChanged += Hsv_PropertyChanged;
            _viewModel.Hex.PropertyChanged += Hex_PropertyChanged;
            _viewModel.Alpha.PropertyChanged += Alpha_PropertyChanged;
        }

        private void Hsv_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HsvModel.Hue))
            {
                if (_colorSelectionView != null)
                {
                    if (_shareDataModel.LastHue != _viewModel.Hsv.Hue)
                    {
                        _colorSelection.RenderColorSelection();

                        _hueSelection.MoveHueMarker(_viewModel.Hsv.Hue);
                    }
                }
            }
            else if (e.PropertyName == nameof(HsvModel.Saturation) || e.PropertyName == nameof(HsvModel.Value))
            {
                if (_shareDataModel.LastSaturation != _viewModel.Hsv.Saturation ||
                    _shareDataModel.LastValue != _viewModel.Hsv.Value)
                {
                    _shareDataModel.LastSaturation = _viewModel.Hsv.Saturation;
                    _shareDataModel.LastValue = _viewModel.Hsv.Value;

                    double x = _viewModel.Hsv.Saturation * (_colorSelectionView.ActualWidth - 1);
                    double y = (1 - _viewModel.Hsv.Value) * (_colorSelectionView.ActualHeight - 1);

                    _colorSelection.MoveColorSelectionMark(x, y);
                }
            }
        }

        private void Hex_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Brush selectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_viewModel.Hex.Hex));
            selectedBrush.Freeze();

            _alphaSelection?.SetSelectedColor(selectedBrush);

            SelectedColor = selectedBrush;
        }

        private void Alpha_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_alphaSelectionView != null)
            {
                if (_shareDataModel.LastAlpha != _viewModel.Alpha.Alpha)
                {
                    _alphaSelection.MoveAlphaMarker(_viewModel.Alpha.Alpha);
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InitHexSection();
            InitAlphaSection();
            InitRgbSection();
            InitHsvSection();
            InitHslSection();
            InitCmykSection();
            InitEyedropper();
            InitColorSelection();
            InitSelectedColor();
            InitHueSelection();
        }

        /// <summary>
        /// Initializes Hex selections.
        /// </summary>
        private void InitHexSection()
        {
            TextBox hexTextBox = GetPart<TextBox>("PART_HexTextBox");
            BindTextBox(hexTextBox, _viewModel.Hex, nameof(ColorPickerViewModel.Hex.Hex), Resource.Hex);
        }

        /// <summary>
        /// Initializes Alpha selections.
        /// </summary>
        private void InitAlphaSection()
        {
            TextBox alphaTextBox = GetPart<TextBox>("PART_AlphaTextBox");
            BindTextBox(alphaTextBox, _viewModel.Alpha, nameof(AlphaModel.Alpha), Resource.Alpha, new PercentConverter(), "%");

            Slider alphaSlider = GetPart<Slider>("PART_AlphaSlider");
            BindSlider(alphaSlider, _viewModel.Alpha, nameof(AlphaModel.Alpha), 0, 100, new PercentConverter());

            _alphaSelectionView = GetPart<Canvas>("PART_AlphaSelectionView");

            if (_alphaSelectionView != null)
            {
                _alphaSelection = new AlphaSelection(_alphaSelectionView, _shareDataModel, _viewModel, _internalResources);
            }
        }

        /// <summary>
        /// Initializes RGB selections.
        /// </summary>
        private void InitRgbSection()
        {
            // Red
            TextBox redTextBox = GetPart<TextBox>("PART_RedTextBox");
            BindTextBox(redTextBox, _viewModel.Rgb, nameof(ColorPickerViewModel.Rgb.Red), Resource.Red);

            Slider redSlider = GetPart<Slider>("PART_RedSlider");
            BindSlider(redSlider, _viewModel.Rgb, nameof(ColorPickerViewModel.Rgb.Red), 0, 255);

            // Green
            TextBox greenTextBox = GetPart<TextBox>("PART_GreenTextBox");
            BindTextBox(greenTextBox, _viewModel.Rgb, nameof(ColorPickerViewModel.Rgb.Green), Resource.Green);

            Slider greenSlider = GetPart<Slider>("PART_GreenSlider");
            BindSlider(greenSlider, _viewModel.Rgb, nameof(ColorPickerViewModel.Rgb.Green), 0, 255);

            // Blue
            TextBox blueTextBox = GetPart<TextBox>("PART_BlueTextBox");
            BindTextBox(blueTextBox, _viewModel.Rgb, nameof(ColorPickerViewModel.Rgb.Blue), Resource.Blue);

            Slider blueSlider = GetPart<Slider>("PART_BlueSlider");
            BindSlider(blueSlider, _viewModel.Rgb, nameof(ColorPickerViewModel.Rgb.Blue), 0, 255);
        }

        /// <summary>
        /// Initializes HSV selections.
        /// </summary>
        private void InitHsvSection()
        {
            // Hue
            TextBox hueTextBox = GetPart<TextBox>("PART_HueTextBox");
            BindTextBox(hueTextBox, _viewModel.Hsv, nameof(ColorPickerViewModel.Hsv.Hue), Resource.Hue, new DegreesConverter(), "°");

            Slider hueSlider = GetPart<Slider>("PART_HueSlider");
            BindSlider(hueSlider, _viewModel.Hsv, nameof(ColorPickerViewModel.Hsv.Hue), 0, 360, new DegreesConverter());

            // Saturation
            TextBox saturationTextBox = GetPart<TextBox>("PART_SaturationTextBox");
            BindTextBox(saturationTextBox, _viewModel.Hsv, nameof(ColorPickerViewModel.Hsv.Saturation), Resource.Saturation, new PercentConverter(), "%");

            Slider saturationSlider = GetPart<Slider>("PART_SaturationSlider");
            BindSlider(saturationSlider, _viewModel.Hsv, nameof(ColorPickerViewModel.Hsv.Saturation), 0, 100, new PercentConverter());

            // Value
            TextBox valueTextBox = GetPart<TextBox>("PART_ValueTextBox");
            BindTextBox(valueTextBox, _viewModel.Hsv, nameof(ColorPickerViewModel.Hsv.Value), Resource.Value, new PercentConverter(), "%");

            Slider valueSlider = GetPart<Slider>("PART_ValueSlider");
            BindSlider(valueSlider, _viewModel.Hsv, nameof(ColorPickerViewModel.Hsv.Value), 0, 100, new PercentConverter());
        }

        /// <summary>
        /// Initializes HSL selections.
        /// </summary>
        private void InitHslSection()
        {
            // Hue
            TextBox hslHueTextBox = GetPart<TextBox>("PART_HslHueTextBox");
            BindTextBox(hslHueTextBox, _viewModel.Hsl, nameof(ColorPickerViewModel.Hsl.Hue), Resource.HslHue, new DegreesConverter(), "°");

            Slider hslHueSlider = GetPart<Slider>("PART_HslHueSlider");
            BindSlider(hslHueSlider, _viewModel.Hsl, nameof(ColorPickerViewModel.Hsl.Hue), 0, 360, new DegreesConverter());

            // Saturation
            TextBox hslSaturationTextBox = GetPart<TextBox>("PART_HslSaturationTextBox");
            BindTextBox(hslSaturationTextBox, _viewModel.Hsl, nameof(ColorPickerViewModel.Hsl.Saturation), Resource.HslSaturation, new PercentConverter(), "%");

            Slider hslSaturationSlider = GetPart<Slider>("PART_HslSaturationSlider");
            BindSlider(hslSaturationSlider, _viewModel.Hsl, nameof(ColorPickerViewModel.Hsl.Saturation), 0, 100, new PercentConverter());

            // Lightness
            TextBox lightnessTextBox = GetPart<TextBox>("PART_LightnessTextBox");
            BindTextBox(lightnessTextBox, _viewModel.Hsl, nameof(ColorPickerViewModel.Hsl.Lightness), Resource.Lightness, new PercentConverter(), "%");

            Slider lightnessSlider = GetPart<Slider>("PART_LightnessSlider");
            BindSlider(lightnessSlider, _viewModel.Hsl, nameof(ColorPickerViewModel.Hsl.Lightness), 0, 100, new PercentConverter());
        }

        /// <summary>
        /// Initializes CMYK selections.
        /// </summary>
        private void InitCmykSection()
        {
            // Cyan
            TextBox cyanTextBox = GetPart<TextBox>("PART_CyanTextBox");
            BindTextBox(cyanTextBox, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Cyan), Resource.Cyan, new PercentConverter(), "%");

            Slider cyanSlider = GetPart<Slider>("PART_CyanSlider");
            BindSlider(cyanSlider, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Cyan), 0, 100, new PercentConverter());

            // Magenta
            TextBox magentaTextBox = GetPart<TextBox>("PART_MagentaTextBox");
            BindTextBox(magentaTextBox, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Magenta), Resource.Magenta, new PercentConverter(), "%");

            Slider magentaSlider = GetPart<Slider>("PART_MagentaSlider");
            BindSlider(magentaSlider, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Magenta), 0, 100, new PercentConverter());

            // Yellow
            TextBox yellowTextBox = GetPart<TextBox>("PART_YellowTextBox");
            BindTextBox(yellowTextBox, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Yellow), Resource.Yellow, new PercentConverter(), "%");

            Slider yellowSlider = GetPart<Slider>("PART_YellowSlider");
            BindSlider(yellowSlider, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Yellow), 0, 100, new PercentConverter());

            // Key
            TextBox keyTextBox = GetPart<TextBox>("PART_KeyTextBox");
            BindTextBox(keyTextBox, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Key), Resource.Key, new PercentConverter(), "%");

            Slider keySlider = GetPart<Slider>("PART_KeySlider");
            BindSlider(keySlider, _viewModel.Cmyk, nameof(ColorPickerViewModel.Cmyk.Key), 0, 100, new PercentConverter());
        }

        /// <summary>
        /// Initializes eye dropper.
        /// </summary>
        private void InitEyedropper()
        {
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
        }

        /// <summary>
        /// Initializes color selection.
        /// </summary>
        private void InitColorSelection()
        {
            _colorSelectionView = GetPart<Canvas>("PART_ColorSelectionView");
            if (_colorSelectionView != null)
            {
                _colorSelection = new ColorSelection(_colorSelectionView, _shareDataModel, _viewModel);
            }
        }

        /// <summary>
        /// Initializes selected color.
        /// </summary>
        private void InitSelectedColor()
        {
            _selectedColorView = GetPart<Border>("PART_SelectedColorView");
            if (_selectedColorView != null)
            {
                _selectedColorView.Background = (DrawingBrush)_internalResources["SquareBrush"];

                var foreground = new Border();
                foreground.SetBinding(Border.BackgroundProperty,
                    new Binding(nameof(ColorPickerViewModel.Hex.Hex)) { Source = _viewModel.Hex });

                _selectedColorView.Child = foreground;
            }
        }

        /// <summary>
        /// Initializes hue selection.
        /// </summary>
        private void InitHueSelection()
        {
            _hueSelectionView = GetPart<Canvas>("PART_HueSelectionView");

            if (_hueSelectionView != null)
            {
                _hueSelection = new HueSelection(_hueSelectionView, _colorSelection, _shareDataModel, _viewModel, _internalResources);
            }
        }

        /// <summary>
        /// Retrieves a template child by name and casts it to the specified type.
        /// </summary>
        private T GetPart<T>(string name) where T : class
        {
            return GetTemplateChild(name) as T;
        }

        /// <summary>
        /// Binds a TextBox to a property.
        /// </summary>
        private void BindTextBox(TextBox textBox, object dataContext, string path, string tooltipResource, IValueConverter converter = null, string valueUnit = null)
        {
            if (textBox == null) return;
            textBox.ToolTip = tooltipResource;
            textBox.SetBinding(TextBox.TextProperty,
                new Binding(path) { Source = dataContext, Mode = BindingMode.TwoWay, Converter = converter, ConverterParameter = valueUnit });

            if (valueUnit != null)
                FocusFormatBehavior.SetUnit(textBox, valueUnit);
        }

        /// <summary>
        /// Binds a Slider to a property.
        /// </summary>
        private void BindSlider(Slider slider, object dataContext, string path, float min, float max, IValueConverter converter = null)
        {
            if (slider == null) return;
            slider.Minimum = min;
            slider.Maximum = max;
            slider.SmallChange = 1;
            slider.LargeChange = 5;
            slider.SetBinding(Slider.ValueProperty,
                new Binding(path) { Source = dataContext, Mode = BindingMode.TwoWay, Converter = converter });
        }

        /// <summary>
        /// Request for color from eye dropper.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EyedropperButton_Click(object sender, RoutedEventArgs e)
        {
            var lastHexValue = _viewModel.Hex.Hex;

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
                    _shareDataModel.LastHue = -1;
                    _shareDataModel.LastSaturation = -1;
                    _shareDataModel.LastValue = -1;

                    _viewModel.Hex.Hex = lastHexValue;
                }
            };
            overlay.ColorChanged += (s, color) =>
            {
                _viewModel.SelectColor(color.R, color.G, color.B);
            };
            overlay.Show();
        }

        /// <summary>
        /// Releases all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            if (_viewModel != null)
            {
                _viewModel.Hsv.PropertyChanged -= Hsv_PropertyChanged;
                _viewModel.Hex.PropertyChanged -= Hex_PropertyChanged;
                _viewModel.Alpha.PropertyChanged -= Alpha_PropertyChanged;
            }

            if (_eyedropperButton != null)
                _eyedropperButton.Click -= EyedropperButton_Click;

            _hueSelection?.Dispose();
            _alphaSelection?.Dispose();
            _colorSelection?.Dispose();
        }
    }
}