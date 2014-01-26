using FluentAssertions;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.IntegrationTests.ClientTests
{
    public class DatabaseTests : ClientTestsOf<IDatabase>
    {
        public DatabaseTests() : base(IntegrationTestsRuntime.TempClientEnvironment)
        {
            SUT = Client.Database;
            SUT.PutAsync().Wait();
        }

        [Fact]
        public void When_Delete_of_existing_db_The_response_should_be_202()
        {
            var response = SUT.DeleteAsync().Result;

            response.Should().Be200Delete();
        }

        [Fact]
        public void When_Compact_of_existing_db_The_response_should_be_202()
        {
            var response = SUT.CompactAsync().Result;

            response.Should().Be202Post();
        }
    }
}