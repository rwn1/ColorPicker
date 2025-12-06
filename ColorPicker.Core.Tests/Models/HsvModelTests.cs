using ColorPicker.Core.Models;
using ColorPicker.Core.Utilities;

namespace ColorPicker.Core.Tests.Models
{
    public class HsvModelTests
    {
        [Fact]
        public void Hue_WhenValueChanges_RaisesPropertyChanged()
        {
            // Arrange
            var model = new HsvModel()
            {
                Hue = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Hue = 10;

            // Assert
            Assert.Equal(nameof(HsvModel.Hue), changedProp);
        }

        [Fact]
        public void Hue_WhenValueDoesNotChange_DoesNotRaisePropertyChanged()
        {
            // Arrange
            double initial = 10;
            var model = new HsvModel
            {
                Hue = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Hue = 10.000001;

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
        public void Hue_WhenSet_ClampsValue(double input, double expected)
        {
            // Arrange
            var model = new HsvModel();

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
        public void Hue_RaisesChangedOnlyWhenValueReallyChanges(
            double initial, double newValue, bool shouldRaise)
        {
            // Arrange
            var model = new HsvModel
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
            var model = new HsvModel()
            {
                Saturation = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Saturation = 0.5;

            // Assert
            Assert.Equal(nameof(HsvModel.Saturation), changedProp);
        }

        [Fact]
        public void Saturation_WhenValueDoesNotChange_DoesNotRaisePropertyChanged()
        {
            // Arrange
            double initial = 0.5;
            var model = new HsvModel
            {
                Saturation = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Saturation = 0.500000001;

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
        public void Saturation_WhenSet_ClampsValue(double input, double expected)
        {
            // Arrange
            var model = new HsvModel();

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
        public void Saturation_RaisesChangedOnlyWhenValueReallyChanges(
            double initial, double newValue, bool shouldRaise)
        {
            // Arrange
            var model = new HsvModel
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
        public void Value_WhenValueChanges_RaisesPropertyChanged() 
        {
            // Arrange
            var model = new HsvModel
            {
                Value = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Value = 0.5;

            // Assert
            Assert.Equal(nameof(HsvModel.Value), changedProp);
        }

        [Fact]
        public void Value_WhenValueDoesNotChange_DoesNotRaisePropertyChanged() 
        {
            // Arrange
            double initial = 0.5;
            var model = new HsvModel
            {
                Value = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Value = 0.500000001;

            // Assert
            Assert.Equal(initial, model.Value);
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
        public void Value_WhenSet_ClampsValue(double input, double expected)
        {
            // Arrange
            var model = new HsvModel();

            model.Value = input;

            // Assert
            Assert.Equal(expected, model.Value, 7); // tolerance 1e-7
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(0, 0.00000001, false)]
        [InlineData(0, 0.0001, true)]
        [InlineData(0.5, 0.51, true)]
        public void Value_RaisesChangedOnlyWhenValueReallyChanges(
            double initial, double newValue, bool shouldRaise)
        {
            // Arrange
            var model = new HsvModel
            {
                Value = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Value = newValue;

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
            double initialHue, double initialSaturation, double initialValue,
            double setHue, double setSaturation, double setValue,
            int expectedEventCount)
        {
            // Arrange
            var model = new HsvModel();
            model.SetFromHub(initialHue, initialSaturation, initialValue);

            var raised = new List<string>();
            model.PropertyChanged += (_, e) => raised.Add(e.PropertyName!);

            // Act
            model.SetFromHub(setHue, setSaturation, setValue);

            // Assert
            Assert.Equal(setHue, model.Hue);
            Assert.Equal(setSaturation, model.Saturation);
            Assert.Equal(setValue, model.Value);

            // Assert
            Assert.Equal(expectedEventCount, raised.Count);

            // Assert
            if (initialHue != setHue)
                Assert.Contains(nameof(HsvModel.Hue), raised);
            if (initialSaturation != setSaturation)
                Assert.Contains(nameof(HsvModel.Saturation), raised);
            if (initialValue != setValue)
                Assert.Contains(nameof(HsvModel.Value), raised);
        }

        [Fact]
        public void ToRgb_ReturnsSameValuesAsColorConversions()
        {
            // Arrange
            var model = new HsvModel
            {
                Hue = 120,
                Saturation = 0.6,
                Value = 0.6
            };

            ColorConversions.HsvToRgb(120, 0.6, 0.6, out byte r1, out byte g1, out byte b1);

            // Act
            model.ToRgb(out byte r2, out byte g2, out byte b2);

            // Assert
            Assert.Equal(r1, r2);
            Assert.Equal(g1, g2);
            Assert.Equal(b1, b2);
        }

        [Fact]
        public void FromRgb_UpdatesHsvAccordingToColorConversions()
        {
            // Arrange
            byte r = 10, g = 200, b = 100;
            ColorConversions.RgbToHsv(r, g, b, out double h, out double s, out double v);

            var model = new HsvModel();

            // Act
            model.FromRgb(r, g, b);

            // Assert
            Assert.Equal(h, model.Hue, 6);
            Assert.Equal(s, model.Saturation, 6);
            Assert.Equal(v, model.Value, 6);
        }

        [Fact]
        public void FromRgb_RaisesPropertyChangedForHsvInCorrectOrder()
        {
            // Arrange
            var model = new HsvModel()
            {
                Hue = 123,
                Saturation = 0.5,
                Value = 0.75
            };

            var raised = new List<string>();

            model.PropertyChanged += (sender, args) => raised.Add(args.PropertyName!);

            // Act
            model.FromRgb(255, 0, 0); // Red → HSV(0,1,1)

            // Assert
            Assert.Equal(new[] { nameof(HsvModel.Hue), nameof(HsvModel.Saturation), nameof(HsvModel.Value) }, raised);
        }

        [Fact]
        public void FromRgb_WhenValuesAreSame_DoesNotRaisePropertyChanged()
        {
            // Arrange
            var model = new HsvModel()
            {
                Hue = 0,
                Saturation = 1,
                Value = 1
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.FromRgb(255, 0, 0); // RGB(255,0,0) == HSV(0,1,1)

            // Assert
            Assert.False(raised);
        }
    }
}