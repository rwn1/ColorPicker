using ColorPicker.Core.Models;
using System.Windows.Media;

namespace ColorPicker.Wpf.App1.ViewModels
{
    internal class MainViewModel: ObservableObject
    {
        private Brush _selectedColor;
        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        public Brush SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor?.ToString() != value?.ToString()) 
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
            _selectedColor = Brushes.Teal;
        }
    }
}