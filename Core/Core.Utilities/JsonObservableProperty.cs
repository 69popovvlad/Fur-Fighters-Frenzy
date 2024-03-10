using System;
using Core.Utilities.ObservableProperty;
using Newtonsoft.Json;

namespace Core.Utilities
{
    [Serializable]
    public class JsonObservableProperty<T>: IReadOnlyObservableProperty<T>
    {
        public event PropertyChanged PropertyChanged;

        [JsonProperty("value")]
        private T _value;

        public string Name { get; set; }

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

        public JsonObservableProperty(T value)
        {
            _value = value;
        }
    }
}