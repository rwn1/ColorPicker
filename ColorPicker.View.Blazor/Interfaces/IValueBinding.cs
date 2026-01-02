namespace ColorPicker.View.Blazor.Interfaces;
internal interface IValueBinding
{
    string Text { get; }

    void OnFocus();

    void OnInput(string? text);

    void OnBlur();
}