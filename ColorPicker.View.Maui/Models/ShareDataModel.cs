namespace ColorPicker.View.Maui.Models;
/// <summary>
/// Model for sharing data between the background (ColorSelectionBackground) and the mark (ColorSelectionMark).
/// </summary>
internal class ShareDataModel
{
    /// <summary>
    /// Cached pixels from background.
    /// </summary>
    public byte[] Pixels { get; set; }
}