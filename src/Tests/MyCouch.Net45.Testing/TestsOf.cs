using System;

namespace MyCouch.Testing
{
    public abstract class TestsOf<T> : TestsOf where T : class
    {
        protected T SUT { get; set; }
    }

    public abstract class TestsOf { }
}