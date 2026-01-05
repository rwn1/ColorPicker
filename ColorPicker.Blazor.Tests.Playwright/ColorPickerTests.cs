using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

[TestFixture]
public class ColorPickerTests : PageTest
{
    private const string _baseUrl = "http://127.0.0.1:5000/colorPicker";

    [SetUp]
    public async Task SetUp()
    {
        await Page.GotoAsync(_baseUrl);

        // Default color
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");
        await hexTextBox.FillAsync("#FF000000");
    }

    //[Test]
    //public async Task Page_Loads_And_ColorPicker_Is_Visible()
    //{
    //    // Color selection
    //    ILocator colorSelectionView = Page.Locator("[data-part=color-selection-view]");
    //    // Hue selection
    //    ILocator hueSelectionView = Page.Locator("[data-part=hue-selection-view]");
    //    // Alpha selection
    //    ILocator alphaSelectionView = Page.Locator("[data-part=alpha-selection-view]");
    //    // Selected color
    //    ILocator selectedColorView = Page.Locator("[data-part=selected-color-view]");
    //    // HEX
    //    ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");
    //    // RGB
    //    ILocator redTextBox = Page.Locator("[data-part=red-text-box]");
    //    ILocator redSlider = Page.Locator("[data-part=red-slider]");
    //    ILocator greenTextBox = Page.Locator("[data-part=green-text-box]");
    //    ILocator greenSlider = Page.Locator("[data-part=green-slider]");
    //    ILocator blueTextBox = Page.Locator("[data-part=blue-text-box]");
    //    ILocator blueSlider = Page.Locator("[data-part=blue-slider]");
    //    // Alpha
    //    ILocator alphaTextBox = Page.Locator("[data-part=alpha-text-box]");
    //    ILocator alphaSlider = Page.Locator("[data-part=alpha-slider]");
    //    // HSV
    //    ILocator hueTextBox = Page.Locator("[data-part=hue-text-box]");
    //    ILocator hueSlider = Page.Locator("[data-part=hue-slider]");
    //    ILocator saturationTextBox = Page.Locator("[data-part=saturation-text-box]");
    //    ILocator saturationSlider = Page.Locator("[data-part=saturation-slider]");
    //    ILocator valueTextBox = Page.Locator("[data-part=value-text-box]");
    //    ILocator valueSlider = Page.Locator("[data-part=value-slider]");
    //    // HSL
    //    ILocator hslHueTextBox = Page.Locator("[data-part=hsl-hue-text-box]");
    //    ILocator hslHueSlider = Page.Locator("[data-part=hsl-hue-slider]");
    //    ILocator hslSaturationTextBox = Page.Locator("[data-part=hsl-saturation-text-box]");
    //    ILocator hslSaturationSlider = Page.Locator("[data-part=hsl-saturation-slider]");
    //    ILocator hslLightnessTextBox = Page.Locator("[data-part=hsl-lightness-text-box]");
    //    ILocator hslLightnessSlider = Page.Locator("[data-part=hsl-lightness-slider]");
    //    // CMYK
    //    ILocator cyanTextBox = Page.Locator("[data-part=cyan-text-box]");
    //    ILocator cyanSlider = Page.Locator("[data-part=cyan-slider]");
    //    ILocator magentaTextBox = Page.Locator("[data-part=magenta-text-box]");
    //    ILocator magentaSlider = Page.Locator("[data-part=magenta-slider]");
    //    ILocator yellowTextBox = Page.Locator("[data-part=yellow-text-box]");
    //    ILocator yellowSlider = Page.Locator("[data-part=yellow-slider]");
    //    ILocator keyTextBox = Page.Locator("[data-part=key-text-box]");
    //    ILocator keySlider = Page.Locator("[data-part=key-slider]");

    //    // Color selection
    //    await Expect(colorSelectionView).ToBeVisibleAsync();
    //    // Hue selection
    //    await Expect(hueSelectionView).ToBeVisibleAsync();
    //    // Alpha selection
    //    await Expect(alphaSelectionView).ToBeVisibleAsync();
    //    // Selected color
    //    await Expect(selectedColorView).ToBeVisibleAsync();
    //    // HEX
    //    await Expect(hexTextBox).ToBeVisibleAsync();
    //    // RGB
    //    await Expect(redTextBox).ToBeVisibleAsync();
    //    await Expect(redSlider).ToBeVisibleAsync();
    //    await Expect(greenTextBox).ToBeVisibleAsync();
    //    await Expect(greenSlider).ToBeVisibleAsync();
    //    await Expect(blueTextBox).ToBeVisibleAsync();
    //    await Expect(blueSlider).ToBeVisibleAsync();
    //    // Alpha
    //    await Expect(alphaTextBox).ToBeVisibleAsync();
    //    await Expect(alphaSlider).ToBeVisibleAsync();
    //    // HSV
    //    await Expect(hueTextBox).ToBeVisibleAsync();
    //    await Expect(hueSlider).ToBeVisibleAsync();
    //    await Expect(saturationTextBox).ToBeVisibleAsync();
    //    await Expect(saturationSlider).ToBeVisibleAsync();
    //    await Expect(valueTextBox).ToBeVisibleAsync();
    //    await Expect(valueSlider).ToBeVisibleAsync();
    //    // HSL
    //    await Expect(hslHueTextBox).ToBeVisibleAsync();
    //    await Expect(hslHueSlider).ToBeVisibleAsync();
    //    await Expect(hslSaturationTextBox).ToBeVisibleAsync();
    //    await Expect(hslSaturationSlider).ToBeVisibleAsync();
    //    await Expect(hslLightnessTextBox).ToBeVisibleAsync();
    //    await Expect(hslLightnessSlider).ToBeVisibleAsync();
    //    //  CMYK
    //    await Expect(cyanTextBox).ToBeVisibleAsync();
    //    await Expect(cyanSlider).ToBeVisibleAsync();
    //    await Expect(magentaTextBox).ToBeVisibleAsync();
    //    await Expect(magentaSlider).ToBeVisibleAsync();
    //    await Expect(yellowTextBox).ToBeVisibleAsync();
    //    await Expect(yellowSlider).ToBeVisibleAsync();
    //    await Expect(keyTextBox).ToBeVisibleAsync();
    //    await Expect(keySlider).ToBeVisibleAsync();
    //}

