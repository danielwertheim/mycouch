using System;

namespace MyCouch.Rich.Schemes.Reflections
{
    public class DynamicStringGetter
    {
        private readonly Func<object, string> _accessor;

        public DynamicStringGetter(Func<object, string> accessor)
        {
            _accessor = accessor;
        }

        public string GetValue<T>(T item)
        {
            return _accessor(item);
        }
    }
}