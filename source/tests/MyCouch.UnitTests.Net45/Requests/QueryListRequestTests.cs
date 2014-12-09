using FluentAssertions;
using MyCouch.Requests;
using System.Collections.Generic;
using Xunit;

namespace MyCouch.UnitTests.Requests
{
    public class QueryListRequestTests : UnitTestsOf<QueryListRequest>
    {
        public QueryListRequestTests()
        {
            SUT = new QueryListRequest("foodesigndoc", "barlistname", "barviewname");
        }

        [Fact]
        public void When_AdditionalQueryParameters_are_null_It_returns_false_for_HasAdditionalQueryParameters()
        {
            SUT.AdditionalQueryParameters = null;

            SUT.HasAdditionalQueryParameters.Should().BeFalse();
        }

        [Fact]
        public void When_AdditionalQueryParameters_are_empty_It_returns_false_for_HasAdditionalQueryParameters()
        {
            SUT.AdditionalQueryParameters = new Dictionary<string, object>();

            SUT.HasAdditionalQueryParameters.Should().BeFalse();
        }

        [Fact]
        public void When_AdditionalQueryParameters_are_specified_It_returns_true_for_HasAdditionalQueryParameters()
        {
            var value = new Dictionary<string, object>();
            value.Add("foo", new { bar = "bar" });
            SUT.AdditionalQueryParameters = value;

            SUT.HasAdditionalQueryParameters.Should().BeTrue();
        }
    }
}