    [TestCase("#FF0000", 255, 0, 0, 100)]
    [TestCase("#00FF00", 0, 255, 0, 100)]
    [TestCase("#0000FF", 0, 0, 255, 100)]
    [TestCase("#FFFFFF", 255, 255, 255, 100)]
    [TestCase("#000000", 0, 0, 0, 100)]
    [TestCase("#80FF0000", 255, 0, 0, 50)]
    [TestCase("#4000FF00", 0, 255, 0, 25)]
    [TestCase("#FF0000FF", 0, 0, 255, 100)]
    [TestCase("#50000000", 0, 0, 0, 31)]
    public async Task Hex_Updates_RgbaTextBoxes(string hex, int r, int g, int b, int a)
    {
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");
        ILocator redTextBox = Page.Locator("[data-part=red-text-box]");
        ILocator greenTextBox = Page.Locator("[data-part=green-text-box]");
        ILocator blueTextBox = Page.Locator("[data-part=blue-text-box]");
        ILocator alphaTextBox = Page.Locator("[data-part=alpha-text-box]");

        if (hexTextBox == null)
            return;

        await hexTextBox.ClickAsync();
        await hexTextBox.FillAsync(hex);

        if (redTextBox != null)
            await Expect(redTextBox).ToHaveValueAsync(r.ToString());
    
        if (greenTextBox != null)
            await Expect(greenTextBox).ToHaveValueAsync(g.ToString());
    
        if (blueTextBox != null)
            await Expect(blueTextBox).ToHaveValueAsync(b.ToString());
    
        if (alphaTextBox != null)
           await Expect(alphaTextBox).ToHaveValueAsync($"{a.ToString()}%");
    }

    [TestCase("#FF0000", 255, 0, 0, 100)]
    [TestCase("#00FF00", 0, 255, 0, 100)]
    [TestCase("#0000FF", 0, 0, 255, 100)]
    [TestCase("#FFFFFF", 255, 255, 255, 100)]
    [TestCase("#000000", 0, 0, 0, 100)]
    [TestCase("#80FF0000", 255, 0, 0, 50)]
    [TestCase("#4000FF00", 0, 255, 0, 25)]
    [TestCase("#FF0000FF", 0, 0, 255, 100)]
    [TestCase("#50000000", 0, 0, 0, 31)]
    public async Task Hex_Updates_RgbaSliders(string hex, int r, int g, int b, int a)
    {
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");
        ILocator redSlider = Page.Locator("[data-part=red-slider]");
        ILocator greenSlider = Page.Locator("[data-part=green-slider]");
        ILocator blueSlider = Page.Locator("[data-part=blue-slider]");
        ILocator alphaSlider = Page.Locator("[data-part=alpha-slider]");

        if (hexTextBox == null)
            return;

        await hexTextBox.ClickAsync();
        await hexTextBox.FillAsync(hex);

        if (redSlider != null)
            await Expect(redSlider).ToHaveValueAsync(r.ToString());
    
        if (greenSlider != null)
           await Expect(greenSlider).ToHaveValueAsync(g.ToString());
    
        if  (blueSlider != null)
           await Expect(blueSlider).ToHaveValueAsync(b.ToString());
    
        if (alphaSlider != null)
            await Expect(alphaSlider).ToHaveValueAsync(a.ToString());
    }

    [TestCase("#FF0000", 0, 100, 100)]
    [TestCase("#00FF00", 120, 100, 100)]
    [TestCase("#0000FF", 240, 100, 100)]
    [TestCase("#FFFFFF", 0, 0, 100)]
    [TestCase("#000000", 0, 0, 0)]
    public async Task Hex_Updates_HsvTextBoxes(string hex, int h, int s, int v)
    {
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");
        ILocator hueTextBox = Page.Locator("[data-part=hue-text-box]");
        ILocator saturationTextBox = Page.Locator("[data-part=saturation-text-box]");
        ILocator valueTextBox = Page.Locator("[data-part=value-text-box]");

        if (hexTextBox == null)
            return;

        await hexTextBox.ClickAsync();
        await hexTextBox.FillAsync(hex);

        if (hueTextBox != null)
            await Expect(hueTextBox).ToHaveValueAsync($"{h.ToString()}°");
    
        if (saturationTextBox != null)
            await Expect(saturationTextBox).ToHaveValueAsync($"{s.ToString()}%");
    
        if (valueTextBox != null)
            await Expect(valueTextBox).ToHaveValueAsync($"{v.ToString()}%");
    }

