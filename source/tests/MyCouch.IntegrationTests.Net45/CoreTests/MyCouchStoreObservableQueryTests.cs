using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using FluentAssertions;
using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;
using Xunit;

namespace MyCouch.IntegrationTests.CoreTests
{
    public class MyCouchStoreObservableQueryTests :
        IntegrationTestsOf<MyCouchStore>,
        IPreserveStatePerFixture,
#if !PCL
        IUseFixture<ViewsFixture>
#else
        IClassFixture<ViewsFixture>
#endif
    {
        protected Artist[] ArtistsById { get; set; }

#if !PCL
        public MyCouchStoreObservableQueryTests()
        {
            SUT = new MyCouchStore(DbClient);
        }

        public void SetFixture(ViewsFixture data)
        {
            ArtistsById = data.Init(Environment);
        }
#else
        public MyCouchStoreObservableQueryTests(ViewsFixture fixture)
        {
            SUT = new MyCouchStore(DbClient);
            ArtistsById = fixture.Init(Environment);
        }
#endif
        [MyFact(TestScenarios.MyCouchStore)]
        public void GetHeaders_When_getting_three_headers_It_returns_the_three_requested_headers()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var ids = artists.Select(a => a.ArtistId).ToArray();

            var headers = SUT.GetHeaders(ids).ToEnumerable();

            headers.ToArray().ShouldBeEquivalentTo(artists.Select(a => new DocumentHeader(a.ArtistId, a.ArtistRev)).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetByIds_for_json_When_ids_are_specified_Then_matching_docs_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var ids = artists.Select(a => a.ArtistId).ToArray();

            var docs = SUT.GetByIds(ids).ToEnumerable();

            docs.Select(d => DbClient.Entities.Serializer.Deserialize<Artist>(d)).ToArray().ShouldBeEquivalentTo(artists);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetByIds_for_entity_When_ids_are_specified_Then_matching_entities_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var ids = artists.Select(a => a.ArtistId).ToArray();

            var docs = SUT.GetByIds<Artist>(ids).ToEnumerable().ToArray();

            docs.ShouldBeEquivalentTo(artists);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetValueByKeys_for_json_When_keys_are_specified_Then_matching_docs_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToCouchKeys();

            var values = SUT.GetValueByKeys(ClientTestData.Views.ArtistsAlbumsViewId, keys)
                .ToEnumerable()
                .ToArray();

            values.ShouldBeEquivalentTo(artists.Select(a => DbClient.Serializer.Serialize(a.Albums)).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetValueByKeys_for_entity_When_keys_are_specified_Then_matching_docs_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToCouchKeys();

            var values = SUT.GetValueByKeys<Album[]>(ClientTestData.Views.ArtistsAlbumsViewId, keys)
                .ToEnumerable()
                .ToArray();

            values.ShouldBeEquivalentTo(artists.Select(a => a.Albums).ToArray());
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetIncludedDocByKeys_for_json_When_Ids_are_specified_Then_matching_docs_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.ArtistId).ToCouchKeys();

            var docs = SUT.GetIncludedDocByKeys(SystemViewIdentity.AllDocs, keys).ToEnumerable();

            docs.Select(d => DbClient.Entities.Serializer.Deserialize<Artist>(d)).ToArray().ShouldBeEquivalentTo(artists);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void GetIncludedDocByKeys_for_entity_When_Ids_are_specified_Then_matching_entities_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.ArtistId).ToCouchKeys();

            var docs = SUT.GetIncludedDocByKeys<Artist>(SystemViewIdentity.AllDocs, keys).ToEnumerable().ToArray();

            docs.ShouldBeEquivalentTo(artists);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_no_key_with_sum_reduce_for_string_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());
            var query = new Query(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Configure(q => q.Reduce(true));
            var numOfRows = 0;

            SUT.Query(query)
                .ForEachAsync((r, i) =>
                {
                    numOfRows++;
                    r.Value.Should().Be(expectedSum.ToString(MyCouchRuntime.FormatingCulture.NumberFormat));
                })
                .ContinueWith(t =>
                {
                    t.IsFaulted.Should().BeFalse();
                    numOfRows.Should().Be(1);
                }).Wait();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_no_key_with_sum_reduce_for_dynamic_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());
            var query = new Query(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Configure(q => q.Reduce(true));
            var numOfRows = 0;

            SUT.Query<dynamic>(query)
                .ForEachAsync((r, i) =>
                {
                    numOfRows++;
                    ((long)r.Value).Should().Be(expectedSum);
                })
                .ContinueWith(t =>
                {
                    t.IsFaulted.Should().BeFalse();
                    numOfRows.Should().Be(1);
                }).Wait();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_no_key_with_sum_reduce_for_typed_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());
            var query = new Query(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Configure(q => q.Reduce(true));
            var numOfRows = 0;

            SUT.Query<int>(query)
                .ForEachAsync((r, i) =>
                {
                    numOfRows++;
                    r.Value.Should().Be(expectedSum);
                })
                .ContinueWith(t =>
                {
                    t.IsFaulted.Should().BeFalse();
                    numOfRows.Should().Be(1);
                }).Wait();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_IncludeDocs_and_no_value_is_returned_for_string_response_Then_the_included_docs_are_extracted()
        {
            var len = 0;
            var query = new Query(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(q => q.IncludeDocs(true));

            SUT.Query(query)
                .ForEachAsync((r, i) =>
                {
                    len++;
                    ArtistsById[i].ShouldBeEquivalentTo(DbClient.Entities.Serializer.Deserialize<Artist>(r.IncludedDoc));
                })
                .ContinueWith(t =>
                {
                    t.IsFaulted.Should().BeFalse();
                    len.Should().Be(ArtistsById.Length);
                }).Wait();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_IncludeDocs_and_no_value_is_returned_for_entity_response_Then_the_included_docs_are_extracted()
        {
            var len = 0;
            var query = new Query(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(q => q.IncludeDocs(true));

            SUT.Query<string, Artist>(query)
                .ForEachAsync((r, i) =>
                {
                    len++;
                    ArtistsById[i].ShouldBeEquivalentTo(r.IncludedDoc);
                })
                .ContinueWith(t =>
                {
                    t.IsFaulted.Should().BeFalse();
                    len.Should().Be(ArtistsById.Length);
                }).Wait();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_IncludeDocs_of_non_array_doc_and_null_value_is_returned_Then_the_neither_included_docs_nor_value_is_extracted()
        {
            var len = 0;
            var query = new Query(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(q => q.IncludeDocs(true));

            SUT.Query<string[], string[]>(query)
                .ForEachAsync((r, i) =>
                {
                    len++;
                    r.Value.Should().BeNull();
                    r.IncludedDoc.Should().BeNull();
                })
                .ContinueWith(t =>
                {
                    t.IsFaulted.Should().BeFalse();
                    len.Should().Be(ArtistsById.Length);
                }).Wait();
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_Skipping_2_of_10_using_json_Then_8_rows_are_returned()
        {
            const int skip = 2;
            var albums = ArtistsById.Skip(skip).Select(a => DbClient.Serializer.Serialize(a.Albums)).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Skip(skip));
            
            var values = SUT.Query(query)
                .ToRowList()
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

            var values = SUT.Query<string[]>(query)
                .ToRowList()
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

            var values = SUT.Query<Album[]>(query)
                .ToRowList()
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

            var values = SUT.Query(query)
                .ToRowList()
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
            
            var values = SUT.Query<string[]>(query)
                .ToRowList()
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

            var values = SUT.Query<Album[]>(query)
                .ToRowList()
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

            var values = SUT.Query(query)
                .ToRowList()
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

            var values = SUT.Query<string[]>(query)
                .ToRowList()
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

            var values = SUT.Query<Album[]>(query)
                .ToRowList()
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

            var values = SUT.Query(query)
                .ToRowList()
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

            var values = SUT.Query<string[]>(query)
                .ToRowList()
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

            var values = SUT.Query<Album[]>(query)
                .ToRowList()
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

            var values = SUT.Query(query)
                .ToRowList()
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

            var values = SUT.Query<string[]>(query)
                .ToRowList()
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

            var values = SUT.Query<Album[]>(query)
                .ToRowList()
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

            var values = SUT.Query(query)
                .ToRowList()
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

            var values = SUT.Query<string[]>(query)
                .ToRowList()
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

            var values = SUT.Query<Album[]>(query)
                .ToRowList()
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

            var values = SUT.Query<Artist>(query)
                .ToRowList()
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

            var values = SUT.Query<Artist>(query)
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(artists);
        }

        [MyFact(TestScenarios.MyCouchStore)]
        public void Query_When_getting_all_artists_It_can_deserialize_artists_properly()
        {
            var query = new Query(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId);

            var values = SUT.Query<Artist>(query)
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBeEquivalentTo(ArtistsById);
        }
    }
}