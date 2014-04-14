namespace MyCouch.EntitySchemes.Reflections
{
    public interface IStringGetter
    {
        string GetValue<T>(T item);
    }
}