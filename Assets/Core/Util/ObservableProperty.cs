using System;

namespace Core.Util
{
    public class ObservableProperty<T>
    {
        private event Action<T> OnChanged;
        
        public T Value
        {
            get => _property;
            set
            {
                _property = value;
                OnChanged?.Invoke(_property);
            }
        }

        public ObservableProperty()
        {
        }
        
        public ObservableProperty(T value)
        {
            Value = value;
        }
        
        public void Subscribe(Action<T> action, bool forceSync = false)
        {
            OnChanged += action;
            
            // If data is set before View synchronization, this logic can be quite useful...
            if (forceSync)
            {
                action?.Invoke(Value);
            }
        }

        public void Unsubscribe(Action<T> action)
        {
            OnChanged -= action;
        }

        private T _property;
    }
}