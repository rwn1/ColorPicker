using Bunit;
using ColorPicker.Core.Utilities;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using System.Drawing;

namespace ColorPicker.Blazor.Tests.bUnit;

public class ColorPickerTests : BunitContext
{
    private readonly RenderFragment<LayoutContext> _fullLayout = ctx => builder =>
    {
        // Color selection
        builder.AddContent(0, ctx.PART_ColorSelectionView);
        // Hue selection
        builder.AddContent(1, ctx.PART_HueSelectionView);
        // Alpha selection
        builder.AddContent(2, ctx.PART_AlphaSelectionView);
        // Selected color
        builder.AddContent(3, ctx.PART_SelectedColorView);
        // HEX
        builder.AddContent(4, ctx.PART_HexTextBox);
        // RGB
        builder.AddContent(5, ctx.PART_RedTextBox);
        builder.AddContent(6, ctx.PART_RedSlider);
        builder.AddContent(7, ctx.PART_GreenTextBox);
        builder.AddContent(8, ctx.PART_GreenSlider);
        builder.AddContent(9, ctx.PART_BlueTextBox);
        builder.AddContent(10, ctx.PART_BlueSlider);
        // Alpha
        builder.AddContent(11, ctx.PART_AlphaTextBox);
        builder.AddContent(12, ctx.PART_AlphaSlider);
        // HSV
        builder.AddContent(13, ctx.PART_HueTextBox);
        builder.AddContent(14, ctx.PART_HueSlider);
        builder.AddContent(15, ctx.PART_SaturationTextBox);
        builder.AddContent(16, ctx.PART_SaturationSlider);
        builder.AddContent(17, ctx.PART_ValueTextBox);
        builder.AddContent(18, ctx.PART_ValueSlider);
        // HSL
        builder.AddContent(19, ctx.PART_HslHueTextBox);
        builder.AddContent(20, ctx.PART_HslHueSlider);
        builder.AddContent(21, ctx.PART_HslSaturationTextBox);
        builder.AddContent(22, ctx.PART_HslSaturationSlider);
        builder.AddContent(23, ctx.PART_HslLightnessTextBox);
        builder.AddContent(24, ctx.PART_HslLightnessSlider);
        // CMYK
        builder.AddContent(25, ctx.PART_CyanTextBox);
        builder.AddContent(26, ctx.PART_CyanSlider);
        builder.AddContent(27, ctx.PART_MagentaTextBox);
        builder.AddContent(28, ctx.PART_MagentaSlider);
        builder.AddContent(29, ctx.PART_YellowTextBox);
        builder.AddContent(30, ctx.PART_YellowSlider);
        builder.AddContent(31, ctx.PART_KeyTextBox);
        builder.AddContent(32, ctx.PART_KeySlider);
    };

    public ColorPickerTests()
    {
        SetupJsModules();
    }

    private void SetupJsModules()
    {
        // Square background
        BunitJSModuleInterop square = JSInterop.SetupModule(
            "./_content/ColorPicker.View.Blazor/squareBackground.js");
        square.SetupVoid("initSquareBackground", _ => true).SetVoidResult();


        // Alpha selection foreground
        BunitJSModuleInterop alphaSelectionForeground = JSInterop.SetupModule(
            "./_content/ColorPicker.View.Blazor/AlphaSelection/alphaSelectionForeground.js");
        alphaSelectionForeground.SetupVoid("initAlphaSelectionForeground", _ => true).SetVoidResult();
        alphaSelectionForeground.SetupVoid("updateAlphaColor", _ => true).SetVoidResult();

        // Selection mark
        BunitJSModuleInterop selectionMarker = JSInterop.SetupModule(
            "./_content/ColorPicker.View.Blazor/selectionMark.js");
        selectionMarker.SetupVoid("initSelectionMarker", _ => true).SetVoidResult();
        selectionMarker.SetupVoid("updateSelectionMarker", _ => true).SetVoidResult();

        // Color selection background
        BunitJSModuleInterop colorSelectionBackground = JSInterop.SetupModule(
            "./_content/ColorPicker.View.Blazor/ColorSelection/colorSelectionBackground.js");
        colorSelectionBackground.SetupVoid("initColorSelectionBackground", _ => true).SetVoidResult();
        colorSelectionBackground.SetupVoid("updateColorSelectionBackground", _ => true).SetVoidResult();

        // Color selection mark
        BunitJSModuleInterop colorSelectionMark = JSInterop.SetupModule(
            "./_content/ColorPicker.View.Blazor/ColorSelection/colorSelectionMark.js");
        colorSelectionMark.SetupVoid("initColorSelectionMarker", _ => true).SetVoidResult();
        colorSelectionMark.SetupVoid("updateColorSelectionMarker", _ => true).SetVoidResult();

        // Hue selection background
        BunitJSModuleInterop hue = JSInterop.SetupModule(
            "./_content/ColorPicker.View.Blazor/HueSelection/hueSelectionBackground.js");
        hue.SetupVoid("initHueSelectionBackground", _ => true).SetVoidResult();

        // Selected color foreground
        BunitJSModuleInterop selectedColorForeground = JSInterop.SetupModule(
            "./_content/ColorPicker.View.Blazor/SelectedColor/selectedColorForeground.js");
        selectedColorForeground.SetupVoid("initSelectedColorForeground", _ => true).SetVoidResult();
        selectedColorForeground.SetupVoid("updateSelectedColorForeground", _ => true).SetVoidResult();
    }

