using ColorPicker.Core.Utilities;

namespace ColorPicker.Tests.Utilities
{
    public class ColorConversionsTests
    {
        [Theory]
        [InlineData(255, 0, 0, 0, 1, 1)]
        [InlineData(0, 255, 0, 120, 1, 1)]
        [InlineData(0, 0, 255, 240, 1, 1)]
        [InlineData(255, 255, 255, 0, 0, 1)]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(128, 128, 128, 0, 0, 0.502)]
        public void RgbToHsv_PrimaryColors(byte r, byte g, byte b, float eh, float es, float ev)
        {
            ColorConversions.RgbToHsv(r, g, b, out float h, out float s, out float v);

            Assert.Equal(eh, h, 3);
            Assert.Equal(es, s, 3);
            Assert.Equal(ev, v, 3);
        }

        [Theory]
        [InlineData(0, 1, 1, 255, 0, 0)]
        [InlineData(120, 1, 1, 0, 255, 0)]
        [InlineData(240, 1, 1, 0, 0, 255)]
        public void HsvToRgb_PrimaryColors(float h, float s, float v, byte er, byte eg, byte eb)
        {
            ColorConversions.HsvToRgb(h, s, v, out byte r, out byte g, out byte b);

            Assert.Equal(er, r);
            Assert.Equal(eg, g);
            Assert.Equal(eb, b);
        }

        [Theory]
        [InlineData(10, 20, 30)]
        [InlineData(200, 100, 50)]
        [InlineData(0, 128, 255)]
        public void RgbToHsvBackToRgb_ReturnsOriginal(byte r, byte g, byte b)
        {
            ColorConversions.RgbToHsv(r, g, b, out float h, out float s, out float v);
            ColorConversions.HsvToRgb(h, s, v, out byte rr, out byte gg, out byte bb);

            Assert.InRange(rr, r - 1, r + 1);
            Assert.InRange(gg, g - 1, g + 1);
            Assert.InRange(bb, b - 1, b + 1);
        }

        [Theory]
        [InlineData(255, 0, 0, 0, 1, 0.5)]
        [InlineData(0, 255, 0, 120, 1, 0.5)]
        [InlineData(0, 0, 255, 240, 1, 0.5)]
        public void RgbToHsl_PrimaryColors(byte r, byte g, byte b, float eh, float es, float el)
        {
            ColorConversions.RgbToHsl(r, g, b, out float h, out float s, out float l);

            Assert.Equal(eh, h, 3);
            Assert.Equal(es, s, 3);
            Assert.Equal(el, l, 3);
        }

        [Theory]
        [InlineData(19.84, 0.5, 0.5)]
        [InlineData(200.16, 1.0, 0.25)]
        [InlineData(300, 0.29, 0.8)]
        public void HslToRgbRoundTrip(float h, float s, float l)
        {
            ColorConversions.HslToRgb(h, s, l, out byte r, out byte g, out byte b);

            ColorConversions.RgbToHsl(r, g, b, out float hh, out float ss, out float ll);

            Assert.Equal(h, hh, 2);
            Assert.Equal(s, ss, 2);
            Assert.Equal(l, ll, 2);
        }

        [Fact]
        public void HslToRgb_ZeroSaturation_ProducesGray()
        {
            ColorConversions.HslToRgb(123, 0, 0.6f, out byte r, out byte g, out byte b);

            Assert.Equal(r, g);
            Assert.Equal(g, b);
        }
    }
}