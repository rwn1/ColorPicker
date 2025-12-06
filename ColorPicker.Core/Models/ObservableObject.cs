using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ColorPicker.Core.Models
{
    /// <summary>
    /// Basic object for notify property change.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that informs about a property change.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Create the OnPropertyChanged method to raise the event.
        /// </summary>
        /// <param name="name">The calling member's name will be used as the parameter.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var ev = PropertyChanged;
            if (ev != null) ev(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}