    [Fact]
    public void ColorPicker_renders_all_layout_parts()
    {
        // Arrange-Act-Render
        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
        });

        // Assert

        // Color selection
        cut.Find("[data-part=color-selection-view]");
        // Hue selection
        cut.Find("[data-part=hue-selection-view]");
        // Alpha selection
        cut.Find("[data-part=alpha-selection-view]");
        // Selected color
        cut.Find("[data-part=selected-color-view]");
        // HEX
        cut.Find("[data-part=hex-text-box]");
        // RGB
        cut.Find("[data-part=red-text-box]");
        cut.Find("[data-part=red-slider]");
        cut.Find("[data-part=green-text-box]");
        cut.Find("[data-part=green-slider]");
        cut.Find("[data-part=blue-text-box]");
        cut.Find("[data-part=blue-slider]");
        // Alpha
        cut.Find("[data-part=alpha-text-box]");
        cut.Find("[data-part=alpha-slider]");
        // HSV
        cut.Find("[data-part=hue-text-box]");
        cut.Find("[data-part=hue-slider]");
        cut.Find("[data-part=saturation-text-box]");
        cut.Find("[data-part=saturation-slider]");
        cut.Find("[data-part=value-text-box]");
        cut.Find("[data-part=value-slider]");
        // HSL
        cut.Find("[data-part=hsl-hue-text-box]");
        cut.Find("[data-part=hsl-hue-slider]");
        cut.Find("[data-part=hsl-saturation-text-box]");
        cut.Find("[data-part=hsl-saturation-slider]");
        cut.Find("[data-part=hsl-lightness-text-box]");
        cut.Find("[data-part=hsl-lightness-slider]");
        // CMYK
        cut.Find("[data-part=cyan-text-box]");
        cut.Find("[data-part=cyan-slider]");
        cut.Find("[data-part=magenta-text-box]");
        cut.Find("[data-part=magenta-slider]");
        cut.Find("[data-part=yellow-text-box]");
        cut.Find("[data-part=yellow-slider]");
        cut.Find("[data-part=key-text-box]");
        cut.Find("[data-part=key-slider]");
    }

    #region HEX

    [Theory]
    [InlineData("#FF0000", 255, 255, 0, 0)]
    [InlineData("#80FF0000", 128, 255, 0, 0)]
    [InlineData("#4000FF00", 64, 0, 255, 0)]
    [InlineData("#00000000", 0, 0, 0, 0)]
    [InlineData("#FFFFFFFF", 255, 255, 255, 255)]
    public void HexTextBox_UpdatesSelectedColor(string hex, byte expectedA, byte expectedR, byte expectedG, byte expectedB)
    {
        // Arrange-Render
        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, Color.FromArgb(255, 0, 0, 0));
        });

        // Act
        cut.Find("[data-part='hex-text-box']").Change(hex);

        // Assert
        cut.WaitForAssertion(() =>
        {
            var c = cut.Instance.SelectedColor;
            c.A.Should().Be(expectedA);
            c.R.Should().Be(expectedR);
            c.G.Should().Be(expectedG);
            c.B.Should().Be(expectedB);
        });
    }

    #endregion

    #region RGB

    [Theory]
    [InlineData(-200, 0)]
    [InlineData(128, 128)]
    [InlineData(64, 64)]
    [InlineData(5, 5)]
    [InlineData(260, 255)]
    public void RedTextBox_UpdatesSelectedColor(int input, byte output)
    {
        // Arrange-Render
        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, Color.Black);
        });

        // Act
        cut.Find("[data-part='red-text-box']").Input(input.ToString());

        // Assert
        cut.WaitForAssertion(() =>
            cut.Instance.SelectedColor.R.Should().Be(output));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(128, 128)]
    [InlineData(64, 64)]
    [InlineData(5, 5)]
    [InlineData(255, 255)]
    public void RedSlider_UpdatesSelectedColor(byte input, byte output)
    {
        // Arrange-Render
        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, Color.Black);
        });

        // Act
        cut.Find("[data-part='red-slider']").Input(input);

        // Assert
        cut.WaitForAssertion(() =>
            cut.Instance.SelectedColor.R.Should().Be(output));
    }

    [Theory]
    [InlineData(-200, 0)]
    [InlineData(128, 128)]
    [InlineData(64, 64)]
    [InlineData(5, 5)]
    [InlineData(260, 255)]
    public void GreenTextBox_UpdatesSelectedColor(int input, byte output)
    {
        // Arrange-Render
        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, Color.Black);
        });

        // Act
        cut.Find("[data-part='green-text-box']").Input(input.ToString());

        // Assert
        cut.WaitForAssertion(() =>
            cut.Instance.SelectedColor.G.Should().Be(output));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(128, 128)]
    [InlineData(64, 64)]
    [InlineData(5, 5)]
    [InlineData(255, 255)]
    public void GreenSlider_UpdatesSelectedColor(byte input, byte output)
    {
        // Arrange-Render
        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, Color.Black);
        });

        // Act
        cut.Find("[data-part='green-slider']").Input(input);

        // Assert
        cut.WaitForAssertion(() =>
            cut.Instance.SelectedColor.G.Should().Be(output));
    }

    [Theory]
    [InlineData(-200, 0)]
    [InlineData(128, 128)]
    [InlineData(64, 64)]
    [InlineData(5, 5)]
    [InlineData(260, 255)]
    public void BlueTextBox_UpdatesSelectedColor(int input, byte output)
    {
        // Arrange-Render
        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, Color.Black);
        });

        // Act
        cut.Find("[data-part='blue-text-box']").Input(input.ToString());

        // Assert
        cut.WaitForAssertion(() =>
            cut.Instance.SelectedColor.B.Should().Be(output));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(128, 128)]
    [InlineData(64, 64)]
    [InlineData(5, 5)]
    [InlineData(255, 255)]
    public void BlueSlider_UpdatesSelectedColor(byte input, byte output)
    {
        // Arrange-Render
        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, Color.Black);
        });

        // Act
        cut.Find("[data-part='blue-slider']").Input(input);

        // Assert
        cut.WaitForAssertion(() =>
            cut.Instance.SelectedColor.B.Should().Be(output));
    }

    #endregion

    #region Alpha

    [Theory]
    [InlineData(200)]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(-214)]
    [InlineData(100)]
    public void AlphaTextBox_UpdatesSelectedColor(int alpha)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='alpha-text-box']").Input(alpha.ToString());

        // Assert
        cut.WaitForAssertion(() =>
        {
            var expectedAlpha = Math.Min(100, Math.Max(0, alpha));

            cut.Instance.SelectedColor.A.Should().Be((byte)Math.Round(255 * expectedAlpha / 100f));
        });
    }

    [Theory]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(100)]
    public void AlphaSlider_UpdatesSelectedColor(int alpha)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='alpha-slider']").Input(alpha);

        // Assert
        cut.WaitForAssertion(() =>
        {
            cut.Instance.SelectedColor.A.Should().Be((byte)Math.Round(255 * alpha / 100f));
        });
    }

    #endregion

    #region HSV

    [Theory]
    [InlineData(200)]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(-214)]
    [InlineData(360)]
    [InlineData(500)]
    public void HueTextBox_UpdatesSelectedColor(int hue)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToHsv(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out _,
            out float ss,
            out float vv
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='hue-text-box']").Input(hue.ToString());

        // Assert
        cut.WaitForAssertion(() =>
        {
            int expectedHue = Math.Min(360, Math.Max(0, hue));

            ColorConversions.HsvToRgb(
                expectedHue,
                ss,
                vv,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(200)]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(360)]
    public void HueSlider_UpdatesSelectedColor(int hue)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToHsv(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out _,
            out float ss,
            out float vv
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='hue-slider']").Input(hue);

        // Assert
        cut.WaitForAssertion(() =>
        {
            ColorConversions.HsvToRgb(
                hue,
                ss,
                vv,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(200)]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(-214)]
    [InlineData(100)]
    public void SaturationTextBox_UpdatesSelectedColor(int saturation)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToHsv(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float hh,
            out _,
            out float vv
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='saturation-text-box']").Input(saturation.ToString());

        // Assert
        cut.WaitForAssertion(() =>
        {
            int expectedSaturation = Math.Min(100, Math.Max(0, saturation));

            ColorConversions.HsvToRgb(
                hh,
                expectedSaturation / 100f,
                vv,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(100)]
    public void SaturationSlider_UpdatesSelectedColor(int saturation)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToHsv(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float hh,
            out _,
            out float vv
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='saturation-slider']").Input(saturation);

        // Assert
        cut.WaitForAssertion(() =>
        {
            ColorConversions.HsvToRgb(
                hh,
                saturation / 100f,
                vv,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(200)]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(-214)]
    [InlineData(100)]
    public void ValueTextBox_UpdatesSelectedColor(int value)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToHsv(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float hh,
            out float ss,
            out _
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='value-text-box']").Input(value.ToString());

        // Assert
        cut.WaitForAssertion(() =>
        {
            int expectedValue = Math.Min(100, Math.Max(0, value));

            ColorConversions.HsvToRgb(
                hh,
                ss,
                expectedValue / 100f,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(100)]
    public void ValueSlider_UpdatesSelectedColor(int value)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToHsv(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float hh,
            out float ss,
            out _
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='value-slider']").Input(value);

        // Assert
        cut.WaitForAssertion(() =>
        {
            ColorConversions.HsvToRgb(
                hh,
                ss,
                value / 100f,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    #endregion

    #region HSL

    [Theory]
    [InlineData(200)]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(-214)]
    [InlineData(360)]
    [InlineData(500)]
    public void HslHueTextBox_UpdatesSelectedColor(int hue)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToHsl(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out _,
            out float ss,
            out float ll
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='hsl-hue-text-box']").Input(hue.ToString());

        // Assert
        cut.WaitForAssertion(() =>
        {
            int expectedHue = Math.Min(360, Math.Max(0, hue));

            ColorConversions.HslToRgb(
                expectedHue,
                ss,
                ll,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(200)]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(360)]
    public void HslHueSlider_UpdatesSelectedColor(int hue)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToHsl(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out _,
            out float ss,
            out float ll
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='hsl-hue-slider']").Input(hue);

        // Assert
        cut.WaitForAssertion(() =>
        {
            ColorConversions.HslToRgb(
                hue,
                ss,
                ll,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(200)]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(-214)]
    [InlineData(100)]
    public void HslSaturationTextBox_UpdatesSelectedColor(int saturation)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToHsl(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float hh,
            out _,
            out float ll
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='hsl-saturation-text-box']").Input(saturation.ToString());

        // Assert
        cut.WaitForAssertion(() =>
        {
            int expectedSaturation = Math.Min(100, Math.Max(0, saturation));

            ColorConversions.HslToRgb(
                hh,
                expectedSaturation / 100f,
                ll,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(100)]
    public void HslSaturationSlider_UpdatesSelectedColor(int saturation)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToHsl(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float hh,
            out _,
            out float ll
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='hsl-saturation-slider']").Input(saturation);

        // Assert
        cut.WaitForAssertion(() =>
        {
            ColorConversions.HslToRgb(
                hh,
                saturation / 100f,
                ll,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(200)]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(-214)]
    [InlineData(100)]
    public void HslLightnessTextBox_UpdatesSelectedColor(int lightness)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToHsl(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float hh,
            out float ss,
            out _
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='hsl-lightness-text-box']").Input(lightness.ToString());

        // Assert
        cut.WaitForAssertion(() =>
        {
            int expectedLightness = Math.Min(100, Math.Max(0, lightness));

            ColorConversions.HslToRgb(
                hh,
                ss,
                expectedLightness / 100f,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(100)]
    public void HslLightnesseSlider_UpdatesSelectedColor(int lightness)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToHsl(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float hh,
            out float ss,
            out _
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='hsl-lightness-slider']").Input(lightness);

        // Assert
        cut.WaitForAssertion(() =>
        {
            ColorConversions.HslToRgb(
                hh,
                ss,
                lightness / 100f,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    #endregion

    #region CMYK

    [Theory]
    [InlineData(200)]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(-214)]
    [InlineData(100)]
    public void CyanTextBox_UpdatesSelectedColor(int cyan)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToCmyk(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out _,
            out float mm,
            out float yy,
            out float kk
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='cyan-text-box']").Input(cyan.ToString());

        // Assert
        cut.WaitForAssertion(() =>
        {
            int expectedCyan = Math.Min(100, Math.Max(0, cyan));

            ColorConversions.CmykToRgb(
                expectedCyan / 100f,
                mm,
                yy,
                kk,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(100)]
    public void CyanSlider_UpdatesSelectedColor(int cyan)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToCmyk(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out _,
            out float mm,
            out float yy,
            out float kk
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='cyan-slider']").Input(cyan);

        // Assert
        cut.WaitForAssertion(() =>
        {
            ColorConversions.CmykToRgb(
                cyan / 100f,
                mm,
                yy,
                kk,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(200)]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(-214)]
    [InlineData(100)]
    public void MagentaTextBox_UpdatesSelectedColor(int magenta)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToCmyk(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float cc,
            out _,
            out float yy,
            out float kk
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='magenta-text-box']").Input(magenta);

        // Assert
        cut.WaitForAssertion(() =>
        {
            int expectedMagenta = Math.Min(100, Math.Max(0, magenta));

            ColorConversions.CmykToRgb(
                cc,
                expectedMagenta / 100f,
                yy,
                kk,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(100)]
    public void MagentaSlider_UpdatesSelectedColor(int magenta)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToCmyk(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float cc,
            out _,
            out float yy,
            out float kk
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='magenta-slider']").Input(magenta);

        // Assert
        cut.WaitForAssertion(() =>
        {
            ColorConversions.CmykToRgb(
                cc,
                magenta / 100f,
                yy,
                kk,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(200)]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(-214)]
    [InlineData(100)]
    public void YellowTextBox_UpdatesSelectedColor(int yellow)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToCmyk(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float cc,
            out float mm,
            out _,
            out float kk
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='yellow-text-box']").Input(yellow);

        // Assert
        cut.WaitForAssertion(() =>
        {
            int expectedYellow = Math.Min(100, Math.Max(0, yellow));

            ColorConversions.CmykToRgb(
                cc,
                mm,
                expectedYellow / 100f,
                kk,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(100)]
    public void YellowSlider_UpdatesSelectedColor(int yellow)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToCmyk(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float cc,
            out float mm,
            out _,
            out float kk
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='yellow-slider']").Input(yellow);

        // Assert
        cut.WaitForAssertion(() =>
        {
            ColorConversions.CmykToRgb(
                cc,
                mm,
                yellow / 100f,
                kk,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(200)]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(-214)]
    [InlineData(100)]
    public void KeyTextBox_UpdatesSelectedColor(int key)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToCmyk(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float cc,
            out float mm,
            out float yy,
            out _
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='key-text-box']").Input(key);

        // Assert
        cut.WaitForAssertion(() =>
        {
            int expectedKey = Math.Min(100, Math.Max(0, key));

            ColorConversions.CmykToRgb(
                cc,
                mm,
                yy,
                expectedKey / 100f,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }

    [Theory]
    [InlineData(12)]
    [InlineData(64)]
    [InlineData(5)]
    [InlineData(100)]
    public void KeySlider_UpdatesSelectedColor(int key)
    {
        // Arrange
        var defaultColor = Color.FromArgb(255, 255, 0, 0);

        ColorConversions.RgbToCmyk(
            defaultColor.R,
            defaultColor.G,
            defaultColor.B,
            out float cc,
            out float mm,
            out float yy,
            out _
        );

        IRenderedComponent<View.Blazor.ColorPicker> cut = Render<View.Blazor.ColorPicker>(p =>
        {
            p.Add(x => x.Layout, _fullLayout);
            p.Add(x => x.SelectedColor, defaultColor);
        });

        // Act
        cut.Find("[data-part='key-slider']").Input(key);

        // Assert
        cut.WaitForAssertion(() =>
        {
            ColorConversions.CmykToRgb(
                cc,
                mm,
                yy,
                key / 100f,
                out byte rr,
                out byte gg,
                out byte bb
            );

            Color c = cut.Instance.SelectedColor;

            c.A.Should().Be(defaultColor.A);
            c.R.Should().Be(rr);
            c.G.Should().Be(gg);
            c.B.Should().Be(bb);
        });
    }
    #endregion
}