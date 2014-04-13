namespace MyCouch.EntitySchemes.Reflections
{
    public class FakeStringGetter : IStringGetter
    {
        public string GetValue<T>(T item)
        {
            return null;
        }
    }
}