using MyCouch.Testing;
using NUnit.Framework;

namespace MyCouch.UnitTests
{
    [TestFixture]
    public abstract class UnitTestsOf<T> : TestsOf<T> where T : class { }

    [TestFixture]
    public abstract class UnitTestsOf : TestsOf { }
}