    [TestCase("#FF0000", 0, 100, 100)]
    [TestCase("#00FF00", 120, 100, 100)]
    [TestCase("#0000FF", 240, 100, 100)]
    [TestCase("#FFFFFF", 0, 0, 100)]
    [TestCase("#000000", 0, 0, 0)]
    public async Task Hex_Updates_HsvSliderss(string hex, int h, int s, int v)
    {
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");
        ILocator hueSlider = Page.Locator("[data-part=hue-slider]");
        ILocator saturationSlider = Page.Locator("[data-part=saturation-slider]");
        ILocator valueSlider = Page.Locator("[data-part=value-slider]");

        if (hexTextBox == null)
            return;

        await hexTextBox.ClickAsync();
        await hexTextBox.FillAsync(hex);

        if (hueSlider != null)
            await Expect(hueSlider).ToHaveValueAsync(h.ToString());
    
        if (saturationSlider != null)
           await Expect(saturationSlider).ToHaveValueAsync(s.ToString());
    
        if (valueSlider != null)
           await Expect(valueSlider).ToHaveValueAsync(v.ToString());
    }

    [TestCase("#FF0000", 0, 100, 50)]
    [TestCase("#00FF00", 120, 100, 50)]
    [TestCase("#0000FF", 240, 100, 50)]
    [TestCase("#FFFFFF", 0, 0, 100)]
    [TestCase("#000000", 0, 0, 0)]
    public async Task Hex_Updates_HslTextBoxes(string hex, int h, int s, int l)
    {
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");
        ILocator hueTextBox = Page.Locator("[data-part=hsl-hue-text-box]");
        ILocator saturationTextBox = Page.Locator("[data-part=hsl-saturation-text-box]");
        ILocator lightnessTextBox = Page.Locator("[data-part=hsl-lightness-text-box]");

        if (hexTextBox == null)
            return;

        await hexTextBox.ClickAsync();
        await hexTextBox.FillAsync(hex);

        if (hueTextBox != null)
           await Expect(hueTextBox).ToHaveValueAsync($"{h.ToString()}°");
    
        if (saturationTextBox != null)
           await Expect(saturationTextBox).ToHaveValueAsync($"{s.ToString()}%");
    
        if (lightnessTextBox != null)
           await Expect(lightnessTextBox).ToHaveValueAsync($"{l.ToString()}%");
    }

    [TestCase("#00FF00", 120, 100, 50)]
    [TestCase("#0000FF", 240, 100, 50)]
    public async Task Hex_Updates_HslSliders(string hex, int h, int s, int l)
    {
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");
        ILocator hueSlider = Page.Locator("[data-part=hsl-hue-slider]");
        ILocator saturationSlider = Page.Locator("[data-part=hsl-saturation-slider]");
        ILocator lightnessSlider = Page.Locator("[data-part=hsl-lightness-slider]");

        if (hexTextBox == null)
            return;

        await hexTextBox.ClickAsync();
        await hexTextBox.FillAsync(hex);

        if (hueSlider != null)
            await Expect(hueSlider).ToHaveValueAsync(h.ToString());
    
        if (saturationSlider != null)
            await Expect(saturationSlider).ToHaveValueAsync(s.ToString());
    
        if (lightnessSlider != null)
            await Expect(lightnessSlider).ToHaveValueAsync(l.ToString());
    }

    [TestCase("#FF0000", 0, 100, 100, 0)]
    [TestCase("#00FF00", 100, 0, 100, 0)]
    [TestCase("#0000FF", 100, 100, 0, 0)]
    [TestCase("#FFFFFF", 0, 0, 0, 0)]
    [TestCase("#000000", 0, 0, 0, 100)]
    public async Task Hex_Updates_CmykTextBoxes(string hex, int c, int m, int y, int k)
    {
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");
        ILocator cyanTextBox = Page.Locator("[data-part=cyan-text-box]");
        ILocator magentaTextBox = Page.Locator("[data-part=magenta-text-box]");
        ILocator yellowTextBox = Page.Locator("[data-part=yellow-text-box]");
        ILocator keyTextBox = Page.Locator("[data-part=key-text-box]");

        if (hexTextBox == null)
            return;

        await hexTextBox.ClickAsync();
        await hexTextBox.FillAsync(hex);

        if (cyanTextBox !=  null)
            await Expect(cyanTextBox).ToHaveValueAsync($"{c.ToString()}%");
    
        if (magentaTextBox != null)
            await Expect(magentaTextBox).ToHaveValueAsync($"{m.ToString()}%");
    
        if (yellowTextBox != null)
            await Expect(yellowTextBox).ToHaveValueAsync($"{y.ToString()}%");
    
        if (keyTextBox != null)
            await Expect(keyTextBox).ToHaveValueAsync($"{k.ToString()}%");
    }

