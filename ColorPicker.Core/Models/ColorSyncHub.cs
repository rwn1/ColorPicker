using ColorPicker.Core.Models;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

/// <summary>
/// An object representing the main hub for synchronization between individual color models.
/// </summary>
public class ColorSyncHub: ObservableObject
{
    /// <summary>
    /// Color models.
    /// </summary>
    public readonly RgbModel Rgb = new RgbModel();
    public readonly HsvModel Hsv = new HsvModel();
    public readonly HslModel Hsl = new HslModel();
    public readonly CmykModel Cmyk = new CmykModel();
    public readonly HexModel Hex = new HexModel();
    public readonly AlphaModel Alpha = new AlphaModel();

    internal bool EnableHsl = true;
    internal bool EnableCmyk = true;

    private bool IsSyncing = false;

    /// <summary>
    /// Initializes a new instance of the ColorSyncHub class.
    /// </summary>
    public ColorSyncHub()
    {
        Rgb.Changed += Rgb_Changed;
        Hsv.Changed += Hsv_Changed;
        Hsl.Changed += Hsl_Changed;
        Cmyk.Changed += Cmyk_Changed;
        Hex.Changed += Hex_Changed;
        Alpha.Changed += Alpha_Changed;
    }

    /// <summary>
    /// Update color models from change in RGB model from UI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Rgb_Changed(object sender, EventArgs e)
    {
        if (IsSyncing) return;
        IsSyncing = true;

        // Current rgb state
        byte r = Rgb.Red;
        byte g = Rgb.Green;
        byte b = Rgb.Blue;

        // Update other modules
        Hsv.FromRgb(r, g, b);
        if (EnableHsl) Hsl.FromRgb(r, g, b);
        if (EnableCmyk) Cmyk.FromRgb(r, g, b);
        Hex.SetFromHub(ToHexArgbString(r, g, b, Alpha.Alpha));

        IsSyncing = false;
    }

    /// <summary>
    /// Update color models from change in HSL model from UI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Hsv_Changed(object sender, EventArgs e)
    {
        if (IsSyncing) return;
        IsSyncing = true;

        // convert HSV -> RGB
        Hsv.ToRgb(out byte r, out byte g, out byte b);

        // update RGB (without firing Rgb.Changed)
        Rgb.SetFromHub(r, g, b);

        // update other modules
        if (EnableHsl) Hsl.FromRgb(r, g, b);
        if (EnableCmyk) Cmyk.FromRgb(r, g, b);
        Hex.SetFromHub(ToHexArgbString(r, g, b, Alpha.Alpha));

        IsSyncing = false;
    }

    /// <summary>
    /// Update color models from change in HSL model from UI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Hsl_Changed(object sender, EventArgs e)
    {
        if (IsSyncing) return;
        IsSyncing = true;

        Hsl.ToRgb(out byte r, out byte g, out byte b);
        Rgb.SetFromHub(r, g, b);

        Hsv.FromRgb(r, g, b);
        if (EnableCmyk) Cmyk.FromRgb(r, g, b);
        Hex.SetFromHub(ToHexArgbString(r, g, b, Alpha.Alpha));

        IsSyncing = false;
    }

    /// <summary>
    /// Update color models from change in CMYK model from UI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Cmyk_Changed(object sender, EventArgs e)
    {
        if (IsSyncing) return;
        IsSyncing = true;

        Cmyk.ToRgb(out byte r, out byte g, out byte b);
        Rgb.SetFromHub(r, g, b);

        Hsv.FromRgb(r, g, b);
        if (EnableHsl) Hsl.FromRgb(r, g, b);
        Hex.SetFromHub(ToHexArgbString(r, g, b, Alpha.Alpha));

        IsSyncing = false;
    }

    /// <summary>
    /// Update color models from change in HEX model from UI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Hex_Changed(object sender, EventArgs e)
    {
        if (IsSyncing) return;

        string hex = Hex.Hex;
        if (string.IsNullOrEmpty(hex)) return;

        if (!Regex.IsMatch(hex, "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$")) return;

        IsSyncing = true;

        string h = hex;
        if (h.Length == 7) h = h.Insert(1, "FF");

        float alpha = int.Parse(h.Substring(1, 2), NumberStyles.HexNumber) / 255.0f;
        byte r = byte.Parse(h.Substring(3, 2), NumberStyles.HexNumber);
        byte g = byte.Parse(h.Substring(5, 2), NumberStyles.HexNumber);
        byte b = byte.Parse(h.Substring(7, 2), NumberStyles.HexNumber);

        Rgb.SetFromHub(r, g, b);
        Hsv.FromRgb(r, g, b);
        Alpha.SetFromHub(alpha);
        if (EnableHsl) Hsl.FromRgb(r, g, b);
        if (EnableCmyk) Cmyk.FromRgb(r, g, b);

        IsSyncing = false;
    }

    /// <summary>
    /// Update color models from change in Alpha model from UI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Alpha_Changed(object sender, EventArgs e)
    {
        if (IsSyncing) return;

        IsSyncing = true;

        Hex.SetFromHub(ToHexArgbString(Rgb.Red, Rgb.Green, Rgb.Blue, Alpha.Alpha));

        IsSyncing = false;
    }

    /// <summary>
    /// Sets the color to all color models.
    /// </summary>
    /// <param name="r">Red component.</param>
    /// <param name="g">Green component.</param>
    /// <param name="b">Blue component.</param>
    /// <param name="alpha">Alpha component.</param>
    public void SetColor(byte r, byte g, byte b, float alpha)
    {
        if (IsSyncing) return;
        IsSyncing = true;

        Rgb.SetFromHub(r, g, b);
        Hsv.FromRgb(r, g, b);
        Alpha.SetFromHub(alpha);
        if (EnableHsl) Hsl.FromRgb(r, g, b);
        if (EnableCmyk) Cmyk.FromRgb(r, g, b);
        Hex.SetFromHub(ToHexArgbString(r, g, b, alpha));

        IsSyncing = false;
    }

    /// <summary>
    /// Returns the color in hexadecimal format from RGB components.
    /// </summary>
    /// <param name="r">Red component.</param>
    /// <param name="g">Green component.</param>
    /// <param name="b">Blue component.</param>
    /// <param name="alpha">Alpha component.</param>
    /// <returns>The color in hexadecimal format from RGB components.</returns>
    private static string ToHexArgbString(byte r, byte g, byte b, float alpha)
    {
        byte a = (byte)Math.Round(Clamp01(alpha) * 255.0);
        return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", a, r, g, b);
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