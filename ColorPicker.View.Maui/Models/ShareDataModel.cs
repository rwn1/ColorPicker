namespace ColorPicker.View.Maui.Models;
/// <summary>
/// Model for sharing data between the background (ColorSelectionNackground) and the mark (ColorSelectionMark).
/// </summary>
internal class ShareDataModel
{
    public byte[] Pixels { get; set; }
}