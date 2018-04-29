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
    public class ViewsTests : IntegrationTestsOf<IViews>,
        IPreserveStatePerFixture,
        IClassFixture<ViewsFixture>
    {
        protected Artist[] ArtistsById { get; set; }

        public ViewsTests(ViewsFixture data)
        {
            ArtistsById = data.Init(Environment);
            SUT = DbClient.Views;
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_no_key_with_sum_reduce_for_string_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Configure(cfg => cfg.Reduce(true));

            var response = SUT.QueryAsync(query).Result;

            response.Should().BeSuccessfulGet(numOfRows: 1);
            response.Rows[0].Value.Should().Be(expectedSum.ToString(MyCouchRuntime.FormatingCulture.NumberFormat));
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_no_key_with_sum_reduce_for_dynamic_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Configure(cfg => cfg.Reduce(true));

            var response = SUT.QueryAsync<dynamic>(query).Result;

            response.Should().BeSuccessfulGet(numOfRows: 1);
            ((long)response.Rows[0].Value).Should().Be(expectedSum);
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_no_key_with_sum_reduce_for_typed_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Configure(cfg => cfg.Reduce(true));

            var response = SUT.QueryAsync<int>(query).Result;

            response.Should().BeSuccessfulGet(numOfRows: 1);
            response.Rows[0].Value.Should().Be(expectedSum);
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_IncludeDocs_and_no_value_is_returned_for_string_response_Then_the_included_docs_are_extracted()
        {
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(cfg => cfg.IncludeDocs(true));

            var response = SUT.QueryAsync(query).Result;

            response.Should().BeSuccessfulGet(ArtistsById.Length);
            for (var i = 0; i < response.RowCount; i++)
            {
                response.Rows[i].Value.Should().BeNull();
                DbClient.Entities.Serializer.Deserialize<Artist>(response.Rows[i].IncludedDoc).Should().BeEquivalentTo(ArtistsById[i]);
            }
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_IncludeDocs_and_no_value_is_returned_for_entity_response_Then_the_included_docs_are_extracted()
        {
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(cfg => cfg.IncludeDocs(true));

            var response = SUT.QueryAsync<string, Artist>(query).Result;

            response.Should().BeSuccessfulGet(ArtistsById.Length);
            for (var i = 0; i < response.RowCount; i++)
            {
                response.Rows[i].Value.Should().BeNull();
                response.Rows[i].IncludedDoc.Should().BeEquivalentTo(ArtistsById[i]);
            }
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_IncludeDocs_of_non_array_doc_and_null_value_is_returned_Then_the_neither_included_docs_nor_value_is_extracted()
        {
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(cfg => cfg.IncludeDocs(true));

            var response = SUT.QueryAsync<string[], string[]>(query).Result;

            response.Should().BeSuccessfulGet(ArtistsById.Length);
            for (var i = 0; i < response.RowCount; i++)
            {
                response.Rows[i].Value.Should().BeNull();
                response.Rows[i].IncludedDoc.Should().BeNull();
            }
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_Skipping_2_of_10_using_json_Then_8_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2);
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

            var response = SUT.QueryAsync(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => DbClient.Serializer.Serialize(a.Albums)).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_Skipping_2_of_10_using_json_array_Then_8_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2);
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

            var response = SUT.QueryAsync<string[]>(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums.Select(i => DbClient.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_Skipping_2_of_10_using_entities_Then_8_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2);
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

            var response = SUT.QueryAsync<Album[]>(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_Limit_to_2_using_json_Then_2_rows_are_returned()
        {
            var artists = ArtistsById.Take(2);
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

            var response = SUT.QueryAsync(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => DbClient.Serializer.Serialize(a.Albums)).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_Limit_to_2_using_json_array_Then_2_rows_are_returned()
        {
            var artists = ArtistsById.Take(2);
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

            var response = SUT.QueryAsync<string[]>(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums.Select(i => DbClient.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_Limit_to_2_using_entities_Then_2_rows_are_returned()
        {
            var artists = ArtistsById.Take(2);
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

            var response = SUT.QueryAsync<Album[]>(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_Key_is_specified_using_json_Then_the_matching_row_is_returned()
        {
            var artist = ArtistsById[2];
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.QueryAsync(query).Result;

            response.Should().BeSuccessfulGet(new[] { DbClient.Serializer.Serialize(artist.Albums) });
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_Key_is_specified_using_json_array_Then_the_matching_row_is_returned()
        {
            var artist = ArtistsById[2];
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.QueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(new[] { artist.Albums.Select(i => DbClient.Serializer.Serialize(i)).ToArray() });
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_Key_is_specified_using_entities_Then_the_matching_row_is_returned()
        {
            var artist = ArtistsById[2];
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.QueryAsync<Album[]>(query).Result;

            response.Should().BeSuccessfulGet(new[] { artist.Albums });
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_Keys_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

            var response = SUT.QueryAsync(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulPost(artists.OrderBy(a => a.ArtistId).Select(a => DbClient.Serializer.Serialize(a.Albums)).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_Keys_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

            var response = SUT.QueryAsync<string[]>(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulPost(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums.Select(i => DbClient.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_Keys_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

            var response = SUT.QueryAsync<Album[]>(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulPost(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_StartKey_and_EndKey_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.QueryAsync(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => DbClient.Serializer.Serialize(a.Albums)).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_StartKey_and_EndKey_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.QueryAsync<string[]>(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums.Select(i => DbClient.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_StartKey_and_EndKey_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.QueryAsync<Album[]>(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var response = SUT.QueryAsync(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Take(artists.Length - 1).Select(a => DbClient.Serializer.Serialize(a.Albums)).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var response = SUT.QueryAsync<string[]>(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Take(artists.Length - 1).Select(a => a.Albums.Select(i => DbClient.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var response = SUT.QueryAsync<Album[]>(query).Result;

            response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
            response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Take(artists.Length - 1).Select(a => a.Albums).ToArray());
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_skip_two_of_ten_It_should_return_the_other_eight()
        {
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId).Configure(cfg => cfg
                .Skip(2));

            var response = SUT.QueryAsync<Artist>(query).Result;

            response.Should().BeSuccessfulGet(8);
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_limit_is_two_of_ten_It_should_return_two()
        {
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId).Configure(cfg => cfg
                .Limit(2));

            var response = SUT.QueryAsync<Artist>(query).Result;

            response.Should().BeSuccessfulGet(2);
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_getting_all_artists_It_can_deserialize_artists_properly()
        {
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId);

            var response = SUT.QueryAsync<Artist>(query).Result;

            response.Should().BeSuccessfulGet(
                ArtistsById.OrderBy(a => a.ArtistId).ToArray(),
                i => i.Id);
        }

        [MyFact(TestScenarios.ViewsContext)]
        public void When_query_as_for_raw_It_shall_return_raw_result()
        {
            var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId);

            var response = SUT.QueryRawAsync(query).Result;

            response.Should().BeGetOfJson();
        }
    }
}