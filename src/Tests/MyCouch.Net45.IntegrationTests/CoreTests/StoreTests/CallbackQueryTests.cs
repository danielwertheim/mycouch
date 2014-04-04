using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;
using Xunit;

namespace MyCouch.IntegrationTests.CoreTests.StoreTests
{
    public class CallbackQueryTests :
        IntegrationTestsOf<MyCouchStore>,
        IPreserveStatePerFixture,
        IUseFixture<ViewsFixture>
    {
        protected Artist[] ArtistsById { get; set; }

        public CallbackQueryTests()
        {
            SUT = new MyCouchStore(Client);
        }

        public void SetFixture(ViewsFixture data)
        {
            data.Init(Environment);
            ArtistsById = data.Artists;
        }

        [Fact]
        public void When_no_key_with_sum_reduce_for_string_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());
            var query = new Query(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Configure(q => q.Reduce(true));
            var numOfRows = 0;

            SUT.QueryAsync(query,
                r =>
                {
                    numOfRows++;
                    r.Value.Should().Be(expectedSum.ToString(MyCouchRuntime.NumberFormat));
                })
                .ContinueWith(t =>
                {
                    t.IsFaulted.Should().BeFalse();
                    numOfRows.Should().Be(1);
                }).Wait();
        }

        [Fact]
        public void When_no_key_with_sum_reduce_for_dynamic_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());
            var query = new Query(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Configure(q => q.Reduce(true));
            var numOfRows = 0;

            SUT.QueryAsync<dynamic>(query,
                r =>
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

        [Fact]
        public void When_no_key_with_sum_reduce_for_typed_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());
            var query = new Query(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Configure(q => q.Reduce(true));
            var numOfRows = 0;

            SUT.QueryAsync<int>(query,
                r =>
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

        [Fact]
        public void When_IncludeDocs_and_no_value_is_returned_for_string_response_Then_the_included_docs_are_extracted()
        {
            int len = 0, i = 0;
            var query = new Query(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(q => q.IncludeDocs(true));

            SUT.QueryAsync(query,
                r =>
                {
                    len++;
                    ArtistsById[i].ShouldBe().ValueEqual(Client.Entities.Serializer.Deserialize<Artist>(r.IncludedDoc));
                    i++;
                })
                .ContinueWith(t =>
                {
                    t.IsFaulted.Should().BeFalse();
                    len.Should().Be(ArtistsById.Length);
                }).Wait();
        }

        [Fact]
        public void When_IncludeDocs_and_no_value_is_returned_for_entity_response_Then_the_included_docs_are_extracted()
        {
            int len = 0, i = 0;
            var query = new Query(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(q => q.IncludeDocs(true));

            SUT.QueryAsync<string, Artist>(query,
                r =>
                {
                    len++;
                    ArtistsById[i].ShouldBe().ValueEqual(r.IncludedDoc);
                    i++;
                })
                .ContinueWith(t =>
                {
                    t.IsFaulted.Should().BeFalse();
                    len.Should().Be(ArtistsById.Length);
                }).Wait();
        }

        [Fact]
        public void When_IncludeDocs_of_non_array_doc_and_null_value_is_returned_Then_the_neither_included_docs_nor_value_is_extracted()
        {
            var len = 0;
            var query = new Query(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(q => q.IncludeDocs(true));

            SUT.QueryAsync<string[], string[]>(query,
                r =>
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

        [Fact]
        public void When_Skipping_2_of_10_using_json_Then_8_rows_are_returned()
        {
            const int skip = 2;
            var albums = ArtistsById.Skip(skip).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Skip(skip));
            var rows = new List<Row>();
            
            SUT.QueryAsync(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(albums);
            }).Wait();
        }

        [Fact]
        public void When_Skipping_2_of_10_using_json_array_Then_8_rows_are_returned()
        {
            const int skip = 2;
            var albums = ArtistsById.Skip(skip).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Skip(skip));
            var rows = new List<Row<string[]>>();

            SUT.QueryAsync<string[]>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(albums);
            }).Wait();
        }

        [Fact]
        public void When_Skipping_2_of_10_using_entities_Then_8_rows_are_returned()
        {
            const int skip = 2;
            var albums = ArtistsById.Skip(skip).Select(a => a.Albums).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Skip(skip));
            var rows = new List<Row<Album[]>>();

            SUT.QueryAsync<Album[]>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(albums);
            }).Wait();
        }

        [Fact]
        public void When_Limit_to_2_using_json_Then_2_rows_are_returned()
        {
            const int limit = 2;
            var albums = ArtistsById.Take(limit).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Limit(limit));
            var rows = new List<Row>();

            SUT.QueryAsync(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(albums);
            }).Wait();
        }

        [Fact]
        public void When_Limit_to_2_using_json_array_Then_2_rows_are_returned()
        {
            const int limit = 2;
            var albums = ArtistsById.Take(limit).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Limit(limit));
            var rows = new List<Row<string[]>>();

            SUT.QueryAsync<string[]>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(albums);
            }).Wait();
        }

        [Fact]
        public void When_Limit_to_2_using_entities_Then_2_rows_are_returned()
        {
            const int limit = 2;
            var albums = ArtistsById.Take(limit).Select(a => a.Albums).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Limit(limit));
            var rows = new List<Row<Album[]>>();

            SUT.QueryAsync<Album[]>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(albums);
            }).Wait();
        }

        [Fact]
        public void When_Key_is_specified_using_json_Then_the_matching_row_is_returned()
        {
            var artist = ArtistsById[2];
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Key(artist.Name));
            var rows = new List<Row>();

            SUT.QueryAsync(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(new[] { Client.Serializer.Serialize(artist.Albums) });
            }).Wait();
        }

        [Fact]
        public void When_Key_is_specified_using_json_array_Then_the_matching_row_is_returned()
        {
            var artist = ArtistsById[2];
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Key(artist.Name));
            var rows = new List<Row<string[]>>();

            SUT.QueryAsync<string[]>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(new[] { artist.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray() });
            }).Wait();
        }

        [Fact]
        public void When_Key_is_specified_using_entities_Then_the_matching_row_is_returned()
        {
            var artist = ArtistsById[2];
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Key(artist.Name));
            var rows = new List<Row<Album[]>>();

            SUT.QueryAsync<Album[]>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(new[] { artist.Albums });
            }).Wait();
        }

        [Fact]
        public void When_Keys_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Keys(keys));
            var rows = new List<Row>();

            SUT.QueryAsync(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
            }).Wait();
        }

        [Fact]
        public void When_Keys_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Keys(keys));
            var rows = new List<Row<string[]>>();

            SUT.QueryAsync<string[]>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
            }).Wait();
        }

        [Fact]
        public void When_Keys_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q.Keys(keys));
            var rows = new List<Row<Album[]>>();

            SUT.QueryAsync<Album[]>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(artists.Select(a => a.Albums).ToArray());
            }).Wait();
        }

        [Fact]
        public void When_StartKey_and_EndKey_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));
            var rows = new List<Row>();

            SUT.QueryAsync(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
            }).Wait();
        }

        [Fact]
        public void When_StartKey_and_EndKey_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));
            var rows = new List<Row<string[]>>();

            SUT.QueryAsync<string[]>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
            }).Wait();
        }

        [Fact]
        public void When_StartKey_and_EndKey_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));
            var rows = new List<Row<Album[]>>();

            SUT.QueryAsync<Album[]>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(artists.Select(a => a.Albums).ToArray());
            }).Wait();
        }

        [Fact]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));
            var rows = new List<Row>();

            SUT.QueryAsync(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(artists.Take(artists.Length - 1).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
            }).Wait();
        }

        [Fact]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));
            var rows = new List<Row<string[]>>();

            SUT.QueryAsync<string[]>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(artists.Take(artists.Length - 1).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
            }).Wait();
        }

        [Fact]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsAlbumsViewId).Configure(q => q
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));
            var rows = new List<Row<Album[]>>();

            SUT.QueryAsync<Album[]>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(artists.Take(artists.Length - 1).Select(a => a.Albums).ToArray());
            }).Wait();
        }

        [Fact]
        public void When_skip_two_of_ten_It_should_return_the_other_eight()
        {
            const int skip = 2;
            var artists = ArtistsById.Skip(skip).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId).Configure(q => q
                .Skip(skip));
            var rows = new List<Row<Artist>>();

            SUT.QueryAsync<Artist>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(artists);
            }).Wait();
        }

        [Fact]
        public void When_limit_is_two_of_ten_It_should_return_the_two_first_artists()
        {
            const int limit = 2;
            var artists = ArtistsById.Take(limit).ToArray();
            var query = new Query(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId).Configure(q => q
                .Limit(limit));
            var rows = new List<Row<Artist>>();

            SUT.QueryAsync<Artist>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(artists);
            }).Wait();
        }

        [Fact]
        public void When_getting_all_artists_It_can_deserialize_artists_properly()
        {
            var query = new Query(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId);
            var rows = new List<Row<Artist>>();

            SUT.QueryAsync<Artist>(query, rows.Add).ContinueWith(t =>
            {
                t.IsFaulted.Should().BeFalse();

                var values = GetRowValues(rows);

                values.ShouldBe().ValueEqual(ArtistsById);
            }).Wait();
        }

        private string[] GetRowValues(IEnumerable<Row> rows)
        {
            return rows.OrderBy(r => r.Id).Select(r => r.Value).ToArray();
        }

        private TValue[] GetRowValues<TValue>(IEnumerable<Row<TValue>> rows)
        {
            return rows.OrderBy(r => r.Id).Select(r => r.Value).ToArray();
        }
    }
}