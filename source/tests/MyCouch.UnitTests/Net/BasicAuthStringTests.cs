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
            ((string)SUT).Should().Be(SUT.Value);
        }
    }
}