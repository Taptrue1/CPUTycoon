using System;
using System.Collections.Generic;

namespace Utils.CustomNumbers
{
    public class CustomNumber<T> : ICustomNumber where T : struct
    {
        public event Action<T> Changed;
        event Action ICustomNumber.Changed
        {
            add => _internalChanged += value;
            remove => _internalChanged -= value;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;

                _value = value;
                _internalChanged?.Invoke();
                Changed?.Invoke(_value);
            }
        }
        
        private T _value;
        private Action _internalChanged;
        
        public CustomNumber(T value)
        {
            _value = value;
        }

        public string GetFormatedString(string format)
        {
            return string.Format(format);
        }
    }
}