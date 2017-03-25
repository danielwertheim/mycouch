using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;
using Xunit;

namespace MyCouch.IntegrationTests.CoreTests
{
    public class MyCouchStoreQueryTests :
        IntegrationTestsOf<MyCouchStore>,
        IPreserveStatePerFixture,
        IClassFixture<ViewsFixture>
    {
        protected Artist[] ArtistsById { get; set; }

        public MyCouchStoreQueryTests(ViewsFixture data)
        {
            ArtistsById = data.Init(Environment);
            SUT = new MyCouchStore(DbClient);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetHeaders_When_getting_three_headers_It_returns_the_three_requested_headers()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var ids = artists.Select(a => a.ArtistId).ToArray();

            var headers = SUT.GetHeadersAsync(ids).Result;

            headers.ToArray().ShouldBeEquivalentTo(artists.Select(a => new DocumentHeader(a.ArtistId, a.ArtistRev)).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetHeadersAync_When_getting_one_existing_and_one_non_existing_header_It_returns_only_the_existing_one()
        {
            var artists = ArtistsById.Skip(2).Take(1).ToArray();
            var ids = artists.Select(a => a.ArtistId).ToList();
            ids.Add("16dcd30f8ba04f7a8702e30e9503f53f");

            var headers = SUT.GetHeadersAsync(ids.ToArray()).Result;

            headers.ToArray().ShouldBeEquivalentTo(artists.Select(a => new DocumentHeader(a.ArtistId, a.ArtistRev)).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetByIds_for_json_When_ids_are_specified_Then_matching_docs_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var ids = artists.Select(a => a.ArtistId).ToArray();

            var docs = SUT.GetByIdsAsync(ids).Result;

            docs.Select(d => DbClient.Entities.Serializer.Deserialize<Artist>(d)).ToArray().ShouldBeEquivalentTo(artists);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetByIds_for_entity_When_ids_are_specified_Then_matching_entities_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var ids = artists.Select(a => a.ArtistId).ToArray();

            var docs = SUT.GetByIdsAsync<Artist>(ids)
                .Result
                .ToArray();

            docs.ShouldBeEquivalentTo(artists);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetValueByKeys_for_json_When_keys_are_specified_Then_matching_docs_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToCouchKeys();

            var values = SUT.GetValueByKeysAsync(ClientTestData.Views.ArtistsAlbumsViewId, keys)
                .Result
                .ToArray();

            values.ShouldBeEquivalentTo(artists.Select(a => DbClient.Serializer.Serialize(a.Albums)).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetValueByKeys_for_entity_When_keys_are_specified_Then_matching_docs_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToCouchKeys();

            var values = SUT.GetValueByKeysAsync<Album[]>(ClientTestData.Views.ArtistsAlbumsViewId, keys)
                .Result
                .ToArray();

            values.ShouldBeEquivalentTo(artists.Select(a => a.Albums).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetIncludedDocByKeys_for_json_When_Ids_are_specified_Then_matching_docs_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.ArtistId).ToCouchKeys();

            var docs = SUT.GetIncludedDocByKeysAsync(SystemViewIdentity.AllDocs, keys).Result;

            docs.Select(d => DbClient.Entities.Serializer.Deserialize<Artist>(d)).ToArray().ShouldBeEquivalentTo(artists);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetIncludedDocByKeys_for_entity_When_Ids_are_specified_Then_matching_entities_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.ArtistId).ToCouchKeys();

            var docs = SUT.GetIncludedDocByKeysAsync<Artist>(SystemViewIdentity.AllDocs, keys)
                .Result
                .ToArray();

            docs.ShouldBeEquivalentTo(artists);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_no_key_with_sum_reduce_for_string_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());
            var query = new Query(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Configure(q => q.Reduce(true));

            var r = SUT.QueryAsync(query)
                .Result
                .ToArray();

            r.Length.Should().Be(1);
            r.Single().Value.Should().Be(expectedSum.ToString(MyCouchRuntime.FormatingCulture.NumberFormat));
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_no_key_with_sum_reduce_for_dynamic_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());
            var query = new Query(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Configure(q => q.Reduce(true));

            var r = SUT.QueryAsync<dynamic>(query)
                .Result
                .ToArray();

            r.Length.Should().Be(1);
            ((long)r.Single().Value).Should().Be(expectedSum);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_no_key_with_sum_reduce_for_typed_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());
            var query = new Query(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Configure(q => q.Reduce(true));

            var r = SUT.QueryAsync<int>(query)
                .Result
                .ToArray();

            r.Length.Should().Be(1);
            r.Single().Value.Should().Be(expectedSum);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_IncludeDocs_and_no_value_is_returned_for_string_response_Then_the_included_docs_are_extracted()
        {
            var query = new Query(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(q => q.IncludeDocs(true));

            var r = SUT.QueryAsync(query)
                .Result
                .ToArray();

            r.Length.Should().Be(ArtistsById.Length);
            r.Each((i, item) => ArtistsById[i].ShouldBeEquivalentTo(DbClient.Entities.Serializer.Deserialize<Artist>(item.IncludedDoc)));
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_IncludeDocs_and_no_value_is_returned_for_entity_response_Then_the_included_docs_are_extracted()
        {
            var query = new Query(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(q => q.IncludeDocs(true));

            var r = SUT.QueryAsync<string, Artist>(query)
                .Result
                .ToArray();

            r.Length.Should().Be(ArtistsById.Length);
            r.Each((i, item) => ArtistsById[i].ShouldBeEquivalentTo(item.IncludedDoc));
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_IncludeDocs_of_non_array_doc_and_null_value_is_returned_Then_the_neither_included_docs_nor_value_is_extracted()
        {
            var query = new Query(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(q => q.IncludeDocs(true));

            var r = SUT.QueryAsync<string[], string[]>(query)
                .Result
                .ToArray();

            r.Length.Should().Be(ArtistsById.Length);
            r.Each(item =>
            {
                item.Value.Should().BeNull();
                item.IncludedDoc.Should().BeNull();
            });
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Skipping_2_of_10_using_json_Then_8_rows_are_returned()
        {
            const int skip = 2;
            var albums = ArtistsById.Skip(skip).Select(a => DbClient.Serializer.Serialize(a.Albums)).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Skip(skip));

            var values = SUT.QueryAsync(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(albums);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Skipping_2_of_10_using_json_array_Then_8_rows_are_returned()
        {
            const int skip = 2;
            var albums = ArtistsById.Skip(skip).Select(a => a.Albums.Select(i => DbClient.Serializer.Serialize(i)).ToArray()).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Skip(skip));

            var values = SUT.QueryAsync<string[]>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(albums);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Skipping_2_of_10_using_entities_Then_8_rows_are_returned()
        {
            const int skip = 2;
            var albums = ArtistsById.Skip(skip).Select(a => a.Albums).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Skip(skip));

            var values = SUT.QueryAsync<Album[]>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(albums);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Limit_to_2_using_json_Then_2_rows_are_returned()
        {
            const int limit = 2;
            var albums = ArtistsById.Take(limit).Select(a => DbClient.Serializer.Serialize(a.Albums)).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Limit(limit));

            var values = SUT.QueryAsync(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(albums);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Limit_to_2_using_json_array_Then_2_rows_are_returned()
        {
            const int limit = 2;
            var albums = ArtistsById.Take(limit).Select(a => a.Albums.Select(i => DbClient.Serializer.Serialize(i)).ToArray()).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Limit(limit));

            var values = SUT.QueryAsync<string[]>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(albums);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Limit_to_2_using_entities_Then_2_rows_are_returned()
        {
            const int limit = 2;
            var albums = ArtistsById.Take(limit).Select(a => a.Albums).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Limit(limit));

            var values = SUT.QueryAsync<Album[]>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(albums);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Key_is_specified_using_json_Then_the_matching_row_is_returned()
        {
            var artist = ArtistsById[2];
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Key(artist.Name));

            var values = SUT.QueryAsync(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(new[] { DbClient.Serializer.Serialize(artist.Albums) });
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Key_is_specified_using_json_array_Then_the_matching_row_is_returned()
        {
            var artist = ArtistsById[2];
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Key(artist.Name));

            var values = SUT.QueryAsync<string[]>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(new[] { artist.Albums.Select(i => DbClient.Serializer.Serialize(i)).ToArray() });
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Key_is_specified_using_entities_Then_the_matching_row_is_returned()
        {
            var artist = ArtistsById[2];
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Key(artist.Name));

            var values = SUT.QueryAsync<Album[]>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(new[] { artist.Albums });
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Keys_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Keys(keys));

            var values = SUT.QueryAsync(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(artists.Select(a => DbClient.Serializer.Serialize(a.Albums)).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Keys_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Keys(keys));

            var values = SUT.QueryAsync<string[]>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(artists.Select(a => a.Albums.Select(i => DbClient.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Keys_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Keys(keys));

            var values = SUT.QueryAsync<Album[]>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(artists.Select(a => a.Albums).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_StartKey_and_EndKey_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var values = SUT.QueryAsync(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(artists.Select(a => DbClient.Serializer.Serialize(a.Albums)).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_StartKey_and_EndKey_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var values = SUT.QueryAsync<string[]>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(artists.Select(a => a.Albums.Select(i => DbClient.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_StartKey_and_EndKey_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var values = SUT.QueryAsync<Album[]>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(artists.Select(a => a.Albums).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var values = SUT.QueryAsync(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(artists.Take(artists.Length - 1).Select(a => DbClient.Serializer.Serialize(a.Albums)).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var values = SUT.QueryAsync<string[]>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(artists.Take(artists.Length - 1).Select(a => a.Albums.Select(i => DbClient.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var values = SUT.QueryAsync<Album[]>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(artists.Take(artists.Length - 1).Select(a => a.Albums).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_skip_two_of_ten_It_should_return_the_other_eight()
        {
            const int skip = 2;
            var artists = ArtistsById.Skip(skip).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId).Configure(q => q
                .Skip(skip));

            var values = SUT.QueryAsync<Artist>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(artists);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_limit_is_two_of_ten_It_should_return_the_two_first_artists()
        {
            const int limit = 2;
            var artists = ArtistsById.Take(limit).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId).Configure(q => q
                .Limit(limit));

            var values = SUT.QueryAsync<Artist>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(artists);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_getting_all_artists_It_can_deserialize_artists_properly()
        {
            var query = new Query(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId);

            var values = SUT.QueryAsync<Artist>(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(ArtistsById);
        }


        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Key_is_specified_using_custom_query_parameters_Then_the_matching_row_is_returned()
        {
            var artist = ArtistsById[2];
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId)
                .Configure(q => q.CustomQueryParameters(new Dictionary<string, object>
                {
                    ["key"] = $"\"{artist.Name}\""
                }));

            var values = SUT.QueryAsync(query)
                .Result
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(new[] { DbClient.Serializer.Serialize(artist.Albums) });
        }
    }
}