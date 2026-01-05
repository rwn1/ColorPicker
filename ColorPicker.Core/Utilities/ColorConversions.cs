using System;

namespace ColorPicker.Core.Utilities
{
    /// <summary>
    /// Color convertions for models.
    /// </summary>
    public static class ColorConversions
    {
        /// <summary>
        /// Color convertions from RGB to HSV.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="h">Hue component.</param>
        /// <param name="s">Saturation component.</param>
        /// <param name="v">Value component.</param>
        public static void RgbToHsv(byte r, byte g, byte b, out float h, out float s, out float v)
        {
            float rd = r / 255.0f;
            float gd = g / 255.0f;
            float bd = b / 255.0f;

            float max = Math.Max(rd, Math.Max(gd, bd));
            float min = Math.Min(rd, Math.Min(gd, bd));
            float d = max - min;

            if (d <= 1e-12)
            {
                h = 0.0f;
            }
            else if (Math.Abs(max - rd) < 1e-12)
            {
                h = 60.0f * (((gd - bd) / d) % 6.0f);
            }
            else if (Math.Abs(max - gd) < 1e-12)
            {
                h = 60.0f * (((bd - rd) / d) + 2.0f);
            }
            else
            {
                h = 60.0f * (((rd - gd) / d) + 4.0f);
            }
            if (h < 0) h += 360.0f;

            v = max;
            s = (max <= 0.0f) ? 0.0f : (d / max);
        }

        /// <summary>
        /// Color convertions from HSV to RGB.
        /// </summary>
        /// <param name="h">Hue component.</param>
        /// <param name="s">Saturation component.</param>
        /// <param name="v">Value component.</param>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        public static void HsvToRgb(float h, float s, float v, out byte r, out byte g, out byte b)
        {
            float hh = h;
            while (hh < 0) hh += 360.0f;
            while (hh >= 360) hh -= 360.0f;

            float c = v * s;
            float hh6 = hh / 60.0f;
            float x = c * (1.0f - Math.Abs((hh6 % 2.0f) - 1.0f));
            float m = v - c;

            float rp = 0, gp = 0, bp = 0;
            int sector = (int)Math.Floor(hh6) % 6;
            switch (sector)
            {
                case 0: rp = c; gp = x; bp = 0; break;
                case 1: rp = x; gp = c; bp = 0; break;
                case 2: rp = 0; gp = c; bp = x; break;
                case 3: rp = 0; gp = x; bp = c; break;
                case 4: rp = x; gp = 0; bp = c; break;
                default: rp = c; gp = 0; bp = x; break;
            }

            r = (byte)Math.Round(Clamp01(rp + m) * 255.0);
            g = (byte)Math.Round(Clamp01(gp + m) * 255.0);
            b = (byte)Math.Round(Clamp01(bp + m) * 255.0);
        }

        /// <summary>
        /// Color convertions from RGB to HSL.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="h">Hue component.</param>
        /// <param name="s">Saturation component.</param>
        /// <param name="l">Lightness component.</param>
        public static void RgbToHsl(byte r, byte g, byte b, out float h, out float s, out float l)
        {
            float rd = r / 255.0f;
            float gd = g / 255.0f;
            float bd = b / 255.0f;

            float max = Math.Max(rd, Math.Max(gd, bd));
            float min = Math.Min(rd, Math.Min(gd, bd));
            float d = max - min;

            l = (max + min) / 2.0f;

            if (d <= 1e-12)
            {
                h = 0.0f;
                s = 0.0f;
            }
            else
            {
                s = d / (1.0f - Math.Abs(2.0f * l - 1.0f));
                if (Math.Abs(max - rd) < 1e-12)
                    h = 60.0f * (((gd - bd) / d) % 6.0f);
                else if (Math.Abs(max - gd) < 1e-12)
                    h = 60.0f * (((bd - rd) / d) + 2.0f);
                else
                    h = 60.0f * (((rd - gd) / d) + 4.0f);

                if (h < 0) h += 360.0f;
            }
        }

