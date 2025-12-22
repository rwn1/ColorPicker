using ColorPicker.Core.Models;
using System.Windows.Media;

namespace ColorPicker.Wpf.App1.ViewModels
{
    internal class MainViewModel: ObservableObject
    {
        private Brush _selectedBrush;
        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        public Brush SelectedBrush
        {
            get => _selectedBrush;
            set
            {
                if (_selectedBrush?.ToString() != value?.ToString()) 
                {
                    _selectedBrush = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _selectedBrush = Brushes.Teal;
        }
    }
}