    [TestCase("#FF0000", 0, 100, 100, 0)]
    [TestCase("#00FF00", 100, 0, 100, 0)]
    [TestCase("#0000FF", 100, 100, 0, 0)]
    [TestCase("#FFFFFF", 0, 0, 0, 0)]
    [TestCase("#000000", 0, 0, 0, 100)]
    public async Task Hex_Updates_CmykSliders(string hex, int c, int m, int y, int k)
    {
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");
        ILocator cyanSlider = Page.Locator("[data-part=cyan-slider]");
        ILocator magentaSlider = Page.Locator("[data-part=magenta-slider]");
        ILocator yellowSlider = Page.Locator("[data-part=yellow-slider]");
        ILocator keySlider = Page.Locator("[data-part=key-slider]");

        if (hexTextBox == null)
            return;

        await hexTextBox.ClickAsync();
        await hexTextBox.FillAsync(hex);

        if (cyanSlider != null)
            await Expect(cyanSlider).ToHaveValueAsync(c.ToString());
    
        if (magentaSlider != null)
            await Expect(magentaSlider).ToHaveValueAsync(m.ToString());
    
        if (yellowSlider != null)
            await Expect(yellowSlider).ToHaveValueAsync(y.ToString());

        if (keySlider != null) 
            await Expect(keySlider).ToHaveValueAsync(k.ToString());
    }

    [TestCase(0, 100, 100, "#FFFF0000")]
    [TestCase(60, 50, 39, "#FF636332")]
    [TestCase(255, 100, 78, "#FF3200C7")]
    [TestCase(0, 66, 83, "#FFD44848")]
    [TestCase(180, 75, 39, "#FF196363")]
    public async Task HsvSliders_Updates_Hex(int h, int s, int v, string hex)
    {
        ILocator hueSlider = Page.Locator("[data-part=hue-slider]");
        ILocator saturationSlider = Page.Locator("[data-part=saturation-slider]");
        ILocator valueSlider = Page.Locator("[data-part=value-slider]");
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");

        if (hueSlider == null || saturationSlider == null || valueSlider == null || hexTextBox == null)
            return;

        await hueSlider.ClickAsync();
        await hueSlider.FillAsync(h.ToString());

        await saturationSlider.ClickAsync();
        await saturationSlider.FillAsync(s.ToString());

        await valueSlider.ClickAsync();
        await valueSlider.FillAsync(v.ToString());

        await Expect(hexTextBox).ToHaveValueAsync(hex);
    }

    [TestCase(0, 100, 100, "#FFFF0000")]
    [TestCase(60, 50, 39, "#FF636332")]
    [TestCase(255, 100, 78, "#FF3200C7")]
    [TestCase(0, 66, 83, "#FFD44848")]
    [TestCase(180, 75, 39, "#FF196363")]
    [TestCase(300, -40, 100, "#FFFFFFFF")]
    [TestCase(255, 100, 78, "#FF3200C7")]
    [TestCase(346, 100, 75, "#FFBF002D")]
    [TestCase(500, 83, 10, "#FF1A0404")]
    [TestCase(78, 83, 100, "#FFC0FF2B")]
    public async Task HsvTextBoxes_Updates_Hex(int h, int s, int v, string hex)
    {
        ILocator hueTextBox = Page.Locator("[data-part=hue-text-box]");
        ILocator saturationTextBox = Page.Locator("[data-part=saturation-text-box]");
        ILocator valueTextBox = Page.Locator("[data-part=value-text-box]");
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");

        if (hueTextBox == null || saturationTextBox == null || valueTextBox == null || hexTextBox == null)
            return;

        await hueTextBox.ClickAsync();
        await hueTextBox.FillAsync(h.ToString());

        await saturationTextBox.ClickAsync();
        await saturationTextBox.FillAsync(s.ToString());

        await valueTextBox.ClickAsync();
        await valueTextBox.FillAsync(v.ToString());

        await Expect(hexTextBox).ToHaveValueAsync(hex);
    }

    [TestCase(0, 100, 50, "#FFFF0000")]
    [TestCase(60, 33, 29, "#FF626232")]
    [TestCase(255, 100, 39, "#FF3200C7")]
    [TestCase(0, 61, 55, "#FFD24646")]
    [TestCase(180, 60, 25, "#FF196666")]
    public async Task HslSliders_Updates_Hex(int h, int s, int l, string hex)
    {
        ILocator hueSlider = Page.Locator("[data-part=hsl-hue-slider]");
        ILocator saturationSlider = Page.Locator("[data-part=hsl-saturation-slider]");
        ILocator lightnessSlider = Page.Locator("[data-part=hsl-lightness-slider]");
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");

        if (hueSlider == null || saturationSlider == null || lightnessSlider == null || hexTextBox == null)
            return;

        await hueSlider.ClickAsync();
        await hueSlider.FillAsync(h.ToString());

        await saturationSlider.ClickAsync();
        await saturationSlider.FillAsync(s.ToString());

        await lightnessSlider.ClickAsync();
        await lightnessSlider.FillAsync(l.ToString());

        await Expect(hexTextBox).ToHaveValueAsync(hex);
    }

