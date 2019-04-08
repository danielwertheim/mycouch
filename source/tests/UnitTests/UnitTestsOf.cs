using MyCouch.Testing;

namespace UnitTests
{
    public abstract class UnitTestsOf<T> : TestsOf<T> where T : class { }

    public abstract class UnitTests : Tests { }
}