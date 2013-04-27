using System;

namespace MyCouch.Reflections
{
    public class DynamicSetter
    {
        private readonly Action<object, object> _accessor;

        public DynamicSetter(Action<object, object> accessor)
        {
            _accessor = accessor;
        }

        public void SetValue<T>(T item, object value)
        {
            _accessor(item, value);
        }
    }
}