    [TestCase(0, 100, 50, "#FFFF0000")]
    [TestCase(60, 33, 29, "#FF626232")]
    [TestCase(255, 100, 39, "#FF3200C7")]
    [TestCase(0, 61, 55, "#FFD24646")]
    [TestCase(180, 60, 25, "#FF1A6666")]
    [TestCase(0, -40, 100, "#FFFFFFFF")]
    [TestCase(255, 100, 39, "#FF3200C7")]
    [TestCase(346, 400, 37, "#FFBD002C")]
    [TestCase(0, 200, 28, "#FF8F0000")]
    [TestCase(78, 83, 100, "#FFFFFFFF")]
    public async Task HslTextBoxes_Updates_Hex(int h, int s, int l, string hex)
    {
        ILocator hueTextBox = Page.Locator("[data-part=hsl-hue-text-box]");
        ILocator saturationTextBox = Page.Locator("[data-part=hsl-saturation-text-box]");
        ILocator lightnessTextBox = Page.Locator("[data-part=hsl-lightness-text-box]");
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");

        if (hueTextBox == null || saturationTextBox == null || lightnessTextBox == null || hexTextBox == null)
            return;

        await hueTextBox.ClickAsync();
        await hueTextBox.FillAsync(h.ToString());

        await saturationTextBox.ClickAsync();
        await saturationTextBox.FillAsync(s.ToString());

        await lightnessTextBox.ClickAsync();
        await lightnessTextBox.FillAsync(l.ToString());

        await Expect(hexTextBox).ToHaveValueAsync(hex);
    }

    [TestCase(0, 100, 100, 0, "#FFFF0000")]
    [TestCase(0, 0, 50, 61, "#FF636332")]
    [TestCase(75, 100, 0, 22, "#FF3200C7")]
    [TestCase(0, 66, 66, 17, "#FFD44848")]
    [TestCase(75, 0, 0, 61, "#FF196363")]
    public async Task CmykSliders_Updates_Hex(int c, int m, int y, int k, string hex)
    {
        ILocator cyanSlider = Page.Locator("[data-part=cyan-slider]");
        ILocator magentaSlider = Page.Locator("[data-part=magenta-slider]");
        ILocator yellowSlider = Page.Locator("[data-part=yellow-slider]");
        ILocator keySlider = Page.Locator("[data-part=key-slider]");
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");

        if (cyanSlider == null || magentaSlider == null || yellowSlider == null || hexTextBox == null || hexTextBox == null)
            return;

        await cyanSlider.ClickAsync();
        await cyanSlider.FillAsync(c.ToString());

        await magentaSlider.ClickAsync();
        await magentaSlider.FillAsync(m.ToString());

        await yellowSlider.ClickAsync();
        await yellowSlider.FillAsync(y.ToString());

        await keySlider.ClickAsync();
        await keySlider.FillAsync(k.ToString());

        await Expect(hexTextBox).ToHaveValueAsync(hex);
    }

    [TestCase(0, 100, 100, 0, "#FFFF0000")]
    [TestCase(0, 0, 50, 61, "#FF636332")]
    [TestCase(75, 100, 0, 22, "#FF3200C7")]
    [TestCase(0, 66, 66, 17, "#FFD44848")]
    [TestCase(75, 0, 0, 61, "#FF196363")]
    [TestCase(0, -40, 0, 0, "#FFFFFFFF")]
    [TestCase(75, 100, 0, 22, "#FF3200C7")]
    [TestCase(0, 400, 77, 25, "#FFBF002C")]
    [TestCase(0, 83, 83, 0, "#FFFF2B2B")]
    [TestCase(25, 0, 83, 0, "#FFBFFF2B")]
    public async Task CmykTextBoxes_Updates_Hex(int c, int m, int y, int k, string hex)
    {
        ILocator cyanTextBox = Page.Locator("[data-part=cyan-text-box]");
        ILocator magentaTextBox = Page.Locator("[data-part=magenta-text-box]");
        ILocator yellowTextBox = Page.Locator("[data-part=yellow-text-box]");
        ILocator keyTextBox = Page.Locator("[data-part=key-text-box]");
        ILocator hexTextBox = Page.Locator("[data-part=hex-text-box]");

        if (cyanTextBox == null || magentaTextBox == null || yellowTextBox == null || keyTextBox == null || hexTextBox == null)
            return;

        await cyanTextBox.ClickAsync();
        await cyanTextBox.FillAsync(c.ToString());

        await magentaTextBox.ClickAsync();
        await magentaTextBox.FillAsync(m.ToString());

        await yellowTextBox.ClickAsync();
        await yellowTextBox.FillAsync(y.ToString());

        await keyTextBox.ClickAsync();
        await keyTextBox.FillAsync(k.ToString());

        await Expect(hexTextBox).ToHaveValueAsync(hex);
    }

