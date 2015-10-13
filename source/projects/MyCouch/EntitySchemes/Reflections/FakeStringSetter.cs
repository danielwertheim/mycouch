namespace MyCouch.EntitySchemes.Reflections
{
    public class FakeStringSetter : IStringSetter
    {
        public void SetValue<T>(T item, string value) { }
    }
}