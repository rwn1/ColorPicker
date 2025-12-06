using ColorPicker.Core.Models;

namespace ColorPicker.Tests.Models
{
    public class RgbModelTests
    {
        [Fact]
        public void Red_WhenValueChanges_RaisesPropertyChanged()
        {
            // Arrange
            var model = new RgbModel()
            {
                Red = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Red = 10;

            // Assert
            Assert.Equal(nameof(RgbModel.Red), changedProp);
        }

        [Fact]
        public void Red_WhenSameValue_DoesNotRaisePropertyChanged()
        {
            // Arrange
            var model = new RgbModel()
            {
                Red = 10
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Red = 10;

            // Assert
            Assert.False(raised);
        }

        [Fact]
        public void Red_WhenSet_UpdatesValue()
        {
            // Arrange
            var model = new RgbModel();

            // Act
            model.Red = 50;

            // Assert
            Assert.Equal((byte)50, model.Red);
        }

        [Fact]
        public void Green_WhenValueChanges_RaisesPropertyChanged()
        {
            // Arrange
            var model = new RgbModel()
            {
                Green = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Green = 10;

            // Assert
            Assert.Equal(nameof(RgbModel.Green), changedProp);
        }

        [Fact]
        public void Green_WhenSameValue_DoesNotRaisePropertyChanged()
        {
            // Arrange
            var model = new RgbModel()
            {
                Green = 10
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Green = 10;

            // Assert
            Assert.False(raised);
        }

        [Fact]
        public void Green_WhenSet_UpdatesValue()
        {
            // Arrange
            var model = new RgbModel();

            // Act
            model.Green = 50;

            // Assert
            Assert.Equal((byte)50, model.Green);
        }

        [Fact]
        public void Blue_WhenValueChanges_RaisesPropertyChanged()
        {
            // Arrange
            var model = new RgbModel()
            {
                Blue = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Blue = 10;

            // Assert
            Assert.Equal(nameof(RgbModel.Blue), changedProp);
        }

        [Fact]
        public void Blue_WhenSameValue_DoesNotRaisePropertyChanged()
        {
            // Arrange
            var model = new RgbModel()
            {
                Blue = 10
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Blue = 10;

            // Assert
            Assert.False(raised);
        }

        [Fact]
        public void Blue_WhenSet_UpdatesValue()
        {
            // Arrange
            var model = new RgbModel();

            // Act
            model.Blue = 50;

            // Assert
            Assert.Equal((byte)50, model.Blue);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0, 0)]
        [InlineData(0, 0, 0, 50, 0, 0, 1)]
        [InlineData(0, 0, 0, 0, 50, 0, 1)]
        [InlineData(0, 0, 0, 0, 0, 50, 1)]
        [InlineData(0, 0, 0, 50, 50, 0, 2)]
        [InlineData(0, 0, 0, 0, 50, 50, 2)]
        [InlineData(0, 0, 0, 50, 0, 50, 2)] 
        [InlineData(0, 0, 0, 50, 50, 50, 3)]
        public void SetFromHub_ChangesOnlyChangedProperties(
                    byte initialRed, byte initialGreen, byte initialBlue,
                    byte setRed, byte setGreen, byte setBlue,
                    int expectedEventCount)
        {
            // Arrange
            var model = new RgbModel();
            model.SetFromHub(initialRed, initialGreen, initialBlue);

            var raised = new List<string>();
            model.PropertyChanged += (sender, args) => raised.Add(args.PropertyName!);

            // Act
            model.SetFromHub(setRed, setGreen, setBlue);

            // Assert
            Assert.Equal(setRed, model.Red);
            Assert.Equal(setGreen, model.Green);
            Assert.Equal(setBlue, model.Blue);

            // Assert
            Assert.Equal(expectedEventCount, raised.Count);

            // Assert
            if (initialRed != setRed)
                Assert.Contains(nameof(RgbModel.Red), raised);
            if (initialGreen != setGreen)
                Assert.Contains(nameof(RgbModel.Green), raised);
            if (initialBlue != setBlue)
                Assert.Contains(nameof(RgbModel.Blue), raised);
        }
    }
}