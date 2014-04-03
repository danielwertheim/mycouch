using System.Net;
using System.Net.Http;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.IntegrationTests.CoreTests.DbClientTests
{
    public class DatabaseTests : ClientTestsOf<IDatabase>
    {
        public DatabaseTests() : base(IntegrationTestsRuntime.TempClientEnvironment)
        {
            SUT = Client.Database;
            SUT.PutAsync().Wait();
        }

        [Fact]
        public void When_Head_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.HeadAsync().Result;

            response.Should().Be(HttpMethod.Head);
        }

        [Fact]
        public void When_Get_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.GetAsync().Result;

            response.Should().BeAnyJson(HttpMethod.Get);
        }

        [Fact]
        public void When_Delete_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.DeleteAsync().Result;

            response.Should().BeOkJson(HttpMethod.Delete);
        }

        [Fact]
        public void When_Compact_of_existing_db_The_response_should_be_202()
        {
            var response = SUT.CompactAsync().Result;

            response.Should().BeOkJson(HttpMethod.Post, HttpStatusCode.Accepted);
        }

        [Fact]
        public void When_ViewCleanup_and_db_exists_The_response_be()
        {
            var response = SUT.ViewCleanup().Result;

            response.Should().BeOkJson(HttpMethod.Post, HttpStatusCode.Accepted);
        }
    }
}