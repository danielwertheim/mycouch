namespace MyCouch.Testing
{
    public abstract class TestsOf<T> : Tests where T : class
    {
        protected T SUT { get; set; }
    }

    public abstract class Tests { }
}