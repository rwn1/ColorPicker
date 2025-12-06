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
        public static void RgbToHsv(byte r, byte g, byte b, out double h, out double s, out double v)
        {
            double rd = r / 255.0;
            double gd = g / 255.0;
            double bd = b / 255.0;

            double max = Math.Max(rd, Math.Max(gd, bd));
            double min = Math.Min(rd, Math.Min(gd, bd));
            double d = max - min;

            if (d <= 1e-12)
            {
                h = 0.0;
            }
            else if (Math.Abs(max - rd) < 1e-12)
            {
                h = 60.0 * (((gd - bd) / d) % 6.0);
            }
            else if (Math.Abs(max - gd) < 1e-12)
            {
                h = 60.0 * (((bd - rd) / d) + 2.0);
            }
            else
            {
                h = 60.0 * (((rd - gd) / d) + 4.0);
            }
            if (h < 0) h += 360.0;

            v = max;
            s = (max <= 0.0) ? 0.0 : (d / max);
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
        public static void HsvToRgb(double h, double s, double v, out byte r, out byte g, out byte b)
        {
            double hh = h;
            while (hh < 0) hh += 360.0;
            while (hh >= 360) hh -= 360.0;

            double c = v * s;
            double hh6 = hh / 60.0;
            double x = c * (1.0 - Math.Abs((hh6 % 2.0) - 1.0));
            double m = v - c;

            double rp = 0, gp = 0, bp = 0;
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
        public static void RgbToHsl(byte r, byte g, byte b, out double h, out double s, out double l)
        {
            double rd = r / 255.0;
            double gd = g / 255.0;
            double bd = b / 255.0;

            double max = Math.Max(rd, Math.Max(gd, bd));
            double min = Math.Min(rd, Math.Min(gd, bd));
            double d = max - min;

            l = (max + min) / 2.0;

            if (d <= 1e-12)
            {
                h = 0.0;
                s = 0.0;
            }
            else
            {
                s = d / (1.0 - Math.Abs(2.0 * l - 1.0));
                if (Math.Abs(max - rd) < 1e-12)
                    h = 60.0 * (((gd - bd) / d) % 6.0);
                else if (Math.Abs(max - gd) < 1e-12)
                    h = 60.0 * (((bd - rd) / d) + 2.0);
                else
                    h = 60.0 * (((rd - gd) / d) + 4.0);

                if (h < 0) h += 360.0;
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
        public static void HslToRgb(double h, double s, double l, out byte r, out byte g, out byte b)
        {
            double hh = h;
            while (hh < 0) hh += 360.0;
            while (hh >= 360) hh -= 360.0;

            if (s <= 1e-12)
            {
                byte lv = (byte)Math.Round(Clamp01(l) * 255.0);
                r = g = b = lv;
                return;
            }

            double c = (1.0 - Math.Abs(2.0 * l - 1.0)) * s;
            double hh6 = hh / 60.0;
            double x = c * (1.0 - Math.Abs((hh6 % 2.0) - 1.0));
            double m = l - c / 2.0;

            double rp = 0, gp = 0, bp = 0;
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
        private static double Clamp01(double v)
        {
            if (v < 0) return 0;
            if (v > 1) return 1;
            return v;
        }
    }
}