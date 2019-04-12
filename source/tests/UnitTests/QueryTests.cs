using System.Collections.Generic;
using FluentAssertions;
using MyCouch;
using Xunit;

namespace UnitTests
{
    public class QueryTests : UnitTestsOf<Query>
    {
        public QueryTests()
        {
            SUT = new Query("foodesigndoc", "barshowname");
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