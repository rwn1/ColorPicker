using ColorPicker.View.Blazor.Interfaces;
using System.ComponentModel;

internal sealed class PercentBinding<T> : IValueBinding, IDisposable
    where T : INotifyPropertyChanged
{
    private readonly FloatPropertyBinding<T> _binding;

    private bool _hasFocus;
    private string _text = string.Empty;
    private int _cachedPercent;

    public event Action? Changed;

    /// <summary>
    /// Initializes a new instance of the PercentBinding class.
    /// </summary>
    /// <param name="binding">Binding model.</param>
    public PercentBinding(FloatPropertyBinding<T> binding)
    {
        _binding = binding;
        _binding.Changed += OnSourceChanged;

        SyncFromModel();
    }

    /// <summary>
    /// Gets or sets the numeric percent value [0–100].
    /// </summary>
    public int Value
    {
        get => _cachedPercent;
        set
        {
            value = Math.Clamp(value, 0, 100);
            if (value == _cachedPercent)
                return;

            _cachedPercent = value;
            _binding.Value = value / 100f;
        }
    }

    /// <summary>
    /// Gets the text shown in the input field
    /// </summary>
    public string Text => _text;

    /// <summary>
    /// Called when input receives focus.
    /// </summary>
    public void OnFocus()
    {
        _hasFocus = true;
        _text = _cachedPercent.ToString();
    }

    /// <summary>
    /// Called on every text change.
    /// </summary>
    public void OnInput(string? text)
    {
        _text = text ?? "";

        if (int.TryParse(_text, out var value))
        {
            value = Math.Clamp(value, 0, 100);
            Value = value;
        }
    }

    /// <summary>
    /// Called when input loses focus.
    /// </summary>
    public void OnBlur()
    {
        if (!_hasFocus)
            return;

        _hasFocus = false;

        //if (int.TryParse(_text, out var value))
        //{
        //    value = Math.Clamp(value, 0, 100);
        //    Value = value;
        //}

        SyncText();
    }

    /// <summary>
    /// Reacts to changes from the model.
    /// </summary>
    private void OnSourceChanged()
    {
        if (_hasFocus)
            return;

        SyncFromModel();
        Changed?.Invoke();
    }

    /// <summary>
    /// Sync cached percent from float model value.
    /// </summary>
    private void SyncFromModel()
    {
        _cachedPercent = (int)Math.Round(_binding.Value * 100f);
        SyncText();
    }

    /// <summary>
    /// Formats text representation.
    /// </summary>
    private void SyncText()
    {
        _text = $"{_cachedPercent}%";
    }

    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        _binding.Changed -= OnSourceChanged;
        _binding.Dispose();
    }
}