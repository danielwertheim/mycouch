using System;

namespace MyCouch.Schemes.Reflections
{
    public class DynamicStringSetter
    {
        private readonly Action<object, string> _accessor;

        public DynamicStringSetter(Action<object, string> accessor)
        {
            _accessor = accessor;
        }

        public void SetValue<T>(T item, string value)
        {
            _accessor(item, value);
        }
    }
}