    [TestCase(75)]
    [TestCase(100)]
    public async Task RedSlider_Updates_RedTextBox(int r)
    {
        ILocator redSlider = Page.Locator("[data-part=red-slider]");
        ILocator redTextBox = Page.Locator("[data-part=red-text-box]");

        if (redSlider == null || redTextBox == null)
            return;

        await redSlider.ClickAsync();
        await redSlider.FillAsync(r.ToString());

        await Expect(redTextBox).ToHaveValueAsync(r.ToString());
    }

    [TestCase(300, 255)]
    [TestCase(-80, 0)]
    [TestCase(100, 100)]
    public async Task RedTextBox_Updates_RedSlider(int r, int expectedR)
    {
        ILocator redTextBox = Page.Locator("[data-part=red-text-box]");
        ILocator redSlider = Page.Locator("[data-part=red-slider]");

        if (redTextBox == null || redSlider == null)
            return;

        await redTextBox.ClickAsync();
        await redTextBox.FillAsync(r.ToString());

        await Expect(redSlider).ToHaveValueAsync(expectedR.ToString());
    }

    [TestCase(75)]
    [TestCase(100)]
    public async Task BlueSlider_Updates_BlueTextBox(int b)
    {
        ILocator blueSlider = Page.Locator("[data-part=blue-slider]");
        ILocator blueTextBox = Page.Locator("[data-part=blue-text-box]");

        if (blueSlider == null || blueTextBox == null)
            return;

        await blueSlider.ClickAsync();
        await blueSlider.FillAsync(b.ToString());

        await Expect(blueTextBox).ToHaveValueAsync(b.ToString());
    }

    [TestCase(300, 255)]
    [TestCase(-80, 0)]
    [TestCase(100, 100)]
    public async Task BlueTextBox_Updates_BlueSlider(int b, int expectedB)
    {
        ILocator blueTextBox = Page.Locator("[data-part=blue-text-box]");
        ILocator blueSlider = Page.Locator("[data-part=blue-slider]");

        if (blueTextBox == null || blueSlider == null)
            return;

        await blueTextBox.ClickAsync();
        await blueTextBox.FillAsync(b.ToString());

        await Expect(blueSlider).ToHaveValueAsync(expectedB.ToString());
    }

    [TestCase(75)]
    [TestCase(100)]
    public async Task GreenSlider_Updates_GreenTextBox(int green)
    {
        ILocator greenSlider = Page.Locator("[data-part=green-slider]");
        ILocator greenTextBox = Page.Locator("[data-part=green-text-box]");

        if (greenSlider == null || greenTextBox == null)
            return;

        await greenSlider.ClickAsync();
        await greenSlider.FillAsync(green.ToString());

        await Expect(greenTextBox).ToHaveValueAsync(green.ToString());
    }

    [TestCase(300, 255)]
    [TestCase(-80, 0)]
    [TestCase(100, 100)]
    public async Task GreenTextBox_Updates_GreenSlider(int g, int expectedG)
    {
        ILocator greenTextBox = Page.Locator("[data-part=green-text-box]");
        ILocator greenSlider = Page.Locator("[data-part=green-slider]");

        if (greenTextBox == null || greenSlider == null)
            return;

        await greenTextBox.ClickAsync();
        await greenTextBox.FillAsync(g.ToString());

        await Expect(greenSlider).ToHaveValueAsync(expectedG.ToString());
    }

    [TestCase(60)]
    [TestCase(100)]
    public async Task AlphaSlider_Updates_AlphaTextBox(int alpha)
    {
        ILocator alphaSlider = Page.Locator("[data-part=alpha-slider]");
        ILocator alphaTextBox = Page.Locator("[data-part=alpha-text-box]");

        if (alphaSlider == null || alphaTextBox == null)
            return;

        await alphaSlider.ClickAsync();
        await alphaSlider.FillAsync(alpha.ToString());

        await Expect(alphaTextBox).ToHaveValueAsync($"{alpha.ToString()}%");
    }

    [TestCase(240, 100)]
    [TestCase(75, 75)]
    [TestCase(-80, 0)]
    public async Task AlphaTextBox_Updates_AlphaSlider(int a, int expectedA)
    {
        ILocator alphaTextBox = Page.Locator("[data-part=alpha-text-box]");
        ILocator alphaSlider = Page.Locator("[data-part=alpha-slider]");

        if (alphaTextBox == null || alphaSlider == null)
            return;

        await alphaTextBox.ClickAsync();
        await alphaTextBox.FillAsync(a.ToString());

        await Expect(alphaSlider).ToHaveValueAsync(expectedA.ToString());
    }

    [TestCase(60)]
    [TestCase(100)]
    public async Task HueSlider_Updates_HueTextBox(int hue)
    {
        ILocator hueSlider = Page.Locator("[data-part=hue-slider]");
        ILocator hueTextBox = Page.Locator("[data-part=hue-text-box]");

        if (hueSlider == null || hueTextBox == null)
            return;

        await hueSlider.ClickAsync();
        await hueSlider.FillAsync(hue.ToString());

        await Expect(hueTextBox).ToHaveValueAsync($"{hue.ToString()}°");
    }

