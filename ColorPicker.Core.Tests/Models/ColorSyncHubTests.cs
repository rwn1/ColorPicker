namespace ColorPicker.Core.Tests.Models
{
    public class ColorSyncHubTests
    {
        [Fact]
        public void RgbChange_UpdatesHsvAndHex_WhenHslAndCmykDisabled()
        {
            // Arrange
            var hub = new ColorSyncHub();
            hub.EnableHsl = false;
            hub.EnableCmyk = false;
            
            // Act
            hub.Rgb.Red = 255;
            hub.Rgb.Green = 0;
            hub.Rgb.Blue = 0;

            // Assert
            Assert.Equal(0, hub.Hsv.Hue, 6);
            Assert.Equal(1, hub.Hsv.Saturation, 6);
            Assert.Equal(1, hub.Hsv.Value, 6);

            Assert.Equal("#FFFF0000", hub.Hex.Hex);
        }

        [Fact]
        public void RgbChange_UpdatesHslAndCmyk_WhenEnabled()
        {
            // Arrange
            var hub = new ColorSyncHub();
            hub.EnableHsl = true;
            hub.EnableCmyk = true;

            // Act
            hub.Rgb.Red = 255;
            hub.Rgb.Green = 0;
            hub.Rgb.Blue = 0;

            // Assert
            Assert.Equal(0, hub.Hsl.Hue, 6);
            Assert.Equal(1, hub.Hsl.Saturation, 6);
            Assert.Equal(0.5, hub.Hsl.Lightness, 6);

            Assert.Equal(0, hub.Cmyk.Cyan, 6);
            Assert.Equal(1, hub.Cmyk.Magenta, 6);
            Assert.Equal(1, hub.Cmyk.Yellow, 6);
            Assert.Equal(0, hub.Cmyk.Key, 6);
        }

        [Fact]
        public void HsvChange_UpdatesRgbAndHex()
        {
            // Arrange
            var hub = new ColorSyncHub();

            // Act
            hub.Hsv.Hue = 120;  // green
            hub.Hsv.Saturation = 1;
            hub.Hsv.Value = 1;

            // Assert
            Assert.Equal(0, hub.Rgb.Red);
            Assert.Equal(255, hub.Rgb.Green);
            Assert.Equal(0, hub.Rgb.Blue);

            Assert.Equal("#FF00FF00", hub.Hex.Hex);
        }

        [Fact]
        public void HslChange_UpdatesRgbAndHsvAndHex()
        {
            // Arrange
            var hub = new ColorSyncHub();
            hub.EnableHsl = true;

            // Act
            hub.Hsl.Hue = 240; // blue
            hub.Hsl.Saturation = 1;
            hub.Hsl.Lightness = 0.5;

            // Assert
            Assert.Equal(0, hub.Rgb.Red);
            Assert.Equal(0, hub.Rgb.Green);
            Assert.Equal(255, hub.Rgb.Blue);

            Assert.Equal(240, hub.Hsv.Hue, 6);
            Assert.Equal("#FF0000FF", hub.Hex.Hex);
        }

        [Fact]
        public void CmykChange_UpdatesRgbAndHsvAndHex()
        {
            // Arrange
            var hub = new ColorSyncHub();
            hub.EnableCmyk = true;

            // Act
            hub.Cmyk.Cyan = 0;
            hub.Cmyk.Magenta = 0;
            hub.Cmyk.Yellow = 1;
            hub.Cmyk.Key = 0;

            // Assert
            Assert.Equal(255, hub.Rgb.Red);
            Assert.Equal(255, hub.Rgb.Green);
            Assert.Equal(0, hub.Rgb.Blue);

            Assert.Equal("#FFFFFF00", hub.Hex.Hex);
        }

        [Fact]
        public void HexChange_UpdatesRgbHsvAndAlpha()
        {
            // Arrange
            var hub = new ColorSyncHub();
            hub.EnableHsl = true;
            hub.EnableCmyk = true;

            // Act
            hub.Hex.Hex = "#80FF00FF"; // ARGB = 80 FF 00 FF → purple, half alpha

            // Assert
            Assert.Equal(0.502, hub.Alpha.Alpha, 3);

            Assert.Equal(255, hub.Rgb.Red);
            Assert.Equal(0, hub.Rgb.Green);
            Assert.Equal(255, hub.Rgb.Blue);

            Assert.Equal(300, hub.Hsv.Hue, 1);
            Assert.Equal(1, hub.Hsv.Saturation, 6);
        }

        [Fact]
        public void SetColor_SetsAllModelsCorrectly()
        {
            // Arrange
            var hub = new ColorSyncHub();
            hub.EnableHsl = true;
            hub.EnableCmyk = true;

            // Act
            hub.SetColor(10, 20, 30, 0.75);

            // Assert
            Assert.Equal(10, hub.Rgb.Red);
            Assert.Equal(20, hub.Rgb.Green);
            Assert.Equal(30, hub.Rgb.Blue);
            Assert.Equal(0.75, hub.Alpha.Alpha);

            Assert.Equal("#BF0A141E", hub.Hex.Hex); // BF = 0xBF = 191 = 0.75 * 255
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(2, 1)]
        [InlineData(0.5, 0.5)]
        public void Alpha_ClampsCorrectly(double input, double expected)
        {
            // Arrange
            var hub = new ColorSyncHub();

            // Act
            hub.Alpha.Alpha = input;

            // Assert
            Assert.Equal(expected, hub.Alpha.Alpha);
        }

        [Fact]
        public void Sync_ProducesNoRecursiveLoops()
        {
            // Arrange
            var hub = new ColorSyncHub();

            int rgbChangedCount = 0;
            hub.Rgb.Changed += (_, _) => rgbChangedCount++;

            // Act
            hub.Rgb.Red = 200;

            // Assert
            Assert.Equal(1, rgbChangedCount);
        }
    }
}