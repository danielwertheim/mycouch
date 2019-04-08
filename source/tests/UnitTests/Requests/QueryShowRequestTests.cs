using System.Collections.Generic;
using FluentAssertions;
using MyCouch.Requests;
using Xunit;

namespace UnitTests.Requests
{
    public class QueryShowRequestTests : UnitTestsOf<QueryShowRequest>
    {
        public QueryShowRequestTests()
        {
            SUT = new QueryShowRequest("foodesigndoc", "barshowname");
        }

        [Fact]
        public void When_Accepts_are_null_It_returns_false_for_HasAccepts()
        {
            SUT.Accepts = null;

            SUT.HasAccepts.Should().BeFalse();
        }

        [Fact]
        public void When_Accepts_are_empty_It_returns_false_for_HasAccepts()
        {
            SUT.Accepts = new string[]{};

            SUT.HasAccepts.Should().BeFalse();
        }

        [Fact]
        public void When_Accepts_are_specified_It_returns_true_for_HasAccepts()
        {
            SUT.Accepts = new[] { "foo" };

            SUT.HasAccepts.Should().BeTrue();
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