    [TestCase(240, 240)]
    [TestCase(500, 360)]
    [TestCase(-80, 0)]
    public async Task HueTextBox_Updates_HueSlider(int hue, int expectedHue)
    {
        ILocator hueTextBox = Page.Locator("[data-part=hue-text-box]");
        ILocator hueSlider = Page.Locator("[data-part=hue-slider]");

        if (hueTextBox == null || hueSlider == null)
            return;

        await hueTextBox.ClickAsync();
        await hueTextBox.FillAsync(hue.ToString());

        await Expect(hueSlider).ToHaveValueAsync(expectedHue.ToString());
    }

    [TestCase(60)]
    [TestCase(100)]
    public async Task SaturationSlider_Updates_SaturationTextBox(int saturation)
    {
        ILocator saturationSlider = Page.Locator("[data-part=saturation-slider]");
        ILocator saturationTextBox = Page.Locator("[data-part=saturation-text-box]");

        if (saturationSlider == null || saturationTextBox == null)
            return;

        await saturationSlider.ClickAsync();
        await saturationSlider.FillAsync(saturation.ToString());

        await Expect(saturationTextBox).ToHaveValueAsync($"{saturation.ToString()}%");
    }

    [TestCase(240, 100)]
    [TestCase(60, 60)]
    [TestCase(-80, 0)]
    public async Task SaturationTextBox_Updates_SaturationSlider(int s, int expectedS)
    {
        ILocator saturationTextBox = Page.Locator("[data-part=saturation-text-box]");
        ILocator saturationSlider = Page.Locator("[data-part=saturation-slider]");

        if (saturationTextBox == null || saturationSlider == null)
            return;

        await saturationTextBox.ClickAsync();
        await saturationTextBox.FillAsync(s.ToString());

        await Expect(saturationSlider).ToHaveValueAsync(expectedS.ToString());
    }

    [TestCase(60)]
    [TestCase(100)]
    public async Task ValueSlider_Updates_ValueTextBox(int value)
    {
        ILocator valueSlider = Page.Locator("[data-part=value-slider]");
        ILocator valueTextBox = Page.Locator("[data-part=value-text-box]");

        if (valueSlider == null || valueTextBox == null)
            return;

        await valueSlider.ClickAsync();
        await valueSlider.FillAsync(value.ToString());

        await Expect(valueTextBox).ToHaveValueAsync($"{value.ToString()}%");
    }

    [TestCase(240, 100)]
    [TestCase(60, 60)]
    [TestCase(-80, 0)]
    public async Task ValueTextBox_Updates_ValueSlider(int v, int expectedV)
    {
        ILocator valueTextBox = Page.Locator("[data-part=value-text-box]");
        ILocator valueSlider = Page.Locator("[data-part=value-slider]");

        if (valueTextBox == null || valueSlider == null)
            return;

        await valueTextBox.ClickAsync();
        await valueTextBox.FillAsync(v.ToString());

        await Expect(valueSlider).ToHaveValueAsync(expectedV.ToString());
    }

    [TestCase(60)]
    [TestCase(100)]
    public async Task HslHueSlider_Updates_HslHueTextBox(int hue)
    {
        ILocator hueSlider = Page.Locator("[data-part=hsl-hue-slider]");
        ILocator hueTextBox = Page.Locator("[data-part=hsl-hue-text-box]");

        if (hueSlider == null || hueTextBox == null)
            return;

        await hueSlider.ClickAsync();
        await hueSlider.FillAsync(hue.ToString());

        await Expect(hueTextBox).ToHaveValueAsync($"{hue.ToString()}°");
    }
    
    [TestCase(500, 360)]
    [TestCase(60, 60)]
    [TestCase(-80, 0)]
    public async Task HslHueTextBox_Updates_HslHueSlider(int hue, int expectedHue)
    {
        ILocator hueTextBox = Page.Locator("[data-part=hsl-hue-text-box]");
        ILocator hueSlider = Page.Locator("[data-part=hsl-hue-slider]");

        if (hueTextBox == null || hueSlider == null)
            return;

        await hueTextBox.ClickAsync();
        await hueTextBox.FillAsync(hue.ToString());

        await Expect(hueSlider).ToHaveValueAsync(expectedHue.ToString());
    }

    [TestCase(60)]
    [TestCase(100)]
    public async Task HslSaturationSlider_Updates_HslSaturationTextBox(int saturation)
    {
        ILocator saturationSlider = Page.Locator("[data-part=hsl-saturation-slider]");
        ILocator saturationTextBox = Page.Locator("[data-part=hsl-saturation-text-box]");

        if (saturationSlider == null || saturationTextBox == null)
            return;

        await saturationSlider.ClickAsync();
        await saturationSlider.FillAsync(saturation.ToString());

        await Expect(saturationTextBox).ToHaveValueAsync($"{saturation.ToString()}%");
    }

    [TestCase(240, 100)]
    [TestCase(60, 60)]
    [TestCase(-80, 0)]
    public async Task HslSaturationTextBox_Updates_HslSaturationSlider(int s, int expectedS)
    {
        ILocator saturationTextBox = Page.Locator("[data-part=hsl-saturation-text-box]");
        ILocator saturationSlider = Page.Locator("[data-part=hsl-saturation-slider]");

        if (saturationTextBox == null || saturationSlider == null)
            return;

        await saturationTextBox.ClickAsync();
        await saturationTextBox.FillAsync(s.ToString());

        await Expect(saturationSlider).ToHaveValueAsync(expectedS.ToString());
    }

