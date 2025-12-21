using ColorPicker.Core.Models;

namespace ColorPicker.Core.Tests.Models
{
    public class HslModelTests
    {
        [Fact]
        public void Hue_WhenValueChanges_RaisesPropertyChanged()
        {
            // Arrange
            var model = new HslModel()
            {
                Hue = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Hue = 10;

            // Assert
            Assert.Equal(nameof(HslModel.Hue), changedProp);
        }

        [Fact]
        public void Hue_WhenValueDoesNotChange_DoesNotRaisePropertyChanged()
        {
            // Arrange
            float initial = 10;
            var model = new HslModel()
            {
                Hue = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Hue = 10.000001f;

            // Assert
            Assert.Equal(initial, model.Hue);
            Assert.False(raised);
        }

        [Theory]
        [InlineData(-100, 0)]
        [InlineData(0, 0)]
        [InlineData(10, 10)]
        [InlineData(359.9999, 359.9999)]
        [InlineData(360, 360)]
        [InlineData(500, 360)]
        public void Hue_WhenSet_ClampsValue(float input, float expected)
        {
            // Arrange
            var model = new HslModel();

            // Act
            model.Hue = input;

            // Assert
            Assert.Equal(expected, model.Hue, 5); // tolerance 1e-5
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(0, 0.000001, false)]
        [InlineData(0, 0.001, true)]
        [InlineData(50, 51, true)]
        public void Hue_RaisesChangedOnlyWhenValueReallyChanges(float initial, float newValue, bool shouldRaise)
        {
            // Arrange
            var model = new HslModel() 
            { 
                Hue = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Hue = newValue;

            // Assert
            Assert.Equal(shouldRaise, raised);
        }

        [Fact]
        public void Saturation_WhenValueChanges_RaisesPropertyChanged()
        {
            // Arrange
            var model = new HslModel()
            {
                Saturation = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Saturation = 0.5f;

            // Assert
            Assert.Equal(nameof(HslModel.Saturation), changedProp);
        }

        [Fact]
        public void Saturation_WhenValueDoesNotChange_DoesNotRaisePropertyChanged()
        {
            // Arrange
            float initial = 0.5f;
            var model = new HslModel
            {
                Saturation = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Saturation = 0.500000001f;

            // Assert
            Assert.Equal(initial, model.Saturation);
            Assert.False(raised);
        }

        [Theory]
        [InlineData(-100, 0)]
        [InlineData(-0.5, 0)]
        [InlineData(0, 0)]
        [InlineData(0.5, 0.5)]
        [InlineData(0.999999, 0.999999)]
        [InlineData(1, 1)]
        [InlineData(100, 1)]
        public void Saturation_WhenSet_ClampsValue(float input, float expected)
        {
            // Arrange
            var model = new HslModel();

            // Act
            model.Saturation = input;

            // Assert
            Assert.Equal(expected, model.Saturation, 7); // tolerance 1e-7
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(0, 0.00000001, false)]
        [InlineData(0, 0.0001, true)]
        [InlineData(0.5, 0.51, true)]
        public void Saturation_RaisesChangedOnlyWhenValueReallyChanges(float initial, float newValue, bool shouldRaise)
        {
            // Arrange
            var model = new HslModel
            {
                Saturation = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Saturation = newValue;

            // Assert
            Assert.Equal(shouldRaise, raised);
        }

        [Fact]
        public void Lightness_WhenValueChanges_RaisesPropertyChanged()
        {
            // Arrange
            var model = new HslModel
            {
                Lightness = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Lightness = 0.5f;

            // Assert
            Assert.Equal(nameof(HslModel.Lightness), changedProp);
        }

        [Fact]
        public void Lightness_WhenValueDoesNotChange_DoesNotRaisePropertyChanged()
        {
            // Arrange
            float initial = 0.5f;
            var model = new HslModel
            {
                Lightness = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Lightness = 0.500000001f;

            // Assert
            Assert.Equal(initial, model.Lightness);
            Assert.False(raised);
        }

        [Theory]
        [InlineData(-100, 0)]
        [InlineData(-0.5, 0)]
        [InlineData(0, 0)]
        [InlineData(0.5, 0.5)]
        [InlineData(0.999999, 0.999999)]
        [InlineData(1, 1)]
        [InlineData(100, 1)]
        public void Lightness_WhenSet_ClampsValue(float input, float expected)
        {
            // Arrange
            var model = new HslModel();

            model.Lightness = input;

            // Assert
            Assert.Equal(expected, model.Lightness, 7); // tolerance 1e-7
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(0, 0.00000001, false)]
        [InlineData(0, 0.0001, true)]
        [InlineData(0.5, 0.51, true)]
        public void Lightness_RaisesChangedOnlyWhenValueReallyChanges(float initial, float newValue, bool shouldRaise)
        {
            // Arrange
            var model = new HslModel
            {
                Lightness = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Lightness = newValue;

            // Assert
            Assert.Equal(shouldRaise, raised);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0, 0)]
        [InlineData(0, 0, 0, 50, 0, 0, 1)]
        [InlineData(0, 0, 0, 0, 0.5, 0, 1)]
        [InlineData(0, 0, 0, 0, 0, 0.5, 1)]
        [InlineData(0, 0, 0, 50, 0.5, 0, 2)]
        [InlineData(0, 0, 0, 0, 0.5, 0.5, 2)]
        [InlineData(0, 0, 0, 50, 0, 0.5, 2)]
        [InlineData(0, 0, 0, 50, 0.5, 0.5, 3)]
        public void SetFromHub_ChangesOnlyChangedProperties(
            float initialHue, float initialSaturation, float initialValue,
            float setHue, float setSaturation, float setValue,
            int expectedEventCount)
        {
            // Arrange
            var model = new HslModel();
            model.SetFromHub(initialHue, initialSaturation, initialValue);

            var raised = new List<string>();
            model.PropertyChanged += (sender, args) => raised.Add(args.PropertyName!);

            // Act
            model.SetFromHub(setHue, setSaturation, setValue);

            // Assert
            Assert.Equal(setHue, model.Hue);
            Assert.Equal(setSaturation, model.Saturation);
            Assert.Equal(setValue, model.Lightness);

            // Assert
            Assert.Equal(expectedEventCount, raised.Count);

            // Assert
            if (initialHue != setHue)
                Assert.Contains(nameof(HslModel.Hue), raised);
            if (initialSaturation != setSaturation)
                Assert.Contains(nameof(HslModel.Saturation), raised);
            if (initialValue != setValue)
                Assert.Contains(nameof(HslModel.Lightness), raised);
        }
    }
}