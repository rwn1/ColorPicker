using ColorPicker.Core.Models;

namespace ColorPicker.Core.Tests.Models
{
    public class AlphaModelTests
    {
        [Fact]
        public void Alpha_WhenValueChanges_RaisesPropertyChanged()
        {
            // Arrange
            var model = new AlphaModel()
            {
                Alpha = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Alpha = 0.5f;

            // Assert
            Assert.Equal(nameof(AlphaModel.Alpha), changedProp);
        }

        [Fact]
        public void Alpha_WhenValueDoesNotChange_DoesNotRaisePropertyChanged()
        {
            // Arrange
            float initial = 0.5f;
            var model = new AlphaModel
            {
                Alpha = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Alpha = 0.500000001f;

            // Assert
            Assert.Equal(initial, model.Alpha);
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
        public void Alpha_WhenSet_ClampsValue(float input, float expected)
        {
            // Arrange
            var model = new AlphaModel();

            // Act
            model.Alpha = input;

            // Assert
            Assert.Equal(expected, model.Alpha, 7); // tolerance 1e-7
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(0, 0.00000001, false)]
        [InlineData(0, 0.0001, true)]
        [InlineData(0.5, 0.51, true)]
        public void Alpha_RaisesChangedOnlyWhenValueReallyChanges(
            float initial, float newValue, bool shouldRaise)
        {
            // Arrange
            var model = new AlphaModel
            {
                Alpha = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Alpha = newValue;

            // Assert
            Assert.Equal(shouldRaise, raised);
        }

        [Fact]
        public void SetFromHub_DoesNotRaiseChanged()
        {
            // Arrange
            var model = new AlphaModel()
            {
                Alpha = 0.1f
            };

            bool changed = false;
            model.Changed += (_, __) => changed = true;

            bool propertyChange = false;
            model.PropertyChanged += (_, __) => propertyChange = true;

            // Act
            model.SetFromHub(0.5f);

            // Assert
            Assert.True(propertyChange);
            Assert.False(changed);
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(-5, 0, false)]
        [InlineData(0.1, 0.9, true)]
        [InlineData(1, 5, false)]
        [InlineData(0, 5, true)]
        public void SetFromHub_RaisesPropertyChangedForChangedValue(float initial, float newValue, bool isChanged)
        {
            // Arrange
            var model = new AlphaModel
            {
                Alpha = initial
            };

            var raised = false;
            model.PropertyChanged += (sender, args) => raised = args.PropertyName == nameof(AlphaModel.Alpha);

            // Act
            model.SetFromHub(newValue);

            // Assert
            if (isChanged)
                Assert.True(raised);
            else
                Assert.False(raised);
        }

        [Fact]
        public void SetFromHub_UpdatesValuesCorrectly()
        {
            // Arrange
            var model = new AlphaModel()
            {
                Alpha = 0.1f
            };

            // Act
            model.SetFromHub(0.6f);

            // Assert
            Assert.Equal(0.6f, model.Alpha);
        }
    }
}