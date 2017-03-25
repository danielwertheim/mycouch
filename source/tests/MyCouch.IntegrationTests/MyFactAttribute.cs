using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace MyCouch.IntegrationTests
{
    public class Discoverer : IXunitTestCaseDiscoverer
    {
        private IMessageSink messageSink;

        public Discoverer(IMessageSink messageSink)
        {
            this.messageSink = messageSink;
        }
        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod,
            IAttributeInfo factAttribute)
        {
            return new[] { new XunitTestCase(messageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod) };
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    [XunitTestCaseDiscoverer("MyCouch.IntegrationTests.Discoverer", "MyCouch.IntegrationTests")]
    public class MyFactAttribute : FactAttribute
    {
        public MyFactAttribute(params string[] scenarios)
        {
            if (!scenarios.Any() || IntegrationTestsRuntime.Environment.SupportsEverything)
                return;

            if (!scenarios.All(r => IntegrationTestsRuntime.Environment.HasSupportFor(r)))
                Skip = string.Format("TestEnvironment does not support ALL test scenario(s): '{0}'.", string.Join("|", scenarios));
        }
    }
}