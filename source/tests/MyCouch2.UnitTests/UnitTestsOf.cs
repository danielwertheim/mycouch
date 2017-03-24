using MyCouch.Testing;
using Xunit;

namespace MyCouch.UnitTests
{
    [Trait("Category", "UnitTests")]
    public abstract class UnitTestsOf<T> : TestsOf<T> where T : class { }

    [Trait("Category", "UnitTests")]
    public abstract class UnitTests : Tests { }
}