namespace MyCouch.EntitySchemes.Reflections
{
    public interface IStringSetter
    {
        void SetValue<T>(T item, string value);
    }
}