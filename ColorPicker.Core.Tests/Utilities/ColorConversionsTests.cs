using ColorPicker.Core.Utilities;

namespace ColorPicker.Tests.Utilities
{
    public class ColorConversionsTests
    {
        private const float _tolerance = 0.001f;

        #region RGB <-> HSV

        [Theory]
        [InlineData(255, 0, 0, 0f, 1f, 1f)]     // Red
        [InlineData(0, 255, 0, 120f, 1f, 1f)]   // Green
        [InlineData(0, 0, 255, 240f, 1f, 1f)]   // Blue
        [InlineData(0, 0, 0, 0f, 0f, 0f)]       // Black
        [InlineData(255, 255, 255, 0f, 0f, 1f)] // White
        public void RgbToHsv_KnownColors_ReturnsExpectedValues(
            byte r, byte g, byte b,
            float expectedH, float expectedS, float expectedV)
        {
            ColorConversions.RgbToHsv(r, g, b, out float h, out float s, out float v);

            Assert.InRange(h, expectedH - _tolerance, expectedH + _tolerance);
            Assert.InRange(s, expectedS - _tolerance, expectedS + _tolerance);
            Assert.InRange(v, expectedV - _tolerance, expectedV + _tolerance);
        }

        [Fact]
        public void RgbToHsvAndBack_RoundTrip_ReturnsOriginalColor()
        {
            byte r = 123, g = 45, b = 200;

            ColorConversions.RgbToHsv(r, g, b, out float h, out float s, out float v);
            ColorConversions.HsvToRgb(h, s, v, out byte rr, out byte gg, out byte bb);

            Assert.Equal(r, rr);
            Assert.Equal(g, gg);
            Assert.Equal(b, bb);
        }

        #endregion

        #region RGB <-> HSL

        [Theory]
        [InlineData(255, 0, 0, 0f, 1f, 0.5f)]
        [InlineData(0, 255, 0, 120f, 1f, 0.5f)]
        [InlineData(0, 0, 255, 240f, 1f, 0.5f)]
        [InlineData(255, 255, 255, 0f, 0f, 1f)]
        [InlineData(0, 0, 0, 0f, 0f, 0f)]
        public void RgbToHsl_KnownColors_ReturnsExpectedValues(
            byte r, byte g, byte b,
            float expectedH, float expectedS, float expectedL)
        {
            ColorConversions.RgbToHsl(r, g, b, out float h, out float s, out float l);

            Assert.InRange(h, expectedH - _tolerance, expectedH + _tolerance);
            Assert.InRange(s, expectedS - _tolerance, expectedS + _tolerance);
            Assert.InRange(l, expectedL - _tolerance, expectedL + _tolerance);
        }

        [Fact]
        public void RgbToHslAndBack_RoundTrip_ReturnsOriginalColor()
        {
            byte r = 10, g = 200, b = 90;

            ColorConversions.RgbToHsl(r, g, b, out float h, out float s, out float l);
            ColorConversions.HslToRgb(h, s, l, out byte rr, out byte gg, out byte bb);

            Assert.Equal(r, rr);
            Assert.Equal(g, gg);
            Assert.Equal(b, bb);
        }

        #endregion

        #region RGB <-> CMYK

        [Theory]
        [InlineData(0, 0, 0, 0f, 0f, 0f, 1f)]
        [InlineData(255, 255, 255, 0f, 0f, 0f, 0f)]
        [InlineData(255, 0, 0, 0f, 1f, 1f, 0f)]
        [InlineData(0, 255, 0, 1f, 0f, 1f, 0f)]
        [InlineData(0, 0, 255, 1f, 1f, 0f, 0f)]
        public void RgbToCmyk_KnownColors_ReturnsExpectedValues(
            byte r, byte g, byte b,
            float expectedC, float expectedM, float expectedY, float expectedK)
        {
            ColorConversions.RgbToCmyk(r, g, b, out float c, out float m, out float y, out float k);

            Assert.InRange(c, expectedC - _tolerance, expectedC + _tolerance);
            Assert.InRange(m, expectedM - _tolerance, expectedM + _tolerance);
            Assert.InRange(y, expectedY - _tolerance, expectedY + _tolerance);
            Assert.InRange(k, expectedK - _tolerance, expectedK + _tolerance);
        }

