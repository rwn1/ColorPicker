using ColorPicker.Core.Models;

namespace ColorPicker.Core.Tests.Models
{
    public class CmykModelTests
    {
        [Fact]
        public void Cyan_WhenValueChanges_RaisesPropertyChanged()
        {
            // Arrange
            var model = new CmykModel()
            {
                Cyan = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Cyan = 0.5;

            // Assert
            Assert.Equal(nameof(CmykModel.Cyan), changedProp);
        }

        [Fact]
        public void Cyan_WhenValueDoesNotChange_DoesNotRaisePropertyChanged()
        {
            // Arrange
            double initial = 0.5;
            var model = new CmykModel
            {
                Cyan = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Cyan = 0.50000001;

            // Assert
            Assert.Equal(initial, model.Cyan);
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
        public void Cyan_WhenSet_ClampsValue(double input, double expected)
        {
            // Arrange
            var model = new CmykModel();

            // Act
            model.Cyan = input;

            // Assert
            Assert.Equal(expected, model.Cyan, 7); // tolerance 1e-7
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(0, 0.00000001, false)]
        [InlineData(0, 0.0001, true)]
        [InlineData(0.5, 0.51, true)]
        public void Cyan_RaisesChangedOnlyWhenValueReallyChanges(
            double initial, double newValue, bool shouldRaise)
        {
            // Arrange
            var model = new CmykModel
            {
                Cyan = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Cyan = newValue;

            // Assert
            Assert.Equal(shouldRaise, raised);
        }

        [Fact]
        public void Magenta_WhenValueChanges_RaisesPropertyChanged()
        {
            // Arrange
            var model = new CmykModel()
            {
                Magenta = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Magenta = 0.5;

            // Assert
            Assert.Equal(nameof(CmykModel.Magenta), changedProp);
        }

        [Fact]
        public void Magenta_WhenValueDoesNotChange_DoesNotRaisePropertyChanged()
        {
            // Arrange
            double initial = 0.5;
            var model = new CmykModel
            {
                Magenta = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Magenta = 0.50000001;

            // Assert
            Assert.Equal(initial, model.Magenta);
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
        public void Magenta_WhenSet_ClampsValue(double input, double expected)
        {
            // Arrange
            var model = new CmykModel();

            // Act
            model.Magenta = input;

            // Assert
            Assert.Equal(expected, model.Magenta, 7); // tolerance 1e-7
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(0, 0.00000001, false)]
        [InlineData(0, 0.0001, true)]
        [InlineData(0.5, 0.51, true)]
        public void Magenta_RaisesChangedOnlyWhenValueReallyChanges(
            double initial, double newValue, bool shouldRaise)
        {
            // Arrange
            var model = new CmykModel
            {
                Magenta = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Magenta = newValue;

            // Assert
            Assert.Equal(shouldRaise, raised);
        }

        [Fact]
        public void Yellow_WhenValueChanges_RaisesPropertyChanged()
        {
            // Arrange
            var model = new CmykModel()
            {
                Yellow = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Yellow = 0.5;

            // Assert
            Assert.Equal(nameof(CmykModel.Yellow), changedProp);
        }

        [Fact]
        public void Yellow_WhenValueDoesNotChange_DoesNotRaisePropertyChanged()
        {
            // Arrange
            double initial = 0.5;
            var model = new CmykModel
            {
                Yellow = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Yellow = 0.50000001;

            // Assert
            Assert.Equal(initial, model.Yellow);
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
        public void Yellow_WhenSet_ClampsValue(double input, double expected)
        {
            // Arrange
            var model = new CmykModel();

            // Act
            model.Yellow = input;

            // Assert
            Assert.Equal(expected, model.Yellow, 7); // tolerance 1e-7
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(0, 0.00000001, false)]
        [InlineData(0, 0.0001, true)]
        [InlineData(0.5, 0.51, true)]
        public void Yellow_RaisesChangedOnlyWhenValueReallyChanges(
            double initial, double newValue, bool shouldRaise)
        {
            // Arrange
            var model = new CmykModel
            {
                Yellow = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Yellow = newValue;

            // Assert
            Assert.Equal(shouldRaise, raised);
        }

        [Fact]
        public void Key_WhenValueChanges_RaisesPropertyChanged()
        {
            // Arrange
            var model = new CmykModel()
            {
                Key = 0
            };

            string? changedProp = null;
            model.PropertyChanged += (sender, args) => changedProp = args.PropertyName;

            // Act
            model.Key = 0.5;

            // Assert
            Assert.Equal(nameof(CmykModel.Key), changedProp);
        }

        [Fact]
        public void Key_WhenValueDoesNotChange_DoesNotRaisePropertyChanged()
        {
            // Arrange
            double initial = 0.5;
            var model = new CmykModel
            {
                Key = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Key = 0.50000001;

            // Assert
            Assert.Equal(initial, model.Key);
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
        public void Key_WhenSet_ClampsValue(double input, double expected)
        {
            // Arrange
            var model = new CmykModel();

            // Act
            model.Key = input;

            // Assert
            Assert.Equal(expected, model.Key, 7); // tolerance 1e-7
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(0, 0.00000001, false)]
        [InlineData(0, 0.0001, true)]
        [InlineData(0.5, 0.51, true)]
        public void Key_RaisesChangedOnlyWhenValueReallyChanges(
            double initial, double newValue, bool shouldRaise)
        {
            // Arrange
            var model = new CmykModel
            {
                Key = initial
            };

            bool raised = false;
            model.PropertyChanged += (_, __) => raised = true;

            // Act
            model.Key = newValue;

            // Assert
            Assert.Equal(shouldRaise, raised);
        }

        [Fact]
        public void SetFromHub_DoesNotRaiseChanged()
        {
            // Arrange
            var model = new CmykModel()
            {
                Cyan = 0,
                Magenta = 0,
                Yellow = 0,
                Key = 0
            };

            bool changed = false;
            model.Changed += (_, __) => changed = true;

            bool propertyChange = false;
            model.PropertyChanged += (_, __) => propertyChange = true;

            // Act
            model.SetFromHub(0.1, 0.2, 0.3, 0.4);

            // Asser
            Assert.True(propertyChange);
            Assert.False(changed);
        }

        [Fact]
        public void SetFromHub_RaisesPropertyChangedOnlyForChangedValues()
        {
            // Arrange
            var model = new CmykModel
            {
                Cyan = 0.1,
                Magenta = 0.2,
                Yellow = 0.3,
                Key = 0.4
            };

            var raised = new List<string>();
            model.PropertyChanged += (sender, args) => raised.Add(args.PropertyName!);

            // Act
            model.SetFromHub(0.1, 0.25, 0.3, 0.9);

            // Assert
            Assert.Equal(2, raised.Count);
            Assert.Equal(new[] { nameof(CmykModel.Magenta), nameof(CmykModel.Key) }, raised);
        }

        [Fact]
        public void SetFromHub_UpdatesValuesCorrectly()
        {
            // Arrange
            var model = new CmykModel();

            // Act
            model.SetFromHub(0.3, 0.4, 0.5, 0.6);

            // Assert
            Assert.Equal(0.3, model.Cyan, 7);
            Assert.Equal(0.4, model.Magenta, 7);
            Assert.Equal(0.5, model.Yellow, 7);
            Assert.Equal(0.6, model.Key, 7);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0, 1)]
        [InlineData(255, 255, 255, 0, 0, 0, 0)]
        [InlineData(255, 0, 0, 0, 1, 1, 0)]
        [InlineData(0, 255, 0, 1, 0, 1, 0)]
        [InlineData(0, 0, 255, 1, 1, 0, 0)]
        public void FromRgb_ComputesCorrectCmyk(byte r, byte g, byte b,
            double ec, double em, double ey, double ek)
        {
            // Arrange
            var model = new CmykModel();

            // Act
            model.FromRgb(r, g, b);

            // Assert
            Assert.Equal(ec, model.Cyan, 6);
            Assert.Equal(em, model.Magenta, 6);
            Assert.Equal(ey, model.Yellow, 6);
            Assert.Equal(ek, model.Key, 6);
        }

        [Fact]
        public void FromRgb_RaisesPropertyChangedCorrectly()
        {
            // Arrange
            var model = new CmykModel
            {
                Cyan = 0.5,
                Magenta = 0.5,
                Yellow = 0.5,
                Key = 0.5
            };

            var raised = new List<string>();
            model.PropertyChanged += (_, e) => raised.Add(e.PropertyName!);

            // Act
            model.FromRgb(255, 0, 0);

            // Assert
            Assert.Contains(nameof(CmykModel.Cyan), raised);
            Assert.Contains(nameof(CmykModel.Magenta), raised);
            Assert.Contains(nameof(CmykModel.Yellow), raised);
            Assert.Contains(nameof(CmykModel.Key), raised);

            Assert.Equal(4, raised.Count);
        }

        [Theory]
        [InlineData(0, 1, 1, 0, 255, 0, 0)]
        [InlineData(1, 0, 1, 0, 0, 255, 0)]
        [InlineData(1, 1, 0, 0, 0, 0, 255)]
        [InlineData(0, 0, 0, 1, 0, 0, 0)]
        [InlineData(0, 0, 0, 0, 255, 255, 255)]
        public void ToRgb_ProducesCorrectValues(
            double c, double m, double y, double k,
            byte er, byte eg, byte eb)
        {
            // Arrange
            var model = new CmykModel
            {
                Cyan = c,
                Magenta = m,
                Yellow = y,
                Key = k
            };

            // Act
            model.ToRgb(out byte r, out byte g, out byte b);

            // Assert
            Assert.Equal(er, r);
            Assert.Equal(eg, g);
            Assert.Equal(eb, b);
        }
    }
}