        /// <summary>
        /// Color convertions from HSL to RGB.
        /// </summary>
        /// <param name="h">Hue component.</param>
        /// <param name="s">Saturation component.</param>
        /// <param name="l">Lightness component.</param>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        public static void HslToRgb(float h, float s, float l, out byte r, out byte g, out byte b)
        {
            float hh = h;
            while (hh < 0) hh += 360.0f;
            while (hh >= 360) hh -= 360.0f;

            if (s <= 1e-12)
            {
                byte lv = (byte)Math.Round(Clamp01(l) * 255.0);
                r = g = b = lv;
                return;
            }

            float c = (1.0f - Math.Abs(2.0f * l - 1.0f)) * s;
            float hh6 = hh / 60.0f;
            float x = c * (1.0f - Math.Abs((hh6 % 2.0f) - 1.0f));
            float m = l - c / 2.0f;

            float rp = 0, gp = 0, bp = 0;
            int sector = (int)Math.Floor(hh6) % 6;
            switch (sector)
            {
                case 0: rp = c; gp = x; bp = 0; break;
                case 1: rp = x; gp = c; bp = 0; break;
                case 2: rp = 0; gp = c; bp = x; break;
                case 3: rp = 0; gp = x; bp = c; break;
                case 4: rp = x; gp = 0; bp = c; break;
                default: rp = c; gp = 0; bp = x; break;
            }

            r = (byte)Math.Round(Clamp01(rp + m) * 255.0);
            g = (byte)Math.Round(Clamp01(gp + m) * 255.0);
            b = (byte)Math.Round(Clamp01(bp + m) * 255.0);
        }

        /// <summary>
        /// Color convertions from RGB to CMYK.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="c">Cyan component.</param>
        /// <param name="m">Magenta component.</param>
        /// <param name="y">Yellow component.</param>
        /// <param name="k">Key (black) component.</param>
        public static void RgbToCmyk(byte r, byte g, byte b, out float c, out float m, out float y, out float k)
        {
            float rd = r / 255.0f;
            float gd = g / 255.0f;
            float bd = b / 255.0f;

            k = 1.0f - Math.Max(rd, Math.Max(gd, bd));
            c = 0.0f;
            m = 0.0f;
            y = 0.0f;

            if (Math.Abs(1.0 - k) > 1e-9)
            {
                c = (1.0f - rd - k) / (1.0f - k);
                m = (1.0f - gd - k) / (1.0f - k);
                y = (1.0f - bd - k) / (1.0f - k);
            }
            else
            {
                c = 0; m = 0; y = 0;
            }

            c = Clamp01(c);
            m = Clamp01(m);
            y = Clamp01(y);
            k = Clamp01(k);
        }

        /// <summary>
        /// Color conversions from CMYK to RGB.
        /// </summary>
        /// <param name="c">Cyan component (0–1).</param>
        /// <param name="m">Magenta component (0–1).</param>
        /// <param name="y">Yellow component (0–1).</param>
        /// <param name="k">Key (black) component (0–1).</param>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        public static void CmykToRgb(float c, float m, float y, float k, out byte r, out byte g, out byte b)
        {
            c = Clamp01(c);
            m = Clamp01(m);
            y = Clamp01(y);
            k = Clamp01(k);

            float rd = (1.0f - c) * (1.0f - k);
            float gd = (1.0f - m) * (1.0f - k);
            float bd = (1.0f - y) * (1.0f - k);

            r = (byte)Math.Round(rd * 255.0f);
            g = (byte)Math.Round(gd * 255.0f);
            b = (byte)Math.Round(bd * 255.0f);
        }

        /// <summary>
        /// Color convertions from HSV to HSL.
        /// </summary>
        /// <param name="h">Hue component.</param>
        /// <param name="s">Saturation component.</param>
        /// <param name="v">Value component.</param>
        /// <param name="h">Hue component.</param>
        /// <param name="s">Saturation component.</param>
        /// <param name="l">Lightness component.</param>
        public static void HsvToHsl(float h, float s, float v, out float hslH, out float hslS, out float hslL)
        {
            hslH = NormalizeHue(h);

            hslL = v * (1.0f - s / 2.0f);

            if (hslL <= 0.0 || hslL >= 1.0)
            {
                hslS = 0.0f;
            }
            else
            {
                hslS = (v - hslL) / Math.Min(hslL, 1.0f - hslL);
            }
        }

        /// <summary>
        /// Normalizes a hue value to the range [0, 360].
        /// </summary>
        /// <param name="h">Hue to normalize.</param>
        /// <returns>Normalized hue.</returns>
        private static float NormalizeHue(float h)
        {
            h %= 360.0f;
            if (h < 0) h += 360.0f;
            return h;
        }

        /// <summary>
        /// Returns the clamp value to the 0–1 range.
        /// </summary>
        /// <param name="v">Value to clamp.</param>
        /// <returns>The clamp value to the 0–1 range.</returns>
        private static float Clamp01(float v)
        {
            if (v < 0) return 0;
            if (v > 1) return 1;
            return v;
        }
    }
}