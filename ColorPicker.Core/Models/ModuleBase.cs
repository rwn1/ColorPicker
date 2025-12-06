using System;
using System.Runtime.CompilerServices;

namespace ColorPicker.Core.Models
{
    /// <summary>
    /// Object for notifying changes from the UI.
    /// </summary>
    public abstract class ModuleBase : ObservableObject
    {
        /// <summary>
        /// Raised when a property was changed by external actor (UI).
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Called by property setters to notify both PropertyChanged and Changed (if not suppressed).
        /// Module maintains suppression flag internally via SetSuppressChanged(true) around hub updates.
        /// </summary>
        protected void NotifyAndRaiseChanged([CallerMemberName] string propertyName = "")
        {
            NotifyPropertyChanged(propertyName);
            if (!_suppressExternalChanged)
            {
                var ev = Changed;
                if (ev != null) ev(this, EventArgs.Empty);
            }
        }

        private bool _suppressExternalChanged = false;

        /// <summary>
        /// Hub uses this to set internal values without causing Changed event.
        /// PropertyChanged will still be raised to update UI.
        /// </summary>
        internal void SetSuppressChanged(bool suppress)
        {
            _suppressExternalChanged = suppress;
        }
    }
}