        [Fact]
        public void RgbToCmykAndBack_RoundTrip_ReturnsOriginalColor()
        {
            byte r = 50, g = 100, b = 150;

            ColorConversions.RgbToCmyk(r, g, b, out float c, out float m, out float y, out float k);
            ColorConversions.CmykToRgb(c, m, y, k, out byte rr, out byte gg, out byte bb);

            Assert.Equal(r, rr);
            Assert.Equal(g, gg);
            Assert.Equal(b, bb);
        }

        #endregion

        #region HSV -> RGB

        [Theory]
        [InlineData(0f, 1f, 1f, 255, 0, 0)]
        [InlineData(120f, 1f, 1f, 0, 255, 0)]
        [InlineData(240f, 1f, 1f, 0, 0, 255)]
        [InlineData(0f, 0f, 0.5f, 128, 128, 128)]
        public void HsvToRgb_KnownValues_ReturnExpectedRgb(
            float h, float s, float v,
            byte er, byte eg, byte eb)
        {
            ColorConversions.HsvToRgb(h, s, v, out byte r, out byte g, out byte b);

            Assert.Equal(er, r);
            Assert.Equal(eg, g);
            Assert.Equal(eb, b);
        }

        #endregion

        #region HSL -> RGB

        [Theory]
        [InlineData(0f, 1f, 0.5f, 255, 0, 0)]
        [InlineData(120f, 1f, 0.5f, 0, 255, 0)]
        [InlineData(240f, 1f, 0.5f, 0, 0, 255)]
        [InlineData(0f, 0f, 0.25f, 64, 64, 64)]
        public void HslToRgb_KnownValues_ReturnExpectedRgb(
            float h, float s, float l,
            byte er, byte eg, byte eb)
        {
            ColorConversions.HslToRgb(h, s, l, out byte r, out byte g, out byte b);

            Assert.Equal(er, r);
            Assert.Equal(eg, g);
            Assert.Equal(eb, b);
        }

        #endregion

        #region CMYK -> RGB

        [Theory]
        [InlineData(0f, 0f, 0f, 1f, 0, 0, 0)]
        [InlineData(0f, 0f, 0f, 0f, 255, 255, 255)]
        [InlineData(0f, 1f, 1f, 0f, 255, 0, 0)]
        [InlineData(1f, 0f, 1f, 0f, 0, 255, 0)]
        [InlineData(1f, 1f, 0f, 0f, 0, 0, 255)]
        public void CmykToRgb_KnownValues_ReturnExpectedRgb(
            float c, float m, float y, float k,
            byte er, byte eg, byte eb)
        {
            ColorConversions.CmykToRgb(c, m, y, k, out byte r, out byte g, out byte b);

            Assert.Equal(er, r);
            Assert.Equal(eg, g);
            Assert.Equal(eb, b);
        }

        #endregion

        #region HSV -> HSL

        [Theory]
        [InlineData(0f, 1f, 1f, 0f, 1f, 0.5f)]
        [InlineData(120f, 1f, 1f, 120f, 1f, 0.5f)]
        [InlineData(0f, 0f, 0.75f, 0f, 0f, 0.75f)]
        public void HsvToHsl_KnownValues_ReturnExpectedHsl(
            float h, float s, float v,
            float eh, float es, float el)
        {
            ColorConversions.HsvToHsl(h, s, v, out float hh, out float ss, out float ll);

            Assert.InRange(hh, eh - _tolerance, eh + _tolerance);
            Assert.InRange(ss, es - _tolerance, es + _tolerance);
            Assert.InRange(ll, el - _tolerance, el + _tolerance);
        }

        #endregion

        #region Round-trip safety

        [Fact]
        public void FullRgb_Hsv_Rgb_RoundTrip()
        {
            byte r = 42, g = 128, b = 200;

            ColorConversions.RgbToHsv(r, g, b, out float h, out float s, out float v);
            ColorConversions.HsvToRgb(h, s, v, out byte rr, out byte gg, out byte bb);

            Assert.Equal(r, rr);
            Assert.Equal(g, gg);
            Assert.Equal(b, bb);
        }

        [Fact]
        public void FullRgb_Cmyk_Rgb_RoundTrip()
        {
            byte r = 70, g = 130, b = 180;

            ColorConversions.RgbToCmyk(r, g, b, out float c, out float m, out float y, out float k);
            ColorConversions.CmykToRgb(c, m, y, k, out byte rr, out byte gg, out byte bb);

            Assert.Equal(r, rr);
            Assert.Equal(g, gg);
            Assert.Equal(b, bb);
        }

        #endregion
    }
}