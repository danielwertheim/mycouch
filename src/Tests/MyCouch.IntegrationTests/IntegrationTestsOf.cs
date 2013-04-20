using MyCouch.Testing;
using NUnit.Framework;

namespace MyCouch.IntegrationTests
{
    [TestFixture]
    public abstract class IntegrationTestsOf<T> : TestsOf<T> where T : class { }

    [TestFixture]
    public abstract class IntegrationTestsOf : TestsOf { }
}