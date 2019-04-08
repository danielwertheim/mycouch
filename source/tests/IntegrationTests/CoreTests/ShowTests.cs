using System.Linq;
using FluentAssertions;
using MyCouch.Requests;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;
using Xunit;
using System.Collections.Generic;
using MyCouch.Net;
using System.Xml.Linq;
using IntegrationTests.TestFixtures;
using MyCouch;

namespace IntegrationTests.CoreTests
{
    public class ShowTests : IntegrationTestsOf<IDocuments>,
        IPreserveStatePerFixture,
        IClassFixture<ShowsFixture>
    {
        protected Artist[] ArtistsById { get; set; }

        public ShowTests(ShowsFixture data)
        {
            ArtistsById = data.Init(Environment);
            SUT = DbClient.Documents;
        }

        [MyFact(TestScenarios.ShowsContext)]
        public void Can_query_show_that_can_work_without_doc_id()
        {
            var query = new QueryShowRequest(ClientTestData.Shows.ArtistsHelloShowId);

            var response = SUT.ShowAsync(query).Result;

            response.Should().BeGetOfHtml();
        }

        [MyFact(TestScenarios.ShowsContext)]
        public void When_querying_show_that_transforms_doc_to_json_It_should_return_json()
        {
            var artist = ArtistsById.First();
            var query = new QueryShowRequest(ClientTestData.Shows.ArtistsJsonShowId).Configure(c => c.DocId(artist.ArtistId));

            var response = SUT.ShowAsync(query).Result;

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
            var query = new QueryShowRequest(ClientTestData.Shows.ArtistsJsonShowWithCustomQueryParamId)
                .Configure(c => c.DocId(artist.ArtistId)
                .CustomQueryParameters(customQueryParams)
                );

            var response = SUT.ShowAsync(query).Result;

            response.Should().BeGetOfJson();
            var transformedArtist = DbClient.Entities.Serializer.Deserialize<dynamic>(response.Content);
            ((string)transformedArtist.name).Should().Be(artist.Name);
            ((string)transformedArtist.foo).Should().Be("42");
        }

        [MyFact(TestScenarios.ShowsContext)]
        public void When_querying_show_that_transforms_doc_to_xml_It_should_return_xml()
        {
            var artist = ArtistsById.First();
            var query = new QueryShowRequest(ClientTestData.Shows.ArtistsXmlShowId)
                .Configure(c => c.DocId(artist.ArtistId)
                .Accepts(HttpContentTypes.Xml)
                );

            var response = SUT.ShowAsync(query).Result;

            response.Should().BeGetOfXml();
            var transformedArtist = XElement.Parse(response.Content);
            transformedArtist.Value.Should().Be(artist.Name);
        }
    }
}