    [TestCase(60)]
    [TestCase(100)]
    public async Task HslLightnessSlider_Updates_HslLightnessTextBox(int lightness)
    {
        ILocator lightnessSlider = Page.Locator("[data-part=hsl-lightness-slider]");
        ILocator lightnessTextBox = Page.Locator("[data-part=hsl-lightness-text-box]");

        if (lightnessSlider == null || lightnessTextBox == null)
            return;

        await lightnessSlider.ClickAsync();
        await lightnessSlider.FillAsync(lightness.ToString());

        await Expect(lightnessTextBox).ToHaveValueAsync($"{lightness.ToString()}%");
    }

    [TestCase(240, 100)]
    [TestCase(75, 75)]
    [TestCase(-80, 0)]
    public async Task HslLightnessTextBox_Updates_HslLightnessSlider(int s, int expectedS)
    {
        ILocator lightnessTextBox = Page.Locator("[data-part=hsl-lightness-text-box]");
        ILocator lightnessSlider = Page.Locator("[data-part=hsl-lightness-slider]");

        if (lightnessTextBox == null || lightnessSlider == null)
            return;

        await lightnessTextBox.ClickAsync();
        await lightnessTextBox.FillAsync(s.ToString());

        await Expect(lightnessSlider).ToHaveValueAsync(expectedS.ToString());
    }

    [TestCase(60)]
    [TestCase(100)]
    public async Task CyanSlider_Updates_CyanTextBox(int cyan)
    {
        ILocator cyanSlider = Page.Locator("[data-part=cyan-slider]");
        ILocator cyanTextBox = Page.Locator("[data-part=cyan-text-box]");

        if (cyanSlider == null || cyanTextBox == null)
            return;

        await cyanSlider.ClickAsync();
        await cyanSlider.FillAsync(cyan.ToString());

        await Expect(cyanTextBox).ToHaveValueAsync($"{cyan.ToString()}%");
    }

    [TestCase(240, 100)]
    [TestCase(75, 75)]
    [TestCase(-80, 0)]
    public async Task CyanTextBox_Updates_CyanSlider(int c, int expectedC)
    {
        ILocator cyanTextBox = Page.Locator("[data-part=cyan-text-box]");
        ILocator cyanSlider = Page.Locator("[data-part=cyan-slider]");

        if (cyanTextBox == null || cyanSlider == null)
            return;

        await cyanTextBox.ClickAsync();
        await cyanTextBox.FillAsync(c.ToString());

        await Expect(cyanSlider).ToHaveValueAsync(expectedC.ToString());
    }

    [TestCase(60)]
    [TestCase(100)]
    public async Task MagentaSlider_Updates_MagentaTextBox(int magenta)
    {
        ILocator magentaSlider = Page.Locator("[data-part=magenta-slider]");
        ILocator magentaTextBox = Page.Locator("[data-part=magenta-text-box]");

        if (magentaSlider == null || magentaTextBox == null)
            return;

        await magentaSlider.ClickAsync();
        await magentaSlider.FillAsync(magenta.ToString());

        await Expect(magentaTextBox).ToHaveValueAsync($"{magenta.ToString()}%");
    }

    [TestCase(240, 100)]
    [TestCase(75, 75)]
    [TestCase(-80, 0)]
    public async Task MagentaTextBox_Updates_MagentaSlider(int m, int expectedM)
    {
        ILocator magentaTextBox = Page.Locator("[data-part=magenta-text-box]");
        ILocator magentaSlider = Page.Locator("[data-part=magenta-slider]");

        if (magentaTextBox == null || magentaSlider == null)
            return;

        await magentaTextBox.ClickAsync();
        await magentaTextBox.FillAsync(m.ToString());

        await Expect(magentaSlider).ToHaveValueAsync(expectedM.ToString());
    }

    [TestCase(60)]
    [TestCase(100)]
    public async Task YellowSlider_Updates_YellowTextBox(int yellow)
    {
        ILocator yellowSlider = Page.Locator("[data-part=yellow-slider]");
        ILocator yellowTextBox = Page.Locator("[data-part=yellow-text-box]");

        if (yellowSlider == null || yellowTextBox == null)
            return;

        await yellowSlider.ClickAsync();
        await yellowSlider.FillAsync(yellow.ToString());

        await Expect(yellowTextBox).ToHaveValueAsync($"{yellow.ToString()}%");
    }

    [TestCase(240, 100)]
    [TestCase(75, 75)]
    [TestCase(-80, 0)]
    public async Task YellowTextBox_Updates_YellowSlider(int y, int expectedY)
    {
        ILocator yellowTextBox = Page.Locator("[data-part=yellow-text-box]");
        ILocator yellowSlider = Page.Locator("[data-part=yellow-slider]");

        if (yellowTextBox == null || yellowSlider == null)
            return;

        await yellowTextBox.ClickAsync();
        await yellowTextBox.FillAsync(y.ToString());

        await Expect(yellowSlider).ToHaveValueAsync(expectedY.ToString());
    }
}