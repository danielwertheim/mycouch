using System.Net;
using System.Net.Http;
using MyCouch.Testing;
using Xunit;

namespace MyCouch.IntegrationTests.CoreTests.ServerClientTests
{
    public class DatabasesTests : ServerClientTestsOf<IDatabases>
    {
        private readonly string _tempDbName;

        public DatabasesTests() : base(IntegrationTestsRuntime.TempEnvironment)
        {
            _tempDbName = Client.Connection.DbName;

            SUT = ServerClient.Databases;
            SUT.PutAsync(_tempDbName).Wait();
        }

        [Fact]
        public void When_Head_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.HeadAsync(_tempDbName).Result;

            response.Should().Be(HttpMethod.Head);
        }

        [Fact]
        public void When_Get_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.GetAsync(_tempDbName).Result;

            response.Should().BeAnyJson(HttpMethod.Get);
        }

        [Fact]
        public void When_Delete_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.DeleteAsync(_tempDbName).Result;

            response.Should().BeOkJson(HttpMethod.Delete);
        }

        [Fact]
        public void When_Compact_of_existing_db_The_response_should_be_202()
        {
            var response = SUT.CompactAsync(_tempDbName).Result;

            response.Should().BeOkJson(HttpMethod.Post, HttpStatusCode.Accepted);
        }

        [Fact]
        public void When_ViewCleanup_and_db_exists_The_response_be()
        {
            var response = SUT.ViewCleanupAsync(_tempDbName).Result;

            response.Should().BeOkJson(HttpMethod.Post, HttpStatusCode.Accepted);
        }
    }
}