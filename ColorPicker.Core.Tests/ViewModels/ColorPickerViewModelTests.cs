namespace ColorPicker.Core.Tests.ViewModels
{
    public class ColorPickerViewModelTests
    {
        [Fact]
        public void Constructor_InitializesAllModels()
        {
            // Arrange
            var vm = new ColorPickerViewModel();

            // Assert
            Assert.NotNull(vm.Rgb);
            Assert.NotNull(vm.Hsv);
            Assert.NotNull(vm.Hsl);
            Assert.NotNull(vm.Cmyk);
            Assert.NotNull(vm.Hex);
            Assert.NotNull(vm.Alpha);
        }

        [Fact]
        public void EnableHsl_RaisesPropertyChanged()
        {
            // Arrange
            var vm = new ColorPickerViewModel();

            bool raised = false;
            vm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(ColorPickerViewModel.EnableHsl))
                    raised = true;
            };

            // Act
            vm.EnableHsl = true;

            // Assert
            Assert.True(raised);
            Assert.True(vm.EnableHsl);
        }

        [Fact]
        public void EnableHsl_DoesNotRaiseEvent_WhenValueIsSame()
        {
            // Arrange
            var vm = new ColorPickerViewModel();

            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            // Act
            vm.EnableHsl = vm.EnableHsl;

            // Assert
            Assert.False(raised);
        }

        [Fact]
        public void EnableCmyk_RaisesPropertyChanged()
        {
            // Arrange
            var vm = new ColorPickerViewModel();

            bool raised = false;
            vm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(ColorPickerViewModel.EnableCmyk))
                    raised = true;
            };

            // Act
            vm.EnableCmyk = true;

            // Assert
            Assert.True(raised);
            Assert.True(vm.EnableCmyk);
        }

        [Fact]
        public void EnableCmyk_DoesNotRaiseEvent_WhenValueIsSame()
        {
            // Arrange
            var vm = new ColorPickerViewModel();

            bool raised = false;
            vm.PropertyChanged += (_, __) => raised = true;

            // Act
            vm.EnableCmyk = vm.EnableCmyk;

            // Assert
            Assert.False(raised);
        }

        [Fact]
        public void SelectColor_UpdatesRgbAndAlpha()
        {
            // Arrange
            var vm = new ColorPickerViewModel();

            vm.SelectColor(255, 0, 0, 0.5f);

            // RGB is publicly verifiable
            Assert.Equal(255, vm.Rgb.Red);
            Assert.Equal(0, vm.Rgb.Green);
            Assert.Equal(0, vm.Rgb.Blue);

            // Alpha model
            Assert.Equal(0.5f, vm.Alpha.Alpha, 6);
        }

        [Fact]
        public void SelectColor_RaisesPropertyChangedRgbForAlphaOnly()
        {
            // Arrange
            var vm = new ColorPickerViewModel();
            vm.SelectColor(100, 50, 25, 1f);

            var raised = new List<string>();
            vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName!);

            // Act
            vm.SelectColor(100, 50, 25, 0.33f);

            // Assert
            Assert.Contains(nameof(ColorPickerViewModel.Alpha), raised);
            Assert.DoesNotContain(nameof(ColorPickerViewModel.Rgb), raised);
            Assert.DoesNotContain(nameof(ColorPickerViewModel.Hsv), raised);
        }

        [Fact]
        public void SelectColor_UpdatesHexValueFromRgb()
        {
            // Arrange
            var vm = new ColorPickerViewModel();

            // Act
            vm.SelectColor(255, 0, 0, 1f);

            // Assert
            Assert.Equal("#FFFF0000", vm.Hex.Hex);
        }

        [Fact]
        public void SelectColor_UpdatesHsvAndAlpha()
        {
            // Arrange
            var vm = new ColorPickerViewModel();

            vm.SelectColor(300, 0.6f, 0.1f, 0.5f);

            // HSV is publicly verifiable
            Assert.Equal(300, vm.Hsv.Hue);
            Assert.Equal(0.6f, vm.Hsv.Saturation);
            Assert.Equal(0.1f, vm.Hsv.Value);

            // Alpha model
            Assert.Equal(0.5f, vm.Alpha.Alpha, 6);
        }

        [Fact]
        public void SelectColor_RaisesPropertyChangedHsvForAlphaOnly()
        {
            // Arrange
            var vm = new ColorPickerViewModel();
            vm.SelectColor(100, 50, 25, 1f);

            var raised = new List<string>();
            vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName!);

            // Act
            vm.SelectColor(100, 50, 25, 0.33f);

            // Assert
            Assert.Contains(nameof(ColorPickerViewModel.Alpha), raised);
            Assert.DoesNotContain(nameof(ColorPickerViewModel.Rgb), raised);
            Assert.DoesNotContain(nameof(ColorPickerViewModel.Hsv), raised);
        }

        [Fact]
        public void SelectColor_UpdatesHexValueFromHsv()
        {
            // Arrange
            var vm = new ColorPickerViewModel();

            // Act
            vm.SelectColor(300, 0.6f, 0.1f, 1f);

            // Assert
            Assert.Equal("#FF1A0A1A", vm.Hex.Hex);
        }
    }
}
