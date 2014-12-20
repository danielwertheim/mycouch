using System.Linq;
using FluentAssertions;
using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Requests;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;
using Xunit;
using MyCouch.Net;

namespace MyCouch.IntegrationTests.CoreTests
{
    public class ListsTests : IntegrationTestsOf<ILists>,
        IPreserveStatePerFixture,
        IUseFixture<ViewsFixture>
    {
        protected Artist[] ArtistsById { get; set; }

        public ListsTests()
        {
            SUT = DbClient.Lists;
        }

        public void SetFixture(ViewsFixture data)
        {
            ArtistsById = data.Init(Environment);
        }

        [MyFact(TestScenarios.ListsContext)]
        public void Should_transform_all_view_rows_when_query_parameters_not_supplied()
        {
            var query = new QueryListRequest(ClientTestData.Views.TransformToDocListId,
                ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId.Name);

            var response = SUT.QueryAsync(query).Result;

            response.Should().BeSuccessfulGet();
            response.ContentType.Should().Contain(HttpContentTypes.Json);
            var transformedArtists = DbClient.Entities.Serializer.Deserialize<dynamic[]>(response.Content);
            transformedArtists.Length.Should().Be(10);
        }

        [MyFact(TestScenarios.ListsContext)]
        public void When_Key_is_specified_using_json_Then_the_matching_row_is_transformed()
        {
            const string keyToReturn = "Fake artist 1";
            var query = new QueryListRequest(ClientTestData.Views.TransformToDocListId,
                ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId.Name).Configure(q => q.Key(keyToReturn));

            var response = SUT.QueryAsync(query).Result;

            response.Should().BeSuccessfulGet();
            response.ContentType.Should().Contain(HttpContentTypes.Json);
            var transformedArtists = DbClient.Entities.Serializer.Deserialize<dynamic[]>(response.Content);
            transformedArtists.Length.Should().Be(1);
            ((string)transformedArtists.Single().name).Should().Be(keyToReturn);
        }

        [MyFact(TestScenarios.ListsContext)]
        public void When_Keys_are_specified_using_json_Then_matching_rows_are_transformed()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new QueryListRequest(ClientTestData.Views.TransformToDocListId,
                ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId.Name).Configure(q => q.Keys(keys));

            var response = SUT.QueryAsync(query).Result;

            response.Should().BeSuccessfulPost();
            response.ContentType.Should().Contain(HttpContentTypes.Json);
            var transformedArtists = DbClient.Entities.Serializer.Deserialize<dynamic[]>(response.Content);
            transformedArtists.Length.Should().Be(3);
        }
    }
}