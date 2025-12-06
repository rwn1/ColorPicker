using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using System.Globalization;

namespace ColorPicker.View.Wpf.Tests
{
    [TestClass]
    public sealed class ColorPickerTests
    {
        private static Application _application;
        private static UIA3Automation _automation;
        private static Window _window;

        /// <summary>
        /// Values for testing the percentages TextBox controls.
        /// </summary>
        private static IEnumerable<object[]> percentagesTextBoxValues =>
            new List<object[]>
            {
                new object[] { "-100", "0", true },
                new object[] { "-50", "0", true },
                new object[] { "50", "50", true },
                new object[] { "99.9999", "100", true },
                new object[] { "100", "100", true },
                new object[] { "150", "100", true },
                new object[] { "1000", "100", true },
                new object[] { "82.4", "82", true },
                new object[] { "ABC", null, false }
            };

        /// <summary>
        /// Values for testing the percentages Slider controls.
        /// </summary>
        private static IEnumerable<object[]> PercentagesSliderValues =>
            new List<object[]>
            {
                new object[] { 0, "0%" },
                new object[] { 50, "50%" },
                new object[] { 99, "99%" },
                new object[] { 100, "100%" },
                new object[] { 82, "82%" }
            };

        /// <summary>
        /// Values for testing the degrees TextBox controls.
        /// </summary>
        private static IEnumerable<object[]> DegreesTextBoxValues =>
            new List<object[]>
            {
                new object[] { "-100", "0", true },
                new object[] { "-0.5", "0", true },
                new object[] { "0.5", "0", true },
                new object[] { "0.999999", "1", true },
                new object[] { "300", "300", true },
                new object[] { "400", "360", true },
                new object[] { "5000", "360", true },
                new object[] { "ABC", null, false }
            };

        /// <summary>
        /// Values for testing the degrees Slider controls.
        /// </summary>
        private static IEnumerable<object[]> DegreesSliderValues =>
            new List<object[]>
            {
                new object[] { 0, "0°" },
                new object[] { 10, "10°" },
                new object[] { 200, "200°" },
                new object[] { 360, "360°" }
            };

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _application = Application.Launch(@"C:\Users\marti\Desktop\C#\CP13\CP12\CP11\CP10\CP9\CP7\CP6\CP\ColorPicker.App\bin\Debug\net8.0-windows\ColorPicker.App.exe");
            if (_application != null)
            {
                _automation = new UIA3Automation();
                _window = _application!.GetMainWindow(_automation);
            }
        }

        [TestInitialize]
        public void Init()
        {
            ResetColorInputs();
        }

        /// <summary>
        /// Resets color input values.
        /// </summary>
        private void ResetColorInputs()
        {
            ResetTextBox("PART_HexTextBox", "#FF000000");

            ResetTextBox("PART_RedTextBox", "0");
            ResetTextBox("PART_GreenTextBox", "0");
            ResetTextBox("PART_BlueTextBox", "0");

            ResetTextBox("PART_AlphaTextBox", "100%");

            ResetTextBox("PART_HueTextBox", "0°");
            ResetTextBox("PART_SaturationTextBox", "0%");
            ResetTextBox("PART_ValueTextBox", "0%");

            ResetTextBox("PART_HslHueTextBox", "0°");
            ResetTextBox("PART_HslSaturationTextBox", "0%");
            ResetTextBox("PART_LightnessTextBox", "0%");

            ResetTextBox("PART_CyanTextBox", "0%");
            ResetTextBox("PART_MagentaTextBox", "0%");
            ResetTextBox("PART_YellowTextBox", "0%");
            ResetTextBox("PART_KeyTextBox", "100%");
        }

        /// <summary>
        /// Resets the TextBox values.
        /// </summary>
        /// <param name="automationId">Identification of the TextBox.</param>
        /// <param name="value">Value to set.</param>
        private void ResetTextBox(string automationId, string value)
        {
            var textBox = _window.FindFirstDescendant(cf => cf.ByAutomationId(automationId))?.AsTextBox();
            if (textBox == null) return;

            if (textBox.Text != value)
            {
                textBox.Text = value;
                Keyboard.Press(VirtualKeyShort.TAB);
            }
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _automation.Dispose();
            _application.Close();
        }

