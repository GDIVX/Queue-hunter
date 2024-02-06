using System;

namespace Assets.Scripts.Core
{
    public class BindableProperty<T> : IBindable<T>
    {
        private T _value;

        public event Action<T> OnValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }
    }
}