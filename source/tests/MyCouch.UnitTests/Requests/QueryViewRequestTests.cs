using System.Collections.Generic;
using FluentAssertions;
using MyCouch.Requests;
using Xunit;

namespace MyCouch.UnitTests.Requests
{
    public class QueryViewRequestTests : UnitTestsOf<QueryViewRequest>
    {
        public QueryViewRequestTests()
        {
            SUT = new QueryViewRequest("foodesigndoc", "barviewname");
        }

        [Fact]
        public void When_Keys_are_null_It_returns_false_for_HasKeys()
        {
            SUT.Keys = null;

            SUT.HasKeys.Should().BeFalse();
        }

        [Fact]
        public void When_Keys_are_empty_It_returns_false_for_HasKeys()
        {
            SUT.Keys = new object[0];

            SUT.HasKeys.Should().BeFalse();
        }

        [Fact]
        public void When_Keys_are_specified_It_returns_true_for_HasKeys()
        {
            SUT.Keys = new object[] { "fake_key" };

            SUT.HasKeys.Should().BeTrue();
        }

        [Fact]
        public void When_AdditionalQueryParameters_are_null_It_returns_false_for_HasAdditionalQueryParameters()
        {
            SUT.CustomQueryParameters = null;

            SUT.HasCustomQueryParameters.Should().BeFalse();
        }

        [Fact]
        public void When_AdditionalQueryParameters_are_empty_It_returns_false_for_HasAdditionalQueryParameters()
        {
            SUT.CustomQueryParameters = new Dictionary<string, object>();

            SUT.HasCustomQueryParameters.Should().BeFalse();
        }

        [Fact]
        public void When_AdditionalQueryParameters_are_specified_It_returns_true_for_HasAdditionalQueryParameters()
        {
            SUT.CustomQueryParameters = new Dictionary<string, object>
            {
                {"foo", new {bar = "bar"}}
            };

            SUT.HasCustomQueryParameters.Should().BeTrue();
        }
    }
}