        #region Hex

        [DataTestMethod]
        [DataRow("AAAAA", false)]
        [DataRow("123456", false)]
        [DataRow("XYZ", false)]
        [DataRow("FFFFF", false)]
        [DataRow("#000000", true)]
        [DataRow("#FFFFFFFF", true)]
        [DataRow("#80ABCDEF", true)]
        [DataRow("FFFFFF", false)]
        [DataRow("#FFF", false)]
        [DataRow("#FFFFF", false)]
        [DataRow("#GGGGGG", false)]
        [DataRow("#1234567", false)]
        [DataRow("#XYZXYZ", false)]
        [DataRow("#ABCDE", false)]
        public void HexTextBox_ValidateValue(string input, bool isValid)
        {
            if (_window != null)
            {
                string defaultValue = "#FF000000";

                var hexTextBox = _window.FindFirstDescendant(cf => cf.ByAutomationId("PART_HexTextBox"))?.AsTextBox();
                if (hexTextBox != null)
                {
                    hexTextBox.Text = defaultValue;
                    Keyboard.Press(VirtualKeyShort.TAB);
                    hexTextBox.Text = input;
                    Keyboard.Press(VirtualKeyShort.TAB);

                    if (isValid)
                    {
                        if (input != defaultValue)
                        {
                            Retry.WhileTrue(
                                () => hexTextBox.Text == defaultValue,
                                timeout: TimeSpan.FromMilliseconds(2000)
                            );
                        }

                        Assert.AreEqual(input, hexTextBox.Text);
                    }
                    else
                    {
                        Assert.AreEqual(defaultValue, hexTextBox.Text);
                        Assert.AreNotEqual(input, hexTextBox.Text);
                    }
                }
            }
        }

        [DataTestMethod]
        [DataRow("#FF00AA", "#FFFF00AA")]
        [DataRow("#FFFFFF", "#FFFFFFFF")]
        [DataRow("#FF00FF", "#FFFF00FF")]
        public void HexTextBox_ValidateValueUpdate(string input, string output)
        {
            if (_window != null)
            {
                var hexTextBox = _window.FindFirstDescendant(cf => cf.ByAutomationId("PART_HexTextBox"))?.AsTextBox();
                if (hexTextBox != null)
                {
                    hexTextBox.Text = input;
                    Keyboard.Press(VirtualKeyShort.TAB);

                    var retryResult = Retry.WhileFalse(
                            () => hexTextBox.Text == output,
                            timeout: TimeSpan.FromMilliseconds(5000)
                        );

                    Assert.IsTrue(retryResult.Success);
                }
            }
        }

        #endregion

        #region RGB

        /// <summary>
        /// Values for testing the RGB TextBox controls.
        /// </summary>
        private static IEnumerable<object[]> RgbTextBoxValues =>
            new List<object[]>
            {
                new object[] { "-100", false },
                new object[] { "-0.5", false },
                new object[] { "5", true },
                new object[] { "50", true },
                new object[] { "200", true },
                new object[] { "300", false },
                new object[] { "ABC", false }
            };

        /// <summary>
        /// Values for testing the RGB Slider controls.
        /// </summary>
        private static IEnumerable<object[]> RgbSliderValues =>
            new List<object[]>
            {
                new object[] { 50 },
                new object[] { 100 },
                new object[] { 255 }
            };

