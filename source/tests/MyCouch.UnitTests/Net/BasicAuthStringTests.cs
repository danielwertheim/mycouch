using FluentAssertions;
using MyCouch.Net;
using Xunit;

namespace MyCouch.UnitTests.Net
{
    public class BasicAuthStringTests : UnitTestsOf<BasicAuthString>
    {
        [Fact]
        public void When_passing_username_and_password_It_initializes_a_base64_encoded_string()
        {
            SUT = new BasicAuthString("testUser", "testPassword");

            SUT.Value.Should().Be("dGVzdFVzZXI6dGVzdFBhc3N3b3Jk");
            SUT.ToString().Should().Be(SUT.Value);
            SUT.Should().Be(SUT.Value);
        }

        [Fact]
        public void When_comparing_against_equal_string_It_should_return_true()
        {
            SUT = new BasicAuthString("testUser", "testPassword");

            SUT.Equals("dGVzdFVzZXI6dGVzdFBhc3N3b3Jk").Should().BeTrue();
        }

        [Fact]
        public void When_comparing_against_non_equal_string_It_should_return_false()
        {
            SUT = new BasicAuthString("testUser", "testPassword");

            SUT.Equals("tester_joe").Should().BeFalse();
        }

        [Fact]
        public void When_comparing_against_equal_object_It_should_return_true()
        {
            SUT = new BasicAuthString("testUser", "testPassword");

            SUT.Equals(new BasicAuthString("testUser", "testPassword")).Should().BeTrue();
        }

        [Fact]
        public void When_comparing_against_non_equal_object_It_should_return_false()
        {
            SUT = new BasicAuthString("testUser", "testPassword");

            SUT.Equals(new BasicAuthString("testerjoe", "faker")).Should().BeFalse();
        }
    }
}