using MyCouch.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyCouch.UnitTests
{
    [TestClass]
    public abstract class UnitTestsOf<T> : TestsOf<T> where T : class { }

    [TestClass]
    public abstract class UnitTestsOf : TestsOf { }
}