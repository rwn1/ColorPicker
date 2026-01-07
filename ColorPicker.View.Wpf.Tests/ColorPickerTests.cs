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
        /// <summary>
        /// Gets the wrapper for an application which should be automated.
        /// </summary>
        private static Application _application = default!;

        /// <summary>
        /// Gets the automation implementation for UIA3.
        /// </summary>
        private static UIA3Automation _automation = default!;

        /// <summary>
        /// Gets the main window of the applications process.
        /// </summary>
        private static Window _window = default!;

        /// <summary>
        /// Values for testing the percentages TextBox controls.
        /// </summary>
        private static IEnumerable<object[]> PercentagesTextBoxValues
            => [
                ["-100", "0", true],
                ["-50", "0", true],
                ["50", "50", true],
                ["99.9999", "100", true],
                ["100", "100", true],
                ["150", "100", true],
                ["1000", "100", true],
                ["82.4", "82", true],
                ["ABC", null, false]
            ];

        /// <summary>
        /// Values for testing the percentages Slider controls.
        /// </summary>
        private static IEnumerable<object[]> PercentagesSliderValues
            => [
                [0, "0%"],
                [50, "50%"],
                [99, "99%"],
                [100, "100%"],
                [82, "82%"]
            ];

        /// <summary>
        /// Values for testing the degrees TextBox controls.
        /// </summary>
        private static IEnumerable<object[]> DegreesTextBoxValues
            => [
                ["-100", "0", true],
                ["-0.5", "0", true],
                ["0.5", "0", true],
                ["0.999999", "1", true],
                ["300", "300", true],
                ["400", "360", true],
                ["5000", "360", true],
                ["ABC", null, false]
            ];

        /// <summary>
        /// Values for testing the degrees Slider controls.
        /// </summary>
        private static IEnumerable<object[]> DegreesSliderValues
            => [
                [0, "0°"],
                [10, "10°"],
                [200, "200°"],
                [360, "360°"]
            ];

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            if (_application is not null)
            {
                _application.Close();
                _application.Dispose();
            }

            _application = Application.Launch(@"..\..\..\..\ColorPicker.Wpf.App1\bin\Release\net8.0-windows\ColorPicker.Wpf.App1.exe");
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
        private static void ResetColorInputs()
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
        private static void ResetTextBox(string automationId, string value)
        {
            TextBox? textBox = _window.FindFirstDescendant(cf => cf.ByAutomationId(automationId))?.AsTextBox();
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
            if (_application is not null)
            {
                if (!_application.HasExited)
                {
                    _application.Close();
                }

                _application.Dispose();
            }

            _automation.Dispose();
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

                TextBox? hexTextBox = _window.FindFirstDescendant(cf => cf.ByAutomationId("PART_HexTextBox"))?.AsTextBox();
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
                TextBox? hexTextBox = _window.FindFirstDescendant(cf => cf.ByAutomationId("PART_HexTextBox"))?.AsTextBox();
                if (hexTextBox != null)
                {
                    hexTextBox.Text = input;
                    Keyboard.Press(VirtualKeyShort.TAB);

                    RetryResult<bool> retryResult = Retry.WhileFalse(
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
        private static IEnumerable<object[]> RgbTextBoxValues
            => [
                ["-100", false],
                ["-0.5", false],
                ["5", true],
                ["50", true],
                ["200", true],
                ["300", false],
                ["ABC", false]
            ];

        /// <summary>
        /// Values for testing the RGB Slider controls.
        /// </summary>
        private static IEnumerable<object[]> RgbSliderValues
            => [
                [50],
                [100],
                [255]
            ];

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
        public static void RgbTextBox_ValidateValue(string input, bool isValid, string textBoxId, string sliderId)
        {
            if (_window != null)
            {
                TextBox? textBox = _window.FindFirstDescendant(cf => cf.ByAutomationId(textBoxId))?.AsTextBox();
                if (textBox != null)
                {
                    textBox.Text = input;
                    Keyboard.Press(VirtualKeyShort.TAB);

                    if (isValid)
                    {
                        Slider? slider = _window.FindFirstDescendant(cf => cf.ByAutomationId(sliderId))?.AsSlider();
                        if (slider != null)
                        {
                            double value = double.Parse(input, NumberStyles.Number, NumberFormatInfo.InvariantInfo);
                            Assert.AreEqual(value, slider.Value);
                        }
                    }
                    else
                    {
                        RetryResult<bool> retryResult = Retry.WhileFalse(
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
        public static void RgbSlider_ValidateValue(double input, string sliderId, string textBoxId)
        {
            if (_window != null)
            {
                Slider? slider = _window.FindFirstDescendant(cf => cf.ByAutomationId(sliderId))?.AsSlider();
                if (slider != null)
                {
                    slider.Value = input;

                    TextBox? textBox = _window.FindFirstDescendant(cf => cf.ByAutomationId(textBoxId))?.AsTextBox();
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
        [DynamicData(nameof(PercentagesTextBoxValues), DynamicDataSourceType.Property)]
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
        [DynamicData(nameof(PercentagesTextBoxValues), DynamicDataSourceType.Property)]
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
        [DynamicData(nameof(PercentagesTextBoxValues), DynamicDataSourceType.Property)]
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
        [DynamicData(nameof(PercentagesTextBoxValues), DynamicDataSourceType.Property)]
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
        [DynamicData(nameof(PercentagesTextBoxValues), DynamicDataSourceType.Property)]
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
        [DynamicData(nameof(PercentagesTextBoxValues), DynamicDataSourceType.Property)]
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
        [DynamicData(nameof(PercentagesTextBoxValues), DynamicDataSourceType.Property)]
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
        [DynamicData(nameof(PercentagesTextBoxValues), DynamicDataSourceType.Property)]
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
        [DynamicData(nameof(PercentagesTextBoxValues), DynamicDataSourceType.Property)]
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
        private static void TextBox_ValidateValue(string input, string output, bool isInputNumber, string textBoxId, string sliderId)
        {
            if (_window != null)
            {
                TextBox? textBox = _window.FindFirstDescendant(cf => cf.ByAutomationId(textBoxId))?.AsTextBox();
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

                        Slider? slider = _window.FindFirstDescendant(cf => cf.ByAutomationId(sliderId))?.AsSlider();
                        if (slider != null)
                        {
                            value = double.Parse(output, NumberStyles.Number, NumberFormatInfo.InvariantInfo);
                            Assert.AreEqual(value, slider.Value);
                        }
                    }
                    else
                    {
                        RetryResult<bool> retryResult = Retry.WhileFalse(
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
        private static void Slider_ValidateValue(double input, string output, string sliderId, string textBoxId)
        {
            if (_window != null)
            {
                Slider? slider = _window.FindFirstDescendant(cf => cf.ByAutomationId(sliderId))?.AsSlider();
                TextBox? textBox = _window.FindFirstDescendant(cf => cf.ByAutomationId(textBoxId))?.AsTextBox();

                if (slider != null && textBox != null)
                {
                    slider.Value = input;

                    RetryResult<bool> retryResult = Retry.WhileFalse(
                            () => textBox.Text == output,
                            timeout: TimeSpan.FromMilliseconds(5000)
                        );

                    Assert.IsTrue(retryResult.Success);
                }
            }
        }
    }
}