using System;
#if !WinRT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

namespace MyCouch.Testing
{
    [TestClass]
    public abstract class TestsOf<T> : TestsOf where T : class
    {
        protected T SUT { get; set; }
    }

    [TestClass]
    public abstract class TestsOf
    {
        protected Action OnTestInitialize { get; set; }
        protected Action OnTestFinalize { get; set; }

        [TestInitialize]
        public void TestInitializer()
        {
            //Now.ValueFn = () => TestConstants.FixedDateTime;
            if(OnTestInitialize != null)
                OnTestInitialize();
        }

        [TestCleanup]
        public void TestFinalizer()
        {
            if (OnTestFinalize != null)
                OnTestFinalize();
            //Now.ValueFn = () => TestConstants.FixedDateTime;
        }
    }
}