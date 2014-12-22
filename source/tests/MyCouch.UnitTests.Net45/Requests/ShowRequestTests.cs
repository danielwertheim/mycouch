using System.Collections.Generic;
using FluentAssertions;
using MyCouch.Requests;
using Xunit;

namespace MyCouch.UnitTests.Requests
{
    public class ShowRequestTests : UnitTestsOf<ShowRequest>
    {
        public ShowRequestTests()
        {
            SUT = new ShowRequest("foodesigndoc", "barviewname");
        }
        
        [Fact]
        public void When_CustomQueryParameters_are_null_It_returns_false_for_HasCustomQueryParameters()
        {
            SUT.CustomQueryParameters = null;

            SUT.HasCustomQueryParameters.Should().BeFalse();
        }

        [Fact]
        public void When_CustomQueryParameters_are_empty_It_returns_false_for_HasCustomQueryParameters()
        {
            SUT.CustomQueryParameters = new Dictionary<string, object>();

            SUT.HasCustomQueryParameters.Should().BeFalse();
        }

        [Fact]
        public void When_CustomQueryParameters_are_specified_It_returns_true_for_HasCustomQueryParameters()
        {
            SUT.CustomQueryParameters = new Dictionary<string, object>
            {
                {"foo", new {bar = "bar"}}
            };

            SUT.HasCustomQueryParameters.Should().BeTrue();
        }
    }
}