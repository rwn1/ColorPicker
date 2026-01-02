using ColorPicker.Core.Models;
using System.Drawing;

namespace ColorPicker.Blazor.App3.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        private Color _selectedColor;
        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        public Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor.ToString() != value.ToString())
                {
                    _selectedColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _selectedColor = Color.Teal;
        }
    }
}