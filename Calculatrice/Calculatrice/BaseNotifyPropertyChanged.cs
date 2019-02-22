using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Template_ListBox
{
    public abstract class BaseNotifyPropertyChanged : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private Dictionary<string, object> _values = new Dictionary<string, object>();
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected object GetField([CallerMemberName] string propertyName = null)
        {
            if (_values.ContainsKey(propertyName)) return _values[propertyName];
            return null;
        }
        protected bool SetField<T>(T value, [CallerMemberName] string propertyName = null)
        {
            T field = default(T);

            if (_values.ContainsKey(propertyName))
            {
                field = (T)_values[propertyName];
                _values[propertyName] = value;
            }
            else
            {
                _values.Add(propertyName, value);
            }

            return SetField(ref field, value, propertyName);
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}