        [DataTestMethod]
        [DynamicData(nameof(RgbTextBoxValues), DynamicDataSourceType.Property)]
        public void RedTextBox_ValidateValue(string input, bool isValid)
        {
            RgbTextBox_ValidateValue(input, isValid, "PART_RedTextBox", "PART_RedSlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(RgbSliderValues), DynamicDataSourceType.Property)]
        public void RedSlider_ValidateValue(double input)
        {
            RgbSlider_ValidateValue(input, "PART_RedSlider", "PART_RedTextBox");
        }

        [DataTestMethod]
        [DynamicData(nameof(RgbTextBoxValues), DynamicDataSourceType.Property)]
        public void GreenTextBox_ValidateValue(string input, bool isValid)
        {
            RgbTextBox_ValidateValue(input, isValid, "PART_GreenTextBox", "PART_GreenSlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(RgbSliderValues), DynamicDataSourceType.Property)]
        public void GreenSlider_ValidateValue(double input)
        {
            RgbSlider_ValidateValue(input, "PART_GreenSlider", "PART_GreenTextBox");
        }

        [DataTestMethod]
        [DynamicData(nameof(RgbTextBoxValues), DynamicDataSourceType.Property)]
        public void BlueTextBox_ValidateValue(string input, bool isValid)
        {
            RgbTextBox_ValidateValue(input, isValid, "PART_BlueTextBox", "PART_BlueSlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(RgbSliderValues), DynamicDataSourceType.Property)]
        public void BlueSlider_ValidateValue(double input)
        {
            RgbSlider_ValidateValue(input, "PART_BlueSlider", "PART_BlueTextBox");
        }

        /// <summary>
        /// Validates the RGB TextBox input values.
        /// </summary>
        /// <param name="input">Input value.</param>
        /// <param name="isValid">Information about whether the input value is valid.</param>
        /// <param name="textBoxId">TextBox identifier.</param>
        /// <param name="sliderId">Slider identifier</param>
        public void RgbTextBox_ValidateValue(string input, bool isValid, string textBoxId, string sliderId)
        {
            if (_window != null)
            {
                var textBox = _window.FindFirstDescendant(cf => cf.ByAutomationId(textBoxId))?.AsTextBox();
                if (textBox != null)
                {
                    textBox.Text = input;
                    Keyboard.Press(VirtualKeyShort.TAB);

                    if (isValid)
                    {
                        var slider = _window.FindFirstDescendant(cf => cf.ByAutomationId(sliderId))?.AsSlider();
                        if (slider != null)
                        {
                            double value = double.Parse(input, NumberStyles.Number, NumberFormatInfo.InvariantInfo);
                            Assert.AreEqual(value, slider.Value);
                        }
                    }
                    else
                    {
                        var retryResult = Retry.WhileFalse(
                            () => textBox.Properties.HelpText.Value == "Error",
                            timeout: TimeSpan.FromMilliseconds(5000)
                        );

                        Assert.IsTrue(retryResult.Success);
                    }
                }
            }
        }

        /// <summary>
        /// Validates the RGB Slider input values.
        /// </summary>
        /// <param name="input">Input value.</param>
        /// <param name="sliderId">Slider identifier</param>
        /// <param name="textBoxId">TextBox identifier.</param>
        public void RgbSlider_ValidateValue(double input, string sliderId, string textBoxId)
        {
            if (_window != null)
            {
                var slider = _window.FindFirstDescendant(cf => cf.ByAutomationId(sliderId))?.AsSlider();
                if (slider != null)
                {
                    slider.Value = input;

                    var textBox = _window.FindFirstDescendant(cf => cf.ByAutomationId(textBoxId))?.AsTextBox();
                    if (textBox != null)
                    {
                        double value = double.Parse(textBox.Text, NumberStyles.Number, NumberFormatInfo.InvariantInfo);
                        Assert.AreEqual(input, value);
                    }
                }
            }
        }









        #endregion

        #region Alpha

        [DataTestMethod]
        [DynamicData(nameof(percentagesTextBoxValues), DynamicDataSourceType.Property)]
        public void AlphaTextBox_ValidateValue(string input, string output, bool isInputNumber)
        {
            TextBox_ValidateValue(input, output, isInputNumber, "PART_AlphaTextBox", "PART_AlphaSlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(PercentagesSliderValues), DynamicDataSourceType.Property)]
        public void AlphaSlider_ValidateValue(double input, string output)
        {
            Slider_ValidateValue(input,output, "PART_AlphaSlider", "PART_AlphaTextBox");
        }

        #endregion

        #region HSV

        [DataTestMethod]
        [DynamicData(nameof(DegreesTextBoxValues), DynamicDataSourceType.Property)]
        public void HueTextBox_ValidateValue(string input, string output, bool isInputNumber)
        {
            TextBox_ValidateValue(input, output, isInputNumber, "PART_HueTextBox", "PART_HueSlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(DegreesSliderValues), DynamicDataSourceType.Property)]
        public void HueSlider_ValidateValue(double input, string output)
        {
            Slider_ValidateValue(input, output, "PART_HueSlider", "PART_HueTextBox");
        }

        [DataTestMethod]
        [DynamicData(nameof(percentagesTextBoxValues), DynamicDataSourceType.Property)]
        public void SaturationTextBox_ValidateValue(string input, string output, bool isInputNumber)
        {
            TextBox_ValidateValue(input, output, isInputNumber, "PART_SaturationTextBox", "PART_SaturationSlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(PercentagesSliderValues), DynamicDataSourceType.Property)]
        public void SaturationSlider_ValidateValue(double input, string output)
        {
            Slider_ValidateValue(input, output, "PART_SaturationSlider", "PART_SaturationTextBox");
        }

        [DataTestMethod]
        [DynamicData(nameof(percentagesTextBoxValues), DynamicDataSourceType.Property)]
        public void ValueTextBox_ValidateValue(string input, string output, bool isInputNumber)
        {
            TextBox_ValidateValue(input, output, isInputNumber, "PART_ValueTextBox", "PART_ValueSlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(PercentagesSliderValues), DynamicDataSourceType.Property)]
        public void ValueSlider_ValidateValue(double input, string output)
        {
            Slider_ValidateValue(input, output, "PART_ValueSlider", "PART_ValueTextBox");
        }

        #endregion

        #region HSL

        [DataTestMethod]
        [DynamicData(nameof(DegreesTextBoxValues), DynamicDataSourceType.Property)]
        public void HslHueTextBox_ValidateValue(string input, string output, bool isInputNumber)
        {
            TextBox_ValidateValue(input, output, isInputNumber, "PART_HslHueTextBox", "PART_HslHueSlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(DegreesSliderValues), DynamicDataSourceType.Property)]
        public void HslHueSlider_ValidateValue(double input, string output)
        {
            Slider_ValidateValue(input, output, "PART_HslHueSlider", "PART_HslHueTextBox");
        }

        [DataTestMethod]
        [DynamicData(nameof(percentagesTextBoxValues), DynamicDataSourceType.Property)]
        public void HslSaturationTextBox_ValidateValue(string input, string output, bool isInputNumber)
        {
            TextBox_ValidateValue(input, output, isInputNumber, "PART_HslSaturationTextBox", "PART_HslSaturationSlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(PercentagesSliderValues), DynamicDataSourceType.Property)]
        public void HslSaturationSlider_ValidateValue(double input, string output)
        {
            Slider_ValidateValue(input, output, "PART_HslSaturationSlider", "PART_HslSaturationTextBox");
        }

        [DataTestMethod]
        [DynamicData(nameof(percentagesTextBoxValues), DynamicDataSourceType.Property)]
        public void LightnessTextBox_ValidateValue(string input, string output, bool isInputNumber)
        {
            TextBox_ValidateValue(input, output, isInputNumber, "PART_LightnessTextBox", "PART_LightnessSlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(PercentagesSliderValues), DynamicDataSourceType.Property)]
        public void LightnessSlider_ValidateValue(double input, string output)
        {
            Slider_ValidateValue(input, output, "PART_LightnessSlider", "PART_LightnessTextBox");
        }

        #endregion

        #region CMYK

        [DataTestMethod]
        [DynamicData(nameof(percentagesTextBoxValues), DynamicDataSourceType.Property)]
        public void CyanTextBox_ValidateValue(string input, string output, bool isInputNumber)
        {
            TextBox_ValidateValue(input, output, isInputNumber, "PART_CyanTextBox", "PART_CyanSlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(PercentagesSliderValues), DynamicDataSourceType.Property)]
        public void CyanSlider_ValidateValue(double input, string output)
        {
            Slider_ValidateValue(input, output, "PART_CyanSlider", "PART_CyanTextBox");
        }

        [DataTestMethod]
        [DynamicData(nameof(percentagesTextBoxValues), DynamicDataSourceType.Property)]
        public void MagentaTextBox_ValidateValue(string input, string output, bool isInputNumber)
        {
            TextBox_ValidateValue(input, output, isInputNumber, "PART_MagentaTextBox", "PART_MagentaSlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(PercentagesSliderValues), DynamicDataSourceType.Property)]
        public void MagentaSlider_ValidateValue(double input, string output)
        {
            Slider_ValidateValue(input, output, "PART_MagentaSlider", "PART_MagentaTextBox");
        }

        [DataTestMethod]
        [DynamicData(nameof(percentagesTextBoxValues), DynamicDataSourceType.Property)]
        public void YellowTextBox_ValidateValue(string input, string output, bool isInputNumber)
        {
            TextBox_ValidateValue(input, output, isInputNumber, "PART_YellowTextBox", "PART_YellowSlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(PercentagesSliderValues), DynamicDataSourceType.Property)]
        public void YellowSlider_ValidateValue(double input, string output)
        {
            Slider_ValidateValue(input, output, "PART_YellowSlider", "PART_YellowTextBox");
        }

        [DataTestMethod]
        [DynamicData(nameof(percentagesTextBoxValues), DynamicDataSourceType.Property)]
        public void KeyTextBox_ValidateValue(string input, string output, bool isInputNumber)
        {
            TextBox_ValidateValue(input, output, isInputNumber, "PART_KeyTextBox", "PART_KeySlider");
        }

        [DataTestMethod]
        [DynamicData(nameof(PercentagesSliderValues), DynamicDataSourceType.Property)]
        public void KeySlider_ValidateValue(double input, string output)
        {
            Slider_ValidateValue(input, output, "PART_KeySlider", "PART_KeyTextBox");
        }

        #endregion

        /// <summary>
        /// Validates the TextBox input values.
        /// </summary>
        /// <param name="input">Input value.</param>
        /// <param name="output">Output value.</param>
        /// <param name="isInputNumber">Information about whether the input value is a number.</param>
        /// <param name="textBoxId">TextBox identifier.</param>
        /// <param name="sliderId">Slider identifier</param>
        private void TextBox_ValidateValue(string input, string output, bool isInputNumber, string textBoxId, string sliderId)
        {
            if (_window != null)
            {
                var textBox = _window.FindFirstDescendant(cf => cf.ByAutomationId(textBoxId))?.AsTextBox();
                if (textBox != null)
                {
                    textBox.Text = input;
                        Keyboard.Press(VirtualKeyShort.TAB);

                    if (isInputNumber)
                    {
                        if (input != output)
                        {
                            Retry.WhileFalse(
                                () => textBox.Text.TrimEnd('°').TrimEnd('%') == output,
                                timeout: TimeSpan.FromMilliseconds(5000)
                            );
                        }

                        double value = double.Parse(textBox.Text.TrimEnd('°').TrimEnd('%'), NumberStyles.Number, NumberFormatInfo.InvariantInfo);
                        Assert.AreEqual(output, value.ToString(NumberFormatInfo.InvariantInfo));

                        var slider = _window.FindFirstDescendant(cf => cf.ByAutomationId(sliderId))?.AsSlider();
                        if (slider != null)
                        {
                            value = double.Parse(output, NumberStyles.Number, NumberFormatInfo.InvariantInfo);
                            Assert.AreEqual(value, slider.Value);
                        }
                    }
                    else
                    {
                        var retryResult = Retry.WhileFalse(
                                () => textBox.Properties.HelpText.Value == "Error",
                                timeout: TimeSpan.FromMilliseconds(5000)
                            );

                        Assert.IsTrue(retryResult.Success);
                    }
                }
            }
        }

        /// <summary>
        /// Validates the Slider input values.
        /// </summary>
        /// <param name="input">Input value.</param>
        /// <param name="output">Output value.</param>
        /// <param name="sliderId">Slider identifier</param>
        /// <param name="textBoxId">TextBox identifier.</param>
        private void Slider_ValidateValue(double input, string output, string sliderId, string textBoxId)
        {
            if (_window != null)
            {
                var slider = _window.FindFirstDescendant(cf => cf.ByAutomationId(sliderId))?.AsSlider();
                var textBox = _window.FindFirstDescendant(cf => cf.ByAutomationId(textBoxId))?.AsTextBox();

                if (slider != null && textBox != null)
                {
                    slider.Value = input;

                    var retryResult = Retry.WhileFalse(
                            () => textBox.Text == output,
                            timeout: TimeSpan.FromMilliseconds(5000)
                        );

                    Assert.IsTrue(retryResult.Success);
                }
            }
        }
    }
}