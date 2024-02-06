using System;

namespace Assets.Scripts.Core
{
    public class BindableProperty<T> : IBindable<T>, IDisposable
    {
        private T _value;

        public BindableProperty(T value)
        {
            Value = value;
        }

        public event Action<T> OnValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value))
                {
                    return;
                }
                _value = value;
                Action<T> handler = OnValueChanged;
                handler?.Invoke(value);
            }
        }

        public void Dispose()
        {
            OnValueChanged = null;
        }
    }
}