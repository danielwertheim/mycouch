using MyCouch.Testing;

namespace MyCouch.UnitTests
{
    public abstract class UnitTestsOf<T> : TestsOf<T> where T : class { }

    public abstract class UnitTestsOf : TestsOf { }
}