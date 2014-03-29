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
    public class MyCouchStoreTests : ClientTestsOf<MyCouchStore>
    {
        public MyCouchStoreTests()
        {
            SUT = new MyCouchStore(Client);
        }

        [Fact]
        public virtual void FlowTestOfJson()
        {
            var storeJson = SUT.StoreAsync(ClientTestData.Artists.Artist1Json);
            storeJson.Result.Id.Should().Be(ClientTestData.Artists.Artist1Id);
            storeJson.Result.Rev.Should().NotBeNullOrWhiteSpace();

            var getJsonById = SUT.GetByIdAsync(storeJson.Result.Id);
            getJsonById.Result.Should().NotBeNullOrWhiteSpace();

            var getJsonByIdAndRev = SUT.GetByIdAsync(storeJson.Result.Id, storeJson.Result.Rev);
            getJsonByIdAndRev.Result.Should().NotBeNullOrWhiteSpace();

            var deleteByIdAndRev = SUT.DeleteAsync(storeJson.Result.Id, storeJson.Result.Rev);
            deleteByIdAndRev.Result.Should().BeTrue();
        }

        [Fact]
        public virtual void FlowTestOfEntities()
        {
            var storeEntity = SUT.StoreAsync(ClientTestData.Artists.Artist2);

            storeEntity.Result.ArtistId.Should().Be(ClientTestData.Artists.Artist2Id);
            storeEntity.Result.ArtistRev.Should().NotBeNullOrWhiteSpace();

            var getEntityById = SUT.GetByIdAsync<Artist>(storeEntity.Result.ArtistId);
            getEntityById.Result.Should().NotBeNull();
            getEntityById.Result.ArtistId.Should().Be(ClientTestData.Artists.Artist2Id);
            getEntityById.Result.ArtistRev.Should().Be(storeEntity.Result.ArtistRev);

            var getEntityByIdAndRev = SUT.GetByIdAsync<Artist>(storeEntity.Result.ArtistId, storeEntity.Result.ArtistRev);
            getEntityByIdAndRev.Result.Should().NotBeNull();
            getEntityByIdAndRev.Result.ArtistId.Should().Be(ClientTestData.Artists.Artist2Id);
            getEntityByIdAndRev.Result.ArtistRev.Should().Be(storeEntity.Result.ArtistRev);

            var deleteByEntity = SUT.DeleteAsync(getEntityById.Result);
            deleteByEntity.Result.Should().BeTrue();
        }
    }

    public class ObservableQueryTests :
        ClientTestsOf<MyCouchStore>,
        IPreserveStatePerFixture,
        IUseFixture<ViewsFixture>
    {
        protected Artist[] ArtistsById { get; set; }

        public ObservableQueryTests()
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

            SUT.Query(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId, q => q.Reduce(true))
                .ForEachAsync((row, i) =>
                {
                    i.Should().Be(0);
                    row.Value.Should().Be(expectedSum.ToString(MyCouchRuntime.NumberFormat));
                })
                .ContinueWith(t => t.IsFaulted.Should().BeFalse());
        }

        [Fact]
        public void When_no_key_with_sum_reduce_for_dynamic_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());

            SUT.Query<dynamic>(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId, q => q.Reduce(true))
                .ForEachAsync((row, i) =>
                {
                    i.Should().Be(0);
                    ((long)row.Value).Should().Be(expectedSum);
                })
                .ContinueWith(t => t.IsFaulted.Should().BeFalse());
        }

        [Fact]
        public void When_no_key_with_sum_reduce_for_typed_response_It_will_be_able_to_sum()
        {
            var expectedSum = ArtistsById.Sum(a => a.Albums.Count());

            SUT.Query<int>(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId, q => q.Reduce(true))
                .ForEachAsync((row, i) =>
                {
                    i.Should().Be(0);
                    row.Value.Should().Be(expectedSum);
                })
                .ContinueWith(t => t.IsFaulted.Should().BeFalse());
        }

        [Fact]
        public void When_IncludeDocs_and_no_value_is_returned_for_string_response_Then_the_included_docs_are_extracted()
        {
            var len = 0;
            SUT.Query(ClientTestData.Views.ArtistsNameNoValueViewId, q => q.IncludeDocs(true))
                .ForEachAsync((row, i) =>
                {
                    len++;
                    ArtistsById[i].ShouldBe().ValueEqual(Client.Entities.Serializer.Deserialize<Artist>(row.IncludedDoc));
                })
                .ContinueWith(t => len.Should().Be(ArtistsById.Length));
        }

        [Fact]
        public void When_IncludeDocs_and_no_value_is_returned_for_entity_response_Then_the_included_docs_are_extracted()
        {
            var len = 0;
            SUT.Query<string, Artist>(ClientTestData.Views.ArtistsNameNoValueViewId, q => q.IncludeDocs(true))
                .ForEachAsync((row, i) =>
                {
                    len++;
                    ArtistsById[i].ShouldBe().ValueEqual(row.IncludedDoc);
                })
                .ContinueWith(t => len.Should().Be(ArtistsById.Length));
        }

        [Fact]
        public void When_IncludeDocs_of_non_array_doc_and_null_value_is_returned_Then_the_neither_included_docs_nor_value_is_extracted()
        {
            var len = 0;
            SUT.Query<string[], string[]>(ClientTestData.Views.ArtistsNameNoValueViewId, q => q.IncludeDocs(true))
                .ForEachAsync((row, i) =>
                {
                    len++;
                    row.Value.Should().BeNull();
                    row.IncludedDoc.Should().BeNull();
                })
                .ContinueWith(t => len.Should().Be(ArtistsById.Length));
        }

        [Fact]
        public void When_Skipping_2_of_10_using_json_Then_8_rows_are_returned()
        {
            const int skip = 2;
            var albums = ArtistsById.Skip(skip).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray();

            var values = SUT.Query(ClientTestData.Views.ArtistsAlbumsViewId, q => q.Skip(skip))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(albums);
        }

        [Fact]
        public void When_Skipping_2_of_10_using_json_array_Then_8_rows_are_returned()
        {
            const int skip = 2;
            var albums = ArtistsById.Skip(skip).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray();
            
            var values = SUT.Query<string[]>(ClientTestData.Views.ArtistsAlbumsViewId, q => q.Skip(skip))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(albums);
        }

        [Fact]
        public void When_Skipping_2_of_10_using_entities_Then_8_rows_are_returned()
        {
            const int skip = 2;
            var albums = ArtistsById.Skip(skip).Select(a => a.Albums).ToArray();

            var values = SUT.Query<Album[]>(ClientTestData.Views.ArtistsAlbumsViewId, q => q.Skip(skip))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(albums);
        }

        [Fact]
        public void When_Limit_to_2_using_json_Then_2_rows_are_returned()
        {
            const int limit = 2;
            var albums = ArtistsById.Take(limit).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray();

            var values = SUT.Query(ClientTestData.Views.ArtistsAlbumsViewId, q => q.Limit(limit))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(albums);
        }

        [Fact]
        public void When_Limit_to_2_using_json_array_Then_2_rows_are_returned()
        {
            const int limit = 2;
            var albums = ArtistsById.Take(limit).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray();

            var values = SUT.Query<string[]>(ClientTestData.Views.ArtistsAlbumsViewId, q => q.Limit(limit))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(albums);
        }

        [Fact]
        public void When_Limit_to_2_using_entities_Then_2_rows_are_returned()
        {
            const int limit = 2;
            var albums = ArtistsById.Take(limit).Select(a => a.Albums).ToArray();

            var values = SUT.Query<Album[]>(ClientTestData.Views.ArtistsAlbumsViewId, q => q.Limit(limit))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(albums);
        }

        [Fact]
        public void When_Key_is_specified_using_json_Then_the_matching_row_is_returned()
        {
            var artist = ArtistsById[2];

            var values = SUT.Query(ClientTestData.Views.ArtistsAlbumsViewId, q => q.Key(artist.Name))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(new[] {Client.Serializer.Serialize(artist.Albums)});
        }

        [Fact]
        public void When_Key_is_specified_using_json_array_Then_the_matching_row_is_returned()
        {
            var artist = ArtistsById[2];

            var values = SUT.Query<string[]>(ClientTestData.Views.ArtistsAlbumsViewId, q => q.Key(artist.Name))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(new[] { artist.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray() });
        }

        [Fact]
        public void When_Key_is_specified_using_entities_Then_the_matching_row_is_returned()
        {
            var artist = ArtistsById[2];

            var values = SUT.Query<Album[]>(ClientTestData.Views.ArtistsAlbumsViewId, q => q.Key(artist.Name))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(new[] { artist.Albums });
        }

        [Fact]
        public void When_Keys_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();

            var values = SUT.Query(ClientTestData.Views.ArtistsAlbumsViewId, q => q.Keys(keys))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [Fact]
        public void When_Keys_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();

            var values = SUT.Query<string[]>(ClientTestData.Views.ArtistsAlbumsViewId, q => q.Keys(keys))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [Fact]
        public void When_Keys_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();

            var values = SUT.Query<Album[]>(ClientTestData.Views.ArtistsAlbumsViewId, q => q.Keys(keys))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(artists.Select(a => a.Albums).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();

            var values = SUT.Query(
                ClientTestData.Views.ArtistsAlbumsViewId,
                q => q
                    .StartKey(artists.First().Name)
                    .EndKey(artists.Last().Name)
                )
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();

            var values = SUT.Query<string[]>(
                ClientTestData.Views.ArtistsAlbumsViewId,
                q => q
                    .StartKey(artists.First().Name)
                    .EndKey(artists.Last().Name)
                )
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();

            var values = SUT.Query<Album[]>(
                ClientTestData.Views.ArtistsAlbumsViewId,
                q => q
                    .StartKey(artists.First().Name)
                    .EndKey(artists.Last().Name)
                )
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(artists.Select(a => a.Albums).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();

            var values = SUT.Query(
                ClientTestData.Views.ArtistsAlbumsViewId,
                q => q
                    .StartKey(artists.First().Name)
                    .EndKey(artists.Last().Name)
                    .InclusiveEnd(false)
                )
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(artists.Take(artists.Length - 1).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();

            var values = SUT.Query<string[]>(
                ClientTestData.Views.ArtistsAlbumsViewId,
                q => q
                    .StartKey(artists.First().Name)
                    .EndKey(artists.Last().Name)
                    .InclusiveEnd(false)
                )
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(artists.Take(artists.Length - 1).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = ArtistsById.Skip(2).Take(5).ToArray();

            var values = SUT.Query<Album[]>(
                ClientTestData.Views.ArtistsAlbumsViewId,
                q => q
                    .StartKey(artists.First().Name)
                    .EndKey(artists.Last().Name)
                    .InclusiveEnd(false)
                )
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(artists.Take(artists.Length - 1).Select(a => a.Albums).ToArray());
        }

        [Fact]
        public void When_skip_two_of_ten_It_should_return_the_other_eight()
        {
            const int skip = 2;
            var artists = ArtistsById.Skip(skip).ToArray();

            var values = SUT.Query<Artist>(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId, q => q.Skip(skip))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(artists);
        }

        [Fact]
        public void When_limit_is_two_of_ten_It_should_return_the_two_first_artists()
        {
            const int limit = 2;
            var artists = ArtistsById.Take(limit).ToArray();

            var values = SUT.Query<Artist>(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId, q => q.Limit(limit))
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(artists);
        }

        [Fact]
        public void When_getting_all_artists_It_can_deserialize_artists_properly()
        {
            var values = SUT.Query<Artist>(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId)
                .ToRowList()
                .OrderBy(r => r.Id)
                .Select(r => r.Value)
                .ToArray();

            values.ShouldBe().ValueEqual(ArtistsById);
        }
    }
}