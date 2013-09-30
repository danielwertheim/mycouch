using FluentAssertions;
using MyCouch.Querying;
using Xunit;

namespace MyCouch.UnitTests.Querying
{
    public class ViewQueryOptionsTests : UnitTestsOf<ViewQueryOptions>
    {
        public ViewQueryOptionsTests()
        {
            SUT = new ViewQueryOptions();
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
    }
}