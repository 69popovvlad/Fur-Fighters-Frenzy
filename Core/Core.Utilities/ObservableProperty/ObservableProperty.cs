using System;

namespace Core.Utilities.ObservableProperty
{
    public sealed class ObservableProperty<T> : IReadOnlyObservableProperty<T>
    {
        public event PropertyChanged PropertyChanged;
        
        private T _value;

        public string Name
        {
            get;
            set;
        }

        public Type Type => typeof(T);

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value))
                {
                    return;
                }

                var lastValue = _value;
                _value = value;
                
                PropertyChanged?.Invoke(Name, _value, lastValue);
            }
        }
        
        public ObservableProperty(T value)
        {
            _value = value;
        }
    }
}
