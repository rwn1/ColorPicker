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