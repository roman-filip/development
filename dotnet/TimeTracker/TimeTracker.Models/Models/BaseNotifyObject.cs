using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RFI.TimeTracker.Models
{
    public class BaseNotifyObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        protected virtual bool SetPropertyValue<TProperty>(ref TProperty storage, TProperty newValue, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, newValue))
            {
                return false;
            }

            storage = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
