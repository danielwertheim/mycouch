using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegrationTests
{
    //public class Discoverer : IXunitTestCaseDiscoverer
    //{
    //    private readonly IMessageSink _messageSink;

    //    public Discoverer(IMessageSink messageSink)
    //    {
    //        _messageSink = messageSink;
    //    }

    //    //public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
    //    //    => new[] { new XunitTestCase(_messageSink, discoveryOptions.MethodDisplayOrDefault(), TestMethodDisplayOptions.All, testMethod) };

    //    protected virtual IXunitTestCase CreateTestCase(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
    //        => new XunitTestCase(_messageSink, discoveryOptions.MethodDisplayOrDefault(), TestMethodDisplayOptions.All, testMethod);

    //    public virtual IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
    //    {
    //        IXunitTestCase testCase;

    //        if (testMethod.Method.GetParameters().Any())
    //            testCase = new ExecutionErrorTestCase(_messageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod, "[Fact] methods are not allowed to have parameters. Did you mean to use [Theory]?");
    //        else if (testMethod.Method.IsGenericMethodDefinition)
    //            testCase = new ExecutionErrorTestCase(_messageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod, "[Fact] methods are not allowed to be generic.");
    //        else
    //            testCase = CreateTestCase(discoveryOptions, testMethod, factAttribute);

    //        return new[] { testCase };
    //    }
    //}

    [AttributeUsage(AttributeTargets.Method)]
    //[XunitTestCaseDiscoverer("IntegrationTests.Discoverer", "IntegrationTests")]
    public class MyFactAttribute : FactAttribute
    {
        public MyFactAttribute(params string[] scenarios)
        {
            if (!scenarios.Any() || IntegrationTestsRuntime.Environment.SupportsEverything)
                return;

            if (!scenarios.All(r => IntegrationTestsRuntime.Environment.HasSupportFor(r)))
                Skip = $"TestEnvironment does not support ALL test scenario(s): '{string.Join("|", scenarios)}'.";
        }
    }
}