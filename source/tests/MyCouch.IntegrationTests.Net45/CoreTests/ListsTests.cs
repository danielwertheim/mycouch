using System.Linq;
using FluentAssertions;
using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Requests;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;
using Xunit;

namespace MyCouch.IntegrationTests.CoreTests
{
    public class ListsTests : IntegrationTestsOf<IViews>,
        IPreserveStatePerFixture,
        IUseFixture<ViewsFixture>
    {
        protected Artist[] ArtistsById { get; set; }

        public ListsTests()
        {
            SUT = DbClient.Views;
        }

        public void SetFixture(ViewsFixture data)
        {
            ArtistsById = data.Init(Environment);
        }

        [MyFact(TestScenarios.ListsContext)]
        public void When_querying_raw_using_list_that_transforms_to_json_It_should_return_json()
        {
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId)
                .Configure(c => c.WithList(ClientTestData.Views.ListNames.TransformToDocListId));

            var response = SUT.QueryRawAsync(query).Result;

            response.Should().BeGetOfJson();
            var transformedArtists = DbClient.Entities.Serializer.Deserialize<dynamic[]>(response.Content);
            transformedArtists.Length.Should().Be(10);
        }

        [MyFact(TestScenarios.ListsContext)]
        public void When_querying_raw_using_list_that_transforms_to_html_It_should_return_html()
        {
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId)
                .Configure(c => c.WithList(ClientTestData.Views.ListNames.TransformToHtmlListId));

            var response = SUT.QueryRawAsync(query).Result;

            response.Should().BeGetOfHtml();
        }

        [MyFact(TestScenarios.ListsContext)]
        public void When_querying_raw_using_key_and_a_list_that_transforms_to_json_It_should_return_json()
        {
            const string keyToReturn = "Fake artist 1";
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId).Configure(c => c
                .WithList(ClientTestData.Views.ListNames.TransformToDocListId)
                .Key(keyToReturn));

            var response = SUT.QueryRawAsync(query).Result;

            response.Should().BeGetOfJson();
            var transformedArtists = DbClient.Entities.Serializer.Deserialize<dynamic[]>(response.Content);
            transformedArtists.Length.Should().Be(1);
            ((string)transformedArtists.Single().name).Should().Be(keyToReturn);
        }

        [MyFact(TestScenarios.ListsContext)]
        public void When_querying_raw_using_key_and_a_list_that_transforms_to_html_It_should_return_html()
        {
            const string keyToReturn = "Fake artist 1";
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId).Configure(c => c
                .WithList(ClientTestData.Views.ListNames.TransformToHtmlListId)
                .Key(keyToReturn));

            var response = SUT.QueryRawAsync(query).Result;

            response.Should().BeGetOfHtml();
        }

        [MyFact(TestScenarios.ListsContext)]
        public void When_querying_raw_using_keys_and_a_list_that_transforms_to_json_It_should_return_json()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId)
                .Configure(c => c.WithList(ClientTestData.Views.ListNames.TransformToDocListId).Keys(keys));

            var response = SUT.QueryRawAsync(query).Result;

            response.Should().BePostOfJson();
            var transformedArtists = DbClient.Entities.Serializer.Deserialize<dynamic[]>(response.Content);
            transformedArtists.Length.Should().Be(3);
        }

        [MyFact(TestScenarios.ListsContext)]
        public void When_querying_raw_using_keys_and_a_list_that_transforms_to_html_It_should_return_html()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId)
                .Configure(c => c.WithList(ClientTestData.Views.ListNames.TransformToHtmlListId).Keys(keys));

            var response = SUT.QueryRawAsync(query).Result;

            response.Should().BePostOfHtml();
        }
    }
}