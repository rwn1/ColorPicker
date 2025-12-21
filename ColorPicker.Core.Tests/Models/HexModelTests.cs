using ColorPicker.Core.Models;

namespace ColorPicker.Core.Tests.Models
{
    public class HexModelTests
    {
        [Theory]
        [InlineData("#FFFFFF")]
        [InlineData("#000000")]
        [InlineData("#FF00FF")]
        [InlineData("#FFFFFFFF")]
        [InlineData("#80ABCDEF")]
        public void Hex_WhenValidValue_UpdatesAndRaisesChanged(string hex)
        {
            // Arrange
            var model = new HexModel()
            {
                Hex = "#FFFF0000"
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Hex = hex;

            // Assert
            Assert.Equal(hex, model.Hex);
            Assert.Equal(nameof(HexModel.Hex), changedProp);
        }

        [Theory]
        [InlineData("FFFFFF")]
        [InlineData("#FFF")]
        [InlineData("#FFFFF")]
        [InlineData("#GGGGGG")]
        [InlineData("#1234567")]
        [InlineData("#XYZXYZ")]
        [InlineData("#ABCDE")] 
        public void Hex_WhenInvalidValue_DoesNotChange(string hex)
        {
            // Arrange
            string initial = "#FFFF0000";
            var model = new HexModel()
            {
                Hex = initial
            };

            bool raised = false;
            model.PropertyChanged += (sender, e) => raised = true;

            // Act
            model.Hex = hex;

            // Assert
            Assert.Equal(initial, model.Hex);
            Assert.False(raised);
        }

        [Fact]
        public void SetFromHub_UpdatesHexAndRaisesPropertyChanged()
        {
            // Arrange
            string initial = "#FFFF0000";
            var model = new HexModel()
            {
                Hex = initial
            };

            string? raised = null;
            model.PropertyChanged += (_, e) => raised = e.PropertyName;

            // Act
            model.SetFromHub("#FF112233");

            // Assert
            Assert.Equal("#FF112233", model.Hex);
            Assert.Equal(nameof(HexModel.Hex), raised);
        }

        [Fact]
        public void SetFromHub_DoesNotRaiseChanged()
        {
            // Arrange
            string initial = "#FFFF0000";
            var model = new HexModel()
            {
                Hex = initial
            };

            bool changed = false;
            model.Changed += (_, __) => changed = true;

            bool propertyChange = false;
            model.PropertyChanged += (_, __) => propertyChange = true;


            // Act
            model.SetFromHub("#FF112233");

            // Assert
            Assert.True(propertyChange);
            Assert.False(changed);
        }

        [Theory]
        [InlineData("#FFFFFF", 255, 255, 255, 1.0)]
        [InlineData("#000000", 0, 0, 0, 1.0)]
        [InlineData("#FF0000", 255, 0, 0, 1.0)]
        [InlineData("#00FF00", 0, 255, 0, 1.0)]
        [InlineData("#0000FF", 0, 0, 255, 1.0)]
        public void ToRgba_RgbFormat_AssumesAlphaFF(string hex, byte r, byte g, byte b, float a)
        {
            // Arrange
            var model = new HexModel
            {
                Hex = hex
            };

            // Act
            model.ToRgba(out byte rr, out byte gg, out byte bb, out float aa);

            // Assert
            Assert.Equal(r, rr);
            Assert.Equal(g, gg);
            Assert.Equal(b, bb);
            Assert.Equal(a, aa, 6);
        }

        [Theory]
        [InlineData("#80FF0000", 255, 0, 0, 0.502)]
        [InlineData("#7F00FF00", 0, 255, 0, 0.498)]
        [InlineData("#400000FF", 0, 0, 255, 0.251)]
        public void ToRgba_WithAlpha_ParsesCorrectly(string hex, byte r, byte g, byte b, float expectedA)
        {
            // Arrange
            var model = new HexModel
            {
                Hex = hex
            };

            // Act
            model.ToRgba(out byte rr, out byte gg, out byte bb, out float a);

            // Assert
            Assert.Equal(r, rr);
            Assert.Equal(g, gg);
            Assert.Equal(b, bb);
            Assert.Equal(expectedA, a, 3);
        }
    }
}
