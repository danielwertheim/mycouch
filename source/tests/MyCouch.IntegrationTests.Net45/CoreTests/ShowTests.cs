using System.Linq;
using FluentAssertions;
using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Requests;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;
using Xunit;
using System.Collections.Generic;

namespace MyCouch.IntegrationTests.CoreTests
{
    public class ShowTests : IntegrationTestsOf<IShows>,
        IPreserveStatePerFixture,
        IUseFixture<ShowsFixture>
    {
        protected Artist[] ArtistsById { get; set; }

        public ShowTests()
        {
            SUT = DbClient.Shows;
        }

        public void SetFixture(ShowsFixture data)
        {
            ArtistsById = data.Init(Environment);
        }

        [MyFact(TestScenarios.ShowsContext)]
        public void Can_query_show_that_can_work_without_doc_id_without_doc_id()
        {
            var query = new ShowRequest(ClientTestData.Shows.ArtistsHelloShowId);

            var response = SUT.QueryRawAsync(query).Result;

            response.Should().BeGetOfHtml();
        }

        [MyFact(TestScenarios.ShowsContext)]
        public void When_querying_show_that_transforms_doc_to_json_It_should_return_json()
        {
            var artist = ArtistsById.First();
            var query = new ShowRequest(ClientTestData.Shows.ArtistsJsonShowId).Configure(c => c.Id(artist.ArtistId));

            var response = SUT.QueryRawAsync(query).Result;

            response.Should().BeGetOfJson();
            var transformedArtist = DbClient.Entities.Serializer.Deserialize<dynamic>(response.Content);
            ((string)transformedArtist.name).Should().Be(artist.Name);
        }

        [MyFact(TestScenarios.ShowsContext)]
        public void Can_send_custom_query_parameters_to_show()
        {
            var artist = ArtistsById.First();
            var customQueryParams = new Dictionary<string, object>();
            customQueryParams.Add("foo", 42);
            var query = new ShowRequest(ClientTestData.Shows.ArtistsJsonShowWithCustomQueryParamId)
                .Configure(c => c.Id(artist.ArtistId)
                .CustomQueryParameters(customQueryParams)
                );

            var response = SUT.QueryRawAsync(query).Result;

            response.Should().BeGetOfJson();
            var transformedArtist = DbClient.Entities.Serializer.Deserialize<dynamic>(response.Content);
            ((string)transformedArtist.name).Should().Be(artist.Name);
            ((string)transformedArtist.foo).Should().Be("42");
        }
    }
}